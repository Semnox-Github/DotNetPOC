/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for Cart Screen
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.1.0   27-Dec-2022   Vignesh Bhat             Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;

namespace Parafait_Kiosk
{
    public partial class usrCtrlCart : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string AMOUNTFORMAT;
        private string AMOUNTFORMATWITHCURRENCYSYMBOL;
        private ExecutionContext executionContext;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines;
        private bool readOnlyMode = true;
        public delegate void cancelSelectedLines(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines);
        public cancelSelectedLines CancelSelectedLines;
        public delegate void refreshCartData();
        public refreshCartData RefreshCartData;
        internal class LineData
        {
            internal List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>();
            internal string offset;
            internal string productName;
            internal string lineType;
            internal int lineId;
            internal double price = -874833.233;
            internal decimal quantity = 0;
            internal double lineAmount = 0;
            internal double taxAmount = 0;
            internal int trxProfileId = -1;
            internal bool receiptPrinted = false;
            internal bool kdsSent = false;
            internal bool cancelled = false;
            internal Semnox.Parafait.Transaction.Transaction.TransactionLine parentLine;
            internal LineData(string offset, string productName,
                string lineType, int lineId, double price, decimal quantity, double lineAmount, double taxAmount, int trxProfileId,
                 bool receiptPrinted, bool kdsSent, bool cancelled,
                 Semnox.Parafait.Transaction.Transaction.TransactionLine parentLine)
            {
                this.offset = offset;
                this.productName = productName;
                this.lineType = lineType;
                this.lineId = lineId;
                this.price = price;
                this.quantity = quantity;
                this.lineAmount = lineAmount;
                this.taxAmount = taxAmount;
                this.trxProfileId = trxProfileId;
                this.receiptPrinted = receiptPrinted;
                this.kdsSent = kdsSent;
                this.cancelled = cancelled;
                this.parentLine = parentLine;
            }
        }
        private List<LineData> lineDataList = new List<LineData>();
        public usrCtrlCart(ExecutionContext executionContext, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxList, bool readOnlyMode = true)
        {
            log.LogMethodEntry(executionContext, trxList, readOnlyMode);
            this.executionContext = executionContext;
            this.trxLines = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>(trxList);
            this.readOnlyMode = readOnlyMode;
            InitializeComponent();
            SetCustomizedFontColors();
            AMOUNTFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            AMOUNTFORMATWITHCURRENCYSYMBOL = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            if (trxList == null || trxList.Any() == false)
            {
                //No Records to process
                string msg = MessageContainerList.GetMessage(executionContext, 16041);
                ValidationException validationException = new ValidationException(msg);
                log.Error(validationException);
                throw validationException;
            }
            this.SuspendLayout();
            BuildTrxLineDisplayElements();
            int height = GetFlpCartProductHeight();
            flpCartProducts.Size = new Size(flpCartProducts.Width, height);
            //this.panelButtons.Location = new Point(this.panelButtons.Location.X, this.flpCartProducts.Height + 1);
            int heightValue = this.flpCartProducts.Height + 5;// this.panelButtons.Height + 5;
            this.Size = new Size(this.Width, heightValue);
            this.ResumeLayout(true);
        }

        private int GetFlpCartProductHeight()
        {
            log.LogMethodEntry();
            int height = 93;
            for (int i = flpCartProducts.Controls.Count-1; i >=0; i--)
            {
                height = flpCartProducts.Controls[i].Location.Y + flpCartProducts.Controls[i].Height;
                break;
            }
            log.LogMethodExit(height);
            return height;
        } 
        public void btnDelete_Clicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (CancelSelectedLines != null)
                {
                    CancelSelectedLines(this.trxLines);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while cancelling lines: " + ex.Message);
            }
            DoRefreshCartData();
            log.LogMethodExit();
        }

        private void DoRefreshCartData()
        {
            log.LogMethodEntry();
            try
            {
                if (RefreshCartData != null)
                {
                    RefreshCartData();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while Refresh Cart Data: " + ex.Message);
            } 
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            { 
                this.BackgroundImage = ThemeManager.CurrentThemeImages.CartItemBackgroundImageSmall;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                //KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void BuildTrxLineDisplayElements()
        {
            log.LogMethodEntry();
            this.flpCartProducts.Controls.Clear();
            try
            {
                this.flpCartProducts.SuspendLayout();
                for (int i = 0; i < trxLines.Count; i++)
                {
                    if (trxLines[i].ProductTypeCode == "LOYALTY")
                        trxLines[i].LineProcessed = true;
                    else
                        trxLines[i].LineProcessed = false;
                }

                int rowcount = 0;
                int totalLineCount = trxLines.Count;
                for (int i = 0; i < trxLines.Count; i++) // display card lines
                {
                    if (trxLines[i].LineValid && !trxLines[i].LineProcessed 
                        && trxLines[i].CardNumber != null
                        && ((trxLines[i].ProductTypeCode != ProductTypeValues.SERVICECHARGE
                        && trxLines[i].ProductTypeCode != ProductTypeValues.GRATUITY
                        && trxLines[i].ProductTypeCode != ProductTypeValues.DEPOSIT
                        && trxLines[i].ProductTypeCode != ProductTypeValues.LOCKERDEPOSIT
                        && trxLines[i].ProductTypeCode != ProductTypeValues.CARDDEPOSIT) || totalLineCount == 1))
                    {
                        //dataGridViewTransaction.Rows.Add();
                        string cardnumber = trxLines[i].CardNumber;
                        int productId = trxLines[i].ProductID;
                        if (cardnumber != null)
                        { 

                            rowcount++;
                            decimal prodQty = 0;
                            decimal prodDepositQty = 0;
                            double prodPrice = 0;
                            double prodDepositPrice = 0;
                            double prodDepositTaxAmount = 0;
                            double prodDepositLineAmount = 0;
                            double totalDepositLineAmount = 0;
                            double totalDepositTaxAmount = 0;
                            double prodTaxAmount = 0;
                            double prodLineAmount = 0;
                            double totalLineAmount = 0;
                            double totalTaxAmount = 0;
                            string prodDescription = string.Empty;
                            for (int j = 0; j < trxLines.Count; j++)
                            {
                                //if (trxLines[i] == trxLines[j])
                                //{
                                //    continue;//skip same line
                                //}
                                if (cardnumber == trxLines[j].CardNumber
                                    && (productId == trxLines[j].ProductID
                                          || (trxLines[j].ProductTypeCode == ProductTypeValues.CARDDEPOSIT
                                              || trxLines[j].ProductTypeCode == ProductTypeValues.DEPOSIT
                                              || trxLines[j].ProductTypeCode == ProductTypeValues.LOCKERDEPOSIT))
                                     && trxLines[j].LineValid && trxLines[j].LineProcessed == false
                                     && trxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE && trxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY)
                                { 
                                    if (trxLines[j].ProductID == productId)
                                    {
                                        if (trxLines[j].ProductTypeCode == ProductTypeValues.ATTRACTION)
                                        {
                                            prodDescription = trxLines[j].ProductName + (string.IsNullOrEmpty(trxLines[j].AttractionDetails) ? "" : "-" + trxLines[j].AttractionDetails) + (string.IsNullOrEmpty(trxLines[j].Remarks) ? "" : "-" + trxLines[j].Remarks);
                                        }
                                        else if (trxLines[j].ProductTypeCode == ProductTypeValues.LOCKER)
                                        {
                                            prodDescription = trxLines[j].ProductName + "-Locker:" + trxLines[j].LockerName + (string.IsNullOrEmpty(trxLines[j].Remarks) ? "" : "-" + trxLines[j].Remarks);
                                        }
                                        else
                                        {
                                            prodDescription = trxLines[j].ProductName + (string.IsNullOrEmpty(trxLines[j].Remarks) ? "" : "-" + trxLines[j].Remarks);
                                        }
                                    }

                                    if (trxLines[j].ProductTypeCode == ProductTypeValues.CARDDEPOSIT
                                        || trxLines[j].ProductTypeCode == ProductTypeValues.DEPOSIT
                                        || trxLines[j].ProductTypeCode == ProductTypeValues.LOCKERDEPOSIT)
                                    {
                                        prodDepositQty = prodDepositQty + trxLines[j].quantity;
                                        prodDepositPrice = trxLines[j].Price;
                                        prodDepositTaxAmount = trxLines[j].tax_amount;
                                        prodDepositLineAmount = trxLines[j].LineAmount;
                                        totalDepositLineAmount = totalDepositLineAmount + trxLines[j].LineAmount;
                                        totalDepositTaxAmount = totalDepositTaxAmount + trxLines[j].tax_amount;
                                    }
                                    else
                                    {
                                        prodQty = prodQty + trxLines[j].quantity;
                                        prodPrice = trxLines[j].Price; 
                                        prodTaxAmount = trxLines[j].tax_amount;
                                        prodLineAmount = trxLines[j].LineAmount;
                                        totalLineAmount = totalLineAmount + trxLines[j].LineAmount;
                                        totalTaxAmount = totalTaxAmount + trxLines[j].tax_amount;
                                    } 
                                    trxLines[j].LineProcessed = true;
                                }
                                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.SiteId, productId);
                                if (productsContainerDTO.CardCount > 1) //handle multicardinsingleproduct
                                {
                                    for (int k = 0; k < trxLines.Count; k++)
                                    {
                                        if (cardnumber != trxLines[k].CardNumber
                                            && productId != trxLines[k].ProductID
                                            && trxLines[k].ParentLine == trxLines[i]
                                            && trxLines[k].LineValid && trxLines[k].LineProcessed == false
                                            && (trxLines[k].ProductTypeCode == ProductTypeValues.DEPOSIT
                                                || trxLines[k].ProductTypeCode == ProductTypeValues.LOCKERDEPOSIT
                                                || trxLines[k].ProductTypeCode == ProductTypeValues.CARDDEPOSIT))
                                        {
                                            prodDepositQty = prodDepositQty + trxLines[k].quantity;
                                            prodDepositPrice = trxLines[k].Price;
                                            prodDepositTaxAmount = trxLines[k].tax_amount;
                                            prodDepositLineAmount = trxLines[k].LineAmount;
                                            totalDepositLineAmount = totalDepositLineAmount + trxLines[k].LineAmount;
                                            totalDepositTaxAmount = totalDepositTaxAmount + trxLines[k].tax_amount;
                                            trxLines[k].LineProcessed = true; 
                                        }
                                    }
                                }
                            }
                            Panel pnlProduct = BuildCardProductPanel(trxLines[i], prodDescription, prodQty, prodPrice, prodTaxAmount, prodLineAmount,
                                                     totalLineAmount, totalTaxAmount, prodDepositQty, prodDepositPrice, prodDepositTaxAmount,
                                                     prodDepositLineAmount, totalDepositLineAmount, totalDepositTaxAmount);
                            this.flpCartProducts.Controls.Add(pnlProduct);
                        }
                    }
                }

                for (int i = 0; i < trxLines.Count; i++) // display non-card trx lines
                {
                    if (trxLines[i].LineValid && !trxLines[i].LineProcessed
                        && trxLines[i].CardNumber == null
                        && trxLines[i].ProductTypeCode != ProductTypeValues.SERVICECHARGE && trxLines[i].ProductTypeCode != ProductTypeValues.GRATUITY)
                    { 

                        for (int j = i; j < trxLines.Count; j++)
                        {
                            if (trxLines[j].CardNumber == null && trxLines[j].LineValid && !trxLines[j].LineProcessed
                                && trxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE && trxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY)
                            {
                                if (trxLines[j].ParentLine != null && trxLines.Equals(trxLines[j].ParentLine))
                                {
                                    var index = trxLines.FindIndex(a => a == trxLines[j].ParentLine);

                                    if (index > -1 && index != j)
                                    {
                                        GenerateNonCardLineData(index);
                                    }
                                }
                                else
                                {
                                    GenerateNonCardLineData(j);
                                } 
                            }
                        }
                    }
                }
                BuildNonCardProductPanels();
            }
            finally
            {
                this.flpCartProducts.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private void BuildNonCardProductPanels()
        {
            log.LogMethodEntry();
            if (lineDataList != null && lineDataList.Any())
            {
                for (int i = 0; i < lineDataList.Count; i++)
                {
                    Panel nonCardPanel = BuildNonCardProductPanel(lineDataList[i], i);
                    this.flpCartProducts.Controls.Add(nonCardPanel);
                }
                if (lineDataList.Count > 3)
                {
                    this.BackgroundImage = ThemeManager.CurrentThemeImages.CartItemBackgroundImage;
                }
            }
            log.LogMethodExit();
        }

        private Panel BuildCardProductPanel(Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine, string prodDescription, decimal prodQty,
            double prodPrice, double prodTaxAmount, double prodLineAmount, double totalLineAmount, double totalTaxAmount, decimal prodDepositQty,
            double prodDepositPrice, double prodDepositTaxAmount, double prodDepositLineAmount, double totalDepositLineAmount, double totalDepositTaxAmount)
        {
            log.LogMethodEntry(trxLine);
            Panel pnlProduct = new Panel();
            double totalPrice = totalLineAmount;
            string spacePadding = "  ";
            Label lblProductDescription = BuildProdDescriptionLabel();
            if (prodDepositQty > 1)
            {
                lblProductDescription.Text = spacePadding + prodQty + " X " + prodDescription
                                              + " [ Split into "+ (prodDepositQty + prodQty) + " ]"; 
            }
            else
            {
                lblProductDescription.Text = spacePadding +(prodQty > 0? prodQty: prodDepositQty) + " X " + prodDescription;// + " + " + points + " Points";
            }  
            int height = 56; 
            Label lblTotalPrice = BuildTotalPriceLabel();
            lblTotalPrice.Text = (totalDepositLineAmount +totalPrice).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            Button btnDeleteItem = BuildDeleteButton();
            if (readOnlyMode == false)
            {
                pnlProduct.Controls.Add(btnDeleteItem);
            }
            else
            {
                lblTotalPrice.Size = new Size(lblTotalPrice.Width + 47, lblTotalPrice.Height);
            }
            pnlProduct.Controls.Add(lblProductDescription);
            pnlProduct.Controls.Add(lblTotalPrice);
            pnlProduct.BackColor = System.Drawing.Color.Transparent; 
            pnlProduct.Location = new System.Drawing.Point(3, 3);
            pnlProduct.Name = "pnlCartProducts";
            pnlProduct.Size = new System.Drawing.Size(822, height);
            pnlProduct.TabIndex = 0;
            log.LogMethodExit();
            return pnlProduct;
        }
        private bool HasCardDepositLine(Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry("trxLine");
            bool hasDepositLine = false;
            if (trxLine.card != null && this.trxLines.Exists(tl => tl.card != null && tl.card.CardNumber == trxLine.card.CardNumber
                                                                    && tl.LineValid && tl.ProductTypeCode == ProductTypeValues.CARDDEPOSIT))
            {
                hasDepositLine = true;
            }
            log.LogMethodExit(hasDepositLine);
            return hasDepositLine;
        }

        private Button BuildDeleteButton()
        {
            log.LogMethodEntry();
            Button btnDeleteItem = new Button();
            btnDeleteItem.BackColor = System.Drawing.Color.Transparent;
            btnDeleteItem.BackgroundImage = ThemeManager.CurrentThemeImages.CartDeleteButtonBackgroundImage;
            btnDeleteItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnDeleteItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnDeleteItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            btnDeleteItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Microsoft Sans Serif");
            btnDeleteItem.Font = new System.Drawing.Font(fontFamName, 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnDeleteItem.ForeColor = System.Drawing.Color.Transparent;
            btnDeleteItem.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            btnDeleteItem.Location = new System.Drawing.Point(770, 3);
            btnDeleteItem.Margin = new System.Windows.Forms.Padding(0);
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.Size = new System.Drawing.Size(47, 47);
            btnDeleteItem.TabIndex = 1;
            btnDeleteItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            btnDeleteItem.UseVisualStyleBackColor = false;
            btnDeleteItem.Click += new System.EventHandler(this.btnDelete_Clicked); 
            log.LogMethodExit();
            return btnDeleteItem;
        }
        private Label BuildTotalPriceLabel()
        {
            log.LogMethodEntry();
            Label lblTotalPrice = new Label();
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Microsoft Sans Serif");
            lblTotalPrice.Font = new System.Drawing.Font(fontFamName, 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //lblTotalPrice.Location = new System.Drawing.Point(565, 48);
            lblTotalPrice.Location = new System.Drawing.Point(555, 3);
            lblTotalPrice.Name = "lblTotalPrice";
            //lblTotalPrice.Size = new System.Drawing.Size(218, 45);
            lblTotalPrice.Size = new System.Drawing.Size(214, 47);
            lblTotalPrice.ForeColor = KioskStatic.CurrentTheme.UsrCtrlCartLblTotalPriceTextForeColor;
            lblTotalPrice.TabIndex = 8;
            lblTotalPrice.Text = "$80.00";
            lblTotalPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //lblTotalPrice.BorderStyle = BorderStyle.FixedSingle;
            log.LogMethodExit();
            return lblTotalPrice;
        }  
        private Label BuildProdDescriptionLabel()
        {
            log.LogMethodEntry();
            Label lblProductDescription = new Label();
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Microsoft Sans Serif");
            lblProductDescription.Font = new System.Drawing.Font(fontFamName, 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //lblProductDescription.Location = new System.Drawing.Point(5, 5);
            lblProductDescription.Location = new System.Drawing.Point(1, 3);
            lblProductDescription.Name = "lblProductDescription";
            lblProductDescription.Size = new System.Drawing.Size(554, 47);
            //lblProductDescription.Size = new System.Drawing.Size(780, 47);
            lblProductDescription.ForeColor = KioskStatic.CurrentTheme.UsrCtrlCartLblProductDescriptionTextForeColor;
            lblProductDescription.TabIndex = 5;
            lblProductDescription.Text = "1 X 1$ Card + 10 ";
            this.lblProductDescription.Name = "lblProductDescription"; 
            lblProductDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //lblProductDescription.BorderStyle = BorderStyle.FixedSingle;
            log.LogMethodExit();
            return lblProductDescription;
        }

        private void GenerateNonCardLineData(int j)
        {
            log.LogMethodEntry(j);
            bool addtoExistingLine = false;
            int targetRow = 0;
            decimal existingQuantity = 0;
            double existingAmount = 0;
            double existingTaxAmount = 0;
            //List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines = null;
            string productName = trxLines[j].ProductName + (string.IsNullOrEmpty(trxLines[j].AttractionDetails) ? "" : "-" + trxLines[j].AttractionDetails) + (string.IsNullOrEmpty(trxLines[j].Remarks) ? "" : "-" + trxLines[j].Remarks);
            int offset = 1;
            string sOffset = "";
            Semnox.Parafait.Transaction.Transaction.TransactionLine tl = trxLines[j].ParentLine;
            if (tl != null && tl != trxLines[j])
            {
                while (tl.ParentLine != null && tl != tl.ParentLine)
                {
                    tl = tl.ParentLine;
                    offset++;
                }
                byte[] b = new byte[] { 20, 37 };
                sOffset = Encoding.Unicode.GetString(b); 
                sOffset = sOffset.PadLeft(offset * 3 + 1, ' ') + " ";
            } 
            bool hasChildLines = false;

            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlChild in trxLines)
            {
                if (tlChild.LineValid && trxLines[j].Equals(tlChild.ParentLine))
                {
                    hasChildLines = true;
                    break;
                }
            }
            if (hasChildLines)
            {
                if (trxLines[j].ProductTypeCode == ProductTypeValues.COMBO)
                {
                    int displayProdCOunt = lineDataList.Count;
                    for (int row = 0; row < displayProdCOunt; row++)
                    {
                        if (productName.Equals(lineDataList[row].productName == null ? "" : lineDataList[row].productName)
                            && trxLines[j].ProductTypeCode.Equals(lineDataList[row].lineType == null ? "" : lineDataList[row].lineType)
                            && (lineDataList[row].lineType == null ? "" : lineDataList[row].lineType) != ProductTypeValues.COMBO
                            && trxLines[j].Price == lineDataList[row].price
                            && trxLines[j].ReceiptPrinted == lineDataList[row].receiptPrinted
                            && trxLines[j].KDSSent == lineDataList[row].kdsSent
                            && trxLines[j].CancelledLine == lineDataList[row].cancelled)
                        {
                            targetRow = row;
                            addtoExistingLine = true;
                            existingAmount = lineDataList[targetRow].lineAmount;
                            existingTaxAmount = lineDataList[targetRow].taxAmount;
                            existingQuantity = lineDataList[targetRow].quantity;
                            break;
                        }
                    }
                } 
            }
            else
            {
                if (trxLines[j].ProductTypeCode == ProductTypeValues.MANUAL
                    || trxLines[j].ProductTypeCode == ProductTypeValues.DEPOSIT
                    || trxLines[j].ProductTypeCode == ProductTypeValues.RENTAL
                    || trxLines[j].ProductTypeCode == ProductTypeValues.RENTALRETURN)
                {
                    for (int row = 0; row < lineDataList.Count; row++)
                    {
                        bool isallowed = false;
                        if (trxLines[j].ParentLine != null && lineDataList[row].parentLine != null)
                        {
                            if (trxLines[j].ParentLine.ProductTypeCode == ProductTypeValues.COMBO)
                            { 
                                if (trxLines[j].ParentLine.Equals(lineDataList[row].parentLine))
                                    isallowed = true;
                            }
                            else
                            {
                                if (trxLines[j].ParentLine == lineDataList[row].parentLine)
                                    isallowed = true;
                            }
                        }
                        else
                        {
                            if (productName.Equals(lineDataList[row].productName == null ? "" : lineDataList[row].productName))
                            {
                                if (trxLines == null)
                                    trxLines = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>();
                                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> tempTrxLines = trxLines.FindAll(x => x.LineValid == true && x.ParentLine != null && x.ParentLine == trxLines[lineDataList[row].lineId]);
                                if (tempTrxLines.Count <= 0)
                                    isallowed = true;
                            }
                        }
                        if (productName.Equals(lineDataList[row].productName == null ? "" : lineDataList[row].productName)
                            && trxLines[j].ProductTypeCode.Equals(lineDataList[row].lineType == null ? "" : lineDataList[row].lineType)
                            && trxLines[j].Price == lineDataList[row].price
                            && trxLines[j].ReceiptPrinted == lineDataList[row].receiptPrinted
                            && trxLines[j].KDSSent == lineDataList[row].kdsSent
                            && trxLines[j].CancelledLine == lineDataList[row].cancelled
                            && trxLines[j].TrxProfileId.Equals(lineDataList[row].trxProfileId)
                            && isallowed)
                        {
                            targetRow = row;
                            addtoExistingLine = true;
                            existingAmount = lineDataList[targetRow].lineAmount;
                            existingTaxAmount = lineDataList[targetRow].taxAmount;
                            existingQuantity = lineDataList[targetRow].quantity;
                            break;
                        }
                    }
                } 
            }
            if (addtoExistingLine == false)
            {
                LineData lineData = new LineData(sOffset, productName, trxLines[j].ProductTypeCode, j, trxLines[j].Price, existingQuantity + trxLines[j].quantity, existingAmount + trxLines[j].LineAmount, existingTaxAmount + trxLines[j].tax_amount, trxLines[j].TrxProfileId, trxLines[j].ReceiptPrinted, trxLines[j].KDSSent, trxLines[j].CancelledLine, trxLines[j].ParentLine);
                lineData.trxLines.Add(trxLines[j]);
                lineDataList.Add(lineData);
            }
            else
            {
                lineDataList[targetRow].quantity = existingQuantity + trxLines[j].quantity;

                lineDataList[targetRow].taxAmount = existingTaxAmount + trxLines[j].tax_amount;
                lineDataList[targetRow].lineAmount = existingAmount + trxLines[j].LineAmount; 
                lineDataList[targetRow].trxLines.Add(trxLines[j]);
            } 

            trxLines[j].LineProcessed = true;

            for (int k = j + 1; k < trxLines.Count; k++)
            {
                if (trxLines[j].Equals(trxLines[k].ParentLine) && trxLines[k].LineValid && !trxLines[k].LineProcessed)
                    GenerateNonCardLineData(k);
            }
            log.LogMethodExit(null);
        }

        private Panel BuildNonCardProductPanel(LineData lineData, int index)
        {
            log.LogMethodEntry(lineData, index);
            Panel pnlProduct = new Panel();
            Label lblTotalPrice = BuildTotalPriceLabel();
            lblTotalPrice.Text = lineData.lineAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            //Label lblCost = BuildCostLabel();
            //string appendText = (string.IsNullOrWhiteSpace(lineData.offset) ? "" : "  ");
            string spacePadding = "  ";
            //lblCost.Text = appendText+ "Cost For Each - " + lineData.price.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            Label lblProductDescription = BuildProdDescriptionLabel();
            lblProductDescription.Text = (string.IsNullOrWhiteSpace(lineData.offset) ? spacePadding : lineData.offset)
                                                + lineData.quantity + " X " + lineData.productName;           
            // + " + " + points + " Points"; 
            //pnlProduct.BackColor = System.Drawing.Color.White;
            //pnlProduct.Controls.Add(lblCost); 
            pnlProduct.Controls.Add(lblProductDescription);
            pnlProduct.Controls.Add(lblTotalPrice);
            if (index == 0 && readOnlyMode == false)
            {
                Button btnDeleteItem = BuildDeleteButton();
                pnlProduct.Controls.Add(btnDeleteItem);
            }
            else
            {
                lblTotalPrice.Size = new Size(lblTotalPrice.Width + 47, lblTotalPrice.Height);
            }
            pnlProduct.Controls.Add(lblProductDescription);
            pnlProduct.Controls.Add(lblTotalPrice); 
            pnlProduct.BackColor = System.Drawing.Color.Transparent; 
            pnlProduct.Location = new System.Drawing.Point(3, 3);
            pnlProduct.Name = "pnlCartProducts";
            int height = 56;
            pnlProduct.Size = new System.Drawing.Size(822, height);
            pnlProduct.TabIndex = 0;
            log.LogMethodExit();
            return pnlProduct;
        }
    }
}
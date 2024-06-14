/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for Cart Screen
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
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
using Semnox.Core.GenericUtilities;

namespace Parafait_Kiosk
{
    public partial class usrCtrlCart : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string AMOUNTFORMAT;
        private string AMOUNTFORMATWITHCURRENCYSYMBOL;
        private string DATETIMEFORMAT;
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
            internal int productId;
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
            internal decimal depositQuantity = 0;
            internal double depositLineAmount = 0;
            internal Semnox.Parafait.Transaction.Transaction.TransactionLine parentLine;
            internal LineData(string offset, int productId, string productName,
                string lineType, int lineId, double price, decimal quantity, double lineAmount, double taxAmount, int trxProfileId,
                 bool receiptPrinted, bool kdsSent, bool cancelled,
                  Semnox.Parafait.Transaction.Transaction.TransactionLine parentLine, decimal depositQuantity, double depositLineAmount)
            {
                this.offset = offset;
                this.productId = productId;
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
                this.depositQuantity = depositQuantity;
                this.depositLineAmount = depositLineAmount;
            }
        }
        private List<LineData> lineDataList = new List<LineData>();
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;

        public usrCtrlCart(ExecutionContext executionContext, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxList, bool readOnlyMode = true)
        {
            log.LogMethodEntry(executionContext, trxList, readOnlyMode);
            this.executionContext = executionContext;
            this.trxLines = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>(trxList);
            this.readOnlyMode = readOnlyMode;
            InitializeComponent();
            SetCustomizedFontColors();
            DATETIMEFORMAT = KioskHelper.GetUIDateTimeFormat(executionContext);
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
            BuildTrxLineDisplayElementsNew();
            int height = GetFlpCartProductHeight();
            flpCartProducts.Size = new Size(flpCartProducts.Width, height);
            int heightValue = this.flpCartProducts.Height + 5;// this.panelButtons.Height + 5;
            this.Size = new Size(this.Width, heightValue);
            this.ResumeLayout(true);
        }

        private int GetFlpCartProductHeight()
        {
            log.LogMethodEntry();
            int height = 93;
            for (int i = flpCartProducts.Controls.Count - 1; i >= 0; i--)
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
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
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
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            lblTotalPrice.Font = new System.Drawing.Font(fontFamName, 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTotalPrice.Location = new System.Drawing.Point(555, 3);
            lblTotalPrice.Name = "lblTotalPrice";
            lblTotalPrice.Size = new System.Drawing.Size(214, 47);
            lblTotalPrice.ForeColor = KioskStatic.CurrentTheme.UsrCtrlCartLblTotalPriceTextForeColor;
            lblTotalPrice.TabIndex = 1; 
            lblTotalPrice.Text = "$80.00";
            //lblTotalPrice.BorderStyle = BorderStyle.FixedSingle;
            lblTotalPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            log.LogMethodExit();
            return lblTotalPrice;
        }
        private Label BuildProdDescriptionLabel()
        {
            log.LogMethodEntry();
            Label lblProductDescription = new Label();
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            lblProductDescription.Font = new System.Drawing.Font(fontFamName, 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblProductDescription.Location = new System.Drawing.Point(1, 3);
            lblProductDescription.Name = "lblProductDescription";
            lblProductDescription.Size = new System.Drawing.Size(554, 47);
            lblProductDescription.ForeColor = KioskStatic.CurrentTheme.UsrCtrlCartLblProductDescriptionTextForeColor;
            lblProductDescription.TabIndex = 5;
            lblProductDescription.Text = "1 X 1$ Card + 10 ";
            this.lblProductDescription.Name = "lblProductDescription";
            lblProductDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            log.LogMethodExit();
            return lblProductDescription;
        }

        private void BuildTrxLineDisplayElementsNew()
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
                lineDataList = new List<LineData>();

                bool hasDepositLineOnly = IsHavingDepositLineOnly();
                if (hasDepositLineOnly)
                {
                    GenerateLineData(0);
                }
                else
                {
                    for (int i = 0; i < trxLines.Count; i++) // display non-card trx lines
                    {
                        Semnox.Parafait.Transaction.Transaction.TransactionLine currentLine = trxLines[i];
                        if (currentLine.LineValid && currentLine.LineProcessed == false
                            && currentLine.ProductTypeCode != ProductTypeValues.SERVICECHARGE
                            && currentLine.ProductTypeCode != ProductTypeValues.GRATUITY
                            && currentLine.ProductTypeCode != ProductTypeValues.CARDDEPOSIT
                            && currentLine.ProductTypeCode != ProductTypeValues.DEPOSIT
                            && currentLine.ProductTypeCode != ProductTypeValues.LOCKERDEPOSIT)
                        {

                            for (int j = i; j < trxLines.Count; j++)
                            {
                                if (trxLines[j].LineValid && !trxLines[j].LineProcessed
                                    && trxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE
                                    && trxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY
                                    && trxLines[j].ProductTypeCode != ProductTypeValues.CARDDEPOSIT
                                    && trxLines[j].ProductTypeCode != ProductTypeValues.DEPOSIT
                                    && trxLines[j].ProductTypeCode != ProductTypeValues.LOCKERDEPOSIT)
                                {
                                    if (trxLines[j].ParentLine != null && trxLines.Equals(trxLines[j].ParentLine))
                                    {
                                        var index = trxLines.FindIndex(a => a == trxLines[j].ParentLine);

                                        if (index > -1 && index != j)
                                        {
                                            GenerateLineData(index);
                                        }
                                    }
                                    else
                                    {
                                        GenerateLineData(j);
                                    }
                                }
                            }
                        }
                    }
                }
                BuildProductPanels();
            }
            finally
            {
                this.flpCartProducts.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private void GenerateLineData(int j)
        {
            log.LogMethodEntry(j);
            bool addtoExistingLine = false;
            int targetRow = 0;
            decimal existingQuantity = 0;
            double existingAmount = 0;
            decimal existingDepositQuantity = 0;
            double existingDepositAmount = 0;
            decimal depositQuantity = 0;
            double depositAmount = 0;
            double existingTaxAmount = 0;
            string productName = GetProductName(trxLines[j]);
            string sOffset = GetOffSet(j);
            bool hasChildLines = false;
            hasChildLines = HasChildLines(j);
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
                            existingDepositQuantity = lineDataList[targetRow].depositQuantity;
                            existingDepositAmount = lineDataList[targetRow].depositLineAmount;
                            break;
                        }
                    }
                }
            }
            else
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
                        existingDepositQuantity = lineDataList[targetRow].depositQuantity;
                        existingDepositAmount = lineDataList[targetRow].depositLineAmount;
                        break;
                    }
                }
            }

            Semnox.Parafait.Transaction.Transaction.TransactionLine depositTrxLine = GetDepositData(trxLines[j], out depositQuantity, out existingDepositAmount);
            if (addtoExistingLine == false)
            {
                LineData lineData = new LineData(sOffset, trxLines[j].ProductID, productName, trxLines[j].ProductTypeCode, j, trxLines[j].Price, existingQuantity + trxLines[j].quantity, existingAmount + trxLines[j].LineAmount, existingTaxAmount + trxLines[j].tax_amount, trxLines[j].TrxProfileId, trxLines[j].ReceiptPrinted, trxLines[j].KDSSent, trxLines[j].CancelledLine, trxLines[j].ParentLine, depositQuantity, existingDepositAmount);
                lineData.trxLines.Add(trxLines[j]);
                if (depositTrxLine != null)
                {
                    lineData.trxLines.Add(depositTrxLine);
                }
                lineDataList.Add(lineData);
            }
            else
            {
                lineDataList[targetRow].quantity = existingQuantity + trxLines[j].quantity;
                lineDataList[targetRow].taxAmount = existingTaxAmount + trxLines[j].tax_amount;
                lineDataList[targetRow].lineAmount = existingAmount + trxLines[j].LineAmount;
                lineDataList[targetRow].depositLineAmount = existingDepositAmount + existingDepositAmount;
                lineDataList[targetRow].depositQuantity = existingDepositQuantity + depositQuantity;

                lineDataList[targetRow].trxLines.Add(trxLines[j]);
                if (depositTrxLine != null)
                {
                    lineDataList[targetRow].trxLines.Add(depositTrxLine);
                }
            }

            trxLines[j].LineProcessed = true;

            for (int k = j + 1; k < trxLines.Count; k++)
            {
                if (trxLines[j].Equals(trxLines[k].ParentLine) && trxLines[k].LineValid && !trxLines[k].LineProcessed)
                    GenerateLineData(k);
            }
            log.LogMethodExit(null);
        }

        private bool HasChildLines(int j)
        {
            log.LogMethodEntry(j);
            bool hasChildLines = false;
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlChild in trxLines)
            {
                if (tlChild.LineValid && trxLines[j].Equals(tlChild.ParentLine))
                {
                    hasChildLines = true;
                    break;
                }
            }
            log.LogMethodExit();
            return hasChildLines;
        }

        private string GetProductName(Semnox.Parafait.Transaction.Transaction.TransactionLine currentLine)
        {
            log.LogMethodEntry();
            string nameValue;
            string productName = KioskHelper.GetProductName(currentLine.ProductID);
            if (currentLine.ProductTypeCode == ProductTypeValues.LOCKER)
            {
                nameValue = productName + "-Locker: " + currentLine.LockerName + (string.IsNullOrEmpty(currentLine.Remarks) ? "" : "|" + currentLine.Remarks);
            }
            else if (currentLine.ProductTypeCode == ProductTypeValues.ATTRACTION)
            {
                string attractionInfo = ((currentLine.LineAtb != null && currentLine.LineAtb.AttractionBookingDTO != null)
                                            ? currentLine.LineAtb.AttractionBookingDTO.AttractionScheduleName + ": "
                                                  + currentLine.LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(DATETIMEFORMAT)
                                            : currentLine.AttractionDetails);
                nameValue = productName + (string.IsNullOrEmpty(attractionInfo) ? "" : "|" + attractionInfo) + (string.IsNullOrEmpty(currentLine.Remarks) ? "" : " - " + currentLine.Remarks);
            }
            else
            {
                nameValue = productName + (string.IsNullOrEmpty(currentLine.Remarks) ? "" : " - " + currentLine.Remarks);
            }
            log.LogMethodExit(nameValue);
            return nameValue;
        }

        private string GetOffSet(int i)
        {
            log.LogMethodEntry();
            int offset = 1;
            string sOffset = "";
            Semnox.Parafait.Transaction.Transaction.TransactionLine tl = trxLines[i].ParentLine;
            if (tl != null && tl != trxLines[i])
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
            log.LogMethodExit();
            return sOffset;
        }

        private Semnox.Parafait.Transaction.Transaction.TransactionLine GetDepositData(Semnox.Parafait.Transaction.Transaction.TransactionLine currentLine, out decimal depositQuantity,
            out double depositAmount)
        {
            log.LogMethodEntry();
            Semnox.Parafait.Transaction.Transaction.TransactionLine depositLineRec = null;
            depositQuantity = 0;
            depositAmount = 0;

            bool hasDepoLineOnly = IsHavingDepositLineOnly();
            if (hasDepoLineOnly == false)
            {
                if (currentLine != null && string.IsNullOrWhiteSpace(currentLine.CardNumber) == false)
                {
                    for (int k = 0; k < trxLines.Count; k++)
                    {
                        Semnox.Parafait.Transaction.Transaction.TransactionLine depLine = trxLines[k];
                        if (depLine.LineValid && depLine.LineProcessed == false
                               && depLine.ParentLine == null
                                && depLine.CardNumber == currentLine.CardNumber
                                && (depLine.ProductTypeCode == ProductTypeValues.DEPOSIT
                                    || depLine.ProductTypeCode == ProductTypeValues.CARDDEPOSIT
                                    || depLine.ProductTypeCode == ProductTypeValues.LOCKERDEPOSIT))
                        {
                            depositQuantity = 1;
                            depositAmount = depLine.LineAmount;
                            depLine.LineProcessed = true;
                            depositLineRec = depLine;
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit(depositLineRec);
            return depositLineRec;
        }
        private void BuildProductPanels()
        {
            log.LogMethodEntry();
            if (lineDataList != null && lineDataList.Any())
            {
                for (int i = 0; i < lineDataList.Count; i++)
                {
                    Panel nonCardPanel = BuildProductPanel(lineDataList[i], i);
                    this.flpCartProducts.Controls.Add(nonCardPanel);
                }
                int lineCount = 0;
                foreach (var item in lineDataList)
                {
                    lineCount = lineCount + (item.trxLines != null && item.trxLines.Any()
                                                   ? item.trxLines.Select(tl => tl.ProductName).Distinct().Count()
                                                   : 1);
                }
                if (lineCount > 2)
                {
                    this.BackgroundImage = ThemeManager.CurrentThemeImages.CartItemBackgroundImage;
                }
            }
            log.LogMethodExit();
        }

        private Panel BuildProductPanel(LineData lineData, int index)
        {
            log.LogMethodEntry(lineData, index);
            Panel pnlProduct = new Panel();
            Label lblTotalPrice = BuildTotalPriceLabel();
            lblTotalPrice.Text = (lineData.lineAmount + lineData.depositLineAmount).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            string spacePadding = "  ";
            Label lblNote = null;
            Label lblProductDescription = BuildProdDescriptionLabel();
            if (lineData.depositQuantity > 1)
            {
                lblProductDescription.Text = (string.IsNullOrWhiteSpace(lineData.offset) ? spacePadding : lineData.offset) + lineData.quantity + " X " + lineData.productName
                                              + " [ " + MessageUtils.getMessage("Split into ") + (lineData.depositQuantity + lineData.quantity) + " ]";
            }
            else
            {
                lblProductDescription.Text = (string.IsNullOrWhiteSpace(lineData.offset) ? spacePadding : lineData.offset)
                                                    + (lineData.quantity > 0 ? lineData.quantity : lineData.depositQuantity) + " X " + lineData.productName;
            }

            if (lineData.depositQuantity == 1)
            {
                bool hasDepoLineOnly = IsHavingDepositLineOnly();
                if (hasDepoLineOnly == false)
                {
                    string depositDescription = GetDepositDescription(lineData);
                    if (string.IsNullOrWhiteSpace(depositDescription) == false)
                    {
                        lblNote = BuildNoteLabel();
                        int paddingLength = (lineData.offset.Length + spacePadding.Length + " X ".Length);
                        string padding = string.Empty;
                        for (int i = 0; i < paddingLength; i++)
                        {
                            padding = padding + " ";
                        };
                        lblNote.Text = padding + depositDescription;
                    }
                }
            }
            if (lineData.lineType == ProductTypeValues.ATTRACTION || lineData.lineType == ProductTypeValues.LOCKER)
            {
                string[] listValues = lblProductDescription.Text.Split('|');
                if (listValues != null && listValues.Length > 1)
                {
                    lblProductDescription.Text = listValues[0];
                    Label lblNoteForATSORLOCK = BuildNoteLabel();
                    int paddingLength = (lineData.offset.Length + spacePadding.Length + " X ".Length);
                    string padding = string.Empty;
                    for (int i = 0; i < paddingLength; i++)
                    {
                        padding = padding + " ";
                    };
                    lblNoteForATSORLOCK.Text = padding + listValues[1];
                    if (lblNote == null)
                    {
                        lblNote = lblNoteForATSORLOCK;
                    }
                    else
                    {
                        lblNote.Text = lblNoteForATSORLOCK.Text + " : " + lblNote.Text;
                    }
                }
            }

            int height = 56;
            if (lblNote != null)
            {
                pnlProduct.Controls.Add(lblNote);
                height = height + lblNote.Height + 2;
            } 
            pnlProduct.Controls.Add(lblProductDescription);
            pnlProduct.Controls.Add(lblTotalPrice);
            pnlProduct.Controls.SetChildIndex(lblProductDescription, 3);
            pnlProduct.Controls.SetChildIndex(lblTotalPrice, 2);
            if (index == 0 && readOnlyMode == false)
            {
                Button btnDeleteItem = BuildDeleteButton();
                pnlProduct.Controls.Add(btnDeleteItem);
                Label lblDeleteBtnExtender = BuildDeleteBtnExtenderLabel();
                pnlProduct.Controls.Add(lblDeleteBtnExtender);
                pnlProduct.Controls.SetChildIndex(lblDeleteBtnExtender, 1);
                pnlProduct.Controls.SetChildIndex(btnDeleteItem, 0);
            }
            else
            {
                lblTotalPrice.Size = new Size(lblTotalPrice.Width + 47, lblTotalPrice.Height);
            }

            pnlProduct.BackColor = System.Drawing.Color.Transparent;
            pnlProduct.Location = new System.Drawing.Point(3, 3);
            pnlProduct.Name = "pnlCartProducts";
            pnlProduct.Size = new System.Drawing.Size(822, height);
            pnlProduct.TabIndex = 0;
            log.LogMethodExit();
            return pnlProduct;
        }

        private string GetDepositDescription(LineData lineData)
        {
            log.LogMethodEntry("lineData");
            string description = string.Empty;
            if (lineData != null && lineData.trxLines != null && lineData.trxLines.Any())
            {
                foreach (var item in lineData.trxLines)
                {
                    if (item.ProductTypeCode == ProductTypeValues.CARDDEPOSIT ||
                        item.ProductTypeCode == ProductTypeValues.DEPOSIT ||
                        item.ProductTypeCode == ProductTypeValues.LOCKERDEPOSIT
                        )
                    {
                        string productName = KioskHelper.GetProductName(item.ProductID);
                        description = productName + " - " + item.LineAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                        break;
                    }
                }
            }
            log.LogMethodExit(description);
            return description;
        }

        private Label BuildNoteLabel()
        {
            log.LogMethodEntry();
            Label lblNote = new Label();
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            lblNote.Font = new System.Drawing.Font(fontFamName, 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblNote.Location = new System.Drawing.Point(1, 47);
            lblNote.Name = "lblNote";
            lblNote.Size = new System.Drawing.Size(554 + 214, 26);
            lblNote.ForeColor = KioskStatic.CurrentTheme.UsrCtrlCartLblProductDescriptionTextForeColor;
            lblNote.TabIndex = 8;
            lblNote.Text = "Note";
            lblNote.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            log.LogMethodExit();
            return lblNote;
        }

        private Label BuildDeleteBtnExtenderLabel()
        {
            log.LogMethodEntry();
            Label lblDeleteBtnExtender = new Label();
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            lblDeleteBtnExtender.Font = new System.Drawing.Font(fontFamName, 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblDeleteBtnExtender.Location = new System.Drawing.Point(742, 0);
            lblDeleteBtnExtender.Margin = new System.Windows.Forms.Padding(0);
            lblDeleteBtnExtender.AutoSize = false;
            lblDeleteBtnExtender.Name = "lblDeleteBtnExtender";
            lblDeleteBtnExtender.MinimumSize = new System.Drawing.Size(82, 49);
            lblDeleteBtnExtender.Size = new System.Drawing.Size(82, 49);
            lblDeleteBtnExtender.BackColor = Color.Transparent;
            lblDeleteBtnExtender.ForeColor = Color.Transparent;
            //lblDeleteBtnExtender.BorderStyle = BorderStyle.FixedSingle;
            lblDeleteBtnExtender.TabIndex = 2;
            lblDeleteBtnExtender.Text = "";
            lblDeleteBtnExtender.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblDeleteBtnExtender.Click += new System.EventHandler(this.btnDelete_Clicked); 
            log.LogMethodExit();
            return lblDeleteBtnExtender;
        }

        private bool IsHavingDepositLineOnly()
        {
            log.LogMethodEntry();
            bool hasDepositLineOnly = false;
            hasDepositLineOnly = (trxLines != null && trxLines.Count == 1
                                        && trxLines.Exists(tl => tl.LineValid
                                                    && (tl.ProductTypeCode == ProductTypeValues.CARDDEPOSIT
                                                     || tl.ProductTypeCode == ProductTypeValues.DEPOSIT
                                                     || tl.ProductTypeCode == ProductTypeValues.LOCKERDEPOSIT)));

            log.LogMethodExit(hasDepositLineOnly);
            return hasDepositLineOnly;
        }
    }
}
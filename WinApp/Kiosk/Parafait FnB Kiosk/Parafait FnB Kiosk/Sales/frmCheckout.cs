/********************************************************************************************
 * Project Name - Parafait Kiosk- frmCheckOut 
 * Description  - frmCheckOut screen
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        10-Nov-2019      Girish             Ticket printer integration
 *2.100.0     05-Aug-2020      Guru S A           Kiosk activity log changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.logger;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.POS;

namespace Parafait_FnB_Kiosk
{
    public partial class frmCheckout : BaseFormMenuChoice
    {
        private readonly Semnox.Core.Utilities.ExecutionContext executionContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
        internal bool _viewOnly = false;
        bool enableBillCollector = false;
        KioskStatic.acceptance ac;
        int billPayOperationStatus = -1;
        private readonly TagNumberParser tagNumberParser;

        public frmCheckout(bool viewOnly = false)
        {
            Common.logEnter();
            InitializeComponent();
            enableBillCollector = Helper.GetEnableBillCollectorFlag();
            Common.utils.setLanguage(this);

            tagNumberParser = new TagNumberParser(Common.utils.ExecutionContext);
            if (enableBillCollector)
            {
                this.panelPayment.Controls.Add(this.btnBillPay);
                this.panelPayment.Controls.Add(this.pictureBoxBillPay);
                this.btnBillPay.Visible = true;
                this.pictureBoxBillPay.Visible = true;
                this.lblBillPay.Visible = true;
                //this.btnCreditCard.Location = new System.Drawing.Point(449, 124);
                //this.btnDebitCard.Location = new System.Drawing.Point(794, 124);
                //this.btnCancelOrder.Location = new System.Drawing.Point(104, 124);
                //this.btnCancelOrder.Size = new System.Drawing.Size(184, 49);
                this.btnCancelOrder.Location = new System.Drawing.Point(40, 124);
                this.btnBillPay.Location = new System.Drawing.Point(228, 124);
                this.pictureBoxBillPay.Location = new System.Drawing.Point(416, 108);
                this.btnCreditCard.Location = new System.Drawing.Point(518, 124);
                this.pictureBox1.Location = new System.Drawing.Point(706, 117);
                this.btnDebitCard.Location = new System.Drawing.Point(808, 124);
                this.pictureBox2.Location = new System.Drawing.Point(996, 117);
                lblCashMessage.Text = "";
            }
            else
            {
                lblCashMessage.Text = Common.utils.MessageUtils.getMessage(1117);
            }

            _viewOnly = viewOnly;

            if (viewOnly)
            {
                panelPayment.Visible = false;
                btnViewOrderCheckOut.Location = new Point(btnCreditCard.Left, panelPayment.Top + btnCreditCard.Top);
            }
            else
                btnViewOrderCheckOut.Visible = false;

            Common.logExit();
        }

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            Common.logEnter();

            this.KeyPreview = true;

            try
            {
                object screenId = Common.utils.executeScalar(@"select screenId 
                                                        from AppScreens 
                                                        where CodeObjectName = 'frmCheckout'");
                if (screenId == null)
                    throw new ApplicationException("Checkout screen not defined in setup");

                _screenModel = new ScreenModel(Convert.ToInt32(screenId));

                base.RenderPanelContent(_screenModel, panelHeader, 1);

                flpCheckout.Top = panelHeader.Bottom;
                flpCheckout.Height = this.Height - panelHeader.Height;

                loadDetails();
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }

            Common.logExit();
        }

        void loadSuggestiveSaleItems(List<int> productsIdList)
        {
            log.LogMethodEntry(productsIdList);
            DataTable dTable;
            Button dynamicButton;
            Control cnrl;

            if (productsIdList.Count == 0)
            {
                if (flpSuggestiveItems.Visible)
                {
                    panelOrderSummary.Height = panelOrderSummary.Height + flpSuggestiveItems.Height;
                    flpSuggestiveItems.Visible = false;
                }
                log.LogMethodExit();
                return;
            }
            dTable = Helper.getSuggestiveProducts(productsIdList);
            cnrl = lblSuggestive;
            flpSuggestiveItems.Controls.Clear();
            flpSuggestiveItems.Controls.Add(cnrl);
            if (dTable != null && dTable.Rows.Count > 0)
            {
                if (!flpSuggestiveItems.Visible)
                {
                    panelOrderSummary.Height = panelOrderSummary.Height - flpSuggestiveItems.Height;
                    flpSuggestiveItems.Visible = true;
                }
                for (int i = 0; i < dTable.Rows.Count && i < 4; i++)
                {
                    dynamicButton = new Button();
                    dynamicButton.FlatAppearance.BorderSize = 0;
                    dynamicButton.FlatAppearance.MouseDownBackColor = this.btnSample1.FlatAppearance.MouseDownBackColor;
                    dynamicButton.FlatAppearance.MouseOverBackColor = this.btnSample1.FlatAppearance.MouseOverBackColor;
                    dynamicButton.FlatStyle = this.btnSample1.FlatStyle;
                    dynamicButton.ForeColor = this.btnSample1.ForeColor;
                    //dynamicButton.Font = this.btnSample1.Font;

                    Image productImage = Helper.GetProductImage((dTable.Rows[i]["ImageFileName"] == DBNull.Value) ? "" : (dTable.Rows[i]["ImageFileName"].ToString()));
                    if (productImage == null)
                        dynamicButton.BackgroundImage = this.btnSample1.BackgroundImage;
                    else
                        dynamicButton.BackgroundImage = productImage;

                    dynamicButton.Location = this.btnSample1.Location;
                    dynamicButton.Margin = this.btnSample1.Margin;
                    dynamicButton.Name = this.btnSample1.Name;
                    dynamicButton.Size = this.btnSample1.Size;
                    if (dynamicButton.Size != dynamicButton.BackgroundImage.Size)
                    {
                        dynamicButton.BackgroundImageLayout = ImageLayout.Stretch;
                    }

                    if (productImage == null)
                        dynamicButton.Text = dTable.Rows[i]["product_name"].ToString() + " \n " + Convert.ToDouble(dTable.Rows[i]["Price"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    else
                        dynamicButton.Text = "";
                    dynamicButton.UseVisualStyleBackColor = this.btnSample1.UseVisualStyleBackColor;
                    dynamicButton.Tag = dTable.Rows[i]["product_id"];
                    dynamicButton.Click += (sen, arg) =>
                    {
                        base.ResetTimeOut();
                        Button btnClicked = (Button)sen;
                        ScreenModel.UIPanelElement pnlElement = new ScreenModel.UIPanelElement();
                        ScreenModel.ElementParameter wbParam = new ScreenModel.ElementParameter();
                        wbParam.OrderedQuantity = 1;
                        wbParam.OrderedValue = btnClicked.Tag; // suggest product id

                        pnlElement.Parameters.Add(wbParam);
                        UserTransaction.OrderDetails.AddItem(pnlElement);
                        UserTransaction.getOrderTotal();
                        loadDetails();
                        vScrollBar.Value = 0;//scroll bar not moving up during the suggestive sale product adding
                    };
                    flpSuggestiveItems.Controls.Add(dynamicButton);
                }

            }
            else
            {
                if (flpSuggestiveItems.Visible)
                {
                    panelOrderSummary.Height = panelOrderSummary.Height + flpSuggestiveItems.Height;
                    flpSuggestiveItems.Visible = false;
                }
            }
            log.LogMethodExit();
        }
        void loadDetails()
        {
            Common.logEnter();
            List<int> productIdList = new List<int>(); //Suggestive sale changes to store the productids
            flpOrderSummary.Controls.Clear();
            foreach (ScreenModel.UIPanelElement element in UserTransaction.OrderDetails.ElementList)
            {
                Transaction trx = UserTransaction.getElementTransaction(element);
                if (trx.TrxLines.Count == 0)
                    continue;

                FlowLayoutPanel flpElement = new FlowLayoutPanel();
                flpElement.AutoSize = flpElementSample.AutoSize;
                flpElement.Margin = flpElementSample.Margin;
                flpElement.FlowDirection = flpElementSample.FlowDirection;

                FlowLayoutPanel flpItems = new FlowLayoutPanel();
                flpItems.AutoSize = flpItemsSample.AutoSize;
                flpItems.Margin = flpItemsSample.Margin;
                flpItems.FlowDirection = flpItemsSample.FlowDirection;

                flpElement.Controls.Add(flpItems);

                //Suggestive sale changes finding the sub product ids in the element parameter list

                foreach (Transaction.TransactionLine tl in trx.TrxLines)
                    productIdList.Add(tl.ProductID);

                //Ends:Suggestive sale changes
                bool variableCard = false;
                if (element.Parameters.Count == 1)
                {
                    DataRow drProd = trx.getProductDetails(Convert.ToInt32(element.Parameters[0].OrderedValue));

                    if (drProd["product_type"].ToString() == "VARIABLECARD")
                    {
                        FlowLayoutPanel flpInternal = new FlowLayoutPanel();
                        flpInternal.AutoSize = flpInternalSample.AutoSize;
                        flpInternal.Margin = flpInternalSample.Margin;
                        flpInternal.FlowDirection = flpInternalSample.FlowDirection;
                        flpInternal.Size = flpInternalSample.Size;

                        flpItems.Controls.Add(flpInternal);

                        Label lblPrimaryProduct = new Label();
                        lblPrimaryProduct.AutoSize = false;
                        lblPrimaryProduct.ForeColor = lblPrimaryProductSample.ForeColor;
                        lblPrimaryProduct.Font = lblPrimaryProductSample.Font;
                        lblPrimaryProduct.Size = lblPrimaryProductSample.Size;

                        flpInternal.Controls.Add(lblPrimaryProduct);

                        Label lblPrice = new Label();
                        lblPrice.ForeColor = lblPriceSample.ForeColor;
                        lblPrice.Font = lblPriceSample.Font;
                        lblPrice.AutoSize = lblPriceSample.AutoSize;

                        flpInternal.Controls.Add(lblPrice);

                        lblPrimaryProduct.Text = drProd["Product_Name"].ToString();

                        lblPrice.Text = element.Parameters[0].UserPrice.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

                        variableCard = true;
                    }
                }

                if (!variableCard)
                {
                    foreach (Transaction.TransactionLine line in trx.TrxLines)
                    {
                        if (line.LineProcessed)
                            continue;

                        if (line.ProductTypeCode == "CARDDEPOSIT")
                            continue;

                        decimal qty = line.quantity;
                        decimal amount = (decimal)line.LineAmount;

                        if (trx.TrxLines.Find(x => line.Equals(x.ParentLine)) == null)
                        {
                            foreach (Transaction.TransactionLine repeat in trx.TrxLines)
                            {
                                if (!repeat.LineProcessed
                                    && ((repeat.ProductID == line.ProductID)
                                        || (line.ProductTypeCode == "NEW" && repeat.ProductTypeCode == "CARDDEPOSIT" && line.CardNumber != repeat.CardNumber))
                                    && repeat != line)
                                {
                                    qty += repeat.quantity;
                                    amount += (decimal)repeat.LineAmount;
                                    repeat.LineProcessed = true;
                                }
                            }
                        }

                        FlowLayoutPanel flpInternal = new FlowLayoutPanel();
                        flpInternal.AutoSize = flpInternalSample.AutoSize;
                        flpInternal.Margin = flpInternalSample.Margin;
                        flpInternal.FlowDirection = flpInternalSample.FlowDirection;
                        flpInternal.Size = flpInternalSample.Size;

                        flpItems.Controls.Add(flpInternal);

                        Label lblPrimaryProduct = new Label();
                        lblPrimaryProduct.AutoSize = false;
                        lblPrimaryProduct.ForeColor = lblPrimaryProductSample.ForeColor;
                        lblPrimaryProduct.Font = lblPrimaryProductSample.Font;
                        lblPrimaryProduct.Size = lblPrimaryProductSample.Size;

                        flpInternal.Controls.Add(lblPrimaryProduct);

                        Label lblPrice = new Label();
                        lblPrice.ForeColor = lblPriceSample.ForeColor;
                        lblPrice.Font = lblPriceSample.Font;
                        lblPrice.AutoSize = lblPriceSample.AutoSize;

                        flpInternal.Controls.Add(lblPrice);

                        lblPrimaryProduct.Text = line.ProductName;

                        if (qty > 1)
                            lblPrimaryProduct.Text += string.Format(" ({0:N0})", qty);

                        if (amount > 0)
                            lblPrice.Text = amount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        else
                            lblPrice.Text = "";

                        foreach (Transaction.TransactionLine childLine in trx.TrxLines)
                        {
                            if (childLine.ProductTypeCode == "CARDDEPOSIT")
                                continue;

                            if (line.Equals(childLine.ParentLine) && !childLine.LineProcessed)
                            {
                                qty = childLine.quantity;
                                amount = (decimal)childLine.LineAmount;

                                if (trx.TrxLines.Find(x => childLine.Equals(x.ParentLine)) == null)
                                {
                                    foreach (Transaction.TransactionLine repeat in trx.TrxLines)
                                    {
                                        if (!repeat.LineProcessed
                                            && !repeat.ModifierLine
                                            && ((repeat.ProductID == childLine.ProductID)
                                                || (childLine.ProductTypeCode == "NEW" && repeat.ProductTypeCode == "CARDDEPOSIT" && childLine.CardNumber != repeat.CardNumber))
                                            && repeat != childLine
                                            && line.Equals(repeat.ParentLine))
                                        {
                                            qty += repeat.quantity;
                                            amount += (decimal)repeat.LineAmount;
                                            repeat.LineProcessed = true;
                                        }
                                    }
                                }

                                flpInternal = new FlowLayoutPanel();
                                flpInternal.AutoSize = flpInternalSample.AutoSize;
                                flpInternal.Margin = flpInternalSample.Margin;
                                flpInternal.FlowDirection = flpInternalSample.FlowDirection;
                                flpInternal.Size = flpInternalSample.Size;

                                flpItems.Controls.Add(flpInternal);

                                Label lblSecProduct = new Label();
                                lblSecProduct.AutoSize = false;
                                lblSecProduct.ForeColor = lblSecondaryProductSample.ForeColor;
                                lblSecProduct.Font = lblSecondaryProductSample.Font;
                                lblSecProduct.Size = lblSecondaryProductSample.Size;

                                if (childLine.ModifierLine)
                                    lblSecProduct.Text = " - " + childLine.ProductName;
                                else
                                    lblSecProduct.Text = childLine.ProductName;

                                if (qty > 1)
                                    lblSecProduct.Text += string.Format(" ({0:N0})", qty);

                                flpInternal.Controls.Add(lblSecProduct);

                                lblPrice = new Label();
                                lblPrice.ForeColor = lblPriceSample.ForeColor;
                                lblPrice.Font = lblPriceSample.Font;
                                lblPrice.AutoSize = lblPriceSample.AutoSize;

                                if (amount > 0)
                                    lblPrice.Text = amount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                else
                                    lblPrice.Text = "";

                                flpInternal.Controls.Add(lblPrice);

                                childLine.LineProcessed = true;

                                foreach (Transaction.TransactionLine nextLevel in trx.TrxLines)
                                {
                                    if (childLine.Equals(nextLevel.ParentLine))
                                    {
                                        flpInternal = new FlowLayoutPanel();
                                        flpInternal.AutoSize = flpInternalSample.AutoSize;
                                        flpInternal.Margin = flpInternalSample.Margin;
                                        flpInternal.FlowDirection = flpInternalSample.FlowDirection;
                                        flpInternal.Size = flpInternalSample.Size;

                                        flpItems.Controls.Add(flpInternal);

                                        Label lblnextLevel = new Label();
                                        lblnextLevel.AutoSize = false;
                                        lblnextLevel.ForeColor = lblSecondaryProductSample.ForeColor;
                                        lblnextLevel.Font = lblSecondaryProductSample.Font;
                                        lblnextLevel.Size = lblSecondaryProductSample.Size;

                                        lblnextLevel.Text = " - " + nextLevel.ProductName;

                                        flpInternal.Controls.Add(lblnextLevel);

                                        lblPrice = new Label();
                                        lblPrice.ForeColor = lblPriceSample.ForeColor;
                                        lblPrice.Font = lblPriceSample.Font;
                                        lblPrice.AutoSize = lblPriceSample.AutoSize;

                                        if (nextLevel.Price > 0)
                                            lblPrice.Text = nextLevel.LineAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                        else
                                            lblPrice.Text = "";

                                        flpInternal.Controls.Add(lblPrice);

                                        nextLevel.LineProcessed = true;
                                    }
                                }
                            }
                        }

                        line.LineProcessed = true;
                    }
                }

                Panel panelButtons = new Panel();
                panelButtons.Size = panelButtonsSample.Size;
                flpElement.Controls.Add(panelButtons);

                Button btnChange = new Button();
                btnChange.Size = btnChangeSample.Size;
                btnChange.Image = btnChangeSample.Image;
                btnChange.FlatStyle = FlatStyle.Flat;
                btnChange.FlatAppearance.BorderSize = 0;
                btnChange.FlatAppearance.CheckedBackColor =
                    btnChange.FlatAppearance.MouseDownBackColor =
                    btnChange.FlatAppearance.MouseOverBackColor = Color.Transparent;

                btnChange.Location = btnChangeSample.Location;
                btnChange.Text = btnChangeSample.Text;
                btnChange.ForeColor = btnChangeSample.ForeColor;

                panelButtons.Controls.Add(btnChange);
                if (element.ActionScreenId < 0)
                    btnChange.Visible = false;
                btnChange.Click += delegate
                {
                    base.ResetTimeOut();
                    Common.OpenScreen(element);
                    UserTransaction.getOrderTotal();
                    loadDetails();
                };

                Button btnDelete = new Button();
                btnDelete.Size = btnDeleteSample.Size;
                btnDelete.Image = btnDeleteSample.Image;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.FlatAppearance.CheckedBackColor =
                    btnDelete.FlatAppearance.MouseDownBackColor =
                    btnDelete.FlatAppearance.MouseOverBackColor = Color.Transparent;

                btnDelete.Location = btnDeleteSample.Location;
                btnDelete.Text = btnDeleteSample.Text;
                btnDelete.ForeColor = btnDeleteSample.ForeColor;

                panelButtons.Controls.Add(btnDelete);

                btnDelete.Click += delegate
                {
                    base.ResetTimeOut();
                    if (Common.ShowDialog(Common.utils.MessageUtils.getMessage(1121)) == System.Windows.Forms.DialogResult.Yes)
                    {
                        UserTransaction.OrderDetails.ElementList.Remove(element);
                        UserTransaction.getOrderTotal();
                        loadDetails();
                        vScrollBar.Value = 0;//scroll bar not moving up during the suggestive sale product adding

                        if (UserTransaction.OrderDetails.ElementList.Count == 0)
                            Close();
                    }
                };

                flpOrderSummary.Controls.Add(flpElement);
            }
            loadSuggestiveSaleItems(productIdList);//Suggestive sale changes
            UpdateHeader();

            lblOrderNumber.Text = UserTransaction.OrderDetails.TableNumber;

            lblSubtotal.Text = UserTransaction.OrderDetails.SubtotalAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            lblTax.Text = UserTransaction.OrderDetails.TaxAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            lblTotal.Text = UserTransaction.OrderDetails.TotalAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

            if (flpOrderSummary.Height > panelOrderSummary.Height)
            {
                //btnScrollDown.Visible = btnScrollUp.Visible = true;
                vScrollBar.Visible = true;


                vScrollBar.Maximum = flpOrderSummary.Height - panelOrderSummary.Height + 100;
            }
            else
            {
                //btnScrollDown.Visible = btnScrollUp.Visible = false;
                vScrollBar.Visible = false;
            }

            flpOrderSummary.Top = 0;
            Common.logExit();
        }

        private void frmCheckout_Activated(object sender, EventArgs e)
        {
            Common.logEnter();
            if (UserTransaction.OrderDetails.ElementList.Count == 0)
                Close();
            Common.logExit();
        }

        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            Common.logEnter();
            base.ResetTimeOut();
            foreach (Form f in Application.OpenForms)
            {
                if (f is frmCheckout)
                {
                    f.Close();
                }
            }
            Common.logExit();
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            if (Common.ShowDialog(Common.utils.MessageUtils.getMessage(1120)) == System.Windows.Forms.DialogResult.Yes)
            {
                if (enableBillCollector)
                {
                    RecordAbort();
                }
                Common.GoHome();
            }
            Common.logExit();
        }

        private void btnCreditCard_Click(object sender, EventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            CheckOut("CREDIT");
            Common.logExit();
        }

        private void btnDebitCard_Click(object sender, EventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            CheckOut("DEBIT");
            Common.logExit();
        }

        void abortAndExit(string message)
        {
            base.ResetTimeOut();
            Common.logEnter();
            if (enableBillCollector)
            {
                RecordAbort();
            }
            else
            {
                cancelCCPayment(UserTransaction.OrderDetails.transactionPaymentsDTO);
            }
            Common.ShowMessage(message);
            Common.logExit();
        }

        private void RecordAbort()
        {
            Common.logEnter();
            if (UserTransaction.OrderDetails != null
                                && UserTransaction.OrderDetails.transactionPaymentsDTO != null
                                && UserTransaction.OrderDetails.transactionPaymentsDTO.paymentModeDTO != null
                                && UserTransaction.OrderDetails.transactionPaymentsDTO.paymentModeDTO.IsCash == false)
            {
                cancelCCPayment(UserTransaction.OrderDetails.transactionPaymentsDTO);
            }
            else
            {
                RecordCashAbort();
            }
            Common.logExit();
        }

        frmBillPayMessage frmBillPayMsg;
        private void BtnBillPayClick(object sender, EventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            CheckOut("BILL");
            Common.logExit();
        }

        void PerformCancelOrder(KioskStatic.acceptance _ac)
        {
            base.ResetTimeOut();
            Common.logEnter();
            ac = _ac;
            if (enableBillCollector)
            {
                RecordAbort();
                Common.GoHome();
            }
            Common.logExit();
        }

        void SubmitBillPayment(KioskStatic.acceptance _ac, int opStatus)
        {
            base.ResetTimeOut();
            Common.logEnter();
            ac = _ac;
            billPayOperationStatus = opStatus;
            Common.logExit();
        }
        // KioskCore.CardDispenser cardDispenser;
        CardDispenser cardDispenser;
        DeviceClass readerDevice;
        USBDevice dispenserCardReader;
        EventHandler cardScanCompleteEvent;
        void CheckOut(string PaymentMode)
        {
            base.ResetTimeOut();
            Common.logEnter();
            try
            {
                Transaction trx = UserTransaction.GetTransaction();
                if (KioskStatic.config.dispport == -1)
                {
                    log.Info("Card dispenser is disabled by setting port number = -1");
                    Common.logToFile("Card dispenser is disabled by setting port number = -1 ");
                    POSPrinterDTO rfidPrinterDTO = KioskStatic.GetRFIDPrinter(executionContext, Common.utils.ParafaitEnv.POSMachineId);
                    foreach (Transaction.TransactionLine line in trx.TrxLines)
                    {
                        if (KioskStatic.IsWristBandPrintTag(line.ProductID, rfidPrinterDTO) == false)
                        {
                            log.Info("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                            Common.ShowMessage(Common.utils.MessageUtils.getMessage(2384)); // Card dispenser is Disabled.Sorry you cannot proceed
                            KioskStatic.logToFile("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                            break;
                        }
                    }
                    return;
                }

                cardDispenser = getCardDispenser(trx);
                readerDevice = null;
                dispenserCardReader = null;
                cardScanCompleteEvent = null;

                if (cardDispenser != null)
                {
                    cardScanCompleteEvent =
                     delegate (object sender, EventArgs e)
                     {
                         Common.logEnter();
                         DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                         Common.logToFile("checkScannedEvent.Message: " + checkScannedEvent.Message);
                         try
                         {
                             TagNumber tagNumber;
                             TagNumberParser tagNumberParser = new TagNumberParser(Common.utils.ExecutionContext);
                             if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                             {
                                 string message = tagNumberParser.Validate(checkScannedEvent.Message);
                                 Common.logToFile(message);
                                 return;
                             }
                             cardDispenser.HandleDispenserCardRead(tagNumber.Value);
                         }
                         catch (Exception ex)
                         {
                             Common.logException(ex);
                         }
                         Common.logExit();
                     };
                    try
                    {
                        readerDevice = DeviceContainer.RegisterDispenserCardReader(Common.utils.ExecutionContext, this, cardScanCompleteEvent, KioskStatic.CardDispenserModel);
                    }
                    catch (Exception ex)
                    {
                        Common.log.Error(ex);
                        Common.ShowMessage(ex.Message + ". " + Common.utils.MessageUtils.getMessage(441));
                        return;
                    }
                }


                try
                {
                    bool isAlohaEnv = Common.utils.getParafaitDefaults("PERFORM_DIRECT_ALOHA_SYNC").Equals("Y");

                    if (isAlohaEnv)
                        UserTransaction.AlohaLogin();

                    Common.logToFile("PaymentMode: " + PaymentMode);
                    if (PaymentMode == "CASH" || PaymentMode == "BILL")
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (PaymentMode == "BILL")
                        {
                            lblCashMessage.Text = Common.utils.MessageUtils.getMessage(1401, UserTransaction.OrderDetails.TotalAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                            frmBillPayMsg = new frmBillPayMessage(lblCashMessage.Text, ac);
                            frmBillPayMsg.setCallBack = new frmBillPayMessage.frmBillPayMessageDelegate(this.SubmitBillPayment);
                            frmBillPayMsg.ShowDialog();
                            if (billPayOperationStatus == 1) //Notes received
                            {
                                if (ac != null && ac.totalValue >= UserTransaction.OrderDetails.TotalAmount)
                                {
                                    Common.logToFile("Bill Payment received");
                                    if (paymentModeDTOList != null)
                                    {
                                        UserTransaction.OrderDetails.transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                                        UserTransaction.OrderDetails.transactionPaymentsDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                                        UserTransaction.OrderDetails.transactionPaymentsDTO.Amount = trx.Net_Transaction_Amount;
                                        trx.TransactionPaymentsDTOList.Add(UserTransaction.OrderDetails.transactionPaymentsDTO);
                                    }
                                    else
                                    {
                                        Common.logToFile("Payment Mode not found. Contact Staff");
                                        throw new ApplicationException("Payment Mode set up not found. Contact Staff");
                                    }
                                }
                                else
                                {
                                    Common.logToFile("Bill Payment not successful");
                                    throw new ApplicationException("Bill Payment not successful");
                                }
                            }
                            else if (billPayOperationStatus == 0) //user cancelled transaction in between
                            {
                                Common.logToFile("Bill Payment Cancelled in between payment");
                                throw new ApplicationException("Bill Payment Cancelled");
                            }
                            else
                            {
                                Common.logToFile("Bill Payment Cancelled with out any payment");
                                ClearDevices();
                                return;
                            }
                        }
                        else
                        {
                            Common.logToFile("Cash Payment received");
                            if (paymentModeDTOList != null)
                            {
                                UserTransaction.OrderDetails.transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                                UserTransaction.OrderDetails.transactionPaymentsDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                                UserTransaction.OrderDetails.transactionPaymentsDTO.Amount = trx.Net_Transaction_Amount;
                                trx.TransactionPaymentsDTOList.Add(UserTransaction.OrderDetails.transactionPaymentsDTO);
                            }
                            else
                            {
                                Common.logToFile("Payment Mode set up not found. Contact Staff");
                                throw new ApplicationException("Payment Mode set up not found. Contact Staff");
                            }
                        }
                    }
                    else
                    {
                        CreditCardPayment(trx, UserTransaction.OrderDetails.transactionPaymentsDTO, (PaymentMode == "DEBIT"));

                        if (UserTransaction.OrderDetails.transactionPaymentsDTO.GatewayPaymentProcessed)
                        {
                            Common.logToFile("Card Payment received");
                            trx.TransactionPaymentsDTOList.Add(UserTransaction.OrderDetails.transactionPaymentsDTO);
                        }
                        else
                            throw new ApplicationException("Payment not successful");
                    }

                    if (isAlohaEnv)
                        UserTransaction.AlohaTransaction(trx);

                    bool cardDispensed = dispenseCards(trx, cardDispenser, UserTransaction.OrderDetails.transactionPaymentsDTO);

                    if (isAlohaEnv == false && cardDispensed == false)
                    {
                        //throw new ApplicationException("Unable to dispense the card");
                        return;
                    }

                    if (Helper.ShowTent())
                    {
                        string delimit = " ";
                        if (string.IsNullOrEmpty(trx.Remarks))
                            delimit = "";
                        trx.Remarks += delimit + "Tent #: " + UserTransaction.OrderDetails.TableNumber + ((Helper.GetShowPartySizeFlag()) ? " Party Size: " + UserTransaction.OrderDetails.NumberOfGuests.ToString() : "");
                    } 
                    UserTransaction.SaveTransaction(trx);
                    UserTransaction.OrderDetails.transactionPaymentsDTO.GatewayPaymentProcessed = false;

                    transferCardCredits(trx);

                    bool allCardSale = UserTransaction.OrderDetails.AllCardSale();

                    if (allCardSale)
                        Common.ShowMessage(Common.utils.MessageUtils.getMessage(499));
                    else
                    {
                        Common.logToFile(Common.utils.MessageUtils.getMessage(1113));
                        (new frmFinishOrder()).ShowDialog();
                    }

                    Common.GoHome();
                }
                catch (Exception ex)
                {
                    Common.logException(ex);
                    abortAndExit(ex.Message);
                }
                finally
                {
                    ClearDevices();
                }
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
            }
            Common.logExit();
        }

        private void ClearDevices()
        {
            Common.logEnter();
            if (cardDispenser != null)
            {
                cardDispenser.spCardDispenser.Close();
                cardDispenser = null;
            }

            if (readerDevice != null)
            {
                readerDevice.UnRegister();
                readerDevice.Dispose();
            }
            if (dispenserCardReader != null)
            {
                dispenserCardReader.UnRegister();
                dispenserCardReader.Dispose();
            }
            Common.logExit();
        }
        void transferCardCredits(Transaction trx)
        {
            Common.logEnter();
            foreach (Transaction.TransactionLine tl in trx.TrxLines)
                tl.LineProcessed = false;

            foreach (ScreenModel.UIPanelElement element in UserTransaction.OrderDetails.ElementList)
            {
                List<Card> cardSet = new List<Card>();

                foreach (ScreenModel.ElementParameter parameter in element.Parameters)
                {
                    if (parameter.OrderedValue != null
                        && parameter.OrderedValue != DBNull.Value
                        && parameter.CardCount > 1)
                    {
                        Card primaryCard = null;
                        foreach (Transaction.TransactionLine tl in trx.TrxLines)
                        {
                            if (tl.LineProcessed == false && tl.card != null && tl.ProductID == Convert.ToInt32(parameter.OrderedValue))
                            {
                                primaryCard = tl.card;
                                cardSet.Add(tl.card);
                                tl.LineProcessed = true;
                                break;
                            }
                        }

                        if (primaryCard != null)
                        {
                            int qty = parameter.CardCount - 1;
                            foreach (Transaction.TransactionLine tl in trx.TrxLines)
                            {
                                if (qty > 0 && tl.LineProcessed == false && tl.card != null && primaryCard != tl.card)
                                {
                                    cardSet.Add(tl.card);
                                    tl.LineProcessed = true;
                                    qty--;

                                    if (qty == 0)
                                        break;
                                }
                            }
                        }
                    }
                }

                if (cardSet.Count > 1)
                {
                    double totalCredits = 0;
                    Card primaryCard = cardSet[0];

                    foreach (Transaction.TransactionLine tl in trx.TrxLines)
                    {
                        if (primaryCard.Equals(tl.card))
                        {
                            totalCredits += tl.Credits;
                        }
                    }

                    TaskProcs tp = new TaskProcs(trx.Utilities);
                    int each = 0;
                    each = (int)(totalCredits / cardSet.Count);
                    foreach (Card card in cardSet)
                    {
                        if (card != primaryCard)
                        {
                            string message = "";
                            bool sv = tp.BalanceTransfer(primaryCard.card_id, card.card_id, each, 0, 0, 0, "Kiosk Trx Transfer", ref message);
                            if (!sv)
                            {
                                Common.logToFile("Trx transfer: " + message);
                            }

                            tp.LinkChildCard(primaryCard.card_id, card.card_id);

                            foreach (Transaction.TransactionLine tlc in trx.TrxLines)
                            {
                                if (card.Equals(tlc.card))
                                    tlc.LineProcessed = true;
                            }
                        }
                    }
                }
            }
            Common.logExit();
        }

        bool dispenseCards(Transaction trx, CardDispenser cardDispenser, TransactionPaymentsDTO transactionPaymentsDTO)
        {
            Common.logEnter();
            bool returnValue = true;
            bool wristBandPrintTag = false;
            bool newCardNumber = false;
            POSPrinterDTO rfidPrinterDTO = KioskStatic.GetRFIDPrinter(executionContext, Common.utils.ParafaitEnv.POSMachineId);
            foreach (Transaction.TransactionLine line in trx.TrxLines)
            {

                wristBandPrintTag = KioskStatic.IsWristBandPrintTag(line.ProductID, rfidPrinterDTO);
                log.Info("wristBandPrintTag  : " + wristBandPrintTag);
                KioskStatic.logToFile("wristBandPrintTag  : " + wristBandPrintTag);

                if (line.LineValid
                    && line.card != null
                    && line.card.CardNumber.StartsWith("T"))
                {
                    if (wristBandPrintTag)
                    {
                        string cardNumber = "";
                        while (!newCardNumber)
                        {
                            cardNumber = KioskStatic.GetTagNumber();
                            AccountListBL accountBL = new AccountListBL(KioskStatic.Utilities.ExecutionContext);
                            List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                            accountSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TAG_NUMBER, newCardNumber.ToString()));
                            List<AccountDTO> accountListDTO = accountBL.GetAccountDTOList(accountSearchParameters, true, true);
                            if (accountListDTO == null || accountListDTO.Count == 0)
                            {
                                line.card.CardNumber = line.CardNumber = cardNumber;
                                newCardNumber = true;
                                KioskStatic.updateKioskActivityLog(-1, -1, cardNumber, "NEWCARD", line.ProductName.ToString(), ac);
                                Common.logToFile("New card number is generated : " + cardNumber);
                                log.Info("New card number is generated : " + cardNumber);
                            }
                        }
                    }
                    else
                    {
                        if (Properties.Settings.Default.CardDispenserPort == -1)
                        {
                            log.Info("Card dispenser is disabled by setting port number  =-1");
                            Common.logToFile("Card dispenser is disabled by setting port number  =-1");
                            Common.ShowMessage(Common.utils.MessageUtils.getMessage(2384));
                            return false;
                        }

                        string message = "";
                        int cardDispenseRetryCount = 3;
                        int cardNumberLength = 0;
                        try { cardNumberLength = Convert.ToInt32(Common.utils.getParafaitDefaults("CARD_NUMBER_LENGTH")); }
                        catch (Exception ex)
                        {
                            cardNumberLength = 0;
                            Common.logException(ex);
                        }
                        while (true)
                        {
                            string cardNumber = "";
                            message = "";

                            base.ResetTimeOut();
                            Common.logToFile("Call cardDispenser.doDispenseCard()"); // 
                            bool succ = cardDispenser.doDispenseCard(ref cardNumber, ref message);
                            Common.logToFile("cardNumber " + cardNumber); //
                            Common.logToFile("message " + message); //
                            if (!succ)
                            {
                                Common.logToFile("message 1:" + message);//
                                Common.ShowMessage(Common.utils.MessageUtils.getMessage(441) +
                                    Environment.NewLine + "Dispenser Error: " +
                                    message + Environment.NewLine +
                                    "Please fix the error and press Close to continue");
                            }
                            else if (string.IsNullOrEmpty(cardNumber))
                            {
                                Common.logToFile("Card Dispensed but not read. Rejecting");
                                Thread.Sleep(300);
                                Common.logToFile("Call cardDispenser.doRejectCard 1");//
                                if (!cardDispenser.doRejectCard(ref message))
                                {
                                    returnValue = false;
                                    Common.logToFile("message 2:" + message);//
                                    Common.ShowMessage(message);
                                    abortAndExit("Card dispenser error; Unable to reject card: " + message);
                                    return returnValue;
                                }
                            }
                            else if (!string.IsNullOrEmpty(cardNumber) && cardNumber.Length != cardNumberLength)
                            {
                                Common.logToFile("Issue while reading card number. Card number length is less than defined setup value. Rejecting " + cardNumber);
                                Thread.Sleep(300);
                                Common.logToFile("Call cardDispenser.doRejectCard 2");//
                                if (!cardDispenser.doRejectCard(ref message))
                                {
                                    returnValue = false;
                                    Common.logToFile("message 3:" + message);//
                                    Common.ShowMessage(message);
                                    abortAndExit("Card dispenser error; Unable to reject card: " + message);
                                    return returnValue;
                                }
                            }
                            else
                            {
                                foreach (Transaction.TransactionLine l in trx.TrxLines)
                                {
                                    if (line.card == l.card)
                                    {
                                        l.card.CardNumber = l.CardNumber = cardNumber;
                                    }
                                }
                                break;
                            }

                            cardDispenseRetryCount--;
                            if (cardDispenseRetryCount > 0)
                            {
                                Common.logToFile("Dispense Failed. Retrying [" + (3 - cardDispenseRetryCount).ToString() + "]");
                                if (readerDevice != null && cardScanCompleteEvent != null)
                                {
                                    readerDevice.UnRegister();
                                    readerDevice.Register(cardScanCompleteEvent);
                                }
                                else if (dispenserCardReader != null && cardScanCompleteEvent != null)
                                {
                                    dispenserCardReader.UnRegister();
                                    dispenserCardReader.Register(cardScanCompleteEvent);
                                }
                            }
                            else
                            {
                                string mes = "Unable to issue card after MAX retries. Contact Staff.";
                                Common.logToFile(mes);//
                                returnValue = false;
                                abortAndExit(mes + " " + message);
                                return returnValue;
                            }
                        }
                        Common.logToFile("Call cardDispenser.doRejectCard 2");//
                        cardDispenser.doEjectCard(ref message);

                        while (true)
                        {
                            Thread.Sleep(300);
                            int cardPosition = 0;
                            Common.logToFile("Call cardDispenser.checkStatus");//
                            if (cardDispenser.checkStatus(ref cardPosition, ref message))
                            {
                                if (cardPosition >= 2)
                                {
                                    message = Common.utils.MessageUtils.getMessage(393);
                                    Common.ShowMessage(message);
                                    Common.logToFile("Card dispensed. Waiting to be removed.");
                                }
                                else
                                {
                                    Common.logToFile("Card removed.");
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    } // else if not auto generate card product
                } // IF card starts with T

            }
            Common.logExit();
            return returnValue;
        }

        CardDispenser getCardDispenser(Transaction trx)
        {
            Common.logEnter();
            //int dispPort = Properties.Settings.Default.CardDispenserPort;
            //if (dispPort == -1)
            //{
            //    log.Info("Card dispenser is disabled by setting port number = -1");
            //    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1677));
            //    Common.logToFile("Card dispenser is disabled by setting port number = -1 ");
            //    return null;
            //}
            foreach (Transaction.TransactionLine line in trx.TrxLines)
            {
                if (line.LineValid
                    && line.CardNumber != null
                    && line.CardNumber.StartsWith("T"))
                {
                    int dispPort = Properties.Settings.Default.CardDispenserPort;
                    if (dispPort <= 0)
                    {
                        throw new ApplicationException("Card dispenser port not defined");
                    }

                    SerialPort spCardDispenser = new System.IO.Ports.SerialPort();

                    spCardDispenser.PortName = "COM" + dispPort.ToString();
                    spCardDispenser.BaudRate = 9600;

                    spCardDispenser.Parity = System.IO.Ports.Parity.None;
                    spCardDispenser.StopBits = System.IO.Ports.StopBits.One;
                    spCardDispenser.DataBits = 8;

                    spCardDispenser.Open();

                    CardDispenser cardDispenser = new K720(spCardDispenser);

                    int cardPosition = -1;
                    string mes = "";
                    bool suc = cardDispenser.checkStatus(ref cardPosition, ref mes);
                    string dispenserMessage = mes;
                    if (suc)
                    {
                        if (cardPosition == 3)
                        {
                            cardDispenser.dispenserWorking = false;
                            dispenserMessage = "Card at mouth positon. Please remove card.";
                            Common.logToFile(dispenserMessage);
                        }
                        else if (cardPosition == 2)
                        {
                            cardDispenser.dispenserWorking = false;
                            string message = "";
                            Common.logToFile("Card at read positon. Ejecting.");
                            cardDispenser.ejectCard(ref message);
                            Common.logToFile(message);
                            dispenserMessage = "There is a Card at Read position";
                        }
                    }
                    else
                    {
                        Common.logToFile(mes);
                        dispenserMessage = Common.utils.MessageUtils.getMessage(377) + ": " + mes;
                    }

                    if (cardDispenser.dispenserWorking == false)
                    {
                        spCardDispenser.Close();
                        throw new ApplicationException(dispenserMessage);
                    }

                    Common.logExit();
                    return cardDispenser;
                }
            }

            Common.logExit();
            return null;
        }

        public void CreditCardPayment(Transaction trx, TransactionPaymentsDTO transactionPaymentsDTO, bool IsDebitCard)
        {
            Common.logEnter();
            Helper.CreditCardPaymentModeDetails ccPaymentModeDetails = Helper.getCreditCardDetails();
            transactionPaymentsDTO.CreditCardAuthorization = "";
            transactionPaymentsDTO.CreditCardExpiry = "";
            transactionPaymentsDTO.CreditCardName = "";
            transactionPaymentsDTO.CreditCardNumber = "";
            transactionPaymentsDTO.Amount = trx.Net_Transaction_Amount;
            transactionPaymentsDTO.PaymentModeId = ccPaymentModeDetails.PaymentModeId;
            transactionPaymentsDTO.NameOnCreditCard = "";
            transactionPaymentsDTO.paymentModeDTO = new PaymentMode(executionContext, ccPaymentModeDetails.PaymentModeId).GetPaymentModeDTO;
            // transactionPaymentsDTO.PaymentMode = ccPaymentModeDetails.PaymentMode;

            if (Enum.IsDefined(typeof(PaymentGateways), ccPaymentModeDetails.Gateway))
                transactionPaymentsDTO.paymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), ccPaymentModeDetails.Gateway, true);
            else
                transactionPaymentsDTO.paymentModeDTO.GatewayLookUp = PaymentGateways.None;

            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(transactionPaymentsDTO.paymentModeDTO.GatewayLookUp);
            paymentGateway.IsCreditCard = !IsDebitCard;
            //paymentGateway.PrintReceipt = printReceipt;

            string message = "";
            if (CreditCardPaymentGateway.MakePayment(transactionPaymentsDTO, Common.utils, ref message))
            {
                Common.logToFile("Credit card payment success");
            }
            else
            {
                Common.logToFile(message);
                Common.logToFile("CC payment failure");

                throw new ApplicationException("Credit Card Payment failed");
            }
            Common.logExit();
        }

        public void cancelCCPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            Common.logEnter();
            if (transactionPaymentsDTO != null && transactionPaymentsDTO.GatewayPaymentProcessed)
            {
                string lclmessage = "";
                if (CreditCardPaymentGateway.RefundAmount(transactionPaymentsDTO, Common.utils, ref lclmessage))
                {
                    Common.logToFile("CC amount refunded: " + transactionPaymentsDTO.Amount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    RecordCardAbort(transactionPaymentsDTO);
                }
                else
                {
                    Common.logToFile("CC refund failed: " + transactionPaymentsDTO.Amount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                }
            }
            Common.logExit();
        }

        private void RecordCardAbort(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            Common.logEnter();
            if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount > 0)
            {
                KioskActivityLogDTO kioskActivityLogDTO = new KioskActivityLogDTO("N", ServerDateTime.Now, KioskStatic.ACTIVITY_TYPE_ABORT_CC,
                                       Convert.ToDouble(transactionPaymentsDTO.Amount),
                                       "", "Abort CC Payment", Common.utils.ParafaitEnv.POSMachineId,
                                      Common.utils.ParafaitEnv.POSMachine, "", false,
                                     transactionPaymentsDTO.TransactionId, 0, KioskStatic.GlobalKioskTrxId++, -1);
                KioskActivityLogBL kioskActivityLogBL = new KioskActivityLogBL(kioskActivityLogDTO);
                kioskActivityLogBL.Save();
            } 
            Common.logExit(); 
        }

        void scrollDown(int value = 10)
        {
            if (flpOrderSummary.Top + flpOrderSummary.Height > panelOrderSummary.Height)
            {
                flpOrderSummary.Top = flpOrderSummary.Top - value;
            }
        }

        void scrollUp(int value = 10)
        {
            if (flpOrderSummary.Top < 0)
                flpOrderSummary.Top = Math.Min(0, flpOrderSummary.Top + value);
        }
        System.Windows.Forms.Timer scrollTimer;
        //bool isTimerTriggered = false;
        private void btnScrollUp_MouseDown(object sender, MouseEventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            scrollTimer = new System.Windows.Forms.Timer();
            scrollTimer.Interval = 30;
            scrollTimer.Tick += delegate
            {
                scrollUp();
                //isTimerTriggered = true;
            };
            scrollTimer.Start();
            Common.logExit();
        }
        private void btnScrollUp_MouseUp(object sender, MouseEventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            if (scrollTimer != null) //&& isTimerTriggered)
            {
                scrollTimer.Stop();
                //isTimerTriggered = false;
            }
            Common.logExit();
        }

        private void btnScrollDown_MouseDown(object sender, MouseEventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            scrollTimer = new System.Windows.Forms.Timer();
            scrollTimer.Interval = 30;
            scrollTimer.Tick += delegate
            {
                scrollDown();
                // isTimerTriggered = true;
            };
            scrollTimer.Start();
            Common.logExit();
        }
        private void btnScrollDown_MouseUp(object sender, MouseEventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            if (scrollTimer != null)//&& isTimerTriggered)
            {
                scrollTimer.Stop();
                //isTimerTriggered = false;
            }
            Common.logExit();
        }

        private void btnViewOrderCheckOut_Click(object sender, EventArgs e)
        {
            base.ResetTimeOut();
            Common.logEnter();
            btnCheckout.PerformClick();
            Common.logExit();
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            base.ResetTimeOut();
            try
            {
                if (e.NewValue > e.OldValue)
                    scrollDown(e.NewValue - e.OldValue);
                else if (e.NewValue < e.OldValue)
                    scrollUp(e.OldValue - e.NewValue);
            }
            catch { }
        }

        private void frmCheckout_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 20) // ctrl-t
            {
                CheckOut("CASH");
            }
        }

        private void frmCheckout_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString().ToLower() == "right")
            {
                SendKeys.Send("{Esc}");
            }
        }



        void RecordCashAbort()
        {
            if (ac != null && ac.totalValue > 0)
            {
                //KioskStatic.logToFile(Common.utils.MessageUtils.getMessage(1399, ac.totalValue.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                //KioskStatic.updateKioskActivityLog(-1, -1, "", "ABORT", "Abort Recharge", ac);
                KioskActivityLogDTO kioskActivityLogDTO = new KioskActivityLogDTO("N", ServerDateTime.Now, "ABORT",
                                         Convert.ToDouble(ac.totalValue),
                                         "", "Abort Bill Payment", Common.utils.ParafaitEnv.POSMachineId,
                                        Common.utils.ParafaitEnv.POSMachine, "", false,
                                        ac.TrxId, 0, KioskStatic.GlobalKioskTrxId++, -1);
                KioskActivityLogBL kioskActivityLogBL = new KioskActivityLogBL(kioskActivityLogDTO);
                kioskActivityLogBL.Save();
                Common.logToFile("Abort recharge... Money entered: " + ac.totalValue.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                try
                {
                    Helper.PrintReceipt(true, ac);
                }
                catch (Exception ex)
                {
                    Common.logToFile("Error while printing receipt");
                    Common.logException(ex);
                }

                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1399, ac.totalValue.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                ac.totalValue = 0;
            }
        }

        //private bool RegisterDispenserKBWedge(USBDevice dispenserCardReader, EventHandler cardScanCompleteEvent)
        //{
        //    Common.logEnter();
        //    string USBReaderVID = Common.utils.getParafaitDefaults("CARD_DISPENSER_READER_VID").Trim();
        //    if (string.IsNullOrEmpty(USBReaderVID))
        //        USBReaderVID = Common.utils.getParafaitDefaults("USB_READER_VID");
        //    Common.logToFile("USBReaderVID: " + USBReaderVID); //
        //    string USBReaderPID = Common.utils.getParafaitDefaults("CARD_DISPENSER_READER_PID").Trim();
        //    if (string.IsNullOrEmpty(USBReaderPID))
        //        USBReaderPID = Common.utils.getParafaitDefaults("USB_READER_PID");
        //    Common.logToFile("USBReaderPID: " + USBReaderPID); //
        //    string USBReaderOptionalString = Common.utils.getParafaitDefaults("CARD_DISPENSER_READER_OPT_STRING");
        //    Common.logToFile("USBReaderOptionalString: " + USBReaderOptionalString); 
        //    if (IntPtr.Size == 4) //32 bit
        //        dispenserCardReader = new KeyboardWedge32();
        //    else
        //        dispenserCardReader = new KeyboardWedge64();

        //    foreach (string optString in USBReaderOptionalString.Split('|'))
        //    {
        //        if (string.IsNullOrEmpty(optString.Trim()))
        //            continue;
        //        Common.logToFile("optString: " + optString); //
        //        Common.logToFile("DispenserCardReader.InitializeUSBReader "); //
        //        bool flag = dispenserCardReader.InitializeUSBReader(this, USBReaderVID, USBReaderPID, optString.Trim());
        //        if (dispenserCardReader.isOpen)
        //        {
        //            Common.logToFile("DispenserCardReader.Register "); //
        //            dispenserCardReader.Register(cardScanCompleteEvent);
        //            //KioskStatic.DispenserReaderDevice = DispenserCardReader;
        //            return true;
        //        }
        //    }

        //    string msg = "Unable to find USB card reader for Card Dispenser. VID: " + USBReaderVID + ", PID: " + USBReaderPID + ", OPT: " + USBReaderOptionalString + " POS: " + Common.utils.ParafaitEnv.POSMachine;
        //    Common.logToFile(msg);
        //    Common.ShowMessage(msg + ". " + Common.utils.MessageUtils.getMessage(441));
        //    Common.logExit();
        //    return false;
        //}
    }
}

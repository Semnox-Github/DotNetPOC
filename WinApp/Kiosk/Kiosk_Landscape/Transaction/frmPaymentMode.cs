/********************************************************************************************
* Project Name - Parafait_Kiosk.frmPaymentMode
* Description  - frmPaymentMode 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Tax display
*2.6.0       30-Apr-2019      Nitin Pai          Adding deposit to kiosk
*2.80        5-Sep-2019       Deeksha            Added logger methods.
*2.90.0      23-Jun-2020      Dakshakh raj       Payment Modes based on Kiosk Configuration set up
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device.PaymentGateway;
using System.Data.SqlClient;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Customer.Accounts;
using System.Drawing;

namespace Parafait_Kiosk
{

    public partial class frmPaymentMode : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int paymentModeScrollIndex = 1;
        private string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        private string couponNumber = string.Empty;
        //private DiscountsSummaryDTO discountSummaryDTO = null;
        private List<POSPaymentModeInclusionDTO> pOSPaymentModeInclusionDTOList;
        //Semnox.Parafait.Transaction.Transaction _currentTrx;
        private List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = new List<KeyValuePair<string, ProductsDTO>>();
        private string AMOUNTFORMAT;
        private string AMOUNTFORMATWITHCURRENCYSYMBOL;
        private string IMAGEFOLDER;
        private string NUMBERFORMAT;
        private PaymentModeDTO usrselectedPaymentModeDTO = new PaymentModeDTO();
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines;
        private Semnox.Parafait.Transaction.Transaction.TransactionLine firstLine;
        private ExecutionContext executionContext;
        private bool showCartInKiosk = false;
        private const int flpCartPHeight = 585;
        private const int pnlProdSummaryHeight = 659;
        private int orginalXValueForFlpCartProducts;
        private double serviceCharges = 0;
        private double gratuityCharges = 0;
        private double totalCharges = 0;
        private string defaultMsg = string.Empty;
        private const bool cartElementsInReadMode = true;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        //internal PaymentModeDTO SelectedPaymentModeDTO { get { return selectedPaymentModeDTO; } }

        public frmPaymentMode(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            this.kioskTransaction = kioskTransaction;
            this.trxLines = kioskTransaction.GetActiveTransactionLines;
            this.firstLine = kioskTransaction.GetFirstNonDepositAndFnDTransactionLine;
            if (firstLine == null && trxLines != null && trxLines.Any())
            {
                firstLine = trxLines[0];
                selectedEntitlementType = kioskTransaction.GetSelectedEntitlementType;
            }
            executionContext = KioskStatic.Utilities.ExecutionContext;
            InitializeComponent();
            lblHeading.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;

            showCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_CART_IN_KIOSK", false);
            NUMBERFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            AMOUNTFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            AMOUNTFORMATWITHCURRENCYSYMBOL = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            IMAGEFOLDER = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY");

            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            txtMessage.Text = defaultMsg = MessageContainerList.GetMessage(executionContext, "Choose Payment Mode");
            int productId = -1;
            if (firstLine != null)
            {
                productId = firstLine.ProductID;
            }
            DisplayValidtyMsg(productId);
            orginalXValueForFlpCartProducts = this.flpCartProducts.Location.X;
            RefreshCartData();
            SetDiscountButtonText();
            DisplaybtnCancel(true);
            //DisplaybtnPrev(false);
            //DisplaybtnCart(showCartInKiosk);
            //RefreshCartIconText(NUMBERFORMAT);
            //lblSiteName.Text = KioskStatic.SiteHeading;

            if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_DISCOUNTS_IN_POS").Equals("N")
                || kioskTransaction.SelectedProductType == KioskTransaction.GETCHECKINCHECKOUTTYPE)
            {
                btnDiscount.Text = MessageContainerList.GetMessage(executionContext, btnDiscount.Text);
                btnDiscount.Visible = false;
                //btnDiscount.Location = new Point(panelSummary.Bottom + 5, 255);
            }
            //SetDiscountButtonText();
            pOSPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>(KioskStatic.pOSPaymentModeInclusionDTOList);
            DisplayPaymentModeOptions();
            SetCustomizedFontColors();
            log.LogMethodExit();
        }
        private void frmPaymentMode_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmPaymentMode_Load ");
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            AdjustflpLocation();
            if (KioskStatic.config.baport <= 0 && pOSPaymentModeInclusionDTOList == null)
            {
                frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(executionContext, 1257) + ". " + MessageContainerList.GetMessage(executionContext, 441));
                this.Close();
                log.LogMethodExit();
                return;
            }
            if (kioskTransaction.PreSelectedPaymentModeDTO != null && KioskStatic.Utilities.getParafaitDefaults("ENABLE_DISCOUNTS_IN_POS").Equals("N"))
            {
                if (KioskStatic.pOSPaymentModeInclusionDTOList != null)
                {
                    PaymentModeDTO userPreSelectedPaymentModeDTO = KioskStatic.pOSPaymentModeInclusionDTOList.Where(p => p.PaymentModeId == kioskTransaction.PreSelectedPaymentModeDTO.PaymentModeId).FirstOrDefault().PaymentModeDTO;
                    ProceedWithPayment(userPreSelectedPaymentModeDTO);
                }
            }
            log.LogMethodExit();
        }

        private void DisplayPaymentModeOptions()
        {
            log.LogMethodEntry();
            fLPPaymentModes.SuspendLayout();
            //changes for kiosk dynamic Payemnet modes
            List<PaymentModeDTO> paymentModeDTOList = new List<PaymentModeDTO>();
            bool found = false;
            if (pOSPaymentModeInclusionDTOList != null && pOSPaymentModeInclusionDTOList.Any())
            {
                if (pOSPaymentModeInclusionDTOList.Exists(p => p.PaymentModeDTO.IsCash == true))
                {
                    if (KioskStatic.config.baport > 0)
                    {
                        foreach (KioskStatic.configuration.acceptorInfo ai in KioskStatic.config.Notes)
                        {
                            if (ai != null)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            foreach (KioskStatic.configuration.acceptorInfo ai in KioskStatic.config.Coins)
                            {
                                if (ai != null)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (!found)
                        {
                            pOSPaymentModeInclusionDTOList.RemoveAll(p => p.PaymentModeDTO.IsCash == true);
                            if (pOSPaymentModeInclusionDTOList.Count == 1)
                            {
                                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 423);
                            }
                        }
                    }
                    else
                    {
                        pOSPaymentModeInclusionDTOList.RemoveAll(p => p.PaymentModeDTO.IsCash == true);
                    }
                    if (pOSPaymentModeInclusionDTOList.Count == 1 && found == false)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 423);
                    }
                }

                bool isPaymentModeDrivenSalesInKioskEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_PAYMENT_MODE_DRIVEN_SALES", false);
                List<POSPaymentModeInclusionDTO> userSelectedSPaymentModeInclusionDTOList = pOSPaymentModeInclusionDTOList;
                if (isPaymentModeDrivenSalesInKioskEnabled == true && kioskTransaction.PreSelectedPaymentModeDTO != null)
                {
                    POSPaymentModeInclusionDTO paymentModeInclusion = pOSPaymentModeInclusionDTOList.Where(p => p.PaymentModeId == kioskTransaction.PreSelectedPaymentModeDTO.PaymentModeId).FirstOrDefault();
                    if (paymentModeInclusion != null)
                    {
                        userSelectedSPaymentModeInclusionDTOList.Clear();
                        //userSelectedSPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
                        userSelectedSPaymentModeInclusionDTOList.Add(paymentModeInclusion);
                    }
                }
                foreach (POSPaymentModeInclusionDTO pOSPaymentModeInclusionDTO in userSelectedSPaymentModeInclusionDTOList)
                {

                    PaymentModeDTO paymentModeDTO = new PaymentModeDTO();
                    paymentModeDTO = pOSPaymentModeInclusionDTO.PaymentModeDTO;
                    Button btnPayment = new Button();
                    String btnText = "";

                    if (paymentModeDTO.IsCash == true && String.IsNullOrEmpty(paymentModeDTO.ImageFileName))
                    {
                        btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.CashButtonImage;
                        if (String.IsNullOrEmpty(pOSPaymentModeInclusionDTO.FriendlyName))
                        {
                            btnText = paymentModeDTO.PaymentMode;
                        }
                        else
                        {
                            btnText = pOSPaymentModeInclusionDTO.FriendlyName;
                        }
                        btnPayment.Text = btnText;
                    }
                    else if (paymentModeDTO.IsDebitCard == true && String.IsNullOrEmpty(paymentModeDTO.ImageFileName))
                    {
                        btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.GameCardButton;
                        if (String.IsNullOrEmpty(pOSPaymentModeInclusionDTO.FriendlyName))
                        {
                            btnText = paymentModeDTO.PaymentMode;
                        }
                        else
                        {
                            btnText = pOSPaymentModeInclusionDTO.FriendlyName;
                        }
                        btnPayment.Text = btnText;
                    }
                    else if (paymentModeDTO.IsCreditCard == true && String.IsNullOrEmpty(paymentModeDTO.ImageFileName))
                    {
                        btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.CreditCardButton;
                        if (String.IsNullOrEmpty(pOSPaymentModeInclusionDTO.FriendlyName))
                        {
                            btnText = paymentModeDTO.PaymentMode;
                        }
                        else
                        {
                            btnText = pOSPaymentModeInclusionDTO.FriendlyName;
                        }
                        btnPayment.Text = btnText;
                    }
                    else
                    {
                        try
                        {
                            object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                    new SqlParameter("@FileName", IMAGEFOLDER + "\\" + paymentModeDTO.ImageFileName));

                            btnPayment.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                            btnPayment.Text = "";
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            KioskStatic.logToFile(ex.Message + ": " + IMAGEFOLDER + "\\" + paymentModeDTO.ImageFileName);
                            if (paymentModeDTO.IsCash)
                            {
                                btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.CashButtonImage;
                            }
                            else if (paymentModeDTO.IsDebitCard)
                            {
                                btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.GameCardButton;
                            }
                            else if (paymentModeDTO.IsCreditCard)
                            {
                                btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.CreditCardButton;
                            }
                            else
                            {
                                btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.DebitCardButton;
                            }
                        }

                    }
                    btnPayment.Name = "btn" + btnText;
                    btnPayment.Tag = paymentModeDTO;
                    InitPaymentButtons(btnPayment);
                    if (paymentModeDTO.IsDebitCard == true && KioskStatic.Utilities.getParafaitDefaults("SHOW_DEBIT_CARD_BUTTON") == "N")
                    {
                        btnPayment.Visible = false;
                    }
                    else if (paymentModeDTO.IsDebitCard == true && btnPayment.Visible == true)
                    {
                        bool applyCardCreditPlusConsumption = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_APPLY_CARD_CREDITPLUS_CONSUMPTION", false);
                        if (kioskTransaction.SelectedProductType == KioskTransaction.GETCHECKINCHECKOUTTYPE && !applyCardCreditPlusConsumption)
                        {
                            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> allTrxLines = kioskTransaction.GetActiveTransactionLines;
                            int usedFreeEntriesFromMemberCard = (allTrxLines == null || allTrxLines.Any() == false) ? 0 : allTrxLines.FindAll(t => t.CreditPlusConsumptionId > -1).Count;
                            int availableFreeEntries = CheckCreditPlusBalance();
                            if (usedFreeEntriesFromMemberCard == 0 && availableFreeEntries > 0)
                            {
                                lblPassportCoupon.Visible = true;
                                string debitCardBtnName = string.IsNullOrWhiteSpace(btnText) ? "Debit Card" : btnText;
                                lblPassportCoupon.Text = MessageContainerList.GetMessage(executionContext, 4775, availableFreeEntries, debitCardBtnName);
                            }
                        }
                    }
                    fLPPaymentModes.Controls.Add(btnPayment);
                }
                Application.DoEvents();
                fLPPaymentModes.ResumeLayout();
                //this.fLPPaymentModes.VerticalScroll.Maximum = (Math.Max((fLPPaymentModes.Controls.Count + 1), 3) + 1) * ((fLPPaymentModes.Controls[0].Height + fLPPaymentModes.Controls[0].Margin.Top + fLPPaymentModes.Controls[0].Margin.Bottom) / 3); 
            }
            else
            {
                //Takes care of default payment mode as cash
                KioskStatic.logToFile("After checking inside if pOSPaymentModeInclusionDTOList == null  ");
                System.IO.Ports.SerialPort spBillAcceptor = new System.IO.Ports.SerialPort();
                spBillAcceptor.PortName = "COM" + KioskStatic.config.baport.ToString();
                spBillAcceptor.BaudRate = 9600;
                spBillAcceptor.DataBits = 8;
                spBillAcceptor.StopBits = System.IO.Ports.StopBits.One;
                spBillAcceptor.Parity = System.IO.Ports.Parity.Even;
                spBillAcceptor.RtsEnable = true;
                spBillAcceptor.WriteTimeout = spBillAcceptor.ReadTimeout = 2000;
                try
                {
                    KioskStatic.logToFile("Inside try blck ");
                    if (!spBillAcceptor.IsOpen)
                    {
                        KioskStatic.logToFile("opening the bill acceptor  ");
                        spBillAcceptor.Open();
                        KioskStatic.logToFile("bill acceptor opened  ");
                        KioskStatic.logToFile("Bill acceptor port " + KioskStatic.config.baport.ToString() + " opened");
                    }
                    if (spBillAcceptor.IsOpen)
                    {
                        KioskStatic.logToFile("Before closing bill acceptor  ");
                        spBillAcceptor.Close();
                        KioskStatic.logToFile("After closing bill acceptor  ");
                    }

                    List<PaymentModeDTO> cashPaymentModeDTOList = null;
                    cashPaymentModeDTOList = KioskStatic.paymentModeDTOList;
                    if (cashPaymentModeDTOList != null && cashPaymentModeDTOList.Any())
                    {
                        PaymentModeDTO paymentModeCDTO = cashPaymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                        if (paymentModeCDTO != null)
                        {
                            //selectedFundsAndDonationsList = GetSelectedFundsAndDonationsList();
                            //if (selectedFundsAndDonationsList != null && selectedFundsAndDonationsList.Any())
                            //{
                            //    kioskTransaction.AddDonationOrFundraiserProducts(selectedFundsAndDonationsList);
                            //}
                            CallTransactionForm(paymentModeCDTO);
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                            Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing frmPaymentMode_Load()" + ex.Message);
                    KioskStatic.logToFile("Error Opening Bill acceptor port " + KioskStatic.config.baport.ToString());
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 423);
                    frmOKMsg.ShowUserMessage(txtMessage.Text + ". " + MessageContainerList.GetMessage(executionContext, 441));
                    this.Close();
                }
            }
            log.LogMethodExit();
        }

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the screen flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }
        private void InitPaymentButtons(Button btnPayment)
        {
            log.LogMethodEntry();
            btnPayment.Margin = new System.Windows.Forms.Padding(34, 10, 0, 15);
            btnPayment.BackColor = System.Drawing.Color.Transparent;
            btnPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnPayment.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            btnPayment.FlatAppearance.BorderSize = 0;
            btnPayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnPayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Microsoft Sans Serif");
            btnPayment.Font = new System.Drawing.Font(fontFamName, 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnPayment.ForeColor = System.Drawing.Color.White;
            btnPayment.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            btnPayment.Location = new System.Drawing.Point(10, 10);
            btnPayment.Size = new System.Drawing.Size(180, 155);
            btnPayment.TabIndex = 12;
            //btnPayment.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            btnPayment.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.PaymentModeButtonTextAlignment); 
            btnPayment.UseVisualStyleBackColor = false;
            btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            btnPayment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            btnPayment.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            log.LogMethodExit();
        }
        private void frmPaymentMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timeout.Stop();
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
        }

        //Dynamic paymentmodes changes
        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
        }
        private void btnPayment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisablePaymentButtons(); 
                Button btnPayment = sender as Button;

                usrselectedPaymentModeDTO = (PaymentModeDTO)btnPayment.Tag;
                //selectedFundsAndDonationsList = GetSelectedFundsAndDonationsList();

                //if (DialogResult != DialogResult.Cancel)
                //{
                //    //string message = "";
                //    if (selectedFundsAndDonationsList != null && selectedFundsAndDonationsList.Any())
                //    {
                //        //if (_currentTrx == null)
                //        //{
                //        //    _currentTrx = new Semnox.Parafait.Transaction.Transaction(KioskStatic.Utilities);
                //        //    _currentTrx.PaymentReference = "Kiosk Transaction";
                //        //    KioskStatic.logToFile("New Trx object created for fund raiser and donation products");
                //        //}
                //        kioskTransaction.AddDonationOrFundraiserProducts(selectedFundsAndDonationsList);
                //        //foreach (KeyValuePair<string, ProductsDTO> keyValuePair in selectedFundsAndDonationsList)
                //        //{
                //        //    if (_currentTrx.createTransactionLine(_rechargeCard, keyValuePair.Value.ProductId, Convert.ToDouble(keyValuePair.Value.Price), 1, ref message) != 0)
                //        //    {
                //        //        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Error") + ": " + message;
                //        //        KioskStatic.logToFile("Error TrxLine1: " + message);
                //        //    }
                //        //}
                //    }
                //    CallTransactionForm(usrselectedPaymentModeDTO);
                //}
                ProceedWithPayment(usrselectedPaymentModeDTO);
                if (DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing btnPayment_Click", ex);
                KioskStatic.logToFile("Error on btnPayment Click: " + ex.Message);
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Sorry unexpected error");
                frmOKMsg.ShowUserMessage(txtMessage.Text);
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Choose Payment Mode");
            }
            finally
            {
                EnablePaymentButtons();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        public void ProceedWithPayment(PaymentModeDTO paymentModeDTO)
        {
            log.LogMethodEntry();

            //selectedFundsAndDonationsList = GetSelectedFundsAndDonationsList();
            if (DialogResult != DialogResult.Cancel)
            {
                //if (selectedFundsAndDonationsList != null && selectedFundsAndDonationsList.Any())
                //{
                //    kioskTransaction.AddDonationOrFundraiserProducts(selectedFundsAndDonationsList);
                //}
                CallTransactionForm(paymentModeDTO);
            }
            log.LogMethodExit();
        }

        private void CallTransactionForm(PaymentModeDTO paymentModeDTO)
        {
            log.LogMethodEntry(paymentModeDTO);
            KioskStatic.logToFile("callTransactionForm(): PaymentMode: " + paymentModeDTO.PaymentMode);
            ResetKioskTimer();
            StopKioskTimer();
            //if (paymentModeDTO != null && paymentModeDTO.IsCash == true)
            //{
            //    if (ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString()))
            //    {
            //        string errMsg = "Smartro VCAT is not supported for Cash Payments in Kiosk";
            //        frmOKMsg.ShowOkMessage(errMsg, true);
            //        return;
            //    }
            //    string option = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "TRX_AUTO_PRINT_AFTER_SAVE");
            //    KioskStatic.logToFile("TRX_AUTO_PRINT_AFTER_SAVE: " + option);
            //    bool printTrxReceipt = true;
            //    if (option.Equals("N"))
            //    {
            //        printTrxReceipt = false;
            //    }
            //    else if (option.Equals("A"))
            //    {
            //        string message = "";
            //        message = MessageContainerList.GetMessage(executionContext, 933);
            //        using (frmYesNo fyn = new frmYesNo(message, ""))
            //        {
            //            DialogResult frmYesNoResult = fyn.ShowDialog();
            //            txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Loading payment screen. Please wait...");
            //            if (frmYesNoResult == DialogResult.No)
            //            {
            //                printTrxReceipt = false;
            //            }
            //            else if (frmYesNoResult == DialogResult.Cancel)
            //            {
            //                DialogResult = DialogResult.OK;
            //                log.LogMethodExit("frmYesNoResult == DialogResult.Cancel");
            //                return;
            //            }
            //        }
            //    }
            //    //else
            //    //{
            //    using (frmCashTransaction frm = new frmCashTransaction(kioskTransaction, kioskTransaction.SelectedProductType, _prowProduct, _rechargeCard, customerDTO, paymentModeDTO,
            //                                                           0, printTrxReceipt, selectedEntitlementType,
            //                                                           discountSummaryDTO == null ? null : discountSummaryDTO,
            //                                                           couponNumber == string.Empty ? null : couponNumber, null, activeFundsDonationsDTO))
            //    {

            //        frm.ShowDialog();
            //        kioskTransaction = frm.GetKioskTransaction;
            //        txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Choose Payment Mode");
            //        DialogResult = DialogResult.OK;
            //    }
            //    //}
            //}
            if (paymentModeDTO != null && paymentModeDTO.IsDebitCard == true)
            {
                TransactionPaymentsDTO transactionPaymentsDTO = null;
                using (Transaction.frmPaymentGameCard frmg = new Transaction.frmPaymentGameCard(kioskTransaction, paymentModeDTO))
                {
                    frmg.ShowDialog();
                    if (frmg.debitTrxPaymentDTO != null)
                    {
                        transactionPaymentsDTO = frmg.debitTrxPaymentDTO;
                    }

                    if (frmg.DialogResult != DialogResult.Cancel)
                    {
                        DialogResult = frmg.DialogResult;
                    }

                    if(frmg.DialogResult == DialogResult.Cancel)
                    {
                        RefreshCartData();
                    }
                    kioskTransaction = frmg.GetKioskTransaction;
                    //else
                    //{
                    //    if (_currentTrx != null)
                    //    {
                    //        CancelFundAndDonationTrxLines();
                    //    }
                    //}
                }
            }
            else if (paymentModeDTO != null)
            {
                if (paymentModeDTO.IsCash == true)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString()))
                    {
                        string errMsg = "Smartro VCAT is not supported for Cash Payments in Kiosk";
                        frmOKMsg.ShowOkMessage(errMsg, true);
                        KioskStatic.logToFile(errMsg);
                        log.LogMethodExit();
                        return;
                    }
                    bool getConfirmationForCashPay = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "NEED_CONFIRMATION_FOR_CASH_PAYMENT", false);
                    if (getConfirmationForCashPay)
                    {
                        //"No refund and No change will be given for Cash payment. Surplus amount will be loaded on to the card as credits. Do you want to proceed?
                        string alertMsg = MessageContainerList.GetMessage(executionContext, 4872);
                        using (frmYesNo fYN = new frmYesNo(alertMsg))
                        {
                            if (fYN.ShowDialog() != DialogResult.Yes)
                            {
                                RefreshCartData();
                                KioskStatic.logToFile("User didnt provide confirmation to proceed with cash payment");
                                log.LogMethodExit();
                                return;
                            }
                        }
                    }
                }
                using (frmCardTransaction frm = new frmCardTransaction(kioskTransaction, paymentModeDTO))
                {
                    DialogResult dr = frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    if (dr != DialogResult.Cancel)
                    {
                        DialogResult = frm.DialogResult;
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        RefreshCartData();
                    }
                    //else
                    //{
                    //    if (_currentTrx != null)
                    //    {
                    //        CancelFundAndDonationTrxLines();
                    //    }
                    //}
                }

            }
            log.LogMethodExit();
        }
        private void DisplayValidtyMsg(int productId)
        {
            log.LogMethodEntry(productId);
            //if (kioskTransaction.ShowCartInKiosk == false)
            //{
                string validityMsg = "";
                int lineCount = 0;
                if (productId > -1)
                {
                    validityMsg = KioskStatic.GetProductCreditPlusValidityMessage(productId, selectedEntitlementType);
                    lineCount = Regex.Split(validityMsg, "\r\n").Count();
                }
                if (lineCount > 2)
                {
                    //lblProductCPValidityMsg.BorderStyle = BorderStyle.FixedSingle;
                    lblProductCPValidityMsg.AutoEllipsis = true;
                    string[] validityMsgList = Regex.Split(validityMsg, "\r\n");
                    lblProductCPValidityMsg.Text = validityMsgList[0] + "\r\n" + validityMsgList[1];
                    System.Windows.Forms.ToolTip validtyMsgToolTip = new ToolTip();
                    validtyMsgToolTip.SetToolTip(lblProductCPValidityMsg, validityMsg);
                }
                else
                {
                    lblProductCPValidityMsg.Text = validityMsg;
                }
                AdjustProductSummaryPanel();
            //}
            //else
            //{
            //    lblProductCPValidityMsg.Visible = false;
            //}
            log.LogMethodExit();
        }
        private void AdjustProductSummaryPanel()
        {
            log.LogMethodEntry();
            //int heightAdjFactor = lblProductCPValidityMsg.Height + 3;
            //pnlProductSummary.SuspendLayout();
            //flpCartProducts.SuspendLayout();
            //flpCartProducts.Size = new Size(flpCartProducts.Width, flpCartPHeight - heightAdjFactor);
            //bigVerticalScrollCartProducts.Size = new Size(bigVerticalScrollCartProducts.Width, flpCartPHeight - heightAdjFactor);
            //pnlProductSummary.Size = new Size(pnlProductSummary.Width, pnlProdSummaryHeight - heightAdjFactor);
            //flpCartProducts.ResumeLayout(true);
            //pnlProductSummary.ResumeLayout(true);
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmPaymentMode");
            try
            {
                foreach (Control c in fLPPaymentModes.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PaymentModeButtonsTextForeColor;//Payment options buttons
                    }
                }

                this.lblHeading.ForeColor = KioskStatic.CurrentTheme.PaymentModeLblHeadingTextForeColor;
                //this.lblCardnumber.ForeColor = KioskStatic.CurrentTheme.PaymentModeLblCardnumberTextForeColor;
                //this.lblPackage.ForeColor = KioskStatic.CurrentTheme.PaymentModeLblPackageTextForeColor;
                //this.btnPrev.ForeColor = KioskStatic.CurrentTheme.PaymentModeBackButtonTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.PaymentModeCancelButtonTextForeColor;//Cancel button
                //this.lblTax.ForeColor = KioskStatic.CurrentTheme.PaymentModeTotalToPayHeaderTextForeColor;//Total to pay header label
                //this.lblDiscount.ForeColor = KioskStatic.CurrentTheme.PaymentModeDiscountLabelTextForeColor;//Coupon button
                this.btnDiscount.ForeColor = KioskStatic.CurrentTheme.PaymentModeDiscountBtnTextForeColor;//Coupon button
                //this.lblPriceOfCardDeposit.ForeColor = KioskStatic.CurrentTheme.PaymentModeLblPriceOfCardDepositTextForeColor;
                this.lblProductCPValidityMsg.ForeColor = KioskStatic.CurrentTheme.PaymentModeCPValidityTextForeColor;//product cp validity
                //this.lblTotalToPay.ForeColor = KioskStatic.CurrentTheme.PaymentModeTotalToPayInfoTextForeColor;//Total to pay info label
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.PaymentModeTextMessageTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.PaymentModeBtnHomeTextForeColor;
                this.lblPassportCoupon.ForeColor = KioskStatic.CurrentTheme.PaymentModeLblPassportCouponTextForeColor;//Passport Coupon Label

                this.lblTotalHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblChargeAmountHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblTaxesHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblDiscountAmountHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblGrandTotalHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;

                this.lblDiscountInfo.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblCharges.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblDiscount.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblGrandTotal.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblTax.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblTotal.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;


                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                //btnPrev.BackgroundImage = 
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnDiscount.BackgroundImage = ThemeManager.CurrentThemeImages.DiscountButton;
                //this.btnCart.SetCartImage(KioskStatic.CurrentTheme.KioskCartIcon);
                //this.btnCart.SetFont(this.btnHome.Font);
                //this.btnCart.SetForeColor(this.btnHome.ForeColor);

                this.pnlProductSummary.BackgroundImage = ThemeManager.CurrentThemeImages.TablePurchaseSummary;
                this.bigVerticalScrollCartProducts.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.bigVerticalScrollpaymentModes.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmPaymentMode: " + ex.Message);
            }
            log.LogMethodExit();
        } 
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                btnHome.PerformClick();
            }
            catch (Exception ex)
            {
                log.Error("Errow in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        }
        //public override void btnCart_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    KioskStatic.logToFile("Cart button clicked");
        //    try
        //    {
        //        base.LaunchCartForm(NUMBERFORMAT);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        frmOKMsg.ShowUserMessage(ex.Message);
        //    }
        //    log.LogMethodExit();
        //}

        private void DisablePaymentButtons()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Disable Payment Buttons");
                fLPPaymentModes.SuspendLayout();
                List<Button> buttonList = fLPPaymentModes.Controls.OfType<Button>().ToList();
                if (buttonList != null && buttonList.Any())
                {
                    for (int j = buttonList.Count - 1; j >= 0; j--)
                    {
                        buttonList[j].Enabled = false;
                    }
                    //buttonList[0].Focus();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                fLPPaymentModes.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private void EnablePaymentButtons()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Enable Payment Buttons");
                fLPPaymentModes.SuspendLayout();
                List<Button> buttonList = fLPPaymentModes.Controls.OfType<Button>().ToList();
                if (buttonList != null && buttonList.Any())
                {
                    for (int j = buttonList.Count - 1; j >= 0; j--)
                    {
                        buttonList[j].Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                fLPPaymentModes.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private int CheckCreditPlusBalance()
        {
            log.LogMethodEntry();
            int consumptionBalance = 0;

            try
            {
                Card card = kioskTransaction.GetTransactionPrimaryCard;
                AccountBL accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, card.card_id);
                int productId = (firstLine != null ? firstLine.ProductID : -1);
                if (accountBL != null && productId > -1)
                {
                    ProductsContainerDTO selectedProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.SiteId, productId);
                    List<ProductsContainerDTO> productsContainerDTOList = new List<ProductsContainerDTO>(selectedProductContainerDTO.ProductId);
                    if (selectedProductContainerDTO.ProductType == ProductTypeValues.COMBO)
                    {
                        if (selectedProductContainerDTO.ComboProductContainerDTOList != null && selectedProductContainerDTO.ComboProductContainerDTOList.Any())
                        {
                            foreach (ComboProductContainerDTO comboProductContainerDTO in selectedProductContainerDTO.ComboProductContainerDTOList)
                            {
                                ProductsContainerDTO childProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, comboProductContainerDTO.ChildProductId);
                                productsContainerDTOList.Add(childProductsContainerDTO);
                            }
                        }
                    }
                    else
                    {
                        productsContainerDTOList.Add(selectedProductContainerDTO);
                    }
                    consumptionBalance = accountBL.GetApplicableCardCPConsumptionsBalance(productsContainerDTOList);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while fetching Card Credit Plus Consumption balance in Paymnet screen: " + ex.Message);
            }
            log.LogMethodExit(consumptionBalance);
            return consumptionBalance;
        }

        public void GroupAndDisplayTrxLines(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines)
        {
            log.LogMethodEntry(trxLines);
            try
            {
                this.flpCartProducts.SuspendLayout();
                this.flpCartProducts.Controls.Clear();
                //List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> displayTrxLines = KioskHelper.GroupDisplayTrxLines(trxLines);
                List<List<KioskHelper.LineData>> displayTrxLineData = KioskHelper.GroupDisplayTrxLinesNew(trxLines);
                List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> displayTrxLines = KioskHelper.ConvertTrxLineData(displayTrxLineData);
                AddLinesToDisplay(displayTrxLines);
                //AddNonCardLinesToDisplay(displayTrxLines);
                AddChargesToDisplay();
                // display trx total
                double subTotalAmount = KioskHelper.GetSubTotalForDisplay(kioskTransaction);
                lblTotal.Text = (subTotalAmount).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                // display tax 
                lblTax.Text = (kioskTransaction.GetTrxTaxAmount()).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                AddDiscountDetailsToDisplay();
                // display grand total
                lblGrandTotal.Text = (kioskTransaction.GetTrxNetAmount()).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                //AdjustflpSize();
                AdjustflpLocation();
            }
            finally
            {
                this.flpCartProducts.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
        private void AddLinesToDisplay(List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> groupedTrxLines)
        {
            log.LogMethodEntry();
            for (int i = 0; i < groupedTrxLines.Count; i++) // display card lines
            {
                if (groupedTrxLines[i].Exists(tl => tl.LineValid))
                {
                    List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxCardProducts = groupedTrxLines[i];
                    usrCtrlCart usrCtrlCart = CreateUsrCtlElement(trxCardProducts);
                    this.flpCartProducts.Controls.Add(usrCtrlCart);
                }
            }
            log.LogMethodExit();
        }
        private void AdjustflpLocation()
        {
            log.LogMethodEntry();
            int xVal = orginalXValueForFlpCartProducts;
            if (bigVerticalScrollCartProducts.Visible == false)
            {
                int adjustFact = (bigVerticalScrollCartProducts.Width / 2) + 4;
                xVal = orginalXValueForFlpCartProducts + adjustFact;
            }
            this.lblCartProductName.Location = new Point(xVal, lblCartProductName.Location.Y);
            this.lblCartProductTotal.Location = new Point(xVal + this.lblCartProductName.Width + 2, lblCartProductTotal.Location.Y);
            this.flpCartProducts.Location = new Point(xVal, flpCartProducts.Location.Y);
            log.LogMethodExit();
        }
        private void AddChargesToDisplay()
        {
            log.LogMethodEntry();
            // display charges
            serviceCharges = kioskTransaction.GetTransactionServiceCharges();
            gratuityCharges = kioskTransaction.GetTransactionGratuityAmount();
            totalCharges = 0;
            totalCharges = serviceCharges + gratuityCharges;
            lblCharges.Text = totalCharges.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            log.LogMethodExit();
        }
        private void AddDiscountDetailsToDisplay()
        {
            log.LogMethodEntry();
            double discountAmount = kioskTransaction.GetTransactionDiscountsAmount();
            lblDiscount.Text = discountAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            log.LogMethodExit();
        }
        private void SetDiscountButtonText()
        {
            log.LogMethodEntry();
            DiscountsSummaryDTO discountsSummaryDTO = kioskTransaction.GetAppliedCouponDiscountSummary();
            SetBtnDiscountText(discountsSummaryDTO);
            log.LogMethodExit();
        }
        private void SetBtnDiscountText(DiscountsSummaryDTO discountsSummaryDTO)
        {
            log.LogMethodEntry();
            this.lblDiscountInfo.Text = string.Empty;
            if (discountsSummaryDTO != null)
            {
                lblDiscount.Text = discountsSummaryDTO.DiscountAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                double subTotalAmount = KioskHelper.GetSubTotalForDisplay(kioskTransaction);
                lblTotal.Text = (subTotalAmount).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                btnDiscount.Text = MessageContainerList.GetMessage(executionContext, "Remove Coupon");
                btnDiscount.Tag = "cancelDiscount";
                couponNumber = discountsSummaryDTO.CouponNumbers.First();
                string part1 = MessageContainerList.GetMessage(executionContext, "Discount") + ": ";
                string part2 = " " + MessageContainerList.GetMessage(executionContext, "applied");
                lblDiscountInfo.Text = part1 + discountsSummaryDTO.DiscountName + part2;
            }
            log.LogMethodExit();
        }

        private usrCtrlCart CreateUsrCtlElement(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines)
        {
            log.LogMethodEntry(trxLines);
            ResetKioskTimer();
            usrCtrlCart usrCtrlElement = null;
            try
            {
                usrCtrlElement = new usrCtrlCart(executionContext, trxLines, cartElementsInReadMode);
                //usrCtrlElement.CancelSelectedLines += new usrCtrlCart.cancelSelectedLines(CancelSelectedLines);
                usrCtrlElement.RefreshCartData += new usrCtrlCart.refreshCartData(RefreshCartData);
            }
            catch (Exception ex)
            {
                string msg = "Error in CreateUsrCtlElement() of cart screen";
                log.Error(msg);
                KioskStatic.logToFile(msg + " : " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit(usrCtrlElement);
            return usrCtrlElement;
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Text = defaultMsg;
                ResetKioskTimer();
                StopKioskTimer();
                if (btnDiscount.Tag.ToString() == "applyDiscount")
                {
                    couponNumber = string.Empty;
                    using (frmScanCoupon frm = new frmScanCoupon(kioskTransaction))
                    {
                        DialogResult dr = frm.ShowDialog();
                        kioskTransaction = frm.GetKioskTransaction;
                        if (dr != System.Windows.Forms.DialogResult.Cancel)
                        {
                            couponNumber = frm.CodeScaned;
                            DiscountsSummaryDTO discountsSummaryDTO = kioskTransaction.GetAppliedDiscountSummaryForCoupon(couponNumber);
                            if (discountsSummaryDTO != null)
                            {
                                SetBtnDiscountText(discountsSummaryDTO);
                                RefreshCartData();
                            }
                        }
                    }
                }
                else if (btnDiscount.Tag.ToString() == "cancelDiscount")
                {
                    double subTotalAmountBefore = KioskHelper.GetSubTotalForDisplay(kioskTransaction);
                    lblTotal.Text = (subTotalAmountBefore).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                    try
                    {
                        kioskTransaction.RemoveDiscountCoupon(couponNumber);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while executing btnDiscount_Click()" + ex.Message);
                        KioskStatic.logToFile(ex.Message);
                        txtMessage.Text = ex.Message;
                    }
                    double subTotalAmountAfter = KioskHelper.GetSubTotalForDisplay(kioskTransaction);
                    lblTotal.Text = (subTotalAmountAfter).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                    btnDiscount.Text = MessageContainerList.GetMessage(executionContext, "Apply Coupon");
                    btnDiscount.Tag = "applyDiscount";
                    lblDiscountInfo.Text = string.Empty;
                    RefreshCartData();
                }
            }
            finally
            {
                ResetKioskTimer();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void RefreshCartData()
        {
            log.LogMethodEntry();
            try
            {
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> transactionLines = kioskTransaction.GetActiveTransactionLines;
                GroupAndDisplayTrxLines(transactionLines);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Refreshing Cart Data", ex);
                KioskStatic.logToFile("Errow while Refreshing Cart Data: " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Back button pressed");
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        } 
        //private void DisplaybtnPrev(bool switchValue)
        //{
        //    log.LogMethodEntry(switchValue);
        //    this.btnPrev.Visible = switchValue;
        //    log.LogMethodExit();
        //}
        private void DisplaybtnCancel(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnCancel.Visible = switchValue;
            log.LogMethodExit();
        }
    }
}

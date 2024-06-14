/********************************************************************************************
 * Project Name - Parafait Kiosk- frmCashInsert 
 * Description  - frmCashInsert
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       28-Sep-2018      Guru S A           Modified for Online Transaction in Kiosk changes
 *2.80        05-Sep-2019      Deeksha            Added logger methods.
 *2.80        11-Nov-2019      Girish Kundar      Modified: Ticket printer integration
 *2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
 *2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
 *2.140.0     24-Nov-2021      Sathyavathi        CEC enhancement - Fund Raiser and Donations Reconciliation
 *2.130.9     12-Jun-2022      Sathyavathi        Removed hard coded amount/number format
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 *2.155.0     22-Jun-2023      Sathyavathi        Attraction Sale in Kiosk
 *2.150.7     10-Nov-2023      Sathyavathi        Customer Lookup Enhancement
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer;

namespace Parafait_Kiosk
{
    public partial class frmCashInsert : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private KioskTransaction kioskTransaction; 
        private string amountFormatWithCurrencySymbol;
        private int noteDenominatorReceived;
        private CommonBillAcceptor billAcceptor;
        private string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        private Utilities Utilities = KioskStatic.Utilities; 
        private List<PaymentModeDTO> paymentModeDTOList = null;
        private string selectedLoyaltyCardNo = ""; 
        private List<KeyValuePair<string, ProductsDTO>> selectedFundsList = new List<KeyValuePair<string, ProductsDTO>>(); 

        public frmCashInsert(string entitlementType, int noteDenominatorReceived)
        {
            log.LogMethodEntry(entitlementType, noteDenominatorReceived);
            this.noteDenominatorReceived = noteDenominatorReceived;
            Utilities.setLanguage();
            InitializeComponent();
            selectedEntitlementType = entitlementType;
            kioskTransaction = new KioskTransaction(KioskStatic.Utilities);
            kioskTransaction.AddCashNotePayment(noteDenominatorReceived);
            amountFormatWithCurrencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            InitializeForm();
            KioskStatic.setDefaultFont(this);
            this.ShowInTaskbar = false;

            billAcceptor = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
            billAcceptor.setReceiveAction = HandleBillAcceptorEvent; 
            lblCashInserted.Text = kioskTransaction.GetCashPaymentAmount().ToString(amountFormatWithCurrencySymbol);
            SetCustomizedFontColors();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            txtMessage.Text = label5.Text;
            KioskStatic.logToFile("Entering direct cash insert...");
            Utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void HandleBillAcceptorEvent(int noteDenominatorReceived)
        {
            log.LogMethodEntry(noteDenominatorReceived);
            kioskTransaction.AddCashNotePayment(noteDenominatorReceived); 
            lblCashInserted.Text = kioskTransaction.GetCashPaymentAmount().ToString(amountFormatWithCurrencySymbol);
            txtMessage.Text = label5.Text;
            log.LogMethodExit();
        }

        private void frmCashInsert_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            billAcceptor.setReceiveAction = null;
            KioskStatic.logToFile("Exiting direct cash insert...");
            Audio.Stop();
            log.LogMethodExit();
        }

        private void InitializeForm()
        {
            log.LogMethodEntry();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CashInsertBackgroundImage);
                btnNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonBig;
                btnRecharge.BackgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonBig;
                btnNewCard.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.DirectCashButtonTextAlignment);
                btnRecharge.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.DirectCashButtonTextAlignment); 
                panel2.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                panelGrid.BackgroundImage = ThemeManager.CurrentThemeImages.RedeemTable;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing initializeForm()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmCashInsert_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Enter CashInsert screen");
            lblSiteName.Text = KioskStatic.SiteHeading; 
            
            Application.DoEvents();
            txtMessage.Text = label5.Text;
            log.LogMethodExit();
        }

        protected override CreateParams CreateParams
        {
            //this method is used to avoid flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        private void btnNewCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnNewCard_Click");
            bool timeoutClose = false;
            try
            {
                DisableButtons();
                billAcceptor.Stop();

                txtMessage.Text = label5.Text;
                if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                {
                    KioskStatic.logToFile("TIME_IN_MINUTES_PER_CREDIT is greater than 0");
                    if (KioskHelper.isTimeEnabledStore() == true)
                    {
                        selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                    }
                    else
                    {
                        using (frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345)))
                        {
                            if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                frmEntitle.Dispose();
                                log.LogMethodExit();
                                return;
                            }
                            selectedEntitlementType = frmEntitle.selectedEntitlement;
                            frmEntitle.Dispose();
                        }
                    }
                }
                btnRecharge.Enabled = btnNewCard.Enabled = false;
                bool autoGeneratedCardNumber = false;
                DataTable dt = KioskStatic.getProductDetails(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId);
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[0]["price"] = kioskTransaction.GetTotalPaymentsReceived();//billAcceptor.GetAcceptance().totalValue;
                    dt.Rows[0]["CardCount"] = 2; // more than 1 to specify that MultipleCardsInSingleProduct to true in frmCardTransaction
                    autoGeneratedCardNumber = dt.Rows[0]["AutoGenerateCardNumber"].ToString() == "N" || dt.Rows[0]["AutoGenerateCardNumber"] == DBNull.Value ? false : true;
                }
                else
                {
                    KioskStatic.logToFile("uanble to fetch record for variable product id: " + KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId.ToString());
                    throw new Exception(KioskStatic.Utilities.MessageUtils.getMessage(1669));
                }

                bool isRegisteredCustomerOnly = (dt.Rows[0]["RegisteredCustomerOnly"].ToString() == "Y") ? true : false;
                bool showMsgRecommendCustomerToRegister = true;
                bool isLinked = CustomerStatic.LinkCustomerToTheTransaction(kioskTransaction, KioskStatic.Utilities.ExecutionContext, isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(), isRegisteredCustomerOnly, isLinked))
                {
                    log.LogMethodExit();
                    return;
                }

                if (KioskStatic.config.dispport == -1 && autoGeneratedCardNumber == false)
                {
                    log.Info("Card dispenser is disabled , dispport == -1");
                    KioskStatic.logToFile("Card dispenser is disabled , dispport == -1");
                    frmOKMsg.ShowUserMessage(KioskStatic.Utilities.MessageUtils.getMessage(2384));
                    KioskStatic.logToFile("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                    return;
                }
                //KioskStatic.acceptance acc = billAcceptor.GetAcceptance();//temp code
                //AddCashPaymentDetails(acc);
                paymentModeDTOList = KioskStatic.paymentModeDTOList;
                if (paymentModeDTOList != null && paymentModeDTOList.Any())
                {
                    selectedFundsList = GetSelectedFundsList();
                    CreateTrxLinesForFundProduct();
                    // less than 2$ inserted, go to transaction with cardcount = 1
                    if (kioskTransaction.GetTotalPaymentsReceived() < 2)
                    {
                        PaymentModeDTO paymentModeCDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                        kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, dt.Rows[0], 1, selectedEntitlementType);
                        CheckForLoyalty();
                        using (frmCardTransaction frm = new frmCardTransaction(kioskTransaction, paymentModeCDTO))
                        {
                            frm.ShowDialog();
                            kioskTransaction = frm.GetKioskTransaction;
                        }
                    }
                    else
                    {
                        using (Home.frmCardCountBasic ccb = new Home.frmCardCountBasic(kioskTransaction, dt.Rows[0], kioskTransaction.GetTotalPaymentsReceived() >= 10 ? 3 : 2, selectedEntitlementType))
                        {
                            ccb.ShowDialog();
                            kioskTransaction = ccb.GetKioskTransaction;
                        }
                    }
                }
                else
                {
                    KioskStatic.logToFile("Payment mode details are missing for the Kiosk. Please verify the setup");
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                timeoutClose = true;
                KioskStatic.logToFile("Timeout occured");
                log.Error(ex);
                BaseForm.PerformTimeoutAbortAction(kioskTransaction, null);
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                frmOKMsg.ShowUserMessage(ex.Message);
                log.Error(ex.Message);
            }
            finally
            {
                if (timeoutClose == true || kioskTransaction.GetTotalPaymentsReceived() <= 0)
                {
                    Close();
                }
                else
                {
                    EnableButtons();
                    billAcceptor.Start();
                }
            }
            log.LogMethodExit();
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool timeoutClose = false;
            try
            {
                DisableButtons();
                billAcceptor.Stop();
                txtMessage.Text = label5.Text;
                btnRecharge.Enabled = btnNewCard.Enabled = false;
                Audio.PlayAudio(Audio.RedeemTokenTapCard);

                if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                {
                    if (KioskHelper.isTimeEnabledStore() == true)
                    {
                        selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                    }
                    else
                    {
                        using (frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345)))
                        {
                            if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                frmEntitle.Dispose();
                                log.LogMethodExit();
                                return;
                            }
                            selectedEntitlementType = frmEntitle.selectedEntitlement;
                            frmEntitle.Dispose();
                        }
                    }
                }

                DataTable dt = KioskStatic.getProductDetails(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId);
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[0]["price"] = kioskTransaction.GetTotalPaymentsReceived();
                }
                else
                {
                    KioskStatic.logToFile("unable to fetch record for variable product id: " + KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId.ToString());
                    throw new Exception(KioskStatic.Utilities.MessageUtils.getMessage(1669));
                }

                bool isRegisteredCustomerOnly = (dt.Rows[0]["RegisteredCustomerOnly"].ToString() == "Y") ? true : false;

                using (frmTapCard ftc = new frmTapCard(kioskTransaction, isRegisteredCustomerOnly, true))
                {
                    DialogResult dr = ftc.ShowDialog();
                    if (dr == DialogResult.Cancel)
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                        throw new CustomerStatic.TimeoutOccurred(msg);
                    }
                    Audio.Stop();

                    if (ftc.Card != null) // valid card tapped
                    {
                        if (ftc.Card.technician_card.Equals('Y'))
                        {
                            frmOKMsg.ShowUserMessage(KioskStatic.Utilities.MessageUtils.getMessage(197, ftc.Card.CardNumber));
                            return;
                        }

                        bool showMsgRecommendCustomerToRegister = true;
                        bool isLinked = CustomerStatic.LinkCustomerToTheTransaction(kioskTransaction, KioskStatic.Utilities.ExecutionContext, isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                        if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(), isRegisteredCustomerOnly, isLinked))
                        {
                            log.LogMethodExit();
                            return;
                        }

                        paymentModeDTOList = KioskStatic.paymentModeDTOList;
                        if (paymentModeDTOList != null && paymentModeDTOList.Any())
                        {
                            PaymentModeDTO paymentModeCDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                            selectedFundsList = GetSelectedFundsList();
                            CreateTrxLinesForFundProduct();
                            CheckForLoyalty();
                            kioskTransaction = KioskHelper.AddRechargeCardProduct(kioskTransaction, ftc.Card, dt.Rows[0], 1, selectedEntitlementType);
                            using (frmCardTransaction fct = new frmCardTransaction(kioskTransaction, paymentModeCDTO))
                            {
                                fct.ShowDialog();
                                kioskTransaction = fct.GetKioskTransaction;
                            }
                        }
                        else
                        {
                            KioskStatic.logToFile("Payment mode details are missing for the Kiosk. Please verify the setup");
                        }
                    }
                    else // time out
                    {
                        ftc.Dispose();
                        log.LogMethodExit();
                        return;
                    }

                    ftc.Dispose();
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                timeoutClose = true;
                KioskStatic.logToFile("Timeout occured");
                log.Error(ex);
                BaseForm.PerformTimeoutAbortAction(kioskTransaction, null);
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                frmOKMsg.ShowUserMessage(ex.Message);
                log.Error(ex.Message);
            }
            finally
            {
                if (timeoutClose == true || kioskTransaction.GetTotalPaymentsReceived() <= 0)
                {
                    Close();
                }
                else
                {
                    EnableButtons();
                    billAcceptor.Start();
                }
            }
            log.LogMethodExit();
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool formClosed = false;
            try
            {
                DisableButtons();
                billAcceptor.Stop();
                btnRecharge.Enabled = btnNewCard.Enabled = false;
                txtMessage.Text = label5.Text;
                if (billAcceptor != null)
                {
                    BaseForm.DirectCashAbortAction(kioskTransaction, null);
                }
                DialogResult = System.Windows.Forms.DialogResult.Cancel; 
                for (int i = Application.OpenForms.Count - 1; i > 0; i--)
                {
                    Application.OpenForms[i].Visible = false;
                    Application.OpenForms[i].Close();
                    formClosed = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("In direct cash form. Cannot go back to home page due to " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {                
                if (formClosed == false)
                {
                    EnableButtons();
                    billAcceptor.Start();
                }
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCashInserts");
            try
            {
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenHeader1TextForeColor;
                //( Insert more cash) - #CashInsertScreenHeader1ForeColor
                this.label1.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenHeader2TextForeColor;
                //(Cash Inserted Header) - #CashInsertScreenHeader2ForeColor
                this.panel2.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenCashInsertedInfoTextForeColor;
                //(Cash Inserted Info) - #CashInsertScreenCashInsertedInfoForeColor
                this.btnNewCard.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenBtnNewCardTextForeColor;
                //#CashInsertScreenBtnNewCardForeColor
                this.btnRecharge.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenBtnRechargeTextForeColor;
                //# CashInsertScreenBtnRechargeForeColor
                this.label5.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenHeader3TextForeColor;
                //(more cash at any time) - #CashInsertScreenHeader3ForeColor
                this.label2.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenDenominationHeaderTextForeColor;
                //(Header for Denomination) -#CashInsertScreenDenominationHeaderForeColor
                this.label3.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenQuantityHeaderTextForeColor;
                //(Header for Quantity) -#CashInsertScreenQuantityHeaderForeColor
                this.label4.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenPointsHeaderTextForeColor;
                //(Header for points) -#CashInsertScreenPointsHeaderForeColor
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.HomeScreenFooterTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnHomeTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenCancelButtonTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCashInserts: " + ex.Message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets active Fund products DTO.
        /// </summary>
        /// <param name=""></param> None
        /// <returns = List<KeyValuePair<string, ProductsDTO>> </returns> dictionary holding the fund products DTO List
        /// 
        public List<KeyValuePair<string, ProductsDTO>> GetSelectedFundsList()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Fetching user selected fund products");
            List<KeyValuePair<string, ProductsDTO>> selectedFundsList = new List<KeyValuePair<string, ProductsDTO>>();
            try
            {
                List<ProductsDTO> activeFunds = KioskHelper.GetActiveFundRaiserProducts(KioskStatic.Utilities.ExecutionContext);
                if (activeFunds != null && activeFunds.Any())
                {
                    using (frmFundRaiserAndDonation frmFandD = new frmFundRaiserAndDonation(activeFunds, true))
                    {
                        if (frmFandD.ShowDialog() == DialogResult.OK)
                        {
                            for (int i = 0; i < frmFandD.selectedProductsDTOList.Count; i++)
                            {
                                selectedFundsList.Add(new KeyValuePair<string, ProductsDTO>(KioskStatic.FUND_RAISER_LOOKUP_VALUE, frmFandD.selectedProductsDTOList[i]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while fetching getting selected fund products : " + ex.Message);
            }

            log.LogMethodExit();
            return selectedFundsList;
        }

        /// <summary>
        /// Creates Transaction Line for the fund/s product selected 
        /// </summary>
        /// <param name="rechargeCard"></param>
        private void CreateTrxLinesForFundProduct()
        {
            log.LogMethodEntry();
            try
            { 
                if (selectedFundsList != null && selectedFundsList.Any())
                {
                    kioskTransaction.AddDonationOrFundraiserProducts(selectedFundsList);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Exception occured while Creating TrxLine for fund raiser: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void CheckForLoyalty()
        {
            log.LogMethodEntry();
            if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_ALOHA_LOYALTY_INTERFACE") == "Y"
                            && string.IsNullOrWhiteSpace(kioskTransaction.LoyaltyCardNumber) == true)
            {
                selectedLoyaltyCardNo = string.Empty;
                using (frmLoyalty fm = new frmLoyalty())
                {
                    if (fm.DialogResult != DialogResult.Cancel)
                    {
                        if (fm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            selectedLoyaltyCardNo = fm.selectedLoyaltyCardNo;
                    }
                    if (fm != null)
                        fm.Dispose();
                }
                if (string.IsNullOrWhiteSpace(selectedLoyaltyCardNo) == false)
                {
                    kioskTransaction.LoyaltyCardNumber = selectedLoyaltyCardNo;
                }
            }
            log.LogMethodExit();
        }
        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                this.btnNewCard.Enabled = true;
                this.btnRecharge.Enabled = true;
                this.btnCancel.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in EnableButtons() in Cash Insert screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            try
            {
                this.btnNewCard.Enabled = false;
                this.btnRecharge.Enabled = false;
                this.btnCancel.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in DisableButtons() in Cash Insert screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

    }
}


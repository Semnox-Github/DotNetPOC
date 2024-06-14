/********************************************************************************************
 * Project Name - Portrait Kiosk
 * Description  - user interface - frmCustomerOptions
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      30-Oct-2019      Deeksha           Created.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.1     22-Feb-2023      Vignesh Bhat       Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace Parafait_Kiosk
{
    public partial class frmCustomerOptions : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private string cardNumber;
        private Card card;
        //private CustomerDTO customerDTO;
        private string defaultMsg;
        //private List<CustomerDTO> signForCustomerDTOList = null;
        private WaiverSetDTO selectedWaiverSetDTO;
        private string eventCode;
        private CustomerDTO selectedCustomerDTO;
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();

        public CustomerDTO GetCustomerDTO { get { return selectedCustomerDTO; } }
        public frmCustomerOptions(WaiverSetDTO waiverSetDTO, string eventCode, CustomerDTO signatoryCustomerDTO )
        {
            log.LogMethodEntry(waiverSetDTO, eventCode, signatoryCustomerDTO); 
            this.utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            this.selectedWaiverSetDTO = waiverSetDTO;
            this.eventCode = eventCode;
            this.selectedCustomerDTO = signatoryCustomerDTO;
            KioskStatic.setDefaultFont(this);
            SetDisplayImages();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2438);//Please choose an option
            txtMessage.Text = defaultMsg;
            KioskStatic.logToFile("Loading customer options form");
            SetCustomizedFontColors();
            //DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void SetDisplayImages()
        {
            log.LogMethodEntry();
            btnNewRegistration.BackgroundImage = ThemeManager.CurrentThemeImages.RegisterPassBig;
            btnExistingCustomer.BackgroundImage = ThemeManager.CurrentThemeImages.RegisteredCustomerBig;
            log.LogMethodExit();
        }

        private void frmCustomerOptions_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (selectedCustomerDTO != null && selectedCustomerDTO.Id > -1)
                {
                    SignWaivers();
                }
                else
                {
                    selectedCustomerDTO = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
                log.Info("Exit Customer Options form");
                KioskStatic.logToFile("Exit Customer Options form");
                this.Close();
            }
            log.LogMethodExit(); 
        }

        private void btnRegCust_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                ResetKioskTimer();
                selectedCustomerDTO = null;
                KioskStatic.logToFile("Registered Customer option selected");
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
                using (frmGetCustomerInput gci = new frmGetCustomerInput())
                {
                    if (gci.ShowDialog() == DialogResult.OK)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
                        CustomerDTO customerDTO = gci.GetCustomerDTO;
                        if (customerDTO != null)
                        {
                            KioskStatic.logToFile("Received registered Customer details");
                            //log.Info("Received customer DTO, closing form");
                            selectedCustomerDTO = customerDTO;
                            SignWaivers();
                        }
                        else
                        {
                            KioskStatic.logToFile("Registered Customer details are not received");
                            log.Info("Registered Customer details are not received");
                            //Customer registration is required to proceed 
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1664)); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableButtons();
                this.Cursor = Cursors.Default;
                txtMessage.Text = defaultMsg;
            }
            log.LogMethodExit();
        }

        private void btnNewCust_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                ResetKioskTimer();
                KioskStatic.logToFile("New Customer option selected");
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
                RegisterCustomer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableButtons();
                this.Cursor = Cursors.Default;
                txtMessage.Text = defaultMsg;
            }
            log.LogMethodExit();
        }
        private void RegisterCustomer()
        {
            log.LogMethodEntry(); 
            try
            {
                ResetKioskTimer();
                selectedCustomerDTO = null;
                string tappedCardNumber = "";
                if (KioskStatic.AllowRegisterWithoutCard)
                {
                    KioskStatic.logToFile("Allow Register Without Card");
                    log.Info("Allow Registe Without Card");
                    StopKioskTimer();
                    ResetKioskTimer();
                    using (frmTapCard ftc = new frmTapCard(utilities.MessageUtils.getMessage(496), utilities.MessageUtils.getMessage("Yes")))
                    {
                        DialogResult dr = ftc.ShowDialog();
                        if (dr == System.Windows.Forms.DialogResult.OK)
                        {
                            tappedCardNumber = null;
                        }
                        else if (ftc.Card == null)
                        {
                            ftc.Dispose();
                            log.LogMethodExit();
                            return;
                        }
                        else
                        {
                            tappedCardNumber = ftc.cardNumber;
                        }
                        ftc.Dispose();
                    }
                }
                else
                {
                    KioskStatic.logToFile("Needs card for Registration");
                    log.Info("Needs card for Registration");
                    using (frmTapCard ftc = new frmTapCard(utilities.MessageUtils.getMessage(500)))
                    {
                        ftc.ShowDialog();
                        if (ftc.Card == null)
                        {
                            ftc.Dispose();
                            log.LogMethodExit();
                            return;
                        }

                        tappedCardNumber = ftc.cardNumber;
                        log.LogVariableState("CardNumber", tappedCardNumber);
                        ftc.Dispose();
                    }
                }

                if (!String.IsNullOrEmpty(tappedCardNumber))
                {
                    Card custRegisterCard = new Card(tappedCardNumber, "", KioskStatic.Utilities);
                    if (custRegisterCard.technician_card.Equals('Y'))
                    {
                        txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(197, custRegisterCard.CardNumber);
                        KioskStatic.logToFile(txtMessage.Text);
                        log.LogMethodExit(txtMessage.Text);
                        return;
                    }
                }
               CustomerDTO customerDTO = null;
                try
                {
                    customerDTO = CustomerStatic.ShowCustomerScreen(tappedCardNumber);
                }
                catch (CustomerStatic.TimeoutOccurred ex)
                {
                    log.Error(ex);
                    PerformTimeoutAbortAction(null, null);
                    this.DialogResult = DialogResult.Cancel;
                    log.LogMethodExit();
                    return;
                }
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                this.Activate();
                log.LogVariableState("customerDTO", customerDTO);
                if (customerDTO != null && customerDTO.Id > -1)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OTP_CHECK_REQUIRED_FOR_WAIVER_REGISTRATION") == "Y")
                    {
                        using (frmCustomerOTP otp = new frmCustomerOTP(customerDTO))
                        {
                            if (otp.ShowDialog() == DialogResult.OK)
                            {
                                log.Info("otp.ShowDialog() == DialogResult.OK");
                                //this.DialogResult = DialogResult.OK; 
                                KioskStatic.logToFile("Received cutomer DTO");
                                log.Info("Received cutomer DTO,");
                                selectedCustomerDTO = customerDTO;
                                SignWaivers();
                                //this.Close();
                            }
                        }
                    }
                    else
                    {
                        //this.DialogResult = DialogResult.OK; 
                        KioskStatic.logToFile("Received cutomer DTO");
                        log.Info("Received cutomer DTO");
                        //this.Close();
                        selectedCustomerDTO = customerDTO;
                        SignWaivers();
                    } 
                }
                else
                {
                    selectedCustomerDTO = null;
                    log.Info("Didnt receive customer DTO");
                    KioskStatic.logToFile("Didnt receive customer DTO");
                    //Customer registration is required to proceed 
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1664)); 
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            { 
                KioskStatic.logToFile("exit RegisterCustomer()");
            }
            log.LogMethodExit();
        }
        

        private void SignWaivers()
        {
            log.LogMethodEntry();
            //ApplicationContext ap = new ApplicationContext();
            try
            {
                if (selectedCustomerDTO != null && selectedCustomerDTO.Id > -1)
                {
                    //KioskHelper.ShowPreloader(ap, utilities, Properties.Resources.Back_button_box, Properties.Resources.PreLoader, statusProgressMsgQueue);
                    this.Cursor = Cursors.WaitCursor;
                    //signForCustomerDTOList = new List<CustomerDTO>();
                    using (frmCustomerDetailsForWaiver frmCustDetailsForWaiver = new frmCustomerDetailsForWaiver(selectedCustomerDTO, selectedWaiverSetDTO, eventCode))
                    {
                        //try
                        //{
                        //    KioskHelper.ClosePreLoaderForm(statusProgressMsgQueue);
                        //}
                        //catch { };
                        this.Cursor = Cursors.WaitCursor;
                        if( frmCustDetailsForWaiver.ShowDialog() == DialogResult.OK  )
                        {
                            log.Debug(" frmCustDetailsForWaiver.ShowDialog() == DialogResult.OK ");
                        }
                        else
                        {
                            log.Debug(" frmCustDetailsForWaiver.ShowDialog() == DialogResult.Cancell ");
                        }
                        selectedCustomerDTO = frmCustDetailsForWaiver.GetSignatoryCustomerDTO;
                        this.Close();
                    }
                }
                else
                {
                    selectedCustomerDTO = null;
                }
            }
            finally
            {
                //int errorRecoveryCounter = 0;
                //KioskHelper.CloseAPObject(ap, errorRecoveryCounter);
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCustomerOptions");
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmCustOptionBtnHomeTextForeColor;
                this.btnNewRegistration.ForeColor = KioskStatic.CurrentTheme.FrmCustOptionBtnNewRegistrationTextForeColor;
                this.btnNewRegistration.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.CustOptionBtnTextAlignment);
                this.btnExistingCustomer.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.CustOptionBtnTextAlignment);
                this.btnExistingCustomer.ForeColor = KioskStatic.CurrentTheme.FrmCustOptionBtnExistingCustomerTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmCustOptionBtnCancelTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.FrmCustOptionBtnCancelTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmCustOptionTxtMessageTextForeColor;
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CustomerOptionsBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCustomerOptions: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnPrev.Enabled = true;
                btnNewRegistration.Enabled = true;
                btnExistingCustomer.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void DisableButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnPrev.Enabled = false;
                btnNewRegistration.Enabled = false;
                btnExistingCustomer.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}

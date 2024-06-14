/********************************************************************************************
 * Project Name - Portait Kiosk
 * Description  - frmSignWaivers user interface
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      30-Oct-2019      Deeksha        Created for waiver phase 2 changes
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.1     22-Feb-2023      Vignesh Bhat       Kiosk Cart Enhancements
*2.150.7     10-Nov-2023      Sathyavathi        Customer Lookup Enhancement
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmSignWaivers : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        //private string cardNumber;
        ///private Card Card;
        private List<WaiverSetDTO> waiverSetDTOList;
        private CustomerDTO signatoryCustomerDTO;
        private string eventCode;
        private CustomerBL customerBL;
        //private List<KeyValuePair<int, int>> managerApprovalList;
        //private bool ignoreEventCode;
        private string defaultMsg;
        public CustomerDTO GetSignatoryDTO { get { return signatoryCustomerDTO; } }

        public frmSignWaivers(List<WaiverSetDTO> waiverSetDTOList, CustomerDTO customerDTO, string eventCode)
        {
            log.LogMethodEntry(waiverSetDTOList, customerDTO, eventCode);
            this.utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            foreach (Control c in panel1.Controls)
            {
                c.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            this.waiverSetDTOList = waiverSetDTOList;
            this.signatoryCustomerDTO = customerDTO;
            SetCustomerName();
            this.eventCode = eventCode;
            defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2422);//Please sign waivers
            SetCustomizedFontColors();
            //DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            KioskStatic.logToFile("Loading sign waivers form");
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void SetCustomerName()
        {
            log.LogMethodEntry();
            if (this.signatoryCustomerDTO != null && this.signatoryCustomerDTO.Id > -1)
            {
                customerBL = new CustomerBL(utilities.ExecutionContext, this.signatoryCustomerDTO);
                lblCustomer.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer")
                                   + ": "
                                   + (string.IsNullOrEmpty(signatoryCustomerDTO.FirstName) ? string.Empty : signatoryCustomerDTO.FirstName)
                                   + " "
                                   + (string.IsNullOrEmpty(signatoryCustomerDTO.LastName) ? string.Empty : signatoryCustomerDTO.LastName);
            }
            else
            {
                lblCustomer.Text = string.Empty;

            }
            log.LogMethodExit();
        }

        private void LoadWaivers()
        {
            log.LogMethodEntry();
            try
            {
                if (this.signatoryCustomerDTO != null)
                {
                    ReLoadCustomerInfo();
                }
                fpnlWaiverSet.Controls.Clear();
                if (this.waiverSetDTOList != null && this.waiverSetDTOList.Any())
                {
                    for (int i = 0; i < waiverSetDTOList.Count; i++)
                    {
                        bool evenRowNo = (i % 2 == 0 ? true : false);
                        CreateWaiverSetUsrCtrl(waiverSetDTOList[i], evenRowNo);
                    }
                }
                if (bigVerticalScrollWaiverSet.Visible == false)
                {
                    this.fpnlWaiverSet.Location = new System.Drawing.Point(30, 65);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ReLoadCustomerInfo()
        {
            log.LogMethodEntry();
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            List<int> custIdList = new List<int>();
            custIdList.Add(this.signatoryCustomerDTO.Id);
            List<CustomerDTO> customerDTOLIst = customerListBL.GetCustomerDTOList(custIdList, true, true, true);
            if (customerDTOLIst != null && customerDTOLIst.Any())
            {
                this.signatoryCustomerDTO = customerDTOLIst[0];
            }
            log.LogMethodExit();
        }

        private void CreateWaiverSetUsrCtrl(WaiverSetDTO waiverSetDTO, bool evenRowNo)
        {
            log.LogMethodEntry(waiverSetDTO, evenRowNo);
            usrCtlWaiverSet usrCtlWaiverSet = new usrCtlWaiverSet(waiverSetDTO, evenRowNo, this.signatoryCustomerDTO);
            usrCtlWaiverSet.SignWaiverSet += new usrCtlWaiverSet.SignWaiverSetDelegate(SignWaiverSet);
            fpnlWaiverSet.Controls.Add(usrCtlWaiverSet);
            log.LogMethodExit();
        }

        private void frmSignWaivers_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                LoadWaivers();
                txtMessage.Text = defaultMsg;
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                ResetKioskTimer();
                log.Error(ex);
                txtMessage.Text = ex.Message;
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message, true))
                {
                    frmOK.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    this.Close();
        //    KioskStatic.logToFile("Cancel button is clicked");
        //    log.LogMethodExit();
        //}

        private void SignWaiverSet(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry();
            txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
            ResetKioskTimer();
            KioskStatic.logToFile("Sign Waiver set");
            try
            {
                DisableButtons();
                this.Cursor = Cursors.WaitCursor;
                if (waiverSetDTO != null)
                {
                    if (signatoryCustomerDTO == null || (signatoryCustomerDTO != null && signatoryCustomerDTO.Id != -1))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        GetCustomer(waiverSetDTO);
                        this.Cursor = Cursors.WaitCursor;
                    }
                    else
                    {
                        signatoryCustomerDTO = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("SignWaiverSet error: " + ex.Message);
                txtMessage.Text = ex.Message;
                using (frmOKMsg frmMsg = new frmOKMsg(ex.Message))
                {
                    frmMsg.ShowDialog();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2422);//Please sign waivers
                EnableButtons();
            }
            log.LogMethodExit();
        }
        private void GetCustomer(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            KioskStatic.logToFile("Show Customer Options");
            try
            {
                if (signatoryCustomerDTO != null && signatoryCustomerDTO.Id > -1)
                {
                    SignWaivers(waiverSetDTO);
                }
                else
                {
                    signatoryCustomerDTO = null;
                }

                kioskTransaction = new KioskTransaction(KioskStatic.Utilities);
                bool isCustomerMandatory = true;
                CustomerDTO customerDTO = null;
                try
                {
                    customerDTO = CustomerStatic.GetCustomer(signatoryCustomerDTO, isCustomerMandatory);
                }
                catch (CustomerStatic.TimeoutOccurred ex)
                {
                    KioskStatic.logToFile("Timeout occured");
                    log.Error(ex);
                    PerformTimeoutAbortAction(null, null);
                    this.DialogResult = DialogResult.Cancel;
                    log.LogMethodExit();
                    return;
                }
                if (customerDTO == null)
                {
                    //5256 - No Customer Found
                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5256));
                }
                if (customerDTO != null && customerDTO.Id > -1)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OTP_CHECK_REQUIRED_FOR_WAIVER_REGISTRATION") == "Y")
                    {
                        using (frmCustomerOTP otp = new frmCustomerOTP(customerDTO))
                        {
                            if (otp.ShowDialog() == DialogResult.OK)
                            {
                                log.Info("otp.ShowDialog() == DialogResult.OK");
                                signatoryCustomerDTO = customerDTO;
                                KioskStatic.logToFile("Received cutomer DTO");
                                log.Info("Received cutomer DTO,");
                                SignWaivers(waiverSetDTO);
                            }
                        }
                    }
                    else
                    {
                        signatoryCustomerDTO = customerDTO;
                        KioskStatic.logToFile("Received cutomer DTO");
                        log.Info("Received cutomer DTO");
                        SignWaivers(waiverSetDTO);
                    }
                    if (signatoryCustomerDTO != null && signatoryCustomerDTO.Id > -1)
                    {
                        SetCustomerName();
                        LoadWaivers();
                    }
                }
                else
                {
                    signatoryCustomerDTO = null;
                    log.Info("Didnt receive customer DTO");
                    KioskStatic.logToFile("Didnt receive customer DTO");
                    //1664 - Customer registration is required to proceed 
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1664));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ShowCustomerOptions error: " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void SignWaivers(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            try
            {
                if (signatoryCustomerDTO != null && signatoryCustomerDTO.Id > -1)
                {
                    this.Cursor = Cursors.WaitCursor;
                    using (frmCustomerDetailsForWaiver frmCustDetailsForWaiver = new frmCustomerDetailsForWaiver(signatoryCustomerDTO, waiverSetDTO, eventCode))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        if (frmCustDetailsForWaiver.ShowDialog() == DialogResult.OK)
                        {
                            log.Debug(" frmCustDetailsForWaiver.ShowDialog() == DialogResult.OK ");
                        }
                        else
                        {
                            log.Debug(" frmCustDetailsForWaiver.ShowDialog() == DialogResult.Cancell ");
                        }
                        signatoryCustomerDTO = frmCustDetailsForWaiver.GetSignatoryCustomerDTO;
                        //this.Close();
                    }
                }
                else
                {
                    signatoryCustomerDTO = null;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (signatoryCustomerDTO != null)
            {
                //This action will clear current customer session. Do you want to proceed?
                using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2459)))//"This action will clear current customer session. Do you want to proceed?")))
                {
                    if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        base.btnHome_Click(sender, e);
                    }
                }
            }
            else
            {
                base.btnHome_Click(sender, e);
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
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmSignWaivers");
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversBtnHomeTextForeColor;
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversLblSiteNameTextForeColor;
                this.lblCustomer.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversLblCustomerTextForeColor;
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversLblGreeting1TextForeColor;
                this.lblWaiver.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversLblWaiverTextForeColor;
                this.lblSelection.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversLblSelectionTextForeColor;
                this.label1.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversLabel1TextForeColor;
                this.fpnlWaiverSet.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversFpnlWaiverSetTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversBtnCancelTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversBtnCancelTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiversTxtMessageTextForeColor;
                this.bigVerticalScrollWaiverSet.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.SignWaiversBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                panel1.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmSignWaivers: " + ex.Message);
            }
            log.LogMethodExit();
        }


        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnPrev.Enabled = true;
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
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}

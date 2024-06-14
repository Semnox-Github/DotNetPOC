/********************************************************************************************
 * Project Name - Portrait Kiosk
 * Description  - Customer Registration Lookup while purchasing the products
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By     Remarks          
 *********************************************************************************************
 *2.150.6     10-Nov-2023      Sathyavathi     Created for Customer Lookup Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;

namespace Parafait_Kiosk
{
    public partial class frmSelectCustomer : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CustomerDTO> customersList;
        private CustomerDTO customerDTO;

        internal CustomerDTO GetCustomerDTO { get { return customerDTO; } }

        public frmSelectCustomer(ExecutionContext executionContext, List<CustomerDTO> matchingCustomersList)
        {
            log.LogMethodEntry(executionContext, matchingCustomersList);
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.logToFile("Loading select customer form");
            this.executionContext = executionContext;
            this.customersList = matchingCustomersList;

            DisplaybtnHome(false);
            DisplaybtnCart(false);
            DisplaybtnCancel(false);
            DisplaybtnPrev(true);
            try
            {
                lblGreeting.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
                //5328 - Confirm your name and Proceed. Press Back to correct
                lblGreeting.Text =
                    txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5328);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
                KioskStatic.setDefaultFont(this);
                SetCustomizedFontColors();
                DisplayCustomerDetails();
                SetCustomImages();
                KioskStatic.Utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error("Error in Select Cutomer screen", ex);
                KioskStatic.logToFile("Error in Select Cutomer screen" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmSelectCustomer_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        //public override void btnCancel_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    try
        //    {
        //        KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
        //        base.btnHome_Click(sender, e);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error in btnCancel_Click", ex);
        //    }
        //    log.LogMethodExit();
        //}

        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (customerDTO != null && customerDTO.Id > 0)
                {
                    CustomerStatic.CheckForTermsAndConditions(customerDTO);
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                else
                {
                    //5328 - Confirm your name and Proceed. Press Back to correct
                    throw new Exception(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5328));
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                KioskStatic.logToFile("Timeout occured");
                log.Error(ex);
                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                this.DialogResult = DialogResult.Cancel;
                log.LogMethodExit();
                return;
            }
            catch (Exception ex)
            {
                string errMsg = "Select Customer screen - Guest did not select the record";
                log.Error(errMsg + ex);
                KioskStatic.logToFile(errMsg + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void bigVerticalScrollView_ButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error in bigVerticalScrollView_ButtonClick", ex);
            }
            log.LogMethodExit();
        }

        private void DisplayCustomerDetails()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            fLPUstCtrls.SuspendLayout();

            foreach (CustomerDTO customer in customersList)
            {
                usrCtrlSelectCustomer usrCtrl = new usrCtrlSelectCustomer(KioskStatic.Utilities.ExecutionContext, customer);
                usrCtrl.selectedCustomerMethod += new usrCtrlSelectCustomer.SelctedCustomerDelegate(SelectedCustomerDelegate);
                usrCtrl.unSelectCustomerMethod += new usrCtrlSelectCustomer.UnSelectedCustomerDelegate(UnSelectCustomerDelegate);
                fLPUstCtrls.Controls.Add(usrCtrl);
            }
            Application.DoEvents();
            fLPUstCtrls.ResumeLayout();
            log.LogMethodExit();
        }

        private void SelectedCustomerDelegate(CustomerDTO customerDTO)
        {
            log.LogMethodEntry(customerDTO);
            ResetKioskTimer();
            try
            {
                foreach (usrCtrlSelectCustomer usrCtrl in fLPUstCtrls.Controls)
                {
                    usrCtrl.SetBackgroungImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
                    usrCtrl.IsSelected = false;
                }

                this.customerDTO = customerDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SelectedCustomerDeiegate() of Select Cutomer screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void UnSelectCustomerDelegate(CustomerDTO customerDTO)
        {
            log.LogMethodEntry(customerDTO);
            ResetKioskTimer();
            try
            {
                foreach (usrCtrlSelectCustomer usrCtrl in fLPUstCtrls.Controls)
                {
                    usrCtrl.SetBackgroungImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
                    usrCtrl.IsSelected = false;
                }
                this.customerDTO = null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in UnSelectCustomerDeiegate() of Select Cutomer screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmSelectCustomer_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error closing Select Cutomer screen()", ex);
            }
            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.DefaultBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnPrev.BackgroundImage =
                    btnCancel.BackgroundImage =
                    btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                this.bigVerticalScrollView.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                pnlCustomerList.BackgroundImage = ThemeManager.CurrentThemeImages.PanelTimeSection;
            }
            catch (Exception ex)
            {
                log.Error("Error Setting Customized background images for Select Cutomer screen", ex);
                KioskStatic.logToFile("Error setting customized background images for Select Cutomer screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.SelectCustomerLblGreetingTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.SelectCustomerBackButtonTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.SelectCustomerBtnCancelTextForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.SelectCustomerBtnProceedTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.SelectCustomerFooterTextForeColor;
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.lblFirstName.ForeColor = KioskStatic.CurrentTheme.SelectCustomerLblFirstNameTextForeColor;
                this.lblLastName.ForeColor = KioskStatic.CurrentTheme.SelectCustomerLblLastNameTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for Select Cutomer screen", ex);
                KioskStatic.logToFile("Error setting customized font colors for Select Cutomer screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

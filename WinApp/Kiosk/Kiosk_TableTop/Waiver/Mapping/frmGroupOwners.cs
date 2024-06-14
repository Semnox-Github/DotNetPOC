/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Waiver Mapping
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.151.2     29-Dec-2023      Sathyavathi        Created for Waiver Mapping Enhancement
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using System.Linq;

namespace Parafait_Kiosk
{
    public partial class frmGroupOwners : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private int selectedCustomerId;
        List<int> customersIdList;
        private UsrCtrlGroupOwners previouslySelectedUsrCtrl;
        List<CustomerRelationshipDTO> customerRelationshipDTOs;

        public int SelectedCustomerId { get { return selectedCustomerId; } }

        public frmGroupOwners(List<int> customersIdList, List<CustomerRelationshipDTO> customerRelationshipDTOs)
        {
            log.LogMethodEntry(customersIdList, customerRelationshipDTOs);
            KioskStatic.logToFile("In frmGroupOwners class");
            this.customersIdList = customersIdList;
            this.customerRelationshipDTOs = customerRelationshipDTOs;
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            DisplaybtnCancel(false);
            DisplaybtnPrev(true);
            KioskStatic.setDefaultFont(this);
            KioskStatic.Utilities.setLanguage(this);
            SetDisplayElements();
            log.LogMethodExit();
        }

        private void frmFilteredCustomers_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                selectedCustomerId = -1;
                if (customersIdList.Count > 0)
                {
                    RefreshCustomersList();
                }
                else
                {
                    log.LogMethodExit();
                    Close();
                }
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
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in frmGroupOwners()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void RefreshCustomersList()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                flpUsrCtrls.SuspendLayout();
                flpUsrCtrls.Controls.Clear();
                foreach (int id in customersIdList.ToList())
                {
                    List<CustomerRelationshipDTO> relatedCustomers = customerRelationshipDTOs;
                    CustomerBL customerBL = null;
                    CustomerRelationshipDTO cusRel = relatedCustomers.FirstOrDefault(c => c.CustomerId == id);
                    if (cusRel != null && cusRel.CustomerDTO != null)
                    {
                        customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, cusRel.CustomerDTO);
                    }
                    else
                    {
                        customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, id, true, true, null);
                    }

                    if (customerBL.CustomerDTO != null)
                    {
                        int customerId = customerBL.CustomerDTO.Id;
                        string customerName = customerBL.CustomerDTO.FirstName;
                        if (!string.IsNullOrWhiteSpace(customerBL.CustomerDTO.LastName))
                        {
                            customerName += " "; //add a space
                            customerName += customerBL.CustomerDTO.LastName;
                        }

                        UsrCtrlGroupOwners usrCtrl = new UsrCtrlGroupOwners(customerName, customerId);
                        usrCtrl.selctedParent += new UsrCtrlGroupOwners.Delegate(SelectedGroupOwner);
                        flpUsrCtrls.Controls.Add(usrCtrl);
                    }
                }
                flpUsrCtrls.ResumeLayout();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in frmGroupOwners()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SelectedGroupOwner(UsrCtrlGroupOwners usrCtrl)
        {
            log.LogMethodEntry(usrCtrl);
            try
            {
                ResetKioskTimer();
                UsrCtrlGroupOwners selectedUsrCtrl = usrCtrl;

                if (previouslySelectedUsrCtrl == selectedUsrCtrl)
                {
                    if (selectedUsrCtrl.IsSelected)
                    {
                        selectedUsrCtrl.IsSelected = false;
                        selectedCustomerId = -1;
                    }
                }
                else
                {
                    foreach (UsrCtrlGroupOwners c in flpUsrCtrls.Controls)
                    {
                        c.IsSelected = false;
                    }
                }
                foreach (UsrCtrlGroupOwners userControl in flpUsrCtrls.Controls)
                {
                    if (userControl.Tag == selectedUsrCtrl.Tag)
                    {
                        previouslySelectedUsrCtrl = userControl;
                        userControl.IsSelected = true;
                        selectedCustomerId = (int)selectedUsrCtrl.Tag;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR in frmGroupOwners" + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                SetOnScreenMessages();
                SetCustomImages();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetDisplayElements() of frmGroupOwners" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetOnScreenMessages()
        {
            log.LogMethodEntry();

            //5466 - Who do you want to link the new member with?
            lblGreetingMsg.Text = MessageContainerList.GetMessage(executionContext, 5466);
            btnLink.Text = MessageContainerList.GetMessage(executionContext, "Link"); //Literal

            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            this.BackgroundImage = ThemeManager.CurrentThemeImages.PreSelectPaymentBackground;
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnClose.BackgroundImage = 
                btnLink.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.YesNoButtons);
            this.bigVerticalScrollPaymentModes.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);

            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            this.lblGreetingMsg.ForeColor = KioskStatic.CurrentTheme.FilteredCustomersGreetingTextForeColor;
            this.btnClose.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsBackButtonTextForeColor;
            this.btnLink.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsProceedButtonTextForeColor;
            log.LogMethodExit();
        }  
        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Cancel Button Pressed in frmGroupOwners: Triggering Home Button Action ");
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        }
        private void btnLink_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                if (selectedCustomerId == -1)
                {
                    //2460 - Please select a record to proceed
                    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2460);
                    throw new ValidationException(errMsg);
                }

                DialogResult = DialogResult.OK;
                Close();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("btnProceed_Click() in frmGroupOwners : " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void ScrollBtnClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining < 20)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            else
                setKioskTimerSecondsValue(tickSecondsRemaining - 1);
            log.LogMethodExit();
        }
    }
}

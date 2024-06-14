/********************************************************************************************
* Project Name - Parafait_Kiosk - Portrait
* Description  - frmCustomerFound
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.150.6    10-Nov-2023      Sathyavathi        Created for Customer Lookup Enhancement
 ********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmCustomerFound : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public frmCustomerFound (string customerName)
        {
            log.LogMethodEntry(customerName);
            InitializeComponent();
            this.Size = this.BackgroundImage.Size;
            KioskStatic.setDefaultFont(this);
            lblCustomerName.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Dear") 
                + " " + customerName;
            lblWelcomeMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Welcome Back!!"); //literal
            //5329 - Click OK to Proceed
            lblClickOKMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5329);
            //btnPrev.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Cancel");
            DisplaybtnPrev(true); 
            DisplaybtnCancel(false);
            SetCustomiImages();
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmCustomerFound_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
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

        private void frmCustomerFound_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            log.LogMethodExit();
        }

        private void SetCustomiImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.OkMsgBox);
                btnProceed.BackgroundImage = btnPrev.BackgroundImage =
                    ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.YesNoButtons);
                pBCustomerFound.BackgroundImage = ThemeManager.CurrentThemeImages.CustomerFoundImage;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors in Customer Found screen", ex);
                KioskStatic.logToFile("Error setting Custom Images in Customer Found screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.CustomerFoundBtnOKTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CustomerFoundBtnPrevTextForeColor;
                this.lblCustomerName.ForeColor = KioskStatic.CurrentTheme.CustomerFoundLblCustomerNameTextForeColor;
                this.lblWelcomeMsg.ForeColor = KioskStatic.CurrentTheme.CustomerFoundLblWelcomeMsgTextForeColor;
                this.lblClickOKMsg.ForeColor = KioskStatic.CurrentTheme.CustomerFoundLblClickOKMsgTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error setting customized font colors in Customer Found screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

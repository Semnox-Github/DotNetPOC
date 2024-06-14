/********************************************************************************************
* Project Name - Parafait_Kiosk - frmAddToCartAlert
* Description  - frmAddToCartAlert 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
********************************************************************************************* 
*2.150.5     17-Oct-2023      Vignesh Bhat      New alert messsage box 
 ********************************************************************************************/
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using System; 
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmAddToCartAlert : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private frmChooseProduct.ProceedAction kioskProceedAction = null;
        /// <summary>
        /// GetKioskTransaction
        /// </summary>
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        /// <summary>
        /// frmAddToCartAlert
        /// </summary>
        /// <param name="kioskTransaction"></param>
        public frmAddToCartAlert(KioskTransaction kioskTransaction, frmChooseProduct.ProceedAction kioskProceedAction)
        {
            log.LogMethodEntry("kioskTransaction", "kioskProceedAction");
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;
            this.kioskProceedAction = kioskProceedAction;
            this.lblmsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
            StartKioskTimer();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }
        /// <summary>
        /// KioskTimer_Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisbleButtons();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisbleButtons();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                if (kioskProceedAction != null)
                {
                    this.Close();
                    kioskProceedAction(kioskTransaction);
                }
                else
                {
                    KioskStatic.logToFile("Unable to check out. kioskProceedAction is not defined.");
                }
            }
            catch (Exception ex)
            {
                this.Show();
                StartKioskTimer();
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message); 
                this.Close();
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit(); 
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry(); 
            try
            {
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.frmAddToCartAlertBtnCloseTextForeColor;
                this.btnClose.BackgroundImage = this.btnCheckOut.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.AddToCartAlertButtons);
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.frmAddToCartAlertLblMsgTextForeColor;
                this.btnCheckOut.ForeColor = KioskStatic.CurrentTheme.frmAddToCartAlertBtnCheckOutTextForeColor;
                this.BackgroundImage = ThemeManager.CurrentThemeImages.OkMsgBoxShort;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmAddToCartAlert: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisbleButtons()
        {
            log.LogMethodEntry();
            try
            {
                this.btnCheckOut.Enabled = false;
                this.btnClose.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Errow in DisbleButtons", ex);
            }
            log.LogMethodExit();
        }


        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                this.btnCheckOut.Enabled = true;
                this.btnClose.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error("Errow in EnableButtons", ex);
            }
            log.LogMethodExit();
        }
    }
}

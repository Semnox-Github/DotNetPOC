/********************************************************************************************
 * Project Name - Portrait Kiosk
 * Description  - user interface -frmReceiptDeliveryModeOptions
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.150.1.0      27-Dec-2022     Vignesh Bhat        Created.
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
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;

namespace Parafait_Kiosk
{
    public partial class frmReceiptDeliveryModeOptions : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        private ExecutionContext executionContext;

        public frmReceiptDeliveryModeOptions(ExecutionContext executionContext, KioskTransaction kioskTransaction, bool isVirtualStore = false)
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.executionContext = executionContext;
            this.kioskTransaction = kioskTransaction;
            KioskStatic.setDefaultFont(this);
            if(isVirtualStore)
            {
                this.btnEmail.Visible = false;
                this.btnPrint.Location = new Point(131, this.btnPrint.Location.Y);
                this.btnNone.Location = new Point(562, this.btnNone.Location.Y);
            }
            SetCustomizedFontColors();
            DisplaybtnHome(false);
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.PRINT;
            this.Close();
            log.LogMethodExit();
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            using (frmGetEmailDetails fged = new frmGetEmailDetails(executionContext, kioskTransaction))
            {
                if (fged.ShowDialog() != DialogResult.Cancel)
                {
                    this.Close();
                }
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void btnNone_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.NONE;
            this.Close();
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
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmReceiptDeliveryModeOptions");
            try
            {
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.FrmReceiptDeliveryModeOptionsLblGreeting1TextForeColor;
                this.btnPrint.ForeColor = KioskStatic.CurrentTheme.FrmReceiptDeliveryModeOptionsBtnPrintTextForeColor;
                this.btnEmail.ForeColor = KioskStatic.CurrentTheme.FrmReceiptDeliveryModeOptionsBtnEmailTextForeColor;
                this.btnNone.ForeColor = KioskStatic.CurrentTheme.FrmReceiptDeliveryModeOptionsBtnNoneTextForeColor;
                this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.ReceiptModeBackgroundImage);
                this.btnPrint.BackgroundImage = ThemeManager.CurrentThemeImages.ReceiptModeBtnBackgroundImage;
                this.btnEmail.BackgroundImage = ThemeManager.CurrentThemeImages.ReceiptModeBtnBackgroundImage;
                this.btnNone.BackgroundImage = ThemeManager.CurrentThemeImages.ReceiptModeBtnBackgroundImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmReceiptDeliveryModeOptions: " + ex.Message);
            }
            log.LogMethodExit();
        }

        
    }
}

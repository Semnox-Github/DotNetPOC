/********************************************************************************************
* Project Name - Parafait_Kiosk - frmEntitlement
* Description  - frmEntitlement 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        05-Sep-2019      Deeksha            Added logger methods.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Drawing;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmEntitlement : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string selectedEntitlement = KioskTransaction.TIME_ENTITLEMENT;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        public frmEntitlement(string message)
        {
            log.LogMethodEntry(message);
            InitializeComponent();
            lblmsg.Text = message;
            btnTime.Text = MessageUtils.getMessage("Play Time");
            btnPoints.Text = MessageUtils.getMessage("Play Points");
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            try
            {
                lblmsg.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, lblmsg.Font.Size, lblmsg.Font.Style);
                btnTime.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, btnTime.Font.Size, btnTime.Font.Style);
                btnPoints.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, btnPoints.Font.Size, btnPoints.Font.Style);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            selectedEntitlement = KioskTransaction.TIME_ENTITLEMENT;
            KioskStatic.logToFile("ENTITLEMENT_TYPE is Time");
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }
        private void btnPoints_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            selectedEntitlement = KioskTransaction.CREDITS_ENTITLEMENT;
            KioskStatic.logToFile("ENTITLEMENT_TYPE is Credits");
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmEntitlments");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.FrmEntitlementLblMsgTextForeColor;
                this.btnTime.ForeColor = KioskStatic.CurrentTheme.FrmEntitlementBtnMsgTextForeColor;
                this.btnPoints.ForeColor = KioskStatic.CurrentTheme.FrmEntitlementBtnPointsTextForeColor;
                this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.EntitlementBox);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements  in frmEntitlments: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

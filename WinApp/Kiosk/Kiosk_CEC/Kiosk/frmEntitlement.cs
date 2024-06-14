/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - frmEntitlement.cs
 * 
 **************
 **Version Log
 ************** 
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0    30-Jun-2021      Dakshak             Theme changes to support customized Font ForeColor
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmEntitlement : BaseForm
    {
         //public bool isPoints = true;
        public string selectedEntitlement = "Time";
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        public frmEntitlement(string message)
        {
            log.LogMethodEntry(message);
            InitializeComponent();
            //this.Size = this.BackgroundImage.Size;
            KioskStatic.setDefaultFont(this);
            this.BackgroundImage = KioskStatic.CurrentTheme.TapCardBackgroundImage;
            btnPoints.BackgroundImage = btnTime.BackgroundImage = KioskStatic.CurrentTheme.ChooseEntitlementImage;
            lblmsg.Text = message;
            btnTime.Text = MessageUtils.getMessage("Play Time");
            btnPoints.Text = MessageUtils.getMessage("Play Points");
            SetCustomizedFontColors();
            //selectedEntitlement = entitlementType;
            log.LogMethodExit();
        }
        private void btnTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            selectedEntitlement = "Time";
            log.LogVariableState("Entitlement Type is", selectedEntitlement);
            KioskStatic.logToFile("ENTITLEMENT_TYPE is Time");
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        private void btnPoints_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            selectedEntitlement = "Credits";
            log.LogVariableState("Entitlement Type is", selectedEntitlement);
            KioskStatic.logToFile("ENTITLEMENT_TYPE is Credits");
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void frmEntitlement_FormClosing(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmEntitlement_FormClosing");
            selectedEntitlement = "";
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.ChooseEntitlementHeaderTextForeColor;
                this.btnTime.ForeColor = KioskStatic.CurrentTheme.ChooseEntitlementBtnTimeTextForeColor;
                this.btnPoints.ForeColor = KioskStatic.CurrentTheme.ChooseEntitlemenBtnPointsTextForeColor;
                this.btnOk.ForeColor = KioskStatic.CurrentTheme.ChooseEntitlementBtnOkTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

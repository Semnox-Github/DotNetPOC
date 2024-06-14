/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for Waiver Mapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.151.2     27-Dec-2023   Sathyavathi        Created for Waiver Mapping Enhancements in Kiosk
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class UsrCtrlWaiverAlert : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int quantity;
        private string productName;
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;

        public UsrCtrlWaiverAlert(ExecutionContext executionContext, string productName, int quantity)
        {
            log.LogMethodEntry(executionContext, productName, quantity);

            InitializeComponent();
            this.executionContext = executionContext;
            this.quantity = quantity;
            this.productName = productName;
            SetDisplayElements();

            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            try
            {
                SetControlText();
                SetCustomImages();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error setting display elements in usrCtrlWaiverAlert" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetControlText()
        {
            log.LogMethodEntry();
            lblProductName.Text = productName;
            lblParticipants.Text = quantity.ToString() + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "participant(s)");
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.WaiverSigningAlertUsrCtrlBackground;
            this.pbxWaiverIcon.Image = ThemeManager.CurrentThemeImages.WaiverIconImage;
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            this.lblProductName.ForeColor = KioskStatic.CurrentTheme.UsrCtrlWaiverSigningAlertLblProductNameTextForeColor;
            this.lblParticipants.ForeColor = KioskStatic.CurrentTheme.UsrCtrlWaiverSigningAlertLblParticipantsTextForeColor;
            log.LogMethodExit();
        }
    }
}

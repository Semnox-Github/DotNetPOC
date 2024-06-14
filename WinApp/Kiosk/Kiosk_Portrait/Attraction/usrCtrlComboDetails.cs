/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for Combo Details Screen
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.155.0.0   06-Jun-2023      Sathyavathi        Created for Attraction Sale in Kiosk
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;

namespace Parafait_Kiosk
{
    public partial class UsrCtrlComboDetails : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        internal string ComboChildIndex { set { lblNum.Text = value; }}

        public UsrCtrlComboDetails(ExecutionContext executionContext, string childProductInfo)
        {
            log.LogMethodEntry(executionContext, childProductInfo);
            InitializeComponent();
            this.executionContext = executionContext;
            try
            {
                lblChildPackageInfo.Text = childProductInfo;
                this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.AttractionsComboProductBackgroundImage;
                this.pnlNum.BackgroundImage = ThemeManager.CurrentThemeImages.SmallCircleImage;
                SetCustomizedFontColors();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error setting display elements in usrCtrlComboChildProductsQty" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                lblChildPackageInfo.ForeColor = KioskStatic.CurrentTheme.UsrCtrlComboDetailsProductNameTextForeColor;
                lblNum.ForeColor = KioskStatic.CurrentTheme.UsrCtrlComboDetailsNumberTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("ERROR: Error while setting customized font colors for the UI elements of usrCtrlComboDetails: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

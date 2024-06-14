/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for Attraction Summary Screen
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.155.0.0   06-Jun-2023      Sathyavathi        Created for Attraction Sale in Kiosk
 *2.152.0.0   12-Dec-2023      Suraj Pai          Modified for Attraction Sale in TableTop Kiosk
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Core.GenericUtilities;

namespace Parafait_Kiosk
{
    public partial class UsrCtrlAttractionSummary : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductsContainerDTO childProductsContainerDTO;
        private string slotInfo;
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private string DATETIMEFORMAT;

        public UsrCtrlAttractionSummary(ExecutionContext executionContext, KioskAttractionDTO kioskAttractionDTO, KioskAttractionChildDTO childDTO)
        {
            log.LogMethodEntry(executionContext, kioskAttractionDTO, childDTO);
            InitializeComponent();
            this.executionContext = executionContext;
            DATETIMEFORMAT = KioskHelper.GetUIDateTimeFormat(executionContext);
            string slotInfo = string.Empty;  
            if (childDTO.ChildAttractionBookingDTO != null)
            {
                slotInfo = childDTO.ChildAttractionBookingDTO.AttractionScheduleName + ": "
                         + childDTO.ChildAttractionBookingDTO.ScheduleFromDate.ToString(DATETIMEFORMAT);
            }
            string productName = KioskHelper.GetProductName(childDTO.ChildProductId);
            string productInfo = (childDTO.ChildProductQuantity * kioskAttractionDTO.Quantity) + " x " + productName;
            this.slotInfo = slotInfo;
            try
            {
                lblProductName.Text = productInfo;
                lblSlotDetails.Text = slotInfo;
                this.pnlUsrCtrl.BackgroundImage = ThemeManager.CurrentThemeImages.AttractionsComboProductBackgroundImage;
                this.pbxSelectd.BackgroundImage = ThemeManager.CurrentThemeImages.CheckboxSelected;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error setting display elements in usrCtrlAttrcationSummary" + ex.Message);
            }
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                this.lblProductName.ForeColor = KioskStatic.CurrentTheme.UsrCtrlAttrcationSummaryLblProductNameTextForeColor;
                this.lblSlotDetails.ForeColor = KioskStatic.CurrentTheme.UsrCtrlAttrcationSummaryLblSlotDetailsTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("ERROR: Error while setting customized font colors for the UI elements of usrCtrlAttrcationSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

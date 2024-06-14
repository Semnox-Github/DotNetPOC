/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for CheckIn Summary screen
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.0.0    21-Oct-2021   Sathyavathi             Created for Check-In feature Phase-2
 *2.150.1      22-Feb-2023   Sathyavathi             Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class UsrCtrlCheckInSummary : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private int parentWidth;
        private Font parentFont;

        public string LblPackage { get { return lblPackage.Text; } set { lblPackage.Text = value; } }
        public string LblDiscountPerc { get { return lblDiscountPerc.Text; } set { lblDiscountPerc.Text = value; } }
        public string LblQuantity { get { return lblQuantity.Text; } set { lblQuantity.Text = value; } }
        public string LblPrice { get { return lblPrice.Text; } set { lblPrice.Text = value; } }
        public string LblTax { get { return lblTax.Text; } set { lblTax.Text = value; } }
        public string LblTotal { get { return lblTotal.Text; } set { lblTotal.Text = value; } }

        public UsrCtrlCheckInSummary(ExecutionContext executionContext, ProductsContainerDTO prodsContainerDTO, int parentWidth, Font font)
        {
            log.LogMethodEntry(executionContext, prodsContainerDTO, parentWidth, font);
            InitializeComponent();
            this.executionContext = executionContext;
            this.parentWidth = parentWidth;
            this.parentFont = font;
            try
            {
                SetBackgroundImage();
                SetDisplayElements(prodsContainerDTO);
                SetCustomizedFontAndFontColors();
            }
            catch (Exception ex)
            {
                log.Error("Error while executing usrCtrlCheckInSummary(): ", ex);
                KioskStatic.logToFile("Error while executing usrCtrlCheckInSummary(): " + ex);
            }
            log.LogMethodExit();
        }

        private void SetBackgroundImage()
        {
            log.LogMethodEntry();
            this.panelUsrCtrl.BackgroundImage = ThemeManager.CurrentThemeImages.PurchaseSummaryTableImage;
            log.LogMethodExit();
        }

        private void SetDisplayElements(ProductsContainerDTO prodsContainerDTO)
        {
            log.LogMethodEntry(prodsContainerDTO);
            log.LogMethodExit();
        }

        private void SetCustomizedFontAndFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements of usrCtrlPurchaseSummary");
            try
            {
                foreach (Control c in this.Controls["panelUsrCtrl"].Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.Font = new Font(parentFont.FontFamily, parentFont.Size/2, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        c.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryPanelUsrCtrlLblTextForeColor; // Color.DarkSlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors in usrCtrlCheckInSummary", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of usrCtrlCheckInSummary: " + ex);
            }
            log.LogMethodExit();
        }
    }
}

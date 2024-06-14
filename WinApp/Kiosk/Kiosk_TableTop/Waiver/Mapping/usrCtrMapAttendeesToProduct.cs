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
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class UsrCtrlMapAttendeesToProduct : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productIndex;
        private int productId;

        internal Point SetLocation { set { this.Location = this.btnProduct.Location = value; } }
        internal Padding SetMargin { set { this.Margin = value; } }

        public UsrCtrlMapAttendeesToProduct(int productIndex, int productId)
        {
            log.LogMethodEntry(productIndex);
            this.productIndex = productIndex;
            this.productId = productId;
            InitializeComponent();
            btnProduct.Tag = productId;
            SetDisplayElements();
            log.LogMethodExit();
        }

        internal void SelectProduct()
        {
            log.LogMethodEntry();

            //this.Padding = new Padding(0, 0, 0, 0);
            //this.Margin = new Padding(0, 0 , 3, 0);
            this.Size = new System.Drawing.Size(92, 69);
            this.btnProduct.Size = new System.Drawing.Size(92, 67);
            this.Location = this.btnProduct.Location = new System.Drawing.Point(0, 0);
            this.btnProduct.BackgroundImage = ThemeManager.CurrentThemeImages.SmallCircleSelected;
            this.btnProduct.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeeToProductBtnProductHighlightTextForeColor;
            log.LogMethodExit();
        }

        internal void UnselectProduct()
        {
            log.LogMethodEntry();
            //this.btnProduct.Padding = new Padding(0, 0, 0, 0);
            //this.Margin = new Padding(0, 0, 0, 0);
            this.Size = new System.Drawing.Size(68, 58);
            this.btnProduct.Size = new System.Drawing.Size(68, 56);
            this.Location = this.btnProduct.Location = new System.Drawing.Point(0, 3);
            btnProduct.BackgroundImage = ThemeManager.CurrentThemeImages.SmallCircleUnselected;
            this.btnProduct.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeeToProductBtnProductTextForeColor;
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
                KioskStatic.logToFile("Error in SetDisplayElements() of usrCtrlMapAttendeesToProduct: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetControlText()
        {
            log.LogMethodEntry();
            btnProduct.Text = productIndex.ToString();
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            btnProduct.BackgroundImage = ThemeManager.CurrentThemeImages.SmallCircleUnselected;
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            this.btnProduct.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeeToProductBtnProductTextForeColor;
            log.LogMethodExit();
        }
    }
}

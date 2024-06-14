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
    public partial class UsrCtrlMapAttendeesToQuantity : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private int quantity;
        private string assignParticipantText;
        private bool isSelected = false;

        internal delegate void SelectedDelegate(int lineNumber);
        internal SelectedDelegate selectedLine;

        internal void SetBGImageToIndicateMappingCompleted(Image value)
        {
            SetBGImage(value);
        }

        private void SetBGImage(Image image)
        {
            log.LogMethodEntry(image);
            this.btnProductQty.BackgroundImage = image;
            log.LogMethodExit();
        }

        internal int Line { get { return quantity; } }
        internal bool IsSelected { get { return isSelected; } set { isSelected = value; UpdateBackgroundImage(); } }
        internal bool IsMapped { get; set; } = false;

        internal void IndicateUsrCtrlAsMappingCompleted(bool value)
        {
            this.pbxMappingCompleted.Visible = value;
        }

        public UsrCtrlMapAttendeesToQuantity(ExecutionContext executionContext, int qty, string assignParticipantText)
        {
            log.LogMethodEntry(executionContext, qty, assignParticipantText);
            InitializeComponent();
            this.executionContext = executionContext;
            this.quantity = qty;
            this.assignParticipantText = assignParticipantText;
            this.pbxMappingCompleted.Visible = false;
            SetDisplayElements();
            log.LogMethodExit();
        }

        public void UsrCtrlMapAttendees_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (!isSelected)
                {
                    this.btnParticipantQty.BackgroundImage = ThemeManager.CurrentThemeImages.SlotSelectedBackgroundImage;
                    IsSelected = true;
                }
                else
                {
                    this.btnParticipantQty.BackgroundImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
                    IsSelected = false;
                }
                if (selectedLine != null)
                {
                    selectedLine(this.Line);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in usrControl_Click() of usrCtrlMapAttendeeToQuantity : " + ex.Message);
            }
            log.LogMethodExit();
        }

        internal void UpdateBtnParticipantQty(string customerName)
        {
            log.LogMethodEntry();
            this.btnParticipantQty.Text = customerName;
            this.btnParticipantQty.ForeColor = KioskStatic.CurrentTheme.MapAttendeesMappedCustomerNameTextForeColor;
            this.btnProductQty.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityLblQtyTextForeColor;
            log.LogMethodExit();
        }

        private void UpdateBackgroundImage()
        {
            log.LogMethodEntry();
            if (isSelected)
            {
                if (!IsMapped)
                {
                    this.btnProductQty.BackgroundImage = ThemeManager.CurrentThemeImages.PersonSelectedIcon;
                }
                this.btnParticipantQty.BackgroundImage = ThemeManager.CurrentThemeImages.SlotSelectedBackgroundImage;
                this.btnProductQty.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityLblQtySelectedTextForeColor;
            }
            else
            {
                if (!IsMapped)
                {
                    this.btnProductQty.BackgroundImage = ThemeManager.CurrentThemeImages.PersonIcon;
                }
                this.btnParticipantQty.BackgroundImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
                this.btnProductQty.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityLblQtyTextForeColor;
            }
            log.LogMethodExit();
        }

        internal void SetBtnTextForeColor()
        {
            log.LogMethodEntry();
            if (isSelected)
            {
                btnParticipantQty.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityBtnProductQtySelectedTextForeColor;
            }
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            try
            {
                SetDisplayText();
                SetCustomImages();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetDisplayElements() of usrCtrlMapAttendeesToQuantity: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetDisplayText()
        {
            log.LogMethodEntry();
            //this.lblQty.Text = (quantity < 10) ? quantity.ToString("D2") : quantity.ToString();
            this.btnProductQty.Text = (quantity < 10) ? quantity.ToString("D2") : quantity.ToString();
            btnParticipantQty.Text = assignParticipantText;
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            this.btnParticipantQty.BackgroundImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
            this.btnProductQty.BackgroundImage = ThemeManager.CurrentThemeImages.PersonIcon;
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            btnParticipantQty.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityBtnProductQtySelectedTextForeColor;
            this.btnProductQty.ForeColor = KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityLblQtyTextForeColor;
            log.LogMethodExit();
        }
    }
}

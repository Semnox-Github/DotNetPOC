/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - User control Waiver Mapping
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
    public partial class UsrCtrlCustomer : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string customerName;
        internal const string EXPAND = "EXPAND";
        internal const string COLLAPSE = "COLLAPSE";

        internal delegate void Delegate(UsrCtrlCustomer parentCustomer);
        internal Delegate selctedParent;

        internal void SetHideExpandCollapseButton(bool value)
        {
            pnlExpandCollapse.Visible = !value;
        }

        public UsrCtrlCustomer(string customerName)
        {
            log.LogMethodEntry(customerName);
            InitializeComponent();
            this.customerName = customerName;
            this.Tag = COLLAPSE;
            SetDisplayElements();
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            try
            {
                SetOnScreenMessages();
                SetCustomImages();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetDisplayElements of usrCtrlCustomer" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetOnScreenMessages()
        {
            log.LogMethodEntry();
            lblCustomer.Text = customerName;
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
            this.pnlExpandCollapse.BackgroundImage = ThemeManager.CurrentThemeImages.Collapse;
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            lblCustomer.ForeColor = KioskStatic.CurrentTheme.UsrCtrlCustomerNameTextForeColor;
            log.LogMethodExit();
        }

        private void pnlExpandCollapse_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ExpandCollapse();
            if (selctedParent != null && pnlExpandCollapse.Visible == true)
            {
                selctedParent(this);
            }
            log.LogMethodExit();
        }

        private void ExpandCollapse()
        {
            log.LogMethodEntry();
            if (this.Tag.Equals(EXPAND))
            {
                this.Tag = COLLAPSE;
                pnlExpandCollapse.BackgroundImage = ThemeManager.CurrentThemeImages.Expand;
            }
            else
            {
                this.Tag = EXPAND;
                pnlExpandCollapse.BackgroundImage = ThemeManager.CurrentThemeImages.Collapse;
            }
            log.LogMethodExit();
        }

        internal void TriggerExpandCollapse()
        {
            ExpandCollapse();
            if (selctedParent != null)
            {
                selctedParent(this);
            }
        }
    }
}

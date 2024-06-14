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
    public partial class UsrCtrlGroupOwners : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerId = -1;
        private string customerName = string.Empty;
        private bool isSelected = false;
        internal delegate void Delegate(UsrCtrlGroupOwners usrCtrl);
        internal Delegate selctedParent;

        internal bool IsSelected { get { return isSelected; } set { isSelected = value; UpdateCheckboxImage(); } }

        public UsrCtrlGroupOwners (string customerName, int customerId)
        {
            log.LogMethodEntry(customerName, customerId);
            InitializeComponent();
            this.customerName = customerName;
            this.customerId = customerId;
            this.Tag = customerId;
            SetDisplayElements();
            log.LogMethodExit();
        }

        public void usrControl_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (isSelected == false)
                {
                    if (selctedParent != null)
                    {
                        selctedParent(this);
                        isSelected = true;
                        UpdateCheckboxImage();
                    }
                }
                else
                {
                    if (selctedParent != null)
                    {
                        selctedParent(this);
                        isSelected = false;
                        UpdateCheckboxImage();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in usrControl_Click() of UsrCtrlGroupOwners: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void UpdateCheckboxImage()
        {
            this.pbxSelectd.BackgroundImage = (isSelected == true) ? Parafait_Kiosk.Properties.Resources.NewTickedCheckBox
                : Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
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
                KioskStatic.logToFile("Error in SetDisplayElements of UsrCtrlGroupOwners" + ex.Message);
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
            this.pbxSelectd.BackgroundImage = Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            lblCustomer.ForeColor = KioskStatic.CurrentTheme.UsrCtrlCustomerNameTextForeColor;
            log.LogMethodExit();
        }
    }
}

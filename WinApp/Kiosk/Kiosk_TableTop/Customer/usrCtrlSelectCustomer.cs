/********************************************************************************************
* Project Name - Parafait_Kiosk - Portrait
* Description  - user control for select customer screen
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.150.6    10-Nov-2023      Sathyavathi        Created for Customer Lookup
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using System.Drawing;

namespace Parafait_Kiosk
{
    public partial class usrCtrlSelectCustomer : UserControl
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        private bool isSelected = false;
        internal delegate void SelctedCustomerDelegate(CustomerDTO selectedCustomerDTO);
        internal delegate void UnSelectedCustomerDelegate(CustomerDTO selectedCustomerDTO);
        internal SelctedCustomerDelegate selectedCustomerMethod;
        internal UnSelectedCustomerDelegate unSelectCustomerMethod;
        private CustomerDTO customerDTO;

        const int TOTAL_NUM_OF_CHAR_TO_SHOW_FIRSTNAME = 8; //number of fixed length characters to show. If firstName < 8, make it 8.
        const int TOTAL_NUM_OF_CHAR_TO_SHOW_LASTNAME = 7; //number of fixed length characters to show. even lastName < 7, make it 7.
        const int NUM_OF_FIRSTXCHAR_TO_SHOW_FIRSTNAME = 4; //number of first x characters to show in first name
        const int NUM_OF_LASTXCHAR_TO_SHOW_LASTNAME = 3; //number of last x characters to show in last name

        public Image SetBackgroungImage { set { this.usrControlPanel.BackgroundImage = value; } }
        public bool IsSelected { get { return this.pbxSelectd.Visible; } set { this.pbxSelectd.Visible = value; } }

        public usrCtrlSelectCustomer(ExecutionContext executionContext, CustomerDTO customer)
        {
            log.LogMethodEntry(executionContext, customer);
            InitializeComponent();
            this.executionContext = executionContext;
            this.customerDTO = customer;
            SetDisplayElements();
            log.LogMethodExit();
        }

        public void usrControl_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (IsSelected == false)
                {
                    if (selectedCustomerMethod != null)
                    {
                        CustomerDTO retValue = customerDTO;
                        selectedCustomerMethod(retValue);
                        this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.SlotSelectedBackgroundImage;
                        IsSelected = true;
                    }
                }
                else
                {
                    if (unSelectCustomerMethod != null)
                    {
                        CustomerDTO retValue = customerDTO;
                        unSelectCustomerMethod(retValue);
                        IsSelected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in usrControl_Click() of UsrCtrlSelectCustomer : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();

            lblCustomerFirstName.Text = GetMaskedString(customerDTO.FirstName, TOTAL_NUM_OF_CHAR_TO_SHOW_FIRSTNAME, NUM_OF_FIRSTXCHAR_TO_SHOW_FIRSTNAME, true);
            lblCustomerFirstName.Text += " ";
            lblCustomerLastName.Text = "";
            lblCustomerLastName.Text += GetMaskedString(customerDTO.LastName, TOTAL_NUM_OF_CHAR_TO_SHOW_LASTNAME, NUM_OF_LASTXCHAR_TO_SHOW_LASTNAME, false);
            IsSelected = false;
            SetCustomiImages();
            SetCustomizedFontColors();
        }

        private string GetMaskedString(string invputValue, int stringLength, int retainXChars, bool retainFirstChars)
        {
            log.LogMethodEntry(invputValue, stringLength, retainXChars, retainFirstChars);
            string outputValue = string.Empty;
            if (retainFirstChars)
            {
                outputValue = RetainFirstChars(invputValue, retainXChars);
            }
            else
            {
                outputValue = RetainLastChars(invputValue, retainXChars);
            }
            log.LogMethodExit(outputValue);
            return outputValue;
        }

        private static string RetainFirstChars(string invputValue, int retainXChars)
        {
            log.LogMethodEntry(invputValue, retainXChars);
            string outputValue = string.Empty;
            if (string.IsNullOrWhiteSpace(invputValue) || invputValue.Length < retainXChars)
            {
                outputValue = invputValue + "*";
                while (outputValue.Length < retainXChars)
                {
                    outputValue = outputValue + "*";
                }
            }
            else if (invputValue.Length >= retainXChars)
            {
                outputValue = invputValue.Substring(0, retainXChars);
            }
            outputValue = outputValue + "****";
            log.LogMethodExit(outputValue);
            return outputValue;
        }


        private static string RetainLastChars(string invputValue, int retainXChars)
        {
            log.LogMethodEntry(invputValue, retainXChars);
            string outputValue = string.Empty;
            if (string.IsNullOrWhiteSpace(invputValue) == false && invputValue.Length < retainXChars)
            {
                outputValue = "*" + invputValue;
                while (outputValue.Length < retainXChars)
                {
                    outputValue = "*" + outputValue;
                }
            }
            else if (invputValue.Length >= retainXChars)
            {
                outputValue = invputValue.Substring(invputValue.Length - retainXChars);
            }
            if (string.IsNullOrWhiteSpace(invputValue) == false)
            {
                outputValue = "****" + outputValue;
            }
            log.LogMethodExit(outputValue);
            return outputValue;
        }

        private void SetCustomiImages()
        {
            log.LogMethodEntry();
            try
            {
                this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.SelectCustomerUsrCtrlBackgroundImage;
                this.pbxSelectd.BackgroundImage = ThemeManager.CurrentThemeImages.CheckboxSelected;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors in UsrCtrlSelectCustomer", ex);
                KioskStatic.logToFile("Error setting Custom Images in UsrCtrlSelectCustomer: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                this.lblCustomerFirstName.ForeColor = KioskStatic.CurrentTheme.UsrCtrlSelectCustomerLblFirstNameTextForeColor;
                this.lblCustomerLastName.ForeColor = KioskStatic.CurrentTheme.UsrCtrlSelectCustomerLblLastNameTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetCustomizedFontColors() of UsrCtrlSelectCustomer : " + ex.Message);
                log.LogMethodExit();
            }
        }
    }
}

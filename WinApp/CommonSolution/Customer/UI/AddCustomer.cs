/********************************************************************************************
 * Project Name - Customer.UI
 * Description  - Class for  of AddCustomer      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 *2.90          03-July-2020     Girish Kundar   Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO  
 ********************************************************************************************/
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;


namespace Semnox.Parafait.Customer
{
    public partial class AddCustomer : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerId = -1;
        private int partnerId = -1;
        private ExecutionContext machineUserContext;
      
        public AddCustomer(Utilities _Utilities, int inCustomerId, int inPartnerId)
        {
            log.LogMethodEntry(_Utilities , inCustomerId, inPartnerId);
            InitializeComponent();
            customerId = inCustomerId;
            partnerId = inPartnerId;

            machineUserContext = ExecutionContext.GetExecutionContext();
            machineUserContext.SetUserId(_Utilities.ParafaitEnv.LoginID);
        
            if (_Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(_Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
         
            CustomersDTO customersDTO;
            if (customerId == -1)
            {
                ResetForm();
                customersDTO = new CustomersDTO();
            }
            else  //Load form
            {
                customersDTO = new Customers(machineUserContext, customerId).GetCustomersDTO;
            }
                
            txtFirstName.Text = customersDTO.FirstName;
            txtAddress1.Text = customersDTO.Address1;
            txtAddress2.Text = customersDTO.Address2;
            txtAddress3.Text = customersDTO.Address3;
            txtCity.Text = customersDTO.City;
            txtState.Text = customersDTO.State;
            txtPin.Text = customersDTO.PostalCode;
            txtCountry.Text = customersDTO.Country;
            txtEmail.Text = customersDTO.EmailId;
            txtContactPhone1.Text = customersDTO.Contact_phone1;
            txtContactPhone2.Text = customersDTO.Contact_phone2;
            txtBirthDate.Text = customersDTO.DateOfBirth.ToShortDateString();
            txtAnniversaryDate.Text = customersDTO.AnniversaryDate.ToShortDateString();
            DataSet dsgender = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("gender_code");
            dt.Columns.Add("gender");
            dt.Rows.Add(new object[2] { "M", "Male" });
            dt.Rows.Add(new object[2] { "F", "Female" });
            dt.Rows.Add(new object[2] { "N", "Not Set" });
            this.comboBoxGender.DataSource = dt;
            this.comboBoxGender.ValueMember = "gender_code";
            this.comboBoxGender.DisplayMember = "gender";
            comboBoxGender.SelectedValue = customersDTO.Gender;
            txtNotes.Text = customersDTO.Notes;
            log.LogMethodExit();
        }

         

        public void ResetForm()
        {
            log.LogMethodEntry();
            txtFirstName.Text="";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtPin.Text = "";
            txtCountry.Text = "";
            txtEmail.Text = "";
            txtContactPhone1.Text = "";
            txtContactPhone2.Text = "";
            txtBirthDate.Text  = ""  ;
            txtAnniversaryDate.Text = ""  ;
            comboBoxGender.SelectedValue=-1;
            txtNotes.Text = "";
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomersDTO customersDTO = new CustomersDTO();
                if (customerId != -1)
                {
                    customersDTO = new Customers(machineUserContext, customerId).GetCustomersDTO;
                }

                bool validate = ValidateForm();

                if (validate)
                {
                    Customers customers = new Customers(machineUserContext, customersDTO);
                    customersDTO.FirstName = txtFirstName.Text;
                    customersDTO.Address1 = txtAddress1.Text;
                    customersDTO.Address2 = txtAddress2.Text;
                    customersDTO.Address3 = txtAddress3.Text;
                    customersDTO.City = txtCity.Text;
                    customersDTO.State = txtState.Text;
                    customersDTO.PostalCode = txtPin.Text;
                    customersDTO.Country = txtCountry.Text;
                    customersDTO.EmailId = txtEmail.Text;
                    customersDTO.Contact_phone1 = txtContactPhone1.Text;
                    customersDTO.Contact_phone2 = txtContactPhone2.Text;
                    customersDTO.DateOfBirth = txtBirthDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtBirthDate.Text);
                    customersDTO.AnniversaryDate = txtAnniversaryDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtAnniversaryDate.Text);
                    switch (comboBoxGender.Text)
                    {
                        case "Male": customersDTO.Gender = "M"; break;
                        case "Female": customersDTO.Gender = "F"; break;
                        case "Not Set": customersDTO.Gender = "N"; break;
                        default: customersDTO.Gender = "N"; break;
                    }
                    customersDTO.Notes = txtNotes.Text;

                    int custId = customers.Save();
 
                    if (customerId == -1)
                    {
                        Partners partners = new Partners(machineUserContext, partnerId);
                        PartnersDTO partnersDTO = partners.GetPartnersDTO;
                        partnersDTO.Customer_Id = custId;
                        partners.Save();
                    }

                    ResetForm();
                    this.Close();
                    log.LogMethodExit();
                }
                    
            }
            catch (Exception expn)
            {
                MessageBox.Show(expn.Message.ToString());
                log.Error("Error occurred :", expn);
                log.LogMethodExit(null, "Exception at btnSave_Click()" + expn.Message);
            }
        }

        public bool ValidateForm()
        {
            log.LogMethodEntry();
            if (txtFirstName.Text == ""   )
            {
                MessageBox.Show("Please enter First name.");
                return false;
            }
              
            if (txtAddress1.Text == "")
            {
                MessageBox.Show("Please enter Address1. ");
                return false;
            }
              if (txtCity.Text == "")
            {
                MessageBox.Show("Please enter City .");
                return false;
            }
              if (txtState.Text == "")
            {
                MessageBox.Show("Please enter State. ");
                return false;
            }
              if (txtPin.Text == "")
            {
                MessageBox.Show("Please enter Pincode. ");
                return false;
            }
              if (txtCountry.Text == "")
            {
                MessageBox.Show("Please enter Country. ");
                return false;
            }
              if (txtEmail.Text == "")
            {
                MessageBox.Show("Please enter Email. ");
                return false;
            }
              if (txtEmail.Text != "")
            {
                string email = txtEmail.Text;
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!(match.Success))
                {
                    MessageBox.Show("Please enter Valid Email. ");
                    return false;
                }
            }

            if (txtContactPhone1.Text == "")
            {
                MessageBox.Show("Please enter Contact Phone 1 .");
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private void AllowNumbersKeyPressed(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (Char.IsDigit(e.KeyChar)) return;
            if (Char.IsControl(e.KeyChar)) return;
            e.Handled = true;
        }
        private void AllowCharKeyPressed(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (Char.IsLetter(e.KeyChar)) return;
            if (Char.IsControl(e.KeyChar)) return;
            e.Handled = true;
        }
    }
}

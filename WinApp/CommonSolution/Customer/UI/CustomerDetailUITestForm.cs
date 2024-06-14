
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CustomerDetailUITestForm : Form
    {
        private CustomerDetailUI customerDetailUI;
        private Utilities utilities;
        /// <summary>
        /// 
        /// </summary>

        public CustomerDetailUITestForm(Utilities utilities)
        {
            InitializeComponent();
            customerDetailUI = new CustomerDetailUI(utilities, MessageBox.Show);
            this.utilities = utilities;
            Controls.Add(customerDetailUI);
            //CustomerBL customerBL = new CustomerBL(executionContext, 840);
            CustomerDTO customerDTO = new CustomerDTO();
            customerDetailUI.CustomerDTO = customerDTO;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<ValidationError> validationErrorList;
            customerDetailUI.ClearValdationErrors();
            validationErrorList = customerDetailUI.UpdateCustomerDTO();
            if (validationErrorList.Count > 0)
            {
                customerDetailUI.ShowValidationError(validationErrorList);
            }
            CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDetailUI.CustomerDTO);
            try
            {
                customerBL.Save(null);
            }
            catch (ValidationException ex)
            {
                customerDetailUI.ShowValidationError(ex.ValidationErrorList);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLookup_Click(object sender, EventArgs e)
        {
            CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities);
            if (customerLookupUI.ShowDialog() == DialogResult.OK)
            {
                customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                if (customerDetailUI.CustomerDTO != null)
                {
                    CustomerVerificationUI customerVerificationUI = new CustomerVerificationUI(utilities, customerDetailUI.CustomerDTO, MessageBox.Show);
                    if (customerVerificationUI.ShowDialog() == DialogResult.OK)
                    {
                        customerDetailUI.RefreshBindings();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowStatusMessage(string status, string type)
        {
            lblStatus.Text = type + " " + status;
        }

        private void btnRelationship_Click(object sender, EventArgs e)
        {

        }
    }
}

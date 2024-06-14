/********************************************************************************************
 * Project Name - Customer.UI
 * Description  - Class for  of CustomerDetailForm      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Customer details form
    /// </summary>
    public partial class CustomerDetailForm : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerDTO customerDTO;
        private Utilities utilities;
        private MessageBoxDelegate messageBoxDelegate;
        private CustomerDetailUI customerDetailUI;
        private bool editable = true;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities">parafait utilities</param>
        /// <param name="customerDTO">customerDTO</param>
        /// <param name="messageBoxDelegate">message delegate</param>
        /// <param name="showKeyboardOnTextboxEntry">show keyboard on text box entry</param>
        public CustomerDetailForm(Utilities utilities, CustomerDTO customerDTO, MessageBoxDelegate messageBoxDelegate, bool showKeyboardOnTextboxEntry = true)
        {
            log.LogMethodEntry(utilities, customerDTO, messageBoxDelegate);
            InitializeComponent();
            lblMessage.Text = "";
            this.utilities = utilities;
            utilities.setLanguage(this);
            this.messageBoxDelegate = messageBoxDelegate;
            this.customerDTO = customerDTO;
            customerDetailUI = new CustomerDetailUI(utilities, messageBoxDelegate, showKeyboardOnTextboxEntry);
            customerDetailUI.SetBackGroundColor(this.BackColor);
            customerDetailUI.CustomerContactInfoEntered += CustomerDetailUI_CustomerContactInfoEntered;
            customerDetailUI.FirstNameLeave += CustomerDetailUI_FirstNameLeave;
            customerDetailUI.UniqueIdentifierValidating += CustomerDetailUI_UniqueIdentifierValidating;
            Controls.Add(customerDetailUI);
            //SetControlsEnabled(false);
            log.LogMethodExit();
        }

        private void CustomerDetailUI_UniqueIdentifierValidating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if ((sender as TextBox).Text.Trim() == "")
                return;

            List<CustomerDTO> customerDTOList = null;
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, (sender as TextBox).Text.Trim());
            customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {

                if (messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 212, customerDTOList[0].FirstName + (customerDTOList[0].LastName == "" ? "" : " " + customerDTOList[0].LastName)), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Details"), MessageBoxButtons.OK) == System.Windows.Forms.DialogResult.OK)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ALLOW_DUPLICATE_UNIQUE_ID") == false)
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 290), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Details"));
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ALLOW_DUPLICATE_UNIQUE_ID") == false)
                    {
                        e.Cancel = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CustomerDetailUI_FirstNameLeave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
            customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, customerDetailUI.FirstName)
                                  .OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                if(customerDTOList.Count == 1 && customerDTOList[0].Id == customerDetailUI.CustomerDTO.Id)
                {
                    return;
                }
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, customerDetailUI.FirstName);
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    if (customerLookupUI.SelectedCustomerDTO != null)
                    {
                        customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                        customerDTO = customerLookupUI.SelectedCustomerDTO;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CustomerDetailUI_CustomerContactInfoEntered(object source, CustomerContactInfoEnteredEventArgs e)
        {
            log.LogMethodEntry(source, e);
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, e.ContactType.ToString());
            customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, e.ContactValue)
                                  .OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                if (customerDTOList.Count == 1 && customerDTOList[0].Id == customerDetailUI.CustomerDTO.Id)
                {
                    return;
                }
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, "","","",
                                                                                     e.ContactType == ContactType.EMAIL ? e.ContactValue : "",
                                                                                     e.ContactType == ContactType.PHONE ? e.ContactValue : "",
                                                                                     "");
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    if (customerLookupUI.SelectedCustomerDTO != null)
                    {
                        customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                        customerDTO = customerLookupUI.SelectedCustomerDTO;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CustomerDetailForm_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            customerDetailUI.CustomerDTO = customerDTO;
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblMessage.Text = "";
            List<ValidationError> validationErrorList;
            customerDetailUI.ClearValdationErrors();
            validationErrorList = customerDetailUI.UpdateCustomerDTO();
            if (validationErrorList.Count > 0)
            {
                lblMessage.Text = validationErrorList[0].Message;
                customerDetailUI.ShowValidationError(validationErrorList);
            }
            else
            {
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDetailUI.CustomerDTO);
                try
                {
                    validationErrorList = customerBL.Validate();
                    if (validationErrorList.Count > 0)
                    {
                        lblMessage.Text = validationErrorList[0].Message;
                        customerDetailUI.ShowValidationError(validationErrorList);
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (ValidationException ex)
                {
                    customerDetailUI.ShowValidationError(ex.ValidationErrorList);
                }
                catch (Exception ex)
                {
                    messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Details"));
                }
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// Changes whether the ui is readonly
        /// </summary>
        /// <param name="value"></param>
        public void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            editable = value;
            customerDetailUI.SetControlsEnabled(value);
            if(value == false)
            {
                btnSave.Visible = false;
                btnCustomerLookup.Visible = false;
                Point btnCloseLocation = btnClose.Location;
                btnClose.Location = new Point((this.Width - btnClose.Width) / 2, btnCloseLocation.Y);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the customer DTO
        /// </summary>

        public CustomerDTO CustomerDTO
        {
            get
            {
                return customerDTO;
            }
        }

        private void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities,
                                                                         customerDetailUI.FirstName,
                                                                         customerDetailUI.MiddleName,
                                                                         customerDetailUI.LastName,
                                                                         customerDetailUI.Email,
                                                                         customerDetailUI.PhoneNumber,
                                                                         customerDetailUI.UniqueIdentifier);
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    if (customerLookupUI.SelectedCustomerDTO != null)
                    {
                        customerDTO = customerLookupUI.SelectedCustomerDTO; //assign customerDTO details to trx customerDTO object
                        customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                    }
                }
            }
            catch (Exception ex)
            {
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Details"));
            }
            log.LogMethodExit();
        }

        private void CustomerDetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //customerDetailUI.UpdateCustomerDTO();
            //if(this.DialogResult != DialogResult.OK && editable)
            //{
            //    if (customerDetailUI.CustomerDTO.IsChangedRecursive)
            //    {
            //        DialogResult DR = messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 566), MessageContainerList.GetMessage(utilities.ExecutionContext, "Close Customers"), MessageBoxButtons.YesNoCancel);
            //        switch (DR)
            //        {
            //            case DialogResult.Yes:
            //                {
            //                    btnSave.PerformClick();
            //                    break;
            //                }
            //            case DialogResult.No: break;
            //            case DialogResult.Cancel: e.Cancel = true; break;
            //            default: break;
            //        }
            //    }
            //} 
            log.LogMethodExit();
        }
    }
}

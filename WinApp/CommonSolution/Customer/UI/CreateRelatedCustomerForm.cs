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
using System.Globalization;
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
    public partial class CreateRelatedCustomerForm : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerDTO relatedCustomerDTO;
        private int customerId;
        private CustomerRelationshipDTO customerRelationshipDTO;
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
        public CreateRelatedCustomerForm(Utilities utilities, int customerId, CustomerDTO relatedCustomerDTO, CustomerRelationshipDTO customerRelationshipDTO, MessageBoxDelegate messageBoxDelegate, bool showKeyboardOnTextboxEntry = true)
        {
            log.LogMethodEntry(utilities, relatedCustomerDTO, messageBoxDelegate);
            InitializeComponent();
            lblMessage.Text = "";
            this.utilities = utilities;
            utilities.setLanguage(this);
            this.messageBoxDelegate = messageBoxDelegate;
            this.customerId = customerId;
            this.relatedCustomerDTO = relatedCustomerDTO;
            this.customerRelationshipDTO = customerRelationshipDTO;
            customerDetailUI = new CustomerDetailUI(utilities, messageBoxDelegate, showKeyboardOnTextboxEntry);
            customerDetailUI.SetBackGroundColor(this.BackColor);
            customerDetailUI.CustomerContactInfoEntered += CustomerDetailUI_CustomerContactInfoEntered;
            customerDetailUI.FirstNameLeave += CustomerDetailUI_FirstNameLeave;
            customerDetailUI.UniqueIdentifierValidating += CustomerDetailUI_UniqueIdentifierValidating;
            customerDetailUI.Location = new Point(0, 70);
            Controls.Add(customerDetailUI);
            customerDetailUI.SetControlsEnabled(ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "REGISTER_CUSTOMER_WITHOUT_CARD"));
            if(relatedCustomerDTO.Id > -1)
            {
                btnCustomerLookup.Visible = false;
                customerDetailUI.SetControlsEnabled(true);
            }
            txtEffectiveDate.Cue = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT");
            txtExpiryDate.Cue = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT");
            //FormBorderStyle = FormBorderStyle.None;
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
                if (customerDTOList.Count == 1 && customerDTOList[0].Id == customerDetailUI.CustomerDTO.Id)
                {
                    return;
                }
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, customerDetailUI.FirstName);
                if (customerLookupUI.ShowDialog() == DialogResult.OK && customerLookupUI.SelectedCustomerDTO.Id != customerId)
                {
                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                    relatedCustomerDTO = customerLookupUI.SelectedCustomerDTO;
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
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, "", "", "",
                                                                                     e.ContactType == ContactType.EMAIL ? e.ContactValue : "",
                                                                                     e.ContactType == ContactType.PHONE ? e.ContactValue : "",
                                                                                     "");
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                    relatedCustomerDTO = customerLookupUI.SelectedCustomerDTO;
                }
            }
            log.LogMethodExit();
        }

        private void CustomerDetailForm_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            BindingSource customerRelationshipTypeBS = new BindingSource();
            customerRelationshipTypeBS.DataSource = GetCustomerRelationshipTypeDTOList();
            cmbCustomerRelationshipType.DisplayMember = "Description";
            cmbCustomerRelationshipType.ValueMember = "Id";
            cmbCustomerRelationshipType.DataSource = customerRelationshipTypeBS;
            customerDetailUI.CustomerDTO = relatedCustomerDTO;
            if(customerRelationshipDTO.EffectiveDate.HasValue)
            {
                dtpEffectiveDate.Value = customerRelationshipDTO.EffectiveDate.Value;
            }
            if(customerRelationshipDTO.ExpiryDate.HasValue)
            {
                dtpExpiryDate.Value = customerRelationshipDTO.ExpiryDate.Value;
            }
            cmbCustomerRelationshipType.SelectedValue = customerRelationshipDTO.CustomerRelationshipTypeId;
            flpButtons.Location = new Point((this.Width - flpButtons.Width) / 2, flpButtons.Location.Y);
            

            log.LogMethodExit();
        }

        private List<CustomerRelationshipTypeDTO> GetCustomerRelationshipTypeDTOList()
        {
            log.LogMethodEntry();
            List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = null;
            CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(utilities.ExecutionContext);
            List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchCustomerRelationshipTypeParams = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
            searchCustomerRelationshipTypeParams.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchCustomerRelationshipTypeParams.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchCustomerRelationshipTypeParams);
            if (customerRelationshipTypeDTOList == null)
            {
                customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
            }
            customerRelationshipTypeDTOList.Insert(0, new CustomerRelationshipTypeDTO());
            customerRelationshipTypeDTOList[0].Description = "SELECT";
            log.LogMethodExit(customerRelationshipTypeDTOList);
            return customerRelationshipTypeDTOList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
                Application.DoEvents();
                List<ValidationError> validationErrorList;
                customerDetailUI.ClearValdationErrors();
                validationErrorList = customerDetailUI.UpdateCustomerDTO();
                if (validationErrorList.Count > 0)
                {
                    lblMessage.Text = validationErrorList[0].Message;
                    customerDetailUI.ShowValidationError(validationErrorList);
                    return;
                }
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDetailUI.CustomerDTO);
                validationErrorList = customerBL.Validate();
                if (validationErrorList.Count > 0)
                {
                    lblMessage.Text = validationErrorList[0].Message;
                    customerDetailUI.ShowValidationError(validationErrorList);
                    return;
                }

                if(cmbCustomerRelationshipType.SelectedValue == null || 
                    (int)cmbCustomerRelationshipType.SelectedValue == -1)
                {
                    string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship Type"));
                    lblMessage.Text = errorMessage;
                    return;
                }
                DateTime? effectiveDate = GetEffectiveDate();
                DateTime? expiryDate = GetExpiryDate();
                if(expiryDate.HasValue && effectiveDate.HasValue && expiryDate.Value < effectiveDate.Value)
                {
                    string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 1445);
                    lblMessage.Text = errorMessage;
                    return;
                }
                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                {
                    if(ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "REGISTER_CUSTOMER_WITHOUT_CARD") ||
                        customerBL.CustomerDTO.Id >= 0)
                    {
                        parafaitDBTransaction.BeginTransaction();
                        customerBL.Save(parafaitDBTransaction.SQLTrx);
                        parafaitDBTransaction.EndTransaction();
                    }
                }

                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                {
                    parafaitDBTransaction.BeginTransaction();
                    customerRelationshipDTO.CustomerRelationshipTypeId = (int)cmbCustomerRelationshipType.SelectedValue;
                    if(customerRelationshipDTO.CustomerId == customerId)
                    {
                        customerRelationshipDTO.RelatedCustomerId = customerBL.CustomerDTO.Id;
                    }
                    else
                    {
                        customerRelationshipDTO.CustomerId = customerBL.CustomerDTO.Id;
                    }
                    customerRelationshipDTO.EffectiveDate = GetEffectiveDate();
                    customerRelationshipDTO.ExpiryDate = GetExpiryDate();
                    CustomerRelationshipBL customerRelationshipBL = new CustomerRelationshipBL(utilities.ExecutionContext, customerRelationshipDTO);
                    customerRelationshipBL.Save(parafaitDBTransaction.SQLTrx);
                    parafaitDBTransaction.EndTransaction();
                    lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Saved Successfully");
                }
            }
            catch (ValidationException ex)
            {
                customerDetailUI.ShowValidationError(ex.ValidationErrorList);
                lblMessage.Text = ex.ValidationErrorList[0].Message;
            }
            catch (Exception ex)
            {
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Create Related Customer"));
            }
            log.LogMethodExit();
        }

        private DateTime? GetExpiryDate()
        {
            DateTime? result = null;
            if (string.IsNullOrWhiteSpace(txtExpiryDate.Text) == false)
            {
                try
                {
                    DateTime expiryDate = DateTime.ParseExact(txtExpiryDate.Text, ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT"), CultureInfo.CurrentCulture);
                    result = expiryDate;
                }
                catch(Exception)
                {
                    log.LogMethodExit(null, "Invalid Expiry Date throwing Validation exception");
                    ValidationError validationError = new ValidationError("CustomerRelationship", "ExpiryDate", MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, "Expiry Date")));
                    throw new ValidationException("Invalid Expiry Date", new List<ValidationError>() { validationError });
                }
            }
            return result;
        }

        private DateTime? GetEffectiveDate()
        {
            DateTime? result = null;
            if (string.IsNullOrWhiteSpace(txtEffectiveDate.Text) == false)
            {
                try
                {
                    DateTime effectiveDate = DateTime.ParseExact(txtEffectiveDate.Text, ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT"), CultureInfo.CurrentCulture);
                    result = effectiveDate;
                }
                catch(Exception)
                {
                    log.LogMethodExit(null, "Invalid Effective Date throwing Validation exception");
                    ValidationError validationError = new ValidationError("CustomerRelationship", "EffectiveDate", MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, "Effective Date")));
                    throw new ValidationException("Invalid Effective Date", new List<ValidationError>() { validationError });
                }
            }
            return result;
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
            if (value == false)
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
                return relatedCustomerDTO;
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
                                                                         string.Empty,
                                                                         string.Empty,
                                                                         customerDetailUI.UniqueIdentifier);
                if (customerLookupUI.ShowDialog() == DialogResult.OK && customerLookupUI.SelectedCustomerDTO.Id != customerId)
                {
                    relatedCustomerDTO = customerLookupUI.SelectedCustomerDTO; //assign customerDTO details to trx customerDTO object
                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                    if(relatedCustomerDTO != null && relatedCustomerDTO.Id > -1)
                    {
                        customerDetailUI.SetControlsEnabled(true);
                    }
                }
            }
            catch (Exception ex)
            {
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Details"));
            }
            log.LogMethodExit();
        }

        private void dtpExpiryDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtExpiryDate.Text = dtpExpiryDate.Value.Date.ToString(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT"));
            log.LogMethodExit();
        }

        private void dtpEffectiveDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtEffectiveDate.Text = dtpEffectiveDate.Value.Date.ToString(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT"));
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

        private void CreateRelatedCustomerForm_Shown(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "REGISTER_CUSTOMER_WITHOUT_CARD") == false &&
                relatedCustomerDTO.Id == -1)
            {
                btnCustomerLookup.PerformClick();
            }
            log.LogMethodExit();
        }
    }
}

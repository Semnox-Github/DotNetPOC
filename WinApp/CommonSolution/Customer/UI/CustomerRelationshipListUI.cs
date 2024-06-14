
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using System.Drawing;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomerRelationship UI class
    /// </summary>
    public partial class CustomerRelationshipListUI : Form
    {
        /// <summary>
        /// Parafait utility 
        /// </summary>
        protected Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerDTO customerDTO;

        private MessageBoxDelegate messageBoxDelegate;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities">parafait utilities</param>
        /// <param name="customerDTO">Customer DTO</param>
        /// <param name="messageBoxDelegate">messageBox delegate</param>
        public CustomerRelationshipListUI(Utilities utilities, CustomerDTO customerDTO, MessageBoxDelegate messageBoxDelegate)
        {
            log.LogMethodEntry(utilities);
            this.customerDTO = customerDTO;
            this.utilities = utilities;
            this.messageBoxDelegate = messageBoxDelegate;
            InitializeComponent();
            utilities.setupDataGridProperties(ref dgvCustomerRelationshipDTOList);
            utilities.setLanguage(this);
            expiryDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT");
            effectiveDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT");
            this.Text += " - " + customerDTO.FirstName + " " + customerDTO.LastName;
            viewCustomerDetailDataGridViewButtonColumn.Text = "...";
            viewCustomerDetailDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            FormBorderStyle = FormBorderStyle.None;
            log.LogMethodExit();
        }

        private async void CustomerRelationshipListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnDataLoadStart();
            await RefreshData();
            OnDataLoadComplete();
            SortableBindingList<CustomerRelationshipDTO> customerRelationshipDTOSortableList = (SortableBindingList<CustomerRelationshipDTO>)customerRelationshipDTOListBS.DataSource;
            if (customerRelationshipDTOSortableList == null ||
                customerRelationshipDTOSortableList.Count == 0)
            {
                btnNew.PerformClick();
            }
            log.LogMethodExit();
        }


        #region OnDataLoadStart and OnDataLoadComplete
        private void OnDataLoadStart()
        {
            log.LogMethodEntry();
            DisableControls();
            lblStatus.Text = "Loading... Please wait...";
            log.LogMethodExit();
        }

        private void OnDataLoadComplete()
        {
            log.LogMethodEntry();
            lblStatus.Text = "";
            EnableControls();
            log.LogMethodExit();
        }
        #endregion

        #region Enable and Disable controls
        private void DisableControls()
        {
            log.LogMethodEntry();
            btnDelete.Enabled = false;
            btnClose.Enabled = false;
            btnRefresh.Enabled = false;
            btnSearch.Enabled = false;
            btnSave.Enabled = false;
            cmbSearchCustomerRelationshipType.Enabled = false;
            chbShowActiveEntries.Enabled = false;
            RefreshControls();
            log.LogMethodExit();
        }

        private void RefreshControls()
        {
            log.LogMethodEntry();
            btnDelete.Refresh();
            btnClose.Refresh();
            btnRefresh.Refresh();
            btnSearch.Refresh();
            btnSave.Refresh();
            cmbSearchCustomerRelationshipType.Refresh();
            chbShowActiveEntries.Refresh();
            log.LogMethodExit();
        }

        private void EnableControls()
        {
            log.LogMethodEntry();
            btnDelete.Enabled = true;
            btnClose.Enabled = true;
            btnRefresh.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = true;
            cmbSearchCustomerRelationshipType.Enabled = true;
            chbShowActiveEntries.Enabled = true;
            RefreshControls();
            log.LogMethodExit();
        }
        #endregion


        private async Task RefreshData()
        {
            log.LogMethodEntry();
            List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = await Task<List<CustomerRelationshipTypeDTO>>.Factory.StartNew(() => { return GetCustomerRelationshipTypeDTOList(); });
            BindingSource customerRelationshipTypeBS = new BindingSource();
            customerRelationshipTypeBS.DataSource = customerRelationshipTypeDTOList;

            customerRelationshipTypeBS = new BindingSource();
            customerRelationshipTypeBS.DataSource = customerRelationshipTypeDTOList;
            cmbSearchCustomerRelationshipType.DisplayMember = "Description";
            cmbSearchCustomerRelationshipType.ValueMember = "Id";
            cmbSearchCustomerRelationshipType.DataSource = customerRelationshipTypeBS;

            customerRelationshipTypeBS = new BindingSource();
            customerRelationshipTypeBS.DataSource = customerRelationshipTypeDTOList;
            customerRelationshipTypeIdDataGridViewComboBoxColumn.DisplayMember = "Description";
            customerRelationshipTypeIdDataGridViewComboBoxColumn.ValueMember = "Id";
            customerRelationshipTypeIdDataGridViewComboBoxColumn.DataSource = customerRelationshipTypeBS;

            await LoadCustomerRelationshipDTOList();
            log.LogMethodExit();
        }

        private async Task LoadCustomerRelationshipDTOList()
        {
            log.LogMethodEntry();
            List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchCustomerRelationshipParams = GetCustomerRelationshipSearchParams();
            IList<CustomerRelationshipDTO> customerRelationshipDTOList = null;
            customerRelationshipDTOList = await Task<List<CustomerRelationshipDTO>>.Factory.StartNew(() => { return GetCustomerRelationshipDTOList(searchCustomerRelationshipParams); });
            if (customerRelationshipDTOList == null)
            {
                customerRelationshipDTOList = new SortableBindingList<CustomerRelationshipDTO>();
            }
            else
            {
                customerRelationshipDTOList = new SortableBindingList<CustomerRelationshipDTO>(customerRelationshipDTOList);
            }
            customerRelationshipDTOListBS.DataSource = customerRelationshipDTOList;
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

        private List<CustomerRelationshipDTO> GetCustomerRelationshipDTOList(List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchCustomerRelationshipParams)
        {
            log.LogMethodEntry(searchCustomerRelationshipParams);
            List<CustomerRelationshipDTO> customerRelationshipDTOList = null;
            CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(utilities.ExecutionContext);
            customerRelationshipDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchCustomerRelationshipParams);
            log.LogMethodExit(customerRelationshipDTOList);
            return customerRelationshipDTOList;
        }

        private List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> GetCustomerRelationshipSearchParams()
        {
            log.LogMethodEntry();
            List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchCustomerRelationshipParams = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            if (Convert.ToInt32(cmbSearchCustomerRelationshipType.SelectedValue) != -1)
            {
                searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_RELATIONSHIP_TYPE_ID, Convert.ToString(cmbSearchCustomerRelationshipType.SelectedValue)));
            }
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
            log.LogMethodExit(searchCustomerRelationshipParams);
            return searchCustomerRelationshipParams;
        }

        private void dgvCustomerRelationshipDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, dgvCustomerRelationshipDTOList.Columns[e.ColumnIndex].HeaderText), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnDataLoadStart();
            await LoadCustomerRelationshipDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnDataLoadStart();
            cmbSearchCustomerRelationshipType.SelectedIndex = 0;
            chbShowActiveEntries.Checked = true;
            await RefreshData();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool error = false;
            dgvCustomerRelationshipDTOList.EndEdit();
            SortableBindingList<CustomerRelationshipDTO> customerRelationshipDTOSortableList = (SortableBindingList<CustomerRelationshipDTO>)customerRelationshipDTOListBS.DataSource;
            CustomerRelationshipBL customerRelationshipBL;
            if (customerRelationshipDTOSortableList != null && error == false)
            {
                for (int i = 0; i < customerRelationshipDTOSortableList.Count; i++)
                {
                    try
                    {
                        if (customerRelationshipDTOSortableList[i].IsChanged)
                        {
                            customerRelationshipBL = new CustomerRelationshipBL(utilities.ExecutionContext, customerRelationshipDTOSortableList[i]);
                            customerRelationshipBL.Save();
                        }
                    }
                    catch (ValidationException ex)
                    {
                        log.Error("validation failed", ex);
                        StringBuilder errorMessageBuilder = new StringBuilder("");
                        foreach (var validationError in ex.ValidationErrorList)
                        {
                            errorMessageBuilder.Append(validationError.Message);
                            errorMessageBuilder.Append(Environment.NewLine);
                        }
                        messageBoxDelegate(errorMessageBuilder.ToString(), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
                        error = true;
                        customerRelationshipDTOListBS.Position = i;
                        dgvCustomerRelationshipDTOList.Rows[i].Selected = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        log.Error("Exception occured while saving customer relationship", ex);
                        customerRelationshipDTOListBS.Position = i;
                        dgvCustomerRelationshipDTOList.Rows[i].Selected = true;
                        messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 718), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
                        break;
                    }
                }
            }
            else
            {
                if (error == false)
                {
                    messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 371), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
                }
            }
            if (!error)
            {
                btnSearch.PerformClick();
            }
            else
            {
                dgvCustomerRelationshipDTOList.Update();
                dgvCustomerRelationshipDTOList.Refresh();
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvCustomerRelationshipDTOList.SelectedRows.Count <= 0 && dgvCustomerRelationshipDTOList.SelectedCells.Count <= 0)
            {
                messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 959), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
                log.LogMethodExit(null, " rows selected. Please select the rows you want to delete and press delete..");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if (this.dgvCustomerRelationshipDTOList.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvCustomerRelationshipDTOList.SelectedCells)
                {
                    dgvCustomerRelationshipDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow row in dgvCustomerRelationshipDTOList.SelectedRows)
            {
                if (Convert.ToInt32(row.Cells[idDataGridViewTextBoxColumn.Index].Value.ToString()) <= 0)
                {
                    dgvCustomerRelationshipDTOList.Rows.RemoveAt(row.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 958), MessageContainerList.GetMessage(utilities.ExecutionContext, "Confirm Inactvation."), MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        refreshFromDB = true;
                        SortableBindingList<CustomerRelationshipDTO> customerRelationshipDTOSortableList = (SortableBindingList<CustomerRelationshipDTO>)customerRelationshipDTOListBS.DataSource;
                        CustomerRelationshipDTO customerRelationshipDTO = customerRelationshipDTOSortableList[row.Index];
                        customerRelationshipDTO.IsActive = false;
                        CustomerRelationshipBL customerRelationshipBL = new CustomerRelationshipBL(utilities.ExecutionContext, customerRelationshipDTO);
                        try
                        {
                            customerRelationshipBL.Save();
                        }
                        catch (ValidationException ex)
                        {
                            log.Error("Error occured while inactivating customer relationship", ex);
                            dgvCustomerRelationshipDTOList.Rows[row.Index].Selected = true;
                            StringBuilder errorMessageBuilder = new StringBuilder("");
                            foreach (var validationError in ex.ValidationErrorList)
                            {
                                errorMessageBuilder.Append(validationError.Message);
                                errorMessageBuilder.Append(Environment.NewLine);
                            }
                            messageBoxDelegate(errorMessageBuilder.ToString(), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
                            continue;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while inactivating customer relationship", ex);
                            dgvCustomerRelationshipDTOList.Rows[row.Index].Selected = true;
                            messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
                            continue;
                        }
                    }
                }
            }
            if (rowsDeleted == true)
            {
                messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 957), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
            }
            if (refreshFromDB == true)
            {
                btnSearch.PerformClick();
            }
            log.LogMethodExit();
        }

        private void dgvCustomerRelationshipDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == viewCustomerDetailDataGridViewButtonColumn.Index)
            {
                if (customerRelationshipDTOListBS.Current != null && customerRelationshipDTOListBS.Current is CustomerRelationshipDTO)
                {
                    CustomerRelationshipDTO customerRelationshipDTO = customerRelationshipDTOListBS.Current as CustomerRelationshipDTO;
                    int relatedCustomerId = customerRelationshipDTO.CustomerId;
                    if (customerRelationshipDTO.CustomerId == customerDTO.Id)
                    {
                        relatedCustomerId = customerRelationshipDTO.RelatedCustomerId;
                    }
                    CustomerBL relatedCustomerBL = new CustomerBL(utilities.ExecutionContext, relatedCustomerId);

                    using (CreateRelatedCustomerForm createRelatedCustomerForm = new CreateRelatedCustomerForm(utilities, customerDTO.Id, relatedCustomerBL.CustomerDTO, customerRelationshipDTO, messageBoxDelegate, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD")))
                    {
                        createRelatedCustomerForm.ShowDialog();
                    }
                    btnRefresh.PerformClick();
                }
            }
            log.LogMethodExit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerDTO relatedCustomerDTO = new CustomerDTO();
            relatedCustomerDTO.CustomerType = customerDTO.CustomerType;
            if (string.IsNullOrWhiteSpace(customerDTO.PhoneNumber) == false)
            {
                ContactDTO contactDTO = new ContactDTO(-1, -1, -1, ContactType.PHONE, customerDTO.PhoneNumber, string.Empty, true, string.Empty, false, -1);
                if (relatedCustomerDTO.ContactDTOList == null)
                {
                    relatedCustomerDTO.ContactDTOList = new List<ContactDTO>();
                }
                relatedCustomerDTO.ContactDTOList.Add(contactDTO);
            }
            if (string.IsNullOrWhiteSpace(customerDTO.Email) == false)
            {
                ContactDTO contactDTO = new ContactDTO(-1, -1, -1, ContactType.EMAIL, customerDTO.Email, string.Empty, true, string.Empty, false, -1);
                if (relatedCustomerDTO.ContactDTOList == null)
                {
                    relatedCustomerDTO.ContactDTOList = new List<ContactDTO>();
                }
                relatedCustomerDTO.ContactDTOList.Add(contactDTO);
            }
            if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0 && customerDTO.LatestAddressDTO != null)
            {
                AddressDTO addressDTO = customerDTO.LatestAddressDTO;
                if (addressDTO.Id > -1)
                {
                    AddressDTO relatedCustomerAddressDTO = new AddressDTO(-1, -1, addressDTO.AddressTypeId, addressDTO.AddressType, addressDTO.Line1, addressDTO.Line2, addressDTO.Line3, addressDTO.City, addressDTO.StateId, addressDTO.PostalCode, addressDTO.CountryId, addressDTO.StateCode, addressDTO.StateName, addressDTO.CountryName, addressDTO.IsDefault, true);
                    if (relatedCustomerDTO.AddressDTOList == null)
                    {
                        relatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
                    }
                    relatedCustomerDTO.AddressDTOList.Add(relatedCustomerAddressDTO);
                }
            }
            CustomerRelationshipDTO customerRelationshipDTO = new CustomerRelationshipDTO(-1, customerDTO.Id, -1, -1, customerDTO.FirstName + " " + customerDTO.LastName, string.Empty, null, null, true);
            using (CreateRelatedCustomerForm crcf = new CreateRelatedCustomerForm(utilities, customerDTO.Id, relatedCustomerDTO, customerRelationshipDTO, messageBoxDelegate, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD")))
            {
                crcf.ShowDialog();
            }
            btnRefresh.PerformClick();
            log.LogMethodExit();
        }

    }
}
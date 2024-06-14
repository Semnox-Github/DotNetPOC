using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.UI
{
    public partial class SelectRelatedCustomerUI : Form
    {
        /// <summary>
        /// Parafait utility 
        /// </summary>
        protected Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CustomerDTO customerDTO;
        List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
        MessageBoxDelegate messageBoxDelegate = null;

        public SelectRelatedCustomerUI(Utilities utilities, CustomerDTO customerDTO, MessageBoxDelegate messageBoxDelegate)
        {
            this.utilities = utilities;
            this.customerDTO = customerDTO;
            this.messageBoxDelegate = messageBoxDelegate;
            InitializeComponent();
            utilities.setupDataGridProperties(ref dgvCustomerRelationshipDTOList);
            utilities.setLanguage(this);
            LoadCustomerRelationshipDTOList();
            chbMainCustomer.Text = customerDTO.FirstName + " " + customerDTO.LastName;
            GetCustomerRelationshipTypeDTOList();
        }

        private void SelectRelatedCustomerUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            for (int i = 0; i < dgvCustomerRelationshipDTOList.Rows.Count; i++)
            {
                dgvCustomerRelationshipDTOList[customerSelected.Index, i].Value = 0;//check every row in the customerSelected column
                dgvCustomerRelationshipDTOList[customerSelected.Index, i].Tag = customerRelationshipDTOListBS[i];
            }
            this.dgvCustomerRelationshipDTOList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            log.LogMethodExit(null);
        }

        private void LoadCustomerRelationshipDTOList()
        {
            log.LogMethodEntry();
            List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchCustomerRelationshipParams = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.EFFECTIVE_DATE, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.EXPIRY_DATE, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            List<CustomerRelationshipDTO> customerRelationshipDTOList = null;
            CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(utilities.ExecutionContext);
            customerRelationshipDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchCustomerRelationshipParams);
            if (customerRelationshipDTOList != null)
            {
                customerRelationshipDTOListBS.DataSource = customerRelationshipDTOList;
            }
            log.LogMethodExit(null);
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

            customerRelationshipTypeIdDataGridViewComboBoxColumn.DataSource = customerRelationshipTypeDTOList;
            customerRelationshipTypeIdDataGridViewComboBoxColumn.DisplayMember = "Description";
            customerRelationshipTypeIdDataGridViewComboBoxColumn.ValueMember = "Id";

            log.LogMethodExit(customerRelationshipTypeDTOList);
            return customerRelationshipTypeDTOList;
        }

        private void DgvCustomerRelationshipDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == viewCustomerDetailDataGridViewButtonColumn.Index)
            {
                if (customerRelationshipDTOListBS.Current != null && customerRelationshipDTOListBS.Current is CustomerRelationshipDTO)
                {
                    int relatedCustomerId = -1;
                    CustomerRelationshipDTO customerRelationshipDTO = customerRelationshipDTOListBS.Current as CustomerRelationshipDTO;
                    if (customerRelationshipDTO.CustomerId != customerDTO.Id)
                    {
                        relatedCustomerId = customerRelationshipDTO.CustomerId;
                    }
                    else if (customerRelationshipDTO.RelatedCustomerId != customerDTO.Id)
                    {
                        relatedCustomerId = customerRelationshipDTO.RelatedCustomerId;
                    }
                    if (relatedCustomerId != -1)
                    {
                        CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, relatedCustomerId);
                        CustomerDetailForm customerDetailForm = new CustomerDetailForm(utilities, customerBL.CustomerDTO, messageBoxDelegate, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
                        customerDetailForm.SetControlsEnabled(false);
                        customerDetailForm.ShowDialog();
                    }
                }
            }
            else if (e.ColumnIndex == customerSelected.Index)
            {
                if (Convert.ToInt32(dgvCustomerRelationshipDTOList.CurrentRow.Cells[customerSelected.Index].Value) == 0)
                    dgvCustomerRelationshipDTOList.CurrentRow.Cells[customerSelected.Index].Value = 1;
                else
                    dgvCustomerRelationshipDTOList.CurrentRow.Cells[customerSelected.Index].Value = 0;

                CheckIfAllTheCustomersAreSelected();
            }
            log.LogMethodExit();
        }

        private void dgvCustomerRelationshipDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, dgvCustomerRelationshipDTOList.Columns[e.ColumnIndex].HeaderText), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Relationship"));
            e.Cancel = true;
            log.LogMethodExit(null);
        }

        private void BtnConfirmSelection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvCustomerRelationshipDTOList.Rows.Count; i++)
            {
                if (Convert.ToInt32(dgvCustomerRelationshipDTOList[customerSelected.Index, i].Value) == 1)
                {
                    int relatedCustomerId = (dgvCustomerRelationshipDTOList[customerSelected.Index, i].Tag as CustomerRelationshipDTO).RelatedCustomerId;
                    if (relatedCustomerId == customerDTO.Id)
                        relatedCustomerId = (dgvCustomerRelationshipDTOList[customerSelected.Index, i].Tag as CustomerRelationshipDTO).CustomerId;
                    CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, relatedCustomerId);
                    customerDTOList.Add(customerBL.CustomerDTO);
                }
            }
            if (chbMainCustomer.Checked)
            {
                customerDTOList.Insert(0, customerDTO);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ChbSelectAll_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int chbSelectAllChecked = 0;
            if (chbSelectAll.Checked)
                chbSelectAllChecked = 1;
            for (int i = 0; i < dgvCustomerRelationshipDTOList.Rows.Count; i++)
            {
                dgvCustomerRelationshipDTOList[customerSelected.Index, i].Value = chbSelectAllChecked;//check every row in the customerSelected column
            }
            chbMainCustomer.Checked = chbSelectAll.Checked;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Returns the Customer list.
        /// </summary>
        public List<CustomerDTO> CustomerDTOList
        {
            get
            {
                return customerDTOList;
            }
        }

        private void chbMainCustomer_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CheckIfAllTheCustomersAreSelected();
            log.LogMethodExit();
        }

        private void CheckIfAllTheCustomersAreSelected()
        {
            log.LogMethodEntry();
            int count = 0;
            for (int i = 0; i < dgvCustomerRelationshipDTOList.Rows.Count; i++)
            {
                if (Convert.ToInt32(dgvCustomerRelationshipDTOList[customerSelected.Index, i].Value) == 1)
                {
                    count++;
                }
            }
            if (count == dgvCustomerRelationshipDTOList.Rows.Count && chbMainCustomer.Checked)
                chbSelectAll.Checked = true;
            else
                chbSelectAll.Checked = false;
            log.LogMethodExit();
        }

        private void btnCustomerDetail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerDetailForm customerDetailForm = new CustomerDetailForm(utilities, customerDTO, messageBoxDelegate, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            customerDetailForm.SetControlsEnabled(false);
            customerDetailForm.ShowDialog();
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - Customer Feedback Response Value UI
 * Description  - User interface for customer feedback response value UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        27-Dec-2016   Raghuveera          Created 
 ********************************************************************************************/


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
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer
{ 
    /// <summary>
    /// User interface of Customer Feedback Survey Response Values UI
    /// </summary>
    public partial class CustomerFeedbackResponseValuesUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList;
        BindingSource customerFeedbackResponseValuesListBS;
        /// <summary>
        /// Constructor used during object creation.
        /// </summary>
        /// <param name="_Utilities">utilities to pass the environment setup.</param>
        public CustomerFeedbackResponseValuesUI(Utilities _Utilities)
        {           
            log.Debug("Starts-CustomerFeedbackResponseUI(Utilities) parameterized constructor.");
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvSurveyDetails);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            LoadResponse();
            PopulateResponseValueGrid();
            log.Debug("Ends-CustomerFeedbackResponseUI(Utilities) parameterized constructor.");
        }
        
        private void PopulateResponseValueGrid()
        {
            log.Debug("Starts-PopulateResponseGrid() method.");
            CustomerFeedbackResponseValuesList customerFeedbackResponseValuesList = new CustomerFeedbackResponseValuesList(machineUserContext);
            List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchByCustomerFeedbackResponseValuesParameters = new List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE, "1"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.RESPONSE_VALUE, txtName.Text));
            }
            searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesListOnDisplay = customerFeedbackResponseValuesList.GetAllCustomerFeedbackResponseValues(searchByCustomerFeedbackResponseValuesParameters,-1);
            customerFeedbackResponseValuesListBS = new BindingSource();
            if (customerFeedbackResponseValuesListOnDisplay != null && customerFeedbackResponseValuesListOnDisplay.Any())
            {
                SortableBindingList<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOSortList = new SortableBindingList<CustomerFeedbackResponseValuesDTO>(customerFeedbackResponseValuesListOnDisplay);
                customerFeedbackResponseValuesListBS.DataSource = customerFeedbackResponseValuesDTOSortList;
            }
            else
                customerFeedbackResponseValuesListBS.DataSource = new SortableBindingList<CustomerFeedbackResponseValuesDTO>();
            customerFeedbackResponseValuesListBS.AddingNew += dgvSurveyDetails_BindingSourceAddNew;
            dgvSurveyDetails.DataSource = customerFeedbackResponseValuesListBS;
            dgvSurveyDetails.DataError += new DataGridViewDataErrorEventHandler(dgvSurveyDetails_ComboDataError);

            log.Debug("Ends-PopulateResponseGrid() method.");
        }

        private void dgvSurveyDetails_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvSurveyDetails_BindingSourceAddNew() event.");
            if (dgvSurveyDetails.Rows.Count == customerFeedbackResponseValuesListBS.Count)
            {
                customerFeedbackResponseValuesListBS.RemoveAt(customerFeedbackResponseValuesListBS.Count - 1);
            }
            log.Debug("Ends-dgvSurveyDetails_BindingSourceAddNew() event.");
        }

        private void dgvSurveyDetails_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
            log.Debug("Starts-dgvSurveyDetails_ComboDataError() event.");
            if (e.ColumnIndex == dgvSurveyDetails.Columns["custFbResponseIdDataGridViewTextBoxColumn"].Index)
            {
                if (customerFeedbackResponseDTOList != null)
                    dgvSurveyDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = customerFeedbackResponseDTOList[0].CustFbResponseId;
            }            
            log.Debug("Ends-dgvSurveyDetails_ComboDataError() event.");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() event.");
            CustomerFeedbackResponseValues customerFeedbackResponseValues;
            BindingSource customerFeedbackResponseValuesListBS = (BindingSource)dgvSurveyDetails.DataSource;
            var customerFeedbackResponseValuesListOnDisplay = (SortableBindingList<CustomerFeedbackResponseValuesDTO>)customerFeedbackResponseValuesListBS.DataSource;
            if (customerFeedbackResponseValuesListOnDisplay.Count > 0)
            {
                foreach (CustomerFeedbackResponseValuesDTO customerFeedbackResponseValuesDTO in customerFeedbackResponseValuesListOnDisplay)
                {
                    if (string.IsNullOrEmpty(customerFeedbackResponseValuesDTO.ResponseValue))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(962));
                        return;
                    }

                    if (customerFeedbackResponseValuesDTO.CustFbResponseId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Invalid Response."));
                        return;
                    }
                    customerFeedbackResponseValues = new CustomerFeedbackResponseValues(machineUserContext,customerFeedbackResponseValuesDTO);
                    customerFeedbackResponseValues.Save();
                }
                btnSearch.PerformClick();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.Debug("Ends-btnSave_Click() event.");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRefresh_Click() event.");
            txtName.Text = "";
            chbShowActiveEntries.Checked = true;
            LoadResponse();
            PopulateResponseValueGrid();
            log.Debug("Ends-btnRefresh_Click() event.");
        }

        private void LoadResponse()
        {
            CustomerFeedbackResponseList customerFeedbackResponseList = new CustomerFeedbackResponseList(machineUserContext);
            customerFeedbackResponseDTOList = new List<CustomerFeedbackResponseDTO>();
            List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
            searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            customerFeedbackResponseDTOList = customerFeedbackResponseList.GetAllCustomerFeedbackResponse(searchByCustomerFeedbackResponseParameters);
            if (customerFeedbackResponseDTOList == null)
            {
                customerFeedbackResponseDTOList = new List<CustomerFeedbackResponseDTO>();
            }
            customerFeedbackResponseDTOList.Insert(0, new CustomerFeedbackResponseDTO());
            customerFeedbackResponseDTOList[0].ResponseName = "<SELECT>";
            custFbResponseIdDataGridViewTextBoxColumn.DataSource = customerFeedbackResponseDTOList;
            custFbResponseIdDataGridViewTextBoxColumn.DisplayMember = "ResponseName";
            custFbResponseIdDataGridViewTextBoxColumn.ValueMember = "CustFbResponseId";
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDelete_Click() event.");
            if (this.dgvSurveyDetails.SelectedRows.Count <= 0 && this.dgvSurveyDetails.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-btnDelete_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.dgvSurveyDetails.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.dgvSurveyDetails.SelectedCells)
                {
                    dgvSurveyDetails.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow customerFeedbackResponseValueRow in this.dgvSurveyDetails.SelectedRows)
            {
                if (customerFeedbackResponseValueRow.Cells[0].Value == null)
                {
                    return;
                }
                if (Convert.ToInt32(customerFeedbackResponseValueRow.Cells[0].Value.ToString()) <= 0)
                {
                    dgvSurveyDetails.Rows.RemoveAt(customerFeedbackResponseValueRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource customerFeedbackResponseValuesDTOListDTOBS = (BindingSource)dgvSurveyDetails.DataSource;
                        var customerFeedbackResponseValuesDTOList = (SortableBindingList<CustomerFeedbackResponseValuesDTO>)customerFeedbackResponseValuesDTOListDTOBS.DataSource;
                        CustomerFeedbackResponseValuesDTO customerFeedbackResponseValuesDTO = customerFeedbackResponseValuesDTOList[customerFeedbackResponseValueRow.Index];
                        customerFeedbackResponseValuesDTO.IsActive = false;
                        CustomerFeedbackResponseValues customerFeedbackResponseValues = new CustomerFeedbackResponseValues(machineUserContext,customerFeedbackResponseValuesDTO);
                        customerFeedbackResponseValues.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            btnSearch.PerformClick();
            log.Debug("Ends-btnDelete_Click() event.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click() event.");
            this.Close();
            log.Debug("Ends-btnClose_Click() event.");
        }

        private void dgvSurveyDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts -dgvSurveyDetails_CellContentClick() event .");
            if (dgvSurveyDetails.Columns[e.ColumnIndex].Name.Equals("Browse"))
            {
                OpenFileDialog fDialog = new OpenFileDialog();
                fDialog.Title = "Select Image to upload";
                fDialog.Filter = "(*.bmp, *.jpg)|*.bmp;*.jpg";
                fDialog.Multiselect = false;
                if (fDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // txtFile.Text = fDialog.FileName.ToString();
                    dgvSurveyDetails.Rows[e.RowIndex].Cells["imageDataGridViewImageColumn"].Value =  Image.FromFile(fDialog.FileName);
                }
                else
                {
                    if(MessageBox.Show("Do you want to remove the existing image?","Image Load",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    {
                        dgvSurveyDetails.Rows[e.RowIndex].Cells["imageDataGridViewImageColumn"].Value = null;
                    }
                }                
            }
            else if (dgvSurveyDetails.Columns[e.ColumnIndex].Name.Equals("Translation"))
            {

                BindingSource customerFeedbackResponseValuesListBS = (BindingSource)dgvSurveyDetails.DataSource;
                var customerFeedbackResponseValuesListOnDisplay = (SortableBindingList<CustomerFeedbackResponseValuesDTO>)customerFeedbackResponseValuesListBS.DataSource;
                if (customerFeedbackResponseValuesListOnDisplay != null && customerFeedbackResponseValuesListOnDisplay.Count >= e.RowIndex + 1 && customerFeedbackResponseValuesListOnDisplay[e.RowIndex].CustFbResponseValueId > -1)
                {
                    ObjectTranslationsUI objectTranslationsUI = new ObjectTranslationsUI(utilities, (dgvSurveyDetails.Rows[e.RowIndex].Cells["responseValueDataGridViewTextBoxColumn"].Value == null) ? "" : dgvSurveyDetails.Rows[e.RowIndex].Cells["responseValueDataGridViewTextBoxColumn"].Value.ToString(), "RESPONSEVALUE", "RESPONSEVALUETABLE", customerFeedbackResponseValuesListOnDisplay[e.RowIndex].Guid);
                    objectTranslationsUI.ShowDialog();
                    
                }
            }

            log.Debug("Ends -dgvSurveyDetails_CellContentClick() event .");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSearch_Click() event.");
            PopulateResponseValueGrid();
            log.Debug("Ends-btnSearch_Click() event.");
        }
    }
}

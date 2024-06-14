/********************************************************************************************
 * Project Name - Customer Feedback Response UI
 * Description  - User interface for customer feedback response UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        27-Dec-2016   Raghuveera          Created 
 *2.70.2        10-Aug-2019   Girish kundar       Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// User interface for customer feedback response UI
    /// </summary>
    public partial class CustomerFeedbackResponseUI : Form
    {
       private Utilities utilities;
       private static readonly  Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
       private List<LookupValuesDTO> lookupValuesDTOList;
        /// <summary>
        /// Constructor used during object creation.
        /// </summary>
        public CustomerFeedbackResponseUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
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
            LoadResponseType();
            PopulateResponseGrid();
            log.LogMethodExit();
        }
        private void PopulateResponseGrid()
        {
            log.LogMethodEntry();
            CustomerFeedbackResponseList customerFeedbackResponseList = new CustomerFeedbackResponseList(machineUserContext);
            List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, "1"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.RESPONSE_NAME, txtName.Text));
            }
            searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<CustomerFeedbackResponseDTO> customerFeedbackResponseListOnDisplay = customerFeedbackResponseList.GetAllCustomerFeedbackResponse(searchByCustomerFeedbackResponseParameters);
            BindingSource customerFeedbackResponseListBS = new BindingSource();
            if (customerFeedbackResponseListOnDisplay != null)
            {
                SortableBindingList<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOSortList = new SortableBindingList<CustomerFeedbackResponseDTO>(customerFeedbackResponseListOnDisplay);
                customerFeedbackResponseListBS.DataSource = customerFeedbackResponseDTOSortList;
            }
            else
                customerFeedbackResponseListBS.DataSource = new SortableBindingList<CustomerFeedbackResponseDTO>();
            dgvSurveyDetails.DataSource = customerFeedbackResponseListBS;
            log.LogMethodExit();
        }

        private void LoadResponseType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                lookupValuesDTOList = new List<LookupValuesDTO>();
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();

                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_RESPONSE_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValue = "<SELECT>";
                //BindingSource bindingSource = new BindingSource();
                //bindingSource.DataSource = lookupValuesDTOList;
                //cmbCriteria.DataSource = bindingSource;
                //cmbCriteria.DisplayMember = "LookupValue";
                //cmbCriteria.ValueMember = "LookupValueId";

                responseTypeIdDataGridViewTextBoxColumn.DataSource = lookupValuesDTOList;
                responseTypeIdDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                responseTypeIdDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadResponseType() Method with an Exception:", e);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateResponseGrid();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            BindingSource customerFeedbackResponseListBS = (BindingSource)dgvSurveyDetails.DataSource;
            var customerFeedbackResponseListOnDisplay = (SortableBindingList<CustomerFeedbackResponseDTO>)customerFeedbackResponseListBS.DataSource;
            if (customerFeedbackResponseListOnDisplay.Count > 0)
            {
                foreach (CustomerFeedbackResponseDTO customerFeedbackResponseDTO in customerFeedbackResponseListOnDisplay)
                {
                    if (string.IsNullOrEmpty(customerFeedbackResponseDTO.ResponseName))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(962));
                        return;
                    }

                    if (customerFeedbackResponseDTO.ResponseTypeId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Invalid Response Type."));
                        return;
                    }
                    CustomerFeedbackResponse customerFeedbackResponse = new CustomerFeedbackResponse(machineUserContext,customerFeedbackResponseDTO);
                    customerFeedbackResponse.Save();
                }
                btnSearch.PerformClick();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtName.Text = "";
            chbShowActiveEntries.Checked = true;
            LoadResponseType();
            PopulateResponseGrid();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
            foreach (DataGridViewRow customerFeedbackResponseRow in this.dgvSurveyDetails.SelectedRows)
            {
                if (customerFeedbackResponseRow.Cells[0].Value == null)
                {
                    return;
                }

                if (Convert.ToInt32(customerFeedbackResponseRow.Cells[0].Value.ToString()) <= 0)
                {
                    dgvSurveyDetails.Rows.RemoveAt(customerFeedbackResponseRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource customerFeedbackResponseDTOListDTOBS = (BindingSource)dgvSurveyDetails.DataSource;
                        var customerFeedbackResponseDTOList = (SortableBindingList<CustomerFeedbackResponseDTO>)customerFeedbackResponseDTOListDTOBS.DataSource;
                        CustomerFeedbackResponseDTO customerFeedbackResponseDTO = customerFeedbackResponseDTOList[customerFeedbackResponseRow.Index];
                        customerFeedbackResponseDTO.IsActive = false;
                        CustomerFeedbackResponse customerFeedbackResponse = new CustomerFeedbackResponse(machineUserContext,customerFeedbackResponseDTO);
                        customerFeedbackResponse.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            btnSearch.PerformClick();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

    }
}

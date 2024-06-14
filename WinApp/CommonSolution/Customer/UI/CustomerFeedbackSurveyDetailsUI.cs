/********************************************************************************************
 * Project Name - Customer Feedback Survey Details UI
 * Description  - User interface for customer feedback survey details UI
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
    /// Customer Feedback Survey Details User interface 
    /// </summary>
    public partial class CustomerFeedbackSurveyDetailsUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        BindingSource customerFeedbackSurveyDetailsListBS;
        List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList;
        List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList;
        List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList;
        List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList;
        List<LookupValuesDTO> lookupValuesDTOList;
        /// <summary>
        /// Constructor used during object creation.
        /// </summary>
        public CustomerFeedbackSurveyDetailsUI(Utilities _Utilities)
        {
            log.Debug("Starts-CustomerFeedbackSurveyUI(Utilities) parameterized constructor.");
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
            LoadSurvey();
            LoadCritiria();
            LoadQuestion();
            LoadResponse();
            //LoadResponseValue();
            PopulateSurveyGrid();
            log.Debug("Ends-CustomerFeedbackSurveyUI(Utilities) parameterized constructor.");
        }

        private void LoadResponse()
        {
            log.Debug("Starts-LoadResponse() method.");
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
            log.Debug("Ends-LoadResponse() method.");
        }
        private void LoadResponseValue(int responseId)
        {
            log.Debug("Starts-LoadResponseValue() method.");
            if (responseId != -1)
            {
                CustomerFeedbackResponseValuesList customerFeedbackResponseValuesList = new CustomerFeedbackResponseValuesList(machineUserContext);
                customerFeedbackResponseValuesDTOList = new List<CustomerFeedbackResponseValuesDTO>();
                List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchByCustomerFeedbackResponseValuesParameters = new List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>>();
                searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE, "1"));
                searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID, responseId.ToString()));
                customerFeedbackResponseValuesDTOList = customerFeedbackResponseValuesList.GetAllCustomerFeedbackResponseValues(searchByCustomerFeedbackResponseValuesParameters, utilities.ParafaitEnv.LanguageId);
                if (customerFeedbackResponseValuesDTOList == null)
                {
                    customerFeedbackResponseValuesDTOList = new List<CustomerFeedbackResponseValuesDTO>();
                }
                customerFeedbackResponseValuesDTOList.Insert(0, new CustomerFeedbackResponseValuesDTO());
                customerFeedbackResponseValuesDTOList[0].ResponseValue = "<SELECT>";
                cmbDataColumn.DataSource = customerFeedbackResponseValuesDTOList;
                cmbDataColumn.DisplayMember = "ResponseValue";
                cmbDataColumn.ValueMember = "CustFbResponseValueId";
            }
            else
            {
                cmbDataColumn.Visible = false;
            }
            log.Debug("Ends-LoadResponseValue() method.");
        }
        private void LoadSurvey()
        {
            log.Debug("Starts-LoadSurvey() method.");
            CustomerFeedbackSurveyList customerFeedbackSurveyList = new CustomerFeedbackSurveyList(machineUserContext);
            customerFeedbackSurveyDTOList = new List<CustomerFeedbackSurveyDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchBycustomerFeedbackSurveyParameters = new List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>>();
            searchBycustomerFeedbackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE, "1"));
            searchBycustomerFeedbackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            customerFeedbackSurveyDTOList = customerFeedbackSurveyList.GetAllCustomerFeedbackSurvey(searchBycustomerFeedbackSurveyParameters);
            if (customerFeedbackSurveyDTOList == null)
            {
                customerFeedbackSurveyDTOList = new List<CustomerFeedbackSurveyDTO>();
            }
            customerFeedbackSurveyDTOList.Insert(0, new CustomerFeedbackSurveyDTO());
            customerFeedbackSurveyDTOList[0].SurveyName = "<SELECT>";
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = customerFeedbackSurveyDTOList;
            cmbSurvey.DataSource = customerFeedbackSurveyDTOList;
            cmbSurvey.DisplayMember = "SurveyName";
            cmbSurvey.ValueMember = "CustFbSurveyId";
            custFbSurveyIdDataGridViewTextBoxColumn.DataSource = customerFeedbackSurveyDTOList;
            custFbSurveyIdDataGridViewTextBoxColumn.DisplayMember = "SurveyName";
            custFbSurveyIdDataGridViewTextBoxColumn.ValueMember = "CustFbSurveyId";
            log.Debug("Ends-LoadSurvey() method.");
        }

        private void LoadQuestion()
        {
            log.Debug("Starts-LoadQuestion() method.");
            CustomerFeedbackQuestionsList customerFeedbackQuestionsList = new CustomerFeedbackQuestionsList(machineUserContext);
            customerFeedbackQuestionsDTOList = new List<CustomerFeedbackQuestionsDTO>();
            List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchByCustomerFeedbackQuestionsParameters = new List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>>();
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));            
            customerFeedbackQuestionsDTOList = customerFeedbackQuestionsList.GetAllCustomerFeedbackQuestions(searchByCustomerFeedbackQuestionsParameters, utilities.ParafaitEnv.LanguageId);
            if (customerFeedbackQuestionsDTOList==null)
            {
                customerFeedbackQuestionsDTOList = new List<CustomerFeedbackQuestionsDTO>();
            }
            customerFeedbackQuestionsDTOList.Insert(0, new CustomerFeedbackQuestionsDTO());
            customerFeedbackQuestionsDTOList[0].Question = "<SELECT>";
          
            BindingSource nextQuestionBindingSource = new BindingSource();            
            nextQuestionBindingSource.DataSource = customerFeedbackQuestionsDTOList;

            nextQuestionIdDataGridViewTextBoxColumn.DataSource = nextQuestionBindingSource;
            nextQuestionIdDataGridViewTextBoxColumn.DisplayMember = "Question";
            nextQuestionIdDataGridViewTextBoxColumn.ValueMember = "CustFbQuestionId";

            custFbQuestionIdDataGridViewTextBoxColumn.DataSource = customerFeedbackQuestionsDTOList;
            custFbQuestionIdDataGridViewTextBoxColumn.DisplayMember = "Question";
            custFbQuestionIdDataGridViewTextBoxColumn.ValueMember = "CustFbQuestionId";

            log.Debug("Ends-LoadQuestion() method.");
        }
        private void LoadCritiria()
        {
            log.Debug("Starts-LoadCritiria() method.");
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                lookupValuesDTOList = new List<LookupValuesDTO>();
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
        
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_FEEDBACK_CRITERIA"));                
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList == null )
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValue = "<SELECT>";
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = lookupValuesDTOList;
                cmbCriteria.DataSource = bindingSource;
                cmbCriteria.DisplayMember = "LookupValue";
                cmbCriteria.ValueMember = "LookupValueId";

                criteriaIdDataGridViewTextBoxColumn.DataSource = bindingSource;
                criteriaIdDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                criteriaIdDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                log.Debug("Ends-LoadCritiria() Method");
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadCritiria() Method with an Exception:", e);
            }
        }
        private void PopulateSurveyGrid()
        {
            log.Debug("Starts-PopulateSurveyGrid() method.");
            //if (cmbSurvey.SelectedValue.ToString().Equals("-1"))
            //{
            //    customerFeedbackSurveyDetailsListBS = new BindingSource();
            //    customerFeedbackSurveyDetailsListBS.DataSource = new SortableBindingList<CustomerFeedbackSurveyDetailsDTO>();
            //    dgvSurveyDetails.DataSource = customerFeedbackSurveyDetailsListBS;
            //    return;
            //}
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsListOnDisplay;
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList();
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> customerFeedbackSurveyDetailsSearchParams = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                customerFeedbackSurveyDetailsSearchParams.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
            }
            if (!cmbSurvey.SelectedValue.ToString().Equals("-1"))
            {
                customerFeedbackSurveyDetailsSearchParams.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, cmbSurvey.SelectedValue.ToString()));
            }
            if (!cmbCriteria.SelectedValue.ToString().Equals("-1"))
            {
                customerFeedbackSurveyDetailsSearchParams.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID, cmbCriteria.SelectedValue.ToString()));
            }
            customerFeedbackSurveyDetailsSearchParams.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            customerFeedbackSurveyDetailsListOnDisplay = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(customerFeedbackSurveyDetailsSearchParams);
            customerFeedbackSurveyDetailsListBS = new BindingSource();
            if (customerFeedbackSurveyDetailsListOnDisplay != null)
            {
                SortableBindingList<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOSortList = new SortableBindingList<CustomerFeedbackSurveyDetailsDTO>(customerFeedbackSurveyDetailsListOnDisplay);
                customerFeedbackSurveyDetailsListBS.DataSource = customerFeedbackSurveyDetailsDTOSortList;
            }
            else
                customerFeedbackSurveyDetailsListBS.DataSource = new SortableBindingList<CustomerFeedbackSurveyDetailsDTO>();
            customerFeedbackSurveyDetailsListBS.AddingNew += dgvSurveyDetails_BindingSourceAddNew;
            dgvSurveyDetails.DataSource = customerFeedbackSurveyDetailsListBS;
            dgvSurveyDetails.DataError += new DataGridViewDataErrorEventHandler(dgvSurveyDetails_ComboDataError);
            log.Debug("Ends-PopulateSurveyGrid() method.");
        }

        private void dgvSurveyDetails_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvSurveyDetails_BindingSourceAddNew() event.");
            if (dgvSurveyDetails.Rows.Count == customerFeedbackSurveyDetailsListBS.Count)
            {
                customerFeedbackSurveyDetailsListBS.RemoveAt(customerFeedbackSurveyDetailsListBS.Count - 1);
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
            else if (e.ColumnIndex == dgvSurveyDetails.Columns["expectedCustFbResponseValueIdDataGridViewTextBoxColumn"].Index)
            {
                if (customerFeedbackResponseValuesDTOList != null)
                    dgvSurveyDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = customerFeedbackResponseValuesDTOList[0].CustFbResponseValueId;
            }
            else if (e.ColumnIndex == dgvSurveyDetails.Columns["criteriaIdDataGridViewTextBoxColumn"].Index)
            {
                if (lookupValuesDTOList != null)
                    dgvSurveyDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = lookupValuesDTOList[0].LookupValueId;
            }
            else if (e.ColumnIndex == dgvSurveyDetails.Columns["custFbQuestionIdDataGridViewTextBoxColumn"].Index)
            {
                if (customerFeedbackQuestionsDTOList != null)
                    dgvSurveyDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = customerFeedbackQuestionsDTOList[0].CustFbQuestionId;
            }
            else if (e.ColumnIndex == dgvSurveyDetails.Columns["nextQuestionIdDataGridViewTextBoxColumn"].Index)
            {
                if (customerFeedbackQuestionsDTOList != null)
                    dgvSurveyDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = customerFeedbackQuestionsDTOList[0].CustFbQuestionId;
            }
            else if (e.ColumnIndex == dgvSurveyDetails.Columns["custFbSurveyIdDataGridViewTextBoxColumn"].Index)
            {
                if (customerFeedbackSurveyDTOList != null)
                    dgvSurveyDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = customerFeedbackSurveyDTOList[0].CustFbSurveyId;
            }
            log.Debug("Ends-dgvSurveyDetails_ComboDataError() event.");
        }

        private void dgvSurveyDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSurveyDetails.Columns[e.ColumnIndex].Name.Equals("ExpectedCustFbResponseValue"))
            {
                if (Convert.ToInt32(dgvSurveyDetails.Rows[e.RowIndex].Cells["custFbResponseIdDataGridViewTextBoxColumn"].Value) > -1)
                {
                    int y = dgvSurveyDetails.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Top + dgvSurveyDetails.Top;
                    int x = dgvSurveyDetails.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Left;
                    cmbDataColumn.Location = new Point(x + 2, y); // segmentDefinitionSourceMapDataGridView.GetCellDisplayRectangle(e.ColumnIndex + 1, e.RowIndex, false).Top;
                    cmbDataColumn.Width = ExpectedCustFbResponseValue.Width;
                    cmbDataColumn.BringToFront();
                    cmbDataColumn.Visible = true;
                    int expectedValueId = Convert.ToInt32(dgvSurveyDetails.Rows[e.RowIndex].Cells["ExpectedCustFbResponseValueId"].Value);

                    dgvSurveyDetails.CurrentCell = dgvSurveyDetails.Rows[e.RowIndex].Cells["ExpectedCustFbResponseValue"];
                    LoadResponseValue(Convert.ToInt32(dgvSurveyDetails.Rows[e.RowIndex].Cells["custFbResponseIdDataGridViewTextBoxColumn"].Value));
                    cmbDataColumn.SelectedValue = expectedValueId;
                    dgvSurveyDetails.Rows[e.RowIndex].Cells["ExpectedCustFbResponseValue"].Value = cmbDataColumn.Text;
                }
                else
                {
                    cmbDataColumn.DataSource = null;
                    cmbDataColumn.Visible = false;
                }
            }
            else
            {
                cmbDataColumn.Visible = false;
            }
        }

        private void cmbDataColumn_SelectedValueChanged(object sender, EventArgs e)
        {
            if (dgvSurveyDetails.CurrentCell != null)
            {
                if (!string.IsNullOrEmpty(cmbDataColumn.Text) && !cmbDataColumn.Text.Equals("<SELECT>") && !cmbDataColumn.Text.Equals("System.Data.DataRowView") && !cmbDataColumn.Text.Equals("Semnox.Parafait.CustomerFeedBackSurvey.CustomerFeedbackResponseValuesDTO") && !cmbDataColumn.SelectedValue.ToString().Equals("Semnox.Parafait.CustomerFeedBackSurvey.CustomerFeedbackResponseValuesDTO"))
                {
                    dgvSurveyDetails.Rows[dgvSurveyDetails.CurrentCell.RowIndex].Cells["ExpectedCustFbResponseValueId"].Value = Convert.ToInt32(cmbDataColumn.SelectedValue);
                    dgvSurveyDetails.Rows[dgvSurveyDetails.CurrentCell.RowIndex].Cells["ExpectedCustFbResponseValue"].Value = cmbDataColumn.Text;
                }
                else if (cmbDataColumn.Text.Equals("<SELECT>") && cmbDataColumn.SelectedValue.ToString().Equals("-1"))
                {
                    dgvSurveyDetails.Rows[dgvSurveyDetails.CurrentCell.RowIndex].Cells["ExpectedCustFbResponseValueId"].Value = -1;
                    dgvSurveyDetails.Rows[dgvSurveyDetails.CurrentCell.RowIndex].Cells["ExpectedCustFbResponseValue"].Value = null;
                }
            }
        }       

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() event.");
            BindingSource customerFeedbackSurveyDetailsListBS = (BindingSource)dgvSurveyDetails.DataSource;
            var customerFeedbackSurveyDetailsListOnDisplay = (SortableBindingList<CustomerFeedbackSurveyDetailsDTO>)customerFeedbackSurveyDetailsListBS.DataSource;
            if (customerFeedbackSurveyDetailsListOnDisplay.Count > 0)
            {
                foreach (CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO in customerFeedbackSurveyDetailsListOnDisplay)
                {
                    if (customerFeedbackSurveyDetailsDTO.CustFbQuestionId==-1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Invalid question."));
                        return;
                    }
                    if (customerFeedbackSurveyDetailsDTO.CustFbSurveyId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Invalid Survey."));
                        return;
                    }
                    if (customerFeedbackSurveyDetailsDTO.CriteriaId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Invalid Criteria."));
                        return;
                    }                   
                    CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(machineUserContext,customerFeedbackSurveyDetailsDTO);
                    customerFeedbackSurveyDetails.Save(null);
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
            chbShowActiveEntries.Checked = true;
            LoadSurvey();
            LoadCritiria();
            LoadQuestion();
            LoadResponse();
            PopulateSurveyGrid();
            log.Debug("Ends-btnRefresh_Click() event.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSearch_Click() event.");
            PopulateSurveyGrid();
            log.Debug("Ends-btnSearch_Click() event.");
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
            foreach (DataGridViewRow customerFeedbackSurveyRow in this.dgvSurveyDetails.SelectedRows)
            {
                if (customerFeedbackSurveyRow.Cells[0].Value == null)
                {
                    return;
                }

                if (Convert.ToInt32(customerFeedbackSurveyRow.Cells[0].Value.ToString()) <= 0)
                {
                    dgvSurveyDetails.Rows.RemoveAt(customerFeedbackSurveyRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource customerFeedbackSurveyDetailsDTOListDTOBS = (BindingSource)dgvSurveyDetails.DataSource;
                        var customerFeedbackSurveyDetailsDTOList = (SortableBindingList<CustomerFeedbackSurveyDetailsDTO>)customerFeedbackSurveyDetailsDTOListDTOBS.DataSource;
                        CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO = customerFeedbackSurveyDetailsDTOList[customerFeedbackSurveyRow.Index];
                        customerFeedbackSurveyDetailsDTO.IsActive = false;
                        CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(machineUserContext,customerFeedbackSurveyDetailsDTO);
                        customerFeedbackSurveyDetails.Save(null);
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
    }
}

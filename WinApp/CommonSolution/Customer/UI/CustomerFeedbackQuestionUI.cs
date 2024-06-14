
/********************************************************************************************
 * Project Name - Customer Feedback Question UI
 * Description  - User interface for Customer Feedback Question UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        27-Dec-2016   Raghuveera          Created 
 *2.70.2        10-Aug-2019   Girish kundar       Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// User interface of Customer Feedback Survey Response Values UI
    /// </summary>
    public partial class CustomerFeedbackQuestionUI : Form
    {
       private Utilities utilities;
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
       private List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList;
       private BindingSource customerFeedbackQuestionsListBS;
        /// <summary>
        /// Constructor used during object creation.
        /// </summary>
        public CustomerFeedbackQuestionUI(Utilities _Utilities)
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
            LoadResponse();
            PopulateQuestionGrid();
            log.LogMethodExit();
        }

        private void LoadResponse()
        {
            log.LogMethodEntry();
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
            log.LogMethodExit();
        }

        private void PopulateQuestionGrid()
        {
            log.LogMethodEntry();
            CustomerFeedbackQuestionsList customerFeedbackQuestionsList = new CustomerFeedbackQuestionsList(machineUserContext);
            List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchByCustomerFeedbackQuestionsParameters = new List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE, "1"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.QUESTION_NO, txtName.Text));
            }
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsListOnDisplay = customerFeedbackQuestionsList.GetAllCustomerFeedbackQuestions(searchByCustomerFeedbackQuestionsParameters, -1);
            customerFeedbackQuestionsListBS = new BindingSource();
            if (customerFeedbackQuestionsListOnDisplay != null)
            {
                SortableBindingList<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOSortList = new SortableBindingList<CustomerFeedbackQuestionsDTO>(customerFeedbackQuestionsListOnDisplay);
                customerFeedbackQuestionsListBS.DataSource = customerFeedbackQuestionsDTOSortList;
            }
            else
                customerFeedbackQuestionsListBS.DataSource = new SortableBindingList<CustomerFeedbackQuestionsDTO>();
            customerFeedbackQuestionsListBS.AddingNew += dgvSurveyDetails_BindingSourceAddNew;
            dgvSurveyDetails.DataSource = customerFeedbackQuestionsListBS;
            dgvSurveyDetails.DataError += new DataGridViewDataErrorEventHandler(dgvSurveyDetails_ComboDataError);

            log.LogMethodExit();
        }
        private void dgvSurveyDetails_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSurveyDetails.Rows.Count == customerFeedbackQuestionsListBS.Count)
            {
                customerFeedbackQuestionsListBS.RemoveAt(customerFeedbackQuestionsListBS.Count - 1);
            }
            log.LogMethodExit();
        }

        private void dgvSurveyDetails_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {

            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == dgvSurveyDetails.Columns["custFbResponseIdDataGridViewTextBoxColumn"].Index)
            {
                if (customerFeedbackResponseDTOList != null)
                    dgvSurveyDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = customerFeedbackResponseDTOList[0].CustFbResponseId;
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerFeedbackQuestions customerFeedbackQuestions;
            BindingSource customerFeedbackQuestionsListBS = (BindingSource)dgvSurveyDetails.DataSource;
            var customerFeedbackQuestionsListOnDisplay = (SortableBindingList<CustomerFeedbackQuestionsDTO>)customerFeedbackQuestionsListBS.DataSource;
            if (customerFeedbackQuestionsListOnDisplay.Count > 0)
            {
                foreach (CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO in customerFeedbackQuestionsListOnDisplay)
                {
                    if (string.IsNullOrEmpty(customerFeedbackQuestionsDTO.QuestionNo))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(962));
                        return;
                    }
                    if (string.IsNullOrEmpty(customerFeedbackQuestionsDTO.Question))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(962));
                        return;
                    }

                    //if (customerFeedbackQuestionsDTO.CustFbResponseId == -1)
                    //{
                    //    MessageBox.Show(utilities.MessageUtils.getMessage("Invalid Response."));
                    //    return;
                    //}
                    customerFeedbackQuestions = new CustomerFeedbackQuestions(machineUserContext,customerFeedbackQuestionsDTO);
                    customerFeedbackQuestions.Save();
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
            LoadResponse();
            PopulateQuestionGrid();
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
            foreach (DataGridViewRow customerFeedbackQuestionRow in this.dgvSurveyDetails.SelectedRows)
            {
                if (customerFeedbackQuestionRow.Cells[0].Value == null)
                {
                    return;
                }

                if (Convert.ToInt32(customerFeedbackQuestionRow.Cells[0].Value.ToString()) <= 0)
                {
                    dgvSurveyDetails.Rows.RemoveAt(customerFeedbackQuestionRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource customerFeedbackQuestionsDTOListDTOBS = (BindingSource)dgvSurveyDetails.DataSource;
                        var customerFeedbackQuestionsDTOList = (SortableBindingList<CustomerFeedbackQuestionsDTO>)customerFeedbackQuestionsDTOListDTOBS.DataSource;
                        CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO = customerFeedbackQuestionsDTOList[customerFeedbackQuestionRow.Index];
                        customerFeedbackQuestionsDTO.IsActive = false;
                        CustomerFeedbackQuestions customerFeedbackQuestions = new CustomerFeedbackQuestions(machineUserContext,customerFeedbackQuestionsDTO);
                        customerFeedbackQuestions.Save();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateQuestionGrid();
            log.LogMethodExit();
        }

        private void dgvSurveyDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSurveyDetails.Columns[e.ColumnIndex].Name.Equals("Translation"))
            {
                BindingSource customerFeedbackQuestionsListBS = (BindingSource)dgvSurveyDetails.DataSource;
                var customerFeedbackQuestionsListOnDisplay = (SortableBindingList<CustomerFeedbackQuestionsDTO>)customerFeedbackQuestionsListBS.DataSource;
                if (customerFeedbackQuestionsListOnDisplay != null && customerFeedbackQuestionsListOnDisplay.Count >= e.RowIndex + 1 && customerFeedbackQuestionsListOnDisplay[e.RowIndex].CustFbQuestionId > -1)
                {
                    ObjectTranslationsUI objectTranslationsUI = new ObjectTranslationsUI(utilities, (dgvSurveyDetails.Rows[e.RowIndex].Cells["questionDataGridViewTextBoxColumn"].Value == null) ? "" : dgvSurveyDetails.Rows[e.RowIndex].Cells["questionDataGridViewTextBoxColumn"].Value.ToString(), "QUESTION", "QUESTIONTABLE", customerFeedbackQuestionsListOnDisplay[e.RowIndex].Guid);
                    objectTranslationsUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
    }
}

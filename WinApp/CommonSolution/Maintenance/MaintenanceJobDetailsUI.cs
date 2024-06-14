/********************************************************************************************
/********************************************************************************************
 * Project Name - Maintenance Job Details UI
 * Description  - User interface for Maintenance Job Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera     Created 
 *********************************************************************************************
 *1.00        22-Feb-2016   Suneetha.S     Modified 
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes merge from WMS  
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
using Semnox.Parafait.User;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Maintenance Job Details UI
    /// </summary>
    public partial class MaintenanceJobDetailsUI : Form
    {
        int statusId;
        int jobTypeId;
        AssetList assetList;
        List<GenericAssetDTO> genericAssetListOnDisplay;
        JobTaskList jobTaskList;
        List<JobTaskDTO> jobTaskDTOListOnDisplay;
        List<UsersDTO> usersDTOList;
        BindingSource userJobTaskItemsBS;
        BindingSource assignedToBS;
        Utilities utilities;
        List<LookupValuesDTO> lookupValuesDTOList;
        //DataTable dtLookupStatus;
        bool loadSearch = true;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        public MaintenanceJobDetailsUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            RegisterKeyDownHandlers(this);
            utilities = _Utilities;
            utilities.setLanguage(this);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            //if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite) 
            //{
            //    btnPublishToSite.Visible = true;
            //}
            //else
            //{
                btnPublishToSite.Visible = false;
            //}
            utilities.setupDataGridProperties(ref adhocDataGridView);
            setupGrid();
            LoadAssignedTo();
            statusId = -1;
            LoadStatus();
            LoadAsset();
            LoadTask();
            jobTypeId = GetJobTypeID();
            PopulateAdhocGrid();
            log.LogMethodExit();
        }
        /// <summary>
        /// To get the Job type id
        /// </summary>
        private int GetJobTypeID()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList=new LookupValuesList(machineUserContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
            lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Job"));
            lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);
            if (lookupOpenValuesDTOList == null || lookupOpenValuesDTOList.Count==0)
            {
                log.LogMethodExit("lookupOpenValuesDTOList == null || lookupOpenValuesDTOList.Count==0");
                return -1;
            }
            log.LogMethodExit(lookupOpenValuesDTOList[0].LookupValueId);
            return lookupOpenValuesDTOList[0].LookupValueId;
            
        }
        private void adhocDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (adhocDataGridView.Rows.Count == userJobTaskItemsBS.Count)
            {
                userJobTaskItemsBS.RemoveAt(userJobTaskItemsBS.Count - 1);
            }
            log.LogMethodExit();
        }
        private void adhocDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == adhocDataGridView.Columns["statusDataGridViewTextBoxColumn"].Index)
            {
                if (lookupValuesDTOList != null)
                    adhocDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = lookupValuesDTOList[0].LookupValueId;
            }
            else if (e.ColumnIndex == adhocDataGridView.Columns["assignedUserIdDataGridViewTextBoxColumn"].Index)
            {
                if (assignedToBS != null)
                    adhocDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = -1;
            }
            log.LogMethodExit();
        }
        private void adhocSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                UserJobItemsBL userJobItemsBL = new UserJobItemsBL(machineUserContext);
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "close"));
                List<LookupValuesDTO> lookupValuesDTO = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

                lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "Open"));
                List<LookupValuesDTO> lookupOpenValuesDTO = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

                for (int i = 0; i < adhocDataGridView.Rows.Count - 1; i++)
                {
                    if (!adhocDataGridView.Rows[i].Cells["taskNameDataGridViewTextBoxColumn"].Value.Equals(string.IsNullOrEmpty(adhocDataGridView.Rows[i].Cells["JobTaskId"].FormattedValue.ToString()) ? adhocDataGridView.Rows[i].Cells["taskNameDataGridViewTextBoxColumn"].Value : adhocDataGridView.Rows[i].Cells["JobTaskId"].FormattedValue.ToString()))
                    {
                        adhocDataGridView.Rows[i].Cells["taskNameDataGridViewTextBoxColumn"].Value = string.IsNullOrEmpty(adhocDataGridView.Rows[i].Cells["JobTaskId"].FormattedValue.ToString()) ? adhocDataGridView.Rows[i].Cells["taskNameDataGridViewTextBoxColumn"].Value : adhocDataGridView.Rows[i].Cells["JobTaskId"].FormattedValue.ToString();
                    }
                    if (!adhocDataGridView.Rows[i].Cells["assetNameDataGridViewTextBoxColumn"].Value.Equals(string.IsNullOrEmpty(adhocDataGridView.Rows[i].Cells["assetIdDataGridViewTextBoxColumn"].FormattedValue.ToString()) ? adhocDataGridView.Rows[i].Cells["assetNameDataGridViewTextBoxColumn"].Value : adhocDataGridView.Rows[i].Cells["assetIdDataGridViewTextBoxColumn"].FormattedValue.ToString()))
                    {
                        adhocDataGridView.Rows[i].Cells["assetNameDataGridViewTextBoxColumn"].Value = string.IsNullOrEmpty(adhocDataGridView.Rows[i].Cells["assetIdDataGridViewTextBoxColumn"].FormattedValue.ToString()) ? adhocDataGridView.Rows[i].Cells["assetNameDataGridViewTextBoxColumn"].Value : adhocDataGridView.Rows[i].Cells["assetIdDataGridViewTextBoxColumn"].FormattedValue.ToString();
                    }
                    if (!adhocDataGridView.Rows[i].Cells["assignedToDataGridViewTextBoxColumn"].Value.Equals(string.IsNullOrEmpty(adhocDataGridView.Rows[i].Cells["assignedUserIdDataGridViewTextBoxColumn"].FormattedValue.ToString()) ? adhocDataGridView.Rows[i].Cells["assignedToDataGridViewTextBoxColumn"].Value : adhocDataGridView.Rows[i].Cells["assignedUserIdDataGridViewTextBoxColumn"].FormattedValue.ToString()))
                    {
                        adhocDataGridView.Rows[i].Cells["assignedToDataGridViewTextBoxColumn"].Value = string.IsNullOrEmpty(adhocDataGridView.Rows[i].Cells["assignedUserIdDataGridViewTextBoxColumn"].FormattedValue.ToString()) ? adhocDataGridView.Rows[i].Cells["assignedToDataGridViewTextBoxColumn"].Value : adhocDataGridView.Rows[i].Cells["assignedUserIdDataGridViewTextBoxColumn"].FormattedValue.ToString();
                    }
                }
                BindingSource addhocListBS = (BindingSource)adhocDataGridView.DataSource;
                var adhocListOnDisplay = (SortableBindingList<UserJobItemsDTO>)addhocListBS.DataSource;
                if (adhocListOnDisplay.Count > 0)
                {
                    foreach (UserJobItemsDTO userJobItemsDTO in adhocListOnDisplay)
                    {
                        if (userJobItemsDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(userJobItemsDTO.ChklstScheduleTime))
                            userJobItemsDTO.ChklstScheduleTime = utilities.getServerTime().ToString("yyyy-MM-dd HH:mm:ss");
                            if (userJobItemsDTO.ValidateTag)
                            {
                                if (!userJobItemsDTO.CardNumber.Equals(userJobItemsDTO.TaskCardNumber))
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(973) + ": " + userJobItemsDTO.TaskName);
                                    return;
                                }
                            }

                            if (userJobItemsDTO.Status == -1)
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(974) + ": " + userJobItemsDTO.TaskName);
                                return;
                            }
                            if (userJobItemsDTO.ChklistValue)
                            {
                                if (userJobItemsDTO.RemarksMandatory)
                                {
                                    if (string.IsNullOrEmpty(userJobItemsDTO.ChklistRemarks))
                                    {
                                        MessageBox.Show(utilities.MessageUtils.getMessage(972) + ": " + userJobItemsDTO.TaskName);
                                        return;
                                    }
                                }
                                if (lookupValuesDTO != null && lookupValuesDTO.Count > 0)
                                {
                                    if (userJobItemsDTO.Status != Convert.ToInt32(lookupValuesDTO[0].LookupValueId))
                                    {
                                        userJobItemsDTO.Status = Convert.ToInt32(lookupValuesDTO[0].LookupValueId);
                                        userJobItemsDTO.ChecklistCloseDate = DateTime.Now.ToString();
                                    }
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                if (lookupOpenValuesDTO != null)
                                {
                                    if (lookupValuesDTO[0].LookupValueId == userJobItemsDTO.Status 
                                        && userJobItemsDTO.ChklistValue == false)
                                        userJobItemsDTO.Status = Convert.ToInt32(lookupOpenValuesDTO[0].LookupValueId);
                                }
                                userJobItemsDTO.ChecklistCloseDate = "";
                            }
                        }
                        UserJobItemsBL adhoc = new UserJobItemsBL(machineUserContext, userJobItemsDTO);
                        adhoc.Save();
                    }
                    btnSearch.PerformClick();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            catch (Exception e1)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + ". " + e1.ToString(), "Adhoc Job Save Error");
                log.Error(e1);
            }
            log.LogMethodExit();
        }
        private void adhocDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (lookupValuesDTOList != null)
            { e.Row.Cells["statusDataGridViewTextBoxColumn"].Value = lookupValuesDTOList[0].LookupValueId; }
            e.Row.Cells["isActiveDataGridViewCheckBoxColumn"].Value = true;
            log.LogMethodExit();
        }
        private void adhocRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            setupGrid();
            LoadAssignedTo();
            statusId = -1;
            LoadStatus();
            LoadAsset();
            LoadTask();
            jobTypeId = GetJobTypeID();
            PopulateAdhocGrid();
            log.LogMethodExit();
        }
        private void adhocCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateAdhocGrid();
            log.LogMethodExit();
        }
        private void dtscheduleDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtScheduleFrom.Text = dtscheduleDate.Value.ToString(utilities.getDateFormat());
            log.LogMethodExit();
        }
        private void dtpCloseDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtScheduleTo.Text = dtpCloseDate.Value.ToString(utilities.getDateFormat());
            log.LogMethodExit();
        }
        private void chbPastDueDate_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtScheduleFrom.ResetText();
            txtScheduleTo.ResetText();
            if (statusId != -1)
            {
                cmbStatus.SelectedValue = statusId;
            }
            if (chbPastDueDate.Checked)
            {
                txtScheduleFrom.Enabled = false;
                txtScheduleTo.Enabled = false;
                cmbStatus.Enabled = false;
                dtscheduleDate.Enabled = false;
                dtpCloseDate.Enabled = false;
            }
            else
            {
                txtScheduleFrom.Enabled = true;
                txtScheduleTo.Enabled = true;
                cmbStatus.Enabled = true;
                dtscheduleDate.Enabled = true;
                dtpCloseDate.Enabled = true;
            }
            log.LogMethodExit();
        }
        private void adhocDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ", " + utilities.MessageUtils.getMessage("Column") + " " + adhocDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the task to the comboboxes
        /// </summary>
        private void LoadTask()
        {
            log.LogMethodEntry();
            jobTaskList = new JobTaskList(machineUserContext);
            List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> searchByJobTaskParameters = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
            searchByJobTaskParameters.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            jobTaskDTOListOnDisplay = jobTaskList.GetAllJobTasks(searchByJobTaskParameters);
            if (jobTaskDTOListOnDisplay == null)
            {
                jobTaskDTOListOnDisplay = new List<JobTaskDTO>();
            }
            BindingSource binding = new BindingSource();
            BindingSource searchBinding = new BindingSource();
            searchBinding.DataSource = jobTaskDTOListOnDisplay;
            binding.DataSource = jobTaskDTOListOnDisplay;
            jobTaskDTOListOnDisplay.Insert(0, new JobTaskDTO());
            JobTaskId.DataSource = jobTaskDTOListOnDisplay;
            JobTaskId.ValueMember = "JobTaskId";
            JobTaskId.ValueType = typeof(Int32);
            JobTaskId.DisplayMember = "TaskName";
            cmbTaskName.DataSource = binding;
            cmbTaskName.ValueMember = "JobTaskId";
            cmbTaskName.DisplayMember = "TaskName";
            dgvTaskSearch.DataSource = searchBinding;
            for (int i = 0; i < dgvTaskSearch.Columns.Count; i++)
            {
                if (!dgvTaskSearch.Columns[i].Name.Equals("TaskName"))
                {
                    dgvTaskSearch.Columns[i].Visible = false;
                }
                else
                {
                    dgvTaskSearch.Columns[i].Width = dgvTaskSearch.Width;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads asset to the Comboboxes
        /// </summary>
        private void LoadAsset()
        {
            log.LogMethodEntry();
            assetList = new AssetList(machineUserContext);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>  searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            genericAssetListOnDisplay = assetList.GetAllAssets(searchByAssetParameters);
            if (genericAssetListOnDisplay == null)
            {
                genericAssetListOnDisplay = new List<GenericAssetDTO>();
            }
            BindingSource binding = new BindingSource();
            BindingSource searchBinding = new BindingSource();
            searchBinding.DataSource = genericAssetListOnDisplay;
            binding.DataSource = genericAssetListOnDisplay;
            genericAssetListOnDisplay.Insert(0, new GenericAssetDTO());
            assetIdDataGridViewTextBoxColumn.DataSource = genericAssetListOnDisplay;
            assetIdDataGridViewTextBoxColumn.ValueMember = "AssetId";
            assetIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
            assetIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            cmbAssetName.DataSource = binding;
            cmbAssetName.ValueMember = "AssetId";
            cmbAssetName.DisplayMember = "Name";
            dgvAssetNameSearch.DataSource = searchBinding;
            for (int i = 0; i < dgvAssetNameSearch.Columns.Count; i++)
            {
                if (!dgvAssetNameSearch.Columns[i].Name.Equals("Name"))
                {
                    dgvAssetNameSearch.Columns[i].Visible = false;
                }
                else
                {
                    dgvAssetNameSearch.Columns[i].Width = dgvAssetNameSearch.Width;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// setup grid
        /// </summary>
        private void setupGrid()
        {
            log.LogMethodEntry();
            for (int i = 0; i < adhocDataGridView.Columns.Count; i++)
            {
                adhocDataGridView.Columns[i].Visible = false;
            }

            adhocDataGridView.Columns["taskNameDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["assetNameDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["chklistValueDataGridViewCheckBoxColumn"].Visible =
            adhocDataGridView.Columns["chklistRemarksDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["validateTagDataGridViewCheckBoxColumn"].Visible =
            adhocDataGridView.Columns["cardNumberDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["statusDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["btnRaiseServiceRequest"].Visible =
            adhocDataGridView.Columns["isActiveDataGridViewCheckBoxColumn"].Visible = true;
            adhocDataGridView.Columns["assignedUserIdDataGridViewTextBoxColumn"].Visible = (utilities.ParafaitEnv.Manager_Flag == "Y");
            adhocDataGridView.Columns["assignedToDataGridViewTextBoxColumn"].Visible = !(utilities.ParafaitEnv.Manager_Flag == "Y");

            adhocDataGridView.Columns["taskNameDataGridViewTextBoxColumn"].ReadOnly =
            adhocDataGridView.Columns["assetNameDataGridViewTextBoxColumn"].ReadOnly =
            adhocDataGridView.Columns["validateTagDataGridViewCheckBoxColumn"].ReadOnly =
            adhocDataGridView.Columns["cardNumberDataGridViewTextBoxColumn"].ReadOnly =
            adhocDataGridView.Columns["assignedToDataGridViewTextBoxColumn"].ReadOnly =
            adhocDataGridView.Columns["isActiveDataGridViewCheckBoxColumn"].ReadOnly = true;

            adhocDataGridView.Columns["statusDataGridViewTextBoxColumn"].ReadOnly = !(utilities.ParafaitEnv.Manager_Flag == "Y");

            adhocDataGridView.Columns["taskNameDataGridViewTextBoxColumn"].DisplayIndex = 1;
            adhocDataGridView.Columns["assetNameDataGridViewTextBoxColumn"].DisplayIndex = 2;
            adhocDataGridView.Columns["chklistValueDataGridViewCheckBoxColumn"].DisplayIndex = 3;
            adhocDataGridView.Columns["chklistRemarksDataGridViewTextBoxColumn"].DisplayIndex = 4;
            adhocDataGridView.Columns["validateTagDataGridViewCheckBoxColumn"].DisplayIndex = 5;
            adhocDataGridView.Columns["cardNumberDataGridViewTextBoxColumn"].DisplayIndex = 6;
            adhocDataGridView.Columns["statusDataGridViewTextBoxColumn"].DisplayIndex = 7;
            adhocDataGridView.Columns["assignedToDataGridViewTextBoxColumn"].DisplayIndex = 8;
            adhocDataGridView.Columns["isActiveDataGridViewCheckBoxColumn"].DisplayIndex = 9;
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the status to the comboboxes
        /// </summary>
        private void LoadStatus()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList != null)
                {
                    BindingSource bindingSource = new BindingSource();
                    lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                    bindingSource.DataSource = lookupValuesDTOList;
                    statusDataGridViewTextBoxColumn.DataSource = lookupValuesDTOList;
                    statusDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                    statusDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                    statusDataGridViewTextBoxColumn.DisplayMember = "Description";
                    cmbStatus.DataSource = bindingSource;
                    cmbStatus.ValueMember = "LookupValueId";
                    cmbStatus.DisplayMember = "Description";
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Open"));
                    List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);
                    cmbStatus.SelectedValue = lookupOpenValuesDTOList[0].LookupValueId;
                    statusId = lookupOpenValuesDTOList[0].LookupValueId;
                }
                else
                {
                    MessageBox.Show("'MAINT_JOB_STATUS' is not found in Lookups");
                } 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads data source to assigned to combobox
        /// </summary>
        private void LoadAssignedTo()
        {
            log.LogMethodEntry();
            try
            { 
                UsersList usersList = new UsersList(machineUserContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                usersDTOList = usersList.GetAllUsers(usersSearchParams);
                if (usersDTOList != null)
                {
                    assignedToBS = new BindingSource();
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = usersDTOList;
                    usersDTOList.Insert(0, new UsersDTO());
                    assignedToBS.DataSource = usersDTOList;
                    cmbAssignedTo.DataSource = usersDTOList;
                    cmbAssignedTo.DisplayMember = "UserName";
                    cmbAssignedTo.ValueMember = "UserId";

                    assignedUserIdDataGridViewTextBoxColumn.DataSource = assignedToBS;
                    assignedUserIdDataGridViewTextBoxColumn.ValueMember = "UserId";
                    assignedUserIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                    assignedUserIdDataGridViewTextBoxColumn.DisplayMember = "UserName";
                    dgvAssignedToSearch.DataSource = bindingSource;
                    for (int i = 0; i < dgvAssignedToSearch.Columns.Count; i++)
                    {
                        if (!dgvAssignedToSearch.Columns[i].Name.Equals("UserName"))
                        {
                            dgvAssignedToSearch.Columns[i].Visible = false;
                        }
                        else
                        {
                            dgvAssignedToSearch.Columns[i].Width = dgvAssignedToSearch.Width;
                        }
                    }
                } 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Function loads the records to the grid.
        /// </summary>
        private void PopulateAdhocGrid()
        {
            log.LogMethodEntry();
            try
            { 
                if (!cmbAssetName.Text.Equals(txtName.Text))
                {
                    cmbAssetName.Text = "";
                    txtName.Text = "";
                }
                if (!cmbAssignedTo.Text.Equals(txtAssignedTo.Text))
                {
                    cmbAssignedTo.Text = "";
                    txtAssignedTo.Text = "";
                }
                if (!cmbTaskName.Text.Equals(txtTaskNameSearch.Text))
                {
                    cmbTaskName.Text = "";
                    txtTaskNameSearch.Text = "";
                }
                List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> userJobItemsParameters = new List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>>();
                if (jobTypeId != -1)
                {
                    userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_TYPE, (jobTypeId == -1) ? "" : jobTypeId.ToString()));
                }
                if (cmbAssignedTo.SelectedValue != null && !cmbAssignedTo.SelectedValue.ToString().Equals("-1"))
                userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.ASSIGNED_TO, (cmbAssignedTo.SelectedValue==null||cmbAssignedTo.SelectedValue.ToString().Equals("-1")) ? "" : cmbAssignedTo.SelectedValue.ToString()));
                if (cmbAssetName.SelectedValue != null && !cmbAssetName.SelectedValue.ToString().Equals("-1"))
                userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_ID, (cmbAssetName.SelectedValue==null||cmbAssetName.SelectedValue.ToString().Equals("-1")) ? "" : cmbAssetName.SelectedValue.ToString()));
                if (cmbTaskName.SelectedValue != null && !cmbTaskName.SelectedValue.ToString().Equals("-1"))
                userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.TASK_ID, (cmbTaskName.SelectedValue==null||cmbTaskName.SelectedValue.ToString().Equals("-1")) ? "" : cmbTaskName.SelectedValue.ToString()));
                if (cmbStatus.SelectedValue != null && !cmbStatus.SelectedValue.ToString().Equals("-1"))
                userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.STATUS, (cmbStatus.SelectedValue==null||cmbStatus.SelectedValue.ToString().Equals("-1")) ? "" : cmbStatus.SelectedValue.ToString()));
                if (chbPastDueDate.Checked)
                    userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.PAST_DUE_DATE, "Y"));
                try
                {
                    if (!string.IsNullOrEmpty(txtScheduleFrom.Text))
                    {
                        userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE, DateTime.Parse(txtScheduleFrom.Text).ToString("yyyy-MM-dd")));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    string msg = utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("From") });
                    MessageBox.Show(msg);
                    log.LogMethodExit(msg);
                    return;
                }
                try
                {
                    if (!string.IsNullOrEmpty(txtScheduleTo.Text))
                    {
                        if (!string.IsNullOrEmpty(txtScheduleFrom.Text) && DateTime.Parse(txtScheduleFrom.Text).CompareTo(DateTime.Parse(txtScheduleTo.Text)) <= 0)
                            userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE, DateTime.Parse(txtScheduleTo.Text).ToString("yyyy-MM-dd")));
                        else
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(971));
                            log.Debug("Ends-btnSearch_Click() Event by showing \"To Date should be greater than from date.\" message.");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    string msg = utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("To") });
                   MessageBox.Show(msg);
                    log.LogMethodExit(msg);
                    return;
                }
                userJobItemsParameters.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                UserJobItemsListBL userJobItemsListBL = new UserJobItemsListBL(machineUserContext);
                userJobTaskItemsBS = new BindingSource();
                List<UserJobItemsDTO> userJobItemsDTOList = userJobItemsListBL.GetAllUserJobItemDTOList(userJobItemsParameters, utilities.ParafaitEnv.User_Id);
                if (userJobItemsDTOList != null)
                {
                    SortableBindingList<UserJobItemsDTO> userJobItemsDTOOSortList = new SortableBindingList<UserJobItemsDTO>(userJobItemsDTOList);
                    userJobTaskItemsBS.DataSource = userJobItemsDTOOSortList;
                }
                else
                {
                    userJobTaskItemsBS.DataSource = new SortableBindingList<UserJobItemsDTO>();
                }
                adhocDataGridView.DataSource = userJobTaskItemsBS;
                chklstScheduleTimeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
                adhocDataGridView.DataSource = userJobTaskItemsBS;
                adhocDataGridView.Columns["chklstScheduleTimeDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
                adhocDataGridView.DataError += new DataGridViewDataErrorEventHandler(adhocDataGridView_ComboDataError); 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }

        private void adhocDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.adhocDataGridView.SelectedRows.Count <= 0 && this.adhocDataGridView.SelectedCells.Count <= 0)
            {
                string msg = utilities.MessageUtils.getMessage(959);
                MessageBox.Show(msg);
                log.LogMethodExit(msg);
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.adhocDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.adhocDataGridView.SelectedCells)
                {
                    adhocDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow maintenanceTaskRow in this.adhocDataGridView.SelectedRows)
            {
                if (maintenanceTaskRow.Cells[0].Value == null)
                {
                    log.LogMethodExit("maintenanceTaskRow.Cells[0].Value == null");
                    return;
                }
                if (Convert.ToInt32(maintenanceTaskRow.Cells[0].Value.ToString()) <= 0)
                {
                    adhocDataGridView.Rows.RemoveAt(maintenanceTaskRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource userJobItemsDTOListDTOBS = (BindingSource)adhocDataGridView.DataSource;
                        var userJobItemsDTOList = (SortableBindingList<UserJobItemsDTO>)userJobItemsDTOListDTOBS.DataSource;
                        UserJobItemsDTO userJobItemsDTO = userJobItemsDTOList[maintenanceTaskRow.Index];
                        userJobItemsDTO.IsActive = false;
                        UserJobItemsBL userJobItemsBL = new UserJobItemsBL(machineUserContext, userJobItemsDTO);
                        userJobItemsBL.Save();
                    }
                }
            }
            if (rowsDeleted == true)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            btnSearch.PerformClick();
            log.LogMethodExit();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (loadSearch)
            {
                if (txtName.Text.Length > 0)
                {
                    if (genericAssetListOnDisplay != null)
                    {
                        List<GenericAssetDTO> assetDTOList = genericAssetListOnDisplay.Where(x => (bool)((string.IsNullOrEmpty(x.Name) ? "" : x.Name.ToLower()).Contains(txtName.Text.ToLower()))).ToList<GenericAssetDTO>();
                        if (assetDTOList.Count > 0)
                        {
                            dgvAssetNameSearch.Visible = true;
                            dgvAssetNameSearch.DataSource = assetDTOList;
                        }
                        else
                        {
                            dgvAssetNameSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbAssetName.Text = "";
                    dgvAssetNameSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void dgvAssetNameSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtName.Text = dgvAssetNameSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbAssetName.Text = txtName.Text;
                dgvAssetNameSearch.Visible = false;
            }
            catch (Exception ex){ log.Error(ex); }
            log.LogMethodExit();
        }

        private void cmbAssetName_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cmbAssetName.Text.Equals("Semnox.Parafait.Maintenance.GenericAssetDTO"))
            {
                txtName.Text = "";
            }
            else
            {
                txtName.Text = cmbAssetName.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void txtAssignedTo_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (loadSearch)
            {
                if (txtAssignedTo.Text.Length > 0)
                {
                    if (usersDTOList != null)
                    {
                        List<UsersDTO> usersList = usersDTOList.Where(x => (bool)((string.IsNullOrEmpty(x.UserName) ? "" : x.UserName.ToLower()).Contains(txtAssignedTo.Text.ToLower()))).ToList<UsersDTO>();
                        if (usersList.Count > 0)
                        {
                            dgvAssignedToSearch.Visible = true;
                            dgvAssignedToSearch.DataSource = usersList;
                        }
                        else
                        {
                            dgvAssignedToSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbAssignedTo.Text = "";
                    dgvAssignedToSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void dgvAssignedTo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtAssignedTo.Text = dgvAssignedToSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbAssignedTo.Text = txtAssignedTo.Text;
                dgvAssignedToSearch.Visible = false;
            }
            catch (Exception ex){ log.Error(ex); }
            log.LogMethodExit();
        }

        private void cmbAssignedTo_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cmbAssignedTo.Text.Equals("Semnox.Parafait.User.UsersDTO"))
            {
                txtAssignedTo.Text = "";
            }
            else
            {
                txtAssignedTo.Text = cmbAssignedTo.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void txtTaskNameSearch_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (loadSearch)
            {
                if (txtTaskNameSearch.Text.Length > 0)
                {
                    if (jobTaskDTOListOnDisplay != null)
                    {
                        List<JobTaskDTO> jobTaskDTOList = jobTaskDTOListOnDisplay.Where(x => (bool)((string.IsNullOrEmpty(x.TaskName) ? "" : x.TaskName.ToLower()).Contains(txtTaskNameSearch.Text.ToLower()))).ToList<JobTaskDTO>();
                        if (jobTaskDTOList.Count > 0)
                        {
                            dgvTaskSearch.Visible = true;
                            dgvTaskSearch.DataSource = jobTaskDTOList;
                        }
                        else
                        {
                            dgvTaskSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    txtTaskNameSearch.Text = "";
                    dgvTaskSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void cmbTaskName_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cmbTaskName.Text.Equals("Semnox.Parafait.Maintenance.JobTaskDTO"))
            {
                txtTaskNameSearch.Text = "";
            }
            else
            {
                txtTaskNameSearch.Text = cmbTaskName.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void dgvTaskSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtTaskNameSearch.Text = dgvTaskSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbTaskName.Text = txtTaskNameSearch.Text;
                dgvTaskSearch.Visible = false;
            }
            catch (Exception ex){ log.Error(ex); }
            log.LogMethodExit();
        }

        private void cmbCommon_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAssignedToSearch.Visible = dgvAssetNameSearch.Visible = dgvTaskSearch.Visible = false;
            log.LogMethodExit();
        }

        private void adhocDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (adhocDataGridView.Rows[e.RowIndex].Cells["cardNumberDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
                {
                    txtCardNumber.Visible = true;
                    txtCardNumber.Focus();
                }
                else if (adhocDataGridView.Rows[e.RowIndex].Cells["btnRaiseServiceRequest"].ColumnIndex == e.ColumnIndex)
                {
                    UpdateMaintenanceRequest updateMaintenanceRequest = new UpdateMaintenanceRequest(utilities, Convert.ToInt32(adhocDataGridView.Rows[e.RowIndex].Cells["maintChklstdetIdDataGridViewTextBoxColumn"].Value));
                    updateMaintenanceRequest.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }



        private void txtCardNumber_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                if (adhocDataGridView.CurrentCell != null)
                {
                    adhocDataGridView.CurrentCell.Value = txtCardNumber.Text;
                    txtCardNumber.Text = "";
                    txtCardNumber.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        private void txtCardNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (char.IsLetterOrDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            { e.KeyChar = char.ToUpper(e.KeyChar); }
            else
            { e.Handled = true; }
            log.LogMethodExit();
        }
        /// <summary>
        /// To fix the issue hiding the List on click of any controls in the form
        /// </summary>
        /// <param name="control"></param>
        private void RegisterKeyDownHandlers(Control control)
        {
            log.LogMethodEntry(control);
            foreach (Control ctl in control.Controls)
            {
                ctl.Click += MyKeyPressEventHandler;
                RegisterKeyDownHandlers(ctl);
            }
            log.LogMethodExit();
        }

        public void MyKeyPressEventHandler(Object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAssetNameSearch.Visible = dgvTaskSearch.Visible = dgvAssignedToSearch.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }

        private void GenericAssetUI_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAssetNameSearch.Visible = dgvTaskSearch.Visible = dgvAssignedToSearch.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Down)
            {
                if (dgvAssetNameSearch.Rows.Count > 0)
                {
                    dgvAssetNameSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssetNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtName.Text = dgvAssetNameSearch.SelectedCells[0].Value.ToString();
                    cmbAssetName.Text = txtName.Text;
                    dgvAssetNameSearch.Visible = false;
                    txtName.Focus();
                }
                catch (Exception ex) { log.Error(ex); }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtName.Focus();
            }
            log.LogMethodExit();
        }

        private void txtTaskNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Down)
            {
                if (dgvTaskSearch.Rows.Count > 0)
                {
                    dgvTaskSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvTaskSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtTaskNameSearch.Text = dgvTaskSearch.SelectedCells[0].Value.ToString();
                    cmbTaskName.Text = txtTaskNameSearch.Text;
                    dgvTaskSearch.Visible = false;
                    txtTaskNameSearch.Focus();
                }
                catch (Exception ex){ log.Error(ex); }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtTaskNameSearch.Focus();
            }
            log.LogMethodExit();
        }

        private void txtAssignedTo_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Down)
            {
                if (dgvTaskSearch.Rows.Count > 0)
                {
                    dgvTaskSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssignedToSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtAssignedTo.Text = dgvAssignedToSearch.SelectedCells[0].Value.ToString();
                    cmbAssignedTo.Text = txtAssignedTo.Text;
                    dgvAssignedToSearch.Visible = false;
                    txtAssignedTo.Focus();
                }
                catch (Exception ex){ log.Error(ex); }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtAssignedTo.Focus();
            }
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e) 
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (adhocDataGridView.CurrentRow != null)
                {
                    int jobIdSelected = -1;
                    if (adhocDataGridView.CurrentRow.Cells["maintChklstdetIdDataGridViewTextBoxColumn"].Value != null)
                    {
                        int.TryParse(adhocDataGridView.CurrentRow.Cells["maintChklstdetIdDataGridViewTextBoxColumn"].Value.ToString(), out jobIdSelected);
                    }
                    if (jobIdSelected >= 0)
                    {
                        Publish.PublishUI publishUI = new Publish.PublishUI(utilities, jobIdSelected, "MaintenanceJob/Service", adhocDataGridView.CurrentRow.Cells["taskNameDataGridViewTextBoxColumn"].Value.ToString());
                        publishUI.ShowDialog();
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Maintenance Job Publish");
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void MaintenanceJobDetailsUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnPublishToSite.Size = new Size(127, 23);
            log.LogMethodExit();
        }
        BindingSource maintJobDTOListDTOBS;
        UserJobItemsDTO maintJobDTO;
        private void adhocDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (adhocDataGridView.Rows.Count>0)
            {
                maintJobDTOListDTOBS = (BindingSource)adhocDataGridView.DataSource;
                var maintenanceJobDTOList = (SortableBindingList<UserJobItemsDTO>)maintJobDTOListDTOBS.DataSource;
                 maintJobDTO = maintenanceJobDTOList[e.RowIndex];
                 if (maintJobDTO.JobScheduleId==-1)
                 {
                     if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite) 
                     {
                         btnPublishToSite.Enabled = true;
                     }
                     else
                     {
                         btnPublishToSite.Enabled = false;
                     }
                 }
                 else
                 {
                     btnPublishToSite.Enabled = false;
                 }
            }
            log.LogMethodExit();
        }
    }
}

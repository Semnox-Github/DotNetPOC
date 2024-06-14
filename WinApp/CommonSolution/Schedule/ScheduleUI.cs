/********************************************************************************************
 * Project Name - Schedule  UI
 * Description  - User interface for Schedule Calendar
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera     Modified         
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
 ********************************************************************************************/


using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Maintenance;
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
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.Schedule
{
    /// <summary>
    /// Schedule UI performs save, Edit View operation of the Schedule
    /// </summary>
    public partial class ScheduleUI : Form
    {
        AssetList assetList;
        List<GenericAssetDTO> genericAssetListOnDisplay;
        AssetGroupList assetGroupList;
        List<AssetGroupDTO> assetGroupListOnDisplay;
        AssetTypeList assetTypeList;
        List<AssetTypeDTO> assetTypeListOnDisplay;
        JobTaskList jobTaskList;
        List<JobTaskDTO> jobTaskDTOListOnDisplay;
        JobTaskGroupList jobTaskGroupListt;
        List<JobTaskGroupDTO> jobTaskGroupDTOListOnDisplay;
        Semnox.Core.Utilities.Utilities utilities;
        BindingSource scheduleAssetTaskListBS;
        ScheduleCalendarDTO scheduleDTO;
        JobScheduleDTO jobScheduleDTO = new JobScheduleDTO();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        public ScheduleUI(ScheduleCalendarDTO _scheduleDTO, Semnox.Core.Utilities.Utilities _Utilities)
        {
            log.LogMethodEntry(_scheduleDTO, _Utilities);
            InitializeComponent();
            utilities = _Utilities;
            _Utilities.setupDataGridProperties(ref scheduleDataGridView);
            scheduleDTO = _scheduleDTO;
            
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                btnPublishToSite.Visible = true;
            }
            else
            {
                btnPublishToSite.Visible = false;
            } 
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            InitSchedule();
            log.LogMethodExit();
        }
        private void scheduleSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblMessage.Text = "";
            if (Validate())
            {
                if (SaveSchedule())
                {                    
                    if (SaveMaintSchedule())
                    {
                        SaveScheduleAssetTask();
                        lblMessage.Text = utilities.MessageUtils.getMessage(1197, utilities.MessageUtils.getMessage("schedule"));
                        PopulateScheduleGrid();
                    }
                }
            }
            log.LogMethodExit();
        }
        private void rbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (rbMonthly.Checked)
            {
                rbRecurTypeDaily.Checked = true;
                gpRecurType.Enabled = true;
            }
            else
            {
                rbRecurTypeDaily.Checked = false;
                rbRecurTypeWeekly.Checked = false;
                gpRecurType.Enabled = false;
            }
            log.LogMethodExit();
        }
        private void chbRecur_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (chbRecur.Checked)
            {
                rbDaily.Checked = true;
                rbWeekly.Checked = false;
                rbMonthly.Checked = false;
                dtpEndDate.Enabled =
                rbDaily.Enabled =
                rbWeekly.Enabled =
                rbMonthly.Enabled = true;
            }
            else
            {
                rbRecurTypeDaily.Checked = false;
                rbRecurTypeWeekly.Checked = false;
                gpRecurType.Enabled = false;

                rbDaily.Checked = false;
                rbWeekly.Checked = false;
                rbMonthly.Checked = false;
                dtpEndDate.Enabled =
                rbDaily.Enabled =
                rbWeekly.Enabled =
                rbMonthly.Enabled = false;
            }
            log.LogMethodExit();
        }
        private void scheduleDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Row.Cells["maintScheduleIdDataGridViewTextBoxColumn"].Value = jobScheduleDTO.JobScheduleId;
            log.LogMethodExit();
        }
        private void exclusionDaysbtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ScheduleExclusionUI scheduleExclusionUI = new ScheduleExclusionUI(scheduleDTO, utilities);
            scheduleExclusionUI.ShowDialog();
            log.LogMethodExit();
        }
        private void scheduleRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InitSchedule();
            log.LogMethodExit();
        }
        private void scheduleCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
        private void scheduleDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scheduleDataGridView.Rows.Count == scheduleAssetTaskListBS.Count)
            {
                scheduleAssetTaskListBS.RemoveAt(scheduleAssetTaskListBS.Count - 1);
            }
            log.LogMethodExit();
        }
        private void scheduleDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == scheduleDataGridView.Columns["maintTaskGroupIdDataGridViewTextBoxColumn"].Index)
            {
                if (jobTaskGroupDTOListOnDisplay != null)
                    scheduleDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = jobTaskGroupDTOListOnDisplay[0].JobTaskGroupId;
            }
            else if (e.ColumnIndex == scheduleDataGridView.Columns["maintTaskIdDataGridViewTextBoxColumn"].Index)
            {
                if (jobTaskDTOListOnDisplay != null)
                    scheduleDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = jobTaskDTOListOnDisplay[0].JobTaskId;
            }
            else if (e.ColumnIndex == scheduleDataGridView.Columns["assetTypeIdDataGridViewTextBoxColumn"].Index)
            {
                if (assetTypeListOnDisplay != null)
                    scheduleDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = assetTypeListOnDisplay[0].AssetTypeId;
            }
            else if (e.ColumnIndex == scheduleDataGridView.Columns["assetGroupIdDataGridViewTextBoxColumn"].Index)
            {
                if (assetGroupListOnDisplay != null)
                    scheduleDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = assetGroupListOnDisplay[0].AssetGroupId;
            }
            else if (e.ColumnIndex == scheduleDataGridView.Columns["assetIDDataGridViewTextBoxColumn"].Index)
            {
                if (genericAssetListOnDisplay != null)
                    scheduleDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = genericAssetListOnDisplay[0].AssetId;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// saving schedule data to databasse and returns the status
        /// </summary>
        /// <returns>returns bool type status</returns>
        private bool SaveSchedule()
        {
            log.LogMethodEntry();
            try
            {
                if (scheduleDTO == null)
                {
                    scheduleDTO = new ScheduleCalendarDTO();
                }
                scheduleDTO.ScheduleName = txtName.Text;
                scheduleDTO.IsActive = (chbActive.Checked) ? true : false;
                scheduleDTO.ScheduleTime = dtscheduleDate.Value.Date;
                scheduleDTO.ScheduleTime = scheduleDTO.ScheduleTime.AddHours((double)(cmbTime.SelectedIndex / 2.0));
                scheduleDTO.RecurEndDate = (chbRecur.Checked) ? dtpEndDate.Value.Date.AddHours((double)(cmbTime.SelectedIndex / 2.0)) : DateTime.MinValue;
                scheduleDTO.RecurFlag = (chbRecur.Checked) ? "Y" : "N";
                if (scheduleDTO.RecurFlag.Equals("Y"))
                {
                    if (scheduleDTO.ScheduleTime.CompareTo(scheduleDTO.RecurEndDate) > 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(993));
                        log.LogMethodExit("scheduleDTO.ScheduleTime.CompareTo(scheduleDTO.RecurEndDate) > 0");
                        return false;
                    }
                }
                scheduleDTO.RecurFrequency = (chbRecur.Checked) ? ((rbDaily.Checked) ? "D" : ((rbWeekly.Checked) ? "W" : ((rbMonthly.Checked) ? "M" : "D"))) : null;
                scheduleDTO.RecurType = (chbRecur.Checked) ? ((rbMonthly.Checked) ? ((rbRecurTypeWeekly.Checked) ? "W" : "D") : "D") : null;
                Semnox.Core.GenericUtilities.ScheduleCalendarBL schedule = new Semnox.Core.GenericUtilities.ScheduleCalendarBL(machineUserContext, scheduleDTO);
                schedule.Save();
                if (scheduleDTO.ScheduleId == -1)
                {
                    log.Debug("End-SaveSchedule() Method ScheduleId=-1");
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                    log.LogMethodExit("scheduleDTO.ScheduleId == -1");
                    return false;
                }
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception e)
            {
                log.Fatal("Ends-SaveSchedule() Method with exception:" + e.Message + e.StackTrace);
                MessageBox.Show(utilities.MessageUtils.getMessage(994));
                log.LogMethodExit("Exception e");
                return false;
            }
        }
        /// <summary>
        /// Saves maintenance schedule details and returns the status
        /// </summary>
        /// <returns>returns bool type status</returns>
        private bool SaveMaintSchedule()
        {
            log.LogMethodEntry();
            try
            {
                if (scheduleDataGridView.Rows.Count > 1 && string.IsNullOrEmpty(cmbAssignedTo.Text))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(979));
                    cmbAssignedTo.Focus();
                    log.LogMethodExit("scheduleDataGridView.Rows.Count > 1 && string.IsNullOrEmpty(cmbAssignedTo.Text)");
                    return false;
                }
                if (jobScheduleDTO.JobScheduleId>=0)
                {
                    if (string.IsNullOrEmpty(cmbAssignedTo.Text))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(979));
                        cmbAssignedTo.Focus();
                        log.LogMethodExit("string.IsNullOrEmpty(cmbAssignedTo.Text)");
                        return false;
                    }
                    if (string.IsNullOrEmpty(txtCompleted.Text))
                    {
                        txtCompleted.Text = "";
                    }
                    else
                    {
                        try
                        {
                            int x = int.Parse(txtCompleted.Text);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            MessageBox.Show(utilities.MessageUtils.getMessage(992), "Invalid no of days.");
                            txtCompleted.Focus();
                            log.LogMethodExit("Invalid no of days");
                            return false;
                        }
                    }
                }
                jobScheduleDTO.ScheduleId = scheduleDTO.ScheduleId;
                jobScheduleDTO.IsActive = (chbActive.Checked);
                jobScheduleDTO.DurationToComplete = (string.IsNullOrEmpty(txtCompleted.Text)) ? -1 : Convert.ToInt32(txtCompleted.Text);
                jobScheduleDTO.UserId = Convert.ToInt32(cmbAssignedTo.SelectedValue);
                if(jobScheduleDTO.JobScheduleId<0 && (int)cmbAssignedTo.SelectedValue < 0)
                {
                    log.LogMethodExit("jobScheduleDTO.JobScheduleId<0 && (int)cmbAssignedTo.SelectedValue < 0");
                    return false;
                }
                JobScheduleBL maintenanceSchedule = new JobScheduleBL(machineUserContext, jobScheduleDTO);
                maintenanceSchedule.SaveShedule();
                if (jobScheduleDTO.JobScheduleId < 0)
                {
                    log.LogMethodExit("jobScheduleDTO.JobScheduleId < 0");
                    return false;
                }
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception e)
            {
                log.Fatal("Ends-SaveMaintSchedule() Method with exception:" + e.Message + e.StackTrace);
                MessageBox.Show(utilities.MessageUtils.getMessage(995));
                return false;
            }
        }
        /// <summary>
        /// Saves Schedue,asset and task mapping records and returns the status
        /// </summary>
        /// <returns>Returns bool type status</returns>
        private bool SaveScheduleAssetTask()
        {
            log.LogMethodEntry();
            try
            { 
                if (jobScheduleDTO.JobScheduleId >= 0)
                {
                    BindingSource scheduleAssetTaskListBS = (BindingSource)scheduleDataGridView.DataSource;
                    var scheduleAssetTaskListOnDisplay = (SortableBindingList<JobScheduleTasksDTO>)scheduleAssetTaskListBS.DataSource;
                    if (scheduleAssetTaskListOnDisplay.Count > 0)
                    {
                        foreach (JobScheduleTasksDTO scheduleAssetTaskDTO in scheduleAssetTaskListOnDisplay)
                        {
                            if (scheduleAssetTaskDTO.IsChanged)
                                scheduleAssetTaskDTO.JobScheduleId = jobScheduleDTO.JobScheduleId;
                            if (scheduleAssetTaskDTO.AssetID == -1 && scheduleAssetTaskDTO.AssetTypeId == -1 && scheduleAssetTaskDTO.AssetGroupId == -1)
                            {
                                continue;
                            }
                            if (scheduleAssetTaskDTO.JobTaskId == -1 && scheduleAssetTaskDTO.JObTaskGroupId == -1)
                            {
                                continue;
                            }
                            JobScheduleTasksBL scheduleAssetTask = new JobScheduleTasksBL(machineUserContext, scheduleAssetTaskDTO);
                            scheduleAssetTask.Save(null);
                        }
                        scheduleDataGridView.DataSource = null;
                        scheduleDataGridView.DataSource = scheduleAssetTaskListBS;
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        log.LogMethodExit("scheduleAssetTaskListOnDisplay.count<0 ends by returning false");
                        return true;
                    }
                }
                else
                {
                    log.Info("MaintScheduleId=-1");
                    MessageBox.Show(utilities.MessageUtils.getMessage(995));
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception e1)
            {
                log.Fatal("Ends-SaveScheduleAssetTask() Method with exception:" + e1.Message + e1.StackTrace);
                MessageBox.Show(utilities.MessageUtils.getMessage(996));
                log.LogMethodExit(false);
                return false;
            } 

        }
        /// <summary>
        /// Initialize the schedule screen
        /// </summary>
        private void InitSchedule()
        {
            log.LogMethodEntry();
            SetTime();
            LoadAssignedTo();
            LoadGridCombo();
            if (scheduleDTO != null)
            {
                GetSchedule();
                GetMaintSchedule();
            }
            PopulateScheduleGrid();
            log.LogMethodExit();
        }
        /// <summary>
        /// Validates the input
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(997));
                txtName.Focus();
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// Loads all the combobox datasources
        /// </summary>
        private void LoadGridCombo()
        {
            log.LogMethodEntry();
            try
            {
                assetList = new AssetList(machineUserContext);
                List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                genericAssetListOnDisplay = assetList.GetAllAssets(assetSearchParams);
                if (genericAssetListOnDisplay == null)
                {
                    genericAssetListOnDisplay = new List<GenericAssetDTO>();
                }
                genericAssetListOnDisplay.Insert(0, new GenericAssetDTO());
                genericAssetListOnDisplay[0].Name = "None";
                assetIDDataGridViewTextBoxColumn.DataSource = genericAssetListOnDisplay;
                assetIDDataGridViewTextBoxColumn.ValueMember = "AssetId";
                assetIDDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                assetIDDataGridViewTextBoxColumn.DisplayMember = "Name";


                assetGroupList = new AssetGroupList(machineUserContext);
                List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> assetGroupSearchParams = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
                assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                assetGroupListOnDisplay = assetGroupList.GetAllAssetGroups(assetGroupSearchParams);
                if (assetGroupListOnDisplay == null)
                {
                    assetGroupListOnDisplay = new List<AssetGroupDTO>();
                }
                assetGroupListOnDisplay.Insert(0, new AssetGroupDTO());
                assetGroupListOnDisplay[0].AssetGroupName = "None";
                assetGroupIdDataGridViewTextBoxColumn.DataSource = assetGroupListOnDisplay;
                assetGroupIdDataGridViewTextBoxColumn.ValueMember = "AssetGroupId";
                assetGroupIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                assetGroupIdDataGridViewTextBoxColumn.DisplayMember = "AssetGroupName";

                assetTypeList = new AssetTypeList(utilities.ExecutionContext);
                List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> assetTypeSearchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                assetTypeListOnDisplay = assetTypeList.GetAllAssetTypes(assetTypeSearchParams);
                if (assetTypeListOnDisplay == null)
                {
                    assetTypeListOnDisplay = new List<AssetTypeDTO>();
                }
                assetTypeListOnDisplay.Insert(0, new AssetTypeDTO());
                assetTypeListOnDisplay[0].Name = "None";
                assetTypeIdDataGridViewTextBoxColumn.DataSource = assetTypeListOnDisplay;
                assetTypeIdDataGridViewTextBoxColumn.ValueMember = "AssetTypeId";
                assetTypeIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                assetTypeIdDataGridViewTextBoxColumn.DisplayMember = "Name";

                jobTaskList = new JobTaskList(machineUserContext);
                List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> maintenanceTaskSearchParams = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
                maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                jobTaskDTOListOnDisplay = jobTaskList.GetAllJobTasks(maintenanceTaskSearchParams);
                if (jobTaskDTOListOnDisplay == null)
                {
                    jobTaskDTOListOnDisplay = new List<JobTaskDTO>();
                }
                jobTaskDTOListOnDisplay.Insert(0, new JobTaskDTO());
                jobTaskDTOListOnDisplay[0].TaskName = "None";
                maintTaskIdDataGridViewTextBoxColumn.DataSource = jobTaskDTOListOnDisplay;
                maintTaskIdDataGridViewTextBoxColumn.ValueMember = "JobTaskId";
                maintTaskIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                maintTaskIdDataGridViewTextBoxColumn.DisplayMember = "TaskName";


                jobTaskGroupListt = new JobTaskGroupList(machineUserContext);
                List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> maintenanceTaskGroupSearchParams = new List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>>();
                maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                jobTaskGroupDTOListOnDisplay = jobTaskGroupListt.GetAllJobTaskGroups(maintenanceTaskGroupSearchParams);
                if (jobTaskGroupDTOListOnDisplay == null)
                {
                    jobTaskGroupDTOListOnDisplay = new List<JobTaskGroupDTO>();
                }
                jobTaskGroupDTOListOnDisplay.Insert(0, new JobTaskGroupDTO());
                jobTaskGroupDTOListOnDisplay[0].TaskGroupName = "None";
                maintTaskGroupIdDataGridViewTextBoxColumn.DataSource = jobTaskGroupDTOListOnDisplay;
                maintTaskGroupIdDataGridViewTextBoxColumn.ValueMember = "JobTaskGroupId";
                maintTaskGroupIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                maintTaskGroupIdDataGridViewTextBoxColumn.DisplayMember = "TaskGroupName";

            }
            catch (Exception e)
            {
                log.Error("Ends-loadAsset() Method with exception: " + e.Message + e.StackTrace);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Function loads the records to the grid.
        /// </summary>
        private void PopulateScheduleGrid()
        {
            log.LogMethodEntry();
            try
            { 
                JobScheduleTasksListBL scheduleAssetTaskList = new JobScheduleTasksListBL(machineUserContext);
                List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>> scheduleAssetTaskSearchParams = new List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>>();
                //scheduleAssetTaskSearchParams.Add(new KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>(ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ACTIVE_FLAG, (chbActive.Checked) ? "Y" : "N"));
                scheduleAssetTaskSearchParams.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                scheduleAssetTaskSearchParams.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_SCHEDULE_ID, jobScheduleDTO.JobScheduleId.ToString()));
                List<JobScheduleTasksDTO> scheduleAssetTaskListOnDisplay = scheduleAssetTaskList.GetAllJobScheduleTaskDTOList(scheduleAssetTaskSearchParams);
                scheduleAssetTaskListBS = new BindingSource();
                if (scheduleAssetTaskListOnDisplay != null)
                {
                    SortableBindingList<JobScheduleTasksDTO> scheduleAssetTaskDTOSortList = new SortableBindingList<JobScheduleTasksDTO>(scheduleAssetTaskListOnDisplay);
                    scheduleAssetTaskListBS.DataSource = scheduleAssetTaskDTOSortList;
                }
                else
                    scheduleAssetTaskListBS.DataSource = new SortableBindingList<JobScheduleTasksDTO>();
                scheduleAssetTaskListBS.AddingNew += scheduleDataGridView_BindingSourceAddNew;
                scheduleDataGridView.DataSource = scheduleAssetTaskListBS;
                scheduleDataGridView.DataError += new DataGridViewDataErrorEventHandler(scheduleDataGridView_ComboDataError); 
            }
            catch (Exception e)
            {
                log.Error("Ends-PopulateScheduleGrid() Method with exception: " + e.Message + e.StackTrace);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the saved data to assignedto and Days
        /// </summary>
        private void GetMaintSchedule()
        {
            log.LogMethodEntry();
            try
            { 
                JobScheduleListBL maintenanceScheduleList = new JobScheduleListBL(machineUserContext);
                List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>> maintenanceScheduleSearchParams = new List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>>();
                if (chbActive.Checked)
                {
                    maintenanceScheduleSearchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.IS_ACTIVE,  "1" ));
                }
                maintenanceScheduleSearchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                maintenanceScheduleSearchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.SCHEDULE_ID, scheduleDTO.ScheduleId.ToString()));
                List<JobScheduleDTO> maintenanceScheduleListOnDisplay = maintenanceScheduleList.GetAllJobScheduleDTOList(maintenanceScheduleSearchParams);
                if (maintenanceScheduleListOnDisplay != null)
                {
                    jobScheduleDTO = maintenanceScheduleListOnDisplay[0];
                }
                cmbAssignedTo.SelectedValue = jobScheduleDTO.UserId;
                txtCompleted.Text = jobScheduleDTO.DurationToComplete == -1 ? "" : jobScheduleDTO.DurationToComplete.ToString(); 
            }
            catch (Exception e)
            {
                log.Error("Ends-loadMaintSchedule() Method with an Exception:" + e.Message + e.StackTrace);
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
                BindingSource assignedToBS;
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<UsersDTO> usersDTOList = usersList.GetAllUsers(usersSearchParams);
                if (usersDTOList == null)
                {
                    usersDTOList = new List<UsersDTO>();
                }
                assignedToBS = new BindingSource();
                usersDTOList.Insert(0, new UsersDTO());
                assignedToBS.DataSource = usersDTOList;
                cmbAssignedTo.DataSource = usersDTOList;
                cmbAssignedTo.DisplayMember = "UserName";
                cmbAssignedTo.ValueMember = "UserId"; 
            }
            catch (Exception e)
            {
                log.Error("Ends-loadAssignedTo() Method with an Exception:" + e.Message + e.StackTrace);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads saved schedules data
        /// </summary>
        private void GetSchedule()
        {
            log.LogMethodEntry();
            try
            { 
                txtName.Text = scheduleDTO.ScheduleName;
                chbActive.Checked = scheduleDTO.IsActive;
                dtscheduleDate.Value = (scheduleDTO.ScheduleTime.Equals(DateTime.MinValue)) ? dtscheduleDate.Value : scheduleDTO.ScheduleTime;
                cmbTime.Text = scheduleDTO.ScheduleTime.ToString("h:mm tt");
                chbRecur.Checked = scheduleDTO.RecurFlag.Equals("Y");
                dtpEndDate.Value = (scheduleDTO.RecurEndDate.Equals(DateTime.MinValue)) ? dtpEndDate.Value : scheduleDTO.RecurEndDate;
                switch (scheduleDTO.RecurFrequency)
                {
                    case "D": rbDaily.Checked = true;
                        break;
                    case "W": rbWeekly.Checked = true;
                        break;
                    case "M": rbMonthly.Checked = true;
                        break;
                    default:
                        if (chbRecur.Checked)
                            rbDaily.Checked = true;
                        else
                        {
                            rbDaily.Checked =
                            rbWeekly.Checked =
                            rbMonthly.Checked = false;
                        }
                        break;
                }
                switch (scheduleDTO.RecurType)
                {
                    case "D": rbRecurTypeDaily.Checked = true;
                        break;
                    case "W": rbRecurTypeWeekly.Checked = true;
                        break;
                    default:
                        if (chbRecur.Checked && rbMonthly.Checked)
                            rbRecurTypeDaily.Checked = true;
                        else
                        {
                            rbRecurTypeDaily.Checked =
                            rbRecurTypeWeekly.Checked = false;
                        }
                        break;
                } 
            }
            catch (Exception e)
            {
                log.Error("Ends-loadSchedule() Method with an Exception:" + e.Message + e.StackTrace);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// sets Time to the Time combobox
        /// </summary>
        private void SetTime()
        {
            log.LogMethodEntry();
            try
            { 
                string time;
                int hour;
                int mins;
                string ampm;

                cmbTime.DisplayMember = "Text";
                cmbTime.ValueMember = "Value";
                for (int i = 0; i < 48; i++)
                {
                    hour = i / 2;
                    mins = (i % 2) * 30;

                    if (hour >= 12)
                        ampm = "PM";
                    else
                        ampm = "AM";

                    if (hour == 0)
                        hour = 12;
                    if (hour > 12)
                        hour = hour - 12;

                    time = hour.ToString() + ":" + mins.ToString().PadLeft(2, '0') + " " + ampm;
                    cmbTime.Items.Add(time);
                    cmbTime.SelectedIndex = 0;
                } 
            }
            catch (Exception e)
            {
                log.Error("Ends-setTime() Method with an Exception:" + e.Message + e.StackTrace);
            }
            log.LogMethodExit();
        }

        private void scheduleDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show("Error in grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + scheduleDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }
        private void scheduleDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].Value.Equals(-1) && (!scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].Value.Equals(-1) || !scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].Value.Equals(-1)))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].ReadOnly = true;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].ReadOnly = false;
                }

            }
            else if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].Value.Equals(-1) && (!scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].Value.Equals(-1) || !scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].Value.Equals(-1)))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].ReadOnly = true;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].ReadOnly = false;
                }
            }
            else if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].Value.Equals(-1) && (!scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].Value.Equals(-1) || !scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].Value.Equals(-1)))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].ReadOnly = true;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].ReadOnly = false;
                }
            }
            else if (scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].Value.Equals(-1) && !scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskIdDataGridViewTextBoxColumn"].Value.Equals(-1))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].ReadOnly = true;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].ReadOnly = false;
                }
            }
            else if (scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskIdDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskIdDataGridViewTextBoxColumn"].Value.Equals(-1) && !scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].Value.Equals(-1))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].ReadOnly = true;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].ReadOnly = false;
                }
            }
            log.LogMethodExit();
        }
        private void scheduleDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].Value.Equals(-1))// && (!scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].Value.Equals(-1) || !scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].Value.Equals(-1))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].ReadOnly =
                        scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].ReadOnly = false;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].ReadOnly =
                        scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].ReadOnly = true;
                }

            }
            else if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].Value.Equals(-1))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].ReadOnly =
                        scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].ReadOnly = false;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].ReadOnly =
                        scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].ReadOnly = true;
                }
            }
            else if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["assetIDDataGridViewTextBoxColumn"].Value.Equals(-1))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].ReadOnly =
                        scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].ReadOnly = false;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["assetTypeIdDataGridViewTextBoxColumn"].ReadOnly =
                        scheduleDataGridView.Rows[e.RowIndex].Cells["assetGroupIdDataGridViewTextBoxColumn"].ReadOnly = true;
                }
            }
            else if (scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].Value.Equals(-1))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskIdDataGridViewTextBoxColumn"].ReadOnly = false;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskIdDataGridViewTextBoxColumn"].ReadOnly = true;
                }
            }
            else if (scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskIdDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
            {
                if (scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskIdDataGridViewTextBoxColumn"].Value.Equals(-1))
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].ReadOnly = false;
                }
                else
                {
                    scheduleDataGridView.Rows[e.RowIndex].Cells["maintTaskGroupIdDataGridViewTextBoxColumn"].ReadOnly = true;
                }
            }
            log.LogMethodExit();
        }
        private void scheduleDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (this.scheduleDataGridView.SelectedRows.Count <= 0 && this.scheduleDataGridView.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-assetDeleteBtn_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.scheduleDataGridView.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.scheduleDataGridView.SelectedCells)
                    {
                        scheduleDataGridView.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow assetRow in this.scheduleDataGridView.SelectedRows)
                {
                    if (assetRow.Cells[0].Value == null)
                    {
                        return;
                    }
                    if (Convert.ToInt32(assetRow.Cells[0].Value.ToString()) <= 0)
                    {
                        scheduleDataGridView.Rows.RemoveAt(assetRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            BindingSource scheduleAssetTaskDTOListDTOBS = (BindingSource)scheduleDataGridView.DataSource;
                            var scheduleAssetTaskDTOList = (SortableBindingList<JobScheduleTasksDTO>)scheduleAssetTaskDTOListDTOBS.DataSource;
                            JobScheduleTasksDTO scheduleAssetTaskDTO = scheduleAssetTaskDTOList[assetRow.Index];
                            scheduleAssetTaskDTO.IsActive = false;
                            JobScheduleTasksBL scheduleAssetTask = new JobScheduleTasksBL(machineUserContext, scheduleAssetTaskDTO);
                            scheduleAssetTask.Save(null);
                        }
                    }
                }
                if (rowsDeleted == true)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (scheduleDTO != null)
                {
                    if (scheduleDTO.ScheduleId > 0)
                    {
                        PublishUI publishUI = new PublishUI(utilities, scheduleDTO.ScheduleId, "MaintanaceSchedule", scheduleDTO.ScheduleName);
                        publishUI.ShowDialog();
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Shedule Publish");
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ScheduleUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnPublishToSite.Size = new Size(127, 23);
            log.LogMethodExit();
        } 
        
    }
}

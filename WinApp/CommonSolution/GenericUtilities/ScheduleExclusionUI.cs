/********************************************************************************************
 * Project Name - Schedule Exclusion UI
 * Description  - User interface for Schedule Exclusion
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        12-Jan-2016   Raghuveera          Created 
 *2.70        08-Mar-2019   Guru S A            Updated for renamed classes in maintenance module
 *2.80        28-Jun-2020   Deeksha             Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Schedule Exclusion UI
    /// </summary>
    public partial class ScheduleExclusionUI : Form
    {
        Semnox.Core.Utilities.Utilities utilities;
        BindingSource scheduleExclusionListBS;
        ScheduleCalendarDTO scheduleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext;
        private ManagementStudioSwitch managementStudioSwitch;
        public ScheduleExclusionUI(ScheduleCalendarDTO _ScheduleDTO, Semnox.Core.Utilities.Utilities _Utilities)
        {
            log.LogMethodEntry(_ScheduleDTO, _Utilities);
            InitializeComponent();
            utilities = _Utilities;
            _Utilities.setupDataGridProperties(ref scheduleExclusionDataGridView);
            scheduleDTO = _ScheduleDTO;
            machineUserContext = ExecutionContext.GetExecutionContext();
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
            if (scheduleDTO != null)
            {
                txtScheduleName.Text = scheduleDTO.ScheduleName;
            }
            LoadDay();
            PopulateScheduleExclusionGrid();
            ThemeUtils.SetupVisuals(this);
            utilities.setLanguage(this);
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads day to the grid.
        /// </summary>
        private void LoadDay()
        {
            log.LogMethodEntry();
            var weekDays = new Dictionary<int, string>();
            weekDays[-1] = utilities.MessageUtils.getMessage("<SELECT>");
            weekDays[1] = utilities.MessageUtils.getMessage("Sunday");
            weekDays[2] = utilities.MessageUtils.getMessage("Monday");
            weekDays[3] = utilities.MessageUtils.getMessage("Tuesday");
            weekDays[4] = utilities.MessageUtils.getMessage("Wednesday");
            weekDays[5] = utilities.MessageUtils.getMessage("Thursday");
            weekDays[6] = utilities.MessageUtils.getMessage("Friday");
            weekDays[7] = utilities.MessageUtils.getMessage("Saturday");
            
            dayDataGridViewTextBoxColumn.DataSource = new BindingSource(weekDays, null);
            dayDataGridViewTextBoxColumn.DisplayMember = "Value";
            dayDataGridViewTextBoxColumn.ValueMember = "Key";
            log.LogMethodExit();
        }
        /// <summary>
        /// Function loads the records to the grid.
        /// </summary>
        private void PopulateScheduleExclusionGrid()
        {
            log.LogMethodEntry();
            ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(machineUserContext);
            List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>> scheduleExclusionSearchParams = new List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>>();
            scheduleExclusionSearchParams.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SCHEDULE_ID, (scheduleDTO == null) ? "-1" : scheduleDTO.ScheduleId.ToString()));
            List<ScheduleCalendarExclusionDTO> scheduleExclusionListOnDisplay = scheduleExclusionList.GetAllScheduleExclusions(scheduleExclusionSearchParams);
            scheduleExclusionListBS = new BindingSource();
            if (scheduleExclusionListOnDisplay != null)
            {
                for (int i = 0; i < scheduleExclusionListOnDisplay.Count;i++)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(scheduleExclusionListOnDisplay[i].ExclusionDate))
                        {
                            scheduleExclusionListOnDisplay[i].ExclusionDate = DateTime.Parse(scheduleExclusionListOnDisplay[i].ExclusionDate).ToString(utilities.getDateTimeFormat());
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                SortableBindingList<ScheduleCalendarExclusionDTO> scheduleExclusionDTOSortList = new SortableBindingList<ScheduleCalendarExclusionDTO>(scheduleExclusionListOnDisplay);
                scheduleExclusionListBS.DataSource = scheduleExclusionDTOSortList;
            }
            else
                scheduleExclusionListBS.DataSource = new SortableBindingList<ScheduleCalendarExclusionDTO>();
            scheduleExclusionListBS.AddingNew += scheduleExclusionDataGridView_BindingSourceAddNew;
            scheduleExclusionDataGridView.DataSource = scheduleExclusionListBS;
            scheduleIdDataGridViewTextBoxColumn.Visible = false;
            log.LogMethodExit();
        }
        void scheduleExclusionDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scheduleExclusionDataGridView.Rows.Count == scheduleExclusionListBS.Count)
            {
                scheduleExclusionListBS.RemoveAt(scheduleExclusionListBS.Count - 1);
            }
            log.LogMethodExit();
        }

        private void scheduleExclusionRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateScheduleExclusionGrid();
            log.LogMethodExit();

        }
        private void scheduleExclusionCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
        private void scheduleExclusionDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.scheduleExclusionDataGridView.SelectedRows.Count <= 0 && this.scheduleExclusionDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit(utilities.MessageUtils.getMessage(959));
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.scheduleExclusionDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.scheduleExclusionDataGridView.SelectedCells)
                {
                    scheduleExclusionDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow scheduleExclusionRow in this.scheduleExclusionDataGridView.SelectedRows)
            {
                if (scheduleExclusionRow.Cells[0].Value == null)
                {
                    log.LogMethodExit("scheduleExclusionRow.Cells[0].Value == null");
                    return;
                }
                if (Convert.ToInt32(scheduleExclusionRow.Cells[0].Value.ToString()) <= 0)
                {
                    scheduleExclusionDataGridView.Rows.RemoveAt(scheduleExclusionRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource scheduleExclusionDTOListDTOBS = (BindingSource)scheduleExclusionDataGridView.DataSource;
                        var scheduleExclusionDTOList = (SortableBindingList<ScheduleCalendarExclusionDTO>)scheduleExclusionDTOListDTOBS.DataSource;
                        ScheduleCalendarExclusionDTO scheduleExclusionDTO = scheduleExclusionDTOList[scheduleExclusionRow.Index];
                        scheduleExclusionDTO.IsActive = false;
                        ScheduleCalendarExclusionBL scheduleExclusion = new ScheduleCalendarExclusionBL(machineUserContext, scheduleExclusionDTO);
                        scheduleExclusion.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));

            PopulateScheduleExclusionGrid();
            log.LogMethodExit();
        }
        private void scheduleExclusionSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            { 
                BindingSource scheduleExclusionListBS = (BindingSource)scheduleExclusionDataGridView.DataSource;
                var scheduleExclusionListOnDisplay = (SortableBindingList<ScheduleCalendarExclusionDTO>)scheduleExclusionListBS.DataSource;
                if (scheduleExclusionListOnDisplay.Count > 0)
                {
                    foreach (ScheduleCalendarExclusionDTO scheduleExclusionDTO in scheduleExclusionListOnDisplay)
                    {
                        if (string.IsNullOrEmpty(scheduleExclusionDTO.ExclusionDate) && scheduleExclusionDTO.Day == -1)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(990)); log.LogMethodExit(utilities.MessageUtils.getMessage(990));
                            log.LogMethodExit(utilities.MessageUtils.getMessage(990));
                            return;
                        }
                        else if(!string.IsNullOrEmpty(scheduleExclusionDTO.ExclusionDate))
                        {
                            try
                            {
                                DateTime.Parse(scheduleExclusionDTO.ExclusionDate);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                MessageBox.Show(utilities.MessageUtils.getMessage(990));
                                log.LogMethodExit(utilities.MessageUtils.getMessage(990));
                                return;
                            }
                        }
                        ScheduleCalendarExclusionBL scheduleExclusion = new ScheduleCalendarExclusionBL(machineUserContext, scheduleExclusionDTO);
                        scheduleExclusion.Save();
                    }
                    PopulateScheduleExclusionGrid();
                    
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));

                log.LogMethodExit();
            }
            catch (Exception e1)
            {
                log.Error(e1);
                MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + ": " + e1.Message);
            }
        }
        private void scheduleExclusionDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Row.Cells["scheduleIdDataGridViewTextBoxColumn"].Value = scheduleDTO.ScheduleId;
            log.LogMethodExit();
        }
        private void scheduleExclusionDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ", " + utilities.MessageUtils.getMessage("Column") + " " + scheduleExclusionDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                scheduleExclusionDataGridView.AllowUserToAddRows = true;
                scheduleExclusionDataGridView.ReadOnly = false;
                scheduleExclusionSaveBtn.Enabled = true;
                scheduleExclusionDeleteBtn.Enabled = true;
            }
            else
            {
                scheduleExclusionDataGridView.AllowUserToAddRows = false;
                scheduleExclusionDataGridView.ReadOnly = true;
                scheduleExclusionSaveBtn.Enabled = false;
                scheduleExclusionDeleteBtn.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}

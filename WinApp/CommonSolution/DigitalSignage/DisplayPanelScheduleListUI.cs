/********************************************************************************************
 * Project Name - DisplayPanelScheduleListUI
 * Description  - UI for display panel schedules
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70       23-Apr-2019   Guru S A       updates due to renamed classes for maint schedule module
 *           29-May-2019   Jagan Mohan    Code merge from Development to WebManagementStudio
 *2.80       17-Feb-2019   Deeksha        Modified to Make DigitalSignage module as
 *                                        read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// DisplayPanelScheduleList UI
    /// </summary>
    public partial class DisplayPanelScheduleListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Constructor of DisplayPanelScheduleListUI class.
        /// </summary>
        /// <param name="utilities"></param>
        public DisplayPanelScheduleListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvDisplayPanelScheduleList);
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
            lnkPublish.Visible = false;
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublish.Visible = true;
            }
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void DisplayPanelScheduleListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cbPanel.DropDownStyle = ComboBoxStyle.DropDownList;
            dtpSchedule.Value = DateTime.Today;
            LoadDisplayPanelDTOList();
            LoadRecurFrequencyAndRecurType();
            LoadScheduleList();
            log.LogMethodExit();
        }

        private void LoadRecurFrequencyAndRecurType()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> recurFrequecyList = new List<KeyValuePair<string, string>>();
            recurFrequecyList.Add(new KeyValuePair<string, string>("", ""));
            recurFrequecyList.Add(new KeyValuePair<string, string>("D", utilities.MessageUtils.getMessage(10195)));
            recurFrequecyList.Add(new KeyValuePair<string, string>("W", utilities.MessageUtils.getMessage(10424)));
            recurFrequecyList.Add(new KeyValuePair<string, string>("M", utilities.MessageUtils.getMessage(11293)));
            recurFrequencyDataGridViewComboBoxColumn.DataSource = recurFrequecyList;
            recurFrequencyDataGridViewComboBoxColumn.ValueMember = "Key";
            recurFrequencyDataGridViewComboBoxColumn.DisplayMember = "Value";

            List<KeyValuePair<string, string>> recurTypeList = new List<KeyValuePair<string, string>>();
            recurTypeList.Add(new KeyValuePair<string, string>("", ""));
            recurTypeList.Add(new KeyValuePair<string, string>("D", utilities.MessageUtils.getMessage(10841)));
            recurTypeList.Add(new KeyValuePair<string, string>("W", utilities.MessageUtils.getMessage(11886)));
            recurTypeDataGridViewComboBoxColumn.DataSource = recurTypeList;
            recurTypeDataGridViewComboBoxColumn.ValueMember = "Key";
            recurTypeDataGridViewComboBoxColumn.DisplayMember = "Value";
            log.LogMethodExit();
        }

        private void LoadDisplayPanelDTOList()
        {
            log.LogMethodEntry();
            DisplayPanelListBL displayPanelListBL = new DisplayPanelListBL(machineUserContext);
            List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<DisplayPanelDTO> displayPanelDTOList = displayPanelListBL.GetDisplayPanelDTOList(searchParameters);
            if (displayPanelDTOList == null)
            {
                displayPanelDTOList = new List<DisplayPanelDTO>();
            }
            displayPanelDTOList.Insert(0, new DisplayPanelDTO());
            displayPanelDTOList[0].PanelName = "<SELECT>";
            cbPanel.DataSource = displayPanelDTOList;
            cbPanel.ValueMember = "PanelId";
            cbPanel.DisplayMember = "PanelName";
            log.LogMethodExit();
        }

        private void LoadScheduleList()
        {
            log.LogMethodEntry();
            DisplayPanelThemeMapListBL displayPanelThemeMapListBL = new DisplayPanelThemeMapListBL(machineUserContext);
            List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if (cbPanel.SelectedItem != null && cbPanel.SelectedItem is DisplayPanelDTO)
            {
                if ((cbPanel.SelectedItem as DisplayPanelDTO).PanelId >= 0)
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.PANEL_ID, (cbPanel.SelectedItem as DisplayPanelDTO).PanelId.ToString()));
                }
            }
            List<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOList = displayPanelThemeMapListBL.GetDisplayPanelThemeMapDTOList(searchParameters);
            List<int> scheduleIdList = new List<int>();
            bool containsScheduleID;
            if (displayPanelThemeMapDTOList != null)
            {
                foreach (DisplayPanelThemeMapDTO displayPanelThemeMapDTO in displayPanelThemeMapDTOList)
                {
                    containsScheduleID = false;
                    foreach (int scheduleId in scheduleIdList)
                    {
                        if (scheduleId == displayPanelThemeMapDTO.ScheduleId)
                        {
                            containsScheduleID = true;
                        }
                    }
                    if (!containsScheduleID)
                    {
                        scheduleIdList.Add(displayPanelThemeMapDTO.ScheduleId);
                    }
                }
            }
            ScheduleCalendarListBL scheduleListBL = new ScheduleCalendarListBL(machineUserContext);
            List<ScheduleCalendarDTO> scheduleDTOList = new List<ScheduleCalendarDTO>();
            foreach (int scheduleId in scheduleIdList)
            {
                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchScheduleParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, scheduleId.ToString()));
                List<ScheduleCalendarDTO> list = scheduleListBL.GetAllSchedule(searchScheduleParameters);
                if (list != null && list.Count > 0)
                {
                    scheduleDTOList.Add(list[0]);
                }
            }
            SortableBindingList<ScheduleCalendarDTO> scheduleDTOSortableBindingList = new SortableBindingList<ScheduleCalendarDTO>();
            Boolean show = true;
            foreach (ScheduleCalendarDTO scheduleDTO in scheduleDTOList)
            {
                show = true;
                if (!string.IsNullOrEmpty(txtScheduleName.Text) && !string.IsNullOrWhiteSpace(txtScheduleName.Text))
                {
                    if (string.IsNullOrEmpty(scheduleDTO.ScheduleName) || !scheduleDTO.ScheduleName.Contains(txtScheduleName.Text))
                    {
                        show = false;
                    }
                }
                if (DateTime.Compare(new DateTime(scheduleDTO.ScheduleTime.Year, scheduleDTO.ScheduleTime.Month, scheduleDTO.ScheduleTime.Day), new DateTime(dtpSchedule.Value.Year, dtpSchedule.Value.Month, dtpSchedule.Value.Day)) > 0)
                {
                    show = false;
                }
                if (string.Equals(scheduleDTO.RecurFlag, "Y"))
                {
                    if (DateTime.Compare(new DateTime(scheduleDTO.RecurEndDate.Year, scheduleDTO.RecurEndDate.Month, scheduleDTO.RecurEndDate.Day), new DateTime(dtpSchedule.Value.Year, dtpSchedule.Value.Month, dtpSchedule.Value.Day)) < 0)
                    {
                        show = false;
                    }
                }
                else
                {
                    if (scheduleDTO.ScheduleEndDate.Date != DateTime.MinValue)
                    {
                        if (DateTime.Compare(new DateTime(scheduleDTO.ScheduleEndDate.Year, scheduleDTO.ScheduleEndDate.Month, scheduleDTO.ScheduleEndDate.Day), new DateTime(dtpSchedule.Value.Year, dtpSchedule.Value.Month, dtpSchedule.Value.Day)) < 0)
                        {
                            show = false;
                        }
                    }
                }
                if (chbShowActiveEntries.Checked && !scheduleDTO.IsActive)
                {
                    show = false;
                }
                if (show)
                {
                    scheduleDTOSortableBindingList.Add(scheduleDTO);
                }
            }
            scheduleDTOListBS.DataSource = scheduleDTOSortableBindingList;
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                LoadScheduleList();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                chbShowActiveEntries.Checked = true;
                txtScheduleName.ResetText();
                dtpSchedule.Value = DateTime.Today;
                cbPanel.SelectedIndex = 0;
                LoadDisplayPanelDTOList();
                LoadScheduleList();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ScheduleCalendarDTO scheduleDTO = new ScheduleCalendarDTO
                {
                    ScheduleId = -1,
                    ScheduleTime = DateTime.Now,
                    ScheduleEndDate = DateTime.Now,
                    RecurFlag = "N",
                    IsActive = true,
                    RecurEndDate = DateTime.MinValue
                };

                ShowDisplayPanelThemeMapListUI(scheduleDTO);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvDisplayPanelScheduleList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (scheduleDTOListBS.Current != null && scheduleDTOListBS.Current is ScheduleCalendarDTO)
                {
                    if ((scheduleDTOListBS.Current as ScheduleCalendarDTO).ScheduleId != -1)
                    {
                        ShowDisplayPanelThemeMapListUI(scheduleDTOListBS.Current as ScheduleCalendarDTO);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ShowDisplayPanelThemeMapListUI(ScheduleCalendarDTO scheduleDTO)
        {
            log.LogMethodEntry(scheduleDTO);
            DisplayPanelThemeMapListUI displayPanelThemeMapListUI = new DisplayPanelThemeMapListUI(utilities, scheduleDTO);
            displayPanelThemeMapListUI.ShowDialog();
            LoadScheduleList();
            log.LogMethodExit();
        }

        private void dgvDisplayPanelScheduleList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvDisplayPanelScheduleList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDisplayPanelScheduleList.CurrentRow.Cells["scheduleIdDataGridViewTextBoxColumn"].Value != null)
                {
                    int id = Convert.ToInt32(dgvDisplayPanelScheduleList.CurrentRow.Cells["scheduleIdDataGridViewTextBoxColumn"].Value);

                    if (id <= 0)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    PublishUI publishUI = new PublishUI(utilities, id, "Schedule", dgvDisplayPanelScheduleList.CurrentRow.Cells["scheduleNameDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }
        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvDisplayPanelScheduleList.AllowUserToAddRows = true;
                dgvDisplayPanelScheduleList.ReadOnly = false;
                btnNew.Enabled = true;
            }
            else
            {
                dgvDisplayPanelScheduleList.AllowUserToAddRows = false;
                dgvDisplayPanelScheduleList.ReadOnly = true;
                btnNew.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}

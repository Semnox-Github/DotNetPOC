/********************************************************************************************
 * Project Name - DisplayPanelThemeMapListUI
 * Description  - UI for display panel theme map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        23-Apr-2019   Guru S A       updates due to renamed classes for maint schedule module
 *2.70.2      14-Aug-2019   Dakshakh       Added logger methods
 *2.80.0      17-Feb-2019   Deeksha        Modified to Make DigitalSignage module as
 *                                         read only in Windows Management Studio.
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// DisplayPanelThemeMapList UI
    /// </summary>
    public partial class DisplayPanelThemeMapListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private ScheduleCalendarDTO scheduleDTO;
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Constructor of DisplayPanelThemeMapListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="scheduleDTO">ScheduleDTO</param>
        public DisplayPanelThemeMapListUI(Utilities utilities, ScheduleCalendarDTO scheduleDTO)
        {
            log.LogMethodEntry(utilities, scheduleDTO);
            InitializeComponent();
            this.utilities = utilities;
            this.scheduleDTO = scheduleDTO;
            utilities.setupDataGridProperties(ref dgvDisplayPanelThemeMapDTOList);
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
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void DisplayPanelThemeMapListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cbPanel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            if (scheduleDTO != null)
            {
                txtScheduleName.MaxLength = 100;
                rdbDaily.Enabled = false;
                rdbMonthly.Enabled = false;
                rdbWeekly.Enabled = false;
                rdbDay.Enabled = false;
                rdbWeekDay.Enabled = false;
                dtpRecurEndDate.Enabled = false;

                chbActive.Checked = false;
                chbRecurFlag.Checked = false;
                rdbDaily.Checked = false;
                rdbMonthly.Checked = false;
                rdbWeekly.Checked = false;
                rdbDay.Checked = false;
                rdbWeekDay.Checked = false;

                txtScheduleName.Text = scheduleDTO.ScheduleName;
                dtpScheduleDate.Value = scheduleDTO.ScheduleTime;
                dtpScheduleTime.Value = scheduleDTO.ScheduleTime;
                dtpScheduleEndTime.MinDate = scheduleDTO.ScheduleTime.Date;
                if (scheduleDTO.ScheduleEndDate == DateTime.MinValue)
                {
                    dtpScheduleEndDate.Value = dtpScheduleEndDate.MinDate;
                    dtpScheduleEndTime.Value = dtpScheduleEndDate.MinDate;
                }
                else
                {
                    dtpScheduleEndDate.Value = scheduleDTO.ScheduleEndDate;
                    dtpScheduleEndTime.Value = scheduleDTO.ScheduleEndDate;
                }
                if (scheduleDTO.RecurEndDate == DateTime.MinValue)
                {
                    dtpRecurEndDate.Value = dtpRecurEndDate.MinDate;
                }
                else
                {
                    dtpRecurEndDate.Value = scheduleDTO.RecurEndDate;
                }
                if (scheduleDTO.IsActive)
                {
                    chbActive.Checked = true;
                }

                if (!string.IsNullOrEmpty(scheduleDTO.RecurFlag) && string.Equals(scheduleDTO.RecurFlag, "Y"))
                {
                    chbRecurFlag.Checked = true;
                    rdbDaily.Enabled = true;
                    rdbMonthly.Enabled = true;
                    rdbWeekly.Enabled = true;
                    dtpRecurEndDate.Enabled = true;
                }
                switch (scheduleDTO.RecurFrequency)
                {
                    case "D":
                        {
                            rdbDaily.Checked = true;
                            dtpScheduleEndDate.Enabled = false;
                            dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleTime.Year,
                                                                    scheduleDTO.ScheduleTime.Month,
                                                                    scheduleDTO.ScheduleTime.Day);
                            break;
                        }
                    case "W":
                        {
                            rdbWeekly.Checked = true;
                            dtpScheduleEndDate.Enabled = true;
                            dtpScheduleEndDate.MaxDate = dtpScheduleDate.Value.AddDays(6);
                            break;
                        }
                    case "M":
                        {
                            rdbMonthly.Checked = true;
                            rdbDay.Enabled = true;
                            rdbWeekDay.Enabled = true;
                            dtpScheduleEndDate.Enabled = false;
                            dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleTime.Year,
                                                                    scheduleDTO.ScheduleTime.Month,
                                                                    scheduleDTO.ScheduleTime.Day);
                            break;
                        }
                }

                switch (scheduleDTO.RecurType)
                {
                    case "D":
                        {
                            rdbDay.Checked = true;
                            break;
                        }
                    case "W":
                        {
                            rdbWeekDay.Checked = true;
                            break;
                        }
                }

            }
            LoadThemeDTOList();
            LoadDisplayPanelDTOList();
            RefreshData();
            log.LogMethodExit();
        }

        private void chbRecurFlag_CheckStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (chbRecurFlag.Checked)
            {
                rdbDaily.Enabled = true;
                rdbMonthly.Enabled = true;
                rdbWeekly.Enabled = true;
                rdbDay.Enabled = true;
                rdbWeekDay.Enabled = true;
                dtpRecurEndDate.Enabled = true;
                if (dtpRecurEndDate.Value == dtpRecurEndDate.MinDate)
                {
                    dtpRecurEndDate.Value = DateTime.Today;
                }
                rdbDaily.Checked = true;
                dtpScheduleEndDate.Enabled = false;
                dtpScheduleEndDate.Value = dtpScheduleDate.Value;
            }
            else
            {
                rdbDaily.Enabled = false;
                rdbMonthly.Enabled = false;
                rdbWeekly.Enabled = false;
                rdbDay.Enabled = false;
                rdbWeekDay.Enabled = false;
                dtpRecurEndDate.Enabled = false;
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;

                rdbDaily.Checked = false;
                rdbMonthly.Checked = false;
                rdbWeekly.Checked = false;
                rdbDay.Checked = false;
                rdbWeekDay.Checked = false;
            }
            log.LogMethodExit();
        }

        private void LoadThemeDTOList()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "THEME_TYPE"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Panel"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<LookupValuesDTO> themeTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            int themeTypeId = -1;
            if (themeTypeLookUpValueList != null && themeTypeLookUpValueList.Count > 0)
            {
                themeTypeId = themeTypeLookUpValueList[0].LookupValueId;
            }
            ThemeListBL themeListBL = new ThemeListBL(machineUserContext);
            List<KeyValuePair<ThemeDTO.SearchByParameters, string>> themeSearchParams = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
            themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_ID, themeTypeId.ToString()));
            themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<ThemeDTO> themeDTOList = themeListBL.GetThemeDTOList(themeSearchParams, true, true);
            if (themeDTOList == null)
            {
                themeDTOList = new List<ThemeDTO>();
            }
            themeDTOList.Insert(0, new ThemeDTO());
            themeDTOList[0].Name = "<SELECT>";
            BindingSource bs = new BindingSource();
            bs.DataSource = themeDTOList;
            dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.DataSource = bs;
            dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.ValueMember = "Id";
            dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.DisplayMember = "Name";

            bs = new BindingSource();
            bs.DataSource = themeDTOList;
            cbTheme.DataSource = bs;
            cbTheme.ValueMember = "Id";
            cbTheme.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private void LoadDisplayPanelDTOList()
        {
            log.LogMethodEntry();
            DisplayPanelListBL displayPanelListBL = new DisplayPanelListBL(machineUserContext);
            List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<DisplayPanelDTO> displayPanelDTOList = displayPanelListBL.GetDisplayPanelDTOList(searchParams);
            if (displayPanelDTOList == null)
            {
                displayPanelDTOList = new List<DisplayPanelDTO>();
            }
            displayPanelDTOList.Insert(0, new DisplayPanelDTO());
            displayPanelDTOList[0].PanelName = "<SELECT>";

            BindingSource bs = new BindingSource();
            bs.DataSource = displayPanelDTOList;
            dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.DataSource = bs;
            dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.ValueMember = "PanelId";
            dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.DisplayMember = "PanelName";

            bs = new BindingSource();
            bs.DataSource = displayPanelDTOList;
            cbPanel.DataSource = bs;
            cbPanel.ValueMember = "PanelId";
            cbPanel.DisplayMember = "PanelName";
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadDisplayPanelThemeMapDTOList();
            log.LogMethodExit();
        }

        private void LoadDisplayPanelThemeMapDTOList()
        {
            log.LogMethodEntry();
            DisplayPanelThemeMapListBL displayPanelThemeMapListBL = new DisplayPanelThemeMapListBL(machineUserContext);
            List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if (chbShowActiveEntries.Checked)
            {
                searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SCHEDULE_ID, scheduleDTO.ScheduleId.ToString()));
            if (cbTheme.SelectedItem != null && cbTheme.SelectedItem is ThemeDTO && (cbTheme.SelectedItem as ThemeDTO).Id >= 0)
            {
                searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.THEME_ID, (cbTheme.SelectedItem as ThemeDTO).Id.ToString()));
            }
            if (cbPanel.SelectedItem != null && cbPanel.SelectedItem is DisplayPanelDTO && (cbPanel.SelectedItem as DisplayPanelDTO).PanelId >= 0)
            {
                searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.PANEL_ID, (cbPanel.SelectedItem as DisplayPanelDTO).PanelId.ToString()));
            }
            List<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOList = displayPanelThemeMapListBL.GetDisplayPanelThemeMapDTOList(searchParameters);
            SortableBindingList<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOSortableList;
            if (displayPanelThemeMapDTOList != null)
            {
                displayPanelThemeMapDTOSortableList = new SortableBindingList<DisplayPanelThemeMapDTO>(displayPanelThemeMapDTOList);
            }
            else
            {
                displayPanelThemeMapDTOSortableList = new SortableBindingList<DisplayPanelThemeMapDTO>();
            }
            displayPanelThemeMapDTOListBS.DataSource = displayPanelThemeMapDTOSortableList;
            log.LogMethodExit();
        }

        private void dgvDisplayPanelThemeMapDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                bool handled = false;
                if (e.ColumnIndex == dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.Index ||
                   e.ColumnIndex == dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.Index)
                {
                    SortableBindingList<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOSortableList = null;
                    if (displayPanelThemeMapDTOListBS.DataSource is SortableBindingList<DisplayPanelThemeMapDTO>)
                    {
                        displayPanelThemeMapDTOSortableList = (SortableBindingList<DisplayPanelThemeMapDTO>)displayPanelThemeMapDTOListBS.DataSource;
                    }
                    if (displayPanelThemeMapDTOSortableList != null && displayPanelThemeMapDTOSortableList.Count > 0)
                    {
                        DataGridViewComboBoxCell cell = dgvDisplayPanelThemeMapDTOList[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
                        if (cell != null)
                        {
                            cell.Value = -1;
                            handled = true;
                        }
                    }
                }
                if (handled == false)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvDisplayPanelThemeMapDTOList.Columns[e.ColumnIndex].HeaderText +
                      ": " + e.Exception.Message);
                }
                e.Cancel = true;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.OK;
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                chbShowActiveEntries.Checked = true;
                cbTheme.SelectedIndex = 0;
                cbPanel.SelectedIndex = 0;
                LoadThemeDTOList();
                LoadDisplayPanelDTOList();
                RefreshData();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvDisplayPanelThemeMapDTOList.EndEdit();
                SortableBindingList<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOSortableList = (SortableBindingList<DisplayPanelThemeMapDTO>)displayPanelThemeMapDTOListBS.DataSource;
                string message;
                DisplayPanelThemeMapBL displayPanelThemeMapBL;
                bool error = false;
                UpdateScheduleDTO();
                message = ValidateScheduleDTO(scheduleDTO);
                if (string.IsNullOrEmpty(message))
                {
                    if (scheduleDTO.IsChanged)
                    {
                        try
                        {
                            ScheduleCalendarBL scheduleBL = new ScheduleCalendarBL(machineUserContext, scheduleDTO);
                            scheduleBL.Save();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(994));
                            log.Error("Error while saving schedule", ex); 
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(message);
                    log.LogMethodExit(message);
                    return;
                }

                if (displayPanelThemeMapDTOSortableList != null)
                {
                    for (int i = 0; i < displayPanelThemeMapDTOSortableList.Count; i++)
                    {
                        if (displayPanelThemeMapDTOSortableList[i].IsChanged)
                        {
                            message = ValidateDisplayPanelThemeMapDTO(displayPanelThemeMapDTOSortableList[i]);
                            if (string.IsNullOrEmpty(message))
                            {
                                try
                                {
                                    displayPanelThemeMapDTOSortableList[i].ScheduleId = scheduleDTO.ScheduleId;
                                    displayPanelThemeMapBL = new DisplayPanelThemeMapBL(machineUserContext, displayPanelThemeMapDTOSortableList[i]);
                                    displayPanelThemeMapBL.Save();
                                }
                                catch (Exception)
                                {
                                    error = true;
                                    log.Error("Error while saving displayPanelThemeMap.");
                                    dgvDisplayPanelThemeMapDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                    break;
                                }
                            }
                            else
                            {
                                error = true;
                                dgvDisplayPanelThemeMapDTOList.Rows[i].Selected = true;
                                MessageBox.Show(message);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                }
                if (!error)
                {
                    RefreshData();
                }
                else
                {
                    dgvDisplayPanelThemeMapDTOList.Update();
                    dgvDisplayPanelThemeMapDTOList.Refresh();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void UpdateScheduleDTO()
        {
            log.LogMethodEntry();
            if (scheduleDTO.ScheduleName != txtScheduleName.Text)
            {
                scheduleDTO.ScheduleName = txtScheduleName.Text;
            }
            if (scheduleDTO.ScheduleTime.Day != dtpScheduleDate.Value.Day ||
                scheduleDTO.ScheduleTime.Month != dtpScheduleDate.Value.Month ||
                scheduleDTO.ScheduleTime.Year != dtpScheduleDate.Value.Year)
            {
                scheduleDTO.ScheduleTime = dtpScheduleDate.Value;
            }

            if (scheduleDTO.ScheduleTime.Hour != dtpScheduleTime.Value.Hour ||
               scheduleDTO.ScheduleTime.Minute != dtpScheduleTime.Value.Minute ||
               scheduleDTO.ScheduleTime.Second != dtpScheduleTime.Value.Second)
            {
                DateTime dateTime = new DateTime(scheduleDTO.ScheduleTime.Year, scheduleDTO.ScheduleTime.Month, scheduleDTO.ScheduleTime.Day, dtpScheduleTime.Value.Hour, dtpScheduleTime.Value.Minute, dtpScheduleTime.Value.Second);
                scheduleDTO.ScheduleTime = dateTime;
            }

            if (scheduleDTO.ScheduleEndDate.Day != dtpScheduleEndDate.Value.Day ||
                scheduleDTO.ScheduleEndDate.Month != dtpScheduleEndDate.Value.Month ||
                scheduleDTO.ScheduleEndDate.Year != dtpScheduleEndDate.Value.Year)
            {
                scheduleDTO.ScheduleEndDate = dtpScheduleEndDate.Value;
            }

            if (scheduleDTO.ScheduleEndDate.Hour != dtpScheduleEndTime.Value.Hour ||
               scheduleDTO.ScheduleEndDate.Minute != dtpScheduleEndTime.Value.Minute ||
               scheduleDTO.ScheduleEndDate.Second != dtpScheduleEndTime.Value.Second)
            {
                DateTime dateTime = new DateTime(scheduleDTO.ScheduleEndDate.Year, scheduleDTO.ScheduleEndDate.Month, scheduleDTO.ScheduleEndDate.Day, dtpScheduleEndTime.Value.Hour, dtpScheduleEndTime.Value.Minute, dtpScheduleEndTime.Value.Second);
                scheduleDTO.ScheduleEndDate = dateTime;
            }

            if (scheduleDTO.RecurFlag == "Y")
            {
                if (!chbRecurFlag.Checked)
                {
                    scheduleDTO.RecurFlag = "N";
                }
            }
            else
            {
                if (chbRecurFlag.Checked)
                {
                    scheduleDTO.RecurFlag = "Y";
                }
            }
            if (scheduleDTO.RecurFlag == "Y")
            {
                if (scheduleDTO.RecurEndDate.Day != dtpRecurEndDate.Value.Day ||
                   scheduleDTO.RecurEndDate.Month != dtpRecurEndDate.Value.Month ||
                   scheduleDTO.RecurEndDate.Year != dtpRecurEndDate.Value.Year)
                {
                    scheduleDTO.RecurEndDate = dtpRecurEndDate.Value;
                }
                switch (scheduleDTO.RecurFrequency)
                {
                    case "D":
                        {
                            if (rdbWeekly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "W";
                            }
                            else if (rdbMonthly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "M";
                            }
                            break;
                        }
                    case "W":
                        {
                            if (rdbDaily.Checked)
                            {
                                scheduleDTO.RecurFrequency = "D";
                            }
                            else if (rdbMonthly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "M";
                            }
                            break;
                        }
                    case "M":
                        {
                            if (rdbDaily.Checked)
                            {
                                scheduleDTO.RecurFrequency = "D";
                            }
                            else if (rdbWeekly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "W";
                            }
                            break;
                        }
                    default:
                        {
                            if (rdbDaily.Checked)
                            {
                                scheduleDTO.RecurFrequency = "D";
                            }
                            else if (rdbWeekly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "W";
                            }
                            else if (rdbMonthly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "M";
                            }
                            break;
                        }
                }
                if (string.Equals(scheduleDTO.RecurFrequency, "M"))
                {
                    switch (scheduleDTO.RecurType)
                    {
                        case "D":
                            {
                                if (rdbWeekDay.Checked)
                                {
                                    scheduleDTO.RecurType = "W";
                                }
                                break;
                            }
                        case "W":
                            {
                                if (rdbDay.Checked)
                                {
                                    scheduleDTO.RecurType = "D";
                                }
                                break;
                            }
                        default:
                            {
                                if (rdbWeekDay.Checked)
                                {
                                    scheduleDTO.RecurType = "W";
                                }
                                if (rdbDay.Checked)
                                {
                                    scheduleDTO.RecurType = "D";
                                }
                                break;
                            }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(scheduleDTO.RecurType))
                    {
                        scheduleDTO.RecurType = "";
                    }
                }
            }
            else
            {
                if (scheduleDTO.RecurEndDate != DateTime.MinValue)
                {
                    scheduleDTO.RecurEndDate = DateTime.MinValue;
                }
                if (!string.IsNullOrEmpty(scheduleDTO.RecurFrequency))
                {
                    scheduleDTO.RecurFrequency = "";
                }
                if (!string.IsNullOrEmpty(scheduleDTO.RecurType))
                {
                    scheduleDTO.RecurType = "";
                }
            }
            if (scheduleDTO.IsActive)
            {
                if (!chbActive.Checked)
                {
                    scheduleDTO.IsActive = false;
                }
            }
            else
            {
                if (chbActive.Checked)
                {
                    scheduleDTO.IsActive = true;
                }
            }
            log.LogMethodExit();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDisplayPanelThemeMapDTOList.SelectedRows.Count <= 0 && dgvDisplayPanelThemeMapDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                bool refreshFromDB = false;
                if (this.dgvDisplayPanelThemeMapDTOList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvDisplayPanelThemeMapDTOList.SelectedCells)
                    {
                        dgvDisplayPanelThemeMapDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow row in dgvDisplayPanelThemeMapDTOList.SelectedRows)
                {
                    if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                    {
                        dgvDisplayPanelThemeMapDTOList.Rows.RemoveAt(row.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            refreshFromDB = true;
                            SortableBindingList<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOSortableList = (SortableBindingList<DisplayPanelThemeMapDTO>)displayPanelThemeMapDTOListBS.DataSource;
                            DisplayPanelThemeMapDTO displayPanelThemeMapDTO = displayPanelThemeMapDTOSortableList[row.Index];
                            displayPanelThemeMapDTO.IsActive = false;
                            DisplayPanelThemeMapBL displayPanelThemeMapBL = new DisplayPanelThemeMapBL(machineUserContext, displayPanelThemeMapDTO);
                            displayPanelThemeMapBL.Save();
                        }
                    }
                }
                if (rowsDeleted == true)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
                if (refreshFromDB == true)
                {
                    RefreshData();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private string ValidateScheduleDTO(ScheduleCalendarDTO scheduleDTO)
        {
            log.LogMethodEntry(scheduleDTO);
            string message = string.Empty;
            if (string.IsNullOrEmpty(scheduleDTO.ScheduleName) || string.IsNullOrWhiteSpace(scheduleDTO.ScheduleName))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", lblScheduleName.Text);
            }
            if (scheduleDTO.ScheduleTime.Date > scheduleDTO.ScheduleEndDate.Date)
            {
                message = utilities.MessageUtils.getMessage(571);
            }
            if (string.Equals(scheduleDTO.RecurFlag, "Y"))
            {
                if (scheduleDTO.ScheduleTime >= scheduleDTO.RecurEndDate)
                {
                    message = utilities.MessageUtils.getMessage(606);
                }
                decimal scheduleTime = new decimal(scheduleDTO.ScheduleTime.Hour + (((double)scheduleDTO.ScheduleTime.Minute) / 100));
                decimal scheduleEndTime = new decimal(scheduleDTO.ScheduleEndDate.Hour + (((double)scheduleDTO.ScheduleEndDate.Minute) / 100));
                if (scheduleTime >= scheduleEndTime)
                {
                    message = utilities.MessageUtils.getMessage(571);
                }
            }
            else
            {
                if (scheduleDTO.ScheduleTime.Date == scheduleDTO.ScheduleEndDate.Date)
                {
                    if (scheduleDTO.ScheduleTime > scheduleDTO.ScheduleEndDate)
                    {
                        message = utilities.MessageUtils.getMessage(571);
                    }
                }
            }
            log.LogMethodExit(message);
            return message;
        }

        private string ValidateDisplayPanelThemeMapDTO(DisplayPanelThemeMapDTO displayPanelThemeMapDTO)
        {
            log.LogMethodEntry(displayPanelThemeMapDTO);
            string message = string.Empty;
            if (displayPanelThemeMapDTO.PanelId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.HeaderText);
            }
            if (displayPanelThemeMapDTO.ThemeId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void btnInclExclDays_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ScheduleExclusionUI scheduleExclusionUI = new ScheduleExclusionUI(scheduleDTO, utilities);
            scheduleExclusionUI.ShowDialog();
            log.LogMethodExit();
        }

        private void rdbDaily_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            UpdateRecurFrequecyAndRecureType();
            log.LogMethodExit();
        }

        private void rdbWeekly_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            UpdateRecurFrequecyAndRecureType();
            log.LogMethodExit();
        }

        private void rdbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            UpdateRecurFrequecyAndRecureType();
            log.LogMethodExit();
        }

        private void UpdateRecurFrequecyAndRecureType()
        {
            log.LogMethodEntry();
            if (rdbDaily.Checked || rdbWeekly.Checked)
            {
                rdbDay.Enabled = false;
                rdbDay.Checked = false;
                rdbWeekDay.Enabled = false;
                rdbWeekDay.Checked = false;
            }
            else if (rdbMonthly.Checked)
            {
                rdbDay.Enabled = true;
                rdbDay.Checked = true;
                rdbWeekDay.Enabled = true;
                rdbWeekDay.Checked = false;
            }
            dtpScheduleEndDate.MinDate = dtpScheduleDate.Value.Date;
            if (rdbDaily.Checked || rdbMonthly.Checked)
            {
                dtpScheduleEndDate.Enabled = false;
                dtpScheduleEndDate.Value = dtpScheduleDate.Value;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;
            }
            else if (rdbWeekly.Checked)
            {
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleEndDate.Year,
                                                        scheduleDTO.ScheduleEndDate.Month,
                                                        scheduleDTO.ScheduleEndDate.Day);
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.Value.AddDays(6);
            }
            else
            {
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleEndDate.Year,
                                                        scheduleDTO.ScheduleEndDate.Month,
                                                        scheduleDTO.ScheduleEndDate.Day);
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                LoadDisplayPanelThemeMapDTOList();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dtpScheduleDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dtpScheduleEndDate.MinDate = dtpScheduleDate.Value.Date;
            if (rdbDaily.Checked || rdbMonthly.Checked)
            {
                dtpScheduleEndDate.Enabled = false;
                dtpScheduleEndDate.Value = dtpScheduleDate.Value;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;
            }
            else if (rdbWeekly.Checked)
            {
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.Value.AddDays(6);
            }
            else
            {
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvDisplayPanelThemeMapDTOList.AllowUserToAddRows = true;
                dgvDisplayPanelThemeMapDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvDisplayPanelThemeMapDTOList.AllowUserToAddRows = false;
                dgvDisplayPanelThemeMapDTOList.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

    }
}

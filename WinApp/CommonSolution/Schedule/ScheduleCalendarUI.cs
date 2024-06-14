
/********************************************************************************************
 * Project Name - Schedule Calendar UI
 * Description  - User interface for schedule calendar
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        22-Feb-2016   Suneetha.S          Modified 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified         
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
 ********************************************************************************************/ 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Windows.Forms;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Schedule
{
    /// <summary>
    /// Schedule Calendar UI
    /// </summary>
    public partial class ScheduleCalendarUI : Form
    {
        Semnox.Core.Utilities.Utilities _Utilities;
        List<ScheduleCalendarDTO> scheduleDTOSearchList = new List<ScheduleCalendarDTO>();
        List<ScheduleCalendarDTO> scheduleDTOList = new List<ScheduleCalendarDTO>();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        public ScheduleCalendarUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            _Utilities = utilities;
            RegisterKeyDownHandlers(this);
            
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            _Utilities.setLanguage(this);
            SetupDay();
            SetupWeek();            
            Loadsearch();
            _Utilities.setupDataGridProperties(ref dgvAll);
            log.LogMethodExit();
        }
        private void SchedueCalendarUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            tabCalendar.SelectedIndex = 2;
            dgvDay.Width = dgvWeek.Width;
            btnPrev.Width = btnNext.Width = 30;
            btnPrev.Font = btnNext.Font = new Font(btnPrev.Font, FontStyle.Bold);
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (tabCalendar.SelectedIndex == 0)
                {
                    datePicker.Value = datePicker.Value.AddDays(-1);
                }
                else if (tabCalendar.SelectedIndex == 1)
                {
                    datePicker.Value = datePicker.Value.AddDays(-7);
                }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (tabCalendar.SelectedIndex == 0)
                {
                    datePicker.Value = datePicker.Value.AddDays(1);
                }
                else if (tabCalendar.SelectedIndex == 1)
                {
                    datePicker.Value = datePicker.Value.AddDays(7);
                }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            updateColumnHeaders();
            if (tabCalendar.SelectedTab.Name.Contains("Week"))
                dgvWeek.Focus();
            else
                dgvDay.Focus();
            log.LogMethodExit();
        }
        /// <summary>
        /// Updates color to the headers
        /// </summary>
        private void updateColumnHeaders()
        {
            log.LogMethodEntry();
            int daysToAdd = 0;
            DateTime FirstDayOfWeek = datePicker.Value.Date;
            switch (datePicker.Value.DayOfWeek)
            {
                case DayOfWeek.Monday: daysToAdd = 0; break;
                case DayOfWeek.Tuesday: daysToAdd = -1; break;
                case DayOfWeek.Wednesday: daysToAdd = -2; break;
                case DayOfWeek.Thursday: daysToAdd = -3; break;
                case DayOfWeek.Friday: daysToAdd = -4; break;
                case DayOfWeek.Saturday: daysToAdd = -5; break;
                case DayOfWeek.Sunday: daysToAdd = -6; break;
                default: daysToAdd = 0; break;
            }
            FirstDayOfWeek = FirstDayOfWeek.AddDays(daysToAdd);
            foreach (DataGridViewColumn dc in dgvWeek.Columns)
            {
                dc.HeaderText = FirstDayOfWeek.AddDays(dc.Index).ToString("dddd, MMM dd");
            }
            dgvDay.Columns[0].HeaderText = datePicker.Value.ToString("dddd, MMM dd");
            log.LogMethodExit();
        }
       
        /// <summary>
        /// Loads the reocords to the day grid
        /// </summary>
        private void SetupDay()
        {
            log.LogMethodEntry();
            SetupTime(ref dgvDay);
            SetupDGVs(ref dgvDay);
            _Utilities.setLanguage(dgvDay);
            log.LogMethodExit();
        }
        private void RefreshGrids()
        {
            log.LogMethodEntry();
            try
            {
                switch (tabCalendar.SelectedTab.Text)
                {
                    case "Week":
                        RefreshWeekGrid();
                        break;
                    case "Day":
                        RefreshDayGrid();
                        break;
                    case "All":
                        LoadAll();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the schedules to day
        /// </summary>
        private void RefreshDayGrid()
        {
            log.LogMethodEntry();
            DateTime scheduleFromDate, scheduleToDate;
            DateTime origscheduleFromDate, origscheduleToDate;

            int scheduleId;
            string isActive;
            string recur_flag, recur_frequency, recurType;
            string scheduleName;
            DateTime recur_end_date = DateTime.MinValue;

            int minRow = -1;
            int maxRow = 100;
            int numrows;
            DateTime cellTime;

            // scroll to top of tab page
            ScrollableControl scrCtl = tabPageWeek as ScrollableControl;
            scrCtl.ScrollControlIntoView(dgvWeek);
            scrCtl = tabPageDay as ScrollableControl;
            scrCtl.ScrollControlIntoView(dgvDay);
            Core.GenericUtilities.ScheduleCalendarBL schedule = new Core.GenericUtilities.ScheduleCalendarBL(machineUserContext);
            List<ScheduleCalendarDTO> scheduleDayList = new List<ScheduleCalendarDTO>();
            List<ScheduleCalendarDTO> scheduleDaySelectedList = schedule.GetScheduleDayList(datePicker.Value.Date, datePicker.Value.Date.AddDays(1), (_Utilities.ParafaitEnv.IsCorporate&&_Utilities.ParafaitEnv.IsCorporate)? _Utilities.ParafaitEnv.SiteId:-1);
            if (chbShowActiveEntries.Checked)
            {
                scheduleDayList = scheduleDaySelectedList.Where(x => (bool)x.IsActive).ToList<ScheduleCalendarDTO>();
            }
            else
            {
                scheduleDayList = scheduleDaySelectedList;
            }
            for (int i = 0; i < tabPageDay.Controls.Count; i++)
            {
                Control c = tabPageDay.Controls[i];
                if (c.Name.Contains("scheduleDisplay"))
                {
                    tabPageDay.Controls.Remove(c);
                    c.Dispose();
                    i = -1;
                }
            }
            if (scheduleDayList != null)
            {

                for (int rows = 0; rows < scheduleDayList.Count; rows++)
                {
                    minRow = -1;
                    maxRow = 100;

                    scheduleId = scheduleDayList[rows].ScheduleId;
                    scheduleName = scheduleDayList[rows].ScheduleName;
                    scheduleFromDate = scheduleDayList[rows].ScheduleTime;
                    scheduleToDate = scheduleDayList[rows].ScheduleTime.AddMinutes(15);
                    isActive = (scheduleDayList[rows].IsActive == true? "Y":"N");
                    recur_flag = scheduleDayList[rows].RecurFlag;
                    recur_frequency = scheduleDayList[rows].RecurFrequency;
                    recurType = scheduleDayList[rows].RecurType;
                    // if (dtDay.Rows[rows]["recur_end_date"] != DBNull.Value)
                    recur_end_date = scheduleDayList[rows].RecurEndDate;

                    scheduleFromDate = scheduleFromDate.Date.AddHours(scheduleFromDate.Hour).AddMinutes(scheduleFromDate.Minute);
                    scheduleToDate = scheduleToDate.Date.AddHours(scheduleToDate.Hour).AddMinutes(scheduleToDate.Minute);

                    origscheduleFromDate = scheduleFromDate;
                    origscheduleToDate = scheduleToDate;

                    if (recur_flag == "Y")
                    {
                        DateTime cellTimeRecur = GetGridCellDateTime(0, 0, false); // get date on day of week
                        if (recur_end_date >= cellTimeRecur.Date) // check if recur has ended
                        {
                            if (recur_frequency == "W")
                            {
                                if (cellTimeRecur.DayOfWeek != scheduleFromDate.DayOfWeek)
                                    continue;
                            }
                            else if (recur_frequency == "M")
                            {
                                if (recurType == "D")
                                {
                                    if (cellTimeRecur.Day != scheduleFromDate.Day)
                                        continue;
                                }
                                else
                                {
                                    DateTime monthStart = new DateTime(scheduleFromDate.Year, scheduleFromDate.Month, 1);
                                    int promoWeekNo = 0;
                                    while (monthStart <= scheduleFromDate)
                                    {
                                        if (monthStart.DayOfWeek == scheduleFromDate.DayOfWeek)
                                            promoWeekNo++;
                                        monthStart = monthStart.AddDays(1);
                                    }

                                    monthStart = new DateTime(cellTimeRecur.Year, cellTimeRecur.Month, 1);
                                    int gridWeekNo = 0;
                                    while (monthStart <= cellTimeRecur)
                                    {
                                        if (monthStart.DayOfWeek == cellTimeRecur.DayOfWeek)
                                            gridWeekNo++;
                                        monthStart = monthStart.AddDays(1);
                                    }

                                    if (cellTimeRecur.DayOfWeek != scheduleFromDate.DayOfWeek || gridWeekNo != promoWeekNo)
                                        continue;
                                }
                            }

                            TimeSpan ts = scheduleToDate.Date - scheduleFromDate.Date; // used to get number of days the promotion spans over. change promotion from and to days as per week day date
                            scheduleFromDate = cellTimeRecur.Date.AddHours(scheduleFromDate.Hour).AddMinutes(scheduleFromDate.Minute);
                            scheduleToDate = cellTimeRecur.Date.AddDays(ts.Days).AddHours(scheduleToDate.Hour).AddMinutes(scheduleToDate.Minute);
                        }
                    }
                    ScheduleCalendarExclusionBL scheduleExclusion = new ScheduleCalendarExclusionBL(machineUserContext, scheduleId);
                    bool status = scheduleExclusion.GetExclusionDays(scheduleId, scheduleFromDate.Date);
                    if (status)
                        continue;
                    
                    for (int i = 0; i < dgvDay.RowCount; i++)
                    {
                        cellTime = GetGridCellDateTime(i, 0, false);
                        if (cellTime >= scheduleFromDate && cellTime < scheduleToDate)
                        {
                            if (minRow == -1)
                                minRow = i;
                            maxRow = i;
                        }
                    }

                    if (minRow != -1)
                    {
                        Label scheduleDisplay = new Label();
                        scheduleDisplay.Name = "scheduleDisplay" + rows.ToString();
                        scheduleDisplay.Font = new Font("arial", 8);
                        scheduleDisplay.BorderStyle = BorderStyle.FixedSingle;

                        Color backColor;
                        switch (rows)
                        {
                            case 1: backColor = Color.LightBlue; break;
                            case 2: backColor = Color.LightCoral; break;
                            case 3: backColor = Color.LightCyan; break;
                            case 4: backColor = Color.LightGreen; break;
                            case 5: backColor = Color.LightPink; break;
                            case 6: backColor = Color.LightSalmon; break;
                            case 7: backColor = Color.LightSkyBlue; break;
                            default: backColor = Color.LightSteelBlue; break;
                        }
                        if (isActive != "Y")
                            backColor = Color.Gray;

                        scheduleDisplay.BackColor = backColor;
                        scheduleDisplay.Text = scheduleName + " - " + origscheduleFromDate.ToString("MMM dd, h:mm tt") + " to " + origscheduleToDate.ToString("MMM dd, h:mm tt");

                        if (recur_flag == "Y")
                        {
                            scheduleDisplay.Text += " Recurs every " + (recur_frequency == "D" ? "day " : "week ");
                            scheduleDisplay.Text += "until " + recur_end_date.ToString(_Utilities.ParafaitEnv.DATE_FORMAT);
                        }

                        scheduleDisplay.Tag = scheduleDayList[rows];//scheduleId;
                        scheduleDisplay.DoubleClick += new EventHandler(scheduleDisplay_DoubleClick);

                        if (maxRow == minRow)
                            numrows = 1;
                        else
                            numrows = maxRow - minRow + 1;
                        scheduleDisplay.Size = new Size(dgvDay.Columns[0].Width - 10, dgvDay.Rows[0].Height * numrows);
                        scheduleDisplay.Location = new Point(dgvDay.RowHeadersWidth + 1, dgvDay.Rows[0].Height * minRow + dgvDay.ColumnHeadersHeight + 1);
                        tabPageDay.Controls.Add(scheduleDisplay);
                        scheduleDisplay.BringToFront();

                    }
                }
            }
            tabPageDay.Refresh();
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the schedules to weekly grid
        /// </summary>
        private void RefreshWeekGrid()
        {
            log.LogMethodEntry();
            Core.GenericUtilities.ScheduleCalendarBL schedule = new Core.GenericUtilities.ScheduleCalendarBL(machineUserContext);
            int scheduleId;
            string isActive;
            string recur_flag, recur_frequency, recurType;
            string scheduleName;
            DateTime recur_end_date = DateTime.MinValue;
            DateTime scheduleFromDate, scheduleToDate;
            DateTime origscheduleFromDate, origscheduleToDate;
            int minRow = -1;
            int maxRow = 100;
            int numrows;
            DateTime cellTime;
            List<ScheduleCalendarDTO> scheduleWeekList = new List<ScheduleCalendarDTO>();
            List<ScheduleCalendarDTO> scheduleWeekSelectedList = schedule.GetScheduleDayList(GetFirstDayOfWeek(), GetFirstDayOfWeek().AddDays(7), (_Utilities.ParafaitEnv.IsCorporate && _Utilities.ParafaitEnv.IsCorporate) ? _Utilities.ParafaitEnv.SiteId : -1);
            if (chbShowActiveEntries.Checked)
            {
                scheduleWeekList = scheduleWeekSelectedList.Where(x => (bool)x.IsActive).ToList<ScheduleCalendarDTO>();
            }
            else
            {
                scheduleWeekList = scheduleWeekSelectedList;
            }

            for (int i = 0; i < tabPageWeek.Controls.Count; i++)
            {
                Control c = tabPageWeek.Controls[i];
                if (c.Name.Contains("scheduleDisplay"))
                {
                    tabPageWeek.Controls.Remove(c);
                    c.Dispose();
                    i = -1;
                }
            }
            if (scheduleWeekList != null)
            {
                for (int rows = 0; rows < scheduleWeekList.Count; rows++)
                {
                    scheduleId = scheduleWeekList[rows].ScheduleId;
                    scheduleName = scheduleWeekList[rows].ScheduleName;
                    scheduleFromDate = scheduleWeekList[rows].ScheduleTime;
                    scheduleToDate = scheduleWeekList[rows].ScheduleTime.AddMinutes(15);
                    isActive = (scheduleWeekList[rows].IsActive == true? "Y":"N");                    
                    recur_flag = scheduleWeekList[rows].RecurFlag;
                    recur_frequency = scheduleWeekList[rows].RecurFrequency.ToString();
                    recurType = scheduleWeekList[rows].RecurType;
                    //if (dtWeek.Rows[rows]["recur_end_date"] != DBNull.Value)
                    recur_end_date = scheduleWeekList[rows].RecurEndDate;

                    scheduleFromDate = scheduleFromDate.Date.AddHours(scheduleFromDate.Hour).AddMinutes(scheduleFromDate.Minute);
                    scheduleToDate = scheduleToDate.Date.AddHours(scheduleToDate.Hour).AddMinutes(scheduleToDate.Minute);

                    origscheduleFromDate = scheduleFromDate;
                    origscheduleToDate = scheduleToDate;

                    int numIterations = 0;

                    if (recur_flag == "Y")
                        numIterations = dgvWeek.Columns.Count; // repeat for each day of week to check if recur applicable
                    else
                        numIterations = 1;

                    for (int recurDate = 0; recurDate < numIterations; recurDate++)
                    {
                        if (recur_flag == "Y")
                        {
                            DateTime cellTimeRecur = GetGridCellDateTime(0, recurDate, true); // get date on day of week

                            if (recur_frequency == "W")
                            {
                                if (cellTimeRecur.DayOfWeek != origscheduleFromDate.DayOfWeek)
                                    continue;
                            }
                            else if (recur_frequency == "M")
                            {
                                if (recurType == "D")
                                {
                                    if (cellTimeRecur.Day != scheduleFromDate.Day)
                                        continue;
                                }
                                else
                                {
                                    DateTime monthStart = new DateTime(scheduleFromDate.Year, scheduleFromDate.Month, 1);
                                    int promoWeekNo = 0;
                                    while (monthStart <= scheduleFromDate)
                                    {
                                        if (monthStart.DayOfWeek == scheduleFromDate.DayOfWeek)
                                            promoWeekNo++;
                                        monthStart = monthStart.AddDays(1);
                                    }

                                    monthStart = new DateTime(cellTimeRecur.Year, cellTimeRecur.Month, 1);
                                    int gridWeekNo = 0;
                                    while (monthStart <= cellTimeRecur)
                                    {
                                        if (monthStart.DayOfWeek == cellTimeRecur.DayOfWeek)
                                            gridWeekNo++;
                                        monthStart = monthStart.AddDays(1);
                                    }

                                    if (cellTimeRecur.DayOfWeek != scheduleFromDate.DayOfWeek || gridWeekNo != promoWeekNo)
                                        continue;
                                }
                            }

                            if (recur_end_date >= cellTimeRecur.Date && origscheduleFromDate.Date <= cellTimeRecur.Date) // check if recur has ended
                            {
                                TimeSpan ts = scheduleToDate.Date - scheduleFromDate.Date; // used to get number of days the promotion spans over. change promotion from and to days as per week day date
                                scheduleFromDate = cellTimeRecur.Date.AddHours(scheduleFromDate.Hour).AddMinutes(scheduleFromDate.Minute);
                                scheduleToDate = cellTimeRecur.Date.AddDays(ts.Days).AddHours(scheduleToDate.Hour).AddMinutes(scheduleToDate.Minute);
                            }
                            else
                            {
                                continue;
                            }
                        }

                        foreach (DataGridViewColumn dc in dgvWeek.Columns)
                        {
                            if (!dc.Visible)
                                continue;
                            ScheduleCalendarExclusionBL scheduleExclusion = new ScheduleCalendarExclusionBL(machineUserContext, scheduleId);
                            bool status = scheduleExclusion.GetExclusionDays(scheduleId, GetGridCellDateTime(0, dc.Index, true));
                            if (status)
                                continue;

                            minRow = -1;
                            maxRow = 100;
                            for (int i = 0; i < dgvWeek.RowCount; i++)
                            {
                                cellTime = GetGridCellDateTime(i, dc.Index, true);

                                if (cellTime >= scheduleFromDate && cellTime < scheduleToDate)
                                {
                                    if (minRow == -1)
                                        minRow = i;
                                    maxRow = i;
                                }
                            }

                            if (minRow != -1)
                            {
                                Label scheduleDisplay = new Label();
                                scheduleDisplay.Name = "scheduleDisplay" + rows.ToString();
                                scheduleDisplay.Font = new Font("arial", 8);
                                scheduleDisplay.BorderStyle = BorderStyle.FixedSingle;

                                Color backColor;
                                switch (rows)
                                {
                                    case 1: backColor = Color.LightBlue; break;
                                    case 2: backColor = Color.LightCoral; break;
                                    case 3: backColor = Color.LightCyan; break;
                                    case 4: backColor = Color.LightGreen; break;
                                    case 5: backColor = Color.LightPink; break;
                                    case 6: backColor = Color.LightSalmon; break;
                                    case 7: backColor = Color.LightSkyBlue; break;
                                    default: backColor = Color.LightSteelBlue; break;
                                }
                                if (isActive != "Y")
                                    backColor = Color.Gray;

                                scheduleDisplay.BackColor = backColor;
                                scheduleDisplay.Text = scheduleName + " - " + origscheduleFromDate.ToString("MMM dd, h:mm tt") + " to " + origscheduleToDate.ToString("MMM dd, h:mm tt");

                                if (recur_flag == "Y")
                                {
                                    scheduleDisplay.Text += " Recurs every " + (recur_frequency == "D" ? "day " : "week ");
                                    scheduleDisplay.Text += "until " + recur_end_date.ToString(_Utilities.ParafaitEnv.DATE_FORMAT);
                                }

                                scheduleDisplay.Tag = scheduleWeekList[rows];//scheduleId;
                                scheduleDisplay.DoubleClick += new EventHandler(scheduleDisplay_DoubleClick);

                                if (maxRow == minRow)
                                    numrows = 1;
                                else
                                    numrows = maxRow - minRow + 1;
                                scheduleDisplay.Size = new Size(dgvWeek.Columns[0].Width - 10, dgvWeek.Rows[0].Height * numrows);
                                scheduleDisplay.Location = new Point(dgvWeek.Columns[0].Width * dc.Index + dgvWeek.RowHeadersWidth + 1, dgvDay.Rows[0].Height * minRow + dgvWeek.ColumnHeadersHeight + 1);
                                tabPageWeek.Controls.Add(scheduleDisplay);
                                scheduleDisplay.BringToFront();
                            }
                        }
                    }
                }
            }
            tabPageWeek.Refresh();
            log.LogMethodExit();
        }
        /// <summary>
        ///Loads data to all tab grid 
        /// </summary>
        private void LoadAll()
        {
            log.LogMethodEntry();
            DateTime dt=new DateTime();
            List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> scheduleSearchParams = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, "1"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_NAME, string.IsNullOrEmpty(txtName.Text) ? "" : txtName.Text));
            }
            scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            ScheduleCalendarListBL scheduleList = new ScheduleCalendarListBL(machineUserContext);
            scheduleDTOList = scheduleList.GetAllSchedule(scheduleSearchParams);
            BindingSource bindingSource = new BindingSource();
            if (scheduleDTOList != null)
            {
                for (int i = 0; i < scheduleDTOList.Count; i++)
                {
                    if (scheduleDTOList[i].RecurEndDate.Equals(DateTime.MinValue))
                    {
                        scheduleDTOList[i].RecurEndDate = dt;
                    }
                }
                SortableBindingList<ScheduleCalendarDTO> scheduleDTOSortList = new SortableBindingList<ScheduleCalendarDTO>(scheduleDTOList);
                bindingSource.DataSource = scheduleDTOSortList;
            }
            else
            {
                bindingSource.DataSource = new SortableBindingList<ScheduleCalendarDTO>();
            }
            dgvAll.DataSource = bindingSource;
            scheduleTimeDataGridViewTextBoxColumn.DefaultCellStyle=
                recurEndDateDataGridViewTextBoxColumn.DefaultCellStyle = _Utilities.gridViewDateTimeCellStyle();
            log.LogMethodExit();
        }
        /// <summary>
        /// get the DateTime of the specified cell
        /// </summary>
        /// <param name="rowIndex">The row index passed for the identification</param>
        /// <param name="colIndex">The Column index passed for the identification</param>
        /// <param name="isweek">To identify the grid</param>
        /// <returns>Datetime object</returns>
        private DateTime GetGridCellDateTime(int rowIndex, int colIndex, bool isweek)
        {
            log.LogMethodEntry(rowIndex, colIndex, isweek);
            DateTime CellTime;
            if (isweek)
            {
                CellTime = GetFirstDayOfWeek();
            }
            else
                CellTime = datePicker.Value.Date;

            CellTime = CellTime.AddDays(colIndex);
            int hour = Convert.ToInt32(rowIndex * 30 / 60);
            int mins = rowIndex % 2 * 30;
            CellTime = CellTime.AddHours(hour);
            CellTime = CellTime.AddMinutes(mins);
            log.LogMethodExit();
            return CellTime;
        }
        /// <summary>
        /// Gets First day of the week
        /// </summary>
        /// <returns>DateTime Object</returns>
        private DateTime GetFirstDayOfWeek()
        {
            log.LogMethodEntry();
            int daysToAdd = 0;
            switch (datePicker.Value.DayOfWeek)
            {
                case DayOfWeek.Monday: daysToAdd = 0; break;
                case DayOfWeek.Tuesday: daysToAdd = -1; break;
                case DayOfWeek.Wednesday: daysToAdd = -2; break;
                case DayOfWeek.Thursday: daysToAdd = -3; break;
                case DayOfWeek.Friday: daysToAdd = -4; break;
                case DayOfWeek.Saturday: daysToAdd = -5; break;
                case DayOfWeek.Sunday: daysToAdd = -6; break;
                default: daysToAdd = 0; break;
            }
            log.LogMethodExit();
            return (datePicker.Value.Date.AddDays(daysToAdd));
        }
        /// <summary>
        /// Loads Serached Records
        /// </summary>
        private void Loadsearch()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> scheduleSearchParams = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                if (chbShowActiveEntries.Checked)
                {
                    scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, "1"));
                }
                scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                ScheduleCalendarDataHandler scheduleDataHandler = new ScheduleCalendarDataHandler(_Utilities.ExecutionContext, null);
                scheduleDTOSearchList = scheduleDataHandler.GetScheduleCalendarDTOList(scheduleSearchParams);
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = scheduleDTOSearchList;
                dgvSearch.DataSource = bindingSource;
                for (int i = 0; i < dgvSearch.Columns.Count; i++)
                {
                    if (!dgvSearch.Columns[i].Name.Equals("ScheduleName"))
                    {
                        dgvSearch.Columns[i].Visible = false;
                    }
                    else
                    {
                        dgvSearch.Columns[i].Width = dgvSearch.Width;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Setups the week
        /// </summary>
        private void SetupWeek()
        {
            log.LogMethodEntry();
            SetupTime(ref dgvWeek);
            SetupDGVs(ref dgvWeek);
            updateColumnHeaders();
            _Utilities.setLanguage(dgvWeek);
            dgvWeek.Refresh();
            log.LogMethodExit();
        }
        /// <summary>
        /// Sets the Time in Grid
        /// </summary>
        /// <param name="dgv">Takes Grid object as parameter</param>
        private void SetupTime(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            dgv.RowTemplate.Height = 16;
            for (int i = 0; i <= 47; i++)
            {
                if (dgv.Name.Contains("Week"))
                    dgv.Rows.Add(new object[] { "", "", "", "", "", "", "" });
                else
                    dgv.Rows.Add(new object[] { "" });

                if (i == 0)
                    dgv.Rows[i].HeaderCell.Value = "12:AM";
                else
                {
                    if (i == 24)
                        dgv.Rows[i].HeaderCell.Value = "12:PM";
                    else
                    {
                        if (i % 2 == 0)
                        {
                            if (i < 24)
                                dgv.Rows[i].HeaderCell.Value = (i / 2).ToString() + ":00";
                            else
                                dgv.Rows[i].HeaderCell.Value = ((i - 24) / 2).ToString() + ":00";
                        }
                        else
                            dgv.Rows[i].HeaderCell.Value = "";
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Sets the gridview properties.
        /// </summary>
        /// <param name="dgv">Takes grid object as parameter</param>
        private void SetupDGVs(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            dgv.ScrollBars = ScrollBars.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.Size = new Size(dgv.Width, dgv.Rows.Count * dgv.Rows[0].Height + dgv.ColumnHeadersHeight);
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.BackgroundColor = this.BackColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("arial", 8);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (DataGridViewColumn dc in dgv.Columns)
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dgv.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgv.GridColor = dgv.DefaultCellStyle.BackColor = Color.LightYellow;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            log.LogMethodExit();
        }
        private void dgvDay_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            cellPaint(sender, e);
            log.LogMethodExit();
        }
        private void cellPaint(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Value != null)
            {
                if (e.Value.ToString().Contains(":")) // header
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    e.Graphics.DrawLine(Pens.Gray, e.CellBounds.X + 10, e.CellBounds.Y, e.CellBounds.Right - 10, e.CellBounds.Y);
                    string hour = e.Value.ToString();
                    string mins = hour.Substring(hour.IndexOf(':') + 1);
                    hour = hour.Substring(0, hour.IndexOf(':'));

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far;

                    e.Graphics.DrawString(hour, new Font(this.Font.FontFamily, 12, FontStyle.Bold)
                        , Brushes.Black,
                        new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 35, e.CellBounds.Y),
                        sf);

                    e.Graphics.DrawString(mins, new Font(this.Font.FontFamily, 9, FontStyle.Regular)
                        , Brushes.Black,
                        new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 10, e.CellBounds.Y),
                        sf);

                    e.Handled = true;
                }
                else if (e.ColumnIndex >= 0 && e.RowIndex >= 0) // cells
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    if (e.RowIndex % 2 == 0)
                    {
                        e.Graphics.DrawLine(Pens.DarkKhaki, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Right, e.CellBounds.Y);
                        e.Graphics.DrawLine(Pens.Khaki, e.CellBounds.X, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Khaki, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Right, e.CellBounds.Y);
                        e.Graphics.DrawLine(Pens.DarkKhaki, e.CellBounds.X, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
                    }
                    e.Graphics.DrawLine(Pens.Black, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.X, e.CellBounds.Bottom);

                    e.Handled = true;
                }
            }
            log.LogMethodExit();
        }
        private void dgvWeek_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            cellPaint(sender, e);
            log.LogMethodExit();
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (txtName.Text.Length > 0)
            {
                if (scheduleDTOSearchList != null)
                {
                    List<ScheduleCalendarDTO> scheduleList = scheduleDTOSearchList.Where(x => (bool)(x.ScheduleName.ToLower().Contains(txtName.Text.ToLower()))).ToList<ScheduleCalendarDTO>();
                    if (scheduleList.Count > 0)
                    {
                        dgvSearch.Visible = true;
                        dgvSearch.DataSource = scheduleList;
                    }
                    else
                    {
                        dgvSearch.Visible = false;
                    }
                }
            }
            else
            {
                dgvSearch.Visible = false;
            }
            log.LogMethodExit();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                dgvSearch.Visible = false;
                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> scheduleSearchParams = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                if (chbShowActiveEntries.Checked)
                {
                    scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, "1"));
                }
                if (!string.IsNullOrEmpty(txtName.Text))
                {
                    scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_NAME, txtName.Text));
                }
                scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_FROM_TIME, datePicker.Value.Date.ToString("yyyy-MM-dd HH:mm:ss")));
                scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_TO_TIME, datePicker.Value.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")));
                scheduleSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                ScheduleCalendarListBL scheduleList = new ScheduleCalendarListBL(machineUserContext);
                List<ScheduleCalendarDTO> scheduleDTOList = scheduleList.GetAllSchedule(scheduleSearchParams);
                SortableBindingList<ScheduleCalendarDTO> scheduleDTOSortList = new SortableBindingList<ScheduleCalendarDTO>();
                if (scheduleDTOList != null)
                {
                   scheduleDTOSortList = new SortableBindingList<ScheduleCalendarDTO>(scheduleDTOList);                    
                }
                dgvAll.DataSource = scheduleDTOSortList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void dgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtName.Text = dgvSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                dgvSearch.Visible = false;
            }
            catch (Exception ex){ log.Error(ex); }
            log.LogMethodExit();
        }
        private void dgvDay_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                using (ScheduleUI scheduleUI = new ScheduleUI(null, _Utilities))
                { scheduleUI.ShowDialog(); }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void dgvAll_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvAll.CurrentRow != null)
                {
                    BindingSource scheduleDTOBS = (BindingSource)dgvAll.DataSource;
                    var scheduleDTOList = (SortableBindingList<ScheduleCalendarDTO>)scheduleDTOBS.DataSource;
                    ScheduleCalendarDTO scheduleDTO = scheduleDTOList[dgvAll.CurrentRow.Index];
                    using (ScheduleUI scheduleUI = new ScheduleUI(scheduleDTO, _Utilities))
                    {
                        scheduleUI.ShowDialog();
                    }
                }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void scheduleDisplay_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ScheduleCalendarDTO scheduleDTO;
                ScheduleUI scheduleUI;
                if (((Label)sender).Tag == null)
                {
                    scheduleUI = new ScheduleUI(null, _Utilities);
                }
                else
                {
                    scheduleDTO = (ScheduleCalendarDTO)((Label)sender).Tag;
                    scheduleUI = new ScheduleUI(scheduleDTO, _Utilities);
                }
                scheduleUI.ShowDialog();
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void dgvWeek_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                using (ScheduleUI scheduleUI = new ScheduleUI(null, _Utilities))
                {
                    scheduleUI.ShowDialog();
                }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
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
            dgvSearch.Visible =  false;
            log.LogMethodExit();
        }

        private void GenericAssetUI_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvSearch.Visible = false;
            log.LogMethodExit();
        }
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Down)
            {
                if (dgvSearch.Rows.Count > 0)
                {
                    dgvSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtName.Text = dgvSearch.SelectedCells[0].Value.ToString();                    
                    dgvSearch.Visible = false;
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

        private void tabCalendar_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshGrids();
            log.LogMethodExit();
        }
    }
}


/********************************************************************************************
 * Project Name - Concurrent Program Schedule UI
 * Description  - User interface for Concurrent Program Schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By         Remarks          
 *********************************************************************************************
 *1.00        25-May-2016    Amaresh             Created 
 *2.70.2        19-Sep-2019    Dakshakh            Modified : Added logs
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Schedule UI
    /// </summary>

    public partial class ScheduleUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;

        int ProgramId = -1;
        string ProgramName = string.Empty;
        bool Active = false;
        private ExecutionContext machineUserContext;
        //List<DayLookupDTO> dayLookupDTOList;
        /// <summary>
        /// Parameterized constructor of ScheduleUI
        /// </summary>
        /// <param name="_Utilities">Utilities object as parameter</param>
        /// <param name="programId">Program Id to load</param>
        /// <param name="active">active  to load</param>
        /// <param name="programName">ProgramName to display</param>

        public ScheduleUI(int programId, bool active, string programName, Utilities _Utilities)
        {
            log.LogMethodEntry(programId, active, programName, _Utilities);

            InitializeComponent();
            utilities = _Utilities;

            machineUserContext = ExecutionContext.GetExecutionContext();
            if (_Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            machineUserContext.SetUserId(utilities.ParafaitEnv.Username);
            ProgramId = programId;
            ProgramName = programName;
            Active = active;

            if (active == true)
            {
                chbShowActiveSchedules.Checked = true;
            }
            else
            {
                chbShowActiveSchedules.Checked = false;
            }

            _Utilities.setupDataGridProperties(ref concurrentProgramsScheduleGridView);
            _Utilities.setLanguage(this);

            ScheduleDetails(programId);

            //modified
            //Binding data to Frequency Dropdown
            DayLookupList DayLookupList = new DayLookupList();
            // dayLookupDTOList = dayLookup.GetAllDayLookup();
            BindingSource bindingsource = new BindingSource();
            bindingsource.DataSource = DayLookupList.GetAllDayLookup();
            frequencyDataGridViewComboBox.DataSource = bindingsource;
            frequencyDataGridViewComboBox.ValueMember = "Day";
            frequencyDataGridViewComboBox.DisplayMember = "Display";

            //Binding data to RunAt Dropdown
            DataTable dthrs = new DataTable();
            dthrs.Columns.Add("Hours");
            for (int i = 00; i < 24; i++)
            {
                for (int k = 00; k < 60; k = k + 5)
                {
                    if (k == 00)
                    {
                        dthrs.Rows.Add(i.ToString() + ":00");
                    }
                    else
                    {
                        dthrs.Rows.Add(i.ToString() + ":" + k);
                    }
                }
            }

            runAtDataGridViewComboBox.DataSource = dthrs;
            runAtDataGridViewComboBox.DisplayMember = "Hours";

            //  concurrentProgramsScheduleGridView.BorderStyle = BorderStyle.FixedSingle;
            startDateDataGridViewTextBoxColumn.DefaultCellStyle = endDateDataGridViewTextBoxColumn.DefaultCellStyle = _Utilities.gridViewDateCellStyle();

            log.LogMethodExit();
        }

        /// <summary>
        /// ScheduleDetails
        /// </summary>
        /// <param name="programId">programId</param>
        void ScheduleDetails(int programId)
        {
            log.LogMethodEntry(programId);
            try
            {
                List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> conProSearchParams = new List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>>();
                conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID, programId.ToString()));
                conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.ACTIVE, ((chbShowActiveSchedules.Checked) ? "1" : "0")));

                ConcurrentProgramScheduleList concurrentProgramsScheduleList = new ConcurrentProgramScheduleList(machineUserContext);
                List<ConcurrentProgramSchedulesDTO> concurrentProgramsScheduleListOnDisplay = concurrentProgramsScheduleList.GetAllConcurrentProgramSchedule(conProSearchParams);
                BindingSource concurrentProgramsScheduleListBS = new BindingSource();

                if (concurrentProgramsScheduleListOnDisplay != null)
                {
                    concurrentProgramsScheduleListBS.DataSource = concurrentProgramsScheduleListOnDisplay;
                }
                else
                {
                    concurrentProgramsScheduleListBS.DataSource = new List<ConcurrentProgramSchedulesDTO>();
                }

                concurrentProgramsScheduleGridView.DataSource = concurrentProgramsScheduleListBS;

                lblProgramName.Text = "Program Name:  " + ProgramName;

                log.LogMethodExit();

            }
            catch
            {
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// concurrentProgramsScheduleGridView_RowValidating
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void concurrentProgramsScheduleGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            log.LogMethodEntry();

            int rwCount = concurrentProgramsScheduleGridView.RowCount;
            for (int i = 0; i < rwCount - 1; i++)
            {
                int startDateMax = -1;
                string startDate = concurrentProgramsScheduleGridView.Rows[i].Cells["startDateDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
                string endDate = concurrentProgramsScheduleGridView.Rows[i].Cells["endDateDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();

                // when row is null
                if (concurrentProgramsScheduleGridView.Rows[i].Cells["runAtDataGridViewComboBox"].Value == null)
                {
                    return;
                }

                try
                {
                    Convert.ToDateTime(startDate).ToShortTimeString();
                    e.Cancel = false;
                }
                catch
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(15) + " : " + utilities.MessageUtils.getMessage("StartDate"));
                    e.Cancel = true;
                    return;
                }

                try
                {
                    Convert.ToDateTime(endDate).ToShortTimeString();
                    e.Cancel = false;
                }
                catch
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(15) + " : " + utilities.MessageUtils.getMessage("EndDate"));
                    e.Cancel = true;
                    return;
                }

                try
                {
                    startDateMax = DateTime.Compare(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                }

                catch { }

                // When startdate is greater is than enddate 
                if (startDateMax > 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1156), "Validation");
                    e.Cancel = true;
                    return;
                }

                #region Duplicate Schedule Validation
                //// Check for Duplication date
                //for (int k = i + 1; k < rwCount - 1; k++)
                //{
                //    int match = -1;
                //    try
                //    {
                //        if (Convert.ToBoolean(concurrentProgramsScheduleGridView.Rows[k].Cells["isActiveDataGridViewCheckBoxColumn"].Value) == false || !isActive)
                //            continue;

                //        if (concurrentProgramsScheduleGridView.Rows[k].Cells["startDateDataGridViewTextBoxColumn"].Value != null)
                //        {
                //            string nextDate = concurrentProgramsScheduleGridView.Rows[k].Cells["startDateDataGridViewTextBoxColumn"].Value.ToString();
                //            match = DateTime.Compare(Convert.ToDateTime(startDate), Convert.ToDateTime(nextDate));
                //        }
                //    }
                //    catch
                //    {
                //        MessageBox.Show(utilities.MessageUtils.getMessage(15) + " : " + utilities.MessageUtils.getMessage("Date Format"));
                //        e.Cancel = true;
                //        return;
                //    }

                //    if (match == 0 && (concurrentProgramsScheduleGridView.Rows[k].Cells["runAtDataGridViewComboBox"].Value.ToString()) == runAt
                //        && Convert.ToBoolean(concurrentProgramsScheduleGridView.Rows[k].Cells["isActiveDataGridViewCheckBoxColumn"].Value) == true)
                //    {
                //        MessageBox.Show("Duplicate Schedule Not Allowed");
                //        e.Cancel = true;
                //        return;
                //    }
                //    else
                //    {
                //        e.Cancel = false;
                //    }
                //}
                #endregion
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSave_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                if (concurrentProgramsScheduleGridView.Rows.Count < 2)
                {
                    return;
                }

                BindingSource concurrentProgramsScheduleBS = (BindingSource)concurrentProgramsScheduleGridView.DataSource;
                var concurrentProgramsScheduleListOnDisplay = (List<ConcurrentProgramSchedulesDTO>)concurrentProgramsScheduleBS.DataSource;

                int currentRowIndex = 0;
                foreach (ConcurrentProgramSchedulesDTO concurrentProramsScheduleDTO in concurrentProgramsScheduleListOnDisplay)
                {
                    if (concurrentProramsScheduleDTO.IsChanged)
                    {
                        ConcurrentProgramSchedules concurrentProgramsSchedule = new ConcurrentProgramSchedules(machineUserContext, concurrentProramsScheduleDTO);
                        concurrentProramsScheduleDTO.ProgramId = ProgramId;

                        if (!IsScheduleExists(concurrentProramsScheduleDTO))
                        {
                            concurrentProgramsSchedule.Save();
                            currentRowIndex++;
                        }
                        else
                        {
                            concurrentProgramsScheduleGridView.ClearSelection();
                            concurrentProgramsScheduleGridView.Rows[currentRowIndex].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(1157), utilities.MessageUtils.getMessage("Validation"));
                            return;
                        }
                    }
                }

                ScheduleDetails(ProgramId);
            }
            catch { }

            log.LogMethodExit();
        }

        /// <summary>
        /// IsScheduleExists
        /// </summary>
        /// <param name="scheduleDTO">scheduleDTO</param>
        /// <returns></returns>
        bool IsScheduleExists(ConcurrentProgramSchedulesDTO scheduleDTO)
        {
            log.LogMethodEntry(scheduleDTO);

            if (scheduleDTO != null)
            {
                List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> conProSearchParams = new List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>>();
                conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID, scheduleDTO.ProgramId.ToString()));
                conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.RUNAT, scheduleDTO.RunAt.ToString()));
                conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.START_DATE, scheduleDTO.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.FREQUENCY, scheduleDTO.Frequency.ToString()));

                ConcurrentProgramScheduleList concurrentProgramsScheduleList = new ConcurrentProgramScheduleList(machineUserContext);
                List<ConcurrentProgramSchedulesDTO> concurrentProgramsScheduleListOnDisplay = concurrentProgramsScheduleList.GetAllConcurrentProgramSchedule(conProSearchParams);

                if (concurrentProgramsScheduleListOnDisplay != null && concurrentProgramsScheduleListOnDisplay.Count > 0)
                {
                    if (scheduleDTO.ProgramScheduleId == concurrentProgramsScheduleListOnDisplay[0].ProgramScheduleId)
                    {
                        //updating existng record allow to update
                        return false;
                    }

                    //Records not found insertion allowed
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    //No Records found insertion or update allowed
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// btnRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ScheduleDetails(ProgramId);
            log.LogMethodExit();
        }

        /// <summary>
        /// btnClose_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// chbShowActiveSchedules_CheckedChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void chbShowActiveSchedules_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ScheduleDetails(ProgramId);
            log.LogMethodExit();
        }

        /// <summary>
        /// concurrentProgramsScheduleGridView_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void concurrentProgramsScheduleGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MessageBox.Show("Error in Schedule grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + concurrentProgramsScheduleGridView.Columns[e.ColumnIndex].DataPropertyName +
                   ": " + e.Exception.Message);
                e.Cancel = true;
            }
            catch { }
            log.LogMethodExit();
        }
    }
}

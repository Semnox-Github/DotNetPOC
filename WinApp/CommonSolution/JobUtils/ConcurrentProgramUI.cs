/********************************************************************************************
 * Project Name - Concurrent Programs UI
 * Description  - User interface for Concurrent programs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Feb-2016    Amaresh          Created 
 *2.70.2      18-Sep-2019    Dakshakh         Modified : Added logger
 *2.120.1     09-Jun-2021    Deeksha          Modified: As part of AWS concurrent program enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Program UI
    /// </summary>
    public partial class ConcurrentProgramUI : Form
    {
        ComboBox CmbExecutableName = new ComboBox();
        int intTempProgramId = 0;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        ExecutionContext machineUserContext;
        /// <summary>
        /// Parameterized constructor of ConcurrentProgramUI
        /// </summary>
        /// <param name="_Utilities">Utilities object as parameter</param>
        public ConcurrentProgramUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            _Utilities.setupDataGridProperties(ref concurrentProgramsDataGridView);
            utilities.setLanguage(this);
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

            // for refresh Program grid
            PopulateConcurrentProgramGrid();

            //Binding data to Execution Method Dropdown
            DataTable dt = new DataTable();
            dt.Columns.Add("ExecutionMethod");
            dt.Columns.Add("Desc");
            dt.Rows.Add("E", "EXE");
            dt.Rows.Add("L", "LIBRARY"); //added rakshith    remove
            dt.Rows.Add("P", "SQL Procedure");
            ExecutionMethodDataGridViewComboBox.DataSource = dt;
            ExecutionMethodDataGridViewComboBox.ValueMember = "ExecutionMethod";
            ExecutionMethodDataGridViewComboBox.DisplayMember = "Desc";

            concurrentProgramsDataGridView.BorderStyle = BorderStyle.FixedSingle;
            lastUpdatedDateDataGridViewTextBoxColumn.DefaultCellStyle = _Utilities.gridViewDateTimeCellStyle();

            log.LogMethodExit();
        }

        /// <summary>
        /// ConcurrentProgramUI_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ConcurrentProgramUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //For Adding combobox
            CmbExecutableName = new ComboBox();
            CmbExecutableName.Visible = false;
            CmbExecutableName.DropDownStyle = ComboBoxStyle.DropDownList;
            concurrentProgramsDataGridView.Controls.Add(CmbExecutableName);
            CmbExecutableName.SelectedValueChanged += this.CmbExecutableName_ValueChanged;
            AddComboBoxValues();

            log.LogMethodExit();
        }

        /// <summary>
        /// Loading Exe names to combobox
        /// </summary>
        private void AddComboBoxValues()
        {
            log.LogMethodEntry();

            string root = Application.StartupPath.ToString().ToLower().Replace("\\bin\\debug", "\\");

            log.Debug("Path is  : " + root);
            if (root.Contains("\\Program Files\\Semnox Solutions".ToLower()) == true)
            {
                root = root.Replace("\\application", "\\server") + "\\";
            }
            log.Debug("New Path is  : " + root);

            string[] exeFiles = Directory.GetFiles(root, "*.exe");

            for (int i = 0; i < exeFiles.Length; i++)
                exeFiles[i] = Path.GetFileName(exeFiles[i]);

            foreach (var files in exeFiles)
            {
                if (!CmbExecutableName.Items.Contains(files))
                {
                    CmbExecutableName.Items.Add(files);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// CmbExecutableName_ValueChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void CmbExecutableName_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            if (concurrentProgramsDataGridView.CurrentRow != null)
            {
                if (concurrentProgramsDataGridView.CurrentRow.Cells["ExecutionMethodDataGridViewComboBox"].Value.ToString() == "E")
                {
                    concurrentProgramsDataGridView.CurrentRow.Cells["ExecutableNameDataGridViewTextBoxColumn"].Value = CmbExecutableName.Text;
                    CmbExecutableName.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads Concurrent Program to the Grid
        /// </summary>
        private void PopulateConcurrentProgramGrid()
        {
            log.LogMethodEntry();

            ConcurrentProgramList concurrentProgramsList = new ConcurrentProgramList(machineUserContext);
            try
            {
                List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> concurrentProgramsSearchParams = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
                concurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "1" : "0"));
                concurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SYSTEM_PROGRAM, "0"));
                List<ConcurrentProgramsDTO> concurentProgramsListOnDisplay = concurrentProgramsList.GetAllConcurrentPrograms(concurrentProgramsSearchParams);
                BindingSource concurrentProgramsListBS = new BindingSource();

                if (concurentProgramsListOnDisplay != null)
                {
                    concurrentProgramsListBS.DataSource = concurentProgramsListOnDisplay;
                }
                else
                {
                    concurrentProgramsListBS.DataSource = new List<ConcurrentProgramsDTO>();
                }

                concurrentProgramsDataGridView.DataSource = concurrentProgramsListBS;
            }
            catch { }

            log.LogMethodExit();
        }

        /// <summary>
        /// btnSearch_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> conccurrentProgramsSearchParams = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
            if (!string.IsNullOrEmpty(txtProgramName.Text.Trim()))
            {
                conccurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, txtProgramName.Text));
            }
            conccurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "1" : "0"));
            conccurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SYSTEM_PROGRAM, "0"));
            ConcurrentProgramList concurrentProgramsList = new ConcurrentProgramList(machineUserContext);

            List<ConcurrentProgramsDTO> concurrentProgramsDTOList = concurrentProgramsList.GetAllConcurrentPrograms(conccurrentProgramsSearchParams);

            BindingSource bindingSource = new BindingSource();
            if (concurrentProgramsDTOList != null)
            {
                bindingSource.DataSource = concurrentProgramsDTOList;
            }
            else
            {
                bindingSource.DataSource = new List<ConcurrentProgramsDTO>();
            }
            concurrentProgramsDataGridView.DataSource = bindingSource;

            log.LogMethodExit();
        }

        /// <summary>
        /// For Validating the programs Grid
        /// </summary>
        private bool ValdiateProgram(ConcurrentProgramsDTO concurrentProgramsDTO, int currentRowIndex)
        {
            log.LogMethodEntry(concurrentProgramsDTO, currentRowIndex);

            if (string.IsNullOrEmpty(concurrentProgramsDTO.ProgramName))
            {
                concurrentProgramsDataGridView.ClearSelection();
                concurrentProgramsDataGridView.Rows[currentRowIndex].Cells["programNameDataGridViewTextBoxColumn"].Selected = true;

                MessageBox.Show(utilities.MessageUtils.getMessage(1153), utilities.MessageUtils.getMessage("Validation"));
                return false;
            }

            if (string.IsNullOrEmpty(concurrentProgramsDTO.ExecutableName))
            {
                concurrentProgramsDataGridView.ClearSelection();
                concurrentProgramsDataGridView.Rows[currentRowIndex].Cells["ExecutableNameDataGridViewTextBoxColumn"].Selected = true;
                MessageBox.Show(utilities.MessageUtils.getMessage(1154), utilities.MessageUtils.getMessage("Validation"));
                return false;
            }

            log.LogMethodExit();
            return true;
        }

        /// <summary>
        /// For Saving the programs details
        /// </summary>
        private void SavePrograms()
        {
            log.LogMethodEntry();

            BindingSource concurrentProgramsBS = (BindingSource)concurrentProgramsDataGridView.DataSource;
            var concurrentProgramsListOnDisplay = (List<ConcurrentProgramsDTO>)concurrentProgramsBS.DataSource;

            int currentRowIndex = 0;
            foreach (ConcurrentProgramsDTO concurrentProgramsDTO in concurrentProgramsListOnDisplay)
            {
                // Validate program
                if (ValdiateProgram(concurrentProgramsDTO, currentRowIndex) == false)
                {
                    return;
                }

                string[] ErrorNotificationMailId = null;
                if(concurrentProgramsDTO.ErrorNotificationMailId != null)
                {
                    ErrorNotificationMailId = concurrentProgramsDTO.ErrorNotificationMailId.Split(',');
                    foreach (string email in ErrorNotificationMailId)
                    {
                        //Validate error notification EmailId
                        if (!string.IsNullOrEmpty(concurrentProgramsDTO.ErrorNotificationMailId) && !IsValidEmail(email, currentRowIndex, false))
                        {
                            return;
                        }
                    }
                }
                
                string[] SuccessNotificationMailId = null;
                if (concurrentProgramsDTO.SuccessNotificationMailId != null)
                {
                    SuccessNotificationMailId = concurrentProgramsDTO.SuccessNotificationMailId.Split(',');
                    foreach (string email in SuccessNotificationMailId)
                    {
                        //Validate Success notification EmailId
                        if (!string.IsNullOrEmpty(concurrentProgramsDTO.SuccessNotificationMailId) && !IsValidEmail(email, currentRowIndex, true))
                        {
                            return;
                        }
                    }
                }
                
                ConcurrentPrograms concurrentPrograms = new ConcurrentPrograms(machineUserContext, concurrentProgramsDTO,null,null,null,null);
                concurrentPrograms.Save();
                currentRowIndex++;
            }

            PopulateConcurrentProgramGrid();

            log.LogMethodExit();
        }

        /// <summary>
        /// IsValidEmail
        /// </summary>
        /// <param name="emailId">emailId</param>
        /// <param name="rwIndex">rwIndex</param>
        /// <param name="successMail">successMail</param>
        /// <returns></returns>
        //Method to check emailId is valid or not
        private bool IsValidEmail(string emailId, int rwIndex, bool successMail)
        {
            log.LogMethodEntry(emailId, rwIndex, successMail);
            if (!System.Text.RegularExpressions.Regex.IsMatch(emailId.Trim(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                if (successMail)
                {
                    concurrentProgramsDataGridView.Rows[rwIndex].Cells["successNotificationMailIdDataGridViewTextBoxColumn"].Selected = true;
                }
                else
                {
                    concurrentProgramsDataGridView.Rows[rwIndex].Cells["errorNotificationMailIdDataGridViewTextBoxColumn"].Selected = true;
                }

                MessageBox.Show(utilities.MessageUtils.getMessage(572), utilities.MessageUtils.getMessage("Validation"));
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// concurrentProgramsDataGridView_CellEnter
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void concurrentProgramsDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                CmbExecutableName.Visible = false;
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                if (e.ColumnIndex == 3)
                {
                    if ((concurrentProgramsDataGridView.Rows[e.RowIndex].Cells["ExecutableNameDataGridViewTextBoxColumn"].ColumnIndex == e.ColumnIndex)
                        && concurrentProgramsDataGridView.Rows[e.RowIndex].Cells["ExecutionMethodDataGridViewComboBox"].Value.ToString() == "E")
                    {
                        CmbExecutableName.Visible = true;
                    }
                    else
                    {
                        CmbExecutableName.Visible = false;
                    }

                    if (CmbExecutableName.Visible == true)
                    {
                        CmbExecutableName.Location = concurrentProgramsDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location;
                        CmbExecutableName.Width = ExecutableNameDataGridViewTextBoxColumn.Width;
                    }
                }
            }

            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
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

            SavePrograms();
            PopulateConcurrentProgramGrid();

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
        /// btnRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        //For Refreshing
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtProgramName.Text = string.Empty;
            PopulateConcurrentProgramGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// concurrentProgramsDataGridView_CellContentClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void concurrentProgramsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();

            if (e.RowIndex < 0)
            {
                return;
            }

            int programId;
            string progamName;
            bool active = false;

            if (e.RowIndex >= 0)
            {
                programId = Convert.ToInt32(concurrentProgramsDataGridView.Rows[e.RowIndex].Cells["programIdDataGridViewTextBoxColumn"].Value);
            }
            else
            {
                return;
            }

            if (concurrentProgramsDataGridView.Columns[e.ColumnIndex].Name == "ArgumentsDataGridButton" && programId > 0
                && concurrentProgramsDataGridView.CurrentRow.Cells["ExecutionMethodDataGridViewComboBox"].Value.ToString() == "P")
            {
                progamName = concurrentProgramsDataGridView.Rows[e.RowIndex].Cells["programNameDataGridViewTextBoxColumn"].Value.ToString();
                ConcurrentProgramArgumentsUI frm = new ConcurrentProgramArgumentsUI(programId, progamName, utilities);
                frm.ShowDialog();
                frm.Dispose();
            }

            if (concurrentProgramsDataGridView.Columns[e.ColumnIndex].Name == "ScheduleDatagridButton" && programId > 0)
            {
                if (IsProgramKeepRunning(programId) == false)
                {
                    progamName = concurrentProgramsDataGridView.Rows[e.RowIndex].Cells["programNameDataGridViewTextBoxColumn"].Value.ToString();
                    active = chbShowActiveEntries.Checked == true ? true : false;
                    ScheduleUI frm = new ScheduleUI(programId, active, progamName, utilities);
                    frm.ShowDialog();
                    frm.Dispose();
                }
            }

            if (concurrentProgramsDataGridView.Columns[e.ColumnIndex].Name == "keepRunningDataGridViewCheckBoxColumn")
            {
                if (!IsProgramKeepRunning(programId) && IsScheduleExist(programId))
                {
                    DialogResult result = MessageBox.Show(utilities.MessageUtils.getMessage(1155), utilities.MessageUtils.getMessage("Confirmation"), MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        concurrentProgramsDataGridView.CurrentRow.Cells[5].Value = true;
                        InActiveSchedules(programId);
                    }
                    else
                    {
                        concurrentProgramsDataGridView.CurrentRow.Cells[5].Value = false;
                    }
                }
            }

            log.LogMethodExit();

        }

        /// <summary>
        /// IsScheduleExist
        /// </summary>
        /// <param name="programId">programId</param>
        /// <returns></returns>
        //Added to check wheather Schedule is exist for programId
        bool IsScheduleExist(int programId)
        {
            log.LogMethodEntry(programId);
            List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> conProSearchParams = new List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>>();
            conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID, programId.ToString()));
            conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.ACTIVE, "1"));

            ConcurrentProgramScheduleList concurrentProgramsScheduleList = new ConcurrentProgramScheduleList(machineUserContext);
            List<ConcurrentProgramSchedulesDTO> concurrentProgramsScheduleListOnDisplay = concurrentProgramsScheduleList.GetAllConcurrentProgramSchedule(conProSearchParams);
            BindingSource concurrentProgramsScheduleListBS = new BindingSource();

            if (concurrentProgramsScheduleListOnDisplay == null || concurrentProgramsScheduleListOnDisplay.Count < 1)
            {
                log.LogMethodExit(false);
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// IsProgramKeepRunning
        /// </summary>
        /// <param name="programId">programId</param>
        /// <returns></returns>
        private bool IsProgramKeepRunning(int programId)
        {
            log.LogMethodEntry(programId);
            ConcurrentProgramList concurrentProgramsList = new ConcurrentProgramList(machineUserContext);

            List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> concurrentProgramsSearchParams = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
            concurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "1"));
            concurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.KEEP_RUNNING, "1"));
            concurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_ID, programId.ToString()));
            List<ConcurrentProgramsDTO> concurentProgramsListDTO = concurrentProgramsList.GetAllConcurrentPrograms(concurrentProgramsSearchParams);

            if (concurentProgramsListDTO != null && concurentProgramsListDTO.Count > 0)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// InActiveSchedules
        /// </summary>
        /// <param name="programId">programId</param>
        //Added to InActive the Schedule in Keep Running 
        private void InActiveSchedules(int programId)
        {
            log.LogMethodEntry(programId);
            List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> conProSearchParams = new List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>>();
            conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID, programId.ToString()));
            conProSearchParams.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.ACTIVE, (chbShowActiveEntries.Checked) ? "1" : "0"));

            ConcurrentProgramScheduleList concurrentProgramsScheduleList = new ConcurrentProgramScheduleList(machineUserContext);
            List<ConcurrentProgramSchedulesDTO> concurrentProgramsScheduleListOnDisplay = concurrentProgramsScheduleList.GetAllConcurrentProgramSchedule(conProSearchParams);
            BindingSource concurrentProgramsScheduleListBS = new BindingSource();

            if (concurrentProgramsScheduleListOnDisplay != null)
            {
                concurrentProgramsScheduleListBS.DataSource = concurrentProgramsScheduleListOnDisplay;
                //modified rakshith ( for each move to here bcz concurrentProgramsScheduleListOnDisplay is null)
                foreach (ConcurrentProgramSchedulesDTO concurrentProgramSchedulesDTO in concurrentProgramsScheduleListOnDisplay)
                {
                    concurrentProgramSchedulesDTO.IsActive = false;
                    ConcurrentProgramSchedules concurrentProgramSchedules = new ConcurrentProgramSchedules(machineUserContext, concurrentProgramSchedulesDTO);
                    concurrentProgramSchedules.Save();
                }
            }
            else
            {
                concurrentProgramsScheduleListBS.DataSource = new List<ConcurrentProgramSchedulesDTO>();
            }


            log.LogMethodExit();
        }

        /// <summary>
        /// concurrentProgramsDataGridView_DefaultValuesNeeded
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void concurrentProgramsDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();

            if (e.Row.IsNewRow == true)
            {
                intTempProgramId--;
                e.Row.Cells[0].Value = intTempProgramId;
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// concurrentProgramsDataGridView_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void concurrentProgramsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show("Error in concurrent program grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + concurrentProgramsDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// chbShowActiveEntries_CheckedChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void chbShowActiveEntries_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateConcurrentProgramGrid();
            log.LogMethodExit();
        }

    }
}






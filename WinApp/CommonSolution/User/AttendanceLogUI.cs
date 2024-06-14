/********************************************************************************************
 * Project Name - AttendanceLog UI
 * Description  - UI of AttendanceLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018   Indhu                   Created 
 *2.70.2        15-Jul-2019   Girish Kundar           Modified :Added LogmethodEntry() and LogMethodExit() 
 *2.70.2        18-Dec-2019   Jinto Thomas            Added parameter execution context for userbl declaration with userid 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public partial class AttendanceLogUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SortableBindingList<UsersDTO> userDTOSortableList;
        private SortableBindingList<AttendanceLogDTO> attendanceLogDTOSortableList;
        private  Utilities utilities;
        private int userID = -1;
        private DateTime oldTime;
        private DateTime fromdate;
        private DateTime todate = DateTime.Now;
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private string dateTimeFormat;
        private TextBox currentTextBox;

        public AttendanceLogUI(Utilities _Utilities, ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(utilities , machineUserContext);
            InitializeComponent();
            utilities = _Utilities;
            this.machineUserContext = machineUserContext;
            currentTextBox = txtLoginId;
            timestampDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            CalToDate.CustomFormat = utilities.ParafaitEnv.DATE_FORMAT;
            CalFromDate.CustomFormat = utilities.ParafaitEnv.DATE_FORMAT;
            dateTimeFormat = utilities.ParafaitEnv.DATETIME_FORMAT;
            RefreshDates();
            LoadDepartment();
            LoadUserRolesDTO();
            timestampDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = Color.LightBlue;
            log.LogMethodExit();
        }

        private void RefreshDates()
        {
            log.LogMethodEntry();
            chbActiveOnly.Checked = true;
            CalFromDate.Value = utilities.getServerTime().AddDays(-1);
            CalToDate.Value = utilities.getServerTime().Date;
            double businessDayStartTime = 6;
            double.TryParse(utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);
            dtpTimeFrom.Value = dtpTimeTo.Value = utilities.getServerTime().Date.AddHours(businessDayStartTime);
            fromdate = Convert.ToDateTime(CalFromDate.Value.Date.AddHours(Convert.ToDouble(dtpTimeFrom.Value.Hour)));
            todate = utilities.getServerTime().Date.AddHours(Convert.ToDouble(dtpTimeFrom.Value.Hour));
            log.LogMethodExit();
        }

        private void LoadUsersDTOList()
        {
            log.LogMethodEntry();
            try
            {
                UsersList usersList = new UsersList(machineUserContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                if (txtLoginId.Text != "")
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, txtLoginId.Text));
                if (txtEmployeeNo.Text != "")
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.EMP_NUMBER, txtEmployeeNo.Text));
                if (Convert.ToInt32(cmbDepartment.SelectedIndex) != -1)
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.DEPARTMENT_ID, cmbDepartment.SelectedValue.ToString()));
                List<UsersDTO> userDTOList = usersList.GetAllUsers(searchParameter,false);

                if (userDTOList != null)
                {
                    userDTOSortableList = new SortableBindingList<UsersDTO>(userDTOList);
                    foreach (UsersDTO userDTO in userDTOList)
                    {
                        Users users = new Users(machineUserContext, userDTO.UserId, true, true);
                        userDTO.CardNumber = (users.UserDTO.UserIdentificationTagsDTOList != null && users.UserDTO.UserIdentificationTagsDTOList.Count > 0) ? users.UserDTO.UserIdentificationTagsDTOList[0].CardNumber : "";
                    }
                }
                else
                {
                    userDTOSortableList = new SortableBindingList<UsersDTO>();
                }
                usersDTOBindingSource.DataSource = userDTOSortableList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                log.Error("Ends-LoadUsersDTOList() Method with an Exception:" + ex.Message + ex.StackTrace);
                log.LogMethodEntry();
            }
            log.LogMethodEntry();
        }

        void LoadUserRolesDTO()
        {
            log.LogMethodEntry();
            try
            {
                UserRolesList userRoleList = new UserRolesList();
                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> SearchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                List<UserRolesDTO> UserRoleDtoList = userRoleList.GetAllUserRoles(SearchParameters);
                if (UserRoleDtoList == null)
                {
                    UserRoleDtoList = new List<UserRolesDTO>();
                }
                UserRoleDtoList.Insert(0, new UserRolesDTO());
                UserRoleDtoList[0].Role = " ";
                UserRoleDtoList[0].RoleId = -1;

                roleIdDataGridViewComboBoxColumn.DataSource = UserRoleDtoList;
                roleIdDataGridViewComboBoxColumn.DisplayMember = "Role";
                roleIdDataGridViewComboBoxColumn.ValueMember = "RoleId";

                attendanceRoleIdDataGridViewTextBoxColumn.DataSource = UserRoleDtoList;
                attendanceRoleIdDataGridViewTextBoxColumn.DisplayMember = "Role";
                attendanceRoleIdDataGridViewTextBoxColumn.ValueMember = "RoleId";
            }
            catch (Exception ex)
            {
                log.Error("Ends with error -LoadUserRolesDTO() :" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadDepartment()
        {
            log.LogMethodEntry();
            DepartmentList departmentList = new DepartmentList(machineUserContext);
            List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<DepartmentDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.ISACTIVE, "Y"));
            searchParameter.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.SITE_ID, utilities.ParafaitEnv.IsCorporate == true ? utilities.ParafaitEnv.SiteId.ToString() : "-1"));
            List<DepartmentDTO> departmentDTOList = departmentList.GetDepartmentDTOList(searchParameter);

            if (departmentDTOList != null && departmentDTOList.Any())
            {
                departmentDTOList.Insert(0, new DepartmentDTO());
                departmentDTOList[0].DepartmentName = " ";
                departmentDTOList[0].DepartmentId = -1;

                cmbDepartment.DataSource = departmentDTOList;
                cmbDepartment.DisplayMember = "departmentName";
                cmbDepartment.ValueMember = "departmentId";
            }
            log.LogMethodExit();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadUsersDTOList();
            log.LogMethodExit();
        }

        private void BtnrefreshSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtLoginId.Text = "";
            txtEmployeeNo.Text = "";
            cmbDepartment.SelectedIndex = -1;
            LoadUsersDTOList();
            log.LogMethodExit();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        private void DgvSearch_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            userID = Convert.ToInt32(dgvSearch.Rows[e.RowIndex].Cells["userIdDataGridViewTextBoxColumn"].Value);
            LoadAttendanceLogDTO(Convert.ToInt32(dgvSearch.Rows[e.RowIndex].Cells["userIdDataGridViewTextBoxColumn"].Value));
            log.LogMethodExit();
        }

        private void LoadAttendanceLogDTO(int userId)
        {
            log.LogMethodEntry(userId);
            AttendanceLogList attendanceLogList = new AttendanceLogList(machineUserContext);
            List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.USER_ID, userId.ToString()));
            if (chbActiveOnly.Checked)
                searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.ISACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.FROM_DATE, fromdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.TO_DATE, todate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            List<AttendanceLogDTO> attendanceLogDtoList = attendanceLogList.GetAllAttendanceUserList(searchParameters);

            if (attendanceLogDtoList != null)
            {
                attendanceLogDtoList = attendanceLogDtoList.OrderBy(x => x.Timestamp).ToList();
                attendanceLogDTOSortableList = new SortableBindingList<AttendanceLogDTO>(attendanceLogDtoList);
            }
            else
            {
                attendanceLogDTOSortableList = new SortableBindingList<AttendanceLogDTO>();
            }
            attendanceLogDTOBindingSource.DataSource = attendanceLogDTOSortableList;

            if (!chbActiveOnly.Checked)
            {
                if (dgvTimeSheetDetails.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvTimeSheetDetails.Rows)
                        if (row.Cells["isActiveDataGridViewTextBoxColumn"].Value.ToString() == "N")
                        {
                            row.DefaultCellStyle.BackColor = Color.Red;
                            row.ReadOnly = true;
                        }
                }
            }

            log.LogMethodExit();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            attendanceLogDTOSortableList = (SortableBindingList<AttendanceLogDTO>)attendanceLogDTOBindingSource.DataSource;
            if (attendanceLogDTOSortableList != null)
            {
                for (int i = 0; i < attendanceLogDTOSortableList.Count; i++)
                {
                    if (attendanceLogDTOSortableList[i].IsChanged)
                    {
                        try
                        {
                            DateTime updatedTime = attendanceLogDTOSortableList[i].Timestamp;

                            //Inactivates the existing record
                            attendanceLogDTOSortableList[i].IsActive = "N";
                            attendanceLogDTOSortableList[i].Timestamp = oldTime;
                            SaveAttendanceLog(attendanceLogDTOSortableList[i]);

                            //Creates a new record
                            AttendanceLogDTO newAttendanceLogDTO = new AttendanceLogDTO();
                            newAttendanceLogDTO = attendanceLogDTOSortableList[i];
                            newAttendanceLogDTO.AttendanceLogId = -1;
                            newAttendanceLogDTO.Timestamp = updatedTime;
                            newAttendanceLogDTO.IsActive = "Y";
                            SaveAttendanceLog(newAttendanceLogDTO);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error while saving event." + ex.Message);
                            dgvTimeSheetDetails.Rows[i].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(718));
                            break;
                        }
                    }
                }
                LoadAttendanceLogDTO(userID);
            }
            log.LogMethodExit();
        }

        private void SaveAttendanceLog(AttendanceLogDTO attendanceLogDTO)
        {
            log.LogMethodEntry();
            AttendanceLogBL attendanceLogBL = new AttendanceLogBL(machineUserContext, attendanceLogDTO);
            attendanceLogBL.Save();
            log.LogMethodExit();
        }

        private void BtnRefreshSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadAttendanceLogDTO(userID);
            log.LogMethodExit();
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
            todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
            LoadAttendanceLogDTO(userID);
            log.LogMethodExit();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadAttendanceLogDTO(userID);
            log.LogMethodExit();
        }

        private void DgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex != -1)
            {
                grpboxEmployeeDetails.Text = "TimeSheet Details - " + dgvSearch.Rows[e.RowIndex].Cells["loginIdDataGridViewTextBoxColumn"].Value;
                userID = Convert.ToInt32(dgvSearch.Rows[e.RowIndex].Cells["userIdDataGridViewTextBoxColumn"].Value);
                LoadAttendanceLogDTO(userID);
            }

            log.LogMethodExit();
        }

        private void DgvTimeSheetDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == dgvTimeSheetDetails.Rows[e.RowIndex].Cells["timestampDataGridViewTextBoxColumn"].ColumnIndex)
            {
                MessageBox.Show(e.Exception.Message);
                log.Error(e.Exception.Message);
            }
            log.LogMethodExit();
        }
        private void TxtLoginId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtLoginId;
            log.LogMethodExit(null);
        }

        private void TxtEmployeeNo_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtEmployeeNo;
            log.LogMethodExit(null);
        }

        AlphaNumericKeyPad keypad;
        private void BtnShowKeyPad_Click(object sender, EventArgs e)
        {
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, keypad == null ? currentTextBox : keypad.currentTextBox);
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Bottom - keypad.Height + 40 );
                keypad.Show();
            }
            else if (keypad.Visible)
            {
                keypad.Hide();
            }
            else
            {
                keypad.Show();
            }
        }

        private void DgvTimeSheetDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == dgvTimeSheetDetails.Rows[e.RowIndex].Cells["timestampDataGridViewTextBoxColumn"].ColumnIndex)
                {
                    oldTime = Convert.ToDateTime(dgvTimeSheetDetails.Rows[e.RowIndex].Cells["timestampDataGridViewTextBoxColumn"].Value);
                }
            }
            log.LogMethodExit(null);
        }

        private void DgvTimeSheetDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvTimeSheetDetails.CurrentCell.ColumnIndex == timestampDataGridViewTextBoxColumn.Index)//select target column
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    if (keypad != null)
                        keypad.currentTextBox = textBox;
                }
            }
        }

       

        private void dgvTimeSheetDetails_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == timestampDataGridViewTextBoxColumn.Index)
            {
                if (e != null)
                {
                    if (e.Value != null)
                    {
                        try
                        {
                            DateTime dt;
                            e.ParsingApplied = DateTime.TryParseExact(e.Value.ToString(), dateTimeFormat, null, DateTimeStyles.None, out dt);
                            if(e.ParsingApplied)
                            {
                                e.Value = dt;
                            }
                        }
                        catch (FormatException)
                        {
                            // Set to false in case another CellParsing handler
                            // wants to try to parse this DataGridViewCellParsingEventArgs instance.
                            e.ParsingApplied = false;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
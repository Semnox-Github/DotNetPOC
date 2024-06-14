/********************************************************************************************
 * Project Name - Manage Attendance UI
 * Description  - UI of Attendance Manage
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018   Indhu                   Created 
 *2.70.2      15-Jul-2019   Girish Kundar           Modified :Added LogmethodEntry() and LogMethodExit() 
 *2.70.2      18-Dec-2019   Jinto Thomas            Added parameter execution context for userbl declaration with userid 
 *2.130.0     26-Jun-2021   Deeksha                 Modified as part of Attendance Modification enhancements
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
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    public partial class ManageAttendanceUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private int userID = -1;
        private int loggedInUserId;
        private ExecutionContext executionContext;
        private TextBox currentTextBox;
        private AlphaNumericKeyPad keypad;
        private List<AttendanceLogDTO> attendanceLogDtoList;
        private List<AttendanceDTO> attendanceDTOList;
        private string approvedBy = string.Empty;
        private List<AttendanceLogDTO> attendanceLogDTOSortableList = new List<AttendanceLogDTO>();
        private double noOfAttendanceModificationDays;


        public ManageAttendanceUI(Utilities utilities, int loggedInUserId)
        {
            log.LogMethodEntry(utilities, loggedInUserId);
            InitializeComponent();
            this.loggedInUserId = loggedInUserId;
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            currentTextBox = txtEmpName;
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void AttendanceModificationUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetUIElementsFontAndFormat();
            LoadDepartment();
            LoadUsersDTO();
            LoadApproveSearchLogDetails();
            bool isManagerLogin = IsManagerLogin();
            if (isManagerLogin)
            {
                LoadAttendanceUserRolesDTO();
                DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3064), "MESSAGE");//"Approve or Reject modification request.."
            }
            else
            {
                tabAttendance.TabPages.Remove(tabApprove);
                tabAttendance.TabPages.Remove(tabSearch);
                DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3065), "MESSAGE");//Add or Edit Clock In/Out records..
                userID = loggedInUserId;
                btnShowKeyPad.Visible = false;
                LoadUserAttendanceLogStatus(dtpLogFromDate.Value, dtpLogToDate.Value);
            }
            LoadAttendanceLog();
            log.LogMethodExit();
        }

        private void SetUIElementsFontAndFormat()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvAttendanceLog);
            utilities.setupDataGridProperties(ref dgvApproveSearch);
            utilities.setupDataGridProperties(ref dgvSearch);
            utilities.setupDataGridProperties(ref dgvUserLog);
            SetDGVCellFont(dgvAttendanceLog);
            SetDGVCellFont(dgvSearch);
            SetDGVCellFont(dgvUserLog);
            SetDGVCellFont(dgvApproveSearch);
            dtpDate.CustomFormat = dtpFromDate.CustomFormat = dtpToDate.CustomFormat = dtpLogFromDate.CustomFormat
                                            = dtpLogToDate.CustomFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            timestampDataGridViewTextBoxColumn.DefaultCellStyle.Format = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
            shiftStartTime.DefaultCellStyle.Format = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            WorkShiftStartTime.DefaultCellStyle.Format = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            dgvAttendanceLog.RowTemplate.Resizable = DataGridViewTriState.True;
            dgvAttendanceLog.RowTemplate.Height = 40;
            dgvApproveSearch.RowTemplate.Height = dgvSearch.RowTemplate.Height = 40;
            dgvSearch.RowTemplate.Height = dgvSearch.RowTemplate.Height = 40;
            dgvUserLog.RowTemplate.Height = dgvSearch.RowTemplate.Height = 40;
            dgvApproveSearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvAttendanceLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUserLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvApproveSearch.ColumnHeadersHeight = dgvAttendanceLog.ColumnHeadersHeight = 40;
            dgvSearch.ColumnHeadersHeight = dgvUserLog.ColumnHeadersHeight = 40;
            dgvAttendanceLog.SelectionMode = dgvApproveSearch.SelectionMode = dgvSearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtpDate.Value = utilities.getServerTime().Date;
            noOfAttendanceModificationDays = Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_ATTENDANCE_MODIFICATION_WITHIN_X_DAYS"));
            dtpToDate.Value = dtpDate.Value;
            dtpFromDate.Value = dtpToDate.Value.AddDays(-noOfAttendanceModificationDays);
            dtpLogToDate.Value = dtpDate.Value;
            dtpLogFromDate.Value = dtpToDate.Value.AddDays(-noOfAttendanceModificationDays);
            log.LogMethodExit();
        }

        private bool IsManagerLogin()
        {
            log.LogMethodEntry();
            bool ismanager = false;
            Users userBL = new Users(executionContext, loggedInUserId);
            if (userBL.UserDTO != null)
            {
                UserRoles userRoles = new UserRoles(executionContext, userBL.UserDTO.RoleId);
                if (userRoles.getUserRolesDTO != null && userRoles.getUserRolesDTO.ManagerFlag == "Y")
                {
                    ismanager = true;
                }
            }
            log.LogMethodExit(ismanager);
            return ismanager;
        }

        private void LoadUsersDTOList()
        {
            log.LogMethodEntry();
            try
            {
                SortableBindingList<UsersDTO> userDTOSortableList;
                UsersList usersList = new UsersList(executionContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                if (!string.IsNullOrEmpty(txtEmpName.Text.ToString()))
                {
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_FIRST_OR_LAST_NAME, txtEmpName.Text));
                }
                if (!string.IsNullOrEmpty(txtLoginId.Text.ToString()))
                {
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, txtLoginId.Text));
                }
                if (!string.IsNullOrEmpty(txtEmpNumber.Text.ToString()))
                {
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.EMP_NUMBER, txtEmpNumber.Text));
                }
                if (cmbDepartment.SelectedValue != null && Convert.ToInt32(cmbDepartment.SelectedValue) != -1)
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.DEPARTMENT_ID, cmbDepartment.SelectedValue.ToString()));
                List<UsersDTO> userDTOList = usersList.GetAllUsers(searchParameter, false);

                if (userDTOList != null)
                {
                    userDTOSortableList = new SortableBindingList<UsersDTO>(userDTOList);
                    foreach (UsersDTO userDTO in userDTOList)
                    {
                        Users users = new Users(executionContext, userDTO.UserId, true, true);
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
                DisplayMessageLine(ex.Message, "ERROR");
                log.Error(ex);
            }
            log.LogMethodEntry();
        }

        private void LoadDepartment()
        {
            log.LogMethodEntry();
            DepartmentList departmentList = new DepartmentList(executionContext);
            List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<DepartmentDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.ISACTIVE, "Y"));
            searchParameter.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.SITE_ID, utilities.ParafaitEnv.IsCorporate == true ? utilities.ParafaitEnv.SiteId.ToString() : "-1"));
            List<DepartmentDTO> departmentDTOList = departmentList.GetDepartmentDTOList(searchParameter);

            if (departmentDTOList != null && departmentDTOList.Any())
            {
                departmentDTOList.Insert(0, new DepartmentDTO());
                departmentDTOList[0].DepartmentName = "All";
                departmentDTOList[0].DepartmentId = -1;

                cmbDepartment.DataSource = departmentDTOList;
                cmbDepartment.DisplayMember = "departmentName";
                cmbDepartment.ValueMember = "departmentId";

                cmbDept.DataSource = departmentDTOList;
                cmbDept.DisplayMember = "departmentName";
                cmbDept.ValueMember = "departmentId";
            }
            log.LogMethodExit();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadUsersDTOList();
            log.LogMethodExit();
        }

        private void BtnrefreshSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtEmpName.Text = string.Empty;
            cmbDepartment.SelectedIndex = -1;
            LoadUsersDTOList();
            log.LogMethodExit();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (attendanceLogDTOSortableList != null && attendanceLogDTOSortableList.Any() && attendanceDTOList.Exists(x => x.IsChangedRecursive))
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(executionContext, 3092), MessageContainerList.GetMessage(executionContext, "Attendance"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                this.Close();
            }
            else
            {
                this.Close();
            }
            log.LogMethodExit();
        }

        private List<UserRolesDTO> LoadAttendanceUserRolesDTO()
        {
            log.LogMethodEntry();
            List<UserRolesDTO> userRolesDTOList = new List<UserRolesDTO>();
            try
            {
                if (userID == -1)
                {
                    log.LogMethodExit();
                    return userRolesDTOList;
                }
                UserRolesList userRolesList = new UserRolesList(executionContext);
                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchByParams = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                searchByParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                userRolesDTOList = userRolesList.GetAllUserRoles(searchByParams);
                if (userRolesDTOList == null)
                {
                    userRolesDTOList = new List<UserRolesDTO>();
                }
                userRolesDTOList.Insert(0, new UserRolesDTO());
                userRolesDTOList[0].Role = string.Empty;
                userRolesDTOList[0].RoleId = -1;

                attendanceRoleIdDataGridViewTextBoxColumn.DataSource = userRolesDTOList;
                attendanceRoleIdDataGridViewTextBoxColumn.DisplayMember = "Role";
                attendanceRoleIdDataGridViewTextBoxColumn.ValueMember = "RoleId";
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit(userRolesDTOList);
            return userRolesDTOList;
        }

        private void LoadUsersDTO()
        {
            log.LogMethodEntry();
            try
            {
                UsersList usersList = new UsersList(executionContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                List<UsersDTO> userDTOList = usersList.GetAllUsers(searchParameter, false);

                if (userDTOList == null)
                {
                    userDTOList = new List<UsersDTO>();
                }
                approvedBy = userDTOList.Find(x => x.UserId == loggedInUserId).UserName;

                userDTOList.Insert(0, new UsersDTO());
                userDTOList[0].UserName = string.Empty;
                userDTOList[0].UserId = -1;

                cmbUserName.DataSource = userDTOList;
                cmbUserName.DisplayMember = "UserName";
                cmbUserName.ValueMember = "UserId";

                cmbUserLog.DataSource = userDTOList;
                cmbUserLog.DisplayMember = "UserName";
                cmbUserLog.ValueMember = "UserId";
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void LoadAttendanceLog()
        {
            log.LogMethodEntry();
            try
            {
                EnableDisableButtons();
                ClearDataSource();
                List<UserRolesDTO> userRolesDTOList = LoadAttendanceUserRolesDTO();
                AttendanceList attendanceList = new AttendanceList(executionContext);
                List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.USER_ID, userID.ToString()));
                searchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.FROM_DATE, dtpDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.TO_DATE, dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<AttendanceDTO> attendanceDTOList = attendanceList.GetAllAttendance(searchParameters, true, false);
                if (attendanceDTOList != null && attendanceDTOList.Any() && attendanceDTOList[0].AttendanceLogDTOList.Any())
                {
                    List<AttendanceLogDTO> attendanceLogDTOList = new List<AttendanceLogDTO>();
                    //attendanceDTOList = attendanceDTOList[0];
                    this.attendanceDTOList = attendanceDTOList;
                    foreach (AttendanceDTO attendanceDTO in attendanceDTOList)
                    {
                        attendanceLogDTOList.AddRange(attendanceDTO.AttendanceLogDTOList);
                    }
                    this.attendanceLogDtoList = attendanceLogDTOList;
                    //attendanceLogDtoList = attendanceDTO.AttendanceLogDTOList;
                    foreach (AttendanceLogDTO logDTO in attendanceLogDTOList)
                    {
                        if ((logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Rejected.ToString()
                                || logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString()
                                || logDTO.IsActive == "N"))
                        {
                            continue;
                        }
                        if (attendanceLogDtoList.Exists(x => x.OriginalAttendanceLogId == logDTO.AttendanceLogId))
                        {
                            List<AttendanceLogDTO> logDTOList = attendanceLogDtoList.FindAll(x => x.OriginalAttendanceLogId == logDTO.AttendanceLogId).ToList();
                            if (logDTOList != null && logDTOList.Exists(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString()))
                                continue;
                        }
                        logDTO.Notes = logDTO.Notes + logDTO.Remarks;
                        logDTO.AcceptChanges();
                        attendanceLogDTOSortableList.Add(logDTO);
                    }
                }

                attendanceLogDTOBindingSource.DataSource = attendanceLogDTOSortableList.OrderBy(x => x.Timestamp).ToList();
                dgvAttendanceLog.ClearSelection();
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void EnableDisableButtons()
        {
            log.LogMethodEntry();
            btnAddClockInOut.Enabled = btnEditClockInOut.Enabled = true;
            btnSubmitApproval.Enabled = true;
            btnApprove.Visible = false;
            btnReject.Visible = false;
            btnSubmitApproval.Visible = true;
            btnSubmitApproval.Location = btnApprove.Location;
            btnClose.Location = btnReject.Location;
            if (IsManagerLogin())
            {
                if (userID != -1 && userID != loggedInUserId)
                {
                    btnSubmitApproval.Visible = false;
                    btnApprove.Visible = btnReject.Visible = true;
                    btnApprove.Enabled = btnReject.Enabled = true;
                    btnClose.Location = new Point(656, 442);
                }
            }
            btnAddClockInOut.BackgroundImage = btnEditClockInOut.BackgroundImage = btnSubmitApproval.BackgroundImage = btnApprove.BackgroundImage = btnReject.BackgroundImage = Properties.Resources.normal2;
            ModifyAttendanceValidation();
            log.LogMethodExit();
        }

        private void BtnAddClockInOut_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_POS_ATTENDANCE").ToString() == "N")
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3071), "ERROR"); //ENABLE_POS_ATTENDANCE is disabled. Cannot record attendance
                    return;
                }
                if (userID == -1)
                {
                    LoadAttendanceUserRolesDTO();
                    userID = loggedInUserId;
                }
                string cardNumber = string.Empty;
                Users users = new Users(executionContext, userID, true, true);
                UsersDTO usersDTO = users.UserDTO;
                if (usersDTO == null || (usersDTO != null && usersDTO.UserIdentificationTagsDTOList.Any() == false))
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3066), "ERROR");// "User does not have a valid card";
                    return;
                }
                if (usersDTO.UserIdentificationTagsDTOList != null && usersDTO.UserIdentificationTagsDTOList.Any())
                {
                    List<UserIdentificationTagsDTO> userIdentificationTags = usersDTO.UserIdentificationTagsDTOList.OrderBy(x => (utilities.getServerTime() > (x.StartDate == null ? utilities.getServerTime() : x.StartDate) && utilities.getServerTime() < (x.EndDate == null ? utilities.getServerTime() : x.EndDate)) && x.AttendanceReaderTag).ToList();
                    if (userIdentificationTags != null && userIdentificationTags.Count == 0)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3066));//  "User does not have a valid card"
                        //this.DialogResult = DialogResult.Cancel;
                        return;
                    }
                }
                cardNumber = usersDTO.UserIdentificationTagsDTOList[0].CardNumber;
                if (string.IsNullOrEmpty(cardNumber))
                {
                    List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = usersDTO.UserIdentificationTagsDTOList.FindAll(x => x.CardNumber != string.Empty).ToList();
                    if (userIdentificationTagsDTOList == null || userIdentificationTagsDTOList.Any() == false)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3066));//  "User does not have a valid card"
                        //this.DialogResult = DialogResult.Cancel;
                        return;
                    }
                    cardNumber = userIdentificationTagsDTOList[0].CardNumber;
                }
                if (attendanceDTOList == null || attendanceDTOList.Any() == false)
                {
                    attendanceDTOList = new List<AttendanceDTO>();
                    DateTime selectedDate = dtpDate.Value;
                    int StartHour = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_WORKSHIFT_STARTTIME")) ? 6 : Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_WORKSHIFT_STARTTIME"));
                    if (selectedDate.Hour < StartHour)
                    {
                        selectedDate = selectedDate.AddDays(-1);
                    }
                    selectedDate = selectedDate.Date.AddHours(StartHour);
                    attendanceDTOList.Add(new AttendanceDTO(-1, userID, selectedDate, -1, selectedDate, 0, "OPEN", "Y"));
                }

                AttendanceLogDTO attendanceLogDTO = new AttendanceLogDTO(-1, cardNumber, -1, dtpDate.Value, string.Empty, -1, "CARD", -1,
                                                                loggedInUserId, string.Empty, -1, executionContext.GetMachineId(), 0, "Y", -1, string.Empty, null, null, usersDTO.UserId, null, null);

                using (AttendanceModificationUI modifyAttendance = new AttendanceModificationUI(utilities, attendanceLogDTO, null, dtpDate.Value))
                {
                    modifyAttendance.ShowDialog();
                    if (modifyAttendance.DialogResult == DialogResult.OK)
                    {
                        AttendanceLogDTO modifiedLogDTO = modifyAttendance.GetAttendanceLogDTO;
                        if (modifiedLogDTO != null && !attendanceLogDtoList.Contains(modifiedLogDTO) && modifiedLogDTO.IsActive == "N")
                        {
                            attendanceLogDTOSortableList.Remove(modifiedLogDTO);
                            attendanceLogDtoList.Remove(modifiedLogDTO);
                        }
                        else if (modifiedLogDTO != null && !attendanceLogDtoList.Contains(modifiedLogDTO))
                        {
                            attendanceLogDtoList.Add(modifiedLogDTO);
                            attendanceLogDTOSortableList.Add(modifiedLogDTO);
                        }
                        attendanceLogDTOBindingSource.DataSource = attendanceLogDTOSortableList.OrderBy(x => x.Timestamp).ToList();
                    }
                    dgvAttendanceLog.Refresh();
                    dgvAttendanceLog.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message, "ERROR");
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void BtnEditClockInOut_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DisplayMessageLine(string.Empty);
                if (dgvAttendanceLog.SelectedRows.Count > 0)
                {
                    AttendanceLogDTO attendanceLogDTO = (AttendanceLogDTO)dgvAttendanceLog.SelectedRows[0].DataBoundItem;
                    if (attendanceLogDTO != null)
                    {
                        AttendanceLogDTO modifiedLogDTO;
                        AttendanceLogDTO originalLogDTO = attendanceLogDTO;
                        if (attendanceLogDTO.OriginalAttendanceLogId > -1 && (attendanceLogDTO.RequestStatus != AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()))
                        {
                            modifiedLogDTO = attendanceLogDTO;
                            originalLogDTO = attendanceLogDtoList.Find(x => x.AttendanceLogId == attendanceLogDTO.OriginalAttendanceLogId);
                        }
                        else if (attendanceLogDTO.AttendanceLogId == -1 && attendanceLogDTO.OriginalAttendanceLogId == -1)
                        {
                            modifiedLogDTO = attendanceLogDTO;
                            originalLogDTO = null;
                        }
                        else
                        {
                            modifiedLogDTO = new AttendanceLogDTO(-1, attendanceLogDTO.CardNumber, attendanceLogDTO.ReaderId, attendanceLogDTO.Timestamp,
                                                                    attendanceLogDTO.Type, attendanceLogDTO.AttendanceId, attendanceLogDTO.Mode, attendanceLogDTO.AttendanceRoleId,
                                                                    attendanceLogDTO.AttendanceRoleApproverId, attendanceLogDTO.Status, attendanceLogDTO.MachineId, attendanceLogDTO.POSMachineId,
                                                                    attendanceLogDTO.TipValue, attendanceLogDTO.IsActive, attendanceLogDTO.AttendanceLogId, string.Empty, null, null,
                                                                    userID, null, null);
                        }
                        AttendanceModify(originalLogDTO, modifiedLogDTO);
                    }
                }
                else
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 2460), "MESSAGE");//Please select a record to proceed
                }
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message, "ERROR");
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void AttendanceModify(AttendanceLogDTO originalLogDTO, AttendanceLogDTO modifiedLogDTO)
        {
            log.LogMethodEntry(originalLogDTO, modifiedLogDTO);
            using (AttendanceModificationUI modifyAttendance = new AttendanceModificationUI(utilities, modifiedLogDTO, originalLogDTO, dtpDate.Value))
            {
                modifyAttendance.ShowDialog();
                modifiedLogDTO = modifyAttendance.GetAttendanceLogDTO;
                if (modifyAttendance.DialogResult == DialogResult.OK)
                {
                    if (modifiedLogDTO != null && !attendanceLogDtoList.Contains(modifiedLogDTO))
                    {
                        attendanceLogDtoList.Add(modifiedLogDTO);
                        attendanceLogDTOSortableList.Remove(attendanceLogDtoList.Find(x => x.AttendanceLogId == modifiedLogDTO.OriginalAttendanceLogId));
                        attendanceLogDTOSortableList.Add(modifiedLogDTO);

                    }
                }
                else if (modifyAttendance.DialogResult == DialogResult.Abort && modifiedLogDTO != null && attendanceLogDtoList.Contains(modifiedLogDTO)
                                    && modifiedLogDTO.IsActive == "N")
                {
                    attendanceLogDTOSortableList.Remove(modifiedLogDTO);
                    //attendanceLogDtoList.Remove(modifiedLogDTO);
                    if (modifiedLogDTO.OriginalAttendanceLogId != -1 &&
                                attendanceLogDtoList.Exists(x => x.AttendanceLogId == modifiedLogDTO.OriginalAttendanceLogId))
                    {
                        attendanceLogDTOSortableList.Add(attendanceLogDtoList.Find(x => x.AttendanceLogId == modifiedLogDTO.OriginalAttendanceLogId));
                        //modifiedLogDTO.AcceptChanges();
                    }
                }
                attendanceLogDTOBindingSource.DataSource = attendanceLogDTOSortableList.OrderBy(x => x.Timestamp).ToList();
                dgvAttendanceLog.Refresh();
                dgvAttendanceLog.ClearSelection();
            }
            log.LogMethodExit();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction dbtrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (attendanceLogDTOSortableList.Exists(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString()))
                    {
                        attendanceLogDTOSortableList.Find(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString()).RequestStatus = null;
                    }
                    dbtrx.BeginTransaction();
                    if (attendanceDTOList != null && attendanceDTOList.Count > 1)
                    {
                        foreach (AttendanceDTO attendanceDTO in attendanceDTOList)
                        {
                            try
                            {
                                AttendanceLogDTO logDTO = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceRoleId != 0 || x.AttendanceRoleId != -1).FirstOrDefault();
                                if (logDTO != null)
                                {
                                    int roleId = logDTO.AttendanceRoleId;
                                    List<AttendanceLogDTO> logDTOList = attendanceLogDtoList.FindAll(x => x.AttendanceRoleId == roleId);
                                    attendanceDTO.AttendanceLogDTOList = logDTOList;
                                    AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTO);
                                    attendanceBL.ApproveAttendance(approvedBy, dbtrx.SQLTrx);
                                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3090), "MESSAGE"); //Attendance Approval is successful
                                }
                            }
                            catch (Exception ex)
                            {
                                attendanceDTO.AttendanceLogDTOList = attendanceLogDtoList;
                                log.Error(ex);
                            }
                        }
                    }
                    else if (attendanceDTOList != null)
                    {
                        attendanceDTOList[0].AttendanceLogDTOList = attendanceLogDtoList;
                        AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTOList[0]);
                        attendanceBL.ApproveAttendance(approvedBy, dbtrx.SQLTrx);
                        DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3090), "MESSAGE"); //Attendance Approval is successful
                    }
                    dbtrx.EndTransaction();
                    LoadAttendanceLog();
                    dgvAttendanceLog.ClearSelection();
                }
                catch (ValidationException exp)
                {
                    dbtrx.RollBack();
                    try
                    {
                        if (exp.ValidationErrorList[0].EntityId != -1)
                        {
                            attendanceLogDtoList.Find(x => x.AttendanceLogId == exp.ValidationErrorList[0].EntityId).RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString();
                        }
                        else
                        {
                            DateTime time = Convert.ToDateTime(exp.ValidationErrorList[0].EntityName);
                            AttendanceLogDTO attendanceLogDTO = attendanceLogDtoList.Find(x => x.Timestamp == time);
                            if (attendanceLogDTO != null)
                                attendanceLogDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    DisplayMessageLine(exp.Message, "ERROR");
                    dgvAttendanceLog.Refresh();
                    dgvAttendanceLog.ClearSelection();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    dbtrx.RollBack();
                }
            }
            log.LogMethodExit();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DisplayMessageLine(string.Empty);
            if (attendanceDTOList != null && attendanceDTOList.Exists(x => x.AttendanceLogDTOList.Any()))
            {
                using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        dbTrx.BeginTransaction();
                        if (attendanceDTOList != null && attendanceDTOList.Count > 1)
                        {
                            foreach (AttendanceDTO attendanceDTO in attendanceDTOList)
                            {
                                try
                                {
                                    AttendanceLogDTO logDTO = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceRoleId != 0 || x.AttendanceRoleId != -1).FirstOrDefault();
                                    if (logDTO != null)
                                    {
                                        int roleId = logDTO.AttendanceRoleId;
                                        List<AttendanceLogDTO> logDTOList = attendanceLogDtoList.FindAll(x => x.AttendanceRoleId == roleId);
                                        attendanceDTO.AttendanceLogDTOList = logDTOList;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    attendanceDTO.AttendanceLogDTOList = attendanceLogDtoList;
                                }
                                AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTO);
                                attendanceBL.RejectAttendance(approvedBy, dbTrx.SQLTrx);
                                DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3091), "MESSAGE"); //Attendance Rejected is successfuly
                            }
                        }
                        else if (attendanceDTOList != null)
                        {
                            attendanceDTOList[0].AttendanceLogDTOList = attendanceLogDtoList;
                            AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTOList[0]);
                            attendanceBL.RejectAttendance(approvedBy, dbTrx.SQLTrx);
                            DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3091), "MESSAGE"); //Attendance Approval is successful
                        }
                        dbTrx.EndTransaction();
                        LoadAttendanceLog();
                    }
                    catch (Exception ex)
                    {
                        DisplayMessageLine(ex.Message, "ERROR");
                        dbTrx.RollBack();
                        log.Error(ex);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DisplayMessageLine(string.Empty);
                if (attendanceLogDTOSortableList != null && attendanceLogDTOSortableList.Any() && attendanceDTOList.Exists(x => x.IsChangedRecursive))
                {    //There are unsaved records.Do you want to continue?
                    if (MessageBox.Show(MessageContainerList.GetMessage(executionContext, 3092), MessageContainerList.GetMessage(executionContext, "Attendance"),
                        MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                if (dtpDate.Value > utilities.getServerTime())
                {
                    ClearDataSource();
                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3068), "ERROR"); //"Attendance modification for future date is not allowed;
                    btnAddClockInOut.Enabled = btnEditClockInOut.Enabled = btnSubmitApproval.Enabled = btnApprove.Enabled = btnReject.Enabled = false;
                    btnAddClockInOut.BackgroundImage = btnEditClockInOut.BackgroundImage = btnSubmitApproval.BackgroundImage = btnApprove.BackgroundImage = btnReject.BackgroundImage = Properties.Resources.button_pressed;
                    return;
                }
                ModifyAttendanceValidation();
                LoadAttendanceLog();
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void DgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisplayMessageLine(string.Empty);
            if (e.RowIndex != -1)
            {
                if (attendanceLogDtoList != null && attendanceLogDtoList.Any() && attendanceLogDtoList.Exists(x => x.IsChanged))
                {  //There are unsaved records.Do you want to continue?
                    if (MessageBox.Show(MessageContainerList.GetMessage(executionContext, 3092), MessageContainerList.GetMessage(executionContext, "Attendance"),
                        MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                grpboxEmployeeDetails.Text = MessageContainerList.GetMessage(executionContext, "TimeSheet details for ") + dgvSearch.Rows[e.RowIndex].Cells["userNameDataGridViewTextBoxColumn"].Value;
                userID = Convert.ToInt32(dgvSearch.Rows[e.RowIndex].Cells["userIdDataGridViewTextBoxColumn"].Value);
                LoadAttendanceLog();
            }
            log.LogMethodExit();
        }

        private void DgvApproveSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.RowIndex != -1)
                {
                    //There are unsaved records.Do you want to continue?
                    if (attendanceLogDtoList != null && attendanceLogDtoList.Any() && attendanceLogDtoList.Exists(x => x.IsChanged))
                    {
                        if (MessageBox.Show(MessageContainerList.GetMessage(executionContext, 3092), MessageContainerList.GetMessage(executionContext, "Attendance"),
                            MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    attendanceLogDtoList = new List<AttendanceLogDTO>();
                    attendanceLogDTOSortableList = new List<AttendanceLogDTO>();
                    grpboxEmployeeDetails.Text = MessageContainerList.GetMessage(executionContext, "TimeSheet Details For ") + dgvApproveSearch.Rows[e.RowIndex].Cells["cmbUserName"].FormattedValue;
                    userID = Convert.ToInt32(dgvApproveSearch.Rows[e.RowIndex].Cells["cmbUserName"].Value);
                    dtpDate.Value = Convert.ToDateTime(dgvApproveSearch.Rows[e.RowIndex].Cells["WorkShiftStartTime"].Value);
                    LoadAttendanceLog();
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void TxtEmpName_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtEmpName;
            log.LogMethodExit(null);
        }

        private void BtnShowKeyPad_Click(object sender, EventArgs e)
        {
            try
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, keypad == null ? currentTextBox : keypad.currentTextBox);
                    keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Bottom - keypad.Height + 40);
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
            catch (Exception ex) { log.Error(ex); }
        }

        private void dgvAttendanceLog_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void dgvAttendanceLog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            DisplayMessageLine(string.Empty);
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridView dg = (DataGridView)sender;
                    if ((dg.Columns[e.ColumnIndex].Name == "RequestStatus"))
                    {
                        LaunchAttendanceModificationUI(dg);
                    }
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void dgvAttendanceLog_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {

                foreach (DataGridViewRow row in dgvAttendanceLog.Rows)
                {
                    string requestStatus = row.Cells["RequestStatus"].Value != null ? row.Cells["RequestStatus"].Value.ToString() : string.Empty;
                    int logID = Convert.ToInt32(row.Cells["attendanceLogIdDataGridViewTextBoxColumn"].Value);
                    bool isChanged = attendanceLogDTOSortableList.Find(x => x.AttendanceLogId == logID).IsChanged;
                    if (requestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString())
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else if (requestStatus == AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString())
                    {
                        row.DefaultCellStyle.BackColor = Color.OrangeRed;
                    }
                    else if (logID == -1 || isChanged)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightSalmon;
                    }
                    else if (requestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString())
                    {
                        row.DefaultCellStyle.BackColor = Color.Gold;
                    }

                }

            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void btnSubmitApproval_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DisplayMessageLine(string.Empty);
            using (ParafaitDBTransaction dbtrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (attendanceLogDTOSortableList.Exists(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString()))
                    {
                        attendanceLogDTOSortableList.Find(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString()).RequestStatus = null;
                    }
                    dbtrx.BeginTransaction();
                    if (attendanceDTOList != null && attendanceDTOList.Count > 1)
                    {
                        foreach (AttendanceDTO attendanceDTO in attendanceDTOList)
                        {
                            try
                            {
                                AttendanceLogDTO logDTO = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceRoleId != 0 || x.AttendanceRoleId != -1).FirstOrDefault();
                                if (logDTO != null)
                                {
                                    int roleId = logDTO.AttendanceRoleId;
                                    List<AttendanceLogDTO> logDTOList = attendanceLogDtoList.FindAll(x => x.AttendanceRoleId == roleId);
                                    attendanceDTO.AttendanceLogDTOList = logDTOList;
                                }
                            }
                            catch (Exception ex)
                            {
                                attendanceDTO.AttendanceLogDTOList = attendanceLogDtoList;
                            }
                            AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTO);
                            attendanceBL.SubmitAttendanceForApproval(dbtrx.SQLTrx);
                            DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3069), "MESSAGE"); //Attendance Records submitted succefully.
                        }
                    }
                    else if(attendanceDTOList != null)
                    {
                        attendanceDTOList[0].AttendanceLogDTOList = attendanceLogDtoList;
                        AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTOList[0]);
                        attendanceBL.SubmitAttendanceForApproval(dbtrx.SQLTrx);
                        DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3069), "MESSAGE"); //Attendance Records submitted succefully.
                    }
                    dbtrx.EndTransaction();
                    LoadAttendanceLog();
                }
                catch (ValidationException exp)
                {
                    dbtrx.RollBack();
                    try
                    {
                        if (exp.ValidationErrorList.Any() && !string.IsNullOrEmpty(exp.ValidationErrorList[0].EntityName))
                        {
                            if (exp.ValidationErrorList[0].EntityId != -1)
                            {
                                attendanceLogDtoList.Find(x => x.AttendanceLogId == exp.ValidationErrorList[0].EntityId).RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString();
                            }
                            else
                            {
                                DateTime time = Convert.ToDateTime(exp.ValidationErrorList[0].EntityName);
                                AttendanceLogDTO attendanceLogDTO = attendanceLogDtoList.Find(x => x.Timestamp == time);
                                if (attendanceLogDTO != null)
                                {
                                    attendanceLogDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString();
                                    attendanceLogDTO.IsActive = "N";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    DisplayMessageLine(exp.Message, "ERROR");
                    dgvAttendanceLog.Refresh();
                    dgvAttendanceLog.ClearSelection();
                }
                catch (Exception ex)
                {
                    dbtrx.RollBack();
                }

            }
            log.LogMethodExit();
        }


        private void dgvAttendanceLog_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            DisplayMessageLine(string.Empty);
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridView dg = (DataGridView)sender;
                    LaunchAttendanceModificationUI(dg);
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void LaunchAttendanceModificationUI(DataGridView dg)
        {
            log.LogMethodEntry();
            DisplayMessageLine(string.Empty);
            AttendanceLogDTO attendanceLogDTO = (AttendanceLogDTO)dg.CurrentRow.DataBoundItem;
            if (attendanceLogDTO.OriginalAttendanceLogId != -1)
            {
                AttendanceLogDTO originalLogDTO = attendanceLogDtoList.Find(x => x.AttendanceLogId == attendanceLogDTO.OriginalAttendanceLogId);
                if (originalLogDTO != null)
                {
                    using (AttendanceModificationUI modifyAttendance = new AttendanceModificationUI(utilities, attendanceLogDTO, originalLogDTO, dtpDate.Value, true))
                    {
                        modifyAttendance.ShowDialog();
                    }
                }
            }
            dgvAttendanceLog.ClearSelection();
            log.LogMethodExit();
        }


        private void btnApproveSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisplayMessageLine(string.Empty);
                LoadApproveSearchLogDetails();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message, "ERROR");
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LoadApproveSearchLogDetails()
        {
            log.LogMethodEntry();
            List<AttendanceDTO> submittedAttendanceDTOList = new List<AttendanceDTO>();
            SortableBindingList<AttendanceDTO> attendanceDTOSortableList = new SortableBindingList<AttendanceDTO>();
            List<UsersDTO> userDTOList = null;
            UsersList usersList = new UsersList(executionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
            if (cmbDepartment.SelectedValue != null && Convert.ToInt32(cmbDepartment.SelectedValue) != -1)
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.DEPARTMENT_ID, cmbDepartment.SelectedValue.ToString()));
            if (!string.IsNullOrEmpty(txtUserName.Text))
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_FIRST_OR_LAST_NAME, txtUserName.Text.ToString()));
            if (!string.IsNullOrEmpty(txtApproveSearchEmpNo.Text))
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.EMP_NUMBER, txtApproveSearchEmpNo.Text.ToString()));
            if (!string.IsNullOrEmpty(txtApproveSearchLoginId.Text))
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, txtApproveSearchLoginId.Text.ToString()));
            userDTOList = usersList.GetAllUsers(searchParameter, false);

            if (userDTOList != null)
            {
                string userIdList = string.Join(",", userDTOList.Select(x => x.UserId));
                AttendanceList attendanceLogList = new AttendanceList(executionContext);
                List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>();
                searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.FROM_DATE, dtpFromDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.TO_DATE, dtpToDate.Value.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.USER_ID_LIST, userIdList));
                List<AttendanceDTO> attendanceDTOList = attendanceLogList.GetAllAttendance(searchByParams, true, true);
                if (attendanceDTOList != null && attendanceDTOList.Any())
                {
                    foreach (AttendanceDTO attendanceDTO in attendanceDTOList)
                    {
                        if (attendanceDTO.AttendanceLogDTOList.Exists(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString()))
                        {
                            attendanceDTO.UserFirstName = userDTOList.Find(x => x.UserId == attendanceDTO.UserId).UserName;
                            attendanceDTO.UserLastName = userDTOList.Find(x => x.UserId == attendanceDTO.UserId).EmpLastName;
                            attendanceDTO.UserLoginId = userDTOList.Find(x => x.UserId == attendanceDTO.UserId).LoginId;
                            attendanceDTO.UserEmpNum = userDTOList.Find(x => x.UserId == attendanceDTO.UserId).EmpNumber;
                            submittedAttendanceDTOList.Add(attendanceDTO);
                        }
                    }
                    submittedAttendanceDTOList = submittedAttendanceDTOList.OrderByDescending(x => x.StartDate).ToList();
                    attendanceDTOSortableList = new SortableBindingList<AttendanceDTO>(submittedAttendanceDTOList);
                }
            }
            attendanceDTOBindingSource.DataSource = attendanceDTOSortableList;
            log.LogMethodExit();
        }

        private void tabAttendance_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                TabControl tabPage = (TabControl)sender;
                DisplayMessageLine(string.Empty);
                ClearDataSource();
                if (tabPage.SelectedTab == tabUserLog)
                {
                    LoadUserAttendanceLogStatus(dtpLogFromDate.Value, dtpLogToDate.Value);
                }
                else if (tabPage.SelectedTab == tabApprove)
                {
                    LoadApproveSearchLogDetails();
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void ClearDataSource()
        {
            log.LogMethodEntry();
            attendanceLogDTOBindingSource.DataSource = null;
            attendanceDTOList = null;
            attendanceLogDtoList = new List<AttendanceLogDTO>();
            attendanceLogDTOSortableList = new List<AttendanceLogDTO>();
            log.LogMethodExit();
        }

        private void DisplayMessageLine(string message, string msgType = null)
        {
            log.LogMethodEntry(message, msgType);
            switch (msgType)
            {
                case "WARNING":
                    lblMessage.BackColor = Color.Yellow;
                    lblMessage.ForeColor = Color.Black;
                    break;
                case "ERROR":
                    lblMessage.BackColor = Color.OrangeRed;
                    lblMessage.ForeColor = Color.White;
                    break;
                case "MESSAGE":
                    lblMessage.BackColor = Color.White;
                    lblMessage.ForeColor = Color.Black;
                    break;
                default:
                    lblMessage.ForeColor = Color.Black;
                    lblMessage.BackColor = Color.White;
                    break;
            }
            lblMessage.Text = message;
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            log.LogMethodExit();
        }

        bool isDTPvaluechanged = false;
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (!isDTPvaluechanged)
                {
                    isDTPvaluechanged = true;
                    int StartHour = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_WORKSHIFT_STARTTIME")) ? 6 : Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_WORKSHIFT_STARTTIME"));
                    dtpDate.Value = dtpDate.Value.Date.AddHours(StartHour);
                    btnAddClockInOut.Enabled = btnEditClockInOut.Enabled = true;
                    if (IsManagerLogin())
                    {
                        btnApprove.Enabled = btnReject.Enabled = true;
                    }
                    else
                    {
                        btnSubmitApproval.Enabled = true;
                    }
                    btnAddClockInOut.BackgroundImage = btnEditClockInOut.BackgroundImage = btnSubmitApproval.BackgroundImage = btnApprove.BackgroundImage = btnReject.BackgroundImage = Properties.Resources.normal2;
                    DisplayMessageLine(string.Empty);
                    ModifyAttendanceValidation();
                    isDTPvaluechanged = false;
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void ModifyAttendanceValidation()
        {
            log.LogMethodEntry();
            int enteredDateRange = Convert.ToInt32(Math.Floor((utilities.getServerTime() - dtpDate.Value).TotalDays));
            if (enteredDateRange > noOfAttendanceModificationDays)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 3067, Convert.ToInt32(noOfAttendanceModificationDays)), "WARNING"); //"You can only modify the attendance for the last &1 days.";
                btnAddClockInOut.Enabled = btnEditClockInOut.Enabled = btnSubmitApproval.Enabled = btnApprove.Enabled = btnReject.Enabled = false;
                btnAddClockInOut.BackgroundImage = btnEditClockInOut.BackgroundImage = btnSubmitApproval.BackgroundImage = btnApprove.BackgroundImage = btnReject.BackgroundImage = Properties.Resources.button_pressed;
            }
            log.LogMethodExit();
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button b = sender as Button;
                b.BackgroundImage = Properties.Resources.normal2;
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button b = sender as Button;
                b.BackgroundImage = Properties.Resources.button_pressed;
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void SetDGVCellFont(DataGridView dgvInput)
        {
            log.LogMethodEntry(dgvInput);
            Font font;
            try
            {
                font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 9, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new Font("Arial", 8, FontStyle.Regular);
            }
            dgvAttendanceLog.ColumnHeadersDefaultCellStyle.Font = new Font(font.FontFamily, 9F, FontStyle.Bold);
            foreach (DataGridViewColumn c in dgvInput.Columns)
            {
                c.DefaultCellStyle.Font = new Font(font.FontFamily, 9F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }

        private void btnLogSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                LoadUserAttendanceLogStatus(dtpLogFromDate.Value, dtpLogToDate.Value);
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void LoadUserAttendanceLogStatus(DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(fromDate, toDate);
            try
            {
                AttendanceList attendanceList = new AttendanceList(executionContext);
                List<AttendanceDTO> attendanceRequestDTOList = attendanceList.GetAttendanceRequestStatusList(fromDate, toDate, loggedInUserId);
                if (attendanceRequestDTOList != null && attendanceRequestDTOList.Any())
                {
                    attendanceRequestDTOList = attendanceRequestDTOList.OrderByDescending(x => x.StartDate).ToList();
                    attendanceLogDTOBindingSource1.DataSource = attendanceRequestDTOList;
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void dgvUserLog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisplayMessageLine(string.Empty);
                if (e.RowIndex != -1)
                {
                    if (attendanceLogDtoList != null && attendanceLogDtoList.Any() && attendanceLogDtoList.Exists(x => x.IsChanged))
                    {  //There are unsaved records.Do you want to continue?
                        if (MessageBox.Show(MessageContainerList.GetMessage(executionContext, 3092), MessageContainerList.GetMessage(executionContext, "Attendance"),
                            MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    grpboxEmployeeDetails.Text = MessageContainerList.GetMessage(executionContext, "TimeSheet Details For ") + dgvUserLog.Rows[e.RowIndex].Cells["cmbUserLog"].FormattedValue;
                    dtpDate.Value = Convert.ToDateTime(dgvUserLog.Rows[e.RowIndex].Cells["shiftStartTime"].Value);
                    userID = Convert.ToInt32(dgvUserLog.Rows[e.RowIndex].Cells["cmbUserLog"].Value);
                    LoadAttendanceLog();
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void txtEmpNumber_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtEmpNumber;
            log.LogMethodExit(null);
        }

        private void txtLoginId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtLoginId;
            log.LogMethodExit(null);
        }

        private void txtApproveSearchEmpNo_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtApproveSearchEmpNo;
            log.LogMethodExit(null);
        }

        private void txtApproveSearchLoginId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtApproveSearchLoginId;
            log.LogMethodExit(null);
        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtUserName;
            log.LogMethodExit(null);
        }
    }
}
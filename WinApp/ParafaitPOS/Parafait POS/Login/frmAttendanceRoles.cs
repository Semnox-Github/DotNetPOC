/********************************************************************************************
 * Project Name - POS 
 * Description  - UI frmAttendanceRoles
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.70.3      26-Feb-2019      Mitesh            Modified : Enhancement - Auto print on Clock In/Out              
 *2.90.0      26-Jun-2020      Girish Kundar     Modified:  Phase -2 ReST API  related changes             
 *2.110.0     24-Dec-2020      Deeksha           Modified : Attendance and PayRate changes to populate userRole considering the attendance role set up.
 *2.130.0     15-Jun-2021      Deeksha           Modified : Attendance and PayRate changes to supprot attendace validation behavior.
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;
using System.Drawing.Printing;
using System.Drawing;

namespace Parafait_POS.Login
{
    public partial class frmAttendanceRoles : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int approverId = -1;
        private UsersDTO attendanceUserDTO;
        private Users userBL = null;
        private AttendanceLogDTO lastLoggedInLogDTO;
        private ExecutionContext executionContext;
        private string currentStatus = string.Empty;
        private double totalAttendanceHours = 0;
        private string cardNumber = string.Empty;
        private bool authenticationSuccess = true;

        public frmAttendanceRoles(Users user, ExecutionContext executionContext)
        {
            log.LogMethodEntry(user, executionContext);
            InitializeComponent();
            this.executionContext = executionContext;
            if (user != null)
            {
                attendanceUserDTO = user.UserDTO;
            }
            else
                attendanceUserDTO = new UsersDTO();

            if (attendanceUserDTO.UserId == -1)
            {
                Security.User User = null;
                if (Authenticate.BasicCheck(ref User, false))
                {
                    Users userBL = new Users(executionContext, User.UserId, true, true);
                    attendanceUserDTO = userBL.UserDTO;
                }
                else
                {
                    authenticationSuccess = false;
                    //this.DialogResult = DialogResult.Cancel;
                    //return;
                }

                if (User != null)
                {
                    cardNumber = User.CardNumber;
                }
            }
            log.LogMethodExit();
        }

        private void frmAttendanceRoles_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_POS_ATTENDANCE").ToString() == "N")
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 3071),  //POS Attendance is disabled.
                                                                MessageContainerList.GetMessage(executionContext, "Attendance"));
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            Security.User User = null;
            if (attendanceUserDTO.UserId == -1)
            {
                //if (Authenticate.BasicCheck(ref User, false))
                //{
                //    Users user = new Users(executionContext, User.UserId, true, true);
                //    attendanceUserDTO = user.UserDTO;
                //}
                //else
                //{
                //    this.DialogResult = DialogResult.Cancel;
                //    return;
                //}
                if(!authenticationSuccess)
                {
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }
                if (attendanceUserDTO.UserIdentificationTagsDTOList == null)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 3066), // "User does not have a valid card"
                                                    MessageContainerList.GetMessage(executionContext, "Attendance"), MessageBoxButtons.OK);
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }
            }
            List<UserIdentificationTagsDTO> userIdentificationTags = attendanceUserDTO.UserIdentificationTagsDTOList.OrderBy(x => (POSStatic.Utilities.getServerTime() > (x.StartDate == null ? POSStatic.Utilities.getServerTime() : x.StartDate) && POSStatic.Utilities.getServerTime() < (x.EndDate == null ? POSStatic.Utilities.getServerTime() : x.EndDate)) && x.AttendanceReaderTag).ToList();
            if (userIdentificationTags != null && userIdentificationTags.Count == 0)
            {
                txtMessage.Text = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 3066)).ToString(); //  "User does not have a valid card"
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            userBL = new Users(executionContext, attendanceUserDTO);
            if (User != null)
            {
                cardNumber = User.CardNumber;
            }
            if (string.IsNullOrEmpty(cardNumber) && !string.IsNullOrEmpty(userIdentificationTags[0].CardNumber))
            {
                cardNumber = userIdentificationTags[0].CardNumber;
            }
            if(string.IsNullOrEmpty(cardNumber))
            {
                userIdentificationTags = userIdentificationTags.FindAll(x => x.CardNumber != string.Empty).ToList();
                if(userIdentificationTags == null || userIdentificationTags.Any() == false)
                {
                    txtMessage.Text = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 3066)).ToString(); //  "User does not have a valid card"
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }
                cardNumber = userIdentificationTags[0].CardNumber;
            }
            List<UserRolesDTO> userRoleDtoList = InitRoles();
            if (userRoleDtoList == null)
                return;
            GetLastLogInDetails();
            if (lastLoggedInLogDTO != null && lastLoggedInLogDTO.AttendanceLogId > -1 && lastLoggedInLogDTO.Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
            {
                cmbAttendancRoles.SelectedValue = lastLoggedInLogDTO.AttendanceRoleId;
            }
            //else if(lastLoggedInLogDTO != null && 
            //    lastLoggedInLogDTO.Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT) && userRoleDtoList != null)
            //{
            //    //UserRoles userRoles = new UserRoles(executionContext, userBL.UserDTO.RoleId);
            //    //if(!userRoles.getUserRolesDTO.EnablePOSClockIn)
            //    //{
            //    //    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4108),  //POS Attendance is disabled at the Role level.
            //    //                                               MessageContainerList.GetMessage(executionContext, "Attendance"));
            //    //    this.DialogResult = DialogResult.Cancel;
            //    //    return;
            //    //}
            //    cmbAttendancRoles.SelectedValue = userRoleDtoList[0].RoleId;
            //}
            EnableDisableButtons();
            approverId = attendanceUserDTO.UserId;
            log.LogMethodExit();
        }

        private List<UserRolesDTO> InitRoles()
        {
            log.LogMethodEntry();
            List<UserRolesDTO> userRoleDtoList = userBL.GetAttendanceUserRoles();
            if(userRoleDtoList != null && userRoleDtoList.Any())
            {
                userRoleDtoList = userRoleDtoList.FindAll(x => x.EnablePOSClockIn == true);
            }
            if (userRoleDtoList == null || userRoleDtoList.Count == 0)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 3083),  //POS Attendance is disabled at the Role level.
                                                                MessageContainerList.GetMessage(executionContext, "Attendance"));
                this.DialogResult = DialogResult.Cancel;
                return null;
            }
            userRoleDtoList = userRoleDtoList.FindAll(x => x.IsActive == true).ToList();
            if (userRoleDtoList != null && userRoleDtoList.Any())
            {
                cmbAttendancRoles.DataSource = userRoleDtoList;
                cmbAttendancRoles.DisplayMember = "Role";
                cmbAttendancRoles.ValueMember = "RoleId";
                if (userRoleDtoList.Count == 1)
                    cmbAttendancRoles.SelectedValue = userRoleDtoList[0].RoleId;
                else
                    cmbAttendancRoles.SelectedValue = "SELECT";
                log.LogMethodExit();
                return userRoleDtoList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        private void GetLastLogInDetails()
        {
            log.LogMethodEntry();

            lblUserName.Text = MessageContainerList.GetMessage(executionContext, attendanceUserDTO.UserName);
            AttendanceDTO attendanceDTO = userBL.GetAttendanceForDay();
            if (attendanceDTO == null || (attendanceDTO.AttendanceLogDTOList == null || attendanceDTO.AttendanceLogDTOList.Any() == false))
            {
                currentStatus = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
                lblStatus.Text = MessageContainerList.GetMessage(executionContext, "Clock In");
                return;
            }
            lastLoggedInLogDTO = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty ||
                                             x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault();

            LookupValuesList lookupValuesList = new LookupValuesList();
            DateTime currentTime = lookupValuesList.GetServerDateTime();
            DateTime? inTime = lastLoggedInLogDTO.Timestamp;
            DateTime? outTime = currentTime;
            totalAttendanceHours = attendanceDTO.Hours;
            if (lastLoggedInLogDTO.Type == AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString())
            {
                totalAttendanceHours = (attendanceDTO.Hours + outTime.Value.Subtract(inTime.Value).TotalHours);
            }
            var timeSpan = TimeSpan.FromHours(totalAttendanceHours);
            int hh = timeSpan.Hours;
            int mm = timeSpan.Minutes;
            int ss = timeSpan.Seconds;
            if (lastLoggedInLogDTO != null && lastLoggedInLogDTO.AttendanceLogId > -1)
            {
                cmbAttendancRoles.Enabled = false;
                if (lastLoggedInLogDTO.Type == AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString()) // clock out
                {
                    currentStatus = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT);
                    if (totalAttendanceHours > 0)
                    {
                        lblStatus.Text = hh + " Hour " + mm + " Min";
                    }
                }
                else if (lastLoggedInLogDTO.Type == AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString() 
                    && lastLoggedInLogDTO.Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK)) // clock in after break
                {
                    currentStatus = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
                    SetAttendanceBreakStatus();
                }
                else if (lastLoggedInLogDTO.Type == AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString())
                {
                    currentStatus = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
                    lblStatus.Text = MessageContainerList.GetMessage(executionContext, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN));
                    if (totalAttendanceHours > 0)
                    {
                        lblStatus.Text = MessageContainerList.GetMessage(executionContext, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN)) + " - " + hh + " Hour " + mm + " Min";
                    }
                }
            }
            log.LogMethodExit();
        }

        private string SetAttendanceBreakStatus()
        {
            log.LogMethodEntry(lastLoggedInLogDTO);
            var timeSpan = TimeSpan.FromHours(totalAttendanceHours);
            int hh = timeSpan.Hours;
            int mm = timeSpan.Minutes;
            int ss = timeSpan.Seconds;

            DateTime BreakStartTime = Convert.ToDateTime(lastLoggedInLogDTO.Timestamp);
            int MinBreakTime = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MINIMUM_BREAK_TIME"));
            int MaxBreakTime = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MAXIMUM_BREAK_TIME"));
            if (MinBreakTime > 0)
            {
                int elapsedMinutes = (int)(POSStatic.Utilities.getServerTime() - BreakStartTime).TotalMinutes;
                if (elapsedMinutes < MinBreakTime)
                    currentStatus = "Early From Break" + "(" + elapsedMinutes + " Mins" + ")";
                else if (elapsedMinutes > MaxBreakTime)
                    currentStatus = "Late From Break" + "(" + elapsedMinutes + " Mins" + ")";
                else
                    currentStatus = "On Time";
                lblStatus.Text = currentStatus;
                if (totalAttendanceHours > 0)
                {
                    lblStatus.Text = currentStatus + " - " + hh + " Hour " + +mm + " Min";
                }
            }
            log.LogMethodExit(currentStatus);
            return currentStatus;
        }

        private void ClockInOut(string userActionType)
        {
            log.LogMethodEntry(userActionType);
            if (string.IsNullOrEmpty(attendanceUserDTO.EmpNumber))
            {
                log.Error("Record Attendance failed. Check user for valid card number, emp number.");
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 3072)); //"Record Attendance failed. Check user for valid card number, emp number."
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            string status = userActionType;
            string sessionRemarks = string.Empty;
            if(lastLoggedInLogDTO != null && lastLoggedInLogDTO.Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))
            {
                status = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK);
                sessionRemarks = SetAttendanceBreakStatus();
                sessionRemarks = sessionRemarks + "|";
            }
            else if(lastLoggedInLogDTO != null && lastLoggedInLogDTO.Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
            {
                status = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
            }
            bool retVal = true;
            if (userActionType == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN))
            {
                bool mgrApprovalRequired = userBL.IsManagerApprovedForClockIn(Convert.ToInt32(cmbAttendancRoles.SelectedValue));
                if (mgrApprovalRequired) // approval not required for clock-out
                {
                    string savFlag = POSStatic.ParafaitEnv.Manager_Flag;
                    POSStatic.ParafaitEnv.Manager_Flag = "N";
                    retVal = Authenticate.Manager(ref approverId);
                    POSStatic.ParafaitEnv.Manager_Flag = savFlag;
                    if (!retVal)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 268);
                        return;
                    }
                }
            }
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    try
                    {
                        dbTrx.BeginTransaction();
                    int tipAmount = 0;
                    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_TIP_ENTRY_DURING_CLOCKOUT").Equals("Y") && userActionType == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
                    {
                        GenericDataEntry tipEntry = new GenericDataEntry(1);
                        tipEntry.Text = MessageContainerList.GetMessage(executionContext, 3073); // "Clock Out: Direct Tip Entry"
                        tipEntry.DataEntryObjects[0].mandatory = true;
                        tipEntry.DataEntryObjects[0].label = MessageContainerList.GetMessage(executionContext, 3074); // "Enter Direct Tip amount"
                        tipEntry.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.Integer;
                        tipEntry.StartPosition = FormStartPosition.CenterScreen;
                        if (tipEntry.ShowDialog() == DialogResult.OK)
                        {
                            tipAmount = string.IsNullOrEmpty(tipEntry.DataEntryObjects[0].data) ? 0 : Convert.ToInt32(tipEntry.DataEntryObjects[0].data);
                        }
                    }
                    userBL.RecordAttendance(Convert.ToInt32(cmbAttendancRoles.SelectedValue), approverId, status, POSStatic.Utilities.ParafaitEnv.POSMachineId, POSStatic.Utilities.getServerTime(), -1,
                                            "CARD", tipAmount, -1, null, null, null, userActionType, cardNumber, dbTrx.SQLTrx, true, sessionRemarks);
                    dbTrx.EndTransaction();
                    }
                    catch(Exception ex)
                    {
                        dbTrx.RollBack();
                        log.Error(ex);
                        throw;
                    }
                    string formattedReceipt = userBL.GetAttendancePrintReciept(true, userActionType);
                    if (!string.IsNullOrEmpty(formattedReceipt))
                    {
                        using (PrintDocument printDocument = new PrintDocument())
                        {
                            printDocument.PrintPage += (sender, args) =>
                            {
                                SizeF sf = args.Graphics.MeasureString(formattedReceipt, new Font("Arial Narrow", 10), 300);
                                args.Graphics.DrawString(formattedReceipt, new Font("Arial Narrow", 10), System.Drawing.Brushes.Black, new RectangleF(new PointF(4.0F, 4.0F), sf),
                                         StringFormat.GenericTypographic);
                            };
                            printDocument.Print();
                        }
                    }
                    if (POSStatic.ParafaitEnv.User_Id == Convert.ToInt32(attendanceUserDTO.UserId))
                    {
                        POSStatic.ParafaitEnv.RoleId = Convert.ToInt32(cmbAttendancRoles.SelectedValue);
                        UserRoles usersRolesBL = new UserRoles(POSStatic.Utilities.ExecutionContext, Convert.ToInt32(cmbAttendancRoles.SelectedValue));
                        POSStatic.ParafaitEnv.Role = usersRolesBL.getUserRolesDTO.Role;

                        POSStatic.ParafaitEnv.Manager_Flag = usersRolesBL.getUserRolesDTO.ManagerFlag;
                        POSStatic.ParafaitEnv.EnablePOSClockIn = usersRolesBL.getUserRolesDTO.EnablePOSClockIn;
                        POSStatic.ParafaitEnv.AllowShiftOpenClose = usersRolesBL.getUserRolesDTO.AllowShiftOpenClose;
                        POSStatic.ParafaitEnv.AllowPOSAccess = usersRolesBL.getUserRolesDTO.AllowPosAccess;
                    }
                    if(lastLoggedInLogDTO == null)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 198, attendanceUserDTO.UserName, cardNumber);
                        DisplayAttendanceLogRequestStatus();
                    }
                    else if (userActionType == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN))
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 198, attendanceUserDTO.UserName, cardNumber));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void DisplayAttendanceLogRequestStatus()
        {
            log.LogMethodEntry();
            string message = MessageContainerList.GetMessage(executionContext, 198, attendanceUserDTO.UserName, attendanceUserDTO.UserIdentificationTagsDTOList[0].CardNumber);
            AttendanceList attendanceList = new AttendanceList(executionContext);
            double noOfAttendanceModificationDays = Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_ATTENDANCE_MODIFICATION_WITHIN_X_DAYS"));
            List<AttendanceDTO> attendanceDTOList = attendanceList.GetAttendanceRequestStatusList(POSStatic.Utilities.getServerTime().AddDays(-noOfAttendanceModificationDays), POSStatic.Utilities.getServerTime(), attendanceUserDTO.UserId);
            if (attendanceDTOList != null && attendanceDTOList.Any())
            {
                message +=  Environment.NewLine + MessageContainerList.GetMessage(executionContext, 4008, noOfAttendanceModificationDays) // "Your Attendance Modification Request Status for the last 15 Days is as below"
                                    + Environment.NewLine;
                foreach (AttendanceDTO attendanceDTO in attendanceDTOList)
                {
                    message = message +  attendanceDTO.StartDate.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext,"DATE_FORMAT"))
                                        + " - " + attendanceDTO.RequestStatus + Environment.NewLine;
                }
            }
            POSUtils.ParafaitMessageBox(message);
            log.LogMethodExit();
        }

        private void EnableDisableButtons()
        {
            log.LogMethodEntry();
            string attendanceValidationBehavior = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ATTENDANCE_VALIDATION_BEHAVIOR");
            if (attendanceValidationBehavior == AttendanceDTO.AttendanceValidationBehaviour.USERENFORCEMENT.ToString())
            {
                cmbAttendancRoles.Enabled = true;
                if (lastLoggedInLogDTO != null && lastLoggedInLogDTO.Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
                {
                    cmbAttendancRoles.Enabled = false;
                }
                if (lastLoggedInLogDTO == null
                    || (currentStatus != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT) && (lastLoggedInLogDTO != null && lastLoggedInLogDTO.Type != AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString())))
                {
                    btnClockIn.Enabled = true;
                    btnBreak.Enabled = false;
                    btnBreak.BackgroundImage = Properties.Resources.button_pressed;
                    btnClockOut.Enabled = false;
                    btnClockOut.BackgroundImage = Properties.Resources.button_pressed;
                }
                else
                {
                    btnBreak.Enabled = true;
                    btnClockOut.Enabled = true;
                    btnClockIn.Enabled = false;
                    btnClockIn.BackgroundImage = Properties.Resources.button_pressed;
                }
            }
            else if (attendanceValidationBehavior == AttendanceDTO.AttendanceValidationBehaviour.SYSTEMENFORCEMENT.ToString())
            {
                cmbAttendancRoles.Enabled = true;
                btnClockIn.Enabled = true;
                btnBreak.Enabled = true;
                btnClockOut.Enabled = true;
            }
            if (Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MINIMUM_BREAK_TIME")) < 0)
            {
                btnClose.Location = btnBreak.Location;
                btnBreak.Visible = false;
            }
            log.LogMethodExit();
        }

        private void frmAttendanceRoles_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Dispose();
            log.LogMethodExit(null);
        }

        private void btnClockIn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (cmbAttendancRoles.SelectedValue == null)
                {
                    log.Error("Record Attendance failed. User role is not selected");
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 5037));//User role is not selected
                    return;
                }
                ClockInOut(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN));
                POSStatic.CLOCKED_IN = true;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnBreak_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ClockInOut(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK));
                POSStatic.CLOCKED_IN = true;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClockOut_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ClockInOut(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT));
                POSStatic.CLOCKED_IN = false;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
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

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button b = sender as Button;
                if (b == btnClockOut)
                {
                    btnClockOut.BackgroundImage = Properties.Resources.red_button_normal;
                }
                else if (b == btnClockIn)
                {
                    btnClockIn.BackgroundImage = Properties.Resources.green_button_normal;
                }
                else if (b == btnBreak)
                {
                    btnBreak.BackgroundImage = Properties.Resources.blue_button_normal;
                }
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

    }
}

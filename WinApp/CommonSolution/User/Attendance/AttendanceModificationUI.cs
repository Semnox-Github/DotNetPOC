/********************************************************************************************
 * Project Name - Attendance Modification UI
 * Description  - UI of Attendance Modification
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.130.0     28-Jun-2021   Deeksha                 Created as part of Attendance Modification enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.User
{
    public partial class AttendanceModificationUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AttendanceLogDTO modifiedLogDTO;
        private AttendanceLogDTO originalLogDTO;
        private ExecutionContext executionContext;
        private Utilities utilities;
        private bool viewOnly;
        private AlphaNumericKeyPad keypad;
        private TextBox currentTextBox;
        private List<UserRolesDTO> userRoleDtoList;
        private DataTable dtFromTime = new DataTable();
        private DateTime attendanceDate;

        public AttendanceLogDTO GetAttendanceLogDTO
        {
            get
            {
                return modifiedLogDTO;
            }
        }

        public AttendanceModificationUI(Utilities utilities, AttendanceLogDTO modifiedLogDTO, AttendanceLogDTO originalLogDTO, DateTime attendanceDate,
                                        bool viewOnly = false)
        {
            log.LogMethodEntry(executionContext, modifiedLogDTO, originalLogDTO, viewOnly);
            InitializeComponent();
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            this.modifiedLogDTO = modifiedLogDTO;
            this.viewOnly = viewOnly;
            this.originalLogDTO = originalLogDTO;
            this.attendanceDate = attendanceDate;
            LoadAttendanceUserRolesDTO();
            dtpOriginalDate.Enabled = cmbOriginalMin.Enabled = cmbOriginalHour.Enabled = cmbOriginalAM.Enabled = false;
            if(originalLogDTO == null)
            {
                grpModified.Text = MessageContainerList.GetMessage(executionContext, "Add New Record");
                this.Text = MessageContainerList.GetMessage(executionContext, "Add Attendance");
                txtMessgae.Text = MessageContainerList.GetMessage(executionContext, 4152); //"Choose attendance type first and then select the status"
                List<string> attendanceStatus = new List<string>();
                attendanceStatus.Add("");
                cmbModifiedStatus.DataSource = attendanceStatus;
            }
            LoadAttendanceType();
            SetAttendanceLogDetails();
            if (viewOnly)
            {
                btnShowKeyPad.Visible = false;
                btnAdd.Visible = false;
                btnDelete.Visible = false;
                cmbModifiedRole.Enabled = dtpModifiedDate.Enabled = cmbModifiedMin.Enabled = cmbModifiedHour.Enabled = cmbModifiedAM.Enabled
                                        = cmbModifiedStatus.Enabled = cmbModifiedType.Enabled = txtModifiedRemarks.Enabled = false;
            }
            utilities.setLanguage(this);
            currentTextBox = txtModifiedRemarks;
            dtpOriginalDate.CustomFormat = dtpModifiedDate.CustomFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT").ToString();
            log.LogMethodExit();
        }

        private void SetAttendanceLogDetails()
        {
            if (modifiedLogDTO != null)
            {
                txtModifiedRemarks.Text = modifiedLogDTO.Remarks;
                dtpModifiedDate.Value = modifiedLogDTO.Timestamp;
                cmbModifiedType.SelectedItem = modifiedLogDTO.Type.ToString();
                if (modifiedLogDTO.AttendanceRoleId != -1)
                    cmbModifiedRole.SelectedValue = modifiedLogDTO.AttendanceRoleId;
                int hour = modifiedLogDTO.Timestamp.Hour;
                if (hour > 12)
                {
                    hour = modifiedLogDTO.Timestamp.Hour - 12;
                }
                cmbModifiedHour.SelectedItem = hour.ToString();
                cmbModifiedMin.SelectedItem = modifiedLogDTO.Timestamp.Minute.ToString();
                cmbModifiedAM.SelectedItem = modifiedLogDTO.Timestamp.Hour < 12 ? "AM" : "PM";
                cmbModifiedStatus.SelectedItem = modifiedLogDTO.Status;
            }
            if (originalLogDTO != null)
            {
                txtOriginalRemarks.Text = originalLogDTO.Remarks;
                dtpOriginalDate.Value = originalLogDTO.Timestamp;
                txtOriginalType.Text = originalLogDTO.Type;
                txtOriginalStatus.Text = originalLogDTO.Status;
                if(userRoleDtoList.Exists(x => x.RoleId == originalLogDTO.AttendanceRoleId))
                    txtOriginalRole.Text = userRoleDtoList != null ? userRoleDtoList.Find(x => x.RoleId == originalLogDTO.AttendanceRoleId).Role : string.Empty;
                int hour = originalLogDTO.Timestamp.Hour;
                if (hour > 12)
                {
                    hour = originalLogDTO.Timestamp.Hour - 12;
                }
                cmbOriginalHour.SelectedItem = hour.ToString();
                cmbOriginalMin.SelectedItem = originalLogDTO.Timestamp.Minute.ToString();
                cmbOriginalAM.SelectedItem = originalLogDTO.Timestamp.Hour < 12 ? "AM" : "PM";

            }
            else
            {
                grpOriginal.Visible = false;
                grpModified.Location = grpOriginal.Location;
            }
        }

        private void LoadAttendanceUserRolesDTO()
        {
            log.LogMethodEntry();
            try
            {
                UserRolesList userRolesList = new UserRolesList(executionContext);
                Users users = new Users(executionContext, modifiedLogDTO.UserId);
                userRoleDtoList = users.GetAttendanceUserRoles();
                if (userRoleDtoList == null || !userRoleDtoList.Any())
                {
                    List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchByParams = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                    searchByParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchByParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ROLE_ID, users.UserDTO.RoleId.ToString()));
                    userRoleDtoList = userRolesList.GetAllUserRoles(searchByParams);
                }

                if (userRoleDtoList == null)
                {
                    userRoleDtoList = new List<UserRolesDTO>();
                }
                userRoleDtoList.Insert(0, new UserRolesDTO());
                userRoleDtoList[0].Role = string.Empty;
                userRoleDtoList[0].RoleId = -1;

                cmbModifiedRole.DataSource = userRoleDtoList;
                cmbModifiedRole.DisplayMember = "Role";
                cmbModifiedRole.ValueMember = "RoleId";

            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void LoadAttendanceType()
        {
            log.LogMethodEntry();
            try
            {
                List<string> attendanceType = new List<string>();
                attendanceType.Add(AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString());
                attendanceType.Add(AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString());
                attendanceType.Insert(0, string.Empty);
                cmbModifiedType.DataSource = attendanceType;
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (cmbModifiedHour.SelectedItem == null || cmbModifiedMin.SelectedItem == null || cmbModifiedAM.SelectedItem == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3070)); //"Date is out of Range."
                }
                int hh = Convert.ToInt32(cmbModifiedHour.SelectedItem);
                int mm = Convert.ToInt32(cmbModifiedMin.SelectedItem);
                string tt = cmbModifiedAM.SelectedItem.ToString();
                string date = dtpModifiedDate.Value.Month + "/" + dtpModifiedDate.Value.Day + "/" + dtpModifiedDate.Value.Year + " " + hh + ":" + mm + ":" + 00 + " " + tt;
                DateTime datetime = DateTime.Parse(date, CultureInfo.InvariantCulture);
                if (datetime < attendanceDate || datetime > attendanceDate.AddDays(1))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3070)); //"Date is out of Range.";
                }
                if (string.IsNullOrEmpty(cmbModifiedType.SelectedValue.ToString()))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3081)); //"choose Attendance Type."
                }
                //else if (Convert.ToInt32(cmbModifiedRole.SelectedValue) == -1)
                //{
                //    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3082)); // choose Attendance Role.
                //}
                else if (string.IsNullOrEmpty(cmbModifiedStatus.SelectedValue.ToString()))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4006)); // choose Attendance Status.
                }
                LookupValuesList lookupValuesList = new LookupValuesList();
                DateTime currentTime = lookupValuesList.GetServerDateTime();
                if (datetime > currentTime)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3068)); //Future date is not allowed.
                }
                modifiedLogDTO.Timestamp = datetime;
                modifiedLogDTO.Remarks = txtModifiedRemarks.Text;
                modifiedLogDTO.AttendanceRoleId = Convert.ToInt32(cmbModifiedRole.SelectedValue);
                modifiedLogDTO.Type = cmbModifiedType.SelectedValue.ToString();
                if (originalLogDTO != null && (originalLogDTO.Status.Contains("Clocked In") || originalLogDTO.Status.Contains("Clocked Out")))
                {
                    if(cmbModifiedStatus.SelectedValue.ToString() == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN)
                        || cmbModifiedStatus.SelectedValue.ToString() == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK))
                    {
                        modifiedLogDTO.Status = "Clocked In";
                    }
                    else if(cmbModifiedStatus.SelectedValue.ToString() == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))
                    {
                        modifiedLogDTO.Status = "On Break";
                    }
                    else
                    {
                        modifiedLogDTO.Status = "Clocked Out";
                    }
                }
                else
                {
                    modifiedLogDTO.Status = cmbModifiedStatus.SelectedValue.ToString();
                }
                modifiedLogDTO.Notes = txtModifiedRemarks.Text;
                modifiedLogDTO.RequestStatus = null;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                txtMessgae.Text = ex.Message;
                txtMessgae.BackColor = Color.Red;
                txtMessgae.ForeColor = Color.White;
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Close();
            }
            catch (Exception ex) { log.Error(ex); }
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

        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox textBox = new TextBox();
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
            log.LogMethodExit();
        }


        private void txtRemarks_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (keypad != null)
                keypad.currentTextBox = txtModifiedRemarks;
            log.LogMethodExit();
        }

        private void cmbModifiedType_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                List<string> attendanceStatus = new List<string>();
                if (!string.IsNullOrEmpty(cmbModifiedType.SelectedValue.ToString()))
                {
                    if (cmbModifiedType.SelectedValue.ToString() == AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString())
                    {
                        attendanceStatus.Add(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN));
                        attendanceStatus.Add(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK));
                    }
                    else if (cmbModifiedType.SelectedValue.ToString() == AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString())
                    {
                        attendanceStatus.Add(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT));
                        attendanceStatus.Add(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK));
                    }
                    cmbModifiedStatus.DataSource = attendanceStatus;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                modifiedLogDTO.IsActive = "N";
                modifiedLogDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Rejected.ToString();
                originalLogDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - Attendance BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018      Indhu               Created 
 *2.80        20-May-2020      Vikas Dwivedi       Modified as per the Standard CheckList
 *2.130       11-Jul2021       Deeksha             Modified as part of POS attendance changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    public class AttendanceBL
    {
        private AttendanceDTO attendanceDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of AttendanceBL
        /// </summary>
        private AttendanceBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the attendance id as the parameter
        /// Would fetch the attendance object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="attendanceId">id of attendance Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AttendanceBL(ExecutionContext executionContext, int attendanceId, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceId, sqlTransaction);
            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler(null);
            attendanceDTO = attendanceDataHandler.GetAttendance(attendanceId);
            if (attendanceDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AttendanceDTO", attendanceId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AttendanceBL object using the AttendanceDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="attendanceDTO">attendanceDTO</param>
        public AttendanceBL(ExecutionContext executionContext, AttendanceDTO attendanceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceDTO);
            this.attendanceDTO = attendanceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the attendanceLog record
        /// Checks if the AttendanceId is not less than 0
        ///     If it is less than 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (attendanceDTO.AttendanceId > -1 &&
                attendanceDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Nothing to save."));
            }
            UpdateAttendanceLogType();
            //Validate();
            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler(sqlTransaction);
            if (attendanceDTO.AttendanceId < 0)
            {
                log.LogVariableState("AttendanceDTO", attendanceDTO);
                attendanceDTO = attendanceDataHandler.InsertAttendance(attendanceDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                attendanceDTO.AcceptChanges();
            }
            else if (attendanceDTO.IsChanged)
            {

                log.LogVariableState("AttendanceDTO", attendanceDTO);
                attendanceDTO = attendanceDataHandler.UpdateAttendance(attendanceDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                attendanceDTO.AcceptChanges();

            }
            SaveAttendanceLog(sqlTransaction);
            attendanceDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : AchievementClassLevelDTOList and AchievementScoreLogDTOList
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SaveAttendanceLog(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (attendanceDTO.AttendanceLogDTOList != null &&
                attendanceDTO.AttendanceLogDTOList.Any())
            {
                foreach (var attendanceLogDTO in attendanceDTO.AttendanceLogDTOList)
                {
                    if (attendanceLogDTO.AttendanceId != attendanceDTO.AttendanceId)
                    {
                        attendanceLogDTO.AttendanceId = attendanceDTO.AttendanceId;
                    }
                }
                AttendanceLogList attendanceLogList = new AttendanceLogList(executionContext, attendanceDTO.AttendanceLogDTOList);
                attendanceLogList.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AttendanceDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            string attendanceType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ATTENDANCE_TYPE_DETERMINATION_METHOD");
            string attendanceValidationBehavior = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ATTENDANCE_VALIDATION_BEHAVIOR");

            if (attendanceType != "FIRST_IN_LAST_OUT")
            {
                List<AttendanceLogDTO> attendanceLogDTOList = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                              x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderBy(y => y.Timestamp).ThenBy(z => z.AttendanceLogId).ToList();
                if (attendanceLogDTOList != null && attendanceLogDTOList.Any())
                {
                    string type = AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString();
                    int clockInRoleId = -1;
                    foreach (AttendanceLogDTO logDTO in attendanceLogDTOList)
                    {
                        if (logDTO.Type == AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString())
                        {
                            clockInRoleId = logDTO.AttendanceRoleId;
                        }
                        else if (logDTO.AttendanceRoleId != clockInRoleId)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3089));
                        }
                        if (type == logDTO.Type)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3088));
                        }
                        type = logDTO.Type;
                    }
                }
            }
        }

        private void UpdateAttendanceLogType()
        {
            log.LogMethodEntry();
            try
            {
                if (attendanceDTO.AttendanceLogDTOList.Any())
                {
                    string attendanceType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ATTENDANCE_TYPE_DETERMINATION_METHOD");
                    if (attendanceType == "FIRST_IN_LAST_OUT")
                    {
                        attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceLogId > -1).FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                         x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault().Type = AttendanceDTO.AttendanceType.ATTENDANCE_INVALID.ToString();
                        attendanceDTO.AttendanceLogDTOList.Find(x => x.AttendanceLogId == -1).Type = AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString();
                        attendanceDTO.AttendanceLogDTOList.Find(x => x.AttendanceLogId == -1).Status = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT);
                    }
                    else if (attendanceDTO.AttendanceLogDTOList.Exists(x => x.Type == null && x.AttendanceLogId == -1))
                    {
                        if (attendanceDTO.AttendanceLogDTOList.Count > 1)
                        {
                            string lastLoggedInStatus = null;
                            List<AttendanceLogDTO> attendanceLogDTOList = attendanceDTO.AttendanceLogDTOList.FindAll(x =>x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                         x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ToList();

                            if (attendanceLogDTOList != null)
                            {
                                attendanceLogDTOList = attendanceLogDTOList.FindAll(x => x.AttendanceLogId != -1);
                                if (attendanceLogDTOList != null)
                                    lastLoggedInStatus = attendanceLogDTOList[0].Status;
                            }
                            if (attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceLogId > -1).FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                             x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault().Type == AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString())
                            {
                                attendanceDTO.AttendanceLogDTOList.FirstOrDefault(x => x.Type == null && x.AttendanceLogId == -1).Type = AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString();
                                attendanceDTO.AttendanceLogDTOList.FirstOrDefault(x => x.Status == null && x.AttendanceLogId == -1).Status = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
                                if (lastLoggedInStatus == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))
                                    attendanceDTO.AttendanceLogDTOList.FirstOrDefault(x => x.AttendanceLogId == -1).Status = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK);
                            }
                            else
                            {
                                attendanceDTO.AttendanceLogDTOList.FirstOrDefault(x => x.Type == null && x.AttendanceLogId == -1).Type = AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString();
                                attendanceDTO.AttendanceLogDTOList.FirstOrDefault(x => x.Status == null && x.AttendanceLogId == -1).Status = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT);
                            }
                        }
                        else
                        {
                            attendanceDTO.AttendanceLogDTOList.FirstOrDefault(x => x.Type == null && x.AttendanceLogId == -1).Type = AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString();
                            attendanceDTO.AttendanceLogDTOList.FirstOrDefault(x => x.Status == null && x.AttendanceLogId == -1).Status = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for AttendanceBL object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AttendanceLogList attendanceLogList = new AttendanceLogList(executionContext);
            List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.ATTENDANCE_ID, attendanceDTO.AttendanceId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.ISACTIVE, "Y"));
            }
            attendanceDTO.AttendanceLogDTOList = attendanceLogList.GetAllAttendanceUserList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        private void CalculateAttendanceHours()
        {
            log.LogMethodEntry();
            Decimal hours = 0;
            DateTime? inTime = null;
            DateTime? outTime = null;

            String attendenceTypeDeterminationMethod = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ATTENDANCE_TYPE_DETERMINATION_METHOD");
            if (attendenceTypeDeterminationMethod == null || string.IsNullOrEmpty(attendenceTypeDeterminationMethod.Trim()))
            {
                //If the determination method is not configured default to alternative in and out
                attendenceTypeDeterminationMethod = "ALTERNATIVE_IN_OUT";
            }
            if (attendenceTypeDeterminationMethod != "ALTERNATIVE_IN_OUT")
            {
                log.Error("Cannot calculate Attendance Hours");
                return;
            }
            List<AttendanceLogDTO> attendanceLogDTOList = attendanceDTO.AttendanceLogDTOList.Any() ?
                                                                        attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                         x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderBy(y => y.Timestamp).ThenBy(z => z.AttendanceLogId).ToList() : null;
            if (attendanceLogDTOList != null && attendanceLogDTOList.Any())
            {
                string lastLoginType = AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString();
                for (int i = 0; i < attendanceLogDTOList.Count; i++)
                {
                    if (lastLoginType == attendanceLogDTOList[i].Type)
                    {
                        log.Error("Cannot calculate Attendance Hours");
                        break;
                    }
                    lastLoginType = attendanceLogDTOList[i].Type;
                    if (attendanceLogDTOList[i].Type == AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString())
                    {
                        inTime = attendanceLogDTOList[i].Timestamp;
                        outTime = null;
                    }
                    if (attendanceLogDTOList[i].Type == AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString())
                    {
                        outTime = attendanceLogDTOList[i].Timestamp;
                        if (inTime != null)
                        {
                            hours = (Decimal)((Double)hours + outTime.Value.Subtract(inTime.Value).TotalHours);
                        }
                        inTime = null;
                    }
                }
            }
            attendanceDTO.Hours = Convert.ToDouble(hours);
            log.LogMethodExit();
        }

        public void RecordAttendance(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                List<AttendanceLogDTO> currentAttendanceLogDTOList = attendanceDTO.AttendanceLogDTOList.Where(x => x.AttendanceLogId == -1).OrderBy(x => x.Timestamp).ToList();
                foreach (AttendanceLogDTO currentAttendanceLogDTO in currentAttendanceLogDTOList)
                {
                    string userActionType = currentAttendanceLogDTO.Type;
                    attendanceDTO.AttendanceLogDTOList.Remove(attendanceDTO.AttendanceLogDTOList.Find(x => x.AttendanceLogId < 0));
                    if (!string.IsNullOrEmpty(currentAttendanceLogDTO.Status))
                    {
                        if (currentAttendanceLogDTO.Type == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN)
                            || currentAttendanceLogDTO.Type == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK))
                        {
                            currentAttendanceLogDTO.Type = AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString();
                        }
                        else
                        {
                            currentAttendanceLogDTO.Type = AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString();
                        }
                    }
                    AttendanceLogDTO lastLoggedInLogDTO = attendanceDTO.AttendanceLogDTOList.Exists(x => x.AttendanceLogId >= 0) ?
                                                  attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceLogId >= 0 && x.RequestStatus == string.Empty || x.RequestStatus == null || 
                                                  x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault() : null;

                    if (lastLoggedInLogDTO != null)
                    {
                        if (lastLoggedInLogDTO.Type == currentAttendanceLogDTO.Type && lastLoggedInLogDTO.Status == currentAttendanceLogDTO.Status )
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4155)); //"Invalid Operation"
                        }
                    }
                    string attendanceValidationBehavior = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ATTENDANCE_VALIDATION_BEHAVIOR");
                    if (attendanceValidationBehavior == AttendanceDTO.AttendanceValidationBehaviour.SYSTEMENFORCEMENT.ToString())
                    {
                        CreateSystemEnforcementAttendanceLogs(currentAttendanceLogDTO, userActionType);
                    }

                    //Insert Update Attendance
                    AttendanceLogDTO attendanceLogDTO = new AttendanceLogDTO(-1, currentAttendanceLogDTO.CardNumber, currentAttendanceLogDTO.ReaderId, currentAttendanceLogDTO.Timestamp, currentAttendanceLogDTO.Type,
                                                                                    attendanceDTO.AttendanceId, currentAttendanceLogDTO.Mode, currentAttendanceLogDTO.AttendanceRoleId,
                                                                                   currentAttendanceLogDTO.AttendanceRoleApproverId, currentAttendanceLogDTO.Status, currentAttendanceLogDTO.MachineId,
                                                                                   currentAttendanceLogDTO.POSMachineId, currentAttendanceLogDTO.TipValue, "Y", currentAttendanceLogDTO.OriginalAttendanceLogId,
                                                                                   currentAttendanceLogDTO.RequestStatus, currentAttendanceLogDTO.ApprovedBy, currentAttendanceLogDTO.ApprovalDate, currentAttendanceLogDTO.UserId, currentAttendanceLogDTO.Remarks, currentAttendanceLogDTO.Notes);
                    attendanceDTO.AttendanceLogDTOList.Add(attendanceLogDTO);
                }
                //Update Attendance Hours
                CalculateAttendanceHours();

                Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void CreateSystemEnforcementAttendanceLogs(AttendanceLogDTO currentAttendanceLogDTO , string userActionType)
        {
            log.LogMethodEntry(currentAttendanceLogDTO);
            string remarks = null;
            string lastLoggedInattendancetype = null;
            int lastLoggedInAttendanceRoleId = -1;
            string lastLoggedInattendanceStatus = null;
            AttendanceLogDTO lastLoggedInLogDTO = attendanceDTO.AttendanceLogDTOList.Exists(x => x.AttendanceLogId >= 0) ?
                                                    attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceLogId >= 0 && x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                    x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault() : null;
            if (lastLoggedInLogDTO != null)
            {
                lastLoggedInattendancetype = lastLoggedInLogDTO.Type;
                lastLoggedInAttendanceRoleId = lastLoggedInLogDTO.AttendanceRoleId;
                lastLoggedInattendanceStatus = lastLoggedInLogDTO.Status;
            }

            if(lastLoggedInAttendanceRoleId != currentAttendanceLogDTO.AttendanceRoleId)
            {
                if(lastLoggedInattendanceStatus == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))
                {
                    remarks = MessageContainerList.GetMessage(executionContext, "System Clocked In");
                    attendanceDTO.AttendanceLogDTOList.Add(new AttendanceLogDTO(-1, currentAttendanceLogDTO.CardNumber, currentAttendanceLogDTO.ReaderId, currentAttendanceLogDTO.Timestamp,
                        AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString(), attendanceDTO.AttendanceId, currentAttendanceLogDTO.Mode, lastLoggedInAttendanceRoleId,
                       currentAttendanceLogDTO.AttendanceRoleApproverId, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK), currentAttendanceLogDTO.MachineId,
                       currentAttendanceLogDTO.POSMachineId, currentAttendanceLogDTO.TipValue, "Y", currentAttendanceLogDTO.OriginalAttendanceLogId,
                       currentAttendanceLogDTO.RequestStatus, currentAttendanceLogDTO.ApprovedBy, currentAttendanceLogDTO.ApprovalDate, currentAttendanceLogDTO.UserId, remarks, null));
                    lastLoggedInattendanceStatus = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK);
                }
                if (lastLoggedInattendanceStatus == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN)
                    || lastLoggedInattendanceStatus == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK))
                {
                    remarks = MessageContainerList.GetMessage(executionContext, "System Clocked Out");
                    attendanceDTO.AttendanceLogDTOList.Add(new AttendanceLogDTO(-1, currentAttendanceLogDTO.CardNumber, currentAttendanceLogDTO.ReaderId, currentAttendanceLogDTO.Timestamp,
                        AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString(), attendanceDTO.AttendanceId, currentAttendanceLogDTO.Mode, lastLoggedInAttendanceRoleId,
                       currentAttendanceLogDTO.AttendanceRoleApproverId, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT), currentAttendanceLogDTO.MachineId,
                       currentAttendanceLogDTO.POSMachineId, currentAttendanceLogDTO.TipValue, "Y", currentAttendanceLogDTO.OriginalAttendanceLogId,
                       currentAttendanceLogDTO.RequestStatus, currentAttendanceLogDTO.ApprovedBy, currentAttendanceLogDTO.ApprovalDate, currentAttendanceLogDTO.UserId, remarks, null));
                    if (currentAttendanceLogDTO.Type == AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString())
                    {
                        currentAttendanceLogDTO.Status = AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
                    }
                }
                lastLoggedInattendancetype = null;
            }

            remarks = currentAttendanceLogDTO.Status;
            if (lastLoggedInattendancetype == null && currentAttendanceLogDTO.Type != AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString()
                    || (lastLoggedInattendancetype == AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString()
                    && currentAttendanceLogDTO.Type == AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString()))
            {
                string status = lastLoggedInattendancetype == null ? AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN)
                                : lastLoggedInattendanceStatus == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK)
                                ? AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK)
                                : AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
                remarks = MessageContainerList.GetMessage(executionContext, "System Clocked In");
                attendanceDTO.AttendanceLogDTOList.Add(new AttendanceLogDTO(-1, currentAttendanceLogDTO.CardNumber, currentAttendanceLogDTO.ReaderId, currentAttendanceLogDTO.Timestamp,
                                                                            AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString(), attendanceDTO.AttendanceId, currentAttendanceLogDTO.Mode, currentAttendanceLogDTO.AttendanceRoleId,
                                                                           currentAttendanceLogDTO.AttendanceRoleApproverId, status, currentAttendanceLogDTO.MachineId,
                                                                           currentAttendanceLogDTO.POSMachineId, currentAttendanceLogDTO.TipValue, "Y", currentAttendanceLogDTO.OriginalAttendanceLogId,
                                                                           currentAttendanceLogDTO.RequestStatus, currentAttendanceLogDTO.ApprovedBy, currentAttendanceLogDTO.ApprovalDate, currentAttendanceLogDTO.UserId, remarks, null));
                currentAttendanceLogDTO.Type = AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString();
                currentAttendanceLogDTO.Status = userActionType;

            }
            else if ((lastLoggedInattendancetype == currentAttendanceLogDTO.Type || lastLoggedInattendancetype == AttendanceDTO.AttendanceType.ATTENDANCE_INVALID.ToString())
                    && (!currentAttendanceLogDTO.Status.Contains(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
                    || !currentAttendanceLogDTO.Status.Contains(AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))))
            {
                string status =  lastLoggedInattendanceStatus == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK)
                                 ? AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK)
                                 : AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT);
                remarks = MessageContainerList.GetMessage(executionContext, "System Clocked Out");
                attendanceDTO.AttendanceLogDTOList.Add(new AttendanceLogDTO(-1, currentAttendanceLogDTO.CardNumber, currentAttendanceLogDTO.ReaderId, currentAttendanceLogDTO.Timestamp, AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString(),
                                                                                attendanceDTO.AttendanceId, currentAttendanceLogDTO.Mode, lastLoggedInAttendanceRoleId,
                                                                                currentAttendanceLogDTO.AttendanceRoleApproverId, status, currentAttendanceLogDTO.MachineId,
                                                                                currentAttendanceLogDTO.POSMachineId, currentAttendanceLogDTO.TipValue, "Y", currentAttendanceLogDTO.OriginalAttendanceLogId,
                                                                                currentAttendanceLogDTO.RequestStatus, currentAttendanceLogDTO.ApprovedBy, currentAttendanceLogDTO.ApprovalDate,
                                                                                currentAttendanceLogDTO.UserId, remarks, null));
                currentAttendanceLogDTO.Type = AttendanceDTO.AttendanceType.ATTENDANCE_IN.ToString();
                currentAttendanceLogDTO.Status = status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK)
                                                          ? AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK)
                                                          : AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN);
            }
            log.LogMethodExit();
        }

        [Obsolete]
        public void PrintClockInClockOutReciept(string userActionType, string loginId)
        {
            log.LogMethodEntry();
            if (userActionType == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT)
                || userActionType == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_AUTOPRINT_RECEIPT_ON_CLOCKOUT"))
                {
                    PrintReceipt(loginId);
                }
            }
            else
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_AUTOPRINT_RECEIPT_ON_CLOCKIN"))
                {
                    PrintReceipt(loginId);
                }
            }
            log.LogMethodExit();
        }

        public string GetClockInClockOutReciept(string userActionType, string loginId)
        {
            log.LogMethodEntry();

            string printReciept = string.Empty;
            if (userActionType == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT)
                || userActionType == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_AUTOPRINT_RECEIPT_ON_CLOCKOUT"))
                {
                    printReciept = PrintReceipt(loginId);
                }
            }
            else
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_AUTOPRINT_RECEIPT_ON_CLOCKIN"))
                {
                    printReciept = PrintReceipt(loginId);
                }
            }
            log.LogMethodExit();
            return printReciept;
        }




        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        private string PrintReceipt(string loginId)
        {
            log.LogMethodEntry(loginId);
            string formattedReceipt = string.Empty;
            try
            {
                if (attendanceDTO.AttendanceLogDTOList.Any())
                {
                    List<AttendanceLogDTO> attendanceLogDTOList = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                         x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderBy(y => y.Timestamp).ThenBy(z => z.AttendanceLogId).ToList();
                    AttendanceLogDTO attendanceLogDTO = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                         x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault();
                    if (attendanceLogDTO == null)
                    {
                        return formattedReceipt;
                    }
                    double tipAmount = attendanceLogDTO.TipValue;
                    DateTime dt = StartOfWeek(attendanceLogDTO.Timestamp, DayOfWeek.Monday).AddHours(Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_WORKSHIFT_STARTTIME")));

                    formattedReceipt = "*----Clock In/Out Receipt----*" + Environment.NewLine;
                    formattedReceipt += "Login Id: " + loginId + Environment.NewLine;
                    formattedReceipt += "Date: " + attendanceLogDTO.Timestamp.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"))
                                                    + Environment.NewLine + Environment.NewLine;

                    foreach (AttendanceLogDTO attndnLogDTO in attendanceLogDTOList)
                    {
                        UserRoles userRoles = new UserRoles(executionContext, attndnLogDTO.AttendanceRoleId);
                        string userRole = string.Empty;
                        if (userRoles.getUserRolesDTO != null)
                            userRole = userRoles.getUserRolesDTO.Role;
                        string status = attndnLogDTO.Status;
                        if (attndnLogDTO.Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK))
                        {
                            status = attndnLogDTO.Notes;
                        }
                        formattedReceipt += attndnLogDTO.Timestamp.Date.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"))
                                                + " - " + attndnLogDTO.Timestamp.ToString("h:mm tt") +
                                                " - " + status + " - " + userRole + Environment.NewLine;

                    }
                    if (attendanceLogDTO.Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
                    {
                        formattedReceipt += new string('*', 20) + Environment.NewLine;
                        formattedReceipt += MessageContainerList.GetMessage(executionContext, "Direct Tips: ") +
                                    ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CURRENCY_SYMBOL") +
                                    tipAmount.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT")) +
                                    Environment.NewLine + Environment.NewLine;
                        formattedReceipt += new string('*', 20);
                    }
                    //using (PrintDocument printDocument = new PrintDocument())
                    //{
                    //    printDocument.PrintPage += (sender, args) =>
                    //    {
                    //        SizeF sf = args.Graphics.MeasureString(formattedReceipt, new Font("Arial Narrow", 10), 300);
                    //        args.Graphics.DrawString(formattedReceipt, new Font("Arial Narrow", 10), System.Drawing.Brushes.Black, new RectangleF(new PointF(4.0F, 4.0F), sf),
                    //                 StringFormat.GenericTypographic);
                    //    };
                    //    printDocument.Print();
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error("Error during print of Clock-In/Out details" + ex.Message);
            }
            log.LogMethodExit(formattedReceipt);
            return formattedReceipt;
        }

        public void ApproveAttendance(string approvedBy, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(approvedBy, sqlTransaction);
            if (attendanceDTO.AttendanceLogDTOList.Exists(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString())
                || attendanceDTO.AttendanceLogDTOList.Exists(x => x.AttendanceLogId == -1))
            {
                AttendanceValidation();
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime approvedTime = lookupValuesList.GetServerDateTime();
                List<AttendanceLogDTO> validAttendanceLogRecords = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.IsActive == "Y").OrderBy(x => x.Timestamp).ToList();
                foreach (AttendanceLogDTO logDTO in validAttendanceLogRecords)
                {
                    if (logDTO.AttendanceLogId != -1 && validAttendanceLogRecords.Exists(x => x.OriginalAttendanceLogId == logDTO.AttendanceLogId))
                    {
                        logDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString();
                        logDTO.IsActive = "N";
                    }
                    else if (logDTO.IsChanged || logDTO.AttendanceLogId == -1 || logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString())
                    {
                        logDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString();
                        logDTO.ApprovedBy = approvedBy;
                        logDTO.ApprovalDate = approvedTime;
                    }


                }
                attendanceDTO.AttendanceLogDTOList = attendanceDTO.AttendanceLogDTOList.OrderBy(x => x.Timestamp).ToList();
                CalculateAttendanceHours();
                Save(sqlTransaction);
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "No modified records found."));
            }
            log.LogMethodExit();
        }

        public void RejectAttendance(string approvedBy, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(approvedBy, sqlTransaction);
            if (attendanceDTO.AttendanceLogDTOList.Exists(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString())
                || attendanceDTO.AttendanceLogDTOList.Exists(x => x.IsChanged))
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime approvedTime = lookupValuesList.GetServerDateTime();
                foreach (AttendanceLogDTO logDTO in attendanceDTO.AttendanceLogDTOList)
                {
                    if (logDTO.AttendanceLogId == -1 || logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString())
                    {
                        logDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Rejected.ToString();
                        logDTO.ApprovedBy = approvedBy;
                        logDTO.ApprovalDate = approvedTime;
                        logDTO.IsActive = "N";
                    }
                }
                CalculateAttendanceHours();
                Save(sqlTransaction);
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "No modified records found."));
            }
            log.LogMethodExit();
        }

        public void SubmitAttendanceForApproval(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (attendanceDTO.AttendanceLogDTOList != null && attendanceDTO.AttendanceLogDTOList.Exists(x => x.IsChanged))
            {
                AttendanceValidation();
                foreach (AttendanceLogDTO logDTO in attendanceDTO.AttendanceLogDTOList)
                {
                    if (logDTO.AttendanceLogId == -1 
                        || (logDTO.RequestStatus != AttendanceLogDTO.AttendanceRequestStatus.Rejected.ToString() && logDTO.IsChanged ))
                    {
                        logDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString();
                    }
                }
                Save();
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "No modified records found."));
            }
            log.LogMethodExit();
        }

        private void AttendanceValidation()
        {
            log.LogMethodEntry();
            List<AttendanceLogDTO> attendanceLogDtoList = new List<AttendanceLogDTO>();
            List<AttendanceLogDTO> validAttendanceLogRecords  = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.IsActive == "Y").OrderBy(x => x.Timestamp).ToList();
            foreach (AttendanceLogDTO logDTO in validAttendanceLogRecords)
            {
                if (logDTO.AttendanceLogId != -1 && validAttendanceLogRecords.Exists(x => x.OriginalAttendanceLogId == logDTO.AttendanceLogId))
                {
                    continue;
                }
                attendanceLogDtoList.Add(logDTO);
            }
            if (attendanceLogDtoList.Any() && attendanceLogDtoList[0].Status != "Clocked In" &&
                (attendanceLogDtoList[0].Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN)))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4007, attendanceLogDtoList[0].Status)); //'Invalid attendance. No clock-in record found for day. First record for the day is &1'
            }
            string type = AttendanceDTO.AttendanceType.ATTENDANCE_OUT.ToString();
            int clockInRoleId = -1;
            string lastLoggedinStatus = string.Empty;
            List<ValidationError> validationErrorList = new List<ValidationError>();
            for (int i = 0; i < attendanceLogDtoList.Count; i++)
            {
                if (attendanceLogDtoList[i].Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN)
                    || attendanceLogDtoList[i].Status == "Clocked In")
                {
                    clockInRoleId = attendanceLogDtoList[i].AttendanceRoleId;
                }
                else if (attendanceLogDtoList[i].AttendanceRoleId != clockInRoleId)
                {
                    ValidationError validationError = new ValidationError(attendanceLogDtoList[i].Timestamp.ToString(), null, MessageContainerList.GetMessage(executionContext, 3089), attendanceLogDtoList[i].AttendanceLogId);
                    validationErrorList.Add(validationError);
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3089), validationErrorList);//'Attendance Modification is Invalid.Attendance Role is not in the correct order.'
                }
                if (type == attendanceLogDtoList[i].Type)
                {
                    ValidationError validationError = new ValidationError(attendanceLogDtoList[i].Timestamp.ToString(), null, MessageContainerList.GetMessage(executionContext, 3088), attendanceLogDtoList[i].AttendanceLogId);
                    validationErrorList.Add(validationError);
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3088), validationErrorList, i);//"Attendance Modification is Invalid: Attendance sequence is not in the correct order"
                }
                if(!string.IsNullOrEmpty(lastLoggedinStatus)
                    && lastLoggedinStatus == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK)
                    && attendanceLogDtoList[i].Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK))
                {
                    ValidationError validationError = new ValidationError(attendanceLogDtoList[i].Timestamp.ToString(), null, MessageContainerList.GetMessage(executionContext, 3088), attendanceLogDtoList[i].AttendanceLogId);
                    validationErrorList.Add(validationError);
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3088), validationErrorList, i);//"Attendance Modification is Invalid: Attendance sequence is not in the correct order"
                }
                if (!string.IsNullOrEmpty(lastLoggedinStatus)
                    && attendanceLogDtoList[i].Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK)
                    && lastLoggedinStatus !=  AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))
                {
                    ValidationError validationError = new ValidationError(attendanceLogDtoList[i].Timestamp.ToString(), null, MessageContainerList.GetMessage(executionContext, 3088), attendanceLogDtoList[i].AttendanceLogId);
                    validationErrorList.Add(validationError);
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3088), validationErrorList, i);//"Attendance Modification is Invalid: Attendance sequence is not in the correct order"
                }
                type = attendanceLogDtoList[i].Type;
                lastLoggedinStatus = attendanceLogDtoList[i].Status;
            }
            if (attendanceLogDtoList[attendanceLogDtoList.Count - 1].Status != "Clocked Out" &&
                attendanceLogDtoList[attendanceLogDtoList.Count - 1].Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
            {
                ValidationError validationError = new ValidationError(attendanceLogDtoList[attendanceLogDtoList.Count - 1].Timestamp.ToString(), null,
                                MessageContainerList.GetMessage(executionContext, 3087, attendanceLogDtoList[attendanceLogDtoList.Count - 1].Status),
                                attendanceLogDtoList[attendanceLogDtoList.Count - 1].AttendanceLogId);
                validationErrorList.Add(validationError);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3087, attendanceLogDtoList[attendanceLogDtoList.Count - 1].Status), validationErrorList); //'Invalid attendance. No clock-out record found for day.Last logged in attendance status is &1'
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AttendanceDTO getAttendanceDTO { get { return attendanceDTO; } }
    }

    /// <summary>
    /// Manages the list of attendance
    /// </summary>
    public class AttendanceList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AttendanceDTO> attendanceDTOList = new List<AttendanceDTO>();

        /// <summary>
        /// Paramterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public AttendanceList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="attendanceDTOList">attendanceDTOList</param>
        public AttendanceList(ExecutionContext executionContext, List<AttendanceDTO> attendanceDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceDTOList);
            this.attendanceDTOList = attendanceDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the Attendance list
        /// </summary>
        public List<AttendanceDTO> GetAllAttendance(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler(sqlTransaction);
            List<AttendanceDTO> attendanceDTOList = attendanceDataHandler.GetAttendanceList(searchParameters, sqlTransaction);
            if (attendanceDTOList != null && attendanceDTOList.Any() && loadChildRecords)
            {
                Build(attendanceDTOList, activeChildRecords, sqlTransaction);
            }

            log.LogMethodExit(attendanceDTOList);
            return attendanceDTOList;
        }
        /// <summary>
        /// Returns the Attendance list
        /// </summary>
        public List<AttendanceDTO> GetAttendance(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler(sqlTransaction);
            List<AttendanceDTO> attendanceDTOList = attendanceDataHandler.GetAttendanceList(searchParameters, sqlTransaction);
            if (attendanceDTOList != null && attendanceDTOList.Any() && loadChildRecords)
            {
                Build(attendanceDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(attendanceDTOList);
            return attendanceDTOList;
        }

        /// <summary>
        /// Builds the List of AttendanceBL object based on the list of Attendance id.
        /// </summary>
        /// <param name="attendanceDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<AttendanceDTO> attendanceDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(attendanceDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AttendanceDTO> attendanceidAttendanceDictionary = new Dictionary<int, AttendanceDTO>();
            StringBuilder sb = new StringBuilder(string.Empty);
            string attendanceIdSet;
            for (int i = 0; i < attendanceDTOList.Count; i++)
            {
                if (attendanceDTOList[i].AttendanceId == -1 ||
                    attendanceidAttendanceDictionary.ContainsKey(attendanceDTOList[i].AttendanceId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(attendanceDTOList[i].AttendanceId);
                attendanceidAttendanceDictionary.Add(attendanceDTOList[i].AttendanceId, attendanceDTOList[i]);
            }

            attendanceIdSet = sb.ToString();

            // Build child Records - AttendanceLogDTO
            AttendanceLogList attendanceLogList = new AttendanceLogList(executionContext);
            List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>> searchAttendanceLogParams = new List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>>();
            searchAttendanceLogParams.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.ATTENDANCE_ID_LIST, attendanceIdSet.ToString()));
            if (activeChildRecords)
            {
                searchAttendanceLogParams.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.ISACTIVE, "Y"));
            }
            List<AttendanceLogDTO> attendanceLogDTOList = attendanceLogList.GetAllAttendanceUserList(searchAttendanceLogParams, sqlTransaction);
            if (attendanceLogDTOList != null && attendanceLogDTOList.Any())
            {
                log.LogVariableState("AttendanceLogDTOList", attendanceLogDTOList);
                foreach (var attendanceLogDTO in attendanceLogDTOList)
                {
                    if (attendanceidAttendanceDictionary.ContainsKey(attendanceLogDTO.AttendanceId))
                    {
                        if (attendanceidAttendanceDictionary[attendanceLogDTO.AttendanceId].AttendanceLogDTOList == null)
                        {
                            attendanceidAttendanceDictionary[attendanceLogDTO.AttendanceId].AttendanceLogDTOList = new List<AttendanceLogDTO>();
                        }
                        attendanceidAttendanceDictionary[attendanceLogDTO.AttendanceId].AttendanceLogDTOList.Add(attendanceLogDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AttendanceLog List
        /// </summary>
        public List<AttendanceDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<AttendanceDTO> savedAttendanceDTOList = new List<AttendanceDTO>();
            if (attendanceDTOList == null ||
               attendanceDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return savedAttendanceDTOList;
            }

            for (int i = 0; i < attendanceDTOList.Count; i++)
            {
                var attendanceDTO = attendanceDTOList[i];
                if (attendanceDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTO);
                    attendanceBL.Save(sqlTransaction);
                    savedAttendanceDTOList.Add(attendanceBL.getAttendanceDTO);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AttendanceDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AttendanceDTOList", attendanceDTO);
                    throw;
                }
            }
            log.LogMethodExit(savedAttendanceDTOList);
            return savedAttendanceDTOList;
        }

        public int GetAttendanceCount(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler(sqlTransaction);
            int attendanceCount = attendanceDataHandler.GetAttendanceCount(searchParameters);
            log.LogMethodExit(attendanceCount);
            return attendanceCount;
        }

        public List<AttendanceDTO> GetAttendanceRequestStatusList(DateTime fromDate, DateTime toDate, int userId)
        {
            log.LogMethodEntry(fromDate, toDate);
            List<AttendanceDTO> attendanceRequestDTOList = new List<AttendanceDTO>();
            List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>();
            searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.FROM_DATE, fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.TO_DATE, toDate.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.USER_ID, userId.ToString()));
            searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AttendanceDTO> attendanceDTOList = GetAllAttendance(searchByParams, true, false);
            if (attendanceDTOList != null && attendanceDTOList.Any())
            {
                foreach (AttendanceDTO attendanceDTO in attendanceDTOList)
                {
                    if (attendanceDTO.AttendanceLogDTOList != null && attendanceDTO.AttendanceLogDTOList.Any()
                        && attendanceDTO.AttendanceLogDTOList.Exists(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Rejected.ToString()
                                    || x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()
                                    || x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString()))
                    {
                        attendanceDTO.AttendanceLogDTOList = attendanceDTO.AttendanceLogDTOList.OrderByDescending(x => x.LastUpdatedDate).ToList();
                        foreach (AttendanceLogDTO logDTO in attendanceDTO.AttendanceLogDTOList)
                        {
                            if (logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Rejected.ToString())
                            {
                                attendanceDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Rejected.ToString();
                                attendanceRequestDTOList.Add(attendanceDTO);
                                break;
                            }
                            else if (logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString())
                            {
                                attendanceDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString();
                                attendanceRequestDTOList.Add(attendanceDTO);
                                break;
                            }
                            else if (logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString())
                            {
                                attendanceDTO.RequestStatus = AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString();
                                attendanceRequestDTOList.Add(attendanceDTO);
                                break;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(attendanceRequestDTOList);
            return attendanceRequestDTOList;
        }
    }
}
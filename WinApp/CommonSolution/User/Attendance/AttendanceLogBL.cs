/********************************************************************************************
 * Project Name - AttendanceLog BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018      Indhu               Created 
 *2.70.2      15-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.90        20-May-2020      Vikas Dwivedi       Modified as per the Standard CheckList
 *2.140.0     14-Sep-2021      Deeksha             Modified : Provisional shift & Cash drawer related changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business Logic for AttendanceLogBL
    /// </summary>
    public class AttendanceLogBL
    {
        private AttendanceLogDTO attendanceLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of AttendanceLogBL
        /// </summary>
        private AttendanceLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AttendanceLogBL object using the AttendanceLogDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="attendanceLogDTO">attendanceLogDTO</param>
        public AttendanceLogBL(ExecutionContext executionContext, AttendanceLogDTO attendanceLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceLogDTO);
            this.attendanceLogDTO = attendanceLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the attendanceLog id as the parameter
        /// Would fetch the attendanceLog object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="leaveId">id of attendanceLog Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AttendanceLogBL(ExecutionContext executionContext, int attendanceLogId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceLogId, sqlTransaction);
            AttendanceLogDataHandler attendanceLogDataHandler = new AttendanceLogDataHandler(sqlTransaction);
            attendanceLogDTO = attendanceLogDataHandler.GetAttendanceLog(attendanceLogId);
            if (attendanceLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AttendanceLogDTO", attendanceLogId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the attendanceLog record
        /// Checks if the AttendanceLogId is not less than 0
        ///     If it is less than 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AttendanceLogDataHandler attendanceLogDataHandler = new AttendanceLogDataHandler(sqlTransaction);
            if (attendanceLogDTO.AttendanceLogId < 0)
            {
                attendanceLogDTO = attendanceLogDataHandler.InsertAttendanceLog(attendanceLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                attendanceLogDTO.AcceptChanges();
            }
            else
            {
                if (attendanceLogDTO.IsChanged)
                {
                    attendanceLogDTO = attendanceLogDataHandler.UpdateAttendanceLog(attendanceLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    attendanceLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AttendanceLogDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AttendanceLogDTO getAttendanceLogDTO { get { return attendanceLogDTO; } }
    }

    /// <summary>
    /// Manages the list of attendanceLog
    /// </summary>
    public class AttendanceLogList
    {
        private static  readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AttendanceLogDTO attendanceLogDTO;
        private List<AttendanceLogDTO> attendanceLogDTOList = new List<AttendanceLogDTO>();

        /// <summary>
        /// Paramterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public AttendanceLogList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="attendanceLogDTOList">attendanceLogDTOList</param>
        public AttendanceLogList(ExecutionContext executionContext, List<AttendanceLogDTO> attendanceLogDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceLogDTOList);
            this.attendanceLogDTOList = attendanceLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the attendanceLog
        /// </summary>

        public AttendanceLogDTO getLastLogin(int userId ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userId , sqlTransaction);
            AttendanceLogDataHandler attendanceLogDataHandler = new AttendanceLogDataHandler(sqlTransaction);
            attendanceLogDTO = attendanceLogDataHandler.getLastLogin(userId);
            log.LogMethodExit(attendanceLogDTO);
            return attendanceLogDTO;
        }

        /// <summary>
        /// Returns the attendanceLog list
        /// </summary>
        public List<AttendanceLogDTO> GetAllAttendanceUserList(List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters ,sqlTransaction);
            AttendanceLogDataHandler attendanceLogDataHandler = new AttendanceLogDataHandler(sqlTransaction);
            List<AttendanceLogDTO> attendanceLogDTOList = attendanceLogDataHandler.GetAttendanceLog(searchParameters);
            if(attendanceLogDTOList != null && attendanceLogDTOList.Any())
                CalculateAttendanceBreakStatus(attendanceLogDTOList);
            log.LogMethodExit(attendanceLogDTOList);
            return attendanceLogDTOList;
        }

        /// <summary>
        /// Saves the AttendanceLog List
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (attendanceLogDTOList == null ||
               attendanceLogDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            CalculateAttendanceBreakStatus(attendanceLogDTOList);
            for (int i = 0; i < attendanceLogDTOList.Count; i++)
            {
                var attendanceLogDTO = attendanceLogDTOList[i];
                if (attendanceLogDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AttendanceLogBL attendanceLogBL = new AttendanceLogBL(executionContext, attendanceLogDTO);
                    attendanceLogBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AttendanceLogDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AttendanceLogDTOList", attendanceLogDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }


        private void CalculateAttendanceBreakStatus(List<AttendanceLogDTO> attnLogDTOList)
        {
            try
            {
                List<AttendanceLogDTO> attendanceLogDTOList = new List<AttendanceLogDTO>();
                log.LogMethodEntry();
                {
                    attnLogDTOList = attnLogDTOList.OrderBy(x => x.Timestamp).ToList();
                    foreach (AttendanceLogDTO logDTO in attnLogDTOList)
                    {
                        if ((logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Rejected.ToString()
                                || logDTO.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Invalid.ToString()
                                || logDTO.IsActive == "N"))
                        {
                            continue;
                        }
                        if (attnLogDTOList.Exists(x => x.OriginalAttendanceLogId == logDTO.AttendanceLogId))
                        {
                            List<AttendanceLogDTO> logDTOList = attnLogDTOList.FindAll(x => x.OriginalAttendanceLogId == logDTO.AttendanceLogId).ToList();
                            if (logDTOList != null && logDTOList.Exists(x => x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Submitted.ToString()))
                                continue;
                        }
                        
                        attendanceLogDTOList.Add(logDTO);
                    }
                }
                int MinBreakTime = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MINIMUM_BREAK_TIME"));
                int MaxBreakTime = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MAXIMUM_BREAK_TIME"));
                DateTime breakStartTime = DateTime.Now ;
                for (int i = 0; i < attendanceLogDTOList.Count; i++)
                {
                    attendanceLogDTOList[i].Notes = string.Empty;
                    if (attendanceLogDTOList[i].Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK))
                    {
                        breakStartTime = Convert.ToDateTime(attendanceLogDTOList[i].Timestamp);
                    }
                    else if (attendanceLogDTOList[i].Status == AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.BACK_FROM_BREAK))
                    {
                        string status = string.Empty;
                        if (MinBreakTime > 0)
                        {
                            int elapsedMinutes = (int)(attendanceLogDTOList[i].Timestamp - breakStartTime).TotalMinutes;
                            if (elapsedMinutes < 60)
                            {
                                if (elapsedMinutes < MinBreakTime)
                                    status = "Early From Break" + " (" + elapsedMinutes + " Mins" + ").";
                                else if (elapsedMinutes > MaxBreakTime)
                                    status = "Late From Break" + " (" + elapsedMinutes + " Mins" + ").";
                                else
                                    status = "On Time.";
                            }
                            else
                            {
                                var timeSpan = TimeSpan.FromMinutes(elapsedMinutes);
                                if (elapsedMinutes < MinBreakTime)
                                    status = "Early From Break" + " (" + timeSpan.Hours + " Hour " + timeSpan.Minutes + " Mins" + ").";
                                else if (elapsedMinutes > MaxBreakTime)
                                    status = "Late From Break" + " (" + timeSpan.Hours + " Hour " + timeSpan.Minutes + " Mins" + ").";
                                else
                                    status = "On Time.";
                            }
                            attendanceLogDTOList[i].Notes = status;
                        }

                    }
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        internal List<AttendanceLogDTO> GetAllClockedInUsers()
        {
            log.LogMethodEntry();
            int workshiftTime = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_WORKSHIFT_STARTTIME"));
            DateTime workshiftStartDate = DateTime.Today.AddHours(workshiftTime);
            List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.STATUS, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN)));
            searchParams.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.FROM_DATE, workshiftStartDate.ToString("yyyy-MM-dd HH:mm:ss")));
            searchParams.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AttendanceLogDTO> attendanceLogDTOList = GetAllAttendanceUserList(searchParams);
            log.LogMethodExit(attendanceLogDTOList);
            return attendanceLogDTOList;
        }
    }
}

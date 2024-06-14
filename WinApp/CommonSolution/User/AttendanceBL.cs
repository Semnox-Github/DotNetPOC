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
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                return;
            }
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
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
                List<AttendanceLogDTO> updatedAttendanceLogDTOList = new List<AttendanceLogDTO>();
                foreach (var attendanceLogDTO in attendanceDTO.AttendanceLogDTOList)
                {
                    if (attendanceLogDTO.AttendanceId != attendanceDTO.AttendanceId)
                    {
                        attendanceLogDTO.AttendanceId = attendanceDTO.AttendanceId;
                    }
                    if (attendanceLogDTO.IsChanged)
                    {
                        updatedAttendanceLogDTOList.Add(attendanceLogDTO);
                    }
                }
                if (updatedAttendanceLogDTOList.Any())
                {
                    log.LogVariableState("UpdatedAttendanceLogDTOList", updatedAttendanceLogDTOList);
                    AttendanceLogList attendanceLogList = new AttendanceLogList(executionContext, updatedAttendanceLogDTOList);
                    attendanceLogList.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AttendanceDTO
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
                searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.ISACTIVE, "1"));
            }
            attendanceDTO.AttendanceLogDTOList = attendanceLogList.GetAllAttendanceUserList(searchParameters, sqlTransaction);
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
    }
}
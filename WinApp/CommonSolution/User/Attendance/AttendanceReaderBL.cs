/********************************************************************************************
* Project Name - HR Module
* Description  - BL Class for Attendance Reader
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70.0      15-Oct-2019  Indrajeet Kumar          Created 
*2.90        25-Jun-2020   Vikas Dwivedi           Modified as per the Standard CheckList
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business Logic for AttendanceReaderBL
    /// </summary>
    public class AttendanceReaderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AttendanceReaderDTO attendanceReaderDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of AttendanceReaderBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private AttendanceReaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LeaveTemplateBL object using the AttendanceReaderDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="attendanceReaderDTO">attendanceReaderDTO</param>
        public AttendanceReaderBL(ExecutionContext executionContext, AttendanceReaderDTO attendanceReaderDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceReaderDTO);
            this.attendanceReaderDTO = attendanceReaderDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the attendanceReader id as the parameter
        /// Would fetch the attendanceReader object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="attendanceReaderId">id of AttendanceReader Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AttendanceReaderBL(ExecutionContext executionContext, int attendanceReaderId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceReaderId, sqlTransaction);
            AttendanceReaderDataHandler attendanceReaderDataHandler = new AttendanceReaderDataHandler(sqlTransaction);
            attendanceReaderDTO = attendanceReaderDataHandler.GetAttendanceReaderDTO(attendanceReaderId);
            if (attendanceReaderDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AttendanceReaderDTO", attendanceReaderId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the AttendanceReaderDTO based on AttendanceReaderId
        /// </summary>
        internal void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                AttendanceReaderDataHandler attendanceReaderDataHandler = new AttendanceReaderDataHandler(sqlTransaction);
                attendanceReaderDataHandler.Delete(attendanceReaderDTO.Id);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting AttendanceReaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the AttendanceReaderDTO
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
        /// Gets AttendanceReaderDTO Object
        /// </summary>
        public AttendanceReaderDTO GetAttendanceReaderDTO
        {
            get { return attendanceReaderDTO; }
        }

        /// <summary>
        /// Save Method
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AttendanceReaderDataHandler attendanceReaderDataHandler = new AttendanceReaderDataHandler(sqlTransaction);
            if (attendanceReaderDTO.Id < 0)
            {
                attendanceReaderDTO = attendanceReaderDataHandler.Insert(attendanceReaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                attendanceReaderDTO.AcceptChanges();
            }
            else
            {
                if (attendanceReaderDTO.IsChanged)
                {
                    attendanceReaderDataHandler.Update(attendanceReaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    attendanceReaderDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of AttendanceReader BL
    /// </summary>
    public class AttendanceReaderListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AttendanceReaderDTO> attendanceReaderDTOList = new List<AttendanceReaderDTO>();
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public AttendanceReaderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="attendanceReaderDTOList">attendanceReaderDTOList</param>
        public AttendanceReaderListBL(ExecutionContext executionContext, List<AttendanceReaderDTO> attendanceReaderDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attendanceReaderDTOList);
            this.attendanceReaderDTOList = attendanceReaderDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Attendance Reader List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AttendanceReaderDTO> GetAllAttendanceReaderList(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceReaderDataHandler attendanceReaderDataHandler = new AttendanceReaderDataHandler(sqlTransaction);
            List<AttendanceReaderDTO> attendanceReaderDTOList = attendanceReaderDataHandler.GetAttendanceReaderDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(attendanceReaderDTOList);
            return attendanceReaderDTOList;
        }

        /// <summary>
        /// Returns the Attendance Reader List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AttendanceReaderDTO> GetAllAttendanceReaderLists(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceReaderDataHandler attendanceReaderDataHandler = new AttendanceReaderDataHandler(sqlTransaction);
            List<AttendanceReaderDTO> attendanceReaderDTOList = attendanceReaderDataHandler.GetAttendanceReaderLists(searchParameters, currentPage, pageSize, sqlTransaction);
            log.LogMethodExit(attendanceReaderDTOList);
            return attendanceReaderDTOList;
        }

        /// <summary>
        /// Save the AttendanceReader List 
        /// </summary>
        public List<AttendanceReaderDTO> SaveUpdateAttendanceReader(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<AttendanceReaderDTO> savedAttendanceReaderDTOList = new List<AttendanceReaderDTO>();
            if (attendanceReaderDTOList == null ||
               attendanceReaderDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return savedAttendanceReaderDTOList;
            }

            for (int i = 0; i < attendanceReaderDTOList.Count; i++)
            {
                var attendanceReaderDTO = attendanceReaderDTOList[i];
                if (attendanceReaderDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AttendanceReaderBL attendanceReaderBL = new AttendanceReaderBL(executionContext, attendanceReaderDTO);
                    attendanceReaderBL.Save(sqlTransaction);
                    savedAttendanceReaderDTOList.Add(attendanceReaderBL.GetAttendanceReaderDTO);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AttendanceReaderDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AttendanceReaderDTOList", attendanceReaderDTO);
                    throw;
                }
            }
            log.LogMethodExit(savedAttendanceReaderDTOList);
            return savedAttendanceReaderDTOList;
        }

        /// <summary>
        /// Delete the Department
        /// <summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (attendanceReaderDTOList != null && attendanceReaderDTOList.Any())
            {
                foreach (AttendanceReaderDTO attendanceReaderDTO in attendanceReaderDTOList)
                {
                    if (attendanceReaderDTO.IsChanged)
                    {
                        try
                        {
                            AttendanceReaderBL attendanceReaderBL = new AttendanceReaderBL(executionContext, attendanceReaderDTO);
                            attendanceReaderBL.Delete(sqlTransaction);
                        }
                        catch (ValidationException valEx)
                        {
                            log.Error(valEx);
                            log.LogMethodExit(null, "Throwing validation Exception : " + valEx.Message);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        public int GetAttendanceReaderCount(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceReaderDataHandler attendanceReaderDataHandler = new AttendanceReaderDataHandler(sqlTransaction);
            int attendanceReaderCount = attendanceReaderDataHandler.GetAttendanceReaderCount(searchParameters);
            log.LogMethodExit(attendanceReaderCount);
            return attendanceReaderCount;
        }
    }
}

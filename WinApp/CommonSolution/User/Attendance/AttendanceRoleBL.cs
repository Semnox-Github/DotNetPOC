/********************************************************************************************
 * Project Name - AttendanceRoles BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018      Indhu               Created 
 *2.70        08-May-2019      Mushahid Faizan     Added SQL Transaction in SaveUpdateAttendanceRolesList() method.
 *2.70.2        15-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *            07-Aug-2019      Mushahid Faizan     Added Delete in Save method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.User
{
    public class AttendanceRoleBL
    {
        private AttendanceRoleDTO attendanceRoleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of AttendanceLog class
        /// </summary>
        public AttendanceRoleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the AttendanceLog DTO based on the attendanceLog id passed 
        /// </summary>
        /// <param name="attendanceRoleId">AttendanceLog id</param>
        public AttendanceRoleBL(ExecutionContext executionContext ,int attendanceRoleId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(attendanceRoleId, sqlTransaction);
            AttendanceRoleDataHandler attendanceRoleDataHandler = new AttendanceRoleDataHandler(sqlTransaction);
            attendanceRoleDTO = attendanceRoleDataHandler.GetAttendanceRoles(attendanceRoleId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates attendanceLog object using the AttendanceRolesDTO
        /// </summary>
        /// <param name="attendanceRoleDTO">AttendanceRolesDTO object</param>
        public AttendanceRoleBL(ExecutionContext executionContext ,AttendanceRoleDTO attendanceRoleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(attendanceRoleDTO);
            this.attendanceRoleDTO = attendanceRoleDTO;
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
            AttendanceRoleDataHandler attendanceRoleDTODataHandler = new AttendanceRoleDataHandler(sqlTransaction);
            if (attendanceRoleDTO.IsActive)
            {
                if (attendanceRoleDTO.Id < 0)
                {
                    attendanceRoleDTO = attendanceRoleDTODataHandler.InsertAttendanceRoles(attendanceRoleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(attendanceRoleDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("user_roles", attendanceRoleDTO.Guid, sqlTransaction);
                    }
                    attendanceRoleDTO.AcceptChanges();
                }
                else
                {
                    if (attendanceRoleDTO.IsChanged)
                    {
                        attendanceRoleDTO = attendanceRoleDTODataHandler.UpdateAttendanceRoles(attendanceRoleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        if (!string.IsNullOrEmpty(attendanceRoleDTO.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("user_roles", attendanceRoleDTO.Guid, sqlTransaction);
                        }
                        attendanceRoleDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (attendanceRoleDTO.Id > 0)
                {
                    attendanceRoleDTODataHandler.Delete(attendanceRoleDTO.Id);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AttendanceRoleDTO GetAttendanceRolesDTO { get { return attendanceRoleDTO; } }
    }

    /// <summary>
    /// Manages the list of attendanceRoleDTO
    /// </summary>
    public class AttendanceRolesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<AttendanceRoleDTO> attendanceRoleDTOsList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public AttendanceRolesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="attendanceRoleDTOsList"></param>
        public AttendanceRolesList(ExecutionContext executionContext, List<AttendanceRoleDTO> attendanceRoleDTOsList)
        {
            log.LogMethodEntry(executionContext, attendanceRoleDTOsList);
            this.executionContext = executionContext;
            this.attendanceRoleDTOsList = attendanceRoleDTOsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the attendanceRoleDTO list
        /// </summary>
        public List<AttendanceRoleDTO> GetAttendanceRoles(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceRoleDataHandler attendanceRoleDTODataHandler = new AttendanceRoleDataHandler(sqlTransaction);
            List<AttendanceRoleDTO> attendanceRoleDTOList = attendanceRoleDTODataHandler.GetAttendanceRoles(searchParameters);
            log.LogMethodExit(attendanceRoleDTOList);
            return attendanceRoleDTOList;
        }

        /// <summary>
        /// Returns the attendanceRoleDTO list
        /// </summary>
        public List<AttendanceRoleDTO> GetAttendanceRoleLists(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null) //added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceRoleDataHandler attendanceRoleDTODataHandler = new AttendanceRoleDataHandler(sqlTransaction);
            List<AttendanceRoleDTO> attendanceRoleDTOList = attendanceRoleDTODataHandler.GetAttendanceRoleLists(searchParameters, currentPage, pageSize, sqlTransaction);
            log.LogMethodExit(attendanceRoleDTOList);
            return attendanceRoleDTOList;
        }


        /// <summary>
        /// This method should be used to Save and Update the Attendance Roles details for Web Management Studio.
        /// </summary>
        public List<AttendanceRoleDTO> SaveUpdateAttendanceRolesList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<AttendanceRoleDTO> savedAttendanceRoleDTOList = new List<AttendanceRoleDTO>();
            if (attendanceRoleDTOsList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (AttendanceRoleDTO attendanceRoleDTO in attendanceRoleDTOsList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AttendanceRoleBL attendanceRoleBL = new AttendanceRoleBL(executionContext ,attendanceRoleDTO);
                            attendanceRoleBL.Save(parafaitDBTrx.SQLTrx);
                            savedAttendanceRoleDTOList.Add(attendanceRoleBL.GetAttendanceRolesDTO);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();
            }
            return savedAttendanceRoleDTOList;
        }

        public int GetAttendanceRolesCount(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttendanceRoleDataHandler attendanceRoleDataHandler = new AttendanceRoleDataHandler(sqlTransaction);
            int attendanceRolesCount = attendanceRoleDataHandler.GetAttendanceRolesCount(searchParameters, sqlTransaction);
            log.LogMethodExit(attendanceRolesCount);
            return attendanceRolesCount;
        }
    }
}

/********************************************************************************************
 * Project Name - AttendanceRoles Data Handler
 * Description  - Data handler of the AttendanceRoles class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018      Indhu               Created
 *2.60        08-May-2019      Mushahid Faizan     Added ISACTIVE DBSearchParameters and handled IsActive datatype from string to bool
                                                   Modified Insert/Update query.
 *2.70.2        15-Jul-2019      Girish Kundar       Modified : Added GetSQLParameter(),SQL Injection Fix.
 *2.70.2        11-Dec-2019      Jinto Thomas        Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the AttendanceRoles data object class. This acts as data holder for the AttendanceRoles business object
    /// </summary>
    public class AttendanceRoleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AttendanceRoleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AttendanceRoleDTO.SearchByParameters, string>
        {
                {AttendanceRoleDTO.SearchByParameters.ID, "ar.Id"},
                {AttendanceRoleDTO.SearchByParameters.ROLE_ID, "ar.RoleId"},
                {AttendanceRoleDTO.SearchByParameters.ATTENDANCE_ROLE_ID, "ar.AttendanceRoleId"},
                {AttendanceRoleDTO.SearchByParameters.MASTER_ENTITY_ID, "ar.MasterEntityId"},
                {AttendanceRoleDTO.SearchByParameters.SITE_ID, "ar.site_id"},
                { AttendanceRoleDTO.SearchByParameters.ISACTIVE, "ar.IsActive"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added
        private const string SELECT_QUERY = @"SELECT * FROM AttendanceRoles AS ar ";
        /// <summary>
        /// Parameterized constructor of AttendanceRolesDataHandler class
        /// </summary>
        public AttendanceRoleDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AttendanceRole Record.
        /// </summary>
        /// <param name="attendanceRoleDTO">AttendanceRoleDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(AttendanceRoleDTO attendanceRoleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceRoleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@Id", attendanceRoleDTO.Id, true);
            ParametersHelper.ParameterHelper(parameters, "@roleId", attendanceRoleDTO.RoleId, true);
            ParametersHelper.ParameterHelper(parameters, "@attendanceRoleId", attendanceRoleDTO.AttendanceRoleId, true);
            ParametersHelper.ParameterHelper(parameters, "@approvalRequired", attendanceRoleDTO.ApprovalRequired);
            ParametersHelper.ParameterHelper(parameters, "@isActive", (attendanceRoleDTO.IsActive ? "Y" : "N"));
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@siteId", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", attendanceRoleDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the AttendanceRoles record to the database
        /// </summary>
        /// <param name="attendanceRolesDTO">AttendanceRolesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AttendanceRoleDTO</returns>
        public AttendanceRoleDTO InsertAttendanceRoles(AttendanceRoleDTO attendanceRolesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceRolesDTO, loginId, siteId);
            string InsertAttendanceRolesQuery = @"insert into AttendanceRoles
                                                    (
                                                        RoleId,
                                                        AttendanceRoleId,
                                                        ApprovalRequired,
                                                        IsActive,
                                                        CreationDate,
                                                        CreatedBy,
                                                        LastUpdatedDate,
                                                        LastUpdatedBy,
                                                        site_id,
                                                        GUID,
                                                        MasterEntityId
                                                    ) 
                                                values 
                                                    (
                                                        @roleId,
                                                        @attendanceRoleId,
                                                        @approvalRequired,
                                                        @isActive,
                                                        Getdate(),
                                                        @createdBy,
                                                        Getdate(),
                                                        @lastUpdatedBy,
                                                        @siteId,
                                                        NewId(),
                                                        @masterEntityId
                                                    ) SELECT  * from AttendanceRoles where Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertAttendanceRolesQuery, BuildSQLParameters(attendanceRolesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttendanceRolesDTO(attendanceRolesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting attendanceRolesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attendanceRolesDTO);
            return attendanceRolesDTO;
        }

        /// <summary>
        /// Updates the AttendanceRoles record
        /// </summary>
        /// <param name="attendanceRolesDTO">AttendanceRolesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AttendanceRoleDTO</returns>
        public AttendanceRoleDTO UpdateAttendanceRoles(AttendanceRoleDTO attendanceRolesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceRolesDTO, loginId, siteId);
            string updateAttendanceRolesQuery = @"update AttendanceRoles 
                                                        set RoleId = @roleId,
                                                        AttendanceRoleId = @attendanceRoleId,
                                                        ApprovalRequired = @approvalRequired,
                                                        IsActive = @isActive,
                                                        LastUpdatedDate = Getdate(),
                                                        LastUpdatedBy = @lastUpdatedBy,
                                                        -- site_id = @siteId,
                                                        MasterEntityId = @MasterEntityId
                                                        where Id = @Id
                                                     SELECT* from AttendanceRoles where Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAttendanceRolesQuery, BuildSQLParameters(attendanceRolesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttendanceRolesDTO(attendanceRolesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating attendanceRolesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attendanceRolesDTO);
            return attendanceRolesDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="attendanceRoleDTO">AttendanceRoleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAttendanceRolesDTO(AttendanceRoleDTO attendanceRoleDTO, DataTable dt)
        {
            log.LogMethodEntry(attendanceRoleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                attendanceRoleDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                attendanceRoleDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                attendanceRoleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                attendanceRoleDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                attendanceRoleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                attendanceRoleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to AttendanceRolesDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AttendanceRolesDTO</returns>
        private AttendanceRoleDTO GetAttendanceRolesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AttendanceRoleDTO attendanceDTO = new AttendanceRoleDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["RoleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RoleId"]),
                                            dataRow["AttendanceRoleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AttendanceRoleId"]),
                                            dataRow["ApprovalRequired"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ApprovalRequired"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : (dataRow["IsActive"].ToString() == "Y" ? true : false),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.Debug(attendanceDTO);
            return attendanceDTO;
        }

        /// Gets the AttendanceRoles data of passed attendanceRoles Id
        /// </summary>
        /// <param name="attendanceRolesId">integer type parameter</param>
        /// <returns>Returns AttendanceRolesDTO</returns>
        public AttendanceRoleDTO GetAttendanceRoles(int attendanceRolesId)
        {
            log.LogMethodEntry(attendanceRolesId);
            string selectAttendanceRolesQuery = SELECT_QUERY + "  WHERE ar.Id = @Id";
            AttendanceRoleDTO attendanceRolesDataObject = null;
            SqlParameter[] selectAttendanceRolesParameters = new SqlParameter[1];
            selectAttendanceRolesParameters[0] = new SqlParameter("@Id", attendanceRolesId);
            DataTable attendanceRoles = dataAccessHandler.executeSelectQuery(selectAttendanceRolesQuery, selectAttendanceRolesParameters, sqlTransaction);
            if (attendanceRoles.Rows.Count > 0)
            {
                DataRow AttendanceRolesRow = attendanceRoles.Rows[0];
                attendanceRolesDataObject = GetAttendanceRolesDTO(AttendanceRolesRow);

            }
            log.LogMethodExit(attendanceRolesDataObject);
            return attendanceRolesDataObject;

        }

        /// <summary>
        /// Gets the AttendanceRoleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AttendanceRoleDTO matching the search criteria</returns>
        public List<AttendanceRoleDTO> GetAttendanceRoleLists(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters);
            List<AttendanceRoleDTO> attendanceRoleDTOList = new List<AttendanceRoleDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters, sqlTransaction);
            if (currentPage > -1 && pageSize > 0)
            {
                selectQuery += " ORDER BY ar.Id OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                attendanceRoleDTOList = new List<AttendanceRoleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AttendanceRoleDTO attendanceRoleDTO = GetAttendanceRolesDTO(dataRow);
                    attendanceRoleDTOList.Add(attendanceRoleDTO);
                }
            }
            log.LogMethodExit(attendanceRoleDTOList);
            return attendanceRoleDTOList;
        }

        /// <summary>
        /// Returns the no of AttendanceRole matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetAttendanceRolesCount(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null) //added
        {
            log.LogMethodEntry(searchParameters);
            int attendanceRoleDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            parameters.Clear();
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                attendanceRoleDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(attendanceRoleDTOCount);
            return attendanceRoleDTOCount;
        }

        /// <summary>
        /// Gets the AttendanceRoleDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AttendanceRoleDTO matching the search criteria</returns>
        public List<AttendanceRoleDTO> GetAttendanceRoles(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<AttendanceRoleDTO> attendanceRoleDTOList = new List<AttendanceRoleDTO>();
            parameters.Clear();
            string selectAttendanceRoleDTOQuery = SELECT_QUERY;
            selectAttendanceRoleDTOQuery = selectAttendanceRoleDTOQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dtAttendanceRoleDTO = dataAccessHandler.executeSelectQuery(selectAttendanceRoleDTOQuery, parameters.ToArray(), sqlTransaction);
            if (dtAttendanceRoleDTO.Rows.Count > 0)
            {
                foreach (DataRow attendanceRoleDTORow in dtAttendanceRoleDTO.Rows)
                {
                    AttendanceRoleDTO attendanceRoleDTO = GetAttendanceRolesDTO(attendanceRoleDTORow);
                    attendanceRoleDTOList.Add(attendanceRoleDTO);
                }

            }
            log.LogMethodExit(attendanceRoleDTOList);
            return attendanceRoleDTOList;
        }


        /// <summary>
        /// Gets the AttendanceRolesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of attendanceRolesDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            StringBuilder query = new StringBuilder(" ");
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                query = new StringBuilder(" where ");
                foreach (KeyValuePair<AttendanceRoleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(AttendanceRoleDTO.SearchByParameters.ID) ||
                                searchParameter.Key.Equals(AttendanceRoleDTO.SearchByParameters.ROLE_ID) ||
                                 searchParameter.Key.Equals(AttendanceRoleDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                searchParameter.Key.Equals(AttendanceRoleDTO.SearchByParameters.ATTENDANCE_ROLE_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == AttendanceRoleDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
            }
            log.LogMethodExit();
            return query.ToString();

        }
        /// <summary>
        /// Based on the Id, appropriate AttendanceRoles record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required 
        /// </summary>
        /// <param name="id">id is passed as parameter</param>
        internal void Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM AttendanceRoles
                             WHERE AttendanceRoles.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
    }
}

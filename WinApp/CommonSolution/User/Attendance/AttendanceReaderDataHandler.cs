/*/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for AttendanceReader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *2.80        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// AttendanceReader Data Handler - Handles insert, update and selection of AttendanceReader object
    /// </summary>
    public class AttendanceReaderDataHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added
        private const string SELECT_QUERY = @"SELECT * FROM AttendanceReader as attendancereader ";

        /// <summary>
        /// Dictionary for searching Parameters for the AttendanceReader object.
        /// </summary>
        private static readonly Dictionary<AttendanceReaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AttendanceReaderDTO.SearchByParameters, string>
        {
            { AttendanceReaderDTO.SearchByParameters.ID,"attendancereader.ID"},
            { AttendanceReaderDTO.SearchByParameters.NAME,"attendancereader.Name"},
            { AttendanceReaderDTO.SearchByParameters.TYPE,"attendancereader.Type"},
            { AttendanceReaderDTO.SearchByParameters.IP_ADDRESS,"attendancereader.IPAddress"},
            { AttendanceReaderDTO.SearchByParameters.MACHINE_NUMBER,"attendancereader.MachineNumber"},
            { AttendanceReaderDTO.SearchByParameters.ACTIVE_FLAG,"attendancereader.ActiveFlag"},
            { AttendanceReaderDTO.SearchByParameters.SITE_ID,"attendancereader.site_id"},
            { AttendanceReaderDTO.SearchByParameters.MASTER_ENTITY_ID,"attendancereader.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for AttendanceReaderDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public AttendanceReaderDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AttendanceReader Record.
        /// </summary>
        /// <param name="attendanceReaderDTO">AttendanceReaderDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(AttendanceReaderDTO attendanceReaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceReaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ID", attendanceReaderDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", attendanceReaderDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", attendanceReaderDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IPAddress", attendanceReaderDTO.IPAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PortNumber", attendanceReaderDTO.PortNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SerialNumber", attendanceReaderDTO.SerialNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineNumber", attendanceReaderDTO.MachineNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", (attendanceReaderDTO.ActiveFlag == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastSynchTime", attendanceReaderDTO.LastSynchTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", attendanceReaderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to AttendanceReaderDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of AttendanceReaderDTO</returns>
        private AttendanceReaderDTO GetAttendanceReaderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AttendanceReaderDTO attendanceReaderDTO = new AttendanceReaderDTO(dataRow["ID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ID"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                                dataRow["IPAddress"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["IPAddress"]),
                                                dataRow["PortNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PortNumber"]),
                                                dataRow["SerialNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SerialNumber"]),
                                                dataRow["MachineNumber"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MachineNumber"]),
                                                dataRow["ActiveFlag"] == DBNull.Value ? true : (dataRow["ActiveFlag"].ToString() == "Y" ? true : false),                                                                                                
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["LastSynchTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastSynchTime"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]));
            return attendanceReaderDTO;
        }

        /// <summary>
        /// Gets the AttendanceReader data of passed AttendanceReader ID
        /// </summary>
        /// <param name="attendanceReaderId">attendanceReaderId is passed as parameter</param>
        /// <returns>Returns AttendanceReaderDTO</returns>
        public AttendanceReaderDTO GetAttendanceReaderDTO(int attendanceReaderId)
        {
            log.LogMethodEntry(attendanceReaderId);
            AttendanceReaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE attendancereader.ID = @ID";
            SqlParameter parameter = new SqlParameter("@ID", attendanceReaderId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAttendanceReaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Deletes the Leave Template record
        /// </summary>
        /// <param name="promotionDTO"></param>
        internal void Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM AttendanceReader
                             WHERE AttendanceReader.ID = @attendanceReaderId";
            SqlParameter parameter = new SqlParameter("@attendanceReaderId", id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="attendanceReaderDTO">AttendanceReaderDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshAttendanceReaderDTO(AttendanceReaderDTO attendanceReaderDTO, DataTable dt)
        {
            log.LogMethodEntry(attendanceReaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                attendanceReaderDTO.Id = Convert.ToInt32(dt.Rows[0]["ID"]);
                attendanceReaderDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                attendanceReaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                attendanceReaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                attendanceReaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                attendanceReaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                attendanceReaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the AttendanceReader Table. 
        /// </summary>
        /// <param name="attendanceReaderDTO">AttendanceReaderDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated AttendanceReaderDTO</returns>
        public AttendanceReaderDTO Insert(AttendanceReaderDTO attendanceReaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceReaderDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[AttendanceReader]
                            (
                            Name,
                            Type,
                            IPAddress,
                            PortNumber,
                            SerialNumber,
                            MachineNumber,
                            ActiveFlag,
                            site_id,
                            Guid,
                            LastSynchTime,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate
                            )
                            VALUES
                            (
                            @Name,
                            @Type,
                            @IPAddress,
                            @PortNumber,
                            @SerialNumber,
                            @MachineNumber,
                            @ActiveFlag,
                            @site_id,
                            NEWID(),
                            @LastSynchTime,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )
                            SELECT * FROM AttendanceReader WHERE ID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(attendanceReaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttendanceReaderDTO(attendanceReaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AttendanceReaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attendanceReaderDTO);
            return attendanceReaderDTO;
        }

        /// <summary>
        /// Update the record in the AttendanceReader Table. 
        /// </summary>
        /// <param name="attendanceReaderDTO">AttendanceReaderDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated AttendanceReaderDTO</returns>
        public AttendanceReaderDTO Update(AttendanceReaderDTO attendanceReaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceReaderDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[AttendanceReader]
                             SET
                             Name = @Name,
                             Type = @Type,
                             IPAddress = @IPAddress,
                             PortNumber = @PortNumber,
                             SerialNumber = @SerialNumber,
                             MachineNumber = @MachineNumber,
                             ActiveFlag = @ActiveFlag,
                             
                             LastSynchTime = @LastSynchTime,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE()
                             WHERE ID = @ID
                            SELECT * FROM AttendanceReader WHERE ID = @ID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(attendanceReaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttendanceReaderDTO(attendanceReaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AttendanceReaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attendanceReaderDTO);
            return attendanceReaderDTO;
        }

        /// <summary>
        /// Gets the AttendanceReaderDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AttendanceReaderDTO matching the search criteria</returns>
        public List<AttendanceReaderDTO> GetAttendanceReaderLists(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters);
            List<AttendanceReaderDTO> attendanceReaderDTOList = new List<AttendanceReaderDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters, sqlTransaction);
            if (currentPage > -1 && pageSize > 0)
            {
                selectQuery += " ORDER BY attendancereader.ID OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                attendanceReaderDTOList = new List<AttendanceReaderDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AttendanceReaderDTO attendanceReaderDTO = GetAttendanceReaderDTO(dataRow);
                    attendanceReaderDTOList.Add(attendanceReaderDTO);
                }
            }
            log.LogMethodExit(attendanceReaderDTOList);
            return attendanceReaderDTOList;
        }

        /// <summary>
        /// Returns the no of AttendanceReader matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetAttendanceReaderCount(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null) //added
        {
            log.LogMethodEntry(searchParameters);
            int attendanceReaderDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                attendanceReaderDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(attendanceReaderDTOCount);
            return attendanceReaderDTOCount;
        }

        /// <summary>
        /// Gets the AttendanceReaderDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AttendanceReaderDTO matching the search criteria</returns>
        public List<AttendanceReaderDTO> GetAttendanceReaderDTOList(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<AttendanceReaderDTO> attendanceReaderDTOList = new List<AttendanceReaderDTO>();
            parameters.Clear();
            string selectAttendanceReaderDTOQuery = SELECT_QUERY;
            selectAttendanceReaderDTOQuery = selectAttendanceReaderDTOQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dtAttendanceReaderDTO = dataAccessHandler.executeSelectQuery(selectAttendanceReaderDTOQuery, parameters.ToArray(), sqlTransaction);
            if (dtAttendanceReaderDTO.Rows.Count > 0)
            {
                foreach (DataRow attendanceReaderDTORow in dtAttendanceReaderDTO.Rows)
                {
                    AttendanceReaderDTO attendanceReaderDTO = GetAttendanceReaderDTO(attendanceReaderDTORow);
                    attendanceReaderDTOList.Add(attendanceReaderDTO);
                }

            }
            log.LogMethodExit(attendanceReaderDTOList);
            return attendanceReaderDTOList;
        }

        /// <summary>
        /// Returns the List of AttendanceReader based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AttendanceReaderDTO </returns>
        public string GetFilterQuery(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AttendanceReaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AttendanceReaderDTO.SearchByParameters.ID ||
                            searchParameter.Key == AttendanceReaderDTO.SearchByParameters.MACHINE_NUMBER ||
                            searchParameter.Key == AttendanceReaderDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AttendanceReaderDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == AttendanceReaderDTO.SearchByParameters.IP_ADDRESS ||
                                searchParameter.Key == AttendanceReaderDTO.SearchByParameters.NAME ||
                                searchParameter.Key == AttendanceReaderDTO.SearchByParameters.TYPE)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AttendanceReaderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
            }
            log.LogMethodExit();
            return query.ToString(); ;
        }
    }
}

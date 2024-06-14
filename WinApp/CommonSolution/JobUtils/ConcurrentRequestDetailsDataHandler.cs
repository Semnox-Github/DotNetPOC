/********************************************************************************************
 * Project Name - Concurrent Request Details Data Handler
 * Description  - Data handler of the Concurrent Request Details class
 * 
 **************
 **Version Logss
 **************
 *Version     Date           Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        24-Jul-2019    Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                          SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query                                                                                                                    
 *2.90          26-May-2020   Mushahid Faizan      Modified : 3 tier changes for Rest API., Added IsActive Column.
 *2.90          26-May-2020   Mushahid Faizan      Modified : 3 tier changes for Rest API., Added IsActive Column.
 *2.150.0.0     12-Sep-2022   Guru S A             Urbanpiper and delivery UI -GAP changes and fixes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    public class ConcurrentRequestDetailsDataHandler
    {
        private static readonly Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ConcurrentRequestDetails cd";
        private const int MAX_DATA_LENGTH = 2000;

        /// <summary>
        /// Dictionary for searching Parameters for the ConcurrentRequestDetails object.
        /// </summary>
        private static readonly Dictionary<ConcurrentRequestDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ConcurrentRequestDetailsDTO.SearchByParameters, string>
            {
                {ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_REQUEST_DETAILS_ID, "cd.ConcurrentRequestDetailsId"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_REQUEST_ID, "cd.ConcurrentRequestId"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_PROGRAM_ID, "cd.ConcurrentProgramId"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_PROGRAM_ID_LIST, "cd.ConcurrentProgramId"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.PARAFAIT_OBJECT_ID, "cd.ParafaitObjectId"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.PARAFAIT_OBJECT_GUID, "cd.ParafaitObjectGuid"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.STATUS,"cd.Status"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.SITE_ID, "cd.site_id"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.MASTER_ENTITIY_ID, "cd.MasterEntityId"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.IS_ACTIVE, "cd.IsActive"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.EXTERNAL_REFERENCE, "cd.externalReference"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.TIMESTAMP_GREATER_THAN_EQUAL_TO, "cd.Timestamp"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.TIMESTAMP_LESS_THAN_EQUAL_TO, "cd.Timestamp"},
                {ConcurrentRequestDetailsDTO.SearchByParameters.STATUS_NOT_IN,"cd.Status"},
            };

        /// <summary>
        /// Default constructor of ConcurrentRequestDetailsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentRequestDetailsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ConcurrentRequestDetails Record.
        /// </summary>
        /// <param name="concurrentRequestDetailsDTO">ConcurrentRequestDetailsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentRequestDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@concurrentRequestDetailsId", concurrentRequestDetailsDTO.ConcurrentRequestDetailsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@concurrentRequestId", concurrentRequestDetailsDTO.ConcurrentRequestId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@timestamp", concurrentRequestDetailsDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@concurrentProgramId", concurrentRequestDetailsDTO.ConcurrentProgramId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parafaitObject", concurrentRequestDetailsDTO.ParafaitObject));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parafaitObjectId", concurrentRequestDetailsDTO.ParafaitObjectId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parafaitObjectGuid", concurrentRequestDetailsDTO.ParafaitObjectGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isSuccessFul", concurrentRequestDetailsDTO.IsSuccessFul));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", concurrentRequestDetailsDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@externalReference", concurrentRequestDetailsDTO.ExternalReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@data", concurrentRequestDetailsDTO.Data.Length <= MAX_DATA_LENGTH ? 
                                                                    concurrentRequestDetailsDTO.Data : concurrentRequestDetailsDTO.Data.Substring(0, MAX_DATA_LENGTH)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", concurrentRequestDetailsDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", concurrentRequestDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", concurrentRequestDetailsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ConcurrentRequestDetails record to the database
        /// </summary>
        /// <param name="concurrentRequestDetailsDTO">ConcurrentRequestDetailsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ConcurrentRequestDetailsDTO InsertConcurrentRequestDetails(ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentRequestDetailsDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ConcurrentRequestDetails]  
                                        ( 
                                            [ConcurrentRequestId]
                                            ,[Timestamp]
                                            ,[ConcurrentProgramId]
                                            ,[ParafaitObject]
                                            ,[ParafaitObjectId]
                                            ,[ParafaitObjectGuid]
                                            ,[IsSuccessFul]
                                            ,[Status]
                                            ,[externalReference]
                                            ,[Data]
                                            ,[Remarks]
                                            ,[CreationDate]
                                            ,[CreatedBy]
                                            ,[LastUpdatedDate]
                                            ,[LastUpdatedBy]
                                            ,[site_id]
                                            ,[Guid] 
                                            ,[MasterEntityId]
                                            ,[IsActive]
                                        ) 
                                VALUES 
                                        (   @concurrentRequestId,
                                            @timestamp, 
                                            @concurrentProgramId, 
                                            @parafaitObject, 
                                            @parafaitObjectId, 
                                            @parafaitObjectGuid,
                                            @isSuccessFul, 
                                            @status, 
                                            @externalReference, 
                                            @data, 
                                            @remarks, 
                                            getDate(), 
                                            @createdBy,  
                                            getDate(),
                                            @lastUpdatedBy, 
                                            @siteId, 
                                            NEWID(), 
                                            @masterEntityId ,
                                            @isActive 
                                       )SELECT * FROM ConcurrentRequestDetails WHERE ConcurrentRequestDetailsId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(concurrentRequestDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentRequestDetailsDTO(concurrentRequestDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ConcurrentRequestDetails", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentRequestDetailsDTO);
            return concurrentRequestDetailsDTO;
        }

        /// <summary>
        /// Updates the ConcurrentRequestDetails record
        /// </summary>
        /// <param name="concurrentRequestDetailsDTO">ConcurrentRequestDetailsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ConcurrentRequestDetailsDTO UpdateConcurrentRequestDetails(ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentRequestDetailsDTO, loginId, siteId);
            string query = @"UPDATE ConcurrentRequestDetails 
                                SET  [ConcurrentRequestId] = @concurrentRequestId
                                   ,[Timestamp] = @timeStamp
                                   ,[ConcurrentProgramId] = @concurrentProgramId
                                   ,[ParafaitObject] = @parafaitObject
                                   ,[ParafaitObjectId] = @parafaitObjectId
                                   ,[ParafaitObjectGuid] = @parafaitObjectGuid
                                   ,[IsSuccessFul] = @isSuccessFul
                                   ,[Status] = @status
                                   ,[externalReference] = @externalReference
                                   ,[Data] = @data
                                   ,[Remarks] = @remarks  
                                   ,[LastUpdatedDate] = getDate()
                                   ,[LastUpdatedBy] = @lastUpdatedBy
                                   --,[site_id] = @siteId
                                   ,[MasterEntityId] = @masterEntityId
                                   ,[IsActive] = @isActive
                             WHERE ConcurrentRequestDetailsId = @ConcurrentRequestDetailsId 
                             SELECT * FROM ConcurrentRequestDetails WHERE ConcurrentRequestDetailsId = @ConcurrentRequestDetailsId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(concurrentRequestDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentRequestDetailsDTO(concurrentRequestDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating concurrentProgramsSchedule", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentRequestDetailsDTO);
            return concurrentRequestDetailsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="concurrentRequestDetailsDTO">concurrentRequestDetailsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshConcurrentRequestDetailsDTO(ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(concurrentRequestDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                concurrentRequestDetailsDTO.ConcurrentRequestDetailsId = Convert.ToInt32(dt.Rows[0]["ConcurrentRequestDetailsId"]);
                concurrentRequestDetailsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                concurrentRequestDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                concurrentRequestDetailsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                concurrentRequestDetailsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                concurrentRequestDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                concurrentRequestDetailsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ConcurrentRequestDetailsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ConcurrentRequestDetailsDTO</returns>
        private ConcurrentRequestDetailsDTO GetConcurrentRequestDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO = new ConcurrentRequestDetailsDTO(Convert.ToInt32(dataRow["concurrentRequestDetailsId"]),
                                            dataRow["concurrentRequestId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["concurrentRequestId"]),
                                            dataRow["timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["timestamp"]),
                                            dataRow["concurrentProgramId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["concurrentProgramId"]),
                                            dataRow["parafaitObject"] == DBNull.Value ? string.Empty : dataRow["parafaitObject"].ToString(),
                                            dataRow["parafaitObjectId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["parafaitObjectId"]),
                                            dataRow["parafaitObjectGuid"] == DBNull.Value ? string.Empty : dataRow["parafaitObjectGuid"].ToString(),
                                            dataRow["isSuccessFul"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["isSuccessFul"]),
                                            dataRow["status"] == DBNull.Value ? string.Empty : dataRow["status"].ToString(),
                                            dataRow["externalReference"] == DBNull.Value ? string.Empty : dataRow["externalReference"].ToString(),
                                            dataRow["data"] == DBNull.Value ? string.Empty : dataRow["data"].ToString(),
                                            dataRow["remarks"] == DBNull.Value ? string.Empty : dataRow["remarks"].ToString(),
                                            dataRow["creationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["creationDate"]),
                                            dataRow["createdBy"] == DBNull.Value ? string.Empty : dataRow["createdBy"].ToString(),
                                            dataRow["lastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["lastUpdatedDate"]),
                                            dataRow["lastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["lastUpdatedBy"].ToString(),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["guid"] == DBNull.Value ? string.Empty : dataRow["guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["masterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["masterEntityId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(concurrentRequestDetailsDTO);
            return concurrentRequestDetailsDTO;
        }

        /// <summary>
        /// Gets the ConcurrentRequestDetails data of passed ConcurrentRequestDetails Id
        /// </summary>
        /// <param name="ConcurrentRequestDetailsId">integer type parameter</param>
        /// <returns>Returns ConcurrentRequestDetailsDTO</returns>
        public ConcurrentRequestDetailsDTO GetConcurrentRequestDetailsDTO(int ConcurrentRequestDetailsId)
        {
            log.LogMethodEntry(ConcurrentRequestDetailsId);
            ConcurrentRequestDetailsDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE cd.ConcurrentRequestDetailsId = @ConcurrentRequestDetailsId";
            SqlParameter parameter = new SqlParameter("@ConcurrentRequestDetailsId", ConcurrentRequestDetailsId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetConcurrentRequestDetailsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the ConcurrentRequestDetailsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ConcurrentRequestDetailsDTO matching the search criteria</returns>
        public List<ConcurrentRequestDetailsDTO> GetConcurrentRequestDetailsDTOList(List<KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ConcurrentRequestDetailsDTO> concurrentRequestDetailsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_PROGRAM_ID
                            || searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_REQUEST_DETAILS_ID
                            || searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_REQUEST_ID
                            || searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.PARAFAIT_OBJECT_ID
                            || searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.MASTER_ENTITIY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.STATUS
                                  || searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.PARAFAIT_OBJECT_GUID
                                  || searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.EXTERNAL_REFERENCE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.IS_ACTIVE) 
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.TIMESTAMP_GREATER_THAN_EQUAL_TO)  
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.TIMESTAMP_LESS_THAN_EQUAL_TO)
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.STATUS_NOT_IN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_PROGRAM_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                concurrentRequestDetailsDTOList = new List<ConcurrentRequestDetailsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO = GetConcurrentRequestDetailsDTO(dataRow);
                    concurrentRequestDetailsDTOList.Add(concurrentRequestDetailsDTO);
                }
            }
            log.LogMethodExit(concurrentRequestDetailsDTOList);
            return concurrentRequestDetailsDTOList;
        }
    }
}

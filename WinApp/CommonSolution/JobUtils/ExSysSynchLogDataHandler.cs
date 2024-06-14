/********************************************************************************************
 * Project Name - logger
 * Description  - Data Handler -ExSysSynchLogDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      07-Jun-2019   Girish Kundar           Created 
 *2.80.0      02-Apr-2020   Akshay Gulaganji        Added search parameters - PARAFAIT_OBJECT, PARAFAIT_OBJECT_ID, HAVING_UN_SUCCESSFUL_COUNT_LESS_THAN and PARAFAIT_OBJECT_ID_LIST
 *2.130.7    14-APR-2022    Girish Kundar           Modified : Aloha BSP integration changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{

    /// <summary>
    /// ExSysSynchLogDataHandler Data Handler - Handles insert, update and select of  ExSysSynchLogData objects
    /// </summary>
    public class ExSysSynchLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ExSysSynchLog AS esl";
        /// <summary>
        /// Dictionary for searching Parameters for the ExSysSynchLog object.
        /// </summary>
        private static readonly Dictionary<ExSysSynchLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ExSysSynchLogDTO.SearchByParameters, string>
        {
            { ExSysSynchLogDTO.SearchByParameters.LOG_ID,"esl.LogId"},
            { ExSysSynchLogDTO.SearchByParameters.LOG_ID_LIST,"esl.LogId"},
            { ExSysSynchLogDTO.SearchByParameters.EX_SYSTEM_NAME,"esl.ExSysName"},
            { ExSysSynchLogDTO.SearchByParameters.SITE_ID,"esl.site_id"},
            { ExSysSynchLogDTO.SearchByParameters.IS_SUCCESSFUL,"esl.IsSuccessFul"},
            { ExSysSynchLogDTO.SearchByParameters.MASTER_ENTITY_ID,"esl.MasterEntityId"},
            { ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT,"esl.ParafaitObject"},
            { ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_ID,"esl.ParafaitObjectId"},
            { ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_GUID,"esl.ParafaitObjectGuid"},
            { ExSysSynchLogDTO.SearchByParameters.STATUS,"esl.Status"},
            { ExSysSynchLogDTO.SearchByParameters.STATUS_LIST,"esl.Status"},
            { ExSysSynchLogDTO.SearchByParameters.DATA,"esl.Data"},
            { ExSysSynchLogDTO.SearchByParameters.TIMESTAMP_FROM,"esl.Timestamp"},
            { ExSysSynchLogDTO.SearchByParameters.TIMESTAMP_TO,"esl.Timestamp"},
            { ExSysSynchLogDTO.SearchByParameters.REMARKS,"esl.Remarks"},
            { ExSysSynchLogDTO.SearchByParameters.REQUEST_ID,"esl.ConcurrentRequestId"},
            { ExSysSynchLogDTO.SearchByParameters.REQUEST_ID_LIST,"esl.ConcurrentRequestId"},
            { ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_ID_LIST,"esl.ParafaitObjectId"},
            { ExSysSynchLogDTO.SearchByParameters.HAVING_UN_SUCCESSFUL_COUNT_LESS_THAN,"esl.ParafaitObjectId"},
            { ExSysSynchLogDTO.SearchByParameters.UNIQUE_OBJECT_STATUS_LIST,"esl.Status"},
            { ExSysSynchLogDTO.SearchByParameters.BSP_ID,"esl.Data"},
            { ExSysSynchLogDTO.SearchByParameters.NOT_IN_REPROCESSING_STATE,"esl.Remarks"},
            { ExSysSynchLogDTO.SearchByParameters.STATUS_NOT_IN_SUCCESS,"esl.Status"},
            { ExSysSynchLogDTO.SearchByParameters.TRX_FROM_DATE,"th.TrxDate"},
            { ExSysSynchLogDTO.SearchByParameters.TRX_TO_DATE,"th.TrxDate"}
        };

        /// <summary>
        /// Parameterized Constructor for ExSysSynchLogDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public ExSysSynchLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ExSysSynchLog Record.
        /// </summary>
        /// <param name="exSysSynchLogDTO">exSysSynchLogDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ExSysSynchLogDTO exSysSynchLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(exSysSynchLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LogId", exSysSynchLogDTO.LogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Data", exSysSynchLogDTO.Data));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExSysName", exSysSynchLogDTO.ExSysName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsSuccessFul", exSysSynchLogDTO.IsSuccessFul));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitObject", exSysSynchLogDTO.ParafaitObject));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitObjectGuid", exSysSynchLogDTO.ParafaitObjectGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitObjectId", exSysSynchLogDTO.ParafaitObjectId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", exSysSynchLogDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", exSysSynchLogDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", exSysSynchLogDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", exSysSynchLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConcurrentRequestId", exSysSynchLogDTO.ConcurrentRequestId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to ExSysSynchLogDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object </param>
        /// <returns>returns the object of ExSysSynchLogDTO</returns>
        private ExSysSynchLogDTO GetExSysSynchLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ExSysSynchLogDTO exSysSynchLogDTO = new ExSysSynchLogDTO(dataRow["LogId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LogId"]),
                                          dataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Timestamp"]),
                                          dataRow["ExSysName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExSysName"]),
                                          dataRow["ParafaitObject"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParafaitObject"]),
                                          dataRow["ParafaitObjectId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParafaitObjectId"]),
                                          dataRow["ParafaitObjectGuid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParafaitObjectGuid"]),
                                          dataRow["IsSuccessFul"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsSuccessFul"]),
                                          dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                          dataRow["Data"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Data"]),
                                          dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["ConcurrentRequestId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ConcurrentRequestId"])


                                          );
            log.LogMethodExit(exSysSynchLogDTO);
            return exSysSynchLogDTO;
        }

        internal void UpdateExsysSynchLogRequestIDAndStatus(List<int> siteBasedLogIDList,string status, int lastRequestId)
        {
            log.LogMethodEntry(siteBasedLogIDList, lastRequestId);
            List<ExSysSynchLogDTO> exsysLogDTOList = new List<ExSysSynchLogDTO>();
            string query = @"UPDATE  e
                                set e.Status = @status, e.ConcurrentRequestId = @requestId, 
                                     Remarks = 'ReProcessing',LastUpdatedBy = 'semnox',LastUpdateDate= GETDATE()
                                     from ExSysSynchLog e , @siteBasedLogIDList List
                                   WHERE LogId = List.Id  
                                    --and e.ConcurrentRequestId != @requestId
                                     ";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@requestId", lastRequestId));
            sqlParameters.Add(new SqlParameter("@status", status));
            int noOfRowsUpdated = dataAccessHandler.BatchUpdate(query, "@siteBasedLogIDList", siteBasedLogIDList, sqlParameters.ToArray(), sqlTransaction);
            log.Debug("Number Of Rows Updated During UpdateExsysSynchLogRequestIDAndStatus " + noOfRowsUpdated +
                        " for lastRequestId " + lastRequestId);
            log.LogMethodExit(exsysLogDTOList);
        }

        public int GetExsysSynchLogCount(List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int result = 0;
            if (searchParameters != null)
            {
                List<ExSysSynchLogDTO> exSysSynchLogDTOList = GetExSysSynchLogDTOList(searchParameters);
                if (exSysSynchLogDTOList != null && exSysSynchLogDTOList.Any())
                {
                    result = exSysSynchLogDTOList.Count;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ExSysSynchLogDTO data of passed Id 
        /// </summary>
        /// <param name="id">id of ExSysSynchLogDTO</param>
        /// <returns>Returns ExSysSynchLogDTO</returns>
        public ExSysSynchLogDTO GetExSysSynchLogDTO(int id)
        {
            log.LogMethodEntry(id);
            ExSysSynchLogDTO result = null;
            string query = SELECT_QUERY + @" WHERE esl.LogId = @LogId";
            SqlParameter parameter = new SqlParameter("@LogId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetExSysSynchLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the ExSysSynchLogDTO Table.
        /// </summary>
        /// <param name="exSysSynchLogDTO">exSysSynchLogDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns> returns the ExSysSynchLogDTO</returns>
        public ExSysSynchLogDTO Insert(ExSysSynchLogDTO exSysSynchLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(exSysSynchLogDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ExSysSynchLog]
                           ([Timestamp],
                            [ExSysName],
                            [ParafaitObject],
                            [ParafaitObjectId],
                            [ParafaitObjectGuid],
                            [IsSuccessFul],
                            [Status],
                            [Data],
                            [Remarks],
                            [site_id],
                            [Guid],
                            [MasterEntityId],
                            [CreatedBy],
                            [CreationDate],
                            [LastUpdatedBy],
                            [LastUpdateDate],ConcurrentRequestId)
                     VALUES
                           (@Timestamp,
                            @ExSysName,
                            @ParafaitObject,
                            @ParafaitObjectId,
                            @ParafaitObjectGuid,
                            @IsSuccessFul,
                            @Status,
                            @Data,
                            @Remarks,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),@ConcurrentRequestId)
                              SELECT * FROM ExSysSynchLog WHERE LogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(exSysSynchLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshExSysSynchLogDTO(exSysSynchLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ExSysSynchLogDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(exSysSynchLogDTO);
            return exSysSynchLogDTO;
        }

        /// <summary>
        ///  Updates the record to the ExSysSynchLogDTO Table.
        /// </summary>
        /// <param name="exSysSynchLogDTO">exSysSynchLogDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns>returns the  ExSysSynchLogDTO</returns>
        public ExSysSynchLogDTO Update(ExSysSynchLogDTO exSysSynchLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(exSysSynchLogDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ExSysSynchLog]
                           SET
                            [Timestamp]         = @Timestamp,
                            [ExSysName]         = @ExSysName,
                            [ParafaitObject]    = @ParafaitObject,
                            [ParafaitObjectId]  = @ParafaitObjectId,
                            [ParafaitObjectGuid]= @ParafaitObjectGuid,
                            [IsSuccessFul]      = @IsSuccessFul,
                            [Status]            = @Status,
                            [Data]              = @Data,
                            [Remarks]           = @Remarks,
                            [MasterEntityId]    = @MasterEntityId,
                            [LastUpdatedBy]     = @LastUpdatedBy,
                            [LastUpdateDate]    = GETDATE(),
                            [ConcurrentRequestId]    = @ConcurrentRequestId
                            WHERE LogId = @LogId
                              SELECT * FROM ExSysSynchLog WHERE LogId = @LogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(exSysSynchLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshExSysSynchLogDTO(exSysSynchLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ExSysSynchLogDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(exSysSynchLogDTO);
            return exSysSynchLogDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="exSysSynchLogDTO">ExSysSynchLogDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshExSysSynchLogDTO(ExSysSynchLogDTO exSysSynchLogDTO, DataTable dt)
        {
            log.LogMethodEntry(exSysSynchLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                exSysSynchLogDTO.LogId = Convert.ToInt32(dt.Rows[0]["LogId"]);
                exSysSynchLogDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                exSysSynchLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                exSysSynchLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                exSysSynchLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                exSysSynchLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                exSysSynchLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal List<ExSysSynchLogDTO> GetExSysSynchLogDTOList(List<int> exsysSynchLogIdList)
        {
            log.LogMethodEntry(exsysSynchLogIdList);
            List<ExSysSynchLogDTO> exSysSynchLogDTOList = new List<ExSysSynchLogDTO>();
            string query = @"SELECT *
                            FROM ExSysSynchLog, @exsysSynchLogIdList List
                            WHERE LogId = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(query, "@exsysSynchLogIdList", exsysSynchLogIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                exSysSynchLogDTOList = table.Rows.Cast<DataRow>().Select(x => GetExSysSynchLogDTO(x)).ToList();
            }
            log.LogMethodExit(exSysSynchLogDTOList);
            return exSysSynchLogDTOList;
        }

        internal List<ExSysSynchLogDTO> GetFailedExSysSynchLogDTOList(List<int> requestIdList)
        {
            log.LogMethodEntry(requestIdList);
            List<ExSysSynchLogDTO> exSysSynchLogDTOList = new List<ExSysSynchLogDTO>();
            string query = @"SELECT *
                            FROM ExSysSynchLog, @requestIdList List
                            WHERE ConcurrentRequestId = List.Id 
                            AND ExSysName = 'Aloha'
                            AND ParafaitObject =  'TrxId'
                            AND IsSuccessFul = 0
                            AND Status = 'Error'";
            DataTable table = dataAccessHandler.BatchSelect(query, "@requestIdList", requestIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                exSysSynchLogDTOList = table.Rows.Cast<DataRow>().Select(x => GetExSysSynchLogDTO(x)).ToList();
            }
            log.LogMethodExit(exSysSynchLogDTOList);
            return exSysSynchLogDTOList;
        }

        /// <summary>
        /// Returns the List of ExSysSynchLogDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>returns the List of ExSysSynchLogDTO</returns>
        public List<ExSysSynchLogDTO> GetExSysSynchLogDTOList(List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ExSysSynchLogDTO> exSysSynchLogDTOList = new List<ExSysSynchLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                if (searchParameters.Exists(x => x.Key == ExSysSynchLogDTO.SearchByParameters.UNIQUE_OBJECT_STATUS_LIST))
                {
                    selectQuery += @" inner join (select *, RANK() over (Partition BY parafaitObjectGuid order by logId desc) as rank
                                        from ExSysSynchLog) el on esl.LogId = el.LogId";
                }
                if(searchParameters.Exists(x => x.Key == ExSysSynchLogDTO.SearchByParameters.TRX_TO_DATE || x.Key == ExSysSynchLogDTO.SearchByParameters.TRX_FROM_DATE))
                {
                    selectQuery += @" inner join trx_header th on th.TrxId = esl.ParafaitObjectId";
                }
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.LOG_ID ||
                            searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.REQUEST_ID ||
                            searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.EX_SYSTEM_NAME ||
                                 searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT ||
                                 searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_GUID ||
                                 searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.STATUS ||
                                 searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.REMARKS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(ExSysSynchLogDTO.SearchByParameters.TIMESTAMP_FROM) ||
                                searchParameter.Key.Equals(ExSysSynchLogDTO.SearchByParameters.TRX_FROM_DATE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(ExSysSynchLogDTO.SearchByParameters.TIMESTAMP_TO) ||
                                  searchParameter.Key.Equals(ExSysSynchLogDTO.SearchByParameters.TRX_TO_DATE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.IS_SUCCESSFUL)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_ID_LIST ||
                                 searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.LOG_ID_LIST ||
                                 searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.REQUEST_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.STATUS_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.UNIQUE_OBJECT_STATUS_LIST)
                        {
                            query.Append(joiner + "rank = 1 and el.Status IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.NOT_IN_REPROCESSING_STATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " NOT IN('ReProcessing')  " +
                                                        " and not exists (select 1 from ExSysSynchLog where " +
                                                        " ParafaitObjectId = esl.ParafaitObjectId and Status = 'Success') ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.STATUS_NOT_IN_SUCCESS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " NOT IN('Success') ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.BSP_ID)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ExSysSynchLogDTO.SearchByParameters.HAVING_UN_SUCCESSFUL_COUNT_LESS_THAN) // Number of Count
                        {

                            query.Append(joiner + "( " + DBSearchParameters[searchParameter.Key] + @" in (SELECT v.ParafaitObjectId
											                                                                FROM (SELECT ex1.ParafaitObjectId, count(1) cnt
												                                                                FROM ExSysSynchLog ex1
												                                                                WHERE ex1.IsSuccessFul = 0
												                                                                        AND NOT EXISTS ( 
														                                                                SELECT 1 FROM ExSysSynchLog ex2
															                                                                WHERE ex2.ParafaitObjectGuid = ex1.ParafaitObjectGuid
															                                                                AND ex2.IsSuccessFul = 1)
															                                                                GROUP BY ParafaitObjectId
															                                                                HAVING count(1) < " + dataAccessHandler.GetParameterName(searchParameter.Key) + ") v))");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ExSysSynchLogDTO exSysSynchLogDTO = GetExSysSynchLogDTO(dataRow);
                    exSysSynchLogDTOList.Add(exSysSynchLogDTO);
                }
            }
            log.LogMethodExit(exSysSynchLogDTOList);
            return exSysSynchLogDTOList;
        }

        internal List<ExSysSynchLogDTO> GetExSysSynchLogDTOList(string parafaitObjectName, string status, string remarks, List<int> parafaitObjectIdList)
        {
            log.LogMethodEntry(parafaitObjectName, parafaitObjectIdList);
            List<ExSysSynchLogDTO> exSysSynchLogDTOList = new List<ExSysSynchLogDTO>();
            string query = @"SELECT *
                            FROM ExSysSynchLog, @ParafaitObjectIdList List
                            WHERE ParafaitObjectId = List.Id 
                              and ParafaitObject = @ParafaitObject 
                              and status = @Status 
                              and remarks = @Remarks ";
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("@ParafaitObject", parafaitObjectName));
            sqlParameterList.Add(new SqlParameter("@Status", status));
            sqlParameterList.Add(new SqlParameter("@Remarks", remarks));
            DataTable table = dataAccessHandler.BatchSelect(query, "@ParafaitObjectIdList", parafaitObjectIdList, sqlParameterList.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                exSysSynchLogDTOList = table.Rows.Cast<DataRow>().Select(x => GetExSysSynchLogDTO(x)).ToList();
            }
            log.LogMethodExit(exSysSynchLogDTOList);
            return exSysSynchLogDTOList;
        }
    }
}

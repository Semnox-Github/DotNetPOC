/********************************************************************************************
 * Project Name - GenericUtilities                                                                       
 * Description  - ParafaitMessageQueueDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 *2.140.0     21-Jun-2021   Fiona Lishal      Modified : Issue fix in UpdateParafaitMessageQueueDTO
 *2.140.0     25-Nov-2021   Fiona Lishal      Modified : Added Searcch Parameter MESSAGE
 *2.140.0     07-Feb-2022   Fiona Lishal      Modified : Added Searcch Parameters FROM_DATE, ATTEMPTS_LESS_THAN, columns  Remarks,
                                                       Attempts,
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// ParafaitMessageQueueDataHandler
    /// </summary>
    public class ParafaitMessageQueueDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ParafaitMessageQueue AS pmq ";
        private List<SqlParameter> parameters;
        private static readonly Dictionary<ParafaitMessageQueueDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ParafaitMessageQueueDTO.SearchByParameters, string>
            {
                {ParafaitMessageQueueDTO.SearchByParameters.MESSAGE_QUEUE_ID, "pmq.MessageQueueId"},
                {ParafaitMessageQueueDTO.SearchByParameters.ENTITY_NAME, "pmq.EntityName"},
                {ParafaitMessageQueueDTO.SearchByParameters.STATUS, "pmq.Status"},
                {ParafaitMessageQueueDTO.SearchByParameters.STATUS_LIST, "pmq.Status"},
                {ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID, "pmq.EntityGuid"},
                {ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID_LIST, "pmq.EntityGuid"},
                {ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE, "pmq.IsActive"},
                {ParafaitMessageQueueDTO.SearchByParameters.MASTER_ENTITY_ID, "pmq.MasterEntityId"},
                {ParafaitMessageQueueDTO.SearchByParameters.SITE_ID, "pmq.site_id"},
                {ParafaitMessageQueueDTO.SearchByParameters.ACTION_TYPE, "pmq.ActionType"},
                {ParafaitMessageQueueDTO.SearchByParameters.MESSAGE, "pmq.Message"},
                {ParafaitMessageQueueDTO.SearchByParameters.FROM_DATE, "pmq.LastUpdatedDate"},
                {ParafaitMessageQueueDTO.SearchByParameters.ATTEMPTS_LESS_THAN, "pmq.Attempts"}

            };
        /// <summary>
        /// Default constructor of ParafaitMessageQueueDataHandler class
        /// </summary>
        public ParafaitMessageQueueDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ParafaitMessageQueueDTO Record.
        /// </summary>
        /// <param name="parafaitMessageQueueDTO">ParafaitMessageQueueDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(ParafaitMessageQueueDTO parafaitMessageQueueDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitMessageQueueDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageQueueId", parafaitMessageQueueDTO.MessageQueueId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EntityGuid", parafaitMessageQueueDTO.EntityGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EntityName", parafaitMessageQueueDTO.EntityName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Message", parafaitMessageQueueDTO.Message));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", parafaitMessageQueueDTO.Status.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", parafaitMessageQueueDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attempts", parafaitMessageQueueDTO.Attempts));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", parafaitMessageQueueDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", parafaitMessageQueueDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActionType", parafaitMessageQueueDTO.ActionType));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parafaitMessageQueueDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ParafaitMessageQueueDTO Insert(ParafaitMessageQueueDTO parafaitMessageQueueDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitMessageQueueDTO, loginId, siteId);
            string insertQuery = @"IF NOT EXISTS(SELECT 1 
                                                 FROM ParafaitMessageQueue 
                                                  WHERE EntityName = 'Transaction' 
                                                  AND Message = @Message 
                                                  AND Remarks = 'Order Creation'
                                                  AND (site_id = @siteId OR @siteId IS NULL))
                                    BEGIN
                                    insert into ParafaitMessageQueue 
                                                        (                                                         
                                                       EntityGuid ,
                                                       EntityName,
                                                       Message,
                                                       Status,
                                                       Remarks,
                                                       Attempts,
                                                       IsActive ,
                                                       CreatedBy ,
                                                       CreationDate ,
                                                       LastUpdatedBy ,
                                                       LastUpdatedDate ,
                                                       Guid ,
                                                       site_id   ,
                                                       MasterEntityId,
                                                       ActionType
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @EntityGuid ,
                                                       @EntityName,
                                                       @Message,
                                                       @Status,
                                                       @Remarks,
                                                       @Attempts,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId,
                                                       @ActionType
                                          )SELECT  * from ParafaitMessageQueue where MessageQueueId = scope_identity() 
                             END";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(parafaitMessageQueueDTO, loginId, siteId).ToArray(), sqlTransaction);
                if (dt != null && dt.Rows.Count > 0)
                {
                    RefreshParafaitMessageQueueDTO(parafaitMessageQueueDTO, dt);
                }
                else
                {
                    string errorMesage = "Duplicate Order Creation request message for " + parafaitMessageQueueDTO.Message;
                    log.LogMethodExit("Throwing Exception - " + errorMesage);
                    throw new ValidationException(errorMesage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ParafaitMessageQueueDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(parafaitMessageQueueDTO);
            return parafaitMessageQueueDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parafaitMessageQueueDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ParafaitMessageQueueDTO Update(ParafaitMessageQueueDTO parafaitMessageQueueDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitMessageQueueDTO, loginId, siteId);
            string updateQuery = @"update ParafaitMessageQueue 
                                         set 
                                             EntityGuid = @EntityGuid,
                                             EntityName= @EntityName,
                                             Message = @Message,
                                             Status = @Status,
                                             Remarks = @Remarks,
                                             Attempts=@Attempts,
                                             IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId,
                                            ActionType= @ActionType
                                               where   MessageQueueId =  @MessageQueueId  
                                         SELECT  * from ParafaitMessageQueue where MessageQueueId =  @MessageQueueId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(parafaitMessageQueueDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshParafaitMessageQueueDTO(parafaitMessageQueueDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ParafaitMessageQueueDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(parafaitMessageQueueDTO);
            return parafaitMessageQueueDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParafaitMessageQueueDTO"></param>
        /// <returns></returns>
        public ParafaitMessageQueueDTO UpdateParafaitMessageQueueDTO(ParafaitMessageQueueDTO ParafaitMessageQueueDTO)
        {
            log.LogMethodEntry(ParafaitMessageQueueDTO);
            string updateQuery = @"update ParafaitMessageQueue 
                                         set 
                                             EntityGuid = @EntityGuid,
                                             EntityName= @EntityName,
                                             Message = @Message,
                                             Status = @Status,
                                             Remarks = @Remarks,
                                             Attempts = @Attempts,
                                             IsActive = @IsActive,
                                             ActionType = @ActionType
                                             where MessageQueueId =  @MessageQueueId  
                                         SELECT  * from ParafaitMessageQueue where MessageQueueId =  @MessageQueueId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(ParafaitMessageQueueDTO, string.Empty, -1).ToArray(), sqlTransaction);
                RefreshParafaitMessageQueueDTO(ParafaitMessageQueueDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ParafaitMessageQueueDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(ParafaitMessageQueueDTO);
            return ParafaitMessageQueueDTO;
        }
        private void RefreshParafaitMessageQueueDTO(ParafaitMessageQueueDTO parafaitMessageQueueDTO, DataTable dt)
        {
            log.LogMethodEntry(parafaitMessageQueueDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                parafaitMessageQueueDTO.MessageQueueId = Convert.ToInt32(dt.Rows[0]["MessageQueueId"]);
                parafaitMessageQueueDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                parafaitMessageQueueDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                parafaitMessageQueueDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                parafaitMessageQueueDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                parafaitMessageQueueDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                parafaitMessageQueueDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private ParafaitMessageQueueDTO GetParafaitMessageQueueDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ParafaitMessageQueueDTO parafaitMessageQueueDTO = new ParafaitMessageQueueDTO
                                    (Convert.ToInt32(dataRow["MessageQueueId"]),
                                    dataRow["EntityGuid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EntityGuid"]),
                                    dataRow["EntityName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EntityName"]),
                                    dataRow["Message"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Message"]),
                                    GetMessageQueueStatus(Convert.ToString(dataRow["Status"])),
                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                    dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                    dataRow["ActionType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ActionType"]),
                                    dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                    dataRow["Attempts"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Attempts"])
                                                    );
            log.LogMethodExit(parafaitMessageQueueDTO);
            return parafaitMessageQueueDTO;
        }

        private MessageQueueStatus GetMessageQueueStatus(string status)
        {
            log.LogMethodEntry(status);
            MessageQueueStatus orderStatus;
            try
            {
                orderStatus = (MessageQueueStatus)Enum.Parse(typeof(MessageQueueStatus), status, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while parsing the OrderMessageStatus type", ex);
                throw ex;
            }
            log.LogMethodExit(orderStatus);
            return orderStatus;
        }
        /// <summary>
        /// Gets the ParafaitMessageQueueDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ParafaitMessageQueueDTO</returns>
        internal ParafaitMessageQueueDTO GetParafaitMessageQueueDTO(int id)
        {
            log.LogMethodEntry(id);
            ParafaitMessageQueueDTO parafaitMessageQueueDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where pmq.MessageQueueId = @MessageQueueId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@MessageQueueId", id);
            DataTable messageQueueTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (messageQueueTable.Rows.Count > 0)
            {
                DataRow gameMachineLevelRow = messageQueueTable.Rows[0];
                parafaitMessageQueueDTO = GetParafaitMessageQueueDTO(gameMachineLevelRow);
            }
            log.LogMethodExit(parafaitMessageQueueDTO);
            return parafaitMessageQueueDTO;

        }

        /// <summary>
        /// GetDeliveryChannels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ParafaitMessageQueueDTO> GetParafaitMessageQueues(List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ParafaitMessageQueueDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                            searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.MESSAGE_QUEUE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_NAME) ||
                           searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID) ||
                          searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ACTION_TYPE) ||
                           searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.STATUS)||
                           searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.MESSAGE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID_LIST) ||
                                    searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.STATUS_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.FROM_DATE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ATTEMPTS_LESS_THAN))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        counter++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                parafaitMessageQueueDTOList = new List<ParafaitMessageQueueDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    ParafaitMessageQueueDTO ParafaitMessageQueueDTO = GetParafaitMessageQueueDTO(dataRow);
                    parafaitMessageQueueDTOList.Add(ParafaitMessageQueueDTO);
                }
            }
            log.LogMethodExit(parafaitMessageQueueDTOList);
            return parafaitMessageQueueDTOList;
        }
        internal List<ParafaitMessageQueueDTO> GetParafaitMessageQueueDTOList(List<string> parafaitMessageQueueEntityGuidList, List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(parafaitMessageQueueEntityGuidList);
            List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = new List<ParafaitMessageQueueDTO>();
            string query = SELECT_QUERY + @" , @parafaitMessageQueueEntityGuidList List
                            where pmq.EntityGuid = List.Value ";
            if (searchParameters != null && searchParameters.Any())
            {
                query = query + GetWhereClause(searchParameters);
            }
            //query = query + @" INNER JOIN @parafaitMessageQueueEntityGuidList List
            //on pmq.EntityGuid = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(query, "@parafaitMessageQueueEntityGuidList", parafaitMessageQueueEntityGuidList, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                parafaitMessageQueueDTOList = table.Rows.Cast<DataRow>().Select(x => GetParafaitMessageQueueDTO(x)).ToList();
            }
            log.LogMethodExit(parafaitMessageQueueDTOList);
            return parafaitMessageQueueDTOList;
        }
        private string GetWhereClause(List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            parameters = new List<SqlParameter>();
            string whereClause = string.Empty;
            if (searchParameters == null || searchParameters.Count == 0)
            {
                log.LogMethodExit(string.Empty, "search parameters is empty");
                return whereClause;
            }
            string joiner = string.Empty;
            StringBuilder query = new StringBuilder("");
            foreach (KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                joiner = " and ";
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    if (searchParameter.Key == ParafaitMessageQueueDTO.SearchByParameters.SITE_ID)
                    {
                        query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }
                    else if (searchParameter.Key == ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE)
                    {
                        query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                    }
                    else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                        searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.MESSAGE_QUEUE_ID))
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }

                    else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_NAME) ||
                       searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID) ||
                       searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.STATUS) ||
                       searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.MESSAGE))
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                    }
                    else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID_LIST) ||
                                searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.STATUS_LIST))
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                        parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                    }
                    else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.FROM_DATE))
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    else if (searchParameter.Key.Equals(ParafaitMessageQueueDTO.SearchByParameters.ATTEMPTS_LESS_THAN))
                    {
                        query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                    }
                    else
                    {
                        query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            whereClause = query.ToString();
            log.LogMethodExit(whereClause);
            return whereClause;
        }
    }
}

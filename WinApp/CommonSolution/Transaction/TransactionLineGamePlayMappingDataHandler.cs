/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - TransactionLineGamePlayMapping
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.150.0     18-Jan-2022      Prajwal S            Created
*            01-June-2023     Prashanth V          Modified insert method to remove insertion of value into SyncStatus column.
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    class TransactionLineGamePlayMappingDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TransactionLineGamePlayMapping AS tlgm ";


        private static readonly Dictionary<TransactionLineGamePlayMappingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionLineGamePlayMappingDTO.SearchByParameters, string>
        {
              {TransactionLineGamePlayMappingDTO.SearchByParameters.TRANSACTION_ID , "tlgm.TrxId"},
              {TransactionLineGamePlayMappingDTO.SearchByParameters.TRANSACTION_LINE_GAME_PLAY_MAPPING_ID , "tlgm.TransactionLineGamePlayMappingId"},
              {TransactionLineGamePlayMappingDTO.SearchByParameters.TRANSACTION_LINE_ID , "tlgm.LineId"},
               {TransactionLineGamePlayMappingDTO.SearchByParameters.SITE_ID , "tlgm.site_id"},
                {TransactionLineGamePlayMappingDTO.SearchByParameters.MASTER_ENTITY_ID , "tlgm.MasterEntityId"},
                {TransactionLineGamePlayMappingDTO.SearchByParameters.GAMEPLAY_ID , "tlgm.GamePlayId"},
                 {TransactionLineGamePlayMappingDTO.SearchByParameters.IS_ACTIVE , "tlgm.IsActive"}
        };

        /// <summary>
        /// Default constructor of TransactionLineGamePlayMappingDataHandler class
        /// </summary>
        public TransactionLineGamePlayMappingDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(TransactionLineGamePlayMappingDTO transactionLineGamePlayMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionLineGamePlayMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionLineGamePlayMappingId", transactionLineGamePlayMappingDTO.TransactionLineGamePlayMappingId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GamePlayId", transactionLineGamePlayMappingDTO.GamePlayId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", transactionLineGamePlayMappingDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionLineId", transactionLineGamePlayMappingDTO.TransactionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", transactionLineGamePlayMappingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", transactionLineGamePlayMappingDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", transactionLineGamePlayMappingDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the TransactionLineGamePlayMapping record to the database
        /// </summary>
        /// <returns>Returns inserted record id</returns>
        internal TransactionLineGamePlayMappingDTO Insert(TransactionLineGamePlayMappingDTO transactionLineGamePlayMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionLineGamePlayMappingDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[TransactionLineGamePlayMapping] 
                                                        (
                                                         GamePlayId,
                                                         TrxId,
                                                         LineId,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastUpdateDate,
                                                         site_id,
                                                         Guid,
                                                         MasterEntityId,
                                                         IsActive

                                                        ) 
                                                values 
                                                        (
                                                          @GamePlayId,
                                                          @TransactionId,
                                                          @TransactionLineId,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedUser,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @IsActive
                                                       
                                                         )SELECT* FROM TransactionLineGamePlayMapping WHERE TransactionLineGamePlayMappingId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(transactionLineGamePlayMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionLineGamePlayMappingDTO(transactionLineGamePlayMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionLineGamePlayMappingDTO);
            return transactionLineGamePlayMappingDTO;
        }

        private void RefreshTransactionLineGamePlayMappingDTO(TransactionLineGamePlayMappingDTO transactionLineGamePlayMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(transactionLineGamePlayMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                transactionLineGamePlayMappingDTO.TransactionLineGamePlayMappingId = Convert.ToInt32(dt.Rows[0]["TransactionLineGamePlayMappingId"]);
                transactionLineGamePlayMappingDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                transactionLineGamePlayMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                transactionLineGamePlayMappingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                transactionLineGamePlayMappingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                transactionLineGamePlayMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                transactionLineGamePlayMappingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the TransactionLineGamePlayMapping  record
        /// </summary>
        internal TransactionLineGamePlayMappingDTO Update(TransactionLineGamePlayMappingDTO transactionLineGamePlayMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionLineGamePlayMappingDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TransactionLineGamePlayMapping] set
                               [GamePlayId]                   = @GamePlayId,
                               [TrxId]                        = @TransactionId,
                               [LineId]                       = @TransactionLineId,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @IsActive,
                               [LastUpdatedBy]                = @LastUpdatedUser,
                               [LastUpdateDate]              = GETDATE()
                               where TransactionLineGamePlayMappingId = @TransactionLineGamePlayMappingId
                             SELECT * FROM TransactionLineGamePlayMapping WHERE TransactionLineGamePlayMappingId = @TransactionLineGamePlayMappingId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(transactionLineGamePlayMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionLineGamePlayMappingDTO(transactionLineGamePlayMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionLineGamePlayMappingDTO);
            return transactionLineGamePlayMappingDTO;
        }

        /// <summary>
        /// Converts the Data row object to TransactionLineGamePlayMappingDTO class type
        /// </summary>
        private TransactionLineGamePlayMappingDTO GetTransactionLineGamePlayMappingDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionLineGamePlayMappingDTO TransactionLineGamePlayMappingDataObject = new TransactionLineGamePlayMappingDTO(Convert.ToInt32(dataRow["TransactionLineGamePlayMappingId"]),
                                                    dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                    dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                                    dataRow["GamePlayId"] == DBNull.Value ? -1: Convert.ToInt32(dataRow["GamePlayId"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"].ToString(),
                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                    );
            log.LogMethodExit();
            return TransactionLineGamePlayMappingDataObject;
        }

        /// <summary>
        /// Gets the GetTransactionLineGamePlayMapping data of passed displaygroup
        /// </summary>
        /// <param name="transactionLineGamePlayMappingId">integer type parameter</param>
        /// <returns>Returns TransactionLineGamePlayMappingDTO</returns>
        internal TransactionLineGamePlayMappingDTO GetTransactionLineGamePlayMapping(int transactionLineGamePlayMappingId)
        {
            log.LogMethodEntry(transactionLineGamePlayMappingId);
            TransactionLineGamePlayMappingDTO result = null;
            string query = SELECT_QUERY + @" WHERE tlgm.TransactionLineGamePlayMappingId = @TransactionLineGamePlayMappingId";
            SqlParameter parameter = new SqlParameter("@TransactionLineGamePlayMappingId", transactionLineGamePlayMappingId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTransactionLineGamePlayMappingDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<TransactionLineGamePlayMappingDTO> GetTransactionLineGamePlayMappingDTOList(List<int> transactionLineGamePlayMappingIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(transactionLineGamePlayMappingIdList);
            List<TransactionLineGamePlayMappingDTO> transactionLineGamePlayMappingDTOList = new List<TransactionLineGamePlayMappingDTO>();
            string query = @"SELECT *
                            FROM TransactionLineGamePlayMapping, @transactionLineGamePlayMappingIdList List
                            WHERE TransactionLineGamePlayMappingId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@transactionLineGamePlayMappingIdList", transactionLineGamePlayMappingIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                transactionLineGamePlayMappingDTOList = table.Rows.Cast<DataRow>().Select(x => GetTransactionLineGamePlayMappingDTO(x)).ToList();
            }
            log.LogMethodExit(transactionLineGamePlayMappingDTOList);
            return transactionLineGamePlayMappingDTOList;
        }

        /// <summary>
        /// Gets the TransactionLineGamePlayMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TransactionLineGamePlayMappingDTO matching the search criteria</returns>    
        internal List<TransactionLineGamePlayMappingDTO> GetTransactionLineGamePlayMappingList(List<KeyValuePair<TransactionLineGamePlayMappingDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TransactionLineGamePlayMappingDTO> transactionLineGamePlayMappingDTOList = new List<TransactionLineGamePlayMappingDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionLineGamePlayMappingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionLineGamePlayMappingDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == TransactionLineGamePlayMappingDTO.SearchByParameters.TRANSACTION_LINE_GAME_PLAY_MAPPING_ID ||
                            searchParameter.Key == TransactionLineGamePlayMappingDTO.SearchByParameters.GAMEPLAY_ID ||
                            searchParameter.Key == TransactionLineGamePlayMappingDTO.SearchByParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        //else if (searchParameter.Key == TransactionLineGamePlayMappingDTO.SearchByParameters.SCORING_EVENT_CALENDAR_ID_LIST)
                        //{
                        //    query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                        //    parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        //}
                        else if (searchParameter.Key == TransactionLineGamePlayMappingDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionLineGamePlayMappingDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                transactionLineGamePlayMappingDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetTransactionLineGamePlayMappingDTO(x)).ToList();
            }
            log.LogMethodExit(transactionLineGamePlayMappingDTOList);
            return transactionLineGamePlayMappingDTOList;
        }
    }

}
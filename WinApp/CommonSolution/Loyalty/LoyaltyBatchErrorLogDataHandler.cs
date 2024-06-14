//using Semnox.Core.Customer.Loyalty.LoyaltyEngineDTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Loyalty
{
    /// <summary>
    ///  LoyaltyBatchErrorLog Data Handler - Handles insert, update and select of  LoyaltyBatchErrorLog objects
    /// </summary>
    public class LoyaltyBatchErrorLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<LoyaltyBatchErrorLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyBatchErrorLogDTO.SearchByParameters, string>
            {
                {LoyaltyBatchErrorLogDTO.SearchByParameters.LOYALTY_BATCH_ERROR_LOG_ID, "LoyaltyBatchErrorLogId"},
                {LoyaltyBatchErrorLogDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {LoyaltyBatchErrorLogDTO.SearchByParameters.SITE_ID, "Site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of LoyaltyBatchErrorLogDataHandler class
        /// </summary>
        public LoyaltyBatchErrorLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyBatchErrorLog Record.
        /// </summary>
        /// <param name="loyaltyBatchErrorLogDTO">LoyaltyBatchErrorLogDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyBatchErrorLogDTO loyaltyBatchErrorLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(loyaltyBatchErrorLogDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyBatchErrorLogId", loyaltyBatchErrorLogDTO.LoyaltyBatchErrorLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", loyaltyBatchErrorLogDTO.TransactionId, true));
            // parameters.Add(dataAccessHandler.GetSQLParameter("@GameplayID", loyaltyBatchErrorLogDTO.GameplayID, true));
            if (loyaltyBatchErrorLogDTO.GameplayID == -1)
            {
                parameters.Add(new SqlParameter("@GameplayID", DBNull.Value));
            }
            else
                parameters.Add(new SqlParameter("@GameplayID", loyaltyBatchErrorLogDTO.GameplayID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ErrorDescription", loyaltyBatchErrorLogDTO.ErrorDescription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", loyaltyBatchErrorLogDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyBatchErrorLogDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the LoyaltyBatchErrorLog record to the database
        /// </summary>
        /// <param name="loyaltyBatchErrorLogDTO">LoyaltyBatchErrorLogDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertLoyaltyBatchErrorLog(LoyaltyBatchErrorLogDTO loyaltyBatchErrorLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(loyaltyBatchErrorLogDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO LoyaltyBatchErrorLog 
                                        ( 
                                              TransactionId ,
                                              GameplayID ,
                                              ErrorDescription,
                                              Guid ,
                                              CreatedBy ,
                                              CreationDate ,
                                              LastUpdatedBy ,
                                              LastUpdateDate ,
                                              Site_id ,
                                            --  SynchStatus ,
                                              MasterEntityId 
                                        ) 
                                VALUES 
                                        (
                                            @TransactionId ,
                                            @GameplayID ,
                                            @ErrorDescription,
                                            NEWID() ,
                                            @CreatedBy ,
                                            GETDATE() ,
                                            @LastUpdatedBy ,
                                            GETDATE() ,
                                            @SiteId ,
                                            --@SynchStatus ,
                                            @MasterEntityId 
                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(loyaltyBatchErrorLogDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the LoyaltyBatchErrorLog record
        /// </summary>
        /// <param name="loyaltyBatchErrorLogDTO">LoyaltyBatchErrorLogDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateLoyaltyBatchErrorLog(LoyaltyBatchErrorLogDTO loyaltyBatchErrorLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(loyaltyBatchErrorLogDTO, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE LoyaltyBatchErrorLog 
                             SET TransactionId = @TransactionId,
                                 GameplayID = @GameplayID, 
                                 ErrorDescription = @ErrorDescription,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate = GETDATE(),
                                -- SynchStatus = @SynchStatus,
                                -- site_id = @SiteId,
                                 MasterEntityId=@MasterEntityId
                             WHERE LoyaltyBatchErrorLogId = @LoyaltyBatchErrorLogId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(loyaltyBatchErrorLogDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to LoyaltyBatchErrorLogDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns LoyaltyBatchErrorLogDTO</returns>
        private LoyaltyBatchErrorLogDTO GetLoyaltyBatchErrorLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyBatchErrorLogDTO loyaltyBatchErrorLogDTO = new LoyaltyBatchErrorLogDTO(Convert.ToInt32(dataRow["LoyaltyBatchErrorLogId"]),
                                            dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                                            dataRow["GameplayID"] == DBNull.Value ? -1 : Convert.ToInt64(dataRow["GameplayID"]),
                                            dataRow["ErrorDescription"] == DBNull.Value ? "" : dataRow["ErrorDescription"].ToString(),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"])
                                            );
            log.LogMethodExit(loyaltyBatchErrorLogDTO);
            return loyaltyBatchErrorLogDTO;
        }

        /// <summary>
        /// Gets the LoyaltyBatchErrorLog data of passed LoyaltyBatchErrorLog Id
        /// </summary>
        /// <param name="loyaltyBatchProcessId">integer type parameter</param>
        /// <returns>Returns LoyaltyBatchErrorLogDTO</returns>
        public LoyaltyBatchErrorLogDTO GetLoyaltyBatchErrorLogDTO(int loyaltyBatchProcessId)
        {
            log.LogMethodEntry(loyaltyBatchProcessId);
            LoyaltyBatchErrorLogDTO returnValue = null;
            string query = @"SELECT *
                               FROM LoyaltyBatchErrorLog
                              WHERE LoyaltyBatchErrorLogId = @LoyaltyBatchErrorLogId";
            SqlParameter parameter = new SqlParameter("@LoyaltyBatchErrorLogId", loyaltyBatchProcessId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetLoyaltyBatchErrorLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the LoyaltyBatchErrorLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LoyaltyBatchErrorLogDTO matching the search criteria</returns>
        public List<LoyaltyBatchErrorLogDTO> GetLoyaltyBatchErrorLogDTOList(List<KeyValuePair<LoyaltyBatchErrorLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LoyaltyBatchErrorLogDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM LoyaltyBatchErrorLog ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<LoyaltyBatchErrorLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == LoyaltyBatchErrorLogDTO.SearchByParameters.LOYALTY_BATCH_ERROR_LOG_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == LoyaltyBatchErrorLogDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == LoyaltyBatchErrorLogDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<LoyaltyBatchErrorLogDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LoyaltyBatchErrorLogDTO loyaltyBatchErrorLogDTO = GetLoyaltyBatchErrorLogDTO(dataRow);
                    list.Add(loyaltyBatchErrorLogDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

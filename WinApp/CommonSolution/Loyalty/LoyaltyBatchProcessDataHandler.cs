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
    ///  LoyaltyBatchProcess Data Handler - Handles insert, update and select of  LoyaltyBatchProcess objects
    /// </summary>
    public class LoyaltyBatchProcessDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<LoyaltyBatchProcessDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyBatchProcessDTO.SearchByParameters, string>
            {
                {LoyaltyBatchProcessDTO.SearchByParameters.LOYALTY_BATCH_PROCESS_ID, "LoyaltyBatchProcessId"}, 
                {LoyaltyBatchProcessDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {LoyaltyBatchProcessDTO.SearchByParameters.SITE_ID, "Site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of LoyaltyBatchProcessDataHandler class
        /// </summary>
        public LoyaltyBatchProcessDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyBatchProcess Record.
        /// </summary>
        /// <param name="loyaltyBatchProcessDTO">LoyaltyBatchProcessDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyBatchProcessDTO loyaltyBatchProcessDTO, string userId, int siteId)
        {
            log.LogMethodEntry(loyaltyBatchProcessDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyBatchProcessId", loyaltyBatchProcessDTO.LoyaltyBatchProcessId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", loyaltyBatchProcessDTO.TransactionId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@GameplayID", loyaltyBatchProcessDTO.GameplayID, true)); 
            if(loyaltyBatchProcessDTO.GameplayID == -1)
            {
                parameters.Add(new SqlParameter("@GameplayID", DBNull.Value));
            }
            else
                parameters.Add(new SqlParameter("@GameplayID", loyaltyBatchProcessDTO.GameplayID));

            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", loyaltyBatchProcessDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyBatchProcessDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the LoyaltyBatchProcess record to the database
        /// </summary>
        /// <param name="loyaltyBatchProcessDTO">LoyaltyBatchProcessDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertLoyaltyBatchProcess(LoyaltyBatchProcessDTO loyaltyBatchProcessDTO, string userId, int siteId)
        {
            log.LogMethodEntry(loyaltyBatchProcessDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO LoyaltyBatchProcess 
                                        ( 
                                              TransactionId ,
                                              GameplayID ,
                                              Guid ,
                                              CreatedBy ,
                                              CreationDate ,
                                              LastUpdatedBy ,
                                              LastUpdateDate ,
                                              Site_id ,
                                             -- SynchStatus ,
                                              MasterEntityId 
                                        ) 
                                VALUES 
                                        (
                                            @TransactionId ,
                                            @GameplayID ,
                                            NEWID() ,
                                            @CreatedBy ,
                                            GETDATE() ,
                                            @LastUpdatedBy ,
                                            GETDATE() ,
                                            @SiteId ,
                                           -- @SynchStatus ,
                                            @MasterEntityId 
                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(loyaltyBatchProcessDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Updates the LoyaltyBatchProcess record
        /// </summary>
        /// <param name="loyaltyBatchProcessDTO">LoyaltyBatchProcessDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateLoyaltyBatchProcess(LoyaltyBatchProcessDTO loyaltyBatchProcessDTO, string userId, int siteId)
        {
            log.LogMethodEntry(loyaltyBatchProcessDTO, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE LoyaltyBatchProcess 
                             SET TransactionId = @TransactionId,
                                 GameplayID = @GameplayID, 
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate = GETDATE(),
                                -- SynchStatus = @SynchStatus,
                                 -- site_id = @SiteId,
                                 MasterEntityId=@MasterEntityId
                             WHERE LoyaltyBatchProcessId = @LoyaltyBatchProcessId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(loyaltyBatchProcessDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Converts the Data row object to LoyaltyBatchProcessDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns LoyaltyBatchProcessDTO</returns>
        private LoyaltyBatchProcessDTO GetLoyaltyBatchProcessDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyBatchProcessDTO loyaltyBatchProcessDTO = new LoyaltyBatchProcessDTO(Convert.ToInt32(dataRow["LoyaltyBatchProcessId"]),
                                            dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                                            dataRow["GameplayID"] == DBNull.Value ? -1 : Convert.ToInt64(dataRow["GameplayID"]),
                                             dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]) 
                                            ); 
            log.LogMethodExit(loyaltyBatchProcessDTO);
            return loyaltyBatchProcessDTO;
        }

        /// <summary>
        /// Gets the LoyaltyBatchProcess data of passed LoyaltyBatchProcess Id
        /// </summary>
        /// <param name="loyaltyBatchProcessId">integer type parameter</param>
        /// <returns>Returns LoyaltyBatchProcessDTO</returns>
        public LoyaltyBatchProcessDTO GetLoyaltyBatchProcessDTO(int loyaltyBatchProcessId)
        {
            log.LogMethodEntry(loyaltyBatchProcessId);
            LoyaltyBatchProcessDTO returnValue = null;
            string query = @"SELECT *
                            FROM LoyaltyBatchProcess
                            WHERE LoyaltyBatchProcessId = @LoyaltyBatchProcessId";
            SqlParameter parameter = new SqlParameter("@LoyaltyBatchProcessId", loyaltyBatchProcessId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetLoyaltyBatchProcessDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the LoyaltyBatchProcessDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LoyaltyBatchProcessDTO matching the search criteria</returns>
        public List<LoyaltyBatchProcessDTO> GetLoyaltyBatchProcessDTOList(List<KeyValuePair<LoyaltyBatchProcessDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LoyaltyBatchProcessDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM LoyaltyBatchProcess ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<LoyaltyBatchProcessDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == LoyaltyBatchProcessDTO.SearchByParameters.LOYALTY_BATCH_PROCESS_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == LoyaltyBatchProcessDTO.SearchByParameters.SITE_ID || 
                                 searchParameter.Key == LoyaltyBatchProcessDTO.SearchByParameters.MASTER_ENTITY_ID)
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
                list = new List<LoyaltyBatchProcessDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LoyaltyBatchProcessDTO loyaltyBatchProcessDTO = GetLoyaltyBatchProcessDTO(dataRow);
                    list.Add(loyaltyBatchProcessDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Purge PurgeBatchProcessLog data older than purgeDate
        /// </summary>
        /// <param name="purgeDate">Date para,</param> 

        public void PurgeBatchProcessLog(DateTime purgeDate)
        {
            log.LogMethodEntry(purgeDate);  
            string query = @"delete from LoyaltyBatchProcess where CreationDate < @PurgeDate";
            SqlParameter parameter = new SqlParameter("@PurgeDate", purgeDate);
            try
            {
                  dataAccessHandler.executeScalar(query, new SqlParameter[] { parameter }, sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            } 
            log.LogMethodExit();
        }
    }
}

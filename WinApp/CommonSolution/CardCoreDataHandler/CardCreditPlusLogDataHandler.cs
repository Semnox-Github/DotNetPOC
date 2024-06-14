using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CardCore
{
    class CardCreditPlusLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<CardCreditPlusLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CardCreditPlusLogDTO.SearchByParameters, string>
            {
                {CardCreditPlusLogDTO.SearchByParameters.CARD_CREDIT_PLUS_LOG_ID, "CardCreditPlusLogId"},
                {CardCreditPlusLogDTO.SearchByParameters.CARD_CREDIT_PLUS_ID, "CardCreditPlusId"},
                {CardCreditPlusLogDTO.SearchByParameters.CREDIT_PLUS_TYPE,"CreditPlusType"}, 
                {CardCreditPlusLogDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {CardCreditPlusLogDTO.SearchByParameters.SITE_ID, "site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of CardCreditPlusLogDataHandler class
        /// </summary>
        public CardCreditPlusLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating cardCreditPlusLog Record.
        /// </summary>
        /// <param name="cardCreditPlusLogDTO">cardCreditPlusLogDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CardCreditPlusLogDTO cardCreditPlusLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardCreditPlusLogDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardCreditPlusLogId", cardCreditPlusLogDTO.CardCreditPlusLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardCreditPlusId", cardCreditPlusLogDTO.CardCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creditPlus", cardCreditPlusLogDTO.CreditPlus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creditPlusType", cardCreditPlusLogDTO.CreditPlusType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creditPlusBalance", cardCreditPlusLogDTO.CreditPlusBalance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@playStartTime", cardCreditPlusLogDTO.PlayStartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@synchStatus", cardCreditPlusLogDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cardCreditPlusLogDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
             
        }

        /// <summary>
        /// Inserts the cardCreditPlusLog record to the database
        /// </summary>
        /// <param name="cardCreditPlusLogDTO">CardCreditPlusLogDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertCardCreditPlusLog(CardCreditPlusLogDTO cardCreditPlusLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardCreditPlusLogDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO CardCreditPlusLog  
                                        ( 
                                           CardCreditPlusId, 
                                           CreditPlus, 
                                           CreditPlusType, 
                                           CreditPlusBalance, 
                                           PlayStartTime, 
                                           CreatedBy,
                                           CreationDate,
                                           LastUpdatedBy, 
                                           LastupdatedDate, 
                                           Guid,
                                           site_id, 
                                           SynchStatus, 
                                           MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                           @cardCreditPlusId, 
                                           @creditPlus, 
                                           @creditPlusType, 
                                           @creditPlusBalance, 
                                           @playStartTime, 
                                           @createdBy,
                                           GETDATE(),
                                           @lastUpdatedBy, 
                                           GETDATE(),
                                           NEWID(),
                                           @siteId, 
                                           @synchStatus, 
                                           @masterEntityId
                                        )SELECT CAST(scope_identity() AS int)"; 
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(cardCreditPlusLogDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Updates the cardCreditPlusLog record
        /// </summary>
        /// <param name="cardCreditPlusLogDTO">CardCreditPlusLogDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateCardCreditPlusLog(CardCreditPlusLogDTO cardCreditPlusLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardCreditPlusLogDTO, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE CardCreditPlusLog 
                                SET CardCreditPlusId = @cardCreditPlusId, 
                                    CreditPlus = @creditPlus, 
                                    CreditPlusType = @creditPlusType, 
                                    CreditPlusBalance = @creditPlusBalance, 
                                    PlayStartTime = @playStartTime, 
                                    LastUpdatedBy = @lastUpdatedBy, 
                                    LastupdatedDate = GETDATE(), 
                                    --site_id = @siteId, 
                                    SynchStatus = @synchStatus, 
                                    MasterEntityId =  @masterEntityId
                              WHERE CardCreditPlusLogId = @cardCreditPlusLogId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(cardCreditPlusLogDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Converts the Data row object to CardCreditPlusLogDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CardCreditPlusLogDTO</returns>
        private CardCreditPlusLogDTO GetCardCreditPlusLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CardCreditPlusLogDTO cardCreditPlusLogDTO = new CardCreditPlusLogDTO(Convert.ToInt32(dataRow["CardCreditPlusLogId"]),
                                            dataRow["CardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardCreditPlusId"]),
                                            dataRow["CreditPlus"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["CreditPlus"]),
                                            dataRow["CreditPlusType"] == DBNull.Value ? "" : dataRow["CreditPlusType"].ToString(),
                                            dataRow["CreditPlusBalance"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["CreditPlusBalance"]),
                                            dataRow["PlayStartTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PlayStartTime"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            ); 
            log.LogMethodExit(cardCreditPlusLogDTO);
            return cardCreditPlusLogDTO;
        }

        /// <summary>
        /// Gets the CustomerRelationshipType data of passed cardCreditPlusLog Id
        /// </summary>
        /// <param name="cardCreditPlusLogId">integer type parameter</param>
        /// <returns>Returns CardCreditPlusLogDTO</returns>
        public CardCreditPlusLogDTO GetCardCreditPlusLogDTO(int id)
        {
            log.LogMethodEntry(id);
            CardCreditPlusLogDTO returnValue = null;
            string query = @"SELECT *
                            FROM CardCreditPlusLog
                            WHERE CardCreditPlusLogId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCardCreditPlusLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the CardCreditPlusLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CardCreditPlusLogDTO matching the search criteria</returns>
        public List<CardCreditPlusLogDTO> GetCardCreditPlusLogDTOList(List<KeyValuePair<CardCreditPlusLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CardCreditPlusLogDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM CardCreditPlusLog ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CardCreditPlusLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if ((searchParameter.Key == CardCreditPlusLogDTO.SearchByParameters.CARD_CREDIT_PLUS_LOG_ID) ||
                             (searchParameter.Key == CardCreditPlusLogDTO.SearchByParameters.CARD_CREDIT_PLUS_ID)  
                             )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if ((searchParameter.Key == CardCreditPlusLogDTO.SearchByParameters.SITE_ID) ||
                                 (searchParameter.Key == CardCreditPlusLogDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == CardCreditPlusLogDTO.SearchByParameters.CREDIT_PLUS_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= '" + searchParameter.Value +"' ");
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
                list = new List<CardCreditPlusLogDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CardCreditPlusLogDTO cardCreditPlusLogDTO = GetCardCreditPlusLogDTO(dataRow);
                    list.Add(cardCreditPlusLogDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

       

    }
}

/********************************************************************************************
 * Project Name - Card Credit Plus PauseTime Log Data Handler
 * Description  - Data handler of the CardCreditPlusPauseTimeLog class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar          Modified :Structure of data Handler - insert /Update methods
 *                                                            Fix for SQL Injection Issue
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query                                                            
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// data handler class for CardCreditPlusPauseTimeLog 
    /// </summary>
    public class CardCreditPlusPauseTimeLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<CardCreditPlusPauseTimeLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CardCreditPlusPauseTimeLogDTO.SearchByParameters, string>
            {
                {CardCreditPlusPauseTimeLogDTO.SearchByParameters.CARD_CP_PAUSE_TIMELOG_ID, "cpl.CardCPPauseTimeLogId"},
                {CardCreditPlusPauseTimeLogDTO.SearchByParameters.CARD_CREDIT_PLUS_ID, "cpl.CardCreditPlusId"},
                {CardCreditPlusPauseTimeLogDTO.SearchByParameters.BALANCE_TIME, "cpl.BalanceTime"},
                {CardCreditPlusPauseTimeLogDTO.SearchByParameters.REFERENCE, "cpl.Reference"},
                {CardCreditPlusPauseTimeLogDTO.SearchByParameters.SITE_ID, "cpl.site_id"},
                {CardCreditPlusPauseTimeLogDTO.SearchByParameters.MASTER_ENTITY_ID, "cpl.MasterEntityId"},
                {CardCreditPlusPauseTimeLogDTO.SearchByParameters.POS_MACHINE,"cpl.POSMachine" }
            };
        private readonly DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * from CardCPPauseTimeLog AS cpl ";

        /// <summary>s
        /// Default constructor of CardCreditPlusPauseTimeLogDataHandler class
        /// </summary>
        public CardCreditPlusPauseTimeLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CardCreditPlusPauseTimeLog Record.
        /// </summary>
        /// <param name="cardCreditPlusPauseTimeLogDTO">CardCreditPlusPauseTimeLogDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CardCreditPlusPauseTimeLogDTO cardCreditPlusPauseTimeLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardCreditPlusPauseTimeLogDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardCPPauseTimeLogId", cardCreditPlusPauseTimeLogDTO.CardCPPauseTimeLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardCreditPlusId", cardCreditPlusPauseTimeLogDTO.CardCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlayStartTime", cardCreditPlusPauseTimeLogDTO.PlayStartTime == DateTime.MinValue ? DBNull.Value : (object)cardCreditPlusPauseTimeLogDTO.PlayStartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PauseStartTime", cardCreditPlusPauseTimeLogDTO.PauseStartTime == DateTime.MinValue ? DBNull.Value : (object)cardCreditPlusPauseTimeLogDTO.PauseStartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BalanceTime", cardCreditPlusPauseTimeLogDTO.BalanceTime.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Reference", string.IsNullOrEmpty(cardCreditPlusPauseTimeLogDTO.Reference) ? DBNull.Value : (object)cardCreditPlusPauseTimeLogDTO.Reference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachine", string.IsNullOrEmpty(cardCreditPlusPauseTimeLogDTO.POSMachine) ? DBNull.Value : (object)cardCreditPlusPauseTimeLogDTO.POSMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", cardCreditPlusPauseTimeLogDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// method to insert InsertCardCreditPlusPauseTimeLog details to database 
        /// </summary>
        /// <param name="cardCreditPlusPauseTimeLogDTO">InsertCardCreditPlusPauseTimeLog DTO object</param>
        /// <param name="userId">ID of the currently loggedIn user</param>
        /// <param name="siteId">Current site id</param>
        /// <returns>Returns CardCreditPlusPauseTimeLogDTO </returns>
        public CardCreditPlusPauseTimeLogDTO InsertCardCreditPlusPauseTimeLog(CardCreditPlusPauseTimeLogDTO cardCreditPlusPauseTimeLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardCreditPlusPauseTimeLogDTO, userId, siteId);
            string insertQuery = @"INSERT INTO CardCPPauseTimeLog 
                                        ( 
                                            CardCreditPlusId,
                                            PlayStartTime,
                                            PauseStartTime,
                                            BalanceTime,
                                            Reference,
                                            POSMachine,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastupdatedDate,
                                            Guid,
                                            site_id,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @CardCreditPlusId,
                                            @PlayStartTime,
                                            @PauseStartTime,
                                            @BalanceTime,
                                            @Reference,
                                            @POSMachine,
                                            @CreatedBy,
                                            GetDate(),
                                            @LastUpdatedBy,
                                            GetDate(),
                                            NEWID(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM CardCPPauseTimeLog WHERE CardCPPauseTimeLogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(cardCreditPlusPauseTimeLogDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshCardCreditPlusPauseTimeLogDTO(cardCreditPlusPauseTimeLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting cardCreditPlusPauseTimeLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cardCreditPlusPauseTimeLogDTO);
            return cardCreditPlusPauseTimeLogDTO;
        }

        /// <summary>
        /// method to update CardCreditPlusPauseTimeLog details exist in the database
        /// </summary>
        /// <param name="cardCreditPlusPauseTimeLogDTO">CardCreditPlusPauseTimeLog DTO object</param>
        /// <param name="userId">updating user login id</param>
        /// <param name="siteId">current site id</param>
        /// <returns>Returns CardCreditPlusPauseTimeLogDTO</returns>
        public CardCreditPlusPauseTimeLogDTO UpdateCardCreditPlusPauseTimeLog(CardCreditPlusPauseTimeLogDTO cardCreditPlusPauseTimeLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardCreditPlusPauseTimeLogDTO, userId, siteId);
            string updateQuery = @"UPDATE CardCPPauseTimeLog 
                             SET CardCreditPlusId = @CardCreditPlusId,
                                 PauseStartTime = @PauseStartTime,
                                 BalanceTime = @BalanceTime,
                                 Reference = @Reference,
                                 POSMachine = @POSMachine,  
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastupdatedDate = GetDate(),   
                                 --site_id = @site_id,
                                 MasterEntityId = @MasterEntityId
                             WHERE CardCPPauseTimeLogId = @CardCPPauseTimeLogId 
                             SELECT * FROM CardCPPauseTimeLog WHERE CardCPPauseTimeLogId = @CardCPPauseTimeLogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(cardCreditPlusPauseTimeLogDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshCardCreditPlusPauseTimeLogDTO(cardCreditPlusPauseTimeLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cardCreditPlusPauseTimeLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cardCreditPlusPauseTimeLogDTO);
            return cardCreditPlusPauseTimeLogDTO;

        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="cardCreditPlusPauseTimeLogDTO">CardCreditPlusPauseTimeLogDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCardCreditPlusPauseTimeLogDTO(CardCreditPlusPauseTimeLogDTO cardCreditPlusPauseTimeLogDTO, DataTable dt)
        {
            log.LogMethodEntry(cardCreditPlusPauseTimeLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cardCreditPlusPauseTimeLogDTO.CardCPPauseTimeLogId = Convert.ToInt32(dt.Rows[0]["CardCPPauseTimeLogId"]);
                cardCreditPlusPauseTimeLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                cardCreditPlusPauseTimeLogDTO.LastupdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                cardCreditPlusPauseTimeLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                cardCreditPlusPauseTimeLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                cardCreditPlusPauseTimeLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                cardCreditPlusPauseTimeLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CardCreditPlusPauseTimeLogDTO class type
        /// </summary>
        /// <param name="cardCPPausTimeDataRow">CardCPPausTime DataRow</param>
        /// <returns>Returns CardCPPausTimeLog</returns>
        private CardCreditPlusPauseTimeLogDTO GetCardCPPauseTimeLogDTO(DataRow cardCPPauseTimeLogDataRow)
        {
            log.LogMethodEntry(cardCPPauseTimeLogDataRow);
            CardCreditPlusPauseTimeLogDTO cardCPPauseTimeLogDataObject = new CardCreditPlusPauseTimeLogDTO(Convert.ToInt32(cardCPPauseTimeLogDataRow["CardCPPauseTimeLogId"]),
                                            cardCPPauseTimeLogDataRow["CardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCPPauseTimeLogDataRow["CardCreditPlusId"]),
                                            cardCPPauseTimeLogDataRow["PlayStartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCPPauseTimeLogDataRow["PlayStartTime"]),
                                            cardCPPauseTimeLogDataRow["PauseStartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCPPauseTimeLogDataRow["PauseStartTime"]),
                                            cardCPPauseTimeLogDataRow["BalanceTime"] == DBNull.Value ? -1 : Convert.ToInt32(cardCPPauseTimeLogDataRow["BalanceTime"]),
                                            cardCPPauseTimeLogDataRow["Reference"] == DBNull.Value ? string.Empty : cardCPPauseTimeLogDataRow["Reference"].ToString(),
                                            cardCPPauseTimeLogDataRow["POSMachine"] == DBNull.Value ? string.Empty : cardCPPauseTimeLogDataRow["POSMachine"].ToString(),
                                            cardCPPauseTimeLogDataRow["CreatedBy"] == DBNull.Value ? string.Empty : cardCPPauseTimeLogDataRow["CreatedBy"].ToString(),
                                            cardCPPauseTimeLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCPPauseTimeLogDataRow["CreationDate"]),
                                            cardCPPauseTimeLogDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : cardCPPauseTimeLogDataRow["LastUpdatedBy"].ToString(),
                                            cardCPPauseTimeLogDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCPPauseTimeLogDataRow["LastupdatedDate"]),
                                            cardCPPauseTimeLogDataRow["Guid"] == DBNull.Value ? string.Empty : cardCPPauseTimeLogDataRow["Guid"].ToString(),
                                            cardCPPauseTimeLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cardCPPauseTimeLogDataRow["site_id"]),
                                            cardCPPauseTimeLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cardCPPauseTimeLogDataRow["SynchStatus"]),
                                            cardCPPauseTimeLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCPPauseTimeLogDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(cardCPPauseTimeLogDataObject);
            return cardCPPauseTimeLogDataObject;
        }

        /// <summary>
        /// Gets the CardCreditPlusPauseTimeLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CardCreditPlusPauseTimeLogDTO matching the search criteria</returns>
        public List<CardCreditPlusPauseTimeLogDTO> GetCardCPPauseTimeLogList(List<KeyValuePair<CardCreditPlusPauseTimeLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCardCPPauseTimeQuery = SELECT_QUERY;
            List<CardCreditPlusPauseTimeLogDTO> cardCPPauseTimeList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CardCreditPlusPauseTimeLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == CardCreditPlusPauseTimeLogDTO.SearchByParameters.CARD_CP_PAUSE_TIMELOG_ID
                            || searchParameter.Key == CardCreditPlusPauseTimeLogDTO.SearchByParameters.CARD_CREDIT_PLUS_ID
                            || searchParameter.Key == CardCreditPlusPauseTimeLogDTO.SearchByParameters.BALANCE_TIME
                            || searchParameter.Key == CardCreditPlusPauseTimeLogDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CardCreditPlusPauseTimeLogDTO.SearchByParameters.POS_MACHINE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CardCreditPlusPauseTimeLogDTO.SearchByParameters.REFERENCE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CardCreditPlusPauseTimeLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                if (searchParameters.Count > 0)
                    selectCardCPPauseTimeQuery = selectCardCPPauseTimeQuery + query;
            }
            DataTable cardCPPauseTimeData = dataAccessHandler.executeSelectQuery(selectCardCPPauseTimeQuery, parameters.ToArray(), sqlTransaction);
            if (cardCPPauseTimeData.Rows.Count > 0)
            {
                cardCPPauseTimeList = new List<CardCreditPlusPauseTimeLogDTO>();
                foreach (DataRow cardCPPauseTimeDataRow in cardCPPauseTimeData.Rows)
                {
                    CardCreditPlusPauseTimeLogDTO cardCPPauseTimeDataObject = GetCardCPPauseTimeLogDTO(cardCPPauseTimeDataRow);
                    cardCPPauseTimeList.Add(cardCPPauseTimeDataObject);
                }
            }
            log.LogMethodExit(cardCPPauseTimeList);
            return cardCPPauseTimeList;

        }

        /// <summary>
        /// Gets the CardCreditPlusPauseTimeLogDTO list matching the card Id
        /// </summary>
        /// <param name="cardId">card id</param>
        /// <returns>Returns the list of CardCreditPlusPauseTimeLogDTO matching the search criteria</returns>
        public List<CardCreditPlusPauseTimeLogDTO> GetCardCPPauseTimeLogListByCardId(int cardId)
        {
            log.LogMethodEntry(cardId);
            string selectCardCPPauseTimeQuery = @"SELECT l.*
                                                    FROM CardCPPauseTimeLog l, CardCreditPlus cp
                                                   WHERE l.cardCreditPlusId = cp.cardCreditPlusId
                                                     AND cp.Card_id = @cardId";
            List<CardCreditPlusPauseTimeLogDTO> cardCPPauseTimeList = null;
            SqlParameter[] selectGameParameters = new SqlParameter[1];
            selectGameParameters[0] = new SqlParameter("@cardId", cardId);
            DataTable cardCPPauseTimeData = dataAccessHandler.executeSelectQuery(selectCardCPPauseTimeQuery, selectGameParameters, sqlTransaction);
            if (cardCPPauseTimeData.Rows.Count > 0)
            {
                cardCPPauseTimeList = new List<CardCreditPlusPauseTimeLogDTO>();
                foreach (DataRow cardCPPauseTimeDataRow in cardCPPauseTimeData.Rows)
                {
                    CardCreditPlusPauseTimeLogDTO cardCPPauseTimeDataObject = GetCardCPPauseTimeLogDTO(cardCPPauseTimeDataRow);
                    cardCPPauseTimeList.Add(cardCPPauseTimeDataObject);
                }
            }
            log.LogMethodExit(cardCPPauseTimeList);
            return cardCPPauseTimeList;

        }

    }
}

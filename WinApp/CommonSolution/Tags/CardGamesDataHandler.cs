/********************************************************************************************
 * Project Name - Semnox.Parafait.Tags -CardGamesDataHandler
 * Description  - CardGamesDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80.0      19-Mar-2020   Mathew NInan            Added new field ValidityStatus to track
 *                                                  status of entitlements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    /// <summary>
    ///  CardGamesDataHandler Data Handler - Handles insert, update and select of  CardGamesDataHandler objects
    /// </summary>
    public class CardGamesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<CardGamesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CardGamesDTO.SearchByParameters, string>
            {
                {CardGamesDTO.SearchByParameters.CARD_GAME_ID, "card_game_id"},
                {CardGamesDTO.SearchByParameters.CARD_ID, "card_id"},
                {CardGamesDTO.SearchByParameters.GAME_ID, "game_id"},
                {CardGamesDTO.SearchByParameters.GAME_PROFILE_ID, "game_profile_id"},
                {CardGamesDTO.SearchByParameters.EXPIRE_WITH_MEMBERSHIP,"ExpireWithMembership"},
                {CardGamesDTO.SearchByParameters.MEMBERSHIP_ID, "MembershipId"},
                {CardGamesDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID, "MembershipRewardsId"},
                 {CardGamesDTO.SearchByParameters.TRANSACTION_ID, "TrxId"},
                {CardGamesDTO.SearchByParameters.SITE_ID, "site_id"} ,
                {CardGamesDTO.SearchByParameters.VALIDITYSTATUS, "ValidityStatus"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of CardGamesDataHandler class
        /// </summary>
        public CardGamesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CardGames Record.
        /// </summary>
        /// <param name="cardGamesDTO">CardGamesDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CardGamesDTO cardGamesDTO, string userId, int siteId)
        {
            
            log.LogMethodEntry(cardGamesDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Card_Game_Id", cardGamesDTO.CardGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Card_Id", cardGamesDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Game_Id", cardGamesDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Quantity", cardGamesDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", cardGamesDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Game_Profile_Id", cardGamesDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Frequency", cardGamesDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastPlayedTime", cardGamesDTO.LastPlayedTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BalanceGames", cardGamesDTO.BalanceGames));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", cardGamesDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Site_Id", cardGamesDTO.Site_Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Last_Update_Date", cardGamesDTO.LastUpdateDate)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardTypeId", cardGamesDTO.CardTypeId, true)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", cardGamesDTO.TrxId, true));  
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxLineId", cardGamesDTO.TrxLineId, true));  
            parameters.Add(dataAccessHandler.GetSQLParameter("@EntitlementType", cardGamesDTO.EntitlementType));  
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptionalAttribute", cardGamesDTO.OptionalAttribute));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", cardGamesDTO.SynchStatus)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomDataSetId", cardGamesDTO.CustomDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketAllowed", cardGamesDTO.TicketAllowed));  
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", cardGamesDTO.FromDate)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", cardGamesDTO.MasterEntityId, true)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@Monday", cardGamesDTO.Monday)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@Tuesday", cardGamesDTO.Tuesday)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@Wednesday", cardGamesDTO.Wednesday)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@Thursday", cardGamesDTO.Thursday)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@Friday", cardGamesDTO.Friday)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@Saturday", cardGamesDTO.Saturday)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sunday", cardGamesDTO.Sunday)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpireWithMembership", cardGamesDTO.ExpireWithMembership)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipId", cardGamesDTO.MembershipId, true)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipRewardsId", cardGamesDTO.MembershipRewardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", cardGamesDTO.CreatedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreationDate", cardGamesDTO.CreationDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", cardGamesDTO.LastUpdatedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidityStatus", (cardGamesDTO.ValidityStatus == CardGamesDTO.TagValidityStatus.Valid ? "Y" : "H")));

            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the cardGames record to the database
        /// </summary>
        /// <param name="cardGamesDTO">CardGamesDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertCardGames(CardGamesDTO cardGamesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardGamesDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO CardGames 
                                        ( 
                                            card_id
                                            ,game_id
                                            ,quantity
                                            ,ExpiryDate
                                            ,game_profile_id
                                            ,Frequency
                                            ,LastPlayedTime
                                            ,BalanceGames
                                            ,Guid
                                            ,site_id
                                            ,CreatedBy
                                            ,CreationDate
                                            ,LastUpdatedBy
                                            ,last_update_date
                                            ,CardTypeId
                                            ,TrxId
                                            ,TrxLineId
                                            ,EntitlementType
                                            ,OptionalAttribute
                                           -- ,SynchStatus
                                            ,CustomDataSetId
                                            ,TicketAllowed
                                            ,FromDate
                                            ,MasterEntityId
                                            ,Monday
                                            ,Tuesday
                                            ,Wednesday
                                            ,Thursday
                                            ,Friday
                                            ,Saturday
                                            ,Sunday
                                            ,ExpireWithMembership
                                            ,MembershipId
                                            ,MembershipRewardsId
                                            ,ValidityStatus
                                        ) 
                                VALUES 
                                        (
                                             @Card_Id
                                            ,@Game_Id
                                            ,@Quantity
                                            ,@ExpiryDate
                                            ,@Game_Profile_Id
                                            ,@Frequency
                                            ,@LastPlayedTime
                                            ,@BalanceGames
                                            ,NEWID()
                                            ,@Site_Id
                                            ,@CreatedBy
                                            ,GETDATE()
                                            ,@LastUpdatedBy
                                            ,GETDATE()
                                            ,@CardTypeId
                                            ,@TrxId
                                            ,@TrxLineId
                                            ,@EntitlementType
                                            ,@OptionalAttribute
                                            --,@SynchStatus
                                            ,@CustomDataSetId
                                            ,@TicketAllowed
                                            ,@FromDate
                                            ,@MasterEntityId
                                            ,@Monday
                                            ,@Tuesday
                                            ,@Wednesday
                                            ,@Thursday
                                            ,@Friday
                                            ,@Saturday
                                            ,@Sunday
                                            ,@ExpireWithMembership
                                            ,@MembershipId
                                            ,@MembershipRewardsId
                                            ,@ValidityStatus
                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(cardGamesDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Updates the CardGames record
        /// </summary>
        /// <param name="cardGamesDTO">CardGamesDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateCardGames(CardGamesDTO cardGamesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardGamesDTO, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE CardGames 
                                SET card_id = @Card_Id
                                   ,game_id = @Game_Id 
                                   ,quantity = @Quantity
                                   ,ExpiryDate = @ExpiryDate
                                   ,game_profile_id = @Game_Profile_Id
                                   ,Frequency = @Frequency
                                   ,LastPlayedTime = @LastPlayedTime
                                   ,BalanceGames = @BalanceGames 
                                   -- ,site_id = @Site_Id
                                   ,LastUpdatedBy = @LastUpdatedBy
                                   ,last_update_date = GETDATE()
                                   ,CardTypeId = @CardTypeId
                                   ,TrxId = @TrxId
                                   ,TrxLineId = @TrxLineId
                                   ,EntitlementType = @EntitlementType
                                   ,OptionalAttribute = @OptionalAttribute
                                 --  ,SynchStatus = @SynchStatus
                                   ,CustomDataSetId = @CustomDataSetId
                                   ,TicketAllowed = @TicketAllowed
                                   ,FromDate = @FromDate
                                   ,MasterEntityId = @MasterEntityId
                                   ,Monday = @Monday
                                   ,Tuesday = @Tuesday
                                   ,Wednesday = @Wednesday
                                   ,Thursday = @Thursday
                                   ,Friday = @Friday
                                   ,Saturday = @Saturday
                                   ,Sunday = @Sunday
                                   ,ExpireWithMembership = @ExpireWithMembership
                                   ,MembershipId = @MembershipId
                                   ,MembershipRewardsId = @MembershipRewardsId
                                   ,ValidityStatus = @ValidityStatus
                             WHERE Card_Game_Id = @Card_Game_Id";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(cardGamesDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Converts the Data row object to CardGamesDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CardGamesDTO</returns>
        private CardGamesDTO GetCardGamesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CardGamesDTO cardGamesDTO = new CardGamesDTO(Convert.ToInt32(dataRow["Card_Game_Id"]), 
                                                         dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                                         dataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_id"]),
                                                         dataRow["quantity"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["quantity"]),
                                                         dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                         dataRow["game_profile_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_profile_id"]),
                                                         dataRow["Frequency"] == DBNull.Value ? "" : Convert.ToString(dataRow["Frequency"]),
                                                         dataRow["LastPlayedTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastPlayedTime"]),
                                                         dataRow["BalanceGames"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BalanceGames"]),
                                                         dataRow["Guid"] == DBNull.Value ? "" : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["last_update_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["last_update_date"]),
                                                         dataRow["CardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardTypeId"]),
                                                         dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                         dataRow["TrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxLineId"]),
                                                         dataRow["EntitlementType"] == DBNull.Value ? "" : Convert.ToString(dataRow["EntitlementType"]),
                                                         dataRow["OptionalAttribute"] == DBNull.Value ? "" : Convert.ToString(dataRow["OptionalAttribute"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomDataSetId"]),
                                                         dataRow["TicketAllowed"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TicketAllowed"]),
                                                         dataRow["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["FromDate"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["Monday"] == DBNull.Value ? "" : Convert.ToString(dataRow["Monday"]), 
                                                         dataRow["Tuesday"] == DBNull.Value ? "" : Convert.ToString(dataRow["Tuesday"]),
                                                         dataRow["Wednesday"] == DBNull.Value ? "" : Convert.ToString(dataRow["Wednesday"]),
                                                         dataRow["Thursday"] == DBNull.Value ? "" : Convert.ToString(dataRow["Thursday"]),
                                                         dataRow["Friday"] == DBNull.Value ? "" : Convert.ToString(dataRow["Friday"]),
                                                         dataRow["Saturday"] == DBNull.Value ? "" : Convert.ToString(dataRow["Saturday"]),
                                                         dataRow["Sunday"] == DBNull.Value ? "" : Convert.ToString(dataRow["Sunday"]), 
                                                         dataRow["ExpireWithMembership"] == DBNull.Value ? "N" : dataRow["ExpireWithMembership"].ToString(),
                                                         dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                                         dataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRewardsId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["ValidityStatus"] == DBNull.Value ? CardGamesDTO.TagValidityStatus.Valid : (dataRow["ValidityStatus"].ToString() == "Y" ? CardGamesDTO.TagValidityStatus.Valid : CardGamesDTO.TagValidityStatus.Hold)
                                                                                              );
            log.LogMethodExit(cardGamesDTO);
            return cardGamesDTO;
        }

        /// <summary>
        /// Gets the CardGames data of passed CardGames Id
        /// </summary>
        /// <param name="cardGameId">integer type parameter</param>
        /// <returns>Returns CardGamesDTO</returns>
        public CardGamesDTO GetCardGamesDTO(int cardGameId)
        {
            log.LogMethodEntry(cardGameId);
            CardGamesDTO returnValue = null;
            string query = @"SELECT *
                            FROM CardGames
                            WHERE Card_Game_Id = @Card_Game_Id";
            SqlParameter parameter = new SqlParameter("@Card_Game_Id", cardGameId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCardGamesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the CardGamesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CardGamesDTO matching the search criteria</returns>
        public List<CardGamesDTO> GetCardGamesDTOList(List<KeyValuePair<CardGamesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CardGamesDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM CardGames ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CardGamesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if ( (searchParameter.Key == CardGamesDTO.SearchByParameters.CARD_GAME_ID) ||
                             (searchParameter.Key == CardGamesDTO.SearchByParameters.CARD_ID) ||
                             (searchParameter.Key == CardGamesDTO.SearchByParameters.GAME_ID) ||
                             (searchParameter.Key == CardGamesDTO.SearchByParameters.GAME_PROFILE_ID) ||
                             (searchParameter.Key == CardGamesDTO.SearchByParameters.MEMBERSHIP_ID) ||
                             (searchParameter.Key == CardGamesDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID) ||
                             (searchParameter.Key == CardGamesDTO.SearchByParameters.TRANSACTION_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == CardGamesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == CardGamesDTO.SearchByParameters.EXPIRE_WITH_MEMBERSHIP)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')= '" + searchParameter.Value+"' ");
                        }
                        else if (searchParameter.Key == CardGamesDTO.SearchByParameters.VALIDITYSTATUS)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')= '" + searchParameter.Value + "' ");
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
                list = new List<CardGamesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CardGamesDTO cardGamesDTO = GetCardGamesDTO(dataRow);
                    list.Add(cardGamesDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

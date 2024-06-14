/********************************************************************************************
 * Project Name - Locker Blocked Cards DataHandler
 * Description  - Data handler of the Locker Blocked Cards  class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        05-Aug-2017   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                                                                                   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Locker Blocked Cards Data Handler - Handles insert, update and select of locker blocked cards data objects
    /// </summary>
    public class LockerBlockedCardsDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LockerBlockedCards AS lbc";

        /// <summary>
        /// Dictionary for searching Parameters for the Locker Blocked Cards object.
        /// </summary>
        private static readonly Dictionary<LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters, string> DBSearchParameters = new Dictionary<LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters, string>
            {
                {LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.CARD_BLOCK_ID, "lbc.CardBlockId"},
                {LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.CARD_NUMBER, "lbc.Name"},
                {LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.IS_ACTIVE, "lbc.IsActive"},
                {LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.MASTER_ENTITY_ID, "lbc.MasterEntityId"},
                {LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.SITE_ID, "lbc.site_id"}
            };

        /// <summary>
        /// Default constructor of LockerBlockedCardsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerBlockedCardsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LockerBlockedCards parameters Record.
        /// </summary>
        /// <param name="lockerBlockedCardsDTO">lockerBlockedCardsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(LockerBlockedCardsDTO lockerBlockedCardsDTO , string loginId, int siteId)
        {
            log.LogMethodEntry(lockerBlockedCardsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
         
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardBlockId", lockerBlockedCardsDTO.CardBlockId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardNumber", lockerBlockedCardsDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", lockerBlockedCardsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", lockerBlockedCardsDTO.Siteid, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lockerBlockedCardsDTO.MasterEntityId, true));
            return parameters;
        }

        /// <summary>
        /// Inserts the lockerBlockedCards record to the database
        /// </summary>
        /// <param name="lockerBlockedCards">LockerBlockedCardsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public LockerBlockedCardsDTO InsertLockerBlockedCards(LockerBlockedCardsDTO lockerBlockedCards, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerBlockedCards, loginId, siteId);
            string insertLockerBlockedCardsQuery = @"INSERT INTO [dbo].[LockerBlockedCards] 
                                                        ( 
                                                          CardNumber,                                                          
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy, 
                                                          LastupdatedDate,
                                                          Guid,
                                                          site_id,
                                                          MasterEntityId
                                                        ) 
                                                values 
                                                        (    
                                                          @cardNumber,                                                          
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate(),                                                       
                                                          Newid(),
                                                          @siteid,
                                                          @masterEntityId
                                                        )SELECT * FROM LockerBlockedCards WHERE CardBlockId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLockerBlockedCardsQuery, GetSQLParameters(lockerBlockedCards, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerBlockedCardsDTO(lockerBlockedCards, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting lockerBlockedCards", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerBlockedCards);
            return lockerBlockedCards;
        }

        /// <summary>
        /// Updates the lockerBlockedCards record
        /// </summary>
        /// <param name="lockerBlockedCards">LockerBlockedCardsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public LockerBlockedCardsDTO UpdateLockerBlockedCards(LockerBlockedCardsDTO lockerBlockedCards, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerBlockedCards, loginId, siteId);
            string updateLockerBlockedCardsQuery = @"update LockerBlockedCards 
                                                   set 
                                                   CardNumber = @cardNumber,
                                                   IsActive = @isActive,
                                                   LastUpdatedBy = @lastUpdatedBy, 
                                                   LastupdatedDate = Getdate(),
                                                   -- site_id=@siteid,
                                                   MasterEntityId=@masterEntityId
                                                   where CardBlockId = @cardBlockId
                                                   SELECT* FROM LockerBlockedCards WHERE  CardBlockId = @cardBlockId";
            try                                    
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateLockerBlockedCardsQuery, GetSQLParameters(lockerBlockedCards, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerBlockedCardsDTO(lockerBlockedCards, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LockerBlockedCards", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerBlockedCards);
            return lockerBlockedCards;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lockerBlockedCardsDTO">lockerBlockedCardsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLockerBlockedCardsDTO(LockerBlockedCardsDTO lockerBlockedCardsDTO , DataTable dt)
        {
            log.LogMethodEntry(lockerBlockedCardsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lockerBlockedCardsDTO.CardBlockId = Convert.ToInt32(dt.Rows[0]["CardBlockId"]);
                lockerBlockedCardsDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                lockerBlockedCardsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lockerBlockedCardsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lockerBlockedCardsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lockerBlockedCardsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lockerBlockedCardsDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LockerBlockedCardsDTO class type
        /// </summary>
        /// <param name="lockerBlockedCardsDataRow">LockerBlockedCardsDTO DataRow</param>
        /// <returns>Returns LockerBlockedCardsDTO</returns>
        private LockerBlockedCardsDTO GetLockerBlockedCardsDTO(DataRow lockerBlockedCardsDataRow)
        {
            log.LogMethodEntry();

            LockerBlockedCardsDTO lockerBlockedCardsDataObject = new LockerBlockedCardsDTO(Convert.ToInt32(lockerBlockedCardsDataRow["CardBlockId"]),
                                            lockerBlockedCardsDataRow["CardNumber"].ToString(),                                            
                                            lockerBlockedCardsDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(lockerBlockedCardsDataRow["IsActive"]),
                                            lockerBlockedCardsDataRow["CreatedBy"].ToString(),
                                            lockerBlockedCardsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerBlockedCardsDataRow["CreationDate"]),
                                            lockerBlockedCardsDataRow["LastUpdatedBy"].ToString(),
                                            lockerBlockedCardsDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerBlockedCardsDataRow["LastupdatedDate"]),
                                            lockerBlockedCardsDataRow["Guid"].ToString(),
                                            lockerBlockedCardsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(lockerBlockedCardsDataRow["site_id"]),
                                            lockerBlockedCardsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(lockerBlockedCardsDataRow["SynchStatus"]),
                                            lockerBlockedCardsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerBlockedCardsDataRow["MasterEntityId"])//Modification on 18-Jul-2016 for publish feature
                                            );
            log.LogMethodExit(lockerBlockedCardsDataObject);
            return lockerBlockedCardsDataObject;
        }

        /// <summary>
        /// Gets the lockerBlockedCards data of passed lockerBlockedCards Id
        /// </summary>
        /// <param name="cardBlockId">integer type parameter</param>
        /// <returns>Returns LockerBlockedCardsDTO</returns>
        public LockerBlockedCardsDTO GetLockerBlockedCards(int cardBlockId)
        {
            log.LogMethodEntry(cardBlockId);
            LockerBlockedCardsDTO result = null;
            string selectLockerBlockedCardsQuery = SELECT_QUERY + @" WHERE lbc.CardBlockId = @cardBlockId";
            SqlParameter parameter = new SqlParameter("@cardBlockId", cardBlockId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLockerBlockedCardsQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetLockerBlockedCardsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Removes data of passed lockerBlockedCards 
        /// </summary>
        /// <param name="cardnumber">string parameter</param>
        /// <returns>Returns LockerBlockedCardsDTO</returns>
        public void RemoveBlockedCards(string  cardnumber)
        {
            log.LogMethodEntry(cardnumber);
            string selectLockerBlockedCardsQuery = @"Delete
                                         from LockerBlockedCards
                                        where CardNumber = @cardnumber";
            SqlParameter[] selectLockerBlockedCardsParameters = new SqlParameter[1];
            selectLockerBlockedCardsParameters[0] = new SqlParameter("@cardnumber", cardnumber);
            dataAccessHandler.executeSelectQuery(selectLockerBlockedCardsQuery, selectLockerBlockedCardsParameters, sqlTransaction);
            log.LogMethodExit(cardnumber);
        }

        /// <summary>
        /// Gets the card dto if the passed card number is blocked 
        /// </summary>
        /// <param name="cardnumber">string type parameter</param>
        /// <returns>Returns LockerBlockedCardsDTO if passed card number exists in blocked list</returns>
        public LockerBlockedCardsDTO GetLockerBlockedCard(string cardnumber)
        {
            log.LogMethodEntry(cardnumber);
            LockerBlockedCardsDTO lockerBlockedCardsDataObject = null; ;
            string selectLockerBlockedCardsQuery = @"select *
                                         from LockerBlockedCards
                                        where Cardnumber = @cardnumber and IsActive = 1";
            SqlParameter[] selectLockerBlockedCardsParameters = new SqlParameter[1];
            selectLockerBlockedCardsParameters[0] = new SqlParameter("@cardnumber", cardnumber);
            DataTable lockerBlockedCards = dataAccessHandler.executeSelectQuery(selectLockerBlockedCardsQuery, selectLockerBlockedCardsParameters, sqlTransaction);
            if (lockerBlockedCards.Rows.Count > 0)
            {
                DataRow lockerBlockedCardsRow = lockerBlockedCards.Rows[0];
                lockerBlockedCardsDataObject = GetLockerBlockedCardsDTO(lockerBlockedCardsRow);
               
            }
            log.LogMethodExit(lockerBlockedCardsDataObject);
            return lockerBlockedCardsDataObject;
        }

        /// <summary>
        /// Gets the LockerBlockedCardsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LockerBlockedCardsDTO matching the search criteria</returns>
        public List<LockerBlockedCardsDTO> GetLockerBlockedCardsList(List<KeyValuePair<LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LockerBlockedCardsDTO> lockerBlockedCardsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.CARD_BLOCK_ID
                            || searchParameter.Key == LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                lockerBlockedCardsDTOList = new List<LockerBlockedCardsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LockerBlockedCardsDTO lockerBlockedCardsDTO  = GetLockerBlockedCardsDTO(dataRow);
                    lockerBlockedCardsDTOList.Add(lockerBlockedCardsDTO);
                }
            }
            log.LogMethodExit(lockerBlockedCardsDTOList);
            return lockerBlockedCardsDTOList;
        }
    }
}

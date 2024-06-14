/********************************************************************************************
 * Project Name - CardCore  Datahandler Programs 
 * Description  - Data object of the CardCoreDatahandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00       15-Nov-2016    Rakshith           Created 
 *2.50.0     04-Dec-2018    Mathew Ninan      deprecated Staticdataexchange 
 *2.60.0     03-May-2019    Divya             SQL Injection
 *2.70.3     09-Oct-2020    Girish            Modified : Fix for Cowplay- Negative Tech Games update. Membership is updating 'games' as -1 on the card
 *2.110.0    08-Apr-2021    Girish            Fix for sql transaction corruption issue during memberhip engine run
 *2.12.0     09-Oct-2020    Guru S A          Membership engine sql session issue
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.CardCore
{
    public class CardCoreDatahandler
    {
        DataAccessHandler dataAccessHandler;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Utilities parafaitUtility;
        //string connstring;
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CardCoreDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CardCoreDTO.SearchByParameters, string>
        {
            {CardCoreDTO.SearchByParameters.CARD_ID, "card_id"},
            {CardCoreDTO.SearchByParameters.CARD_NUMBER, "card_number"} ,
            {CardCoreDTO.SearchByParameters.CUSTOMER_ID, "customer_id"} ,
            {CardCoreDTO.SearchByParameters.VALID_FLAG, "valid_flag"} ,
            {CardCoreDTO.SearchByParameters.CUSTOMER_PHONE_NUMBER, "contact_phone1"} ,

        };
        /// <summary>
        /// Default constructor of  AgentsDataHandler class
        /// </summary>
        public CardCoreDatahandler()
        {
            log.Debug("starts-CustomersDatahandler() Method.");
            dataAccessHandler = new DataAccessHandler();
            //connstring = dataAccessHandler.ConnectionString;
            log.Debug("Ends-CustomersDatahandler() Method.");

        }
        /// <summary>
        /// return the record from the database
        /// Convert the datarow to CardCoreDTO object
        /// </summary>
        /// <returns>return the CardCoreDTO object</returns>
        private CardCoreDTO GetCardCoreDTO(DataRow cardDataRow)
        {
            log.Debug("starts- GetCustomer(DataRow cardDataRow) Method.");
            try
            {
                CardCoreDTO cardCoreDTO = new CardCoreDTO(
                                                            cardDataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["card_id"]),
                                                            cardDataRow["card_number"].ToString(),
                                                            cardDataRow["issue_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardDataRow["issue_date"]),
                                                            cardDataRow["face_value"] == DBNull.Value ? 0 : float.Parse(cardDataRow["face_value"].ToString()),
                                                            cardDataRow["refund_flag"] == DBNull.Value ? '\0' : Convert.ToChar(cardDataRow["refund_flag"]),
                                                            cardDataRow["refund_amount"] == DBNull.Value ? 0 : float.Parse(cardDataRow["refund_amount"].ToString()),
                                                            cardDataRow["refund_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardDataRow["refund_date"]),
                                                            cardDataRow["valid_flag"] == DBNull.Value ? '\0' : Convert.ToChar(cardDataRow["valid_flag"]),
                                                            cardDataRow["ticket_count"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["ticket_count"]),
                                                            cardDataRow["notes"].ToString(),
                                                            cardDataRow["last_update_time"].ToString(),
                                                            cardDataRow["credits"] == DBNull.Value ? 0 : Convert.ToDecimal(cardDataRow["credits"]),
                                                            cardDataRow["courtesy"] == DBNull.Value ? 0 : Convert.ToDecimal(cardDataRow["courtesy"]),
                                                            cardDataRow["bonus"] == DBNull.Value ? 0 : Convert.ToDecimal(cardDataRow["bonus"]),
                                                            cardDataRow["time"] == DBNull.Value ? 0 : Convert.ToDecimal(cardDataRow["time"]),
                                                            cardDataRow["customer_id"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["customer_id"]),
                                                            cardDataRow["credits_played"] == DBNull.Value ? 0 : Convert.ToDecimal(cardDataRow["credits_played"]),
                                                            cardDataRow["ticket_allowed"] == DBNull.Value ? '\0' : Convert.ToChar(cardDataRow["ticket_allowed"]),
                                                            cardDataRow["real_ticket_mode"] == DBNull.Value ? '\0' : Convert.ToChar(cardDataRow["real_ticket_mode"]),
                                                            cardDataRow["vip_customer"] == DBNull.Value ? '\0' : Convert.ToChar(cardDataRow["vip_customer"]),
                                                            cardDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["site_id"]),
                                                            cardDataRow["start_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardDataRow["start_time"]),
                                                            cardDataRow["last_played_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardDataRow["last_played_time"]),
                                                            cardDataRow["technician_card"] == DBNull.Value ? '\0' : Convert.ToChar(cardDataRow["technician_card"]),
                                                            cardDataRow["tech_games"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["tech_games"]),
                                                            cardDataRow["timer_reset_card"] == DBNull.Value ? '\0' : Convert.ToChar(cardDataRow["timer_reset_card"]),
                                                            cardDataRow["loyalty_points"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["loyalty_points"]),
                                                            cardDataRow["lastUpdatedBy"].ToString(),
                                                            cardDataRow["cardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["cardTypeId"]),
                                                            cardDataRow["guid"].ToString(),
                                                            cardDataRow["upload_site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["upload_site_id"]),
                                                            cardDataRow["upload_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardDataRow["upload_time"]),
                                                            cardDataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cardDataRow["synchStatus"]),
                                                            cardDataRow["expiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardDataRow["expiryDate"]),
                                                            cardDataRow["downloadBatchId"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["downloadBatchId"]),
                                                            cardDataRow["refreshFromHQTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardDataRow["refreshFromHQTime"]),
                                                            cardDataRow["masterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cardDataRow["masterEntityId"]),
                                                            cardDataRow["PrimaryCard"] == DBNull.Value ? "N" : cardDataRow["PrimaryCard"].ToString());

                cardCoreDTO.CardIdentifier = cardDataRow["cardIdentifier"] == DBNull.Value ? "" : cardDataRow["cardIdentifier"].ToString();

                log.Debug("Ends- GetCustomer(DataRow cardDataRow) Method.");
                return cardCoreDTO;
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// return the record from the database based on  customerId
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>return the CardCoreDTO object</returns>
        /// or empty CardCoreDTO
        public CardCoreDTO GetCardDTOByCardNumber(string CardNumber)
        {
            CardCoreDTO cardCoreDTO = new CardCoreDTO();
            log.Debug("starts- GetCardDTO(string CardNumber)");
            try
            {
                List<KeyValuePair<CardCoreDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CardCoreDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CARD_NUMBER, CardNumber));
                searchParameters.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.VALID_FLAG, "Y"));

                List<CardCoreDTO> cardDTOList = new List<CardCoreDTO>();
                cardDTOList = GetAllCardsList(searchParameters, false);
                if (cardDTOList.Count > 0)
                {
                    if (cardDTOList.Count == 1)
                        cardCoreDTO = cardDTOList[0];
                    else
                        throw new System.Exception("Error: Duplicate Cards with Same card # ");
                }
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message);
            }
            return cardCoreDTO;
        }


        /// <summary>
        /// return the record from the database based on  customerId
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>return the CardCoreDTO object</returns>
        /// or empty CardCoreDTO
        public CardCoreDTO GetCardDTOById(int cardId)
        {
            CardCoreDTO cardCoreDTO = new CardCoreDTO();
            log.Debug("starts- GetCardDTO(string CardNumber)");
            try
            {
                List<KeyValuePair<CardCoreDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CardCoreDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CARD_ID, cardId.ToString()));
                searchParameters.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.VALID_FLAG, "Y"));
                List<CardCoreDTO> cardDTOList = new List<CardCoreDTO>();
                cardDTOList = GetAllCardsList(searchParameters,false);
                if (cardDTOList.Count > 0)
                {
                    if (cardDTOList.Count == 1)
                        cardCoreDTO = cardDTOList[0];
                    else
                        throw new System.Exception("Error: Duplicate Cards with Same card # ");
                }
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message);
            }
            return cardCoreDTO;
        }

        
        /// <summary>
        /// Gets the CardCoreDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic CardCoreDTO matching the search criteria</returns>
        public List<CardCoreDTO> GetAllCardsList(List<KeyValuePair<CardCoreDTO.SearchByParameters, string>> searchParameters, Boolean loadCardDetails, SqlTransaction sqlTrx = null, string passPhrase = "")
        {
            log.Debug("Starts-GetAllCardsList(searchParameters) Method.");
            int count = 0;
            try
            {
                string selectCardQuery = @"SELECT *
                                                FROM Cards";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    StringBuilder query = new StringBuilder(" where ");
                    foreach (KeyValuePair<CardCoreDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            string joinOperartor = (count == 0) ? " " : " and ";
                            string joinOperartorOr = (count == 0) ? " " : " or ";

                            if (searchParameter.Key.Equals(CardCoreDTO.SearchByParameters.CARD_NUMBER) ||
                                 (searchParameter.Key.Equals(CardCoreDTO.SearchByParameters.VALID_FLAG)))
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =" + dataAccessHandler.GetParameterName(searchParameter.Key) );
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(CardCoreDTO.SearchByParameters.CARD_ID) ||
                                searchParameter.Key.Equals(CardCoreDTO.SearchByParameters.CUSTOMER_ID))
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(CardCoreDTO.SearchByParameters.CUSTOMER_PHONE_NUMBER))
                            {
                                string phoneQuery = @"customer_id in (SELECT customer_id from CustomerView('" + passPhrase + "') where contact_phone1  = " + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ";
                                query.Append(joinOperartor + phoneQuery );
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else
                            {
                                query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            count++;
                        }
                        else
                        {
                            log.Debug("Ends-GetAllCardsList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                            throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        }
                    }
                    if (searchParameters.Count > 0)
                        selectCardQuery = selectCardQuery + query;

                }
                DataTable dtCardDTO = dataAccessHandler.executeSelectQuery(selectCardQuery, parameters.ToArray(), sqlTrx);

                List<CardCoreDTO> cardCoreDTOList = new List<CardCoreDTO>();
                if (dtCardDTO.Rows.Count > 0)
                {
                    CardCreditPlusDataHandler cardCreditPlusDataHandler = new CardCreditPlusDataHandler();
                    CardGamesDataHandler cardGamesDataHandler = new CardGamesDataHandler(sqlTrx);
                    // CardDiscountsDataHandler cardDiscountsDataHandler = new CardDiscountsDataHandler(null);
                    Type type = Type.GetType("Semnox.Parafait.CardCore.CardDiscountsDataHandler,CardCoreDataHandler");
                    object cardDiscountsDataHandler = null;
                    if (type != null)
                    {
                        ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(SqlTransaction)});
                        cardDiscountsDataHandler = constructor.Invoke(new object[] { sqlTrx });
                    }
                    else
                        throw new Exception("Unable to fetch Card Discounts DataHandler class from assembly");

                    List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>> cardCreditPlusSearchParam;
                    cardCreditPlusSearchParam = new List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>>();
                    List<KeyValuePair<CardGamesDTO.SearchByParameters, string>> cardGamesSearchParam;
                    cardGamesSearchParam = new List<KeyValuePair<CardGamesDTO.SearchByParameters, string>>();
                    List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>> cardDiscountsSearchParam;
                    cardDiscountsSearchParam = new List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>>();

                    foreach (DataRow customersRow in dtCardDTO.Rows)
                    {
                        CardCoreDTO cardCoreDTO = GetCardCoreDTO(customersRow);
                        cardCoreDTO.CustomerFingerPrintDTO = GetCustomerFingerprintDto(cardCoreDTO.CardId, sqlTrx);
                        if(loadCardDetails)
                        { 
                            cardCoreDTO.CardCreditPlusDTOList = new List<CardCreditPlusDTO>();
                            cardCoreDTO.CardGamesDTOList = new List<CardGamesDTO>(); 
                            cardCoreDTO.CardDiscountsDTOList = new List<CardDiscountsDTO>(); 
                           
                            cardCreditPlusSearchParam.Add(new KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>(CardCreditPlusDTO.SearchByParameters.CARD_ID, cardCoreDTO.CardId.ToString()));
                            //cardCreditPlusSearchParam.Add(new KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>(CardCreditPlusDTO.SearchByParameters.SITE_ID, cardCoreDTO.Site_id.ToString()));
                            List<CardCreditPlusDTO> cardCreditPlusDTOAddList = cardCreditPlusDataHandler.GetAllCardCreditPlus(cardCreditPlusSearchParam, sqlTrx);
                            if (cardCreditPlusDTOAddList != null)
                                cardCoreDTO.CardCreditPlusDTOList.AddRange(cardCreditPlusDTOAddList);
                            cardCreditPlusSearchParam.Clear();

                            cardGamesSearchParam.Add(new KeyValuePair<CardGamesDTO.SearchByParameters, string>(CardGamesDTO.SearchByParameters.CARD_ID, cardCoreDTO.CardId.ToString()));
                            //cardGamesSearchParam.Add(new KeyValuePair<CardGamesDTO.SearchByParameters, string>(CardGamesDTO.SearchByParameters.SITE_ID, cardCoreDTO.Site_id.ToString()));
                            List<CardGamesDTO> cardGamesDTOAddList = cardGamesDataHandler.GetCardGamesDTOList(cardGamesSearchParam);
                            if (cardGamesDTOAddList != null)
                                cardCoreDTO.CardGamesDTOList.AddRange(cardGamesDTOAddList); 
                            cardGamesSearchParam.Clear();

                            cardDiscountsSearchParam.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.CARD_ID, cardCoreDTO.CardId.ToString()));
                           // cardDiscountsSearchParam.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.SITE_ID, cardCoreDTO.Site_id.ToString()));
                            List<CardDiscountsDTO> cardDiscountsDTOAddList = new List<CardDiscountsDTO>();
                            if (cardDiscountsDataHandler != null)
                            {
                                cardDiscountsDTOAddList = (List<CardDiscountsDTO>)type.GetMethod("GetCardDiscountsDTOList").Invoke(cardDiscountsDataHandler, new object[] { cardDiscountsSearchParam });
                            }
                            else
                                throw new Exception("Unable to fetch Card Discounts DataHandler class from assembly");
                            // List<CardDiscountsDTO> cardDiscountsDTOAddList = cardDiscountsDataHandler.GetCardDiscountsDTOList(cardDiscountsSearchParam);
                            if (cardDiscountsDTOAddList != null && cardDiscountsDTOAddList.Count > 0)
                                cardCoreDTO.CardDiscountsDTOList.AddRange(cardDiscountsDTOAddList);
                            cardDiscountsSearchParam.Clear();
                            
                        }
                        cardCoreDTOList.Add(cardCoreDTO);
                    }

                }
                log.Debug("Ends-GetAllCardsList(searchParameters) Method.");
                return cardCoreDTOList;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                log.Error(expn);
                throw new System.Exception("At GetAllCustomersList  " + expn.Message.ToString());
            }
        }

        private CustomerFingerPrintDTO GetCustomerFingerprintDto(int cardId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(cardId, sqlTrx);
            CustomerFingerPrintDTO customerFingerPrintDTO = null;
            try
            {
                string cardQuery = @"select * from CustomerFingerPrint 
                                               where CardId = @cardId 
                                                and ActiveFlag = '1'";

                SqlParameter[] cardParameters = new SqlParameter[1];
                cardParameters[0] = new SqlParameter("@cardId", cardId);

                DataTable cardTable = dataAccessHandler.executeSelectQuery(cardQuery, cardParameters, sqlTrx);
                if (cardTable.Rows.Count > 0)
                {
                    foreach (DataRow drCard in cardTable.Rows)
                    {
                        customerFingerPrintDTO = new CustomerFingerPrintDTO(
                            Convert.ToInt32(drCard["Id"].ToString()),
                            drCard["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(drCard["CardId"].ToString()),
                            drCard["Template"].ToString(),
                            drCard["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(drCard["ActiveFlag"].ToString()),
                            drCard["Source"].ToString(),
                            drCard["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(drCard["LastUpdatedDate"].ToString()),
                            drCard["LastUpdatedBy"].ToString(),
                            drCard["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(drCard["site_id"].ToString()),
                            drCard["Guid"].ToString(),
                            drCard["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(drCard["SynchStatus"].ToString()),
                            drCard["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(drCard["MasterEntityId"].ToString())
                            );
                    }
                }
                log.Debug("Ends-GetCustomerFingerprintDto(int cardId).");
                return customerFingerPrintDTO;
            }
            catch
            {
                throw;
            }

        }

               
        /// <summary>
        /// GetParentChildCardInfo(int customerId, ref int parentCardId) method
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="parentCardId">parentCardId</param>
        /// <returns>returns list of CardCoreDTO</returns>
        public List<CardCoreDTO> GetParentChildCardInfo(int customerId, ref int parentCardId)
        {
            log.Debug("Begins-GetChildCardInfo(string parentCardId).");

            try
            {
                parentCardId = GetParentCard(customerId);

                List<CardCoreDTO> cardDTOList = new List<CardCoreDTO>();
                string cardQuery = @" select CASE WHEN card_id=@parentCardId THEN 0 ELSE 1 END as orderby ,* 
								        from cards   where customer_id=@customerId 
                                                and valid_flag = 'Y'
                                                and refund_flag = 'N'
                                                and card_number not like 'T%'
                                       union
								            select CASE WHEN card_id=@parentCardId THEN 0 ELSE 1 END as orderby ,* 
										    from cards   where  card_id =@parentCardId 
										    and valid_flag = 'Y' 
                                            and refund_flag = 'N'
                                            and card_number not like 'T%'

                                       union
                                            select CASE WHEN card_id=@parentCardId THEN 0 ELSE 1 END as orderby ,*  
										    from cards where card_id in
                                            (  
                                                (select  ChildCardId as cardId from ParentChildCards where  ParentCardId =@parentCardId)  
                                                 Union
                                                (select   ParentCardId as cardId from ParentChildCards where childcardId =@parentCardId)
                                            ) 
                                            and valid_flag = 'Y' 
                                            and refund_flag = 'N'
                                            and card_number not like 'T%'
                                        ";

                SqlParameter[] cardParameters = new SqlParameter[2];
                cardParameters[0] = new SqlParameter("@customerId", customerId);
                cardParameters[1] = new SqlParameter("@parentCardId", parentCardId);

                DataTable cardTable = dataAccessHandler.executeSelectQuery(cardQuery, cardParameters);

                foreach (DataRow cardRow in cardTable.Rows)
                {
                    CardCoreDTO cardCoreDTO = GetCardCoreDTO(cardRow);
                    cardDTOList.Add(cardCoreDTO);
                }

                log.Debug("Ends-GetChildCardInfo(string parentCardId).");
                return cardDTOList;
            }
            catch
            {
                throw;
            }

        }



        /// <summary>
        /// GetParentCard(int customerId)
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>return parent card id</returns>
        public int GetParentCard(int customerId)
        {

            log.Debug("Begins-GetParentCard(int customerId).");
            int parentCardId = -1;
            try
            {
                string cardQuery = @" select * from ParentChildCards where ParentCardId in 
                                        (select card_id from cards where customer_id=@customerId)
                                        or
                                        ChildCardId in 
                                        (select card_id from cards where customer_id=@customerId) ";

                SqlParameter[] cardParameters = new SqlParameter[1];
                cardParameters[0] = new SqlParameter("@customerId", customerId);

                DataTable cardTable = dataAccessHandler.executeSelectQuery(cardQuery, cardParameters);
                foreach (DataRow drCard in cardTable.Rows)
                {
                        int.TryParse(drCard["ParentCardId"].ToString(), out parentCardId);
                }
                log.Debug("Ends-GetParentCard(int customerId).");
                return parentCardId;
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        ///  Redeem Balance from card
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="ticketCount"></param>
        /// <returns></returns>
        public bool RedeemBalanceFromCard(int cardId, int ticketCount, SqlTransaction SQLTrx)
        {

            log.Debug("Begins-RedeemBalanceFromCard(int cardId, int ticketCount).");
            try
            {
                if (ticketCount > 0)
                {
                    string cardQuery = @"update cards set ticket_count = (ticket_count - @ticket_count), last_update_time=getdate(), LastUpdatedBy='Semnox' 
                                            where card_id = @card_id ";

                    SqlParameter[] cardParameters = new SqlParameter[2];
                    cardParameters[0] = new SqlParameter("@card_id", cardId);
                    cardParameters[1] = new SqlParameter("@ticket_count", ticketCount);

                    dataAccessHandler.executeUpdateQuery(cardQuery, cardParameters, SQLTrx);
                }
                
                log.Debug("Ends-RedeemBalanceFromCard(int cardId, int ticketCount).");
                return true;
            }
            catch
            {
                throw;
            }
        }
        
        public bool DeactivateFingerprint(int cardId, string userId)
        {
            bool status = false;
            try
            {
                string cardFingerPrintUpdateQuery = @"update CustomerFingerPrint 
                                                     set ActiveFlag = 'False', 
                                                     LastUpdatedDate = getdate(), 
                                                     LastUpdatedBy = @userId 
                                            where CardId = @card_id ";

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@userId", userId);
                parameters[1] = new SqlParameter("@card_id", cardId);

                int rowsEffected = dataAccessHandler.executeUpdateQuery(cardFingerPrintUpdateQuery, parameters);
                if (rowsEffected != 0)
                    status = true;
            }
            catch
            {
                throw;
            }
            return status;
        }


        /// <summary>
        /// UpdateCardIdentifier
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="cardIdentifier"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateCardIdentifier(int cardId, string cardIdentifier, string userId)
        {
            bool status = false;
            try
            {
                log.Debug("Starts - UpdateCardIdentifier " + cardId.ToString());

                string cardIdentifierUpdateQuery = @"update cards 
                                                     set cardIdentifier = @cardIdentifier, 
                                                     last_update_time = getdate(), 
                                                     LastUpdatedBy = @userId 
                                            where card_id = @card_id ";

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@card_id", cardId);
                parameters[1] = new SqlParameter("@userId", userId);
                parameters[2] = new SqlParameter("@cardIdentifier", cardIdentifier);

                int rowsEffected = dataAccessHandler.executeUpdateQuery(cardIdentifierUpdateQuery, parameters);
                if (rowsEffected != 0)
                    status = true;

                log.Debug("Ends - UpdateCardIdentifier " + cardId.ToString());
            }
            catch(Exception ex)
            {
                log.Log("Error - UpdateCardIdentifier " + cardId.ToString(), ex);
                throw;
            }
            return status;
        }




        /// <summary>
        /// LinkCardToCustomer
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool LinkCardToCustomer(int cardId, int customerId, string userId, SqlTransaction sqlTrx)
        {
            bool status = false;
            try
            {
                log.Debug("Starts - LinkCardToCustomer - Card Id : " + cardId.ToString() + "| customerDTO Id :" + customerId.ToString());

                string cardLinkCustomerUpdateQuery = @"update cards 
                                                     set customer_id = @customerId, 
                                                     last_update_time = getdate(), 
                                                     LastUpdatedBy = @userId 
                                            where card_id = @card_id ";

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@card_id", cardId);
                parameters[1] = new SqlParameter("@customerId", customerId);
                parameters[2] = new SqlParameter("@userId", userId);

                int rowsEffected = dataAccessHandler.executeUpdateQuery(cardLinkCustomerUpdateQuery, parameters, sqlTrx);
                if (rowsEffected != 0)
                    status = true;

                log.Debug("Ends - LinkCardToCustomer " + cardId.ToString());
            }
            catch (Exception ex)
            {
                log.Log("Error - LinkCardToCustomer " + cardId.ToString(), ex);
                throw;
            }
            return status;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Customer Record.
        /// </summary>
        /// <param name="cardCoreDTO">CardCoreDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CardCoreDTO cardCoreDTO, string userId, int siteId)
        {
            log.LogMethodEntry(cardCoreDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>(); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", cardCoreDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardNumber", cardCoreDTO.Card_number));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IssueDate", cardCoreDTO.Issue_date));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FaceValue", cardCoreDTO.Face_value));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RefundFlag", cardCoreDTO.Refund_flag));
            if (cardCoreDTO.Refund_amount == -1)
            {
                cardCoreDTO.Refund_amount = 0;
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@RefundAmount", cardCoreDTO.Refund_amount));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@RefundDate", cardCoreDTO.Refund_date));
            if (cardCoreDTO.Refund_date != null && cardCoreDTO.Refund_date == DateTime.MinValue)
                parameters.Add(new SqlParameter("@RefundDate", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@RefundDate", cardCoreDTO.Refund_date));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidFlag", cardCoreDTO.Valid_flag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketCount", cardCoreDTO.Ticket_count));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Notes", cardCoreDTO.Notes));
            if (cardCoreDTO.Last_update_time != null && cardCoreDTO.Last_update_time == DateTime.MinValue.ToString())
                parameters.Add(new SqlParameter("@LastUpdateTime", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@LastUpdateTime", cardCoreDTO.Last_update_time));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdateTime", cardCoreDTO.Last_update_time));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Credits", cardCoreDTO.Credits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Courtesy", cardCoreDTO.Courtesy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Bonus", cardCoreDTO.Bonus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Time", cardCoreDTO.Time));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", cardCoreDTO.Customer_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditsPlayed", cardCoreDTO.Credits_played));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketAllowed", cardCoreDTO.Ticket_allowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RealTicketMode", cardCoreDTO.Real_ticket_mode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VipCustomer", cardCoreDTO.Vip_customer));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Site_Id", siteId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@StartTime", cardCoreDTO.Start_time));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@LastPlayedTime", cardCoreDTO.Last_played_time));
            if (cardCoreDTO.Start_time != null && cardCoreDTO.Start_time == DateTime.MinValue)
                parameters.Add(new SqlParameter("@StartTime", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@StartTime", cardCoreDTO.Start_time));
            if (cardCoreDTO.Last_played_time != null && cardCoreDTO.Last_played_time == DateTime.MinValue)
                parameters.Add(new SqlParameter("@LastPlayedTime", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@LastPlayedTime", cardCoreDTO.Last_played_time));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TechnicianCard", cardCoreDTO.Technician_card));
            if (cardCoreDTO.Tech_games == -1)
            {
                parameters.Add(new SqlParameter("@TechGames", DBNull.Value));
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@TechGames", cardCoreDTO.Tech_games));
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimerResetCard", cardCoreDTO.Timer_reset_card));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyPoints", cardCoreDTO.Loyalty_points));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardTypeId", cardCoreDTO.CardTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UploadSiteId", cardCoreDTO.Upload_site_id, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@UploadTime", cardCoreDTO.Upload_time));
            if (cardCoreDTO.Upload_time != null && cardCoreDTO.Upload_time == DateTime.MinValue)
                parameters.Add(new SqlParameter("@UploadTime", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@UploadTime", cardCoreDTO.Upload_time));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", cardCoreDTO.SynchStatus));
            if (cardCoreDTO.SynchStatus)
                parameters.Add(new SqlParameter("@SynchStatus", cardCoreDTO.SynchStatus));
            else
                parameters.Add(new SqlParameter("@SynchStatus", DBNull.Value)); 
            //parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", cardCoreDTO.ExpiryDate));
            if (cardCoreDTO.ExpiryDate != null && cardCoreDTO.ExpiryDate == DateTime.MinValue)
                parameters.Add(new SqlParameter("@ExpiryDate", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@ExpiryDate", cardCoreDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DownloadBatchId", cardCoreDTO.DownloadBatchId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@RefreshFromHQTime", cardCoreDTO.RefreshFromHQTime));
            if (cardCoreDTO.RefreshFromHQTime != null && cardCoreDTO.RefreshFromHQTime == DateTime.MinValue)
                parameters.Add(new SqlParameter("@RefreshFromHQTime", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@RefreshFromHQTime", cardCoreDTO.RefreshFromHQTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", cardCoreDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardIdentifier", cardCoreDTO.CardIdentifier));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PrimaryCard", cardCoreDTO.PrimaryCard)); 
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Cards record to the database
        /// </summary>
        /// <param name="cardCoreDTO">CardCoreDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertCards(CardCoreDTO cardCoreDTO, string userId, int siteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(cardCoreDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO Cards 
                                        ( 
                                            card_number ,
                                            issue_date ,
                                            face_value ,
                                            refund_flag ,
                                            refund_amount ,
                                            refund_date, 
                                            valid_flag, 
                                            ticket_count, 
                                            notes, 
                                            last_update_time, 
                                            credits, 
                                            courtesy, 
                                            bonus, 
                                            time, 
                                            customer_id, 
                                            credits_played, 
                                            ticket_allowed, 
                                            real_ticket_mode, 
                                            vip_customer, 
                                            site_id, 
                                            start_time, 
                                            last_played_time, 
                                            technician_card, 
                                            tech_games, 
                                            timer_reset_card, 
                                            loyalty_points, 
                                            LastUpdatedBy, 
                                            CardTypeId, 
                                            Guid, 
                                            upload_site_id, 
                                            upload_time, 
                                           -- SynchStatus, 
                                            ExpiryDate, 
                                            DownloadBatchId, 
                                            RefreshFromHQTime, 
                                            MasterEntityId, 
                                            CardIdentifier, 
                                            PrimaryCard
                                        ) 
                                VALUES  
                                        (
                                            @CardNumber ,
                                            @IssueDate ,
                                            @FaceValue ,
                                            @RefundFlag ,
                                            @RefundAmount ,
                                            @RefundDate, 
                                            @ValidFlag, 
                                            @TicketCount, 
                                            @Notes, 
                                            GETDATE(),
                                            @Credits, 
                                            @Courtesy, 
                                            @Bonus, 
                                            @Time, 
                                            @CustomerId, 
                                            @CreditsPlayed, 
                                            @TicketAllowed, 
                                            @RealTicketMode, 
                                            @VipCustomer, 
                                            @Site_Id, 
                                            @StartTime, 
                                            @LastPlayedTime, 
                                            @TechnicianCard, 
                                            @TechGames, 
                                            @TimerResetCard, 
                                            @LoyaltyPoints, 
                                            @LastUpdatedBy, 
                                            @CardTypeId, 
                                            NEWID(), 
                                            @UploadSiteId, 
                                            @UploadTime, 
                                            --@SynchStatus, 
                                            @ExpiryDate, 
                                            @DownloadBatchId, 
                                            @RefreshFromHQTime, 
                                            @MasterEntityId, 
                                            @CardIdentifier, 
                                            @PrimaryCard
                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(cardCoreDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Updates the Cards record
        /// </summary>
        /// <param name="cardCoreDTO">CardCoreDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateCards(CardCoreDTO cardCoreDTO, string userId, int siteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(cardCoreDTO, userId, siteId, sqlTransaction);
            int rowsUpdated;
            string query = @"UPDATE Cards
                                SET card_number = @CardNumber,
                                    issue_date = @IssueDate,
                                    face_value = @FaceValue,
                                    refund_flag = @RefundFlag,
                                    refund_amount = @RefundAmount,
                                    refund_date = @RefundDate,
                                    valid_flag = @ValidFlag,  
                                    ticket_count = @TicketCount, 
                                    notes = @Notes, 
                                    last_update_time = GETDATE(),
                                    credits = @Credits, 
                                    courtesy = @Courtesy, 
                                    bonus = @Bonus,
                                    time =  @Time, 
                                    customer_id = @CustomerId, 
                                    credits_played = @CreditsPlayed, 
                                    ticket_allowed = @TicketAllowed, 
                                    real_ticket_mode =  @RealTicketMode, 
                                    vip_customer = @VipCustomer, 
                                    start_time = @StartTime,
                                    last_played_time = @LastPlayedTime,  
                                    technician_card =  @TechnicianCard, 
                                    tech_games = @TechGames, 
                                    timer_reset_card = @TimerResetCard, 
                                    loyalty_points = @LoyaltyPoints, 
                                    LastUpdatedBy = @LastUpdatedBy,  
                                    CardTypeId = @CardTypeId,  
                                    upload_site_id = @UploadSiteId, 
                                    upload_time = @UploadTime, 
                                   -- SynchStatus = @SynchStatus, 
                                    ExpiryDate = @ExpiryDate, 
                                    DownloadBatchId = @DownloadBatchId, 
                                    RefreshFromHQTime = @RefreshFromHQTime, 
                                    MasterEntityId = @MasterEntityId, 
                                    CardIdentifier = @CardIdentifier, 
                                    PrimaryCard = @PrimaryCard
                             WHERE card_id = @CardId";
            try
            {
                List<SqlParameter> sqlParamList = GetSQLParameters(cardCoreDTO, userId, siteId);
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, sqlParamList.ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }


    }
}

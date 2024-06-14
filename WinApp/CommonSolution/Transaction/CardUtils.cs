/********************************************************************************************
 * Project Name - Transaction
 * Description  - CardUtils class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0      12-Dec-2021    Guru S A            Booking execute process performance fixes
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using Semnox.Parafait;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Data.SqlClient;
using System.Data;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.Transaction
{
    public class CardUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities utils;
        DBUtils dbUtils;
        ParafaitEnv env;
        MessageUtils msgUtils;
        /// <summary>
        /// CardUtils
        /// </summary>
        /// <param name="utils"></param>
        public CardUtils(Utilities utils)
        {
            log.LogMethodEntry(utils);

            this.utils = utils;
            this.dbUtils = utils.DBUtilities;
            env = utils.ParafaitEnv; //new ParafaitEnv(dbUtils);
            msgUtils = new MessageUtils(dbUtils);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// refreshRequiredFromHQ
        /// </summary>
        /// <param name="CurrentCard"></param>
        /// <returns></returns>
        public bool refreshRequiredFromHQ(Card CurrentCard)
        {
            log.LogMethodEntry(CurrentCard);

            if (CurrentCard.siteId > 0 && CurrentCard.siteId != env.SiteId) // roaming card
            {
                if (env.ALLOW_ROAMING_CARDS == "N")
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    if (env.RoamingSitesArray.Contains(CurrentCard.siteId)) // auto roaming zone
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                    else if (env.ENABLE_ON_DEMAND_ROAMING == "Y" && env.AUTOMATIC_ON_DEMAND_ROAMING == "Y")
                    {
                        int refreshFromHQThreshold;
                        try
                        {
                            refreshFromHQThreshold = -1 * Convert.ToInt32(utils.getParafaitDefaults("ROAMING_CARD_HQ_REFRESH_THRESHOLD"));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Unable to get ROAMING_CARD_HQ_REFRESH_THRESHOLD", ex);
                            refreshFromHQThreshold = -60;
                        }
                        if (CurrentCard.RefreshFromHQTime > ServerDateTime.Now.AddMinutes(refreshFromHQThreshold))
                        {
                            log.LogMethodExit(false);
                            return false;

                        }
                        else
                        {
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                    else
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                }
            }
            else if (CurrentCard.CardStatus == "NEW") // card not in local DB. card could be NEW or roaming from other site. 17-Mar-2016
            {
                if (CurrentCard.ReaderDevice == null || !CurrentCard.ReaderDevice.isRoamingCard) // it was an unissued card. nothing to do.
                {
                    log.LogMethodExit(false);
                    return false;
                }
                else // card is roaming from other (non auto-roam) sites
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// getCardFromHQ
        /// </summary> 
        public bool getCardFromHQ(RemotingClient remotingClient, ref Card CurrentCard, ref string Message)
        {
            log.LogMethodEntry(remotingClient, CurrentCard, Message);

            if (CurrentCard.siteId > 0 && CurrentCard.siteId != env.SiteId) // roaming card
            {
                if (env.ALLOW_ROAMING_CARDS == "N")
                {
                    Message = msgUtils.getMessage(133);

                    log.LogVariableState("CurrentCard ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    if (env.RoamingSitesArray.Contains(CurrentCard.siteId)) // auto roaming zone
                    {
                        log.LogVariableState("CurrentCard ", CurrentCard);
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(true);
                        return true;
                    }
                    else if (env.ENABLE_ON_DEMAND_ROAMING == "Y" && env.AUTOMATIC_ON_DEMAND_ROAMING == "Y")
                    {
                        int refreshFromHQThreshold;
                        try
                        {
                            refreshFromHQThreshold = -1 * Convert.ToInt32(utils.getParafaitDefaults("ROAMING_CARD_HQ_REFRESH_THRESHOLD"));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Unable to get ROAMING_CARD_HQ_REFRESH_THRESHOLD", ex);
                            refreshFromHQThreshold = -60;
                        }
                        if (CurrentCard.RefreshFromHQTime > ServerDateTime.Now.AddMinutes(refreshFromHQThreshold))
                        {
                            log.LogMethodExit(true);
                            return true;
                        }
                        else
                        {
                            log.LogMethodExit();
                            return refreshCardFromHQ(remotingClient, ref CurrentCard, ref Message);
                        }
                    }
                    else
                    {
                        Message = msgUtils.getMessage(196, CurrentCard.siteId);
                        log.LogVariableState("CurrentCard ", CurrentCard);
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            else if (CurrentCard.CardStatus == "NEW") // card not in local DB. card could be NEW or roaming from other site. 17-Mar-2016
            {
                if (CurrentCard.ReaderDevice == null || !CurrentCard.ReaderDevice.isRoamingCard)
                {
                    log.LogVariableState("CurrentCard ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
                // card is roaming from other (non auto-roam) sites
                else if (env.ENABLE_ON_DEMAND_ROAMING == "Y" && env.AUTOMATIC_ON_DEMAND_ROAMING == "Y")
                {
                    int refreshFromHQThreshold;
                    try
                    {
                        refreshFromHQThreshold = -1 * Convert.ToInt32(utils.getParafaitDefaults("ROAMING_CARD_HQ_REFRESH_THRESHOLD"));
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unable to get ROAMING_CARD_HQ_REFRESH_THRESHOLD", ex);
                        refreshFromHQThreshold = -60;
                    }
                    if (CurrentCard.RefreshFromHQTime > ServerDateTime.Now.AddMinutes(refreshFromHQThreshold))
                    {
                        log.LogVariableState("CurrentCard ", CurrentCard);
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        log.LogVariableState("CurrentCard ", CurrentCard);
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit("RefreshCardFromHQ called");
                        return refreshCardFromHQ(remotingClient, ref CurrentCard, ref Message);
                    }
                }
                else
                {
                    log.LogVariableState("CurrentCard ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else
            {
                log.LogVariableState("CurrentCard ", CurrentCard);
                log.LogVariableState("Message ", Message);
                log.LogMethodExit(true);
                return true;
            }
        }

        /// <summary>
        /// Method to pull card information when manual consolidated view is required.
        /// </summary>
        /// <param name="remotingClient"></param>
        /// <param name="CurrentCard"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool getCardFromHQForced(RemotingClient remotingClient, ref Card CurrentCard, ref string Message)
        {
            log.LogMethodEntry(remotingClient, CurrentCard, Message);
            if (CurrentCard.siteId > 0 && CurrentCard.siteId != env.SiteId) // roaming card
            {
                if (env.ALLOW_ROAMING_CARDS == "N")
                {
                    Message = msgUtils.getMessage(133);

                    log.LogVariableState("CurrentCard ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    if (env.RoamingSitesArray.Contains(CurrentCard.siteId)) // auto roaming zone
                    {
                        log.LogVariableState("CurrentCard ", CurrentCard);
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(true);
                        return true;
                    }
                    else if (env.ENABLE_ON_DEMAND_ROAMING == "Y")
                    {
                        int refreshFromHQThreshold;
                        try
                        {
                            refreshFromHQThreshold = -1 * Convert.ToInt32(utils.getParafaitDefaults("ROAMING_CARD_HQ_REFRESH_THRESHOLD"));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Unable to get ROAMING_CARD_HQ_REFRESH_THRESHOLD", ex);
                            refreshFromHQThreshold = -60;
                        }
                        if (CurrentCard.RefreshFromHQTime > ServerDateTime.Now.AddMinutes(refreshFromHQThreshold))
                        {
                            log.LogMethodExit(true);
                            return true;
                        }
                        else
                        {
                            log.LogMethodExit();
                            return refreshCardFromHQ(remotingClient, ref CurrentCard, ref Message);
                        }
                    }
                    else
                    {
                        Message = msgUtils.getMessage(196, CurrentCard.siteId);
                        log.LogVariableState("CurrentCard ", CurrentCard);
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            else if (CurrentCard.CardStatus == "NEW") // card not in local DB. card could be NEW or roaming from other site. 17-Mar-2016
            {
                if (CurrentCard.ReaderDevice == null || !CurrentCard.ReaderDevice.isRoamingCard)
                {
                    log.LogVariableState("CurrentCard ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
                // card is roaming from other (non auto-roam) sites
                else if (env.ENABLE_ON_DEMAND_ROAMING == "Y")
                {
                    int refreshFromHQThreshold;
                    try
                    {
                        refreshFromHQThreshold = -1 * Convert.ToInt32(utils.getParafaitDefaults("ROAMING_CARD_HQ_REFRESH_THRESHOLD"));
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unable to get ROAMING_CARD_HQ_REFRESH_THRESHOLD", ex);
                        refreshFromHQThreshold = -60;
                    }
                    if (CurrentCard.RefreshFromHQTime > ServerDateTime.Now.AddMinutes(refreshFromHQThreshold))
                    {
                        log.LogVariableState("CurrentCard ", CurrentCard);
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        log.LogVariableState("CurrentCard ", CurrentCard);
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit("RefreshCardFromHQ called");
                        return refreshCardFromHQ(remotingClient, ref CurrentCard, ref Message);
                    }
                }
                else
                {
                    log.LogVariableState("CurrentCard ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else
            {
                log.LogVariableState("CurrentCard ", CurrentCard);
                log.LogVariableState("Message ", Message);
                log.LogMethodExit(true);
                return true;
            }
        }

        bool refreshCardFromHQ(RemotingClient remotingClient, ref Card CurrentCard, ref string Message)
        {
            log.LogMethodEntry(remotingClient, CurrentCard, Message);

            if (remotingClient != null)
            {
                if (remotingClient.GetServerCard(CurrentCard.CardNumber, env.SiteId, ref Message) == "NOTFOUND")
                {
                    Message = msgUtils.getMessage(283);

                    log.LogVariableState("Current Card ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
                else if (Message == "SUCCESS")
                {
                    if (CurrentCard.isMifare)
                    {
                        CurrentCard = new MifareCard(CurrentCard.ReaderDevice, CurrentCard.CardNumber, env.LoginID, this.utils);
                    }
                    else
                    {
                        CurrentCard = new Card(CurrentCard.ReaderDevice, CurrentCard.CardNumber, env.LoginID, this.utils);
                    }

                    log.LogVariableState("Current Card ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogVariableState("Current Card ", CurrentCard);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                Message = utils.MessageUtils.getMessage(285);
                log.LogVariableState("Current Card ", CurrentCard);
                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// GetCardList
        /// </summary> 
        /// <returns></returns>
        public List<Card> GetCardList(List<string> cardNumberList, string ploginId, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();
            List<Card> cardList = new List<Card>();
            int bonusDays = 0;
            string CommandText = string.Empty;
            if (utils.ParafaitEnv.REACTIVATE_EXPIRED_CARD)
            {

                bonusDays = ParafaitDefaultContainerList.GetParafaitDefault<int>(utils.ExecutionContext, "CARD_EXPIRY_GRACE_PERIOD", 0);
            }
            if (cardNumberList != null && cardNumberList.Any())
            {
                cardNumberList = cardNumberList.Distinct().ToList();
            }

            CommandText = @"select c.*, getdate() as sysdate  
                                                          from CardView c left outer join Membership m on m.membershipId = c.membershipId,
                                                               @cardNumberList list
                                                        where card_number = list.Value
                                                          and (valid_flag = 'Y' and (ExpiryDate is null or ExpiryDate > getdate())
                                                                or (@reactivateExpired = 1 
                                                                    and Refund_flag = 'N'
                                                                    and case when  @ExpireAfterMonths = -1  then DATEADD(day,@bonusdays*-1,ExpiryDate)  else ExpiryDate  end < getdate()
                                                                    and card_id = (select max(card_id) 
                                                                                     from cards ce 
                                                                                    where ce.card_number = c.card_number)))
                                                        order by issue_date desc";

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@bonusdays", bonusDays));
            sqlParameters.Add(new SqlParameter("@ExpireAfterMonths", (utils.ParafaitEnv.CARD_EXPIRY_RULE == "ISSUEDATE" ? utils.ParafaitEnv.CARD_VALIDITY : -1)));
            sqlParameters.Add(new SqlParameter("@reactivateExpired", utils.ParafaitEnv.REACTIVATE_EXPIRED_CARD));
            DataAccessHandler dataAccessHandler = new DataAccessHandler();
            DataTable DT = dataAccessHandler.BatchSelect(CommandText, "@cardNumberList", cardNumberList, sqlParameters.ToArray(), sqlTrx);
            for (int i = 0; i < cardNumberList.Count; i++)
            {
                List<DataRow> dataRows = DT.Select("card_number = '" + cardNumberList[i] + "'", "ExpiryDate desc").ToList();
                Card card = new Card(utils);
                card.loginId = ploginId;
                card = SetCardAttributes(utils.ExecutionContext, cardNumberList[i], card, dataRows);
                cardList.Add(card);
            }
            AddCustomerDetails(cardList, DT, sqlTrx);
            cardList = AddCreditPlusTimeDetails(cardList, sqlTrx);
            log.LogMethodExit();
            return cardList;
        }

        private Card SetCardAttributes(ExecutionContext executionContext, string cardNumber, Card card, List<DataRow> dataRowList )
        {
            log.LogMethodEntry(executionContext, cardNumber, dataRowList);
            if (dataRowList.Count == 0)
            {
                card.CardNumber = cardNumber;
                card.CardStatus = "NEW";
                card.issue_date = ServerDateTime.Now;
            }
            else
            {
                if (dataRowList[0]["ExpiryDate"] != DBNull.Value)
                    card.ExpiryDate = (DateTime)dataRowList[0]["ExpiryDate"];
                else
                    card.ExpiryDate = DateTime.MinValue;

                if (card.ExpiryDate.Equals(DateTime.MinValue) || card.ExpiryDate > Convert.ToDateTime(dataRowList[0]["sysdate"]))
                    card.CardStatus = "ISSUED";
                else
                    card.CardStatus = "EXPIRED";

                card.CardNumber = dataRowList[0]["card_number"].ToString();
                card.card_id = Convert.ToInt32(dataRowList[0]["card_id"]);

                if (dataRowList[0]["site_id"] != DBNull.Value)
                    card.siteId = Convert.ToInt32(dataRowList[0]["site_id"]);
                else
                    card.siteId = -1;

                card.issue_date = (DateTime)dataRowList[0]["issue_date"];

                if (dataRowList[0]["face_value"] != DBNull.Value)
                    card.face_value = (float)Convert.ToDouble(dataRowList[0]["face_value"]);

                card.refund_flag = dataRowList[0]["refund_flag"].ToString()[0];
                if (dataRowList[0]["refund_amount"] == DBNull.Value)
                    card.refund_amount = -1;
                else
                    card.refund_amount = (float)Convert.ToDouble(dataRowList[0]["refund_amount"]);
                if (dataRowList[0]["refund_date"] == DBNull.Value)
                    card.refund_date = DateTime.MinValue;
                else
                    card.refund_date = (DateTime)dataRowList[0]["refund_date"];

                card.valid_flag = (char)dataRowList[0]["valid_flag"].ToString()[0];
                card.real_ticket_mode = (char)dataRowList[0]["real_ticket_mode"].ToString()[0];
                card.vip_customer = (char)dataRowList[0]["vip_customer"].ToString()[0];
                card.ticket_allowed = (char)dataRowList[0]["ticket_allowed"].ToString()[0];

                card.technician_card = (dataRowList[0]["technician_card"] == DBNull.Value ? 'N' : dataRowList[0]["technician_card"].ToString()[0]);

                if (dataRowList[0]["ticket_count"] != DBNull.Value)
                    card.ticket_count = Convert.ToInt32(dataRowList[0]["ticket_count"]);

                card.notes = dataRowList[0]["notes"].ToString();

                if (dataRowList[0]["last_update_time"] != DBNull.Value)
                    card.last_update_time = (DateTime)dataRowList[0]["last_update_time"];
                else
                    card.last_update_time = DateTime.MinValue;

                if (dataRowList[0]["last_played_time"] != DBNull.Value)
                    card.last_played_time = (DateTime)dataRowList[0]["last_played_time"];
                else
                    card.last_played_time = DateTime.MinValue;

                if (dataRowList[0]["start_time"] != DBNull.Value)
                    card.start_time = (DateTime)dataRowList[0]["start_time"];
                else
                    card.start_time = DateTime.MinValue;

                if (dataRowList[0]["RefreshFromHQTime"] != DBNull.Value)
                    card.RefreshFromHQTime = (DateTime)dataRowList[0]["RefreshFromHQTime"];
                else
                    card.RefreshFromHQTime = DateTime.MinValue;

                if (dataRowList[0]["credits"] != DBNull.Value)
                    card.credits = Convert.ToDouble(dataRowList[0]["credits"]);

                if (dataRowList[0]["courtesy"] != DBNull.Value)
                    card.courtesy = Convert.ToDouble(dataRowList[0]["courtesy"]);

                if (dataRowList[0]["bonus"] != DBNull.Value)
                    card.bonus = Convert.ToDouble(dataRowList[0]["bonus"]);

                if (dataRowList[0]["time"] != DBNull.Value)
                {
                    card.time = Convert.ToDouble(dataRowList[0]["time"]);
                    if (card.time > 0)
                    {
                        if (card.start_time != DateTime.MinValue)
                        {
                            TimeSpan ts = card.start_time.AddMinutes(card.time) - Convert.ToDateTime(dataRowList[0]["sysdate"]);

                            double balanceTime = ts.TotalSeconds;
                            if (balanceTime <= 0)
                                card.time = 0;
                            else
                                card.time = Math.Floor(balanceTime / 60) + (balanceTime % 60) / 100;
                        }
                    }
                }

                if (dataRowList[0]["guid"] == DBNull.Value)
                    card.SetCardGuid = "";
                else
                    card.SetCardGuid = Convert.ToString(dataRowList[0]["guid"]);

                log.LogVariableState("@card_id", card.card_id);

                if (dataRowList[0]["customer_id"] == DBNull.Value)
                    card.customer_id = -1;
                else
                {
                    card.customer_id = Convert.ToInt32(dataRowList[0]["customer_id"]);
                }

                if (dataRowList[0]["credits_played"] != DBNull.Value)
                    card.credits_played = Convert.ToDouble(dataRowList[0]["credits_played"]);

                if (dataRowList[0]["loyalty_points"] != DBNull.Value)
                    card.loyalty_points = Convert.ToDouble(dataRowList[0]["loyalty_points"]);

                if (dataRowList[0]["tech_games"] != DBNull.Value)
                    card.tech_games = Convert.ToInt32(dataRowList[0]["tech_games"]);

                if (dataRowList[0]["MembershipId"] != DBNull.Value)
                {
                    card.MembershipId = Convert.ToInt32(dataRowList[0]["MembershipId"]);
                    card.MembershipName = dataRowList[0]["MembershipName"].ToString();
                }
                else
                {
                    card.MembershipId = -1;
                    card.MembershipName = MessageContainerList.GetMessage(executionContext, "Normal");
                }

                if (dataRowList[0]["PrimaryCard"] != DBNull.Value)
                    card.primaryCard = dataRowList[0]["PrimaryCard"].ToString();

                card.TotalCreditPlusLoyaltyPoints = Convert.ToDouble(dataRowList[0]["CreditPlusLoyaltyPoints"]);
                card.RedeemableCreditPlusLoyaltyPoints = Convert.ToDouble(dataRowList[0]["RedeemableCreditPlusLoyaltyPoints"]);
                card.CreditPlusVirtualPoints = Convert.ToDouble(dataRowList[0]["CreditPlusVirtualPoints"]);
                card.CreditPlusTickets = Convert.ToInt32(dataRowList[0]["CreditPlusTickets"]);
                card.CreditPlusCardBalance = Convert.ToDouble(dataRowList[0]["CreditPlusCardBalance"] == DBNull.Value ? 0 : dataRowList[0]["CreditPlusCardBalance"]);
                card.CreditPlusCredits = Convert.ToDouble(dataRowList[0]["CreditPlusCredits"]);
                card.CreditPlusBonus = Convert.ToDouble(dataRowList[0]["CreditPlusBonus"]);
                card.creditPlusItemPurchase = Convert.ToDouble(dataRowList[0]["creditPlusItemPurchase"]);
            }
            log.LogMethodExit();
            return card;
        }
        private void AddCustomerDetails(List<Card> cardList, DataTable DT, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            List<DataRow> custIdRows = DT.Select("customer_id IS NOT NULL").ToList();
            List<int> customerIdList = new List<int>();
            if (custIdRows != null && custIdRows.Any())
            {
                for (int i = 0; i < custIdRows.Count; i++)
                {
                    customerIdList.Add(Convert.ToInt32(custIdRows[i]["customer_id"]));
                }
            }
            if (customerIdList != null && customerIdList.Any())
            {
                customerIdList = customerIdList.Distinct().ToList();
                CustomerListBL customerListBL = new CustomerListBL(utils.ExecutionContext);
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerIdList, true, true, false, sqlTrx);
                if (customerDTOList != null && customerDTOList.Any())
                {
                    for (int i = 0; i < cardList.Count; i++)
                    {
                        if (cardList[i].customer_id > -1)
                        {
                            cardList[i].customerDTO = customerDTOList.Find(cust => cust.Id == cardList[i].customer_id);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private List<Card> AddCreditPlusTimeDetails(List<Card> cardList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            List<int> cardIdList = new List<int>();
            if (cardList != null && cardList.Any())
            {
                for (int i = 0; i < cardList.Count; i++)
                {
                    if (cardList[i].card_id > -1)
                        cardIdList.Add(cardList[i].card_id);
                }
            }
            if (cardIdList != null && cardIdList.Any())
            {
                cardIdList = cardIdList.Distinct().ToList();
                string CommandText = @"select CardId, isnull(sum(TimeBalance), 0) as timeBalance  
                                           from (select cp.card_id as CardId, case when PlayStartTime is null 
                                                        then CreditPlusBalance 
                                                     else case when TimeTo is null 
                                                      then datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime))
                                                      else datediff(MI, getdate(), 
                                                   (select min(endTime) 
                                                    from (select DATEADD(MI, (timeto - cast(timeto as integer))*100, DateAdd(HH, case timeto when 0 then 24 else timeto end, dateadd(D, 0, datediff(D, 0, getdate())))) endTime 
                                                      union all 
                                                       select dateadd(MI, CreditPlusBalance, PlayStartTime)) as v)) 
                                                      end
                                                     end TimeBalance
                                                from CardCreditPlus cp, @cardIdList List
                                                where cp.card_id = List.Id
                                                  AND isnull(validityStatus, 'Y') != 'H' 
                                                  and CreditPlusType in ('M')
                                                  and (case when PlayStartTime is null then getdate() else dateadd(MI, CreditPlusBalance, PlayStartTime) end) >= getdate()
                                                  and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                                  and (cp.PeriodTo is null or cp.PeriodTo > getdate())) v 
                                               group by v.CardId";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                DataTable DT = dataAccessHandler.BatchSelect(CommandText, "@cardIdList", cardIdList, sqlParameters.ToArray(), sqlTrx);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    int index = cardList.FindIndex(c => c.card_id == Convert.ToInt32(DT.Rows[i]["CardId"]));
                    if (index > -1)
                    {
                        cardList[index].CreditPlusTime = Convert.ToInt32(DT.Rows[i]["timeBalance"]);
                    }
                }
            }
            log.LogMethodExit();
            return cardList;
        }
    }
}

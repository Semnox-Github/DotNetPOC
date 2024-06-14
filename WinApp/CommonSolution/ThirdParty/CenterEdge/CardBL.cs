/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CardBL class - This is business layer class for Cards
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 2.140.8     03-Oct-2023       Abhishek                  Modified : In Get card details, 
                                                         return the product id privilege product. 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{


    /// <summary>
    /// This class manages the centerEdge card details
    /// </summary>
    public class CardBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AccountDTO accountDTO;

        /// <summary>
        /// Parameterized constructor of CardBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public CardBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CardBL(ExecutionContext executionContext, Card cardDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
            TagNumber tagNumber;
            if (tagNumberParser.TryParse(cardDTO.cardNumber, out tagNumber) == false)
            {
                string message = tagNumberParser.Validate(cardDTO.cardNumber);
                log.LogMethodExit(null, "Throwing Exception- " + message);
                throw new Exception(message);
            }
            AccountBL accountBL = new AccountBL(executionContext, cardDTO.cardNumber, true, true);
            if (accountBL.AccountDTO != null)
            {
                log.Debug("Card exists");
                throw new ValidationException("Card exists");
            }
            DateTime currentTime = ServerDateTime.Now;
            accountDTO = new AccountDTO(-1, cardDTO.cardNumber, "", currentTime, null, false, null, null, true,
                                        null, null, null, null, null, null, -1, null, true, true, false, null, null, "N", null,
                                        false, null, -1, null, null, -1, null, null, false, -1, "", "",
                                        currentTime, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(),
                                        currentTime);
            log.LogMethodExit(accountDTO);
        }

        public CardBL(ExecutionContext executionContext, string accountNumber, SqlTransaction sqlTransaction = null)
                 : this(executionContext)
        {
            log.LogMethodEntry(accountNumber);
            TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
            TagNumber tagNumber;
            if (tagNumberParser.TryParse(accountNumber, out tagNumber) == false)
            {
                string message = tagNumberParser.Validate(accountNumber);
                log.LogMethodExit(null, "Throwing Exception- " + message);
                throw new Exception(message);
            }
            AccountBL accountBL = new AccountBL(executionContext, accountNumber, true, true, sqlTransaction);
            if (accountBL.AccountDTO == null || accountBL.AccountDTO.ValidFlag == false)
            {
                log.Debug("Card Not Found");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Card Not Found"));
            }
            AccountBuilderBL accountBuilderBL = new AccountBuilderBL(executionContext);
            accountBuilderBL.Build(accountBL.AccountDTO, true, sqlTransaction);
            accountDTO = accountBL.AccountDTO;
            log.LogMethodExit(accountDTO);
        }


        /// <summary>
        /// This method builds the CEcard response object from semnox Account  object
        /// </summary>
        public Card GetDetails(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Card ceCardResponse = new Card();
            if (accountDTO != null)
            {
                ceCardResponse.cardNumber = accountDTO.TagNumber;
                log.Debug("accountDTO.IssueDate DB format : " + accountDTO.IssueDate);
                ceCardResponse.issuedAtTime = Convert.ToDateTime(accountDTO.IssueDate).ToUniversalTime();
                log.Debug("ceCardResponse.IssueDate UTC format : " + ceCardResponse.issuedAtTime);
                ceCardResponse.balance = GetCardBalance();
                List<TimePlays> timePlays = GetTimePlayDetails(sqlTransaction);
                List<Privilege> Privileges = GetPrivilegeDetails(sqlTransaction);
                ceCardResponse.timePlays.AddRange(timePlays);
                ceCardResponse.privileges.AddRange(Privileges);
            }
            log.LogMethodExit(ceCardResponse);
            return ceCardResponse;
        }


        /// <summary>
        /// This method builds the CenterEdge point details from account DTO
        /// </summary>
        /// <returns></returns>
        private Points GetCardBalance()
        {
            log.LogMethodEntry(accountDTO);
            Points pointTypes = new Points();
            pointTypes.bonusPoints = Convert.ToDecimal(accountDTO.TotalBonusBalance);
            pointTypes.redemptionTickets = Convert.ToDecimal(accountDTO.TotalTicketsBalance);
            pointTypes.regularPoints = Convert.ToDecimal(accountDTO.TotalCreditsBalance);
            log.LogMethodExit(pointTypes);
            return pointTypes;
        }


        /// <summary>
        /// This method builds the CenterEdge time play details from account DTO
        /// </summary>
        /// <returns></returns>
        private List<TimePlays> GetTimePlayDetails(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<TimePlays> timePlayList = new List<TimePlays>();
            AccountBL accountBL = new AccountBL(executionContext, accountDTO.AccountId, true, true, sqlTransaction);
            if (accountBL.CardHasCreditPlus(CreditPlusType.TIME))
            {
                List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountDTO.AccountCreditPlusDTOList.Where(x => x.CreditPlusType == CreditPlusType.TIME && x.CreditPlusBalance >= 0).ToList();
                decimal creditPlusTimeBalance = Convert.ToDecimal(accountDTO.TotalTimeBalance);
                int refundProductId = -1;
                int productId = -1;
                int fromCardProductId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONSOLIDATION_TASK_FROM_CARD_PRODUCT"));
                int toCardProductId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONSOLIDATION_TASK_TO_CARD_PRODUCT"));
                TimePlayGroupBL timePlayGroupBL = new TimePlayGroupBL(executionContext);
                TimePlayGroups timePlayGroups = timePlayGroupBL.GetTimePlayGroups(0, -1);
                if (timePlayGroups.timePlayGroups != null && timePlayGroups.timePlayGroups.Any())
                {
                    productId = timePlayGroups.timePlayGroups.FirstOrDefault().id;
                    log.Debug("productId : " + productId);
                }
                foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountCreditPlusDTOList)
                {
                    TimePlays timePlays = new TimePlays();
                    DateTime serverTime = ServerDateTime.Now;
                    AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, accountCreditPlusDTO);
                    Transaction.TransactionBL transactionBL = new Transaction.TransactionBL(executionContext, accountCreditPlusDTO.TransactionId, sqlTransaction);
                    timePlays.type = "minutes";
                    refundProductId = GetRefundProductId();
                    timePlays.groupId = transactionBL.GetProductId(accountCreditPlusDTO.TransactionLineId, sqlTransaction);
                    if (refundProductId == timePlays.groupId)
                    {
                        timePlays.groupId = productId;
                    }
                    if (timePlays.groupId == fromCardProductId || timePlays.groupId == toCardProductId)
                    {
                        timePlays.groupId = productId;
                    }
                    DateTime? expiryTime = accountCreditPlusBL.GetCreditExpiryDate(serverTime);
                    timePlays.expirationDateTime = string.IsNullOrEmpty(expiryTime.ToString()) ? null : Convert.ToDateTime(expiryTime).ToString("yyyy-MM-ddTHH:mm:ssK");
                    timePlays.started = accountCreditPlusBL.TimePlayStarted(serverTime);
                    timePlays.minutesRemaining = Convert.ToInt32(accountCreditPlusBL.GetCreditPlusTime(serverTime));
                    // If balance is there then only show else skip
                    if (timePlays.minutesRemaining > 0)
                    {
                        timePlayList.Add(timePlays);
                    }
                }
            }
            log.LogMethodExit(timePlayList);
            return timePlayList;
        }
        public int GetRefundProductId()
        {
            log.LogMethodEntry();
            int refundProductId = -1;
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, ProductTypeValues.REFUND));
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParams);
            if (productsDTOList != null && productsDTOList.Any())
            {
                refundProductId = productsDTOList[0].ProductId;
            }
            log.LogMethodExit(refundProductId);
            return refundProductId;
        }

        /// <summary>
        /// This method builds the CenterEdge privilege details from account DTO
        /// </summary>
        /// <returns></returns>
        private List<Privilege> GetPrivilegeDetails(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<Privilege> privilegesList = new List<Privilege>();
            List<Privilege> result = new List<Privilege>();
            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
            {
                decimal gameBalance = Convert.ToDecimal(accountDTO.TotalGamesBalance);
                log.Debug("gameBalance : " + gameBalance);
                foreach (AccountGameDTO accountGameDTO in accountDTO.AccountGameDTOList)
                {
                    AccountGameBL accountGameBL = new AccountGameBL(executionContext, accountGameDTO);
                    //Transaction.TransactionBL transactionBL = new Transaction.TransactionBL(executionContext, accountGameDTO.TransactionId, sqlTransaction);
                    List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> searchParameters = new List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    if (accountGameDTO.GameId > -1)
                    {
                        searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.GAME_ID, accountGameDTO.GameId.ToString()));
                    }
                    else
                    {
                        searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.GAME_PROFILE_ID, accountGameDTO.GameProfileId.ToString()));
                    }
                    ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext);
                    List<ProductGamesDTO> productGamesDTOList = productGamesListBL.GetProductGamesDTOList(searchParameters);
                    int groupId = -1;
                    if (productGamesDTOList != null && productGamesDTOList.Any())
                    {
                        groupId = productGamesDTOList.FirstOrDefault().Product_id;
                    }
                    int gameCount = accountGameBL.GetGameBalance();
                    string expirationDateTime = string.IsNullOrEmpty(accountGameDTO.ExpiryDate.ToString()) ? null : Convert.ToDateTime(accountGameDTO.ExpiryDate.ToString()).ToString("yyyy-MM-ddTHH:mm:ssK");
                    if (expirationDateTime != null && DateTime.Parse(expirationDateTime) > ServerDateTime.Now && gameCount != 0)
                    {
                        Privilege privileges = new Privilege(groupId, gameCount, expirationDateTime);
                        privilegesList.Add(privileges);
                    }
                    else if (expirationDateTime == null && gameCount != 0)
                    {
                        Privilege privileges = new Privilege(groupId, gameCount, expirationDateTime);
                        privilegesList.Add(privileges);
                    }
                }
                foreach (Privilege privilegeDTO in privilegesList)
                {
                    if (result.Exists(x => x.groupId == privilegeDTO.groupId))
                    {
                        Privilege privileges = result.Where(x => x.groupId == privilegeDTO.groupId).FirstOrDefault();
                        privileges.count += privilegeDTO.count;
                    }
                    else
                    {
                        result.Add(privilegeDTO);
                    }
                }
                privilegesList = new List<Privilege>(result);
            }
            log.LogMethodExit(privilegesList);
            return privilegesList;
        }

        /// <summary>
        /// This method saves the CenterEdge card object into accountDTO in parafait
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
            if (tagNumberParser.IsValid(accountDTO.TagNumber) == false)
            {
                string errorMessage = tagNumberParser.Validate(accountDTO.TagNumber);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            TransactionBL transactionBL = new TransactionBL(executionContext);
            accountDTO = transactionBL.Activate(accountDTO.TagNumber, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// This method makes accountDTO invalid if exists else throws exception
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                accountDTO.ValidFlag = false;
                AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                accountBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit();
        }
    }
}

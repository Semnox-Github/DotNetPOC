/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Cards "ViewCards" entity. Created to fetch, update and insert Cards.
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        19-Feb-2019     Mushahid Faizan     Created
 *2.70        11-Sept-2019    Jagan Mohana        Renamed controller name ViewCardsController 
 *2.80        15-Oct-2019      Nitin Pai          Web Services clean up and Customer registration related changes
 *2.80        05-Apr-2020      Girish Kundar      Modified: API path change and removed token from response body
 *2.120.6     24-Feb-2022      Nitin Pai          Prevent a wild card search of accounts
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System.Threading.Tasks;
using Semnox.Parafait.User;
using Semnox.Parafait.Customer;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Customer.Accounts
{
    public class AccountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Semnox.Core.Utilities.Utilities utilities;

        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Cards Collections.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Account/Accounts")]
        public async Task<HttpResponseMessage> Get(string isActive = "Y", bool chkVipCustomer = false, bool chkRoamingCards = true, bool chkTechCards = false, DateTime? fromDate = null, DateTime? toDate = null,
                                                   int accountId = -1, int customerId = -1, string accountNumber = null, int? tagSiteId = null, int pageNumber = 0, int pageSize = 0, bool buildChildRecords = false, bool activeRecordsOnly = false,
                                                   bool buildActivityHistory = false, bool buildGamePlayHistory = false, int lastActivityHistoryId = -1, int lastGamePlayHistoryId = -1, string entitlementType = null,
                                                   int activityHistoryPageNumber = 0, int gamePlayHistoryPageNumber = 0, bool buildBarCode = false, bool includeFutureEntitlements = false, bool closedTransactionsOnly = true,
                                                   DateTime? activityFromDate = null, DateTime? activityToDate = null, string customerName = null)
        {
            try
            {
                log.LogMethodEntry(isActive, chkVipCustomer, chkRoamingCards, chkTechCards, fromDate, toDate, accountId, customerId, accountNumber,
                                   pageNumber, pageSize, customerName);

                // Prevent a blank search of accounts
                if (customerId == -1 && accountId == -1 && String.IsNullOrWhiteSpace(accountNumber) && fromDate == null && toDate == null)
                {
                    log.Fatal("Blank search encountered. Forcing an error. " + this.Request.Content);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = "Invalid Search Parameters" });
                }

                ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                AccountDTOCollection accountDTOCollection = await accountUseCases.GetAccounts(isActive, chkVipCustomer, chkRoamingCards, chkTechCards, fromDate, toDate,
                                accountId, customerId, accountNumber, tagSiteId, pageNumber, pageSize, buildChildRecords, activeRecordsOnly,
                                buildActivityHistory, buildGamePlayHistory, lastActivityHistoryId, lastGamePlayHistoryId, entitlementType,
                                activityHistoryPageNumber, gamePlayHistoryPageNumber, buildBarCode, includeFutureEntitlements, closedTransactionsOnly,
                                activityFromDate, activityToDate, customerName);
                return Request.CreateResponse(HttpStatusCode.OK, accountDTOCollection);
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Cards Collections.
        /// </summary>
        /// <param name="accountList"></param>
        [HttpPost]
        [Route("api/Customer/Account/Accounts")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<AccountDTO> accountList)
        {
            try
            {
                log.LogMethodEntry(accountList);
                ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                List<AccountDTO> result = await accountUseCases.SaveAccounts(accountList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Account.
        /// </summary>
        /// <param name="accountDTO"></param>
        [HttpPost]
        [Route("api/Customer/Account/AccountSingle")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]AccountDTO accountDTO)
        {
            try
            {
                log.LogMethodEntry(accountDTO);

                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                Users user = new Users(executionContext, executionContext.GetUserId(), accountDTO.SiteId);
                if (String.IsNullOrEmpty(user.UserDTO.LoginId))
                {
                    String message1 = "Please setup the user " + executionContext.GetUserId() + ":" + executionContext.GetSiteId();
                    log.Error(message1);
                    throw new Exception(message1);
                }
                utilities = new Utilities();
                utilities.ParafaitEnv.SiteId = accountDTO.SiteId;
                utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
                utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
                utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                utilities.ExecutionContext.SetSiteId(accountDTO.SiteId);
                utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
                utilities.ParafaitEnv.Initialize();

                String message = "";

                if (accountDTO != null)
                {
                    bool linkCustomerToCard = false;
                    AccountBL existingAccountBL = new AccountBL(utilities.ExecutionContext, accountDTO.AccountId);
                    if (accountDTO.CustomerId > -1)
                    {
                        if (existingAccountBL.AccountDTO.CustomerId == -1)
                            linkCustomerToCard = true;

                        log.Debug("link card to customer:" + linkCustomerToCard);
                    }

                    AccountBL accountBL = null;
                    if (!linkCustomerToCard)
                    {
                        accountBL = new AccountBL(utilities.ExecutionContext, accountDTO);
                        accountBL.Save(null);
                    }
                    else
                    {
                        log.Debug("check if card can be linked to customer:" + linkCustomerToCard);
                        //accountBL = new AccountBL(utilities.ExecutionContext, accountDTO.AccountIdentifier, true);
                        bool canLinkAccount = existingAccountBL.CanAccountLinkToCustomer(ref message);
                        if (canLinkAccount)
                        {
                            log.Debug("linking card to customer:" + linkCustomerToCard);
                            CustomerBL customerBL = new CustomerBL(executionContext, accountDTO.CustomerId);
                            AccountServiceDTO accountServiceDTO = new AccountServiceDTO();
                            accountServiceDTO.CustomerDTO = customerBL.CustomerDTO;

                            // take the account as the DB account
                            accountServiceDTO.SourceAccountDTO = existingAccountBL.AccountDTO;

                            if (!String.IsNullOrEmpty(accountDTO.AccountIdentifier) && String.IsNullOrEmpty(accountServiceDTO.SourceAccountDTO.AccountIdentifier) == true)
                                accountServiceDTO.SourceAccountDTO.AccountIdentifier = accountDTO.AccountIdentifier;

                            LinkAccountToCustomerBL linkAccountToCustomerBL = new LinkAccountToCustomerBL(executionContext, accountServiceDTO);
                            linkAccountToCustomerBL.LinkAccount();

                            // refresh the account
                            accountBL = new AccountBL(utilities.ExecutionContext, accountDTO);
                        }
                        else
                        {
                            log.Debug("Card cannot be lined to customer " + message);

                            string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new ValidationException("This card cannot be linked to the customer."), executionContext);
                            log.Error(customException);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                        }
                    }
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = accountBL.AccountDTO });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Delete the JSON Object Cards Collections.
        /// </summary>
        /// <param name="accountList"></param>
        [HttpDelete]
        [Route("api/Customer/Account/Accounts")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<AccountDTO> accountList)
        {
            try
            {
                log.LogMethodEntry(accountList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (accountList != null || accountList.Count != 0)
                {
                    using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                    {
                        transaction.BeginTransaction();
                        AccountListBL accountListBL = new AccountListBL(executionContext, accountList);
                        accountListBL.SaveUpdateManualChangesList(transaction.SQLTrx);
                        transaction.EndTransaction();
                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                    }

                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}

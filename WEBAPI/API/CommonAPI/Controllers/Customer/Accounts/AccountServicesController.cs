/********************************************************************************************
 * Project Name - AccountServicesController
 * Description  - Controller for Performing all card related tasks
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.60       07-May-2019      Nitin Pai      Initial Version 
 * 2.80       09-Sep-2019      Divya A        Gateway clean up and customer registration related changes  
 * 2.80       08-Apr-2020      Nitin Pai      Renaming Account Services DTO adn fixes
 * 2.130.10   08-Sep-2022      Nitin Pai      Enhanced customer activity user log table
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Parafait.Customer;
using System.Linq;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.Customer.Accounts
{
    public class AccountServicesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Account Activities such as LinkAccount, Balance Transfer, Refund, etc are 
        /// performed by this method.
        /// </summary>
        [HttpPost]
        [Route("api/Customer/Account/AccountServices")]
        public HttpResponseMessage Post([FromBody]AccountServicesDTO accountServicesDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(accountServicesDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
                utilities.ParafaitEnv.SiteId = accountServicesDTO.SourceAccountDTO.SiteId;
                utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                Users user = new Users(executionContext, executionContext.GetUserId());
                utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
                utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                utilities.ExecutionContext.SetSiteId(accountServicesDTO.SourceAccountDTO.SiteId);
                utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
                utilities.ParafaitEnv.Initialize();
                log.Debug("AccountServices:SiteId" + utilities.ParafaitEnv.SiteId);
                log.Debug("AccountServices:IsCorporate" + utilities.ParafaitEnv.IsCorporate);

                string message = "";
                if (accountServicesDTO == null || accountServicesDTO.SourceAccountDTO == null)
                {
                    message = "AccountServiceDTO cannot be null";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                }

                CustomerBL sourceCustomerBL = null;
                AccountBL sourceAccount = null;
                // validate device before modifying the account
                if ((accountServicesDTO.ServiceType == AccountServicesDTO.ActivityType.BALANCETRANSFER) ||
                    (accountServicesDTO.ServiceType == AccountServicesDTO.ActivityType.LOSTCARD))
                {
                    bool validAccount = false;
                    try
                    {
                        sourceAccount = new AccountBL(executionContext, accountServicesDTO.SourceAccountDTO.AccountId, true, true);
                        if (sourceAccount.AccountDTO != null)
                        {
                            if(!sourceAccount.AccountDTO.TagNumber.ToUpper()[0].Equals('T'))
                            {
                                sourceCustomerBL = new CustomerBL(executionContext, sourceAccount.AccountDTO.CustomerId, true);
                                if (sourceCustomerBL.CustomerDTO != null && sourceCustomerBL.CustomerDTO.ContactDTOList != null)
                                {
                                    CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, sourceAccount.AccountDTO.CustomerId, accountServicesDTO.DeviceUuid.ToString(),
                                    "ACCOUNTSERVICE", accountServicesDTO.ServiceType + " for " + sourceAccount.AccountDTO.TagNumber, ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), this.Request.Content.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.CARD),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                                    CustomerActivityUserLogBL customerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, customerActivityUserLogDTO);
                                    customerActivityUserLogBL.Save();

                                    List<ContactDTO> contacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.UUID.Equals(accountServicesDTO.DeviceUuid.ToString())
                                                                                            && x.IsActive).ToList();
                                    if (contacts != null && contacts.Any())
                                    {
                                        validAccount = true;
                                    }
                                    else
                                    {
                                        log.Debug("sent uuid" + accountServicesDTO.DeviceUuid.ToString());
                                        log.Debug("actual uuid" + string.Join("-", sourceCustomerBL.CustomerDTO.ContactDTOList[0].UUID.ToString()));
                                        validAccount = true;
                                        //message = "An attempt was made to modify your card " + performAccountActivityDTO.Value + ". If this was not you, please contact site immediately to deactivate this card.";
                                        //Messaging messaging = new Messaging(utilities);
                                        //contacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).ToList();
                                        //if (contacts.Any())
                                        //{
                                        //    messaging.SendEMail(contacts[0].Attribute1, "Account modification notification", message, -1, performAccountActivityDTO.Reference, sourceCustomerBL.CustomerDTO.Id, utilities.getServerTime());
                                        //}
                                        //contacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).ToList();
                                        //if (contacts.Any())
                                        //{
                                        //    messaging.SendSMS(contacts[0].Attribute1, message);
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                log.Debug("This operation cannot be performed on a temporary card. Please visit site.");
                                message = utilities.MessageUtils.getMessage(2370);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.LogMethodExit(message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
                    }

                    if (!validAccount)
                    {
                        log.LogMethodExit(message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                    }
                }
                TaskProcs tp = new TaskProcs(utilities);
                switch (accountServicesDTO.ServiceType)
                {
                    case AccountServicesDTO.ActivityType.BALANCETRANSFER:
                        {
                            if (accountServicesDTO.SourceAccountDTO.AccountId > 0 && accountServicesDTO.DestinationAccountDTO != null && accountServicesDTO.DestinationAccountDTO.AccountId > 0
                                && accountServicesDTO.Value > 0 && !String.IsNullOrEmpty(accountServicesDTO.EntitlementType) )
                            {
                                AccountBL desinationCardBL = new AccountBL(utilities.ExecutionContext, accountServicesDTO.DestinationAccountDTO.AccountId, true, true, null);

                                Dictionary<string, decimal> entitlements = new Dictionary<string, decimal>();
                                entitlements.Add(accountServicesDTO.EntitlementType, accountServicesDTO.Value);

                                if (!tp.TranferEntitlementBalance(sourceAccount, desinationCardBL, entitlements, accountServicesDTO.Remarks, ref message, -1, -1,null))
                                {
                                    message = "Could not transfer balance. Please visit site." + message;
                                    log.LogMethodExit(message);
                                    message = utilities.MessageUtils.getMessage(2367);
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                                }

                                message = utilities.MessageUtils.getMessage(2368).Replace("&1", accountServicesDTO.Value.ToString()).Replace("&2", sourceAccount.AccountDTO.TagNumber).Replace("&3", desinationCardBL.AccountDTO.TagNumber).Replace("&4", message);
                                Messaging messaging = new Messaging(utilities);
                                List<ContactDTO> contacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).ToList();
                                if (contacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_EMAIL").Equals("Y"))
                                {
                                    messaging.SendEMail(contacts[0].Attribute1, "Funds transfer notification", message, -1, accountServicesDTO.Reference, sourceCustomerBL.CustomerDTO.Id, utilities.getServerTime());
                                }
                                contacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).ToList();
                                if (contacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_SMS").Equals("Y"))
                                {
                                    messaging.SendSMS(contacts[0].Attribute1, message);
                                }

                                CustomerBL destinationCustomerBL = new CustomerBL(executionContext, desinationCardBL.AccountDTO.CustomerId, true);
                                if(destinationCustomerBL != null && destinationCustomerBL.CustomerDTO != null)
                                {
                                    List<ContactDTO> desinationContacts = destinationCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).ToList();
                                    if (desinationContacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_EMAIL").Equals("Y"))
                                    {
                                        String recMessage = utilities.MessageUtils.getMessage(2368).Replace("&1", accountServicesDTO.Value.ToString()).Replace("&2", sourceAccount.AccountDTO.TagNumber).Replace("&3", desinationCardBL.AccountDTO.TagNumber).Replace("&4", message);
                                        messaging.SendEMail(desinationContacts[0].Attribute1, "Funds transfer notification", message, -1, accountServicesDTO.Reference, sourceCustomerBL.CustomerDTO.Id, utilities.getServerTime());
                                    }
                                    desinationContacts = destinationCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).ToList();
                                    if (desinationContacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_SMS").Equals("Y"))
                                    {
                                        messaging.SendSMS(desinationContacts[0].Attribute1, message);
                                    }
                                }

                                log.LogMethodExit(message);
                                return Request.CreateResponse(HttpStatusCode.OK, new { data = message, token = securityTokenDTO.Token });
                            }
                            else
                            {
                                message = "Source or destination account not found." + accountServicesDTO.DestinationAccountDTO.TagNumber + ":" + accountServicesDTO.DestinationAccountDTO.TagNumber + ":"
                                    + accountServicesDTO.Value + accountServicesDTO.EntitlementType;
                            }
                        }
                        break;
                    case AccountServicesDTO.ActivityType.REFUNDGAME:
                        {
                            if (!tp.RefundCardGames(new Card(accountServicesDTO.SourceAccountDTO.TagNumber, utilities.ParafaitEnv.LoginID, utilities),
                                (double)accountServicesDTO.Value, -1, accountServicesDTO.Remarks, ref message))
                            {
                                message = "Error" + message;
                                log.LogMethodExit(message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = message, token = securityTokenDTO.Token });
                        }
                    case AccountServicesDTO.ActivityType.REFUNDTAG:
                        {
                            AccountBL accountBL = new AccountBL(executionContext, accountServicesDTO.SourceAccountDTO.AccountId);
                            if (!tp.RefundCard(new Card(accountServicesDTO.SourceAccountDTO.TagNumber, utilities.ParafaitEnv.LoginID, utilities),
                                accountBL.AccountDTO.FaceValue == null ? 0 : Convert.ToDouble(accountBL.AccountDTO.FaceValue.ToString()),
                                accountBL.AccountDTO.Credits == null ? 0 : Convert.ToDouble(accountBL.AccountDTO.Credits.ToString()),
                                accountBL.AccountDTO.TotalCreditPlusBalance == null ? 0 : Convert.ToDouble(accountBL.AccountDTO.TotalCreditPlusBalance.ToString()),
                                accountServicesDTO.Remarks, ref message))
                            {
                                message = "Error" + message;
                                log.LogMethodExit(message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = message, token = securityTokenDTO.Token });
                        }
                    //case PerformAccountActivityDTO.ActivityType.DEACTIVATEFINGERPRINT:
                    //    {
                    //        AccountBL accountBL = new AccountBL(executionContext, performAccountActivityDTO.SourceAccountDTO);

                    //        bool status = accountBL.DeactivateFingerprint(performAccountActivityDTO.SourceAccountDTO.AccountId, performAccountActivityDTO.UserId, sqlTransaction);
                    //        if (!status)
                    //        {
                    //            message = "Please enter the valid accountId to deactivate";
                    //            log.LogMethodExit(message);
                    //            return Request.CreateResponse(HttpStatusCode.OK, new { data = message, token = securityTokenDTO.Token });
                    //        }
                    //        else
                    //        {
                    //            log.LogMethodExit(message);
                    //            message = "DeactivateFingerprint on card: " + performAccountActivityDTO.SourceAccountDTO.TagNumber + " was successful. " + message;
                    //            return Request.CreateResponse(HttpStatusCode.OK, new { data = accountBL.AccountDTO, token = securityTokenDTO.Token });
                    //        }                                    
                    //    }
                    case AccountServicesDTO.ActivityType.LOADVALUE:
                        {
                            Card currentCard = new Card(accountServicesDTO.SourceAccountDTO.TagNumber, securityTokenDTO.LoginId, utilities);
                            string entitlementType = "";
                            if (accountServicesDTO.Value < 0)
                            {
                                message = "Invalid card value.";
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                            }

                            if (accountServicesDTO.EntitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS)))
                            {
                                if (accountServicesDTO.Value <= Convert.ToDecimal(utilities.getParafaitDefaults("LOAD_BONUS_LIMIT")))
                                {
                                    entitlementType = String.IsNullOrEmpty(Convert.ToString(utilities.getParafaitDefaults("LOAD_BONUS_DEFAULT_ENT_TYPE"))) ? "B" : Convert.ToString(utilities.getParafaitDefaults("LOAD_BONUS_DEFAULT_ENT_TYPE"));
                                    if (!(tp.loadBonus(currentCard, Convert.ToDouble(accountServicesDTO.Value), tp.getEntitlementType(entitlementType), false, 0, "Loaded from tablet", ref message)))
                                    {
                                        message = "Error" + message;
                                        log.LogMethodExit(message);
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                                    }
                                }
                                else
                                {
                                    message = "Load Bonus limit has been set to " + utilities.getParafaitDefaults("LOAD_BONUS_LIMIT").ToString();
                                    return Request.CreateResponse(HttpStatusCode.OK, new { data = message, token = securityTokenDTO.Token });
                                }
                            }
                            else if (accountServicesDTO.EntitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.TICKET)))
                            {
                                int ticketVal = (int)accountServicesDTO.Value;
                                bool managerApprovalReceived = (utilities.ParafaitEnv.ManagerId != -1);
                                RedemptionBL redemptionBL = new RedemptionBL(executionContext);
                                redemptionBL.ManualTicketLimitChecks(managerApprovalReceived, ticketVal);
                                if (ticketVal <= Convert.ToInt32(utilities.getParafaitDefaults("LOAD_TICKETS_LIMIT")))
                                {
                                    if (!tp.loadTickets(currentCard, ticketVal, accountServicesDTO.Remarks, -1, ref message))
                                    {
                                        message = "ERROR" + message;
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                                    }
                                }
                                else
                                {
                                    message = "Load Tickets limit has been set to " + utilities.getParafaitDefaults("LOAD_TICKETS_LIMIT").ToString();
                                    return Request.CreateResponse(HttpStatusCode.OK, new { data = message, token = securityTokenDTO.Token });
                                }
                            }
                            else if (accountServicesDTO.EntitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.LOYALTY_POINT)))
                            {
                                double loyaltyPtsVal = Convert.ToDouble(accountServicesDTO.Value);
                                if (!(tp.loadBonus(currentCard, Convert.ToDouble(loyaltyPtsVal), tp.getEntitlementType("L"), false, 0, accountServicesDTO.Remarks, ref message)))
                                {
                                    message = "ERROR" + message;
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                                }
                            }
                            AccountBL accountBL = new AccountBL(executionContext, currentCard.card_id);
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = accountBL.AccountDTO, token = securityTokenDTO.Token });
                        }
                    case AccountServicesDTO.ActivityType.LOSTCARD:
                        {
                            if(accountServicesDTO.DeviceUuid != null)
                            {
                                string cardNum = "T" + accountServicesDTO.SourceAccountDTO.TagNumber.Substring(1);
                                AccountBL newAccountBL = null;
                                using (ParafaitDBTransaction dbTransaction = new ParafaitDBTransaction())
                                {
                                    try
                                    {
                                        dbTransaction.BeginTransaction();
                                        Card fromCard = new Card(accountServicesDTO.SourceAccountDTO.AccountId, utilities.ParafaitEnv.LoginID, utilities, dbTransaction.SQLTrx);
                                        Card toCard = new Card(cardNum, utilities.ParafaitEnv.LoginID, utilities, dbTransaction.SQLTrx);
                                        if (!tp.transferCard(fromCard, toCard, "New Card generated and " + accountServicesDTO.SourceAccountDTO.TagNumber + " is deactivated ", ref message, dbTransaction.SQLTrx))
                                        {
                                            message = "This card operation failed. Please visit site. " + message;
                                            log.LogMethodExit(message);
                                            message = utilities.MessageUtils.getMessage(2367);
                                            dbTransaction.RollBack();
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                                        }
                                        log.LogMethodExit(message);
                                        newAccountBL = new AccountBL(executionContext, toCard.CardNumber, true, true, dbTransaction.SQLTrx);
                                        dbTransaction.EndTransaction();
                                    }
                                    catch (Exception ex)
                                    {
                                        message = "This card operation failed. Please visit site. " + message;
                                        log.LogMethodExit(message);
                                        message = utilities.MessageUtils.getMessage(2367);
                                        dbTransaction.RollBack();
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                                    }
                                }

                                if (sourceCustomerBL != null && sourceCustomerBL.CustomerDTO != null)
                                {
                                    List<ContactDTO> desinationContacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).ToList();
                                    String recMessage = "Your card " + sourceAccount.AccountDTO.TagNumber + " has been marked as lost. New card " + cardNum + " has been issued. Please visit the site to convert this card to a physical card.";
                                    Messaging messaging = new Messaging(utilities);
                                    if (desinationContacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_EMAIL").Equals("Y"))
                                    {
                                        messaging.SendEMail(desinationContacts[0].Attribute1, "Card activity notification", recMessage, -1, "Lost Card", sourceCustomerBL.CustomerDTO.Id, utilities.getServerTime());
                                    }

                                    desinationContacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).ToList();
                                    if (desinationContacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_SMS").Equals("Y"))
                                    {
                                        messaging.SendSMS(desinationContacts[0].Attribute1, recMessage);
                                    }
                                }

                                return Request.CreateResponse(HttpStatusCode.OK, new { data = newAccountBL.AccountDTO, token = securityTokenDTO.Token });
                            }
                            else
                            {
                                message = "Source or destination account not found." + accountServicesDTO.DestinationAccountDTO.TagNumber + ":" + accountServicesDTO.DestinationAccountDTO.TagNumber + ":"
                                    + accountServicesDTO.Value + accountServicesDTO.EntitlementType;
                            }
                        }
                        break;
                    case AccountServicesDTO.ActivityType.DEDUCTBALANCE:
                        {
                            try
                            {
                                AccountBL accountBL = new AccountBL(executionContext, accountServicesDTO.SourceAccountDTO);
                                List<ValidationError> validationErrorList = accountBL.Validate();
                                if (validationErrorList.Any())
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, new { data = validationErrorList, token = securityTokenDTO.Token });
                                }

                                string status = "";// accountBL.DeductCardGameEntitlement(performAccountActivityDTO.MachineName, ref message);
                                return Request.CreateResponse(HttpStatusCode.OK, new { data = new { status, message }, token = securityTokenDTO.Token });
                            }
                            catch (Exception ex)
                            {
                                message = message + ex;
                            }
                        }
                        break;
                    default:
                        message = "Invalid activity type";
                        break;
                }

                log.LogMethodExit(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Get Card, Game, Refund info etc based on the activity type, accountId, account number.
        /// </summary>
        [HttpGet]
        [Route("api/Customer/Account/AccountServices")]
        public HttpResponseMessage Get(AccountServicesDTO.ActivityType activityType, int accountId = -1, string accountNumber = "", DateTime? fromDate = null, DateTime? toDate = null,
            decimal amount = 0, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            SqlTransaction sqlTransaction = null;

            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
                utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                SqlConnection conn = utilities.createConnection();
                sqlTransaction = conn.BeginTransaction();

                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddDays(1);
                AccountBL accountBL = null;

                if (fromDate != null)
                {
                    //DateTime.TryParseExact(fromDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                }

                if (toDate != null)
                {
                    //DateTime.TryParseExact(toDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                }
                else
                {
                    endDate = utilities.getServerTime();
                }


                if (accountId < 0 && string.IsNullOrEmpty(accountNumber))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "Pass the valid account id or Account number", token = securityTokenDTO.Token });
                }
                if (accountId > 0)
                {
                    accountBL = new AccountBL(executionContext, accountId);
                }
                else if (!string.IsNullOrEmpty(accountNumber))
                {
                    accountBL = new AccountBL(executionContext, accountNumber);
                }
                if(accountBL == null)
                {
                    log.LogMethodExit("Invalid Account");
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "Account not found", token = securityTokenDTO.Token });
                }
                var content = new object();
                switch (activityType)
                {
                    case AccountServicesDTO.ActivityType.REFUNDGAME:
                        {
                            log.LogMethodEntry(accountBL, executionContext);
                            accountBL.AccountDTO.RefundAccountGameDTOList = accountBL.GetRefundableGames(utilities.getServerTime());
                            content = accountBL.AccountDTO;
                            log.LogMethodExit();
                            break;
                        }
                    case AccountServicesDTO.ActivityType.REFUNDBALANCE:
                        {
                            log.LogMethodEntry(accountBL, executionContext);
                            accountBL.AccountDTO.RefundAccountCreditPlusDTOList = accountBL.GetRefundableCredits(utilities.getServerTime(), utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_DEPOSIT.Equals("Y"),
                                utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_CREDITS.Equals("Y"), utilities.ParafaitEnv.ALLOW_REFUND_OF_CREDITPLUS.Equals("Y"));
                            content = accountBL.AccountDTO;
                            log.LogMethodExit(content);
                            break;
                        }
                    case AccountServicesDTO.ActivityType.AVAILABLEBALANCE:
                        {
                            log.LogMethodEntry(accountBL);
                            content = accountBL.GetCurrentAvailableBalance();
                            log.LogMethodExit();
                            break;
                        }
                    case AccountServicesDTO.ActivityType.TOTALAVAILABLEBALANCE:
                        {
                            log.LogMethodEntry(accountBL);
                            content = accountBL.GetTotalAvailableBalance(utilities.getServerTime());
                            log.LogMethodExit();
                            break;
                        }
                    case AccountServicesDTO.ActivityType.CHECKBALANCE:
                        {
                            log.LogMethodEntry(accountBL);
                            Decimal? availableBalance = accountBL.GetCurrentAvailableBalance();
                            content = 0.0M;
                            if(availableBalance != null)
                            {
                                content = availableBalance < amount ? availableBalance : amount;
                            }
                            log.LogMethodExit();
                            break;
                        }
                    default:
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = "Activity Type not found", token = securityTokenDTO.Token });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

    }
}

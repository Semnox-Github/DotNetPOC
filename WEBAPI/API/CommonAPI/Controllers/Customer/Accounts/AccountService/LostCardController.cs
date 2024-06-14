/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Transfer the balances
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Customer.Accounts.AccountService
{
    public class LostCardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("api/Customer/Account/AccountService/LostCard")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] AccountServiceDTO accountServiceDTO)
        {
            ExecutionContext executionContext = null;
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(accountServiceDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (accountServiceDTO.SourceAccountDTO != null)
                {
                    String message = "";
                    if (accountServiceDTO.SourceAccountDTO.AccountId > 0 )
                    {
                        //if (accountServiceDTO.DeviceUuid != null)
                        {
                            Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
                            utilities.ParafaitEnv.SiteId = accountServiceDTO.SourceAccountDTO.SiteId;
                            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                            Users user = new Users(executionContext, executionContext.GetUserId());
                            utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
                            utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
                            utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                            utilities.ExecutionContext.SetSiteId(accountServiceDTO.SourceAccountDTO.SiteId);
                            utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
                            utilities.ParafaitEnv.Initialize();

                            TaskProcs tp = new TaskProcs(utilities);

                            AccountBL newAccountBL = null;
                            AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountServiceDTO.SourceAccountDTO.AccountId, true, true);
                            accountServiceDTO.SourceAccountDTO = accountBL.AccountDTO;
                            string cardNum = "T" + accountServiceDTO.SourceAccountDTO.TagNumber.Substring(1);
                            
                            using (ParafaitDBTransaction dbTransaction = new ParafaitDBTransaction())
                            {
                                try
                                {
                                    dbTransaction.BeginTransaction();
                                    Card fromCard = new Card(accountServiceDTO.SourceAccountDTO.AccountId, utilities.ParafaitEnv.LoginID, utilities, dbTransaction.SQLTrx);
                                    Card toCard = new Card(cardNum, utilities.ParafaitEnv.LoginID, utilities, dbTransaction.SQLTrx);
                                    if (!tp.transferCard(fromCard, toCard, "New Card generated and " + accountServiceDTO.SourceAccountDTO.TagNumber + " is deactivated ", ref message, dbTransaction.SQLTrx))
                                    {
                                        message = "This card operation failed. Please visit site. " + message;
                                        log.LogMethodExit(message);
                                        message = utilities.MessageUtils.getMessage(2367);
                                        dbTransaction.RollBack();
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message, token = securityTokenDTO.Token });
                                    }
                                    message = "Your card " + accountServiceDTO.SourceAccountDTO.TagNumber + " has been marked as lost. New card " + cardNum + " has been issued. Please visit the site to convert this card to a physical card.";
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
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message});
                                }
                            }
                        }
                        //else
                        //{
                        //    message = "Source or destination account not found." + accountServiceDTO.SourceAccountDTO.TagNumber;
                        //}
                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = message });
                    }
                    else
                    {
                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                    }
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
    }
}

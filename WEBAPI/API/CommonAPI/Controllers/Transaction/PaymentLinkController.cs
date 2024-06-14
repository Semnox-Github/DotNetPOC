/********************************************************************************************
 * Project Name - Transaction
 * Description  - API for the Payment Link
 * 
 **************
 **Version Log
 **************
 *Version     Date         Modified By           Remarks          
 *********************************************************************************************
 *2.100       19-Sep-2020  Girish Kundar        Created 
 ********************************************************************************************/


using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Transaction
{
    public class PaymentLinkController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object PaymentLinks 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/PaymentLinks")]
        public HttpResponseMessage Get(int transactionId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (transactionId == -1)
                {
                    log.Error("Invalid transaction id");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Invalid transaction id"));
                }
                if (TransactionPaymentLink.ISPaymentLinkEnbled(executionContext))
                {
                    Utilities utilities = new Utilities();
                    utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                    utilities.ParafaitEnv.RoleId = securityTokenDTO.RoleId;
                    utilities.ParafaitEnv.SetPOSMachine("", executionContext.POSMachineName);
                    utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                    TransactionPaymentLink transactionPaymentLink = new TransactionPaymentLink(executionContext, utilities, transactionId);
                    var content = transactionPaymentLink.GeneratePaymentLink();
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.Error("Payment Link is not Enabled");
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
        /// Performs a Post operation on PaymentLinks details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/PaymentLinks")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] TransactionEventContactsDTO transactionEventContactsDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionEventContactsDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                Utilities utilities = new Utilities();
                utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                utilities.ParafaitEnv.RoleId = securityTokenDTO.RoleId;
                utilities.ParafaitEnv.SetPOSMachine("", executionContext.POSMachineName);
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                if (transactionEventContactsDTO != null)
                {
                    TransactionPaymentLink transactionPaymentLink = new TransactionPaymentLink(executionContext, utilities, transactionEventContactsDTO);
                    transactionPaymentLink.SendPaymentLink(transactionEventContactsDTO.MessageChannel, null);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
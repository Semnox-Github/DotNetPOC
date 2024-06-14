/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - CardTransactionController  API -  Issues/Saves the list of card related data in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.CenterEdge;

namespace Semnox.CommonAPI.Controllers.ThirdParty.CenterEdge
{
    public class CardTransactionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [Route("api/ThirdParty/CenterEdge/Cards/{cardNumber}/Transactions/{transactionId:int=-1}")]
        [Authorize]
        public HttpResponseMessage Get([FromUri] string cardNumber, int transactionId = -1, int skip = 0, int take = 100)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(cardNumber, transactionId, skip, take);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (transactionId < 0 && string.IsNullOrEmpty(cardNumber))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { code = ErrorCode.cardNotFound.ToString(), message = "Card Not Found" });
                }
                if (transactionId > 0 && string.IsNullOrEmpty(cardNumber))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { code = ErrorCode.cardNotFound.ToString(), message = "Card Not Found" });
                }
                TransactionList ceTransaction = new TransactionList(executionContext);
                CardTransaction cardTransaction = ceTransaction.GetCardTransactions(cardNumber, transactionId, skip, take);
                string result = JsonConvert.SerializeObject(cardTransaction, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.DeserializeObject<JObject>(result));
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.NotFound, new { code = ErrorCode.cardNotFound.ToString(), message = "Card Not Found" });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new {code = ErrorCode.badRequest.ToString(), message = customException });
            }
        }
        [HttpPost]
        [Route("api/ThirdParty/CenterEdge/Cards/{accountNumber}/Transactions")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] string accountNumber, [FromBody] TransactionDTO postTransaction)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(postTransaction);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (string.IsNullOrEmpty(accountNumber))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { code = ErrorCode.cardNotFound.ToString(), message = "Card Not Found" });
                }
                if (postTransaction == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = "transaction is empty" });
                }
                postTransaction.cardNumber = accountNumber;
                TransactionBL transactionBL = new TransactionBL(executionContext, postTransaction);
                transactionBL.Save();
                CardTransaction cardTransactionResponse = transactionBL.GetTransactionDetails();
                var result = JsonConvert.SerializeObject(cardTransactionResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                log.LogMethodExit(JsonConvert.DeserializeObject<JObject>(result));
                return Request.CreateResponse(HttpStatusCode.Created, JsonConvert.DeserializeObject<JObject>(result));
            }
            catch (ValidationException valEx)
            {
                if(valEx.Message == "Card Not Found")
                {
                    string error = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                    log.Error(error);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { code = ErrorCode.cardNotFound.ToString(), message = "Card Not Found" });
                }
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = customException });
            }
        }
    }
}

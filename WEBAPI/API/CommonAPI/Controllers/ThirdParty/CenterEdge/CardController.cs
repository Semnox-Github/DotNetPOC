/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - Card API -  Gets the card related data from Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.CenterEdge;
using Semnox.Parafait.ThirdParty.CenterEdge.TransactionService;

namespace Semnox.CommonAPI.ThirdParty.CenterEdge
{
    public class CardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Cards List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/ThirdParty/CenterEdge/Cards/{cardNumber}",Order =2)]
        [Authorize]
        public HttpResponseMessage Get( string cardNumber)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();    
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { code = ErrorCode.cardNotFound.ToString(), message = "Card Not Found" });
                }
               
                CardBL cardBL = new CardBL(executionContext, cardNumber);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, cardBL.GetDetails());
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.NotFound, new
                {
                    code = ErrorCode.cardNotFound.ToString(),
                    message = "Card Not Found"
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.NotFound, new { code = ErrorCode.cardNotFound.ToString(), message = "Card Not Found" });
            }
        }

        /// <summary>
        /// Post the JSON Object  : Creates the new card 
        [HttpPost]
        [Route("api/ThirdParty/CenterEdge/Cards/{cardNumber}",Order =2)]
        [Authorize]
        public HttpResponseMessage Post([FromUri] string cardNumber, [FromBody] Operators operatorDTO)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(cardNumber, operatorDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (string.IsNullOrEmpty(cardNumber))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = "Bad Request" });
                }
                Card cardDTO = new Card();
                cardDTO.cardNumber = cardNumber;
                CardBL cardBL = new CardBL(executionContext, cardDTO);
                cardBL.Save();
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, cardBL.GetDetails());
            }
            catch (ValidationException valEx) // Throws only when card exists
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Conflict, new {
                    code = ErrorCode.cardExists.ToString(),
                    message = "Card already exists"
                });
            }
            catch (Exception ex) // All other exceptions are bad requests
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = customException });
            }
        }

        /// <summary>
        /// Delete the JSON Object  Cards List
        [HttpDelete]
        [Route("api/ThirdParty/CenterEdge/Cards/{cardNumber}",Order =2)]
        [Authorize]
        public HttpResponseMessage Delete([FromUri] string cardNumber, [FromBody] Card cardDTO)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(cardNumber, cardDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (string.IsNullOrEmpty(cardNumber))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = "Bad Request" });
                }
                TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(cardNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(cardNumber);
                    log.LogMethodExit(null, "Throwing Exception- " + message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = "Bad Request" });
                }
                CardBL cardBL = new CardBL(executionContext, cardNumber);
                cardBL.Delete();
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.NoContent); // If the card doesn't exist, this request should still return success as a 204, since HTTP DELETE requests are idempotent.
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(),  message = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object CETransactionServiceDTO
        /// </summary>
        /// <param name="CETransactionServiceDTO">CETransactionServiceDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/ThirdParty/CenterEdge/Cards/BulkIssue", Order = 1)]
        [Authorize]
        public HttpResponseMessage Post([FromBody]CETransactionServiceDTO transactionServiceDTO)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionServiceDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (transactionServiceDTO == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { code = ErrorCode.badRequest.ToString(), message = "Bad Request" });
                }
                if (transactionServiceDTO.adjustments == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = "adjustments is empty" });
                }
                TransactionBL transactionBL = new TransactionBL(executionContext, transactionServiceDTO);
                List<Card> cardList = transactionBL.IssueMultipleCards();
                string result = JsonConvert.SerializeObject(cardList, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                log.LogMethodExit(JsonConvert.DeserializeObject<JArray>(result));
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.DeserializeObject<JArray>(result));
            }
            catch (ValidationException valEx)
            {
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

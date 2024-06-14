/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - CardCombine  API -  Issues/Saves the list of card related data in Parafait
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
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.CenterEdge;
using Semnox.Parafait.ThirdParty.CenterEdge.TransactionService;
using Semnox.Parafait.Transaction;

/* Moves balance, time play value, privileges, etc from one card to another.

The destination card is the number in the path, and it may be a preexisting card that already has value, in which case the value from the source card is added to the destination card. If the destination card doesn't exist, it should be created.

If a preexisting destination card is not supported at all, this should be indicated to CenterEdge in the capabilities response.

The source card is left active in the system with no remaining balance. It may still be associated with a customer or have more value added.

This operation should be atomic, meaning that either both cards are updated or neither is changed.*/

namespace Semnox.CommonAPI.ThirdParty.CenterEdge.Cards
{

    /// <summary>
    /// This API does the Card consolidate task . 
    /// </summary>
    public class CardCombineController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("api/ThirdParty/CenterEdge/Cards/{cardNumber}/Combine")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] string cardNumber, [FromBody] CardCombineDTO cardDTO)
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
                if (string.IsNullOrEmpty(cardNumber) || cardDTO == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = "Bad Request" });
                }
                if (string.IsNullOrEmpty(cardDTO.sourceCardNumber))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = "Bad Request" });
                }
                if (cardNumber == cardDTO.sourceCardNumber)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = "Card numbers should be different" });
                }
                TransactionServiceDTO transactionServiceDTO = new TransactionServiceDTO();
                transactionServiceDTO.SourceAccountDTO.TagNumber = cardDTO.sourceCardNumber;
                transactionServiceDTO.DestinationAccountDTO.TagNumber = cardNumber;
                CardCombineBL cardCombineBL = new CardCombineBL(executionContext, transactionServiceDTO);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, cardCombineBL.ConsolidateCards());
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

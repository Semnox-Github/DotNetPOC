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

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.CenterEdge;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.ThirdParty.CenterEdge.Cards
{
    public class CardTransactionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpGet]
        [Route("api/ThirdParty/CenterEdge/Cards/{cardNumber}/Transactions/{transactionId:int=-1}")]
        [Authorize]
        public HttpResponseMessage Get([FromUri] string cardNumber ,int transactionId =-1 ,int skip =0, int take =0)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(cardNumber,skip,take);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                TransactionBL transactionBL = new TransactionBL(executionContext);
                CardTransaction cardTransaction = new CardTransaction();
                if (string.IsNullOrEmpty(cardNumber) || cardNumber != "12345678")
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new     {code = "cardNotFound", description = "Card Not Found"});
                }
                //if(transactionId > -1)
                //{
                    Parafait.ThirdParty.CenterEdge.Transaction trasaction = transactionBL.GetDetails();
                    Parafait.ThirdParty.CenterEdge.Transaction trasaction1 = transactionBL.GetAdjustDetails();
                cardTransaction.cardNumber = cardNumber;
                cardTransaction.skipped = 0;
                cardTransaction.totalCount = 1;
                    cardTransaction.transactions = new System.Collections.Generic.List<Parafait.ThirdParty.CenterEdge.Transaction> { trasaction, trasaction1 };
                //}
                //else
                //{
                //    //  transactions = transactionBL.GetDetails(cardNumber,skip,take);
                //    // get all the cards transactions
                //}
                return Request.CreateResponse(HttpStatusCode.OK, new { data = cardTransaction });
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


        [HttpPost]
        [Route("api/ThirdParty/CenterEdge/Cards/{cardNumber}/Transactions")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] string cardNumber , CardTransaction cardTransaction)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(cardTransaction);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (string.IsNullOrEmpty(cardNumber) || cardNumber != "12345678")
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new
                    {
                        code=  "cardNotFound",
                        description ="Card Not Found"   
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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

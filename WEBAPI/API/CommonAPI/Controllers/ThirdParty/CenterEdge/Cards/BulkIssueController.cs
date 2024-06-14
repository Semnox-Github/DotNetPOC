/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - BulkIssue API -  Issues/Saves the list of card related data in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *2.110.00    28-Sept-2020           Girish Kundar          Created 
 *******************************************************************************************/


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

namespace Semnox.CommonAPI.ThirdParty.CenterEdge.Cards
{
    public class BulkIssueController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object CETransactionServiceDTO
        /// </summary>
        /// <param name="CETransactionServiceDTO">CETransactionServiceDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/ThirdParty/CenterEdge/Card/BulkIssue")]
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), description = "Bad Request" });
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

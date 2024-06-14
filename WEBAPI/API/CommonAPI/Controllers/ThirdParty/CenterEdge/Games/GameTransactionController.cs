/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - GameTransactionController  API -  Issues/Saves the list of card related data in Parafait
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
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.ThirdParty.CenterEdge.Cards
{
    public class GameTransactionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpGet]
        [Route("api/ThirdParty/CenterEdge/Games/Transactions")]
        [Authorize]
        public HttpResponseMessage Get(int sinceId = 0, int take = 100)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(sinceId, take);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if(sinceId < 0 || take < 0)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = "badRequest", description = "Bad Request" });
                }
                TransactionList transactionListBL = new TransactionList(executionContext);
                CardTransaction cardTransaction = new CardTransaction();
                List<Parafait.ThirdParty.CenterEdge.TransactionDTO> resultList = transactionListBL.GetGamePlayTransactions("", sinceId, -1, 0, take);
                cardTransaction.transactions = new List<Parafait.ThirdParty.CenterEdge.TransactionDTO>();
                cardTransaction.transactions.AddRange(resultList);
                cardTransaction.sinceId = sinceId;
                cardTransaction.totalCount = cardTransaction.transactions.Count;
                string result = JsonConvert.SerializeObject(cardTransaction, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.DeserializeObject<JObject>(result));
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

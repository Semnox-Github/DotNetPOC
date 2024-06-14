/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Consolidate Cards
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Transaction.TransactionService
{
    public class ConsolidateCardsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Consolidates the Cards
        /// </summary>
        /// <param name="accountDTOList"></param>
        /// <param name="remarks"></param>
        /// <param name="invalidateSourceCard"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Transaction/TransactionService/ConsolidateCards")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] TransactionServiceDTO transactionServiceDTO)
        {

            log.LogMethodEntry(transactionServiceDTO);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                if (transactionServiceDTO.AccountDTOList != null && transactionServiceDTO.AccountDTOList.Any())
                {
                    ConsolidateCardBL consolidateCardBL = new ConsolidateCardBL(executionContext, transactionServiceDTO);
                    consolidateCardBL.CardConsolidate();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "Card consolidation is successfully completed" });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid inputs" });
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

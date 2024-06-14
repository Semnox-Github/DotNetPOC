/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Load Tickets to Card
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.70.3     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ParafaitServices.AccountService;
using Semnox.Core.GenericUtilities;
using ParafaitServices.TransactionService;

namespace Semnox.CommonAPI.Controllers.Transaction.TransactionService
{
    public class LoadTicketsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        
        [HttpPost]
        [Route("api/Transaction/TransactionService/LoadTickets")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] TransactionServiceDTO transactionServiceDTO)
        {
            try
            {
                log.LogMethodEntry(transactionServiceDTO);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                LoadTickets loadTickets = new LoadTickets(executionContext);
                if (transactionServiceDTO.SourceAccountDTO != null)
                {
                    loadTickets.LoadTicketsToCard(transactionServiceDTO.SourceAccountDTO, transactionServiceDTO.Tickets, transactionServiceDTO.Remarks);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""});
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

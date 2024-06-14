/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API to Deactivate the account
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120.0    22-Apr-2021   Prajwal S              Created
 ********************************************************************************************/


using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Customer.Accounts.AccountService
{
    public class DeactivateAccountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      


        [HttpPost]
        [Route("api/Customer/Account/AccountService/Deactivate")]
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
                if (accountServiceDTO != null && (accountServiceDTO.SourceAccountDTO != null && accountServiceDTO.SourceAccountDTO.AccountId > 0))
                {
                    DeactivateAccountBL deactivateAccountBL = new DeactivateAccountBL(executionContext, accountServiceDTO);
                    deactivateAccountBL.Deactivate();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = MessageContainerList.GetMessage(executionContext, "Card Deactivated Successfully")});
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid inputs" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = customException
                });
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

/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Link the Cards
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
namespace Semnox.CommonAPI.Customer.Accounts.AccountService
{
    public class LinkAccountsController : ApiController
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
        [Route("api/Customer/Account/AccountService/LinkAccounts")]
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
                if (accountServiceDTO.AccountDTOList != null && accountServiceDTO.AccountDTOList.Count > 0)
                {
                    if (accountServiceDTO.SourceAccountDTO != null && accountServiceDTO.SourceAccountDTO.AccountId > 0)
                    {
                        LinkAccountBL linkAccountBL = new LinkAccountBL(executionContext, accountServiceDTO);
                        linkAccountBL.LinkAccounts();
                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = "Account linked successfully" });
                    }
                    else
                    {
                        throw new ValidationException("Invalid Source Account");
                    }
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

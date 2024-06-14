/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Transfer the one account To another
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
namespace Semnox.CommonAPI.Customer.Accounts.AccountService
{
    public class TransferAccountsController : ApiController
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
        [Route("api/Customer/Account/AccountService/TransferAccounts")]
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
                if (accountServiceDTO.SourceAccountDTO != null && accountServiceDTO.DestinationAccountDTO != null )
                {
                    if(accountServiceDTO.DestinationAccountDTO.AccountId > -1)
                    {
                        throw new ValidationException("Please select new card to transfer");
                    }
                    AccountTransferBL transferAccountBL = new AccountTransferBL(executionContext, accountServiceDTO);
                    transferAccountBL.TransferAccounts();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "Successfully transferred"  });
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

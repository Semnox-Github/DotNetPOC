/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Cards "Audit Trail" entity. 
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        27-Feb-2019     Nagesh Badiger      Created
 *2.70.0      17-Sept-2019    Jagan Mohana        Renamed AuditTrailController to AccountAuditTrailController
 *2.80        05-Apr-2020      Girish Kundar      Modified: API path change and removed token from response body 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Customer.Accounts
{
    public class AccountAuditTrailController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object Cards Collections.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Account/AccountAuditTrails")]
        public HttpResponseMessage Get(string accountId)
        {
            try
            {
                log.LogMethodEntry(accountId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountAuditDTO.SearchByParameters, string>(AccountAuditDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
                AccountAuditListBL accountAuditListBL = new AccountAuditListBL(executionContext);
                var content = accountAuditListBL.GetAccountAuditDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}

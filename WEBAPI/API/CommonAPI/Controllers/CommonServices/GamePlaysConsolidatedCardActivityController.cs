/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Machines Game Plays
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        12-Sept-2018   Jagan          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.CommonAPI.CommonServices
{
    public class GamePlaysConsolidatedCardActivityController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Game Plays List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/CommonServices/GamePlaysConsolidatedCardActivity/")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get(string accountId, string cardNumber)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(accountId, cardNumber);                

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID, accountId));

                AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(executionContext);
                var content = accountActivityViewListBL.GetConsolidatedAccountActivityDTOList(searchParameters, true, null);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}

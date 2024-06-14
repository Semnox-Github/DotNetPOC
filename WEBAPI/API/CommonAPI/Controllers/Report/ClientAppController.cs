/********************************************************************************************
 * Project Name - ClientAppController
 * Description  - API to return Client App 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ClientApp;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers
{
    public class ClientAppController:ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the client app DTO
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/ClientApp/ClientApps")]
        public HttpResponseMessage Get(string appId=null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(appId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                ClientAppListBL clientAppListBL = new ClientAppListBL(executionContext);
                List<KeyValuePair<ClientAppDTO.SearchParameters, string>> searchParameters = new List<KeyValuePair<ClientAppDTO.SearchParameters, string>>();
                if(!string.IsNullOrEmpty(appId))
                    searchParameters.Add(new KeyValuePair<ClientAppDTO.SearchParameters, string>(ClientAppDTO.SearchParameters.APP_ID, appId));

                List<ClientAppDTO> clientAppDTOList = clientAppListBL.GetAllClientApp(searchParameters);
                log.LogMethodExit(clientAppDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = clientAppDTOList });

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }

        }

    }
}
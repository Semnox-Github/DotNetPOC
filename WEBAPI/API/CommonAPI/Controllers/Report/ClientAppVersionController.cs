/********************************************************************************************
 * Project Name - ClientAppversionController
 * Description  - Controller hosted on Client App DB Manager
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.ClientApp;

namespace Semnox.CommonAPI.Controllers
{
    public class InputHash
    {
        public string codeHash;
    }

    public class ClientAppversionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="buildNumber"></param>
        /// <param name="generatedTime"></param>
        /// <param name="securityCode"></param>
        /// <param name="inputHash"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/ClientApp/ClientAppVersion")]
        public HttpResponseMessage Post(string appId, string buildNumber, DateTime generatedTime, string securityCode, [FromBody]InputHash inputHash)
        {
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            try
            {
                log.LogMethodEntry(appId, buildNumber, inputHash.codeHash, generatedTime);
                ClientAppVersionMappingListBL clientAppVersionMappingListBL = new ClientAppVersionMappingListBL(executionContext);
                ClientAppVersionMappingDTO clientAppVersionMappingDTO = clientAppVersionMappingListBL.GetClientAppVersion(inputHash.codeHash, appId, buildNumber, generatedTime, securityCode);
                log.LogMethodExit(clientAppVersionMappingDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = clientAppVersionMappingDTO });
            }
            catch (ValidationException ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = ex.Message });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }

    }
}
/********************************************************************************************
* Project Name - Controller
* Description  - Created to get the version of the parafait.
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.100.0    16-Oct-2020   Mushahid Faizan           Created.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class ParafaitVersionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON ParfaitVersions 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/ParafaitVersions")]
        public HttpResponseMessage Get(int parafaitVersionId = -1, string parafaitExecutableName = null, int majorVersion = -1, int minorVersion = -1, int patchVersion = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(parafaitVersionId, parafaitExecutableName, majorVersion, minorVersion, patchVersion);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                ParafaitExecutableVersionNumberListBL parafaitExecutableVersionNumberListBL = new ParafaitExecutableVersionNumberListBL(executionContext);
                List<KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>(ParafaitExecutableVersionNumberDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (!string.IsNullOrEmpty(parafaitExecutableName))
                {
                    searchParameters.Add(new KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>(ParafaitExecutableVersionNumberDTO.SearchByParameters.PARAFAIT_EXECUTABLE_NAME, parafaitExecutableName.ToString()));
                }
                if (parafaitVersionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>(ParafaitExecutableVersionNumberDTO.SearchByParameters.ID, parafaitVersionId.ToString()));
                }
                if (majorVersion > -1)
                {
                    searchParameters.Add(new KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>(ParafaitExecutableVersionNumberDTO.SearchByParameters.MAJOR_VERSION, majorVersion.ToString()));
                }
                if (minorVersion > -1)
                {
                    searchParameters.Add(new KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>(ParafaitExecutableVersionNumberDTO.SearchByParameters.MINOR_VERSION, minorVersion.ToString()));
                }
                if (patchVersion > -1)
                {
                    searchParameters.Add(new KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>(ParafaitExecutableVersionNumberDTO.SearchByParameters.MINOR_VERSION, patchVersion.ToString()));
                }
                var content = parafaitExecutableVersionNumberListBL.GetParafaitExecutableVersionNumberDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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

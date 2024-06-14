/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the DB Synch
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.150       28-Sep-2023   Lakshminarayana Rao    Created 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;

namespace Semnox.CommonAPI.Configuration
{
    public class SystemOptionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object DBSynch List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/SystemOptions")]
        public HttpResponseMessage Get()
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                SystemOptionsList systemOptionsList = new SystemOptionsList(executionContext);
                List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                var content = systemOptionsList.GetSystemOptionsDTOList(searchParameters);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Post the JSON SystemOption.
        /// </summary>
        /// <param name="systemOptionsDTOList">systemOptionsDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Configuration/SystemOptions")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<SystemOptionsDTO> systemOptionsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(systemOptionsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (systemOptionsDTOList != null && systemOptionsDTOList.Any())
                {
                    SystemOptionsList systemOptionsList = new SystemOptionsList(executionContext, systemOptionsDTOList);
                    systemOptionsList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
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
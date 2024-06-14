/********************************************************************************************
* Project Name - Generic Utilities
* Description  - Controller for ObjectTranslation Class.
* 
**************
**Version Log
**************
*Version     Date             Modified By       Remarks          
*********************************************************************************************
*2.80        23-Mar-2019     Mushahid Faizan   Created
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace Semnox.CommonAPI.Controllers.CommonServices
{
    public class ObjectTranslationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// returns ObjectTranslationsDTOList
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Common/ObjectTranslations")]
        [Authorize]
        public HttpResponseMessage Get(bool isActive = false, string objectName = null, string elementName = null, string elementGuid = null)
        {
            try
            {
                log.LogMethodEntry(isActive, objectName, elementName, elementGuid);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
                if (isActive)
                {
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.IS_ACTIVE, "1"));
                }
                if (!string.IsNullOrEmpty(objectName))
                {
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.OBJECT, objectName));
                }
                if (!string.IsNullOrEmpty(elementName))
                {
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT, elementName));
                }
                if (!string.IsNullOrEmpty(elementGuid))
                {
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, elementGuid));
                }
                searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(executionContext);
                var content = objectTranslationsList.GetAllObjectTranslations(searchParameters);
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

        /// <summary>
        ///  Post the objectTranslationsDTOList
        /// </summary>
        /// <param name="objectTranslationsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Common/ObjectTranslations")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ObjectTranslationsDTO> objectTranslationsDTOList)
        {
            try
            {
                log.LogMethodEntry(objectTranslationsDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;

                if (objectTranslationsDTOList != null && objectTranslationsDTOList.Any())
                {
                    ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(executionContext, objectTranslationsDTOList);
                    objectTranslationsList.Save();
                    log.LogMethodExit(objectTranslationsDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = objectTranslationsDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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

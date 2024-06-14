/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert content management
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.60        20-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        20-May-2020   Mushahid Faizan           Modified :As per Rest API standard, Added searchParameters
*                                                    and Renamed controller from ApplicationContentManagmentController to ApplicationContentController
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.GameServer
{
    public class ApplicationContentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON applicationContentDTOList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Common/ApplicationContents")]
        public HttpResponseMessage Get(string isActive = null, string application = null, int appContentId = -1, string chapter = null, int contentId = -1,
                                        string module = null, bool loadActiveChild = false, bool buildChildRecords = false)
        {
            log.LogMethodEntry(isActive, application, appContentId, chapter, contentId, module, loadActiveChild, buildChildRecords);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                ApplicationContentListBL applicationContentListBL = new ApplicationContentListBL(executionContext);
                List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(application))
                {
                    searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.APPLICATION, application.ToString()));
                }
                if (!string.IsNullOrEmpty(module))
                {
                    searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.MODULE, module.ToString()));
                }
                if (appContentId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.APP_CONTENT_ID, appContentId.ToString()));
                }
                if (!string.IsNullOrEmpty(chapter))
                {
                    searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.CHAPTER, chapter.ToString()));
                }
                if (contentId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.CONTENT_ID, contentId.ToString()));
                }
                List<ApplicationContentDTO> content = applicationContentListBL.GetApplicationContentDTOList(searchParameters, buildChildRecords, loadActiveChild);
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

        /// <summary>
        /// Post the JSON applicationContentDTOList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Common/ApplicationContents")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ApplicationContentDTO> applicationContentDTOList)
        {
            log.LogMethodEntry(applicationContentDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (applicationContentDTOList != null && applicationContentDTOList.Any())
                {
                    ApplicationContentListBL applicationContentListBL = new ApplicationContentListBL(executionContext, applicationContentDTOList);
                    applicationContentListBL.Save();
                    log.LogMethodExit(applicationContentDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = applicationContentDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
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

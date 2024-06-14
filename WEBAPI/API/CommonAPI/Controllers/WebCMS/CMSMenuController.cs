/********************************************************************************************
* Project Name - CMS                                                                      
* Description  - Controller of the CMSMenu class
*
**************
**Version Log
*Version     Date          Modified By          Remarks          
*********************************************************************************************
*2.80        11-May-2020   Indrajeet Kumar      Created 
********************************************************************************************/
using System;
using System.Web;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.WebCMS;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.WebCMS
{
    public class CMSMenuController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object Questions List
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/WebCMS/CMSMenus")]
        [Authorize]
        public HttpResponseMessage Get(int menuId = -1, string  isActive = null)
        {
            try
            {
                log.LogMethodEntry(menuId, isActive);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>> SearchParameter = new List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>>();
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        SearchParameter.Add(new KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>(CMSMenusDTO.SearchByRequestParameters.ACTIVE,isActive));
                    }
                }
                if (menuId > -1)
                {
                    SearchParameter.Add(new KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>(CMSMenusDTO.SearchByRequestParameters.MENU_ID, menuId.ToString()));
                }
              

                CMSMenusBLList cMSMenusBLList = new CMSMenusBLList(executionContext);
                var content = cMSMenusBLList.GetAllCmsMenus(SearchParameter);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                return response;
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post request for CMSModules
        /// </summary>
        /// <param name="cMSModulesDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/WebCMS/CMSMenus")]
        public HttpResponseMessage Post([FromBody] List<CMSMenusDTO> cMSMenusDTOList)
        {
            try
            {
                log.LogMethodEntry(cMSMenusDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (cMSMenusDTOList != null && cMSMenusDTOList.Any())
                {
                    CMSMenusBLList cMSMenusBLList = new CMSMenusBLList(executionContext, cMSMenusDTOList);
                    cMSMenusBLList.Save();                         
                    log.LogMethodExit(cMSMenusDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = cMSMenusDTOList });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Input data is invalid." });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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

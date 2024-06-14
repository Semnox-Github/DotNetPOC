/********************************************************************************************
* Project Name - CMS                                                                      
* Description  - Controller of the CMSModule class
*
**************
**Version Log
*Version     Date          Modified By          Remarks          
*********************************************************************************************
*2.80        24-Apr-2020   Indrajeet Kumar      Created 
*2.130.2     17-Feb-2022   Nitin Pai            CMS Changes for SmartFun
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
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Controllers.WebCMS
{
    public class CMSModuleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object Module.
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/WebCMS/CMSModules")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string moduleName = null, string isActive = null, bool buildChildRecords = false, bool activeChildRecords = false, bool replacePlaceHolders = true)
        {
            try
            {
                log.LogMethodEntry(moduleName, isActive, buildChildRecords, activeChildRecords);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>> searchParameter = new List<KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>>();
                if (!String.IsNullOrEmpty(moduleName))
                {
                    searchParameter.Add(new KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>(CMSModulesDTO.SearchByRequestParameters.DESCRIPTION, moduleName));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameter.Add(new KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>(CMSModulesDTO.SearchByRequestParameters.ACTIVE, isActive));
                    }
                }

                CMSModulesBLList cmsModuleBLList = new CMSModulesBLList(executionContext);
                var content = await Task<List<CMSModulesDTO>>.Factory.StartNew(() => {
                    return cmsModuleBLList.GetAllCMSModules(searchParameter, buildChildRecords, activeChildRecords, null, replacePlaceHolders);
                });
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
        /// Post request for CMSModule
        /// </summary> 
        /// <param name="cmsModulesDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/WebCMS/CMSModules")]
        public HttpResponseMessage Post([FromBody] List<CMSModulesDTO> cmsModulesDTOList)
        {
            try
            {
                log.LogMethodEntry(cmsModulesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (cmsModulesDTOList != null && cmsModulesDTOList.Any())
                {
                    CMSModulesBLList cmsModuleBLList = new CMSModulesBLList(executionContext, cmsModulesDTOList);
                    cmsModuleBLList.Save();
                    log.LogMethodExit(cmsModulesDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = cmsModulesDTOList });
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


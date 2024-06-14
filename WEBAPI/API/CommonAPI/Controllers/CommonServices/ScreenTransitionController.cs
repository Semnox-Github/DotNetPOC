/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Themes and Screen Transition details
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By     Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana    Created
 *2.60        16-Apr-2019    Jagan Mohana    Handled the reader themes and signage themes for the both Game and Digital Signage
 *2.80        16-May-2020     Girish Kundar  Modified: REST API standard
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.DigitalSignage;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.CommonServices
{
    public class ScreenTransitionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object Themes and Screen transitions List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/CommonServices/ScreenTransitions")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, string moduleName = null)
        {
            try
            {
                log.LogMethodEntry(isActive, moduleName);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));
                if (string.IsNullOrEmpty(moduleName) == false && moduleName.ToUpper().ToString() == "DIGITALSIGNAGE")
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "THEME_TYPE"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                    if (lookupValuesDTOList != null)
                    {
                        searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_ID, lookupValuesDTOList.Find(x => x.LookupValue == "Panel" && x.LookupName == "THEME_TYPE").LookupValueId.ToString()));
                    }
                }
                else
                {
                    string themeTypeList = "Audio,Display,Visualization";
                    searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_LIST, themeTypeList.ToString()));
                }
                ThemeListBL screentransitionList = new ThemeListBL(executionContext);
                var content = screentransitionList.GetThemeDTOList(searchParameters,true,true);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content   });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message   });
            }
        }

        /// <summary>
        /// Performs a Post operation on screen zone details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/CommonServices/ScreenTransitions")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ThemeDTO> themesToList)
        {
            try
            {
                log.LogMethodEntry(themesToList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (themesToList != null)
                {
                    // if themesToList.Id is less than zero then insert or else update
                    ThemeListBL themeListBL = new ThemeListBL(executionContext,themesToList);
                    themeListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""   });
                }
                else
                {
                    log.Debug("ScreenTransitionsController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""   });
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
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message   });
            }
        }
        /// <summary>
        /// Performs a Delete operation on screen zone details.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        //DELETE: api/Subscriber/
        [HttpDelete]
        [Route("api/CommonServices/ScreenTransitions")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ThemeDTO> themeDTOs)
        {
            try
            {
                log.LogMethodEntry(themeDTOs);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (themeDTOs.Count != 0)
                {
                    ThemeListBL ThemeListBL = new ThemeListBL(executionContext,themeDTOs);
                    ThemeListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""   });
                }
                else
                {
                    log.Debug("ScreenTransitionsController-Delete() Method. Else condition");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""   });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message   });
            }
        }
    }
}
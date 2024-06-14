/**************************************************************************************************
 * Project Name - Games 
 * Description  - Controller for ReaderTheme
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.80        13-Mar-2020       Vikas Dwivedi             Created to Get and Post Methods.
 **************************************************************************************************/
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class ReaderThemeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
       
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object ThemeDTO
        /// </summary>        
        /// <returns>HttpResponseMessage</returns> 
        /// <param name="moduleName">moduleName</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="isActive">activeRecordsOnly</param>
        [HttpGet]
        [Route("api/Game/ReaderThemes")]
        [Authorize]
        public HttpResponseMessage Get(string moduleName = null, string isActive = null, bool buildChildRecords = false,bool activeChildRecords = false)
        {
            
                log.LogMethodEntry(moduleName, isActive, buildChildRecords, activeChildRecords);
                SecurityTokenDTO securityTokenDTO = null;
                ExecutionContext executionContext = null;
                try
                {
                    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                    securityTokenBL.GenerateJWTToken();
                    securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                    executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                    List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (moduleName.ToUpper().ToString() == "DIGITALSIGNAGE")
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
                ThemeListBL themeListBL = new ThemeListBL(executionContext);
                var content = themeListBL.GetThemeDTOList(searchParameters, buildChildRecords, activeChildRecords, null);
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
        /// Post the JSON Object of ThemeDTO List
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Game/ReaderThemes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ThemeDTO> themesToList)
        {
            log.LogMethodEntry(themesToList);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (themesToList != null && themesToList.Any())
                {
                    // if themesToList.Id is less than zero then insert or else update
                    ThemeListBL themeListBL = new ThemeListBL(executionContext, themesToList);
                    themeListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = themesToList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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

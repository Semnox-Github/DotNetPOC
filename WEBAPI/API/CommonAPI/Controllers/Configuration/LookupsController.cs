/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the lookups Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         13-Mar-2019   Jagan Mohana         Created 
               08-Apr-2019   Akshay Gulaganji     modified Get() --> lookupValuesList.GetAllLookups(searchParameters, loadChild);
               09-Apr-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
*2.90         11-May-2020   Girish Kundar         Modified : Moved to Configuration and Changes as part of the REST API  
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

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class LookupsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object lookups and lookup values Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/Lookups")]
        public HttpResponseMessage Get(int lookupId = -1, string lookupName = null, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(lookupId, lookupName, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.ISACTIVE, "1"));
                    }
                }
                if (lookupId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_ID, lookupId.ToString()));
                }
                if (!string.IsNullOrEmpty(lookupName))
                {
                    searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, lookupName.ToString()));
                }

                LookupsList lookupsListBL = new LookupsList(executionContext);
                List<LookupsDTO> lookupsDTOList = lookupsListBL.GetAllLookups(searchParameters, true);
                if(lookupsDTOList !=null && lookupsDTOList.Any())
                {
                    lookupsDTOList = lookupsDTOList.OrderBy(x=>x.LookupId).ToList();
                }
                bool isProtected = false;
                if (securityTokenDTO.LoginId.ToLower() != "semnox")
                {
                    isProtected = true;
                }
                /// isProtectedReadonly : UI handle the lookup and lookup value can be readonly or not, if the Protected value equals to "Y"
                log.LogMethodExit(lookupsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = lookupsDTOList, isProtectedReadonly = isProtected });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Performs a Post operation on lookups details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Configuration/Lookups")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<LookupsDTO> lookupsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(lookupsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (lookupsDTOList != null)
                {
                    LookupsList lookupsList = new LookupsList(executionContext, lookupsDTOList);
                    lookupsList.Save();
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

        /// <summary>
        /// Performs a Delete operation on lookups details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Configuration/Lookups")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<LookupsDTO> lookupsDTOList)
        {
            log.LogMethodEntry(lookupsDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                if (lookupsDTOList != null && lookupsDTOList.Count != 0)
                {
                    LookupsList lookupsList = new LookupsList(executionContext, lookupsDTOList);
                    lookupsList.Delete();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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

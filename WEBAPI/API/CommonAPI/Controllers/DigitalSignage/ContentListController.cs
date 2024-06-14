/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Content List
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana Rao        Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.DigitalSignage;
using System.Web.Http;
using System.Web;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.DigitalSignage
{
    [Route("api/[controller]")]
    public class ContentListController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Content List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/ContentList/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DSLookupDTO.SearchByParameters, string>>();
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));

                DSLookupListBL lookupListBL = new DSLookupListBL(executionContext);
                var content = lookupListBL.GetDSLookupDTOList(searchParameters);
                log.LogMethodEntry(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on dslookup details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/ContentList/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<DSLookupDTO> lookupDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(lookupDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (lookupDTOs != null)
                {
                    // if lookupDTOs.DSLookupID is less than zero then insert or else update
                    DSLookupListBL media = new DSLookupListBL(lookupDTOs, executionContext);
                    media.SaveUpdateDsLookupList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("ContentListController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Conflict, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on dslookup details.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        //DELETE: api/Subscriber/
        [HttpDelete]
        [Route("api/DigitalSignage/ContentList/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<DSLookupDTO> lookuplistDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(lookuplistDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (lookuplistDTOs.Count != 0)
                {
                    DSLookupListBL item = new DSLookupListBL(lookuplistDTOs, executionContext);
                    item.SaveUpdateDsLookupList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("ContentListController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}

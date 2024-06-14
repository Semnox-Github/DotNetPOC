/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Content Pattern
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        07-May-2019   Akshay Gulaganji        Created 
 ********************************************************************************************/
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

namespace Semnox.CommonAPI.DigitalSignage
{
    [Route("api/[controller]")]
    public class ContentPatternController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Content Pattern
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/ContentPattern/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext);
                List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.IS_ACTIVE, isActive));
                }               
                List<SignagePatternDTO> signagePatternDTOList = signagePatternListBL.GetSignagePatternDTOList(searchParameters);
                SortableBindingList<SignagePatternDTO> signagePatternDTOSortableList;
                if (signagePatternDTOList != null)
                {
                    signagePatternDTOSortableList = new SortableBindingList<SignagePatternDTO>(signagePatternDTOList);
                }
                else
                {
                    signagePatternDTOSortableList = new SortableBindingList<SignagePatternDTO>();
                }
                log.LogMethodEntry(signagePatternDTOSortableList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = signagePatternDTOSortableList, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on SignagePattern
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/ContentPattern/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<SignagePatternDTO> signagePatternDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(signagePatternDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (signagePatternDTOList != null)
                {
                    SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext, signagePatternDTOList);
                    signagePatternListBL.SaveUpdateSignagePatternList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Delete operation on SignagePattern
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // DELETE: api/Subscriber/
        [HttpDelete]
        [Route("api/DigitalSignage/ContentPattern/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<SignagePatternDTO> signagePatternDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(signagePatternDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (signagePatternDTOList != null)
                {
                    SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext, signagePatternDTOList);
                    signagePatternListBL.SaveUpdateSignagePatternList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}

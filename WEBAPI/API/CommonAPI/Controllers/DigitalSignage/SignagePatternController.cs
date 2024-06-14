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
 *2.90        10-Aug-2020   Mushahid Faizan        Modified : Renamed Controller from ContentPatternController to SignagePatternController
 *                                                  Added search parameters in get, Removed Delete() and removed token from response body.
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
using Semnox.Parafait.DigitalSignage;

namespace Semnox.CommonAPI.DigitalSignage
{
    public class SignagePatternController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Content Pattern
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/SignagePatterns")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int signagePatternId = -1, string signagePatternName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext);
                List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }

                if (signagePatternId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.SIGNAGE_PATTERN_ID, signagePatternId.ToString()));
                }
                if (!string.IsNullOrEmpty(signagePatternName))
                {
                    searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.NAME, signagePatternName));
                }

                List<SignagePatternDTO> signagePatternDTOList = signagePatternListBL.GetSignagePatternDTOList(searchParameters);
                log.LogMethodEntry(signagePatternDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = signagePatternDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Performs a Post operation on SignagePattern
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/DigitalSignage/SignagePatterns")]
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
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (signagePatternDTOList != null && signagePatternDTOList.Any())
                {
                    SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext, signagePatternDTOList);
                    signagePatternListBL.Save();
                    log.LogMethodExit(signagePatternDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = signagePatternDTOList });
                }
                else
                {
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
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }
    }
}

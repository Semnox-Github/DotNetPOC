/********************************************************************************************
 * Project Name - Currency
 * Description  - API for the Currency Details
 * 
 **************
 **Version Log
 **************
 *Version     Date         Modified By           Remarks          
 *********************************************************************************************
 *2.60       19-Mar-2019   Jagan Mohana Rao      Created 
             26-Mar-2019   Mushahid Faizan       Added log Method Entry & Exit, IsActive SearchParameter &
                                                 declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Currency;

namespace Semnox.CommonAPI.SiteSetup
{
    public class CurrencyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Currency List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/Currency/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                CurrencyList currencyList = new CurrencyList(executionContext);
                List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>> searchParameters = new List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>>();

                searchParameters.Add(new KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>(CurrencyDTO.SearchByCurrencyParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>(CurrencyDTO.SearchByCurrencyParameters.IS_ACTIVE, isActive));
                }
                var content = currencyList.GetAllCurrency(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on CurrencyDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/Currency/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<CurrencyDTO> currencyDTOList)
        {
            try
            {
                log.LogMethodEntry(currencyDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (currencyDTOList != null)
                {
                    // if currencyDTOList.currencyId is less than zero then insert or else update
                    CurrencyList countryDTOList = new CurrencyList(executionContext, currencyDTOList);
                    countryDTOList.SaveUpdateCurrencyList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
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
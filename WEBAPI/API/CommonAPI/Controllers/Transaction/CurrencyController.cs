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

namespace Semnox.CommonAPI.Transaction
{
    public class CurrencyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Currency List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/Currencies")]
        public HttpResponseMessage Get(string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                CurrencyList currencyList = new CurrencyList(executionContext);
                List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>> searchParameters = new List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>>();

                searchParameters.Add(new KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>(CurrencyDTO.SearchByCurrencyParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>(CurrencyDTO.SearchByCurrencyParameters.IS_ACTIVE, isActive));
                    }
                }
                var content = currencyList.GetAllCurrency(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Performs a Post operation on CurrencyDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/Currencies")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<CurrencyDTO> currencyDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(currencyDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (currencyDTOList != null)
                {
                    CurrencyList countryDTOList = new CurrencyList(executionContext, currencyDTOList);
                    countryDTOList.SaveUpdateCurrencyList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "NotFound" });
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
/********************************************************************************************
 * Project Name - RedemptionCurrencyCount Controller
 * Description  - Created RedemptionCurrencyCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   11-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RedemptionCurrencyCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Currency List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/RedemptionCurrencyCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null,int currencyId = -1, string currencyCode = null,
                                        int productId = -1, int currentPage = 0, int pageSize = 10)
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

                RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(executionContext);
                List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();

                searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, isActive));
                    }
                }

                if (!string.IsNullOrEmpty(currencyCode))
                {
                    searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.BARCODE, currencyCode));
                }
                if (currencyId > -1)
                {
                    searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID, currencyId.ToString()));
                }
                if (productId > -1)
                {
                    searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.PRODUCT_ID, productId.ToString()));
                }

                IRedemptionCurrencyUseCases redemptionCurrencyUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfRedemptionCurrencies = await redemptionCurrencyUseCases.GetRedemptionCurrencyCount(searchParameters); 
                log.LogVariableState("totalNoOfRedemptionCurrencies", totalNoOfRedemptionCurrencies);
                totalNoOfPages = (totalNoOfRedemptionCurrencies / pageSize) + ((totalNoOfRedemptionCurrencies % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfRedemptionCurrencies);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfRedemptionCurrencies, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }
    }
}
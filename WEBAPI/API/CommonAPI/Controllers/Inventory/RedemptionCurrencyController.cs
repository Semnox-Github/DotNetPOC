/********************************************************************************************
 * Project Name - Currency
 * Description  - API for the Redemption Currency Details
 * 
 **************
 **Version Log
 **************
 *Version     Date         Modified By           Remarks          
 *********************************************************************************************
 *2.100.0    05-Oct-2020   Mushahid Faizan      Created 
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Linq;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RedemptionCurrencyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Currency List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/RedemptionCurrencies")]
        public async Task<HttpResponseMessage> Get(string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false, int currencyId = -1, string currencyCode = null,
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

                int totalNoOfPages = 0;
                int totalNoOfRedemptionCurrencies = await Task<int>.Factory.StartNew(() => { return redemptionCurrencyList.GetRedemptionCurrenciesCount(searchParameters, null); });
                log.LogVariableState("totalNoOfRedemptionCurrencies", totalNoOfRedemptionCurrencies);
                totalNoOfPages = (totalNoOfRedemptionCurrencies / pageSize) + ((totalNoOfRedemptionCurrencies % pageSize) > 0 ? 1 : 0);

                IRedemptionCurrencyUseCases redemptionCurrencyUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyUseCases(executionContext);
                List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = await redemptionCurrencyUseCases.GetRedemptionCurrencies(searchParameters, currentPage, pageSize);
                log.LogMethodExit(redemptionCurrencyDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionCurrencyDTOList, currentPageNo = currentPage, TotalCount = totalNoOfRedemptionCurrencies });
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

        /// <summary>
        /// Performs a Post operation on RedemptionCurrencyDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Inventory/RedemptionCurrencies")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<RedemptionCurrencyDTO> redemptionCurrencyDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCurrencyDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (redemptionCurrencyDTOList == null || redemptionCurrencyDTOList.Any(a => a.CurrencyId > 0))
                {
                    log.LogMethodExit(redemptionCurrencyDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IRedemptionCurrencyUseCases redemptionCurrencyUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyUseCases(executionContext);
                await redemptionCurrencyUseCases.SaveRedemptionCurrencies(redemptionCurrencyDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionCurrencyDTOList });
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

        /// <summary>
        /// Performs a Post operation on RedemptionCurrencyDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/Inventory/RedemptionCurrencies")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<RedemptionCurrencyDTO> redemptionCurrencyDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCurrencyDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (redemptionCurrencyDTOList == null || redemptionCurrencyDTOList.Any(a => a.CurrencyId < 0))
                {
                    log.LogMethodExit(redemptionCurrencyDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IRedemptionCurrencyUseCases redemptionCurrencyUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyUseCases(executionContext);
                await redemptionCurrencyUseCases.SaveRedemptionCurrencies(redemptionCurrencyDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionCurrencyDTOList });
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

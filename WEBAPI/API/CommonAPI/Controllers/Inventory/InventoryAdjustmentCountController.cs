/********************************************************************************************
 * Project Name - InventoryAdjustmentCount Controller
 * Description  - Created InventoryAdjustmentCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   10-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryAdjustmentCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryAdjustments.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/AdjustmentCounts")]
        public async Task<HttpResponseMessage> Get(string productCode = null, string description = null, string productBarcode = null,
                                                   int locationId = -1, string purchaseable = null, string advancedSearch = null, string pivotColumns = null,
                                                   bool ignoreWastage = true, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(currentPage, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters = new List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(productCode))
                {
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PRODUCT_CODE, productCode));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.DESCRIPTION, description));
                }
                if (!string.IsNullOrEmpty(productBarcode))
                {
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.BARCODE, productBarcode));
                }
                if (locationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.LOCATION_ID, locationId.ToString()));
                }
                if (!string.IsNullOrEmpty(purchaseable))
                {
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PURCHASEABLE, purchaseable));
                }

                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                IInventoryAdjustmentsUseCases inventoryAdjustmentsUseCases = InventoryUseCaseFactory.GetInventoryAdjustmentsUseCases(executionContext);

                int totalNoOfPages = 0;
                int totalInventoryAdjustmentsCount = await inventoryAdjustmentsUseCases.GetInventoryAdjustmentCount(searchParameters, advancedSearch, pivotColumns);
                log.LogVariableState("totalCount", totalInventoryAdjustmentsCount);
                totalNoOfPages = (totalInventoryAdjustmentsCount / pageSize) + ((totalInventoryAdjustmentsCount % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalInventoryAdjustmentsCount);

                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalInventoryAdjustmentsCount, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
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

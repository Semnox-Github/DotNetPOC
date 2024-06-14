/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Physical Count Inventory .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    04-Jan-2021  Mushahid Faizan                  created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.PhysicalCount;

namespace Semnox.CommonAPI.Controllers
{
    public class PhysicalCountInventoryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the PhysicalCountReview.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/{physicalCountId}/PhysicalCountAdjustments")]
        public async Task<HttpResponseMessage> Get([FromUri] int physicalCountId, int locationId = -1, DateTime? startDate = null, int categoryId = -1, int uomId = -1,
                                                   string productCode = null, string productBarcode = null, string description = null, string filterText = null, bool ismodifiedDuringPhysicalCount = true,
                                                   string isPurchaseable = null, string advancedSearch = null, string symbol = null, double quantity = 0.0, int currentPage = 0, int pageSize = 10)
        {

            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(physicalCountId, currentPage, pageSize);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters = new List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>>();
                searchParameters.Add(new KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>(PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                DateTime physicalCountStartDate = Convert.ToDateTime(startDate);

                if (startDate == null)
                {
                    physicalCountStartDate= ServerDateTime.Now;
                    //physicalCountStartDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                }
                //DateTime physicalCountStartDate = Convert.ToDateTime(startDate);
                //physicalCountStartDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);

                if (categoryId > -1)
                {
                    searchParameters.Add(new KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>(PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CATEGORYID, categoryId.ToString()));
                }
                if (uomId > -1)
                {
                    searchParameters.Add(new KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>(PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.UOM_ID, uomId.ToString()));
                }
                if (!string.IsNullOrEmpty(productCode))
                {
                    searchParameters.Add(new KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>(PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CODE, productCode.ToString()));
                }
                if (!string.IsNullOrEmpty(productBarcode))
                {
                    searchParameters.Add(new KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>(PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.BARCODE, productBarcode.ToString()));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    searchParameters.Add(new KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>(PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.DESCRIPTION, description.ToString()));
                }
                if (!string.IsNullOrEmpty(isPurchaseable))
                {
                    searchParameters.Add(new KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>(PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.INVENTORYITEMSONLY, isPurchaseable.ToString()));
                }
                if (!string.IsNullOrEmpty(symbol))
                {
                    filterText = "(CurrentInventoryQuantity " + symbol + quantity + ")";
                }

                PhysicalCountReviewList physicalCountReviewsList = new PhysicalCountReviewList(executionContext);
                IPhysicalCountInventoryUseCases physicalCountInventoryUseCases = InventoryUseCaseFactory.GetPhysicalCountReviewsUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalPhysicalCountInventory = await physicalCountInventoryUseCases.GetPhysicalCountInventoryCounts(searchParameters, filterText, advancedSearch, physicalCountId, physicalCountStartDate, locationId,null);
                log.LogVariableState("totalPhysicalCountInventory", totalPhysicalCountInventory);
                totalNoOfPages = (totalPhysicalCountInventory / pageSize) + ((totalPhysicalCountInventory % pageSize) > 0 ? 1 : 0);

                List<PhysicalCountReviewDTO> physicalCountReviewsDTOList = await physicalCountInventoryUseCases.GetPhysicalCountReviews(searchParameters, advancedSearch, filterText, physicalCountId, physicalCountStartDate, locationId, ismodifiedDuringPhysicalCount,currentPage, pageSize, null);
                log.LogMethodExit(physicalCountReviewsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = physicalCountReviewsDTOList, currentPageNo = currentPage, TotalCount = totalPhysicalCountInventory });
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
        /// Update the JSON Object InventoryPhysicalCountDTO
        /// </summary>
        /// <param name="physicalCountReviewDTOList">inventoryPhysicalCountDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/{physicalCountId}/PhysicalCountAdjustments")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<PhysicalCountReviewDTO> physicalCountReviewDTOList, [FromUri]int physicalCountId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(physicalCountReviewDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (physicalCountReviewDTOList == null || physicalCountReviewDTOList.Any(a => a.LocationID < 0))
                {
                    log.LogMethodExit(physicalCountReviewDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IPhysicalCountInventoryUseCases physicalCountInventoryUseCases = InventoryUseCaseFactory.GetPhysicalCountReviewsUseCases(executionContext);
                await physicalCountInventoryUseCases.SavePhysicalCountReviews(physicalCountReviewDTOList, physicalCountId);

                log.LogMethodExit(physicalCountReviewDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = physicalCountReviewDTOList });
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

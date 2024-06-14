﻿/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Inventory Adjustments .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    14-Dec-2020  Mushahid Faizan         Created.
 *2.110.0   29-Dec-2020   Abhishek                Modified: added factory references  
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryAdjustmentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryAdjustments.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Adjustments")]
        public async Task<HttpResponseMessage> Get(string productCode = null, string description = null, string productBarcode = null, 
                                                   int locationId = -1, string purchaseable = null, string advancedSearch = null, string pivotColumns = null, 
                                                   bool ignoreWastage = true, int currentPage = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(currentPage, pageSize);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

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
                int totalNoOfPages = 0;
                int totalInventoryAdjustmentsCount = await Task<int>.Factory.StartNew(() => { return inventoryAdjustmentsList.GetInventoryAdjustmentsSummaryCount(searchParameters, advancedSearch, pivotColumns); });
                log.LogVariableState("totalCount", totalInventoryAdjustmentsCount);
                totalNoOfPages = (totalInventoryAdjustmentsCount / pageSize) + ((totalInventoryAdjustmentsCount % pageSize) > 0 ? 1 : 0);

                IInventoryAdjustmentsUseCases inventoryAdjustmentsUseCases = InventoryUseCaseFactory.GetInventoryAdjustmentsUseCases(executionContext);
                List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOList = await inventoryAdjustmentsUseCases.GetInventoryAdjustmentsSummary(searchParameters, advancedSearch, pivotColumns, ignoreWastage, currentPage, pageSize);
                log.LogMethodExit(inventoryAdjustmentsSummaryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryAdjustmentsSummaryDTOList, currentPageNo = currentPage, TotalCount = totalInventoryAdjustmentsCount });
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
        /// Post the JSON Object InventoryAdjustmentsDTO
        /// </summary>
        /// <param name="inventoryAdjustmentsDTOList">inventoryAdjustmentsDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Inventory/Adjustments")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryAdjustmentsDTO> inventoryAdjustmentsDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryAdjustmentsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (inventoryAdjustmentsDTOList == null || inventoryAdjustmentsDTOList.Any(a => a.AdjustmentId > 0))
                {
                    log.LogMethodExit(inventoryAdjustmentsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryAdjustmentsUseCases inventoryAdjustmentsUseCases = InventoryUseCaseFactory.GetInventoryAdjustmentsUseCases(executionContext);
                await inventoryAdjustmentsUseCases.SaveInventoryAdjustments(inventoryAdjustmentsDTOList);

                log.LogMethodExit(inventoryAdjustmentsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryAdjustmentsDTOList });
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
        /// Update the JSON Object InventoryAdjustmentsDTO
        /// </summary>
        /// <param name="inventoryAdjustmentsDTOList">inventoryAdjustmentsDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/Adjustments")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<InventoryAdjustmentsDTO> inventoryAdjustmentsDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryAdjustmentsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (inventoryAdjustmentsDTOList == null || inventoryAdjustmentsDTOList.Any(a => a.AdjustmentId < 0))
                {
                    log.LogMethodExit(inventoryAdjustmentsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryAdjustmentsUseCases inventoryAdjustmentsUseCases = InventoryUseCaseFactory.GetInventoryAdjustmentsUseCases(executionContext);
                await inventoryAdjustmentsUseCases.SaveInventoryAdjustments(inventoryAdjustmentsDTOList);
                log.LogMethodExit(inventoryAdjustmentsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryAdjustmentsDTOList });
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

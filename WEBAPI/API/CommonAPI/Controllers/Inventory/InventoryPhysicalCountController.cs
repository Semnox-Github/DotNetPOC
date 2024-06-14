/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Inventory PhysicalCount .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    14-Dec-2020  Mushahid Faizan         Created
 *2.110.0    04-Jan-2021  Abhishek                Modified : build logic 
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
    public class InventoryPhysicalCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryPhysicalCount.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/PhysicalCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int inventoryPhysicalCountId = -1, int locationId = -1, string name = null, string status = null,
                                                     DateTime? fromDate = null, DateTime? toDate = null, int currentPage = 0, int pageSize = 10)
        {

            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, inventoryPhysicalCountId, currentPage, pageSize);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters = new List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(name))
                {
                    searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.NAME, name.ToString()));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.STATUS, status.ToString()));
                }
                if (inventoryPhysicalCountId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.PHYSICAL_COUNT_ID, inventoryPhysicalCountId.ToString()));
                }
                if (locationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.LOCATIONID, locationId.ToString()));
                }

                if (fromDate != null)
                {
                    DateTime physicalCountFromDate = Convert.ToDateTime(fromDate);
                    searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.START_DATE, physicalCountFromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (toDate != null)
                {
                    DateTime physicalCountToDate = Convert.ToDateTime(toDate);
                    searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.END_DATE, physicalCountToDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                InventoryPhysicalCountList inventoryPhysicalCountList = new InventoryPhysicalCountList(executionContext);
                IInventoryPhysicalCountUseCases inventoryPhysicalCountUseCases = InventoryUseCaseFactory.GetInventoryPhysicalCountsUseCases(executionContext);

                int totalNoOfPages = 0;
                int totalPhysicalCount = await inventoryPhysicalCountUseCases.GetInventoryPhysicalCount(searchParameters, null);
                log.LogVariableState("totalPhysicalCount", totalPhysicalCount);
                totalNoOfPages = (totalPhysicalCount / pageSize) + ((totalPhysicalCount % pageSize) > 0 ? 1 : 0);

                List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList = await inventoryPhysicalCountUseCases.GetInventoryPhysicalCounts(searchParameters, currentPage, pageSize);
                log.LogMethodExit(inventoryPhysicalCountDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryPhysicalCountDTOList, currentPageNo = currentPage, TotalCount = totalPhysicalCount });

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
        /// Post the JSON Object InventoryPhysicalCountDTO
        /// </summary>
        /// <param name="inventoryPhysicalCountDTOList">inventoryPhysicalCountDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Inventory/PhysicalCounts")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryPhysicalCountDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (inventoryPhysicalCountDTOList == null || inventoryPhysicalCountDTOList.Any(a => a.PhysicalCountID > 0))
                {
                    log.LogMethodExit(inventoryPhysicalCountDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryPhysicalCountUseCases inventoryPhysicalCountUseCases = InventoryUseCaseFactory.GetInventoryPhysicalCountsUseCases(executionContext);
                List<InventoryPhysicalCountDTO> savedInventoryPhysicalCountDTOList = await inventoryPhysicalCountUseCases.SaveInventoryPhysicalCounts(inventoryPhysicalCountDTOList);

                log.LogMethodExit(savedInventoryPhysicalCountDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = savedInventoryPhysicalCountDTOList });
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
        /// <param name="inventoryPhysicalCountDTOList">inventoryPhysicalCountDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/PhysicalCounts")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryPhysicalCountDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (inventoryPhysicalCountDTOList == null || inventoryPhysicalCountDTOList.Any(a => a.PhysicalCountID < 0))
                {
                    log.LogMethodExit(inventoryPhysicalCountDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryPhysicalCountUseCases inventoryPhysicalCountUseCases = InventoryUseCaseFactory.GetInventoryPhysicalCountsUseCases(executionContext);
                await inventoryPhysicalCountUseCases.SaveInventoryPhysicalCounts(inventoryPhysicalCountDTOList);

                log.LogMethodExit(inventoryPhysicalCountDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryPhysicalCountDTOList });
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

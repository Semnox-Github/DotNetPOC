/********************************************************************************************
 * Project Name -InventoryWastageCount Controller
 * Description  - Created InventoryWastageCount Controller
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
using System.Globalization;
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
    public class InventoryWastageCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the inventoryWastageSummaryList.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/WastageCounts")]
        public async Task<HttpResponseMessage> Get(DateTime? fromDate = null, DateTime? toDate = null, bool buildHistory = false, string category = null,
                                                  int productId = -1, int inventoryId = -1, int locationId = -1, string productCode = null,
                                                  string productDescription = null, int currentPage = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(category, productDescription);

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters = new List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (buildHistory)
                {
                    if (fromDate == null && toDate == null)
                    {
                        DateTime wastageFromDate = ServerDateTime.Now.AddDays(-1);
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE, wastageFromDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_TO_DATE, ServerDateTime.Now.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    if (fromDate != null && toDate != null)
                    {
                        DateTime wastageFromDate = Convert.ToDateTime(fromDate);
                        DateTime wastageToDate = Convert.ToDateTime(toDate);
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE, wastageFromDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_TO_DATE, wastageToDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    if (!string.IsNullOrEmpty(category))
                    {
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.CATEGORY, category));
                    }
                    if (!string.IsNullOrEmpty(productDescription))
                    {
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.PRODUCT_DESCRIPTION, productDescription));
                    }

                    if (!string.IsNullOrEmpty(productCode))
                    {
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.CODE, productCode));
                    }
                    if (locationId > -1)
                    {
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.LOCATION_ID, locationId.ToString()));
                    }
                    if (inventoryId > -1)
                    {
                        searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.INVENTORY_ID, inventoryId.ToString()));
                    }
                }
                else
                {
                    DateTime wastageFromDate = ServerDateTime.Now;
                    double businessStartTime = 6;
                    try
                    {
                        businessStartTime = ParafaitDefaultContainerList.GetParafaitDefault<double>(executionContext, "BUSINESS_DAY_START_TIME", 6);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        businessStartTime = 6;
                    }
                    if (wastageFromDate.Hour >= 0 && wastageFromDate.Hour < businessStartTime)
                    {
                        wastageFromDate = wastageFromDate.AddDays(-1);
                    }
                    wastageFromDate = wastageFromDate.Date.AddHours(businessStartTime);
                    searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE, wastageFromDate.Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                }

                IInventoryWastageUseCases inventoryWastageUseCases = InventoryUseCaseFactory.GetInventoryWastagesUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfInventoryWastages = await inventoryWastageUseCases.GetInventoryWastageCount(searchParameters);
                log.LogVariableState("totalNoOfInventoryWastages", totalNoOfInventoryWastages);
                totalNoOfPages = (totalNoOfInventoryWastages / pageSize) + ((totalNoOfInventoryWastages % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfInventoryWastages);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfInventoryWastages, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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

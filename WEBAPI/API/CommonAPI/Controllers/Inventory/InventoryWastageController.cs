/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Inventory Wastage Summary .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    14-Dec-2020  Mushahid Faizan         Created.
 *2.110.0    28-Dec-2020  Abhishek                Modified : added factory references. 
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryWastageController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the inventoryWastageSummaryList.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Wastages")]
        public async Task<HttpResponseMessage> Get(DateTime? fromDate = null, DateTime? toDate = null, bool buildHistory = false, string category = null,
                                                  int productId = -1, int inventoryId = -1, int locationId = -1, string productCode = null,
                                                  string productDescription = null, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(category, productDescription);

                // ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

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
                int totalNoOfPages = 0;
                int totalNoOfInventoryWastages = await Task<int>.Factory.StartNew(() => { return inventoryAdjustmentsList.GetInventoryWastagesCount(searchParameters, null); });
                log.LogVariableState("totalNoOfInventoryWastages", totalNoOfInventoryWastages);
                totalNoOfPages = (totalNoOfInventoryWastages / pageSize) + ((totalNoOfInventoryWastages % pageSize) > 0 ? 1 : 0);

                IInventoryWastageUseCases inventoryWastageUseCases = InventoryUseCaseFactory.GetInventoryWastagesUseCases(executionContext);
                List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = await inventoryWastageUseCases.GetInventoryWastages(searchParameters, currentPage, pageSize);
                log.LogMethodExit(inventoryWastageSummaryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryWastageSummaryDTOList, currentPageNo = currentPage, TotalCount = totalNoOfInventoryWastages });
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

        /// <summary>
        /// Post the JSON Object of InventoryWastageSummaryDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Wastages")]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryWastageSummaryDTOList);
                // ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (inventoryWastageSummaryDTOList == null || inventoryWastageSummaryDTOList.Any(x => x.AdjustmentId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (inventoryWastageSummaryDTOList != null && inventoryWastageSummaryDTOList.Any())
                {
                    IInventoryWastageUseCases inventoryWastageUseCases = InventoryUseCaseFactory.GetInventoryWastagesUseCases(executionContext);
                    await inventoryWastageUseCases.SaveInventoryWastages(inventoryWastageSummaryDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryWastageSummaryDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            //catch (ValidationException valEx)
            //{
            //    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
            //    log.Error(customException);
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            //}
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }

        /// <summary>
        /// Put the JSON Object of InventoryWastageSummaryDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>   
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/Wastages")]
        public async Task<HttpResponseMessage> Put([FromBody] List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryWastageSummaryDTOList);
               // ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (inventoryWastageSummaryDTOList == null || inventoryWastageSummaryDTOList.Any(x => x.AdjustmentId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (inventoryWastageSummaryDTOList != null && inventoryWastageSummaryDTOList.Any())
                {
                    IInventoryWastageUseCases inventoryWastageUseCases = InventoryUseCaseFactory.GetInventoryWastagesUseCases(executionContext);
                    await inventoryWastageUseCases.SaveInventoryWastages(inventoryWastageSummaryDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryWastageSummaryDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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

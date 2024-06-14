/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the  Issue Headers .
 *  
 **************
 **Version Log
 **************
 *Version            Date                Created By               Remarks          
 ***************************************************************************************************
 *2.110.0            15-Dec-2020         Mushahid Faizan           Created.
 *2.150.0            20-Oct-2022         Abhishek                  Modified : Added search parameters Fromdate, Todate and Issue Number
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

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryIssueController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryIssueHeaders.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Issues")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int inventoryIssueId = -1, int documentTypeId = -1, int purchaseOrderId = -1, int requisitionId = -1,
                                                    string status = null, bool buildChildRecords = false, bool loadActiveChild = false, string guidIdList = null, DateTime? fromDate = null,
                                                    DateTime? toDate = null, string issueNumber = null, int currentPage = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, inventoryIssueId, documentTypeId, purchaseOrderId, requisitionId, status, buildChildRecords, loadActiveChild, fromDate,
                                   toDate, issueNumber, currentPage, pageSize);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters = new List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(status))
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.STATUS, status.ToString()));
                }
                if (inventoryIssueId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.INVENTORY_ISSUE_ID, inventoryIssueId.ToString()));
                }
                if (documentTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.DOCUMENT_TYPE_ID, documentTypeId.ToString()));
                }
                if (purchaseOrderId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.PURCHASE_ORDER_ID, purchaseOrderId.ToString()));
                }
                if (requisitionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.REQUISITION_ID, requisitionId.ToString()));
                }

                if (!string.IsNullOrEmpty(guidIdList))
                {

                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<string> guidList = new List<string>();

                    guidList = guidIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).ToList();

                    String guidIdListString = String.Join(",", guidList.ToArray());

                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.GUID_ID_LIST, guidIdListString));
                }
                if (fromDate != null && toDate != null)
                {
                    DateTime issueFromDate = Convert.ToDateTime(fromDate);
                    DateTime issueToDate = Convert.ToDateTime(toDate);
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_FROM_DATE, issueFromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_TO_DATE, issueToDate.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                }
                if (!string.IsNullOrEmpty(issueNumber))
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_NUMBER, issueNumber));
                }

                InventoryIssueHeaderList inventoryIssueHeadersList = new InventoryIssueHeaderList();

                int totalNoOfPages = 0;
                int totalCount = await Task<int>.Factory.StartNew(() => { return inventoryIssueHeadersList.GetInventoryIssueHeaderCount(searchParameters, null); });
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

                IInventoryIssueHeaderUseCases inventoryIssueHeadersUseCases = InventoryUseCaseFactory.GetInventoryIssueHeadersUseCases(executionContext);
                List<InventoryIssueHeaderDTO> inventoryIssueHeadersDTOList = await inventoryIssueHeadersUseCases.GetInventoryIssueHeaders(searchParameters, currentPage, pageSize, buildChildRecords, loadActiveChild);

                log.LogMethodExit(inventoryIssueHeadersDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryIssueHeadersDTOList, currentPageNo = currentPage, TotalCount = totalCount });

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
        /// Post the JSON Object InventoryIssueHeaderDTO
        /// </summary>
        /// <param name="inventoryIssueHeadersDTOList">inventoryIssueHeadersDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Inventory/Issues")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryIssueHeaderDTO> inventoryIssueHeadersDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryIssueHeadersDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<InventoryIssueHeaderDTO> savedInventoryIssueHeaderDTOList = new List<InventoryIssueHeaderDTO>();
                if (inventoryIssueHeadersDTOList == null)
                {
                    log.LogMethodExit(inventoryIssueHeadersDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryIssueHeaderUseCases inventoryIssueHeadersUseCases = InventoryUseCaseFactory.GetInventoryIssueHeadersUseCases(executionContext);
                savedInventoryIssueHeaderDTOList = await inventoryIssueHeadersUseCases.SaveInventoryIssueHeaders(inventoryIssueHeadersDTOList);

                log.LogMethodExit(savedInventoryIssueHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = savedInventoryIssueHeaderDTOList });
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
        /// Update the JSON Object InventoryIssueHeaderDTO
        /// </summary>
        /// <param name="inventoryIssueHeadersDTOList">inventoryIssueHeadersDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/Issues")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<InventoryIssueHeaderDTO> inventoryIssueHeadersDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryIssueHeadersDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<InventoryIssueHeaderDTO> savedInventoryIssueHeaderDTOList = new List<InventoryIssueHeaderDTO>();
                if (inventoryIssueHeadersDTOList == null || inventoryIssueHeadersDTOList.Any(a => a.InventoryIssueId < 0))
                {
                    log.LogMethodExit(inventoryIssueHeadersDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryIssueHeaderUseCases inventoryIssueHeadersUseCases = InventoryUseCaseFactory.GetInventoryIssueHeadersUseCases(executionContext);
                savedInventoryIssueHeaderDTOList = await inventoryIssueHeadersUseCases.SaveInventoryIssueHeaders(inventoryIssueHeadersDTOList);

                log.LogMethodExit(savedInventoryIssueHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = savedInventoryIssueHeaderDTOList });
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

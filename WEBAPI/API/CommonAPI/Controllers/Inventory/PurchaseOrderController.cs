/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Purchase Orders .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    09-Dec-2020  Mushahid Faizan         Created.
 *2.150.0    09-Jun-2022  Abhishek                Modified: Web Inventory Redesign - Added search parameter isAutoPO.
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
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the PurchaseOrders.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/PurchaseOrders")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int documentTypeId = -1, int purchaseOrderId = -1, int vendorId = -1, string documentStatus = null,
                                                    string orderStatus = null, string orderNumber = null, bool loadActiveChild = false, bool buildChildRecords = false,
                                                    DateTime? fromDate = null, DateTime? toDate = null, string purchaseOrderIdList = null, string guidIdList = null,
                                                    string isAutoPO = null, int currentPage = 0, int pageSize = 10, bool mostRepeated = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, documentTypeId, purchaseOrderId, vendorId, documentStatus, orderStatus, orderNumber, loadActiveChild, buildChildRecords,
                                   fromDate, toDate, purchaseOrderIdList, guidIdList, isAutoPO, currentPage, pageSize, mostRepeated);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> searchParameters = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>();
                searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(orderStatus))
                {
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERSTATUS, orderStatus.ToString()));
                }
                if (!string.IsNullOrEmpty(orderNumber))
                {
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER, orderNumber.ToString()));
                }
                if (!string.IsNullOrEmpty(documentStatus))
                {
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_STATUS, documentStatus.ToString()));
                }
                if (documentTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_TYPE_ID, documentTypeId.ToString()));
                }
                if (purchaseOrderId > -1)
                {
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID, purchaseOrderId.ToString()));
                }
                if (vendorId > -1)
                {
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.VENDORID, vendorId.ToString()));
                }
                if (fromDate != null && toDate != null)
                {
                    DateTime poFromDate = Convert.ToDateTime(fromDate);
                    DateTime poToDate = Convert.ToDateTime(toDate);
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.FROM_DATE, poFromDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.TO_DATE, poToDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                }

                if (!string.IsNullOrEmpty(purchaseOrderIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> poIdList = new List<int>();

                    poIdList = purchaseOrderIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String purchaseOrderListString = String.Join(",", poIdList.ToArray());
                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDER_ID_LIST, purchaseOrderListString));
                }

                if (!string.IsNullOrEmpty(guidIdList))
                {

                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<string> guidList = new List<string>();

                    guidList = guidIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).ToList();

                    String guidIdListString = String.Join(",", guidList.ToArray());

                    searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.GUID_ID_LIST, guidIdListString));
                }
                if (string.IsNullOrEmpty(isAutoPO) == false)
                {
                    if (isAutoPO.ToString() == "1" || isAutoPO.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.IS_AUTO_PO, isAutoPO.ToString()));
                    }
                }
                PurchaseOrderList purchaseOrderList = new PurchaseOrderList(executionContext);

                int totalNoOfPages = 0;
                int totalCount = await Task<int>.Factory.StartNew(() => { return purchaseOrderList.GetPurchaseOrderCount(searchParameters, null); });
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

                IPurchaseOrderUseCases purchaseOrderUseCases = InventoryUseCaseFactory.GetPurchaseOrderUseCases(executionContext);
                List<PurchaseOrderDTO> purchaseOrderDTOList = await purchaseOrderUseCases.GetPurchaseOrders(searchParameters, buildChildRecords, loadActiveChild, currentPage, pageSize, mostRepeated);

                log.LogMethodExit(purchaseOrderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = purchaseOrderDTOList, currentPageNo = currentPage, TotalCount = totalCount });
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
        /// Post the JSON Object PurchaseOrderDTO
        /// </summary>
        /// <param name="purchaseOrderDTOList">purchaseOrderDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/PurchaseOrders")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<PurchaseOrderDTO> purchaseOrderDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(purchaseOrderDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (purchaseOrderDTOList == null)
                {
                    log.LogMethodExit(purchaseOrderDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IPurchaseOrderUseCases purchaseOrderUseCases = InventoryUseCaseFactory.GetPurchaseOrderUseCases(executionContext);
                await purchaseOrderUseCases.SavePurchaseOrders(purchaseOrderDTOList);

                log.LogMethodExit(purchaseOrderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = purchaseOrderDTOList });
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
        /// Update the JSON Object PurchaseOrderDTO
        /// </summary>
        /// <param name="purchaseOrderDTOList">purchaseOrderDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/PurchaseOrders")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<PurchaseOrderDTO> purchaseOrderDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(purchaseOrderDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
               
                if (purchaseOrderDTOList == null || purchaseOrderDTOList.Any(a => a.PurchaseOrderId < 0))
                {
                    log.LogMethodExit(purchaseOrderDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IPurchaseOrderUseCases purchaseOrderUseCases = InventoryUseCaseFactory.GetPurchaseOrderUseCases(executionContext);
                await purchaseOrderUseCases.SavePurchaseOrders(purchaseOrderDTOList);

                log.LogMethodExit(purchaseOrderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = purchaseOrderDTOList });
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

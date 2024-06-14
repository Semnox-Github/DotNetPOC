/********************************************************************************************
 * Project Name - PurchaseOrderCount Controller
 * Description  - Created PurchaseOrderCount Controller
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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the PurchaseOrders.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/PurchaseOrderCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int documentTypeId = -1, int purchaseOrderId = -1, int vendorId = -1, string documentStatus = null,
                                                    string orderStatus = null, string orderNumber = null, int currentPage = 0, int pageSize = 10)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, documentTypeId, purchaseOrderId, vendorId, currentPage, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

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

                PurchaseOrderList purchaseOrderList = new PurchaseOrderList(executionContext);
                IPurchaseOrderUseCases purchaseOrderUseCases = InventoryUseCaseFactory.GetPurchaseOrderUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalCount = await purchaseOrderUseCases.GetPurchaseOrderCount(searchParameters);
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalCount);

                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalCount, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
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
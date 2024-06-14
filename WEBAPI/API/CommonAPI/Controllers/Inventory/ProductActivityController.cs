/********************************************************************************************
* Project Name - CommonAPI
* Description  - ProductActivityController - Created to get the Product Activity
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.110.00   31-Dec-2020     Abhishek              Created : As part of Inventory UI Redesign      
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Parafait.Inventory;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Inventory.Requisition;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Products
{
    public class ProductActivityController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of ProductActivityViewDTO
        /// </summary>
        /// <param name="locationId">locationId</param>
        /// <param name="productId">productId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/ProductActivities")]
        public async Task<HttpResponseMessage> Get(int locationId = -1, int productId = -1, int lotId = -1, int currentPage = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(locationId, productId, currentPage, pageSize);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ProductActivityViewList productActivityViewList = new ProductActivityViewList(executionContext);

                int totalNoOfPages = 0;
                int totalNoOfproductActivities = await Task<int>.Factory.StartNew(() => { return productActivityViewList.GetProductActivityCount(locationId, productId, lotId, null); });
                log.LogVariableState("totalNoOfproductActivities", totalNoOfproductActivities);
                totalNoOfPages = (totalNoOfproductActivities / pageSize) + ((totalNoOfproductActivities % pageSize) > 0 ? 1 : 0);

                IProductActivityUseCases productActivityUseCases = InventoryUseCaseFactory.GetProductActivityUseCases(executionContext);
                List<ProductActivityViewDTO> productActivityViewDTOList = await productActivityUseCases.GetProductActivities(locationId, productId, lotId, currentPage, pageSize, null);
                log.LogMethodExit(productActivityViewDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productActivityViewDTOList, currentPageNo = currentPage, TotalCount = totalNoOfproductActivities }); 
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
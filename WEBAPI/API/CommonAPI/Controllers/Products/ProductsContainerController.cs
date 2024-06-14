/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the Products controller.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 2.110.0       14-Dec-2020   Deeksha              Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Products
{
    public class ProductsContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductsContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, int machineId = -1, string manualType = null, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, rebuildCache, hash);
                ProductsContainerDTOCollection productsContainerDTOCollection = await
                           Task<ProductsContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               if (rebuildCache)
                               {
                                   ProductViewContainerList.Rebuild(siteId, manualType);
                               }
                               return ProductViewContainerList.GetProductsContainerDTOCollection(siteId, manualType, hash);
                           });
                log.LogMethodExit(productsContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsContainerDTOCollection });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}

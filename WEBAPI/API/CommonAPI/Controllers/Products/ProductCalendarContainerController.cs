/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Controller class for price container
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.1      10-Aug-2021      Lakshminarayana           Created : Static menu enhancement
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
    public class ProductCalendarContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductsCalenderContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string manualProductType = null, DateTime? startDateTime = null, DateTime? endDateTime = null, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, rebuildCache, hash);
                if (string.IsNullOrWhiteSpace(manualProductType) ||
                    startDateTime.HasValue == false ||
                    endDateTime.HasValue == false ||
                   startDateTime.Value >= endDateTime.Value)
                {
                    log.LogMethodExit(null, "Bad Request");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                ProductCalendarContainerDTOCollection productCalendarContainerDTOCollection = await
                           Task<ProductCalendarContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               if (rebuildCache)
                               {
                                   ProductViewContainerList.Rebuild(siteId, manualProductType);
                               }
                               return ProductViewContainerList.GetProductCalendarContainerDTOCollection(siteId, manualProductType, startDateTime.Value, endDateTime.Value, hash);
                           });
                log.LogMethodExit(productCalendarContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productCalendarContainerDTOCollection });
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

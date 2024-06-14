/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the Product Price container DTO List controller.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 2.110.0       14-Dec-2020   Deeksha              Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.ProductPrice;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Products
{
    public class ProductPriceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductPrice")]
        public async Task<HttpResponseMessage> Get(string menuType= null, int membershipId = -1, int transactionProfileId = -1, DateTime? dateTime = null)
        {
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                log.LogMethodEntry(menuType, membershipId, transactionProfileId, dateTime);
                if (ProductMenuType.IsValid(menuType) == false ||
                    dateTime.HasValue == false)
                {
                    log.LogMethodExit(null, "Bad Request");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                List<ProductsPriceContainerDTO> result = await
                           Task<List<ProductsPriceContainerDTO>>.Factory.StartNew(() =>
                           {
                               return ProductPriceViewContainerList.GetProductsPriceContainerDTOList(executionContext, menuType, membershipId, transactionProfileId, dateTime.Value);
                           });
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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

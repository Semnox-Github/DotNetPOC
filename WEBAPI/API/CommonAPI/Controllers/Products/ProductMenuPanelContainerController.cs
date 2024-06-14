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
using System.Collections.Generic;
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
    public class ProductMenuPanelContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/MenuPanelContainer")]
        public async Task<HttpResponseMessage> Get(string menuType = null, DateTime? visitDateTime = null)
        {
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                log.LogMethodEntry(menuType, visitDateTime);
                if (ProductMenuType.IsValid(menuType) == false)
                {
                    log.LogMethodExit(null, "Bad Request");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList = await
                           Task<List<ProductMenuPanelContainerDTO>>.Factory.StartNew(() =>
                           {
                               DateTime dateTime = visitDateTime.HasValue? visitDateTime.Value : DateTime.Now;
                               return ProductMenuViewContainerList.GetProductMenuPanelContainerDTOList(executionContext, menuType, dateTime);
                           });
                log.LogMethodExit(productMenuPanelContainerDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productMenuPanelContainerDTOList });
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

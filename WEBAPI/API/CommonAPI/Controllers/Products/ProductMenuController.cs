/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - product menu controller
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      20-Jul-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Product
{
    public class ProductMenuController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object productmenu List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/Menus")]
        public async Task<HttpResponseMessage> Get(string isActive = null,
                                                   int menuId = -1,
                                                   string name = "",
                                                   DateTime? endDateGreaterThanEqualTo = null,
                                                   DateTime? startDateLessThanEqualTo = null,
                                                   int siteId = -1,
                                                   bool loadChildRecords = true,
                                                   bool loadActiveChildRecords = true)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(executionContext);
                List<ProductMenuDTO> productMenuDTOList = await productMenuUseCases.GetProductMenuDTOList(isActive, menuId, name, endDateGreaterThanEqualTo, startDateLessThanEqualTo, siteId, loadChildRecords, loadActiveChildRecords);
                log.LogMethodExit(productMenuDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = productMenuDTOList,
                });
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
        /// <summary>
        /// Performs a Post operation on productmenus
        /// </summary>         
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Product/Menus")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ProductMenuDTO> productMenuDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(executionContext);
                List<ProductMenuDTO> savedProductMenuDTOList = await productMenuUseCases.SaveProductMenuDTOList(productMenuDTOList);
                log.LogMethodExit(savedProductMenuDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = savedProductMenuDTOList,
                });
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


        /// <summary>
        /// Performs a Put operation on productmenus
        /// </summary>         
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/Product/Menus")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<ProductMenuDTO> productMenuDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (productMenuDTOList == null || 
                    productMenuDTOList.Any(a => a.MenuId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(executionContext);
                List<ProductMenuDTO> savedProductMenuDTOList = await productMenuUseCases.SaveProductMenuDTOList(productMenuDTOList);
                log.LogMethodExit(savedProductMenuDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = savedProductMenuDTOList,
                });
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

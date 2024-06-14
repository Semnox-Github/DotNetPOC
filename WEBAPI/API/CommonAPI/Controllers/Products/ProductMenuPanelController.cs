/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - product menu panel controller
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
    public class ProductMenuPanelController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object productmenuPanels
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/MenuPanels")]
        public async Task<HttpResponseMessage> Get(string isActive = null,
                                                    int panelId = -1,
                                                    string name = "",
                                                    int siteId = -1,
                                                    bool loadChildRecords = true,
                                                    bool loadActiveChildRecords = true,
                                                    string guid = "")
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(executionContext);
                List<ProductMenuPanelDTO> productMenuPanelDTOList = await productMenuUseCases.GetProductMenuPanelDTOList(isActive, panelId, name, siteId, loadChildRecords, loadActiveChildRecords, guid);
                log.LogMethodExit(productMenuPanelDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = productMenuPanelDTOList,
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
        /// Performs a Post operation on productmenuPanels
        /// </summary>         
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Product/MenuPanels")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ProductMenuPanelDTO> productMenuPanelDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(executionContext);
                List<ProductMenuPanelDTO> savedProductMenuPanelDTOList = await productMenuUseCases.SaveProductMenuPanelDTOList(productMenuPanelDTOList);
                log.LogMethodExit(savedProductMenuPanelDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = savedProductMenuPanelDTOList,
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
        /// Performs a PUT operation on productmenuPanels
        /// </summary>         
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/Product/MenuPanels")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<ProductMenuPanelDTO> productMenuPanelDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (productMenuPanelDTOList == null ||
                    productMenuPanelDTOList.Any(a => a.PanelId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(executionContext);
                List<ProductMenuPanelDTO> savedProductMenuPanelDTOList = await productMenuUseCases.SaveProductMenuPanelDTOList(productMenuPanelDTOList);
                log.LogMethodExit(savedProductMenuPanelDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = savedProductMenuPanelDTOList,
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

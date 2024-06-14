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
    public class AddProductMenuPanelContentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Performs a Post operation on productmenuPanels
        /// </summary>         
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Product/MenuPanels/Contents")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int panelId, [FromBody] List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(executionContext);
                string result = await productMenuUseCases.AddProductMenuPanelContentDTOList(panelId, productMenuPanelContentDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
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

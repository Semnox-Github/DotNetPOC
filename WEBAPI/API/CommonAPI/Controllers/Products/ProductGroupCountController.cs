/******************************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch, update and inserts for Product Group Entity.
 *  
 **************
 **Version Log
 **************
 *Version   Date            Modified By               Remarks          
 ******************************************************************************************************
 *2.170     20-Aug-2023     Lakshminarayana           Created
 *******************************************************************************************************/

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

namespace Semnox.CommonAPI.Controllers.Product
{
    public class ProductGroupCountController : ApiController
    {
        private Semnox.Parafait.logging.Logger log;

        /// <summary>
        /// Get the JSON ProductGroup Setup.
        /// </summary>
        /// <param name="discountType">discountType i.e., T for Transaction G for Game Play and L for Loyalty</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductGroupsCount")]
        public async Task<HttpResponseMessage> Get(int id = -1,
                                                   string name = "",
                                                   string isActive = null,
                                                   int siteId = -1)
        {
            ExecutionContext executionContext = null;

            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.LogMethodEntry(id, name, isActive, siteId);


                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductsUseCases productUseCases = ProductsUseCaseFactory.GetProductUseCases(executionContext,
                    RequestIdentifierHelper.GetRequestIdentifier(Request));
                int result = await productUseCases.GetProductGroupDTOListCount(id, name, isActive, siteId);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
                });
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
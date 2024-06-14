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
    public class ProductGroupController : ApiController
    {
        private Semnox.Parafait.logging.Logger log;

        /// <summary>
        /// Get the JSON ProductGroup Setup.
        /// </summary>
        /// <param name="discountType">discountType i.e., T for Transaction G for Game Play and L for Loyalty</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductGroups")]
        public async Task<HttpResponseMessage> Get(int id = -1,
                                                   string name = "",
                                                   string isActive = null,
                                                   int siteId = -1,
                                                   bool loadChildRecords = true,
                                                   bool loadActiveChildRecords = true,
                                                   int pageNumber = 0,
                                                   int pageSize = 0)
        {
            ExecutionContext executionContext = null;

            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.LogMethodEntry(id, name, isActive, siteId, loadChildRecords, loadActiveChildRecords, pageNumber,
                                   pageSize);


                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductsUseCases productUseCases = ProductsUseCaseFactory.GetProductUseCases(executionContext,
                   RequestIdentifierHelper.GetRequestIdentifier(Request));
                int result = await productUseCases.GetProductGroupDTOListCount(id, name, isActive, siteId);
                List<ProductGroupDTO> productGroupDTOList = await productUseCases.GetProductGroupDTOList(id, name, isActive, siteId, loadChildRecords, loadActiveChildRecords, pageNumber,
                                   pageSize);
                log.LogMethodExit(productGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = productGroupDTOList,
                    TotalCount = result
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object ProductGroup Setup
        /// </summary>
        /// <param name="<productGroupDTOList">discountDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<ProductGroupDTO> productGroupDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.LogMethodEntry(productGroupDTOList);
                IProductsUseCases prodductUseCases = ProductsUseCaseFactory.GetProductUseCases(executionContext, RequestIdentifierHelper.GetRequestIdentifier(Request));
                List<ProductGroupDTO> savedProductGroupDTOList = await prodductUseCases.SaveProductGroupDTOList(productGroupDTOList);
                log.LogMethodExit(savedProductGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = savedProductGroupDTOList,
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
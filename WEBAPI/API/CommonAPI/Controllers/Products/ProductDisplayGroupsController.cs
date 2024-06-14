/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert product display groups in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        10-Jan-2019   Jagan Mohana Rao          Created to get, insert, update and Delete Methods.
 *********************************************************************************************
 *2.60        18-Mar-2019   Akshay Gulaganji          Modified Response Message for Post and Delete method
 *********************************************************************************************
 *2.70        29-June-2019  Indrajeet Kumar           Modified Delete - Implemented Hard Deletion 
 *2.100.0     10-Sep-2020   Vikas Dwivedi             Modified as per the REST API Standards.
 2.120.00     06-Apr-2021   B Mahesh Pai             Modified Get,Post Delete and Added Put method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Linq;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Products
{
    public class ProductDisplayGroupsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON products modifiers.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductDisplayGroups")]
        public async Task<HttpResponseMessage> Get(int productsDisplayGroupId = -1, int productId = -1, int displayGroupId = -1, string isActive = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsDisplayGroupId, productId, displayGroupId, isActive);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParameters = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (productsDisplayGroupId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.ID, productsDisplayGroupId.ToString()));
                }
                if (productId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, productId.ToString()));
                }
                if (displayGroupId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, displayGroupId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, isActive));
                    }
                }
                IProductDisplayGroupUseCases productDisplayGroupsUseCases = ProductsUseCaseFactory.GetProductsDisplayGroups(executionContext);
                List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList = await productDisplayGroupsUseCases.GetProductsDisplayGroups(searchParameters);
                log.LogMethodExit(productsDisplayGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsDisplayGroupDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Product Display Groups
        /// </summary>
        /// <param name="productsDisplayGroupList">productsDisplayGroupList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductDisplayGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsDisplayGroupDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (productsDisplayGroupDTOList == null )
                {
                    log.LogMethodExit(productsDisplayGroupDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductDisplayGroupUseCases productDisplayGroupsUseCases = ProductsUseCaseFactory.GetProductsDisplayGroups(executionContext);
                await productDisplayGroupsUseCases.SaveProductsDisplayGroups(productsDisplayGroupDTOList);
                log.LogMethodExit(productsDisplayGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsDisplayGroupDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the productsDisplayGroupList collection
        /// <param name="productsDisplayGroupDTOList">productsDisplayGroupList</param>
        [HttpPut]
        [Route("api/Product/ProductDisplayGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(productsDisplayGroupDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (productsDisplayGroupDTOList == null || productsDisplayGroupDTOList.Any(a => a.Id < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductDisplayGroupUseCases productDisplayGroupsUseCases = ProductsUseCaseFactory.GetProductsDisplayGroups(executionContext);
                await productDisplayGroupsUseCases.SaveProductsDisplayGroups(productsDisplayGroupDTOList);
                log.LogMethodExit(productsDisplayGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsDisplayGroupDTOList });
            }

            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Delete the Product Display Groups
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/ProductDisplayGroups")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsDisplayGroupDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (productsDisplayGroupDTOList != null && productsDisplayGroupDTOList.Any())
                {
                    IProductDisplayGroupUseCases productDisplayGroupsUseCases = ProductsUseCaseFactory.GetProductsDisplayGroups(executionContext);
                    productDisplayGroupsUseCases.Delete(productsDisplayGroupDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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

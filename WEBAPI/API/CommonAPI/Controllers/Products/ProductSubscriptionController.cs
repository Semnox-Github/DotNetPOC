/********************************************************************************************
 * Project Name - ProductSubscription
 * Description  - Created to fetch, update and insert ProductSubscription details.   
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By              Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Jan-2021     Guru S A                 Created for Subscription feature
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

namespace Semnox.CommonAPI.Products
{
    /// <summary>
    /// ProductSubscriptionController
    /// </summary>
    public class ProductSubscriptionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Subscription Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductSubscriptions")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int productId = -1, int productSubscriptionId = -1)
        {
            log.LogMethodEntry(isActive, productId, productSubscriptionId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductSubscriptionUseCases productSubscriptionUseCases = ProductsUseCaseFactory.GetProductSubscriptionUseCases(executionContext);
                List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.SiteId)));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (productId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID, productId.ToString()));
                }
                if (productSubscriptionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.ID, productSubscriptionId.ToString()));
                }

               // ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext);
                List<ProductSubscriptionDTO> ProductSubscriptionDTOList = await productSubscriptionUseCases.GetProductSubscription(searchParameters);
                log.LogMethodExit(ProductSubscriptionDTOList); 
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = ProductSubscriptionDTOList 
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
        /// Performs a Post operation on ProductSubscriptionDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Product/ProductSubscriptions")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ProductSubscriptionDTO> productSubscriptionDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IProductSubscriptionUseCases productSubscriptionUseCases = ProductsUseCaseFactory.GetProductSubscriptionUseCases(executionContext);
                //ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext, productSubscriptionDTOList);
                var content = await productSubscriptionUseCases.SaveProductSubscription(productSubscriptionDTOList);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = content
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

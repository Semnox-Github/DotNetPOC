/********************************************************************************************
 * Project Name - Product Details Controller
 * Description  - Created to fetch, update and insert product details in the product details entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        15-Feb-2019   Akshay Gulaganji          Created to Get and Post Methods.
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    /// <summary>
    /// APIController of Product Details
    /// </summary>
    public class ProductDetailsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Gets the Jason Object of Product Details List
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="productType">productType</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/ProductDetails/")]
        public HttpResponseMessage Get(string productId, bool loadChildren = true)
        {
            try
            {
                log.LogMethodEntry(productId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, productId));
                }
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                ProductsList productList = new ProductsList(executionContext);
                var content = productList.GetProductsDTOList(searchParameters, loadChildren);

                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Posts the Product Details List
        /// </summary>
        /// <param name="productDtoList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Products/ProductDetails/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ProductsDTO> productDtoList)
        {
            try
            {
                log.LogMethodEntry(productDtoList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productDtoList != null || productDtoList.Count > 0)
                {
                    ProductsList products = new ProductsList(executionContext, productDtoList);
                    products.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}

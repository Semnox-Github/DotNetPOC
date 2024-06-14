/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert product discounts in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        21-Jan-2019   Jagan Mohana Rao        Created to get, insert, update and Delete Methods.
 *********************************************************************************************
 *2.60        18-Mar-2019   Akshay Gulaganji        Modified isActive Parameter and Added ExecutionContext
 *2.110.0     21-Nov-2019   Girish Kundar           Modified :  REST API changes for Inventory UI redesign
 *2.120.00    30-Mar-2021   Roshan Devadiga         Modified Get,Post Delete
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Product;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System.Linq;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Products
{
    public class ProductDiscountsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON products modifiers.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductDiscounts")]
        public async Task<HttpResponseMessage> Get(string isActive, string productId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, productId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.PRODUCT_ID, productId));
                }
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                IProductDiscountUseCases productDiscountUseCases = ProductsUseCaseFactory.GetProductDiscountUseCases(executionContext);
                List<ProductDiscountsDTO> productDiscountsDTOList = await productDiscountUseCases.GetProductDiscounts(searchParameters);
                log.LogMethodExit(productDiscountsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productDiscountsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Product Discounts
        /// </summary>
        /// <param name="productDiscountsDTOList">productDiscountsDTOs</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductDiscounts")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<ProductDiscountsDTO> productDiscountsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productDiscountsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productDiscountsDTOList != null && productDiscountsDTOList.Any())
                {
                    IProductDiscountUseCases productDiscountUseCases = ProductsUseCaseFactory.GetProductDiscountUseCases(executionContext);
                    await productDiscountUseCases.SaveProductDiscounts(productDiscountsDTOList);
                    log.LogMethodExit(productDiscountsDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = " " });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Delete the Product Discounts
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/ProductDiscounts")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ProductDiscountsDTO> productDiscountsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productDiscountsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productDiscountsDTOList != null && productDiscountsDTOList.Any())
                {
                    IProductDiscountUseCases productDiscountsUseCases = ProductsUseCaseFactory.GetProductDiscountUseCases(executionContext);
                    productDiscountsUseCases.Delete(productDiscountsDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = " " });
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

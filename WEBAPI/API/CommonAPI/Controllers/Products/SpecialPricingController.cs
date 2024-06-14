/********************************************************************************************
 * Project Name - Products Special Pricing Controller
 * Description  - Created to fetch, update and insert Products Special Pricing.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        29-Jan-2019   Akshay Gulaganji          Created to get, insert, update and Delete Methods.
 *2.60        21-Mar-2019   Nagesh Badiger            Added ExecutionContext and added Custom Generic Exception 
 *2.70        29-Jun-2019   Akshay Gulaganji          modified Delete() method
*2.110.0      21-Nov-2020   Girish Kundar             EndPoint changes. 
*2.120.0      14-jun-2021   B Mahesh Pai              Modified Get, Post,Delete and added  Put method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System.Linq;
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Products
{
    public class SpecialPricingController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Products Special Pricing.
        /// </summary>       
        /// <param name="isActive">isActive</param>
        /// <param name="productId">productId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/SpecialPricing")]
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

                List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>> searchParameters = new List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRODUCT_ID, productId));
                }
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.ACTIVE_FLAG, isActive));
                }
                IProductsSpecialPricingUseCases specialPricingUseCases = ProductsUseCaseFactory.GetProductsSpecialPricings(executionContext);
                List<ProductsSpecialPricingDTO> specialPricingDTOList = await specialPricingUseCases.GetProductsSpecialPricings(searchParameters);
                log.LogMethodExit(specialPricingDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = specialPricingDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Products Special Pricing.
        /// </summary>
        /// <param name="productsSpecialPricingList">productsSpecialPricingList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/SpecialPricing")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<ProductsSpecialPricingDTO> productsSpecialPricingList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(productsSpecialPricingList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productsSpecialPricingList == null )
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductsSpecialPricingUseCases specialPricingUseCases = ProductsUseCaseFactory.GetProductsSpecialPricings(executionContext);
                await specialPricingUseCases.SaveProductsSpecialPricings(productsSpecialPricingList);
                log.LogMethodExit(productsSpecialPricingList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the ProductsSpecialPricingDTO collection
        /// <param name="productsSpecialPricingList">ProductsSpecialPricingDTO</param>
        [HttpPut]
        [Route("api/Product/SpecialPricing")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ProductsSpecialPricingDTO> productsSpecialPricingList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(productsSpecialPricingList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productsSpecialPricingList == null || productsSpecialPricingList.Any(a => a.PricingId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductsSpecialPricingUseCases specialPricingUseCases = ProductsUseCaseFactory.GetProductsSpecialPricings(executionContext);
                await specialPricingUseCases.SaveProductsSpecialPricings(productsSpecialPricingList);
                log.LogMethodExit(productsSpecialPricingList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }

            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Deletes the JSON Products Special Pricing.
        /// </summary>
        /// <param name="productsSpecialPricingList">productsSpecialPricingList</param>
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/SpecialPricing")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ProductsSpecialPricingDTO> productsSpecialPricingList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;


            try
            {
                log.LogMethodEntry(productsSpecialPricingList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (productsSpecialPricingList != null && productsSpecialPricingList.Any())
                {
                    IProductsSpecialPricingUseCases specialPricingUseCases = ProductsUseCaseFactory.GetProductsSpecialPricings(executionContext);
                    specialPricingUseCases.Delete(productsSpecialPricingList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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

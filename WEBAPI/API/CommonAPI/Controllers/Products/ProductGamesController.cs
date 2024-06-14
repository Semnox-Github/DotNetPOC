/********************************************************************************************
 * Project Name - ProductsGamesEntitlementsController
 * Description  - Created to fetch, update and insert ProductsGamesEntitlements in the product entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60       31-Jan-2019    Akshay Gulaganji          Created to Get, Post and Delete Methods.
 *2.60       24-Mar-2019    Nagesh Badiger            Added Custom Generic Exception and log method entry and method exit
 *********************************************************************************************
 *2.70       29-June-2019   Indrajeet Kumar           Modified Delete - Implemented Hard Deletion.
 *2.110.0    21-Nov-2020   Girish Kundar             EndPoint changes. 
  *2.120.00  07-Apr-2021   Roshan Devadiga           Modified Get,Post Delete and Added Put method
 *********************************************************************************************/


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
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    /// <summary>
    /// APIController of ProductsGamesController
    /// </summary>
    public class ProductsGamesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Content List Details of ProductsGamesEntitlements i.e., Parent and child objects
        /// </summary>
        /// <param name="productId">productId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Product/ProductGames")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = "", string productId = "")
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

                List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> searchParameters = new List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>();
                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID, productId));
                }
                searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.ISACTIVE, isActive));
                }
                IProductGamesUseCases productGamesUseCases = ProductsUseCaseFactory.GetProductGamesUseCases(executionContext);
                List<ProductGamesDTO> productGamesDTOList = await productGamesUseCases.GetProductGames(searchParameters);
                log.LogMethodExit(productGamesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productGamesDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Posts the Details of Product Games List of Objects
        /// </summary>
        /// <param name="productGamesDTOList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Product/ProductGames")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ProductGamesDTO> productGamesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productGamesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productGamesDTOList == null)
                {
                    log.LogMethodExit(productGamesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IProductGamesUseCases productGamesUseCases = ProductsUseCaseFactory.GetProductGamesUseCases(executionContext);
                await productGamesUseCases.SaveProductGames(productGamesDTOList);
                log.LogMethodExit(productGamesDTOList);
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
        /// Deletes(Soft Deletion using update method in BL) the Details of Product Games List of Objects using
        /// </summary>
        /// <param name="productGamesDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Product/ProductGames")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ProductGamesDTO> productGamesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productGamesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productGamesDTOList != null || productGamesDTOList.Any())
                {
                    IProductGamesUseCases productGamesUseCases = ProductsUseCaseFactory.GetProductGamesUseCases(executionContext);
                    productGamesUseCases.Delete(productGamesDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
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
        /// <summary>
        /// Post the ProductGamesDTOList collection
        /// <param name="productGamesDTOList">ProductGamesDTOList</param>
        [HttpPut]
        [Route("api/Product/ProductGames")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<ProductGamesDTO> productGamesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(productGamesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productGamesDTOList == null || productGamesDTOList.Any(a => a.Game_id < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductGamesUseCases productGamesUseCases = ProductsUseCaseFactory.GetProductGamesUseCases(executionContext);
                await productGamesUseCases.SaveProductGames(productGamesDTOList);
                log.LogMethodExit(productGamesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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

/********************************************************************************************
 * Project Name - ProductsCalendar Controller
 * Description  - Created to fetch, update and insert Product Setup Calendar in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By                Remarks          
 *********************************************************************************************
 *2.60        10-Jan-2019   Jagan Mohana Rao          Created to get, insert, update and Delete Methods.
 *2.60        21-Mar-2019   Nagesh Badiger            Added Custom Generic Exception
 *2.70.0      21-Jun-2019   Nagesh Badiger            Modified Delete method.
 *2.110.0     21-Nov-2019   Girish Kundar             Modified :  REST API changes for Inventory UI redesign
 *2.120.00    09-Mar-2021   Roshan Devadiga           Modified Get,Post Delete and Added Put method
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
    public class ProductsCalendarController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Product Setup Calendar
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductCalendars")]
        public async Task<HttpResponseMessage> Get(string productId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>(ProductsCalenderDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>(ProductsCalenderDTO.SearchByParameters.PRODUCT_ID, productId));
                }
                IProductsCalenderUseCases productsCalenderUseCases = ProductsUseCaseFactory.GetProductsCalenderUseCases(executionContext);
                List<ProductsCalenderDTO> productsCalenderDTOList = await productsCalenderUseCases.GetProductsCalenders(searchParameters);
                log.LogMethodExit(productsCalenderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsCalenderDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the JSON Product Calender.
        /// </summary>
        /// <param name="productsCalenderList"></param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductCalendars")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<ProductsCalenderDTO> productsCalenderList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsCalenderList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productsCalenderList == null )
                {
                    log.LogMethodExit(productsCalenderList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductsCalenderUseCases productsCalenderUseCases = ProductsUseCaseFactory.GetProductsCalenderUseCases(executionContext);
                await productsCalenderUseCases.SaveProductsCalenders(productsCalenderList);
                log.LogMethodExit(productsCalenderList);
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
        /// Delete the JSON Product Calender.
        /// </summary>
        /// <param name="productsCalenderList"></param>
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/ProductCalendars")]
        [Authorize]
        public HttpResponseMessage Delete(List<ProductsCalenderDTO> productsCalenderList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsCalenderList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productsCalenderList != null || productsCalenderList.Any())
                {
                    IProductsCalenderUseCases productsCalenderUseCases = ProductsUseCaseFactory.GetProductsCalenderUseCases(executionContext);
                    productsCalenderUseCases.Delete(productsCalenderList);
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
        /// Post the ProductsCalenderList collection
        /// <param name="productsCalenderList">ProductsCalenderList</param>
        [HttpPut]
        [Route("api/Product/ProductCalendars")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ProductsCalenderDTO> productsCalenderList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(productsCalenderList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productsCalenderList == null || productsCalenderList.Any(a => a.ProductCalendarId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductsCalenderUseCases productsCalenderUseCases = ProductsUseCaseFactory.GetProductsCalenderUseCases(executionContext);
                await productsCalenderUseCases.SaveProductsCalenders(productsCalenderList);
                log.LogMethodExit(productsCalenderList);
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
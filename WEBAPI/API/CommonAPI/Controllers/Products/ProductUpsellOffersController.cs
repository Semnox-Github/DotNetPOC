/********************************************************************************************
 * Project Name - ProductDetailsUpsellOffers Controller
 * Description  - Created to fetch, update and insert Upsell Offers in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        22-Jan-2019   Akshay Gulaganji          Created to get, insert, update and Delete Methods.
 *2.70        07-Jul-2019   Indrajeet Kumar           Modified Delete method.
 *2.110.0     21-Nov-2019   Girish Kundar             Modified :  REST API changes for Inventory UI redesign
 *2.120.00    06-Apr-2021   Roshan Devadiga           Modified Get,Post Delete and Added Put method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class ProductUpsellOffersController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Upsell Offers.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductUpsellOffers")]
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

                List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>> searchParameters = new List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>>();
                searchParameters.Add(new KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>(UpsellOffersDTO.SearchByUpsellOffersParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>(UpsellOffersDTO.SearchByUpsellOffersParameters.PRODUCT_ID, productId));
                }
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>(UpsellOffersDTO.SearchByUpsellOffersParameters.ACTIVE_FLAG, isActive));
                }
                IUpsellOfferUseCases upsellOfferUseCases = ProductsUseCaseFactory.GetUpsellOfferUseCases(executionContext);
                List<UpsellOffersDTO> upsellOffersDTOList = await upsellOfferUseCases.GetUpsellOffers(searchParameters);
                log.LogMethodExit(upsellOffersDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = upsellOffersDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Upsell Offers
        /// </summary>
        /// <param name="upsellOffersList">upsellOffersList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductUpsellOffers")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<UpsellOffersDTO> upsellOffersList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(upsellOffersList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (upsellOffersList == null )
                {
                    log.LogMethodExit(upsellOffersList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IUpsellOfferUseCases upsellOfferUseCases = ProductsUseCaseFactory.GetUpsellOfferUseCases(executionContext);
                await upsellOfferUseCases.SaveUpsellOffers(upsellOffersList);
                log.LogMethodExit(upsellOffersList);
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
        /// Deletes the JSON Object Upsell Offers
        /// </summary>
        /// <param name="upsellOffersList">upsellOffersList</param>
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/ProductUpsellOffers")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<UpsellOffersDTO> upsellOffersList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(upsellOffersList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (upsellOffersList != null && upsellOffersList.Any())
                {
                    IUpsellOfferUseCases upsellOfferUseCases = ProductsUseCaseFactory.GetUpsellOfferUseCases(executionContext);
                    upsellOfferUseCases.Delete(upsellOffersList);
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
        /// <summary>
        /// Post the UpsellOffersList collection
        /// <param name="upsellOffersList">UpsellOffersList</param>
        [HttpPut]
        [Route("api/Product/ProductUpsellOffers")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<UpsellOffersDTO> upsellOffersList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(upsellOffersList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (upsellOffersList == null || upsellOffersList.Any(a => a.OfferId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IUpsellOfferUseCases upsellOfferUseCases = ProductsUseCaseFactory.GetUpsellOfferUseCases(executionContext);
                await upsellOfferUseCases.SaveUpsellOffers(upsellOffersList);
                log.LogMethodExit(upsellOffersList);
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

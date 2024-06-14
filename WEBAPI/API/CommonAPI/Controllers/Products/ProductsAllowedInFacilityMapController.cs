/********************************************************************************************
 * Project Name - Map Products To Facility Map Controller
 * Description  - Created to fetch, update, insert & delete in the Facility Map for Product entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.70       12-Jul-2019   Akshay Gulaganji         Created
*2.110.0     21-Nov-2020   Girish Kundar            EndPoint changes. 
*2.120.0     06-Apr-2021   B Mahesh Pai             Modified Get and post , Added Put method
 ***************************************************************************************************/

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
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Products
{
    public class ProductsAllowedInFacilityMapController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Gets the ProductsAllowedInFacilityMapDTO list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductsAllowedInFacilityMaps")]
        public async Task<HttpResponseMessage> Get(string isActive, string productId, string facilityMapId = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, productId, facilityMapId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(executionContext);
                List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID, productId.ToString()));
                searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                if (!string.IsNullOrEmpty(facilityMapId))
                {
                    searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                }
                IProductsAllowedInFacilityMapUseCases productsAllowedInFacilityMapUseCases = ProductsUseCaseFactory.GetProductsAllowedInFacilityMaps(executionContext);
                List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList = await productsAllowedInFacilityMapUseCases.GetProductsAllowedInFacilityMaps(searchParameters);
                log.LogMethodExit(productsAllowedInFacilityMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsAllowedInFacilityMapDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Posts the ProductsAllowedInFacilityMapDTO
        /// </summary>
        /// <param name="productsAllowedInFacilityMapDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/ProductsAllowedInFacilityMaps")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsAllowedInFacilityMapDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productsAllowedInFacilityMapDTO == null)
                {
                    log.LogMethodExit(productsAllowedInFacilityMapDTO);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductsAllowedInFacilityMapUseCases productsAllowedInFacilityMapUseCases = ProductsUseCaseFactory.GetProductsAllowedInFacilityMaps(executionContext);
                await productsAllowedInFacilityMapUseCases.SaveProductsAllowedInFacilityMaps(productsAllowedInFacilityMapDTO);
                log.LogMethodExit(productsAllowedInFacilityMapDTO);
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
        /// Post the ProductsAllowedInFacilityMapDTO collection
        /// <param name="productsAllowedInFacilityMapDTO">ProductsAllowedInFacilityMapDTO</param>
        [HttpPut]
        [Route("api/Product/ProductsAllowedInFacilityMaps")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(productsAllowedInFacilityMapDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productsAllowedInFacilityMapDTO == null || productsAllowedInFacilityMapDTO.Any(a => a.ProductsAllowedInFacilityMapId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductsAllowedInFacilityMapUseCases productsAllowedInFacilityMapUseCases = ProductsUseCaseFactory.GetProductsAllowedInFacilityMaps(executionContext);
                await productsAllowedInFacilityMapUseCases.SaveProductsAllowedInFacilityMaps(productsAllowedInFacilityMapDTO);
                log.LogMethodExit(productsAllowedInFacilityMapDTO);
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

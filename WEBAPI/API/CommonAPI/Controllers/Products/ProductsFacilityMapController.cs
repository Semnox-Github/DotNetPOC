/********************************************************************************************
 * Project Name - Allowed Products Controller
 * Description  - Created to fetch, update and insert in the Allowed Products entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.70       14-Jun-2019   Nagesh Badiger           Created 
 *2.70       17-Jul-2019   Akshay Gulaganji         modified GET(), POST() and DELETE() methods
 ***************************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    public class ProductsFacilityMapController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the Allowed Products.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/ProductsFacilityMap/")]
        public HttpResponseMessage Get(string activityType, string isActive = null, string productId = null, string facilityMapId = null)
        {
            try
            {
                log.LogMethodEntry(isActive, facilityMapId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(executionContext);
                List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>();               
                searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(isActive) && isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                if(activityType.ToUpper().ToString() == "ALLOWEDPRODUCTS" && !string.IsNullOrEmpty(facilityMapId))
                {
                    searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                }
                if (activityType.ToUpper().ToString() == "ALLOWEDPRODUCTS" && !string.IsNullOrEmpty(facilityMapId))
                {
                    searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                }

                List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOList = productsAllowedInFacilityListBL.GetProductsAllowedInFacilityMapDTOList(searchParameters);
                SortableBindingList<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOSortList = null;
                if (productsAllowedInFacilityDTOList != null)
                {
                    productsAllowedInFacilityDTOSortList = new SortableBindingList<ProductsAllowedInFacilityMapDTO>(productsAllowedInFacilityDTOList);
                }                
                log.LogMethodExit(productsAllowedInFacilityDTOSortList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsAllowedInFacilityDTOSortList, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Object Allowed Products
        /// </summary>
        /// <param name="productsAllowedInFacilityDTOList">productsAllowedInFacilityDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Products/ProductsFacilityMap/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList)
        {
            try
            {
                log.LogMethodEntry(productsAllowedInFacilityMapDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productsAllowedInFacilityMapDTOList != null && productsAllowedInFacilityMapDTOList.Count != 0)
                {
                    ProductsAllowedInFacilityMapListBL productsAllowedInFacilityMapListBL = new ProductsAllowedInFacilityMapListBL(executionContext, productsAllowedInFacilityMapDTOList);
                    productsAllowedInFacilityMapListBL.Save();

                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (ValidationException valEx)
            {
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.Accepted, new { data = validationException, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Delete the Allowed Products
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Products/ProductsFacilityMap/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList)
        {
            try
            {
                log.LogMethodEntry(productsAllowedInFacilityMapDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productsAllowedInFacilityMapDTOList != null && productsAllowedInFacilityMapDTOList.Count != 0)
                {
                    ProductsAllowedInFacilityMapListBL productsAllowedInFacilityMapListBL = new ProductsAllowedInFacilityMapListBL(executionContext, productsAllowedInFacilityMapDTOList);
                    productsAllowedInFacilityMapListBL.Save();

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

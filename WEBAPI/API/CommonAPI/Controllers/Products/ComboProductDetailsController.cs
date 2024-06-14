/********************************************************************************************
 * Project Name - Products Controller/ComboProductDetailsController
 * Description  - Created to fetch, update and insert in the ComboProduct Details.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60        15-Feb-2019   Nagesh Badiger          Created to get, insert, update and Delete Methods.
 ****************************************************************************************************
 *2.60        18-Mar-2019   Akshay Gulaganji          Added ExecutionContext and CustomGenericException
 *2.70        07-Jul-2019   Indrajeet Kumar         Modified Delete for Hard Deletion
 *2.110.0     10-Sep-2020   Vikas Dwivedi           Modified as per the REST API Standards.
 ***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class ComboProductDetailsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the ComboProductDetails.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ComboProducts")]
        public HttpResponseMessage Get(string isActive = null, int productId = -1, int comboProductId = -1, int childProductId = -1, int categoryId = -1,
                                            int displayGroupId = -1, bool priceInclusive = false, bool additionalProduct = false, string childProductType = null,
                                            string hasActiveSubscriptionChild = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, productId, comboProductId, childProductId, categoryId, displayGroupId, priceInclusive, additionalProduct, childProductType, hasActiveSubscriptionChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                ComboProductList comboProductList = new ComboProductList(executionContext);
                if (productId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, productId.ToString()));
                }
                if (comboProductId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.COMBOPRODUCT_ID, comboProductId.ToString()));
                }
                if (childProductId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.CHILD_PRODUCT_ID, childProductId.ToString()));
                }
                if (categoryId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.CATEGORY_ID, categoryId.ToString()));
                }
                if (displayGroupId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.DISPLAY_GROUP_ID, displayGroupId.ToString()));
                }
                if (!string.IsNullOrEmpty(childProductType))
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.CHILD_PRODUCT_TYPE, childProductType));
                }
                if (additionalProduct)
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.ADDITIONAL_PRODUCT, additionalProduct.ToString()));
                }
                if (priceInclusive)
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRICE_INCLUSIVE, priceInclusive.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                if (string.IsNullOrEmpty(hasActiveSubscriptionChild) == false)
                {
                    if (hasActiveSubscriptionChild == "1" || hasActiveSubscriptionChild == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.HAS_ACTIVE_SUBSCRIPTION_CHILD, "1"));
                    }
                    else
                    {
                        searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.HAS_ACTIVE_SUBSCRIPTION_CHILD, "0"));
                    }
                }
                List<ComboProductDTO> comboProductDTOList = comboProductList.GetComboProductDTOList(searchParameters);
                log.LogMethodExit(comboProductDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = comboProductDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object ComboProductDetails
        /// </summary>
        /// <param name="comboProductDTOList">ComboProductDetails</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Product/ComboProducts")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ComboProductDTO> comboProductDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(comboProductDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (comboProductDTOList != null && comboProductDTOList.Any())
                {
                    ComboProductList comboProductList = new ComboProductList(executionContext, comboProductDTOList);
                    comboProductList.SaveUpdateComboProductList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Post the JSON Object ComboProductDetails
        /// </summary>
        /// <param name="comboProductDTOList">ComboProductDetails</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Product/ComboProducts")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ComboProductDTO> comboProductDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(comboProductDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (comboProductDTOList != null && comboProductDTOList.Any())
                {
                    ComboProductList comboProductList = new ComboProductList(executionContext, comboProductDTOList);
                    comboProductList.DeleteComboProductList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}

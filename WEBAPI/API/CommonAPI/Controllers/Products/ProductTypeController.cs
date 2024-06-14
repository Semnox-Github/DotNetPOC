/********************************************************************************************
 * Project Name - ProductType Controller
 * Description  - Created to fetch, update and insert ProductType in the Product Setup.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60        7-Feb-2019    Indrajeet Kumar          Created to get, insert, update and Delete Methods.
 *2.60        28-Mar-2019   Nagesh Badiger           Added Custom Generic Exception and log method entry and method exit
 *********************************************************************************************
 *2.70        29-June-2019  Indrajeet Kumar          Modified Delete - Implement Hard Deletion
 *2.110.0     10-Sep-2020   Vikas Dwivedi            Modified as per the REST API Standards.
 ********************************************************************************************/

 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products.Controllers.Products
{
    public class ProductTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON ProductType 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductTypes")]
        public HttpResponseMessage Get(int productTypeId = -1, string productType = null, int orderTypeId = -1, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productTypeId, productType, orderTypeId, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (productTypeId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE_ID, productTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(productType))
                {
                    searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, productType));
                }
                if (orderTypeId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.ORDERTYPEID, orderTypeId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                ProductTypeListBL productTypeListBL = new ProductTypeListBL(executionContext);
                List<ProductTypeDTO> productTypeDTOList = productTypeListBL.GetProductTypeDTOList(searchParameters);
                log.LogMethodExit(productTypeDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productTypeDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON ProductType.
        /// </summary>
        /// <param name="productTypeList"></param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Product/ProductTypes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<ProductTypeDTO> productTypeList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productTypeList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productTypeList != null && productTypeList.Any())
                {
                    ProductTypeListBL productTypeListObj = new ProductTypeListBL(executionContext, productTypeList);
                    productTypeListObj.Save();
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
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Delete the JSON ProductType.
        /// </summary>
        /// <param name="productTypeList"></param>
        /// <returns>HttpMessage</returns>
        [HttpDelete]
        [Route("api/Product/ProductTypes")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ProductTypeDTO> productTypeList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productTypeList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productTypeList != null && productTypeList.Any())
                {
                    ProductTypeListBL productTypeListObj = new ProductTypeListBL(executionContext, productTypeList);
                    productTypeListObj.Delete();
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
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationException });
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

/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert product modifiers in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        10-Jan-2019   Jagan Mohana Rao          Created to get, insert, update and Delete Methods.
 *2.110.0       21-Nov-2019   Girish Kundar       Modified :  REST API changes for Inventory UI redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Product;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Products
{
    public class ProductModifiersController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       

        /// <summary>
        /// Get the JSON products modifiers.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductModifiers")]
        public HttpResponseMessage Get(string isActive, string productId)
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

                List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.PRODUCT_ID, productId));
                }
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.ISACTIVE, isActive));
                }
                ProductModifiersList productModifiersList = new ProductModifiersList(executionContext);
                var content = productModifiersList.GetAllProductModifiersList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
        /// Post the JSON Object Product Modifiers
        /// </summary>
        /// <param name="productModifiersList">productModifiersList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductModifiers")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<ProductModifiersDTO> productModifiersList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productModifiersList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productModifiersList != null && productModifiersList.Count != 0)
                {
                    ProductModifiersList productModifiers = new ProductModifiersList(executionContext, productModifiersList);
                    productModifiers.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound"  });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
        /// Delete the Product Modifiers
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/ProductModifiers")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ProductModifiersDTO> productModifiersList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productModifiersList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productModifiersList != null && productModifiersList.Count != 0)
                {
                    ProductModifiersList productModifiers = new ProductModifiersList(executionContext, productModifiersList);
                    productModifiers.Delete();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound"  });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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

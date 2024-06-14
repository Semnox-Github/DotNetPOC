/********************************************************************************************
 * Project Name - ProductSetup ExtendedCredits Controller
 * Description  - Created to fetch, update and insert Extended Credits in the product Setup.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        4-Feb-2019   Indrajeet Kumar          Created to get, insert, update and Delete Methods.
 *2.60        24-Mar-2019  Nagesh Badiger           Added Custom Generic Exception and log method entry and method exit
 *********************************************************************************************
 *2.70        29-June-2019 Indrajeet Kumar          Modified Delete - Implemented Hard Deletion
 *2.110.0       21-Nov-2019   Girish Kundar       Modified :  REST API changes for Inventory UI redesign
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

namespace Semnox.CommonAPI.Products
{
    public class ProductsCreditPlusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Extended Credits
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductsCreditPlus")]
        public HttpResponseMessage Get(string isActive, string productId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive,productId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>(ProductCreditPlusDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>(ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID, productId));
                }
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>(ProductCreditPlusDTO.SearchByParameters.ISACTIVE, isActive));
                }

                ProductCreditPlusBLList productCreditPlusBLList = new ProductCreditPlusBLList(executionContext);
                var content = productCreditPlusBLList.GetAllProductCreditPlusListDTOList(searchParameters,true);
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
        /// Post the JSON Extended Credits.
        /// </summary>
        /// <param name="productCreditPlusList"></param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductsCreditPlus")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<ProductCreditPlusDTO> productCreditPlusList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productCreditPlusList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productCreditPlusList != null || productCreditPlusList.Any())
                {
                    ProductCreditPlusBLList productCreditPlusBLList = new ProductCreditPlusBLList(executionContext, productCreditPlusList);
                    productCreditPlusBLList.SaveUpdateProductCreditPlusList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
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
        /// Delete the JSON Extended Credits.
        /// </summary>
        /// <param name="productCreditPlusList"></param>
        /// <returns>HttpMessage</returns>
        [HttpDelete]
        [Route("api/Product/ProductsCreditPlus")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ProductCreditPlusDTO> productCreditPlusList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productCreditPlusList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productCreditPlusList != null || productCreditPlusList.Any())
                {
                    ProductCreditPlusBLList productCreditPlusBLList = new ProductCreditPlusBLList(executionContext, productCreditPlusList);
                    productCreditPlusBLList.DeleteProductCreditPlusList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
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

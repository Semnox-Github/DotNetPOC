/********************************************************************************************
 * Project Name - Products Description Controller
 * Description  - Created to fetch, update and insert Products Description in the product details entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        04-Mar-2019   Akshay Gulaganji          Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.60        20-Mar-2019   Akshay Gulaganji          Added customGenericException
 *2.120.1     21-Jun-2021   Deeksha                   Modified to handle unique identifier error during product save
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    public class ProductDescriptionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Gets the Json of ProductDescription
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Product/ProductDescription")]
        [Authorize]
        public HttpResponseMessage Get(string productId,string elementName)
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (string.IsNullOrEmpty(productId) == false && Convert.ToInt32(productId) > 0)
                {
                    List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.OBJECT, "PRODUCTS"));
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT, elementName));
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    ProductsList productsList = new ProductsList(executionContext);
                    var content = productsList.GetProductsDescriptionList(searchParameters, productId);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
            }
        }

        [HttpPost]
        [Route("api/Product/ProductDescription")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ObjectTranslationsDTO> objectTranslationsDTOList)
        {
            try
            {
                log.LogMethodEntry(objectTranslationsDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;

                if (objectTranslationsDTOList != null || objectTranslationsDTOList.Count != 0)
                {
                    ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(executionContext, objectTranslationsDTOList);
                    objectTranslationsList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
            }
        }
        [HttpDelete]
        [Route("api/Product/ProductDescription")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ObjectTranslationsDTO> objectTranslationsDTOList)
        {
            try
            {
                log.LogMethodEntry(objectTranslationsDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;

                if (objectTranslationsDTOList != null || objectTranslationsDTOList.Count > 0)
                {
                    ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(executionContext, objectTranslationsDTOList);
                    objectTranslationsList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
            }
        }
    }
}

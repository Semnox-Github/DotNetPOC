/********************************************************************************************
 * Project Name - Product Category Controller
 * Description  - Created to fetch, update and insert Product Controller in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        24-Jan-2019   Indrajeet Kumar          Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.50        20-Mar-2019   Akshay Gulaganji         Added customGenericException and modified isActive from string to bool
 ********************************************************************************************
 *2.70        30-June-2019  Indrajeet Kumar          Modified Delete - for Hard Deletion. 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Parafait.Category;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Products
{
    public class ProductCategoryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Product Category.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/ProductCategory/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, isActive));
                }
                CategoryList categoryList = new CategoryList(executionContext);
                var content = categoryList.GetAllCategory(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Product Category
        /// </summary>
        /// <param name="ProductCategory">ProductCategory</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Products/ProductCategory")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<CategoryDTO> categoryList)
        {
            try
            {
                log.LogMethodEntry(categoryList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (categoryList != null || categoryList.Count != 0)
                {
                    CategoryList category = new CategoryList(executionContext, categoryList);
                    category.Save();
                    log.LogMethodExit();
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

        /// <summary>
        /// Delete the JSON Product Category
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Products/ProductCategory")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<CategoryDTO> categoryList)
        {
            try
            {
                log.LogMethodEntry(categoryList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (categoryList != null || categoryList.Count != 0)
                {
                    CategoryList category = new CategoryList(executionContext, categoryList);
                    category.DeleteCategoryList();
                    log.LogMethodExit();
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

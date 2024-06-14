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
 *2.100.0     04-Oct-2020  Mushahid Faizan         Modified: as per API Standards, namespace changes, endPoint Changes, added searchParameters in get(),
 *                                                 Renamed Controller from ProductCategoryController to CategoryController
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class CategoryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Product Category.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Categories")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int categoryId = -1, string categoryIdList = null, string categoryName = null, bool loadActiveChild = false, bool buildChildRecords = false, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(categoryName))
                {
                    searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.NAME, categoryName));
                }
                if (categoryId > -1)
                {
                    searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.CATEGORY_ID, categoryId.ToString()));
                }
                if (!string.IsNullOrEmpty(categoryIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> categoryListId = new List<int>();

                    categoryListId = categoryIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String requisitionListString = String.Join(",", categoryListId.ToArray());
                    searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.CATEGORY_ID_LIST, requisitionListString));
                }
                CategoryList categoryList = new CategoryList(executionContext);

                int totalNoOfPages = 0;
                int totalNoOfCategories = await Task<int>.Factory.StartNew(() => { return categoryList.GetCategoriesCount(searchParameters, null); });
                log.LogVariableState("totalNoOfCategories", totalNoOfCategories);
                totalNoOfPages = (totalNoOfCategories / pageSize) + ((totalNoOfCategories % pageSize) > 0 ? 1 : 0);

                ICategoryUseCases categoryUseCases = InventoryUseCaseFactory.GetCategoryUseCases(executionContext);
                List<CategoryDTO> categoryDTOList = await categoryUseCases.GetCategories(searchParameters, buildChildRecords, loadActiveChild, currentPage, pageSize);
                log.LogMethodExit(categoryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = categoryDTOList, currentPageNo = currentPage, TotalCount = totalNoOfCategories });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Product Category
        /// </summary>
        /// <param name="categoryDTOList">categoryList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/Categories")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<CategoryDTO> categoryDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(categoryDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (categoryDTOList == null || categoryDTOList.Any(a => a.CategoryId > 0))
                {
                    log.LogMethodExit(categoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICategoryUseCases categoryUseCases = InventoryUseCaseFactory.GetCategoryUseCases(executionContext);
                await categoryUseCases.SaveCategories(categoryDTOList);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = categoryDTOList });
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
        /// Post the JSON Product Category
        /// </summary>
        /// <param name="categoryDTOList">categoryList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/Categories")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<CategoryDTO> categoryDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(categoryDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                // Checks if the id is greater than to 0, If it is greater than to 0, then update 
                if (categoryDTOList == null || categoryDTOList.Any(a => a.CategoryId < 0))
                {
                    log.LogMethodExit(categoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICategoryUseCases categoryUseCases = InventoryUseCaseFactory.GetCategoryUseCases(executionContext);
                await categoryUseCases.SaveCategories(categoryDTOList);
                log.LogMethodExit(categoryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = categoryDTOList });
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
        /// Delete the JSON Product Category
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Inventory/Categories")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromBody]List<CategoryDTO> categoryDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(categoryDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (categoryDTOList != null && categoryDTOList.Any())
                {
                    ICategoryUseCases categoryUseCases = InventoryUseCaseFactory.GetCategoryUseCases(executionContext);
                    await categoryUseCases.DeleteCategories(categoryDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit(categoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
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

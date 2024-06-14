/********************************************************************************************
 * Project Name - CategoryCount Controller
 * Description  - Created CategoryCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   10-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Inventory;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class CategoryCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Product Category.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/CategoryCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int categoryId = -1, string categoryName = null, int currentPage = 0, int pageSize = 10)
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

                CategoryList categoryList = new CategoryList(executionContext);
                ICategoryUseCases categoryUseCases = InventoryUseCaseFactory.GetCategoryUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfCategories = await categoryUseCases.GetCategoryCount(searchParameters); 
                log.LogVariableState("totalNoOfCategories", totalNoOfCategories);
                totalNoOfPages = (totalNoOfCategories / pageSize) + ((totalNoOfCategories % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfCategories);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfCategories, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }
    }
}
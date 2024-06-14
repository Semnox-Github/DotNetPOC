/********************************************************************************************
 * Project Name - ProductCount Controller
 * Description  - Created ProductCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   11-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class ProductCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Product.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Inventory/ProductCounts")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, int productId = -1, int manualProductId = -1, int segmentProductId = -1,
            string productName = null, string code = null, string description = null, int categoryId = -1, bool buildImage = false, string type = null, string advSearch = null,
           int uomId = -1, int defaultVendorId = -1, bool isSellable = false, bool isPublished = false, bool isRedeemable = false, string productIdList = null,
           string barcode = null, string displayGroup = null, bool lotControlled = false, int currentPage = 0, int pageSize = 10)
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

                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_NAME, productName));
                }
                if (productId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, productId.ToString()));
                }
                if (categoryId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CATEGORY, categoryId.ToString()));
                }
                if (!string.IsNullOrEmpty(barcode))
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.BARCODE, barcode.ToString()));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE, code.ToString()));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.DESCRIPTION, description.ToString()));
                }
                if (!string.IsNullOrEmpty(displayGroup))
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME, displayGroup.ToString()));
                }
                if (isRedeemable)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.ISREDEEMABLE, "Y"));
                }
                if (isSellable)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.ISSELLABLE, "Y"));
                }
                if (isPublished)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_PUBLISHED, "Y"));
                }
                if (lotControlled)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE, lotControlled.ToString()));
                }
                if (manualProductId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID, manualProductId.ToString()));
                }
                if (segmentProductId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID, segmentProductId.ToString()));
                }
                if (defaultVendorId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.VENDOR_ID, defaultVendorId.ToString()));
                }
                if (!string.IsNullOrEmpty(productIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> productListId = new List<int>();

                    productListId = productIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String productsIdListString = String.Join(",", productListId.ToArray());
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST, productsIdListString));
                }
                ProductList productList = new ProductList(executionContext);
                IProductsUseCases productUseCases = ProductsUseCaseFactory.GetProductUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfProducts = await productUseCases.GetInventoryProductCount(searchParameters);
                log.LogVariableState("totalNoOfProducts", totalNoOfProducts);
                totalNoOfPages = (totalNoOfProducts / pageSize) + ((totalNoOfProducts % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfProducts);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfProducts, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
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

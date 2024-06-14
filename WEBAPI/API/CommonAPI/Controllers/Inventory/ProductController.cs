/********************************************************************************************
 * Project Name - Product Controller
 * Description  - Created to fetch, update and insert inventory product.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.100.0     19-Oct-2020   Mushahid Faizan      Created.
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 * *2.110.0    14-Dec-2020    Deeksha              Web Inventory UI redesign changes with REST API.
 *2.120.0     18-May-2021   Mushahid Faizan     Modified : Added search parameter as part of Web Inventory changes
 *2.150.0     13-Jul-2022   Abhishek            Modified: Web Inventory Redesign - Added search parameter isPurchaseable,
                                                defaultLocationId,codeOrDescription
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class ProductController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Product.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Inventory/Products")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, int productId = -1, int manualProductId = -1, int segmentProductId = -1,
            string productName = null, string code = null, string description = null, int categoryId = -1, bool buildImage = false, string type = null, string advSearch = null,
           int uomId = -1, int defaultVendorId = -1, bool isSellable = false, bool isPublished = false, bool isRedeemable = false, bool loadActiveChild = false,
           string barcode = null, string displayGroup = null, bool lotControlled = false, bool buildChildRecords = false, int currentPage = 0, int pageSize = 10,
           string productIdList = null, bool includeInPlan = false, string categoryIdList = null, string uomIdList = null, string itemTypeIdList = null,
            bool isPurchaseable = false, int defaultLocationId = -1, string codeOrDescription = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

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

                if (!string.IsNullOrEmpty(productIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> productListId = new List<int>();

                    productListId = productIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String productsIdListString = String.Join(",", productListId.ToArray());
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST, productsIdListString));
                }
                if (!string.IsNullOrEmpty(categoryIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> categoryListId = new List<int>();

                    categoryListId = categoryIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String categoryIdListString = String.Join(",", categoryListId.ToArray());
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CATEGORY_ID_LIST, categoryIdListString));
                }
                if (!string.IsNullOrEmpty(uomIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> uomListId = new List<int>();

                    uomListId = uomIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String uomIdListString = String.Join(",", uomListId.ToArray());
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.UOM_ID_LIST, uomIdListString));
                }
                if (!string.IsNullOrEmpty(itemTypeIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> itemTypeListId = new List<int>();

                    itemTypeListId = itemTypeIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String itemTypeIdListString = String.Join(",", itemTypeListId.ToArray());
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.ITEM_TYPE_ID_LIST, itemTypeIdListString));
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
                if (includeInPlan)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.INCLUDE_IN_PLAN, includeInPlan.ToString()));
                }
                if (isPurchaseable)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.ISPURCHASEABLE, "Y"));
                }
                if (defaultLocationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.DEFAULT_LOCATION_ID, defaultLocationId.ToString()));
                }
                if (!string.IsNullOrEmpty(codeOrDescription))
                {
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_OR_DESCRIPTION, codeOrDescription.ToString()));
                }
                ProductList productList = new ProductList(executionContext);

                int totalNoOfPages = 0;
                int totalNoOfProducts = await Task<int>.Factory.StartNew(() => { return productList.GetProductCount(searchParameters, null); });
                log.LogVariableState("totalNoOfProducts", totalNoOfProducts);
                totalNoOfPages = (totalNoOfProducts / pageSize) + ((totalNoOfProducts % pageSize) > 0 ? 1 : 0);

                IProductsUseCases productUseCases = ProductsUseCaseFactory.GetProductUseCases(executionContext);
                List<ProductDTO> productDTOList = await productUseCases.GetInventoryProducts(searchParameters, buildChildRecords, loadActiveChild, buildImage, currentPage, pageSize, type, advSearch);
                log.LogMethodExit(productDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productDTOList, currentPageNo = currentPage, TotalCount = totalNoOfProducts });

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
        /// Post the JSON of Product
        /// </summary>
        /// <param name="Product">Product</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/Products")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<ProductDTO> productDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                ProductList product = new ProductList(executionContext, productDTOList);
                product.Save();
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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


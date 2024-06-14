/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalProductController  API -  add and delete product  data in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 *2.130.7    07-Apr-2022            Ashish Bhat            Modified( External  REST API.)
 *2.140.3    24-Feb-2023            Abhishek               Modified : Added parameter customItemNumber to fetch 
 *                                                         product by external reference.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Environments
{
    public class ExternalProductController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Products
        /// </summary>       
        /// <param name="productId">productId</param>
        /// <param name="customItemNumber">customItemNumber</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Products")]
        public async Task<HttpResponseMessage> Get(int productId = -1, string customItemNumber = null, string externalReference = null)
        {
            log.LogMethodEntry(productId, customItemNumber, externalReference);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<CustomDataViewDTO> customDataViewDTOList = null;
                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "Y"));
                if (!string.IsNullOrEmpty(customItemNumber))
                {
                    CustomDataListBL customDataListBL = new CustomDataListBL(executionContext);
                    List<KeyValuePair<CustomDataViewDTO.SearchByParameters, string>> searchCustomDataViewParameters = new List<KeyValuePair<CustomDataViewDTO.SearchByParameters, string>>();
                    searchCustomDataViewParameters.Add(new KeyValuePair<CustomDataViewDTO.SearchByParameters, string>(CustomDataViewDTO.SearchByParameters.APPLICABILITY, "PRODUCT"));
                    searchCustomDataViewParameters.Add(new KeyValuePair<CustomDataViewDTO.SearchByParameters, string>(CustomDataViewDTO.SearchByParameters.VALUECHAR, customItemNumber.ToString()));
                    customDataViewDTOList = customDataListBL.GetCustomDataViewDTOList(searchCustomDataViewParameters);
                    if (customDataViewDTOList != null && customDataViewDTOList.Count > 0)
                    {
                        string customDataSetIdList = string.Empty;
                        StringBuilder sb = new StringBuilder("");
                        Dictionary<int, CustomDataViewDTO> customerDataSetIdDictionary = new Dictionary<int, CustomDataViewDTO>();
                        for (int i = 0; i < customDataViewDTOList.Count; i++)
                        {
                            if (customDataViewDTOList[i].CustomDataSetId == -1 ||
                                customerDataSetIdDictionary.ContainsKey(customDataViewDTOList[i].CustomDataSetId))
                            {
                                continue;
                            }
                            if (i != 0)
                            {
                                sb.Append(",");
                            }
                            sb.Append(customDataViewDTOList[i].CustomDataSetId.ToString());
                            customerDataSetIdDictionary.Add(customDataViewDTOList[i].CustomDataSetId, customDataViewDTOList[i]);
                        }
                        customDataSetIdList = sb.ToString();
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_ID_LIST, customDataSetIdList.ToString()));
                    }
                }
                if (productId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, productId.ToString()));
                }
                if (!string.IsNullOrEmpty(externalReference))
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE, externalReference.ToString()));
                }
                List<ProductsDTO> productsDTOList = new List<ProductsDTO>();
                ProductsList productList = new ProductsList(executionContext);
                productsDTOList = productList.GetProductsList(searchParameters, 0, 0, false, false, null, false, null);
                if (executionContext.GetMachineId() != -1)
                {
                    posProductExclusionsListBL posProductExclusionsListBL = new posProductExclusionsListBL(executionContext);
                    List<KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>(POSProductExclusionsDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                    searchByParameters.Add(new KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>(POSProductExclusionsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchByParameters.Add(new KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>(POSProductExclusionsDTO.SearchByParameters.POS_MACHINE_ID, executionContext.GetMachineId().ToString()));
                    List<POSProductExclusionsDTO> pOSProductExclusionsDTOList = posProductExclusionsListBL.GetPOSProductExclusionDTOList(searchByParameters, null);
                    string displayGroupIdList = string.Empty;
                    if (pOSProductExclusionsDTOList != null && pOSProductExclusionsDTOList.Count > 0)
                    {
                        //string displayGroupIdList = string.Empty;
                        StringBuilder sb = new StringBuilder("");
                        Dictionary<int, POSProductExclusionsDTO> displayGroupIdListDictionary = new Dictionary<int, POSProductExclusionsDTO>();
                        for (int i = 0; i < pOSProductExclusionsDTOList.Count; i++)
                        {
                            if (pOSProductExclusionsDTOList[i].ExclusionId == -1 ||
                                displayGroupIdListDictionary.ContainsKey(pOSProductExclusionsDTOList[i].ExclusionId))
                            {
                                continue;
                            }
                            if (i != 0)
                            {
                                sb.Append(",");
                            }
                            sb.Append(pOSProductExclusionsDTOList[i].ProductDisplayGroupFormatId.ToString());
                            displayGroupIdListDictionary.Add(pOSProductExclusionsDTOList[i].ExclusionId, pOSProductExclusionsDTOList[i]);
                        }
                        displayGroupIdList = sb.ToString();
                    }
                    POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.GetSiteId(), executionContext.GetMachineId());
                    productList = new ProductsList(executionContext);
                    List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchExcludeParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                    searchExcludeParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID_LIST, displayGroupIdList));
                    List<ProductsDTO> excludeProductsDTOList = productList.GetProductsList(searchExcludeParameters, 0, 0, false, false, null, false, null);
                    foreach (ProductsDTO excludeProductsDTO in excludeProductsDTOList)
                    {
                        if (pOSMachineContainerDTO.IncludedProductIdList.Contains(excludeProductsDTO.ProductId))
                        {
                            continue;
                        }
                        productsDTOList.RemoveAll(r => r.ProductId == excludeProductsDTO.ProductId);
                    }
                }
                List<ExternalProductDTO> externalProductDTOList = new List<ExternalProductDTO>();
                ExternalProductDTO externalProductDTO = new ExternalProductDTO();
                if (productsDTOList != null && productsDTOList.Any())
                {
                    foreach (ProductsDTO productsDTO in productsDTOList)
                    {
                        List<int> displayIdList = new List<int>();
                        List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParametersGroup = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                        searchParametersGroup.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        if (productsDTO.ProductId > 0)
                        {
                            searchParametersGroup.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, productsDTO.ProductId.ToString()));
                        }
                        IProductDisplayGroupUseCases productsDisplayGroupsUseCases = ProductsUseCaseFactory.GetProductsDisplayGroups(executionContext);
                        List<ProductsDisplayGroupDTO> productsDisplayGroupsDTOList = await productsDisplayGroupsUseCases.GetProductsDisplayGroups(searchParametersGroup);
                        if (productsDisplayGroupsDTOList != null && productsDisplayGroupsDTOList.Any())
                        {
                            foreach (ProductsDisplayGroupDTO productDisplayGroupDTO in productsDisplayGroupsDTOList)
                            {
                                displayIdList.Add(productDisplayGroupDTO.DisplayGroupId);
                            }
                        }
                        externalProductDTO = new ExternalProductDTO(productsDTO.ProductId, displayIdList, productsDTO.ProductName, Convert.ToDouble(productsDTO.Price),
                                  productsDTO.Tax_id, Convert.ToInt32(productsDTO.FaceValue), productsDTO.TaxInclusivePrice, productsDTO.ExpiryDate.ToString(), productsDTO.AvailableUnits, productsDTO.Description,
                                  productsDTO.WebDescription, productsDTO.OnlyForVIP == "N" ? false : true, productsDTO.AllowPriceOverride == "N" ? false : true, productsDTO.RegisteredCustomerOnly == "N" ? false : true, productsDTO.ManagerApprovalRequired == "N" ? false : true,
                                  productsDTO.MinimumQuantity, productsDTO.DisplayInPOS == "N" ? false : true, productsDTO.ProductType, productsDTO.CategoryName, Convert.ToInt32(productsDTO.TaxPercentage), productsDTO.MaxQtyPerDay.ToString(),
                                  customDataViewDTOList != null ? customItemNumber : productsDTO.ExternalSystemReference, productsDTO.SearchDescription, productsDTO.IsRecommended, productsDTO.SiteId);
                        externalProductDTOList.Add(externalProductDTO);
                    }
                }
                log.LogMethodExit(externalProductDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, externalProductDTOList);
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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

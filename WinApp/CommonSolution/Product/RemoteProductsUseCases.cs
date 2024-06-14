/********************************************************************************************
 * Project Name - Site
 * Description  - RemoteSiteUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.110.0      14-Dec-2020       Deeksha            Created :Inventory UI/POS UI re-design with REST API
 2.110.1         14-Feb-2021      Mushahid Faizan    Modified : Web Inventory phase 2 changes
 2.140.0       14-Sep-2021        Prajwal S         Modified : Addded Use cases
 2.150.0      18-May-2022       Abhishek            Modified: Added CustomerProfileGroupsUseCases for Customer Profile
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class RemoteProductsUseCases : RemoteUseCases , IProductsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTS_URL = "/api/Product/Products";
        private const string PRODUCT_GROUP_URL = "/api/Product/ProductGroups";
        private const string PRODUCT_GROUP_COUNT_URL = "/api/Product/ProductGroupsCount";
        private const string PRODUCTS_CONTAINER_URL = "/api/Product/ProductsContainer";
        private const string BOM_PRODUCT_URL = "api/Inventory/BOMProductCost";
        private const string DUPLICATE_PRODUCT_URL = "api/Product/Duplicate";
        private const string INVENTORY_PRODUCTS_URL = "/api/Inventory/Products";
        private const string PRODUCT_COUNTS_URL = "/api/Inventory/ProductCounts";
        private const string PRODUCT_CALENDAR_CONTAINER_URL = "/api/Product/ProductsCalenderContainer";
        private const string CUSTOMER_PROFILING_GROUP_URL = "api/Product/CustomerProfilingGroups";

        public RemoteProductsUseCases(ExecutionContext executionContext, string requestGuid)
            : base(executionContext, requestGuid)
        {
            log.LogMethodEntry(executionContext, requestGuid);
            log.LogMethodExit();
        }

        public async Task<List<ProductsDTO>> GetProducts(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ProductsDTO> result = await Get<List<ProductsDTO>>(PRODUCTS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetInventoryProductCount(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildInventoryProductsSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(PRODUCT_COUNTS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> GetBOMProductCost(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, int productId)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildInventoryProductsSearchParameter(searchParameters));
            }
            try
            {
                string result = await Get<string>(BOM_PRODUCT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<ProductDTO>> GetInventoryProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>
                  parameters, bool buildChildRecords, bool loadActiveChild, bool buildImage, int currentPage = 0, int pageSize = 0 , string type = null, string advSearch=null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildInventoryProductsSearchParameter(parameters));
                searchParameterList.Add(new KeyValuePair<string, string>("type".ToString(), type));
                searchParameterList.Add(new KeyValuePair<string, string>("buildImage".ToString(), buildImage.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("advSearch".ToString(), advSearch.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            }
            try
            {
                List<ProductDTO> result = await Get<List<ProductDTO>>(INVENTORY_PRODUCTS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> productsSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductsDTO.SearchByProductParameters, string> searchParameter in productsSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductsDTO.SearchByProductParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsDTO.SearchByProductParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsDTO.SearchByProductParameters.SITEID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                  
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        private List<KeyValuePair<string, string>> BuildInventoryProductsSearchParameter(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productsSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductDTO.SearchByProductParameters, string> searchParameter in productsSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductDTO.SearchByProductParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDTO.SearchByProductParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProducts(List<ProductsDTO> productsDTOList)
        {
            log.LogMethodEntry(productsDTOList);
            try
            {
                string result = await Post<string>(PRODUCTS_URL, productsDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> SaveDuplicateProductDetails(ProductsDTO productsDTO)
        {
            log.LogMethodEntry(productsDTO);
            try
            {
                string result = await Post<string>(DUPLICATE_PRODUCT_URL, productsDTO);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<ProductsContainerDTOCollection> GetProductsContainerDTOCollection(int siteId, string manualType, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("manualType", manualType.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            ProductsContainerDTOCollection result = await Get<ProductsContainerDTOCollection>(PRODUCTS_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<ProductCalendarContainerDTOCollection> GetProductCalendarContainerDTOCollection(int siteId, string manualProductType, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, manualProductType, startDateTime, endDateTime, hash, rebuildCache);
            ProductCalendarContainerDTOCollection result = await Get<ProductCalendarContainerDTOCollection>(PRODUCT_CALENDAR_CONTAINER_URL, new WebApiGetRequestParameterCollection("siteId", siteId, "manualProductType", manualProductType, "startDateTime", startDateTime, "endDateTime", endDateTime, "hash", hash, "rebuildCache", rebuildCache));
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<CustomerProfilingGroupDTO>> GetCustomerProfilingGroups(List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>>
                  parameters, bool buildChildRecords, bool loadActiveChildRecords/*, int currentPage = 0, int pageSize = 0*/)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));

                searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChildRecords".ToString(), loadActiveChildRecords.ToString()));
                //searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
                //searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            }
            try
            {
                List<CustomerProfilingGroupDTO> result = await Get<List<CustomerProfilingGroupDTO>>(CUSTOMER_PROFILING_GROUP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>> customerProfilesSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string> searchParameter in customerProfilesSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerProfilingGroupDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerProfilingGroupDTO.SearchByParameters.CUSTOMER_PROFILE_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerProfileGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerProfilingGroupDTO.SearchByParameters.GROUP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("groupName".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerProfilingGroupDTO.SearchByParameters.MASTER_ENTITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterEntityId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerProfilingGroupDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<CustomerProfilingGroupDTO>> SaveCustomerProfilingGroups(List<CustomerProfilingGroupDTO> CustomerProfilingGroupDTOList)
        {
            log.LogMethodEntry(CustomerProfilingGroupDTOList);
            try
            {
                List<CustomerProfilingGroupDTO> result = await Post<List<CustomerProfilingGroupDTO>>(CUSTOMER_PROFILING_GROUP_URL, CustomerProfilingGroupDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<ProductGroupDTO>> GetProductGroupDTOList(int id = -1,
                                                                        string name = "",
                                                                        string isActive = null,
                                                                        int siteId = -1,
                                                                        bool loadChildRecords = true,
                                                                        bool loadActiveChildRecords = true,
                                                                        int pageNumber = 0,
                                                                        int pageSize = 0)
        {
            log.LogMethodEntry(id, name, isActive, siteId, loadChildRecords, loadActiveChildRecords, pageNumber,
                                   pageSize);
            WebApiGetRequestParameterCollection parameters = new WebApiGetRequestParameterCollection("id", id,
                                                                                                     "name", name,
                                                                                                     "isActive", isActive,
                                                                                                     "siteId", siteId,
                                                                                                     "loadChildRecords", loadChildRecords,
                                                                                                     "loadActiveChildRecords", loadActiveChildRecords,
                                                                                                     "pageNumber", pageNumber,
                                                                                                     "pageSize", pageSize
                                                                                                     );
            List<ProductGroupDTO> result = await Get<List<ProductGroupDTO>>(PRODUCT_GROUP_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<int> GetProductGroupDTOListCount(int id = -1,
                                                           string name = "",
                                                           string isActive = null,
                                                           int siteId = -1)
        {
            log.LogMethodEntry(id, name, isActive, siteId);
            WebApiGetRequestParameterCollection parameters = new WebApiGetRequestParameterCollection("id", id,
                                                                                                     "name", name,
                                                                                                     "isActive", isActive,
                                                                                                     "siteId", siteId
                                                                                                     );
            int result = await Get<int>(PRODUCT_GROUP_COUNT_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<ProductGroupDTO>> SaveProductGroupDTOList(List<ProductGroupDTO> productgroupDTOList)
        {
            log.LogMethodEntry(productgroupDTOList);
            List<ProductGroupDTO> result = await Post<List<ProductGroupDTO>>(PRODUCT_GROUP_URL, productgroupDTOList);
            log.LogMethodExit(result);
            return result;
        }
    }
}

/********************************************************************************************
 * Project Name - Products
 * Description  - IProductsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version        Date             Modified By        Remarks          
 *********************************************************************************************
 2.110.0         01-Dec-2020      Deeksha            Created : Web Inventory/POS UI design with REST API
 2.110.1         14-Feb-2021      Mushahid Faizan    Modified : Web Inventory phase 2 changes
 2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 *2.140.00     14-Sep-2021       Prajwal S              Modified : Added Use Cases. 
 *2.150.0       18-May-2022       Abhishek         Modified: Added CustomerProfileGroupsUseCases for Customer Profile
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IProductsUseCases
    {
        Task<List<ProductsDTO>> GetProducts(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters);
        Task<string> SaveProducts(List<ProductsDTO> productDTOList);
        Task<ProductsContainerDTOCollection> GetProductsContainerDTOCollection(int siteId ,string manualProductType, string hash, bool rebuildCache);
        Task<List<ProductDTO>> GetInventoryProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>
                                                    searchParameters, bool buildChildRecords, bool loadActiveChild, bool buildImage, int currentPage = 0, int pageSize = 0 , string type = null, string advSearch = null);
        Task<int> GetInventoryProductCount(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters);
        Task<string> GetBOMProductCost(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, int productId);
        Task<string> SaveDuplicateProductDetails(ProductsDTO productsDTO);
        Task<ProductCalendarContainerDTOCollection> GetProductCalendarContainerDTOCollection(int siteId, string manualProductType, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache);
        Task<List<CustomerProfilingGroupDTO>> GetCustomerProfilingGroups(List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>> searchParameters, bool buildChildRecords, bool loadActiveChildRecords/*, int currentPage = 0, int pageSize = 0*/);
        Task<List<CustomerProfilingGroupDTO>> SaveCustomerProfilingGroups(List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList);
        Task<List<ProductGroupDTO>> GetProductGroupDTOList(int id = -1,
                                                           string name = "",
                                                           string isActive = null,
                                                           int siteId = -1,
                                                           bool loadChildRecords = true,
                                                           bool loadActiveChildRecords = true,
                                                           int pageNumber = 0,
                                                           int pageSize = 0);

        Task<int> GetProductGroupDTOListCount(int id = -1,
                                              string name = "",
                                              string isActive = null,
                                              int siteId = -1);

        Task<List<ProductGroupDTO>> SaveProductGroupDTOList(List<ProductGroupDTO> productGroupDTOList);
    }
}

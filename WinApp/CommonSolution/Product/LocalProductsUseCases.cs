/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 *********************************************************************************************
 2.110.0      12-Dec-2020      Deeksha               Created : Web Inventory/POS UI Redesign with REST API
 2.110.1      14-Feb-2021   Mushahid Faizan    Modified : Web Inventory phase 2 changes
 2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 *2.140.00   14-Sep-2021      Prajwal S            Modified :Added Use cases.
 *2.150.0    18-May-2022       Abhishek        Modified: Added CustomerProfileGroupsUseCases for Customer Profile
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class LocalProductsUseCases : LocalUseCases, IProductsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LocalProductsUseCases(ExecutionContext executionContext,string requestGuid) :base(executionContext, requestGuid)
        {
            log.LogMethodEntry(executionContext, requestGuid);
            log.LogMethodExit();
        }

        public async Task<List<ProductsDTO>> GetProducts(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            return await Task<List<ProductsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ProductsList productsListBL = new ProductsList(executionContext);
                List<ProductsDTO> productsDTOList = productsListBL.GetProductsDTOList(searchParameters);
                log.LogMethodExit(productsDTOList);
                return productsDTOList;
            });
        }

        public async Task<int> GetInventoryProductCount(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ProductList productListBL = new ProductList(executionContext);
                int productCount = productListBL.GetProductCount(searchParameters);
                log.LogMethodExit(productCount);
                return productCount;
            });
        }

        public async Task<string> GetBOMProductCost(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, int productId)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ProductList productList = new ProductList();
                List<ProductDTO> products = productList.GetAdancedAllProducts(searchParameters);
                string content = string.Empty;
                if (products != null)
                {
                    object bomCost = productList.GetBOMProductCost(productId);
                    if (bomCost != DBNull.Value)
                    {
                        content = Convert.ToDecimal(bomCost).ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "INVENTORY_COST_FORMAT"));
                    }
                }
                return content;
            });
        }

        public async Task<List<ProductDTO>> GetInventoryProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>
                           searchParameters, bool buildChildRecords, bool loadActiveChild, bool buildImage, int currentPage = 0, int pageSize = 0,
            string type = null, string advSearch = null)
        {
            return await Task<List<ProductDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ProductList productListBL = new ProductList(executionContext);
                List<ProductDTO> productDTOList = productListBL.GetAllProducts(searchParameters, buildChildRecords, loadActiveChild, currentPage, pageSize, null, type, buildImage, advSearch);
                log.LogMethodExit(productDTOList);
                return productDTOList;
            });
        }
        public async Task<string> SaveProducts(List<ProductsDTO> productsDTOList)
        {
            log.LogMethodEntry();
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    if (productsDTOList == null)
                    {
                        throw new ValidationException("productsDTOList is empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (ProductsDTO productsDTO in productsDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                Products productsBL = new Products(executionContext, productsDTO);
                                productsBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> SaveDuplicateProductDetails(ProductsDTO productsDTO)
        {
            log.LogMethodEntry(productsDTO);
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    Products productsBL = new Products(executionContext);
                    productsBL.SaveDuplicateProductDetails(productsDTO.ProductId);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<ProductsContainerDTOCollection> GetProductsContainerDTOCollection(int siteId, string manualType, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);

            return await Task<ProductsContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    ProductsContainerList.Rebuild(siteId);
                }
                ProductsContainerDTOCollection result = ProductsContainerList.GetProductsContainerDTOCollection(siteId, manualType);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });

        }

        /// <summary>
        /// Get Available Product values
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="hash">hash</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns></returns>
        public async Task<ProductCalendarContainerDTOCollection> GetProductCalendarContainerDTOCollection(int siteId, string manualProductType, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);

            return await Task<ProductCalendarContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    ProductsContainerList.Rebuild(siteId);
                }
                ProductCalendarContainerDTOCollection result = ProductsContainerList.GetProductCalendarContainerDTOCollection(siteId, manualProductType, startDateTime, endDateTime);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });

        }

        public async Task<List<CustomerProfilingGroupDTO>> GetCustomerProfilingGroups(List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>> searchParameters,
                                                                       bool buildChildRecords, bool loadActiveChildRecords/*, int currentPage = 0, int pageSize = 0*/)
        {
            return await Task<List<CustomerProfilingGroupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                CustomerProfilingGroupListBL customerProfilingGroupListBL = new CustomerProfilingGroupListBL(executionContext);
                List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList = customerProfilingGroupListBL.GetCustomerProfilingGroups(searchParameters, buildChildRecords, loadActiveChildRecords);
                log.LogMethodExit(customerProfilingGroupDTOList);
                return customerProfilingGroupDTOList;
            });
        }

        public async Task<List<CustomerProfilingGroupDTO>> SaveCustomerProfilingGroups(List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList)
        {
            log.LogMethodEntry(customerProfilingGroupDTOList);
            return await Task<List<CustomerProfilingGroupDTO>>.Factory.StartNew(() =>
            {
                List<CustomerProfilingGroupDTO> customerProfilingGroupList = new List<CustomerProfilingGroupDTO>();
                if (customerProfilingGroupDTOList == null)
                {
                    throw new ValidationException("customerProfilingGroupDTOList is empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        CustomerProfilingGroupListBL customerProfilingGroupListBL = new CustomerProfilingGroupListBL(executionContext, customerProfilingGroupDTOList);
                        customerProfilingGroupList = customerProfilingGroupListBL.Save(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }
                }
                log.LogMethodExit(customerProfilingGroupList);
                return customerProfilingGroupList;
            });
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
            return await Task<List<ProductGroupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(id, name, isActive, siteId, loadChildRecords, loadActiveChildRecords, pageNumber,
                                   pageSize);
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    ProductGroupListBL productgroupListBL = new ProductGroupListBL(executionContext, unitOfWork);
                    SearchParameterList<ProductGroupDTO.SearchByParameters> searchParameters = new SearchParameterList<ProductGroupDTO.SearchByParameters>();
                    searchParameters.Add(ProductGroupDTO.SearchByParameters.IS_ACTIVE, isActive);
                    searchParameters.Add(ProductGroupDTO.SearchByParameters.ID, id);
                    searchParameters.Add(ProductGroupDTO.SearchByParameters.NAME, name);
                    searchParameters.Add(ProductGroupDTO.SearchByParameters.SITE_ID, executionContext.SiteId);
                    List<ProductGroupDTO> result = productgroupListBL.GetProductGroupDTOList(searchParameters, loadChildRecords, loadActiveChildRecords, pageNumber, pageSize);
                    return result;
                }
            });
        }

        public async Task<int> GetProductGroupDTOListCount(int id = -1,
                                                                             string name = "",
                                                                             string isActive = null,
                                                                             int siteId = -1)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(id, name, isActive, siteId);
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    ProductGroupListBL productgroupListBL = new ProductGroupListBL(executionContext, unitOfWork);
                    SearchParameterList<ProductGroupDTO.SearchByParameters> searchParameters = new SearchParameterList<ProductGroupDTO.SearchByParameters>();
                    searchParameters.Add(ProductGroupDTO.SearchByParameters.IS_ACTIVE, isActive);
                    searchParameters.Add(ProductGroupDTO.SearchByParameters.ID, id);
                    searchParameters.Add(ProductGroupDTO.SearchByParameters.NAME, name);
                    searchParameters.Add(ProductGroupDTO.SearchByParameters.SITE_ID, executionContext.SiteId);
                    int result = productgroupListBL.GetProductGroupDTOListCount(searchParameters);
                    return result;
                }
            });
        }

        public async Task<List<ProductGroupDTO>> SaveProductGroupDTOList(List<ProductGroupDTO> productgroupDTOList)
        {
            return await Task<List<ProductGroupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(productgroupDTOList);
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (IsDuplicateRequest())
                    {
                        ProductGroupListBL productgroupListBL = new ProductGroupListBL();
                        List<ProductGroupDTO> result = productgroupListBL.GetProductGroupDTOList(GetEntityGuidList().ToList(), true, false);
                        log.LogMethodExit(result, "Duplicate request");
                        return result;
                    }
                    else
                    {
                        unitOfWork.Begin();
                        ProductGroupListBL productgroupListBL = new ProductGroupListBL(executionContext, unitOfWork);
                        List<ProductGroupDTO> result = productgroupListBL.Save(productgroupDTOList);
                        CreateApplicationRequestLog("ProductGroup", "Save ProductGroup", result.Select(x => x.Guid), unitOfWork.SQLTrx);
                        unitOfWork.Commit();
                        log.LogMethodExit(result);
                        return result;
                    }

                }

            });
        }
    }
}

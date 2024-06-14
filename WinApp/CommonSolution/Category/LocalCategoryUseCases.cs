/********************************************************************************************
 * Project Name - Category 
 * Description  - LocalCategoryUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************
 *2.110.0     07-Oct-2020   Mushahid Faizan    Created as per inventory changes,
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Category
{
    public class LocalCategoryUseCases : ICategoryUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalCategoryUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<DataTable> GetColumnsName(string tableName)
        {
            log.LogMethodEntry(tableName);
            dynamic columnName = null;
            return await Task<DataTable>.Factory.StartNew(() =>
            {
                if (tableName.ToUpper() == "CATEGORY")
                {
                    CategoryList categoryListBL = new CategoryList(executionContext);
                    columnName = categoryListBL.GetCategoryColumnsName();
                }
                if (tableName.ToUpper() == "VENDOR")
                {
                    VendorList vendorListBL = new VendorList(executionContext);
                    columnName = vendorListBL.GetVendorColumnsName();
                }

                log.LogMethodExit(columnName);
                return columnName;
            });
        }

        public async Task<List<CategoryDTO>> GetCategories(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>
                          searchParameters, bool buildChildRecords, bool loadActiveChild, int currentPage = 0, int pageSize = 0)
        {
            return await Task<List<CategoryDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                CategoryList categoryListBL = new CategoryList(executionContext);
                List<CategoryDTO> categoryDTOList = categoryListBL.GetAllCategory(searchParameters, buildChildRecords, loadActiveChild, currentPage, pageSize);
                log.LogMethodExit(categoryDTOList);
                return categoryDTOList;
            });
        }

        public async Task<int> GetCategoryCount(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                CategoryList categoryListBL = new CategoryList(executionContext);
                int categoryCount = categoryListBL.GetCategoriesCount(searchParameters);
                log.LogMethodExit(categoryCount);
                return categoryCount;
            });
        }

        public async Task<string> SaveCategories(List<CategoryDTO> categoryDTOList)
        {
            log.LogMethodEntry("categoryDTOList");
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;

                if (categoryDTOList == null)
                {
                    throw new ValidationException("categoryDTOList is empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (CategoryDTO categoryDTO in categoryDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Category categoryBL = new Category(executionContext, categoryDTO);
                            categoryBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }

                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ;
                        }
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }



        public async Task<string> DeleteCategories(List<CategoryDTO> categoryDTOList)
        {
            log.LogMethodEntry("categoryDTOList");
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (categoryDTOList == null)
                {
                    throw new ValidationException("categoryDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (CategoryDTO categoryDTO in categoryDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Category categoryBL = new Category(executionContext, categoryDTO);
                            categoryBL.DeleteCategory(categoryDTO.CategoryId, parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }

                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw valEx;
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw ex;
                            }
                        }
                    }
                    result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }


        public async Task<CategoryContainerDTOCollection> GetCategoryContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<CategoryContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    CategoryContainerList.Rebuild(siteId);
                }
                CategoryContainerDTOCollection result = CategoryContainerList.GetCategoryContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

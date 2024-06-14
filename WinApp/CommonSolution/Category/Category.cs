/********************************************************************************************
 * Project Name - Category
 * Description  - Business logic file for Category
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *****************************************************************************************************************
 *2.70        29-June-2019  Indrajeet Kumar         Created DeleteCategory() & DeleteCategoryList() method for Hard Deletion.
 *2.70        02-Jul-2019   Dakshakh raj            Modified : Save() method Insert/Update method returns DTO.
 *                                                             Added execution Context object to the constructors.
 *2.70.2        25-Sep-2019   Deeksha                 Added getter property 
 *2.110.0     07-Oct-2020   Mushahid Faizan        Modified as per 3 tier standards, Added methods for Pagination and Excel Sheet functionalities.
 *2.150.0     13-Dec-2022   Abhishek               Modified:Validate() as a part of Web Inventory Redesign.
 *****************************************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Category
{
    /// <summary>
    /// Category
    /// </summary>
    public class Category
    {
        private CategoryDTO categoryDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of Category class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private Category(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.categoryDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="categoryDTO">Parameter of the type CategoryDTO</param>
        /// <param name="executionContext">Parameter of the type ExecutionContext</param>
        public Category(ExecutionContext executionContext, CategoryDTO categoryDTO)
        : this(executionContext)
        {
            log.LogMethodEntry(categoryDTO);
            this.categoryDTO = categoryDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the Category id as the parameter
        /// Would fetch the Category object based on the ID passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="categoryId">CategoryId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Category(ExecutionContext executionContext, int categoryId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(categoryId);
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            categoryDTO = categoryDataHandler.GetCategory(categoryId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TaxDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (string.IsNullOrEmpty(categoryDTO.Name) || string.IsNullOrWhiteSpace(categoryDTO.Name))
            {
                log.Error("Enter Category ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2607, MessageContainerList.GetMessage(executionContext, "Category"));
                throw new ValidationException(errorMessage);
            }
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<CategoryDTO> categoryDTOList = categoryDataHandler.GetCategoryList(searchParameters);

            if (categoryDTOList != null && categoryDTOList.Any())
            {
                if (categoryDTOList.Exists(x => x.Name.ToLower() == categoryDTO.Name.ToLower()) && categoryDTO.CategoryId == -1)
                {
                    log.Error("Duplicate entries detail");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Category"));
                    throw new ValidationException(errorMessage);
                }

                if (categoryDTOList.Exists(x => x.Name.ToLower() == categoryDTO.Name.ToLower() && x.CategoryId != categoryDTO.CategoryId))
                {
                    log.Error("Duplicate Update detail");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Category"));
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Category
        /// Category will be inserted if CategoryId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            if (categoryDTO == null || categoryDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            Validate(sqlTransaction);
            if (categoryDTO.CategoryId < 0)
            {
                categoryDTO = categoryDataHandler.InsertCategory(categoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                categoryDTO.AcceptChanges();
            }
            else
            {
                if (categoryDTO.IsChanged)
                {
                    categoryDTO = categoryDataHandler.UpdateCategory(categoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    categoryDTO.AcceptChanges();
                }
            }
            SaveChild(sqlTransaction);
            categoryDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : AccountingCodeCombinationDTO 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveChild(SqlTransaction sqlTransaction)
        {
            if (categoryDTO.AccountingCodeCombinationDTOList != null &&
                categoryDTO.AccountingCodeCombinationDTOList.Any())
            {
                List<AccountingCodeCombinationDTO> updatedAccountingCodeCombinationDTOList = new List<AccountingCodeCombinationDTO>();
                foreach (var accountingCodeCombinationDTO in categoryDTO.AccountingCodeCombinationDTOList)
                {
                    if (accountingCodeCombinationDTO.ObjectId != categoryDTO.CategoryId)
                    {
                        accountingCodeCombinationDTO.ObjectId = categoryDTO.CategoryId;
                    }
                    if (accountingCodeCombinationDTO.IsChanged)
                    {
                        updatedAccountingCodeCombinationDTOList.Add(accountingCodeCombinationDTO);
                    }
                }
                if (updatedAccountingCodeCombinationDTOList.Any())
                {
                    AccountingCodeCombinationList accountingCodeCombinationList = new AccountingCodeCombinationList(executionContext, updatedAccountingCodeCombinationDTOList);
                    accountingCodeCombinationList.SaveUpdateAccountingCodeList(sqlTransaction);
                }
            }
        }


        /// <summary>
        /// Delete the DeleteCategory record - Hard Deletion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public void DeleteCategory(int categoryId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(categoryId);
            try
            {
                CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
                categoryDataHandler.DeleteCategory(categoryId);
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        public CategoryDTO GetCategoryDTO { get { return categoryDTO; } }
    }
    /// <summary>
    ///  Manages the list of category
    /// </summary>
    public class CategoryList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
        private ExecutionContext executionContext;
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        public CategoryList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CategoryList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with executionContext and categoryDTOList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="categoryDTOList"></param>
        public CategoryList(ExecutionContext executionContext, List<CategoryDTO> categoryDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, categoryDTOList);
            this.categoryDTOList = categoryDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Category list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>List of CategoryDTO</returns>
        public List<CategoryDTO> GetAllCategory(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters,
             bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            categoryDTOList = categoryDataHandler.GetCategoryList(searchParameters, currentPage, pageSize);
            if (categoryDTOList != null && categoryDTOList.Any() && loadChildRecords)
            {
                Build(categoryDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(categoryDTOList);
            return categoryDTOList;
        }
        private void Build(List<CategoryDTO> categoryDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, CategoryDTO> categoryDTODictionary = new Dictionary<int, CategoryDTO>();
            List<int> categoryIdList = new List<int>();
            for (int i = 0; i < categoryDTOList.Count; i++)
            {
                if (categoryDTODictionary.ContainsKey(categoryDTOList[i].CategoryId))
                {
                    continue;
                }
                categoryDTODictionary.Add(categoryDTOList[i].CategoryId, categoryDTOList[i]);
                categoryIdList.Add(categoryDTOList[i].CategoryId);
            }
            AccountingCodeCombinationList accountingCodeCombinationList = new AccountingCodeCombinationList(executionContext);
            List<AccountingCodeCombinationDTO> accountingCodeCombinationDTOList = accountingCodeCombinationList.GetAccountingCodeDTOList(categoryIdList, activeChildRecords, sqlTransaction);

            if (accountingCodeCombinationDTOList != null && accountingCodeCombinationDTOList.Any())
            {
                for (int i = 0; i < accountingCodeCombinationDTOList.Count; i++)
                {
                    if (categoryDTODictionary.ContainsKey(accountingCodeCombinationDTOList[i].ObjectId) == false)
                    {
                        continue;
                    }
                    CategoryDTO categoryDTO = categoryDTODictionary[accountingCodeCombinationDTOList[i].ObjectId];
                    if (categoryDTO.AccountingCodeCombinationDTOList == null)
                    {
                        categoryDTO.AccountingCodeCombinationDTOList = new List<AccountingCodeCombinationDTO>();
                    }
                    categoryDTO.AccountingCodeCombinationDTOList.Add(accountingCodeCombinationDTOList[i]);
                }
            }
        }
        /// <summary>
        /// Returns the no of Categories matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetCategoriesCount(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            int categoriesCount = categoryDataHandler.GetCategoriesCount(searchParameters);
            log.LogMethodExit(categoriesCount);
            return categoriesCount;
        }

        /// <summary>
        /// Retriving category by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the category</param>
        /// <returns> List of CategoryDTO </returns>
        public List<CategoryDTO> GetCategoryList(string sqlQuery, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlQuery, sqlTransaction);
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            var result = categoryDataHandler.GetCategoryList(sqlQuery);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the column name list of the Category table.
        /// </summary>
        /// <returns>Category columns</returns>
        public DataTable GetCategoryColumnsName(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CategoryDataHandler autoPatchDepPlanDataHandler = new CategoryDataHandler(sqlTransaction);
            var result = autoPatchDepPlanDataHandler.GetCategoryColumns();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// This method is will return Sheet object for Category.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            categoryDTOList = categoryDataHandler.GetCategoryList(searchParameters);

            CategoryExcelDTODefinition categoryExcelDTODefinition = new CategoryExcelDTODefinition(executionContext, "");
            ///Building headers from CategoryExcelDTODefinition
            categoryExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (categoryDTOList != null && categoryDTOList.Any())
            {
                foreach (CategoryDTO categoryDTO in categoryDTOList)
                {
                    categoryExcelDTODefinition.Configure(categoryDTO);

                    Row row = new Row();
                    categoryExcelDTODefinition.Serialize(row, categoryDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }

        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            CategoryExcelDTODefinition categoryExcelDTODefinition = new CategoryExcelDTODefinition(executionContext, "");
            List<CategoryDTO> rowCategoryDTOList = new List<CategoryDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    CategoryDTO rowCategoryDTO = (CategoryDTO)categoryExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowCategoryDTOList.Add(rowCategoryDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowCategoryDTOList != null && rowCategoryDTOList.Any())
                    {
                        CategoryList categoryListBL = new CategoryList(executionContext, rowCategoryDTOList);
                        categoryListBL.Save(sqlTransaction);
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx.Message, valEx);
                    throw valEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;
        }


        /// <summary>
        /// Validates and saves the categoryDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (categoryDTOList == null ||
                categoryDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            List<CategoryDTO> updatedCategoryDTOList = new List<CategoryDTO>(categoryDTOList.Count);
            for (int i = 0; i < categoryDTOList.Count; i++)
            {
                if (categoryDTOList[i].IsChanged == false)
                {
                    continue;
                }
                Category category = new Category(executionContext, categoryDTOList[i]);
                updatedCategoryDTOList.Add(categoryDTOList[i]);
            }
            if (updatedCategoryDTOList.Any() == false)
            {
                log.LogMethodExit(null, "Nothing changed.");
                return;
            }
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            categoryDataHandler.Save(updatedCategoryDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the segmentDefinitionSourceMapDTOList 
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        public void SaveList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (categoryDTOList == null ||
                categoryDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < categoryDTOList.Count; i++)
            {
                var categoryDTO = categoryDTOList[i];
                if (categoryDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    Category categoryBL = new Category(executionContext, categoryDTO);
                    categoryBL.Save(sqlTransaction);
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
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving categoryDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("categoryDTO", categoryDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the CategoryList based on CategoryId
        /// </summary>
        public void DeleteCategoryList()
        {
            log.LogMethodEntry();
            if (categoryDTOList != null && categoryDTOList.Count > 0)
            {
                foreach (CategoryDTO CategoryDTO in categoryDTOList)
                {
                    if (CategoryDTO.IsChanged && CategoryDTO.IsActive == false)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();

                                Category category = new Category(executionContext, CategoryDTO);
                                category.DeleteCategory(CategoryDTO.CategoryId, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        public DateTime? GetCategoryLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler(sqlTransaction);
            DateTime? result = categoryDataHandler.GetCategoryLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

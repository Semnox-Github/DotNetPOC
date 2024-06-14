/********************************************************************************************
 * Project Name - Redemption 
 * Description  - CategoryExcelDTODefinition  object of Category Excel information
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By             Remarks          
 *********************************************************************************************
 *2.110.0    03-Nov-2020        Mushahid Faizan          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Category
{
    public class CategoryExcelDTODefinition : ComplexAttributeDefinition
    {
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public CategoryExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(CategoryDTO))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("CategoryId", "CategoryId", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Name", "Category Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ParentCategoryId", "Parent Category", new CategoryValueConverter(executionContext)));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedUserId", "LastUpdatedUserId", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdateDate", "LastUpdateDate", new NullableDateTimeValueConverter()));

        }
    }

    class CategoryValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<KeyValuePair<int, CategoryDTO>> categoryIdCategorDTOyKeyValuePair;
        List<KeyValuePair<string, CategoryDTO>> categoryNameCategoryDTOKeyValuePair;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CategoryValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            categoryNameCategoryDTOKeyValuePair = new List<KeyValuePair<string, CategoryDTO>>();
            categoryIdCategorDTOyKeyValuePair = new List<KeyValuePair<int, CategoryDTO>>();
            List<CategoryDTO> categoryList = new List<CategoryDTO>();

            CategoryList categoryDTOList = new CategoryList(executionContext);
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            searchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            categoryList = categoryDTOList.GetAllCategory(searchParams);
            if (categoryList != null && categoryList.Count > 0)
            {
                foreach (CategoryDTO categoryDTO in categoryList)
                {
                    categoryIdCategorDTOyKeyValuePair.Add(new KeyValuePair<int, CategoryDTO>(categoryDTO.CategoryId, categoryDTO));
                    categoryNameCategoryDTOKeyValuePair.Add(new KeyValuePair<string, CategoryDTO>(categoryDTO.Name, categoryDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts categoryname to categoryid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int categoryId = -1;
            for (int i = 0; i < categoryNameCategoryDTOKeyValuePair.Count; i++)
            {
                if (categoryNameCategoryDTOKeyValuePair[i].Key == stringValue)
                {
                    categoryNameCategoryDTOKeyValuePair[i] = new KeyValuePair<string, CategoryDTO>(categoryNameCategoryDTOKeyValuePair[i].Key, categoryNameCategoryDTOKeyValuePair[i].Value);
                    categoryId = categoryNameCategoryDTOKeyValuePair[i].Value.CategoryId;
                }
            }

            log.LogMethodExit(categoryId);
            return categoryId;
        }
        /// <summary>
        /// Converts categoryid to categoryname
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string categoryName = string.Empty;

            for (int i = 0; i < categoryIdCategorDTOyKeyValuePair.Count; i++)
            {
                if (categoryIdCategorDTOyKeyValuePair[i].Key == Convert.ToInt32(value))
                {
                    categoryIdCategorDTOyKeyValuePair[i] = new KeyValuePair<int, CategoryDTO>(categoryIdCategorDTOyKeyValuePair[i].Key, categoryIdCategorDTOyKeyValuePair[i].Value);

                    categoryName = categoryIdCategorDTOyKeyValuePair[i].Value.Name;
                }
            }
            log.LogMethodExit(categoryName);
            return categoryName;
        }
    }

}

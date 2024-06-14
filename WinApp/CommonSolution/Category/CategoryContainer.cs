/********************************************************************************************
 * Project Name - Category
 * Description  - CategoryContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      20-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Category
{
    public class CategoryContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, CategoryContainerDTO> categoryIdcategoryContainerDTODictionary = new Dictionary<int, CategoryContainerDTO>();
        private readonly Dictionary<int, CategoryDTO> categoryIdCategoryDTODictionary = new Dictionary<int, CategoryDTO>();
        private readonly CategoryContainerDTOCollection categoryContainerDTOCollection;
        private readonly DateTime? categoryModuleLastUpdateTime;
        private readonly int siteId;
        private readonly List<CategoryDTO> categoryDTOList;

        public CategoryContainer(int siteId) : this(siteId, GetCategoryDTOList(siteId), GetCategoryModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public CategoryContainer(int siteId, List<CategoryDTO> categoryDTOList, DateTime? categoryModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.categoryDTOList = categoryDTOList;
            this.categoryModuleLastUpdateTime = categoryModuleLastUpdateTime;
            foreach (var categoryDTO in categoryDTOList)
            {
                if (categoryIdCategoryDTODictionary.ContainsKey(categoryDTO.CategoryId))
                {
                    continue;
                }
                categoryIdCategoryDTODictionary.Add(categoryDTO.CategoryId, categoryDTO);
            }
            List<CategoryContainerDTO> categoryContainerDTOList = new List<CategoryContainerDTO>();
            foreach (CategoryDTO categoryDTO in categoryDTOList)
            {
                if (categoryIdcategoryContainerDTODictionary.ContainsKey(categoryDTO.CategoryId))
                {
                    continue;
                }
                CategoryContainerDTO categoryContainerDTO = new CategoryContainerDTO(categoryDTO.CategoryId, categoryDTO.Name);
                List<int> childCategoryIdList = new List<int>();
                List<int> parentCategoryIdList = new List<int>();
                CollectChildCategoryIdList(categoryDTO.CategoryId, childCategoryIdList);
                CollectParentCategoryIdList(categoryDTO.CategoryId, parentCategoryIdList);
                categoryContainerDTO.ChildCategoryIdList = childCategoryIdList;
                categoryContainerDTO.ParentCategoryIdList = parentCategoryIdList;
                categoryContainerDTOList.Add(categoryContainerDTO);
                categoryIdcategoryContainerDTODictionary.Add(categoryDTO.CategoryId, categoryContainerDTO);
            }
            categoryContainerDTOCollection = new CategoryContainerDTOCollection(categoryContainerDTOList);
            log.LogMethodExit();
        }

        private void CollectParentCategoryIdList(int categoryId, List<int> parentCategoryIdList)
        {
            log.LogMethodEntry(categoryId, parentCategoryIdList);
            if (categoryIdCategoryDTODictionary.ContainsKey(categoryId) == false)
            {
                return;
            }
            CategoryDTO categoryDTO = categoryIdCategoryDTODictionary[categoryId];
            if (categoryDTO.ParentCategoryId == -1 || 
                parentCategoryIdList.Contains(categoryDTO.ParentCategoryId))
            {
                return;
            }
            parentCategoryIdList.Add(categoryDTO.ParentCategoryId);
            CollectParentCategoryIdList(categoryDTO.ParentCategoryId, parentCategoryIdList);
        }

        internal List<CategoryContainerDTO> GetCategoryContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(categoryContainerDTOCollection.CategoryContainerDTOList);
            return categoryContainerDTOCollection.CategoryContainerDTOList;
        }

        private void CollectChildCategoryIdList(int categoryId, List<int> childCategoryIdList)
        {
            log.LogMethodEntry(categoryId, childCategoryIdList);
            foreach (var categoryDTO in categoryDTOList)
            {
                if(childCategoryIdList.Contains(categoryDTO.CategoryId))
                {
                    continue;
                }
                if (categoryDTO.ParentCategoryId == categoryId)
                {
                    childCategoryIdList.Add(categoryDTO.CategoryId);
                    CollectChildCategoryIdList(categoryDTO.CategoryId, childCategoryIdList);
                }
            }
            log.LogMethodExit();
        }


        private static List<CategoryDTO> GetCategoryDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<CategoryDTO> categoryDTOList = null;
            try
            {
                CategoryList categoryList = new CategoryList();
                List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, siteId.ToString()));
                categoryDTOList = categoryList.GetAllCategory(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the categories.", ex);
            }

            if (categoryDTOList == null)
            {
                categoryDTOList = new List<CategoryDTO>();
            }
            log.LogMethodExit(categoryDTOList);
            return categoryDTOList;
        }

        private static DateTime? GetCategoryModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                CategoryList categoryList = new CategoryList();
                result = categoryList.GetCategoryLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the category max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public CategoryContainerDTO GetCategoryContainerDTO(int categoryId)
        {
            log.LogMethodEntry(categoryId);
            if (categoryIdcategoryContainerDTODictionary.ContainsKey(categoryId) == false)
            {
                string errorMessage = "category with category Id :" + categoryId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            CategoryContainerDTO result = categoryIdcategoryContainerDTODictionary[categoryId]; ;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// gets the CategoryContainer 
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public CategoryContainerDTO GetCategoryContainerDTOOrDefault(int categoryId)
        {
            log.LogMethodEntry(categoryId);
            if (categoryIdcategoryContainerDTODictionary.ContainsKey(categoryId) == false)
            {
                string message = "Products with categoryId : " + categoryId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = categoryIdcategoryContainerDTODictionary[categoryId];
            log.LogMethodExit(result);
            return result;
        }

        public CategoryContainerDTOCollection GetCategoryContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(categoryContainerDTOCollection);
            return categoryContainerDTOCollection;
        }

        public CategoryContainer Refresh()
        {
            log.LogMethodEntry();
            CategoryList categoryList = new CategoryList();
            DateTime? updateTime = categoryList.GetCategoryLastUpdateTime(siteId);
            if (categoryModuleLastUpdateTime.HasValue
                && categoryModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in category since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            CategoryContainer result = new CategoryContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public IEnumerable<CategoryContainerDTO> GetActiveCategoryContainerDTOList(Func<CategoryContainerDTO, bool> predicate)
        {
            log.LogMethodEntry(predicate);
            IEnumerable<CategoryContainerDTO> result;
            result = categoryContainerDTOCollection.CategoryContainerDTOList.Where(predicate);
            log.LogMethodExit(result);
            return result;
        }
    }
}

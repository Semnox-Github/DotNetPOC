/********************************************************************************************
 * Project Name - Category
 * Description  - CategoryContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      20-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.Category
{
    public class CategoryContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CategoryContainer> categoryContainerCache = new Cache<int, CategoryContainer>();
        private static Timer refreshTimer;

        static CategoryContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = categoryContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CategoryContainer categoryContainer;
                if (categoryContainerCache.TryGetValue(uniqueKey, out categoryContainer))
                {
                    categoryContainerCache[uniqueKey] = categoryContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static CategoryContainer GetCategoryContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            CategoryContainer result = categoryContainerCache.GetOrAdd(siteId, (k) => new CategoryContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static CategoryContainerDTOCollection GetCategoryContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            CategoryContainer container = GetCategoryContainer(siteId);
            CategoryContainerDTOCollection result = container.GetCategoryContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CategoryContainer categoryContainer = GetCategoryContainer(siteId);
            categoryContainerCache[siteId] = categoryContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CategoryContainerDTO based on the site and categoryId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="categoryId">option value categoryId</param>
        /// <returns></returns>
        public static CategoryContainerDTO GetCategoryContainerDTO(int siteId, int categoryId)
        {
            log.LogMethodEntry(siteId, categoryId);
            CategoryContainer categoryContainer = GetCategoryContainer(siteId);
            CategoryContainerDTO result = categoryContainer.GetCategoryContainerDTO(categoryId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the CategoryContainerDTOList based on the site
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="categoryId">option value categoryId</param>
        /// <returns></returns>
        public static List<CategoryContainerDTO> GetCategoryContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            CategoryContainer categoryContainer = GetCategoryContainer(siteId);
            var result = categoryContainer.GetCategoryContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the CategoryContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="categoryId">categoryId</param>
        /// <returns></returns>
        public static CategoryContainerDTO GetCategoryContainerDTO(ExecutionContext executionContext, int categoryId)
        {
            log.LogMethodEntry(executionContext, categoryId);
            CategoryContainerDTO categoryContainerDTO = GetCategoryContainerDTO(executionContext.GetSiteId(), categoryId);
            log.LogMethodExit(categoryContainerDTO);
            return categoryContainerDTO;
        }

        public static IEnumerable<CategoryContainerDTO> GetActiveCategoryContainerDTOList(int siteId, Func<CategoryContainerDTO, bool> predicate)
        {
            log.LogMethodEntry(siteId);
            CategoryContainer container = GetCategoryContainer(siteId);
            var result = container.GetActiveCategoryContainerDTOList(predicate);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the CategoryContainerDTO based on the site and executionContext else returns null
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static CategoryContainerDTO GetCategoryContainerDTOOrDefault(ExecutionContext executionContext, int categoryId)
        {
            log.LogMethodEntry(executionContext, categoryId);
            log.LogMethodExit();
            return GetCategoryContainerDTOOrDefault(executionContext.SiteId, categoryId);
        }

        /// <summary>
        /// /// Gets the CategoryContainerDTO based on the site and categoryId else returns null
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static CategoryContainerDTO GetCategoryContainerDTOOrDefault(int siteId, int categoryId)
        {
            log.LogMethodEntry(siteId, categoryId);
            CategoryContainer container = GetCategoryContainer(siteId);
            var result = container.GetCategoryContainerDTOOrDefault(categoryId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

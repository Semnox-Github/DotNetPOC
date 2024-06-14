/********************************************************************************************
* Project Name - ViewContainer
* Description  - CategoryViewContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;

namespace Semnox.Parafait.ViewContainer
{
    public class CategoryViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CategoryViewContainer> categoryViewContainerCache = new Cache<int, CategoryViewContainer>();
        private static Timer refreshTimer;

        static CategoryViewContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = categoryViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CategoryViewContainer categoryViewContainer;
                if (categoryViewContainerCache.TryGetValue(uniqueKey, out categoryViewContainer))
                {
                    categoryViewContainerCache[uniqueKey] = categoryViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static CategoryViewContainer GetCategoryViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = categoryViewContainerCache.GetOrAdd(siteId, (k) => new CategoryViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the CategoryContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="categoryId">category identifier</param>
        /// <returns></returns>
        public static CategoryContainerDTO GetCategoryContainerDTO(ExecutionContext executionContext, int categoryId)
        {
            log.LogMethodEntry(executionContext, categoryId);
            CategoryContainerDTO result = GetCategoryContainerDTO(executionContext.SiteId, categoryId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the CategoryContainerDTOList for a given context
        /// </summary>
        /// <param name="siteId">execution context</param>
        /// <param name="categoryId">category identifier</param>
        /// <returns></returns>
        public static CategoryContainerDTO GetCategoryContainerDTO(int siteId, int categoryId)
        {
            log.LogMethodEntry(siteId, categoryId);
            CategoryViewContainer categoryViewContainer = GetCategoryViewContainer(siteId);
            CategoryContainerDTO result = categoryViewContainer.GetCategoryContainerDTO(categoryId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the CategoryContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<CategoryContainerDTO> GetCategoryContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            CategoryViewContainer categoryViewContainer = GetCategoryViewContainer(executionContext.SiteId);
            List<CategoryContainerDTO> result = categoryViewContainer.GetCategoryContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static CategoryContainerDTOCollection GetCategoryContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            CategoryViewContainer container = GetCategoryViewContainer(siteId);
            CategoryContainerDTOCollection categoryContainerDTOCollection = container.GetCategoryContainerDTOCollection(hash);
            return categoryContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CategoryViewContainer container = GetCategoryViewContainer(siteId);
            categoryViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}

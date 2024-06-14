/********************************************************************************************
* Project Name - ViewContainer
* Description  - CategoryViewContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Parafait.Category;
using Semnox.Parafait.Inventory;

namespace Semnox.Parafait.ViewContainer
{
    class CategoryViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CategoryContainerDTOCollection categoryContainerDTOCollection;
        private readonly ConcurrentDictionary<int, CategoryContainerDTO> categoryContainerDTODictionary = new ConcurrentDictionary<int, CategoryContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="categoryContainerDTOCollection">categoryContainerDTOCollection</param>
        internal CategoryViewContainer(int siteId, CategoryContainerDTOCollection categoryContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, categoryContainerDTOCollection);
            this.siteId = siteId;
            this.categoryContainerDTOCollection = categoryContainerDTOCollection;
            if (categoryContainerDTOCollection != null &&
                categoryContainerDTOCollection.CategoryContainerDTOList != null &&
               categoryContainerDTOCollection.CategoryContainerDTOList.Any())
            {
                foreach (var categoryContainerDTO in categoryContainerDTOCollection.CategoryContainerDTOList)
                {
                    categoryContainerDTODictionary[categoryContainerDTO.CategoryId] = categoryContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal CategoryViewContainer(int siteId)
              : this(siteId, GetCategoryContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static CategoryContainerDTOCollection GetCategoryContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            CategoryContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ICategoryUseCases categoryUseCases = InventoryUseCaseFactory.GetCategoryUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<CategoryContainerDTOCollection> task = categoryUseCases.GetCategoryContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving CategoryContainerDTOCollection.", ex);
                result = new CategoryContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        internal CategoryContainerDTO GetCategoryContainerDTO(int categoryId)
        {
            log.LogMethodEntry(categoryId);
            if (categoryContainerDTODictionary.ContainsKey(categoryId) == false)
            {
                string errorMessage = "category with category Id :" + categoryId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            CategoryContainerDTO result = categoryContainerDTODictionary[categoryId]; ;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in CategoryContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal CategoryContainerDTOCollection GetCategoryContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (categoryContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(categoryContainerDTOCollection);
            return categoryContainerDTOCollection;
        }
        internal List<CategoryContainerDTO> GetCategoryContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(categoryContainerDTOCollection.CategoryContainerDTOList);
            return categoryContainerDTOCollection.CategoryContainerDTOList;
        }
        internal CategoryViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            CategoryContainerDTOCollection latestCategoryContainerDTOCollection = GetCategoryContainerDTOCollection(siteId, categoryContainerDTOCollection.Hash, rebuildCache);
            if (latestCategoryContainerDTOCollection == null ||
                latestCategoryContainerDTOCollection.CategoryContainerDTOList == null ||
                latestCategoryContainerDTOCollection.CategoryContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            CategoryViewContainer result = new CategoryViewContainer(siteId, latestCategoryContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}

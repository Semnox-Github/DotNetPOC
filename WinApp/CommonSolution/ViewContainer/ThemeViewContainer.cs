using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    public class ThemeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ThemeContainerDTOCollection themeDTOCollection;
        private readonly ConcurrentDictionary<int, ThemeContainerDTO> themeContainerDTODictionary = new ConcurrentDictionary<int, ThemeContainerDTO>();
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal ThemeViewContainer(int siteId, ThemeContainerDTOCollection themeDTOCollection)
        {
            log.LogMethodEntry(siteId, themeDTOCollection);
            this.siteId = siteId;
            this.themeDTOCollection = themeDTOCollection;
            //lastRefreshTime = DateTime.Now;
            if (themeDTOCollection != null &&
               themeDTOCollection.ThemeContainerDTOList != null &&
               themeDTOCollection.ThemeContainerDTOList.Any())
            {
                foreach (var themeContainerDTO in themeDTOCollection.ThemeContainerDTOList)
                {
                    themeContainerDTODictionary[themeContainerDTO.Id] = themeContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal ThemeViewContainer(int siteId) :
            this(siteId, GetThemeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static ThemeContainerDTOCollection GetThemeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            ThemeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IThemeUseCases themeUseCases = ThemeUseCaseFactory.GetThemeUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ThemeContainerDTOCollection> themeViewDTOCollectionTask = themeUseCases.GetThemeContainerDTOCollection(siteId, hash, rebuildCache);
                    themeViewDTOCollectionTask.Wait();
                    result = themeViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ThemeContainerDTOCollection.", ex);
                result = new ThemeContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in ThemeDTOCollection
        /// </summary>
        /// <returns></returns>
        internal ThemeContainerDTOCollection GetThemeDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (themeDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(themeDTOCollection);
            return themeDTOCollection;
        }

        internal List<ThemeContainerDTO> GetThemeContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(themeDTOCollection.ThemeContainerDTOList);
            return themeDTOCollection.ThemeContainerDTOList;
        }

        /// <summary>
        /// returns the ThemeContainerDTO for the ThemeId
        /// </summary>
        /// <param name="ThemeId"></param>
        /// <returns></returns>
        public ThemeContainerDTO GetThemeContainerDTO(int ThemeId)
        {
            log.LogMethodEntry(ThemeId);
            if (themeContainerDTODictionary.ContainsKey(ThemeId) == false)
            {
                string errorMessage = "Theme with Theme Id :" + ThemeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ThemeContainerDTO result = themeContainerDTODictionary[ThemeId];
            log.LogMethodExit(result);
            return result;
        }


        internal ThemeViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            ThemeContainerDTOCollection latestThemeDTOCollection = GetThemeContainerDTOCollection(siteId, themeDTOCollection.Hash, true);
            if (latestThemeDTOCollection == null ||
                latestThemeDTOCollection.ThemeContainerDTOList == null ||
                latestThemeDTOCollection.ThemeContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ThemeViewContainer result = new ThemeViewContainer(siteId, latestThemeDTOCollection);
            log.LogMethodExit(result);
            return result;
        }


    }
}


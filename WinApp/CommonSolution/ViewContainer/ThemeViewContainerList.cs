using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the Theme container object
    /// </summary>
    public class ThemeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, ThemeViewContainer> themeViewContainerDictionary = new ConcurrentDictionary<int, ThemeViewContainer>();
        private static Timer refreshTimer;
        static ThemeViewContainerList()
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
            List<int> uniqueKeyList = themeViewContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                ThemeViewContainer themeViewContainer;
                if (themeViewContainerDictionary.TryGetValue(uniqueKey, out themeViewContainer))
                {
                    themeViewContainerDictionary[uniqueKey] = themeViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ThemeViewContainer GetThemeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (themeViewContainerDictionary.ContainsKey(siteId) == false)
            {
                themeViewContainerDictionary[siteId] = new ThemeViewContainer(siteId);
            }
            ThemeViewContainer result = themeViewContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ThemeContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// /// <param name="rebuildCache">hash</param>
        /// <returns></returns>
        public static ThemeContainerDTOCollection GetThemeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            ThemeViewContainer container = GetThemeViewContainer(siteId);
            ThemeContainerDTOCollection themeContainerDTOCollection = container.GetThemeDTOCollection(hash);
            return themeContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ThemeViewContainer container = GetThemeViewContainer(siteId);
            themeViewContainerDictionary[siteId] = container.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ThemeContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static ThemeContainerDTO GetThemeContainerDTO(ExecutionContext executionContext)
        {
            return GetThemeContainerDTO(executionContext.SiteId, executionContext.MachineId);
        }

        /// <summary>
        /// Returns the ThemeContainerDTO based on the siteId and ThemeId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="ThemeId">Theme Id</param>
        /// <returns></returns>
        public static ThemeContainerDTO GetThemeContainerDTO(int siteId, int ThemeId)
        {
            log.LogMethodEntry(siteId, ThemeId);
            ThemeViewContainer themeViewContainer = GetThemeViewContainer(siteId);
            ThemeContainerDTO result = themeViewContainer.GetThemeContainerDTO(ThemeId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the Theme role container DTO list 
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static List<ThemeContainerDTO> GetThemeContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ThemeViewContainer themeViewContainer = GetThemeViewContainer(executionContext.SiteId);
            List<ThemeContainerDTO> result = themeViewContainer.GetThemeContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
    }
}

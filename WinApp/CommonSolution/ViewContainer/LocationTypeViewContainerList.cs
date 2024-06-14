/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationTypeViewContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      15-Jan-2021      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Location;

namespace Semnox.Parafait.ViewContainer
{
    public class LocationTypeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LocationTypeViewContainer> locationTypeViewContainerCache = new Cache<int, LocationTypeViewContainer>();
        private static Timer refreshTimer;
        static LocationTypeViewContainerList()
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
            var uniqueKeyList = locationTypeViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LocationTypeViewContainer locationTypeViewContainer;
                if (locationTypeViewContainerCache.TryGetValue(uniqueKey, out locationTypeViewContainer))
                {
                    locationTypeViewContainerCache[uniqueKey] = locationTypeViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static LocationTypeViewContainer GetLocationTypeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = locationTypeViewContainerCache.GetOrAdd(siteId, (k) => new LocationTypeViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the LocationTypeContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<LocationTypeContainerDTO> GetLocationTypeContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            LocationTypeViewContainer locationTypeViewContainer = GetLocationTypeViewContainer(executionContext.SiteId);
            List<LocationTypeContainerDTO> result = locationTypeViewContainer.GetLocationTypeContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        public static LocationTypeContainerDTOCollection GetLocationTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            LocationTypeViewContainer container = GetLocationTypeViewContainer(siteId);
            LocationTypeContainerDTOCollection locationTypeContainerDTOCollection = container.GetLocationTypeDTOCollection(hash);
            return locationTypeContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LocationTypeViewContainer container = GetLocationTypeViewContainer(siteId);
            locationTypeViewContainerCache[siteId] = container.Refresh();
            log.LogMethodExit();
        }
    }
}

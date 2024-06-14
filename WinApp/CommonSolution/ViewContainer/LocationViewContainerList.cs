/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationViewContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LocationViewContainer> locationViewContainerCache = new Cache<int, LocationViewContainer>();
        private static Timer refreshTimer;
        static LocationViewContainerList()
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
            var uniqueKeyList = locationViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LocationViewContainer locationViewContainer;
                if (locationViewContainerCache.TryGetValue(uniqueKey, out locationViewContainer))
                {
                    locationViewContainerCache[uniqueKey] = locationViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static LocationViewContainer GetLocationViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = locationViewContainerCache.GetOrAdd(siteId, (k)=> new LocationViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<LocationContainerDTO> GetLocationContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            LocationViewContainer locatioViewContainer = GetLocationViewContainer(executionContext.SiteId);
            List<LocationContainerDTO> result = locatioViewContainer.GetLocationContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        public static LocationContainerDTOCollection GetLocationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            LocationViewContainer container = GetLocationViewContainer(siteId);
            LocationContainerDTOCollection locationContainerDTOCollection = container.GetLocationDTOCollection(hash);
            return locationContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LocationViewContainer container = GetLocationViewContainer(siteId);
            locationViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}

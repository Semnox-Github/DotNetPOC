/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationViewMasterList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationViewMasterList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, LocationViewContainer> locationViewContainerDictionary = new ConcurrentDictionary<int, LocationViewContainer>();
        private static Timer refreshTimer;
        static LocationViewMasterList()
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
            List<int> uniqueKeyList = locationViewContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                LocationViewContainer locationViewContainer;
                if (locationViewContainerDictionary.TryGetValue(uniqueKey, out locationViewContainer))
                {
                    locationViewContainerDictionary[uniqueKey] = locationViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static LocationViewContainer GetLocationViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (locationViewContainerDictionary.ContainsKey(siteId) == false)
            {
                locationViewContainerDictionary[siteId] = new LocationViewContainer(siteId);
            }
            LocationViewContainer result = locationViewContainerDictionary[siteId];
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
            locationViewContainerDictionary[siteId] = container.Refresh();
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationTypeContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      15-Jan-2021      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationTypeContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LocationTypeContainer> locationTypeContainerCache = new Cache<int, LocationTypeContainer>();
        private static Timer refreshTimer;

        static LocationTypeContainerList()
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
            var uniqueKeyList = locationTypeContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LocationTypeContainer locationTypeContainer;
                if (locationTypeContainerCache.TryGetValue(uniqueKey, out locationTypeContainer))
                {
                    locationTypeContainerCache[uniqueKey] = locationTypeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static LocationTypeContainer GetLocationTypeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            LocationTypeContainer result = locationTypeContainerCache.GetOrAdd(siteId, (k)=>new LocationTypeContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static List<LocationTypeContainerDTO> GetLocationTypeContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            LocationTypeContainer container = GetLocationTypeContainer(siteId);
            List<LocationTypeContainerDTO> locationTypeContainerDTOList = container.GetLocationTypeContainerDTOList();
            log.LogMethodExit(locationTypeContainerDTOList);
            return locationTypeContainerDTOList;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LocationTypeContainer locationTypeContainer = GetLocationTypeContainer(siteId);
            locationTypeContainerCache[siteId] = locationTypeContainer.Refresh();
            log.LogMethodExit();
        }
    }
}

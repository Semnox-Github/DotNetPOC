/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationMasterList class 
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
    public class LocationContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LocationContainer> locationContainerDictionary = new Cache<int, LocationContainer>();
        private static Timer refreshTimer;

        static LocationContainerList()
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
            var uniqueKeyList = locationContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LocationContainer locationContainer;
                if (locationContainerDictionary.TryGetValue(uniqueKey, out locationContainer))
                {
                    locationContainerDictionary[uniqueKey] = locationContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static LocationContainer GetLocationContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            LocationContainer result = locationContainerDictionary.GetOrAdd(siteId, (k)=>new LocationContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static List<LocationContainerDTO> GetLocationContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            LocationContainer container = GetLocationContainer(siteId);
            List<LocationContainerDTO> locationContainerDTOList = container.GetLocationContainerDTOList();
            log.LogMethodExit(locationContainerDTOList);
            return locationContainerDTOList;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LocationContainer redemptionCurrencyContainer = GetLocationContainer(siteId);
            locationContainerDictionary[siteId] = redemptionCurrencyContainer.Refresh();
            log.LogMethodExit();
        }

    }
}

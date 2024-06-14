/********************************************************************************************
 * Project Name - Product
 * Description  - FacilityList class to get the List of Facility from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0     16-Aug-2021       Prajwal S                Created :
 ********************************************************************************************/

using System;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    public static class FacilityContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, FacilityContainer> facilityContainerCache = new Cache<int, FacilityContainer>();
        private static Timer refreshTimer;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        static FacilityContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        /// <summary>
        /// Container Refresh after elapsing the time set for refresh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            var uniqueKeyList = facilityContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                FacilityContainer facilityContainer;
                if (facilityContainerCache.TryGetValue(uniqueKey, out facilityContainer))
                {
                    facilityContainerCache[uniqueKey] = facilityContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Container Data for the Site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static FacilityContainer GetFacilityContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            FacilityContainer result = facilityContainerCache.GetOrAdd(siteId, (k) => new FacilityContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static FacilityContainerDTOCollection GetFacilityContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            FacilityContainer container = GetFacilityContainer(siteId);
            FacilityContainerDTOCollection result = container.GetFacilityContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            FacilityContainer facilityContainer = GetFacilityContainer(siteId);
            facilityContainerCache[siteId] = facilityContainer.Refresh();
            log.LogMethodExit();
        }

    }
}

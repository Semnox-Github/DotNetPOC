/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationViewContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LocationContainerDTOCollection locationDTOCollection;
        private readonly ConcurrentDictionary<int, LocationContainerDTO> locationDictionary;
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal LocationViewContainer(int siteId, LocationContainerDTOCollection locationDTOCollection)
        {
            log.LogMethodEntry(siteId, locationDTOCollection);
            this.siteId = siteId;
            this.locationDTOCollection = locationDTOCollection;
            this.locationDictionary = new ConcurrentDictionary<int, LocationContainerDTO>();
            //lastRefreshTime = DateTime.Now;
            if (locationDTOCollection != null &&
               locationDTOCollection.LocationContainerDTOList != null &&
               locationDTOCollection.LocationContainerDTOList.Any())
            {
                foreach (var locationContainerDTO in locationDTOCollection.LocationContainerDTOList)
                {
                    locationDictionary[locationContainerDTO.LocationId] = locationContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal LocationViewContainer(int siteId) :
            this(siteId, GetLocationContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static LocationContainerDTOCollection GetLocationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            LocationContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ILocationUseCases locationUseCases = InventoryUseCaseFactory.GetLocationUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<LocationContainerDTOCollection> locationViewDTOCollectionTask = locationUseCases.GetLocationContainerDTOCollection(siteId, hash, rebuildCache);
                    locationViewDTOCollectionTask.Wait();
                    result = locationViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving LocationContainerDTOCollection.", ex);
                result = new LocationContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in LocationDTOCollection
        /// </summary>
        /// <returns></returns>
        internal LocationContainerDTOCollection GetLocationDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (locationDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(locationDTOCollection);
            return locationDTOCollection;
        }

        internal LocationViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            LocationContainerDTOCollection latestLocationDTOCollection = GetLocationContainerDTOCollection(siteId, locationDTOCollection.Hash, true);
            if (latestLocationDTOCollection == null ||
                latestLocationDTOCollection.LocationContainerDTOList == null ||
                latestLocationDTOCollection.LocationContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            LocationViewContainer result = new LocationViewContainer(siteId, latestLocationDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

    }
}

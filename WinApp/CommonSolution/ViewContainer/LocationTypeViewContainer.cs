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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.Location;

namespace Semnox.Parafait.ViewContainer
{
    public class LocationTypeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LocationTypeContainerDTOCollection locationTypeDTOCollection;
        private readonly ConcurrentDictionary<int, LocationTypeContainerDTO> locationTypeDictionary;
        private readonly int siteId;

        internal LocationTypeViewContainer(int siteId, LocationTypeContainerDTOCollection locationTypeDTOCollection)
        {
            log.LogMethodEntry(siteId, locationTypeDTOCollection);
            this.siteId = siteId;
            this.locationTypeDTOCollection = locationTypeDTOCollection;
            this.locationTypeDictionary = new ConcurrentDictionary<int, LocationTypeContainerDTO>();
            if (locationTypeDTOCollection != null &&
               locationTypeDTOCollection.LocationTypeContainerDTOList != null &&
               locationTypeDTOCollection.LocationTypeContainerDTOList.Any())
            {
                foreach (var locationTypeContainerDTO in locationTypeDTOCollection.LocationTypeContainerDTOList)
                {
                    locationTypeDictionary[locationTypeContainerDTO.LocationTypeId] = locationTypeContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal LocationTypeViewContainer(int siteId) :
            this(siteId, GetLocationTypeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        internal List<LocationTypeContainerDTO> GetLocationTypeContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(locationTypeDTOCollection.LocationTypeContainerDTOList);
            return locationTypeDTOCollection.LocationTypeContainerDTOList;
        }
        private static LocationTypeContainerDTOCollection GetLocationTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            LocationTypeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ILocationTypeUseCases locationTypeUseCases = InventoryUseCaseFactory.GetLocationTypeUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<LocationTypeContainerDTOCollection> locationTypeViewDTOCollectionTask = locationTypeUseCases.GetLocationTypeContainerDTOCollection(siteId, hash, rebuildCache);
                    locationTypeViewDTOCollectionTask.Wait();
                    result = locationTypeViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving LocationTypeContainerDTOCollection.", ex);
                result = new LocationTypeContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in LocationTypeDTOCollection
        /// </summary>
        /// <returns></returns>
        internal LocationTypeContainerDTOCollection GetLocationTypeDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (locationTypeDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(locationTypeDTOCollection);
            return locationTypeDTOCollection;
        }

        internal LocationTypeViewContainer Refresh()
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            LocationTypeContainerDTOCollection latestLocationTypeDTOCollection = GetLocationTypeContainerDTOCollection(siteId, locationTypeDTOCollection.Hash, true);
            if (latestLocationTypeDTOCollection == null ||
                latestLocationTypeDTOCollection.LocationTypeContainerDTOList == null ||
                latestLocationTypeDTOCollection.LocationTypeContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            LocationTypeViewContainer result = new LocationTypeViewContainer(siteId, latestLocationTypeDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}

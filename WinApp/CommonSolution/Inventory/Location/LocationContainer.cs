/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
2.110.0         09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<LocationDTO> locationDTOList;
        private readonly DateTime? locationLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, LocationDTO> locationDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal LocationContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            executionContext.SetSiteId(siteId);
            locationDTODictionary = new ConcurrentDictionary<int, LocationDTO>();
            locationDTOList = new List<LocationDTO>();
            LocationList locationListBL = new LocationList(executionContext);
            locationLastUpdateTime = locationListBL.GetLocationLastUpdateTime(siteId);

            List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
            searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, siteId.ToString()));
            locationDTOList = locationListBL.GetAllLocations(searchParameters, null, true);
            if (locationDTOList != null && locationDTOList.Any())
            {
                foreach (LocationDTO locationDTO in locationDTOList)
                {
                    locationDTODictionary[locationDTO.LocationId] = locationDTO;
                }
            }
            else
            {
                locationDTOList = new List<LocationDTO>();
                locationDTODictionary = new ConcurrentDictionary<int, LocationDTO>();
            }
            log.LogMethodExit();
        }


        public List<LocationContainerDTO> GetLocationContainerDTOList()
        {
            log.LogMethodEntry();
            List<LocationContainerDTO> locationViewDTOList = new List<LocationContainerDTO>();
            List<LocationTypeContainerDTO> locationTypeContainerDTOList = LocationTypeContainerList.GetLocationTypeContainerDTOList(siteId);
            foreach (LocationDTO locationDTO in locationDTOList)
            {
                string locationTypeName = locationTypeContainerDTOList.Where(x => x.LocationTypeId == locationDTO.LocationTypeId).FirstOrDefault().LocationType;
                LocationContainerDTO locationViewDTO = new LocationContainerDTO(locationDTO.LocationId,
                                                                                 locationDTO.Name,
                                                                                 locationDTO.IsAvailableToSell,
                                                                                 locationDTO.Barcode,
                                                                                 locationDTO.IsTurnInLocation,
                                                                                 locationDTO.IsStore,
                                                                                 locationDTO.MassUpdatedAllowed,
                                                                                 locationDTO.RemarksMandatory,
                                                                                 locationDTO.LocationTypeId,
                                                                                 locationDTO.CustomDataSetId,
                                                                                 locationDTO.ExternalSystemReference,
                                                                                 locationTypeName
                                                                               );
                locationViewDTOList.Add(locationViewDTO);
            }
            log.LogMethodExit(locationViewDTOList);
            return locationViewDTOList;
        }

        public LocationContainer Refresh()
        {
            log.LogMethodEntry();
            LocationList locationListBL = new LocationList(executionContext);
            DateTime? updateTime = locationListBL.GetLocationLastUpdateTime(siteId);
            if (locationLastUpdateTime.HasValue
                && locationLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Location since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            LocationContainer result = new LocationContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }


    }
}

/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationTypeContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
* 2.110.0     15-Jan-2021      Vikas Dwivedi           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationTypeContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<LocationTypeDTO> locationTypeDTOList;
        private readonly DateTime? locationLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, LocationTypeDTO> locationTypeDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal LocationTypeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            locationTypeDTODictionary = new ConcurrentDictionary<int, LocationTypeDTO>();
            locationTypeDTOList = new List<LocationTypeDTO>();
            LocationTypeList locationTypeListBL = new LocationTypeList(executionContext);
            locationLastUpdateTime = locationTypeListBL.GetLocationTypeLastUpdateTime(siteId);

            List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> searchParameters = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
            searchParameters.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, siteId.ToString()));
            locationTypeDTOList = locationTypeListBL.GetAllLocationType(searchParameters);
            if (locationTypeDTOList != null && locationTypeDTOList.Any())
            {
                foreach (LocationTypeDTO locationTypeDTO in locationTypeDTOList)
                {
                    locationTypeDTODictionary[locationTypeDTO.LocationTypeId] = locationTypeDTO;
                }
            }
            else
            {
                locationTypeDTOList = new List<LocationTypeDTO>();
                locationTypeDTODictionary = new ConcurrentDictionary<int, LocationTypeDTO>();
            }
            log.LogMethodExit();
        }


        public List<LocationTypeContainerDTO> GetLocationTypeContainerDTOList()
        {
            log.LogMethodEntry();
            List<LocationTypeContainerDTO> locationTypeViewDTOList = new List<LocationTypeContainerDTO>();
            foreach (LocationTypeDTO locationTypeDTO in locationTypeDTOList)
            {
                LocationTypeContainerDTO locationTypeViewDTO = new LocationTypeContainerDTO(locationTypeDTO.LocationTypeId, locationTypeDTO.LocationType, locationTypeDTO.Description);
                locationTypeViewDTOList.Add(locationTypeViewDTO);
            }
            log.LogMethodExit(locationTypeViewDTOList);
            return locationTypeViewDTOList;
        }

        public LocationTypeContainer Refresh()
        {
            log.LogMethodEntry();
            LocationTypeList locationTypeListBL = new LocationTypeList(executionContext);
            DateTime? updateTime = locationTypeListBL.GetLocationTypeLastUpdateTime(siteId);
            if (locationLastUpdateTime.HasValue
                && locationLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in LocationType since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            LocationTypeContainer result = new LocationTypeContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of Physical Count  Excel DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    11-Jan-2021   Mushahid Faizan Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class InventoryPhysicalCountExcelDTODefinition : ComplexAttributeDefinition
    {
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public InventoryPhysicalCountExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(InventoryPhysicalCountDTO))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("PhysicalCountID", "PhysicalCountID", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Name", "Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Status", "Status", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LocationId", "Location", new LocationValueConverter(executionContext)));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("InitiatedBy", "InitiatedBy", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ClosedBy", "ClosedBy", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("StartDate", "StartDate", new NullableDateTimeValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("EndDate", "EndDate", new NullableDateTimeValueConverter()));


        }
    }


    class LocationValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<KeyValuePair<int, LocationDTO>> locationIdDTOKeyValuePair;
        List<KeyValuePair<string, LocationDTO>> locationNameDTOKeyValuePair;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LocationValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            locationIdDTOKeyValuePair = new List<KeyValuePair<int, LocationDTO>>();
            locationNameDTOKeyValuePair = new List<KeyValuePair<string, LocationDTO>>();
            List<LocationDTO> locationList = new List<LocationDTO>();

            LocationList locationDTOList = new LocationList(executionContext);
            List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParams = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
            searchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            locationList = locationDTOList.GetAllLocations(searchParams);

            if (locationList != null && locationList.Count > 0)
            {
                foreach (LocationDTO locationDTO in locationList)
                {
                    locationIdDTOKeyValuePair.Add(new KeyValuePair<int, LocationDTO>(locationDTO.LocationId, locationDTO));
                    locationNameDTOKeyValuePair.Add(new KeyValuePair<string, LocationDTO>(locationDTO.Name, locationDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts locationname to locationid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int locationId = -1;

            for (int i = 0; i < locationNameDTOKeyValuePair.Count; i++)
            {
                if (locationNameDTOKeyValuePair[i].Key == stringValue.ToUpper())
                {
                    locationNameDTOKeyValuePair[i] = new KeyValuePair<string, LocationDTO>(locationNameDTOKeyValuePair[i].Key, locationNameDTOKeyValuePair[i].Value);
                    locationId = locationNameDTOKeyValuePair[i].Value.LocationId;
                }
            }

            log.LogMethodExit(locationId);
            return locationId;
        }
        /// <summary>
        /// Converts locationid to locationname
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string locationName = string.Empty;

            for (int i = 0; i < locationIdDTOKeyValuePair.Count; i++)
            {
                if (locationIdDTOKeyValuePair[i].Key == Convert.ToInt32(value))
                {
                    locationIdDTOKeyValuePair[i] = new KeyValuePair<int, LocationDTO>(locationIdDTOKeyValuePair[i].Key, locationIdDTOKeyValuePair[i].Value);

                    locationName = locationIdDTOKeyValuePair[i].Value.Name;
                }
            }

            log.LogMethodExit(locationName);
            return locationName;
        }
    }
}

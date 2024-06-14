/********************************************************************************************
 * Project Name - Location
 * Description  - LocationExcelDTODefinition holds the excel information for location
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *2.110.0        28-Sept-2020           Mushahid Faizan          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class LocationExcelDTODefinition : ComplexAttributeDefinition
    {

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public LocationExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(LocationDTO))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LocationId", "LocationId", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Name", "Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("RemarksMandatory", "RemarksMandatory", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsAvailableToSell", "Available To Sell", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsStore", "Is Store", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsTurnInLocation", "Turn In Location", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("MassUpdatedAllowed", "Allow Mass Update", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LocationTypeId", "Location Type", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Barcode", "Barcode", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("CustomDataSetId", "Custom", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ExternalSystemReference", "External System Reference", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastModUserId", "LastModUserId", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastModDttm", "LastModDttm", new NullableDateTimeValueConverter()));

        }
    }

    class LocationTypeValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<KeyValuePair<int, LocationTypeDTO>> LocationTypeIdLocationTypeDTOKeyValuePair;
        List<KeyValuePair<string, LocationTypeDTO>> locationTypeLocationTypeDTOKeyValuePair;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LocationTypeValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            locationTypeLocationTypeDTOKeyValuePair = new List<KeyValuePair<string, LocationTypeDTO>>();
            LocationTypeIdLocationTypeDTOKeyValuePair = new List<KeyValuePair<int, LocationTypeDTO>>();
            List<LocationTypeDTO> LocationTypeList = new List<LocationTypeDTO>();

            LocationTypeList LocationTypeDTOList = new LocationTypeList(executionContext);
            List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> searchParams = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
            searchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            LocationTypeList = LocationTypeDTOList.GetAllLocationType(searchParams);
            if (LocationTypeList != null && LocationTypeList.Count > 0)
            {
                foreach (LocationTypeDTO locationTypeDTO in LocationTypeList)
                {
                    LocationTypeIdLocationTypeDTOKeyValuePair.Add(new KeyValuePair<int, LocationTypeDTO>(locationTypeDTO.LocationTypeId, locationTypeDTO));
                    locationTypeLocationTypeDTOKeyValuePair.Add(new KeyValuePair<string, LocationTypeDTO>(locationTypeDTO.LocationType, locationTypeDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts locationType to LocationTypeid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int LocationTypeId = -1;
            for (int i = 0; i < locationTypeLocationTypeDTOKeyValuePair.Count; i++)
            {
                if (locationTypeLocationTypeDTOKeyValuePair[i].Key == stringValue)
                {
                    locationTypeLocationTypeDTOKeyValuePair[i] = new KeyValuePair<string, LocationTypeDTO>(locationTypeLocationTypeDTOKeyValuePair[i].Key, locationTypeLocationTypeDTOKeyValuePair[i].Value);
                    LocationTypeId = locationTypeLocationTypeDTOKeyValuePair[i].Value.LocationTypeId;
                }
            }

            log.LogMethodExit(LocationTypeId);
            return LocationTypeId;
        }
        /// <summary>
        /// Converts LocationTypeid to locationType
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string locationType = string.Empty;

            for (int i = 0; i < LocationTypeIdLocationTypeDTOKeyValuePair.Count; i++)
            {
                if (LocationTypeIdLocationTypeDTOKeyValuePair[i].Key == Convert.ToInt32(value))
                {
                    LocationTypeIdLocationTypeDTOKeyValuePair[i] = new KeyValuePair<int, LocationTypeDTO>(LocationTypeIdLocationTypeDTOKeyValuePair[i].Key, LocationTypeIdLocationTypeDTOKeyValuePair[i].Value);

                    locationType = LocationTypeIdLocationTypeDTOKeyValuePair[i].Value.LocationType;
                }
            }
            log.LogMethodExit(locationType);
            return locationType;
        }
    }
}

/********************************************************************************************************
 * Project Name - Location
 * Description  - Bussiness logic of Location
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************************
*2.60.2      11-Jun-2019      Nagesh Badiger      Added parametrized constructor and SaveUpdateLocationList() in LocationList
*2.70        15-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
*2.70.2      17-Oct-2019      Dakshakh raj        Modified : Issue fix for Loading location types
*2.70.2      26-Dec-2019      Girish Kundar       Modified : Added method GetInvenotryWastageLocationDTO() as part of Wastage Management. 
*2.110.0     07-Oct-2020      Mushahid Faizan     Modified : Inventory UI redesign changes
 *********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// LocationBL
    /// </summary>
    public class LocationBL
    {
        private LocationDTO locations;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private LocationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="locationDTO"></param>
        public LocationBL(ExecutionContext executionContext, LocationDTO locationDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, locationDTO);
            this.locations = locationDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the location id as the parameter
        /// Would fetch the location object from the database based on the id passed. 
        /// </summary>
        /// <param name="locationId">Location id</param>
        /// <param name="sqltransaction">SQL Transaction</param>
        public LocationBL(ExecutionContext executionContext, int locationId, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(locationId, sqltransaction);
            LocationDataHandler locationTypeDataHandler = new LocationDataHandler(sqltransaction);
            locations = locationTypeDataHandler.GetLocation(locationId);
            log.LogMethodExit(locations);
        }

        /// <summary>
        /// Saves the Locations 
        /// Checks if the Location id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>id</returns>
        public int Save(SqlTransaction sqlTransaction = null)
        {
            int locationId = -1;
            LocationDataHandler requisitionsDataHandler = new LocationDataHandler(sqlTransaction);
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);

            Validate(sqlTransaction);
            if (locations.LocationId < 0)
            {
                locations = requisitionsDataHandler.InsertLocations(locations, executionContext.GetUserId(), executionContext.GetSiteId());
                locations.AcceptChanges();
            }
            else
            {
                if (locations.IsChanged)
                {
                    locations = requisitionsDataHandler.UpdateLocations(locations, executionContext.GetUserId(), executionContext.GetSiteId());
                    locationId = locations.LocationId;
                    locations.AcceptChanges();
                }
            }
            if (!string.IsNullOrEmpty(locations.Guid))
            {
                InventoryActivityLogDTO InventoryActivityLogDTO = new InventoryActivityLogDTO(serverTimeObject.GetServerDateTime(), "Location Inserted",
                                                         locations.Guid, false, executionContext.GetSiteId(), "Location", -1, locations.LocationId + ":" + locations.Name, -1, executionContext.GetUserId(),
                                                         serverTimeObject.GetServerDateTime(), executionContext.GetUserId(), serverTimeObject.GetServerDateTime());


                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, InventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            return locationId;
        }

        /// <summary>
        /// Validates the LocationDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            int isStoreLocationID = -1;
            string locationType = string.Empty;
            LocationList locationList = new LocationList(executionContext);
            InventoryList inventoryList = new InventoryList();
            if (locations != null)
            {
                if (string.IsNullOrEmpty(locations.Name) || string.IsNullOrWhiteSpace(locations.Name))
                {
                    log.Error("Enter Location ");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2607, MessageContainerList.GetMessage(executionContext, "Location"));
                    throw new ValidationException(errorMessage);
                }
                if (locations.LocationTypeId == -1)
                {
                    log.Error("Please select Location type.");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, "Please select Location type.");
                    throw new ValidationException(errorMessage);
                }
                LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
                List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (locations.IsStore == "Y")
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.ISSTORE, "Y"));
                }
                searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "Y"));
                List<LocationDTO> locationDTOList = locationDataHandler.GetLocationList(searchParameters);
                if (locationDTOList != null && locationDTOList.Count > 0)
                    if (locations.IsStore == "Y")
                    {
                        isStoreLocationID = locationDTOList[0].LocationId;
                    }
                if (locationDTOList != null && locationDTOList.Any())
                {
                    if(locationDTOList.Exists(x => x.Name.ToLower() == locations.Name.ToLower() && x.LocationTypeId == locations.LocationTypeId) && locations.LocationId == -1)
                    {
                        log.Error("Duplicate entries detail ");
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Location"));
                        throw new ValidationException(errorMessage);
                    }
                    if (locationDTOList.Exists(x => x.Name.ToLower() == locations.Name.ToLower() && x.LocationTypeId == locations.LocationTypeId && x.LocationId != locations.LocationId))
                    {
                        log.Error("Duplicate entries detail ");
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Location"));
                        throw new ValidationException(errorMessage);
                    }
                }
                LocationType LocationType = new LocationType(executionContext, locations.LocationTypeId);
                LocationTypeDTO locationTypeDTO = LocationType.LocationTypeDTO;
                if (locationTypeDTO != null)
                {
                    locationType = locationTypeDTO.LocationType;
                }
                if (locations.LocationId < 0 && (locationType == "Receive" || locationType == "Purchase" || locationType == "Adjustment"))
                {
                    log.Error("Please select Location type.");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, "Location of type " + locationType + " already exists.");
                    throw new ValidationException(errorMessage);
                }
                if (locations.LocationId > -1 && locations.IsActive == false)
                {
                    List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, locations.LocationId.ToString()));
                    List<InventoryDTO> inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams);
                    if (inventoryDTOList != null && inventoryDTOList.Exists(x => (x.Quantity > 0)))
                    {
                        log.Error("Location has stock. Please clear it before delete. ");
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1536, MessageContainerList.GetMessage(executionContext, "LocationId"));
                        throw new ValidationException(errorMessage);
                    }
                }
                if (isStoreLocationID != -1 && locations.LocationId != isStoreLocationID && locations.IsStore == "Y")
                {
                    log.Error("Store already exists. There cannot be more than one store in a site.");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, "Store already exists. There cannot be more than one store in a site.");
                    throw new ValidationException(errorMessage);
                }
                //To ensures that No one should add a location called 'Wastage' of type Wastage.
                LocationDTO wastageLocationDTO = locationList.GetWastageLocationDTO();
                if (locations.LocationId < 0 && locations.Name.Equals(wastageLocationDTO.Name, StringComparison.CurrentCultureIgnoreCase)
                    && locationType == "Wastage")
                {
                    log.Error("Location  Wastage is already exists ");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2454, MessageContainerList.GetMessage(executionContext, "LocationName"));
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LocationDTO GetLocationDTO { get { return locations; } }
    }

    /// <summary>
    /// Manages the list of location
    /// </summary>
    public class LocationList
    {
        ExecutionContext executionUserContext = ExecutionContext.GetExecutionContext();
        logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LocationDTO> locationDTOList = new List<LocationDTO>();
        private ExecutionContext executionContext;
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LocationList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="locationDTOList">locationDTOList</param>
        public LocationList(ExecutionContext executionContext, List<LocationDTO> locationDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, locationDTOList);
            this.locationDTOList = locationDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the no of location matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetLocationCount(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
            int locationCount = locationDataHandler.GetLocationCount(searchParameters);
            log.LogMethodExit(locationCount);
            return locationCount;
        }

        /// <summary>
        /// Returns the location list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>location DTO List</returns>
        public List<LocationDTO> GetAllLocations(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>
                                  searchParameters, SqlTransaction sqlTransaction = null,
                                  bool includeWastageLocation = false, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, includeWastageLocation);
            LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
            List<LocationDTO> locationDTOList = locationDataHandler.GetLocationList(searchParameters, currentPage, pageSize);
            if (includeWastageLocation == false && locationDTOList != null && locationDTOList.Any())
            {
                List<LocationDTO> WastageLocationDTOList = GetWastageLocationDTOList();
                foreach(LocationDTO wastageLocationDTO in WastageLocationDTOList)
                {
                    int index = locationDTOList.FindIndex(x => x.LocationId == wastageLocationDTO.LocationId);
                    if (index > -1)
                    {
                        locationDTOList.RemoveAt(index);
                    }
                }
            }
            log.LogMethodExit(locationDTOList);
            return locationDTOList;
        }

        /// <summary>
        ///  Location type can be Comma separated values like LocationTypeName1,LocationTypeName2,...
        /// </summary>
        /// <param name="locationType">locationType</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the location list</returns>
        public List<LocationDTO> GetAllLocations(string locationType, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationType, sqlTransaction);
            string typeId = "";
            int counter = 0;
            LocationTypeList locationTypeList = new LocationTypeList(executionContext);
            List<LocationTypeDTO> locationTypeDTOList;
            List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> locationTypeSearchParams = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
            locationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE, "1"));
            locationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, executionUserContext.GetSiteId().ToString()));
            locationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE, locationType.ToString()));
            locationTypeDTOList = locationTypeList.GetAllLocationType(locationTypeSearchParams);

            if (locationTypeDTOList != null && locationTypeDTOList.Count > 0)
            {
                foreach (LocationTypeDTO locationTypeDTO in locationTypeDTOList)
                {
                    typeId += ((counter == 0) ? "" : ",") + locationTypeDTO.LocationTypeId;
                    counter++;
                }
                LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
                List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> locationSearchParams = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "1"));
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionUserContext.GetSiteId().ToString()));
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.LOCATION_TYPE_ID, typeId));
                locationDTOList = locationDataHandler.GetLocationList(locationSearchParams);
                log.LogMethodExit(locationDTOList);
                return locationDTOList;
            }
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// Returns the location DTO
        /// </summary>
        /// <param name="locationId">locationId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>location DTO</returns>
        public LocationDTO GetLocation(int locationId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, sqlTransaction);
            LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
            LocationDTO locationDTO = locationDataHandler.GetLocation(locationId);
            log.LogMethodExit(locationDTO);
            return locationDTO;
        }

        /// <summary>
        /// GetLocationsOnLocationType
        /// </summary>
        /// <param name="locationTypes">locationTypes</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> Returns the List</returns>
        public List<LocationDTO> GetLocationsOnLocationType(string locationTypes, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationTypes);
            LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
            List<LocationDTO> locationDTOList = locationDataHandler.GetLocationsListOnType(locationTypes);
            log.LogMethodExit(locationDTOList);
            return locationDTOList;
        }

        /// <summary>
        /// GetLocationsOnLocationType
        /// </summary>
        /// <param name="locationTypes">locationTypes</param>
        /// <param name="SiteID">SiteID</param>
        /// <returns> Returns the List</returns>
        public List<LocationDTO> GetLocationsOnLocationType(string locationTypes, int SiteID)
        {
            log.LogMethodEntry(locationTypes, SiteID);
            log.LogMethodExit();
            return GetLocationsOnLocationType(locationTypes, SiteID, false);
        }

        /// <summary>
        /// GetLocationsOnLocationType
        /// </summary>
        /// <param name="locationTypes"></param>
        /// <param name="SiteID"></param>
        /// <param name="isPublishedLocation"></param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> Returns the List</returns>
        public List<LocationDTO> GetLocationsOnLocationType(string locationTypes, int SiteID, bool isPublishedLocation, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationTypes, SiteID, isPublishedLocation, sqlTransaction);
            LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
            List<LocationDTO> locationDTOList = locationDataHandler.GetLocationsListOnType(locationTypes, SiteID, isPublishedLocation);
            log.LogMethodExit(locationDTOList);
            return locationDTOList;
        }

        /// <summary>
        /// GetLocationListToOpenPhysicalCount
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>location DTO List</returns>
        public List<LocationDTO> GetLocationListToOpenPhysicalCount(int SiteID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID);
            LocationDataHandler locationDataHandler = new LocationDataHandler();
            List<LocationDTO> locationDTOList = locationDataHandler.GetLocationListToOpenPhysicalCount(SiteID);
            log.LogMethodExit(locationDTOList);
            return locationDTOList;
        }

        /// <summary>
        /// Location type can be Comma separated values like LocationTypeName1,LocationTypeName2,...
        /// </summary>
        /// <param name="locationType">locationType</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the location list</returns>
        public List<LocationDTO> GetAllLocationsForPhysicalCount(string locationType, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationType, sqlTransaction);
            string typeId = "";
            int counter = 0;
            LocationTypeList locationTypeList = new LocationTypeList(executionContext);
            List<LocationTypeDTO> locationTypeDTOList;
            List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> locationTypeSearchParams = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
            locationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE, "1"));
            locationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, executionUserContext.GetSiteId().ToString()));
            locationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE, locationType.ToString()));
            locationTypeDTOList = locationTypeList.GetAllLocationType(locationTypeSearchParams);

            if (locationTypeDTOList != null && locationTypeDTOList.Count > 0)
            {
                foreach (LocationTypeDTO locationTypeDTO in locationTypeDTOList)
                {
                    typeId += ((counter == 0) ? "" : ",") + locationTypeDTO.LocationTypeId;
                    counter++;
                }
                LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
                List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> locationSearchParams = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "1"));
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionUserContext.GetSiteId().ToString()));
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.LOCATION_TYPE_ID, typeId));
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.MASSUPDATEALLOWED, "Y"));
                List<LocationDTO> locationDTOlist = locationDataHandler.GetLocationList(locationSearchParams);
                log.LogMethodExit(locationDTOlist);
                return locationDTOlist;
            }
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// This method is will return Sheet object for Location.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
            List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
            searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            locationDTOList = locationDataHandler.GetLocationList(searchParameters);

            LocationExcelDTODefinition locationExcelDTODefinition = new LocationExcelDTODefinition(executionContext, "");
            ///Building headers from LocationExcelDTODefinition
            locationExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (locationDTOList != null && locationDTOList.Any())
            {
                foreach (LocationDTO locationDTO in locationDTOList)
                {
                    locationExcelDTODefinition.Configure(locationDTO);

                    Row row = new Row();
                    locationExcelDTODefinition.Serialize(row, locationDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }


        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            LocationExcelDTODefinition locationExcelDTODefinition = new LocationExcelDTODefinition(executionContext, "");
            List<LocationDTO> rowLocationDTOList = new List<LocationDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    LocationDTO rowLocationDTO = (LocationDTO)locationExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowLocationDTOList.Add(rowLocationDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowLocationDTOList != null && rowLocationDTOList.Any())
                    {
                        LocationList locationListBL = new LocationList(executionContext, rowLocationDTOList);
                        locationListBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;
        }


        /// <summary>
        /// Saves Location List
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            if (locationDTOList == null ||
               locationDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < locationDTOList.Count; i++)
            {
                var locationDTO = locationDTOList[i];
                if (locationDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    LocationBL locationBL = new LocationBL(executionContext, locationDTO);
                    locationBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving locationDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("locationDTO", locationDTO);
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns WastageLocation DTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>LocationDTO</returns>
        public LocationDTO GetWastageLocationDTO(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LocationDTO wastageLocationDTO = new LocationDTO();
            LocationDataHandler locationDataHandler = new LocationDataHandler();
            List<LocationDTO> wastageLocationDTOList = locationDataHandler.GetLocationsListOnType("'Wastage'", executionContext.GetSiteId(), false);
            if (wastageLocationDTOList == null || wastageLocationDTOList.Any() == false)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 2455));
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2455));
            }
            wastageLocationDTO = wastageLocationDTOList[0];
            log.LogMethodExit(wastageLocationDTO);
            return wastageLocationDTO;
        }

        /// <summary>
        /// Returns WastageLocation DTO List
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>LocationDTO</returns>
        public List<LocationDTO> GetWastageLocationDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LocationDataHandler locationDataHandler = new LocationDataHandler();
            List<LocationDTO> wastageLocationDTOList = locationDataHandler.GetLocationsListOnType("'Wastage'", executionContext.GetSiteId(), false);
            if (wastageLocationDTOList == null || wastageLocationDTOList.Any() == false)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 2455));
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2455));
            }
            log.LogMethodExit(wastageLocationDTOList);
            return wastageLocationDTOList;
        }

        public DateTime? GetLocationLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            LocationDataHandler locationDataHandler = new LocationDataHandler(sqlTransaction);
            DateTime? result = locationDataHandler.GetLocationLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

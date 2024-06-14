/********************************************************************************************
 * Project Name - Location Handler
 * Description  - Data handler of the Location class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        15-Jul-2019   Dakshakh raj     Modified : added GetSQLParameters(),
 *                                                      SQL injection Issue Fix
 *2.70.2      09-Dec-2019   Jinto Thomas     Removed siteid from update query                                                       
 *2.70.2      29-Dec-2019   Girish Kundar    Modified : GetLocationList() method added location name as separate search parameter   
 *2.110.0         07-Oct-2020   Mushahid Faizan        Modified : Inventory UI redesign changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// LocationDataHandler
    /// </summary>
    public class LocationDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Location AS loc";
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Dictionary for searching Parameters for the Location object.
        /// </summary>
        private static readonly Dictionary<LocationDTO.SearchByLocationParameters, string> DBSearchParameters = new Dictionary<LocationDTO.SearchByLocationParameters, string>
               {
                    {LocationDTO.SearchByLocationParameters.LOCATION_ID, "loc.LocationId"},
                    {LocationDTO.SearchByLocationParameters.LOCATION_ID_LIST, "loc.LocationId"},
                    {LocationDTO.SearchByLocationParameters.IS_ACTIVE, "loc.IsActive"},
                    {LocationDTO.SearchByLocationParameters.SITE_ID,"loc.site_id"},
                    {LocationDTO.SearchByLocationParameters.LOCATION_TYPE_ID,"loc.LocationTypeId"},
                    {LocationDTO.SearchByLocationParameters.BARCODE,"loc.barCode"},
                    {LocationDTO.SearchByLocationParameters.LOCATION_NAME,"loc.Name"},
                    {LocationDTO.SearchByLocationParameters.LOCATION_NAME_EXACT,"loc.Name"},
                    {LocationDTO.SearchByLocationParameters.ISSTORE,"loc.isStore"},
                    {LocationDTO.SearchByLocationParameters.MASSUPDATEALLOWED,"loc.MassUpdateAllowed"},
                    {LocationDTO.SearchByLocationParameters.ISREMARKSMANDATORY,"loc.RemarksMandatory"},
                    {LocationDTO.SearchByLocationParameters.CUSTOMDATASETID,"loc.CustomDataSetId"},
                    {LocationDTO.SearchByLocationParameters.MASTER_ENTITY_ID,"loc.MasterEntityId"}
               };

        /// <summary>
        /// Default constructor of LocationDataHandler class
        /// </summary>
        public LocationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Location parameters Record.
        /// </summary>
        /// <param name="locationDTO">locationDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(LocationDTO locationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(locationDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@locationId", locationDTO.LocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(locationDTO.Name) ? DBNull.Value : (object)locationDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastModUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", locationDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isAvailableToSell", locationDTO.IsAvailableToSell));
            parameters.Add(dataAccessHandler.GetSQLParameter("@barcode", string.IsNullOrEmpty(locationDTO.Barcode) ? DBNull.Value : (object)locationDTO.Barcode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isTurnInLocation", locationDTO.IsTurnInLocation));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isStore", locationDTO.IsStore));
            parameters.Add(dataAccessHandler.GetSQLParameter("@massUpdatedAllowed", locationDTO.MassUpdatedAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarksMandatory", locationDTO.RemarksMandatory));
            parameters.Add(dataAccessHandler.GetSQLParameter("@locationTypeId", locationDTO.LocationTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customDataSetId", locationDTO.CustomDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@externalSystemReference", locationDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", locationDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Converts the Data row object to LocationDTO class type
        /// </summary>
        /// <param name="locationDataRow">LocationDTO DataRow</param>
        /// <returns>Returns LocationDTO</returns>
        private LocationDTO GetLocationDTO(DataRow locationDataRow)
        {
            log.LogMethodEntry(locationDataRow);
            LocationDTO locationDataObject = new LocationDTO(Convert.ToInt32(locationDataRow["LocationId"]),
                                                          locationDataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(locationDataRow["Name"]),
                                                          locationDataRow["LastModUserId"] == DBNull.Value ? string.Empty : Convert.ToString(locationDataRow["LastModUserId"]),
                                                          locationDataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(locationDataRow["LastModDttm"]),
                                                          locationDataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(locationDataRow["IsActive"]) == "Y",
                                                          locationDataRow["IsAvailableToSell"] == DBNull.Value ? "N" : Convert.ToString(locationDataRow["IsAvailableToSell"]),
                                                          locationDataRow["barCode"].ToString(),
                                                          locationDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(locationDataRow["site_id"]),
                                                          locationDataRow["Guid"].ToString(),
                                                          locationDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(locationDataRow["SynchStatus"]),
                                                          locationDataRow["IsTurnInLocation"] == DBNull.Value ? "N" : Convert.ToString(locationDataRow["IsTurnInLocation"]),
                                                          locationDataRow["IsStore"] == DBNull.Value ? "N" : Convert.ToString(locationDataRow["IsStore"]),
                                                          locationDataRow["MassUpdateAllowed"] == DBNull.Value ? "N" : Convert.ToString(locationDataRow["MassUpdateAllowed"]),
                                                          locationDataRow["RemarksMandatory"] == DBNull.Value ? "N" : Convert.ToString(locationDataRow["RemarksMandatory"]),
                                                          locationDataRow["LocationTypeID"] == DBNull.Value ? -1 : Convert.ToInt32(locationDataRow["LocationTypeID"]),
                                                          locationDataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(locationDataRow["CustomDataSetId"]),
                                                          locationDataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(locationDataRow["ExternalSystemReference"]),
                                                          locationDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(locationDataRow["MasterEntityId"]),
                                                          locationDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(locationDataRow["CreatedBy"]),
                                                          locationDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(locationDataRow["CreationDate"])
                                                         );
            log.LogMethodEntry(locationDataObject);
            return locationDataObject;
        }

        /// <summary>
        /// Updates the locations header 
        /// </summary>
        /// <param name="locationDTO">LocationDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public LocationDTO UpdateLocations(LocationDTO locationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(locationDTO, loginId, siteId);
            string updateLocationQuery = @"update Location SET
		                                                LocationTypeID= @locationTypeId,             
                                                        Name= @name,                                                 
                                                        IsAvailableToSell=@isAvailableToSell,
                                                        barCode=@barcode,
                                                        IsTurnInLocation=@isTurnInLocation,
                                                        IsStore=@isStore,
                                                        MassUpdateAllowed=@massUpdatedAllowed,
                                                        RemarksMandatory=@remarksMandatory, 
                                                        LastModUserId= @lastModUserId,
                                                        LastModDttm=Getdate(),
                                                        --site_id=@siteid,
                                                        IsActive=@isActive,
                                                        CustomDataSetId = @CustomDataSetId,
                                                        ExternalSystemReference = @ExternalSystemReference,
                                                        MasterEntityId = @masterEntityId
		                                                where LocationId = @locationId 
                                                        SELECT* FROM Location WHERE  LocationId = @locationId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateLocationQuery, GetSQLParameters(locationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLocationDTO(locationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LocationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(locationDTO);
            return locationDTO;
        }

        /// <summary>
        /// Inserts the Locations record to the database
        /// </summary>
        /// <param name="locationDTO">LocationDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns LocationDTO</returns>
        public LocationDTO InsertLocations(LocationDTO locationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(locationDTO, loginId, siteId);
            string insertLocationQuery = @"INSERT INTO[dbo].[Location]  
                                                        (   
                                                        LocationTypeID,             
                                                        Name,                                                 
                                                        IsAvailableToSell,
                                                        barCode,
                                                        IsTurnInLocation,
                                                        IsStore,
                                                        MassUpdateAllowed,
                                                        RemarksMandatory, 
                                                        LastModUserId,
                                                        LastModDttm,
                                                        Guid,
                                                        site_id,
                                                        IsActive,
                                                        CustomDataSetId,
                                                        ExternalSystemReference,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate
                                                        ) 
                                                values 
                                                        (
                                                         @locationTypeId,
                                                         @name,
                                                         @isAvailableToSell,
                                                         @barcode,
                                                         @isTurnInLocation,   
                                                         @isStore,
                                                         @massUpdatedAllowed,
                                                         @remarksMandatory,                                                      
                                                         @lastModUserId,
                                                         Getdate(),                                                        
                                                         NEWID(),
                                                         @siteid,
                                                         @isActive,
                                                         --@machine_id
                                                         @CustomDataSetId,
                                                         @ExternalSystemReference,
                                                         @masterEntityId,
                                                         @createdBy,
                                                         Getdate() 
                                                        )SELECT * FROM Location WHERE LocationId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLocationQuery, GetSQLParameters(locationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLocationDTO(locationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LocationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(locationDTO);
            return locationDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="locationDTO">locationDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLocationDTO(LocationDTO locationDTO, DataTable dt)
        {
            log.LogMethodEntry(locationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                locationDTO.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
                locationDTO.LastModDttm = dataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastModDttm"]);
                locationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                locationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                locationDTO.LastModUserId = dataRow["LastModUserId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastModUserId"]);
                locationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                locationDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the location data of passed patch asset application id
        /// </summary>
        /// <param name="locationId">integer type parameter</param>
        /// <returns>Returns LocationDTO</returns>
        public LocationDTO GetLocation(int locationId)
        {
            log.LogMethodEntry(locationId);
            LocationDTO result = null;
            string selectLocationQuery = SELECT_QUERY + @" WHERE loc.LocationId = @locationId";
            SqlParameter parameter = new SqlParameter("@locationId", locationId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLocationQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetLocationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the no of Locations matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetLocationCount(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int categoryDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                categoryDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(categoryDTOCount);
            return categoryDTOCount;
        }

        /// <summary>
        /// Returns the List of LocationDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of LocationDTO</returns>
        public string GetFilterQuery(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<LocationDTO.SearchByLocationParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LocationDTO.SearchByLocationParameters.LOCATION_ID
                            || searchParameter.Key == LocationDTO.SearchByLocationParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == LocationDTO.SearchByLocationParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LocationDTO.SearchByLocationParameters.LOCATION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == LocationDTO.SearchByLocationParameters.LOCATION_TYPE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == LocationDTO.SearchByLocationParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == LocationDTO.SearchByLocationParameters.LOCATION_NAME_EXACT)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the LocationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LocationDTO matching the search criteria</returns>
        public List<LocationDTO> GetLocationList(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<LocationDTO> locationDTOList = new List<LocationDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY loc.LocationId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                locationDTOList = new List<LocationDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LocationDTO locationDTO = GetLocationDTO(dataRow);
                    locationDTOList.Add(locationDTO);
                }
            }
            log.LogMethodExit(locationDTOList);
            return locationDTOList;
        }


        /// <summary>
        /// Gets the LocationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LocationDTO matching the search criteria</returns>
        public List<LocationDTO> GetLocationList(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LocationDTO> locationDTOList = new List<LocationDTO>();

            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                locationDTOList = new List<LocationDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LocationDTO locationDTO = GetLocationDTO(dataRow);
                    locationDTOList.Add(locationDTO);
                }
            }
            log.LogMethodExit(locationDTOList);
            return locationDTOList;
        }


        /// <summary>
        ///  Gets the LocationDTO list matching the search key
        /// </summary>
        /// <param name="locationTypes"></param>
        /// <returns>location DTO List</returns>
        public List<LocationDTO> GetLocationsListOnType(string locationTypes)
        {
            log.LogMethodEntry(locationTypes);
            List<LocationDTO> locationDTOList = GetLocationsListOnType(locationTypes, -1, false);
            log.LogMethodExit(locationDTOList);
            return locationDTOList;
        }

        /// <summary>
        ///  Gets the LocationDTO list matching the search key
        /// </summary>
        /// <param name="locationTypes"></param>
        /// <param name="SiteID"></param>
        /// <param name="isPublishedLocation"></param>
        /// <returns>location list</returns>
        public List<LocationDTO> GetLocationsListOnType(string locationTypes, int SiteID, bool isPublishedLocation)
        {
            log.LogMethodEntry(locationTypes, SiteID, isPublishedLocation);
            List<LocationDTO> locationlist = null;
            string selectLocations = @"select * from Location l 
                                                    inner join LocationType lt on l.LocationTypeID = lt.LocationTypeId 
                                                    where (l.site_id = @site_id or @site_id = -1) " + ((isPublishedLocation) ? " and isnull(l.MasterEntityId,-1)> -1" : "") + "and lt.LocationType ";
            StringBuilder query = new StringBuilder();
            query.Append(" IN(" + locationTypes + ") ");
            //query.Append(" and isActive = 'Y' "); 
            selectLocations = selectLocations + query;
            SqlParameter[] selectReqParameters = new SqlParameter[1];
            selectReqParameters[0] = new SqlParameter("@site_id", SiteID);
            DataTable locationDT = dataAccessHandler.executeSelectQuery(selectLocations, selectReqParameters.ToArray(), sqlTransaction);
            if (locationDT.Rows.Count > 0)
            {
                locationlist = new List<LocationDTO>();
                foreach (DataRow locationDTRow in locationDT.Rows)
                {
                    LocationDTO locationDataObject = GetLocationDTO(locationDTRow);
                    locationlist.Add(locationDataObject);
                    log.LogMethodExit();
                }
            }
            else
            {
                log.LogMethodExit();

            }
            log.LogMethodExit(locationlist);
            return locationlist;
        }

        /// <summary>
        /// Gets the LocationDTO list matching the search key
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <returns>Returns the list of LocationDTO matching the search criteria</returns>
        public List<LocationDTO> GetLocationListToOpenPhysicalCount(int SiteID)
        {
            log.LogMethodEntry(SiteID);
            List<LocationDTO> locationlist = null;
            string selectLocations = @"select l.*
                                        from location l, locationType t 
                                        where not exists (select *
				                                          from invphysicalcount i
				                                          where status = 'Open' 
					                                        and l.LocationId = i.locationid
					                                        and MassUpdateAllowed = 'Y'
					                                        and (i.site_id = @SiteID or @SiteID = -1)
					                                        )
	                                        and not exists (select *
					                                        from invphysicalcount i
					                                        where status = 'Open' 
						                                        and i.LocationId is null
						                                        and (i.site_id = @SiteID or @SiteID = -1)
						                                        )
                                        and (l.site_id = @SiteID or @SiteID = -1)
										and l.LocationTypeID = t.LocationTypeId
										and t.LocationType != 'Department'
                                        and t.LocationType != 'Wastage'
                                        and MassUpdateAllowed = 'Y'
                                        and l.isactive = 'Y'";
            SqlParameter[] selectLocationParameters = new SqlParameter[1];
            selectLocationParameters[0] = new SqlParameter("@SiteID", SiteID);
            DataTable locationDT = dataAccessHandler.executeSelectQuery(selectLocations, selectLocationParameters, sqlTransaction);
            if (locationDT.Rows.Count > 0)
            {
                locationlist = new List<LocationDTO>();
                foreach (DataRow locationDTRow in locationDT.Rows)
                {
                    LocationDTO locationDataObject = GetLocationDTO(locationDTRow);
                    locationlist.Add(locationDataObject);
                }
            }

            log.LogMethodExit(locationlist);
            return locationlist;
        }

        /// <summary>
        /// locationDataRow
        /// </summary>
        public string locationDataRow { get; set; }

        internal DateTime? GetLocationLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastModDttm) LastModDttm 
                            FROM (
                            select max(LastModDttm) LastModDttm from location WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastModDttm"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastModDttm"]);
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}

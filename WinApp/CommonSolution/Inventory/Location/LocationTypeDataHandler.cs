/********************************************************************************************
* Project Name -Location Type DataHandler
* Description  -Data object of Location Type
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        18-Aug-2016   Amaresh          Created 
*2.70.0     02-Aug-2019    Jagan Mohana     Removed the GetLocationTypeListOnType() method and used GetAllLocationType() by passing searchParameters
*2.70        15-Jul-2019   Dakshakh raj     Modified : added GetSQLParameters() and SQL injection Issue Fix
*2.70.2        09-Dec-2019   Jinto Thomas     Removed siteid from update query 
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
    /// Location Type - Handles insert, update and select of Location Type objects
    /// </summary>
    public class LocationTypeDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LocationType AS lt";

        /// <summary>
        /// Dictionary for searching Parameters for the ReportParameters object.
        /// </summary>
        private static readonly Dictionary<LocationTypeDTO.SearchByLocationTypeParameters, string> DBSearchParameters = new Dictionary<LocationTypeDTO.SearchByLocationTypeParameters, string>
               {
                    {LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE_ID, "lt.LocationTypeId"},
                    {LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE,"lt.LocationType"},
                    {LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE, "lt.isActive"},
                    {LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, "lt.Site_id"},
                    {LocationTypeDTO.SearchByLocationTypeParameters.MASTER_ENTITY_ID, "lt.MasterEntityId"}
               };

        /// <summary>
        /// Default constructor of LocationTypeDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LocationTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LocationTypeDTO class type
        /// </summary>
        /// <param name="locationTypeDataRow">LocationTypeDTO DataRow</param>
        /// <returns>Returns LocationTypeDTO</returns>
        private LocationTypeDTO GetLocationTypeDTO(DataRow locationTypeDataRow)
        {
            log.LogMethodEntry(locationTypeDataRow);

            LocationTypeDTO locationTypeDataObject = new LocationTypeDTO(
                                             locationTypeDataRow["LocationTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(locationTypeDataRow["LocationTypeId"]),
                                             locationTypeDataRow["LocationType"].ToString(),
                                             locationTypeDataRow["Description"].ToString(),
                                             locationTypeDataRow["CreatedBy"].ToString(),
                                             locationTypeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(locationTypeDataRow["CreationDate"]),
                                             locationTypeDataRow["LastUpdatedBy"].ToString(),
                                             locationTypeDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(locationTypeDataRow["LastupdatedDate"]),
                                             locationTypeDataRow["isActive"] == DBNull.Value ? false : Convert.ToBoolean(locationTypeDataRow["isActive"]),
                                             locationTypeDataRow["Guid"].ToString(),
                                             locationTypeDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(locationTypeDataRow["MasterEntityId"]),
                                             locationTypeDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(locationTypeDataRow["Site_id"]),
                                             locationTypeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(locationTypeDataRow["SynchStatus"])
                                             );
            log.LogMethodExit(locationTypeDataObject);
            return locationTypeDataObject;
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating LocationType Record.
        /// </summary>
        /// <param name="locationTypeDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(LocationTypeDTO locationTypeDTO , string loginId, int siteId)
        {
            
            log.LogMethodEntry(locationTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@locationTypeId", locationTypeDTO.LocationTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@locationType", string.IsNullOrEmpty(locationTypeDTO.LocationType) ? DBNull.Value : (object)locationTypeDTO.LocationType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(locationTypeDTO.Description) ? DBNull.Value : (object)locationTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", locationTypeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", locationTypeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@synchStatus", locationTypeDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the LocationType record to the database
        /// </summary>
        /// <param name="locationTypeDTO">LocationTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>LocationType DTO</returns>
        public LocationTypeDTO InsertLocationType(LocationTypeDTO locationTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(locationTypeDTO, loginId, siteId);
            string insertLocationTypeQuery = @"INSERT INTO [dbo].[LocationType]   
                                                        (
                                                        LocationType,
                                                        Description,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate, 
                                                        isActive,                                    
                                                        Guid,
                                                        MasterEntityId,
                                                        Site_id
                                                        ) 
                                                values 
                                                        ( 
                                                         @locationType,
                                                         @description,
                                                         @createdBy,
                                                         Getdate(),
                                                         @lastUpdatedBy,
                                                         Getdate(),
                                                         @isActive,
                                                         NEWID(),
                                                         @masterEntityId,
                                                         @siteId
                                                         )SELECT * FROM LocationType WHERE LocationTypeId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLocationTypeQuery, GetSQLParameters(locationTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLocationTypeDTO(locationTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LocationTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(locationTypeDTO);
            return locationTypeDTO;
        }

        /// <summary>
        /// Updates the LocationType record
        /// </summary>
        /// <param name="LocationTypeDTO">LocationTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>LocationType DTO</returns>
        public LocationTypeDTO UpdateLocationType(LocationTypeDTO locationTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(locationTypeDTO, loginId, siteId);
            string updateLocationTypeQuery = @"update LocationType 
                                             set LocationType=@locationType,
                                             Description=@description,
                                             CreatedBy =@createdBy,
                                             CreationDate=Getdate(),
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastupdatedDate = Getdate(),
                                             isActive =@isActive,
                                             MasterEntityId=@masterEntityId         
                                             --Site_id=@siteId
                                       WHERE LocationTypeId = @locationTypeId SELECT* FROM LocationType WHERE LocationTypeId = @locationTypeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateLocationTypeQuery, GetSQLParameters(locationTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLocationTypeDTO(locationTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LocationTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(locationTypeDTO);
            return locationTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="reportsScheduleEmailDTO">reportsScheduleEmailDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLocationTypeDTO(LocationTypeDTO locationTypeDTO , DataTable dt)
        {
            log.LogMethodEntry(locationTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                locationTypeDTO.LocationTypeId = Convert.ToInt32(dt.Rows[0]["LocationTypeId"]);
                locationTypeDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                locationTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                locationTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                locationTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                locationTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                locationTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the LocationType data of passed Id
        /// </summary>
        /// <param name="locationTypeId">Int type parameter</param>
        /// <returns>Returns LocationTypeDTO</returns>
        public LocationTypeDTO GetLocationType(int locationTypeId)
        {
            log.LogMethodEntry(locationTypeId);
            LocationTypeDTO result = null;
            string selectLocationTypeQuery = SELECT_QUERY + @" WHERE lt.LocationTypeId = @locationTypeId";

            SqlParameter parameter = new SqlParameter("@locationTypeId", locationTypeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLocationTypeQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLocationTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the LocationDTO list matching the search key
        /// </summary>
        /// <param name="locationTypes"></param>
        /// <param name="SiteID"></param>
        /// <returns></returns>
        public List<LocationTypeDTO> GetLocationTypeListOnType(string locationTypes, int SiteID)
        {
            log.LogMethodEntry(locationTypes, SiteID);
            List<LocationTypeDTO> locationlist = null;
            string selectLocations = @"select * from Locationtype  
                                                    where (site_id = @SiteID or -1 = @SiteID) and LocationType ";
            StringBuilder query = new StringBuilder();
            query.Append(" IN(" + locationTypes + ") ");
            selectLocations = selectLocations + query;
            SqlParameter[] selectReqParameters = new SqlParameter[1];
            selectReqParameters[0] = new SqlParameter("@SiteID", SiteID);
            //selectReqParameters[1] = new SqlParameter("@locationType", locationTypes);
            DataTable locationTypeDT = dataAccessHandler.executeSelectQuery(selectLocations, selectReqParameters.ToArray(),sqlTransaction);
            if (locationTypeDT.Rows.Count > 0)
            {
                locationlist = new List<LocationTypeDTO>();
                foreach (DataRow locationTypeDTRow in locationTypeDT.Rows)
                {
                    LocationTypeDTO locationTypeDataObject = GetLocationTypeDTO(locationTypeDTRow);
                    locationlist.Add(locationTypeDataObject);
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
        /// Gets the LocationTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LocationTypeDTO matching the search criteria</returns>
        public List<LocationTypeDTO> GetLocationTypeList(List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LocationTypeDTO> locationTypeList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE_ID
                            || searchParameter.Key == LocationTypeDTO.SearchByLocationTypeParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" ));
                        }
                        else if (searchParameter.Key == LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                locationTypeList = new List<LocationTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LocationTypeDTO locationTypeDTO  = GetLocationTypeDTO(dataRow);
                    locationTypeList.Add(locationTypeDTO);
                }
            }
            log.LogMethodExit(locationTypeList);
            return locationTypeList;
        }

        internal DateTime? GetLocationTypeLastUpdateTime(int siteId)
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

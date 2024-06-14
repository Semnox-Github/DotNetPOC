/* Project Name - Semnox.Parafait.Booking.FacilityDataHandler 
* Description  - Data handler object of the CheckInFacility
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.70        26-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
*2.70        29-Jun-2019    Akshay G             Added DeleteFacility() method
*2.70.2      09-Oct-2019    Akshay G             ClubSpeed interface enhancement changes - Added InterfaceType, InterfaceName and ExternalSystemReference
*2.70.2      10-Dec-2019    Jinto Thomas         Removed siteid from update query
*2.80.0      27-Feb-2020    Girish Kundar        Modified : 3 tier changes for API 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class FacilityDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<FacilityDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<FacilityDTO.SearchByParameters, string>
            {
                {FacilityDTO.SearchByParameters.FACILITY_ID, "fac.facilityId"},
                {FacilityDTO.SearchByParameters.FACILITY_ID_LIST, "fac.facilityId"},
                {FacilityDTO.SearchByParameters.FACILITY_NAME, "fac.facilityName"},
                {FacilityDTO.SearchByParameters.ACTIVE_FLAG, "fac.active_Flag"},
                {FacilityDTO.SearchByParameters.ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE, "fac.AllowMultipleBookings"},
                {FacilityDTO.SearchByParameters.MASTER_ENTITY_ID, "fac.MasterEntityId"},
                {FacilityDTO.SearchByParameters.SITE_ID, "fac.site_id"},
                {FacilityDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, "product_type"},
                {FacilityDTO.SearchByParameters.FACILITY_MAP_ID, ""},
                {FacilityDTO.SearchByParameters.INTERFACE_TYPE, "fac.InterfaceType"},
                {FacilityDTO.SearchByParameters.INTERFACE_NAME, "fac.InterfaceName"}
            };

        private static readonly string facilitySelectQuery = @"SELECT *  FROM CheckInFacility fac";

        /// <summary>
        /// Default constructor of  FacilityDataHandler class
        /// </summary>
        public FacilityDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating checkInFacility Record.
        /// </summary>
        /// <param name="FacilityDTO">FacilityDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(FacilityDTO facilityDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityId", facilityDTO.FacilityId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityName", facilityDTO.FacilityName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", facilityDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Capacity", facilityDTO.Capacity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InternetKey", facilityDTO.InternetKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenPosition", facilityDTO.ScreenPosition));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", (facilityDTO.ActiveFlag == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AllowMultipleBookings", facilityDTO.AllowMultipleBookings));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", facilityDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InterfaceType", facilityDTO.InterfaceType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InterfaceName", facilityDTO.InterfaceName, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", facilityDTO.ExternalSystemReference));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Facility record to the database
        /// </summary>
        /// <param name="FacilityDTO">FacilityDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public FacilityDTO InsertFacility(FacilityDTO facilityDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityDTO, loginId, siteId);
            string query = @"INSERT INTO dbo.CheckInFacility
                                       (FacilityName
                                       ,description
                                       ,active_flag
                                       ,AllowMultipleBookings
                                       ,Capacity
                                       ,last_updated_date
                                       ,last_updated_user
                                       ,InternetKey
                                       ,Guid
                                       ,site_id 
                                       ,ScreenPosition
                                       ,MasterEntityId
                                       ,CreatedBy
                                       ,CreationDate
                                       ,InterfaceType
                                       ,InterfaceName
                                       ,ExternalSystemReference
                                     )
                                 VALUES
                                       (@FacilityName
                                       ,@Description
                                       ,@ActiveFlag
                                       ,@AllowMultipleBookings
                                       ,@Capacity
                                       ,getdate()
                                       ,@LastUpdatedBy
                                       ,@InternetKey
                                       ,NEWID()
                                       ,@site_id 
                                       ,@ScreenPosition
                                       ,@MasterEntityId
                                       ,@CreatedBy
                                       ,getdate()
                                       ,@InterfaceType
                                       ,@InterfaceName
                                       ,@ExternalSystemReference
                                      ) SELECT * FROM CheckInFacility WHERE FacilityId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilityDTO(facilityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting facilityDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityDTO);
            return facilityDTO;
        }

        private void RefreshFacilityDTO(FacilityDTO facilityDTO, DataTable dt)
        {
            log.LogMethodEntry(facilityDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                facilityDTO.FacilityId = Convert.ToInt32(dt.Rows[0]["FacilityId"]);
                facilityDTO.LastUpdatedDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                facilityDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                facilityDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                facilityDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                facilityDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                facilityDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the Facility record
        /// </summary>
        /// <param name="FacilityDTO">facilityDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public FacilityDTO UpdateFacility(FacilityDTO facilityDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityDTO, loginId, siteId);
            string query = @"
                            UPDATE dbo.CheckInFacility
                               SET FacilityName = @FacilityName
                                  ,description = @Description
                                  ,active_flag = @ActiveFlag
                                  ,AllowMultipleBookings = @AllowMultipleBookings
                                  ,Capacity = @Capacity
                                  ,last_updated_date = getdate()
                                  ,last_updated_user = @LastUpdatedBy
                                  ,InternetKey = @InternetKey 
                                  ,ScreenPosition = @ScreenPosition
                                  ,MasterEntityId = @MasterEntityId
                                  ,InterfaceType = @InterfaceType
                                  ,InterfaceName = @InterfaceName
                                  ,ExternalSystemReference = @ExternalSystemReference
                                  WHERE FacilityId = @FacilityId
                                 SELECT * FROM CheckInFacility WHERE FacilityId =  @FacilityId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilityDTO(facilityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating facilityDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityDTO);
            return facilityDTO;
        }


        /// <summary>
        /// Converts the Data row object to GetFacilityDTO calss type
        /// </summary>
        /// <param name="facilityRow">facility DataRow</param>
        /// <returns>Returns facilityDTO</returns>
        private FacilityDTO GetFacilityDTO(DataRow facilityRow)
        {
            log.LogMethodEntry(facilityRow);
            FacilityDTO checkInFacilityDTO = new FacilityDTO(
                        Convert.ToInt32(facilityRow["FacilityId"]),
                        facilityRow["FacilityName"] == DBNull.Value ? string.Empty : Convert.ToString(facilityRow["FacilityName"]),
                        facilityRow["description"] == DBNull.Value ? string.Empty : Convert.ToString(facilityRow["description"]),
                        string.IsNullOrEmpty(facilityRow["active_flag"].ToString()) ? true : (facilityRow["active_flag"].ToString() == "Y" ? true : false),
                        facilityRow["AllowMultipleBookings"] == DBNull.Value ? true : Convert.ToBoolean(facilityRow["AllowMultipleBookings"]),
                        string.IsNullOrEmpty(facilityRow["Capacity"].ToString()) ? (int?)null : Convert.ToInt32(facilityRow["Capacity"]),
                        string.IsNullOrEmpty(facilityRow["InternetKey"].ToString()) ? (int?)null : Convert.ToInt32(facilityRow["InternetKey"]),
                        facilityRow["screenPosition"] == DBNull.Value ? string.Empty : Convert.ToString(facilityRow["screenPosition"]),
                        string.IsNullOrEmpty(facilityRow["site_id"].ToString()) ? -1 : Convert.ToInt32(facilityRow["site_id"]),
                        facilityRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(facilityRow["guid"]),
                        string.IsNullOrEmpty(facilityRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(facilityRow["SynchStatus"]),
                        string.IsNullOrEmpty(facilityRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(facilityRow["MasterEntityId"]),
                        string.IsNullOrEmpty(facilityRow["CreatedBy"].ToString()) ? string.Empty : Convert.ToString(facilityRow["CreatedBy"]),
                        facilityRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilityRow["CreationDate"]),
                        string.IsNullOrEmpty(facilityRow["last_updated_user"].ToString()) ? string.Empty : Convert.ToString(facilityRow["last_updated_user"]),
                        facilityRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilityRow["last_updated_date"]),
                        facilityRow["InterfaceType"] == DBNull.Value ? -1 : Convert.ToInt32(facilityRow["InterfaceType"]),
                        facilityRow["InterfaceName"] == DBNull.Value ? -1 : Convert.ToInt32(facilityRow["InterfaceName"]),
                        facilityRow["ExternalSystemReference"].ToString()
                        );
            log.LogMethodExit(checkInFacilityDTO);
            return checkInFacilityDTO;
        }

        /// <summary>
        /// Gets the Facility data of passed facility Id
        /// </summary>
        /// <param name="facilityId">integer type parameter</param>
        /// <returns>Returns FacilityDTO</returns>
        public FacilityDTO GetFacilityDTO(int facilityId)
        {
            log.LogMethodEntry(facilityId);
            string selectCheckInFacilityQuery = facilitySelectQuery + "  WHERE FacilityID = @FacilityId";
            SqlParameter[] selectCheckInFacilityParameters = new SqlParameter[1];
            selectCheckInFacilityParameters[0] = new SqlParameter("@FacilityId", facilityId);
            DataTable checkInFacility = dataAccessHandler.executeSelectQuery(selectCheckInFacilityQuery, selectCheckInFacilityParameters, sqlTransaction);
            FacilityDTO checkInFacilityDataObject = null;
            if (checkInFacility.Rows.Count > 0)
            {
                DataRow CheckInFacilityRow = checkInFacility.Rows[0];
                checkInFacilityDataObject = GetFacilityDTO(CheckInFacilityRow);
            }
            log.LogMethodExit(checkInFacilityDataObject);
            return checkInFacilityDataObject;
        }

        /// <summary>
        /// Gets the FacilityDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of FacilityDTO matching the search criteria</returns>
        public List<FacilityDTO> GetFacilityDTOList(List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<FacilityDTO> list = new List<FacilityDTO>();
            int count = 0;
            string selectQuery = facilitySelectQuery;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner =string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<FacilityDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == FacilityDTO.SearchByParameters.FACILITY_ID ||
                            searchParameter.Key == FacilityDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == FacilityDTO.SearchByParameters.INTERFACE_TYPE ||
                            searchParameter.Key == FacilityDTO.SearchByParameters.INTERFACE_NAME
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.FACILITY_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN)
                        {
                            query.Append(joiner + @"EXISTS ( SELECT 1
                                                               FROM product_type pt, (SELECT p.* from products p
                                                                                       WHERE p.active_flag = 'Y'
						                                                                 AND (
                                                                                                --EXISTS (SELECT 1 
							                                                                    --        FROM facilityMapDetails fmd 
											                                                    --        WHERE fmd.FacilityId = fac.facilityId 
											                                                    --          AND fmd.IsActive = 1
											                                                    --          AND fmd.FacilityMapId = p.FacilityMapId)
									                                                            --OR
									                                                           EXISTS (SELECT 1
											                                                             FROM ProductsAllowedInFacility paif ,
											                                                                  facilityMapDetails fmd
											                                                            WHERE paif.isActive = 1
											                                                              AND paif.FacilityMapId = fmd.FacilityMapId
                                                                                                          AND fmd.IsActive = 1
											                                                              AND fmd.FacilityId = fac.facilityId 
											                                                              AND paif.ProductsId = p.product_id))) as pp
                                                                AND pp.product_type_id = pt.product_type_id
                                                                AND pt.product_type IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ) )");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.FACILITY_MAP_ID)
                        {
                            query.Append(joiner + @"EXISTS ( SELECT 1 
                                                               FROM facilityMap fm, facilityMapDetails fmd
                                                              WHERE fmd.FacilityId = fac.facilityId 
                                                                AND fmd.IsActive = 1
                                                                AND fmd.FacilityMapId = fm.FacilityMapId 
                                                                AND fm.FacilityMapId =  " + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    count++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FacilityDTO facilityDTO = GetFacilityDTO(dataRow);
                    list.Add(facilityDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        internal List<FacilityDTO> GetFacilityListWithMaxRowNColumnInfo()
        {
            log.LogMethodEntry();
            List<FacilityDTO> list = null;
            string selecWithMaxRowNColumnInfoQuery = @"(SELECT FT.FacilityId facilityid,
                                                               CF.FacilityName facilityname,
                                                               CF.description description,
                                                               max(FT.RowIndex) + 1 maxrowindex,
	                                                           max(FT.ColumnIndex) + 1 maxcolindex
                                                          FROM CheckInFacility CF,
                                                               FacilityTables FT
                                                         WHERE FT.FacilityId = CF.FacilityId 
                                                           AND FT.active='Y'
                                                         GROUP BY FT.FacilityId,CF.FacilityName ";
            DataTable facilityInfoDT = dataAccessHandler.executeSelectQuery(selecWithMaxRowNColumnInfoQuery, null, sqlTransaction);

            if (facilityInfoDT.Rows.Count > 0)
            {
                list = new List<FacilityDTO>();
                foreach (DataRow dataRow in facilityInfoDT.Rows)
                {
                    FacilityDTO facilityDTO = GetFacilityDTOWithMaxRowNColumnInfo(dataRow);
                    list.Add(facilityDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        private FacilityDTO GetFacilityDTOWithMaxRowNColumnInfo(DataRow facilityRow)
        {
            log.LogMethodEntry();
            FacilityDTO facilityDTO = new FacilityDTO(Convert.ToInt32(facilityRow["facilityid"]),
                                                      facilityRow["facilityname"].ToString(),
                                                      facilityRow["description"].ToString(),
                                                      Convert.ToInt32(facilityRow["maxrowindex"]),
                                                      Convert.ToInt32(facilityRow["maxcolindex"]));
            log.LogMethodExit(facilityDTO);
            return facilityDTO;
        }


        /// <summary>
        /// GetConfiguredFacility
        /// </summary>
        /// <returns> list of FacilityDTO</returns>
        internal List<FacilityDTO> GetConfiguredFacility(string macAddress)
        {
            log.LogMethodEntry(macAddress);
            List<FacilityDTO> facilityLists = new List<FacilityDTO>();
            try
            {
                string getFacilityQuery = @"select CF.FacilityId, CF.FacilityName, CF.description Description, MAX(FT.RowIndex) + 1 MaxRowIndex, MAX(FT.ColumnIndex) + 1 MaxColIndex 
                                             from CheckInFacility CF, FacilityPOSAssignment FPA, POSMachines PM,  FacilityTables FT
                                            where FPA.FacilityId=CF.FacilityId and FPA.POSMachineId=PM.POSMachineId and FT.FacilityId = CF.FacilityId and CF.active_flag = 'Y'
                                              and (PM.IPAddress= @macAddress or PM.Computer_Name= @macAddress) GROUP BY CF.FacilityId,CF.FacilityName,CF.description";

                List<SqlParameter> queryParams = new List<SqlParameter>();
                queryParams.Add(new SqlParameter("@macAddress", macAddress));

                DataTable dtFacilities = dataAccessHandler.executeSelectQuery(getFacilityQuery, queryParams.ToArray(), sqlTransaction);

                if (dtFacilities.Rows.Count > 0)
                {
                    FacilityDTO facilityDTO = null;
                    foreach (DataRow facilityRow in dtFacilities.Rows)
                    {
                        facilityDTO = new FacilityDTO(
                            Convert.ToInt32(facilityRow["FacilityId"]),
                            facilityRow["FacilityName"].ToString(),
                             facilityRow["Description"].ToString(),
                            Convert.ToInt32(facilityRow["MaxRowIndex"]),
                            Convert.ToInt32(facilityRow["MaxColIndex"]));
                        facilityLists.Add(facilityDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(facilityLists);
            return facilityLists;
        }
        /// <summary>
        /// Based on the facilityId, appropriate Facility record will be deleted
        /// </summary>
        /// <param name="facilityId">facilityId</param>
        /// <returns>return the int</returns>
        internal int DeleteFacility(int facilityId)
        {
            log.LogMethodEntry(facilityId);
            try
            {
                string deleteQuery = @"delete from CheckInFacility where FacilityId = @facilityId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@facilityId", facilityId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }
    }
}

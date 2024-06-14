/********************************************************************************************
 * Project Name - FacilityMapDataHandler
 * Description  - data handler file for  FacilityMap
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        14-Jun-2019   Guru S A                Created 
 *2.71        24-Jul-2019   Nitin Pai               Attraction enhancement for combo products
 *2.70.2      18-Oct-2019   Akshay G                ClubSpeed enhancement changes - Added DBSearchParameters
 *2.70.2      10-Dec-2019   Jinto Thomas            Removed siteid from update query
 *            01-Jan-2020   Akshay G                Added searchParameters - ALLOWED_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE and ALLOWED_PRODUCTS_IS_COMBO_CHILD
 *2.90        03-Jun-2020   Guru S A                reservation enhancements for commando release
 *2.100       24-Sep-2020   Nitin Pai               Attraction Reschedule: Schedule information is moved from ATB to DAS
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// FacilityMap data handler class
    /// </summary>
    public class FacilityMapDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT vf.*, (SELECT sum(ISNULL(fac.Capacity,0)) 
                                                             from checkinFacility fac, FacilityMapDetails vfd
                                                            WHERE vfd.FacilityMapId = vf.FacilityMapId
                                                              AND vfd.IsActive = 1
                                                              AND fac.FacilityId = vfd.FacilityId
                                                              AND fac.Active_flag = 'Y' ) as FacilityCapacity FROM FacilityMap AS vf"; // Create alias names for the Table
        /// <summary>
        /// Dictionary for searching Parameters for the FacilityMap object.
        /// </summary>
        private static readonly Dictionary<FacilityMapDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<FacilityMapDTO.SearchByParameters, string>
        {
            { FacilityMapDTO.SearchByParameters.FACILITY_MAP_ID,"vf.FacilityMapId"},
            { FacilityMapDTO.SearchByParameters.MASTER_SCHEDULE_ID,"vf.MasterScheduleId"},
            { FacilityMapDTO.SearchByParameters.FACILITY_MAP_IDS_IN,"vf.FacilityMapId"},
            { FacilityMapDTO.SearchByParameters.CANCELLATION_PRODUCT_ID,"vf.CancellationProductId"},
            { FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN,"product_type"},
            { FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_ID,"productId"},
            { FacilityMapDTO.SearchByParameters.IS_ACTIVE,"vf.IsActive"},
            { FacilityMapDTO.SearchByParameters.SITE_ID,"vf.site_id"},
            { FacilityMapDTO.SearchByParameters.MASTER_ENTITY_ID,"vf.MasterEntityId"},
            { FacilityMapDTO.SearchByParameters.FACILITY_ID,"fac.FacilityId"},
            { FacilityMapDTO.SearchByParameters.FACILITY_IDS_IN,"fac.FacilityId"},
            { FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_IDS_IN,"productId"},
            { FacilityMapDTO.SearchByParameters.LAST_UPDATED_FROM_DATE,"vf.LastUpdateDate"},
            { FacilityMapDTO.SearchByParameters.LAST_UPDATED_TO_DATE,"vf.LastUpdateDate"},
            { FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE,"p.ExternalSystemReference"},
            { FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCTS_IS_COMBO_CHILD,"cp.ChildProductId"},
            { FacilityMapDTO.SearchByParameters.FACILITY_MAP_NAME,"vf.FacilityMapName"}
        };

        /// <summary>
        /// Parameterized Constructor for FacilityMapDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public FacilityMapDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating FacilityMap Record.
        /// </summary>
        /// <param name="facilityMapDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(FacilityMapDTO facilityMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityMapDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityMapId", facilityMapDTO.FacilityMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityMapName", facilityMapDTO.FacilityMapName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterScheduleId", facilityMapDTO.MasterScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancellationProductId", facilityMapDTO.CancellationProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GraceTime", facilityMapDTO.GraceTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", facilityMapDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", facilityMapDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to FacilityMapDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>facilityMapDTO</returns>
        private FacilityMapDTO GetFacilityMapDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            FacilityMapDTO facilityMapDTO = new FacilityMapDTO(dataRow["FacilityMapId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FacilityMapId"]),
                                                         dataRow["FacilityMapName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FacilityMapName"]),
                                                         dataRow["MasterScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterScheduleId"]),
                                                         dataRow["CancellationProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CancellationProductId"]),
                                                         string.IsNullOrEmpty(dataRow["graceTime"].ToString()) ? (int?)null : Convert.ToInt32(dataRow["graceTime"]),
                                                         dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["FacilityCapacity"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["FacilityCapacity"])
                                                        );
            log.LogMethodExit(facilityMapDTO);
            return facilityMapDTO;
        }

        /// <summary>
        /// Gets the FacilityMap data of passed FacilityMapId 
        /// </summary>
        /// <param name="facilityMapId">integer type parameter</param>
        /// <returns>Returns FacilityMapDTO</returns>
        public FacilityMapDTO GetFacilityMap(int facilityMapId)
        {
            log.LogMethodEntry(facilityMapId);
            FacilityMapDTO result = null;
            string query = SELECT_QUERY + @" WHERE vf.FacilityMapId = @FacilityMapId";
            SqlParameter parameter = new SqlParameter("@FacilityMapId", facilityMapId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetFacilityMapDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the FacilityMap Table.
        /// </summary>
        /// <param name="facilityMapDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public FacilityMapDTO Insert(FacilityMapDTO facilityMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityMapDTO, loginId, siteId);
            string query = @"INSERT INTO dbo.FacilityMap
                            (
                            FacilityMapName
                           ,MasterScheduleId
                           ,CancellationProductId
                           ,GraceTime
                           ,IsActive
                           ,Guid
                           ,CreatedBy
                           ,CreationDate
                           ,LastUpdatedBy
                           ,LastUpdateDate
                           ,site_id
                           ,MasterEntityId 
                            )
                        VALUES
                            (
                            @FacilityMapName
                           ,@MasterScheduleId
                           ,@CancellationProductId
                           ,@GraceTime
                           ,@IsActive
                           ,NEWID()
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE() 
                           ,@site_id
                           ,@MasterEntityId
                         )
                            SELECT * FROM FacilityMap WHERE FacilityMapId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilityMapDTO(facilityMapDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting FacilityMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityMapDTO);
            return facilityMapDTO;
        }

        /// <summary>
        ///  Updates the record to the FacilityMap Table.
        /// </summary>
        /// <param name="facilityMapDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public FacilityMapDTO Update(FacilityMapDTO facilityMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityMapDTO, loginId, siteId);
            string query = @"UPDATE dbo.FacilityMap
                                SET FacilityMapName =   @FacilityMapName
                                   ,MasterScheduleId = @MasterScheduleId
                                   ,CancellationProductId =   @CancellationProductId
                                   ,GraceTime = @GraceTime
                                   ,IsActive =  @IsActive
                                   ,LastUpdatedBy = @LastUpdatedBy
                                   ,LastUpdateDate = GETDATE()
                                   -- ,site_id = @site_id
                                   ,MasterEntityId =   @MasterEntityId 
                              where FacilityMapId = @FacilityMapId;
                           SELECT * FROM FacilityMap WHERE FacilityMapId = @FacilityMapId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilityMapDTO(facilityMapDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating facilityMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityMapDTO);
            return facilityMapDTO;
        }

        private void RefreshFacilityMapDTO(FacilityMapDTO facilityMapDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityMapDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                facilityMapDTO.FacilityMapId = Convert.ToInt32(dt.Rows[0]["FacilityMapId"]);
                facilityMapDTO.LastUpdatedDate = dt.Rows[0]["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                facilityMapDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                facilityMapDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                facilityMapDTO.LastUpdatedBy = dt.Rows[0]["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                facilityMapDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                facilityMapDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of facilityMapDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>facilityMapDTOList</returns>
        public List<FacilityMapDTO> GetAllFacilityMap(List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<FacilityMapDTO> facilityMapDTOList = new List<FacilityMapDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<FacilityMapDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == FacilityMapDTO.SearchByParameters.FACILITY_MAP_ID ||
                            searchParameter.Key == FacilityMapDTO.SearchByParameters.MASTER_SCHEDULE_ID ||
                            searchParameter.Key == FacilityMapDTO.SearchByParameters.CANCELLATION_PRODUCT_ID ||
                             searchParameter.Key == FacilityMapDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN)
                        {
                            query.Append(joiner + @"EXISTS ( SELECT 1 
                                                               FROM product_type pt,(SELECt  p.* 
			                                                                           FROM products p 
								                                                      WHERE p.active_flag = 'Y' 
								                                                        AND (
                                                                                             --p.FacilityMapId = vf.FacilityMapId 
									                                                         --or
                                                                                                 exists (SELECT 1
											                                                               FROM ProductsAllowedInFacility paif 
														                                                  WHERE paif.isActive = 1
														                                                    AND paif.FacilityMapId = vf.FacilityMapId
														                                                    AND paif.ProductsId = p.product_id))
                                                                                       ) as pp
                                                              WHERE pp.product_type_id = pt.product_type_id
                                                                AND pt.product_type IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ) )");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_ID)
                        {
                            query.Append(joiner + @"EXISTS ( SELECt  1
			                                                   FROM products p, ProductsAllowedInFacility paif																	         
								                              WHERE p.active_flag = 'Y' 
								                                AND paif.isActive = 1
															    AND paif.ProductsId = p.product_id
																AND paif.FacilityMapId = vf.FacilityMapId  
																AND p.product_id =  " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.FACILITY_MAP_IDS_IN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_IDS_IN)
                        {
                            query.Append(joiner + @"EXISTS ( SELECT  1
			                                                   FROM products p, ProductsAllowedInFacility paif																	         
								                              WHERE p.active_flag = 'Y' 
								                                AND paif.isActive = 1
															    AND paif.ProductsId = p.product_id
																AND paif.FacilityMapId = vf.FacilityMapId  
																AND p.product_id IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ) )");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.FACILITY_ID)
                        {
                            query.Append(joiner + @"EXISTS (SELECT 1 
                                                             from checkinFacility fac, FacilityMapDetails vfd
                                                            WHERE vfd.FacilityMapId = vf.FacilityMapId
                                                              AND vfd.IsActive = 1
                                                              AND fac.FacilityId = vfd.FacilityId
                                                              AND fac.Active_flag = 'Y' 
                                                              AND fac.FacilityId =  " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.FACILITY_IDS_IN)
                        {
                            query.Append(joiner + @"EXISTS (SELECT 1 
                                                             from checkinFacility fac, FacilityMapDetails vfd
                                                            WHERE vfd.FacilityMapId = vf.FacilityMapId
                                                              AND vfd.IsActive = 1
                                                              AND fac.FacilityId = vfd.FacilityId
                                                              AND fac.Active_flag = 'Y' 
                                                              AND fac.FacilityId IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ) )");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.LAST_UPDATED_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.LAST_UPDATED_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCTS_IS_COMBO_CHILD) // bit - 0 or 1
                        {
                            query.Append(joiner + @"ISNULL( ( SELECT  1
					                                                FROM ComboProduct cp, products p, ProductsAllowedInFacility paif																	         
					                                                WHERE p.active_flag = 'Y' 
					                                                AND paif.isActive = 1
					                                                AND paif.ProductsId = p.product_id
					                                                AND paif.FacilityMapId = vf.FacilityMapId  
					                                                AND ISNULL(cp.ISActive,1) = 1
					                                                AND " + DBSearchParameters[searchParameter.Key] + @" = paif.ProductsId 
                                                            ),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE) // bit - 0 or 1
                        {
                            query.Append(joiner + @"ISNULL( ( SELECT 1 FROM products p, ProductsAllowedInFacility paif
                                                                        WHERE p.active_flag = 'Y' 
                                                                        AND paif.isActive = 1
                                                                        AND paif.ProductsId = p.product_id
                                                                        AND paif.FacilityMapId = vf.FacilityMapId  
                                                                        AND " + DBSearchParameters[searchParameter.Key] + @" IS NOT NULL 
                                                            ),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == FacilityMapDTO.SearchByParameters.FACILITY_MAP_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FacilityMapDTO facilityMapDTO = GetFacilityMapDTO(dataRow);
                    facilityMapDTOList.Add(facilityMapDTO);
                }
            }
            log.LogMethodExit(facilityMapDTOList);
            return facilityMapDTOList;
        }

        internal int GetTotalBookedUnitsForAttractions(int facilityMapId, DateTime scheduleFromDate, DateTime scheduleToDate, int bookingId)
        {
            log.LogMethodEntry(facilityMapId, scheduleFromDate, scheduleToDate, bookingId);
            int bookedUnits = 0;
            string selectTotalBookedUnitsQuery = @"SELECT ISNULL( (
                                                    select sum(bookedUnits) as bookedUnits
                                                      FROM (
                                                            SELECT da.attractionScheduleId as ScheduleId, (CASE WHEN atb.expiryDate IS NULL THEN BookedUnits WHEN atb.expiryDate < getdate() THEN 0 ELSE BookedUnits END) bookedUnits
                                                             FROM attractionBookings atb, DayAttractionSchedule da
                                                             WHERE atb.DayAttractionScheduleId = da.DayAttractionScheduleId
                                                               AND ((  da.ScheduleDateTime <= @scheduleFromDate  AND  da.scheduleToDateTime > @scheduleFromDate    )
                                                                    OR (da.ScheduleDateTime < @scheduleToDate AND da.scheduleToDateTime >= @scheduleToDate  )
																	OR (  @scheduleFromDate < da.ScheduleDateTime  AND @scheduleToDate > da.scheduleToDateTime  )) 
                                                               AND exists (
                                                                        SELECT 1 
                                                                          FROM facilityMapDetails vfd, facilityMapDetails vfdInput,
                                                                               CheckInFacility fac
                                                                         where vfdInput.facilityMapId = @facilityMapId                                                               
                                                                           AND vfdInput.FacilityId = vfd.FacilityId
                                                                           AND vfd.facilityMapId = da.facilityMapId
                                                                           AND vfd.IsActive = 1
                                                                           AND vfd.FacilityId = fac.FacilityId
                                                                           AND fac.active_flag = 'Y'    
                                                                            )  
                                                               AND atb.BookingId != @bookingId 
                                                               AND NOT EXISTS (SELECT 1 
                                                                                 FROM bookings b 
                                                                                where b.trxId = atb.trxId and b.ExpiryTime is not null and b.ExpiryTime < getdate()
                                                                                  AND b.Status not in ('CANCELLED','SYSTEMABANDONED')
                                                             )) as atbBooked),0) AS bookedUnits";
            SqlParameter[] selectTotalBookedUnitsParameters = new SqlParameter[4];
            selectTotalBookedUnitsParameters[0] = new SqlParameter("@facilityMapId", facilityMapId);
            selectTotalBookedUnitsParameters[1] = new SqlParameter("@scheduleFromDate", scheduleFromDate);
            selectTotalBookedUnitsParameters[2] = new SqlParameter("@scheduleToDate", scheduleToDate);
            selectTotalBookedUnitsParameters[3] = new SqlParameter("@bookingId", bookingId);
            DataTable facilityBookedUnitsDT = dataAccessHandler.executeSelectQuery(selectTotalBookedUnitsQuery, selectTotalBookedUnitsParameters, sqlTransaction);

            if (facilityBookedUnitsDT.Rows.Count > 0)
            {
                bookedUnits = Convert.ToInt32(facilityBookedUnitsDT.Rows[0]["bookedUnits"]);
            }
            log.LogMethodExit(bookedUnits);
            return bookedUnits;
        }

        internal int GetTotalBookedUnitsForReservation(int facilityMapId, DateTime scheduleFromDate, DateTime scheduleToDate, int productId, int bookingTrxId, int trxReservationScheduleId)
        {
            log.LogMethodEntry(facilityMapId, scheduleFromDate, scheduleToDate, bookingTrxId, trxReservationScheduleId);
            int bookedUnits = 0;
            string selectTotalBookedUnitsQuery = @"SELECT ISNULL( (
                                                    select sum(bookedUnits) as bookedUnits
                                                      FROM ( 
                                                            SELECT trs.SchedulesId as scheduleId, (CASE WHEN trs.Cancelled =0 AND 
                                                                                                             CASE when ISNULL(bb.expirytime,getdate()) < getdate() THEN 1 ELSE 0 END = 0 THEN 
                                                                                                             trs.GuestQuantity 
                                                                                                        ELSE 0 END) bookedUnits
                                                              FROM TrxReservationSchedule trs 
                                                                    left outer join bookings bb on bb.trxId = trs.TrxId and bb.status not in ('CANCELLED','SYSTEMABANDONED')
                                                            WHERE ( (  trs.ScheduleFromDate <= @scheduleFromDate  AND  trs.scheduleToDate > @scheduleFromDate    )
                                                                    OR (trs.ScheduleFromDate < @scheduleToDate AND trs.scheduleToDate >= @scheduleToDate  )
																	OR (  @scheduleFromDate < trs.ScheduleFromDate  AND @scheduleToDate > trs.scheduleToDate  ) )
                                                               AND trs.cancelled = 0 
                                                               AND ISNULL(trs.ExpiryDate, getdate()+1) > getdate()
                                                               AND exists (SELECT 1 
                                                                             FROM facilityMapDetails vfd, facilityMapDetails vfdInput,
                                                                                  CheckInFacility fac
                                                                            where vfdInput.facilityMapId = @facilityMapId                                                               
                                                                              AND vfdInput.FacilityId = vfd.FacilityId
                                                                              AND vfd.facilityMapId = trs.facilityMapId
                                                                              AND vfd.IsActive = 1
                                                                              AND vfd.FacilityId = fac.FacilityId
                                                                              AND fac.active_flag = 'Y'    
                                                                            ) 
                                                               --AND exists (SELECT 1 
                                                                 --            FROM products p , ProductsAllowedInFacility paif
                                                                   --         WHERE paif.facilityMapId = @facilityMapId 
                                                                     --         AND paif.ProductsId = p.product_id
                                                                       --       AND paif.IsActive = '1'
                                                                         --     AND (p.Product_Id = @productId OR @productId = -1))
                                                               AND trs.TrxReservationScheduleId != @trxReservationScheduleId 
                                                               AND not exists (SELECT 1 
                                                                                from bookings b, trx_header th 
                                                                               where b.trxId = th.trxId 
                                                                                 and th.trxId = @trxId
                                                                                 and th.trxId = trs.TrxId
                                                                                 and (b.status not in ('CANCELLED','SYSTEMABANDONED')
                                                                                      OR (b.status in ('WIP','BLOCKED') and (b.ExpiryTime is null or ExpiryTime > getdate()))
                                                                                     ) 
                                                                             )
                                                            ) as FACBookings ),0) as bookedUnits ";
            SqlParameter[] selectTotalBookedUnitsParameters = new SqlParameter[5];
            selectTotalBookedUnitsParameters[0] = new SqlParameter("@facilityMapId", facilityMapId);
            selectTotalBookedUnitsParameters[1] = new SqlParameter("@scheduleFromDate", scheduleFromDate);
            selectTotalBookedUnitsParameters[2] = new SqlParameter("@scheduleToDate", scheduleToDate);
            if (trxReservationScheduleId == -1)
            {
                selectTotalBookedUnitsParameters[3] = new SqlParameter("@trxId", bookingTrxId);
            }
            else
            {
                selectTotalBookedUnitsParameters[3] = new SqlParameter("@trxId", -1);
            }
            selectTotalBookedUnitsParameters[4] = new SqlParameter("@trxReservationScheduleId", trxReservationScheduleId);
            DataTable facilityBookedUnitsDT = dataAccessHandler.executeSelectQuery(selectTotalBookedUnitsQuery, selectTotalBookedUnitsParameters, sqlTransaction);

            if (facilityBookedUnitsDT.Rows.Count > 0)
            {
                bookedUnits = Convert.ToInt32(facilityBookedUnitsDT.Rows[0]["bookedUnits"]);
            }
            log.LogMethodExit(bookedUnits);
            return bookedUnits;
        }

        internal List<FacilityMapDTO> GetFacilityMapByIdList(List<int> facilityMapIdList)
        {
            log.LogMethodEntry(facilityMapIdList);
            List<FacilityMapDTO> result = new List<FacilityMapDTO>();
            string query = SELECT_QUERY + @", @MapIdList List WHERE vf.FacilityMapId =  List.Id"; 
            DataTable dataTable = dataAccessHandler.BatchSelect(query, "@MapIdList", facilityMapIdList, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    FacilityMapDTO dto = GetFacilityMapDTO(item);
                    result.Add(dto);
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

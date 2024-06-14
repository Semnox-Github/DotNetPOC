/* Project Name - Semnox.Parafait.Booking.FacilityDataHandler 
* Description  - Data handler object of the CheckInFacility
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Booking
{
    public class FacilityDataHandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<FacilityDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<FacilityDTO.SearchByParameters, string>
            {
                {FacilityDTO.SearchByParameters.FACILITY_ID, "facilityId"},
                {FacilityDTO.SearchByParameters.FACILITY_NAME, "facilityName"},
                {FacilityDTO.SearchByParameters.ACTIVE_FLAG, "active_Flag"}, 
                {FacilityDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {FacilityDTO.SearchByParameters.SITE_ID, "site_id"} 
            };

        private static readonly string facilitySelectQuery = @"SELECT *  FROM CheckInFacility ";

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
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(FacilityDTO facilityDTO, string userId, int siteId)
        { 
            log.LogMethodEntry(facilityDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityId", facilityDTO.FacilityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityName", facilityDTO.FacilityName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", facilityDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Capacity", facilityDTO.Capacity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InternetKey", facilityDTO.InternetKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenPosition", facilityDTO.ScreenPosition));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GraceTime", facilityDTO.GraceTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", (facilityDTO.ActiveFlag == true? "Y":"N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", facilityDTO.MasterEntityId, true)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId)); 
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Facility record to the database
        /// </summary>
        /// <param name="FacilityDTO">FacilityDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertFacility(FacilityDTO facilityDTO, string userId, int siteId)
        {
            log.LogMethodEntry(facilityDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO dbo.CheckInFacility
                                       (FacilityName
                                       ,description
                                       ,active_flag
                                       ,Capacity
                                       ,last_updated_date
                                       ,last_updated_user
                                       ,InternetKey
                                       ,Guid
                                       ,site_id 
                                       ,ScreenPosition
                                       ,MasterEntityId
                                       ,GraceTime
                                       ,CreatedBy
                                       ,CreationDate)
                                 VALUES
                                       (@FacilityName
                                       ,@Description
                                       ,@ActiveFlag
                                       ,@Capacity
                                       ,getdate()
                                       ,@LastUpdatedBy
                                       ,@InternetKey
                                       ,NEWID()
                                       ,@site_id 
                                       ,@ScreenPosition
                                       ,@MasterEntityId
                                       ,@GraceTime
                                       ,@CreatedBy
                                       ,getdate())SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(facilityDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the Facility record
        /// </summary>
        /// <param name="FacilityDTO">facilityDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateFacility(FacilityDTO facilityDTO, string userId, int siteId)
        {
            log.LogMethodEntry(facilityDTO, userId, siteId);
            int rowsUpdated;
            string query = @"
                            UPDATE dbo.CheckInFacility
                               SET FacilityName = @FacilityName
                                  ,description = @Description
                                  ,active_flag = @ActiveFlag
                                  ,Capacity = @Capacity
                                  ,last_updated_date = getdate()
                                  ,last_updated_user = @LastUpdatedBy
                                  ,InternetKey = @InternetKey 
                                  --,site_id = @site_id  
                                  ,ScreenPosition = @ScreenPosition
                                  ,MasterEntityId = @MasterEntityId
                                  ,GraceTime = @GraceTime
                             WHERE FacilityId = @FacilityId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(facilityDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
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
                                                                    facilityRow["FacilityName"].ToString(),
                                                                    facilityRow["description"].ToString(),
                                                                    string.IsNullOrEmpty(facilityRow["active_flag"].ToString()) ? false : (facilityRow["active_flag"].ToString() == "Y" ? true : false),
                                                                    string.IsNullOrEmpty(facilityRow["Capacity"].ToString()) ? (int?)null : Convert.ToInt32(facilityRow["Capacity"]),
                                                                    string.IsNullOrEmpty(facilityRow["InternetKey"].ToString()) ? (int?)null : Convert.ToInt32(facilityRow["InternetKey"]),
                                                                    facilityRow["screenPosition"].ToString(),
                                                                    string.IsNullOrEmpty(facilityRow["graceTime"].ToString()) ? (int?)null : Convert.ToInt32(facilityRow["graceTime"]),
                                                                    string.IsNullOrEmpty(facilityRow["site_id"].ToString()) ? -1 : Convert.ToInt32(facilityRow["site_id"]),
                                                                    facilityRow["guid"].ToString(),
                                                                    string.IsNullOrEmpty(facilityRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(facilityRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(facilityRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(facilityRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(facilityRow["CreatedBy"].ToString()) ? "" : Convert.ToString(facilityRow["CreatedBy"]),
                                                                    facilityRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilityRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(facilityRow["last_updated_user"].ToString())? "" : Convert.ToString(facilityRow["last_updated_user"]),
                                                                    facilityRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilityRow["last_updated_date"])
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
            FacilityDTO checkInFacilityDataObject = new FacilityDTO();
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
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<FacilityDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == FacilityDTO.SearchByParameters.FACILITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == FacilityDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.ACTIVE_FLAG )
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + (searchParameter.Value == "1"? "'Y'":"'N'"));
                        }
                        else if (searchParameter.Key == FacilityDTO.SearchByParameters.FACILITY_NAME)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = N'" + searchParameter.Value + "' ");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + " N'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
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
    }
}

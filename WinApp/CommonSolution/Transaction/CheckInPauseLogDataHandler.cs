/********************************************************************************************
 * Project Name - CheckInPauseLogDataHandler
 * Description  - Data handler of CheckInPauseLog Class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Mar-2019   Indhu          Created 
 *2.70.2        10-Dec-2019   Jinto Thomas   Removed siteid from update query
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class CheckInPauseLogDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DataAccessHandler dataAccessHandler;
        SqlTransaction sqlTransaction;

        private static readonly Dictionary<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string> DBSearchParameters = new Dictionary<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>
            {
                {CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_PAUSE_LOG_ID, "CheckInPauseLogId"},
                {CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, "CheckInDetailId"},
                {CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID_LIST, "CheckInDetailId"},
                {CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.IS_ACTIVE, "IsActive"},
                {CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.SITE_ID, "site_id"},
                {CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "PauseEndTime"},
             };

        /// <summary>
        /// Default constructor of CheckInPauseLogDataHandler class
        /// </summary>
        public CheckInPauseLogDataHandler()
        {
            log.Debug("Starts-CheckInPauseLogDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-CheckInPauseLogDataHandler() default constructor.");
        }
        /// <summary>
        /// Default constructor of CheckInPauseLogDataHandler class
        /// </summary>
        public CheckInPauseLogDataHandler(SqlTransaction sqlTransaction) : this()
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            //dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountCreditPlusConsumption Record.
        /// </summary>
        /// <param name="checkInPauseLogDTO">checkInPauseLogDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CheckInPauseLogDTO checkInPauseLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(checkInPauseLogDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInPauseLogId", checkInPauseLogDTO.CheckInPauseLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInDetailId", checkInPauseLogDTO.CheckInDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PauseEndTime", checkInPauseLogDTO.PauseEndTime.HasValue ? checkInPauseLogDTO.PauseEndTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : null));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PauseStartTime", checkInPauseLogDTO.PauseStartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TotalPauseTime", checkInPauseLogDTO.TotalPauseTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachine", checkInPauseLogDTO.POSMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PausedBy", checkInPauseLogDTO.PausedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnPausedBy", checkInPauseLogDTO.UnPausedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", checkInPauseLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", checkInPauseLogDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the CheckInPauseLog record to the database
        /// </summary>
        /// <param name="checkInPauseLogDTO">CheckInPauseLogDTO type object</param>
        /// <param name="userId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public CheckInPauseLogDTO InsertCheckInPauseLogDTO(CheckInPauseLogDTO checkInPauseLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(checkInPauseLogDTO, userId, siteId);
            string query = @"insert into CheckInPauseLog
                                        (
                                        CheckInDetailId,
                                        PauseStartTime,
                                        PauseEndTime,
                                        TotalPauseTime,
                                        POSMachine,
                                        PausedBy,
                                        UnPausedBy,
                                        IsActive,
                                        CreatedBy,
                                        CreationDate,
                                        LastUpdatedBy,
                                        LastupdatedDate,
                                        site_id,
                                        MasterEntityId
                                        )
                                    values
                                        (
                                        @CheckInDetailId,
                                        @PauseStartTime,
                                        @PauseEndTime,
                                        @TotalPauseTime,
                                        @POSMachine,
                                        @PausedBy,
                                        @UnPausedBy,
                                        @IsActive,
                                        @CreatedBy,
                                        getdate(),
                                        @LastUpdatedBy,
                                        getdate(),
                                        @site_id,
                                        @MasterEntityId
                                        )
                          SELECT * FROM CheckInPauseLog WHERE CheckInPauseLogId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkInPauseLogDTO, userId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    checkInPauseLogDTO.CheckInPauseLogId = Convert.ToInt32(dt.Rows[0]["CheckInPauseLogId"]);
                    checkInPauseLogDTO.LastUpdateDate = Convert.ToDateTime(dt.Rows[0]["LastupdatedDate"]);
                    checkInPauseLogDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                    checkInPauseLogDTO.LastUpdatedBy = userId;
                    checkInPauseLogDTO.SiteId = siteId;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(checkInPauseLogDTO);
            return checkInPauseLogDTO;
        }

        /// <summary>
        /// Update the CheckInPauseLog record to the database
        /// </summary>
        /// <param name="checkInPauseLogDTO">CheckInPauseLogDTO type object</param>
        /// <param name="userId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public CheckInPauseLogDTO UpdateCheckInPauseLogDTO(CheckInPauseLogDTO checkInPauseLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(checkInPauseLogDTO, userId, siteId);
            string query = @"update CheckInPauseLog set 
                                            CheckInDetailId = @CheckInDetailId,
                                        PauseStartTime= @PauseStartTime,
                                        PauseEndTime = @PauseEndTime,
                                        TotalPauseTime = @TotalPauseTime,
                                        POSMachine = @POSMachine,
                                        PausedBy = @PausedBy,
                                        UnPausedBy = @UnPausedBy,
                                        IsActive = @IsActive,
                                        LastUpdatedBy = @LastUpdatedBy,
                                        LastupdatedDate = getdate(),
                                        -- site_id = @site_id,
                                        MasterEntityId = @MasterEntityId 
                                            where CheckInPauseLogId = @CheckInPauseLogId
                                SELECT * FROM CheckInPauseLog WHERE CheckInPauseLogId = @CheckInPauseLogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkInPauseLogDTO, userId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    checkInPauseLogDTO.LastUpdateDate = Convert.ToDateTime(dt.Rows[0]["LastupdatedDate"]);
                    checkInPauseLogDTO.LastUpdatedBy = userId;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(checkInPauseLogDTO);
            return checkInPauseLogDTO;
        }


        /// <summary>
        /// Converts the Data row object to CheckInPauseLogDTO class type
        /// </summary>
        /// <param name="checkInPauseLogDataRow">CheckInPauseLogDTO DataRow</param>
        /// <returns>Returns CheckInPauseLogDTO</returns>
        private CheckInPauseLogDTO GetCheckInPauseLogDTO(DataRow checkInPauseLogDataRow)
        {
            log.LogMethodEntry(checkInPauseLogDataRow);
            CheckInPauseLogDTO checkInPauseLogDataObject = new CheckInPauseLogDTO(Convert.ToInt32(checkInPauseLogDataRow["CheckInPauseLogId"]),
                                                    checkInPauseLogDataRow["CheckInDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(checkInPauseLogDataRow["CheckInDetailId"]),
                                                    checkInPauseLogDataRow["PauseStartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(checkInPauseLogDataRow["PauseStartTime"]),
                                                    checkInPauseLogDataRow["PauseEndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(checkInPauseLogDataRow["PauseEndTime"]),
                                                    checkInPauseLogDataRow["TotalPauseTime"] == DBNull.Value ? 0 : Convert.ToInt32(checkInPauseLogDataRow["TotalPauseTime"]),
                                                    checkInPauseLogDataRow["POSMachine"].ToString(),
                                                    checkInPauseLogDataRow["PausedBy"].ToString(),
                                                    checkInPauseLogDataRow["UnPausedBy"].ToString(),
                                                    checkInPauseLogDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(checkInPauseLogDataRow["IsActive"]),
                                                    checkInPauseLogDataRow["CreatedBy"].ToString(),
                                                    checkInPauseLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(checkInPauseLogDataRow["CreationDate"]),
                                                    checkInPauseLogDataRow["LastUpdatedBy"].ToString(),
                                                    checkInPauseLogDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(checkInPauseLogDataRow["LastUpdatedDate"]),
                                                    checkInPauseLogDataRow["Guid"].ToString(),
                                                    checkInPauseLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(checkInPauseLogDataRow["site_id"]),
                                                    checkInPauseLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(checkInPauseLogDataRow["SynchStatus"]),
                                                    checkInPauseLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(checkInPauseLogDataRow["MasterEntityId"])
                                                    
                                                    );
            log.LogMethodExit(checkInPauseLogDataObject);
            return checkInPauseLogDataObject;
        }


        /// <summary>
        /// Gets the checkInPauseLog data of passed checkInPauseLogId
        /// </summary>
        /// <param name="checkInPauseLogId">integer type parameter</param>
        /// <returns>Returns CheckInPauseLogDTO</returns>
        public CheckInPauseLogDTO GetCheckInPauseLogDTO(int checkInPauseLogId)
        {
            log.LogMethodEntry(checkInPauseLogId);
            string selectQuery = @"select * from CheckInPauseLog
                                        where CheckInPauseLogId = @CheckInPauseLogId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@CheckInPauseLogId", checkInPauseLogId);
            DataTable checkInPauseLogDataTable = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);
            if (checkInPauseLogDataTable.Rows.Count > 0)
            {
                DataRow checkInPauseLogRow = checkInPauseLogDataTable.Rows[0];
                CheckInPauseLogDTO checkInPauseLogDataObject = GetCheckInPauseLogDTO(checkInPauseLogRow);
                log.LogMethodExit(checkInPauseLogDataObject);
                return checkInPauseLogDataObject;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Gets the CheckInPauseLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrx">SqlTransaction object</param>
        /// <returns>Returns the list of CheckInPauseLogDTO matching the search criteria</returns>
        public List<CheckInPauseLogDTO> GetCheckInPauseLogDTOList(List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParameters, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            string selectQuery = @"select * from CheckInPauseLog ";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                string queryJoiner = "";
                foreach (KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_PAUSE_LOG_ID) ||
                            searchParameter.Key.Equals(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID))
                        {
                            query.Append(queryJoiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                            queryJoiner = " and ";
                        }
                        else if (searchParameter.Key.Equals(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.SITE_ID))
                        {
                            query.Append(queryJoiner + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + "OR -1 =" + searchParameter.Value + ")");
                            queryJoiner = " and ";
                        }
                        else if (searchParameter.Key.Equals(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.IS_ACTIVE))
                        {
                            query.Append(queryJoiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + searchParameter.Value);
                            queryJoiner = " and ";
                        }
                        else if (searchParameter.Key.Equals(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID_LIST))
                        {
                            query.Append(queryJoiner + DBSearchParameters[searchParameter.Key] + " in ( " + searchParameter.Value + " )");
                            queryJoiner = " and ";
                        }
                        else if (searchParameter.Key.Equals(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL))
                        {
                            query.Append(queryJoiner + DBSearchParameters[searchParameter.Key] + " is " + searchParameter.Value);
                            queryJoiner = " and ";
                        }
                        else
                        {
                            query.Append(queryJoiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            queryJoiner = " and ";
                        }
                    }
                    else
                    {
                        log.Error("The query parameter does not exist " + searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }


            DataTable checkInPauseLogData = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTrx);
            if (checkInPauseLogData.Rows.Count > 0)
            {
                List<CheckInPauseLogDTO> checkInPauseLogList = new List<CheckInPauseLogDTO>();
                foreach (DataRow checkInPauseLogDataRow in checkInPauseLogData.Rows)
                {
                    CheckInPauseLogDTO checkInPauseLogDataObject = GetCheckInPauseLogDTO(checkInPauseLogDataRow);
                    checkInPauseLogList.Add(checkInPauseLogDataObject);
                }
                log.LogMethodExit(checkInPauseLogList);
                return checkInPauseLogList;
            }
            else
            {
                log.Debug(null);
                return null;
            }
        }

    }
}

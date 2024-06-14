/********************************************************************************************
 * Project Name - Concurrent Programs Schedules Data Handler
 * Description  - Data handler of the Concurrent Programs Schedules class
 * 
 **************
 **Version Logss
 **************
 *Version     Date           Modified By         Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016    Amaresh             Created
 *2.70.2        24-Jul-2019    Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                          SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query        
 *2.90         26-May-2020    Mushahid Faizan  Modified : 3 Tier changes for Rest API.
 *2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 *2.120.1     11-May-2021      Deeksha          Modified for AWS JobScheduler enhancements
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Programs Schedule DataHandler - Handles insert, update and select of ConcurrentPrograms objects
    /// </summary>

    public class ConcurrentProgramSchedulesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ConcurrentProgramSchedules as cs";

        /// <summary>
        /// Dictionary for searching Parameters for the ConcurrentProgramSchedules object.
        /// </summary>
        private static readonly Dictionary<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string> DBSearchParameters = new Dictionary<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>
        {
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_SCHEDULE_ID, "cs.ProgramScheduleId"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID, "cs.ProgramId"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID_LIST, "cs.ProgramId"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.ACTIVE, "cs.Active"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.RUNAT, "cs.Runat"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.START_DATE, "cs.StartDate"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.KEEP_RUNNING, "cs.KeepRunning"},
              //{ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.CONCURRENT_REQUEST_ID, "cs.ConcurrentRequestId"},
              //{ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.CONCURRENT_SET_PROGRAM_ID, "cs.ConcurrentRequestSetProgramId"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.FREQUENCY, "cs.Frequency"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.MASTER_ENTITY_ID, "cs.MasterEntityId"},
              {ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.SITE_ID, "cs.site_id"}
        };

        /// <summary>
        /// Default constructor of ConcurrentProgramSchedulesDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentProgramSchedulesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor of ReportScheduleDataHandler class
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentProgramSchedulesDataHandler(string connectionString, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(connectionString, sqlTransaction);
            if (!string.IsNullOrEmpty(connectionString))
            {
                dataAccessHandler = new DataAccessHandler(connectionString);
            }
            else
            {
                dataAccessHandler = new DataAccessHandler();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ConcurrentProgramSchedules parameters Record.
        /// </summary>
        /// <param name="concurrentProgramSchedulesDTO">concurrentProgramSchedulesDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ConcurrentProgramSchedulesDTO concurrentProgramSchedulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramSchedulesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@programScheduleId", concurrentProgramSchedulesDTO.ProgramScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@programId", concurrentProgramSchedulesDTO.ProgramId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@frequency", concurrentProgramSchedulesDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@startDate", concurrentProgramSchedulesDTO.StartDate == DateTime.MinValue ? DBNull.Value : (object)concurrentProgramSchedulesDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@endDate", concurrentProgramSchedulesDTO.EndDate == DateTime.MinValue ? DBNull.Value : (object)concurrentProgramSchedulesDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastExecutedOn", string.IsNullOrEmpty(concurrentProgramSchedulesDTO.LastExecutedOn) ? DBNull.Value : (object)DateTime.Parse(concurrentProgramSchedulesDTO.LastExecutedOn)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@runAt", string.IsNullOrEmpty(concurrentProgramSchedulesDTO.RunAt) ? DBNull.Value : (object)concurrentProgramSchedulesDTO.RunAt));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", concurrentProgramSchedulesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", concurrentProgramSchedulesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@ConcurrentRequestId", concurrentProgramSchedulesDTO.ConcurrentRequestId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@ConcurrentRequestSetId", concurrentProgramSchedulesDTO.ConcurrentRequestSetId, true));
            return parameters;
        }

        /// <summary>
        /// Inserts the Concurrent Programs Schedule record to the database 
        /// </summary>
        /// <param name="concurrentProgramsSchedule">ConcurrentProgramSchedulesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param> 
        /// <returns>Returns inserted record id</returns>

        public ConcurrentProgramSchedulesDTO InsertConcurrentProgramsSchedule(ConcurrentProgramSchedulesDTO concurrentProgramsSchedule, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramsSchedule, loginId, siteId);
            string insertconcurrentQuery = @"INSERT INTO [dbo].[ConcurrentProgramSchedules] 
                                                       (                                                 
                                                         ProgramId,
                                                         StartDate,
                                                         RunAt,
                                                         Frequency,
                                                         EndDate,
                                                         Active,
                                                         LastUpdatedDate,
                                                         LastUpdatedUser,
                                                         LastExecutedOn,
                                                         site_id,
                                                         Guid,
                                                         MasterEntityId,
                                                         CreatedBy,
                                                         CreationDate
                                                         --ConcurrentRequestId,
                                                         --ConcurrentRequestSetId
                                                        ) 
                                                  values 
                                                        (
                                                          @programId,
                                                          @startDate,
                                                          @runAt,
                                                          @frequency,
                                                          @endDate,
                                                          @isActive,
                                                          Getdate(),
                                                          @lastUpdatedUser,
                                                          @lastExecutedOn,
                                                          @siteId,
                                                          Newid(),
                                                          @masterEntityId,
                                                          @createdBy,
                                                          Getdate()
                                                          --@ConcurrentRequestId,
                                                          --@ConcurrentRequestSetId
                                                         )SELECT * FROM ConcurrentProgramSchedules WHERE ProgramScheduleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertconcurrentQuery, GetSQLParameters(concurrentProgramsSchedule, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentProgramsScheduleDTO(concurrentProgramsSchedule, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting concurrentProgramsSchedule", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentProgramsSchedule);
            return concurrentProgramsSchedule;
        }

        /// <summary>
        /// Updates the Concurrent Programs Schedule record
        /// </summary>
        /// <param name="concurrentProgramsSchedule">ConcurrentProgramSchedulesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ConcurrentProgramSchedulesDTO UpdateConcurrentProgramsSchedule(ConcurrentProgramSchedulesDTO concurrentProgramsSchedule, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramsSchedule, loginId, siteId);
            string UpdateConcurrentProgramsScheduleQuery = @"update ConcurrentProgramSchedules 
                                                           set ProgramId =@programId, 
                                                             StartDate =@startDate,
                                                             RunAt =@runAt,
                                                             Frequency = @frequency,
                                                            -- ConcurrentRequestId = @ConcurrentRequestId,
                                                            -- ConcurrentRequestSetId = @ConcurrentRequestSetId,
                                                             EndDate =@endDate,
                                                             Active =@isActive,                                                    
                                                             LastUpdatedDate =Getdate(),
                                                             LastUpdatedUser =@lastUpdatedUser,
                                                             LastExecutedOn = @lastExecutedOn
                                                             --site_id =@siteId
                                                           where ProgramScheduleId = @programScheduleId
                                                           SELECT* FROM ConcurrentProgramSchedules WHERE ProgramScheduleId = @programScheduleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(UpdateConcurrentProgramsScheduleQuery, GetSQLParameters(concurrentProgramsSchedule, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentProgramsScheduleDTO(concurrentProgramsSchedule, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating concurrentProgramsSchedule", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentProgramsSchedule);
            return concurrentProgramsSchedule;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="concurrentProgramSchedulesDTO">concurrentProgramSchedulesDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshConcurrentProgramsScheduleDTO(ConcurrentProgramSchedulesDTO concurrentProgramSchedulesDTO, DataTable dt)
        {
            log.LogMethodEntry(concurrentProgramSchedulesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                concurrentProgramSchedulesDTO.ProgramScheduleId = Convert.ToInt32(dt.Rows[0]["ProgramScheduleId"]);
                concurrentProgramSchedulesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                concurrentProgramSchedulesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                concurrentProgramSchedulesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                concurrentProgramSchedulesDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                concurrentProgramSchedulesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                concurrentProgramSchedulesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ConcurrentProgramSchedulesDTO class type
        /// </summary>
        /// <param name="concurrentProgramsScheduleDataRow">Concurrent DataRow</param>
        /// <returns>Returns ConcurrentProgramsSchedule</returns>
        private ConcurrentProgramSchedulesDTO GetConcurrentProgramsScheduleDTO(DataRow concurrentProgramsScheduleDataRow)
        {
            log.LogMethodEntry(concurrentProgramsScheduleDataRow);
            ConcurrentProgramSchedulesDTO ConcurrentProgramsScheduleDataObject = new ConcurrentProgramSchedulesDTO(Convert.ToInt32(concurrentProgramsScheduleDataRow["ProgramScheduleId"]),
                                                    concurrentProgramsScheduleDataRow["ProgramId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsScheduleDataRow["ProgramId"]),
                                                    concurrentProgramsScheduleDataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentProgramsScheduleDataRow["StartDate"]),
                                                    concurrentProgramsScheduleDataRow["RunAt"].ToString(),
                                                    concurrentProgramsScheduleDataRow["Frequency"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsScheduleDataRow["Frequency"]),
                                                    concurrentProgramsScheduleDataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentProgramsScheduleDataRow["EndDate"]),
                                                    concurrentProgramsScheduleDataRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramsScheduleDataRow["Active"]),
                                                    concurrentProgramsScheduleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsScheduleDataRow["site_id"]),
                                                    concurrentProgramsScheduleDataRow["Guid"].ToString(),
                                                    concurrentProgramsScheduleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramsScheduleDataRow["SynchStatus"]),
                                                    concurrentProgramsScheduleDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentProgramsScheduleDataRow["LastUpdatedDate"]),
                                                    concurrentProgramsScheduleDataRow["LastUpdatedUser"].ToString(),
                                                    concurrentProgramsScheduleDataRow["LastExecutedOn"].ToString(),
                                                    concurrentProgramsScheduleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsScheduleDataRow["MasterEntityId"]),
                                                    concurrentProgramsScheduleDataRow["CreatedBy"].ToString(),
                                                    concurrentProgramsScheduleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentProgramsScheduleDataRow["CreationDate"])
                                                    //concurrentProgramsScheduleDataRow["ConcurrentRequestId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsScheduleDataRow["ConcurrentRequestId"]),
                                                    //concurrentProgramsScheduleDataRow["ConcurrentRequestSetId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsScheduleDataRow["ConcurrentRequestSetId"])
                                                    );
            log.LogMethodExit(ConcurrentProgramsScheduleDataObject);
            return ConcurrentProgramsScheduleDataObject;
        }

        /// <summary>
        /// Gets the Concurrent Programs Schedule data of passed Program Id
        /// </summary>
        /// <param name="programId">integer type parameter</param>
        /// <returns>Returns ConcurrentProgramSchedulesDTO</returns>
        public ConcurrentProgramSchedulesDTO GetConcurrentProgramsSchedules(int programId)
        {
            log.LogMethodEntry(programId);
            ConcurrentProgramSchedulesDTO result = null;
            string selectConcurrentProgramsSchedulesQuery = SELECT_QUERY + @" WHERE cs.ProgramId = @programId";
            SqlParameter parameter = new SqlParameter("@programId", programId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectConcurrentProgramsSchedulesQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetConcurrentProgramsScheduleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the Concurrent Programs Schedule data of passed Program Schedule Id
        /// </summary>
        /// <param name="programScheduleId">integer type parameter</param>
        /// <returns>Returns ConcurrentProgramSchedulesDTO</returns>
        public ConcurrentProgramSchedulesDTO GetConProgramSchedule(int programScheduleId)
        {
            log.LogMethodEntry(programScheduleId);
            ConcurrentProgramSchedulesDTO result = null;
            string selectConProQuery = SELECT_QUERY + @" WHERE cs.ProgramScheduleId = @ProgramScheduleId";

            SqlParameter parameter = new SqlParameter("@ProgramScheduleId", programScheduleId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectConProQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetConcurrentProgramsScheduleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ConcurrentProgramSchedulesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ConcurrentProgramSchedulesDTO matching the search criteria</returns>
        public List<ConcurrentProgramSchedulesDTO> GetConcurrentProgramsSchedulesList(List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID
                            || searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_SCHEDULE_ID
                            //|| searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.CONCURRENT_SET_PROGRAM_ID
                            //|| searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.CONCURRENT_REQUEST_ID
                            || searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.RUNAT
                                  || searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.FREQUENCY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.START_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
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
                    counter++;
                }
                selectQuery = selectQuery + query + " Order by ProgramScheduleId";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                concurrentProgramSchedulesDTOList = new List<ConcurrentProgramSchedulesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ConcurrentProgramSchedulesDTO concurrentProgramSchedulesDTO = GetConcurrentProgramsScheduleDTO(dataRow);
                    concurrentProgramSchedulesDTOList.Add(concurrentProgramSchedulesDTO);
                }
            }
            log.LogMethodExit(concurrentProgramSchedulesDTOList);
            return concurrentProgramSchedulesDTOList;
        }
    }
}

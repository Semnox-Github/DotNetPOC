
/********************************************************************************************
 * Project Name - Concurrent Request Data Handler
 * Description  - Data handler of the Concurrent Requests class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016    Jeevan            Created
 *2.70.2        24-Jul-2019    Dakshakh raj      Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2      20-Nov-2019    Akshay G          Modified : GetSQLParameters()
 *                                             Added - GetTopConcurrentRequestsCompleted()
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                                                                       
 *2.90       26-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API., Added IsActive Column.
 *2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 *2.100       09-May-2020    Nitin Pai        Fix, included site id check
 *2.120.1       26-Apr-2021   Deeksha         Modified as part of AWS Concurrent Programs enhancements
 *2.140.6     29-May-2023    Deeksha          Modified as part of Aloha BSP Enhancements
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Requests DataHandler - Handles insert, update and select of ConcurrentRequests objects
    /// </summary>

    public class ConcurrentRequestsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM concurrentRequests as ccr ";

        enum Phase { Pending, Running, Complete };
        enum Status { Normal, Error, Aborted };

        /// <summary>
        /// Dictionary for searching Parameters for the ConcurrentRequests object.
        /// </summary>
        private static readonly Dictionary<ConcurrentRequestsDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<ConcurrentRequestsDTO.SearchByRequestParameters, string>
        {
              {ConcurrentRequestsDTO.SearchByRequestParameters.REQUEST_ID, "ccr.RequestId"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.STATUS, "ccr.Status"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.PHASE, "ccr.Phase"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.START_TIME, "ccr.StartTime"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.START_FROM_DATE, "ccr.StartTime"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID, "ccr.ProgramId"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID_LIST, "ccr.ProgramId"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.PROCESS_ID, "ccr.ProcessId"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "ccr.masterEntityId"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID, "ccr.site_id"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.IS_ACTIVE, "ccr.IsActive"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_NAME, "cp.ProgramName"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_EXECUTABLE_NAME, "cp.ExecutableName"},
              {ConcurrentRequestsDTO.SearchByRequestParameters.REQUEST_GUID, "ccr.Guid"}
             
              //{ConcurrentRequestsDTO.SearchByRequestParameters.CONCURRENT_REQUEST_SET_ID, "ccr.ConcurrentRequestSetProgramId"}
        };

        /// <summary>
        /// Default constructor of ConcurrentRequestsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentRequestsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// parameterized constructor of ReportScheduleDataHandler class
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentRequestsDataHandler(string connectionString, SqlTransaction sqlTransaction = null)
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
        /// Builds the SQL Parameter list used for inserting and updating ConcurrentPrograms parameters Record.
        /// </summary>
        /// <param name="concurrentProgramsDTO">concurrentProgramsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ConcurrentRequestsDTO concurrentRequestsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentRequestsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@RequestedTime", string.IsNullOrEmpty(concurrentRequestsDTO.RequestedTime) ? DateTime.MinValue : DateTime.Parse(concurrentRequestsDTO.RequestedTime)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@StartTime", string.IsNullOrEmpty(concurrentRequestsDTO.StartTime) ? DateTime.MinValue : DateTime.Parse(concurrentRequestsDTO.StartTime)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@ActualStartTime", string.IsNullOrEmpty(concurrentRequestsDTO.ActualStartTime) ? DateTime.Now : DateTime.Parse(concurrentRequestsDTO.ActualStartTime)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@EndTime", string.IsNullOrEmpty(concurrentRequestsDTO.EndTime) ? DateTime.MinValue : DateTime.Parse(concurrentRequestsDTO.EndTime)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@requestId", concurrentRequestsDTO.RequestId, true));
                parameters.Add(dataAccessHandler.GetSQLParameter("@programId", concurrentRequestsDTO.ProgramId, true));
                parameters.Add(dataAccessHandler.GetSQLParameter("@RequestedBy", concurrentRequestsDTO.RequestedBy));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Phase", concurrentRequestsDTO.Phase));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Status", concurrentRequestsDTO.Status));
                parameters.Add(dataAccessHandler.GetSQLParameter("@RelaunchOnExit", concurrentRequestsDTO.RelaunchOnExit));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument1", (concurrentRequestsDTO.Argument1 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument1)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument2", (concurrentRequestsDTO.Argument2 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument2)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument3", (concurrentRequestsDTO.Argument3 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument3)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument4", (concurrentRequestsDTO.Argument4 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument4)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument5", (concurrentRequestsDTO.Argument5 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument5)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument6", (concurrentRequestsDTO.Argument6 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument6)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument7", (concurrentRequestsDTO.Argument7 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument7)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument8", (concurrentRequestsDTO.Argument8 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument8)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument9", (concurrentRequestsDTO.Argument9 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument9)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@Argument10", (concurrentRequestsDTO.Argument10 == null ? DBNull.Value.ToString() : concurrentRequestsDTO.Argument10)));
                parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
                parameters.Add(dataAccessHandler.GetSQLParameter("@programScheduleId", concurrentRequestsDTO.ProgramScheduleId, true));
                parameters.Add(dataAccessHandler.GetSQLParameter("@errorCount", concurrentRequestsDTO.ErrorCount == -1 ? DBNull.Value : (object)concurrentRequestsDTO.ErrorCount));
                parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", concurrentRequestsDTO.MasterEntityId, true));
                parameters.Add(dataAccessHandler.GetSQLParameter("@ProcessId", concurrentRequestsDTO.ProcessId, true));
                parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
                parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedby", loginId));
                parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", concurrentRequestsDTO.IsActive));
                // parameters.Add(dataAccessHandler.GetSQLParameter("@ConcurrentRequestSetId", concurrentRequestsDTO.ConcurrentRequestSetId, true));
            }
            catch (Exception ex)
            {
                log.Debug("Date time conversion failed ");
                log.LogVariableState("concurrentRequestsDTO", concurrentRequestsDTO);
                log.Error(ex.Message);
                throw;
            }
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Concurrent Requests record to the database
        /// </summary>
        /// <param name="concurrentRequests">ConcurrentRequestsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ConcurrentRequestsDTO InsertConcurrentRequests(ConcurrentRequestsDTO concurrentRequests, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentRequests, loginId, siteId);
            string insertconcurrentQuery = @"INSERT INTO[dbo].[concurrentRequests] 
                                                        (                                                 
                                                             [ProgramId]
                                                            ,[ProgramScheduleId]
                                                            ,[RequestedTime]
                                                            ,[RequestedBy]
                                                            ,[StartTime]
                                                            ,[ActualStartTime]
                                                            ,[EndTime]
                                                            ,[Phase]
                                                            ,[Status]
                                                            ,[RelaunchOnExit]
                                                            ,[Argument1]
                                                            ,[Argument2]
                                                            ,[Argument3]
                                                            ,[Argument4]
                                                            ,[Argument5]
                                                            ,[Argument6]
                                                            ,[Argument7]
                                                            ,[Argument8]
                                                            ,[Argument9]
                                                            ,[Argument10]
                                                            ,[site_id]
                                                            ,[Guid]
                                                            ,[ProcessId]
                                                            ,[MasterEntityId]
                                                            ,[CreatedBy]
                                                            ,[CreationDate]
                                                            ,[LastUpdateDate]
                                                            ,[LastUpdatedBy]
                                                            ,[IsActive]
                                                            --,[ConcurrentRequestSetId]
                                                        ) 
                                                values 
                                                        (
                                                            @ProgramId,
                                                            @ProgramScheduleId,
                                                            @RequestedTime,
                                                            @RequestedBy,
                                                            @StartTime,
                                                            @ActualStartTime,
                                                            @EndTime,
                                                            @Phase,
                                                            @Status,
                                                            @RelaunchOnExit,
                                                            @Argument1,
                                                            @Argument2,
                                                            @Argument3,
                                                            @Argument4,
                                                            @Argument5,
                                                            @Argument6,
                                                            @Argument7,
                                                            @Argument8,
                                                            @Argument9,
                                                            @Argument10,
                                                            @siteId,
                                                            NEWID(), 
                                                            @ProcessId,
                                                            @masterEntityId,
                                                            @createdBy,
                                                            Getdate(),
                                                            Getdate(),
                                                            @createdBy,
                                                            @isActive
                                                            --@ConcurrentRequestSetId
                                                            )SELECT * FROM concurrentRequests WHERE RequestId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertconcurrentQuery, GetSQLParameters(concurrentRequests, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentRequestsDTO(concurrentRequests, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting concurrentRequests", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentRequests);
            return concurrentRequests;
        }

        /// <summary>
        /// Updates the Concurrent Requests record
        /// </summary>
        /// <param name="concurrentRequests">ConcurrentRequestsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ConcurrentRequestsDTO UpdateConcurrentRequests(ConcurrentRequestsDTO concurrentRequests, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentRequests, loginId, siteId);
            string updateConcurQuery = @"update ConcurrentRequests 
                                                set  
                                                    ActualStartTime = @ActualStartTime,
                                                    EndTime = @EndTime,                                                    
                                                    Phase = @Phase,
                                                    Status = @Status,
                                                    RelaunchOnExit = @RelaunchOnExit,
                                                    Argument1 = @Argument1,
                                                    Argument2 = @Argument2,
                                                    Argument3 = @Argument3,
                                                    Argument4 = @Argument4,
                                                    Argument5 = @Argument5,
                                                    Argument6 = @Argument6,
                                                    Argument7 = @Argument7,
                                                    Argument8 = @Argument8,
                                                    Argument9 = @Argument9,
                                                    Argument10 = @Argument10,
                                                    -- site_id =@siteId,
                                                    ProcessId=@ProcessId,
                                                    ErrorCount = @errorCount,
                                                    LastUpdateDate =Getdate(),
                                                    LastUpdatedBy =@lastUpdatedBy,
                                                    IsActive = @isActive
                                                    --ConcurrentRequestSetId=@ConcurrentRequestSetId
                                                    where RequestId = @RequestId
                                                    SELECT* FROM ConcurrentRequests WHERE  RequestId = @RequestId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateConcurQuery, GetSQLParameters(concurrentRequests, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentRequestsDTO(concurrentRequests, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating concurrentRequests", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentRequests);
            return concurrentRequests;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="concurrentRequestsDTO">concurrentRequestsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshConcurrentRequestsDTO(ConcurrentRequestsDTO concurrentRequestsDTO, DataTable dt)
        {
            log.LogMethodEntry(concurrentRequestsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                concurrentRequestsDTO.RequestId = Convert.ToInt32(dt.Rows[0]["RequestId"]);
                concurrentRequestsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                concurrentRequestsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                concurrentRequestsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                concurrentRequestsDTO.LastUpdatedUserId = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                concurrentRequestsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                concurrentRequestsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ConcurrentRequestsDataRow class type
        /// </summary>
        /// <param name="concurrentRequestDataRow">Concurrent DataRow</param>
        /// <returns>Returns ConRequestDataObject</returns>
        private ConcurrentRequestsDTO GetConcurrentRequestsDTO(DataRow concurrentRequestDataRow)
        {
            log.LogMethodEntry(concurrentRequestDataRow);
            ConcurrentRequestsDTO ConRequestDataObject = new ConcurrentRequestsDTO(
                                                        concurrentRequestDataRow["RequestId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentRequestDataRow["RequestId"]),
                                                        concurrentRequestDataRow["ProgramId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentRequestDataRow["ProgramId"]),
                                                        concurrentRequestDataRow["ProgramScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentRequestDataRow["ProgramScheduleId"]),
                                                        concurrentRequestDataRow["RequestedTime"] == DBNull.Value ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToDateTime(concurrentRequestDataRow["RequestedTime"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                                        concurrentRequestDataRow["RequestedBy"] == DBNull.Value ? null : concurrentRequestDataRow["RequestedBy"].ToString(),
                                                        concurrentRequestDataRow["StartTime"] == DBNull.Value ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToDateTime(concurrentRequestDataRow["StartTime"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                                        concurrentRequestDataRow["ActualStartTime"] == DBNull.Value ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToDateTime(concurrentRequestDataRow["ActualStartTime"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                                        concurrentRequestDataRow["EndTime"] == DBNull.Value ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToDateTime(concurrentRequestDataRow["EndTime"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                                        concurrentRequestDataRow["Phase"] == DBNull.Value ? null : concurrentRequestDataRow["Phase"].ToString(),
                                                        concurrentRequestDataRow["Status"] == DBNull.Value ? null : concurrentRequestDataRow["Status"].ToString(),
                                                        concurrentRequestDataRow["RelaunchOnExit"] == DBNull.Value ? false : Convert.ToBoolean(concurrentRequestDataRow["RelaunchOnExit"]),
                                                        concurrentRequestDataRow["Argument1"] == DBNull.Value ? null : concurrentRequestDataRow["Argument1"].ToString(),
                                                        concurrentRequestDataRow["Argument2"] == DBNull.Value ? null : concurrentRequestDataRow["Argument2"].ToString(),
                                                        concurrentRequestDataRow["Argument3"] == DBNull.Value ? null : concurrentRequestDataRow["Argument3"].ToString(),
                                                        concurrentRequestDataRow["Argument4"] == DBNull.Value ? null : concurrentRequestDataRow["Argument4"].ToString(),
                                                        concurrentRequestDataRow["Argument5"] == DBNull.Value ? null : concurrentRequestDataRow["Argument5"].ToString(),
                                                        concurrentRequestDataRow["Argument6"] == DBNull.Value ? null : concurrentRequestDataRow["Argument6"].ToString(),
                                                        concurrentRequestDataRow["Argument7"] == DBNull.Value ? null : concurrentRequestDataRow["Argument7"].ToString(),
                                                        concurrentRequestDataRow["Argument8"] == DBNull.Value ? null : concurrentRequestDataRow["Argument8"].ToString(),
                                                        concurrentRequestDataRow["Argument9"] == DBNull.Value ? null : concurrentRequestDataRow["Argument9"].ToString(),
                                                        concurrentRequestDataRow["Argument10"] == DBNull.Value ? null : concurrentRequestDataRow["Argument10"].ToString(),
                                                        concurrentRequestDataRow["ProcessId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentRequestDataRow["ProcessId"]),
                                                        concurrentRequestDataRow["ErrorCount"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentRequestDataRow["ErrorCount"]),
                                                        concurrentRequestDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentRequestDataRow["site_id"]),
                                                        concurrentRequestDataRow["Guid"] == DBNull.Value ? string.Empty : concurrentRequestDataRow["Guid"].ToString(),
                                                        concurrentRequestDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(concurrentRequestDataRow["SynchStatus"]),
                                                        concurrentRequestDataRow["masterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentRequestDataRow["masterEntityId"]),
                                                        concurrentRequestDataRow["createdBy"] == DBNull.Value ? string.Empty : concurrentRequestDataRow["createdBy"].ToString(),
                                                        concurrentRequestDataRow["creationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentRequestDataRow["creationDate"]),
                                                        concurrentRequestDataRow["lastUpdatedBy"] == DBNull.Value ? string.Empty : concurrentRequestDataRow["lastUpdatedBy"].ToString(),
                                                        concurrentRequestDataRow["lastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentRequestDataRow["lastUpdateDate"]),
                                                        concurrentRequestDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(concurrentRequestDataRow["IsActive"])
                                                    //concurrentRequestDataRow["ConcurrentRequestSetId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentRequestDataRow["ConcurrentRequestSetId"])
                                                    );
            log.LogMethodExit(ConRequestDataObject);
            return ConRequestDataObject;
        }

        /// <summary>
        /// Gets the Concurrent Requests data of passed Request Id
        /// </summary>
        /// <param name="RequestId">integer type parameter</param>
        /// <returns>Returns ConcurrentRequestsDTO</returns>
        public ConcurrentRequestsDTO GetConcurrentRequests(int RequestId)
        {
            log.LogMethodEntry(RequestId);
            ConcurrentRequestsDTO result = null;
            string selectConcurrentRequestQuery = SELECT_QUERY + @" WHERE ccr.RequestId = @RequestId";
            SqlParameter parameter = new SqlParameter("@RequestId", RequestId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectConcurrentRequestQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetConcurrentRequestsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ConcurrentRequestsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ConcurrentRequestsDTO matching the search criteria</returns>
        public List<ConcurrentRequestsDTO> GetConcurrentRequestsList(List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID
                             || searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.PROCESS_ID
                             || searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.REQUEST_ID
                             //|| searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.CONCURRENT_PROGRAM_SCHEDULE_ID
                             //|| searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.CONCURRENT_REQUEST_SET_ID
                             || searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.MASTER_ENTITY_ID
                             )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.PHASE
                                  || searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.STATUS
                                  || searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.REQUEST_GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.START_TIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.START_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.IS_ACTIVE) // column to be added
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_NAME ||
                            searchParameter.Key == ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_EXECUTABLE_NAME)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 FROM ConcurrentPrograms cp
                                                              where cp.ProgramId = ccr.ProgramId 
                                                               and " + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                           + ") ");
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
                selectQuery = selectQuery + query + " Order by ProgramId";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                concurrentRequestsDTOList = new List<ConcurrentRequestsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ConcurrentRequestsDTO concurrentRequestsDTO = GetConcurrentRequestsDTO(dataRow);
                    concurrentRequestsDTOList.Add(concurrentRequestsDTO);
                }
            }
            log.LogMethodExit(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }


        /// <summary>
        /// Method to get requests needs to be cleaned up 
        /// </summary>
        /// <returns>Returns Datatable</returns>
        public List<ConcurrentRequestsDTO> GetCleanupRequests()
        {
            log.LogMethodEntry();
            SqlParameter[] selectConcurrentRequestParameters = new SqlParameter[1];
            selectConcurrentRequestParameters[0] = new SqlParameter("@PhaseRunning", Phase.Running.ToString());
            string selectConcurrentRequestsQuery = SELECT_QUERY + @" WHERE ccr.Phase = @PhaseRunning";
            List<ConcurrentRequestsDTO> concurrentRequestsList = null;
            DataTable concurrentRequestsData = dataAccessHandler.executeSelectQuery(selectConcurrentRequestsQuery, selectConcurrentRequestParameters, sqlTransaction);
            if (concurrentRequestsData.Rows.Count > 0)
            {
                concurrentRequestsList = new List<ConcurrentRequestsDTO>();
                foreach (DataRow concurrentDataRow in concurrentRequestsData.Rows)
                {
                    ConcurrentRequestsDTO concurrentRequestsDataObject = GetConcurrentRequestsDTO(concurrentDataRow);
                    concurrentRequestsList.Add(concurrentRequestsDataObject);
                }
            }
            log.Debug(concurrentRequestsList);
            return concurrentRequestsList;
        }

        /// <summary>
        /// Gets the Concurrent Requests which are pending and scheduled to run 
        /// </summary>
        /// <returns>Returns Datatable</returns>
        public List<ConcurrentRequestsDTO> GetConcurrentRequestsScheduledToRun(int siteId)
        {
            log.LogMethodEntry();
            string selectConcurrentRequestQuery = @"select cr.* 
                                                          from ConcurrentRequests cr, ConcurrentPrograms cp
                                                          where isnull(startTime, getdate()) <= getdate()
                                                          and Phase = @Phase
                                                          and cp.Active = 1 
                                                          and cr.ProgramId = cp.ProgramId 
                                                          and (cr.site_id = @siteId or @siteId = -1)
                                                          and (cp.site_id = @siteId or @siteId = -1)
                                                          and cr.ProgramId not in (select ProgramId from ConcurrentRequests where Phase = 'Running')
                                                          order by RequestId ";
            List<ConcurrentRequestsDTO> concurrentRequestsList = null;
            SqlParameter[] selectConcurrentRequestParameters = new SqlParameter[2];
            selectConcurrentRequestParameters[0] = new SqlParameter("@Phase", "Pending");
            selectConcurrentRequestParameters[1] = new SqlParameter("@siteId", siteId.ToString());
            DataTable concurrentRequestsData = dataAccessHandler.executeSelectQuery(selectConcurrentRequestQuery, selectConcurrentRequestParameters, sqlTransaction);

            if (concurrentRequestsData.Rows.Count > 0)
            {
                concurrentRequestsList = new List<ConcurrentRequestsDTO>();
                foreach (DataRow concurrentDataRow in concurrentRequestsData.Rows)
                {
                    ConcurrentRequestsDTO concurrentRequestsDataObject = GetConcurrentRequestsDTO(concurrentDataRow);
                    concurrentRequestsList.Add(concurrentRequestsDataObject);
                }
            }
            log.Debug(concurrentRequestsList);
            return concurrentRequestsList;
        }

        /// <summary>
        /// Gets the Concurrent Requests which are running  
        /// </summary>
        /// <returns>Returns Datatable</returns>
        public DataTable GetConcurrentRequestsRunning(int siteId)
        {
            log.LogMethodEntry();
            string selectConcurrentRequestQuery = @"select top 500 cr.RequestId, ProgramName, Phase, Status, ExecutableName 
                                                            from ConcurrentRequests cr, ConcurrentPrograms cp
                                                            where isnull(startTime, getdate()) <= getdate()
                                                            and cr.ProgramId = cp.ProgramId 
                                                            and (cr.site_id = @siteId or @siteId = -1)
                                                            and (cp.site_id = @siteId or @siteId = -1)
                                                            order by case Phase when 'Running' then 0 else 1 end, RequestId desc ";

            List<SqlParameter> selectConcurrentRequestParameters = new List<SqlParameter>();
            selectConcurrentRequestParameters.Add(new SqlParameter("@siteId", siteId.ToString()));
            DataTable concurrentRequestsData = dataAccessHandler.executeSelectQuery(selectConcurrentRequestQuery, selectConcurrentRequestParameters.ToArray(), sqlTransaction);

            log.LogMethodExit(concurrentRequestsData);
            return concurrentRequestsData;
        }

        /// <summary>
        /// Method to checks schedule and Get programs schedule which are due for the current day
        /// </summary>
        /// <param name="IncludeSystemPrograms">IncludeSystemPrograms</param>
        /// <returns></returns>
        public List<ConcurrentRequestsDTO> BuildNewRequestDTOForProgramsDueForSystemPrograms(int siteId)
        {
            log.LogMethodEntry();
            int Hour = DateTime.Now.Hour;
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = new List<ConcurrentRequestsDTO>();
            List<SqlParameter> selectConcurrentRequestParameters = new List<SqlParameter>();

            string selectConcurrentRequestQuery = string.Empty;

            // Include system programs 
            selectConcurrentRequestQuery = @"select ProgramId, null as ProgramScheduleId , getdate() as StartTime , null requestId, getdate() as RequestedTime,null as RequestedBy,
													getdate() as ActualStartTime, null EndTime,'Pending' as Phase,'Normal' as Status,0 as RelaunchOnExit , null Argument1,
													null Argument2, null Argument3, null Argument4, null Argument5, null Argument6,null Argument7,null Argument8,
													null Argument9, null Argument10,null ProcessId,null ErrorCount,null site_id,NEWID() as Guid,null SynchStatus,
													null masterEntityId,null createdBy,null creationDate,null lastUpdatedBy,null lastUpdateDate, 1 as IsActive
                                                    from ConcurrentPrograms where (SystemProgram=1 or KeepRunning = 1) and active=1
                                                    and ( site_id = @site_Id or @site_Id = -1)
                                                     and ProgramId not in (select distinct ProgramId 
                                                                                    from ConcurrentRequests
                                                                                   where Phase='Pending' and ProgramScheduleId is null
                                                                                    and ( site_id = @site_Id or @site_Id = -1))";
            selectConcurrentRequestParameters.Add(new SqlParameter("@site_id", siteId));
            DataTable concurrentRequestsData = dataAccessHandler.executeSelectQuery(selectConcurrentRequestQuery, selectConcurrentRequestParameters.ToArray(), sqlTransaction);
            if (concurrentRequestsData.Rows.Count > 0)
            {
                for (int i = 0; i < concurrentRequestsData.Rows.Count; i++)
                {
                    ConcurrentRequestsDTO concurrentRequestsDTO = GetConcurrentRequestsDTO(concurrentRequestsData.Rows[i]);
                    concurrentRequestsDTOList.Add(concurrentRequestsDTO);
                }
            }
            log.LogMethodExit(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }

        /// <summary>
        /// Method to checks schedule and Get programs schedule which are due for the current day
        /// </summary>
        /// <param name="IncludeSystemPrograms">IncludeSystemPrograms</param>
        /// <returns></returns>
        public List<ConcurrentRequestsDTO> BuildNewRequestDTOForProgramsDueForSchedules(int siteId)
        {
            log.LogMethodEntry();
            int Hour = DateTime.Now.Hour;
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = new List<ConcurrentRequestsDTO>();
            List<SqlParameter> selectConcurrentRequestParameters = new List<SqlParameter>();

            string selectConcurrentRequestQuery = string.Empty;

            selectConcurrentRequestQuery = @"select ProgramId, 0 as ProgramScheduleId , getdate() as StartTime , null requestId, getdate() as RequestedTime,null as RequestedBy,
													getdate() as ActualStartTime, null EndTime,'Pending' as Phase,'Normal' as  Status,0 as RelaunchOnExit , null Argument1,
													null Argument2, null Argument3, null Argument4, null Argument5, null Argument6,null Argument7,null Argument8,
													null Argument9, null Argument10,null ProcessId,null ErrorCount,null site_id,NEWID() as Guid,null SynchStatus,
													null masterEntityId,null createdBy,null creationDate,null lastUpdatedBy,null lastUpdateDate, 1 as IsActive                                              
										    from ConcurrentPrograms where SystemProgram= 0 and KeepRunning = 1 and active=1
                                                     and ProgramId not in (select distinct ProgramId 
                                                                                    from ConcurrentRequests
                                                                                   where Phase='Pending' OR Phase = 'Running')
                                                    union all 
	                                                select cps.ProgramId,cps.ProgramScheduleId,
                                                      case when (frequency> 2000 and cps.LastExecutedOn is not null  and  CONVERT(date,dateadd(MINUTE, (frequency-2000), cps.LastExecutedOn))>= CONVERT(date,@Today)) then 
                                                          dateadd(MINUTE, (frequency-2000), cps.LastExecutedOn)								                        
                                                          else CONVERT(DATETIME, @Today) + CONVERT(DATETIME,RunAt) end as StartTime, --cps.ConcurrentRequestSetId as RequestSetId,
		                                                  null requestId, getdate() as RequestedTime,null as RequestedBy,
													                                                getdate() as ActualStartTime, null EndTime,'Pending' as Phase,'Normal' as  Status,0 as RelaunchOnExit , null Argument1,
													                                null Argument2, null Argument3, null Argument4, null Argument5, null Argument6,null Argument7,null Argument8,
													                                null Argument9, null Argument10,null ProcessId,null ErrorCount,null site_id,NEWID() as Guid,null SynchStatus,
													                                null masterEntityId,null createdBy,null creationDate,null lastUpdatedBy,null lastUpdateDate, 1 as IsActive

                                                         from 
                                                        ConcurrentPrograms cp,ConcurrentProgramSchedules cps
                                                                where cp.active = 1 and cps.active=1
                                                                and cp.ProgramId = cps.ProgramId
                                                                and RunAt <= cast(case when frequency> 2000 then '23:59' else @hour end as time)
                                                                and @Today between startdate and enddate
                                                                and (cps.LastExecutedOn < @Today or cps.LastExecutedOn is null or frequency > 2000) 
                                                                and (frequency = -1 or " + //daily
                                                                " frequency = @dayofweek or " + //weekly
                                                                " (frequency = 100 and @firstDayOfMonth = 'Y') or " + // monthly
                                                                " (frequency = 500 and @firstDayOfMonth = 'Y' and month(@Today)%3=1) or " + // monthly
                                                                " (frequency > 1000 and frequency - 1000 = @date) or " + // specific date
                                                                @" (frequency > 2000 and getdate()>=CONVERT(DATETIME, startdate) + CONVERT(DATETIME,RunAt) 
                                                                                and (cps.LastExecutedOn is null or DATEDIFF(MINUTE,dateadd(MINUTE, -1, cps.LastExecutedOn),getdate()) > frequency-2000 ))" + // Every Hour
                                                                ")" +
                                                                @" 
                                                                and ProgramScheduleId not in (select distinct ISNULL(ProgramScheduleId,-1)
                                                                                                from ConcurrentRequests
                                                                                               where Phase='Pending' )

                                                            and ( cps.site_id = @site_Id or @site_Id = -1)

                                                          order by StartTime";
            selectConcurrentRequestParameters.Add(new SqlParameter("@Today", DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            selectConcurrentRequestParameters.Add(new SqlParameter("@hour", DateTime.Now.ToShortTimeString()));
            selectConcurrentRequestParameters.Add(new SqlParameter("@date", DateTime.Now.Day));
            selectConcurrentRequestParameters.Add(new SqlParameter("@site_id", siteId));
            int dayofweek = -1;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }
            selectConcurrentRequestParameters.Add(new SqlParameter("@dayofweek", dayofweek));

            if (DateTime.Now.Day == 1) // first day of month
                selectConcurrentRequestParameters.Add(new SqlParameter("@firstDayOfMonth", "Y"));
            else
                selectConcurrentRequestParameters.Add(new SqlParameter("@firstDayOfMonth", "N"));

            if (selectConcurrentRequestParameters != null && selectConcurrentRequestParameters.Any())
            {
                foreach (SqlParameter item in selectConcurrentRequestParameters)
                {
                    log.Debug(item);
                    log.Debug(item.SqlValue);
                }
            }
            DataTable concurrentRequestsData = dataAccessHandler.executeSelectQuery(selectConcurrentRequestQuery, selectConcurrentRequestParameters.ToArray(), sqlTransaction);
            if (concurrentRequestsData.Rows.Count > 0)
            {
                for (int i = 0; i < concurrentRequestsData.Rows.Count; i++)
                {
                    ConcurrentRequestsDTO concurrentRequestsDTO = GetConcurrentRequestsDTO(concurrentRequestsData.Rows[i]);
                    concurrentRequestsDTOList.Add(concurrentRequestsDTO);
                }
            }
            log.LogMethodExit(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }

        /// <summary>
        /// Method to checks schedule and Get programs schedule which are due for the current day
        /// </summary>
        /// <param name="IncludeSystemPrograms">IncludeSystemPrograms</param>
        /// <returns></returns>
        public DataTable GetConcurrentProgramScheduleDue(bool IncludeSystemPrograms, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(IncludeSystemPrograms);
            int Hour = DateTime.Now.Hour;

            List<SqlParameter> selectConcurrentRequestParameters = new List<SqlParameter>();

            string selectConcurrentRequestQuery = string.Empty;

            if (IncludeSystemPrograms == true)
            {
                // Include system programs 
                selectConcurrentRequestQuery = @"select ProgramId, 0 as ProgramScheduleId , getdate() as StartTime 
                                                    from ConcurrentPrograms where (SystemProgram=1 or KeepRunning = 1) and active=1
                                                     and ProgramId not in (select distinct ProgramId 
                                                                                    from ConcurrentRequests
                                                                                   where Phase='Pending' and ProgramScheduleId = 0)
                                                    union all ";
            }
            else
            {
                selectConcurrentRequestQuery = @"select ProgramId, 0 as ProgramScheduleId , getdate() as StartTime 
                                                    from ConcurrentPrograms where SystemProgram= 0 and KeepRunning = 1 and active=1
                                                     and ProgramId not in (select distinct ProgramId 
                                                                                    from ConcurrentRequests
                                                                                   where Phase='Pending' OR Phase = 'Running')
                                                    union all ";
            }

            selectConcurrentRequestQuery += @"select cps.ProgramId,cps.ProgramScheduleId,
                                                    case when (frequency> 2000 and cps.LastExecutedOn is not null  and  CONVERT(date,dateadd(MINUTE, (frequency-2000), cps.LastExecutedOn))>= CONVERT(date,@Today)) then 
								                        dateadd(MINUTE, (frequency-2000), cps.LastExecutedOn)								                        
                                                        else CONVERT(DATETIME, @Today) + CONVERT(DATETIME,RunAt) end as StartTime
                                                    from 
                                                        ConcurrentPrograms cp,ConcurrentProgramSchedules cps
                                                    where cp.active = 1 and cps.active=1
                                                    and cp.ProgramId = cps.ProgramId
                                                    and RunAt <= cast(case when frequency> 2000 then '23:59' else @hour end as time)
                                                    and @Today between startdate and enddate
                                                    and (cps.LastExecutedOn < @Today or cps.LastExecutedOn is null or frequency > 2000) 
                                                    and (frequency = -1 or " + //daily
                                                    " frequency = @dayofweek or " + //weekly
                                                    " (frequency = 100 and @firstDayOfMonth = 'Y') or " + // monthly
                                                    " (frequency = 500 and @firstDayOfMonth = 'Y' and month(@Today)%3=1) or " + // monthly
                                                    " (frequency > 1000 and frequency - 1000 = @date) or " + // specific date
                                                    @" (frequency > 2000 and getdate()>=CONVERT(DATETIME, startdate) + CONVERT(DATETIME,RunAt) 
                                                                    and (cps.LastExecutedOn is null or DATEDIFF(MINUTE,dateadd(MINUTE, -1, cps.LastExecutedOn),getdate()) > frequency-2000 ))" + // Every Hour
                                                    ")" +
                                                    @" 
                                                    and ProgramScheduleId not in (select distinct ISNULL(ProgramScheduleId,-1)
                                                                                    from ConcurrentRequests
                                                                                   where Phase='Pending' )
                                                    order by 3 ";
            selectConcurrentRequestParameters.Add(new SqlParameter("@Today", DateTime.Now.Date));
            selectConcurrentRequestParameters.Add(new SqlParameter("@hour", DateTime.Now.ToShortTimeString()));
            selectConcurrentRequestParameters.Add(new SqlParameter("@date", DateTime.Now.Day));
            int dayofweek = -1;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }
            selectConcurrentRequestParameters.Add(new SqlParameter("@dayofweek", dayofweek));

            if (DateTime.Now.Day == 1) // first day of month
                selectConcurrentRequestParameters.Add(new SqlParameter("@firstDayOfMonth", "Y"));
            else
                selectConcurrentRequestParameters.Add(new SqlParameter("@firstDayOfMonth", "N"));

            DataTable concurrentRequestsData = dataAccessHandler.executeSelectQuery(selectConcurrentRequestQuery, selectConcurrentRequestParameters.ToArray(), sqlTransaction);

            log.LogMethodExit(concurrentRequestsData);
            return concurrentRequestsData;
        }
        /// <summary>
        /// Gets the top concurrentRequests row based on ProgramName and siteId
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ConcurrentRequestsDTO GetTopCompletedConcurrentRequest(int programId, int siteId)
        {
            log.LogMethodEntry(programId, siteId);
            ConcurrentRequestsDTO concurrentRequestsDTO = null;
            string selectConcurrentRequestQuery = @"select top 1 cr.* from ConcurrentRequests cr, ConcurrentPrograms cp
                                                            where isnull(startTime, getdate()) <= getdate()
                                                            and cr.Phase='Complete'
															and cr.Status='Normal'
															and cr.ProgramId=cp.ProgramId
															and ( cr.site_id = @site_Id or @site_Id = -1)
                                                            and cp.ProgramId = @programId
                                                            order by RequestId desc";

            SqlParameter[] selectConcurrentRequestParameters = new SqlParameter[2];
            selectConcurrentRequestParameters[0] = new SqlParameter("@programId", programId);
            selectConcurrentRequestParameters[1] = new SqlParameter("@site_Id", siteId);
            DataTable concurrentRequestsData = dataAccessHandler.executeSelectQuery(selectConcurrentRequestQuery, selectConcurrentRequestParameters, sqlTransaction);
            if (concurrentRequestsData.Rows.Count > 0)
            {
                concurrentRequestsDTO = GetConcurrentRequestsDTO(concurrentRequestsData.Rows[0]);
            }
            log.LogMethodExit(concurrentRequestsDTO);
            return concurrentRequestsDTO;
        }


        /// <summary>
        /// Gets the top concurrentRequests row based on ProgramName and siteId
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<ConcurrentRequestsDTO> GetTopCompletedConcurrentRequest(List<int> siteIdList)
        {
            log.LogMethodEntry(siteIdList);
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = new List<ConcurrentRequestsDTO>();
            string selectConcurrentRequestQuery = @"select top 1 cr.* from ConcurrentRequests cr, @siteIdList List,
                                                            ConcurrentPrograms cp
                                                            where isnull(startTime, getdate()) <= getdate()
                                                            and cr.Phase='Complete'
															and cr.Status='Normal'
															and cr.ProgramId=cp.ProgramId
															and ( cr.site_id = List.Id)
                                                            and cp.ProgramName = 'AlohaBSPInterface'
                                                            order by RequestId desc";
            DataTable table = dataAccessHandler.BatchSelect(selectConcurrentRequestQuery, "@siteIdList", siteIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                concurrentRequestsDTOList = table.Rows.Cast<DataRow>().Select(x => GetConcurrentRequestsDTO(x)).ToList();
            }
            log.LogMethodExit(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }


        public ConcurrentRequestsDTO GetTopCompletedRunningRequestsForBSP(int siteId)
        {
            ConcurrentRequestsDTO concurrentRequestsDTO = null;
            string selectConcurrentRequestQuery = @"select top 1 cr.* from ConcurrentRequests cr, ConcurrentPrograms cp
                                                            where isnull(startTime, getdate()) <= getdate()
                                                            --and cr.Phase='Running'
															--and cr.Status='Normal'
															and cp.ProgramName in('AlohaBSPInterface')
															and ( cr.site_id = @site_Id or @site_Id = -1)
                                                            order by RequestId desc";

            SqlParameter[] selectConcurrentRequestParameters = new SqlParameter[1];
            selectConcurrentRequestParameters[0] = new SqlParameter("@site_Id", siteId);
            DataTable concurrentRequestsData = dataAccessHandler.executeSelectQuery(selectConcurrentRequestQuery, selectConcurrentRequestParameters, sqlTransaction);
            if (concurrentRequestsData.Rows.Count > 0)
            {
                concurrentRequestsDTO = GetConcurrentRequestsDTO(concurrentRequestsData.Rows[0]);
            }
            log.LogMethodExit(concurrentRequestsDTO);
            return concurrentRequestsDTO;
        }

    }
}

/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for WorkShiftSchedule 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
  *2.100.0     31-Aug-2020   Mushahid Faizan   siteId, CreatedBy, LastUpdatedBy changes in GetSQLParameters().
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// WorkShiftSchedule Data Handler - Handles insert, update and select of WorkShiftSchedule objects
    /// </summary>
    public class WorkShiftScheduleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM WorkShiftSchedule as wss ";

        /// <summary>
        /// Dictionary for searching Parameters for the WorkShiftSchedule object.
        /// </summary>
        private static readonly Dictionary<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string> DBSearchParameters = new Dictionary<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>
        {
            { WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.WORK_SHIFT_SCHEDULE_ID,"wss.WorkShiftScheduleId"},
            { WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.WORK_SHIFT_ID,"wss.WorkShiftId"},
            { WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.WORK_SHIFT_ID_LIST,"wss.WorkShiftId"},
            { WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.SEQUENCE,"wss.Sequence"},
            { WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.START_TIME,"wss.StartTime"},
            { WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.SITE_ID,"wss.site_id"},
            { WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.IS_ACTIVE,"wss.IsActive"},
            { WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.MASTER_ENTITY_ID,"wss.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for WorkShiftScheduleDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public WorkShiftScheduleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating WorkShiftSchedule Record.
        /// </summary>
        /// <param name="workShiftScheduleDTO">WorkShiftScheduleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(WorkShiftScheduleDTO workShiftScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftScheduleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@WorkShiftScheduleId", workShiftScheduleDTO.WorkShiftScheduleId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@WorkShiftId", workShiftScheduleDTO.WorkShiftId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sequence", workShiftScheduleDTO.Sequence));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartTime", workShiftScheduleDTO.StartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", workShiftScheduleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", workShiftScheduleDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to WorkShiftScheduleDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of WorkShiftScheduleDTO</returns>
        private WorkShiftScheduleDTO GetWorkShiftScheduleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            WorkShiftScheduleDTO workShiftScheduleDTO = new WorkShiftScheduleDTO(dataRow["WorkShiftScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WorkShiftScheduleId"]),
                                                dataRow["WorkShiftId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WorkShiftId"]),
                                                dataRow["Sequence"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Sequence"]),
                                                dataRow["StartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartTime"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                );
            return workShiftScheduleDTO;
        }


        /// <summary>
        /// Gets the WorkShiftSchedule data of passed WorkShiftScheduleId 
        /// </summary>
        /// <param name="workShiftScheduleId">workShiftScheduleId is passed as parameter</param>
        /// <returns>Returns WorkShiftScheduleDTO</returns>
        public WorkShiftScheduleDTO GetWorkShiftSchedule(int workShiftScheduleId)
        {
            log.LogMethodEntry(workShiftScheduleId);
            WorkShiftScheduleDTO result = null;
            string query = SELECT_QUERY + @" WHERE wss.WorkShiftScheduleId = @WorkShiftScheduleId";
            SqlParameter parameter = new SqlParameter("@WorkShiftScheduleId", workShiftScheduleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetWorkShiftScheduleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="workShiftScheduleDTO">WorkShiftScheduleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
    
        private void RefreshWorkShiftScheduleDTO(WorkShiftScheduleDTO workShiftScheduleDTO, DataTable dt)
        {
            log.LogMethodEntry(workShiftScheduleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                workShiftScheduleDTO.WorkShiftScheduleId = Convert.ToInt32(dt.Rows[0]["WorkShiftScheduleId"]);
                workShiftScheduleDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                workShiftScheduleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                workShiftScheduleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                workShiftScheduleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                workShiftScheduleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                workShiftScheduleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
                                                
        /// <summary>
        ///  Inserts the record to the WorkShiftSchedule Table. 
        /// </summary>
        /// <param name="workShiftScheduleDTO">WorkShiftScheduleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated WorkShiftScheduleDTO</returns>
        public WorkShiftScheduleDTO Insert(WorkShiftScheduleDTO workShiftScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftScheduleDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[WorkShiftSchedule]
                            (
                            WorkShiftId,
                            Sequence,
                            StartTime,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate , IsActive
                            )
                            VALUES
                            (
                            @WorkShiftId,
                            @Sequence,
                            @StartTime,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(), @isActive
                            )
                            SELECT * FROM WorkShiftSchedule WHERE WorkShiftScheduleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(workShiftScheduleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWorkShiftScheduleDTO(workShiftScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting WorkShiftScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(workShiftScheduleDTO);
            return workShiftScheduleDTO;
        }

        /// <summary>
        /// Update the record in the WorkShiftSchedule Table. 
        /// </summary>
        /// <param name="workShiftScheduleDTO">WorkShiftScheduleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated WorkShiftScheduleDTO</returns>
        public WorkShiftScheduleDTO Update(WorkShiftScheduleDTO workShiftScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftScheduleDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[WorkShiftSchedule]
                             SET
                             WorkShiftId = @WorkShiftId,
                             Sequence = @Sequence,
                             StartTime = @StartTime,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE(),
                             IsActive = @isActive()
                             WHERE WorkShiftScheduleId = @WorkShiftScheduleId
                            SELECT * FROM WorkShiftSchedule WHERE WorkShiftScheduleId = @WorkShiftScheduleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(workShiftScheduleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWorkShiftScheduleDTO(workShiftScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(workShiftScheduleDTO);
            return workShiftScheduleDTO;
        }

        /// <summary>
        /// Returns the List of WorkShiftScheduleDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of WorkShiftScheduleDTO </returns>
        public List<WorkShiftScheduleDTO> GetWorkShiftScheduleDTOList(List<KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>> searchParameters,
                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<WorkShiftScheduleDTO> workShiftScheduleDTOList = new List<WorkShiftScheduleDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.WORK_SHIFT_SCHEDULE_ID ||
                            searchParameter.Key == WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.WORK_SHIFT_ID ||
                            searchParameter.Key == WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.SEQUENCE ||
                            searchParameter.Key == WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.WORK_SHIFT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.START_TIME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                    WorkShiftScheduleDTO workShiftScheduleDTO = GetWorkShiftScheduleDTO(dataRow);
                    workShiftScheduleDTOList.Add(workShiftScheduleDTO);
                }
            }
            log.LogMethodExit(workShiftScheduleDTOList);
            return workShiftScheduleDTOList;
        }
    }
}

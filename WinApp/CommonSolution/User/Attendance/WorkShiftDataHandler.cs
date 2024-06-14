/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for WorkShift 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
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
    /// WorkShift Data Handler - Handles insert, update and select of WorkShift objects
    /// </summary>
    public class WorkShiftDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM WorkShift as ws ";

        /// <summary>
        /// Dictionary for searching Parameters for the WorkShift object.
        /// </summary>
        private static readonly Dictionary<WorkShiftDTO.SearchByWorkShiftParameters, string> DBSearchParameters = new Dictionary<WorkShiftDTO.SearchByWorkShiftParameters, string>
        {
            { WorkShiftDTO.SearchByWorkShiftParameters.WORK_SHIFT_ID,"ws.WorkShiftId"},
            { WorkShiftDTO.SearchByWorkShiftParameters.NAME,"ws.Name"},
            { WorkShiftDTO.SearchByWorkShiftParameters.STARTDATE,"ws.StartDate"},
            { WorkShiftDTO.SearchByWorkShiftParameters.ENDDATE,"ws.EndDate"},
            { WorkShiftDTO.SearchByWorkShiftParameters.STATUS,"ws.Status"},
            { WorkShiftDTO.SearchByWorkShiftParameters.FREQUENCY,"ws.Frequency"},
            { WorkShiftDTO.SearchByWorkShiftParameters.SITE_ID,"ws.site_id"},
            { WorkShiftDTO.SearchByWorkShiftParameters.IS_ACTIVE,"ws.IsActive"},
            { WorkShiftDTO.SearchByWorkShiftParameters.MASTER_ENTITY_ID,"ws.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for WorkShiftDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public WorkShiftDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating WorkShift Record.
        /// </summary>
        /// <param name="workShiftDTO">WorkShiftDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(WorkShiftDTO workShiftDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@WorkShiftId", workShiftDTO.WorkShiftId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", workShiftDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartDate", workShiftDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Frequency", workShiftDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", workShiftDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndDate", workShiftDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@WeekSchedule", workShiftDTO.WeekSchedule));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", workShiftDTO.MasterEntityId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", workShiftDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to WorkShiftDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of WorkShiftDTO</returns>
        private WorkShiftDTO GetWorkShiftDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            WorkShiftDTO workShiftDTO = new WorkShiftDTO(dataRow["WorkShiftId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WorkShiftId"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartDate"]),
                                                dataRow["Frequency"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Frequency"]),
                                                dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                                dataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndDate"]),
                                                dataRow["WeekSchedule"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["WeekSchedule"]),
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
            return workShiftDTO;
        }

        /// <summary>
        /// Gets the WorkShift data of passed WorkShiftId 
        /// </summary>
        /// <param name="workShiftId">workShiftId of WorkShift passed as parameter</param>
        /// <returns>Returns WorkShiftDTO</returns>
        public WorkShiftDTO GetWorkShift(int workShiftId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(workShiftId);
            WorkShiftDTO result = null;
            string query = SELECT_QUERY + @" WHERE workshift.WorkShiftId = @WorkShiftId";
            SqlParameter parameter = new SqlParameter("@WorkShiftId", workShiftId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetWorkShiftDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="workShiftDTO">WorkShiftDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshWorkShiftDTO(WorkShiftDTO workShiftDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                workShiftDTO.WorkShiftId = Convert.ToInt32(dt.Rows[0]["WorkShiftId"]);
                workShiftDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                workShiftDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                workShiftDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                workShiftDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                workShiftDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                workShiftDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
                                               
        /// <summary>
        ///  Inserts the record to the WorkShift Table. 
        /// </summary>
        /// <param name="workShiftDTO">WorkShiftDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated WorkShiftDTO</returns>
        public WorkShiftDTO Insert(WorkShiftDTO workShiftDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[WorkShift]
                            (
                            Name,
                            StartDate,
                            Frequency,
                            Status,
                            EndDate,
                            WeekSchedule,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate,
                            IsActive
                            )
                            VALUES
                            (
                            @Name,
                            @StartDate,
                            @Frequency,
                            @Status,
                            @EndDate,
                            @WeekSchedule,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),@isActive    
                            )
                            SELECT * FROM WorkShift WHERE WorkShiftId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(workShiftDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWorkShiftDTO(workShiftDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting WorkShiftDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(workShiftDTO);
            return workShiftDTO;
        }

        /// <summary>
        /// Update the record in the WorkShift Table. 
        /// </summary>
        /// <param name="workShiftDTO">WorkShiftDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated WorkShiftDTO</returns>
        public WorkShiftDTO Update(WorkShiftDTO workShiftDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[WorkShift]
                             SET
                             Name = @Name,
                             StartDate = @StartDate,
                             Frequency = @Frequency,
                             Status = @Status,
                             EndDate = @EndDate,
                             WeekSchedule = @WeekSchedule,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE(),
                             IsActive = @isActive
                             WHERE WorkShiftId = @WorkShiftId
                            SELECT * FROM WorkShift WHERE WorkShiftId = @WorkShiftId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(workShiftDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWorkShiftDTO(workShiftDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating WorkShiftDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(workShiftDTO);
            return workShiftDTO;
        }

        /// <summary>
        /// Returns the List of WorkShiftDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of WorkShiftDTO </returns>
        public List<WorkShiftDTO> GetWorkShiftDTOList(List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>> searchParameters,
                                                        SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<WorkShiftDTO> workShiftDTOList = new List<WorkShiftDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == WorkShiftDTO.SearchByWorkShiftParameters.WORK_SHIFT_ID ||
                            searchParameter.Key == WorkShiftDTO.SearchByWorkShiftParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WorkShiftDTO.SearchByWorkShiftParameters.NAME || 
                                 searchParameter.Key == WorkShiftDTO.SearchByWorkShiftParameters.FREQUENCY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == WorkShiftDTO.SearchByWorkShiftParameters.STARTDATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == WorkShiftDTO.SearchByWorkShiftParameters.ENDDATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == WorkShiftDTO.SearchByWorkShiftParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WorkShiftDTO.SearchByWorkShiftParameters.IS_ACTIVE)
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
                    WorkShiftDTO workShiftDTO = GetWorkShiftDTO(dataRow);
                    workShiftDTOList.Add(workShiftDTO);
                }
            }
            log.LogMethodExit(workShiftDTOList);
            return workShiftDTOList;
        }

    }       
}

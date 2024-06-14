/********************************************************************************************
 * Project Name - Job Schedule Task Data Handler
 * Description  - Data handler of the Job Schedule Task class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        11-Mar-2019   Guru S A            Rename MaintenanceScheduleDTO as JobScheduleDTO
 *2.70.2        25-Jul-2019   Dakshakh Raj        Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix
 *2.70.2        25-Jul-2019   Guru S A            Waiver phase 2 changes
 *2.70.2        06-Dec-2019   Jinto Thomas            Removed siteid from update query  
*2.90         11-Aug-2020   Mushahid Faizan     Modified : Added GetJobScheduleTaskDTOList() and changed default isActive value to true.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Job Schedule Task Data Handler - Handles insert, update and select of Job Schedule Task data objects
    /// </summary>
    public class JobScheduleTasksDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM Maint_SchAssetTasks as msa ";

        /// <summary>
        /// Dictionary for searching Parameters for the ApplicationContent object.
        /// </summary>
        private static readonly Dictionary<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string> DBSearchParameters = new Dictionary<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>

        {
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_SCHEDULE_TASK_ID, "msa.MaintSchAssetTaskId"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_SCHEDULE_ID, "msa.MaintScheduleId"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.ASSET_GROUP_ID, "msa.AssetGroupId"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.ASSET_TYPE_ID, "msa.AssetTypeId"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.ASSET_ID, "msa.AssetID"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_TASK_GROUP_ID, "msa.MaintTaskGroupId"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_TASK_ID, "msa.MaintTaskId"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.IS_ACTIVE, "msa.IsActive"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.MASTER_ENTITY_ID, "msa.MasterEntityId"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.SITE_ID, "msa.site_id"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_ID, "msa.BookingId"},
                {JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_CHECK_LIST_ID, "msa.BookingCheckListId"}
            };
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTrx;
        /// <summary>
        /// Default constructor of JobScheduleTasksDataHandler class
        /// </summary>
        public JobScheduleTasksDataHandler(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            this.sqlTrx = sqlTrx;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating ApplicationContent Reecord.
        /// </summary>
        /// <param name="applicationContentDTO">applicationContentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(JobScheduleTasksDTO jobScheduleTasksDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobScheduleTasksDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintSchAssetTaskId", jobScheduleTasksDTO.JobScheduleTaskId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintScheduleId", jobScheduleTasksDTO.JobScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetGroupId", jobScheduleTasksDTO.AssetGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetTypeId", jobScheduleTasksDTO.AssetTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetID", jobScheduleTasksDTO.AssetID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintTaskGroupId", jobScheduleTasksDTO.JObTaskGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintTaskId", jobScheduleTasksDTO.JobTaskId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (jobScheduleTasksDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", jobScheduleTasksDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BookingId", jobScheduleTasksDTO.BookingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BookingCheckListId", jobScheduleTasksDTO.BookingCheckListId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Job Schedule Task record to the database
        /// </summary>
        /// <param name="jobScheduleTasksDTO">JobScheduleTasksDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public JobScheduleTasksDTO InsertJobScheduleTask(JobScheduleTasksDTO jobScheduleTasksDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobScheduleTasksDTO, loginId, siteId);
            string insertScheduleAssetTaskQuery = @"insert into Maint_SchAssetTasks 
                                                        (                                                           
                                                          MaintScheduleId,
                                                          AssetGroupId,
                                                          AssetTypeId,
                                                          AssetID,
                                                          MaintTaskGroupId,
                                                          MaintTaskId,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          Guid,
                                                          site_id, 
                                                          MasterEntityId,
                                                          BookingId,
                                                          BookingCheckListId
                                                        ) 
                                                values 
                                                        (
                                                          @maintScheduleId,
                                                          @assetGroupId,
                                                          @assetTypeId,
                                                          @assetID,
                                                          @maintTaskGroupId,
                                                          @maintTaskId,
                                                          @isActive,
                                                          @createdBy,
                                                          getdate(),
                                                          @lastUpdatedBy,
                                                          getdate(),
                                                          NEWID(),
                                                          @siteid, 
                                                          @masterEntityId,
                                                          @BookingId,
                                                          @BookingCheckListId
                                                        )SELECT * FROM Maint_SchAssetTasks WHERE MaintSchAssetTaskId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertScheduleAssetTaskQuery, GetSQLParameters(jobScheduleTasksDTO, loginId, siteId).ToArray(), sqlTrx);
                RefreshJobScheduleTasksDTO(jobScheduleTasksDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting jobScheduleTasksDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(jobScheduleTasksDTO);
            return jobScheduleTasksDTO;
        }


        /// <summary>
        /// Updates the job schedule task record
        /// </summary>
        /// <param name="jobScheduleTasksDTO">JobScheduleTasksDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public JobScheduleTasksDTO UpdateJobScheduleTask(JobScheduleTasksDTO jobScheduleTasksDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobScheduleTasksDTO, loginId, siteId);
            string updateScheduleAssetTaskQuery = @"update Maint_SchAssetTasks 
                                                        set MaintScheduleId=@maintScheduleId,
                                                            AssetGroupId=@assetGroupId,
                                                            AssetTypeId=@assetTypeId,
                                                            AssetID=@assetID,
                                                            MaintTaskGroupId=@maintTaskGroupId,
                                                            MaintTaskId=@maintTaskId,
                                                            IsActive=@isActive,                                                          
                                                            LastUpdatedBy=@lastUpdatedBy,
                                                            LastupdatedDate=GetDate(),
                                                            --site_id=@siteid, 
                                                            MasterEntityId=@masterEntityId,
                                                            BookingId = @BookingId,
                                                            BookingCheckListId = @BookingCheckListId
                                                       where MaintSchAssetTaskId = @maintSchAssetTaskId
                                                       SELECT* FROM Maint_SchAssetTasks WHERE MaintSchAssetTaskId = @maintSchAssetTaskId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateScheduleAssetTaskQuery, GetSQLParameters(jobScheduleTasksDTO, loginId, siteId).ToArray(), sqlTrx);
                RefreshJobScheduleTasksDTO(jobScheduleTasksDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating jobScheduleTasksDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(jobScheduleTasksDTO);
            return jobScheduleTasksDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="jobScheduleTasksDTO">jobScheduleTasksDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshJobScheduleTasksDTO(JobScheduleTasksDTO jobScheduleTasksDTO, DataTable dt)
        {
            log.LogMethodEntry(jobScheduleTasksDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                jobScheduleTasksDTO.JobScheduleTaskId = Convert.ToInt32(dt.Rows[0]["MaintSchAssetTaskId"]);
                jobScheduleTasksDTO.LastupdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                jobScheduleTasksDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                jobScheduleTasksDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                jobScheduleTasksDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                jobScheduleTasksDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                jobScheduleTasksDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to JobScheduleTasksDTO class type
        /// </summary>
        /// <param name="jobScheduleTasksDataRow">JobScheduleTasksDTO DataRow</param>
        /// <returns>Returns JobScheduleTasksDTO</returns>
        private JobScheduleTasksDTO GetJobScheduleTaskDTO(DataRow jobScheduleTasksDataRow)
        {
            log.LogMethodEntry(jobScheduleTasksDataRow);
            JobScheduleTasksDTO jobScheduleTasksDataObject = new JobScheduleTasksDTO(Convert.ToInt32(jobScheduleTasksDataRow["MaintSchAssetTaskId"]),
                                            jobScheduleTasksDataRow["MaintScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["MaintScheduleId"]),
                                            jobScheduleTasksDataRow["AssetGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["AssetGroupId"]),
                                            jobScheduleTasksDataRow["AssetTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["AssetTypeId"]),
                                            jobScheduleTasksDataRow["AssetID"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["AssetID"]),
                                            jobScheduleTasksDataRow["MaintTaskGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["MaintTaskGroupId"]),
                                            jobScheduleTasksDataRow["MaintTaskId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["MaintTaskId"]),
                                                jobScheduleTasksDataRow["IsActive"] == DBNull.Value ? true : (jobScheduleTasksDataRow["IsActive"].ToString() == "Y" ? true : false),
                                            jobScheduleTasksDataRow["CreatedBy"].ToString(),
                                            jobScheduleTasksDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobScheduleTasksDataRow["CreationDate"]),
                                            jobScheduleTasksDataRow["LastUpdatedBy"].ToString(),
                                            jobScheduleTasksDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobScheduleTasksDataRow["LastupdatedDate"]),
                                            jobScheduleTasksDataRow["Guid"].ToString(),
                                            jobScheduleTasksDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["site_id"]),
                                            jobScheduleTasksDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(jobScheduleTasksDataRow["SynchStatus"]),
                                            jobScheduleTasksDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["MasterEntityId"]),
                                            jobScheduleTasksDataRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["BookingId"]),
                                            jobScheduleTasksDataRow["BookingCheckListId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleTasksDataRow["BookingCheckListId"])
                                            );
            log.LogMethodExit(jobScheduleTasksDataObject);
            return jobScheduleTasksDataObject;
        }
        /// <summary>
        /// Gets the schedule data of passed Job Schedule Task Id
        /// </summary>
        /// <param name="jobScheduleTasksId">integer type parameter</param>
        /// <returns>Returns JobScheduleTasksDTO</returns>
        public JobScheduleTasksDTO GetJobScheduleTaskDTO(int jobScheduleTasksId)
        {
            log.LogMethodEntry(jobScheduleTasksId);
            string selectScheduleAssetTaskQuery = SELECT_QUERY + @" WHERE msa.MaintSchAssetTaskId = @maintSchAssetTaskId";
            SqlParameter[] selectScheduleAssetTaskParameters = new SqlParameter[1];
            selectScheduleAssetTaskParameters[0] = new SqlParameter("@maintSchAssetTaskId", jobScheduleTasksId);
            DataTable schedule = dataAccessHandler.executeSelectQuery(selectScheduleAssetTaskQuery, selectScheduleAssetTaskParameters, sqlTrx);
            JobScheduleTasksDTO jobScheduleTasksDTO = null;
            if (schedule.Rows.Count > 0)
            {
                DataRow jobScheduleTasksDTORow = schedule.Rows[0];
                jobScheduleTasksDTO = GetJobScheduleTaskDTO(jobScheduleTasksDTORow); 
            }
            log.LogMethodExit(jobScheduleTasksDTO);
            return jobScheduleTasksDTO;
        }

        /// <summary>
        /// Gets the JobScheduleTasksDTO List for jobSchedule Id List
        /// </summary>
        /// <param name="jobScheduleIdList">integer list parameter</param>
        /// <returns>Returns List of JobScheduleTasksDTO</returns>
        public List<JobScheduleTasksDTO> GetJobScheduleTaskDTOList(List<int> jobScheduleIdList, bool activeRecords)
        {
            log.LogMethodEntry(jobScheduleIdList);
            List<JobScheduleTasksDTO> list = new List<JobScheduleTasksDTO>();
            string query = @"SELECT Maint_SchAssetTasks.*
                            FROM Maint_SchAssetTasks, @jobScheduleIdList List
                            WHERE MaintScheduleId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@jobScheduleIdList", jobScheduleIdList, null, sqlTrx);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetJobScheduleTaskDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the JobScheduleTasksDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of JobScheduleTasksDTO matching the search criteria</returns>
        public List<JobScheduleTasksDTO> GetJobScheduleTaskDTOList(List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectScheduleAssetTaskQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = " ";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : string.Empty;
                        if (searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_SCHEDULE_ID 
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.ASSET_GROUP_ID
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.ASSET_ID 
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.ASSET_TYPE_ID
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_SCHEDULE_TASK_ID
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_ID
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_TASK_GROUP_ID                            
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_TASK_ID
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_CHECK_LIST_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                if (searchParameters.Count > 0)
                    selectScheduleAssetTaskQuery = selectScheduleAssetTaskQuery + query;
            }
            DataTable ScheduleAssetTaskData = dataAccessHandler.executeSelectQuery(selectScheduleAssetTaskQuery, parameters.ToArray(), sqlTrx);
            List<JobScheduleTasksDTO> jobScheduleTasksDTOList = null;
            if (ScheduleAssetTaskData.Rows.Count > 0)
            {
                jobScheduleTasksDTOList = new List<JobScheduleTasksDTO>();
                foreach (DataRow ScheduleAssetTaskDataRow in ScheduleAssetTaskData.Rows)
                {
                    JobScheduleTasksDTO jobScheduleTasksDTODataObject = GetJobScheduleTaskDTO(ScheduleAssetTaskDataRow);
                    jobScheduleTasksDTOList.Add(jobScheduleTasksDTODataObject);
                } 
            }
            log.LogMethodExit(jobScheduleTasksDTOList);
            return jobScheduleTasksDTOList;
        }
    }
}

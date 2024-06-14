/********************************************************************************************
 * Project Name - Job Task Group Data Handler
 * Description  - Data handler of the job task group class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        24-Dec-2015   Raghuveera     Created 
 *2.70        08-Mar-2019   Guru S A       Rename MaintenanceTaskGroupDataHandler as JobTaskGroupDataHandle
 *            29-May-2019   Jagan Mohan    Code merge from Development to WebManagementStudio
 *2.70.2        13-Nov-2019   Guru S A       Waiver phase 2 changes
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{

    /// <summary>
    /// Job Task Group Data Handler - Handles insert, update and select of Maintenance Task Group Data objects
    /// </summary>
    public class JobTaskGroupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string> DBSearchParameters = new Dictionary<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>
            {
                {JobTaskGroupDTO.SearchByJobTaskGroupParameters.JOB_TASK_GROUP_ID, "MaintTaskGroupId"},
                {JobTaskGroupDTO.SearchByJobTaskGroupParameters.TASK_GROUP_NAME, "TaskGroupName"},
                {JobTaskGroupDTO.SearchByJobTaskGroupParameters.IS_ACTIVE, "IsActive"},
                {JobTaskGroupDTO.SearchByJobTaskGroupParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, "site_id"},
                {JobTaskGroupDTO.SearchByJobTaskGroupParameters.HAS_ACTIVE_TASKS, ""}
            };
        private SqlTransaction sqlTransaction;
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of JobTaskGroupDataHandler class
        /// </summary>
        public JobTaskGroupDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the Job task group record to the database
        /// </summary>
        /// <param name="jobTaskGroup">JobTaskGroupDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertJobTaskGroup(JobTaskGroupDTO jobTaskGroup, string userId, int siteId)
        {
            log.LogMethodEntry(jobTaskGroup, userId, siteId);
            string insertMaintenanceTaskGroupQuery = @"insert into Maint_TaskGroups 
                                                        (
                                                        TaskGroupName,
                                                        MasterEntityId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id 
                                                        ) 
                                                values 
                                                        (
                                                        @taskGroupName,
                                                        @masterEntityId,
                                                        @isActive,
                                                        @createdBy,
                                                        Getdate(),
                                                        @lastUpdatedBy,
                                                        Getdate(),
                                                        Newid(),
                                                        @siteid 
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateMaintenanceTaskGroupParameters = new List<SqlParameter>();
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@taskGroupName", jobTaskGroup.TaskGroupName));
            if (jobTaskGroup.MasterEntityId == -1)
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@masterEntityId", jobTaskGroup.MasterEntityId));
            }
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@isActive", (jobTaskGroup.IsActive == true? "Y":"N")));
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@createdBy", userId));
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@siteId", siteId)); 
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertMaintenanceTaskGroupQuery, updateMaintenanceTaskGroupParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the job task group record
        /// </summary>
        /// <param name="jobTaskGroup">JobTaskGroupDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateJobTaskGroup(JobTaskGroupDTO jobTaskGroup, string userId, int siteId)
        {
            log.LogMethodEntry(jobTaskGroup, userId, siteId);
            string updateMaintenanceTaskGroupQuery = @"update Maint_TaskGroups 
                                                         set TaskGroupName = @taskGroupName,
                                                             MasterEntityId=@masterEntityId, 
                                                             IsActive = @isActive,
                                                             LastUpdatedBy = @lastUpdatedBy, 
                                                             LastupdatedDate = Getdate()
                                                             --site_id=@siteid 
                                                       where MaintTaskGroupId = @maintTaskGroupId";
            List<SqlParameter> updateMaintenanceTaskGroupParameters = new List<SqlParameter>();
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@maintTaskGroupId", jobTaskGroup.JobTaskGroupId));
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@taskGroupName", jobTaskGroup.TaskGroupName));
            if (jobTaskGroup.MasterEntityId == -1)
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@masterEntityId", jobTaskGroup.MasterEntityId));
            }
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@isActive", (jobTaskGroup.IsActive == true ? "Y" : "N")));
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@siteId", siteId));
            
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateMaintenanceTaskGroupQuery, updateMaintenanceTaskGroupParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to GetJobTaskGroupDTO class type
        /// </summary>
        /// <param name="jobTaskGroupDataRow">GetJobTaskGroupDTO DataRow</param>
        /// <returns>Returns JobTaskGroupDTO</returns>
        private JobTaskGroupDTO GetJobTaskGroupDTO(DataRow jobTaskGroupDataRow)
        {
            log.LogMethodEntry(jobTaskGroupDataRow);
            JobTaskGroupDTO jobTaskGroupDataObject = new JobTaskGroupDTO(Convert.ToInt32(jobTaskGroupDataRow["MaintTaskGroupId"]),
                                            jobTaskGroupDataRow["TaskGroupName"].ToString(),
                                            jobTaskGroupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(jobTaskGroupDataRow["MasterEntityId"]),
                                            jobTaskGroupDataRow["IsActive"] == DBNull.Value ? false :(jobTaskGroupDataRow["IsActive"].ToString() == "Y"? true: false),
                                            jobTaskGroupDataRow["CreatedBy"].ToString(),
                                            jobTaskGroupDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobTaskGroupDataRow["CreationDate"]),
                                            jobTaskGroupDataRow["LastUpdatedBy"].ToString(),
                                            jobTaskGroupDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobTaskGroupDataRow["LastupdatedDate"]),
                                            jobTaskGroupDataRow["Guid"].ToString(),
                                            jobTaskGroupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(jobTaskGroupDataRow["site_id"]),
                                            jobTaskGroupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(jobTaskGroupDataRow["SynchStatus"])
                                            );
            log.LogMethodExit(jobTaskGroupDataObject);
            return jobTaskGroupDataObject;
        }

        /// <summary>
        /// Gets the job task group data of passed job task group Id
        /// </summary>
        /// <param name="jobTaskGroupId">integer type parameter</param>
        /// <returns>Returns JobTaskGroupDTO</returns>
        public JobTaskGroupDTO GetJobTaskGroup(int jobTaskGroupId)
        {
            log.LogMethodEntry(jobTaskGroupId);
            string selectJobTaskGroupQuery = @"select *
                                                 from Maint_TaskGroups
                                                where MaintTaskGroupId = @maintTaskGroupId";
            SqlParameter[] selectJobTaskGroupParameters = new SqlParameter[1];
            selectJobTaskGroupParameters[0] = new SqlParameter("@maintTaskGroupId", jobTaskGroupId);
            DataTable jobTaskGroup = dataAccessHandler.executeSelectQuery(selectJobTaskGroupQuery, selectJobTaskGroupParameters, sqlTransaction);
            JobTaskGroupDTO jobTaskGroupDataObject = null;
            if (jobTaskGroup.Rows.Count > 0)
            {
                DataRow jobTaskGroupRow = jobTaskGroup.Rows[0];
                jobTaskGroupDataObject = GetJobTaskGroupDTO(jobTaskGroupRow); 
            }
            log.LogMethodExit(jobTaskGroupDataObject);
            return jobTaskGroupDataObject;
        }

        /// <summary>
        /// Gets the JobTaskGroupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of JobTaskGroupDTO matching the search criteria</returns>
        public List<JobTaskGroupDTO> GetJobTaskGroupList(List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectJobTaskGroupQuery = @"select *
                                                 from Maint_TaskGroups";
            if (searchParameters != null)
            {
                string joiner = " ";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == JobTaskGroupDTO.SearchByJobTaskGroupParameters.JOB_TASK_GROUP_ID 
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID
                            || searchParameter.Key == JobTaskGroupDTO.SearchByJobTaskGroupParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == JobTaskGroupDTO.SearchByJobTaskGroupParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')= " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "'Y'" : "'N'"));
                        }
                        else if (searchParameter.Key == JobTaskGroupDTO.SearchByJobTaskGroupParameters.HAS_ACTIVE_TASKS)
                        {
                            query.Append(joiner + @"  exists (SELECT 1 
                                                                from Maint_Tasks 
                                                               where Maint_Tasks.MaintTaskGroupId = Maint_TaskGroups.MaintTaskGroupId
                                                                 and Maint_Tasks.IsActive = 'Y')" );
                        }
                        else
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectJobTaskGroupQuery = selectJobTaskGroupQuery + query;
            }

            DataTable maintenanceTaskGroupData = dataAccessHandler.executeSelectQuery(selectJobTaskGroupQuery,null,sqlTransaction);
            List<JobTaskGroupDTO> maintenanceTaskGroupList = null;
            if (maintenanceTaskGroupData.Rows.Count > 0)
            {
                 maintenanceTaskGroupList = new List<JobTaskGroupDTO>();
                foreach (DataRow maintenanceTaskGroupDataRow in maintenanceTaskGroupData.Rows)
                {
                    JobTaskGroupDTO maintenanceTaskGroupDataObject = GetJobTaskGroupDTO(maintenanceTaskGroupDataRow);
                    maintenanceTaskGroupList.Add(maintenanceTaskGroupDataObject);
                } 
            }
            log.LogMethodExit(maintenanceTaskGroupList);
            return maintenanceTaskGroupList;
        }
    }
}

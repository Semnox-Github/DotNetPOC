/********************************************************************************************
 * Project Name - Schedule Asset Task Data Handler
 * Description  - Data handler of the Schedule Asset Task class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Schedule Asset Task Data Handler - Handles insert, update and select of Schedule Asset Task data objects
    /// </summary>
    public class ScheduleAssetTaskDataHandler
    {
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string> DBSearchParameters = new Dictionary<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>
            {
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_SCH_ASSET_TASK_ID, "MaintSchAssetTaskId"},
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_SCHEDULE_ID, "MaintScheduleId"},
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ASSET_GROUP_ID, "AssetGroupId"},
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ASSET_TYPE_ID, "AssetTypeId"},
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ASSET_ID, "AssetID"},
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_TASK_GROUP_ID, "MaintTaskGroupId"},
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_TASK_ID, "MaintTaskId"},
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ACTIVE_FLAG, "IsActive"},
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MASTER_ENTITY_ID, "MasterEntityId"},//Starts:Modification on 18-Jul-2016 for publish feature
                {ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.SITE_ID, "site_id"}//Ends: Modification on 18-Jul-2016 for publish feature

            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ScheduleAssetTaskDataHandler class
        /// </summary>
        public ScheduleAssetTaskDataHandler()
        {
            log.Debug("Starts-ScheduleAssetTaskDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-ScheduleAssetTaskDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the Schedule Asset Task record to the database
        /// </summary>
        /// <param name="scheduleAssetTask">ScheduleAssetTaskDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertScheduleAssetTask(ScheduleAssetTaskDTO scheduleAssetTask, string userId, int siteId)
        {
            log.Debug("Starts-InsertScheduleAssetTask(scheduleAssetTask, userId, siteId) Method.");
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
                                                          SynchStatus,
                                                          MasterEntityId
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
                                                          @synchStatus,
                                                          @masterEntityId
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateScheduleAssetTaskParameters = new List<SqlParameter>();
            if (scheduleAssetTask.MaintScheduleId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintScheduleId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintScheduleId", scheduleAssetTask.MaintScheduleId));
            }
            if (scheduleAssetTask.AssetGroupId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetGroupId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetGroupId", scheduleAssetTask.AssetGroupId));
            }
            if (scheduleAssetTask.AssetTypeId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetTypeId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetTypeId", scheduleAssetTask.AssetTypeId));
            }
            if (scheduleAssetTask.AssetID == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetID", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetID", scheduleAssetTask.AssetID));
            }

            if (scheduleAssetTask.MaintTaskGroupId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintTaskGroupId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintTaskGroupId", scheduleAssetTask.MaintTaskGroupId));
            }
            if (scheduleAssetTask.MaintTaskId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintTaskId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintTaskId", scheduleAssetTask.MaintTaskId));
            }
            updateScheduleAssetTaskParameters.Add(new SqlParameter("@isActive", (scheduleAssetTask.IsActive) ? "Y" : "N"));
            updateScheduleAssetTaskParameters.Add(new SqlParameter("@createdBy", userId));
            updateScheduleAssetTaskParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@siteId", siteId));
            if (scheduleAssetTask.SynchStatus)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@synchStatus", scheduleAssetTask.SynchStatus));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            if (scheduleAssetTask.MasterEntityId == -1)//starts:Modification on 18-Jul-2016 for publish feature
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@masterEntityId", scheduleAssetTask.MasterEntityId));
            }//Ends:Modification on 18-Jul-2016 for publish feature
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertScheduleAssetTaskQuery, updateScheduleAssetTaskParameters.ToArray());
            log.Debug("Ends-InsertScheduleAssetTask(scheduleAssetTask, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the maintenance task record
        /// </summary>
        /// <param name="scheduleAssetTask">ScheduleAssetTaskDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateScheduleAssetTask(ScheduleAssetTaskDTO scheduleAssetTask, string userId, int siteId)
        {
            log.Debug("Starts-UpdateScheduleAssetTask(scheduleAssetTask, userId, siteId) Method.");
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
                                                          SynchStatus=@synchStatus,
                                                          MasterEntityId=@masterEntityId
                                       where MaintSchAssetTaskId = @maintSchAssetTaskId";
            List<SqlParameter> updateScheduleAssetTaskParameters = new List<SqlParameter>();
            updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintSchAssetTaskId", scheduleAssetTask.MaintSchAssetTaskId));
            if (scheduleAssetTask.MaintScheduleId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintScheduleId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintScheduleId", scheduleAssetTask.MaintScheduleId));
            }
            if (scheduleAssetTask.AssetGroupId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetGroupId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetGroupId", scheduleAssetTask.AssetGroupId));
            }
            if (scheduleAssetTask.AssetTypeId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetTypeId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetTypeId", scheduleAssetTask.AssetTypeId));
            }
            if (scheduleAssetTask.AssetID == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetID", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@assetID", scheduleAssetTask.AssetID));
            }
            if (scheduleAssetTask.MaintTaskGroupId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintTaskGroupId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintTaskGroupId", scheduleAssetTask.MaintTaskGroupId));
            }
            if (scheduleAssetTask.MaintTaskId == -1)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintTaskId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@maintTaskId", scheduleAssetTask.MaintTaskId));
            }
            updateScheduleAssetTaskParameters.Add(new SqlParameter("@isActive", (scheduleAssetTask.IsActive) ? "Y" : "N"));
            updateScheduleAssetTaskParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@siteId", siteId));
            if (scheduleAssetTask.SynchStatus)
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@synchStatus", scheduleAssetTask.SynchStatus));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            if (scheduleAssetTask.MasterEntityId == -1)//starts:Modification on 18-Jul-2016 for publish feature
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateScheduleAssetTaskParameters.Add(new SqlParameter("@masterEntityId", scheduleAssetTask.MasterEntityId));
            }//Ends:Modification on 18-Jul-2016 for publish feature
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateScheduleAssetTaskQuery, updateScheduleAssetTaskParameters.ToArray());
            log.Debug("Ends-UpdateScheduleAssetTask(scheduleAssetTask, userId, siteId) Method.");
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to ScheduleAssetTaskDTO class type
        /// </summary>
        /// <param name="scheduleAssetTaskDataRow">ScheduleAssetTaskDTO DataRow</param>
        /// <returns>Returns ScheduleAssetTaskDTO</returns>
        private ScheduleAssetTaskDTO GetScheduleAssetTaskDTO(DataRow scheduleAssetTaskDataRow)
        {
            log.Debug("Starts-GetScheduleAssetTaskDTO(scheduleAssetTaskDataRow) Method.");
            ScheduleAssetTaskDTO scheduleAssetTaskDataObject = new ScheduleAssetTaskDTO(Convert.ToInt32(scheduleAssetTaskDataRow["MaintSchAssetTaskId"]),
                                            scheduleAssetTaskDataRow["MaintScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleAssetTaskDataRow["MaintScheduleId"]),
                                            scheduleAssetTaskDataRow["AssetGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleAssetTaskDataRow["AssetGroupId"]),
                                            scheduleAssetTaskDataRow["AssetTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleAssetTaskDataRow["AssetTypeId"]),
                                            scheduleAssetTaskDataRow["AssetID"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleAssetTaskDataRow["AssetID"]),
                                            scheduleAssetTaskDataRow["MaintTaskGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleAssetTaskDataRow["MaintTaskGroupId"]),
                                            scheduleAssetTaskDataRow["MaintTaskId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleAssetTaskDataRow["MaintTaskId"]),
                                            scheduleAssetTaskDataRow["IsActive"].ToString(),
                                            scheduleAssetTaskDataRow["CreatedBy"].ToString(),
                                            scheduleAssetTaskDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleAssetTaskDataRow["CreationDate"]),
                                            scheduleAssetTaskDataRow["LastUpdatedBy"].ToString(),
                                            scheduleAssetTaskDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleAssetTaskDataRow["LastupdatedDate"]),
                                            scheduleAssetTaskDataRow["Guid"].ToString(),
                                            scheduleAssetTaskDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleAssetTaskDataRow["site_id"]),
                                            scheduleAssetTaskDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(scheduleAssetTaskDataRow["SynchStatus"]),
                                            scheduleAssetTaskDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleAssetTaskDataRow["MasterEntityId"])//Modification on 18-Jul-2016 for publish feature
                                            );
            log.Debug("Ends-GetScheduleAssetTaskDTO(scheduleAssetTaskDataRow) Method.");
            return scheduleAssetTaskDataObject;
        }
        /// <summary>
        /// Gets the schedule data of passed schedule asset task Id
        /// </summary>
        /// <param name="scheduleAssetTaskId">integer type parameter</param>
        /// <returns>Returns ScheduleAssetTaskDTO</returns>
        public ScheduleAssetTaskDTO GetScheduleAssetTask(int scheduleAssetTaskId)
        {
            log.Debug("Starts-GetScheduleAssetTask(ScheduleAssetTaskId) Method.");
            string selectScheduleAssetTaskQuery = @"select *
                                         from Maint_SchAssetTasks
                                        where MaintSchAssetTaskId = @maintSchAssetTaskId";
            SqlParameter[] selectScheduleAssetTaskParameters = new SqlParameter[1];
            selectScheduleAssetTaskParameters[0] = new SqlParameter("@maintSchAssetTaskId", scheduleAssetTaskId);
            DataTable schedule = dataAccessHandler.executeSelectQuery(selectScheduleAssetTaskQuery, selectScheduleAssetTaskParameters);
            if (schedule.Rows.Count > 0)
            {
                DataRow scheduleAssetTaskRow = schedule.Rows[0];
                ScheduleAssetTaskDTO scheduleAssetTaskDataObject = GetScheduleAssetTaskDTO(scheduleAssetTaskRow);
                log.Debug("Ends-GetScheduleAssetTask(scheduleAssetTaskId) Method by returnting scheduleAssetTaskDataObject.");
                return scheduleAssetTaskDataObject;
            }
            else
            {
                log.Debug("Ends-GetScheduleAssetTask(scheduleAssetTaskId) Method by returnting null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the ScheduleAssetTaskDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScheduleAssetTaskDTO matching the search criteria</returns>
        public List<ScheduleAssetTaskDTO> GetScheduleAssetTaskList(List<KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetScheduleAssetTaskList(searchParameters) Method.");
            int count = 0;
            string selectScheduleAssetTaskQuery = @"select *
                                         from Maint_SchAssetTasks";
            if (searchParameters != null)
            {
                string joiner = " ";//starts:Modification on 18-Jul-2016 for publish feature
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : " ";
                        if (searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_SCHEDULE_ID || searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ASSET_GROUP_ID
                            || searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ASSET_ID || searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ASSET_TYPE_ID
                            || searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_SCH_ASSET_TASK_ID || searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_TASK_GROUP_ID
                            || searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MASTER_ENTITY_ID || searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_TASK_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = '" + searchParameter.Value+"' ");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetScheduleAssetTaskList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectScheduleAssetTaskQuery = selectScheduleAssetTaskQuery + query;
            }
            DataTable ScheduleAssetTaskData = dataAccessHandler.executeSelectQuery(selectScheduleAssetTaskQuery, null);
            if (ScheduleAssetTaskData.Rows.Count > 0)
            {
                List<ScheduleAssetTaskDTO> scheduleAssetTaskList = new List<ScheduleAssetTaskDTO>();
                foreach (DataRow ScheduleAssetTaskDataRow in ScheduleAssetTaskData.Rows)
                {
                    ScheduleAssetTaskDTO scheduleAssetTaskDataObject = GetScheduleAssetTaskDTO(ScheduleAssetTaskDataRow);
                    scheduleAssetTaskList.Add(scheduleAssetTaskDataObject);
                }
                log.Debug("Ends-GetScheduleAssetTaskList(searchParameters) Method by returning scheduleAssetTaskList.");
                return scheduleAssetTaskList;
            }
            else
            {
                log.Debug("Ends-ScheduleAssetTaskList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}

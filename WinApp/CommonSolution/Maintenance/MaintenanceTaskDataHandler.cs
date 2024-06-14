/********************************************************************************************
 * Project Name - Maintenance Task Data Handler
 * Description  - Data handler of the maintenance task class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera     Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera     Modified 
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
    /// Maintenance Task Data Handler - Handles insert, update and select of Maintenance Task Data objects
    /// </summary>
    public class MaintenanceTaskDataHandler
    {
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string> DBSearchParameters = new Dictionary<MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>
            {                
                {MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.TASK_NAME, "TaskName"},
                {MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MAINT_TASK_GROUP_ID, "MaintTaskGroupId"},
                {MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.ACTIVE_FLAG, "IsActive"},
                {MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MASTER_ENTITY_ID,"MasterEntityId"},//starts:Modification on 18-Jul-2016 for publish feature
                {MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.SITE_ID, "site_id"},
                {MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MAINT_TASK_ID, "MaintTaskId"}//Ends:Modification on 18-Jul-2016 for publish feature
            };
         DataAccessHandler  dataAccessHandler;

        /// <summary>
        /// Default constructor of MaintenanceTaskDataHandler class
        /// </summary>
        public MaintenanceTaskDataHandler()
        {
            log.Debug("Starts-MaintenanceTaskDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-MaintenanceTaskDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the maintenance task record to the database
        /// </summary>
        /// <param name="maintenanceTask">MaintenanceTaskDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertMaintenanceTask(MaintenanceTaskDTO maintenanceTask, string userId, int siteId)
        {
            log.Debug("Starts-InsertMaintenanceTask(maintenanceTask, userId, siteId) Method.");
            string insertMaintenanceTaskQuery = @"insert into Maint_Tasks 
                                                        (                                                         
                                                        TaskName,
                                                        MaintTaskGroupId,
                                                        ValidateTag,
                                                        CardNumber,
                                                        CardId,
                                                        RemarksMandatory,
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
                                                        @taskName,
                                                        @maintTaskGroupId,
                                                        @validateTag,
                                                        @cardNumber,
                                                        @cardId,
                                                        @remarksMandatory,
                                                        @isActive,
                                                        @createdBy,
                                                        Getdate(),
                                                        @updatedBy,
                                                        Getdate(),                                                        
                                                        Newid(),
                                                        @siteid,
                                                        @synchStatus,
                                                        @masterEntityId
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateMaintenanceTaskParameters = new List<SqlParameter>();
            updateMaintenanceTaskParameters.Add(new SqlParameter("@taskName", maintenanceTask.TaskName));
            if (maintenanceTask.MaintTaskGroupId == -1)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@maintTaskGroupId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@maintTaskGroupId", maintenanceTask.MaintTaskGroupId));
            }
            updateMaintenanceTaskParameters.Add(new SqlParameter("@validateTag", maintenanceTask.ValidateTag));
            if (string.IsNullOrEmpty(maintenanceTask.CardNumber))
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@cardNumber", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@cardNumber", maintenanceTask.CardNumber));
            }
            if (maintenanceTask.CardId == -1)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@cardId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@cardId", maintenanceTask.CardId));
            }
            updateMaintenanceTaskParameters.Add(new SqlParameter("@remarksMandatory", maintenanceTask.RemarksMandatory));
            updateMaintenanceTaskParameters.Add(new SqlParameter("@isActive", maintenanceTask.IsActive));
            updateMaintenanceTaskParameters.Add(new SqlParameter("@createdBy", userId));
            updateMaintenanceTaskParameters.Add(new SqlParameter("@updatedBy", userId));
            if (siteId == -1)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@siteid",  DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (maintenanceTask.MasterEntityId == -1)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@masterEntityId", maintenanceTask.MasterEntityId));
            }
            if (maintenanceTask.SynchStatus)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@synchStatus", maintenanceTask.SynchStatus));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }            
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertMaintenanceTaskQuery, updateMaintenanceTaskParameters.ToArray());
            log.Debug("Ends-InsertMaintenanceTask(maintenanceTask, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the maintenance task record
        /// </summary>
        /// <param name="maintenanceTask">MaintenanceTaskDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateMaintenanceTask(MaintenanceTaskDTO maintenanceTask, string userId, int siteId)
        {
            log.Debug("Starts-UpdateMaintenanceTask(maintenanceTask, userId, siteId) Method.");
            string updateMaintenanceTaskQuery = @"update Maint_Tasks 
                                         set TaskName = @taskName,
                                             MaintTaskGroupId=@maintTaskGroupId,
                                             ValidateTag=@validateTag,
                                             CardNumber=@cardNumber,
                                             CardId=@cardId,
                                             RemarksMandatory=@remarksMandatory,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             SynchStatus = @synchStatus,
                                             MasterEntityId=@masterEntityId
                                       where MaintTaskId = @maintTaskId";
            List<SqlParameter> updateMaintenanceTaskParameters = new List<SqlParameter>();
            updateMaintenanceTaskParameters.Add(new SqlParameter("@maintTaskId", maintenanceTask.MaintTaskId));
            updateMaintenanceTaskParameters.Add(new SqlParameter("@taskName", maintenanceTask.TaskName));
            if (maintenanceTask.MaintTaskGroupId == -1)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@maintTaskGroupId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@maintTaskGroupId", maintenanceTask.MaintTaskGroupId));
            }
            updateMaintenanceTaskParameters.Add(new SqlParameter("@validateTag", maintenanceTask.ValidateTag));
            if (string.IsNullOrEmpty(maintenanceTask.CardNumber))
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@cardNumber", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@cardNumber", maintenanceTask.CardNumber));
            }
            if (maintenanceTask.CardId == -1)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@cardId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@cardId", maintenanceTask.CardId));
            }
            updateMaintenanceTaskParameters.Add(new SqlParameter("@remarksMandatory", maintenanceTask.RemarksMandatory));
            updateMaintenanceTaskParameters.Add(new SqlParameter("@isActive", maintenanceTask.IsActive));
            
            updateMaintenanceTaskParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateMaintenanceTaskParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateMaintenanceTaskParameters.Add(new SqlParameter("@siteId", siteId));
            if (maintenanceTask.MasterEntityId == -1)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@masterEntityId", maintenanceTask.MasterEntityId));
            }
            if (maintenanceTask.SynchStatus)
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@synchStatus", maintenanceTask.SynchStatus));
            }
            else
            {
                updateMaintenanceTaskParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateMaintenanceTaskQuery, updateMaintenanceTaskParameters.ToArray());
            log.Debug("Ends-UpdateMaintenanceTask(maintenanceTask, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to MaintenanceTaskDTO class type
        /// </summary>
        /// <param name="maintenanceTaskDataRow">MaintenanceTaskDTO DataRow</param>
        /// <returns>Returns MaintenanceTaskDTO</returns>
        private MaintenanceTaskDTO GetMaintenanceTaskDTO(DataRow maintenanceTaskDataRow)
        {
            log.Debug("Starts-GetMaintenanceTaskDTO(maintenanceTaskDataRow) Method.");
            MaintenanceTaskDTO maintenanceTaskDataObject = new MaintenanceTaskDTO(Convert.ToInt32(maintenanceTaskDataRow["MaintTaskId"]),
                                            maintenanceTaskDataRow["TaskName"].ToString(),
                                            maintenanceTaskDataRow["MaintTaskGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceTaskDataRow["MaintTaskGroupId"]),
                                            maintenanceTaskDataRow["ValidateTag"].ToString(),
                                            maintenanceTaskDataRow["CardNumber"] == DBNull.Value ? null : maintenanceTaskDataRow["CardNumber"].ToString(),                                            
                                            maintenanceTaskDataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceTaskDataRow["CardId"]),
                                            maintenanceTaskDataRow["RemarksMandatory"].ToString(),
                                            maintenanceTaskDataRow["IsActive"].ToString(),
                                            maintenanceTaskDataRow["CreatedBy"].ToString(),
                                            maintenanceTaskDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(maintenanceTaskDataRow["CreationDate"]),
                                            maintenanceTaskDataRow["LastUpdatedBy"].ToString(),
                                            maintenanceTaskDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(maintenanceTaskDataRow["LastupdatedDate"]),
                                            maintenanceTaskDataRow["Guid"].ToString(),
                                            maintenanceTaskDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceTaskDataRow["site_id"]),
                                            maintenanceTaskDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(maintenanceTaskDataRow["SynchStatus"]),
                                            maintenanceTaskDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceTaskDataRow["MasterEntityId"])//Modification on 18-Jul-2016 for adding publish to site
                                            );
            log.Debug("Ends-GetMaintenanceTaskDTO(maintenanceTaskDataRow) Method.");
            return maintenanceTaskDataObject;
        }

        /// <summary>
        /// Gets the maintenance task data of passed maintenance task Id
        /// </summary>
        /// <param name="MaintTaskId">integer type parameter</param>
        /// <returns>Returns MaintenanceTaskDTO</returns>
        public MaintenanceTaskDTO GetMaintenanceTask(int maintTaskId)
        {
            log.Debug("Starts-GetMaintenanceTask(maintTaskId) Method.");
            string selectMaintenanceTaskQuery = @"select *
                                         from Maint_Tasks
                                        where MaintTaskId = @maintTaskId";
            SqlParameter[] selectMaintenanceTaskParameters = new SqlParameter[1];
            selectMaintenanceTaskParameters[0] = new SqlParameter("@maintTaskId", maintTaskId);
            DataTable maintenanceTask = dataAccessHandler.executeSelectQuery(selectMaintenanceTaskQuery, selectMaintenanceTaskParameters);
            if (maintenanceTask.Rows.Count > 0)
            {
                DataRow maintenanceTaskRow = maintenanceTask.Rows[0];
                MaintenanceTaskDTO maintenanceTaskDataObject = GetMaintenanceTaskDTO(maintenanceTaskRow);
                log.Debug("Ends-GetMaintenanceTask(MaintTaskId) Method by returnting maintenanceTaskDataObject.");
                return maintenanceTaskDataObject;
            }
            else
            {
                log.Debug("Ends-GetMaintenanceTask(maintTaskId) Method by returnting null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the MaintenanceTaskDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MaintenanceTaskDTO matching the search criteria</returns>
        public List<MaintenanceTaskDTO> GetMaintenanceTaskList(List<KeyValuePair<MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetMaintenanceTaskList(searchParameters) Method.");
            int count = 0;
            string selectMaintenanceTaskQuery = @"select *
                                         from Maint_Tasks";
            if (searchParameters != null)
            {
                string joiner = " ";//starts:Modification on 18-Jul-2016 for publish feature
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : " ";

                        if (searchParameter.Key == MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MAINT_TASK_ID || searchParameter.Key == MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MASTER_ENTITY_ID || searchParameter.Key == MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MAINT_TASK_GROUP_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner +"Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }//Ends:Modification on 18-Jul-2016 for publish feature
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetMaintenanceTaskList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectMaintenanceTaskQuery = selectMaintenanceTaskQuery + query;
            }

            DataTable maintenanceTaskData = dataAccessHandler.executeSelectQuery(selectMaintenanceTaskQuery, null);
            if (maintenanceTaskData.Rows.Count > 0)
            {
                List<MaintenanceTaskDTO> maintenanceTaskList = new List<MaintenanceTaskDTO>();
                foreach (DataRow maintenanceTaskDataRow in maintenanceTaskData.Rows)
                {
                    MaintenanceTaskDTO maintenanceTaskDataObject = GetMaintenanceTaskDTO(maintenanceTaskDataRow);
                    maintenanceTaskList.Add(maintenanceTaskDataObject);
                }
                log.Debug("Ends-GetMaintenanceTaskList(searchParameters) Method by returning maintenanceTaskList.");
                return maintenanceTaskList;
            }
            else
            {
                log.Debug("Ends-GetMaintenanceTaskList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}

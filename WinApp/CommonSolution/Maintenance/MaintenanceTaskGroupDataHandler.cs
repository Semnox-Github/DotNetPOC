/********************************************************************************************
 * Project Name - Maintenance Task Group Data Handler
 * Description  - Data handler of the maintenance task group class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        24-Dec-2015   Raghuveera          Created 
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
    /// Maintenance Task Group Data Handler - Handles insert, update and select of Maintenance Task Group Data objects
    /// </summary>
    public class MaintenanceTaskGroupDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string> DBSearchParameters = new Dictionary<MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>
            {
                {MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MAINT_TASK_GROUP_ID, "MaintTaskGroupId"},
                {MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.TASK_GROUP_NAME, "TaskGroupName"},
                {MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.ACTIVE_FLAG, "IsActive"},
                {MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.SITE_ID, "site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of MaintenanceTaskGroupDataHandler class
        /// </summary>
        public MaintenanceTaskGroupDataHandler()
        {
            log.Debug("Starts-MaintenanceTaskGroupDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-MaintenanceTaskGroupDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the maintenance task group record to the database
        /// </summary>
        /// <param name="maintenanceTaskGroup">MaintenanceTaskGroupDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertMaintenanceTaskGroup(MaintenanceTaskGroupDTO maintenanceTaskGroup, string userId, int siteId)
        {
            log.Debug("Starts-InsertMaintenanceTaskGroup(maintenanceTaskGroup, userId, siteId) Method.");
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
                                                        site_id,
                                                        SynchStatus
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
                                                        @siteid,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateMaintenanceTaskGroupParameters = new List<SqlParameter>();
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@taskGroupName", maintenanceTaskGroup.TaskGroupName));
            if (maintenanceTaskGroup.MasterEntityId == -1)
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@masterEntityId", maintenanceTaskGroup.MasterEntityId));
            }
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@isActive", maintenanceTaskGroup.IsActive));
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@createdBy", userId));
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@siteId", siteId));
            if (maintenanceTaskGroup.SynchStatus)
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@synchStatus", maintenanceTaskGroup.SynchStatus));
            }
            else
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertMaintenanceTaskGroupQuery, updateMaintenanceTaskGroupParameters.ToArray());
            log.Debug("Ends-InsertMaintenanceTaskGroup(maintenanceTaskGroup, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the maintenance task group record
        /// </summary>
        /// <param name="maintenanceTaskGroup">MaintenanceTaskGroupDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateMaintenanceTaskGroup(MaintenanceTaskGroupDTO maintenanceTaskGroup, string userId, int siteId)
        {
            log.Debug("Starts-UpdateMaintenanceTaskGroup(maintenanceTaskGroup, userId, siteId) Method.");
            string updateMaintenanceTaskGroupQuery = @"update Maint_TaskGroups 
                                         set TaskGroupName = @taskGroupName,
                                             MasterEntityId=@masterEntityId,                                             
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where MaintTaskGroupId = @maintTaskGroupId";
            List<SqlParameter> updateMaintenanceTaskGroupParameters = new List<SqlParameter>();
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@maintTaskGroupId", maintenanceTaskGroup.MaintTaskGroupId));
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@taskGroupName", maintenanceTaskGroup.TaskGroupName));
            if (maintenanceTaskGroup.MasterEntityId == -1)
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@masterEntityId", maintenanceTaskGroup.MasterEntityId));
            }
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@isActive", maintenanceTaskGroup.IsActive));
            updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@siteId", siteId));
            if (maintenanceTaskGroup.SynchStatus)
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@synchStatus", maintenanceTaskGroup.SynchStatus));
            }
            else
            {
                updateMaintenanceTaskGroupParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateMaintenanceTaskGroupQuery, updateMaintenanceTaskGroupParameters.ToArray());
            log.Debug("Ends-UpdateMaintenanceTaskGroup(maintenanceTaskGroup, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to MaintenanceTaskGroupDTO class type
        /// </summary>
        /// <param name="maintenanceTaskGroupDataRow">MaintenanceTaskGroupDTO DataRow</param>
        /// <returns>Returns MaintenanceTaskGroupDTO</returns>
        private MaintenanceTaskGroupDTO GetMaintenanceTaskGroupDTO(DataRow maintenanceTaskGroupDataRow)
        {
            log.Debug("Starts-GetMaintenanceTaskGroupDTO(maintenanceTaskGroupDataRow) Method.");
            MaintenanceTaskGroupDTO maintenanceTaskGroupDataObject = new MaintenanceTaskGroupDTO(Convert.ToInt32(maintenanceTaskGroupDataRow["MaintTaskGroupId"]),
                                            maintenanceTaskGroupDataRow["TaskGroupName"].ToString(),
                                            maintenanceTaskGroupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceTaskGroupDataRow["MasterEntityId"]),
                                            maintenanceTaskGroupDataRow["IsActive"].ToString(),
                                            maintenanceTaskGroupDataRow["CreatedBy"].ToString(),
                                            maintenanceTaskGroupDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(maintenanceTaskGroupDataRow["CreationDate"]),
                                            maintenanceTaskGroupDataRow["LastUpdatedBy"].ToString(),
                                            maintenanceTaskGroupDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(maintenanceTaskGroupDataRow["LastupdatedDate"]),
                                            maintenanceTaskGroupDataRow["Guid"].ToString(),
                                            maintenanceTaskGroupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceTaskGroupDataRow["site_id"]),
                                            maintenanceTaskGroupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(maintenanceTaskGroupDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetMaintenanceTaskGroupDTO(maintenanceTaskGroupDataRow) Method.");
            return maintenanceTaskGroupDataObject;
        }

        /// <summary>
        /// Gets the maintenance task group data of passed maintenance task group Id
        /// </summary>
        /// <param name="MaintTaskId">integer type parameter</param>
        /// <returns>Returns MaintenanceTaskGroupDTO</returns>
        public MaintenanceTaskGroupDTO GetMaintenanceTaskGroup(int maintTaskGroupId)
        {
            log.Debug("Starts-GetMaintenanceTaskGroup(maintTaskGroupId) Method.");
            string selectMaintenanceTaskGroupQuery = @"select *
                                         from Maint_TaskGroups
                                        where MaintTaskGroupId = @maintTaskGroupId";
            SqlParameter[] selectMaintenanceTaskGroupParameters = new SqlParameter[1];
            selectMaintenanceTaskGroupParameters[0] = new SqlParameter("@maintTaskGroupId", maintTaskGroupId);
            DataTable maintenanceTaskGroup = dataAccessHandler.executeSelectQuery(selectMaintenanceTaskGroupQuery, selectMaintenanceTaskGroupParameters);
            if (maintenanceTaskGroup.Rows.Count > 0)
            {
                DataRow maintenanceTaskGroupRow = maintenanceTaskGroup.Rows[0];
                MaintenanceTaskGroupDTO maintenanceTaskGroupDataObject = GetMaintenanceTaskGroupDTO(maintenanceTaskGroupRow);
                log.Debug("Ends-GetMaintenanceTaskGroup(maintTaskGroupId) Method by returnting maintenanceTaskGroupDataObject.");
                return maintenanceTaskGroupDataObject;
            }
            else
            {
                log.Debug("Ends-GetMaintenanceTaskGroup(maintTaskGroupId) Method by returnting null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the MaintenanceTaskGroupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MaintenanceTaskGroupDTO matching the search criteria</returns>
        public List<MaintenanceTaskGroupDTO> GetMaintenanceTaskGroupList(List<KeyValuePair<MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetMaintenanceTaskGroupList(searchParameters) Method.");
            int count = 0;
            string selectMaintenanceTaskGroupQuery = @"select *
                                         from Maint_TaskGroups";
            if (searchParameters != null)
            {
                string joiner = " ";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MAINT_TASK_GROUP_ID || searchParameter.Key == MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetMaintenanceTaskGroupList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectMaintenanceTaskGroupQuery = selectMaintenanceTaskGroupQuery + query;
            }

            DataTable maintenanceTaskGroupData = dataAccessHandler.executeSelectQuery(selectMaintenanceTaskGroupQuery, null);
            if (maintenanceTaskGroupData.Rows.Count > 0)
            {
                List<MaintenanceTaskGroupDTO> maintenanceTaskGroupList = new List<MaintenanceTaskGroupDTO>();
                foreach (DataRow maintenanceTaskGroupDataRow in maintenanceTaskGroupData.Rows)
                {
                    MaintenanceTaskGroupDTO maintenanceTaskGroupDataObject = GetMaintenanceTaskGroupDTO(maintenanceTaskGroupDataRow);
                    maintenanceTaskGroupList.Add(maintenanceTaskGroupDataObject);
                }
                log.Debug("Ends-GetMaintenanceTaskGroupList(searchParameters) Method by returning maintenanceTaskGroupList.");
                return maintenanceTaskGroupList;
            }
            else
            {
                log.Debug("Ends-GetMaintenanceTaskGroupList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}

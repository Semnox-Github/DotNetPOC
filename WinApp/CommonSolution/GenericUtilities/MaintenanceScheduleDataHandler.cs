/********************************************************************************************
 * Project Name - Maintenance Schedule Data Handler
 * Description  - Data handler of the maintenance schedule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        30-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Maintenance Schedule Data Handler - Handles insert, update and select of maintenance schedule data objects
    /// </summary>
    public class MaintenanceScheduleDataHandler
    {
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string> DBSearchParameters = new Dictionary<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>
            {
                {MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.SCHEDULE_ID, "ScheduleId"},
                {MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.MAINT_SCHEDULE_ID, "MaintScheduleId"},
                {MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.ACTIVE_FLAG, "IsActive"},
                {MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.MASTER_ENTITY_ID, "MasterEntityId"},//Starts:Modification on 18-Jul-2016 for publish feature
                {MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.SITE_ID, "site_id"}//Ends: Modification on 18-Jul-2016 for publish feature
            };
         DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of MaintenanceTaskGroupDataHandler class
        /// </summary>
        public MaintenanceScheduleDataHandler()
        {
            log.Debug("Starts-MaintenanceScheduleDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-MaintenanceScheduleDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the maintenanceSchedule record to the database
        /// </summary>
        /// <param name="maintenanceSchedule">MaintenanceScheduleDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertMaintenanceSchedule(MaintenanceScheduleDTO maintenanceSchedule, string userId, int siteId)
        {
            log.Debug("Starts-InsertMaintenanceSchedule(maintenanceSchedule, userId, siteId) Method.");
            string insertMaintenanceScheduleQuery = @"insert into Maint_Schedule 
                                                        (
                                                          ScheduleId,
                                                          UserId,
                                                          DepartmentId,
                                                          DurationToComplete,
                                                          MaxValueJobCreated,
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
                                                          @scheduleId,
                                                          @userId,
                                                          @departmentId,
                                                          @durationToComplete,
                                                          @maxValueJobCreated,                                                          
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),                                                        
                                                          Newid(),
                                                          @siteid,
                                                          @synchStatus,
                                                          @masterEntityId
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateMaintenanceScheduleParameters = new List<SqlParameter>();
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@scheduleId", maintenanceSchedule.ScheduleId));
            if (maintenanceSchedule.UserId == -1)
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@userId", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@userId", maintenanceSchedule.UserId));
            }
            if (maintenanceSchedule.DepartmentId == -1)
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@departmentId", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@departmentId", maintenanceSchedule.DepartmentId));
            }
            if (maintenanceSchedule.DurationToComplete == -1)
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@durationToComplete", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@durationToComplete", maintenanceSchedule.DurationToComplete));
            }
            if (maintenanceSchedule.MaxValueJobCreated.Equals(DateTime.MinValue))
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@maxValueJobCreated", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@maxValueJobCreated", maintenanceSchedule.MaxValueJobCreated));
            }
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@isActive", maintenanceSchedule.IsActive));
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@createdBy", userId));
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@siteid", siteId));
            if (maintenanceSchedule.SynchStatus)
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@synchStatus", maintenanceSchedule.SynchStatus));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            if (maintenanceSchedule.MasterEntityId == -1)//starts:Modification on 18-Jul-2016 for publish feature
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@masterEntityId", maintenanceSchedule.MasterEntityId));
            }//Ends:Modification on 18-Jul-2016 for publish feature
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertMaintenanceScheduleQuery, updateMaintenanceScheduleParameters.ToArray());
            log.Debug("Ends-InsertMaintenanceSchedule(maintenanceSchedule, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the maintenance schedule record
        /// </summary>
        /// <param name="maintenanceSchedule">MaintenanceScheduleDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateMaintenanceSchedule(MaintenanceScheduleDTO maintenanceSchedule, string userId, int siteId)
        {
            log.Debug("Starts-UpdateMaintenanceSchedule(maintenanceSchedule, userId, siteId) Method.");
            string updateMaintenanceScheduleQuery = @"update Maint_Schedule 
                                         set ScheduleId=@scheduleId,
                                             UserId=@userId,
                                             DepartmentId=@departmentId,
                                             DurationToComplete=@durationToComplete,
                                             MaxValueJobCreated=@maxValueJobCreated,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             SynchStatus = @synchStatus,
                                             MasterEntityId=@masterEntityId
                                       where MaintScheduleId = @maintScheduleId";
            List<SqlParameter> updateMaintenanceScheduleParameters = new List<SqlParameter>();
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@maintScheduleId", maintenanceSchedule.MaintScheduleId));
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@scheduleId", maintenanceSchedule.ScheduleId));
            if (maintenanceSchedule.UserId == -1)
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@userId", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@userId", maintenanceSchedule.UserId));
            }
            if (maintenanceSchedule.DepartmentId == -1)
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@departmentId", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@departmentId", maintenanceSchedule.DepartmentId));
            }
            if (maintenanceSchedule.DurationToComplete == -1)
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@durationToComplete", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@durationToComplete", maintenanceSchedule.DurationToComplete));
            }
            if (maintenanceSchedule.MaxValueJobCreated.Equals(DateTime.MinValue))
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@maxValueJobCreated", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@maxValueJobCreated", maintenanceSchedule.MaxValueJobCreated));
            }
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@isActive", maintenanceSchedule.IsActive));
            updateMaintenanceScheduleParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@siteId", siteId));
            if (maintenanceSchedule.SynchStatus)
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@synchStatus", maintenanceSchedule.SynchStatus));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            if (maintenanceSchedule.MasterEntityId == -1)//starts:Modification on 18-Jul-2016 for publish feature
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceScheduleParameters.Add(new SqlParameter("@masterEntityId", maintenanceSchedule.MasterEntityId));
            }//Ends:Modification on 18-Jul-2016 for publish feature
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateMaintenanceScheduleQuery, updateMaintenanceScheduleParameters.ToArray());
            log.Debug("Ends-UpdateMaintenanceSchedule(maintenanceSchedule, userId, siteId) Method.");
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to MaintenanceScheduleDTO class type
        /// </summary>
        /// <param name="maintenanceScheduleDataRow">MaintenanceScheduleDTO DataRow</param>
        /// <returns>Returns MaintenanceScheduleDTO</returns>
        private MaintenanceScheduleDTO GetMaintenanceScheduleDTO(DataRow maintenanceScheduleDataRow)
        {
            log.Debug("Starts-GetMaintenanceScheduleDTO(maintenanceScheduleDataRow) Method.");
            MaintenanceScheduleDTO maintenanceScheduleDataObject = new MaintenanceScheduleDTO(Convert.ToInt32(maintenanceScheduleDataRow["MaintScheduleId"]),
                                            Convert.ToInt32(maintenanceScheduleDataRow["ScheduleId"]),
                                            Convert.ToInt32(maintenanceScheduleDataRow["UserId"]),
                                            maintenanceScheduleDataRow["DepartmentId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceScheduleDataRow["DepartmentId"]),
                                            maintenanceScheduleDataRow["DurationToComplete"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceScheduleDataRow["DurationToComplete"]),
                                            maintenanceScheduleDataRow["MaxValueJobCreated"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(maintenanceScheduleDataRow["MaxValueJobCreated"]),
                                            maintenanceScheduleDataRow["IsActive"].ToString(),
                                            maintenanceScheduleDataRow["CreatedBy"].ToString(),
                                            maintenanceScheduleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(maintenanceScheduleDataRow["CreationDate"]),
                                            maintenanceScheduleDataRow["LastUpdatedBy"].ToString(),
                                            maintenanceScheduleDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(maintenanceScheduleDataRow["LastupdatedDate"]),
                                            maintenanceScheduleDataRow["Guid"].ToString(),
                                            maintenanceScheduleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceScheduleDataRow["site_id"]),
                                            maintenanceScheduleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(maintenanceScheduleDataRow["SynchStatus"]),
                                            maintenanceScheduleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceScheduleDataRow["MasterEntityId"])//Modification on 18-Jul-2016 for publish feature
                                            );
            log.Debug("Ends-GetMaintenanceScheduleDTO(maintenanceScheduleDataRow) Method.");
            return maintenanceScheduleDataObject;
        }

        /// <summary>
        /// Gets the maintenance schedule data of passed schedule Id
        /// </summary>
        /// <param name="maintenanceScheduleId">integer type parameter</param>
        /// <returns>Returns MaintenanceScheduleDTO</returns>
        public MaintenanceScheduleDTO GetMaintenanceSchedule(int maintenanceScheduleId)
        {
            log.Debug("Starts-GetMaintenanceSchedule(maintenanceScheduleId) Method.");
            string selectMaintenanceScheduleQuery = @"select *
                                         from Maint_Schedule
                                        where MaintScheduleId = @maintScheduleId";
            SqlParameter[] selectMaintenanceScheduleParameters = new SqlParameter[1];
            selectMaintenanceScheduleParameters[0] = new SqlParameter("@maintScheduleId", maintenanceScheduleId);
            DataTable maintenanceSchedule = dataAccessHandler.executeSelectQuery(selectMaintenanceScheduleQuery, selectMaintenanceScheduleParameters);
            if (maintenanceSchedule.Rows.Count > 0)
            {
                DataRow maintenanceScheduleRow = maintenanceSchedule.Rows[0];
                MaintenanceScheduleDTO maintenanceScheduleDataObject = GetMaintenanceScheduleDTO(maintenanceScheduleRow);
                log.Debug("Ends-GetMaintenanceSchedule(maintenanceScheduleId) Method by returnting maintenanceScheduleDataObject.");
                return maintenanceScheduleDataObject;
            }
            else
            {
                log.Debug("Ends-GetMaintenanceSchedule(maintenanceScheduleId) Method by returnting null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the MaintenanceScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MaintenanceScheduleDTO matching the search criteria</returns>
        public List<MaintenanceScheduleDTO> GetMaintenanceScheduleList(List<KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetMaintenanceScheduleList(searchParameters) Method.");
            int count = 0;
            string selectMaintenanceScheduleQuery = @"select *
                                         from Maint_Schedule";
            if (searchParameters != null)
            {
                string joiner = " ";//starts:Modification on 18-Jul-2016 for publish feature
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : " ";
                        if (searchParameter.Key == MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.SCHEDULE_ID || searchParameter.Key == MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.MAINT_SCHEDULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " = -1)");
                        }
                        else if (searchParameter.Key == MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = '"+ searchParameter.Value +"' ");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }

                        count++;//Ends:Modification on 18-Jul-2016 for publish feature
                    }
                    else
                    {
                        log.Debug("Ends-GetMaintenanceScheduleList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if ((searchParameters != null) && (searchParameters.Count > 0))
                    selectMaintenanceScheduleQuery = selectMaintenanceScheduleQuery + query;
            }
            DataTable maintenanceScheduleData = dataAccessHandler.executeSelectQuery(selectMaintenanceScheduleQuery, null);
            if (maintenanceScheduleData.Rows.Count > 0)
            {
                List<MaintenanceScheduleDTO> maintenanceScheduleList = new List<MaintenanceScheduleDTO>();
                foreach (DataRow maintenanceScheduleDataRow in maintenanceScheduleData.Rows)
                {
                    MaintenanceScheduleDTO maintenanceScheduleDataObject = GetMaintenanceScheduleDTO(maintenanceScheduleDataRow);
                    maintenanceScheduleList.Add(maintenanceScheduleDataObject);
                }
                log.Debug("Ends-GetMaintenanceScheduleList(searchParameters) Method by returning maintenanceScheduleList.");
                return maintenanceScheduleList;
            }
            else
            {
                log.Debug("Ends-GetMaintenanceScheduleList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}

/********************************************************************************************
 * Project Name - Maintenance Job summary Data Handler
 * Description  - Data handler of the maintenance job class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Jan-2016   Raghuveera          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Maintenance Job summary Data Handler - Handles insert, update and select of Maintenance Job summary Data objects
    /// </summary>
    public class MaintenanceJobSummaryDataHandler
    {
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters, string> DBSearchParameters = new Dictionary<MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters, string>
            {                
                {MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters.JOB_NAME, "MaintJobName"},
                {MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters.MAINT_SCHEDULE_ID, "MaintScheduleId"},
                {MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters.ASSIGNED_TO, "AssignedTo"},
                {MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters.STATUS, "Status"},
                {MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters.ACTIVE_FLAG, "IsActive"}
            };
         DataAccessHandler  dataAccessHandler;

        /// <summary>
        /// Default constructor of MaintenanceJobSummaryDataHandler class
        /// </summary>
        public MaintenanceJobSummaryDataHandler()
        {
            log.Debug("Starts-MaintenanceJobSummaryDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-MaintenanceJobSummaryDataHandler() default constructor.");
        }
        
        
        /// <summary>
        /// Converts the Data row object to MaintenanceJobSummaryDTO class type
        /// </summary>
        /// <param name="maintenanceJobSummaryDataRow">MaintenanceJobSummaryDTO DataRow</param>
        /// <returns>Returns MaintenanceJobSummaryDTO</returns>
        private MaintenanceJobSummaryDTO GetMaintenanceJobSummaryDTO(DataRow maintenanceJobSummaryDataRow)
        {
            log.Debug("Starts-GetMaintenanceJobSummaryDTO(maintenanceJobSummaryDataRow) Method.");
            MaintenanceJobSummaryDTO maintenanceJobSummaryDataObject = new MaintenanceJobSummaryDTO(maintenanceJobSummaryDataRow["MaintScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobSummaryDataRow["MaintScheduleId"]),
                                            maintenanceJobSummaryDataRow["MaintJobName"].ToString(),
                                            maintenanceJobSummaryDataRow["ChklstScheduleTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(maintenanceJobSummaryDataRow["ChklstScheduleTime"]),
                                            maintenanceJobSummaryDataRow["AssignedTo"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobSummaryDataRow["AssignedTo"]),
                                            maintenanceJobSummaryDataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobSummaryDataRow["Status"]));
            log.Debug("Ends-GetMaintenanceJobSummaryDTO(maintenanceJobSummaryDataRow) Method.");
            return maintenanceJobSummaryDataObject;
        }

        
        /// <summary>
        /// select the list of users
        /// </summary>
        /// <returns>returns the data table</returns>
        public DataTable getUser()
        {
            DataTable dTable = new DataTable();
            string userQuery = @"select user_id,username from " +
                                "(select '-1' as user_id,'' as username " +
                                "union all " +
                                "select user_id,username from users usr " +
                                "join user_roles rol on usr.role_id=rol.role_id " +
                                "where rol.role<>'Semnox Admin' and rol.role<>'System Administrator' and usr.active_flag='Y' " +
                                ") as a  order by username";
            dTable = dataAccessHandler.executeSelectQuery(userQuery, null);
            return dTable;
        }
       
        /// <summary>
        /// select the list of status
        /// </summary>
        /// <param name="statusName">To select Specific status</param>
        /// <returns>returns the data table</returns>
        public DataTable getLookUpStatus(string statusName)
        {
            DataTable dTable = new DataTable();
            string statusQuery = @"select LookupValueId,Description from lookupvalues where ";
            if (!string.IsNullOrEmpty(statusName))
            {
                statusQuery += "Description='" + statusName + "' and ";
            }
            statusQuery += " lookupid=(select LookupId from lookups where LookupName='JOB_STATUS')";
            dTable = dataAccessHandler.executeSelectQuery(statusQuery, null);
            return dTable;
        }
        /// <summary>
        /// Gets the MaintenanceJobSummaryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MaintenanceJobSummaryDTO matching the search criteria</returns>
        public List<MaintenanceJobSummaryDTO> GetMaintenanceJobSummaryList(List<KeyValuePair<MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetMaintenanceJobSummaryList(searchParameters) Method.");
            int count = 0;
            string selectMaintenanceJobQuery = @"select MaintScheduleId,MaintJobName,ChklstScheduleTime,AssignedTo,Status from (select *
                                         from Maint_ChecklistDetails";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        else
                        query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetMaintenanceJobList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }
            selectMaintenanceJobQuery=selectMaintenanceJobQuery + ") as jobs Group by MaintScheduleId,MaintJobName,ChklstScheduleTime,AssignedTo,Status ";
            DataTable maintenanceJobSummaryData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, null);
            if (maintenanceJobSummaryData.Rows.Count > 0)
            {
                List<MaintenanceJobSummaryDTO> maintenanceJobSummaryList = new List<MaintenanceJobSummaryDTO>();
                foreach (DataRow maintenanceJobSummaryDataRow in maintenanceJobSummaryData.Rows)
                {
                    MaintenanceJobSummaryDTO maintenanceJobSummaryDataObject = GetMaintenanceJobSummaryDTO(maintenanceJobSummaryDataRow);
                    maintenanceJobSummaryList.Add(maintenanceJobSummaryDataObject);
                }
                log.Debug("Ends-GetMaintenanceJobSummaryList(searchParameters) Method by returning maintenanceJobSummaryList.");
                return maintenanceJobSummaryList;
            }
            else
            {
                log.Debug("Ends-GetMaintenanceJobSummaryList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}

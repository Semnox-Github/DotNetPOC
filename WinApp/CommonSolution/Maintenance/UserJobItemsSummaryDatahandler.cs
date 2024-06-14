/********************************************************************************************
 * Project Name - User Job Items summary Data Handler
 * Description  - Data handler of the  User Job Items class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Jan-2016   Raghuveera     Created 
 *2.70        08-Mar-2019   Guru S A       Renamed UserJobItemsSummaryDataHandler as UserJobItemsSummaryDataHandler
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
    /// User Job Items summary Data Handler - Handles insert, update and select of User Job Items summary Data objects
    /// </summary>
    public class UserJobItemsSummaryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters, string> DBSearchParameters = new Dictionary<UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters, string>
            {                
                {UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters.JOB_NAME, "MaintJobName"},
                {UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters.JOB_SCHEDULE_ID, "MaintScheduleId"},
                {UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters.ASSIGNED_TO, "AssignedTo"},
                {UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters.STATUS, "Status"},
                {UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters.IS_ACTIVE, "IsActive"}
            };
        private SqlTransaction sqlTransaction;
        DataAccessHandler  dataAccessHandler;

        /// <summary>
        /// Default constructor of UserJobItemsSummaryDataHandler class
        /// </summary>
        public UserJobItemsSummaryDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        
        
        /// <summary>
        /// Converts the Data row object to UserJobItemsSummaryDTO class type
        /// </summary>
        /// <param name="userJobItemsSummaryDataRow">UserJobItemsSummaryDTO DataRow</param>
        /// <returns>Returns UserJobItemsSummaryDTO</returns>
        private UserJobItemsSummaryDTO GetUserJobItemsSummaryDTO(DataRow userJobItemsSummaryDataRow)
        {
            log.LogMethodEntry(userJobItemsSummaryDataRow);
            UserJobItemsSummaryDTO maintenanceJobSummaryDataObject = new UserJobItemsSummaryDTO(userJobItemsSummaryDataRow["MaintScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsSummaryDataRow["MaintScheduleId"]),
                                            userJobItemsSummaryDataRow["MaintJobName"].ToString(),
                                            userJobItemsSummaryDataRow["ChklstScheduleTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userJobItemsSummaryDataRow["ChklstScheduleTime"]),
                                            userJobItemsSummaryDataRow["AssignedTo"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsSummaryDataRow["AssignedTo"]),
                                            userJobItemsSummaryDataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsSummaryDataRow["Status"]));
            log.LogMethodExit(maintenanceJobSummaryDataObject);
            return maintenanceJobSummaryDataObject;
        }

        
        /// <summary>
        /// select the list of users
        /// </summary>
        /// <returns>returns the data table</returns>
        public DataTable getUser()
        {
            log.LogMethodEntry();
            DataTable dTable = new DataTable();
            string userQuery = @"select user_id,username from " +
                                "(select '-1' as user_id,'' as username " +
                                "union all " +
                                "select user_id,username from users usr " +
                                "join user_roles rol on usr.role_id=rol.role_id " +
                                "where rol.role<>'Semnox Admin' and rol.role<>'System Administrator' and usr.active_flag='Y' " +
                                ") as a  order by username";
            dTable = dataAccessHandler.executeSelectQuery(userQuery, null, sqlTransaction);
            log.LogMethodExit(dTable);
            return dTable;
        }
       
        /// <summary>
        /// select the list of status
        /// </summary>
        /// <param name="statusName">To select Specific status</param>
        /// <returns>returns the data table</returns>
        public DataTable getLookUpStatus(string statusName)
        {
            log.LogMethodEntry(statusName);
            DataTable dTable = new DataTable();
            string statusQuery = @"select LookupValueId,Description from lookupvalues where ";
            if (!string.IsNullOrEmpty(statusName))
            {
                statusQuery += "Description='" + statusName + "' and ";
            }
            statusQuery += " lookupid=(select LookupId from lookups where LookupName='JOB_STATUS')";
            dTable = dataAccessHandler.executeSelectQuery(statusQuery, null, sqlTransaction);

            log.LogMethodExit(dTable);
            return dTable;
        }
        /// <summary>
        /// Gets the UserJobItemsSummaryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserJobItemsSummaryDTO matching the search criteria</returns>
        public List<UserJobItemsSummaryDTO> GetUserJobItemsSummaryList(List<KeyValuePair<UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectMaintenanceJobQuery = @"select MaintScheduleId,MaintJobName,ChklstScheduleTime,AssignedTo,Status from (select *
                                                                                                                                 from Maint_ChecklistDetails";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<UserJobItemsSummaryDTO.SearchByUserJobItemsSummaryParameters, string> searchParameter in searchParameters)
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
                        log.Debug("throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }
            selectMaintenanceJobQuery=selectMaintenanceJobQuery + ") as jobs Group by MaintScheduleId,MaintJobName,ChklstScheduleTime,AssignedTo,Status ";
            DataTable maintenanceJobSummaryData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, null);
            List<UserJobItemsSummaryDTO> userJobItemsSummaryDTOList = null;
            if (maintenanceJobSummaryData.Rows.Count > 0)
            {
                userJobItemsSummaryDTOList = new List<UserJobItemsSummaryDTO>();
                foreach (DataRow maintenanceJobSummaryDataRow in maintenanceJobSummaryData.Rows)
                {
                    UserJobItemsSummaryDTO maintenanceJobSummaryDataObject = GetUserJobItemsSummaryDTO(maintenanceJobSummaryDataRow);
                    userJobItemsSummaryDTOList.Add(maintenanceJobSummaryDataObject);
                } 
            }
            log.LogMethodExit(userJobItemsSummaryDTOList);
            return userJobItemsSummaryDTOList;
        }
    }
}

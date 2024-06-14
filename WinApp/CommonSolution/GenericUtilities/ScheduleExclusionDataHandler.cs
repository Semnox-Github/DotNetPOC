/********************************************************************************************
 * Project Name - Schedule Exclusion Data Handler
 * Description  - Data handler of the Schedule Exclusion class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Jan-2016   Raghuveera          Created 
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
    /// Schedule Exclusion Data Handler - Handles insert, update and select of schedule exclusion data objects
    /// </summary>
    public class ScheduleExclusionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string> DBSearchParameters = new Dictionary<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>
            {
                {ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SCHEDULE_EXCLUSION_ID, "ScheduleExclusionId"},
                {ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SCHEDULE_ID, "ScheduleId"},
                {ScheduleExclusionDTO.SearchByScheduleExclusionParameters.EXCLUSION_DATE, "ExclusionDate"},
                {ScheduleExclusionDTO.SearchByScheduleExclusionParameters.IS_ACTIVE, "IsActive"},
                {ScheduleExclusionDTO.SearchByScheduleExclusionParameters.MASTER_ENTITY_ID, "MasterEntityId"},//Starts:Modification on 18-Jul-2016 for publish feature
                {ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SITE_ID, "site_id"}//Ends: Modification on 18-Jul-2016 for publish feature
            };
         DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ScheduleExclusionDataHandler class
        /// </summary>
        public ScheduleExclusionDataHandler()
        {
            log.Debug("Starts-ScheduleExclusionDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-ScheduleExclusionDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the schedule exclusion record to the database
        /// </summary>
        /// <param name="scheduleExclusion">ScheduleExclusionDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertScheduleExclusion(ScheduleExclusionDTO scheduleExclusion, string userId, int siteId)
        {
            log.LogMethodEntry(scheduleExclusion, userId, siteId);
            string insertScheduleExclusionQuery = @"insert into Schedule_ExclusionDays 
                                                        ( 
                                                          ScheduleId,
                                                          ExclusionDate,
                                                          IncludeDate,
                                                          Day,
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
                                                          @exclusionDate,
                                                          @includeDate,
                                                          @day,
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
            List<SqlParameter> updateScheduleExclusionParameters = new List<SqlParameter>();
            if (scheduleExclusion.ScheduleId == -1)
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@scheduleId", DBNull.Value));
            }
            else
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@scheduleId", scheduleExclusion.ScheduleId));
            }
            if (string.IsNullOrEmpty(scheduleExclusion.ExclusionDate))
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@exclusionDate", DBNull.Value));
            }
            else
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@exclusionDate", DateTime.Parse(scheduleExclusion.ExclusionDate)));
            }
            updateScheduleExclusionParameters.Add(new SqlParameter("@includeDate", scheduleExclusion.IncludeDate));
            if (scheduleExclusion.Day == -1)
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@day", DBNull.Value));
            }
            else
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@day", scheduleExclusion.Day));
            }
            updateScheduleExclusionParameters.Add(new SqlParameter("@isActive", (scheduleExclusion.IsActive == true? "Y":"N")));
            updateScheduleExclusionParameters.Add(new SqlParameter("@createdBy", userId));
            updateScheduleExclusionParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateScheduleExclusionParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateScheduleExclusionParameters.Add(new SqlParameter("@siteId", siteId));
            if (scheduleExclusion.SynchStatus)
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@synchStatus", scheduleExclusion.SynchStatus));
            }
            else
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            if (scheduleExclusion.MasterEntityId == -1)
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@masterEntityId", scheduleExclusion.MasterEntityId));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertScheduleExclusionQuery, updateScheduleExclusionParameters.ToArray());
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the schedule exclusion record
        /// </summary>
        /// <param name="scheduleExclusion">ScheduleExclusionDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateScheduleExclusion(ScheduleExclusionDTO scheduleExclusion, string userId, int siteId)
        {
            log.LogMethodEntry(scheduleExclusion, userId, siteId);
            string updateScheduleExclusionQuery = @"update Schedule_ExclusionDays 
                                         set ScheduleId=@scheduleId,
                                                          ExclusionDate=@exclusionDate,
                                                          IncludeDate=@includeDate,
                                                          Day=@day,
                                                          IsActive=@isActive,                                                          
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate(),                                                          
                                                          --site_id=@siteid,
                                                          SynchStatus=@synchStatus,
                                                          MasterEntityId=@masterEntityId
                                       where ScheduleExclusionId = @scheduleExclusionId";
            List<SqlParameter> updateScheduleExclusionParameters = new List<SqlParameter>();
            updateScheduleExclusionParameters.Add(new SqlParameter("@scheduleExclusionId", scheduleExclusion.ScheduleExclusionId));
            updateScheduleExclusionParameters.Add(new SqlParameter("@scheduleId", scheduleExclusion.ScheduleId));
            if (string.IsNullOrEmpty(scheduleExclusion.ExclusionDate))
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@exclusionDate", DBNull.Value));
            }
            else
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@exclusionDate", DateTime.Parse(scheduleExclusion.ExclusionDate)));
            }
            updateScheduleExclusionParameters.Add(new SqlParameter("@includeDate", scheduleExclusion.IncludeDate));
            if (scheduleExclusion.Day == -1)
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@day", DBNull.Value));
            }
            else
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@day", scheduleExclusion.Day));
            }
            updateScheduleExclusionParameters.Add(new SqlParameter("@isActive", (scheduleExclusion.IsActive == true ? "Y" : "N")));
            updateScheduleExclusionParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateScheduleExclusionParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateScheduleExclusionParameters.Add(new SqlParameter("@siteId", siteId));
            updateScheduleExclusionParameters.Add(new SqlParameter("@synchStatus", scheduleExclusion.SynchStatus));
            if (scheduleExclusion.MasterEntityId == -1)
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateScheduleExclusionParameters.Add(new SqlParameter("@masterEntityId", scheduleExclusion.MasterEntityId));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateScheduleExclusionQuery, updateScheduleExclusionParameters.ToArray());
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to ScheduleExclusionDTO class type
        /// </summary>
        /// <param name="scheduleExclusionDataRow">ScheduleExclusionDTO DataRow</param>
        /// <returns>Returns ScheduleExclusionDTO</returns>
        private ScheduleExclusionDTO GetScheduleExclusionDTO(DataRow scheduleExclusionDataRow)
        {
            log.LogMethodEntry(scheduleExclusionDataRow);
            ScheduleExclusionDTO scheduleDataObject = new ScheduleExclusionDTO(Convert.ToInt32(scheduleExclusionDataRow["ScheduleExclusionId"]),
                                            Convert.ToInt32(scheduleExclusionDataRow["ScheduleId"]),
                                            scheduleExclusionDataRow["ExclusionDate"] .ToString(),
                                            scheduleExclusionDataRow["IncludeDate"].ToString(),
                                            scheduleExclusionDataRow["Day"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleExclusionDataRow["Day"]),
                                            scheduleExclusionDataRow["IsActive"] == DBNull.Value ? false : (scheduleExclusionDataRow["IsActive"].ToString() == "Y"? true: false), 
                                            scheduleExclusionDataRow["CreatedBy"].ToString(),
                                            scheduleExclusionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleExclusionDataRow["CreationDate"]),
                                            scheduleExclusionDataRow["LastUpdatedBy"].ToString(),
                                            scheduleExclusionDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleExclusionDataRow["LastupdatedDate"]),
                                            scheduleExclusionDataRow["Guid"].ToString(),
                                            scheduleExclusionDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleExclusionDataRow["site_id"]),
                                            scheduleExclusionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(scheduleExclusionDataRow["SynchStatus"]),
                                            scheduleExclusionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleExclusionDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(scheduleDataObject);
            return scheduleDataObject;
        }
        /// <summary>
        /// Gets the schedule exclusion data of passed schedule exclusion Id
        /// </summary>
        /// <param name="scheduleExclusionId">integer type parameter</param>
        /// <returns>Returns ScheduleExclusionDTO</returns>
        public ScheduleExclusionDTO GetScheduleExclusion(int scheduleExclusionId)
        {
            log.LogMethodEntry(scheduleExclusionId);
            string selectScheduleQuery = @"select *
                                         from Schedule_ExclusionDays
                                        where ScheduleExclusionId = @scheduleExclusionId";
            SqlParameter[] selectScheduleExclusionParameters = new SqlParameter[1];
            selectScheduleExclusionParameters[0] = new SqlParameter("@scheduleExclusionId", scheduleExclusionId);
            DataTable schedule = dataAccessHandler.executeSelectQuery(selectScheduleQuery, selectScheduleExclusionParameters);
            if (schedule.Rows.Count > 0)
            {
                DataRow scheduleExclusionRow = schedule.Rows[0];
                ScheduleExclusionDTO scheduleExclusionDataObject = GetScheduleExclusionDTO(scheduleExclusionRow);
                log.LogMethodExit(scheduleExclusionDataObject);
                return scheduleExclusionDataObject;
            }
            else
            {
                log.Debug("Ends-GetScheduleExclusion(scheduleExclusionId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// helps to find the existence inclusion exclusion record for the passed parameter.
        /// </summary>
        /// <param name="scheduleID"></param>
        /// <param name="dtDate"></param>
        /// <returns>bool type value</returns>
        public bool GetExclusionDays(int scheduleID, DateTime dtDate)
        {
            log.LogMethodEntry(scheduleID, dtDate);
            string sqlQuery = @"select 1 from Schedule_ExclusionDays 
                                                                    where ScheduleId = @id 
                                                                    and (ExclusionDate = @date or Day = DATEPART(WEEKDAY, @Date)) 
                                                                    and IncludeDate = 'N'";
            SqlParameter[] selectScheduleExclusionParameters = new SqlParameter[2];
            selectScheduleExclusionParameters[0] = new SqlParameter("@id", scheduleID);
            selectScheduleExclusionParameters[1] = new SqlParameter("@Date", dtDate);
            DataTable dTable = dataAccessHandler.executeSelectQuery(sqlQuery, selectScheduleExclusionParameters);
            if (dTable == null || dTable.Rows.Count == 0)
                return false;
            log.LogMethodExit();
            return true;
        }

        /// <summary>
        /// Gets the ScheduleExclusionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScheduleExclusionDTO matching the search criteria</returns>
        public List<ScheduleExclusionDTO> GetScheduleExclusionList(List<KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectScheduleExclusionQuery = @"select *
                                         from Schedule_ExclusionDays";
            if (searchParameters != null)
            {
                string joiner = " ";//starts:Modification on 18-Jul-2016 for publish feature
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : " ";
                        if (searchParameter.Key == ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SCHEDULE_ID || searchParameter.Key == ScheduleExclusionDTO.SearchByScheduleExclusionParameters.MASTER_ENTITY_ID || searchParameter.Key == ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SCHEDULE_EXCLUSION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ScheduleExclusionDTO.SearchByScheduleExclusionParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + (searchParameter.Value.ToString() == "1"? "'Y'":"'N'"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                        }//Ends:Modification on 18-Jul-2016 for publish feature
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetScheduleExclusionList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectScheduleExclusionQuery = selectScheduleExclusionQuery + query;
            }
            DataTable ScheduleExclusionData = dataAccessHandler.executeSelectQuery(selectScheduleExclusionQuery, null);
            if (ScheduleExclusionData.Rows.Count > 0)
            {
                List<ScheduleExclusionDTO> scheduleExclusionList = new List<ScheduleExclusionDTO>();
                foreach (DataRow ScheduleExclusionDataRow in ScheduleExclusionData.Rows)
                {
                    ScheduleExclusionDTO scheduleExclusionDataObject = GetScheduleExclusionDTO(ScheduleExclusionDataRow);
                    scheduleExclusionList.Add(scheduleExclusionDataObject);
                }
                log.LogMethodExit(scheduleExclusionList);
                return scheduleExclusionList;
            }
            else
            {
                log.Debug("Ends-ScheduleExclusionList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}

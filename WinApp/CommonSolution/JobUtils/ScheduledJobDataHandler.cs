/********************************************************************************************
 * Project Name - Scheduled Job Data Handler
 * Description  - Data handler of the scheduled job class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *1.00        04-Feb-2016    Raghuveera          Created
 *2.70.2        19-Sep-2019    Dakshakh            Modified : Added logs
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Scheduled Job Data Handler
    /// </summary>
    public class ScheduledJobDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of ScheduledJobDataHandler class
        /// </summary>
        public ScheduledJobDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to ScheduledJobDTO class type
        /// </summary>
        /// <param name="scheduledJobDataRow">Scheduled DataRow</param>
        /// <returns>Returns Scheduled</returns>
        private ScheduledJobDTO GetScheduledJobDTO(DataRow scheduledJobDataRow)
        {
            log.LogMethodEntry(scheduledJobDataRow);
            ScheduledJobDTO assetDataObject = new ScheduledJobDTO(scheduledJobDataRow["ScheduleId"]==DBNull.Value?-1: Convert.ToInt32(scheduledJobDataRow["ScheduleId"]),
                                            scheduledJobDataRow["MaintScheduleId"]==DBNull.Value?-1: Convert.ToInt32(scheduledJobDataRow["MaintScheduleId"]),
                                            scheduledJobDataRow["MaintJobType"].ToString(),
                                            scheduledJobDataRow["MaintJobName"].ToString(),
                                            scheduledJobDataRow["AssignedTo"].ToString(),
                                            scheduledJobDataRow["AssignedUserId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduledJobDataRow["AssignedUserId"]),
                                            scheduledJobDataRow["DepartmentId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduledJobDataRow["DepartmentId"]),
                                            scheduledJobDataRow["DurationToComplete"] == DBNull.Value ? -1 : Convert.ToInt32(scheduledJobDataRow["DurationToComplete"]),
                                            scheduledJobDataRow["ChklstScheduleTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduledJobDataRow["ChklstScheduleTime"]),
                                            scheduledJobDataRow["CreatedBy"].ToString(),
                                            scheduledJobDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduledJobDataRow["CreationDate"]),
                                            scheduledJobDataRow["LastUpdatedBy"].ToString(),
                                            scheduledJobDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduledJobDataRow["LastupdatedDate"])
                                            );
            log.LogMethodExit(assetDataObject);
            return assetDataObject;
        }
        /// <summary>
        /// get all scheduled job from database
        /// </summary>
        /// <returns>ScheduledJobDTO list</returns>
        public List<ScheduledJobDTO> GetScheduledJobsList(DateTime date)
        {
            log.LogMethodEntry(date);
            string selectMaintenanceJobQuery = @"SELECT Sch.ScheduleId ScheduleId
                                                ,MSch.MaintScheduleId MaintScheduleId
                                                ,'Job' MaintJobType
                                                ,Sch.ScheduleName MaintJobName
	                                            ,U.username AssignedTo
	                                            ,MSch.UserId AssignedUserId
	                                            ,MSch.DepartmentId
	                                            ,MSch.DurationToComplete DurationToComplete
	                                            ,ScheduleJob.NextJobDate ChklstScheduleTime
	                                            ,Sch.CreatedBy
	                                            ,Sch.CreationDate
	                                            ,Sch.LastUpdatedBy
	                                            ,Sch.LastupdatedDate
                                                FROM Schedule Sch,
                                                Users U,
                                                Maint_Schedule MSch,
                                                 (      
		                                            SELECT scheduleId,
			                                        CASE
			                                        WHEN RECURFLAG='N'
			                                        THEN ScheduleTime
			                                        ELSE
				                                    CASE 
				                                    WHEN RecurFrequency = 'D' 
				                                    THEN DATEADD(DAY, DATEDIFF(day, ScheduleTime, @date), ScheduleTime)
				                                    WHEN RecurFrequency = 'W'
				                                    THEN DATEADD(WEEK, DATEDIFF(week, ScheduleTime, @date), ScheduleTime)
				                                    WHEN RecurFrequency = 'M'
				                                    THEN case RecurType 
						                            WHEN 'D' THEN DATEADD( HH,
											                       DATEPART(HH,ScheduleTime),
											                       DATEADD(MONTH, 
													               DATEDIFF(MONTH, ScheduleTime, @date), 
													               CAST( CONVERT(DATE,ScheduleTime) AS DATETIME)
													                    )
											                        )
						                            WHEN 'W' THEN CAST(DATEADD(DD,
												    (DATEPART(WEEKDAY, ScheduleTime) + 7 - datepart(WEEKDAY, dateadd(day,-DATEPART(DAY,@date - 1),@date))) % 7 
															 + 1 
															 + 7 * (((DATEPART(DD, ScheduleTime) + 6 
															 - (DATEPART(WEEKDAY, ScheduleTime) + 7 
															 - DATEPART(WEEKDAY, DATEADD(day, -DATEPART(DAY,ScheduleTime-1), ScheduleTime))) % 7) / 7) - 1)
													,CONVERT(DATE,DATEADD(day,-DATEPART(DAY,@date),@date))) as DATETIME
										            ) + CAST(CAST(ScheduleTime as TIME) as DATETIME)
						                            ELSE null 
						                            END
					                                ELSE NULL
					                                END 
				                                    END NextJobDate
		                                            FROM Schedule
		                                            WHERE IsActive = 'Y'
		                                            ) ScheduleJob
                                                    WHERE Sch.scheduleId = ScheduleJob.ScheduleId
                                                    AND Sch.ScheduleId = MSch.ScheduleId
	                                                AND MSch.UserId = U.user_id
	                                                AND ScheduleJob.NextJobDate > ISNULL(MSch.MaxValueJobCreated, DATEADD(DD,-1,Sch.ScheduleTime))
                                                    AND ScheduleJob.NextJobDate <= dateadd(dd,1,convert(datetime,convert(date,@date))) 
                                                    AND Sch.IsActive = 'Y'
                                                    AND
                                                    (
                                                    EXISTS (SELECT 1
                                                    FROM Schedule_ExclusionDays sed
                                                    WHERE sed.ScheduleId = Sch.ScheduleId
                                                    AND ((@date between ExclusionDate and DATEADD(dd, 1, ExclusionDate))
                                                    or sed.Day = DATEPART(WEEKDAY, @date))
                                                    AND IncludeDate = 'Y')
                                                    OR (
                                                        (Sch.recurFlag = 'N') 
                                                        AND (@date 
                                                        BETWEEN CONVERT(DATE, Sch.ScheduleTime) AND DATEADD(dd, 1, Sch.ScheduleTime))
                                                        )
                                                    OR (
                                                        (Sch.recurFlag = 'Y') AND (Sch.RecurFrequency = 'D') 
                                                        AND (DATEADD( HH,
                                                        DATEPART(HH,ScheduleTime),
                                                        DATEADD(D, 0, DATEDIFF(D, 0, @date))
                                                        ) 
                                                    BETWEEN CONVERT(DATE, Sch.ScheduleTime) AND Sch.RecurEndDate)
                                                    )	    
                                                    OR (
                                                        (Sch.RecurFrequency = 'W') 
                                                        AND (
                                                        cast(DATEADD(D, 0, DATEDIFF(D, 0, @date)) as DATETIME) + cast(cast(@date as time) as DATETIME) 
                                                        BETWEEN CONVERT(DATE, Sch.ScheduleTime) AND Sch.RecurEndDate
                                                        )  
                                                    )
                                                    OR (
                                                        (Sch.RecurFrequency = 'M') 
                                                         AND (
                                                            CAST(DATEADD(D, 0, DATEDIFF(D, 0, @date)) as DATETIME) + CAST(cast(@date as time) as DATETIME)
                                                            BETWEEN CONVERT(DATE, Sch.ScheduleTime) AND Sch.RecurEndDate
                                                             ) 
                                                      )
                                                     )
                                                    AND NOT EXISTS (SELECT 1
                                                    FROM Schedule_ExclusionDays sed
                                                    WHERE sed.ScheduleId = Sch.ScheduleId
                                                    AND isnull(IncludeDate, 'N') = 'N'
                                                    AND (
                                                    ( @date between ExclusionDate and dateadd(dd, 1, ExclusionDate)
                                                     )
                                                    OR sed.Day = DATEPART(WEEKDAY, @date)
                                                    )
                                                   )";
            SqlParameter[] scheduledJobParameter = new SqlParameter[1];
            if (date.Equals(DateTime.MinValue))
            {
                scheduledJobParameter[0] = new SqlParameter("@date", DateTime.Now);
            }
            else
            {
                scheduledJobParameter[0] = new SqlParameter("@date", date);
            }

            DataTable scheduledJobData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, scheduledJobParameter);//selectMaintenanceJobQuery
            if (scheduledJobData.Rows.Count > 0)
            {
                List<ScheduledJobDTO> scheduledJobList = new List<ScheduledJobDTO>();
                foreach (DataRow scheduledJobDataRow in scheduledJobData.Rows)
                {
                    ScheduledJobDTO scheduledJobDataObject = GetScheduledJobDTO(scheduledJobDataRow);
                    scheduledJobList.Add(scheduledJobDataObject);
                }
                log.LogMethodExit(scheduledJobList);
                return scheduledJobList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
    }
}

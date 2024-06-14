/********************************************************************************************
 * Project Name - Dashboard
 * Description  - CollectionDashBoardDataHandler class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.7      23-Apr-2022       Nitin Pai                Changes to add Sales Dashbard report in BIZINSIGHTS and LITE apps
 2.130.10     17-Jul-2022       Nitin Pai                Show weekly collection in BizInsights
 2.150.2      05-Apr-2023        Guru Kodaja              Todals Sales in Tablet connection string issue
 2.150.2      07-Jul-2023        Abhishek                 Modified:BizInsights Lite- Displaying Sales/Consumption Based on ManagementFormAccess.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Text;

namespace Semnox.Parafait.DashBoard
{
    public class CollectionDashBoardDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        string siteTimeZoneName = "";
        /// <summary>
        /// Default constructor of WeeklyCollectionReportDataHandler class
        /// </summary>
        public CollectionDashBoardDataHandler()
        {
            log.Debug("Starts-WeeklyCollectionReportDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-WeeklyCollectionReportDataHandler() default constructor.");
        }


        /// <summary>
        /// Gets the Weeekly Collection DTO containing as on date and WeeklyCollectionReportDTO list 
        /// </summary>
        /// <returns>Returns WeeklyCollectionDTO</returns>
        public WeeklyCollectionDTO GetWeeklyCollectionList(int roleId = -1)
        {
            // Check to be made generic
            log.Debug("Starts-GetWeeklyCollectionList() Method.");

            int siteId = -1;
            DataTable dtSites = dataAccessHandler.executeSelectQuery("select COUNT(site_id) siteCount,max(site_id) site_id  from site", null);
            if (Convert.ToInt32(dtSites.Rows[0]["siteCount"]) == 1)
            {
                siteId = Convert.ToInt32(dtSites.Rows[0]["site_id"]);
            }


            DateTime curdate = getServerBusinessTime();
            DateTime weekstartdate = curdate;

            while (weekstartdate.DayOfWeek != DayOfWeek.Monday)
            {
                weekstartdate = weekstartdate.AddDays(-1);
            }

            log.Debug("DASHBOARD REPORT: DayOfWeek.Monday");
            log.Debug("DASHBOARD REPORT: weekstartdate" + weekstartdate);

            log.Debug("CurDate before offset " + curdate);
            log.Debug("weekstartdate before offset " + weekstartdate);
            curdate = OffsetTime(curdate, siteId);
            weekstartdate = OffsetTime(weekstartdate, siteId);
            log.Debug("CurDate after offset " + curdate);
            log.Debug("weekstartdate after offset " + weekstartdate);

            string roleCondition = string.Empty;
            if (roleId > -1)
            {
                List<int> siteList = new List<int>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string query = "select distinct s.site_name as SiteName, s.site_id as Id " +
                                              " from site s,ManagementFormAccess ma " +
                                             " where ma.FunctionGUID = s.Guid and(ma.role_id = @role or - 1 = @role) " +
                                             " and ma.FunctionGroup = 'Data Access' " +
                                             " and ma.access_allowed = 'Y' and ma.main_menu = 'Sites' " +
                                             " and s.site_name != 'Master' order by 2 asc";
                parameters.Add(new SqlParameter("@role", roleId));
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), null);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int id = dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]);
                        siteList.Add(id);
                    }
                }
                string siteIdList = string.Empty;
                StringBuilder sb = new StringBuilder("");
                for (int i = 0; i < siteList.Count; i++)
                {
                    if (i != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(siteList[i]);
                }
                siteIdList = sb.ToString();
                roleCondition += " and s.site_id IN (" + siteIdList + ")";
            }
            string selectWeeklyCollQuery = @"select
                                                    s.site_id,site_name,
                                                            isnull(CollectionToday,0) CollectionToday,
                                                            isnull(CollectionPrevDay,0) CollectionPreviousDay,
                                                            isnull(CollectionWeek,0) CollectionWeek,
                                                            isnull(CollectionPrevWeek,0) CollectionPreviousWeek,
                                                            isnull(GamePlayToday,0) GamePlayToday,
                                                            isnull(GamePlayPrevDay,0) GamePlayPreviousDay,
                                                            isnull(GamePlayWeek,0) GamePlayWeek,
                                                            isnull(GamePlayPrevWeek,0) GamePlayPreviousWeek
                                                from site s
                                                left outer join (
                                                        SELECT isnull(site_Id,@siteid) site_id,
                                                                            sum(case when trxdate >= @TodayStart and trxdate < @TodayEnd then  (cashAmount + CreditCardAmount + OtherPaymentModeAmount) else 0 end)  CollectionToday,
                                                                            sum(case when trxdate >= @PrevDayStart and trxdate < @TodayStart then  (cashAmount + CreditCardAmount + OtherPaymentModeAmount) else 0 end)  CollectionPrevDay,
                                                                            sum(case when trxdate >= @FromDateWeek and trxdate < @TodayEnd then  (cashAmount + CreditCardAmount + OtherPaymentModeAmount) else 0 end)  CollectionWeek,
                                                                            sum(case when trxdate >= @FromDate and trxdate < @FromDateWeek then  (cashAmount + CreditCardAmount + OtherPaymentModeAmount) else 0 end)  CollectionPrevWeek
                                                                    FROM TRX_HEADER with (index (trx_date))
                                                                    WHERE TRXDATE BETWEEN @FROMDATE AND @TodayEnd
                                                                    and status = 'CLOSED'
                                                                    group by isnull(site_id,@siteid)) coll on coll.site_id = s.site_id
                                                left outer join (
                                                        select isnull(gp.site_id,@siteid) site_id,
                                                                sum(case when play_date  >= @TodayStart and play_date < @TodayEnd then  (gp.credits +gp.courtesy + gp.bonus+ gp.time ) else 0 end)  GamePlayToday,
                                                                sum(case when play_date  >= @PrevDayStart and play_date < @TodayStart then  (gp.credits +gp.courtesy + gp.bonus+ gp.time ) else 0 end)  GamePlayPrevDay,
                                                                sum(case when play_date  >= @FromDateWeek and play_date < @TodayEnd then  (gp.credits +gp.courtesy + gp.bonus+ gp.time ) else 0 end)  GamePlayWeek,
                                                                sum(case when play_date  >= @FromDate and play_date < @FromDateWeek then  (gp.credits +gp.courtesy + gp.bonus+ gp.time ) else 0 end)  GamePlayPrevWeek
                                                                    from (SELECT play_date, Gameplay_id, card_id, site_id, (credits + CPCredits) Credits, courtesy,
                                                                                        (bonus + CPBonus) Bonus, time, CardGame
                                                                                FROM GAMEPLAY gp with (index(IX_Gameplay_Playdate))
                                                                                WHERE  PLAY_DATE BETWEEN @FromDate and @TodayEnd
                                                                                ) gp, cards c
                                                                    WHERE gp.card_id = c.card_id
                                                                        and c.technician_card = 'N'
                                                                    GROUP BY isnull(gp.site_id,@siteid)
                                                            ) gp on gp.site_id = s.site_id
                                                where isnull(s.SiteCode,0)>0 "+ roleCondition + " order by 1";

            List<SqlParameter> weeklyCollParameters = new List<SqlParameter>();
            weeklyCollParameters.Add(new SqlParameter("@siteid", siteId));
            DateTime fromDate = weekstartdate.AddDays(-7);
            weeklyCollParameters.Add(new SqlParameter("@FromDate", fromDate));
            DateTime fromDateWeek = weekstartdate;
            weeklyCollParameters.Add(new SqlParameter("@FromDateWeek", weekstartdate));
            DateTime prevDayStart = curdate.AddDays(-1);
            weeklyCollParameters.Add(new SqlParameter("@PrevDayStart", curdate.AddDays(-1)));
            DateTime todayStart = curdate;
            weeklyCollParameters.Add(new SqlParameter("@TodayStart", curdate));
            DateTime todayEnd = curdate.AddDays(1);
            weeklyCollParameters.Add(new SqlParameter("@TodayEnd", curdate.AddDays(1)));

            for (int wc = 0; wc < weeklyCollParameters.Count; wc++)
            {
                log.Debug("DASHBOARD REPORT: weeklyCollParameters: ParameterName:" + weeklyCollParameters[wc].ParameterName + " || " + weeklyCollParameters[wc].Value);
            }

            DataTable WeeklyCollData = dataAccessHandler.executeSelectQuery(selectWeeklyCollQuery, weeklyCollParameters.ToArray());

            if (WeeklyCollData.Rows.Count > 0)
            {
                List<WeeklyCollectionReportDTO> weeklyCollectionReportDTOList = new List<WeeklyCollectionReportDTO>();
                foreach (DataRow weeklyCollDataRow in WeeklyCollData.Rows)
                {
                    WeeklyCollectionReportDTO weeklyCollectionReportDTO =
                                new WeeklyCollectionReportDTO(weeklyCollDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(weeklyCollDataRow["site_id"]),
                                                                weeklyCollDataRow["site_name"].ToString(),
                                                                Convert.ToInt32(weeklyCollDataRow["CollectionToday"]),
                                                                Convert.ToInt32(weeklyCollDataRow["CollectionPreviousDay"]),
                                                                Convert.ToInt32(weeklyCollDataRow["CollectionWeek"]),
                                                                Convert.ToInt32(weeklyCollDataRow["CollectionPreviousWeek"]),
                                                                Convert.ToInt32(weeklyCollDataRow["GamePlayToday"]),
                                                                Convert.ToInt32(weeklyCollDataRow["GamePlayPreviousDay"]),
                                                                Convert.ToInt32(weeklyCollDataRow["GamePlayWeek"]),
                                                                Convert.ToInt32(weeklyCollDataRow["GamePlayPreviousWeek"]));

                    weeklyCollectionReportDTO.WeeklyCollectionPOS = GetWeeklyCollectionPOSList(Convert.ToInt32(weeklyCollDataRow["site_id"]), "-1");
                    weeklyCollectionReportDTOList.Add(weeklyCollectionReportDTO);


                }
                // Summary Row
                weeklyCollectionReportDTOList.Add(new WeeklyCollectionReportDTO(
                                                         9999,
                                                         "Total",
                                                        Convert.ToInt32(WeeklyCollData.Compute("Sum(CollectionToday)", "")),
                                                        Convert.ToInt32(WeeklyCollData.Compute("Sum(CollectionPreviousDay)", "")),
                                                        Convert.ToInt32(WeeklyCollData.Compute("Sum(CollectionWeek)", "")),
                                                        Convert.ToInt32(WeeklyCollData.Compute("Sum(CollectionPreviousWeek)", "")),
                                                        Convert.ToInt32(WeeklyCollData.Compute("Sum(GamePlayToday)", "")),
                                                        Convert.ToInt32(WeeklyCollData.Compute("Sum(GamePlayPreviousDay)", "")),
                                                        Convert.ToInt32(WeeklyCollData.Compute("Sum(GamePlayWeek)", "")),
                                                        Convert.ToInt32(WeeklyCollData.Compute("Sum(GamePlayPreviousWeek)", "")))
                                                  );

                DateTime serverTime = getServerBusinessTime();

                //if (!string.IsNullOrWhiteSpace(siteTimeZoneName) && siteTimeZoneName != TimeZone.CurrentTimeZone.StandardName)
                //{
                //    TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);
                //    serverTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetTimeZone); //getServerBusinessTime();
                //}
                TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);
                serverTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetTimeZone); //getServerBusinessTime();
                String dateTimeString = serverTime.ToString("'Report as on' dddd, MMM dd, yyyy  hh:mm tt");
                WeeklyCollectionDTO weeklyCollection = new WeeklyCollectionDTO(dateTimeString, fromDate.ToString(), fromDateWeek.ToString(), prevDayStart.ToString(), todayStart.ToString(), todayEnd.ToString() + "|  " + siteTimeZoneName, weeklyCollectionReportDTOList);

                log.Debug("Exiting WeeklyCollection");
                return weeklyCollection;
            }
            else
            {
                log.Debug("Ends-GetWeeklyCollectionList() Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the GetWeeklyCollectionPOSList list 
        /// <param name="siteId">int type parameter</param>
        /// <param name="posMachine">string type parameter</param>
        /// </summary>
        /// <returns>Returns the list of WeeklyCollectionPOSReportDTO </returns>
        public List<WeeklyCollectionPOSReportDTO> GetWeeklyCollectionPOSList(int siteId, string posMachine)
        {
            // Check to be made generic
            log.Debug("Starts-GetWeeklyCollectionPOSList() Method.");

            DateTime curdate = getServerBusinessTime();
            DateTime weekstartdate = curdate;

            while (weekstartdate.DayOfWeek != DayOfWeek.Monday)
            {
                weekstartdate = weekstartdate.AddDays(-1);
            }

            DataTable dtSites = dataAccessHandler.executeSelectQuery("select COUNT(site_id) siteCount,max(site_id) site_id  from site", null);
            if (Convert.ToInt32(dtSites.Rows[0]["siteCount"]) == 1)
            {
                siteId = -1;
            }

            log.Debug("CurDate before offset " + curdate);
            log.Debug("weekstartdate before offset " + weekstartdate);
            curdate = OffsetTime(curdate, siteId);
            weekstartdate = OffsetTime(weekstartdate, siteId);
            log.Debug("CurDate after offset " + curdate);
            log.Debug("weekstartdate after offset " + weekstartdate);

            string selectWeeklyCollQuery = @"select POSMachineId,POSName pos_machine,
                                                isnull(CollectionToday,0) CollectionToday,isnull(CollectionPrevDay,0) CollectionPreviousDay,
                                                isnull(CollectionWeek,0) CollectionWeek,isnull(CollectionPrevWeek,0) CollectionPreviousWeek
                                             from POSMachines pm 
                                             left outer join (select pos_machine,
                                                                    sum(case when trxdate >= @TodayStart and trxdate < @TodayEnd then ((amount * CashRatio) + (amount * CreditCardRatio)) else 0 end)  CollectionToday,
                                                                    sum(case when trxdate >= @PrevDayStart and trxdate < @TodayStart then ((amount * CashRatio) + (amount * CreditCardRatio)) else 0 end)  CollectionPrevDay,
                                                                    sum(case when trxdate >= @FromDateWeek and trxdate < @TodayEnd then ((amount * CashRatio) + (amount * CreditCardRatio)) else 0 end)  CollectionWeek,
                                                                    sum(case when trxdate >= @FromDate and trxdate < @FromDateWeek then ((amount * CashRatio) + (amount * CreditCardRatio)) else 0 end)  CollectionPrevWeek
                                                                from TransactionView v
                                                                where trxdate >= @FromDate and trxdate < @TodayEnd 
                                                                                            and (site_id = @SiteId or  @SiteId=-1)
                                                                                            and (pos_machine=@posMachine or  @posMachine='-1')
                                                                group by pos_machine) tv  on pm.POSName=tv.pos_machine
                                              where (site_id = @SiteId or  @SiteId=-1 )  
                                              and (pos_machine=@posMachine or  @posMachine='-1') 
                                              and ((isnull(CollectionToday,0)+isnull(CollectionPrevDay,0)++isnull(CollectionWeek,0)++isnull(CollectionPrevWeek,0)) > 0)
                                                order by POSName";

            List<SqlParameter> weeklyCollParameters = new List<SqlParameter>();

            weeklyCollParameters.Add(new SqlParameter("@SiteId", siteId));
            weeklyCollParameters.Add(new SqlParameter("@FromDate", weekstartdate.AddDays(-7)));
            weeklyCollParameters.Add(new SqlParameter("@FromDateWeek", weekstartdate));
            weeklyCollParameters.Add(new SqlParameter("@PrevDayStart", curdate.AddDays(-1)));
            weeklyCollParameters.Add(new SqlParameter("@TodayStart", curdate));
            weeklyCollParameters.Add(new SqlParameter("@TodayEnd", curdate.AddDays(1)));

            if (posMachine.Length <= 0)
                posMachine = "-1";

            weeklyCollParameters.Add(new SqlParameter("@posMachine", posMachine));

            DataTable WeeklyCollData = dataAccessHandler.executeSelectQuery(selectWeeklyCollQuery, weeklyCollParameters.ToArray());

            if (WeeklyCollData.Rows.Count > 0)
            {
                List<WeeklyCollectionPOSReportDTO> weeklyCollectionPOSReportDTOList = new List<WeeklyCollectionPOSReportDTO>();
                foreach (DataRow weeklyCollPOSDataRow in WeeklyCollData.Rows)
                {

                    weeklyCollectionPOSReportDTOList.Add(new WeeklyCollectionPOSReportDTO(
                                                               weeklyCollPOSDataRow["pos_machine"].ToString(),
                                                               Convert.ToInt32(weeklyCollPOSDataRow["CollectionToday"]),
                                                               Convert.ToInt32(weeklyCollPOSDataRow["CollectionPreviousDay"]),
                                                               Convert.ToInt32(weeklyCollPOSDataRow["CollectionWeek"]),
                                                               Convert.ToInt32(weeklyCollPOSDataRow["CollectionPreviousWeek"]))
                                                        );


                }
                log.Debug("Ends-GetWeeklyCollectionList() Method by returning weeklyCollectionPOSReportDTOList.");
                return weeklyCollectionPOSReportDTOList;
            }
            else
            {
                log.Debug("Ends-GetWeeklyCollectionList() Method by returning null.");
                return null;
            }
        }

        public DateTime getServerBusinessTime()
        {
            string connectionString = null;
            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                if (item.Name.IndexOf("ParafaitConnectionString", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    connectionString = item.ConnectionString;
                    break;
                }
            }

            DateTime curDate = DateTime.Now;
            double businessDayStartTime = 6;// 12??

            using (Utilities parafaitUtility = new Utilities(connectionString))
            {
                siteTimeZoneName = parafaitUtility.getParafaitDefaults("WEBSITE_TIME_ZONE");
                double.TryParse(parafaitUtility.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);
            }

            if (!string.IsNullOrWhiteSpace(siteTimeZoneName) && siteTimeZoneName != TimeZone.CurrentTimeZone.StandardName)
            {
                TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);

                log.Debug("DASHBOARD REPORT: localTimeZone" + localTimeZone);
                log.Debug("DASHBOARD REPORT: targetTimeZone" + targetTimeZone);
                log.Debug("DASHBOARD REPORT: businessDayStartTime" + businessDayStartTime);

                DateTime localToSiteTime = TimeZoneInfo.ConvertTime(curDate, localTimeZone, targetTimeZone);
                if (localToSiteTime.Hour < businessDayStartTime)
                {
                    log.Debug("DASHBOARD REPORT: RESETTING TODAY TO YESTRDAY" + localToSiteTime);
                    localToSiteTime = localToSiteTime.AddDays(-1);
                }

                DateTime businesStartTime = localToSiteTime.Date.AddHours(businessDayStartTime);
                DateTime siteToLocalTime = TimeZoneInfo.ConvertTime(businesStartTime, targetTimeZone, localTimeZone);
                curDate = siteToLocalTime;

                log.Debug("DASHBOARD REPORT: localToSiteTime" + localToSiteTime);
                log.Debug("DASHBOARD REPORT: businesStartTime" + businesStartTime);
                log.Debug("DASHBOARD REPORT: siteToLocalTime" + siteToLocalTime);
            }
            else
            {
                log.Debug("No time zone set so returning starttime " + curDate.Date.AddHours(businessDayStartTime));
                curDate = curDate.Date.AddHours(businessDayStartTime);
            }

            log.LogMethodExit(curDate);
            return curDate;
        }

        public DateTime OffsetTime(DateTime curDate, int siteId)
        {
            log.LogMethodEntry(curDate, siteId);
            TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
            int offSetDuration = 0;
            offSetDuration = timeZoneUtil.GetOffSetDuration(siteId, curDate);
            curDate = curDate.AddSeconds(offSetDuration);

            log.LogMethodExit(curDate);
            return curDate;
        }

    }

}

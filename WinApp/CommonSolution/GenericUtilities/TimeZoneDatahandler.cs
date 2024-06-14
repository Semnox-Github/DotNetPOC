/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - TimeZoneDatahandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70.2        12-Aug-2019  Deeksha              Added logger methods.
 ********************************************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using Semnox.Core.Utilities;
namespace Semnox.Core.GenericUtilities
{
    public class TimeZoneDatahandler
    {

        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string connstring;

        /// <summary>
        /// Default constructor of TimeZoneDatahandler class
        /// </summary>
        public TimeZoneDatahandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new  DataAccessHandler();
            connstring = dataAccessHandler.ConnectionString;
            log.LogMethodExit();
        }


        public List<KeyValuePair<int, string>> GetSiteTimeZones()
        {
            log.LogMethodEntry();
            List<KeyValuePair<int, string>> SiteTimeZones = new List<KeyValuePair<int, string>>();

            DataTable dt = dataAccessHandler.executeSelectQuery(@"select count(*) as sitecount from site", null);

            string sqlQuery = "";
            if ( Convert.ToInt32(dt.Rows[0]["sitecount"])  > 1)
            {
                sqlQuery = @"select s.site_id, isnull(WEBSITE_TIME_ZONE,'') WEBSITE_TIME_ZONE, isnull(BUSINESS_DAY_START_TIME,0)  BUSINESS_DAY_START_TIME  from site s
                                    left outer join (select site_id, default_value as WEBSITE_TIME_ZONE from parafait_defaults where default_value_name='WEBSITE_TIME_ZONE' ) df1  on  s.site_id = df1.site_id
                                    left outer join  (select site_id, default_value as BUSINESS_DAY_START_TIME from parafait_defaults where default_value_name='BUSINESS_DAY_START_TIME' ) df2  on  s.site_id = df2.site_id ";
            }
            else
            {
                sqlQuery = @"select site_id, 
                                    (select top 1 isnull(default_value,'')  as WEBSITE_TIME_ZONE from parafait_defaults where default_value_name='WEBSITE_TIME_ZONE' ) WEBSITE_TIME_ZONE,
                                    (select top 1   isnull(default_value,0 )  as BUSINESS_DAY_START_TIME from parafait_defaults where default_value_name='BUSINESS_DAY_START_TIME' )  BUSINESS_DAY_START_TIME
                            from site ";
            }

            dt = dataAccessHandler.executeSelectQuery(sqlQuery, null);

            if (dt != null || dt.Rows.Count > 0)
            {
                foreach (DataRow drow in dt.Rows)
                {
                    SiteTimeZones.Add(new KeyValuePair<int, string>(Convert.ToInt32(drow["site_id"]), drow["WEBSITE_TIME_ZONE"] + "|" + drow["BUSINESS_DAY_START_TIME"]));
                }
            }
            log.LogMethodExit(SiteTimeZones);
            return SiteTimeZones;

        }


        ///// <summary>
        ///// Get TimeZone Offset Hours
        ///// </summary>
        ///// <param name="inTrxDate">DateTime</param>
        ///// <returns> returns TimeSpan</returns>
        //public int GetOffSetHours(int siteId, DateTime inTrxDate)
        //{
        //    int offSetHours = 0;
        //    string siteTimeZoneName = "";

        //    SqlParameter[] selectParameters = new SqlParameter[1];
        //    selectParameters[0] = new SqlParameter("@siteId", siteId);

        //    DataTable dt = dataAccessHandler.executeSelectQuery(@"select top 1 isnull(default_value,'') as default_value from parafait_defaults
        //                                        where default_value_name='WEBSITE_TIME_ZONE' 
        //                                        and (site_id = @siteId or @siteId = -1 ) ", selectParameters);

        //    if (dt == null ||dt.Rows.Count == 0)
        //        return 0;

        //    siteTimeZoneName = dt.Rows[0]["default_value"].ToString();

        //    if (!String.IsNullOrEmpty(siteTimeZoneName))
        //    {
        //        if (siteTimeZoneName != TimeZone.CurrentTimeZone.StandardName)
        //        {
        //            DateTime locUtcTime = inTrxDate.ToUniversalTime();

        //            //TimeZoneInfo hostTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone.StandardName);
        //            TimeZoneInfo hostTimeZone = TimeZoneInfo.Local;
        //            TimeZoneInfo siteTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);

        //            DateTime HostServerTime = TimeZoneInfo.ConvertTimeFromUtc(locUtcTime, hostTimeZone);
        //            DateTime SiteServerTime = TimeZoneInfo.ConvertTimeFromUtc(locUtcTime, siteTimeZone);

        //            TimeSpan datediff = HostServerTime - SiteServerTime;

        //            offSetHours = datediff.Hours;
        //        }
        //    }

        //    return offSetHours;

        //    // End Logic to udpate Transcation Date based on site time zone
        //}




        /// <summary>
        /// GetBussinessOffSetHours() method
        /// </summary>
        /// <returns> returns int hours</returns>
        public int GetBussinessOffSetHours()
        {
            log.LogMethodEntry();
            int hours = 0;

            try
            {
                DateTime curDate = DateTime.Now.Date;
                // Start Logic to udpate Transcation Date based on site time zone
                using ( Semnox.Core.Utilities.Utilities  parafaitUtility = new  Semnox.Core.Utilities.Utilities (connstring))
                {

                    string siteTimeZoneName = parafaitUtility.getParafaitDefaults("WEBSITE_TIME_ZONE");
                    double businessDayStartTime = 6;
                    double.TryParse(parafaitUtility.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);

                    if (siteTimeZoneName != TimeZone.CurrentTimeZone.StandardName)
                    {
                        DateTime locUtcTime = curDate.AddHours(businessDayStartTime + 1).ToUniversalTime();
                        TimeZoneInfo hostTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone.StandardName);
                        TimeZoneInfo siteTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);

                        DateTime HostServerTime = TimeZoneInfo.ConvertTimeFromUtc(locUtcTime, hostTimeZone);
                        DateTime SiteServerTime = TimeZoneInfo.ConvertTimeFromUtc(locUtcTime, siteTimeZone);

                        TimeSpan datediff = SiteServerTime - HostServerTime;

                        hours = datediff.Hours;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while executing GetBussinessOffSetHours()" + ex.Message);
                log.LogMethodExit("Throwing Exception" + ex.Message);
                throw;
            }

            log.LogMethodExit(hours);
            return hours;
        }

    }
}

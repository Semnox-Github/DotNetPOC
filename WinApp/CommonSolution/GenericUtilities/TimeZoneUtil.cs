/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - TimeZoneUtils
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70.2        12-Aug-2019  Deeksha              Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    public class TimeZoneUtil
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<KeyValuePair<int, string>> SiteTimeZoneList = new List<KeyValuePair<int, string>>();

        public TimeZoneUtil() 
        {
            log.LogMethodEntry();
            SiteTimeZoneList = new TimeZoneDatahandler().GetSiteTimeZones();
            log.LogMethodExit();
        }



        /// <summary>
        /// GetOffSetDuration method
        /// </summary>
        /// <param name="SiteId">SiteId</param>
        /// <returns>returns int offset duration in seconds</returns>
        public int GetOffSetDuration(int SiteId, DateTime inDate)
        {
            log.LogMethodEntry(SiteId, inDate);
            int offSetHours = 0;
            string siteTimeZoneName = "";

            DateTime inDateLocal = inDate;
            if (inDate.Date == inDate)
            {
                int bizStartHour = Utilities.ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext.GetExecutionContext(), "BUSINESS_DAY_START_TIME", 6);
                inDateLocal = inDateLocal.AddHours(bizStartHour);
                log.LogVariableState("inDateLocal with biz start hr", inDateLocal);
            }

            if (SiteId == -1 && SiteTimeZoneList.Count == 1)
            {
                siteTimeZoneName = SiteTimeZoneList[0].Value.ToString().Split('|')[0];
            }
            else if (SiteTimeZoneList.Where(x=>x.Key == SiteId).Count() == 1)
            {
                siteTimeZoneName = SiteTimeZoneList.Where(x => x.Key == SiteId).FirstOrDefault().Value.ToString().Split('|')[0];
            }
            log.LogVariableState("siteTimeZoneName", siteTimeZoneName);
            log.LogVariableState("TimeZone.CurrentTimeZone.StandardName", TimeZone.CurrentTimeZone.StandardName);

            if (siteTimeZoneName!= ""  && siteTimeZoneName != TimeZone.CurrentTimeZone.StandardName)
            {
                DateTime locUtcTime = inDateLocal.ToUniversalTime();
                log.LogVariableState("locUtcTime", locUtcTime);

                string dBServerTimeZone = new Utilities.LookupValuesList(Utilities.ExecutionContext.GetExecutionContext()).GetServerTimeZone();
                log.LogVariableState("dBServerTimeZone", dBServerTimeZone);

                TimeZoneInfo hostTimeZone = string.IsNullOrWhiteSpace(dBServerTimeZone) ? TimeZoneInfo.Local : TimeZoneInfo.FindSystemTimeZoneById(dBServerTimeZone);
                TimeZoneInfo siteTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);
                log.LogVariableState("hostTimeZone", hostTimeZone.StandardName);
                log.LogVariableState("siteTimeZone", siteTimeZone.StandardName);

                DateTime HostServerTime = TimeZoneInfo.ConvertTimeFromUtc(locUtcTime, hostTimeZone);
                DateTime SiteServerTime = TimeZoneInfo.ConvertTimeFromUtc(locUtcTime, siteTimeZone);
                log.LogVariableState("HostServerTime", HostServerTime);
                log.LogVariableState("SiteServerTime", SiteServerTime);

                TimeSpan datediff = HostServerTime - SiteServerTime;
                offSetHours = (int) datediff.TotalSeconds;
                //if (offSetHours == 0
                //    && hostTimeZone.BaseUtcOffset != siteTimeZone.BaseUtcOffset
                //    && hostTimeZone.GetUtcOffset(locUtcTime) == siteTimeZone.GetUtcOffset(locUtcTime))
                //{ //day light save switch over scenario. Goinig with base offset
                //    log.Debug("Using base UTC offset");
                //    log.LogVariableState("hostTimeZone.BaseUtcOffset", hostTimeZone.BaseUtcOffset);
                //    log.LogVariableState("siteTimeZone.BaseUtcOffset", siteTimeZone.BaseUtcOffset);
                //    TimeSpan difference = hostTimeZone.BaseUtcOffset - siteTimeZone.BaseUtcOffset;
                //    offSetHours = (int)difference.TotalSeconds;
                //    log.LogVariableState("BaseUtcOffset dff", offSetHours);
                //}
            }
            log.LogMethodExit(offSetHours);
            return offSetHours;
        }

            /// <summary>
        /// GetBussinessOffSetHours() method
        /// </summary>
        /// <returns> returns int hours</returns>
        public int GetBussinessOffSetHours()
        {
            log.LogMethodEntry();
            TimeZoneDatahandler timeZoneDatahandler = new TimeZoneDatahandler();
            int returnValue = timeZoneDatahandler.GetBussinessOffSetHours();
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}

/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - SiteDateTime class
 *
 **************
 ** Version Log
 **************
 * Version     Date               Modified By            Remarks
 *********************************************************************************************
 *2.150.0      09-Mar-2022       Lakshminarayana         Created : SiteDateTime Enhancement
 ********************************************************************************************/
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.ViewContainer
{
    public class SiteDateTime
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly object locker = new object();
        private static Timer refreshTimer;
        private static TimeSpan difference;
        private static bool canCommunicateWithServer;
        private static int refreshFrequecyInSeconds = 10;
        private const int maxRefreshFrequencyInSeconds = 300;
        private const int minRefreshFrequencyInSeconds = 60;

        static SiteDateTime()
        {
            log.LogMethodEntry();
            try
            {
                Difference = GetSiteDateTimeDifference(false);
                CanCommunicateWithServer = true;
            }
            catch (Exception)
            {
                CanCommunicateWithServer = false;
            }
            refreshTimer = new Timer(refreshFrequecyInSeconds * 1000);
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            refreshTimer.Stop();
            TimeSpan currentDifference = GetSiteDateTimeDifference(false);
            TimeSpan previousDifference = Difference;
            Difference = currentDifference;
            TimeSpan delta = previousDifference.Subtract(currentDifference);
            log.Debug("currentDifference: " + currentDifference.TotalMilliseconds);
            log.Debug("previousDifference: " + previousDifference.TotalMilliseconds);
            log.Debug("delta: " + delta.TotalMilliseconds);
            log.Debug("before refreshFrequecyInSeconds: " + refreshFrequecyInSeconds);
            if (Math.Abs(delta.TotalSeconds) > 1)
            {
                refreshFrequecyInSeconds = Math.Max(minRefreshFrequencyInSeconds, refreshFrequecyInSeconds / 2);
            }
            else
            {
                refreshFrequecyInSeconds = Math.Min(maxRefreshFrequencyInSeconds, refreshFrequecyInSeconds * 2);
            }
            log.Debug("after refreshFrequecyInSeconds: " + refreshFrequecyInSeconds);
            refreshTimer.Interval = refreshFrequecyInSeconds * 1000;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        /// <summary>
        /// Recalculates the difference
        /// </summary>
        public static void Rebuild()
        {
            log.LogMethodEntry();
            try
            {
                Difference = GetSiteDateTimeDifference(true);
                CanCommunicateWithServer = true;
            }
            catch (Exception)
            {
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the UTCTime 
        /// </summary>
        /// <returns>UTCTime</returns>
        public static DateTime GetUTCTime()
        {
            log.LogMethodEntry();
            if(CanCommunicateWithServer == false)
            {
                throw new ParafaitApplicationException("Unable to communicate with the server");
            }
            DateTime result = DateTime.UtcNow.Add(Difference);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the SiteDateTime 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <returns>SiteDateTime</returns>
        public static DateTime GetSiteDateTime(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            DateTime result = GetSiteDateTime(executionContext.SiteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the SiteDateTime 
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <returns>SiteDateTime</returns>
        public static DateTime GetSiteDateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime utcTime = GetUTCTime();
            string siteTimeZoneName = SiteViewContainerList.GetTimeZoneName(siteId);
            if (string.IsNullOrWhiteSpace(siteTimeZoneName))
            {
                siteTimeZoneName = TimeZoneInfo.Local.Id;
            }
            TimeZoneInfo siteTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);
            DateTime result = TimeZoneInfo.ConvertTimeFromUtc(utcTime, siteTimeZone);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Calculates the time difference 
        /// </summary>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns></returns>
        private static TimeSpan GetSiteDateTimeDifference(bool rebuildCache)
        {
            log.LogMethodEntry(rebuildCache);
            ExecutionContext executionContext = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
            ISiteUseCases siteUseCases = SiteUseCaseFactory.GetSiteUseCases(executionContext);
            DateTime serverUtcDateTime;
            using (NoSynchronizationContextScope.Enter())
            {
                Task<DateTime> task = siteUseCases.GetUTCDateTime(rebuildCache);
                task.Wait();
                serverUtcDateTime = task.Result;
            }
            TimeSpan result = serverUtcDateTime.Subtract(DateTime.UtcNow);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get/Set method of the difference field
        /// </summary>
        private static TimeSpan Difference
        {
            get
            {
                lock (locker)
                {
                    return difference;
                }
            }

            set
            {
                lock (locker)
                {
                    difference = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the canCommunicateWithServer field
        /// </summary>
        private static bool CanCommunicateWithServer
        {
            get
            {
                lock (locker)
                {
                    return canCommunicateWithServer;
                }
            }

            set
            {
                lock (locker)
                {
                    canCommunicateWithServer = value;
                }
            }
        }
    }
}

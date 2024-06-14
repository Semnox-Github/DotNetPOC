using System;
using System.Data;
using System.Timers;

namespace Semnox.Core.Utilities
{
    public class ServerDateTime
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly object locker = new object();
        private static Timer refreshTimer;
        private static TimeSpan difference;
        private static int refreshFrequecyInSeconds = 10;
        private const int maxRefreshFrequencyInSeconds = 300;
        private const int minRefreshFrequencyInSeconds = 10;
        private static string timeZone;
        static ServerDateTime()
        {
            log.LogMethodEntry();
            Difference = GetServerDateTimeDifference();
            timeZone = GetServerTimeZone();
            refreshTimer = new Timer(refreshFrequecyInSeconds * 1000);
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static string GetServerTimeZone()
        {
            log.LogMethodEntry();
            string serverTimeZone;
            try
            {
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                string selectQuery = @"DECLARE @TimeZone VARCHAR(50) ;
                                EXEC MASTER.dbo.xp_regread 'HKEY_LOCAL_MACHINE', 'SYSTEM\CurrentControlSet\Control\TimeZoneInformation', 'TimeZoneKeyName', @TimeZone OUT ;
                                SELECT @TimeZone ";
                serverTimeZone = dataAccessHandler.executeScalar(selectQuery, null, null).ToString();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving server time zone", ex);
                serverTimeZone = TimeZoneInfo.Local.Id;
            }
            log.LogMethodExit(serverTimeZone);
            return serverTimeZone;
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            refreshTimer.Stop();
            TimeSpan currentDifference = GetServerDateTimeDifference();
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

        public static DateTime Now
        {
            get
            {
                return DateTime.Now.Add(Difference);

            }
        }

        public static string TimeZone
        {
            get
            {
                return timeZone;

            }
        }

        private static TimeSpan GetServerDateTimeDifference()
        {
            log.LogMethodEntry();
            DateTime dbServerDateTime = DateTime.Now;
            try
            {
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                string selectQuery = @"SELECT GETDATE() as ServerDateTime";
                DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, null);
                dbServerDateTime = Convert.ToDateTime(table.Rows[0]["ServerDateTime"]);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving db server current date time", ex);
                dbServerDateTime = DateTime.Now;
            }

            log.Debug("Server DateTime :" + dbServerDateTime);
            log.Debug("DateTime.Now :" + DateTime.Now);
            TimeSpan result = dbServerDateTime.Subtract(DateTime.Now);
            log.LogMethodExit(result);
            return result;
        }

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
    }
}

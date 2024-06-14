/********************************************************************************************
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        18-Sep-2019     Dakshakh raj       Modified : added Logs
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Reports
{

    /// <summary>
    /// CentralTimeZone class
    /// </summary>
    public class CentralTimeZone
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static TimeZone ServerTimeZone = TimeZone.CurrentTimeZone;
        static TimeSpan ts = ServerTimeZone.GetUtcOffset(DateTime.Now);
        static int ServerOffset = ts.Hours * 60 + ts.Minutes;
        //static int FinalOffset = Common.offset;
        static int TimeZoneOffset=0;


        /// <summary>
        /// getLocalTime method
        /// </summary>
        /// <param name="ServerTime">ServerTime</param>
        /// <param name="FinalOffset">FinalOffset</param>
        /// <returns>returns DateTime</returns>
        public static DateTime getLocalTime(DateTime ServerTime, int FinalOffset)
        {
            log.LogMethodEntry(ServerTime, FinalOffset);
            log.LogMethodExit(ServerTime.AddMinutes(-1 * FinalOffset));
            return (ServerTime.AddMinutes(-1 * FinalOffset));
        }


        /// <summary>
        /// getServerTime method
        /// </summary>
        /// <param name="LocalTime">LocalTime</param>
        /// <param name="FinalOffset">FinalOffset</param>
        /// <returns>returns DateTime</returns>
        public static DateTime getServerTime(DateTime LocalTime, int FinalOffset)
        {
            log.LogMethodEntry(LocalTime, FinalOffset);
            log.LogMethodExit(LocalTime.AddMinutes(FinalOffset));
            return (LocalTime.AddMinutes(FinalOffset));
        }


        /// <summary>
        /// FinalOffset method
        /// </summary>
        /// <returns>returns int</returns>
        public static int FinalOffset()
        {
            log.LogMethodEntry();
            int LocalOffset = Convert.ToInt32(TimeZoneOffset);
            log.LogMethodExit(ServerOffset + LocalOffset);
            return (ServerOffset + LocalOffset);
        }
    }
}

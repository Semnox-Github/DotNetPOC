/********************************************************************************************
 * Project Name - Monitor Dashboard  Asset DTO
 * Description  - Data object of monitor dashboard asset  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        3-May-2016   Jeevan          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DashBoard
{
    /// <summary>
    /// This is the monitor dashboard asset summary data object class. This acts as data holder for the monitor dashboard asset summary business object
    /// </summary>
    public class MonitorAssetLogReportDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DateTime timestamp;
        string status;
        string priority;
        string logText;
        string logKey;
        string logValue;
    

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorAssetLogReportDTO()
        {
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MonitorAssetLogReportDTO(DateTime timestamp, string status, string priority,string logText, string logKey, string logValue)
        {
            log.Debug("Starts-MonitorAssetLogReportDTO(with all the data fields) Parameterized constructor.");
            this.timestamp =timestamp;
            this.status = status;
            this.priority = priority;
            this.logText = logText;
            this.logKey = logKey;
            this.logValue = logValue;
            log.Debug("Ends-MonitorAssetLogReportDTO(with all the data fields) Parameterized constructor.");
       
        }

        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        public DateTime Timestamp { get { return timestamp; } set { timestamp = value; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status { get { return status; } set { status = value; } }
        /// <summary>
        /// Get/Set method of the Priority field
        /// </summary>
        public string Priority { get { return priority; } set { priority = value; } }
        /// <summary>
        /// Get/Set method of the LogText field
        /// </summary>
        public string LogText { get { return logText; } set { logText = value; } }
        /// <summary>
        /// Get/Set method of the LogKey field
        /// </summary>
        public string LogKey { get { return logKey; } set { logKey = value; } }
        /// <summary>
        /// Get/Set method of the LogValue field
        /// </summary>
        public string LogValue { get { return logValue; } set { logValue = value; } }
  
    }
}

/********************************************************************************************
 * Project Name - Monitor Dashboard  AssetSummary DTO
 * Description  - Data object of monitor dashboard asset summary 
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
    public class MonitorAssetSummaryReportDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int siteId;
        int siteCode;
        string siteName;
        int totalAssets;
        int errorAssets;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorAssetSummaryReportDTO()
        {
            log.Debug("Starts-MonitorAssetSummaryReportDTO() default constructor.");
            siteId = -1;
            siteCode = -1;
            totalAssets = 0;
            errorAssets = 0;
            log.Debug("Ends-MonitorAssetSummaryReportDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MonitorAssetSummaryReportDTO(int siteId, int siteCode, string siteName, int totalAssets, int errorAssets)
        {
            log.Debug("Starts-MonitorAssetSummaryReportDTO(with all the data fields) Parameterized constructor.");
            this.siteId = siteId;
            this.siteCode = siteCode;
            this.siteName = siteName;
            this.totalAssets = totalAssets;
            this.errorAssets = errorAssets;
            log.Debug("Ends-MonitorAssetSummaryReportDTO(with all the data fields) Parameterized constructor.");
       
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SiteCode field
        /// </summary>
        public int SiteCode { get { return siteCode; } set { siteCode = value; } }
        /// <summary>
        /// Get/Set method of the SiteName field
        /// </summary>
        public string SiteName { get { return siteName; } set { siteName = value; } }
        /// <summary>
        /// Get/Set method of the TotalAssets field
        /// </summary>
        public int TotalAssets { get { return totalAssets; } set { totalAssets = value; } }
        /// <summary>
        /// Get/Set method of the ErrorAssets field
        /// </summary>
        public int ErrorAssets { get { return errorAssets; } set { errorAssets = value; } }
    }
}

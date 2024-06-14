using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DashBoard
{
    public class MonitorDashBoard
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorDashBoard()
        {

        }

    }


    /// <summary>
    /// Manages the list for the Monitor Dashboard 
    /// </summary>
    public class MonitorDashBoardList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Method to retrive monitor dashboard summary
        /// <param name="priority">string type parameter</param>
        /// <param name="showErrorSites">bool type parameter</param>
        /// <returns>Returns the list of MonitorAssetSummaryReportDTO </returns>
        /// </summary>
        public List<MonitorAssetSummaryReportDTO> GetMonitorSiteAssetSummary(bool showErrorSites, string priority)
        {
            log.Debug("Starts-GetMonitorSiteAssetSummary() method.");
            MonitorDashBoardDataHandler monitorDashBoardDataHandler = new MonitorDashBoardDataHandler();
            log.Debug("Ends-GetMonitorSiteAssetSummary() method by returning the result of monitorDashBoardDataHandler.GetMonitorSiteAssetSummaryList(blnShowErrorSites, strPriority) call.");
            return monitorDashBoardDataHandler.GetMonitorSiteAssetSummaryList(showErrorSites, priority);
        }


        /// <summary>
        /// Method to retrive monitor dashboard cross tab summary
        /// <param name="priority">string type parameter</param>
        /// <returns>Returns data table with MonitorAssetSummary in Cross tab fromat </returns>
        /// </summary>
        public System.Data.DataTable GetMonitorAssetSummaryCrossTab(string priority)
        {
            log.Debug("Starts-GetMonitorSiteAssetCrossTabSummary() method.");
            MonitorDashBoardDataHandler monitorDashBoardDataHandler = new MonitorDashBoardDataHandler();
            log.Debug("Ends-GetMonitorSiteAssetCrossTabSummary() method by returning the result of monitorDashBoardDataHandler.GetMonitorAssetSummaryCrossTab(priority) call.");
            return monitorDashBoardDataHandler.GetMonitorAssetSummaryCrossTab(priority);
        }


        /// <summary>
        /// Method to retrive Monitored assets for the site 
        /// <param name="siteId">int type parameter</param>
        /// <param name="priority">string type parameter</param>
        /// <param name="showErrorSites">bool type parameter</param>
        /// <returns>Returns the list of MonitorAssetsReportDTO </returns>
        /// </summary>
        public List<MonitorAssetsReportDTO> GetMonitorAssets(int siteId, string priority, bool showErrorSites)
        {
            log.Debug("Starts-GetMonitorAssets(intSiteId,strPriority,blnShowErrorSites) method.");
            MonitorDashBoardDataHandler monitorDashBoardDataHandler = new MonitorDashBoardDataHandler();
            log.Debug("Ends-GetMonitorAssets(intSiteId,strPriority,blnShowErrorSites) method by returning the result of monitorDashBoardDataHandler.GetMonitorAssetsList(intSiteId,strPriority,blnShowErrorSites) call.");
            return monitorDashBoardDataHandler.GetMonitorAssetsList(siteId, priority, showErrorSites);
        }


        /// <summary>
        /// Method to retrive Monitor log 
        /// <param name="monitorId">int type parameter</param>
        /// <returns>Returns the list of MonitorAssetLogReportDTO </returns>
        /// </summary>
        public List<MonitorAssetLogReportDTO> GetMonitorAssetLog(int monitorId, int maxRows)
        {
            log.Debug("Starts-GetMonitorAssetLog(intMonitorId) method.");
            MonitorDashBoardDataHandler monitorDashBoardDataHandler = new MonitorDashBoardDataHandler();
            log.Debug("Ends-GetMonitorAssetLog(searchParameters) method by returning the result of monitorDashBoardDataHandler.GetMonitorAssetLogList(intMonitorId,intMaxRows) call.");
            return monitorDashBoardDataHandler.GetMonitorAssetLogList(monitorId, maxRows);
        }



    }


}

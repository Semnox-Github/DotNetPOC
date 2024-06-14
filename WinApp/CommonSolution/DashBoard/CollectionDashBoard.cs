using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DashBoard
{
    public class CollectionDashBoard
    {
       Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public CollectionDashBoard()
        {
        }

    }


    /// <summary>
    /// Manages the list of weeekly collection Report
    /// </summary>
    public class CollectionDashBoardList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the weekly site collection list summary
        /// </summary>
        public WeeklyCollectionDTO GetWeeklyCollectionList(int roleId = -1)
        {
            log.LogMethodEntry(roleId);
            log.Debug("Starts-GetWeeklyCollectionList() method.");
            CollectionDashBoardDataHandler weeklyCollectionReportDataHandler = new CollectionDashBoardDataHandler();
            log.Debug("Ends-GetWeeklyCollectionList() method by returning the result of weeklyCollectionReportDataHandler.GetWeeklyCollectionList() call.");
            log.LogMethodExit(roleId);
            return weeklyCollectionReportDataHandler.GetWeeklyCollectionList(roleId);
        }


        /// <summary>
        /// <param name="siteId">int type parameter</param>
        /// <param name="posMachine">string type parameter</param>
        /// Returns the weekly collection list POS summary
        /// </summary>
        public List<WeeklyCollectionPOSReportDTO> GetWeeklyCollectionPOSList(int siteId, string posMachine)
        {
            //List<KeyValuePair<WeeklyCollectionPOSReportDTO.SearchByWeeklyCollectionPOSTypeParameters, string>> searchParameters
            log.Debug("Starts-GetWeeklyCollectionPOSList(searchParameters) method.");
            CollectionDashBoardDataHandler weeklyCollectionReportDataHandler = new CollectionDashBoardDataHandler();
            log.Debug("Ends-GetWeeklyCollectionPOSList(searchParameters) method by returning the result of weeklyCollectionReportDataHandler.GetWeeklyCollectionPOSList(searchParameters) call.");
            return weeklyCollectionReportDataHandler.GetWeeklyCollectionPOSList(siteId, posMachine);
        }


        //public DateTime GetServerBusinessTime()
        //{
        //    CollectionDashBoardDataHandler weeklyCollectionReportDataHandler = new CollectionDashBoardDataHandler();
        //    return weeklyCollectionReportDataHandler.getServerBusinessTime();
        //}

    }

}

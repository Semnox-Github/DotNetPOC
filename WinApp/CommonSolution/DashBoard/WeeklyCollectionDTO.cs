using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DashBoard
{
    public class WeeklyCollectionDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        String currentDate;
        List<WeeklyCollectionReportDTO> siteCollectionList;

        String fromDate;
        String fromDateWeek;
        String prevDayStart;
        String todayStart;
        String todayEnd;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WeeklyCollectionDTO()
        {
            log.Debug("Starts-SiteCollectionDTO() default constructor.");
            this.currentDate = "";
            this.siteCollectionList = null;
            log.Debug("Ends-SiteCollectionDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public WeeklyCollectionDTO(String currentDate, List<WeeklyCollectionReportDTO> siteCollectionList)
        {
            log.Debug("Starts-SiteCollectionDTO(with all the data fields) Parameterized constructor.");
            this.currentDate = currentDate;
            this.siteCollectionList = siteCollectionList;
            log.Debug("Ends-SiteCollectionDTO(with all the data fields) Parameterized constructor.");
        }

        public WeeklyCollectionDTO(String currentDate,  String fromDate, String fromDateWeek, String prevDayStart, String todayStart, String todayEnd, List<WeeklyCollectionReportDTO> siteCollectionList)
        {
            
            this.currentDate = currentDate;
            this.siteCollectionList = siteCollectionList;

            this.fromDate = fromDate;
            this.fromDateWeek = fromDateWeek;
            this.prevDayStart = prevDayStart;
            this.todayStart = todayStart;
            this.todayEnd = todayEnd;
        }


        /// <summary>
        /// Get/Set method of the currentDate field
        /// </summary>
        public String CurrentDate { get { return currentDate; } set { currentDate = value; } }

        public String FromDate { get { return fromDate; } set { fromDate = value; } }

        public String FromDateWeek { get { return fromDateWeek; } set { fromDateWeek = value; } }

        public String PrevDayStart { get { return prevDayStart; } set { prevDayStart = value; } }

        public String TodayStart { get { return todayStart; } set { todayStart = value; } }

        public String TodayEnd { get { return todayEnd; } set { todayEnd = value; } }

        /// <summary>
        /// Get/Set method of the siteCollectionList field
        /// </summary>
        public List<WeeklyCollectionReportDTO> SiteCollectionList { get { return siteCollectionList; } set { siteCollectionList = value; } }
    }
}

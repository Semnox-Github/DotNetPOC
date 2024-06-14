/********************************************************************************************
 * Project Name - Weekly Collection POS  DTO
 * Description  - Data object of monitor  Weekly Collection POS
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
    /// This is the Weekly collection POS Report data object class. This acts as data holder for the Tablet dashboard Weekly collection POS  business object
    /// </summary>
    public class WeeklyCollectionPOSReportDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string posmachine;
        int collectionToday;
        int collectionPreviousDay;
        int collectionWeek;
        int collectionPreviousWeek;

        /// <summary>
        /// SearchByMonitorAssetParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByWeeklyCollectionPOSTypeParameters
        {
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 0,
            /// <summary>
            /// Search by POS_MACHINE field
            /// </summary>
            POS_MACHINE = 1
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public WeeklyCollectionPOSReportDTO()
        {
            log.Debug("Starts-WeeklyCollectionPOSReportDTO() default constructor.");
            collectionToday = 0;
            collectionPreviousDay = 0;
            collectionWeek = 0;
            collectionPreviousWeek = 0;
            log.Debug("Ends-WeeklyCollectionPOSReportDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public WeeklyCollectionPOSReportDTO(string posmachine, int collectionToday, int collectionPreviousDay,
                                                    int collectionWeek, int collectionPreviousWeek)
        {
            log.Debug("Starts-WeeklyCollectionPOSReportDTO(with all the data fields) Parameterized constructor.");
            this.posmachine = posmachine;
            this.collectionToday = collectionToday;
            this.collectionPreviousDay = collectionPreviousDay;
            this.collectionWeek = collectionWeek;
            this.collectionPreviousWeek = collectionPreviousWeek;
            log.Debug("Ends-WeeklyCollectionPOSReportDTO(with all the data fields) Parameterized constructor.");

        }

        /// <summary>
        /// Get/Set method of the Site_Name field
        /// </summary>
        public string PosMachine { get { return posmachine; } set { posmachine = value; } }
        /// <summary>
        /// Get/Set method of the CollectionToday field
        /// </summary>
        public int CollectionToday { get { return collectionToday; } set { collectionToday = value; } }
        /// <summary>
        /// Get/Set method of the CollectionPreviousDay field
        /// </summary>
        public int CollectionPreviousDay { get { return collectionPreviousDay; } set { collectionPreviousDay = value; } }
        /// <summary>
        /// Get/Set method of the CollectionWeek field
        /// </summary>
        public int CollectionWeek { get { return collectionWeek; } set { collectionWeek = value; } }
        /// <summary>
        /// Get/Set method of the CollectionPreviousWeek field
        /// </summary>
        public int CollectionPreviousWeek { get { return collectionPreviousWeek; } set { collectionPreviousWeek = value; } }
    }


}

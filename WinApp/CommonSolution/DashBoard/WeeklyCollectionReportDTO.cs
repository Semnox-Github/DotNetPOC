/********************************************************************************************
 * Project Name - Weekly Collection  DTO
 * Description  - Data object of monitor  Weekly Collection 
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
    /// This is the Weekly collection Report data object class. This acts as data holder for the Tablet dashboard Weekly collection business object
    /// </summary>
    public class WeeklyCollectionReportDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int siteId;
        string siteName;
        int collectionToday;
        int collectionPreviousDay;
        int collectionWeek;
        int collectionPreviousWeek;
        int gamePlayToday;
        int gamePlayPreviousDay;
        int gamePlayWeek;
        int gamePlayPreviousWeek;
        public List<WeeklyCollectionPOSReportDTO> WeeklyCollectionPOS;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WeeklyCollectionReportDTO()
        {
            log.Debug("Starts-WeeklyCollectionReportDTO() default constructor.");
            siteId = -1;
            collectionToday = 0;
            collectionPreviousDay = 0;
            collectionWeek = 0;
            collectionPreviousWeek = 0;
            gamePlayToday = 0;
            gamePlayPreviousDay = 0;
            gamePlayWeek = 0;
            gamePlayPreviousWeek = 0;
            WeeklyCollectionPOS = new List<WeeklyCollectionPOSReportDTO>();
            log.Debug("Ends-WeeklyCollectionReportDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public WeeklyCollectionReportDTO(int siteId, string siteName, int collectionToday, int collectionPreviousDay,
                                                                      int collectionWeek, int collectionPreviousWeek,
                                                                      int gamePlayToday, int gamePlayPreviousDay,
                                                                      int gamePlayWeek,int gamePlayPreviousWeek)
        {
            log.Debug("Starts-WeeklyCollectionReportDTO(with all the data fields) Parameterized constructor.");
            this.siteId = siteId;
            this.siteName = siteName;
            this.collectionToday = collectionToday;
            this.collectionPreviousDay = collectionPreviousDay;
            this.collectionWeek = collectionWeek;
            this.collectionPreviousWeek = collectionPreviousWeek;
            this.gamePlayToday = gamePlayToday;
            this.gamePlayPreviousDay = gamePlayPreviousDay;
            this.gamePlayWeek = gamePlayWeek;
            this.gamePlayPreviousWeek = gamePlayPreviousWeek;
            WeeklyCollectionPOS = new List<WeeklyCollectionPOSReportDTO>();
            log.Debug("Ends-WeeklyCollectionReportDTO(with all the data fields) Parameterized constructor.");

        }

        /// <summary>
        /// Get/Set method of the Site_Id field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Site_Name field
        /// </summary>
        public string SiteName { get { return siteName; } set { siteName = value; } }
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
        /// <summary>
        /// Get/Set method of the GamePlayToday field
        /// </summary>
        public int GamePlayToday { get { return gamePlayToday; } set { gamePlayToday = value; } }
        /// <summary>
        /// Get/Set method of the GamePlayPreviousDay field
        /// </summary>
        public int GamePlayPreviousDay { get { return gamePlayPreviousDay; } set { gamePlayPreviousDay = value; } }
        /// <summary>
        /// Get/Set method of the GamePlayWeek field
        /// </summary>
        public int GamePlayWeek { get { return gamePlayWeek; } set { gamePlayWeek = value; } }
        /// <summary>
        /// Get/Set method of the GamePlayWeek field
        /// </summary>
        public int GamePlayPreviousWeek { get { return gamePlayPreviousWeek; } set { gamePlayPreviousWeek = value; } }

    }
}

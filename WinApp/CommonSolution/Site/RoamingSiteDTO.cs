/********************************************************************************************
 * Project Name - Site
 * Description  - Data object of RoamingSite
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0       21-Dec-2020   Lakshminarayana    Created for POS UI Redesign.
 *********************************************************************************************/
using System;

namespace Semnox.Parafait.Site
{
    public class RoamingSiteDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Roaming site Id field
            /// </summary>
            ROAMING_SITE_ID,

            /// <summary>
            /// Search by  Auto roam field
            /// </summary>
            AUTO_ROAM,

        }


        private int id;
        private int roamingSiteId;
        private string siteName;
        private string siteAddress;
        private DateTime? lastUploadTime;
        private bool autoRoam;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public RoamingSiteDTO()
        {
            log.LogMethodEntry();
            id = -1;
            roamingSiteId = -1;
            siteName = string.Empty;
            siteAddress = string.Empty;
            autoRoam = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public RoamingSiteDTO(int id, int roamingSiteId, string siteName, string siteAddress, DateTime? lastUploadTime, bool autoRoam)
            : this()
        {
            log.LogMethodEntry(id, roamingSiteId, siteName, siteAddress, lastUploadTime, autoRoam);
            this.id = id;
            this.roamingSiteId = roamingSiteId;
            this.siteName = siteName;
            this.siteAddress = siteAddress;
            this.lastUploadTime = lastUploadTime;
            this.autoRoam = autoRoam;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with All the data fields.
        /// </summary>
        public RoamingSiteDTO(int id, int roamingSiteId, string siteName, string siteAddress, DateTime? lastUploadTime, bool autoRoam,
                              string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(id, roamingSiteId, siteName, siteAddress, lastUploadTime, autoRoam)
        {
            log.LogMethodEntry(id, roamingSiteId, siteName, siteAddress, lastUploadTime, autoRoam, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="roamingSiteDTO"></param>
        public RoamingSiteDTO(RoamingSiteDTO roamingSiteDTO)
            : this()
        {
            log.LogMethodEntry(roamingSiteDTO);
            this.id = roamingSiteDTO.id;
            this.roamingSiteId = roamingSiteDTO.roamingSiteId;
            this.siteName = roamingSiteDTO.siteName;
            this.siteAddress = roamingSiteDTO.siteAddress;
            this.lastUploadTime = roamingSiteDTO.lastUploadTime;
            this.autoRoam = roamingSiteDTO.autoRoam;
            this.createdBy = roamingSiteDTO.createdBy;
            this.creationDate = roamingSiteDTO.creationDate;
            this.lastUpdatedBy = roamingSiteDTO.lastUpdatedBy;
            this.lastUpdateDate = roamingSiteDTO.lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }
        /// <summary>
        /// Get/Set method of the RoamingSiteId field
        /// </summary>
        public int RoamingSiteId { get { return roamingSiteId; } set { this.IsChanged = true; roamingSiteId = value; } }

        /// <summary>
        /// Get/Set method of the siteName field
        /// </summary>
        public string SiteName { get { return siteName; } set { this.IsChanged = true; siteName = value; } }

        /// <summary>
        /// Get/Set method of the siteAddress field
        /// </summary>
        public string SiteAddress { get { return siteAddress; } set { this.IsChanged = true; siteAddress = value; } }

        /// <summary>
        /// Get/Set method of the lastUploadTime field
        /// </summary>
        public DateTime? LastUploadTime { get { return lastUploadTime; } set { this.IsChanged = true; lastUploadTime = value; } }

        /// <summary>
        /// Get/Set method of the RecurType field
        /// </summary>
        public bool AutoRoam { get { return autoRoam; } set { this.IsChanged = true; autoRoam = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}

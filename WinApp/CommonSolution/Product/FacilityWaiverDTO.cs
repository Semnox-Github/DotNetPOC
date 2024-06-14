/********************************************************************************************
 * Project Name - FacilityWaiver DTO
 * Description  - Data object of FacilityWaiver
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       26-Sep-2019    Deeksha       Created for waiver phase 2
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class FacilityWaiverDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {

            /// <summary>
            /// Search by FACILITY WAIVER ID field
            /// </summary>
            FACILITY_WAIVER__ID,

            /// <summary>
            /// Search by  FACILITY ID field
            /// </summary>
            FACILITY_ID,
            /// <summary>
            /// Search by  FACILITY ID field
            /// </summary>
            FACILITY_ID_LIST,
            /// <summary>
            /// Search by WAIVER SET ID field
            /// </summary>
            WAIVER_SET_ID,

            /// <summary>
            /// Search by EFFECTIVE FROM field
            /// </summary>
            EFFECTIVE_FROM,

            /// <summary>
            /// Search by EFFECTIVE TO field
            /// </summary>
            EFFECTIVE_TO,

            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID
        }

        private int facilityWaiverId;
        private int facilityId;
        private int waiverSetId;
        private DateTime? effectiveFrom;
        private DateTime? effectiveTo;
        private bool isActive;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public FacilityWaiverDTO()
        {
            log.LogMethodEntry();
            facilityWaiverId = -1;
            facilityId = -1;
            waiverSetId = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with required data fields.
        /// </summary>
        public FacilityWaiverDTO( int facilityWaiverId, int facilityId, int waiverSetId, DateTime? effectiveFrom, DateTime? effectiveTo, bool isActive)
            :this()
        {
            log.LogMethodEntry(facilityWaiverId, facilityId, waiverSetId, effectiveFrom, effectiveTo, isActive);
            this.facilityWaiverId = facilityWaiverId;
            this.facilityId = facilityId;
            this.waiverSetId = waiverSetId;
            this.effectiveFrom = effectiveFrom;
            this.effectiveTo = effectiveTo;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with all the parameter.
        /// </summary>
        public FacilityWaiverDTO(int facilityWaiverId, int facilityId, int waiverSetId, DateTime? effectiveFrom, DateTime? effectiveTo,
                                     bool isActive, string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy,
                                     DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(facilityWaiverId, facilityId, waiverSetId, effectiveFrom, effectiveTo, isActive)
        {
            log.LogMethodEntry(facilityWaiverId, facilityId, waiverSetId, effectiveFrom, effectiveTo, isActive, guid,  siteId,  synchStatus,  masterEntityId,  createdBy,
                                     creationDate, lastUpdatedBy, lastUpdateDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the FacilityWaiverId field
        /// </summary>
        [DisplayName("FacilityWaiverId")]
        public int FacilityWaiverId { get { return facilityWaiverId; } set { facilityWaiverId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        [DisplayName("FacilityId")]
        public int FacilityId { get { return facilityId; } set { facilityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetId field
        /// </summary>
        [DisplayName("WaiverSetId")]
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EffectiveFrom field
        /// </summary>
        [DisplayName("EffectiveFrom")]
        public DateTime? EffectiveFrom { get { return effectiveFrom; } set { effectiveFrom = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EffectiveFrom field
        /// </summary>
        [DisplayName("EffectiveTo")]
        public DateTime? EffectiveTo { get { return effectiveTo; } set { effectiveTo= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; ; } }

        /// <summary>
        /// Get/Set method of the ValueInTickets field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastModifiedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the lastModifiedDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || facilityWaiverId < 0;
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
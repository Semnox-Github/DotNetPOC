/* Project Name - ReservationCoreDTO Programs 
* Description  - Data object of the FacilitySeatLayoutDTO
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.70        22-Feb-2019    Akshay Gulaganji     Added SearchByFacilitySeatLayoutParameter and IsChanged property field
*2.70        26-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
*2.70        29-Jun-2019    Akshay Gulaganji     Added isActive property
*2.70        28-Feb-2020    Girish Kundar        Modified : 3 Tier changes for API
********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class FacilitySeatLayoutDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByFacilitySeatLayoutParameter enum controls the search fields, this can be expanded to include additional fields
        /// </summary> 
        public enum SearchByFacilitySeatLayoutParameter
        {
            /// <summary>
            /// Search by  FACILITY_ID field
            /// </summary>
            FACILITY_ID,
            /// <summary>
            /// Search by  FACILITY_ID field
            /// </summary>
            FACILITY_ID_LIST,
            /// <summary>
            /// Search by  TYPE field
            /// </summary>
            TYPE,
            /// <summary>
            /// Search by  LAYOUT_ID field
            /// </summary>
            LAYOUT_ID,
            /// <summary>
            /// Search by  ROW_COLUMN_INDEX field
            /// </summary>
            ROW_COLUMN_INDEX,
            /// <summary>
            /// Search by  ACTIVE_FLAG field
            /// </summary>
            HAS_SEATS,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE
        }

        private int layoutId;
        private int facilityId;
        private string rowColumnName;
        private char type;
        private int rowColumnIndex;
        private char hasSeats;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilitySeatLayoutDTO()
        {
            log.LogMethodEntry();
            this.layoutId = -1;
            this.facilityId = -1;
            this.rowColumnName = "";
            this.site_id = -1;
            this.hasSeats = 'Y';
            this.isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        public FacilitySeatLayoutDTO(int layoutId, int facilityId, string rowColumnName, char type, int rowColumnIndex,
                                       char hasSeats, bool isActive)
            : this()
        {
            log.LogMethodEntry(layoutId, facilityId, rowColumnName, type, rowColumnIndex, hasSeats, isActive);
            this.layoutId = layoutId;
            this.facilityId = facilityId;
            this.rowColumnName = rowColumnName;
            this.type = type;
            this.rowColumnIndex = rowColumnIndex;
            this.hasSeats = hasSeats;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized constructor with all the fields
        /// </summary>
        public FacilitySeatLayoutDTO(int layoutId, int facilityId, string rowColumnName, char type, int rowColumnIndex,
                                   char hasSeats, string guid, bool synchStatus, int site_id, int masterEntityId,
                                   bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                   DateTime lastUpdateDate)
            : this(layoutId, facilityId, rowColumnName, type, rowColumnIndex, hasSeats, isActive)
        {
            log.LogMethodEntry(layoutId, facilityId, rowColumnName, type, rowColumnIndex, hasSeats, guid,
                               synchStatus, site_id, masterEntityId, isActive,
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LayoutId field
        /// </summary>
        [DisplayName("LayoutId")]
        [DefaultValue(-1)]
        public int LayoutId { get { return layoutId; } set { layoutId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        [DisplayName("FacilityId")]
        [DefaultValue(-1)]
        public int FacilityId { get { return facilityId; } set { facilityId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RowColumnName field
        /// </summary>
        [DisplayName("RowColumnName")]
        [DefaultValue("")]
        public string RowColumnName { get { return rowColumnName; } set { rowColumnName = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public char Type { get { return type; } set { type = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RowColumnIndex field
        /// </summary>
        [DisplayName("RowColumnIndex")]
        [DefaultValue(-1)]
        public int RowColumnIndex { get { return rowColumnIndex; } set { rowColumnIndex = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the HasSeats field
        /// </summary>
        [DisplayName("HasSeats")]
        public char HasSeats { get { return hasSeats; } set { hasSeats = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [DefaultValue(-1)]
        public int SiteId { get { return site_id; } set { site_id = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [DefaultValue(-1)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

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
                    return notifyingObjectIsChanged || layoutId < 0;
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

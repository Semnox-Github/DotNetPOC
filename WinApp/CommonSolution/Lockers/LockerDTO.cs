/********************************************************************************************
 * Project Name - Lockers DTO
 * Description  - Data object of locker DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera     Created 
 *2.40.1      12-07-2018    Raghuveera     Innovate locker to block the locker card added field cardOverrideSequence
 *2.70.2        19-Jul-2019   Dakshakh raj   Modified : Added Parameterized costrustor,
 *                                                    CreatedBy and CreationDate fields
 *2.80        19-Dec-2019   Lakshminarayana  Modified signage locker layout enhancement 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// This is the locker data object class. This acts as data holder for the locker business object
    /// </summary>
    public class LockerDTO
    {
        /// <summary>
        /// SearchByLockersParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByLockersParameters
        {
            /// <summary>
            /// Search by LOCKER ID field
            /// </summary>
            LOCKER_ID,
            
            /// <summary>
            /// Search by LOCKER NAME field
            /// </summary>
            LOCKER_NAME,
            
            /// <summary>
            /// Search by PANEL ID field
            /// </summary>
            PANEL_ID,
            
            /// <summary>
            /// Search by IDENTIFIRE field
            /// </summary>
            IDENTIFIRE,
           
            /// <summary>
            /// Search by IS DISABLED field
            /// </summary>
            IS_DISABLED,
           
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
           
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
           
            /// <summary>
            /// Search by LOCKER STATUS field
            /// </summary>
            LOCKER_STATUS,
            
            /// <summary>
            /// Search by BATTERY STATUS field
            /// </summary>
            BATTERY_STATUS,
           
            /// <summary>
            /// Search by Zone ID LIST field
            /// </summary>
            Zone_ID_LIST
        }
        private int lockerId;
        private string lockerName;
        private int panelId;
        private int rowIndex;
        private int columnIndex;
        private int identifier;
        private bool isActive;
        private bool isDisabled;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string lockerStatus;
        private DateTime statusChangeDate;
        private int batteryStatus;
        DateTime batteryStatusChangeDate;
        private int cardOverrideSequence;
        private string createdBy;
        private DateTime creationDate;
        private LockerAllocationDTO lockerAllocated;
        private int? positionX;
        private int? positionY;
        private string externalIdentifier;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Contructor
        /// </summary>
        public LockerDTO()
        {
            log.LogMethodEntry();
            panelId = -1;
            rowIndex = -1;
            lockerId = -1;
            columnIndex = -1;
            identifier = -1;
            masterEntityId = -1;
            isDisabled = false;
            isActive = true;
            siteId = -1;
            batteryStatus = 0;
            lockerStatus = "O";
            cardOverrideSequence = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required fields
        /// </summary>        
        public LockerDTO(int lockerId, string lockerName, int panelId, int rowIndex, int columnIndex, int identifier,
                          bool isActive, bool isDisabled, string lockerStatus, DateTime statusChangeDate, int batteryStatus,
                          DateTime batteryStatusChangeDate, int cardOverrideSequence, int? positionX, int? positionY, string externalIdentifier)
            : this()
        {
            log.LogMethodEntry(lockerId, lockerName, panelId, rowIndex, columnIndex, identifier,
                           isActive, isDisabled, lockerStatus, statusChangeDate, batteryStatus,
                           batteryStatusChangeDate, cardOverrideSequence, positionX, positionY, externalIdentifier);
            this.lockerId = lockerId;
            this.lockerName = lockerName;
            this.panelId = panelId;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.identifier = identifier;
            this.isActive = isActive;
            this.isDisabled = isDisabled;
            this.lockerStatus = lockerStatus;
            this.statusChangeDate = statusChangeDate;
            this.batteryStatus = batteryStatus;
            this.batteryStatusChangeDate = batteryStatusChangeDate;
            this.cardOverrideSequence = cardOverrideSequence;
            this.positionX = positionX;
            this.positionY = positionY;
            this.externalIdentifier = externalIdentifier;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>        
        public LockerDTO(int lockerId, string lockerName, int panelId, int rowIndex, int columnIndex, int identifier,
                          bool isActive, bool isDisabled, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                          string guid, bool synchStatus, int masterEntityId, string lockerStatus, DateTime statusChangeDate, int batteryStatus,
                          DateTime batteryStatusChangeDate, int cardOverrideSequence, string createdBy, DateTime creationDate, int? positionX, int? positionY,
                          string externalIdentifier)
            : this(lockerId, lockerName, panelId, rowIndex, columnIndex, identifier, isActive, isDisabled, lockerStatus, statusChangeDate, batteryStatus,
                  batteryStatusChangeDate, cardOverrideSequence, positionX, positionY, externalIdentifier)
        {
            log.LogMethodEntry(lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus, masterEntityId, createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Get/Set method of the LockerId field
        /// </summary>
        [DisplayName("LockerId")]
        [ReadOnly(true)]
        public int LockerId { get { return lockerId; } set { lockerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LockerName field
        /// </summary>
        [DisplayName("Locker Name")]
        public string LockerName { get { return lockerName; } set { lockerName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PanelId field
        /// </summary>
        [DisplayName("PanelId")]
        [ReadOnly(true)]
        public int PanelId { get { return panelId; } set { panelId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RowIndex field
        /// </summary>
        [DisplayName("RowIndex")]
        public int RowIndex { get { return rowIndex; } set { rowIndex = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ColumnIndex field
        /// </summary>
        [DisplayName("ColumnIndex")]
        public int ColumnIndex { get { return columnIndex; } set { columnIndex = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Identifier field
        /// </summary>
        [DisplayName("Identifier")]
        public int Identifier { get { return identifier; } set { identifier = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsDisabled field
        /// </summary>
        [DisplayName("IsDisabled")]
        public bool IsDisabled { get { return isDisabled; } set { isDisabled = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        [DisplayName("LastUpdatedUserId")]
        [Browsable(false)]
        public string LastUpdatedUserId { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LockerStatus field
        /// </summary>
        [DisplayName("Locker Status")]
        [ReadOnly(true)]
        public string LockerStatus { get { return lockerStatus; } set { lockerStatus = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the statusChangeDate field
        /// </summary>
        [DisplayName("Status Changed On")]
        [ReadOnly(true)]
        public DateTime StatusChangeDate { get { return statusChangeDate; } set { statusChangeDate = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the BatteryStatus field
        /// </summary>
        [DisplayName("Battery Status")]
        [ReadOnly(true)]
        public int BatteryStatus { get { return batteryStatus; } set { batteryStatus = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the BatteryStatusChangeDate field
        /// </summary>
        [DisplayName("Battery Status Changed On")]
        [ReadOnly(true)]
        public DateTime BatteryStatusChangeDate { get { return batteryStatusChangeDate; } set { batteryStatusChangeDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardOverrideSequence field
        /// </summary>
        [DisplayName("Card Override Sequence")]
        [ReadOnly(true)]
        public int CardOverrideSequence { get { return cardOverrideSequence; } set { cardOverrideSequence = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Locker Allocated field
        /// </summary>
        public LockerAllocationDTO LockerAllocated { get { return lockerAllocated; } set { lockerAllocated = value; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || lockerId < 0;
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
        /// Get method of the positionX field
        /// </summary>
        [DisplayName("Position X")]
        public int? PositionX
        {
            get { return positionX; }
            set { positionX = value; IsChanged = true;}
        }

        /// <summary>
        /// Get method of the positionY field
        /// </summary>
        [DisplayName("Position Y")]
        public int? PositionY
        {
            get { return positionY; }
            set { positionY = value; IsChanged = true;}
        }

        /// <summary>
        /// Get/Set method of the LockerStatus field
        /// </summary>]
        public string ExternalIdentifier { get { return externalIdentifier; } set { externalIdentifier = value; this.IsChanged = true; } }

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

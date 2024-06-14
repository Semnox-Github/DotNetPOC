/********************************************************************************************
 * Project Name - LockerAllocation DTO
 * Description  - Data object of locker allocation DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera     Created 
 *2.70        18-Jul-2019   Dakshakh raj   Modified : Added Parameterized costrustor,
 *                                                    createdBy, creationDate fields
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// This is the locker allocation data object class. This acts as data holder for the locker allocation business object
    /// </summary>
    public class LockerAllocationDTO
    {
        /// <summary>
        /// SearchByLockerAllocationParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByLockerAllocationParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            
            /// <summary>
            /// Search by CARD ID field
            /// </summary>
            CARD_ID,
           
            /// <summary>
            /// Search by CARD NUMBER field
            /// </summary>
            CARD_NUMBER,
           
            /// <summary>
            /// Search by LOCKER ID field
            /// </summary>
            LOCKER_ID,
           
            /// <summary>
            /// Search by VALID FROM TIME field
            /// </summary>
            VALID_FROM_TIME,
            
            /// <summary>
            /// Search by VALID TO TIME field
            /// </summary>
            VALID_TO_TIME,
            
            /// <summary>
            /// Search by TRX ID field
            /// </summary>
            TRX_ID,
           
            /// <summary>
            /// Search by TRX LINE ID field
            /// </summary>
            TRX_LINE_ID,
           
            /// <summary>
            /// Search by REFUNDED field
            /// </summary>
            REFUNDED,
           
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
           
            /// <summary>
            /// Search by VALID BETWEEN DATE field
            /// </summary>
            VALID_BETWEEN_DATE,
            /// <summary>
            /// Search by LOCKER RETURN POLICY LIMIT field
            /// </summary>
            LOCKER_RETURN_POLICY_LIMIT
        }

        private int id;
        private int cardId;
        private string cardNumber;
        private int lockerId;
        private string issuedBy;
        private DateTime issuedTime;
        private DateTime validFromTime;
        private DateTime validToTime;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private DateTime lastUpdatedTime;
        private string lastUpdatedBy;
        private int trxId;
        private int trxLineId;
        private bool refunded;
        private int masterEntityId;
        private string zoneCode;
        private string createdBy;
        private DateTime creationDate;
        //string batteryStatus;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Contructor
        /// </summary>
        public LockerAllocationDTO()
        {
            log.LogMethodEntry();
            id = -1;
            cardId = -1;
            lockerId = -1;
            trxId = -1;
            trxLineId = -1;
            masterEntityId = -1;
            refunded = false;
            siteId = -1;
            zoneCode = "";
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with Required fields
        /// </summary>        
        public LockerAllocationDTO(int id, int cardId, string cardNumber, int lockerId, string issuedBy,
                                   DateTime issuedTime, DateTime validFromTime, DateTime validToTime,
                                   int trxId, int trxLineId, bool refunded, string zoneCode)//, string batteryStatus
            :this()
        {
            log.LogMethodEntry(id,  cardId,  cardNumber,  lockerId,  issuedBy, issuedTime,  validFromTime,  validToTime, trxId,  trxLineId,  refunded,  zoneCode);
            this.id = id;
            this.cardId = cardId;
            this.cardNumber = cardNumber;
            this.lockerId = lockerId;
            this.issuedBy = issuedBy;
            this.issuedTime = issuedTime;
            this.validFromTime = validFromTime;
            this.validToTime = validToTime;
            this.trxId = trxId;
            this.trxLineId = trxLineId;
            this.refunded = refunded;
            this.zoneCode = zoneCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>        
        public LockerAllocationDTO(int id, int cardId, string cardNumber, int lockerId, string issuedBy,
                                   DateTime issuedTime, DateTime validFromTime, DateTime validToTime,
                                   string guid, int siteId, bool synchStatus, DateTime lastUpdatedTime,
                                   string lastUpdatedBy, int trxId, int trxLineId, bool refunded,
                                   int masterEntityId, string zoneCode, string createdBy, DateTime creationDate)
           : this(id, cardId, cardNumber, lockerId, issuedBy, issuedTime, validFromTime, validToTime, trxId, trxLineId, refunded, zoneCode)
        {
            log.LogMethodEntry(guid, siteId, synchStatus, lastUpdatedTime, lastUpdatedBy, masterEntityId, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.lastUpdatedTime = lastUpdatedTime;
            this.lastUpdatedBy = lastUpdatedBy;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        [Browsable(false)]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LockerId field
        /// </summary>
        [DisplayName("LockerId")]
        public int LockerId { get { return lockerId; } set { lockerId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ValidFromTime field
        /// </summary>
        [DisplayName("Valid From Time")]
        public DateTime ValidFromTime { get { return validFromTime; } set { validFromTime = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ValidToTime field
        /// </summary>
        [DisplayName("Valid To Time")]
        public DateTime ValidToTime { get { return validToTime; } set { validToTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        [DisplayName("TrxId")]
        public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        [DisplayName("TrxLineId")]
        public int TrxLineId { get { return trxLineId; } set { trxLineId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Refunded field
        /// </summary>
        [DisplayName("Refunded")]
        public bool Refunded { get { return refunded; } set { refunded = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ZoneCode field
        /// </summary>
        [DisplayName("Zone Code")]
        public string ZoneCode { get { return zoneCode; } set { zoneCode = value; this.IsChanged = true; } }


        ///// <summary>
        ///// Get/Set method of the BatteryStatus field
        ///// </summary>
        //[DisplayName("Battery Status")]
        //public string BatteryStatus { get { return batteryStatus; } set { batteryStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the issuedBy field
        /// </summary>
        [DisplayName("Issued By")]
        [Browsable(false)]
        public string IssuedBy { get { return issuedBy; } set { issuedBy = value; } }

        /// <summary>
        /// Get method of the IssuedTime field
        /// </summary>
        [DisplayName("Issued Time")]
        [Browsable(false)]
        public DateTime IssuedTime { get { return issuedTime; } set { issuedTime = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        [DisplayName("LastUpdatedUserId")]
        [Browsable(false)]
        public string LastUpdatedUserId { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

            /// <summary>
            /// Get method of the LastUpdatedTime field
            /// </summary>
            [DisplayName("LastUpdatedTime")]
        [Browsable(false)]
        public DateTime LastUpdatedTime { get { return lastUpdatedTime; } set { lastUpdatedTime = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
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

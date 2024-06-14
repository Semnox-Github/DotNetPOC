/********************************************************************************************
 * Project Name - Locker Blocked Cards DTO 
 * Description  - Data object of Locker Blocked Cards DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        05-Aug-2017   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// This is the  Locker Blocked Cards data object class. This acts as data holder for the Locker Blocked Cards business object
    /// </summary>
    public class LockerBlockedCardsDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByLockerBlockedCardsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByLockerBlockedCardsParameters
        {
            /// <summary>
            /// Search by BLOCK ID field
            /// </summary>
            CARD_BLOCK_ID,
            
            /// <summary>
            /// Search by CARD NUMBER field
            /// </summary>
            CARD_NUMBER,            
            
            /// <summary>
            /// Search by IS ALIVE field
            /// </summary>
            IS_ACTIVE,            
            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
           
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }

        private int cardBlockId;
        private string cardNumber;        
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LockerBlockedCardsDTO()
        {
            log.LogMethodEntry();
            cardBlockId = -1;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public LockerBlockedCardsDTO(int cardBlockId, string cardNumber, bool isActive)
            :this()
        {
            log.LogMethodEntry(cardBlockId, cardNumber, isActive);
            this.cardBlockId = cardBlockId;
            this.cardNumber = cardNumber;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LockerBlockedCardsDTO(int cardBlockId, string cardNumber, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                    string guid, int siteId, bool synchStatus, int masterEntityId)
            : this(cardBlockId, cardNumber, isActive)
        {
            log.LogMethodEntry(createdBy,  creationDate,  lastUpdatedBy,  lastUpdatedDate, guid,  siteId,  synchStatus,  masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit ();
        }

        /// <summary>
        /// Get/Set method of the CardBlockId field
        /// </summary>
        [DisplayName("CardBlockId")]
        [ReadOnly(true)]
        public int CardBlockId { get { return cardBlockId; } set { cardBlockId = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
       
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
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

            /// <summary>
            /// Get/Set method of the Guid field
            /// </summary>
            [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; }  }
       
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
       
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || cardBlockId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.CardCore
{
    public class CardCreditPlusLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  CARD_CREDITPLUS_LOG_ID field
            /// </summary>
            CARD_CREDIT_PLUS_LOG_ID = 1,
            /// <summary>
            /// Search by  CARD_CREDIT_PLUS_ID field
            /// </summary>
            CARD_CREDIT_PLUS_ID = 2,
            /// <summary>
            /// CREDIT_PLUS_TYPE
            /// </summary>
            CREDIT_PLUS_TYPE = 3, 
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID = 4,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 5
        }

        int cardCreditPlusLogId;
        int cardCreditPlusId;
        double creditPlus;
        string creditPlusType;
        double creditPlusBalance;
        DateTime? playStartTime;
        string createdBy;
        DateTime creationDate;
        DateTime lastupdatedDate;
        string lastUpdatedBy;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardCreditPlusLogDTO()
        {
            log.LogMethodEntry();
            cardCreditPlusLogId = -1;
            cardCreditPlusId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CardCreditPlusLogDTO(int cardCreditPlusLogId, int cardCreditPlusId, double creditPlus, string creditPlusType, double creditPlusBalance, DateTime? playStartTime, string createdBy, DateTime creationDate, DateTime lastupdatedDate, string lastUpdatedBy, string guid, int siteId, bool synchStatus, int masterEntityId)
        {
            log.LogMethodEntry(cardCreditPlusLogId, cardCreditPlusId, creditPlus, creditPlusType, creditPlusBalance, playStartTime, createdBy, creationDate, lastupdatedDate, lastUpdatedBy, guid, siteId, synchStatus, masterEntityId);
            this.cardCreditPlusLogId = cardCreditPlusLogId;
            this.cardCreditPlusId = cardCreditPlusId;
            this.creditPlus = creditPlus;
            this.creditPlusType = creditPlusType;
            this.playStartTime = playStartTime;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.creationDate = creationDate;
            this.lastupdatedDate = lastupdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the cardCreditPlusLogId field
        /// </summary>
        [DisplayName("Card Credit Plus Log Id")]
        public int CardCreditPlusLogId
        { get { return cardCreditPlusLogId; } set { this.IsChanged = true; cardCreditPlusLogId = value; } }

        /// <summary>
        /// Get/Set method of the cardCreditPlusId field
        /// </summary>
        [DisplayName("Card Credit Plus Id")]
        public int CardCreditPlusId
        { get { return cardCreditPlusId; } set { this.IsChanged = true; cardCreditPlusId = value; } }

        /// <summary>
        /// Get/Set method of the creditPlus field
        /// </summary>
        [DisplayName("creditPlus")]
        public double CreditPlus
        { get { return creditPlus; } set { this.IsChanged = true; creditPlus = value; } }

        /// <summary>
        /// Get/Set method of the creditPlus field
        /// </summary>
        [DisplayName("creditPlus")]
        public string CreditPlusType
        { get { return creditPlusType; } set { this.IsChanged = true; creditPlusType = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusBalance field
        /// </summary>
        [DisplayName("CreditPlusBalance")]
        public double CreditPlusBalance
        { get { return creditPlusBalance; } set { this.IsChanged = true; creditPlusBalance = value; } }

        /// <summary>
        /// Get/Set method of the playStartTime field
        /// </summary>
        [DisplayName("PlayStartTime")]
        public DateTime? PlayStartTime
        { get { return playStartTime; } set { this.IsChanged = true; playStartTime = value; } }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy
        { get { return createdBy; } set { this.IsChanged = true; createdBy = value; } }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate
        { get { return creationDate; } set { this.IsChanged = true; creationDate = value; } }
    

        /// <summary>
        /// Get/Set method of the lastupdatedDate field
        /// </summary>
        [DisplayName("LastupdatedDate")]
        public DateTime LastupdatedDate
        { get { return lastupdatedDate; } set { this.IsChanged = true; lastupdatedDate = value; } }


        /// <summary>
        /// Get/Set method of the lastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy
        { get { return lastUpdatedBy; } set { this.IsChanged = true; lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid
        { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        [DisplayName("siteId")]
        public int SiteId
        { get { return siteId; } set { this.IsChanged = true; siteId = value; } }

        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        [DisplayName("synchStatus")]
        public bool SynchStatus
        { get { return synchStatus; } set { this.IsChanged = true; synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId
        { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } } 

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
                    return notifyingObjectIsChanged || cardCreditPlusLogId < 0;
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
            log.LogMethodExit(null);
        }
    }
}

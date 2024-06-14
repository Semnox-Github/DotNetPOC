/********************************************************************************************
 * Project Name -  Customer Membership Progression DTO
 * Description  - Data object of CustomerMembershipProgression
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar       Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the MembershipProgression data object class. 
    /// </summary>
    public class CustomerMembershipProgressionDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// cardId
            /// </summary>
            Card_ID,
            /// <summary>
            /// Search by cardTypeId field
            /// </summary>
            CARD_TYPE_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by masterEntityId field
            /// </summary>
            MASTERENTITY_ID,
            /// <summary>
            /// Search by membershipId field
            /// </summary>
            MEMBERSHIP_ID,
            /// <summary>
            /// Search by customerId field
            /// </summary>
            CUSTOMER_ID

        }

        private int id;
        private int cardId;
        private int cardTypeId;
        private DateTime? effectiveDate;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int membershipId;
        private int customerId;
        private DateTime? effectiveFromDate;
        private DateTime? effectiveToDate;
        private DateTime? lastRetentionDate;
        private string createdBy;
        private DateTime? creationDate;
        private string lastUpdatedBy;
        private DateTime? lastUpdateDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerMembershipProgressionDTO()
        {
            log.LogMethodEntry();
            id = -1;
            cardId = -1;
            cardTypeId = -1;
            site_id = -1;
            masterEntityId = -1;
            membershipId = -1;
            customerId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public CustomerMembershipProgressionDTO(int id, int cardId, int cardTypeId, DateTime? effectiveDate,
                                                int membershipId, int customerId, DateTime? effectiveFromDate,
                                                DateTime? effectiveToDate, DateTime? lastRetentionDate)
            : this()
        {
            log.LogMethodEntry(id, cardId, cardTypeId, effectiveDate, membershipId, customerId, effectiveFromDate, 
                               effectiveToDate, lastRetentionDate);
            this.id = id;
            this.cardId = cardId;
            this.cardTypeId = cardTypeId;
            this.effectiveDate = effectiveDate;
            this.membershipId = membershipId;
            this.customerId = customerId;
            this.effectiveFromDate = effectiveFromDate;
            this.effectiveToDate = effectiveToDate;
            this.lastRetentionDate = lastRetentionDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerMembershipProgressionDTO(int id, int cardId, int cardTypeId, DateTime? effectiveDate, int siteId, string guid, bool synchStatus, int masterEntityId,
                                                int membershipId, int customerId, DateTime? effectiveFromDate, DateTime? effectiveToDate, DateTime? lastRetentionDate, string createdBy,
                                                DateTime? creationDate, string lastUpdatedBy, DateTime? lastUpdateDate)
            : this(id, cardId, cardTypeId, effectiveDate,membershipId, customerId, effectiveFromDate, effectiveToDate, lastRetentionDate)
        {
            log.LogMethodEntry(id, cardId, cardTypeId, effectiveDate, effectiveDate, siteId, guid, synchStatus, masterEntityId,
                                membershipId, customerId, effectiveFromDate, effectiveToDate, lastRetentionDate, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);

            this.site_id = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = CreatedBy;
            this.creationDate = CreationDate;
            this.lastUpdatedBy = LastUpdatedBy;
            this.lastUpdateDate = LastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("Card Id")]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardTypeId field
        /// </summary>
        [DisplayName("CardType Id")]
        public int CardTypeId { get { return cardTypeId; } set { cardTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EffectiveDate field
        /// </summary>
        [DisplayName("Effective Date")]
        public DateTime? EffectiveDate { get { return effectiveDate; } set { effectiveDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [DisplayName("Site Id")]
        public int Site_Id { get { return site_id; } set { site_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        [DisplayName("Membership Id")]
        public int MembershipId { get { return membershipId; } set { membershipId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("Customer Id")]
        public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EffectiveFromDate field
        /// </summary>
        [DisplayName("Effective From Date")]
        public DateTime? EffectiveFromDate { get { return effectiveFromDate; } set { effectiveFromDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Effective To Date field
        /// </summary>
        [DisplayName("Effective To Date")]
        public DateTime? EffectiveToDate { get { return effectiveToDate; } set { effectiveToDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastRetentionDate field
        /// </summary>
        [DisplayName("Last Retention Date")]
        public DateTime? LastRetentionDate { get { return lastRetentionDate; } set { lastRetentionDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime? CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime? LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }

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

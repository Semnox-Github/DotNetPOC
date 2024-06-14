/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of LoyaltyRedemptionRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      24-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the LoyaltyRedemptionRuleDTO data object class. This acts as data holder for the LoyaltyRedemptionRule business object
    /// </summary>
    public class LoyaltyRedemptionRuleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    REDEMPTION RULE ID field
            /// </summary>
            REDEMPTION_RULE_ID,
            /// <summary>
            /// Search by    LOYALTY ATTRIBUTE ID field
            /// </summary>
            LOYALTY_ATTR_ID,
            /// <summary>
            /// Search by EXPIRY DATE field
            /// </summary>
            EXPIRY_DATE,
            /// <summary>
            /// Search by  ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int redemptionRuleId;
        private int loyaltyAttributeId;
        private decimal loyaltyPoints;
        private decimal redemptionValue;
        private DateTime expiryDate;
        private bool activeFlag;
        private string guid;
        private bool synchStatus;
        private decimal minimumPoints;
        private decimal maximiumPoints;
        private char multiplesOnly;
        private decimal virtualPoints;
        private int masterEntityId;
        private int siteId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoyaltyRedemptionRuleDTO()
        {
            log.LogMethodEntry();
            redemptionRuleId = -1;
            loyaltyAttributeId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields.
        /// </summary>
        public LoyaltyRedemptionRuleDTO(int redemptionRuleId, int loyaltyAttributeId, decimal loyaltyPoints, decimal redemptionValue,
                                        DateTime expiryDate, bool activeFlag, decimal minimumPoints, decimal maximiumPoints, char multiplesOnly, decimal virtualLoyaltyPoints)
            : this()
        {
            log.LogMethodEntry(redemptionRuleId, loyaltyAttributeId, loyaltyPoints, redemptionValue, expiryDate, activeFlag,
                               guid, synchStatus, minimumPoints, maximiumPoints, multiplesOnly, virtualLoyaltyPoints);

            this.redemptionRuleId = redemptionRuleId;
            this.loyaltyAttributeId = loyaltyAttributeId;
            this.loyaltyPoints = loyaltyPoints;
            this.redemptionValue = redemptionValue;
            this.expiryDate = expiryDate;
            this.activeFlag = activeFlag;
            this.minimumPoints = minimumPoints;
            this.maximiumPoints = maximiumPoints;
            this.multiplesOnly = multiplesOnly;
            this.virtualPoints = virtualLoyaltyPoints;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public LoyaltyRedemptionRuleDTO(int redemptionRuleId, int loyaltyAttributeId, decimal loyaltyPoints, decimal redemptionValue,
                                       DateTime expiryDate, bool activeFlag, string guid, bool synchStatus, decimal minimumPoints,
                                       decimal maximiumPoints, char multiplesOnly, int masterEntityId, int siteId, DateTime lastUpdatedDate,
                                       string lastUpdatedBy, string createdBy, DateTime creationDate, decimal virtualLoyaltyPoints)
           : this(redemptionRuleId, loyaltyAttributeId, loyaltyPoints, redemptionValue, expiryDate, activeFlag, minimumPoints,
                                   maximiumPoints, multiplesOnly, virtualLoyaltyPoints)
        {
            log.LogMethodEntry(redemptionRuleId, loyaltyAttributeId, loyaltyPoints, redemptionValue, expiryDate, activeFlag,
                               guid, synchStatus, minimumPoints, maximiumPoints, multiplesOnly, virtualLoyaltyPoints, masterEntityId,
                               siteId, lastUpdatedDate, lastUpdatedBy, createdBy, creationDate);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RedemptionRuleId  field
        /// </summary>
        public int RedemptionRuleId
        {
            get { return redemptionRuleId; }
            set { redemptionRuleId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LoyaltyAttributeId field
        /// </summary>
        public int LoyaltyAttributeId
        {
            get { return loyaltyAttributeId; }
            set { loyaltyAttributeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LoyaltyPoints field
        /// </summary>
        public decimal LoyaltyPoints
        {
            get { return loyaltyPoints; }
            set { loyaltyPoints = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the RedemptionValue field
        /// </summary>
        public decimal RedemptionValue
        {
            get { return redemptionValue; }
            set { redemptionValue = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MinimumPoints field
        /// </summary>
        public decimal MinimumPoints
        {
            get { return minimumPoints; }
            set { minimumPoints = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MaximiumPoints field
        /// </summary>
        public decimal MaximiumPoints
        {
            get { return maximiumPoints; }
            set { maximiumPoints = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MultiplesOnly field
        /// </summary>
        public char MultiplesOnly
        {
            get { return multiplesOnly; }
            set { multiplesOnly = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VirtualLoyaltyPoints field
        /// </summary>
        public decimal VirtualPoints
        {
            get { return virtualPoints; }
            set { virtualPoints = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || redemptionRuleId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}

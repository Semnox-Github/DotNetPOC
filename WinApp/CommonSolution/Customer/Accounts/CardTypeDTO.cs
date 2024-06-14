/********************************************************************************************
 * Project Name - Customer
 * Description  - Data object of CardTyp
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        27-May-2020   Girish Kundar               Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Customer.Accounts
{
    public class CardTypeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int cardTypeId;
        private string cardType;
        private string description;
        private int? minimumUsageTrigger;
        private char automaticApply;
        private int discountId;
        private decimal? redemptionDiscount;
        private int? minimumRechargeTrigger;
        private int priceListId;
        private int? triggerDurationInDays;
        private bool? vipStatusTrigger;
        private int baseCardTypeId;
        private int? loyaltyPointsTrigger;
        private int newMembershipId;
        private bool cardTypeMigrated;
        private string migrationMessage;
        private int existingTriggerSource;
        private int qualifyingDuration;
        private decimal loyaltyPointConvRatio;
        private bool redeemLoyaltyPoints;
        private int? migrationOrder;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool qualifyingDurationProceed;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            CARDTYPE_ID,
            /// <summarycard type
            /// Search by USER_NAME field
            /// </summary>
            CARDTYPE,
            /// <summary>
            /// Search by discount ID field
            /// </summary>
            DISCOUNT_ID,
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY id field
            /// </summary>
            MASTER_ENTITY_ID

        }
        /// <summary>
        /// Default constructor for CardTypeDTO
        /// </summary>
        public CardTypeDTO()
        {
            log.LogMethodEntry();
            discountId = -1;
            priceListId = -1;
            baseCardTypeId = -1;
            siteId = -1;
            masterEntityId = -1;
            migrationOrder = null;
            loyaltyPointsTrigger = null;
            triggerDurationInDays = null;
            minimumRechargeTrigger = null;
            vipStatusTrigger = null;
            redemptionDiscount = null;
            minimumUsageTrigger = null;
            newMembershipId = -1;
            automaticApply = 'N';
            qualifyingDurationProceed = false;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor for CardTypeDTO with required fields
        /// </summary>
        public CardTypeDTO(int cardTypeId, string cardType, string description, int? minimumUsageTrigger, char automaticApply, int discountId,
                            decimal? redemptionDiscount, int? minimumRechargeTrigger, int priceListId, int? triggerDurationInDays,
                            bool? vipStatusTrigger, int baseCardTypeId, int? loyaltyPointsTrigger,
                            int newMembershipId, bool cardTypeMigrated, string migrationMessage, int existingTriggerSource, int qualifyingDuration,
                            decimal loyaltyPointConvRatio, bool redeemLoyaltyPoints, int? migrationOrder)
            : this()
        {
            log.LogMethodEntry();
            this.cardTypeId = cardTypeId;
            this.cardType = cardType;
            this.description = description;
            this.minimumUsageTrigger = minimumUsageTrigger;
            this.automaticApply = automaticApply;
            this.discountId = discountId;
            this.redemptionDiscount = redemptionDiscount;
            this.minimumRechargeTrigger = minimumRechargeTrigger;
            this.priceListId = priceListId;
            this.triggerDurationInDays = triggerDurationInDays;
            this.vipStatusTrigger = vipStatusTrigger;
            this.baseCardTypeId = baseCardTypeId;
            this.loyaltyPointsTrigger = loyaltyPointsTrigger;
            this.newMembershipId = newMembershipId;
            this.cardTypeMigrated = cardTypeMigrated;
            this.migrationMessage = migrationMessage;
            this.existingTriggerSource = existingTriggerSource;
            this.qualifyingDuration = qualifyingDuration;
            this.loyaltyPointConvRatio = loyaltyPointConvRatio;
            this.redeemLoyaltyPoints = redeemLoyaltyPoints;
            this.migrationOrder = migrationOrder;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor for CardTypeDTO with required fields
        /// </summary>
        public CardTypeDTO(int cardTypeId, string cardType, string description, int? minimumUsageTrigger, char automaticApply, int discountId,
                            decimal? redemptionDiscount, int? minimumRechargeTrigger, int priceListId, int? triggerDurationInDays,
                            bool? vipStatusTrigger, int baseCardTypeId, int? loyaltyPointsTrigger,
                            int newMembershipId, bool cardTypeMigrated, string migrationMessage, int existingTriggerSource, int qualifyingDuration,
                            decimal loyaltyPointConvRatio, bool redeemLoyaltyPoints, int? migrationOrder, int siteId, string guid,
                            bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate
              )
            : this(cardTypeId,  cardType,  description,  minimumUsageTrigger, automaticApply,  discountId,
                             redemptionDiscount,minimumRechargeTrigger,  priceListId,  triggerDurationInDays,
                             vipStatusTrigger,  baseCardTypeId,  loyaltyPointsTrigger,
                             newMembershipId,  cardTypeMigrated,  migrationMessage, existingTriggerSource, qualifyingDuration,
                             loyaltyPointConvRatio,  redeemLoyaltyPoints,  migrationOrder)
        {
            log.LogMethodEntry(cardTypeId, cardType, description, minimumUsageTrigger, automaticApply, discountId,
                             redemptionDiscount, minimumRechargeTrigger, priceListId, triggerDurationInDays,
                             vipStatusTrigger, baseCardTypeId, loyaltyPointsTrigger,
                             newMembershipId, cardTypeMigrated, migrationMessage, existingTriggerSource, qualifyingDuration,
                             loyaltyPointConvRatio, redeemLoyaltyPoints, migrationOrder, siteId, guid,
                             synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

      
        public int CardTypeId { get { return cardTypeId; } set { cardTypeId = value; this.IsChanged = true; } }
       
        public string CardType { get { return cardType; } set { cardType = value; this.IsChanged = true; } }
      
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
       
        public int? MinimumUsageTrigger { get { return minimumUsageTrigger; } set { minimumUsageTrigger = value; this.IsChanged = true; } }
     
        public char AutomaticApply { get { return automaticApply; } set { automaticApply = value; this.IsChanged = true; } }
        public int DiscountId { get { return discountId; } set { discountId = value; this.IsChanged = true; } }
     
        public decimal? RedemptionDiscount { get { return redemptionDiscount; } set { redemptionDiscount = value; this.IsChanged = true; } }
       
        public int? MinimumRechargeTrigger { get { return minimumRechargeTrigger; } set { minimumRechargeTrigger = value; this.IsChanged = true; } }
     
        public int PriceListId { get { return priceListId; } set { priceListId = value; this.IsChanged = true; } }
     
        public int? TriggerDurationInDays { get { return triggerDurationInDays; } set { triggerDurationInDays = value; this.IsChanged = true; } }
     
        public bool? VipStatusTrigger { get { return vipStatusTrigger; } set { vipStatusTrigger = value; this.IsChanged = true; } }
    
        public int BaseCardTypeId { get { return baseCardTypeId; } set { baseCardTypeId = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId
        
        public int? LoyaltyPointsTrigger { get { return loyaltyPointsTrigger; } set { loyaltyPointsTrigger = value; this.IsChanged = true; } }
        
        public int MembershipId { get { return newMembershipId; } set { newMembershipId = value; this.IsChanged = true; } }
      
        public bool CardTypeMigrated { get { return cardTypeMigrated; } set { cardTypeMigrated = value; this.IsChanged = true; } }
      
        public string MigrationMessage { get { return migrationMessage; } set { migrationMessage = value; this.IsChanged = true; } }
       
        public int ExistingTriggerSource { get { return existingTriggerSource; } set { existingTriggerSource = value; this.IsChanged = true; } }
       
        public int QualifyingDuration { get { return qualifyingDuration; } set { qualifyingDuration = value; this.IsChanged = true; } }
       
        public decimal LoyaltyPointConvRatio { get { return loyaltyPointConvRatio; } set { loyaltyPointConvRatio = value; this.IsChanged = true; } }
       
        public bool RedeemLoyaltyPoints { get { return redeemLoyaltyPoints; } set { redeemLoyaltyPoints = value; this.IsChanged = true; } }
      
        public int? MigrationOrder { get { return migrationOrder; } set { migrationOrder = value; this.IsChanged = true; } }
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value ; this.IsChanged = true; } }
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        public string Guid { get { return guid; } set { guid = value; } }
        public int SiteId { get { return siteId; } set { siteId = value; } }
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        public bool QualifyingDurationProceed { get { return qualifyingDurationProceed; } set { qualifyingDurationProceed = value; } }
        

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || cardTypeId < 0;
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

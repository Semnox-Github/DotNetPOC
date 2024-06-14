/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of LoyaltyBonusAttribute
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      24-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the LoyaltyBonusAttributeDTO data object class. This acts as data holder for the LoyaltyBonusAttributes business object
    /// </summary>
    public class LoyaltyBonusAttributeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    LOYALTY BONUS ID field
            /// </summary>
            LOYALTY_BONUS_ID,
            /// <summary>
            /// Search by    LOYALTY RULE ID field
            /// </summary>
            LOYALTY_RULE_ID,
            /// <summary>
            /// Search by    LOYALTY RULE ID field
            /// </summary>
            LOYALTY_RULE_ID_LIST,
            /// <summary>
            /// Search by LOYALTY ATTRIBUTE ID field
            /// </summary>
            LOYALTY_ATTRIBUTE_ID,
            /// <summary>
            /// Search by  PERIOD FROM field
            /// </summary>
            PERIOD_FROM,
            /// <summary>
            /// Search by PERIOD TO field
            /// </summary>
            PERIOD_TO,
            /// <summary>
            /// Search by  CAMPAIGN ID field
            /// </summary>
            TIME_FROM,
            /// <summary>
            /// Search by TIME FROM field
            /// </summary>
            TIME_TO,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by TIME TO field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int loyaltyBonusId;
        private int loyaltyRuleId;
        private int loyaltyAttributeId;
        private decimal bonusPercentage;
        private decimal bonusValue;
        private decimal minimumSaleAmount;
        private DateTime periodFrom;
        private DateTime periodTo;
        private decimal timeFrom;
        private decimal timeTo;
        private int numberOfDays;
        private char monday;
        private char tuesday;
        private char wednesday;
        private char thursday;
        private char friday;
        private char saturday;
        private char sunday;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private char extendOnReload;
        private int validAfterDays;
        private char forMembershipOnly;
        private char applicableElement;
        private string createdBy;
        private DateTime creationDate;
        private int masterEntityId;
        private List<LoyaltyBonusPurchaseCriteriaDTO> loyaltyBonusPurchaseCriteriaDTOList;
        private List<LoyaltyBonusRewardCriteriaDTO> loyaltyBonusRewardCriteriaDTOList;
    
        private bool activeFlag;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoyaltyBonusAttributeDTO()
        {
            log.LogMethodEntry();
            loyaltyBonusId = -1;
            loyaltyRuleId = -1;
            loyaltyAttributeId = -1;
            siteId = -1;
            masterEntityId = -1;
            loyaltyBonusPurchaseCriteriaDTOList = new List<LoyaltyBonusPurchaseCriteriaDTO>();
            loyaltyBonusRewardCriteriaDTOList = new List<LoyaltyBonusRewardCriteriaDTO>();
            activeFlag = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public LoyaltyBonusAttributeDTO(int loyaltyBonusId, int loyaltyRuleId, int loyaltyAttributeId, decimal bonusPercentage, 
                        decimal bonusValue, decimal minimumSaleAmount, DateTime periodFrom, DateTime periodTo, decimal timeFrom, 
                        decimal timeTo, int numberOfDays, char monday, char tuesday, char wednesday, char thursday, char friday, 
                        char saturday, char sunday, char extendOnReload, int validAfterDays, 
                        char forMembershipOnly, char applicableElement,bool activeFlag) 
            : this()
        {
            log.LogMethodEntry(loyaltyBonusId, loyaltyRuleId, loyaltyAttributeId, bonusPercentage, bonusValue, minimumSaleAmount,
                               periodFrom, periodTo, timeFrom, timeTo, numberOfDays, monday, tuesday, wednesday, thursday,
                               friday, saturday, sunday, extendOnReload, validAfterDays, forMembershipOnly, applicableElement, activeFlag);
            this.loyaltyBonusId = loyaltyBonusId;
            this.loyaltyRuleId = loyaltyRuleId;
            this.loyaltyAttributeId = loyaltyAttributeId;
            this.bonusPercentage = bonusPercentage;
            this.bonusValue = bonusValue;
            this.minimumSaleAmount = minimumSaleAmount;
            this.periodFrom = periodFrom;
            this.periodTo = periodTo;
            this.timeFrom = timeFrom;
            this.timeTo = timeTo;
            this.numberOfDays = numberOfDays;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.extendOnReload = extendOnReload;
            this.validAfterDays = validAfterDays;
            this.forMembershipOnly = forMembershipOnly;
            this.applicableElement = applicableElement;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public LoyaltyBonusAttributeDTO(int loyaltyBonusId, int loyaltyRuleId, int loyaltyAttributeId, decimal bonusPercentage,
                                                decimal bonusValue, decimal minimumSaleAmount, DateTime periodFrom, DateTime periodTo,
                                                decimal timeFrom, decimal timeTo, int numberOfDays, char monday, char tuesday, char wednesday, char thursday,
                                                char friday, char saturday, char sunday, DateTime lastUpdatedDate, string lastUpdatedBy, string guid, int siteId,
                                                bool synchStatus, char extendOnReload, int validAfterDays, char forMembershipOnly, char applicableElement,
                                                string createdBy, DateTime creationDate, int masterEntityId,bool activeFlag)
            : this(loyaltyBonusId, loyaltyRuleId, loyaltyAttributeId, bonusPercentage, bonusValue, minimumSaleAmount,
                               periodFrom, periodTo, timeFrom, timeTo, numberOfDays, monday, tuesday, wednesday, thursday,
                               friday, saturday, sunday, extendOnReload, validAfterDays, forMembershipOnly, applicableElement, activeFlag)
        {
            log.LogMethodEntry(loyaltyBonusId, loyaltyRuleId, loyaltyAttributeId, bonusPercentage, bonusValue, minimumSaleAmount,
                               periodFrom, periodTo, timeFrom, timeTo, numberOfDays, monday, tuesday, wednesday, thursday,
                               friday, saturday, sunday, extendOnReload, validAfterDays, forMembershipOnly, applicableElement,
                               lastUpdatedDate, lastUpdatedBy, guid, siteId, synchStatus, createdBy, creationDate, masterEntityId, activeFlag);
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
        /// Get/Set method of the LoyaltyBonusId  field
        /// </summary>
        public int LoyaltyBonusId
        {
            get { return loyaltyBonusId; }
            set { loyaltyBonusId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LoyaltyRuleId field
        /// </summary>
        public int LoyaltyRuleId
        {
            get { return loyaltyRuleId; }
            set { loyaltyRuleId = value; this.IsChanged = true; }
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
        /// Get/Set method of the BonusPercentage field
        /// </summary>
        public decimal BonusPercentage
        {
            get { return bonusPercentage; }
            set { bonusPercentage = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the BonusValue field
        /// </summary>
        public decimal BonusValue
        {
            get { return bonusValue; }
            set { bonusValue = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MinimumSaleAmount field
        /// </summary>
        public decimal MinimumSaleAmount
        {
            get { return minimumSaleAmount; }
            set { minimumSaleAmount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PeriodFrom field
        /// </summary>
        public DateTime PeriodFrom
        {
            get { return periodFrom; }
            set { periodFrom = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PeriodTo field
        /// </summary>
        public DateTime PeriodTo
        {
            get { return periodTo; }
            set { periodTo = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TimeFrom field
        /// </summary>
        public decimal TimeFrom
        {
            get { return timeFrom; }
            set { timeFrom = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TimeTo field
        /// </summary>
        public decimal TimeTo
        {
            get { return timeTo; }
            set { timeTo = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the NumberOfDays Id field
        /// </summary>
        public int NumberOfDays
        {
            get { return numberOfDays; }
            set { numberOfDays = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Monday field
        /// </summary>
        public char Monday
        {
            get { return monday; }
            set { monday = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        public char Tuesday
        {
            get { return tuesday; }
            set { tuesday = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Wednesday field
        /// </summary>
        public char Wednesday
        {
            get { return wednesday; }
            set { wednesday = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Thursday field
        /// </summary>
        public char Thursday
        {
            get { return thursday; }
            set { thursday = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Friday field
        /// </summary>
        public char Friday
        {
            get { return friday; }
            set { friday = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Saturday field
        /// </summary>
        public char Saturday
        {
            get { return saturday; }
            set { saturday = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Sunday field
        /// </summary>
        public char Sunday
        {
            get { return sunday; }
            set { sunday = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ExtendOnReload field
        /// </summary>
        public char ExtendOnReload
        {
            get { return extendOnReload; }
            set { extendOnReload = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ValidAfterDays  field
        /// </summary>
        public int ValidAfterDays
        {
            get { return validAfterDays; }
            set { validAfterDays = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ForMembershipOnly field
        /// </summary>
        public char ForMembershipOnly
        {
            get { return forMembershipOnly; }
            set { forMembershipOnly = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ApplicableElement field
        /// </summary>
        public char ApplicableElement
        {
            get { return applicableElement; }
            set { applicableElement = value; this.IsChanged = true; }
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
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LoyaltyBonusPurchaseCriteriaDTOList field
        /// </summary>
        public List<LoyaltyBonusPurchaseCriteriaDTO> LoyaltyBonusPurchaseCriteriaDTOList
        {
            get { return loyaltyBonusPurchaseCriteriaDTOList; }
            set { loyaltyBonusPurchaseCriteriaDTOList = value; }
        }
        /// <summary>
        /// Get/Set method of the LoyaltyBonusRewardCriteriaDTOList field
        /// </summary>
        public List<LoyaltyBonusRewardCriteriaDTO> LoyaltyBonusRewardCriteriaDTOList
        {
            get { return loyaltyBonusRewardCriteriaDTOList; }
            set { loyaltyBonusRewardCriteriaDTOList = value; }
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
                    return notifyingObjectIsChanged || loyaltyBonusId < 0;
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
        /// Returns whether the LoyaltyBonusAttributeDTO changed or any of its LoyaltyBonusPurchaseCriteriaDTO , LoyaltyBonusRewardCriteriaDTO children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (loyaltyBonusPurchaseCriteriaDTOList != null &&
                    loyaltyBonusPurchaseCriteriaDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (loyaltyBonusRewardCriteriaDTOList != null &&
                   loyaltyBonusRewardCriteriaDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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

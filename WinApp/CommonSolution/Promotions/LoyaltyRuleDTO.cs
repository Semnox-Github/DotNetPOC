/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of LoyaltyRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        13-Jun-2019   Girish kundar       Created
 *2.70        03-Jul-2019   Dakshakh raj        Modified : (Added Parameterized costrustor,
                                                       Added CreatedBy and CreationDate columns)
 *2.80       06-Feb-2020   Girish Kundar       Modified : As per the 3 tier standard
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the Loyalty data object class. This acts as data holder for the Loyalty business object
    /// </summary>
    public class LoyaltyRuleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  LOYALTY RULE ID field
            /// </summary>
            LOYALTY_RULE_ID,
            /// <summary>
            /// Search by ACTIVE FLAG field, Pass Y or N
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by VIP ONLY field, Pass Y or N
            /// </summary>
            VIP_ONLY,
            /// <summary>
            /// Search by APPLY IMMEDIATE field, Pass 1 or 0
            /// </summary>
            APPLY_IMMEDIATE,
            /// <summary>
            /// Search by MEMBERSHIP ID field
            /// </summary>
            MEMBERSHIP_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by PURCHASE_OR_CONSUMPTION_APPLICABILITY field
            /// </summary>
            PURCHASE_OR_CONSUMPTION_APPLICABILITY,
            /// <summary>
            /// Search by PURCHASE_OR_CONSUMPTION_APPLICABILITY field
            /// </summary>
            EXPIRY_DATE
        }

        private int loyaltyRuleId;
        private string name;
        private double? minimumUsedCredits;
        private double? minimumSaleAmount;
        private DateTime? expiryDate;
        private string purchaseOrConsumptionApplicability;
        private bool activeFlag;
        private bool vipOnly;
        private int instances;
        private bool firstInstancesOnly;
        private bool onDifferentDays;
        private DateTime? periodFrom;
        private DateTime? periodTo;
        private double? timeFrom;
        private double? timeTo;
        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
        private int numberOfDays;
        private DateTime? lastUpdatedDate;
        private string lastUpdatedBy;
        private bool excludeNewCardIssue;
        //private int cardTypeId;
        private int customerCount;
        private string customerCountType;
        private string guid;
        private int site_id;
        private double? maximumUsedCredits;
        private double? maximumSaleAmount;
        private bool synchStatus;
        private int rewardCount;
        private int internetKey;
        private int daysAfterFirstPurchase;
        private int masterEntityId;
        private string applyImmediate;
        private int membershipId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private List<LoyaltyRuleTriggerDTO> loyaltyRuleTriggerDTOList;
        private List<LoyaltyBonusAttributeDTO> loyaltyBonusAttributeDTOList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoyaltyRuleDTO()
        {
            log.LogMethodEntry();
            loyaltyRuleId = -1;  
            activeFlag = true;
            site_id = -1; 
            masterEntityId = -1; 
            membershipId = -1;
            applyImmediate = "N";
            loyaltyRuleTriggerDTOList = new List<LoyaltyRuleTriggerDTO>();
            loyaltyBonusAttributeDTOList = new List<LoyaltyBonusAttributeDTO>();
            expiryDate = null;
            periodFrom = null;
            periodTo = null;
            lastUpdatedDate = null;
            timeFrom = null;
            timeTo = null;
            minimumSaleAmount = null;
            minimumUsedCredits = null;
            maximumUsedCredits = null;
            maximumSaleAmount = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public LoyaltyRuleDTO(int loyaltyRuleId, string name, double? minimumUsedCredits, double? minimumSaleAmount, DateTime? expiryDate, string purchaseOrConsumptionApplicability, bool activeFlag,
                              bool vipOnly, int instances, bool firstInstancesOnly, bool onDifferentDays, DateTime? periodFrom, DateTime? periodTo, double? timeFrom, double? timeTo, bool monday,
                              bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday, int numberOfDays, bool excludeNewCardIssue,
                              int customerCount, string customerCountType, double? maximumUsedCredits, double? maximumSaleAmount, int rewardCount,
                              int internetKey, int daysAfterFirstPurchase, string applyImmediate, int membershipId )
            : this()
        {
            log.LogMethodEntry(loyaltyRuleId, name, minimumUsedCredits, minimumSaleAmount, expiryDate, purchaseOrConsumptionApplicability, activeFlag,
                               vipOnly, instances,firstInstancesOnly, onDifferentDays,  periodFrom, periodTo,  timeFrom,  timeTo,  monday,
                               tuesday,  wednesday,  thursday,  friday,  saturday,  sunday,  numberOfDays,  excludeNewCardIssue,
                               customerCount,  customerCountType,  maximumUsedCredits, maximumSaleAmount, rewardCount,
                               internetKey, daysAfterFirstPurchase,  applyImmediate, membershipId);

            this.loyaltyRuleId = loyaltyRuleId;
            this.name = name;
            this.minimumUsedCredits = minimumUsedCredits;
            this.minimumSaleAmount = minimumSaleAmount;
            this.expiryDate = expiryDate;
            this.purchaseOrConsumptionApplicability = purchaseOrConsumptionApplicability;
            this.activeFlag = activeFlag;
            this.vipOnly = vipOnly;
            this.instances = instances;
            this.firstInstancesOnly = firstInstancesOnly;
            this.onDifferentDays = onDifferentDays;
            this.periodFrom = periodFrom;
            this.periodTo = periodTo;
            this.timeFrom = timeFrom;
            this.timeTo = timeTo;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.numberOfDays = numberOfDays;
            this.excludeNewCardIssue = excludeNewCardIssue;
            this.customerCount = customerCount;
            this.customerCountType = customerCountType;
            this.maximumUsedCredits = maximumUsedCredits;
            this.maximumSaleAmount = maximumSaleAmount;
            this.rewardCount = rewardCount;
            this.internetKey = internetKey;
            this.daysAfterFirstPurchase = daysAfterFirstPurchase;
            this.applyImmediate = applyImmediate;
            this.membershipId = membershipId;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LoyaltyRuleDTO(int loyaltyRuleId, string name, double? minimumUsedCredits, double? minimumSaleAmount, DateTime? expiryDate, string purchaseOrConsumptionApplicability, bool activeFlag,
                              bool vipOnly, int instances, bool firstInstancesOnly, bool onDifferentDays, DateTime? periodFrom, DateTime? periodTo, double? timeFrom, double? timeTo, bool monday,
                              bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday, int numberOfDays, DateTime? lastUpdatedDate, string lastUpdatedBy, bool excludeNewCardIssue,
                              int customerCount, string customerCountType, string guid, int site_id, double? maximumUsedCredits, double? maximumSaleAmount, bool synchStatus, int rewardCount, 
                              int internetKey, int daysAfterFirstPurchase, int masterEntityId, string applyImmediate, int membershipId, string createdBy, DateTime creationDate)
            :this(loyaltyRuleId, name, minimumUsedCredits, minimumSaleAmount, expiryDate, purchaseOrConsumptionApplicability, activeFlag,
                               vipOnly, instances, firstInstancesOnly, onDifferentDays, periodFrom, periodTo, timeFrom, timeTo, monday,
                               tuesday, wednesday, thursday, friday, saturday, sunday, numberOfDays, excludeNewCardIssue,
                                customerCount, customerCountType, maximumUsedCredits, maximumSaleAmount, rewardCount,
                               internetKey, daysAfterFirstPurchase, applyImmediate, membershipId)
        {
            log.LogMethodEntry(loyaltyRuleId, name,  minimumUsedCredits, minimumSaleAmount, expiryDate, purchaseOrConsumptionApplicability, activeFlag,
                               vipOnly, instances,  firstInstancesOnly, onDifferentDays, periodFrom, periodTo, timeFrom, timeTo, monday,
                               tuesday, wednesday, thursday, friday, saturday, sunday, numberOfDays,lastUpdatedDate, lastUpdatedBy, excludeNewCardIssue,
                               customerCount, customerCountType, guid, site_id, maximumUsedCredits, maximumSaleAmount, synchStatus, rewardCount,
                               internetKey, daysAfterFirstPurchase, masterEntityId, applyImmediate, membershipId, createdBy, creationDate);
            
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LoyaltyRuleId field
        /// </summary>
        [DisplayName("LoyaltyRuleId")]
        public int LoyaltyRuleId
        {
            get { return loyaltyRuleId; }
            set { this.IsChanged = true; loyaltyRuleId = value; }
        }

        /// <summary>
        /// Get/Set method of the name field
        /// </summary>
        [DisplayName("Name")]
        public string Name
        {
            get { return name; }
            set { this.IsChanged = true; name = value; }
        }
        /// <summary>
        /// Get/Set method of the minimumUsedCredits field
        /// </summary>
        [DisplayName("Minimum Used Credits")]
        public double? MinimumUsedCredits
        {
            get { return minimumUsedCredits; }
            set { this.IsChanged = true; minimumUsedCredits = value; }
        }
        /// <summary>
        /// Get/Set method of the MinimumSaleAmount field
        /// </summary>
        [DisplayName("Minimum Sale Amount")]
        public double? MinimumSaleAmount
        {
            get { return minimumSaleAmount; }
            set { this.IsChanged = true; minimumSaleAmount = value; }
        }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate
        {
            get { return expiryDate; }
            set { this.IsChanged = true; expiryDate = value; }
        }

        /// <summary>
        /// Get/Set method of the PurchaseOrConsumptionApplicability field
        /// </summary>
        [DisplayName("Purchase Or Consumption Applicability")]
        public string PurchaseOrConsumptionApplicability
        {
            get { return purchaseOrConsumptionApplicability; }
            set { this.IsChanged = true; purchaseOrConsumptionApplicability = value; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("Active Flag")]
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { this.IsChanged = true; activeFlag = value; }
        }

        /// <summary>
        /// Get/Set method of the VIPOnly field
        /// </summary>
        [DisplayName("VIP Only")]
        public bool VIPOnly
        {
            get { return vipOnly; }
            set { this.IsChanged = true; vipOnly = value; }
        }

        /// <summary>
        /// Get/Set method of the Instances field
        /// </summary>
        [DisplayName("Instances")]
        public int Instances
        {
            get { return instances; }
            set { this.IsChanged = true; instances = value; }
        }

        /// <summary>
        /// Get/Set method of the FirstInstancesOnly field
        /// </summary>
        [DisplayName("First Instances Only")]
        public bool FirstInstancesOnly
        {
            get { return firstInstancesOnly; }
            set { this.IsChanged = true; firstInstancesOnly = value; }
        }
        /// <summary>
        /// Get/Set method of the OnDifferentDays field
        /// </summary>
        [DisplayName("On Different Days")]
        public bool OnDifferentDays
        {
            get { return onDifferentDays; }
            set { this.IsChanged = true; onDifferentDays = value; }
        }

        /// <summary>
        /// Get/Set method of the PeriodFrom field
        /// </summary>
        [DisplayName("Period From")]
        public DateTime? PeriodFrom
        {
            get { return periodFrom; }
            set { this.IsChanged = true; periodFrom = value; }
        }

        /// <summary>
        /// Get/Set method of the PeriodTo field
        /// </summary>
        [DisplayName("Period To")]
        public DateTime? PeriodTo
        {
            get { return periodTo; }
            set { this.IsChanged = true; periodTo = value; }
        }

        /// <summary>
        /// Get/Set method of the TimeFrom field
        /// </summary>
        [DisplayName("Time From")]
        public double? TimeFrom
        {
            get { return timeFrom; }
            set { this.IsChanged = true; timeFrom = value; }
        }

        /// <summary>
        /// Get/Set method of the TimeTo field
        /// </summary>
        [DisplayName("Time To")]
        public double? TimeTo
        {
            get { return timeTo; }
            set { this.IsChanged = true; timeTo = value; }
        }

        /// <summary>
        /// Get/Set method of the Monday field
        /// </summary>
        [DisplayName("Monday")]
        public bool Monday
        {
            get { return monday; }
            set { this.IsChanged = true; monday = value; }
        }

        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        [DisplayName("Tuesday")]
        public bool Tuesday
        {
            get { return tuesday; }
            set { this.IsChanged = true; tuesday = value; }
        }

        /// <summary>
        /// Get/Set method of the Wednesday field
        /// </summary>
        [DisplayName("Wednesday")]
        public bool Wednesday
        {
            get { return wednesday; }
            set { this.IsChanged = true; wednesday = value; }
        }

        /// <summary>
        /// Get/Set method of the Thursday field
        /// </summary>
        [DisplayName("Thursday")]
        public bool Thursday
        {
            get { return thursday; }
            set { this.IsChanged = true; thursday = value; }
        }

        /// <summary>
        /// Get/Set method of the Friday field
        /// </summary>
        [DisplayName("Friday")]
        public bool Friday
        {
            get { return friday; }
            set { this.IsChanged = true; friday = value; }
        }

        /// <summary>
        /// Get/Set method of the Saturday field
        /// </summary>
        [DisplayName("Saturday")]
        public bool Saturday
        {
            get { return saturday; }
            set { this.IsChanged = true; saturday = value; }
        }

        /// <summary>
        /// Get/Set method of the Sunday field
        /// </summary>
        [DisplayName("Sunday")]
        public bool Sunday
        {
            get { return sunday; }
            set { this.IsChanged = true; sunday = value; }
        }

        /// <summary>
        /// Get/Set method of the NumberOfDays field
        /// </summary>
        [DisplayName("Number Of Days")]
        public int NumberOfDays
        {
            get { return numberOfDays; }
            set { this.IsChanged = true; numberOfDays = value; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime? LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { this.IsChanged = true; lastUpdatedDate = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { this.IsChanged = true; lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the ExcludeNewCardIssue field
        /// </summary>
        [DisplayName("Exclude New Card Issue")]
        public bool ExcludeNewCardIssue
        {
            get { return excludeNewCardIssue; }
            set { this.IsChanged = true; excludeNewCardIssue = value; }
        }

        

        /// <summary>
        /// Get/Set method of the CustomerCount field
        /// </summary>
        [DisplayName("Customer Count")]
        public int CustomerCount
        {
            get { return customerCount; }
            set { this.IsChanged = true; customerCount = value; }
        }
        /// <summary>
        /// Get/Set method of the CustomerCountType field
        /// </summary>
        [DisplayName("Customer Count Type")]
        public string CustomerCountType
        {
            get { return customerCountType; }
            set { this.IsChanged = true; customerCountType = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid
        {
            get { return guid; }
            set { this.IsChanged = true; guid = value; }
        }

        /// <summary>
        /// Get/Set method of the Site_Id field
        /// </summary>
        [DisplayName("Site Id")]
        public int Site_Id
        {
            get { return site_id; }
            set { this.IsChanged = true; site_id = value; }
        }
        /// <summary>
        /// Get/Set method of the MaximumUsedCredits field
        /// </summary>
        [DisplayName("Maximum Used Credits")]
        public double? MaximumUsedCredits
        {
            get { return maximumUsedCredits; }
            set { this.IsChanged = true; maximumUsedCredits = value; }
        }

        /// <summary>
        /// Get/Set method of the MaximumSaleAmount field
        /// </summary>
        [DisplayName("Maximum Sale Amount")]
        public double? MaximumSaleAmount
        {
            get { return maximumSaleAmount; }
            set { this.IsChanged = true; maximumSaleAmount = value; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { this.IsChanged = true; synchStatus = value; }
        }

        /// <summary>
        /// Get/Set method of the RewardCount field
        /// </summary>
        [DisplayName("Reward Count")]
        public int RewardCount
        {
            get { return rewardCount; }
            set { this.IsChanged = true; rewardCount = value; }
        }

        /// <summary>
        /// Get/Set method of the InternetKey field
        /// </summary>
        [DisplayName("Internet Key")]
        public int InternetKey
        {
            get { return internetKey; }
            set { this.IsChanged = true; internetKey = value; }
        }

        /// <summary>
        /// Get/Set method of the DaysAfterFirstPurchase field
        /// </summary>
        [DisplayName("Days After First Purchase")]
        public int DaysAfterFirstPurchase
        {
            get { return daysAfterFirstPurchase; }
            set { this.IsChanged = true; daysAfterFirstPurchase = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { this.IsChanged = true; masterEntityId = value; }
        }
        /// <summary>
        /// Get/Set method of the ApplyImmediate field
        /// </summary>
        [DisplayName("Apply Immediate")]
        public string ApplyImmediate
        {
            get { return applyImmediate; }
            set { this.IsChanged = true; applyImmediate = value; }
        } 
        /// <summary>
        /// Get/Set method of the LoyaltyRuleId field
        /// </summary>
        [DisplayName("Membership Id")]
        public int MembershipId
        {
            get { return membershipId; }
            set { this.IsChanged = true; membershipId = value; }
        }


        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get       {   return createdBy;        }
            set       {    createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get    {  return creationDate;}
            set    { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the loyaltyRuleTriggerDTOList field
        /// </summary>
        public List<LoyaltyRuleTriggerDTO> LoyaltyRuleTriggerDTOList
        {
            get { return loyaltyRuleTriggerDTOList; }
            set { loyaltyRuleTriggerDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the loyaltyBonusAttributeDTOList field
        /// </summary>
        public List<LoyaltyBonusAttributeDTO> LoyaltyBonusAttributeDTOList
        {
            get { return loyaltyBonusAttributeDTOList; }
            set { loyaltyBonusAttributeDTOList = value; }
        }
        /// <summary>
        /// Returns whether the LoyaltyRule changed or any of its loyaltyRuleTriggerDTOList  children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (loyaltyRuleTriggerDTOList != null &&
                    loyaltyRuleTriggerDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (loyaltyBonusAttributeDTOList != null &&
                    loyaltyBonusAttributeDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            { lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || loyaltyRuleId < 0;
                }
            }

            set
            {lock (notifyingObjectIsChangedSyncRoot)
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

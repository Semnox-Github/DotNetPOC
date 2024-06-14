/********************************************************************************************
 * Project Name - Semnox.Parafait.Customer.Accounts -AccountCreditPlusDTO
 * Description  - AccountCreditPlusDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.4.0       28-Sep-2018   Guru S A                 Modified for Pause allowed changes 
 *2.60        21-Feb-2019   Mushahid Faizan          Added isActive Parameter. 
 *2.70.2      23-Jul-2019   Girish Kundar            Modified : Added Constructor with required Parameter
 *                                                              And Who columns
 2.80.0       19-Mar-2020   Jinto Thomas             Modified : Added Constructor with required Parameter
 *                                                              And SourceCreditPlusId columns                                                              
 *2.80.0      19-Mar-2020   Mathew NInan             Added new field ValidityStatus to track
 *                                                       status of entitlements
 *2.100.0     25-Sep-2020   Dakshakh                 Modified - Get/Set method IsChanged() Added accountCreditPlusId check         
 *2.110.0     08-Dec-2020   Guru S A                 Subscription changes         
 *2.110.0     05-Feb-2021   Akshay G                 Added CREDITPLUS_TYPE_LIST searchParameter as part of License enhancement.      
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountCreditPlus data object class. This acts as data holder for the AccountCreditPlus business object
    /// </summary>
    public class AccountCreditPlusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ACCOUNT_CREDITPLUS_ID field
            /// </summary>
            ACCOUNT_CREDITPLUS_ID,
            /// <summary>
            /// Search by ACCOUNT_ID field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by CREDITPLUSTYPE field
            /// </summary>
            CREDITPLUS_TYPE,
            /// <summary>
            /// Search by ForMembershipOnly field
            /// </summary>
            FORMEMBERSHIP_ONLY,
            /// <summary>
            /// Search by expireWithMembership field
            /// </summary>
            EXPIRE_WITH_MEMBERSHIP,
            /// <summary>
            /// Search by membershipRewardsId field
            /// </summary>
            MEMBERSHIP_REWARDS_ID,
            /// <summary>
            /// Search by membershipId field
            /// </summary>
            MEMBERSHIPS_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by TRANSACTION_ID field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by cardId list field
            /// </summary>
            ACCOUNT_ID_LIST,
            /// <summary>
            /// Search by Pause Allowed field
            /// </summary>
            PAUSE_ALLOWED,
            /// <summary>
            ///  Search by isActive
            /// </summary>
            ISACTIVE,
            /// <summary>
            ///  Search by MasterEntity Id
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SOURCE_CREDITPLUS_ID field
            /// </summary>
            SOURCE_CREDITPLUS_ID,
            /// <summary>
            ///  Search by ValidityStatus Id
            /// </summary>
            VALIDITYSTATUS,
            /// <summary>
            /// Search by SUBSCRIPTION_BILLING_SCHEDULE_ID field
            /// </summary>
            SUBSCRIPTION_BILLING_SCHEDULE_ID,
            /// <summary>
            /// Search by CREDITPLUS_TYPE_LIST field
            /// </summary>
            CREDITPLUS_TYPE_LIST,
            /// <summary>
            /// Search by SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED field
            /// </summary>
            SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED,
            /// <summary>
            /// Search by records having Consumption
            /// </summary>
            HAS_CONSUMPTION_RULE

        }

        private int accountCreditPlusId;
        private decimal? creditPlus;
        private CreditPlusType creditPlusType;
        private bool refundable;
        private string remarks;
        private int accountId;
        private int transactionId;
        private int transactionLineId;
        private decimal? creditPlusBalance;
        private DateTime? periodFrom;
        private DateTime? periodTo;
        private decimal? timeFrom;
        private decimal? timeTo;
        private int? numberOfDays;
        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
        private decimal? minimumSaleAmount;
        private int loyaltyRuleId;
        private bool extendOnReload;
        private DateTime? playStartTime;
        private bool ticketAllowed;
        private bool forMembershipOnly;
        private bool expireWithMembership;
        private int membershipId;
        private int membershipRewardsId;
        private bool pauseAllowed;
        private bool isActive;
        private AccountDTO.AccountValidityStatus validityStatus;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private List<AccountCreditPlusConsumptionDTO> accountCreditPlusConsumptionDTOList;
        private List<AccountCreditPlusPurchaseCriteriaDTO> accountCreditPlusPurchaseCriteriaDTOList;
        private List<EntityOverrideDatesDTO> entityOverrideDatesDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private int sourceCreditPlusId;
        private int subscriptionBillingScheduleId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountCreditPlusDTO()
        {
            log.LogMethodEntry();
            accountCreditPlusId = -1;
            accountId = -1;
            masterEntityId = -1;
            loyaltyRuleId = -1;
            transactionId = -1;
            transactionLineId = -1;
            membershipId = -1;
            membershipRewardsId = -1;
            refundable = true;
            extendOnReload = false;
            sunday = true;
            monday = true;
            tuesday = true;
            wednesday = true;
            thursday = true;
            friday = true;
            saturday = true;
            ticketAllowed = true;
            creditPlusType = CreditPlusType.CARD_BALANCE;
            isActive = true;
            pauseAllowed = true;
            siteId = -1;
            accountCreditPlusConsumptionDTOList = new List<AccountCreditPlusConsumptionDTO>();
            accountCreditPlusPurchaseCriteriaDTOList = new List<AccountCreditPlusPurchaseCriteriaDTO>();
            entityOverrideDatesDTOList = new List<EntityOverrideDatesDTO>();
            sourceCreditPlusId = -1;
            subscriptionBillingScheduleId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountCreditPlusDTO(int accountCreditPlusId, decimal? creditPlus, CreditPlusType creditPlusType, bool refundable,
                         string remarks, int accountId, int transactionId, int transactionLineId, decimal? creditPlusBalance,
                         DateTime? periodFrom, DateTime? periodTo, decimal? timeFrom, decimal? timeTo, int? numberOfDays,
                         bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday,
                         bool sunday, decimal? minimumSaleAmount, int loyaltyRuleId, bool extendOnReload, DateTime? playStartTime,
                         bool ticketAllowed, bool forMembershipOnly, bool expireWithMembership, int membershipId, int membershipRewardsId,
                         bool pauseAllowed, bool isActive, int sourceCreditPlusId, AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId)
            : this()
        {
            log.LogMethodEntry(accountCreditPlusId, creditPlus, creditPlusType, refundable, remarks, accountId,
                               transactionId, transactionLineId, creditPlusBalance, periodFrom, periodTo,
                               timeFrom, timeTo, numberOfDays, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, minimumSaleAmount, loyaltyRuleId, extendOnReload, playStartTime, ticketAllowed,
                               forMembershipOnly, expireWithMembership, membershipId, membershipRewardsId,
                               pauseAllowed, isActive, sourceCreditPlusId, validityStatus.ToString(), subscriptionBillingScheduleId);
            this.accountCreditPlusId = accountCreditPlusId;
            this.creditPlus = creditPlus;
            this.creditPlusType = creditPlusType;
            this.refundable = refundable;
            this.remarks = remarks;
            this.accountId = accountId;
            this.transactionId = transactionId;
            this.transactionLineId = transactionLineId;
            this.creditPlusBalance = creditPlusBalance;
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
            this.minimumSaleAmount = minimumSaleAmount;
            this.loyaltyRuleId = loyaltyRuleId;
            this.extendOnReload = extendOnReload;
            this.playStartTime = playStartTime;
            this.ticketAllowed = ticketAllowed;
            this.forMembershipOnly = forMembershipOnly;
            this.expireWithMembership = expireWithMembership;
            this.membershipId = membershipId;
            this.membershipRewardsId = membershipRewardsId;
            this.pauseAllowed = pauseAllowed;
            this.isActive = isActive;
            this.sourceCreditPlusId = sourceCreditPlusId;
            this.validityStatus = validityStatus;
            this.subscriptionBillingScheduleId = subscriptionBillingScheduleId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountCreditPlusDTO(int accountCreditPlusId, decimal? creditPlus, CreditPlusType creditPlusType, bool refundable,
                         string remarks, int accountId, int transactionId, int transactionLineId, decimal? creditPlusBalance,
                         DateTime? periodFrom, DateTime? periodTo, decimal? timeFrom, decimal? timeTo, int? numberOfDays,
                         bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday,
                         bool sunday, decimal? minimumSaleAmount, int loyaltyRuleId, bool extendOnReload, DateTime? playStartTime,
                         bool ticketAllowed, bool forMembershipOnly, bool expireWithMembership, int membershipId, int membershipRewardsId,
                         DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                         int siteId, int masterEntityId, bool synchStatus, string guid, bool pauseAllowed, bool isActive,string createdBy, int sourceCreditPlusId, AccountDTO.AccountValidityStatus validityStatus,int subscriptionBillingScheduleId)
            : this(accountCreditPlusId, creditPlus, creditPlusType, refundable, remarks, accountId,
                               transactionId, transactionLineId, creditPlusBalance, periodFrom, periodTo,
                               timeFrom, timeTo, numberOfDays, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, minimumSaleAmount, loyaltyRuleId, extendOnReload, playStartTime, ticketAllowed,
                               forMembershipOnly, expireWithMembership, membershipId, membershipRewardsId,
                               pauseAllowed, isActive, sourceCreditPlusId, validityStatus, subscriptionBillingScheduleId)
        {
            log.LogMethodEntry(accountCreditPlusId, creditPlus, creditPlusType, refundable, remarks, accountId,
                               transactionId, transactionLineId, creditPlusBalance, periodFrom, periodTo,
                               timeFrom, timeTo, numberOfDays, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, minimumSaleAmount, loyaltyRuleId, extendOnReload, playStartTime, ticketAllowed,
                               forMembershipOnly, expireWithMembership, membershipId, membershipRewardsId, creationDate,
                               lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid, pauseAllowed, isActive, createdBy, validityStatus.ToString(), subscriptionBillingScheduleId);
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [Browsable(false)]
        public int AccountCreditPlusId
        {
            get
            {
                return accountCreditPlusId;
            }

            set
            {
                this.IsChanged = true;
                accountCreditPlusId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the accountId field
        /// </summary>
        [Browsable(false)]
        public int AccountId
        {
            get
            {
                return accountId;
            }

            set
            {
                this.IsChanged = true;
                accountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the creditPlusType field
        /// </summary>
        [DisplayName("CreditPlus Type")]
        public CreditPlusType CreditPlusType
        {
            get
            {
                return creditPlusType;
            }

            set
            {
                this.IsChanged = true;
                creditPlusType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the creditPlus field
        /// </summary>
        [DisplayName("CreditPlus Loaded")]
        public decimal? CreditPlus
        {
            get
            {
                return creditPlus;
            }

            set
            {
                this.IsChanged = true;
                creditPlus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the creditPlusBalance field
        /// </summary>
        [DisplayName("CreditPlus Balance")]
        public decimal? CreditPlusBalance
        {
            get
            {
                return creditPlusBalance;
            }

            set
            {
                this.IsChanged = true;
                creditPlusBalance = value;
            }
        }

        /// <summary>
        /// Get/Set method of the periodFrom field
        /// </summary>
        [DisplayName("Period From")]
        public DateTime? PeriodFrom
        {
            get
            {
                return periodFrom;
            }

            set
            {
                this.IsChanged = true;
                periodFrom = value;
            }
        }

        /// <summary>
        /// Get/Set method of the periodTo field
        /// </summary>
        [DisplayName("Period To")]
        public DateTime? PeriodTo
        {
            get
            {
                return periodTo;
            }

            set
            {
                this.IsChanged = true;
                periodTo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the extendOnReload field
        /// </summary>
        [DisplayName("Extend On Reload")]
        public bool ExtendOnReload
        {
            get
            {
                return extendOnReload;
            }
            set
            {
                this.IsChanged = true;
                extendOnReload = value;
            }
        }

        /// <summary>
        /// Get/Set method of the refundable field
        /// </summary>
        [DisplayName("Refundable")]
        public bool Refundable
        {
            get
            {
                return refundable;
            }

            set
            {
                this.IsChanged = true;
                refundable = value;
            }
        }

        /// <summary>
        /// Get/Set method of the timeFrom field
        /// </summary>
        [DisplayName("Time From")]
        public decimal? TimeFrom
        {
            get
            {
                return timeFrom;
            }

            set
            {
                this.IsChanged = true;
                timeFrom = value;
            }
        }

        /// <summary>
        /// Get/Set method of the timeTo field
        /// </summary>
        [DisplayName("Time To")]
        public decimal? TimeTo
        {
            get
            {
                return timeTo;
            }

            set
            {
                this.IsChanged = true;
                timeTo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Monday field
        /// </summary>
        [DisplayName("Monday")]
        public bool Monday
        {
            get
            {
                return monday;
            }

            set
            {
                this.IsChanged = true;
                monday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        [DisplayName("Tuesday")]
        public bool Tuesday
        {
            get
            {
                return tuesday;
            }

            set
            {
                this.IsChanged = true;
                tuesday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Wednesday field
        /// </summary>
        [DisplayName("Wednesday")]
        public bool Wednesday
        {
            get
            {
                return wednesday;
            }

            set
            {
                this.IsChanged = true;
                wednesday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Thursday field
        /// </summary>
        [DisplayName("Thursday")]
        public bool Thursday
        {
            get
            {
                return thursday;
            }

            set
            {
                this.IsChanged = true;
                thursday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Friday field
        /// </summary>
        [DisplayName("Friday")]
        public bool Friday
        {
            get
            {
                return friday;
            }

            set
            {
                this.IsChanged = true;
                friday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Saturday field
        /// </summary>
        [DisplayName("Saturday")]
        public bool Saturday
        {
            get
            {
                return saturday;
            }

            set
            {
                this.IsChanged = true;
                saturday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Sunday field
        /// </summary>
        [DisplayName("Sunday")]
        public bool Sunday
        {
            get
            {
                return sunday;
            }

            set
            {
                this.IsChanged = true;
                sunday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ticketAllowed field
        /// </summary>
        [DisplayName("Ticket Allowed")]
        public bool TicketAllowed
        {
            get
            {
                return ticketAllowed;
            }
            set
            {
                this.IsChanged = true;
                ticketAllowed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PauseAllowed field
        /// </summary>
        [DisplayName("PauseAllowed")]
        public bool PauseAllowed
        {
            get
            {
                return pauseAllowed;
            }

            set
            {
                this.IsChanged = true;
                pauseAllowed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the remarks field
        /// </summary>
        [DisplayName("remarks")]
        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                this.IsChanged = true;
                remarks = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary>
        [DisplayName("Expire With Membership")]
        public bool ExpireWithMembership
        {
            get
            {
                return expireWithMembership;
            }
            set
            {
                this.IsChanged = true;
                expireWithMembership = value;
            }
        }

        /// <summary>
        /// Get/Set method of the transactionId field
        /// </summary>
        [Browsable(false)]
        public int TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                this.IsChanged = true;
                transactionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the transactionLineId field
        /// </summary>
        [Browsable(false)]
        public int TransactionLineId
        {
            get
            {
                return transactionLineId;
            }

            set
            {
                this.IsChanged = true;
                transactionLineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the numberOfDays field
        /// </summary>
        [Browsable(false)]
        public int? NumberOfDays
        {
            get
            {
                return numberOfDays;
            }

            set
            {
                this.IsChanged = true;
                numberOfDays = value;
            }
        }

        /// <summary>
        /// Get/Set method of the minimumSaleAmount field
        /// </summary>
        [Browsable(false)]
        public decimal? MinimumSaleAmount
        {
            get
            {
                return minimumSaleAmount;
            }

            set
            {
                this.IsChanged = true;
                minimumSaleAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the loyaltyRuleId field
        /// </summary>
        [Browsable(false)]
        public int LoyaltyRuleId
        {
            get
            {
                return loyaltyRuleId;
            }

            set
            {
                this.IsChanged = true;
                loyaltyRuleId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the playStartTime field
        /// </summary>
        [Browsable(false)]
        public DateTime? PlayStartTime
        {
            get
            {
                return playStartTime;
            }
            set
            {
                this.IsChanged = true;
                playStartTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the validityStatus field
        /// </summary>
        public AccountDTO.AccountValidityStatus ValidityStatus
        {
            get
            {
                return validityStatus;
            }
            set
            {
                this.IsChanged = true;
                validityStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the forMembershipOnly field
        /// </summary>
        [Browsable(false)]
        public bool ForMembershipOnly
        {
            get
            {
                return forMembershipOnly;
            }
            set
            {
                this.IsChanged = true;
                forMembershipOnly = value;
            }
        }

        /// <summary>
        /// Get/Set method of the membershipId field
        /// </summary>
        [Browsable(false)]
        public int MembershipId
        {
            get
            {
                return membershipId;
            }

            set
            {
                this.IsChanged = true;
                membershipId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the membershipRewardsId field
        /// </summary>
        [Browsable(false)]
        public int MembershipRewardsId
        {
            get
            {
                return membershipRewardsId;
            }

            set
            {
                this.IsChanged = true;
                membershipRewardsId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }

            set
            {
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [Browsable(false)]
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set Methods for accountCreditPlusConsumptionDTOList field
        /// </summary>
        public List<AccountCreditPlusConsumptionDTO> AccountCreditPlusConsumptionDTOList
        {
            get
            {
                return accountCreditPlusConsumptionDTOList;
            }
            set
            {
                accountCreditPlusConsumptionDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Methods for accountCreditPlusPurchaseCriteriaDTOList field
        /// </summary>
        public List<AccountCreditPlusPurchaseCriteriaDTO> AccountCreditPlusPurchaseCriteriaDTOList
        {
            get
            {
                return accountCreditPlusPurchaseCriteriaDTOList;
            }
            set
            {
                accountCreditPlusPurchaseCriteriaDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Methods for entityOverrideDatesDTOList field
        /// </summary>
        public List<EntityOverrideDatesDTO> EntityOverrideDatesDTOList
        {
            get
            {
                return entityOverrideDatesDTOList;
            }
            set
            {
                entityOverrideDatesDTOList = value;
            }
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
                    return notifyingObjectIsChanged || accountCreditPlusId < 0;
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
        /// Returns whether customer or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (AccountCreditPlusConsumptionDTOList != null)
                {
                    foreach (var accountCreditPlusConsumptionDTO in AccountCreditPlusConsumptionDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || accountCreditPlusConsumptionDTO.IsChanged;
                    }
                }
                if (AccountCreditPlusPurchaseCriteriaDTOList != null)
                {
                    foreach (var accountCreditPlusPurchaseCriteriaDTO in AccountCreditPlusPurchaseCriteriaDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || accountCreditPlusPurchaseCriteriaDTO.IsChanged;
                    }
                }
                if (EntityOverrideDatesDTOList != null)
                {
                    foreach (var entityOverrideDatesDTO in EntityOverrideDatesDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || entityOverrideDatesDTO.IsChanged;
                    }
                }
                return isChangedRecursive;
            }
        }
        /// <summary>
        /// Get set method for SourceCreditPlusId
        /// </summary>
        public int SourceCreditPlusId
        {
            get
            {
                return sourceCreditPlusId;
            }
            set
            {
                this.IsChanged = true;
                sourceCreditPlusId = value;
            }
        }

        /// <summary>
        /// Get set method for subscriptionBillingScheduleId
        /// </summary>
        public int SubscriptionBillingScheduleId
        {
            get
            {
                return subscriptionBillingScheduleId;
            }
            set
            {
                this.IsChanged = true;
                subscriptionBillingScheduleId = value;
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

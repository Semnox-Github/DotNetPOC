/********************************************************************************************
 * Project Name - AccountGame DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        21-Feb-2019     Mushahid Faizan     Added "ISACTIVE" ,SearchByParameters
 *2.70.2      23-Jul-2019     Girish Kundar       Modified : Added Constructor with required Parameter
 *2.80.0      19-Mar-2020     Mathew NInan        Added new field ValidityStatus to track status of entitlements        
 *2.110.0     08-Dec-2020     Guru S A            Subscription changes              
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountGame data object class. This acts as data holder for the AccountGame business object
    /// </summary>
    public class AccountGameDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by AccountGameId field
            /// </summary>
            ACCOUNT_GAME_ID,
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by AccountId List field
            /// </summary>
            ACCOUNT_ID_LIST,
            /// <summary>
            /// Search by GameId field
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by GameProfileId field
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by EXPIREWITHMEMBERSHIP field
            /// </summary>
            EXPIRE_WITH_MEMBERSHIP,
            /// <summary>
            /// Search by MEMBERSHIP_ID field
            /// </summary>
            MEMBERSHIP_ID,
            /// <summary>
            /// Search by MEMBERSHIP_REWARDS_ID field
            /// </summary>
            MEMBERSHIP_REWARDS_ID,
            /// <summary>
            /// Search by TRANSACTION_ID field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            ///  Search by isActive
            /// </summary>
            ISACTIVE,
            /// <summary>
            ///  Search by MasterEntity Id
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            ///  Search by ValidityStatus Id
            /// </summary>
            VALIDITYSTATUS,
            /// <summary>
            /// Search by TRANSACTION_ID_LIST field
            /// </summary>
            TRANSACTION_ID_LIST,
            /// <summary>
            /// Search by FROM_DATE field
            /// </summary>
            FROM_DATE,
            /// <summary>
            /// Search by SUBSCRIPTION_BILLING_SCHEDULE_ID field
            /// </summary>
            SUBSCRIPTION_BILLING_SCHEDULE_ID,
            /// <summary>
            /// Search by SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED field
            /// </summary>
            SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED
        }

        private int accountGameId;
        private int accountId;
        private int gameId;
        private decimal quantity;
        private DateTime? expiryDate;
        private int gameProfileId;
        private string frequency;
        private DateTime? lastPlayedTime;
        private int balanceGames;
        private int transactionId;
        private int transactionLineId;
        private string entitlementType;
        private string optionalAttribute;
        private int customDataSetId;
        private bool ticketAllowed;
        private DateTime? fromDate;
        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
        private bool expireWithMembership;
        private int membershipId;
        private int membershipRewardsId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private AccountDTO.AccountValidityStatus validityStatus;
        private bool synchStatus;
        private string guid;
        private List<AccountGameExtendedDTO> accountGameExtendedDTOList;
        private List<EntityOverrideDatesDTO> entityOverrideDatesDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private int subscriptionBillingScheduleId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountGameDTO()
        {
            log.LogMethodEntry();
            accountGameId = -1;
            accountId = -1;
            masterEntityId = -1;
            gameId = -1;
            gameProfileId = -1;
            transactionId = -1;
            transactionLineId = -1;
            customDataSetId = -1;
            membershipId = -1;
            membershipRewardsId = -1;
            isActive = true;
            frequency = "N";
            sunday = true;
            monday = true;
            tuesday = true;
            wednesday = true;
            thursday = true;
            friday = true;
            saturday = true;
            ticketAllowed = true;
            siteId = -1;
            masterEntityId = -1;
            validityStatus = AccountDTO.AccountValidityStatus.Valid;
            subscriptionBillingScheduleId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountGameDTO(int accountGameId, int accountId, int gameId, decimal quantity,
                         DateTime? expiryDate, int gameProfileId, string frequency, DateTime? lastPlayedTime,
                         int balanceGames, int transactionId, int transactionLineId, string entitlementType,
                         string optionalAttribute, int customDataSetId, bool ticketAllowed, DateTime? fromDate,
                         bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday,
                         bool sunday, bool expireWithMembership, int membershipId, int membershipRewardsId, bool isActive,
                         AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId)
            :this()
        {
            log.LogMethodEntry(accountGameId, accountId, gameId, quantity, expiryDate, gameProfileId,
                               frequency, lastPlayedTime, balanceGames, transactionId, transactionLineId,
                               entitlementType, optionalAttribute, customDataSetId, ticketAllowed,
                               fromDate, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, expireWithMembership, membershipId, membershipRewardsId, isActive, validityStatus, subscriptionBillingScheduleId);
            this.accountGameId = accountGameId;
            this.accountId = accountId;
            this.gameId = gameId;
            this.quantity = quantity;
            this.expiryDate = expiryDate;
            this.gameProfileId = gameProfileId;
            this.frequency = frequency;
            this.lastPlayedTime = lastPlayedTime;
            this.balanceGames = balanceGames;
            this.transactionId = transactionId;
            this.transactionLineId = transactionLineId;
            this.entitlementType = entitlementType;
            this.optionalAttribute = optionalAttribute;
            this.customDataSetId = customDataSetId;
            this.ticketAllowed = ticketAllowed;
            this.fromDate = fromDate;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.expireWithMembership = expireWithMembership;
            this.membershipId = membershipId;
            this.membershipRewardsId = membershipRewardsId;
            this.isActive = isActive;
            this.validityStatus = validityStatus;
            this.subscriptionBillingScheduleId = subscriptionBillingScheduleId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountGameDTO(int accountGameId, int accountId, int gameId, decimal quantity,
                         DateTime? expiryDate, int gameProfileId, string frequency, DateTime? lastPlayedTime,
                         int balanceGames, int transactionId, int transactionLineId, string entitlementType,
                         string optionalAttribute, int customDataSetId, bool ticketAllowed, DateTime? fromDate,
                         bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday,
                         bool sunday, bool expireWithMembership, int membershipId, int membershipRewardsId,
                         string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                         int siteId, int masterEntityId, bool synchStatus, string guid, bool isActive, AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId)
            :this(accountGameId, accountId, gameId, quantity, expiryDate, gameProfileId,
                               frequency, lastPlayedTime, balanceGames, transactionId, transactionLineId,
                               entitlementType, optionalAttribute, customDataSetId, ticketAllowed,
                               fromDate, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, expireWithMembership, membershipId, membershipRewardsId, isActive, validityStatus, subscriptionBillingScheduleId)
        {
            log.LogMethodEntry(accountGameId, accountId, gameId, quantity, expiryDate, gameProfileId,
                               frequency, lastPlayedTime, balanceGames, transactionId, transactionLineId,
                               entitlementType, optionalAttribute, customDataSetId, ticketAllowed,
                               fromDate, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, expireWithMembership, membershipId, membershipRewardsId,
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid, isActive, validityStatus, subscriptionBillingScheduleId);
          
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int AccountGameId
        {
            get
            {
                return accountGameId;
            }

            set
            {
                this.IsChanged = true;
                accountGameId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the gameProfileId field
        /// </summary>
        [DisplayName("Game Profile")]
        public int GameProfileId
        {
            get
            {
                return gameProfileId;
            }

            set
            {
                this.IsChanged = true;
                gameProfileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the gameId field
        /// </summary>
        [DisplayName("Game")]
        public int GameId
        {
            get
            {
                return gameId;
            }

            set
            {
                this.IsChanged = true;
                gameId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the quantity field
        /// </summary>
        [DisplayName("Play Count / Entt. Value")]
        public decimal Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                this.IsChanged = true;
                quantity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the frequency field
        /// </summary>
        [DisplayName("Frequency")]
        public string Frequency
        {
            get
            {
                return frequency;
            }

            set
            {
                this.IsChanged = true;
                frequency = value;
            }
        }

        /// <summary>
        /// Get/Set method of the balanceGames field
        /// </summary>
        [DisplayName("Balance Games")]
        public int BalanceGames
        {
            get
            {
                return balanceGames;
            }

            set
            {
                this.IsChanged = true;
                balanceGames = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lastPlayedTime field
        /// </summary>
        [DisplayName("Last Played Time")]
        public DateTime? LastPlayedTime
        {
            get
            {
                return lastPlayedTime;
            }

            set
            {
                this.IsChanged = true;
                lastPlayedTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the fromDate field
        /// </summary>
        [DisplayName("From Date")]
        public DateTime? FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                this.IsChanged = true;
                fromDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the expiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
                this.IsChanged = true;
                expiryDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the entitlementType field
        /// </summary>
        [DisplayName("Entitlement Type")]
        public string EntitlementType
        {
            get
            {
                return entitlementType;
            }

            set
            {
                this.IsChanged = true;
                entitlementType = value;
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
        /// Get/Set method of the optionalAttribute field
        /// </summary>
        [DisplayName("Optional Attribute")]
        public string OptionalAttribute
        {
            get
            {
                return optionalAttribute;
            }

            set
            {
                this.IsChanged = true;
                optionalAttribute = value;
            }
        }

        /// <summary>
        /// Get/Set method of the monday field
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
        /// Get/Set method of the tuesday field
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
        /// Get/Set method of the wednesday field
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
        /// Get/Set method of the thursday field
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
        /// Get/Set method of the friday field
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
        /// Get/Set method of the saturday field
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
        /// Get/Set method of the sunday field
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
        /// Get/Set method of the expireWithMembership field
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
        /// Get/Set method of the validityStatus field
        /// </summary>
        [DisplayName("Validity Status")]
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
        /// Get/Set method of the customDataSetId field
        /// </summary>
        [Browsable(false)]
        public int CustomDataSetId
        {
            get
            {
                return customDataSetId;
            }

            set
            {
                this.IsChanged = true;
                customDataSetId = value;
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
        /// Get/Set Methods for accountGameExtendedDTOList field
        /// </summary>
        [Browsable(false)]
        public List<AccountGameExtendedDTO> AccountGameExtendedDTOList
        {
            get
            {
                return accountGameExtendedDTOList;
            }
            set
            {
                accountGameExtendedDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Methods for entityOverrideDatesDTOList field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the subscriptionBillingScheduleId field
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || accountGameId < 0;
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
                if (AccountGameExtendedDTOList != null)
                {
                    foreach (var accountGameExtendedDTO in AccountGameExtendedDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || accountGameExtendedDTO.IsChanged;
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

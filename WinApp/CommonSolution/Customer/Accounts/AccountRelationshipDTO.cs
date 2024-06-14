/********************************************************************************************
 * Project Name - AccountRelationship DTO
 * Description  - Data object of AccountRelationship
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00       02-Feb-2017   Lakshminarayana           Created 
 *2.70.2       19-Jul-2019    Girish Kundar            Modified : Added Constructor with required Parameter
 *2.140.0    23-June-2021      Prashanth                Modified : Added DailyLimtPercentage field
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountRelationship data object class. This acts as data holder for the AccountRelationship business object
    /// </summary>
    public class AccountRelationshipDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by AcountRelationshipId field
            /// </summary>
            ACCOUNT_RELATIONSHIP_ID,
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by list of AccountIds 
            /// </summary>
            ACCOUNT_ID_LIST,
            /// <summary>
            /// Search by RelatedAccountId field
            /// </summary>
            RELATED_ACCOUNT_ID,
            /// <summary>
            /// Search by list of RelatedAccountIds
            /// </summary>
            RELATED_ACCOUNT_ID_LIST,
            /// <summary>
            /// Search by Either AccountId or Related AccountId field
            /// </summary>
            EITHER_ACCOUNT_ID_OR_RELATED_ACCOUNT_ID,
            /// <summary>
            /// Search by List of Either AccountIds or Related AccountIds
            /// </summary>
            EITHER_ACCOUNT_ID_OR_RELATED_ACCOUNT_ID_LIST,
            /// <summary>
            /// Relationship where both accounts are valid
            /// </summary>
            VALID_ACCOUNTS,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int accountRelationshipId;
        private int accountId;
        private int relatedAccountId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        int? dailyLimitPercentage;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountRelationshipDTO()
        {
            log.LogMethodEntry();
            accountRelationshipId = -1;
            accountId = -1;
            masterEntityId = -1;
            relatedAccountId = -1;
            isActive = true;
            siteId = -1;
            dailyLimitPercentage = null;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountRelationshipDTO(int accountRelationshipId, int accountId, int relatedAccountId,
                                      bool isActive, int? dailyLimitPercentage = null)
            :this()
        {
            log.LogMethodEntry(accountRelationshipId, accountId, relatedAccountId, isActive);
            this.accountRelationshipId = accountRelationshipId;
            this.accountId = accountId;
            this.relatedAccountId = relatedAccountId;
            this.isActive = isActive;
            this.dailyLimitPercentage = dailyLimitPercentage;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountRelationshipDTO(int accountRelationshipId, int accountId, int relatedAccountId,
                                      bool isActive, string createdBy, DateTime creationDate,
                                      string lastUpdatedBy, DateTime lastUpdateDate, int siteId, 
                                      int masterEntityId, bool synchStatus, string guid, int? dailyLimitPercentage = null)
            :this(accountRelationshipId, accountId, relatedAccountId, isActive, dailyLimitPercentage)
        {
            log.LogMethodEntry(accountRelationshipId, accountId, relatedAccountId, isActive, 
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, 
                               masterEntityId, synchStatus, guid);
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
        /// Get/Set method of the accountRelationshipId field
        /// </summary>
        [DisplayName("Id")]
        public int AccountRelationshipId
        {
            get
            {
                return accountRelationshipId;
            }

            set
            {
                this.IsChanged = true;
                accountRelationshipId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the accountId field
        /// </summary>
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
        /// Get/Set method of the pOSTypeId field
        /// </summary>
        public int RelatedAccountId
        {
            get
            {
                return relatedAccountId;
            }

            set
            {
                this.IsChanged = true;
                relatedAccountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [DisplayName("Active?")]
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
        /// Get/Set method of the createdBy field
        /// </summary>
        [DisplayName("Created By")]
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
        /// Get/Set method of the creationDate field
        /// </summary>
        [DisplayName("Creation Date")]
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
        /// Get/Set method of the lastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
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
        /// Get/Set method of the lastUpdateDate field
        /// </summary>
        [DisplayName("Last Update Date")]
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
        /// Get/Set method of the DailyLimitPercentage field
        /// </summary>
        public int? DailyLimitPercentage
        {
            get
            {
                return dailyLimitPercentage;
            }
            set
            {
                this.IsChanged = true;
                dailyLimitPercentage = value;
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
                    return notifyingObjectIsChanged || accountRelationshipId < 0;
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

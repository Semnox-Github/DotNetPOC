/********************************************************************************************
 * Project Name - Account CreditPlus Purchase Criteria DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        23-Jul-2019     Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                           And Who columns
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountCreditPlusPurchaseCriteria data object class. This acts as data holder for the AccountCreditPlusPurchaseCriteria business object
    /// </summary>
    public class AccountCreditPlusPurchaseCriteriaDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by AccountCreditPlusPurchaseCriteriaId field
            /// </summary>
            ACCOUNT_CREDITPLUS_PURCHASE_CRITERIA_ID,
            /// <summary>
            /// Search by AccountCreditPlusId field
            /// </summary>
            ACCOUNT_CREDITPLUS_ID,
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by AccountId List field
            /// </summary>
            ACCOUNT_ID_LIST,
            /// <summary>
            /// Search by PosTypeId field
            /// </summary>
            POSTYPE_ID,
            /// <summary>
            /// Search by ProductId field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            ///  Search by MasterEntity Id
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int accountCreditPlusPurchaseCriteriaId;
        private int accountCreditPlusId;
        private int pOSTypeId;
        private int productId;
        private bool isActive;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountCreditPlusPurchaseCriteriaDTO()
        {
            log.LogMethodEntry();
            accountCreditPlusPurchaseCriteriaId = -1;
            accountCreditPlusId = -1;
            masterEntityId = -1;
            pOSTypeId = -1;
            productId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountCreditPlusPurchaseCriteriaDTO(int accountCreditPlusPurchaseCriteriaId, int accountCreditPlusId, int pOSTypeId, int productId)
            :this()
        {
            log.LogMethodEntry(accountCreditPlusPurchaseCriteriaId, accountCreditPlusId, pOSTypeId, productId);
            this.accountCreditPlusPurchaseCriteriaId = accountCreditPlusPurchaseCriteriaId;
            this.accountCreditPlusId = accountCreditPlusId;
            this.pOSTypeId = pOSTypeId;
            this.productId = productId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountCreditPlusPurchaseCriteriaDTO(int accountCreditPlusPurchaseCriteriaId, int accountCreditPlusId, int pOSTypeId, int productId,
                         string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid,
                         string createdBy ,DateTime creationDate)
            :this(accountCreditPlusPurchaseCriteriaId, accountCreditPlusId, pOSTypeId, productId)
        {
            log.LogMethodEntry(accountCreditPlusPurchaseCriteriaId, accountCreditPlusId, pOSTypeId, productId, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid, createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the accountCreditPlusPurchaseCriteriaId field
        /// </summary>
        [DisplayName("Id")]
        public int AccountCreditPlusPurchaseCriteriaId
        {
            get
            {
                return accountCreditPlusPurchaseCriteriaId;
            }

            set
            {
                this.IsChanged = true;
                accountCreditPlusPurchaseCriteriaId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the accountCreditPlusId field
        /// </summary>
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
        /// Get/Set method of the pOSTypeId field
        /// </summary>
        [DisplayName("POS Type")]
        public int POSTypeId
        {
            get
            {
                return pOSTypeId;
            }

            set
            {
                this.IsChanged = true;
                pOSTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the productId field
        /// </summary>
        [DisplayName("Product")]
        public int ProductId
        {
            get
            {
                return productId;
            }

            set
            {
                this.IsChanged = true;
                productId = value;
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
                this.IsChanged = true;
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
        /// Get/Set method of the isActive field
        /// </summary>
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
        /// Get/Set method of the CreationDate field
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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || this.accountCreditPlusPurchaseCriteriaId == -1;
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

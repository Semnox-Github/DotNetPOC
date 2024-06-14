/********************************************************************************************
 * Project Name - Semnox.Parafait.Customer.Accounts -AccountCreditPlusConsumptionDTO
 * Description  - AccountCreditPlusConsumptionDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        06-Mar-2019   Mushahid Faizan         Added isActive SearchByParameters.
 *2.70.2      23-Jul-2019   Girish Kundar           Modified : Added Constructor with required Parameter
 *                                                             and  Who columns
 *2.90.0      09-Sep-2020   Girish Kundar           Modified : Issue fix DTO fetching property instead of data member                                                              
 *2.110.0     10-Dec-2020   Guru S A                For Subscription changes                   
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountCreditPlusConsumption data object class. This acts as data holder for the AccountCreditPlusConsumption business object
    /// </summary>
    public class AccountCreditPlusConsumptionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by accountCreditplusConsumptionId field
            /// </summary>
            ACCOUNT_CREDITPLUS_CONSUMPTION_ID,
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
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int accountCreditPlusConsumptionId;
        private int accountCreditPlusId;
        private int pOSTypeId;
        private DateTime? expiryDate;
        private int productId;
        private int gameProfileId;
        private int gameId;
        private decimal? discountPercentage;
        private decimal? discountedPrice;
        private int? consumptionQty;
        private int? consumptionBalance;
        private int? quantityLimit;
        private int categoryId;
        private decimal? discountAmount;
        private int orderTypeId;
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
        public AccountCreditPlusConsumptionDTO()
        {
            log.LogMethodEntry();
            accountCreditPlusConsumptionId = -1;
            accountCreditPlusId = -1;
            masterEntityId = -1;
            pOSTypeId = -1;
            productId = -1;
            gameId = -1;
            gameProfileId = -1;
            categoryId = -1;
            orderTypeId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountCreditPlusConsumptionDTO(int accountCreditPlusConsumptionId, int accountCreditPlusId, int pOSTypeId, DateTime? expiryDate,
                         int productId, int gameProfileId, int gameId, decimal? discountPercentage, decimal? discountedPrice,
                         int? consumptionBalance, int? quantityLimit, int categoryId, decimal? discountAmount, int orderTypeId, bool isActive, int? consumptionQty)
            :this()
        {
            log.LogMethodEntry(accountCreditPlusConsumptionId, accountCreditPlusId, pOSTypeId, expiryDate,productId,
                               gameProfileId , gameId, discountPercentage, discountedPrice,
                               consumptionBalance, quantityLimit, categoryId, discountAmount, orderTypeId, isActive, consumptionQty);
            this.accountCreditPlusConsumptionId = accountCreditPlusConsumptionId;
            this.accountCreditPlusId = accountCreditPlusId;
            this.pOSTypeId = pOSTypeId;
            this.expiryDate = expiryDate;
            this.productId = productId;
            this.gameProfileId = gameProfileId;
            this.gameId = gameId;
            this.discountPercentage = discountPercentage;
            this.discountedPrice = discountedPrice;
            this.consumptionBalance = consumptionBalance;
            this.quantityLimit = quantityLimit;
            this.categoryId = categoryId;
            this.discountAmount = discountAmount;
            this.orderTypeId = orderTypeId;
            this.isActive = isActive;
            this.consumptionQty = consumptionQty;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountCreditPlusConsumptionDTO(int accountCreditPlusConsumptionId, int accountCreditPlusId, int pOSTypeId, DateTime? expiryDate,
                         int productId, int gameProfileId, int gameId, decimal? discountPercentage, decimal? discountedPrice,
                         int? consumptionBalance, int? quantityLimit, int categoryId, decimal? discountAmount, int orderTypeId,
                         string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid, bool isActive,
                         string createdBy ,DateTime creationDate, int? consumptionQty)
            : this(accountCreditPlusConsumptionId, accountCreditPlusId, pOSTypeId, expiryDate, productId,
                  gameProfileId, gameId, discountPercentage, discountedPrice,
                  consumptionBalance, quantityLimit, categoryId, discountAmount, orderTypeId, isActive, consumptionQty)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionId, accountCreditPlusId, pOSTypeId, expiryDate, productId,
                               gameProfileId, gameId, discountPercentage, discountedPrice,
                               consumptionBalance, quantityLimit, categoryId, discountAmount, orderTypeId, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid, isActive ,createdBy,creationDate, consumptionQty);
            
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
        /// Get/Set method of the accountCreditPlusConsumptionId field
        /// </summary>
        [Browsable(false)]
        public int AccountCreditPlusConsumptionId
        {
            get
            {
                return accountCreditPlusConsumptionId;
            }

            set
            {
                this.IsChanged = true;
                accountCreditPlusConsumptionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the accountCreditPlusId field
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
        /// Get/Set method of the pOSTypeId field
        /// </summary>
        [DisplayName("POS Counter")]
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
        /// Get/Set method of the categoryId field
        /// </summary>
        [DisplayName("Category")]
        public int CategoryId
        {
            get
            {
                return categoryId;
            }

            set
            {
                this.IsChanged = true;
                categoryId = value;
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
        /// Get/Set method of the orderTypeId field
        /// </summary>
        [DisplayName("Order Type")]
        public int OrderTypeId
        {
            get
            {
                return orderTypeId;
            }

            set
            {
                this.IsChanged = true;
                orderTypeId = value;
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
        [DisplayName("Games")]
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
        /// Get/Set method of the discountPercentage field
        /// </summary>
        [DisplayName("Discount Percentage")]
        public decimal? DiscountPercentage
        {
            get
            {
                return discountPercentage;
            }

            set
            {
                this.IsChanged = true;
                discountPercentage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the discountAmount field
        /// </summary>
        [DisplayName("Discount Amount")]
        public decimal? DiscountAmount
        {
            get
            {
                return discountAmount;
            }

            set
            {
                this.IsChanged = true;
                discountAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the discountedPrice field
        /// </summary>
        [DisplayName("Discounted Price")]
        public decimal? DiscountedPrice
        {
            get
            {
                return discountedPrice;
            }

            set
            {
                this.IsChanged = true;
                discountedPrice = value;
            }
        }

        /// <summary>
        /// Get/Set method of the consumptionBalance field
        /// </summary>
        [DisplayName("Balance")]
        public int? ConsumptionBalance
        {
            get
            {
                return consumptionBalance;
            }

            set
            {
                this.IsChanged = true;
                consumptionBalance = value;
            }
        }

        /// <summary>
        /// Get/Set method of the consumptionQty field
        /// </summary>
        [DisplayName("Consumption Qty")]
        public int? ConsumptionQty
        {
            get
            {
                return consumptionQty;
            }

            set
            {
                this.IsChanged = true;
                consumptionQty = value;
            }
        }

        /// <summary>
        /// Get/Set method of the quantityLimit field
        /// </summary>
        [DisplayName("Daily Limit")]
        public int? QuantityLimit
        {
            get
            {
                return quantityLimit;
            }

            set
            {
                this.IsChanged = true;
                quantityLimit = value;
            }
        }

        /// <summary>
        /// Get/Set method of the expiryDate field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the lastUpdatedBy field
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
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lastUpdateDate field
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
                this.IsChanged = true;
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
                this.IsChanged = true;
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
                this.IsChanged = true;
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || accountCreditPlusConsumptionId < 0;
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

/********************************************************************************************
* Project Name - SubscriptionHeaderHistoryDTO
* Description - DTO for SubscriptionHeaderHistory 
*
**************
**Version Log 
**************
*Version    Date        Modified By          Remarks
*********************************************************************************************
*2.110.0     14-Dec-2020    Fiona              Created for subscription feature
*2.120.0     18-Mar-2021    Guru S A           For Subscription phase 2 changes
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionHeaderHistoryDTO Class
    /// </summary>
    public class SubscriptionHeaderHistoryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int subscriptionHeaderHistoryId;
        private int subscriptionHeaderId;
        private string subscriptionNumber;
        private int transactionId;
        private int transactionLineId;
        private int customerId;
        private int customerContactId;
        private int customerCreditCardsID;
        private int productSubscriptionId;
        private string productSubscriptionName;
        private string productSubscriptionDescription;
        private decimal subscriptionPrice;
        private int subscriptionCycle;
        private string unitOfSubscriptionCycle;
        private int subscriptionCycleValidity;
        //private bool seasonalSubscription;
        private DateTime? seasonStartDate;
        //private DateTime? seasonEndDate;
        private int? freeTrialPeriodCycle;
        private bool allowPause;
        private bool billInAdvance;
        private string subscriptionPaymentCollectionMode;
        private string selectedPaymentCollectionMode;
        private bool autoRenew;
        private decimal? autoRenewalMarkupPercent;
        private int? renewalGracePeriodCycle;
        private int? noOfRenewalReminders;
        private int? reminderFrequencyInDays;
        private int? sendFirstReminderBeforeXDays;
        private DateTime? lastRenewalReminderSentOn;
        private int? renewalReminderCount; 
        private DateTime? lastPaymentRetryLimitReminderSentOn;
        private int? paymentRetryLimitReminderCount;
        private string status;
        private bool taxInclusivePrice;
        private int productsId;
        private int sourceSubscriptionHeaderId;
        private DateTime subscriptionStartDate;
        private DateTime subscriptionEndDate;
        private string pausedBy;
        private string pauseApprovedBy;
        private string unPausedBy;
        private string unPauseApprovedBy;
        private string cancellationOption;
        private string cancelledBy;
        private string cancellationApprovedBy;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private List<SubscriptionBillingScheduleHistoryDTO> subscriptionBillingScheduleHistoryDTOList;

        

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Search Parameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            SUBSCRIPTION_HEADER_HISTORY_ID,
            /// <summary>
            /// Search by  ProductsId field
            /// </summary>
            SUBSCRIPTION_HEADER_ID,
            /// <summary>
            /// Search by  ProductsIdList field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  IS_ACTIVE field
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
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SubscriptionHeaderHistoryDTO()
        {
            log.LogMethodEntry();
            this.subscriptionHeaderHistoryId = -1;
            this.subscriptionHeaderId = -1;
            this.transactionId = -1;
            this.transactionLineId = -1;
            this.customerId = -1;
            this.customerContactId = -1;
            this.customerCreditCardsID = -1;
            this.productSubscriptionId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            this.subscriptionBillingScheduleHistoryDTOList = null;
            this.sourceSubscriptionHeaderId = -1;
            this.subscriptionStartDate = DateTime.MinValue;
            this.subscriptionEndDate = DateTime.MinValue;
            log.LogMethodExit();
        }
        /// <summary>
        /// Required fields Constructor
        /// </summary>

        public SubscriptionHeaderHistoryDTO(int subscriptionHeaderHistoryId, int subscriptionHeaderId, int transactionId, int transactionLineId, 
            int customerId, int customerContactId, int customerCreditCardsID, int productSubscriptionId, string productSubscriptionName, 
            string productSubscriptionDescription, decimal subscriptionPrice, int subscriptionCycle, string unitOfSubscriptionCycle, 
            int subscriptionCycleValidity,// bool seasonalSubscription, 
            DateTime? seasonStartDate, //DateTime? seasonEndDate, 
            int? freeTrialPeriodCycle, 
            bool allowPause, bool billInAdvance, string subscriptionPaymentCollectionMode, string selectedPaymentCollectionMode, bool autoRenew, 
            decimal autoRenewalMarkupPercent, int? renewalGracePeriodCycle, int? noOfRenewalReminders, int? reminderFrequencyInDays, 
            int? sendFirstReminderBeforeXDays, string status, bool taxInclusivePrice, int productsId, DateTime? lastRenewalReminderSentOn, int? renewalReminderCount, 
            DateTime? lastPaymentRetryLimitReminderSentOn, int? paymentRetryLimitReminderCount, int sourceSubscriptionHeaderId, DateTime subscriptionStartDate,
            DateTime subscriptionEndDate, string pausedBy, string pauseApprovedBy, string unPausedBy, string unPauseApprovedBy, string cancellationOption,
            string cancelledBy, string cancellationApprovedBy, string subscriptionNumber) :this()
        {
            log.LogMethodEntry( subscriptionHeaderHistoryId,  subscriptionHeaderId,  transactionId,  transactionLineId,
             customerId,  customerContactId,  customerCreditCardsID,  productSubscriptionId,  productSubscriptionName,
             productSubscriptionDescription,  subscriptionPrice,  subscriptionCycle,  unitOfSubscriptionCycle,
             subscriptionCycleValidity, seasonStartDate, freeTrialPeriodCycle,
             allowPause,  billInAdvance,  subscriptionPaymentCollectionMode,  selectedPaymentCollectionMode,  autoRenew,
             autoRenewalMarkupPercent, renewalGracePeriodCycle,  noOfRenewalReminders,  reminderFrequencyInDays,
             sendFirstReminderBeforeXDays,  status,  taxInclusivePrice,  productsId, lastRenewalReminderSentOn, renewalReminderCount
             , lastPaymentRetryLimitReminderSentOn, paymentRetryLimitReminderCount, sourceSubscriptionHeaderId
             , subscriptionStartDate, subscriptionEndDate, pausedBy, pauseApprovedBy, unPausedBy, unPauseApprovedBy, cancellationOption, cancelledBy, cancellationApprovedBy, subscriptionNumber);
            this.subscriptionHeaderHistoryId = subscriptionHeaderHistoryId;
            this.subscriptionHeaderId = subscriptionHeaderId;
            this.transactionId = transactionId;
            this.transactionLineId = transactionLineId;
            this.customerId = customerId;
            this.customerContactId = customerContactId;
            this.customerCreditCardsID = customerCreditCardsID;
            this.productSubscriptionId = productSubscriptionId;
            this.productSubscriptionName = productSubscriptionName;
            this.productSubscriptionDescription = productSubscriptionDescription;
            this.subscriptionPrice = subscriptionPrice;
            this.subscriptionCycle = subscriptionCycle;
            this.unitOfSubscriptionCycle = unitOfSubscriptionCycle;
            this.subscriptionCycleValidity = subscriptionCycleValidity;
            //this.seasonalSubscription = seasonalSubscription;
            this.seasonStartDate = seasonStartDate;
            //this.seasonEndDate = seasonEndDate;
            this.freeTrialPeriodCycle = freeTrialPeriodCycle;
            this.allowPause = allowPause;
            this.billInAdvance = billInAdvance;
            this.subscriptionPaymentCollectionMode = subscriptionPaymentCollectionMode;
            this.selectedPaymentCollectionMode = selectedPaymentCollectionMode;
            this.autoRenew = autoRenew;
            this.autoRenewalMarkupPercent = autoRenewalMarkupPercent;
            this.renewalGracePeriodCycle = renewalGracePeriodCycle;
            this.noOfRenewalReminders = noOfRenewalReminders;
            this.reminderFrequencyInDays = reminderFrequencyInDays;
            this.sendFirstReminderBeforeXDays = sendFirstReminderBeforeXDays;
            this.status = status;
            this.taxInclusivePrice = taxInclusivePrice;
            this.productsId = productsId;
            this.lastRenewalReminderSentOn = lastRenewalReminderSentOn;
            this.renewalReminderCount = renewalReminderCount; 
            this.lastPaymentRetryLimitReminderSentOn = lastPaymentRetryLimitReminderSentOn;
            this.paymentRetryLimitReminderCount = paymentRetryLimitReminderCount;
            this.sourceSubscriptionHeaderId = sourceSubscriptionHeaderId;
            this.subscriptionStartDate = subscriptionStartDate;
            this.subscriptionEndDate = subscriptionEndDate;
            this.pausedBy = pausedBy;
            this.pauseApprovedBy = pauseApprovedBy;
            this.unPausedBy = unPausedBy;
            this.unPauseApprovedBy = unPauseApprovedBy;
            this.cancellationOption = cancellationOption;
            this.cancelledBy = cancelledBy;
            this.cancellationApprovedBy = cancellationApprovedBy;
            this.subscriptionNumber = subscriptionNumber;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all fields
        /// </summary>

        public SubscriptionHeaderHistoryDTO(int subscriptionHeaderHistoryId, int subscriptionHeaderId, int transactionId, 
            int transactionLineId, int customerId, int customerContactId, int customerCreditCardsID, int productSubscriptionId, 
            string productSubscriptionName, string productSubscriptionDescription, decimal subscriptionPrice, int subscriptionCycle, 
            string unitOfSubscriptionCycle, int subscriptionCycleValidity, //bool seasonalSubscription, 
            DateTime? seasonStartDate, // DateTime? seasonEndDate, 
            int? freeTrialPeriodCycle, bool allowPause, bool billInAdvance, string subscriptionPaymentCollectionMode, 
            string selectedPaymentCollectionMode, bool autoRenew, decimal autoRenewalMarkupPercent, int? renewalGracePeriodCycle, int? noOfRenewalReminders,
            int? reminderFrequencyInDays, int? sendFirstReminderBeforeXDays, string status, bool taxInclusivePrice, int productsId, DateTime? lastRenewalReminderSentOn, 
            int? renewalReminderCount, DateTime? lastPaymentRetryLimitReminderSentOn,
            int? paymentRetryLimitReminderCount, int sourceSubscriptionHeaderId, DateTime subscriptionStartDate,
            DateTime subscriptionEndDate, string pausedBy, string pauseApprovedBy, string unPausedBy, string unPauseApprovedBy, string cancellationOption,
            string cancelledBy, string cancellationApprovedBy, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId,
            bool synchStatus, string guid, string subscriptionNumber)
            : this(subscriptionHeaderHistoryId, subscriptionHeaderId, transactionId, transactionLineId, customerId, customerContactId, 
                  customerCreditCardsID, productSubscriptionId, productSubscriptionName, productSubscriptionDescription, subscriptionPrice, subscriptionCycle, 
                  unitOfSubscriptionCycle, subscriptionCycleValidity, seasonStartDate, freeTrialPeriodCycle, allowPause, 
                  billInAdvance, subscriptionPaymentCollectionMode, selectedPaymentCollectionMode, autoRenew, autoRenewalMarkupPercent, renewalGracePeriodCycle, 
                  noOfRenewalReminders, reminderFrequencyInDays, sendFirstReminderBeforeXDays, status, taxInclusivePrice, productsId, lastRenewalReminderSentOn, renewalReminderCount, 
                  lastPaymentRetryLimitReminderSentOn, paymentRetryLimitReminderCount, sourceSubscriptionHeaderId, subscriptionStartDate, subscriptionEndDate, pausedBy,
                  pauseApprovedBy, unPausedBy, unPauseApprovedBy, cancellationOption, cancelledBy, cancellationApprovedBy, subscriptionNumber)
        {
            log.LogMethodEntry( subscriptionHeaderHistoryId,  subscriptionHeaderId,  transactionId, transactionLineId,  customerId,  customerContactId,  customerCreditCardsID,  productSubscriptionId,
             productSubscriptionName,  productSubscriptionDescription,  subscriptionPrice,  subscriptionCycle, unitOfSubscriptionCycle,  subscriptionCycleValidity, seasonStartDate,
              freeTrialPeriodCycle,  allowPause,  billInAdvance,  subscriptionPaymentCollectionMode, selectedPaymentCollectionMode,  autoRenew,  autoRenewalMarkupPercent, 
              renewalGracePeriodCycle,   noOfRenewalReminders,
             reminderFrequencyInDays,  sendFirstReminderBeforeXDays,  status,  taxInclusivePrice,  productsId, lastRenewalReminderSentOn, renewalReminderCount,
             lastPaymentRetryLimitReminderSentOn, paymentRetryLimitReminderCount, sourceSubscriptionHeaderId, subscriptionStartDate, subscriptionEndDate,
             pausedBy, pauseApprovedBy, unPausedBy, unPauseApprovedBy, cancellationOption, cancelledBy, cancellationApprovedBy, isActive, createdBy,  
             creationDate,  lastUpdatedBy,  lastUpdateDate,  siteId,  masterEntityId,  synchStatus,  guid, subscriptionNumber);
            this.isActive = isActive;
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
        /// Get/Set method of the SubscriptionHeaderHistoryId field
        /// </summary>
        public int SubscriptionHeaderHistoryId
        {
            get { return subscriptionHeaderHistoryId; }
            set { subscriptionHeaderHistoryId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionHeaderId field
        /// </summary>
        public int SubscriptionHeaderId
        {
            get { return subscriptionHeaderId; }
            set { subscriptionHeaderId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>

        public int TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TransactionLineId field
        /// </summary>
        public int TransactionLineId
        {
            get { return transactionLineId; }
            set { transactionLineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CustomerContactId field
        /// </summary>
        public int CustomerContactId
        {
            get { return customerContactId; }
            set { customerContactId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CustomerCreditCardsID field
        /// </summary>
        public int CustomerCreditCardsID
        {
            get { return customerCreditCardsID; }
            set { customerCreditCardsID = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductSubscriptionId field
        /// </summary>
        public int ProductSubscriptionId
        {
            get { return productSubscriptionId; }
            set { productSubscriptionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductSubscriptionName field
        /// </summary>
        public string ProductSubscriptionName
        {
            get { return productSubscriptionName; }
            set { productSubscriptionName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductSubscriptionDescription field
        /// </summary>
        public string ProductSubscriptionDescription
        {
            get { return productSubscriptionDescription; }
            set { productSubscriptionDescription = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionPrice field
        /// </summary>
        public decimal SubscriptionPrice
        {
            get { return subscriptionPrice; }
            set { subscriptionPrice = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionCycle field
        /// </summary>
        public int SubscriptionCycle
        {
            get { return subscriptionCycle; }
            set { subscriptionCycle = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductsId field
        /// </summary>
        public string UnitOfSubscriptionCycle
        {
            get { return unitOfSubscriptionCycle; }
            set { unitOfSubscriptionCycle = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionCycleValidity field
        /// </summary>
        public int SubscriptionCycleValidity
        {
            get { return subscriptionCycleValidity; }
            set { subscriptionCycleValidity = value; this.IsChanged = true; }
        }
        ///// <summary>
        ///// Get/Set method of the SeasonalSubscription field
        ///// </summary>
        //public bool SeasonalSubscription
        //{
        //    get { return seasonalSubscription; }
        //    set { seasonalSubscription = value; this.IsChanged = true; }
        //}
        /// <summary>
        /// Get/Set method of the SeasonStartDate field
        /// </summary>
        public DateTime? SeasonStartDate
        {
            get { return seasonStartDate; }
            set { seasonStartDate = value; this.IsChanged = true; }
        }
        ///// <summary>
        ///// Get/Set method of the SeasonEndDate field
        ///// </summary>
        //public DateTime? SeasonEndDate
        //{
        //    get { return seasonEndDate; }
        //    set { seasonEndDate = value; this.IsChanged = true; }
        //}

        /// <summary>
        /// Get/Set method of the FreeTrialPeriodCycle field
        /// </summary>
        public int? FreeTrialPeriodCycle
        {
            get { return freeTrialPeriodCycle; }
            set { freeTrialPeriodCycle = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AllowPause field
        /// </summary>
        public bool AllowPause
        {
            get { return allowPause; }
            set { allowPause = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the BillInAdvance field
        /// </summary>

        public bool BillInAdvance
        {
            get { return billInAdvance; }
            set { billInAdvance = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionPaymentCollectionMode field
        /// </summary>
        public string SubscriptionPaymentCollectionMode
        {
            get { return subscriptionPaymentCollectionMode; }
            set { subscriptionPaymentCollectionMode = value; }
        }
        /// <summary>
        /// Get/Set method of the PaymentCollectionMode field
        /// </summary>

        public string SelectedPaymentCollectionMode
        {
            get { return selectedPaymentCollectionMode; }
            set { selectedPaymentCollectionMode = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AutoRenew field
        /// </summary>
        public bool AutoRenew
        {
            get { return autoRenew; }
            set { autoRenew = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AutoRenewalMarkupPercent field
        /// </summary>
        public decimal? AutoRenewalMarkupPercent
        {
            get { return autoRenewalMarkupPercent; }
            set { autoRenewalMarkupPercent = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the RenewalGracePeriodCycle field
        /// </summary>
        public int? RenewalGracePeriodCycle
        {
            get { return renewalGracePeriodCycle; }
            set { renewalGracePeriodCycle = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the NoOfRenewalReminders field
        /// </summary>
        public int? NoOfRenewalReminders
        {
            get { return noOfRenewalReminders; }
            set { noOfRenewalReminders = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SendFirstReminderBeforeXDays field
        /// </summary>
        public int? SendFirstReminderBeforeXDays
        {
            get { return sendFirstReminderBeforeXDays; }
            set { sendFirstReminderBeforeXDays = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ReminderFrequencyInDays field
        /// </summary>
        public int? ReminderFrequencyInDays
        {
            get { return reminderFrequencyInDays; }
            set { reminderFrequencyInDays = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TaxInclusivePrice field
        /// </summary>
        public bool TaxInclusivePrice
        {
            get { return taxInclusivePrice; }
            set { taxInclusivePrice = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductsId  field
        /// </summary>
        public int ProductsId
        {
            get { return productsId; }
            set { productsId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>

        /// <summary>
        /// Get/Set method of the lastRenewalReminderSentOn field
        /// </summary>
        public DateTime? LastRenewalReminderSentOn
        {
            get { return lastRenewalReminderSentOn; }
            set { lastRenewalReminderSentOn = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the RenewalReminderCount field
        /// </summary>
        public int? RenewalReminderCount
        {
            get { return renewalReminderCount; }
            set { renewalReminderCount = value; this.IsChanged = true; }
        } 
        /// <summary>
        /// Get/Set method of the LastPaymentRetryLimitReminderSentOn field
        /// </summary>
        public DateTime? LastPaymentRetryLimitReminderSentOn
        {
            get { return lastPaymentRetryLimitReminderSentOn; }
            set { lastPaymentRetryLimitReminderSentOn = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PaymentRetryLimitReminderCount field
        /// </summary>
        public int? PaymentRetryLimitReminderCount
        {
            get { return paymentRetryLimitReminderCount; }
            set { paymentRetryLimitReminderCount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SourceSubscriptionHeaderId field
        /// </summary>
        public int SourceSubscriptionHeaderId
        {
            get { return sourceSubscriptionHeaderId; }
            set { sourceSubscriptionHeaderId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionStartDate field
        /// </summary>
        public DateTime SubscriptionStartDate
        {
            get { return subscriptionStartDate; }
            set { subscriptionStartDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionEndDate field
        /// </summary>
        public DateTime SubscriptionEndDate
        {
            get { return subscriptionEndDate; }
            set { subscriptionEndDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PausedBy field
        /// </summary>
        public string PausedBy
        {
            get { return pausedBy; }
            set { pausedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PauseApprovedBy field
        /// </summary>
        public string PauseApprovedBy
        {
            get { return pauseApprovedBy; }
            set { pauseApprovedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UnPausedBy field
        /// </summary>
        public string UnPausedBy
        {
            get { return unPausedBy; }
            set { unPausedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the unPauseApprovedBy field
        /// </summary>
        public string UnPauseApprovedBy
        {
            get { return unPauseApprovedBy; }
            set { unPauseApprovedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the cancellationOption field
        /// </summary>
        public string CancellationOption
        {
            get { return cancellationOption; }
            set { cancellationOption = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PausedBy field
        /// </summary>
        public string CancelledBy
        {
            get { return cancelledBy; }
            set { cancelledBy = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CancellationApprovedBy field
        /// </summary>
        public string CancellationApprovedBy
        {
            get { return cancellationApprovedBy; }
            set { cancellationApprovedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of IsActive 
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
        /// SubscriptionNumber
        /// </summary>
        public string SubscriptionNumber
        {
            get
            {
                return subscriptionNumber;
            }

            set
            {
                this.IsChanged = true;
                subscriptionNumber = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the SubscriptionBillingScheduleHistoryDTOList field
        /// </summary>
        public List<SubscriptionBillingScheduleHistoryDTO> SubscriptionBillingScheduleHistoryDTOList
        {
            get { return subscriptionBillingScheduleHistoryDTOList; }
            set { subscriptionBillingScheduleHistoryDTOList = value; this.IsChanged = true; }

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
                    return notifyingObjectIsChanged || subscriptionHeaderHistoryId < 0;
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

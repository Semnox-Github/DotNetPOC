/********************************************************************************************
 * Project Name - SubscriptionHeader DTO  
 * Description  - DTO class for SubscriptionHeader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Guru S A           Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A           For Subscription phase 2 changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionHeaderDTO
    /// </summary>
    public class SubscriptionHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int subscriptionHeaderId;
        private string subscriptionNumber;
        private int transactionId;
        private int transactionLineId;
        private int customerId;
        private int customerContactId;
        private int customerCreditCardsId;
        private int productsId;
        private int productSubscriptionId;
        private string productSubscriptionName;
        private string productSubscriptionDescription;
        private decimal subscriptionPrice;
        private bool taxInclusivePrice;
        private int subscriptionCycle;
        private string unitOfSubscriptionCycle;
        private int subscriptionCycleValidity;
        //private bool seasonalSubscription;
        private DateTime? seasonStartDate;
        //private DateTime? seasonEndDate;
        private int? freeTrialPeriodCycle;
        private bool billInAdvance;
        private string subscriptionPaymentCollectionMode;
        private string selectedPaymentCollectionMode;
        private bool autoRenew;
        private decimal autoRenewalMarkupPercent;
        private int? renewalGracePeriodCycle;
        private int? noOfRenewalReminders;
        private int? reminderFrequencyInDays;
        private int? sendFirstReminderBeforeXDays;
        private DateTime? lastRenewalReminderSentOn;
        private int? renewalReminderCount;
        private DateTime? lastPaymentRetryLimitReminderSentOn;
        private int? paymentRetryLimitReminderCount;
        private bool allowPause;
        private string status;
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
        private List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  subscriptionHeaderId field
            /// </summary>
            SUBSCRIPTION_HEADER_ID,
            /// <summary>
            /// Search by  transactionId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by  transactionLineId field
            /// </summary>
            TRANSACTION_LINE_ID,
            /// <summary>
            /// Search by  customerId field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  customerContactId field
            /// </summary>
            CUSTOMER_CONTACT_ID,
            /// <summary>
            /// Search by  customerCreditCardsID field
            /// </summary>
            CUSTOMER_CREDIT_CARD_ID,
            /// <summary>
            /// Search by  PRODUCTS_ID field
            /// </summary>
            PRODUCTS_ID,
            /// <summary>
            /// Search by  productSubscriptionId field
            /// </summary>
            PRODUCT_SUBSCRIPTION_ID,
            /// <summary>
            /// Search by  productSubscriptionName field
            /// </summary>
            PRODUCT_SUBSCRIPTION_NAME,
            /// <summary>
            /// Search by  seasonalSubscription field
            /// </summary>
            SEASONAL_SUBSCRIPTION,
            /// <summary>
            /// Search by  autoRenew field
            /// </summary>
            AUTO_RENEW,
            /// <summary>
            /// Search by  selectedPaymentCollectionMode field
            /// </summary>
            SELECTED_PAYMENT_COLLECTION_MODE,
            /// <summary>
            /// Search by  status field
            /// </summary>
            STATUS,
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
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search for HAS_PAST_PENDING_BILL_CYCLES
            /// </summary>
            HAS_PAST_PENDING_BILL_CYCLES,
            /// <summary>
            /// Search for EXPIRES_IN_X_DAYS_IS_TRUE
            /// </summary>
            RENEWAL_REMINDER_IN_X_DAYS_IS_TRUE,
            /// <summary>
            /// Search for NOT_REACHED_PAYMENT_RETRY_LIMIT
            /// </summary>
            NOT_REACHED_PAYMENT_RETRY_LIMIT,
            /// <summary>
            /// Search for REACHED_PAYMENT_RETRY_LIMIT
            /// </summary>
            REACHED_PAYMENT_RETRY_LIMIT,
            /// <summary>
            /// Search for HAS_UNBILLED_CYCLES
            /// </summary>
            HAS_UNBILLED_CYCLES,
            /// <summary>
            /// Search for CUSTOMER_FIRST_NAME_LIKE
            /// </summary>
            CUSTOMER_FIRST_NAME_LIKE,
            /// <summary>
            /// Search for LATEST_BILL_CYCLE_HAS_PAYMENT_ERROR
            /// </summary>
            LATEST_BILL_CYCLE_HAS_PAYMENT_ERROR,
            /// <summary>
            /// Search for HAS_EXPIRED_CREDIT_CARD
            /// </summary>
            HAS_EXPIRED_CREDIT_CARD,
            /// <summary>
            /// Search for CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING
            /// </summary>
            CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING,
            /// <summary>
            /// Search for SUBSCRIPTION_EXPIRES_IN_XDAYS
            /// </summary>
            SUBSCRIPTION_EXPIRES_IN_XDAYS,
            /// <summary>
            /// Search for CREATION_DATE_LESS_THAN
            /// </summary> 
            CREATION_DATE_LESS_THAN,
            /// <summary>
            /// Search for CREATION_DATE_GREATER_EQUAL_TO
            /// </summary> 
            CREATION_DATE_GREATER_EQUAL_TO,
            /// <summary>
            /// Search for CANCELLATION_DATE_LESS_THAN
            /// </summary> 
            CANCELLATION_DATE_LESS_THAN,
            /// <summary>
            /// Search for CANCELLATION_DATE_GREATER_EQUAL_TO
            /// </summary> 
            CANCELLATION_DATE_GREATER_EQUAL_TO,
            /// <summary>
            /// Search for SUBSCRIPTION_IS_EXPIRED
            /// </summary> 
            SUBSCRIPTION_IS_EXPIRED,
            /// <summary>
            /// Search for RENEWED
            /// </summary> 
            RENEWED,
            /// <summary>
            /// Search for SOURCE_SUBSCRIPTION_HEADER_ID
            /// </summary> 
            SOURCE_SUBSCRIPTION_HEADER_ID,
            /// <summary>
            /// Search for TRX_STATUS
            /// </summary> 
            TRX_STATUS
        }
        /// <summary>
        /// SubscriptionHeaderDTO
        /// </summary>
        public SubscriptionHeaderDTO()
        {
            log.LogMethodEntry();
            subscriptionHeaderId = -1;
            transactionId = -1;
            transactionLineId = -1;
            customerId = -1;
            customerContactId = -1;
            customerCreditCardsId = -1;
            productsId = -1;
            productSubscriptionId = -1;
            subscriptionPrice = 0;
            subscriptionCycle = 0;
            subscriptionCycleValidity = 0;
            isActive = true;
            //seasonalSubscription = false;
            billInAdvance = false;
            allowPause = false;
            taxInclusivePrice = false;
            siteId = -1;
            masterEntityId = -1;
            status = SubscriptionStatus.ACTIVE;
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>();
            sourceSubscriptionHeaderId = -1;
            subscriptionStartDate = DateTime.MinValue;
            subscriptionEndDate = DateTime.MinValue;
            log.LogMethodExit();
        }
        /// <summary>
        /// SubscriptionHeaderDTO
        /// </summary> 
        public SubscriptionHeaderDTO(int subscriptionHeaderId, int transactionId, int transactionLineId, int customerId, int customerContactId, int customerCreditCardsId, int productsId, int productSubscriptionId,
                                    string productSubscriptionName, string productSubscriptionDescription, decimal subscriptionPrice, bool taxInclusivePrice, int subscriptionCycle, string unitOfSubscriptionCycle,
                                    int subscriptionCycleValidity, //bool seasonalSubscription, 
                                    DateTime? seasonStartDate, //DateTime? seasonEndDate,
                                    int? freeTrialPeriodCycle, bool billInAdvance,
                                    string subscriptionPaymentCollectionMode, string selectedPaymentCollectionMode, bool autoRenew, decimal autoRenewalMarkupPercent, int? renewalGracePeriodCycle,
                                    int? noOfRenewalReminders, int? reminderFrequencyInDays, int? sendFirstReminderBeforeXDays, bool allowPause, string status,
                                    DateTime? lastRenewalReminderSentOn, int? renewalReminderCount,
                                    DateTime? lastPaymentRetryLimitReminderSentOn, int? paymentRetryLimitReminderCount, int sourceSubscriptionHeaderId, DateTime subscriptionStartDate,
                                    DateTime subscriptionEndDate, string pausedBy, string pauseApprovedBy, string unPausedBy, string unPauseApprovedBy, string cancellationOption, 
                                    string cancelledBy, string cancellationApprovedBy, string subscriptionNumber) : this()
        {
            log.LogMethodEntry(subscriptionHeaderId, transactionId, transactionLineId, customerId, customerContactId, customerCreditCardsId, productsId, productSubscriptionId,
                               productSubscriptionName, productSubscriptionDescription, subscriptionPrice, taxInclusivePrice, subscriptionCycle, unitOfSubscriptionCycle,
                               subscriptionCycleValidity, seasonStartDate, freeTrialPeriodCycle, billInAdvance,
                               subscriptionPaymentCollectionMode, selectedPaymentCollectionMode, autoRenew, autoRenewalMarkupPercent, renewalGracePeriodCycle,
                               noOfRenewalReminders, reminderFrequencyInDays, sendFirstReminderBeforeXDays, allowPause, status,
                               lastRenewalReminderSentOn, renewalReminderCount, lastPaymentRetryLimitReminderSentOn, paymentRetryLimitReminderCount, sourceSubscriptionHeaderId,
                               subscriptionStartDate, subscriptionEndDate, pausedBy, pauseApprovedBy, unPausedBy, unPauseApprovedBy, cancellationOption, cancelledBy, cancellationApprovedBy, subscriptionNumber);
            this.subscriptionHeaderId = subscriptionHeaderId;
            this.transactionId = transactionId;
            this.transactionLineId = transactionLineId;
            this.customerId = customerId;
            this.customerContactId = customerContactId;
            this.customerCreditCardsId = customerCreditCardsId;
            this.productsId = productsId;
            this.productSubscriptionId = productSubscriptionId;
            this.productSubscriptionName = productSubscriptionName;
            this.productSubscriptionDescription = productSubscriptionDescription;
            this.subscriptionPrice = subscriptionPrice;
            this.taxInclusivePrice = taxInclusivePrice;
            this.subscriptionCycle = subscriptionCycle;
            this.unitOfSubscriptionCycle = unitOfSubscriptionCycle;
            this.subscriptionCycleValidity = subscriptionCycleValidity;
            //this.seasonalSubscription = seasonalSubscription;
            this.seasonStartDate = seasonStartDate;
            //this.seasonEndDate = seasonEndDate;
            this.freeTrialPeriodCycle = freeTrialPeriodCycle;
            this.billInAdvance = billInAdvance;
            this.subscriptionPaymentCollectionMode = subscriptionPaymentCollectionMode;
            this.selectedPaymentCollectionMode = selectedPaymentCollectionMode;
            this.autoRenew = autoRenew;
            this.autoRenewalMarkupPercent = autoRenewalMarkupPercent;
            this.renewalGracePeriodCycle = renewalGracePeriodCycle;
            this.noOfRenewalReminders = noOfRenewalReminders;
            this.reminderFrequencyInDays = reminderFrequencyInDays;
            this.sendFirstReminderBeforeXDays = sendFirstReminderBeforeXDays;
            this.allowPause = allowPause;
            this.status = status;
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
        /// SubscriptionHeaderDTO
        /// </summary> 
        public SubscriptionHeaderDTO(int subscriptionHeaderId, int transactionId, int transactionLineId, int customerId, int customerContactId, int customerCreditCardsId, int productsId, int productSubscriptionId,
                                    string productSubscriptionName, string productSubscriptionDescription, decimal subscriptionPrice, bool taxInclusivePrice, int subscriptionCycle, string unitOfSubscriptionCycle,
                                    int subscriptionCycleValidity, //bool seasonalSubscription, 
                                    DateTime? seasonStartDate, //DateTime? seasonEndDate, 
                                    int? freeTrialPeriodCycle, bool billInAdvance,
                                    string subscriptionPaymentCollectionMode, string selectedPaymentCollectionMode, bool autoRenew, decimal autoRenewalMarkupPercent, int? renewalGracePeriodCycle,
                                    int? noOfRenewalReminders, int? reminderFrequencyInDays, int? sendFirstReminderBeforeXDays, bool allowPause, string status,
                                    DateTime? lastRenewalReminderSentOn, int? renewalReminderCount,
                                    DateTime? lastPaymentRetryLimitReminderSentOn, int? paymentRetryLimitReminderCount, int sourceSubscriptionHeaderId, DateTime subscriptionStartDate, 
                                    DateTime subscriptionEndDate, string pausedBy, string pauseApprovedBy, string unPausedBy, string unPauseApprovedBy, string cancellationOption, 
                                    string cancelledBy, string cancellationApprovedBy, 
                                    bool isActive, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid,
                                    string subscriptionNumber)
            : this(subscriptionHeaderId, transactionId, transactionLineId, customerId, customerContactId, customerCreditCardsId, productsId, productSubscriptionId,
                               productSubscriptionName, productSubscriptionDescription, subscriptionPrice, taxInclusivePrice, subscriptionCycle, unitOfSubscriptionCycle,
                               subscriptionCycleValidity, seasonStartDate, freeTrialPeriodCycle, billInAdvance,
                               subscriptionPaymentCollectionMode, selectedPaymentCollectionMode, autoRenew, autoRenewalMarkupPercent, renewalGracePeriodCycle,
                               noOfRenewalReminders, reminderFrequencyInDays, sendFirstReminderBeforeXDays, allowPause, status, lastRenewalReminderSentOn, renewalReminderCount,
                               lastPaymentRetryLimitReminderSentOn, paymentRetryLimitReminderCount, sourceSubscriptionHeaderId, subscriptionStartDate, subscriptionEndDate, pausedBy,
                               pauseApprovedBy, unPausedBy, unPauseApprovedBy, cancellationOption, cancelledBy, cancellationApprovedBy, subscriptionNumber)

        {
            log.LogMethodEntry(subscriptionHeaderId, transactionId, transactionLineId, customerId, customerContactId, customerCreditCardsId, productsId, productSubscriptionId,
                               productSubscriptionName, productSubscriptionDescription, subscriptionPrice, taxInclusivePrice, subscriptionCycle, unitOfSubscriptionCycle,
                               subscriptionCycleValidity, seasonStartDate, freeTrialPeriodCycle, billInAdvance,
                               subscriptionPaymentCollectionMode, selectedPaymentCollectionMode, autoRenew, autoRenewalMarkupPercent, renewalGracePeriodCycle,
                               noOfRenewalReminders, reminderFrequencyInDays, sendFirstReminderBeforeXDays, allowPause, status, lastRenewalReminderSentOn, renewalReminderCount,
                               lastPaymentRetryLimitReminderSentOn, paymentRetryLimitReminderCount, sourceSubscriptionHeaderId, subscriptionStartDate, subscriptionEndDate,
                               pausedBy, pauseApprovedBy, unPausedBy, unPauseApprovedBy, cancellationOption, cancelledBy, cancellationApprovedBy, 
                               isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid,
                               subscriptionNumber);
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
        /// Get/Set method of the subscriptionHeaderId field
        /// </summary>
        public int SubscriptionHeaderId
        {
            get { return subscriptionHeaderId; }
            set { subscriptionHeaderId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the transactionId field
        /// </summary>
        public int TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the transactionLineId field
        /// </summary>
        public int TransactionLineId
        {
            get { return transactionLineId; }
            set { transactionLineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the customerId field
        /// </summary>
        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the customerContactId field
        /// </summary>
        public int CustomerContactId
        {
            get { return customerContactId; }
            set { customerContactId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the customerCreditCardsId field
        /// </summary>
        public int CustomerCreditCardsId
        {
            get { return customerCreditCardsId; }
            set { customerCreditCardsId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the productsId field
        /// </summary>
        public int ProductsId
        {
            get { return productsId; }
            set { productsId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the productSubscriptionId field
        /// </summary>
        public int ProductSubscriptionId
        {
            get { return productSubscriptionId; }
            set { productSubscriptionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the productSubscriptionName field
        /// </summary>
        public string ProductSubscriptionName
        {
            get { return productSubscriptionName; }
            set { productSubscriptionName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the productSubscriptionDescription field
        /// </summary>
        public string ProductSubscriptionDescription
        {
            get { return productSubscriptionDescription; }
            set { productSubscriptionDescription = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the subscriptionPrice field
        /// </summary>
        public decimal SubscriptionPrice
        {
            get { return subscriptionPrice; }
            set { subscriptionPrice = value; this.IsChanged = true; }
        }
        //taxInclusivePrice
        /// <summary>
        /// Get/Set method of the taxInclusivePrice field
        /// </summary>
        public bool TaxInclusivePrice
        {
            get { return taxInclusivePrice; }
            set { taxInclusivePrice = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the subscriptionCycle field
        /// </summary>
        public int SubscriptionCycle
        {
            get { return subscriptionCycle; }
            set { subscriptionCycle = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the unitOfSubscriptionCycle field
        /// </summary>
        public string UnitOfSubscriptionCycle
        {
            get { return unitOfSubscriptionCycle; }
            set { unitOfSubscriptionCycle = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the subscriptionCycleValidity field
        /// </summary>
        public int SubscriptionCycleValidity
        {
            get { return subscriptionCycleValidity; }
            set { subscriptionCycleValidity = value; this.IsChanged = true; }
        }
        ///// <summary>
        ///// Get/Set method of the seasonalSubscription field
        ///// </summary>
        //public bool SeasonalSubscription
        //{
        //    get { return seasonalSubscription; }
        //    set { seasonalSubscription = value; this.IsChanged = true; }
        //}
        /// <summary>
        /// Get/Set method of the seasonStartDate field
        /// </summary>
        public DateTime? SeasonStartDate
        {
            get { return seasonStartDate; }
            set { seasonStartDate = value; this.IsChanged = true; }
        }
        ///// <summary>
        ///// Get/Set method of the seasonEndDate field
        ///// </summary>
        //public DateTime? SeasonEndDate
        //{
        //    get { return seasonEndDate; }
        //    set { seasonEndDate = value; this.IsChanged = true; }
        //}
        /// <summary>
        /// Get/Set method of the freeTrialPeriodCycle field
        /// </summary>
        public int? FreeTrialPeriodCycle
        {
            get { return freeTrialPeriodCycle; }
            set { freeTrialPeriodCycle = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the billInAdvance field
        /// </summary>
        public bool BillInAdvance
        {
            get { return billInAdvance; }
            set { billInAdvance = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the subscriptionPaymentCollectionMode field
        /// </summary>
        public string SubscriptionPaymentCollectionMode
        {
            get { return subscriptionPaymentCollectionMode; }
            set { subscriptionPaymentCollectionMode = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the selectedPaymentCollectionMode field
        /// </summary>
        public string SelectedPaymentCollectionMode
        {
            get { return selectedPaymentCollectionMode; }
            set { selectedPaymentCollectionMode = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the autoRenew field
        /// </summary>
        public bool AutoRenew
        {
            get { return autoRenew; }
            set { autoRenew = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the autoRenewalMarkupPercent field
        /// </summary>
        public decimal AutoRenewalMarkupPercent
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
        /// Get/Set method of the noOfRenewalReminders field
        /// </summary>
        public int? NoOfRenewalReminders
        {
            get { return noOfRenewalReminders; }
            set { noOfRenewalReminders = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the reminderFrequencyInDays field
        /// </summary>
        public int? ReminderFrequencyInDays
        {
            get { return reminderFrequencyInDays; }
            set { reminderFrequencyInDays = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the sendFirstReminderBeforeXDays field
        /// </summary>
        public int? SendFirstReminderBeforeXDays
        {
            get { return sendFirstReminderBeforeXDays; }
            set { sendFirstReminderBeforeXDays = value; this.IsChanged = true; }
        }
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
        /// Get/Set method of the allowPause field
        /// </summary>
        public bool AllowPause
        {
            get { return allowPause; }
            set { allowPause = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the status field
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; this.IsChanged = true; }
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
        /// Get/Set method of the IsActive field
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
        public string SubscriptionNumber { get { return subscriptionNumber;  }
            set
            {
                this.IsChanged = true;
                subscriptionNumber = value; }
        }
        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleDTOList field
        /// </summary>
        public List<SubscriptionBillingScheduleDTO> SubscriptionBillingScheduleDTOList
        {
            get
            {
                return subscriptionBillingScheduleDTOList;
            }

            set
            {
                subscriptionBillingScheduleDTOList = value;
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
                    return notifyingObjectIsChanged || subscriptionHeaderId < 0;
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
        /// Is Changed Recursive
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (subscriptionBillingScheduleDTOList != null &&
                   subscriptionBillingScheduleDTOList.Any(x => x.IsChanged || x.SubscriptionBillingScheduleId == -1))
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
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Subscription Status
    /// </summary>
    public static class SubscriptionStatus
    {
        // <summary>
        /// ACTIVE status
        /// </summary>
        public const string ACTIVE = "ACTIVE";
        // <summary>
        /// CANCELLED status
        /// </summary>
        public const string CANCELLED = "CANCELLED";
        // <summary>
        /// EXPIRED status
        /// </summary>
        public const string EXPIRED = "EXPIRED";
        // <summary>
        /// PAUSED status
        /// </summary>
        public const string PAUSED = "PAUSED";
        /// <summary>
        /// ValidSubscritionHeaderStatus
        /// </summary>
        /// <param name="statusValue"></param>
        /// <returns></returns>
        public static bool ValidSubscritionHeaderStatus(string statusValue)
        {
            bool validValue = false;
            if (statusValue == ACTIVE || statusValue == CANCELLED || statusValue == EXPIRED || statusValue == PAUSED)
            {
                validValue = true;
            }
            return validValue;
        }
        /// <summary>
        /// ValidSubscritionLineStatus
        /// </summary>
        /// <param name="statusValue"></param>
        /// <returns></returns>
        public static bool ValidSubscritionBillingScheduleStatus(string statusValue)
        {
            bool validValue = false;
            if (statusValue == ACTIVE || statusValue == CANCELLED || statusValue == PAUSED)
            {
                validValue = true;
            }
            return validValue;
        }
    }

    
    /// <summary>
    /// SubscriptionHeaderSummaryDTO
    /// </summary>
    public class SubscriptionHeaderSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int subscriptionHeaderId;
        private int customerId;
        private int productSubscriptionId;
        private string productSubscriptionName;
        private string productSubscriptionDescription; 
        private DateTime? lastBillOnDate;
        private DateTime? nextBillOnDate;
        private DateTime? subscriptionExpiryDate;
        private Image lastPaymentStatus;
        private Image creditCardStatus;
        private List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList;
        public SubscriptionHeaderSummaryDTO()
        {
            log.LogMethodEntry();
            this.subscriptionHeaderId = -1;
            this.customerId = -1;
            this.productSubscriptionId = -1; 
            log.LogMethodExit();
        }

        /// <summary>
        /// SubscriptionHeaderDTO
        /// </summary> 
        public SubscriptionHeaderSummaryDTO(int subscriptionHeaderId, int customerId, int productSubscriptionId, string productSubscriptionName, string productSubscriptionDescription, 
                                     DateTime? lastBillOnDate, DateTime? nextBillOnDate, DateTime? subscriptionExpiryDate, Image lastPaymentStatus, Image creditCardStatus) : this()
        {
            log.LogMethodEntry(subscriptionHeaderId,  customerId, productSubscriptionId, productSubscriptionName, productSubscriptionDescription,
                                     lastBillOnDate, nextBillOnDate, subscriptionExpiryDate, lastPaymentStatus, creditCardStatus);
            this.subscriptionHeaderId = subscriptionHeaderId; 
            this.customerId = customerId; 
            this.productSubscriptionId = productSubscriptionId;
            this.productSubscriptionName = productSubscriptionName;
            this.productSubscriptionDescription = productSubscriptionDescription; 
            this.lastBillOnDate = lastBillOnDate;
            this.nextBillOnDate = nextBillOnDate;
            this.subscriptionExpiryDate = subscriptionExpiryDate;
            this.lastPaymentStatus = lastPaymentStatus;
            this.creditCardStatus = creditCardStatus;
            log.LogMethodExit();
        }
         
        /// <summary>
        /// Get/Set method of the subscriptionHeaderId field
        /// </summary>
        public int SubscriptionHeaderId
        {
            get { return subscriptionHeaderId; }
            set { subscriptionHeaderId = value;  }
        } 
        /// <summary>
        /// Get/Set method of the customerId field
        /// </summary>
        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value;  }
        } 
        /// <summary>
        /// Get/Set method of the productSubscriptionId field
        /// </summary>
        public int ProductSubscriptionId
        {
            get { return productSubscriptionId; }
            set { productSubscriptionId = value;  }
        }
        /// <summary>
        /// Get/Set method of the productSubscriptionName field
        /// </summary>
        public string ProductSubscriptionName
        {
            get { return productSubscriptionName; }
            set { productSubscriptionName = value; }
        }
        /// <summary>
        /// Get/Set method of the productSubscriptionDescription field
        /// </summary>
        public string ProductSubscriptionDescription
        {
            get { return productSubscriptionDescription; }
            set { productSubscriptionDescription = value; }
        }
        /// <summary>
        /// Get/Set method of the lastBillOnDate field
        /// </summary>
        public DateTime? LastBillOnDate
        {
            get { return lastBillOnDate; }
            set { lastBillOnDate = value; }
        }
        /// <summary>
        /// Get/Set method of the nextBillOnDate field
        /// </summary>
        public DateTime? NextBillOnDate
        {
            get { return nextBillOnDate; }
            set { nextBillOnDate = value; }
        }

        /// <summary>
        /// Get/Set method of the subscriptionExpiryDate field
        /// </summary>
        public DateTime? SubscriptionExpiryDate
        {
            get { return subscriptionExpiryDate; }
            set { subscriptionExpiryDate = value; }
        }
        /// <summary>
        /// Get/Set method of the lastPaymentStatus field
        /// </summary>
        public Image LastPaymentStatus
        {
            get { return lastPaymentStatus; }
            set { lastPaymentStatus = value; }
        }

        /// <summary>
        /// Get/Set method of the creditCardStatus field
        /// </summary>
        public Image CreditCardStatus
        {
            get { return creditCardStatus; }
            set { creditCardStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleDTOList field
        /// </summary>
        public List<SubscriptionBillingScheduleDTO> SubscriptionBillingScheduleDTOList
        {
            get
            {
                return subscriptionBillingScheduleDTOList;
            }

            set
            {
                subscriptionBillingScheduleDTOList = value;
            }
        }
          
    }
}

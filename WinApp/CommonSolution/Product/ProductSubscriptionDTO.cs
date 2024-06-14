/********************************************************************************************
* Project Name - ProductSubscriptionDTO
* Description - DTO for ProductSubscription 
*
**************
**Version Log 
**************
*Version    Date        Modified By           Remarks
*********************************************************************************************
*2.110.0    08-Dec-2020    Fiona               Created for subscription feature
*2.120.0    18-Mar-2021    Guru S A            For Subscription phase 2 changes
 *********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductSubscriptionDTO
    /// </summary>
    public class ProductSubscriptionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private int productSubscriptionId;
        private int productsId;
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
        private bool billInAdvance;
        private string paymentCollectionMode;
        private bool autoRenew;
        private decimal autoRenewalMarkupPercent;
        private int? renewalGracePeriodCycle;
        private int? noOfRenewalReminders;
        private int? reminderFrequencyInDays;
        private int? sendFirstReminderBeforeXDays;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool allowPause;
        private string cancellationOption;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  ProductsId field
            /// </summary>
            PRODUCTS_ID,
            /// <summary>
            /// Search by  ProductsIdList field
            /// </summary>
            PRODUCTS_ID_LIST,
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

        public ProductSubscriptionDTO()
        {
            log.LogMethodEntry();
            productSubscriptionId = -1;
            productsId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            subscriptionPrice = 0;
            subscriptionCycle = 0;
            subscriptionCycleValidity = 0;
            autoRenewalMarkupPercent = 0;
            log.LogMethodExit();
        }

        public ProductSubscriptionDTO(int productSubscriptionId, int productsId, string productSubscriptionName, 
            string productSubscriptionDescription, decimal subscriptionPrice, int subscriptionCycle,
            string unitOfSubscriptionCycle, int subscriptionCycleValidity, 
            DateTime? seasonStartDate, int? freeTrialPeriodCycle, 
            bool allowPause, bool billInAdvance, string paymentCollectionMode, bool autoRenew, decimal autoRenewalMarkupPercent, 
            int? renewalGracePeriodCycle, int? noOfRenewalReminders, int? sendFirstReminderBeforeXDays, int? reminderFrequencyInDays, string cancellationOption) :this()
        {
            log.LogMethodEntry(productSubscriptionId, productsId, productSubscriptionName, productSubscriptionDescription, subscriptionPrice, subscriptionCycle, unitOfSubscriptionCycle,
                subscriptionCycleValidity, seasonStartDate, freeTrialPeriodCycle, allowPause, billInAdvance, paymentCollectionMode, autoRenew, 
                autoRenewalMarkupPercent, renewalGracePeriodCycle, noOfRenewalReminders, sendFirstReminderBeforeXDays, reminderFrequencyInDays, cancellationOption);
            this.productSubscriptionId = productSubscriptionId;
            this.productsId = productsId;
            this.productSubscriptionName = productSubscriptionName;
            this.productSubscriptionDescription = productSubscriptionDescription;
            this.subscriptionPrice = subscriptionPrice;
            this.subscriptionCycle = subscriptionCycle;
            this.unitOfSubscriptionCycle = unitOfSubscriptionCycle;
            this.subscriptionCycleValidity = subscriptionCycleValidity;
           // this.seasonalSubscription = seasonalSubscription;
            this.seasonStartDate = seasonStartDate;
            //this.seasonEndDate = seasonEndDate;
            this.freeTrialPeriodCycle = freeTrialPeriodCycle;
            this.billInAdvance = billInAdvance;
            this.paymentCollectionMode = paymentCollectionMode;
            this.autoRenew = autoRenew;
            this.autoRenewalMarkupPercent = autoRenewalMarkupPercent;
            this.renewalGracePeriodCycle = renewalGracePeriodCycle;
            this.noOfRenewalReminders = noOfRenewalReminders;
            this.sendFirstReminderBeforeXDays = sendFirstReminderBeforeXDays;
            this.allowPause = allowPause;
            this.reminderFrequencyInDays = reminderFrequencyInDays;
            this.cancellationOption = cancellationOption;
            log.LogMethodExit();
        }



        public ProductSubscriptionDTO(int productSubscriptionId, int productsId, string productSubscriptionName, string productSubscriptionDescription,
            decimal subscriptionPrice, int subscriptionCycle, string unitOfSubscriptionCycle, int subscriptionCycleValidity,
            DateTime? seasonStartDate, int? freeTrialPeriodCycle, bool allowPause, bool billInAdvance,
            string paymentCollectionMode, bool autoRenew, decimal autoRenewalMarkupPercent, int? renewalGracePeriodCycle, int? noOfRenewalReminders,
            int? sendFirstReminderBeforeXDays, int? reminderFrequencyInDays, string cancellationOption, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
            int siteId, int masterEntityId, bool synchStatus, string guid)
            :  this(productSubscriptionId, productsId,  productSubscriptionName,  productSubscriptionDescription,  
                   subscriptionPrice,  subscriptionCycle,  unitOfSubscriptionCycle,   subscriptionCycleValidity,  
                   seasonStartDate, freeTrialPeriodCycle,  allowPause,  billInAdvance,  
                   paymentCollectionMode,  autoRenew, autoRenewalMarkupPercent,   renewalGracePeriodCycle,   noOfRenewalReminders,   
                   sendFirstReminderBeforeXDays, reminderFrequencyInDays, cancellationOption)
     
        {
            log.LogMethodEntry(productSubscriptionId, productsId, productSubscriptionName, productSubscriptionDescription, subscriptionPrice, subscriptionCycle, unitOfSubscriptionCycle, subscriptionCycleValidity,
                seasonStartDate, freeTrialPeriodCycle, billInAdvance, paymentCollectionMode, autoRenew, autoRenewalMarkupPercent,
                renewalGracePeriodCycle, noOfRenewalReminders, sendFirstReminderBeforeXDays, reminderFrequencyInDays, cancellationOption, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, 
                masterEntityId, synchStatus,  guid);
              
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
        /// Get/Set method of the ProductSubscriptionId field
        /// </summary>
        public int ProductSubscriptionId
        {
            get { return productSubscriptionId; }
            set { productSubscriptionId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ProductsId field
        /// </summary>
        public int ProductsId
        {
            get { return productsId; }
            set { productsId = value; }
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
        /// Get/Set method of the freeTrialPeriodCycle field
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
        /// Get/Set method of the PaymentCollectionMode field
        /// </summary>

        public string PaymentCollectionMode
        {
            get { return paymentCollectionMode; }
            set { paymentCollectionMode = value; this.IsChanged = true; }
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
        public decimal AutoRenewalMarkupPercent
        {
            get { return autoRenewalMarkupPercent; }
            set { autoRenewalMarkupPercent = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the renewalGracePeriodCycle field
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
        /// Get/Set method of the cancellationOption field
        /// </summary>
        public string CancellationOption
        {
            get { return cancellationOption; }
            set { cancellationOption = value; this.IsChanged = true; }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || productSubscriptionId < 0;
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
    /// <summary>
    /// Unit Of Subscription Cycle - Values
    /// </summary>
    public static class UnitOfSubscriptionCycle
    {
        /// <summary>
        /// DAYS - D
        /// </summary>
        public const string DAYS = "D";
        /// <summary>
        /// MONTHS - M
        /// </summary>
        public const string MONTHS = "M";
        /// <summary>
        /// YEARS - Y
        /// </summary>
        public const string YEARS = "Y";
        /// <summary>
        /// ValidUnitOfSubscriptionCycle
        /// </summary>
        /// <param name="uoscValue"></param>
        /// <returns></returns>
        public static bool ValidUnitOfSubscriptionCycle(string uoscValue)
        {
            bool validValue = false;
            if (uoscValue == DAYS || uoscValue == MONTHS || uoscValue == YEARS)
            {
                validValue = true;
            }
            return validValue;
        }
        /// <summary>
        /// GetUnitOfSubscriptionCycleDescription
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="uomValue"></param>
        /// <returns></returns>
        public static string GetUnitOfSubscriptionCycleDescription(ExecutionContext executionContext,  string uomValue)
        {
            string description = (uomValue == "D" ? MessageContainerList.GetMessage(executionContext, "Days") 
                                                  : uomValue == "M" ? MessageContainerList.GetMessage(executionContext, "Months")
                                                  : uomValue == "Y" ? MessageContainerList.GetMessage(executionContext, "Years") : "");
            return description;
        }
    }
    /// <summary>
    /// Payment Collection Mode - Values
    /// </summary>
    public static class SubscriptionPaymentCollectionMode
    {
        /// <summary>
        /// SUBSCRIPTION_CYCLE - S
        /// </summary>
        public const string SUBSCRIPTION_CYCLE = "S";
        /// <summary>
        /// FULL - F
        /// </summary>
        public const string FULL = "F";
        /// <summary>
        /// CUSTOMER_CHOICE - C
        /// </summary>
        public const string CUSTOMER_CHOICE = "C";
        /// <summary>
        /// Valid Payment Collection Mode
        /// </summary>
        /// <param name="pcmValue"></param>
        /// <returns></returns>
        public static bool ValidPaymentCollectionMode(string pcmValue)
        {
            bool validValue = false;
            if (pcmValue == SUBSCRIPTION_CYCLE || pcmValue == FULL || pcmValue == CUSTOMER_CHOICE)
            {
                validValue = true;
            }
            return validValue;
        }
        /// <summary>
        /// Valid Selected Payment Collection Mode
        /// </summary>
        /// <param name="pcmValue"></param>
        /// <returns></returns>
        public static bool ValidSelectedPaymentCollectionMode(string pcmValue)
        {
            bool validValue = false;
            if (pcmValue == SUBSCRIPTION_CYCLE || pcmValue == FULL)
            {
                validValue = true;
            }
            return validValue;
        }
        /// <summary>
        /// GetPaymentDescription
        /// </summary>
        /// <param name="pcmValue"></param>
        /// <returns></returns>
        public static string GetPaymentDescription(string pcmValue)
        {
            string description = string.Empty;
            switch (pcmValue)
            {
                case SUBSCRIPTION_CYCLE:
                    description = "Subscription Cycle";
                    break;
                case FULL:
                    description = "Full";
                    break;
                case CUSTOMER_CHOICE:
                    description = "Customer Choice";
                    break;

            }
            return description;
        }
    }

    /// <summary>
    ///Subscription Cancellation Option - Values
    /// </summary>
    public static class SubscriptionCancellationOption
    {
        /// <summary>
        /// CANCELL_UNBILLED_CYCLES - U
        /// </summary>
        public const string CANCELL_UNBILLED_CYCLES = "U";
        /// <summary>
        /// CANCEL_AUTO_RENEWAL_ONLY - A
        /// </summary>
        public const string CANCEL_AUTO_RENEWAL_ONLY = "A"; 
        /// <summary>
        /// Valid Payment Collection Mode
        /// </summary>
        /// <param name="pcmValue"></param>
        /// <returns></returns>
        public static bool ValidCancellationOption(string pcmValue)
        {
            bool validValue = false;
            if (pcmValue == CANCELL_UNBILLED_CYCLES || pcmValue == CANCEL_AUTO_RENEWAL_ONLY)
            {
                validValue = true;
            }
            return validValue;
        } 
        /// <summary>
        /// GetPaymentDescription
        /// </summary>
        /// <param name="pcmValue"></param>
        /// <returns></returns>
        public static string GetCancellationOptionDescription(string pcmValue)
        {
            string description = string.Empty;
            switch (pcmValue)
            {
                case CANCELL_UNBILLED_CYCLES:
                    description = "Cancel Unbilled Cycles";
                    break; 
                case CANCEL_AUTO_RENEWAL_ONLY:
                    description = "Cancel Auto Renewal Option Only";
                    break;

            }
            return description;
        }
    }
}

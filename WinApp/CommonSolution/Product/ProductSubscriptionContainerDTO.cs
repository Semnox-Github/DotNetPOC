/********************************************************************************************
* Project Name - ProductSubscriptionContainerDTO
* Description - DTO for ProductSubscription container 
*
**************
**Version Log 
**************
*Version    Date        Modified By           Remarks
********************************************************************************************* 
*2.120.0    18-Mar-2021    Guru S A            For Subscription phase 2 changes
 *********************************************************************************************/ 
using System; 

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductSubscriptionContainerDTO
    /// </summary>
    public class ProductSubscriptionContainerDTO
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
        private bool allowPause;
        private string cancellationOption; 


        public ProductSubscriptionContainerDTO()
        {
            log.LogMethodEntry();
            productSubscriptionId = -1;
            productsId = -1; 
            subscriptionPrice = 0;
            subscriptionCycle = 0;
            subscriptionCycleValidity = 0;
            autoRenewalMarkupPercent = 0;
            log.LogMethodExit();
        }

        public ProductSubscriptionContainerDTO(int productSubscriptionId, int productsId, string productSubscriptionName,
            string productSubscriptionDescription, decimal subscriptionPrice, int subscriptionCycle,
            string unitOfSubscriptionCycle, int subscriptionCycleValidity,
            DateTime? seasonStartDate, int? freeTrialPeriodCycle,
            bool allowPause, bool billInAdvance, string paymentCollectionMode, bool autoRenew, decimal autoRenewalMarkupPercent,
            int? renewalGracePeriodCycle, int? noOfRenewalReminders, int? sendFirstReminderBeforeXDays, int? reminderFrequencyInDays, string cancellationOption) : this()
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
            this.seasonStartDate = seasonStartDate; 
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

        public ProductSubscriptionContainerDTO(ProductSubscriptionContainerDTO productSubscriptionContainerDTO) 
            : this(productSubscriptionContainerDTO.productSubscriptionId, productSubscriptionContainerDTO.productsId, productSubscriptionContainerDTO.productSubscriptionName,
            productSubscriptionContainerDTO.productSubscriptionDescription, productSubscriptionContainerDTO.subscriptionPrice, productSubscriptionContainerDTO.subscriptionCycle,
            productSubscriptionContainerDTO.unitOfSubscriptionCycle, productSubscriptionContainerDTO.subscriptionCycleValidity,
            productSubscriptionContainerDTO.seasonStartDate, productSubscriptionContainerDTO.freeTrialPeriodCycle,
            productSubscriptionContainerDTO.allowPause, productSubscriptionContainerDTO.billInAdvance, productSubscriptionContainerDTO.paymentCollectionMode, productSubscriptionContainerDTO.autoRenew, productSubscriptionContainerDTO.autoRenewalMarkupPercent,
            productSubscriptionContainerDTO.renewalGracePeriodCycle, productSubscriptionContainerDTO.noOfRenewalReminders, productSubscriptionContainerDTO.sendFirstReminderBeforeXDays, productSubscriptionContainerDTO.reminderFrequencyInDays, productSubscriptionContainerDTO.cancellationOption)
        {
            log.LogMethodEntry(productSubscriptionContainerDTO);
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the ProductSubscriptionId field
        /// </summary>
        public int ProductSubscriptionId
        {
            get { return productSubscriptionId; }
            set { productSubscriptionId = value; }
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
            set { productSubscriptionName = value;  }
        }
        /// <summary>
        /// Get/Set method of the ProductSubscriptionDescription field
        /// </summary>
        public string ProductSubscriptionDescription
        {
            get { return productSubscriptionDescription; }
            set { productSubscriptionDescription = value;  }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionPrice field
        /// </summary>
        public decimal SubscriptionPrice
        {
            get { return subscriptionPrice; }
            set { subscriptionPrice = value;  }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionCycle field
        /// </summary>
        public int SubscriptionCycle
        {
            get { return subscriptionCycle; }
            set { subscriptionCycle = value;  }
        }
        /// <summary>
        /// Get/Set method of the ProductsId field
        /// </summary>
        public string UnitOfSubscriptionCycle
        {
            get { return unitOfSubscriptionCycle; }
            set { unitOfSubscriptionCycle = value;  }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionCycleValidity field
        /// </summary>
        public int SubscriptionCycleValidity
        {
            get { return subscriptionCycleValidity; }
            set { subscriptionCycleValidity = value;  }
        }
        public DateTime? SeasonStartDate
        {
            get { return seasonStartDate; }
            set { seasonStartDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the freeTrialPeriodCycle field
        /// </summary>
        public int? FreeTrialPeriodCycle
        {
            get { return freeTrialPeriodCycle; }
            set { freeTrialPeriodCycle = value;  }
        }
        /// <summary>
        /// Get/Set method of the AllowPause field
        /// </summary>
        public bool AllowPause
        {
            get { return allowPause; }
            set { allowPause = value;  }
        }
        /// <summary>
        /// Get/Set method of the BillInAdvance field
        /// </summary>
        public bool BillInAdvance
        {
            get { return billInAdvance; }
            set { billInAdvance = value;  }
        }
        /// <summary>
        /// Get/Set method of the PaymentCollectionMode field
        /// </summary>
        public string PaymentCollectionMode
        {
            get { return paymentCollectionMode; }
            set { paymentCollectionMode = value;  }
        }
        /// <summary>
        /// Get/Set method of the AutoRenew field
        /// </summary>
        public bool AutoRenew
        {
            get { return autoRenew; }
            set { autoRenew = value;  }
        }
        /// <summary>
        /// Get/Set method of the AutoRenewalMarkupPercent field
        /// </summary>
        public decimal AutoRenewalMarkupPercent
        {
            get { return autoRenewalMarkupPercent; }
            set { autoRenewalMarkupPercent = value;  }
        }
        /// <summary>
        /// Get/Set method of the renewalGracePeriodCycle field
        /// </summary>
        public int? RenewalGracePeriodCycle
        {
            get { return renewalGracePeriodCycle; }
            set { renewalGracePeriodCycle = value;  }
        }
        /// <summary>
        /// Get/Set method of the NoOfRenewalReminders field
        /// </summary>
        public int? NoOfRenewalReminders
        {
            get { return noOfRenewalReminders; }
            set { noOfRenewalReminders = value;  }
        }
        /// <summary>
        /// Get/Set method of the SendFirstReminderBeforeXDays field
        /// </summary>
        public int? SendFirstReminderBeforeXDays
        {
            get { return sendFirstReminderBeforeXDays; }
            set { sendFirstReminderBeforeXDays = value;  }
        }
        /// <summary>
        /// Get/Set method of the ReminderFrequencyInDays field
        /// </summary>
        public int? ReminderFrequencyInDays
        {
            get { return reminderFrequencyInDays; }
            set { reminderFrequencyInDays = value;  }
        }
        /// <summary>
        /// Get/Set method of the cancellationOption field
        /// </summary>
        public string CancellationOption
        {
            get { return cancellationOption; }
            set { cancellationOption = value;  }
        } 
    }
}

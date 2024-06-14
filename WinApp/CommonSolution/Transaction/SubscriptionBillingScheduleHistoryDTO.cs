/********************************************************************************************
* Project Name - SubscriptionBillingScheduleHistoryDTO
* Description - DTO for SubscriptionBillingScheduleHistory 
*
**************
**Version Log 
**************
*Version    Date          Modified By          Remarks
*********************************************************************************************
*2.110.0    14-Dec-2020     Fiona              Created for subscription feature
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
    /// SubscriptionBillingScheduleHistoryDTO Class
    /// </summary>
    public class SubscriptionBillingScheduleHistoryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int subscriptionBillingScheduleHistoryId;
        private int subscriptionBillingScheduleId;
        private int subscriptionHeaderId;
        private int transactionId;
        private int transactionLineId;
        private DateTime? billFromDate;
        private DateTime? billToDate;
        private DateTime? billOnDate;
        private decimal? billAmount;
        private decimal? overridedBillAmount;
        private string overrideReason;
        private string overrideBy;
        private string overrideApprovedBy;
        private int? paymentProcessingFailureCount;
        private string status;
        private string cancelledBy;
        private string cancellationApprovedBy;
        private string lineType;
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

        /// <summary>
        /// Search Parameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  subscriptionBillingScheduleHistoryId field
            /// </summary>
            SUBSCRIPTION_BILLING_SCHEDULE_HISTORY_ID,
            /// <summary>
            /// Search by  subscriptionBillingScheduleId field
            /// </summary>
            SUBSCRIPTION_BILLING_SCHEDULE_ID,
            /// <summary>
            /// Search by  subscriptionHeaderId field
            /// </summary>
            SUBSCRIPTION_HEADER_ID,  
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
        public SubscriptionBillingScheduleHistoryDTO()
        {
            log.LogMethodEntry();
            this.subscriptionBillingScheduleHistoryId = -1;
            this.subscriptionBillingScheduleId = -1;
            this.subscriptionHeaderId = -1;
            this.transactionId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with Required fields
        /// </summary>
        public SubscriptionBillingScheduleHistoryDTO(int subscriptionBillingScheduleHistoryId, int subscriptionBillingScheduleId, int subscriptionHeaderId, 
            int transactionId, int transactionLineId, DateTime billFromDate, DateTime billToDate, DateTime billOnDate, decimal? billAmount, 
            decimal? overridedBillAmount, string overrideReason, string overrideBy, string overrideApprovedBy, int? paymentProcessingFailureCount, string status,
            string cancelledBy, string cancellationApprovedBy, string lineType) :this()
        {
            log.LogMethodEntry( subscriptionBillingScheduleHistoryId,  subscriptionBillingScheduleId,  subscriptionHeaderId,
             transactionId,  transactionLineId,  billFromDate,  billToDate,  billOnDate,   billAmount, overridedBillAmount,  overrideReason,  overrideBy, 
             overrideApprovedBy,   paymentProcessingFailureCount,  status, cancelledBy, cancellationApprovedBy, lineType);
            this.subscriptionBillingScheduleHistoryId = subscriptionBillingScheduleHistoryId;
            this.subscriptionBillingScheduleId = subscriptionBillingScheduleId;
            this.subscriptionHeaderId = subscriptionHeaderId;
            this.transactionId = transactionId;
            this.transactionLineId = transactionLineId;
            this.billFromDate = billFromDate;
            this.billToDate = billToDate;
            this.billOnDate = billOnDate;
            this.billAmount = billAmount;
            this.overridedBillAmount = overridedBillAmount;
            this.overrideReason = overrideReason;
            this.overrideBy = overrideBy;
            this.overrideApprovedBy = overrideApprovedBy;
            this.paymentProcessingFailureCount = paymentProcessingFailureCount;
            this.status = status;
            this.cancelledBy = cancelledBy;
            this.cancellationApprovedBy = cancellationApprovedBy;
            this.lineType = lineType;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with All fields
        /// </summary>
        public SubscriptionBillingScheduleHistoryDTO(int subscriptionBillingScheduleHistoryId, int subscriptionBillingScheduleId, 
            int subscriptionHeaderId, int transactionId, int transactionLineId, DateTime billFromDate, DateTime billToDate, DateTime billOnDate, 
            decimal? billAmount, decimal? overridedBillAmount, string overrideReason, string overrideBy, string overrideApprovedBy, 
            int? paymentProcessingFailureCount, string status, string cancelledBy, string cancellationApprovedBy, string lineType, 
            bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, 
            DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid) : 
            this(subscriptionBillingScheduleHistoryId, subscriptionBillingScheduleId, subscriptionHeaderId, transactionId, transactionLineId,
                billFromDate, billToDate, billOnDate, billAmount, overridedBillAmount, overrideReason, overrideBy, overrideApprovedBy, 
                paymentProcessingFailureCount, status, cancelledBy, cancellationApprovedBy, lineType)
        {
            log.LogMethodEntry( subscriptionBillingScheduleHistoryId,  subscriptionBillingScheduleId, subscriptionHeaderId,  transactionId,  transactionLineId,  billFromDate,  billToDate, 
                billOnDate, billAmount,   overridedBillAmount,  overrideReason,  overrideBy,  overrideApprovedBy, paymentProcessingFailureCount,  status,
                cancelledBy, cancellationApprovedBy, lineType,
              isActive, createdBy,  creationDate,  lastUpdatedBy,lastUpdateDate,  siteId,  masterEntityId,  synchStatus,  guid);
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
        /// Get/Set method of the SubscriptionBillingScheduleHistoryId field
        /// </summary>
        public int SubscriptionBillingScheduleHistoryId
        {
            get { return subscriptionBillingScheduleHistoryId; }
            set { subscriptionBillingScheduleHistoryId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubscriptionBillingScheduleId field
        /// </summary>
        public int SubscriptionBillingScheduleId
        {
            get { return subscriptionBillingScheduleId; }
            set { subscriptionBillingScheduleId = value; this.IsChanged = true; }
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
        /// Get/Set method of the BillFromDate field
        /// </summary>
        public DateTime? BillFromDate
        {
            get { return billFromDate; }
            set { billFromDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the BillToDate field
        /// </summary>
        public DateTime? BillToDate
        {
            get { return billToDate; }
            set { billToDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the BillOnDate field
        /// </summary>
        public DateTime? BillOnDate
        {
            get { return billOnDate; }
            set { billOnDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the BillAmount field
        /// </summary>
        public decimal? BillAmount
        {
            get { return billAmount; }
            set { billAmount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the OverridedBillAmount field
        /// </summary>
        public decimal? OverridedBillAmount
        {
            get { return overridedBillAmount; }
            set { overridedBillAmount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the OverrideReason field
        /// </summary>
        public string OverrideReason
        {
            get { return overrideReason; }
            set { overrideReason = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the OverrideBy field
        /// </summary>
        public string OverrideBy
        {
            get { return overrideBy; }
            set { overrideBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the OverrideApprovedBy field
        /// </summary>
        public string OverrideApprovedBy
        {
            get { return overrideApprovedBy; }
            set { overrideApprovedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PaymentProcessingFailureCount field
        /// </summary>
        public int? PaymentProcessingFailureCount
        {
            get { return paymentProcessingFailureCount; }
            set { paymentProcessingFailureCount = value; this.IsChanged = true; }
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
        /// Get/Set method of the IsActive field
        /// </summary>
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
        /// Get/Set method of the lineType field
        /// </summary>
        public string LineType
        {
            get { return lineType; }
            set { lineType = value; this.IsChanged = true; }
        }
        /// <summary>
        /// IsActive
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
                    return notifyingObjectIsChanged || subscriptionBillingScheduleHistoryId < 0;
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

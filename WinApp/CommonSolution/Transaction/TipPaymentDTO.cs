/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of TipPayment
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      22-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the TipPayment data object class. This acts as data holder for the TipPayment business object
    /// </summary>
    public class TipPaymentDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  TIP ID field
            /// </summary>
            TIP_ID,
            /// <summary>
            /// Search by  TIP ID LIST field
            /// </summary>
            TIP_ID_LIST,
            /// <summary>
            /// Search by  PAYMENT ID field
            /// </summary>
            PAYMENT_ID,
            /// <summary>
            /// Search by  USER ID  field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
            private int tipId ;
            private int paymentId ;
            private int userId ;
            private int splitByPercentage ;
            private string guid;
            private int siteId;
            private bool synchStatus;
            private string createdBy;
            private DateTime creationDate;
            private int masterEntityId;
            private string lastUpdatedBy;
            private DateTime lastUpdatedDate;
            private bool notifyingObjectIsChanged;
            private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public TipPaymentDTO()
        {
            log.LogMethodEntry();
            tipId = -1;
            paymentId = -1;
            siteId = -1;
            masterEntityId = -1;
            userId = -1;
            createdBy = string.Empty;
            creationDate =DateTime.MinValue;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with Required fields.
        /// </summary>
        public TipPaymentDTO(int tipId, int paymentId, int userId, int splitByPercentage)
            :this()
        {
            log.LogMethodEntry(tipId, paymentId, userId, splitByPercentage);
            this.tipId = tipId;
            this.paymentId = paymentId;
            this.splitByPercentage = splitByPercentage;
            this.userId = userId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the fields.
        /// </summary>
        public TipPaymentDTO(int tipId, int paymentId, int userId, int splitByPercentage, string guid ,
                             int siteId ,  bool synchStatus , string createdBy , DateTime creationDate,
                             int masterEntityId , string lastUpdatedBy, DateTime lastUpdatedDate)
            :this(tipId, paymentId, userId, splitByPercentage)
        {
            log.LogMethodEntry(tipId, paymentId,  userId,  splitByPercentage,  guid, siteId,  synchStatus,  
                               createdBy,  creationDate,  masterEntityId,  lastUpdatedBy,  lastUpdatedDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TipId field
        /// </summary>
        public int TipId
        {
            get { return tipId; }
            set { tipId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PaymentId field
        /// </summary>
        public int PaymentId
        {
            get { return paymentId; }
            set { paymentId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SplitByPercentage field
        /// </summary>
        public int SplitByPercentage
        {
            get { return splitByPercentage; }
            set { splitByPercentage = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;  }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;  }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
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
                    return notifyingObjectIsChanged || tipId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
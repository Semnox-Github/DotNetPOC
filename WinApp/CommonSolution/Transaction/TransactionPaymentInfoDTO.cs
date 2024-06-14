/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of TrxPaymentsInfo
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
    /// This is the TrxPaymentsInfo data object class. This acts as data holder for the TrxPaymentsInfo business object
    /// </summary>
    public class TransactionPaymentInfoDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by   ID LIST  field
            /// </summary>
            ID_LIST,
            /// <summary>
            /// Search by  PAYMENT ID field
            /// </summary>
            PAYMENT_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int id;
        private int paymentId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionPaymentInfoDTO()
        {
            log.LogMethodEntry();
            id = -1;
            paymentId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields. 
        /// </summary>
        public TransactionPaymentInfoDTO(int id, int paymentId)
            :this()
        {
            log.LogMethodEntry(id, paymentId);
            this.id = id;
            this.paymentId = paymentId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields. 
        /// </summary>
        public TransactionPaymentInfoDTO(int id, int paymentId,string guid, bool synchStatus, int masterEntityId, int siteId,
                                  string createdBy, DateTime creationDate, string lastUpdatedBy,DateTime lastUpdatedDate )
            :this(id, paymentId)
        {
            log.LogMethodEntry(id, paymentId, guid, synchStatus, masterEntityId, siteId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate);
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
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
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
            set { synchStatus = value;  }
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
                    return notifyingObjectIsChanged || id < 0;
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

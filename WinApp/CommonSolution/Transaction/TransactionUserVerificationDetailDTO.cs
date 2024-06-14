/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of TrxUserVerificationDetail
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
    /// This is the TrxUserVerificationDetail data object class. This acts as data holder for the TrxUserVerificationDetail business object
    /// </summary>
    public class TransactionUserVerificationDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   TRX USERR VERFICATION DETAIL ID field
            /// </summary>
            TRX_USR_VERFN_DETAIL_ID,
            /// <summary>
            /// Search by   TRX USERR VERFICATION DETAIL ID LIST field
            /// </summary>
            TRX_USR_VERFN_DETAIL_ID_LIST,
            /// <summary>
            /// Search by   TRANSACTION ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// <summary>
            /// Search by  VERIFICATION ID field
            /// </summary>
            VERIFICATION_ID,
             /// <summary>
            /// Search by  LINE ID field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by  ACTIVE FLAG field
            /// <summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int trxUserVerificationDetId;
        private int trxId;
        private int lineId;
        private string verificationId;
        private string verificationName;
        private string verificationRemarks;
        private string activeFlag; 
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransactionUserVerificationDetailDTO()
        {
            log.LogMethodEntry();
            trxUserVerificationDetId = -1;
            trxId = -1;
            lineId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = "Y";
            log.LogMethodExit();
        }

        /// <summary>
        ///   Parameterized constructor with Required fields
        /// </summary>
        public TransactionUserVerificationDetailDTO(int trxUserVerificationDetId, int trxId, int lineId, string verificationId,
                                            string verificationName, string verificationRemarks, string activeFlag)
            :this()
        {
            log.LogMethodEntry(trxUserVerificationDetId, trxId, lineId, verificationId, verificationName, verificationRemarks,activeFlag);

            this.trxUserVerificationDetId = trxUserVerificationDetId;
            this.trxId = trxId;
            this.lineId = lineId;
            this.verificationId = verificationId;
            this.verificationName = verificationName;
            this.verificationRemarks = verificationRemarks;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Parameterized constructor with all the fields
        /// </summary>
        public TransactionUserVerificationDetailDTO(int trxUserVerificationDetId,int trxId, int lineId,string verificationId,
                                            string verificationName, string verificationRemarks,string activeFlag,
                                            int siteId, string guid, bool synchStatus,int masterEntityId,
                                            DateTime creationDate, string createdBy, DateTime lastUpdatedDate,
                                            string lastUpdatedBy)
            :this(trxUserVerificationDetId, trxId, lineId, verificationId, verificationName, verificationRemarks, activeFlag)
        {
            log.LogMethodEntry(trxUserVerificationDetId, trxId, lineId, verificationId,verificationName, verificationRemarks,
                               activeFlag, siteId, guid,synchStatus, masterEntityId, creationDate, createdBy, lastUpdatedDate, lastUpdatedBy);

            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the TrxUserVerificationDetId field
        /// </summary>
        public int TrxUserVerificationDetId
        {
            get { return trxUserVerificationDetId; }
            set { trxUserVerificationDetId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        public int LineId
        {
            get { return lineId; }
            set { lineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VerificationId field
        /// </summary>
        public string VerificationId
        {
            get { return verificationId; }
            set { verificationId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VerificationName field
        /// </summary>
        public string VerificationName
        {
            get { return verificationName; }
            set { verificationName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VerificationRemarks field
        /// </summary>
        public string VerificationRemarks
        {
            get { return verificationRemarks; }
            set { verificationRemarks = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public string ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || trxUserVerificationDetId < 0 ;
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

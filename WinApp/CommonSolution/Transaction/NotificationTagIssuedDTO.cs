/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Object of the NotificationTagIssued class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    public class NotificationTagIssuedDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by NOTIFICATIONTAGISSUEDID
            /// </summary>
            NOTIFICATIONTAGISSUEDID,
            /// <summary>
            /// Search by CARDID
            /// </summary>
            CARDID,
            /// <summary>
            ///  Search by TRXID
            /// </summary>
            TRANSACTIONID,
            /// <summary>
            /// Search by LINEID
            /// </summary>
            LINEID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by ISRETURNED field
            /// </summary>
            ISRETURNED,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Guid field
            /// </summary>
            GUID,
            /// <summary>
            /// Search by ISSUEDATE field
            /// </summary>
            ISSUEDATE,
            /// <summary>
            /// Search by STARTDATE field
            /// </summary>
            STARTDATE,
            /// <summary>
            /// Search by EXPIRYDATE field
            /// </summary>
            EXPIRYDATE,
            /// <summary>
            /// Search by Current Start Date field
            /// </summary>
            CURRENTSTARTDATE,
            /// <summary>
            /// Search by Last Update after passed value
            /// </summary>
            LASTUPDATEDATE_AFTER,
            /// <summary>
            /// Search by Last Update or Start date after passed value
            /// </summary>
            LASTUPDATE_STARTDATE_AFTER,
            /// <summary>
            /// Search by Expiry date is null or after passed value
            /// </summary>
            EXPIRY_DATE_NULL_AFTER
        }

        private int notificationTagIssuedId;
        private int cardId;
        private DateTime issueDate;
        private DateTime startDate;
        private DateTime expiryDate;
        private int trxId;
        private int lineId;
        private bool isReturned;
        private DateTime returnDate;
        private int notificationTagProfileId;
        private DateTime lastSessionAlertTime;
        private DateTime lastAlertTimeBeforeExpiry;
        private DateTime lastAlertTimeOnExpiry;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NotificationTagIssuedDTO()
        {
            log.LogMethodEntry();
            notificationTagIssuedId = -1;
            cardId = -1;
            trxId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        /// <param name="notificationTagIssuedId"></param>
        /// <param name="cardId"></param>
        /// <param name="issueDate"></param>
        /// <param name="startDate"></param>
        /// <param name="expiryDate"></param>
        /// <param name="trxId"></param>
        /// <param name="lineId"></param>
        /// <param name="isReturned"></param>
        /// <param name="returnDate"></param>
        /// <param name="notificationTagProfileId"></param>
        /// <param name="lastSessionAlertTime"></param>
        /// <param name="lastAlertTimeBeforeExpiry"></param>
        /// <param name="lastAlertTimeOnExpiry"></param>
        /// <param name="isActive"></param>
        public NotificationTagIssuedDTO(int notificationTagIssuedId, int cardId, DateTime issueDate, DateTime startDate, DateTime expiryDate, int trxId,
                                        int lineId, bool isReturned, DateTime returnDate, int notificationTagProfileId, DateTime lastSessionAlertTime,
                                        DateTime lastAlertTimeBeforeExpiry, DateTime lastAlertTimeOnExpiry, bool isActive) : this()
        {
            log.LogMethodEntry(notificationTagIssuedId, cardId, issueDate, startDate, expiryDate, trxId,  lineId,  isReturned,  returnDate,  notificationTagProfileId,  lastSessionAlertTime,
                                         lastAlertTimeBeforeExpiry,  lastAlertTimeOnExpiry,  isActive);
            this.notificationTagIssuedId = notificationTagIssuedId;
            this.cardId = cardId;
            this.issueDate = issueDate;
            this.startDate = startDate;
            this.expiryDate = expiryDate;
            this.trxId = trxId;
            this.lineId = lineId;
            this.isReturned = isReturned;
            this.returnDate = returnDate;
            this.notificationTagProfileId = notificationTagProfileId;
            this.lastSessionAlertTime = lastSessionAlertTime;
            this.lastAlertTimeBeforeExpiry = lastAlertTimeBeforeExpiry;
            this.lastAlertTimeOnExpiry = lastAlertTimeOnExpiry;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        /// <param name="notificationTagIssuedId"></param>
        /// <param name="cardId"></param>
        /// <param name="issueDate"></param>
        /// <param name="startDate"></param>
        /// <param name="expiryDate"></param>
        /// <param name="trxId"></param>
        /// <param name="lineId"></param>
        /// <param name="isReturned"></param>
        /// <param name="returnDate"></param>
        /// <param name="notificationTagProfileId"></param>
        /// <param name="lastSessionAlertTime"></param>
        /// <param name="lastAlertTimeBeforeExpiry"></param>
        /// <param name="lastAlertTimeOnExpiry"></param>
        /// <param name="isActive"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="siteId"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="synchStatus"></param>
        /// <param name="guid"></param>
        public NotificationTagIssuedDTO(int notificationTagIssuedId, int cardId, DateTime issueDate, DateTime startDate, DateTime expiryDate, int trxId,
                                        int lineId, bool isReturned, DateTime returnDate, int notificationTagProfileId, DateTime lastSessionAlertTime,
                                        DateTime lastAlertTimeBeforeExpiry, DateTime lastAlertTimeOnExpiry, bool isActive, string createdBy,
                                       DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                       int siteId, int masterEntityId, bool synchStatus, string guid) 
            : this(notificationTagIssuedId, cardId, issueDate, startDate, expiryDate, trxId, lineId, isReturned, returnDate, notificationTagProfileId, lastSessionAlertTime,
                                         lastAlertTimeBeforeExpiry, lastAlertTimeOnExpiry, isActive)
        {
            log.LogMethodEntry(notificationTagIssuedId, cardId, issueDate, startDate, expiryDate, trxId, lineId, isReturned, returnDate, notificationTagProfileId, lastSessionAlertTime,
                                         lastAlertTimeBeforeExpiry, lastAlertTimeOnExpiry, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId,
                                         masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
        }


        /// <summary>
        /// Get/Set method of the LastAlertTimeBeforeExpiry field
        /// </summary>
        public DateTime LastAlertTimeBeforeExpiry { get { return lastAlertTimeBeforeExpiry; } set { lastAlertTimeBeforeExpiry = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastAlertTimeOnExpiry field
        /// </summary>
        public DateTime LastAlertTimeOnExpiry { get { return lastAlertTimeOnExpiry; } set { lastAlertTimeOnExpiry = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsReturned field
        /// </summary>
        public bool IsReturned { get { return isReturned; } set { isReturned = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the IssueDate field
        /// </summary>
        public DateTime IssueDate { get { return issueDate; } set { this.IsChanged = true; issueDate = value; } }

        /// <summary>
        /// Get method of the StartDate field
        /// </summary>
        public DateTime StartDate { get { return startDate; } set { this.IsChanged = true; startDate = value; } }

        /// <summary>
        /// Get method of the ExpiryDate field
        /// </summary>
        public DateTime ExpiryDate { get { return expiryDate; } set { this.IsChanged = true; expiryDate = value; } }

        /// <summary>
        /// Get method of the ReturnDate field
        /// </summary>
        public DateTime ReturnDate { get { return returnDate; } set { this.IsChanged = true; returnDate = value; } }

        /// <summary>
        /// Get method of the LastSessionAlertTime field
        /// </summary>
        public DateTime LastSessionAlertTime { get { return lastSessionAlertTime; } set { this.IsChanged = true; lastSessionAlertTime = value; } }
        /// <summary>
        /// Get/Set method of the NotificationTagIssuedId field
        /// </summary>
        public int NotificationTagIssuedId { get { return notificationTagIssuedId; } set { this.IsChanged = true; notificationTagIssuedId = value; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId { get { return cardId; } set { this.IsChanged = true; cardId = value; } }

        /// <summary>
        /// Get/Set method of the trxId field
        /// </summary>
        public int TransactionId { get { return trxId; } set { this.IsChanged = true; trxId = value; } }

        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        public int LineId { get { return lineId; } set { this.IsChanged = true; lineId = value; } }

        /// <summary>
        /// Get/Set method of the NotificationTagProfileId field
        /// </summary>
        public int NotificationTagProfileId { get { return notificationTagProfileId; } set { this.IsChanged = true; notificationTagProfileId = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || notificationTagIssuedId < 0;
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

/********************************************************************************************
 * Project Name - Communication
 * Description  - Data object of MessagingRequest
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      24-May-2019   Girish Kundar           Created 
 *2.100     06-Jul-2020   Jinto Thomas            Added cc,bcc,messagingClientName part of email enhancement
 *2.100.0   15-Sep-2020   Nitin Pai               Push Notification: Added ToDevice (token), MessageType, MessageRead
 *2.150.0   26-Nov-2021   Deeksha                 Modified to Add country Code field.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// This is the MessagingRequest data object class. This acts as data holder for the MessagingRequest business object
    /// </summary>
    public class MessagingRequestDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   MessagingRequest R ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by    BATCH ID field
            /// </summary>
            BATCH_ID,
            /// <summary>
            /// Search by REFERENCE  field
            /// </summary>
            REFERENCE,
            /// <summary>
            /// Search by STATUS ID field
            /// </summary>
            STATUS,
            /// <summary>
            /// Search by CUSTOMER ID field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  SEND DATE field
            /// </summary>
            SEND_DATE,
            /// <summary>
            /// Search by  SUBJECT field
            /// </summary>
            SUBJECT,
            /// <summary>
            /// Search by  CARD ID field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by IS ATTEMPT_LESS_THAN field
            /// </summary>
            ATTEMPT_LESS_THAN,
            /// <summary>
            /// Search by IS STATUS_NOT_EQ field
            /// </summary>
            STATUS_NOT_EQ,
            /// <summary>
            /// Search by MESSAGE_TYPE field
            /// </summary>
            MESSAGE_TYPE,
            /// <summary>
            /// Search by FROM_DATE field
            /// </summary>
            FROM_DATE,
            /// <summary>
            /// Search by TO_DATE field
            /// </summary>
            TO_DATE,
            /// <summary>
            /// Search by MESSAGE_READ field
            /// </summary>
            MESSAGE_READ,
            /// <summary>
            /// Search by IS MESSAGING_CLIENT_ID field
            /// </summary>
            MESSAGING_CLIENT_ID,
            /// <summary>
            /// Search by IS SIGNED_IN_CUSTOMERS_ONLY field
            /// </summary>
            SIGNED_IN_CUSTOMERS_ONLY,
            /// <summary>
            /// Search by IS PARAFAIT_FUNCTION_EVENT_ID field
            /// </summary>
            PARAFAIT_FUNCTION_EVENT_ID,
            /// <summary>
            /// Search by IS ID_LIST field
            /// </summary>
            ID_LIST
        }
        private int id;
        private int? batchId;
        private string reference;
        private string messageType;
        private string toEmails;
        private string toMobile;
        private string status;
        private string statusMessage;
        private DateTime? sendDate;
        private DateTime? sendAttemptDate;
        private int? attempts;
        private string subject;
        private string body;
        private int customerId;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private int? cardId;
        private string attachFile;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime creationDate;
        private string cc;
        private string bcc;
        private string messagingClientName;
        private int messagingClientId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool activeFlag;
        private bool messageRead;
        private bool signedInCustomersOnly;
        private string toDevice;
        private string countryCode;
        private string trxNumber;
        private int? parafaitFunctionEventId;
        private int? originalMessageId;
        private int? trxId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MessagingRequestDTO()
        {
            log.LogMethodEntry();
            id = -1;
            customerId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public MessagingRequestDTO(int id, int? batchId, string reference, string messageType, string toEmails, string toMobile,
                                   string status, string statusMessage, DateTime? sendDate, DateTime? sendAttemptDate, int? attempts,
                                   string subject, string body, int customerId, int? cardId, string attachFile,bool activeFlag, string cc,
                                   string bcc, int messagingClientId, bool messageRead, string toDevice, bool signedInCustomersOnly = true, string countryCode = null,
                                   string trxNumber = null, int? parafaitFunctionEventId = null, int? originalMessageId = null, int? trxId = null)
            :this()
        {
            log.LogMethodEntry(id, batchId, reference, messageType, toEmails, toMobile, status, statusMessage, sendDate,
                               sendAttemptDate, attempts, subject, body, customerId, cardId, attachFile, activeFlag, cc, bcc, messagingClientName, messageRead, toDevice, signedInCustomersOnly, countryCode,
                               trxNumber, parafaitFunctionEventId, originalMessageId, trxId);

            this.id = id;
            this.batchId = batchId;
            this.reference = reference;
            this.messageType = messageType;
            this.toEmails = toEmails;
            this.toMobile = toMobile;
            this.status = status;
            this.statusMessage = statusMessage;
            this.sendDate = sendDate;
            this.sendAttemptDate = sendAttemptDate;
            this.attempts = attempts;
            this.subject = subject;
            this.body = body;
            this.customerId = customerId;
            this.cardId = cardId;
            this.attachFile = attachFile;
            this.activeFlag = activeFlag;
            this.cc = cc;
            this.bcc = bcc;
            this.messagingClientId = messagingClientId;
            this.messageRead = messageRead;
            this.toDevice = toDevice;
            this.signedInCustomersOnly = signedInCustomersOnly;
            this.countryCode = countryCode;
            this.trxNumber = trxNumber;
            this.parafaitFunctionEventId = parafaitFunctionEventId;
            this.originalMessageId = originalMessageId;
            this.trxId = trxId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public MessagingRequestDTO(int id, int? batchId, string reference, string messageType, string toEmails, string toMobile,
                                   string status, string statusMessage, DateTime? sendDate, DateTime? sendAttemptDate, int? attempts,
                                   string subject, string body, int customerId, string guid, int siteId, bool synchStatus, int masterEntityId,
                                   int? cardId, string attachFile, DateTime lastUpdatedDate, string lastUpdatedBy, string createdBy, DateTime creationDate, bool activeFlag,
                                   string cc, string bcc, int messagingClientId, bool messageRead, string toDevice, bool signedInCustomersOnly, string countryCode,
                                   string trxNumber, int? parafaitFunctionEventId, int? originalMessageId, int? trxId)
            : this(id, batchId, reference, messageType, toEmails, toMobile, status, statusMessage, sendDate,
                               sendAttemptDate, attempts, subject, body, customerId, cardId, attachFile, activeFlag, cc, bcc, messagingClientId, messageRead, toDevice, signedInCustomersOnly, countryCode, 
                               trxNumber, parafaitFunctionEventId, originalMessageId, trxId)
        {
            log.LogMethodEntry(id, batchId, reference, messageType, toEmails, toMobile,status, statusMessage, sendDate,
                               sendAttemptDate, attempts, subject, body, customerId, guid, siteId,synchStatus, masterEntityId,
                               cardId, attachFile, lastUpdatedDate, lastUpdatedBy, createdBy, creationDate, activeFlag, cc, bcc, messagingClientId, messageRead, toDevice, signedInCustomersOnly, countryCode, 
                               trxNumber, parafaitFunctionEventId, originalMessageId, trxId);

            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id  field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the BatchId field
        /// </summary>
        public int? BatchId
        {
            get { return batchId; }
            set { batchId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the active flag  field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Reference field
        /// </summary>
        public string Reference
        {
            get { return reference; }
            set { reference = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MessageType field
        /// </summary>
        public string MessageType
        {
            get { return messageType; }
            set { messageType = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ToEmail field
        /// </summary>
        public string ToEmail
        {
            get { return toEmails; }
            set { toEmails = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ToMobile field
        /// </summary>
        public string ToMobile
        {
            get { return toMobile; }
            set { toMobile = value; this.IsChanged = true; }
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
        /// Get/Set method of the StatusMessage field
        /// </summary>
        public string StatusMessage
        {
            get { return statusMessage; }
            set { statusMessage = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SendDate field
        /// </summary>
        public DateTime? SendDate
        {
            get { return sendDate; }
            set { sendDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the sendAttemptDate field
        /// </summary>
        public DateTime? SendAttemptDate
        {
            get { return sendAttemptDate; }
            set { sendAttemptDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Attempts field
        /// </summary>
        public int? Attempts
        {
            get { return attempts; }
            set { attempts = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Subject field
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Body field
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
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
        /// Get/Set method of the CardId field
        /// </summary>
        public int? CardId
        {
            get { return cardId; }
            set { cardId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AttachFile field
        /// </summary>
        public string AttachFile
        {
            get { return attachFile; }
            set { attachFile = value; this.IsChanged = true; }
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
        /// Get/Set method of the MessageRead field
        /// </summary>
        public bool MessageRead
        {
            get { return messageRead; }
            set { messageRead = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Cc field
        /// </summary>
        public string Cc
        {
            get { return cc; }
            set { cc = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Bcc field
        /// </summary>
        public string Bcc
        {
            get { return bcc; }
            set { bcc = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MessagingClientName field
        /// </summary>
        public string MessagingClientName
        {
            get { return messagingClientName; }
            set { messagingClientName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MessagingClientName field
        /// </summary>
        public int MessagingClientId
        {
            get { return messagingClientId; }
            set { messagingClientId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MessagingClientName field
        /// </summary>
        public string ToDevice
        {
            get { return toDevice; }
            set { toDevice = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SignedInCustomersOnly field
        /// </summary>
        public bool SignedInCustomersOnly
        {
            get { return signedInCustomersOnly; }
            set { signedInCustomersOnly = value;  this.IsChanged = true;}
        }

        /// <summary>
        /// Get/Set method of the CountryCode field
        /// </summary>
        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the TrxNumber field
        /// </summary>
        public string TrxNumber
        {
            get { return trxNumber; }
            set { trxNumber = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ParafaitFunctionEventId field
        /// </summary>
        public int? ParafaitFunctionEventId
        {
            get { return parafaitFunctionEventId; }
            set { parafaitFunctionEventId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the OriginalMessageId field
        /// </summary>
        public int? OriginalMessageId
        {
            get { return originalMessageId; }
            set { originalMessageId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int? TrxId
        {
            get { return trxId; }
            set { trxId = value; this.IsChanged = true; }
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

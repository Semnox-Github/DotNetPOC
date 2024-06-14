/********************************************************************************************
 * Project Name - Communication
 * Description  - Data object class for MessagingRequestSummaryView
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
  2.150.01     03-Feb-2023    Yashodhara C H         Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// This is the MessagingRequest data object class. This acts as data holder for the MessagingRequest business object
    /// </summary>
    public class MessagingRequestSummaryViewDTO
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
            MESSAGE_ID,
            /// <summary>
            /// Search by CUSTOMER ID field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  SUBJECT field
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
            /// Search by ORIGINAL_REFERENCENUMBER field
            /// </summary>
            ORIGINAL_MESSAGE_ID,
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
            MESSAGE_ID_LIST,
            /// <summary>
            /// Search by IS PARAFAIT_FUNCTION_EVENT_ID_LIST field
            /// </summary>
            PARAFAIT_FUNCTION_EVENT_ID_LIST,
            /// <summary>
            /// Search by IS ID_ORIG_ID field
            /// </summary>
            PARENT_AND_CHILD_MESSAGES_BY_ID,
            /// <summary>
            /// Search by TO_EMAILS
            /// </summary>
            TO_EMAIL_LIST,
            /// <summary>
            /// Search by TO_MOBILES
            /// </summary>
            TO_MOBILE_LIST,
            /// <summary>
            /// Search by TRX_NUMBER
            /// </summary>
            TRX_NUMBER,
            /// <summary>
            /// Search by TRANSACTION_OTP
            /// </summary>
            TRX_OTP,
            /// <summary>
            /// Search by TRX_OTP_LIST
            /// </summary>
            TRX_OTP_LIST,
            /// <summary>
            /// Search by TRX_NUMBER_LIST
            /// </summary>
            TRX_NUMBER_LIST,
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
        private bool messageRead;
        private bool signedInCustomersOnly;
        private string toDevice;
        private string countryCode;
        private string trxNumber;
        private int? resendCount;
        private int? parafaitFunctionEventId;
        private int? originalMessageId;
        private int? trxId;
        private string parafaitFunctionEventName;
        private string customerName;
        private string clientName;
        private string trxOTP;

        public static string GetMessageType(String messageType)
        {
            string returnValue = string.Empty;
            switch (messageType.ToUpper().Replace(" ", ""))
            {
                case "EMAIL":
                    returnValue = "E";
                    break;
                case "SMS":
                    returnValue = "S";
                    break;
                case "APPNOTIFICATION":
                    returnValue = "A";
                    break;
                default:
                    returnValue = "";
                    break;
            }
            return returnValue;
        }

        public static string ToMessageType(String messageType)
        {
            string returnValue = string.Empty;
            switch (messageType.ToUpper().Replace(" ", ""))
            {
                case "E":
                    returnValue = "Email";
                    break;
                case "S":
                    returnValue = "SMS";
                    break;
                case "A":
                    returnValue = "App Notification";
                    break;
                default:
                    returnValue = "";
                    break;
            }
            return returnValue;
        }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public MessagingRequestSummaryViewDTO()
        {
            log.LogMethodEntry();
            id = -1;
            customerId = -1;
            siteId = -1;
            masterEntityId = -1;
            parafaitFunctionEventId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public MessagingRequestSummaryViewDTO(int id, int? batchId, string reference, string messageType, string toEmails, string toMobile,
                                   string status, string statusMessage, DateTime? sendDate, DateTime? sendAttemptDate, int? attempts,
                                   string subject, string body, int customerId, int? cardId, string attachFile, string cc,
                                   string bcc, int messagingClientId, bool messageRead, string toDevice, bool signedInCustomersOnly = true, 
                                   string countryCode = null, string trxNumber = null, int? resendCount = null, int? parafaitFunctionEventId = null, 
                                   int? originalMessageId = null, int? trxId = null, string parafaitFunctionEventName = null, string customerName = null, string clientName = null, 
                                   string trxOTP =  null)
            :this()
        {
            log.LogMethodEntry(id, batchId, reference, messageType, toEmails, toMobile, status, statusMessage, sendDate,
                               sendAttemptDate, attempts, subject, body, customerId, cardId, attachFile, cc, bcc, messagingClientName, messageRead, toDevice, signedInCustomersOnly, countryCode,
                               trxNumber, resendCount, parafaitFunctionEventId, originalMessageId, trxId, customerName, clientName, trxOTP);

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
            this.cc = cc;
            this.bcc = bcc;
            this.messagingClientId = messagingClientId;
            this.messageRead = messageRead;
            this.toDevice = toDevice;
            this.signedInCustomersOnly = signedInCustomersOnly;
            this.countryCode = countryCode;
            this.trxNumber = trxNumber;
            this.resendCount = resendCount;
            this.parafaitFunctionEventId = parafaitFunctionEventId;
            this.originalMessageId = originalMessageId;
            this.trxId = trxId;
            this.parafaitFunctionEventName = parafaitFunctionEventName;
            this.customerName = customerName;
            this.clientName = clientName;
            this.trxOTP = trxOTP;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public MessagingRequestSummaryViewDTO(int id, int? batchId, string reference, string messageType, string toEmails, string toMobile,
                                   string status, string statusMessage, DateTime? sendDate, DateTime? sendAttemptDate, int? attempts,
                                   string subject, string body, int customerId, string guid,int siteId, bool synchStatus,int masterEntityId,
                                   int? cardId,string attachFile, DateTime lastUpdatedDate, string lastUpdatedBy, string createdBy, DateTime creationDate,
                                   string cc, string bcc, int messagingClientId, bool messageRead, string toDevice, bool signedInCustomersOnly, string countryCode,
                                   string trxNumber, int? resendCount, int? parafaitFunctionEventId, int? originalMessageId, int? trxId, string parafaitFunctionEventName, string customerName,
                                   string clientName, string trxOTP = null)
            :this(id, batchId, reference, messageType, toEmails, toMobile, status, statusMessage, sendDate, sendAttemptDate, attempts, subject, body, 
                  customerId, cardId, attachFile, cc, bcc, messagingClientId, messageRead, toDevice, signedInCustomersOnly, 
                  countryCode, trxNumber, resendCount, parafaitFunctionEventId, originalMessageId, trxId, parafaitFunctionEventName, customerName, clientName, trxOTP)
        {
            log.LogMethodEntry(id, batchId, reference, messageType, toEmails, toMobile,status, statusMessage, sendDate,
                               sendAttemptDate, attempts, subject, body, customerId, guid, siteId,synchStatus, masterEntityId,
                               cardId, attachFile, lastUpdatedDate, lastUpdatedBy, createdBy, creationDate, cc, bcc, messagingClientId, messageRead, toDevice, signedInCustomersOnly, countryCode, 
                               trxNumber, resendCount, parafaitFunctionEventId, originalMessageId, trxId, parafaitFunctionEventName, customerName, clientName, trxOTP);

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
            set { id = value; }
        }
        /// <summary>
        /// Get/Set method of the BatchId field
        /// </summary>
        public int? BatchId
        {
            get { return batchId; }
            set { batchId = value; }
        }

        /// <summary>
        /// Get/Set method of the Reference field
        /// </summary>
        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }
        /// <summary>
        /// Get/Set method of the MessageType field
        /// </summary>
        public string MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }
        /// <summary>
        /// Get/Set method of the ToEmail field
        /// </summary>
        public string ToEmail
        {
            get { return toEmails; }
            set { toEmails = value; }
        }
        /// <summary>
        /// Get/Set method of the ToMobile field
        /// </summary>
        public string ToMobile
        {
            get { return toMobile; }
            set { toMobile = value; }
        }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// Get/Set method of the StatusMessage field
        /// </summary>
        public string StatusMessage
        {
            get { return statusMessage; }
            set { statusMessage = value; }
        }
        /// <summary>
        /// Get/Set method of the SendDate field
        /// </summary>
        public DateTime? SendDate
        {
            get { return sendDate; }
            set { sendDate = value; }
        }
        /// <summary>
        /// Get/Set method of the sendAttemptDate field
        /// </summary>
        public DateTime? SendAttemptDate
        {
            get { return sendAttemptDate; }
            set { sendAttemptDate = value; }
        }
        /// <summary>
        /// Get/Set method of the Attempts field
        /// </summary>
        public int? Attempts
        {
            get { return attempts; }
            set { attempts = value; }
        }
        /// <summary>
        /// Get/Set method of the Subject field
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        /// <summary>
        /// Get/Set method of the Body field
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value; }
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
            set { customerId = value; }
        }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int? CardId
        {
            get { return cardId; }
            set { cardId = value; }
        }
        /// <summary>
        /// Get/Set method of the AttachFile field
        /// </summary>
        public string AttachFile
        {
            get { return attachFile; }
            set { attachFile = value; }
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
            set { guid = value; }
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
            set { masterEntityId = value; }
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
            set { messageRead = value; }
        }

        /// <summary>
        /// Get/Set method of the Cc field
        /// </summary>
        public string Cc
        {
            get { return cc; }
            set { cc = value; }
        }
        /// <summary>
        /// Get/Set method of the Bcc field
        /// </summary>
        public string Bcc
        {
            get { return bcc; }
            set { bcc = value; }
        }
        /// <summary>
        /// Get/Set method of the MessagingClientName field
        /// </summary>
        public string MessagingClientName
        {
            get { return messagingClientName; }
            set { messagingClientName = value; }
        }

        /// <summary>
        /// Get/Set method of the MessagingClientName field
        /// </summary>
        public int MessagingClientId
        {
            get { return messagingClientId; }
            set { messagingClientId = value; }
        }

        /// <summary>
        /// Get/Set method of the MessagingClientName field
        /// </summary>
        public string ToDevice
        {
            get { return toDevice; }
            set { toDevice = value; }
        }

        /// <summary>
        /// Get/Set method of the SignedInCustomersOnly field
        /// </summary>
        public bool SignedInCustomersOnly
        {
            get { return signedInCustomersOnly; }
            set { signedInCustomersOnly = value; }
        }

        /// <summary>
        /// Get/Set method of the CountryCode field
        /// </summary>
        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }

        /// <summary>
        /// Get/Set method of the TrxNumber field
        /// </summary>
        public string TrxNumber
        {
            get { return trxNumber; }
            set { trxNumber = value; }
        }

        /// <summary>
        /// Get/Set method of the ResendCount field
        /// </summary>
        public int? ResendCount
        {
            get { return resendCount; }
            set { resendCount = value; }
        }

        /// <summary>
        /// Get/Set method of the ParafaitFunctionEventId field
        /// </summary>
        public int? ParafaitFunctionEventId
        {
            get { return parafaitFunctionEventId; }
            set { parafaitFunctionEventId = value; }
        }

        /// <summary>
        /// Get/Set method of the originalMessageId field
        /// </summary>
        public int? OriginalMessageId
        {
            get { return originalMessageId; }
            set { originalMessageId = value; }
        }

        /// <summary>
        /// Get/Set method of the trxId field
        /// </summary>
        public int? TrxId
        {
            get { return trxId; }
            set { trxId = value; }
        }

        /// <summary>
        /// Get/Set method of the parafaitFunctionEvent field.
        /// </summary>
        public string ParafaitFunctionEventName
        {
            get { return parafaitFunctionEventName; }
            set { parafaitFunctionEventName = value;}
        }

        /// <summary>
        /// Get/Set method of the CustomerName field.
        /// </summary>
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        /// <summary>
        /// Get/Set method of the ClientName field.
        /// </summary>
        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }

        public string TrxOTP
        {
            get { return trxOTP; }
            set { trxOTP = value; }
        }
    }
}

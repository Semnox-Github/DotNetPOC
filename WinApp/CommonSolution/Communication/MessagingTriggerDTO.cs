/********************************************************************************************
 * Project Name - Communication
 * Description  - Data object of MessagingTriggers
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70       24-May-2019     Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// This is the MessagingTrigger data object class. This acts as data holder for the MessagingTriggers business object
    /// </summary>
    public class MessagingTriggerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    TRIGGER ID field
            /// </summary>
            TRIGGER_ID,
            /// <summary>
            /// Search by    TRIGGER NAME field
            /// </summary>
            TRIGGER_NAME,
            /// <summary>
            /// Search by TYPE CODE field
            /// </summary>
            TYPE_CODE,
            /// <summary>
            /// Search by  RECEIPT TEMPLATE  ID field
            /// </summary>
            RECEIPT_TEMPLATE_ID,
            /// <summary>
            /// Search by MESSAGE TYPE field
            /// </summary>
            MESSAGE_TYPE,
            /// <summary>
            /// Search by  SEND RECEIPT field
            /// </summary>
            SEND_RECEIPT,
            /// <summary>
            /// Search by  EMAIL SUBJECT  field
            /// </summary>
            EMAIL_SUBJECT,
            /// <summary>
            /// Search by  ACTIVE FLAG field
            /// </summary>
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
        private int triggerId;
        private string triggerName;
        private char typeCode;
        private bool activeFlag;
        private bool? vipOnly;
        private char? messageType;
        private string smsTemplate;
        private string emailSubject;
        private string emailTemplate;
        private decimal? minimumSaleAmount;
        private int? minimumTicketCount;
        private bool? messageCustomer;
        private string smsNumbers;
        private string emailIds;
        private bool? sendReceipt;
        private int? receiptTemplateId;
        private DateTime? timeStamp;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private List<MessagingTriggerCriteriaDTO> messagingTriggerCriteriaDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MessagingTriggerDTO()
        {
            log.LogMethodEntry();
            triggerId = -1;
            receiptTemplateId = -1;
            activeFlag = true;
            siteId = -1;
            vipOnly = false;
            messageCustomer = false;
            sendReceipt = false;
            masterEntityId = -1;
            minimumSaleAmount = null;
            minimumTicketCount = null;
            timeStamp = null;
            messageCustomer = null;
            receiptTemplateId = null;
            messagingTriggerCriteriaDTOList = new List<MessagingTriggerCriteriaDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public MessagingTriggerDTO(int triggerId, string triggerName, char typeCode, bool activeFlag, bool? vipOnly, char? messageType,
                                   string smsTemplate, string emailSubject, string emailTemplate, decimal? minimumSaleAmount,
                                   int? minimumTicketCount, bool? messageCustomer, string smsNumbers, string emailIds, bool? sendReceipt,
                                   int? receiptTemplateId, DateTime? timeStamp)
            :this()
        {
            log.LogMethodEntry(triggerId, triggerName, typeCode, activeFlag, vipOnly, messageType, smsTemplate, emailSubject,
                               emailTemplate, minimumSaleAmount, minimumTicketCount, messageCustomer, smsNumbers,
                               emailIds, sendReceipt, receiptTemplateId, timeStamp);

            this.triggerId = triggerId;
            this.triggerName = triggerName;
            this.typeCode = typeCode;
            this.activeFlag = activeFlag;
            this.vipOnly = vipOnly;
            this.messageType = messageType;
            this.smsTemplate = smsTemplate;
            this.emailSubject = emailSubject;
            this.emailTemplate = emailTemplate;
            this.minimumSaleAmount = minimumSaleAmount;
            this.minimumTicketCount = minimumTicketCount;
            this.messageCustomer = messageCustomer;
            this.smsNumbers = smsNumbers;
            this.emailIds = emailIds;
            this.sendReceipt = sendReceipt;
            this.receiptTemplateId = receiptTemplateId;
            this.timeStamp = timeStamp;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public MessagingTriggerDTO(int triggerId, string triggerName, char typeCode, bool activeFlag,bool? vipOnly, char? messageType,
                                   string smsTemplate, string emailSubject, string emailTemplate, decimal? minimumSaleAmount,
                                   int? minimumTicketCount,  bool? messageCustomer, string smsNumbers, string emailIds, bool? sendReceipt,
                                   int? receiptTemplateId,DateTime? timeStamp,string lastUpdatedBy,DateTime lastUpdatedDate, string guid,
                                   int siteId,  bool synchStatus,  int masterEntityId, string createdBy,  DateTime creationDate)
            :this(triggerId, triggerName, typeCode, activeFlag, vipOnly, messageType, smsTemplate, emailSubject,
                               emailTemplate, minimumSaleAmount, minimumTicketCount, messageCustomer, smsNumbers,
                               emailIds, sendReceipt, receiptTemplateId, timeStamp)
        {
            log.LogMethodEntry(triggerId, triggerName, typeCode, activeFlag,vipOnly, messageType, smsTemplate, emailSubject,
                               emailTemplate, minimumSaleAmount, minimumTicketCount,  messageCustomer, smsNumbers,
                               emailIds,  sendReceipt, receiptTemplateId,  timeStamp, lastUpdatedBy, lastUpdatedDate, guid,
                               siteId, synchStatus, masterEntityId, createdBy, creationDate );
            this.masterEntityId = masterEntityId;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the TriggerId  field
        /// </summary>
        public int TriggerId
        {
            get { return triggerId; }
            set { triggerId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TriggerName field
        /// </summary>
        public string TriggerName
        {
            get { return triggerName; }
            set { triggerName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TypeCode field
        /// </summary>
        public char TypeCode
        {
            get { return typeCode; }
            set { typeCode = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VIPOnly field
        /// </summary>
        public bool? VIPOnly
        {
            get { return vipOnly; }
            set { vipOnly = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MessageType field
        /// </summary>
        public char? MessageType
        {
            get { return messageType; }
            set { messageType = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SMSTemplate field
        /// </summary>
        public string SMSTemplate
        {
            get { return smsTemplate; }
            set { smsTemplate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EmailSubject field
        /// </summary>
        public string EmailSubject
        {
            get { return emailSubject; }
            set { emailSubject = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EmailTemplate field
        /// </summary>
        public string EmailTemplate
        {
            get { return emailTemplate; }
            set { emailTemplate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MinimumSaleAmount field
        /// </summary>
        public Decimal? MinimumSaleAmount
        {
            get { return minimumSaleAmount; }
            set { minimumSaleAmount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MinimumTicketCount field
        /// </summary>
        public int? MinimumTicketCount
        {
            get { return minimumTicketCount; }
            set { minimumTicketCount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MessageCustomer field
        /// </summary>
        public bool? MessageCustomer
        {
            get { return messageCustomer; }
            set { messageCustomer = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SMSNumbers field
        /// </summary>
        public string SMSNumbers
        {
            get { return smsNumbers; }
            set { smsNumbers = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EmailIds field
        /// </summary>
        public string EmailIds
        {
            get { return emailIds; }
            set { emailIds = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
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
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SendReceipt field
        /// </summary>
        public bool? SendReceipt
        {
            get { return sendReceipt; }
            set { sendReceipt = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReceiptTemplateId field
        /// </summary>
        public int? ReceiptTemplateId
        {
            get { return receiptTemplateId; }
            set { receiptTemplateId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TimeStamp field
        /// </summary>
        public DateTime? TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; this.IsChanged = true; }
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
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the MessagingTriggerCriteriaDTOList field
        /// </summary>
        public List<MessagingTriggerCriteriaDTO> MessagingTriggerCriteriaDTOList
        {
            get { return messagingTriggerCriteriaDTOList; }
            set { messagingTriggerCriteriaDTOList = value; }
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
                    return notifyingObjectIsChanged || triggerId < 0;
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
        /// Returns whether the MessagingTriggerDTO changed or any of its MessagingTriggerCriteria DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (messagingTriggerCriteriaDTOList != null &&
                  messagingTriggerCriteriaDTOList.Any(x => x.IsChanged))
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - MessagingClientFunctionLookUpDTO
 * Description  - Data object of the messaing client Function LookUp
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80.0      25-Jun-2020   Jinto Thomas   Created.
 *2.100.0     22-Aug-2020   Vikas Dwivedi  Modified as per 3-Tier Standard CheckList 
 *2.110.0     10-Dec-2020   Fiona          For Subscription changes                  
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Communication
{
    public class MessagingClientFunctionLookUpDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by MESSAGE CLIENT ID field
            /// </summary>
            CLIENT_ID,
            ///// <summary>
            ///// Search by LOOKUP ID field
            ///// </summary>
            //LOOKUP_ID,
            ///// <summary>
            ///// Search by LOOKUP VALUE ID field
            ///// </summary>
            //LOOKUP_VALUE_ID,
            /// <summary>
            /// Search by MESSAGE TYPE field
            /// </summary>
            MESSAGE_TYPE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary> 
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by EVENT ID field
            /// </summary> 
            PARAFAIT_FUNCTION_EVENT_ID,
            /// <summary>
            /// Search by PARAFAIT_FUNCTION_EVENT_NAME field
            /// </summary> 
            PARAFAIT_FUNCTION_EVENT_NAME,
            /// <summary>
            /// Search by PARAFAIT_FUNCTION_NAME field
            /// </summary> 
            PARAFAIT_FUNCTION_NAME
        }

        private int id;
        private int messageClientId;
       // private int lookUpId;
       // private int lookUpValueId;
        private string messageType;
        private bool synchStatus;
        private string guid;
        private int siteId;
        private bool isActive;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private MessagingClientDTO messagingClientDTO;
        private string ccList;
        private string bccList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private int parafaitFunctionEventId;
        private int messageTemplateId;
        private int receiptPrintTemplateId; 

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MessagingClientFunctionLookUpDTO()
        {
            log.LogMethodEntry();
            id = -1;
            messageClientId = -1;
            //lookUpId = -1;
            //lookUpValueId = -1;
            messageType = string.Empty;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            parafaitFunctionEventId = -1;
            messageTemplateId = -1;
            receiptPrintTemplateId = -1;
            ccList = string.Empty;
            bccList = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>

        public MessagingClientFunctionLookUpDTO(int id, int messageClientId, string messageType, int parafaitFunctionEventId, int messageTemplateId, int receiptPrintTemplateId, string ccList, string bccList)
            : this()
        {
            log.LogMethodEntry(id, messageClientId, messageType, parafaitFunctionEventId, messageTemplateId, receiptPrintTemplateId, ccList, bccList);
            this.id = id;
            this.messageClientId = messageClientId;
           // this.lookUpId = lookUpId;
            //this.lookUpValueId = lookUpValueId;
            this.messageType = messageType;
            this.parafaitFunctionEventId = parafaitFunctionEventId;
            this.messageTemplateId = messageTemplateId;
            this.receiptPrintTemplateId = receiptPrintTemplateId;
            this.ccList = ccList;
            this.bccList = bccList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>

        public MessagingClientFunctionLookUpDTO(int id, int messageClientId, string messageType, int parafaitFunctionEventId, int messageTemplateId, int receiptPrintTemplateId,
                                                string ccList, string bccList, bool synchStatus, string guid, int siteId, bool isActive,
                                                int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate)
            : this(id, messageClientId, messageType, parafaitFunctionEventId, messageTemplateId, receiptPrintTemplateId, ccList, bccList)
        {
            log.LogMethodEntry(id, messageClientId, messageType, parafaitFunctionEventId, messageTemplateId, receiptPrintTemplateId, ccList, bccList,
                              synchStatus, guid, siteId, isActive, masterEntityId, createdBy,  creationDate, lastUpdatedBy, lastUpdatedDate);
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.siteId = siteId;
            this.isActive = isActive;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
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
        /// Get/Set method of the MessageClientId  field
        /// </summary>

        public int MessageClientId
        {
            get { return messageClientId; }
            set { messageClientId = value; this.IsChanged = true; }
        }

        ///// <summary>
        ///// Get/Set method of the LookUpValueId  field
        ///// </summary>

        //public int LookUpId
        //{
        //    get { return lookUpId; }
        //    set { lookUpId = value; this.IsChanged = true; }
        //}

        ///// <summary>
        ///// Get/Set method of the LookUpValueId  field
        ///// </summary>

        //public int LookUpValueId
        //{
        //    get { return lookUpValueId; }
        //    set { lookUpValueId = value; this.IsChanged = true; }
        //}

        /// <summary>
        /// Get/Set method of the MessageType  field
        /// </summary>
        public string MessageType
        {
            get { return messageType; }
            set { messageType = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ccList  field
        /// </summary>
        public string CCList
        {
            get { return ccList; }
            set { ccList = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the bccList  field
        /// </summary>
        public string BCCList
        {
            get { return bccList; }
            set { bccList = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SysnchStatus  field
        /// </summary>

        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }

        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteId  field
        /// </summary>

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
        /// Get/Set method of the ParafaitFunctionEventId field
        /// </summary>
        public int ParafaitFunctionEventId
        {
            get { return parafaitFunctionEventId; }
            set { parafaitFunctionEventId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MessageTemplateId field
        /// </summary>
        public int MessageTemplateId
        {
            get { return messageTemplateId; }
            set { messageTemplateId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MessageTemplateId field
        /// </summary>
        public int ReceiptPrintTemplateId
        {
            get { return receiptPrintTemplateId; }
            set { receiptPrintTemplateId = value; this.IsChanged = true; }
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
            set { creationDate = value; }
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
        /// Get/Set method of the LastUpdatedDate  field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate  field
        /// </summary>

        public MessagingClientDTO MessagingClientDTO
        {
            get { return messagingClientDTO; }
            set { messagingClientDTO = value; }
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

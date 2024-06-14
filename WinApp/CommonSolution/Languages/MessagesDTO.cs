/********************************************************************************************
 * Project Name - MessagesDTO
 * Description  - Data object of the messages
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        04-Jan-2017   Vinayaka V          Created 
 *2.60        06-May-2019   Mushahid Faizan     Added AcceptChanges() method and IsChanged Property.
                                                Added child list i.e  List<TranslatedMessageDTO> TranslatedMessageList for child records.
 *2.70.2.0      17-Jul-2019    Girish Kundar      modified : Added Parametrized Constructor with required fields
 *            29-Jul-2019    Mushahid Faizan     Added IsActive column
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// This is the Messages data object class. This acts as data holder for the Messages business object
    /// </summary>
    public class MessagesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByMessageParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByMessageParameters
        {
            /// <summary>
            /// Search by MESSAGE_ID field
            /// </summary>
            MESSAGE_ID,
            /// <summary>
            /// Search by MESSAGE_NO field
            /// </summary>
            MESSAGE_NO,
            /// <summary>
            /// Search by LITERALS_ONLY field
            /// </summary>
            LITERALS_ONLY,
            /// <summary>
            /// Search by MESSAGE field
            /// </summary>
            MESSAGE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary> 
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by MESSAGES_ONLY field
            /// </summary>
            MESSAGES_ONLY,
            /// <summary>
            /// Search by LANGUAGE_ID field
            /// </summary>
            LANGUAGE_ID,
        }


        private int messageId;
        private int messageNo;
        private string message;
        private int site_Id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;        
        private List<TranslatedMessageDTO> translatedMessagesDTO;


        /// <summary>
        /// Default constructor
        /// </summary>
        public MessagesDTO()
        {
            log.LogMethodEntry();
            messageId = -1;
            messageNo = -1;
            message = "";
            site_Id = -1;
            guid = "";
            synchStatus = false;
            masterEntityId = -1;
            isActive = true;
            translatedMessagesDTO = new List<TranslatedMessageDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required  parameters
        /// </summary>
        public MessagesDTO(int messageId, int messageNo, string message)
            :this()
        {
            log.LogMethodEntry(messageId,  messageNo,  message);
            this.messageId = messageId;
            this.messageNo = messageNo;
            this.message = message;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        public MessagesDTO(int messageId, int messageNo, string message,
                           int site_Id, string guid, bool synchStatus, int masterEntityId, bool isActive, string createdBy,
                           DateTime creationDate ,string lastUpdatedBy ,DateTime lastUpdateDate)
            :this(messageId,  messageNo, message)
        {
            log.LogMethodEntry(messageId,  messageNo,  message,
                               site_Id,  guid,  synchStatus,  masterEntityId, isActive, createdBy,
                               creationDate,  lastUpdatedBy,  lastUpdateDate);
           
            this.site_Id = site_Id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MessageId field
        /// </summary>
        [DisplayName("MessageId")]
        public int MessageId { get { return messageId; } set { messageId = value; } }
        /// <summary>
        /// Get/Set method of the MessageNo field
        /// </summary>
        [DisplayName("MessageNo")]
        public int MessageNo { get { return messageNo; } set { messageNo = value; } }
        /// <summary>
        /// Get/Set method of the message field
        /// </summary>
        [DisplayName("Message")]
        public string Message { get { return message; } set { message = value; } }
        /// <summary>
        /// Get/Set method of the Site Id field
        /// </summary>
        [DisplayName("Site_Id")]
        public int Site_Id { get { return site_Id; } set { site_Id = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the Master EntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }


        /// </summary>
        public List<TranslatedMessageDTO> TranslatedMessageList
        {
            get
            {
                return translatedMessagesDTO;
            }

            set
            {
                translatedMessagesDTO = value;
            }
        }

        /// <summary>
        /// Returns whether the MessagesDTO changed or any of its TranslatedMessagesDTO children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (translatedMessagesDTO != null &&
                   translatedMessagesDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }
        
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || messageId < 0;
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

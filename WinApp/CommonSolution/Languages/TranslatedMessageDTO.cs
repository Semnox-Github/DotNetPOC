/********************************************************************************************
 * Project Name - Site Setup
 * Description  - Data Object for the Translated Message details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Mushahid Faizan   Created 
 *2.70.2        17-Jul-2019    Girish Kundar    modified : Added Parametrized Constructor with required fields
 ********************************************************************************************/
using System;
using System.ComponentModel;


namespace Semnox.Parafait.Languages
{
    public class TranslatedMessageDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByMessageParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by MESSAGE_ID field
            /// </summary>
            MESSAGE_ID,
            /// <summary>
            /// Search by MESSAGE_ID field
            /// </summary>
            MESSAGE_ID_LIST,
            /// <summary>
            /// Search by LANGUAGEID field
            /// </summary>
            LANGUAGEID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary> 
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by ACTIVE_LANGUAGES field
            /// </summary> 
            ACTIVE_LANGUAGES
        }

        private int id;
        private int messageId;
        private string message;
        private int languageId;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        /// <summary>
        /// Default Constructor for TranslatedMessage
        /// </summary>
        public TranslatedMessageDTO()
        {
            log.LogMethodEntry();
            id = -1;
            messageId = -1;
            message = string.Empty;
            languageId = -1;
            siteId = -1;
            guid = string.Empty;
            synchStatus = false;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required parameters
        /// </summary>
        public TranslatedMessageDTO(int id, int messageId, string message, int languageId)
            : this()
        {
            log.LogMethodEntry(id, messageId, message, languageId);
            this.id = id;
            this.messageId = messageId;
            this.message = message;
            this.languageId = languageId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        public TranslatedMessageDTO(int id, int messageId, string message, int languageId,
                           int siteId, string guid, bool synchStatus, int masterEntityId,
                           string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            : this(id, messageId, message, languageId)
        {
            log.LogMethodEntry(id, messageId, message, languageId,
                            siteId, guid, synchStatus, masterEntityId,
                            createdBy, creationDate, lastUpdatedBy, lastUpdateDate, isActive);

            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for Id field
        /// </summary>
        public int ID { get { return id; } set { id = value; } }
        /// <summary>
        ///  Get/Set for MessageId field
        /// </summary>
        public int MessageId { get { return messageId; } set { messageId = value; IsChanged = true; } }
        /// <summary>
        ///  Get/Set for Message field
        /// </summary>
        public string Message { get { return message; } set { message = value; IsChanged = true; } }
        /// <summary>
        ///  Get/Set for LanguageId field
        /// </summary>
        public int LanguageId { get { return languageId; } set { languageId = value; IsChanged = true; } }
        /// <summary>
        ///  Get/Set for Site_Id field
        /// </summary>
        public int Site_Id { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the Master EntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// </summary>
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
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
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}

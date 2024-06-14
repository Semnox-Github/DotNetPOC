/********************************************************************************************
 * Project Name - ApplicationContentTranslatedDTO
 * Description  - Data object of ApplicationContentTranslatedDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019     Dakshakh raj     Modified : Added Parameterized costrustor,
 *                                                        IsActive field.
 *2.80       21-May-2020       Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentTranslatedDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by TOKENID field
            /// </summary>
            ID,

            /// <summary>
            /// Search by TOKEN field
            /// </summary>
            APP_CONTENT_ID,
            /// <summary>
            /// Search by AppContentId field
            /// </summary>
            APP_CONTENT_ID_LIST,

            /// <summary>
            /// Search by OBJECT field
            /// </summary>
            LANGUAGE_ID,

            /// <summary>
            /// Search by OBJECT GUID field
            /// </summary>
            CHAPTER,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG
        }

        private int id;
        private int appContentId;
        private int languageId;
        private string chapter;
        private int contentId;
        private string createdBy;
        private bool isActive;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int site_id;
        private bool synchStatus;
        private string guid;
        private int masterEntityId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApplicationContentTranslatedDTO()
        {
            log.LogMethodEntry();

            id = -1;
            appContentId = -1;
            languageId = -1;
            chapter = string.Empty;
            contentId = -1;
            site_id = -1;
            masterEntityId = -1;

            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with required fields
        /// </summary>
        public ApplicationContentTranslatedDTO(int id, int appContentId, int languageId, string chapter, int contentId, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, appContentId, languageId, chapter, contentId, isActive);
            this.id = id;
            this.appContentId = appContentId;
            this.languageId = languageId;
            this.chapter = chapter;
            this.contentId = contentId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        public ApplicationContentTranslatedDTO(int id, int appContentId, int languageId, string chapter, int contentId, bool isActive,
                                               string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                               int site_id, bool synchStatus, string guid, int masterEntityId)
            :this(id, appContentId, languageId, chapter, contentId, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, site_id, synchStatus, guid, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Object field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the TokenId field
        /// </summary>
        [DisplayName("AppContentId")]
        public int AppContentId { get { return appContentId; } set { appContentId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Token field
        /// </summary>
        [DisplayName("LanguageId")]
        public int LanguageId { get { return languageId; } set { languageId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ObjectGuid field
        /// </summary>
        [DisplayName("Chapter")]
        public string Chapter { get { return chapter; } set { chapter = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        [DisplayName("ContentId")]
        public int ContentId { get { return contentId; } set { contentId = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;} }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }


        /// <summary>
        ///  Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        ///  Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
            log.LogMethodExit(null);
        }

    }
}

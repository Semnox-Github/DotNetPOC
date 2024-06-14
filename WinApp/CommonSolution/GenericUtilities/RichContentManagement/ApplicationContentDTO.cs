/********************************************************************************************
 * Project Name - ApplicationContentDTO
 * Description  - Data object of ApplicationContentDTO
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentDTO
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
            /// Search by APP CONTENT ID field
            /// </summary>
            APP_CONTENT_ID,

            /// <summary>
            /// Search by APPLICATION field
            /// </summary>
            APPLICATION,

            /// <summary>
            /// Search by MODULE field
            /// </summary>
            MODULE,

            /// <summary>
            /// Search by CHAPTER field
            /// </summary>
            CHAPTER,

            /// <summary>
            /// Search by CONTENT ID field
            /// </summary>
            CONTENT_ID,

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

        private int appContentId;
        private string application;
        private string module;
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
        private RichContentDTO richContentDTO;
        List<ApplicationContentTranslatedDTO> applicationContentTranslatedDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApplicationContentDTO()
        {
            log.LogMethodEntry();
            appContentId = -1;
            application = string.Empty;
            module = string.Empty;
            site_id = -1;
            masterEntityId = -1;
            contentId = -1;
            applicationContentTranslatedDTOList = new List<ApplicationContentTranslatedDTO>();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// constructor with required fields
        /// </summary>
        public ApplicationContentDTO(int appContentId, string application, string module, string chapter, int contentId, bool isActive)
            :this()
        {
            log.LogMethodEntry(appContentId, application, module, chapter, contentId, isActive);
            this.appContentId = appContentId;
            this.application = application;
            this.module = module;
            this.chapter = chapter;
            this.contentId = contentId;
            this.isActive = isActive;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// constructor with all the parameters
        /// </summary>

        public ApplicationContentDTO(int appContentId, string application, string module, string chapter, int contentId, bool isActive,
                                     string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                     int site_id, bool synchStatus, string guid, int masterEntityId)
            :this(appContentId, application, module, chapter, contentId, isActive)
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
            log.LogMethodExit(null);
        }


        /// <summary>
        /// Get/Set method of the AppContentId field
        /// </summary>
        [DisplayName("AppContentId")]
        public int AppContentId { get { return appContentId; } set { appContentId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Application field
        /// </summary>
        [DisplayName("Application")]
        public string Application { get { return application; } set { application = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Module field
        /// </summary>
        [DisplayName("Module")]
        public string Module { get { return module; } set { module = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Chapter field
        /// </summary>
        [DisplayName("Chapter")]
        public string Chapter { get { return chapter; } set { chapter = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ContentId field
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
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }


        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }



        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }


        /// <summary>
        ///  Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId { get { return site_id; } set { site_id = value;  } }

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
                    return notifyingObjectIsChanged || appContentId < 0;
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
        /// Get/Set Methods for applicationContentTranslatedDTOList field
        /// </summary>
        public List<ApplicationContentTranslatedDTO> ApplicationContentTranslatedDTOList
        {
            get
            {
                return applicationContentTranslatedDTOList;
            }
            set
            {
                applicationContentTranslatedDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set Methods for richContentDTO field
        /// </summary>
        public RichContentDTO RichContentDTO
        {
            get
            {
                return richContentDTO;
            }
            set
            {
                richContentDTO = value;
            }
        }


        /// <summary>
        /// IsChangedRecursive
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (applicationContentTranslatedDTOList != null &&
                   applicationContentTranslatedDTOList.Any(x => x.IsChanged))
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
            this.IsChanged = false;
            log.LogMethodExit(null);
        }

    }
}

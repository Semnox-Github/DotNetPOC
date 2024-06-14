/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Data object of Maintenance Comments
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By       Remarks          
 *********************************************************************************************
 *2.150.3     21-Mar-2022    Abhishek          Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the Maintenance Comments data object class. This acts as data holder for the Maintenance Comments business object
    /// </summary>
    public class MaintenanceCommentsDTO 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByCommentsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCommentsParameters
        {
            /// <summary>
            /// Search by COMMENT_ID field
            /// </summary>
            COMMENT_ID,
            /// <summary>
            /// Search by MAINT_CHECK_LIST_DETAIL_ID field
            /// </summary>
            MAINT_CHECK_LIST_DETAIL_ID,
            /// <summary>
            /// Search by COMMENT_TYPE field
            /// </summary>
            COMMENT_TYPE,
            /// <summary>
            /// Search by COMMENT field
            /// </summary>
            COMMENT,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }
        
        private int commentId;
        private int maintCheckListDetailId;
        private int commentType;
        private string comment;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MaintenanceCommentsDTO()
        {
            log.LogMethodEntry();
            commentId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public MaintenanceCommentsDTO(int commentId, int maintCheckListDetailId, int commentType, string comment, bool isActive)
            : this()
        {
            log.LogMethodEntry(commentId, maintCheckListDetailId, commentType, comment, isActive);
            this.commentId = commentId;
            this.maintCheckListDetailId = maintCheckListDetailId;
            this.commentType = commentType;
            this.comment = comment;
            this.isActive =isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MaintenanceCommentsDTO(int commentId, int maintCheckListDetailId, int commentType, string comment, bool isActive, string createdBy, DateTime creationDate,
                                      string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId)
            :this(commentId, maintCheckListDetailId, commentType, comment, isActive)
        {
            log.LogMethodEntry(commentId, maintCheckListDetailId, commentType, comment, isActive, createdBy, creationDate, lastUpdatedBy, 
                               lastUpdatedDate, guid, siteId, synchStatus, masterEntityId);
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CommentId field
        /// </summary>
        public int CommentId { get { return commentId; } set { commentId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Asset name field
        /// </summary>        
        public int MaintCheckListDetailId { get { return maintCheckListDetailId; } set { maintCheckListDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Asset name field
        /// </summary>        
        public int CommentType { get { return commentType; } set { commentType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Asset name field
        /// </summary>        
        public string Comment { get { return comment; } set { comment = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
        /// Get method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || commentId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}

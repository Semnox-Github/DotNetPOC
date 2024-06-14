/********************************************************************************************
 * Project Name - RichContent DTO
 * Description  - Data object of RichContent DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        2-Sep-2019     Dakshakh raj     Modified : Added Parameterized costrustor,
 *                                                        IsActive field.
 *2.70.2        04-Feb-2020      Nitin Pai           Guest App phase 2 changes                                                        
 ********************************************************************************************/
using System;
using System.ComponentModel;


namespace Semnox.Core.GenericUtilities
{
    public class RichContentDTO
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
            ID ,

            /// <summary>
            /// Search by TOKEN field
            /// </summary>
            CONTENT_NAME ,

            /// <summary>
            /// Search by OBJECT field
            /// </summary>
            FILE_NAME ,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID ,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG
        }

        int id;
        string contentName;
        string fileName;
        byte[] data;
        bool isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        int site_id;
        bool synchStatus;
        string guid;
        int masterEntityId;
        string contentType;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RichContentDTO()
        {
            log.LogMethodEntry();

            id = -1;
            contentName = "";
            fileName = "";
            site_id = -1;
            masterEntityId = -1;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// constructor with required fields
        /// </summary>
        public RichContentDTO(int id, string contentName, string fileName, byte[] data, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, contentName, fileName, data, isActive);
            this.id = id;
            this.contentName = contentName;
            this.fileName = fileName;
            this.data = data;
            this.isActive = isActive;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// constructor with all the parameters
        /// </summary>

        public RichContentDTO(int id, string contentName, string fileName, byte[] data, bool isActive,
            string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
            int site_id, bool synchStatus, string guid, int masterEntityId)
            :this(id, contentName, fileName, data, isActive)

        {
            log.LogMethodEntry();
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
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Token field
        /// </summary>
        [DisplayName("ContentName")]
        public string ContentName { get { return contentName; } set { contentName = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Object field
        /// </summary>
        [DisplayName("FileName")]
        public string FileName { get { return fileName; } set { fileName = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ObjectGuid field
        /// </summary>
        [DisplayName("Data")]
        public byte[] Data { get { return data; } set { data = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }

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
        /// Get/Set method of the Object field
        /// </summary>
        [DisplayName("ContentType")]
        public string ContentType { get { return contentType; } set { contentType = value; } }

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

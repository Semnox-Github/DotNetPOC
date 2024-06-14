/********************************************************************************************
 * Project Name - CMSGroups DTO Class  
 * Description  -Data object class of the CMSGroups
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *********************************************************************************************
 *1.00        23-Sept-2016    Rakshith          Created 
 *2.70        09-Jul-2019     Girish Kundar      Modified : Added constructor with required parameters.
 *                                                          Added masterEntity   field.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the  CMSGroupsDTO data object class. This acts as data holder for the Menus Items business object
    /// </summary>
    public class CMSGroupsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected int groupId;
        protected int parentGroupId;
        protected string name;
        protected string imageFileName;
        protected bool active;
        protected string guid;
        protected bool synchStatus;
        protected int site_id;
        protected string createdBy;
        protected DateTime creationDate;
        protected string lastUpdatedBy;
        protected DateTime lastUpdatedDate;
        protected string groupUrl;
        protected int masterEntityId;
        protected bool notifyingObjectIsChanged;
        protected readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSGroupsDTO()
        {
            this.groupId = -1;
            this.parentGroupId = -1;
            this.name = string.Empty;
            this.imageFileName = string.Empty;
            this.site_id = -1;
            this.masterEntityId = -1;
            this.groupUrl = string.Empty;
        }


        /// <summary>
        /// Constructor with Required the data fields
        /// </summary> 
        public CMSGroupsDTO(int groupId, int parentGroupId, string name, string imageFileName, string groupUrl,bool active)
            :this()
        {
            log.LogMethodEntry(groupId, parentGroupId, name, imageFileName, groupUrl, active);
            this.groupId = groupId;
            this.parentGroupId = parentGroupId;
            this.name = name;
            this.imageFileName = imageFileName;
            this.groupUrl = groupUrl;
            this.active = active;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary> 
        public CMSGroupsDTO(int groupId, int parentGroupId, string name, string imageFileName, string groupUrl,
                                bool active, string guid, bool synchStatus, int site_id, string createdBy,
                                DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int masterEntityId)
            :this(groupId, parentGroupId, name, imageFileName, groupUrl, active)
        {
            log.LogMethodEntry(groupId, parentGroupId, name, imageFileName, groupUrl, active, guid, synchStatus, site_id, 
                               createdBy,creationDate, lastUpdatedBy, lastUpdatedDate, masterEntityId);
            
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by GROUP ID field
            /// </summary>
            GROUP_ID,
            /// <summary>
            /// Search by NAME  field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by PARENT GROUP ID field
            /// </summary>
            PARENT_GROUP_ID ,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,

        }

        /// <summary>
        /// Get/Set method of the GroupId field
        /// </summary>
        [DisplayName("GroupId")]
        [DefaultValue(-1)]
        public int GroupId { get { return groupId; } set { groupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParentGroupId field
        /// </summary>
        [DisplayName("ParentGroupId")]
        [DefaultValue(-1)]
        public int ParentGroupId { get { return parentGroupId; } set { parentGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ImageFileName field
        /// </summary>
        [DisplayName("ImageFileName")]
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the GroupUrl field
        /// </summary>
        [DisplayName("GroupUrl")]
        public string GroupUrl { get { return groupUrl; } set { groupUrl = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate { get { return creationDate; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
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
                    return notifyingObjectIsChanged;
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
    /// <summary>
    /// This is the  CMSGroupsDTOTree data object class. This acts as data holder for the CMSGroupsDTO  Items business object
    /// </summary>
    public class CMSGroupsDTOTree : CMSGroupsDTO
    {
        private List<CMSGroupsDTO> subGroup;
        private int count;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSGroupsDTOTree()
            : base()
        {
            subGroup = null;
            count = 0;
        }

        /// <summary>
        /// Constructor with Parent class data and Child  data fields
        /// </summary> 
        public CMSGroupsDTOTree(CMSGroupsDTO cmsGroupsDTO, List<CMSGroupsDTO> subGroup, int count)
        {
            this.groupId = cmsGroupsDTO.GroupId;
            this.parentGroupId = cmsGroupsDTO.ParentGroupId;
            this.name = cmsGroupsDTO.Name;
            this.imageFileName = cmsGroupsDTO.ImageFileName;
            this.groupUrl = cmsGroupsDTO.GroupUrl;
            this.active = cmsGroupsDTO.Active;
            this.guid = cmsGroupsDTO.Guid;
            this.synchStatus = cmsGroupsDTO.SynchStatus;
            this.site_id = cmsGroupsDTO.Site_id;
            this.createdBy = cmsGroupsDTO.CreatedBy;
            this.creationDate = cmsGroupsDTO.CreationDate;
            this.lastUpdatedBy = cmsGroupsDTO.LastUpdatedBy;
            this.lastUpdatedDate = cmsGroupsDTO.LastupdatedDate;
            this.masterEntityId = cmsGroupsDTO.MasterEntityId;
            this.subGroup = subGroup;
            this.count = count;
        }

        /// <summary>
        /// Get/Set method of the SubGroup List<CMSGroupsDTO> 
        /// </summary>
        [DisplayName("SubGroup")]
        public List<CMSGroupsDTO> SubGroup { get { return subGroup; } set { subGroup = value; } }

        /// <summary>
        /// Get/Set method of the Count field
        /// </summary>
        [DisplayName("Count")]
        public int Count { get { return count; } set { count = value; } }
    }
}

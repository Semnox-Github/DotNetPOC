/********************************************************************************************
 * Project Name - AgentGroups Programs 
 * Description  - Data object of the AgentGroups
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        08-June-2016   Rakshith           Created 
 *2.70        13-June-2019   Jagan Mohana       Added siteId search parameter
 *2.70.2        15-Jul -2019   Girish Kundar      Modified : Added Parametrized Constructor with required fields
 *                                                        And MasterEntityId field.
 *2.80        11-Jun-2020   Mushahid Faizan      Modified : 3 Tier Changes For Rest API., Added IsActive Column
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the AgentGroupsDTO data object class. This acts as data holder for the AgentGroupsDTO business object
    /// </summary>
    public class AgentGroupsDTO : IChangeTracking
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected int agentGroupId;
        protected string groupName;
        protected string remarks;
        protected int site_id;
        protected string guid;
        protected bool synchStatus;
        protected int partnerId;
        protected string createdBy;
        protected DateTime creationDate;
        protected string lastUpdatedUser;
        protected DateTime lastUpdatedDate;
        private int masterEntityId;
        protected bool isActive;
        private List<AgentGroupAgentsDTO> agentGroupAgentsDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AgentGroupsDTO()
        {
            log.LogMethodEntry();
            this.agentGroupId = -1;
            this.groupName = string.Empty;
            this.remarks = string.Empty;
            this.site_id = -1;
            this.partnerId = -1;
            masterEntityId = -1;
            this.isActive = true;
            agentGroupAgentsDTO = new List<AgentGroupAgentsDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor With Required Parameter
        /// </summary>
        public AgentGroupsDTO(int agentGroupId, string groupName, string remarks, int partnerId, bool isActive)
             : this()
        {
            log.LogMethodEntry(agentGroupId, groupName, remarks, partnerId, isActive);
            this.agentGroupId = agentGroupId;
            this.groupName = groupName;
            this.partnerId = partnerId;
            this.remarks = remarks;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor With Parameter
        /// </summary>
        public AgentGroupsDTO(int agentGroupId, string groupName, string remarks, int site_id, string guid, bool synchStatus,
                                 int partnerId, string createdBy, DateTime creationDate, string lastUpdatedUser,
                                 DateTime lastUpdatedDate, int masterEntityId, bool isActive)
             : this(agentGroupId, groupName, remarks, partnerId, isActive)
        {
            log.LogMethodEntry(agentGroupId, groupName, remarks, site_id, guid, synchStatus,
                                 partnerId, createdBy, creationDate, lastUpdatedUser,
                                 lastUpdatedDate, masterEntityId);

            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedUser = lastUpdatedUser;
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
            /// Search by AGENT_GROUP_ID field
            /// </summary>
            AGENT_GROUP_ID,
            /// <summary>
            /// Search by PARTNER_ID field
            /// </summary>
            PARTNER_ID,
            /// <summary>
            /// Search by GROUP_NAME field
            /// </summary>
            GROUP_NAME,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE
        }


        /// <summary>
        /// Get/Set method of the AgentGroupId field
        /// </summary>
        [DisplayName("AgentGroupId")]
        public int AgentGroupId { get { return agentGroupId; } set { agentGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PartnerIds field
        /// </summary>
        [DisplayName("PartnerId")]
        public int PartnerId { get { return partnerId; } set { partnerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GroupName field
        /// </summary>
        [DisplayName("GroupName")]
        public string GroupName { get { return groupName; } set { groupName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the GroupName field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site id ")]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid ")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

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
        [DisplayName("LastUpdatedUser")]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AgentGroupAgentsDTOList field
        /// </summary>
        public List<AgentGroupAgentsDTO> AgentGroupAgentsDTOList { get { return agentGroupAgentsDTO; } set { agentGroupAgentsDTO = value; } }

        /// <summary>
        /// Returns whether the AgentGroupDTO changed or any of its agentGroupAgentsDTO children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (agentGroupAgentsDTO != null &&
                   agentGroupAgentsDTO.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || agentGroupId < 0;
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

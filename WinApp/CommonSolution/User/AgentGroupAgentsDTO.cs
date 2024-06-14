/********************************************************************************************
 * Project Name - AgentGroupAgentsDTO Programs 
 * Description  - Data object of the AgentGroupAgentsDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        08-June-2016   Rakshith           Created 
 *2.70.2       15-Jul -2019   Girish Kundar      Modified : Added Parametrized Constructor with required fields.
 *                                                        And MasterEntityId field.
 *2.80        11-Jun-2020   Mushahid Faizan      Modified : 3 Tier Changes For Rest API., Added IsActive Column
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    public class AgentGroupAgentsDTO : IChangeTracking
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected int id;
        protected int agentGroupId;
        protected int agentId;
        protected string guid;
        protected int site_id;
        protected bool synchStatus;
        protected string createdBy;
        protected DateTime creationDate;
        protected string lastUpdatedUser;
        protected DateTime lastUpdatedDate;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private bool isActive;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AgentGroupAgentsDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.agentGroupId = -1;
            this.agentId = -1;
            this.site_id = -1;
            masterEntityId = -1;
            this.isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor With Required Parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="agentGroupId">agentGroupId</param>
        /// <param name="agentId">agentId</param>
        public AgentGroupAgentsDTO(int id, int agentGroupId, int agentId, bool isActive)
          : this()
        {
            log.LogMethodEntry(id, agentGroupId, agentId, isActive);
            this.id = id;
            this.agentGroupId = agentGroupId;
            this.agentId = agentId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor With all the Fields
        /// </summary>
        public AgentGroupAgentsDTO(int id, int agentGroupId, int agentId, string guid, int site_id, bool synchStatus,
                                    string createdBy, DateTime creationDate, string lastUpdatedUser, DateTime lastUpdatedDate, int masterEntityId, bool isActive)
            : this(id, agentGroupId, agentId, isActive)
        {
            log.LogMethodEntry(id, agentGroupId, agentId, guid, site_id, synchStatus,
                                createdBy, creationDate, lastUpdatedUser, lastUpdatedDate, masterEntityId);
            this.guid = guid;
            this.site_id = site_id;
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
            /// Search by AGENT GROUP ID field
            /// </summary>
            AGENT_GROUP_ID,
            /// <summary>
            /// Search by AGENT GROUP ID LIST field
            /// </summary>
            AGENT_GROUP_ID_LIST,
            /// <summary>
            /// Search by AGENT ID field
            /// </summary>
            AGENT_ID,
            /// <summary>
            /// Search by AGENT ID LIST field
            /// </summary>
            AGENT_ID_LIST,
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
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
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AgentGroupId field
        /// </summary>
        [DisplayName("AgentGroupId")]
        public int AgentGroupId { get { return agentGroupId; } set { agentGroupId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the AgentGroupId field
        /// </summary>
        [DisplayName("AgentId")]
        public int AgentId { get { return agentId; } set { agentId = value; this.IsChanged = true; } }

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

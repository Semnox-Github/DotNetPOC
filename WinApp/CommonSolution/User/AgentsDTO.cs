/********************************************************************************************
 * Project Name - Agents DTO Programs 
 * Description  - Data object of the Agents DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        10-May-2016   Rakshith           Created 
 *2.70.2       15-Jul -2019   Girish Kundar      Modified : Added Parametrized Constructor with required fields
 *                                                        And MasterEntityId field.
 *2.90        11-Jun-2020   Mushahid Faizan     Modified : 3 Tier Changes for Rest API.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the AgentsDTO data object class. This acts as data holder for the AgentsDTO business object
    /// </summary>
    public class AgentsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int agentId;
        private int partnerId;
        private int userId;
        private string mobileNo;
        private double commission;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedUser;
        private DateTime lastUpdatedDate;
        private bool active;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AgentsDTO()
        {
            log.LogMethodEntry();
            this.agentId = -1;
            this.partnerId = -1;
            this.commission = 0;
            this.userId = -1;
            this.siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor With Required Parameter
        /// </summary>
        public AgentsDTO(int agentId, int partnerId, int userId, string mobileNo, double commission, bool active)
            : this()
        {
            log.LogMethodEntry(agentId, partnerId, userId, mobileNo, commission, active);
            this.agentId = agentId;
            this.partnerId = partnerId;
            this.userId = userId;
            this.mobileNo = mobileNo;
            this.commission = commission;
            this.active = active;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor With all the  Parameter
        /// </summary>
        public AgentsDTO(int agentId, int partnerId, int userId, string mobileNo, double commission, string createdBy,
                              DateTime creationDate, string lastUpdatedUser, DateTime lastUpdatedDate, bool active,
                              int site_id, string guid, bool synchStatus, int masterEntityId)
            : this(agentId, partnerId, userId, mobileNo, commission, active)
        {
            log.LogMethodEntry(agentId, partnerId, userId, mobileNo, commission, createdBy,
                               creationDate, lastUpdatedUser, lastUpdatedDate, active,
                               site_id, guid, synchStatus, masterEntityId);

            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PARTNER ID field
            /// </summary>
            PARTNER_ID,
            /// <summary>
            /// Search by USER ID field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by AGENT ID field
            /// </summary>
            AGENT_ID,
            /// <summary>
            /// Search by LOGIN_ID field
            /// </summary>
            LOGINID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }

        /// <summary>
        /// Get/Set method of the AgentId field
        /// </summary>
        [DisplayName("AgentId")]
        public int AgentId { get { return agentId; } set { agentId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PartnerId field
        /// </summary>
        [DisplayName("Partner Id")]
        public int PartnerId { get { return partnerId; } set { partnerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the User_Id field
        /// </summary>
        [DisplayName("User_Id")]
        public int User_Id { get { return userId; } set { userId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MobileNo field
        /// </summary>
        [DisplayName("MobileNo")]
        public string MobileNo { get { return mobileNo; } set { mobileNo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Commission field
        /// </summary>
        [DisplayName("Commission")]
        public double Commission { get { return commission; } set { commission = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

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
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site id ")]
        public int Site_id { get { return siteId; } set { siteId = value; } }

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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
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
                    return notifyingObjectIsChanged || agentId < 0;
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
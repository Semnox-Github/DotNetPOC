/********************************************************************************************
 * Project Name - State DTO
 * Description  - Data object of State
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor
 *            29-Jul-2019   Mushahid Faizan   Added IsActive and LogMethodEntry/Exit    
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    public class StateDTO
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
            /// Search by STATE ID field
            /// </summary>
            STATE_ID,

            /// <summary>
            /// Search by STATE field
            /// </summary>
            STATE,

            /// <summary>
            /// Search by STATE DESCRIPTION field
            /// </summary>
            STATE_DESCRIPTION,

            /// <summary>
            /// Search by COUNTRY ID field
            /// </summary>
            COUNTRY_ID,
            
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,            
            /// <summary>
            /// Search by IsActive Field
            /// </summary>
            ISACTIVE

        }
        private int stateId;
        private string state;
        private string description;
        private int countryId;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool isActive;

        /// <summary>
        /// Default constructor
        /// </summary>
        public StateDTO()
        {
            log.LogMethodEntry();
            stateId =  -1;
            state = "";
            description = "";
            countryId =  -1;
            siteId = -1;
            synchStatus = false;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with required data fields
        /// </summary>
        public StateDTO(int stateId, string state, string description, int countryId)
            :this()
        {
            log.LogMethodEntry(stateId, state, description, countryId);
            this.stateId = stateId;
            this.state = state;
            this.description = description;
            this.countryId = countryId;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        public StateDTO(int stateId, string state, string description, int countryId, int siteId, string guid, bool synchStatus, int masterEntityId, 
                          string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            :this(stateId, state, description, countryId)
        {
            log.LogMethodEntry(siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
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
        /// Get/Set method of the StateId field
        /// </summary>
        [DisplayName("StateId")]
        public int StateId { get { return stateId; } set { stateId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the State field
        /// </summary>
        [DisplayName("State")]
        public string State { get { return state; } set { state = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CountryId field
        /// </summary>
        [DisplayName("CountryId")]
        public int CountryId { get { return countryId; } set { countryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value;  } }

        /// <summary>
        /// Get/Set method IsActive
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        
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
                    return notifyingObjectIsChanged || stateId < 0;
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

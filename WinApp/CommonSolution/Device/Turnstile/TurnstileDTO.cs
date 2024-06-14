/********************************************************************************************
 * Project Name - Turnstile DTO
 * Description  - Data object of Turnstile
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        10-Jul-2019   Girish kundar            Modified : Added constructor for required fields .
 *                                                              Added Missed Who columns. 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.Turnstile
{
    public class TurnstileDTO
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
            /// Search by TURNSTILE_ID field
            /// </summary>
            TURNSTILE_ID,
            /// <summary>
            /// Search by TURNSTILE_NAME field
            /// </summary>
            TURNSTILE_NAME,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by TYPE field
            /// </summary>
            TYPE,
            /// <summary>
            /// Search by MAKE field
            /// </summary>
            MAKE,
            /// <summary>
            /// Search by MODEL field
            /// </summary>
            MODEL,
            /// <summary>
            /// Search by GAME_PROFILE_ID field
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID

        }

        private int turnstileId;
        private string turnstileName;
        private string passageAAlias;
        private string passageBAlias;
        private string iPAddress;
        private int? portNumber;
        private bool useRSProtocol;
        private bool active;
        private int type;
        private int make;
        private int model;
        private int gameProfileId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;

        private TurnstileClass turnstileClass;


        /// <summary>
        /// Default constructor
        /// </summary>
        public TurnstileDTO()
        {
            log.LogMethodEntry();
            turnstileId = -1;
            turnstileName = string.Empty;
            passageAAlias = string.Empty;
            passageBAlias = string.Empty;
            iPAddress = string.Empty;
            //portNumber = -1;
            useRSProtocol = false;
            active = true;
            type = -1;
            make = -1;
            model = -1;
            gameProfileId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized Constructor  with required parameters
        /// </summary>
        public TurnstileDTO(int turnstileId, string turnstileName, string passageAAlias, string passageBAlias, string iPAddress, int? portNumber, bool useRSProtocol, bool active, int type,
                             int make, int model, int gameProfileId)
            : this()
        {
            log.LogMethodEntry(turnstileId, turnstileName, passageAAlias, passageBAlias, iPAddress, portNumber,
                               useRSProtocol, active, type, make, model, gameProfileId);
            this.turnstileId = turnstileId;
            this.turnstileName = turnstileName;
            this.passageAAlias = passageAAlias;
            this.passageBAlias = passageBAlias;
            this.iPAddress = iPAddress;
            this.portNumber = portNumber;
            this.useRSProtocol = useRSProtocol;
            this.active = active;
            this.type = type;
            this.make = make;
            this.model = model;
            this.gameProfileId = gameProfileId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor  with All the  parameters
        /// </summary>
        public TurnstileDTO(int turnstileId, string turnstileName, string passageAAlias, string passageBAlias,
                             string iPAddress, int? portNumber, bool useRSProtocol, bool active, int type,
                             int make, int model, int gameProfileId, DateTime lastUpdatedDate, string lastUpdatedUser,
                             string guid, bool synchStatus, int masterEntityId, int siteId,
                             string createdBy, DateTime creationDate)
            : this(turnstileId, turnstileName, passageAAlias, passageBAlias, iPAddress, portNumber,
                               useRSProtocol, active, type, make, model, gameProfileId)
        {
            log.LogMethodEntry(turnstileId, turnstileName, passageAAlias, passageBAlias,
                              iPAddress, portNumber, useRSProtocol, active, type,
                              make, model, gameProfileId, lastUpdatedDate, lastUpdatedUser,
                              guid, synchStatus, masterEntityId, siteId,
                              createdBy, creationDate);

            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();

        }

        /// <summary>
        /// Get/Set method of the TurnstileId field
        /// </summary>
        [DisplayName("TurnstileId")]
        public int TurnstileId { get { return turnstileId; } set { turnstileId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TurnstileName field
        /// </summary>
        [DisplayName("TurnstileName")]
        public string TurnstileName { get { return turnstileName; } set { turnstileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PassageAAlias field
        /// </summary>
        [DisplayName("PassageAAlias")]
        public string PassageAAlias { get { return passageAAlias; } set { passageAAlias = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PassageBAlias field
        /// </summary>
        [DisplayName("Passage B Alias")]
        public string PassageBAlias { get { return passageBAlias; } set { passageBAlias = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IPAddress field
        /// </summary>
        [DisplayName("IP Address")]
        public string IPAddress { get { return iPAddress; } set { iPAddress = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PortNumber field
        /// </summary>
        [DisplayName("Port Number")]
        public int? PortNumber { get { return portNumber; } set { portNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UseRSProtocol field
        /// </summary>
        [DisplayName("Use RS Protocol")]
        public bool UseRSProtocol { get { return useRSProtocol; } set { useRSProtocol = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public int Type { get { return type; } set { type = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Make field
        /// </summary>
        [DisplayName("Make")]
        public int Make { get { return make; } set { make = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Model field
        /// </summary>
        [DisplayName("Model")]
        public int Model { get { return model; } set { model = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GameProfileId field
        /// </summary>
        [DisplayName("Game Profile")]
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedUser")]
        [Browsable(false)]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; ; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set Methods for TurnStileClass field
        /// </summary>
        public TurnstileClass TurnstileClass
        {
            get
            {
                return turnstileClass;
            }
            set
            {
                turnstileClass = value;
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
                    return notifyingObjectIsChanged || turnstileId < 0;
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

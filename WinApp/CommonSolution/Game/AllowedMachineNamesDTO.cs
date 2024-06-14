/********************************************************************************************
 * Project Name - Semnox.Parafait.Game - AllowedMachineNamesDTO
 * Description  - AllowedMachineNamesDTO data transfer object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.160.0    02-Feb-2023       Roshan Devadiga        Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Game
{
    public class AllowedMachineNamesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by  ALLOWED_MACHINE_ID
            /// </summary>
            ALLOWED_MACHINE_ID,
            /// <summary>
            /// Search by GAME_ID
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by MACHINE_NAME
            /// </summary>
            MACHINE_NAME,
            /// <summary>
            /// Search by IS ACTIVE
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
        }
        private int allowedMachineId;
        private int gameId;
        private string machineName;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AllowedMachineNamesDTO()
        {
            log.LogMethodEntry();
            allowedMachineId = -1;
            gameId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public AllowedMachineNamesDTO(int allowedMachineId, int gameId, string machineName,bool isActive)
            : this()
        {
            log.LogMethodEntry(allowedMachineId, gameId, machineName, isActive);
            this.allowedMachineId = allowedMachineId;
            this.gameId = gameId;
            this.machineName = machineName;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AllowedMachineNamesDTO(int allowedMachineId, int gameId, string machineName, bool isActive, string guid, int siteId, bool synchStatus, int masterEntityId,
                                        string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(allowedMachineId, gameId, machineName, isActive)
        {
            log.LogMethodEntry(allowedMachineId, gameId, machineName, isActive, guid, siteId,
                                synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int AllowedMachineId { get { return allowedMachineId; } set { this.IsChanged = true; allowedMachineId = value; } }

        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        public int GameId { get { return gameId; } set { this.IsChanged = true; gameId = value; } }

        /// <summary>
        /// Get/Set method of the MachineName field
        /// </summary>
        public string MachineName { get { return machineName; } set { this.IsChanged = true; machineName = value; } }

      
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || allowedMachineId < 0;
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

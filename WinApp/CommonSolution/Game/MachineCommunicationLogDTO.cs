/********************************************************************************************
 * Project Name - MachineCommunicationLogDTO
 * Description  - Data object of MachineCommunicationLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        26-Mar-2019   Indhu          Created 
 *2.70.2        29-Jul-2019   Deeksha        Modified:Added a new constructor with required fields.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// MachineCommunicationLogDTO
    /// </summary>
    public class MachineCommunicationLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By MACHINE_ID
            /// </summary>
            MACHINE_ID,

            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID,
        }

        private int machinesCommunicationLogId;
        private int machineId;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private bool isActive;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private double communicationSuccessRatio;
        private DateTime lastServerCommunicatedTime;


        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineCommunicationLogDTO()
        {
            log.LogMethodEntry();
            machinesCommunicationLogId = -1;
            machineId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            communicationSuccessRatio = 0;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with requiered data fields
        /// </summary>
        public MachineCommunicationLogDTO(int machinesCommunicationLogId, int machineId,
                                          double communicationSuccessRatio, bool isActive, DateTime lastServerCommunicatedTime)
            :this()
        {
            log.LogMethodEntry( machinesCommunicationLogId,  machineId, communicationSuccessRatio, isActive, lastServerCommunicatedTime);
            this.machinesCommunicationLogId = machinesCommunicationLogId;
            this.machineId = machineId;           
            this.isActive = isActive;            
            this.communicationSuccessRatio = communicationSuccessRatio;            
            this.lastServerCommunicatedTime = lastServerCommunicatedTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MachineCommunicationLogDTO(int machinesCommunicationLogId, int machineId,
                                          double communicationSuccessRatio, bool isActive, string createdBy, DateTime creationDate,
                                          string lastUpdatedBy, DateTime lastUpdateDate, 
                                          int siteId, int masterEntityId,bool synchStatus, 
                                          string guid,DateTime lastServerCommunicatedTime)
            :this(machinesCommunicationLogId, machineId, communicationSuccessRatio, isActive, lastServerCommunicatedTime)
        {
            log.LogMethodEntry(machinesCommunicationLogId, machineId, communicationSuccessRatio, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid, lastServerCommunicatedTime);
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method for MachinesCommunicationLogId 
        /// </summary>
        public int MachinesCommunicationLogId { get { return machinesCommunicationLogId; } set { machinesCommunicationLogId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method for MachineId
        /// </summary>
        public int MachineId { get { return machineId; } set { machineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method for LastUpdateDate
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        /// <summary>
        /// Get/Set method for LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }
        /// <summary>
        /// Get/Set method for IsActive
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method for creationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method for CreatedBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;} }
        /// <summary>
        /// Get/Set method for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method for SiteId
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method for SyncStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;} }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }
        /// <summary>
        /// Get/Set method for CommunicationSuccessRatio 
        /// </summary>
        public double CommunicationSuccessRatio { get { return communicationSuccessRatio; } set { communicationSuccessRatio = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method for LastServerCommunicatedTime 
        /// </summary>
        public DateTime LastServerCommunicatedTime { get { return lastServerCommunicatedTime; } set { lastServerCommunicatedTime = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || machinesCommunicationLogId < 0;
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

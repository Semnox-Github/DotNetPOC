/********************************************************************************************
 * Project Name - CommunicationLogDTO
 * Description  - Data object of CommunicationLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        29-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// This is the CommunicationLogDTO data object class. This acts as data holder for the CommunicationLog business object
    /// </summary>
    public class CommunicationLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by  MASTER ADDRESS field
            /// </summary>
            MASTER_ADDRESS,
            /// <summary>
            /// Search by  MACHINE ADDRESS field
            /// </summary>
            MACHINE_ADDRESS,
            /// <summary>
            /// Search by  MACHINE ID field
            /// </summary>
            MACHINE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private string masterAddress;
        private string machineAddress;
        private string receivedData;
        private string lastSentData;
        private DateTime? timestamp;
        private int machineId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CommunicationLogDTO()
        {
            log.LogMethodEntry();
            machineId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public CommunicationLogDTO(string masterAddress,string machineAddress,string receivedData,string lastSentData, DateTime? timestamp,int machineId,DateTime lastUpdatedDate,
                                   string lastUpdatedBy,string guid, int siteId, bool synchStatus, int masterEntityId,string createdBy, DateTime creationDate )
        {
            log.LogMethodEntry(masterAddress, machineAddress, receivedData, lastSentData, timestamp, machineId, lastUpdatedDate,
                                   lastUpdatedBy, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate);
            this.masterAddress = masterAddress;
            this.machineAddress = machineAddress;
            this.receivedData = receivedData;
            this.lastSentData = lastSentData;
            this.timestamp = timestamp;
            this.machineId = machineId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MasterAddress  field
        /// </summary>
        public string MasterAddress
        {
            get { return masterAddress; }
            set { masterAddress = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MachineAddress  field
        /// </summary>
        public string MachineAddress
        {
            get { return machineAddress; }
            set { machineAddress = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ReceivedData  field
        /// </summary>
        public string ReceivedData
        {
            get { return receivedData; }
            set { receivedData = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastSentData  field
        /// </summary>
        public string LastSentData
        {
            get { return lastSentData; }
            set { lastSentData = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Timestamp  field
        /// </summary>
        public DateTime? Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MachineId  field
        /// </summary>
        public int MachineId
        {
            get { return machineId; }
            set { machineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}

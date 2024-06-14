/********************************************************************************************
 * Project Name - User
 * Description  - Data object of AttendanceReader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        27-May-2019   Girish Kundar           Created 
 *2.80        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the AttendanceReaderDTO data object class. This acts as data holder for the AttendanceReader business object
    /// </summary>
    public class AttendanceReaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by   NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by TYPE  field
            /// </summary>
            TYPE,
            /// <summary>
            /// Search by IP ADDRESS field
            /// </summary>
            IP_ADDRESS,
            /// <summary>
            /// Search by MACHINE NUMBER field
            /// </summary>
            MACHINE_NUMBER,
            /// <summary>
            /// Search by  ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int id;
        private string name;
        private string type;
        private string ipAddress;
        private string portNumber;
        private string serialNumber;
        private int? machineNumber;
        private bool activeFlag;
        private string guid;
        private int siteId;
        private DateTime? lastSynchTime;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AttendanceReaderDTO()
        {
            log.LogMethodEntry();
            id = -1;
            siteId = -1;
            activeFlag = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public AttendanceReaderDTO( int id,string name,string type,string ipAddress, string portNumber,string serialNumber,
                                    int? machineNumber, bool activeFlag, DateTime? lastSynchTime) 
            : this()
        {
            log.LogMethodEntry(id, name, type, ipAddress, portNumber, serialNumber, machineNumber, activeFlag, 
                                guid, siteId, lastSynchTime, synchStatus, masterEntityId);
            this.id = id;
            this.name = name;
            this.type = type;
            this.ipAddress = ipAddress;
            this.portNumber = portNumber;
            this.serialNumber = serialNumber;
            this.machineNumber = machineNumber;
            this.activeFlag = activeFlag;
            this.lastSynchTime = lastSynchTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public AttendanceReaderDTO( int id,string name,string type,string ipAddress, string portNumber,string serialNumber,
                                    int? machineNumber, bool activeFlag,string guid, int siteId,DateTime? lastSynchTime, bool synchStatus, int masterEntityId,
                                    DateTime lastUpdatedDate, string lastUpdatedBy,string createdBy, DateTime creationDate )
            : this(id, name, type, ipAddress, portNumber, serialNumber, machineNumber, activeFlag, lastSynchTime)
        {
            log.LogMethodEntry(id, name, type, ipAddress, portNumber, serialNumber, machineNumber, activeFlag, guid, siteId, lastSynchTime, synchStatus, masterEntityId,
                                    lastUpdatedDate, lastUpdatedBy, createdBy, creationDate );
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
        /// Get/Set method of the Id  field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the IPAddress field
        /// </summary>
        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PortNumber field
        /// </summary>
        public string PortNumber
        {
            get { return portNumber; }
            set { portNumber = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SerialNumber field
        /// </summary>
        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MachineNumber field
        /// </summary>
        public int? MachineNumber
        {
            get { return machineNumber; }
            set { machineNumber = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastSynchTime field
        /// </summary>
        public DateTime? LastSynchTime
        {
            get { return lastSynchTime; }
            set { lastSynchTime = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
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
            set { synchStatus = value; }
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
        /// Get/Set method to track changes to the object
        /// </summary>
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
            IsChanged = false;
            log.LogMethodExit();
        }

    }
}

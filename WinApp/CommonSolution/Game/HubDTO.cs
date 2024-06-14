/********************************************************************************************
 * Project Name - Hub Data dto                                                                          
 * Description  - Dto of the Hub class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 ********************************************************************************************* 
 *2.50.0      12-dec-2018   Guru S A         Who column changes
 *2.50        26-Feb-2019   Indrajeet Kumar  Added Attributes
 *2.70        10-Jul-2019   Girish kundar    Added :Added constructor for required fields .
 *                                                  Added IsEBYTE field. 
 *2.70.2       01-Aug-2019   Deeksha          Added setter method for who fields.
 *2.70.3       21-Dec-2019   Archana          Added machineCount field and hubNameWithMachineCount get property
 *2.110.0      12-Feb-2021   Girish Kundar     Added new field for Radian Wristband (IsRadian) 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// HubDTO
    /// </summary>
    public class HubDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        /// SearchByHubParameters
        /// </summary>
        public enum SearchByHubParameters
        {
            /// <summary>
            /// HUB_ID search field
            /// </summary>
            HUB_ID ,
            /// <summary>
            /// HUB_NAME search field
            /// </summary>
            HUB_NAME,
            /// <summary>
            /// ACTIVE_FLAG search field
            /// </summary>
            IS_ACTIVE,
            // <summary>
            /// SITE_ID search field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            ///IS EBYTE search field
            /// </summary>
            IS_EBYTE,
            /// <summary>
            ///IS RADIAN search field
            /// </summary>
            IS_RADIAN,
            /// <summary>
            ///IS RESTART_AP search field
            /// </summary>
            RESTART_AP
        }

       private int masterId;
       private string masterName;
       private int? portNumber;
       private int baudRate;
       private string notes;
       private bool isActive;
       private string address;
       private string frequency;
       private string serverMachine;
       private string directMode;
       private string guid;
       private int siteId;
       private string ipAddress;
       private int tcpPort;
       private string macAddress;
       private bool synchStatus;
       private bool restartAP;
       private int masterEntityId;
       private string createdBy;
       private DateTime creationDate;
       private string lastupdatedBy;
       private DateTime lastupdateDate;
       private int dataRate; 
       private string registerId;
       private string registervalue; 
       private string configureType; 
       private bool isEByte;
        private bool isRadian;
        private EBYTEDTO ebyteDTO;
       private int machineCount;       

        /// <summary>
        /// Default constructor
        /// </summary>
        public HubDTO()
        {
            log.LogMethodEntry();
            masterId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            isEByte = false;
            isRadian = false;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required the data fields
        /// </summary>
        public HubDTO(int masterId, string masterName, int? portNumber, int baudRate, string notes,
                          bool isActive, string address, string frequency, string serverMachine, string directMode,
                          string ipAddress, int tcpPort, string macAddress, bool restartAP, bool isEBYTE, bool isRadian)
            : this()
        {
            log.LogMethodEntry(masterId, masterName, portNumber, baudRate, notes, isActive, address,
                               frequency, serverMachine, directMode, ipAddress, tcpPort, macAddress,
                               restartAP, isEBYTE, isRadian);
            this.masterId = masterId;
            this.masterName = masterName;
            this.portNumber = portNumber;
            this.baudRate = baudRate;
            this.notes = notes;
            this.isActive = isActive;
            this.address = address;
            this.frequency = frequency;
            this.serverMachine = serverMachine;
            this.directMode = directMode;
            this.ipAddress = ipAddress;
            this.tcpPort = tcpPort;
            this.macAddress = macAddress;
            this.restartAP = restartAP;
            this.isEByte = isEBYTE;
            this.isRadian = isRadian;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public HubDTO(int masterId, string masterName, int? portNumber, int baudRate, string notes,
                          bool isActive, string address, string frequency, string serverMachine, string directMode,
                          string guid, int siteId, string ipAddress, int tcpPort, string macAddress,
                          bool synchStatus, bool restartAP, int masterEntityId, string createdBy, DateTime creationDate,
                          string lastupdatedBy, DateTime lastupdateDate, bool isEByte, bool isRadian)
            : this(masterId, masterName, portNumber, baudRate, notes, isActive, address,
                               frequency, serverMachine, directMode, ipAddress, tcpPort, macAddress,
                               restartAP, isEByte, isRadian)
        {
            log.LogMethodEntry(masterId, masterName, portNumber, baudRate, notes, isActive, address, frequency,
                              serverMachine, directMode, guid, siteId, ipAddress, tcpPort, macAddress,
                              synchStatus, restartAP, masterEntityId, createdBy, creationDate, lastupdatedBy,
                              lastupdateDate, this.isEByte, this.isRadian);

            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastupdatedBy = lastupdatedBy;
            this.lastupdateDate = lastupdateDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set for MasterId
        /// </summary>
        public int MasterId { get { return masterId; } set { masterId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for MasterName
        /// </summary>
        public string MasterName { get { return masterName; } set { masterName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for PORTNumber
        /// </summary>
        public int? PortNumber { get { return portNumber; } set { portNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for baudRate
        /// </summary>
        public int BaudRate { get { return baudRate; } set { baudRate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for Notes
        /// </summary>
        public string Notes { get { return notes; } set { notes = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for IsActive
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for Address
        /// </summary>
        public string Address { get { return address; } set { address = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for Frequency
        /// </summary>
        public string Frequency { get { return frequency; } set { frequency = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for ServerMachine
        /// </summary>
        public string ServerMachine { get { return serverMachine; } set { serverMachine = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for DirectMode
        /// </summary>
        public string DirectMode { get { return directMode; } set { directMode = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set for SiteId
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value;  } }
        /// <summary>
        /// Get/Set for IPAddress
        /// </summary>
        public string IPAddress { get { return ipAddress; } set { ipAddress = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for TCPPort
        /// </summary>
        public int TCPPort { get { return tcpPort; } set { tcpPort = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for MACAddress
        /// </summary>
        public string MACAddress { get { return macAddress; } set { macAddress = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for SyncStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }
        /// <summary>
        /// Get/Set for RestartAP
        /// </summary>
        public bool RestartAP { get { return restartAP; } set { restartAP = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for CreatedBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set for CreationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set for lastupdatedBy
        /// </summary>
        public string LastupdatedBy { get { return lastupdatedBy; } set { lastupdatedBy = value; } }

        /// <summary>
        /// Get/Set for lastupdateDate
        /// </summary>
        public DateTime LastupdateDate { get { return lastupdateDate; } set { lastupdateDate = value; } }

        /// <summary>
        /// Get/Set for DataRate
        /// </summary>
        public int DataRate { get { return dataRate; } set { dataRate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for RegisterId
        /// </summary>
        public string RegisterId { get { return registerId; } set { registerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for RegisterValue
        /// </summary>
        public string RegisterValue { get { return registervalue; } set { registervalue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for ConfigureType
        /// </summary>
        public string ConfigureHubType { get { return configureType; } set { configureType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for IsEByte
        /// </summary>
        public bool IsEByte { get { return isEByte; } set { isEByte = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for IsRadian
        /// </summary>
        public bool IsRadian { get { return isRadian; } set { isRadian = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for EBYTEHubDTO
        /// </summary>
        public EBYTEDTO EBYTEDTO { get { return ebyteDTO; } set { ebyteDTO = value; } }
        
        /// <summary>
        /// Get/Set for MachineCount
        /// </summary>
        public int MachineCount { get { return machineCount; } set { machineCount = value; } }
        /// <summary>
        /// Get/Set for HubNameWithMachineCount
        /// </summary>
        public string HubNameWithMachineCount
        { get
            {
                return ((string.IsNullOrEmpty(MasterName) || string.IsNullOrWhiteSpace(MasterName)) ?  " " : MasterName + "[MachineCount:" + MachineCount + "]");
            }
          set { }
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
                    return notifyingObjectIsChanged || masterId < 0;
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

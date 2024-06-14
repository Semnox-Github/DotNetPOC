/********************************************************************************************
* Project Name - Game
* Description  - DTO for Hub Container Class.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     09-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public class HubContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int masterId;
        private string masterName;
        private int? portNumber;
        private int baudRate;
        private string notes;
        private string address;
        private string frequency;
        private string serverMachine;
        private string directMode;
        private string ipAddress;
        private int tcpPort;
        private string macAddress;
        private bool restartAP;
        private int dataRate;
        private string registerId;
        private string registervalue;
        private string configureType;
        private bool isEByte;
        private EBYTEDTO ebyteDTO;
        private int machineCount;
        private bool isRadian;
        /// <summary>
        /// Default constructor
        /// </summary>
        public HubContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required the data fields
        /// </summary>
        public HubContainerDTO(int masterId, string masterName, int? portNumber, int baudRate, string notes,
                          string address, string frequency, string serverMachine, string directMode,
                          string ipAddress, int tcpPort, string macAddress, bool restartAP, bool isEBYTE,
                          bool isRadian)
            : this()
        {
            log.LogMethodEntry(masterId, masterName, portNumber, baudRate, notes, address,
                               frequency, serverMachine, directMode, ipAddress, tcpPort, macAddress,
                               restartAP, isEBYTE, isRadian);
            this.masterId = masterId;
            this.masterName = masterName;
            this.portNumber = portNumber;
            this.baudRate = baudRate;
            this.notes = notes;
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
        /// Get/Set for MasterId
        /// </summary>
        public int MasterId { get { return masterId; } set { masterId = value;  } }
        /// <summary>
        /// Get/Set for MasterName
        /// </summary>
        public string MasterName { get { return masterName; } set { masterName = value;  } }
        /// <summary>
        /// Get/Set for PORTNumber
        /// </summary>
        public int? PortNumber { get { return portNumber; } set { portNumber = value;  } }
        /// <summary>
        /// Get/Set for baudRate
        /// </summary>
        public int BaudRate { get { return baudRate; } set { baudRate = value;  } }
        /// <summary>
        /// Get/Set for Notes
        /// </summary>
        public string Notes { get { return notes; } set { notes = value;  } }
        /// <summary>
        /// Get/Set for Address
        /// </summary>
        public string Address { get { return address; } set { address = value;  } }
        /// <summary>
        /// Get/Set for Frequency
        /// </summary>
        public string Frequency { get { return frequency; } set { frequency = value;  } }
        /// <summary>
        /// Get/Set for ServerMachine
        /// </summary>
        public string ServerMachine { get { return serverMachine; } set { serverMachine = value;  } }
        /// <summary>
        /// Get/Set for DirectMode
        /// </summary>
        public string DirectMode { get { return directMode; } set { directMode = value;  } }
        /// <summary>
        /// Get/Set for IPAddress
        /// </summary>
        public string IPAddress { get { return ipAddress; } set { ipAddress = value;  } }
        /// <summary>
        /// Get/Set for TCPPort
        /// </summary>
        public int TCPPort { get { return tcpPort; } set { tcpPort = value;  } }
        /// <summary>
        /// Get/Set for MACAddress
        /// </summary>
        public string MACAddress { get { return macAddress; } set { macAddress = value;  } }

        /// <summary>
        /// Get/Set for RestartAP
        /// </summary>
        public bool RestartAP { get { return restartAP; } set { restartAP = value;  } }

        /// <summary>
        /// Get/Set for DataRate
        /// </summary>
        public int DataRate { get { return dataRate; } set { dataRate = value;  } }

        /// <summary>
        /// Get/Set for RegisterId
        /// </summary>
        public string RegisterId { get { return registerId; } set { registerId = value;  } }

        /// <summary>
        /// Get/Set for RegisterValue
        /// </summary>
        public string RegisterValue { get { return registervalue; } set { registervalue = value;  } }

        /// <summary>
        /// Get/Set for ConfigureType
        /// </summary>
        public string ConfigureHubType { get { return configureType; } set { configureType = value;  } }

        /// <summary>
        /// Get/Set for IsEByte
        /// </summary>
        public bool IsEByte { get { return isEByte; } set { isEByte = value;  } }

        /// <summary>
        /// Get/Set for EBYTEHubDTO
        /// </summary>
        public EBYTEDTO EBYTEDTO { get { return ebyteDTO; } set { ebyteDTO = value; } }

        /// <summary>
        /// Get/Set for MachineCount
        /// </summary>
        public int MachineCount { get { return machineCount; } set { machineCount = value; } }

        /// <summary>
        /// Get/Set for IsRadian
        /// </summary>
        public bool IsRadian { get { return isRadian; } set { isRadian = value; } }
        /// <summary>
        /// Get/Set for HubNameWithMachineCount
        /// </summary>
        public string HubNameWithMachineCount
        {
            get
            {
                return ((string.IsNullOrEmpty(MasterName) || string.IsNullOrWhiteSpace(MasterName)) ? " " : MasterName + "[MachineCount:" + MachineCount + "]");
            }
            set { }
        }
    }
}

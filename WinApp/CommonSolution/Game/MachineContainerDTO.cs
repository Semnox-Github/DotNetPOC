using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    public class MachineContainerDTO
    {
        /// <summary>
        /// This is the Machine container data object class. This acts as data holder for the game business object
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int machineId;
        private string machineName;
        private string machineAddress;
        private int gameId;
        private int masterId;
        private string notes;
        private string ticketAllowed;
        private string timerMachine;
        private int timerInterval;
        private string groupTimer;
        private int numberOfCoins;
        private string ticketMode;
        private int customDataSetId;
        private int themeId;
        private int themeNumber;
        private string showAd;
        private string ipAddress;
        private int tcpPort;
        private string macAddress;
        private string description;
        private string serialNumber;
        private string softwareVersion;
        private int purchasePrice;
        private int readerType;
        private double payoutCost;
        private int inventoryLocationId;
        private int referenceMachineId;
        private string externalMachineReference;
        private string machineTag;
        private int communicationSuccessRatio;
        private int previousMachineId;
        private int nextMachineId;
        private DateTime machineArrivalDate;
        private List<GamePriceTierContainerDTO> gamePriceTierContainerDTOList = new List<GamePriceTierContainerDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public MachineContainerDTO(int machineId, string machineName, string machineAddress, int gameId, int masterId,
                           string notes, string ticketAllowed,
                           string timerMachine, int timerInterval, string groupTimer,
                           int numberOfCoins, string ticketMode, int customDataSetId, int themeId, int themeNumber, string showAd,
                           string ipAddress, int tcpPort, string macAddress, string description, string serialNumber,
                           string softwareVersion, int purchasePrice, int readerType, double payoutCost, int inventoryLocationId, int referenceMachineId,
                           string externalMachineReference, string machineTag, int communicationSuccessRatio, int previousMachineId, int nextMachineId, DateTime machineArrivalDate)
            : this()
        {
            log.LogMethodEntry(machineId, machineName, machineAddress, gameId, masterId,
                            notes, ticketAllowed, timerMachine, timerInterval, groupTimer,
                            numberOfCoins, ticketMode, customDataSetId, themeId, themeNumber, showAd,
                            ipAddress, tcpPort, macAddress, description, serialNumber, softwareVersion,
                            purchasePrice, readerType, payoutCost, inventoryLocationId, referenceMachineId,
                            externalMachineReference, machineTag, communicationSuccessRatio, previousMachineId, nextMachineId, machineArrivalDate
                            );
            this.machineId = machineId;
            this.machineName = machineName;
            this.machineAddress = machineAddress;
            this.gameId = gameId;
            this.masterId = masterId;
            this.notes = notes;
            this.ticketAllowed = ticketAllowed;
            this.timerMachine = timerMachine;
            this.timerInterval = timerInterval;
            this.groupTimer = groupTimer;
            this.numberOfCoins = numberOfCoins;
            this.ticketMode = ticketMode;
            this.customDataSetId = customDataSetId;
            this.themeId = themeId;
            this.themeNumber = themeNumber;
            this.showAd = showAd;
            this.ipAddress = ipAddress;
            this.tcpPort = tcpPort;
            this.macAddress = macAddress;
            this.description = description;
            this.serialNumber = serialNumber;
            this.softwareVersion = softwareVersion;
            this.purchasePrice = purchasePrice;
            this.readerType = readerType;
            this.payoutCost = payoutCost;
            this.inventoryLocationId = inventoryLocationId;
            this.referenceMachineId = referenceMachineId;
            this.externalMachineReference = externalMachineReference;
            this.machineTag = machineTag;
            this.communicationSuccessRatio = communicationSuccessRatio;
            this.previousMachineId = previousMachineId;
            this.nextMachineId = nextMachineId;
            this.machineArrivalDate = machineArrivalDate;
            log.LogMethodExit();
        }
       ///<summary>
        /// Get/Set method for MachineId
        /// </summary>
        public int MachineId { get { return machineId; } set { machineId = value;  } }

        /// <summary>
        /// Get/Set method for MachineName
        /// </summary>
        public string MachineName { get { return machineName; } set { machineName = value;  } }

        /// <summary>
        /// Get/Set method for MachineAddress
        /// </summary>
        public string MachineAddress { get { return machineAddress; } set { machineAddress = value;  } }

        /// <summary>
        /// Get/Set method for GameId
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value;  } }

        /// <summary>
        /// Get/Set method for MasterId
        /// </summary>
        public int MasterId { get { return masterId; } set { masterId = value;  } }

        /// <summary>
        /// Get/Set method for Notes
        /// </summary>
        public string Notes { get { return notes; } set { notes = value;  } }


        /// <summary>
        /// Get/Set method for TicketAllowed
        /// </summary>
        public string TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value;  } }

        /// <summary>
        /// Get/Set method for TimerMachine
        /// </summary>
        public string TimerMachine { get { return timerMachine; } set { timerMachine = value;  } }

        /// <summary>
        /// Get/Set method for TimeInterval
        /// </summary>
        public int TimerInterval { get { return timerInterval; } set { timerInterval = value;  } }

        /// <summary>
        /// Get/Set method for GroupTimer
        /// </summary>
        public string GroupTimer { get { return groupTimer; } set { groupTimer = value;  } }

        /// <summary>
        /// Get/Set method for NumberOfCoins
        /// </summary>
        public int NumberOfCoins { get { return numberOfCoins; } set { numberOfCoins = value;  } }

        /// <summary>
        /// Get/Set method for TicketMode
        /// </summary>
        public string TicketMode { get { return ticketMode; } set { ticketMode = value;  } }

        /// <summary>
        /// Get/Set method for CustomeDataSetId
        /// </summary>
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value;  } }

        /// <summary>
        /// Get/Set method for ThemeId
        /// </summary>
        public int ThemeId { get { return themeId; } set { themeId = value;  } }

        /// <summary>
        /// Get/Set method for ThemeNumber
        /// </summary>
        public int ThemeNumber { get { return themeNumber; } set { themeNumber = value;  } }

        /// <summary>
        /// Get/Set method for ShowAd
        /// </summary>
        public string ShowAd { get { return showAd; } set { showAd = value;  } }

        /// <summary>
        /// Get/Set method for IpAddress
        /// </summary>
        public string IPAddress { get { return ipAddress; } set { ipAddress = value;  } }

        /// <summary>
        /// Get/Set method for TCPPort
        /// </summary>
        public int TCPPort { get { return tcpPort; } set { tcpPort = value;  } }

        /// <summary>
        /// Get/Set method for MACAddress
        /// </summary>
        public string MacAddress { get { return macAddress; } set { macAddress = value;  } }

        /// <summary>
        /// Get/Set method for Description
        /// </summary>
        public string Description { get { return description; } set { description = value;  } }

        /// <summary>
        /// Get/Set method for SerialNumber
        /// </summary>
        public string SerialNumber { get { return serialNumber; } set { serialNumber = value;  } }

        /// <summary>
        /// Get/Set method for Machine flag
        /// </summary>
        public string MachineTag { get { return machineTag; } set { machineTag = value;  } }

        /// <summary>
        /// Get/Set method for SoftwareVersion
        /// </summary>
        public string SoftwareVersion { get { return softwareVersion; } set { softwareVersion = value;  } }

        /// <summary>
        /// Get/Set method for PurchasePrice
        /// </summary>
        public int PurchasePrice { get { return purchasePrice; } set { purchasePrice = value;  } }


        /// <summary>
        /// Get/Set method for ReaderType
        /// </summary>
        public int ReaderType { get { return readerType; } set { readerType = value;  } }

        /// <summary>
        /// Get/Set method for PayOutCost
        /// </summary>
        public double PayoutCost { get { return payoutCost; } set { payoutCost = value;  } }

        /// <summary>
        /// Get/Set method for InventoryLocationId
        /// </summary>
        public int InventoryLocationId { get { return inventoryLocationId; } set { inventoryLocationId = value;  } }

        /// <summary>
        /// Get/Set method for ReferenceMachineId
        /// </summary>
        public int ReferenceMachineId { get { return referenceMachineId; } set { referenceMachineId = value;  } }

        /// <summary>
        /// Get/Set method for ExternalMachineReference 
        /// </summary>
        public string ExternalMachineReference { get { return externalMachineReference; } set { externalMachineReference = value; } }

        /// <summary>
        /// Get/Set method for CommunicationSuccessRatio 
        /// </summary>
        public int CommunicationSuccessRatio { get { return communicationSuccessRatio; } set { communicationSuccessRatio = value; } }

        /// <summary>
        /// Get/Set method for PreviousMachineId
        /// </summary>
        public int PreviousMachineId { get { return previousMachineId; } set { previousMachineId = value; } }
        /// <summary>
        /// Get/Set method for NextMachineId
        /// </summary>
        public int NextMachineId { get { return nextMachineId; } set { nextMachineId = value; } }

       public DateTime MachineArrivalDate
        {
            get { return machineArrivalDate; }
            set { machineArrivalDate = value;  }
        }

        /// <summary>
        /// Get/Set for GamePriceTierContainerDTOList
        /// </summary>
        public List<GamePriceTierContainerDTO> GamePriceTierContainerDTOList { get { return gamePriceTierContainerDTOList; } set { gamePriceTierContainerDTOList = value; } }
    }
}

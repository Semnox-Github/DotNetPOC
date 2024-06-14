/***************************************************************************************************************
 * Project Name - Machine Data dto                                                                          
 * Description  - Dto of the Machine class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ****************************************************************************************************************
 *2.50.0      12-dec-2018   Guru S A      Who column changes
 *2.60.2      16-mar-2019   Muhammed mehraj  added foreign keys columns -1 to default constructor
 *2.70.2        18-Jun-2019   Girish Kundar  Modified: Added List<MachineInputDevicesDTO>
                                                     List<MachineTransferLogDTO> as Data Members. 
 *2.70.2        29-Jul-2019   Deeksha         Added MasterEntityId as a search parameter.Added a recursive function for the List DTO.
 *2.70.3        21-Dec-2019   Archana         TicketMode enum is added and GameName and HubName fields are added
 *                                            also get properties are added for MachineNameGameNameHubName and MachineNameHubName 
 *                                             *2.80        22-Jan-2020   Indrajeet K    Added Property - MachineArrivalTime
  *2.110.0     01-Feb-2021   Girish Kundar Modified : Virtual Arcade changes - Added Machine characteristics and other temp fields                                               
  *2.130.0    01-Jul-2021  Mathew Ninan    Added two properties - QRPlayIdentifier string and EraseQRPlayIdentifier flag 
 ***************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// MachineDTO
    /// </summary>
    public class MachineDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByMachineParameters
        /// </summary>
        public enum SearchByMachineParameters
        {
            /// <summary>
            /// Search By GAME ID
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search By MACHINE NAME
            /// </summary>
            MACHINE_NAME,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search By MACHINE ID
            /// </summary>
            MACHINE_ID,
            /// <summary>
            /// Search By MAC ADDRESS
            /// </summary>
            MACADDRESS,
            /// <summary>
            /// Search By CUSTOM DATA SET ID
            /// </summary>
            CUSTOM_DATA_SET_ID,
            /// <summary>
            /// Search By EXTERNAL MACHINE REFERENCE
            /// </summary>
            EXTERNAL_MACHINE_REFERENCE,
            /// <summary>
            /// Search By EXTERNAL MACHINE REFERENCE
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ID LiST
            /// </summary>
            MASTER_ID,
            /// <summary>
            /// Search By REFERENCE MACHINE ID
            /// </summary>
            REFERENCE_MACHINE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search By IS_VIRTUAL_ARCADE 
            /// </summary>
            IS_VIRTUAL_ARCADE,
            /// <summary>
            /// Search By QRPlayIdentifier 
            /// </summary>
            QR_PLAY_IDENTIFIER,
            /// <summary>
            /// Search By ALLOWED_MACHINE_ID 
            /// </summary>
            ALLOWED_MACHINE_ID,
            /// <summary>
            /// Search by DISCOUNT_ID_LIST field is
            /// </summary>
            ALLOWED_MACHINE_ID_LIST,
        }

        public enum TICKETMODE
        {
            /// <summary>
            /// Default
            /// </summary>
            DEFAULT,
            /// <summary>
            /// Physical
            /// </summary>
            PHYSICAL,
            /// <summary>
            /// E-Ticket
            /// </summary>
            ETICKET
        }
        private int machineId;
        private string machineName;
        private string machineAddress;
        private int gameId;
        private int masterId;
        private string notes;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private string ticketAllowed;
        private string isActive;
        private string timerMachine;
        private int timerInterval;
        private string groupTimer;
        private int numberOfCoins;
        private string ticketMode;
        private int customDataSetId;
        private int themeId;
        private int themeNumber;
        private string showAd;
        private string guid;
        private int siteId;
        private string ipAddress;
        private int tcpPort;
        private string macAddress;
        private string description;
        private string serialNumber;
        private string softwareVersion;
        private bool synchStatus;
        private int purchasePrice;
        private int readerType;
        private double payoutCost;
        private int inventoryLocationId;
        private int referenceMachineId;
        private int masterEntityId;
        private string externalMachineReference;
        private string machineTag;
        private int communicationSuccessRatio;
        private int previousMachineId;
        private int nextMachineId;
        private List<MachineAttributeDTO> machineAttributes;
        private DateTime creationDate;
        private string createdBy;
        private int activeDisplayThemeId;
        private MachineCommunicationLogDTO machineCommunicationLogDTO;
        private string machineNameGameName;
        private List<MachineInputDevicesDTO> machineInputDevicesDTOList;
        private List<MachineTransferLogDTO> machineTransferLogDTOList;
        private string gameName;
        private string hubName;
        private DateTime machineArrivalDate;
        private string vipPrice;
        private double communicationSuccessRate;
        private string machineCharacteristics;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string qRPlayIdentifier;
        private bool eraseQRPlayIdentifier;
        private int allowedMachineId;
        private List<GamePriceTierDTO> gamePriceTierDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineDTO()
        {
            log.LogMethodEntry();
            machineAttributes = new List<MachineAttributeDTO>();
            machineId = -1;
            masterId = -1;
            gameId = -1;
            customDataSetId = -1;
            isActive = "Y";
            inventoryLocationId = -1;
            siteId = -1;
            masterEntityId = -1;
            previousMachineId = -1;
            nextMachineId = -1;
            referenceMachineId = -1;
            themeId = -1;
            communicationSuccessRatio = 0;
            activeDisplayThemeId = -1;
            allowedMachineId = -1;
            machineInputDevicesDTOList = new List<MachineInputDevicesDTO>();
            machineTransferLogDTOList = new List<MachineTransferLogDTO>();
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public MachineDTO(int machineId, string machineName, string machineAddress, int gameId, int masterId,
                           string notes, string ticketAllowed,
                           string isActive, string timerMachine, int timerInterval, string groupTimer,
                           int numberOfCoins, string ticketMode, int customDataSetId, int themeId, int themeNumber, string showAd,
                           string ipAddress, int tcpPort, string macAddress, string description, string serialNumber,
                           string softwareVersion, int purchasePrice, int readerType, double payoutCost, int inventoryLocationId, int referenceMachineId,
                           string externalMachineReference, string machineTag, int communicationSuccessRatio, int previousMachineId, int nextMachineId,
                           DateTime machineArrivalDate, string machineCharacteristics, string attribute1, string attribute2, string attribute3, int allowedMachineId)
            : this()
        {
            log.LogMethodEntry(machineId, machineName, machineAddress, gameId, masterId,
                            notes, ticketAllowed, isActive, timerMachine, timerInterval, groupTimer,
                            numberOfCoins, ticketMode, customDataSetId, themeId, themeNumber, showAd,
                            ipAddress, tcpPort, macAddress, description, serialNumber, softwareVersion,
                            purchasePrice, readerType, payoutCost, inventoryLocationId, referenceMachineId,
                            externalMachineReference, machineTag, communicationSuccessRatio, previousMachineId,
                            nextMachineId, machineArrivalDate, machineCharacteristics, attribute1, attribute2, attribute3,
                            allowedMachineId);
            this.machineId = machineId;
            this.machineName = machineName;
            this.machineAddress = machineAddress;
            this.gameId = gameId;
            this.masterId = masterId;
            this.notes = notes;
            this.ticketAllowed = ticketAllowed;
            this.isActive = isActive;
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
            this.machineCharacteristics = machineCharacteristics;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.allowedMachineId = allowedMachineId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MachineDTO(int machineId, string machineName, string machineAddress, int gameId, int masterId,
                           string notes, DateTime lastUpdateDate, string lastUpdatedBy, string ticketAllowed,
                           string isActive, string timerMachine, int timerInterval, string groupTimer,
                           int numberOfCoins, string ticketMode, int customDataSetId, int themeId, int themeNumber, string showAd,
                           string guid, int siteId, string ipAddress, int tcpPort, string macAddress,
                           string description, string serialNumber, string softwareVersion, bool synchStatus,
                           int purchasePrice, int readerType, double payoutCost, int inventoryLocationId, int referenceMachineId, int masterEntityId,
                           string externalMachineReference, string machineTag, int communicationSuccessRatio, int previousMachineId,
                           int nextMachineId, DateTime creationDate, string createdBy, DateTime machineArrivalDate,
                           string machineCharacteristics, string attribute1, string attribute2, string attribute3,
                           string qRPlayIdentifier, bool eraseQRPlayIdentifier, int allowedMachineId)
            : this(machineId, machineName, machineAddress, gameId, masterId,
                            notes, ticketAllowed, isActive, timerMachine, timerInterval, groupTimer,
                            numberOfCoins, ticketMode, customDataSetId, themeId, themeNumber, showAd,
                            ipAddress, tcpPort, macAddress, description, serialNumber, softwareVersion,
                            purchasePrice, readerType, payoutCost, inventoryLocationId, referenceMachineId,
                            externalMachineReference, machineTag, communicationSuccessRatio, previousMachineId,
                            nextMachineId, machineArrivalDate, machineCharacteristics, attribute1, attribute2, attribute3, allowedMachineId)
        {
            log.LogMethodEntry(machineId, machineName, machineAddress, gameId, masterId,
                            notes, lastUpdateDate, lastUpdatedBy, ticketAllowed,
                            isActive, timerMachine, timerInterval, groupTimer,
                            numberOfCoins, ticketMode, customDataSetId, themeId, themeNumber, showAd,
                            guid, siteId, ipAddress, tcpPort, macAddress,
                            description, serialNumber, softwareVersion, synchStatus,
                            purchasePrice, readerType, payoutCost, inventoryLocationId, referenceMachineId, masterEntityId,
                            externalMachineReference, machineTag, communicationSuccessRatio, previousMachineId, nextMachineId,
                            creationDate, createdBy, machineArrivalDate, machineCharacteristics, attribute1, attribute2, attribute3, allowedMachineId);

            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.qRPlayIdentifier = qRPlayIdentifier;
            this.eraseQRPlayIdentifier = eraseQRPlayIdentifier;
            activeDisplayThemeId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method for MachineId
        /// </summary>
        public int MachineId { get { return machineId; } set { machineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for MachineName
        /// </summary>
        public string MachineName { get { return machineName; } set { machineName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for MachineAddress
        /// </summary>
        public string MachineAddress { get { return machineAddress; } set { machineAddress = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for GameId
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for MasterId
        /// </summary>
        public int MasterId { get { return masterId; } set { masterId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for Notes
        /// </summary>
        public string Notes { get { return notes; } set { notes = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for LastUpdateDate
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method for LastUpdatedUser
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method for TicketAllowed
        /// </summary>
        public string TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for IsActive
        /// </summary>
        public string IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for creationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method for CreatedBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method for TimerMachine
        /// </summary>
        public string TimerMachine { get { return timerMachine; } set { timerMachine = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for TimeInterval
        /// </summary>
        public int TimerInterval { get { return timerInterval; } set { timerInterval = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for GroupTimer
        /// </summary>
        public string GroupTimer { get { return groupTimer; } set { groupTimer = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for NumberOfCoins
        /// </summary>
        public int NumberOfCoins { get { return numberOfCoins; } set { numberOfCoins = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for TicketMode
        /// </summary>
        public string TicketMode { get { return ticketMode; } set { ticketMode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for CustomeDataSetId
        /// </summary>
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for ThemeId
        /// </summary>
        public int ThemeId { get { return themeId; } set { themeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for ThemeNumber
        /// </summary>
        public int ThemeNumber { get { return themeNumber; } set { themeNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for ShowAd
        /// </summary>
        public string ShowAd { get { return showAd; } set { showAd = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for SiteId
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method for IpAddress
        /// </summary>
        public string IPAddress { get { return ipAddress; } set { ipAddress = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for TCPPort
        /// </summary>
        public int TCPPort { get { return tcpPort; } set { tcpPort = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for MACAddress
        /// </summary>
        public string MacAddress { get { return macAddress; } set { macAddress = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for Description
        /// </summary>
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for SerialNumber
        /// </summary>
        public string SerialNumber { get { return serialNumber; } set { serialNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for Machine flag
        /// </summary>
        public string MachineTag { get { return machineTag; } set { machineTag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for SoftwareVersion
        /// </summary>
        public string SoftwareVersion { get { return softwareVersion; } set { softwareVersion = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for SyncStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }


        /// <summary>
        /// Get/Set method for PurchasePrice
        /// </summary>
        public int PurchasePrice { get { return purchasePrice; } set { purchasePrice = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method for ReaderType
        /// </summary>
        public int ReaderType { get { return readerType; } set { readerType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for PayOutCost
        /// </summary>
        public double PayoutCost { get { return payoutCost; } set { payoutCost = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for InventoryLocationId
        /// </summary>
        public int InventoryLocationId { get { return inventoryLocationId; } set { inventoryLocationId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for ReferenceMachineId
        /// </summary>
        public int ReferenceMachineId { get { return referenceMachineId; } set { referenceMachineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method for ExternalMachineReference 
        /// </summary>
        public string ExternalMachineReference { get { return externalMachineReference; } set { externalMachineReference = value; } }

        /// <summary>
        /// Get/Set method for CommunicationSuccessRatio 
        /// </summary>
        public int CommunicationSuccessRatio { get { return communicationSuccessRatio; } set { communicationSuccessRatio = value; } }

        /// <summary>
        /// Get/Set method for ActiveDisplayThemeId
        /// </summary>
        public int ActiveDisplayThemeId { get { return activeDisplayThemeId; } set { activeDisplayThemeId = value; } }

        /// <summary>
        /// Get/Set method for PreviousMachineId
        /// </summary>
        public int PreviousMachineId { get { return previousMachineId; } set { previousMachineId = value; } }
        /// <summary>
        /// Get/Set method for NextMachineId
        /// </summary>
        public int NextMachineId { get { return nextMachineId; } set { nextMachineId = value; } }


        /// <summary>
        /// Get/Set method for GameMachineAttributes
        /// </summary>
        public List<MachineAttributeDTO> GameMachineAttributes { get { return machineAttributes; } set { machineAttributes = value; } }

        /// <summary>
        /// Get/Set method for MachineCommunicationLogDTO
        /// </summary>
        public MachineCommunicationLogDTO MachineCommunicationLogDTO { get { return machineCommunicationLogDTO; } set { machineCommunicationLogDTO = value; } }

        /// <summary>
        /// Get/Set method for MachineNameGameName
        /// </summary>
        public string MachineNameGameName
        {
            get
            {
                return ((string.IsNullOrEmpty(GameName) || string.IsNullOrWhiteSpace(GameName) || string.IsNullOrEmpty(MachineName) || string.IsNullOrWhiteSpace(MachineName)) ? " " : GameName + "-" + MachineName);
            }
            set { }
        }

        /// <summary>
        /// Get/Set method for GameName
        /// </summary>
        public string GameName { get { return gameName; } set { gameName = value; } }

        /// <summary>
        /// Get/Set method for HubName
        /// </summary>
        public string HubName { get { return hubName; } set { hubName = value; } }
        public double CommunicationSuccessRate { get { return communicationSuccessRate; } set { communicationSuccessRate = value; } }
        public string VipPrice { get { return vipPrice; } set { vipPrice = value; } }
        public string MachineCharacteristics { get { return machineCharacteristics; } set { machineCharacteristics = value; } }
        public string Attribute1 { get { return attribute1; } set { attribute1 = value; } }
        public string Attribute2 { get { return attribute2; } set { attribute2 = value; } }
        public string Attribute3 { get { return attribute3; } set { attribute3 = value; } }

        /// <summary>
        /// QRPlayIdentifier
        /// </summary>
        public string QRPlayIdentifier
        {
            get { return qRPlayIdentifier; }
            set { qRPlayIdentifier = value; this.IsChanged = true; }
        }

        /// <summary>
        /// EraseQRPlayIdentifier to mark QR identifier for removal
        /// </summary>
        public bool EraseQRPlayIdentifier
        {
            get { return eraseQRPlayIdentifier; }
            set { eraseQRPlayIdentifier = value; this.IsChanged = true; }
        }


        /// <summary>
        /// Get/Set method for MachineNameGameNameHubName  with 
        /// </summary>
        public string MachineNameGameNameHubName
        {
            get
            {
                return ((string.IsNullOrEmpty(HubName) || string.IsNullOrWhiteSpace(HubName) || string.IsNullOrEmpty(MachineNameGameName) || string.IsNullOrWhiteSpace(MachineNameGameName)) ? " " : MachineNameGameName + "[" + HubName + "]");
            }
            set { }
        }

        /// <summary>
        /// Get/Set method for MachineNameHubName  with 
        /// </summary>
        public string MachineNameHubName
        {
            get
            {
                return ((string.IsNullOrEmpty(HubName) || string.IsNullOrWhiteSpace(HubName) || string.IsNullOrEmpty(MachineName) || string.IsNullOrWhiteSpace(MachineName)) ? " " : MachineName + "[" + HubName + "]");
            }
            set { }
        }
        /// <summary>
        /// Get/Set method of the MachineInputDevicesDTOList field
        /// </summary>
        public List<MachineInputDevicesDTO> MachineInputDevicesDTOList
        {
            get { return machineInputDevicesDTOList; }
            set { machineInputDevicesDTOList = value; }
        }
        public int AllowedMachineID { get { return allowedMachineId; } set { allowedMachineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MachineTransferLogDTOList field
        /// </summary>
        public List<MachineTransferLogDTO> MachineTransferLogDTOList
        {
            get { return machineTransferLogDTOList; }
            set { machineTransferLogDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the gamePriceTierDTOList field
        /// </summary>
        public List<GamePriceTierDTO> GamePriceTierDTOList
        {
            get { return gamePriceTierDTOList; }
            set { gamePriceTierDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the MachineArrivalDate
        /// </summary>
        public DateTime MachineArrivalDate
        {
            get { return machineArrivalDate; }
            set { machineArrivalDate = value; this.IsChanged = true; }
        }

        /// <summary>
        ///  Sets the attribute list
        /// </summary>
        /// <param name="machineAttributes">MachineAttributeDTO typed list to set the attribute</param>
        public void SetAttributeList(List<MachineAttributeDTO> machineAttributes)
        {
            log.LogMethodEntry(machineAttributes);
            this.machineAttributes = machineAttributes;
            log.LogMethodExit();
        }

        /// <summary>
        /// Adding to attributes
        /// </summary>
        /// <param name="machineAttribute">MachineAttribute typed value to add the attribute</param>
        /// <param name="attributeValue">String typed attribute value</param>
        public void AddToAttributes(MachineAttributeDTO.MachineAttribute machineAttribute, string attributeValue)
        {
            log.LogMethodEntry(machineAttribute, attributeValue);
            machineAttributes.Add(new MachineAttributeDTO(machineAttribute, attributeValue, MachineAttributeDTO.AttributeContext.MACHINE));
            log.LogMethodExit();
        }

        /// <summary>
        ///  Adding to attributes
        /// </summary>
        /// <param name="attributeId">Integer typed attribute identification no</param>
        /// <param name="machineAttribute">MachineAttribute typed value to add the attribute</param>
        /// <param name="attributeValue">String typed attribute value</param>
        /// <param name="isFlag"></param>
        /// <param name="isSoftwareBased">Boolean flag to identify whether attribute is software based or not</param>
        public void AddToAttributes(int attributeId, MachineAttributeDTO.MachineAttribute machineAttribute, string attributeValue, string isFlag, string isSoftwareBased, string guid)
        {
            log.LogMethodEntry(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareBased);
            machineAttributes.Add(new MachineAttributeDTO(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareBased, MachineAttributeDTO.AttributeContext.MACHINE, guid, synchStatus, siteId, lastUpdatedBy, lastUpdateDate, masterEntityId, createdBy, creationDate));
            log.LogMethodExit();
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
                    return notifyingObjectIsChanged || machineId < 0;
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
        /// Returns whether the MachineDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (machineInputDevicesDTOList != null &&
                   machineInputDevicesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (machineTransferLogDTOList != null &&
                  machineTransferLogDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (machineAttributes != null &&
                    machineAttributes.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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

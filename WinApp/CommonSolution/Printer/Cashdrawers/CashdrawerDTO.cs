/********************************************************************************************
 * Project Name - Device
 * Description  - CashdrawerUseCaseFactory.cs
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Aug-2021     Girish Kundar              Created : Multi cashdrawer for POS changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    public enum CashdrawerIntefaceTypes {
        SERIALPORT,
        RECEIPTPRINTER
    }
    public class CashdrawerDTO 
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int cashdrawerId;
        private string cashdrawerName;
        private string interfaceType;
        private string communicationString;
        private int serialPort;
        private int serialPortBaud;
        private bool isSystem;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        public enum SearchByParameters
        {
            /// <summary>
            /// Search By cashdrawerlId
            /// </summary>
            CAHSDRAWER_ID,
            /// <summary>
            /// Search By cashdrawerName 
            /// </summary>
            CASHDRAWER_NAME,
            /// <summary>
            /// Search By interfaceType 
            /// </summary>
            INTERFACE_TYPE,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search By IS_SYSTEM FLAG
            /// </summary>
            IS_SYSTEM,
            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }
        /// <summary>
        /// default constructor
        /// </summary>
        public CashdrawerDTO()
        {
            log.LogMethodEntry();
            cashdrawerId = -1;
            cashdrawerName = string.Empty;
            communicationString = string.Empty;
            isSystem = false;
            interfaceType = CashdrawerIntefaceTypes.RECEIPTPRINTER.ToString();
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public CashdrawerDTO(int cashdrawerId, string cashdrawerName, string interfaceType,
                                               string communicationString, int serialPort, int serialPortBaud ,
                                               bool isSystem , bool isActive)
    : this()
        {
            log.LogMethodEntry(cashdrawerId, cashdrawerName, interfaceType, communicationString, serialPort, serialPortBaud,
                               isSystem, isActive);
            this.cashdrawerId = cashdrawerId;
            this.cashdrawerName = cashdrawerName;
            this.interfaceType = interfaceType;
            this.communicationString = communicationString;
            this.serialPort = serialPort;
            this.serialPortBaud = serialPortBaud;
            this.isSystem = isSystem;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public CashdrawerDTO(CashdrawerDTO cashdrawerDTO)
        {
            log.LogMethodEntry(cashdrawerDTO);
            this.cashdrawerId = cashdrawerDTO.CashdrawerId;
            this.cashdrawerName = cashdrawerDTO.CashdrawerName;
            this.interfaceType = cashdrawerDTO.interfaceType;
            this.communicationString = cashdrawerDTO.CommunicationString;
            this.serialPort = cashdrawerDTO.SerialPort;
            this.serialPortBaud = cashdrawerDTO.SerialPortBaud;
            this.isSystem = cashdrawerDTO.IsSystem;
            this.isActive = cashdrawerDTO.IsActive;
            this.isActive = cashdrawerDTO.IsActive;
            this.createdBy = cashdrawerDTO.CreatedBy;
            this.creationDate = cashdrawerDTO.CreationDate;
            this.lastUpdatedBy = cashdrawerDTO.LastUpdatedBy;
            this.lastUpdateDate = cashdrawerDTO.LastUpdateDate;
            this.siteId = cashdrawerDTO.SiteId;
            this.masterEntityId = cashdrawerDTO.MasterEntityId;
            this.synchStatus = cashdrawerDTO.SynchStatus;
            this.guid = cashdrawerDTO.Guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with All data fields.
        /// </summary>
        public CashdrawerDTO(int cashdrawerId, string cashdrawerName, string interfaceType,
                                               string communicationString, int serialPort, int serialPortBaud,
                                               bool isSystem, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                      DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
    : this(cashdrawerId, cashdrawerName, interfaceType, communicationString, serialPort, serialPortBaud,
                               isSystem, isActive)
        {
            log.LogMethodEntry(cashdrawerId, cashdrawerName, interfaceType, communicationString, serialPort, serialPortBaud,
                               isSystem, isActive, createdBy,
                               creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the cashdrawerId field
        /// </summary>
        public int CashdrawerId { get { return cashdrawerId; } set { this.IsChanged = true; cashdrawerId = value; } }
        /// <summary>
        /// Get/Set method of the cashdrawerName field
        /// </summary>
        public string CashdrawerName { get { return cashdrawerName; } set { this.IsChanged = true; cashdrawerName = value; } }
        /// <summary>
        /// Get/Set method of the interfaceType field
        /// </summary>
        public string InterfaceType { get { return interfaceType; } set { this.IsChanged = true; interfaceType = value; } }

        /// <summary>
        /// Get/Set method of the PrintString field
        /// </summary>
        public string CommunicationString { get { return communicationString; } set { this.IsChanged = true; communicationString = value; } }
        /// <summary>
        /// Get/Set method of the serialPort field
        /// </summary>
        public int SerialPort { get { return serialPort; } set { this.IsChanged = true; serialPort = value; } }
        /// <summary>
        /// Get/Set method of the PrintString field
        /// </summary>
        public int SerialPortBaud { get { return serialPortBaud; } set { this.IsChanged = true; serialPortBaud = value; } }
          /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsSystem { get { return isSystem; } set { this.IsChanged = true; isSystem = value; } }
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
        /// Get/Set method of the LastUpdatedDate field
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
                    return notifyingObjectIsChanged || cashdrawerId < 0;
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

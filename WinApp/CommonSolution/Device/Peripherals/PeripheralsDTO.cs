/********************************************************************************************
 * Project Name - Device
 * Description  - Data object of Peripherals
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        10-Jul-2019   Girish kundar         Modified : Added constructor for required fields .
 *                                                             Added Missed Who columns. 
 *2.80        09-Mar-2020   Vikas Dwivedi           Modified as per the Standards for Phase 1 changes.
 *2.110.0     07-Dec-2020     Mathew Ninan          Added support for two new fields 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.Peripherals
{
    public class PeripheralsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by DEVICE_ID field
            /// </summary>
            DEVICE_ID,

            /// <summary>
            /// Search by DEVICE_NAME field
            /// </summary>
            DEVICE_NAME,

            /// <summary>
            /// Search by POS_MACHINE_ID field
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by POS_MACHINE_ID LIST field
            /// </summary>
            POS_MACHINE_ID_LIST,

            /// <summary>
            /// Search by DEVICE_TYPE field
            /// </summary>
            DEVICE_TYPE,

            /// <summary>
            /// Search by DEVICE_SUBTYPE field
            /// </summary>
            DEVICE_SUBTYPE,

            /// <summary>
            /// Search by VID field
            /// </summary>
            VID,

            /// <summary>
            /// Search by PID field
            /// </summary>
            PID,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID,

            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,

            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by EnableTagEncryption field
            /// </summary>
            ENABLE_TAG_ENCRYPTION

        }

        private int deviceId;
        private string deviceName;
        private int posMachineId;
        private string deviceType;
        private string deviceSubType;
        private string vid;
        private string pid;
        private string optionalString;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private bool active;
        private int masterEntityId;
        private bool enableTagEncryption;
        private string excludeDecryptionForTagLength;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private bool readerIsForRechargeOnly;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PeripheralsDTO()
        {
            log.LogMethodEntry();
            deviceId = -1;            
            posMachineId = -1;          
            siteId = -1;           
            masterEntityId = -1;
            readerIsForRechargeOnly = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>

        public PeripheralsDTO(int deviceId, string deviceName, int posMachineId, string deviceType, string deviceSubType,
            string vid, string pid, string optionalString, bool active)
            : this()
        {
            log.LogMethodEntry(deviceId, deviceName, posMachineId, deviceType, deviceSubType, vid, pid, optionalString, active);
            this.deviceId = deviceId;
            this.deviceName = deviceName;
            this.posMachineId = posMachineId;
            this.deviceType = deviceType;
            this.deviceSubType = deviceSubType;
            this.vid = vid;
            this.pid = pid;
            this.optionalString = optionalString;
            this.active = active;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>

        public PeripheralsDTO(int deviceId, string deviceName, int posMachineId, string deviceType, string deviceSubType, 
            string vid, string pid, string optionalString, int siteId, string guid, bool synchStatus, DateTime lastUpdatedDate, 
            string lastUpdatedUser, bool active, int masterEntityId, string createdBy, DateTime creationDate)
            : this(deviceId, deviceName, posMachineId, deviceType, deviceSubType, vid, pid, optionalString, active)
        {
            log.LogMethodEntry(deviceId, deviceName, posMachineId, deviceType, deviceSubType,
                vid, pid, optionalString, siteId, guid, synchStatus, lastUpdatedDate, 
                lastUpdatedUser, active, masterEntityId, createdBy, creationDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        public PeripheralsDTO(int deviceId, string deviceName, int posMachineId, string deviceType, string deviceSubType,
            string vid, string pid, string optionalString, int siteId, string guid, bool synchStatus, DateTime lastUpdatedDate,
            string lastUpdatedUser, bool active, int masterEntityId, string createdBy, DateTime creationDate, 
            bool enableTagDecryption, string excludeDecryptionForTagLength, bool readerIsForRechargeOnly)
            : this(deviceId, deviceName, posMachineId, deviceType, deviceSubType, vid, pid, optionalString, siteId, guid, 
                   synchStatus, lastUpdatedDate, lastUpdatedUser, active, masterEntityId, createdBy, creationDate)
        {
            log.LogMethodEntry(deviceId, deviceName, posMachineId, deviceType, deviceSubType, vid, pid, optionalString, siteId, guid,
                   synchStatus, lastUpdatedDate, lastUpdatedUser, active, masterEntityId, createdBy, creationDate,
                   enableTagDecryption, excludeDecryptionForTagLength, readerIsForRechargeOnly);
            this.enableTagEncryption = enableTagDecryption;
            this.excludeDecryptionForTagLength = excludeDecryptionForTagLength;
            this.readerIsForRechargeOnly = readerIsForRechargeOnly;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the DeviceId field
        /// </summary>
        [DisplayName("DeviceId")]
        public int DeviceId { get { return deviceId; } set { deviceId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the DeviceName field
        /// </summary>
        [DisplayName("DeviceName")]
        public string DeviceName { get { return deviceName; } set { deviceName = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the PosMachineId field
        /// </summary>
        [DisplayName("PosMachineId")]
        public int PosMachineId { get { return posMachineId; } set { posMachineId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the DeviceType field
        /// </summary>
        [DisplayName("DeviceType")]
        public string DeviceType { get { return deviceType; } set { deviceType = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the DeviceSubType field
        /// </summary>
        [DisplayName("DeviceSubType")]
        public string DeviceSubType { get { return deviceSubType; } set { deviceSubType = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Vid field
        /// </summary>
        [DisplayName("Vid")]
        public string Vid { get { return vid; } set { vid = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method of the Pid field
        /// </summary>
        [DisplayName("Pid")]
        public string Pid { get { return pid; } set { pid = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the OptionalString field
        /// </summary>
        [DisplayName("OptionalString")]
        public string OptionalString { get { return optionalString; } set { optionalString = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }


        /// <summary>
        ///  Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedUser")]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }


        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the EnableTagDecryption field
        /// </summary>
        [DisplayName("EnableTagDecryption")]
        public bool EnableTagDecryption { get { return enableTagEncryption; } set { enableTagEncryption = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the ExcludeDecryptionForTagLength field
        /// </summary>
        [DisplayName("ExcludeDecryptionForTagLength")]
        public string ExcludeDecryptionForTagLength { get { return excludeDecryptionForTagLength; } set { excludeDecryptionForTagLength = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the ReaderIsForRechargeOnly field
        /// </summary>
        [DisplayName("ReaderIsForRechargeOnly")]
        public bool ReaderIsForRechargeOnly { get { return readerIsForRechargeOnly; } set { readerIsForRechargeOnly = value; this.IsChanged = true; } }

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
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || deviceId < 0;
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

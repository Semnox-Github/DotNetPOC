/********************************************************************************************
 * Project Name - MachineInputDevices Data dto                                                                          
 * Description  - Dto of the MachineInputDevices class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      12-dec-2018   Guru S A      Who column changes
 *2.70.2        29-Jul-2019   Deeksha       Added a new constructor with required fields.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    public class MachineInputDevicesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by DEVICE ID field
            /// </summary>
            DEVICE_ID,
            /// <summary>
            /// Search by DEVICE NAME field
            /// </summary>
            DEVICE_NAME ,
            /// <summary>
            /// Search by DEVICE TYPE ID field
            /// </summary>
            DEVICE_TYPE_ID,
            /// <summary>
            /// Search by DEVICE MODEL ID field
            /// </summary>
            DEVICE_MODEL_ID ,
            /// <summary>
            /// Search by MACHINE ID field
            /// </summary>
            MACHINE_ID,
            /// <summary>
            /// Search by IP ADDRESS field
            /// </summary>
            IP_ADDRESS ,
            /// <summary>
            /// Search by MAC ADDRESS field
            /// </summary>
            MAC_ADDRESS ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID

        }

        private int deviceId;
        private string deviceName;
        private int deviceTypeId;
        private int deviceModelId;
        private int machineId;
        private bool isActive;
        private string ipAddress;
        private int portNo;
        private string macAddress;
        private int fpTemplateFormat;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private DateTime creationDate;
        private string createdBy;
        private string deviceType;
        private string deviceModel;
        private string fPTFormat;

        /// <summary>
        /// default constructor
        /// </summary>
        public MachineInputDevicesDTO()
        {
            log.LogMethodEntry();
            deviceId = -1;
            deviceName = string.Empty;
            deviceTypeId = -1;
            deviceModelId = -1;
            machineId = -1;
            isActive = true;
            ipAddress = string.Empty;
            portNo = -1;
            macAddress = string.Empty;
            fpTemplateFormat = -1;
            siteId = -1;
            guid = string.Empty;
            synchStatus = false; 
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required table fields.
        /// </summary>
        public MachineInputDevicesDTO(int deviceId, string deviceName, int deviceTypeId, int deviceModelId, int machineId, bool isActive, string ipAddress, int portNo, string macAddress, int fpTemplateFormat)
            :this()
        {
            log.LogMethodEntry(deviceId, deviceName, deviceTypeId, deviceModelId, machineId, isActive, ipAddress, portNo, macAddress, fpTemplateFormat);
            this.deviceId = deviceId;
            this.deviceName = deviceName;
            this.deviceTypeId = deviceTypeId;
            this.deviceModelId = deviceModelId;
            this.machineId = machineId;
            this.isActive = isActive;
            this.ipAddress = ipAddress;
            this.portNo = portNo;
            this.macAddress = macAddress;
            this.fpTemplateFormat = fpTemplateFormat;          
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the table fields.
        /// </summary>
        public MachineInputDevicesDTO(int deviceId, string deviceName, int deviceTypeId, int deviceModelId, int machineId, bool isActive, string ipAddress, 
                                        int portNo, string macAddress, int fpTemplateFormat, int siteId, string guid, bool synchStatus, DateTime lastUpdateDate,
                                        string lastUpdatedBy, DateTime creationDate, string createdBy, int masterEntityId)
            :this(deviceId, deviceName, deviceTypeId, deviceModelId, machineId, isActive, ipAddress, portNo, macAddress, fpTemplateFormat)
        {
            log.LogMethodEntry(deviceId, deviceName, deviceTypeId, deviceModelId, machineId, isActive, ipAddress, portNo, macAddress, fpTemplateFormat, siteId,  guid,  synchStatus, 
                lastUpdateDate,  lastUpdatedBy,  creationDate,  createdBy,  masterEntityId);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.masterEntityId = masterEntityId;
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
        /// Get/Set method of the DeviceTypeId field
        /// </summary>
        [DisplayName("DeviceTypeId")]
        public int DeviceTypeId { get { return deviceTypeId; } set { deviceTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the deviceModelId field
        /// </summary>
        [DisplayName("DeviceModelId")]
        public int DeviceModelId { get { return deviceModelId; } set { deviceModelId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the machineId field
        /// </summary>
        [DisplayName("MachineId")]
        public int MachineId { get { return machineId; } set { machineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ipAddress field
        /// </summary>
        [DisplayName("IPAddress")]
        public string IPAddress { get { return ipAddress; } set { ipAddress = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PortNo field
        /// </summary>
        [DisplayName("PortNo")]
        public int PortNo { get { return portNo; } set { portNo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the macAddress field
        /// </summary>
        [DisplayName("MacAddress")]
        public string MacAddress { get { return macAddress; } set { macAddress = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the fpTemplateFormat field
        /// </summary>
        [DisplayName("FPTemplateFormat")]
        public int FPTemplateFormat { get { return fpTemplateFormat; } set { fpTemplateFormat = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
         

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DeviceType field
        /// </summary>
        public string DeviceType { get { return deviceType; } set { deviceType = value; } }

        /// <summary>
        /// Get/Set method of the DeviceModel field
        /// </summary>
        public string DeviceModel { get { return deviceModel; } set { deviceModel = value; } }

        /// <summary>
        /// Get/Set method of the FPTFormat field
        /// </summary>
        public string FPTFormat { get { return fPTFormat; } set { fPTFormat = value; } }

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

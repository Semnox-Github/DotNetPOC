/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Search Parameter for  MachineInputdevice Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    public class MachineInputDeviceSearchParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int deviceId;
        private string deviceName;
        private int deviceTypeId;
        private int deviceModelId;
        private int machineId;
        private bool isActive;
        private string ipAddress;
        private string macAddress;
        private int siteId;

        /// <summary>
        /// default constructor
        /// </summary>
        public MachineInputDeviceSearchParams()
        {
            log.LogMethodEntry();
            deviceId = -1;
            deviceName = "";
            deviceTypeId = -1;
            deviceModelId = -1;
            machineId = -1;
            isActive = false;
            ipAddress = "";
            macAddress = "";
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the DeviceId field
        /// </summary>
        [DisplayName("DeviceId")]
        public int DeviceId { get { return deviceId; } set { deviceId = value;} }


        /// <summary>
        /// Get/Set method of the DeviceName field
        /// </summary>
        [DisplayName("DeviceName")]
        public string DeviceName { get { return deviceName; } set { deviceName = value; } }

        /// <summary>
        /// Get/Set method of the DeviceTypeId field
        /// </summary>
        [DisplayName("DeviceTypeId")]
        public int DeviceTypeId { get { return deviceTypeId; } set { deviceTypeId = value;  } }

        /// <summary>
        /// Get/Set method of the deviceModelId field
        /// </summary>
        [DisplayName("DeviceModelId")]
        public int DeviceModelId { get { return deviceModelId; } set { deviceModelId = value; } }

        /// <summary>
        /// Get/Set method of the machineId field
        /// </summary>
        [DisplayName("MachineId")]
        public int MachineId { get { return machineId; } set { machineId = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the ipAddress field
        /// </summary>
        [DisplayName("IPAddress")]
        public string IPAddress { get { return ipAddress; } set { ipAddress = value; } }

        /// <summary>
        /// Get/Set method of the macAddress field
        /// </summary>
        [DisplayName("MacAddress")]
        public string MacAddress { get { return macAddress; } set { macAddress = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value;} }
    }
}

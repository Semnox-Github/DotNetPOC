/********************************************************************************************
 * Project Name - Device
 * Description  - Container Data object of Peripherals
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
*2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.Peripherals
{
    public class PeripheralContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int deviceId;
        private string deviceName;
        private int posMachineId;
        private string deviceType;
        private string deviceSubType;
        private string vid;
        private string pid;
        private string optionalString;
        private bool enableTagEncryption;
        private string excludeDecryptionForTagLength;
        private bool readerIsForRechargeOnly = false;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PeripheralContainerDTO()
        {
            log.LogMethodEntry();
            deviceId = -1;            
            posMachineId = -1;
            readerIsForRechargeOnly = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>

        public PeripheralContainerDTO(int deviceId, string deviceName, int posMachineId, string deviceType, string deviceSubType,
            string vid, string pid, string optionalString, bool enableTagEncryption, string excludeDecryptionForTagLength,
             bool readerIsForRechargeOnly)
            : this()
        {
            log.LogMethodEntry(deviceId, deviceName, posMachineId, deviceType, deviceSubType, vid, pid, optionalString, enableTagEncryption, excludeDecryptionForTagLength);
            this.deviceId = deviceId;
            this.deviceName = deviceName;
            this.posMachineId = posMachineId;
            this.deviceType = deviceType;
            this.deviceSubType = deviceSubType;
            this.vid = vid;
            this.pid = pid;
            this.optionalString = optionalString;
            this.enableTagEncryption = enableTagEncryption;
            this.excludeDecryptionForTagLength = excludeDecryptionForTagLength;
            this.readerIsForRechargeOnly = readerIsForRechargeOnly;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the DeviceId field
        /// </summary>
        [DisplayName("DeviceId")]
        public int DeviceId { get { return deviceId; } set { deviceId = value; } }


        /// <summary>
        /// Get/Set method of the DeviceName field
        /// </summary>
        [DisplayName("DeviceName")]
        public string DeviceName { get { return deviceName; } set { deviceName = value;  } }


        /// <summary>
        /// Get/Set method of the PosMachineId field
        /// </summary>
        [DisplayName("PosMachineId")]
        public int PosMachineId { get { return posMachineId; } set { posMachineId = value;  } }


        /// <summary>
        /// Get/Set method of the DeviceType field
        /// </summary>
        [DisplayName("DeviceType")]
        public string DeviceType { get { return deviceType; } set { deviceType = value;  } }


        /// <summary>
        /// Get/Set method of the DeviceSubType field
        /// </summary>
        [DisplayName("DeviceSubType")]
        public string DeviceSubType { get { return deviceSubType; } set { deviceSubType = value;  } }


        /// <summary>
        /// Get/Set method of the Vid field
        /// </summary>
        [DisplayName("Vid")]
        public string Vid { get { return vid; } set { vid = value;  } }


        /// <summary>
        ///  Get/Set method of the Pid field
        /// </summary>
        [DisplayName("Pid")]
        public string Pid { get { return pid; } set { pid = value;  } }


        /// <summary>
        /// Get/Set method of the OptionalString field
        /// </summary>
        [DisplayName("OptionalString")]
        public string OptionalString { get { return optionalString; } set { optionalString = value;  } }


        /// <summary>
        ///  Get/Set method of the EnableTagDecryption field
        /// </summary>
        [DisplayName("EnableTagDecryption")]
        public bool EnableTagDecryption { get { return enableTagEncryption; } set { enableTagEncryption = value;  } }

        /// <summary>
        ///  Get/Set method of the ExcludeDecryptionForTagLength field
        /// </summary>
        [DisplayName("ExcludeDecryptionForTagLength")]
        public string ExcludeDecryptionForTagLength { get { return excludeDecryptionForTagLength; } set { excludeDecryptionForTagLength = value;  } }
        /// <summary>
        ///  Get/Set method of the ReaderIsForRechargeOnly field
        /// </summary>
        [DisplayName("ReaderIsForRechargeOnly")]
        public bool ReaderIsForRechargeOnly { get { return readerIsForRechargeOnly; } set { readerIsForRechargeOnly = value; } }

    }
}

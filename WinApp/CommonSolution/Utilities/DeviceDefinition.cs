/********************************************************************************************
 * Project Name - Utilities
 * Description  - Definition of a device
 * 
 **************
 **Version Log
 **************
 *Version        Date            Modified By         Remarks          
 *********************************************************************************************
 *2.110.0        1-Jul-2019      Lakshminarayana     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Core.Utilities
{
    public class DeviceDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string name;
        private DeviceType deviceType;
        private DeviceSubType deviceSubType;
        private string vid;
        private string pid;
        private string optString;
        private bool isDefault;
        private IntPtr handle;
        private IEnumerable<MifareKeyContainerDTO> mifareKeyContainerDTOList;
        private int siteId;
        private int deviceAddress = -1;
        private List<string> displayMessageLineList;
        private bool enableTagDecryption;
        private string excludeDecryptionForTagLength;
        private bool readerIsForRechargeOnly = false;

        public DeviceDefinition(string name,
                                DeviceType deviceType,
                                DeviceSubType deviceSubType,
                                string vid,
                                string pid,
                                string optString,
                                bool isDefault,
                                IntPtr handle,
                                IEnumerable<MifareKeyContainerDTO> mifareKeyContainerDTOList,
                                int siteId,
                                List<string> displayMessageLineList,
                                bool enableTagDecryption,
                                string excludeDecryptionForTagLength,
                                bool readerIsForRechargeOnly)
        {
            log.LogMethodEntry(name, deviceType, deviceSubType, vid, pid, optString,
                               isDefault, handle, mifareKeyContainerDTOList, siteId,
                               displayMessageLineList, enableTagDecryption,
                               excludeDecryptionForTagLength, readerIsForRechargeOnly);
            this.name = name;
            this.deviceType = deviceType;
            this.deviceSubType = deviceSubType;
            this.vid = vid;
            this.pid = pid;
            this.optString = optString;
            this.isDefault = isDefault;
            this.handle = handle;
            this.mifareKeyContainerDTOList = mifareKeyContainerDTOList;
            this.siteId = siteId;
            this.displayMessageLineList = displayMessageLineList;
            this.enableTagDecryption = enableTagDecryption;
            this.excludeDecryptionForTagLength = excludeDecryptionForTagLength;
            this.readerIsForRechargeOnly = readerIsForRechargeOnly;
            log.LogMethodExit();

        }

        public DeviceDefinition(string name,
                                DeviceType deviceType,
                                string vid,
                                string pid,
                                string optString,
                                IntPtr handle,
                                bool isDefault = false,
                                bool enableTagDecryption = false,
                                string excludeDecryptionForTagLength = null,
                                bool readerIsForRechargeOnly = false)
        {
            log.LogMethodEntry(name, deviceType, vid, pid, optString,
                               isDefault, handle, enableTagDecryption,
                               excludeDecryptionForTagLength, readerIsForRechargeOnly);
            this.name = name;
            this.deviceType = deviceType;
            this.deviceSubType = DeviceSubType.KeyboardWedge;
            this.vid = vid;
            this.pid = pid;
            this.optString = optString;
            this.isDefault = isDefault;
            this.handle = handle;
            this.enableTagDecryption = enableTagDecryption;
            this.excludeDecryptionForTagLength = excludeDecryptionForTagLength;
            this.readerIsForRechargeOnly = readerIsForRechargeOnly;
            log.LogMethodExit();

        }

        public DeviceDefinition(string name,
                                DeviceSubType deviceSubType,
                                IEnumerable<MifareKeyContainerDTO> mifareKeyContainerDTOList,
                                int siteId,
                                bool isDefault = false,
                                List<string> displayMessageLineList = null,
                                string serialNumber = null,
                                bool readerIsForRechargeOnly = false
                                )
        {
            log.LogMethodEntry(name, deviceSubType, isDefault, mifareKeyContainerDTOList, siteId,
                               displayMessageLineList, serialNumber, readerIsForRechargeOnly);
            this.name = name;
            deviceType = DeviceType.CardReader;
            this.deviceSubType = deviceSubType;
            optString = serialNumber;
            this.isDefault = isDefault;
            this.mifareKeyContainerDTOList = mifareKeyContainerDTOList;
            this.siteId = siteId;
            this.displayMessageLineList = displayMessageLineList;
            this.readerIsForRechargeOnly = readerIsForRechargeOnly;
            log.LogMethodExit();

        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public DeviceType DeviceType
        {
            get
            {
                return deviceType;
            }
            set
            {
                deviceType = value;
            }
        }
        public DeviceSubType DeviceSubType
        {
            get
            {
                return deviceSubType;
            }
            set
            {
                deviceSubType = value;
            }
        }
        public string Vid
        {
            get
            {
                return vid;
            }
            set
            {
                vid = value;
            }
        }
        public string Pid
        {
            get
            {
                return pid;
            }
            set
            {
                pid = value;
            }
        }
        public string OptString
        {
            get
            {
                return optString;
            }
            set
            {
                optString = value;
            }
        }
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }
            set
            {
                isDefault = value;
            }
        }
        public IntPtr Handle
        {
            get
            {
                return handle;
            }
            set
            {
                handle = value;
            }
        }
        public IEnumerable<MifareKeyContainerDTO> MifareKeyContainerDTOList
        {
            get
            {
                return mifareKeyContainerDTOList;
            }
            set
            {
                mifareKeyContainerDTOList = value;
            }
        }
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }
        public int DeviceAddress
        {
            get
            {
                return deviceAddress;
            }
            set
            {
                deviceAddress = value;
            }
        }
        public List<string> DisplayMessageLineList
        {
            get
            {
                return displayMessageLineList;
            }
            set
            {
                displayMessageLineList = value;
            }
        }
        public bool EnableTagDecryption
        {
            get
            {
                return enableTagDecryption;
            }
            set
            {
                enableTagDecryption = value;
            }
        }
        public string ExcludeDecryptionForTagLength
        {
            get
            {
                return excludeDecryptionForTagLength;
            }
            set
            {
                excludeDecryptionForTagLength = value;
            }
        }
        public bool ReaderIsForRechargeOnly
        {
            get
            {
                return readerIsForRechargeOnly;
            }
            set
            {
                readerIsForRechargeOnly = value;
            }
        }
    }
}

/********************************************************************************************
 * Project Name - Device
 * Description  - Factory class, handles creation of the mifare tag readers and barcode readers
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    /// <summary>
    /// Factory class, handles creation of the mifare tag readers and barcode readers
    /// </summary>
    public class DeviceFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<DeviceSubType, int> deviceSubTypeIndexDictionary = new Dictionary<DeviceSubType, int>()
        {
            { DeviceSubType.ACR1222L, 0},
            { DeviceSubType.ACR1252U, 0},
            { DeviceSubType.ACR122U, 0},
            { DeviceSubType.MIBlack, 0},
            { DeviceSubType.KeyboardWedge, 0}
        };
        int ACR1222LIndex;
        int ACR1252UIndex;
        int ACR122UIndex;
        int MIBlackIndex;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DeviceFactory()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates a device specified in the device defintion
        /// </summary>
        /// <param name="deviceDefinition">specification of the device</param>
        /// <returns>device</returns>
        public DeviceClass CreateDevice(DeviceDefinition deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            DeviceClass result;
            if (deviceDefinition.DeviceAddress < 0)
            {
                deviceDefinition.DeviceAddress = deviceSubTypeIndexDictionary[deviceDefinition.DeviceSubType];
            }
            switch (deviceDefinition.DeviceSubType)
            {
                case DeviceSubType.ACR1222L:
                    {

                        result = new ACR1222L(deviceDefinition);
                        break;
                    }
                case DeviceSubType.ACR1252U:
                    {
                        result = new ACR1252U(deviceDefinition);
                        break;
                    }
                case DeviceSubType.ACR122U:
                    {
                        result = new ACR122U(deviceDefinition);
                        break;
                    }
                case DeviceSubType.MIBlack:
                    {
                        result = new MIBlack(deviceDefinition);
                        break;
                    }
                case DeviceSubType.KeyboardWedge:
                    {
                        if (IntPtr.Size == 4)
                        {
                            result = new KeyboardWedge32(deviceDefinition);
                        }
                        else
                        {
                            result = new KeyboardWedge64(deviceDefinition);
                        }
                        if ((result as USBDevice).isOpen == false)
                        {
                            throw new Exception("Unable to connect KeyboardWedge Device: " + deviceDefinition.Name);
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception("Invalid device definition. unable to create a device of sub type :" + deviceDefinition.DeviceSubType.ToString());
                    }
            }
            deviceSubTypeIndexDictionary[deviceDefinition.DeviceSubType] += 1;
            log.LogMethodExit(result);
            return result;
        }
    }
}

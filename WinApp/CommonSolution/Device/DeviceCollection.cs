/********************************************************************************************
 * Project Name - Device
 * Description  - Contains a list of peripherals attached the the system, devices can be tag readers, barcode readers
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         17-Nov-2020       Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    /// <summary>
    /// Contains a list of peripherals attached the the system, devices can be tag readers, barcode readers
    /// </summary>
    public class DeviceCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DeviceClass> deviceList = new List<DeviceClass>();
        private Dictionary<DeviceType, List<DeviceClass>> deviceTypeDeviceListMap = new Dictionary<DeviceType, List<DeviceClass>>()
        {
            {DeviceType.CardReader, new List<DeviceClass>()},
            {DeviceType.BarcodeReader, new List<DeviceClass>()},
        };
        private Dictionary<DeviceType, DeviceClass> deviceTypePrimaryDeviceMap = new Dictionary<DeviceType, DeviceClass>()
        {
            {DeviceType.CardReader, null},
            {DeviceType.BarcodeReader, null},
        };

        protected DeviceFactory deviceFactory;

        protected DeviceCollection()
        {
            log.LogMethodEntry();
            deviceFactory = new DeviceFactory();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="deviceDefinitionList">list of device definitions</param>
        public DeviceCollection(List<DeviceDefinition> deviceDefinitionList)
        {
            log.LogMethodEntry(deviceDefinitionList);
            deviceFactory = new DeviceFactory();
            foreach (var deviceDefinition in deviceDefinitionList)
            {
                if (deviceDefinition.IsDefault)
                {
                    if (deviceTypePrimaryDeviceMap[deviceDefinition.DeviceType] == null ||
                           deviceTypePrimaryDeviceMap[deviceDefinition.DeviceType].DeviceDefinition.DeviceSubType == deviceDefinition.DeviceSubType)
                    {
                        AddDevice(CreateDevice(deviceDefinition));
                    }
                }
                else
                {
                    AddDevice(CreateDevice(deviceDefinition));
                }
            }
            log.LogMethodExit();
        }

        protected DeviceClass CreateDevice(DeviceDefinition deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            DeviceClass device = null;
            try
            {
                device = deviceFactory.CreateDevice(deviceDefinition);
            }
            catch (Exception ex)
            {
                log.LogVariableState("deviceDefinition", deviceDefinition);
                log.Error("Error occurred while registering the device: " + deviceDefinition.Name, ex);
            }
            return device;
        }

        protected void AddDevice(DeviceClass device)
        {
            log.LogMethodEntry();
            if (device == null)
            {
                log.LogMethodExit(null, "device is null");
                return;
            }
            log.LogVariableState("device.DeviceDefinition", device.DeviceDefinition);
            deviceList.Add(device);
            deviceTypeDeviceListMap[device.DeviceDefinition.DeviceType].Add(device);
            if (deviceTypePrimaryDeviceMap[device.DeviceDefinition.DeviceType] == null)
            {
                deviceTypePrimaryDeviceMap[device.DeviceDefinition.DeviceType] = device;
            }
            log.LogMethodExit();
        }

        protected void RemoveDevice(DeviceClass device)
        {
            log.LogMethodEntry();
            if (device == null)
            {
                log.LogMethodExit(null, "device is null");
                return;
            }
            log.LogVariableState("device.DeviceDefinition", device.DeviceDefinition);
            deviceList.Remove(device);
            deviceTypeDeviceListMap[device.DeviceDefinition.DeviceType].Remove(device);
            if (deviceTypePrimaryDeviceMap[device.DeviceDefinition.DeviceType] == device)
            {
                deviceTypePrimaryDeviceMap[device.DeviceDefinition.DeviceType] = deviceTypeDeviceListMap[device.DeviceDefinition.DeviceType].FirstOrDefault();
            }
            try
            {
                device.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while disposing the device", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the primary device of a given device type
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>

        public DeviceClass GetPrimaryDevice(DeviceType deviceType)
        {
            return deviceTypePrimaryDeviceMap[deviceType];
        }

        /// <summary>
        /// Retuns all the devices of a given device type
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public IEnumerable<DeviceClass> GetDevices(DeviceType deviceType)
        {
            return deviceTypeDeviceListMap[deviceType];
        }

        /// <summary>
        /// retuns the first device matching the predicate or null
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DeviceClass GetDevice(Func<DeviceDefinition, bool> predicate)
        {
            var device = deviceList.FirstOrDefault(x => predicate(x.DeviceDefinition));
            return device;
        }

        /// <summary>
        /// returns the devices matching the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<DeviceClass> GetDevices(Func<DeviceDefinition, bool> predicate)
        {
            var matchingDeviceList = deviceList.Where(x => predicate(x.DeviceDefinition));
            return matchingDeviceList;
        }

        public virtual void Dispose()
        {
            log.LogMethodEntry();
            foreach (var device in deviceList)
            {
                try
                {
                    device.Dispose();
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while disposing the device", ex);
                }
            }
            deviceList.Clear();
            deviceTypeDeviceListMap.Clear();
            deviceTypePrimaryDeviceMap.Clear();
            log.LogMethodExit();
        }

    }
}

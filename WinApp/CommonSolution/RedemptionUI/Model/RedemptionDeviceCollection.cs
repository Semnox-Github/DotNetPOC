/********************************************************************************************
 * Project Name - CommonUI
 * Description  - Sub class of Device collection contains code for initializing Redemption Peripherals
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.200.0      17-Nov-2020      Lakshminarayana           Created : Redemption UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.RedemptionUI
{
    /// <summary>
    /// Redemption Device collection contains code for initializing Redemption Peripherals
    /// </summary>
    public class RedemptionDeviceCollection : DeviceCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private Dictionary<DeviceType, Dictionary<int, List<DeviceDefinition>>> deviceTypeDeviceDefinitionDictionary = new Dictionary<DeviceType, Dictionary<int, List<DeviceDefinition>>>()
        {
            {DeviceType.CardReader, new Dictionary<int, List<DeviceDefinition>>()},
            {DeviceType.BarcodeReader, new Dictionary<int, List<DeviceDefinition>>()},
        };
        private Dictionary<DeviceType, Dictionary<int, DeviceClass>> deviceTypeDeviceDictionary = new Dictionary<DeviceType, Dictionary<int, DeviceClass>>()
            {
            {DeviceType.CardReader, new Dictionary<int, DeviceClass>()},
            {DeviceType.BarcodeReader, new Dictionary<int, DeviceClass>()},
        };

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="handle">window handle</param>
        public RedemptionDeviceCollection(ExecutionContext executionContext, IntPtr handle)
        {
            log.LogMethodEntry(executionContext, handle);
            this.executionContext = executionContext;
            IEnumerable<MifareKeyContainerDTO> mifareKeyList = MifareKeyViewContainerList.GetMifareKeyContainerDTOList(executionContext);
            SiteContainerDTO siteContainerDTO = SiteViewContainerList.GetCurrentSiteContainerDTO(executionContext);
            int siteId = siteContainerDTO.SiteId;
            List<string> displayMessageLineList = new List<string>();
            string siteName = siteContainerDTO.SiteName;
            displayMessageLineList.Add(siteName);
            string tapTagMessage = MessageViewContainerList.GetMessage(executionContext, 257);
            displayMessageLineList.Add(tapTagMessage);


            string cardReaderSerialNumber = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "CARD_READER_SERIAL_NUMBER");
            var serialNumbers = cardReaderSerialNumber.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            int index = 0;
            foreach (var serialNumber in serialNumbers)
            {
                if (string.IsNullOrWhiteSpace(serialNumber))
                {
                    continue;
                }
                DeviceDefinition deviceDefinition = new DeviceDefinition("ACRU1252U-" + cardReaderSerialNumber, DeviceSubType.ACR1252U, mifareKeyList, siteId, false, displayMessageLineList, serialNumber);
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.CardReader, deviceDefinition, index);
                index++;
            }
            for (int i = 0; i < 8; i++)
            {
                DeviceDefinition deviceDefinition = new DeviceDefinition("ACR1222L-" + (i + 1), DeviceSubType.ACR1222L, mifareKeyList, siteId, false, displayMessageLineList);
                deviceDefinition.DeviceAddress = i;
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.CardReader, deviceDefinition, i);
                deviceDefinition = new DeviceDefinition("ACRU1252U-" + (i + 1), DeviceSubType.ACR1252U, mifareKeyList, siteId);
                deviceDefinition.DeviceAddress = i;
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.CardReader, deviceDefinition, i);
                deviceDefinition = new DeviceDefinition("ACRU122U-" + (i + 1), DeviceSubType.ACR122U, mifareKeyList, siteId);
                deviceDefinition.DeviceAddress = i;
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.CardReader, deviceDefinition, i);
                deviceDefinition = new DeviceDefinition("MIBlack-" + (i + 1), DeviceSubType.MIBlack, mifareKeyList, siteId);
                deviceDefinition.DeviceAddress = i;
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.CardReader, deviceDefinition, i);
            }
            List<DeviceDefinition> peripheralDeviceDefinitionList = GetPeripheralDevices(executionContext, mifareKeyList, siteId, displayMessageLineList, handle).ToList();
            index = 0;
            foreach (var deviceDefinition in peripheralDeviceDefinitionList.Where(x => x.DeviceType == DeviceType.CardReader))
            {
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.CardReader, deviceDefinition, index);
                index++;
            }
            foreach (var deviceDefinition in GetParafaitDefaultUSBCardReaderDevices(executionContext, handle))
            {
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.CardReader, deviceDefinition, index);
                index++;
            }

            index = 0;
            foreach (var deviceDefinition in peripheralDeviceDefinitionList.Where(x => x.DeviceType == DeviceType.BarcodeReader))
            {
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.BarcodeReader, deviceDefinition, index);
                index++;
            }
            foreach (var deviceDefinition in GetParafaitDefaultUSBBarcodeReaderDevices(executionContext, handle))
            {
                AddToDeviceTypeDeviceDefinitionDictionary(DeviceType.BarcodeReader, deviceDefinition, index);
                index++;
            }
            log.LogMethodExit();
        }

        private void AddToDeviceTypeDeviceDefinitionDictionary(DeviceType deviceType, DeviceDefinition deviceDefinition, int index)
        {
            log.LogMethodEntry(deviceType, deviceDefinition, index);
            if (deviceTypeDeviceDefinitionDictionary.ContainsKey(deviceType) == false)
            {
                log.LogMethodExit(null, "Invalid deviceType " + deviceType.ToString());
                return;
            }
            if (deviceTypeDeviceDefinitionDictionary[deviceType].ContainsKey(index) == false)
            {
                deviceTypeDeviceDefinitionDictionary[deviceType].Add(index, new List<DeviceDefinition>());
            }
            deviceTypeDeviceDefinitionDictionary[deviceType][index].Add(deviceDefinition);
            log.LogMethodExit();
        }

        private static IEnumerable<DeviceDefinition> GetPeripheralDevices(ExecutionContext executionContext, IEnumerable<MifareKeyContainerDTO> mifareKeyList, int siteId, List<string> displayMessageLineList, IntPtr handle)
        {
            log.LogMethodEntry(executionContext, mifareKeyList, siteId, displayMessageLineList, handle);
            POSMachineContainerDTO posMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(executionContext);
            if (posMachineContainerDTO.PeripheralContainerDTOList != null && posMachineContainerDTO.PeripheralContainerDTOList.Count > 0)
            {
                foreach (PeripheralContainerDTO peripheralContainerDTO in posMachineContainerDTO.PeripheralContainerDTOList.OrderBy(x => x.DeviceId))
                {
                    yield return new DeviceDefinition(peripheralContainerDTO.DeviceName, (DeviceType)Enum.Parse(typeof(DeviceType), peripheralContainerDTO.DeviceType), (DeviceSubType)Enum.Parse(typeof(DeviceSubType), peripheralContainerDTO.DeviceSubType), peripheralContainerDTO.Vid, peripheralContainerDTO.Pid, peripheralContainerDTO.OptionalString, false, handle, mifareKeyList, siteId, displayMessageLineList, peripheralContainerDTO.EnableTagDecryption, peripheralContainerDTO.ExcludeDecryptionForTagLength, peripheralContainerDTO.ReaderIsForRechargeOnly);
                }
            }
            log.LogMethodExit();
        }

        private static IEnumerable<DeviceDefinition> GetParafaitDefaultUSBBarcodeReaderDevices(ExecutionContext executionContext, IntPtr handle)
        {
            log.LogMethodEntry(executionContext, handle);
            string USBBarcodeReaderVID = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_VID");
            string USBBarcodeReaderPID = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_PID");
            string USBBarcodeReaderOptionalString = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_OPT_STRING");
            string[] barcodeOptStrings = USBBarcodeReaderOptionalString.Split('|');

            if (USBBarcodeReaderVID.Trim() != string.Empty)
            {
                foreach (string optValue in barcodeOptStrings)
                {
                    yield return new DeviceDefinition("USBBarcodeReader", DeviceType.BarcodeReader, USBBarcodeReaderVID, USBBarcodeReaderPID, optValue, handle);
                }
            }
            log.LogMethodExit();
        }

        private static IEnumerable<DeviceDefinition> GetParafaitDefaultUSBCardReaderDevices(ExecutionContext executionContext, IntPtr handle)
        {
            log.LogMethodEntry(executionContext, handle);
            string USBReaderVID = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "USB_READER_VID");
            string USBReaderPID = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "USB_READER_PID");
            string USBReaderOptionalString = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "USB_READER_OPT_STRING");
            string[] optStrings = USBReaderOptionalString.Split('|');
            if (USBReaderVID.Trim() != string.Empty)
            {
                foreach (string optValue in optStrings)
                {
                    yield return new DeviceDefinition("USBCardReader", DeviceType.CardReader, USBReaderVID, USBReaderPID, optValue, handle);
                }
            }
            log.LogMethodExit();
        }

        public DeviceClass GetDevice(DeviceType deviceType, int index)
        {
            log.LogMethodEntry(deviceType, index);
            string errorMessage;
            if (deviceTypeDeviceDictionary.ContainsKey(deviceType) == false ||
                deviceTypeDeviceDefinitionDictionary.ContainsKey(deviceType) == false)
            {
                errorMessage = "Invalid deviceType: " + deviceType.ToString() + ". Not supported.";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            if (deviceTypeDeviceDictionary[deviceType].ContainsKey(index))
            {
                RemoveDevice(deviceTypeDeviceDictionary[deviceType][index]);
                deviceTypeDeviceDictionary[deviceType].Remove(index);
            }
            if (deviceType == DeviceType.BarcodeReader)
            {
                errorMessage = MessageViewContainerList.GetMessage(executionContext, 2688);
            }
            else
            {
                errorMessage = MessageViewContainerList.GetMessage(executionContext, 281);
            }
            if (deviceTypeDeviceDefinitionDictionary[deviceType].ContainsKey(index) == false)
            {
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            DeviceClass result = null;
            foreach (var deviceDefinition in deviceTypeDeviceDefinitionDictionary[deviceType][index])
            {
                result = CreateDevice(deviceDefinition);
                if (result != null)
                {
                    deviceTypeDeviceDictionary[deviceType].Add(index, result);
                    AddDevice(result);
                    log.LogMethodExit(result);
                    return result;
                }
            }
            log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            throw new Exception(errorMessage);
        }

        public override void Dispose()
        {
            base.Dispose();
            deviceTypeDeviceDefinitionDictionary.Clear();
            deviceTypeDeviceDictionary.Clear();
        }
    }
}

/********************************************************************************************
 * Project Name - CommonUI
 * Description  - Sub class of Device collection contains code for initializing Redemption Peripherals
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : Redemption UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI.BaseUI.Model
{
    /// <summary>
    /// Redemption Device collection contains code for initializing Redemption Peripherals
    /// </summary>
    public class RedemptionDeviceCollection : DeviceCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="handle">window handle</param>
        public RedemptionDeviceCollection(ExecutionContext executionContext, IntPtr handle) : base(GetDeviceDefinitionList(executionContext, handle))
        {
            log.LogMethodEntry(executionContext, handle);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        private static List<DeviceDefinition> GetDeviceDefinitionList(ExecutionContext executionContext, IntPtr handle)
        {
            log.LogMethodEntry(executionContext, handle);
            IEnumerable<MifareKeyContainerDTO> mifareKeyList = MifareKeyViewContainerList.GetMifareKeyContainerDTOList(executionContext);
            SiteContainerDTO siteContainerDTO = SiteViewContainerList.GetCurrentSiteContainerDTO(executionContext);
            int siteId = siteContainerDTO.SiteId;
            List<string> displayMessageLineList = new List<string>();
            string siteName = siteContainerDTO.SiteName;
            displayMessageLineList.Add(siteName);
            string tapTagMessage = MessageContainerList.GetMessage(executionContext, 257);
            displayMessageLineList.Add(tapTagMessage);
            
            List<DeviceDefinition> diviceDefinitionList = new List<DeviceDefinition>();

            diviceDefinitionList.AddRange(GetPeripheralDevices(executionContext, mifareKeyList, siteId, displayMessageLineList, handle));
            diviceDefinitionList.AddRange(GetParafaitDefaultUSBCardReaderDevices(executionContext, handle));
            diviceDefinitionList.AddRange(GetParafaitDefaultUSBBarcodeReaderDevices(executionContext, handle));
            diviceDefinitionList.AddRange(GetMifareCardReaderDevices(executionContext, mifareKeyList, siteId, displayMessageLineList));
            
            log.LogMethodExit(diviceDefinitionList);
            return diviceDefinitionList;
        }

        private static IEnumerable<DeviceDefinition> GetPeripheralDevices(ExecutionContext executionContext, IEnumerable<MifareKeyContainerDTO> mifareKeyList, int siteId, List<string> displayMessageLineList, IntPtr handle)
        {
            log.LogMethodEntry(executionContext, mifareKeyList, siteId, displayMessageLineList, handle);
            POSMachineContainerDTO posMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(executionContext);
            if (posMachineContainerDTO.PeripheralContainerDTOList!= null && posMachineContainerDTO.PeripheralContainerDTOList.Count > 0)
            {
                foreach (PeripheralContainerDTO peripheralContainerDTO in posMachineContainerDTO.PeripheralContainerDTOList)
                {
                     yield return new DeviceDefinition(peripheralContainerDTO.DeviceName, (DeviceType)Enum.Parse(typeof(DeviceType), peripheralContainerDTO.DeviceType), (DeviceSubType)Enum.Parse(typeof(DeviceSubType),peripheralContainerDTO.DeviceSubType), peripheralContainerDTO.Vid, peripheralContainerDTO.Pid, peripheralContainerDTO.OptionalString, false, handle, mifareKeyList, siteId, displayMessageLineList, peripheralContainerDTO.EnableTagDecryption, peripheralContainerDTO.ExcludeDecryptionForTagLength);
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

        private static IEnumerable<DeviceDefinition> GetMifareCardReaderDevices(ExecutionContext executionContext, IEnumerable<MifareKeyContainerDTO> mifareKeyList, int siteId, List<string> displayMessageLineList)
        {
            log.LogMethodEntry(executionContext, mifareKeyList, siteId, displayMessageLineList);
            string cardReaderSerialNumber = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "CARD_READER_SERIAL_NUMBER");
            var serialNumbers = cardReaderSerialNumber.Split(new []{ '|'},StringSplitOptions.RemoveEmptyEntries);
            foreach (var serialNumber in serialNumbers)
            {
                if(string.IsNullOrWhiteSpace(serialNumber))
                {
                    continue;
                }
                yield return new DeviceDefinition("ACRU1252U-" + cardReaderSerialNumber, DeviceSubType.ACR1252U, mifareKeyList, siteId, false, displayMessageLineList, serialNumber);
            }
            for (int i = 0; i < 8; i++)
            {
                yield return new DeviceDefinition("ACR1222L-" + (i + 1), DeviceSubType.ACR1222L, mifareKeyList, siteId, false, displayMessageLineList);
                yield return new DeviceDefinition("ACRU1252U-" + (i + 1), DeviceSubType.ACR1252U, mifareKeyList, siteId);
                yield return new DeviceDefinition("ACRU122U-" + (i + 1), DeviceSubType.ACR122U, mifareKeyList, siteId );
                yield return new DeviceDefinition("MIBlack-" + (i + 1), DeviceSubType.MIBlack, mifareKeyList, siteId);
            }
            
            log.LogMethodExit();
        }
    }
}

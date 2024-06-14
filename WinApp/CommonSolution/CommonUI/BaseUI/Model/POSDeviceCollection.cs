/********************************************************************************************
 * Project Name - CommonUI
 * Description  - Sub class of Device collection contains code for initializing POS Peripherals
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.200.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
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

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// POS Device collection contains code for initializing POS Peripherals
    /// </summary>
    public class POSDeviceCollection : DeviceCollection
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        #endregion
        #region Methods
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="handle">window handle</param>
        public POSDeviceCollection(ExecutionContext executionContext, IntPtr handle) : base(GetDeviceDefinitionList(executionContext, handle))
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
            string tapTagMessage = MessageViewContainerList.GetMessage(executionContext, 257);
            displayMessageLineList.Add(tapTagMessage);

            List<DeviceDefinition> diviceDefinitionList = new List<DeviceDefinition>();

            diviceDefinitionList.AddRange(GetDefaultMifareCardReaderDevices(executionContext, mifareKeyList, siteId, displayMessageLineList));
            diviceDefinitionList.AddRange(GetDefaultUSBCardReaderDevices(executionContext, handle));
            diviceDefinitionList.AddRange(GetDefaultUSBBarcodeReaderDevices(executionContext, handle));
            diviceDefinitionList.AddRange(GetPeripheralDevices(executionContext, mifareKeyList, siteId, displayMessageLineList, handle));

            log.LogMethodExit(diviceDefinitionList);
            return diviceDefinitionList;
        }
        private static IEnumerable<DeviceDefinition> GetPeripheralDevices(ExecutionContext executionContext, IEnumerable<MifareKeyContainerDTO> mifareKeyList, int siteId, List<string> displayMessageLineList, IntPtr handle)
        {
            log.LogMethodEntry(executionContext, mifareKeyList, siteId, displayMessageLineList, handle);
            POSMachineContainerDTO posMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(executionContext);
            if (posMachineContainerDTO.PeripheralContainerDTOList != null && posMachineContainerDTO.PeripheralContainerDTOList.Count > 0)
            {
                foreach (PeripheralContainerDTO peripheralContainerDTO in posMachineContainerDTO.PeripheralContainerDTOList)
                {
                    yield return new DeviceDefinition(peripheralContainerDTO.DeviceName, (DeviceType)Enum.Parse(typeof(DeviceType), peripheralContainerDTO.DeviceType), (DeviceSubType)Enum.Parse(typeof(DeviceSubType), peripheralContainerDTO.DeviceSubType), peripheralContainerDTO.Vid, peripheralContainerDTO.Pid, peripheralContainerDTO.OptionalString, false, handle, mifareKeyList, siteId, displayMessageLineList, peripheralContainerDTO.EnableTagDecryption, peripheralContainerDTO.ExcludeDecryptionForTagLength, peripheralContainerDTO.ReaderIsForRechargeOnly);
                }
            }
            log.LogMethodExit();
        }
        private static IEnumerable<DeviceDefinition> GetDefaultUSBBarcodeReaderDevices(ExecutionContext executionContext, IntPtr handle)
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
                    yield return new DeviceDefinition("DefaultBarcodeReader", DeviceType.BarcodeReader, USBBarcodeReaderVID, USBBarcodeReaderPID, optValue, handle);
                }
            }
            log.LogMethodExit();
        }
        private static IEnumerable<DeviceDefinition> GetDefaultUSBCardReaderDevices(ExecutionContext executionContext, IntPtr handle)
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
                    yield return new DeviceDefinition("DefaultUSBCardReader", DeviceType.CardReader, USBReaderVID, USBReaderPID, optValue, handle, true);
                }
            }
            log.LogMethodExit();
        }
        private static IEnumerable<DeviceDefinition> GetDefaultMifareCardReaderDevices(ExecutionContext executionContext, IEnumerable<MifareKeyContainerDTO> mifareKeyList, int siteId, List<string> displayMessageLineList)
        {
            log.LogMethodEntry(executionContext, mifareKeyList, siteId, displayMessageLineList);
            string cardReaderSerialNumber = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "CARD_READER_SERIAL_NUMBER").Split('|')[0];
            yield return new DeviceDefinition("ACR1222L", DeviceSubType.ACR1222L, mifareKeyList, siteId, true, displayMessageLineList);
            yield return new DeviceDefinition(string.IsNullOrWhiteSpace(cardReaderSerialNumber) ? "ACRU1252U" : "ACRU1252U-" + cardReaderSerialNumber, DeviceSubType.ACR1252U, mifareKeyList, siteId, true, displayMessageLineList, cardReaderSerialNumber);
            yield return new DeviceDefinition("ACRU122U", DeviceSubType.ACR122U, mifareKeyList, siteId, true);
            yield return new DeviceDefinition("MIBlack", DeviceSubType.MIBlack, mifareKeyList, siteId, true);
            log.LogMethodExit();
        }
        #endregion
    }
}

/********************************************************************************************
 * Project Name - Common
 * Description  -  Class for Devices
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019     Girish Kundar        Modified : Added Logger methods and Removed unused namespace's 
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Parafait_POS.Common
{
    public static class Devices
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<DeviceClass> POSDevices = new List<DeviceClass>();
        public static List<DeviceClass> CardReaders = new List<DeviceClass>();
        public static DeviceClass PrimaryBarcodeScanner;
        public static DeviceClass PrimaryCardReader;

        public static void AddBarcodeScanner(DeviceClass device)
        {
            log.LogMethodEntry();
            POSDevices.Add(device);
            log.LogMethodExit();
        }

        public static void AddCardReader(DeviceClass device)
        {
            log.LogMethodEntry();
            CardReaders.Add(device);
            POSDevices.Add(device);
            log.LogMethodExit();
        }

        public static void RegisterCardReaders(EventHandler CardScanCompleteEventHandle)
        {
            log.LogMethodEntry();
            foreach (DeviceClass device in CardReaders)
                device.Register(CardScanCompleteEventHandle);
            log.LogMethodExit();
        }


        public static void RegisterPrimaryCardReader(EventHandler CardScanCompleteEventHandle)
        {
            log.LogMethodEntry();
            if (Common.Devices.PrimaryCardReader != null)
                Common.Devices.PrimaryCardReader.Register(CardScanCompleteEventHandle);
            log.LogMethodExit();
        }

        public static void RegisterLegacyCardReadEvent(EventHandler CardScanCompleteEventHandle)
        {
            log.LogMethodEntry();
            foreach (DeviceClass device in CardReaders)
                device.RegisterUnAuthenticated(CardScanCompleteEventHandle);
            log.LogMethodExit();
        }

        public static void UnregisterCardReaders()
        {
            log.LogMethodEntry();
            foreach (DeviceClass device in CardReaders)
                device.UnRegister();
            log.LogMethodExit();
        }

        public static void UnregisterPrimaryCardReader()
        {
            log.LogMethodEntry();
            if (Common.Devices.PrimaryCardReader != null)
                Common.Devices.PrimaryCardReader.UnRegister();
            log.LogMethodExit();
        }

        public static void UnRegisterUnAuthenticated()
        {
            log.LogMethodEntry();
            foreach (DeviceClass device in CardReaders)
                device.UnRegisterUnAuthenticated();
            log.LogMethodExit();
        }

        public static void DisposeAllDevices()
        {
            log.LogMethodEntry();
            foreach (DeviceClass device in POSDevices)
                device.Dispose();
            POSDevices.Clear();
            CardReaders.Clear();
            PrimaryCardReader = PrimaryBarcodeScanner = null;
            log.LogMethodExit();
        }
    }
}

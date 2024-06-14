using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Semnox.Parafait.Device;

namespace Semnox.Core.Utilities
{
    public static class Devices
    {
        public static List<DeviceClass> POSDevices = new List<DeviceClass>();
        public static List<DeviceClass> CardReaders = new List<DeviceClass>();
        public static DeviceClass PrimaryBarcodeScanner;
        public static DeviceClass PrimaryCardReader;

        public static void AddBarcodeScanner(DeviceClass device)
        {
            POSDevices.Add(device);
        }

        public static void AddCardReader(DeviceClass device)
        {
            CardReaders.Add(device);
            POSDevices.Add(device);
        }

        public static void RegisterCardReaders(EventHandler CardScanCompleteEventHandle)
        {
            foreach (DeviceClass device in CardReaders)
                device.Register(CardScanCompleteEventHandle);
        }

        public static void RegisterPrimaryCardReader(EventHandler CardScanCompleteEventHandle)
        {
            if (Devices.PrimaryCardReader != null)
                Devices.PrimaryCardReader.Register(CardScanCompleteEventHandle);
        }

        public static void UnregisterCardReaders()
        {
            foreach (DeviceClass device in CardReaders)
                device.UnRegister();
        }

        public static void UnRegisterUnAuthenticated()
        {
            foreach (DeviceClass device in CardReaders)
                device.UnRegisterUnAuthenticated();
        }

        public static void UnregisterPrimaryCardReader()
        {
            if (Devices.PrimaryCardReader != null)
                Devices.PrimaryCardReader.UnRegister();
        }

        public static void DisposeAllDevices()
        {
            foreach (DeviceClass device in POSDevices)
            {
                device.Dispose();
            }
            POSDevices.Clear();
            CardReaders.Clear();

            if (PrimaryBarcodeScanner != null)
            {
                PrimaryBarcodeScanner.Dispose();
            }
            if (PrimaryCardReader != null)
            {
                PrimaryCardReader.Dispose();
            }

            PrimaryCardReader = PrimaryBarcodeScanner = null;
        }
    }
}

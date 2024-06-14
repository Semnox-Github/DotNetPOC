using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.ServerCore
{
    public class MachineInputDevice
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum InputDeviceType
        {
            FingerPrint
        }

        public enum InputDeviceModel
        {
            FutronicFS84,
            Other
        }

        public enum FPTemplateFormats
        {
            ANSI,
            ISO,
            Futronic35,
            None
        }

        public InputDeviceType DeviceType;
        public InputDeviceModel DeviceModel;
        public string IPAddress;
        public int PortNo;
        public string MacAddress;
        public bool IsActive;
        public int DeviceId;
        public FPTemplateFormats FPTemplateFormat;

        public MachineInputDevice(int deviceId,
                                    InputDeviceType deviceType,
                                    InputDeviceModel deviceModel,
                                    string iPAddress,
                                    int portNo,
                                    string macAddress,
                                    FPTemplateFormats fPTemplateFormat)
        {
                log.LogMethodEntry(deviceId, deviceType, deviceModel, iPAddress, portNo, macAddress, fPTemplateFormat);
                DeviceId = deviceId;
                DeviceType = deviceType;
                DeviceModel = deviceModel;
                FPTemplateFormat = fPTemplateFormat;

                IPAddress = iPAddress;
                MacAddress = macAddress;
                PortNo = portNo;
                IsActive = true;
                log.LogMethodExit(null);
        }
    }
}

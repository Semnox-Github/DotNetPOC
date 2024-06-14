using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Biometric
{
    public interface iFingerPrintReader
    {
        byte[] CaptureTemplate();
        bool Verify(byte[] Template, byte[] refTemplate);
        void LedState(uint LedNoMask, bool OnOff);
        void LedBuzzerState(uint LedNoMask, bool LedOnOff, bool BuzzerOnOff);
        void LockState(bool OnOff);
        void Buzzer(int Interval);
    }
}

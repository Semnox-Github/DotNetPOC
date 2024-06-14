using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class FingerPrintReader
    {
        public enum MATCH_SCORE : int
        {
            MATCH_SCORE_LOW,
            MATCH_SCORE_LOW_MEDIUM,
            MATCH_SCORE_MEDIUM,
            MATCH_SCORE_HIGH_MEDIUM,
            MATCH_SCORE_HIGH,
            MATCH_SCORE_VERY_HIGH
        }

        public virtual byte[] CaptureTemplate()
        {
            return null;
        }

        public virtual bool Verify(byte[] Template, byte[] refTemplate)
        {
            return true;
        }

        public virtual void SetMatchScoreValue(MATCH_SCORE MatchScore)
        { }

        public virtual void LedState(uint LedNoMask, bool OnOff) { }
        public virtual void LedBuzzerState(uint LedNoMask, bool LedOnOff, bool BuzzerOnOff) { }
        public virtual void LockState(bool OnOff) { }
        public virtual void Buzzer(int Interval) { }
    }
}

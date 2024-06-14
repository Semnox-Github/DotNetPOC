/********************************************************************************************
 * Project Name - Device.Printer
 * Description  - Class for FingerPrintReader      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/

namespace Semnox.Parafait.Device.Printer
{
    public class FingerPrintReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;

        }

        public virtual bool Verify(byte[] Template, byte[] refTemplate)
        {
            log.LogMethodEntry(Template , refTemplate);
            log.LogMethodExit(true);
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

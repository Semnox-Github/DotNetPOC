/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - Coin Acceptor.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System.IO.Ports;

namespace Semnox.Parafait.KioskCore.CoinAcceptor
{
    public class CoinAcceptor
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private double amountRemainingToBeCollected;
        public enum Models { IMP10, UCA2, MICROCOIN_SP, HS636 };
        public SerialPort spCoinAcceptor;
        public int ReceivedCoinDenomination = 0;
        public bool criticalError = false;
        //public KioskStatic.acceptance acceptance = new KioskStatic.acceptance();
        internal string portName;
        public delegate void dataReceivedDelegate();
        public dataReceivedDelegate dataReceivedEvent = null;
        public double AmountRemainingToBeCollected { get { return amountRemainingToBeCollected; } set { amountRemainingToBeCollected = value; } }

        public virtual bool ProcessReceivedData(byte[] coinRec, ref string message)
        {
            log.LogMethodEntry(coinRec, message);
            log.LogMethodExit(false);
            return false;
        }

        public virtual void disableCoinAcceptor()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual void checkStatus()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual bool set_acceptance(bool isTokenMode = false)
        {
            log.LogMethodEntry(isTokenMode);
            log.LogMethodExit(true);
            return true;
        }

        
        public virtual void OpenComm()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public virtual void CloseComm()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }
}

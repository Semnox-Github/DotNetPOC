/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - CardDispenser.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System.IO.Ports;
using System.Threading;

namespace Semnox.Parafait.KioskCore.CardDispenser
{
    public class CardDispenser
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum Models { K720, SCT0M0, K720RF };
        public bool dispenserWorking = false;
        public bool cardLowlevel = false;
        public SerialPort spCardDispenser;
        internal string portName;
        public bool criticalError = false;

        protected string _CardNumber;
        protected Semnox.Core.Utilities.ExecutionContext executionContext; 

        public CardDispenser(string serialPortNum)
        {
            log.LogMethodEntry(serialPortNum);
            executionContext = KioskStatic.Utilities.ExecutionContext;
            portName = "COM" + serialPortNum;
            log.LogMethodExit();
        }

        public void HandleDispenserCardRead(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            _CardNumber = CardNumber;
            KioskStatic.logToFile("Reading dispensed Card: " + _CardNumber);
            log.LogMethodExit();
        }

        public virtual bool checkStatus(ref int cardPosition, ref string message)
        {
            log.LogMethodEntry(cardPosition, message);
            log.LogMethodExit(true);
            return true;
        }

        protected virtual bool dispenseCard(ref string message, ref string cardNumber)
        {
            log.LogMethodEntry(message, cardNumber);
            log.LogMethodExit(true);
            return true;
        }

        protected virtual bool captureCard(ref string message)
        {
            log.LogMethodEntry(message);
            log.LogMethodExit(true);
            return true;
        }

        public virtual bool ejectCard(ref string message)
        {
            log.LogMethodEntry(message);
            log.LogMethodExit(true);
            return true;
        }

        public virtual void DispenseToPosition(string position)
        {
            log.LogMethodEntry(position);
            log.LogMethodExit(true);
        }

        public virtual void reset()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public bool doDispenseCard(ref string cardNumber, ref string message)
        {
            log.LogMethodEntry(cardNumber, message);
            int cardPosition = -1;
            _CardNumber = "";
            if (checkStatus(ref cardPosition, ref message))
            {
                if (cardPosition > 1)
                {
                    if (!doRejectCard(ref message))
                        return false;

                    if (!checkStatus(ref cardPosition, ref message))
                        return false;
                }

                if (cardPosition != 1) // make sure card is prepared
                {
                    int retries = 4;
                    while (retries-- > 0)
                    {
                        Thread.Sleep(300);
                        if (checkStatus(ref cardPosition, ref message))
                        {
                            if (cardPosition == 1)
                                break;
                        }
                    }

                    if (cardPosition != 1)
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                if (!dispenseCard(ref message, ref cardNumber))
                {
                    log.LogMethodExit(false);
                    return false;
                }

                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool doEjectCard(ref string message)
        {
            log.LogMethodEntry(message);
            int cardPosition = -1;
            if (checkStatus(ref cardPosition, ref message))
            {                
                if (!ejectCard(ref message))
                {
                    log.LogMethodExit(false);
                    return false;
                }

                while (true)
                {
                    Thread.Sleep(300);
                    if (checkStatus(ref cardPosition, ref message))
                    {
                        if (cardPosition >= 2)
                        {
                            KioskStatic.logToFile("Card dispensed. Waiting to be removed.");
                            log.LogMethodExit(true);
                            return true;
                        }
                        else if (cardPosition < 0 && dispenserWorking) // wait for next card to be ready
                            continue;
                        else
                        {
                            KioskStatic.logToFile("Card removed.");
                            break;
                        }
                    }
                    else
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool doRejectCard(ref string message)
        {
            log.LogMethodEntry(message);
            int cardPosition = -1;
            if (checkStatus(ref cardPosition, ref message))
            {
                if (cardPosition != 2)
                {
                    log.LogMethodExit(true);
                    return true;
                }

                if (!captureCard(ref message))
                {
                    log.LogMethodExit(false);
                    return false;
                }
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
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

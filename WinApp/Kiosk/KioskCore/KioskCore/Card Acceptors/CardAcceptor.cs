/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - CardAcceptor.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.KioskCore.CardAcceptor
{
    public class CardAcceptor
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum Models { SK310UR04 };
        public UInt32 deviceHandle;
        internal string portName;
        internal string message;
        public string Message
        {
            get { return message; }
        }
        public virtual bool OpenComm(string PortNumber, uint BaudRate, ref string message)
        {
            log.LogMethodEntry(PortNumber, BaudRate, message);
            log.LogMethodExit(true);
            return true;
        }

        public virtual void OpenComm()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        //public virtual bool OpenComm(string PortNumber, uint BaudRate, ref string message)
        //{
        //    return true;
        //}

        public virtual void CloseComm()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual bool Initialize(ref string message)
        {
            log.LogMethodEntry(message);
            log.LogMethodExit(true);
            return true;
        }

        public virtual void AllowAllCards()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual void BlockAllCards()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual void GetCardStatus(ref int Position, ref string message)
        {
            log.LogMethodEntry(Position, message);
            log.LogMethodExit();
        }

        public virtual void EjectCardFront(bool hold = false)
        {
            log.LogMethodEntry(hold);
            log.LogMethodExit();
        }
    }
}

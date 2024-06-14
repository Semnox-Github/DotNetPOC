/********************************************************************************************
 * Project Name - Device.Turnstile
 * Description  - Class for  of TurnstileClass      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Device.Turnstile
{
    public abstract class TurnstileClass : IDisposable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public virtual void Dispose()
        {

        }

        ~TurnstileClass()
        {
            Dispose();
        }

        public enum TunstileMake
        {
            TISO,
            Other
        }

        public enum TunstileModel
        {
            Centurion,
            Twix,
            Bastion,
            SpeedBlade,
            Sweeper,
            Sesame,
            Swing,
            Other
        }

        public enum TunstileType
        {
            Tripod,
            FlapBarrier,
            Swing,
            DropArm,
            FullHeight
        }        

        public class turnstileData
        {
            public bool Connected;
            public bool SingleA;
            public bool SingleB;
            public bool FreeA;
            public bool FreeB;
            public bool LockA;
            public bool LockB;
            public bool Panic;
            public bool Alarm;

            public turnstileData()
            { }

            public turnstileData(byte[] data)
            { }
        }
        public turnstileData TurnstileData = new turnstileData();

        public enum Status
        {
            Success = 1,
            Failed = 2
        }

        public class turnstileStatus
        {
            public turnstileStatus(Status status)
            {
                log.LogMethodEntry(status);
                Status = status;
                log.LogMethodExit();
            }
            public turnstileStatus(Status status, bool error, string Message)
            {
                log.LogMethodEntry(status , error, Message);
                Status = status;
                Error = error;
                ErrorMessage = Message;
                log.LogMethodExit();
            }
            public bool Error { get; set; }
            public string ErrorMessage { get; set; }
            public Status Status { get; set; }
        }

        public delegate void statusDelegate(turnstileStatus Status);
        public event statusDelegate TurnstileStatusEvent;

        public delegate void dataDelegate(turnstileData Data);
        public event dataDelegate TurnstileDataEvent;

        protected virtual void myTurnstileStatusEvent(turnstileStatus Status)
        {
            if (TurnstileStatusEvent != null)
                TurnstileStatusEvent(Status);
        }

        protected virtual void myTurnstileDataEvent(turnstileData Data)
        {
            if (TurnstileDataEvent != null)
                TurnstileDataEvent(Data);
        }

        #region Command

        public virtual bool Connect() 
        {
            log.LogMethodEntry();
            log.LogMethodExit(true);
            return true; 
        }

        public virtual void Disconnect() { }

        /// <summary>
        /// Command activates the passage A at the time set in the turnstile (5 seconds)
        /// </summary>
        public virtual void SingleA()
        { }

        /// <summary>
        /// Command activates the passage B at the time set in the turnstile (5 seconds)
        /// </summary>
        public virtual void SingleB()
        { }

        /// <summary>
        /// Command activates the passage A on the retention time in the active state
        /// </summary>
        public virtual void FreeA()
        { }

        /// <summary>
        /// Command activates the passage B on the retention time in the active state 
        /// </summary>
        public virtual void FreeB()
        { }

        /// <summary>
        /// Command deactivates the passage A on the retention time in the active state
        /// </summary>
        public virtual void CancelFreeA()
        { }

        /// <summary>
        /// Command deactivates the passage B on the retention time in the active state 
        /// </summary>
        public virtual void CancelFreeB()
        { }

        /// <summary>
        /// Command lock passage A
        /// </summary>        
        public virtual void LockA()
        { }

        /// <summary>
        /// Command lock passage B
        /// </summary>        
        public virtual void LockB()
        { }

        /// <summary>
        /// Command Unlock passage A
        /// </summary>
        public virtual void UnlockA()
        { }

        /// <summary>
        /// Command Unlock passage B
        /// </summary>
        public virtual void UnlockB()
        { }

        /// <summary>
        /// Command activate and deactivate "Panic" state or activate in rs mode
        /// </summary>
        public virtual void Panic()
        { }

        /// <summary>
        /// Command deactivate "Panic" state
        /// </summary>
        public virtual void PanicOff()
        { }

        public virtual void GetStatus()
        { }

        public virtual void SetParameters(string TurnstileIP, int TurnstilePort)
        { }

        #endregion
    }
}

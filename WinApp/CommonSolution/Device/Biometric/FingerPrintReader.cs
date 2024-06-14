/********************************************************************************************
 * Project Name - Device.Biometric
 * Description  - Class for FingerPrintReader      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar     Modified : Added Logger Methods.
 *2.80          09-Apr-2020   Indrajeet Kumar   Modified : Base Method to support both Futronic and Morpho Device
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading;

namespace Semnox.Parafait.Device.Biometric
{
    // Delegate that invokes to display the message.
    public delegate void MessagePrint(int status);

    // Delegate that invokes to display the Live Aquisition.
    //public delegate IntPtr LiveFingerPrint(IntPtr Handle);

    // Delegate that invokes to display the QualityProgress.
    public delegate void QualityProgress(byte quality);

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
        /// <summary>
        /// Delegate instance to display message.
        /// </summary>
        protected MessagePrint messagePrint;

        protected IntPtr liveFingerPrint;

        protected SynchronizationContext synContext;
        protected event EventHandler morphoInputHandler;
        /// <summary>
        /// Delegate instance to QualityProgress.
        /// </summary>
        protected QualityProgress qualityProgress;        

        protected List<EventHandler> callBackList = new List<EventHandler>();
        public List<EventHandler> GetCallBackList { get { return callBackList; } }

        public virtual void Initialize(int portno, string portaddress, MessagePrint messagePrint, IntPtr liveFingerPrint, QualityProgress qualityProgress)
        {
            log.LogMethodEntry(portno, portaddress, messagePrint, liveFingerPrint, qualityProgress);
            this.messagePrint = messagePrint;
            this.liveFingerPrint = liveFingerPrint;
            this.qualityProgress = qualityProgress;
            log.LogMethodExit();
        }

        public virtual void Register(EventHandler callBackEventHandler)
        {
            log.LogMethodEntry();
            callBackList.Add(callBackEventHandler);
            morphoInputHandler = callBackEventHandler;
            synContext = SynchronizationContext.Current;
            log.Debug("SynContext Value" + synContext);
            log.LogVariableState("callBackList", callBackList);
            log.Info("callBackList count" + callBackList.Count.ToString());
            log.LogMethodExit();
        }


        public virtual void UnRegister()
        {
            log.LogMethodEntry();
            if (callBackList.Count > 1)
            {
                callBackList.RemoveAt(callBackList.Count - 1);
                morphoInputHandler = callBackList[callBackList.Count - 1];
                log.LogVariableState("Target handler: ", morphoInputHandler.Target);
            }
            log.LogVariableState("callBackList", callBackList);
            log.Info("callBackList count" + callBackList.Count.ToString());
            log.LogMethodExit();
        }        

        protected void FireFingerPrintScanCompleteEvent(byte[] fingerPrintScannedValue)
        {
            log.LogMethodEntry(fingerPrintScannedValue);
            synContext.Post(new SendOrPostCallback(delegate (object state)
            {
                log.Debug("MorphoInputHandler Value: " + morphoInputHandler + "MorphoInputHandler.Target Value: " + morphoInputHandler.Target);
                if (morphoInputHandler != null && morphoInputHandler.Target != null)
                {
                    log.LogVariableState("Call back method: ", morphoInputHandler.Method.ToString() + " " + morphoInputHandler.Target);
                    if (morphoInputHandler.Target.GetType().BaseType.ToString().Contains("Form"))
                    {
                        System.Windows.Forms.Form f = morphoInputHandler.Target as System.Windows.Forms.Form;
                        if (f != null && (f.IsDisposed || f.Disposing || f.Visible == false))
                        {
                            UnRegister();
                        }
                    }
                }

                var handler = morphoInputHandler;
                log.Debug("Handler Value: " + handler);
                if (handler != null)
                {
                    handler(this, new MorphoScannedEventArgs(fingerPrintScannedValue));
                }
            }), null);
            synContext.OperationCompleted();
        }
        public virtual byte[] Scan()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }
        public virtual bool Verify(List<UserFingerPrintDetailDTO> userFingerPrintDetailDTOList, byte[] refTemplate)
        {
            log.LogMethodEntry(userFingerPrintDetailDTOList, refTemplate);
            log.LogMethodExit(false);
            return false;
        }
        public virtual UserFingerPrintDetailDTO Identify(List<UserFingerPrintDetailDTO> userFingerPrintDetailDTOList, byte[] scannedTemplate)
        {
            log.LogMethodEntry(userFingerPrintDetailDTOList, scannedTemplate);
            log.LogMethodExit();
            return null;
        }
        public virtual void Dispose()
        {
            log.LogMethodEntry();
            callBackList.Clear();
            log.LogMethodExit();
        }

        public virtual void SetMatchScoreValue(MATCH_SCORE MatchScore)
        { }
        public virtual void LedState(uint LedNoMask, bool OnOff) { }
        public virtual void LedBuzzerState(uint LedNoMask, bool LedOnOff, bool BuzzerOnOff) { }
        public virtual void LockState(bool OnOff) { }
        public virtual void Buzzer(int Interval) { }
    }

    public class MorphoScannedEventArgs : EventArgs
    {
        public byte[] fingerPrintTemplate
        {
            get;
            private set;
        }

        public MorphoScannedEventArgs(byte[] scannedValue)
        {
            this.fingerPrintTemplate = scannedValue;
        }
    }
}

/********************************************************************************************
 * Project Name - Utilities
 * Description  - Base class of all the card readers
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        1-Jul-2019      Lakshminarayana     Modified to support ULC Cards 
 *2.70       30-Jul-2019      Mathew Ninan    Added disconnect method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Semnox.Core.Utilities
{
    public class DeviceClass : NativeWindow
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool defaultKeyChanged
        {
            get;
            protected set;
        }

        public bool isRoamingCard
        {
            get;
            protected set;
        }

        public int TagSiteId
        {
            get;
            protected set;
        }

        protected SynchronizationContext synContext;
        protected event EventHandler deviceInputHandler;
        protected event EventHandler deviceNonreadableHandler;
        protected DeviceDefinition deviceDefinition;

        public DeviceDefinition DeviceDefinition
        {
            get
            {
                return deviceDefinition;
            }
            set
            {
                deviceDefinition = value;
            }
        }
        public EventHandler DeviceInputHandler
        {
            get { return deviceInputHandler; }
        }

        public EventHandler DeviceNonreadableHandler
        {
            get { return deviceNonreadableHandler; }
        }

        protected List<EventHandler> callBackList = new List<EventHandler>();

        public List<EventHandler> GetCallBackList { get { return callBackList; } }
        public virtual void Register(EventHandler callBackEventHandler)
        {
            log.LogMethodEntry();
            callBackList.Add(callBackEventHandler);
            deviceInputHandler = callBackEventHandler;
            log.LogVariableState("callBackList", callBackList);
            log.Info("callBackList count" + callBackList.Count.ToString());
            startListener();
            log.LogMethodExit();
        }

        public virtual void RegisterUnAuthenticated(EventHandler callBackEventHandler)
        {
            deviceNonreadableHandler = callBackEventHandler;
        }

        public virtual void UnRegisterUnAuthenticated()
        {
            deviceNonreadableHandler = null;
        }

        public DeviceClass()
        {
            log.LogMethodEntry();
            defaultKeyChanged = true;
            isRoamingCard = true;
            log.LogMethodExit();
        }

        public DeviceClass(DeviceDefinition deviceDefinition) : this()
        {
            log.LogMethodEntry(deviceDefinition);
            this.deviceDefinition = deviceDefinition;
            log.LogMethodExit();
        }

        public virtual void UnRegister()
        {
            log.LogMethodEntry();
            if (callBackList.Count > 1)
            {
                callBackList.RemoveAt(callBackList.Count - 1);
                deviceInputHandler = callBackList[callBackList.Count - 1];
                log.LogVariableState("Target handler: ", deviceInputHandler.Target);
            }
            log.LogVariableState("callBackList", callBackList);
            log.Info("callBackList count" + callBackList.Count.ToString());
            log.LogMethodExit();
        }

        public virtual void Dispose()
        {
            callBackList.Clear();
            //deviceInputHandler = null;
        }
        protected void FireDeviceReadCompleteEvent(string deviceScannedValue)
        {
            log.LogMethodEntry(deviceScannedValue);
            synContext?.Post(new SendOrPostCallback(delegate (object state)
            {
                if (deviceInputHandler != null && deviceInputHandler.Target != null)
                {
                    log.LogVariableState("Call back method: ", deviceInputHandler.Method.ToString() + " " + deviceInputHandler.Target);
                    if (deviceInputHandler.Target.GetType().BaseType.ToString().Contains("Form"))
                    {
                        System.Windows.Forms.Form f = deviceInputHandler.Target as System.Windows.Forms.Form;
                        if (f != null && (f.IsDisposed || f.Disposing || f.Visible == false))
                        {
                            UnRegister();
                        }
                    }
                }

                var handler = deviceInputHandler;

                if (handler != null)
                {
                    handler(this, new DeviceScannedEventArgs(deviceScannedValue));
                }
            }), null);
            synContext?.OperationCompleted();
        }

        protected void FireDeviceReadUnAuthenticateEvent(string deviceScannedValue)
        {
            synContext?.Post(new SendOrPostCallback(delegate (object state)
            {
                if (deviceNonreadableHandler != null && deviceNonreadableHandler.Target != null)
                {
                    if (deviceNonreadableHandler.Target.GetType().BaseType.ToString().Contains("Form"))
                    {
                        System.Windows.Forms.Form f = deviceNonreadableHandler.Target as System.Windows.Forms.Form;
                        if (f != null && (f.IsDisposed || f.Disposing || f.Visible == false))
                            UnRegister();
                    }
                }

                var handler = deviceNonreadableHandler;

                if (handler != null)
                {
                    handler(this, new DeviceScannedEventArgs(deviceScannedValue));
                }
            }), null);

            synContext?.OperationCompleted();
        }

        public virtual string readCardNumber()
        {
            return "";
        } 
        public virtual string readValidatedCardNumber()
        {
            return string.Empty;
        }

        public virtual void disconnect()
        {

        }

        public virtual void startListener()
        {

        }

        public virtual void stopListener()
        {

        }

        public virtual bool read_data_basic_auth(int blockAddress, int numberOfBlocks, ref byte[] currentKey, ref byte[] paramReceivedData, ref string message)
        {
            return true;
        }

        public virtual bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey, ref byte[] paramReceivedData, ref string message)
        {
            return true;
        }

        public virtual bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData, ref string message)
        {
            return true;
        }

        public virtual bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
            return true;
        }

        public virtual void Authenticate(byte blockAddress, byte[] key)
        {
        }

        public virtual void beep(int duration, bool asynchronous)
        {
        }

        public virtual void beep(int duration = 1)
        {
        }

        public virtual string getSerialNumber()
        {
            return "";
        }

        public virtual string setSerialNumber(string serialNumber)
        {
            return "";
        }

        public virtual void DisplayMessage(params string[] Lines)
        {
        }

        public virtual void DisplayMessage(int LineNumber, string Message)
        {
        }

        public virtual void ClearDisplay()
        {
        }

        public virtual CardType CardType
        {
            get { return CardType.UNKNOWN; }
        }
    }

    public class DeviceScannedEventArgs : EventArgs
    {
        public string Message
        {
            get;
            private set;
        }

        public DeviceScannedEventArgs(string deviceScannedValue)
        {
            this.Message = deviceScannedValue;
        }
    }
}

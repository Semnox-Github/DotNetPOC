/********************************************************************************************
 * Project Name - Device
 * Description  - StimaCLSLib
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2      08-Aug-2019       Deeksha        Added Logger Methods.
 ********************************************************************************************/
using System;
using System.IO.Ports;
using System.Threading;

namespace Semnox.Parafait.Device
{
    public class SynchReceiveCOMPort
    {
        public SerialPort comPort;
        public string ReceivedData = "";
        public delegate void ReceiveAction();

        private ReceiveAction receiveAction;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SynchReceiveCOMPort(int portNumber, int BaudRate)
        {
            log.LogMethodEntry("portNumber", BaudRate);
            newCOMPort(portNumber, BaudRate);
            log.LogMethodExit();
        }

        public SynchReceiveCOMPort(int portNumber)
        {
            log.LogMethodEntry("portNumber");
            newCOMPort(portNumber, 9600);
            log.LogMethodExit();
        }

        private void newCOMPort(int portNumber, int BaudRate)
        {
            log.LogMethodEntry("portNumber", BaudRate);
            comPort = new SerialPort("COM" + portNumber.ToString());
            comPort.BaudRate = BaudRate;
            comPort.Parity = Parity.None;
            comPort.ReadTimeout = 3000;
            comPort.WriteTimeout = 1000;
            comPort.ReceivedBytesThreshold = 3;
            comPort.DiscardNull = false;
            log.LogMethodExit();
        }

        public bool Open()
        {
            log.LogMethodEntry();
            try
            {
                comPort.Open();
                comPort.DiscardInBuffer();
                log.LogMethodExit(true);
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Error occurred  while executing Open()", ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        public void Close()
        {
            log.LogMethodEntry();
            if (comPort.IsOpen)
                comPort.Close();
            log.LogMethodExit();
        }

        public ReceiveAction setReceiveAction
        {
            get
            {
                return receiveAction;
            }
            set
            {
                receiveAction = value;
            }
        }        

        public void WriteData(string data)
        {
            log.LogMethodEntry(data);
            comPort.DiscardInBuffer();
            comPort.DiscardOutBuffer();
            comPort.Write(data);

            string tempReceivedData = "";

            try
            {
                byte b = Convert.ToByte(comPort.ReadByte());
                tempReceivedData = Convert.ToChar(b).ToString();
                while (true)
                {
                    Thread.Sleep(20);
                    tempReceivedData = tempReceivedData + comPort.ReadExisting();
                    Thread.Sleep(5);
                    if (comPort.BytesToRead == 0)
                        break;
                }

                ReceivedData = tempReceivedData.Replace("\r", "").Replace("\n", "");
                receiveAction.Invoke();
                log.LogMethodExit();
            }
            catch (TimeoutException ex)
            {
                ReceivedData = "Receive FAILED: ReceiveTimeout: " + ex.Message;
                receiveAction.Invoke();
                log.Error("Error while executing writeData", ex);
            }
            catch (Exception ex)
            {
                ReceivedData = "Receive FAILED: " + ex.Message;
                receiveAction.Invoke();
                log.Error("Error while executing writeData", ex);
            }
        }
    }
}
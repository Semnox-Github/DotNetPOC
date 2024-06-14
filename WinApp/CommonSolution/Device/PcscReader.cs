/*===========================================================================================
 * 
 *  Copyright (C)   : Advanced Card System Ltd
 * 
 *  File            : PcscReader.cs
 * 
 *  Description     : Contain methods and properties for generic implementation of pcsc reader
 * 
 *  Author          : Arturo Salvamante
 *  
 *  Date            : June 03, 2011
 * 
 *  Revision Traile : [Author] / [Date if modification] / [Details of Modifications done] 
 * =========================================================================================
 *  Modified to add Logger Methods by Deeksha on 09-Aug-2019
 * =========================================================================================*/

using System;
using System.Linq;


namespace Semnox.Parafait.Device
{
    internal delegate void TransmitApduDelegate(object sender, TransmitApduEventArg e);

    internal class TransmitApduEventArg : EventArgs
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private byte[] data_;
        public byte[] data
        {
            get { return data_; }
            set { data_ = value; }
        }

        public TransmitApduEventArg(byte[] data)
        {
            log.LogMethodEntry(data);
            data_ = data;
            log.LogMethodExit();
        }

        public string GetAsString(bool spaceinBetween)
        {
            log.LogMethodEntry(spaceinBetween);
            if (data_ == null)
            {
                log.LogMethodExit();
                return "";
            }

            string tmpStr = string.Empty;
            for (int i = 0; i < data_.Length; i++)
            {
                tmpStr += string.Format("{0:X2}", data_[i]);

                if (spaceinBetween)
                    tmpStr += " ";
            }
            log.LogMethodExit(tmpStr);
            return tmpStr;
        }
    }

    internal class PcscReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IntPtr hCard_ = new IntPtr();
        private IntPtr hContext_ = new IntPtr();
        private int pProtocol_ = PcscProvider.SCARD_PROTOCOL_T0 | PcscProvider.SCARD_PROTOCOL_T1;
        private int pdwActiveProtocol_;
        private int shareMode_ = PcscProvider.SCARD_SHARE_SHARED;
        private uint _operationControlCode = 0;
        private string _readerName = "";
        private int lastError = 0;
        private Apdu _apduCommand = new Apdu();

        internal event TransmitApduDelegate OnSendCommand;
        internal event TransmitApduDelegate OnReceivedCommand;

        public PcscReader()
        {
            log.LogMethodEntry();
            establishContext();
            log.LogMethodExit();
        }

        public PcscReader(string readerName)
        {
            log.LogMethodEntry(readerName);
            _readerName = readerName;
            establishContext();
            log.LogMethodExit();
        }

        public IntPtr cardHandle
        {
            get { return hCard_; }
            set { hCard_ = value; }
        }

        public IntPtr resourceMngrContext
        {
            get { return hContext_; }
            set { hContext_ = value; }
        }

        public int preferedProtocol
        {
            get { return pProtocol_; }
            set { pProtocol_ = value; }
        }

        public int activeProtocol
        {
            get { return pdwActiveProtocol_; }
            set { pdwActiveProtocol_ = value; }
        }

        public int shareMode
        {
            get { return shareMode_; }
            set { shareMode_ = value; }
        }

        public string readerName
        {
            get { return _readerName; }
            set { _readerName = value; }
        }

        internal Apdu apduCommand
        {
            get { return _apduCommand; }
            set { _apduCommand = value; }
        }

        public PcscReader pcscConnection
        {
            get { return this; }
        }

        public uint operationControlCode
        {
            get { return _operationControlCode; }
            set { _operationControlCode = value; }
        }
        
        #region Private Methods

        void establishContext()
        {
            log.LogMethodEntry();
            int retCode;
            retCode = PcscProvider.SCardEstablishContext(PcscProvider.SCARD_SCOPE_USER, 0, 0, ref hContext_);
            if (retCode != PcscProvider.SCARD_S_SUCCESS)
                throw new Exception("Unable to establish context - " + PcscProvider.GetScardErrMsg(retCode));
            log.LogMethodExit();
        }

        void releaseContext()
        {
            log.LogMethodEntry();
            int retCode = PcscProvider.SCardReleaseContext(hContext_);
            if (retCode != PcscProvider.SCARD_S_SUCCESS)
                throw new PcscException(retCode);
            log.LogMethodExit();
        }

        void resetContext()
        {
            log.LogMethodEntry();
            releaseContext();
            establishContext();
            log.LogMethodExit();
        }

        #endregion

        public void connect()
        {
            log.LogMethodEntry();
            if (_readerName.Trim() == "")
                throw new Exception("Smartacard reader is not specified");

            connect(_readerName, pProtocol_, shareMode_);
            log.LogMethodExit();
        }

        public void connect(string readerName)
        {
            log.LogMethodEntry(readerName);
            _readerName = readerName;
            connect(_readerName, pProtocol_, shareMode_);
            log.LogMethodExit();
        }

        public void connect(string readerName, int preferedProtocol, int shareMode)
        {
            log.LogMethodEntry(readerName, preferedProtocol, shareMode);
            int returnCode;

            returnCode = PcscProvider.SCardConnect(hContext_, readerName, shareMode, preferedProtocol, ref hCard_, ref pdwActiveProtocol_);
            log.LogVariableState("Return code: ", returnCode.ToString());
            if (returnCode != PcscProvider.SCARD_S_SUCCESS)
            {
                log.LogVariableState("Card Connect return code: ", returnCode);
                lastError = returnCode;
                throw new PcscException(returnCode);
            }

            
            shareMode_ = shareMode;
            pProtocol_ = preferedProtocol;
            _readerName = readerName;
            log.LogMethodExit();
        }

        public void connectDirect()
        {
            log.LogMethodEntry();
            connect(readerName, PcscProvider.SCARD_PROTOCOL_UNDEFINED, PcscProvider.SCARD_SHARE_DIRECT);
            log.LogMethodExit();
        }
        
        public string[] getReaderList()
        {
            log.LogMethodEntry();
            byte[] returnData;
            byte[] sReaderGroup = null;
            string[] readerList = new string[0];
            string readerString = string.Empty;
            int returnCode;
            IntPtr hContext = new IntPtr();
            int readerCount = 255;

            returnCode = PcscProvider.SCardEstablishContext(PcscProvider.SCARD_SCOPE_USER, 0, 0, ref hContext);
            if (returnCode != PcscProvider.SCARD_S_SUCCESS)
            {
                lastError = returnCode;
                throw new PcscException(returnCode);
            }

            returnCode = PcscProvider.SCardListReaders(hContext_, null, null, ref readerCount);
            if (returnCode != PcscProvider.SCARD_S_SUCCESS)
            {
                lastError = returnCode;
                throw new PcscException(returnCode);
            }

            returnData = new byte[readerCount];

            returnCode = PcscProvider.SCardListReaders(hContext_, sReaderGroup, returnData, ref readerCount);
            if (returnCode != PcscProvider.SCARD_S_SUCCESS)
                throw new PcscException(returnCode);

            readerString = System.Text.ASCIIEncoding.ASCII.GetString(returnData).Trim('\0');
            readerList = readerString.Split('\0');
            log.LogMethodExit(readerList);
            return readerList;
        }
        
        public void disconnect()
        {
            log.LogMethodEntry();
            int returnValue = PcscProvider.SCardDisconnect(hCard_, PcscProvider.SCARD_UNPOWER_CARD);
            log.LogMethodExit(returnValue);
            if (returnValue != PcscProvider.SCARD_S_SUCCESS)
                throw new PcscException(returnValue);
            log.LogMethodExit();
        }

        internal void sendCommand(ref Apdu apdu)
        {
            log.LogMethodEntry(apdu);
            apduCommand = apdu;
            sendCommand();
            apdu = apduCommand;
            log.LogMethodExit();
        }

        public void sendCommand()
        {
            log.LogMethodEntry();
            byte[] sendBuff, recvBuff;
            int sendLen, recvLen, returnCode;
            PcscProvider.SCARD_IO_REQUEST ioRequest;

            ioRequest.dwProtocol = pdwActiveProtocol_;
            ioRequest.cbPciLength = 8;

            if (apduCommand.data == null)
                sendBuff = new byte[5];
            else
                sendBuff = new byte[5 + apduCommand.data.Length];

            recvLen = apduCommand.lengthExpected + 2;

            Array.Copy(new byte[] { apduCommand.instructionClass, apduCommand.instructionCode, apduCommand.parameter1, apduCommand.parameter2, apduCommand.parameter3 }, sendBuff, 5);

            if (apduCommand.data != null)
                Array.Copy(apduCommand.data, 0, sendBuff, 5, apduCommand.data.Length);

            sendLen = sendBuff.Length;

            apduCommand.statusWord = new byte[2];
            recvBuff = new byte[recvLen];

            sendCommandTriggerEvent(new TransmitApduEventArg(sendBuff));
            returnCode = PcscProvider.SCardTransmit(hCard_,
                                                ref ioRequest,
                                                sendBuff,
                                                sendLen,
                                                ref ioRequest,
                                                recvBuff,
                                                ref recvLen);
            if (returnCode == 0)
            {
                receivedCommandTriggerEvent(new TransmitApduEventArg(recvBuff.Take(recvLen).ToArray()));
                if (recvLen > 1)
                    Array.Copy(recvBuff, recvLen - 2, apduCommand.statusWord, 0, 2);

                if (recvLen > 2)
                {
                    apduCommand.response = new byte[recvLen - 2];
                    Array.Copy(recvBuff, 0, apduCommand.response, 0, recvLen - 2);
                }
            }
            else
            {
                throw new PcscException(returnCode);
            }
            log.LogMethodExit();
        }

        internal void sendCardControl(ref Apdu apdu, uint controlCode)
        {
            log.LogMethodEntry(apdu, controlCode);
            apduCommand = apdu;
            operationControlCode = controlCode;
            sendCardControl();
            apdu = apduCommand;
            log.LogMethodExit();
        }

        public void sendCardControl()
        {
            log.LogMethodEntry();
            byte[] sendBuff, recvbuff;
            int sendLen, recvLen, returnCode, actualLength = 0;
            PcscProvider.SCARD_IO_REQUEST ioRequest;

            ioRequest.dwProtocol = pdwActiveProtocol_;
            ioRequest.cbPciLength = 8;

            if (apduCommand.data == null)
                throw new Exception("No data specified");

            sendBuff = new byte[apduCommand.data.Length];
            recvLen = apduCommand.lengthExpected;

            Array.Copy(apduCommand.data, 0, sendBuff, 0, apduCommand.data.Length);

            sendLen = sendBuff.Length;

            apduCommand.statusWord = new byte[2];
            recvbuff = new byte[recvLen];

            sendCommandTriggerEvent(new TransmitApduEventArg(sendBuff));
            returnCode = PcscProvider.SCardControl(hCard_,
                                                operationControlCode,
                                                sendBuff,
                                                sendLen,
                                                recvbuff,
                                                recvbuff.Length,
                                                ref actualLength);

            if (returnCode == 0)
            {
                apduCommand.actualLengthReceived = actualLength;

                receivedCommandTriggerEvent(new TransmitApduEventArg(recvbuff.Take(actualLength).ToArray()));

                apduCommand.response = new byte[actualLength];
                try
                {
                    Array.Copy(recvbuff, 0, apduCommand.response, 0, actualLength);

                    if (actualLength > 1)
                        Array.Copy(recvbuff, actualLength - 2, apduCommand.statusWord, 0, 2);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured  while executing sendCardControl()", ex);
                }

                //if (apdu.actualLengthReceived >= 2)
                //{
                //    apdu.receiveData = new byte[actualLength - 2];
                //    Array.Copy(recvbuff, 0, apdu.receiveData, 0, actualLength - 2);
                //}
            }
            else
            {
                throw new PcscException(returnCode);
            }
            log.LogMethodExit();
        }

        public virtual byte[] getFirmwareVersion()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            throw new NotImplementedException();

        }
        
        void sendCommandTriggerEvent(TransmitApduEventArg e)
        {
            log.LogMethodEntry();
            if (OnSendCommand == null)
            {
                log.LogMethodExit();
                return;
            }

            OnSendCommand(this, e);
            log.LogMethodExit();
        }

        void receivedCommandTriggerEvent(TransmitApduEventArg e)
        {
            log.LogMethodEntry();
            if (OnReceivedCommand == null)
            {
                log.LogMethodExit();
                return;
            }

            OnReceivedCommand(this, e);
            log.LogMethodExit();
        }

    }
}

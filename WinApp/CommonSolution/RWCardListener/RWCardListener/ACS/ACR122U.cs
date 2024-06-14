/*=========================================================================================
'  Copyright(C):    Advanced Card Systems Ltd 
' 
'  Description:     This sample program outlines the steps on how to
'                   transact with Mifare 1K/4K cards using ACR128
'  
'  Author :         Daryl M. Rojas
'
'  Module :         ModWinscard.cs
'   
'  Date   :         June 18, 2008
'
'  Revision Trail:  (Date/Author/Description) 
'
'=========================================================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using Semnox.Parafait.RWCardListener.ACS;
using Semnox.Parafait.RWCardListener.ACS.Pcsc;
using Semnox.Parafait.RWCardListener.ACS.Readers.Pcsc;
using System.Threading;
using  Semnox.Parafait.RWCardListener;
using System.Collections;

namespace Semnox.Parafait.RWCardListener.ACS
{
    public class ACR122U : RWCardListener
    {
        private CHIP_TYPE currentChipType = CHIP_TYPE.UNKNOWN;
        private Acr1281UC1 acr1281UC1;
        private MifareClassic mifareClassic;
        private int _deviceAddress = 0;

        private event EventHandler CardReaderReadComplete;
        public ACR122U()
        {
            initialize();
            base.init();
        }

        public ACR122U(int DeviceAddress)
        {
            _deviceAddress = DeviceAddress;
            initialize();
            base.init();
        }

        public ACR122U(byte[] defaultKey)
        {
            initialize();
            authKeyArray = new List<byte[]>();
            authKeyArray.Add(defaultKey);
        }

        public override bool registerRWCardReader(ref string message, EventHandler currEventHandler)
        {
            message = "MiFare reader registered";
            CardReaderReadComplete = currEventHandler;

            start_cardListener();
            return true;
        }

        protected override void readWrite_CardListener()
        {
            while (RunThread == true)
            {
                cardNumber = readCardNumber();
                if (cardNumber != "")
                    break;
                Thread.Sleep(100);
            }

            if (cardNumber != "")
            {
                FireCardReadCompleteEvent(cardNumber);
            }
        }

        private void FireCardReadCompleteEvent(string CardReaderScannedValue)
        {
            synContext.Post(new SendOrPostCallback(delegate(object state)
            {
                var handler = CardReaderReadComplete;

                if (handler != null)
                {
                    handler(this, new MifareCardReaderScannedEventArgs(CardReaderScannedValue));
                }
            }), null);
            synContext.OperationCompleted();
        }

        public override bool read_data_basic_auth(int blockAddress, int numberOfBlocks, ref byte[] currentKey, ref byte[] paramReceivedData, ref string message)
        {
            foreach (byte[] key in authKeyArray)
            {
                if (read_data(blockAddress, numberOfBlocks, key, ref paramReceivedData, ref message))
                {
                    currentKey = key;
                    return true;
                }
                Thread.Sleep(50);
            }
            return false;
        }

        public override bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey, ref byte[] paramReceivedData, ref string message)
        {
            try
            {
                acr1281UC1.connect();
                mifareClassic = new MifareClassic(acr1281UC1.pcscConnection);
                Authenticate((byte)blockAddress, paramAuthKey);

                byte[] tempStr;
                paramReceivedData = new byte[numberOfBlocks * 16];
                int i = 0;
                while (i < numberOfBlocks)
                {
                    tempStr = mifareClassic.readBinary((byte)(blockAddress + i), 16);
                    Array.Copy(tempStr, 0, paramReceivedData, i * 16, 16);
                    i++;
                }

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public override bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData, ref string message)
        {
            byte[] buff = new byte[16];

            try
            {
                acr1281UC1.connect();
                mifareClassic = new MifareClassic(acr1281UC1.pcscConnection);
                Authenticate((byte)blockAddress, authKey);

                int i = 0;
                while (i < numberOfBlocks)
                {
                    Array.Copy(writeData, i * 16, buff, 0, 16);
                    mifareClassic.updateBinary((byte)(blockAddress + i), buff, 16);
                    i++;
                }
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public override string readCardNumber()
        {
            try
            {
                acr1281UC1.connect();
                mifareClassic = new MifareClassic(acr1281UC1.pcscConnection);

                currentChipType = acr1281UC1.getChipType();

                if (currentChipType != CHIP_TYPE.MIFARE_1K && currentChipType != CHIP_TYPE.MIFARE_4K)
                {
                    throw new ApplicationException("Card is not supported.\r\nPlease present Mifare Classic card");
                }

                byte[] card = acr1281UC1.getCardSerialNumber();

                string card_num = "";
                foreach (byte values in card)
                    card_num += String.Format("{0:X2}", values);

                if (card_num.Equals("00000000"))
                    card_num = "";

                return card_num;
            }
            catch
            {
                return "";
            }
        }

        public override bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
            try
            {
                byte[] dataBuffer = new byte[0];
                if (!read_data(blockAddress, 1, currentAuthKey, ref dataBuffer, ref message))
                    return false;

                for (int i = 0; i < 6; i++)
                    dataBuffer[i] = newAuthKey[i];

                mifareClassic.updateBinary((byte)blockAddress, dataBuffer, 16);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        /* Read-only reader */

        public override bool registerReadOnlyCardReader(ref string message, EventHandler currEventHandler)
        {
            message = "MiFare reader registered";
            CardReaderReadComplete = currEventHandler;

            start_readOnlycardListener();

            return true;
        }

        public override void readOnly_CardListener()
        {
            const int PURSE_BLOCK = 4;
            byte[] dataBuffer = new byte[16];
            bool response = false;
            string prevCardNumber = "";

            while (ReadOnlyRunThread == true)
            {
                cardNumber = readCardNumber();
                if (cardNumber != "")
                {
                    if (cardNumber != prevCardNumber)
                    {
                        foreach (byte[] key in authKeyArray)
                        {
                            Thread.Sleep(100);
                            try
                            {
                                Authenticate(PURSE_BLOCK, key);
                                response = true;
                            }
                            catch
                            {
                                response = false;
                            }

                            if (response)
                            {
                                if (bytesEqual(key, authKeyArray[0], 6) == false) // if other than default key was allowed, change key to default key
                                {
                                    string message = "";
                                    response = change_authentication_key(PURSE_BLOCK + 3, key, authKeyArray[0], ref message);
                                    if (!response)
                                        break;
                                    else
                                        beep();
                                }
                                prevCardNumber = cardNumber;
                                FireCardReadCompleteEvent(cardNumber);
                                beep();
                                break;
                            }
                        }
                        if (!response)
                            beep(1, false);
                    }
                }
                else
                {
                    prevCardNumber = cardNumber = "";
                }

                Thread.Sleep(300);
            }
        }

        public override void beep(int repeat, bool asynchronous)
        {
            if (firstTime)
            {
                disableBeepOnCardDetect();
                firstTime = false;
            }
            Apdu apduCommand = new Apdu();
            apduCommand.setCommand(new byte[]{ 0xFF,            //Instruction Class
                                               0x00,            //Instruction Code
                                               0x40,            //RFU
                                               0x50,            //RFU
                                               0x04});

            //LED / buzzer control Data Bytes
            apduCommand.data = new byte[] {  0x01,            
                                             0x00,            
                                             (byte)repeat, 
                                             0x01};

            Thread thr = new Thread(() =>
            {
                try
                {
                    acr1281UC1.sendCommand(ref apduCommand);
                }
                catch { }
            });

            thr.Start();

            if (!asynchronous)
                thr.Join();
        }

        public override void beep(int repeat = 1)
        {
            beep(repeat, true);
        }

        bool firstTime = true;
        void disableBeepOnCardDetect()
        {
            Apdu apduC = new Apdu();
            apduC.setCommand(new byte[]{ 0xFF,            //Instruction Class
                                        0x00,            //Instruction Code
                                        0x52,            //RFU
                                        0x00,            //RFU
                                        0x00});

            try
            {
                acr1281UC1.sendCommand(ref apduC);
                //acr1281UC1.sendCardControl(ref apduC, (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4);
            }
            catch { }
        }

        private void initialize()
        {
            try
            {
                string[] readerList;

                acr1281UC1 = new Acr1281UC1();

                //Register to event OnReceivedCommand
                acr1281UC1.OnReceivedCommand += new TransmitApduDelegate(acr1281UC1_OnReceivedCommand);

                //Register to event OnSendCommand
                acr1281UC1.OnSendCommand += new TransmitApduDelegate(acr1281UC1_OnSendCommand);

                //Get all smart card reader connected to computer
                readerList = acr1281UC1.getReaderList();

                acr1281UC1.readerName = readerList[_deviceAddress];
            }
            catch (PcscException pcscException)
            {
                throw pcscException;
            }
            catch (Exception generalException)
            {
                throw generalException;
            }
        }

        private void Authenticate(byte blockNumber, byte[] key)
        {
            KEYTYPES keyType = KEYTYPES.ACR122_KEYTYPE_A;
            byte keyNumber = 0x00;

            try
            {
                loadKey(key);
                acr1281UC1.authenticate(blockNumber, keyType, keyNumber);
            }
            catch (PcscException pcscException)
            {
                throw pcscException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void loadKey(byte[] key)
        {
            byte keyNumber = 0x00;
            KEY_STRUCTURE keyStructure = KEY_STRUCTURE.VOLATILE;
            try
            {
                acr1281UC1.loadAuthKey(keyStructure, keyNumber, key);
            }
            catch (PcscException pcscException)
            {
                throw pcscException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Helper functions

        byte[] getBytes(string stringBytes, char delimeter)
        {
            int counter = 0;
            byte tmpByte;
            string[] arrayString = stringBytes.Split(delimeter);
            byte[] bytesResult = new byte[arrayString.Length];

            foreach (string str in arrayString)
            {
                if (byte.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out tmpByte))
                {
                    bytesResult[counter] = tmpByte;
                    counter++;
                }
                else
                {
                    return null;
                }
            }

            return bytesResult;
        }

        string byteArrayToString(byte[] b, int startIndx, int len, bool spaceinbetween)
        {
            byte[] newByte;

            if (b.Length < startIndx + len)
                Array.Resize(ref b, startIndx + len);

            newByte = new byte[len];
            Array.Copy(b, startIndx, newByte, 0, len);

            return byteArrayToString(newByte, spaceinbetween);
        }

        string byteArrayToString(byte[] tmpbytes, bool spaceinbetween)
        {
            string tmpStr = string.Empty;

            if (tmpbytes == null)
                return "";

            for (int i = 0; i < tmpbytes.Length; i++)
            {
                tmpStr += string.Format("{0:X2}", tmpbytes[i]);

                if (spaceinbetween)
                    tmpStr += " ";
            }

            return tmpStr;
        }

        #endregion

        void acr1281UC1_OnSendCommand(object sender, TransmitApduEventArg e)
        {

        }

        void acr1281UC1_OnReceivedCommand(object sender, TransmitApduEventArg e)
        {

        }
    }
}
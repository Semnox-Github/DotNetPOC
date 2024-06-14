/********************************************************************************************
 * Project Name - Device
 * Description  - This sample program outlines the steps on how to transact with Mifare cards using ACR Readers
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 1.00         28-Jun-2014     Iqbal Mohammad      Created
 * 1.01         1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.70         30-Jul-2019     Mathew Ninan        Disconnect method added
 * 2.70.2       08-Aug-2019     Deeksha             Modified to add logger methods.
 * 2.80.7       21-Feb-2022     Guru S A            ACR reader performance fix 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    public class ACRReader : MifareDevice
    {
        internal CHIP_TYPE currentChipType = CHIP_TYPE.UNKNOWN;
        internal ACSProvider acsProvider;
        internal MifareClassic mifareClassic;
        internal int _deviceAddress = 0;
        internal string _ModelNumber = "";

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ACRReader()
            :base()
        {
            //log.LogMethodEntry();
            init();
            //log.LogMethodExit();
        }

        public ACRReader(DeviceDefinition deviceDefinition)
            :base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
	        if(string.IsNullOrWhiteSpace(deviceDefinition.OptString) == false)
	        {
		        init(deviceDefinition.OptString);
	        }
	        else
	        {
		        _deviceAddress = deviceDefinition.DeviceAddress;
		        init();
	        }
            log.LogMethodExit();
        }

        public ACRReader(int DeviceAddress)
            :base()
        {
            //log.LogMethodEntry(DeviceAddress);
            _deviceAddress = DeviceAddress;
            init();
            //log.LogMethodExit();
        }

        public ACRReader(byte[] defaultKey)
            : base(new List<byte[]> { defaultKey })
        {
            //log.LogMethodEntry("defaultKey");
            init();
            //log.LogMethodExit();
        }

        public ACRReader(List<byte[]> defaultKeys)
            : base(defaultKeys)
        {
            //log.LogMethodEntry("defaultKeys");
            init();
            //log.LogMethodExit();
        }

        public ACRReader(int DeviceAddress, List<byte[]> defaultKey)
            :base(defaultKey)
        {
            //log.LogMethodEntry(DeviceAddress, "defaultKey");
            _deviceAddress = DeviceAddress;
            init();
            //log.LogMethodExit();
        }

        public ACRReader(string SerialNumber)
            : base()
        {
            //log.LogMethodEntry(SerialNumber);
            init(SerialNumber);
            //log.LogMethodExit();
        }

        public ACRReader(int DeviceAddress, bool readerIsForRechargeOnly)
           : base(readerIsForRechargeOnly)
        {
            _deviceAddress = DeviceAddress;  
            init();
        }
        public ACRReader(string SerialNumber, bool readerIsForRechargeOnly)
           : base(readerIsForRechargeOnly)
        {  
            init(SerialNumber);
        }
        public static List<string> getReaderList()
        {
            //log.LogMethodEntry();
            try
            {
                ACSProvider lclAcsProvider = new ACSProvider();

                List<string> lstReaders = new List<string>(lclAcsProvider.getReaderList());
                lstReaders.RemoveAll(x => x.ToLower().Contains("sam"));
                //log.LogMethodExit(lstReaders);
                return lstReaders;
            }
            catch (PcscException pcscException)
            {
                //log.Error("Error occurred while getting the Reader List", pcscException);
                //log.LogMethodExit(null, "Throwing exception -" + pcscException.Message);
                throw;
            }
            catch (Exception generalException)
            {
                //log.Error("Error occurred while getting the Reader List", generalException);
                //log.LogMethodExit(null, "Throwing exception -" + generalException.Message);
                throw;
            }
        }

        public int DeviceAddress
        {
            get { return _deviceAddress; }
        }

        void init()
        {
            //log.LogMethodEntry();
            initialize();
            setDefaultLEDBuzzerState(); 
            //log.LogMethodExit();
        }

        void init(string SerialNumber)
        {
            //log.LogMethodEntry(SerialNumber);
            initialize(SerialNumber);
            setDefaultLEDBuzzerState(); 
            //log.LogMethodExit();
        }

        public override bool read_data_basic_auth(int blockAddress, int numberOfBlocks, ref byte[] currentKey, ref byte[] paramReceivedData, ref string message)
        {
            lock (LockObject)
            {
                //log.LogMethodEntry(blockAddress, numberOfBlocks, "currentKey", paramReceivedData, message);
                foreach (byte[] key in authKeyArray)
                {
                    if (read_data(blockAddress, numberOfBlocks, key, ref paramReceivedData, ref message))
                    {
                        currentKey = key;
                        //log.LogMethodExit(true);
                        return true;
                    }
                    Thread.Sleep(50);
                }
                //log.LogMethodExit(false);
                return false;
            }
        }

        public override bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey, ref byte[] paramReceivedData, ref string message)
        {
            lock (LockObject)
            {
                //log.LogMethodEntry(blockAddress, numberOfBlocks, "paramAuthKey", paramReceivedData, message);
                try
                {
                    acsProvider.connect();
                    mifareClassic = new MifareClassic(acsProvider.pcscConnection);
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
                    //log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    //log.LogMethodExit(false);
                    message = ex.Message;
                    //log.Error(message);
                    return false;
                }
            }
        }

        public override bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData, ref string message)
        {
            lock (LockObject)
            {
                //log.LogMethodEntry(blockAddress, numberOfBlocks, "authKey", writeData, message);
                byte[] buff = new byte[16];

                try
                {
                    acsProvider.connect();
                    mifareClassic = new MifareClassic(acsProvider.pcscConnection);
                    Authenticate((byte)blockAddress, authKey);

                    int i = 0;
                    while (i < numberOfBlocks)
                    {
                        Array.Copy(writeData, i * 16, buff, 0, 16);
                        mifareClassic.updateBinary((byte)(blockAddress + i), buff, 16);
                        i++;
                    }
                    //log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    //log.Error(message);
                    //log.LogMethodExit(false);
                    return false;
                }
            }
        }

        public override void disconnect()
        {
            //log.LogMethodEntry();
            try
            {
                acsProvider.disconnect();
            }
            catch (Exception ex)
            {
                //log.Error("Error disconnecting", ex);
            }
            //log.LogMethodExit();
        }

        public override string readCardNumber()
        {
            //log.LogMethodEntry();
            lock (LockObject)
            {
                try
                {
                    acsProvider.connect(); 
                    mifareClassic = new MifareClassic(acsProvider.pcscConnection);
                    currentChipType = CHIP_TYPE.UNKNOWN;
                    currentChipType = acsProvider.getChipType(); 
                    if (currentChipType != CHIP_TYPE.MIFARE_1K && 
                        currentChipType != CHIP_TYPE.MIFARE_4K && 
                        currentChipType != CHIP_TYPE.MIFARE_ULTRALIGHT &&
                        currentChipType != CHIP_TYPE.MIFARE_UTLRALIGHT_C)
                    {
                        throw new ApplicationException("Card is not supported.\r\nPlease present Mifare card");
                    }

                    byte[] card = acsProvider.getCardSerialNumber();
                    string card_num = "";
                    if (card == null)
                    { 
                        return card_num;
                    }
                    foreach (byte values in card)
                        card_num += String.Format("{0:X2}", values);

                    if (card_num.Equals("00000000"))
                        card_num = ""; 
                    return card_num;
                }
                catch(Exception ex)
                {
                    currentChipType = CHIP_TYPE.UNKNOWN; 
                    return "";
                }
            }
        }

        public override bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
            lock (LockObject)
            { 
                try
                {
                    byte[] dataBuffer = new byte[0];
                    if (!read_data(blockAddress, 1, currentAuthKey, ref dataBuffer, ref message))
                    { 
                        return false;
                    }

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
        }

        protected virtual void setDefaultLEDBuzzerState()
        { 
        }

        protected void setDefaultLEDBuzzerState(params byte[] stateBytes)
        {
            //log.LogMethodEntry(stateBytes);
            ACSProvider lclACS = new ACSProvider();
            try
            {
                lclACS.readerName = acsProvider.readerName;
                lclACS.connectDirect();

                Apdu apduC = new Apdu();
                apduC.lengthExpected = 1;
                apduC.data = stateBytes;

                uint operationControlCode = (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4;
                lclACS.sendCardControl(ref apduC, operationControlCode);
            }
            catch(Exception ex)
            {
                //log.Error("Error occurred while setting the Default LEDBuzzerState", ex);
            }
            finally
            {
                try
                {
                    lclACS.disconnect();
                }
                catch(Exception ex)
                {
                    //log.Error("Error disconnecting", ex);
                }
            }
            //log.LogMethodExit();
        }

        List<string> filterByModel(string[] readerList)
        {
            //log.LogMethodEntry(readerList);
            List<string> lstReaders = new List<string>(readerList);
            lstReaders.RemoveAll(x => x.ToLower().Contains("sam"));
            string[] modelKeyWords = _ModelNumber.Split('|');
            foreach (string keyWord in modelKeyWords)
                lstReaders.RemoveAll(x => x.ToLower().Contains(keyWord.ToLower()) == false);
            //log.LogMethodExit(lstReaders);
            return lstReaders;
        }

        internal virtual void initialize(string SerialNumber)
        {
            //log.LogMethodEntry(SerialNumber);
            try
            {
                string[] readerList;

                acsProvider = new ACSProvider();

                //Register to event OnReceivedCommand
                acsProvider.OnReceivedCommand += new TransmitApduDelegate(acsProvider_OnReceivedCommand);

                //Register to event OnSendCommand
                acsProvider.OnSendCommand += new TransmitApduDelegate(acsProvider_OnSendCommand);

                //Get all smart card reader connected to computer
                readerList = acsProvider.getReaderList();

                List<string> lstReaders = filterByModel(readerList);

                foreach (string reader in lstReaders)
                {
                    acsProvider.readerName = reader;
                    if (getSerialNumber().ToLower() == SerialNumber.ToLower())
                    {
                        //log.LogMethodExit();
                        return;
                    }
                }

                throw new PcscException("Reader with serial number " + SerialNumber + " not found");
               
            }
            catch (PcscException pcscException)
            {
                //log.Error("Error while executing initialize() ", pcscException);
                //log.LogMethodExit(null, "Throwing exception -" + pcscException.Message);
                throw ;
            }
            catch (Exception generalException)
            {
                //log.Error("Error while executing initialize()", generalException);
                //log.LogMethodExit(null, "Throwing exception -" + generalException.Message);
                throw ;
            }
           
        }

        internal virtual void initialize()
        {
            //log.LogMethodEntry();
            try
            {
                string[] readerList;

                acsProvider = new ACSProvider();

                //Register to event OnReceivedCommand
                acsProvider.OnReceivedCommand += new TransmitApduDelegate(acsProvider_OnReceivedCommand);

                //Register to event OnSendCommand
                acsProvider.OnSendCommand += new TransmitApduDelegate(acsProvider_OnSendCommand);

                //Get all smart card reader connected to computer
                readerList = acsProvider.getReaderList();
                List<string> lstReaders = filterByModel(readerList);

                if (lstReaders.Count == 0)
                    throw new ApplicationException("Unable to find reader: " + _ModelNumber);

                acsProvider.readerName = lstReaders[_deviceAddress];
                //log.LogMethodExit();
            }
            catch (PcscException pcscException)
            {
                //log.Error("Error while executing initialize()", pcscException);
                //log.LogMethodExit(null, "Throwing exception -" + pcscException.Message);
                throw ;
            }
            catch (Exception generalException)
            {
                //log.Error("Error while executing initialize()", generalException);
                //log.LogMethodExit(null, "Throwing exception -" + generalException.Message);
                throw ;
            }
        }

        protected void beep(params byte[] beepData)
        {
            lock (LockObject)
            {
                //log.LogMethodEntry(beepData);
                Apdu apduCommand = new Apdu();
                apduCommand.lengthExpected = 1;
                apduCommand.data = beepData;

                uint operationControlCode = (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4;
                try
                {
                    acsProvider.sendCardControl(ref apduCommand, operationControlCode);
                }
                catch(Exception ex)
                {
                    ACSProvider lclACS = new ACSProvider();
                    try
                    {
                        lclACS.readerName = acsProvider.readerName;
                        lclACS.connectDirect();
                        lclACS.sendCardControl(ref apduCommand, operationControlCode);
                    }
                    catch(Exception exp)
                    {
                        //log.Error("Error occurred while connecting", exp);
                    }
                    finally
                    {
                        try
                        {
                            lclACS.disconnect();
                        }
                        catch(Exception expn)
                        {
                            //log.Error("Error disconnecting", expn);
                        }
                    }
                    //log.Error("Error Occurred while sending Card Control", ex);
                }
                //log.LogMethodExit();
            }
        }

        public override void beep(int repeat = 1)
        {
            //log.LogMethodEntry(repeat);
            beep(repeat, true);
            //log.LogMethodExit();
        }

        public override void Authenticate(byte blockNumber, byte[] key)
        {
            lock (LockObject)
            {
                //log.LogMethodEntry(blockNumber, "key");
                KEYTYPES keyType = KEYTYPES.ACR122_KEYTYPE_A;
                byte keyNumber = 0x00;

                try
                {
                    loadKey(key);
                    acsProvider.authenticate(blockNumber, keyType, keyNumber);
                    //log.LogMethodExit();
                }
                catch (PcscException pcscException)
                {
                    //log.Error("Error occurred while authentication", pcscException);
                    //log.LogMethodExit(null, "Throwing exception -" + pcscException.Message);
                    throw ;
                }
                catch (Exception ex)
                {
                    //log.Error("Error occurred while authentication", ex);
                    //log.LogMethodExit(null, "Throwing exception -" + ex.Message);
                    throw ;
                }
            }
        }

        void loadKey(byte[] key)
        {
            //log.LogMethodEntry("key");
            byte keyNumber = 0x00;
            KEY_STRUCTURE keyStructure = KEY_STRUCTURE.VOLATILE;
            try
            {
                acsProvider.loadAuthKey(keyStructure, keyNumber, key);
                //log.LogMethodExit();
            }
            catch (PcscException pcscException)
            {
                //log.Error("Error occurred while Loading Key", pcscException);
                //log.LogMethodExit(null, "Throwing exception -" + pcscException.Message);
                throw ;
            }
            catch (Exception ex)
            {
                //log.Error("Error occurred while  Loading Key", ex);
                //log.LogMethodExit(null, "Throwing exception -" + ex.Message);
                throw ;
            }
            
        }

        #region Helper functions

        byte[] getBytes(string stringBytes, char delimeter)
        {
            //log.LogMethodEntry(stringBytes, delimeter);
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
                    //log.LogMethodExit();
                    return null;
                }
            }
            //log.LogMethodExit(bytesResult);
            return bytesResult;
        }

        string byteArrayToString(byte[] b, int startIndx, int len, bool spaceinbetween)
        {
            //log.LogMethodEntry(b, startIndx, len, spaceinbetween);
            byte[] newByte;

            if (b.Length < startIndx + len)
                Array.Resize(ref b, startIndx + len);

            newByte = new byte[len];
            Array.Copy(b, startIndx, newByte, 0, len);
            string returnValue = byteArrayToString(newByte, spaceinbetween);
            //log.LogMethodExit(returnValue);
            return returnValue;
        }

        string byteArrayToString(byte[] tmpbytes, bool spaceinbetween)
        {
            //log.LogMethodEntry(tmpbytes, spaceinbetween);
            string tmpStr = string.Empty;

            if (tmpbytes == null)
            {
                //log.LogMethodExit();
                return "";
            }

            for (int i = 0; i < tmpbytes.Length; i++)
            {
                tmpStr += string.Format("{0:X2}", tmpbytes[i]);

                if (spaceinbetween)
                    tmpStr += " ";
            }
            //log.LogMethodExit(tmpStr);
            return tmpStr;
        }

        #endregion

        internal void acsProvider_OnSendCommand(object sender, TransmitApduEventArg e)
        {
            //log.LogMethodEntry();
            //log.LogMethodExit();

        }

        internal void acsProvider_OnReceivedCommand(object sender, TransmitApduEventArg e)
        {
            //log.LogMethodEntry();
            //log.LogMethodExit();
        }
    }
}
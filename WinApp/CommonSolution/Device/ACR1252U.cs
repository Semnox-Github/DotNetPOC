/********************************************************************************************
 * Project Name - Device
 * Description  - This sample program outlines the steps on how to transact with Mifare cards using ACR1252U
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 1.00         28-Jun-2014     Iqbal Mohammad      Created
 * 2.70         1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.70.2       08-Aug-2019     Deeksha             Modified to add logger methods.
 * 2.80.7       21-Feb-2022     Guru S A            ACR reader performance fix 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    public class ACR1252U : ACRReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private UlcKeyStore ulcKeyStore;
        public ACR1252U()
            : base()
        {
            log.LogMethodEntry();
            LoadUltralightCKeys();
            log.LogMethodExit();
        }

        public ACR1252U(DeviceDefinition deviceDefinition)
        :base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            ulcKeyStore = new UlcKeyStore(deviceDefinition.MifareKeyContainerDTOList);
            log.LogMethodExit();
        }

        private void LoadUltralightCKeys()
        {
            log.LogMethodEntry();
            ulcKeyStore = new UlcKeyStore();
            log.LogMethodExit();
        }

        public ACR1252U(int deviceAddress)
            : base(deviceAddress)
        {
            log.LogMethodEntry(deviceAddress);
            LoadUltralightCKeys();
            log.LogMethodExit();
        }

        public ACR1252U(byte[] defaultKey)
            : base(GetMifareKeys(defaultKey))
        {
            log.LogMethodEntry("defaultKey");
            ulcKeyStore = new UlcKeyStore(defaultKey);
            log.LogMethodExit();
        }

        public ACR1252U(int deviceAddress, List<byte[]> defaultKeys)
            : base(deviceAddress, GetMifareKeys(defaultKeys))
        {
            log.LogMethodEntry(deviceAddress, "defaultKeys");
            ulcKeyStore = new UlcKeyStore(defaultKeys);
            log.LogMethodExit();
        }

        public ACR1252U(string serialNumber)
            : base(serialNumber)
        {
            log.LogMethodEntry(serialNumber);
            LoadUltralightCKeys();
            log.LogMethodExit();
        }

        public ACR1252U(int deviceAddress, bool readerForRechargeOnly)
           : base(deviceAddress, readerForRechargeOnly)
        {
            log.LogMethodEntry(deviceAddress, readerForRechargeOnly);
            LoadUltralightCKeys();
            log.LogMethodExit();
        }
        public ACR1252U(string serialNumber, bool readerForRechargeOnly)
           : base(serialNumber, readerForRechargeOnly)
        {
            log.LogMethodEntry(serialNumber, readerForRechargeOnly);
            LoadUltralightCKeys();
            log.LogMethodExit();
        }
        private static List<byte[]> GetMifareKeys(byte[] defaultKey)
        {
            log.LogMethodEntry("defaultKey");
            List<byte[]> mifareKeys = new List<byte[]>();
            if (defaultKey == null)
            {
                log.LogMethodExit("mifareKeys", "Empty defaultKey");
                return mifareKeys;
            }

            if (UlcKey.IsValidKeyBytes(defaultKey) == false)
            {
                mifareKeys.Add(defaultKey);
            }

            log.LogMethodExit("mifareKeys");
            return mifareKeys;
        }

        private static List<byte[]> GetMifareKeys(IEnumerable<byte[]> defaultKeys)
        {
            log.LogMethodEntry("defaultKeys");
            List<byte[]> mifareKeys;
            if (defaultKeys == null)
            {
                mifareKeys = new List<byte[]>();
                log.LogMethodExit(mifareKeys, "Empty defaultKeys");
                return mifareKeys;
            }
            mifareKeys = defaultKeys.Where(defaultKey => UlcKey.IsValidKeyBytes(defaultKey) == false).ToList();
            log.LogMethodExit("mifareKeys");
            return mifareKeys;
        }

        

        public override void beep(int repeat, bool asynchronous)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(repeat, asynchronous);
                if (asynchronous)
                {
                    Thread thr = new Thread(() => { base.beep(0xE0, 0x00, 0x00, 0x28, 0x01, (byte) (repeat * 10)); });

                    thr.Start();
                }
                else
                    base.beep(0xE0, 0x00, 0x00, 0x28, 0x01, (byte) (repeat * 10));

                log.LogMethodExit();
            }
        }

        protected override void setDefaultLEDBuzzerState()
        {
            log.LogMethodEntry();
            base.setDefaultLEDBuzzerState(0xE0, 0x00, 0x00, 0x21, 0x01, 0x77);
            log.LogMethodExit();
        }

        internal override void initialize()
        {
            log.LogMethodEntry();
            _ModelNumber = "1252";
            base.initialize();
            log.LogMethodExit();
        }

        internal override void initialize(string serialNumber)
        {
            log.LogMethodEntry(serialNumber);
            _ModelNumber = "1252";
            base.initialize(serialNumber);
            log.LogMethodExit();
        }

        public override string getSerialNumber()
        {
            log.LogMethodEntry();
            ACSProvider lclACS = new ACSProvider();
            try
            {
                lclACS.readerName = acsProvider.readerName;
                lclACS.connectDirect();

                Apdu apduC = new Apdu();
                apduC.lengthExpected = 21;
                apduC.data = new byte[] { 0xE0, 0x00, 0x00, 0x33, 0x00 };

                uint operationControlCode = (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4;
                lclACS.sendCardControl(ref apduC, operationControlCode);

                int length = apduC.response[4];

                byte[] serialNumber = new byte[length];
                Array.Copy(apduC.response, 5, serialNumber, 0, length);

                string result = ASCIIEncoding.ASCII.GetString(serialNumber);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while getting the serial number", ex);
                log.LogMethodExit(string.Empty);
                return string.Empty;
            }
            finally
            {
                try
                {
                    lclACS.disconnect();
                }
                catch (Exception ex)
                {
                    log.Error("Error disconnecting", ex);
                }
            }
        }

        public override string setSerialNumber(string serialNumber)
        {
            log.LogMethodEntry(serialNumber);
            ACSProvider lclACS = new ACSProvider();
            try
            {
                lclACS.readerName = acsProvider.readerName;
                lclACS.connectDirect();

                Apdu apduC = new Apdu();
                apduC.lengthExpected = 21;
                apduC.data = new byte[serialNumber.Length + 5];

                byte[] cmd = new byte[] { 0xE0, 0x00, 0x00, 0xDA, (byte)serialNumber.Length };
                Array.Copy(cmd, apduC.data, cmd.Length);
                byte[] data = ASCIIEncoding.ASCII.GetBytes(serialNumber);
                Array.Copy(data, 0, apduC.data, cmd.Length, data.Length);

                uint operationControlCode = (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4;
                lclACS.sendCardControl(ref apduC, operationControlCode);

                if (apduC.response[5] == 0x90 && apduC.response[6] == 0x00)
                {
                    string errorMessage = "Serial number is locked";
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ApplicationException(errorMessage);
                }
                else if (apduC.response[5] == 0x90 && apduC.response[6] == 0x00)
                {
                    string errorMessage = "Serial number is too long";
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ApplicationException(errorMessage);
                }

                int length = apduC.response[4];

                byte[] retSerial = new byte[length];
                Array.Copy(apduC.response, 5, retSerial, 0, length);

                string result = ASCIIEncoding.ASCII.GetString(retSerial);
                log.LogMethodExit(result);
                return result;
            }
            finally
            {
                try
                {
                    lclACS.disconnect();
                }
                catch (Exception ex)
                {
                    log.Error("Error disconnecting", ex);
                }
            }
        }

        public override bool Validate()
        {
            log.LogMethodEntry();
            bool result;
            try
            {
                //log.Fatal("Start Validate");
                //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                //currentChipType = acsProvider.getChipType();
                if (currentChipType == CHIP_TYPE.MIFARE_UTLRALIGHT_C)
                {
                    acsProvider.connect();
                    log.Debug("connect");
                    result = ValidateUltraLightC();
                    log.Debug("disconnect");
                    acsProvider.disconnect();
                }
                else if(currentChipType == CHIP_TYPE.MIFARE_ULTRALIGHT)
                {
                    isRoamingCard = false;
                    TagSiteId = SiteId;
                    result = true;
                }
                else
                {
                    result = base.Validate();
                }
            }
            catch (Exception ex)
            {
                //log.Fatal("Error in Validate: " + ex.Message); 
                log.Error("Error occurred while validating the card.", ex);
                result = false;
            }

            //log.Fatal("End Validate");
            //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
            log.LogMethodExit(result);
            return result;
        }

        private bool ValidateUltraLightC()
        {
            log.LogMethodEntry();
            if (ulcKeyStore == null || ulcKeyStore.LatestCustomerUlcKey == null)
            {
                log.LogMethodExit(false, "No valid customer ultralight key is configured");
                return false;
            }

            foreach (UlcKey key in ulcKeyStore.ValidUltralightCKeys)
            {
                bool authenticationSuccessful = Authenticate(key);
                if (authenticationSuccessful == false)
                {
                    acsProvider.disconnect();
                    Thread.Sleep(200);
                    acsProvider.connect();
                    continue;
                }

                if (key != ulcKeyStore.LatestCustomerUlcKey)
                {
                    log.Debug("Changing Authentication key");
                    defaultKeyChanged = true;
                    isRoamingCard = false;
                    TagSiteId = SiteId;
                    string message = "";

                    Thread.Sleep(50);
                    acsProvider.connect();
                    bool authenticationKeyChanged = ChangeAuthenticationKey(ulcKeyStore.LatestCustomerUlcKey);
                    if (authenticationKeyChanged == false)
                    {
                        log.LogMethodExit(false, "Unable to change the authentication key.");
                        return false;
                    }
                    
                    beep();

                    bool siteIdWrittenToTheCard = WriteSiteIdToTheCard(ref message);
                    if (siteIdWrittenToTheCard == false)
                    {
                        log.LogMethodExit(false, "Unable to write the site id to the card");
                        return false;
                    }
                }
                else
                {
                    log.Debug("Not Changing Authentication key");
                    defaultKeyChanged = false;
                    string message = "";
                    int cardSiteId = GetCardSiteId(ref message);
                    isRoamingCard = cardSiteId != SiteId;
                    TagSiteId = cardSiteId;
                }
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false, "Unable to authenticate card with any key.");
            return false;
        }

        private int GetCardSiteId(ref string message)
        {
            log.LogMethodEntry(message);
            Core.GenericUtilities.ByteArray siteIdByteArray = acsProvider.ReadUlc(6);
            if (siteIdByteArray == null)
            {
                log.LogMethodExit(-1, "Unable to read site id from the card");
                return -1;
            }

            int result = BitConverter.ToInt32(siteIdByteArray.Value, 0);
            log.LogMethodExit(result);
            return result;
        }

        private bool WriteSiteIdToTheCard(ref string message)
        {
            log.LogMethodEntry(message);
            Core.GenericUtilities.ByteArray siteIdBuffer = GetSiteIdBuffer();
            bool writeSuccessful =
                WriteUlc(6, 1, ulcKeyStore.LatestCustomerUlcKey, siteIdBuffer.Value, ref message);
            log.LogMethodExit(writeSuccessful);
            return writeSuccessful;
        }

        private Core.GenericUtilities.ByteArray GetSiteIdBuffer()
        {
            log.LogMethodEntry();
            byte[] siteIdBuffer = new byte[16];
            siteIdBuffer[0] = (byte)(SiteId >> 0);
            siteIdBuffer[1] = (byte)(SiteId >> 8);
            siteIdBuffer[2] = (byte)(SiteId >> 16);
            siteIdBuffer[3] = (byte)(SiteId >> 24);
            Core.GenericUtilities.ByteArray result = new Core.GenericUtilities.ByteArray(siteIdBuffer);
            log.LogMethodExit(result);
            return result;
        }

        private bool ChangeAuthenticationKey(UlcKey newKey)
        {
            log.LogMethodEntry("newKey");
            bool authenticationKeyChanged;
            try
            {
                authenticationKeyChanged = WriteUltralightAuthenticationKey(newKey);
                if (authenticationKeyChanged)
                {
                    log.Debug("disconnect");
                    acsProvider.disconnect();
                    log.Debug("connect");
                    acsProvider.connect();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while changing the authentication key", ex);
                authenticationKeyChanged = false;
            }

            log.LogMethodExit(authenticationKeyChanged);
            return authenticationKeyChanged;
        }

        private bool WriteUltralightAuthenticationKey(UlcKey ulcKey)
        {
            log.LogMethodEntry("ulcKey");
            bool result = true;
            result &= acsProvider.WriteUlcMemoryPage(0x2C, ulcKey.SubArray(0, 4));
            result &= acsProvider.WriteUlcMemoryPage(0x2D, ulcKey.SubArray(4, 4));
            result &= acsProvider.WriteUlcMemoryPage(0x2E, ulcKey.SubArray(8, 4));
            result &= acsProvider.WriteUlcMemoryPage(0x2F, ulcKey.SubArray(12, 4));
            log.LogMethodExit(result);
            return result;
        }

        public override void Authenticate(byte blockNumber, byte[] key)
        {
            lock (LockObject)
            {
                //log.Fatal("Start Authenticate");
                //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                log.LogMethodEntry(blockNumber, "key");
                //currentChipType = acsProvider.getChipType();
                if (currentChipType != CHIP_TYPE.MIFARE_UTLRALIGHT_C)
                {
                    base.Authenticate(blockNumber, key);
                    //log.Fatal("End Authenticate");
                    //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                    log.LogMethodExit(null, "Normal mifare card");
                    return;
                }
                Thread.Sleep(200);
                acsProvider.connect();
                log.Debug("connect");

                if (UlcKey.IsValidKeyBytes(key) == false)
                {
                    string errorMessage = "key is not a valid ultralight c key.";
                    log.LogMethodExit(null, "Throwing Exception -" + errorMessage);
                    throw new Exception(errorMessage);
                }
                
                UlcKey ulcKey = new UlcKey(key);
                if (Authenticate(ulcKey) == false)
                {
                    string errorMessage = "Unable to authenticate with the card.";
                    log.LogMethodExit(null, "Throwing Exception -" + errorMessage);
                    throw new Exception(errorMessage);
                }
                log.Debug("disconnect");
                acsProvider.disconnect();
                log.LogMethodExit();
            }
        }

        private bool Authenticate(UlcKey key)
        {
            log.LogMethodEntry("key");
            bool result;
            try
            {
                Core.GenericUtilities.ByteArray encRandB = acsProvider.GetEncryptedRandB();
                log.LogVariableState("encRandB", encRandB);

                Core.GenericUtilities.ByteArray initialVector = new Core.GenericUtilities.ByteArray("0000000000000000");
                TripleDesEncryption tripleDesEncryption = new TripleDesEncryption(key.TranslatedKey);
                Core.GenericUtilities.ByteArray randB = tripleDesEncryption.Decrypt(encRandB, initialVector);

                log.LogVariableState("randB", randB);

                Core.GenericUtilities.ByteArray randA = new RandomByteArray(8);
                log.LogVariableState("randA", randA);

                Core.GenericUtilities.ByteArray randBn = randB.RotateLeftBy(1);
                log.LogVariableState("randBn", randBn);

                Core.GenericUtilities.ByteArray randARandBn = randA.Append(randBn);
                log.LogVariableState("randARandBn", randARandBn);

                Core.GenericUtilities.ByteArray encRandARandBn = tripleDesEncryption.Encrypt(randARandBn, encRandB);
                log.LogVariableState("encRandARandBn", encRandARandBn);

                Core.GenericUtilities.ByteArray encRandAn = acsProvider.GetEncryptedRandAn(encRandARandBn);
                log.LogVariableState("encRandAn", encRandAn);

                initialVector = encRandARandBn.SubArray(8, 8);
                log.LogVariableState("initialVector", initialVector);

                Core.GenericUtilities.ByteArray decRandAn = tripleDesEncryption.Decrypt(encRandAn, initialVector);
                log.LogVariableState("decRandAn", decRandAn);

                Core.GenericUtilities.ByteArray randAn = randA.RotateLeftBy(1);
                log.LogVariableState("randAn", randAn);

                result = decRandAn == randAn;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while authenticating ULC card", ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        public override CardType CardType
        {
            get
            {
                lock (LockObject)
                {
                    CardType result;
                    try
                    {
                        acsProvider.connect();
                        log.Debug("connect");
                        //currentChipType = acsProvider.getChipType();
                        result = currentChipType == CHIP_TYPE.MIFARE_UTLRALIGHT_C
                            ? CardType.MIFARE_ULTRA_LIGHT_C
                            : CardType.MIFARE;
                    }
                    catch (Exception ex)
                    {
                        result = CardType.MIFARE;
                        log.Error("Error occurred while getting chip type.", ex);
                    }

                    return result;
                }
            }
        }

        public override bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, "currentAuthKey", "newAuthKey", message);
                bool result;
                try
                {
                    acsProvider.connect();
                    log.Debug("connect");
                    //currentChipType = acsProvider.getChipType();
                    if (currentChipType != CHIP_TYPE.MIFARE_UTLRALIGHT_C)
                    {
                        result = base.change_authentication_key(blockAddress, currentAuthKey, newAuthKey, ref message);
                        log.LogMethodExit(result, "Normal mifare card");
                        return result;
                    }
                    
                    if (UlcKey.IsValidKeyBytes(currentAuthKey) == false)
                    {
                        message = "currentAuthKey is not a valid ultralight c key.";
                        log.LogMethodExit(false, message);
                        return false;
                    }

                    if (UlcKey.IsValidKeyBytes(newAuthKey) == false)
                    {
                        message = "newAuthKey is not a valid ultralight c key.";
                        log.LogMethodExit(false, message);
                        return false;
                    }

                    UlcKey currentKey = new UlcKey(currentAuthKey);
                    bool canAuthenticateWithCurrentKey = Authenticate(currentKey);
                    if (canAuthenticateWithCurrentKey == false)
                    {
                        log.LogMethodExit(false, "Unable to authenticate with current key.");
                        return false;
                    }

                    UlcKey newKey = new UlcKey(newAuthKey);
                    result = ChangeAuthenticationKey(newKey);
                    log.Debug("disconnect");
                    acsProvider.disconnect();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    log.Error("Error occurred while changing the authentication key]", ex);
                    result = false;
                }
                log.LogMethodExit(result);
                return result;
            }
        }

        public override bool read_data_basic_auth(int blockAddress, int numberOfBlocks, ref byte[] currentKey, ref byte[] paramReceivedData, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, numberOfBlocks, "currentKey", paramReceivedData, message);
                acsProvider.connect();
                log.Debug("connect");
                //currentChipType = acsProvider.getChipType();
                if (currentChipType != CHIP_TYPE.MIFARE_UTLRALIGHT_C)
                {
                    bool result = base.read_data_basic_auth(blockAddress, numberOfBlocks, ref currentKey, ref paramReceivedData, ref message);
                    log.LogMethodExit(result, "Normal mifare card");
                    return result;
                }

                foreach (UlcKey key in ulcKeyStore.ValidUltralightCKeys)
                {
                    Thread.Sleep(200);
                    acsProvider.connect();
                    if (ReadUlc(blockAddress, numberOfBlocks, key, ref paramReceivedData, ref message))
                    {
                        currentKey = key.Value;
                        log.LogMethodExit(true, "Found the key");
                        return true;
                    }
                    acsProvider.disconnect();
                }

                log.LogMethodExit(false);
                return false;
            }
        }

        public override bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey,
            ref byte[] paramReceivedData, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, numberOfBlocks, "paramAuthKey", paramReceivedData, message);
                bool result;
                try
                {
                    Thread.Sleep(200);
                    acsProvider.connect();
                    log.Debug("connect");
                    //currentChipType = acsProvider.getChipType();
                    if (currentChipType != CHIP_TYPE.MIFARE_UTLRALIGHT_C)
                    {
                        result = base.read_data(blockAddress, numberOfBlocks, paramAuthKey, ref paramReceivedData,
                            ref message);
                        log.LogMethodExit(result, "Normal mifare card");
                        return result;
                    }
                    
                    if (UlcKey.IsValidKeyBytes(paramAuthKey) == false)
                    {
                        message = "paramAuthKey is not a valid ultralight c key.";
                        log.LogMethodExit(false, message);
                        return false;
                    }

                    
                    UlcKey key = new UlcKey(paramAuthKey);
                    result = ReadUlc(blockAddress, numberOfBlocks, key, ref paramReceivedData, ref message);
                    log.Debug("disconnect");
                    acsProvider.disconnect();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    log.Error("Error occurred while reading the card", ex);
                    result = false;
                }
                log.LogMethodExit(result);
                return result;
            }
        }

        public override bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData,
            ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, numberOfBlocks, "authKey", writeData, message);
                bool result;
                try
                {
                    Thread.Sleep(200);
                    acsProvider.connect();
                    log.Debug("connect");
                    //currentChipType = acsProvider.getChipType();
                    if (currentChipType != CHIP_TYPE.MIFARE_UTLRALIGHT_C)
                    {
                        result = base.write_data(blockAddress, numberOfBlocks, authKey, writeData,
                            ref message);
                        log.LogMethodExit(result, "Normal mifare card");
                        return result;
                    }
                    if (UlcKey.IsValidKeyBytes(authKey) == false)
                    {
                        message = "authKey is not a valid ultralight c key.";
                        log.LogMethodExit(false, message);
                        return false;
                    }

                    UlcKey key = new UlcKey(authKey);
                    result = WriteUlc(blockAddress, numberOfBlocks, key, writeData, ref message);
                    log.Debug("disconnect");
                    acsProvider.disconnect();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    log.Error("Error occurred while writing the data to the card", ex);
                    result = false;
                }
                log.LogMethodExit(result);
                return result;
            }
        }

        private bool WriteUlc(int blockAddress, int numberOfBlocks, UlcKey key, byte[] writeData,
            ref string message)
        {
            log.LogMethodEntry(blockAddress, numberOfBlocks, "key", writeData, message);

            if (writeData == null)
            {
                message = "writeData is empty";
                log.LogMethodExit(false, message);
                return false;
            }

            if (writeData.Length != 16 * numberOfBlocks)
            {
                message = "writeData length is " + writeData.Length + " which is not equal to the expected length " +
                          16 * numberOfBlocks;
                log.LogMethodExit(false, message);
                return false;
            }

            bool canAuthenticate = Authenticate(key);
            if (canAuthenticate == false)
            {
                log.LogMethodExit(false, "Unable to authenticate the card");
                return false;
            }
            Core.GenericUtilities.ByteArray dataToBeWritten = new Core.GenericUtilities.ByteArray(writeData);
            bool result = true;
            for (int i = 0; i < numberOfBlocks; i++)
            {
                result &= acsProvider.WriteUlc(blockAddress + i, dataToBeWritten.SubArray(i * 16, 16));
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool ReadUlc(int blockAddress, int numberOfBlocks, UlcKey key,
            ref byte[] paramReceivedData, ref string message)
        {
            log.LogMethodEntry(blockAddress, numberOfBlocks, "key", paramReceivedData, message);
            try
            {
                bool canAuthenticate = Authenticate(key);
                if (canAuthenticate == false)
                {
                    log.LogMethodExit(false, "Unable to authenticate the card");
                    return false;
                }
                Core.GenericUtilities.ByteArray data = null;
                for (int i = 0; i < numberOfBlocks; i++)
                {
                    Core.GenericUtilities.ByteArray byteArray = acsProvider.ReadUlc((blockAddress + i));
                    if (byteArray == null)
                    {
                        log.LogMethodExit(false, "Unable to read data from the card");
                        return false;
                    }
                    if (data == null)
                    {
                        data = byteArray;
                    }
                    else
                    {
                        data = data.Append(byteArray);
                    }
                }

                if (data != null)
                    paramReceivedData = data.Value;
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while reading the data from the card", ex);
                message = ex.Message;
            }

            log.LogMethodExit(false);
            return false;
        }

        public bool LockMemory()
        {
            log.LogMethodEntry();
            bool result = true;
            if (ulcKeyStore.LatestCustomerUlcKey == null)
            {
                log.LogMethodExit(false, "Latest customer ultralight c key is empty.");
                return false;
            }
            acsProvider.connect();
            result &= Authenticate(ulcKeyStore.LatestCustomerUlcKey);
            if (result)
            {
                acsProvider.WriteUlcMemoryPage(0x2a, new Core.GenericUtilities.ByteArray("03-00-00-00"));
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
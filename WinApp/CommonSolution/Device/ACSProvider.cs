/********************************************************************************************
 * Project Name - Device
 * Description  - Contains Methods and Properties related to ACR device's functionality
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 1.00        28-Jun-2014     Arturo Salvamante      Created
 * 1.01        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.70.2        08-Aug-2019     Deeksha             Modified to add logger methods.
 ********************************************************************************************/

using System;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Device
{
    /*internal enum CHIP_TYPE
    {
        UNKNOWN = 0,
        MIFARE_1K = 1,
        MIFARE_4K = 2,
        MIFARE_ULTRALIGHT = 3,
    }

    internal enum KEY_STRUCTURE
    {
        VOLATILE = 0x00,
        NON_VOLATILE = 0x20
    }

    internal enum KEYTYPES
    {
        ACR122_KEYTYPE_A = 96,
        ACR122_KEYTYPE_B = 97,
    }*/

    internal class ACSProvider : PcscReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public byte[] getCardSerialNumber()
        {
            log.LogMethodEntry();
            byte[] cardSerial;

            apduCommand = new Apdu();
            apduCommand.setCommand(new byte[] {  0xFF,      //Instruction Class
                                                 0xCA,      //Instruction Code
                                                 0x00,      //Parameter 1
                                                 0x00,      //Parameter 2
                                                 0x00 });   //Parameter 3
            apduCommand.lengthExpected = 20;
            sendCommand();

            if (apduCommand.statusWord[0] != 0x90)
                return null;

            cardSerial = new byte[apduCommand.response.Length];
            Array.Copy(apduCommand.response, cardSerial, cardSerial.Length);
            log.LogMethodExit(cardSerial);
            return cardSerial;

        }

        public byte[] getAnswerToSelect()
        {
            log.LogMethodEntry();
            apduCommand = new Apdu();
            apduCommand.setCommand(new byte[] {  0xFF, 
                                                 0xCA, 
                                                 0x01, 
                                                 0x00, 
                                                 0x00 });

            apduCommand.lengthExpected = 50;

            sendCommand();

            if (!apduCommand.statusWordEqualTo(new byte[] { 0x90, 0x00 }))
                throw new CardException("Unable to get Answer to Select (ATS)", apduCommand.statusWord);
            byte[] returnValue = apduCommand.response;
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public void loadAuthKey(KEY_STRUCTURE keyStructure, byte keyNumber, byte[] key)
        {
            log.LogMethodEntry("keyStructure", "keyNumber", "key");
            if (key.Length != 6)
                throw new Exception("Invalid key length");


            apduCommand = new Apdu();
            apduCommand.setCommand(new byte[] {  0xFF,                  //Instruction Class
                                                 0x82,                  //Instruction code
                                                 (byte)keyStructure,    //Key Structure
                                                 keyNumber,             //Key Number
                                                 0x06 });               //Length of key

            //Set key to load
            apduCommand.data = key;

            sendCommand();

            if (!apduCommand.statusWordEqualTo(new byte[] { 0x90, 0x00 }))
                throw new CardException("Load key failed", apduCommand.statusWord);
            log.LogMethodExit();
                
        }

        public void authenticate(byte blockNumber, KEYTYPES keyType, byte KeyNumber)
        {
            log.LogMethodEntry(blockNumber, "keyType", "KeyNumber");
            if (KeyNumber < 0x00 && KeyNumber > 0x20)
                throw new Exception("Key number is invalid");

            apduCommand = new Apdu();
            apduCommand.setCommand(new byte[]{ 0xFF,            //Instruction Class
                                               0x86,            //Instruction Code
                                               0x00,            //RFU
                                               0x00,            //RFU
                                               0x05});          //Length of authentication data bytes

            //Authentication Data Bytes
            apduCommand.data = new byte[] {  0x01,              //Version
                                             0x00,              //RFU
                                             (byte)blockNumber, //Block Number
                                             (byte)keyType,     //Key Type
                                             KeyNumber};        //Key Number

            sendCommand();

            if (!apduCommand.statusWordEqualTo(new byte[] { 0x90, 0x00 }))
                throw new CardException("Authenticate failed", apduCommand.statusWord);
            log.LogMethodExit();
        }

        public CHIP_TYPE getChipType()
        {
            log.LogMethodEntry();
            int rdrLen = 0, retCode, protocol = pcscConnection.activeProtocol;
            int pdwSate = 0, atrLen = 33;
            byte[] atr = new byte[100];
            CHIP_TYPE cardType = CHIP_TYPE.UNKNOWN;


            retCode = PcscProvider.SCardStatus(pcscConnection.cardHandle, pcscConnection.readerName, ref rdrLen, ref pdwSate,
                                                ref protocol, atr, ref atrLen);

            if (retCode != PcscProvider.SCARD_S_SUCCESS)
                throw new PcscException(retCode);

            pcscConnection.activeProtocol = protocol;

            if (atr.Length < 33)
                return CHIP_TYPE.UNKNOWN;

            Array.Resize(ref atr, atrLen);

            if (atr[13] == 0x00 && atr[14] == 0x01)
                cardType = CHIP_TYPE.MIFARE_1K;
            else if (atr[13] == 0x00 && atr[14] == 0x02)
                cardType = CHIP_TYPE.MIFARE_4K;
            else if (atr[13] == 0x00 && atr[14] == 0x03)
                cardType = CHIP_TYPE.MIFARE_ULTRALIGHT;
            else if (atr[13] == 0x00 && atr[14] == 0x3A)
                cardType = CHIP_TYPE.MIFARE_UTLRALIGHT_C;
            else
                cardType = CHIP_TYPE.UNKNOWN;
            log.LogMethodExit(cardType);
            return cardType;
        }

        internal ByteArray ReadUlc(int blockNumber)
        {
            log.LogMethodEntry(blockNumber);
            byte memoryLocation = GetUlcMemoryLocation(blockNumber);
            if (memoryLocation == 0x00)
            {
                log.LogMethodExit(null, "Invalid memory location.");
                return null;
            }
            ByteArray result = null;
            try
            {
                apduCommand = new Apdu();
                apduCommand.setCommand(new byte[]{ 0xFF,           
                    0x00,
                    0x00,
                    0x00,
                    0x02});          
                apduCommand.data = new byte[]{0x30,memoryLocation};
                apduCommand.lengthExpected = 0x10;
                sendCommand();
                log.LogVariableState("response", apduCommand.response);
                if(apduCommand.response != null && 
                   apduCommand.response.Length == 16)
                {
                    result = new ByteArray(apduCommand.response);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred reading data from the card", ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal bool WriteUlc(int blockNumber, ByteArray data)
        {
            log.LogMethodEntry(blockNumber, data);
            if (data.Length != 16)
            {
                log.LogMethodExit(false, "data size should be 16. parameter data length is " + data.Length);
                return false;
            }

            byte memoryLocation = GetUlcMemoryLocation(blockNumber);
            if (memoryLocation == 0x00)
            {
                log.LogMethodExit(false, "Invalid memory location.");
                return false;
            }

            bool result = true;
            for (byte i = 0; i < 4; i++)
            {
                result &= WriteUlcMemoryPage((byte) (memoryLocation + i), data.SubArray(i * 4, 4));
            }

            log.LogMethodExit(result);
            return result;
        }

        internal bool WriteUlcMemoryPage(byte memoryLocation, ByteArray data)
        {
            log.LogMethodEntry(memoryLocation, data);
            bool result = false;
            try
            {
                apduCommand.setCommand(new byte[]
                    {
                0xFF,
                0x00,
                0x00,
                0x00,
                0x06
                    });
                apduCommand.data = data.Prepend(new byte[] { 0xA2, memoryLocation }).Value;
                apduCommand.lengthExpected = 0x01;
                sendCommand();
                log.LogVariableState("response", apduCommand.response);

                result = apduCommand.response != null && apduCommand.response[0] == 0x0A;
            }
            catch (Exception ex)
            {
                result = false;
                log.Error("Error occurred while writing data to the ULC card", ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        private byte GetUlcMemoryLocation(int blockNumber)
        {
            log.LogMethodEntry(blockNumber);
            byte result = 0x00;
            if (blockNumber < 4 || blockNumber > 14)
            {
                log.LogMethodExit(result, "Invalid block number");
                return result;
            }

            switch (blockNumber)
            {
                case 4:
                {
                    result = 0x04;
                    break;
                }

                case 5:
                {
                    result = 0x08;
                    break;
                }

                case 6:
                {
                    result = 0x0C;
                    break;
                }

                case 7:
                {
                    result = 0x00;
                    break;
                }

                case 8:
                {
                    result = 0x10;
                    break;
                }

                case 9:
                {
                    result = 0x14;
                    break;
                }

                case 10:
                {
                    result = 0x18;
                    break;
                }

                case 11:
                {
                    result = 0x00;
                    break;
                }

                case 12:
                {
                    result = 0x1C;
                    break;
                }

                case 13:
                {
                    result = 0x20;
                    break;
                }

                case 14:
                {
                    result = 0x24;
                    break;
                }

                default:
                {
                    result = 0x00;
                    break;
                }
            }

            log.LogMethodExit(result);
            return result;
        }

        //public bool HaltCard()
        //{
        //    log.LogMethodEntry();
        //    apduCommand = new Apdu();
        //    apduCommand.setCommand(new byte[]
        //    {
        //        0xFF,
        //        0x00,
        //        0x00,
        //        0x00,
        //        0x03
        //    });
        //    apduCommand.data = new byte[] {0xD4, 0x44, 0x01};
        //    apduCommand.lengthExpected = 0x09;
        //    sendCommand();

        //    log.LogVariableState("response", apduCommand.response);
        //    log.LogMethodExit(true);
        //    return true;
        //}

        public ByteArray GetEncryptedRandB()
        {
            log.LogMethodEntry();
            apduCommand = new Apdu();
            apduCommand.setCommand(new byte[]
            {
                0xFF,
                0x00,
                0x00,
                0x00,
                0x02
            });
            apduCommand.data = new byte[] {0x1A, 0x00};
            apduCommand.lengthExpected = 0x09;
            sendCommand();

            log.LogVariableState("response", apduCommand.response);
            if (apduCommand.response == null || apduCommand.response.Length != 9 || apduCommand.response[0] != 0xAF)
            {
                string errorMessage = "Unable to get the encrypted randb from the ULC card";
                log.LogMethodExit(null, "Throwing Exception- " + errorMessage);
                throw new Exception(errorMessage);
            }

            ByteArray response = new ByteArray(apduCommand.response);
            ByteArray result = response.SubArray(1, 8);
            log.LogMethodExit(result);
            return result;
        }

        public ByteArray GetEncryptedRandAn(ByteArray encRandARandBn)
        {
            log.LogMethodEntry(encRandARandBn);
            apduCommand = new Apdu();
            apduCommand.setCommand(new byte[]
            {
                0xFF,
                0x00,
                0x00,
                0x00,
                0x11
            });
            apduCommand.data = encRandARandBn.Prepend(0xAF).Value;
            apduCommand.lengthExpected = 0x09;
            sendCommand();

            log.LogVariableState("response", apduCommand.response);
            if (apduCommand.response == null || apduCommand.response.Length != 9 || apduCommand.response[0] != 0x00)
            {
                string errorMessage = "Unable to get the encrypted randAn from the ULC card";
                log.LogMethodExit(null, "Throwing Exception- " + errorMessage);
                throw new Exception(errorMessage);
            }

            ByteArray response = new ByteArray(apduCommand.response);
            ByteArray result = response.SubArray(1, 8);
            log.LogMethodExit(result);
            return result;
        }
    }
}

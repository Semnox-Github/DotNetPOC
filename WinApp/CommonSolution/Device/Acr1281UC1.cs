/********************************************************************************************
 * Project Name - Device
 * Description  - Contains Methods and Properties related to ACR1281U-C1 device's functionality
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 1.00        19-Oct-2011     Arturo Salvamante      Created
 * 1.01        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.70.2        08-Aug-2019     Deeksha             Modified to add logger methods.
 ********************************************************************************************/

using System;

namespace Semnox.Parafait.Device
{
    internal enum CHIP_TYPE
    {
        UNKNOWN = 0,
        MIFARE_1K = 1,
        MIFARE_4K = 2,
        MIFARE_ULTRALIGHT = 3,
        MIFARE_UTLRALIGHT_C = 4,
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
    }

    internal class Acr1281UC1 : PcscReader
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
            {
                log.LogMethodExit();
                return null;
            }

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
            {
                log.LogMethodExit();
                return CHIP_TYPE.UNKNOWN;
            }

            Array.Resize(ref atr, atrLen);

            if (atr[13] == 0x00 && atr[14] == 0x01)
                cardType = CHIP_TYPE.MIFARE_1K;
            else if (atr[13] == 0x00 && atr[14] == 0x02)
                cardType = CHIP_TYPE.MIFARE_4K;
            else if (atr[13] == 0x00 && atr[14] == 0x03)
                cardType = CHIP_TYPE.MIFARE_ULTRALIGHT;
            else
                cardType = CHIP_TYPE.UNKNOWN;
            log.LogMethodExit(cardType);
            return cardType;
        }

    }
}

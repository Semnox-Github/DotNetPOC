/*===========================================================================================
 * 
 *  Copyright (C)   : Advanced Card System Ltd
 * 
 *  File            : MifareClassic.cs
 * 
 *  Description     : Contain methods and properties related to mifare classic
 * 
 *  Author          : Arturo Salvamante
 *  
 *  Date            : October 19, 2011
 * 
 *  Revision Traile : [Author] / [Date if modification] / [Details of Modifications done] 
 *=========================================================================================
 *  Modified to add Logger Methods by Deeksha on 08-Aug-2019
 * =========================================================================================*/

using System;
using System.Linq;


namespace Semnox.Parafait.Device
{
    internal class MifareClassic
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum VALUEBLOCKOPERATION
        {
            STORE = 0,
            INCREMENT = 1,
            DECREMENT = 2,
        }

        public MifareClassic(string readerName)
        {
            log.LogMethodEntry(readerName);
            _pcscConnection = new PcscReader(readerName);
            log.LogMethodExit();
        }

        public MifareClassic(PcscReader pcsc)
        {
            log.LogMethodEntry(pcsc);
            _pcscConnection = pcsc;
            log.LogMethodExit();
        }

        private PcscReader _pcscConnection;
        public PcscReader pcscConnection
        {
            get { return _pcscConnection; }
            set { _pcscConnection = value; }
        }
        
        private string getErrorMessage(byte[] sw1sw2)
        {
            log.LogMethodEntry(sw1sw2);
            if (sw1sw2.Length < 2)
            {
                string msg = "Unknown Status Word (" + Helper.byteAsString(sw1sw2, false) + ")";
                log.LogMethodExit(msg);
                return msg;
            }

            else if (sw1sw2[0] == 0x63 && sw1sw2[1] == 0x00)
            {
                string msg = "Command failed";
                log.LogMethodExit(msg);
                return msg;
            }
            else
            {
                string msg = "Unknown Status Word (" + Helper.byteAsString(sw1sw2, false) + ")";
                log.LogMethodExit(msg);
                return msg;
            }
        }

        public static bool isMifareClassic(byte[] atr)
        {
            log.LogMethodEntry(atr);
            if (atr != null && atr.Length > 8 && Helper.byteArrayIsEqual(atr.Skip(4).Take(3).ToArray(), new byte[] { 0x80, 0x4F, 0x0C }))
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public void valueBlock(byte blockNumber, VALUEBLOCKOPERATION transType, int amount)
        {
            log.LogMethodEntry(blockNumber, transType, amount);
            Apdu apdu;
            apdu = new Apdu();
            apdu.setCommand(new byte[] { 0xFF, 0xD7, 0x00, blockNumber, 0x05 });

            apdu.data = new byte[5];
            apdu.data[0] = (byte)transType;
            Array.Copy(Helper.intToByte(amount), 0, apdu.data, 1, 4);

            pcscConnection.sendCommand(ref apdu);

            if (!apdu.statusWordEqualTo(new byte[] { 0x90, 0x00 }))
                throw new CardException("Value block operation failed", apdu.statusWord);
            log.LogMethodExit();
        }

        public void store(byte blockNumber, Int32 amount)
        {
            log.LogMethodEntry(blockNumber, amount);
            valueBlock(blockNumber, VALUEBLOCKOPERATION.STORE, amount);
            log.LogMethodExit();
        }

        public void decrement(byte blockNumber, Int32 amount)
        {
            log.LogMethodEntry(blockNumber, amount);
            valueBlock(blockNumber, VALUEBLOCKOPERATION.DECREMENT, amount);
            log.LogMethodExit();
        }

        public void increment(byte blockNumber, Int32 amount)
        {
            log.LogMethodEntry(blockNumber, amount);
            valueBlock(blockNumber, VALUEBLOCKOPERATION.INCREMENT, amount);
            log.LogMethodExit();
        }

        public Int32 inquireAmount(byte blockNumber)
        {
            log.LogMethodEntry(blockNumber);
            Apdu apdu;

            apdu = new Apdu();
            apdu.setCommand(new byte[] { 0xFF, 0xB1, 0x00, blockNumber, 0x04 });
            apdu.data = null;
            apdu.lengthExpected = 4;

            pcscConnection.sendCommand(ref apdu);

            if (apdu.statusWord[0] != 0x90)
                throw new CardException("Read value failed", apdu.statusWord);
            int returnValue = Helper.byteToInt(apdu.response);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public void restoreAmount(byte sourceBlock, byte targetBlock)
        {
            log.LogMethodEntry(sourceBlock, targetBlock);
            Apdu apdu;

            apdu = new Apdu();
            apdu.lengthExpected = 2;

            apdu.setCommand(new byte[] { 0xFF, 0xD7, 0x00, sourceBlock, 0x02 });

            apdu.data = new byte[2];
            apdu.data[0] = 0x03;
            apdu.data[1] = targetBlock;

            pcscConnection.sendCommand(ref apdu);

            if (apdu.statusWord[0] != 0x90)
                throw new CardException("Restore value failed", apdu.statusWord);
            log.LogMethodExit();

        }

        public byte[] readBinary(byte blockNumber, byte length)
        {
            log.LogMethodEntry(blockNumber, length);
            Apdu apdu;
            
            apdu = new Apdu();
            apdu.setCommand(new byte[] { 0xFF, 0xB0, 0x00, blockNumber, length });
            apdu.data = new byte[0];
            apdu.lengthExpected =length;

            pcscConnection.sendCommand(ref apdu);
            if (apdu.statusWord[0] != 0x90)
                throw new CardException("Read failed", apdu.statusWord);
            byte[] returnValue = apdu.response.Take(length).ToArray();
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        
        public void updateBinary(byte blockNumber, byte[] data, byte length)
        {
            log.LogMethodEntry(blockNumber, data, length);
            Apdu apdu;

            if (data.Length > 48)
                throw new Exception("Data has invalid length");

            if (length % 16 != 0)
            {
                log.Error("Data length must be multiple of 16");
                throw new Exception("Data length must be multiple of 16");
            }

            //if (data.Length != 16)
            //    Array.Resize(ref data, 16);

            apdu = new Apdu();
            apdu.setCommand(new byte[] { 0xFF, 0xD6, 0x00, blockNumber, length });

            apdu.data = new byte[data.Length];
            Array.Copy(data, apdu.data, length);

            pcscConnection.sendCommand(ref apdu);

            if (apdu.statusWord[0] != 0x90)
            {
                log.Error("Update failed" + apdu.statusWord);
                throw new CardException("Update failed", apdu.statusWord);
            }
            log.LogMethodExit();
        }

    }
}

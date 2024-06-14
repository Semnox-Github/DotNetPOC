/********************************************************************************************
 * Project Name - Device
 * Description  - SK310UR04Listener
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2      08-Aug-2019       Deeksha        Added Logger Methods.
 ********************************************************************************************/
using SK310UR04DLL;
using System;
using System.Threading;

namespace Semnox.Parafait.Device
{
    public class SK310UR04Listener : RWCardListener
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        uint deviceHandle;
        bool USBInterface = false;
        private event EventHandler CardReaderReadComplete;
        public SK310UR04Listener(int handle)
        {
            log.LogMethodEntry(handle);
            deviceHandle = (uint)handle;
            base.init();
            log.LogMethodExit();
        }

        public override bool registerRWCardReader(ref string message, EventHandler currEventHandler)
        {
            log.LogMethodEntry(message);
            start_cardListener();
            CardReaderReadComplete = currEventHandler;
            log.LogMethodExit(true);
            return true;
        }

        public override void start_cardListener()
        {
            log.LogMethodEntry();
            cardNumber = "";
            synContext = SynchronizationContext.Current;

            stop_cardListener();

            RunThread = true;
            RWCardListenerThread = new Thread(new ThreadStart(readWrite_CardListener));
            RWCardListenerThread.IsBackground = true;
            RWCardListenerThread.Start();
            log.LogMethodExit();
        }

        protected override void readWrite_CardListener()
        {
            log.LogMethodEntry();
            byte modeOfOperation = 0x00;
            string message = "";

            while (RunThread == true && execute(MifareOperation.SELECT_OP, ref nullReferenceArray, null, ref message, modeOfOperation) != true)
            {
                Thread.Sleep(100);
            }

            if (cardNumber != "")
            {
                FireCardReadCompleteEvent(cardNumber);
            }
            log.LogMethodExit();
        }

        private void FireCardReadCompleteEvent(string CardReaderScannedValue)
        {
            log.LogMethodEntry(CardReaderScannedValue);
            synContext.Post(new SendOrPostCallback(delegate(object state)
            {
                var handler = CardReaderReadComplete;

                if (handler != null)
                {
                    handler(this, new MifareCardReaderScannedEventArgs(CardReaderScannedValue));
                }
            }), null);
            synContext.OperationCompleted();
            log.LogMethodExit();
        }

        public override bool read_data_basic_auth(int blockAddress, int numberOfBlocks, ref byte[] currentKey, ref byte[] paramReceivedData, ref string message)
        {
            log.LogMethodEntry(blockAddress, numberOfBlocks, "currentKey", paramReceivedData, message);
            byte modeOfOperation = 0x00;
            foreach (byte[] key in authKeyArray)
            {
                if (execute(MifareOperation.READ_OP, ref paramReceivedData, key, ref message, modeOfOperation, blockAddress, numberOfBlocks))
                {
                    currentKey = key;
                    log.LogMethodExit(true);
                    return true;
                }
                Thread.Sleep(50);
            }
            log.LogMethodExit(false);
            return false;
        }

        public override bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey, ref byte[] paramReceivedData, ref string message)
        {
            log.LogMethodEntry(blockAddress, numberOfBlocks, "paramAuthKey", paramReceivedData, message);
            byte modeOfOperation = 0x00;
            bool ret = execute(MifareOperation.READ_OP, ref paramReceivedData, paramAuthKey, ref message, modeOfOperation, blockAddress, numberOfBlocks);
            log.LogMethodExit(ret);
            return ret;
        }

        public override bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData, ref string message)
        {
            log.LogMethodEntry(blockAddress, numberOfBlocks, "authKey", writeData, message);
            byte modeOfOperation = 0x00;
            bool ret = execute(MifareOperation.WRITE_OP, ref writeData, authKey, ref message, modeOfOperation, blockAddress, numberOfBlocks);
            log.LogMethodExit(ret);
            return ret;
        }

        public override string readCardNumber()
        {
            log.LogMethodEntry();
            byte modeOfOperation = 0x00;
            string message = "";
            if (execute(MifareOperation.SELECT_OP, ref nullReferenceArray, null, ref message, modeOfOperation) != true)
            {
                log.LogMethodExit();
                return "";
            }
            else
            {
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
        }

        public override bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
            log.LogMethodEntry(blockAddress, "currentAuthKey", "newAuthKey", message);
            bool response;
            try
            {
                response = VerifyKey(blockAddress, currentAuthKey, ref message);
            }
            catch (Exception ex)
            {
                response = false;
                message += ex.Message;
                log.Error(message);
            }

            if (!response)
            {
                message += " Verify current key on block " + blockAddress + " with basic authentication failed.";
                log.Debug(message);
                log.LogMethodExit(false);
                return false;
            }

            response = changeKey(blockAddress, newAuthKey, ref message);
            if (!response)
            {
                message += " Change key on block " + blockAddress + " failed with new authentication.";
                log.Debug(message);
                log.LogMethodExit(false);
                return false;
            }
            message = "Successfully changed authentication key.";
            log.Debug(message);
            log.LogMethodExit(true);
            return true;
        }

        bool changeKey(int blockAddress, byte[] newKey, ref string message)
        {
            log.LogMethodEntry(blockAddress, "newKey", message);
            if (deviceHandle != 0)
            {
                byte Cm, Pm;
                UInt16 TxDataLen, RxDataLen;
                byte[] TxData = new byte[1024];
                byte[] RxData = new byte[1024];
                byte ReType = 0;
                byte St0, St1;

                Cm = 0x60;
                Pm = 0x33;
                St0 = St1 = 0;
                TxDataLen = 11;
                RxDataLen = 0;

                TxData[0] = 0x00;
                TxData[1] = 0xd5;
                TxData[2] = 0x00;
                TxData[3] = Convert.ToByte(blockAddress / 4); //SECTOR NUMBER
                TxData[4] = 0x06;

                TxData[5] = newKey[0];
                TxData[6] = newKey[1];
                TxData[7] = newKey[2];
                TxData[8] = newKey[3];
                TxData[9] = newKey[4];
                TxData[10] = newKey[5];
                
                int i = 0;
                if (USBInterface)
                {
                    i = IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }
                else
                {
                    i = IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }

                if (i == 0)
                {
                    if (ReType == 0x50)
                    {
                        switch (RxData[0])
                        {
                            case 0x90:
                                {
                                    message ="Mifare Card key reset success";
                                    log.LogMethodExit(true);
                                    return true;
                                }
                            case 0x6f:
                                {
                                    message = "Mifare Card key reset failure";
                                    break;
                                }
                            default:
                                {
                                    message = "Mifare Card key reset Error: " + RxData[0].ToString();
                                    break;
                                }
                        }
                    }
                    else if ((ReType == 0x4e))
                    {
                        message = "Cold Reset ERROR; Error Code: " + (char)St0 + (char)St1;
                    }
                    else
                    {
                        message = "Communication Error";
                    }
                }
                else
                {
                    message = "Communication Error";
                }
            }
            else
            {
                message = "Device not Connected or Comm. port is not Opened";
            }
            log.LogMethodExit(false);
            return false;
        }

        private bool execute(MifareOperation op, ref byte[] dataArray, byte[] authKey, ref string message, params object[] args)
        {
            log.LogMethodEntry(op, dataArray, "authKey", message, args);
            byte modeOfOperation = 0;
            int blockAddress = 0;
            int numberOfBlocks = 0;
            if (args.Length > 0)
            {
                try
                {
                    modeOfOperation = Convert.ToByte(args[0]);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred  while executing ConvertToByte", ex);
                }
            }
            if (args.Length > 1)
            {
                try
                {
                    blockAddress = Convert.ToInt32(args[1]);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred  while executing ConvertToInt", ex);
                }
            }
            if (args.Length > 2)
            {
                try
                {
                    numberOfBlocks = Convert.ToInt32(args[2]);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred  while executing ConvertToInt", ex);
                }
            }

            switch (op)
            {
                case MifareOperation.SELECT_OP:
                    {
                        cardNumber = "";
                        if (!ActivateCard(ref message))
                        {
                            log.LogMethodExit(false);
                            return false;
                        }
                        log.LogMethodExit(true);
                        return true;
                    }
                case MifareOperation.READ_OP:
                    {
                        if (!ActivateCard(ref message))
                        {
                            log.LogMethodExit(false);
                            return false;
                        }

                        if (!VerifyKey(blockAddress, authKey, ref message))
                        {
                            log.LogMethodExit(false);
                            return false;
                        }

                        if (deviceHandle != 0)
                        {
                            byte Cm, Pm;
                            UInt16 TxDataLen, RxDataLen;
                            byte[] TxData = new byte[1024];
                            byte[] RxData = new byte[1024];
                            byte ReType = 0;
                            byte St0, St1;

                            Cm = 0x60;
                            Pm = 0x33;
                            St0 = St1 = 0;
                            TxDataLen = 5;
                            RxDataLen = 0;

                            TxData[0] = 0x00;
                            TxData[1] = 0xb0;
                            TxData[2] = Convert.ToByte(blockAddress / 4); //SECTOR NUMBER
                            TxData[3] = Convert.ToByte(blockAddress % 4);  //Start Block
                            TxData[4] = Convert.ToByte(numberOfBlocks); //Numbers of Blocks

                            int i = 0;
                            if (USBInterface)
                            {
                                i = IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                            }
                            else
                            {
                                i = IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                            }

                            if (i == 0)
                            {
                                if (ReType == 0x50)
                                {
                                    switch (RxData[RxDataLen - 2])
                                    {
                                        case 0x90:
                                            {
                                                Array.Copy(RxData, dataArray, RxDataLen - 2);
                                                log.LogMethodExit(true);
                                                return true;
                                            }
                                        case 0x6f:
                                            {
                                                message = "Mifare Card read failure";
                                                break;
                                            }
                                        default:
                                            {
                                                message = "Mifare Card read Error: " + RxData[0].ToString();
                                                break;
                                            }
                                    }
                                }
                                else if ((ReType == 0x4e))
                                {
                                    message = "Cold Reset ERROR; Error Code:  " + (char)St0 + (char)St1;
                                }
                                else
                                {
                                    message = "Communication Error";
                                }
                            }
                            else
                            {
                                message = "Communication Error";
                            }
                        }
                        else
                        {
                            message = "Device not connected or Comm. port is not Opened";
                        }
                        log.LogMethodExit(false);
                        return false;
                    }
                case MifareOperation.WRITE_OP:
                    {
                        if (!ActivateCard(ref message))
                        {
                            log.LogMethodExit(false);
                            return false;
                        }

                        if (!VerifyKey(blockAddress, authKey, ref message))
                        {
                            log.LogMethodExit(false);
                            return false;
                        }

                        if (deviceHandle != 0)
                        {
                            byte Cm, Pm;
                            UInt16 TxDataLen, RxDataLen;
                            byte[] TxData = new byte[1024];
                            byte[] RxData = new byte[1024];
                            byte ReType = 0;
                            byte St0, St1;

                            Cm = 0x60;
                            Pm = 0x33;
                            St0 = St1 = 0;
                            TxDataLen = Convert.ToUInt16(5 + numberOfBlocks * 16);
                            RxDataLen = 0;

                            TxData[0] = 0x00;
                            TxData[1] = 0xd1;
                            TxData[2] = Convert.ToByte(blockAddress / 4); //SECTOR NUMBER
                            TxData[3] = Convert.ToByte(blockAddress % 4);  //Start Block
                            TxData[4] = Convert.ToByte(numberOfBlocks); //Numbers of Blocks

                            Array.Copy(dataArray, 0, TxData, 5, numberOfBlocks * 16);

                            int i = 0;
                            if (USBInterface)
                            {
                                i = IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                            }
                            else
                            {
                                i = IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                            }

                            if (i == 0)
                            {
                                if (ReType == 0x50)
                                {
                                    switch (RxData[0])
                                    {
                                        case 0x90:
                                            {
                                                log.LogMethodExit(true);
                                                return true;
                                            }
                                        default:
                                            {
                                                message = "Mifare Card write Error: " + RxData[0].ToString();
                                                break;
                                            }
                                    }
                                }
                                else if ((ReType == 0x4e))
                                {
                                    message = "Cold Reset ERROR; Error Code:  " + (char)St0 + (char)St1;
                                }
                                else
                                {
                                    message = "Communication Error";
                                }
                            }
                            else
                            {
                                message = "Communication Error";
                            }
                        }
                        else
                        {
                            message = "Device not connected or Comm. port is not Opened";
                        }
                        log.LogMethodExit(false);
                        return false;
                    }
                default:
                    log.LogMethodExit(false);
                    return false;
            }
        }

        public override void beep(int duration)
        {
            log.LogMethodEntry(duration);
            log.LogMethodExit();
        }

        private bool ActivateCard(ref string message)
        {
            log.LogMethodEntry(message);
            if (deviceHandle != 0)
            {
                byte Cm, Pm;
                UInt16 TxDataLen, RxDataLen;
                byte[] TxData = new byte[1024];
                byte[] RxData = new byte[1024];
                byte ReType = 0;
                byte St0, St1;

                Cm = 0x60;
                Pm = 0x30;
                St0 = St1 = 0;
                TxDataLen = 2;
                RxDataLen = 0;

                TxData[0] = 0x41;
                TxData[1] = 0x42;

                int i = 0;
                if (USBInterface)
                {
                    i = IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }
                else
                {
                    i = IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }

                if (i == 0)
                {
                    if (ReType == 0x50)
                    {
                        switch (RxData[0])
                        {
                            case 0x4d:  //mifare one Card
                                {
                                    string CardUID = "";
                                    for (int n = 0; n < RxData[3]; n++)
                                    {
                                        CardUID += RxData[n + 4].ToString("X2");
                                    }
                                    cardNumber = CardUID;
                                    log.LogMethodExit(true);
                                    return true;
                                }
                            default:
                                {
                                    message = "Invalid card type";
                                    log.LogMethodExit(false);
                                    return false;
                                }
                        }
                    }
                    else if ((ReType == 0x4e))
                    {
                        message = "Cold Reset ERROR; Error Code: " + (char)St0 + (char)St1;
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                    {
                        message = "Communication Error";
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                else
                {
                    message = "Communication Error";
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                message = "Device not connected or Comm. port is not Opened";
                log.LogMethodExit(false);
                return false;
            }
        }

        private bool VerifyKey(int blockAddress, byte[] authKey, ref string message)
        {
            log.LogMethodEntry(blockAddress, "authKey", message);
            if (deviceHandle != 0)
            {
                byte Cm, Pm;
                UInt16 TxDataLen, RxDataLen;
                byte[] TxData = new byte[1024];
                byte[] RxData = new byte[1024];
                byte ReType = 0;
                byte St0, St1;

                Cm = 0x60;
                Pm = 0x33;
                St0 = St1 = 0;
                TxDataLen = 11;
                RxDataLen = 0;

                TxData[0] = 0x00;
                TxData[1] = 0x20;
                TxData[2] = 0x00;
                TxData[3] = Convert.ToByte(blockAddress / 4); //SECTOR NUMBER
                TxData[4] = 0x06;

                TxData[5] = authKey[0];
                TxData[6] = authKey[1];
                TxData[7] = authKey[2];
                TxData[8] = authKey[3];
                TxData[9] = authKey[4];
                TxData[10] = authKey[5];

                int i = 0;
                if (USBInterface)
                {
                    i = IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }
                else
                {
                    i = IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }

                if (i == 0)
                {
                    if (ReType == 0x50)
                    {
                        switch (RxData[0])
                        {
                            case 0x90:
                                {
                                    log.LogMethodExit(true);
                                    return true;
                                }
                            case 0x6f:
                                {
                                    message = "Mifare Card key verify failure";
                                    break;
                                }
                            default:
                                {
                                    message = "Mifare one Card key verify Error: " + RxData[0].ToString();
                                    break;
                                }
                        }
                    }
                    else if ((ReType == 0x4e))
                    {
                        message = "Cold Reset ERROR; Error Code: " + (char)St0 + (char)St1;
                    }
                    else
                    {
                        message = "Communication Error";
                    }
                }
                else
                {
                    message = "Communication Error";
                }
            }
            else
            {
                message = "Device not Connected or Comm. port is not Opened";
            }
            log.LogMethodExit(false);
            return false;
        }
    }
}

/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - UCA2.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.KioskCore.CoinAcceptor
{
    public class UCA2 : CoinAcceptor
    {
        private SerialPort spUCA2 = null;
        private const byte SEQ_START = 0x90;
        private const byte SEQ_END = 0x03;
        internal class dataPacket
        {
            public int Length = 0;
            public byte[] Data = new byte[25];
        }

        private dataPacket DataPacket = new dataPacket();

        public UCA2()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public UCA2(int serialPortNum)
        {
            log.LogMethodEntry(serialPortNum);
            if (serialPortNum <= 0)
            {
                throw new ValidationException($"Invalid serial port for coin acceptor UCA2: {serialPortNum}");
            }
            portName = "COM" + serialPortNum.ToString();
            log.LogMethodExit();
        }

        public override bool ProcessReceivedData(byte[] uca2_rec, ref string message)
        {
            log.LogMethodEntry(uca2_rec, message);
            bool processReceivedSuccess = false;
            try
            {
                OpenComm();
                try
                {
                    processReceivedSuccess = MyProcessReceivedData(uca2_rec, ref message);
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile("Error in MyProcessReceivedData: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in ProcessReceivedData: " + ex.Message);
            }
            log.LogMethodExit(processReceivedSuccess);
            return processReceivedSuccess;
        }

        public bool MyProcessReceivedData(byte[] uca2_rec, ref string message)
        {
            log.LogMethodEntry(uca2_rec, message);
            try
            {
                criticalError = false;
                if (uca2_rec[0].Equals(0xff) || uca2_rec[0].Equals(0x26))
                {
                    checkStatus();
                    log.LogMethodExit(false);
                    return false;
                }
                else if (uca2_rec[0].Equals(0x00))
                {
                    message = "Coin acceptor switched off";
                    criticalError = true;
                    log.LogMethodExit(false);
                    return false;
                }

                if (uca2_rec[1].Equals(0x05)) // 5 byte response
                {
                    if (uca2_rec[2].Equals(0x11))
                    {
                        //message = "Insert Coins";
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 399);
                    }
                    else if (uca2_rec[2].Equals(0x14))
                    {
                        message = "UCA2 Disabled";
                    }
                    else if (uca2_rec[2].Equals(0x17))
                        message = "Coin Fishing";
                    else if (uca2_rec[2].Equals(0x18))
                        message = "Checksum Error";
                    else if (uca2_rec[2].Equals(0x50))
                    {
                        //message = "Insert Coins";
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 399);
                    }
                    else if (uca2_rec[2].Equals(0x4B))
                        message = "Invalid command received by UCA2";
                }
                else if (uca2_rec[1].Equals(0x06)) // 6 byte response
                {
                    if (uca2_rec[2].Equals(0x16)) //error
                    {
                        if (uca2_rec[3].Equals(0x01))
                            message = "Coin sensor 1 problem (Coil 1)";
                        else if (uca2_rec[3].Equals(0x02))
                            message = "Coin sensor 2 problem  (Coil 2)";
                        else if (uca2_rec[3].Equals(0x03))
                            message = "Sensor 3 Problem (Drop in 2)";
                        else if (uca2_rec[3].Equals(0x04))
                            message = "Sensor 4 Problem (Drop in 1)";
                        else if (uca2_rec[3].Equals(0x05))
                            message = "Sensor 5 Problem (Coin Return)";
                        else
                            message = "Unknown coin problem occured";

                        criticalError = true;
                    }
                    else if (uca2_rec[2].Equals(0x12)) //accepting
                    {
                        string returnValue = BitConverter.ToString(uca2_rec).Replace("-", "");
                        KioskStatic.logToFile("uca2_rec data: " + returnValue);
                        ReceivedCoinDenomination = Convert.ToInt32(uca2_rec[3]);
                        bool ret = true;// KioskStatic.updateAcceptance(-1, ReceivedCoinDenomination, acceptance);
                        if (ret)
                        {
                            message = KioskStatic.config.Coins[ReceivedCoinDenomination].Name + " accepted";
                            KioskStatic.logToFile(message);
                        }
                        else
                        {
                            ReceivedCoinDenomination = 0;
                            message = "Denomination " + ReceivedCoinDenomination.ToString() + " rejected";
                            KioskStatic.logToFile(message);
                        }
                        checkStatus();
                        log.LogMethodExit(ret);
                        return ret;
                    }
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                message = ex.Message + ex.StackTrace;
                log.Error(ex.Message);
                log.LogMethodExit(false);
                return false;
            }
        }

        public override void disableCoinAcceptor()
        {
            log.LogMethodEntry();
            try
            {
                OpenComm();
                try
                {
                    MyDisableCoinAcceptor();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile("Error in MyDisableCoinAcceptor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in disableCoinAcceptor: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public void MyDisableCoinAcceptor()
        {
            log.LogMethodEntry();
            if (spUCA2.IsOpen)
            {
                byte[] inp = new byte[] {
                0x90,
                0x05,
                0x02,
                0x03,
                0x9A
            };
                spUCA2.Write(inp, 0, 5);

                while (ReceivedCoinDenomination > 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(10);
                }

                CloseComm();
            }
            log.LogMethodExit();
        }

        public override bool set_acceptance(bool isTokenMode = false)
        {
            log.LogMethodEntry(isTokenMode);
            bool setAcceptanceSuccess = false;
            try
            {
                OpenComm();
                try
                {
                    setAcceptanceSuccess = MySet_Acceptance(isTokenMode);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile("Error in MySet_Acceptance: " + ex.Message);
                }
                //finally
                //{
                //   // CloseComm();
                //}
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in set_acceptance: " + ex.Message);
            }
            log.LogMethodExit(setAcceptanceSuccess);
            return setAcceptanceSuccess;
        }

        public bool MySet_Acceptance(bool isTokenMode = false)
        {
            log.LogMethodEntry(isTokenMode);
            if (spUCA2.IsOpen)
            {
                KioskStatic.logToFile("spUCA2.IsOpen: " + spUCA2.IsOpen);
                byte acceptance = 0;
                for (int i = 1; i < 9; i++)
                {
                    if (KioskStatic.config.Coins[i] != null
                        && ((isTokenMode && KioskStatic.config.Coins[i].isToken)
                         || (!isTokenMode && !KioskStatic.config.Coins[i].isToken)))
                    {
                        Int32 b = 0x01;
                        b = b << i - 1;
                        acceptance += Convert.ToByte(b);
                    }
                }
                byte[] inp = new byte[] {
                                0x90,
                                0x06,
                                0x12,
                                acceptance,
                                0x03,
                                Convert.ToByte((0x90 + 0x06 + 0x12 + acceptance + 0x03))
                            };
                spUCA2.Write(inp, 0, 6);
                KioskStatic.logToFile("acceptance: " + acceptance);
                if (acceptance > 0)
                {
                    System.Threading.Thread.Sleep(100);
                    //enable coin acceptor
                    inp = new byte[] {
                                0x90,
                                0x05,
                                0x01,
                                0x03,
                                0x99
                            };
                    spUCA2.Write(inp, 0, 5);
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(true);
                    return false;
                }
            }
            else
            {
                KioskStatic.logToFile("spUCA2.IsOpen: " + spUCA2.IsOpen);
                log.LogMethodExit(true);
                return false;
            }
        }
        public override void checkStatus()
        {
            log.LogMethodEntry();
            try
            {
                OpenComm();
                try
                {
                    MyCheckStatus();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile("Error in MyCheckStatus: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in checkStatus: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public void MyCheckStatus()
        {
            log.LogMethodEntry();
            byte[] inp = new byte[] {
                                0x90,
                                0x05,
                                0x11,
                                0x03,
                                0xA9
                            };
            spUCA2.Write(inp, 0, 5);
            log.LogMethodExit();
        }


        public override void OpenComm()
        {
            log.LogMethodEntry();
            try
            {
                if (spUCA2 == null || !spUCA2.IsOpen)
                {
                    if (spUCA2 == null)
                    {
                        spUCA2 = new System.IO.Ports.SerialPort(portName);
                        spUCA2.BaudRate = 9600;
                        spUCA2.DataBits = 8;
                        spUCA2.Parity = System.IO.Ports.Parity.None;
                        spUCA2.StopBits = System.IO.Ports.StopBits.One;

                        spUCA2.WriteTimeout = spUCA2.ReadTimeout = 1000;

                        spUCA2.DataReceived += spUCA2_DataReceived;
                    }
                    spUCA2.Open();
                    KioskStatic.logToFile("Coin Acceptor port " + spUCA2.PortName + " opened");
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error opening Coin Acceptor port " + spUCA2.PortName + " : " + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            log.LogMethodExit();
        }

        private void spUCA2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                List<byte[]> bList = new List<byte[]>();
                int totalBytesToRead = 0;
                while (spUCA2.BytesToRead > 0)
                {
                    int bytesToRead = spUCA2.BytesToRead;
                    byte[] dataRec = new byte[bytesToRead];
                    spUCA2.Read(dataRec, 0, bytesToRead);
                    if (dataRec != null && dataRec.Count() > 0)
                    {
                        bList.Add(dataRec);
                    }
                    totalBytesToRead = totalBytesToRead + bytesToRead;
                }
                if (bList != null && bList.Any())
                {
                    int byteSizeVal = bList.Sum(br => br.Count());
                    byte[] serialData = new byte[byteSizeVal];
                    int i = 0;
                    foreach (byte[] item in bList)
                    {
                        foreach (byte byteItem in item)
                        {
                            serialData[i] = byteItem;
                            i++;
                        }
                    }
                    KioskStatic.logToFile("SP CoinAcceptor BytesToRead: " + totalBytesToRead.ToString());
                    string returnValue = BitConverter.ToString(serialData).Replace("-", "");
                    KioskStatic.logToFile("SP CoinAcceptor Bytes data: " + returnValue);

                    byte[] UCA2_rec = new byte[serialData.Length + DataPacket.Length];
                    if (DataPacket.Length > 0)
                    {
                        Array.Copy(DataPacket.Data, UCA2_rec, DataPacket.Length);
                    }

                    Array.Copy(serialData, 0, UCA2_rec, DataPacket.Length, serialData.Length);
                    DataPacket.Length = 0;

                    KioskStatic.logToFile("Parsing data: " + BitConverter.ToString(UCA2_rec).Replace("-", "")); 

                    int start = 0;
                    do
                    {
                        for (; start < UCA2_rec.Length && UCA2_rec[start] != SEQ_START; start++) ;

                        if (start == UCA2_rec.Length) // start 0x90 not found
                        {
                            KioskStatic.logToFile("Invalid data sequence; Ignored");
                        }
                        else
                        {
                            if (UCA2_rec.Length > start + 1) // packet length byte (2nd byte) is also received
                            {
                                int pktLen = UCA2_rec[start + 1];

                                int end = start + pktLen - 2;
                                if (UCA2_rec.Length > end) // SEQ_END is also received
                                {
                                    if (UCA2_rec[end] == SEQ_END)
                                    {
                                        if (end == UCA2_rec.Length) // end 0x03 not found 
                                        {
                                            // store the data in DataPacket buffer and wait for more from serial port
                                            DataPacket.Length = end - start;
                                            Array.Copy(UCA2_rec, start, DataPacket.Data, 0, DataPacket.Length);
                                            start = end;

                                            KioskStatic.logToFile("Save: " + BitConverter.ToString(DataPacket.Data, 0, DataPacket.Length).Replace("-", ""));
                                        }
                                        else if (end == UCA2_rec.Length - 1) // end 0x03 found but end byte is the last byte (no checksum)
                                        {
                                            // store the data in DataPacket buffer and wait for more from serial port
                                            DataPacket.Length = end - start + 1;
                                            Array.Copy(UCA2_rec, start, DataPacket.Data, 0, DataPacket.Length);
                                            start = end + 1;

                                            KioskStatic.logToFile("Save: " + BitConverter.ToString(DataPacket.Data, 0, DataPacket.Length).Replace("-", ""));
                                        }
                                        else // valid end byte 0x03 found and checksum is available
                                        {
                                            end += 2;
                                            byte[] chunk = new byte[end - start];

                                            Array.Copy(UCA2_rec, start, chunk, 0, chunk.Length);

                                            KioskStatic.logToFile("Processing Sequence: " + BitConverter.ToString(chunk).Replace("-", ""));

                                            if (!CheckSum(chunk))
                                            {
                                                KioskStatic.logToFile("Invalid checksum: " + chunk[chunk.Length - 1].ToString("X2"));
                                            }
                                            else
                                            {
                                                KioskStatic.logToFile("Valid Data Sequence");

                                                KioskStatic.coinAcceptorDatareceived = true;

                                                if (KioskStatic.caReceiveAction != null)
                                                {
                                                    KioskStatic.coinAcceptor_rec = chunk;
                                                    KioskStatic.caReceiveAction.Invoke();
                                                }
                                            }
                                            start = end;
                                        }
                                    }
                                    else // invalid SEQ_END byte. Should be 0x03. Ignore and continue
                                    {
                                        KioskStatic.logToFile("Invalid data sequence; Ignored");
                                        start = end + 1;
                                    }
                                }
                                else // SEQ_END is not received. Save and continue.
                                {
                                    // store the data in DataPacket buffer and wait for more from serial port
                                    DataPacket.Length = UCA2_rec.Length - start;
                                    Array.Copy(UCA2_rec, start, DataPacket.Data, 0, DataPacket.Length);
                                    start = UCA2_rec.Length;

                                    KioskStatic.logToFile("Save: " + BitConverter.ToString(DataPacket.Data, 0, DataPacket.Length).Replace("-", ""));
                                }
                            }
                            else // packet length byte not received. Save and continue
                            {
                                // store the data in DataPacket buffer and wait for more from serial port
                                DataPacket.Length = UCA2_rec.Length - start;
                                Array.Copy(UCA2_rec, start, DataPacket.Data, 0, DataPacket.Length);
                                start = UCA2_rec.Length;

                                KioskStatic.logToFile("Save: " + BitConverter.ToString(DataPacket.Data, 0, DataPacket.Length).Replace("-", ""));
                            }
                        }
                    }
                    while (start < UCA2_rec.Length);
                }
                else
                {
                    KioskStatic.logToFile("SP CoinAcceptor Bytes data is empty");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in frmHome spUCA2_DataReceived: " + ex.Message);
            }
            log.LogMethodExit();
        }

        bool CheckSum(byte[] data)
        { 
            int val = 0;
            for (int i = 0; i < data.Length - 1; i++)
            {
                val += data[i];
            }
            bool isValidCheckSum = (BitConverter.GetBytes(val)[0] == data[data.Length - 1]);
            if (isValidCheckSum == false)
            {
                string returnValue = BitConverter.ToString(new byte[] { BitConverter.GetBytes(val)[0] }).Replace("-", "");
                string returnValueData = BitConverter.ToString(new byte[] { data[data.Length - 1] }).Replace("-", "");
                KioskStatic.logToFile("SP CoinAcceptor CheckSum val byte: " + returnValue + " last data byte: " +returnValueData);
            }
            return isValidCheckSum;
        }

        public override void CloseComm()
        {
            log.LogMethodEntry();
            try
            {
                if (spUCA2 != null)
                {
                    if (spUCA2.IsOpen)
                    {
                        spUCA2.Close();
                        KioskStatic.logToFile("Coin Acceptor port " + spUCA2.PortName + " Closed");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error Closing Coin Acceptor port : " + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            log.LogMethodExit();
        }
    }
}
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
 * 2.80.0      08-Jul-2020       Girish         Added Logger Methods for printer methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Semnox.Parafait.Device
{
    internal class StimaCLSLib
    {
        public enum ERROR_LIST
        {
            OPERATION_OK,
            OPERATION_NOT_OK,
            DEVICE_NOT_PLUGGED,
            TIMEOUT,
            WRONG_PARAMETER_VALUE,
            OBJECT_NULL,
            DEVICE_CLOSE,
            NO_ANSWER_FROM_DEVICE,
            SYSTEM_ERROR
        };

        public enum StatusCodes
        {
            SUCCESS = 0x00,
            NO_TRANSPONDER = 0x01,
            WRONG_DATA = 0x02,
            WRITE_ERROR = 0x03,
            ADDRESS_ERROR = 0x04,
            WRONG_TRANSPONDER_TYPE = 0x05,
            AUTHENTICATION_ERROR = 0x08,
            GENERAL_ERROR = 0x0E,
            RF_COMMUNICATION_ERROR = 0x83,
            DATA_BUFFER_OVERFLOW = 0x93,
            MORE_DATA = 0x94,
            ISO15693_ERROR = 0x95,
            ISO14443_ERROR = 0x96,
            EEPROM_FAILURE = 0x10,
            PARAMETER_RANGE_ERROR = 0x11,
            UNKNOWN_COMMAND = 0x80,
            LENGTH_ERROR = 0x81,
            COMMAND_NOT_AVAILABLE = 0x82
        }
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected internal TcpClient PRINTER = new TcpClient();
        protected internal NetworkStream NETWORKSTREAM;

        private const int bytesperlong = 4;
        private const int bitsperbyte = 8;
        private static string TO_SVELTA = Convert.ToChar(0x1C) + "<SVEL>";

        public bool IsOpen = false;
        public string PRINTER_IP_ADDRESS = "192.168.0.0";
        public int PRINTER_PORT = 9100;
        public int READ_TIMEOUT = 2000;
        public int WRITE_TIMEOUT = 5000;

        /***********************************************
         * OPEN COMMUNICATION PORT
         ***********************************************/
        public ERROR_LIST OpenPrinter()
        {
            log.LogMethodEntry();
            if (IsOpen == true)
            {
                log.LogMethodExit("ERROR_LIST.OPERATION_OK");
                return ERROR_LIST.OPERATION_OK;
            }
            System.Net.NetworkInformation.Ping ping_class = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply pingReply = ping_class.Send(PRINTER_IP_ADDRESS);
            if (pingReply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                try
                {
                    PRINTER = new System.Net.Sockets.TcpClient();
                    PRINTER.Connect(PRINTER_IP_ADDRESS, Convert.ToInt32(PRINTER_PORT));
                    NETWORKSTREAM = PRINTER.GetStream();
                    NETWORKSTREAM.ReadTimeout = READ_TIMEOUT;
                    NETWORKSTREAM.WriteTimeout = WRITE_TIMEOUT;
                    if (SetKeepAlive(PRINTER, 60000, 500) == false)
                    {
                        log.LogMethodExit("ERROR_LIST.OPERATION_NOT_OK");
                        return ERROR_LIST.OPERATION_NOT_OK;
                    }
                    IsOpen = true;
                    log.LogMethodExit("ERROR_LIST.OPERATION_OK");
                    return ERROR_LIST.OPERATION_OK;
                }
                catch
                {
                    IsOpen = false;
                    log.LogMethodExit("ERROR_LIST.OPERATION_NOT_OK");
                    return ERROR_LIST.OPERATION_NOT_OK;
                }
            }
            else
            {
                IsOpen = false;
                log.LogMethodExit("ERROR_LIST.DEVICE_NOT_PLUGGED");
                return ERROR_LIST.DEVICE_NOT_PLUGGED;
            }
        }
        /***********************************************
         * CLOSE COMMUNICATION PORT
         ***********************************************/
        public ERROR_LIST ClosePrinter()
        {
            log.LogMethodEntry();
            try
            {
                if (NETWORKSTREAM != null)
                    NETWORKSTREAM.Close();
                PRINTER.Close();
                IsOpen = false;
                log.LogMethodExit("ERROR_LIST.OPERATION_OK");
                return ERROR_LIST.OPERATION_OK;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred  while executing ClosePrinter()", ex);
                log.LogMethodExit("ERROR_LIST.OPERATION_NOT_OK");
                return ERROR_LIST.OPERATION_NOT_OK;
            }
        }
        /***********************************************
         * READ
         ***********************************************/
        public ERROR_LIST GenericRead(ref byte[] BufferRead)
        {
            log.LogMethodEntry(BufferRead);
            if (IsOpen == false)
            {
                log.LogMethodExit("ERROR_LIST.DEVICE_CLOSE");
                return ERROR_LIST.DEVICE_CLOSE;
            }

            List<byte> fifoBuffer = new List<byte>();
            try
            {
                if (NETWORKSTREAM == null)
                {
                    log.LogMethodExit("ERROR_LIST.OBJECT_NULL");
                    return ERROR_LIST.OBJECT_NULL;
                }
                if (NETWORKSTREAM.CanRead == false)
                {
                    log.LogMethodExit("ERROR_LIST.OPERATION_NOT_OK");
                    return ERROR_LIST.OPERATION_NOT_OK;
                }

                DateTime sts = DateTime.Now;
                //wait for the data to arrive
                while ((NETWORKSTREAM.DataAvailable == false) && ((DateTime.Now - sts).TotalMilliseconds < READ_TIMEOUT))
                    WAIT(100);

                WAIT(100);
                //readl all
                while (NETWORKSTREAM.DataAvailable == true)
                    fifoBuffer.Add(Convert.ToByte(NETWORKSTREAM.ReadByte()));

                WAIT(100);
                //double check
                if (NETWORKSTREAM.DataAvailable == true)
                {
                    while (NETWORKSTREAM.DataAvailable == true)
                        fifoBuffer.Add(Convert.ToByte(NETWORKSTREAM.ReadByte()));
                }

                if (fifoBuffer.Count == 0)
                {
                    BufferRead = new byte[0];
                    fifoBuffer.Clear();
                    log.LogMethodExit("ERROR_LIST.TIMEOUT");
                    return ERROR_LIST.TIMEOUT;
                }

                //copy list into array
                BufferRead = new byte[fifoBuffer.Count];
                for (int i = 0; i < BufferRead.Length; i++)
                    BufferRead[i] = fifoBuffer[i];
                fifoBuffer.Clear();
                log.LogMethodExit("ERROR_LIST.OPERATION_OK");
                return ERROR_LIST.OPERATION_OK;
            }
            catch (Exception ex)
            {
                BufferRead = new byte[0];
                fifoBuffer.Clear();
                log.Error("Error occurred  while executing GenericRead()", ex);
                log.LogMethodExit("ERROR_LIST.OPERATION_NOT_OK");
                return ERROR_LIST.OPERATION_NOT_OK;
            }
        }
        /***********************************************
         * WRITE
         ***********************************************/
        public ERROR_LIST GenericWrite(byte[] WriteBuffer)
        {
            log.LogMethodEntry(WriteBuffer);
            if (IsOpen == false)
            {
                log.LogMethodExit("ERROR_LIST.DEVICE_CLOSE");
                return ERROR_LIST.DEVICE_CLOSE;
            }

            if (NETWORKSTREAM == null)
            {
                log.LogMethodExit("ERROR_LIST.OBJECT_NULL");
                return ERROR_LIST.OBJECT_NULL;
            }

            if (NETWORKSTREAM.CanWrite == true)
                try
                {
                    if (NETWORKSTREAM.CanWrite == true)
                        NETWORKSTREAM.Write(WriteBuffer, 0, WriteBuffer.Length);
                    else
                    {
                        log.LogMethodExit("ERROR_LIST.OPERATION_NOT_OK");
                        return ERROR_LIST.OPERATION_NOT_OK;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToUpper().Contains("TIMEOUT"))
                    {
                        log.LogMethodExit("ERROR_LIST.TIMEOUT");
                        return ERROR_LIST.TIMEOUT;
                    }
                    log.LogMethodExit("ERROR_LIST.OPERATION_NOT_OK");
                    return ERROR_LIST.OPERATION_NOT_OK;
                }
            else
            {
                log.LogMethodExit("ERROR_LIST.OPERATION_NOT_OK");
                return ERROR_LIST.OPERATION_NOT_OK;
            }
            log.LogMethodExit("ERROR_LIST.OPERATION_OK");
            return ERROR_LIST.OPERATION_OK;
        }
        /***********************************************
         * FORCE SVELTA
         ***********************************************/
        public ERROR_LIST ForceSvelta()
        {
            log.LogMethodEntry();
            if (IsOpen == false)
            {
                log.LogMethodExit("ERROR_LIST.DEVICE_CLOSE");
                return ERROR_LIST.DEVICE_CLOSE;
            }
            ERROR_LIST err = ERROR_LIST.SYSTEM_ERROR;
            err = GenericWrite(System.Text.Encoding.ASCII.GetBytes(TO_SVELTA));

            if (err != ERROR_LIST.OPERATION_OK)
            {
                log.LogMethodExit(err);
                return err;
            }
            log.LogMethodExit("ERROR_LIST.OPERATION_OK");
            return ERROR_LIST.OPERATION_OK;
        }
        /***********************************************
		 * ENABLE KEEPALIVE t1-> interval t2-> time
	     ***********************************************/
        private bool SetKeepAlive(TcpClient sock, ulong t1, ulong t2)
        {
            log.LogMethodEntry(sock, t1, t2);
            try
            {
                //resulting structure
                int size = (3 * bytesperlong);
                byte[] SIO_KEEPALIVE_VALS = new byte[size];
                //array to hold input values
                ulong[] input = new ulong[3];
                //put input arguments in input array
                if ((t1 == 0) || (t2 == 0))
                    input[0] = 0;
                else
                    input[0] = 1;
                input[1] = t1; //time millis
                input[2] = t2; //interval millis
                for (int i = 0; i < input.Length; i++)
                {
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 3] = (byte)(input[i] >> ((bytesperlong - 1) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 2] = (byte)(input[i] >> ((bytesperlong - 2) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 1] = (byte)(input[i] >> ((bytesperlong - 3) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 0] = (byte)(input[i] >> ((bytesperlong - 4) * bitsperbyte) & 0xff);
                }
                byte[] result = BitConverter.GetBytes(0);
                sock.Client.IOControl(IOControlCode.KeepAliveValues, SIO_KEEPALIVE_VALS, result);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred  while executing SetKeepAlive()", ex);
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }
        /*****************************************************************
        SLEEP SW
        *****************************************************************/
        internal void WAIT(int t)
        {
            log.LogMethodEntry(t);
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < t)
            {
                System.Threading.Thread.Sleep(1);
                System.Windows.Forms.Application.DoEvents();
            }
            log.LogMethodExit();
        }
    }
}


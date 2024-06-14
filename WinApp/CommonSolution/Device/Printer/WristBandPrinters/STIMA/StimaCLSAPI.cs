/********************************************************************************************
 * Project Name - Device
 * Description  - StimaCLSAPI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2      08-Aug-2019       Deeksha        Added Logger Methods.
 * 2.120       17-Apr-2021       Guru S A       Wristband printing flow enhancements
 *             27-Dec-2021       Iqbal          Direct printing support
********************************************************************************************/
using Semnox.Parafait.Device.Printer.WristBandPrinters;
using System;

namespace Semnox.Parafait.Device
{
    internal class StimaCLSAPI
    {
        StimaCLSLib PRINTER = new StimaCLSLib();

        private string RFID_ON_SVELTA = "<COM2>";
        private string RFID_OFF_SVELTA = "<COM1>";
        private string PRINTER_STATUS = "<S1>";
        private string PRINT_STATUS = "<S3>";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private enum RFID_TAG_TYPE
        {
            UNKNOWN = 0,
            ISO15693 = 1,
            ISO14443A = 2,
            ISO14443B = 3,
            JEWEL = 4
        }
        private struct FEIG_TAG
        {
            public RFID_TAG_TYPE TYPE;
            public byte[] UID;
            public byte TR_INFO;
            public byte DATA_DIMENSION;
        }

        private FEIG_TAG TagRfid = new FEIG_TAG();
        private byte[] FEIG_RF_ON = { 0x06, 0x00, 0x6A, 0x01, 0x17, 0xC1 };
        private byte[] FEIG_RF_OFF = { 0x06, 0x00, 0x6A, 0x00, 0x9E, 0xD0 };
        private byte[] PRINTER_ANSWER = new byte[0];
        private byte[] RESTART_PRINTER_COMMAND = { 0x1C, 0xDD, 0x01, 0xA0, 0x00 }; 
        private byte[] EPOS_EMULATION_MODE = { 0x1C, 0x3C, 0x45, 0x50, 0x4F, 0x53, 0x3E };

       
        public void SetIPAddress(string IPAddress)
        {
            log.LogMethodEntry("IPAddress");
            PRINTER.PRINTER_IP_ADDRESS = IPAddress; 
            log.LogMethodExit();
        }

        public void SetIPPort(int IPPort)
        {
            log.LogMethodEntry("IPPort");
            PRINTER.PRINTER_PORT = IPPort;
            log.LogMethodExit();
        }

        public void Open()
        {
            log.LogMethodEntry();
            StimaCLSLib.ERROR_LIST retVal = PRINTER.OpenPrinter();
            if (retVal != StimaCLSLib.ERROR_LIST.OPERATION_OK)
                throw new ApplicationException("Unable to open Stima CLS RFID reader: " + retVal.ToString());

            PRINTER.ForceSvelta();
            WAIT(50);
            //enable rfid mode
            TurnOn_Off_RFID(false);
            TurnOn_Off_RFID(true);
            FEIG_POWER_ON_OFF_ANTENNA(true);

            log.LogMethodExit();
        }

        public void Print(byte[] printCommand)
        {
            log.LogMethodEntry();

            try
            {
                TurnOn_Off_RFID(false);
                WAIT(200);

                StimaCLSLib.ERROR_LIST retVal = PRINTER.GenericWrite(printCommand);

                if (retVal != StimaCLSLib.ERROR_LIST.OPERATION_OK)
                {
                    log.Error("1: Unable to write to Stima CLS Printer: " + retVal.ToString());
                    throw new ApplicationException("Unable to write to Stima CLS Printer: " + retVal.ToString());
                }

                PRINTER.GenericWrite(System.Text.Encoding.ASCII.GetBytes(PRINT_STATUS));

                DateTime waitTill = DateTime.Now.AddMilliseconds(5000);
                do
                {
                    PRINTER.GenericRead(ref PRINTER_ANSWER);
                }
                while (PRINTER_ANSWER.Length == 0 && DateTime.Now < waitTill);

                if (PRINTER_ANSWER.Length == 0)
                {
                    log.Error("2: Unable to read printer status after printing");
                    throw new ApplicationException("Unable to read printer status after printing");
                }

                if (PRINTER_ANSWER[0] != 0x06)
                {
                    log.Error("3: Print did not complete successfully [" + PRINTER_ANSWER[0].ToString("X2") + "]");
                    throw new ApplicationException("Print did not complete successfully [" + PRINTER_ANSWER[0].ToString("X2") + "]");
                }

            }
            finally
            {
                TurnOn_Off_RFID(true);
            }
            log.LogMethodExit();
        }

        public byte[] GetStatus()
        {
            log.LogMethodEntry();

            try
            {
                TurnOn_Off_RFID(false);
                WAIT(200);

                PRINTER.GenericWrite(System.Text.Encoding.ASCII.GetBytes(PRINTER_STATUS));
                PRINTER.GenericRead(ref PRINTER_ANSWER);
                if (PRINTER_ANSWER.Length == 0)
                {
                    log.Error("4: Unable to read printer status");
                    throw new ApplicationException("Unable to read printer status");
                }

                log.LogMethodExit();

                return PRINTER_ANSWER;
            }
            finally
            {
                TurnOn_Off_RFID(true);
            }
        }

        public void Close()
        {
            log.LogMethodEntry();
            try
            {
                FEIG_POWER_ON_OFF_ANTENNA(false);
                TurnOn_Off_RFID(false);
                WAIT(150);
            }
            finally
            {
                StimaCLSLib.ERROR_LIST retVal = PRINTER.ClosePrinter();
                if (retVal != StimaCLSLib.ERROR_LIST.OPERATION_OK)
                    throw new ApplicationException("Unable to close Stima CLS RFID reader: " + retVal.ToString());
            }
            log.LogMethodExit();
        }

        /*****************************************************************
        FEIG CRC16 ADDING
        *****************************************************************/
        private void FEIG_ADD_CRC_16(ref byte[] Command)
        {
            log.LogMethodEntry(Command);
            int CRC_POLYNOM = 0x8408;
            int CRC_PRESET = 0xFFFF;
            int crc = CRC_PRESET;
            for (int i = 0; i < Command.Length - 2; i++)
            {
                crc ^= Command[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) == 1)
                        crc = (crc >> 1) ^ CRC_POLYNOM;
                    else
                        crc = (crc >> 1);
                }
            }
            Command[Command.Length - 1] = Convert.ToByte(crc / 0x100);
            Command[Command.Length - 2] = Convert.ToByte(crc - (Command[Command.Length - 1] * 0x100));
            log.LogMethodExit();
        }

        public string Select()
        {
            log.LogMethodEntry();
            if (FEIG_INVENTORY() == true)
            {
                string cardNumber = "";
                int uidBytes = TagRfid.UID.Length;
                if (TagRfid.UID.Length == 7) // mifare
                    uidBytes = 4;

                for (int i = TagRfid.UID.Length - 1; i >= TagRfid.UID.Length - uidBytes; i--)
                    cardNumber += TagRfid.UID[i].ToString("X2");

                // commented for now. No need to connect / select the transponder if only UID is required. 
                //if (!FEIG_SELECT_TAG())
                //throw new ApplicationException("Unable to connect to tag");
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }

            throw new ApplicationException("Unable to read RFID or RFID band not found");
        }

        /*****************************************************************
        FEIG INVENTORY
        *****************************************************************/
        private bool FEIG_INVENTORY()
        {
            log.LogMethodEntry();
            byte[] FEIG_INVENTORY_COMMAND = { 0x07, 0x00, 0xB0, 0x01, 0x00, 0xCE, 0x93 };

            //init tag object
            TagRfid = new FEIG_TAG();
            //send inventory command
            PRINTER.GenericWrite(FEIG_INVENTORY_COMMAND);
            //analyze answer
            if (PRINTER.GenericRead(ref PRINTER_ANSWER) == StimaCLSLib.ERROR_LIST.OPERATION_OK)
            {
                //if answer is at least 6 bytes
                if (PRINTER_ANSWER.Length > 6)
                {
                    //look for 0xB0 n 0x00
                    for (int k = 0; k < PRINTER_ANSWER.Length; k++)
                    {
                        if ((PRINTER_ANSWER[k] == 0xB0) && (PRINTER_ANSWER[k + 2] > 0))
                        {
                            // PRINTER_ANSWER[k-2] = number of byte of the answer
                            // PRINTER_ANSWER[k+2] = number of tags in the antenna area
                            // PRINTER_ANSWER[k+3] = tag type

                            int numTagsIndex = k + 2;
                            int numTags = PRINTER_ANSWER[numTagsIndex];
                            int UIDStartIndex = numTagsIndex + 3;
                            int iUidLength = 8;

                            switch (PRINTER_ANSWER[numTagsIndex + 1])
                            {
                                case 0x03:
                                    {
                                        TagRfid.TYPE = RFID_TAG_TYPE.ISO15693;
                                        UIDStartIndex = numTagsIndex + 3;
                                        iUidLength = 8;
                                        break;
                                    }
                                case 0x04:
                                    {
                                        TagRfid.TYPE = RFID_TAG_TYPE.ISO14443A;
                                        UIDStartIndex = numTagsIndex + 4;
                                        iUidLength = 7;
                                        break;
                                    }
                                case 0x05:
                                    {
                                        TagRfid.TYPE = RFID_TAG_TYPE.ISO14443B;
                                        UIDStartIndex = numTagsIndex + 7;
                                        iUidLength = 4;
                                        break;
                                    }
                                case 0x08:
                                    {
                                        TagRfid.TYPE = RFID_TAG_TYPE.JEWEL;
                                        UIDStartIndex = numTagsIndex + 4;
                                        iUidLength = 6;
                                        break;
                                    }
                            }

                            //calc of the UID length
                            TagRfid.UID = new byte[iUidLength];

                            //get data dimension from the TR_INFO
                            TagRfid.TR_INFO = PRINTER_ANSWER[(k + 3) + 1];
                            if (TagRfid.TR_INFO != 0)
                                TagRfid.DATA_DIMENSION = 16;
                            else
                                TagRfid.DATA_DIMENSION = 4;
                            int q = 0;
                            //add uid
                            for (int i = UIDStartIndex; i < (UIDStartIndex + iUidLength); i++)
                            {
                                TagRfid.UID[q] = PRINTER_ANSWER[i];
                                q += 1;
                            }
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        public void LoadKey(byte[] Key)
        {
            //build write command
            log.LogMethodEntry("Key");
            byte[] FEIG_WRITE_COMMAND = new byte[13];
            FEIG_WRITE_COMMAND[0] = (byte)FEIG_WRITE_COMMAND.Length;
            FEIG_WRITE_COMMAND[1] = 0x00;
            FEIG_WRITE_COMMAND[2] = 0xA2;
            FEIG_WRITE_COMMAND[3] = 0x00;
            FEIG_WRITE_COMMAND[4] = 0x00;
            FEIG_WRITE_COMMAND[5] = Key[0];
            FEIG_WRITE_COMMAND[6] = Key[1];
            FEIG_WRITE_COMMAND[7] = Key[2];
            FEIG_WRITE_COMMAND[8] = Key[3];
            FEIG_WRITE_COMMAND[9] = Key[4];
            FEIG_WRITE_COMMAND[10] = Key[5];

            FEIG_ADD_CRC_16(ref FEIG_WRITE_COMMAND);

            //send command
            PRINTER.GenericWrite(FEIG_WRITE_COMMAND);

            StimaCLSLib.ERROR_LIST retVal = PRINTER.GenericRead(ref PRINTER_ANSWER);
            if (retVal == StimaCLSLib.ERROR_LIST.OPERATION_OK)
            {
                StimaCLSLib.StatusCodes status = Check_FEIG_ANSWER(PRINTER_ANSWER, 0xA2, 0x00, 1);

                if (status != StimaCLSLib.StatusCodes.SUCCESS)
                    throw new ApplicationException("Unable to load key. Error: " + status.ToString());
            }
            else
                throw new ApplicationException("Unable to load key. Error: " + retVal.ToString());
            log.LogMethodExit();
        }
        
        public void Authenticate(int BlockNumber)
        {
            //build write command
            log.LogMethodEntry(BlockNumber);
            byte[] FEIG_WRITE_COMMAND = new byte[10];
            FEIG_WRITE_COMMAND[0] = (byte)FEIG_WRITE_COMMAND.Length;
            FEIG_WRITE_COMMAND[1] = 0x00;
            FEIG_WRITE_COMMAND[2] = 0xB2;
            FEIG_WRITE_COMMAND[3] = 0xB0;
            FEIG_WRITE_COMMAND[4] = 0x02;
            FEIG_WRITE_COMMAND[5] = (byte)BlockNumber;
            FEIG_WRITE_COMMAND[6] = 0x00;
            FEIG_WRITE_COMMAND[7] = 0x00;

            FEIG_ADD_CRC_16(ref FEIG_WRITE_COMMAND);

            //send command
            PRINTER.GenericWrite(FEIG_WRITE_COMMAND);

            //analyze answer - look for 0xB0 0x00
            StimaCLSLib.ERROR_LIST retVal = PRINTER.GenericRead(ref PRINTER_ANSWER);
            if (retVal == StimaCLSLib.ERROR_LIST.OPERATION_OK)
            {
                StimaCLSLib.StatusCodes status = Check_FEIG_ANSWER(PRINTER_ANSWER, 0xB2, 0x00, 1);

                if (status != StimaCLSLib.StatusCodes.SUCCESS)
                    throw new ApplicationException("Unable to authenticate block. Error: " + status.ToString());
            }
            else
                throw new ApplicationException("Unable to authenticate block. Error: " + retVal.ToString());
            log.LogMethodExit();
        }

        public byte[] ReadBlock(int BlockNumber, int NumBlocks)
        {
            log.LogMethodEntry(BlockNumber, NumBlocks);
            byte[] ByteRead = new byte[0];

            //build read command
            byte[] FEIG_READ_COMMAND = new byte[9];
            FEIG_READ_COMMAND[0] = 0x9;
            FEIG_READ_COMMAND[1] = 0x00;
            FEIG_READ_COMMAND[2] = 0xB0;
            FEIG_READ_COMMAND[3] = 0x23;
            FEIG_READ_COMMAND[4] = 0x02;
            FEIG_READ_COMMAND[5] = (byte)BlockNumber;
            FEIG_READ_COMMAND[6] = (byte)NumBlocks;
            FEIG_ADD_CRC_16(ref FEIG_READ_COMMAND);

            //send command
            PRINTER.GenericWrite(FEIG_READ_COMMAND);
            //analyze answer
            if (PRINTER.GenericRead(ref PRINTER_ANSWER) == StimaCLSLib.ERROR_LIST.OPERATION_OK)
            {
                //look for 0xB0 0x00
                for (int k = 0; k < PRINTER_ANSWER.Length; k++)
                {
                    if ((PRINTER_ANSWER[k] == 0xb0) && (PRINTER_ANSWER[k + 1] == 0x00))
                    {
                        ByteRead = new byte[0];
                        if (TagRfid.TR_INFO == 0)
                        {
                            //calc of the package dimension
                            int package_dimension = PRINTER_ANSWER[k + 3] + 1;
                            //erase zeros that separates memory blocks
                            int num_zero_erase = (PRINTER_ANSWER[k - 2] - 9) / package_dimension;
                            //resize array for reading
                            ByteRead = new byte[PRINTER_ANSWER[k - 2] - 9 - num_zero_erase];
                            int index = 1;
                            int counter = 0;
                            for (int w = k + 5; w < PRINTER_ANSWER.Length - 2; w++)
                            {
                                //jump the zero that separates the data read
                                if (index == (TagRfid.DATA_DIMENSION + 1))
                                    index = 1;
                                else
                                {
                                    ByteRead[counter] = PRINTER_ANSWER[w];
                                    counter++;
                                    index++;
                                }
                            }
                        }
                        else
                        {
                            int blockSize = PRINTER_ANSWER[k + 3];
                            ByteRead = new byte[blockSize * NumBlocks];
                            int dataStartIndex = k + 5;

                            int counter = 0;
                            for (int i = 1; i <= NumBlocks; i++)
                            {
                                // ignore the block separator byte
                                for (int w = dataStartIndex + (i * 17) - 2; w >= dataStartIndex + ((i - 1) * 17); w--)
                                {
                                    ByteRead[counter] = PRINTER_ANSWER[w];
                                    counter++;
                                }
                            }
                        }
                        log.LogMethodExit(ByteRead);
                        return ByteRead;
                    }
                }
            }
            else
            {
                StimaCLSLib.StatusCodes status = Check_FEIG_ANSWER(PRINTER_ANSWER, 0xB0, 0x00, 1);
                throw new ApplicationException("Unable to read block. Error: " + status.ToString());
            }
            log.LogMethodExit();
            return null;
        }

        public void WriteBlock(int BlockNumber, int numBlocks, byte[] Data)
        {
            //build write command
            log.LogMethodEntry(BlockNumber, numBlocks, Data);
            byte[] FEIG_WRITE_COMMAND = new byte[(TagRfid.DATA_DIMENSION * numBlocks) + 10];
            FEIG_WRITE_COMMAND[0] = Convert.ToByte((TagRfid.DATA_DIMENSION * numBlocks) + 10);
            FEIG_WRITE_COMMAND[1] = 0x00;
            FEIG_WRITE_COMMAND[2] = 0xB0;
            FEIG_WRITE_COMMAND[3] = 0x24;
            FEIG_WRITE_COMMAND[4] = 0x02;
            FEIG_WRITE_COMMAND[5] = (byte)BlockNumber;
            FEIG_WRITE_COMMAND[6] = (byte)numBlocks;
            FEIG_WRITE_COMMAND[7] = TagRfid.DATA_DIMENSION;

            // ISO commands store MSB first while our data is LSB first. Hence, reverse.
            int counter = 8;
            for (int i = 1; i <= numBlocks; i++)
            {
                for (int w = (i * 16) - 1; w >= ((i - 1) * 16); w--)
                {
                    if (w > Data.Length)
                        FEIG_WRITE_COMMAND[counter] = 0x00;
                    else
                        FEIG_WRITE_COMMAND[counter] = Data[w];

                    counter++;
                }
            }

            FEIG_ADD_CRC_16(ref FEIG_WRITE_COMMAND);

            //send command
            PRINTER.GenericWrite(FEIG_WRITE_COMMAND);
            //analyze answer - look for 0xB0 0x00
            StimaCLSLib.ERROR_LIST retVal = PRINTER.GenericRead(ref PRINTER_ANSWER);
            if (retVal == StimaCLSLib.ERROR_LIST.OPERATION_OK)
            {
                StimaCLSLib.StatusCodes status = Check_FEIG_ANSWER(PRINTER_ANSWER, 0xB0, 0x00, 1);

                if (status != StimaCLSLib.StatusCodes.SUCCESS)
                    throw new ApplicationException("Unable to write to block. Error: " + status.ToString());
            }
            else
                throw new ApplicationException("Unable to write to block. Error: " + retVal.ToString());
            log.LogMethodExit();
        }

        /*****************************************************************
        FEIG SELECT
        *****************************************************************/
        private bool FEIG_SELECT_TAG()
        {
            log.LogMethodEntry();
            byte[] FEIG_SELECT_COMMAND = { 0x0F, 0x00, 0xB0, 0x25, 0x21, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int uidIndex = 6;
            switch (TagRfid.TYPE)
            {
                case RFID_TAG_TYPE.ISO15693: uidIndex = 5; break;
                case RFID_TAG_TYPE.ISO14443A: uidIndex = 6; break;
                case RFID_TAG_TYPE.ISO14443B: uidIndex = 9; break;
            }

            //add uid to the select command
            for (int i = uidIndex; i < uidIndex + TagRfid.UID.Length; i++)
                FEIG_SELECT_COMMAND[i] = TagRfid.UID[i - uidIndex];
            FEIG_SELECT_COMMAND[FEIG_SELECT_COMMAND.Length - 1] = 0x00;
            FEIG_SELECT_COMMAND[FEIG_SELECT_COMMAND.Length - 2] = 0x00;
            FEIG_ADD_CRC_16(ref FEIG_SELECT_COMMAND);

            //send select command
            PRINTER.GenericWrite(FEIG_SELECT_COMMAND);
            //analyze answer - look for 0xB0 0x00

            bool returnValue = false;
            if (PRINTER.GenericRead(ref PRINTER_ANSWER) == StimaCLSLib.ERROR_LIST.OPERATION_OK)
            {
                returnValue = (Check_FEIG_ANSWER(PRINTER_ANSWER, 0xB0, 0x00, 1) == StimaCLSLib.StatusCodes.SUCCESS);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /*****************************************************************
         CHECK ANSWER FROM FEIG
         *****************************************************************/
        private StimaCLSLib.StatusCodes Check_FEIG_ANSWER(byte[] printer_answer, byte flag, byte good_answer, int index_from_flag)
        {
            log.LogMethodEntry(printer_answer, flag, good_answer, index_from_flag);
            for (int i = 0; i < printer_answer.Length; i++)
            {
                //look for flag
                if (printer_answer[i] == flag)
                {
                    //if flag position + index is inside the buffer
                    if (i + index_from_flag < printer_answer.Length)
                    {
                        //look for a the answer from the flag position
                        if (printer_answer[i + index_from_flag] == good_answer)
                        {
                            log.LogMethodExit();
                            return StimaCLSLib.StatusCodes.SUCCESS;
                        }
                        else
                        {
                            try
                            {
                                log.LogMethodExit();
                                return (StimaCLSLib.StatusCodes)printer_answer[i + index_from_flag];
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occurred  while executing VerifyKey()", ex);
                                log.LogMethodExit();
                                return StimaCLSLib.StatusCodes.GENERAL_ERROR;
                            }
                        }
                    }
                    else
                    {
                        log.LogMethodExit();
                        return StimaCLSLib.StatusCodes.GENERAL_ERROR;
                    }
                }
            }
            log.LogMethodExit();
            return StimaCLSLib.StatusCodes.GENERAL_ERROR;
        }

        private void WAIT(int t)
        {
            log.LogMethodEntry(t);
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < t)
            {
                System.Threading.Thread.Sleep(10);
                System.Windows.Forms.Application.DoEvents();
            }
            log.LogMethodExit();
        }

        /*****************************************************************
        Enable\Disable rfid communication
        *****************************************************************/
        private void TurnOn_Off_RFID(bool enable)
        {
            log.LogMethodEntry(enable);
            if (enable == true)
            {
                //enable rfid mode
                PRINTER.GenericWrite(System.Text.Encoding.ASCII.GetBytes(RFID_ON_SVELTA));
            }
            else
            {
                //enable printer mode
                PRINTER.GenericWrite(System.Text.Encoding.ASCII.GetBytes(RFID_OFF_SVELTA));
            }
            log.LogMethodExit();
        }

        /*****************************************************************
        FEIG POWER ON\OFF ANTENNA
        *****************************************************************/
        private void FEIG_POWER_ON_OFF_ANTENNA(bool antenna_on)
        {
            //send command for powering on\off rfid antenna
            log.LogMethodEntry(antenna_on);
            if (antenna_on == true)
            {
                PRINTER.GenericWrite(FEIG_RF_ON);
            }
            else
                PRINTER.GenericWrite(FEIG_RF_OFF);

            //analyze answer - look for 0x6A 0x00
            StimaCLSLib.ERROR_LIST retVal = PRINTER.GenericRead(ref PRINTER_ANSWER);
            if (retVal == StimaCLSLib.ERROR_LIST.OPERATION_OK)
            {
                StimaCLSLib.StatusCodes status = Check_FEIG_ANSWER(PRINTER_ANSWER, 0x6A, 0x00, 1);

                if (status != StimaCLSLib.StatusCodes.SUCCESS)
                    throw new ApplicationException("Unable to turn RF " + (antenna_on ? "ON" : "OFF") + ". Error: " + status.ToString());
            }
            else
                throw new ApplicationException("Unable to turn RF " + (antenna_on ? "ON" : "OFF") + ". Error: " + retVal.ToString());
            log.LogMethodExit();
        }
        /// <summary>
        /// RestartPrinter
        /// </summary>
        public void RestartPrinter()
        {
            //send command for restarting the printer
            log.LogMethodEntry();
            try
            {
                StimaCLSLib.ERROR_LIST retVal = PRINTER.OpenPrinter();
                if (retVal != StimaCLSLib.ERROR_LIST.OPERATION_OK)
                    throw new ApplicationException("Unable to open Stima RFID reader: " + retVal.ToString());

                //Set EPOS emulation mode
                StimaCLSLib.ERROR_LIST errorList = PRINTER.GenericWrite(EPOS_EMULATION_MODE);
                if (errorList == StimaCLSLib.ERROR_LIST.OPERATION_OK)
                {
                    //Send restart Command
                    errorList = PRINTER.GenericWrite(RESTART_PRINTER_COMMAND);

                    if (errorList != StimaCLSLib.ERROR_LIST.OPERATION_OK)
                    {
                        throw new ApplicationException("Unable to restart Stima printer. Error: " + errorList.ToString());
                    }
                    //wait for restart
                    PRINTER.ForceSvelta();
                    WAIT(50);
                }
                else
                {
                    throw new ApplicationException("Unable to set Stima printer in EPOS Emulation Mode. Error: " + errorList.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            finally
            {
                StimaCLSLib.ERROR_LIST retVal = PRINTER.ClosePrinter();
                if (retVal != StimaCLSLib.ERROR_LIST.OPERATION_OK)
                    throw new ApplicationException("Unable to close Stima RFID reader: " + retVal.ToString());
            }
            log.LogMethodExit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// VerifoneHandler class
    /// </summary>
    public class VerifoneHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private System.IO.Ports.SerialPort mobjRS232;
        /// <summary>
        /// int portResponse 
        /// </summary>
        public static int portResponse = -1;
        /// <summary>
        /// int verifonePort
        /// </summary>
        public static int verifonePort = -1;       
        private const byte NUL = 0;
        private const byte SOH = 1;
        private const byte STX = 2;
        private const byte ETX = 3;
        private const byte EOT = 4;
        private const byte ENQ = 5;
        private const byte ACK = 6;
        private const byte BEL = 7;
        private const byte BS = 8;
        private const byte HT = 9;
        private const byte LF = 10;
        private const byte VT = 11;
        private const byte FF = 12;
        private const byte CR = 13;
        private const byte SO = 14;
        private const byte SI = 15;
        private const byte DLE = 16;
        private const byte DC1 = 17;
        private const byte DC2 = 18;
        private const byte DC3 = 19;
        private const byte DC4 = 20;
        private const byte NAK = 21;
        private const byte SYN = 22;
        private const byte ETB = 23;
        private const byte CAN = 24;
        private const byte EM = 25;
        private const byte SB = 26;
        private const byte ESC = 27;
        private const byte FS = 28;
        private const byte GS = 29;
        private const byte RS = 30;
        private const byte US = 31;
        private const byte DEL = 127;

        /// <summary>
        /// Instance of class CustomerAttribute
        /// </summary>
        public CustomerAttribute customerAttribute = new CustomerAttribute();

        /// <summary>
        /// CustomerAttribute Class
        /// </summary>
        public class CustomerAttribute
        {
            /// <summary>
            /// string CardNumber
            /// </summary>
            public string CardNumber;
            /// <summary>
            /// string PinBlock
            /// </summary>
            public string PinBlock;
            /// <summary>
            /// string PinData
            /// </summary>
            public string PinData;
            /// <summary>
            /// string PinSerial
            /// </summary>
            public string PinSerial;
            /// <summary>
            /// string ExpDate
            /// </summary>
            public string ExpDate;
            /// <summary>
            /// string Track1data
            /// </summary>
            public string Track1data;
            /// <summary>
            /// string Track2data
            /// </summary>
            public string Track2data;
            /// <summary>
            /// string Track3data
            /// </summary>
            public string Track3data;
            /// <summary>
            /// string statusCode
            /// </summary>
            public string statusCode;
            /// <summary>
            /// bool isSwipe
            /// </summary>
            public bool isSwipe = false;
        }
        //All following variables are used to stored the binary commands with LRC     
        /// <summary>
        /// byte[] CANCELL_SESSION_REQUEST_72
        /// </summary>
        public static byte[] CANCELL_SESSION_REQUEST_72;
        /// <summary>
        /// byte[] GET_ENCRYPTION_MODE_E00
        /// </summary>
        public static byte[] GET_ENCRYPTION_MODE_E00;
        private static byte[] DERIVED_KEY_SUPPORT_E20;
        private static byte[] DISPLAY_BANK_RESPONSE_MSG_S09;
        private static byte[] DISPLAY_MENU_S13;
        private static byte[] DISPLAY_MEGs_S14;
        private static byte[] OBTAIN_CARD_DATA_FROM_MANUALENTRY_OR_SWIPE_S16;
        private static byte[] OBTAIN_CARD_DATA_FROM_SWIPE_OR_TAP_S20;
        private static byte[] DATA_ENTRY_REQUEST_S21;
        private static byte[] ACCEPT_AND_ENCRYPT_PIN_DISPLAY_CUSTOM_MSGs_Z62;

        /// <summary>
        /// Constructor
        /// </summary>
        public VerifoneHandler()
        {
            log.LogMethodEntry();
            generateCommand();
            log.LogMethodExit(null);
        }
        private void generateCommand()
        {
            log.LogMethodEntry();

            string cmdString = "";
            //Cancel Session Request
            cmdString = Convert.ToChar(STX) + "72" + Convert.ToChar(ETX);
            binaryCommandGenerator(ref CANCELL_SESSION_REQUEST_72, cmdString);

            //Registration Track Data
            cmdString = Convert.ToChar(STX) + "E2000" + Convert.ToChar(ETX);
            binaryCommandGenerator(ref DERIVED_KEY_SUPPORT_E20, cmdString);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// DisplayBankResponse Method
        /// </summary>
        /// <param name="ToplineMsg"></param>
        /// <param name="BottomLineMsg"></param>
        public void displayBankResponse(string ToplineMsg, string BottomLineMsg)
        {
            log.LogMethodEntry(ToplineMsg, BottomLineMsg);

            //Displaying bank response
            string cmdString = "";
            cmdString = Convert.ToChar(STX) + "S09" + ToplineMsg + Convert.ToChar(FS) + BottomLineMsg + Convert.ToChar(ETX);
            binaryCommandGenerator(ref DISPLAY_BANK_RESPONSE_MSG_S09, cmdString);            
            SendCommand(DISPLAY_BANK_RESPONSE_MSG_S09);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Display Message method
        /// </summary>
        /// <param name="Msg1"></param>
        /// <param name="Msg2"></param>
        /// <param name="Msg3"></param>
        /// <param name="Msg4"></param>
        /// <param name="timeOutSec"></param>
        public void displayMessages(string Msg1, string Msg2, string Msg3, string Msg4, int timeOutSec)
        {
            log.LogMethodEntry(Msg1, Msg2, Msg3, Msg4, timeOutSec);

            //displaying other message
            string cmdString = "";            
            cmdString = Convert.ToChar(STX) + "S14" + Msg1 + Convert.ToChar(FS) + Msg2 + Convert.ToChar(FS) + Msg3 + Convert.ToChar(FS) + Msg4 + Convert.ToChar(FS) + timeOutSec.ToString().PadLeft(5, '0') + Convert.ToChar(ETX);
            binaryCommandGenerator(ref DISPLAY_MEGs_S14, cmdString);            
            SendCommand(DISPLAY_MEGs_S14);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// This function is used to prompt read card.Set port read time out before calling this function
        /// </summary>
        /// <param name="amount">This will be displayed on pin pad and need to be multiplied by 100.i.e., If amount=1234 it displayed as 12.34.  </param>
        /// <param name="timeOutSec"> This is the time out value.For read card it is between 0-255 seconds  </param>             
        public bool ReadCard(double amount, int timeOutSec = 60)
        {
            log.LogMethodEntry(amount, timeOutSec);

            string cmdString = "";
            int portNumber;
            portNumber = portResponse;
            int intEnd;
            int intStart;
            string Response;
            bool status = false;
            try
            {
                if (amount > 0)
                {
                    cmdString = Convert.ToChar(STX) + "S20" + timeOutSec.ToString().PadLeft(3, '0') + Convert.ToChar(FS) + amount.ToString() + Convert.ToChar(ETX);
                }
                else
                {
                    cmdString = Convert.ToChar(STX) + "S20" + timeOutSec.ToString().PadLeft(3, '0') + Convert.ToChar(FS) + Convert.ToChar(ETX);
                    //return false;
                }
                binaryCommandGenerator(ref OBTAIN_CARD_DATA_FROM_SWIPE_OR_TAP_S20, cmdString);
                status = SendCommand(OBTAIN_CARD_DATA_FROM_SWIPE_OR_TAP_S20, timeOutSec * 1000);
                if (!status)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                Response = ReadResponse();
                customerAttribute.statusCode = Response.Substring(1, 2);
                if (Response.Length > 16)
                {
                    customerAttribute.isSwipe = true;
                    intEnd = Response.IndexOf("=");
                    if (intEnd > 5)
                    {
                        customerAttribute.CardNumber = Response.Substring(4, intEnd - 4);
                        customerAttribute.ExpDate = Response.Substring(intEnd + 1, 4);
                        intStart = Response.IndexOf(Convert.ToChar(FS));
                        customerAttribute.Track2data = Response.Substring(3, intStart - 3);
                        intEnd = Response.IndexOf(Convert.ToChar(FS), intStart + 1);
                        if (intEnd > intStart)
                        {
                            customerAttribute.Track1data = Response.Substring(intStart + 1, intEnd - intStart - 1);
                        }
                        intStart = intEnd;
                        intEnd = Response.IndexOf(Convert.ToChar(FS), intStart + 1);
                        if (intEnd > intStart)
                        {
                            customerAttribute.Track3data = Response.Substring(intStart + 1, intEnd - intStart - 1);
                        }
                        intEnd = Response.IndexOf(Convert.ToChar(ETX), intEnd + 1);
                    }
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
                log.LogMethodExit(true);
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Error while reading the card", ex);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                closeVerifonePort();
            }
        }

        /// <summary>
        /// This function is used to prompt read card.Set port read time out before calling this function
        /// </summary>
        /// <param name="timeOutSec"> This is the time out value.For read card it is between 0-255 seconds  </param>               
        public bool ReadCardEntryorSwipe(int timeOutSec = 60)
        {
            log.LogMethodEntry(timeOutSec);

            string cmdString = "";
            int portNumber;
            portNumber = portResponse;
            int intEnd;
            int intStart;
            string Response;
            bool status = false;
            try
            {
                cmdString = Convert.ToChar(STX) + "S160" + Convert.ToChar(FS) + "0103" + Convert.ToChar(FS) + "2" + Convert.ToChar(FS) + "0" + Convert.ToChar(FS) + timeOutSec.ToString().PadLeft(3, '0') + Convert.ToChar(ETX);//" + timeOutSec.ToString().PadLeft(3, '0') "
                binaryCommandGenerator(ref OBTAIN_CARD_DATA_FROM_MANUALENTRY_OR_SWIPE_S16, cmdString);                
                status = SendCommand(OBTAIN_CARD_DATA_FROM_MANUALENTRY_OR_SWIPE_S16, timeOutSec*1000);
                if (!status)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                Response = ReadResponse();                
                customerAttribute.statusCode = Response.Substring(1, 2);

                intStart = 3;
                intEnd = Response.IndexOf(Convert.ToChar(FS), intStart + 1);
                if ((intEnd - intStart) > 13)//14
                {
                    //ManualEntry
                    customerAttribute.isSwipe = false;
                    customerAttribute.CardNumber = Response.Substring(intStart, intEnd - intStart);
                    customerAttribute.Track2data = Response.Substring(intStart, intEnd - intStart);
                    intStart = intEnd + 1; intEnd = Response.IndexOf(Convert.ToChar(FS), intStart);
                    customerAttribute.Track2data = Response.Substring(intStart, intEnd - intStart) + customerAttribute.Track2data;
                    customerAttribute.ExpDate = customerAttribute.Track2data.Substring(customerAttribute.Track2data.IndexOf("=") + 1, 4);
                }
                else
                {
                    intStart = intEnd;
                    intEnd = Response.IndexOf(Convert.ToChar(ETX), intStart + 1);
                    if ((intEnd - intStart) > 17)//14+4
                    {
                        customerAttribute.isSwipe = true;
                        intStart = Response.IndexOf(";", intStart + 1);
                        intEnd = Response.IndexOf("?", intStart + 1) + 1;
                        customerAttribute.Track2data = Response.Substring(intStart, intEnd - intStart);
                        customerAttribute.CardNumber = customerAttribute.Track2data.Substring(1, customerAttribute.Track2data.IndexOf("=", intStart + 1) - 1);
                        customerAttribute.ExpDate = customerAttribute.Track2data.Substring(customerAttribute.Track2data.IndexOf("=") + 1, 4);
                    }
                }

                log.LogMethodExit(true);
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while reading card entry or swiping", ex);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                closeVerifonePort();
            }
        }

        /// <summary>
        /// GetRegiStart method
        /// </summary>
        public void GetRegiStart()
        {
            log.LogMethodEntry();

            //verifone register track retrieved from this function
            int portNumber;
            portNumber = portResponse;
            int intEnd;
            int intStart;
            string Response;
            try
            {               
                SendCommand(DERIVED_KEY_SUPPORT_E20);                
                Response = ReadResponse();
                if (Response.Length < 16)
                {
                    Response = ReadResponse();
                }
                if (Response.Length > 16)
                {
                    intEnd = Response.IndexOf("=");
                    if (intEnd > 5)
                    {
                        intStart = Response.IndexOf(Convert.ToChar(FS));
                        customerAttribute.CardNumber = Response.Substring(intStart + 2, intEnd - intStart - 2);
                        customerAttribute.ExpDate = Response.Substring(intEnd + 1, 4);
                        intEnd = Response.IndexOf(Convert.ToChar(FS), intStart + 1);
                        customerAttribute.Track2data = Response.Substring(intStart + 1, intEnd - intStart - 1);
                        intStart = intEnd;
                        intEnd = Response.IndexOf(Convert.ToChar(FS), intStart + 1);
                        if (intEnd > intStart)
                        {
                            customerAttribute.Track1data = Response.Substring(intStart + 1, intEnd - intStart - 1);
                        }
                        intStart = intEnd;
                        intEnd = Response.IndexOf(Convert.ToChar(FS), intStart + 1);
                        if (intEnd > intStart)
                        {
                            customerAttribute.Track3data = Response.Substring(intStart + 1, intEnd - intStart - 1);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while retrieving register track", ex);
            }

            log.LogMethodExit(null);
        }
        
        /// <summary>
        /// This function is used to request pin entry
        /// </summary>
        /// <param name="AcountNo"> should be greater than 8 digits and less than 20 digits </param>
        /// <param name="NullPinAllowed"> this is an optional argument by defualt it takes as 'false' value.If it is 'true' then user can skip pin entry by pressing Enter </param>            
        /// <param name="MinLength"> Is an optional argument.MinLength should be greater than 3 and less than or equal to 12 </param>
        /// <param name="MaxLength"> Is an optional argument.MaxLength should be less than or equal to 12 </param>        
        /// <param name="MessageLine1"> Is an optional argument.This message will be displayed in the pinpad. </param>
        /// <param name="MessageLine2"> Is an optional argument.This message will be displayed in the pinpad. </param>
        /// <param name="timeOutSec"> Timeout time. </param>
        public bool PinRequest(string AcountNo, bool NullPinAllowed = false, int MinLength = 4, int MaxLength = 12, string MessageLine1 = "Enter PIN ", string MessageLine2 = "", int timeOutSec = 0)
        {
            log.LogMethodEntry(AcountNo, NullPinAllowed, MinLength, MaxLength, MessageLine1, MessageLine2, timeOutSec);

            string cmdString = "";
            string pin = "";
            string Response = "";
            bool status = false;
            try
            {
                pin = (NullPinAllowed) ? "Y" : "N";
                if (AcountNo.Length > 13 && AcountNo.Length < 20)
                {
                    cmdString = Convert.ToChar(STX) + "Z62." + AcountNo + Convert.ToChar(FS) + MinLength.ToString().PadLeft(2, '0') + MaxLength.ToString().PadLeft(2, '0') + pin + MessageLine1 + Convert.ToChar(FS) + MessageLine2 + Convert.ToChar(FS) + "Processing..." + Convert.ToChar(ETX);
                    binaryCommandGenerator(ref ACCEPT_AND_ENCRYPT_PIN_DISPLAY_CUSTOM_MSGs_Z62, cmdString);                    
                    status=SendCommand(ACCEPT_AND_ENCRYPT_PIN_DISPLAY_CUSTOM_MSGs_Z62,timeOutSec*1000);
                    if (!status)
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                    Response = ReadResponse();
                    if (Response.Equals("1"))
                    {
                        customerAttribute.statusCode = "01";
                        log.LogMethodExit(false);
                        return false;
                    }
                    else if(Response.Equals("002"))
                    {
                        customerAttribute.statusCode = "02";
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                    {
                        customerAttribute.statusCode = "00";
                    }
                    if (Response.Length > 5)//This if is put to avoid exception from next statement. 
                    {
                        customerAttribute.PinData = Response.Substring(4, Response.Length - 5);
                        if (customerAttribute.PinData.Length > 16)
                        {
                            customerAttribute.PinBlock = customerAttribute.PinData.Substring(customerAttribute.PinData.Length - 16);
                            customerAttribute.PinSerial = customerAttribute.PinData.Substring(0, customerAttribute.PinData.Length - 16);                            
                        }                        
                    }
                    log.LogMethodExit(true);
                    return true;
                }
                log.LogMethodExit(false);
                return false;
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while pinning request", ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// This function is used to add the other Charges.
        /// </summary>
        /// <param name="CurrentTrxAmount"> Is the transaction amount used to display. </param>
        /// <param name="TipFlag"> Prompt for tip and collect the amount entered if this flag is set to true</param>            
        /// <param name="CashBackFlag"> Prompt for cash back, and collect the amount entered if this flag is set to true</param>
        /// <param name="surChargeFlag"> Prompt for surcharge amount confirmation if this flag is set to true </param>
        /// <param name="surChargeAmount"> Optional, Surcharge amount that is to be displayed and confirmed if the Surcharge Flag is set to true (valid range is 1-9999). </param>
        public string OtherCharges(double CurrentTrxAmount, bool TipFlag = false, bool CashBackFlag = false, bool surChargeFlag = false, double surChargeAmount = 0)
        {
            log.LogMethodEntry(CurrentTrxAmount, TipFlag, CashBackFlag, surChargeFlag, surChargeAmount);

            string cmdString = "";
            string Response = "";
            int portNumber;
            string Tip = "", CashBack = "", surCharge = "";
            portNumber = portResponse;
            try
            {
                Tip = (TipFlag) ? "1" : "0";
                CashBack = (CashBackFlag) ? "1" : "0";
                surCharge = (surChargeFlag) ? "1" : "0";
                if (surCharge.Equals("1"))
                {
                    if (!(surChargeAmount > 0))
                    {
                        log.LogMethodExit("Error:Surcharge Amount");
                        return "Error:Surcharge Amount";
                    }
                }
                cmdString = Convert.ToChar(STX) + "S210." + CurrentTrxAmount.ToString(".00") + Convert.ToChar(FS) + Tip + CashBack + surCharge + Convert.ToChar(FS) + surChargeAmount.ToString(".00") + Convert.ToChar(ETX);
                binaryCommandGenerator(ref DATA_ENTRY_REQUEST_S21, cmdString);               
                SendCommand(DATA_ENTRY_REQUEST_S21);                
                Response = ReadResponse();

                log.LogMethodExit(Response);
                return Response;
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while making other changes", ex);
                log.LogMethodExit("Error");
                return "Error";
            }
        }

        /// <summary>
        /// This option used to display the user menu.
        /// </summary>
        /// <param name="MessageLine1"> This message will be displayed on pinpad.Up to 16 characters </param>
        /// <param name="Choice1"> This will display the first option button on pinpad.Up to 16 characters </param>            
        /// <param name="MessageLine2"> Is an optional argument to display the remaining part of MessageLine1.Up to 16 characters. </param>
        /// <param name="Choice2"> Is an optional argument.Displays the second option button on pinpad.Up to 16 characters</param>
        /// <param name="Choice3"> Is an optional argument.Displays the third option button on pinpad.Up to 16 characters</param>
        /// <param name="Choice4"> Is an optional argument.Displays the fourth option button on pinpad.Up to 16 characters</param>
        public string UserOptions(string MessageLine1, string Choice1, string MessageLine2 = "", string Choice2 = "", string Choice3 = "", string Choice4 = "")
        {
            log.LogMethodEntry(MessageLine1, Choice1, MessageLine2, Choice2, Choice3, Choice4);

            string cmdString = "";
            string Response = "";
            try
            {
                cmdString = Convert.ToChar(STX) + "S13" + MessageLine1;
                //if (!string.IsNullOrEmpty(MessageLine2))
                //{
                    cmdString += Convert.ToChar(FS) + MessageLine2;
                //}
                cmdString += Convert.ToChar(FS) + Choice1;
                //if (!string.IsNullOrEmpty(Choice2))
                //{
                    cmdString += Convert.ToChar(FS) + Choice2;
                //}
                //if (!string.IsNullOrEmpty(Choice3))
                //{
                    cmdString += Convert.ToChar(FS) + Choice3;
                //}
                //if (!string.IsNullOrEmpty(Choice4))
                //{
                    cmdString += Convert.ToChar(FS) + Choice4;
                //}
                cmdString += Convert.ToChar(ETX);
                binaryCommandGenerator(ref DISPLAY_MENU_S13, cmdString);                
                SendCommand(DISPLAY_MENU_S13);                
                Response = ReadResponse();

                log.LogMethodExit(Response);
                return Response;
            }
            catch(Exception ex)
            {
                log.Error("Error occurred  while enabling User Options", ex);

                log.LogMethodExit("Error");
                return "Error";
            }
        }

        /// <summary>
        /// This function is used send the commands like GET_ENCRYPTION_MODE_E00 etc
        /// returns the boolean type result
        /// </summary>
        /// <param name="Command"> Is the byte array of command with LRC</param>   
        /// <param name="ReadTimeOut"> Read Time out parameter </param>           
        public bool SendCommand(byte[] Command, int ReadTimeOut = 0)
        {
            log.LogMethodEntry(Command, ReadTimeOut);

            int b;
            int portnumber;
            int nCounter = 10;
            int count = 3;
            try
            {
                Thread.Sleep(300);
                portnumber = portResponse;
                if (portnumber != -1)
                {
                    closeVerifonePort();
                }
                openVerifonePort(portnumber, ReadTimeOut);
                while (true)//if NAK is received then packet should be resent 3 times
                {
                    Thread.Sleep(150);//There should be at least 100 ms delay between sending and recieving commands
                    mobjRS232.Write(Command, 0, Command.Length);
                    mobjRS232.ReadTimeout = 2000;
                    Thread.Sleep(150);//There should be at least 100 ms delay between sending and recieving commands
                    while (true)
                    {
                        b = mobjRS232.ReadByte();
                        if (b == NAK)
                        {
                            count--;
                            if (count == 0)
                            {
                                log.LogMethodExit(false);
                                return false;
                            }
                            break;
                        }
                        else if (b == ACK)
                        {
                            if (ReadTimeOut > 0)
                            {
                                mobjRS232.ReadTimeout = ReadTimeOut;
                            }
                            else
                            {
                                mobjRS232.ReadTimeout = -1;
                            }

                            log.LogMethodExit(true);
                            return true;
                        }
                        else
                        {
                            nCounter--;
                            if (nCounter == 0)
                            {
                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while sending command", ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        private void binaryCommandGenerator(ref byte[] CmdArray, string cmdstring)
        {
            log.LogMethodEntry(CmdArray, cmdstring);

            //converting the passed command to the byte array
            byte[] abytSendData;
            byte byteLRC;
            abytSendData = new byte[cmdstring.Length];
            for (int i = 0; i < cmdstring.Length; i++)
            {
                abytSendData[i] = Convert.ToByte(cmdstring[i]);
            }
            byteLRC = ComputeLRC2(abytSendData);
            CmdArray = new byte[abytSendData.Length + 1];
            for (int i = 0; i < abytSendData.Length; i++)
            {
                CmdArray[i] = abytSendData[i];
            }
            CmdArray[abytSendData.Length] = byteLRC;

            log.LogMethodExit(null);
        }

        private string ReadResponse()
        {
            log.LogMethodEntry();

            //reading pinpad response and returns as string
            bool stop = false;
            bool StopCharRecieved = false;
            int responseByte;
            int count = 2;
            byte LRC;
            string mstrDataRecieved = "";
            try
            {
                while (count > 0)
                {
                    Thread.Sleep(150);//There should be at least 100 ms delay between sending and recieving commands
                    while (!stop)
                    {
                        responseByte = mobjRS232.ReadByte();
                        if (responseByte == EOT)
                        {
                            log.LogMethodEntry("1");
                            return "1";
                        }
                        if (responseByte == STX)
                        {
                            while (!StopCharRecieved)
                            {
                                if (responseByte > 0)
                                {
                                    mstrDataRecieved += Convert.ToChar(responseByte).ToString();
                                }
                                if (responseByte == ETX)
                                {
                                    StopCharRecieved = true;
                                }
                                responseByte = mobjRS232.ReadByte();
                            }
                            if (StopCharRecieved)
                            {
                                LRC = ComputeLRC(mstrDataRecieved.Substring(1));
                                if (LRC == responseByte)
                                {
                                    stop = true;
                                    count = 0;
                                    break;
                                }
                                else
                                {
                                    mstrDataRecieved = "";
                                    count--;
                                    SendResponse(false);
                                    break;
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(150);//There should be at least 100 ms delay between sending and recieving commands
                SendResponse(true);

                //log.LogMethodExit(mstrDataRecieved);
                return mstrDataRecieved;
            }
            catch (System.TimeoutException ex)
            {
                log.Error("Caught a TimeoutException - Error occurred while performing timeout", ex);
                Thread.Sleep(150);
                SendCommand(CANCELL_SESSION_REQUEST_72);
                Thread.Sleep(150);
                log.LogMethodExit("002");
                return "002";

            }
            catch(Exception ex)
            {
                log.Error("Error occurred while performing timeout", ex);
                log.LogMethodExit("Error");
                return "Error.";
            }
            finally
            {
                closeVerifonePort();
            }
        }

        private void SendResponse(bool isACK)
        {
            log.LogMethodEntry(isACK);

            byte[] cmd = new byte[1];
            if (isACK)
            {
                cmd[0] = ACK;
            }
            else
            {
                cmd[0] = NAK;
            }            
            mobjRS232.Write(cmd, 0, 1);

            log.LogMethodExit(null);
        }        

        /// <summary>
        /// This function is used open the pinpad Port
        /// returns the boolean type result
        /// </summary>
        /// <param name="PortNumber"> The port no which is need to be opened</param>   
        /// <param name="ReadTimeOut"> Is an Optional argument. This value will be in seconds. Which is set before reading the port. This time out will keep the port open for the secified time period</param>   
        public bool openVerifonePort(int PortNumber, int ReadTimeOut = 0)
        {
            log.LogMethodEntry(PortNumber, ReadTimeOut);

            bool bolSuccess = true;
            if (PortNumber < 0)
            {
                PortNumber = verifonePort;
            }
            System.IO.Ports.Parity objParity;
            try
            {
                closeVerifonePort();
                portResponse = -1;
                objParity = System.IO.Ports.Parity.Even;
                if (ReadTimeOut > 0)
                {
                    mobjRS232.ReadTimeout = ReadTimeOut;
                }
               
                bolSuccess = commSettings(PortNumber, objParity);
                if (bolSuccess)
                {
                    portResponse = PortNumber;
                    mobjRS232.DiscardOutBuffer();                    
                }
                else
                {
                    portResponse = -1;
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while opening Verifone Port", ex);
                bolSuccess = false;
            }

            log.LogMethodExit(bolSuccess);
            return bolSuccess;
        }
        private bool commSettings(int intPort, System.IO.Ports.Parity parity)
        {
            log.LogMethodEntry(intPort, parity);

            //opening the port
            bool bolOK = true;
            if (intPort >= 1 && intPort <= 30)
            {                
                mobjRS232 = new System.IO.Ports.SerialPort("COM" + intPort.ToString(), 115200, parity, 7, System.IO.Ports.StopBits.One);
                mobjRS232.Handshake = System.IO.Ports.Handshake.None;
                try
                {
                    mobjRS232.Open();
                    mobjRS232.DiscardOutBuffer();
                }
                catch(Exception ex)
                {
                    log.Error("Error occurred while opening the port", ex);
                    bolOK = false;
                }
            }

            log.LogMethodExit(bolOK);
            return bolOK;
        }

        /// <summary>
        /// Closing the opened port.       
        /// </summary>
        public void closeVerifonePort()
        {
            log.LogMethodEntry();

            if (mobjRS232 != null)
            {
                if (portResponse >= 1 && portResponse <= 30)
                {
                    mobjRS232.Close();
                }
            }
            portResponse = -1;

            log.LogMethodExit(null);
        }

        private byte ComputeLRC2(byte[] b)
        {
            log.LogMethodEntry(b);

            //computes LRC before sending data to the port
            byte bytChar = 0;
            for (int i = 1; i < b.Length; i++)
            {
                bytChar ^= b[i];//Xor operation
            }

            log.LogMethodExit(bytChar);
            return bytChar;
        }

        private byte ComputeLRC(string s)
        {
            log.LogMethodEntry(s);

            //compute LRC on response data from port
            byte bytChar;
            byte[] bytearray;
            bytearray = Encoding.ASCII.GetBytes(s);
            bytChar = 0;
            for (int i = 0; i < bytearray.Length; i++)
            {
                bytChar ^= bytearray[i];
            }

            log.LogMethodExit(bytChar);
            return bytChar;
        }
    }
}

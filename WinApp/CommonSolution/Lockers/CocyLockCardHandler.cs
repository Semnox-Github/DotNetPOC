/********************************************************************************************
 * Project Name - CocyLockCardHandler
 * Description  - Cocy Lock handling logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value 
 *2.130.00    31-Aug-2018      Dakshakh raj   Modified create guest card method to add locker make parameter 
 ********************************************************************************************/
//using Semnox.Core.Utilities;
using Semnox.Core.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Semnox.Parafait.Device.Lockers
{

    /// <summary>
    /// Cocy Locker Lock business logic 
    /// </summary>
    public class CocyLockCardHandler : ParafaitLockCardHandler
    {

        /// <summary>
        /// Auth key
        /// </summary>
        protected byte[] authKey = new byte[] { 0x11, 0x1C, 0x6C, 0x9C, 0x62, 0x22 };//Moved to child  Encoding.Default.GetBytes(Encryption.GetParafaitKeys("CocyInnovate"));//= 
        /// <summary>
        /// SYSTEM_CARD_CODE
        /// </summary>
        public const byte SYSTEM_CARD_CODE = 0xF1;
        /// <summary>
        /// SETUP_CARD_CODE
        /// </summary>
        public const byte SETUP_CARD_CODE = 0x97;
        /// <summary>
        /// MASTER_CARD_CODE
        /// </summary>
        public const byte MASTER_CARD_CODE = 0xE2;
        /// <summary>
        /// CLOCK_CARD_CODE
        /// </summary>
        public const byte CLOCK_CARD_CODE = 0xD3;
        /// <summary>
        /// PARAMETER_CARD_CODE
        /// </summary>
        public const byte PARAMETER_CARD_CODE = 0xC4;
        /// <summary>
        /// DISABLE_CARD_CODE
        /// </summary>
        public const byte DISABLE_CARD_CODE = 0x88;
        /// <summary>
        /// GUEST_CARD_CODE
        /// </summary>
        public const byte GUEST_CARD_CODE = 0xB5;
        /// <summary>
        /// COMMAND_CODE_POSITION
        /// </summary>
        public const int COMMAND_CODE_POSITION = 0;
        /// <summary>
        /// FROM_YEAR_POSITION
        /// </summary>
        public const int FROM_YEAR_POSITION = 4;
        /// <summary>
        /// FROM_MONTH_POSITION
        /// </summary>
        public const int FROM_MONTH_POSITION = 5;
        /// <summary>
        /// FROM_DAY_POSITION
        /// </summary>
        public const int FROM_DAY_POSITION = 6;
        /// <summary>
        /// FROM_HOUR_POSITION
        /// </summary>
        public const int FROM_HOUR_POSITION = 7;
        /// <summary>
        /// FROM_MINUTE_POSITION
        /// </summary>
        public const int FROM_MINUTE_POSITION = 8;
        /// <summary>
        /// FROM_SECOND_POSITION
        /// </summary>
        public const int FROM_SECOND_POSITION = 9;
        /// <summary>
        /// TO_YEAR_POSITION
        /// </summary>
        public const int TO_YEAR_POSITION = 10;
        /// <summary>
        /// TO_MONTH_POSITION
        /// </summary>
        public const int TO_MONTH_POSITION = 11;
        /// <summary>
        /// TO_DAY_POSITION
        /// </summary>
        public const int TO_DAY_POSITION = 12;
        /// <summary>
        /// TO_HOUR_POSITION
        /// </summary>
        public const int TO_HOUR_POSITION = 13;
        /// <summary>
        /// TO_MINUTE_POSITION
        /// </summary>
        public const int TO_MINUTE_POSITION = 14;
        /// <summary>
        /// CUSTOMER_CODE_POSITION
        /// </summary>
        public const int CUSTOMER_CODE_POSITION = 15;
        /// <summary>
        /// BLOCK_NUMBER
        /// </summary>
        public const int BLOCK_NUMBER = 16;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        byte[] dataBuffer = new byte[16];

        /// <summary>
        /// Constructor takes the card class type object as parameter
        /// </summary>
        /// <param name="readerDevice">ReaderDevice of Card class object</param>
        /// <param name="machineUserContext"> Execution context from parafait utils</param>
        /// <param name="customerCode">Mifair customer code</param>
        public CocyLockCardHandler(DeviceClass readerDevice, ExecutionContext machineUserContext, byte customerCode)
            : base(readerDevice, machineUserContext)
        {
            log.LogMethodEntry(readerDevice, machineUserContext, customerCode);
            authKey = new byte[] { 0x4b, 0x21, 0x4d, 0x30, 0x4e, 0x4f };
            this.customerCode = customerCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// WriteToLockerCardLockerNo
        /// </summary>
        private void WriteToLockerCardLockerNo()
        {
            log.LogMethodEntry();
            string message = "";
            bool response = readerDevice.write_data((BLOCK_NUMBER + 1), 1, authKey, dataBuffer, ref message);
            if (response == false)
            {
                log.Debug("Ends-WriteToLockerCardLockerNo() method by throwing an exception: Failed in write to the locker key, " + message);
                throw new Exception("Failed in write to the locker key, " + message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// WriteToLockerCard
        /// </summary>
        private void WriteToLockerCard()
        {
            log.LogMethodEntry();
            string message = "";
            bool response = readerDevice.write_data(BLOCK_NUMBER, 1, authKey, dataBuffer, ref message);
            if (response == false)
            {
                bool defaultKeyResponse = readerDevice.write_data(BLOCK_NUMBER, 1, defaultKey, dataBuffer, ref message);
                if (defaultKeyResponse == false)
                {
                    log.Debug("Ends-WriteToLockerCard() method by throwing an exception:Failed in write to the locker key, " + message );
                    throw new Exception("Failed in write to the locker key, " + message);
                }
                else
                {
                    response = readerDevice.change_authentication_key((BLOCK_NUMBER + 3), defaultKey, authKey, ref message);
                    if (defaultKeyResponse == false)
                    {
                        log.Debug("Ends-WriteToLockerCard() method by an exception: Failed to modify the locker key, " + message);
                        throw new Exception("Failed to modify the locker key, " + message);
                    }
                }
            }
            readerDevice.beep();
            log.LogMethodExit();
        }

        /// <summary>
        /// ConvertToNumericHex
        /// </summary>
        /// <param name="data">data</param>
        /// <returns></returns>
        private byte ConvertToNumericHex(int data)
        {
            log.LogMethodEntry(data);
            if (data > 99)
            {
                log.Debug("Ends-ConvertToNumericHex(data) method throwing an exception :Converting to number hex - Invalid value. Value should be less than 99. Value sent was " + data);
                throw new ArgumentException("Converting to number hex - Invalid value. Value should be less than 99. Value sent was " + data);
            }
            byte finalData = (byte)((data / 10) * 16 + (data % 10));
            log.LogMethodExit(finalData);
            return finalData;
        }

        /// <summary>
        /// SetupWriteBuffer
        /// </summary>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        private void SetupWriteBuffer(DateTime fromTime, DateTime toTime)
        {
            log.LogMethodEntry(fromTime, toTime);
            if (toTime < fromTime)
            {
                log.Debug("Ends-SetupWriteBuffer(fromTime,toTime) method by throwing an exception:Invalid parameters, the to time is less than from time");
                throw new Exception("Invalid parameters, the to time is less than from time");
            }
            if (toTime.Year < 2000)
            {
                log.Debug("Ends-SetupWriteBuffer(fromTime,toTime) method by throwing an exception Invalid parameters, the year of the to time is less than 2000, this is not supported");
                throw new Exception("Invalid parameters, the year of the to time is less than 2000, this is not supported");
            }
            if (fromTime.Year < 2000)
            {
                log.Debug("Ends-SetupWriteBuffer(fromTime,toTime) method by throwing an exception: Invalid parameters, the year of the from time is less than 2000, this is not supported");
                throw new Exception("Invalid parameters, the year of the from time is less than 2000, this is not supported");
            }

            dataBuffer[FROM_YEAR_POSITION] = (byte)(ConvertToNumericHex(fromTime.Year - 2000) % 0xFF);
            dataBuffer[FROM_MONTH_POSITION] = (byte)ConvertToNumericHex(fromTime.Month);
            dataBuffer[FROM_DAY_POSITION] = (byte)ConvertToNumericHex(fromTime.Day);
            dataBuffer[FROM_HOUR_POSITION] = (byte)ConvertToNumericHex(fromTime.Hour);
            dataBuffer[FROM_MINUTE_POSITION] = (byte)ConvertToNumericHex(fromTime.Minute);
            dataBuffer[FROM_SECOND_POSITION] = (byte)ConvertToNumericHex(fromTime.Second);

            dataBuffer[TO_YEAR_POSITION] = (byte)(ConvertToNumericHex(toTime.Year - 2000) % 0xFF);
            dataBuffer[TO_MONTH_POSITION] = (byte)ConvertToNumericHex(toTime.Month);
            dataBuffer[TO_DAY_POSITION] = (byte)ConvertToNumericHex(toTime.Day);
            dataBuffer[TO_HOUR_POSITION] = (byte)ConvertToNumericHex(toTime.Hour);
            dataBuffer[TO_MINUTE_POSITION] = (byte)ConvertToNumericHex(toTime.Minute);

            dataBuffer[CUSTOMER_CODE_POSITION] = customerCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates the system card
        /// </summary>       
        public override void CreateSystemCard(DateTime fromTime, DateTime toTime)
        {
            log.LogMethodEntry(fromTime, toTime);
            SetupWriteBuffer(fromTime, toTime);
            dataBuffer[COMMAND_CODE_POSITION] = SYSTEM_CARD_CODE;
            dataBuffer[1] = dataBuffer[2] = dataBuffer[3] = 0xFF;
            WriteToLockerCard();
            logLockerCardAction("SYSTEMCARD", customerCode.ToString("X2"),"");
            log.LogMethodExit();
        }

        /// <summary>
        /// CreateSystemCard
        /// </summary>
        /// <param name="toTime">toTime</param>
        void CreateSystemCard(DateTime toTime)
        {
            log.LogMethodEntry(toTime);
            DateTime fromTime = DateTime.Now;
            CreateSystemCard(fromTime, toTime);
            logLockerCardAction("SYSTEMCARD", toTime.Date.AddHours(23).AddMinutes(59).ToString("dd-MMM-yyyy H:mm"),"");
            log.LogMethodExit();
        }

        /// <summary>
        /// CreateSystemCard
        /// </summary>
        void CreateSystemCard()
        {
            log.LogMethodEntry();
            DateTime fromTime = DateTime.Now;
            DateTime toTime = fromTime.AddYears(1);
            CreateSystemCard(fromTime, toTime);
            logLockerCardAction("SYSTEMCARD", toTime.Date.AddHours(23).AddMinutes(59).ToString("dd-MMM-yyyy H:mm"),"");
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates the setup card
        /// </summary>
        public override void CreateSetupCard(DateTime validFromTime, DateTime validToTime, byte openHour, byte openMinute)
        {
            log.LogMethodEntry(validFromTime, validToTime, openHour, openMinute);
            SetupWriteBuffer(validFromTime, validToTime);
            dataBuffer[COMMAND_CODE_POSITION] = SETUP_CARD_CODE;
            dataBuffer[1] = openHour;
            dataBuffer[2] = openMinute;
            dataBuffer[3] = 0xFF;
            WriteToLockerCard();
            logLockerCardAction("SETUPCARD", openHour.ToString() + "-" + openMinute.ToString(),"");
            log.LogMethodExit();
        }

        /// <summary>
        /// CreateSetupCard
        /// </summary>
        /// <param name="validToTime">validToTime</param>
        void CreateSetupCard(DateTime validToTime)
        {
            log.LogMethodEntry(validToTime);
            CreateSetupCard(DateTime.Now, validToTime, DEFAULT_OPENING_HOUR, 00);
            logLockerCardAction("SETUPCARD", DEFAULT_OPENING_HOUR,"");
            log.LogMethodExit();
        }

        /// <summary>
        /// CreateSetupCard
        /// </summary>
        void CreateSetupCard()
        {
            log.LogMethodEntry();
            DateTime fromTime = DateTime.Now;
            DateTime toTime = fromTime.AddYears(1);
            CreateSetupCard(fromTime, toTime, DEFAULT_OPENING_HOUR, 00);
            logLockerCardAction("SETUPCARD", DEFAULT_OPENING_HOUR,"");
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates the master card
        /// </summary>
        public override void CreateMasterCard(DateTime validFromTime, DateTime validToTime)
        {
            log.LogMethodEntry(validFromTime, validToTime);
            SetupWriteBuffer(validFromTime, validToTime);
            dataBuffer[COMMAND_CODE_POSITION] = MASTER_CARD_CODE;
            dataBuffer[1] = dataBuffer[2] = dataBuffer[3] = 0xFF;
            WriteToLockerCard();
            logLockerCardAction("MASTERCARD", validToTime.Date.AddHours(23).AddMinutes(59).ToString("dd-MMM-yyyy H:mm"),"");
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates the clock card
        /// </summary>
        public override void CreateClockCard(DateTime validFromTime, DateTime validToTime)
        {
            log.LogMethodEntry(validFromTime, validToTime);
            SetupWriteBuffer(validFromTime, validToTime);
            dataBuffer[COMMAND_CODE_POSITION] = CLOCK_CARD_CODE;
            dataBuffer[1] = dataBuffer[2] = dataBuffer[3] = 0xFF;
            WriteToLockerCard();
            logLockerCardAction("CLOCKCARD", validFromTime.ToString("dd-MMM-yyyy H:mm"),"");
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates the parameter card
        /// </summary>
        public override void CreateParameterCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, bool autoOpen, uint lockerNumber)
        {
            log.LogMethodEntry(validFromTime, validToTime, isFixedMode, autoOpen, lockerNumber);
            SetupWriteBuffer(validFromTime, validToTime);
            dataBuffer[COMMAND_CODE_POSITION] = PARAMETER_CARD_CODE;
            if (isFixedMode)
                dataBuffer[1] = 0x08;
            else
                dataBuffer[1] = 0x0F;
            if (autoOpen)
                dataBuffer[1] |= 0x10;
            if (lockerNumber > 9999)
            {
                log.Debug("Ends-CreateParameterCard(validFromTime,validToTime,isFixedMode,autoOpen,lockerNumber) method by throwing an exception: Invalid locker number. It has to be less than 9999. Currently provided value is " + lockerNumber);
                throw new ArgumentException("Invalid locker number. It has to be less than 9999. Currently provided value is " + lockerNumber);
            }
            if (lockerNumber == 0)
            {
                log.Debug("Ends-CreateParameterCard(validFromTime,validToTime,isFixedMode,autoOpen,lockerNumber) method by throwing an exception: Invalid locker number. It has to be greater than 0. Currently provided value is " + lockerNumber);
                throw new ArgumentException("Invalid locker number. It has to be greater than 0. Currently provided value is " + lockerNumber);
            }

            dataBuffer[3] = (byte)(lockerNumber & 0xFF);
            dataBuffer[2] = (byte)((lockerNumber >> 8) & 0xFF);
            WriteToLockerCard();
            logLockerCardAction("PARAMETERCARD", lockerNumber,"");
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates the disable card
        /// </summary>
        public override void CreateDisableCard(DateTime validToTime)
        {
            log.LogMethodEntry(validToTime);
            if (lockerAllocationDTO != null)
            {
                SetupWriteBuffer(lockerAllocationDTO.IssuedTime, validToTime);
                dataBuffer[COMMAND_CODE_POSITION] = DISABLE_CARD_CODE;
                dataBuffer[1] = dataBuffer[2] = dataBuffer[3] = 0xFF;
                WriteToLockerCard();
                logLockerCardAction("DISABLECARD", lockerAllocationDTO.IssuedTime.ToString("dd-MMM-yyyy H:mm"),"");
                log.Debug("Ends-CreateDisableCard(validToTime) method.");
            }
            else
            {
                log.Debug("Ends-CreateDisableCard(validToTime) method with exception :Locker allocation details does not exists.");
                throw (new Exception("Locker allocation details does not exists."));                
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates the guest card
        /// </summary>
        public override void CreateGuestCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, uint lockerNumber, string zoneGroup, int panelId, string lockerMake, string externalIdentifier = null)
        {
            log.LogMethodEntry(validFromTime, validToTime, isFixedMode, lockerNumber, zoneGroup, panelId, lockerMake, externalIdentifier);
            SetupWriteBuffer(validFromTime, validToTime);
            dataBuffer[COMMAND_CODE_POSITION] = GUEST_CARD_CODE;
            if (lockerNumber > 9999)
            {
                log.Debug("Ends-CreateGuestCard(validFromTime,validToTime,isFixedMode, lockerNumber) method by an exception: Invalid locker number. It has to be less than 9999. Currently provided value is " + lockerNumber);
                throw new ArgumentException("Invalid locker number. It has to be less than 9999. Currently provided value is " + lockerNumber);
            }
            if ((lockerNumber == 0) && (isFixedMode == true))
            {
                log.Debug("Ends-CreateGuestCard(validFromTime,validToTime,isFixedMode, lockerNumber) method by an exception: Invalid locker number. It has to be greater than 0. Currently provided value is " + lockerNumber);
                throw new ArgumentException("Invalid locker number. It has to be greater than 0. Currently provided value is " + lockerNumber);
            }

            if (isFixedMode)
                dataBuffer[1] = 0x08;
            else
            {
                dataBuffer[1] = 0x0F;
                lockerNumber = 0;
            }
            dataBuffer[2] = dataBuffer[3] = 0xFF;
            WriteToLockerCard();
            for (int i = 0; i < 16; i++)
                dataBuffer[i] = 0xFF;
            dataBuffer[1] = (byte)(lockerNumber & 0xFF);
            dataBuffer[0] = (byte)((lockerNumber >> 8) & 0xFF);
            System.Threading.Thread.Sleep(300);
            WriteToLockerCardLockerNo();
            logLockerCardAction("GUESTCARD", lockerNumber,"");
            log.LogMethodExit();
        }

        /// <summary>
        /// getString
        /// </summary>
        /// <param name="bytes">bytes</param>
        /// <param name="startPos">startPos</param>
        /// <param name="length">length</param>
        /// <returns></returns>
        string getString(byte[] bytes, int startPos, int length)
        {
            log.LogMethodEntry(bytes, startPos, length);
            byte[] b = new byte[length];
            Array.Copy(bytes, startPos, b, 0, length);
            string s = BitConverter.ToString(b);
            s = s.Substring(0, 8) + " " + s.Substring(9).Replace('-', ':');
            log.LogMethodExit(s);
            return s;
        }

        /// <summary>
        /// Gets card details
        /// </summary>
        public override string GetReadCardDetails(ref int LockerNumber)
        {
            log.LogMethodEntry(LockerNumber);
            LockerNumber = -1;
            string message = "";
            byte[] db = new byte[32];
            bool response = readerDevice.read_data(BLOCK_NUMBER, 2, authKey, ref db, ref message);
            if (response)
            {
                string cardInfo = "";
                //db[04] = ConvertToNumericHex(9 + (db[04] & (byte)0xf0 * 10 + db[04] & (byte)0x0f));
                //db[10] = ConvertToNumericHex(9 + (db[10] & (byte)0xf0 * 10 + db[10] & (byte)0x0f));
                switch (db[COMMAND_CODE_POSITION])
                {
                    case SYSTEM_CARD_CODE:
                        {
                            cardInfo = "System Card. Project Code: " + db[15].ToString(); break;
                        }
                    case SETUP_CARD_CODE:
                        {
                            cardInfo = "Setting Card. IssueTime: " + getString(db, 4, 6) + ";Auto Open: " + db[01].ToString("#0") + ":" + db[02].ToString("#0"); break;
                        }
                    case MASTER_CARD_CODE:
                        {
                            cardInfo = "Master Card. IssueTime: " + getString(db, 4, 6); break;
                        }
                    case CLOCK_CARD_CODE:
                        {
                            cardInfo = "Clock Card. IssueTime: " + getString(db, 4, 6); break;
                        }
                    case PARAMETER_CARD_CODE:
                        {
                            string lockType, autoOpen;
                            if ((db[1] & (byte)0x0f) == 0x01)
                                lockType = "Free Mode";
                            else
                                lockType = "Fixed Mode";

                            if ((db[1] & (byte)0xf0) == 0x10)
                                autoOpen = "Auto Open On Expiry";
                            else
                                autoOpen = "No Auto Open on Expiry";

                            int lockNumber = db[2];
                            lockNumber = (lockNumber << 8) + db[3];

                            cardInfo = "Parameter Card. " + lockType + " " + autoOpen + "; Locker no: " + lockNumber.ToString(); break;
                        }
                    case DISABLE_CARD_CODE:
                        {
                            cardInfo = "Inhibit Card. IssueTime: " + getString(db, 04, 6); break;
                        }
                    case GUEST_CARD_CODE:
                        {
                            string lockType;
                            if (db[1] == 0x0f)
                                lockType = "Free Mode";
                            else
                                lockType = "Fixed Mode";

                            int lockNumber = db[16];
                            lockNumber = (lockNumber << 8) + db[17];

                            string validity = getString(db, 04, 6) + " to " + getString(db, 10, 5);
                            cardInfo = "Guest Card. " + lockType + "; Locker no: " + lockNumber.ToString() + ". Validity: " + validity;
                            LockerNumber = lockNumber;
                            break;
                        }
                    default: throw new ApplicationException("Invalid locker card (" + db[0].ToString() + ")");
                }
                log.LogMethodExit(cardInfo);
                return cardInfo;
            }
            else
            {
                log.Debug("Ends-GetCardDetails(LockerNumber) method by throwing an exception: Unable to read card: " + message);
                throw new ApplicationException("Unable to read card: " + message);
            }

        }
        /// <summary>
        /// Erase the card
        /// </summary>
        public override void EraseCard()
        {
            log.LogMethodEntry();
            for (int i = 0; i < 16; i++)
                dataBuffer[i] = 0;
            WriteToLockerCard();
            System.Threading.Thread.Sleep(300);
            WriteToLockerCardLockerNo();
            string message = "";
            readerDevice.change_authentication_key((BLOCK_NUMBER + 3), authKey, defaultKey, ref message);
            logLockerCardAction("ERASECARD", "","");
            log.LogMethodExit();
        }
    }
}

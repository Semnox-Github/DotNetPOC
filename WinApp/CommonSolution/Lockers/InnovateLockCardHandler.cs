/********************************************************************************************
 * Project Name - InnovateLockCardHandler
 * Description  - innovate locker handling logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value and 
 *                                            locker overriding changes for blocking the previous locker 
 *2.130.00    31-Aug-2018      Dakshakh raj   Modified create guest card method to add locker make parameter 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Semnox.Parafait.EncryptionUtils;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Business logic for innovate locker
    /// </summary>
    public class InnovateLockCardHandler : ParafaitLockCardHandler
    {
        /// <summary>
        /// Auth key
        /// </summary>
        protected byte[] authKey = new byte[] { 0x11, 0x1C, 0x6C, 0x9C, 0x62, 0x22 };//Moved to child Encoding.Default.GetBytes(Encryption.GetParafaitKeys("CocyInnovate"));//
        /// <summary>
        /// SYSTEM_CARD_CODE
        /// </summary>
        public const byte SYSTEM_CARD_CODE = 0x00;
        /// <summary>
        /// SETUP_CARD_CODE
        /// </summary>
        public const byte SETUP_CARD_CODE = 0x97;
        /// <summary>
        /// MASTER_CARD_CODE
        /// </summary>
        public const byte MASTER_CARD_CODE = 0xB0;
        /// <summary>
        /// CLOCK_CARD_CODE
        /// </summary>
        public const byte CLOCK_CARD_CODE = 0x30;
        /// <summary>
        /// ROOMKEY_CARD_CODE
        /// </summary>
        public const byte ROOMKEY_CARD_CODE = 0x20;
        /// <summary>
        /// GROUP_CARD_CODE
        /// </summary>
        public const byte GROUP_CARD_CODE = 0x50;
        /// <summary>
        /// RECORD_CARD_CODE
        /// </summary>
        public const byte RECORD_CARD_CODE = 0x10;
        /// <summary>
        /// ROOMKEYGUEST_CARD_CODE
        /// </summary>
        public const byte ROOMKEYGUEST_CARD_CODE = 0x60;
        /// <summary>
        /// GROUPKEY_CARD_CODE
        /// </summary>
        public const byte GROUPKEY_CARD_CODE = 0x80;
        /// <summary>
        /// FLOORKEYGUEST_CARD_CODE
        /// </summary>
        public const byte FLOORKEYGUEST_CARD_CODE = 0xD0;
        /// <summary>
        /// BUILDINGKEYGUEST_CARD_CODE
        /// </summary>
        public const byte BUILDINGKEYGUEST_CARD_CODE = 0xC0;
        /// <summary>
        /// CHECKOUTGUEST_CARD_CODE
        /// </summary>
        public const byte CHECKOUTGUEST_CARD_CODE = 0x70;
        /// <summary>
        /// BLOCK_CARD_CODE
        /// </summary>
        public const byte BLOCK_CARD_CODE = 0x40;

        /// <summary>
        /// FIXED_FIRST_BYTE
        /// </summary>
        public const byte FIXED_FIRST_BYTE = 0xC9;
        /// <summary>
        /// VENDOR_CODE
        /// </summary>
        public const byte VENDOR_CODE = 0x01;
        /// <summary>
        /// FACILITY_TOPBYTE
        /// </summary>
        public const byte FACILITY_TOPBYTE = 0x0D;
        /// <summary>
        /// FACILITY_BOTTOMBYTE
        /// </summary>
        public const byte FACILITY_BOTTOMBYTE = 0x0E;
        /// <summary>
        /// LOCK_OPENING_CODE
        /// </summary>
        public const byte LOCK_OPENING_CODE = 0x7F;
        /// <summary>
        /// VALID_START_TIME
        /// </summary>
        public const byte VALID_START_TIME = 0x00;
        /// <summary>
        /// OPEN_PUBLIC_DOOR
        /// </summary>
        public const byte OPEN_PUBLIC_DOOR = 0xFF;
        /// <summary>
        /// OVERRIDE_PREVIOUS_CARD
        /// </summary>
        public const byte OVERRIDE_PREVIOUS_CARD = 0x00;
        /// <summary>
        /// MASTER_CARD_INDICATOR
        /// </summary>
        public const byte MASTER_CARD_INDICATOR = 0xB0;
        /// <summary>
        /// DAI_FLAG
        /// </summary>
        public const byte DAI_FLAG = 0x80;
        /// <summary>
        /// TT_FLAG
        /// </summary>
        public const byte TT_FLAG = 0x00;
        /// <summary>
        /// SECTOR_NUMBER
        /// </summary>
        public const int SECTOR_NUMBER = 14;
        /// <summary>
        /// Card number
        /// </summary>
        private string cardNumber;
        /// <summary>
        /// COMMAND_CODE_POSITION
        /// </summary>
        public const int COMMAND_CODE_POSITION = 9;
        const int BLOCK_NUMBER = SECTOR_NUMBER * 4;
        byte[] dataBuffer = new byte[16];
        byte[] encryptedDataBuffer = new byte[16];
        bool writeEncrypted = true;
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        /// <summary>
        /// Constructor takes card object
        /// </summary>
        /// <param name="readerDevice">ReaderDevice of Card class object</param>
        /// <param name="machineUserContext"> Execution context from parafait utils</param>
        /// <param name="customerCode">Mifair customer code</param>
        /// <param name="cardNumber">Card Number</param>
        public InnovateLockCardHandler(DeviceClass readerDevice, ExecutionContext machineUserContext, byte customerCode, string cardNumber)
            : base(readerDevice, machineUserContext)
        {
            log.LogMethodEntry(readerDevice, machineUserContext, customerCode, cardNumber);
            authKey = Encoding.Default.GetBytes(Encryption.GetParafaitKeys("Kimono"));//new byte[]{ 0x4b, 0x21, 0x4d, 0x30, 0x4e, 0x4f };//
            this.customerCode = customerCode;
            this.cardNumber = cardNumber;
            log.LogMethodExit();
        }
        /// <summary>
        /// Getting unencrypted data
        /// </summary>        
        public byte[] GetUnEncryptedData()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return dataBuffer;            
        }
        /// <summary>
        /// Get encrypted data
        /// </summary>        
        public byte[] GetEncryptedData()
        {
            log.LogMethodEntry();
            log.LogMethodExit(encryptedDataBuffer, "encryptedDataBuffer");
            return encryptedDataBuffer;
        }

        private void WriteToLockerCard(byte[] dataBufferToWrite)
        {
            log.LogMethodEntry(dataBufferToWrite);
            string message = "";
            if(readerDevice==null)
            {
                log.LogMethodExit(null,"Please place the card on the reader.");
                throw new Exception("Please place the card on the reader.");
            }
            bool response = readerDevice.write_data(BLOCK_NUMBER + 1, 1, authKey, dataBufferToWrite, ref message);
            if (response == false)
            {
                bool defaultKeyResponse = readerDevice.write_data(BLOCK_NUMBER + 1, 1, defaultKey, dataBufferToWrite, ref message);
                if (defaultKeyResponse == false)
                {
                    log.Debug("Ends-WriteToLockerCard(dataBufferToWrite) method by throwing an exception: Failed in write to the locker key, " + message);
                    throw new Exception("Failed in write to the locker key, " + message);
                }
                else
                {
                    response = readerDevice.change_authentication_key((BLOCK_NUMBER + 3), defaultKey, authKey, ref message);
                    if (defaultKeyResponse == false)
                    {
                        log.Debug("Ends-WriteToLockerCard(dataBufferToWrite) method by throwing an exception: Failed to modify the locker key, " + message);
                        throw new Exception("Failed to modify the locker key, " + message);
                    }
                }
            }

            readerDevice.beep();
            log.LogMethodExit();
        }

        private byte ConvertToNumericHex(int data)
        {
            log.LogMethodEntry(data);
            if (data > 99)
            {
                log.Debug("Ends-ConvertToNumericHex(data) method by throwing an exception: Converting to number hex - Invalid value. Value should be less than 99. Value sent was " + data);
                throw new ArgumentException("Converting to number hex - Invalid value. Value should be less than 99. Value sent was " + data);
            }
            byte finalData = (byte)((data / 10) * 16 + (data % 10));
            log.LogMethodExit(finalData, "finalData");
            return finalData;
        }

        private void SetupWriteBuffer(DateTime fromTime, DateTime toTime, bool isUnlimited)
        {
            log.LogMethodEntry(fromTime, toTime, isUnlimited);
            if (toTime < fromTime && !isUnlimited)
            {
                log.Debug("Ends-SetupWriteBuffer(fromTime,toTime,isUnlimited) method by throwing an exception:Invalid parameters, the to time is less than from time");
                throw new Exception("Invalid parameters, the to time is less than from time");
            }
            if (toTime.Year < 2009 && !isUnlimited)
            {
                log.Debug("Ends-SetupWriteBuffer(fromTime,toTime,isUnlimited) method by throwing an exception:Invalid parameters, the year of the to time is less than 2009, this is not supported");
                throw new Exception("Invalid parameters, the year of the to time is less than 2009, this is not supported");
            }
            if (fromTime.Year < 2009)
            {
                log.Debug("Ends-SetupWriteBuffer(fromTime,toTime,isUnlimited) method by throwing an exception:Invalid parameters, the year of the from time is less than 2009, this is not supported");
                throw new Exception("Invalid parameters, the year of the from time is less than 2009, this is not supported");
            }

            dataBuffer[0] = FIXED_FIRST_BYTE;
            dataBuffer[1] = VENDOR_CODE;
            dataBuffer[2] = FACILITY_TOPBYTE;
            //dataBuffer[3] = FACILITY_BOTTOMBYTE;
            dataBuffer[3] = customerCode;
            dataBuffer[4] = LOCK_OPENING_CODE;
            dataBuffer[5] = VALID_START_TIME;
            dataBuffer[6] = 0x00;
            dataBuffer[7] = OPEN_PUBLIC_DOOR;
            dataBuffer[8] = OVERRIDE_PREVIOUS_CARD;

            byte fromYear = Convert.ToByte(fromTime.Year - 2009);
            byte fromMonth = Convert.ToByte(fromTime.Month);
            int fromDay = fromTime.Day;
            int fromHour = fromTime.Hour;
            int fromMinute = fromTime.Minute;

            dataBuffer[10] = Convert.ToByte((fromYear << 4) + fromMonth);
            int finalDateTime = (fromDay << 11) + (fromHour << 6) + fromMinute;
            dataBuffer[11] = Convert.ToByte((finalDateTime >> 8) & 0xFF);
            dataBuffer[12] = Convert.ToByte(finalDateTime & 0xFF);

            if (isUnlimited == false)
            {
                int toYear = Convert.ToByte(toTime.Year - 2009);
                int toMonth = Convert.ToByte(toTime.Month);
                int toDay = toTime.Day;
                int toHour = toTime.Hour;
                int toMinute = toTime.Minute;

                dataBuffer[13] = Convert.ToByte((toYear << 4) + toMonth);
                finalDateTime = (toDay << 11) + (toHour << 6) + toMinute;
                dataBuffer[14] = Convert.ToByte((finalDateTime >> 8) & 0xFF);
                dataBuffer[15] = Convert.ToByte(finalDateTime & 0xFF);
            }
            else
            {
                dataBuffer[13] = 0xFF;
                dataBuffer[14] = 0xFF;
                dataBuffer[15] = 0xFF;
            }
        }

        string encryptionKey = Encryption.GetParafaitKeys("InnovateEncryption");//"12WqSaXzuC3CwCHCxCyC1C2C34ReFdVc";//
        byte[] UpdateKey()
        {
            byte[] keyAdds = Encoding.ASCII.GetBytes(cardNumber);
            byte[] encryptionKeyInBytes = Encoding.ASCII.GetBytes(encryptionKey);
            encryptionKeyInBytes[09] = keyAdds[0];
            encryptionKeyInBytes[11] = keyAdds[1];
            encryptionKeyInBytes[13] = keyAdds[2];
            encryptionKeyInBytes[15] = keyAdds[3];
            encryptionKeyInBytes[17] = keyAdds[4];
            encryptionKeyInBytes[19] = keyAdds[5];
            encryptionKeyInBytes[21] = keyAdds[6];
            encryptionKeyInBytes[23] = keyAdds[7];
            log.LogMethodExit(encryptionKeyInBytes, "encryptionKeyInBytes");
            return encryptionKeyInBytes;
        }

        /// <summary>
        /// Creates system card
        /// </summary>
        public override void CreateSystemCard(DateTime fromTime, DateTime toTime)
        {
            log.LogMethodEntry(fromTime, toTime);
            SetupWriteBuffer(fromTime, toTime, false);
            dataBuffer[4] = Convert.ToByte((~dataBuffer[2]) & 0xFF);
            dataBuffer[5] = Convert.ToByte((~dataBuffer[3]) & 0xFF);
            dataBuffer[6] = 0x80;
            dataBuffer[7] = 0x9C;
            dataBuffer[9] = SYSTEM_CARD_CODE;
            byte[] finalEncryptionKey = UpdateKey();
            encryptedDataBuffer = EncryptionAES.Encrypt(dataBuffer, finalEncryptionKey);

            if (writeEncrypted == true)
                WriteToLockerCard(encryptedDataBuffer);
            else
                WriteToLockerCard(dataBuffer);
            logLockerCardAction("SYSTEMCARD", customerCode.ToString("X2"),cardNumber);
            log.LogMethodExit();
        }
        /// <summary>
        /// Reading battery status of the locker from card
        /// </summary>
        /// <returns> returns string type</returns>
        public override string ReadBatteryStatus()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// Create Master Card
        /// </summary>
        public override void CreateMasterCard(DateTime validFromTime, DateTime validToTime)
        {
            log.LogMethodEntry(validFromTime, validToTime);
            SetupWriteBuffer(validFromTime, validToTime.Date.AddHours(23).AddMinutes(59), false);
            dataBuffer[9] = MASTER_CARD_CODE;
            byte[] finalEncryptionKey = UpdateKey();
            encryptedDataBuffer = EncryptionAES.Encrypt(dataBuffer, finalEncryptionKey);
            if (writeEncrypted == true)
                WriteToLockerCard(encryptedDataBuffer);
            else
                WriteToLockerCard(dataBuffer);
            logLockerCardAction("MASTERCARD", validToTime.Date.AddHours(23).AddMinutes(59).ToString("dd-MMM-yyyy H:mm"),cardNumber);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates clock card 
        /// </summary>
        public override void CreateClockCard(DateTime validFromTime, DateTime validToTime)
        {
            log.LogMethodEntry(validFromTime, validToTime);
            SetupWriteBuffer(validFromTime, validToTime, false);
            dataBuffer[4] = 0x00;
            dataBuffer[7] = 0x00;
            dataBuffer[COMMAND_CODE_POSITION] = CLOCK_CARD_CODE;
            dataBuffer[13] = Convert.ToByte(validFromTime.DayOfWeek);
            dataBuffer[14] = 0x00;
            dataBuffer[15] = 0x00;
            byte[] finalEncryptionKey = UpdateKey();
            encryptedDataBuffer = EncryptionAES.Encrypt(dataBuffer, finalEncryptionKey);
            if (writeEncrypted == true)
                WriteToLockerCard(encryptedDataBuffer);
            else
                WriteToLockerCard(dataBuffer);
            logLockerCardAction("CLOCKCARD", validFromTime.ToString("dd-MMM-yyyy H:mm"),cardNumber);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Parameter card 
        /// </summary>
        public override void CreateParameterCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, bool autoOpen, uint lockerNumber)
        {
            log.LogMethodEntry(validFromTime, validToTime, isFixedMode, autoOpen, lockerNumber);
            SetupWriteBuffer(validFromTime, validToTime, false);
            byte roomNo = (byte)((lockerNumber & 0x7F) | DAI_FLAG);
            byte floorNo = (byte)(((lockerNumber & 0x3F80) >> 7) | TT_FLAG);
            byte buildingNo = (byte)((lockerNumber & 0xC000) >> 8);
            dataBuffer[5] = roomNo;
            dataBuffer[6] = floorNo;
            dataBuffer[7] = buildingNo;
            dataBuffer[COMMAND_CODE_POSITION] = ROOMKEY_CARD_CODE;
            byte[] finalEncryptionKey = UpdateKey();
            encryptedDataBuffer = EncryptionAES.Encrypt(dataBuffer, finalEncryptionKey);
            if (writeEncrypted == true)
                WriteToLockerCard(encryptedDataBuffer);
            else
                WriteToLockerCard(dataBuffer);
            logLockerCardAction("PARAMETERCARD", lockerNumber,cardNumber);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates disable card 
        /// </summary>
        public override void CreateDisableCard(DateTime validToTime)
        {
            log.LogMethodEntry(validToTime);
            if (lockerAllocationDTO != null)
            {
                CreateCheckoutCard(lockerAllocationDTO.IssuedTime, validToTime);
            }
            else
            {
                log.Debug("Ends-CreateDisableCard(validToTime) method with exception :Locker allocation details does not exists.");
                throw (new Exception("Locker allocation details does not exists."));
            }
            log.LogMethodExit();
        }

        void CreateBlockCard(DateTime issueTime, DateTime validToTime)
        {
            log.LogMethodEntry(issueTime,validToTime);
            SetupWriteBuffer(issueTime, validToTime, true);
            byte a = dataBuffer[10];
            byte b = dataBuffer[11];
            byte c = dataBuffer[12];
            SetupWriteBuffer(DateTime.Now, DateTime.Now.AddDays(1), false);
            dataBuffer[COMMAND_CODE_POSITION] = BLOCK_CARD_CODE;
            dataBuffer[4] = ROOMKEYGUEST_CARD_CODE;
            dataBuffer[5] = a;
            dataBuffer[6] = b;
            dataBuffer[7] = c;

            byte[] finalEncryptionKey = UpdateKey();
            encryptedDataBuffer = EncryptionAES.Encrypt(dataBuffer, finalEncryptionKey);
            if (writeEncrypted == true)
                WriteToLockerCard(encryptedDataBuffer);
            else
                WriteToLockerCard(dataBuffer);
            logLockerCardAction("BLOCKCARD", issueTime.ToString("dd-MMM-yyyy H:mm"),cardNumber);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates checkout card
        /// </summary>        
        public void CreateCheckoutCard(DateTime issueTime, DateTime validToTime)
        {
            log.LogMethodEntry(issueTime, validToTime);
            SetupWriteBuffer(DateTime.Now, validToTime, true);
            dataBuffer[5] = 0x00;
            dataBuffer[6] = 0x00;
            dataBuffer[7] = 0x00;
            dataBuffer[COMMAND_CODE_POSITION] = CHECKOUTGUEST_CARD_CODE;

            byte[] finalEncryptionKey = UpdateKey();
            encryptedDataBuffer = EncryptionAES.Encrypt(dataBuffer, finalEncryptionKey);
            if (writeEncrypted == true)
                WriteToLockerCard(encryptedDataBuffer);
            else
                WriteToLockerCard(dataBuffer);
            logLockerCardAction("CHECKOUTCARD", issueTime.ToString("dd-MMM-yyyy H:mm"),cardNumber);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Guest Card
        /// </summary>        
        public override void CreateGuestCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, uint lockerNumber, string zoneGroup, int panelId, string lockerMake, string externalIdentifier = null)
        {
            log.LogMethodEntry(validFromTime, validToTime, isFixedMode, lockerNumber, zoneGroup, panelId, lockerMake, externalIdentifier);
            
            LockerDTO lockerDTO = null;
            Locker locker = new Locker(executionContext, lockerDTO);
            int cardOverrideSequence = -1;
            SetupWriteBuffer(validFromTime, validToTime, false);
            byte roomNo = (byte)((lockerNumber & 0x7F) | DAI_FLAG);
            byte floorNo = (byte)(((lockerNumber & 0x3F80) >> 7) | TT_FLAG);
            byte buildingNo = (byte)((lockerNumber & 0xC000) >> 8);
            dataBuffer[4] = 0x00;
            dataBuffer[5] = roomNo;
            dataBuffer[6] = floorNo;
            dataBuffer[7] = buildingNo;
            dataBuffer[COMMAND_CODE_POSITION] = ROOMKEYGUEST_CARD_CODE;

            lockerDTO = locker.GetSelectedLockerDTO(lockerAllocationDTO, panelId, lockerNumber, this.cardNumber);
            if (lockerDTO != null && lockerDTO.LockerId > -1)
            {
                cardOverrideSequence = locker.UpdateCardOverrideSequence(255);
                if (cardOverrideSequence > -1)
                {
                    dataBuffer[8] = (byte)cardOverrideSequence;
                }
            }
            byte[] finalEncryptionKey = UpdateKey();
            encryptedDataBuffer = EncryptionAES.Encrypt(dataBuffer, finalEncryptionKey);
            if (writeEncrypted == true)
                WriteToLockerCard(encryptedDataBuffer);
            else
                WriteToLockerCard(dataBuffer);
            logLockerCardAction("GUESTCARD", lockerNumber, cardNumber);
            log.LogMethodExit();
        }

        string getString(byte[] bytes, int startPos)
        {
            log.LogMethodEntry(bytes, startPos);
            int year = 2009 + ((bytes[startPos] & 0xF0) >> 4);
            int month = (bytes[startPos] & 0x0F);
            int Day = bytes[startPos + 1] >> 3;
            int hour = ((bytes[startPos + 1] & 0x07) << 2) + (bytes[startPos + 2] >> 6);
            int min = bytes[startPos + 2] & 0x3F;
            log.LogMethodExit(year.ToString() + "-" + month.ToString("0#") + "-" + Day.ToString("0#") + " " + hour.ToString() + ":" + min.ToString("0#"));
            return year.ToString() + "-" + month.ToString("0#") + "-" + Day.ToString("0#") + " " + hour.ToString() + ":" + min.ToString("0#");
            
        }

        /// <summary>
        /// Get card Details
        /// </summary>
        public override string GetReadCardDetails(ref int LockerNumber)
        {
            log.LogMethodEntry(LockerNumber);
            LockerNumber = -1;
            string message = "";
            byte[] db = new byte[16];
            bool response = readerDevice.read_data(BLOCK_NUMBER + 1, 1, authKey, ref db, ref message);
            if (response)
            {
                db = EncryptionAES.Decrypt(db, UpdateKey());
                string cardInfo = "";
                switch (db[COMMAND_CODE_POSITION])
                {
                    case SYSTEM_CARD_CODE:
                        {
                            cardInfo = "System Card. Project Code: " + db[3].ToString(); break;
                        }
                    case MASTER_CARD_CODE:
                        {
                            cardInfo = "Master Card. EndTime: " + getString(db, 13); break;
                        }
                    case CLOCK_CARD_CODE:
                        {
                            cardInfo = "Clock Card. DayOfWeek: " + db[13].ToString() + " Time: " + getString(db, 10); break;
                        }
                    case ROOMKEY_CARD_CODE:
                        {
                            string lockType = "Fixed Mode";

                            int lockNumber = db[7];
                            lockNumber = lockNumber << 8;
                            lockNumber = lockNumber + ((db[6] & 0x7E) << 7) + ((db[6] & 0x01) << 7) + (db[5] & 0x7F);

                            cardInfo = "Parameter Card. " + lockType + "; Locker no: " + lockNumber.ToString(); break;
                        }
                    case CHECKOUTGUEST_CARD_CODE:
                        {
                            cardInfo = "CheckOut Card. IssueTime: " + getString(db, 10) + "; EndTime: " + getString(db, 13); break;
                        }
                    case BLOCK_CARD_CODE:
                        {
                            cardInfo = "Inhibit Card. For: " + getString(db, 5) + "; IssueTime: " + getString(db, 10) + "; EndTime: " + getString(db, 13); break;
                        }
                    case ROOMKEYGUEST_CARD_CODE:
                        {
                            string lockType = "Fixed Mode";

                            int lockNumber = db[7];
                            lockNumber = lockNumber << 8;
                            lockNumber = lockNumber + ((db[6] & 0x7E) << 7) + ((db[6] & 0x01) << 7) + (db[5] & 0x7F);

                            string validity = getString(db, 10) + " to " + getString(db, 13);
                            cardInfo = "Guest Card. " + lockType + "; Locker no: " + lockNumber.ToString() +"; Override sequence:"+ Convert.ToInt32(db[8]) + ". Validity: " + validity;
                            LockerNumber = lockNumber;
                            break;
                        }
                    default: throw new ApplicationException("Invalid locker card (" + db[COMMAND_CODE_POSITION].ToString() + ")");
                }
                log.LogMethodExit(cardInfo, "cardInfo");
                return cardInfo;
            }
            else
            {
                log.Error("Unable to read card: " + message);
                throw new ApplicationException("Unable to read card: " + message);
            }
        }

        /// <summary>
        /// Erase card
        /// </summary>
        public override void EraseCard()
        {
            log.LogMethodEntry();
            for (int i = 0; i < 16; i++)
                dataBuffer[i] = 0;

            WriteToLockerCard(dataBuffer);

            System.Threading.Thread.Sleep(10);
            string message = "";
            readerDevice.change_authentication_key((BLOCK_NUMBER + 3), authKey, defaultKey, ref message);

            logLockerCardAction("ERASECARD", "",cardNumber);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates record Card
        /// </summary>        
        public void CreateRecordCard(DeviceClass readerDevice, DateTime validFromTime, DateTime validToTime, byte[] cardNumber, bool isUnlimited, bool shouldWriteToCard, bool writeEncrypted)
        {
            log.LogMethodEntry(readerDevice, validFromTime, validToTime, cardNumber, isUnlimited, shouldWriteToCard, writeEncrypted);
            SetupWriteBuffer(validFromTime, validToTime, false);
            dataBuffer[4] = 0x00;
            dataBuffer[5] = 0x00;
            dataBuffer[6] = 0x00;
            dataBuffer[7] = 0x02;
            dataBuffer[COMMAND_CODE_POSITION] = RECORD_CARD_CODE;
            byte[] finalEncryptionKey = UpdateKey();
            encryptedDataBuffer = EncryptionAES.Encrypt(dataBuffer, finalEncryptionKey);
            if (writeEncrypted == true)
                WriteToLockerCard(encryptedDataBuffer);
            else
                WriteToLockerCard(dataBuffer);
            logLockerCardAction("RECORDCARD", "",this.cardNumber);
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - Hecere Locker Lock
 * Description  - The bussiness logic for Hecere locker lock
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.3    26-May-2023    Abhishek       Created : As a Part of Hecere Locker Integration 
 ********************************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Hecere locker class which inherites the ParafaitLockerLock class
    /// </summary>
    public class HecereLockCardHandler : ParafaitLockCardHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private string cardNumber;

        /// <summary>
        /// Hecere constructor which accepts the card object as parameter
        /// </summary>
        /// <param name="readerDevice">ReaderDevice of Card class object</param>
        /// <param name="machineUserContext"> Execution context from parafait utils</param>
        /// <param name="cardNumber">cardNumber</param>
        public HecereLockCardHandler(DeviceClass readerDevice, ExecutionContext machineUserContext, string cardNumber)
            : base(readerDevice, machineUserContext)
        {
            log.LogMethodEntry(readerDevice, machineUserContext, cardNumber);
            this.cardNumber = cardNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates the guest card
        /// </summary>
        /// <param name="validFromTime">Valid from </param>
        /// <param name="validToTime">Valid To</param>
        /// <param name="isFixedMode"> Mode of issue</param>
        /// <param name="lockerNumber">Locker number</param>
        /// <param name="zoneGroup">Zone Group 0 or A~Z</param>
        /// <param name="externalIdentifier">ExternalIdentifier</param>
        public override void CreateGuestCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, uint lockerNumber, string zoneGroup, int panelId, string lockerMake, string externalIdentifier = null)
        {
            log.LogMethodEntry(validToTime, isFixedMode, lockerNumber, zoneGroup, externalIdentifier);
            try
            {
                int exitCode;
                string endDate = validToTime.ToString("yyMMddHH");
                StringBuilder lockerNum = new StringBuilder(18);
                lockerNum.Append(externalIdentifier);
                StringBuilder date = new StringBuilder(10);
                date.Append(endDate);
                StringBuilder cardNo = new StringBuilder(8);
                cardNo.Append(cardNumber);
                if (lockerAllocationDTO != null)
                {
                    exitCode = HecereAPIFunction.GSSDK_WriteGuestCard(lockerNum, date, 1, cardNo);
                    if (exitCode != 00)
                    {
                        log.Error(exitCode);
                        string returnValue = GetHecereACSErrorMessage(exitCode);
                        log.LogMethodExit(null, "Throwing Exception -  " + returnValue);
                        if (IsParafaitEnvironment)
                        {
                            throw new Exception(returnValue);
                        }
                        else
                        {
                            throw new Exception(exitCode + ", " + returnValue);
                        }
                    }
                }
            }
            finally
            {
                if (readerDevice != null)
                {
                    log.Debug("Starting Listener");
                    readerDevice.startListener();
                }
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get card Details
        /// </summary>
        public override string GetReadCardDetails(ref int LockerNumber)
        {
            log.LogMethodEntry(LockerNumber);
            try
            {
                LockerDTO lockerDTO;
                int exitCode;
                string lockerList = "";
                string toDate = "";
                StringBuilder lockerNum = new StringBuilder(18);
                StringBuilder validDate = new StringBuilder(8);
                validDate.Append(toDate);
                StringBuilder cardNo = new StringBuilder(8);
                cardNo.Append(cardNumber);
                if (lockerAllocationDTO != null)
                {
                    exitCode = HecereAPIFunction.GSSDK_ReadGuestCard(lockerNum, validDate, 1, cardNo);
                    if (exitCode == 00)
                    {
                        Locker locker = new Locker(lockerAllocationDTO.LockerId);
                        LockerNumber = Convert.ToInt32(lockerNum);
                        if (locker.getLockerDTO == null)
                        {
                            lockerDTO = locker.GetLockerDetailsOnidenfire(LockerNumber);
                            locker = new Locker(executionContext, lockerDTO);
                        }
                        locker.Save();
                    }
                    else
                    {
                        log.Error(exitCode);
                        string returnValue = GetHecereACSErrorMessage(exitCode);
                        log.LogMethodExit(null, "Throwing Exception -  " + returnValue);
                        if (IsParafaitEnvironment)
                        {
                            throw new Exception(returnValue);
                        }
                        else
                        {
                            throw new Exception(exitCode + ", " + returnValue);
                        }
                    }
                }
            }
            finally
            {
                if (readerDevice != null)
                {
                    log.Debug("Starting Listener");
                    readerDevice.startListener();
                }
            }
            log.LogMethodExit();
            return "";
        }

        /// <summary>
        /// Erase the cards
        /// </summary>
        public override void EraseCard()
        {
            log.LogMethodEntry();
            try
            {
                int exitCode;
                StringBuilder cardNum = new StringBuilder(8);
                cardNum.Append(cardNumber);
                exitCode = HecereAPIFunction.GSSDK_ClearGuestCard(1, cardNum);
                if (exitCode != 00)
                {
                    log.Error(exitCode);
                    string returnValue = GetHecereACSErrorMessage(exitCode);
                    log.LogMethodExit(null, "Throwing Exception -  " + returnValue);
                    if (IsParafaitEnvironment)
                    {
                        throw new Exception(returnValue);
                    }
                    else
                    {
                        throw new Exception(exitCode + ", " + returnValue);
                    }
                }
            }
            finally
            {
                if (readerDevice != null)
                {
                    log.Debug("Starting Listener");
                    readerDevice.startListener();
                }
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Returns Hecere error messages
        /// </summary>
        /// <param name="exitcode">The error code one which we get from Hecere api  </param>
        /// <returns>Returns the error message</returns>
        public string GetHecereACSErrorMessage(int exitcode)
        {
            log.LogMethodEntry(exitcode);
            string returnMessage = "";
            switch (exitcode)
            {
                case 20: returnMessage = "No card machine found."; break;
                case 21: returnMessage = "The card reader model is incorrect."; break;
                case 22: returnMessage = "No cards were found."; break;
                case 23: returnMessage = "The comparison card password is incorrect."; break;
                case 24: returnMessage = "Card reading failed."; break;
                case 25: returnMessage = "Failed to write card."; break;
                case 26: returnMessage = "Stuck machine error."; break;
                case 27: returnMessage = "The cabinet lock interface authorization error."; break;
                case 28: returnMessage = "Cabinet lock interface authorization failed."; break;
                case 29: returnMessage = "Card data error."; break;
                case 30: returnMessage = "This card is not a guest hand card."; break;
                case 31: returnMessage = "This card is the new user card."; break;
                case 32: returnMessage = "This card is a recycled card."; break;
                case 33: returnMessage = "This card is a cancelled card."; break;
                case 34: returnMessage = "There is an error in the encoding format of the room number."; break;
                case 35: returnMessage = "The time passed in is in the wrong format."; break;
                case 36: returnMessage = "memory space error."; break;
                case 37: returnMessage = "Not a hand card."; break;
                case 38: returnMessage = "The wrong unit of time."; break;
                case 39: returnMessage = "The length of time is wrong."; break;
                case 40: returnMessage = "This card is an authorization card."; break;
                case 41: returnMessage = "Failed to log in to the card machine."; break;
            }
            log.LogMethodExit(returnMessage);
            return returnMessage;
        }
    }

    class HecereAPIFunction
    {
        private const string DLLNAME = @"GSSDK_A1.dll";

        /// <summary>
        /// This function write card, this function can only issue card
        /// </summary>
        /// <param name="p_csRoomNo"> the cabinet lock code written into the hand card, 18 characters, this cabinet lock code must be obtained 
        /// using GSA1_CN_SDKTool.exe tools, the encoding must be correct;</param>
        /// <param name="p_csEndTime"> End time of hand card unlocking, 8 characters, pay attention to allocating memory space, format is YYMMDDHH，
        /// such as 15010818 means 18:00 on January 8, 2015 </param>
        /// <param name="p_nBeep">Whether the card reader sounds, integer, 1 sound, 0 - no sound</param>
        /// <param name="p_csSNo">The returned card has a fixed serial number of 8 characters</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GSSDK_WriteGuestCard(StringBuilder p_csRoomNo, StringBuilder p_csEndTime, int p_nBeep, StringBuilder p_csCardSN);

        /// <summary>
        /// This function read hand card, this function can only read card.
        /// </summary>
        /// <param name="p_csRoomNo">Returns the cabinet lock code of the card, 18 characters</param>
        /// <param name="p_csEndTime">The end time of the card unlocking, 8 characters, the format is YYMMDDHH，
        /// such as 15010818 means 18:00 on January 8, 2015;</param>
        /// <param name="p_nBeep">Whether the card reader sounds, integer, 1 sound, 0 - no sound</param>
        /// <param name="p_csSNo ">The returned card has a fixed serial number of 8 characters</param>
        /// <returns># 00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GSSDK_ReadGuestCard(StringBuilder p_csRoomNo, StringBuilder p_csEndTime, int p_nBeep, StringBuilder p_csCardSN);

        /// <summary>
        /// This function Clean the hand card, that is, the hand card recycling.
        /// </summary>
        /// <param name="p_nBeep">Whether the card reader sounds, integer, 1 sound, 0 - no sound</param>
        /// <param name="p_csCardSN">The returned card has a fixed serial number of 8 characters </param>
        /// <returns># 00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GSSDK_ClearGuestCard(int p_nBeep, StringBuilder p_csCardSN);
    }
}

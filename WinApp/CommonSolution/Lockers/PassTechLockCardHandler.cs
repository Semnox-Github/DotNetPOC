/********************************************************************************************
 * Project Name - Passtech Locker Lock
 * Description  - The bussiness logic for Passtech locker lock
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera      Created 
 *2.90        22-Jul-2020   Mushahid Faizan Modified spelling name of GetLockerDetailsOnidenfire to GetLockerDetailsOnIdentifire.
  *2.130.00    29-Jun-2021   Dakshakh raj   Modified as part of Metra locker integration 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Passtech locker class which inherites the ParafaitLockerLock class
    /// </summary>
    public class PassTechLockCardHandler : ParafaitLockCardHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        /// <summary>
        /// Passtech constructor which accepts the card object as parameter
        /// </summary>
        /// <param name="readerDevice">ReaderDevice of Card class object</param>
        /// <param name="machineUserContext"> Execution context from parafait utils</param>
        public PassTechLockCardHandler(DeviceClass readerDevice, ExecutionContext machineUserContext)
            : base(readerDevice, machineUserContext)
        {
            log.LogMethodEntry(readerDevice, machineUserContext);
            int code;
            code = PassTechAPIFunction.PT_SetBASE_SECTOR(0x06);
            if (code != 0)
            {
                log.Debug("Ends-PassTechLockerLock(inCard) constructor by throwing exception 'Unable to set the base sector'");
                if (IsParafaitEnvironment)
                {
                    throw new Exception("Unable to set the base sector");
                }
                else
                {
                    throw new Exception("200,Unable to set the base sector");
                }
            }
            code = PassTechAPIFunction.PT_SetAUTH_TYPE(0x01);
            if (code != 0)
            {
                log.Debug("Ends-PassTechLockerLock(inCard) constructor by throwing exception 'Auth type setting process failed'");
                if (IsParafaitEnvironment)
                {
                    throw new Exception("Auth type setting process failed");
                }
                else
                {
                    log.Debug("201, Auth type setting process failed");
                    throw new Exception("201,Auth type setting process failed");
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This function is used to create master card with different types without any validity. Currently this fuction is defined only for PassTech locker purpose
        /// </summary>
        /// <param name="masterCardType">0x31:Master card 1 which erase the card number from locker and opens the lock.
        /// 0x32: Master card 2 will open the locker without erasing the card number from the locker
        /// </param>
        public override void CreateMasterCard(byte masterCardType)
        {
            log.LogMethodEntry(masterCardType);
            byte exitCode;
            int code;
            bool isKeyCardRead;
            if (ReadKey(out isKeyCardRead))
            {
                if (isKeyCardRead)
                {
                    if (IsParafaitEnvironment)
                    {
                        MessageBox.Show("Please place the Master card on the reader and Press OK", "PassTech Locker");
                    }
                    else
                    {
                        throw (new Exception("202, Please place the Master card on the reader."));
                    }
                }
                exitCode = PassTechAPIFunction.PT_MasterCardIssue(masterCardType);
                code = PassTechAPIFunction.PT_Buzzer(1);
                if (exitCode != 0x00)
                {
                    code = PassTechAPIFunction.PT_Buzzer(2);
                    log.Debug("Ends-CreateMasterCard(masterCardType) method."+GetPassTechACSErrorMessage(exitCode) + " Master card " + ((masterCardType == 0x31) ? "1" : "2") + " creation failed.");
                    throw new Exception(GetPassTechACSErrorMessage(exitCode) + " Master card " + ((masterCardType == 0x31) ? "1" : "2") + " creation failed.");

                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Erase the cards
        /// </summary>
        public override void EraseCard()
        {
            log.LogMethodEntry();
            CreateGuestCard(Convert.ToDateTime("1901-01-01"), Convert.ToDateTime("1901-01-02"), true, 9999, "0",-1,null);
            log.LogMethodExit();
        }

        /// <summary>
        /// sending the online locker command
        /// </summary>
        /// <returns></returns>
        public override bool SendOnlineCommand(string onlineServiceUrl, RequestType requestType, List<string> lockerList, List<string> cardList, string zoneName, string lockerMake)
        {
            log.LogMethodEntry(onlineServiceUrl, requestType, lockerList, cardList, zoneName);
            OnlineLockerIntegChannelFactory channelFactory = new OnlineLockerIntegChannelFactory(onlineServiceUrl, false);
            IOnlineLockerIntegrationInterface httpProxy = channelFactory.CreateChannel();
            log.LogMethodExit(httpProxy.SendCommand(requestType, lockerList, cardList));
            return httpProxy.SendCommand(requestType, lockerList, cardList); ;
        }
        /// <summary>
        /// Creates the guest card
        /// </summary>
        /// <param name="validFromTime">Valid from </param>
        /// <param name="validToTime">Valid To</param>
        /// <param name="isFixedMode"> Mode of issue</param>
        /// <param name="lockerNumber">Locker number</param>
        /// <param name="zoneGroup">Zone Group 0 or A~Z</param>
        public override void CreateGuestCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, uint lockerNumber, string zoneGroup, int panelId, string lockerMake, string externalIdentifier = null)
        {
            log.LogMethodEntry(validFromTime, validToTime, isFixedMode, lockerNumber, zoneGroup, externalIdentifier);
            try
            {
                if (readerDevice != null)
                {
                    log.Debug("Disconnecting reader");
                    readerDevice.disconnect();
                    log.Debug("Stopping Listener");
                    readerDevice.stopListener();
                }
                byte exitCode;
                bool isKeyCardRead;
                byte[] fromDate = Encoding.ASCII.GetBytes(validFromTime.ToString("yyyyMMddHHmmss"));
                byte[] toDate = Encoding.ASCII.GetBytes(validToTime.ToString("yyyyMMddHHmmss"));
                byte mode;
                uint lockerNo = lockerNumber;
                byte[] zoneCode = Encoding.ASCII.GetBytes(zoneGroup);
                byte[] lockerList = Encoding.ASCII.GetBytes(((isFixedMode) ? zoneGroup + lockerNo.ToString().PadLeft(4, '0') : "").PadRight(35, '0'));
                mode = (isFixedMode) ? (byte)0x09 : (byte)0x08;
                if (ReadKey(out isKeyCardRead))
                {
                    if (isKeyCardRead)
                    {
                        if (IsParafaitEnvironment)
                        {
                            MessageBox.Show("Please place the user card on the reader and Press OK", "PassTech Locker");
                        }
                        else
                        {
                            throw (new Exception("203, Please place the user card on the reader"));
                        }
                    }
                    exitCode = PassTechAPIFunction.PT_PersonalizeUserCard(mode, lockerList, 1, fromDate, toDate, 0x30, 0x00, zoneCode);
                    if (exitCode != 0x00)
                    {
                        log.Error(exitCode);
                        string returnValue = GetPassTechACSErrorMessage(exitCode);
                        log.LogMethodExit(null, "Throwing Exception -  " + returnValue);
                        if (IsParafaitEnvironment)
                        {
                            throw new Exception(returnValue);
                        }
                        else
                        {
                            throw new Exception(exitCode+", "+returnValue);
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
        /// ReadKey
        /// </summary>
        /// <param name="IsReadFromCard">IsReadFromCard</param>
        /// <returns></returns>
        private bool ReadKey(out bool IsReadFromCard)
        {
            log.LogMethodEntry();
            byte exitCode;
            int code;
            IsReadFromCard = false;
            byte[] userKey = new byte[128];
            if (!File.Exists(Application.StartupPath + "\\_KeyInfo.bin"))
            {
                IsReadFromCard = true;
                if (IsParafaitEnvironment)
                {
                    MessageBox.Show("Please place the key card on the reader and Press OK", "PassTech Locker");
                }
                else
                {
                    log.Error("204,Please place the key card on the reader.");
                    throw new Exception("204,Please place the key card on the reader.");
                }
                exitCode = PassTechAPIFunction.PT_newKeyCardDownload(0x49, userKey);//
                if (IsReadFromCard)
                    code = PassTechAPIFunction.PT_Buzzer(1);
                if (exitCode != 0x00)
                {
                    log.Error(exitCode);
                    log.Debug("Ends-CreateMasterCard(IsReadFromCard) method." + GetPassTechACSErrorMessage(exitCode));
                    if (IsParafaitEnvironment)
                    {
                        throw new Exception(GetPassTechACSErrorMessage(exitCode));
                    }
                    else
                    {
                        throw new Exception(exitCode+", "+GetPassTechACSErrorMessage(exitCode));
                    }
                }
            }
            exitCode = PassTechAPIFunction.PT_newKeyCardDownload(0x46, userKey);
            if (IsReadFromCard)
                code = PassTechAPIFunction.PT_Buzzer(1);
            if (exitCode != 0x00)
            {
                log.Error(exitCode);
                log.Debug("Ends-ReadKey(IsReadFromCard) method." + GetPassTechACSErrorMessage(exitCode));
                if (IsParafaitEnvironment)
                {
                    throw new Exception(GetPassTechACSErrorMessage(exitCode));
                }
                else
                {
                    throw new Exception(exitCode +", "+ GetPassTechACSErrorMessage(exitCode));
                }
            }
            log.LogMethodExit(true);
            return true;
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
                bool isKeyCardRead;
                byte exitCode;
                byte[] szCSN = new byte[7];
                short IssueCnt = -1, nTCount = -1;
                byte bCardType = new byte();
                byte[] szTIDList = new byte[35];
                string lockerList = "";
                byte[] szFrom = new byte[14];
                string fromdate = "";
                byte[] szTo = new byte[14];
                string toDate = "";
                byte[] szLockerID = new byte[5];
                byte bBattStat = new byte();
                byte bTLFlag = new byte();
                byte bTLTerm = new byte();
                byte szGType = new byte();
                if (lockerAllocationDTO != null)
                {
                    if (ReadKey(out isKeyCardRead))
                    {
                        exitCode = PassTechAPIFunction.PT_ReadUserCard(szCSN, ref IssueCnt, ref bCardType, szTIDList, ref nTCount, szFrom, szTo, szLockerID, out bBattStat, out bTLFlag, out bTLTerm, out szGType);
                        if (exitCode == 0x00)
                        {
                            Locker locker = new Locker(lockerAllocationDTO.LockerId);
                            lockerList = Encoding.ASCII.GetString(szTIDList);
                            LockerNumber = Convert.ToInt32(lockerList.Substring(1, 4));
                            if (locker.getLockerDTO==null)
                            {
                                lockerDTO = locker.GetLockerDetailsOnidenfire(LockerNumber);
                                locker = new Locker(executionContext,lockerDTO);
                            }
                            if (locker.getLockerDTO.BatteryStatus != ((bBattStat == 0)? 0: 1))
                            {
                                locker.getLockerDTO.BatteryStatus = ((bBattStat == 0) ? 0 : 1);
                                locker.getLockerDTO.BatteryStatusChangeDate = DateTime.Now;
                                LockerLog.POSLockerLogMessage(lockerAllocationDTO.LockerId, "Debug", "Battery Status", ((bBattStat == 0) ? "N" : "L"));
                            }
                            if (locker.getLockerDTO.LockerStatus != ((bTLFlag==0x30)?"C":"O"))
                            {
                                locker.getLockerDTO.LockerStatus = ((bTLFlag == 0x30) ? "C" : "O");
                                locker.getLockerDTO.StatusChangeDate = DateTime.Now;
                                LockerLog.POSLockerLogMessage(lockerAllocationDTO.LockerId, "Debug", "Locker Status recorded on tapped card:"+ lockerAllocationDTO.CardNumber, ((bTLFlag == 0x30) ? "C" : "O"));
                            }                            
                            locker.Save();
                            
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                log.Error("Locker card read error:" + ex.Message, ex);
            }
            log.LogMethodExit();
            return "";
        }
        ///// <summary>
        ///// Reading battery status of the locker from card
        ///// </summary>
        ///// <returns> returns string type</returns>
        //public override string ReadBatteryStatus()
        //{
        //    log.Debug("Starts-ReadCardData() method");
        //    byte exitCode;
        //    int code;
        //    bool isKeyCardRead;
        //    try
        //    {
        //        if (ReadKey(out isKeyCardRead))
        //        {
        //            if (isKeyCardRead)
        //                MessageBox.Show("Please place the user card on the reader and Press OK", "PassTech Locker");
        //            PassTechLockCardHandlerDTO passTechLockerLockDTO = new PassTechLockCardHandlerDTO();
        //            exitCode = PassTechAPIFunction.PT_ReadUserCard(passTechLockerLockDTO.UsercardSerialNo, ref passTechLockerLockDTO.IssueCount, ref passTechLockerLockDTO.bCardType, passTechLockerLockDTO.LockerIdList, ref passTechLockerLockDTO.nTCount, passTechLockerLockDTO.ValidFrom, passTechLockerLockDTO.ValidTo, passTechLockerLockDTO.LastAccessLockerID, ref passTechLockerLockDTO.BatteryStatus, ref passTechLockerLockDTO.DoorOpenCloseAfterTime, ref passTechLockerLockDTO.LockerTimeLimit);
        //            code = PassTechAPIFunction.PT_Buzzer(1);
        //            if (exitCode != 0x00)
        //            {
        //                log.Debug("Ends-ReadCardData() method by returning null");
        //                return null;
        //            }
        //            if (passTechLockerLockDTO.BatteryStatus == 0x30)
        //                lockerAllocationDTO.BatteryStatus = "Normal";
        //            else if (passTechLockerLockDTO.BatteryStatus == 0x43)
        //                lockerAllocationDTO.BatteryStatus = "Low";
        //            LockerAllocation lockerAllocation = new LockerAllocation(lockerAllocationDTO);
        //            lockerAllocation.Save(null);
        //            log.Debug("Ends-ReadCardData() method by returning passTechLockerLockDTO");
        //            return lockerAllocationDTO.BatteryStatus;
        //        }
        //        log.Debug("Ends-ReadCardData() method by returning null");
        //        return null;
        //    }
        //    catch
        //    {
        //        log.Debug("Ends-ReadCardData() method by returning null");
        //        return null;
        //    }
        //}

            /// <summary>
            /// Gets card details
            /// </summary>
        

        /// <summary>
        /// Returns Passtech error messages
        /// </summary>
        /// <param name="exitcode">The error code one which we get from passtech api  </param>
        /// <returns>Returns the error message</returns>
        public string GetPassTechACSErrorMessage(byte exitcode)
        {
            log.LogMethodEntry(exitcode);
            string returnMessage = "";
            switch (exitcode)
            {
                case 0x29: returnMessage = "An error occurred in setting the smart card file object pointer."; break;
                case 0x02: returnMessage = "The action was canceled by an SCardCancel request."; break;
                case 0x0E: returnMessage = "The system could not dispose of the media in the requested manner."; break;
                case 0x1C: returnMessage = "The smart card does not meet minimal requirements for support."; break;
                case 0x23: returnMessage = "The specified directory does not exist in the smart card."; break;
                case 0x1B: returnMessage = "The reader driver did not produce a unique reader name."; break;
                case 0x24: returnMessage = "The specified file does not exist in the smart card."; break;
                case 0x21: returnMessage = "The requested order of object creation is not supported."; break;
                case 0x20: returnMessage = "No primary provider can be found for the smart card."; break;
                case 0x08: returnMessage = "The data buffer for returned data is too small for the returned data."; break;
                case 0x15: returnMessage = "An ATR string obtained from the registry is not a valid ATR string."; break;
                case 0x2A: returnMessage = "The supplied PIN is incorrect."; break;
                case 0x03: returnMessage = "The supplied handle was not valid."; break;
                case 0x04: returnMessage = "One or more of the supplied parameters could not be properly interpreted."; break;
                case 0x05: returnMessage = "Registry startup information is missing or not valid."; break;
                case 0x11: returnMessage = "One or more of the supplied parameter values could not be properly interpreted."; break;
                case 0x27: returnMessage = "Access is denied to the file."; break;
                case 0x25: returnMessage = "The supplied path does not represent a smart card directory."; break;
                case 0x26: returnMessage = "The supplied path does not represent a smart card file."; break;
                case 0x06: returnMessage = "Not enough memory available to complete this command."; break;
                case 0x2E: returnMessage = "No smart card reader is available."; break;
                case 0x1D: returnMessage = "The smart card resource manager is not running."; break;
                case 0x0C: returnMessage = "The operation requires a smart card, but no smart card is currently in the device."; break;
                case 0x2C: returnMessage = "The requested certificate does not exist."; break;
                case 0x10: returnMessage = "The reader or card is not ready to accept commands."; break;
                case 0x16: returnMessage = "An attempt was made to end a nonexistent transaction."; break;
                case 0x19: returnMessage = "The PCI receive buffer was too small."; break;
                case 0x0F: returnMessage = "The requested protocols are incompatible with the protocol currently in use with the card."; break;
                case 0x17: returnMessage = "The specified reader is not currently available for use."; break;
                case 0x1A: returnMessage = "The reader driver does not meet minimal requirements for support."; break;
                case 0x1E: returnMessage = "The smart card resource manager has shut down."; break;
                case 0x0B: returnMessage = "The smart card cannot be accessed because of other outstanding connections."; break;
                case 0x12: returnMessage = "The action was canceled by the system, presumably to log off or shut down."; break;
                case 0x0A: returnMessage = "The user-specified time-out value has expired."; break;
                case 0x1F: returnMessage = "An unexpected card error has occurred."; break;
                case 0x0D: returnMessage = "The specified smart card name is not recognized."; break;
                case 0x09: returnMessage = "The specified reader name is not recognized."; break;
                case 0x2B: returnMessage = "An unrecognized error code was returned from a layered component."; break;
                case 0x22: returnMessage = "This smart card does not support the requested feature."; break;
                case 0x28: returnMessage = "An attempt was made to write more data than would fit in the target object."; break;
                case 0x13: returnMessage = "An internal communications error has been detected."; break;
                case 0x01: returnMessage = "An internal consistency check failed."; break;
                case 0x14: returnMessage = "An internal error has been detected, but the source is unknown."; break;
                case 0x07: returnMessage = "An internal consistency timer has expired."; break;
                case 0x18: returnMessage = "The operation has been aborted to allow the server application to exit."; break;
                case 0x6e: returnMessage = "The action was canceled by the user."; break;
                case 0x6C: returnMessage = "The card cannot be accessed because the maximum number of PIN entry attempts has been reached."; break;
                case 0x6D: returnMessage = "The end of the smart card file has been reached."; break;
                case 0x69: returnMessage = "The smart card has been removed, so further communication is not possible."; break;
                case 0x68: returnMessage = "The smart card has been reset, so any shared state information is not valid."; break;
                case 0x6A: returnMessage = "Access was denied because of a security violation."; break;
                case 0x67: returnMessage = "Power has been removed from the smart card, so that further communication is not possible."; break;
                case 0x66: returnMessage = "The smart card is not responding to a reset."; break;
                case 0x65: returnMessage = "The reader cannot communicate with the card, due to ATR string configuration conflicts."; break;
                case 0x6B: returnMessage = "The card cannot be accessed because the wrong PIN was presented."; break;
                case 0xA0: returnMessage = "DE620’s communication preference error"; break;
                case 0xA1: returnMessage = "Mifare Standard 1K / No Mifare Standard 4K"; break;
                case 0xB0: returnMessage = "Key Value not set"; break;
                case 0xB1: returnMessage = "Key Load communicate preference Error"; break;
                case 0xB2: returnMessage = "Key Load Error"; break;
                case 0xBA: returnMessage = "Key number is invalid"; break;
                case 0xBB: returnMessage = "Mutual Authentication error"; break;
                case 0xC0: returnMessage = "Card Read error"; break;
                case 0xC1: returnMessage = "Card Read error"; break;
                case 0xC2: returnMessage = "Card Write error"; break;
                case 0xD4: returnMessage = "Key Card not read"; break;
                case 0xE0: returnMessage = "Card Type error"; break;
                case 0xE1: returnMessage = "Parameter is invalid"; break;
            }
            log.LogMethodExit(returnMessage);
            return returnMessage;
        }
    }
    
    class PassTechAPIFunction
    {
        private const string DLLNAME = @"PTSDKDll.dll";
        /// <summary>
        /// This Deactivates the device buzar
        /// </summary>
        /// <param name="nType">1 is sucess and !=1 is failure</param>
        /// <returns>0 sucess,other number is the error code </returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_Buzzer")]
        public static extern int PT_Buzzer(int nType);

        /// <summary>
        /// This function is base sector set.
        /// </summary>
        /// <param name="bSectorNo">sector number (0x06)</param>
        /// <returns># 0 : success  # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_SetBASE_SECTOR")]
        public static extern int PT_SetBASE_SECTOR(byte bSectorNo);

        /// <summary>
        /// This function is authentication type set.
        /// </summary>
        /// <param name="bType">Authetication Type (0x01)</param>
        /// <returns># 0 : success  # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_SetAUTH_TYPE")]
        public static extern int PT_SetAUTH_TYPE(byte bType);			//_FIXED_KEY=0x01, _VARY_KEY=0x02

        /// <summary>
        /// This function returns the type and chip serial no of master card
        /// </summary>
        /// <param name="bMasterCardType">Master card type(1Byte)  
        /// 1) 0x31 : Master card(1) 
        /// 2) 0x32 : Master card(2)</param>
        /// <param name="lpszCardNoType">Card Chip Serial No(8Bytes)</param>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_GetMasterCardType")]
        public static extern byte PT_GetMasterCardType(byte[] bMasterCardType, byte[] lpszCardNoType);

        /// <summary>
        /// This function is User Key Download
        /// </summary>
        /// <param name="bAccesType">Key Access Type(1Byte)  
        /// 1) 0x00, 0x43 : Key Card Access type (more secure and recommend) 
        /// 2) 0x49 : Card Access and Key File creation type    
        /// 3) 0x46 : Key File Access type</param>
        /// <param name="lpUserKey">lpUserKey(128Bytes)
        /// lpUserKey is 128Bytes
        ///    0~8: sector#0,     8~16: sector#1,    16~24 : sector#2,   24~32 : sector#3
        ///  32~40: sector#4,    40~48: sector#5,    48~56 : sector#6,   56~64 : sector#7
        ///  64~72: sector#8,    72~80: sector#9,    80~88 : sector#10,  88~96 : sector#11
        /// 96~104: sector#12, 104~112: sector#13, 112~120 : sector#14,120~128 : sector#15</param>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_newKeyCardDownload")]
        public static extern byte PT_newKeyCardDownload(byte bAccesType, byte[] lpUserKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bType">Card Type In case of bType: 
        /// 0x08 : Free-selection Card
        /// 0x09 : Fixed-selection Card</param>
        /// <param name="szLockerList">“A0001”+”01” (MainID+SubID) (7Bytes*5) (must 35Bytes,  Default Values : All 0x30)</param>
        /// <param name="nLockerMaxCount">nLockerMaxCount(Maximum  Lock count one card can occupy ) In case of LockerMaxCount: 1~5</param>
        /// <param name="szFromDate">lpFromDate (Ex: “20110610181416”) In case of DateTime : “YYYYMMDDhhnnss” (must 14bytes)</param>
        /// <param name="szToDate">lpToDate (Ex: “20110610181416”)In case of DateTime : “YYYYMMDDhhnnss” (must 14bytes)</param>
        /// <param name="bTLFlag">LockerTime Limit duration(must 1Byte)
        /// <param name="szGType">Group filed should be use full in case of free mode to restrict the card to the specific zone </param>
        /// 0x31 : Close
        /// 0x32 : Open</param>
        /// <param name="bTLTerm">Parameter for door open/close after Limit Time durtation (1Byte)
        /// 0x00 : No Time Limit
        /// 1~99 : Time Limit duration(hours)</param>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_PersonalizeUserCard")]
        public static extern byte PT_PersonalizeUserCard(byte bType, byte[] szLockerList, int nLockerMaxCount, byte[] szFromDate, byte[] szToDate, byte bTLFlag, byte bTLTerm, byte[] szGType);

        /// <summary>
        /// This function returns the parameters of user cards.
        /// </summary>
        /// <param name="szCSN">User Card Serial No(must 7bytes)</param>
        /// <param name="IssueCnt">No. of issuance of User Card </param>
        /// <param name="bCardType">User Card Type   
        /// 0x08 : Free Selection, 
        /// 0x09 : Assigned Mode</param>
        /// <param name="szTIDList">Locker IDList(must 35bytes)
        /// In case of Locker ID : “0000000”~”Z999999” (7bytes)
        /// #1stByte : Zone(Group) Code -> ‘0’, ‘A’~’Z’
        /// #2nd~5th4Bytes :Lock number
        /// # 6th~7th2Bytes : Sub ID</param>
        /// <param name="nTCount"> Locker Multi User Count In case of LockerMaxCount: 0~5</param>
        /// <param name="szFrom">Valid Term(Start Date/Time)
        /// In case of Date/ime : “YYYYMMDDhhnnss” (must 14bytes)
        /// ->Default value of Lock Time Limit in cards “20000101000000” (SDK only)</param>
        /// <param name="szTo">Valid Term(End Date/Time)
        /// In case of Date/ime : “YYYYMMDDhhnnss” (must 14bytes)
        /// ->Default value of Lock Time Limit in cards “20000101000000” (SDK only)</param>
        /// <param name="szLockerID">Last access Lock ID(5Bytes / with Sub ID)</param>
        /// <param name="bBattStat">Last access Lock Battery Status(1Byte)
        /// 0x30 : Normal  
        /// 0x43 : Low</param>
        /// <param name="bTLFlag">Parameter for door Open/Close after Limt Time duration (1Byte)
        /// 0x30 : Close
        /// 0x31 : Open</param>
        /// <param name="bTLTerm">LockerTime Limit duration(must 1Byte)
        /// 0x00: No Time Limit
        /// 1 ~ 99 : Time Limit duration (hours)</param>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_ReadUserCard")]
        public static extern byte PT_ReadUserCard(byte[] szCSN, ref short IssueCnt, ref byte bCardType, byte[] szTIDList, ref short nTCount, byte[] szFrom, byte[] szTo, byte[] szLockerID, out byte bBattStat, out byte bTLFlag, out byte bTLTerm, out byte szGType);
        /// <summary>
        /// This function is Maintenance Card Setting
        /// </summary>
        /// <param name="szDateTime">szDateTime  (Ex: “20110610181416”) In case of szDateTime : “YYYYMMDDhhnnss” (must 14bytes)</param>
        /// <param name="nAddSec">nAddSec In case of AddSec : 0 ~ 99</param>
        /// <param name="bMasterFlag">MasterCard Change Flag(1Byte) In case of bMasterFlag : 0x30: not change, 0x31: change</param>
        /// <param name="szMasterCard">Master Card Chip Serial No  In case of szMasterCard (must 40bytes)
        /// Card Chip Serail No HexString(8Bytes) * 5   Ex) “AAAAAAAABBBBBBBBCCCCCCCCDDDDDDDDFFFFFFFF”</param>
        /// <param name="bPWDFlag">Password Change Flag(1Byte)
        /// In case of bPWDFlag : 0x30: not change, 0x31: change(Open Door and Delete PWD), 
        /// 0x32: change(Open and Close door only)</param>
        /// <param name="szPassword">Password(6Bytes, Numeric)</param>
        /// <param name="bPT200Flag">PT200 Model setting mode 
        /// In case of bPT200Flag (must 1byte) : 0x00 : not change, 0x30:change(PWD and CARD), 0x31:change(PWD), 0x32:change(CARD)</param>
        /// <param name="bChannel">Channel Change Flag(1Byte)
        /// In case of bChannel : 0x30: not change, 0x31: change</param>
        /// <param name="nChannel">0x00:Wireless Commuication mode off, 11~25 : Channel Value</param>
        /// <param name="bTLFlag">Time Limit Setting Flag(1Byte)</param>
        /// <param name="bTLTerm">Time Limit seting (unit:Hour), 1~99 : Time Limit Value</param>
        /// <param name="bBLCheckFlag">B/L Change Flag(1Byte)
        /// In case of bBLCheckFlag : 0x30: not change, 0x31: change</param>
        /// <param name="bBLCheck">B/L Change Flag(1Byte)
        /// In case of bBLCheckValue : 0x00: B/L Mode off, 0x30: change(B/L ON) , 0x31: change(B/L OFF)</param>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_SetMaintenanceCard")]
        public static extern byte PT_SetMaintenanceCard(byte[] szDateTime, int nAddSec, byte bMasterFlag, byte[] szMasterCard, byte bPWDFlag, byte[] szPassword, byte bPT200Flag, byte bChannel, short nChannel, byte bTLFlag, byte bTLTerm, byte bBLCheckFlag, byte bBLCheck);
        /// <summary>
        /// This function is SetupCard 3Shift Setting
        /// </summary>
        /// <param name="szTimeList">BYTE szTimeList[in] : 3Shift Time Value (must 84bytes) Openning time (Type: hhmm)
        /// SUN: “0000””0010””0020” (12bytes)
        /// MON: “0000””0010””0020” (12bytes)
        /// TUE: “0000””0010””0020” (12bytes)
        /// WEN: “0000””0010””0020” (12bytes)
        /// THU: “0000””0010””0020” (12bytes)
        /// FRI: “0000””0010””0020” (12bytes)
        /// SAT: “0000””0010””0020” (12bytes)</param>
        /// <param name="szShiftOPF">3Shift Auto Locker Open Flag
        /// In case of bOpenFlag(7Bytes): 
        /// All 0x30 : use 3Shift Auto Open
        /// All 0x31 : not use Auto Open</param>
        /// <param name="bAlarm">Alarm Setting
        /// In case of bAlarm (1Byte): 
        /// 0x30 : Alarm off
        /// 0x31 : Alarm on</param>
        /// <param name="bLED">LED Setting
        /// In case of bLED (1Byte): 
        /// 0x30 : LED off
        /// 0x31 : LED on</param>
        /// <param name="b3ShiftFlag">3Shift Use Flag
        /// In case of bUseFlag: 
        /// 0x30 : no use 3Shift
        /// 0x31 : use 3Shift</param>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_Set3ShiftInfo")]
        public static extern byte PT_Set3ShiftInfo(byte[] szTimeList, byte[] szShiftOPF, byte bAlarm, byte bLED, byte b3ShiftFlag);

        /// <summary>
        /// This function TRXCard ReadData( Including Zone Code, 7UID Card ID, Channel No, Battery Status) 
        /// </summary>
        /// <param name="szLockerID">Lock ID (7bytes)
        /// ->1stByte : Zone(Group) Code
        /// ->Zone(Group) Code : ‘0’, ‘A’~’Z’</param>
        /// <param name="bType"> LockType (1byte)
        /// 0x08 : Free-selection 
        /// 0x09 : Assigned-selection</param>
        /// <param name="szDateTime">szDateTime(14bytes, Ex: “20110610181416”)
        /// Time of lock at Data collection</param>
        /// <param name="bChannel">wireless communication Channel(1Byte, 0x00, 0x11~0x19)</param>
        /// <param name="bBattStat">Battery Status (1Byte, 0x00:Normal, 0x01:Low)</param>
        /// <param name="nTRXCnt">Audit Card TRX Count(Max : 18)</param>
        /// <param name="szTRXData">Audit Card TRX Data(37Bytes * nTRXCnt)</param>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_GetAuditCard")]
        public static extern byte PT_GetAuditCard(byte[] szLockerID, byte[] bType, byte[] szDateTime, byte[] bChannel, byte[] bBattStat, ref int nTRXCnt, byte[] szTRXData);

        /// <summary>
        /// This function clear audit card.
        /// </summary>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_ClearAuditCard")]
        public static extern byte PT_ClearAuditCard();

        /// <summary>
        /// This function returns the parameters from Lock Info Card.
        /// </summary>
        /// <param name="nInfoCnt">Card Information Count(Max : 40)</param>
        /// <param name="lpLockInfoData">Lock Info Card Information Data(50Bytes * nInfoCnt)</param>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_GetLockInfoCard")]
        public static extern byte PT_GetLockInfoCard(ref int nInfoCnt, byte[] lpLockInfoData);


        /// <summary>
        /// This function clear lock info card.
        /// </summary>
        /// <returns># 0x00 : success    # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_ClearLockInfoCard")]
        public static extern byte PT_ClearLockInfoCard();

        /// <summary>
        /// This function reads Card Serial No.
        /// </summary>
        /// <param name="lpCSN">Card Serial No(7Bytes, BCD Type)</param>
        /// <returns># success : 0x00 # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_GetUID")]
        public static extern byte PT_GetUID(byte[] lpCSN);

        /// <summary>
        /// This function returns the Card Type.
        /// </summary>        
        /// <returns>
        /// # 0x00 : Initialized Card
        /// # 0x01 : Key Card
        /// # 0x05 : Owner Card 
        /// # 0x06 : Master Card
        /// # 0x08 : Locker Free-Selection Card 
        /// # 0x09 : Locker Fixed-Selection Card
        /// # 0x13 : Audit Card (1K)
        /// # 0x14 : Maintenance Card Setting 
        /// # 0x17 : Setup Card
        /// # 0x1B : User Add Card
        /// # 0x1C : Lock Info card (4K)
        /// # 0xFE : User Card(Initialized card)
        /// # other: Terminal error code
        /// </returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_GetCardType")]
        public static extern byte PT_GetCardType();

        /// <summary>
        /// This function issues Master Card(I) and Master Card(II).
        /// </summary>
        /// <param name="bCardFlag">Master Card Type
        /// In case of bCardFlag: 
        /// 0x31 : Master Card(I)
        /// 0x32 : Master Card(II)
        /// </param>
        /// <returns># success : 0x00 # other : Terminal error code</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "PT_MasterCardIssue")]
        public static extern byte PT_MasterCardIssue(byte bCardFlag);

    }
}

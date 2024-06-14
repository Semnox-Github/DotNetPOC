/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra Lock Card Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date               Modified By       Remarks          
 *********************************************************************************************
 *2.130.00    26-May-2021        Dakshakh raj      Created 
 ********************************************************************************************/

using System;
using System.Linq;
using Semnox.Core.Utilities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Metra locker class which inherites the ParafaitLockerLock class
    /// </summary>
    public class MetraLockCardHandler : ParafaitLockCardHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext;
        string cardNumber;
        string lockerIdentifier;
        string zoneCode;
        string lockerMake;
        string lockerMode;
        private LookupValuesList serverDateTime;

        /// <summary>
        /// constructor for Metra Lock Card Handler
        /// </summary>
        /// <param name="readerDevice"></param>
        /// <param name="machineUserContext"></param>
        /// <param name="cardNumber"></param>
        /// <param name="lockerIdentifier"></param>
        /// <param name="zoneCode"></param>
        /// <param name="lockerMake"></param>
        /// <param name="lockerMode"></param>
        public MetraLockCardHandler(DeviceClass readerDevice, ExecutionContext machineUserContext, string cardNumber, string lockerIdentifier, string zoneCode, string lockerMake, string lockerMode)
            : base(readerDevice, machineUserContext)
        {
            log.LogMethodEntry(readerDevice, machineUserContext, cardNumber, lockerIdentifier, zoneCode, lockerMake, lockerMode);
            this.machineUserContext = machineUserContext;
            this.cardNumber = cardNumber;
            this.lockerIdentifier = lockerIdentifier;
            this.zoneCode = zoneCode;
            this.lockerMake = lockerMake;
            this.lockerMode = lockerMode;
            this.serverDateTime = new LookupValuesList(machineUserContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Metra Lock Card Handler
        /// </summary>
        /// <param name="readerDevice"></param>
        /// <param name="machineUserContext"></param>
        /// <param name="cardNumber"></param>
        public MetraLockCardHandler(DeviceClass readerDevice, ExecutionContext machineUserContext)
            : base(readerDevice, machineUserContext)
        {
            log.LogMethodEntry(readerDevice, machineUserContext);
            this.machineUserContext = machineUserContext;
            this.serverDateTime = new LookupValuesList(machineUserContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates the guest card
        /// </summary>
        /// <param name="validFromTime"></param>
        /// <param name="validToTime"></param>
        /// <param name="isFixedMode"></param>
        /// <param name="lockerNumber"></param>
        /// <param name="zoneGroup"></param>
        /// <param name="panelId"></param>
        /// <param name="lockerMake"></param>
        public override void CreateGuestCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, uint lockerNumber, string zoneGroup, int panelId, string lockerMake, string externalIdentifier = null)
        {
            log.LogMethodEntry(validFromTime, validToTime, isFixedMode, lockerNumber, zoneGroup, panelId, lockerMake, externalIdentifier);
            try
            {
                MetraLockerIssueResponse metraLockerIssueResponse;
                using (NoSynchronizationContextScope.Enter())
                {
                    MetraLockerIssueCommand metraLockerIssueCommand = MetraCommandFactory.GetMetraLockerIssueCommand(lockerMake, this.cardNumber, zoneGroup, isFixedMode, lockerNumber.ToString(), validFromTime, validToTime, machineUserContext);
                    Task<MetraLockerIssueResponse> metraLockerIssueResponseTask = metraLockerIssueCommand.Execute();
                    metraLockerIssueResponseTask.Wait();
                    metraLockerIssueResponse = metraLockerIssueResponseTask.Result;
                }
                if (metraLockerIssueResponse.IsSuccess == false)
                {
                    string message = MessageContainerList.GetMessage(machineUserContext, 4047);
                    log.Error(message);
                    throw new Exception(message);
                }
                if (isFixedMode)
                {
                    log.Info(string.Format("Locker {0} allocated to card {1} from {2} till {3} in zone {4}", lockerNumber, cardNumber, validFromTime, validToTime, zoneGroup));
                }
                else
                {
                    log.Info(string.Format("Locker allocated to card {0} from {1} till {2} in zone {3}", cardNumber, validFromTime, validToTime, zoneGroup));
                }

            }
            catch (Exception ex)
            {
                if (ex != null && ex.InnerException != null)
                {
                    string message = MessageContainerList.GetMessage(machineUserContext, 4047);
                    log.Error(message + ex.InnerException, ex);
                    throw new Exception(ex.InnerException.Message);
                }
                else
                {
                    string message = MessageContainerList.GetMessage(machineUserContext, 4047);
                    log.Error(message + ex, ex);
                    throw new Exception(ex.Message);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Get Card Details
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public override MetraLockerCardInfoResponse GetCardDetails(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            MetraLockerCardInfoResponse metraLockerCardInfoResponse;
            using (NoSynchronizationContextScope.Enter())
            {
                MetraLockerCardInfoCommand metraLockerCardInfoCommand = MetraCommandFactory.GetMetraLockerCardInfoCommand(cardNumber.ToString(), machineUserContext);
                Task<MetraLockerCardInfoResponse> metraLockerCardInfoResponseTask = metraLockerCardInfoCommand.Execute();
                metraLockerCardInfoResponseTask.Wait();
                metraLockerCardInfoResponse = metraLockerCardInfoResponseTask.Result;
            }
            log.Info(string.Format("Get card details for card number {0}", cardNumber));
            log.LogMethodExit();
            return metraLockerCardInfoResponse;
        }

        /// <summary>
        /// Erase Card
        /// </summary>
        public override void EraseCard()
        {
            log.LogMethodEntry();
            try
            {
                if (IsLockerLocked() && this.lockerMake != null && this.lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()))
                {
                    List<string> lockerList = new List<string>();
                    lockerList.Add(lockerIdentifier);
                    SendOnlineCommand(null, RequestType.OPEN_LOCK, lockerList, null, zoneCode, lockerMake);
                }
                if (!string.IsNullOrEmpty(this.lockerMode) && this.lockerMode == "FIXED")
                {
                    MetraLockerFreeResponse metraLockerFreeResponse;
                    using (NoSynchronizationContextScope.Enter())
                    {
                        MetraLockerFreeCommand metraLockerFreeCommand = MetraCommandFactory.GetMetraLockerFreeCommand(this.lockerMake, this.lockerMode, zoneCode, lockerIdentifier, machineUserContext);
                        Task<MetraLockerFreeResponse> metraLockerFreeResponseTask = metraLockerFreeCommand.Execute();
                        metraLockerFreeResponseTask.Wait();
                        metraLockerFreeResponse = metraLockerFreeResponseTask.Result;
                    }
                    if (metraLockerFreeResponse.IsSuccess == false)
                    {
                        string message = MessageContainerList.GetMessage(machineUserContext, 4048);
                        throw new Exception(message);
                    }
                    log.Info(string.Format("Locker free command called for {0} and locker make used is {1}", lockerIdentifier, lockerMake));
                }
                else
                {
                    MetraLockerEraseResponse metraLockerEraseResponse;
                    using (NoSynchronizationContextScope.Enter())
                    {
                        MetraLockerEraseCommand metraLockerEraseCommand = MetraCommandFactory.GetMetraLockerEraseCommand(this.cardNumber, machineUserContext, this.lockerMake, this.lockerMode, zoneCode, lockerIdentifier);
                        Task<MetraLockerEraseResponse> metraLockerEraseResponseTask = metraLockerEraseCommand.Execute();
                        metraLockerEraseResponseTask.Wait();
                        metraLockerEraseResponse = metraLockerEraseResponseTask.Result;
                    }
                    if (metraLockerEraseResponse.IsSuccess == false)
                    {
                        string message = MessageContainerList.GetMessage(machineUserContext, 4049);
                        throw new Exception(message);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex != null && ex.InnerException != null)
                {
                    string message = MessageContainerList.GetMessage(machineUserContext, 4047);
                    log.Error(message + ex.InnerException, ex);
                    throw new Exception(ex.InnerException.Message);
                }
                else
                {
                    string message = MessageContainerList.GetMessage(machineUserContext, 4047);
                    log.Error(message + ex, ex);
                    throw new Exception(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Is Locker Locked
        /// </summary>
        /// <returns></returns>
        private bool IsLockerLocked()
        {
            log.LogMethodEntry();
            MetraLockerCardInfoResponse metraLockerCardInfoResponse;
            metraLockerCardInfoResponse = GetCardDetails(lockerAllocationDTO.CardNumber);
            if (metraLockerCardInfoResponse.IsSuccess && metraLockerCardInfoResponse.LockerNumber > -1 && metraLockerCardInfoResponse.LockerNumber.ToString() == lockerIdentifier)
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

        /// <summary>
        /// Send Online Command
        /// </summary>
        /// <param name="onlineServiceUrl"></param>
        /// <param name="requestType"></param>
        /// <param name="lockerList"></param>
        /// <param name="cardList"></param>
        /// <param name="zoneCode"></param>
        /// <param name="lockerMake"></param>
        /// <returns></returns>
        public override bool SendOnlineCommand(string onlineServiceUrl, RequestType requestType, List<string> lockerList, List<string> cardList, string zoneCode, string lockerMake)
        {
            log.LogMethodEntry(onlineServiceUrl, requestType, lockerList, cardList, zoneCode, lockerMake);
            if (requestType == RequestType.OPEN_LOCK || requestType == RequestType.OPEN_ALL_LOCK && lockerList != null && lockerList.Any())
            {
                foreach (string lockerNumber in lockerList)
                {
                    MetraLockerUnlockResponse metraLockerUnlockResponse;
                    using (NoSynchronizationContextScope.Enter())
                    {
                        MetraLockerUnlockCommand metraLockerUnlockCommand = MetraCommandFactory.GetMetraLockerUnlockCommand(lockerMake, zoneCode, lockerNumber, machineUserContext);
                        Task<MetraLockerUnlockResponse> metraLockerUnlockResponseTask = metraLockerUnlockCommand.Execute();
                        metraLockerUnlockResponseTask.Wait();
                        metraLockerUnlockResponse = metraLockerUnlockResponseTask.Result;
                    }
                    if (metraLockerUnlockResponse.IsSuccess == false)
                    {
                        string message = MessageContainerList.GetMessage(machineUserContext, 4050);
                        log.LogMethodExit(message);
                        if (lockerList.Count == 1)
                        {
                            throw new Exception(message);
                        }
                    }
                    else
                    {
                        log.LogMethodExit();
                        log.Info(string.Format("Locker number {0} is unlocked", lockerNumber));
                        continue;
                    }
                }
                return true;
            }
            else if (requestType == RequestType.BLOCK_CARD || requestType == RequestType.UNBLOCK_CARD && cardList != null & cardList.Any())
            {
                string action = string.Empty;
                if (requestType == RequestType.BLOCK_CARD)
                {
                    action = "add";
                }
                if (requestType == RequestType.UNBLOCK_CARD)
                {
                    action = "remove";
                }
                foreach (string cardNumber in cardList)
                {
                    MetraLockerBlackListResponse metraLockerBlackListResponse;
                    using (NoSynchronizationContextScope.Enter())
                    {
                        MetraLockerBlackListCommand metraLockerBlackListCommand = MetraCommandFactory.GetMetraLockerBlackListCommand(cardNumber, action, machineUserContext, this.lockerMake);
                        Task<MetraLockerBlackListResponse> metraLockerBlackListResponseTask = metraLockerBlackListCommand.Execute();
                        metraLockerBlackListResponseTask.Wait();
                        metraLockerBlackListResponse = metraLockerBlackListResponseTask.Result;
                    }
                    if (metraLockerBlackListResponse.IsSuccess == false && requestType == RequestType.BLOCK_CARD)
                    {
                        string message = MessageContainerList.GetMessage(machineUserContext, 4051);
                        log.LogMethodExit(message);
                        throw new Exception(message);
                    }
                    if (metraLockerBlackListResponse.IsSuccess == false && requestType == RequestType.UNBLOCK_CARD)
                    {
                        string message = MessageContainerList.GetMessage(machineUserContext, 4052);
                        log.LogMethodExit(message);
                        throw new Exception(message);
                    }
                    else
                    {
                        log.LogMethodExit();
                        log.Info(string.Format("Card block successful for card number {0}", cardNumber));
                        return true;
                    }
                }
            }
            else
            {
                string message = MessageContainerList.GetMessage(machineUserContext, 4053);
                log.LogMethodExit(message);
                throw new Exception(message);
            }
            return false;
        }

        /// <summary>
        /// Get Locker Number
        /// </summary>
        /// <param name="zoneCode"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public override string GetLockerNumber(string zoneCode, string identifier)
        {
            log.LogMethodEntry(zoneCode, identifier);
            string lockerNumber = identifier;
            log.LogMethodExit(lockerNumber);
            return lockerNumber;
        }
    }
}

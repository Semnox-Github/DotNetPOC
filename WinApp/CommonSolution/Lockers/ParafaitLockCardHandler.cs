/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - The bussiness logic for parafait locker lock
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera     Created 
 *2.70.2      18-Sep-2019   Dakshakh raj   modified : added logs
 *2.120.0     01-Apr-2021   Dakshakh raj   modified : Enabling variable hours for Passtech Lockers and enabling function to extend the time
 *2.130.00    29-Jun-2021   Dakshakh raj   Modified as part of Metra locker integration 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
//using Semnox.Core.Lookups;
using Semnox.Core.Utilities;
using Semnox.Parafait.logger;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Business logic for the base class of parafait locker lock
    /// </summary>
    public class ParafaitLockCardHandler
    {
        /// <summary>
        /// Defualt locker opening hour.
        /// </summary>
        public const int DEFAULT_OPENING_HOUR = 18;

        private bool isParafaitEnvironment = true;
        public bool IsParafaitEnvironment { get { return isParafaitEnvironment; } set { isParafaitEnvironment = value; } }

        /// <summary>
        /// Default key
        /// </summary>
        protected byte[] defaultKey = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };//Encoding.Default.GetBytes(Encryption.GetParafaitKeys("CardDefault")); //
        /// <summary>
        /// Card class object 
        /// </summary>
        protected DeviceClass readerDevice;
        /// <summary>
        /// Customer code
        /// </summary>
        protected byte customerCode;//Change name
        /// <summary>
        /// Config Card Approving User is hold the string type data
        /// </summary>
        private string configCardApprovingUser;
        /// <summary>
        /// Config Card Approving User is hold the string type data
        /// </summary>
        public string ConfigCardApprovingUser { get { return configCardApprovingUser; } set { configCardApprovingUser = value; } }
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Locker allocation
        /// </summary>
        protected LockerAllocationDTO lockerAllocationDTO;

        ExecutionContext machineUserContext;
        /// <summary>
        /// constructor which accepts card type object as parameter
        /// </summary>
        /// <param name="readerDevice"> Card class reader device object</param>
        /// <param name="machineUserContext"> Execution context from parafait utils</param>
        public ParafaitLockCardHandler(DeviceClass readerDevice, ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(readerDevice, machineUserContext);
            this.readerDevice = readerDevice;
            this.machineUserContext = machineUserContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Sets the allocation 
        /// </summary>
        /// <param name="lockerAllocationDTO"></param>
        public void SetAllocation(LockerAllocationDTO lockerAllocationDTO)
        {
            log.LogMethodEntry(lockerAllocationDTO);
            this.lockerAllocationDTO = lockerAllocationDTO;
            log.LogMethodExit();
        }
        /// <summary>
        ///Creates the system card 
        /// </summary>
        public virtual void CreateSystemCard(DateTime fromTime, DateTime toTime)
        { }

        /// <summary>
        ///Creates the setup card 
        /// </summary>
        public virtual void CreateSetupCard(DateTime validFromTime, DateTime validToTime, byte openHour, byte openMinute)
        { }
        /// <summary>
        ///Creates the master card 
        /// </summary>
        public virtual void CreateMasterCard(DateTime validFromTime, DateTime validToTime)
        { }

        /// <summary>
        ///Creates the master card with type 
        /// </summary>
        public virtual void CreateMasterCard(byte masterCardType)//Modified on 30-Mar-2017 For Passtech integration
        { }

        /// <summary>
        ///Creates the clock card
        /// </summary>
        public virtual void CreateClockCard(DateTime validFromTime, DateTime validToTime)
        { }

        /// <summary>
        ///Creates the parameter card
        /// </summary>
        public virtual void CreateParameterCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, bool autoOpen, uint lockerNumber)
        { }

        /// <summary>
        ///Creates the disable card
        /// </summary>
        public virtual void CreateDisableCard(DateTime validToTime)
        { }

        /// <summary>
        ///Creates the guest card
        /// </summary>
        public virtual void CreateGuestCard(DateTime validFromTime, DateTime validToTime, bool isFixedMode, uint lockerNumber, string zoneGroup, int panelId, string lockerMake, string externalIdentifier = null)
        {
            log.LogMethodEntry(validFromTime, validToTime, isFixedMode, lockerNumber, zoneGroup, lockerMake, externalIdentifier);
            log.LogMethodExit(null);
        }

        /// <summary>
        ///Erase card
        /// </summary>
        public virtual void EraseCard()
        { }

        /// <summary>
        ///Reads data from the card
        /// </summary>
        public virtual string GetReadCardDetails(ref int LockerNumber)
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual MetraLockerCardInfoResponse GetCardDetails(string CardNumber)
        {
            return null;
        }

        /// <summary>
        /// sending the online locker command
        /// </summary>
        /// <returns></returns>
        public virtual bool SendOnlineCommand(string onlineServiceUrl, RequestType requestType, List<string> lockerList, List<string> cardList, string zoneCode, string lockerMake)
        {
            return false;
        }

        /// <summary>
        ///Gets the locker allocated card details
        /// </summary>
        public virtual LockerAllocationDTO GetLockerAllocationCardDetails(int cardId)
        {
            log.LogMethodEntry(cardId);

            LockerAllocation lockerAllocation = new LockerAllocation();
            LockerDTO lockerDTO = new LockerDTO();
            lockerAllocationDTO = lockerAllocation.GetLockerAllocationOnCardId(cardId);
            if (lockerAllocationDTO == null)
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                log.LogMethodExit(lockerAllocationDTO);
                return lockerAllocationDTO;
            }
        }
        /// <summary>
        ///Gets the locker details
        /// </summary>
        public virtual LockerAllocationDTO GetLockerAllocationDetailsOnIdenitfier(int identifier, int panelId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(identifier, panelId, sqlTransaction);
            LockerDTO lockerDTO = new LockerDTO();
            List<LockerDTO> lockerDTOs = new List<LockerDTO>();
            // Locker locker = new Locker(machineUserContext);
            //lockerDTO = locker.GetLockerDetailsOnidenfire(identifier);
            LockersList lockersList = new LockersList();
            List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>> searchLockerParameters = new List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>>();
            searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.IS_ACTIVE, "1"));
            searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.IDENTIFIRE, identifier.ToString()));
            if (panelId != -1)
            {
                searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.PANEL_ID, panelId.ToString()));
            }
            lockerDTOs = lockersList.GetAllLocker(searchLockerParameters, true, sqlTransaction);
            log.LogVariableState("LockerDTOList", lockerDTOs);
            if (lockerDTOs != null && lockerDTOs.Count > 0 && lockerDTOs.Exists(x => (bool)(x.LockerAllocated != null)))
            {
                lockerDTO = lockerDTOs.FindAll(x => (bool)(x.LockerAllocated != null))[0];
            }
            if (lockerDTO == null)
            {
                log.LogMethodExit(null);
                return null;
            }
            lockerAllocationDTO = lockerDTO.LockerAllocated;
            if (lockerAllocationDTO == null)
            {
                log.LogMethodExit(null);
                return null;
            }
            else
            {
                log.LogMethodExit(lockerAllocationDTO);
                return lockerAllocationDTO;
            }
        }

        /// <summary>
        /// Reads the battery status from the card
        /// </summary>
        /// <returns> Returns string type</returns>
        public virtual string ReadBatteryStatus()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }


        /// <summary>
        /// Creates the locker card
        /// </summary>
        public void CreateLockerCard(LockerAllocationDTO lockerAllocateDTO, string selectionMode, SqlTransaction SQLTrx, string zoneCode, string lockerMake, DateTime? issueDate = null)//(int LockerNumber, int TrxId, int TrxLineId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(lockerAllocateDTO, selectionMode, SQLTrx, issueDate);
            bool isFixedMode = (selectionMode == ParafaitLockCardHandlerDTO.LockerSelectionMode.FIXED.ToString()) | ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "LOCKER_LOCK_MAKE").Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString());
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            LockerAllocation lockerAllocation;
            DateTime lockerIssueDateTime = ((issueDate == DateTime.MinValue || issueDate == null) ? lookupValuesList.GetServerDateTime() : (DateTime)issueDate);
            Locker lockerBL;
            LockerAllocationList lockerAllocationList = new LockerAllocationList();
            List<LockerAllocationDTO> lockerAllocationDTOList = new List<LockerAllocationDTO>();
            List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> lockerAllocationSearchParams = new List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>>();
            lockerBL = new Locker(lockerAllocateDTO.LockerId);
            lockerAllocateDTO.ValidFromTime = lockerIssueDateTime;
            if (isFixedMode)
            {
                if (lockerAllocateDTO.LockerId == -1)
                {
                    throw new Exception("Locker should be selected.");
                }
                lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.REFUNDED, "0"));
                lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.LOCKER_ID, lockerAllocateDTO.LockerId.ToString()));
                lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.VALID_BETWEEN_DATE, lockerIssueDateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                lockerAllocationDTOList = lockerAllocationList.GetAllLockerAllocations(lockerAllocationSearchParams, SQLTrx);
                if (lockerAllocationDTOList == null || (lockerAllocationDTOList != null && lockerAllocationDTOList.Count == 0))
                {
                    lockerAllocation = new LockerAllocation(lockerAllocateDTO);
                    lockerAllocation.Save(SQLTrx);
                }
            }
            else
            {
                lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.CARD_ID, lockerAllocateDTO.CardId.ToString()));
                lockerAllocationDTOList = lockerAllocationList.GetAllLockerAllocations(lockerAllocationSearchParams, SQLTrx);
                log.LogVariableState("lockerAllocationDTOList", lockerAllocationDTOList);
                if (lockerAllocationDTOList != null && lockerAllocationDTOList.Count > 0)
                {
                    lockerAllocateDTO.Id = lockerAllocationDTOList[0].Id;
                }
                else
                {
                    lockerAllocateDTO.Id = -1;
                }
                lockerAllocation = new LockerAllocation(lockerAllocateDTO);
                lockerAllocation.Save(SQLTrx);
            }

            if (lockerAllocateDTO.Id == -1)
            {
                log.Debug("Ends-CreateLockerCard(LockerNumber,TrxId,TrxLineId,SQLTrx) method throws: 'Locker already issued' exception.");
                throw new ApplicationException("Locker already issued");
            }
            if (lockerAllocateDTO.Id > -1)
            {
                this.lockerAllocationDTO = lockerAllocateDTO;
                CreateGuestCard(lockerAllocateDTO.ValidFromTime, lockerAllocateDTO.ValidToTime, isFixedMode, Convert.ToUInt32(((lockerBL.getLockerDTO != null && lockerBL.getLockerDTO.LockerId > -1) ? lockerBL.getLockerDTO.Identifier : 0)), lockerAllocateDTO.ZoneCode, -1, lockerMake, lockerBL.getLockerDTO.ExternalIdentifier);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the locker
        /// </summary>
        public void ReturnLocker(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            lockerAllocationDTO.Refunded = true;
            LockerAllocation lockerAllocation = new LockerAllocation(lockerAllocationDTO);
            lockerAllocation.Save(sqlTrx);
            EraseCard();
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the locker without card
        /// </summary>
        public static void ReturnLockerWithoutCard(LockerAllocationDTO lockerAllocationDTO, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(lockerAllocationDTO);
            if (!lockerAllocationDTO.Refunded)
            {
                lockerAllocationDTO.Refunded = true;
                LockerAllocation lockerAllocation = new LockerAllocation(lockerAllocationDTO);

                lockerAllocation.Save(sqlTrx);
            }
            else
            {
                log.Error("Locker already refunded");
                throw new ApplicationException("Locker already refunded");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Logs the locker action.
        /// </summary>
        public void logLockerCardAction(string Action, object Data, string cardNumber)
        {
            log.LogMethodEntry(Action, Data, cardNumber);
            string Approver = string.IsNullOrEmpty(configCardApprovingUser) ? machineUserContext.GetUserId() : configCardApprovingUser;
            EventLogDTO eventLogDTO = new EventLogDTO(-1, "LOCKER_SETUP", DateTime.Now, "D", machineUserContext.GetUserId(), Environment.MachineName, Action + " - " + cardNumber + " - " + Data.ToString(), "Locker card action", "LOCKER", 2, "Approver", Approver, "", machineUserContext.GetSiteId(), false);
            Semnox.Parafait.logger.EventLog eventLogDataHandler = new Semnox.Parafait.logger.EventLog(machineUserContext, eventLogDTO);
            eventLogDataHandler.Save();
            //eventLogDataHandler.InsertEventLog("LOCKER_SETUP", "D", machineUserContext.GetUserId(), Environment.MachineName, Action + " - " + cardNumber + " - " + Data.ToString(), "Locker card action", "LOCKER", 2, "Approver", Approver, false, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
            log.LogMethodExit();
        }

        public virtual string GetLockerNumber(string zoneCode, string identifier)
        {
            log.LogMethodEntry(zoneCode, identifier);
            string lockerNumber = ((string.IsNullOrEmpty(zoneCode)) ? "0" : zoneCode) + identifier.PadLeft(4, '0');
            log.LogMethodExit(lockerNumber);
            return lockerNumber;
        }
    }
}

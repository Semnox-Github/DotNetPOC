/********************************************************************************************
 * Project Name - Locker Allocation
 * Description  - The bussiness logic for locker allocation
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera     Created 
 *2.70        18-Jul-2019   Dakshakh raj   Modified : Save() method Insert/Update method returns DTO.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System;
namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    ///Bussiness logic class for Locker Allocation operations
    /// </summary>
    public class LockerAllocation
    {
        private LockerAllocationDTO lockerAllocationDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of LockerAllocation class
        /// </summary>
        public LockerAllocation()
        {
            log.LogMethodEntry();
            lockerAllocationDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the Locker DTO based on the locker id passed 
        /// </summary>
        /// <param name="lockerAllocationId">Locker Allocation id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public LockerAllocation(int lockerAllocationId, SqlTransaction sqltransaction = null)
            : this()
        {
            log.LogMethodEntry(lockerAllocationId, sqltransaction);
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqltransaction);
            lockerAllocationDTO = lockerAllocationDataHandler.GetLockerAllocation(lockerAllocationId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates locker allocation object using the LockerAllocationDTO
        /// </summary>
        /// <param name="lockerAllocation">LockerAllocationDTO object</param>
        public LockerAllocation(LockerAllocationDTO lockerAllocation)
            : this()
        {
            log.LogMethodEntry(lockerAllocation);
            this.lockerAllocationDTO = lockerAllocation;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates locker allocation object using the LockerAllocationDTO
        /// </summary>
        /// <param name="cardNumber">Card number passed object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerAllocation(string cardNumber,SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(cardNumber, sqlTransaction);
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqlTransaction);
            lockerAllocationDTO = lockerAllocationDataHandler.GetLockerAllocationOnAPCardNumber(cardNumber);
            log.LogMethodExit();

        }

        /// <summary>
        /// Loads the allocation by card id
        /// </summary>
        /// <param name="cardId">card id of the allocated locker</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void LoadAllocationByCardId(int cardId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardId, sqlTransaction);
            List<LockerAllocationDTO> lockerAllocationDTOs=null;
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqlTransaction);
            List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> lockerAllocationSearchParams = new List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>>();            
            lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.CARD_ID, cardId.ToString()));
            log.LogMethodExit();
            lockerAllocationDTOs = lockerAllocationDataHandler.GetLockerAllocationList(lockerAllocationSearchParams);
            if (lockerAllocationDTOs != null && lockerAllocationDTOs.Count > 0)
            {
                lockerAllocationDTO = lockerAllocationDTOs[0];
            }
            log.LogMethodExit(lockerAllocationDTO);
        }

        /// <summary>
        ///  Set all the parameters passed to DTO
        /// </summary>
        /// <param name="lockerId">lockerId</param>
        /// <param name="isRefunded">isRefunded</param>
        public void SetAllocationDetails(int lockerId, bool isRefunded)
        {
            log.LogMethodEntry(lockerId, isRefunded);
            if (lockerAllocationDTO != null)
            {
                lockerAllocationDTO.LockerId = lockerId;
                lockerAllocationDTO.Refunded = isRefunded;
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Saves the locker allocation
        /// Checks if the allocation id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ExecutionContext machineUserContext =  ExecutionContext.GetExecutionContext();
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler();
            if (lockerAllocationDTO.Id < 0)
            {
                lockerAllocationDTO = lockerAllocationDataHandler.InsertLockerAllocation(lockerAllocationDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                lockerAllocationDTO.AcceptChanges();
            }
            else
            {
                if (lockerAllocationDTO.IsChanged)
                {                   
                    lockerAllocationDataHandler.UpdateLockerAllocation(lockerAllocationDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());             
                    lockerAllocationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the locker allocation record which matches the identifier  
        /// </summary>
        /// <param name="lockerId">integer type parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public LockerAllocationDTO GetLockerAllocationOnLockeId(int lockerId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lockerId, sqlTransaction);
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqlTransaction);
            LockerAllocationDTO lockerAllocationDTO = lockerAllocationDataHandler.GetLockerAllocationOnLockerId(lockerId);
            log.LogMethodExit(lockerAllocationDTO);
            return lockerAllocationDTO;
        }

        /// <summary>
        /// Gets the locker allocation record which matches the identifier  
        /// </summary>
        /// <param name="lockerNumber">integer type parameter</param>
        /// <param name="sqlTransaction">sqlTransactionr</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public LockerAllocationDTO GetLockerAllocationOnAPLockerNumber(int lockerNumber, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lockerNumber, sqlTransaction);
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqlTransaction);
            LockerAllocationDTO lockerAllocationDTO = lockerAllocationDataHandler.GetLockerAllocationOnAPLockerNumber(lockerNumber);
            log.LogMethodExit(lockerAllocationDTO);
            return lockerAllocationDTO;
        }

        /// <summary>
        /// Check the locker issue mode and inserts the record based on the mode.
        /// </summary>
        /// <param name="sqlTransaction">Holds the sql transaction object</param>
        public void Save( SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext machineUserContext =  ExecutionContext.GetExecutionContext();
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqlTransaction);
            if (lockerAllocationDTO.Id < 0)
            {
                lockerAllocationDTO = lockerAllocationDataHandler.InsertLockerAllocation(lockerAllocationDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                lockerAllocationDTO.AcceptChanges();
            }
            else
            {
                if (lockerAllocationDTO.IsChanged)
                {
                    lockerAllocationDataHandler.UpdateLockerAllocation(lockerAllocationDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    lockerAllocationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        public LockerAllocationDTO GetValidAllocation(int lockerId,int cardId)
        {
            log.LogMethodEntry();
            List<LockerAllocationDTO> lockerAllocationDTOList = null;
            
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            string value = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "LOCKER_RETURN_ALLOWED_AFTER_EXPIRY");
            int lockerReturnLimit = Convert.ToInt32(string.IsNullOrEmpty(value) ? "-1" : value);
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            DateTime serverTimeNow = lookupValuesList.GetServerDateTime();
            LockerAllocationList lockerAllocationList = new LockerAllocationList();
            List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> lockerAllocationSearchParams = new List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>>();
            if (lockerId != -1 || cardId != -1)
            {
                lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.REFUNDED, "0"));
                if (lockerId != -1)
                {
                    lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.LOCKER_ID, lockerId.ToString()));
                }
                if (cardId != -1)
                {
                    lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.CARD_ID, cardId.ToString()));
                }
                if (lockerReturnLimit > -1)
                {
                    serverTimeNow = serverTimeNow.AddHours(lockerReturnLimit * -1);
                    lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.LOCKER_RETURN_POLICY_LIMIT, serverTimeNow.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                lockerAllocationDTOList = lockerAllocationList.GetAllLockerAllocations(lockerAllocationSearchParams);
                if (lockerAllocationDTOList == null || (lockerAllocationDTOList != null && lockerAllocationDTOList.Count == 0))
                {
                    log.LogMethodExit(null);
                    return new LockerAllocationDTO();
                }
            }
            else
            {
                log.LogMethodExit(null);
                return new LockerAllocationDTO();
            }
            log.LogMethodExit(lockerAllocationDTOList[0]);
            return lockerAllocationDTOList[0];
        }

        /// <summary>
        /// Gets the locker allocation record which matches the locker card id  
        /// </summary>
        /// <param name="cardId">integer type parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public LockerAllocationDTO GetLockerAllocationOnCardId(int cardId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardId, sqlTransaction);
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqlTransaction);
            LockerAllocationDTO lockerAllocationDTO = lockerAllocationDataHandler.GetLockerAllocationOnCardId(cardId);
            log.LogMethodExit(lockerAllocationDTO);
            return lockerAllocationDTO;
        }
        
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LockerAllocationDTO GetLockerAllocationDTO { get { return lockerAllocationDTO; } }
    }

    /// <summary>
    /// Manages the list of locker allocations
    /// </summary>
    public class LockerAllocationList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the locker allocation list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerAllocationDTO> GetAllLockerAllocations(List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqlTransaction);
            List<LockerAllocationDTO> lockerAllocationDTOList = lockerAllocationDataHandler.GetLockerAllocationList(searchParameters);
            log.LogMethodExit(lockerAllocationDTOList);
            return lockerAllocationDTOList;
        }
        /// <summary>
        /// Returns the allocation for the zone list passed 
        /// </summary>
        /// <param name="zoneList">List of zone id's for the in query eg :- (1,2,3) </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerAllocationDTO> GetLockerAllocationOnZoneList(string zoneList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(zoneList, sqlTransaction);
            LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler(sqlTransaction);
            List<LockerAllocationDTO> lockerAllocationDTOList = lockerAllocationDataHandler.GetLockerAllocationOnZoneList(zoneList);
            log.LogMethodExit(lockerAllocationDTOList);
            return lockerAllocationDTOList;
        }
    }
}

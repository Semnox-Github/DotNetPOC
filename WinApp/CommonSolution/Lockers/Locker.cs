/********************************************************************************************
 * Project Name - Locker
 * Description  - The bussiness logic for locker
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera     Created
 *2.70.2        19-Jul-2019   Dakshakh raj   Modified : Save() method Insert/Update method returns DTO.
 *2.90        27-May-2020   Mushahid Faizan Modified : 3 tier changes for Rest API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    ///Bussiness logic class for Locker operations
    /// </summary>
    public class Locker
    {
        private LockerDTO lockerDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of Locker class
        /// </summary>
        public Locker(ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the Locker DTO based on the locker id passed 
        /// </summary>
        /// <param name="lockerId">Locker id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Locker(int lockerId, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(lockerId, sqlTransaction);
            LockerDataHandler lockerDataHandler = new LockerDataHandler(sqlTransaction);
            lockerDTO = lockerDataHandler.GetLocker(lockerId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates locker object using the LockerDTO :this is used in the AccessPointCommunication.cs
        /// </summary>
        /// <param name="locker">LockerDTO object</param>
        public Locker(LockerDTO locker)
        {
            log.LogMethodEntry(locker);
            this.lockerDTO = locker;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates locker object using the LockerDTO
        /// </summary>
        /// <param name="locker">LockerDTO object</param>
        public Locker(ExecutionContext executionContext, LockerDTO locker)
            : this(executionContext)
        {
            log.LogMethodEntry(locker);
            this.lockerDTO = locker;
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the locker record
        /// </summary>
        /// <param name="panelId">int parameter</param>
        /// <param name="rows">int parameter</param>
        /// <param name="columns">int parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void RemoveLockers(int panelId, int rows, int columns,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(panelId, rows, columns, sqlTransaction);
            LockerDataHandler lockerDataHandler = new LockerDataHandler(sqlTransaction);            
            lockerDataHandler.RemoveLockers(panelId, rows, columns);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the locker details on identifire
        /// </summary>
        /// <param name="identifire">integer type parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns LockerDTO</returns>
        public LockerDTO GetLockerDetailsOnidenfire(int identifire,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(identifire, sqlTransaction);
            LockerDataHandler lockerDataHandler = new LockerDataHandler(sqlTransaction);
            LockerDTO lockerDTO =  lockerDataHandler.GetLockerDetailsOnIdentifier(identifire);
            log.LogMethodExit(lockerDTO);
            return lockerDTO;
        }

        /// <summary>
        /// Gets the locker details on card id
        /// </summary>
        /// <param name="cardId">integer type parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns LockerDTO</returns>
        public LockerDTO GetLockerDetailsOnCardId(int cardId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardId, sqlTransaction);
            LockerDataHandler lockerDataHandler = new LockerDataHandler(sqlTransaction);
            LockerDTO lockerDTO = lockerDataHandler.GetLockerDetailsOnCardId(cardId);
            log.LogMethodExit(lockerDTO);
            return lockerDTO;
        }

        /// <summary>
        /// Saves the locker record
        /// Checks if the locker id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            //ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            LockerDataHandler lockerDataHandler = new LockerDataHandler(sqlTransaction);
            if (lockerDTO.IsChanged == false  && lockerDTO.LockerId >-1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (lockerDTO.LockerId < 0)
            {
                lockerDTO = lockerDataHandler.InsertLocker(lockerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                lockerDTO.AcceptChanges();
            }
            else if (lockerDTO.IsChanged)
            {
                lockerDataHandler.UpdateLocker(lockerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                lockerDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (lockerDTO == null)
            {
                //Validation to be implemented.
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Get Selected LockerDTO
        /// </summary>
        /// <param name="lockerAllocationDTO">lockerAllocationDTO</param>
        /// <param name="panelId">panelId</param>
        /// <param name="lockerNumber">lockerNumber</param>
        /// <param name="cardNumber">cardNumber</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public LockerDTO GetSelectedLockerDTO(LockerAllocationDTO lockerAllocationDTO, int panelId, uint lockerNumber,string cardNumber, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lockerAllocationDTO, panelId, lockerNumber, cardNumber, sqlTransaction);
            if (lockerAllocationDTO != null && lockerAllocationDTO.LockerId > -1)
            {
                LockerDataHandler lockerDataHandler = new LockerDataHandler(sqlTransaction);
                lockerDTO = lockerDataHandler.GetLocker(lockerAllocationDTO.LockerId);                
            }
            else
            {
                List<LockerDTO> lockerDTOs = new List<LockerDTO>();
                LockersList lockersList = new LockersList();
                List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>> searchLockerParameters = new List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>>();
                searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.IS_ACTIVE, "1"));
                searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.IDENTIFIRE, lockerNumber.ToString()));
                if (panelId != -1)
                {
                    searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.PANEL_ID, panelId.ToString()));
                }
                lockerDTOs = lockersList.GetAllLocker(searchLockerParameters, true, sqlTransaction);
                if (lockerDTOs != null && lockerDTOs.Count > 0)
                {
                    foreach (LockerDTO lockerDto in lockerDTOs.OrderBy(s => s.LockerAllocated))
                    {
                        if (lockerDto.LockerAllocated != null)
                        {
                            if (lockerDto.LockerAllocated.CardNumber.Equals(cardNumber))
                            {
                                lockerDto.LockerAllocated.ValidFromTime = lockerDto.LockerAllocated.ValidFromTime.AddMinutes(1);
                                lockerDto.LockerAllocated.ValidToTime = lockerDto.LockerAllocated.ValidToTime.AddMinutes(1);
                                //validFromTime = lockerDto.LockerAllocated.ValidFromTime;
                                //validToTime = lockerDto.LockerAllocated.ValidToTime;
                                LockerAllocation lockerAllocation = new LockerAllocation(lockerDto.LockerAllocated);
                                lockerAllocation.Save(null);
                                lockerDTO = lockerDto;
                                break;
                            }
                        }
                        else if (lockerDto.LockerAllocated == null)
                        {
                            lockerDTO = lockerDto;
                            break;
                        }
                    }
                    if (lockerDTO == null)
                    {
                        log.Info("Invalid locker or card!!!...");
                        throw new Exception("Invalid locker or card!!!...");
                    }
                }
                else
                {
                    log.Info("Invalid locker!!!...");
                    throw new Exception("Invalid locker!!!...");
                }
            }
            log.LogMethodExit(lockerDTO, "lockerDTO");
            return lockerDTO;
        }

        /// <summary>
        /// Update Card Override Sequence
        /// </summary>
        /// <param name="maxlimit">maxlimit</param>
        /// <returns></returns>
        public int UpdateCardOverrideSequence(int maxlimit)
        {
            log.LogMethodEntry(maxlimit);
            int cardOverride = -1;
            if (lockerDTO != null)
            {
                if (lockerDTO.CardOverrideSequence == -1)
                {
                    lockerDTO.CardOverrideSequence = 1;
                }
                else
                {
                    if (lockerDTO.CardOverrideSequence == 255)
                    {
                        lockerDTO.CardOverrideSequence = 1;
                    }
                    else
                    {
                        lockerDTO.CardOverrideSequence += 1;
                    }
                }
                Save();
                cardOverride = lockerDTO.CardOverrideSequence;
            }
            log.LogMethodExit();
            return cardOverride;
        }
       
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LockerDTO getLockerDTO { get { return lockerDTO; } }
    }

    /// <summary>
    /// Manages the list of lockers
    /// </summary>
    public class LockersList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<LockerDTO> lockerDTOList = new List<LockerDTO>();

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LockersList(ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LockersList(ExecutionContext executionContext, List<LockerDTO> lockerDTOList) : this(executionContext)
        {
            log.LogMethodEntry(lockerDTOList);
            this.lockerDTOList = lockerDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the locker detail which matches with the passed locker id and status
        /// </summary>
        /// <param name="lockerIdFrom">integer type parameter</param>
        /// <param name="lockerIdTo">integer type parameter</param>
        /// <param name="lockerStatus">string type parameter</param>
        /// <param name="batteryStatus">string type parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns LockersDTO</returns>
        public List<LockerDTO> GetLockersWithStatus(int lockerIdFrom, int lockerIdTo, string lockerStatus, string batteryStatus, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lockerIdFrom, lockerIdTo, lockerStatus, batteryStatus, sqlTransaction);
            LockerDataHandler lockerDataHandler = new LockerDataHandler(sqlTransaction);
            List<LockerDTO> lockerDTOList = lockerDataHandler.GetLockersWithStatus(lockerIdFrom, lockerIdTo, lockerStatus, batteryStatus);
            log.LogMethodExit(lockerDTOList);
            return lockerDTOList;
        }

        /// <summary>
        /// Returns the locker list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecord">loadChildRecord</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerDTO> GetAllLocker(List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>> searchParameters, bool loadChildRecord, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, loadChildRecord, sqlTransaction);
            LockerDataHandler lockerDataHandler = new LockerDataHandler(sqlTransaction);
            List<LockerDTO> lockerDTOList = lockerDataHandler.GetLockersList(searchParameters, loadChildRecord);
            log.LogMethodExit(lockerDTOList);
            return lockerDTOList;
        }

        /// <summary>
        /// Save or update locker for Web Management Studio
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            if (lockerDTOList == null ||
               lockerDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < lockerDTOList.Count; i++)
            {
                var lockerDTO = lockerDTOList[i];
                if (lockerDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    Locker locker = new Locker(executionContext, lockerDTO);
                    locker.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving lockerDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("lockerDTO", lockerDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}

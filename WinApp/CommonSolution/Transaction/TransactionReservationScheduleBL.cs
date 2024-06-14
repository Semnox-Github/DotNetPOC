/* Project Name - TransactionReservationScheduleBL  
* Description  - BL class of the TrxReservationSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70        26-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes 
*2.90        03-Jun-2020    Guru S A             Reservation enhancements for commando release
*2.120.0     09-Oct-2020    Guru S A             Membership engine sql session issue
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// BL class for TrxReservationSchedule
    /// </summary>
    public class TransactionReservationScheduleBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TransactionReservationScheduleDTO transactionReservationScheduleDTO;
        private ExecutionContext executionContext;
        private LookupValuesList serverTimeObject;

        /// <summary>
        /// Parameterized constructor of TransactionReservationScheduleBL class
        /// </summary>
        private TransactionReservationScheduleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            transactionReservationScheduleDTO = null;
            this.executionContext = executionContext;
            serverTimeObject = new LookupValuesList(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TransactionReservationScheduleBL id as the parameter
        /// Would fetch the TransactionReservationScheduleBL object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public TransactionReservationScheduleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionReservationScheduleDatahandler transactionReservationScheduleDatahandler = new TransactionReservationScheduleDatahandler(sqlTransaction);
            transactionReservationScheduleDTO = transactionReservationScheduleDatahandler.GetTransactionReservationScheduleDTO(id);
            if (transactionReservationScheduleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TransactionReservationScheduleDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TransactionReservationScheduleBL object using the TransactionReservationScheduleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="transactionReservationScheduleDTO">TransactionReservationScheduleDTO object</param>
        public TransactionReservationScheduleBL(ExecutionContext executionContext, TransactionReservationScheduleDTO transactionReservationScheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionReservationScheduleDTO);
            this.transactionReservationScheduleDTO = transactionReservationScheduleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TransactionReservationScheduleBL
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TransactionReservationScheduleDatahandler transactionReservationScheduleDatahandler = new TransactionReservationScheduleDatahandler(sqlTransaction);
            if (transactionReservationScheduleDTO.TrxReservationScheduleId < 0)
            {
                CanAcceptReservationSchedule(sqlTransaction);
                SetExpiryDate(sqlTransaction);
                int id = transactionReservationScheduleDatahandler.InsertTransactionReservationSchedule(transactionReservationScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                transactionReservationScheduleDTO.TrxReservationScheduleId = id;
                transactionReservationScheduleDTO.AcceptChanges();
            }
            else
            {
                if (transactionReservationScheduleDTO.IsChanged)
                {
                    ReSetExpiryDate();
                    transactionReservationScheduleDatahandler.UpdateTransactionReservationSchedule(transactionReservationScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionReservationScheduleDTO.AcceptChanges();
                }
            } 
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionReservationScheduleDTO TransactionReservationScheduleDTO
        {
            get
            {
                return transactionReservationScheduleDTO;
            }
        }
        /// <summary>
        /// Can Accept Reservation Schedule
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void CanAcceptReservationSchedule(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (this.transactionReservationScheduleDTO != null)
            {
                int scheduleId = transactionReservationScheduleDTO.SchedulesId;
                int facilityMapId = transactionReservationScheduleDTO.FacilityMapId;
                DateTime scheduleFrom = transactionReservationScheduleDTO.ScheduleFromDate;
                DateTime scheduleTo = transactionReservationScheduleDTO.ScheduleToDate;
                SchedulesBL schedulesBL = new SchedulesBL(executionContext, scheduleId, null, true);
                decimal fromTime = Convert.ToDecimal(scheduleFrom.Hour + scheduleFrom.Minute / 100.0);
                decimal toTime = Convert.ToDecimal(scheduleTo.Hour + scheduleTo.Minute / 100.0);
                int duration = (int)(scheduleTo - scheduleFrom).TotalMinutes;
                string message = string.Empty;
                if (scheduleFrom > scheduleTo)
                {
                    message = MessageContainerList.GetMessage(executionContext, 305);
                    log.Error(message);
                    log.LogMethodExit(message);
                    throw new ValidationException(message);
                }
                if (transactionReservationScheduleDTO.GuestQuantity < 1)
                {
                    message = MessageContainerList.GetMessage(executionContext, 2104);//Please enter valid guest quantity
                    log.Error(message);
                    log.LogMethodExit(message);
                    throw new ValidationException(message);
                }
                ScheduleDetailsDTO elibleSchedule = schedulesBL.GetEligibleScheduleDetails(scheduleFrom, fromTime, toTime, facilityMapId);
                if (elibleSchedule != null)
                {
                    FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, facilityMapId);
                    facilityMapBL.CanAccomodateReservationQty(transactionReservationScheduleDTO.GuestQuantity, elibleSchedule.RuleUnits, scheduleFrom, scheduleTo, transactionReservationScheduleDTO.TrxId, transactionReservationScheduleDTO.TrxReservationScheduleId);
                }
                else
                {
                    log.Error("Invalid scheudle info - scheduleId: " + scheduleId.ToString() + "FacilityMapId: " + facilityMapId.ToString() + "scheduleFrom: " + scheduleFrom.ToString() + " scheduleTo:" + toTime.ToString());
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2140, scheduleFrom.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext,"DATETIME_FORMAT")), scheduleTo.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
                    // "Schedule " + scheduleFrom.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) + " to " + scheduleTo.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) + "is not valid"
                }
            }
            log.LogMethodExit();
        }
        private void SetExpiryDate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (transactionReservationScheduleDTO.TrxId == -1)
            {
                int reservationCompletionLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "RESERVATION_COMPLETION_TIME_LIMIT", 0);
                transactionReservationScheduleDTO.ExpiryDate = serverTimeObject.GetServerDateTime().AddMinutes(reservationCompletionLimit);
            }
            log.LogMethodExit();
        }

        private void ReSetExpiryDate()
        {
            log.LogMethodEntry();
            if (transactionReservationScheduleDTO.TrxId > -1)
            {
                transactionReservationScheduleDTO.ExpiryDate = null;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// ExpireSchedule will expire or delete the record
        /// </summary>
        public void ExpireSchedule(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (transactionReservationScheduleDTO.TrxReservationScheduleId > -1)
            {
                TransactionReservationScheduleDatahandler transactionReservationScheduleDatahandler = new TransactionReservationScheduleDatahandler(sqlTransaction);
                if (transactionReservationScheduleDTO.TrxId > -1)
                {
                    transactionReservationScheduleDTO.Cancelled = true;
                    transactionReservationScheduleDTO.CancelledBy = executionContext.GetUserId();
                    transactionReservationScheduleDTO.ExpiryDate = serverTimeObject.GetServerDateTime();
                    transactionReservationScheduleDatahandler.UpdateTransactionReservationSchedule(transactionReservationScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionReservationScheduleDTO.AcceptChanges();
                }
                else
                {
                    transactionReservationScheduleDatahandler.DeleteTransactionReservationSchedule(transactionReservationScheduleDTO.TrxReservationScheduleId);
                    transactionReservationScheduleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }


    /// <summary>
    /// Manages the list of TransactionReservationScheduleBLs
    /// </summary>
    public class TransactionReservationScheduleListBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public TransactionReservationScheduleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetTransactionReservationScheduleDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<TransactionReservationScheduleDTO></returns>
        public List<TransactionReservationScheduleDTO> GetTransactionReservationScheduleDTOList(List<KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionReservationScheduleDatahandler transactionReservationScheduleDatahandler = new TransactionReservationScheduleDatahandler(sqlTransaction);
            List<TransactionReservationScheduleDTO> returnValue = transactionReservationScheduleDatahandler.GetTransactionReservationScheduleDTOList(searchParameters);             
            log.LogMethodExit(returnValue);
            return returnValue;
        }
         
    }

}
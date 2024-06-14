/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  TransactionLine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        11-Nov-2019   Lakshminarayana         Created 
 *2.100         24-Sep-2020   Nitin Pai             Added transaction line list class to get transaction dto list
 *2.140.0       01-Jun-2021   Fiona Lishal          Modified for Delivery Order enhancements for F&B
 *2.140.2       14-APR-2022    Girish Kundar       Modified : Aloha BSP changes
 *2.140.2       17-May-2022   Girish Kundar        Modified : TET changes 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction.KDS;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for TransactionLine class.
    /// </summary>
    public class TransactionLineBL
    {
        private TransactionLineDTO transactionLineDto;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TransactionLineBL class
        /// </summary>
        private TransactionLineBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TransactionLineBL object using the TransactionLineDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="transactionLineDto">TransactionLineDTO object</param>
        public TransactionLineBL(ExecutionContext executionContext, TransactionLineDTO transactionLineDto)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionLineDto);
            this.transactionLineDto = transactionLineDto;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the transactionLineDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (transactionLineDto.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TransactionLineDataHandler transactionLineDataHandler = new TransactionLineDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (transactionLineDto.IsChanged)
            {
                log.LogVariableState("TransactionLineDTO", transactionLineDto);
                transactionLineDto = transactionLineDataHandler.Update(transactionLineDto, executionContext.GetUserId(), executionContext.GetSiteId());
                transactionLineDto.AcceptChanges();
            }
            log.LogMethodExit();
        }

        public void UpdateRemarks(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TransactionLineDataHandler transactionLineDataHandler = new TransactionLineDataHandler(sqlTransaction);
            transactionLineDataHandler.UpdateRemarks(transactionLineDto, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TransactionLineDTO  values 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (transactionLineDto.TransactionId < 0)
            {
                ValidationError validationError = new ValidationError("TransactionLine", "TransactionId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Transaction Id")));
                validationErrorList.Add(validationError);
            }
            if (transactionLineDto.LineId < 0)
            {
                ValidationError validationError = new ValidationError("TransactionLine", "LineId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Display Batch Id")));
                validationErrorList.Add(validationError);
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// AmendKOTScheduleTime
        /// </summary>
        /// <param name="timeToAmend"></param>
        /// <param name="sqlTransaction"></param>
        public void AmendKOTScheduleTime(double timeToAmend, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(timeToAmend, sqlTransaction);
            CanAmendKOTSchedule();

            if (transactionLineDto.KDSOrderLineDTOList != null && transactionLineDto.KDSOrderLineDTOList.Any())
            {
                foreach (KDSOrderLineDTO orderItem in transactionLineDto.KDSOrderLineDTOList)
                {
                    KDSOrderLineBL kDSOrderLineBL = new KDSOrderLineBL(executionContext, orderItem.Id, sqlTransaction);
                    kDSOrderLineBL.AmendKOTScheduleTime(timeToAmend, sqlTransaction);
                }
            } 
            log.LogMethodExit();
        }
        private void CanAmendKOTSchedule()
        {
            log.LogMethodEntry();
            if (this.transactionLineDto == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4015);
                //
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (transactionLineDto.CancelledTime.HasValue)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 231);
                //Transaction Line(s) Cancelled
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (transactionLineDto.KDSOrderLineDTOList != null && transactionLineDto.KDSOrderLineDTOList.Any() 
                 && transactionLineDto.KDSOrderLineDTOList.Exists(k => k.ScheduleTime == null))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4020);
                //Cannot Amend Time for Immediate Orders
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (transactionLineDto.KDSOrderLineDTOList != null && transactionLineDto.KDSOrderLineDTOList.Any()
                 && transactionLineDto.KDSOrderLineDTOList.Exists(k => (k.ScheduleTime != null
                                                                         && (k.OrderedTime != null || k.PrepareStartTime != null || k.PreparedTime != null || k.DeliveredTime != null))))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "xxxx");
                //Cannot amend  time as item is not longer in scheduled status
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }             
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionLineDTO TransactionLineDTO
        {
            get
            {
                return transactionLineDto;
            }
        }

    }

    /// <summary>
    /// Business logic for TransactionLine List class.
    /// </summary>
    public class TransactionLineListBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<TransactionLineDTO> transactionLineDTOList;

        public TransactionLineListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public TransactionLineListBL(ExecutionContext executionContext)
            : this()
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="transactionLineDTOList">transactionLineDTOList</param>
        public TransactionLineListBL(ExecutionContext executionContext, List<TransactionLineDTO> transactionLineDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.transactionLineDTOList = transactionLineDTOList;
            log.LogMethodExit();
        }

        public List<TransactionLineDTO> GetTransactionLineDTOList(List<int> transactionIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionIdList, sqlTransaction);
            TransactionLineDataHandler transactionLineDataHandler = new TransactionLineDataHandler(sqlTransaction);
            List<TransactionLineDTO> transactionLineDTOList = transactionLineDataHandler.GetTransactionLines(transactionIdList);
            log.LogMethodExit(transactionLineDTOList);
            return transactionLineDTOList;
        }

        public List<TransactionLineDTO> GetTransactionLineDTOList(List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>> searchParameters, int pageNumber = 0, int pageSize = 500, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            TransactionLineDataHandler transactionLineDataHandler = new TransactionLineDataHandler(sqlTransaction);
            List<TransactionLineDTO> transactionLineDTOList = transactionLineDataHandler.GetTransactionLineDTOList(searchParameters, pageNumber, pageSize);
            log.LogMethodExit(transactionLineDTOList);
            return transactionLineDTOList;
        }
        /// <summary>
        /// Validates and saves the RedemptionGiftsDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (transactionLineDTOList == null ||
                !transactionLineDTOList.Any())
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            TransactionLineDataHandler transactionLineDataHandler = new TransactionLineDataHandler(sqlTrx); ;
            transactionLineDataHandler.Save(transactionLineDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
    }
}

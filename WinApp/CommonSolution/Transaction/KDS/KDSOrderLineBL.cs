/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  KDSOrderLine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        16-June-2019   Girish Kundar           Created 
 *2.140.0     21-Jun-2021    Fiona Lishal            Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Business logic for KDSOrderLine class.
    /// </summary>
    public class KDSOrderLineBL
    {
        private KDSOrderLineDTO kdsOrderLineDto;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of KDSOrderLineBL class
        /// </summary>
        private KDSOrderLineBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates KDSOrderLineBL object using the KDSOrderLineDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="kdsOrderLineDto">KDSOrderLineDTO object</param>
        public KDSOrderLineBL(ExecutionContext executionContext, KDSOrderLineDTO kdsOrderLineDto)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, kdsOrderLineDto);
            this.kdsOrderLineDto = kdsOrderLineDto;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the KDSOrderLine id as the parameter
        /// Would fetch the KDSOrderLine object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of KDSOrderLine passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public KDSOrderLineBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            KDSOrderLineDataHandler kdsOrderLineDataHandler = new KDSOrderLineDataHandler(sqlTransaction);
            kdsOrderLineDto = kdsOrderLineDataHandler.GetKDSOrderLineDTO(id);
            if (kdsOrderLineDto == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "KDSOrderLine", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the kdsOrderLineDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (kdsOrderLineDto.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            KDSOrderLineDataHandler kdsOrderLineDataHandler = new KDSOrderLineDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (kdsOrderLineDto.Id < 0)
            {
                log.LogVariableState("KDSOrderLineDTO", kdsOrderLineDto);
                kdsOrderLineDto = kdsOrderLineDataHandler.Insert(kdsOrderLineDto, executionContext.GetUserId(), executionContext.GetSiteId());
                kdsOrderLineDto.AcceptChanges();
            }
            else if (kdsOrderLineDto.IsChanged)
            {
                log.LogVariableState("KDSOrderLineDTO", kdsOrderLineDto);
                kdsOrderLineDto = kdsOrderLineDataHandler.Update(kdsOrderLineDto, executionContext.GetUserId(), executionContext.GetSiteId());
                kdsOrderLineDto.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the KDSOrderLineDTO  values 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (kdsOrderLineDto.TrxId < 0)
            {
                ValidationError validationError = new ValidationError("KDSOrderLine", "TrxId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Transaction Id")));
                validationErrorList.Add(validationError);
            }
            if (kdsOrderLineDto.DisplayBatchId < 0)
            {
                ValidationError validationError = new ValidationError("KDSOrderLine", "DisplayBatchId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Display Batch Id")));
                validationErrorList.Add(validationError);
            }
            if (kdsOrderLineDto.TerminalId < 0)
            {
                ValidationError validationError = new ValidationError("KDSOrderLine", "TerminalId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Terminal Id")));
                validationErrorList.Add(validationError);
            }
            if (kdsOrderLineDto.LineId < 0)
            {
                ValidationError validationError = new ValidationError("KDSOrderLine", "LineId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Line Id")));
                validationErrorList.Add(validationError);
            }
            if (kdsOrderLineDto.DisplayTemplateId < 0)
            {
                ValidationError validationError = new ValidationError("KDSOrderLine", "DisplayTemplateId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Display Template Id")));
                validationErrorList.Add(validationError);
            }
            if ((kdsOrderLineDto.OrderedTime.HasValue == false || kdsOrderLineDto.OrderedTime.Value == DateTime.MinValue) 
                && (kdsOrderLineDto.ScheduleTime.HasValue == false || kdsOrderLineDto.ScheduleTime.Value == DateTime.MinValue))
            {
                ValidationError validationError = new ValidationError("KDSOrderLine", "OrderedTime", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Ordered Time")));
                validationErrorList.Add(validationError);
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public KDSOrderLineDTO KDSOrderLineDTO
        {
            get
            {
                return kdsOrderLineDto;
            }
        }

        /// <summary>
        /// Prepares the order line
        /// </summary>
        /// <param name="currentServerTime">current Server Time</param>
        internal void Prepare(DateTime currentServerTime)
        {
            log.LogMethodEntry();
            if (kdsOrderLineDto.PreparedTime.HasValue)
            {
                log.LogMethodExit(null, "order line is already prepared");
                return;
            }

            kdsOrderLineDto.PreparedTime = currentServerTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Delivers the order line
        /// </summary>
        /// <param name="currentServerTime">current Server Time</param>
        internal void Deliver(DateTime currentServerTime)
        {
            log.LogMethodEntry();
            
            if (kdsOrderLineDto.DeliveredTime.HasValue)
            {
                log.LogMethodExit(null, "order line is already delivered");
                return;
            }
            if (kdsOrderLineDto.PreparedTime.HasValue == false)
            {
                Prepare(currentServerTime);
            }
            kdsOrderLineDto.DeliveredTime = currentServerTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns whether order line is prepared
        /// </summary>
        /// <returns></returns>
        internal bool IsPrepared()
        {
            return kdsOrderLineDto.LineCancelledTime.HasValue || kdsOrderLineDto.PreparedTime.HasValue;
        }

        /// <summary>
        /// Returns whether order line is delivered
        /// </summary>
        /// <returns></returns>
        internal bool IsDelivered()
        {
            return kdsOrderLineDto.LineCancelledTime.HasValue || kdsOrderLineDto.DeliveredTime.HasValue;
        }

        /// <summary>
        /// Recalls an item to kitchen for fixing
        /// </summary>
        public void RecallToKitchen()
        {
            log.LogMethodEntry();
            if (kdsOrderLineDto.DeliveredTime.HasValue)
            {
                RecallToDeliveryStation();
            }

            if (kdsOrderLineDto.PreparedTime.HasValue == false)
            {
                log.LogMethodExit(null, "item is not yet prepared.");
                return;
            }
            kdsOrderLineDto.PreparedTime = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Recalls an delivered item to delivery station. Which may be further recalled to kitchen for fixing
        /// </summary>
        public void RecallToDeliveryStation()
        {
            log.LogMethodEntry();
            if (kdsOrderLineDto.DeliveredTime.HasValue == false)
            {
                log.LogMethodExit(null, "item is not yet delivered.");
                return;
            }
            kdsOrderLineDto.DeliveredTime = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the status of the line
        /// </summary>
        /// <returns>status of the order line</returns>
        public string GetStatus()
        {
            log.LogMethodEntry();
            string status;
            if (IsCancelled())
            {
                status = "X";
            }
            else if (IsDelivered())
            {
                status = "D";
            }
            else if (IsPrepared())
            {
                status = "C";
            }
            else
            {
                status = "P";
            }
            log.LogMethodExit(status);
            return status;
        }

        /// <summary>
        /// returns whether the line is cancelled
        /// </summary>
        /// <returns></returns>
        private bool IsCancelled()
        {
            return kdsOrderLineDto.LineCancelledTime.HasValue;
        }

        /// <summary>
        /// Prepares an cancelled order line
        /// </summary>
        /// <param name="currentServerTime"></param>
        public void PrepareCancelledLine(DateTime currentServerTime)
        {
            log.LogMethodEntry(currentServerTime);
            if (kdsOrderLineDto.PreparedTime.HasValue == false)
            {
                kdsOrderLineDto.PreparedTime = currentServerTime;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Prepares an cancelled order line
        /// </summary>
        /// <param name="currentServerTime"></param>
        public void DeliverCancelledLine(DateTime currentServerTime)
        {
            log.LogMethodEntry(currentServerTime);
            if (kdsOrderLineDto.DeliveredTime.HasValue == false)
            {
                kdsOrderLineDto.DeliveredTime = currentServerTime;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// AmendKOTScheduleTime
        /// </summary>
        /// <param name="minutes"></param>
        /// <param name="sqlTransaction"></param>
        public void AmendKOTScheduleTime(double minutes, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(minutes, sqlTransaction);
            CanAmendKOTSchedule();
            if (minutes == 0)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime serverDateTime = lookupValuesList.GetServerDateTime();
                kdsOrderLineDto.ScheduleTime = serverDateTime;
            }
            else
            {
                DateTime scheduleTime =(DateTime)kdsOrderLineDto.ScheduleTime;
                kdsOrderLineDto.ScheduleTime = scheduleTime.AddMinutes(minutes);
            }
            Save(sqlTransaction);
            log.LogMethodExit();
        }
        private void CanAmendKOTSchedule()
        {
            log.LogMethodEntry();
            if (IsDelivered())
            {
                string message = MessageContainerList.GetMessage(executionContext, 3032);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            if (IsPrepared())
            {
                string message = MessageContainerList.GetMessage(executionContext, 3033);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            if (kdsOrderLineDto.ScheduleTime == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 3041);
                log.LogMethodExit(null, "Throwing Exception - " + message);//ScheduleTime Cannot be null
                throw new Exception("ScheduleTime Cannot be null");
            }
            log.LogMethodExit();
        }
    }
    /// <summary>
    /// Manages the list of KDSOrderLine
    /// </summary>
    public class KDSOrderLineListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<KDSOrderLineDTO> kdsOrderLineDtoList = new List<KDSOrderLineDTO>();
        /// <summary>
        /// Parameterized constructor for KDSOrderLineListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public KDSOrderLineListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for KDSOrderLineListBL with kdsOrderLineDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="kdsOrderLineDtoList">kdsOrderLineDTOList object is passed as parameter</param>
        public KDSOrderLineListBL(ExecutionContext executionContext,
                                                List<KDSOrderLineDTO> kdsOrderLineDtoList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, kdsOrderLineDtoList);
            this.kdsOrderLineDtoList = kdsOrderLineDtoList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the KDSOrderLineDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of KDSOrderLineDTO</returns>
        public List<KDSOrderLineDTO> GetKDSOrderLineDTOList(List<KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            KDSOrderLineDataHandler kdsOrderLineDataHandler = new KDSOrderLineDataHandler(sqlTransaction);
            List<KDSOrderLineDTO> result = kdsOrderLineDataHandler.GetKDSOrderLineDTOList(searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Saves the  List of kdsOrderLineDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (kdsOrderLineDtoList == null ||
                kdsOrderLineDtoList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < kdsOrderLineDtoList.Count; i++)
            {
                var kdsOrderLineDto = kdsOrderLineDtoList[i];
                if (kdsOrderLineDto.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    KDSOrderLineBL kdsOrderLineBl = new KDSOrderLineBL(executionContext, kdsOrderLineDto);
                    kdsOrderLineBl.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving KDSOrderLineDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("KDSOrderLineDTO", kdsOrderLineDto);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is called from the Parent class KDSOrder.
        /// Gets KDSOrderLineDTO List based on the Id list of displayBatchId
        /// </summary>
        /// <param name="displayBatchIdSet">displayBatchId has list of Ids</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the AppUIPanelElementAttributeDTO List</returns>
        public List<KDSOrderLineDTO> GetKDSOrderLineDTOList(List<int> displayBatchIdSet, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(displayBatchIdSet, sqlTransaction);
            KDSOrderLineDataHandler kdsOrderLineDataHandler = new KDSOrderLineDataHandler(sqlTransaction);
            List<KDSOrderLineDTO> kdsOrderLineDTOList = kdsOrderLineDataHandler.GetKDSOrderLineDTOList(displayBatchIdSet);
            log.LogMethodExit(kdsOrderLineDTOList);
            return kdsOrderLineDTOList;
        }
    }
}

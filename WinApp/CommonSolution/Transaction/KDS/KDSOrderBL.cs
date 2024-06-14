/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  KDSOrder
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        16-June-2019   Lakshminarayana           Created
 *2.140.0     21-Jun-2021    Fiona Lishal        Modified for Delivery Order enhancements for F&B and Urban Piper
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Business logic for KDSOrder class.
    /// </summary>
    public class KDSOrderBL
    {
        private KDSOrderDTO kdsOrderDto;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of KDSOrderBL class
        /// </summary>
        private KDSOrderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates KDSOrderBL object using the KDSOrderDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="kdsOrderDto">KDSOrderDTO object</param>
        public KDSOrderBL(ExecutionContext executionContext, KDSOrderDTO kdsOrderDto)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, kdsOrderDto);
            this.kdsOrderDto = kdsOrderDto;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the KDSOrder id as the parameter
        /// Would fetch the KDSOrder object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="transactionId">transaction id</param>
        /// <param name="displayBatchId"> id of KDSOrder passed as parameter</param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public KDSOrderBL(ExecutionContext executionContext, int transactionId, int displayBatchId, bool loadChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, displayBatchId, sqlTransaction);
            KDSOrderDataHandler kdsOrderDataHandler = new KDSOrderDataHandler(sqlTransaction);
            kdsOrderDto = kdsOrderDataHandler.GetKDSOrderDTO(transactionId, displayBatchId);
            if (kdsOrderDto == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "KDSOrder", displayBatchId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(displayBatchId, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the KDSOrder id as the parameter
        /// Would fetch the KDSOrder object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="transaction">transaction</param>
        /// <param name="transactionLines">items to be prepared in the kitchen</param>
        /// <param name="posPrinterDTO">pos printerDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <param name="isKDSOrder"></param>
        public KDSOrderBL(ExecutionContext executionContext,
                          Transaction transaction,
                          List<Transaction.TransactionLine> transactionLines,
                          POSPrinterDTO posPrinterDTO,
                          SqlTransaction sqlTransaction = null,
                          bool isKDSOrder = true)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transaction, transactionLines, sqlTransaction, isKDSOrder);
            if (transactionLines == null || transactionLines.Any() == false)
            {
                string message = "Unable to create a new KDS order with empty transaction lines.";
                log.LogMethodExit(null, "Throwing exception - " + message);
                throw new Exception(message);
            }
            int displayBatchId = CreateDisplayBatchId();
            DateTime orderedTime = GetServerDateTime();
            DateTime? scheduledDeliveryTime = ((transaction != null && transaction.TransctionOrderDispensingDTO != null) ? transaction.TransctionOrderDispensingDTO.ScheduledDispensingTime : null);
            log.Info(scheduledDeliveryTime);
            kdsOrderDto = new KDSOrderDTO(displayBatchId, posPrinterDTO.PrinterId, transaction.Trx_id, orderedTime, posPrinterDTO.PrintTemplateId, string.Empty, string.Empty, transaction.Trx_No, null, null);
            List<KDSOrderLineDTO> kdsOrderLineDtoList = new List<KDSOrderLineDTO>(transactionLines.Count);
            foreach (Transaction.TransactionLine transactionLine in transactionLines)
            {
                KDSOrderLineDTO kdsOrderLineDTO = new KDSOrderLineDTO(-1, posPrinterDTO.PrinterId,
                    transaction.Trx_id, transactionLine.DBLineId, orderedTime, null,
                    posPrinterDTO.PrintTemplateId, displayBatchId, null, null,
                    transactionLine.ProductID, string.Empty,string.Empty,  string.Empty,
                    transactionLine.quantity, transactionLine.Remarks, transactionLine.ParentLine != null ? transactionLine.ParentLine.DBLineId : -1, null,
                    scheduledDeliveryTime, (isKDSOrder ? KDSOrderLineDTO.KDSKOTEntryType.KDS: KDSOrderLineDTO.KDSKOTEntryType.KOT));
                kdsOrderLineDtoList.Add(kdsOrderLineDTO);
            }

            kdsOrderDto.KDSOrderLineDtoList = kdsOrderLineDtoList;
            log.LogMethodExit();
        }

        private static int CreateDisplayBatchId()
        {
            byte[] seed = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(seed);
            int seedInt = BitConverter.ToInt32(seed, 0);
            Random rnd = new Random(seedInt);
            int displayBatchId = rnd.Next(999999999);
            return displayBatchId;
        }

        private DateTime GetServerDateTime()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime serverDateTime = lookupValuesList.GetServerDateTime();
            log.LogMethodExit(serverDateTime);
            return serverDateTime;
        }

        /// <summary>
        /// Builds the child records for AppUIPanel object.
        /// </summary>
        /// <param name="displayBatchId">display Batch Id</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        private void Build(int displayBatchId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(displayBatchId, sqlTransaction);
            KDSOrderLineListBL kdsOrderLineListBL = new KDSOrderLineListBL(executionContext);
            List<KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>> searchParameters =
                new List<KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>(
                        KDSOrderLineDTO.SearchByParameters.TRX_ID, kdsOrderDto.TransactionId.ToString()),
                    new KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>(
                        KDSOrderLineDTO.SearchByParameters.DISPLAY_BATCH_ID, displayBatchId.ToString())
                };
            kdsOrderDto.KDSOrderLineDtoList = kdsOrderLineListBL.GetKDSOrderLineDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the kdsOrderDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (kdsOrderDto.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            foreach (KDSOrderLineDTO kdsOrderLineDto in kdsOrderDto.KDSOrderLineDtoList)
            {
                if (kdsOrderLineDto.DisplayBatchId != kdsOrderDto.DisplayBatchId)
                {
                    kdsOrderLineDto.DisplayBatchId = kdsOrderDto.DisplayBatchId;
                }
            }
            KDSOrderDataHandler kdsOrderDataHandler = new KDSOrderDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            KDSOrderLineListBL kdsOrderLineListBl = new KDSOrderLineListBL(executionContext, kdsOrderDto.KDSOrderLineDtoList);
            kdsOrderLineListBl.Save(sqlTransaction);
            kdsOrderDto = kdsOrderDataHandler.RefreshDTO(kdsOrderDto);
            kdsOrderDto.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the KDSOrderDTO  values 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (kdsOrderDto.DisplayBatchId < 0)
            {
                ValidationError validationError = new ValidationError("KDSOrder", "DisplayBatchId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Display Batch Id")));
                validationErrorList.Add(validationError);
            }
            if (kdsOrderDto.KDSOrderLineDtoList != null && kdsOrderDto.KDSOrderLineDtoList.Any())
            {
                foreach (KDSOrderLineDTO kdsOrderLineDto in kdsOrderDto.KDSOrderLineDtoList)
                {
                    KDSOrderLineBL kdsOrderLineBl = new KDSOrderLineBL(executionContext, kdsOrderLineDto);
                    validationErrorList.AddRange(kdsOrderLineBl.Validate(sqlTransaction));
                }
            }
            else
            {
                ValidationError validationError = new ValidationError("KDSOrder", "KDSOrderLineDtoList", MessageContainerList.GetMessage(executionContext, 2266));
                validationErrorList.Add(validationError);
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public KDSOrderDTO KDSOrderDTO
        {
            get
            {
                return kdsOrderDto;
            }
        }

        /// <summary>
        /// Returns whether all the order lines are prepared
        /// </summary>
        /// <returns></returns>
        public bool IsPrepared()
        {
            return kdsOrderDto.KDSOrderLineDtoList.Where(x => x.LineCancelledTime.HasValue == false).Any(x => x.PreparedTime.HasValue == false) == false;
        }

        /// <summary>
        /// Returns whether the specified the order line is prepared
        /// </summary>
        /// <returns></returns>
        public bool IsPrepared(int lineId)
        {
            log.LogMethodEntry(lineId);
            ThrowExceptionIfInvalidLineId(lineId);
            KDSOrderLineBL kdsOrderLineBL = GetKDSOrderLineBLWithLineId(lineId);
            bool result = kdsOrderLineBL.IsPrepared();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns whether all the order lines are delivered
        /// </summary>
        /// <returns></returns>
        public bool IsDelivered()
        {
            return kdsOrderDto.KDSOrderLineDtoList.Where(x => x.LineCancelledTime.HasValue == false).Any(x => x.DeliveredTime.HasValue == false) == false;
        }

        /// <summary>
        /// Returns whether the specified the order line is prepared
        /// </summary>
        /// <param name="lineId">line id</param>
        /// <returns></returns>
        public bool IsDelivered(int lineId)
        {
            log.LogMethodEntry(lineId);
            ThrowExceptionIfInvalidLineId(lineId);
            KDSOrderLineBL kdsOrderLineBL = GetKDSOrderLineBLWithLineId(lineId);
            bool result = kdsOrderLineBL.IsDelivered();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Prepares the order
        /// </summary>
        public void Prepare()
        {
            log.LogMethodEntry();
            DateTime currentServerTime = GetServerDateTime();
            if (IsCancelledOrder())
            {
                foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderDto.KDSOrderLineDtoList)
                {
                    KDSOrderLineBL kdsOrderLineBL = new KDSOrderLineBL(executionContext, kdsOrderLineDTO);
                    kdsOrderLineBL.PrepareCancelledLine(currentServerTime);
                }
                log.LogMethodExit(null, "prepared an cancelled order");
                return;
            }
            foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderDto.KDSOrderLineDtoList)
            {
                KDSOrderLineBL kdsOrderLineBL = new KDSOrderLineBL(executionContext, kdsOrderLineDTO);
                kdsOrderLineBL.Prepare(currentServerTime);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns whether the KDS order is cancelled
        /// </summary>
        /// <returns></returns>
        public bool IsCancelledOrder()
        {
            return kdsOrderDto.KDSOrderLineDtoList.Any(x => x.LineCancelledTime.HasValue == false) == false;
        }

        /// <summary>
        /// Prepares the specified order line
        /// </summary>
        /// <param name="lineId">line id</param>
        public void PrepareOrderLine(int lineId)
        {
            log.LogMethodEntry(lineId);
            ThrowExceptionIfInvalidLineId(lineId);
            DateTime currentServerTime = GetServerDateTime();
            KDSOrderLineBL kdsOrderLineBL = GetKDSOrderLineBLWithLineId(lineId);
            kdsOrderLineBL.Prepare(currentServerTime);
            log.LogMethodExit();
        }

        /// <summary>
        /// Delivers the order
        /// </summary>
        public void Deliver()
        {
            log.LogMethodEntry();
            DateTime currentServerTime = GetServerDateTime();
            if (IsCancelledOrder())
            {
                foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderDto.KDSOrderLineDtoList)
                {
                    KDSOrderLineBL kdsOrderLineBL = new KDSOrderLineBL(executionContext, kdsOrderLineDTO);
                    kdsOrderLineBL.DeliverCancelledLine(currentServerTime);
                }
                log.LogMethodExit(null, "delivered an cancelled order");
                return;
            }
            foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderDto.KDSOrderLineDtoList)
            {
                KDSOrderLineBL kdsOrderLineBL = new KDSOrderLineBL(executionContext, kdsOrderLineDTO);
                kdsOrderLineBL.Deliver(currentServerTime);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delivers the specified order line
        /// </summary>
        /// <param name="lineId">line Id</param>
        public void DeliverOrderLine(int lineId)
        {
            log.LogMethodEntry(lineId);
            ThrowExceptionIfInvalidLineId(lineId);
            DateTime currentServerTime = GetServerDateTime();
            KDSOrderLineBL kdsOrderLineBL = GetKDSOrderLineBLWithLineId(lineId);
            kdsOrderLineBL.Deliver(currentServerTime);
            log.LogMethodExit();
        }

        /// <summary>
        /// Recalls an order to kitchen for fixing
        /// </summary>
        public void RecallToKitchen()
        {
            log.LogMethodEntry();
            foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderDto.KDSOrderLineDtoList)
            {
                KDSOrderLineBL kdsOrderLineBL = new KDSOrderLineBL(executionContext, kdsOrderLineDTO);
                kdsOrderLineBL.RecallToKitchen();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Recalls the specified order line to kitchen
        /// </summary>
        /// <param name="lineId"></param>
        internal void RecallOrderLineToKitchen(int lineId)
        {
            log.LogMethodEntry(lineId);
            ThrowExceptionIfInvalidLineId(lineId);
            KDSOrderLineBL kdsOrderLineBL = GetKDSOrderLineBLWithLineId(lineId);
            kdsOrderLineBL.RecallToKitchen();
            log.LogMethodExit();
        }

        /// <summary>
        /// Recalls an delivered order to delivery station. Which may be further recalled to kitchen for fixing
        /// </summary>
        public void RecallToDeliveryStation()
        {
            log.LogMethodEntry();
            foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderDto.KDSOrderLineDtoList)
            {
                KDSOrderLineBL kdsOrderLineBL = new KDSOrderLineBL(executionContext, kdsOrderLineDTO);
                kdsOrderLineBL.RecallToDeliveryStation();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Recalls an delivered order to delivery station. Which may be further recalled to kitchen for fixing
        /// </summary>
        /// <param name="lineId">line id</param>
        public void RecallOrderLineToDeliveryStation(int lineId)
        {
            log.LogMethodEntry(lineId);
            ThrowExceptionIfInvalidLineId(lineId);
            KDSOrderLineBL kdsOrderLineBL = GetKDSOrderLineBLWithLineId(lineId);
            kdsOrderLineBL.RecallToDeliveryStation();
            log.LogMethodExit();
        }

        private KDSOrderLineBL GetKDSOrderLineBLWithLineId(int lineId)
        {
            log.LogMethodEntry(lineId);
            KDSOrderLineDTO kdsOrderLineDTO = kdsOrderDto.KDSOrderLineDtoList.FirstOrDefault(x => x.LineId == lineId);
            KDSOrderLineBL kdsOrderLineBL = new KDSOrderLineBL(executionContext, kdsOrderLineDTO);
            log.LogMethodExit(kdsOrderLineBL);
            return kdsOrderLineBL;
        }

        private void ThrowExceptionIfInvalidLineId(int lineId)
        {
            log.LogMethodEntry(lineId);
            if (kdsOrderDto.KDSOrderLineDtoList == null ||
                kdsOrderDto.KDSOrderLineDtoList.Any(x => x.LineId == lineId) == false)
            {
                string message = "Unable to find order line with lineId (" + lineId + ").";
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the group of KDS Order line group dto. grouped by product id
        /// </summary>
        /// <returns></returns>
        public List<KDSOrderLineGroupDTO> GetKDSOrderLineGroupDTOList()
        {
            log.LogMethodEntry();
            List<KDSOrderLineGroupDTO> kdsOrderLineGroupDTOList = new List<KDSOrderLineGroupDTO>();
            List<KDSOrderLineGroupBL> parentKDSOrderLineGroupBLList = new List<KDSOrderLineGroupBL>();
            Dictionary<int, KDSOrderLineGroupBL> lineIdKDSOrderLineGroupBLDictionary = new Dictionary<int, KDSOrderLineGroupBL>();
            foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderDto.KDSOrderLineDtoList)
            {
                //if (kdsOrderLineDTO.LineCancelledTime.HasValue)
                //{
                //    continue;
                //}
                if (kdsOrderLineDTO.ParentLineId >= 0)
                {
                    continue;
                }
                KDSOrderLineGroupBL kdsOrderLineGroupBL = new KDSOrderLineGroupBL(executionContext, kdsOrderLineDTO);
                parentKDSOrderLineGroupBLList.Add(kdsOrderLineGroupBL);
                if (lineIdKDSOrderLineGroupBLDictionary.ContainsKey(kdsOrderLineDTO.LineId))
                {
                    continue;
                }
                lineIdKDSOrderLineGroupBLDictionary.Add(kdsOrderLineDTO.LineId, kdsOrderLineGroupBL);
            }

            foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderDto.KDSOrderLineDtoList)
            {
                //if (kdsOrderLineDTO.LineCancelledTime.HasValue)
                //{
                //    continue;
                //}
                if (kdsOrderLineDTO.ParentLineId < 0)
                {
                    continue;
                }
                KDSOrderLineGroupBL kdsOrderLineGroupBL = new KDSOrderLineGroupBL(executionContext, kdsOrderLineDTO);
                if (lineIdKDSOrderLineGroupBLDictionary.ContainsKey(kdsOrderLineDTO.LineId))
                {
                    continue;
                }

                if (lineIdKDSOrderLineGroupBLDictionary.ContainsKey(kdsOrderLineDTO.ParentLineId))
                {
                    KDSOrderLineGroupBL parent = lineIdKDSOrderLineGroupBLDictionary[kdsOrderLineDTO.ParentLineId];
                    parent.AddChild(kdsOrderLineGroupBL);
                }
                else
                {
                    parentKDSOrderLineGroupBLList.Add(kdsOrderLineGroupBL);
                }
                lineIdKDSOrderLineGroupBLDictionary.Add(kdsOrderLineDTO.LineId, kdsOrderLineGroupBL);
            }

            List<KDSOrderLineGroupBL> mergedKDSOrderLineGroupBLList = new List<KDSOrderLineGroupBL>();
            Dictionary<string, KDSOrderLineGroupBL> lineHierarchyStringKDSOrderLineGroupBLDictionary = new Dictionary<string, KDSOrderLineGroupBL>();
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in parentKDSOrderLineGroupBLList)
            {
                kdsOrderLineGroupBL.Consolidate();
            }
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in parentKDSOrderLineGroupBLList)
            {
                string lineHierarchyString = kdsOrderLineGroupBL.GetLineHierarchyString();
                if (lineHierarchyStringKDSOrderLineGroupBLDictionary.ContainsKey(lineHierarchyString))
                {
                    lineHierarchyStringKDSOrderLineGroupBLDictionary[lineHierarchyString].Merge(kdsOrderLineGroupBL);
                    continue;
                }
                lineHierarchyStringKDSOrderLineGroupBLDictionary.Add(lineHierarchyString, kdsOrderLineGroupBL);
                mergedKDSOrderLineGroupBLList.Add(kdsOrderLineGroupBL);
            }
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in mergedKDSOrderLineGroupBLList)
            {
                kdsOrderLineGroupBL.SetProductNameOffset(0);
                kdsOrderLineGroupDTOList.AddRange(kdsOrderLineGroupBL.GetKDSOrderLineGroupList());
            }
            log.LogMethodExit(kdsOrderLineGroupDTOList);
            return kdsOrderLineGroupDTOList;
        }

        /// <summary>
        /// Returns the status of the specified order line
        /// </summary>
        /// <param name="lineId">line id</param>
        /// <returns></returns>
        public string GetOrderLineStatus(int lineId)
        {
            log.LogMethodEntry(lineId);
            ThrowExceptionIfInvalidLineId(lineId);
            KDSOrderLineBL kdsOrderLineBL = GetKDSOrderLineBLWithLineId(lineId);
            string status = kdsOrderLineBL.GetStatus();
            log.LogMethodExit(status);
            return status;
        } 
    }
    /// <summary>
    /// Manages the list of KDSOrder
    /// </summary>
    public class KDSOrderListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<KDSOrderDTO> kdsOrderDtoList = new List<KDSOrderDTO>();
        /// <summary>
        /// Parameterized constructor for KDSOrderListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public KDSOrderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for KDSOrderListBL with kdsOrderDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="kdsOrderDtoList">kdsOrderDTOList object is passed as parameter</param>
        public KDSOrderListBL(ExecutionContext executionContext,
                              List<KDSOrderDTO> kdsOrderDtoList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, kdsOrderDtoList);
            this.kdsOrderDtoList = kdsOrderDtoList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the KDSOrderDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="loadChildRecords">loadChildRecords true or false</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of KDSOrderDTO</returns>
        public List<KDSOrderDTO> GetKDSOrderDTOList(List<KeyValuePair<KDSOrderDTO.SearchByParameters, string>> searchParameters,
                                                    bool loadChildRecords = false,
                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            KDSOrderDataHandler kdsOrderDataHandler = new KDSOrderDataHandler(sqlTransaction);
            List<KDSOrderDTO> result = kdsOrderDataHandler.GetKDSOrderDTOList(searchParameters);
            if (result.Any() && loadChildRecords)
            {
                Build(result, sqlTransaction);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Builds the List of KDSOrder objects.
        /// </summary>
        /// <param name="kdsOrderDTOList">kds Order DTO List</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        private void Build(List<KDSOrderDTO> kdsOrderDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(kdsOrderDTOList, sqlTransaction);
            Dictionary<string, KDSOrderDTO> displayBatchIdKDSOrderDictionary = new Dictionary<string, KDSOrderDTO>();
            List<int> displayBatchIdSet = new List<int>();
            foreach (KDSOrderDTO kdsOrderDTO in kdsOrderDTOList)
            {
                string key = "TransactionId:" + kdsOrderDTO.TransactionId + "DisplayBatchId:" +
                             kdsOrderDTO.DisplayBatchId;
                if (kdsOrderDTO.DisplayBatchId == -1 ||
                    displayBatchIdKDSOrderDictionary.ContainsKey(key))
                {
                    continue;
                }
                displayBatchIdSet.Add(kdsOrderDTO.DisplayBatchId);
                displayBatchIdKDSOrderDictionary.Add(key, kdsOrderDTO);
            }
            KDSOrderLineListBL kdsOrderLineListBL = new KDSOrderLineListBL(executionContext);
            List<KDSOrderLineDTO> kdsOrderLineDTOList = kdsOrderLineListBL.GetKDSOrderLineDTOList(displayBatchIdSet, sqlTransaction);
            if (kdsOrderLineDTOList.Any())
            {
                log.LogVariableState("kdsOrderLineDTOList", kdsOrderLineDTOList);
                foreach (KDSOrderLineDTO kdsOrderLineDTO in kdsOrderLineDTOList)
                {
                    string key = "TransactionId:" + kdsOrderLineDTO.TrxId + "DisplayBatchId:" +
                                 kdsOrderLineDTO.DisplayBatchId;
                    if (!displayBatchIdKDSOrderDictionary.ContainsKey(key))
                        continue;
                    if (displayBatchIdKDSOrderDictionary[key].KDSOrderLineDtoList == null)
                    {
                        displayBatchIdKDSOrderDictionary[key].KDSOrderLineDtoList = new List<KDSOrderLineDTO>();
                    }
                    displayBatchIdKDSOrderDictionary[key].KDSOrderLineDtoList.Add(kdsOrderLineDTO);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  List of kdsOrderDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (kdsOrderDtoList == null ||
                kdsOrderDtoList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < kdsOrderDtoList.Count; i++)
            {
                var kdsOrderDto = kdsOrderDtoList[i];
                if (kdsOrderDto.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    KDSOrderBL kdsOrderBl = new KDSOrderBL(executionContext, kdsOrderDto);
                    kdsOrderBl.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving KDSOrderDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("KDSOrderDTO", kdsOrderDto);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - TransctionOrderDispensingBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0    02-Jun-2021    Fiona              Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DeliveryIntegration; 

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransctionOrderDispensingBL
    /// </summary>
    public class TransactionOrderDispensingBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TransactionOrderDispensingDTO transctionOrderDispensingDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private TransactionOrderDispensingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parameterTransctionOrderDispensingDTO"></param>
        /// <param name="sqlTransaction"></param>
        /// <param name="buildChildRecords"></param>
        /// <param name="buildActiveChildRecords"></param>
        public TransactionOrderDispensingBL(ExecutionContext executionContext, TransactionOrderDispensingDTO parameterTransctionOrderDispensingDTO, bool buildChildRecords = true, bool buildActiveChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterTransctionOrderDispensingDTO, sqlTransaction);

            if (parameterTransctionOrderDispensingDTO.TransactionOrderDispensingId > -1)
            {
                LoadTransctionOrderDispensingDTO(parameterTransctionOrderDispensingDTO.TransactionOrderDispensingId, buildChildRecords, buildActiveChildRecords, sqlTransaction);//added sql
                ThrowIfDTOIsNull(parameterTransctionOrderDispensingDTO.TransactionOrderDispensingId);
                Update(parameterTransctionOrderDispensingDTO);
            }
            else
            {
                ValidateNew(parameterTransctionOrderDispensingDTO, sqlTransaction);
                transctionOrderDispensingDTO = new TransactionOrderDispensingDTO(parameterTransctionOrderDispensingDTO);
                //transctionOrderDispensingDTO = new TransactionOrderDispensingDTO(-1, parameterTransctionOrderDispensingDTO.TransactionId, parameterTransctionOrderDispensingDTO.OrderDispensingTypeId, parameterTransctionOrderDispensingDTO.DeliveryChannelId,
                //                                                parameterTransctionOrderDispensingDTO.ScheduledDispensingTime, parameterTransctionOrderDispensingDTO.ReconfirmationOrder, parameterTransctionOrderDispensingDTO.ReConfirmPreparation, parameterTransctionOrderDispensingDTO.DeliveryAddressId, parameterTransctionOrderDispensingDTO.DeliveryContactId, parameterTransctionOrderDispensingDTO.IsActive);
                //if (parameterTransctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList != null && parameterTransctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any())
                //{
                //    transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
                //    foreach (TransactionDeliveryDetailsDTO parameterTransactionDeliveryDetailsDTO in parameterTransctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList)
                //    {
                //        if (parameterTransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId > -1)
                //        {
                //            string message = MessageContainerList.GetMessage(executionContext, 2196, "TransactionDeliveryDetails", parameterTransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId);
                //            log.LogMethodExit(null, "Throwing Exception - " + message);
                //            throw new EntityNotFoundException(message);
                //        }
                //        var transactionDeliveryDetailsDTO = new TransactionDeliveryDetailsDTO(-1, -1, parameterTransactionDeliveryDetailsDTO.RiderId, parameterTransactionDeliveryDetailsDTO.ExternalRiderName,
                //            parameterTransactionDeliveryDetailsDTO.RiderPhoneNumber, parameterTransactionDeliveryDetailsDTO.RiderDeliveryStatus, parameterTransactionDeliveryDetailsDTO.Remarks, parameterTransactionDeliveryDetailsDTO.ExternalSystemReference, parameterTransactionDeliveryDetailsDTO.IsActive);
                //        TransactionDeliveryDetailsBL transactionDeliveryDetailsBL = new TransactionDeliveryDetailsBL(executionContext, transactionDeliveryDetailsDTO);
                //        transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Add(transactionDeliveryDetailsBL.TransactionDeliveryDetailsDTO);
                //    }
                //}
            }
            log.LogMethodExit();
        }

        private void LoadTransctionOrderDispensingDTO(int id, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            TransactionOrderDispensingDataHandler transctionOrderDispensingDataHandler = new TransactionOrderDispensingDataHandler(sqlTransaction);
            transctionOrderDispensingDTO = transctionOrderDispensingDataHandler.GetTransactionOrderDispensingDTO(id);
            ThrowIfDTOIsNull(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            TransactionDeliveryDetailsListBL transactionDeliveryDetailsListBL = new TransactionDeliveryDetailsListBL(executionContext);
            List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList = transactionDeliveryDetailsListBL.GetTransactionDeliveryDetails(new List<int>() { transctionOrderDispensingDTO.TransactionOrderDispensingId }, activeChildRecords, sqlTransaction);
            transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = transactionDeliveryDetailsDTOList;
            log.LogMethodExit();
        }

        private void ThrowIfDTOIsNull(int id)
        {
            log.LogMethodEntry(id);
            if (transctionOrderDispensingDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TransctionOrderDispensing", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void Update(TransactionOrderDispensingDTO parameterTransctionOrderDispensingDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterTransctionOrderDispensingDTO);
            ChangeTransactionOrderDispensingId(parameterTransctionOrderDispensingDTO.TransactionOrderDispensingId);
            ChangeTransactionId(parameterTransctionOrderDispensingDTO.TransactionId);
            ChangeOrderDispensingTypeId(parameterTransctionOrderDispensingDTO.OrderDispensingTypeId);
            ChangeDeliveryChannelId(parameterTransctionOrderDispensingDTO.DeliveryChannelId);
            ChangeDeliveryAddressId(parameterTransctionOrderDispensingDTO.DeliveryAddressId);
            ChangeScheduledDispensingTime(parameterTransctionOrderDispensingDTO.ScheduledDispensingTime);
            ChangeReconfirmationOrder(parameterTransctionOrderDispensingDTO.ReconfirmationOrder);
            ChangeReConfirmPreparation(parameterTransctionOrderDispensingDTO.ReConfirmPreparation);
            ChangeDeliveryContactId(parameterTransctionOrderDispensingDTO.DeliveryContactId);
            ChangeIsActive(parameterTransctionOrderDispensingDTO.IsActive);
            ChangeExternalSystemReference(parameterTransctionOrderDispensingDTO.ExternalSystemReference);
            ChangeDeliveryChannelCustomerReferenceNo(parameterTransctionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo);
            Dictionary<int, TransactionDeliveryDetailsDTO> transactionDeliveryDetailDTODictionary = new Dictionary<int, TransactionDeliveryDetailsDTO>();
            if (transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList != null &&
               transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any())
            {
                foreach (var transactionDeliveryDetailsDTO in transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList)
                {
                    transactionDeliveryDetailDTODictionary.Add(transactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId, transactionDeliveryDetailsDTO);
                }
            }
            if (parameterTransctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList != null &&
               parameterTransctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any())
            {
                foreach (TransactionDeliveryDetailsDTO parameterTransactionDeliveryDetailsDTO in parameterTransctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList)
                {
                    if (transactionDeliveryDetailDTODictionary.ContainsKey(parameterTransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId))
                    {
                        TransactionDeliveryDetailsBL transactionDeliveryDetailsBL = new TransactionDeliveryDetailsBL(executionContext, transactionDeliveryDetailDTODictionary[parameterTransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId]);
                        transactionDeliveryDetailsBL.Update(parameterTransactionDeliveryDetailsDTO);
                    }
                    //else if(parameterTransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId >- 1)
                    //{
                    //    TransactionDeliveryDetailsBL transactionDeliveryDetailsBL = new TransactionDeliveryDetailsBL(executionContext, parameterTransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId, sqlTransaction);
                    //    if(transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList==null)
                    //    {
                    //        transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
                    //    }
                    //    transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Add(parameterTransactionDeliveryDetailsDTO);
                    //    transactionDeliveryDetailsBL.Update(parameterTransactionDeliveryDetailsDTO);
                    //}
                    else
                    {
                        TransactionDeliveryDetailsBL transactionDeliveryDetailsBL = new TransactionDeliveryDetailsBL(executionContext, parameterTransactionDeliveryDetailsDTO);
                        if (transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList == null)
                        {
                            transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
                        }
                        transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Add(transactionDeliveryDetailsBL.TransactionDeliveryDetailsDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ChangeExternalSystemReference(string externalSystemReference)
        {
            log.LogMethodEntry(externalSystemReference);
            if (transctionOrderDispensingDTO.ExternalSystemReference == externalSystemReference)
            {
                log.LogMethodExit(null, "No changes to ExternalSystemReference");
                return;
            }
            transctionOrderDispensingDTO.ExternalSystemReference = externalSystemReference;
            log.LogMethodExit();
        }
        private void ChangeDeliveryChannelCustomerReferenceNo(string deliveryChannelCustomerReferenceNo)
        {
            log.LogMethodEntry(deliveryChannelCustomerReferenceNo);
            if (transctionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo == deliveryChannelCustomerReferenceNo)
            {
                log.LogMethodExit(null, "No changes to deliveryChannelCustomerReferenceNo");
                return;
            }
            transctionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo = deliveryChannelCustomerReferenceNo;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        private void ChangeTransactionId(int transactionId)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.TransactionId == transactionId)
            {
                log.LogMethodExit(null, "No changes to TransactionId");
                return;
            }
            transctionOrderDispensingDTO.TransactionId = transactionId;
            log.LogMethodExit();
        }
        private void ChangeDeliveryContactId(int DeliveryContactId)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.DeliveryContactId == DeliveryContactId)
            {
                log.LogMethodExit(null, "No changes to DeliveryContactId");
                return;
            }
            transctionOrderDispensingDTO.DeliveryContactId = DeliveryContactId;
            log.LogMethodExit();
        }
        private void ChangeIsActive(bool IsActive)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.IsActive == IsActive)
            {
                log.LogMethodExit(null, "No changes to IsActive");
                return;
            }
            transctionOrderDispensingDTO.IsActive = IsActive;
            log.LogMethodExit();
        }

        private void ChangeTransactionOrderDispensingId(int transactionOrderDispensingId)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.TransactionOrderDispensingId == transactionOrderDispensingId)
            {
                log.LogMethodExit(null, "No changes to TransctionOrderDispensingId");
                return;
            }
            transctionOrderDispensingDTO.TransactionOrderDispensingId = transactionOrderDispensingId;
            log.LogMethodExit();
        }
        private void ChangeOrderDispensingTypeId(int OrderDispensingTypeId)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.OrderDispensingTypeId == OrderDispensingTypeId)
            {
                log.LogMethodExit(null, "No changes to TransctionOrderDispensingId");
                return;
            }
            transctionOrderDispensingDTO.OrderDispensingTypeId = OrderDispensingTypeId;
            log.LogMethodExit();
        }
        private void ChangeDeliveryChannelId(int deliveryChannelId)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.DeliveryChannelId == deliveryChannelId)
            {
                log.LogMethodExit(null, "No changes to DeliveryAddressId");
                return;
            }
            transctionOrderDispensingDTO.DeliveryChannelId = deliveryChannelId;
            log.LogMethodExit();
        }
        private void ChangeDeliveryAddressId(int deliveryAddressId)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.DeliveryAddressId == deliveryAddressId)
            {
                log.LogMethodExit(null, "No changes to DeliveryAddressId");
                return;
            }
            transctionOrderDispensingDTO.DeliveryAddressId = deliveryAddressId;
            log.LogMethodExit();
        }
        private void ChangeScheduledDispensingTime(DateTime? ScheduledDispensingTime)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.ScheduledDispensingTime == ScheduledDispensingTime)
            {
                log.LogMethodExit(null, "No changes to ScheduledDispensingTime");
                return;
            }
            transctionOrderDispensingDTO.ScheduledDispensingTime = ScheduledDispensingTime;
            log.LogMethodExit();
        }
        private void ChangeReconfirmationOrder(TransactionOrderDispensingDTO.ReConformationStatus ReconfirmationOrder)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.ReconfirmationOrder == ReconfirmationOrder)
            {
                log.LogMethodExit(null, "No changes to ReconfirmationOrder");
                return;
            }
            transctionOrderDispensingDTO.ReconfirmationOrder = ReconfirmationOrder;
            log.LogMethodExit();
        }

        private void ChangeReConfirmPreparation(TransactionOrderDispensingDTO.ReConformationStatus ReConfirmPreparation)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.ReConfirmPreparation == ReConfirmPreparation)
            {
                log.LogMethodExit(null, "No changes to ScheduledDispensingTime");
                return;
            }
            transctionOrderDispensingDTO.ReConfirmPreparation = ReConfirmPreparation;
            log.LogMethodExit();
        }
        private void ValidateNew(TransactionOrderDispensingDTO inputDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inputDTO != null && inputDTO.IsChangedRecursive)
            {
                if (inputDTO.TransactionDeliveryDetailsDTOList != null && inputDTO.TransactionDeliveryDetailsDTOList.Any())
                {
                    foreach (TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO in inputDTO.TransactionDeliveryDetailsDTOList)
                    {
                        if (transactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "TransactionDeliveryDetails", transactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                    }
                }

            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public TransactionOrderDispensingBL(ExecutionContext executionContext, int id, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            LoadTransctionOrderDispensingDTO(id, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TransactionOrderDispensingDataHandler transactionDeliveryDetailsDataHandler = new TransactionOrderDispensingDataHandler(sqlTransaction);
            if (transctionOrderDispensingDTO.TransactionOrderDispensingId < 0)
            {
                transctionOrderDispensingDTO = transactionDeliveryDetailsDataHandler.Insert(transctionOrderDispensingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                //if (!string.IsNullOrEmpty(transctionOrderDispensingDTO.Guid))
                //{
                //    AuditLog auditLog = new AuditLog(executionContext);
                //    auditLog.AuditTable("TransctionOrderDispensing", transctionOrderDispensingDTO.Guid, sqlTransaction);
                //}
                transctionOrderDispensingDTO.AcceptChanges();
            }
            else
            {
                if (transctionOrderDispensingDTO.IsChanged)
                {

                    transctionOrderDispensingDTO = transactionDeliveryDetailsDataHandler.Update(transctionOrderDispensingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    //if (!string.IsNullOrEmpty(transctionOrderDispensingDTO.Guid))
                    //{
                    //    AuditLog auditLog = new AuditLog(executionContext);
                    //    auditLog.AuditTable("TransctionOrderDispensing", transctionOrderDispensingDTO.Guid, sqlTransaction);
                    //}
                    transctionOrderDispensingDTO.AcceptChanges();
                }
            }
            if (transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList != null && transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any())
            {
                foreach (TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO in transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList)
                {
                    if (transactionDeliveryDetailsDTO.TransctionOrderDispensingId != transctionOrderDispensingDTO.TransactionOrderDispensingId)
                    {
                        transactionDeliveryDetailsDTO.TransctionOrderDispensingId = transctionOrderDispensingDTO.TransactionOrderDispensingId;
                    }
                }
                TransactionDeliveryDetailsListBL transactionDeliveryDetailsListBL = new TransactionDeliveryDetailsListBL(executionContext, transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList);
                transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = transactionDeliveryDetailsListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// AssignRider
        /// </summary>
        /// <param name="riderDTO"></param>
        /// <param name="sqlTransaction"></param>
        public void AssignRider(TransactionDeliveryDetailsDTO riderDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(riderDTO, sqlTransaction);
            if (CanAssignRider())
            {
                UnAssignExistingRider(sqlTransaction);
                if (riderDTO != null)
                {
                    riderDTO.IsActive = true;
                    if (this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList == null)
                    {
                        this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
                    }
                    if (riderDTO.TrasactionDeliveryDetailsId > -1)
                    {
                        for (int i = 0; i < this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Count; i++)
                        {
                            if (this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList[i].TrasactionDeliveryDetailsId == riderDTO.TrasactionDeliveryDetailsId)
                            {
                                this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList[i].IsActive = true;
                                this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList[i].RiderDeliveryStatus = riderDTO.RiderDeliveryStatus;
                            }
                        }
                    }
                    else
                    {
                        this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Add(riderDTO);
                    }
                    if (this.transctionOrderDispensingDTO.IsChangedRecursive)
                    {
                        Save(sqlTransaction);
                    }
                }
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 4018, MessageContainerList.GetMessage(executionContext, "Rider Assignment"));
                //"&1 is not allowed for the delivery channel"
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            log.LogMethodExit();
        }
        private bool CanAssignRider()
        {
            log.LogMethodEntry();
            bool result = true;
            try
            { 
                DeliveryChannelContainerDTO deliveryChannelDTO = GetDeliveryChannelContainerDTO(transctionOrderDispensingDTO.DeliveryChannelId);
                if (deliveryChannelDTO != null)
                {
                    result = deliveryChannelDTO.ManualRiderAssignmentAllowed;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void UnAssignExistingRider(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList != null && transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any())
            {
                List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsContainerList.GetLookupsContainerDTO(executionContext.SiteId, "RIDER_DELIVERY_STATUS").LookupValuesContainerDTOList;
                foreach (TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO in transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList)
                {
                    if (transactionDeliveryDetailsDTO.IsActive)
                    {
                        if (lookupValuesContainerDTOList.Exists(x => x.LookupValue.ToLower() == "unassigned"))
                        {
                            transactionDeliveryDetailsDTO.RiderDeliveryStatus = lookupValuesContainerDTOList.FirstOrDefault(x => x.LookupValue.ToLower() == "unassigned").LookupValueId;
                        }
                        transactionDeliveryDetailsDTO.IsActive = false;
                    }
                }
                if (transctionOrderDispensingDTO.IsChangedRecursive)
                {
                    Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private bool CanUnAssignRider()
        {
            log.LogMethodEntry();

            bool result = true;
            try
            { 
                DeliveryChannelContainerDTO deliveryChannelDTO = GetDeliveryChannelContainerDTO(transctionOrderDispensingDTO.DeliveryChannelId);
                if (deliveryChannelDTO != null)
                {
                    result = deliveryChannelDTO.ManualRiderAssignmentAllowed;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = false;
            }
            log.LogMethodExit();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="riderDTO"></param>
        /// <param name="sqlTransaction"></param>
        public void UnAssignRider(TransactionDeliveryDetailsDTO riderDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(riderDTO, sqlTransaction);
            if (CanUnAssignRider())
            {
                if (riderDTO != null)
                {
                    if (this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList == null || this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any() == false)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3058);//"Cannot Unassign Rider. Rider doesn't belong to the group"
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                    if (riderDTO.TrasactionDeliveryDetailsId == -1)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3059);//"Cannot Unassign unsaved Rider"
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                    if (this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Exists(rider =>
                       rider.TrasactionDeliveryDetailsId == riderDTO.TrasactionDeliveryDetailsId) == false)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3058); //"Cannot Unassign Rider. Rider doesnt belong to the group"
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                    List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsContainerList.GetLookupsContainerDTO(executionContext.SiteId, "RIDER_DELIVERY_STATUS").LookupValuesContainerDTOList;
                    for (int i = 0; i < this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Count; i++)
                    {
                        if (this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList[i].TrasactionDeliveryDetailsId == riderDTO.TrasactionDeliveryDetailsId)
                        {

                            if (lookupValuesContainerDTOList.Exists(x => x.LookupValue.ToLower() == "unassigned"))
                            {
                                this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList[i].RiderDeliveryStatus = lookupValuesContainerDTOList.FirstOrDefault(x => x.LookupValue.ToLower() == "unassigned").LookupValueId;
                            }
                            this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList[i].IsActive = false;
                        }
                    }
                    if (this.transctionOrderDispensingDTO.IsChangedRecursive)
                    {
                        Save(sqlTransaction);
                    }
                }
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 4018, MessageContainerList.GetMessage(executionContext, "Rider UnAssignment"));
                //"&1 is not allowed for the delivery channel"
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="riderDTOList"></param>
        /// <param name="sqlTransaction"></param>
        public void SaveRiderDeliveryStatus(List<TransactionDeliveryDetailsDTO> riderDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            CanSaveRiderDeliveryStatus(riderDTOList);
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsContainerList.GetLookupsContainerDTO(executionContext.SiteId, "RIDER_DELIVERY_STATUS", executionContext).LookupValuesContainerDTOList;
            List<int> validRiderDeliveryStatus = new List<int>();
            foreach (LookupValuesContainerDTO lookupValuesContainerDTO in lookupValuesContainerDTOList)
            {
                validRiderDeliveryStatus.Add(lookupValuesContainerDTO.LookupValueId);
            }
            foreach (TransactionDeliveryDetailsDTO riderDTO in riderDTOList)
            {
                TransactionDeliveryDetailsDTO childTransactionDeliveryDetailsDTO = this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Find(x => x.IsActive
                                                                                                          && x.TrasactionDeliveryDetailsId == riderDTO.TrasactionDeliveryDetailsId
                                                                                                          && x.TrasactionDeliveryDetailsId > -1);
                if (childTransactionDeliveryDetailsDTO != null && childTransactionDeliveryDetailsDTO.RiderDeliveryStatus != riderDTO.RiderDeliveryStatus)
                {
                    if (validRiderDeliveryStatus.Contains(riderDTO.RiderDeliveryStatus) == false)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3034);//"Invalid Rider Delivery Status"
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                    childTransactionDeliveryDetailsDTO.RiderDeliveryStatus = riderDTO.RiderDeliveryStatus;
                    //TransactionDeliveryDetailsBL transactionDeliveryDetailsBL = new TransactionDeliveryDetailsBL(executionContext, childTransactionDeliveryDetailsDTO);
                    //transactionDeliveryDetailsBL.Save(sqlTransaction);
                }
            }
            if (this.transctionOrderDispensingDTO.IsChangedRecursive)
            {
                Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void CanSaveRiderDeliveryStatus(List<TransactionDeliveryDetailsDTO> riderDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(riderDTOList, sqlTransaction);
            if (riderDTOList != null || riderDTOList.Any(x => x.IsChanged == true))
            { 
                DeliveryChannelContainerDTO channelContainerDTO = GetDeliveryChannelContainerDTO(transctionOrderDispensingDTO.DeliveryChannelId);
                if (channelContainerDTO != null && channelContainerDTO.ManualRiderAssignmentAllowed == false)
                {
                    //string message = MessageContainerList.GetMessage(executionContext, "Cannot Update Rider status. Delivery Channel does not allow");
                    string message = MessageContainerList.GetMessage(executionContext, 4018, MessageContainerList.GetMessage(executionContext, "Rider status Update"));
                    //"&1 is not allowed for the delivery channel"
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ValidationException(message);
                }

                foreach (TransactionDeliveryDetailsDTO riderDTO in riderDTOList.Where(x => x.IsChanged == true))
                {
                    if (riderDTO.TrasactionDeliveryDetailsId == -1)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3035);// "Transaction Delivery Details not saved"
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                    if (transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Exists(x => x.TrasactionDeliveryDetailsId == riderDTO.TrasactionDeliveryDetailsId) == false)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3038);//"Entry not found"
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }

                }

            }
            log.LogMethodExit();
        }
         
        private DeliveryChannelContainerDTO GetDeliveryChannelContainerDTO(int channelId)
        {
            log.LogMethodEntry(channelId);
            DeliveryChannelContainerDTO deliveryChannelDTO = null;
            OnlineOrderDeliveryIntegrationContainer integrationContainer = new OnlineOrderDeliveryIntegrationContainer(executionContext.GetSiteId());
            OnlineOrderDeliveryIntegrationContainerDTOCollection containerDTOCollection = integrationContainer.GetOnlineOrderDeliveryIntegrationContainerDTOCollection();
            if (containerDTOCollection != null && containerDTOCollection.OnlineOrderDeliveryIntegrationContainerDTOList != null
                && containerDTOCollection.OnlineOrderDeliveryIntegrationContainerDTOList.Any())
            {
                OnlineOrderDeliveryIntegrationContainerDTO containerDTO = containerDTOCollection.OnlineOrderDeliveryIntegrationContainerDTOList.Find(c => c.IntegrationName == DeliveryIntegrations.UrbanPiper.ToString());
                if (containerDTO != null && containerDTO.DeliveryChannelContainerDTOList != null)
                {
                    deliveryChannelDTO = containerDTO.DeliveryChannelContainerDTOList.Find(c => c.DeliveryChannelId == channelId);
                }
                else
                {
                    log.Error("Unable to fetch OnlineOrderDeliveryIntegrationContainerDTO for "
                        + DeliveryIntegrations.UrbanPiper.ToString());
                }
            }
            else
            {
                log.Error("Unable to fetch OnlineOrderDeliveryIntegrationContainerDTOCollection");
            }
            log.LogMethodExit(deliveryChannelDTO);
            return deliveryChannelDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="riderDTOList"></param>
        /// <param name="sqlTransaction"></param>
        public void SaveRiderAssignmentRemarks(List<TransactionDeliveryDetailsDTO> riderDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(riderDTOList);
            CanSaveRiderAssignmentRemarks(riderDTOList);

            foreach (TransactionDeliveryDetailsDTO riderDTO in riderDTOList)
            {
                TransactionDeliveryDetailsDTO childTransactionDeliveryDetailsDTO = this.transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Find(x => x.IsActive
                                                                                                          && x.TrasactionDeliveryDetailsId == riderDTO.TrasactionDeliveryDetailsId
                                                                                                          && x.TrasactionDeliveryDetailsId > -1);
                if (childTransactionDeliveryDetailsDTO != null && childTransactionDeliveryDetailsDTO.Remarks != riderDTO.Remarks)
                {
                    childTransactionDeliveryDetailsDTO.Remarks = riderDTO.Remarks;
                    //TransactionDeliveryDetailsBL transactionDeliveryDetailsBL = new TransactionDeliveryDetailsBL(executionContext, riderDTO);
                    //transactionDeliveryDetailsBL.Save(sqlTransaction);
                }
            }
            if (this.transctionOrderDispensingDTO.IsChangedRecursive)
            {
                Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="riderDTOList"></param>
        private void CanSaveRiderAssignmentRemarks(List<TransactionDeliveryDetailsDTO> riderDTOList)
        {
            log.LogMethodEntry(riderDTOList);
            if (riderDTOList != null || riderDTOList.Any(x => x.IsChanged == true))
            {
                foreach (TransactionDeliveryDetailsDTO riderDTO in riderDTOList.Where(x => x.IsChanged == true))
                {
                    if (riderDTO.TrasactionDeliveryDetailsId == -1)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3037);//"Transaction Delivery Details not saved"
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                    //var dtolist = transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Where(x => x.TrasactionDeliveryDetailsId != riderDTO.TrasactionDeliveryDetailsId);
                    if (transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Exists(x => x.TrasactionDeliveryDetailsId == riderDTO.TrasactionDeliveryDetailsId) == false)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3038);//"Rider delivery details not found"
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                }
            }


            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void SetAsCustomerReconfirmedOrder(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.ReconfirmationOrder == TransactionOrderDispensingDTO.ReConformationStatus.YES)
            {
                CanMarkAsReconfirmedOrder();
                transctionOrderDispensingDTO.ReconfirmationOrder = TransactionOrderDispensingDTO.ReConformationStatus.CONFIRMED;
                Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void CanMarkAsReconfirmedOrder()
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.ReconfirmationOrder == TransactionOrderDispensingDTO.ReConformationStatus.CONFIRMED)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3039);//Order Already Confirmed
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void SetAsPreparationReconfirmedOrder(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (transctionOrderDispensingDTO.ReConfirmPreparation == TransactionOrderDispensingDTO.ReConformationStatus.YES)
            {
                CanMarkAsPreparationReconfirmedOrder();
                transctionOrderDispensingDTO.ReConfirmPreparation = TransactionOrderDispensingDTO.ReConformationStatus.CONFIRMED;
                Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void CanMarkAsPreparationReconfirmedOrder()
        {
            log.LogMethodEntry();
            if (transctionOrderDispensingDTO.ReConfirmPreparation == TransactionOrderDispensingDTO.ReConformationStatus.CONFIRMED)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3040);//Preparation is already Confirmed
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionOrderDispensingDTO TransctionOrderDispensingDTO
        {
            get
            {
                TransactionOrderDispensingDTO result = new TransactionOrderDispensingDTO(transctionOrderDispensingDTO);
                return result;
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class TransactionOrderDispensingListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<TransactionOrderDispensingDTO> transctionOrderDispensingDTOList;
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TransactionOrderDispensingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.transctionOrderDispensingDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transctionOrderDispensingDTOList"></param>
        public TransactionOrderDispensingListBL(ExecutionContext executionContext, List<TransactionOrderDispensingDTO> transctionOrderDispensingDTOList)
        {
            log.LogMethodEntry(executionContext, transctionOrderDispensingDTOList);
            this.transctionOrderDispensingDTOList = transctionOrderDispensingDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="loadActiveChild"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<TransactionOrderDispensingDTO> GetTransctionOrderDispensing(List<KeyValuePair<TransactionOrderDispensingDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChild = false,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionOrderDispensingDataHandler transctionOrderDispensingDataHandler = new TransactionOrderDispensingDataHandler(sqlTransaction);
            List<TransactionOrderDispensingDTO> result = transctionOrderDispensingDataHandler.GetTransactionOrderDispensingDTO(searchParameters);
            if (loadChildRecords && result != null && result.Any())
            {
                Build(result, loadActiveChild, sqlTransaction);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void Build(List<TransactionOrderDispensingDTO> transctionOrderDispensingDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transctionOrderDispensingDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, TransactionOrderDispensingDTO> transctionOrderDispensingDTOIdMap = new Dictionary<int, TransactionOrderDispensingDTO>();
            List<int> transctionOrderDispensingIdList = new List<int>();
            for (int i = 0; i < transctionOrderDispensingDTOList.Count; i++)
            {
                if (transctionOrderDispensingDTOIdMap.ContainsKey(transctionOrderDispensingDTOList[i].TransactionOrderDispensingId))
                {
                    continue;
                }
                transctionOrderDispensingDTOIdMap.Add(transctionOrderDispensingDTOList[i].TransactionOrderDispensingId, transctionOrderDispensingDTOList[i]);
                transctionOrderDispensingIdList.Add(transctionOrderDispensingDTOList[i].TransactionOrderDispensingId);
            }

            TransactionDeliveryDetailsListBL transactionDeliveryDetailsListBL = new TransactionDeliveryDetailsListBL(executionContext);
            List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList = transactionDeliveryDetailsListBL.GetTransactionDeliveryDetails(transctionOrderDispensingIdList, activeChildRecords, sqlTransaction);
            if (transactionDeliveryDetailsDTOList != null && transactionDeliveryDetailsDTOList.Any())
            {
                for (int i = 0; i < transactionDeliveryDetailsDTOList.Count; i++)
                {
                    if (transctionOrderDispensingDTOIdMap.ContainsKey(transactionDeliveryDetailsDTOList[i].TransctionOrderDispensingId) == false)
                    {
                        continue;
                    }
                    TransactionOrderDispensingDTO transctionOrderDispensingDTO = transctionOrderDispensingDTOIdMap[transactionDeliveryDetailsDTOList[i].TransctionOrderDispensingId];
                    if (transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList == null)
                    {
                        transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
                    }
                    transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Add(transactionDeliveryDetailsDTOList[i]);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TransactionOrderDispensingDTO> Save()
        {
            log.LogMethodEntry();
            List<TransactionOrderDispensingDTO> savedTransctionOrderDispensingDTO = new List<TransactionOrderDispensingDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (transctionOrderDispensingDTOList != null && transctionOrderDispensingDTOList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (TransactionOrderDispensingDTO transctionOrderDispensingDTO in transctionOrderDispensingDTOList)
                        {
                            TransactionOrderDispensingBL transactionDeliveryDetails = new TransactionOrderDispensingBL(executionContext, transctionOrderDispensingDTO);
                            transactionDeliveryDetails.Save(parafaitDBTrx.SQLTrx);
                            savedTransctionOrderDispensingDTO.Add(transactionDeliveryDetails.TransctionOrderDispensingDTO);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }
            }
            log.LogMethodExit(savedTransctionOrderDispensingDTO);
            return savedTransctionOrderDispensingDTO;
        }
    }
}

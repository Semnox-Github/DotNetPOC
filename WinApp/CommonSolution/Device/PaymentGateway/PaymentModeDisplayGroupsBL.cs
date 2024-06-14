/********************************************************************************************
 * Project Name - PaymentModeDisplayGroupsBL                                                                       
 * Description  - PaymentModeDisplayGroupsBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.150.1    26-Jan-2023   Guru S A              Created 
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModeDisplayGroupsBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO;
        private ExecutionContext executionContext;
        private PaymentModeDisplayGroupsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor
        /// </summary> 
        public PaymentModeDisplayGroupsBL(ExecutionContext executionContext, PaymentModeDisplayGroupsDTO parameterDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterDTO, sqlTransaction);
            if (parameterDTO.PaymentModeDisplayGroupId > -1)
            {
                LoadPaymentModeDisplayGroupDTO(parameterDTO.PaymentModeDisplayGroupId, sqlTransaction);
                ThrowIfDTOIsNull(parameterDTO.PaymentModeDisplayGroupId);
                Update(parameterDTO);
            }
            else
            {
                //Validate(parameterPaymentModeDisplayGroupsDTO, sqlTransaction);
                paymentModeDisplayGroupsDTO = new PaymentModeDisplayGroupsDTO(parameterDTO);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor
        /// </summary> 
        public PaymentModeDisplayGroupsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
         : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadPaymentModeDisplayGroupDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        private void Validate(PaymentModeDisplayGroupsDTO inputDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (inputDTO != null && inputDTO.IsChanged)
            {
                if (inputDTO.PaymentModeId == -1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 249, (MessageContainerList.GetMessage(executionContext, "Payment Mode"))));
                    //&1 is mandatory. Please enter a value.
                }
                if (inputDTO.ProductDisplayGroupId == -1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 249, (MessageContainerList.GetMessage(executionContext, "Product Display Group"))));
                    //&1 is mandatory. Please enter a value.
                }
            }
            log.LogMethodExit();
        }

        private void ThrowIfDTOIsNull(int Id)
        {
            log.LogMethodEntry(Id);
            if (paymentModeDisplayGroupsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PaymentModeDisplayGroups", Id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadPaymentModeDisplayGroupDTO(int paymentModeDisplayGroupId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(paymentModeDisplayGroupId, sqlTransaction);
            PaymentModeDisplayGroupsDataHandler paymentModeDisplayGroupsDataHandler = new PaymentModeDisplayGroupsDataHandler(sqlTransaction);
            paymentModeDisplayGroupsDTO = paymentModeDisplayGroupsDataHandler.GetPaymentModeDisplayGroupsDTO(paymentModeDisplayGroupId);
            ThrowIfDTOIsNull(paymentModeDisplayGroupId);
            log.LogMethodExit();
        }
        private void Update(PaymentModeDisplayGroupsDTO parameterDTO)
        {
            log.LogMethodEntry(parameterDTO);
            ChangePaymentModeId(parameterDTO.PaymentModeId);
            ChangeProductDisplayGroupId(parameterDTO.ProductDisplayGroupId);
            ChangeIsActive(parameterDTO.IsActive);  
            log.LogMethodExit();
        } 
        private void ChangePaymentModeId(int paymentModeId)
        {
            log.LogMethodEntry(paymentModeId);
            if (paymentModeDisplayGroupsDTO.PaymentModeId == paymentModeId)
            {
                log.LogMethodExit(null, "No changes to PaymentModeId");
                return;
            }
            paymentModeDisplayGroupsDTO.PaymentModeId = paymentModeId;
            log.LogMethodExit();
        }
        private void ChangeProductDisplayGroupId(int productDisplayGroupId)
        {
            log.LogMethodEntry(productDisplayGroupId);
            if (paymentModeDisplayGroupsDTO.ProductDisplayGroupId == productDisplayGroupId)
            {
                log.LogMethodExit(null, "No changes to ProductDisplayGroupId");
                return;
            }
            paymentModeDisplayGroupsDTO.ProductDisplayGroupId = productDisplayGroupId;
            log.LogMethodExit();
        }
        private void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (paymentModeDisplayGroupsDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to isActive");
                return;
            }
            paymentModeDisplayGroupsDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(this.paymentModeDisplayGroupsDTO, sqlTransaction);
            PaymentModeDisplayGroupsDataHandler paymentModeDisplayGroupsDataHandler = new PaymentModeDisplayGroupsDataHandler(sqlTransaction);
            if (paymentModeDisplayGroupsDTO.PaymentModeDisplayGroupId < 0)
            {
                paymentModeDisplayGroupsDTO = paymentModeDisplayGroupsDataHandler.Insert(paymentModeDisplayGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(paymentModeDisplayGroupsDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("PaymentModeDisplayGroups", paymentModeDisplayGroupsDTO.Guid, sqlTransaction);
                }
                paymentModeDisplayGroupsDTO.AcceptChanges();
            }
            else
            {
                if (paymentModeDisplayGroupsDTO.IsChanged)
                {
                    paymentModeDisplayGroupsDTO = paymentModeDisplayGroupsDataHandler.Update(paymentModeDisplayGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(paymentModeDisplayGroupsDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("PaymentModeDisplayGroups", paymentModeDisplayGroupsDTO.Guid, sqlTransaction);
                    }
                    paymentModeDisplayGroupsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        public PaymentModeDisplayGroupsDTO PaymentModeDisplayGroupsDTO
        {
            get
            {
                PaymentModeDisplayGroupsDTO result = new PaymentModeDisplayGroupsDTO(paymentModeDisplayGroupsDTO);
                return result;
            }
        }
    }
    /// <summary>
    /// PaymentModeDisplayGroupsListBL
    /// </summary>
    public class PaymentModeDisplayGroupsListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList;
        public PaymentModeDisplayGroupsListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary> 
        public PaymentModeDisplayGroupsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.paymentModeDisplayGroupsDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary> 
        public PaymentModeDisplayGroupsListBL(ExecutionContext executionContext, List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList)
        {
            log.LogMethodEntry(executionContext, paymentModeDisplayGroupsDTOList);
            this.paymentModeDisplayGroupsDTOList = paymentModeDisplayGroupsDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetPaymentModeDisplayGroups
        /// </summary> 
        public List<PaymentModeDisplayGroupsDTO> GetPaymentModeDisplayGroups(List<KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PaymentModeDisplayGroupsDataHandler paymentModeDisplayGroupsDataHandler = new PaymentModeDisplayGroupsDataHandler(sqlTransaction);
            List<PaymentModeDisplayGroupsDTO> result = paymentModeDisplayGroupsDataHandler.GetPaymentModeDisplayGroupsDTOList(searchParameters);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Save
        /// </summary> 
        public List<PaymentModeDisplayGroupsDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<PaymentModeDisplayGroupsDTO> savedPaymentModeDisplayGroupsDTO = new List<PaymentModeDisplayGroupsDTO>();
            {
                try
                {
                    if (paymentModeDisplayGroupsDTOList != null && paymentModeDisplayGroupsDTOList.Any())
                    {
                        foreach (PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO in paymentModeDisplayGroupsDTOList)
                        {
                            PaymentModeDisplayGroupsBL paymentModeDisplayGroups = new PaymentModeDisplayGroupsBL(executionContext, paymentModeDisplayGroupsDTO);
                            paymentModeDisplayGroups.Save(sqlTransaction);
                            savedPaymentModeDisplayGroupsDTO.Add(paymentModeDisplayGroups.PaymentModeDisplayGroupsDTO);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }
            }
            log.LogMethodExit(savedPaymentModeDisplayGroupsDTO);
            return savedPaymentModeDisplayGroupsDTO;
        }
        /// <summary>
        /// GetPaymentModeDisplayGroups
        /// </summary> 
        public List<PaymentModeDisplayGroupsDTO> GetPaymentModeDisplayGroups(List<int> paymentModeDisplayGroupsIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(paymentModeDisplayGroupsIdList, activeChildRecords, sqlTransaction);
            PaymentModeDisplayGroupsDataHandler paymentModeDisplayGroupsDataHandler = new PaymentModeDisplayGroupsDataHandler(sqlTransaction);
            List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList = paymentModeDisplayGroupsDataHandler.GetPaymentModeDisplayGroupsDTOList(paymentModeDisplayGroupsIdList, activeChildRecords);
            log.LogMethodExit(paymentModeDisplayGroupsDTOList);
            return paymentModeDisplayGroupsDTOList;
        }

        /// <summary>
        /// GetPaymentModeDisplayGroupsByPaymentModeId
        /// </summary> 
        public List<PaymentModeDisplayGroupsDTO> GetPaymentModeDisplayGroupsByPaymentModeIdList(List<int> paymentModeIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(paymentModeIdList, activeChildRecords, sqlTransaction);
            PaymentModeDisplayGroupsDataHandler paymentModeDisplayGroupsDataHandler = new PaymentModeDisplayGroupsDataHandler(sqlTransaction);
            List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList = paymentModeDisplayGroupsDataHandler.GetPaymentModeDisplayGroupsDTOListByPaymentModeIdList(paymentModeIdList, activeChildRecords);
            log.LogMethodExit(paymentModeDisplayGroupsDTOList);
            return paymentModeDisplayGroupsDTOList;
        } 
        /// <summary>
        /// GetPaymentModeDisplayGroupsModuleLastUpdateTime
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DateTime? GetPaymentModeDisplayGroupsModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            PaymentModeDisplayGroupsDataHandler enabledAttributessDataHandler = new PaymentModeDisplayGroupsDataHandler();
            DateTime? result = enabledAttributessDataHandler.GetPaymentModeDisplayGroupsLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }
}

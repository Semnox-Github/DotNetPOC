/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - TransactionDeliveryDetailsBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.140.0      02-Jun-2021  Fiona              Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionDeliveryDetailsBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO;
        private ExecutionContext executionContext;
        private readonly string passPhrase;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private TransactionDeliveryDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(), "USERS_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parametertransactionDeliveryDetailsDTO">parameterDeliveryChannelDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public TransactionDeliveryDetailsBL(ExecutionContext executionContext, TransactionDeliveryDetailsDTO parametertransactionDeliveryDetailsDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parametertransactionDeliveryDetailsDTO, sqlTransaction);

            if (parametertransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId > -1)
            {
                LoadTransactionDeliveryDetailsDTO(parametertransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId, sqlTransaction);//added sql
                ThrowIfDTOIsNull(parametertransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId);
                Update(parametertransactionDeliveryDetailsDTO);
            }
            else
            {
                Validate(parametertransactionDeliveryDetailsDTO, sqlTransaction);
                transactionDeliveryDetailsDTO = new TransactionDeliveryDetailsDTO(-1, parametertransactionDeliveryDetailsDTO.TransctionOrderDispensingId, parametertransactionDeliveryDetailsDTO.RiderId, parametertransactionDeliveryDetailsDTO.ExternalRiderName,
                                                                 parametertransactionDeliveryDetailsDTO.RiderPhoneNumber, parametertransactionDeliveryDetailsDTO.RiderDeliveryStatus, parametertransactionDeliveryDetailsDTO.Remarks, parametertransactionDeliveryDetailsDTO.ExternalSystemReference, parametertransactionDeliveryDetailsDTO.IsActive);
            }
            log.LogMethodExit();
        }
        private void LoadTransactionDeliveryDetailsDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            TransactionDeliveryDetailsDataHandler transactionDeliveryDetailsDataHandler = new TransactionDeliveryDetailsDataHandler(passPhrase,sqlTransaction);
            transactionDeliveryDetailsDTO = transactionDeliveryDetailsDataHandler.GetTrasactionDeliveryDetailsDTO(id);
            ThrowIfDTOIsNull(id);
            log.LogMethodExit();
        }
        private void ThrowIfDTOIsNull(int id)
        {
            log.LogMethodEntry(id);
            if (transactionDeliveryDetailsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TransactionDeliveryDetails", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        internal void Update(TransactionDeliveryDetailsDTO parameterTransactionDeliveryDetailsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterTransactionDeliveryDetailsDTO);
            ChangeTrasactionDeliveryDetailsId(parameterTransactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId);
            ChangeTransctionOrderDispensingId(parameterTransactionDeliveryDetailsDTO.TransctionOrderDispensingId);
            ChangeRiderId(parameterTransactionDeliveryDetailsDTO.RiderId);
            ChangeExternalRiderName(parameterTransactionDeliveryDetailsDTO.ExternalRiderName);
            ChangeRiderPhoneNumber(parameterTransactionDeliveryDetailsDTO.RiderPhoneNumber);
            ChangeRiderDeliveryStatus(parameterTransactionDeliveryDetailsDTO.RiderDeliveryStatus);
            ChangeRemarks(parameterTransactionDeliveryDetailsDTO.Remarks);
            ChangeExternalSystemReference(parameterTransactionDeliveryDetailsDTO.ExternalSystemReference);
            ChangeIsActive(parameterTransactionDeliveryDetailsDTO.IsActive);
            log.LogMethodExit();
        }

        private void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (transactionDeliveryDetailsDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to isActive");
                return;
            }
            transactionDeliveryDetailsDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ChangeExternalSystemReference(string externalSystemReference)
        {
            log.LogMethodEntry(externalSystemReference);
            if (transactionDeliveryDetailsDTO.ExternalSystemReference == externalSystemReference)
            {
                log.LogMethodExit(null, "No changes to Remarks");
                return;
            }
            transactionDeliveryDetailsDTO.ExternalSystemReference = externalSystemReference;
            log.LogMethodExit();
        }

        private void ChangeRemarks(string remarks)
        {
            log.LogMethodEntry(remarks);
            if (transactionDeliveryDetailsDTO.Remarks == remarks)
            {
                log.LogMethodExit(null, "No changes to Remarks");
                return;
            }
            transactionDeliveryDetailsDTO.Remarks = remarks;
            log.LogMethodExit();
        }

        private void ChangeRiderDeliveryStatus(int riderDeliveryStatus)
        {
            log.LogMethodEntry(riderDeliveryStatus);
            if (transactionDeliveryDetailsDTO.RiderDeliveryStatus == riderDeliveryStatus)
            {
                log.LogMethodExit(null, "No changes to RiderDeliveryStatus");
                return;
            }
            transactionDeliveryDetailsDTO.RiderDeliveryStatus = riderDeliveryStatus;
            log.LogMethodExit();
        }

        private void ChangeRiderPhoneNumber(string riderPhoneNumber)
        {
            log.LogMethodEntry(riderPhoneNumber);
            if (transactionDeliveryDetailsDTO.RiderPhoneNumber == riderPhoneNumber)
            {
                log.LogMethodExit(null, "No changes to RiderPhoneNumber");
                return;
            }
            transactionDeliveryDetailsDTO.RiderPhoneNumber = riderPhoneNumber;
            log.LogMethodExit();
        }

        private void ChangeExternalRiderName(string externalRiderName)
        {
            log.LogMethodEntry(externalRiderName);
            if (transactionDeliveryDetailsDTO.ExternalRiderName == externalRiderName)
            {
                log.LogMethodExit(null, "No changes to externalRiderName");
                return;
            }
            transactionDeliveryDetailsDTO.ExternalRiderName = externalRiderName;
            log.LogMethodExit();
        }

        private void ChangeRiderId(int riderId)
        {
            log.LogMethodEntry(riderId);
            if (transactionDeliveryDetailsDTO.RiderId == riderId)
            {
                log.LogMethodExit(null, "No changes to riderId");
                return;
            }
            transactionDeliveryDetailsDTO.RiderId = riderId;
            log.LogMethodExit();
        }

        private void ChangeTransctionOrderDispensingId(int transctionOrderDispensingId)
        {
            log.LogMethodEntry(transctionOrderDispensingId);
            if (transactionDeliveryDetailsDTO.TransctionOrderDispensingId == transctionOrderDispensingId)
            {
                log.LogMethodExit(null, "No changes to TransctionOrderDispensingId");
                return;
            }
            transactionDeliveryDetailsDTO.TransctionOrderDispensingId = transctionOrderDispensingId;
            log.LogMethodExit();
        }

        private void ChangeTrasactionDeliveryDetailsId(int trasactionDeliveryDetailsId)
        {
            log.LogMethodEntry(transactionDeliveryDetailsDTO);
            if (transactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId == trasactionDeliveryDetailsId)
            {
                log.LogMethodExit(null, "No changes to TrasactionDeliveryDetailsId");
                return;
            }
            transactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId = trasactionDeliveryDetailsId;
            log.LogMethodExit();
        }

        private void Validate(TransactionDeliveryDetailsDTO inputDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inputDTO, sqlTransaction);
            if (inputDTO != null && inputDTO.IsChanged)
            {
                // Validation code here 
               // return validation exceptions
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the User Pass Phrase
        /// </summary>
        //private static string GetUsersPassPhrase()
        //{
        //    log.LogMethodEntry();
        //    string passPhrase;
        //    //if (SiteContainerList.IsCorporate())
        //    //{
        //    //    passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(SiteContainerList.GetMasterSiteId(), "USERS_ENCRYPTION_PASS_PHRASE");
        //    //}
        //    //else
        //    //{
        //    //    passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(-1, "USERS_ENCRYPTION_PASS_PHRASE");
        //    //}
        //    passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(), "USERS_ENCRYPTION_PASS_PHRASE");
        //    log.LogMethodExit("passPhrase");
        //    return passPhrase;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public TransactionDeliveryDetailsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadTransactionDeliveryDetailsDTO(id, sqlTransaction);
            log.LogMethodExit();
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(this.transactionDeliveryDetailsDTO, sqlTransaction);
            TransactionDeliveryDetailsDataHandler transactionDeliveryDetailsDataHandler = new TransactionDeliveryDetailsDataHandler(passPhrase,sqlTransaction);
            if (transactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId < 0)
            {
                transactionDeliveryDetailsDTO = transactionDeliveryDetailsDataHandler.Insert(transactionDeliveryDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                //if (!string.IsNullOrEmpty(transactionDeliveryDetailsDTO.Guid))
                //{
                //    AuditLog auditLog = new AuditLog(executionContext);
                //    auditLog.AuditTable("TransactionDeliveryDetails", transactionDeliveryDetailsDTO.Guid, sqlTransaction);
                //}
                transactionDeliveryDetailsDTO.AcceptChanges();
            }
            else
            {
                if (transactionDeliveryDetailsDTO.IsChanged)
                {
                    transactionDeliveryDetailsDTO = transactionDeliveryDetailsDataHandler.Update(transactionDeliveryDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    //if (!string.IsNullOrEmpty(transactionDeliveryDetailsDTO.Guid))
                    //{
                    //    AuditLog auditLog = new AuditLog(executionContext);
                    //    auditLog.AuditTable("TransactionDeliveryDetails", transactionDeliveryDetailsDTO.Guid, sqlTransaction);
                    //}
                    transactionDeliveryDetailsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionDeliveryDetailsDTO TransactionDeliveryDetailsDTO
        {
            get
            {
                TransactionDeliveryDetailsDTO result = new TransactionDeliveryDetailsDTO(transactionDeliveryDetailsDTO);
                return result;
            }
        }
    }
    /// <summary>
    /// TransactionDeliveryDetailsListBL
    /// </summary>
    public class TransactionDeliveryDetailsListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList;
        private string passPhrase = string.Empty;
        /// <summary>
        /// default constructor
        /// </summary>
        public TransactionDeliveryDetailsListBL()
        {
            log.LogMethodEntry();
            this.passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(), "USERS_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TransactionDeliveryDetailsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.transactionDeliveryDetailsDTOList = null;
            this.passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(), "USERS_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        public TransactionDeliveryDetailsListBL(ExecutionContext executionContext, List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList)
        {
            log.LogMethodEntry(executionContext, transactionDeliveryDetailsDTOList);
            this.transactionDeliveryDetailsDTOList = transactionDeliveryDetailsDTOList;
            this.executionContext = executionContext;
            this.passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(), "USERS_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }
        /// <summary>
        /// GetTransactionDeliveryDetails
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<TransactionDeliveryDetailsDTO> GetTransactionDeliveryDetails(List<KeyValuePair<TransactionDeliveryDetailsDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction); 
            TransactionDeliveryDetailsDataHandler transactionDeliveryDetailsDataHandler = new TransactionDeliveryDetailsDataHandler(passPhrase, sqlTransaction);
            List<TransactionDeliveryDetailsDTO> result = transactionDeliveryDetailsDataHandler.GetTrasactionDeliveryDetailsDTOList(searchParameters);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TransactionDeliveryDetailsDTO> Save(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry();
            List<TransactionDeliveryDetailsDTO> savedTransactionDeliveryDetailsDTO = new List<TransactionDeliveryDetailsDTO>();
            {
                try
                {
                    if (transactionDeliveryDetailsDTOList != null && transactionDeliveryDetailsDTOList.Any())
                    {
                        
                        foreach (TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO in transactionDeliveryDetailsDTOList)
                        {
                            TransactionDeliveryDetailsBL transactionDeliveryDetails = new TransactionDeliveryDetailsBL(executionContext, transactionDeliveryDetailsDTO);
                            transactionDeliveryDetails.Save(sqlTransaction);
                            savedTransactionDeliveryDetailsDTO.Add(transactionDeliveryDetails.TransactionDeliveryDetailsDTO);
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
            log.LogMethodExit(savedTransactionDeliveryDetailsDTO);
            return savedTransactionDeliveryDetailsDTO;
        }
        internal List<TransactionDeliveryDetailsDTO> GetTransactionDeliveryDetails(List<int> transctionOrderDispensingIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(transctionOrderDispensingIdList, activeChildRecords, sqlTransaction);
            TransactionDeliveryDetailsDataHandler transactionDeliveryDetailsDataHandler = new TransactionDeliveryDetailsDataHandler(passPhrase,sqlTransaction);
            List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList = transactionDeliveryDetailsDataHandler.GetTransactionDeliveryDetailsDTOList(transctionOrderDispensingIdList, activeChildRecords);
            log.LogMethodExit(transactionDeliveryDetailsDTOList);
            return transactionDeliveryDetailsDTOList;
        }
    }

}

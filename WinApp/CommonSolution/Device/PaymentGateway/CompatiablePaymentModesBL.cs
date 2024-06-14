/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - CompatiablePaymentModesBL
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By         Remarks          
 ********************************************************************************************* 
  *2.140.0    16-Aug-2021   Fiona              Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CompatiablePaymentModesBL
    {
        private CompatiablePaymentModesDTO compatiablePaymentModesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private CompatiablePaymentModesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parameterCompatiablePaymentModesDTO"></param>
        /// <param name="sqlTransaction"></param>
        public CompatiablePaymentModesBL(ExecutionContext executionContext, CompatiablePaymentModesDTO parameterCompatiablePaymentModesDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterCompatiablePaymentModesDTO, sqlTransaction);

            if (parameterCompatiablePaymentModesDTO.Id > -1)
            {
                LoadCompatiablePaymentModesDTO(parameterCompatiablePaymentModesDTO.Id, sqlTransaction);//added sql
                ThrowIfDTOIsNull(parameterCompatiablePaymentModesDTO.Id);
                Update(parameterCompatiablePaymentModesDTO);
            }
            else
            {
                Validate(parameterCompatiablePaymentModesDTO, sqlTransaction);
                compatiablePaymentModesDTO = new CompatiablePaymentModesDTO(parameterCompatiablePaymentModesDTO);
            }
            log.LogMethodExit();
        }

        private void Validate(CompatiablePaymentModesDTO inputDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (inputDTO != null && inputDTO.IsChanged)
            {
                // Validation code here 
                // return validation exceptions
            }
            log.LogMethodExit();
        }

        private void Update(CompatiablePaymentModesDTO parameterCompatiablePaymentModesDTO)
        { 
            log.LogMethodEntry(parameterCompatiablePaymentModesDTO);
            ChangeId(parameterCompatiablePaymentModesDTO.Id);
            ChangePaymentModeId(parameterCompatiablePaymentModesDTO.PaymentModeId);
            ChangeCompatiablePaymentModeId(parameterCompatiablePaymentModesDTO.CompatiablePaymentModeId);
            ChangeIdActive(parameterCompatiablePaymentModesDTO.IsActive);
            compatiablePaymentModesDTO.IsActive = parameterCompatiablePaymentModesDTO.IsActive;
            log.LogMethodExit();
        }

        private void ChangeIdActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (compatiablePaymentModesDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to isActive");
                return;
            }
            compatiablePaymentModesDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ChangeCompatiablePaymentModeId(int compatiablePaymentModeId)
        {
            log.LogMethodEntry(compatiablePaymentModeId);
            if (compatiablePaymentModesDTO.CompatiablePaymentModeId == compatiablePaymentModeId)
            {
                log.LogMethodExit(null, "No changes to compatiablePaymentModeId");
                return;
            }
            compatiablePaymentModesDTO.CompatiablePaymentModeId = compatiablePaymentModeId;
            log.LogMethodExit();
        }

        private void ChangePaymentModeId(int paymentModeId)
        {
            log.LogMethodEntry(paymentModeId);
            if (compatiablePaymentModesDTO.PaymentModeId == paymentModeId)
            {
                log.LogMethodExit(null, "No changes to paymentModeId");
                return;
            }
            compatiablePaymentModesDTO.PaymentModeId = paymentModeId;
            log.LogMethodExit();
        }

        private void ChangeId(int id)
        {
            log.LogMethodEntry(id);
            if(compatiablePaymentModesDTO.Id==id)
            {
                log.LogMethodExit(null, "No changes to id");
                return;
            }
            compatiablePaymentModesDTO.Id = id;
            log.LogMethodExit();
        }
        
        private void ThrowIfDTOIsNull(int id)
        {
            log.LogMethodEntry(id);
            if (compatiablePaymentModesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CompatiablePaymentModes", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        private void LoadCompatiablePaymentModesDTO(int id, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(id, sqlTransaction);
            CompatiablePaymentModesDataHandler compatiablePaymentModesDataHandler = new CompatiablePaymentModesDataHandler(sqlTransaction);
            compatiablePaymentModesDTO = compatiablePaymentModesDataHandler.GetCompatiablePaymentModesDTO(id);
            ThrowIfDTOIsNull(id);
            log.LogMethodExit();
        }
        /// <summary>
        /// CompatiablePaymentModesBL Contructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public CompatiablePaymentModesBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
          : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadCompatiablePaymentModesDTO(id, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(this.compatiablePaymentModesDTO, sqlTransaction);
            CompatiablePaymentModesDataHandler compatiablePaymentModesDataHandler = new CompatiablePaymentModesDataHandler(sqlTransaction);
            if (compatiablePaymentModesDTO.Id < 0)
            {
                compatiablePaymentModesDTO = compatiablePaymentModesDataHandler.Insert(compatiablePaymentModesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(compatiablePaymentModesDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("CompatiablePaymentModes", compatiablePaymentModesDTO.Guid, sqlTransaction);
                }
                compatiablePaymentModesDTO.AcceptChanges();
            }
            else
            {
                if (compatiablePaymentModesDTO.IsChanged)
                {
                    compatiablePaymentModesDTO = compatiablePaymentModesDataHandler.Update(compatiablePaymentModesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(compatiablePaymentModesDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("CompatiablePaymentModes", compatiablePaymentModesDTO.Guid, sqlTransaction);
                    }
                    compatiablePaymentModesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CompatiablePaymentModesDTO CompatiablePaymentModesDTO
        {
            get
            {
                CompatiablePaymentModesDTO result = new CompatiablePaymentModesDTO(compatiablePaymentModesDTO);
                return result;
            }
        }

    }
    public class CompatiablePaymentModesListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CompatiablePaymentModesDTO> compatiablePaymentModesDTOList;
        public CompatiablePaymentModesListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CompatiablePaymentModesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.compatiablePaymentModesDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="CompatiablePaymentModesDTOList"></param>
        public CompatiablePaymentModesListBL(ExecutionContext executionContext, List<CompatiablePaymentModesDTO> CompatiablePaymentModesDTOList)
        {
            log.LogMethodEntry(executionContext, CompatiablePaymentModesDTOList);
            this.compatiablePaymentModesDTOList = CompatiablePaymentModesDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetCompatiablePaymentModes
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CompatiablePaymentModesDTO> GetCompatiablePaymentModes(List<KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CompatiablePaymentModesDataHandler compatiablePaymentModesDataHandler = new CompatiablePaymentModesDataHandler(sqlTransaction);
            List<CompatiablePaymentModesDTO> result = compatiablePaymentModesDataHandler.GetCompatiablePaymentModesDTOList(searchParameters);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CompatiablePaymentModesDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<CompatiablePaymentModesDTO> savedCompatiablePaymentModesDTO = new List<CompatiablePaymentModesDTO>();
            {
                try
                {
                    if (compatiablePaymentModesDTOList != null && compatiablePaymentModesDTOList.Any())
                    {

                        foreach (CompatiablePaymentModesDTO compatiablePaymentModesDTO in compatiablePaymentModesDTOList)
                        {
                            CompatiablePaymentModesBL compatiablePaymentModes = new CompatiablePaymentModesBL(executionContext, compatiablePaymentModesDTO);
                            compatiablePaymentModes.Save(sqlTransaction);
                            savedCompatiablePaymentModesDTO.Add(compatiablePaymentModes.CompatiablePaymentModesDTO);
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
            log.LogMethodExit(savedCompatiablePaymentModesDTO);
            return savedCompatiablePaymentModesDTO;
        }
        internal List<CompatiablePaymentModesDTO> GetCompatiablePaymentModes(List<int> transctionCompatiablePaymentModesIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(transctionCompatiablePaymentModesIdList, activeChildRecords, sqlTransaction);
            CompatiablePaymentModesDataHandler compatiablePaymentModesDataHandler = new CompatiablePaymentModesDataHandler(sqlTransaction);
            List<CompatiablePaymentModesDTO> compatiablePaymentModesDTOList = compatiablePaymentModesDataHandler.GetCompatiablePaymentModesDTOList(transctionCompatiablePaymentModesIdList, activeChildRecords);
            log.LogMethodExit(compatiablePaymentModesDTOList);
            return compatiablePaymentModesDTOList;
        }
    }
}

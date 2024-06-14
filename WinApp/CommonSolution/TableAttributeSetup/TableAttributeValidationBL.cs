/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - TableAttributeValidationBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0    23-Aug-2021  Fiona              Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
namespace Semnox.Parafait.TableAttributeSetup
{
    public class TableAttributeValidationBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TableAttributeValidationDTO tableAttributeValidationDTO;
        private ExecutionContext executionContext;
        private TableAttributeValidationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        internal TableAttributeValidationBL(ExecutionContext executionContext, TableAttributeValidationDTO tableAttributeValidationDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, tableAttributeValidationDTO);
            if(tableAttributeValidationDTO.TableAttributeValidationId < -1)
            {
                //Validate
            }
            this.tableAttributeValidationDTO = tableAttributeValidationDTO;
            log.LogMethodExit();
        }
        //public TableAttributeValidationBL(ExecutionContext executionContext, TableAttributeValidationDTO parameterTableAttributeValidationDTO, SqlTransaction sqlTransaction = null)
        //    : this(executionContext)
        //{
        //    log.LogMethodEntry(executionContext, parameterTableAttributeValidationDTO, sqlTransaction);

        //    if (parameterTableAttributeValidationDTO.TableAttributeValidationId > -1)
        //    {
        //        LoadTableAttributeValidationDTO(parameterTableAttributeValidationDTO.TableAttributeValidationId, sqlTransaction);//added sql
        //        ThrowIfDTOIsNull(parameterTableAttributeValidationDTO.TableAttributeValidationId);
        //        Update(parameterTableAttributeValidationDTO);
        //    }
        //    else
        //    {
        //        Validate(parameterTableAttributeValidationDTO, sqlTransaction);
        //        tableAttributeValidationDTO = new TableAttributeValidationDTO(parameterTableAttributeValidationDTO);
        //    }
        //    log.LogMethodExit();
        //}
        private void LoadTableAttributeValidationDTO(int tableAttributeValidationId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(tableAttributeValidationId, sqlTransaction);
            TableAttributeValidationDataHandler tableAttributeValidationDataHandler = new TableAttributeValidationDataHandler(sqlTransaction);
            tableAttributeValidationDTO = tableAttributeValidationDataHandler.GetTableAttributeValidationDTO(tableAttributeValidationId);
            ThrowIfDTOIsNull(tableAttributeValidationId);
            log.LogMethodExit();
        }
        private void ThrowIfDTOIsNull(int tableAttributeValidationId)
        {
            log.LogMethodEntry(tableAttributeValidationId);
            if (tableAttributeValidationDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TableAttributeValidation", tableAttributeValidationId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        internal void Update(TableAttributeValidationDTO parameterTableAttributeValidationDTO)
        {
            log.LogMethodEntry(parameterTableAttributeValidationDTO);
            ChangeTableAttributeValidationId(parameterTableAttributeValidationDTO.TableAttributeValidationId);
            ChangeTableAttributeSetupId(parameterTableAttributeValidationDTO.TableAttributeSetupId);
            ChangeDataValidationRule(parameterTableAttributeValidationDTO.DataValidationRule);
            ChangeIsActive(parameterTableAttributeValidationDTO.IsActive);
            log.LogMethodExit();
        }

        private void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (tableAttributeValidationDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to isActive");
                return;
            }
            tableAttributeValidationDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ChangeDataValidationRule(string dataValidationRule)
        {
            log.LogMethodEntry(dataValidationRule);
            if(tableAttributeValidationDTO.DataValidationRule != dataValidationRule)
            {
                tableAttributeValidationDTO.DataValidationRule = dataValidationRule;
            }
            log.LogMethodExit();
        }

        private void ChangeTableAttributeValidationId(int tableAttributeValidationId)
        {
            log.LogMethodEntry(tableAttributeValidationId);
            if (tableAttributeValidationDTO.TableAttributeValidationId == tableAttributeValidationId)
            {
                log.LogMethodExit(null, "No changes to TableAttributeSetupId");
                return;
            }
            tableAttributeValidationDTO.TableAttributeValidationId = tableAttributeValidationId;
            log.LogMethodExit();
        }

        private void ChangeTableAttributeSetupId(int tableAttributeSetupId)
        {
            log.LogMethodEntry(tableAttributeSetupId);
            if (tableAttributeValidationDTO.TableAttributeSetupId == tableAttributeSetupId)
            {
                log.LogMethodExit(null, "No changes to TableAttributeSetupId");
                return;
            }
            tableAttributeValidationDTO.TableAttributeSetupId = tableAttributeSetupId;
            log.LogMethodExit();
        }

        public TableAttributeValidationBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
          : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadTableAttributeValidationDTO(id, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(this.tableAttributeValidationDTO, sqlTransaction);
            TableAttributeValidationDataHandler tableAttributeValidationDataHandler = new TableAttributeValidationDataHandler(sqlTransaction);
            if (tableAttributeValidationDTO.TableAttributeValidationId < 0)
            {
                tableAttributeValidationDTO = tableAttributeValidationDataHandler.Insert(tableAttributeValidationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(tableAttributeValidationDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("TableAttributeValidation", tableAttributeValidationDTO.Guid, sqlTransaction);
                }
                tableAttributeValidationDTO.AcceptChanges();
            }
            else
            {
                if (tableAttributeValidationDTO.IsChanged)
                {
                    tableAttributeValidationDTO = tableAttributeValidationDataHandler.Update(tableAttributeValidationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(tableAttributeValidationDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("TableAttributeValidation", tableAttributeValidationDTO.Guid, sqlTransaction);
                    }
                    tableAttributeValidationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        private void Validate(TableAttributeValidationDTO inputDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (inputDTO != null && inputDTO.IsChanged)
            {
                // Validation code here 
                // return validation exceptions
            }
            log.LogMethodExit();
        }
        public TableAttributeValidationDTO TableAttributeValidationDTO
        {
            get
            {
                TableAttributeValidationDTO result = new TableAttributeValidationDTO(tableAttributeValidationDTO);
                return result;
            }
        }

    }
    public class TableAttributeValidationListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<TableAttributeValidationDTO> tableAttributeValidationDTOList;
        public TableAttributeValidationListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public TableAttributeValidationListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.tableAttributeValidationDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="TableAttributeValidationDTOList"></param>
        public TableAttributeValidationListBL(ExecutionContext executionContext, List<TableAttributeValidationDTO> TableAttributeValidationDTOList)
        {
            log.LogMethodEntry(executionContext, TableAttributeValidationDTOList);
            this.tableAttributeValidationDTOList = TableAttributeValidationDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetTableAttributeValidation
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<TableAttributeValidationDTO> GetTableAttributeValidation(List<KeyValuePair<TableAttributeValidationDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TableAttributeValidationDataHandler tableAttributeValidationDataHandler = new TableAttributeValidationDataHandler(sqlTransaction);
            List<TableAttributeValidationDTO> result = tableAttributeValidationDataHandler.GetTableAttributeValidationDTOList(searchParameters);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<TableAttributeValidationDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<TableAttributeValidationDTO> savedTableAttributeValidationDTO = new List<TableAttributeValidationDTO>();
            {
                try
                {
                    if (tableAttributeValidationDTOList != null && tableAttributeValidationDTOList.Any())
                    {
                        foreach (TableAttributeValidationDTO TableAttributeValidationDTO in tableAttributeValidationDTOList)
                        {
                            TableAttributeValidationBL tableAttributeValidation = new TableAttributeValidationBL(executionContext, TableAttributeValidationDTO);
                            tableAttributeValidation.Save(sqlTransaction);
                            savedTableAttributeValidationDTO.Add(tableAttributeValidation.TableAttributeValidationDTO);
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
            log.LogMethodExit(savedTableAttributeValidationDTO);
            return savedTableAttributeValidationDTO;
        }
        internal List<TableAttributeValidationDTO> GetTableAttributeValidation(List<int> transctionTableAttributeSetIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(transctionTableAttributeSetIdList, activeChildRecords, sqlTransaction);
            TableAttributeValidationDataHandler tableAttributeValidationDataHandler = new TableAttributeValidationDataHandler(sqlTransaction);
            List<TableAttributeValidationDTO> tableAttributeValidationDTOList = tableAttributeValidationDataHandler.GetTableAttributeValidationDTOList(transctionTableAttributeSetIdList, activeChildRecords);
            log.LogMethodExit(tableAttributeValidationDTOList);
            return tableAttributeValidationDTOList;
        }
    }
}

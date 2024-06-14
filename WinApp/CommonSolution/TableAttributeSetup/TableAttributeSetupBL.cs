/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - TableAttributeSetupBL
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
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class TableAttributeSetupBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TableAttributeSetupDTO tableAttributeSetupDTO;
        private ExecutionContext executionContext;
        private TableAttributeSetupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        internal TableAttributeSetupBL(ExecutionContext executionContext, TableAttributeSetupDTO tableAttributeSetupDTO)
           : this(executionContext)
        {
            log.LogMethodEntry();
            if(tableAttributeSetupDTO.TableAttributeSetupId<-1)
            {
                //Validate
            }
            this.tableAttributeSetupDTO = tableAttributeSetupDTO;
            log.LogMethodExit();
        }

        //public TableAttributeSetupBL(ExecutionContext executionContext, TableAttributeSetupDTO parameterTableAttributeSetupDTO, SqlTransaction sqlTransaction = null)
        //   : this(executionContext)
        //{
        //    log.LogMethodEntry(executionContext, parameterTableAttributeSetupDTO, sqlTransaction);

        //    if (parameterTableAttributeSetupDTO.TableAttributeSetupId > -1)
        //    {
        //        LoadTableAttributeSetupDTO(parameterTableAttributeSetupDTO.TableAttributeSetupId, true, true, sqlTransaction);//added sql
        //        ThrowIfDTOIsNull(parameterTableAttributeSetupDTO.TableAttributeSetupId);
        //        Update(parameterTableAttributeSetupDTO);
        //    }
        //    else
        //    {
        //        Validate(parameterTableAttributeSetupDTO, sqlTransaction);
        //        tableAttributeSetupDTO = new TableAttributeSetupDTO(parameterTableAttributeSetupDTO);
        //        if(parameterTableAttributeSetupDTO.TableAttributeValidationDTOList != null && parameterTableAttributeSetupDTO.TableAttributeValidationDTOList.Any())
        //        {
        //            tableAttributeSetupDTO.TableAttributeValidationDTOList = new List<TableAttributeValidationDTO>();
        //            foreach (TableAttributeValidationDTO tableAttributeValidationDTO in parameterTableAttributeSetupDTO.TableAttributeValidationDTOList)
        //            {
        //                if (tableAttributeValidationDTO.TableAttributeValidationId > -1)
        //                {
        //                    string message = MessageContainerList.GetMessage(executionContext, 2196, "tableAttributeValidationDTO", tableAttributeValidationDTO.TableAttributeValidationId);
        //                    log.LogMethodExit(null, "Throwing Exception - " + message);
        //                    throw new EntityNotFoundException(message);
        //                }
        //                var parameterTableAttributeValidationDTO = new TableAttributeValidationDTO(tableAttributeValidationDTO);
        //                TableAttributeValidationBL tableAttributeValidationBL = new TableAttributeValidationBL(executionContext, parameterTableAttributeValidationDTO);
        //                tableAttributeSetupDTO.TableAttributeValidationDTOList.Add(tableAttributeValidationBL.TableAttributeValidationDTO);
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}
        private void LoadTableAttributeSetupDTO(int tableAttributeSetupId, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(tableAttributeSetupId, sqlTransaction);
            TableAttributeSetupDataHandler tableAttributeSetupDataHandler = new TableAttributeSetupDataHandler(sqlTransaction);
            tableAttributeSetupDTO = tableAttributeSetupDataHandler.GetTableAttributeSetupDTO(tableAttributeSetupId);
            
            ThrowIfDTOIsNull(tableAttributeSetupId);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void ThrowIfDTOIsNull(int tableAttributeSetupId)
        {
            log.LogMethodEntry(tableAttributeSetupId);
            if (tableAttributeSetupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TableAttributeSetup", tableAttributeSetupId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        internal void Update(TableAttributeSetupDTO parameterTableAttributeSetupDTO)
        {
            log.LogMethodEntry(parameterTableAttributeSetupDTO);
            ChangeTableAttributeSetupId(parameterTableAttributeSetupDTO.TableAttributeSetupId);
            ChangeAttributeEnabledTableId(parameterTableAttributeSetupDTO.AttributeEnabledTableId);
            ChangeColumnName(parameterTableAttributeSetupDTO.ColumnName);
            ChangeDisplayName(parameterTableAttributeSetupDTO.DisplayName);
            ChangeDataSourceType(parameterTableAttributeSetupDTO.DataSourceType);
            ChangeDataType(parameterTableAttributeSetupDTO.DataType);
            ChangeLookupId(parameterTableAttributeSetupDTO.LookupId);
            ChangeSQLSource(parameterTableAttributeSetupDTO.SQLSource);
            ChangeSQLDisplayMember(parameterTableAttributeSetupDTO.SQLDisplayMember);
            ChangeSQLValueMember(parameterTableAttributeSetupDTO.SQLValueMember);
            ChangeIsActive(parameterTableAttributeSetupDTO.IsActive);
            Dictionary<int, TableAttributeValidationDTO> tableTAttributeValidationDTODictionary = new Dictionary<int, TableAttributeValidationDTO>();
            if (tableAttributeSetupDTO.TableAttributeValidationDTOList!=null && tableAttributeSetupDTO.TableAttributeValidationDTOList.Any())
            {
                foreach(TableAttributeValidationDTO tableAttributeValidationDTO in tableAttributeSetupDTO.TableAttributeValidationDTOList)
                {
                    tableTAttributeValidationDTODictionary.Add(tableAttributeValidationDTO.TableAttributeValidationId, tableAttributeValidationDTO);
                }
            }
            if(parameterTableAttributeSetupDTO.TableAttributeValidationDTOList != null && parameterTableAttributeSetupDTO.TableAttributeValidationDTOList.Any())
            {
                foreach(TableAttributeValidationDTO parameterTableAttributeValidationDTO in parameterTableAttributeSetupDTO.TableAttributeValidationDTOList)
                {
                    if(tableTAttributeValidationDTODictionary.ContainsKey(parameterTableAttributeValidationDTO.TableAttributeValidationId))
                    {
                        TableAttributeValidationBL tableAttributeValidationBL = new TableAttributeValidationBL(executionContext, tableTAttributeValidationDTODictionary[parameterTableAttributeValidationDTO.TableAttributeValidationId]);
                        tableAttributeValidationBL.Update(parameterTableAttributeValidationDTO);
                    }
                    else
                    {
                        TableAttributeValidationBL tableAttributeValidationBL = new TableAttributeValidationBL(executionContext, parameterTableAttributeValidationDTO);
                        if(tableAttributeSetupDTO.TableAttributeValidationDTOList==null)
                        {
                            tableAttributeSetupDTO.TableAttributeValidationDTOList = new List<TableAttributeValidationDTO>();
                        }
                        tableAttributeSetupDTO.TableAttributeValidationDTOList.Add(tableAttributeValidationBL.TableAttributeValidationDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ChangeDisplayName(string displayName)
        {
            log.LogMethodEntry(displayName);
            if (tableAttributeSetupDTO.DisplayName == displayName)
            {
                log.LogMethodExit(null, "No changes to displayName");
                return;
            }
            tableAttributeSetupDTO.DisplayName = displayName;
            log.LogMethodExit();
        }

        private void ChangeColumnName(string columnName)
        {
            log.LogMethodEntry(columnName);
            if (tableAttributeSetupDTO.ColumnName == columnName)
            {
                log.LogMethodExit(null, "No changes to columnName");
                return;
            }
            tableAttributeSetupDTO.ColumnName = columnName;
            log.LogMethodExit();
        }

        private void ChangeSQLValueMember(string sQLValueMember)
        {
            log.LogMethodEntry(sQLValueMember);
            if (tableAttributeSetupDTO.SQLValueMember == sQLValueMember)
            {
                log.LogMethodExit(null, "No changes to sQLValueMember");
                return;
            }
            tableAttributeSetupDTO.SQLValueMember = sQLValueMember;
            log.LogMethodExit();
        }

        private void ChangeSQLDisplayMember(string sQLDisplayMember)
        {
            log.LogMethodEntry(sQLDisplayMember);
            if (tableAttributeSetupDTO.SQLDisplayMember == sQLDisplayMember)
            {
                log.LogMethodExit(null, "No changes to sQLDisplayMember");
                return;
            }
            tableAttributeSetupDTO.SQLDisplayMember = sQLDisplayMember;
            log.LogMethodExit();
        }

        private void ChangeSQLSource(string sQLSource)
        {
            log.LogMethodEntry(sQLSource);
            if (tableAttributeSetupDTO.SQLSource == sQLSource)
            {
                log.LogMethodExit(null, "No changes to sQLSource");
                return;
            }
            tableAttributeSetupDTO.SQLSource = sQLSource;
            log.LogMethodExit();
        }

        private void ChangeLookupId(int lookupId)
        {
            log.LogMethodEntry(lookupId);
            if (tableAttributeSetupDTO.LookupId == lookupId)
            {
                log.LogMethodExit(null, "No changes to lookupId");
                return;
            }
            tableAttributeSetupDTO.LookupId = lookupId;
            log.LogMethodExit();
        }

        private void ChangeDataType(TableAttributeSetupDTO.DataTypeEnum dataType)
        {
            log.LogMethodEntry(dataType);
            if (tableAttributeSetupDTO.DataType == dataType)
            {
                log.LogMethodExit(null, "No changes to dataType");
                return;
            }
            tableAttributeSetupDTO.DataType = dataType;
            log.LogMethodExit();
        }

        private void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (tableAttributeSetupDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to isActive");
                return;
            }
            tableAttributeSetupDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ChangeDataSourceType(TableAttributeSetupDTO.DataSourceTypeEnum dataSourceType)
        {
            log.LogMethodEntry(dataSourceType);
            if (tableAttributeSetupDTO.DataSourceType == dataSourceType)
            {
                log.LogMethodExit(null, "No changes to dataSourceType");
                return;
            }
            tableAttributeSetupDTO.DataSourceType = dataSourceType;
            log.LogMethodExit();
        }

        private void ChangeAttributeEnabledTableId(int attributeEnabledTableId)
        {
            log.LogMethodEntry();
            if (tableAttributeSetupDTO.AttributeEnabledTableId == attributeEnabledTableId)
            {
                log.LogMethodExit(null, "No changes to TableAttributeSetupId");
                return;
            }
            tableAttributeSetupDTO.AttributeEnabledTableId = attributeEnabledTableId;
            log.LogMethodExit();
        }

        private void ChangeTableAttributeSetupId(int tableAttributeSetupId)
        {
            log.LogMethodEntry();
            if (tableAttributeSetupDTO.TableAttributeSetupId == tableAttributeSetupId)
            {
                log.LogMethodExit(null, "No changes to TableAttributeSetupId");
                return;
            }
            tableAttributeSetupDTO.TableAttributeSetupId = tableAttributeSetupId;
            log.LogMethodExit();
        }

        public TableAttributeSetupBL(ExecutionContext executionContext, int id, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
          : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadTableAttributeSetupDTO(id, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            TableAttributeValidationListBL tableAttributeValidationListBL = new TableAttributeValidationListBL(executionContext);
            List<TableAttributeValidationDTO> tableAttributeValidationDTOList = tableAttributeValidationListBL.GetTableAttributeValidation(new List<int>() { tableAttributeSetupDTO.TableAttributeSetupId }, activeChildRecords, sqlTransaction);
            tableAttributeSetupDTO.TableAttributeValidationDTOList = tableAttributeValidationDTOList;
            log.LogMethodExit();

        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(this.tableAttributeSetupDTO, sqlTransaction);
            TableAttributeSetupDataHandler tableAttributeSetupDataHandler = new TableAttributeSetupDataHandler(sqlTransaction);
            if (tableAttributeSetupDTO.TableAttributeSetupId < 0)
            {
                tableAttributeSetupDTO = tableAttributeSetupDataHandler.Insert(tableAttributeSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(tableAttributeSetupDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("TableAttributeSetup", tableAttributeSetupDTO.Guid, sqlTransaction);
                }
                tableAttributeSetupDTO.AcceptChanges();
            }
            else
            {
                if (tableAttributeSetupDTO.IsChanged)
                {
                    tableAttributeSetupDTO = tableAttributeSetupDataHandler.Update(tableAttributeSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(tableAttributeSetupDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("TableAttributeSetup", tableAttributeSetupDTO.Guid, sqlTransaction);
                    }
                    tableAttributeSetupDTO.AcceptChanges();
                }
            }
            if(tableAttributeSetupDTO.TableAttributeValidationDTOList!=null && tableAttributeSetupDTO.TableAttributeValidationDTOList.Any())
            {
               foreach(TableAttributeValidationDTO tableAttributeValidationDTO in tableAttributeSetupDTO.TableAttributeValidationDTOList)
               {
                    if(tableAttributeValidationDTO.TableAttributeSetupId != tableAttributeSetupDTO.TableAttributeSetupId)
                    {
                        tableAttributeValidationDTO.TableAttributeSetupId = tableAttributeSetupDTO.TableAttributeSetupId;
                    }
               }
               TableAttributeValidationListBL tableAttributeValidationListBL = new TableAttributeValidationListBL(executionContext, tableAttributeSetupDTO.TableAttributeValidationDTOList);
               tableAttributeValidationListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void Validate(TableAttributeSetupDTO inputDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (inputDTO != null && inputDTO.IsChanged)
            {
                // Validation code here 
                // return validation exceptions
                if (inputDTO.DataSourceType == TableAttributeSetupDTO.DataSourceTypeEnum.NONE)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Data Source Type")));
                    //&1 is mandatory. Please enter a value.
                }
                if (inputDTO.DataType == TableAttributeSetupDTO.DataTypeEnum.NONE)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Data Type")));
                    //&1 is mandatory. Please enter a value..
                }
            }
            log.LogMethodExit();
        }
        public TableAttributeSetupDTO TableAttributeSetupDTO
        {
            get
            {
                TableAttributeSetupDTO result = new TableAttributeSetupDTO(tableAttributeSetupDTO);
                return result;
            }
        }
    }
    public class TableAttributeSetupListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<TableAttributeSetupDTO> TableAttributeSetupDTOList;
        public TableAttributeSetupListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public TableAttributeSetupListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.TableAttributeSetupDTOList = null;
            log.LogMethodExit();
        }
        public TableAttributeSetupListBL(ExecutionContext executionContext, List<TableAttributeSetupDTO> TableAttributeSetupDTOList)
        {
            log.LogMethodEntry(executionContext, TableAttributeSetupDTOList);
            this.TableAttributeSetupDTOList = TableAttributeSetupDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<TableAttributeSetupDTO> GetTableAttributeSetup(List<KeyValuePair<TableAttributeSetupDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChild = false,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TableAttributeSetupDataHandler tableAttributeSetupDataHandler = new TableAttributeSetupDataHandler(sqlTransaction);
            List<TableAttributeSetupDTO> result = tableAttributeSetupDataHandler.GetTableAttributeSetupDTOList(searchParameters);
            if(loadChildRecords && result != null && result.Any())
            {
                Build(result, loadActiveChild, sqlTransaction);
            }
            log.LogMethodExit(result);
            return result;
        }
        public List<TableAttributeSetupDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<TableAttributeSetupDTO> savedTableAttributeSetupDTO = new List<TableAttributeSetupDTO>();
            {
                try
                {
                    if (TableAttributeSetupDTOList != null && TableAttributeSetupDTOList.Any())
                    {

                        foreach (TableAttributeSetupDTO tableAttributeSetupDTO in TableAttributeSetupDTOList)
                        {
                            TableAttributeSetupBL tableAttributeSetup = new TableAttributeSetupBL(executionContext, tableAttributeSetupDTO);
                            tableAttributeSetup.Save(sqlTransaction);
                            savedTableAttributeSetupDTO.Add(tableAttributeSetup.TableAttributeSetupDTO);
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
            log.LogMethodExit(savedTableAttributeSetupDTO);
            return savedTableAttributeSetupDTO;
        }
        private void Build(List<TableAttributeSetupDTO> tableAttributeSetupDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(tableAttributeSetupDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, TableAttributeSetupDTO> tableAttributeSetupDTOIdMap = new Dictionary<int, TableAttributeSetupDTO>();
            List<int> tableAttributeSetupDTOIdList = new List<int>();
            for (int i = 0; i < tableAttributeSetupDTOList.Count; i++)
            {
                if (tableAttributeSetupDTOIdMap.ContainsKey(tableAttributeSetupDTOList[i].TableAttributeSetupId))
                {
                    continue;
                }
                tableAttributeSetupDTOIdMap.Add(tableAttributeSetupDTOList[i].TableAttributeSetupId, tableAttributeSetupDTOList[i]);
                tableAttributeSetupDTOIdList.Add(tableAttributeSetupDTOList[i].TableAttributeSetupId);
            }

            TableAttributeValidationListBL tableAttributeValidationListBL = new TableAttributeValidationListBL(executionContext);
            List<TableAttributeValidationDTO> tableAttributeValidationDTOList = tableAttributeValidationListBL.GetTableAttributeValidation(tableAttributeSetupDTOIdList, activeChildRecords, sqlTransaction);
            if (tableAttributeValidationDTOList != null && tableAttributeValidationDTOList.Any())
            {
                for (int i = 0; i < tableAttributeValidationDTOList.Count; i++)
                {
                    if (tableAttributeSetupDTOIdMap.ContainsKey(tableAttributeValidationDTOList[i].TableAttributeSetupId) == false)
                    {
                        continue;
                    }
                    TableAttributeSetupDTO tableAttributeSetupDTO = tableAttributeSetupDTOIdMap[tableAttributeValidationDTOList[i].TableAttributeSetupId];
                    if (tableAttributeSetupDTO.TableAttributeValidationDTOList == null)
                    {
                        tableAttributeSetupDTO.TableAttributeValidationDTOList = new List<TableAttributeValidationDTO>();
                    }
                    tableAttributeSetupDTO.TableAttributeValidationDTOList.Add(tableAttributeValidationDTOList[i]);
                }
            }
        }
        internal List<TableAttributeSetupDTO> GetTableAttributeSetup(List<int> attributeEnabledTablesIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(attributeEnabledTablesIdList, activeChildRecords, sqlTransaction);
            TableAttributeSetupDataHandler tableAttributeSetupDataHandler = new TableAttributeSetupDataHandler(sqlTransaction);
            List<TableAttributeSetupDTO> tableAttributeSetupDTOList = tableAttributeSetupDataHandler.GetTableAttributeSetupDTOList(attributeEnabledTablesIdList, activeChildRecords);
            Build(tableAttributeSetupDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(tableAttributeSetupDTOList);
            return tableAttributeSetupDTOList;
        }
    }
}

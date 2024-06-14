/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - TableAttributeSetup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.140.0      16-Aug-2021  Fiona              Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
namespace Semnox.Parafait.TableAttributeSetup
{
    public class AttributeEnabledTablesBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AttributeEnabledTablesDTO attributeEnabledTablesDTO;
        private ExecutionContext executionContext;
        private AttributeEnabledTablesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public AttributeEnabledTablesBL(ExecutionContext executionContext, AttributeEnabledTablesDTO parameterAttributeEnabledTablesDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterAttributeEnabledTablesDTO, sqlTransaction);

            if (parameterAttributeEnabledTablesDTO.AttributeEnabledTableId > -1)
            {
                LoadAttributeEnabledTablesDTO(parameterAttributeEnabledTablesDTO.AttributeEnabledTableId,true ,true , sqlTransaction);//added sql
                ThrowIfDTOIsNull(parameterAttributeEnabledTablesDTO.AttributeEnabledTableId);
                Update(parameterAttributeEnabledTablesDTO);
            }
            else
            {
                ValidateNew(parameterAttributeEnabledTablesDTO, sqlTransaction);
                attributeEnabledTablesDTO = new AttributeEnabledTablesDTO(parameterAttributeEnabledTablesDTO);
                //if(parameterAttributeEnabledTablesDTO.TableAttributeSetupDTOList!=null && parameterAttributeEnabledTablesDTO.TableAttributeSetupDTOList.Any())
                //{
                //    attributeEnabledTablesDTO.TableAttributeSetupDTOList = new List<TableAttributeSetupDTO>();
                //    foreach(TableAttributeSetupDTO parameterTableAttributeSetupDTO in parameterAttributeEnabledTablesDTO.TableAttributeSetupDTOList)
                //    {
                //        if(parameterTableAttributeSetupDTO.TableAttributeSetupId > -1)
                //        {
                //            string message = MessageContainerList.GetMessage(executionContext, 2196, "TableAttributeSetupDTO", parameterTableAttributeSetupDTO.TableAttributeSetupId);
                //            log.LogMethodExit(null, "Throwing Exception - " + message);
                //            throw new EntityNotFoundException(message);
                //        }
                //        var newTableAttributeSetupDTO = new TableAttributeSetupDTO(parameterTableAttributeSetupDTO);
                //        TableAttributeSetupBL tableAttributeSetupBL = new TableAttributeSetupBL(executionContext, newTableAttributeSetupDTO);
                //        attributeEnabledTablesDTO.TableAttributeSetupDTOList.Add(tableAttributeSetupBL.TableAttributeSetupDTO);
                //    }
                //}
            }
            log.LogMethodExit();
        }

        private void ValidateNew(AttributeEnabledTablesDTO inputDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inputDTO != null && inputDTO.IsChangedRecursive)
            {
                if (inputDTO.TableAttributeSetupDTOList != null && inputDTO.TableAttributeSetupDTOList.Any())
                {
                    foreach (TableAttributeSetupDTO tableAttributeSetupDTO in inputDTO.TableAttributeSetupDTOList)
                    {
                        if (tableAttributeSetupDTO.TableAttributeSetupId > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "TableAttributeSetup", tableAttributeSetupDTO.TableAttributeSetupId);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                    }
                }

            }
            log.LogMethodExit();
        }

        private void Validate(AttributeEnabledTablesDTO inputDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (inputDTO != null && inputDTO.IsChanged)
            {
                // Validation code here 
                // return validation exceptions
            }
            log.LogMethodExit();
        }

        private void Update(AttributeEnabledTablesDTO parameterAttributeEnabledTablesDTO)
        {
            log.LogMethodEntry(parameterAttributeEnabledTablesDTO);
            ChangeAttributeEnabledTableId(parameterAttributeEnabledTablesDTO.AttributeEnabledTableId);
            ChangeTableName(parameterAttributeEnabledTablesDTO.TableName);
            ChangeDescription(parameterAttributeEnabledTablesDTO.Description);
            ChangeIsActive(parameterAttributeEnabledTablesDTO.IsActive);
            Dictionary<int, TableAttributeSetupDTO> tableAttributeSetupDTODictionary = new Dictionary<int, TableAttributeSetupDTO>();
            if (attributeEnabledTablesDTO.TableAttributeSetupDTOList != null &&
               attributeEnabledTablesDTO.TableAttributeSetupDTOList.Any())
            {
                foreach (var tableAttributeSetupDTO in attributeEnabledTablesDTO.TableAttributeSetupDTOList)
                {
                    tableAttributeSetupDTODictionary.Add(tableAttributeSetupDTO.TableAttributeSetupId, tableAttributeSetupDTO);
                }
            }
            if (parameterAttributeEnabledTablesDTO.TableAttributeSetupDTOList != null &&
               parameterAttributeEnabledTablesDTO.TableAttributeSetupDTOList.Any())
            {
                foreach (TableAttributeSetupDTO parameterTableAttributeSetupDTO in parameterAttributeEnabledTablesDTO.TableAttributeSetupDTOList)
                {
                    if (tableAttributeSetupDTODictionary.ContainsKey(parameterTableAttributeSetupDTO.TableAttributeSetupId))
                    {
                        TableAttributeSetupBL tableAttributeSetupBL = new TableAttributeSetupBL(executionContext, tableAttributeSetupDTODictionary[parameterTableAttributeSetupDTO.TableAttributeSetupId]);
                        tableAttributeSetupBL.Update(parameterTableAttributeSetupDTO);
                        //tableAttributeSetupDTODictionary[parameterTableAttributeSetupDTO.TableAttributeSetupId] = tableAttributeSetupBL.TableAttributeSetupDTO; 
                    }
                    else
                    {
                        TableAttributeSetupBL tableAttributeSetupBL = new TableAttributeSetupBL(executionContext, parameterTableAttributeSetupDTO);
                        if (attributeEnabledTablesDTO.TableAttributeSetupDTOList == null)
                        {
                            attributeEnabledTablesDTO.TableAttributeSetupDTOList = new List<TableAttributeSetupDTO>();
                        }
                        attributeEnabledTablesDTO.TableAttributeSetupDTOList.Add(tableAttributeSetupBL.TableAttributeSetupDTO);
                    }
                } 

                //for (int i = 0; i < attributeEnabledTablesDTO.TableAttributeSetupDTOList.Count; i++)
                //{ 
                //    if (attributeEnabledTablesDTO.TableAttributeSetupDTOList[i].TableAttributeSetupId > -1 
                //        && tableAttributeSetupDTODictionary.ContainsKey(attributeEnabledTablesDTO.TableAttributeSetupDTOList[i].TableAttributeSetupId))
                //    {
                //        attributeEnabledTablesDTO.TableAttributeSetupDTOList[i] = tableAttributeSetupDTODictionary[attributeEnabledTablesDTO.TableAttributeSetupDTOList[i].TableAttributeSetupId];
                //    }
                //}
            }
            log.LogMethodExit();
        }

        private void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (attributeEnabledTablesDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to isActive");
                return;
            }
            attributeEnabledTablesDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ChangeDescription(string description)
        {
            log.LogMethodEntry(description);
            if (attributeEnabledTablesDTO.Description == description)
            {
                log.LogMethodExit(null, "No changes to Description");
                return;
            }
            attributeEnabledTablesDTO.Description = description;
            log.LogMethodExit();
        }
        private void ChangeTableName(string tableName)
        {
            log.LogMethodEntry(tableName);
            if (attributeEnabledTablesDTO.TableName == tableName)
            {
                log.LogMethodExit(null, "No changes to tableName");
                return;
            }
            attributeEnabledTablesDTO.TableName = tableName;
            log.LogMethodExit();
        }
        private void ChangeAttributeEnabledTableId(int attributeEnabledTableId)
        {
            log.LogMethodEntry(attributeEnabledTableId);
            if (attributeEnabledTablesDTO.AttributeEnabledTableId == attributeEnabledTableId)
            {
                log.LogMethodExit(null, "No changes to attributeEnabledTableId");
                return;
            }
            attributeEnabledTablesDTO.AttributeEnabledTableId = attributeEnabledTableId;
            log.LogMethodExit();
        }
        private void ThrowIfDTOIsNull(int attributeEnabledTableId)
        {
            log.LogMethodEntry(attributeEnabledTableId);
            if (attributeEnabledTablesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AttributeEnabledTables", attributeEnabledTableId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadAttributeEnabledTablesDTO(int attributeEnabledTableId, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(attributeEnabledTableId, sqlTransaction);
            AttributeEnabledTablesDataHandler attributeEnabledTablesDataHandler = new AttributeEnabledTablesDataHandler(sqlTransaction);
            attributeEnabledTablesDTO = attributeEnabledTablesDataHandler.GetAttributeEnabledTablesDTO(attributeEnabledTableId);
            ThrowIfDTOIsNull(attributeEnabledTableId);
            if(loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            TableAttributeSetupListBL tableAttributeSetupListBL = new TableAttributeSetupListBL(executionContext);
            List<TableAttributeSetupDTO> tableAttributeSetupDTOList = tableAttributeSetupListBL.GetTableAttributeSetup(new List<int>() { attributeEnabledTablesDTO.AttributeEnabledTableId }, activeChildRecords, sqlTransaction);
            attributeEnabledTablesDTO.TableAttributeSetupDTOList = tableAttributeSetupDTOList;
            log.LogMethodExit();

        }
        public AttributeEnabledTablesBL(ExecutionContext executionContext, int id, bool loadChildRecords=false, bool activeChildRecords=false, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadAttributeEnabledTablesDTO(id, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(this.attributeEnabledTablesDTO, sqlTransaction);
            AttributeEnabledTablesDataHandler attributeEnabledTablesDataHandler = new AttributeEnabledTablesDataHandler(sqlTransaction);
            if (attributeEnabledTablesDTO.AttributeEnabledTableId < 0)
            {
                attributeEnabledTablesDTO = attributeEnabledTablesDataHandler.Insert(attributeEnabledTablesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(attributeEnabledTablesDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("AttributeEnabledTables", attributeEnabledTablesDTO.Guid, sqlTransaction);
                }
                attributeEnabledTablesDTO.AcceptChanges();
            }
            else
            {
                if (attributeEnabledTablesDTO.IsChanged)
                {
                    attributeEnabledTablesDTO = attributeEnabledTablesDataHandler.Update(attributeEnabledTablesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(attributeEnabledTablesDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("AttributeEnabledTables", attributeEnabledTablesDTO.Guid, sqlTransaction);
                    }
                    attributeEnabledTablesDTO.AcceptChanges();
                }
            }
            if(attributeEnabledTablesDTO.TableAttributeSetupDTOList!=null && attributeEnabledTablesDTO.TableAttributeSetupDTOList.Any())
            {
                foreach (TableAttributeSetupDTO tableAttributeSetupDTO in attributeEnabledTablesDTO.TableAttributeSetupDTOList)
                {
                    if(tableAttributeSetupDTO.AttributeEnabledTableId != attributeEnabledTablesDTO.AttributeEnabledTableId)
                    {
                        tableAttributeSetupDTO.AttributeEnabledTableId = attributeEnabledTablesDTO.AttributeEnabledTableId;
                    }
                }
                TableAttributeSetupListBL tableAttributeSetupListBL = new TableAttributeSetupListBL(executionContext, attributeEnabledTablesDTO.TableAttributeSetupDTOList);
                tableAttributeSetupListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AttributeEnabledTablesDTO AttributeEnabledTablesDTO
        {
            get
            {
                AttributeEnabledTablesDTO result = new AttributeEnabledTablesDTO(attributeEnabledTablesDTO);
                return result;
            }
        }
    }
    public class AttributeEnabledTablesListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList;
        public AttributeEnabledTablesListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public AttributeEnabledTablesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.attributeEnabledTablesDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="attributeEnabledTablesDTOList"></param>
        public AttributeEnabledTablesListBL(ExecutionContext executionContext, List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList)
        {
            log.LogMethodEntry(executionContext, attributeEnabledTablesDTOList);
            this.attributeEnabledTablesDTOList = attributeEnabledTablesDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetAttributeEnabledTables
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AttributeEnabledTablesDTO> GetAttributeEnabledTables(List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>> searchParameters,
             bool loadChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChild, sqlTransaction);
            AttributeEnabledTablesDataHandler attributeEnabledTablesDataHandler = new AttributeEnabledTablesDataHandler(sqlTransaction);
            List<AttributeEnabledTablesDTO> result = attributeEnabledTablesDataHandler.GetAttributeEnabledTablesDTOList(searchParameters);
            if (loadChildRecords && result != null && result.Any())
            {
                Build(result, loadActiveChild, sqlTransaction);
            }
            log.LogMethodExit(result);
            return result;
        }
        private void Build(List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(attributeEnabledTablesDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AttributeEnabledTablesDTO> attributeEnabledTablesDTOIdMap = new Dictionary<int, AttributeEnabledTablesDTO>();
            List<int> attributeEnabledTablesDTOIdList = new List<int>();
            for (int i = 0; i < attributeEnabledTablesDTOList.Count; i++)
            {
                if (attributeEnabledTablesDTOIdMap.ContainsKey(attributeEnabledTablesDTOList[i].AttributeEnabledTableId))
                {
                    continue;
                }
                attributeEnabledTablesDTOIdMap.Add(attributeEnabledTablesDTOList[i].AttributeEnabledTableId, attributeEnabledTablesDTOList[i]);
                attributeEnabledTablesDTOIdList.Add(attributeEnabledTablesDTOList[i].AttributeEnabledTableId);
            }
            TableAttributeSetupListBL tableAttributeSetupListBL = new TableAttributeSetupListBL(executionContext);
            List<TableAttributeSetupDTO> tableAttributeSetupDTOList = tableAttributeSetupListBL.GetTableAttributeSetup(attributeEnabledTablesDTOIdList, activeChildRecords, sqlTransaction);
            if (tableAttributeSetupDTOList != null && tableAttributeSetupDTOList.Any())
            {
                for (int i = 0; i < tableAttributeSetupDTOList.Count; i++)
                {
                    if (attributeEnabledTablesDTOIdMap.ContainsKey(tableAttributeSetupDTOList[i].AttributeEnabledTableId) == false)
                    {
                        continue;
                    }
                    AttributeEnabledTablesDTO attributeEnabledTablesDTO = attributeEnabledTablesDTOIdMap[tableAttributeSetupDTOList[i].AttributeEnabledTableId];
                    if (attributeEnabledTablesDTO.TableAttributeSetupDTOList == null)
                    {
                        attributeEnabledTablesDTO.TableAttributeSetupDTOList = new List<TableAttributeSetupDTO>();
                    }
                    attributeEnabledTablesDTO.TableAttributeSetupDTOList.Add(tableAttributeSetupDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
        public List<AttributeEnabledTablesDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<AttributeEnabledTablesDTO> savedAttributeEnabledTablesDTO = new List<AttributeEnabledTablesDTO>();
            {
                try
                {
                    if (attributeEnabledTablesDTOList != null && attributeEnabledTablesDTOList.Any())
                    {

                        foreach (AttributeEnabledTablesDTO attributeEnabledTablesDTO in attributeEnabledTablesDTOList)
                        {
                            AttributeEnabledTablesBL attributeEnabledTables = new AttributeEnabledTablesBL(executionContext, attributeEnabledTablesDTO, sqlTransaction);
                            attributeEnabledTables.Save(sqlTransaction);
                            savedAttributeEnabledTablesDTO.Add(attributeEnabledTables.AttributeEnabledTablesDTO);
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
            log.LogMethodExit(savedAttributeEnabledTablesDTO);
            return savedAttributeEnabledTablesDTO;
        }
        internal List<AttributeEnabledTablesDTO> GetAttributeEnabledTables(List<int> transctionAttributeEnabledTablesIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(transctionAttributeEnabledTablesIdList, activeChildRecords, sqlTransaction);
            AttributeEnabledTablesDataHandler attributeEnabledTablesDataHandler = new AttributeEnabledTablesDataHandler(sqlTransaction);
            List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList = attributeEnabledTablesDataHandler.GetAttributeEnabledTablesDTOList(transctionAttributeEnabledTablesIdList, activeChildRecords);
            log.LogMethodExit(attributeEnabledTablesDTOList);
            return attributeEnabledTablesDTOList;
        }

        internal DateTime? GetAttributeEnabledTablesModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry();
            AttributeEnabledTablesDataHandler attributeEnabledTablesDataHandler = new AttributeEnabledTablesDataHandler();
            DateTime? result = attributeEnabledTablesDataHandler.GetAttributeEnabledTablesLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

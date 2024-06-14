/********************************************************************************************
 * Project Name - TableAttributeSetup                                                                       
 * Description  - EnabledAttibutesBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0    18-Aug-2021   Fiona              Created 
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
    public class EnabledAttributesBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EnabledAttributesDTO enabledAttibutesDTO;
        private ExecutionContext executionContext;
        private EnabledAttributesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public EnabledAttributesBL(ExecutionContext executionContext, EnabledAttributesDTO parameterEnabledAttibutesDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterEnabledAttibutesDTO, sqlTransaction);

            if (parameterEnabledAttibutesDTO.EnabledAttibuteId > -1)
            {
                LoadEnabledAttibutesDTO(parameterEnabledAttibutesDTO.EnabledAttibuteId, sqlTransaction);
                ThrowIfDTOIsNull(parameterEnabledAttibutesDTO.EnabledAttibuteId);
                Update(parameterEnabledAttibutesDTO);
            }
            else
            {
                //Validate(parameterEnabledAttibutesDTO, sqlTransaction);
                enabledAttibutesDTO = new EnabledAttributesDTO(parameterEnabledAttibutesDTO);
            }
            log.LogMethodExit();
        }
        public EnabledAttributesBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
         : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadEnabledAttibutesDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        private void Validate(EnabledAttributesDTO inputDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (inputDTO != null && inputDTO.IsChanged)
            {
                if (string.IsNullOrWhiteSpace(inputDTO.EnabledAttributeName))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 249, (MessageContainerList.GetMessage(executionContext, "Enabled Attribute Name"))));
                    //&1 is mandatory. Please enter a value.
                }
                if (inputDTO.MandatoryOrOptional == EnabledAttributesDTO.IsMandatoryOrOptional.Mandatory && string.IsNullOrWhiteSpace(inputDTO.DefaultValue))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4109, inputDTO.EnabledAttributeName));
                    //'Please provide default value for the mandatory field &1'
                }

                List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.RECORD_GUID, inputDTO.RecordGuid));
                searchParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.IS_ACTIVE, "1"));
                EnabledAttibutesListBL enabledAttibutesListBL = new EnabledAttibutesListBL();
                List<EnabledAttributesDTO> currentEnabledAttributesDTOList = enabledAttibutesListBL.GetEnabledAttibutes(searchParameters);
                if (currentEnabledAttributesDTOList != null && currentEnabledAttributesDTOList.Any(x => x.EnabledAttributeName.ToLower() == inputDTO.EnabledAttributeName.ToLower()))
                {
                    EnabledAttributesDTO duplicateEnabledAttributesDTO = currentEnabledAttributesDTOList.FirstOrDefault(x => x.EnabledAttibuteId == inputDTO.EnabledAttibuteId);
                    if (duplicateEnabledAttributesDTO == null || (duplicateEnabledAttributesDTO != null && duplicateEnabledAttributesDTO.EnabledAttributeName.ToLower() != inputDTO.EnabledAttributeName.ToLower()))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Enabled Attribute name already Exists."));
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ThrowIfDTOIsNull(int Id)
        {
            log.LogMethodEntry(Id);
            if (enabledAttibutesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "EnabledAttibutes", Id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadEnabledAttibutesDTO(int attributeEnabledTableId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(attributeEnabledTableId, sqlTransaction);
            EnabledAttributesDataHandler enabledAttibutesDataHandler = new EnabledAttributesDataHandler(sqlTransaction);
            enabledAttibutesDTO = enabledAttibutesDataHandler.GetEnabledAttibutesDTO(attributeEnabledTableId);
            ThrowIfDTOIsNull(attributeEnabledTableId);
            log.LogMethodExit();
        }
        private void Update(EnabledAttributesDTO parameterEnabledAttibutesDTO)
        {
            log.LogMethodEntry(parameterEnabledAttibutesDTO);
            ChangeEnabledAttibuteId(parameterEnabledAttibutesDTO.EnabledAttibuteId);
            ChangeRecordGuid(parameterEnabledAttibutesDTO.RecordGuid);
            ChangeTableName(parameterEnabledAttibutesDTO.TableName);
            ChangeEnabledAttributeName(parameterEnabledAttibutesDTO.EnabledAttributeName);
            ChangeMandatoryOrOptional(parameterEnabledAttibutesDTO.MandatoryOrOptional);
            ChangeIsActive(parameterEnabledAttibutesDTO.IsActive);
            ChangeDefaultValue(parameterEnabledAttibutesDTO.DefaultValue);
            log.LogMethodExit();
        }

        private void ChangeDefaultValue(string defaultValue)
        {
            log.LogMethodEntry(defaultValue);
            if (enabledAttibutesDTO.DefaultValue == defaultValue)
            {
                log.LogMethodExit(null, "No changes to defaultValue");
                return;
            }
            enabledAttibutesDTO.DefaultValue = defaultValue;
            log.LogMethodExit();
        }

        private void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (enabledAttibutesDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to isActive");
                return;
            }
            enabledAttibutesDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ChangeMandatoryOrOptional(EnabledAttributesDTO.IsMandatoryOrOptional mandatoryOrOptional)
        {
            log.LogMethodEntry(mandatoryOrOptional);
            if (enabledAttibutesDTO.MandatoryOrOptional == mandatoryOrOptional)
            {
                log.LogMethodExit(null, "No changes to mandatoryOrOptional");
                return;
            }
            enabledAttibutesDTO.MandatoryOrOptional = mandatoryOrOptional;
            log.LogMethodExit();
        }

        private void ChangeEnabledAttributeName(string enabledAttributeName)
        {
            log.LogMethodEntry(enabledAttributeName);
            if (enabledAttibutesDTO.EnabledAttributeName == enabledAttributeName)
            {
                log.LogMethodExit(null, "No changes to EnabledAttributeName");
                return;
            }
            enabledAttibutesDTO.EnabledAttributeName = enabledAttributeName;
            log.LogMethodExit();
        }

        private void ChangeTableName(string tableName)
        {
            log.LogMethodEntry(tableName);
            if (enabledAttibutesDTO.TableName == tableName)
            {
                log.LogMethodExit(null, "No changes to tableName");
                return;
            }
            enabledAttibutesDTO.TableName = tableName;
            log.LogMethodExit();
        }

        private void ChangeRecordGuid(string recordGuid)
        {
            log.LogMethodEntry(recordGuid);
            if (enabledAttibutesDTO.RecordGuid == recordGuid)
            {
                log.LogMethodExit(null, "No changes to recordGuid");
                return;
            }
            enabledAttibutesDTO.RecordGuid = recordGuid;
            log.LogMethodExit();
        }

        private void ChangeEnabledAttibuteId(int enabledAttibuteId)
        {
            log.LogMethodEntry(enabledAttibuteId);
            if (enabledAttibutesDTO.EnabledAttibuteId == enabledAttibuteId)
            {
                log.LogMethodExit(null, "No changes to enabledAttibuteId");
                return;
            }
            enabledAttibutesDTO.EnabledAttibuteId = enabledAttibuteId;
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(this.enabledAttibutesDTO, sqlTransaction);
            EnabledAttributesDataHandler enabledAttibutesDataHandler = new EnabledAttributesDataHandler(sqlTransaction);
            if (enabledAttibutesDTO.EnabledAttibuteId < 0)
            {
                enabledAttibutesDTO = enabledAttibutesDataHandler.Insert(enabledAttibutesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(enabledAttibutesDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("EnabledAttibutes", enabledAttibutesDTO.Guid, sqlTransaction);
                }
                enabledAttibutesDTO.AcceptChanges();
            }
            else
            {
                if (enabledAttibutesDTO.IsChanged)
                {
                    enabledAttibutesDTO = enabledAttibutesDataHandler.Update(enabledAttibutesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(enabledAttibutesDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("EnabledAttibutes", enabledAttibutesDTO.Guid, sqlTransaction);
                    }
                    enabledAttibutesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        public EnabledAttributesDTO EnabledAttibutesDTO
        {
            get
            {
                EnabledAttributesDTO result = new EnabledAttributesDTO(enabledAttibutesDTO);
                return result;
            }
        }
    }
    public class EnabledAttibutesListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<EnabledAttributesDTO> enabledAttibutesDTOList;
        public EnabledAttibutesListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public EnabledAttibutesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.enabledAttibutesDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="enabledAttibutesDTOList"></param>
        public EnabledAttibutesListBL(ExecutionContext executionContext, List<EnabledAttributesDTO> enabledAttibutesDTOList)
        {
            log.LogMethodEntry(executionContext, enabledAttibutesDTOList);
            this.enabledAttibutesDTOList = enabledAttibutesDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetEnabledAttibutes
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<EnabledAttributesDTO> GetEnabledAttibutes(List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            EnabledAttributesDataHandler enabledAttibutesDataHandler = new EnabledAttributesDataHandler(sqlTransaction);
            List<EnabledAttributesDTO> result = enabledAttibutesDataHandler.GetEnabledAttibutesDTOList(searchParameters);
            log.LogMethodExit(result);
            return result;
        }
        public List<EnabledAttributesDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<EnabledAttributesDTO> savedEnabledAttibutesDTO = new List<EnabledAttributesDTO>();
            {
                try
                {
                    if (enabledAttibutesDTOList != null && enabledAttibutesDTOList.Any())
                    {

                        foreach (EnabledAttributesDTO enabledAttibutesDTO in enabledAttibutesDTOList)
                        {
                            EnabledAttributesBL enabledAttibutes = new EnabledAttributesBL(executionContext, enabledAttibutesDTO);
                            enabledAttibutes.Save(sqlTransaction);
                            savedEnabledAttibutesDTO.Add(enabledAttibutes.EnabledAttibutesDTO);
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
            log.LogMethodExit(savedEnabledAttibutesDTO);
            return savedEnabledAttibutesDTO;
        }
        internal List<EnabledAttributesDTO> GetEnabledAttibutes(List<int> transctionEnabledAttibutesIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(transctionEnabledAttibutesIdList, activeChildRecords, sqlTransaction);
            EnabledAttributesDataHandler enabledAttibutesDataHandler = new EnabledAttributesDataHandler(sqlTransaction);
            List<EnabledAttributesDTO> enabledAttibutesDTOList = enabledAttibutesDataHandler.GetEnabledAttibutesDTOList(transctionEnabledAttibutesIdList, activeChildRecords);
            log.LogMethodExit(enabledAttibutesDTOList);
            return enabledAttibutesDTOList;
        }

        internal DateTime? GetEnabledAttributesModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            EnabledAttributesDataHandler enabledAttributessDataHandler = new EnabledAttributesDataHandler();
            DateTime? result = enabledAttributessDataHandler.GetEnabledAttributesModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

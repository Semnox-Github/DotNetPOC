/********************************************************************************************
 * Project Name - TableAttributeSetup                                                                       
 * Description  - TableAttributeDetailsListBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0    08-Sep-2021    Guru S A           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.TableAttributeSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.TableAttributeDetailsUtils
{

    /// <summary>
    /// TableAttributeDetailsBL
    /// </summary>
    public class TableAttributeDetailsBL
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private TableAttributeDetailsDTO tableAttributeDetailsDTO;
        public TableAttributeDetailsBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TableAttributeDetailsBL(ExecutionContext executionContext, TableAttributeDetailsDTO tableAttributeDetailsDTO)
        {
            log.LogMethodEntry(executionContext, tableAttributeDetailsDTO);
            this.executionContext = executionContext;
            this.tableAttributeDetailsDTO = tableAttributeDetailsDTO;
            if (tableAttributeDetailsDTO == null || executionContext == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1956);
                //Invalid Empty Parameters
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// ValidateAttributeValue
        /// </summary>
        public void ValidateAttributeValue()
        {
            log.LogMethodEntry();
            if(tableAttributeDetailsDTO.MandatoryOrOptional == EnabledAttributesDTO.IsMandatoryOrOptional.Mandatory 
                && string.IsNullOrWhiteSpace(tableAttributeDetailsDTO.AttributeValue))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 165, tableAttributeDetailsDTO.AttributeDisplayName));
                //&1 is mandatory
            }
            if (string.IsNullOrWhiteSpace(tableAttributeDetailsDTO.AttributeValue) == false)
            {
                string attributeValue = tableAttributeDetailsDTO.AttributeValue;
                if (tableAttributeDetailsDTO.DataType == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                {
                    DateTime dateTimeValue;
                    if (DateTime.TryParse(attributeValue, out dateTimeValue) == false)
                    {
                        throw new ValidationException(tableAttributeDetailsDTO.AttributeDisplayName + " - "+ MessageContainerList.GetMessage(executionContext, 15));
                        //Invalid Date value
                    }
                }
                if (tableAttributeDetailsDTO.DataType == TableAttributeSetupDTO.DataTypeEnum.NUMBER)
                {
                    int numberValue;
                    if (Int32.TryParse(attributeValue, out numberValue) == false)
                    {
                        throw new ValidationException(tableAttributeDetailsDTO.AttributeDisplayName + " - " + MessageContainerList.GetMessage(executionContext, 648));
                        //Please enter a valid number
                    }
                }
                if (tableAttributeDetailsDTO.DataValidationRuleList != null && tableAttributeDetailsDTO.DataValidationRuleList.Any())
                {
                    for (int i = 0; i < tableAttributeDetailsDTO.DataValidationRuleList.Count; i++)
                    {
                        Regex rgx = new Regex(tableAttributeDetailsDTO.DataValidationRuleList[i]);
                        if (rgx.IsMatch(attributeValue) == false)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4095, tableAttributeDetailsDTO.AttributeDisplayName));
                            //&1 field value failed to clear data validation rules
                        }
                    }
                }
            }

            log.LogMethodExit(); 
        }
    }
        /// <summary>
        /// TableAttributeDetailsListBL
        /// </summary>
        public class TableAttributeDetailsListBL
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext; 
        public TableAttributeDetailsListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TableAttributeDetailsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }
        /// <summary>
        /// GetTableAttributeDetailsDTOList
        /// </summary>
        /// <param name="attributeEnabledTable"></param>
        /// <param name="enabledTableName"></param>
        /// <param name="recordGuid"></param>
        /// <returns></returns>
        public List<TableAttributeDetailsDTO> GetTableAttributeDetailsDTOList(EnabledAttributesDTO.TableWithEnabledAttributes enabledTableName, string recordGuid)
        {
            log.LogMethodEntry(enabledTableName, recordGuid);
            List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList = new List<TableAttributeDetailsDTO>();
            List<EnabledAttributesContainerDTO> enabledAttributeContainerDTOList = EnabledAttributesContainerList.GetEnabledAttributesContainerDTOCollection(executionContext.GetSiteId()).EnabledAttributesContainerDTOList;
            if (enabledAttributeContainerDTOList != null && enabledAttributeContainerDTOList.Any())
            {
               string nameValue = EnabledAttributesDTO.TableWithEnabledAttributesToString(enabledTableName);
               List<EnabledAttributesContainerDTO> inputRecordsEACDTOList = enabledAttributeContainerDTOList.Where(eat =>  eat.TableName == nameValue
                                                                                                    && eat.RecordGuid == recordGuid).ToList();
                if (inputRecordsEACDTOList != null && inputRecordsEACDTOList.Any())
                {
                    //Record in enabled table has attributes defined
                    List<AttributeEnabledTablesContainerDTO> aetDTOList = AttributeEnabledTablesContainerList.GetAttributeEnabledTablesContainerDTOCollection(executionContext.GetSiteId()).AttributeEnabledTablesContainerDTOList;
                    if (aetDTOList != null && aetDTOList.Any())
                    {
                        AttributeEnabledTablesDTO.AttributeEnabledTableNames attributeEnabledTable = EnabledAttributesDTO.GetAttributeEnabledTable(enabledTableName);
                        string enabledTableValue = AttributeEnabledTablesDTO.AttributeEnabledTableNamesToString(attributeEnabledTable);
                        AttributeEnabledTablesContainerDTO aetDTO = aetDTOList.Find(aet => aet.TableName == enabledTableValue);
                        if (aetDTO != null && aetDTO.TableAttributeSetupContainerDTOList != null && aetDTO.TableAttributeSetupContainerDTOList.Any())
                        {
                            //loop through table attributes setup and get details for recordGuid's enabled attributes
                            for (int i = 0; i < inputRecordsEACDTOList.Count; i++)
                            {
                                EnabledAttributesContainerDTO inputEACDTO = inputRecordsEACDTOList[i];

                                for (int j = 0; j < aetDTO.TableAttributeSetupContainerDTOList.Count; j++)
                                {
                                    TableAttributeSetupContainerDTO attributeSetupDTO = aetDTO.TableAttributeSetupContainerDTOList[j];
                                    if (attributeSetupDTO != null && attributeSetupDTO.ColumnName == inputEACDTO.EnabledAttributeName)
                                    {
                                        List<string> validationRuleList = new List<string>();
                                        if (attributeSetupDTO.TableAttributeValidationContainerDTOList != null 
                                            && attributeSetupDTO.TableAttributeValidationContainerDTOList.Any())
                                        {
                                            validationRuleList = attributeSetupDTO.TableAttributeValidationContainerDTOList.Select(tav => tav.DataValidationRule).ToList();
                                        }
                                        TableAttributeDetailsDTO tableAttributeDetailsDTO = new TableAttributeDetailsDTO(aetDTO.TableName, inputEACDTO.TableName,
                                            inputEACDTO.RecordGuid, attributeSetupDTO.ColumnName, attributeSetupDTO.DisplayName, inputEACDTO.MandatoryOrOptional,
                                            attributeSetupDTO.DataType, attributeSetupDTO.LookupId, attributeSetupDTO.SQLSource, attributeSetupDTO.SQLDisplayMember,
                                            attributeSetupDTO.SQLValueMember, validationRuleList, null, inputEACDTO.DefaultValue);

                                        tableAttributeDetailsDTOList.Add(tableAttributeDetailsDTO);
                                    }
                                } 
                            }
                        }
                    }
                }
            }            
            log.LogMethodExit(tableAttributeDetailsDTOList);
            return tableAttributeDetailsDTOList;
        }

        public List<KeyValuePair<string, string>> GetSQLDataList(string sqlSource, string sqlDisplayMember, string sqlValueMember)
        {
            log.LogMethodEntry(sqlSource, sqlDisplayMember, sqlValueMember);
            List<KeyValuePair<string, string>> sourceDataList = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrWhiteSpace(sqlSource) || string.IsNullOrWhiteSpace(sqlDisplayMember) || string.IsNullOrWhiteSpace(sqlValueMember))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1930));
                //Invalid Empty Parameters
            }
            TableAttributeDetailsDataHandler tableAttributeDetailsDataHandler = new TableAttributeSetup.TableAttributeDetailsDataHandler();
            sourceDataList = tableAttributeDetailsDataHandler.GetSQLDataList(sqlSource, sqlDisplayMember,  sqlValueMember);
            log.LogMethodExit(sourceDataList);
            return sourceDataList;
        }

    }
}

/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of CustomDataSetDTODefinition      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    class CustomDataSetDTODefinition:ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string applicability;
        protected List<CustomAttributesDTO> customAttributesDTOList;
        protected ExecutionContext executionContext;
        public CustomDataSetDTODefinition(ExecutionContext executionContext, string fieldName, string applicability):base(fieldName, typeof(CustomDataSetDTO))
        {
            log.LogMethodEntry(executionContext, fieldName, applicability);
            this.applicability = applicability;
            this.executionContext = executionContext;
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, applicability));
            searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            log.Debug("Begins - GetCustomAttributesDTOList(searchParameters)");
            customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters);
            List<CustomAttributesDTO> enabledCustomAttributesDTOList = new List<CustomAttributesDTO>();
            if(customAttributesDTOList != null)
            {
                foreach (var customAttributesDTO in customAttributesDTOList)
                {
                    if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, customAttributesDTO.Name) != "N")
                    {
                        enabledCustomAttributesDTOList.Add(customAttributesDTO);
                    }
                }
            }
            customAttributesDTOList = enabledCustomAttributesDTOList.OrderBy(x=>x.Sequence).ToList();
            log.Debug("Ends - GetCustomAttributesDTOList(searchParameters)");
        }

        public override string DisplayName
        {
            get
            {
                string result = string.Empty;
                if(customAttributesDTOList != null && customAttributesDTOList.Count > 0)
                {
                    result = customAttributesDTOList[0].Name;
                }
                return result;
            }
        }

        public override void Configure(object templateObject)
        {
            log.LogMethodEntry(templateObject);
            if (templateObject == null)
            {
                SetDisplayHeaderRows(false);
            }
            log.LogMethodExit();
        }

        public override void BuildHeaderRow(Row headerRow)
        {
            log.LogMethodEntry(headerRow);
            if (displayHeaderRows && customAttributesDTOList != null && customAttributesDTOList.Count > 0)
            {
                foreach (var customAttributesDTO in customAttributesDTOList)
                {
                    headerRow.AddCell(new Cell(customAttributesDTO.Name));
                }
            }
            log.LogMethodExit();
        }

        public override object Deserialize(Row headerRow, Row row, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow, row, currentIndex);
            CustomDataSetDTO customDataSetDTO = null;
            if(customAttributesDTOList != null && 
                customAttributesDTOList.Count > 0 &&
                row.Cells.Count > currentIndex)
            {
                bool found = true;
                while (found)
                {
                    CustomAttributesDTO customAttributesDTO = GetCustomAttributesDTO(headerRow[currentIndex].Value);
                    if (customAttributesDTO != null)
                    {
                        if (customDataSetDTO == null)
                        {
                            customDataSetDTO = new CustomDataSetDTO();
                            customDataSetDTO.CustomDataDTOList = new List<CustomDataDTO>();
                        }
                        CustomDataDTO customDataDTO = CreateCustomDataDTO(customAttributesDTO, row[currentIndex].Value);
                        customDataSetDTO.CustomDataDTOList.Add(customDataDTO);
                        currentIndex++;
                    }
                    else
                    {
                        found = false;
                    }
                }
            }
            log.LogMethodExit(customDataSetDTO);
            return customDataSetDTO;
        }

        public override void Serialize(Row row, object value)
        {
            log.LogMethodEntry(row, value);
            if (value != null && value is CustomDataSetDTO)
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, value as CustomDataSetDTO);
                foreach (var customAttributesDTO in customAttributesDTOList)
                {
                    row.AddCell(new Cell(GetCustomDataString(customAttributesDTO, customDataSetBL.GetCustomDataDTO(customAttributesDTO.CustomAttributeId))));
                }
            }
            else if (displayHeaderRows && customAttributesDTOList != null && customAttributesDTOList.Count > 0)
            {
                foreach (var customAttributesDTO in customAttributesDTOList)
                {
                    row.AddCell(new Cell(string.Empty));
                }
            }
            log.LogMethodExit();
        }

        private string GetCustomDataString(CustomAttributesDTO customAttributesDTO, CustomDataDTO customDataDTO)
        {
            log.LogMethodEntry(customAttributesDTO, customDataDTO);
            string result = string.Empty;
            if(customDataDTO != null)
            {
                switch(customAttributesDTO.Type)
                {
                    case "TEXT":
                        {
                            result = customDataDTO.CustomDataText;
                            break;
                        }
                    case "NUMBER":
                        {
                            if(customDataDTO.CustomDataNumber.HasValue)
                            {
                                result = customDataDTO.CustomDataNumber.Value.ToString();
                            }
                            break;
                        }
                    case "DATE":
                        {
                            if(customDataDTO.CustomDataDate.HasValue)
                            {
                                result = customDataDTO.CustomDataDate.Value.ToString(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"));
                            }
                            break;
                        }
                    case "LIST":
                        {
                            CustomAttributesBL customAttributesBL = new CustomAttributesBL(executionContext, customAttributesDTO);
                            result = customAttributesBL.GetValue(customDataDTO.ValueId);
                            break;
                        }
                }
            }
            log.LogMethodExit();
            return result;
        }

        private CustomDataDTO CreateCustomDataDTO(CustomAttributesDTO customAttributesDTO, string value)
        {
            log.LogMethodEntry(customAttributesDTO , value);
            CustomDataDTO customDataDTO = new CustomDataDTO();
            customDataDTO.CustomAttributeId = customAttributesDTO.CustomAttributeId;
            if(customAttributesDTO.Type == "TEXT")
            {
                customDataDTO.CustomDataText = value;
            }
            else if(customAttributesDTO.Type == "NUMBER")
            {
                customDataDTO.CustomDataNumber = (decimal?) new NullableDecimalValueConverter("").FromString(value);
            }
            else if(customAttributesDTO.Type == "DATE")
            {
                customDataDTO.CustomDataDate = (DateTime?)new NullableDateTimeValueConverter(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT")).FromString(value);
            }
            else if (customAttributesDTO.Type == "LIST")
            {
                CustomAttributesBL customAttributesBL = new CustomAttributesBL(executionContext, customAttributesDTO);
                customDataDTO.ValueId = customAttributesBL.GetValueId(value);
            }
            log.LogMethodExit(customDataDTO);
            return customDataDTO;
        }

        private CustomAttributesDTO GetCustomAttributesDTO(string customAttributeName)
        {
            log.LogMethodEntry(customAttributeName);
            CustomAttributesDTO result = null;
            if (customAttributesDTOList != null && customAttributesDTOList.Count > 0)
            {
                foreach (var customAttributesDTO in customAttributesDTOList)
                {
                    if(customAttributesDTO.Name.ToUpper() == customAttributeName.ToUpper())
                    {
                        result = customAttributesDTO;
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Validates the header row
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="currentIndex"></param>
        public override void ValidateHeaderRow(Row headerRow, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow , currentIndex);
            if (headerRow.Cells.Count > currentIndex)
            {
                foreach (var customAttributesDTO in customAttributesDTOList)
                {
                    if(customAttributesDTO.Name != headerRow[currentIndex].Value)
                    {
                        throw new Exception("Invalid header definition. please check the template");
                    }
                    currentIndex++;
                }
            }
            log.LogMethodExit();
        }
    }
}

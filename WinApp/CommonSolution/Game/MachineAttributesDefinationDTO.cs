/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Bulk Upload Mapper MachineAttributesDefinationDTO Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       16-Mar-2019   Muhammed Mehraj  Created  
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/

using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Mapper class to MachineAttributeDTO
    /// </summary>
    class MachineAttributesDefinationDTO : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected List<MachineAttributeDTO> machineAttributesDTOList;
        protected ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public MachineAttributesDefinationDTO(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(MachineAttributeDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);
            this.executionContext = executionContext;
            GameSystem gameSystem = new GameSystem("MACHINES", -1, executionContext);
            machineAttributesDTOList = gameSystem.GetMachineAttributes();
            log.LogMethodExit();
        }

        /// <summary>
        /// Display name property to prepare headers
        /// </summary>
        public override string DisplayName
        {
            get
            {
                string result = string.Empty;
                if (machineAttributesDTOList != null && machineAttributesDTOList.Count > 0)
                {
                    result = machineAttributesDTOList[0].AttributeName.ToString();
                }
                return result;
            }
        }
        /// <summary>
        /// Configure excel sheet templete 
        /// </summary>
        /// <param name="templateObject"></param>

        public override void Configure(object templateObject)
        {
            log.LogMethodEntry(templateObject);
            if (templateObject == null)
            {
                SetDisplayHeaderRows(false);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// BuildHeaderRow Builds the Header rows
        /// </summary>
        /// <param name="headerRow"></param>

        public override void BuildHeaderRow(Row headerRow)
        {
            log.LogMethodEntry(headerRow);
            if (displayHeaderRows && machineAttributesDTOList != null && machineAttributesDTOList.Count > 0)
            {
                foreach (MachineAttributeDTO machineAttributeDTO in machineAttributesDTOList)
                {
                    headerRow.AddCell(new Cell(machineAttributeDTO.AttributeName.ToString()));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Serialize all the rows 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public override void Serialize(Row row, object value)
        {
            log.LogMethodEntry(row, value);
            if (value != null && value is MachineAttributeDTO)
            {
                GameSystem gameSystem = new GameSystem("MACHINES", -1, executionContext);

                foreach (var customAttributesDTO in machineAttributesDTOList)
                {
                    row.AddCell(new Cell(machineAttributesDTOList.Where(x => x.AttributeId == Convert.ToInt32(value)).Select(c => c.AttributeValue).ToString()));
                }
            }
            else if (displayHeaderRows && machineAttributesDTOList != null && machineAttributesDTOList.Count > 0)
            {
                foreach (var customAttributesDTO in machineAttributesDTOList)
                {
                    row.AddCell(new Cell(string.Empty));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deserialize the sheet rows with MachineAttributes
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="row"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public override object Deserialize(Row headerRow, Row row, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow, row, currentIndex);
            List<MachineAttributeDTO> machineAttributeDTOList = new List<MachineAttributeDTO>();
            if (machineAttributesDTOList != null &&
                machineAttributesDTOList.Count > 0 &&
                row.Cells.Count > currentIndex)
            {
                bool found = true;
                while (found)
                {
                    if (row.Cells.Count > currentIndex)
                    {
                        MachineAttributeDTO machineAttributeDTO = GetAttributesValues(headerRow[currentIndex].Value, row[currentIndex].Value);
                        if (machineAttributeDTO != null)
                        {
                            machineAttributeDTOList.Add(machineAttributeDTO);
                            currentIndex++;
                        }
                    }
                    else
                    {
                        found = false;
                    }
                }
            }
            log.LogMethodExit(machineAttributeDTOList);
            return machineAttributeDTOList;
        }

        /// <summary>
        /// Gets the AttributeName and value and assigns it to MachineAttributeDTO.MachineAttribute
        /// </summary>
        /// <param name="attributeNames"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        private MachineAttributeDTO GetAttributesValues(string attributeNames, string attributeValue)
        {
            log.LogMethodEntry(attributeNames, attributeValue);
            MachineAttributeDTO result = null;
            if (machineAttributesDTOList != null && machineAttributesDTOList.Count > 0)
            {
                foreach (MachineAttributeDTO machineAttributeDTO in machineAttributesDTOList)
                {
                    MachineAttributeDTO.MachineAttribute attributeName = (MachineAttributeDTO.MachineAttribute)Enum.Parse(typeof(MachineAttributeDTO.MachineAttribute), attributeNames, true);
                    if (machineAttributeDTO.AttributeName == attributeName)
                    {
                        machineAttributeDTO.AttributeValue = attributeValue;
                        result = machineAttributeDTO;
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}

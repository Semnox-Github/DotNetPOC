/********************************************************************************************
 * Project Name - CustomDataContainerDTO DTO
 * Description  - Data object of CustomDataContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *1.00        25-Jul-2021   Girish Kundar          Created 

 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the CustomDataSet data object class. This acts as data holder for the CustomDataSet business object
    /// </summary>
    public class CustomDataContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// default Constructor
        /// </summary>
        public CustomDataContainerDTO()
        {
            log.LogMethodEntry();
            CustomAttributeId = -1;
            CustomDataId = -1;
            CustomDataSetId = -1;
            ValueId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="customDataContainerDTO"></param>
        public CustomDataContainerDTO(CustomDataContainerDTO customDataContainerDTO)
        {
            log.LogMethodEntry(customDataContainerDTO);
            CustomAttributeId = customDataContainerDTO.CustomAttributeId;
            Name = customDataContainerDTO.Name;
            Type = customDataContainerDTO.Type; 
            CustomDataId = customDataContainerDTO.CustomDataId;
            CustomDataSetId = customDataContainerDTO.CustomDataSetId;
            Value = customDataContainerDTO.Value;
            CustomDataText = customDataContainerDTO.CustomDataText;
            CustomDataNumber = customDataContainerDTO.CustomDataNumber;
            CustomDataDate = customDataContainerDTO.CustomDataDate;
            ValueId = customDataContainerDTO.ValueId;
            CustomeDataSetGuid = customDataContainerDTO.CustomeDataSetGuid;
            CustomeDataGuid = customDataContainerDTO.CustomeDataGuid;
            log.LogMethodExit();
        }

        public int CustomAttributeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int CustomDataId { get; set; }
        public int CustomDataSetId { get; set; }
        public string Value { get; set; }
        public string CustomDataText { get; set; }
        public decimal? CustomDataNumber { get; set; }
        public DateTime? CustomDataDate { get; set; }
        public int ValueId { get; set; }
        public string CustomeDataSetGuid { get; set; }
        public string CustomeDataGuid { get; set; }
    }
}

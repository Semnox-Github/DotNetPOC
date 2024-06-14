/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of Segment Definition  Excel DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    15-Oct-2020   Mushahid Faizan Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class SegmentExcelDTODefinition : ComplexAttributeDefinition
    {

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public SegmentExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(SegmentDefinitionDTO))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("SegmentDefinitionId", "SegmentDefinitionId", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("SegmentName", "SegmentName", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ApplicableEntity", "ApplicableEntity", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("SequenceOrder", "SequenceOrder", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsMandatory", "IsMandatory", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", "LastUpdatedBy", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastupdatedDate", "LastupdatedDate", new NullableDateTimeValueConverter()));

        }
    }
}

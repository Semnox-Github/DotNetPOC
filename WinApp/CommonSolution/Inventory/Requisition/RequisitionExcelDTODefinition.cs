/********************************************************************************************
 * Project Name - Inventory
 * Description  -RequisitionExcelDTODefinition holds the excel informaton for Requisition. 
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0       04-Feb-2020   Mushahid Faizan      Created : Inventory UI redesign changes
 *********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    class RequisitionExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public RequisitionExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(RequisitionDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequisitionId", "RequisitionId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TemplateId", "TemplateId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequisitionNo", "RequisitionNo", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequisitionType", "RequisitionType", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequestingDept", "RequestingDept", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("FromDepartment", "FromDepartment", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ToDepartment", "ToDepartment", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequiredByDate", "RequiredByDate", new NullableDateTimeValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("EstimatedValue", "EstimatedValue", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Status", "Status", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DocumentTypeName", "DocumentTypeName", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("FromSiteId", "FromSiteId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ToSiteId", "ToSiteId", new IntValueConverter()));
            log.LogMethodExit();


        }
    }
}

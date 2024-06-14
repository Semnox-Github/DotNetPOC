/********************************************************************************************
 * Project Name - Inventory
 * Description  -RequisitionLinesExcelDTODefinition holds the excel informaton for RequisitionLines. 
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
    public class RequisitionLinesExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public RequisitionLinesExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(RequisitionLinesDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequisitionId", "RequisitionId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequisitionLineId", "RequisitionLineId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequisitionNo", "RequisitionNo", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductId", "ProductId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Code", "Code", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Description", "Description", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequestedQuantity", "RequestedQuantity", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ApprovedQuantity", "ApprovedQuantity", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequiredByDate", "RequiredByDate", new NullableDateTimeValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new NullableBooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Status", "Status", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOM", "UOM", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("StockAtLocation", "StockAtLocation", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Price", "Price", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CategoryName", "CategoryName", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOMId", "UOMId", new IntValueConverter()));
            log.LogMethodExit();
        }

    }
}

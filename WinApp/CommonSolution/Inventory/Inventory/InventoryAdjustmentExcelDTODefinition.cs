/********************************************************************************************
 * Project Name - Inventory
 * Description  -InventoryWastageExcelDTODefinition holds the excel informaton for Inventory Wastage Summary. 
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
    public class InventoryAdjustmentExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public InventoryAdjustmentExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(InventoryAdjustmentsDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("AdjustmentId", "AdjustmentId", new DoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AdjustmentType", "AdjustmentType", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AdjustmentQuantity", "AdjustmentQuantity", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("FromLocationId", "FromLocationId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ToLocationId", "ToLocationId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductId", "ProductId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Timestamp", "Timestamp", new NullableDateTimeValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AdjustmentTypeId", "AdjustmentTypeId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LotID", "LotID", new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DocumentTypeID", "DocumentTypeID", new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Price", "Price", new NullableDoubleValueConverter()));

            log.LogMethodExit();


        }
    }
}

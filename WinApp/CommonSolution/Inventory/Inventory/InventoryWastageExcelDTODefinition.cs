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
    public class InventoryWastageExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public InventoryWastageExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(InventoryWastageSummaryDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductId", "ProductId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CategoryId", "CategoryId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LocationName", "LocationName", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LotNumber", "LotNumber", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOM", "UOM", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Category", "Category", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AdjustmentId", "AdjustmentId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductCode", "ProductCode", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductDescription", "ProductDescription", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("WastageQuantity", "WastageQuantity", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AvailableQuantity", "AvailableQuantity", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LocationId", "LocationId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));

            log.LogMethodExit();


        }
    }
}

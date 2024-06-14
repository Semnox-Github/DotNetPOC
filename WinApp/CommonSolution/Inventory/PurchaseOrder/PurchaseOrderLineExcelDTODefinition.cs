/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created Data object for excel sheet
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1    18-Feb-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    class PurchaseOrderLineExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public PurchaseOrderLineExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(PurchaseOrderLineDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("PurchaseOrderLineId", "PurchaseOrderLine Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PurchaseOrderId", "purchaseOrder Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ItemCode", "item Code", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Description", "description", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Quantity", "quantity", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UnitPrice", "unit Price", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("SubTotal", "sub Total", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Timestamp", "time stamp", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxAmount", "tax Amount", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DiscountPercentage", "discount Percentage", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequiredByDate", "requiredByDate", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductId", "product Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CancelledDate", "cancelled Date", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequisitionId", "requisition Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequisitionLineId", "requisitionLine Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UnitLogisticsCost", "unitLogistics Cost", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PriceInTickets", "priceIn Tickets", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OriginalReferenceGUID", "originalReference GUID", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PurchaseTaxId", "purchaseTax Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOMId", "Uom Id", new IntValueConverter()));
            log.LogMethodExit();
        }
    }
} 
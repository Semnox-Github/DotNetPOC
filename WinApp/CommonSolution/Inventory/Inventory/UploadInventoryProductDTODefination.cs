/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - Bulk Upload Mapper UploadInventoryProductDTODefination class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70       25-Jul-2019   Muhammed Mehraj  Created     
 *2.70.2     13-Aug-2019   Deeksha          Added logger methods.
 *2.90.0     02-Jul-2020   Deeksha          Inventory process : Weighted Avg Costing changes.
 *2.100.0    13-Sep-2020   Deeksha          Modified for Recipe Management enhancement.
 *2.100.0    17-Nov-2020   Mushahid Faizan  WMS Issue fixes.
 ********************************************************************************************/

using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class UploadInventoryProductDTODefination : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UploadInventoryProductDTODefination(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(UploadInventoryProductDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Code", "Code", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductName", "Product Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Description", "Description", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CategoryName", "Category", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PriceInTickets", "Price In Tickets", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Cost", "Cost", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ReorderPoint", "Reorder Point", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("SalePrice", "Sale Price", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Vendor", "Vendor Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("BarCode", "Bar Code", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LotControlled", "LotControlled", new NullableBooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("MarketListItem", "MarketListItem", new NullableBooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ExpiryType", "ExpiryType", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IssuingApproach", "IssuingApproach", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ExpiryDays", "ExpiryDays", new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Redeemable", "Redeemable", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Sellable", "Sellable", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Uom", "UOM", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ItemMarkUp", "Item Markup %", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AutoUpdatePit", "Auto Update PIT?", new NullableBooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DisplayInPos", "Display In POS?", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DisplayGroup", "Display Group", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("HsnSacCode", "HSN SAC Code ", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OpeningQty", "Opening Qty", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ReceivePrice", "Receive Price", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ExpiryDate", "Expiry Date", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ReorderQty", "Reorder Qty", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("SalesTax", "Sales Tax", new StringValueConverter()));
            //attributeDefinitionList.Add(new CustomSegmentDTODefination(executionContext, "CustomSegmentDefinitionList"));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CostIncludesTax", "CostIncludesTax", new NullableBooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ItemType", "ItemType", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("YieldPercentage", "YieldPercentage", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IncludeInPlan", "IncludeInPlan", new NullableBooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RecipeDescription", "RecipeDescription", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("InventoryUOM", "InventoryUOM", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PreparationTime", "PreparationTime", new NullableIntValueConverter()));

            attributeDefinitionList.Add(new CustomSegmentDTODefination(executionContext, "CustomSegmentDefinitionList"));
            log.LogMethodExit();
        }
    }
}

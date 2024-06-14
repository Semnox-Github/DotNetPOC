/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of Physical Count  Excel DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    11-Jan-2021   Mushahid Faizan Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory 
{
    public class PhysicalCountReviewExcelDTODefinition : ComplexAttributeDefinition
    {
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public PhysicalCountReviewExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(PhysicalCountReviewDTO))
        {
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductID", "Product Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Code", "Code", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Description", "Description", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Category", "Category", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Barcode", "Barcode", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("SKU", "SKU", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LotID", "Lot Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LotNumber", "Lot Number", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LocationID", "Location Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Location", "Location", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CurrentInventoryQuantity", "Current Quantity", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("NewQuantity", "New Quantity", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PhysicalCountRemarks", "Remarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RemarksMandatory", "Remarks Mandatory", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOM", "UOM", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("HistoryId", "History Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PhysicalCountId", "Physical CountId", new IntValueConverter()));
        }
    }
}

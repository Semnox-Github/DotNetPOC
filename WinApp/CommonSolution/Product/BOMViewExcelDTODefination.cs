/********************************************************************************************
 * Project Name - Product 
 * Description  - BOMViewExcelDTODefination  object of BOM View Excel information
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By             Remarks          
 *********************************************************************************************
 *2.150.0    29-Dec-2022        Abhishek                Created 
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class BOMViewExcelDTODefination : ComplexAttributeDefinition
    {
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public BOMViewExcelDTODefination(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(BOMExcelDTO))
        {
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductCode", "Product Code", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductName", "Product Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ChildProductCode", "Child Product Code", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ChildProductName", "Child Product Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Quantity", "Quantity", new NullableDecimalValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOM", "UOM", new StringValueConverter()));
        }
    }
}

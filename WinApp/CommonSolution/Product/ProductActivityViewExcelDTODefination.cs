/********************************************************************************************
 * Project Name - Product 
 * Description  - ProductActivityViewExcelDTODefination  object ofProduct Activity View Excel information
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By             Remarks          
 *********************************************************************************************
 *2.150.2    26-Jul-2022        Abhishek                Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class ProductActivityViewExcelDTODefination : ComplexAttributeDefinition
    {
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public ProductActivityViewExcelDTODefination(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(ProductActivityViewExcel))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Location", "Location", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransferLocation", "Transfer Location", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TrxType", "Trx Type", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Trx_Date", "Trx_Date", new NullableDateTimeValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Quantity", "Quantity", new NullableDoubleValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOM", "UOM", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("UserName", "User Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LotId", "Lot Id", new IntValueConverter()));
        }
    }
}

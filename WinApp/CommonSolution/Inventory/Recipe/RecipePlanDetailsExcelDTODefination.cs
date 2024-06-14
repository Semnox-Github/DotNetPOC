/********************************************************************************************
 * Project Name - Inventory 
 * Description  - RecipePlanDetailsExcelDTODefination  object of Recipe Plan Details Excel information
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By             Remarks          
 *********************************************************************************************
 *2.130.0    20-May-2021        Mushahid Faizan          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RecipePlanDetailsExcelDTODefination : ComplexAttributeDefinition
    {
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public RecipePlanDetailsExcelDTODefination(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(RecipePlanDetailsExcel))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("PlanDate", "PlanDate", new NullableDateTimeValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("RecipeName", "Recipe Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("PlannedQty", "PlannedQty", new NullableDecimalValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IncrementedQty", "IncrementedQty", new NullableDecimalValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("FinalQty", "FinalQty", new NullableDecimalValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOM", "UOM", new StringValueConverter()));

        }
    }
}

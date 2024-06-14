/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of Tax  Excel DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    14-Oct-2020   Mushahid Faizan Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Product
{
    public class TaxExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public TaxExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(TaxDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxId", "TaxId", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxName", "TaxName", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxPercentage", "TaxPercentage", new NullableDoubleValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ActiveFlag", "ActiveFlag", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", "LastUpdatedBy", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdateDate", "LastUpdateDate", new NullableDateTimeValueConverter()));

            log.LogMethodExit();

        }
    }

}

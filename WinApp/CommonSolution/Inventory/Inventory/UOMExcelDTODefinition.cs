/********************************************************************************************
 * Project Name - Inventory
 * Description  -UOMExcelDTODefinition holds the excel informaton for UOM 
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       24-oct-2020   Girish Kundar       Created : Inventory UI redesign changes
 *********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class UOMExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public UOMExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(UOMDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOMId", "UOMId", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOM", "UOM", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", "LastUpdatedBy", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdateDate", "LastUpdateDate", new NullableDateTimeValueConverter()));

            log.LogMethodExit();


        }
    }
}

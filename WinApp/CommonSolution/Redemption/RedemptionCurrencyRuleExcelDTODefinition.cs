/********************************************************************************************
 * Project Name - Redemption 
 * Description  - RedemptionCurrencyRuleExcelDTODefinition  object of currency rule Excel information
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By             Remarks          
 *********************************************************************************************
 *2.110.o    02-Dec-2020        Mushahid Faizan        Created.
 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleExcelDTODefinition : ComplexAttributeDefinition
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public RedemptionCurrencyRuleExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(RedemptionCurrencyRuleDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("RedemptionCurrencyRuleId", "RedemptionCurrencyRuleId", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("RedemptionCurrencyRuleName", "RedemptionCurrencyRuleName", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Description", "Description", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Amount", "Ticket", new DecimalValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Percentage", "Bonus %", new DecimalValueConverter()));


            attributeDefinitionList.Add(new SimpleAttributeDefinition("Priority", "Priority", new IntValueConverter()));





            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Cumulative", "Cumulative", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", "LastUpdatedBy", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdateDate", "LastUpdateDate", new NullableDateTimeValueConverter()));

            log.LogMethodExit();

        }
    }
}

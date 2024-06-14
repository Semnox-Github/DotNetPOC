/********************************************************************************************
 * Project Name - Customer.Accounts
 * Description  - Class for  of AccountActivityDTODefinition      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Customer DTO definition class
    /// </summary>
    public class AccountActivityDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        /// <param name="ticketAlias"></param>
        public AccountActivityDTODefinition(ExecutionContext executionContext, string fieldName, string ticketAlias) : base(fieldName, typeof(AccountActivityDTO))
        {
            log.LogMethodEntry(executionContext, fieldName, ticketAlias);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Date", MessageContainerList.GetMessage(executionContext, "Date"), new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Product", MessageContainerList.GetMessage(executionContext, "Product"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Amount", MessageContainerList.GetMessage(executionContext, "Amount"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Credits", MessageContainerList.GetMessage(executionContext, "Credits"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Courtesy", MessageContainerList.GetMessage(executionContext, "Courtesy"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Bonus", MessageContainerList.GetMessage(executionContext, "Bonus"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Time", MessageContainerList.GetMessage(executionContext, "Time"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Tokens", MessageContainerList.GetMessage(executionContext, "Tokens"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Tickets", string.IsNullOrWhiteSpace(ticketAlias)? MessageContainerList.GetMessage(executionContext, "Tickets") : ticketAlias, new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LoyaltyPoints", MessageContainerList.GetMessage(executionContext, "Loyalty Points"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Site", MessageContainerList.GetMessage(executionContext, "Site"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("POS", MessageContainerList.GetMessage(executionContext, "POS"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UserName", MessageContainerList.GetMessage(executionContext, "UserName"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Quantity", MessageContainerList.GetMessage(executionContext, "Quantity"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Price", MessageContainerList.GetMessage(executionContext, "Price"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RefId", MessageContainerList.GetMessage(executionContext, "RefId"), new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ActivityType", MessageContainerList.GetMessage(executionContext, "Activity Type"), new StringValueConverter()));
            log.LogMethodExit();
        }
    }
}

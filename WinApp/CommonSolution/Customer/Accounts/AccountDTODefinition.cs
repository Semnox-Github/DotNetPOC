/********************************************************************************************
 * Project Name - Customer.Accounts
 * Description  - Class for  of AccountDTODefinition      
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
    public class AccountDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public AccountDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(AccountDTO))
        {
            log.LogMethodEntry(executionContext,  fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AccountId", MessageContainerList.GetMessage(executionContext, "Card Id"), new ForeignKeyValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TagNumber", MessageContainerList.GetMessage(executionContext, "Card Number"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CustomerName", MessageContainerList.GetMessage(executionContext, "Customer"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IssueDate", MessageContainerList.GetMessage(executionContext, "Issue Date"), new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("FaceValue", MessageContainerList.GetMessage(executionContext, "Deposit"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Credits", MessageContainerList.GetMessage(executionContext, "Credits"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Courtesy", MessageContainerList.GetMessage(executionContext, "Courtesy"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Bonus", MessageContainerList.GetMessage(executionContext, "Bonus"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Time", MessageContainerList.GetMessage(executionContext, "Time"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TicketCount", MessageContainerList.GetMessage(executionContext, "Ticket Count"), new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LoyaltyPoints", MessageContainerList.GetMessage(executionContext, "Loyalty Points"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CreditsPlayed", MessageContainerList.GetMessage(executionContext, "Credits Played"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RealTicketMode", MessageContainerList.GetMessage(executionContext, "Real Ticket Mode"), new BooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VipCustomer", MessageContainerList.GetMessage(executionContext, "Vip Customer"), new BooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TicketAllowed", MessageContainerList.GetMessage(executionContext, "Ticket Allowed"), new BooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TechnicianCard", MessageContainerList.GetMessage(executionContext, "Tech Card?"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TimerResetCard", MessageContainerList.GetMessage(executionContext, "Timer Reset Card"), new BooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TechGames", MessageContainerList.GetMessage(executionContext, "Tech Games"), new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ValidFlag", MessageContainerList.GetMessage(executionContext, "Valid Flag"), new BooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RefundFlag", MessageContainerList.GetMessage(executionContext, "Refund Flag"), new BooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RefundAmount", MessageContainerList.GetMessage(executionContext, "Refund Amount"), new NullableDecimalValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RefundDate", MessageContainerList.GetMessage(executionContext, "Refund Date"), new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ExpiryDate", MessageContainerList.GetMessage(executionContext, "Expiry Date"), new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("StartTime", MessageContainerList.GetMessage(executionContext, "Start Time"), new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastPlayedTime", MessageContainerList.GetMessage(executionContext, "Last Played Time"), new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Notes", MessageContainerList.GetMessage(executionContext, "Notes"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdateDate", MessageContainerList.GetMessage(executionContext, "Last Update Time"), new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", MessageContainerList.GetMessage(executionContext, "Last Updated By"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PrimaryAccount", MessageContainerList.GetMessage(executionContext, "Primary Card"), new BooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AccountIdentifier", MessageContainerList.GetMessage(executionContext, "Card Identifier"), new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("MembershipName", MessageContainerList.GetMessage(executionContext, "Membership"), new StringValueConverter()));
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - TransactionExcelDTODefinition
 * Description  - TransactionExcelDTODefinition
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.90        18-Aug-2019   Vikas          Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public class TransactionHeadersExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TransactionHeadersExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(TransactionDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionId", "ID", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionNumber", "Trx No", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TableNumber", "Table#", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionDate", "Date", new DateTimeValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionAmount", "Amount", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxAmount", "Tax", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionNetAmount", "Net_amount", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Paid", "Paid", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionDiscountPercentage", "avg_disc_%", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PaymentModeName", "pay_mode", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PosMachine", "POS", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UserName", "Cashier", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CustomerName", "Customer_Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CashAmount", "Cash", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CreditCardAmount", "C_C_Amount", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("GameCardAmount", "Game_card_amt", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OtherPaymentModeAmount", "Other_Mode_Amt", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PaymentReference", "Ref", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Status", "Status", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));
            log.LogVariableState("AttributeDefinitionList", attributeDefinitionList);
            log.LogMethodExit();
        }
    }
}

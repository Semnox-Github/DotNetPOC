/********************************************************************************************
 * Project Name - DiscountCouponsExcelDTODefinition
 * Description  - DiscountCouponsExcelDTODefinition
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        19-APR-2019   Raghuveera      Created 
 *2.70.2        31-Jul-2019   Girish Kundar   Added LogMethodEcnty() and LogMethodExit() and Removed Unused namespace's.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    public class DiscountCouponsExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DiscountCouponsExcelDTODefinition(ExecutionContext executionContext, string fieldName, bool isPaymentCoupon, string datetimeFormat="dd-MMM-yyyy") : base(fieldName, typeof(DiscountCouponsDTO))
        {
            log.LogMethodEntry(executionContext, fieldName, isPaymentCoupon, datetimeFormat);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DiscountId", "DiscountId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("FromNumber", "FromNumber", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ToNumber", "ToNumber", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Count", "CouponCount", new IntValueConverter()));
            if(isPaymentCoupon)
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("CouponValue", "CouponValue", new DecimalValueConverter()));
            }
            attributeDefinitionList.Add(new SimpleAttributeDefinition("StartDate", "StartDate", new DateTimeValueConverter(datetimeFormat)));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ExpiryDate", "ExpiryDate", new DateTimeValueConverter(datetimeFormat)));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionId", "TransactionId", new IntValueConverter()));
            log.LogVariableState("AttributeDefinitionList" , attributeDefinitionList);
            log.LogMethodExit();
        }        
    }
}

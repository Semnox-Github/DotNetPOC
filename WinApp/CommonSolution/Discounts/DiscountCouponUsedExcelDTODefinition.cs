/********************************************************************************************
 * Project Name - Discounts
 * Description  - class for DiscountCouponUsedExcelDTODefinition
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
    class DiscountCouponUsedExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DiscountCouponUsedExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(DiscountCouponsUsedDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CouponNumber", "CouponNumber", new StringValueConverter()));
            log.LogMethodExit();
        }
    }
}

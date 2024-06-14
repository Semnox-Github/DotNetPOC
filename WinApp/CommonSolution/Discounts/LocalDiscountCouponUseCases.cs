/********************************************************************************************
 * Project Name - Discounts 
 * Description  - LocalDiscountCouponUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      04-May-2021       Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Discounts
{
    public class LocalDiscountCouponUseCases : LocalUseCases, IDiscountCouponUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LocalDiscountCouponUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<DiscountCouponSummaryDTO> GetDiscountCouponSummary(string couponNumber)
        {
            return await Task<DiscountCouponSummaryDTO>.Factory.StartNew(() =>
            {
                try
                {
                    log.LogMethodEntry(couponNumber);
                    DiscountCouponSummaryDTO discountCouponSummaryDTO = null;
                    DiscountCouponSummaryBL discountCouponSummaryBL = new DiscountCouponSummaryBL(executionContext,couponNumber);
                    discountCouponSummaryBL.GetDiscountCoupons(couponNumber);
                    discountCouponSummaryDTO = discountCouponSummaryBL.GetDiscountCouponSummaryDTO;
                    log.LogMethodEntry(discountCouponSummaryDTO);
                    return discountCouponSummaryDTO;
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    throw valEx;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw ex;
                }
            });
        }
    }
}

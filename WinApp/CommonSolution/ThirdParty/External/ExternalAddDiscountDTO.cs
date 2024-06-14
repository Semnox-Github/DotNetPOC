/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the add Discounts details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   M S Shreyas             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalAddDiscountDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for DiscountName
        /// </summary>
        public string DiscountName { get; set; }
        /// <summary>
        /// Get/Set for Amount
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Get/Set for CouponNumber
        /// </summary>
        public string CouponNumber { get; set; }
        /// <summary>
        /// Get/Set for Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Default constructor 
        /// </summary>
        public ExternalAddDiscountDTO()
        {
            log.LogMethodEntry();
            DiscountName = String.Empty; ;
            CouponNumber = string.Empty;
            Remarks = String.Empty; ;
            log.LogMethodExit();

        }
        /// <summary>
        /// constructor with parameter
        /// </summary>
        public ExternalAddDiscountDTO(string discountName, decimal? amount,string couponNumber,string remarks)
        {
            log.LogMethodEntry(discountName, amount, couponNumber, remarks);
            this.DiscountName = discountName;
            this.Amount = amount;
            this.CouponNumber = couponNumber;        
            this.Remarks = remarks;
            log.LogMethodExit();

        }
    }
}

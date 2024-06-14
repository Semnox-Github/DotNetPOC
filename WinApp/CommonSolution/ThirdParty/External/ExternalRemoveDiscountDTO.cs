/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the remove discount details .
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
    public class ExternalRemoveDiscountDTO

    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for DiscountName
        /// </summary>
        public string DiscountName { get; set; }
        //spelling is same as in pdf DIscount

        /// <summary>
        /// Get/Set for CouponNUmber
        /// </summary>
        public string CouponNumber { get; set; }
        /// <summary>
        /// Get/Set for Remark
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExternalRemoveDiscountDTO()
        {
            log.LogMethodEntry();
            DiscountName = String.Empty;
            CouponNumber = String.Empty;
            Remarks = String.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Parameter
        /// </summary>
        public ExternalRemoveDiscountDTO(string discountName, string couponNumber, string remarks)
        {
            log.LogMethodEntry(discountName, couponNumber, remarks);
            this.DiscountName = discountName;
            this.CouponNumber = couponNumber;
            this.Remarks = remarks;
            log.LogMethodExit();

        }

    }
}

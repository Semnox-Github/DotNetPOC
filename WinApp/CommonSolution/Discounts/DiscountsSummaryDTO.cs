/********************************************************************************************
 * Project Name - Discounts
 * Description  - class for DiscountsSummaryDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Girish Kundar   Added LogMethodEcnty() and LogMethodExit() and Removed Unused namespace's.
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the Discounts summary object class. This is used to summarize the discount across lines. 
    /// </summary>
    public class DiscountsSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set method of the DiscountId field
        /// </summary>
        public int DiscountId
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the DiscountPercentage field
        /// </summary>
        public decimal DiscountPercentage
        {
            get;set;
        }

        /// <summary>
        /// Get/Set method of the DiscountName field
        /// </summary>
        public string DiscountName
        {
            get;set;
        }

        /// <summary>
        /// Get/Set method of the DiscountAmount field
        /// </summary>
        public decimal DiscountAmount
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the Count field
        /// </summary>
        public int Count
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the VariableDiscountAmount field
        /// </summary>
        public decimal VariableDiscountAmount
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the DisplayChar field
        /// </summary>
        public string DisplayChar
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the DisplayChar field
        /// </summary>
        public decimal TotalLineAmount
        {
            get; set;
        }


        /// <summary>
        /// Get/Set method of the ApprovedBy field
        /// </summary>
        public int ApprovedBy
        {
            get; set;
        }


        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the TotalNoOfLines field
        /// </summary>
        public int TotalNoOfLines
        {
            get; set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountsSummaryDTO()
        {
            log.LogMethodEntry();
            DisplayChar = "*";
            DiscountName = "";
            CouponNumbers = new HashSet<string>();
            log.LogMethodExit();
        }

        public HashSet<string> CouponNumbers
        {
            get;set;
        }
    }
}

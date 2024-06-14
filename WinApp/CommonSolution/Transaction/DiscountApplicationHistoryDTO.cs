//using Semnox.Parafait.TransactionLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Stores the information of discount applied on a transaction.
    /// </summary>
    public class DiscountApplicationHistoryDTO
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
        /// Get/Set method of the CouponNumber field
        /// </summary>
        public string CouponNumber
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the VariableDiscountAmount field
        /// </summary>
        public decimal? VariableDiscountAmount
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
        /// Get/Set method of the TransactionLineBL field
        /// </summary>
        public Transaction.TransactionLine TransactionLineBL
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the IsCancelled field, used only in web
        /// </summary>
        public bool IsCancelled
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set method of the TransactionLineDTO field, used only in web
        /// </summary>
        public string TransactionLineGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DiscountApplicationHistoryDTO()
        {
            log.LogMethodEntry();

            log.LogMethodExit(null);
        }
    }
}

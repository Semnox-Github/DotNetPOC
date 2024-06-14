/********************************************************************************************
 * Project Name - Customer 
 * Description  - AccountCreditPlusSummaryList Class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks          
 *********************************************************************************************
 **2.150.02   21-Mar-2023      Yashodhara C H         Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountCreditPlusSummaryList Class
    /// </summary>
    public class AccountSummaryOptions
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountSummaryOptions(ExecutionContext executionContext,
                                     DateTime? fromDate, DateTime? toDate, string creditPlusType, bool showExpiryEntitlements)
        {
            log.LogMethodEntry(executionContext, fromDate, toDate, creditPlusType, showExpiryEntitlements);
            this.executionContext = executionContext;
            FromDate = fromDate;
            ToDate = toDate;
            CreditPlusType = string.IsNullOrWhiteSpace(creditPlusType) == false ? CreditPlusTypeConverter.FromString(creditPlusType) : (CreditPlusType?) null;
            ShowExpiryEntitlements = showExpiryEntitlements;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method for PeriodFrom
        /// </summary>
        public DateTime? FromDate { get; set; }
        
        /// <summary>
        /// Get/Set method for PeriodTo
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Get/Set method for CreditPlusType
        /// </summary>
        public CreditPlusType? CreditPlusType { get; set; }

        /// <summary>
        /// Get/Set method for CreditPlusType
        /// </summary>
        public bool ShowExpiryEntitlements { get; set; }

    }
}

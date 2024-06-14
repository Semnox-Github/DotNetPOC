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
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountCreditPlusSummaryList Class
    /// </summary>
    public class AccountCreditPlusSummaryCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private AccountSummaryOptions accountSummaryOptions;
        private List<AccountCreditPlusSummaryBL> accountCreditPlusSummaryBLList = new List<AccountCreditPlusSummaryBL>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountCreditPlusSummaryCollection(ExecutionContext executionContext, 
                                                  AccountBL accountBL, 
                                                  AccountSummaryOptions accountSummaryOptions)
            :this(executionContext, accountBL.GetAccountCreditPlusSummaryBLList(accountSummaryOptions), accountSummaryOptions)
        {
            log.LogMethodEntry(executionContext, accountBL, accountSummaryOptions);
            log.LogMethodExit();
        }

        public AccountCreditPlusSummaryCollection(ExecutionContext executionContext,
                                                  List<AccountCreditPlusSummaryBL> parameterAccountCreditPlusSummaryBLList,
                                                  AccountSummaryOptions accountSummaryOptions)
        {
            log.LogMethodEntry(executionContext, parameterAccountCreditPlusSummaryBLList, accountSummaryOptions);
            this.executionContext = executionContext;
            this.accountSummaryOptions = accountSummaryOptions;
            foreach(var accountCreditPlusSummaryBL in parameterAccountCreditPlusSummaryBLList)
            {
                accountCreditPlusSummaryBLList.Add(accountCreditPlusSummaryBL);
            }
            log.LogMethodExit();
        }

        public List<AccountCreditPlusSummaryBL> AccountCreditPlusSummaryBLList
        {
            get
            {
                return accountCreditPlusSummaryBLList;
            }
        }

        public IEnumerable<AccountCreditPlusSummaryBL> AccountCreditPlusSummaryBLSequenctialList
        {
            get
            {
                return accountCreditPlusSummaryBLList.OrderBy(x => (x.PeriodFrom == null) ? DateTime.MinValue : x.PeriodFrom).OrderBy(x => x.AccountCreditPlusId);
            }
        }
    }
}

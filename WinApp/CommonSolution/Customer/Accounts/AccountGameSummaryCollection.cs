/********************************************************************************************
 * Project Name - 
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks          
 *********************************************************************************************
 **2.150.02   21-Mar-23    Yashodhara C H
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Manages the list of CardGameMetricView
    /// </summary>
    public class AccountGameSummaryCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private AccountSummaryOptions accountSummaryOptions;
        private List<AccountGameSummaryBL> accountGameSummaryBLList = new List<AccountGameSummaryBL>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountGameSummaryCollection(ExecutionContext executionContext,
                                                 AccountBL accountBL,
                                                 AccountSummaryOptions accountSummaryOptions)
           : this(executionContext, accountBL.GetAccountGameSummaryBLList(accountSummaryOptions), accountSummaryOptions)
        {
            log.LogMethodEntry(executionContext, accountBL, accountSummaryOptions);
            log.LogMethodExit();
        }

        public AccountGameSummaryCollection(ExecutionContext executionContext,
                                                  List<AccountGameSummaryBL> parameterAccountGameSummaryBLList,
                                                  AccountSummaryOptions accountSummaryOptions)
        {
            log.LogMethodEntry(executionContext, parameterAccountGameSummaryBLList, accountSummaryOptions);
            this.executionContext = executionContext;
            this.accountSummaryOptions = accountSummaryOptions;
            foreach (var accountGameSummaryBL in parameterAccountGameSummaryBLList)
            {
                accountGameSummaryBLList.Add(accountGameSummaryBL);
            }
            log.LogMethodExit();
        }

        public List<AccountGameSummaryBL> AccountGameSummaryBLList
        {
            get
            {
                return accountGameSummaryBLList;
            }
        }

        public IEnumerable<AccountGameSummaryBL> AccountGameSummaryBLSequenctialList
        {
            get
            {
                return accountGameSummaryBLList.OrderBy(x => (x.FromDate == null) ? DateTime.MinValue : x.FromDate).OrderBy( x => x.AccountGameId);
            }
        }
    }
}

/********************************************************************************************
 * Project Name - AcconuntDiscountsSummaryViewBL
 * Description  - Data object of the AccountDiscountsSummaryViewBL
 * 
 **************
 **Version Log
 **************
 *Version      Date           Modified By        Remarks          
 *********************************************************************************************
 *2.130.11   07-Sep-2022     Yashodhara C H      Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// AccountDiscountsSummaryViewBL class
    /// </summary>
    public class AccountDiscountsSummaryViewBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AccountDiscountsSummaryViewDTO accountDiscountsSummaryViewDTO;

        /// <summary>
        /// Parameterized constructor of AccountDiscountsSummaryViewBL class
        /// </summary>
        private AccountDiscountsSummaryViewBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountDiscount id as the parameter
        /// Would fetch the accountDiscountsSummaryView object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountDiscountsSummaryViewBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AccountDiscountsSummaryViewDataHandler accountDiscountsSummaryDataHandler = new AccountDiscountsSummaryViewDataHandler(sqlTransaction);
            accountDiscountsSummaryViewDTO = accountDiscountsSummaryDataHandler.GetAccountDiscountsSummaryViewDTO(id);
            if (accountDiscountsSummaryViewDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountDiscount", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with accountDiscountsSummaryViewDTO as parameter
        /// </summary>
        /// <param name="accountDiscountsSummaryViewDTO"></param>
        private AccountDiscountsSummaryViewBL(ExecutionContext executionContext, AccountDiscountsSummaryViewDTO accountDiscountsSummaryViewDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountDiscountsSummaryViewDTO = accountDiscountsSummaryViewDTO;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of AccountDiscountsSummaryView
    /// </summary>
    public class AccountDiscountsSummaryViewListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountDiscountsSummaryViewListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AccountSummaryListDiscountsView list
        /// </summary>
        public List<AccountDiscountsSummaryViewDTO> GetAccountDiscountsSummaryViewDTOList(List<KeyValuePair<AccountDiscountsSummaryViewDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountDiscountsSummaryViewDataHandler accountDiscountsSummaryDataHandler = new AccountDiscountsSummaryViewDataHandler(sqlTransaction);
            List<AccountDiscountsSummaryViewDTO> returnValue = accountDiscountsSummaryDataHandler.GetAccountDiscountSummaryViewDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

    }
}

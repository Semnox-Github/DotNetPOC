/********************************************************************************************
 * Project Name - AccountGamesSummaryViewBL
 * Description  - Data object of the AccountGamesSummaryViewBL
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
    /// AccountGamesSummaryViewBL Class
    /// </summary>
    public class AccountGamesSummaryViewBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AccountGamesSummaryViewDTO accountGamesSummaryViewDTO;

        /// <summary>
        /// Parameterized constructor of AccountGamesSummaryViewBL class
        /// </summary>
        private AccountGamesSummaryViewBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountGame id as the parameter
        /// Would fetch the accountGamesSummaryView object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountGamesSummaryViewBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AccountGamesSummaryViewDataHandler accountGamesSummaryViewDataHandler = new AccountGamesSummaryViewDataHandler(sqlTransaction);
            accountGamesSummaryViewDTO = accountGamesSummaryViewDataHandler.GetAccountGamesSummaryViewDTO(id);
            if (accountGamesSummaryViewDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountGamesSummary", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with accountGamesSummaryViewDTO as parameter
        /// </summary>
        /// <param name="accountGamesSummaryViewDTO"></param>
        private AccountGamesSummaryViewBL(ExecutionContext executionContext, AccountGamesSummaryViewDTO accountGamesSummaryViewDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountGamesSummaryViewDTO = accountGamesSummaryViewDTO;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of AccountGamesSummaryViewBL
    /// </summary>
    public class AccountGamesSummaryViewListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountGamesSummaryViewListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AccountGamesSummaryView list
        /// </summary>
        public List<AccountGamesSummaryViewDTO> GetAccountGamesSummaryViewDTOList(List<KeyValuePair<AccountGamesSummaryViewDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountGamesSummaryViewDataHandler accountGamesSummaryViewDataHandler = new AccountGamesSummaryViewDataHandler(sqlTransaction);
            List<AccountGamesSummaryViewDTO> accountGamesSummaryViewDTOList = accountGamesSummaryViewDataHandler.GetAccountGamesSummaryViewDTOs(searchParameters);
            log.LogMethodExit(accountGamesSummaryViewDTOList);
            return accountGamesSummaryViewDTOList;
        }

    }
}


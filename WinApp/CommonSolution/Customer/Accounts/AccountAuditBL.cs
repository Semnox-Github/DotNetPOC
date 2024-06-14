
/********************************************************************************************
 * Project Name - Device
 * Description  - Bowa Pegas Fiscal Printer
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.80.0      21-May-2020      Girish Kundar  Modified : Made default constructor as Private
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic for AccountAudit class.
    /// </summary>
    public class AccountAuditBL
    {
        AccountAuditDTO accountAuditDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountAuditBL class
        /// </summary>
        private AccountAuditBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AccountAuditBL object using the AccountAuditDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountAuditDTO">AccountAuditDTO object</param>
        public AccountAuditBL(ExecutionContext executionContext, AccountAuditDTO accountAuditDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountAuditDTO);
            this.accountAuditDTO = accountAuditDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AccountAudit
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AccountAuditDataHandler accountAuditDataHandler = new AccountAuditDataHandler(sqlTransaction);
            if (accountAuditDTO.AccountAuditId < 0)
            {
                accountAuditDTO = accountAuditDataHandler.InsertAccountAudit(accountAuditDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                accountAuditDTO.AcceptChanges();
            }
            else
            {
                throw new InvalidOperationException();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountAuditDTO AccountAuditDTO
        {
            get
            {
                return accountAuditDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of AccountAudit
    /// </summary>
    public class AccountAuditListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountAuditListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountAudit list
        /// </summary>
        public List<AccountAuditDTO> GetAccountAuditDTOList(List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountAuditDataHandler accountAuditDataHandler = new AccountAuditDataHandler(sqlTransaction);
            List<AccountAuditDTO> returnValue = accountAuditDataHandler.GetAccountAuditDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the no of accountAudits matching the search criteria
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optianal sql transaction</param>
        /// <returns></returns>
        public int GetAccountAuditCount(List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AccountAuditDataHandler accountAuditDataHandler = new AccountAuditDataHandler(sqlTransaction);
            int accountAuditCount = accountAuditDataHandler.GetAccountAuditCount(searchParameters);
            log.LogMethodExit(accountAuditCount);
            return accountAuditCount;
        }

        /// <summary>
        /// Returns the AccountAudit list
        /// </summary>
        public List<AccountAuditDTO> GetAccountAuditDTOList(List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> searchParameters,
            int pageNumber, int pageSize, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, pageNumber, pageSize, sqlTransaction);
            AccountAuditDataHandler accountAuditDataHandler = new AccountAuditDataHandler(sqlTransaction);
            List<AccountAuditDTO> accountAuditDTOList = accountAuditDataHandler.GetAccountAuditDTOList(searchParameters, pageNumber, pageSize);
            log.LogMethodExit(accountAuditDTOList);
            return accountAuditDTOList;
        }
    }
}

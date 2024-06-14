/********************************************************************************************
 * Project Name - AccountCreditPlusPurchaseCriteriaBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.70.2      02-Aug-2019        Girish Kundar      Removed Unused Namespace.
 *2.80.0      21-May-2020        Girish Kundar      Modified : Made default constructor as Private   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic for AccountCreditPlusPurchaseCriteria class.
    /// </summary>
    public class AccountCreditPlusPurchaseCriteriaBL
    {
        private AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountCreditPlusPurchaseCriteriaBL class
        /// </summary>
        private AccountCreditPlusPurchaseCriteriaBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountCreditPlusPurchaseCriteria id as the parameter
        /// Would fetch the accountCreditPlusPurchaseCriteria object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountCreditPlusPurchaseCriteriaBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AccountCreditPlusPurchaseCriteriaDataHandler accountCreditPlusPurchaseCriteriaDataHandler = new AccountCreditPlusPurchaseCriteriaDataHandler(sqlTransaction);
            accountCreditPlusPurchaseCriteriaDTO = accountCreditPlusPurchaseCriteriaDataHandler.GetAccountCreditPlusPurchaseCriteriaDTO(id);
            if (accountCreditPlusPurchaseCriteriaDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountCreditPlusPurchaseCriteria", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AccountCreditPlusPurchaseCriteriaBL object using the AccountCreditPlusPurchaseCriteriaDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountCreditPlusPurchaseCriteriaDTO">AccountCreditPlusPurchaseCriteriaDTO object</param>
        public AccountCreditPlusPurchaseCriteriaBL(ExecutionContext executionContext, AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountCreditPlusPurchaseCriteriaDTO);
            this.accountCreditPlusPurchaseCriteriaDTO = accountCreditPlusPurchaseCriteriaDTO;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Saves the AccountCreditPlusPurchaseCriteria
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(int parentSiteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            AccountCreditPlusPurchaseCriteriaDataHandler accountCreditPlusPurchaseCriteriaDataHandler = new AccountCreditPlusPurchaseCriteriaDataHandler(sqlTransaction);
            if (accountCreditPlusPurchaseCriteriaDTO.IsChanged)
            {
                if (accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusPurchaseCriteriaId < 0)
                {
                    accountCreditPlusPurchaseCriteriaDTO = accountCreditPlusPurchaseCriteriaDataHandler.InsertAccountCreditPlusPurchaseCriteria(accountCreditPlusPurchaseCriteriaDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    accountCreditPlusPurchaseCriteriaDTO.AcceptChanges();
                }
                else
                {
                    if (accountCreditPlusPurchaseCriteriaDTO.IsChanged)
                    {
                        accountCreditPlusPurchaseCriteriaDTO = accountCreditPlusPurchaseCriteriaDataHandler.UpdateAccountCreditPlusPurchaseCriteria(accountCreditPlusPurchaseCriteriaDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        accountCreditPlusPurchaseCriteriaDTO.AcceptChanges();
                    }
                }
                CreateRoamingData(parentSiteId, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountCreditPlusPurchaseCriteriaDTO AccountCreditPlusPurchaseCriteriaDTO
        {
            get
            {
                return accountCreditPlusPurchaseCriteriaDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private void CreateRoamingData(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            if (parentSiteId > -1 && parentSiteId != accountCreditPlusPurchaseCriteriaDTO.SiteId && executionContext.GetSiteId() > -1
                    && accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusPurchaseCriteriaId > -1)
            {
                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", accountCreditPlusPurchaseCriteriaDTO.Guid, "CardCreditPlusPurchaseCriteria", DateTime.Now, parentSiteId);
                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                dBSynchLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of AccountCreditPlusPurchaseCriteria
    /// </summary>
    public class AccountCreditPlusPurchaseCriteriaListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountCreditPlusPurchaseCriteriaListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountCreditPlusPurchaseCriteria list
        /// </summary>
        public List<AccountCreditPlusPurchaseCriteriaDTO> GetAccountCreditPlusPurchaseCriteriaDTOList(List<KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountCreditPlusPurchaseCriteriaDataHandler accountCreditPlusPurchaseCriteriaDataHandler = new AccountCreditPlusPurchaseCriteriaDataHandler(sqlTransaction);
            List<AccountCreditPlusPurchaseCriteriaDTO> returnValue = accountCreditPlusPurchaseCriteriaDataHandler.GetAccountCreditPlusPurchaseCriteriaDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the AccountCreditPlusPurchaseCriteria list
        /// </summary>
        public List<AccountCreditPlusPurchaseCriteriaDTO> GetAccountCreditPlusPurchaseCriteriaDTOListByAccountIds(List<int> accountIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountIdList, sqlTransaction);
            AccountCreditPlusPurchaseCriteriaDataHandler accountCreditPlusPurchaseCriteriaDataHandler = new AccountCreditPlusPurchaseCriteriaDataHandler(sqlTransaction);
            List<AccountCreditPlusPurchaseCriteriaDTO> returnValue = accountCreditPlusPurchaseCriteriaDataHandler.GetAccountCreditPlusPurchaseCriteriaDTOListByAccountIdList(accountIdList);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}

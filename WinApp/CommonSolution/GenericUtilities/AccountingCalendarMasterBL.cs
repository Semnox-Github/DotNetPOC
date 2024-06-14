/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - BL Logic of Accounting Calendar Master
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       30-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Accounting Calendar Master BL
    /// </summary>
    public class AccountingCalendarMasterBL
    {
        private AccountingCalendarMasterDTO accountingCalendarMasterDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountingCalendarMasterBL class
        /// </summary>
        private AccountingCalendarMasterBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates AccountingCalendarMasterBL object using the AccountingCalendarMasterDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountingCalendarMasterDTO">accountingCalendarMasterDTO DTO object</param>
        public AccountingCalendarMasterBL(ExecutionContext executionContext,
                                          AccountingCalendarMasterDTO accountingCalendarMasterDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountingCalendarMasterDTO);
            this.accountingCalendarMasterDTO = accountingCalendarMasterDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AccountingCalendarMaster  id as the parameter
        /// Would fetch the AccountingCalendarMaster object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -AccountingCalendarMaster </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AccountingCalendarMasterBL(ExecutionContext executionContext, int accountingCalendarMasteId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountingCalendarMasteId, sqlTransaction);
            AccountingCalendarMasterDataHandler accountingCalendarMasterDataHandler = new AccountingCalendarMasterDataHandler(sqlTransaction);
            accountingCalendarMasterDTO = accountingCalendarMasterDataHandler.GetccountingCalendarMasterId(accountingCalendarMasteId);
            if (accountingCalendarMasterDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountingCalendarMasterDTO", accountingCalendarMasteId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AccountingCalendarMaster DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (accountingCalendarMasterDTO.IsChanged == false
                && accountingCalendarMasterDTO.AccountingCalendarMasterId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AccountingCalendarMasterDataHandler accountingCalendarMasterDataHandler = new AccountingCalendarMasterDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (accountingCalendarMasterDTO.AccountingCalendarMasterId < 0)
            {
                accountingCalendarMasterDTO = accountingCalendarMasterDataHandler.Insert(accountingCalendarMasterDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                accountingCalendarMasterDTO.AcceptChanges();
            }
            else if (accountingCalendarMasterDTO.IsChanged)
            {
                accountingCalendarMasterDTO = accountingCalendarMasterDataHandler.Update(accountingCalendarMasterDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                accountingCalendarMasterDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AccountingCalendarMasterDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the AccountingCalendarMasterDTO
        /// </summary>
        public AccountingCalendarMasterDTO AccountingCalendarMasterDTO
        {
            get
            {
                return accountingCalendarMasterDTO;
            }
        }
    }

    /// <summary>
    /// Accounting Calendar Master List BL
    /// </summary>
    public class AccountingCalendarMasterListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList = new List<AccountingCalendarMasterDTO>();

        /// <summary>
        /// Parameterized constructor of AccountingCalendarMasterListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public AccountingCalendarMasterListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="accountingCalendarMasterDTOList">AccountingCalendarMaster DTO List as parameter </param>
        public AccountingCalendarMasterListBL(ExecutionContext executionContext,
                                               List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountingCalendarMasterDTOList);
            this.accountingCalendarMasterDTOList = accountingCalendarMasterDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the AccountingCalendarMaster DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of AccountingCalendarMasterDTO </returns>
        public List<AccountingCalendarMasterDTO> GetAccountingCalendarMasterDTOList(List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AccountingCalendarMasterDataHandler accountingCalendarMasterDataHandler = new AccountingCalendarMasterDataHandler(sqlTransaction);
            List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList = accountingCalendarMasterDataHandler.GetAccountingCalendarMasterDTOList(searchParameters);
            log.LogMethodExit(accountingCalendarMasterDTOList);
            return accountingCalendarMasterDTOList;
        }

        /// <summary>
        /// Saves the  list of AccountingCalendarMaster DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (accountingCalendarMasterDTOList == null ||
                accountingCalendarMasterDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < accountingCalendarMasterDTOList.Count; i++)
            {
                AccountingCalendarMasterDTO accountingCalendarMasterDTO = accountingCalendarMasterDTOList[i];
                if (accountingCalendarMasterDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AccountingCalendarMasterBL accountingCalendarMasterBL = new AccountingCalendarMasterBL(executionContext, accountingCalendarMasterDTO);
                    accountingCalendarMasterBL.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AccountingCalendarMasterDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AccountingCalendarMasterDTO", accountingCalendarMasterDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}

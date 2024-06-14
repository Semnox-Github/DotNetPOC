
/********************************************************************************************
 * Project Name - AccountingCodeCombination
 * Description  - Bussiness logic of AccountingCodeCombination
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Dec-2016   Amaresh      Created 
 *2.70.0      20-Jun-2019   Nagesh Badiger       Added DeleteAccountingCode() and DeleteAccountingCodeList() methods.
 *2.110.0     16-Oct-2020   Mushahid Faizan      Added GetAccountingCodeDTOList().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// AccountingCodeCombination allowes to access the AccountingCodeCombination details based on the bussiness logic.
    /// </summary>
    public class AccountingCodeCombination
    {
        private AccountingCodeCombinationDTO accountingCodeCombinationDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        public AccountingCodeCombination(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountingCodeCombinationDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Id parameter
        /// </summary>
        /// <param name="id">id</param>
        public AccountingCodeCombination(int id)
        {
            log.LogMethodEntry(id);
            this.executionContext = ExecutionContext.GetExecutionContext();
            AccountingCodeCombinationDataHandler accountingCodeCombinationDataHandler = new AccountingCodeCombinationDataHandler(null);
            this.accountingCodeCombinationDTO = accountingCodeCombinationDataHandler.GetAccountingCode(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountingCodeCombinationDTO"></param>
        public AccountingCodeCombination(ExecutionContext executionContext, AccountingCodeCombinationDTO accountingCodeCombinationDTO)
        {
            log.LogMethodEntry(accountingCodeCombinationDTO, executionContext);
            this.accountingCodeCombinationDTO = accountingCodeCombinationDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// get AccountingCodeCombinationDTO Object
        /// </summary>
        public AccountingCodeCombinationDTO GetAccountingCodeCombinationDTO
        {
            get { return accountingCodeCombinationDTO; }
        }


        /// <summary>
        /// Saves the AccountingCodeCombination
        /// Checks if the id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            AccountingCodeCombinationDataHandler AccountingCodeCombinationDataHandler = new AccountingCodeCombinationDataHandler(sqlTransaction);
            if (accountingCodeCombinationDTO.Id < 0)
            {
                int id = AccountingCodeCombinationDataHandler.InsertAccountingCodeCombination(accountingCodeCombinationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                accountingCodeCombinationDTO.Id = id;
            }
            else
            {
                if (accountingCodeCombinationDTO.IsChanged == true)
                {
                    AccountingCodeCombinationDataHandler.UpdateAccountingCodeCombination(accountingCodeCombinationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    accountingCodeCombinationDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }
        /// <summary>
        /// Deletes the AccountingCode details based on Id
        /// </summary>
        /// <param name="Id">Id</param>        
        public void DeleteAccountingCode(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            try
            {
                AccountingCodeCombinationDataHandler accountingCodeCombinationDataHandler = new AccountingCodeCombinationDataHandler(sqlTransaction);
                accountingCodeCombinationDataHandler.DeleteAccountingCode(id);
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of AccountingCodeCombination
    /// </summary>
    public class AccountingCodeCombinationList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AccountingCodeCombinationDTO> accountingCodeCombinationList = new List<AccountingCodeCombinationDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        public AccountingCodeCombinationList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountingCodeCombinationList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="accountingCodeCombinationList"></param>
        /// <param name="executionContext"></param>
        public AccountingCodeCombinationList(ExecutionContext executionContext, List<AccountingCodeCombinationDTO> accountingCodeCombinationList)
        {
            log.LogMethodEntry(accountingCodeCombinationList, executionContext);
            this.executionContext = executionContext;
            this.accountingCodeCombinationList = accountingCodeCombinationList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountingCodeCombination list
        /// </summary>
        public List<AccountingCodeCombinationDTO> GetAllAccountingCode(List<KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountingCodeCombinationDataHandler accountingCodeCombinationDataHandler = new AccountingCodeCombinationDataHandler(sqlTransaction);
            log.LogMethodExit();
            return accountingCodeCombinationDataHandler.GetAccountingCodeList(searchParameters);
        }

        /// <summary>
        /// Gets the AccountingCodeCombinationDTO List for categoryIdList
        /// </summary>
        /// <param name="categoryIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of AccountingCodeCombinationDTO</returns>
        public List<AccountingCodeCombinationDTO> GetAccountingCodeDTOList(List<int> categoryIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(categoryIdList, activeRecords);
            AccountingCodeCombinationDataHandler accountingCodeCombinationDataHandler = new AccountingCodeCombinationDataHandler(sqlTransaction);
            this.accountingCodeCombinationList = accountingCodeCombinationDataHandler.GetAccountingCodeDTOList(categoryIdList, activeRecords, sqlTransaction);
            log.LogMethodExit(accountingCodeCombinationList);
            return accountingCodeCombinationList;
        }
        /// <summary>
        ///  Save Update Method AccountingCodeList
        /// </summary>
        public void SaveUpdateAccountingCodeList(SqlTransaction sqlTransaction=null)
        {
            try
            {
                log.LogMethodEntry();
                if (accountingCodeCombinationList != null)
                {
                    foreach (AccountingCodeCombinationDTO accountingCodeCombinationDto in accountingCodeCombinationList)
                    {
                        AccountingCodeCombination accountingCodeCombination = new AccountingCodeCombination(executionContext, accountingCodeCombinationDto);
                        accountingCodeCombination.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Hard Deletions for Accounting Code List
        /// </summary>
        public void DeleteAccountingCodeList()
        {
            log.LogMethodEntry();
            if (accountingCodeCombinationList != null && accountingCodeCombinationList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (AccountingCodeCombinationDTO accountingCodeCombinationDTO in accountingCodeCombinationList)
                    {
                        if (accountingCodeCombinationDTO.IsChanged && accountingCodeCombinationDTO.IsActive == false)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AccountingCodeCombination accountingCodeCombination = new AccountingCodeCombination(executionContext);
                                accountingCodeCombination.DeleteAccountingCode(accountingCodeCombinationDTO.Id, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}

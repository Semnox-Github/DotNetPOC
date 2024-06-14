/********************************************************************************************
 * Project Name - AccountService
 * Description  - Load Tickets to card
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.70.3     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This class provides the function to link the accounts to the customer
    /// </summary>
    public class LinkAccountBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private AccountServiceDTO accountServiceDTO;
        private Utilities utilities;
        private LinkAccountBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterzed constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountServiceDTO"></param>
        public LinkAccountBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountServiceDTO = accountServiceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Link all accounts
        /// </summary>
        public void LinkAccounts(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SqlConnection sqlConnection = null;
            SqlTransaction parafaitDBTrx;
            if (sqlTransaction == null)
            {
                sqlConnection = utilities.createConnection();
                parafaitDBTrx = sqlConnection.BeginTransaction();
            }
            else
            {
                parafaitDBTrx = sqlTransaction;
            }
            try
            {
                string message = string.Empty;
                AccountBL accountBL = new AccountBL(executionContext, accountServiceDTO.SourceAccountDTO.AccountId, true, true, parafaitDBTrx);
                if (accountBL.AccountDTO != null && accountServiceDTO.AccountDTOList.Count > 0)
                {
                    if (accountBL.AccountDTO.AccountRelationshipDTOList == null)
                    {
                        accountBL.AccountDTO.AccountRelationshipDTOList = new List<AccountRelationshipDTO>();
                    }
                    foreach (AccountDTO tempAccount in accountServiceDTO.AccountDTOList)
                    {
                        tempAccount.PrimaryAccount = false;
                        AccountRelationshipDTO accountRelationshipDTO = new AccountRelationshipDTO();
                        accountRelationshipDTO.RelatedAccountId = tempAccount.AccountId; // child account Id
                        accountRelationshipDTO.AccountId = accountBL.AccountDTO.AccountId;
                        accountBL.AccountDTO.AccountRelationshipDTOList.Add(accountRelationshipDTO);
                    }
                    accountBL = new AccountBL(executionContext, accountBL.AccountDTO);
                    accountBL.Save(parafaitDBTrx);
                    log.LogMethodExit();
                }
                else
                {
                    log.Error("Invalid source account");
                    throw new ValidationException("Invalid source account");
                }
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
            }

            catch (Exception ex)
            {
                if (sqlTransaction == null)  //SQLTransaction handled locally
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                throw new Exception(ex.Message);

            }
        }
    }
}

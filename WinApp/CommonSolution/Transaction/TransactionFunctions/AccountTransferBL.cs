/********************************************************************************************
 * Project Name - AccountService
 * Description  - TransferAccountBL
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
namespace Semnox.Parafait.Transaction
{

    /// <summary>
    /// TransferAccountBL implements the transfer functionalities /amount/credits
    /// </summary>
    public class AccountTransferBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities = null;
        private AccountServiceDTO accountServiceDTO;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private AccountTransferBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountServiceDTO"></param>
        public AccountTransferBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountServiceDTO = accountServiceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// TransferAccount method
        /// </summary>
        public void TransferAccounts(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);        // destinationAccountDTO should be new card
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
                HelperClassBL.SetParafairEnvValues(executionContext, utilities);
                TaskProcs taskProcs = new TaskProcs(utilities);
                AccountDTO destinationAccountDTO = accountServiceDTO.DestinationAccountDTO;
                AccountDTO sourceAccountDTO = accountServiceDTO.SourceAccountDTO;

                Card currentCard = new Card(sourceAccountDTO.AccountId, executionContext.GetUserId(), utilities, parafaitDBTrx);
                Card transferToCard = new Card(destinationAccountDTO.AccountId, executionContext.GetUserId(), utilities, parafaitDBTrx);
                transferToCard.CardNumber = destinationAccountDTO.TagNumber;
                if (accountServiceDTO != null)
                {
                    if (!taskProcs.transferCard(currentCard, transferToCard, accountServiceDTO.Remarks, ref message, parafaitDBTrx))
                    {
                        log.Error("TRANSFERCARD- unable to transfer from (" + currentCard + ") To (" + transferToCard + ") as error " + message);
                        throw new Exception(message);
                    }
                    else
                    {
                        log.Debug("Transfer Success");
                        message = "Transfer card success";
                    }
                }
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
                log.LogMethodExit();
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

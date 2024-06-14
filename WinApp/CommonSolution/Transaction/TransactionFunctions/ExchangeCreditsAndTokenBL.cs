/********************************************************************************************
 * Project Name - AccountService
 * Description  - Load Tickets to card
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.3     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Class is used as Use case for exchange credits and tokens visa-versa
    /// </summary>
    public class ExchangeCreditsAndTokenBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private AccountServiceDTO accountServiceDTO;

        /// <summary>
        ///  default Constructor 
        /// </summary>
        /// <param name="executionContext"></param>
        private ExchangeCreditsAndTokenBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountServiceDTO"></param>
        public ExchangeCreditsAndTokenBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountServiceDTO = accountServiceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Exchange method 
        /// </summary>
        public void Exchange(SqlTransaction sqlTransaction = null)
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
                HelperClassBL.SetParafairEnvValues(executionContext, utilities);
                TaskProcs taskProcs = new TaskProcs(utilities);
                Card currentCard = new Card(accountServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities, parafaitDBTrx);
                if (accountServiceDTO.ExchangeCreditsForToken)
                {
                    if (!taskProcs.exchangeCreditForToken(currentCard, accountServiceDTO.Tokens, Convert.ToDouble(accountServiceDTO.Credits), accountServiceDTO.Remarks, ref message))
                    {
                        log.Error("EXCHANGECREDITFORTOKEN-  has error " + message);
                        throw new Exception(message);
                    }
                    else
                    {
                        log.Info("EXCHANGECREDITFORTOKEN-  Credits exchanged successfully");
                        log.LogMethodExit();
                    }
                }
                else
                {
                    if (!taskProcs.exchangeTokenForCredit(currentCard, accountServiceDTO.Tokens, Convert.ToDouble(accountServiceDTO.Credits), accountServiceDTO.Remarks, ref message, accountServiceDTO.TrxId))
                    {
                        log.Error("EXCHANGETOKENFORCREDIT-  has error " + message);
                        throw new Exception(message);
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
                log.Error(ex.Message);
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

/********************************************************************************************
 * Project Name - AccountService
 * Description  - Exchange Credits And Time
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
    /// Class for exchange credits to time and time to credits
    /// </summary>
    public class ExchangeCreditsAndTimeBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private AccountServiceDTO accountServiceDTO;
        private ExchangeCreditsAndTimeBL(ExecutionContext executionContext)
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
        public ExchangeCreditsAndTimeBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountServiceDTO = accountServiceDTO;
            log.LogMethodExit();

        }


        /// <summary>
        /// Method to excahge credits to time or time to credits
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
                Card updateCard = new Card(accountServiceDTO.SourceAccountDTO.TagNumber, executionContext.GetUserId(), utilities, parafaitDBTrx);
                bool succTransfer = true;
                if (accountServiceDTO.ExchangeCreditsForTime)
                {
                    succTransfer = taskProcs.ConvertCreditsForTime(updateCard, Convert.ToDouble(accountServiceDTO.Credits), -1, -1, true, accountServiceDTO.Remarks, ref message, parafaitDBTrx);
                    if (!succTransfer)
                    {
                        log.Error(message);
                        throw new Exception(message);
                    }
                }
                else
                {
                    succTransfer = taskProcs.ConvertTimeForCredit(updateCard, Convert.ToDouble(accountServiceDTO.Time), true, accountServiceDTO.Remarks, ref message, parafaitDBTrx);
                    if (!succTransfer)
                    {
                        log.Error(message);
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
/********************************************************************************************
 * Project Name - AccountService
 * Description  -Pause Account BL class
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
    /// PauseAccountBL class implements pause account functionality
    /// </summary>
    public class PauseAccountBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private AccountServiceDTO accountServiceDTO;
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private PauseAccountBL(ExecutionContext executionContext)
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
        public PauseAccountBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountServiceDTO = accountServiceDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Method to pause the time entitke ments
        /// </summary>
        public void PauseAllTimeEntitlements(SqlTransaction sqlTransaction = null)
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
                bool succPauseTime = true;
                HelperClassBL.SetParafairEnvValues(executionContext, utilities);
                TaskProcs taskProcs = new TaskProcs(utilities);
                succPauseTime = taskProcs.PauseTimeEntitlement(accountServiceDTO.SourceAccountDTO.AccountId, accountServiceDTO.Remarks, ref message, parafaitDBTrx);
                if (!succPauseTime)
                {
                    log.Error("PAUSETIMEENTITLEMENT-  has error " + message);
                    throw new Exception(message);
                }
                else
                {
                    log.Info("PAUSETIMEENTITLEMENT Time Paused successfully");
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

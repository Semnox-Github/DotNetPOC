/********************************************************************************************
 * Project Name - AccountService
 * Description  -Deactivate the cards
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120.0    22-Apr-2021   Prajwal S              Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// DeactivateAccountBL
    /// </summary>
    public class DeactivateAccountBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private AccountServiceDTO accountServiceDTO;


        private DeactivateAccountBL(ExecutionContext executionContext)
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
        public DeactivateAccountBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountServiceDTO);
            this.accountServiceDTO = accountServiceDTO;
            HelperClassBL.SetParafairEnvValues(executionContext, utilities);
            log.LogMethodExit();
        }

        /// <summary>
        /// Deactivate method
        /// </summary>
        public void Deactivate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
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
                DeactivateCard(parafaitDBTrx);
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// DeactivateCard
        /// </summary>
        private void DeactivateCard(SqlTransaction parafaitDBTrx)
        {
            log.LogMethodEntry(parafaitDBTrx);
            try
            {
                string message = string.Empty;
                HelperClassBL.SetParafairEnvValues(executionContext, utilities);
                TaskProcs taskProcs = new TaskProcs(utilities);
                List<Card> cardList = new List<Card>();
                Card card = new Card(accountServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities);
                if (!taskProcs.RefundCard(card, 0, 0, 0, "Deactivate", ref message, true, parafaitDBTrx))
                {
                    message = "Error" + message;
                    log.LogMethodExit(message);
                    throw new Exception(message);
                }
                else
                    card.invalidateCard(parafaitDBTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }


    }
}

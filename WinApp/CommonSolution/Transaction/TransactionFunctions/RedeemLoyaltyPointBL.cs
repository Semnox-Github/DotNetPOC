/********************************************************************************************
 * Project Name - AccountService
 * Description  - Redeem Loyalty Points from card to other entitlements
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0    12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RedeemLoyaltyPoints class implements RedeemLoyaltyPoints functionalities
    /// </summary>
    public class RedeemLoyaltyPointBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private TransactionServiceDTO transactionServiceDTO;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private RedeemLoyaltyPointBL(ExecutionContext executionContext)
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
        /// <param name="transactionServiceDTO"></param>
        public RedeemLoyaltyPointBL(ExecutionContext executionContext, TransactionServiceDTO transactionServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.transactionServiceDTO = transactionServiceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Redeem Loyalty points
        /// </summary>
        public void RedeemLoyaltyPoints(SqlTransaction sqlTransaction = null)
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
            string message = string.Empty;
            HelperClassBL.SetParafairEnvValues(executionContext, utilities);
            TaskProcs taskProcs = new TaskProcs(utilities);
            try
            {
                Card currentCard = new Card(transactionServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities, parafaitDBTrx);
                double redeemPoints = Convert.ToDouble(transactionServiceDTO.LoyaltyPoints);
                if (string.IsNullOrEmpty(transactionServiceDTO.LoyaltyAttribute))
                {
                    throw new ValidationException("loyaltyAttribute is not valid");
                }
                string DBColumnName = HelperClassBL.GetDBColumnNameForLoyaltyRedeem(transactionServiceDTO.LoyaltyAttribute);
                Loyalty loyalty = new Loyalty(utilities);
                int transactionId = loyalty.RedeemLoyaltyPoints(currentCard.card_id, currentCard.CardNumber,
                                                        Convert.ToDouble(transactionServiceDTO.LoyaltyPoints), DBColumnName, Convert.ToDouble(transactionServiceDTO.Value),
                                                        utilities.ParafaitEnv.POSMachine, utilities.ParafaitEnv.User_Id, parafaitDBTrx);
                taskProcs.createTask(currentCard.card_id, TaskProcs.REDEEMLOYALTY, Convert.ToDouble(transactionServiceDTO.Value), -1, -1, -1, -1, -1, -1, transactionServiceDTO.Remarks, parafaitDBTrx, -1, -1, -1, -1, transactionId);
                log.LogMethodExit();
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

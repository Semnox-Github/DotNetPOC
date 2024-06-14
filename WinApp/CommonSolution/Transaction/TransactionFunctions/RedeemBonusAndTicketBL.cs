/********************************************************************************************
 * Project Name - AccountService
 * Description  - Redeem Bonus And Ticket BL
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
    /// RedeemBonusAndTicketBL implements the redeem functionalities
    /// </summary>
    public class RedeemBonusAndTicketBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private AccountServiceDTO accountServiceDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private RedeemBonusAndTicketBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructors
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountServiceDTO"></param>
        public RedeemBonusAndTicketBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountServiceDTO = accountServiceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// RedeemEntitlements method
        /// </summary>
        public void RedeemEntitlements(SqlTransaction sqlTransaction = null)
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
                Card CurrentCard = new Card(accountServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities, parafaitDBTrx);
                if (accountServiceDTO.RedeemBonusForTickets)
                {
                    if (!taskProcs.RedeemBonusForTicket(CurrentCard, Convert.ToDouble(accountServiceDTO.Bonus), Convert.ToInt32(accountServiceDTO.Tickets), accountServiceDTO.Remarks, ref message))
                    {
                        log.Error("REDEEMBONUSFORTICKET- unable to RedeemBonusForTickets as error" + message);
                        throw new Exception(message);
                    }
                    else
                    {
                        log.Debug("-REDEEMBONUSFORTICKET Bonus redeemed for ticket successfully");
                    }
                }
                else
                {
                    TaskProcs.EntitlementType type = taskProcs.getEntitlementType(accountServiceDTO.EntitlementType);
                    if (string.IsNullOrEmpty(type.ToString()))
                    {
                        throw new Exception("Entitlement type is not valid");
                    }
                    if (type == TaskProcs.EntitlementType.CardBalance)
                    {
                        if (!taskProcs.RedeemTicketsForCredit(CurrentCard, accountServiceDTO.Tickets, Convert.ToDouble(accountServiceDTO.Bonus), accountServiceDTO.Remarks, ref message))
                        {
                            log.Error("REDEEMTICKETSFORCREDITS- unable to RedeemTicketsForCredits as error" + message);
                            throw new Exception(message);
                        }
                        else
                        {
                            log.Info("REDEEMTICKETSFORBONUS- redeemed for Credits successfully");
                        }
                    }
                    else if (type == TaskProcs.EntitlementType.Bonus)
                    {

                        if (!taskProcs.RedeemTicketsForBonus(CurrentCard, accountServiceDTO.Tickets, Convert.ToDouble(accountServiceDTO.Bonus), accountServiceDTO.Remarks, ref message))
                        {
                            log.Error("REDEEMTICKETSFORCREDITS- unable to RedeemTicketsForBonus as error" + message);
                            throw new Exception(message);
                        }
                        else
                        {
                            log.Info("REDEEMTICKETSFORBONUS- redeemed for RedeemTicketsForBonus successfully");
                        }

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




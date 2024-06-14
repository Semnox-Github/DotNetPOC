/********************************************************************************************
 * Project Name - TransactionUseCaseFactory
 * Description  - TransactionUseCaseFactory class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By     Remarks
 *********************************************************************************************
 2.110.0         25-Jan-2021       Guru S A        For Subscription changes
 2.110.0         11-Feb-2021       Fiona           For Virtual Arcade changes
 2.120.0         11-Feb-2021       Prajwal         Modified : Urban pipers and Notification Tag changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction.VirtualArcade;
using System.Configuration;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionUseCaseFactory
    /// </summary>
    public class TransactionUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GetSubscriptionHeaderUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ISubscriptionHeaderUseCases GetSubscriptionHeaderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISubscriptionHeaderUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteSubscriptionHeaderUseCases(executionContext);
            }
            else
            {
                result = new LocalSubscriptionHeaderUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetCustomerCreditCardsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ICustomerCreditCardsUseCases GetCustomerCreditCardsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerCreditCardsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerCreditCardsUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerCreditCardsUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetTrxPOSPrinterOverrideRulesUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ITrxPOSPrinterOverrideRulesUseCases GetTrxPOSPrinterOverrideRulesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITrxPOSPrinterOverrideRulesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTrxPOSPrinterOverrideRulesUseCases(executionContext);
            }
            else
            {
                result = new LocalTrxPOSPrinterOverrideRulesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetCustomerGamePlayLevelResultUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ICustomerGamePlayLevelResultUseCases GetCustomerGamePlayLevelResultUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerGamePlayLevelResultUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerGamePlayLevelResultUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerGamePlayLevelResultUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetWaiverUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ITransactionUseCases GetTransactionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITransactionUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTransactionUseCases(executionContext);
            }
            else
            {
                result = new LocalTransactionUseCase(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
       
        /// <summary>
        /// GetOrderHeaderUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IOrderHeaderUseCases GetOrderHeaderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IOrderHeaderUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteOrderHeaderUseCases(executionContext);
            }
            else
            {
                result = new LocalOrderHeaderUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetNotificationTagManualEventsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static INotificationTagManualEventUseCases GetNotificationTagManualEventsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            INotificationTagManualEventUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteNotificationTagManualEventUseCases(executionContext);
            }
            else
            {
                result = new LocalNotificationTagManualEventUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetNotificationTagProfileUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static INotificationTagProfileUseCases GetNotificationTagProfileUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            INotificationTagProfileUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteNotificationTagProfileUseCases(executionContext);
            }
            else
            {
                result = new LocalNotificationTagProfileUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetNotificationTagIssuedUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static INotificationTagIssuedUseCases GetNotificationTagIssuedUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            INotificationTagIssuedUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteNotificationTagIssuedUseCases(executionContext);
            }
            else
            {
                result = new LocalNotificationTagIssuedUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get TransactionSummaryUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ITransactionSummaryViewUseCases GetTransactionSummaryViewUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITransactionSummaryViewUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTransactionSummaryViewUseCases(executionContext);
            }
            else
            {
                result = new LocalTransactionSummaryViewUseCase(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

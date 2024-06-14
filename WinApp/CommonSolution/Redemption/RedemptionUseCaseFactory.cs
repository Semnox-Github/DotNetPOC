/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.110.0      21-Dec-2020      Abhishek                  Modified : added factory TicketStation for REST API
 ********************************************************************************************/
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IRedemptionUseCases GetRedemptionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IRedemptionUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteRedemptionUseCases(executionContext);
            }
            else
            {
                result = new LocalRedemptionUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static IRedemptionCurrencyUseCases GetRedemptionCurrencyUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IRedemptionCurrencyUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteRedemptionCurrencyUseCases(executionContext);
            }
            else
            {
                result = new LocalRedemptionCurrencyUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static IRedemptionCurrencyRuleUseCases GetRedemptionCurrencyRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IRedemptionCurrencyRuleUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteRedemptionCurrencyRuleUseCases(executionContext);
            }
            else
            {
                result = new LocalRedemptionCurrencyRuleUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static ITicketReceiptUseCases GetTicketReceiptUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITicketReceiptUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTicketReceiptUseCases(executionContext);
            }
            else
            {
                result = new LocalTicketReceiptUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static ITicketStationUseCases GetTicketStationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITicketStationUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTicketStationUseCases(executionContext);
            }
            else
            {
                result = new LocalTicketStationUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

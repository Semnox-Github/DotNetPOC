/********************************************************************************************
 * Project Name - Accounts
 * Description  - AccountUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         13-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.Customer.Accounts
{
    public class AccountUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IAccountUseCases GetAccountUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAccountUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAccountUseCases(executionContext);
            }
            else
            {
                result = new LocalAccountUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

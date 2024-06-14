/********************************************************************************************
 * Project Name - Accounts
 * Description  - LinkedAccountUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Apr-2022       Nitin Pai                 Created : 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.Customer.Accounts
{
    public class LinkedAccountUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ILinkedAccountUseCases GetLinkedAccountUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILinkedAccountUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteLinkedAccountUseCases(executionContext);
            }
            else
            {
                result = new LocalLinkedAccountUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

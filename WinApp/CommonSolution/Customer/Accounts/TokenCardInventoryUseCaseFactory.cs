/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Token card inventory usecase factory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.Customer.Accounts
{
    public class TokenCardInventoryUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ITokenCardInventoryUseCases GetTokenCardInventoryUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITokenCardInventoryUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTokenCardInventoryUseCases(executionContext);
            }
            else
            {
                result = new LocalTokenCardInventoryUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

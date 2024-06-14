/********************************************************************************************
 * Project Name - Utilities
 * Description  - SystemOptionFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Configuration;

namespace Semnox.Core.Utilities
{
    public class SystemOptionUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ISystemOptionUseCases GetSystemOptionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISystemOptionUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteSystemOptionUseCases(executionContext);
            }
            else
            {
                result = new LocalSystemOptionUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

/********************************************************************************************
 * Project Name - Utilities
 * Description  - LanguageFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    public class LanguageUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ILanguageUseCases GetLanguageUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILanguageUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteLanguageUseCases(executionContext);
            }
            else
            {
                result = new LocalLanguageUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IMessageUseCases GetMessageUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMessageUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMessageUseCases(executionContext);
            }
            else
            {
                result = new LocalMessageUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

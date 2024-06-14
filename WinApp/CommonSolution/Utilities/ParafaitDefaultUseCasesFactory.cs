/********************************************************************************************
 * Project Name - Utilities
 * Description  - ParafaitDefaultFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Configuration;

namespace Semnox.Core.Utilities
{
    public class ParafaitDefaultUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IParafaitDefaultUseCases GetParafaitDefaultUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IParafaitDefaultUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteParafaitDefaultUseCases(executionContext);
            }
            else
            {
                result = new LocalParafaitDefaultUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

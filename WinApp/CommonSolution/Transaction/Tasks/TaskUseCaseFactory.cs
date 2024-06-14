/********************************************************************************************
 * Project Name - Transaction
 * Description  - TaskUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          23-Mar-2021      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TaskUseCaseFactory
    /// </summary>
    public class TaskUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GetTaskUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns>ITaskUseCases</returns>
        public static ITaskUseCases GetTaskUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITaskUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTaskUseCases(executionContext);
            }
            else
            {
                result = new LocalTaskUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

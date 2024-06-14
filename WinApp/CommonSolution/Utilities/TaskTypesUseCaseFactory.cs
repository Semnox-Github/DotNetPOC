/********************************************************************************************
 * Project Name - Utilities
 * Description  - Factory class to instantiate the utilities use cases 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00     04-Mar-2021      Roshan Devadiga         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    
   public  class TaskTypesUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ITaskTypesUseCases GetTaskTypesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITaskTypesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTaskTypesUseCases(executionContext);
            }
            else
            {
                result = new LocalTaskTypesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

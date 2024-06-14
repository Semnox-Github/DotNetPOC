/********************************************************************************************
 * Project Name - DBSynch
 * Description  - Factory class to instantiate the DBSynch use cases 
 *  
 **************
 **Version Log
 **************
 *Version         Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00         05-May-2021      Roshan Devadiga           Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DBSynch
{
   public class DBSynchUseCaseFactory
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// GetDbSynchTableUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IDbSynchTableUseCases GetDbSynchTableUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDbSynchTableUseCases themeUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                themeUseCases = new RemoteDbSynchTableUseCases(executionContext);
            }
            else
            {
                themeUseCases = new LocalDbSynchTableUseCases(executionContext);
            }
            log.LogMethodExit(themeUseCases);
            return themeUseCases;
        }
    }
}

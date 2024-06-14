/********************************************************************************************
 * Project Name - Achievements
 * Description  - Factory class to instantiate the Achievements use cases 
 *  
 **************
 **Version Log
 **************
 *Version         Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00         11-May-2021      Roshan Devadiga           Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Achievements
{
   public class AchievementUseCaseFactory
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// GetScoringEventPolicyUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IScoringEventPolicyUseCases GetScoringEventPolicyUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IScoringEventPolicyUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteScoringEventPolicyUseCases(executionContext);
            }
            else
            {
                result = new LocalScoringEventPolicyUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

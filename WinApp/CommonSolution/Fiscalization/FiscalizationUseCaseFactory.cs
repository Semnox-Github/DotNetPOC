/********************************************************************************************
 * Project Name - Fiscalization
 * Description  - FiscalizationUseCaseFactory class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By     Remarks
 *********************************************************************************************
 2.155.1         11-Aug-2023       Guru S A        Chile fiscaliation changes
 ********************************************************************************************/
using Semnox.Core.Utilities; 
using System.Configuration;
using System.Linq;
using System.Text; 

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// FiscalizationUseCaseFactory
    /// </summary>
    public class FiscalizationUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// GetParafaitFiscalizationUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IParafaitFiscalizationUseCases GetParafaitFiscalizationUseCases(ExecutionContext executionContext, Utilities utilities)
        {
            log.LogMethodEntry(executionContext, utilities);
            IParafaitFiscalizationUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteParafaitFiscalizationUseCases(executionContext, utilities);
            }
            else
            {
                result = new LocalParafaitFiscalizationUseCases(executionContext, utilities);
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}

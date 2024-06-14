/********************************************************************************************
 * Project Name - Site
 * Description  - SiteUseCaseFactory class
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.Site
{
    public class SiteUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ISiteUseCases GetSiteUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISiteUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteSiteUseCases(executionContext);
            }
            else
            {
                result = new LocalSiteUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static ISiteTimeUseCases GetSiteTimeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISiteTimeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteSiteTimeUseCases(executionContext);
            }
            else
            {
                result = new LocalSiteTimeUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

    }
}

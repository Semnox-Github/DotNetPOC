using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    public  class ThemeUseCaseFactory
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IThemeUseCases GetThemeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IThemeUseCases themeUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                themeUseCases = new RemoteThemeUseCases(executionContext);
            }
            else
            {
                themeUseCases = new LocalThemeUseCases(executionContext);
            }
            log.LogMethodExit(themeUseCases);
            return themeUseCases;
        }
    }
}

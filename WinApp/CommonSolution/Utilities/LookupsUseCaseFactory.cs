using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class LookupsUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ILookupsUseCases GetLookupsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILookupsUseCases lookupUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                lookupUseCases = new RemoteLookupsUseCases(executionContext);
            }
            else
            {
                lookupUseCases = new LocalLookupsUseCases(executionContext);
            }
            log.LogMethodExit(lookupUseCases);
            return lookupUseCases;
        }
    }
}

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class POSMachineFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IPOSMachineDataService GetPOSMachineContainerDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPOSMachineDataService result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePOSMachineDataService(executionContext);
            }
            else
            {
                result = new LocalPOSMachineDataService(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}

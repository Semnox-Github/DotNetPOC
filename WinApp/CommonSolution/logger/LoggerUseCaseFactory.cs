using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
   public class LoggerUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IMonitorAssetUseCases GetMonitorAssets(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMonitorAssetUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMonitorAssetUseCases(executionContext);
            }
            else
            {
                result = new LocalMonitorAssetUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static IMonitorUseCases GetMonitors(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMonitorUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMonitorUseCases(executionContext);
            }
            else
            {
                result = new LocalMonitorUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static IMonitorPriorityUseCases GetMonitorPriorities(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMonitorPriorityUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMonitorPriorityUseCases(executionContext);
            }
            else
            {
                result = new LocalMonitorPriorityUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IMonitorLogUseCases GetMonitorLogsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMonitorLogUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMonitorLogUseCases(executionContext);
            }
            else
            {
                result = new LocalMonitorLogUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetEventLogsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="requestGuid"></param>
        /// <returns></returns>
        public static IEventLogUseCases GetEventLogsUseCases(ExecutionContext executionContext, string requestGuid)
        {
            log.LogMethodEntry(executionContext, requestGuid);
            IEventLogUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteEventLogUseCases(executionContext, requestGuid);
            }
            else
            {
                result = new LocalEventLogUseCases(executionContext, requestGuid);
            }

            log.LogMethodExit(result);
            return result;
        }
   }
}

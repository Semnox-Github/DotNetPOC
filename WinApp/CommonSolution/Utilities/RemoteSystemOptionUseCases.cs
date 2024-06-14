/********************************************************************************************
 * Project Name - Utilities
 * Description  - RemoteSystemOptionUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class RemoteSystemOptionUseCases : RemoteUseCases, ISystemOptionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SYSTEM_OPTION_URL = "api/Configuration/SystemOptions";
        private const string SYSTEM_OPTION_CONTAINER_URL = "api/Configuration/SystemOptionContainer";

        public RemoteSystemOptionUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<SystemOptionContainerDTOCollection> GetSystemOptionContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            SystemOptionContainerDTOCollection result = await Get<SystemOptionContainerDTOCollection>(SYSTEM_OPTION_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}

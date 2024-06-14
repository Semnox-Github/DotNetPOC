/********************************************************************************************
 * Project Name - Utilities
 * Description  - RemoteParafaitDefaultUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class RemoteParafaitDefaultUseCases : RemoteUseCases, IParafaitDefaultUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PARAFAIT_DEFAULT_URL = "api/Configuration/ParafaitDefaults";
        private const string PARAFAIT_DEFAULT_CONTAINER_URL = "api/Configuration/ParafaitDefaultContainer";

        public RemoteParafaitDefaultUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<ParafaitDefaultContainerDTOCollection> GetParafaitDefaultContainerDTOCollection(int siteId, int userPkId, int machineId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, userPkId, machineId, hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("userPkId", userPkId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("machineId", machineId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            ParafaitDefaultContainerDTOCollection result = await Get<ParafaitDefaultContainerDTOCollection>(PARAFAIT_DEFAULT_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}

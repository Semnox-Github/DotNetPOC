/********************************************************************************************
 * Project Name - POS
 * Description  - RemotePOSMachineViewContainerDataService class to get the data  from API by doing remote call  
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
using System.Collections.Generic;

namespace Semnox.Parafait.POS
{
    public class RemotePOSMachineViewContainerDataService : RemoteDataService, IPOSMachineViewContainerDataService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/POS/POSMachineContainer";

        public RemotePOSMachineViewContainerDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public POSMachineViewDTOCollection Get(string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            POSMachineViewDTOCollection result = null;
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            string responseString = Get(GET_URL, parameters);
            dynamic response = JsonConvert.DeserializeObject(responseString);
            if (response != null)
            {
                object data = response["data"];
                result = JsonConvert.DeserializeObject<POSMachineViewDTOCollection>(data.ToString());
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

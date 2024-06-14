/********************************************************************************************
 * Project Name - RemoteMachineContainerDataService  Class
 * Description  - RemoteMachineContainerDataService class to get the data  from API by doing remote call  
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
using System.Globalization;

namespace Semnox.Parafait.Game
{
    public class RemoteMachineContainerDataService : RemoteDataService, IMachineContainerDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/Game/MachineContainer";

        public RemoteMachineContainerDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public List<MachineDTO> Get(DateTime? maxLastUpdatedDate, string hash)
        {
            log.LogMethodEntry(maxLastUpdatedDate, hash);
            List<MachineDTO> result = null;
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            if (maxLastUpdatedDate.HasValue)
            {
                parameters.Add(new KeyValuePair<string, string>("maxLastUpdatedDate", maxLastUpdatedDate.Value.ToString(CultureInfo.InvariantCulture)));
            }
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string responseString = Get(GET_URL, parameters);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                if (response != null)
                {
                    object data = response["data"];
                    result = JsonConvert.DeserializeObject<List<MachineDTO>>(data.ToString());
                }
                log.LogMethodExit(result);
            
            return result;
        }
    }
}

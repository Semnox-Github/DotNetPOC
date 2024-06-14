/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteHubDataService class to get the data  from API by doing remote call  
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

namespace Semnox.Parafait.Game
{
    public class RemoteReaderConfigurationDataService : RemoteDataService, IReaderConfigurationDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/Game/ReaderConfigs";

        public RemoteReaderConfigurationDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public List<MachineAttributeDTO> GetReaderConfigurations(List<KeyValuePair<string, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<MachineAttributeDTO> result = null;
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string responseString = Get(GET_URL, parameters);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                if (response != null)
                {
                    object data = response["data"];
                    result = JsonConvert.DeserializeObject<List<MachineAttributeDTO>>(data.ToString());
                }
                log.LogMethodExit(result);
            
            return result;
        }

        public string PostReaderConfigurations(List<MachineAttributeDTO> machineAttributeDTOList, string moduleName, string moduleId)
        {
            log.LogMethodEntry(machineAttributeDTOList);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            //try
            //{ 
            string content = JsonConvert.SerializeObject(machineAttributeDTOList);
                //content += JsonConvert.SerializeObject(moduleName);
                //content += JsonConvert.SerializeObject(moduleId);
                string responseString = Post(GET_URL + "/?moduleName="+ moduleName+ "&moduleId=" + moduleId, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return "Success";
            //}
            //catch (WebApiException wex)
            //{
            //    log.Error(wex);
            //    throw;
            //}

        }
    }
}

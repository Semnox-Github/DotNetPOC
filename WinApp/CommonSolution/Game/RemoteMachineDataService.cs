/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteMachineDataService class to get the data  from API by doing remote call  
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
    public class RemoteMachineDataService : RemoteDataService, IMachineDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/Game/Machines";

        public RemoteMachineDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public List<MachineDTO> GetMachines(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> parameters, bool loadChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            List<MachineDTO> result = null;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadAttribute".ToString(), loadChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();

            string responseString = Get(GET_URL, searchParameterList);
            dynamic response = JsonConvert.DeserializeObject(responseString);
            if (response != null)
            {
                object data = response["data"];
                result = JsonConvert.DeserializeObject<List<MachineDTO>>(data.ToString());
            }
            log.LogMethodExit(result);
            
            return result;
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> machineSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MachineDTO.SearchByMachineParameters, string> searchParameter in machineSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MachineDTO.SearchByMachineParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.MASTER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.GAME_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.MACADDRESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.MACHINE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineName".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("referenceMachineId".ToString(), searchParameter.Value));
                        }

                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public string PostMachines(List<MachineDTO> machineDTOList)
        {
            try
            {
                log.LogMethodEntry(machineDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(machineDTOList);
                string responseString = Post(GET_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return "Success";
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }

        }

        public string DeleteMachines(List<MachineDTO> machineDTOList)
        {
            try
            {
                log.LogMethodEntry(machineDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(machineDTOList);
                string responseString = Delete(GET_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }
    }
}

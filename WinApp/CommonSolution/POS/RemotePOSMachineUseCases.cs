/********************************************************************************************
 * Project Name - POS
 * Description  - RemotePOSMachineDataService class to get the data  from API by doing remote call  
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
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class RemotePOSMachineUseCases : RemoteUseCases, IPOSMachineUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string POS_MACHINE_URL = "/api/POS/POSMachines";
        private const string POS_MACHINE_CONTAINER_URL = "/api/POS/POSMachinesContainer";
        public RemotePOSMachineUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<POSMachineDTO>> GetPOSMachines(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<POSMachineDTO> result = await Get<List<POSMachineDTO>>(POS_MACHINE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSMachineDTO.SearchByPOSMachineParameters.POS_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineName".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posMachineId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<POSMachineContainerDTOCollection> GetPOSMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            POSMachineContainerDTOCollection result = await Get<POSMachineContainerDTOCollection>(POS_MACHINE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<string> SavePOSMachines(List<POSMachineDTO> posMachineDTOList)
        {
            log.LogMethodEntry(posMachineDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(posMachineDTOList);
                string responseString = await Post<string>(POS_MACHINE_URL, content);
                //dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

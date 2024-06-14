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

namespace Semnox.Parafait.POS
{
    public class RemotePOSMachineDataService : RemoteDataService, IPOSMachineDataService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/Configuration/POSMachines";

        public RemotePOSMachineDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public List<POSMachineDTO> GetPOSMachines(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> parameters, bool loadChildRecords = false)
        {
            log.LogMethodEntry(parameters);

            List<POSMachineDTO> result = null;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), loadChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string responseString = Get(GET_URL, searchParameterList);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                if (response != null)
                {
                    object data = response["data"];
                    result = JsonConvert.DeserializeObject<List<POSMachineDTO>>(data.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
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

        public string PostPOSMachines(List<POSMachineDTO> posMachineDTOList)
        {
            log.LogMethodEntry(posMachineDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(posMachineDTOList);
                string responseString = Post(GET_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

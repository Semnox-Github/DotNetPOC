/********************************************************************************************
 * Project Name - User
 * Description  - RemoteAgentsUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version      Date              Modified By             Remarks          
 *********************************************************************************************
 2.120.0       31-Mar-2021       Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    class RemoteAgentsUseCases : RemoteUseCases, IAgentsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Agents_URL = "api/HR/Agents";
        public RemoteAgentsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<AgentsDTO>> GetAgents(List<KeyValuePair<AgentsDTO.SearchByParameters, string>>
                          parameters,  SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<AgentsDTO> result = await Get<List<AgentsDTO>>(Agents_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AgentsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AgentsDTO.SearchByParameters.ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case AgentsDTO.SearchByParameters.AGENT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("agentsId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AgentsDTO.SearchByParameters.PARTNER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("partnerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AgentsDTO.SearchByParameters.USER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("userId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveAgents(List<AgentsDTO> agentsDTOList)
        {
            log.LogMethodEntry(agentsDTOList);
            try
            {
                string responseString = await Post<string>(Agents_URL, agentsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteAgents(List<AgentsDTO> agentsDTOList)
        {
            try
            {
                log.LogMethodEntry(agentsDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(agentsDTOList);
                string responseString = await Delete(Agents_URL, content);
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
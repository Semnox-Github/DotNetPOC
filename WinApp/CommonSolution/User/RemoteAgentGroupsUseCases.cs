/********************************************************************************************
 * Project Name - User
 * Description  - RemoteAgentGroupsUseCases class to get the data  from API by doing remote call  
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
    class RemoteAgentGroupsUseCases : RemoteUseCases, IAgentGroupsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string AgentGroups_URL = "api/HR/AgentGroups";
        public RemoteAgentGroupsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<AgentGroupsDTO>> GetAgentGroups(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>
                          parameters, bool loadChildRecords, bool activeChildRecords,  SqlTransaction sqlTransaction = null)
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
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<AgentGroupsDTO> result = await Get<List<AgentGroupsDTO>>(AgentGroups_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AgentGroupsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AgentGroupsDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case AgentGroupsDTO.SearchByParameters.AGENT_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("groupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AgentGroupsDTO.SearchByParameters.PARTNER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("partnerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AgentGroupsDTO.SearchByParameters.GROUP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("groupName".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveAgentGroups(List<AgentGroupsDTO> AgentGroupsDTOList)
        {
            log.LogMethodEntry(AgentGroupsDTOList);
            try
            {
                string responseString = await Post<string>(AgentGroups_URL, AgentGroupsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteAgentGroups(List<AgentGroupsDTO> AgentGroupsDTOList)
        {
            try
            {
                log.LogMethodEntry(AgentGroupsDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(AgentGroupsDTOList);
                string responseString = await Delete(AgentGroups_URL, content);
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
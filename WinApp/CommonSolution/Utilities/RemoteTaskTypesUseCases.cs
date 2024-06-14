/********************************************************************************************
 * Project Name - Utilities
 * Description  - TaskTypesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    02-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Semnox.Core.Utilities
{
    public class RemoteTaskTypesUseCases : RemoteUseCases, ITaskTypesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TASKTYPES_URL = "api/Transaction/TaskTypes";
        private const string TASKTYPES_CONTAINER_URL = "api/Transaction/TaskTypesContainer";

        public RemoteTaskTypesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<TaskTypesDTO>> GetTaskTypes(List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>>
                         parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<TaskTypesDTO> result = await Get<List<TaskTypesDTO>>(TASKTYPES_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<TaskTypesDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case TaskTypesDTO.SearchByParameters.TASK_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taskTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaskTypesDTO.SearchByParameters.TASK_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taskType".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveTaskTypes(List<TaskTypesDTO> taskTypeDTOList)
        {
            log.LogMethodEntry(taskTypeDTOList);
            try
            {
                string responseString = await Post<string>(TASKTYPES_URL, taskTypeDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<TaskTypesContainerDTOCollection> GetTaskTypesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            TaskTypesContainerDTOCollection result = await Get<TaskTypesContainerDTOCollection>(TASKTYPES_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
        public async Task<string> Delete(List<TaskTypesDTO> taskTypesDTOList)
        {
            try
            {
                log.LogMethodEntry(taskTypesDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(taskTypesDTOList);
                string responseString = await Delete(TASKTYPES_URL, content);
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

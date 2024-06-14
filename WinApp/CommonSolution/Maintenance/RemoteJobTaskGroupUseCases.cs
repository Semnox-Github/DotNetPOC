/********************************************************************************************
* Project Name - JobTaskGroup
* Description  - RemoteJobTaskGroupUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   21-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    class RemoteJobTaskGroupUseCases: RemoteUseCases,IJobTaskGroupUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string JOBTASKGROUP_URL = "api/Maintenance/TaskGroups";
        public RemoteJobTaskGroupUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<JobTaskGroupDTO>> GetJobTaskGroups(List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> parameters)
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
                List<JobTaskGroupDTO> result = await Get<List<JobTaskGroupDTO>>(JOBTASKGROUP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case JobTaskGroupDTO.SearchByJobTaskGroupParameters.JOB_TASK_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("JobtaskGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case JobTaskGroupDTO.SearchByJobTaskGroupParameters.TASK_GROUP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taskGroupName".ToString(), searchParameter.Value));
                        }
                        break;
                    case JobTaskGroupDTO.SearchByJobTaskGroupParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case JobTaskGroupDTO.SearchByJobTaskGroupParameters.HAS_ACTIVE_TASKS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("HasActiveTasks".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveJobTaskGroups(List<JobTaskGroupDTO> jobTaskGroupDTOList)
        {
            log.LogMethodEntry(jobTaskGroupDTOList);
            try
            {
                string responseString = await Post<string>(JOBTASKGROUP_URL, jobTaskGroupDTOList);
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

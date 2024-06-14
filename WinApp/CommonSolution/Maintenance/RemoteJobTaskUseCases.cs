/********************************************************************************************
* Project Name - JobTask
* Description  - RemoteJobTaskUseCases
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    class RemoteJobTaskUseCases: RemoteUseCases,IJobTaskUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string JOBTASK_URL = "api/Maintenance/Tasks";
        public RemoteJobTaskUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<JobTaskDTO>> GetJobTasks(List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> parameters, SqlTransaction sqlTransaction = null)
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
                List<JobTaskDTO> result = await Get<List<JobTaskDTO>>(JOBTASK_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("JobtaskId".ToString(), searchParameter.Value));
                        }
                        break;
                    case JobTaskDTO.SearchByJobTaskParameters.TASK_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taskName".ToString(), searchParameter.Value));
                        }
                        break;
                    case JobTaskDTO.SearchByJobTaskParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("JobTaskGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveJobTasks(List<JobTaskDTO> jobTaskDTOList)
        {
            log.LogMethodEntry(jobTaskDTOList);
            try
            {
                string responseString = await Post<string>(JOBTASK_URL, jobTaskDTOList);
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

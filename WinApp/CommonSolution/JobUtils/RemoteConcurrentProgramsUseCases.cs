/********************************************************************************************
* Project Name - ConcurrentProgram
* Description  - RemoteConcurrentProgramsUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   27-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    class RemoteConcurrentProgramsUseCases: RemoteUseCases,IConcurrentProgramsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CONCURRENTPROGRAMS_URL = "api/Jobs/ConcurrentJobs";
        public RemoteConcurrentProgramsUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<ConcurrentProgramsDTO>> GetConcurrentPrograms(List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), loadActiveRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<ConcurrentProgramsDTO> result = await Get<List<ConcurrentProgramsDTO>>(CONCURRENTPROGRAMS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ProgramId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("programName".ToString(), searchParameter.Value));
                        }
                        break;
                    case ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                    case ConcurrentProgramsDTO.SearchByProgramsParameters.ERROR_NOTIFICATION_MAIL:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ErrorNotificationMail".ToString(), searchParameter.Value));
                        }
                        break;
                    case ConcurrentProgramsDTO.SearchByProgramsParameters.EXECUTABLE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ExecutableName".ToString(), searchParameter.Value));
                        }
                        break;
                    case ConcurrentProgramsDTO.SearchByProgramsParameters.KEEP_RUNNING:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("KeepRunning".ToString(), searchParameter.Value));
                        }
                        break;
                    case ConcurrentProgramsDTO.SearchByProgramsParameters.SUCCESS_NOTIFICATION_MAIL:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("SuccessNotificationMail".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveConcurrentPrograms(List<ConcurrentProgramsDTO> concurrentProgramsDTOList)
        {
            log.LogMethodEntry(concurrentProgramsDTOList);
            try
            {
                string responseString = await Post<string>(CONCURRENTPROGRAMS_URL, concurrentProgramsDTOList);
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

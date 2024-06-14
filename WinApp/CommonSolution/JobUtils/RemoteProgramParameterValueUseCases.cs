/********************************************************************************************
* Project Name - ProgramParameterValue
* Description  - RemoteProgramParameterValueUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.1    18-May-2021   B Mahesh Pai             Created 
********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    class RemoteProgramParameterValueUseCases:RemoteUseCases, IProgramParameterValueUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ProgramParameterValue_URL = "api/Jobs/ProgramParameters";
        public RemoteProgramParameterValueUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<ProgramParameterValueDTO>> GetProgramParameterValues(List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchParameters,
                                      bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<ProgramParameterValueDTO> result = await Get<List<ProgramParameterValueDTO>>(ProgramParameterValue_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
            private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> lookupSearchParams)
            {
                List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                foreach (KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
                {
                    switch (searchParameter.Key)
                    {

                        case ProgramParameterValueDTO.SearchByParameters.CONCURRENTPROGRAM_SCHEDULE_ID:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("ConcurrentRequestId".ToString(), searchParameter.Value));
                            }
                            break;
                    case ProgramParameterValueDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsAcrive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProgramParameterValueDTO.SearchByParameters.PARAMETER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ParameterId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProgramParameterValueDTO.SearchByParameters.PROGRAM_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ProgramId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProgramParameterValueDTO.SearchByParameters.PROGRAM_PARAMETER_VALUE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ProgramParameterValueId".ToString(), searchParameter.Value));
                        }
                        break;
                }
                }
                log.LogMethodExit(searchParameterList);
                return searchParameterList;
            }
        public async Task<string> SaveProgramParameterValues(List<ProgramParameterValueDTO> programParameterValueDTO)
        {
            log.LogMethodEntry(programParameterValueDTO);
            try
            {
                string responseString = await Post<string>(ProgramParameterValue_URL, programParameterValueDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<ProgramParameterValueDTO> programParameterValueDTOList)
        {
            try
            {
                log.LogMethodEntry(programParameterValueDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(programParameterValueDTOList);
                string responseString = await Delete(ProgramParameterValue_URL, content);
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


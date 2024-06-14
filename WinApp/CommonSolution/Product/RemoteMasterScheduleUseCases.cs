/********************************************************************************************
 * Project Name - Product
 * Description  - MasterScheduleUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    10-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public class RemoteMasterScheduleUseCases:RemoteUseCases,IMasterScheduleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MASTERSCHEDULE_URL = "api/Product/MasterSchedules";

        public RemoteMasterScheduleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<MasterScheduleDTO>> GetMasterSchedules(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>>
                         parameters, bool loadChildActiveRecords = false, bool loadChildRecord = false, int facilityMapId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters,loadChildActiveRecords,loadChildRecord,facilityMapId,sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildActiveRecords".ToString(), loadChildActiveRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecord".ToString(), loadChildRecord.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("facilityMapId".ToString(), facilityMapId.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<MasterScheduleDTO> result = await Get<List<MasterScheduleDTO>>(MASTERSCHEDULE_URL,searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MasterScheduleDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterScheduleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterScheduleIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case MasterScheduleDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveMasterSchedules(List<MasterScheduleDTO> masterScheduleDTOList)
        {
            log.LogMethodEntry(masterScheduleDTOList);
            try
            {
                string responseString = await Post<string>(MASTERSCHEDULE_URL,masterScheduleDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> Delete(List<MasterScheduleDTO> masterScheduleDTOList)
        {
            try
            {
                log.LogMethodEntry(masterScheduleDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(masterScheduleDTOList);
                string responseString = await Delete(MASTERSCHEDULE_URL,content);
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

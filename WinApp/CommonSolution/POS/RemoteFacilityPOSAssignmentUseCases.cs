/********************************************************************************************
* Project Name - User
* Description  -  RemoteFacilityPOSAssignmentUseCases  class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    06-Apr-2021       B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/using System;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Semnox.Parafait.POS
{
    class RemoteFacilityPOSAssignmentUseCases : RemoteUseCases, IFacilityPOSAssignmentUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string FACILITYPOSASSIGNMENT_URL = "api/";


        public RemoteFacilityPOSAssignmentUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<FacilityPOSAssignmentDTO>> GetFacilityPOSAssignments(List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>>
                            parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<FacilityPOSAssignmentDTO> result = await Get<List<FacilityPOSAssignmentDTO>>(FACILITYPOSASSIGNMENT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case FacilityPOSAssignmentDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("  ".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityPOSAssignmentDTO.SearchByParameters.POS_MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("POSMachineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityPOSAssignmentDTO.SearchByParameters.FACILITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("FacilityId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityPOSAssignmentDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveFacilityPOSAssignments(List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList)
        {
            log.LogMethodEntry(facilityPOSAssignmentDTOList);
            try
            {
                string responseString = await Post<string>(FACILITYPOSASSIGNMENT_URL, facilityPOSAssignmentDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList)
        {
            try
            {
                log.LogMethodEntry(facilityPOSAssignmentDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(facilityPOSAssignmentDTOList);
                string responseString = await Delete(FACILITYPOSASSIGNMENT_URL, content);
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


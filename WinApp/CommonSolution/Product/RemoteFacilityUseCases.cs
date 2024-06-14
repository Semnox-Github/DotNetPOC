/********************************************************************************************
 * Project Name - Product
 * Description  - FacilityUseCases class 
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
    public class RemoteFacilityUseCases:RemoteUseCases,IFacilityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string FACILITY_URL = "api/Product/Facilities";

        public RemoteFacilityUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<FacilityDTO>> GetFacilitys(List<KeyValuePair<FacilityDTO.SearchByParameters, string>>
                         searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters,loadChildRecords,activeChildRecords,sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<FacilityDTO> result = await Get<List<FacilityDTO>>(FACILITY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<FacilityDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<FacilityDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case FacilityDTO.SearchByParameters.FACILITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityDTO.SearchByParameters.FACILITY_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityDTO.SearchByParameters.FACILITY_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityName".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("havingProductTypesIn".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityDTO.SearchByParameters.FACILITY_MAP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityMapId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityDTO.SearchByParameters.INTERFACE_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("interfaceType".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityDTO.SearchByParameters.INTERFACE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("interfaceName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveFacilitys(List<FacilityDTO> facilityDTOList)
        {
            log.LogMethodEntry(facilityDTOList);
            try
            {
                string responseString = await Post<string>(FACILITY_URL, facilityDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> Delete(List<FacilityDTO> facilityDTOList)
        {
            try
            {
                log.LogMethodEntry(facilityDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(facilityDTOList);
                string responseString = await Delete(FACILITY_URL, content);
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

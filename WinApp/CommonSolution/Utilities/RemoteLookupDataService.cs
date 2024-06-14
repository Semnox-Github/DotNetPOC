/********************************************************************************************
 * Project Name - Utilities
 * Description  - RemoteLookupDataService class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Semnox.Core.Utilities
{
    public class RemoteLookupDataService : RemoteDataService, ILookupDataService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/Configuration/Lookups";

        public RemoteLookupDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public List<LookupsDTO> GetLookups(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            List<LookupsDTO> result = null;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string responseString = Get(GET_URL, searchParameterList);
            dynamic response = JsonConvert.DeserializeObject(responseString);
            if (response != null)
            {
                object data = response["data"];
                result = JsonConvert.DeserializeObject<List<LookupsDTO>>(data.ToString());
            }
            log.LogMethodExit(result);

            return result;
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<LookupsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case LookupsDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case LookupsDTO.SearchByParameters.LOOKUP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("lookupName".ToString(), searchParameter.Value));
                        }
                        break;
                    case LookupsDTO.SearchByParameters.LOOKUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("lookupId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

     public string PostLookups(List<LookupsDTO> lookupsDTOList)
        {
            log.LogMethodEntry(lookupsDTOList);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string content = JsonConvert.SerializeObject(lookupsDTOList);
                string responseString = Post(GET_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
       
        }
        public string DeleteLookups(List<LookupsDTO> lookupsDTOList)
        {
            try
            {
                log.LogMethodEntry(lookupsDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(lookupsDTOList);
                // string responseString = Delete(GET_URL, content);
                // dynamic response = JsonConvert.DeserializeObject(responseString);
                // log.LogMethodExit(response);
                return "";//response;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

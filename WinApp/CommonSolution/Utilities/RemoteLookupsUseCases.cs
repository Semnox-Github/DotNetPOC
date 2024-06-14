/********************************************************************************************
 * Project Name - Lookups
 * Description  - RemoteLookupsUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         06-May-2021       B Mahesh Pai        Modified
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class RemoteLookupsUseCases : RemoteUseCases, ILookupsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LOOKUPS_URL = "api/Configuration/Lookups";
        private const string Lookups_CONTAINER_URL = "api/Lookups/LookupsContainer";

        public RemoteLookupsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<LookupsContainerDTOCollection> GetLookupsContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            LookupsContainerDTOCollection result = await Get<LookupsContainerDTOCollection>(Lookups_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
        public async Task<List<LookupsDTO>> GetLookups(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters, bool loadChild = false,
                                             bool loadActiveChildRecords = true, SqlTransaction sqlTransaction = null)


        {
            log.LogMethodEntry(searchParameters, loadChild, loadActiveChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChild".ToString(), loadChild.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChildRecords".ToString(), loadActiveChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<LookupsDTO> result = await Get<List<LookupsDTO>>(LOOKUPS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
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
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbSurveryId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LookupsDTO.SearchByParameters.LOOKUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("FromDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case LookupsDTO.SearchByParameters.LOOKUP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                   
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveLookups(List<LookupsDTO> lookupsDTOList)
        {
            log.LogMethodEntry(lookupsDTOList);
            try
            {
                string responseString = await Post<string>(LOOKUPS_URL, lookupsDTOList);
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

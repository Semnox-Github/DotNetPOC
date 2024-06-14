/********************************************************************************************
 * Project Name - Waiver 
 * Description  - RemoteWaiverSetUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************
 *2.130.0     20-Jul-2021   Mushahid Faizan    Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Waiver
{
    class RemoteWaiverSetUseCases : RemoteUseCases, IWaiverSetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string WaiverSet_URL = "api/Product/WaiverSigningOptions";
        private const string WAIVERSET_CONTAINER_URL = "api/common/WaiverSetContainer";

        public RemoteWaiverSetUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<WaiverSetDTO>> GetWaiverSets(List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> waiversSearchList,
             bool loadActiveChild = false, bool removeIncompleteRecords = false, bool getLanguageSpecificContent = false, string waiverSetSigningOptions = null)
        {
            log.LogMethodEntry(waiversSearchList, loadActiveChild, removeIncompleteRecords, getLanguageSpecificContent, waiverSetSigningOptions);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (waiversSearchList != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(waiversSearchList));
            }
            try
            {
                List<WaiverSetDTO> result = await Get<List<WaiverSetDTO>>(WaiverSet_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case WaiverSetDTO.SearchByWaiverParameters.WAIVER_SET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("waiverSetId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveWaiverSets(List<WaiverSetDTO> waiverSetDTOList)
        {
            log.LogMethodEntry(waiverSetDTOList);
            try
            {
                string responseString = await Post<string>(WaiverSet_URL, waiverSetDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<WaiverSetContainerDTOCollection> GetWaiverSetContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("languageId", languageId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            WaiverSetContainerDTOCollection result = await Get<WaiverSetContainerDTOCollection>(WAIVERSET_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}

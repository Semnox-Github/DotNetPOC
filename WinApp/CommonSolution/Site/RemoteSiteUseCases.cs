/********************************************************************************************
 * Project Name - Site
 * Description  - RemoteSiteUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 2.120.0     17-Mar-2021       Prajwal S                Modfified : Added GetSite method.
 2.150.0     09-Mar-2022       Abhishek                 Modified : Added GetUTCDateTime() as a part of SiteDateTime Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Site
{
    public class RemoteSiteUseCases : RemoteUseCases, ISiteUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SITE_URL = "api/Organization/Sites";
        private const string SITE_CONTAINER_URL = "api/Organization/SiteContainer";
        private const string UTC_DATE_TIME_URL = "api/Organization/UtcDateTime";

        public RemoteSiteUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<SiteContainerDTOCollection> GetSiteContainerDTOCollection(string hash, bool onlineEnabledOnly, bool fnBEnabledOnly, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            parameters.Add(new KeyValuePair<string, string>("onlineEnabledOnly", onlineEnabledOnly.ToString()));
            parameters.Add(new KeyValuePair<string, string>("fnBEnabledOnly", fnBEnabledOnly.ToString()));
            SiteContainerDTOCollection result = await Get<SiteContainerDTOCollection>(SITE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<SiteDTO>> GetSites(List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>
                    parameters, bool loadChildRecords, bool activeChildRecords)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<SiteDTO> result = await Get<List<SiteDTO>>(SITE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<SiteDTO.SearchBySiteParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case SiteDTO.SearchBySiteParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case SiteDTO.SearchBySiteParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SiteDTO.SearchBySiteParameters.ONLINE_ENABLED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("onlineEnabled".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<DateTime> GetUTCDateTime(bool rebuildCache)
        {
            log.LogMethodEntry(rebuildCache);
            DateTime result = await Get<DateTime>(UTC_DATE_TIME_URL, new WebApiGetRequestParameterCollection("rebuildCache", rebuildCache));
            log.LogMethodExit(result);
            return result;
        }

    }
}

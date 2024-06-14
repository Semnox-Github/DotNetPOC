/********************************************************************************************
 * Project Name - Site
 * Description  - LocalSiteUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
2.150.0     09-Mar-2022       Abhishek                Modified : Added GetUTCDateTime() as a part of SiteDateTime Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Site
{
    public class LocalSiteUseCases : LocalUseCases, ISiteUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalSiteUseCases(ExecutionContext executionContext)
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<SiteContainerDTOCollection> GetSiteContainerDTOCollection(string hash, bool onlineEnabledOnly, bool fnBEnabledOnly, bool rebuildCache)
        {
            return await Task<SiteContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    SiteContainerList.Rebuild();
                }
                SiteContainerDTOCollection result = SiteContainerList.GetSiteContainerDTOCollection();
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<List<SiteDTO>> GetSites(List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>
                            searchParameters, bool loadChildRecords, bool activeChildRecords)
        {
            return await Task<List<SiteDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                SiteList siteListBL = new SiteList(executionContext);
                List<SiteDTO> siteDTOList = siteListBL.GetAllSites(searchParameters, null, loadChildRecords, activeChildRecords);

                log.LogMethodExit(siteDTOList);
                return siteDTOList;
            });
        }

        public async Task<DateTime> GetUTCDateTime(bool rebuildCache)
        {
            return await Task<DateTime>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(rebuildCache);
                DateTime result = ServerDateTime.Now.ToUniversalTime();
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}


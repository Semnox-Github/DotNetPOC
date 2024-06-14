/********************************************************************************************
 * Project Name - GenericUtilities  Class
 * Description  - LocalLookupContainerDataService class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Core.Utilities
{
    public class LocalLookupContainerDataService : ILookupContainerDataService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLookupContainerDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<LookupsDTO> Get(DateTime? maxLastUpdatedDate, string hash)
        {
            log.LogMethodEntry(maxLastUpdatedDate, hash);
            LookupsList lookupsListBL = new LookupsList(executionContext);
            int siteId = GetSiteId();
            DateTime? updateTime = lookupsListBL.GetLookupModuleLastUpdateTime(siteId);
            DateTime updateTimeutc;
            if (updateTime.HasValue)
            {
                updateTimeutc = (DateTime)updateTime;
                if (maxLastUpdatedDate.HasValue
                && maxLastUpdatedDate >= updateTimeutc.ToUniversalTime())
                {
                    log.LogMethodExit(null, "No changes in look up module since " + maxLastUpdatedDate);
                    return null;
                }
            }
            List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.ISACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            List<LookupsDTO> lookupsDTOList = lookupsListBL.GetAllLookups(searchParameters, true);
            log.LogMethodExit(lookupsDTOList);
            return lookupsDTOList;
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
    }
}

﻿/********************************************************************************************
 * Project Name - Game
 * Description  - LocalHubContainerDataService class to get the data  from local DB 
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

namespace Semnox.Parafait.Game
{
    public class LocalHubContainerDataService : IHubContainerDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalHubContainerDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<HubDTO> Get(DateTime? maxLastUpdatedDate, string hash)
        {
            log.LogMethodEntry(maxLastUpdatedDate, hash);
            HubList hubListBL = new HubList(executionContext);
            int siteId = GetSiteId();
            DateTime? updateTime = hubListBL.GetHubModuleLastUpdateTime(siteId);
            DateTime updateTimeutc;
            if (updateTime.HasValue)
            {
                updateTimeutc = (DateTime)updateTime;
                if (maxLastUpdatedDate.HasValue
                && maxLastUpdatedDate >= updateTimeutc.ToUniversalTime())
                {
                    log.LogMethodExit(null, "No changes in Hub module since " + maxLastUpdatedDate);
                    return null;
                }
            }
            List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
            searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, siteId.ToString()));
            List<HubDTO> hubDTOList = hubListBL.GetHubSearchList(searchParameters,null,true,true);
            log.LogMethodExit(hubDTOList);
            return hubDTOList;
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

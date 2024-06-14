/********************************************************************************************
 * Project Name - Gamea 
 * Description  - LocalHubDataService class to get the data  from local DB 
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
    public class LocalHubDataService : IHubDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalHubDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<HubDTO> GetHubs(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> parameters , bool loadMachineCount = false)
        {
            log.LogMethodEntry(parameters);
            HubList hubsList = new HubList(executionContext);
            int siteId = GetSiteId();
            List<HubDTO> hubDTOList = hubsList.GetHubSearchList(parameters,null, loadMachineCount, false);
            log.LogMethodExit(hubDTOList);
            return hubDTOList;
        }

        public string PostHubs(List<HubDTO> hubDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(hubDTOList);
                HubList hubList = new HubList(executionContext, hubDTOList);
                hubList.Save();
                result = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = "Falied";
            }
            log.LogMethodExit(result);
            return result;
        }

        public string DeleteHubs(List<HubDTO> hubDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(hubDTOList);
                HubList hubList = new HubList(executionContext, hubDTOList);
                hubList.Save();
                result = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = "Falied";
            }
            log.LogMethodExit(result);
            return result;
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

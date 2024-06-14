/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - LocalThemeContainerDataService class to get the data  from local DB 
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

namespace Semnox.Parafait.DigitalSignage
{
    public class LocalThemeContainerDataService : IThemeContainerDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalThemeContainerDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<ThemeDTO> Get(DateTime? maxLastUpdatedDate, string hash)
        {
            log.LogMethodEntry(maxLastUpdatedDate, hash);
            ThemeListBL ThemeListBL = new ThemeListBL(executionContext);
            int siteId = GetSiteId();
            DateTime? updateTime = ThemeListBL.GetThemeModuleLastUpdateTime(siteId);
            DateTime updateTimeutc;
            if (updateTime.HasValue)
            {
                updateTimeutc = (DateTime)updateTime;
                if (maxLastUpdatedDate.HasValue
                && maxLastUpdatedDate >= updateTimeutc.ToUniversalTime())
                {
                    log.LogMethodExit(null, "No changes in Theme module since " + maxLastUpdatedDate);
                    return null;
                }
            }
            List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            List<ThemeDTO> ThemeDTOList = ThemeListBL.GetThemeDTOList(searchParameters,true,true);
            log.LogMethodExit(ThemeDTOList);
            return ThemeDTOList;
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

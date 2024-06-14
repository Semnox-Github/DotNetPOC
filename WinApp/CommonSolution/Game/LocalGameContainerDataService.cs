/********************************************************************************************
 * Project Name - LocalGameContainerDataService  Class
 * Description  - LocalGameContainerDataService class to get the data  from local DB 
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
    public class LocalGameContainerDataService : IGameContainerDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalGameContainerDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<GameDTO> Get(DateTime? maxLastUpdatedDate, string hash)
        {
            log.LogMethodEntry(maxLastUpdatedDate, hash);
            GameList gameListBL = new GameList(executionContext);
            int siteId = GetSiteId();
            DateTime? updateTime = gameListBL.GetGameModuleLastUpdateTime(siteId);
            DateTime updateTimeutc;
            if (updateTime.HasValue)
            {
                updateTimeutc = (DateTime)updateTime;
                if (maxLastUpdatedDate.HasValue
                && maxLastUpdatedDate >= updateTimeutc.ToUniversalTime())
                {
                    log.LogMethodExit(null, "No changes in Game module since " + maxLastUpdatedDate);
                    return null;
                }
            }
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, siteId.ToString()));
            List<GameDTO> gamesDTOList = gameListBL.GetGameList(searchParameters, true);
            log.LogMethodExit(gamesDTOList);
            return gamesDTOList;
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

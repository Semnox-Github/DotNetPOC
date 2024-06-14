/********************************************************************************************
 * Project Name - Gamea 
 * Description  - LocalGameDataService class to get the data  from local DB 
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
    public class LocalGameDataService : IGameDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalGameDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<GameDTO> GetGames(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> parameters , bool loadChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            GameList gameList = new GameList(executionContext);
            int siteId = GetSiteId();
            List<GameDTO> gameDTOList = gameList.GetGameList(parameters,loadChildRecords);
            log.LogMethodExit(gameDTOList);
            return gameDTOList;
        }

        public string PostGames(List<GameDTO> gameDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(gameDTOList);
                GameList gameList = new GameList(gameDTOList,executionContext);
                gameList.SaveUpdateGameList();
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

        public string DeleteGames(List<GameDTO> gameDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(gameDTOList);
                GameList gameList = new GameList(gameDTOList, executionContext);
                gameList.DeleteGameList();
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

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
    public class LocalGameProfileDataService : IGameProfileDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalGameProfileDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<GameProfileDTO> GetGameProfiles(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> parameters , bool loadChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            int siteId = GetSiteId();
            GameProfileList gameProfileList = new GameProfileList(executionContext,parameters);
            List<GameProfileDTO> gameProfileDTOList = gameProfileList.GetGameProfileList;
            log.LogMethodExit(gameProfileDTOList);
            return gameProfileDTOList;
        }

        public string PostGameProfiles(List<GameProfileDTO> gameProfileDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(gameProfileDTOList);
                GameProfileList gameProfileList = new GameProfileList(gameProfileDTOList, executionContext);
                gameProfileList.SaveUpdateGameProfileList();
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

        public string DeleteGameProfiles(List<GameProfileDTO> gameProfileDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(gameProfileDTOList);
                GameProfileList gameProfileList = new GameProfileList(gameProfileDTOList, executionContext);
                gameProfileList.DeleteGameProfileList();
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

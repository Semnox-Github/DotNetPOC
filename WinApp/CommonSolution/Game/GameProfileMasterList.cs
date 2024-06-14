/********************************************************************************************
 * Project Name - Games
 * Description  - GameMasterList class to get the List of games from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    public static class GameProfileMasterList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, GameProfileContainer> gameProfileContainerDictionary = new ConcurrentDictionary<int, GameProfileContainer>();
        private static readonly object locker = new object();

        private static GameProfileContainer GetGameProfileContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (gameProfileContainerDictionary.ContainsKey(executionContext.GetSiteId()) == false)
            {
                gameProfileContainerDictionary[executionContext.GetSiteId()] = new GameProfileContainer(executionContext.GetSiteId());
            }
            GameProfileContainer gameProfileContainer = gameProfileContainerDictionary[executionContext.GetSiteId()];
            log.LogMethodExit(gameProfileContainer);
            return gameProfileContainer;
        }


        public static List<GameProfileDTO> GetGameProfilesList(ExecutionContext executionContext, DateTime? maxLastUpdatedDate, string hash)
        {
            log.LogMethodEntry(executionContext, maxLastUpdatedDate, hash);
            List<GameProfileDTO> gameProfileDTOList=null;
            lock (locker)
            {
                GameProfileContainer gameProfileContainer = GetGameProfileContainer(executionContext);
                //gameProfileDTOList = gameProfileContainer.GetGameProfileDTOListModifiedAfter(maxLastUpdatedDate, hash);
            }
            log.LogMethodExit(gameProfileDTOList);
            return gameProfileDTOList;
        }

        //public static GameProfileDTO GetGameProfileDTO(ExecutionContext executionContext, int gameProfileId)
        //{
        //    log.LogMethodEntry(executionContext, gameProfileId);
        //    GameProfileDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            GameProfileContainer gameProfileContainer = GetGameProfileContainer(executionContext);
        //            result = gameProfileContainer.GetGameProfileDTO(gameProfileId);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}

        //public static GameProfileDTO GetGameProfileDTO(ExecutionContext executionContext, string profileName)
        //{
        //    log.LogMethodEntry(executionContext, profileName);
        //    GameProfileDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            GameProfileContainer gameProfileContainer = GetGameProfileContainer(executionContext);
        //            result = gameProfileContainer.GetGameProfileDTO(profileName);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}

        //public static GameProfileGetBL GetGameProfileBL(ExecutionContext executionContext, int gameId)
        //{
        //    log.LogMethodEntry(executionContext, gameId);
        //    GameProfileGetBL gameProfileGetBL;
        //    lock (locker)
        //    {
        //        GameProfileContainer gameProfileContainer = GetGameProfileContainer(executionContext);
        //        gameProfileGetBL = gameProfileContainer.GetGameProfileBL(gameId);
        //    }
        //    log.LogMethodExit(gameProfileGetBL);
        //    return gameProfileGetBL;
        //}

        /// <summary>
        /// clears the master list
        /// </summary>
        public static void Clear()
        {
            log.LogMethodEntry();
            lock (locker)
            {
                foreach (var gameContainer in gameProfileContainerDictionary.Values)
                {
                    gameContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
    }
}

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
    public static class GameMasterList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, GameContainer> gameContainerDictionary = new ConcurrentDictionary<int, GameContainer>();
        private static readonly object locker = new object();

        private static GameContainer GetGameContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (gameContainerDictionary.ContainsKey(executionContext.GetSiteId()) == false)
            {
                gameContainerDictionary[executionContext.GetSiteId()] = new GameContainer(executionContext.GetSiteId());
            }
            GameContainer gameContainer = gameContainerDictionary[executionContext.GetSiteId()];
            log.LogMethodExit(gameContainer);
            return gameContainer;
        }
        //public static List<GameDTO> GetGamesList(ExecutionContext executionContext, DateTime? maxLastUpdatedDate, string hash)
        //{
        //    log.LogMethodEntry(executionContext, maxLastUpdatedDate, hash);
        //    List<GameDTO> gameDTOList;
        //    lock (locker)
        //    {
        //        GameContainer gameContainer = GetGameContainer(executionContext);
        //        gameDTOList = gameContainer.GetGameDTOListModifiedAfter(maxLastUpdatedDate, hash);
        //    }
        //    log.LogMethodExit(gameDTOList);
        //    return gameDTOList;
        //}

        //public static GameDTO GetGameDTO(ExecutionContext executionContext, int gameId)
        //{
        //    log.LogMethodEntry(executionContext, gameId);
        //    GameDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            GameContainer gameContainer = GetGameContainer(executionContext);
        //            result = gameContainer.GetGameDTO(gameId);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}
        //public static GameDTO GetGameDTO(ExecutionContext executionContext, string gameName)
        //{
        //    log.LogMethodEntry(executionContext, gameName);
        //    GameDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            GameContainer gameContainer = GetGameContainer(executionContext);
        //            result = gameContainer.(gameName);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}


        /// <summary>
        /// clears the master list
        /// </summary>
        public static void Clear()
        {
            log.LogMethodEntry();
            lock (locker)
            {
                foreach (var gameContainer in gameContainerDictionary.Values)
                {
                    gameContainer.RebuildCache();
                }
            }
            log.LogMethodExit();
        }
    }
}

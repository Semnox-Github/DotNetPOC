/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - GameViewContainer class
 *
 **************
 ** Version Log
  **************
  * Version        Date Modified       By                Remarks
 *********************************************************************************************
 2.110.0           18-Dec-2020         Prajwal S         Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the list of Games
    /// </summary>
    public class GameViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly GameContainerDTOCollection gameDTOCollection;
        private readonly ConcurrentDictionary<int, GameContainerDTO> gameContainerDTODictionary = new ConcurrentDictionary<int, GameContainerDTO>();
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal GameViewContainer(int siteId, GameContainerDTOCollection gameDTOCollection)
        {
            log.LogMethodEntry(siteId, gameDTOCollection);
            this.siteId = siteId;
            this.gameDTOCollection = gameDTOCollection;
            //lastRefreshTime = DateTime.Now;
            if (gameDTOCollection != null &&
               gameDTOCollection.GameContainerDTOList != null &&
               gameDTOCollection.GameContainerDTOList.Any())
            {
                foreach (var gameContainerDTO in gameDTOCollection.GameContainerDTOList)
                {
                    gameContainerDTODictionary[gameContainerDTO.GameId] = gameContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal GameViewContainer(int siteId) :
            this(siteId, GetGameContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static GameContainerDTOCollection GetGameContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            GameContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IGameUseCases gameUseCases = GameUseCaseFactory.GetGameUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<GameContainerDTOCollection> gameViewDTOCollectionTask = gameUseCases.GetGameContainerDTOCollection(siteId, hash, rebuildCache);
                    gameViewDTOCollectionTask.Wait();
                    result = gameViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving GameContainerDTOCollection.", ex);
                result = new GameContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in GameDTOCollection
        /// </summary>
        /// <returns></returns>
        internal GameContainerDTOCollection GetGameDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (gameDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(gameDTOCollection);
            return gameDTOCollection;
        }

        internal List<GameContainerDTO> GetGameContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(gameDTOCollection.GameContainerDTOList);
            return gameDTOCollection.GameContainerDTOList;
        }


        /// <summary>
        /// returns the GameContainerDTO for the GameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public GameContainerDTO GetGameContainerDTO(int gameId)
        {
            log.LogMethodEntry(gameId);
            if (gameContainerDTODictionary.ContainsKey(gameId) == false)
            {
                string errorMessage = "Game with Game Id :" + gameId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            GameContainerDTO result = gameContainerDTODictionary[gameId];
            log.LogMethodExit(result);
            return result;
        }


        internal GameViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            GameContainerDTOCollection latestGameDTOCollection = GetGameContainerDTOCollection(siteId, gameDTOCollection.Hash, true);
            if (latestGameDTOCollection == null ||
                latestGameDTOCollection.GameContainerDTOList == null ||
                latestGameDTOCollection.GameContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            GameViewContainer result = new GameViewContainer(siteId, latestGameDTOCollection);
            log.LogMethodExit(result);
            return result;
        }


    }
}


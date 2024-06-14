/********************************************************************************************
 * Project Name - GameContainer  Class
 * Description  - GameContainer class to get the data    
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 2.110.0     14-Dec-2020       Prajwal S                 Updated for Changes in Container API.
 2.150.2     6-Apr-2023        Yashodhara C H            Updated the container to latest code structure.
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    class GameContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int siteId;
        private readonly DateTime? gameModuleLastUpdateTime;
        private readonly Dictionary<int, GameDTO> gameIdGameDTODictionary = new Dictionary<int, GameDTO>();
        private readonly Dictionary<int, GameContainerDTO> gameIdGameContainerDTODictionary = new Dictionary<int, GameContainerDTO>();
        private readonly List<GameDTO> gameDTOList;
        private readonly GameContainerDTOCollection gameContainerDTOCollection;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="siteId"></param>
        public GameContainer(int siteId)
            : this(siteId, GetGameDTOList(siteId), GetGameModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);

            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="gameDTOList"></param>
        /// <param name="gameModuleLastUpdateTime"></param>
        public GameContainer(int siteId, List<GameDTO> gameDTOList, DateTime? gameModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, gameDTOList, gameModuleLastUpdateTime);
            this.siteId = siteId;
            this.gameDTOList = gameDTOList;
            this.gameModuleLastUpdateTime = gameModuleLastUpdateTime;
            foreach(var gameDTO in gameDTOList)
            {
                if(gameIdGameDTODictionary.ContainsKey(gameDTO.GameId))
                {
                    continue;
                }
                gameIdGameDTODictionary.Add(gameDTO.GameId, gameDTO);
            }
            List<GameContainerDTO> gameContainerDTOList = new List<GameContainerDTO>();
            foreach (GameDTO gameDTO in gameDTOList)
            {
                if(gameIdGameContainerDTODictionary.ContainsKey(gameDTO.GameId))
                {
                    continue;
                }
                GameContainerDTO gameContainerDTO = new GameContainerDTO(gameDTO.GameId, gameDTO.GameName, gameDTO.GameDescription, gameDTO.GameCompanyName,
                gameDTO.PlayCredits, gameDTO.VipPlayCredits, gameDTO.Notes, gameDTO.GameProfileId, gameDTO.InternetKey, gameDTO.RepeatPlayDiscount,
                gameDTO.UserIdentifier, gameDTO.CustomDataSetId, gameDTO.ProductId, gameDTO.GameTag);
                gameContainerDTOList.Add(gameContainerDTO);
                gameIdGameContainerDTODictionary.Add(gameContainerDTO.GameId, gameContainerDTO);
                if(gameDTO.GamePriceTierDTOList != null)
                {
                    foreach (var gamePriceTierDTO in gameDTO.GamePriceTierDTOList)
                    {
                        GamePriceTierContainerDTO gamePriceTierContainerDTO = new GamePriceTierContainerDTO(gamePriceTierDTO.GamePriceTierId, gamePriceTierDTO.GameId, gamePriceTierDTO.GameProfileId, gamePriceTierDTO.Name, gamePriceTierDTO.Description, gamePriceTierDTO.PlayCount, gamePriceTierDTO.PlayCredits, gamePriceTierDTO.VipPlayCredits, gamePriceTierDTO.SortOrder);
                        gameContainerDTO.GamePriceTierContainerDTOList.Add(gamePriceTierContainerDTO);
                    }
                }
            }
            gameContainerDTOCollection = new GameContainerDTOCollection(gameContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the list of GameContainerDTO
        /// </summary>
        /// <returns></returns>
        internal List<GameContainerDTO> GetGameContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(gameContainerDTOCollection.GameContainerDTOList);
            return gameContainerDTOCollection.GameContainerDTOList;
        }

        /// <summary>
        /// Gets the list of GameContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal GameContainerDTOCollection GetGameContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(gameContainerDTOCollection);
            return gameContainerDTOCollection;
        }

        /// <summary>
        /// Gets the Game details based on siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static List<GameDTO> GetGameDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<GameDTO> gameDTOList = null;
            try
            {
                GameList gameList = new GameList();
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, siteId.ToString()));
            gameDTOList = gameList.GetGameList(searchParameters, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Game.", ex);
            }

            if (gameDTOList == null)
            {
                gameDTOList = new List<GameDTO>();
            }

            log.LogMethodExit(gameDTOList);
            return gameDTOList;
        }

        /// <summary>
        /// gets the Last updated time 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static DateTime? GetGameModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                GameList gameList = new GameList();
                result = gameList.GetGameModuleLastUpdateTime(siteId);
            }

            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the game max last update date.", ex);
                result = null;
            }
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// gets the Last updated time else returns null
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public GameContainerDTO GetGameContainerDTOOrDefault(int gameId)
        {
            log.LogMethodEntry(gameId);
            if (gameIdGameContainerDTODictionary.ContainsKey(gameId) == false)
            {
                string message = "Products with gameId : " + gameId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = gameIdGameContainerDTODictionary[gameId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the GameContainerDTO details based on gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public GameContainerDTO GetGameContainerDTO(int gameId)
        {
            log.LogMethodEntry(gameId);
            if (gameIdGameContainerDTODictionary.ContainsKey(gameId))
            {
                string errorMessage = "Game with game Id :" + gameId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            GameContainerDTO gameContainerDTO = gameIdGameContainerDTODictionary[gameId];
            log.LogMethodExit(gameContainerDTO);
            return gameContainerDTO;
        }

        /// <summary>
        /// Rebulds the container
        /// </summary>
        public GameContainer Refresh() //added
        {
            log.LogMethodEntry();
            DateTime? updateTime = GetGameModuleLastUpdateTime(siteId);
            if (gameModuleLastUpdateTime.HasValue
                && gameModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Game since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            GameContainer result = new GameContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }
}
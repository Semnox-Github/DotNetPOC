/********************************************************************************************
 * Project Name - Game                                                                          
 * Description  - Manages the game object. The game machines are the physical objects and they
 * are of a particular type which is referred to as game
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *1.10        12-Jan-2016   Mathew         Updated to handle hierarchy based Machine attribute
 *                                         assignment
 *2.40        27-Aug-2018   Rajiv          Modified file to hold insert,Update and Delete Method.
 *                                         Added new method SaveUpdateGameList
 *2.41        07-Nov-2018   Rajiv          Commented existing logic which does not required anymore.
 * 
 *2.60        15-Apr-2019   Akshay Gulaganji updated log.MethodEntry() and log.MethodExit()
 *2.60.2      07-May-2019   Jagan Mohana   Created new DeleteGame()
 *2.60.2      27-May-2019   Jagan Mohana    Added AuditLog code to InsertUpdateGames() for saving the Audits to DB Table DBAuditLog
 *2.70.2      26-Jul-2019   Deeksha        Modified InsertUpdateGames() to return DTO instead of id.
 *            25-Sept-2019  Jagan Mohana   Added AuditLog code to InsertUpdateGames() for saving the Audits to DB Table DBAuditLog
 2.10         24-Aug-2020   Girish Kundar  Modified: POS UI Redesign with REST API
 *2.110.0     17-Dec-2021   Prajwal S      Modified: Created new GetGamesLists for API> Added GetGameCount.
 *2.130.0     30-Sep-2021   Lakshinarayana  Modified:GamePriceTier enhancement for linux reader 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Game class to get game details for specific game or game profile.
    /// This also has method to get game attributes
    /// Game class inherits GameProfile class
    /// </summary>
    public class Game : GameProfile
    {
        private GameDTO gameDto;
        private GameDataHandler gameDataHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of Game class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public Game(SqlTransaction sqlTransaction = null) : base()
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                this.gameDto = null;
                this.gameDataHandler = new GameDataHandler(sqlTransaction);
                this.executionContext = ExecutionContext.GetExecutionContext();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        /// Constructor with the game id as the parameter
        /// Would fetch the game object from the database based on the id passed. 
        /// </summary>
        /// <param name="gameId">Game id</param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public Game(int gameId, ExecutionContext executionContext, SqlTransaction sqlTransaction = null, bool loadchildRecords = false, bool activeChildRecords = false)//added
            : this()
        {
            log.LogMethodEntry(gameId, executionContext, sqlTransaction, loadchildRecords, activeChildRecords);
            this.gameDataHandler = new GameDataHandler(sqlTransaction);
            this.executionContext = executionContext;
            this.gameDto = gameDataHandler.GetGame(gameId);
            if (gameDto != null)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME, gameDto.GameId, executionContext.GetSiteId());
                gameDto.SetAttributeList(machineAttributes);
                GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGames(new List<int> { gameId }, activeChildRecords, sqlTransaction);
                gameDto.GamePriceTierDTOList = gamePriceTierDTOList;
                if (loadchildRecords)
                {
                    Build(activeChildRecords,sqlTransaction);
                }

            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the  executionContext as the parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Game(ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
            : base(executionContext)
        {
            try
            {
                log.LogMethodEntry(executionContext, sqlTransaction);
                this.executionContext = executionContext;
                this.gameDto = null;
                this.gameDataHandler = new GameDataHandler(sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        /// Constructor with the game profile id as the parameter
        /// Would fetch the game profile object from the database based on the id passed. 
        /// </summary>
        /// <param name="gameId">Game id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Game(int gameId, SqlTransaction sqlTransaction = null, bool loadchildRecords = false, bool activeChildRecords = false) : this()//added
        {
            try
            {
                log.LogMethodEntry(gameId, sqlTransaction, loadchildRecords, activeChildRecords);
                GameDataHandler gameDataHandler = new GameDataHandler(sqlTransaction);
                gameDto = gameDataHandler.GetGame(gameId);
                if (gameDto != null)
                {
                    MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                    List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME, gameDto.GameId, executionContext.GetSiteId());
                    gameDto.SetAttributeList(machineAttributes);
                    GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                    List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGames(new List<int> { gameId }, activeChildRecords, sqlTransaction);
                    gameDto.GamePriceTierDTOList = gamePriceTierDTOList;
                    if (loadchildRecords)
                    {
                        Build(activeChildRecords,sqlTransaction);
                    }
                }
                base.GetGameProfile(gameDto.GameProfileId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }

        }

        /// <summary>
        /// Creates game object using the GameDTO
        /// </summary>
        /// <param name="game">GameDTO object</param>
        public Game(GameDTO game) : this()
        {
            try
            {
                log.LogMethodEntry(game);
                this.gameDto = game;
                base.GetGameProfile(game.GameProfileId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        /// Populates the game object based on the game id
        /// This works the same way as the constructor with the game id as parameter
        /// Provision was created so that blank object could be created first and then 
        /// the game DTO could be populated.
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void GetGame(int gameId, SqlTransaction sqlTransaction = null, bool loadchildRecords = false, bool activeChildRecords = false)
        {
            try
            {
                log.LogMethodEntry(gameId, sqlTransaction, loadchildRecords, activeChildRecords);
                GameDataHandler gameDataHandler = new GameDataHandler(sqlTransaction);
                gameDto = gameDataHandler.GetGame(gameId);

                if (gameDto != null)
                {
                    MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                    List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME, gameDto.GameId, executionContext.GetSiteId());
                    gameDto.SetAttributeList(machineAttributes);
                    GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                    List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGames(new List<int> { gameId }, activeChildRecords, sqlTransaction);
                    gameDto.GamePriceTierDTOList = gamePriceTierDTOList;
                    base.GetGameProfile(gameDto.GameProfileId);
                    if (loadchildRecords)
                    {
                        Build(activeChildRecords,sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        /// Gets the machine attributes set at the game level
        /// </summary>
        /// <param name="attribute">The machine attribute, the value of which is being requested</param>
        /// <returns>Returns the machine attribute value</returns>
        public MachineAttributeDTO GetGameMachineAttribute(MachineAttributeDTO.MachineAttribute attribute)
        {
            log.LogMethodEntry(attribute);
            foreach (MachineAttributeDTO currAttribute in gameDto.GameAttributes)
            {
                if (currAttribute.AttributeName == attribute)
                {
                    log.LogMethodExit(currAttribute);
                    return currAttribute;
                }
            }
            log.Error("The game system attribute by name " + attribute + " does not exist for game " + gameDto.GameName + ". Please check the system setup");
            //12-Jan-2016 - Throw exception if attribute not found
            throw new Exception("The game system attribute by name " + attribute + " does not exist for game " + gameDto.GameName + ". Please check the system setup");
            //return base.GetGameMachineAttribute(attribute); //12-Jan-2016
        }

        private void ValidateGamePriceTierConstraints()
        {
            log.LogMethodEntry();
            if (gameDto.IsActive == false || gameDto.GamePriceTierDTOList.Any() == false)
            {
                log.LogMethodExit("inactive game or empty child list");
                return;
            }
            var duplicateGroup = gameDto.GamePriceTierDTOList.Where(x => x.IsActive).GroupBy(x => x.SortOrder).Where(x => x.Count() > 1);
            if (duplicateGroup.Any())
            {
                var duplicateGamePriceTierDTO = duplicateGroup.First().FirstOrDefault();
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4995, duplicateGamePriceTierDTO.Name, duplicateGamePriceTierDTO.SortOrder);
                throw new ValidationException("Duplicate sort order in game price tier records.", "GamePriceTier", "SortOrder", errorMessage);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Insert or update game 
        /// Would Insert the game object in the database. 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void InsertUpdateGames(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            string userId = executionContext.GetUserId();
            int siteId = executionContext.GetSiteId();
            try
            {
                ValidateGamePriceTierConstraints();
                this.gameDataHandler = new GameDataHandler(sqlTransaction);
                if (gameDto.GameId < 0)
                {
                    gameDto = gameDataHandler.InsertGames(gameDto, userId, siteId);
                    if (!string.IsNullOrEmpty(gameDto.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("games", gameDto.Guid);
                    }
                    gameDto.AcceptChanges();
                }
                else
                {
                    if (gameDto.IsChanged)
                    {
                        gameDto = gameDataHandler.UpdateGame(gameDto, userId, siteId);
                        if (!string.IsNullOrEmpty(gameDto.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("games", gameDto.Guid);
                        }
                        gameDto.AcceptChanges();
                    }
                }

                if (gameDto.GamePriceTierDTOList != null && gameDto.GamePriceTierDTOList.Any())
                {
                    List<GamePriceTierDTO> updatedGamePriceTierDTOList = new List<GamePriceTierDTO>();
                    foreach (var gamePriceTierDTO in gameDto.GamePriceTierDTOList)
                    {
                        if (gamePriceTierDTO.GameId != gameDto.GameId)
                        {
                            gamePriceTierDTO.GameId = gameDto.GameId;
                        }
                        if (gamePriceTierDTO.IsChangedRecursive)
                        {
                            updatedGamePriceTierDTOList.Add(gamePriceTierDTO);
                        }
                    }
                    if (updatedGamePriceTierDTOList.Any())
                    {
                        GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL(executionContext, updatedGamePriceTierDTOList);
                        gamePriceTierListBL.Save(sqlTransaction);
                    }

                }
                SaveAllowedMachineNamesList(sqlTransaction);
                gameDto.AcceptChanges();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit();
        }
        private void SaveAllowedMachineNamesList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (gameDto != null && gameDto.AllowedMachineDTOList != null &&
                gameDto.AllowedMachineDTOList.Any())
            {
                foreach (var allowedMachineList in gameDto.AllowedMachineDTOList)
                {
                    if (allowedMachineList.GameId != gameDto.GameId)
                    {
                        allowedMachineList.GameId = gameDto.GameId;
                    }
                }
                AllowedMachineNamesListBL allowedMachineNamesListBL = new AllowedMachineNamesListBL(executionContext, gameDto.AllowedMachineDTOList);
                allowedMachineNamesListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the GameDTO based on gameId
        /// </summary>
        /// <param name="gameId"></param>
        internal void DeleteGame(int gameId)
        {
            log.LogMethodEntry(gameId);
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            try
            {
                parafaitDBTrx.BeginTransaction();
                sqlTransaction = parafaitDBTrx.SQLTrx;
                GameSystemList gameSystemList = new GameSystemList(executionContext);
                List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>>();
                searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.GAME_ID, Convert.ToString(gameId)));
                List<MachineAttributeDTO> machineAttributeDTOList = gameSystemList.GetMachineAttributeDTOList(searchByParameters, MachineAttributeDTO.AttributeContext.GAME);
                if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
                {
                    foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
                    {
                        GameSystem gameSystem = new GameSystem();
                        gameSystem.DeleteMachineAttribute(machineAttributeDTO.AttributeId, gameId, executionContext.GetSiteId(), MachineAttributeDTO.AttributeContext.GAME, sqlTransaction);
                    }
                }
                GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGames(new List<int> { gameId }, false, sqlTransaction);
                if(gamePriceTierDTOList != null)
                {
                    foreach (var gamePriceTierDTO in gamePriceTierDTOList)
                    {
                        GamePriceTierBL gamePriceTierBL = new GamePriceTierBL(executionContext, gamePriceTierDTO);
                        gamePriceTierBL.Delete(parafaitDBTrx.SQLTrx);
                    }
                }
                this.gameDataHandler = new GameDataHandler(sqlTransaction);
                gameDataHandler.DeleteGame(gameId);
                parafaitDBTrx.EndTransaction();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                parafaitDBTrx.RollBack();
                throw;
            }
        }
        /// <summary>
        /// Builds the child records for AllowedMachineNamesDTO object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AllowedMachineNamesListBL allowedMachineNamesListBL = new AllowedMachineNamesListBL(executionContext);
            List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.GAME_ID, gameDto.GameId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.IS_ACTIVE, "Y"));
            }
            gameDto.AllowedMachineDTOList = allowedMachineNamesListBL.GetAllowedMachineNamesList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// get UserDto Object
        /// </summary>
        public GameDTO GetGameDTO
        {
            get { return gameDto; }
        }

        /// <summary>
        /// set UserDto Object
        /// To update jwt field in db table, the updated userdto will be passed from WEB API Controller.
        /// Manoj - 23/sep/2018
        /// </summary>
        public GameDTO SetGameDTO
        {
            set { gameDto = value; }
        }

    }

    /// <summary>
    /// Manages the list of games
    /// </summary>
    public class GameList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<GameDTO> gameDTOList;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// GameList Constructor
        /// </summary>
        public GameList()
        {
            log.LogMethodEntry();
            this.executionContext = ExecutionContext.GetExecutionContext();
            this.gameDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// GameList Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public GameList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.gameDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GameList Constructor
        /// </summary>
        /// <param name="gameDTOList">gameDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public GameList(List<GameDTO> gameDTOList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(gameDTOList, executionContext);
            this.gameDTOList = gameDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Game list matching with search key
        /// </summary>
        /// <param name="searchParameters">Hold the values [GameDTO.SearchByGameParameters,string] type as search key</param>
        /// <param name="loadAttributes">loadAttributes</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<GameDTO> GetGameList(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters, bool loadAttributes = true,
                                           SqlTransaction sqlTransaction = null,
                                           int currentPage = 0, int pageSize = 0, bool activeChildRecords = true)
        {
            try
            {
                log.LogMethodEntry(searchParameters, loadAttributes, sqlTransaction, activeChildRecords);
                GameDataHandler gameDataHandler = new GameDataHandler(sqlTransaction);
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                gameDTOList = gameDataHandler.GetGameList(searchParameters, currentPage, pageSize);
                if (gameDTOList != null && gameDTOList.Any() && loadAttributes)
                {
                    Dictionary<int, GameDTO> gameIdGameDTODictionary = new Dictionary<int, GameDTO>();
                    foreach (GameDTO gameDTO in gameDTOList)
                    {
                        List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME, gameDTO.GameId, executionContext.GetSiteId());
                        gameDTO.SetAttributeList(machineAttributes);
                        if (gameIdGameDTODictionary.ContainsKey(gameDTO.GameId))
                        {
                            continue;
                        }
                        gameIdGameDTODictionary.Add(gameDTO.GameId, gameDTO);
                    }
                    GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                    List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGames(gameIdGameDTODictionary.Keys.ToList(), activeChildRecords, sqlTransaction);
                    if(gamePriceTierDTOList != null)
                    {
                        foreach (var gamePriceTierDTO in gamePriceTierDTOList)
                        {
                            if (gameIdGameDTODictionary.ContainsKey(gamePriceTierDTO.GameId))
                            {
                                GameDTO gameDTO = gameIdGameDTODictionary[gamePriceTierDTO.GameId];
                                if (gameDTO.GamePriceTierDTOList == null)
                                {
                                    gameDTO.GamePriceTierDTOList = new List<GamePriceTierDTO>();
                                }
                                gameDTO.GamePriceTierDTOList.Add(gamePriceTierDTO);
                            }
                        }
                    }
                    Build(gameDTOList, activeChildRecords, sqlTransaction);
                }
                log.LogMethodExit(gameDTOList);
                return gameDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the Game list matching with search key and siteId.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="siteId"></param>
        /// <param name="loadAttributes"></param>
        /// <param name="sqlTransaction"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<GameDTO> GetGameList(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters, int siteId, bool loadAttributes = true,
                                           SqlTransaction sqlTransaction = null,
                                           int currentPage = 0, int pageSize = 0, bool activeChildRecords = true)
        {
            try
            {
                log.LogMethodEntry(searchParameters, loadAttributes, sqlTransaction);
                GameDataHandler gameDataHandler = new GameDataHandler(sqlTransaction);
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                gameDTOList = gameDataHandler.GetGameList(searchParameters, currentPage, pageSize);
                if (gameDTOList != null && gameDTOList.Any() && loadAttributes)
                {
                    Dictionary<int, GameDTO> gameIdGameDTODictionary = new Dictionary<int, GameDTO>();
                    foreach (GameDTO gameDTO in gameDTOList)
                    {
                        List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME, gameDTO.GameId, siteId);
                        gameDTO.SetAttributeList(machineAttributes);
                        if (gameIdGameDTODictionary.ContainsKey(gameDTO.GameId))
                        {
                            continue;
                        }
                        gameIdGameDTODictionary.Add(gameDTO.GameId, gameDTO);
                    }
                    GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                    List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGames(gameIdGameDTODictionary.Keys.ToList(), activeChildRecords, sqlTransaction);
                    if (gamePriceTierDTOList != null)
                    {
                        foreach (var gamePriceTierDTO in gamePriceTierDTOList)
                        {
                            if (gameIdGameDTODictionary.ContainsKey(gamePriceTierDTO.GameId))
                            {
                                GameDTO gameDTO = gameIdGameDTODictionary[gamePriceTierDTO.GameId];
                                if (gameDTO.GamePriceTierDTOList == null)
                                {
                                    gameDTO.GamePriceTierDTOList = new List<GamePriceTierDTO>();
                                }
                                gameDTO.GamePriceTierDTOList.Add(gamePriceTierDTO);
                            }
                        }
                    }
                    Build(gameDTOList, activeChildRecords, sqlTransaction);
                }
                log.LogMethodExit(gameDTOList);
                return gameDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the Game List
        /// </summary>
        public List<GameDTO> SaveUpdateGameList(SqlTransaction sqlTransaction = null)
        {
            List<GameDTO> gameDTOLists = new List<GameDTO>();
            try
            {
                log.LogMethodEntry();
                
                if (gameDTOList != null && gameDTOList.Any())
                {
                    foreach (GameDTO gamedto in gameDTOList)
                    {
                        Game game = new Game(executionContext);
                        game.SetGameDTO = gamedto;
                        List<ValidationError> validationErrors = Validations(gamedto, sqlTransaction);//added validations
                        if (validationErrors != null && validationErrors.Any())
                        {
                            throw new ValidationException("Validation Failed", validationErrors);
                        }
                        game.InsertUpdateGames(sqlTransaction);
                        gameDTOLists.Add(game.GetGameDTO);
                    }
                }
                
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                log.Error(ex);
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 652));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 544));
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(gameDTOLists);
            return gameDTOLists;
        }


        /// <summary>
        /// Deletes the Game List
        /// </summary>
        public void DeleteGameList()
        {
            try
            {
                log.LogMethodEntry();
                if (gameDTOList != null && gameDTOList.Any())
                {
                    foreach (GameDTO gameDTO in gameDTOList)
                    {
                        Game game = new Game(executionContext);
                        game.DeleteGame(gameDTO.GameId);
                    }
                }
                log.LogMethodExit();
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        private void Build(List<GameDTO> gamesDTOList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            Dictionary<int, GameDTO> gameIdDTODictionary = new Dictionary<int, GameDTO>();
            List<int> gameIdList = new List<int>();
            if (gamesDTOList != null && gamesDTOList.Any())
            {
                for (int i = 0; i < gamesDTOList.Count; i++)
                {
                    if (gamesDTOList[i].GameId == -1 ||
                        gameIdDTODictionary.ContainsKey(gamesDTOList[i].GameId))
                    {
                        continue;
                    }

                    gameIdList.Add(gamesDTOList[i].GameId);
                    gameIdDTODictionary.Add(gamesDTOList[i].GameId, gamesDTOList[i]);
                }
                AllowedMachineNamesListBL allowedMachineNamesListBL = new AllowedMachineNamesListBL(executionContext);
                List<AllowedMachineNamesDTO> allowedMachineNamesDTOList = allowedMachineNamesListBL.GetAllowedMachineNamesListOfGames(gameIdList, activeChildRecords, sqlTransaction);

                if (allowedMachineNamesDTOList != null && allowedMachineNamesDTOList.Any())
                {
                    log.LogVariableState("allowedMachineListDTOList", allowedMachineNamesDTOList);
                    foreach (AllowedMachineNamesDTO allowedMachineNamesDTO in allowedMachineNamesDTOList)
                    {
                        if (gameIdDTODictionary.ContainsKey(allowedMachineNamesDTO.GameId))
                        {
                            if (gameIdDTODictionary[allowedMachineNamesDTO.GameId].AllowedMachineDTOList == null)
                            {
                                gameIdDTODictionary[allowedMachineNamesDTO.GameId].AllowedMachineDTOList = new List<AllowedMachineNamesDTO>();
                            }
                            gameIdDTODictionary[allowedMachineNamesDTO.GameId].AllowedMachineDTOList.Add(allowedMachineNamesDTO);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the game list
        /// </summary>
        public List<GameDTO> GameDTOList { get { return gameDTOList; } }

        public DateTime? GetGameModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            GameDataHandler gameDataHandler = new GameDataHandler();
            DateTime? result = gameDataHandler.GetGameModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the no of Game matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetGameCount(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added for pagination
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            GameDataHandler gameDataHandler = new GameDataHandler(sqlTransaction);
            int gamesCount = gameDataHandler.GetGamesCount(searchParameters);
            log.LogMethodExit(gamesCount);
            return gamesCount;
        }
        private List<ValidationError> Validations(GameDTO gameDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(gameDTO);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.GAME_NAME, gameDTO.GameName));
            GameDataHandler gameDataHandler = new GameDataHandler(sqlTransaction);
            List<GameDTO> gameDTOLists = gameDataHandler.GetGameList(searchParameters);
            if (gameDTOLists != null && gameDTOLists.Any() && gameDTO != null)
            {
                List<GameDTO> gameNameExist = gameDTOLists.Where(g => !string.IsNullOrWhiteSpace(gameDTO.GameName.ToLower())
                            && g.GameName.ToLower() == gameDTO.GameName.ToLower()
                            && g.GameId != gameDTO.GameId).ToList();
                if (gameNameExist != null && gameNameExist.Any())
                {
                    validationErrorList.Add(new ValidationError("Game", "Game Name", MessageContainerList.GetMessage(executionContext, 652))); //Please enter unique Game Names
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;

        }
    }
}


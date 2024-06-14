/********************************************************************************************
 * Project Name - Game Profile                                                                          
 * Description  - Game Profile is used to define the type of game. The games could be 
 * of the redemption type, video game type etc
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *1.10        12-Jan-2016   Mathew         Updated to handle hierarchy based Machine attribute
 *                                         assignment
 *1.20        12-Dec-2016   Vivek          New Generic Method BuildMachineListParams added
 *                                         Builds MachineDTO.SearchByMachineParameters from MachineParams
 *                                         Added New Method GetGameMachine
 *2.40        3-Aug-2018    Jagan          For Game Profile API Modifications for insert/update,delete.
 *                                         Added New Mathods - SaveUpdateGameProfileList 
 *2.60        15-Apr-2019   Akshay Gulaganji  updated log.LogMethodEntry and log.LogMethodExit()
 *2.60.2      07-May-2019   Jagan Mohana   Created new DeleteGameProfile()
 *2.60.2      27-May-2019   Jagan Mohana    Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 *2.70.2        26-Jul-2019   Deeksha       Modified Save function to return DTO instead of id.
 *            25-Sept-2019  Jagan Mohana    Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 *2.80.0      25-Mar-2020   Girish Kundar   Modified:  for saving the Audits to DB Table DBAuditLog
  *2.110.0      16-Dec-2020  Prajwal S        Added Contructir with no parameter for list. Added GetgameProfile Count, added
 *                                           new GetGameProfileLists for API.
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Game Profile is used to define the type of game.
    /// The games could be of the redemption type, video game type etc
    /// </summary>
    public class GameProfile : GameSystem
    {
        private GameProfileDTO gameProfileDto;
        private GameProfileDataHandler gameProfileDataHandler;
        private readonly ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of GameProfile class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public GameProfile(SqlTransaction sqlTransaction = null) : base()
        {
            log.LogMethodEntry(sqlTransaction);
            this.executionContext = ExecutionContext.GetExecutionContext();
            this.gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
            this.gameProfileDto = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the game profile id as the parameter
        /// Would fetch the game profile object from the database based on the id passed. 
        /// </summary>
        /// <param name="gameProfileId">Game profile id</param>
        /// <param name="executionContext">executionContext</param>
        public GameProfile(int gameProfileId, ExecutionContext executionContext)
            : this()
        {
            log.LogMethodEntry(gameProfileId, executionContext);
            this.executionContext = executionContext;
            this.gameProfileDto = gameProfileDataHandler.GetGameProfile(gameProfileId);
            if (gameProfileDto != null)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME_PROFILE, gameProfileId, executionContext.GetSiteId());
                gameProfileDto.SetAttributeList(machineAttributes);
                GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGameProfiles(new List<int> { gameProfileId }, true, sqlTransaction);
                gameProfileDto.GamePriceTierDTOList = gamePriceTierDTOList;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates game profile object using the GameProfileDTO
        /// </summary>
        /// <param name="gameProfile">GameProfileDTO object</param>
        public GameProfile(GameProfileDTO gameProfile)
            : this()
        {
            log.LogMethodEntry(gameProfile);
            this.gameProfileDto = gameProfile;
            log.LogMethodExit();
        }

        /// <summary>
        /// Populates the game profile object based on the game profile id
        /// This works the same way as the constructor with the game profile id as parameter
        /// Provision was created so that blank object could be created first and then 
        /// the game profile DTO could be populated.
        /// </summary>
        /// <param name="gameProfileId">Is an integer number used for game profile identification</param>
        public void GetGameProfile(int gameProfileId)
        {
            // Kiran - This method duplicates the constructor and ideally should not have been there
            // But I was not able to handle the machine object being created and during the process ensuring 
            // that the parents are properly populated and had to resort to this method
            log.LogMethodEntry(gameProfileId);
            this.gameProfileDto = gameProfileDataHandler.GetGameProfile(gameProfileId);
            if (gameProfileDto != null)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME_PROFILE, gameProfileId, executionContext.GetSiteId());
                gameProfileDto.SetAttributeList(machineAttributes);
                GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGameProfiles(new List<int> { gameProfileId }, true, sqlTransaction);
                gameProfileDto.GamePriceTierDTOList = gamePriceTierDTOList;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public GameProfile(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
            this.gameProfileDto = null;
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the machine attributes set at the game profile level
        /// </summary>
        /// <param name="attribute">The machine attribute, the value of which is being requested</param>
        /// <returns>Returns the machine attribute value</returns>
        public MachineAttributeDTO GetGameProfileMachineAttribute(MachineAttributeDTO.MachineAttribute attribute)
        {
            log.LogMethodEntry(attribute);
            foreach (MachineAttributeDTO currAttribute in gameProfileDto.ProfileAttributes)
            {
                if (currAttribute.AttributeName == attribute)
                {
                    log.LogMethodExit(currAttribute);
                    return currAttribute;
                }
            }
            log.Error("The game system attribute by name " + attribute + " does not exist for game profile " + gameProfileDto.ProfileName + ". Please check the system setup");
            //12-Jan-2016 - Throw exception if attribute not found
            throw new Exception("The game system attribute by name " + attribute + " does not exist for game profile " + gameProfileDto.ProfileName + ". Please check the system setup");
            //return base.GetGameMachineAttribute(attribute); //12-Jan-2016
        }

        private void ValidateGamePriceTierConstraints()
        {
            log.LogMethodEntry();
            if (gameProfileDto.IsActive == false || gameProfileDto.GamePriceTierDTOList.Any() == false)
            {
                log.LogMethodExit("inactive game profile or empty child list");
                return;
            }
            var duplicateGroup = gameProfileDto.GamePriceTierDTOList.Where(x => x.IsActive).GroupBy(x => x.SortOrder).Where(x => x.Count() > 1);
            if (duplicateGroup.Any())
            {
                var duplicateGamePriceTierDTO = duplicateGroup.First().FirstOrDefault();
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4995, duplicateGamePriceTierDTO.Name, duplicateGamePriceTierDTO.SortOrder);
                throw new ValidationException("Duplicate sort order in game price tier records.", "GamePriceTier", "SortOrder", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save/Update the Game Profile Details
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                string userId = executionContext.GetUserId();
                int siteId = executionContext.GetSiteId();
                ValidateGamePriceTierConstraints();
                GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
                if (gameProfileDto.GameProfileId < 0)
                {
                    gameProfileDto = gameProfileDataHandler.InsertGameProfile(gameProfileDto, userId, siteId);
                    SaveAndUpdateGameProfileAttributes(userId, siteId);
                    gameProfileDto.AcceptChanges();
                    AddManagementFormAccess(sqlTransaction);
                }
                else
                {
                    if (gameProfileDto.IsChanged)
                    {
                        GameProfileDTO existinGameProfileDTO = new GameProfile(gameProfileDto.GameProfileId, executionContext).GetGameProfileDTO;
                        gameProfileDto = gameProfileDataHandler.UpdateGameProfile(gameProfileDto, userId, siteId);
                        SaveAndUpdateGameProfileAttributes(userId, siteId);
                        gameProfileDto.AcceptChanges();
                        if (existinGameProfileDTO.ProfileName.ToLower().ToString() != gameProfileDto.ProfileName.ToLower().ToString())
                        {
                            RenameManagementFormAccess(existinGameProfileDTO.ProfileName, sqlTransaction);
                        }
                        if (existinGameProfileDTO.IsActive != gameProfileDto.IsActive)
                        {
                            UpdateManagementFormAccess(gameProfileDto, sqlTransaction);
                        }
                    }
                }
                if (gameProfileDto.GamePriceTierDTOList != null && gameProfileDto.GamePriceTierDTOList.Any())
                {
                    List<GamePriceTierDTO> updatedGamePriceTierDTOList = new List<GamePriceTierDTO>();
                    foreach (var gamePriceTierDTO in gameProfileDto.GamePriceTierDTOList)
                    {
                        if (gamePriceTierDTO.GameProfileId != gameProfileDto.GameProfileId)
                        {
                            gamePriceTierDTO.GameProfileId = gameProfileDto.GameProfileId;
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
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        private void AddManagementFormAccess(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (gameProfileDto.GameProfileId > -1)
            {
                GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
                gameProfileDataHandler.AddManagementFormAccess(gameProfileDto.ProfileName, gameProfileDto.Guid, executionContext.GetSiteId(), gameProfileDto.IsActive);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void RenameManagementFormAccess(string existingFormName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (gameProfileDto.GameProfileId > -1)
            {
                GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
                gameProfileDataHandler.RenameManagementFormAccess(gameProfileDto.ProfileName, existingFormName, executionContext.GetSiteId(), gameProfileDto.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void UpdateManagementFormAccess(GameProfileDTO gameProfileDto, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (gameProfileDto.GameProfileId > -1)
            {
                GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
                gameProfileDataHandler.UpdateManagementFormAccess(gameProfileDto.ProfileName, executionContext.GetSiteId(), gameProfileDto.IsActive, gameProfileDto.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void SaveAndUpdateGameProfileAttributes(string userId, int siteId)
        {
            log.LogMethodEntry();
            if (gameProfileDto.ProfileAttributes != null && gameProfileDto.ProfileAttributes.Any())
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                MachineAttributeDTO machineAttributeDTO = new MachineAttributeDTO();
                foreach (MachineAttributeDTO currAttribute in gameProfileDto.ProfileAttributes)
                {
                    machineAttributeDTO = currAttribute;
                    if (machineAttributeDTO.AttributeId == -1 && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.GAME_PROFILE)
                    {
                        machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.GAME_PROFILE, gameProfileDto.GameProfileId, userId, siteId);
                        /// Below code is to save the Audit log details into DBAuditLog
                        if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                        }
                        machineAttributeDTO.AcceptChanges();
                    }
                    else
                    {
                        if (machineAttributeDTO.IsChanged == true && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.GAME_PROFILE)
                        {
                            machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.GAME_PROFILE, gameProfileDto.GameProfileId, userId, siteId);
                            /// Below code is to save the Audit log details into DBAuditLog
                            if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                            {
                                AuditLog auditLog = new AuditLog(executionContext);
                                auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                            }
                            machineAttributeDTO.AcceptChanges();
                        }
                    }

                }
            }

            /// Below code is to save the Audit log details into DBAuditLog
            if (!string.IsNullOrEmpty(gameProfileDto.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("game_profile", gameProfileDto.Guid);
            }
            log.LogMethodExit();

        }
        /// <summary>
        /// Delete the GameProfileDTO based on gameId
        /// </summary>
        /// <param name="gameProfileId">gameProfileId</param>        
        public void DeleteGameProfile(GameProfileDTO gameProfileDTO)
        {
            log.LogMethodEntry(gameProfileDTO);
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            log.LogMethodEntry(gameProfileDTO);
            try
            {
                parafaitDBTrx.BeginTransaction();
                sqlTransaction = parafaitDBTrx.SQLTrx;
                GameSystemList gameSystemList = new GameSystemList(executionContext);
                List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>>();
                searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.GAME_PROFILE_ID, Convert.ToString(gameProfileDTO.GameProfileId)));
                List<MachineAttributeDTO> machineAttributeDTOList = gameSystemList.GetMachineAttributeDTOList(searchByParameters, MachineAttributeDTO.AttributeContext.GAME_PROFILE);
                if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
                {
                    foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
                    {
                        GameSystem gameSystem = new GameSystem();
                        gameSystem.DeleteMachineAttribute(machineAttributeDTO.AttributeId, gameProfileDTO.GameProfileId, executionContext.GetSiteId(), MachineAttributeDTO.AttributeContext.GAME_PROFILE, sqlTransaction);
                    }
                }

                GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGameProfiles(new List<int> { gameProfileDTO.GameProfileId }, false, sqlTransaction);
                if (gamePriceTierDTOList != null)
                {
                    foreach (var gamePriceTierDTO in gamePriceTierDTOList)
                    {
                        GamePriceTierBL gamePriceTierBL = new GamePriceTierBL(executionContext, gamePriceTierDTO);
                        gamePriceTierBL.Delete(parafaitDBTrx.SQLTrx);
                    }
                }

                this.gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
                UpdateManagementFormAccess(gameProfileDTO, sqlTransaction);
                gameProfileDataHandler.DeleteGameProfile(gameProfileDTO.GameProfileId);
                parafaitDBTrx.EndTransaction();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                parafaitDBTrx.RollBack();
                throw;
            }
        }

        /// <summary>
        /// get UserDto Object
        /// </summary>
        public GameProfileDTO GetGameProfileDTO
        {
            get { return gameProfileDto; }
        }

        /// <summary>
        /// set UserDto Object
        /// To update jwt field in db table, the updated userdto will be passed from WEB API Controller.
        /// Manoj - 23/sep/2018
        /// </summary>
        public GameProfileDTO SetGameProfileDTO
        {
            set { gameProfileDto = value; }
        }
    }

    /// <summary>
    /// Manages the list of game profiles
    /// </summary>
    public class GameProfileList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<GameProfileDTO> gameProfileDTOList;
        private readonly ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        /// <summary>
        ///constructor with no parameter
        /// </summary>
        /// <param name="executionContext"></param>
        public GameProfileList()
        {
            log.LogMethodEntry();
            this.gameProfileDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// paramterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public GameProfileList(ExecutionContext executionContext)
                : this()
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="gameProfilelist">gameProfilelist</param>
        /// <param name="executionContext">executionContext</param>
        public GameProfileList(List<GameProfileDTO> gameProfileDTOList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(gameProfileDTOList, executionContext);
            this.executionContext = executionContext;
            this.gameProfileDTOList = gameProfileDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Gets the Game Profile List matching the search key
        /// </summary>
        /// <param name="searchParameters">List of key valye pair containing the search parameter and search values</param>        
        /// <param name="sqlTransaction">sqlTransaction</param>
        public GameProfileList(ExecutionContext executionContext, List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters, SqlTransaction sqlTransaction = null,
                                                int currentPage = 0, int pageSize = 0, bool activeChildRecords = true)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, searchParameters, sqlTransaction);
            GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
            gameProfileDTOList = gameProfileDataHandler.GetGameProfileList(searchParameters, currentPage, pageSize );
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            if (gameProfileDTOList != null && gameProfileDTOList.Any())
            {
                Dictionary<int, GameProfileDTO> gameProfileIdGameDTODictionary = new Dictionary<int, GameProfileDTO>();
                foreach (GameProfileDTO gameProfileDTO in gameProfileDTOList)
                {
                    List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME_PROFILE, gameProfileDTO.GameProfileId, executionContext.GetSiteId());
                    gameProfileDTO.SetAttributeList(machineAttributes);
                    if(gameProfileIdGameDTODictionary.ContainsKey(gameProfileDTO.GameProfileId) == false)
                    {
                        gameProfileIdGameDTODictionary.Add(gameProfileDTO.GameProfileId, gameProfileDTO);
                    }
                }
                GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGameProfiles(gameProfileIdGameDTODictionary.Keys.ToList(), activeChildRecords, sqlTransaction);
                if (gamePriceTierDTOList != null)
                {
                    foreach (var gamePriceTierDTO in gamePriceTierDTOList)
                    {
                        if (gameProfileIdGameDTODictionary.ContainsKey(gamePriceTierDTO.GameProfileId))
                        {
                            GameProfileDTO gameProfileDTO = gameProfileIdGameDTODictionary[gamePriceTierDTO.GameProfileId];
                            if (gameProfileDTO.GamePriceTierDTOList == null)
                            {
                                gameProfileDTO.GamePriceTierDTOList = new List<GamePriceTierDTO>();
                            }
                            gameProfileDTO.GamePriceTierDTOList.Add(gamePriceTierDTO);
                        }
                    }
                }

            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Gets the Game Profile List matching the search key
        /// </summary>
        /// <param name="searchParameters">List of key valye pair containing the search parameter and search values</param>        
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<GameProfileDTO> GetGameProfileDTOList(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters,
                                     bool loadAttributes = true, int currentPage = 0, int pageSize = 0,
                                     SqlTransaction sqlTransaction = null, bool activeChildRecords = true) //added
        {
            log.LogMethodEntry(searchParameters, loadAttributes, sqlTransaction);
            GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
            gameProfileDTOList = gameProfileDataHandler.GetGameProfileList(searchParameters, currentPage, pageSize);
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            if (gameProfileDTOList != null && gameProfileDTOList.Any() && loadAttributes)
            {
                Dictionary<int, GameProfileDTO> gameProfileIdGameDTODictionary = new Dictionary<int, GameProfileDTO>();
                foreach (GameProfileDTO gameProfileDTO in gameProfileDTOList)
                {
                    List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME_PROFILE, gameProfileDTO.GameProfileId, executionContext.GetSiteId());
                    gameProfileDTO.SetAttributeList(machineAttributes);
                    if (gameProfileIdGameDTODictionary.ContainsKey(gameProfileDTO.GameProfileId) == false)
                    {
                        gameProfileIdGameDTODictionary.Add(gameProfileDTO.GameProfileId, gameProfileDTO);
                    }
                }
                GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGameProfiles(gameProfileIdGameDTODictionary.Keys.ToList(), activeChildRecords, sqlTransaction);
                if (gamePriceTierDTOList != null)
                {
                    foreach (var gamePriceTierDTO in gamePriceTierDTOList)
                    {
                        if (gameProfileIdGameDTODictionary.ContainsKey(gamePriceTierDTO.GameProfileId))
                        {
                            GameProfileDTO gameProfileDTO = gameProfileIdGameDTODictionary[gamePriceTierDTO.GameProfileId];
                            if (gameProfileDTO.GamePriceTierDTOList == null)
                            {
                                gameProfileDTO.GamePriceTierDTOList = new List<GamePriceTierDTO>();
                            }
                            gameProfileDTO.GamePriceTierDTOList.Add(gamePriceTierDTO);
                        }
                    }
                }
            }
            log.LogMethodExit(gameProfileDTOList);
            return gameProfileDTOList;
        }


        /// <summary>
        /// Gets the Game Profile List matching the search key and siteId.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="siteId"></param>
        /// <param name="loadAttributes"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<GameProfileDTO> GetGameProfileDTOList(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters, int siteId,
                                     bool loadAttributes = true, int currentPage = 0, int pageSize = 0,
                                     SqlTransaction sqlTransaction = null, bool activeChildRecords = true) //added
        {
            log.LogMethodEntry(searchParameters, loadAttributes, sqlTransaction);
            GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
            gameProfileDTOList = gameProfileDataHandler.GetGameProfileList(searchParameters, currentPage, pageSize);
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            if (gameProfileDTOList != null && gameProfileDTOList.Any() && loadAttributes)
            {
                Dictionary<int, GameProfileDTO> gameProfileIdGameDTODictionary = new Dictionary<int, GameProfileDTO>();
                foreach (GameProfileDTO gameProfileDTO in gameProfileDTOList)
                {
                    List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME_PROFILE, gameProfileDTO.GameProfileId, siteId);
                    gameProfileDTO.SetAttributeList(machineAttributes);
                    if (gameProfileIdGameDTODictionary.ContainsKey(gameProfileDTO.GameProfileId) == false)
                    {
                        gameProfileIdGameDTODictionary.Add(gameProfileDTO.GameProfileId, gameProfileDTO);
                    }
                }
                GamePriceTierListBL gamePriceTierListBL = new GamePriceTierListBL();
                List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierListBL.GetGamePriceTierDTOListOfGameProfiles(gameProfileIdGameDTODictionary.Keys.ToList(), activeChildRecords, sqlTransaction);
                if (gamePriceTierDTOList != null)
                {
                    foreach (var gamePriceTierDTO in gamePriceTierDTOList)
                    {
                        if (gameProfileIdGameDTODictionary.ContainsKey(gamePriceTierDTO.GameProfileId))
                        {
                            GameProfileDTO gameProfileDTO = gameProfileIdGameDTODictionary[gamePriceTierDTO.GameProfileId];
                            if (gameProfileDTO.GamePriceTierDTOList == null)
                            {
                                gameProfileDTO.GamePriceTierDTOList = new List<GamePriceTierDTO>();
                            }
                            gameProfileDTO.GamePriceTierDTOList.Add(gamePriceTierDTO);
                        }
                    }
                }
            }
            log.LogMethodExit(gameProfileDTOList);
            return gameProfileDTOList;
        }

        /// <summary>
        /// Saves the Game Profile details
        /// </summary>
        public List<GameProfileDTO> SaveUpdateGameProfileList(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                if (gameProfileDTOList != null && gameProfileDTOList.Any())
                {
                    foreach (GameProfileDTO gameProfileDto in gameProfileDTOList)
                    {
                        /// The incoming list will have ThemeId as negative and positive IDs
                        /// -1, -2 represents fo rrecords which are added newly. 
                        /// The Positive ThemeId will be for the records which are edited and IsChanged=true
                        GameProfile gameProfile = new GameProfile(executionContext);
                        gameProfile.SetGameProfileDTO = gameProfileDto;
                        List<ValidationError> validationErrors = Validations(gameProfileDto,sqlTransaction);
                        if (validationErrors != null && validationErrors.Any())
                        {
                            throw new ValidationException("Validation Failed", validationErrors);
                        }
                        gameProfile.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                log.Error(ex);
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 653));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 543));
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
            return gameProfileDTOList;
        }

        /// <summary>
        /// Delete the Game Profiles List 
        /// </summary>
        public void DeleteGameProfileList()
        {
            try
            {
                log.LogMethodEntry();
                if (gameProfileDTOList != null && gameProfileDTOList.Any())
                {
                    foreach (GameProfileDTO gameProfileDTO in gameProfileDTOList)
                    {
                        GameProfile gameProfile = new GameProfile(executionContext);
                        gameProfile.DeleteGameProfile(gameProfileDTO); 
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

        /// <summary>
        /// Returns the game profile list
        /// </summary>
        public List<GameProfileDTO> GetGameProfileList { get { return gameProfileDTOList; } }
        public DateTime? GetGameProfileModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler();
            DateTime? result = gameProfileDataHandler.GetGameProfileModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public int GetGameProfileCount(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
            int gameProfilesCount = gameProfileDataHandler.GetGameProfilesCount(searchParameters);
            log.LogMethodExit(gameProfilesCount);
            return gameProfilesCount;
        }
        private List<ValidationError> Validations(GameProfileDTO gameProfileDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(gameProfileDTO);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_NAME, gameProfileDTO.ProfileName));
            GameProfileDataHandler gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
            List<GameProfileDTO> gameprofileDTOLists = gameProfileDataHandler.GetGameProfileList(searchParameters);
            if (gameprofileDTOLists != null && gameprofileDTOLists.Any() && gameProfileDTO != null)
            {
                List<GameProfileDTO> gameProfileNameExist = 
                            gameprofileDTOLists.Where(g => !string.IsNullOrWhiteSpace(gameProfileDTO.ProfileName.ToLower()) 
                            && g.ProfileName.ToLower() == gameProfileDTO.ProfileName.ToLower() 
                            && g.GameProfileId != gameProfileDTO.GameProfileId).ToList();
                if (gameProfileNameExist != null && gameProfileNameExist.Any())
                {
                    validationErrorList.Add(new ValidationError("Game Profile", "Profile Name", MessageContainerList.GetMessage(executionContext, 653))); //Please enter unique Game Profiles

                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


    }

    /// <summary>
    /// Manages the list of game profiles
    /// </summary>
    public class GameProfileSearchBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Takes GameProfileParams as parameter
        /// </summary>
        /// <returns>Returns List of KeyValuePair(GameProfileDTO.SearchByGameProfileParameters, string) by converting gameSearchParams</returns>
        public List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> BuildGameProfileSearchParameters(GameProfileParams gameSearchParams)
        {
            log.LogMethodEntry(gameSearchParams);
            List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> gSearchParams = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
            if (gSearchParams == null)
            {
                // Add mandatory columns here
            }
            else
            {
                if (gameSearchParams.GameProfileId != -1)
                    gSearchParams.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_ID, Convert.ToString(gameSearchParams.GameProfileId)));
                if (gameSearchParams.GameProfileName != null)
                    gSearchParams.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_NAME, gameSearchParams.GameProfileName));

            }
            log.LogMethodExit(gSearchParams);
            return gSearchParams;
        }
    }
}
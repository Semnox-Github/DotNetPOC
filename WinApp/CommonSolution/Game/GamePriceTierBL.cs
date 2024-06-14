/********************************************************************************************
 * Project Name - GamePriceTier BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        30-Sep-2021      Lakshminarayana     Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using System.Linq;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Business logic for GamePriceTier class.
    /// </summary>
    public class GamePriceTierBL
    {
        private GamePriceTierDTO gamePriceTierDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private GamePriceTierBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the gamePriceTier id as the parameter
        /// Would fetch the gamePriceTier object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public GamePriceTierBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            GamePriceTierDataHandler gamePriceTierDataHandler = new GamePriceTierDataHandler(sqlTransaction);
            gamePriceTierDTO = gamePriceTierDataHandler.GetGamePriceTierDTO(id);
            if (gamePriceTierDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Order Type Group Map ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates GamePriceTierBL object using the GamePriceTierDTO
        /// </summary>
        /// <param name="gamePriceTierDTO">GamePriceTierDTO object</param>
        public GamePriceTierBL(ExecutionContext executionContext, GamePriceTierDTO gamePriceTierDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, gamePriceTierDTO);            
            this.gamePriceTierDTO = gamePriceTierDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the GamePriceTier
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (gamePriceTierDTO.IsChanged == false
                && gamePriceTierDTO.GamePriceTierId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            GamePriceTierDataHandler gamePriceTierDataHandler = new GamePriceTierDataHandler(sqlTransaction);
            if (gamePriceTierDTO.IsActive)
            {
                List<ValidationError> validationErrors = Validate(sqlTransaction);
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
            }
            if (gamePriceTierDTO.GamePriceTierId < 0)
            {
                gamePriceTierDTO = gamePriceTierDataHandler.Insert(gamePriceTierDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                gamePriceTierDTO.AcceptChanges();
            }
            else
            {
                if (gamePriceTierDTO.IsChanged)
                {
                    gamePriceTierDTO = gamePriceTierDataHandler.Update(gamePriceTierDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    gamePriceTierDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the GamePriceTier
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (gamePriceTierDTO.GamePriceTierId <= -1)
            {
                log.LogMethodExit(null, "Nothing to delete.");
                return;
            }
            GamePriceTierDataHandler gamePriceTierDataHandler = new GamePriceTierDataHandler(sqlTransaction);
            gamePriceTierDataHandler.Delete(gamePriceTierDTO.GamePriceTierId);
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if(gamePriceTierDTO.PlayCredits < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Play Credits"));
                validationErrorList.Add(new ValidationError("GamePriceTier", "PlayCredits", errorMessage));
            }
            if (gamePriceTierDTO.VipPlayCredits < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "VIP Play Credits"));
                validationErrorList.Add(new ValidationError("GamePriceTier", "VipPlayCredits", errorMessage));
            }
            if (string.IsNullOrWhiteSpace(gamePriceTierDTO.Name))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Name"));
                validationErrorList.Add(new ValidationError("GamePriceTier", "Name", errorMessage));
            }
            else if (gamePriceTierDTO.Name.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Name"), 100);
                validationErrorList.Add(new ValidationError("GamePriceTier", "Name", errorMessage));
            }
            if (gamePriceTierDTO.PlayCount <= 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Play Count"));
                validationErrorList.Add(new ValidationError("GamePriceTier", "PlayCount", errorMessage));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public GamePriceTierDTO GamePriceTierDTO { get { return gamePriceTierDTO; } }      
    }

    /// <summary>
    /// Manages the list of GamePriceTier
    /// </summary>
    public class GamePriceTierListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<GamePriceTierDTO> gamePriceTierDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public GamePriceTierListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="gamePriceTierDTOList"></param>
        public GamePriceTierListBL(ExecutionContext executionContext, List<GamePriceTierDTO> gamePriceTierDTOList)
            : this()
        {
            log.LogMethodEntry(executionContext, gamePriceTierDTOList);
            this.executionContext = executionContext;
            this.gamePriceTierDTOList = gamePriceTierDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the GamePriceTier list
        /// </summary>
        public List<GamePriceTierDTO> GetGamePriceTierDTOList(List<KeyValuePair<GamePriceTierDTO.SearchByParameters, string>> searchParameters,
                                                                      SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            GamePriceTierDataHandler gamePriceTierDataHandler = new GamePriceTierDataHandler(sqlTransaction);
            List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierDataHandler.GetGamePriceTierDTOList(searchParameters);
            log.LogMethodExit(gamePriceTierDTOList);
            return gamePriceTierDTOList;
        }

        /// <summary>
        /// Returns the GamePriceTier list
        /// </summary>
        public List<GamePriceTierDTO> GetGamePriceTierDTOListOfGames(List<int> gameIdList, bool activeRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(gameIdList, activeRecords, sqlTransaction);
            GamePriceTierDataHandler gamePriceTierDataHandler = new GamePriceTierDataHandler(sqlTransaction);
            List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierDataHandler.GetGamePriceTierDTOListOfGames(gameIdList, activeRecords);
            log.LogMethodExit(gamePriceTierDTOList);
            return gamePriceTierDTOList;
        }

        /// <summary>
        /// Returns the GamePriceTier list
        /// </summary>
        public List<GamePriceTierDTO> GetGamePriceTierDTOListOfGameProfiles(List<int> gameProfileIdList, bool activeRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(gameProfileIdList, activeRecords, sqlTransaction);
            GamePriceTierDataHandler gamePriceTierDataHandler = new GamePriceTierDataHandler(sqlTransaction);
            List<GamePriceTierDTO> gamePriceTierDTOList = gamePriceTierDataHandler.GetGamePriceTierDTOListOfGameProfiles(gameProfileIdList, activeRecords);
            log.LogMethodExit(gamePriceTierDTOList);
            return gamePriceTierDTOList;
        }

        /// <summary>
        /// Save GamePriceTierDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if(gamePriceTierDTOList != null && gamePriceTierDTOList.Any())
            {
                foreach (GamePriceTierDTO gamePriceTierDTO in gamePriceTierDTOList)
                {
                    GamePriceTierBL gamePriceTierBL = new GamePriceTierBL(executionContext, gamePriceTierDTO);
                    gamePriceTierBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }
    }
}

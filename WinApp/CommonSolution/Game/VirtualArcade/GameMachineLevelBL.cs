/********************************************************************************************
 * Project Name - Game                                                                          
 * Description  - Business logic  class to manipulate game machine level details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
   *2.130.4     28-Feb-2022   Girish Kundar    Modified : Added two new columns AutoLoadEntitlement, EntitlementType
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game.VirtualArcade
{

    /// <summary>
    /// GameMachineLevelBL
    /// </summary>
    public class GameMachineLevelBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GameMachineLevelDTO gameMachineLevelDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private GameMachineLevelBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GameMachineLevelBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parameterGameMachineLevelDTO"></param>
        /// <param name="sqlTransaction"></param>
        public GameMachineLevelBL(ExecutionContext executionContext, GameMachineLevelDTO parameterGameMachineLevelDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterGameMachineLevelDTO, sqlTransaction);

            if (parameterGameMachineLevelDTO.GameMachineLevelId > -1)
            {
                LoadGameMachineLevelDTO(parameterGameMachineLevelDTO.GameMachineLevelId, sqlTransaction);//added sql
                ThrowIfUserDTOIsNull(parameterGameMachineLevelDTO.GameMachineLevelId);
                Update(parameterGameMachineLevelDTO);
            }
            else
            {
                Validate(sqlTransaction);
                gameMachineLevelDTO = new GameMachineLevelDTO(-1, parameterGameMachineLevelDTO.MachineId, parameterGameMachineLevelDTO.LevelName, parameterGameMachineLevelDTO.LevelCharacteristics,
                    parameterGameMachineLevelDTO.QualifyingScore, parameterGameMachineLevelDTO.ScoreToVPRatio, parameterGameMachineLevelDTO.ScoreToXPRatio, parameterGameMachineLevelDTO.TranslationFileName,
                    parameterGameMachineLevelDTO.ImageFileName, parameterGameMachineLevelDTO.IsActive, parameterGameMachineLevelDTO.AutoLoadEntitlement, parameterGameMachineLevelDTO.EntitlementType);
            }
            log.LogMethodExit();
        }
        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (gameMachineLevelDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "GameMachineLevel", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadGameMachineLevelDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            GameMachineLevelDataHandler gameMachineLevelDataHandler = new GameMachineLevelDataHandler(sqlTransaction);
            gameMachineLevelDTO = gameMachineLevelDataHandler.GetGameMachineLevelDTO(id);
            ThrowIfUserDTOIsNull(id);
            log.LogMethodExit();
        }

        private void Update(GameMachineLevelDTO parameterGameMachineLevelDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterGameMachineLevelDTO);
            gameMachineLevelDTO.MachineId = parameterGameMachineLevelDTO.MachineId;
            gameMachineLevelDTO.LevelName = parameterGameMachineLevelDTO.LevelName;
            gameMachineLevelDTO.LevelCharacteristics = parameterGameMachineLevelDTO.LevelCharacteristics;
            gameMachineLevelDTO.ScoreToVPRatio = parameterGameMachineLevelDTO.ScoreToVPRatio;
            gameMachineLevelDTO.QualifyingScore = parameterGameMachineLevelDTO.QualifyingScore;
            gameMachineLevelDTO.ScoreToXPRatio = parameterGameMachineLevelDTO.ScoreToXPRatio;
            gameMachineLevelDTO.IsActive = parameterGameMachineLevelDTO.IsActive;
            gameMachineLevelDTO.TranslationFileName = parameterGameMachineLevelDTO.TranslationFileName;
            gameMachineLevelDTO.ImageFileName = parameterGameMachineLevelDTO.ImageFileName;
            gameMachineLevelDTO.AutoLoadEntitlement = parameterGameMachineLevelDTO.AutoLoadEntitlement;
            gameMachineLevelDTO.EntitlementType = parameterGameMachineLevelDTO.EntitlementType;
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            // Validation code here 
            // return validation exceptions
            log.LogMethodExit();
        }

        /// <summary>
        /// GameMachineLevelBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public GameMachineLevelBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadGameMachineLevelDTO(id, sqlTransaction);
            log.LogMethodExit();
        }


        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SaveImpl(sqlTransaction);
            if (!string.IsNullOrEmpty(gameMachineLevelDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("GameMachineLevels", gameMachineLevelDTO.Guid, sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void SaveImpl(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            GameMachineLevelDataHandler gameMachineLevelDataHandler = new GameMachineLevelDataHandler(sqlTransaction);
            if (gameMachineLevelDTO.GameMachineLevelId < 0)
            {
                gameMachineLevelDTO = gameMachineLevelDataHandler.Insert(gameMachineLevelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                gameMachineLevelDTO.AcceptChanges();
            }
            else
            {
                if (gameMachineLevelDTO.IsChanged)
                {
                    gameMachineLevelDTO = gameMachineLevelDataHandler.Update(gameMachineLevelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    gameMachineLevelDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get GameMachineLevelDTO Object
        /// </summary>
        public GameMachineLevelDTO GameMachineLevelDTO
        {
            get
            {
                GameMachineLevelDTO result = new GameMachineLevelDTO(gameMachineLevelDTO);
                return result;
            }
        }

        public decimal GetScoreToVirtualPointRatio()
        {
            log.LogMethodEntry();
            decimal virtualLoyaltyPointsRatio = 0;
            try
            {
                if (gameMachineLevelDTO != null)
                {
                    virtualLoyaltyPointsRatio = gameMachineLevelDTO.ScoreToVPRatio.HasValue ? Convert.ToDecimal(gameMachineLevelDTO.ScoreToVPRatio) : 0;
                }
                if (virtualLoyaltyPointsRatio <= 0)
                {
                    string virtualRatio = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "VIRTUAL_SCORE_TO_VIRTUAL_POINTS_CONVERSION_RATIO");
                    if (string.IsNullOrWhiteSpace(virtualRatio))
                    {
                        virtualLoyaltyPointsRatio = 1;
                    }
                    else
                    {
                        virtualLoyaltyPointsRatio = Convert.ToDecimal(virtualRatio);
                    }
                    if (virtualLoyaltyPointsRatio == 0)
                    {
                        virtualLoyaltyPointsRatio = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 1;
            }
            log.LogMethodExit();
            return virtualLoyaltyPointsRatio;
        }
    }

    /// <summary>
    /// GameMachineLevelListBL
    /// </summary>
    public class GameMachineLevelListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<GameMachineLevelDTO> gameMachineLevelDTOList;

        /// <summary>
        /// default constructor
        /// </summary>
        public GameMachineLevelListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public GameMachineLevelListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.gameMachineLevelDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// GameMachineLevelListBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="gameMachineLevelDTOList"></param>
        public GameMachineLevelListBL(ExecutionContext executionContext, List<GameMachineLevelDTO> gameMachineLevelDTOList)
        {
            log.LogMethodEntry(executionContext, gameMachineLevelDTOList);
            this.gameMachineLevelDTOList = gameMachineLevelDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetGameMachineLevels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<GameMachineLevelDTO> GetGameMachineLevels(List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            GameMachineLevelDataHandler gameMachineLevelDataHandler = new GameMachineLevelDataHandler(sqlTransaction);
            List<GameMachineLevelDTO> gameMachineLevelDTOList = gameMachineLevelDataHandler.GetGameMachineLevels(searchParameters);
            log.LogMethodExit(gameMachineLevelDTOList);
            return gameMachineLevelDTOList;
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <returns></returns>

        public List<GameMachineLevelDTO> Save()
        {
            log.LogMethodEntry();
            List<GameMachineLevelDTO> savedGameMachineLevelDTOList = new List<GameMachineLevelDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (gameMachineLevelDTOList != null && gameMachineLevelDTOList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (GameMachineLevelDTO gameMachineLevelDTO in gameMachineLevelDTOList)
                        {
                            GameMachineLevelBL gameMachineLevelBL = new GameMachineLevelBL(executionContext, gameMachineLevelDTO);
                            gameMachineLevelBL.Save(parafaitDBTrx.SQLTrx);
                            savedGameMachineLevelDTOList.Add(gameMachineLevelBL.GameMachineLevelDTO);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    if (sqlEx.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }

            }
            log.LogMethodExit(savedGameMachineLevelDTOList);
            return savedGameMachineLevelDTOList;
        }

    }
}

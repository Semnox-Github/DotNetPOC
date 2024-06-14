/********************************************************************************************
 * Project Name - Game
 * Description  - LocalMachineInputUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 *2.110.0     08-Feb-2021      Fiona                      Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game.VirtualArcade
{
    /// <summary>
    /// LocalGameMachineLevelUseCases
    /// </summary>
    public class LocalGameMachineLevelUseCases : IGameMachineLevelUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// LocalGameMachineLevelUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalGameMachineLevelUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetGameMachineLevels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<GameMachineLevelDTO>> GetGameMachineLevels(List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<GameMachineLevelDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                GameMachineLevelListBL GameMachineLevelListBL = new GameMachineLevelListBL(executionContext);
                List<GameMachineLevelDTO> GameMachineLevel = GameMachineLevelListBL.GetGameMachineLevels(searchParameters, sqlTransaction);

                log.LogMethodExit(GameMachineLevel);
                return GameMachineLevel;


            });
        }

        /// <summary>
        /// SaveGameMachineLevels
        /// </summary>
        /// <param name="gameMachineLevelDTOList"></param>
        /// <returns></returns>
        public async Task<List<GameMachineLevelDTO>> SaveGameMachineLevels(List<GameMachineLevelDTO> gameMachineLevelDTOList)
        {
            return await Task<List<GameMachineLevelDTO>>.Factory.StartNew(() =>
            {
                string message = string.Empty;
                List<GameMachineLevelDTO> result = null;
                try
                {
                    log.LogMethodEntry(gameMachineLevelDTOList);
                    
                    if (gameMachineLevelDTOList == null)
                    {
                        throw new ValidationException("GameMachineLevel is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {

                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            GameMachineLevelListBL gameMachineLevelList = new GameMachineLevelListBL(executionContext, gameMachineLevelDTOList);
                            result= gameMachineLevelList.Save();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }

                    }
                    message = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    message = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

/********************************************************************************************
 * Project Name - Game
 * Description  - LocalCustomerGamePlayLevelResultUseCases class 
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

namespace Semnox.Parafait.Transaction.VirtualArcade
{
    /// <summary>
    /// LocalCustomerGamePlayLevelResultUseCases
    /// </summary>
    public class LocalCustomerGamePlayLevelResultUseCases : ICustomerGamePlayLevelResultUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// LocalCustomerGamePlayLevelResultUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalCustomerGamePlayLevelResultUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// GetCustomerGamePlayLevelResults
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<CustomerGamePlayLevelResultDTO>> GetCustomerGamePlayLevelResults(List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<CustomerGamePlayLevelResultDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                CustomerGamePlayLevelResultListBL customerGamePlayLevelResultListBL = new CustomerGamePlayLevelResultListBL(executionContext);
                List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList = customerGamePlayLevelResultListBL.GetCustomerGamePlayLevelResults(searchParameters, sqlTransaction);

                log.LogMethodExit(customerGamePlayLevelResultDTOList);
                return customerGamePlayLevelResultDTOList;


            });
        }

        /// <summary>
        /// SaveCustomerGamePlayLevelResults
        /// </summary>
        /// <param name="customerGamePlayLevelResultDTOList"></param>
        /// <returns></returns>
        public async Task<List<GamePlayWinningsDTO>> SaveCustomerGamePlayLevelResults(List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList)
        {
            return await Task<List<GamePlayWinningsDTO>>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                List<GamePlayWinningsDTO> gamePlayWinningsDTOList = null;
                try
                {
                    log.LogMethodEntry(customerGamePlayLevelResultDTOList);
                    if (customerGamePlayLevelResultDTOList == null)
                    {
                        throw new ValidationException("CustomerGamePlayLevelResultDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {

                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerGamePlayLevelResultListBL customerGamePlayLevelResultList = new CustomerGamePlayLevelResultListBL(executionContext, customerGamePlayLevelResultDTOList);
                            gamePlayWinningsDTOList = customerGamePlayLevelResultList.Save(parafaitDBTrx.SQLTrx);
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
                            throw ex;
                        }
                    }

                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(gamePlayWinningsDTOList);
                return gamePlayWinningsDTOList;
            });
        }

        /// <summary>
        /// GetCustomerGamePlayWinnings
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<GamePlayWinningsDTO>> GetCustomerGamePlayWinnings(int customerId)
        {
            return await Task<List<GamePlayWinningsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(customerId);

                CustomerGamePlayLevelResultListBL customerGamePlayLevelResultListBL = new CustomerGamePlayLevelResultListBL(executionContext);
                List<GamePlayWinningsDTO> gamePlayWinningDTOList = customerGamePlayLevelResultListBL.GetCustomerGamePlayWinnings(customerId);

                log.LogMethodExit(gamePlayWinningDTOList);
                return gamePlayWinningDTOList;


            });
        }
        /// <summary>
        /// GetLeaderBoard
        /// </summary>
        /// <param name="gameMachineLevelId"></param>
        /// <returns></returns>
        public async Task<List<LeaderBoardDTO>> GetLeaderBoard(int gameMachineLevelId)
        {
            return await Task<List<LeaderBoardDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(gameMachineLevelId);

                CustomerGamePlayLevelResultListBL customerGamePlayLevelResultListBL = new CustomerGamePlayLevelResultListBL(executionContext);
                List<LeaderBoardDTO> leaderBoardDTOList = customerGamePlayLevelResultListBL.GetLeaderBoard(gameMachineLevelId);

                log.LogMethodExit(leaderBoardDTOList);
                return leaderBoardDTOList;


            });
        }

    }
}

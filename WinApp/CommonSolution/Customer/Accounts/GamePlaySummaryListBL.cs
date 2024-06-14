using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Manages the list of CardGameMetricView
    /// </summary>
    public class GamePlaySummaryListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AccountActivityHelper accountActivityHelper;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountActivityHelper">execution context</param>
        public GamePlaySummaryListBL(ExecutionContext executionContext, AccountActivityHelper accountActivityHelper = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.accountActivityHelper = accountActivityHelper;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CardGameMetricView list
        /// </summary>
        public List<GamePlayDTO> GetGamePlayDTOList(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, bool addSummaryRow = true, bool detailed = true, 
            SqlTransaction sqlTransaction = null, int numberOfRecords = -1, int pageNumber = 0)
        {
            log.LogMethodEntry(searchParameters);
            GamePlaySummaryDataHandler gamePlaySummaryDataHandler = new GamePlaySummaryDataHandler(sqlTransaction);
            List<GamePlayDTO> gamePlayDTOList = gamePlaySummaryDataHandler.GetGamePlayDTOList(searchParameters, detailed, numberOfRecords, pageNumber);
            if (gamePlayDTOList != null && gamePlayDTOList.Count > 0 && addSummaryRow)
            {
                GamePlayDTO summaryRow = GetSummaryGamePlayDTO(gamePlayDTOList);
                gamePlayDTOList.Insert(0, summaryRow);
            }
            log.LogMethodExit(gamePlayDTOList);
            return gamePlayDTOList;
        }
        /// <summary>
        /// Returns the CardGameMetricView list
        /// </summary>
        public List<GamePlayDTO> GetGamePlayDTOListByAccountIds(List<int> accountIdList, List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, bool addSummaryRow = true, bool detailed = true,
            SqlTransaction sqlTransaction = null, int numberOfRecords = -1, int pageNumber = 0)
        {
            log.LogMethodEntry(accountIdList, searchParameters);
            GamePlaySummaryDataHandler gamePlaySummaryDataHandler = new GamePlaySummaryDataHandler(sqlTransaction);
            List<GamePlayDTO> gamePlayDTOList = gamePlaySummaryDataHandler.GetGamePlayDTOListByAccountIdList(accountIdList, searchParameters, detailed, numberOfRecords, pageNumber);
            if (gamePlayDTOList != null && gamePlayDTOList.Count > 0 && addSummaryRow)
            {
                GamePlayDTO summaryRow = GetSummaryGamePlayDTO(gamePlayDTOList);
                gamePlayDTOList.Insert(0, summaryRow);
            }
            log.LogMethodExit(gamePlayDTOList);
            return gamePlayDTOList;
        }
        /// <summary>
        /// Returns the Consolidated GamePlayDTOList 
        /// </summary>
        /// <returns></returns>
        public List<GamePlayDTO> GetConsolidatedGamePlayDTOList(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, bool addSummaryRow = true, bool detailed = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<GamePlayDTO> consolidatedGamePlayDTOList = new List<GamePlayDTO>();
            List<GamePlayDTO> localGamePlayDTOList = GetGamePlayDTOList(searchParameters,false,detailed, sqlTransaction);
            List<GamePlayDTO> serverGamePlayDTOList = accountActivityHelper.GetServerGamePlayDTOList(detailed);
            if (localGamePlayDTOList != null && localGamePlayDTOList.Count > 0)
            {
                consolidatedGamePlayDTOList.AddRange(localGamePlayDTOList);
            }
            if (serverGamePlayDTOList != null && serverGamePlayDTOList.Count > 0)
            {
                foreach (var serverGamePlayDTO in serverGamePlayDTOList)
                {
                    if (consolidatedGamePlayDTOList.FirstOrDefault(x => x.PlayDate == serverGamePlayDTO.PlayDate && x.Game == serverGamePlayDTO.Game) == null)
                    {
                        consolidatedGamePlayDTOList.Add(serverGamePlayDTO);
                    }
                    else
                    {
                        GamePlayDTO gamePlayDTO = consolidatedGamePlayDTOList.FirstOrDefault(x => x.PlayDate == serverGamePlayDTO.PlayDate && x.Game == serverGamePlayDTO.Game);
                        if (gamePlayDTO != null)
                        {
                            gamePlayDTO.Site = serverGamePlayDTO.Site;
                        }
                    }
                }
            }
            if (consolidatedGamePlayDTOList != null && consolidatedGamePlayDTOList.Count > 0)
            {
                consolidatedGamePlayDTOList = consolidatedGamePlayDTOList.OrderByDescending(x => x.PlayDate).ToList();
                if(addSummaryRow)
                {
                    GamePlayDTO summaryGamePlayDTO = GetSummaryGamePlayDTO(consolidatedGamePlayDTOList);
                    consolidatedGamePlayDTOList.Insert(0, summaryGamePlayDTO);
                }
            }
            log.LogMethodExit(consolidatedGamePlayDTOList);
            return consolidatedGamePlayDTOList;
        }

        private GamePlayDTO GetSummaryGamePlayDTO(List<GamePlayDTO> gamePlayDTOList)
        {
            log.LogMethodEntry(gamePlayDTOList);
            GamePlayDTO summaryRow = new GamePlayDTO();
            summaryRow.Game = MessageContainerList.GetMessage(executionContext, "Grand Total");
            summaryRow.PlayDate = DateTime.MinValue;
            summaryRow.Credits = 0;
            summaryRow.CPCardBalance = 0;
            summaryRow.CPCredits = 0;
            summaryRow.CardGame = 0;
            summaryRow.Courtesy = 0;
            summaryRow.Bonus = 0;
            summaryRow.CPBonus = 0;
            summaryRow.Time = 0;
            summaryRow.TicketCount = 0;
            summaryRow.ETickets = 0;
            summaryRow.ManualTickets = 0;
            summaryRow.TicketEaterTickets = 0;

            foreach (var gamePlayDTO in gamePlayDTOList)
            {
                summaryRow.Credits = summaryRow.Credits + gamePlayDTO.Credits;
                summaryRow.CPCardBalance = summaryRow.CPCardBalance + gamePlayDTO.CPCardBalance;
                summaryRow.CPCredits = summaryRow.CPCredits + gamePlayDTO.CPCredits;
                summaryRow.CardGame = summaryRow.CardGame + gamePlayDTO.CardGame;
                summaryRow.Courtesy = summaryRow.Courtesy + gamePlayDTO.Courtesy;
                summaryRow.Bonus = summaryRow.Bonus + gamePlayDTO.Bonus;
                summaryRow.CPBonus = summaryRow.CPBonus + gamePlayDTO.CPBonus;
                summaryRow.Time = summaryRow.Time + gamePlayDTO.Time;
                summaryRow.TicketCount = summaryRow.TicketCount + gamePlayDTO.TicketCount;
                summaryRow.ETickets = summaryRow.ETickets + gamePlayDTO.ETickets;
                summaryRow.ManualTickets = summaryRow.ManualTickets + gamePlayDTO.ManualTickets;
                summaryRow.TicketEaterTickets = summaryRow.TicketEaterTickets + gamePlayDTO.TicketEaterTickets;
            }
            log.LogMethodExit(summaryRow);
            return summaryRow;
        }
    }
}

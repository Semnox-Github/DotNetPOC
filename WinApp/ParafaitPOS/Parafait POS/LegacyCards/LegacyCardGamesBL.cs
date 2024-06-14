/*/********************************************************************************************
 * Project Name - LegacyCardGamesBL
 * Description  - BL class for LegacyCardGamesBL
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    public class LegacyCardGamesBL
    {
        LegacyCardGamesDTO legacyCardGamesDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LegacyCardGamesBL class
        /// </summary>
        private LegacyCardGamesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the accountGame id as the parameter
        /// Would fetch the accountGame object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public LegacyCardGamesBL(ExecutionContext executionContext, int id,
            bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            LegacyCardGamesDataHandler legacyCardGamesDataHandler = new LegacyCardGamesDataHandler(sqlTransaction);
            legacyCardGamesDTO = legacyCardGamesDataHandler.GetLegacyCardGamesDTO(id);
            if (legacyCardGamesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountGame", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (legacyCardGamesDTO != null && loadChildRecords)
            {
                LegacyCardGameBuilderBL legacyCardGameBuilderBL = new LegacyCardGameBuilderBL(executionContext);
                legacyCardGameBuilderBL.Build(legacyCardGamesDTO, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LegacyCardGamesBL object using the AccountGameDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountGameDTO">AccountGameDTO object</param>
        public LegacyCardGamesBL(ExecutionContext executionContext, LegacyCardGamesDTO legacyCardGamesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, legacyCardGamesDTO);
            this.legacyCardGamesDTO = legacyCardGamesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AccountGame
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            LegacyCardGamesDataHandler legacyCardGamesDataHandler = new LegacyCardGamesDataHandler(sqlTransaction);
            if (legacyCardGamesDTO.IsChanged)
            {
                if (legacyCardGamesDTO.IsActive)
                {
                    if (legacyCardGamesDTO.LegacyCardGameId < 0)
                    {
                        legacyCardGamesDTO = legacyCardGamesDataHandler.InsertLegacyCardGame(legacyCardGamesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        legacyCardGamesDTO.AcceptChanges();
                    }
                    else
                    {
                        if (legacyCardGamesDTO.IsChanged)
                        {
                            legacyCardGamesDTO = legacyCardGamesDataHandler.UpdateLegacyCardGame(legacyCardGamesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            legacyCardGamesDTO.AcceptChanges();
                        }
                    }
                    if (legacyCardGamesDTO.LegacyCardGameExtendedDTOList != null)
                    {
                        foreach (var legacyCardGameExtendedDTO in legacyCardGamesDTO.LegacyCardGameExtendedDTOList)
                        {
                            if (legacyCardGameExtendedDTO.IsChanged || legacyCardGameExtendedDTO.LegacyCardGameExtendedId == -1)
                            {
                                if (legacyCardGameExtendedDTO.LegacyCardGameId != legacyCardGamesDTO.LegacyCardGameId)
                                {
                                    legacyCardGameExtendedDTO.LegacyCardGameId = legacyCardGamesDTO.LegacyCardGameId;
                                }
                                LegacyCardGameExtendedBL legacyCardGameExtendedBL = new LegacyCardGameExtendedBL(executionContext, legacyCardGameExtendedDTO);
                                legacyCardGameExtendedBL.Save(sqlTransaction);
                            }
                        }
                    }

                    log.LogMethodExit();
                }
            }
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LegacyCardGamesDTO LegacyCardGamesDTO
        {
            get
            {
                return legacyCardGamesDTO;
            }
        }

        //public int GetGameBalance()
        //{
        //    log.LogMethodEntry(legacyCardGamesDTO);
        //    int result = 0;
        //    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
        //    DateTime serverDate = lookupValuesList.GetServerDateTime();
        //    if (legacyCardGamesDTO.LastPlayedTime.HasValue == false)
        //    {
        //        result = Convert.ToInt32(legacyCardGamesDTO.BalanceGames);
        //        log.LogMethodExit(result);
        //        return result;
        //    }
        //    switch (legacyCardGamesDTO.Frequency)
        //    {
        //        case "N":
        //            {
        //                result = legacyCardGamesDTO.BalanceGames;
        //                break;
        //            }
        //        case "D":
        //            {
        //                if (GetUniqueYearDay(legacyCardGamesDTO.LastPlayedTime.Value) == GetUniqueYearDay(serverDate))
        //                {
        //                    result = legacyCardGamesDTO.BalanceGames;
        //                }
        //                else
        //                {
        //                    result = Convert.ToInt32(legacyCardGamesDTO.Quantity);
        //                }
        //                break;
        //            }
        //        case "W":
        //            {
        //                if (GetUniqueYearWeek(legacyCardGamesDTO.LastPlayedTime.Value) == GetUniqueYearWeek(serverDate))
        //                {
        //                    result = legacyCardGamesDTO.BalanceGames;
        //                }
        //                else
        //                {
        //                    result = Convert.ToInt32(legacyCardGamesDTO.Quantity);
        //                }
        //                break;
        //            }
        //        case "M":
        //            {
        //                if (GetUniqueYearMonth(legacyCardGamesDTO.LastPlayedTime.Value) == GetUniqueYearMonth(serverDate))
        //                {
        //                    result = legacyCardGamesDTO.BalanceGames;
        //                }
        //                else
        //                {
        //                    result = Convert.ToInt32(legacyCardGamesDTO.Quantity);
        //                }
        //                break;
        //            }
        //        case "Y":
        //            {
        //                if (legacyCardGamesDTO.LastPlayedTime.Value.Year == serverDate.Year)
        //                {
        //                    result = legacyCardGamesDTO.BalanceGames;
        //                }
        //                else
        //                {
        //                    result = Convert.ToInt32(legacyCardGamesDTO.Quantity);
        //                }
        //                break;
        //            }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}

        private int GetUniqueYearDay(DateTime value)
        {
            return value.Year * 1000 + value.DayOfYear;
        }

        private int GetUniqueYearWeek(DateTime value)
        {
            return value.Year * 1000 + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private int GetUniqueYearMonth(DateTime value)
        {
            return value.Year * 1000 + value.Month;
        }
    }

    /// <summary>
    /// Manages the list of AccountGame
    /// </summary>
    public class LegacyCardGameListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public LegacyCardGameListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountGame list
        /// </summary>
        public List<LegacyCardGamesDTO> GetLegacyCardGamesDTOList(List<KeyValuePair<LegacyCardGamesDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            LegacyCardGamesDataHandler legacyCardGamesDataHandler = new LegacyCardGamesDataHandler(sqlTransaction);
            List<LegacyCardGamesDTO> legacyCardGameDTOList = legacyCardGamesDataHandler.GetLegacyCardGamesDTOList(searchParameters);
            if (loadChildRecords)
            {
                if (legacyCardGameDTOList != null && legacyCardGameDTOList.Count > 0)
                {
                    LegacyCardGameBuilderBL legacyCardGameBuilderBL = new LegacyCardGameBuilderBL(executionContext);
                    legacyCardGameBuilderBL.Build(legacyCardGameDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(legacyCardGameDTOList);
            return legacyCardGameDTOList;
        }
    }

    /// <summary>
    /// Builds the complex AccountGame entity structure
    /// </summary>
    public class LegacyCardGameBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public LegacyCardGameBuilderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex accountGame DTO structure
        /// </summary>
        /// <param name="legacyCardGamesDTO">AccountGame dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(LegacyCardGamesDTO legacyCardGamesDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(legacyCardGamesDTO, activeChildRecords);
            if (legacyCardGamesDTO != null && legacyCardGamesDTO.LegacyCardGameId != -1)
            {
                AccountGameExtendedListBL accountGameExtendedListBL = new AccountGameExtendedListBL(executionContext);
                List<KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string>> accountGameExtendedSearchParams = new List<KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string>>();
                accountGameExtendedSearchParams.Add(new KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string>(LegacyCardGameExtendedDTO.SearchByParameters.LEGACY_CARD_GAME_ID, legacyCardGamesDTO.LegacyCardGameId.ToString()));
                legacyCardGamesDTO.LegacyCardGameExtendedDTOList = accountGameExtendedListBL.GetLegacyCardGameExtendedDTOList(accountGameExtendedSearchParams, sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex accountGameDTO structure
        /// </summary>
        /// <param name="legacyCardGameList">AccountGame dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(List<LegacyCardGamesDTO>  legacyCardGameList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(legacyCardGameList, activeChildRecords, sqlTransaction);
            if (legacyCardGameList != null && legacyCardGameList.Count > 0)
            {
                Dictionary<int, LegacyCardGamesDTO> legacyCardGameIdAccountGameDictionary = new Dictionary<int, LegacyCardGamesDTO>();
                Dictionary<string, LegacyCardGamesDTO> legacyCardGameGuidAccountGameDictionary = new Dictionary<string, LegacyCardGamesDTO>();
                HashSet<int> cardIdSet = new HashSet<int>();
                string cardIdList;
                for (int i = 0; i < legacyCardGameList.Count; i++)
                {
                    if (legacyCardGameList[i].LegacyCardGameId != -1 &&
                        legacyCardGameList[i].LegacyCard_id != -1)
                    {
                        cardIdSet.Add(legacyCardGameList[i].LegacyCard_id);
                        legacyCardGameIdAccountGameDictionary.Add(legacyCardGameList[i].LegacyCardGameId, legacyCardGameList[i]);
                    }
                }
                cardIdList = string.Join<int>(",", cardIdSet);
                AccountGameExtendedListBL legacyCardGameExtendedListBL = new AccountGameExtendedListBL(executionContext);
                List<KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string>> accountGameExtendedSearchParams = new List<KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string>>();
                accountGameExtendedSearchParams.Add(new KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string>(LegacyCardGameExtendedDTO.SearchByParameters.Card_ID_LIST, cardIdList));
                List<LegacyCardGameExtendedDTO> legacyCardGameExtendedDTOList = legacyCardGameExtendedListBL.GetLegacyCardGameExtendedDTOList(accountGameExtendedSearchParams, sqlTransaction);
                if (legacyCardGameExtendedDTOList != null && legacyCardGameExtendedDTOList.Count > 0)
                {
                    foreach (var legacyCardGameExtendedDTO in legacyCardGameExtendedDTOList)
                    {
                        if (legacyCardGameIdAccountGameDictionary.ContainsKey(legacyCardGameExtendedDTO.LegacyCardGameId))
                        {
                            if (legacyCardGameIdAccountGameDictionary[legacyCardGameExtendedDTO.LegacyCardGameId].LegacyCardGameExtendedDTOList == null)
                            {
                                legacyCardGameIdAccountGameDictionary[legacyCardGameExtendedDTO.LegacyCardGameId].LegacyCardGameExtendedDTOList = new List<LegacyCardGameExtendedDTO>();
                            }
                            legacyCardGameIdAccountGameDictionary[legacyCardGameExtendedDTO.LegacyCardGameId].LegacyCardGameExtendedDTOList.Add(legacyCardGameExtendedDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}


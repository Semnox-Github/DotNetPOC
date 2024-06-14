/********************************************************************************************
 * Project Name - Game Play Summary Data Handler
 * Description  - Data handler of the Contact class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.60        08-May-2019   Nitin Pai           Added date parameter for Guest App
 *2.70.2      19-Jul-2019   Girish Kundar       Modified :Fix for SQL Injection Issue  
 *2.70.2      15-Oct-2019   Nitin Pai           Gateway Clean up - adding methods for rest api call
 *2.70.2      03-Mar-2020   Jeevan              modifed search parameters to match from and to date definitions
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountGameMetricView Data Handler - Handles insert, update and select of  AccountGameMetricView objects
    /// </summary>
    public class GamePlaySummaryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<GamePlayDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<GamePlayDTO.SearchByParameters, string>
            {
                {GamePlayDTO.SearchByParameters.CARD_ID, "card_id"},
                {GamePlayDTO.SearchByParameters.CARD_ID_LIST, "card_id"},
                {GamePlayDTO.SearchByParameters.FROM_DATE, "Date"},
                {GamePlayDTO.SearchByParameters.TO_DATE, "Date"},
                {GamePlayDTO.SearchByParameters.LAST_GAMEPLAY_ID_OF_SET, "GamePlay_Id"},
            };
        private readonly DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of AccountGameMetricViewDataHandler class
        /// </summary>
        public GamePlaySummaryDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to GamePlayDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <param name="detailed">contains detailed data</param>
        /// <returns>Returns GamePlayDTO</returns>
        public GamePlayDTO GetGamePlayDTO(DataRow dataRow, bool detailed)
        {
            log.LogMethodEntry(dataRow);
            GamePlayDTO gamePlayDTO = new GamePlayDTO(Convert.ToInt32(dataRow["card_id"]),
                                            Convert.ToInt32(dataRow["gameplay_id"]),
                                            dataRow["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Date"]),
                                            dataRow["Game"] == DBNull.Value ? "" : Convert.ToString(dataRow["Game"]),
                                            dataRow["Machine"] == DBNull.Value ? "" : Convert.ToString(dataRow["Machine"]),
                                            dataRow["Credits"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Credits"]),
                                            detailed == false || dataRow["CPCardBalance"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["CPCardBalance"]),
                                            detailed == false || dataRow["CPCredits"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["CPCredits"]),
                                            detailed == false || dataRow["CardGame"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["CardGame"]),
                                            dataRow["Courtesy"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Courtesy"]),
                                            dataRow["Bonus"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Bonus"]),
                                            detailed == false || dataRow["CPBonus"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["CPBonus"]),
                                            dataRow["Time"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Time"]),
                                            dataRow["Tickets"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Tickets"]),
                                            dataRow["e-Tickets"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["e-Tickets"]),
                                            dataRow["Manual Tickets"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Manual Tickets"]),
                                            dataRow["T.Eater Tickets"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["T.Eater Tickets"]),
                                            dataRow["Mode"] == DBNull.Value ? "" : Convert.ToString(dataRow["Mode"]),
                                            dataRow["Site"] == DBNull.Value ? "" : Convert.ToString(dataRow["Site"]),
                                            dataRow["task_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["task_id"])
                                            );
            log.LogMethodExit(gamePlayDTO);
            return gamePlayDTO;
        }

        /// <summary>
        /// Gets the GamePlayDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="detailed">Whether to show detailed data</param>
        /// <returns>Returns the list of GamePlayDTO matching the search criteria</returns>
        public List<GamePlayDTO> GetGamePlayDTOList(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, bool detailed = false, int numberOfRecords = -1, int pageNumber = 0)
        {
            log.LogMethodEntry(searchParameters);
            List<GamePlayDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"SELECT * FROM " + (detailed? " GameMetricViewExtendedForDisplay " : " GameMetricViewForDisplay ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");

                String offsetQuery = "";
                if (numberOfRecords > -1 && (pageNumber * numberOfRecords) >= 0)
                {
                    offsetQuery = " OFFSET " + pageNumber * numberOfRecords + " ROWS FETCH NEXT " + numberOfRecords.ToString() + " ROWS ONLY";
                }

                foreach (KeyValuePair<GamePlayDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == GamePlayDTO.SearchByParameters.CARD_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GamePlayDTO.SearchByParameters.CARD_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == GamePlayDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == GamePlayDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == GamePlayDTO.SearchByParameters.LAST_GAMEPLAY_ID_OF_SET)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query + " order by date desc " + offsetQuery.ToString();
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<GamePlayDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GamePlayDTO gamePlayDTO = GetGamePlayDTO(dataRow, detailed);
                    list.Add(gamePlayDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the GamePlayDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="detailed">Whether to show detailed data</param>
        /// <returns>Returns the list of GamePlayDTO matching the search criteria</returns>
        public List<GamePlayDTO> GetGamePlayDTOListByAccountIdList(List<int> accountIdList, 
            List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, bool detailed = false, int numberOfRecords = -1, int pageNumber = 0)
        {
            log.LogMethodEntry(accountIdList, searchParameters, detailed, numberOfRecords, pageNumber);
            List<GamePlayDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"SELECT * FROM " + (detailed ? " GameMetricViewExtendedForDisplay " : " GameMetricViewForDisplay ");
            if (accountIdList != null && accountIdList.Any())
            {
                selectQuery = selectQuery + ", @accountIdList List WHERE Card_Id = List.Id ";
            }
            else
            {
                log.LogMethodExit(null, "throwing exception");
                log.LogVariableState("accountIdList", accountIdList);
                throw new Exception("The input parameter (accountIdList) is not provided");
            }
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" ");

                String offsetQuery = "";
                if (numberOfRecords > -1 && (pageNumber * numberOfRecords) >= 0)
                {
                    offsetQuery = " OFFSET " + pageNumber * numberOfRecords + " ROWS FETCH NEXT " + numberOfRecords.ToString() + " ROWS ONLY";
                }

                foreach (KeyValuePair<GamePlayDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == GamePlayDTO.SearchByParameters.CARD_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GamePlayDTO.SearchByParameters.CARD_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == GamePlayDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == GamePlayDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == GamePlayDTO.SearchByParameters.LAST_GAMEPLAY_ID_OF_SET)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query + " order by date desc " + offsetQuery.ToString();
            }
            //DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            DataTable table = dataAccessHandler.BatchSelect(selectQuery, "@accountIdList", accountIdList, parameters.ToArray(), sqlTransaction); 
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetGamePlayDTO(x, detailed)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

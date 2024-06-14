/********************************************************************************************
 * Project Name - DBSynch
 * Description  - Data handler of the DBSynchLogSummarySummary class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        29-Sep-2023   Lakshminarayana   Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DBSynch
{
    /// <summary>
    ///  DBSynchLogSummary Data Handler - Handles insert, update and select of  DBSynchLogSummary objects
    /// </summary>
    public class DBSynchLogSummaryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT TableName, site_id, COUNT(TableName) Count FROM DBSynchLog AS dbl ";
        private static readonly Dictionary<DBSynchLogSummaryDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DBSynchLogSummaryDTO.SearchByParameters, string>
            {
                {DBSynchLogSummaryDTO.SearchByParameters.TABLE_NAME, "dbl.TableName"},
                {DBSynchLogSummaryDTO.SearchByParameters.IS_PROCESSED, "ISNULL(dbl.IsProcessed, 'N')"},
                {DBSynchLogSummaryDTO.SearchByParameters.SITE_ID, "dbl.site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of DBSynchLogSummaryDataHandler class
        /// </summary>
        public DBSynchLogSummaryDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to DBSynchLogSummaryDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DBSynchLogSummaryDTO</returns>
        private DBSynchLogSummaryDTO GetDBSynchLogSummaryDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DBSynchLogSummaryDTO dBSynchLogSummaryDTO = new DBSynchLogSummaryDTO(Convert.ToString(dataRow["TableName"]),
                                                                                 dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                                 dataRow["Count"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Count"]));
            log.LogMethodExit(dBSynchLogSummaryDTO);
            return dBSynchLogSummaryDTO;
        }

        /// <summary>
        /// Gets the DBSynchLogSummaryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DBSynchLogSummaryDTO matching the search criteria</returns>
        public List<DBSynchLogSummaryDTO> GetDBSynchLogSummaryDTOList(List<KeyValuePair<DBSynchLogSummaryDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DBSynchLogSummaryDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
              
                StringBuilder query = new StringBuilder("  WHERE ");
                foreach (KeyValuePair<DBSynchLogSummaryDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DBSynchLogSummaryDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DBSynchLogSummaryDTO.SearchByParameters.IS_PROCESSED)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                selectQuery = selectQuery + query + " GROUP BY TableName, site_id ";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray());
            if (dataTable.Rows.Count > 0)
            {
                list = new List<DBSynchLogSummaryDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DBSynchLogSummaryDTO dBSynchLogSummaryDTO = GetDBSynchLogSummaryDTO(dataRow);
                    list.Add(dBSynchLogSummaryDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
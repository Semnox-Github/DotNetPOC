/********************************************************************************************
 * Project Name - Site
 * Description  - Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1     26-Apr-2021   Deeksha               Created as part of AWS Job Scheduler enhancements
 ********************************************************************************************/
using System;
using System.Data;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Site
{
    public class CentralCompanyDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Company AS cp ";

        private static readonly Dictionary<CentralCompanyDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CentralCompanyDTO.SearchByParameters, string>
            {
                {CentralCompanyDTO.SearchByParameters.CENTRAL_COMPANY_ID, "cp.company_id"},
                {CentralCompanyDTO.SearchByParameters.CENTRAL_COMPANY_NAME, "cp.company_name"},
                {CentralCompanyDTO.SearchByParameters.IS_ACTIVE, "cp.active"},
            };

        public CentralCompanyDataHandler(string connectionString)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler(connectionString);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor for CentralCompanyDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public CentralCompanyDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Get SQL Parameters
        /// </summary>
        /// <param name="CentralCompanyDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(CentralCompanyDTO centralCompanyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(centralCompanyDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CentralCompanyId", centralCompanyDTO.CentralCompanyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CentralCompanyName", centralCompanyDTO.CentralCompanyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoginKey", centralCompanyDTO.LoginKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Version", centralCompanyDTO.Version));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DBName", centralCompanyDTO.DBName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeZone", centralCompanyDTO.TimeZone));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Active", centralCompanyDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Offset", centralCompanyDTO.Offset));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BuisnessDayStartTime", centralCompanyDTO.BuisnessDayStartTime));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// GetCentralCompanyDTO
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private CentralCompanyDTO GetCentralCompanyDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CentralCompanyDTO CentralCompanyDTO = new CentralCompanyDTO(
                dataRow["company_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["company_id"]),
                dataRow["company_name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["company_name"]),
                dataRow["login_key"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["login_key"]),
                dataRow["Version"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Version"]),
                dataRow["DbName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DbName"]),
                dataRow["Timezone"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Timezone"]),
                dataRow["offset"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["offset"]),
                dataRow["businessDayStartTime"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["businessDayStartTime"]),
                dataRow["active"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["active"]),
                dataRow["ApplicationFolderPath"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ApplicationFolderPath"])
                );
            log.LogMethodExit(CentralCompanyDTO);
            return CentralCompanyDTO;
        }

        internal CentralCompanyDTO GetCentralCompanyDTO(int companyId)
        {
            log.LogMethodEntry(companyId);
            CentralCompanyDTO result = null;
            string query = SELECT_QUERY + @" WHERE cp.company_id = @CentralCompanyId";
            SqlParameter parameter = new SqlParameter("@CentralCompanyId", companyId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCentralCompanyDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetDbList
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        internal List<CentralCompanyDTO> GetDbList()
        {
            log.LogMethodEntry();
            DataTable siteList = new DataTable();
            List<CentralCompanyDTO> centralCompanyDTOList = null;
            string selectSitesQuery = @"select * from Company where active='Y'  ";
            DataTable dtSites = dataAccessHandler.executeSelectQuery(selectSitesQuery, null);
            if (dtSites.Rows.Count > 0)
            {
                centralCompanyDTOList = dtSites.Rows.Cast<DataRow>().Select(x => GetCentralCompanyDTO(x)).ToList();
            }
            return centralCompanyDTOList;
        }


        internal List<CentralCompanyDTO> GetCentralCompanyDTOList(List<KeyValuePair<CentralCompanyDTO.SearchByParameters, string>> searchParameters,
                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CentralCompanyDTO> centralCompanyDTOList = new List<CentralCompanyDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CentralCompanyDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CentralCompanyDTO.SearchByParameters.CENTRAL_COMPANY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CentralCompanyDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                centralCompanyDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCentralCompanyDTO(x)).ToList();
            }
            log.LogMethodExit(centralCompanyDTOList);
            return centralCompanyDTOList;
        }
    }
}


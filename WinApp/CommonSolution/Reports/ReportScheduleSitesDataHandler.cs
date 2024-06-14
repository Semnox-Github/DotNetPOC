/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Handler object of Report Schedule Sites
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0       04-Jan-2021   Laster Menezes      Created
 *********************************************************************************************/

using System;
using System.Data;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// Report Schedule Sites DataHandler
    /// </summary>
    public class ReportScheduleSitesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ReportScheduleSites AS rss ";


        private static readonly Dictionary<ReportScheduleSitesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReportScheduleSitesDTO.SearchByParameters, string>
            {
                {ReportScheduleSitesDTO.SearchByParameters.SCHEDULE_ID, "rss.ScheduleId"},
                {ReportScheduleSitesDTO.SearchByParameters.SITE_ID, "rss.site_id"},
                {ReportScheduleSitesDTO.SearchByParameters.MASTER_ENTITY_ID, "rss.MasterEntityId"},
            };


        /// <summary>
        /// Default constructor of ReportScheduleSitesDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportScheduleSitesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        ///  Parameterized constructor of ReportScheduleReportsDataHandler class
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportScheduleSitesDataHandler(string connectionString, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(connectionString, sqlTransaction);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                dataAccessHandler = new DataAccessHandler();
            }
            else
            {
                dataAccessHandler = new DataAccessHandler(connectionString);
            }

            log.LogMethodExit();
        }


        /// <summary>
        /// GetSQLParameters
        /// </summary>
        /// <param name="reportScheduleSitesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>List of SqlParameter</returns>
        private List<SqlParameter> GetSQLParameters(ReportScheduleSitesDTO reportScheduleSitesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleSitesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReportScheduleSitesId", reportScheduleSitesDTO.ReportScheduleSitesId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScheduleId", reportScheduleSitesDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReportScheduleSitesOrgID", reportScheduleSitesDTO.ReportScheduleSitesOrgId== -1 ? DBNull.Value 
                                                                    : (object)(reportScheduleSitesDTO.ReportScheduleSitesOrgId), true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReportScheduleSitesSiteID", reportScheduleSitesDTO.ReportScheduleSitesSiteId == -1 ? DBNull.Value 
                                                                    : (object)reportScheduleSitesDTO.ReportScheduleSitesSiteId));           
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", reportScheduleSitesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="reportScheduleSitesDTO"></param>
        internal void Delete(ReportScheduleSitesDTO reportScheduleSitesDTO)
        {
            log.LogMethodEntry(reportScheduleSitesDTO);
            string query = @"DELETE  
                             FROM ReportScheduleSites
                             WHERE ReportScheduleSites.Id = @ReportScheduleSitesId";
            SqlParameter parameter = new SqlParameter("@ReportScheduleSitesId", reportScheduleSitesDTO.ReportScheduleSitesId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            reportScheduleSitesDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// RefreshReportScheduleSitesDTO
        /// </summary>
        /// <param name="reportScheduleSitesDTO"></param>
        /// <param name="dt"></param>
        private void RefreshReportScheduleSitesDTO(ReportScheduleSitesDTO reportScheduleSitesDTO, DataTable dt)
        {
            log.LogMethodEntry(reportScheduleSitesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reportScheduleSitesDTO.ReportScheduleSitesId = Convert.ToInt32(dataRow["Id"]);
                reportScheduleSitesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                reportScheduleSitesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reportScheduleSitesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                reportScheduleSitesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                reportScheduleSitesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reportScheduleSitesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// GetReportScheduleSitesDTO
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>ReportScheduleSitesDTO</returns>
        private ReportScheduleSitesDTO GetReportScheduleSitesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ReportScheduleSitesDTO reportScheduleSitesDTO = new ReportScheduleSitesDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["ScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScheduleId"]),
                                                dataRow["ReportScheduleSitesOrgID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReportScheduleSitesOrgID"]),
                                                dataRow["ReportScheduleSitesSiteID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReportScheduleSitesSiteID"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                );
            log.LogMethodExit(reportScheduleSitesDTO);
            return reportScheduleSitesDTO;
        }


        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="reportScheduleSitesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>ReportScheduleSitesDTO</returns>
        internal ReportScheduleSitesDTO Insert(ReportScheduleSitesDTO reportScheduleSitesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleSitesDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ReportScheduleSites]
                               ([ScheduleId]
                               ,[ReportScheduleSitesOrgID]
                               ,[ReportScheduleSitesSiteID]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdateDate]
                               ,[site_id]
                               ,[Guid]
                               ,[MasterEntityId])                               
                         VALUES
                               (
                                @ScheduleId,
                                @ReportScheduleSitesOrgID,
                                @ReportScheduleSitesSiteID,                               
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                @SiteId,
                                NEWID(), 
                                @MasterEntityId
                                )
                                SELECT * FROM ReportScheduleSites WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(reportScheduleSitesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportScheduleSitesDTO(reportScheduleSitesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportScheduleSitesDTO);
            return reportScheduleSitesDTO;
        }


        /// <summary>
        /// Update
        /// </summary>
        /// <param name="reportScheduleSitesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>ReportScheduleSitesDTO</returns>
        internal ReportScheduleSitesDTO Update(ReportScheduleSitesDTO reportScheduleSitesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleSitesDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ReportScheduleSites] set
                               [ScheduleId]                 = @ScheduleId,
                               [ReportScheduleSitesOrgID]   = @ReportScheduleSitesOrgID,
                               [ReportScheduleSitesSiteID]  = @ReportScheduleSitesSiteID,                               
                               [MasterEntityId]             = @MasterEntityId,                              
                               [LastUpdatedBy]              = @LastUpdatedBy,
                               [LastUpdateDate]             = GETDATE()
                               where Id = @ReportScheduleSitesId
                             SELECT * FROM ReportScheduleSites WHERE Id = @ReportScheduleSitesId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(reportScheduleSitesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportScheduleSitesDTO(reportScheduleSitesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportScheduleSitesDTO);
            return reportScheduleSitesDTO;
        }


        /// <summary>
        /// GetReportScheduleSitesDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>List of ReportScheduleSitesDTOs</returns>
        internal List<ReportScheduleSitesDTO> GetReportScheduleSitesDTOList(List<KeyValuePair<ReportScheduleSitesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ReportScheduleSitesDTO> reportScheduleSitesDTO = new List<ReportScheduleSitesDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReportScheduleSitesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReportScheduleSitesDTO.SearchByParameters.SCHEDULE_ID ||
                            searchParameter.Key == ReportScheduleSitesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }                        
                        else if (searchParameter.Key == ReportScheduleSitesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
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
                reportScheduleSitesDTO = dataTable.Rows.Cast<DataRow>().Select(x => GetReportScheduleSitesDTO(x)).ToList();
            }
            log.LogMethodExit(reportScheduleSitesDTO);
            return reportScheduleSitesDTO;
        }
    }
}

/********************************************************************************************
 * Project Name - Site
 * Description  - Data Handler object of Roaming Site Entity
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0       21-Dec-2020   Lakshminarayana    Created for POS UI Redesign.
 *********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Roaming Site DataHandler
    /// </summary>
    public class RoamingSiteDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RoamingSites AS rs ";

        private static readonly Dictionary<RoamingSiteDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RoamingSiteDTO.SearchByParameters, string>
            {
                {RoamingSiteDTO.SearchByParameters.ROAMING_SITE_ID, "rs.RoamingSiteId"},
                {RoamingSiteDTO.SearchByParameters.AUTO_ROAM, "rs.AutoRoam"},
            };


        /// <summary>
        /// Parameterized Constructor for RoamingSiteDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public RoamingSiteDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(RoamingSiteDTO roamingSiteDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(roamingSiteDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", roamingSiteDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RoamingSiteId", roamingSiteDTO.RoamingSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteName", roamingSiteDTO.SiteName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteAddress", roamingSiteDTO.SiteAddress)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUploadTime", roamingSiteDTO.LastUploadTime)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@AutoRoam", roamingSiteDTO.AutoRoam? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        internal RoamingSiteDTO GetRoamingSiteDTO(int id)
        {
            log.LogMethodEntry(id);
            RoamingSiteDTO result = null;
            string query = SELECT_QUERY + @" WHERE rs.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRoamingSiteDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal void Delete(RoamingSiteDTO roamingSiteDTO)
        {
            log.LogMethodEntry(roamingSiteDTO);
            string query = @"DELETE  
                             FROM RoamingSite
                             WHERE RoamingSite.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", roamingSiteDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            roamingSiteDTO.AcceptChanges();
            log.LogMethodExit();
        }


        private void RefreshRoamingSiteDTO(RoamingSiteDTO roamingSiteDTO, DataTable dt)
        {
            log.LogMethodEntry(roamingSiteDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                roamingSiteDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                roamingSiteDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                roamingSiteDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                roamingSiteDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                roamingSiteDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
            }
            log.LogMethodExit();
        }


        private RoamingSiteDTO GetRoamingSiteDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RoamingSiteDTO roamingSiteDTO = new RoamingSiteDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                                dataRow["RoamingSiteId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RoamingSiteId"]),
                                                                dataRow["site_name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["site_name"]),
                                                                dataRow["site_address"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["site_address"]),
                                                                dataRow["LastuploadTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["LastuploadTime"]),
                                                                dataRow["AutoRoam"] == DBNull.Value ? false : Convert.ToString(dataRow["AutoRoam"]) == "Y",
                                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]));
            log.LogMethodExit(roamingSiteDTO);
            return roamingSiteDTO;
        }

        internal RoamingSiteDTO Insert(RoamingSiteDTO roamingSiteDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(roamingSiteDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RoamingSites]
                               ([RoamingSiteId]
                               ,[site_name]
                               ,[site_address]
                               ,[LastuploadTime]
                               ,[AutoRoam]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdateDate]
                               )
                               
                         VALUES
                               (
                                @RoamingSiteId,
                                @SiteName,
                                @SiteAddress,
                                @LastUploadTime,
                                @AutoRoam,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE())
                                SELECT * FROM RoamingSites WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(roamingSiteDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRoamingSiteDTO(roamingSiteDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(roamingSiteDTO);
            return roamingSiteDTO;
        }

        internal RoamingSiteDTO Update(RoamingSiteDTO roamingSiteDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(roamingSiteDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RoamingSites] set
                               [RoamingSiteId] = @RoamingSiteId,
                               [site_name] = @SiteName,
                               [site_address] = @SiteAddress,
                               [LastuploadTime] = @LastUploadTime,
                               [AutoRoam] = @AutoRoam,
                               [LastUpdatedBy] = @LastUpdatedBy,
                               [LastUpdateDate] = GETDATE()
                               where Id = @Id
                             SELECT * FROM RoamingSites WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(roamingSiteDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRoamingSiteDTO(roamingSiteDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(roamingSiteDTO);
            return roamingSiteDTO;
        }

        internal List<RoamingSiteDTO> GetRoamingSiteDTOList(List<KeyValuePair<RoamingSiteDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RoamingSiteDTO> roamingSiteDTOList = new List<RoamingSiteDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RoamingSiteDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RoamingSiteDTO.SearchByParameters.ROAMING_SITE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RoamingSiteDTO.SearchByParameters.AUTO_ROAM)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" || searchParameter.Value == "Y"? "Y" : "N"));
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
                roamingSiteDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRoamingSiteDTO(x)).ToList();
            }
            log.LogMethodExit(roamingSiteDTOList);
            return roamingSiteDTOList;
        }

        internal DateTime? GetRoamingSiteModuleLastUpdateTime()
        {
            log.LogMethodEntry();
            string query = @"select max(LastUpdateDate) LastUpdateDate from RoamingSites";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        30-May-2019   Girish Kundar           Created 
 *2.80      04-Jun-2020   Mushahid Faizan         Modified : Update Query
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// AppScreenProfileDataHandler Data Handler - Handles insert, update and select of  AppScreenProfile objects
    /// </summary>
    public class AppScreenProfileDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AppScreenProfile ";
        /// <summary>
        /// Dictionary for searching Parameters for the AppScreenProfile object.
        /// </summary>
        private static readonly Dictionary<AppScreenProfileDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AppScreenProfileDTO.SearchByParameters, string>
        {
            { AppScreenProfileDTO.SearchByParameters.APP_SCREEN_PROFILE_ID,"AppScreenProfileId"},
            { AppScreenProfileDTO.SearchByParameters.APP_SCREEN_PROFILE_NAME,"AppScreenProfileName"},
            { AppScreenProfileDTO.SearchByParameters.SITE_ID,"site_id"},
            { AppScreenProfileDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for AppScreenProfileDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public AppScreenProfileDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AppUIPanels Record.
        /// </summary>
        /// <param name="appScreenProfileDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(AppScreenProfileDTO appScreenProfileDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenProfileDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AppScreenProfileId", appScreenProfileDTO.AppScreenProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AppScreenProfileName", appScreenProfileDTO.AppScreenProfileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", appScreenProfileDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to AppScreenProfileDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>AppScreenProfileDTO</returns>
        private AppScreenProfileDTO GetAppScreenProfileDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AppScreenProfileDTO appScreenProfileDTO = new AppScreenProfileDTO(dataRow["AppScreenProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AppScreenProfileId"]),
                                                         dataRow["AppScreenProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["AppScreenProfileName"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(appScreenProfileDTO);
            return appScreenProfileDTO;
        }

        /// <summary>
        /// Gets the AppScreenProfile data of passed appScreenProfileId 
        /// </summary>
        /// <param name="appScreenProfileId"></param>
        /// <returns></returns>
        public AppScreenProfileDTO GetAppScreenProfile(int appScreenProfileId)
        {
            log.LogMethodEntry(appScreenProfileId);
            AppScreenProfileDTO result = null;
            string query = SELECT_QUERY + @" WHERE AppScreenProfile.AppScreenProfileId = @AppScreenProfileId";
            SqlParameter parameter = new SqlParameter("@AppScreenProfileId", appScreenProfileId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAppScreenProfileDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the appScreenProfile record
        /// </summary>
        /// <param name="appScreenProfileDTO"></param>
        internal void Delete(AppScreenProfileDTO appScreenProfileDTO)
        {
            log.LogMethodEntry(appScreenProfileDTO);
            string query = @"DELETE  
                             FROM AppScreenProfile
                             WHERE  AppScreenProfile.AppScreenProfileId = @AppScreenProfileId";
            SqlParameter parameter = new SqlParameter("@AppScreenProfileId", appScreenProfileDTO.AppScreenProfileId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            appScreenProfileDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the records to the AppScreenProfile table 
        /// </summary>
        /// <param name="appScreenProfileDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>

        public AppScreenProfileDTO Insert(AppScreenProfileDTO appScreenProfileDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenProfileDTO, userId, siteId);
            string query = @"INSERT INTO [dbo].[AppScreenProfile]
                               (
                                AppScreenProfileName
                               ,LastUpdatedBy
                               ,LastUpdatedDate
                               ,site_id
                               ,Guid
                               ,MasterEntityId
                               ,CreationDate
                               ,CreatedBy
                                )
                         VALUES
                               (
                                @AppScreenProfileName
                               ,@LastUpdatedBy
                               ,GETDATE()
                               ,@site_id
                               ,NEWID()
                               ,@MasterEntityId
                               ,GETDATE()
                               ,@CreatedBy
                                )  SELECT * FROM AppScreenProfile WHERE AppScreenProfileId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appScreenProfileDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAppScreenProfileDTO(appScreenProfileDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting AppScreenProfileDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appScreenProfileDTO);
            return appScreenProfileDTO;
        }
        /// <summary>
        /// Updates the records to the AppScreenProfile table 
        /// </summary>
        /// <param name="appScreenProfileDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AppScreenProfileDTO Update(AppScreenProfileDTO appScreenProfileDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenProfileDTO, userId, siteId);
            string query = @"UPDATE [dbo].[AppScreenProfile]
                               SET
                                AppScreenProfileName = @AppScreenProfileName
                               ,LastUpdatedBy = @LastUpdatedBy
                               ,LastUpdatedDate = GETDATE()
                               --,site_id = @site_id
                               ,MasterEntityId = @MasterEntityId
                                Where AppScreenProfileId = @AppScreenProfileId 
                                SELECT * FROM AppScreenProfile WHERE AppScreenProfileId = @AppScreenProfileId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appScreenProfileDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAppScreenProfileDTO(appScreenProfileDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating AppScreenProfileDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appScreenProfileDTO);
            return appScreenProfileDTO;
        }

        private void RefreshAppScreenProfileDTO(AppScreenProfileDTO appScreenProfileDTO, DataTable dt, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenProfileDTO, dt, userId, siteId);
            if (dt.Rows.Count > 0)
            {
                appScreenProfileDTO.AppScreenProfileId = Convert.ToInt32(dt.Rows[0]["AppScreenProfileId"]);
                appScreenProfileDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastupdatedDate"]);
                appScreenProfileDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                appScreenProfileDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                appScreenProfileDTO.LastUpdatedBy = userId;
                appScreenProfileDTO.CreatedBy = userId;
                appScreenProfileDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AppScreenProfileDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<AppScreenProfileDTO> GetAllappScreenProfile(List<KeyValuePair<AppScreenProfileDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AppScreenProfileDTO> appScreenProfileDTOList = new List<AppScreenProfileDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AppScreenProfileDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AppScreenProfileDTO.SearchByParameters.APP_SCREEN_PROFILE_ID
                            || searchParameter.Key == AppScreenProfileDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppScreenProfileDTO.SearchByParameters.APP_SCREEN_PROFILE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppScreenProfileDTO.SearchByParameters.SITE_ID)
                        { 
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppScreenProfileDTO.SearchByParameters.APP_SCREEN_PROFILE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        //else if (searchParameter.Key == AppScreenProfileDTO.SearchByParameters.ACTIVE_FLAG)  // bit
                        //{
                        //    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        //}
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AppScreenProfileDTO appScreenProfileDTO = GetAppScreenProfileDTO(dataRow);
                    appScreenProfileDTOList.Add(appScreenProfileDTO);
                }
            }
            log.LogMethodExit(appScreenProfileDTOList);
            return appScreenProfileDTOList;
        }
     
    }
}

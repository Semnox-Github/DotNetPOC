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
 *2.80      02-Jun-2020   Mushahid Faizan         Modified : 3 tier changes for RestAPI.
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
    /// AppScreenDataHandler Data Handler - Handles insert, update and select of  AppScreens objects
    /// </summary>
    public class AppScreenDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AppScreens ";
        /// <summary>
        /// Dictionary for searching Parameters for the AppUIPanels object.
        /// </summary>
        private static readonly Dictionary<AppScreenDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AppScreenDTO.SearchByParameters, string>
        {
            { AppScreenDTO.SearchByParameters.SCREEN_ID,"ScreenId"},
            { AppScreenDTO.SearchByParameters.SCREEN_KEY,"ScreenKey"},
            { AppScreenDTO.SearchByParameters.SCREEN_NAME,"ScreenName"},
            { AppScreenDTO.SearchByParameters.CODE_OBJECT_NAME,"CodeObjectName"},
            { AppScreenDTO.SearchByParameters.APP_SCREEN_PROFILE_ID,"AppScreenProfileId"},
            { AppScreenDTO.SearchByParameters.ACTIVE_FLAG,"ActiveFlag"},
            { AppScreenDTO.SearchByParameters.SITE_ID,"site_id"},
            { AppScreenDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for AppScreenDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public AppScreenDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AppUIPanels Record.
        /// </summary>
        /// <param name="appScreenDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(AppScreenDTO appScreenDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenId", appScreenDTO.ScreenId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenKey", appScreenDTO.ScreenKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenName", appScreenDTO.ScreenName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CodeObjectName", appScreenDTO.CodeObjectName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", appScreenDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AppScreenProfileId", appScreenDTO.AppScreenProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", appScreenDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to AppScreenDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>appUIPanelDTO</returns>
        private AppScreenDTO GetAppScreenDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AppScreenDTO appScreenDTO = new AppScreenDTO(dataRow["ScreenId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScreenId"]),
                                                         dataRow["ScreenName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ScreenName"]),
                                                         dataRow["CodeObjectName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CodeObjectName"]),
                                                         dataRow["ScreenKey"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ScreenKey"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["AppScreenProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AppScreenProfileId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ActiveFlag"]) // to be confirmed. if null then true or false?
                                                        );
            log.LogMethodExit(appScreenDTO);
            return appScreenDTO;
        }

        /// <summary>
        /// Gets the AppScreen data of passed screenId 
        /// </summary>
        /// <param name="screenId"></param>
        /// <returns></returns>
        public AppScreenDTO GetAppScreen(int screenId)
        {
            log.LogMethodEntry(screenId);
            AppScreenDTO result = null;
            string query = SELECT_QUERY + @" WHERE AppScreens.ScreenId = @ScreenId";
            SqlParameter parameter = new SqlParameter("@ScreenId", screenId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAppScreenDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the AppScreen record
        /// </summary>
        /// <param name="appScreenDTO"></param>
        internal void Delete(AppScreenDTO appScreenDTO)
        {
            log.LogMethodEntry(appScreenDTO);
            string query = @"DELETE  
                             FROM AppScreens
                             WHERE  AppScreens.ScreenId = @ScreenId";
            SqlParameter parameter = new SqlParameter("@ScreenId", appScreenDTO.ScreenId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            appScreenDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Inserts the record to the AppUIPanels Table.
        /// </summary>
        /// <param name="appScreenDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AppScreenDTO Insert(AppScreenDTO appScreenDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenDTO, userId, siteId);
            string query = @"INSERT INTO [dbo].[AppScreens]
                               (
                                ScreenName
                               ,CodeObjectName
                               ,ScreenKey
                               ,LastUpdatedBy
                               ,LastUpdatedDate
                               ,site_id
                               ,Guid
                               ,MasterEntityId
                               ,AppScreenProfileId
                               ,CreatedBy
                               ,CreationDate
                               ,ActiveFlag
                               )
                         VALUES
                               (
                                @ScreenName
                               ,@CodeObjectName
                               ,@ScreenKey
                               ,@LastUpdatedBy
                               ,GETDATE()
                               ,@site_id
                               ,NEWID()
                               ,@MasterEntityId
                               ,@AppScreenProfileId
                               ,@CreatedBy
                               ,GETDATE()
                               ,@ActiveFlag
                                ) SELECT * FROM AppScreens WHERE ScreenId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appScreenDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAppScreenDTO(appScreenDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting AppScreenDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appScreenDTO);
            return appScreenDTO;
        }

        /// <summary>
        /// Update the record to the AppUIPanels Table.
        /// </summary>
        /// <param name="appScreenDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AppScreenDTO Update(AppScreenDTO appScreenDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenDTO, userId, siteId);
            string query = @"UPDATE [dbo].[AppScreens]
                              SET
                                ScreenName = @ScreenName
                               ,CodeObjectName = @CodeObjectName
                               ,ScreenKey = @ScreenKey
                               ,LastUpdatedBy = @LastUpdatedBy
                               ,LastUpdatedDate = GETDATE()
                               ,site_id = @site_id
                               ,MasterEntityId = @MasterEntityId
                               ,AppScreenProfileId = @AppScreenProfileId
                               ,ActiveFlag = @ActiveFlag
                                where ScreenId = @ScreenId 
                                SELECT * FROM AppScreens WHERE ScreenId = @ScreenId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appScreenDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAppScreenDTO(appScreenDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating AppScreenDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appScreenDTO);
            return appScreenDTO;
        }
        private void RefreshAppScreenDTO(AppScreenDTO appScreenDTO, DataTable dt, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenDTO, dt, userId, siteId);
            if (dt.Rows.Count > 0)
            {
                appScreenDTO.ScreenId = Convert.ToInt32(dt.Rows[0]["ScreenId"]);
                appScreenDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastupdatedDate"]);
                appScreenDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                appScreenDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                appScreenDTO.LastUpdatedBy = userId;
                appScreenDTO.CreatedBy = userId;
                appScreenDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AppScreenDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<AppScreenDTO> GetAllAppScreen(List<KeyValuePair<AppScreenDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AppScreenDTO> appScreenDTOList = new List<AppScreenDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            // should get child records also
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AppScreenDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AppScreenDTO.SearchByParameters.SCREEN_ID 
                            ||searchParameter.Key == AppScreenDTO.SearchByParameters.APP_SCREEN_PROFILE_ID 
                            ||searchParameter.Key == AppScreenDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppScreenDTO.SearchByParameters.SCREEN_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppScreenDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppScreenDTO.SearchByParameters.SCREEN_KEY
                                || searchParameter.Key == AppScreenDTO.SearchByParameters.SCREEN_NAME
                                || searchParameter.Key == AppScreenDTO.SearchByParameters.CODE_OBJECT_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppScreenDTO.SearchByParameters.ACTIVE_FLAG)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AppScreenDTO appScreenDTO = GetAppScreenDTO(dataRow);
                    appScreenDTOList.Add(appScreenDTO);
                }
            }
            log.LogMethodExit(appScreenDTOList);
            return appScreenDTOList;
        }

    }
}

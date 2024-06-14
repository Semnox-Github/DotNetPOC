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
using System.Text;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// AppScreenUIPanelDataHandler Data Handler - Handles insert, update and select of  AppScreenProfile objects
    /// </summary>
    public class AppScreenUIPanelDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AppScreenUIPanels ";
        /// <summary>
        /// Dictionary for searching Parameters for the AppScreenProfile object.
        /// </summary>
        private static readonly Dictionary<AppScreenUIPanelDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AppScreenUIPanelDTO.SearchByParameters, string>
        {
            {AppScreenUIPanelDTO.SearchByParameters.SCREEN_PANEL_ID,"ScreenUIPanelId"},
            {AppScreenUIPanelDTO.SearchByParameters.SCREEN_ID,"ScreenId"},
            {AppScreenUIPanelDTO.SearchByParameters.SCREEN_ID_LIST,"ScreenId"},
            {AppScreenUIPanelDTO.SearchByParameters.UI_PANEL_ID,"UIPanelId"},
            {AppScreenUIPanelDTO.SearchByParameters.UI_PANEL_INDEX,"UIPanelIndex"},
            {AppScreenUIPanelDTO.SearchByParameters.SITE_ID,"site_id"},
            {AppScreenUIPanelDTO.SearchByParameters.ACTIVE_FLAG,"ActiveFlag"},
            {AppScreenUIPanelDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for AppScreenUIPanelDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public AppScreenUIPanelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AppScreenUIPanel Record.
        /// </summary>
        /// <param name="appScreenUIPanelDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(AppScreenUIPanelDTO appScreenUIPanelDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenUIPanelDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenUIPanelId", appScreenUIPanelDTO.ScreenUIPanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenId", appScreenUIPanelDTO.ScreenId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelId", appScreenUIPanelDTO.UIPanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelIndex", appScreenUIPanelDTO.UIPanelIndex));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", appScreenUIPanelDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", appScreenUIPanelDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to GetAppScreenUIPanelDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>AppScreenProfileDTO</returns>
        private AppScreenUIPanelDTO GetAppScreenUIPanelDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AppScreenUIPanelDTO appScreenUIPanelDTO = new AppScreenUIPanelDTO(
                                                         dataRow["ScreenUIPanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScreenUIPanelId"]),
                                                         dataRow["ScreenId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScreenId"]),
                                                         dataRow["UIPanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIPanelId"]),
                                                         dataRow["UIPanelIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIPanelIndex"]),
                                                         dataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ActiveFlag"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(appScreenUIPanelDTO);
            return appScreenUIPanelDTO;
        }

        /// <summary>
        /// Gets the AppScreenProfile data of passed ScreenUIPanelId 
        /// </summary>
        /// <param name="screenUIPanelId"></param>
        /// <returns></returns>
        public AppScreenUIPanelDTO GetAppScreenUIPanel(int screenUIPanelId)
        {
            log.LogMethodEntry(screenUIPanelId);
            AppScreenUIPanelDTO result = null;
            string query = SELECT_QUERY + @" WHERE AppScreenUIPanel.ScreenUIPanelId = @ScreenUIPanelId";
            SqlParameter parameter = new SqlParameter("@ScreenUIPanelId", screenUIPanelId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAppScreenUIPanelDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the appScreenUIPanel record
        /// </summary>
        /// <param name="appScreenUIPanelDTO"></param>
        internal void Delete(AppScreenUIPanelDTO appScreenUIPanelDTO)
        {
            log.LogMethodEntry(appScreenUIPanelDTO);
            string query = @"DELETE  
                             FROM AppScreenUIPanels
                             WHERE AppScreenUIPanels.ScreenUIPanelId = @ScreenUIPanelId";
            SqlParameter parameter = new SqlParameter("@ScreenUIPanelId", appScreenUIPanelDTO.ScreenUIPanelId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            appScreenUIPanelDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the records to the AppScreenUIPanels table 
        /// </summary>
        /// <param name="appScreenUIPanelDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AppScreenUIPanelDTO Insert(AppScreenUIPanelDTO appScreenUIPanelDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenUIPanelDTO, userId, siteId);
            string query = @"INSERT INTO [dbo].[AppScreenUIPanels]
                           (ScreenId
                           ,UIPanelId
                           ,UIPanelIndex
                           ,ActiveFlag
                           ,LastUpdatedBy
                           ,LastUpdatedDate
                           ,site_id
                           ,Guid
                           ,MasterEntityId
                           ,CreatedBy
                           ,CreationDate)
                     VALUES
                           (@ScreenId
                           ,@UIPanelId
                           ,@UIPanelIndex
                           ,@ActiveFlag
                           ,@LastUpdatedBy
                           ,GETDATE()
                           ,@site_id
                           ,NEWID()
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,GETDATE() )  SELECT * FROM AppScreenUIPanels WHERE ScreenUIPanelId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appScreenUIPanelDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAppScreenUIPanelDTO(appScreenUIPanelDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting AppScreenUIPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appScreenUIPanelDTO);
            return appScreenUIPanelDTO;
        }

        /// <summary>
        /// Update the records to the AppScreenUIPanels table 
        /// </summary>
        /// <param name="appScreenUIPanelDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AppScreenUIPanelDTO Update(AppScreenUIPanelDTO appScreenUIPanelDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenUIPanelDTO, userId, siteId);
            string query = @"UPDATE  [dbo].[AppScreenUIPanels]
                           SET
                            ScreenId = @ScreenId
                           ,UIPanelId = @UIPanelId
                           ,UIPanelIndex = @UIPanelIndex
                           ,ActiveFlag = @ActiveFlag
                           ,LastUpdatedBy = @LastUpdatedBy
                           ,LastUpdatedDate = GETDATE()
                           --,site_id = @site_id
                           ,MasterEntityId = @MasterEntityId
                            WHERE ScreenUIPanelId = @ScreenUIPanelId
                            SELECT * FROM AppScreenUIPanels WHERE ScreenUIPanelId = @ScreenUIPanelId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appScreenUIPanelDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAppScreenUIPanelDTO(appScreenUIPanelDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating AppScreenUIPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appScreenUIPanelDTO);
            return appScreenUIPanelDTO;
        }
        private void RefreshAppScreenUIPanelDTO(AppScreenUIPanelDTO appScreenUIPanelDTO, DataTable dt, string userId, int siteId)
        {
            log.LogMethodEntry(appScreenUIPanelDTO, dt, userId, siteId);
            if (dt.Rows.Count > 0)
            {
                appScreenUIPanelDTO.ScreenUIPanelId = Convert.ToInt32(dt.Rows[0]["ScreenUIPanelId"]);
                appScreenUIPanelDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastupdatedDate"]);
                appScreenUIPanelDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                appScreenUIPanelDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                appScreenUIPanelDTO.LastUpdatedBy = userId;
                appScreenUIPanelDTO.CreatedBy = userId;
                appScreenUIPanelDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AppScreenUIPanelDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<AppScreenUIPanelDTO> GetAllAppScreenUIPanel(List<KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AppScreenUIPanelDTO> appScreenUIPanelDTOList = new List<AppScreenUIPanelDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AppScreenUIPanelDTO.SearchByParameters.SCREEN_PANEL_ID
                            || searchParameter.Key == AppScreenUIPanelDTO.SearchByParameters.SCREEN_ID
                            || searchParameter.Key == AppScreenUIPanelDTO.SearchByParameters.UI_PANEL_ID
                            || searchParameter.Key == AppScreenUIPanelDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppScreenUIPanelDTO.SearchByParameters.SCREEN_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppScreenUIPanelDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppScreenUIPanelDTO.SearchByParameters.ACTIVE_FLAG)  // bit
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
                    AppScreenUIPanelDTO appScreenUIPanelDTO = GetAppScreenUIPanelDTO(dataRow);
                    appScreenUIPanelDTOList.Add(appScreenUIPanelDTO);
                }
            }
            log.LogMethodExit(appScreenUIPanelDTOList);
            return appScreenUIPanelDTOList;
        }
    }
}

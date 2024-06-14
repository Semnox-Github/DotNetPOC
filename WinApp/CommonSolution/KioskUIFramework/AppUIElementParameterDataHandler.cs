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
*2.80        05-Jun-2020   Mushahid Faizan         Modified : 3 Tier Changes for Rest API.
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
    /// AppUIElementParameterDataHandler Data Handler - Handles insert, update and select of  AppUIElementParameters objects
    /// </summary>
    public class AppUIElementParameterDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AppUIElementParameters aep ";
        /// <summary>
        /// Dictionary for searching Parameters for the AppUIElementParameters object.
        /// </summary>
        private static readonly Dictionary<AppUIElementParameterDTO.SearchByParameters, string> DBSearchParameters
                                                              = new Dictionary<AppUIElementParameterDTO.SearchByParameters, string>
        {
            {AppUIElementParameterDTO.SearchByParameters.PARAMETER_ID,"aep.ParameterId"},
            {AppUIElementParameterDTO.SearchByParameters.PARAMETER_ID_LIST,"aep.ParameterId"},
            {AppUIElementParameterDTO.SearchByParameters.PARAMETER_NAME,"aep.ParameterName"},
            {AppUIElementParameterDTO.SearchByParameters.PARENT_PARAMETER_ID,"aep.ParentParameterId"},
            {AppUIElementParameterDTO.SearchByParameters.ACTION_SCREEN_ID,"aep.ActionScreenId"},
            {AppUIElementParameterDTO.SearchByParameters.SCREEN_GROUP,"aep.ScreenGroup"},
            {AppUIElementParameterDTO.SearchByParameters.UI_PANEL_ELEMENT_ID,"aep.UIPanelElementId"},
            {AppUIElementParameterDTO.SearchByParameters.UI_PANEL_ELEMENT_ID_LIST,"aep.UIPanelElementId"},
            {AppUIElementParameterDTO.SearchByParameters.SITE_ID,"aep.site_id"},
            {AppUIElementParameterDTO.SearchByParameters.ACTIVE_FLAG,"aep.ActiveFlag"},
            {AppUIElementParameterDTO.SearchByParameters.MASTER_ENTITY_ID,"aep.MasterEntityId"}
        };
        /// <summary>
        /// Parameterized Constructor for AppUIElementParameterDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public AppUIElementParameterDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AppUIElementParameters Record.
        /// </summary>
        /// <param name="appUIElementParameterDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(AppUIElementParameterDTO appUIElementParameterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIElementParameterDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParameterId", appUIElementParameterDTO.ParameterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActionScreenId", appUIElementParameterDTO.ActionScreenId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentParameterId", appUIElementParameterDTO.ParentParameterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelElementId", appUIElementParameterDTO.UIPanelElementId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SQLBindName", appUIElementParameterDTO.SQLBindName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenGroup", appUIElementParameterDTO.ScreenGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParameterName", appUIElementParameterDTO.ParameterName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Parameter", appUIElementParameterDTO.Parameter));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayIndex", appUIElementParameterDTO.DisplayIndex));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", appUIElementParameterDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", appUIElementParameterDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to AppUIElementParameterDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>AppUIElementParameterDTO</returns>
        private AppUIElementParameterDTO GetAppUIElementParameterDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AppUIElementParameterDTO appUIElementParameterDTO = new AppUIElementParameterDTO(
                                                         dataRow["ParameterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParameterId"]),
                                                         dataRow["ParameterName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParameterName"]),
                                                         dataRow["Parameter"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Parameter"]),
                                                         dataRow["UIPanelElementId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIPanelElementId"]),
                                                         dataRow["DisplayIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayIndex"]),
                                                         dataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ActiveFlag"]),
                                                         dataRow["SQLBindName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SQLBindName"]),
                                                         dataRow["ParentParameterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentParameterId"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["ScreenGroup"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScreenGroup"].ToString()),
                                                         dataRow["ActionScreenId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ActionScreenId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(appUIElementParameterDTO);
            return appUIElementParameterDTO;
        }
        /// <summary>
        /// Deletes the appUIElementParameterDTO record
        /// </summary>
        /// <param name="appUIElementParameterDTO"></param>
        internal void Delete(AppUIElementParameterDTO appUIElementParameterDTO)
        {
                log.LogMethodEntry(appUIElementParameterDTO);
                string query = @"DELETE  
                             FROM AppUIElementParameters
                             WHERE AppUIElementParameters.ParameterId = @ParameterId";
                SqlParameter parameter = new SqlParameter("@ParameterId", appUIElementParameterDTO.ParameterId);
                dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
                log.LogMethodExit();
        }

        /// <summary>
        /// Gets the AppUIElementParameters data of passed parameterId 
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public AppUIElementParameterDTO GetAppUIElementParameter(int parameterId)
        {
            log.LogMethodEntry(parameterId);
            AppUIElementParameterDTO result = null;
            string query = SELECT_QUERY + @" WHERE aep.ParameterId = @ParameterId";
            SqlParameter parameter = new SqlParameter("@ParameterId", parameterId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAppUIElementParameterDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Inserts the record to AppUIElementParameters table
        /// </summary>
        /// <param name="appUIElementParameterDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AppUIElementParameterDTO Insert(AppUIElementParameterDTO appUIElementParameterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIElementParameterDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[AppUIElementParameters]
                           (ParameterName
                           ,Parameter
                           ,UIPanelElementId
                           ,DisplayIndex
                           ,ActiveFlag
                           ,SQLBindName
                           ,ParentParameterId
                           ,MasterEntityId
                           ,LastUpdatedBy
                           ,LastUpdatedDate
                           ,site_id
                           ,Guid
                           ,ScreenGroup
                           ,ActionScreenId
                           ,CreatedBy
                           ,CreationDate)
                     VALUES
                           (@ParameterName
                           ,@Parameter
                           ,@UIPanelElementId
                           ,@DisplayIndex
                           ,@ActiveFlag
                           ,@SQLBindName
                           ,@ParentParameterId
                           ,@MasterEntityId
                           ,@LastUpdatedBy
                           ,GETDATE()
                           ,@site_id
                           ,NEWID()
                           ,@ScreenGroup
                           ,@ActionScreenId
                           ,@CreatedBy
                           ,GETDATE()) SELECT * FROM AppUIElementParameters WHERE ParameterId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIElementParameterDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIElementParameterDTO(appUIElementParameterDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AppUIElementParameterDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIElementParameterDTO);
            return appUIElementParameterDTO;
        }

        /// <summary>
        /// Updates the record to AppUIElementParameters table
        /// </summary>
        /// <param name="appUIElementParameterDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AppUIElementParameterDTO Update(AppUIElementParameterDTO appUIElementParameterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIElementParameterDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[AppUIElementParameters]
                           SET 
                            ParameterName = @ParameterName
                           ,Parameter = @Parameter
                           ,UIPanelElementId = @UIPanelElementId
                           ,DisplayIndex = @DisplayIndex
                           ,ActiveFlag = @ActiveFlag
                           ,SQLBindName = @SQLBindName
                           ,ParentParameterId = @ParentParameterId
                           ,MasterEntityId = @MasterEntityId
                           ,LastUpdatedBy = @LastUpdatedBy
                           ,LastUpdatedDate = GETDATE()
                           ,ScreenGroup = @ScreenGroup
                           ,ActionScreenId = @ActionScreenId
                           where ParameterId = @ParameterId
                          SELECT * FROM AppUIElementParameters WHERE  ParameterId = @ParameterId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIElementParameterDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIElementParameterDTO(appUIElementParameterDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AppUIElementParameterDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIElementParameterDTO);
            return appUIElementParameterDTO;
        }

        private void RefreshAppUIElementParameterDTO(AppUIElementParameterDTO appUIElementParameterDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIElementParameterDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                appUIElementParameterDTO.ParameterId = Convert.ToInt32(dt.Rows[0]["ParameterId"]);
                appUIElementParameterDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastupdatedDate"]);
                appUIElementParameterDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                appUIElementParameterDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                appUIElementParameterDTO.LastUpdatedBy = loginId;
                appUIElementParameterDTO.CreatedBy = loginId;
                appUIElementParameterDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AppUIElementParameterDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<AppUIElementParameterDTO> GetAllAppUIElementParameter(List<KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AppUIElementParameterDTO> appUIElementParameterDTOList = new List<AppUIElementParameterDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.PARENT_PARAMETER_ID
                            || searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.PARAMETER_ID
                            || searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.UI_PANEL_ELEMENT_ID
                            || searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.ACTION_SCREEN_ID
                            || searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.PARAMETER_NAME
                            || searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.PARAMETER
                            || searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.SQL_BIND_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.UI_PANEL_ELEMENT_ID_LIST
                                || searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.PARAMETER_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIElementParameterDTO.SearchByParameters.ACTIVE_FLAG)  // bit
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
                    AppUIElementParameterDTO appUIElementParameterDTO = GetAppUIElementParameterDTO(dataRow);
                    appUIElementParameterDTOList.Add(appUIElementParameterDTO);
                }
            }
            log.LogMethodExit(appUIElementParameterDTOList);
            return appUIElementParameterDTOList;
        }

        /// <summary>
        /// GetAllAppUIPanelElements 
        /// </summary>
        /// <param name="appUIPanelElementIdList"></param>
        /// <param name="activeChildRecords"></param>
        /// <returns></returns>
        public List<AppUIElementParameterDTO> GetAllAppUIElementParameter(List<int> appUIPanelElementIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(appUIPanelElementIdList, activeChildRecords);
            string query = SELECT_QUERY + " INNER JOIN @AppUIPanelElementIdList List ON AppUIElementParameters.UIPanelElementId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND ActiveFlag = 1 ";
            }
            DataTable dataTable = dataAccessHandler.BatchSelect(query, "@AppUIPanelElementIdList", appUIPanelElementIdList, null, sqlTransaction);
            List<AppUIElementParameterDTO> appUIElementParameterDTOList = GetAppUIElementParameterDTOList(dataTable);
            log.LogMethodExit(appUIElementParameterDTOList);
            return appUIElementParameterDTOList;
        }
        private List<AppUIElementParameterDTO> GetAppUIElementParameterDTOList(DataTable dataTable)
        {
            log.LogMethodEntry(dataTable);
            List<AppUIElementParameterDTO> appUIElementParameterDTOList = new List<AppUIElementParameterDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AppUIElementParameterDTO appUIElementParameterDTO = GetAppUIElementParameterDTO(dataRow);
                    appUIElementParameterDTOList.Add(appUIElementParameterDTO);
                }
            }
            log.LogMethodExit(appUIElementParameterDTOList);
            return appUIElementParameterDTOList;
        }
    }
}

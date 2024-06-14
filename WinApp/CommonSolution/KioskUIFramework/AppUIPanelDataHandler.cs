/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Data Handler -AppUIPanelDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      17-May-2019   Girish Kundar           Created 
 *2.80      04-Jun-2020   Mushahid Faizan         Modified : Update Query
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// AppUIPanel Data Handler - Handles insert, update and select of  AppUIPanel objects
    /// </summary>
    public class AppUIPanelDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AppUIPanels AS aup"; // Create alias names for the Table
        /// <summary>
        /// Dictionary for searching Parameters for the AppUIPanel object.
        /// </summary>
        private static readonly Dictionary<AppUIPanelDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AppUIPanelDTO.SearchByParameters, string>
        {
            { AppUIPanelDTO.SearchByParameters.UI_PANEL_ID,"aup.UIPanelId"},
            { AppUIPanelDTO.SearchByParameters.UI_PANEL_ID_LIST,"aup.UIPanelId"},
            { AppUIPanelDTO.SearchByParameters.UI_PANEL_KEY,"aup.UIPanelKey"},
            { AppUIPanelDTO.SearchByParameters.UI_PANEL_NAME,"aup.UIPanelName"},
            { AppUIPanelDTO.SearchByParameters.APP_SCREEN_PROFILE_ID,"aup.AppScreenProfileId"},
            { AppUIPanelDTO.SearchByParameters.ACTIVE_FLAG,"aup.ActiveFlag"},
            { AppUIPanelDTO.SearchByParameters.SITE_ID,"aup.site_id"},
            { AppUIPanelDTO.SearchByParameters.MASTER_ENTITY_ID,"aup.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for AppUIPanelDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AppUIPanelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AppUIPanel Record.
        /// </summary>
        /// <param name="appUIPanelDTO">appUIPanelDTO object</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AppUIPanelDTO appUIPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelId", appUIPanelDTO.UIPanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelName", appUIPanelDTO.UIPanelName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelKey", appUIPanelDTO.UIPanelKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PanelWidth", appUIPanelDTO.PanelWidth));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", appUIPanelDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AppScreenProfileId", appUIPanelDTO.AppScreenProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", appUIPanelDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Deletes the AppUIPanel record
        /// </summary>
        /// <param name="appUIPanelDTO">appUIPanelDTO is passed as parameter</param>
        internal void Delete(AppUIPanelDTO appUIPanelDTO)
        {
            log.LogMethodEntry(appUIPanelDTO);
            string query = @"DELETE  
                             FROM AppUIPanels
                             WHERE AppUIPanels.UIPanelId = @UIPanelId";
            SqlParameter parameter = new SqlParameter("@UIPanelId", appUIPanelDTO.UIPanelId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            appUIPanelDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to AppUIPanelDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object of type DataRow</param>
        /// <returns>Object of AppUIPanelDTO</returns>
        private AppUIPanelDTO GetAppUIPanelDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AppUIPanelDTO appUIPanelDTO = new AppUIPanelDTO(dataRow["UIPanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIPanelId"]),
                                                         dataRow["UIPanelName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UIPanelName"]),
                                                         dataRow["UIPanelKey"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UIPanelKey"]),
                                                         dataRow["PanelWidth"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["PanelWidth"]),
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
            log.LogMethodExit(appUIPanelDTO);
            return appUIPanelDTO;
        }

        /// <summary>
        /// Gets the AppUIPanel data of passed UIPanelId 
        /// </summary>
        /// <param name="uiPanelId">uiPanelId passed as parameter</param>
        /// <returns>AppUIPanelElementsDTO</returns>
        public AppUIPanelDTO GetAppUIPanel(int uiPanelId)
        {
            log.LogMethodEntry(uiPanelId);
            AppUIPanelDTO result = null;
            string query = SELECT_QUERY + @" WHERE aup.UIPanelId = @UIPanelId";
            SqlParameter parameter = new SqlParameter("@UIPanelId", uiPanelId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAppUIPanelDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the AppUIPanel Table.
        /// </summary>
        /// <param name="appUIPanelDTO">appUIPanelDTO object</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>object of AppUIPanelDTO</returns>
        public AppUIPanelDTO Insert(AppUIPanelDTO appUIPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[AppUIPanels]
                            (
                            UIPanelName ,
                            UIPanelKey,
                            PanelWidth ,
                            LastUpdatedBy ,
                            LastUpdatedDate ,
                            site_id,
                            Guid ,
                            MasterEntityId,
                            AppScreenProfileId,
                            CreatedBy,
                            CreationDate,
                            ActiveFlag
                            )
                        VALUES
                            (
                            @UIPanelName,
                            @UIPanelKey,
                            @PanelWidth,
                            @LastUpdatedBy,
                            GETDATE(),
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @AppScreenProfileId,
                            @CreatedBy,
                            GETDATE(),
                            @ActiveFlag
                         )
                            SELECT * FROM AppUIPanels WHERE UIPanelId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIPanelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIPanelDTO(appUIPanelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AppUIPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIPanelDTO);
            return appUIPanelDTO;
        }

        /// <summary>
        ///  Updates the record to the AppUIPanel Table.
        /// </summary>
        /// <param name="appUIPanelDTO">AppUIPanelDTO object</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>returns the object of AppUIPanelDTO</returns>
        public AppUIPanelDTO Update(AppUIPanelDTO appUIPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AppUIPanels]
                            SET
                            UIPanelName =@UIPanelName,
                            UIPanelKey = @UIPanelKey,
                            PanelWidth =  @PanelWidth,
                            LastUpdatedBy = @LastUpdatedBy,
                            LastUpdatedDate = GETDATE(),
                           -- site_id = @site_id,
                            MasterEntityId = @MasterEntityId,
                            AppScreenProfileId = @AppScreenProfileId,
                            ActiveFlag = @ActiveFlag
                            where UIPanelId = @UIPanelId 
                           SELECT * FROM AppUIPanels WHERE UIPanelId = @UIPanelId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIPanelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIPanelDTO(appUIPanelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating AppUIPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIPanelDTO);
            return appUIPanelDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="appUIPanelDTO">AppUIPanelDTO object is passed as parameter</param>
        /// <param name="dt">dt object of DataTable to hold the current record</param>
       
        private void RefreshAppUIPanelDTO(AppUIPanelDTO appUIPanelDTO, DataTable dt)
        {
            log.LogMethodEntry(appUIPanelDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                appUIPanelDTO.UIPanelId = Convert.ToInt32(dt.Rows[0]["UIPanelId"]);
                appUIPanelDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                appUIPanelDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                appUIPanelDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                appUIPanelDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                appUIPanelDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                appUIPanelDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AppUIPanelDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>returns the AppUIPanelDTO List</returns>
        public List<AppUIPanelDTO> GetAppUIPanels(List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AppUIPanelDTO> appUIPanelsDTOList = new List<AppUIPanelDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AppUIPanelDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AppUIPanelDTO.SearchByParameters.UI_PANEL_ID ||
                            searchParameter.Key == AppUIPanelDTO.SearchByParameters.APP_SCREEN_PROFILE_ID ||
                             searchParameter.Key == AppUIPanelDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIPanelDTO.SearchByParameters.UI_PANEL_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppUIPanelDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" +DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIPanelDTO.SearchByParameters.UI_PANEL_KEY 
                            ||   searchParameter.Key == AppUIPanelDTO.SearchByParameters.UI_PANEL_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppUIPanelDTO.SearchByParameters.ACTIVE_FLAG)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        //  Reference For the search by Date Parameter.
                        // else if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN ||
                        //        searchParameter.Key == DiscountCouponsDTO.SearchByParameters.START_DATE_GREATER_THAN)
                        //{
                        //    query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE())>=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        //}
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
                    AppUIPanelDTO appUIPanelsDTO = GetAppUIPanelDTO(dataRow);
                    appUIPanelsDTOList.Add(appUIPanelsDTO);
                }
            }
            log.LogMethodExit(appUIPanelsDTOList);
            return appUIPanelsDTOList;
        }

    }

}


/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Data Handler -AppUIPanelElementDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      17-May-2019   Girish Kundar           Created 
 *2.80      04-Jun-2020   Mushahid Faizan          Modified : Update Query and Added elementIndex column.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// AppUIPanelElement Data Handler - Handles insert, update and select of  AppUIPanelElement objects
    /// </summary>
    internal class AppUIPanelElementDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AppUIPanelElements AS ape ";

        /// <summary>
        /// Dictionary for searching Parameters for the AppUIPanelElement object.
        /// </summary>
        private static readonly Dictionary<AppUIPanelElementDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AppUIPanelElementDTO.SearchByParameters, string>
        {
            { AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ELEMENT_ID,"ape.UIPanelElementId"},
            { AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ELEMENT_ID_LIST,"ape.UIPanelElementId"},
            { AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ID,"ape.UIPanelId"},
            { AppUIPanelElementDTO.SearchByParameters.ACTION_SCREEN_ID,"ape.ActionScreenId"},
            { AppUIPanelElementDTO.SearchByParameters.ELEMENT_NAME,"ape.ElementName"},
            { AppUIPanelElementDTO.SearchByParameters.ACTIVE_FLAG,"ape.ActiveFlag"},
            { AppUIPanelElementDTO.SearchByParameters.SITE_ID,"ape.site_id"},
            { AppUIPanelElementDTO.SearchByParameters.MASTER_ENTITY_ID,"ape.MasterEntityId"},
            { AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ID_LIST,"ape.UIPanelId"}
        };
        /// <summary>
        /// Parameterized Constructor for AppUIPanelElementDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">object of SqlTransaction</param>
        public AppUIPanelElementDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AppUIPanelElement Record.
        /// </summary>
        /// <param name="appUIPanelElementDTO">AppUIPanelElementDTO object</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AppUIPanelElementDTO appUIPanelElementDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelElementDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelElementId", appUIPanelElementDTO.UIPanelElementId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelId", appUIPanelElementDTO.UIPanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ElementName", appUIPanelElementDTO.ElementName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActionScreenId", appUIPanelElementDTO.ActionScreenId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", appUIPanelElementDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", appUIPanelElementDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@elementIndex", appUIPanelElementDTO.ElementIndex));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Inserts the record to the AppUIPanelElement Table.
        /// </summary>
        /// <param name="appUIPanelElementDTO">appUIPanelElementDTO object</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns>Returns the object of AppUIPanelElementDTO</returns>
        public AppUIPanelElementDTO Insert(AppUIPanelElementDTO appUIPanelElementDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelElementDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AppUIPanelElements]
                            (
                               UIPanelId,
                               ElementName,
                               ActionScreenId,
                               ActiveFlag,
                               LastUpdatedBy,
                               LastUpdatedDate,
                               site_id,
                               Guid,
                               MasterEntityId,
                               CreatedBy,
                               CreationDate, ElementIndex
                            )
                        VALUES
                                (
                                @UIPanelId,
                                @ElementName  ,                   
                                @ActionScreenId,
                                @ActiveFlag,
                                @LastUpdatedBy,
                                GETDATE(),
                                @site_id,
                                NEWID()  ,  
                                @MasterEntityId,
                                @CreatedBy,
                                GETDATE(), @elementIndex
                            )
                        SELECT * FROM AppUIPanelElements WHERE UIPanelElementId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIPanelElementDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIPanelElementDTO(appUIPanelElementDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AppUIPanelElementDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIPanelElementDTO);
            return appUIPanelElementDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update , DTO can be accessed anywhere with full data captured in it.
        /// </summary>
        /// <param name="appUIPanelElementDTO">AppUIPanelElementDTO object is passed as parameter</param>
        /// <param name="dt"> dt object of DataTable to hold the current record</param>
        private void RefreshAppUIPanelElementDTO(AppUIPanelElementDTO appUIPanelElementDTO, DataTable dt)
        {
            log.LogMethodEntry(appUIPanelElementDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                appUIPanelElementDTO.UIPanelElementId = Convert.ToInt32(dt.Rows[0]["UIPanelElementId"]);
                appUIPanelElementDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                appUIPanelElementDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                appUIPanelElementDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                appUIPanelElementDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                appUIPanelElementDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                appUIPanelElementDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the AppUIPanelElement record based on UIPanelElementId of appUIPanelElementDTO
        /// </summary>
        /// <param name="appUIPanelElementDTO">appUIPanelElementDTO object</param>
        internal void Delete(AppUIPanelElementDTO appUIPanelElementDTO)
        {
            log.LogMethodEntry(appUIPanelElementDTO);
            string query = @"DELETE  
                             FROM AppUIPanelElements
                             WHERE AppUIPanelElements.UIPanelElementId = @UIPanelElementId";
            SqlParameter parameter = new SqlParameter("@UIPanelElementId", appUIPanelElementDTO.UIPanelElementId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        ///  Updates the record to the AppUIPanelElement Table.
        /// </summary>
        /// <param name="appUIPanelElementDTO">AppUIPanelElementDTO object passed as parameter</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site Id of user </param>
        /// <returns>Returns the object of AppUIPanelElementDTO</returns>
        public AppUIPanelElementDTO Update(AppUIPanelElementDTO appUIPanelElementDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelElementDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AppUIPanelElements]
                            SET
                                UIPanelId =@UIPanelId,
                                ElementName = @ElementName,
                                ActionScreenId = @ActionScreenId,
                                ActiveFlag = @ActiveFlag   ,                  
                                LastUpdatedBy = @LastUpdatedBy,
                                LastUpdatedDate = GETDATE(),
                                --site_id  = @site_id ,
                                MasterEntityId = @MasterEntityId,
                                ElementIndex = @elementIndex
                                Where UIPanelElementId  = @UIPanelElementId
                            SELECT * FROM AppUIPanelElements WHERE UIPanelElementId = @UIPanelElementId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIPanelElementDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIPanelElementDTO(appUIPanelElementDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AppUIPanelElementDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIPanelElementDTO);
            return appUIPanelElementDTO;
        }
        /// <summary>
        /// Returns the List AppUIPanelElementDTO from the DataTable object.
        /// </summary>
        /// <param name="dataTable">dataTable object of DataTable is passed as parameter.</param>
        /// <returns>Returns the List of AppUIPanelElementDTO </returns>
        private List<AppUIPanelElementDTO> GetAppUIPanelElementDTOList(DataTable dataTable)
        {
            log.LogMethodEntry(dataTable);
            List<AppUIPanelElementDTO> appUIPanelElementsList = new List<AppUIPanelElementDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AppUIPanelElementDTO appUIPanelElementDTO = GetAppUIPanelElementDTO(dataRow);
                    appUIPanelElementsList.Add(appUIPanelElementDTO);
                }
            }
            log.LogMethodExit(appUIPanelElementsList);
            return appUIPanelElementsList;
        }

        /// <summary>
        ///  Converts the Data row object to AppUIPanelElementDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object of DataRow.</param>
        /// <returns>Returns the object of AppUIPanelElementDTO</returns>
        private AppUIPanelElementDTO GetAppUIPanelElementDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AppUIPanelElementDTO appUIPanelElementDTO = new AppUIPanelElementDTO(dataRow["UIPanelElementId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIPanelElementId"]),
                                                         dataRow["UIPanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIPanelId"]),
                                                         dataRow["ElementName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ElementName"]),
                                                         dataRow["ActionScreenId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ActionScreenId"]),
                                                         dataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ActiveFlag"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["ElementIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ElementIndex"])
                                                        );
            log.LogMethodExit(appUIPanelElementDTO);
            return appUIPanelElementDTO;
        }

        /// <summary>
        /// Gets the AppUIPanelElement data of passed UIPanelElementId 
        /// </summary>
        /// <param name="uiPanelElementId">uiPanelElementId passed as the parameter</param>
        /// <returns>Returns the object of AppUIPanelElementDTO</returns>
        public AppUIPanelElementDTO GetAppUIPanelElementDTO(int uiPanelElementId)
        {
            log.LogMethodEntry(uiPanelElementId);
            string query = SELECT_QUERY + @"WHERE ape.UIPanelElementId = @UIPanelElementId";
            SqlParameter parameter = new SqlParameter("@UIPanelElementId", uiPanelElementId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            List<AppUIPanelElementDTO> appUIPanelElementsList = GetAppUIPanelElementDTOList(dataTable);
            AppUIPanelElementDTO result = appUIPanelElementsList.FirstOrDefault();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of AppUIPanelElementDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>The List of AppUIPanelElementDTO</returns>
        public List<AppUIPanelElementDTO> GetAppUIPanelElements(List<KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
           
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ELEMENT_ID 
                            || searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ID 
                            || searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.ACTION_SCREEN_ID 
                            || searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ID_LIST ||
                                 searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ELEMENT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.ELEMENT_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppUIPanelElementDTO.SearchByParameters.ACTIVE_FLAG)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
            List<AppUIPanelElementDTO> appUIPanelElementsList = GetAppUIPanelElementDTOList(dataTable);
            log.LogMethodExit(appUIPanelElementsList);
            return appUIPanelElementsList;
        }
       
    }
}

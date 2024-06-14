/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Data Handler -AppUIPanelElementAttributeDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        16-May-2019   Girish Kundar           Created 
 *2.80      02-Jun-2020   Mushahid Faizan         Modified : 3 tier changes for RestAPI.
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
    /// AppUIPanelElementAttribute Data Handler - Handles insert, update and select of  AppUIPanelElementAttribute objects
    /// </summary>
    internal class AppUIPanelElementAttributeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AppUIPanelElementAttribute AS apea "; // Use Alias Name for the Table .
        /// <summary> 
        /// Dictionary for searching Parameters for the AppUIPanelElementAttributeDTO object.
        /// </summary>
        private static readonly Dictionary<AppUIPanelElementAttributeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AppUIPanelElementAttributeDTO.SearchByParameters, string>
        {
            { AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ATTRIBUTE_ID,"apea.UIpanelElementAttributeId"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ATTRIBUTE_ID_LIST,"apea.UIpanelElementAttributeId"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ID,"apea.UIpanelElementId"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ID_LIST,"apea.UIpanelElementId"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.SCREEN_UI_PANEL_ID,"apea.ScreenUIPanelId"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.SCREEN_UI_PANEL_ID_LIST,"apea.ScreenUIPanelId"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.LANGUAGE_ID,"apea.LanguageId"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.DISPLAY_TEXT,"apea.DisplayText"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.ACTIVE_FLAG,"apea.ActiveFlag"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.FILE_NAME,"apea.FileName"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.SITE_ID,"apea.site_id"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.MASTER_ENTITY_ID,"apea.MasterEntityId"},
            { AppUIPanelElementAttributeDTO.SearchByParameters.UI_ELEMENT_ID_LIST,"apea.UIpanelElementId"}

        };
        /// <summary>
        /// Parameterized Constructor for AppUIPanelElementAttributeDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public AppUIPanelElementAttributeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AppUIPanelElementAttribute Record.
        /// </summary>
        /// <param name="appUIPanelElementAttributeDTO">AppUIPanelElementAttributeDTO object is passed as parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelElementAttributeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelElementAttributeId", appUIPanelElementAttributeDTO.UIPanelElementAttributeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIPanelElementId", appUIPanelElementAttributeDTO.UIPanelElementId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayText", appUIPanelElementAttributeDTO.DisplayText));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FileName", appUIPanelElementAttributeDTO.FileName));
            SqlParameter parameter = new SqlParameter("@Image", SqlDbType.VarBinary);
            if (appUIPanelElementAttributeDTO.Image == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = appUIPanelElementAttributeDTO.Image;
            }
            parameters.Add(parameter);
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActionScreenTitle1", appUIPanelElementAttributeDTO.ActionScreenTitle1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActionScreenTitle2", appUIPanelElementAttributeDTO.ActionScreenTitle2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActionScreenFooter1", appUIPanelElementAttributeDTO.ActionScreenFooter1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActionScreenFooter2", appUIPanelElementAttributeDTO.ActionScreenFooter2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", appUIPanelElementAttributeDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LanguageId", appUIPanelElementAttributeDTO.LanguageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScreenUIPanelId", appUIPanelElementAttributeDTO.ScreenUIPanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", appUIPanelElementAttributeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Deletes the AppUIPanelElementAttributeDTO.
        /// </summary>
        /// <param name="appUIPanelElementAttributeDTO">AppUIPanelElementAttributeDTO Object</param>
        internal void Delete(AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO)
        {
            log.LogMethodEntry(appUIPanelElementAttributeDTO);
            string query = @"DELETE  
                             FROM AppUIPanelElementAttribute
                             WHERE AppUIPanelElementAttribute.UIPanelElementAttributeId = @UIPanelElementAttributeId";
            SqlParameter parameter = new SqlParameter("@UIPanelElementAttributeId", appUIPanelElementAttributeDTO.UIPanelElementAttributeId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the record to the AppUIPanelElementAttribute Table.
        /// </summary>
        /// <param name="appUIPanelElementAttributeDTO">AppUIPanelElementAttributeDTO Object</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site Id </param>
        public AppUIPanelElementAttributeDTO Insert(AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelElementAttributeDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[AppUIPanelElementAttribute]
                            (
                                UIPanelElementId,
                                DisplayText,
                                FileName,
                                Image,
                                ActionScreenTitle1,
                                ActionScreenTitle2,
                                ActionScreenFooter1,
                                ActionScreenFooter2,
                                ActiveFlag,
                                LanguageId,
                                ScreenUIPanelId,
                                LastUpdatedBy,
                                LastUpdatedDate,
                                site_id,
                                Guid,
                                MasterEntityId,
                                CreatedBy,
                                CreationDate 
                                )
                            VALUES
                                (
                                @UIPanelElementId,
                                @DisplayText,
                                @FileName,
                                @Image,
                                @ActionScreenTitle1,
                                @ActionScreenTitle2,
                                @ActionScreenFooter1,
                                @ActionScreenFooter2,
                                @ActiveFlag,
                                @LanguageId,
                                @ScreenUIPanelId,
                                @LastUpdatedBy,
                                GETDATE(),
                                @site_id,
                                NEWID(),
                                @MasterEntityId,
                                @CreatedBy,
                                GETDATE() )
                            SELECT * FROM AppUIPanelElementAttribute WHERE UIPanelElementAttributeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIPanelElementAttributeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIPanelElementAttributeDTO(appUIPanelElementAttributeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AppUIPanelElementAttributeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIPanelElementAttributeDTO);
            return appUIPanelElementAttributeDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update , DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="appUIPanelElementAttributeDTO">AppUIPanelElementAttributeDTO object is passed as parameter</param>
        /// <param name="dt">dt is an  object of DataTable to hold the current record</param>
        private void RefreshAppUIPanelElementAttributeDTO(AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO, DataTable dt)
        {
            log.LogMethodEntry(appUIPanelElementAttributeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                appUIPanelElementAttributeDTO.UIPanelElementAttributeId = Convert.ToInt32(dt.Rows[0]["UIPanelElementAttributeId"]);
                appUIPanelElementAttributeDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                appUIPanelElementAttributeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                appUIPanelElementAttributeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                appUIPanelElementAttributeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                appUIPanelElementAttributeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                appUIPanelElementAttributeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the record to the AppUIPanelElementAttribute Table.
        /// </summary>
        /// <param name="appUIPanelElementAttributeDTO">An object of AppUIPanelElementAttributeDTO  </param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>returns the AppUIPanelElementAttributeDTO object </returns>
        public AppUIPanelElementAttributeDTO Update(AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(appUIPanelElementAttributeDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AppUIPanelElementAttribute]
                             SET
                                UIPanelElementId =@UIPanelElementId,
                                DisplayText = @DisplayText,
                                FileName = @FileName,
                                Image = @Image,
                                ActionScreenTitle1 = @ActionScreenTitle1,
                                ActionScreenTitle2 = @ActionScreenTitle2,
                                ActionScreenFooter1 =@ActionScreenFooter1,
                                ActionScreenFooter2 =@ActionScreenFooter2,
                                ActiveFlag = @ActiveFlag ,
                                LanguageId = @LanguageId,
                                ScreenUIPanelId = @ScreenUIPanelId,
                                LastUpdatedBy = @LastUpdatedBy,
                                LastUpdatedDate = GETDATE(),
                                MasterEntityId =  @MasterEntityId
                              where UIPanelElementAttributeId  = @UIPanelElementAttributeId
                            SELECT * FROM AppUIPanelElementAttribute WHERE UIPanelElementAttributeId = @UIPanelElementAttributeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIPanelElementAttributeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIPanelElementAttributeDTO(appUIPanelElementAttributeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AppUIPanelElementAttributeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIPanelElementAttributeDTO);
            return appUIPanelElementAttributeDTO;
        }

        /// <summary>
        /// Returns the  List of AppUIPanelElementAttributeDTO from the DataTable Object. 
        /// </summary>
        /// <param name="dataTable">dataTable object</param>
        /// <returns> List of AppUIPanelElementAttributeDTO </returns>
        private List<AppUIPanelElementAttributeDTO> GetAppUIPanelElementAttributeDTOList(DataTable dataTable)
        {
            log.LogMethodEntry(dataTable);
            List<AppUIPanelElementAttributeDTO> appUIPanelElementAttributeDTOList = new List<AppUIPanelElementAttributeDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO = GetAppUIPanelElementAttributeDTO(dataRow);
                    appUIPanelElementAttributeDTOList.Add(appUIPanelElementAttributeDTO);
                }
            }
            log.LogMethodExit(appUIPanelElementAttributeDTOList);
            return appUIPanelElementAttributeDTOList;
        }

        /// <summary>
        ///  Converts the Data row object to AppUIPanelElementAttributeDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object of DataRow</param>
        /// <returns>returns the AppUIPanelElementAttributeDTO</returns>
        private AppUIPanelElementAttributeDTO GetAppUIPanelElementAttributeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO = new AppUIPanelElementAttributeDTO(dataRow["UIPanelElementAttributeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIPanelElementAttributeId"]),
                                    dataRow["UIPanelElementId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIPanelElementId"]),
                                    dataRow["DisplayText"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DisplayText"]),
                                    dataRow["FileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FileName"]),
                                    dataRow["Image"] == DBNull.Value ? null : dataRow["Image"] as byte[],  // data type var binary , converting to  byte[]
                                    dataRow["ActionScreenTitle1"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ActionScreenTitle1"]),
                                    dataRow["ActionScreenTitle2"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ActionScreenTitle2"]),
                                    dataRow["ActionScreenFooter1"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ActionScreenFooter1"]),
                                    dataRow["ActionScreenFooter2"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ActionScreenFooter2"]),
                                    dataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ActiveFlag"]),
                                    dataRow["LanguageId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LanguageId"]),
                                    dataRow["ScreenUIPanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScreenUIPanelId"]),
                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                    dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(appUIPanelElementAttributeDTO);
            return appUIPanelElementAttributeDTO;
        }

        /// <summary>
        /// Gets the AppUIPanelElementAttribute data of passed uiPanelElementAttributeId 
        /// </summary>
        /// <param name="uiPanelElementAttributeId">uiPanelElementAttributeId is passed as a parameter</param>
        /// <returns>AppUIPanelElementAttributeDTO</returns>
        public AppUIPanelElementAttributeDTO GetAppUIPanelElementAttributeDTO(int uiPanelElementAttributeId)
        {
            log.LogMethodEntry(uiPanelElementAttributeId);
            AppUIPanelElementAttributeDTO result = null;
            string query = SELECT_QUERY + @"WHERE apea.UIPanelElementAttributeId = @UIPanelElementAttributeId";
            SqlParameter parameter = new SqlParameter("@UIPanelElementAttributeId", uiPanelElementAttributeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAppUIPanelElementAttributeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of AppUIPanelElementAttributeDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>returns the List of AppUIPanelElementAttributeDTO</returns>
        public List<AppUIPanelElementAttributeDTO> GetAppUIPanelElementAttributes(List<KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AppUIPanelElementAttributeDTO> appUIPanelElementAttributeDTOList = new List<AppUIPanelElementAttributeDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ATTRIBUTE_ID ||
                            searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ID ||
                            searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.SCREEN_UI_PANEL_ID ||
                            searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.LANGUAGE_ID ||
                            searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.UI_ELEMENT_ID_LIST ||
                            searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.SCREEN_UI_PANEL_ID_LIST ||
                            searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ID_LIST ||
                                 searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ATTRIBUTE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }

                        else if (searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIPanelElementAttributeDTO.SearchByParameters.ACTIVE_FLAG) // bit
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
                    AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO = GetAppUIPanelElementAttributeDTO(dataRow);
                    appUIPanelElementAttributeDTOList.Add(appUIPanelElementAttributeDTO);
                }
            }
            log.LogMethodExit(appUIPanelElementAttributeDTOList);
            return appUIPanelElementAttributeDTOList;
        }

    }
}

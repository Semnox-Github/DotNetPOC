/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - Data object of AppUIElementParameterAttribute
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70        15-May-2019   Girish Kundar           Created 
*2.80        04-Jun-2020   Mushahid Faizan          Modified : 3 Tier Changes for Rest API
*2.90        24-Aug-2020   Girish Kundar            Modified : Issue Fix Child entity delete 
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
    /// AppUIElementParameterAttributeDataHandler Data Handler - Handles insert, update and select of  AppUIElementParameterAttribute objects
    /// </summary>
    public class AppUIElementParameterAttributeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AppUIElementParameterAttribute ";
        /// <summary>
        /// Dictionary for searching Parameters for the AppUIElementParameterAttribute object.
        /// </summary>
        private static readonly Dictionary<AppUIElementParameterAttributeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AppUIElementParameterAttributeDTO.SearchByParameters, string>
        {
            {AppUIElementParameterAttributeDTO.SearchByParameters.UI_PARAMETER_ATTRIBUTE_ID,"UIParameterAttributeId"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.PARAMETER_ID,"ParameterId"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.PARAMETER_ID_LIST,"ParameterId"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.LANGUAGE_ID,"LanguageId"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.FILE_NAME,"FileName"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.DISPLAY_TEXT_1,"DisplayText1"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.DISPLAY_TEXT_2,"DisplayText2"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.SITE_ID,"site_id"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.ACTIVE_FLAG,"ActiveFlag"},
            {AppUIElementParameterAttributeDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for AppUIElementParameterAttributeDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public AppUIElementParameterAttributeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AppUIElementParameterAttribute Record.
        /// </summary>
        /// <param name="appUIElementParameterAttributeDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appUIElementParameterAttributeDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@UIParameterAttributeId", appUIElementParameterAttributeDTO.UIParameterAttributeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParameterId", appUIElementParameterAttributeDTO.ParameterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LanguageId", appUIElementParameterAttributeDTO.LanguageId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayImage", appUIElementParameterAttributeDTO.DisplayImage));
            SqlParameter parameter = new SqlParameter("@DisplayImage", SqlDbType.VarBinary);
            if (appUIElementParameterAttributeDTO.DisplayImage == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = appUIElementParameterAttributeDTO.DisplayImage;
            }
            parameters.Add(parameter);
            parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayText1", appUIElementParameterAttributeDTO.DisplayText1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayText2", appUIElementParameterAttributeDTO.DisplayText2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FileName", appUIElementParameterAttributeDTO.FileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", appUIElementParameterAttributeDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", appUIElementParameterAttributeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to GetAppScreenUIPanelDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>AppScreenProfileDTO</returns>
        private AppUIElementParameterAttributeDTO GetAppUIElementParameterAttributeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO = new AppUIElementParameterAttributeDTO(
                                                         dataRow["UIParameterAttributeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UIParameterAttributeId"]),
                                                         dataRow["ParameterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParameterId"]),
                                                         dataRow["DisplayText1"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DisplayText1"]),
                                                         dataRow["DisplayText2"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DisplayText2"]),
                                                         dataRow["LanguageId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LanguageId"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["DisplayImage"] == DBNull.Value ? null : dataRow["DisplayImage"] as byte[],
                                                         dataRow["FileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FileName"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ActiveFlag"])
                                                        );
            log.LogMethodExit(appUIElementParameterAttributeDTO);
            return appUIElementParameterAttributeDTO;
        }
        /// <summary>
        /// Gets the AppUIElementParameterAttribute data of passed UIParameterAttributeId 
        /// </summary>
        /// <param name="uiParameterAttributeId"></param>
        /// <returns></returns>
        public AppUIElementParameterAttributeDTO GetAppUIElementParameterAttribute(int uiParameterAttributeId)
        {
            log.LogMethodEntry(uiParameterAttributeId);
            AppUIElementParameterAttributeDTO result = null;
            string query = SELECT_QUERY + @" WHERE AppUIElementParameterAttribute.UIParameterAttributeId = @UIParameterAttributeId";
            SqlParameter parameter = new SqlParameter("@UIParameterAttributeId", uiParameterAttributeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAppUIElementParameterAttributeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///  Deletes the AppUIElementParameterAttributeDTO record based on Id.
        /// </summary>
        /// <param name="appUIElementParameterAttributeDTO"></param>
        internal void Delete(AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO)
        {
            log.LogMethodEntry(appUIElementParameterAttributeDTO);
            string query = @"DELETE  
                             FROM AppUIElementParameterAttribute
                             WHERE AppUIElementParameterAttribute.UIParameterAttributeId = @UIParameterAttributeId";
            SqlParameter parameter = new SqlParameter("@UIParameterAttributeId", appUIElementParameterAttributeDTO.UIParameterAttributeId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the record to the AppUIElementParameterAttribute table
        /// </summary>
        /// <param name="appUIElementParameterAttributeDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>

        public AppUIElementParameterAttributeDTO Insert(AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appUIElementParameterAttributeDTO, userId, siteId);
            string query = @"INSERT INTO [dbo].[AppUIElementParameterAttribute]
                           (ParameterId
                           ,DisplayText1
                           ,DisplayText2
                           ,LanguageId
                           ,MasterEntityId
                           ,LastUpdatedBy
                           ,LastUpdatedDate
                           ,site_id
                           ,Guid
                           ,DisplayImage
                           ,FileName
                           ,CreatedBy
                           ,CreationDate)
                     VALUES
                           (@ParameterId
                           ,@DisplayText1
                           ,@DisplayText2
                           ,@LanguageId
                           ,@MasterEntityId
                           ,@LastUpdatedBy
                           ,GETDATE()
                           ,@site_id
                           ,NEWID()
                           ,@DisplayImage
                           ,@FileName
                           ,@CreatedBy
                           ,GETDATE())  SELECT * FROM AppUIElementParameterAttribute WHERE UIParameterAttributeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIElementParameterAttributeDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIElementParameterAttributeDTO(appUIElementParameterAttributeDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AppUIElementParameterAttributeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIElementParameterAttributeDTO);
            return appUIElementParameterAttributeDTO;
        }

        /// <summary>
        /// Updates the record to the AppUIElementParameterAttribute table
        /// </summary>
        /// <param name="appUIElementParameterAttributeDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>

        public AppUIElementParameterAttributeDTO Update(AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO, string userId, int siteId)
        {
            log.LogMethodEntry(appUIElementParameterAttributeDTO, userId, siteId);
            string query = @"UPDATE [dbo].[AppUIElementParameterAttribute]
                           SET
                            ParameterId =@ParameterId
                           ,DisplayText1 = @DisplayText1
                           ,DisplayText2 = @DisplayText2
                           ,LanguageId =@LanguageId
                           ,MasterEntityId = @MasterEntityId
                           ,LastUpdatedBy = @LastUpdatedBy
                           ,LastUpdatedDate = GETDATE()
                           ,DisplayImage = @DisplayImage
                            Where UIParameterAttributeId = @UIParameterAttributeId
                           SELECT * FROM AppUIElementParameterAttribute WHERE UIParameterAttributeId = @UIParameterAttributeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(appUIElementParameterAttributeDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAppUIElementParameterAttributeDTO(appUIElementParameterAttributeDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating AppUIElementParameterAttributeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(appUIElementParameterAttributeDTO);
            return appUIElementParameterAttributeDTO;
        }

        private void RefreshAppUIElementParameterAttributeDTO(AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO, DataTable dt, string userId, int siteId)
        {
            log.LogMethodEntry(appUIElementParameterAttributeDTO, dt, userId, siteId);
            if (dt.Rows.Count > 0)
            {
                appUIElementParameterAttributeDTO.UIParameterAttributeId = Convert.ToInt32(dt.Rows[0]["UIParameterAttributeId"]);
                appUIElementParameterAttributeDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastupdatedDate"]);
                appUIElementParameterAttributeDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                appUIElementParameterAttributeDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                appUIElementParameterAttributeDTO.LastUpdatedBy = userId;
                appUIElementParameterAttributeDTO.CreatedBy = userId;
                appUIElementParameterAttributeDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AppUIElementParameterAttributeDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<AppUIElementParameterAttributeDTO> GetAllAppUIElementParameterAttribute(List<KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AppUIElementParameterAttributeDTO> appUIElementParameterAttributeDTOList = new List<AppUIElementParameterAttributeDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.UI_PARAMETER_ATTRIBUTE_ID
                            || searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.PARAMETER_ID
                            || searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.LANGUAGE_ID
                            || searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.FILE_NAME
                            || searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.DISPLAY_TEXT_1
                            || searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.DISPLAY_TEXT_2)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.PARAMETER_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AppUIElementParameterAttributeDTO.SearchByParameters.ACTIVE_FLAG)  // bit
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
                    AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO = GetAppUIElementParameterAttributeDTO(dataRow);
                    appUIElementParameterAttributeDTOList.Add(appUIElementParameterAttributeDTO);
                }
            }
            log.LogMethodExit(appUIElementParameterAttributeDTOList);
            return appUIElementParameterAttributeDTOList;
        }
    }
}

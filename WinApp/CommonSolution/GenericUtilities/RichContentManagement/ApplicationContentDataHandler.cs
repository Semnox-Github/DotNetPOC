/********************************************************************************************
 * Project Name - ApplicationContent Data Handler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Jeevan            Created 
 *2.4.0       25-Nov-2018      Raghuveera        Children loading modified
 *2.60.0      03-May-2019      Divya             SQL Injection
 *2.70.2        25-Jul-2019      Dakshakh Raj      Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix and
 *                                                          Added IsActive to insert/update method
 *2.70.2        06-Dec-2019      Jinto Thomas     Removed siteid from update query                                                           
 *2.90.0        06-Jun-2020      Girish Kundar    Modified: Rest API - phase 2 changes  
 *2.130.0     21-Jul-2021       Mushahid Faizan     Modified : POS UI Redesign changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ApplicationContent as ac ";

        /// <summary>
        /// Dictionary for searching Parameters for the ApplicationContent object.
        /// </summary>
        private static readonly Dictionary<ApplicationContentDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ApplicationContentDTO.SearchByParameters, string>
            {
                {ApplicationContentDTO.SearchByParameters.APP_CONTENT_ID, "ac.APPCONTENTID"},
                {ApplicationContentDTO.SearchByParameters.APPLICATION, "ac.APPLICATION"},
                {ApplicationContentDTO.SearchByParameters.MODULE, "ac.MODULE"},
                {ApplicationContentDTO.SearchByParameters.CHAPTER, "ac.CHAPTER"},
                {ApplicationContentDTO.SearchByParameters.CONTENT_ID, "ac.CONTENTID"},
                {ApplicationContentDTO.SearchByParameters.SITE_ID, "ac.SITE_ID"},
                {ApplicationContentDTO.SearchByParameters.MASTER_ENTITY_ID, "ac.MasterEntityId"},
                {ApplicationContentDTO.SearchByParameters.ACTIVE_FLAG, "ac.IsActive"}
             };

        private readonly SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of ApplicationContentDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ApplicationContentDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating ApplicationContent Reecord.
        /// </summary>
        /// <param name="applicationContentDTO">applicationContentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ApplicationContentDTO applicationContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationContentDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@appContentId", applicationContentDTO.AppContentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@application", applicationContentDTO.Application));
            parameters.Add(dataAccessHandler.GetSQLParameter("@module", applicationContentDTO.Module));
            parameters.Add(dataAccessHandler.GetSQLParameter("@chapter", applicationContentDTO.Chapter));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentId", applicationContentDTO.ContentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", applicationContentDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (applicationContentDTO.IsActive == true ? "Y" : "N")));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ApplicationContent record to the database
        /// </summary>
        /// <param name="applicationContentDTO">applicationContentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        ///<returns>Returns inserted record id</returns>
        public ApplicationContentDTO InsertApplicationContent(ApplicationContentDTO applicationContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationContentDTO, loginId, siteId);
            string insertApplicationContentQuery = @"INSERT INTO[dbo].[ApplicationContent]
                                                            ( 
                                                            Application,
                                                            Module, 
                                                            Chapter,
                                                            ContentId,
                                                            IsActive,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdatedDate,
                                                            site_id,
                                                            Guid,
                                                            MasterEntityId
                                                         )
                                                       values
                                                         ( 
                                                            @application,
                                                            @module,
                                                            @chapter,
                                                            @contentId,
                                                            @isActive,
                                                            @createdBy,
                                                            GETDATE(),
                                                            @lastUpdatedBy,
                                                            GETDATE(),
                                                            @siteId,
                                                            NewID(),
                                                            @masterEntityId
                                                          )SELECT * FROM ApplicationContent WHERE AppContentId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertApplicationContentQuery, GetSQLParameters(applicationContentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApplicationContentDTO(applicationContentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ApplicationContentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationContentDTO);
            return applicationContentDTO;
        }

        /// <summary>
        /// Updates the applicationContent record to the database
        /// </summary>
        /// <param name="applicationContentDTO">applicationContentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">Returns # of rows updated</param>
        /// <returns></returns>
        public ApplicationContentDTO UpdateApplicationContent(ApplicationContentDTO applicationContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationContentDTO, loginId, siteId);
            string updateApplicationContentDTOQuery = @"update ApplicationContent
                                                         set
															Application = @application,
                                                            Module = @module,
                                                            Chapter = @chapter,
                                                            ContentId = @contentId,
                                                            IsActive = @isActive,
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdatedDate = GETDATE(),
                                                            -- site_id = @siteId,
                                                            MasterEntityId = @masterEntityId
                                                          where 
                                                            AppContentId = @appContentId 
                                                          SELECT* FROM ApplicationContent WHERE  AppContentId = @appContentId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateApplicationContentDTOQuery, GetSQLParameters(applicationContentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApplicationContentDTO(applicationContentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ApplicationContentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationContentDTO);
            return applicationContentDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="applicationContentDTO">applicationContentDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshApplicationContentDTO(ApplicationContentDTO applicationContentDTO, DataTable dt)
        {
            log.LogMethodEntry(applicationContentDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                applicationContentDTO.AppContentId = Convert.ToInt32(dt.Rows[0]["AppContentId"]);
                applicationContentDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                applicationContentDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                applicationContentDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                applicationContentDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                applicationContentDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                applicationContentDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to ApplicationContentDTO class type
        /// </summary>
        /// <param name="ApplicationContentDTODataRow">ApplicationContentDTODataRow</param>
        /// <returns>Returns ApplicationContentDTO</returns>
        private ApplicationContentDTO GetApplicationContent(DataRow ApplicationContentDataRow)
        {
            log.LogMethodEntry(ApplicationContentDataRow);
            ApplicationContentDTO applicationContentDTO = new ApplicationContentDTO(
                                 ApplicationContentDataRow["AppContentId"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentDataRow["AppContentId"]),
                                 ApplicationContentDataRow["Application"] == DBNull.Value ? string.Empty : ApplicationContentDataRow["Application"].ToString(),
                                 ApplicationContentDataRow["Module"] == DBNull.Value ? string.Empty : ApplicationContentDataRow["Module"].ToString(),
                                 ApplicationContentDataRow["Chapter"] == DBNull.Value ? string.Empty : ApplicationContentDataRow["Chapter"].ToString(),
                                 ApplicationContentDataRow["ContentId"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentDataRow["ContentId"]),
                                 ApplicationContentDataRow["IsActive"] == DBNull.Value ? false : (ApplicationContentDataRow["IsActive"].ToString() == "Y" ? true : false),
                                 ApplicationContentDataRow["CreatedBy"] == DBNull.Value ? string.Empty : ApplicationContentDataRow["CreatedBy"].ToString(),
                                 ApplicationContentDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ApplicationContentDataRow["CreationDate"]),
                                 ApplicationContentDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : ApplicationContentDataRow["LastUpdatedBy"].ToString(),
                                 ApplicationContentDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ApplicationContentDataRow["LastUpdatedDate"]),
                                 ApplicationContentDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentDataRow["Site_id"]),
                                 ApplicationContentDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(ApplicationContentDataRow["SynchStatus"]),
                                 ApplicationContentDataRow["Guid"].ToString(),
                                 ApplicationContentDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentDataRow["MasterEntityId"])
                                 );
            log.LogMethodExit(applicationContentDTO);
            return applicationContentDTO;
        }

        /// <summary>
        /// Gets the ApplicationContentsDTO data of passed appContentId
        /// </summary>
        /// <param name="appContentId">appContentId</param>
        /// <returns>Returns ApplicationContentsDTO</returns>
        public ApplicationContentDTO GetApplicationContent(int appContentId)
        {
            log.LogMethodEntry(appContentId);
            ApplicationContentDTO applicationContentDTO = null;
            string selectApplicationContentDTOQuery = SELECT_QUERY + @" WHERE ac.appContentId = @appContentId";
            SqlParameter parameter = new SqlParameter("@appContentId", appContentId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectApplicationContentDTOQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                applicationContentDTO = GetApplicationContent(dataTable.Rows[0]);
            }
            log.LogMethodExit(applicationContentDTO);
            return applicationContentDTO;
        }


        /// <summary>
        /// Gets the ApplicationContentsDTO data of passed applicationValue
        /// </summary>
        /// <param name="applicationValue"></param>
        /// <returns>Returns ApplicationContentsDTO</returns>
        public ApplicationContentDTO GetApplicationContent(string applicationValue)
        {
            log.LogMethodEntry(applicationValue);
            ApplicationContentDTO applicationContentDTO = null;
            string selectApplicationContentDTOQuery = SELECT_QUERY + @" WHERE ac.application = @applicationValue";
            SqlParameter[] selectApplicationContentDTOParameters = new SqlParameter[1];
            selectApplicationContentDTOParameters[0] = new SqlParameter("@applicationValue", applicationValue);
            DataTable selectedApplicationContent = dataAccessHandler.executeSelectQuery(selectApplicationContentDTOQuery, selectApplicationContentDTOParameters, sqlTransaction);
            if (selectedApplicationContent.Rows.Count > 0)
            {
                DataRow ApplicationContentRow = selectedApplicationContent.Rows[0];
                applicationContentDTO = GetApplicationContent(ApplicationContentRow);
            }
            log.LogMethodExit();
            return applicationContentDTO;
        }

        /// <summary>
        /// Gets the ApplicationContentDTO list matching the search key.
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildren">loadChildren</param>
        /// <returns>Returns the list of ApplicationContentsDTO matching the search criteria</returns>
        public List<ApplicationContentDTO> GetApplicationContentDTOList(List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ApplicationContentDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ApplicationContentDTO.SearchByParameters.APP_CONTENT_ID
                            || searchParameter.Key == ApplicationContentDTO.SearchByParameters.CONTENT_ID
                            || searchParameter.Key == ApplicationContentDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationContentDTO.SearchByParameters.APPLICATION
                                 || searchParameter.Key == ApplicationContentDTO.SearchByParameters.MODULE
                                 || searchParameter.Key == ApplicationContentDTO.SearchByParameters.CHAPTER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ApplicationContentDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationContentDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
            List<ApplicationContentDTO> applicationContentDTOList = new List<ApplicationContentDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ApplicationContentDTO applicationContentDTO = GetApplicationContent(dataRow);
                    applicationContentDTOList.Add(applicationContentDTO);
                }
            }
            log.LogMethodExit(applicationContentDTOList);
            return applicationContentDTOList;
        }

        /// <summary>
        /// Deletes the ApplicationContent record
        /// </summary>
        /// <param name="applicationContentDTO"></param>
        public void Delete(ApplicationContentDTO applicationContentDTO)
        {
            log.LogMethodEntry(applicationContentDTO);
            string query = @"DELETE  
                             FROM ApplicationContent
                             WHERE AppContentId = @appContentId";
            SqlParameter parameter = new SqlParameter("@appContentId", applicationContentDTO.AppContentId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        internal DateTime? GetApplicationContentLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from ApplicationContent WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from ApplicationContentTranslated WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from RichContent WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

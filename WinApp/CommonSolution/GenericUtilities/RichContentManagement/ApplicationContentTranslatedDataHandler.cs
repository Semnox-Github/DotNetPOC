/********************************************************************************************
 * Project Name - ApplicationContent Data Handler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019       Dakshakh         Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix and
 *                                                          Added IsActive to insert/update method
 *2.70.2        06-Dec-2019     Jinto Thomas       Removed siteid from update query     
 *2.90       21-May-2020       Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 *2.130.0     21-Jul-2021       Mushahid Faizan     Modified : POS UI Redesign changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Data;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentTranslatedDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction = null;
        DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ApplicationContentTranslated as act ";

        /// <summary>
        /// Dictionary for searching Parameters for the ApplicationContentTranslated object.
        /// </summary>
        private static readonly Dictionary<ApplicationContentTranslatedDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ApplicationContentTranslatedDTO.SearchByParameters, string>
            {
                {ApplicationContentTranslatedDTO.SearchByParameters.ID, "act.ID"},
                {ApplicationContentTranslatedDTO.SearchByParameters.APP_CONTENT_ID, "act.APPCONTENTID"},
                {ApplicationContentTranslatedDTO.SearchByParameters.APP_CONTENT_ID_LIST, "act.APPCONTENTID"},
                {ApplicationContentTranslatedDTO.SearchByParameters.LANGUAGE_ID, "act.LANGUAGEID"},
                {ApplicationContentTranslatedDTO.SearchByParameters.CHAPTER, "act.CHAPTER"},
                {ApplicationContentTranslatedDTO.SearchByParameters.SITE_ID, "act.SITE_ID"},
                {ApplicationContentTranslatedDTO.SearchByParameters.MASTER_ENTITY_ID, "act.MasterEntityId"},
                {ApplicationContentTranslatedDTO.SearchByParameters.ACTIVE_FLAG, "act.IsActive"}

             };

        /// <summary>
        /// Default constructor of ApplicationContentHandler class
        /// </summary>
        public ApplicationContentTranslatedDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of ApplicationContentTranslatedDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ApplicationContentTranslatedDataHandler(SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating applicationContentTranslated Reecord.
        /// </summary>
        /// <param name="applicationContentTranslatedDTO">applicationContentTranslatedDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ApplicationContentTranslatedDTO applicationContentTranslatedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationContentTranslatedDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", applicationContentTranslatedDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@appContentId", applicationContentTranslatedDTO.AppContentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@languageId", applicationContentTranslatedDTO.LanguageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@chapter", applicationContentTranslatedDTO.Chapter));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentId", applicationContentTranslatedDTO.ContentId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (applicationContentTranslatedDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", applicationContentTranslatedDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ApplicationContentTranslated record to the database
        /// </summary>
        /// <param name="applicationContentTranslatedDTO"></param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        ///<returns>Returns inserted record id</returns>
        public ApplicationContentTranslatedDTO InsertApplicationContent(ApplicationContentTranslatedDTO applicationContentTranslatedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationContentTranslatedDTO, loginId, siteId);
            string insertApplicationContentTranslatedQuery = @"INSERT INTO[dbo].[ApplicationContentTranslated]
                                                            ( 
															AppContentId,
                                                            LanguageId,
                                                            Chapter,
                                                            ContentId,
                                                            IsActive,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdateDate,
                                                            site_id,
                                                            Guid,
                                                            MasterEntityId
                                                         )
                                                       values
                                                         ( 
															@appContentId,
                                                            @languageId,
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
                                                          )SELECT * FROM ApplicationContentTranslated WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertApplicationContentTranslatedQuery, GetSQLParameters(applicationContentTranslatedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApplicationContentTranslatedDTO(applicationContentTranslatedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ApplicationContentTranslatedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationContentTranslatedDTO);
            return applicationContentTranslatedDTO;
        }



        /// <summary>
        /// Updates the ApplicationContentTranslatedDTO record to the database
        /// </summary>
        /// <param name="applicationContentTranslatedDTO"></param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">Returns # of rows updated</param>
        /// <returns></returns>
        public ApplicationContentTranslatedDTO UpdateApplicationContent(ApplicationContentTranslatedDTO applicationContentTranslatedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationContentTranslatedDTO, loginId, siteId);
            string updateApplicationContentTranslatedDTOQuery = @"update ApplicationContentTranslated
                                                         set
															AppContentId= @appContentId,
                                                            LanguageId = @languageId,
                                                            Chapter = @chapter,
                                                            ContentId = @contentId,
                                                            IsActive = @isActive,
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdateDate = GETDATE(),
                                                           -- site_id = @siteId,
                                                            MasterEntityId = @masterEntityId
                                                          where 
                                                            Id = @id
                                                          SELECT* FROM ApplicationContentTranslated WHERE  Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateApplicationContentTranslatedDTOQuery, GetSQLParameters(applicationContentTranslatedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApplicationContentTranslatedDTO(applicationContentTranslatedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ApplicationContentTranslatedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationContentTranslatedDTO);
            return applicationContentTranslatedDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="applicationContentTranslatedDTO">applicationContentTranslatedDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshApplicationContentTranslatedDTO(ApplicationContentTranslatedDTO applicationContentTranslatedDTO, DataTable dt)
        {
            log.LogMethodEntry(applicationContentTranslatedDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                applicationContentTranslatedDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                applicationContentTranslatedDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                applicationContentTranslatedDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                applicationContentTranslatedDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                applicationContentTranslatedDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                applicationContentTranslatedDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                applicationContentTranslatedDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to ApplicationContentTranslatedDataRow class type
        /// </summary>
        /// <param name="ApplicationContentTranslatedDataRow">ApplicationContentTranslatedDataRow</param>
        /// <returns>Returns ApplicationContentDTO</returns>
        private ApplicationContentTranslatedDTO GetApplicationContentTranslated(DataRow ApplicationContentTranslatedDataRow)
        {
            log.LogMethodEntry(ApplicationContentTranslatedDataRow);
            ApplicationContentTranslatedDTO applicationContentTranslatedDTO = new ApplicationContentTranslatedDTO(
                                 ApplicationContentTranslatedDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentTranslatedDataRow["Id"]),
                                 ApplicationContentTranslatedDataRow["AppContentId"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentTranslatedDataRow["AppContentId"]),
                                 ApplicationContentTranslatedDataRow["LanguageId"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentTranslatedDataRow["LanguageId"]),
                                 ApplicationContentTranslatedDataRow["Chapter"] == DBNull.Value ? string.Empty : ApplicationContentTranslatedDataRow["Chapter"].ToString(),
                                 ApplicationContentTranslatedDataRow["ContentId"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentTranslatedDataRow["ContentId"]),
                                 ApplicationContentTranslatedDataRow["IsActive"] == DBNull.Value ? false : (ApplicationContentTranslatedDataRow["IsActive"].ToString() == "Y" ? true : false),
                                 ApplicationContentTranslatedDataRow["CreatedBy"] == DBNull.Value ? string.Empty : ApplicationContentTranslatedDataRow["CreatedBy"].ToString(),
                                 ApplicationContentTranslatedDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ApplicationContentTranslatedDataRow["CreationDate"]),
                                 ApplicationContentTranslatedDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : ApplicationContentTranslatedDataRow["LastUpdatedBy"].ToString(),
                                 ApplicationContentTranslatedDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ApplicationContentTranslatedDataRow["LastUpdateDate"]),
                                 ApplicationContentTranslatedDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentTranslatedDataRow["Site_id"]),
                                 ApplicationContentTranslatedDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(ApplicationContentTranslatedDataRow["SynchStatus"]),
                                 ApplicationContentTranslatedDataRow["Guid"].ToString(),
                                 ApplicationContentTranslatedDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(ApplicationContentTranslatedDataRow["MasterEntityId"])
                                 );
            log.LogMethodExit(applicationContentTranslatedDTO);
            return applicationContentTranslatedDTO;
        }

        /// <summary>
        /// Gets the ApplicationContentTranslatedDTO data of passed id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Returns ApplicationContentTranslatedDTO</returns>
        public ApplicationContentTranslatedDTO GetApplicationContentTranslated(int id)
        {
            log.LogMethodEntry(id);
            ApplicationContentTranslatedDTO applicationContentTranslatedDTO = null;
            string selectApplicationContentTranslatedDTOQuery = SELECT_QUERY + @" WHERE act.id = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectApplicationContentTranslatedDTOQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                applicationContentTranslatedDTO = GetApplicationContentTranslated(dataTable.Rows[0]);
            }
            log.LogMethodExit(applicationContentTranslatedDTO);
            return applicationContentTranslatedDTO;
        }

        /// <summary>
        /// Gets the ApplicationContentTranslatedDTO data of passed chapter
        /// </summary>
        /// <param name="chapter">chapter</param>
        /// <returns>Returns ApplicationContentTranslatedDTO</returns>
        public ApplicationContentTranslatedDTO GetApplicationContentTranslated(string chapter)
        {
            log.LogMethodEntry(chapter);
            ApplicationContentTranslatedDTO applicationContentTranslatedDTO = null;
            string selectApplicationContentTranslatedDTOQuery = SELECT_QUERY + @" WHERE act.chapter = @chapter";
            SqlParameter[] selectApplicationContentDTOParameters = new SqlParameter[1];
            selectApplicationContentDTOParameters[0] = new SqlParameter("@chapter", chapter);
            DataTable selectedApplicationContentTranslated = dataAccessHandler.executeSelectQuery(selectApplicationContentTranslatedDTOQuery, selectApplicationContentDTOParameters, sqlTransaction);
            if (selectedApplicationContentTranslated.Rows.Count > 0)
            {
                DataRow ApplicationContentTranslatedRow = selectedApplicationContentTranslated.Rows[0];
                applicationContentTranslatedDTO = GetApplicationContentTranslated(ApplicationContentTranslatedRow);
            }

            log.LogMethodExit(applicationContentTranslatedDTO);
            return applicationContentTranslatedDTO;
        }

        /// <summary>
        /// Gets the ApplicationContentTranslatedDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>Returns the list of ApplicationContentTranslatedDTO matching the search criteria</returns>
        public List<ApplicationContentTranslatedDTO> GetApplicationContentTranslatedDTOList(List<KeyValuePair<ApplicationContentTranslatedDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ApplicationContentTranslatedDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ApplicationContentTranslatedDTO.SearchByParameters.ID
                            || searchParameter.Key == ApplicationContentTranslatedDTO.SearchByParameters.LANGUAGE_ID
                            || searchParameter.Key == ApplicationContentTranslatedDTO.SearchByParameters.APP_CONTENT_ID
                            || searchParameter.Key == ApplicationContentTranslatedDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationContentTranslatedDTO.SearchByParameters.CHAPTER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ApplicationContentTranslatedDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationContentTranslatedDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == ApplicationContentTranslatedDTO.SearchByParameters.APP_CONTENT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
            List<ApplicationContentTranslatedDTO> applicationContentTranslatedDTOList = new List<ApplicationContentTranslatedDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ApplicationContentTranslatedDTO applicationContentTranslatedDTO = GetApplicationContentTranslated(dataRow);
                    applicationContentTranslatedDTOList.Add(applicationContentTranslatedDTO);
                }
            }
            log.LogMethodExit(applicationContentTranslatedDTOList);
            return applicationContentTranslatedDTOList;
        }

        /// <summary>
        /// Deletes the ApplicationContentTranslated record
        /// </summary>
        /// <param name="applicationContentTranslatedDTO"></param>
        public void Delete(ApplicationContentTranslatedDTO applicationContentTranslatedDTO)
        {
            log.LogMethodEntry(applicationContentTranslatedDTO);
            string query = @"DELETE  
                             FROM ApplicationContentTranslated
                             WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", applicationContentTranslatedDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter });
            log.LogMethodExit();
        }

    }

}


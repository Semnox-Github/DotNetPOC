/********************************************************************************************
 * Project Name - TranslatedMessage Data Handler
 * Description  - DataHandler of the TranslatedMessage
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Mushahid Faizan     Created
 *2.70.2      30-Jul -2019  Girish Kundar      Modified : Fix for SQL injection and Changed Insert and Update method.
 *2.140.0     30-Nov -2021  Abhishek           Modified : Added site id in the insert query
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace Semnox.Parafait.Languages
{
    public class TranslatedMessageDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MessagesTranslated AS mt, languages l where l.languageId = mt.LanguageId";
        private static readonly Dictionary<TranslatedMessageDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TranslatedMessageDTO.SearchByParameters, string>
            {
                {TranslatedMessageDTO.SearchByParameters.ID, "mt.Id"},
                {TranslatedMessageDTO.SearchByParameters.MESSAGE_ID, "mt.MessageId"},
                {TranslatedMessageDTO.SearchByParameters.MESSAGE_ID_LIST, "mt.MessageId"},
                {TranslatedMessageDTO.SearchByParameters.LANGUAGEID, "mt.LanguageId"},
                {TranslatedMessageDTO.SearchByParameters.MASTER_ENTITY_ID, "mt.MasterEntityId"},
                {TranslatedMessageDTO.SearchByParameters.SITE_ID, "mt.site_id"},
                {TranslatedMessageDTO.SearchByParameters.ACTIVE_LANGUAGES, "l.Active"}

            };

        /// <summary>
        /// Default constructor of TranslatedMessageDataHandler class
        /// </summary>
        public TranslatedMessageDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to TranslatedMessageDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow </param>
        /// <returns>Returns translatedMessageDTO</returns>
        public TranslatedMessageDTO GetTranslatedMessageDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TranslatedMessageDTO translatedMessageDTO = new TranslatedMessageDTO(
                                                    dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                    dataRow["MessageId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MessageId"]),
                                                    dataRow["Message"].ToString(),
                                                    dataRow["LanguageId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LanguageId"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                        );
            log.LogMethodExit(translatedMessageDTO);
            return translatedMessageDTO;
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Translated Messages Record.
        /// </summary>
        /// <param name="TranslatedMessageDTO">TranslatedMessageDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(TranslatedMessageDTO translatedMessageDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(translatedMessageDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", translatedMessageDTO.ID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageId", translatedMessageDTO.MessageId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Message", translatedMessageDTO.Message));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LanguageId", translatedMessageDTO.LanguageId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", translatedMessageDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", translatedMessageDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", translatedMessageDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", translatedMessageDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the translatedMessageDTO record to the database
        /// </summary>
        /// <param name="translatedMessageDTO">translatedMessageDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns TranslatedMessageDTO</returns>
        public TranslatedMessageDTO InsertTranslatedMessages(TranslatedMessageDTO translatedMessageDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(translatedMessageDTO, loginId, siteId);
            string query = @"INSERT INTO MessagesTranslated 
                                        ( 
                                            MessageId,
                                            Message,
                                            LanguageId,
                                            site_id,
                                            Guid,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            IsActive
                                        ) 
                                VALUES 
                                        (
                                            @MessageId,
                                            @Message,
                                            @LanguageId,
                                            @SiteId,
                                            NEWID(),
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @isActive
                                        ) SELECT * FROM MessagesTranslated WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(translatedMessageDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTranslatedMessageDTO(translatedMessageDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting translatedMessageDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(translatedMessageDTO);
            return translatedMessageDTO;
        }

        /// <summary>
        /// Updates the translatedMessageDTO record
        /// </summary>
        /// <param name="translatedMessageDTO">translatedMessageDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns TranslatedMessageDTO</returns>
        public TranslatedMessageDTO UpdateTranslatedMessages(TranslatedMessageDTO translatedMessageDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(translatedMessageDTO, loginId, siteId);
            string query = @"UPDATE [MessagesTranslated] SET
                            [MessageId] = @MessageId,
                            [Message] = @Message,
                            [LanguageId] = @LanguageId,
                            LastUpdatedBy= @LastUpdatedBy,
                            LastUpdateDate = GetDate(),
                            [IsActive] = @isActive
                            WHERE (Id = @Id) 
                     SELECT * FROM MessagesTranslated WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(translatedMessageDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTranslatedMessageDTO(translatedMessageDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating translatedMessageDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(translatedMessageDTO);
            return translatedMessageDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="translatedMessageDTO">TranslatedMessageDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshTranslatedMessageDTO(TranslatedMessageDTO translatedMessageDTO, DataTable dt)
        {
            log.LogMethodEntry(translatedMessageDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                translatedMessageDTO.ID = Convert.ToInt32(dt.Rows[0]["Id"]);
                translatedMessageDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                translatedMessageDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                translatedMessageDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                translatedMessageDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                translatedMessageDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                translatedMessageDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the translatedMessageDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of translatedMessageDTO matching the search criteria</returns>
        public List<TranslatedMessageDTO> GetAllTranslatedMessages(List<KeyValuePair<TranslatedMessageDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectLanguagesQuery = SELECT_QUERY;

            string joiner = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" and ");
                foreach (KeyValuePair<TranslatedMessageDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(TranslatedMessageDTO.SearchByParameters.MESSAGE_ID)
                            || searchParameter.Key.Equals(TranslatedMessageDTO.SearchByParameters.LANGUAGEID)
                            || searchParameter.Key.Equals(TranslatedMessageDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TranslatedMessageDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TranslatedMessageDTO.SearchByParameters.ACTIVE_LANGUAGES)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "= 1 )");
                        }
                        else if (searchParameter.Key == TranslatedMessageDTO.SearchByParameters.MESSAGE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }

                if (searchParameters.Count > 0)
                    selectLanguagesQuery = selectLanguagesQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLanguagesQuery, parameters.ToArray(), sqlTransaction);
            List<TranslatedMessageDTO> translatedMessageList = new List<TranslatedMessageDTO>();
            if (dataTable.Rows.Count > 0)
            {

                foreach (DataRow usersDataRow in dataTable.Rows)
                {
                    TranslatedMessageDTO translatedMessageObject = GetTranslatedMessageDTO(usersDataRow);
                    translatedMessageList.Add(translatedMessageObject);
                }
                log.LogMethodExit(translatedMessageList);
                return translatedMessageList;
            }
            else
            {
                log.LogMethodExit(translatedMessageList);
                return translatedMessageList;
            }
        }

        /// <summary>
        ///  Deletes the MessagesTranslated record based on Id 
        /// </summary>
        /// <param name="id">id is passed as parameter</param>
        internal void Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM MessagesTranslated
                             WHERE MessagesTranslated.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        internal List<TranslatedMessageDTO> GetTranslatedMessageDTOList(List<int> messageIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(messageIdList);
            List<TranslatedMessageDTO> productIdListDetailsDTOList = new List<TranslatedMessageDTO>();
            string query = @"SELECT *
                            FROM MessagesTranslated, @messageIdList List
                            WHERE MessageId = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 1 or IsActive is null) ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@messageIdList", messageIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productIdListDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetTranslatedMessageDTO(x)).ToList();
            }
            log.LogMethodExit(productIdListDetailsDTOList);
            return productIdListDetailsDTOList;
        }
    }
}

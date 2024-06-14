/********************************************************************************************
 * Project Name - LanguagesDatahandler
 * Description  - LanguagesDatahandler object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        24-June-2016   Rakshith          Created 
 *2.40        08-Oct-2018    Vivek             Added a method to fetch 
 *2.60.0      03-May-2019    Divya             SQL Injection
 *2.60        06-May-2019   Mushahid Faizan    Added Insert/update and GetSQLParameters() method.
 *2.70        24-Jul-2019   Mushahid Faizan    Added Delete Method.
 *2.70.2        29-Jul-2019   Girish Kundar      Modified : Changed the Structure of Data Handler.
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    public class LanguagesDatahandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM Languages AS lg ";
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;


        private static readonly Dictionary<LanguagesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LanguagesDTO.SearchByParameters, string>
            {
                {LanguagesDTO.SearchByParameters.LANGUAGE_CODE, "lg.LanguageCode"},
                {LanguagesDTO.SearchByParameters.LANGUAGE_ID, "lg.LanguageId"},
                {LanguagesDTO.SearchByParameters.LANGUAGE_NAME, "lg.LanguageName"},
                {LanguagesDTO.SearchByParameters.IS_ACTIVE, "lg.Active"},
                {LanguagesDTO.SearchByParameters.MASTER_ENTITY_ID, "lg.MasterEntityId"},
                {LanguagesDTO.SearchByParameters.SITE_ID, "lg.site_id"},
                {LanguagesDTO.SearchByParameters.READER_LANGUAGE_NO, "lg.ReaderLanguageNo"},

            };

        /// <summary>
        /// Default constructor of UserDataHandler class
        /// </summary>
        public LanguagesDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to LanguagesDTO class type
        /// </summary>
        /// <param name="languagesDataRow">languagesDataRow </param>
        /// <returns>Returns LanguagesDTO</returns>
        public LanguagesDTO GetLanguagesDTO(DataRow languagesDataRow)
        {
            log.LogMethodEntry(languagesDataRow);
            LanguagesDTO languagesDTO = new LanguagesDTO(
                                                    languagesDataRow["LanguageId"] == DBNull.Value ? -1 : Convert.ToInt32(languagesDataRow["LanguageId"]),
                                                    languagesDataRow["LanguageName"].ToString(),
                                                    languagesDataRow["LanguageCode"].ToString(),
                                                    languagesDataRow["FontName"] == DBNull.Value ? string.Empty : Convert.ToString(languagesDataRow["FontName"]),
                                                    languagesDataRow["FontSize"] == DBNull.Value ? -1 : Convert.ToInt32(languagesDataRow["FontSize"]),
                                                    languagesDataRow["ReaderLanguageNo"] == DBNull.Value ? -1 : Convert.ToInt32(languagesDataRow["ReaderLanguageNo"]),
                                                    languagesDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(languagesDataRow["Site_id"]),
                                                    languagesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(languagesDataRow["SynchStatus"]),
                                                    languagesDataRow["Remarks"].ToString(),
                                                    languagesDataRow["CultureCode"].ToString(),
                                                    languagesDataRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(languagesDataRow["Active"]),
                                                    languagesDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(languagesDataRow["Guid"]),
                                                    languagesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(languagesDataRow["MasterEntityId"]),
                                                    languagesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(languagesDataRow["CreatedBy"]),
                                                    languagesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(languagesDataRow["CreationDate"]),
                                                    languagesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(languagesDataRow["LastUpdatedBy"]),
                                                    languagesDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(languagesDataRow["LastUpdateDate"])
                                                        );
            log.LogMethodExit(languagesDTO);
            return languagesDTO;
        }

        /// <summary>
        /// Gets the LanguagesDTO matching the languageId parameter
        /// </summary>
        /// <param name="languageId">languageId parameter</param>
        public LanguagesDTO GetLanguage(int languageId)
        {
            log.LogMethodEntry();
            LanguagesDTO languageDTO = null;
            string selectLanguagesQuery = SELECT_QUERY + "  where lg.LanguageId=@languageId";

            SqlParameter[] selectLanguageParameters = new SqlParameter[1];
            selectLanguageParameters[0] = new SqlParameter("@languageId", languageId);
            DataTable languageData = dataAccessHandler.executeSelectQuery(selectLanguagesQuery, selectLanguageParameters, sqlTransaction);
            if (languageData.Rows.Count > 0)
            {
                foreach (DataRow languageRow in languageData.Rows)
                {
                    languageDTO = GetLanguagesDTO(languageRow);
                }
            }
            log.LogMethodExit(languageDTO);
            return languageDTO;
        }


        /// <summary>
        /// Gets the LanguagesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LanguagesDTO matching the search criteria</returns>
        public List<LanguagesDTO> GetAllLanguagesList(List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectLanguagesQuery = SELECT_QUERY;
            List<LanguagesDTO> languagesDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<LanguagesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(LanguagesDTO.SearchByParameters.LANGUAGE_ID) 
                            || searchParameter.Key.Equals(LanguagesDTO.SearchByParameters.READER_LANGUAGE_NO)
                            || searchParameter.Key.Equals(LanguagesDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LanguagesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " = -1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LanguagesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" ));

                        }
                        else if (searchParameter.Key.Equals(LanguagesDTO.SearchByParameters.LANGUAGE_CODE) ||
                                  searchParameter.Key.Equals(LanguagesDTO.SearchByParameters.LANGUAGE_NAME))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            DataTable usersData = dataAccessHandler.executeSelectQuery(selectLanguagesQuery, parameters.ToArray(), sqlTransaction);
            if (usersData.Rows.Count > 0)
            {
                languagesDTOList = new List<LanguagesDTO>();
                foreach (DataRow usersDataRow in usersData.Rows)
                {
                    LanguagesDTO LanguageObject = GetLanguagesDTO(usersDataRow);
                    languagesDTOList.Add(LanguageObject);
                }
            }
            log.LogMethodExit(languagesDTOList);
            return languagesDTOList;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Languages Record.
        /// </summary>
        /// <param name="languagesDTO">languagesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LanguagesDTO languagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(languagesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@languageId", languagesDTO.LanguageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LanguageName", languagesDTO.LanguageName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LanguageCode", languagesDTO.LanguageCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FontName", languagesDTO.FontName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FontSize", languagesDTO.FontSize, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReaderLanguageNo", languagesDTO.ReaderLanguageNo, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", languagesDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CultureCode", languagesDTO.CultureCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Active", languagesDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", languagesDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", languagesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        

        /// <summary>
        /// Inserts the languagesDTO record to the database
        /// </summary>
        /// <param name="languagesDTO">languagesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns LanguagesDTO</returns>
        public LanguagesDTO InsertLanguages(LanguagesDTO languagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(languagesDTO, loginId, siteId);
            string query = @"INSERT INTO Languages 
                                        ( 
                                            LanguageName,
                                            LanguageCode,
                                            FontName,
                                            FontSize,
                                            ReaderLanguageNo,
                                            site_id,
                                            Guid,
                                            Remarks,
                                            CultureCode,
                                            Active,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @LanguageName,
                                            @LanguageCode,
                                            @FontName,
                                            @FontSize,
                                            @ReaderLanguageNo,
                                            @site_id,
                                            NEWID(),
                                            @Remarks,
                                            @CultureCode,
                                            @Active,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @MasterEntityId
                                        ) SELECT * from Languages where LanguageId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(languagesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLanguagesDTO(languagesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting languagesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(languagesDTO);
            return languagesDTO;
        }

        /// <summary>
        /// Updates the languagesDTO record
        /// </summary>
        /// <param name="languagesDTO">languagesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the LanguagesDTO</returns>
        public LanguagesDTO UpdateLanguages(LanguagesDTO languagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(languagesDTO, loginId, siteId);
            string query = @"UPDATE [Languages] SET [LanguageName] = @LanguageName,
                            [LanguageCode] = @LanguageCode, 
                            [FontName] = @FontName,
                            [FontSize] = @FontSize, 
                            [ReaderLanguageNo] = @ReaderLanguageNo,
                            -- [site_id] = @site_id,
                            [Guid] = @Guid,
                            [Remarks] = @Remarks,
                            [CultureCode] = @CultureCode,
                            [Active] = @Active ,
                            LastUpdatedBy = @LastUpdatedBy,
                            LastUpdateDate = GetDate()
                            WHERE (LanguageId = @LanguageId)
                    SELECT * from Languages where LanguageId = @LanguageId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(languagesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLanguagesDTO(languagesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating languagesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(languagesDTO);
            return languagesDTO;
        }

        /// <summary>
        ///  updates the LanguagesDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="LanguagesDTO">LanguagesDTO object is passed</param>
        /// <param name="dt">dt an object of DataTable</param>
        private void RefreshLanguagesDTO(LanguagesDTO languagesDTO, DataTable dt)
        {
            log.LogMethodEntry(languagesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                languagesDTO.LanguageId = Convert.ToInt32(dt.Rows[0]["LanguageId"]);
                languagesDTO.LastUpdateDate = dt.Rows[0]["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                languagesDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                languagesDTO.LastUpdatedBy = dt.Rows[0]["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                languagesDTO.Site_id = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
                languagesDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                languagesDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the record from the database based on languageId
        /// </summary>
        /// <returns>return the int </returns>
        public int DeleteLanguages(int languageId)
        {
            log.LogMethodEntry(languageId);
            try
            {
                string deleteQuery = @"delete  
                                          from languages
                                          where LanguageId = @languageId";

                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@languageId", languageId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }

        internal DateTime? GetLanguageModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastUpdateDate) LastUpdatedDate from Languages WHERE (site_id = @siteId or @siteId = -1)";
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

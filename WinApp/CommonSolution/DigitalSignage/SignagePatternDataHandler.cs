/********************************************************************************************
 * Project Name - Signage Pattern Data Handler
 * Description  - Data handler of the Signage Pattern class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana     Created 
 *2.70.2        31-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query 
  *2.110.0     30-Nov-2020       Prajwal S          Modified : Dictionary for searching parameters.
 *                                                 Modified : UpdateSignagePattern.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    class SignagePatternDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        /// <summary>
        /// Dictionary for searching Parameters for the SignagePatternDTO object.
        /// </summary>
        private static readonly Dictionary<SignagePatternDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SignagePatternDTO.SearchByParameters, string>
        {
                {SignagePatternDTO.SearchByParameters.SIGNAGE_PATTERN_ID,"SignagePattern.SignagePatternId"},
                {SignagePatternDTO.SearchByParameters.NAME,"SignagePattern.Name"},
                {SignagePatternDTO.SearchByParameters.IS_ACTIVE, "SignagePattern.IsActive"},
                {SignagePatternDTO.SearchByParameters.SITE_ID, "SignagePattern.site_id"},
                {SignagePatternDTO.SearchByParameters.MASTER_ENTITY_ID, "SignagePattern.MasterEntityId"}
        };
        private const string SELECT_QUERY = @" SELECT * FROM SignagePattern  ";

        /// <summary>
        ///  Default constructor of SignagePatternDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public SignagePatternDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating SignagePattern Record.
        /// </summary>
        /// <param name="signagePatternDTO">SignagePattern type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(SignagePatternDTO signagePatternDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(signagePatternDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@SignagePatternId", signagePatternDTO.SignagePatternId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", signagePatternDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Pattern", signagePatternDTO.Pattern));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", signagePatternDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", signagePatternDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the SignagePattern record to the database
        /// </summary>
        /// <param name="signagePatternDTO">signagePatternDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>signagePatternDTO</returns>
        public SignagePatternDTO InsertSignagePattern(SignagePatternDTO signagePatternDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(signagePatternDTO, loginId, siteId);
            string query = @"INSERT INTO SignagePattern
                                        ( 
                                            Name,
                                            Pattern,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            guid,
                                            MasterEntityId
                                        )
                                        VALUES
                                        (  @Name,
                                            @Pattern,
                                            @IsActive,
                                            @CreatedBy,
                                            Getdate(),
                                            @LastUpdatedBy,
                                            Getdate(),
                                            @site_id,
                                            NEWID(),
                                            @MasterEntityId
                                        )
                                        SELECT * FROM SignagePattern WHERE SignagePatternId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(signagePatternDTO, loginId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    signagePatternDTO.SignagePatternId = Convert.ToInt32(dt.Rows[0]["SignagePatternId"]);
                    signagePatternDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastUpdatedDate"]);
                    signagePatternDTO.CreatedBy = loginId;
                    signagePatternDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                    signagePatternDTO.Guid = Convert.ToString(dt.Rows[0]["guid"]);
                    signagePatternDTO.LastUpdatedBy = loginId;
                    signagePatternDTO.SiteId = siteId;
                }
            }
            catch (Exception ex)
            {
                log.LogVariableState("SignagePatternDTO", signagePatternDTO);
                log.Error("Error occured while inserting the SignagePattern record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(signagePatternDTO);
            return signagePatternDTO;
        }

        /// <summary>
        /// Update Signage Pattern
        /// </summary>
        /// <param name="signagePatternDTO">signagePatternDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>signagePatternDTO</returns>
        public SignagePatternDTO UpdateSignagePattern(SignagePatternDTO signagePatternDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(signagePatternDTO, loginId, siteId);
            string query = @"UPDATE SignagePattern SET 
                            Name = @Name,
                            Pattern = @Pattern,
                            IsActive = @IsActive,
                            LastUpdatedBy = @LastUpdatedBy,
                            LastUpdatedDate = Getdate(),
                            --site_id = @site_id,
                            MasterEntityId =  @MasterEntityId
                            WHERE  SignagePatternId = @SignagePatternId
                            SELECT * FROM SignagePattern WHERE SignagePatternId = @SignagePatternId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(signagePatternDTO, loginId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    signagePatternDTO.SignagePatternId = Convert.ToInt32(dt.Rows[0]["SignagePatternId"]);
                    signagePatternDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastUpdatedDate"]);
                    signagePatternDTO.CreatedBy = loginId;
                    signagePatternDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                    signagePatternDTO.Guid = Convert.ToString(dt.Rows[0]["guid"]);
                    signagePatternDTO.LastUpdatedBy = loginId;
                    signagePatternDTO.SiteId = siteId;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating the SignagePattern record", ex);
                log.LogVariableState("SignagePatternDTO", signagePatternDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(signagePatternDTO);
            return signagePatternDTO;
        }

        /// <summary>
        /// Create Signage Pattern DTO List
        /// </summary>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        private List<SignagePatternDTO> CreateSignagePatternDTOList(SqlDataReader reader)
        {
            log.LogMethodEntry(reader);
            List<SignagePatternDTO> signagePatternDTOList = new List<SignagePatternDTO>();
            int signagePatternId = reader.GetOrdinal("SignagePatternId");
            int name = reader.GetOrdinal("Name");
            int pattern = reader.GetOrdinal("Pattern");
            int isActive = reader.GetOrdinal("IsActive");
            int createdBy = reader.GetOrdinal("CreatedBy");
            int creationDate = reader.GetOrdinal("CreationDate");
            int lastUpdatedBy = reader.GetOrdinal("LastUpdatedBy");
            int lastUpdatedDate = reader.GetOrdinal("LastUpdatedDate");
            int site_id = reader.GetOrdinal("site_id");
            int guid = reader.GetOrdinal("guid");
            int masterEntityId = reader.GetOrdinal("MasterEntityId");
            int synchStatus = reader.GetOrdinal("SynchStatus");

            while (reader.Read())
            {
                SignagePatternDTO signagePatternDTO = new SignagePatternDTO(reader.IsDBNull(signagePatternId) ? -1 : reader.GetInt32(signagePatternId),
                                        reader.IsDBNull(name) ? "" : reader.GetString(name),
                                        reader.IsDBNull(pattern) ? "" : reader.GetString(pattern),
                                        reader.IsDBNull(isActive) ? false : reader.GetBoolean(isActive),
                                        reader.IsDBNull(createdBy) ? "" : reader.GetString(createdBy),
                                        reader.IsDBNull(creationDate) ? DateTime.MinValue : reader.GetDateTime(creationDate),
                                        reader.IsDBNull(lastUpdatedBy) ? "" : reader.GetString(lastUpdatedBy),
                                        reader.IsDBNull(lastUpdatedDate) ? DateTime.MinValue : reader.GetDateTime(lastUpdatedDate),
                                        reader.IsDBNull(site_id) ? -1 : reader.GetInt32(site_id),
                                        reader.IsDBNull(masterEntityId) ? -1 : reader.GetInt32(masterEntityId),
                                        reader.IsDBNull(synchStatus) ? false : reader.GetBoolean(synchStatus),
                                        reader.IsDBNull(guid) ? "" : reader.GetGuid(guid).ToString()
                                        );
                signagePatternDTOList.Add(signagePatternDTO);
            }
            log.LogMethodExit(signagePatternDTOList);
            return signagePatternDTOList;
        }

        /// <summary>
        /// Gets the SignagePattern data of passed signagePattern Id
        /// </summary>
        /// <param name="signagePatternId">integer type parameter</param>
        /// <param name="openTransactionsOnly">openTransactionsOnly</param>
        /// <returns>Returns SignagePatternDTO</returns>
        public SignagePatternDTO GetSignagePatternDTO(int signagePatternId, bool openTransactionsOnly)
        {
            log.LogMethodEntry(signagePatternId);
            SignagePatternDTO result = null;
            string selectQuery = SELECT_QUERY + " WHERE SignagePattern.SignagePatternId = @SignagePatternId";

            SqlParameter[] selectSignagePatternParameters = new SqlParameter[1];
            selectSignagePatternParameters[0] = new SqlParameter("@SignagePatternId", signagePatternId);
            List<SignagePatternDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, selectSignagePatternParameters, sqlTransaction, CreateSignagePatternDTOList);
            if (list != null)
            {
                result = list[0];
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the SignagePattern data of passed signagePattern Id
        /// </summary>
        /// <param name="contentGuid">integer type parameter</param>
        /// <param name="openTransactionsOnly">openTransactionsOnly</param>
        /// <returns>Returns SignagePatternDTO</returns>
        public SignagePatternDTO GetSignagePatternDTOByGuid(string contentGuid, bool openTransactionsOnly)
        {
            log.LogMethodEntry(contentGuid);
            SignagePatternDTO result = null;
            string selectQuery = SELECT_QUERY + " WHERE SignagePattern.guid = @ContentGuid";

            SqlParameter[] selectSignagePatternParameters = new SqlParameter[1];
            selectSignagePatternParameters[0] = new SqlParameter("@ContentGuid", contentGuid);
            List<SignagePatternDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, selectSignagePatternParameters, sqlTransaction, CreateSignagePatternDTOList);
            if (list != null)
            {
                result = list[0];
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the SignagePatternDTO list matching the UserId
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of signagePatternDTO matching the search criteria</returns>
        public List<SignagePatternDTO> GetSignagePatternDTOList(List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string whereClause = string.Empty;
            if (searchParameters == null || searchParameters.Count == 0)
            {
                log.LogMethodExit(string.Empty, "search parameters is empty");
            }
            else
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SignagePatternDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(SignagePatternDTO.SearchByParameters.SIGNAGE_PATTERN_ID) ||
                                searchParameter.Key.Equals(SignagePatternDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == SignagePatternDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == SignagePatternDTO.SearchByParameters.IS_ACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1")));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
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
                whereClause = query.ToString();
            }
            
            
            string selectQuery = SELECT_QUERY + whereClause;
            List<SignagePatternDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, parameters.ToArray(), sqlTransaction, CreateSignagePatternDTOList);
            log.LogMethodExit(list);
            return list;
        }
    }
}

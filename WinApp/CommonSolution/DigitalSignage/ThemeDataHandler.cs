/********************************************************************************************
 * Project Name - Theme Data Handler
 * Description  - Data handler of the Theme class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana     Created 
 *2.70.2        31-Jul-2019   Dakshakh raj      Modified : Added Parameterized costrustor
 *2.70.2       06-Dec-2019   Jinto Thomas       Removed siteid from update query
 *2.70.3       07-Jan-2020   Archana            Search parameter TYPE_ID_LIST is added   
 *2.100.0         10-Aug-2020   Mushahid Faizan     Modified : default isActive value to true.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    ///  Theme Data Handler - Handles insert, update and select of  Theme objects
    /// </summary>
    public class ThemeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Theme";

        /// <summary>
        /// Dictionary for searching Parameters for the Theme object.
        /// </summary>
        private static readonly Dictionary<ThemeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ThemeDTO.SearchByParameters, string>
            {
                {ThemeDTO.SearchByParameters.ID, "Theme.Id"},
                {ThemeDTO.SearchByParameters.TYPE_ID, "Theme.TypeId"},
                {ThemeDTO.SearchByParameters.TYPE_LIST, "LookupValues.LookupValue"},
                {ThemeDTO.SearchByParameters.NAME, "Theme.Name"},
                {ThemeDTO.SearchByParameters.IS_ACTIVE, "Theme.IsActive"},
                {ThemeDTO.SearchByParameters.MASTER_ENTITY_ID,"Theme.MasterEntityId"},
                {ThemeDTO.SearchByParameters.SITE_ID, "Theme.site_id"},
                {ThemeDTO.SearchByParameters.TYPE_ID_LIST, "Theme.TypeId"}
            };

        /// <summary>
        /// Default constructor of ThemeDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ThemeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating screenTransitionsDTO parameters Record.
        /// </summary>
        /// <param name="themeDTO">themeDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ThemeDTO themeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(themeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", themeDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", string.IsNullOrEmpty(themeDTO.Name) ? DBNull.Value : (object)themeDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TypeId", themeDTO.TypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", string.IsNullOrEmpty(themeDTO.Description) ? DBNull.Value : (object)themeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InitialScreenId", themeDTO.InitialScreenId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ThemeNumber", themeDTO.ThemeNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (themeDTO.IsActive == true?"Y":"N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", themeDTO.MasterEntityId, true));
            log.LogMethodExit(parameters); 
            return parameters;
        }

        /// <summary>
        /// Inserts the Theme record to the database
        /// </summary>
        /// <param name="themeDTO">ThemeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ThemeDTO InsertTheme(ThemeDTO themeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(themeDTO, loginId, siteId);
            string query = @"INSERT INTO Theme 
                                        ( 
                                            Name,
                                            TypeId,
                                            Description,
                                            InitialScreenId,
                                            ThemeNumber,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            MasterEntityId,
                                            Guid
                                        ) 
                                VALUES 
                                        (
                                            @Name,
                                            @TypeId,
                                            @Description,
                                            @InitialScreenId,
                                            @ThemeNumber,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            NEWID()
                                        ) SELECT * FROM Theme WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(themeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshThemeDTO(themeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting themeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(themeDTO);
            return themeDTO;
        }


        /// <summary>
        /// Updates the Theme record
        /// </summary>
        /// <param name="themeDTO">ThemeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ThemeDTO UpdateTheme(ThemeDTO themeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(themeDTO, loginId, siteId);
            string query = @"UPDATE Theme 
                             SET Name=@Name,
                                 TypeId=@TypeId,
                                 Description=@Description,
                                 InitialScreenId=@InitialScreenId,
                                 ThemeNumber=@ThemeNumber,
                                 IsActive=@IsActive,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdatedDate = GETDATE()
                                 --site_id=@site_id
                             WHERE Id = @Id
                             SELECT * FROM Theme WHERE Id = @Id";
            try
            {
                if (string.Equals(themeDTO.IsActive, "N") && GetThemeReferenceCount(themeDTO.Id) > 0)
                {
                    throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                }
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(themeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshThemeDTO(themeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating themeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(themeDTO);
            return themeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="themeDTO">themeDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshThemeDTO(ThemeDTO themeDTO, DataTable dt)
        {
            log.LogMethodEntry(themeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                themeDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                themeDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                themeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                themeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                themeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                themeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                themeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether theme is in use.
        /// <param name="id">Theme Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        private int GetThemeReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT COUNT(*) as ReferenceCount
                             FROM DisplayPanelThemeMap
                             WHERE ThemeId=@ThemeId AND IsActive = 'Y'";
            SqlParameter parameter = new SqlParameter("@ThemeId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Converts the Data row object to ThemeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ThemeDTO</returns>
        private ThemeDTO GetThemeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ThemeDTO themeDTO = new ThemeDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["Name"] == DBNull.Value ? "" : dataRow["Name"].ToString(),
                                            dataRow["TypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TypeId"]),
                                            dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                                            dataRow["InitialScreenId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["InitialScreenId"]),
                                            dataRow["ThemeNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ThemeNumber"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : (dataRow["IsActive"].ToString() == "Y"? true: false),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(themeDTO);
            return themeDTO;
        }

        /// <summary>
        /// Gets the Theme data of passed Theme Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ThemeDTO</returns>
        public ThemeDTO GetThemeDTO(int id)
        {
            log.LogMethodEntry(id);
            ThemeDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE Theme.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetThemeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ThemeDTO matching the search criteria</returns>
        public List<ThemeDTO> GetThemeDTOList(List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ThemeDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT Theme.* FROM Theme 
                                   INNER JOIN LookupValues 
                                   ON LookupValues.LookupValueId = Theme.TypeId ";
            if((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach(KeyValuePair<ThemeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if(searchParameter.Key == ThemeDTO.SearchByParameters.ID ||
                            searchParameter.Key == ThemeDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == ThemeDTO.SearchByParameters.TYPE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == ThemeDTO.SearchByParameters.TYPE_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if(searchParameter.Key == ThemeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ThemeDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == ThemeDTO.SearchByParameters.TYPE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                list = new List<ThemeDTO>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    ThemeDTO themeDTO = GetThemeDTO(dataRow);
                    list.Add(themeDTO);
                }
               
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetThemeTypeString
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        private string GetThemeTypeString(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            string[] types = value.Split(',');
            string result = string.Empty;
            string seperator = string.Empty;
            foreach (var type in types)
            {
                result += seperator + "'" + type + "'";
                seperator = ", ";
            }
            return result;
        }
        internal DateTime? GetThemeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from Theme WHERE (site_id = @siteId or @siteId = -1)
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

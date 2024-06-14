/********************************************************************************************
 * Project Name - ScreenTransitions Data Handler
 * Description  - Data handler of the ScreenTransitions class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana     Created
 *2.70.2        31-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                          
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
    ///  ScreenTransitions Data Handler - Handles insert, update and select of  ScreenTransitions objects
    /// </summary>
    public class ScreenTransitionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ScreenTransitions AS st";

        /// <summary>
        /// Dictionary for searching Parameters for the ScreenTransitions object.
        /// </summary>
        private static readonly Dictionary<ScreenTransitionsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ScreenTransitionsDTO.SearchByParameters, string>
            {
                {ScreenTransitionsDTO.SearchByParameters.ID, "st.Id"},
                {ScreenTransitionsDTO.SearchByParameters.THEME_ID, "st.ThemeId"},
                {ScreenTransitionsDTO.SearchByParameters.THEME_ID_LIST, "st.ThemeId"},
                {ScreenTransitionsDTO.SearchByParameters.IS_ACTIVE, "st.IsActive"},
                {ScreenTransitionsDTO.SearchByParameters.MASTER_ENTITY_ID,"st.MasterEntityId"},
                {ScreenTransitionsDTO.SearchByParameters.SITE_ID, "st.site_id"}
            };

        /// <summary>
        /// Default constructor of ScreenTransitionsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScreenTransitionsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating screenTransitionsDTO parameters Record.
        /// </summary>
        /// <param name="screenTransitionsDTO">screenTransitionsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ScreenTransitionsDTO screenTransitionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenTransitionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", screenTransitionsDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ThemeId", screenTransitionsDTO.ThemeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromScreenId", screenTransitionsDTO.FromScreenId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EventId", screenTransitionsDTO.EventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToScreenId", screenTransitionsDTO.ToScreenId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (screenTransitionsDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", screenTransitionsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ScreenTransitions record to the database
        /// </summary>
        /// <param name="screenTransitionsDTO">ScreenTransitionsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>ScreenTransitions DTO</returns>
        public ScreenTransitionsDTO InsertScreenTransitions(ScreenTransitionsDTO screenTransitionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenTransitionsDTO, loginId, siteId);
            string query = @"INSERT INTO ScreenTransitions 
                                        ( 
                                            ThemeId,
                                            FromScreenId,
                                            EventId,
                                            ToScreenId,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @ThemeId,
                                            @FromScreenId,
                                            @EventId,
                                            @ToScreenId,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        )SELECT * FROM ScreenTransitions WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(screenTransitionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScreenTransitionsDTO(screenTransitionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting screenTransitionsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(screenTransitionsDTO);
            return screenTransitionsDTO;
        }

        /// <summary>
        /// Updates the ScreenTransitions record
        /// </summary>
        /// <param name="screenTransitionsDTO">ScreenTransitionsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>ScreenTransitions DTO</returns>
        public ScreenTransitionsDTO UpdateScreenTransitions(ScreenTransitionsDTO screenTransitionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenTransitionsDTO, loginId, siteId);
            string query = @"UPDATE ScreenTransitions 
                             SET ThemeId=@ThemeId,
                                 FromScreenId=@FromScreenId,
                                 EventId=@EventId,
                                 ToScreenId=@ToScreenId,
                                 IsActive=@IsActive,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdatedDate = GETDATE()
                                 --site_id=@site_id
                             WHERE Id = @Id
                             SELECT * FROM ScreenTransitions WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(screenTransitionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScreenTransitionsDTO(screenTransitionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating screenTransitionsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(screenTransitionsDTO);
            return screenTransitionsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="screenTransitionsDTO">screenTransitionsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshScreenTransitionsDTO(ScreenTransitionsDTO screenTransitionsDTO, DataTable dt)
        {
            log.LogMethodEntry(screenTransitionsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                screenTransitionsDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                screenTransitionsDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                screenTransitionsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                screenTransitionsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                screenTransitionsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                screenTransitionsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                screenTransitionsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ScreenTransitionsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ScreenTransitionsDTO</returns>
        private ScreenTransitionsDTO GetScreenTransitionsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ScreenTransitionsDTO screenTransitionsDTO = new ScreenTransitionsDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ThemeId"]),
                                            dataRow["FromScreenId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FromScreenId"]),
                                            dataRow["EventId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["EventId"]),
                                            dataRow["ToScreenId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ToScreenId"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : (dataRow["IsActive"].ToString() == "Y" ? true : false),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(screenTransitionsDTO);
            return screenTransitionsDTO;
        }

        /// <summary>
        /// Gets the ScreenTransitions data of passed ScreenTransitions Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ScreenTransitionsDTO</returns>
        public ScreenTransitionsDTO GetScreenTransitionsDTO(int id)
        {
            log.LogMethodEntry(id);
            ScreenTransitionsDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE st.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetScreenTransitionsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScreenTransitionsDTO matching the search criteria</returns>
        public List<ScreenTransitionsDTO> GetScreenTransitionsDTOList(List<KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ScreenTransitionsDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == ScreenTransitionsDTO.SearchByParameters.ID ||
                            searchParameter.Key == ScreenTransitionsDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == ScreenTransitionsDTO.SearchByParameters.THEME_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScreenTransitionsDTO.SearchByParameters.THEME_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScreenTransitionsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScreenTransitionsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ScreenTransitionsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ScreenTransitionsDTO screenTransitionsDTO = GetScreenTransitionsDTO(dataRow);
                    list.Add(screenTransitionsDTO);
                }
            }

            log.LogMethodExit(list);
            return list;
        }
    }
}

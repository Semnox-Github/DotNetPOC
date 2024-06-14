/********************************************************************************************
* Project Name - Achievements
* Description  - Data Handler -AchievementClass 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70        02-JUl-2019   Deeksha                 Modified :Added GetSqlParameter(),SQL injection issue Fix
*                                                            createdBy and creationDate fields 
*                                                            changed log.debug to log.logMethodEntry
*                                                            and log.logMethodExit
*2.80        27-Aug-2019   Vikas Dwivedi            Added SqlTransaction,
*                                                   Added SqlTransaction in the Constructor,
*                                                   Modified executeInsertQuery, executeUpdateQuery,
*                                                   executeSelectQuery, executeDeleteQuery and passed 
*                                                   SqlTransaction as a parameter,
*                                                   Added WHO columns.
*2.80        19-Nov-2019   Vikas Dwivedi            Added Logger Method.
*2.70.2        05-Dec-2019   Jinto Thomas           Removed siteid from update query
*2.80        04-Mar-2020   Vikas Dwivedi            Modified as per the Standards for Phase 1 Changes.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Achievements
{
    public class AchievementClassDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AchievementClass AS ac ";
        /// <summary>
        /// Dictionary for searching Parameters for the AchievementClass object.
        /// </summary>
        private static readonly Dictionary<AchievementClassDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AchievementClassDTO.SearchByParameters, string>
            {
                {AchievementClassDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID, "ac.AchievementClassId"},
                {AchievementClassDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID_LIST, "ac.AchievementClassId"},
                {AchievementClassDTO.SearchByParameters.ACHIEVEMENT_PROJECT_ID, "ac.AchievementProjectId"},
                {AchievementClassDTO.SearchByParameters.ACHIEVEMENT_PROJECT_ID_LIST, "ac.AchievementProjectId"},
                {AchievementClassDTO.SearchByParameters.CLASS_NAME, "ac.ClassName"},
                {AchievementClassDTO.SearchByParameters.GAME_ID, "ac.GameId"},
                {AchievementClassDTO.SearchByParameters.IS_ACTIVE, "ac.IsActive"},
                {AchievementClassDTO.SearchByParameters.SITE_ID, "ac.site_id"},
                {AchievementClassDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, "ac.ExternalSystemReference"},
                {AchievementClassDTO.SearchByParameters.MASTER_ENTITY_ID, "ac.MasterEntityId"}
            };

        /// <summary>
        /// Parameterized Constructor for AchievementClassDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public AchievementClassDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AchievementClass Record.
        /// </summary>
        /// <param name="achievementClassDTO">AchievementClassDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(AchievementClassDTO achievementClassDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementClassDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementClassId", achievementClassDTO.AchievementClassId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ClassName", achievementClassDTO.ClassName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementProjectId", achievementClassDTO.AchievementProjectId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameId", achievementClassDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", achievementClassDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", achievementClassDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", achievementClassDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", achievementClassDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to AchievementClassDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the AchievementClassDTO</returns>
        private AchievementClassDTO GetAchievementClassDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AchievementClassDTO achievementClassDTO = new AchievementClassDTO(dataRow["AchievementClassId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AchievementClassId"]),
                                                         dataRow["ClassName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ClassName"]),
                                                         dataRow["AchievementProjectId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AchievementProjectId"]),
                                                         dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                                         dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                         dataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalSystemReference"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                         dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                         dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]));
            log.LogMethodExit(achievementClassDTO);
            return achievementClassDTO;
        }
        /// <summary>
        /// Gets the AchievementClass data of passed id 
        /// </summary>
        /// <param name="id">id of AchievementClass is passed as parameter</param>
        /// <returns>Returns AchievementClassDTO</returns>
        public AchievementClassDTO GetAchievementClassDTO(int id)
        {
            log.LogMethodEntry(id);
            AchievementClassDTO result = null;
            string query = SELECT_QUERY + @" WHERE ac.AchievementClassId= @AchievementClassId";
            SqlParameter parameter = new SqlParameter("@AchievementClassId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAchievementClassDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the AchievementClassDTO Table.
        /// </summary>
        /// <param name="AchievementClassDTO">AchievementClassDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the AchievementClassDTO </returns>
        public AchievementClassDTO InsertAchievementClass(AchievementClassDTO achievementClassDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementClassDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AchievementClass]
                                                            (ClassName,
                                                            AchievementProjectId,
                                                            GameId,
                                                            ProductId,
                                                            ExternalSystemReference,
                                                            IsActive,
                                                            LastUpdatedDate,
                                                            LastUpdatedUser,
                                                            Guid,
                                                            MasterEntityId,
                                                            site_id,
                                                            CreatedBy,
                                                            CreationDate
                                                           )
                                                       values
                                                         (
                                                            @ClassName,
                                                            @AchievementProjectId,
                                                            @GameId,
                                                            @ProductId,
                                                            @ExternalSystemReference,
                                                            @IsActive,
                                                            GetDate(),
                                                            @LastUpdatedUser,
                                                            NewId(),
                                                            @MasterEntityId,
                                                            @site_id,
                                                            @CreatedBy,
                                                            getDate() )
                                            SELECT* FROM AchievementClass WHERE AchievementClassId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementClassDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementClassDTO(achievementClassDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AchievementClassDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementClassDTO);
            return achievementClassDTO;
        }

        /// <summary>
        ///  Updates the record to the AchievementClassDTO Table.
        /// </summary>
        /// <param name="AchievementClassDTO">AchievementClassDTO object passed as parameter</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the AchievementClassDTO</returns>
        public AchievementClassDTO UpdateAchievementClass(AchievementClassDTO achievementClassDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementClassDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AchievementClass]
                           SET 
                            ClassName            =   @ClassName,
                            AchievementProjectId =   @AchievementProjectId,
                            GameId               =   @GameId,
                            ProductId            =   @ProductId,
                            ExternalSystemReference= @ExternalSystemReference,
                            IsActive             =   @IsActive,
                            LastUpdatedDate      =   GetDate(),
                            LastUpdatedUser      =   @LastUpdatedUser,
                            Guid                 =   NewId(),
                            MasterEntityId       =   @MasterEntityId
                            --site_id              =   @site_id
          
                           WHERE AchievementClassId =@AchievementClassId 
                                    SELECT * FROM AchievementClass WHERE AchievementClassId = @AchievementClassId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementClassDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementClassDTO(achievementClassDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AchievementClassDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementClassDTO);
            return achievementClassDTO;
        }

        /// <summary>
        /// Delete the record from the AchievementClass database based on achievementClassId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int achievementClassId)
        {
            log.LogMethodEntry(achievementClassId);
            string query = @"DELETE  
                             FROM AchievementClass
                             WHERE AchievementClass.AchievementClassId = @AchievementClassId";
            SqlParameter parameter = new SqlParameter("@AchievementClassId", achievementClassId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementClassDTO">AchievementClassDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAchievementClassDTO(AchievementClassDTO achievementClassDTO, DataTable dt)
        {
            log.LogMethodEntry(achievementClassDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                achievementClassDTO.AchievementClassId = Convert.ToInt32(dt.Rows[0]["AchievementClassId"]);
                achievementClassDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                achievementClassDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                achievementClassDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                achievementClassDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                achievementClassDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                achievementClassDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AchievementClass based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AchievementClass</returns>
        public List<AchievementClassDTO> GetAchievementClassDTOList(List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AchievementClassDTO> achievementClassDTOList = new List<AchievementClassDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AchievementClassDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (
                           searchParameter.Key.Equals(AchievementClassDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID) ||
                           searchParameter.Key.Equals(AchievementClassDTO.SearchByParameters.ACHIEVEMENT_PROJECT_ID) ||
                           searchParameter.Key.Equals(AchievementClassDTO.SearchByParameters.GAME_ID) ||
                           searchParameter.Key.Equals(AchievementClassDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AchievementClassDTO.SearchByParameters.CLASS_NAME ||
                                (searchParameter.Key == AchievementClassDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                        }
                        else if (searchParameter.Key == AchievementClassDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AchievementClassDTO.SearchByParameters.ACHIEVEMENT_PROJECT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AchievementClassDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AchievementClassDTO.SearchByParameters.IS_ACTIVE) // bit
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
                    AchievementClassDTO achievementClassDTO = GetAchievementClassDTO(dataRow);
                    achievementClassDTOList.Add(achievementClassDTO);
                }
            }
            log.LogMethodExit(achievementClassDTOList);
            return achievementClassDTOList;
        }
    }

}



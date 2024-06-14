/********************************************************************************************
 * Project Name - Achievement
 * Description  - Data Handler -AchievementLevel
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-July-2019    Deeksha              Modified :Added GetSqlParameter() 
 *                                                 SQL injection issue has been fixed
 *                                                 changed log.debug to log.logMethodEntry
 *                                                 and log.logMethodExit
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Achievements
{
    public class AchievementLevelDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AchievementLevel AS al ";
        /// <summary>
        /// Dictionary for searching Parameters for the AchievementLevel object.
        /// </summary> 
        private static readonly Dictionary<AchievementLevelDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AchievementLevelDTO.SearchByParameters, string>
            {
                {AchievementLevelDTO.SearchByParameters.ID, "al.Id"},
                {AchievementLevelDTO.SearchByParameters.CARD_ID, "al.CardId"},
                {AchievementLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, "al.AchievementClassLevelId"},
                {AchievementLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID_LIST, "al.AchievementClassLevelId"},
                {AchievementLevelDTO.SearchByParameters.ISVALID, "al.IsValid"},
                {AchievementLevelDTO.SearchByParameters.IS_ACTIVE, "al.IsActive"},
                {AchievementLevelDTO.SearchByParameters.EFFECTIVE_DATE, "al.EffectiveDate"},
                {AchievementLevelDTO.SearchByParameters.SITE_ID, "al.site_id"},
                {AchievementLevelDTO.SearchByParameters.MASTER_ENTITY_ID, "al.MasterEntityId"}
             };

        /// <summary>
        /// Parameterized Constructor for AchievementLevelDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public AchievementLevelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AchievementLevel Record.
        /// </summary>
        /// <param name="achievementLevelDTO">AchievementLevelDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(AchievementLevelDTO achievementLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementLevelDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", achievementLevelDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", achievementLevelDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementClassLevelId", achievementLevelDTO.AchievementClassLevelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsValid", achievementLevelDTO.IsValid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", achievementLevelDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveDate", achievementLevelDTO.EffectiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", achievementLevelDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        // <summary>
        /// Converts the Data row object to AchievementLevelDTO class type
        /// </summary>
        /// <param name="achievementLevelDataRow">AchievementLevelDTO DataRow</param>
        /// <returns>Returns AchievementLevelDTO</returns>
        private AchievementLevelDTO GetAchievementLevelDTO(DataRow achievementLevelDataRow)
        {
            log.LogMethodEntry(achievementLevelDataRow);

            AchievementLevelDTO achievementLevelDTO = new AchievementLevelDTO(Convert.ToInt32(achievementLevelDataRow["Id"]),
                achievementLevelDataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(achievementLevelDataRow["CardId"]),
                achievementLevelDataRow["AchievementClassLevelId"] == DBNull.Value ? -1 : Convert.ToInt32(achievementLevelDataRow["AchievementClassLevelId"]),
                achievementLevelDataRow["IsValid"] == DBNull.Value ? false : Convert.ToBoolean(achievementLevelDataRow["IsValid"]),
                achievementLevelDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(achievementLevelDataRow["IsActive"]),
                achievementLevelDataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(achievementLevelDataRow["EffectiveDate"]),
                achievementLevelDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(achievementLevelDataRow["LastUpdatedDate"]),
                achievementLevelDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(achievementLevelDataRow["LastUpdatedUser"]),
                achievementLevelDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(achievementLevelDataRow["Guid"]),
                achievementLevelDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(achievementLevelDataRow["SynchStatus"]),
                achievementLevelDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(achievementLevelDataRow["MasterEntityId"]),
                achievementLevelDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(achievementLevelDataRow["site_id"]),
                achievementLevelDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(achievementLevelDataRow["CreatedBy"]),
                achievementLevelDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(achievementLevelDataRow["CreationDate"]));
            log.LogMethodExit(achievementLevelDTO);
            return achievementLevelDTO;
        }

        /// <summary>
        /// Gets the AchievementLevel data of passed id 
        /// </summary>
        /// <param name="id">id of AchievementLevel is passed as parameter</param>
        /// <returns>Returns AchievementLevelDTO</returns>
        public AchievementLevelDTO GetAchievementLevelDTO(int id)
        {
            log.LogMethodEntry(id);
            AchievementLevelDTO result = null;
            string query = SELECT_QUERY + @" WHERE al.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAchievementLevelDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the AchievementLevelDTO record
        /// </summary>
        /// <param name="achievementLevelDTO">AchievementLevelDTO is passed as parameter</param>

        internal int Delete(int Id)
        {
            log.LogMethodEntry(Id);
            string query = @"DELETE  
                             FROM AchievementLevel
                             WHERE AchievementLevel.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", Id);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Inserts the AchievementLevel record to the database
        /// </summary>
        /// <param name="achievementLevelDTO">AchievementLevelDTO type object</param>
        /// <returns>Returns inserted record id</returns>
        public AchievementLevelDTO InsertAchievementLevel(AchievementLevelDTO achievementLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementLevelDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[AchievementLevel]
                                                         (
                                                            CardId,
                                                            AchievementClassLevelId,
                                                            IsValid,
                                                            IsActive,
                                                            EffectiveDate,
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
                                                            @CardId,
                                                            @AchievementClassLevelId,
                                                            @IsValid,
                                                            @IsActive,
                                                            @EffectiveDate,
                                                            GetDate(),
                                                            @LastUpdatedUser,
                                                            NewId(), 
                                                            @MasterEntityId,
                                                            @site_id,
                                                            @CreatedBy,
                                                            GETDATE()
                                                        ) SELECT * FROM AchievementLevel WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementLevelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementLevelDTO(achievementLevelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AchievementLevelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementLevelDTO);
            return achievementLevelDTO;
        }

        /// <summary>
        /// Updates the AchievementLevel record to the database
        /// </summary>
        /// <param name="achievementLevelDTO">AchievementLevelDTO type object</param>
        /// <returns>Returns the count of updated rows</returns>
        public AchievementLevelDTO UpdateAchievementLevel(AchievementLevelDTO achievementLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementLevelDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AchievementLevel]
                           SET
                                CardId = @CardId,
                                AchievementClassLevelId = @AchievementClassLevelId,
                                IsValid = @IsValid,
                                IsActive = @IsActive,
                                EffectiveDate = @effectiveDate,
                                LastUpdatedDate = GetDate(),
                                LastUpdatedUser =  @LastUpdatedUser,
                                MasterEntityId = @MasterEntityId
                                --site_id = @site_id
                                    WHERE Id =@Id 
                                    SELECT * FROM AchievementLevel WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementLevelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementLevelDTO(achievementLevelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AchievementLevelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementLevelDTO);
            return achievementLevelDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementLevelDTO">AchievementLevelDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// 
        private void RefreshAchievementLevelDTO(AchievementLevelDTO achievementLevelDTO, DataTable dt)
        {
            log.LogMethodEntry(achievementLevelDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                achievementLevelDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                achievementLevelDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                achievementLevelDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                achievementLevelDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                achievementLevelDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                achievementLevelDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                achievementLevelDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AchievementLevel based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AchievementLevel</returns>
        public List<AchievementLevelDTO> GetAchievementClassLevelsList(List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AchievementLevelDTO> achievementLevelDTOList = new List<AchievementLevelDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AchievementLevelDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (
                           searchParameter.Key.Equals(AchievementLevelDTO.SearchByParameters.ID) ||
                           searchParameter.Key.Equals(AchievementLevelDTO.SearchByParameters.CARD_ID) ||
                           searchParameter.Key.Equals(AchievementClassDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                           searchParameter.Key.Equals(AchievementLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AchievementLevelDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    AchievementLevelDTO achievementLevelDTO = GetAchievementLevelDTO(dataRow);
                    achievementLevelDTOList.Add(achievementLevelDTO);
                }
            }
            log.LogMethodExit(achievementLevelDTOList);
            return achievementLevelDTOList;
        }
        /// <summary>
        /// GetAchievementLevelListExtended
        /// </summary>
        /// <param name="achievementParams"></param>
        /// <returns></returns>

        public List<AchievementLevelExtended> GetAchievementLevelListExtended(AchievementParams achievementParams)
        {
            log.LogMethodEntry(achievementParams);
            try
            {
                List<AchievementLevelExtended> achievementLevelDTOExtList = new List<AchievementLevelExtended>();
                string getAchievementlevelQuery = @"select ClassName,Picture,ac.GameId,  * 
                                                            from AchievementLevel al
                                                        inner join AchievementClassLevel acl on al.AchievementClassLevelId = acl.AchievementClassLevelId
                                                        inner join AchievementClass ac on ac.AchievementClassId = acl.AchievementClassId 
                                                        where CardId = @CardId  " + (achievementParams.ShowValidLevelsOnly == true ? " and IsValid = 1 " : " ") +
                                                        @" and al.AchievementClassLevelId 
                                                        in (select AchievementClassLevelId from
                                                        AchievementClassLevel 
                                                        where AchievementClassId = @AchievementClassId or  @AchievementClassId = -1)";

                SqlParameter[] getAchievementLevelParameters = new SqlParameter[2];
                getAchievementLevelParameters[0] = new SqlParameter("@CardId", achievementParams.CardId);
                getAchievementLevelParameters[1] = new SqlParameter("@AchievementClassId", achievementParams.AchievementClassId);

                DataTable dtAchLevel = dataAccessHandler.executeSelectQuery(getAchievementlevelQuery, getAchievementLevelParameters);

                if (dtAchLevel.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dtAchLevel.Rows)
                    {
                        AchievementLevelExtended achLevelExtended = new AchievementLevelExtended(dataRow["LevelName"].ToString(), GetAchievementLevelDTO(dataRow));
                        achLevelExtended.ClassName = dataRow["ClassName"].ToString();
                        achLevelExtended.MedalPicture = dataRow["Picture"].ToString();
                        achLevelExtended.IsPrimaryLevel = dataRow["GameId"] == DBNull.Value ? true : false;
                        achievementLevelDTOExtList.Add(achLevelExtended);
                    }
                }

                log.LogMethodExit(achievementLevelDTOExtList);
                return achievementLevelDTOExtList;
            }
            catch (Exception ex)
            {
                log.Error("Exception at GetAchievementLevelList()");
                log.LogMethodExit(null, "Throwing exception -" + ex.Message);
                throw;
            }
        }
    }
}


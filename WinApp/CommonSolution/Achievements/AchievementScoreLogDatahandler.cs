/********************************************************************************************
 * Project Name - Achievements
 * Description  - Data Handler -AchievementScoreLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        04-JUl-2019   Deeksha                 Modified :Added GetSqlParameter(),
 *                                                  SQL injection issue fixed,
 *                                                  changed log.debug to log.logMethodEntry
 *                                                  and log.logMethodExit
 *2.70.2      05-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.80        04-Mar-2020   Vikas Dwivedi           Modified as per the standards for Phase 1 changes.
 *2.130.0     22-Sep-2021   Mathew Ninan            Adding ScoringEventId to ScoreLog 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Achievements
{
    public class AchievementScoreLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AchievementScoreLog AS asl ";

        /// <summary>
        /// Dictionary for searching Parameters for the AchievementScoreLog object.
        /// </summary>
        private static readonly Dictionary<AchievementScoreLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AchievementScoreLogDTO.SearchByParameters, string>
            {
                    {AchievementScoreLogDTO.SearchByParameters.ID, "asl.Id"},
                    {AchievementScoreLogDTO.SearchByParameters.CARD_ID, "asl.CardId"},
                    {AchievementScoreLogDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID, "asl.AchievementClassId"},
                    {AchievementScoreLogDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID_LIST, "asl.AchievementClassId"},
                    {AchievementScoreLogDTO.SearchByParameters.MACHINE_ID, "asl.MachineId"} ,
                    {AchievementScoreLogDTO.SearchByParameters.CONVERTED_TO_ENTITLEMENT, "asl.ConvertedToEntitlement"},
                    {AchievementScoreLogDTO.SearchByParameters.IS_ACTIVE, "asl.IsActive"},
                    {AchievementScoreLogDTO.SearchByParameters.MASTER_ENTITY_ID, "asl.MasterentityId"},
            };

        /// <summary>
        /// Parameterized Constructor for AchievementScoreLogDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public AchievementScoreLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AchievementClass Record.
        /// </summary>
        /// <param name="AchievementScoreLogDTO">AchievementScoreLogDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(AchievementScoreLogDTO achievementScoreLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementScoreLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", achievementScoreLogDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", achievementScoreLogDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementClassId", achievementScoreLogDTO.AchievementClassId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineId", achievementScoreLogDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Score", achievementScoreLogDTO.Score));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", achievementScoreLogDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConvertedToEntitlement", achievementScoreLogDTO.ConvertedToEntitlement));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardCreditPlusId", achievementScoreLogDTO.CardCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", achievementScoreLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", achievementScoreLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventId", achievementScoreLogDTO.ScoringEventId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to AchievementScoreLogDTO class type
        /// </summary>
        /// <param name="achievementScoreLogDataRow">AchievementScoreLogDTO DataRow</param>
        /// <returns>Returns AchievementScoreLogDTO</returns>
        private AchievementScoreLogDTO GetAchievementScoreLogDTO(DataRow achievementScoreLogDataRow)
        {
            log.LogMethodEntry(achievementScoreLogDataRow);
            AchievementScoreLogDTO achievementScoreLogDTO = new AchievementScoreLogDTO
                (
                string.IsNullOrEmpty(achievementScoreLogDataRow["Id"].ToString()) ? -1 : Convert.ToInt32(achievementScoreLogDataRow["Id"]),
                string.IsNullOrEmpty(achievementScoreLogDataRow["CardId"].ToString()) ? -1 : Convert.ToInt32(achievementScoreLogDataRow["CardId"]),
                string.IsNullOrEmpty(achievementScoreLogDataRow["AchievementClassId"].ToString()) ? -1 : Convert.ToInt32(achievementScoreLogDataRow["AchievementClassId"]),
                string.IsNullOrEmpty(achievementScoreLogDataRow["MachineId"].ToString()) ? -1 : Convert.ToInt32(achievementScoreLogDataRow["MachineId"]),
                string.IsNullOrEmpty(achievementScoreLogDataRow["Score"].ToString()) ? -1 : Convert.ToDecimal(achievementScoreLogDataRow["Score"]),
                achievementScoreLogDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(achievementScoreLogDataRow["Timestamp"]),
                achievementScoreLogDataRow["ConvertedToEntitlement"] == DBNull.Value ? false : Convert.ToBoolean(achievementScoreLogDataRow["ConvertedToEntitlement"]),
                string.IsNullOrEmpty(achievementScoreLogDataRow["CardCreditPlusId"].ToString()) ? -1 : Convert.ToInt32(achievementScoreLogDataRow["CardCreditPlusId"]),
                achievementScoreLogDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(achievementScoreLogDataRow["IsActive"]),
                achievementScoreLogDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(achievementScoreLogDataRow["LastupdatedDate"]),
                achievementScoreLogDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(achievementScoreLogDataRow["LastUpdateduser"]),
                achievementScoreLogDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(achievementScoreLogDataRow["Guid"]),
                achievementScoreLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(achievementScoreLogDataRow["SynchStatus"]),
                achievementScoreLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(achievementScoreLogDataRow["MasterEntityId"]),
                achievementScoreLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(achievementScoreLogDataRow["site_id"]),
                achievementScoreLogDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(achievementScoreLogDataRow["CreatedBy"]),
                achievementScoreLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(achievementScoreLogDataRow["CreationDate"]),
                string.IsNullOrEmpty(achievementScoreLogDataRow["ScoringEventId"].ToString()) ? -1 : Convert.ToInt32(achievementScoreLogDataRow["ScoringEventId"])

                );
            log.LogMethodExit(achievementScoreLogDTO);
            return achievementScoreLogDTO;
        }

        /// <summary>
        /// Gets the AchievementScoreLog data of passed id 
        /// </summary>
        /// <param name="id">id of AchievementScoreLog is passed as parameter</param>
        /// <returns>Returns AchievementScoreLog</returns>
        public AchievementScoreLogDTO GetAchievementScoreLogDTO(int id)
        {
            log.LogMethodEntry(id);
            AchievementScoreLogDTO result = null;
            string query = SELECT_QUERY + @" WHERE asl.Id= @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAchievementScoreLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///  Inserts the record to the AchievementScoreLogDTO Table.
        /// </summary>
        /// <param name="AchievementScoreLogDTO">AchievementScoreLogDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the AchievementScoreLogDTO </returns>
        public AchievementScoreLogDTO InsertAchievementScoreLog(AchievementScoreLogDTO achievementScoreLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementScoreLogDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AchievementScoreLog]
                                                    (
                                                            CardId
                                                            ,AchievementClassId
                                                            ,MachineId
                                                            ,Score
                                                            ,Timestamp
                                                            ,ConvertedToEntitlement
                                                            ,CardCreditPlusId
                                                            ,IsActive
                                                            ,LastUpdatedDate
                                                            ,LastUpdatedUser
                                                            ,Guid                                                        
                                                            ,MasterEntityId
                                                            ,site_id 
                                                            ,CreatedBy
                                                            ,CreationDate
                                                            ,ScoringEventId
                                                         )
                                                       values
                                                         (
                                                             @CardId
                                                            ,@AchievementClassId
                                                            ,@MachineId
                                                            ,@Score
                                                            ,@Timestamp
                                                            ,@ConvertedToEntitlement
                                                            ,@CardCreditPlusId
                                                            ,@isActive
                                                            ,GETDATE()
                                                            ,@lastUpdatedUser
                                                            ,NewId()                                                         
                                                            ,@MasterEntityId
                                                            ,@site_id
                                                            ,@CreatedBy
                                                            ,GETDATE()
                                                            ,@ScoringEventId)
                                                         SELECT* FROM AchievementScoreLog WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementScoreLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementScoreLogDTO(achievementScoreLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AchievementScoreLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementScoreLogDTO);
            return achievementScoreLogDTO;
        }
        /// <summary>
        ///  Updates the record to the AchievementScoreLog Table.
        /// </summary>
        /// <param name="AchievementScoreLogDTO">AchievementScoreLogDTO object passed as parameter</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the AchievementScoreLogDTO</returns>
        public AchievementScoreLogDTO UpdateAchievementScoreLog(AchievementScoreLogDTO achievementScoreLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementScoreLogDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AchievementScoreLog]
                           SET 

                                                            CardId=@CardId,
                                                            AchievementClassId=@AchievementClassId,
                                                            MachineId=@MachineId,
                                                            Score=@Score,
                                                            Timestamp=@Timestamp,
                                                            ConvertedToEntitlement=@ConvertedToEntitlement,
                                                            CardCreditPlusId = @CardCreditPlusId,
                                                            IsActive = @isActive,
                                                            LastUpdatedDate = GetDate(),
                                                            LastUpdatedUser= @lastUpdatedUser,
                                                            MasterEntityId= @MasterEntityId,
                                                            ScoringEventId = @ScoringEventId
                                                           -- site_id = @site_id                                                                                                             
                                                            WHERE Id = @Id 
                                                                 SELECT * FROM AchievementScoreLog WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementScoreLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementScoreLogDTO(achievementScoreLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AchievementScoreLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementScoreLogDTO);
            return achievementScoreLogDTO;
        }

        /// <summary>
        /// Delete the record from the AchievementScoreLogDTO database based on Id
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int Id)
        {
            log.LogMethodEntry(Id);
            string query = @"DELETE  
                             FROM AchievementScoreLog
                             WHERE AchievementScoreLog.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", Id);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="AchievementScoreLogDTO">AchievementScoreLogDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshAchievementScoreLogDTO(AchievementScoreLogDTO achievementScoreLogDTO, DataTable dt)
        {
            log.LogMethodEntry(achievementScoreLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                achievementScoreLogDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                achievementScoreLogDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                achievementScoreLogDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                achievementScoreLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                achievementScoreLogDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                achievementScoreLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                achievementScoreLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the List of AchievementScoreLog based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AchievementScoreLog</returns>
        public List<AchievementScoreLogDTO> GetAchievementScoreLogDTOList(List<KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AchievementScoreLogDTO> achievementScoreLogDTOList = new List<AchievementScoreLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (
                           searchParameter.Key.Equals(AchievementScoreLogDTO.SearchByParameters.ID) ||
                           searchParameter.Key.Equals(AchievementScoreLogDTO.SearchByParameters.CARD_ID) ||
                           searchParameter.Key.Equals(AchievementScoreLogDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID) ||
                           searchParameter.Key.Equals(AchievementScoreLogDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                           searchParameter.Key.Equals(AchievementScoreLogDTO.SearchByParameters.MACHINE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(AchievementScoreLogDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
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
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AchievementScoreLogDTO achievementScoreLogDTO = GetAchievementScoreLogDTO(dataRow);
                    achievementScoreLogDTOList.Add(achievementScoreLogDTO);
                }
            }
            log.LogMethodExit(achievementScoreLogDTOList);
            return achievementScoreLogDTOList;
        }

    }
}

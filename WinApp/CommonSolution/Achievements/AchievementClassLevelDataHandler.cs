/********************************************************************************************
* Project Name - AchievementClassLevel Data Handler
* Description  - Data handler of the AchievementClassLevel class
* 
**************
**Version Log
**************
*Version     Date          Modified By              Remarks          
*********************************************************************************************
 *2.70        03-JUl-2019   Deeksha                 Modified :Added GetSqlParameter(), 
                                                    createdBy and creationDate fields  
                                                    changed log.debug to log.logMethodEntry
 *                                                  and log.logMethodExit
 *2.80        27-Aug-2019   Vikas Dwivedi           Added SqlTransaction,
 *                                                  Added SqlTransaction in the Constructor,
 *                                                  Modified executeInsertQuery, executeUpdateQuery,
 *                                                  executeSelectQuery and passed 
 *                                                  SqlTransaction as a parameter,
 *                                                  Added WHO columns.
 *2.80        19-Nov-2019   Vikas Dwivedi           Added Logger Method.  
 *2.70.2      05-Dec-2019   Jinto Thomas            Removed siteid from update query
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Achievements
{
    public class AchievementClassLevelDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AchievementClassLevel AS acl ";

        /// <summary>
        /// Dictionary for searching Parameters for the AchievementClassLevel object.
        /// </summary>
        private static readonly Dictionary<AchievementClassLevelDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AchievementClassLevelDTO.SearchByParameters, string>
            {
                {AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, "acl.AchievementClassLevelId"},
                {AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID_LIST, "acl.AchievementClassLevelId"},
                {AchievementClassLevelDTO.SearchByParameters.LEVEL_NAME, "acl.LevelName"},
                {AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID, "acl.AchievementClassId"},
                {AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID_LIST, "acl.AchievementClassId"},
                {AchievementClassLevelDTO.SearchByParameters.PARENT_LEVEL_ID, "acl.ParentLevelId"},
                {AchievementClassLevelDTO.SearchByParameters.QUALIFYING_LEVEL_ID, "acl.QualifyingLevelId"},
                {AchievementClassLevelDTO.SearchByParameters.REGISTRATION_REQUIRED, "acl.RegistrationRequired"},
                {AchievementClassLevelDTO.SearchByParameters.IS_ACTIVE, "acl.IsActive"},
                {AchievementClassLevelDTO.SearchByParameters.SITE_ID, "acl.site_id"},
                {AchievementClassLevelDTO.SearchByParameters.MASTER_ENTITY_ID, "acl.MasterEntityId"}
             };

        /// <summary>
        /// Parameterized Constructor for AchievementClassLevelDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public AchievementClassLevelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AchievementClassLevel Record.
        /// </summary>
        /// <param name="achievementClassLevelDTO">AchievementClassLevelDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(AchievementClassLevelDTO achievementClassLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementClassLevelDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementClassLevelId", achievementClassLevelDTO.AchievementClassLevelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LevelName", achievementClassLevelDTO.LevelName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementClassId", achievementClassLevelDTO.AchievementClassId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentLevelId", achievementClassLevelDTO.ParentLevelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QualifyingScore", Convert.ToDouble(achievementClassLevelDTO.QualifyingScore)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QualifyingLevelId", achievementClassLevelDTO.QualifyingLevelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RegistrationRequired", achievementClassLevelDTO.RegistrationRequired));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BonusEntitlement", achievementClassLevelDTO.BonusEntitlement));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BonusAmount", Convert.ToDouble(achievementClassLevelDTO.BonusAmount)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", achievementClassLevelDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Picture", achievementClassLevelDTO.Picture));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", achievementClassLevelDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", achievementClassLevelDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to AchievementClassLevelDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the AchievementClassLevelDTO</returns>

        private AchievementClassLevelDTO GetAchievementClassLevelDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AchievementClassLevelDTO achievementClassLevelDTO = new AchievementClassLevelDTO(dataRow["AchievementClassLevelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AchievementClassLevelId"]),
                                                          dataRow["LevelName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LevelName"]),
                                                          dataRow["AchievementClassId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AchievementClassId"]),
                                                          dataRow["ParentLevelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentLevelId"]),
                                                          dataRow["QualifyingScore"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["QualifyingScore"]),
                                                          dataRow["QualifyingLevelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["QualifyingLevelId"]),
                                                          dataRow["RegistrationRequired"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["RegistrationRequired"]),
                                                          dataRow["BonusEntitlement"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["BonusEntitlement"]),
                                                          dataRow["BonusAmount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["BonusAmount"]),
                                                          dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                          dataRow["Picture"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Picture"]),
                                                          dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                          dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]),
                                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                          dataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalSystemReference"]),
                                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]));
            log.LogMethodExit(achievementClassLevelDTO);
            return achievementClassLevelDTO;
        }

        /// <summary>
        /// Gets the AchievementClassLevel data of passed id 
        /// </summary>
        /// <param name="id">id of AchievementClassLevel is passed as parameter</param>
        /// <returns>Returns AchievementClassLevelDTO</returns>

        public AchievementClassLevelDTO GetAchievementClassLevelDTO(int id)
        {
            log.LogMethodEntry(id);
            AchievementClassLevelDTO result = null;
            string query = SELECT_QUERY + @" WHERE acl.AchievementClassLevelId = @AchievementClassLevelId";
            SqlParameter parameter = new SqlParameter("@AchievementClassLevelId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAchievementClassLevelDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the achievementClassLevel record
        /// </summary>
        /// <param name="AchievementClassLevelDTO">AchievementClassLevelDTO is passed as parameter</param>

        internal int Delete(int Id)
        {
            log.LogMethodEntry(Id);
            string query = @"DELETE  
                             FROM AchievementClassLevel
                             WHERE AchievementClassLevel.AchievementClassLevelId = @AchievementClassLevelId";
            SqlParameter parameter = new SqlParameter("@AchievementClassLevelId", Id);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        ///  Inserts the record to the AchievementClassLevelDTO Table.
        /// </summary>
        /// <param name="AchievementClassLevelDTO">AchievementClassLevelDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the AchievementClassLevelDTO </returns> 
        public AchievementClassLevelDTO InsertAchievementClassLevelDTO(AchievementClassLevelDTO achievementClassLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementClassLevelDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[AchievementClassLevel]
                           (LevelName,
                            AchievementClassId,
                            ParentLevelId,
                            QualifyingScore,
                            QualifyingLevelId,
                            RegistrationRequired,
                            BonusEntitlement,
                            BonusAmount,
                            ExternalSystemReference,
                            Picture,
                            IsActive,
                            LastUpdatedDate,
                            LastUpdatedUser,
                            Guid,
                            MasterEntityId,
                            site_id,
                            CreatedBy,
                            CreationDate)
                     VALUES
                           (@LevelName,
                            @AchievementClassId,
                            @ParentLevelId,
                            @QualifyingScore,
                            @QualifyingLevelId,
                            @RegistrationRequired,
                            @BonusEntitlement,
                            @BonusAmount,
                            @ExternalSystemReference,
                            @Picture,
                            @IsActive,
                            GETDATE(),
                            @LastUpdatedUser,
                            NEWID(),
                            @MasterEntityId,
                            @site_id,
                            @CreatedBy,
                            GETDATE())
                                    SELECT * FROM AchievementClasslevel WHERE AchievementClassLevelId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementClassLevelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementClassLevelDTO(achievementClassLevelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AchievementClassLevelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementClassLevelDTO);
            return achievementClassLevelDTO;
        }

        /// <summary>
        /// Updates the AchievementClassLevel record to the database
        /// </summary>
        /// <param name="achievementClassLevelDTO">AchievementClassLevelDTO type object</param>
        /// <returns>Returns the count of updated rows</returns>
        public AchievementClassLevelDTO UpdateAchievementClassLevel(AchievementClassLevelDTO achievementClassLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementClassLevelDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AchievementClassLevel]
                                               SET 
                                                            LevelName               =  @LevelName,
                                                            AchievementClassId      =  @AchievementClassId,
                                                            ParentLevelId           =  @ParentLevelId,
                                                            QualifyingScore         =  @QualifyingScore,
                                                            QualifyingLevelId       =  @QualifyingLevelId,
                                                            RegistrationRequired    =  @RegistrationRequired,
                                                            BonusEntitlement        =  @BonusEntitlement,
                                                            BonusAmount             =  @BonusAmount,
                                                            IsActive                =  @IsActive,
                                                            Picture                 =  @Picture,
                                                            LastUpdatedDate         =  GetDate(),
                                                            LastUpdatedUser         =  @LastUpdatedUser,                                                          
                                                            MasterEntityId          =  @MasterEntityId,
                                                            -- site_id                 =  @site_id,
                                                            ExternalSystemReference =  @ExternalSystemReference
                                                            WHERE AchievementClassLevelId = @AchievementClassLevelId 
                                                           SELECT * FROM AchievementClassLevel WHERE AchievementClassLevelId = @AchievementClassLevelId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementClassLevelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementClassLevelDTO(achievementClassLevelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating AchievementClassLevelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementClassLevelDTO);
            return achievementClassLevelDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementClassLevelDTO">AchievementClassLevelDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshAchievementClassLevelDTO(AchievementClassLevelDTO achievementClassLevelDTO, DataTable dt)
        {
            log.LogMethodEntry(achievementClassLevelDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                achievementClassLevelDTO.AchievementClassLevelId = Convert.ToInt32(dt.Rows[0]["AchievementClassLevelId"]);
                achievementClassLevelDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                achievementClassLevelDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                achievementClassLevelDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                achievementClassLevelDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdateduser"]);
                achievementClassLevelDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                achievementClassLevelDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AchievementClassLevelDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AchievementClassLevelDTO</returns>

        public List<AchievementClassLevelDTO> GetAchievementClassLevelsList(List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AchievementClassLevelDTO> achievementClassLevelDTOList = new List<AchievementClassLevelDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID
                            || searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID
                            || searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.PARENT_LEVEL_ID
                            || searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.QUALIFYING_LEVEL_ID
                            || searchParameter.Key.Equals(AchievementClassLevelDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.LEVEL_NAME)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                        }
                        else if (searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.SITE_ID)

                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.IS_ACTIVE ||
                                searchParameter.Key == AchievementClassLevelDTO.SearchByParameters.REGISTRATION_REQUIRED)    // bit
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
                    AchievementClassLevelDTO achievementClassLevelDTO = GetAchievementClassLevelDTO(dataRow);
                    achievementClassLevelDTOList.Add(achievementClassLevelDTO);
                }
            }
            log.LogMethodExit(achievementClassLevelDTOList);
            return achievementClassLevelDTOList;
        }

        ///// <summary>
        ///// Get AchievementClassLevel based on the list of AchievementClass Id.
        ///// </summary>
        ///// <param name="achievementClassIdList">achievementClassIdList holds the AchievementClass Id list</param>
        ///// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        ///// <returns>Returns the List of AchievementClassLevelDTO</returns>
        //public List<AchievementClassLevelDTO> GetAchievementClassLevelsList(List<int> achievementClassIdList, bool activeChildRecords)
        //{
        //    log.LogMethodEntry(achievementClassIdList, activeChildRecords);
        //    string query = SELECT_QUERY + " INNER JOIN @AchievementClassIdList List ON acl.AchievementClassId = List.Id ";
        //    if (activeChildRecords)
        //    {
        //        query += " AND ActiveFlag = 1 ";
        //    }
        //    DataTable dataTable = dataAccessHandler.BatchSelect(query, "@AchievementClassIdList", achievementClassIdList, null, sqlTransaction);
        //    List<AchievementClassLevelDTO> achievementClassLevelsList = GetAchievementClassLevelDTOList(dataTable);
        //    log.LogMethodExit(achievementClassLevelsList);
        //    return achievementClassLevelsList;
        //}

        ///// <summary>
        ///// Returns the List AchievementClassLevelDTO from the DataTable object.
        ///// </summary>
        ///// <param name="dataTable">dataTable object of DataTable is passed as parameter.</param>
        ///// <returns>Returns the List of AchievementClassLevelDTO </returns>
        //private List<AchievementClassLevelDTO> GetAchievementClassLevelDTOList(DataTable dataTable)
        //{
        //    log.LogMethodEntry(dataTable);
        //    List<AchievementClassLevelDTO> achievemetClassLevelsList = new List<AchievementClassLevelDTO>();
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            AchievementClassLevelDTO appUIPanelElementDTO = GetAchievementClassLevelDTO(dataRow);
        //            achievemetClassLevelsList.Add(appUIPanelElementDTO);
        //        }
        //    }
        //    log.LogMethodExit(achievemetClassLevelsList);
        //    return achievemetClassLevelsList;
        //}

    }
}

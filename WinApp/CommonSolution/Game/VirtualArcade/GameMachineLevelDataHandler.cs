/********************************************************************************************
 * Project Name - Game                                                                          
 * Description  - GameMachineLevelDataHandler class to manipulate game machine level details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
   *2.130.4     28-Feb-2022   Girish Kundar    Modified : Added two new columns AutoLoadEntitlement, EntitlementType
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Newtonsoft.Json;
namespace Semnox.Parafait.Game.VirtualArcade
{
    /// <summary>
    /// GameMachineLevelDataHandler
    /// </summary>
    public class GameMachineLevelDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM GameMachineLevels AS gml ";
        private static readonly Dictionary<GameMachineLevelDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<GameMachineLevelDTO.SearchByParameters, string>
            {
                {GameMachineLevelDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID, "gml.GameMachineLevelId"},
                {GameMachineLevelDTO.SearchByParameters.MACHINE_ID, "gml.MachineId"},
                {GameMachineLevelDTO.SearchByParameters.LEVEL_NAME, "gml.LevelName"},
                {GameMachineLevelDTO.SearchByParameters.IS_ACTIVE, "gml.IsActive"},
                {GameMachineLevelDTO.SearchByParameters.MASTER_ENTITY_ID, "gml.MasterEntityId"},
                {GameMachineLevelDTO.SearchByParameters.SITE_ID, "gml.site_id"}
            };
        /// <summary>
        /// Default constructor of GameMachineLevelDataHandler class
        /// </summary>
        public GameMachineLevelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating GameMachineLevelDTO Record.
        /// </summary>
        /// <param name="gameMachineLevelDTO">GameMachineLevelDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(GameMachineLevelDTO gameMachineLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gameMachineLevelDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameMachineLevelId", gameMachineLevelDTO.GameMachineLevelId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineId", gameMachineLevelDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LevelName", gameMachineLevelDTO.LevelName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LevelCharacteristics", gameMachineLevelDTO.LevelCharacteristics));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoreToXPRatio", gameMachineLevelDTO.ScoreToXPRatio));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoreToVPRatio", gameMachineLevelDTO.ScoreToVPRatio));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QualifyingScore", gameMachineLevelDTO.QualifyingScore));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", gameMachineLevelDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TranslationFileName", gameMachineLevelDTO.TranslationFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ImageFileName", gameMachineLevelDTO.ImageFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", gameMachineLevelDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AutoLoadEntitlement", gameMachineLevelDTO.AutoLoadEntitlement));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EntitlementType", gameMachineLevelDTO.EntitlementType));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="gameMachineLevelDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public GameMachineLevelDTO Insert(GameMachineLevelDTO gameMachineLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gameMachineLevelDTO, loginId, siteId);
            string insertQuery = @"insert into GameMachineLevels 
                                                        (                                                         
                                                       MachineId ,
                                                       LevelName,
                                                       LevelCharacteristics,
                                                       QualifyingScore ,
                                                       ScoreToVPRatio,
                                                       ScoreToXPRatio,
                                                       TranslationFileName ,
                                                       ImageFIleName ,
                                                       IsActive ,
                                                       CreatedBy ,
                                                       CreationDate ,
                                                       LastUpdatedBy ,
                                                       LastUpdatedDate ,
                                                       Guid ,
                                                       site_id   ,
                                                       MasterEntityId ,
                                                       AutoLoadEntitlement ,
                                                       EntitlementType   
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @MachineId ,
                                                       @LevelName,
                                                       @LevelCharacteristics,
                                                       @QualifyingScore ,
                                                       @ScoreToVPRatio,
                                                       @ScoreToXPRatio,
                                                       @TranslationFileName ,
                                                       @ImageFIleName ,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId ,
                                                       @AutoLoadEntitlement ,
                                                       @EntitlementType   
                                          )SELECT  * from GameMachineLevels where GameMachineLevelId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(gameMachineLevelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGameMachineLevelDTO(gameMachineLevelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting gameMachineLevelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gameMachineLevelDTO);
            return gameMachineLevelDTO;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameMachineLevelDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public GameMachineLevelDTO Update(GameMachineLevelDTO gameMachineLevelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gameMachineLevelDTO, loginId, siteId);
            string updateQuery = @"update GameMachineLevels 
                                         set 
                                            MachineId =  @MachineId ,
                                            LevelName= @LevelName,
                                            LevelCharacteristics=  @LevelCharacteristics,
                                            QualifyingScore = @QualifyingScore ,
                                            ScoreToVPRatio=  @ScoreToVPRatio,
                                            ScoreToXPRatio=@ScoreToXPRatio,
                                            TranslationFileName = @TranslationFileName ,
                                            ImageFIleName =  @ImageFIleName ,
                                            IsActive =   @IsActive ,
                                            LastUpdatedBy =  @LastUpdatedBy,
                                            LastUpdatedDate =  GetDate(),
                                            MasterEntityId =    @MasterEntityId ,
                                            AutoLoadEntitlement = @AutoLoadEntitlement ,
                                            EntitlementType = @EntitlementType 
                                               where  GameMachineLevelId =  @GameMachineLevelId  
                                    SELECT  * from GameMachineLevels where GameMachineLevelId = @GameMachineLevelId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(gameMachineLevelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGameMachineLevelDTO(gameMachineLevelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating gameMachineLevelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gameMachineLevelDTO);
            return gameMachineLevelDTO;
        }


        private void RefreshGameMachineLevelDTO(GameMachineLevelDTO gameMachineLevelDTO, DataTable dt)
        {
            log.LogMethodEntry(gameMachineLevelDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                gameMachineLevelDTO.GameMachineLevelId = Convert.ToInt32(dt.Rows[0]["GameMachineLevelId"]);
                gameMachineLevelDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                gameMachineLevelDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                gameMachineLevelDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                gameMachineLevelDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                gameMachineLevelDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                gameMachineLevelDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private GameMachineLevelDTO GetGameMachineLevelDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            GameMachineLevelDTO gameMachineLevelDTO = new GameMachineLevelDTO(Convert.ToInt32(dataRow["GameMachineLevelId"]),
                                                    dataRow["MachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MachineId"]),
                                                    dataRow["LevelName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LevelName"]),
                                                    dataRow["LevelCharacteristics"] == DBNull.Value ? string.Empty : GetDeSerializedString(Convert.ToString(dataRow["LevelCharacteristics"])),
                                                    dataRow["QualifyingScore"] == DBNull.Value ? (int?) null : Convert.ToInt32(dataRow["QualifyingScore"]),
                                                    dataRow["ScoreToVPRatio"] == DBNull.Value ? (decimal?)null : Convert.ToInt32(dataRow["ScoreToVPRatio"]),
                                                    dataRow["ScoreToXPRatio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ScoreToXPRatio"]),
                                                    dataRow["TranslationFileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TranslationFileName"]),
                                                    dataRow["ImageFileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ImageFileName"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["AutoLoadEntitlement"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AutoLoadEntitlement"]),
                                                    dataRow["EntitlementType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EntitlementType"])
                                                    );
            log.LogMethodExit(gameMachineLevelDTO);
            return gameMachineLevelDTO;
        }

        private string GetDeSerializedString(string jsonString)
        {
            log.LogMethodEntry(jsonString);
            try
            {
                var result = JsonConvert.DeserializeObject<string>(jsonString);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// Gets the GameMachineLevelDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns GameMachineLevelDTO</returns>
        internal GameMachineLevelDTO GetGameMachineLevelDTO(int id)
        {
            log.LogMethodEntry(id);
            GameMachineLevelDTO gameMachineLevelDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where gml.GameMachineLevelId = @GameMachineLevelId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@GameMachineLevelId", id);
            DataTable gameMachineLevelTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (gameMachineLevelTable.Rows.Count > 0)
            {
                DataRow gameMachineLevelRow = gameMachineLevelTable.Rows[0];
                gameMachineLevelDTO = GetGameMachineLevelDTO(gameMachineLevelRow);
            }
            log.LogMethodExit(gameMachineLevelDTO);
            return gameMachineLevelDTO;

        }

        internal decimal GetEntitlementValue(decimal virtualPoints, string entitlementType)
        {
            try
            {
                log.LogMethodEntry(virtualPoints, entitlementType);
                decimal redemptionValue = 0m;
                string redemptionRuleQuery = "select DBColumnName, attribute, convert(varchar, RedemptionValue) + ' for ' + convert(varchar, VirtualLoyaltyPoints) as \"Rule\", " +
                                    @"RedemptionValue * (case MultiplesOnly 
                                                when 'Y' then Convert(int, ((case when @virtualPoints < isnull(MinimumPoints, 0) then 
                                                 0 else @virtualPoints end) / (case when isnull(MinimumPoints, 1) = 0 
                                                then 1 else MinimumPoints end))) * (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end)
                                                else (case when @virtualPoints < isnull(MinimumPoints, 0) then 0 else @virtualPoints end) end)
                                                / case VirtualLoyaltyPoints when 0 then null else VirtualLoyaltyPoints end Redemption_value, " +
                                     "MinimumPoints \"Min Points\", MultiplesOnly \"Multiples Only\", RedemptionValue Rate, VirtualLoyaltyPoints " +
                                     "from LoyaltyRedemptionRule lrr, LoyaltyAttributes la " +
                                     "where lrr.LoyaltyAttributeId = la.LoyaltyAttributeId " +
                                     "and lrr.activeflag = 'Y' " +
                                     "and (lrr.ExpiryDate is null or lrr.ExpiryDate >= getdate())";

                SqlParameter[] selectUserParameters = new SqlParameter[1];
                selectUserParameters[0] = new SqlParameter("@virtualPoints", virtualPoints);
                DataTable redemptionRuleData = dataAccessHandler.executeSelectQuery(redemptionRuleQuery, selectUserParameters, sqlTransaction);
                if (redemptionRuleData.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in redemptionRuleData.Rows)
                    {
                        if (dataRow["attribute"].ToString() == "tickets")
                        {
                            entitlementType = "T";
                            redemptionValue = Convert.ToDecimal(dataRow["RedemptionValue"]);
                            log.Debug("Redemption_Value : " + redemptionValue);
                            if (redemptionValue <= 0)
                            {
                                log.LogMethodExit(redemptionValue);
                                return redemptionValue;
                            }
                        }
                        virtualPoints = redemptionValue / (Convert.ToDecimal(dataRow["Rate"]) / Convert.ToDecimal(dataRow["VirtualLoyaltyPoints"]));
                        log.Debug("virtualPoints : " + virtualPoints);
                    }
                }
                log.LogMethodExit(redemptionValue);
                return redemptionValue;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

        }

        /// <summary>
        /// GetGameMachineLevels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<GameMachineLevelDTO> GetGameMachineLevels(List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<GameMachineLevelDTO> gameMachineLevelDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<GameMachineLevelDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : "  and  ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == GameMachineLevelDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GameMachineLevelDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(GameMachineLevelDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(GameMachineLevelDTO.SearchByParameters.MACHINE_ID) ||
                                  searchParameter.Key.Equals(GameMachineLevelDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID) )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GameMachineLevelDTO.SearchByParameters.LEVEL_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                gameMachineLevelDTOList = new List<GameMachineLevelDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    GameMachineLevelDTO gameMachineLevelDTO = GetGameMachineLevelDTO(dataRow);
                    gameMachineLevelDTOList.Add(gameMachineLevelDTO);
                }
            }
            log.LogMethodExit(gameMachineLevelDTOList);
            return gameMachineLevelDTOList;
        }
    }
}

/********************************************************************************************
 * Project Name - Game Profile Data Handler                                                                          
 * Description  - Data handler of the game profile class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *1.10        11-Jan-2016   Mathew         Call to game profile attribute is done only if  
 *                                         context is GAME_PROFILE
 *2.40         04-09-2018    Jagan          Properties added the created_by,created_date in InsertGameProfile() and
                                           IS_ACTIVE enum key added in SearchByGameProfileParameters.
 *2.41        07-Nov-2018   Rajiv          Modified existing logic to handle null values.   
 *2.50.0      12-dec-2018   Guru S A       Who column changes
 *2.60        16-Apr-2019   Jagan Mohana   Added new property ProfileIdentifier
 *2.60.2      07-May-2019   Jagan Mohana   Created new DeleteGameProfile()
 *2.70        17-Jun-2019   Girish Kundar  Modified: Fix for the SQL Injection Issue 
 *2.70.2        26-Jul-2019   Deeksha        Modified Insert/Update functions to return DTO,
 *                                                   Added GetSqlParameter().
 *2.80.0       25-Mar-2020   Girish Kundar  Modified: DBAudit log changes                                                   
  *2.110.0      17-Dec-2020   Prajwal S      Modified for POS UI redesign using REST API.   
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Game profile data handler - Handles insert, update and select of game profile data objects
    /// </summary>
    public class GameProfileDataHandler
    {
        private const string SELECT_QUERY = @"SELECT * FROM game_profile AS gp ";
        /// <summary>
        /// Dictionary for searching Parameters for the AchievementClass object.
        /// </summary>
        private static readonly Dictionary<GameProfileDTO.SearchByGameProfileParameters, string> DBSearchParameters = new Dictionary<GameProfileDTO.SearchByGameProfileParameters, string>
            {
                {GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_NAME, "gp.profile_name"},
                {GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_ID, "gp.game_profile_id"},
                {GameProfileDTO.SearchByGameProfileParameters.SITE_ID, "gp.site_id"},
                {GameProfileDTO.SearchByGameProfileParameters.IS_ACTIVE, "gp.isActive"},
                {GameProfileDTO.SearchByGameProfileParameters.Master_Entity_Id, "gp.MasterEntityId"}
            };

        private DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of GameProfileDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public GameProfileDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Game Record.
        /// </summary>
        /// <param name="GameProfileDTO">GameDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(GameProfileDTO gameProfileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gameProfileDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameProfileId", gameProfileDTO.GameProfileId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@themeId", gameProfileDTO.ThemeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@profileName", gameProfileDTO.ProfileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@courtesyAllowed", gameProfileDTO.CourtesyAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@timeAllowed", gameProfileDTO.TimeAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tokenRedemptiom", gameProfileDTO.RedemptionToken));
            parameters.Add(dataAccessHandler.GetSQLParameter("@showAd", gameProfileDTO.ShowAd));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketEater", gameProfileDTO.IsTicketEater));
            parameters.Add(dataAccessHandler.GetSQLParameter("@physicalToken", gameProfileDTO.PhysicalToken));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bonusAllowed", gameProfileDTO.BonusAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@themeNumber", gameProfileDTO.ThemeNumber <= 0 ? DBNull.Value : (object)(gameProfileDTO.ThemeNumber)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketAllowedOnCredit", gameProfileDTO.TicketAllowedOnCredit));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketAllowedOnCourtesy", gameProfileDTO.TicketAllowedOnCourtesy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketAllowedOnBonus", gameProfileDTO.TicketAllowedOnBonus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketAllowedOnTime", gameProfileDTO.TicketAllowedOnTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@playCredits", gameProfileDTO.PlayCredits <= 0 ? DBNull.Value : (object)(gameProfileDTO.PlayCredits)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@vipPlayCredits", gameProfileDTO.VipPlayCredits <= 0 ? DBNull.Value : (object)(gameProfileDTO.VipPlayCredits)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@internetKey", gameProfileDTO.InternetKey <= 0 ? DBNull.Value : (object)(gameProfileDTO.InternetKey)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tokenPrice", gameProfileDTO.TokenPrice <= 0 ? DBNull.Value : (object)(gameProfileDTO.TokenPrice)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@redeemTokenTo", string.IsNullOrEmpty(gameProfileDTO.RedeemTokenTo) ? DBNull.Value : (object)gameProfileDTO.RedeemTokenTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserIdentifier", gameProfileDTO.UserIdentifier <= 0 ? DBNull.Value : (object)(gameProfileDTO.UserIdentifier)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@profileIdentifier", string.IsNullOrEmpty(gameProfileDTO.ProfileIdentifier) ? DBNull.Value : (object)gameProfileDTO.ProfileIdentifier));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomDataSetId", gameProfileDTO.CustomDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creditAllowed", gameProfileDTO.CreditAllowed, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", gameProfileDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", gameProfileDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ForceRedeemToCard", gameProfileDTO.ForceRedeemToCard));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastModUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the game profile data
        /// </summary>
        /// <param name="gameProfile">GameProfileDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public GameProfileDTO InsertGameProfile(GameProfileDTO gameProfile, string loginId, int siteId)
        {
            // Kiran - Logic to handle the custom data is not completed
            log.LogMethodEntry(gameProfile, loginId, siteId);
            string insertGameProfileQuery = @"insert into game_profile 
                                                            (
                                                              profile_name,
                                                              courtesy_allowed,
                                                              time_allowed, 
                                                              ticket_allowed_on_credit, 
                                                              ticket_allowed_on_courtesy,
                                                              ticket_allowed_on_bonus,
                                                              ticket_allowed_on_time, 
                                                              play_credits, 
                                                              vip_play_credits,
                                                              last_updated_date,
                                                              last_updated_user, 
                                                              InternetKey, 
                                                              TokenRedemption,
                                                              PhysicalToken,
                                                              TokenPrice,
                                                              RedeemTokenTo,
                                                              bonus_allowed,
                                                              ThemeNumber,
                                                              ThemeId,
                                                              ShowAd,
                                                              TicketEater,
                                                              Guid,
                                                              site_id,
                                                              credit_allowed,
                                                              UserIdentifier,
                                                              CustomDataSetId,
                                                              MasterEntityId,
                                                              creationdate,
                                                              createdby,
                                                              isActive,
                                                              ProfileIdentifier,
                                                              ForceRedeemToCard
                                                            ) 
                                                    values 
                                                            (
                                                              @profileName,
                                                              @courtesyAllowed,
                                                              @timeAllowed,
                                                              @ticketAllowedOnCredit,
                                                              @ticketAllowedOnCourtesy,
                                                              @ticketAllowedOnBonus,
                                                              @ticketAllowedOnTime,
                                                              @playCredits,
                                                              @vipPlayCredits,
                                                              GETDATE(),
                                                              @lastUpdatedUser, 
                                                              @internetKey, 
                                                              @tokenRedemptiom,
                                                              @physicalToken,
                                                              @tokenPrice,
                                                              @redeemTokenTo,
                                                              @bonusAllowed,
                                                              @themeNumber,
                                                              @themeId,
                                                              @showAd,
                                                              @ticketEater,
                                                              NEWID(),
                                                              @siteId,
                                                              @creditAllowed,
                                                              @UserIdentifier,
                                                              @customDataSetId,
                                                              @MasterEntityId,
                                                              GETDATE(),
                                                              @createdBy,
                                                              @IsActive,
                                                              @profileIdentifier,
                                                              @ForceRedeemToCard)
                                        SELECT * FROM game_profile WHERE game_profile_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertGameProfileQuery, GetSQLParameters(gameProfile, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGameProfileDTO(gameProfile, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting gameProfile", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }              
            log.LogMethodExit(gameProfile);
            return gameProfile;

        }


        /// <summary>
        /// Updates the game profile
        /// </summary>
        /// <param name="gameProfile">GameProfileDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public GameProfileDTO UpdateGameProfile(GameProfileDTO gameProfile, string loginId, int siteId)
        {
            log.LogMethodEntry(gameProfile, loginId, siteId);
            string updateGameProfileQuery = @"update game_profile 
                                                 set profile_name = @profileName,
                                                     courtesy_allowed = @courtesyAllowed,
                                                     time_allowed = @timeAllowed,  
                                                     ticket_allowed_on_credit = @ticketAllowedOnCredit,
                                                     ticket_allowed_on_courtesy = @ticketAllowedOnCourtesy,
                                                     ticket_allowed_on_bonus = @ticketAllowedOnBonus,
                                                     ticket_allowed_on_time = @ticketAllowedOnTime,
                                                     play_credits = @playCredits,
                                                     vip_play_credits = @vipPlayCredits,
                                                     last_updated_date = GETDATE(),
                                                     last_updated_user = @lastUpdatedUser, 
                                                     InternetKey = @internetKey, 
                                                     TokenRedemption = @tokenRedemptiom, 
                                                     PhysicalToken = @physicalToken,
                                                     TokenPrice = @tokenPrice,
                                                     RedeemTokenTo = @redeemTokenTo,
                                                     bonus_allowed = @bonusAllowed,
                                                     ThemeNumber = @themeNumber,
                                                     ThemeId = @themeId,
                                                     ShowAd = @showAd,
                                                     TicketEater = @ticketEater,
                                                     credit_allowed = @creditAllowed,
                                                     UserIdentifier = @UserIdentifier,
                                                     CustomDataSetId = @customDataSetId,
                                                     MasterEntityId = @MasterEntityId,
                                                     IsActive      = @IsActive,
                                                     ProfileIdentifier = @profileIdentifier,
                                                     ForceRedeemToCard = @ForceRedeemToCard
                                               where game_profile_id = @gameProfileId
                             SELECT * FROM game_profile WHERE game_profile_id = @gameProfileId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateGameProfileQuery, GetSQLParameters(gameProfile, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGameProfileDTO(gameProfile, dt);

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating gameProfile", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gameProfile);
            return gameProfile;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="GameProfileDTO">GameProfileDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshGameProfileDTO(GameProfileDTO gameProfile, DataTable dt)
        {
            log.LogMethodEntry(gameProfile, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                gameProfile.GameProfileId = Convert.ToInt32(dt.Rows[0]["game_profile_id"]);
                gameProfile.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                gameProfile.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                gameProfile.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                gameProfile.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                gameProfile.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                gameProfile.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Based on the gameProfileId, appropriate gameProfile record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="gameProfileId">primary key of gameProfileId </param>
        /// <returns>return the int </returns>
        internal int DeleteGameProfile(int gameProfileId)
        {
            log.LogMethodEntry(gameProfileId);
            try
            {
                string gameProfileQuery = @"delete from game_profile where game_profile_id = @gameProfileId";
                SqlParameter[] gameProfileParameters = new SqlParameter[1];
                gameProfileParameters[0] = new SqlParameter("@gameProfileId", gameProfileId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(gameProfileQuery, gameProfileParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// Converts the Data row object to GameProfileDTO class type
        /// </summary>
        /// <param name="gameProfileDataRow">Game Profile DataRow</param>
        /// <returns>Returns GameProfileDTO</returns>
        private GameProfileDTO GetGameProfileDTO(DataRow gameProfileDataRow)
        {
            log.LogMethodEntry(gameProfileDataRow);
            GameProfileDTO gameProfileDataObject = new GameProfileDTO(Convert.ToInt32(gameProfileDataRow["game_profile_id"]),
                                                                    gameProfileDataRow["profile_name"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["profile_name"]),
                                                                    gameProfileDataRow["credit_allowed"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["credit_allowed"]),
                                                                    gameProfileDataRow["bonus_allowed"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["bonus_allowed"]),
                                                                    gameProfileDataRow["courtesy_allowed"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["courtesy_allowed"]),
                                                                    gameProfileDataRow["time_allowed"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["time_allowed"]),
                                                                    gameProfileDataRow["ticket_allowed_on_credit"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["ticket_allowed_on_credit"]),
                                                                    gameProfileDataRow["ticket_allowed_on_courtesy"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["ticket_allowed_on_courtesy"]),
                                                                    gameProfileDataRow["ticket_allowed_on_bonus"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["ticket_allowed_on_bonus"]),
                                                                    gameProfileDataRow["ticket_allowed_on_time"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["ticket_allowed_on_time"]),
                                                                    gameProfileDataRow["play_credits"] == DBNull.Value ? 0.0 : Convert.ToDouble(gameProfileDataRow["play_credits"]),
                                                                    gameProfileDataRow["vip_play_credits"] == DBNull.Value ? 0.0 : Convert.ToDouble(gameProfileDataRow["vip_play_credits"]),
                                                                    gameProfileDataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gameProfileDataRow["last_updated_date"]),
                                                                    gameProfileDataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["last_updated_user"]),
                                                                    gameProfileDataRow["InternetKey"] == DBNull.Value ? 0 : Convert.ToInt32(gameProfileDataRow["InternetKey"]),
                                                                    gameProfileDataRow["TokenRedemption"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["TokenRedemption"]),
                                                                    gameProfileDataRow["PhysicalToken"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["PhysicalToken"]),
                                                                    gameProfileDataRow["TokenPrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(gameProfileDataRow["TokenPrice"]),
                                                                    gameProfileDataRow["RedeemTokenTo"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["RedeemTokenTo"]),
                                                                    gameProfileDataRow["ThemeNumber"] == DBNull.Value ? -1 : Convert.ToInt32(gameProfileDataRow["ThemeNumber"]),
                                                                    gameProfileDataRow["ThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(gameProfileDataRow["ThemeId"]),
                                                                    gameProfileDataRow["ShowAd"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["ShowAd"]),
                                                                    gameProfileDataRow["TicketEater"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["TicketEater"]),
                                                                    gameProfileDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["Guid"]),
                                                                    gameProfileDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(gameProfileDataRow["site_id"]),
                                                                    gameProfileDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(gameProfileDataRow["SynchStatus"]),
                                                                    gameProfileDataRow["UserIdentifier"] == DBNull.Value ? -1 : Convert.ToInt32(gameProfileDataRow["UserIdentifier"]),
                                                                    gameProfileDataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(gameProfileDataRow["CustomDataSetId"]),
                                                                    gameProfileDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(gameProfileDataRow["MasterEntityId"]),
                                                                    gameProfileDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(gameProfileDataRow["IsActive"]),
                                                                    gameProfileDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["CreatedBy"]),
                                                                    gameProfileDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gameProfileDataRow["CreationDate"]),
                                                                    gameProfileDataRow["ProfileIdentifier"] == DBNull.Value ? string.Empty : Convert.ToString(gameProfileDataRow["ProfileIdentifier"]),
                                                                    gameProfileDataRow["ForceRedeemToCard"] == DBNull.Value ? true : Convert.ToBoolean(gameProfileDataRow["ForceRedeemToCard"]));
            log.LogMethodExit(gameProfileDataObject);
            return gameProfileDataObject;
        }

        /// <summary>
        /// Gets the game profile data of passed game profile id
        /// </summary>
        /// <param name="gameProfileId">Game Profile Id</param>
        /// <returns>Returns GameProfileDTO</returns>
        public GameProfileDTO GetGameProfile(int gameProfileId)
        {
            log.LogMethodEntry(gameProfileId);
            try
            {
                string selectGameProfileQuery = @"select *
                                                from game_profile
                                               where game_profile_id = @gameProfileId";
                SqlParameter[] selectGameProfileParameters = new SqlParameter[1];
                selectGameProfileParameters[0] = new SqlParameter("@gameProfileId", gameProfileId);
                DataTable gameProfileData = dataAccessHandler.executeSelectQuery(selectGameProfileQuery, selectGameProfileParameters, sqlTransaction);

                if (gameProfileData.Rows.Count > 0)
                {
                    DataRow gameProfileDataRow = gameProfileData.Rows[0];
                    GameProfileDTO gameProfileDataObject = GetGameProfileDTO(gameProfileDataRow);
                    log.LogMethodExit(gameProfileDataObject);
                    return gameProfileDataObject;
                }
                else
                {
                    log.LogMethodExit(" Method by returning null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ;
            }
        }


        /// <summary>
        /// Gets the GameProfileDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of GameProfileDTO matching the search criteria</returns>
        public List<GameProfileDTO> GetGameProfileList(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters, 
                                                          int currentPage = 0, int pageSize = 0 )
        {
            log.LogMethodEntry(searchParameters);
            List<GameProfileDTO> gameProfileDTOList = new List<GameProfileDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage >= 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY gp.game_profile_id OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                gameProfileDTOList = new List<GameProfileDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GameProfileDTO GameProfileDTO = GetGameProfileDTO(dataRow);
                    gameProfileDTOList.Add(GameProfileDTO);
                }
            }
            log.LogMethodExit(gameProfileDTOList);
            return gameProfileDTOList;
        }

        /// <summary>
        /// Returns the no of GameProfile matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetGameProfilesCount(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int GameProfileDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                GameProfileDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(GameProfileDTOCount);
            return GameProfileDTOCount;
        }


        ///// <summary>
        ///// Gets the GameProfileDTO list matching the search key
        ///// </summary>
        ///// <param name="searchParameters">List of search parameters</param>
        ///// <param name="sqlTrxn">SqlTransaction object</param>
        ///// <returns>Returns the list of GameProfileDTO matching the search criteria</returns>
        //public List<GameProfileDTO> GetGameProfileList(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters)
        //{
        //    log.LogMethodEntry(searchParameters);
        //    List<GameProfileDTO> GameProfileDTOList = null;
        //    parameters.Clear();
        //    string selectQuery = SELECT_QUERY;
        //    selectQuery = selectQuery + GetFilterQuery(searchParameters);
        //    DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        GameProfileDTOList = new List<GameProfileDTO>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            GameProfileDTO GameProfileDTO = GetGameProfileDTO(dataRow);
        //            GameProfileDTOList.Add(GameProfileDTO);
        //        }
        //    }
        //    log.LogMethodExit(GameProfileDTOList);
        //    return GameProfileDTOList;
        //}

        /// <summary>
        /// Gets the GameProfileDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="loadAttributes">loadAttributes</param>
        /// <returns>Returns the list of GameProfileDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters)
        {

            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" where ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;

                foreach (KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : "  and ";

                        if (searchParameter.Key == GameProfileDTO.SearchByGameProfileParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_ID
                           || searchParameter.Key == GameProfileDTO.SearchByGameProfileParameters.Master_Entity_Id)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == GameProfileDTO.SearchByGameProfileParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

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

            }
                log.LogMethodExit();
                return query.ToString();
            }

        public DateTime? GetGameProfileModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(last_updated_date) LastUpdatedDate from game_profile WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from GameProfileAttributes WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from GameProfileAttributeValues WHERE (site_id = @siteId or @siteId = -1) and machine_id is null
                            ) a";
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
        /// <summary>
        /// Inserts the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="functionalGuid">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void AddManagementFormAccess(string formName, string functionGuid, int siteId, bool isActive)
        {
            log.LogMethodEntry(formName, functionGuid, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'Game Profile',@formName,'Data Access',@siteId,@functionGuid,@isActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", isActive));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Rename the managementFormAccess record to the database
        /// </summary>
        /// <param name="newFormName">string type object</param>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void RenameManagementFormAccess(string newFormName, string formName, int siteId, string functionGuid)
        {
            log.LogMethodEntry(newFormName, formName, siteId);
            string query = @"exec RenameManagementFormAccess @newFormName,'Game Profile',@formName,'Data Access',@siteId,@functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@newFormName", newFormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Update the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="updatedIsActive">Site to which the record belongs</param>
        /// <param name="functionGuid">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void UpdateManagementFormAccess(string formName, int siteId, bool updatedIsActive, string functionGuid)
        {
            log.LogMethodEntry(formName, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'Game Profile',@formName,'Data Access',@siteId,@functionGuid,@updatedIsActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedIsActive", updatedIsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
    }
}

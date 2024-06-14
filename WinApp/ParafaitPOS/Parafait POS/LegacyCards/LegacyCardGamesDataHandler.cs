/*/********************************************************************************************
 * Project Name - LegacyCardGamesDataHandler
 * Description  - Data Handler File for LegacyCardGames
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    public class LegacyCardGamesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LegacyCardGames as lcg ";
        private readonly DataAccessHandler dataAccessHandler;

        private static readonly Dictionary<LegacyCardGamesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LegacyCardGamesDTO.SearchByParameters, string>
            {
                {LegacyCardGamesDTO.SearchByParameters.LEGACY_CARD_GAME_ID, "lcg.LegacyCardGameId"},
                {LegacyCardGamesDTO.SearchByParameters.LEGACY_CARD_ID, "lcg.LegacyCard_Id"},
                {LegacyCardGamesDTO.SearchByParameters.SITE_ID, "lcg.site_id"},
                {LegacyCardGamesDTO.SearchByParameters.MASTER_ENTITY_ID, "lcg.MasterEntityId"},
                {LegacyCardGamesDTO.SearchByParameters.CARD_ID_LIST, "lcg.LegacyCard_Id"},
            };

        /// <summary>
        /// Default constructor of LegacyCardGamesDataHandler class
        /// </summary>
        public LegacyCardGamesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LegacyCardGame Record.
        /// </summary>
        /// <param name="LegacyCardGamesDTO">LegacyCardGamesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LegacyCardGamesDTO LegacyCardGamesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(LegacyCardGamesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LegacyCardGameId", LegacyCardGamesDTO.LegacyCardGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Legacygame_name", LegacyCardGamesDTO.LegacycardGame_name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LegacyCard_Id", LegacyCardGamesDTO.LegacyCard_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Quantity", LegacyCardGamesDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RevisedQuantity", LegacyCardGamesDTO.RevisedQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", LegacyCardGamesDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileName", LegacyCardGamesDTO.GameProfileName, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Frequency", LegacyCardGamesDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketAllowed", LegacyCardGamesDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", LegacyCardGamesDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Monday", LegacyCardGamesDTO.Monday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Tuesday", LegacyCardGamesDTO.Tuesday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Wednesday", LegacyCardGamesDTO.Wednesday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Thursday", LegacyCardGamesDTO.Thursday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Friday", LegacyCardGamesDTO.Friday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Saturday", LegacyCardGamesDTO.Saturday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sunday", LegacyCardGamesDTO.Sunday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", LegacyCardGamesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", LegacyCardGamesDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Site_Id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", LegacyCardGamesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the LegacyCardGames record to the database
        /// </summary>
        /// <param name="LegacyCardGamesDTO">LegacyCardGamesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted LegacyCardGame record</returns>
        public LegacyCardGamesDTO InsertLegacyCardGame(LegacyCardGamesDTO LegacyCardGamesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(LegacyCardGamesDTO, loginId, siteId);
            string query = @"INSERT INTO LegacyCardGames 
                                        ( 
                                            Legacygame_name
                                            ,LegacyCard_Id
                                            ,quantity
                                            ,RevisedQuantity
                                            ,ExpiryDate
                                            ,GameProfileName
                                            ,Frequency
                                            ,TicketAllowed
                                            ,FromDate
                                            ,MasterEntityId
                                            ,Monday
                                            ,Tuesday
                                            ,Wednesday
                                            ,Thursday
                                            ,Friday
                                            ,Saturday
                                            ,Sunday
                                            ,IsActive
                                            ,CreatedBy
                                            ,LastUpdatedBy
                                            ,site_id
                                            ,Guid
                                        ) 
                                VALUES 
                                        (
                                             @Legacygame_name
                                            ,@LegacyCard_Id
                                            ,@Quantity
                                            ,@RevisedQuantity
                                            ,@ExpiryDate
                                            ,@GameProfileName
                                            ,@Frequency
                                            ,@TicketAllowed
                                            ,@FromDate
                                            ,@MasterEntityId
                                            ,@Monday
                                            ,@Tuesday
                                            ,@Wednesday
                                            ,@Thursday
                                            ,@Friday
                                            ,@Saturday
                                            ,@Sunday
                                            ,@IsActive
                                            ,@CreatedBy
                                            ,@LastUpdatedBy
                                            ,@Site_Id
                                            ,Guid
                                        )
                                        SELECT * FROM LegacyCardGames WHERE LegacyCardGameId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(LegacyCardGamesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardGamesDTO(LegacyCardGamesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting LegacyCardGamesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(LegacyCardGamesDTO);
            return LegacyCardGamesDTO;
        }

        /// <summary>
        /// Updates the LegacyCardGame record
        /// </summary>
        /// <param name="LegacyCardGamesDTO">LegacyCardGamesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted LegacyCardGame record</returns>
        public LegacyCardGamesDTO UpdateLegacyCardGame(LegacyCardGamesDTO LegacyCardGamesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(LegacyCardGamesDTO, loginId, siteId);
            string query = @"UPDATE LegacyCardGames 
                                SET Legacygame_name = @Legacygame_name
                                   ,LegacyCard_Id = @LegacyCard_Id 
                                   ,quantity = @Quantity
                                   ,RevisedQuantity = @RevisedQuantity
                                   ,ExpiryDate = @ExpiryDate
                                   ,GameProfileName = @GameProfileName
                                   ,Frequency = @Frequency
                                   ,TicketAllowed = @TicketAllowed
                                   ,FromDate = @FromDate
                                   ,MasterEntityId = @MasterEntityId
                                   ,Monday = @Monday
                                   ,Tuesday = @Tuesday
                                   ,Wednesday = @Wednesday
                                   ,Thursday = @Thursday
                                   ,Friday = @Friday
                                   ,Saturday = @Saturday
                                   ,Sunday = @Sunday
                                   ,IsActive = @IsActive
                                   ,LastUpdatedBy = @LastUpdatedBy
                                   ,LastupdatedDate = GETDATE()
                             WHERE LegacyCardGameId = @LegacyCardGameId
                             SELECT * FROM LegacyCardGames WHERE LegacyCardGameId = @LegacyCardGameId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(LegacyCardGamesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardGamesDTO(LegacyCardGamesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LegacyCardGamesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(LegacyCardGamesDTO);
            return LegacyCardGamesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="LegacyCardGamesDTO">LegacyCardGamesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshLegacyCardGamesDTO(LegacyCardGamesDTO LegacyCardGamesDTO, DataTable dt)
        {
            log.LogMethodEntry(LegacyCardGamesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                LegacyCardGamesDTO.LegacyCardGameId = Convert.ToInt32(dt.Rows[0]["LegacyCardGameId"]);
                LegacyCardGamesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                LegacyCardGamesDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                LegacyCardGamesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                LegacyCardGamesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                LegacyCardGamesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                LegacyCardGamesDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to LegacyCardGamesDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns LegacyCardGamesDTO</returns>
        private LegacyCardGamesDTO GetLegacyCardGamesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LegacyCardGamesDTO LegacyCardGamesDTO = new LegacyCardGamesDTO(Convert.ToInt32(dataRow["LegacyCardGameId"]),
                                                         dataRow["LegacyCard_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LegacyCard_Id"]),
                                                         dataRow["Legacygame_name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Legacygame_name"]),
                                                         dataRow["quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["quantity"]),
                                                         dataRow["RevisedQuantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["RevisedQuantity"]),
                                                         dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                         dataRow["GameProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GameProfileName"]),
                                                         dataRow["Frequency"] == DBNull.Value ? "N" : Convert.ToString(dataRow["Frequency"]),
                                                         dataRow["TicketAllowed"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TicketAllowed"]),
                                                         dataRow["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["FromDate"]),
                                                         dataRow["Monday"] == DBNull.Value ? false : Convert.ToString(dataRow["Monday"]) == "Y",
                                                         dataRow["Tuesday"] == DBNull.Value ? false : Convert.ToString(dataRow["Tuesday"]) == "Y",
                                                         dataRow["Wednesday"] == DBNull.Value ? false : Convert.ToString(dataRow["Wednesday"]) == "Y",
                                                         dataRow["Thursday"] == DBNull.Value ? false : Convert.ToString(dataRow["Thursday"]) == "Y",
                                                         dataRow["Friday"] == DBNull.Value ? false : Convert.ToString(dataRow["Friday"]) == "Y",
                                                         dataRow["Saturday"] == DBNull.Value ? false : Convert.ToString(dataRow["Saturday"]) == "Y",
                                                         dataRow["Sunday"] == DBNull.Value ? false : Convert.ToString(dataRow["Sunday"]) == "Y",
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["IsActive"] == DBNull.Value ? true : (Convert.ToBoolean(dataRow["IsActive"])),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(LegacyCardGamesDTO);
            return LegacyCardGamesDTO;
        }

        /// <summary>
        /// Gets the legacyCard data of passed legacyCardGame Id
        /// </summary>
        /// <param name="legacyCardGameId">integer type parameter</param>
        /// <returns>Returns LegacyCardGamesDTO</returns>
        public LegacyCardGamesDTO GetLegacyCardGamesDTO(int legacyCardGameId)
        {
            log.LogMethodEntry(legacyCardGameId);
            LegacyCardGamesDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE lcg.LegacyCardGameId = @LegacyCardGameId";
            SqlParameter parameter = new SqlParameter("@LegacyCardGameId", legacyCardGameId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetLegacyCardGamesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the LegacyCardGamesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LegacyCardGamesDTO matching the search criteria</returns>
        public List<LegacyCardGamesDTO> GetLegacyCardGamesDTOList(List<KeyValuePair<LegacyCardGamesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LegacyCardGamesDTO> list = null;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LegacyCardGamesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key == LegacyCardGamesDTO.SearchByParameters.LEGACY_CARD_GAME_ID) ||
                             (searchParameter.Key == LegacyCardGamesDTO.SearchByParameters.LEGACY_CARD_ID) ||
                             (searchParameter.Key == LegacyCardGamesDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardGamesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardGamesDTO.SearchByParameters.ISACTIVE) //bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == LegacyCardGamesDTO.SearchByParameters.CARD_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<LegacyCardGamesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LegacyCardGamesDTO LegacyCardGamesDTO = GetLegacyCardGamesDTO(dataRow);
                    list.Add(LegacyCardGamesDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

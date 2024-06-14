/*/********************************************************************************************
 * Project Name - POS
 * Description  - Data Handler File for LegacyCard
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70.3      10-June-2019   Divya A                 Created 
 *2.100.0     03-Sep-2020    Dakshakh                Modified 
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_POS
{
    /// <summary>
    /// LegacyCard Data Handler - Handles insert, update and selection of LegacyCard objects
    /// </summary>
    public class LegacyCardDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM legacy_cards as lc ";

        /// <summary>
        /// Dictionary for searching Parameters for the TicketTemplateHeader object.
        /// </summary>
        private static readonly Dictionary<LegacyCardDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LegacyCardDTO.SearchByParameters, string>
        {
            { LegacyCardDTO.SearchByParameters.CARD_ID,"lc.card_id"},
            { LegacyCardDTO.SearchByParameters.CARD_NUMBER,"lc.card_number"},
            { LegacyCardDTO.SearchByParameters.CUSTOMER_ID,"lc.customer_id"},
            { LegacyCardDTO.SearchByParameters.VALID_FLAG,"lc.valid_flag"},
            { LegacyCardDTO.SearchByParameters.SITE_ID,"lc.site_id"},
            { LegacyCardDTO.SearchByParameters.MASTER_ENTITY_ID,"lc.MasterEntityId"},
            { LegacyCardDTO.SearchByParameters.CARD_NUMBER_OR_PRINTED_CARD_NUMBER,"lc.card_number"},
            { LegacyCardDTO.SearchByParameters.PARAFAIT_CARD_ID,"lc.transfer_to_card"},
            { LegacyCardDTO.SearchByParameters.TRX_ID,"lc.TrxId"}
        };

        /// <summary>
        /// Parameterized Constructor for LegacyCardDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public LegacyCardDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LegacyCard Record.
        /// </summary>
        /// <param name="legacyCardDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(LegacyCardDTO legacyCardDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(legacyCardDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_id", legacyCardDTO.CardId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_number", legacyCardDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@issue_date", legacyCardDTO.IssueDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@face_value", legacyCardDTO.FaceValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@refund_flag", legacyCardDTO.RefundFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@refund_amount", legacyCardDTO.RefundAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@refund_date", legacyCardDTO.RefundDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@valid_flag", legacyCardDTO.ValidFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticket_count", legacyCardDTO.TicketCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", legacyCardDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@credits", legacyCardDTO.Credits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@courtesy", legacyCardDTO.Courtesy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bonus", legacyCardDTO.Bonus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@time", legacyCardDTO.Time));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customer_id", legacyCardDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@credits_played", legacyCardDTO.CreditsPlayed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticket_allowed", legacyCardDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@real_ticket_mode", legacyCardDTO.RealTicketMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@vip_customer", legacyCardDTO.VipCustomer));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@start_time", legacyCardDTO.StartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_played_time", legacyCardDTO.LastPlayedTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@technician_card", legacyCardDTO.TechnicianCard));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tech_games", legacyCardDTO.TechGames));
            parameters.Add(dataAccessHandler.GetSQLParameter("@timer_reset_card", legacyCardDTO.TimerResetCard));
            parameters.Add(dataAccessHandler.GetSQLParameter("@loyalty_points", legacyCardDTO.LoyaltyPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardTypeId", legacyCardDTO.CardTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", legacyCardDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@upload_site_id", legacyCardDTO.UploadSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@upload_time", legacyCardDTO.UploadTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", legacyCardDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", legacyCardDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@transferred", legacyCardDTO.Transferred));
            parameters.Add(dataAccessHandler.GetSQLParameter("@transfer_to_card", legacyCardDTO.TransferToCard));
            parameters.Add(dataAccessHandler.GetSQLParameter("@transfer_date", legacyCardDTO.TransferDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_purchase_date", legacyCardDTO.LastPurchaseDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@transferred_cardgames", legacyCardDTO.TransferredCardgames));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Temp_Card_id", legacyCardDTO.TempCardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Temp_Card_Number", legacyCardDTO.TempCardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", legacyCardDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printed_card_number", legacyCardDTO.PrintedCardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_face_value", legacyCardDTO.RevisedFaceValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_ticket_count", legacyCardDTO.RevisedTicketCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_credits", legacyCardDTO.RevisedCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_courtesy", legacyCardDTO.RevisedCourtesy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_bonus", legacyCardDTO.RevisedBonus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_time", legacyCardDTO.RevisedTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_credits_played", legacyCardDTO.RevisedCreditsPlayed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_last_played_time", legacyCardDTO.RevisedLastPlayedTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Revised_loyalty_points", legacyCardDTO.RevisedLoyaltyPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RevisedBy", legacyCardDTO.RevisedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApprovedBy", legacyCardDTO.ApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardClearedDate", legacyCardDTO.CardClearedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardClearedBy", legacyCardDTO.CardClearedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", legacyCardDTO.TrxId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        private LegacyCardDTO GetLegacyCardDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LegacyCardDTO legacyCardDTO = new LegacyCardDTO(
                                                dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                                dataRow["card_number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["card_number"]),
                                                dataRow["issue_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["issue_date"]),
                                                dataRow["face_value"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["face_value"]),
                                                dataRow["refund_flag"] == DBNull.Value ? 'Y' : Convert.ToChar(dataRow["refund_flag"]),
                                                dataRow["refund_amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["refund_amount"]),
                                                dataRow["refund_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["refund_date"]),
                                                dataRow["valid_flag"] == DBNull.Value ? 'Y' : Convert.ToChar(dataRow["valid_flag"]),
                                                dataRow["ticket_count"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ticket_count"]),
                                                dataRow["notes"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["notes"]),
                                                dataRow["last_update_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_time"]),
                                                dataRow["credits"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["credits"]),
                                                dataRow["courtesy"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["courtesy"]),
                                                dataRow["bonus"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["bonus"]),
                                                dataRow["time"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["time"]),
                                                dataRow["customer_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["customer_id"]),
                                                dataRow["credits_played"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["credits_played"]),
                                                dataRow["ticket_allowed"] == DBNull.Value ? 'Y' : Convert.ToChar(dataRow["ticket_allowed"]),
                                                dataRow["real_ticket_mode"] == DBNull.Value ? 'Y' : Convert.ToChar(dataRow["real_ticket_mode"]),
                                                dataRow["vip_customer"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["vip_customer"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["start_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["start_time"]),
                                                dataRow["last_played_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_played_time"]),
                                                dataRow["technician_card"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["technician_card"]),
                                                dataRow["tech_games"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["tech_games"]),
                                                dataRow["timer_reset_card"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["timer_reset_card"]),
                                                dataRow["loyalty_points"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["loyalty_points"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["CardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardTypeId"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["upload_site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["upload_site_id"]),
                                                dataRow["upload_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["upload_time"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["ExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                dataRow["status"] == DBNull.Value ? 'S' : Convert.ToChar(dataRow["status"]),
                                                dataRow["transferred"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["transferred"]),
                                                dataRow["transfer_to_card"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["transfer_to_card"]),
                                                dataRow["transfer_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["transfer_date"]),
                                                dataRow["last_purchase_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_purchase_date"]),
                                                dataRow["transferred_cardgames"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["transferred_cardgames"]),
                                                dataRow["Temp_Card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Temp_Card_id"]),
                                                dataRow["Temp_Card_Number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Temp_Card_Number"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["printed_card_number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["printed_card_number"]),
                                                dataRow["Revised_face_value"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Revised_face_value"]),
                                                dataRow["Revised_ticket_count"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Revised_ticket_count"]),
                                                dataRow["Revised_credits"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Revised_credits"]),
                                                dataRow["Revised_courtesy"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Revised_courtesy"]),
                                                dataRow["Revised_bonus"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Revised_bonus"]),
                                                dataRow["Revised_time"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Revised_time"]),
                                                dataRow["Revised_credits_played"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Revised_credits_played"]),
                                                dataRow["Revised_last_played_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Revised_last_played_time"]),
                                                dataRow["Revised_loyalty_points"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Revised_loyalty_points"]),
                                                dataRow["RevisedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RevisedBy"]),
                                                dataRow["ApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ApprovedBy"]),
                                                dataRow["CardClearedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CardClearedDate"]),
                                                dataRow["CardClearedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CardClearedBy"]),
                                                dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]));
            return legacyCardDTO;
        }

        /// <summary>
        /// Gets the LegacyCard data of passed LegacyCard ID
        /// </summary>
        /// <param name="legacyCardId"></param>
        /// <returns>Returns LegacyCardDTO</returns>
        public LegacyCardDTO GetLegacyCardDTO(int legacyCardId)
        {
            log.LogMethodEntry(legacyCardId);
            LegacyCardDTO result = null;
            string query = SELECT_QUERY + @" WHERE lc.card_id = @card_id";
            SqlParameter parameter = new SqlParameter("@card_id", legacyCardId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLegacyCardDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void RefreshLegacyCardDTO(LegacyCardDTO legacyCardDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(legacyCardDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                legacyCardDTO.CardId = Convert.ToInt32(dt.Rows[0]["card_id"]);
                legacyCardDTO.LastUpdateTime = Convert.ToDateTime(dt.Rows[0]["last_update_time"]);
                legacyCardDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                legacyCardDTO.LastUpdatedBy = loginId;
                legacyCardDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the LegacyCard Table. 
        /// </summary>
        /// <param name="legacyCardDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated LegacyCardDTO</returns>
        public LegacyCardDTO Insert(LegacyCardDTO legacyCardDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(legacyCardDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[legacy_cards]
                            (
                            card_number,
                            issue_date,
                            face_value,
                            refund_flag,
                            refund_amount,
                            refund_date,
                            valid_flag,
                            ticket_count,
                            notes,
                            last_update_time,
                            credits,
                            courtesy,
                            bonus,
                            time,
                            customer_id,
                            credits_played,
                            ticket_allowed,
                            real_ticket_mode,
                            vip_customer,
                            site_id,
                            start_time,
                            last_played_time,
                            technician_card,
                            tech_games,
                            timer_reset_card,
                            loyalty_points,
                            LastUpdatedBy,
                            CardTypeId,
                            Guid,
                            upload_site_id,
                            upload_time,
                            ExpiryDate,
                            status,
                            transferred,
                            transfer_to_card,
                            transfer_date,
                            last_purchase_date,
                            transferred_cardgames,
                            Temp_Card_id,
                            Temp_Card_Number,
                            MasterEntityId,
                            printed_card_number,
                            Revised_face_value,
                            Revised_ticket_count,
                            Revised_credits,
                            Revised_courtesy,
                            Revised_bonus,
                            Revised_time,
                            Revised_credits_played,
                            Revised_last_played_time,
                            Revised_loyalty_points,
                            RevisedBy,
                            ApprovedBy,
                            CardClearedDate,
                            CardClearedBy,
                            TrxId
                            )
                            VALUES
                            (
                            @card_number,
                            @issue_date,
                            @face_value,
                            @refund_flag,
                            @refund_amount,
                            @refund_date,
                            @valid_flag,
                            @ticket_count,
                            @notes,
                            GETDATE(),
                            @credits,
                            @courtesy,
                            @bonus,
                            @time,
                            @customer_id,
                            @credits_played,
                            @ticket_allowed,
                            @real_ticket_mode,
                            @vip_customer,
                            @site_id,
                            @start_time,
                            @last_played_time,
                            @technician_card,
                            @tech_games,
                            @timer_reset_card,
                            @loyalty_points,
                            @LastUpdatedBy,
                            @CardTypeId,
                            NEWID(),
                            @upload_site_id,
                            @upload_time,
                            @ExpiryDate,
                            @status,
                            @transferred,
                            @transfer_to_card,
                            @transfer_date,
                            @last_purchase_date,
                            @transferred_cardgames,
                            @Temp_Card_id,
                            @Temp_Card_Number,
                            @MasterEntityId,
                            @printed_card_number,
                            @Revised_face_value,
                            @Revised_ticket_count,
                            @Revised_credits,
                            @Revised_courtesy,
                            @Revised_bonus,
                            @Revised_time,
                            @Revised_credits_played,
                            @Revised_last_played_time,
                            @Revised_loyalty_points,
                            @RevisedBy,
                            @ApprovedBy,
                            @CardClearedDate,
                            @CardClearedBy,
                            @TrxId
                            )
                            SELECT * FROM legacy_cards WHERE card_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(legacyCardDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardDTO(legacyCardDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LegacyCardDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(legacyCardDTO);
            return legacyCardDTO;
        }

        /// <summary>
        /// Update the record in the LegacyCard Table. 
        /// </summary>
        /// <param name="legacyCardDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated LegacyCardDTO</returns>
        public LegacyCardDTO Update(LegacyCardDTO legacyCardDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(legacyCardDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[legacy_cards]
                             SET
                             card_number = @card_number,
                             issue_date = @issue_date,
                             face_value = @face_value,
                             refund_flag = @refund_flag,
                             refund_amount = @refund_amount,
                             refund_date = @refund_date,
                             valid_flag = @valid_flag,
                             ticket_count = @ticket_count,
                             notes = @notes,
                             last_update_time = GETDATE(),
                             credits = @credits,
                             courtesy = @courtesy,
                             bonus = @bonus,
                             time = @time,
                             customer_id = @customer_id,
                             credits_played = @credits_played,
                             ticket_allowed = @ticket_allowed,
                             real_ticket_mode = @real_ticket_mode,
                             vip_customer = @vip_customer,
                             start_time = @start_time,
                             last_played_time = @last_played_time,
                             technician_card = @technician_card,
                             tech_games = @tech_games,
                             timer_reset_card = @timer_reset_card,
                             loyalty_points = @loyalty_points,
                             LastUpdatedBy = @LastUpdatedBy,
                             CardTypeId = @CardTypeId,
                             upload_site_id = @upload_site_id,
                             upload_time = @upload_time,
                             ExpiryDate = @ExpiryDate,
                             status = @status,
                             transferred = @transferred,
                             transfer_to_card = @transfer_to_card,
                             transfer_date = @transfer_date,
                             last_purchase_date = @last_purchase_date,
                             transferred_cardgames = @transferred_cardgames,
                             Temp_Card_id = @Temp_Card_id,
                             Temp_Card_Number = @Temp_Card_Number,
                             MasterEntityId = @MasterEntityId,
                             printed_card_number = @printed_card_number,
                             Revised_face_value = @Revised_face_value,
                             Revised_ticket_count = @Revised_ticket_count,
                             Revised_credits = @Revised_credits,
                             Revised_courtesy = @Revised_courtesy,
                             Revised_bonus = @Revised_bonus,
                             Revised_time = @Revised_time,
                             Revised_credits_played = @Revised_credits_played,
                             Revised_last_played_time = @Revised_last_played_time,
                             Revised_loyalty_points = @Revised_loyalty_points,
                             RevisedBy = @RevisedBy,
                             ApprovedBy = @ApprovedBy,
                             CardClearedDate = @CardClearedDate,
                             CardClearedBy = @CardClearedBy,
                             TrxID = @TrxId
                             WHERE card_id = @card_id
                            SELECT * FROM legacy_cards WHERE card_id = @card_id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(legacyCardDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardDTO(legacyCardDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LegacyCardDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(legacyCardDTO);
            return legacyCardDTO;
        }

        /// <summary>
        /// Returns the List of LegacyCard based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<LegacyCardDTO> GetLegacyCardDTO(List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LegacyCardDTO> legacyCardDTOList = new List<LegacyCardDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LegacyCardDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LegacyCardDTO.SearchByParameters.CARD_ID ||
                            searchParameter.Key == LegacyCardDTO.SearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == LegacyCardDTO.SearchByParameters.PARAFAIT_CARD_ID ||
                            searchParameter.Key == LegacyCardDTO.SearchByParameters.TRX_ID ||
                            searchParameter.Key == LegacyCardDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardDTO.SearchByParameters.VALID_FLAG)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToChar(searchParameter.Value)));
                        }
                        else if ((searchParameter.Key == LegacyCardDTO.SearchByParameters.CARD_NUMBER))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if ((searchParameter.Key == LegacyCardDTO.SearchByParameters.CARD_NUMBER_OR_PRINTED_CARD_NUMBER))
                        {
                            query.Append(joiner + "( lc.card_number =" + dataAccessHandler.GetParameterName(searchParameter.Key) + " OR lc.printed_card_number =" + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == LegacyCardDTO.SearchByParameters.SITE_ID)
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
                    LegacyCardDTO legacyCardDTO = GetLegacyCardDTO(dataRow);
                    legacyCardDTOList.Add(legacyCardDTO);
                }
            }
            log.LogMethodExit(legacyCardDTOList);
            return legacyCardDTOList;
        }

        public DataTable GetSacoaPackages(string legacyCardNumber, int cardTime = 0)
        {
            string query = @"select data.CardNumber, data.Quantity, lp.LegacyProductId, lp.LegacyProductName,
                                        lp.ParafaitProductId, lp.ParafaitProductName, isnull(pt.Duration, 0) Duration
                                    from Sacoa_datPassportData data, Sacoa_cnfPassportTypes pt, LegacyToParafaitProductMapping lp
                                    where data.PassportID = pt.ID
                                    and lp.LegacyProductId = pt.ID
                                    and ((pt.Duration > 0 
		                                    and not exists (select 1
				                                        from Sacoa_logHyperPassportTransactions t
				                                        where t.PassportID = data.ID))
	                                    or (pt.Duration = 0 and data.Quantity > 0))
                                    and pt.Deleted = 0
                                    and data.DateIssued > '1-feb-2013'
                                    and data.CardNumber = @cardNumber
                                    union all
                                    select top 1 @cardNumber, 0, lp.LegacyProductId, lp.LegacyProductName, 
                                        lp.ParafaitProductId, lp.ParafaitProductName, -1
                                    from LegacyToParafaitProductMapping lp
                                    where lp.LegacyProductId = -1
                                    and @cardTime > 0";
            List<SqlParameter> parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@cardNumber", legacyCardNumber));
            parameter.Add(new SqlParameter("@cardTime", cardTime));
            DataTable dtGetSacoaPackages = null;
            dtGetSacoaPackages = dataAccessHandler.executeSelectQuery(query, parameter.ToArray(), sqlTransaction);
            if (dtGetSacoaPackages.Rows.Count > 0)
            {
                return dtGetSacoaPackages;
            }
            else
            {
                log.LogMethodExit(dtGetSacoaPackages);
                return dtGetSacoaPackages;
            }
        }

        public DataTable GetLegacyCardsDTOList(string legacyCardNumber, string parafaitCardNumber)
        {
            log.LogMethodEntry(legacyCardNumber, parafaitCardNumber);
            string query = string.Empty;
            string queryJoiner = string.Empty;
            List<SqlParameter> parameter = new List<SqlParameter>();
            if (!string.IsNullOrWhiteSpace(parafaitCardNumber))
            {
                queryJoiner = " and c.card_number like @parafaitCardNumber + '%'";
            }
            query = @"select l.*, c.card_number parafaitcardnumber " +
                                 "from legacy_cards l left outer join cards c on l.transfer_to_card = c.card_id " +
                                 "where l.card_number like @legacyCardNumber + '%'" +
                                 queryJoiner +
                                 " order by transfer_date desc";
            parameter.Add(new SqlParameter("@legacyCardNumber", legacyCardNumber));
            parameter.Add(new SqlParameter("@parafaitCardNumber", parafaitCardNumber));
            DataTable dtLegacyCardsDTOList = null;
            dtLegacyCardsDTOList = dataAccessHandler.executeSelectQuery(query, parameter.ToArray(), sqlTransaction);
            if (dtLegacyCardsDTOList.Rows.Count > 0)
            {
                return dtLegacyCardsDTOList;
            }
            else
            {
                log.LogMethodExit(dtLegacyCardsDTOList);
                return dtLegacyCardsDTOList;
            }
        }
    }
}

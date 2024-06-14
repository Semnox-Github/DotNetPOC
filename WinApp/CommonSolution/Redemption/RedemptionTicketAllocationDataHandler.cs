/********************************************************************************************
 * Project Name - Redemption
 * Description  - Data handler of the RedemptionTicketAllocation 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.3.0       25-Jun-2018   Archana/Guru S A  Created 
 *2.70.2        19-Jul-2019   Deeksha           Modified: Sql injection issue fix
 *2.70.2        16-Sep-2019   Dakshakh raj      Redemption currency rule enhancement
 *2.70.2        10-Dec-2019   Jinto Thomas      Removed siteid from update query
 *2.70.3        30-Jan-2020   Archana           Modified to include MANUAL_TICKETS_PER_DAY_BY_LOGIN_ID search parameter 
 *                                              to get manual ticket count added by specific user
 *2.100.0     31-Aug-2020   Mushahid Faizan      siteId changes in GetSQLParameters().                                             
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// RedemptionTicketAllocationDataHandler
    /// </summary>
    public class RedemptionTicketAllocationDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RedemptionTicketAllocation AS rta ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary for searching Parameters for the DisplayPanel object.
        /// </summary>
        private static readonly Dictionary<RedemptionTicketAllocationDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RedemptionTicketAllocationDTO.SearchByParameters, string>
            {
                {RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_TICKET_ALLOCATION_ID, "rta.Id"},
                {RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID, "rta.RedemptionId"},
                {RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_GIFT_ID, "rta.RedemptionGiftId"},
                {RedemptionTicketAllocationDTO.SearchByParameters.CURRENCY_ID, "rta.CurrencyId"},
                {RedemptionTicketAllocationDTO.SearchByParameters.MANUAL_TICKET_RECEIPT_ID, "rta.ManualTicketReceiptId"},
                {RedemptionTicketAllocationDTO.SearchByParameters.TRX_ID, "rta.TrxId"},
                {RedemptionTicketAllocationDTO.SearchByParameters.MANUAL_TICKET_RECEIPT_NO, "rta.ManualTicketReceiptNo"},
                {RedemptionTicketAllocationDTO.SearchByParameters.MASTER_ENTITY_ID,"rta.MasterEntityId"},
                {RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, "rta.site_id"},
                {RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_CURRENCY_RULE_ID, "rta.RedemptionCurrencyRuleId"},
                {RedemptionTicketAllocationDTO.SearchByParameters.SOURCE_CURRENCY_RULE_ID, "rta.SourceCurrencyRuleId"},
                {RedemptionTicketAllocationDTO.SearchByParameters.MANUAL_TICKETS_PER_DAY_BY_LOGIN_ID, "rta.CreatedBy"}
            };
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS RedemptionTicketAllocationType;
                                            MERGE INTO RedemptionTicketAllocation tbl
                                            USING @RedemptionTicketAllocationList AS src
                                            ON src.ID = tbl.ID
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                             RedemptionId=src.RedemptionId,
                                             RedemptionGiftId=src.RedemptionGiftId,
                                             ManualTickets=src.ManualTickets,
                                             GraceTickets=src.GraceTickets,
                                             CardId=src.CardId, 
                                             ETickets=src.ETickets,
                                             CurrencyId=src.CurrencyId,
                                             CurrencyQuantity=src.CurrencyQuantity,
                                             CurrencyTickets=src.CurrencyTickets,
                                             ManualTicketReceiptNo=src.ManualTicketReceiptNo,
                                             ReceiptTickets=src.ReceiptTickets,
                                             TurnInTickets=src.TurnInTickets,
                                             LastUpdatedBy=src.LastUpdatedBy,
                                             LastUpdatedDate=getdate(), 
                                             ManualTicketReceiptId=src.ManualTicketReceiptId,
                                             TrxId=src.TrxId,
                                             TrxLineId=src.TrxLineId,
                                             RedemptionCurrencyRuleId=src.RedemptionCurrencyRuleId,
                                             RedemptionCurrencyRuleTicket=src.RedemptionCurrencyRuleTicket,
                                             SourceCurrencyRuleId=src.SourceCurrencyRuleId,
                                            MasterEntityId = src.MasterEntityId--,
                                            --site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            RedemptionId,
                                            RedemptionGiftId,
                                            ManualTickets,
                                            GraceTickets,
                                            CardId,
                                            ETickets,
                                            CurrencyId,
                                            CurrencyQuantity,
                                            CurrencyTickets,
                                            ManualTicketReceiptNo,
                                            ReceiptTickets,
                                            TurnInTickets,
                                            site_id,
                                            MasterEntityId,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            Guid,
                                            CreationDate,
                                            CreatedBy,
                                            ManualTicketReceiptId,
                                            TrxId,
                                            TrxLineId,
                                            RedemptionCurrencyRuleId,
                                            RedemptionCurrencyRuleTicket,
                                            SourceCurrencyRuleId
                                            )VALUES (
                                            src.RedemptionId,
                                            src.RedemptionGiftId,
                                            src.ManualTickets,
                                            src.GraceTickets,
                                            src.CardId,
                                            src.ETickets,
                                            src.CurrencyId,
                                            src.CurrencyQuantity,
                                            src.CurrencyTickets,
                                            src.ManualTicketReceiptNo,
                                            src.ReceiptTickets,
                                            src.TurnInTickets,
                                            src.site_id,
                                            src.MasterEntityId,
                                            src.LastUpdatedBy,
                                            GETDATE(),
                                            src.Guid,
                                            GETDATE(),
                                            src.CreatedBy,
                                            src.ManualTicketReceiptId,
                                            src.TrxId,
                                            src.TrxLineId,
                                            src.RedemptionCurrencyRuleId,
                                            src.RedemptionCurrencyRuleTicket,
                                            src.SourceCurrencyRuleId
                                            )
                                            OUTPUT
                                            inserted.Id,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdatedDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            Id,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdatedDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion
        /// <summary>
        /// Default constructor of RedemptionTicketAllocationDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public RedemptionTicketAllocationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
       
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating RedemptionTicketAllocation Record.
        /// </summary>
        /// <param name="redemptionTicketAllocationDTO">redemptionTicketAllocationDTO</param>
        /// <param name="userId">userId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>parameters</returns>
        private List<SqlParameter> GetSQLParameters(RedemptionTicketAllocationDTO redemptionTicketAllocationDTO, string userId, int siteId)
        {
            log.LogMethodEntry(redemptionTicketAllocationDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", redemptionTicketAllocationDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionId", redemptionTicketAllocationDTO.RedemptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionGiftId", redemptionTicketAllocationDTO.RedemptionGiftId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ManualTickets", redemptionTicketAllocationDTO.ManualTickets,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GraceTickets", redemptionTicketAllocationDTO.GraceTickets,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", redemptionTicketAllocationDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ETickets", redemptionTicketAllocationDTO.ETickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CurrencyId", redemptionTicketAllocationDTO.CurrencyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CurrencyQuantity", redemptionTicketAllocationDTO.CurrencyQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CurrencyTickets", redemptionTicketAllocationDTO.CurrencyTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ManualTicketReceiptNo", redemptionTicketAllocationDTO.ManualTicketReceiptNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceiptTickets", redemptionTicketAllocationDTO.ReceiptTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TurnInTickets", redemptionTicketAllocationDTO.TurnInTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ManualTicketReceiptId", redemptionTicketAllocationDTO.ManualTicketReceiptId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", redemptionTicketAllocationDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", redemptionTicketAllocationDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxLineId", redemptionTicketAllocationDTO.TrxLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionCurrencyRuleId", redemptionTicketAllocationDTO.RedemptionCurrencyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionCurrencyRuleTicket", redemptionTicketAllocationDTO.RedemptionCurrencyRuleTicket));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SourceCurrencyRuleId", redemptionTicketAllocationDTO.SourceCurrencyRuleId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Redemption ticket allocation records to the database
        /// </summary>
        /// <param name="redemptionTicketAllocationDTO">RedemptionTicketAllocationDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public RedemptionTicketAllocationDTO InsertRedemptionTicketAllocation(RedemptionTicketAllocationDTO redemptionTicketAllocationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionTicketAllocationDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RedemptionTicketAllocation] 
                                        ( 
                                            RedemptionId,
                                            RedemptionGiftId,
                                            ManualTickets,
                                            GraceTickets,
                                            CardId,
                                            ETickets,
                                            CurrencyId,
                                            CurrencyQuantity,
                                            CurrencyTickets,
                                            ManualTicketReceiptNo,
                                            ReceiptTickets,
                                            TurnInTickets,
                                            site_id,
                                            MasterEntityId,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            Guid,
                                            CreationDate,
                                            CreatedBy,
                                            ManualTicketReceiptId,
                                            TrxId,
                                            TrxLineId,
                                            RedemptionCurrencyRuleId,
                                            RedemptionCurrencyRuleTicket,
                                            SourceCurrencyRuleId
                                        ) 
                                VALUES 
                                        (
                                            @RedemptionId,
                                            @RedemptionGiftId,
                                            @ManualTickets,
                                            @GraceTickets,
                                            @CardId,
                                            @ETickets,
                                            @CurrencyId,
                                            @CurrencyQuantity,
                                            @CurrencyTickets,
                                            @ManualTicketReceiptNo,
                                            @ReceiptTickets,
                                            @TurnInTickets,
                                            @site_id,
                                            @MasterEntityId,
                                            @LastUpdatedBy,
                                            getdate(),
                                            NEWID(),
                                            getdate(),
                                            @CreatedBy,
                                            @ManualTicketReceiptId,
                                            @TrxId,
                                            @TrxLineId,
                                            @RedemptionCurrencyRuleId,
                                            @RedemptionCurrencyRuleTicket,
                                            @SourceCurrencyRuleId
                                        )SELECT* FROM RedemptionTicketAllocation WHERE Id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionTicketAllocationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionTicketAllocationDTO(redemptionTicketAllocationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting redemptionTicketAllocationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionTicketAllocationDTO);
            return redemptionTicketAllocationDTO;
        }


        /// <summary>
        /// Updates the redemption ticket allocation record
        /// </summary>
        /// <param name="redemptionTicketAllocationDTO">RedemptionTicketAllocationDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public RedemptionTicketAllocationDTO UpdateRedemptionTicketAallocation(RedemptionTicketAllocationDTO redemptionTicketAllocationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionTicketAllocationDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[RedemptionTicketAllocation]
                             SET RedemptionId=@RedemptionId,
                                 RedemptionGiftId=@RedemptionGiftId,
                                 ManualTickets=@ManualTickets,
                                 GraceTickets=@GraceTickets,
                                 CardId=@CardId, 
                                 ETickets=@ETickets,
                                 CurrencyId=@CurrencyId,
                                 CurrencyQuantity=@CurrencyQuantity,
                                 CurrencyTickets=@CurrencyTickets,
                                 ManualTicketReceiptNo=@ManualTicketReceiptNo,
                                 ReceiptTickets=@ReceiptTickets,
                                 TurnInTickets=@TurnInTickets,
                                 -- site_id=@site_id,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdatedDate=getdate(), 
                                 ManualTicketReceiptId=@ManualTicketReceiptId,
                                 TrxId=@TrxId,
                                 TrxLineId=@TrxLineId,
                                 RedemptionCurrencyRuleId=@RedemptionCurrencyRuleId,
                                 RedemptionCurrencyRuleTicket=@RedemptionCurrencyRuleTicket,
                                 SourceCurrencyRuleId=@SourceCurrencyRuleId
                             WHERE id = @Id
                    SELECT * FROM RedemptionTicketAllocation WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionTicketAllocationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionTicketAllocationDTO(redemptionTicketAllocationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating redemptionTicketAllocationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionTicketAllocationDTO);
            return redemptionTicketAllocationDTO;
        }

        /// <summary>
        /// Delete the record from the RedemptionTicketAllocationDTO database based on redemptionTicketAllocationId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int redemptionTicketAllocationId)
        {
            log.LogMethodEntry(redemptionTicketAllocationId);
            string query = @"DELETE  
                             FROM RedemptionTicketAllocation
                             WHERE RedemptionTicketAllocation.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", redemptionTicketAllocationId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="redemptionTicketAllocationDTO">RedemptionTicketAllocationDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshRedemptionTicketAllocationDTO(RedemptionTicketAllocationDTO redemptionTicketAllocationDTO, DataTable dt)
        {
            log.LogMethodEntry(redemptionTicketAllocationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                redemptionTicketAllocationDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                redemptionTicketAllocationDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                redemptionTicketAllocationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                redemptionTicketAllocationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);                
                redemptionTicketAllocationDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                redemptionTicketAllocationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                redemptionTicketAllocationDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);

            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to RedemptionTicketAllocationDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns RedemptionTicketAllocationDTO</returns>
        private RedemptionTicketAllocationDTO GetRedemptionTicketAllocationDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RedemptionTicketAllocationDTO RedemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["RedemptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RedemptionId"]),
                                            dataRow["RedemptionGiftId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RedemptionGiftId"]),
                                            dataRow["ManualTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ManualTickets"]),
                                            dataRow["GraceTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["GraceTickets"]),
                                            dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                            dataRow["ETickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ETickets"]),
                                            dataRow["CurrencyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyId"]),
                                            dataRow["CurrencyQuantity"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CurrencyQuantity"]),
                                            dataRow["CurrencyTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["CurrencyTickets"]),
                                            dataRow["ManualTicketReceiptNo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ManualTicketReceiptNo"]),
                                            dataRow["ReceiptTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ReceiptTickets"]),
                                            dataRow["TurnInTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TurnInTickets"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["ManualTicketReceiptId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ManualTicketReceiptId"]),
                                            dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                            dataRow["TrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxLineId"]),
                                            dataRow["RedemptionCurrencyRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RedemptionCurrencyRuleId"]),
                                            dataRow["RedemptionCurrencyRuleTicket"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["RedemptionCurrencyRuleTicket"]),
                                            dataRow["SourceCurrencyRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SourceCurrencyRuleId"])
                                            );
            log.LogMethodExit(RedemptionTicketAllocationDTO);
            return RedemptionTicketAllocationDTO;
        }

        /// <summary>
        /// Gets the RedemptionTicketAllocation data of passed RedemptionTicketAllocation Id
        /// </summary>
        /// <param name="redemptionTicketAllocationId">Redemption ticket allocation id</param>
        /// <returns>Returns RedemptionTicketAllocationDTO</returns>
        public RedemptionTicketAllocationDTO GetRedemptionTicketAllocationDTO(int redemptionTicketAllocationId)
        {
            log.LogMethodEntry(redemptionTicketAllocationId);
            RedemptionTicketAllocationDTO returnValue = null;
            string query = @"SELECT *
                            FROM RedemptionTicketAllocation
                            WHERE id = @redemptionTicketAllocationId";
            SqlParameter parameter = new SqlParameter("@redemptionTicketAllocationId", redemptionTicketAllocationId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetRedemptionTicketAllocationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the RedemptionTicketAllocationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RedemptionTicketAllocationDTO matching the search criteria</returns>
        public List<RedemptionTicketAllocationDTO> GetRedemptionTicketAllocationDTOList(List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> searchParameters, int siteId)
        {
            log.LogMethodEntry(searchParameters, siteId);
            List<RedemptionTicketAllocationDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            int count = 0;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_TICKET_ALLOCATION_ID ||
                            searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID ||
                            searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_GIFT_ID ||
                            searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.CURRENCY_ID ||
                            searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.MANUAL_TICKET_RECEIPT_ID ||
                            searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.TRX_ID ||
                            searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_CURRENCY_RULE_ID ||
                            searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.SOURCE_CURRENCY_RULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.MANUAL_TICKET_RECEIPT_NO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionTicketAllocationDTO.SearchByParameters.MANUAL_TICKETS_PER_DAY_BY_LOGIN_ID)
                        {
                            Utilities utilities = new Utilities();
                            object dayStartTime = utilities.executeScalar(@" 
                                                                    declare @buisnessStartTime int
                                                                    set @buisnessStartTime = Isnull((select default_value
                                                                                                      from parafait_defaults
                                                                                                     where default_value_name  = 'BUSINESS_DAY_START_TIME'
                                                                                                       and (site_id = @siteId or @siteId = -1)), 6)
                                                                    --select @buisnessStartTime as BusinessStartTime
                                                                    declare @startDateTime dateTime
                                                                    declare @currentHour int = (SELECT DATEPART(HOUR, GETDATE()));
                                                                    set @startDateTime = case when @currentHour < @buisnessStartTime 
						                                                                        then DATEADD(HOUR, @buisnessStartTime, DATEADD(DAY,-1,getdate())) 
						                                                                        else DATEADD(HOUR,@buisnessStartTime,CAST(CAST(CURRENT_TIMESTAMP AS VARCHAR(11)) AS DATETIME))
				                                                                            end
					                                                                        select @startDateTime",
                                                        new SqlParameter("@siteId", siteId));
                            query.Append(joiner + " rta.CreatedBy = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " and rta.CreationDate >= @dayStartTime and rta.CreationDate < @dayEndTime");

                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            parameters.Add(new SqlParameter("@dayStartTime", Convert.ToDateTime(dayStartTime).ToString("yyyy/MM/dd HH:mm:ss"))); 
                            parameters.Add(new SqlParameter("@dayEndTime", Convert.ToDateTime(dayStartTime).AddDays(1).ToString("yyyy/MM/dd HH:mm:ss")));
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
                list = new List<RedemptionTicketAllocationDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RedemptionTicketAllocationDTO RedemptionTicketAllocationDTO = GetRedemptionTicketAllocationDTO(dataRow);
                    list.Add(RedemptionTicketAllocationDTO);
                }

            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Inserts the RedemptionTicketAllocationDTOList record to the database
        /// </summary>
        /// <param name="productBarcodeDTOList">List of ProductBarcodeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionTicketAllocationDTOList, loginId, siteId);
            Dictionary<string, RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOGuidMap = GetRedemptionTicketAllocationDTOGuidMap(redemptionTicketAllocationDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(redemptionTicketAllocationDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "RedemptionTicketAllocationType",
                                                                "@RedemptionTicketAllocationList");
            Update(redemptionTicketAllocationDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(redemptionTicketAllocationDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[27];
            columnStructures[0] = new SqlMetaData("Id", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("RedemptionId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("RedemptionGiftId", SqlDbType.Int);
            columnStructures[3] = new SqlMetaData("ManualTickets", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("GraceTickets", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("CardId", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("ETickets", SqlDbType.Int);
            columnStructures[7] = new SqlMetaData("CurrencyId", SqlDbType.Int);
            columnStructures[8] = new SqlMetaData("CurrencyQuantity", SqlDbType.Decimal);
            columnStructures[9] = new SqlMetaData("CurrencyTickets", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("ManualTicketReceiptNo", SqlDbType.VarChar,20);
            columnStructures[11] = new SqlMetaData("ReceiptTickets", SqlDbType.Int);
            columnStructures[12] = new SqlMetaData("TurnInTickets", SqlDbType.Int);
            columnStructures[13] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 100);
            columnStructures[14] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[15] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 100);
            columnStructures[16] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);
            columnStructures[17] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[18] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[19] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[20] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[21] = new SqlMetaData("ManualTicketReceiptId", SqlDbType.Int);
            columnStructures[22] = new SqlMetaData("TrxId", SqlDbType.Int);
            columnStructures[23] = new SqlMetaData("TrxLineId", SqlDbType.Int);
            columnStructures[24] = new SqlMetaData("RedemptionCurrencyRuleId", SqlDbType.Int);
            columnStructures[25] = new SqlMetaData("RedemptionCurrencyRuleTicket", SqlDbType.Int);
            columnStructures[26] = new SqlMetaData("SourceCurrencyRuleId", SqlDbType.Int);

            for (int i = 0; i < redemptionTicketAllocationDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].Id, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].RedemptionId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].RedemptionGiftId,true));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].ManualTickets));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].GraceTickets));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].CardId,true));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].ETickets));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].CurrencyId, true));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].CurrencyQuantity));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].CurrencyTickets));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].ManualTicketReceiptNo));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].ReceiptTickets));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].TurnInTickets));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].CreationDate));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(Guid.Parse(redemptionTicketAllocationDTOList[i].Guid)));
                dataRecord.SetValue(18, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(19, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].SynchStatus));
                dataRecord.SetValue(20, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(21, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].ManualTicketReceiptId, true));
                dataRecord.SetValue(22, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].TrxId, true));
                dataRecord.SetValue(23, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].TrxLineId));
                dataRecord.SetValue(24, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].RedemptionCurrencyRuleId, true));
                dataRecord.SetValue(25, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].RedemptionCurrencyRuleTicket));
                dataRecord.SetValue(26, dataAccessHandler.GetParameterValue(redemptionTicketAllocationDTOList[i].SourceCurrencyRuleId, true));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, RedemptionTicketAllocationDTO> GetRedemptionTicketAllocationDTOGuidMap(List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList)
        {
            Dictionary<string, RedemptionTicketAllocationDTO> result = new Dictionary<string, RedemptionTicketAllocationDTO>();
            for (int i = 0; i < redemptionTicketAllocationDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(redemptionTicketAllocationDTOList[i].Guid))
                {
                    redemptionTicketAllocationDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(redemptionTicketAllocationDTOList[i].Guid, redemptionTicketAllocationDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                RedemptionTicketAllocationDTO redemptionTicketAllocationDTO = redemptionTicketAllocationDTOGuidMap[Convert.ToString(row["Guid"])];
                redemptionTicketAllocationDTO.Id = row["Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Id"]);
                redemptionTicketAllocationDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                redemptionTicketAllocationDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                redemptionTicketAllocationDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                redemptionTicketAllocationDTO.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                redemptionTicketAllocationDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                redemptionTicketAllocationDTO.AcceptChanges();
            }
        }
    }
}

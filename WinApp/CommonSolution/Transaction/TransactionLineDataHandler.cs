/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - TransactionLineDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        11-Nov-2019   Lakshminarayana         Created
 *2.100       24-Sep-2020   Nitin Pai               Added functionality to search for transaction lines
 *2.140.2     14-APR-2022   Girish Kundar           Modified : Aloha BSP changes
 *2.140.2      17-May-2022   Girish Kundar          Modified : TET changes 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionLineDataHandler Data Handler - Handles insert, update and select of  TransactionLine objects
    /// </summary>
    public class TransactionLineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;

        private static readonly Dictionary<TransactionLineDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionLineDTO.SearchByParameters, string>
        {
                {TransactionLineDTO.SearchByParameters.TRANSACTION_ID,"trx_lines.TrxId"},
                {TransactionLineDTO.SearchByParameters.LINE_ID,"trx_lines.LineId"},
                {TransactionLineDTO.SearchByParameters.TRANSACTION_ID_LIST, "trx_lines.TrxId"}
        };

        private const string SELECT_QUERY = "SELECT trx_lines.* from trx_lines";
        private List<SqlParameter> parameters = new List<SqlParameter>();
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS [TransactionLinesType];
                                            MERGE INTO trx_lines tbl
                                            USING @TransactionLineList AS src
                                            ON src.TrxId = tbl.TrxId 
                                            AND 
                                            src.LineId=tbl.LineId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            product_id=src.product_id,
                                             price=src.price,
                                             quantity=src.quantity,
                                             amount=src.amount,
                                             credits=src.credits,  
                                             card_number=src.card_number,   
                                             card_id=src.card_id,
                                             tax_id=src.tax_id,
                                             tax_percentage=src.tax_percentage,
                                             tickets=src.tickets,
                                             Remarks=src.Remarks,
                                             LastUpdatedBy=src.LastUpdatedBy,
                                             LastUpdateDate=getdate(), 
                                            MasterEntityId = src.MasterEntityId--,
                                            --site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            TrxId,
                                            LineId,
                                            product_id,
                                            price,
                                            quantity,
                                            amount,
                                            credits,
                                            card_number,
                                            card_id,
                                            tax_id,
                                            tax_percentage,
                                            tickets,
                                            site_id,
                                            Remarks,
                                            Guid,
                                            MasterEntityId,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            CreationDate,
                                            CreatedBy
                                            )VALUES (
                                            src.TrxId,
                                            src.LineId,
                                            src.product_id,
                                            src.price,
                                            src.quantity,
                                            src.amount,
                                            src.credits,
                                            src.card_number,
                                            src.card_id,
                                            src.tax_id,
                                            src.tax_percentage,
                                            src.tickets,
                                            src.site_id,
                                            src.Remarks,
                                            src.Guid,
                                            src.MasterEntityId,
                                            src.LastUpdatedBy,
                                            getdate(),
                                            getdate(),
                                            src.CreatedBy
                                            )
                                            OUTPUT
                                            inserted.TrxId,
                                            inserted.LineId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            TrxId,
                                            LineId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion
        /// <summary>
        /// Parameterized Constructor for TransactionLineDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction  object</param>
        public TransactionLineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating KDSOrderLine Record.
        /// </summary>
        /// <param name="transactionLineDto">TransactionLineDTO object passed as parameter</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the List of SQL parameter </returns>
        private List<SqlParameter> GetSqlParameters(TransactionLineDTO transactionLineDto, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionLineDto, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@LineId", transactionLineDto.LineId, true),
                dataAccessHandler.GetSQLParameter("@TransactionId", transactionLineDto.TransactionId, true),
                dataAccessHandler.GetSQLParameter("@CardNumber", transactionLineDto.CardNumber, true),
                dataAccessHandler.GetSQLParameter("@CardGuid", transactionLineDto.CardGuid),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId),
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Updates the record to the TransactionLine Table.
        /// </summary>
        /// <param name="transactionLineDto">TransactionLineDTO object passed as parameter</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the TransactionLineDTO</returns>
        public TransactionLineDTO Update(TransactionLineDTO transactionLineDto, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionLineDto, loginId, siteId);
            string query = @"UPDATE  [dbo].[trx_lines]
                             SET 
                                    card_number = @CardNumber,
                                    LastUpdatedBy = @LastUpdatedBy,
                                    LastUpdateDate = GETDATE()
                            WHERE TrxId  = @TransactionId AND LineId = @LineId 
                            IF @CardGuid IS NOT NULL AND EXISTS(SELECT card_id from Cards Where Guid = @CardGuid)
                            BEGIN
                                UPDATE  [dbo].[trx_lines] 
                                SET card_id = (SELECT card_id from Cards Where Guid = @CardGuid)
                                WHERE TrxId  = @TransactionId AND LineId = @LineId 
                            END
                            SELECT * FROM trx_lines WHERE TrxId  = @TransactionId AND LineId = @LineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSqlParameters(transactionLineDto, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionLineDTO(transactionLineDto, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TransactionLineDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionLineDto);
            return transactionLineDto;
        }

        public void UpdateRemarks(TransactionLineDTO transactionLineDto, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionLineDto, loginId, siteId);
            string query = @"UPDATE  [dbo].[trx_lines]
                             SET 
                                    Remarks = @Remarks,
                                    LastUpdatedBy = @LastUpdatedBy,
                                    LastUpdateDate = GETDATE()
                            WHERE TrxId  = @TransactionId AND LineId = @LineId ";
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@Remarks", transactionLineDto.Remarks));
                sqlParameters.Add(new SqlParameter("@LastUpdatedBy", loginId));
                sqlParameters.Add(new SqlParameter("@TransactionId", transactionLineDto.TransactionId));
                sqlParameters.Add(new SqlParameter("@LineId", transactionLineDto.LineId));
                int updated = dataAccessHandler.executeUpdateQuery(query, sqlParameters.ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while UpdateRemarks", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="transactionLineDto">TransactionLineDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshTransactionLineDTO(TransactionLineDTO transactionLineDto, DataTable dt)
        {
            log.LogMethodEntry(transactionLineDto, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                transactionLineDto.LineId = Convert.ToInt32(dt.Rows[0]["LineId"]);
                transactionLineDto.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                transactionLineDto.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                transactionLineDto.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                transactionLineDto.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                transactionLineDto.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                transactionLineDto.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                transactionLineDto.CancelledTime = dataRow["CancelledTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["CancelledTime"]);
            }
            log.LogMethodExit();
        }

        internal List<TransactionLineDTO> GetTransactionLines(List<int> transactionIdList)
        {
            log.LogMethodEntry(transactionIdList);
            List<TransactionLineDTO> transactionLineDTOList = new List<TransactionLineDTO>();
            string query = @"SELECT *
                            FROM trx_lines,Products, @transactionIdList List
                            WHERE trx_lines.product_id = Products.product_id and TrxId = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(query, "@transactionIdList", transactionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                transactionLineDTOList = table.Rows.Cast<DataRow>().Select(x => GetTransactionLineDTO(x)).ToList();
            }

            log.LogMethodExit(transactionLineDTOList);
            return transactionLineDTOList;
        }
        private string GetWhereClause(List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>> searchParameters)
        {
            string selectQuery = "";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionLineDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionLineDTO.SearchByParameters.TRANSACTION_ID
                            || searchParameter.Key == TransactionLineDTO.SearchByParameters.LINE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionLineDTO.SearchByParameters.TRANSACTION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ) ");
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

            return selectQuery;
        }

        /// <summary>
        /// Converts the Data row object to DayAttractionScheduleDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DayAttractionScheduleDTO</returns>
        private TransactionLineDTO GetTransactionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionLineDTO transactionLineDTO = new TransactionLineDTO(
                                                            Convert.ToInt32(dataRow["TrxId"]),
                                                            Convert.ToInt32(dataRow["LineId"]),
                                                            Convert.ToInt32(dataRow["product_id"]),
                                                            dataRow["price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["price"]),
                                                            dataRow["quantity"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["quantity"]),
                                                            dataRow["amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["amount"]),
                                                            dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                                            dataRow["Card_Number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Card_Number"]),
                                                            dataRow["credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["credits"]),
                                                            dataRow["courtesy"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["courtesy"]),
                                                            dataRow["tax_percentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["tax_percentage"]),
                                                            dataRow["tax_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["tax_id"]),
                                                            dataRow["time"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["time"]),
                                                            dataRow["bonus"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["bonus"]),
                                                            dataRow["tickets"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["tickets"]),
                                                            dataRow["loyalty_points"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["loyalty_points"]),
                                                            dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                            dataRow["promotion_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["promotion_id"]),
                                                            dataRow["ReceiptPrinted"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ReceiptPrinted"]),
                                                            dataRow["ParentLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentLineId"]),
                                                            dataRow["UserPrice"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["UserPrice"]),
                                                            dataRow["kotPrintCount"] == DBNull.Value ?(int?)null : Convert.ToInt32(dataRow["kotPrintCount"]),
                                                            dataRow["GamePlayId"] == DBNull.Value ? -1 : Convert.ToInt64(dataRow["GamePlayId"]),
                                                            dataRow["KDSSent"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["KDSSent"]),
                                                            dataRow["CreditPlusConsumptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CreditPlusConsumptionId"]),
                                                            dataRow["CancelledTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CancelledTime"]),
                                                            dataRow["CancelledBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CancelledBy"]),
                                                            dataRow["ProductDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductDescription"]),
                                                            dataRow["IsWaiverSignRequired"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["IsWaiverSignRequired"]),
                                                            dataRow["OriginalLineID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OriginalLineID"]),
                                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                            dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                                            dataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRewardsId"]),
                                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                            dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]),
                                                            dataRow["CancelCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CancelCode"])
                                                            );
            log.LogMethodExit(transactionLineDTO);
            return transactionLineDTO;
        }

        /// <summary>
        /// Converts the Data row object to TransactionLineDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns TransactionLineDTO</returns>
        private TransactionLineDTO GetTransactionLineDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionLineDTO transactionLineDTO = new TransactionLineDTO(Convert.ToInt32(dataRow["TrxId"]),
                                                                                    Convert.ToInt32(dataRow["LineId"]),
                       dataRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["product_id"]),
                       dataRow["price"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["price"]),
                       dataRow["quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["quantity"]),
                       dataRow["amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["amount"]),
                       dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                       dataRow["card_number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["card_number"]),
                       dataRow["credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["credits"]),
                       dataRow["courtesy"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["courtesy"]),
                       dataRow["tax_percentage"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["tax_percentage"]),
                       dataRow["tax_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["tax_id"]),
                       dataRow["time"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["time"]),
                       dataRow["bonus"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["bonus"]),
                       dataRow["tickets"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["tickets"]),
                       dataRow["loyalty_points"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["loyalty_points"]),
                       dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                       dataRow["promotion_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["promotion_id"]),
                       dataRow["ReceiptPrinted"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ReceiptPrinted"]),
                       dataRow["ParentLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentLineId"]),
                       dataRow["UserPrice"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["UserPrice"]),
                       dataRow["KOTPrintCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["KOTPrintCount"]),
                       dataRow["GameplayId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameplayId"]),
                       dataRow["KDSSent"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["KDSSent"]),
                       dataRow["CreditPlusConsumptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CreditPlusConsumptionId"]),
                       dataRow["CancelledTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CancelledTime"]),
                       dataRow["CancelledBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CancelledBy"]),
                       dataRow["ProductDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductDescription"]),
                       dataRow["IsWaiverSignRequired"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["IsWaiverSignRequired"]),
                       dataRow["OriginalLineID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OriginalLineID"]),
                       dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                       dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                       dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                       dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                       dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                       dataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRewardsId"]),
                       dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                       dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                       dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                       dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                       "N",
                       "N",
                       dataRow["product_name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["product_name"])
                       );
            log.LogMethodExit(transactionLineDTO);
            return transactionLineDTO;
        }
        /// <summary>
        /// Gets the TransactionDTO list matching the UserId
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="pageNumber">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Returns the list of transactionDTO matching the search criteria</returns>
        public List<TransactionLineDTO> GetTransactionLineDTOList(List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>> searchParameters, int pageNumber = 0, int pageSize = 500)
        {
            log.LogMethodEntry(searchParameters);
            string selectQuery = SELECT_QUERY;
            selectQuery = SELECT_QUERY + GetWhereClause(searchParameters);
            selectQuery += " ORDER BY trx_lines.TrxId, trx_lines.LineId OFFSET " + (pageNumber * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";

            List<TransactionLineDTO> transactionLineDTOList = new List<TransactionLineDTO>();
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                //dayAttractionScheduleDTOList = new List<DayAttractionScheduleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TransactionLineDTO transactionLineDTO = GetTransactionDTO(dataRow);
                    transactionLineDTOList.Add(transactionLineDTO);
                }
            }
            return transactionLineDTOList;
        }
        /// <summary>
        /// Inserts the transactionLineDTOList record to the database
        /// </summary>
        /// <param name="transactionLineDTOList">List of ProductBarcodeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<TransactionLineDTO> transactionLineDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionLineDTOList, loginId, siteId);
            Dictionary<string, TransactionLineDTO> transactionlinesDTOGuidMap = GetTransctionLinesDTOGuidMap(transactionLineDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(transactionLineDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "TransactionLinesType",
                                                                "@TransactionLineList");
            Update(transactionlinesDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<TransactionLineDTO> transactionLineDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(transactionLineDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[21];
            columnStructures[0] = new SqlMetaData("TrxId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("LineId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("product_id", SqlDbType.Int);
            columnStructures[3] = new SqlMetaData("price", SqlDbType.Decimal);
            columnStructures[4] = new SqlMetaData("quantity", SqlDbType.Decimal);
            columnStructures[5] = new SqlMetaData("amount", SqlDbType.Decimal);
            columnStructures[6] = new SqlMetaData("credits", SqlDbType.Decimal);
            columnStructures[7] = new SqlMetaData("card_number", SqlDbType.NVarChar, 100);
            columnStructures[8] = new SqlMetaData("card_id", SqlDbType.Int);
            columnStructures[9] = new SqlMetaData("tax_id", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("tax_percentage", SqlDbType.Float);
            columnStructures[11] = new SqlMetaData("tickets", SqlDbType.Decimal);
            columnStructures[12] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[13] = new SqlMetaData("Remarks", SqlDbType.NVarChar, 800);
            columnStructures[14] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 200);
            columnStructures[15] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[16] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 200);
            columnStructures[17] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            columnStructures[18] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[19] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[20] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            for (int i = 0; i < transactionLineDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].TransactionId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].LineId));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].ProductId, true));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].Price));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].Quantity));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].Amount));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].Credits));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].CardNumber));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].CardId, true));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].TaxId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].TaxPercentage == null ? 0 : (double)transactionLineDTOList[i].TaxPercentage));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].Tickets));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].Remarks));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].CreationDate));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(18, dataAccessHandler.GetParameterValue(Guid.Parse(transactionLineDTOList[i].Guid)));
                dataRecord.SetValue(19, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].SynchStatus));
                dataRecord.SetValue(20, dataAccessHandler.GetParameterValue(transactionLineDTOList[i].MasterEntityId, true));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, TransactionLineDTO> GetTransctionLinesDTOGuidMap(List<TransactionLineDTO> transactionLineDTOList)
        {
            Dictionary<string, TransactionLineDTO> result = new Dictionary<string, TransactionLineDTO>();
            for (int i = 0; i < transactionLineDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(transactionLineDTOList[i].Guid))
                {
                    transactionLineDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(transactionLineDTOList[i].Guid, transactionLineDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, TransactionLineDTO> transactionLineDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                TransactionLineDTO transactionLineDTO = transactionLineDTOGuidMap[Convert.ToString(row["Guid"])];
                transactionLineDTO.TransactionId = row["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(row["TrxId"]);
                transactionLineDTO.LineId = row["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(row["LineId"]);
                transactionLineDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                transactionLineDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                transactionLineDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                transactionLineDTO.LastUpdatedDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                transactionLineDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                transactionLineDTO.AcceptChanges();
            }
        }

    }

}

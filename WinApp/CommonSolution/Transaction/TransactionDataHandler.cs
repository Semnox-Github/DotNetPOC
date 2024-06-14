/********************************************************************************************
 * Project Name - Transaction Data Handler                                                                    
 * Description  - Data Handler for Transction 
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.3       18-Mar-2020   Nitin Pai            Added new search parameters and search methods for TrxHeaderDTO
 *2.80.0       20-Mar-2020   Akshay G             Added searchParameters - TRANSACTION_ID_LIST, HAS_PRODUCT_ID_LIST, modified POS_MACHINE_ID value 
 *2.80.0       18-May-2020   Girish Kundar        Modified: Added trx number , Remarks as search parameter
 *2.80.0       04-Jun-2020   Nitin                Removed TrxHeaderDTO and TrxLineDTO. Using TransactionDTO and TransactionLineDTO. Removed method and references from this calss
 *2.80.0       04-Jun-2020   Nitin                Website Enhancement - Continue as Guest - Saving CustomerIdentifier field in trx_header table
 *2.90.0       04-Jun-2020   Girish Kundar        Modified : Phase -2 chanages Report module
 *2.100.0      05-Aug-2020   Guru S A             Kiosk activity log changes
 *2.140.0      27-Jun-2021   Fiona Lishal         Modified for Delivery Order enhancements for F&B and Urban Piper
 *2.140.0      27-Jun-2021   Fiona Lishal         Issue fixes for Delivery Order enhancements for F&B and Urban Piper
 *2.140.2      14-APR-2022   Girish Kundar        Modified : Aloha BSP changes
 *2.140.2      17-May-2022   Girish Kundar        Modified : TET changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using System.Globalization;
using Semnox.Parafait.Device.PaymentGateway;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    class TransactionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private List<SqlParameter> parameters;

        private static readonly Dictionary<TransactionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionDTO.SearchByParameters, string>
        {
                {TransactionDTO.SearchByParameters.TRANSACTION_ID,"trx_header.TrxId"},
                {TransactionDTO.SearchByParameters.STATUS, "trx_header.Status"},
                {TransactionDTO.SearchByParameters.SITE_ID, "trx_header.site_id"},
                {TransactionDTO.SearchByParameters.ORDER_ID, "trx_header.OrderId"},
                {TransactionDTO.SearchByParameters.POS_MACHINE_ID, "trx_header.POSMachineId"}, // Modified from pos_machine to POSMachineId
                {TransactionDTO.SearchByParameters.POS_TYPE_ID, "trx_header.POSTypeId"},
                {TransactionDTO.SearchByParameters.POS_NAME, "trx_header.pos_machine"},
                {TransactionDTO.SearchByParameters.TRANSACTION_OTP, "trx_header.TransactionOTP"},
                {TransactionDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, "trx_header.external_system_reference"},
                {TransactionDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE, "trx_header.Original_system_reference"},
                {TransactionDTO.SearchByParameters.ONLINE_ONLY, "trx_header.Original_system_reference"},
                {TransactionDTO.SearchByParameters.CUSTOMER_ID, "trx_header.customerId"},
                {TransactionDTO.SearchByParameters.CUSTOMER_GUID_ID, "trx_header.customerId"},
                {TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, "trx_header.TrxDate"},
                {TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, "trx_header.TrxDate"},
                {TransactionDTO.SearchByParameters.LAST_UPDATE_FROM_TIME, "trx_header.LastUpdateTime"},
                {TransactionDTO.SearchByParameters.LAST_UPDATE_TO_TIME, "trx_header.LastUpdateTime"},
                {TransactionDTO.SearchByParameters.STATUS_NOT_IN, "trx_header.Status"},
                {TransactionDTO.SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID, "ws.CustomerSignedWaiverId"},
                {TransactionDTO.SearchByParameters.HAS_ATTRACTION_BOOKINGS, "atb.TrxId"},
                {TransactionDTO.SearchByParameters.IS_RESERVATION_TRANSACTION, "b.TrxId"},
                {TransactionDTO.SearchByParameters.HAS_EXTERNAL_SYSTEM_REFERENCE, "trx_header.External_System_Reference"},
                {TransactionDTO.SearchByParameters.HAS_PRODUCT_TYPE, "pt.product_type"},
                {TransactionDTO.SearchByParameters.TRANSACTION_NUMBER, "trx_header.trx_no"},
                {TransactionDTO.SearchByParameters.REMARKS, "trx_header.Remarks"},
                {TransactionDTO.SearchByParameters.USER_ID, "trx_header.user_id"},
                {TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, "trx_header.TrxId"},
                {TransactionDTO.SearchByParameters.HAS_PRODUCT_ID_LIST, "trx_lines.product_id"},
                {TransactionDTO.SearchByParameters.GUID, "trx_header.Guid"},
                {TransactionDTO.SearchByParameters.CUSTOMER_IDENTIFIER, "trx_header.CustomerIdentifier"},
                {TransactionDTO.SearchByParameters.ORIGINAL_TRX_ID, "trx_header.OriginalTrxid"},
                {TransactionDTO.SearchByParameters.LINKED_BILL_CYCLE_TRX_FOR_SUBSCRIPTION_HEADER_ID, "sbs.SubscriptionHeaderId"},
                {TransactionDTO.SearchByParameters.POS_OVERRIDE_OPTION_NAME_LIST, "ppoo.OptionName"},
                {TransactionDTO.SearchByParameters.NEEDS_ORDER_DISPENSING, ""},
                {TransactionDTO.SearchByParameters.TRX_PAYMENT_MODE_ID, "trxPayments.PaymentModeId"},
                {TransactionDTO.SearchByParameters.TRX_NOT_IN_EXSYS_LOG, ""},
                {TransactionDTO.SearchByParameters.IS_POS_MACHINE_INCLUDED_IN_BSP, ""},
                {TransactionDTO.SearchByParameters.AMOUNT_GREATER_THAN_ZERO, "trx_header.TrxNetAmount"}
            };

        private const string SELECT_QUERY = @" SELECT trx_header.*, 
                                               OrderHeader.TableNumber,
                                               (select isnull(sum(amount), 0) 
                                                  from trxPayments 
                                                 where trxId = trx_header.TrxId) Paid,
                                               users.username,
                                               Cards.card_number,
                                               (SELECT TOP 1 IsNULL(Profile.FirstName,'') + ISNULL(' '+ Profile.LastName,'')
                                                FROM Profile, Customers 
                                                WHERE Profile.Id = Customers.ProfileId
                                                AND (Customers.customer_id = trx_header.customerid OR Cards.customer_id = Customers.customer_id)) CustomerName
                                               FROM trx_header
                                               LEFT OUTER JOIN OrderHeader ON OrderHeader.OrderId = trx_header.OrderId 
                                               LEFT OUTER JOIN users ON users.user_id = trx_header.user_id
                                               LEFT OUTER JOIN Cards on Cards.card_id = trx_header.PrimaryCardId ";

        /// <summary>
        /// Default constructor of TransactionDataHandler class
        /// </summary>
        public TransactionDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Transaction Record.
        /// </summary>
        /// <param name="transactionDTO">Transaction type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(TransactionDTO transactionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(transactionDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", transactionDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxDate", transactionDTO.TransactionDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxAmount", transactionDTO.TransactionAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxDiscountPercentage", transactionDTO.TransactionDiscountPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxAmount", transactionDTO.TaxAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxNetAmount", transactionDTO.TransactionNetAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pos_machine", transactionDTO.PosMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@user_id", transactionDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@payment_mode", transactionDTO.PaymentMode, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CashAmount", transactionDTO.CashAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditCardAmount", transactionDTO.CreditCardAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameCardAmount", transactionDTO.GameCardAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentReference", transactionDTO.PaymentReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PrimaryCardId", transactionDTO.PrimaryCardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", transactionDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeId", transactionDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@trx_no", transactionDTO.TransactionNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", transactionDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", transactionDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OtherPaymentModeAmount", transactionDTO.OtherPaymentModeAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", transactionDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxProfileId", transactionDTO.TransactionProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TokenNumber", transactionDTO.TokenNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Original_System_Reference", transactionDTO.OriginalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customerId", transactionDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@External_System_Reference", transactionDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReprintCount", transactionDTO.ReprintCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OriginalTrxID", transactionDTO.OriginalTransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", transactionDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", transactionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderTypeGroupId", transactionDTO.OrderTypeGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionOTP", transactionDTO.TransactionOTP));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerIdentifier", transactionDTO.CustomerIdentifier));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Transaction record to the database
        /// </summary>
        public TransactionDTO InsertTransaction(TransactionDTO transactionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(transactionDTO, userId, siteId);
            string query = @"INSERT INTO trx_header
                                        ( 
                                            TrxDate,
                                            TrxAmount,
                                            TrxDiscountPercentage,
                                            TaxAmount,
                                            TrxNetAmount,
                                            pos_machine,
                                            user_id,
                                            payment_mode,
                                            CashAmount,
                                            CreditCardAmount,
                                            GameCardAmount,
                                            PaymentReference,
                                            PrimaryCardId,
                                            TransactionId,
                                            POSTypeId,
                                            site_id,
                                            trx_no,
                                            Remarks,
                                            POSMachineId,
                                            OtherPaymentModeAmount,
                                            Status,
                                            TrxProfileId,
                                            LastUpdateTime,
                                            LastUpdatedBy,
                                            TokenNumber,
                                            Original_System_Reference,
                                            customerId,
                                            External_System_Reference,
                                            ReprintCount,
                                            OriginalTrxID,
                                            CreatedBy,
                                            MasterEntityId,
                                            OrderTypeGroupId, 
                                            CreationDate,
                                            CustomerIdentifier
                                        )
                                        VALUES
                                        (  
                                            @TrxDate,
                                            @TrxAmount,
                                            @TrxDiscountPercentage,
                                            @TaxAmount,
                                            @TrxNetAmount,
                                            @pos_machine,
                                            @user_id,
                                            @payment_mode,
                                            @CashAmount,
                                            @CreditCardAmount,
                                            @GameCardAmount,
                                            @PaymentReference,
                                            @PrimaryCardId,
                                            @TransactionId,
                                            @POSTypeId,
                                            @site_id,
                                            @trx_no,
                                            @Remarks,
                                            @POSMachineId,
                                            @OtherPaymentModeAmount,
                                            @Status,
                                            @TrxProfileId,
                                            GetDate(),
                                            @LastUpdatedBy,
                                            @TokenNumber,
                                            @Original_System_Reference,
                                            @customerId,
                                            @External_System_Reference,
                                            @ReprintCount,
                                            @OriginalTrxID,
                                            @CreatedBy,
                                            @MasterEntityId,
                                            @OrderTypeGroupId, 
                                            GetDate(),
                                            @CustomerIdentifier
                                        )
                                        SELECT * FROM trx_header WHERE TrxId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(transactionDTO, userId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    transactionDTO.CreatedBy = transactionDTO.UserId;
                    transactionDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                    transactionDTO.TransactionId = Convert.ToInt32(dt.Rows[0]["TrxId"]);
                    transactionDTO.LastUpdateTime = Convert.ToDateTime(dt.Rows[0]["LastUpdateTime"]);
                    transactionDTO.Guid = Convert.ToString(dt.Rows[0]["guid"]);
                    transactionDTO.LastUpdatedBy = userId;
                    transactionDTO.SiteId = siteId;
                }
            }
            catch (Exception ex)
            {
                log.LogVariableState("TransactionDTO", transactionDTO);
                log.Error("Error occured while inserting the Transaction record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }

        public TransactionDTO UpdateTransaction(TransactionDTO transactionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(transactionDTO, userId, siteId);
            /* removing following field updates from transaction update
              * TrxDate = @TrxDate,
                             TrxAmount = @TrxAmount,
                             TrxDiscountPercentage = @TrxDiscountPercentage,
                             TaxAmount= @TaxAmount,
                             TrxNetAmount= @TrxNetAmount,
                             pos_machine = @pos_machine,
                             user_id = @user_id,
                             payment_mode = @payment_mode,
                             CashAmount = @CashAmount,
                             CreditCardAmount = @CreditCardAmount,
                             GameCardAmount = @GameCardAmount,
                             PaymentReference = @PaymentReference,
                             PrimaryCardId = @PrimaryCardId,
                             OrderId = @OrderId,
                             POSTypeId = @POSTypeId,
                             site_id =  @site_id,
                             trx_no = @trx_no,
                             Remarks = @Remarks,
                             POSMachineId = @POSMachineId,
                             OtherPaymentModeAmount = @OtherPaymentModeAmount,
                             Status = @Status,
                             TrxProfileId = @TrxProfileId,
                             LastUpdateTime = GetDate(),
                             LastUpdatedBy = @LastUpdatedBy,
                             TokenNumber = @TokenNumber,
                             Original_System_Reference = @Original_System_Reference,
                             customerId = @customerId,
                             External_System_Reference = @External_System_Reference,
                             ReprintCount = @ReprintCount,
                             OriginalTrxID = @OriginalTrxID,
                             MasterEntityId = @MasterEntityId,
                             OrderTypeGroupId = @OrderTypeGroupId,
                             * */
            string query = @"UPDATE trx_header SET 
                            LastUpdateTime = GetDate(),
                            LastUpdatedBy = @LastUpdatedBy,
                            External_System_Reference = @External_System_Reference,
                            Remarks = @Remarks,
                            Status = @Status,
                            TransactionOTP = @TransactionOTP
                            WHERE  TrxId = @TrxId
                            SELECT * FROM trx_header WHERE TrxId = @TrxId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(transactionDTO, userId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    transactionDTO.LastUpdateTime = Convert.ToDateTime(dt.Rows[0]["LastUpdateTime"]);
                    transactionDTO.LastUpdatedBy = userId;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating the Transaction record", ex);
                log.LogVariableState("TransactionDTO", transactionDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }

        private List<TransactionDTO> CreateTransactionDTOList(SqlDataReader reader)
        {
            log.LogMethodEntry(reader);
            List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
            int trxId = reader.GetOrdinal("TrxId");
            int trxDate = reader.GetOrdinal("TrxDate");
            int trxAmount = reader.GetOrdinal("TrxAmount");
            int trxDiscountPercentage = reader.GetOrdinal("TrxDiscountPercentage");
            int taxAmount = reader.GetOrdinal("TaxAmount");
            int trxNetAmount = reader.GetOrdinal("TrxNetAmount");
            int pos_machine = reader.GetOrdinal("pos_machine");
            int user_id = reader.GetOrdinal("user_id");
            int payment_mode = reader.GetOrdinal("payment_mode");
            int cashAmount = reader.GetOrdinal("CashAmount");
            int creditCardAmount = reader.GetOrdinal("CreditCardAmount");
            int gameCardAmount = reader.GetOrdinal("GameCardAmount");
            int paymentReference = reader.GetOrdinal("PaymentReference");
            int primaryCardId = reader.GetOrdinal("PrimaryCardId");
            int orderId = reader.GetOrdinal("OrderId");
            int pOSTypeId = reader.GetOrdinal("POSTypeId");
            int guid = reader.GetOrdinal("Guid");
            int site_id = reader.GetOrdinal("site_id");
            int synchStatus = reader.GetOrdinal("SynchStatus");
            int trx_no = reader.GetOrdinal("trx_no");
            int remarks = reader.GetOrdinal("Remarks");
            int pOSMachineId = reader.GetOrdinal("POSMachineId");
            int otherPaymentModeAmount = reader.GetOrdinal("OtherPaymentModeAmount");
            int status = reader.GetOrdinal("Status");
            int trxProfileId = reader.GetOrdinal("TrxProfileId");
            int lastUpdateTime = reader.GetOrdinal("LastUpdateTime");
            int lastUpdatedBy = reader.GetOrdinal("LastUpdatedBy");
            int tokenNumber = reader.GetOrdinal("TokenNumber");
            int original_System_Reference = reader.GetOrdinal("Original_System_Reference");
            int customerId = reader.GetOrdinal("customerId");
            int external_System_Reference = reader.GetOrdinal("External_System_Reference");
            int reprintCount = reader.GetOrdinal("ReprintCount");
            int originalTrxID = reader.GetOrdinal("OriginalTrxID");
            int createdBy = reader.GetOrdinal("CreatedBy");
            int masterEntityId = reader.GetOrdinal("MasterEntityId");
            int orderTypeGroupId = reader.GetOrdinal("OrderTypeGroupId");
            int tableNumber = reader.GetOrdinal("TableNumber");
            int paid = reader.GetOrdinal("Paid");
            int creationDate = reader.GetOrdinal("CreationDate");
            int username = reader.GetOrdinal("username");
            int transactionOTP = reader.GetOrdinal("TransactionOTP");
            int cardNumber = reader.GetOrdinal("card_number");
            int customerName = reader.GetOrdinal("CustomerName");
            int customerIdentifier = reader.GetOrdinal("CustomerIdentifier");
            while (reader.Read())
            {
                TransactionDTO transactionDTO = new TransactionDTO(reader.IsDBNull(trxId) ? -1 : reader.GetInt32(trxId),
                                        reader.IsDBNull(trxDate) ? DateTime.MinValue : reader.GetDateTime(trxDate),
                                        reader.IsDBNull(trxAmount) ? (decimal?)null : (decimal)reader.GetDouble(trxAmount),
                                        reader.IsDBNull(trxDiscountPercentage) ? (decimal?)null : (decimal)reader.GetDouble(trxDiscountPercentage),
                                        reader.IsDBNull(taxAmount) ? (decimal?)null : (decimal)reader.GetDouble(taxAmount),
                                        reader.IsDBNull(trxNetAmount) ? (decimal?)null : (decimal)reader.GetDouble(trxNetAmount),
                                        reader.IsDBNull(pos_machine) ? string.Empty : reader.GetString(pos_machine),
                                        reader.IsDBNull(user_id) ? -1 : reader.GetInt32(user_id),
                                        reader.IsDBNull(payment_mode) ? -1 : reader.GetInt32(payment_mode),
                                        reader.IsDBNull(cashAmount) ? (decimal?)null : (decimal)reader.GetDouble(cashAmount),
                                        reader.IsDBNull(creditCardAmount) ? (decimal?)null : (decimal)reader.GetDouble(creditCardAmount),
                                        reader.IsDBNull(gameCardAmount) ? (decimal?)null : (decimal)reader.GetDouble(gameCardAmount),
                                        reader.IsDBNull(paymentReference) ? string.Empty : reader.GetString(paymentReference),
                                        reader.IsDBNull(primaryCardId) ? -1 : reader.GetInt32(primaryCardId),
                                        reader.IsDBNull(orderId) ? -1 : reader.GetInt32(orderId),
                                        reader.IsDBNull(pOSTypeId) ? -1 : reader.GetInt32(pOSTypeId),
                                        reader.IsDBNull(trx_no) ? "" : reader.GetString(trx_no),
                                        reader.IsDBNull(remarks) ? "" : reader.GetString(remarks),
                                        reader.IsDBNull(pOSMachineId) ? -1 : reader.GetInt32(pOSMachineId),
                                        reader.IsDBNull(otherPaymentModeAmount) ? (decimal?)null : reader.GetDecimal(otherPaymentModeAmount),
                                        reader.IsDBNull(status) ? string.Empty : reader.GetString(status),
                                        reader.IsDBNull(trxProfileId) ? -1 : reader.GetInt32(trxProfileId),
                                        reader.IsDBNull(lastUpdateTime) ? DateTime.MinValue : reader.GetDateTime(lastUpdateTime),
                                        reader.IsDBNull(lastUpdatedBy) ? string.Empty : reader.GetString(lastUpdatedBy),
                                        reader.IsDBNull(tokenNumber) ? string.Empty : reader.GetString(tokenNumber),
                                        reader.IsDBNull(original_System_Reference) ? string.Empty : reader.GetString(original_System_Reference),
                                        reader.IsDBNull(customerId) ? -1 : reader.GetInt32(customerId),
                                        reader.IsDBNull(external_System_Reference) ? string.Empty : reader.GetString(external_System_Reference),
                                        reader.IsDBNull(reprintCount) ? 0 : reader.GetInt32(reprintCount),
                                        reader.IsDBNull(originalTrxID) ? -1 : reader.GetInt32(originalTrxID),
                                        reader.IsDBNull(orderTypeGroupId) ? -1 : reader.GetInt32(orderTypeGroupId),
                                        reader.IsDBNull(tableNumber) ? string.Empty : reader.GetString(tableNumber),
                                        reader.IsDBNull(paid) ? (decimal?)null : reader.GetDecimal(paid),
                                        reader.IsDBNull(username) ? string.Empty : reader.GetString(username),
                                        reader.IsDBNull(createdBy) ? -1 : reader.GetInt32(createdBy),
                                        reader.IsDBNull(creationDate) ? DateTime.MinValue : reader.GetDateTime(creationDate),
                                        reader.IsDBNull(guid) ? string.Empty : reader.GetGuid(guid).ToString(),
                                        reader.IsDBNull(synchStatus) ? false : reader.GetBoolean(synchStatus),
                                        reader.IsDBNull(site_id) ? -1 : reader.GetInt32(site_id),
                                        reader.IsDBNull(masterEntityId) ? -1 : reader.GetInt32(masterEntityId),
                                        reader.IsDBNull(transactionOTP) ? string.Empty : reader.GetString(transactionOTP),
                                        reader.IsDBNull(cardNumber) ? string.Empty : reader.GetString(cardNumber),
                                        reader.IsDBNull(customerName) ? string.Empty : reader.GetString(customerName),
                                        reader.IsDBNull(customerIdentifier) ? string.Empty : reader.GetString(customerIdentifier)
                                        );
                transactionDTOList.Add(transactionDTO);
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }


        /// <summary>
        /// Gets the Transaction data of passed transaction Id
        /// </summary>
        /// <param name="transactionId">integer type parameter</param>
        /// <returns>Returns TransactionDTO</returns>
        public TransactionDTO GetTransactionDTO(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            TransactionDTO result = null;
            string selectQuery = SELECT_QUERY + " WHERE trx_header.TrxId = @TrxId";
            SqlParameter[] selectTransactionParameters = new SqlParameter[1];
            selectTransactionParameters[0] = new SqlParameter("@TrxId", transactionId);
            List<TransactionDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, selectTransactionParameters, sqlTransaction, CreateTransactionDTOList);
            if (list != null)
            {
                result = list[0];
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the no of transactions matching the search criteria
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetTransactionCount(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int trxCount = 0;
            string selectQuery = @" SELECT COUNT(1) AS TotalCount 
                                    FROM trx_header
                                    LEFT OUTER JOIN OrderHeader ON OrderHeader.OrderId = trx_header.OrderId 
                                    LEFT OUTER JOIN users ON users.user_id = trx_header.user_id ";
            selectQuery += GetWhereClause(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                trxCount = Convert.ToInt32(dataTable.Rows[0]["TotalCount"]);
            }
            log.LogMethodExit(trxCount);
            return trxCount;
        }

        /// <summary>
        /// Gets the TransactionDTO list matching the UserId
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="pageNumber">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Returns the list of transactionDTO matching the search criteria</returns>
        public List<TransactionDTO> GetTransactionDTOList(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters, int pageNumber = 0, int pageSize = 10)
        {
            log.LogMethodEntry(searchParameters);
            string selectQuery = SELECT_QUERY;
            selectQuery = SELECT_QUERY + GetWhereClause(searchParameters);
            selectQuery += " ORDER BY trx_header.TrxId desc OFFSET " + (pageNumber * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            List<TransactionDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, parameters.ToArray(), sqlTransaction, CreateTransactionDTOList);
            log.LogMethodExit(list);
            return list;
        }

        private string GetWhereClause(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            parameters = new List<SqlParameter>();
            int count = 0;
            string whereClause = string.Empty;
            if (searchParameters == null || searchParameters.Count == 0)
            {
                log.LogMethodExit(string.Empty, "search parameters is empty");
                return whereClause;
            }
            string joiner = string.Empty;
            StringBuilder query = new StringBuilder(" where ");
            foreach (KeyValuePair<TransactionDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? " " : " and ";
                    {
                        if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.ORDER_ID) ||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.TRANSACTION_ID) ||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.POS_MACHINE_ID) ||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.CUSTOMER_ID)||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.ORIGINAL_TRX_ID)||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.POS_TYPE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.GUID))
                        {
                            query.Append(joiner + "CONVERT(varchar(200), " + DBSearchParameters[searchParameter.Key] + ") = '" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.ONLINE_ONLY)
                        {
                            query.Append(joiner + @" (Original_System_Reference is not null or TransactionOTP is not null) ");
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.CUSTOMER_GUID_ID)
                        {
                            query.Append(joiner + " EXISTS ( SELECT 1 FROM customers WHERE customer_id = trx_header.customerId AND customers.Guid = " + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == TransactionDTO.SearchByParameters.USER_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.POS_NAME) ||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE) ||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE) ||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.TRANSACTION_OTP)||
                            searchParameter.Key.Equals(TransactionDTO.SearchByParameters.TRANSACTION_NUMBER))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.STATUS))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.STATUS_NOT_IN))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " NOT IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE) ||
                                 searchParameter.Key.Equals(TransactionDTO.SearchByParameters.LAST_UPDATE_FROM_TIME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE) ||
                                 searchParameter.Key.Equals(TransactionDTO.SearchByParameters.LAST_UPDATE_TO_TIME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.TRX_PAYMENT_MODE_ID))
                        {
                            query.Append(joiner + "EXISTS (select 1 from TrxPayments where trxId = trx_header.TrxId and " + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                                       from WaiversSigned ws 
                                                                      where ws.CustomerSignedWaiverId = " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                                       + @"  and ws.TrxId = trx_header.trxId
					                                                         and ws.IsActive = 1
					                                                         and ISNULL(ws.ExpiryDate, getdate()) >= getdate()
					                                                         and trx_header.TrxDate > getdate()) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.TRX_NOT_IN_EXSYS_LOG)
                        {
                            query.Append(joiner + @" not exists (select 'x' from ExSysSynchLog ex where ParafaitObjectId = trx_header.trxid ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.IS_POS_MACHINE_INCLUDED_IN_BSP)
                        {
                            query.Append(joiner + @" trx_header.POSMachineId in (select pos.POSMachineId
                                                      from POSMachines pos, parafait_defaults pd, ParafaitOptionValues pv
                                                      where pv.POSMachineId = pos.POSMachineId
													     and pv.OptionId = pd.default_value_id
													     and pd.default_value_name = 'IS_ALOHA_ENV'
													     and pv.OptionValue = 'Y'
													     )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.IS_RESERVATION_TRANSACTION) // bit value 
                        {
                            query.Append(joiner + @"( ISNULL((SELECT 1 FROM bookings b WHERE " + DBSearchParameters[searchParameter.Key] + " = trx_header.TrxId),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.HAS_ATTRACTION_BOOKINGS)
                        {
                            query.Append(joiner + @"( EXISTS(SELECT 1 FROM AttractionBookings atb WHERE atb.trxId = trx_header.TrxId) )");
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.HAS_EXTERNAL_SYSTEM_REFERENCE) // bit value 
                        {
                            query.Append(joiner + @"( ISNULL((CASE WHEN " + DBSearchParameters[searchParameter.Key] + " IS NOT NULL THEN 1 ELSE 0 END),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.HAS_PRODUCT_TYPE) // value - product_Type 
                        {
                            query.Append(joiner + @"( EXISTS(SELECT 1 FROM trx_lines
										                                LEFT JOIN Products p on p.product_id = trx_lines.product_id
										                                LEFT JOIN product_type pt on p.product_type_id = pt.product_type_id
									                                WHERE trx_lines.TrxId = trx_header.TrxId 
									                                AND " + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.HAS_PRODUCT_ID_LIST) // value - product_ID 
                        {
                            query.Append(joiner + @"( EXISTS(SELECT 1 FROM trx_lines
									                                WHERE trx_lines.TrxId = trx_header.TrxId 
									                                AND " + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST) // in separated string format 
                        {
                            query.Append(joiner + @"(" + DBSearchParameters[searchParameter.Key] + " IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + "))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(TransactionDTO.SearchByParameters.AMOUNT_GREATER_THAN_ZERO))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "> 0");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.CUSTOMER_IDENTIFIER) 
                        {
                            // Do not filter for this, this is filtered in the BL
                            query.Append(joiner + @"( 1 = 1 )");
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.LINKED_BILL_CYCLE_TRX_FOR_SUBSCRIPTION_HEADER_ID)
                        {
                            query.Append(joiner + @"( EXISTS (select 1 as t from (
                                                                    SELECT 1 as col
                                                                    FROM SubscriptionBillingSchedule sbs
								                                    WHERE sbs.TransactionId = trx_header.TrxId 
									                                  AND " + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " " +
                                                                  @"UNION  
                                                                    SELECT 1 as col
                                                                      FROM SubscriptionBillingScheduleHistory sbsh
								                                     WHERE sbsh.TransactionId = trx_header.TrxId 
									                                   AND sbsh.SubscriptionHeaderId = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) as Temp)) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.POS_OVERRIDE_OPTION_NAME_LIST)
                        {
                            query.Append(joiner + @"( EXISTS ( SELECT 1 FROM ( SELECT 1 as column1
                                                                 FROM TrxPOSPrinterOverrideRules tppor, 
                                                                      POSPrinterOverrideOptions ppoo
                                                                WHERE tppor.TransactionId = trx_header.TrxId
                                                                  AND tppor.POSPrinterOverrideOptionID = ppoo.POSPrinterOverrideOptionId
                                                                  AND " + DBSearchParameters[searchParameter.Key]
                                                                  + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value)
                                                                  + @" ) UNION all (select 1 as column1 
                                                                                FROM TrxPOSPrinterOverrideRules tppor, 
                                                                                     POSPrinterOverrideOptions ppoo 
                                                                                WHERE  trx_header.OriginalTrxID is not null
                                                                                  and tppor.TransactionId = trx_header.OriginalTrxID                                                                              
                                                                                  AND tppor.POSPrinterOverrideOptionID = ppoo.POSPrinterOverrideOptionId
                                                                                  AND " + DBSearchParameters[searchParameter.Key]
                                                                                    + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value)
                                                                  + ")))As new))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionDTO.SearchByParameters.NEEDS_ORDER_DISPENSING)
                        {
                            query.Append(joiner + @" (ISNULL((select top 1 1 
                                                                from TransactionOrderDispensing tod 
		                                                       where tod.TransactionId = trx_header.TrxId),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    count++;
                }
                else
                {
                    log.Error("Ends-GetAllTransactionList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                    log.LogMethodExit("Throwing exception- The query parameter does not exist ");
                    throw new Exception("The query parameter does not exist");
                }
            }
            whereClause = query.ToString();
            log.LogMethodExit(whereClause);
            return whereClause;
        }

        ///// <summary>
        ///// Gets Last update time for the Transaction
        ///// </summary>
        ///// <param name="transactionId">integer type parameter</param>
        ///// <returns>Returns Last Transaction update date time</returns>
        //public DateTime GetLastTransactionUpdateDateTime(int transactionId)
        //{
        //    log.LogMethodEntry(transactionId);
        //    DateTime lastUpdateTime = DateTime.MinValue;
        //    string query = "select LastUpdateTime from trx_header WHERE trxid = @trxId";
        //    SqlParameter parameter = new SqlParameter("@trxId", transactionId);
        //    DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        lastUpdateTime = dataTable.Rows[0]["LastUpdateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataTable.Rows[0]["LastUpdateTime"]);
        //    }
        //    log.LogMethodExit(lastUpdateTime);
        //    return lastUpdateTime;

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<PurchasedTransactionLineStruct> GetPurchasedTransactionLineDTOList(DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(fromDate, toDate);
            List<SqlParameter> trxParameters = new List<SqlParameter>();
            List<PurchasedTransactionLineStruct> list = new List<PurchasedTransactionLineStruct>();
            string selectQuery = @"select '' a,t.trxid,t.product_name,t.product_type,sum(t.quantity) quantity,
                              sum(t.Net_Amount*(1-(isnull(td.discountpercentage,0)/100)) * (t.CashRatio + t.CreditCardRatio)) Sale_price,
                      sum(isnull(Servicecharge,0)) Servicecharge,sum(isnull(tx.tax,0)) tax,
                      sum(t.amount*(isnull(td.discountpercentage,0)/100)* (t.CashRatio + t.CreditCardRatio)) Discount,
                      sum(t.amount*(1-(isnull(td.discountpercentage,0)/100)) * (t.CashRatio + t.CreditCardRatio))  as [GrossTotal],
                      u.username Cashier,c.Name Category,trxdate,
                      pt.POSTypeName PosCounter,(case  when status='Cancelled' then 'T'  else 'F' end) void
                      from TransactionView t left outer join (SELECT TrxId, LineId, SUM(discountpercentage) discountpercentage FROM TrxDiscounts group by TrxId, LineId) td on td.trxid = t.trxid and td.lineid = t.lineid
                      left join POSMachines pm on t.pos_machine=pm.POSName
                      left join POSTypes pt on pm.POSTypeId=pt.POSTypeId
                      left join (select TrxId, lineid, sum(Servicecharge) as Servicecharge, sum(Tax) as Tax from 
			        					(select TrxId, lineid,(select case when TaxStructureId=( ISNULL((SELECT TOP 1 TaxStructureId 
			        						From TaxStructure Where StructureName = 'Service Charge'),0))
			        						then taxamt else 0 end ) Servicecharge,
			        					(select case when TaxStructureId!=( ISNULL((SELECT TOP 1 TaxStructureId 
			        						From TaxStructure Where StructureName = 'Service Charge'),0))
			        						then taxamt else 0 end ) Tax 
			        					from 
			        					(select case when tl.taxstructureid IS NULL then tax_name 
			        						else StructureName end tax_name, 
			        						tl.taxstructureid, TrxId, lineid, Amount taxamt 
			        						from TrxTaxLines tl left outer join TaxStructure ts 
			        						on tl.taxstructureid = ts.taxstructureid, tax t
			        					 where exists (select 1 from transactionview h
			        					 where trxdate >= @fromdate and TrxDate < @todate
                                         and h.TrxId = tl.TrxId and h.lineid = tl.LineId)
			        					 and tl.TaxId = t.tax_id) A) B
			        			group by TrxId, lineid) Tx on tx.trxid = t.trxid and tx.lineid = t.lineid
					        	,products p left join Category c on p.CategoryId=c.CategoryId,users u
                    where t.product_id=p.product_id  and t.user_id=u.user_id and trxdate >= @fromDate and trxdate <= @toDate
                    group by trx_no,t.trxid,t.product_name,t.product_type,u.username ,c.Name ,trxdate,pt.POSTypeName,status";
            trxParameters.Add(new SqlParameter("@fromdate", fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            trxParameters.Add(new SqlParameter("@todate", toDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            DataTable dt = dataAccessHandler.executeSelectQuery(selectQuery, trxParameters.ToArray(), sqlTransaction);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PurchasedTransactionLineStruct purchasedTransactionLineSt = new PurchasedTransactionLineStruct();
                    purchasedTransactionLineSt.TransactionId = dr["trxid"] == DBNull.Value ? -1 : Convert.ToInt32(dr["trxid"]);
                    purchasedTransactionLineSt.PosMachineName = dr["PosCounter"] == DBNull.Value ? string.Empty : dr["PosCounter"].ToString();
                    purchasedTransactionLineSt.ProductName = dr["product_name"] == DBNull.Value ? string.Empty : dr["product_name"].ToString();
                    purchasedTransactionLineSt.ProductType = dr["Category"] == DBNull.Value ? string.Empty : dr["Category"].ToString();
                    purchasedTransactionLineSt.Quantity = dr["quantity"] == DBNull.Value ? 1 : Convert.ToInt32(dr["quantity"]);
                    purchasedTransactionLineSt.ProductPrice = dr["Sale_price"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Sale_price"]);
                    purchasedTransactionLineSt.Tax = dr["tax"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["tax"]);
                    purchasedTransactionLineSt.ServiceCharge = dr["Servicecharge"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Servicecharge"]);
                    purchasedTransactionLineSt.TotalAmount = dr["GrossTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["GrossTotal"]);
                    purchasedTransactionLineSt.DiscountPercentage = dr["Discount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Discount"]);
                    purchasedTransactionLineSt.TransactionDate = dr["trxdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["trxdate"]);
                    purchasedTransactionLineSt.UserName = dr["Cashier"] == DBNull.Value ? string.Empty : dr["Cashier"].ToString();
                    purchasedTransactionLineSt.Void = dr["void"] == DBNull.Value ? "F" : dr["void"].ToString();
                    list.Add(purchasedTransactionLineSt);
                }
            }
            log.LogMethodExit(dt);
            return list;
        }

        public bool IsQueueProductExists(string sourceTagNumber, string destinationTagNumber)
        {
            log.LogMethodEntry(sourceTagNumber, destinationTagNumber);
            bool result = false;
            List<SqlParameter> trxParameters = new List<SqlParameter>();
            string selectQuery = @"select top 1 1 
                                      from cardGames cg1, cards c1
                                     where cg1.card_id = c1.card_id
                                        and c1.Valid_Flag = 'Y'
                                        and c1.Card_number = @sourceTagNumber
                                        and cg1.BalanceGames > 0
                                        and (cg1.ExpiryDate is null or cg1.ExpiryDate > getdate())
                                        and dbo.GetGameProfileValue(cg1.game_id, 'QUEUE_SETUP_REQUIRED') = '1'
                                        and exists (select 1 
                                                     from cardGames cg2, cards c2
                                                    where cg2.card_id = c2.card_id
                                                      and c2.Valid_Flag = 'Y'
                                                      and c2.Card_number = @destinationTagNumber
                                                      and cg2.BalanceGames > 0
                                                      and (cg2.ExpiryDate is null or cg2.ExpiryDate > getdate())
                                                      and dbo.GetGameProfileValue(cg2.game_id, 'QUEUE_SETUP_REQUIRED') = '1')";
            trxParameters.Add(new SqlParameter("@sourceTagNumber", sourceTagNumber));
            trxParameters.Add(new SqlParameter("@destinationTagNumber", destinationTagNumber));
            DataTable dt = dataAccessHandler.executeSelectQuery(selectQuery, trxParameters.ToArray(), sqlTransaction);
            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal DataTable GetTrxHeaderDetails(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters,
                                    string trxNoHeading, bool showAmountFieldsTransaction, int loginUserId)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            string query = @"exec dbo.SetContextInfo @loginUserId; select
                              [TrxId] as ID " +
                              ",[Trx_no] as \"" + trxNoHeading + "\" " +
                            @",oh.TableNumber as Table#
                              ,[TrxDate] as Date
                              ,case when @showAmountFields = 1 then [TrxAmount] else 0  end as Amount
                              ,case when @showAmountFields = 1 then [TaxAmount] else 0 end as Tax
                              ,case when @showAmountFields = 1 then [TrxNetAmount] else 0 end as Net_amount 
                              ,case when @showAmountFields = 1 then (select isnull(sum(amount), 0) 
                                  from trxPayments 
                                 where trxId = trx_header.TrxId) else 0 end Paid" +
                             ",case when @showAmountFields = 1 then [TrxDiscountPercentage] else 0 end as \"avg_disc_%\" " +
                            @",case payment_mode when 1 then 'Cash' when 2 then 'Credit Card' when 3 then 'Debit Card' when 4 then 'Other' when 5 then 'Multiple' else '' end as pay_mode
                              ,[pos_machine] as POS
                              ,usr.username as Cashier
                              ,p.FirstName + ' '+ isnull(p.LastName,'') as Customer_Name,
                              case when @showAmountFields = 1 then CashAmount else 0 end Cash, 
                              case when @showAmountFields = 1 then CreditCardAmount else 0 end C_C_Amount, 
                              case when @showAmountFields = 1 then GameCardAmount else 0 end Game_card_amt,
                              case when @showAmountFields = 1 then OtherPaymentModeAmount else 0 end Other_Mode_Amt,
                              PaymentReference Ref,
                              Status,
                              trx_header.Remarks
                            from trx_header trx_header left outer join users usr on usr.user_id = trx_header.user_id
                                              left outer join OrderHeader oh on oh.OrderId = trx_header.OrderId
											  left outer join customers c on c.customer_id = trx_header.customerId
                                              left outer join Profile p on p.id = c.profileId,
                                (select top 1 *
                           from (select user_id, r.role_id, r.DataAccessLevel, r.DataAccessRuleId
                             from users u, user_roles r
                             where user_id = dbo.GetContextInfo()
                             and r.role_id = u.role_id
                             union all select -1, -1, 'S',-1) viw order by 1 desc) u
                            " + GetWhereClause(searchParameters) + @"
                                and (trx_header.user_id in (select user_id 
	                                 from  DataAccessView
					                 where
						               ( (Entity = 'Transaction' and u.DataAccessRuleId is not null)
						                  OR 
						                  u.DataAccessRuleId is null
						                )
						             ))
                            order by trxdate desc";
            parameters.Add(new SqlParameter("@showAmountFields", showAmountFieldsTransaction));
            parameters.Add(new SqlParameter("@loginUserId", loginUserId));
            DataTable trxHeaderDataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit(trxHeaderDataTable);
            log.LogMethodExit(trxHeaderDataTable);
            return trxHeaderDataTable;
        }

        internal DataTable GetRefreshLines(int trxId ,bool showAmountFieldsTransaction)
        {
            log.LogMethodEntry(showAmountFieldsTransaction, trxId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            string query = @"select  
                          p.[product_name] as Product 
                          ,l.[quantity]
                         ,case when @showAmountFields = 1 then l.[price] else 0 end as Price
                         ,case when @showAmountFields = 1 then l.[amount] else 0 end as amount
                         ,(select top 1 p.FirstName + ' '+ isnull(p.LastName,'')
                                              from customers c,
                                                      Profile p,
                                                      CustomerSignedWaiver csw,
                                                         WaiversSigned ws
                                                where ws.TrxId = l.trxId
                                                   and ws.LineId = l.LineId
                                                   and ws.IsActive = 1
                                                   and ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId
                                                   and csw.SignedFor = c.customer_id
                                                   and p.id = c.profileId) as Waiver_Customer
                         ,l.[card_number]
                          ,l.[credits] 
                          ,l.[courtesy] 
                          ,l.[bonus] 
                          ,l.[time] 
                          ,l.[tickets] 
                          ,t.[tax_name] " +
                          ",l.[tax_percentage] as \"Tax %\" " +
                         @",l.[loyalty_points] 
                         ,case when @showAmountFields = 1 then l.[UserPrice] else 0 end UserPrice
                         ,l.[LineId] Line
                         ,isnull(WaiverSignedCount,0) as Waivers_Signed
                         ,l.Remarks  
                     FROM trx_lines l left outer join products p on p.product_id = l.product_id
                         left outer join tax t on t.tax_id = l.tax_id
                         left outer join (Select w.LineId,  Count(DISTINCT  customerSignedWaiverId) as WaiverSignedCount from waiversSigned w
                                           where trxid = @trxid and isnull(isActive, 0) = 1 
                                           and isnull(customerSignedWaiverId, -1) != -1 group by w.LineId)  wsc on l.Lineid  = wsc.LineId
                     where trxid = @trxid
                            order by l.lineId";
            sqlParameters.Add(new SqlParameter("@showAmountFields", showAmountFieldsTransaction));
            sqlParameters.Add(new SqlParameter("@trxid", trxId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        internal DataTable GetTransactionDetailsWithEntitlements(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters,
                                  string trxNoUserColumnHeading, bool showAmountFieldsTransaction, int loginUserId)
        {
            log.LogMethodEntry();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            string query = @"exec dbo.SetContextInfo @loginUserId ;select trx_header.trxid ID " +
                          ",trx_header.trx_no as \"" + trxNoUserColumnHeading + "\" " +
                          @",trx_header.trxdate as Date 
                          ,case when @showAmountFields = 1 then [TrxAmount] else 0 end as Amount 
                          ,case when @showAmountFields = 1 then [TrxNetAmount] else 0 end as Net_amount 
                          ,case payment_mode when 1 then 'Cash' when 2 then 'Credit Card' when 3 then 'Debit Card' when 4 then 'Other' else 'Multiple' end as pay_mode 
                          ,[pos_machine] as POS 
                          ,u.username as Cashier 
                          ,p.[product_name] as Product " +
                          ",case when @showAmountFields = 1 then l.[price] else 0 end as \"Price/Disc%\" " +
                          @",case when @showAmountFields = 1 then l.[amount] else 0 end Line_amount 
                          ,(select top 1 p.FirstName + ' '+ isnull(p.LastName,'')
                                              from customers c,
                                                      Profile p,
                                                      CustomerSignedWaiver csw,
                                                         WaiversSigned ws
                                                where ws.TrxId = l.trxId
                                                   and ws.LineId = l.LineId
                                                   and ws.IsActive = 1
                                                   and ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId
                                                   and csw.SignedFor = c.customer_id
                                                   and p.id = c.profileId) as Waiver_Customer
                          ,l.[card_number] 
                          ,l.[credits] 
                          ,l.[courtesy] 
                          ,l.[bonus] 
                          ,l.[time] 
                          ,l.[tickets] 
                          ,t.[tax_name] " +
                          ",l.[tax_percentage] as \"Tax %\" " +
                          @",l.[quantity] 
                          ,l.[loyalty_points] 
                          ,l.[LineId] Line 
                          ,trx_header.[Status] Status 
                      FROM trx_header trx_header, users u, trx_lines l left outer join products p on p.product_id = l.product_id 
                          left outer join tax t on t.tax_id = l.tax_id,
                                (select top 1 *
                           from (select user_id, r.role_id, r.DataAccessLevel, r.DataAccessRuleId
                             from users u, user_roles r
                             where user_id = dbo.GetContextInfo()
                             and r.role_id = u.role_id
                             union all select -1, -1, 'S',-1) viw order by 1 desc) ur " + GetWhereClause(searchParameters) +
                              " and l.trxid = trx_header.trxid and u.user_id = trx_header.user_id" +

                            @" and (trx_header.user_id in (select user_id 
	                                 from  DataAccessView
					                 where
						               ( (Entity = 'Transaction' and ur.DataAccessRuleId is not null)
						                  OR 
						                  ur.DataAccessRuleId is null
						                )
						             ))--New rule introduced
                            order by trx_header.trxdate desc, lineId";
            log.LogMethodExit();
            parameters.Add(new SqlParameter("@showAmountFields", showAmountFieldsTransaction));
            parameters.Add(new SqlParameter("@loginUserId", loginUserId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        internal List<int> GetAllTransactions(int accountId, int ceTransactionId, int siteId)
        {
            List<int> result = new List<int>();
            string query = @"SELECT Distinct(TH.TrxId) FROM trx_header TH
                                    LEFT OUTER JOIN trx_lines TL ON TH.TrxId = TL.TrxId
                                    Where TL.card_id = @accountId
                                   -- and TH.POSMachineId = @POSMachineId
                                    and (TH.site_id = @siteId or @siteId =-1) ";
            if (ceTransactionId > -1)
            {
                query = query + "  and TH.External_System_Reference = " + ceTransactionId;
            }
           
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@accountId", accountId));
            parameters.Add(new SqlParameter("@siteId", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    result.Add(Convert.ToInt32(dataRow["TrxId"]));
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        internal DateTime? GetLastUpdateTimeOfLinkedReservations(int transactionId, int siteId)
        {
            log.LogMethodEntry(transactionId, siteId);
            DateTime? result = null;
            string query = @"SELECT Max(LastUpdatedDate) LastUpdatedDate
                               FROM bookings
                              WHERE TrxId = @TrxId
                                and (site_id = @SiteId OR @SiteId = -1) ";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@TrxId", transactionId));
            parameters.Add(new SqlParameter("@siteId", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;

        }

        internal List<TransactionDTO> GetTransactionDTOList(TransactionSearchCriteria searchCriteria)
        {
            log.LogMethodEntry(searchCriteria);
            List<TransactionDTO> list = new List<TransactionDTO>();
            if (searchCriteria != null)
            {

                string selectQuery = "SELECT * from (" + SELECT_QUERY
                                      + @") th where exists ( select 1 
                                                                from trx_Header 
                                                               ";
                string searchClause = searchCriteria.GetWhereClause();
                string trxPaymentsClause = " ";
                string trxIdWhereClause = "where trx_Header.TrxId = th.TrxId and ";
                if (string.IsNullOrWhiteSpace(searchClause) == false)
                {
                    if (searchClause.Contains("tps1"))
                    {
                        trxPaymentsClause = " inner join TrxPayments tps1 on tps1.TrxId = trx_Header.TrxId ";
                    }
                    selectQuery += trxPaymentsClause + trxIdWhereClause + searchClause + " ) ";
                }
                selectQuery += searchCriteria.GetOrderByClause();
                selectQuery += searchCriteria.GetPaginationClause();
                if (parameters == null)
                    parameters = new List<SqlParameter>();
                List<SqlParameter> parameterList = new List<SqlParameter>(searchCriteria.GetSqlParameters());
                list = dataAccessHandler.GetDataFromReader(selectQuery, parameterList.ToArray(), sqlTransaction, CreateTransactionDTOList);

            }
            log.LogMethodExit(list);
            return list;
        }
        public List<TransactionDTO> GetTransactionsForAlohaSynch(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters,
                                                          int sqlConnectionTimeOut = 600)
        {
            log.LogMethodEntry(searchParameters, sqlConnectionTimeOut);
            dataAccessHandler.CommandTimeOut = sqlConnectionTimeOut;
            string ALOHA_SELECT_QUERY = @" SELECT trx_header.*, 
                                               OrderHeader.TableNumber,
                                               (select isnull(sum(amount), 0) 
                                                  from trxPayments 
                                                 where trxId = trx_header.TrxId) Paid,
                                               users.username,
                                               Cards.card_number,
                                               (SELECT TOP 1 IsNULL(Profile.FirstName,'') + ISNULL(' '+ Profile.LastName,'')
                                                FROM Profile, Customers 
                                                WHERE Profile.Id = Customers.ProfileId
                                                AND (Customers.customer_id = trx_header.customerid OR Cards.customer_id = Customers.customer_id)) CustomerName
                                               FROM trx_header  WITH(INDEX(Trx_Date))
                                               LEFT OUTER JOIN OrderHeader ON OrderHeader.OrderId = trx_header.OrderId 
                                               LEFT OUTER JOIN users ON users.user_id = trx_header.user_id
                                               LEFT OUTER JOIN Cards on Cards.card_id = trx_header.PrimaryCardId ";

            ALOHA_SELECT_QUERY = ALOHA_SELECT_QUERY + GetWhereClause(searchParameters);
            List<TransactionDTO> transactionDTOList = dataAccessHandler.GetDataFromReader(ALOHA_SELECT_QUERY, parameters.ToArray(), sqlTransaction, CreateTransactionDTOList);
            if (transactionDTOList != null && transactionDTOList.Any())
            {
                List<int> transactionIdList = transactionDTOList.Select(x => x.TransactionId).ToList();
                log.LogVariableState("transactionIdList", transactionIdList);
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
                TransactionLineListBL transactionLineListBL = new TransactionLineListBL();
                List<TransactionLineDTO> transactionLinesDTOList = transactionLineListBL.GetTransactionLineDTOList(transactionIdList);
                List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPayments(transactionIdList);
                foreach (TransactionDTO transactionDTO in transactionDTOList)
                {
                    transactionDTO.TransactionLinesDTOList = new List<TransactionLineDTO>();
                    transactionDTO.TrxPaymentsDTOList = new List<TransactionPaymentsDTO>();
                    if (transactionLinesDTOList != null)
                    {
                        List<TransactionLineDTO> list = transactionLinesDTOList.Where(x => x.TransactionId == transactionDTO.TransactionId).ToList();
                        transactionDTO.TransactionLinesDTOList.AddRange(list);
                    }
                    if (transactionPaymentsDTOList != null)
                    {
                        List<TransactionPaymentsDTO> paymentlist = transactionPaymentsDTOList.Where(x => x.TransactionId == transactionDTO.TransactionId).ToList();
                        transactionDTO.TrxPaymentsDTOList.AddRange(paymentlist);
                    }
                }
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }
        public List<TransactionDTO> GetTETTransactions()
        {
            log.LogMethodEntry();
            List<int> result = new List<int>();
            string TET_SELECT_QUERY = @"SELECT distinct(th.TrxId) from trx_header th , trx_lines tl
                                  WHERE tl.trxid = th.TrxId
                                      AND tl.cancelledTime is null
								      AND th.Status != 'CANCELLED'
                                      AND th.TrxNetAmount >= 0
								      AND ISNULL(tl.Remarks,'') != 'TET:Y'
                                      AND EXISTS (select 'x' from products p
                                                where tl.product_id = p.product_id
				                                  and p.ExternalSystemReference = 'TET')";

            DataTable dataTable = dataAccessHandler.executeSelectQuery(TET_SELECT_QUERY, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    result.Add(Convert.ToInt32(dataRow["TrxId"]));
                }
            }
            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> trxsearchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
            trxsearchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, string.Join(",", result)));
            string selectQuery = SELECT_QUERY + GetWhereClause(trxsearchParameters);
            List<TransactionDTO> transactionDTOList = dataAccessHandler.GetDataFromReader(selectQuery, parameters.ToArray(), sqlTransaction, CreateTransactionDTOList);
            if (transactionDTOList != null && transactionDTOList.Any())
            {
                List<int> transactionIdList = transactionDTOList.Select(x => x.TransactionId).ToList();
                log.LogVariableState("transactionIdList", transactionIdList);
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
                TransactionLineListBL transactionLineListBL = new TransactionLineListBL();
                List<TransactionLineDTO> transactionLinesDTOList = transactionLineListBL.GetTransactionLineDTOList(transactionIdList);
                foreach (TransactionDTO transactionDTO in transactionDTOList)
                {
                    transactionDTO.TransactionLinesDTOList = new List<TransactionLineDTO>();
                    if (transactionLinesDTOList != null)
                    {
                        List<TransactionLineDTO> list = transactionLinesDTOList.Where(x => x.TransactionId == transactionDTO.TransactionId).ToList();
                        transactionDTO.TransactionLinesDTOList.AddRange(list);
                    }
                }
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        internal void UpdateTransactionRemarksAndExtReference(string transactionGuid, string externalSystemRef,
                                                                string remarks)
        {
            log.LogMethodEntry(transactionGuid, externalSystemRef, remarks);
            string selectQuery = "UPDATE trx_header SET Remarks = @remarks,External_System_Reference = @externalSystemRef" +
                                        " WHERE trx_header.Guid = @Guid and External_System_Reference is null";
            SqlParameter[] selectTransactionParameters = new SqlParameter[3];
            selectTransactionParameters[0] = new SqlParameter("@Guid", transactionGuid);
            selectTransactionParameters[1] = new SqlParameter("@remarks", remarks);
            selectTransactionParameters[2] = new SqlParameter("@externalSystemRef", externalSystemRef);
            try
            {
                int rowUpdated = dataAccessHandler.executeUpdateQuery(selectQuery, selectTransactionParameters.ToArray(), sqlTransaction);
                log.Debug("rowUpdated :" + rowUpdated);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating the Transaction record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }



        internal List<TransactionDTO> GetOrdersToBePostedInAloha(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters,
                                                        int sqlConnectionTimeOut = 600)
        {
            log.LogMethodEntry(searchParameters, sqlConnectionTimeOut);
            dataAccessHandler.CommandTimeOut = sqlConnectionTimeOut;
            string ALOHA_SELECT_QUERY = @"SELECT trx_header.*, 
                                              null TableNumber,
                                               (select isnull(sum(amount), 0) 
                                                  from trxPayments 
                                                 where trxId = trx_header.TrxId) Paid,
                                               null username,
                                               null  card_number,
                                               null CustomerName
                                               FROM trx_header  WITH(INDEX(Trx_Date))";

            ALOHA_SELECT_QUERY = ALOHA_SELECT_QUERY + GetWhereClause(searchParameters);

            List <TransactionDTO> transactionDTOList = dataAccessHandler.GetDataFromReader(ALOHA_SELECT_QUERY, parameters.ToArray(), sqlTransaction, CreateTransactionDTOList);
            if (transactionDTOList != null && transactionDTOList.Any())
            {
                List<int> transactionIdList = transactionDTOList.Select(x => x.TransactionId).ToList();
                log.LogVariableState("transactionIdList", transactionIdList);
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
                TransactionLineListBL transactionLineListBL = new TransactionLineListBL();
                List<TransactionLineDTO> transactionLinesDTOList = transactionLineListBL.GetTransactionLineDTOList(transactionIdList);
                List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPayments(transactionIdList);
                foreach (TransactionDTO transactionDTO in transactionDTOList)
                {
                    transactionDTO.TransactionLinesDTOList = new List<TransactionLineDTO>();
                    transactionDTO.TrxPaymentsDTOList = new List<TransactionPaymentsDTO>();
                    if (transactionLinesDTOList != null)
                    {
                        List<TransactionLineDTO> list = transactionLinesDTOList.Where(x => x.TransactionId == transactionDTO.TransactionId).ToList();
                        transactionDTO.TransactionLinesDTOList.AddRange(list);
                    }
                    if (transactionPaymentsDTOList != null)
                    {
                        List<TransactionPaymentsDTO> paymentlist = transactionPaymentsDTOList.Where(x => x.TransactionId == transactionDTO.TransactionId).ToList();
                        transactionDTO.TrxPaymentsDTOList.AddRange(paymentlist);
                    }
                }
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }
    }
}

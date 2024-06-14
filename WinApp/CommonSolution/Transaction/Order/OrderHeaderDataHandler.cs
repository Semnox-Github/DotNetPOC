/********************************************************************************************
 * Project Name - OrderHeaderDataHandler
 * Description  - OrderHeaderDataHandler  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     01-Feb-2021    Girish Kundar       Modified : Urban Piper changes
 *2.130.0     01-Jun-2021    Fiona Lishal        Modified for Delivery Order enhancements for F&B
 *2.140.0     01-Dec-2021    Girish Kundar       Modified : View Open order Ui changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Data;
using System.Globalization;

namespace Semnox.Parafait.Transaction
{
    class OrderHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private string passPhrase;
        private static readonly Dictionary<OrderHeaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<OrderHeaderDTO.SearchByParameters, string>
        {
                {OrderHeaderDTO.SearchByParameters.ORDER_ID,"OrderHeader.OrderId"},
                {OrderHeaderDTO.SearchByParameters.TABLE_ID,"OrderHeader.TableId"},
                {OrderHeaderDTO.SearchByParameters.TRANSACTION_NUMBER,"trx_header.trx_no"},
                {OrderHeaderDTO.SearchByParameters.RESERVATION_CODE,"b.ReservationCode"},
                {OrderHeaderDTO.SearchByParameters.TRANSACTION_ID,"trx_header.TrxId"},
                {OrderHeaderDTO.SearchByParameters.PHONE_NUMBER,"Contact.Attribute1"},
                {OrderHeaderDTO.SearchByParameters.IS_ACTIVE, "OrderHeader.IsActive"},
                {OrderHeaderDTO.SearchByParameters.SITE_ID, "OrderHeader.site_id"},
                {OrderHeaderDTO.SearchByParameters.CARD_NUMBER, " ISNULL(TrxCards.card_number, OrderCards.card_number) "},
                {OrderHeaderDTO.SearchByParameters.TABLE_NUMBER, " ISNULL(TableNumber + '-' + facilityName, OrderHeader.TableNumber) "},
                {OrderHeaderDTO.SearchByParameters.CUSTOMER_NAME, " ISNULL(OrderHeader.CustomerName, p.FirstName + (CASE WHEN p.LastName IS NULL THEN '' ELSE ' ' END) +ISNULL(p.LastName, '')) "},
                {OrderHeaderDTO.SearchByParameters.TRANSACTION_STATUS, " trx_header.status "},
                {OrderHeaderDTO.SearchByParameters.TRANSACTION_STATUS_LIST, " trx_header.status "},
                {OrderHeaderDTO.SearchByParameters.LAST_UPDATED_DATE, " OrderHeader.LastUpdateTime "},
                {OrderHeaderDTO.SearchByParameters.ORDER_GUID, " OrderHeader.Guid"}
        };



        private const string SELECT_QUERY = @" SELECT OrderHeader.OrderId,
                                                        OrderHeader.TableNumber,
                                                        ISNULL(TableNumber + '-' + facilityName, OrderHeader.TableNumber) FacilityTableNumber,
                                                        ISNULL(OrderHeader.CustomerName, p.FirstName + (CASE WHEN p.LastName IS NULL THEN '' ELSE ' ' END) +ISNULL(p.LastName, '')) CustomerName,
                                                        OrderHeader.WaiterName,
                                                        OrderHeader.Remarks,
                                                        OrderHeader.OrderStatus,
                                                        OrderHeader.POSMachineId,
                                                        OrderHeader.UserId,
                                                        trx_header.TrxDate OrderDate,
                                                        OrderHeader.LastUpdateTime,
                                                        OrderHeader.CardId,
                                                        OrderHeader.Guid,
                                                        OrderHeader.site_id,
                                                        OrderHeader.SynchStatus,
                                                        OrderHeader.TableId,
                                                        OrderHeader.MasterEntityId,
                                                        OrderHeader.GuestCount,
                                                        OrderHeader.OrderStatusId,
                                                        OrderHeader.IsActive,
                                                        OrderHeader.CreatedBy,
                                                        OrderHeader.CreationDate,
                                                        OrderHeader.LastUpdatedBy,
                                                        OrderHeader.TransactionOrderTypeId,    
                                                        trx_header.TrxId, 
                                                        trx_header.TrxNetAmount, 
                                                        ISNULL(POSMachines.POSName, trx_header.pos_machine) as POSMachineName,
                                                        ISNULL(OrderCards.card_number, TrxCards.card_number) as CardNumber,
                                                        b.ReservationCode
                                                        FROM OrderHeader  
                                                        LEFT OUTER JOIN trx_header ON trx_header.OrderId = OrderHeader.OrderId
                                                        LEFT OUTER JOIN Cards OrderCards ON OrderHeader.CardId = OrderCards.card_id
                                                        LEFT OUTER JOIN Cards TrxCards ON trx_header.PrimaryCardId = TrxCards.card_id
                                                        LEFT OUTER JOIN POSMachines ON POSMachines.POSMachineId = OrderHeader.POSMachineId
                                                        LEFT OUTER JOIN FacilityTables ft on ft.tableId = OrderHeader.tableId
                                                        LEFT OUTER JOIN CheckInFacility f on f.FacilityId = ft.FacilityId
                                                        LEFT OUTER JOIN customers c on trx_header.customerId = c.customer_id
                                                        LEFT OUTER JOIN Profile p on c.ProfileId = p.Id
                                                        LEFT OUTER JOIN Bookings b on b.TrxId = trx_header.TrxId ";

        /// <summary>
        /// Default constructor of OrderHeaderDataHandler class
        /// </summary>
        public OrderHeaderDataHandler(string passPhrase, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            this.passPhrase = passPhrase;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating OrderHeader Record.
        /// </summary>
        /// <param name="orderHeaderDTO">OrderHeader type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(OrderHeaderDTO orderHeaderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(orderHeaderDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderId", orderHeaderDTO.OrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableNumber", orderHeaderDTO.TableNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerName", orderHeaderDTO.CustomerName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@WaiterName", orderHeaderDTO.WaiterName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", orderHeaderDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderStatus", orderHeaderDTO.Status.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", orderHeaderDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserId", orderHeaderDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", orderHeaderDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableId", orderHeaderDTO.TableId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GuestCount", orderHeaderDTO.GuestCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderStatusId", orderHeaderDTO.OrderStatusId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", orderHeaderDTO.IsActive ? "Y" : "N"));

            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", orderHeaderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@transactionOrderTypeId", orderHeaderDTO.TransactionOrderTypeId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the OrderHeader record to the database
        /// </summary>
        public OrderHeaderDTO InsertOrderHeader(OrderHeaderDTO orderHeaderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(orderHeaderDTO, userId, siteId);
            string query = @"INSERT INTO OrderHeader
                                        ( 
                                            TableNumber,
                                            CustomerName,
                                            WaiterName,
                                            Remarks,
                                            OrderStatus,
                                            POSMachineId,
                                            UserId,
                                            OrderDate,
                                            CardId,
                                            TableId,
                                            GuestCount,
                                            OrderStatusId,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdateTime,
                                            LastUpdatedBy,
                                            GUID,
                                            site_id,
                                            MasterEntityId,
                                            TransactionOrderTypeId
                                        )
                                        VALUES
                                        (  @TableNumber,
                                            @CustomerName,
                                            @WaiterName,
                                            @Remarks,
                                            @OrderStatus,
                                            @POSMachineId,
                                            @UserId,
                                            Getdate(),
                                            @CardId,
                                            @TableId,
                                            @GuestCount,
                                            @OrderStatusId,
                                            @IsActive,
                                            @CreatedBy,
                                            Getdate(),
                                            Getdate(),
                                            @LastUpdatedBy,
                                            NewId(),
                                            @site_id,
                                            @MasterEntityId,
                                            @transactionOrderTypeId
                                        )
                                        SELECT * FROM OrderHeader WHERE OrderId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(orderHeaderDTO, userId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    orderHeaderDTO.OrderId = Convert.ToInt32(dt.Rows[0]["OrderId"]);
                    orderHeaderDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastUpdateTime"]);
                    orderHeaderDTO.CreatedBy = userId;
                    orderHeaderDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                    orderHeaderDTO.Guid = Convert.ToString(dt.Rows[0]["guid"]);
                    orderHeaderDTO.LastUpdatedBy = userId;
                    orderHeaderDTO.SiteId = siteId;
                    orderHeaderDTO.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"]);
                }
            }
            catch (Exception ex)
            {
                log.LogVariableState("OrderHeaderDTO", orderHeaderDTO);
                log.Error("Error occured while inserting the OrderHeader record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderHeaderDTO);
            return orderHeaderDTO;
        }

        public OrderHeaderDTO UpdateOrderHeader(OrderHeaderDTO orderHeaderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(orderHeaderDTO, userId, siteId);
            string query = @"UPDATE OrderHeader SET 
                            TableNumber = @TableNumber,
                            CustomerName = @CustomerName,
                            WaiterName = @WaiterName,
                            Remarks= @Remarks,
                            OrderStatus= @OrderStatus,
                            POSMachineId = @POSMachineId,
                            UserId = @UserId,
                            CardId = @CardId,
                            TableId = @TableId,
                            GuestCount = @GuestCount,
                            OrderStatusId=@OrderStatusId,
                            IsActive = @IsActive,
                            LastUpdateTime = Getdate(),
                            LastUpdatedBy = @LastUpdatedBy,
                            -- site_id = @site_id,
                            MasterEntityId =  @MasterEntityId
                            WHERE  OrderId = @OrderId
                            SELECT * FROM OrderHeader WHERE OrderId = @OrderId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(orderHeaderDTO, userId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    orderHeaderDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastUpdateTime"]);
                    orderHeaderDTO.LastUpdatedBy = userId;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating the OrderHeader record", ex);
                log.LogVariableState("OrderHeaderDTO", orderHeaderDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderHeaderDTO);
            return orderHeaderDTO;
        }

        private OrderStatus GetOrderStatus(string status)
        {
            log.LogMethodEntry(status);
            OrderStatus orderStatus;
            try
            {
                orderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), status, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while parsing the contact type", ex);
                throw ex;
            }
            log.LogMethodExit(orderStatus);
            return orderStatus;
        }

        private List<OrderHeaderDTO> CreateOrderHeaderDTOList(SqlDataReader reader)
        {
            log.LogMethodEntry(reader);
            Dictionary<int, OrderHeaderDTO> orderHeaderDTODictionary = new Dictionary<int, OrderHeaderDTO>();
            List<OrderHeaderDTO> orderHeaderDTOList = new List<OrderHeaderDTO>();
            int orderId = reader.GetOrdinal("OrderId");
            int tableNumber = reader.GetOrdinal("TableNumber");
            int facilityTableNumber = reader.GetOrdinal("FacilityTableNumber");
            int customerName = reader.GetOrdinal("CustomerName");
            int waiterName = reader.GetOrdinal("WaiterName");
            int remarks = reader.GetOrdinal("Remarks");
            int orderStatus = reader.GetOrdinal("OrderStatus");
            int pOSMachineId = reader.GetOrdinal("POSMachineId");
            int pOSMachineName = reader.GetOrdinal("POSMachineName");
            int userId = reader.GetOrdinal("UserId");
            int orderDate = reader.GetOrdinal("OrderDate");
            int cardId = reader.GetOrdinal("CardId");
            int cardNumber = reader.GetOrdinal("CardNumber");
            int trxNetAmount = reader.GetOrdinal("TrxNetAmount");
            int tableId = reader.GetOrdinal("TableId");
            int guestCount = reader.GetOrdinal("GuestCount");
            int orderStatusId = reader.GetOrdinal("OrderStatusId");
            int transactionId = reader.GetOrdinal("TrxId");
            int isActive = reader.GetOrdinal("IsActive");
            int createdBy = reader.GetOrdinal("CreatedBy");
            int creationDate = reader.GetOrdinal("CreationDate");
            int lastUpdateTime = reader.GetOrdinal("LastUpdateTime");
            int lastUpdatedBy = reader.GetOrdinal("LastUpdatedBy");
            int site_id = reader.GetOrdinal("site_id");
            int masterEntityId = reader.GetOrdinal("MasterEntityId");
            int transactionOrderTypeId = reader.GetOrdinal("TransactionOrderTypeId");
            int synchStatus = reader.GetOrdinal("SynchStatus");
            int guid = reader.GetOrdinal("Guid");
            int reservationCode = reader.GetOrdinal("ReservationCode");
            while (reader.Read())
            {
                OrderHeaderDTO orderHeaderDTO;
                if (orderHeaderDTODictionary.ContainsKey(reader.GetInt32(orderId)) == false)
                {
                    orderHeaderDTO = new OrderHeaderDTO(reader.IsDBNull(orderId) ? -1 : reader.GetInt32(orderId),
                                        reader.IsDBNull(tableNumber) ? "" : reader.GetString(tableNumber),
                                        reader.IsDBNull(facilityTableNumber) ? "" : reader.GetString(facilityTableNumber),
                                        reader.IsDBNull(customerName) ? "" : reader.GetString(customerName),
                                        reader.IsDBNull(waiterName) ? "" : reader.GetString(waiterName),
                                        reader.IsDBNull(remarks) ? "" : reader.GetString(remarks),
                                        GetOrderStatus(reader.IsDBNull(orderStatus) ? "" : reader.GetString(orderStatus)),
                                        reader.IsDBNull(pOSMachineId) ? -1 : reader.GetInt32(pOSMachineId),
                                        reader.IsDBNull(pOSMachineName) ? "" : reader.GetString(pOSMachineName),
                                        reader.IsDBNull(userId) ? -1 : reader.GetInt32(userId),
                                        reader.IsDBNull(orderDate) ? DateTime.MinValue : reader.GetDateTime(orderDate),
                                        reader.IsDBNull(cardId) ? -1 : reader.GetInt32(cardId),
                                        reader.IsDBNull(cardNumber) ? "" : reader.GetString(cardNumber),
                                        reader.IsDBNull(trxNetAmount) ? 0 : (decimal)reader.GetDouble(trxNetAmount),
                                        reader.IsDBNull(tableId) ? -1 : reader.GetInt32(tableId),
                                        reader.IsDBNull(guestCount) ? 0 : reader.GetInt32(guestCount),
                                        reader.IsDBNull(orderStatusId) ? -1 : reader.GetInt32(orderStatusId),
                                        reader.IsDBNull(isActive) ? true : reader.GetString(isActive) == "Y",
                                        reader.IsDBNull(creationDate) ? DateTime.MinValue : reader.GetDateTime(creationDate),
                                        reader.IsDBNull(createdBy) ? "" : reader.GetString(createdBy),
                                        reader.IsDBNull(lastUpdateTime) ? DateTime.MinValue : reader.GetDateTime(lastUpdateTime),
                                        reader.IsDBNull(lastUpdatedBy) ? "" : reader.GetString(lastUpdatedBy),
                                        reader.IsDBNull(site_id) ? -1 : reader.GetInt32(site_id),
                                        reader.IsDBNull(guid) ? "" : reader.GetGuid(guid).ToString(),
                                        reader.IsDBNull(synchStatus) ? false : reader.GetBoolean(synchStatus),
                                        reader.IsDBNull(masterEntityId) ? -1 : reader.GetInt32(masterEntityId),
                                        reader.IsDBNull(transactionOrderTypeId) ? -1 : reader.GetInt32(transactionOrderTypeId),
                                        reader.IsDBNull(reservationCode) ? "" : reader.GetString(reservationCode));

                    orderHeaderDTOList.Add(orderHeaderDTO);
                    orderHeaderDTODictionary.Add(orderHeaderDTO.OrderId, orderHeaderDTO);
                }
                else
                {
                    orderHeaderDTO = orderHeaderDTODictionary[reader.GetInt32(orderId)];
                    orderHeaderDTO.Amount += reader.IsDBNull(trxNetAmount) ? 0 : (decimal)reader.GetDouble(trxNetAmount);
                }
                if (reader.IsDBNull(transactionId) == false)
                {
                    orderHeaderDTO.TransactionIdList.Add(reader.GetInt32(transactionId));
                }
            }
            log.LogMethodExit(orderHeaderDTOList);
            return orderHeaderDTOList;
        }

        private string GetTransactionNumber(int trxId)
        {
            log.LogMethodEntry(trxId);
            string result = string.Empty;
            string selectQuery = "Select trx_no from trx_header WHERE TrxId = @TrxId";
            SqlParameter[] selectOrderHeaderParameters = new SqlParameter[1];
            selectOrderHeaderParameters[0] = new SqlParameter("@TrxId", trxId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, selectOrderHeaderParameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                result = dataRow["trx_no"].ToString();
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the OrderHeader data of passed orderHeader Id
        /// </summary>
        /// <param name="orderHeaderId">integer type parameter</param>
        /// <returns>Returns OrderHeaderDTO</returns>
        public OrderHeaderDTO GetOrderHeaderDTO(int orderHeaderId, bool openTransactionsOnly)
        {
            log.LogMethodEntry(orderHeaderId);
            OrderHeaderDTO result = null;
            string selectQuery = SELECT_QUERY + " WHERE OrderHeader.OrderId = @OrderId";
            if (openTransactionsOnly)
            {
                selectQuery = selectQuery + " AND trx_header.Status in ('OPEN','INITIATED','ORDERED','BOOKING', 'PREPARED') ";
            }

            SqlParameter[] selectOrderHeaderParameters = new SqlParameter[1];
            selectOrderHeaderParameters[0] = new SqlParameter("@OrderId", orderHeaderId);
            List<OrderHeaderDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, selectOrderHeaderParameters, sqlTransaction, CreateOrderHeaderDTOList);
            if (list != null)
            {
                result = list[0];
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<OrderHeaderDTO> GetOpenOrderHeaderDTOList(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchParameters,
                                                              int POSTypeId,
                                                              int userId,
                                                              int POSMachineId,
                                                              string POSMachineName,
                                                              bool enableOrderShareAcrossPOSCounters,
                                                              bool enableOrderShareAcrossUsers,
                                                              bool enableOrderShareAcrossPOS)
        {
            log.LogMethodEntry(searchParameters);
            string selectQuery = @"SELECT OrderHeader.OrderId,
                                    OrderHeader.TableNumber,
                                    ISNULL(TableNumber + '-' + facilityName, OrderHeader.TableNumber) FacilityTableNumber,
                                    ISNULL(OrderHeader.CustomerName, p.FirstName + (CASE WHEN p.LastName IS NULL THEN '' ELSE ' ' END) +ISNULL(p.LastName, '')) CustomerName,
                                    OrderHeader.WaiterName,
                                    OrderHeader.Remarks,
                                    OrderHeader.OrderStatus,
                                    OrderHeader.POSMachineId,
                                    OrderHeader.UserId,
                                    trx_header.TrxDate OrderDate,
                                    OrderHeader.LastUpdateTime,
                                    OrderHeader.CardId,
                                    OrderHeader.Guid,
                                    OrderHeader.site_id,
                                    OrderHeader.SynchStatus,
                                    OrderHeader.TableId,
                                    OrderHeader.MasterEntityId,
                                    OrderHeader.GuestCount,
                                    OrderHeader.OrderStatusId,
                                    OrderHeader.IsActive,
                                    OrderHeader.CreatedBy,
                                    OrderHeader.CreationDate,
                                    OrderHeader.LastUpdatedBy,
                                    OrderHeader.TransactionOrderTypeId,
                                    trx_header.TrxId, 
                                    trx_header.TrxNetAmount, 
                                    ISNULL(POSMachines.POSName, trx_header.pos_machine) as POSMachineName,
                                    ISNULL(OrderCards.card_number, TrxCards.card_number) as CardNumber,
                                    b.ReservationCode
                                    FROM OrderHeader  
                                    LEFT OUTER JOIN trx_header ON trx_header.OrderId = OrderHeader.OrderId
                                    LEFT OUTER JOIN Cards OrderCards ON OrderHeader.CardId = OrderCards.card_id
                                    LEFT OUTER JOIN Cards TrxCards ON trx_header.PrimaryCardId = TrxCards.card_id
                                    LEFT OUTER JOIN POSMachines ON POSMachines.POSMachineId = OrderHeader.POSMachineId
                                    LEFT OUTER JOIN FacilityTables ft on ft.tableId = OrderHeader.tableId
                                    LEFT OUTER JOIN CheckInFacility f on f.FacilityId = ft.FacilityId
                                    LEFT OUTER JOIN customers c on trx_header.customerId = c.customer_id
                                    LEFT OUTER JOIN Profile p on c.ProfileId = p.Id
                                    LEFT OUTER JOIN Bookings b on b.TrxId = trx_header.TrxId";
            string whereClause = GetWhereClause(searchParameters);
            string joiner = string.IsNullOrWhiteSpace(whereClause) ? " where " : " and ";
            selectQuery = selectQuery + whereClause + joiner + @" trx_header.status in ('OPEN','INITIATED','ORDERED', 'PREPARED')
                                    and (@enableOrderShareAcrossPOSCounters = 1 or isnull(trx_header.POSTypeId, -1) = @POSTypeId)
                                    and (@enableOrderShareAcrossUsers = 1 or trx_header.user_id = @userId)
                                    and (@enableOrderShareAcrossPOS = 1 or (trx_header.POSMachineId = @POSMachineId or trx_header.POS_Machine = @POSMachineName))
                                    order by trxdate ";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@POSTypeId", POSTypeId),
                                                             new SqlParameter("@userId", userId),
                                                             new SqlParameter("@POSMachineId", POSMachineId),
                                                             new SqlParameter("@POSMachineName", POSMachineName),
                                                             new SqlParameter("@enableOrderShareAcrossPOSCounters", enableOrderShareAcrossPOSCounters),
                                                             new SqlParameter("@enableOrderShareAcrossUsers", enableOrderShareAcrossUsers),
                                                             new SqlParameter("@enableOrderShareAcrossPOS", enableOrderShareAcrossPOS),
                                                             dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase)};
            List<OrderHeaderDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, parameters, sqlTransaction, CreateOrderHeaderDTOList);
            if (list != null && list.Any())
            {
                foreach (OrderHeaderDTO orderHeaderDTO in list)
                {
                    if (orderHeaderDTO.TransactionIdList !=  null && orderHeaderDTO.TransactionIdList.Any())
                    {
                        foreach (int id in orderHeaderDTO.TransactionIdList)
                        {
                            orderHeaderDTO.TransactionNoList.Add(GetTransactionNumber(id));
                        }
                    }
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Gets the OrderHeaderDTO list matching the UserId
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of orderHeaderDTO matching the search criteria</returns>
        public List<OrderHeaderDTO> GetOrderHeaderDTOList(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            string selectQuery = SELECT_QUERY;
            selectQuery = SELECT_QUERY + GetWhereClause(searchParameters);
            List<OrderHeaderDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase) }, sqlTransaction, CreateOrderHeaderDTOList);
            log.LogMethodExit(list);
            return list;
        }

        private static string GetWhereClause(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string whereClause = string.Empty;
            if (searchParameters == null || searchParameters.Count == 0)
            {
                log.LogMethodExit(string.Empty, "search parameters is empty");
                return whereClause;
            }
            string joiner = "";
            StringBuilder query = new StringBuilder(" where ");
            foreach (KeyValuePair<OrderHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? " " : " and ";
                    {
                        if (searchParameter.Key.Equals(OrderHeaderDTO.SearchByParameters.ORDER_ID) ||
                            searchParameter.Key.Equals(OrderHeaderDTO.SearchByParameters.TABLE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                        }
                        else if (searchParameter.Key.Equals(OrderHeaderDTO.SearchByParameters.ORDER_GUID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value+ "'");
                        }
                        else if (searchParameter.Key.Equals(OrderHeaderDTO.SearchByParameters.TRANSACTION_STATUS_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + searchParameter.Value + ") ");
                        }
                        else if (searchParameter.Key.Equals(OrderHeaderDTO.SearchByParameters.TRANSACTION_ID))
                        {
                            query.Append(joiner + " EXISTS(SELECT 1 FROM OrderHeader oh, trx_header th WHERE  oh.OrderId = th.OrderId AND th.trxid = " + searchParameter.Value + " and OrderHeader.OrderId = oh.OrderId) ");
                        }
                        else if (searchParameter.Key.Equals(OrderHeaderDTO.SearchByParameters.TRANSACTION_NUMBER))
                        {
                            query.Append(joiner + " EXISTS(SELECT 1 FROM OrderHeader oh, trx_header th WHERE oh.OrderId = th.OrderId AND th.trx_no like N'%" + searchParameter.Value + "%' and OrderHeader.OrderId = oh.OrderId) ");
                        }
                        else if (searchParameter.Key.Equals(OrderHeaderDTO.SearchByParameters.PHONE_NUMBER))
                        {
                            query.Append(joiner + @" (EXISTS(SELECT 1 
			                                                 FROM OrderHeader oh, trx_header th, customers cu, Profile p, Contact c, ContactType ct
			                                                 WHERE oh.OrderId = th.OrderId 
			                                                 AND cu.customer_id = th.customerId
			                                                 AND cu.ProfileId = p.Id
			                                                 AND c.ProfileId = p.Id
			                                                 AND c.ContactTypeId = ct.Id
			                                                 AND ct.Name = 'PHONE'
			                                                 AND CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,c.Attribute1)) like N'%" + searchParameter.Value + @"%'
			                                                 AND OrderHeader.OrderId = oh.OrderId)
                                                      OR EXISTS(SELECT 1 
			                                                 FROM OrderHeader oh, trx_header th, cards cd, customers cu, Profile p, Contact c, ContactType ct
			                                                 WHERE oh.OrderId = th.OrderId 
                                                             AND th.PrimaryCardId = cd.card_id
                                                             AND cd.customer_id = cu.customer_id
			                                                 AND cu.ProfileId = p.Id
			                                                 AND c.ProfileId = p.Id
			                                                 AND c.ContactTypeId = ct.Id
			                                                 AND ct.Name = 'PHONE'
			                                                 AND CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,c.Attribute1)) like N'%" + searchParameter.Value + @"%'
			                                                 AND OrderHeader.OrderId = oh.OrderId))");
                        }
                        else if (searchParameter.Key == OrderHeaderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == OrderHeaderDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = '"+ searchParameter.Value+"'");
                        }
                        else if (searchParameter.Key.Equals(OrderHeaderDTO.SearchByParameters.LAST_UPDATED_DATE))
                        {
                            query.Append(joiner + " IsNull ( " + DBSearchParameters[searchParameter.Key] + ", GetDate()) >= '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                        }
                    }
                    count++;
                }
                else
                {
                    log.Error("Ends-GetAllOrderHeaderList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                    log.LogMethodExit("Throwing exception- The query parameter does not exist ");
                    throw new Exception("The query parameter does not exist");
                }
            }
            whereClause = query.ToString();
            log.LogMethodExit(whereClause);
            return whereClause;
        }
    }
}

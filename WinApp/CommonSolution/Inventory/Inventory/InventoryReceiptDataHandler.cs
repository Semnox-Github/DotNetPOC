/********************************************************************************************
* Project Name - Inventory Receipt Data Handler
* Description  - Data handler of the inventory receipt class
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        10-Aug-2016   Raghuveera          Created 
*2.70.2        13-Jul-2019   Deeksha             Modifications as per three tier standard
*2.70.2        15-Nov-2019   Archana             Modified to add HAS_PRODUCT_ID search parameter
*2.70.2        09-Dec-2019   Jinto Thomas        Removed siteid from update query
*2.110.0     16-Dec-2020     Abhishek            Modified for web API changes
*2.120.0     12-Apr-2021     Mushahid Faizan     Web Inventory Changes
*2.130       04-Jun-2021     Girish Kundar       Modified - POS stock changes
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory  Receipt  - Handles insert, update and select of inventory receipt objects
    /// </summary>
    public class InventoryReceiptDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Receipt  ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        ///  Dictionary for searching Parameters for the inventory receipt object.
        /// </summary>
        private static readonly Dictionary<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string> DBSearchParameters = new Dictionary<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>
            {
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIPT_ID, "ReceiptId"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.DOCUMENT_TYPE_ID, "DocumentTypeID"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_FROM_DATE, "ReceiveDate"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_TO_DATE, "ReceiveDate"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVED_DATE, "ReceiveDate"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID, "site_id"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_ID, "PurchaseOrderId"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.GATE_PASS_NUMBER, "GatePassNumber"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.GRN, "GRN"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_TO_LOCATION_ID, "ReceiveToLocationID"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVED_BY, "ReceivedBy"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDOR_BILL_NUMBER, "VendorBillNumber"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDORNAME, "VendorName"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.ORDERNUMBER, "OrderNumber"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_IDS, "PurchaseOrderId"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.HAS_PRODUCT_ID, ""},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.DOCUMENT_TYPE_ID_LIST, "DocumentTypeID"},
                {InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDORNAME_LIST, "VendorName"},
                 {InventoryReceiptDTO.SearchByInventoryReceiptParameters.IS_ACTIVE, "IsActive"}
            };

        /// <summary>
        /// Default constructor of InventoryReceiptDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public InventoryReceiptDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryReceipt Record.
        /// </summary>
        /// <param name="inventoryReceiptDTO">InventoryReceiptDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryReceiptDTO inventoryReceiptDTO, string loginId, int siteId)
        {
            double verifyDouble = 0;
            log.LogMethodEntry(inventoryReceiptDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@receiptId", inventoryReceiptDTO.ReceiptId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorBillNumber", string.IsNullOrEmpty(inventoryReceiptDTO.VendorBillNumber) ? DBNull.Value : (object)inventoryReceiptDTO.VendorBillNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gatePassNumber", string.IsNullOrEmpty(inventoryReceiptDTO.GatePassNumber) ? DBNull.Value : (object)inventoryReceiptDTO.GatePassNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gRN", string.IsNullOrEmpty(inventoryReceiptDTO.GRN) ? DBNull.Value : (object)inventoryReceiptDTO.GRN));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseOrderId", inventoryReceiptDTO.PurchaseOrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(inventoryReceiptDTO.Remarks) ? DBNull.Value : (object)inventoryReceiptDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receiveDate", inventoryReceiptDTO.ReceiveDate.Equals(DateTime.MinValue) ? DBNull.Value : (object)inventoryReceiptDTO.ReceiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receivedBy", string.IsNullOrEmpty(inventoryReceiptDTO.ReceivedBy) ? DBNull.Value : (object)inventoryReceiptDTO.ReceivedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceSystemID", string.IsNullOrEmpty(inventoryReceiptDTO.SourceSystemID) ? DBNull.Value : (object)inventoryReceiptDTO.SourceSystemID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receiveToLocationID", inventoryReceiptDTO.ReceiveToLocationID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@documentTypeID", inventoryReceiptDTO.DocumentTypeID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryReceiptDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MarkupPercent", (Double.TryParse(inventoryReceiptDTO.MarkupPercent.ToString(), out verifyDouble) == false) || Double.IsNaN(inventoryReceiptDTO.MarkupPercent) || inventoryReceiptDTO.MarkupPercent.ToString() == string.Empty ? DBNull.Value : (object)inventoryReceiptDTO.MarkupPercent));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the inventory receipt record to the database
        /// </summary>
        /// <param name="inventoryReceiptDTO">InventoryReceiptDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">sqlTransaction</param>
        /// <returns>Returns inserted record id</returns>
        public InventoryReceiptDTO Insert(InventoryReceiptDTO inventoryReceiptDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryReceiptDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[Receipt] 
                                                        ( 
                                                        VendorBillNumber,
                                                        GatePassNumber,
                                                        GRN,
                                                        PurchaseOrderId,
                                                        Remarks,
                                                        ReceiveDate,
                                                        ReceivedBy,
                                                        SourceSystemID,
                                                        ReceiveToLocationID,
                                                        DocumentTypeID,
                                                        MasterEntityId,
                                                        Guid,
                                                        site_id,
                                                        MarkupPercent,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate
                                                        ) 
                                                values 
                                                        ( 
                                                         @vendorBillNumber,
                                                         @gatePassNumber,
                                                         @gRN,
                                                         @purchaseOrderId,
                                                         @remarks,
                                                         @receiveDate,
                                                         @receivedBy,
                                                         @sourceSystemID,
                                                         @receiveToLocationID,
                                                         @documentTypeID,
                                                         @masterEntityId,
                                                         NEWID(),
                                                         @siteid,
                                                         @MarkupPercent,
                                                         @createdBy,
                                                         GETDATE(),
                                                         @lastUpdatedBy,
                                                         GETDATE()
                                                        ) SELECT* FROM Receipt WHERE ReceiptId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryReceiptDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryReceiptDTO(inventoryReceiptDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting inventoryReceiptDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryReceiptDTO);
            return inventoryReceiptDTO;
        }

        /// <summary>
        /// Updates the Inventory receipt record
        /// </summary>
        /// <param name="inventoryReceiptDTO">InventoryReceiptDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param> 
        /// <param name="SQLTrx">SQL trx</param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryReceiptDTO Update(InventoryReceiptDTO inventoryReceiptDTO, string loginId, int siteId)
        {

            log.LogMethodEntry(inventoryReceiptDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[Receipt]
                                    SET  VendorBillNumber=@vendorBillNumber,
                                             GatePassNumber=@gatePassNumber,
                                             GRN=@gRN,
                                             PurchaseOrderId=@purchaseOrderId,
                                             Remarks=@remarks,
                                             ReceiveDate=@receiveDate,
                                             ReceivedBy=@receivedBy,
                                             SourceSystemID=@sourceSystemID,
                                             ReceiveToLocationID=@receiveToLocationID,
                                             DocumentTypeID=@documentTypeID,
                                             --site_id=@siteid,
                                             MasterEntityId=@masterEntityId,
                                             MarkupPercent=@MarkupPercent ,                                            
                                             LastUpdateDate = GETDATE(),
                                             LastUpdatedBy = @lastUpdatedBy
                                             
                                       WHERE ReceiptId =@receiptId 
                                    SELECT * FROM Receipt WHERE ReceiptId = @receiptId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryReceiptDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryReceiptDTO(inventoryReceiptDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryReceiptDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryReceiptDTO);
            return inventoryReceiptDTO;
        }

        /// <summary>
        /// Converts the Data row object to InventoryReceiptDTO class type
        /// </summary>
        /// <param name="inventoryReceiptDataRow">InventoryReceipt DataRow</param>
        /// <returns>Returns InventoryReceipt</returns>
        private InventoryReceiptDTO GetInventoryReceiptDTO(DataRow inventoryReceiptDataRow)
        {
            log.LogMethodEntry(inventoryReceiptDataRow);
            InventoryReceiptDTO inventoryReceiptDataObject = new InventoryReceiptDTO(Convert.ToInt32(inventoryReceiptDataRow["ReceiptId"]),
                                            inventoryReceiptDataRow["VendorBillNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["VendorBillNumber"]),
                                            inventoryReceiptDataRow["GatePassNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["GatePassNumber"]),
                                            inventoryReceiptDataRow["GRN"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["GRN"]),
                                            inventoryReceiptDataRow["PurchaseOrderId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiptDataRow["PurchaseOrderId"]),
                                            inventoryReceiptDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["Remarks"]),
                                            inventoryReceiptDataRow["ReceiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryReceiptDataRow["ReceiveDate"]),
                                            inventoryReceiptDataRow["ReceivedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["ReceivedBy"]),
                                            inventoryReceiptDataRow["SourceSystemID"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["SourceSystemID"]),
                                            inventoryReceiptDataRow["ReceiveToLocationID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiptDataRow["ReceiveToLocationID"]),
                                            inventoryReceiptDataRow["DocumentTypeID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiptDataRow["DocumentTypeID"]),
                                            inventoryReceiptDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(inventoryReceiptDataRow["IsActive"]),
                                            inventoryReceiptDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["Guid"]),
                                            inventoryReceiptDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiptDataRow["site_id"]),
                                            inventoryReceiptDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryReceiptDataRow["SynchStatus"]),
                                            inventoryReceiptDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiptDataRow["MasterEntityId"]),
                                            inventoryReceiptDataRow["VendorName"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["VendorName"]),
                                            inventoryReceiptDataRow["OrderNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["OrderNumber"]),
                                            inventoryReceiptDataRow["ReceiptAmount"] == DBNull.Value ? 0 : Convert.ToDouble(inventoryReceiptDataRow["ReceiptAmount"]),
                                            inventoryReceiptDataRow["OrderDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryReceiptDataRow["OrderDate"]),
                                            inventoryReceiptDataRow["MarkupPercent"] == DBNull.Value ? double.NaN : Convert.ToDouble(inventoryReceiptDataRow["MarkupPercent"]),
                                            new List<InventoryReceiveLinesDTO>(),
                                            inventoryReceiptDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["CreatedBy"]),
                                            inventoryReceiptDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryReceiptDataRow["CreationDate"]),
                                            inventoryReceiptDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiptDataRow["LastUpdatedBy"]),
                                            inventoryReceiptDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryReceiptDataRow["LastUpdateDate"])

                                            );
            log.LogMethodExit(inventoryReceiptDataObject);
            return inventoryReceiptDataObject;
        }


        /// <summary>
        /// Delete the record from the Receipt database based on ReceiptId
        /// <param name="receiptId">receiptId </param>
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int receiptId)
        {
            log.LogMethodEntry(receiptId);
            string query = @"DELETE  
                             FROM Receipt
                             WHERE Receipt.ReceiptId = @receiptId";
            SqlParameter parameter = new SqlParameter("@receiptId", receiptId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryReceiptDTO">InventoryReceiptDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryReceiptDTO(InventoryReceiptDTO inventoryReceiptDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryReceiptDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryReceiptDTO.ReceiptId = Convert.ToInt32(dt.Rows[0]["ReceiptId"]);
                inventoryReceiptDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                inventoryReceiptDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryReceiptDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryReceiptDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryReceiptDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryReceiptDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Inventory Receipt data of passed asset asset Group Id
        /// </summary>
        /// <param name="inventoryReceiptId">integer type parameter</param>
        /// <returns>Returns InventoryReceiptDTO</returns>
        public InventoryReceiptDTO GetInventoryReceipt(int inventoryReceiptId)
        {
            log.LogMethodEntry(inventoryReceiptId);
            string selectInventoryReceiptQuery = @"select *
                                                   from (
                                                                SELECT r.*, v.name VendorName, OrderNumber, (select sum(amount) from PurchaseOrderreceive_line where ReceiptId = r.receiptId) receiptAmount, OrderDate
                                                          FROM Receipt r, purchaseorder po, vendor v
                                                          where r.purchaseorderid = po.purchaseorderid
                                                         and po.vendorid = v.vendorid)v
                                                  where ReceiptId = @inventoryReceiptId";
            SqlParameter[] selectInventoryReceiptParameters = new SqlParameter[1];
            selectInventoryReceiptParameters[0] = new SqlParameter("@inventoryReceiptId", inventoryReceiptId);
            DataTable inventoryReceipt = dataAccessHandler.executeSelectQuery(selectInventoryReceiptQuery, selectInventoryReceiptParameters, sqlTransaction);
            if (inventoryReceipt.Rows.Count > 0)
            {
                DataRow InventoryReceiptRow = inventoryReceipt.Rows[0];
                InventoryReceiptDTO inventoryReceiptDataObject = GetInventoryReceiptDTO(InventoryReceiptRow);
                log.LogMethodExit(inventoryReceiptDataObject);
                return inventoryReceiptDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the InventoryReceiptDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        ///// <returns>Returns the list of InventoryReceiptDTO matching the search criteria</returns>
        public List<InventoryReceiptDTO> GetInventoryReceiptList(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            parameters.Clear();
            string selectInventoryReceiptQuery = @"select *
                                                   from
                                                   (
                                                    select r.*, 
                                                    v.name VendorName, ordernumber, (select sum(amount) from PurchaseOrderreceive_line where ReceiptId = r.receiptId) receiptAmount, OrderDate
                                                    from Receipt r join purchaseorder po on r.purchaseorderid = po.purchaseorderid
                                                        join vendor v on po.vendorid = v.vendorid
                                                    )v";

            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<InventoryReceiptDTO> inventoryReceiptDTOList = null;
            selectInventoryReceiptQuery = selectInventoryReceiptQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryReceiptQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryReceiptDTOList = new List<InventoryReceiptDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryReceiptDTO inventoryReceiptDTO = GetInventoryReceiptDTO(dataRow);
                    inventoryReceiptDTOList.Add(inventoryReceiptDTO);
                }
            }
            log.LogMethodExit(inventoryReceiptDTOList);
            return inventoryReceiptDTOList;
        }



        /// <summary>
        /// Gets the InventoryReceiptDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryReceiptDTO matching the search criteria</returns>
        private string GetFilterQuery(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIPT_ID
                            || searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_ID
                            || searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.DOCUMENT_TYPE_ID
                            || searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_TO_LOCATION_ID)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.HAS_PRODUCT_ID)
                        {
                            query.Append(joiner + @" exists (select 1 
                                                               from PurchaseOrderReceive_Line rl  
                                                              where v.receiptId = rl.ReceiptId 
                                                               and rl.ProductId = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVED_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_FROM_DATE)
                        {
                            decimal businessStartTime = Convert.ToDecimal(searchParameter.Value);
                            string dateQuery = @"  ReceiveDate between  case when datepart(hour, getdate())
                                                                                   between 0 and " + businessStartTime + " then dateadd(hour, 6,convert(datetime, convert(date, getdate() - 1)))" +
                                                                                   " else dateadd(hour, " + businessStartTime + ", convert(datetime, convert(date, getdate()))) end and case when " +
                                                                                   "datepart(hour, getdate()) between 0 and " + businessStartTime + " then dateadd(hour," + businessStartTime + ",convert(datetime, convert(date, getdate())))" +
                                                                                   " else dateadd(hour, " + businessStartTime + ", convert(datetime, convert(date, getdate() + 1))) end";
                            query.Append(joiner + dateQuery);
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.GATE_PASS_NUMBER
                            || searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.GRN
                            || searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDOR_BILL_NUMBER
                            || searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.ORDERNUMBER
                            || searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDORNAME)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_IDS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.DOCUMENT_TYPE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDORNAME_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                    count++;
                }

            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public string GetNextSeqNo(string sequenceName)
        {
            log.LogMethodEntry(sequenceName);
            DataTable dTable = dataAccessHandler.executeSelectQuery(@"declare @value varchar(20)
                                exec GetNextSeqValue N'" + sequenceName + "', @value out, -1 "
                                   + " select @value", null, sqlTransaction);
            try
            {
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    object o = dTable.Rows[0][0];
                    if (o != null)
                    {
                        log.LogMethodExit(o);
                        return (o.ToString());
                    }
                    else
                    {
                        log.LogMethodExit();
                        return "";
                    }
                }

            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                return "";
            }

            log.LogMethodExit();
            return "";
        }

        /// <summary>
        /// Gets the InventoryReceiptDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryReceiptDTO matching the search criteria</returns>
        public List<InventoryReceiptDTO> GetInventoryReceiptsList(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters,
                                                                  int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryReceiptDTO> inventoryReceiptDTOList = new List<InventoryReceiptDTO>();
            parameters.Clear();
            string selectInventoryReceiptQuery = @"select *
                                                   from
                                                   (
                                                    select r.*, 
                                                    v.name VendorName, ordernumber, (select sum(amount) from PurchaseOrderreceive_line where ReceiptId = r.receiptId) receiptAmount, OrderDate
                                                    from Receipt r join purchaseorder po on r.purchaseorderid = po.purchaseorderid
                                                        join vendor v on po.vendorid = v.vendorid
                                                    )v";
            string selectQuery = selectInventoryReceiptQuery + GetFilterQuery(searchParameters);
            if (currentPage > 0 || pageSize > 0)
            {
                selectQuery += " ORDER BY ReceiptId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryReceiptDTOList = new List<InventoryReceiptDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryReceiptDTO inventoryReceiptDTO = GetInventoryReceiptDTO(dataRow);
                    inventoryReceiptDTOList.Add(inventoryReceiptDTO);
                }
            }
            log.LogMethodExit(inventoryReceiptDTOList);
            return inventoryReceiptDTOList;
        }

        /// <summary>
        /// Returns the no of InventoryReceipts matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetInventoryReceiptsCount(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int requisitionDTOCount = 0;
            //string selectQuery = SELECT_QUERY;
            string selectQuery = @"select *
                                                   from
                                                   (
                                                    select r.*, 
                                                    v.name VendorName, ordernumber, (select sum(amount) from PurchaseOrderreceive_line where ReceiptId = r.receiptId) receiptAmount, OrderDate
                                                    from Receipt r join purchaseorder po on r.purchaseorderid = po.purchaseorderid
                                                        join vendor v on po.vendorid = v.vendorid
                                                    )v";

            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(requisitionDTOCount);
            return requisitionDTOCount;
        }
    }
}

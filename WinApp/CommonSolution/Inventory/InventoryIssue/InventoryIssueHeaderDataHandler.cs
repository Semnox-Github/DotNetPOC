/********************************************************************************************
 * Project Name - Inventory Issue Header Data Handler
 * Description  - Data handler of the inventory issue headerclass
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        09-Aug-2016   Raghuveera        Created 
 * 2.60       22-March-2019 Girish Kundar     Adding Issue Number and Reference Number
 *2.60.2      06-Jun-2019   Akshay Gulaganji  Code merge from Development to WebManagementStudio
 *2.70.2        14-Jul-2019   Deeksha           Modifications as per 3 tier changes.
 *2.70.2        09-Dec-2019   Jinto Thomas      Removed siteid from update query. 
 *2.110.0     28-Dec-2020     Mushahid Faizan      Modified : Web Inventory Changes
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
    /// Inventory Issue Header  - Handles insert, update and select of inventory issue header objects
    /// </summary>
    public class InventoryIssueHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        List<SqlParameter> parameters = new List<SqlParameter>();
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM InventoryIssueHeader AS ish ";
        /// <summary>
        /// Dictionary for searching Parameters for the InventoryIssueHeader  object.
        /// </summary>
        private static readonly Dictionary<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string> DBSearchParameters = new Dictionary<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>
            {
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.INVENTORY_ISSUE_ID, "ish.InventoryIssueId"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.DOCUMENT_TYPE_ID, "ish.DocumentTypeID"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG, "ish.IsActive"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_DATE, "ish.IssueDate"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_FROM_DATE, "ish.IssueDate"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_TO_DATE, "ish.IssueDate"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.MASTER_ENTITY_ID,"ish.MasterEntityId"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.SITE_ID, "ish.site_id"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.PURCHASE_ORDER_ID, "ish.PurchaseOrderId"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.REQUISITION_ID, "ish.RequisitionID"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ORIGINAL_REFERENCE_GUID,"ish.OriginalReferenceGUID"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.FROM_SITE_ID, "ish.FromSiteID"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.TO_SITE_ID, "ish.ToSiteID"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.STATUS, "ish.Status"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.GUID, "ish.Guid"},
                {InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.GUID_ID_LIST, "ish.Guid"},
                { InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_NUMBER,"ish.IssueNumber" }
             
            };


        /// <summary>
        /// Default constructor of InventoryIssueHeaderDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public InventoryIssueHeaderDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryIssueHeaderDataHandler Record.
        /// </summary>
        /// <param name="InventoryIssueHeaderDTO">InventoryIssueHeaderDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryIssueHeaderDTO inventoryIssueHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryIssueHeaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@inventoryIssueId", inventoryIssueHeaderDTO.InventoryIssueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@documentTypeID", inventoryIssueHeaderDTO.DocumentTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseOrderId", inventoryIssueHeaderDTO.PurchaseOrderId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionID", inventoryIssueHeaderDTO.RequisitionID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@issueDate", inventoryIssueHeaderDTO.IssueDate.Equals(DateTime.MinValue) ? DBNull.Value:(object) inventoryIssueHeaderDTO.IssueDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(inventoryIssueHeaderDTO.Remarks) ? DBNull.Value : (object)inventoryIssueHeaderDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deliveryNoteNumber", string.IsNullOrEmpty(inventoryIssueHeaderDTO.DeliveryNoteNumber) ? DBNull.Value : (object)inventoryIssueHeaderDTO.DeliveryNoteNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deliveryNoteDate", inventoryIssueHeaderDTO.DeliveryNoteDate.Equals(DateTime.MinValue) ? DBNull.Value:(object) inventoryIssueHeaderDTO.DeliveryNoteDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@supplierInvoiceNumber", string.IsNullOrEmpty(inventoryIssueHeaderDTO.SupplierInvoiceNumber) ? DBNull.Value : (object)inventoryIssueHeaderDTO.SupplierInvoiceNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@supplierInvoiceDate", inventoryIssueHeaderDTO.SupplierInvoiceDate.Equals(DateTime.MinValue) ? DBNull.Value : (object) inventoryIssueHeaderDTO.SupplierInvoiceDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryIssueHeaderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", inventoryIssueHeaderDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromSiteID", inventoryIssueHeaderDTO.FromSiteID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toSiteID", inventoryIssueHeaderDTO.ToSiteID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(inventoryIssueHeaderDTO.Status) ? DBNull.Value : (object)inventoryIssueHeaderDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalReferenceGUID", string.IsNullOrEmpty(inventoryIssueHeaderDTO.OriginalReferenceGUID) ? DBNull.Value : (object)inventoryIssueHeaderDTO.OriginalReferenceGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@issueNumber", string.IsNullOrEmpty(inventoryIssueHeaderDTO.IssueNumber) ? DBNull.Value : (object)inventoryIssueHeaderDTO.IssueNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@referenceNumber", string.IsNullOrEmpty(inventoryIssueHeaderDTO.ReferenceNumber) ? DBNull.Value : (object)inventoryIssueHeaderDTO.ReferenceNumber));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the asset type record to the database
        /// </summary>
        /// <param name="inventoryIssueHeaderDTO">InventoryIssueHeaderDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SQL Transactions </param>
        /// <returns>Returns inserted record id</returns>

        public InventoryIssueHeaderDTO InsertInventoryIssueHeader(InventoryIssueHeaderDTO inventoryIssueHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryIssueHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[InventoryIssueHeader]
                                                        ( 
                                                        DocumentTypeID,
                                                        PurchaseOrderId,
                                                        RequisitionID,
                                                        IssueDate,
                                                        Remarks,
                                                        DeliveryNoteNumber,
                                                        DeliveryNoteDate,
                                                        SupplierInvoiceNumber,
                                                        SupplierInvoiceDate,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        FromSiteID,
                                                        ToSiteID,
                                                        Status,
                                                        OriginalReferenceGUID,
                                                        IssueNumber,
                                                        ReferenceNumber
                                                        ) 
                                                values 
                                                        (                                                         
                                                         @documentTypeID,
                                                         @purchaseOrderId,
                                                         @requisitionID,
                                                         @issueDate,
                                                         @remarks,
                                                         @deliveryNoteNumber,
                                                         @deliveryNoteDate,
                                                         @supplierInvoiceNumber,
                                                         @supplierInvoiceDate,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId,
                                                         @fromSiteID,
                                                         @toSiteID,
                                                         @status,
                                                         @originalReferenceGUID,
                                                         @issueNumber,
                                                         @referenceNumber
                                                        )     
                                SELECT * FROM InventoryIssueHeader WHERE InventoryIssueId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryIssueHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryIssueHeaderDTO(inventoryIssueHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting inventoryIssueHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(inventoryIssueHeaderDTO);
            return inventoryIssueHeaderDTO;
        }


        /// <summary>
        /// Updates the Inventory Issue header record
        /// </summary>
        /// <param name="inventoryIssueHeaderDTO">InventoryIssueHeaderDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SQL Transactions </param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryIssueHeaderDTO UpdateInventoryIssueHeader(InventoryIssueHeaderDTO inventoryIssueHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryIssueHeaderDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[InventoryIssueHeader]
                                    SET  DocumentTypeID=@documentTypeID,
                                             PurchaseOrderId=@purchaseOrderId,
                                             RequisitionID=@requisitionID,
                                             IssueDate=@issueDate,
                                             Remarks=@remarks,
                                             DeliveryNoteNumber=@deliveryNoteNumber,
                                             DeliveryNoteDate=@deliveryNoteDate,
                                             SupplierInvoiceNumber=@supplierInvoiceNumber,
                                             SupplierInvoiceDate=@supplierInvoiceDate,                                            
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             FromSiteID =  @fromSiteID,
                                             TOSiteID =  @toSiteID,
                                             Status = @status,
                                             ReferenceNumber= @referenceNumber
                                               
                                       WHERE InventoryIssueId =@inventoryIssueId 
                                    SELECT * FROM InventoryIssueHeader WHERE InventoryIssueId = @inventoryIssueId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryIssueHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryIssueHeaderDTO(inventoryIssueHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryIssueHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryIssueHeaderDTO);
            return inventoryIssueHeaderDTO;
        }

        /// <summary>
        /// Delete the record from the InventoryIssueHeader database based on InventoryIssueId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int inventoryIssueId)
        {
            log.LogMethodEntry(inventoryIssueId);
            string query = @"DELETE  
                             FROM InventoryIssueHeader
                             WHERE InventoryIssueHeader.InventoryIssueId = @inventoryIssueId";
            SqlParameter parameter = new SqlParameter("@inventoryIssueId", inventoryIssueId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryIssueHeaderDTO">inventoryIssueHeaderDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryIssueHeaderDTO(InventoryIssueHeaderDTO inventoryIssueHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryIssueHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryIssueHeaderDTO.InventoryIssueId = Convert.ToInt32(dt.Rows[0]["InventoryIssueId"]);
                inventoryIssueHeaderDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                inventoryIssueHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryIssueHeaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryIssueHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryIssueHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryIssueHeaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
       
        /// <summary>
        /// Insert Inventory Transaction
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="LocationId"></param>
        /// <param name="receiveLineID"></param>
        /// <param name="detailQuantity"></param>
        /// <param name="price"></param>
        /// <param name="taxPercentage"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public int insertInventoryTransaction(int ProductId, int LocationId, int receiveLineID, double detailQuantity, double price, double taxPercentage, string userId, int siteId)
        {
            log.LogMethodEntry(ProductId, LocationId, receiveLineID, detailQuantity, price, taxPercentage, userId, siteId);
            string insertInventoryTransactionQuery = @"INSERT INTO [dbo].[InventoryTransaction]
                                       ([ParafaitTrxId]
                                       ,[TrxDate]
                                       ,[Username]
                                       ,[POSMachine]
                                       ,[ProductId]
                                       ,[LocationId]
                                       ,[Quantity]
                                       ,[SalePrice]
                                       ,[TaxPercentage]
                                       ,[TaxInclusivePrice]
                                       ,[LineId]
                                       ,[POSMachineId]
                                       ,[site_id]
                                       ,[Guid]
                                       ,[SynchStatus]
                                       ,[MasterEntityId]
                                       ,[InventoryTransactionTypeID]
                                       ,[LotID])
                                 VALUES
                                       (null
                                       ,getdate()
                                       ,@username
                                       ,null
                                       ,@ProductId
                                       ,@LocationId
                                       ,@Quantity
                                       ,@SalePrice
                                       ,@TaxPercentage
                                       ,@TaxInclusivePrice
                                       ,@LineId
                                       ,null
                                       ,@site_id
                                       ,newid()
                                       ,null
                                       ,null
                                       ,(select top 1 LookupValueid from Lookups l, LookupValues v where l.LookupId = v.LookupId and LookupValue = 'DirectIssue' and l.lookupname = 'INVENTORY_TRANSACTION_TYPE')
                                       ,@LotID)SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateInventoryTransactionParameters = new List<SqlParameter>();
            updateInventoryTransactionParameters.Add(new SqlParameter("@username", userId));
            updateInventoryTransactionParameters.Add(new SqlParameter("@ProductId", ProductId));
            updateInventoryTransactionParameters.Add(new SqlParameter("@LocationId", LocationId));
            updateInventoryTransactionParameters.Add(new SqlParameter("@Quantity", detailQuantity));
            updateInventoryTransactionParameters.Add(new SqlParameter("@SalePrice", price));
            updateInventoryTransactionParameters.Add(new SqlParameter("@TaxPercentage", taxPercentage));
            updateInventoryTransactionParameters.Add(new SqlParameter("@TaxInclusivePrice", DBNull.Value));
            updateInventoryTransactionParameters.Add(new SqlParameter("@LineId", receiveLineID));
            if (siteId == -1)
            {
                updateInventoryTransactionParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            }
            else
            {
                updateInventoryTransactionParameters.Add(new SqlParameter("@site_id", siteId));
            }
            updateInventoryTransactionParameters.Add(new SqlParameter("@LotID", DBNull.Value));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertInventoryTransactionQuery, updateInventoryTransactionParameters.ToArray(),sqlTransaction);
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

       

        /// <summary>
        /// Converts the Data row object to InventoryIssueHeaderDTO class type
        /// </summary>
        /// <param name="inventoryIssueHeaderDataRow">InventoryIssueHeader DataRow</param>
        /// <returns>Returns InventoryIssueHeader</returns>
        private InventoryIssueHeaderDTO GetInventoryIssueHeaderDTO(DataRow inventoryIssueHeaderDataRow)
        {
            log.LogMethodEntry(inventoryIssueHeaderDataRow);
            InventoryIssueHeaderDTO inventoryIssueHeaderDataObject = new InventoryIssueHeaderDTO(Convert.ToInt32(inventoryIssueHeaderDataRow["InventoryIssueId"]),
                                            inventoryIssueHeaderDataRow["DocumentTypeID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueHeaderDataRow["DocumentTypeID"]),
                                            inventoryIssueHeaderDataRow["PurchaseOrderId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueHeaderDataRow["PurchaseOrderId"]),
                                            inventoryIssueHeaderDataRow["RequisitionID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueHeaderDataRow["RequisitionID"]),
                                            inventoryIssueHeaderDataRow["IssueDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryIssueHeaderDataRow["IssueDate"]),
                                            inventoryIssueHeaderDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["Remarks"]),
                                            inventoryIssueHeaderDataRow["DeliveryNoteNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["DeliveryNoteNumber"]),
                                            inventoryIssueHeaderDataRow["DeliveryNoteDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryIssueHeaderDataRow["DeliveryNoteDate"]),
                                            inventoryIssueHeaderDataRow["SupplierInvoiceNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["SupplierInvoiceNumber"]),
                                            inventoryIssueHeaderDataRow["SupplierInvoiceDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryIssueHeaderDataRow["SupplierInvoiceDate"]),
                                            inventoryIssueHeaderDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(inventoryIssueHeaderDataRow["IsActive"]),
                                            inventoryIssueHeaderDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["CreatedBy"]),
                                            inventoryIssueHeaderDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryIssueHeaderDataRow["CreationDate"]),
                                            inventoryIssueHeaderDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["LastUpdatedBy"]),
                                            inventoryIssueHeaderDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryIssueHeaderDataRow["LastupdatedDate"]),
                                            inventoryIssueHeaderDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["Guid"]),
                                            inventoryIssueHeaderDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueHeaderDataRow["site_id"]),
                                            inventoryIssueHeaderDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryIssueHeaderDataRow["SynchStatus"]),
                                            inventoryIssueHeaderDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueHeaderDataRow["MasterEntityId"]),
                                            inventoryIssueHeaderDataRow["FromSiteID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueHeaderDataRow["FromSiteID"]),
                                            inventoryIssueHeaderDataRow["ToSiteID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueHeaderDataRow["ToSiteID"]),
                                            inventoryIssueHeaderDataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["Status"]),
                                            inventoryIssueHeaderDataRow["OriginalReferenceGUID"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["OriginalReferenceGUID"]),
                                            inventoryIssueHeaderDataRow["IssueNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["IssueNumber"]),
                                            inventoryIssueHeaderDataRow["ReferenceNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueHeaderDataRow["ReferenceNumber"])
                                            );
            log.LogMethodExit(inventoryIssueHeaderDataObject);
            return inventoryIssueHeaderDataObject;
        }

        /// <summary>
        /// Gets the InventoryIssueHeader data of passed id 
        /// </summary>
        /// <param name="id">id of InventoryIssueHeader is passed as parameter</param>
        /// <returns>Returns InventoryIssueHeader</returns>
        public InventoryIssueHeaderDTO GetInventoryIssueHeader(int inventoryIssueId)
        {
            log.LogMethodEntry(inventoryIssueId);
            InventoryIssueHeaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE ish.InventoryIssueId= @inventoryIssueId";
            SqlParameter parameter = new SqlParameter("@inventoryIssueId", inventoryIssueId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetInventoryIssueHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// </summary>
        /// <param name="sequenceName">sequenceName</param>
        /// <param name="SQLTrx">SqlTransaction</param>
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
            catch(Exception ex)
            {
                log.LogMethodExit(ex);
                return "";
            }
            log.LogMethodExit();
            return "";

        }

        /// <summary>
        /// Gets the InventoryIssueHeaderDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryIssueHeaderDTO matching the search criteria</returns>
        public List<InventoryIssueHeaderDTO> GetInventoryIssueHeaderLists(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = new List<InventoryIssueHeaderDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage > 0 || pageSize > 0)
            {
                selectQuery += " ORDER BY ish.InventoryIssueId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryIssueHeaderDTOList = new List<InventoryIssueHeaderDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryIssueHeaderDTO InventoryIssueHeaderDTO = GetInventoryIssueHeaderDTO(dataRow);
                    inventoryIssueHeaderDTOList.Add(InventoryIssueHeaderDTO);
                }
            }
            log.LogMethodExit(inventoryIssueHeaderDTOList);
            return inventoryIssueHeaderDTOList;
        }

        /// <summary>
        /// Returns the no of InventoryIssueHeader matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetInventoryIssueHeadersCount(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int inventoryIssueHeaderDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryIssueHeaderDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(inventoryIssueHeaderDTOCount);
            return inventoryIssueHeaderDTOCount;
        }


        /// <summary>
        /// Gets the InventoryIssueHeaderDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object</param>
        /// <returns>Returns the list of InventoryIssueHeaderDTO matching the search criteria</returns>
        public List<InventoryIssueHeaderDTO> GetInventoryIssueHeaderList(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters) //modified.
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = null;
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryIssueHeaderDTOList = new List<InventoryIssueHeaderDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryIssueHeaderDTO inventoryIssueHeaderDTO = GetInventoryIssueHeaderDTO(dataRow);
                    inventoryIssueHeaderDTOList.Add(inventoryIssueHeaderDTO);
                }
            }
            log.LogMethodExit(inventoryIssueHeaderDTOList);
            return inventoryIssueHeaderDTOList;
        }

        public string GetFilterQuery(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = count == 0 ? string.Empty : " and ";

                        if (searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.INVENTORY_ISSUE_ID
                            || searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.PURCHASE_ORDER_ID
                            || searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.REQUISITION_ID
                            || searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.DOCUMENT_TYPE_ID
                            || searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.FROM_SITE_ID
                            || searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.TO_SITE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.STATUS
                            || searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ORIGINAL_REFERENCE_GUID
                            || searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_NUMBER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                        else if (searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG)   // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.GUID_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
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
            log.LogMethodExit();
            return query.ToString();
        }
    }
}
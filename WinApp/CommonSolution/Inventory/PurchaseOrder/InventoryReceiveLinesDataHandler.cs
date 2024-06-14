/********************************************************************************************
* Project Name -Inventory Receive Lines DTO
* Description  -Data object of inventory receive lines
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00          10-Aug-2016   Raghuveera          Created 
********************************************************************************************
* *1.00        29-Aug-2016   Suneetha            Modified 
********************************************************************************************
* 2.60         10-Apr-2019    Girish             Modified :  Added PurchaseTaxId and TaxAmount Columns                              
*                                                Table PurchaseOrderReceive_Line
* 2.70.2       16-Jul-2019    Deeksha            Modification as per three tier changes
*2.70.2        09-Dec-2019    Jinto Thomas       Removed siteid from update query 
*2.100.0       27-Jul-2020    Deeksha            Added UOMID field.
*2.100.1       01-Jul-2021    Deeksha            RecievedBy Column Issue During Inventory Recieve
*2.110.0       21-Dec-2020    Abhishek           Modified: added GetInventoryReceiveLinesDTOList() for web API  
*2.110.0       28-Dec-2020    Prajwal S          Modified and Added Methods for WEB API changes.
*2.120.0       28-Apr-2021    Mushahid Faizan    Modified GetSQLParameters() for PriceInTickets.
 *2.130        04-Jun-2021     Girish Kundar     Modified - POS stock changes Added Remarks column
********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory  Receive Lines  - Handles insert, update and select of inventory receive lines objects
    /// </summary>
    public class InventoryReceiveLinesDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM PurchaseOrderReceive_Line AS pl ";
        List<SqlParameter> parameters = new List<SqlParameter>();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;

        private static readonly Dictionary<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string> DBSearchParameters = new Dictionary<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>
            {
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_RECEIVE_LINE_ID, "PurchaseOrderReceiveLineId"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_ID, "PurchaseOrderId"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PRODUCT_ID, "ProductId"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.VENDOR_ITEM_CODE,"VendorItemCode"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.QUANTITY, "Quantity"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.LOCATION_ID, "LocationId"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.IS_RECEIVED, "IsReceived"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_LINE_ID, "PurchaseOrderLineId"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.VENDOR_BILL_NUMBER, "VendorBillNumber"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.RECEIPT_ID, "ReceiptId"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.SITE_ID, "site_id"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_IDS, "PurchaseOrderId"},
                {InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.UOM_ID, "UOMId"}
            };
   

        /// <summary>
        /// Default constructor of InventoryReceiveLinesDataHandler class
        /// </summary>
        public InventoryReceiveLinesDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryReceiveLinesDataHandler Record.
        /// </summary>
        /// <param name="InventoryReceiveLinesDTO">InventoryReceiveLinesDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryReceiveLinesDTO inventoryReceiveLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryReceiveLinesDTO, loginId, siteId);
            double verifyDouble = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderReceiveLineId", inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderId", inventoryReceiveLinesDTO.PurchaseOrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", inventoryReceiveLinesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@locationId", inventoryReceiveLinesDTO.LocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseOrderLineId", inventoryReceiveLinesDTO.PurchaseOrderLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receiptId", inventoryReceiveLinesDTO.ReceiptId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionId", inventoryReceiveLinesDTO.RequisitionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionLineId", inventoryReceiveLinesDTO.RequisitionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseTaxId", inventoryReceiveLinesDTO.PurchaseTaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(inventoryReceiveLinesDTO.Description) ? DBNull.Value : (object)inventoryReceiveLinesDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@vendorItemCode", string.IsNullOrEmpty(inventoryReceiveLinesDTO.VendorItemCode) ? DBNull.Value : (object)inventoryReceiveLinesDTO.VendorItemCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isReceived", string.IsNullOrEmpty(inventoryReceiveLinesDTO.IsReceived) ? DBNull.Value : (object)inventoryReceiveLinesDTO.IsReceived));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxInclusive", string.IsNullOrEmpty(inventoryReceiveLinesDTO.TaxInclusive) ? DBNull.Value : (object)inventoryReceiveLinesDTO.TaxInclusive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@vendorBillNumber", string.IsNullOrEmpty(inventoryReceiveLinesDTO.VendorBillNumber) ? DBNull.Value : (object)inventoryReceiveLinesDTO.VendorBillNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receivedBy", string.IsNullOrEmpty(inventoryReceiveLinesDTO.ReceivedBy) ? DBNull.Value : (object)inventoryReceiveLinesDTO.ReceivedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceSystemID", string.IsNullOrEmpty(inventoryReceiveLinesDTO.SourceSystemID) ? DBNull.Value : (object)inventoryReceiveLinesDTO.SourceSystemID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@quantity", inventoryReceiveLinesDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@price", inventoryReceiveLinesDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxPercentage", inventoryReceiveLinesDTO.TaxPercentage < 0? DBNull.Value : (object)inventoryReceiveLinesDTO.TaxPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@amount", inventoryReceiveLinesDTO.Amount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxAmount", inventoryReceiveLinesDTO.TaxAmount <= 0 ? DBNull.Value:(object) inventoryReceiveLinesDTO.TaxAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@priceInTickets", (Double.TryParse(inventoryReceiveLinesDTO.PriceInTickets.ToString(), out verifyDouble) == false) || Double.IsNaN(inventoryReceiveLinesDTO.PriceInTickets) || inventoryReceiveLinesDTO.PriceInTickets.ToString() == "" ? DBNull.Value :(object)inventoryReceiveLinesDTO.PriceInTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryReceiveLinesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", inventoryReceiveLinesDTO.UOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receiveRemarks", inventoryReceiveLinesDTO.ReceiveRemarks));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the inventory receive lines record to the database
        /// </summary>
        /// <param name="inventoryReceiveLinesDTO">InventoryReceiveLinesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx"></param>
        /// <returns>Returns inserted record id</returns>
        public InventoryReceiveLinesDTO Insert(InventoryReceiveLinesDTO inventoryReceiveLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryReceiveLinesDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[PurchaseOrderReceive_Line]
                                                        (
                                                        PurchaseOrderId,
                                                        ProductId,
                                                        Description,
                                                        VendorItemCode,
                                                        Quantity,
                                                        LocationId,
                                                        IsReceived,
                                                        PurchaseOrderLineId,
                                                        Price,
                                                        tax_percentage,
                                                        amount,
                                                        tax_inclusive,
                                                        ReceiptId,
                                                        VendorBillNumber,
                                                        ReceivedBy,
                                                        Timestamp,
                                                        SourceSystemID,
                                                        RequisitionId,
                                                        RequisitionLineId,
                                                        MasterEntityId,
                                                        Guid,
                                                        site_id,
                                                        PriceInTickets,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdateDate,
                                                        PurchaseTaxId,
                                                        TaxAmount,
                                                        UOMId,
                                                        Remarks
                                                        ) 
                                                values 
                                                        ( 
                                                         @purchaseOrderId,
                                                         @productId,
                                                         @description,
                                                         @vendorItemCode,
                                                         @quantity,
                                                         @locationId,
                                                         @isReceived,
                                                         @purchaseOrderLineId,
                                                         @price,
                                                         @taxPercentage,
                                                         @amount,
                                                         @taxInclusive,
                                                         @receiptId,
                                                         @vendorBillNumber,
                                                         @receivedBy,                                                                                                                
                                                         Getdate(),
                                                         @sourceSystemID,
                                                         @requisitionId,
                                                         @requisitionLineId,
                                                         @masterEntityId,
                                                         NEWID(),
                                                         @site_id,
                                                         @priceInTickets,
                                                         @CreatedBy,
                                                         GETDATE(),
                                                         @LastUpdatedBy,
                                                         GETDATE(),
                                                         @purchaseTaxId,
                                                         @taxAmount,
                                                         @UOMId,@receiveRemarks)
                                                        SELECT * FROM PurchaseOrderReceive_Line WHERE PurchaseOrderReceiveLineId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryReceiveLinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryReceiveLinesDTO(inventoryReceiveLinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting inventoryReceiveLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(inventoryReceiveLinesDTO);
            return inventoryReceiveLinesDTO;
        }

        /// <summary>
        /// Updates the Inventory receipt record
        /// </summary>
        /// <param name="inventoryReceiveLinesDTO">InventoryReceiveLinesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx"></param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryReceiveLinesDTO Update(InventoryReceiveLinesDTO inventoryReceiveLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryReceiveLinesDTO, loginId);
            string query = @"UPDATE  [dbo].[PurchaseOrderReceive_Line]
                                    SET  PurchaseOrderId=@purchaseOrderId,
                                             ProductId=@productId,
                                             Description=@description,
                                             VendorItemCode=@vendorItemCode,
                                             Quantity=@quantity,
                                             LocationId=@locationId,
                                             IsReceived=@isReceived,
                                             PurchaseOrderLineId=@purchaseOrderLineId,
                                             Price=@price,
                                             tax_percentage=@taxPercentage,
                                             amount=@amount,
                                             tax_inclusive=@taxInclusive,
                                             ReceiptId=@receiptId,
                                             VendorBillNumber=@vendorBillNumber,
                                             ReceivedBy=@receivedBy,
                                             Timestamp=Getdate(),
                                             SourceSystemID=@sourceSystemID ,
                                             --site_id=@site_id,
                                             MasterEntityId=@masterEntityId,
                                             PriceInTickets = @priceInTickets,
                                             PurchaseTaxId = @purchaseTaxId,
                                             TaxAmount = @taxAmount ,
                                             UOMId = @UOMId ,
                                             Remarks = @receiveRemarks 
                                       where PurchaseOrderReceiveLineId = @PurchaseOrderReceiveLineId
            SELECT * FROM PurchaseOrderReceive_Line WHERE PurchaseOrderReceiveLineId = @PurchaseOrderReceiveLineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryReceiveLinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryReceiveLinesDTO(inventoryReceiveLinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryReceiveLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryReceiveLinesDTO);
            return inventoryReceiveLinesDTO;
        }


        /// <summary>
        /// Delete the record from the PurchaseOrderReceive_Line database based on PurchaseOrderReceive_LineID
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int purchaseOrderReceive_LineID)
        {
            log.LogMethodEntry(purchaseOrderReceive_LineID);
            string query = @"DELETE  
                             FROM PurchaseOrderReceive_Line
                             WHERE PurchaseOrderReceive_Line.PurchaseOrderReceiveLineId = @purchaseOrderReceiveLineId";
            SqlParameter parameter = new SqlParameter("@purchaseOrderReceiveLineId", purchaseOrderReceive_LineID);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryDocumentTypeDTO">InventoryDocumentTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryReceiveLinesDTO(InventoryReceiveLinesDTO inventoryReceiveLinesDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryReceiveLinesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId = Convert.ToInt32(dt.Rows[0]["PurchaseOrderReceiveLineId"]);
                inventoryReceiveLinesDTO.LastUpdatedDate = dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]);
                inventoryReceiveLinesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryReceiveLinesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryReceiveLinesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryReceiveLinesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryReceiveLinesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

    
        /// <summary>
        /// Converts the Data row object to InventoryReceiveLinesDTO class type
        /// </summary>
        /// <param name="inventoryReceiveLinesDataRow">InventoryReceiveLines DataRow</param>
        /// <returns>Returns InventoryReceiveLines</returns>
        private InventoryReceiveLinesDTO GetInventoryReceiveLinesDTO(DataRow inventoryReceiveLinesDataRow)
        {
            log.LogMethodEntry(inventoryReceiveLinesDataRow);
            InventoryReceiveLinesDTO inventoryReceiveLinesDataObject = new InventoryReceiveLinesDTO(Convert.ToInt32(inventoryReceiveLinesDataRow["PurchaseOrderReceiveLineId"]),
                                            inventoryReceiveLinesDataRow["PurchaseOrderId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["PurchaseOrderId"]),
                                            inventoryReceiveLinesDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["ProductId"]),
                                            inventoryReceiveLinesDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["Description"]),
                                            inventoryReceiveLinesDataRow["VendorItemCode"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["VendorItemCode"]),
                                            inventoryReceiveLinesDataRow["Quantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["Quantity"].ToString()),
                                            inventoryReceiveLinesDataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["locationId"]),
                                            inventoryReceiveLinesDataRow["IsReceived"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["IsReceived"]),
                                            inventoryReceiveLinesDataRow["PurchaseOrderLineId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["PurchaseOrderLineId"]),
                                            inventoryReceiveLinesDataRow["Price"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["Price"].ToString()),
                                            inventoryReceiveLinesDataRow["tax_percentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["tax_percentage"].ToString()),
                                            inventoryReceiveLinesDataRow["amount"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["amount"].ToString()),
                                            inventoryReceiveLinesDataRow["tax_inclusive"] == DBNull.Value ? "N" : inventoryReceiveLinesDataRow["tax_inclusive"].ToString(),
                                            //inventoryReceiveLinesDataRow["TaxId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["TaxId"]),
                                            inventoryReceiveLinesDataRow["ReceiptId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["ReceiptId"]),
                                            inventoryReceiveLinesDataRow["VendorBillNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["VendorBillNumber"]),
                                            inventoryReceiveLinesDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryReceiveLinesDataRow["Timestamp"]),
                                            inventoryReceiveLinesDataRow["SourceSystemID"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["SourceSystemID"]),
                                            inventoryReceiveLinesDataRow["ReceivedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["ReceivedBy"]),
                                            inventoryReceiveLinesDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["Guid"]),
                                            inventoryReceiveLinesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["site_id"]),
                                            inventoryReceiveLinesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryReceiveLinesDataRow["SynchStatus"]),
                                            inventoryReceiveLinesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["MasterEntityId"]),
                                            inventoryReceiveLinesDataRow["RequisitionId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["RequisitionId"]),
                                            inventoryReceiveLinesDataRow["RequisitionLineId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["RequisitionLineId"]),
                                            inventoryReceiveLinesDataRow["ProductCode"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["ProductCode"]),
                                            inventoryReceiveLinesDataRow["POQuantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["POQuantity"].ToString()),
                                            inventoryReceiveLinesDataRow["POUnitPrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["POUnitPrice"].ToString()),
                                            inventoryReceiveLinesDataRow["POTaxAmount"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["POTaxAmount"].ToString()),
                                            inventoryReceiveLinesDataRow["POSubtotal"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["POSubtotal"].ToString()),
                                            inventoryReceiveLinesDataRow["CurrentStock"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryReceiveLinesDataRow["CurrentStock"].ToString()),
                                            inventoryReceiveLinesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["CreatedBy"]),
                                            inventoryReceiveLinesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryReceiveLinesDataRow["CreationDate"]),
                                            inventoryReceiveLinesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["LastUpdatedBy"]),
                                            inventoryReceiveLinesDataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryReceiveLinesDataRow["LastupdateDate"]),
                                            new List<InventoryLotDTO>(),
                                            inventoryReceiveLinesDataRow["PriceInTickets"] == DBNull.Value ? double.NaN : Convert.ToDouble(inventoryReceiveLinesDataRow["PriceInTickets"]),
                                            inventoryReceiveLinesDataRow["PurchaseTaxId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["PurchaseTaxId"]),
                                            inventoryReceiveLinesDataRow["TaxAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(inventoryReceiveLinesDataRow["TaxAmount"].ToString()),
                                            inventoryReceiveLinesDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryReceiveLinesDataRow["UOMId"].ToString()),
                                            false,
                                            string.Empty,
                                            inventoryReceiveLinesDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryReceiveLinesDataRow["Remarks"].ToString())
                                            );
            log.LogMethodExit(inventoryReceiveLinesDataObject);
            return inventoryReceiveLinesDataObject;
        }

        /// <summary>
        /// Gets the Inventory Receipt data of passed Inventory Receive Lines Id
        /// </summary>
        /// <param name="inventoryReceiveLinesId">integer type parameter</param>
        /// <returns>Returns InventoryReceiveLinesDTO</returns>
        public InventoryReceiveLinesDTO GetInventoryReceiveLines(int inventoryReceiveLinesId)
        {
            log.LogMethodEntry(inventoryReceiveLinesId);
            string selectInventoryReceiveLinesQuery = @"select *
                                                        from (
		                                                        select prl.*,
                                                                    p.Code ProductCode,
			                                                        pol.Quantity POQuantity, 
			                                                        pol.UnitPrice POUnitPrice, 
			                                                        pol.TaxAmount POTaxAmount, 
			                                                        pol.SubTotal POSubTotal, 
			                                                        (select isnull(sum(inv.Quantity), 0)
			                                                         from Inventory inv 
			                                                         where inv.ProductId = prl.ProductId 
				                                                        and inv.LocationId = prl.LocationId
				                                                        ) CurrentStock 
		                                                        from PurchaseOrderReceive_Line prl, PurchaseOrder_Line pol, Product p 
		                                                        where prl.PurchaseOrderId = pol.PurchaseOrderId 
			                                                        and prl.PurchaseOrderLineId = pol.PurchaseOrderLineId 
			                                                        and p.ProductId = prl.ProductId )v
                                                       WHERE PurchaseOrderReceiveLineId = @inventoryReceiveLinesId";
            SqlParameter parameter = new SqlParameter("@inventoryReceiveLinesId", inventoryReceiveLinesId);
            DataTable inventoryReceiveLines = dataAccessHandler.executeSelectQuery(selectInventoryReceiveLinesQuery, new SqlParameter[] { parameter });
            if (inventoryReceiveLines.Rows.Count > 0)
            {
                DataRow InventoryReceiveLinesRow = inventoryReceiveLines.Rows[0];
                InventoryReceiveLinesDTO inventoryReceiveLinesDTO = GetInventoryReceiveLinesDTO(InventoryReceiveLinesRow);
                log.LogMethodExit( inventoryReceiveLinesDTO);
                return inventoryReceiveLinesDTO;
            }
            else
            {
                log.LogMethodExit( null);
                return null;
            }
        }

        /// <summary>
        /// Gets the InventoryReceiveLinesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryReceiveLinesDTO matching the search criteria</returns>
        public List<InventoryReceiveLinesDTO> GetInventoryReceiveLinesListDTO(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList = new List<InventoryReceiveLinesDTO>();
            parameters.Clear();
            string selectInventoryReceiveLinesQuery = @"select *
                                                        from (
		                                                        select prl.*,
                                                                    p.Code ProductCode,
			                                                        pol.Quantity POQuantity, 
			                                                        pol.UnitPrice POUnitPrice, 
			                                                        pol.TaxAmount POTaxAmount, 
			                                                        pol.SubTotal POSubTotal, 
			                                                        (select isnull(sum(inv.Quantity), 0)
			                                                         from Inventory inv 
			                                                         where inv.ProductId = prl.ProductId 
				                                                        and inv.LocationId = prl.LocationId
				                                                        ) CurrentStock 
		                                                        from PurchaseOrderReceive_Line prl, PurchaseOrder_Line pol, Product p 
		                                                        where prl.PurchaseOrderId = pol.PurchaseOrderId 
			                                                        and prl.PurchaseOrderLineId = pol.PurchaseOrderLineId 
			                                                        and p.ProductId = prl.ProductId )v";
            string selectQuery = selectInventoryReceiveLinesQuery + GetFilterQuery(searchParameters);
            if (currentPage > 0 || pageSize > 0)
            {
                selectQuery += " ORDER BY PurchaseOrderReceiveLineId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryReceiveLinesDTOList = new List<InventoryReceiveLinesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryReceiveLinesDTO inventoryReceiveLinesDTO = GetInventoryReceiveLinesDTO(dataRow);
                    inventoryReceiveLinesDTOList.Add(inventoryReceiveLinesDTO);
                }
            }
            log.LogMethodExit(inventoryReceiveLinesDTOList);
            return inventoryReceiveLinesDTOList;
        }

        /// <summary>
        /// Returns the no of Requisition matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetInventoryReceiveLinesCount(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int inventoryReceiveLinesDTOCount = 0;
            string selectInventoryReceiveLinesQuery = @"select *
                                                        from (
		                                                        select prl.*,
                                                                    p.Code ProductCode,
			                                                        pol.Quantity POQuantity, 
			                                                        pol.UnitPrice POUnitPrice, 
			                                                        pol.TaxAmount POTaxAmount, 
			                                                        pol.SubTotal POSubTotal, 
			                                                        (select isnull(sum(inv.Quantity), 0)
			                                                         from Inventory inv 
			                                                         where inv.ProductId = prl.ProductId 
				                                                        and inv.LocationId = prl.LocationId
				                                                        ) CurrentStock 
		                                                        from PurchaseOrderReceive_Line prl, PurchaseOrder_Line pol, Product p 
		                                                        where prl.PurchaseOrderId = pol.PurchaseOrderId 
			                                                        and prl.PurchaseOrderLineId = pol.PurchaseOrderLineId 
			                                                        and p.ProductId = prl.ProductId )v";
            string selectQuery = selectInventoryReceiveLinesQuery;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryReceiveLinesDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(inventoryReceiveLinesDTOCount);
            return inventoryReceiveLinesDTOCount;
        }

        /// <summary>
        /// Gets the InventoryReceiveLinesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="SQLTrx">Passing sql transaction</param>
        /// <returns>Returns the list of InventoryReceiveLinesDTO matching the search criteria</returns>

        public List<InventoryReceiveLinesDTO> GetInventoryReceiveLinesList(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> searchParameters) //modified
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList = null;
            string selectInventoryReceiveLinesQuery = @"select *
                                                        from (
		                                                        select prl.*,
                                                                    p.Code ProductCode,
			                                                        pol.Quantity POQuantity, 
			                                                        pol.UnitPrice POUnitPrice, 
			                                                        pol.TaxAmount POTaxAmount, 
			                                                        pol.SubTotal POSubTotal, 
			                                                        (select isnull(sum(inv.Quantity), 0)
			                                                         from Inventory inv 
			                                                         where inv.ProductId = prl.ProductId 
				                                                        and inv.LocationId = prl.LocationId
				                                                        ) CurrentStock 
		                                                        from PurchaseOrderReceive_Line prl, PurchaseOrder_Line pol, Product p 
		                                                        where prl.PurchaseOrderId = pol.PurchaseOrderId 
			                                                        and prl.PurchaseOrderLineId = pol.PurchaseOrderLineId 
			                                                        and p.ProductId = prl.ProductId )v";
            parameters.Clear();
            selectInventoryReceiveLinesQuery = selectInventoryReceiveLinesQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryReceiveLinesQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryReceiveLinesDTOList = new List<InventoryReceiveLinesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryReceiveLinesDTO InventoryReceiveLinesDTO = GetInventoryReceiveLinesDTO(dataRow);
                    inventoryReceiveLinesDTOList.Add(InventoryReceiveLinesDTO);
                }
            }
            log.LogMethodExit(inventoryReceiveLinesDTOList);
            return inventoryReceiveLinesDTOList;
        }

        public string GetFilterQuery(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            int count = 0;
           
            if (searchParameters != null)
            {
                string joiner;
                query = new StringBuilder(" where ");
                foreach (KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.RECEIPT_ID
                            || searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_ID
                            || searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_LINE_ID
                            || searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_RECEIVE_LINE_ID
                            || searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PRODUCT_ID
                            || searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.UOM_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.VENDOR_BILL_NUMBER
                            || searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.VENDOR_ITEM_CODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_IDS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
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
    




    /// <summary>
    /// Gets the InventoryReceiveLinesDTO list matching the search key
    /// </summary>
    /// <param name="receiptId">receiptId</param>
    /// <returns>Returns the list of InventoryReceiveLinesDTO matching the search criteria</returns>
    /// 29-Aug-2016 Modified - to get all non marketlist product
    public List<InventoryReceiveLinesDTO> GetNonMarketlistInventoryReceiveLines(int receiptId)
        {
            log.LogMethodEntry(receiptId);
            //int count = 0;
            List<InventoryReceiveLinesDTO> inventoryReceiveLinesList = null;
            string selectInventoryReceiveLinesQuery = @"select *
                                                        from (
		                                                        select prl.*,
                                                                    p.Code ProductCode,
			                                                        pol.Quantity POQuantity, 
			                                                        pol.UnitPrice POUnitPrice, 
			                                                        pol.TaxAmount POTaxAmount, 
			                                                        pol.SubTotal POSubTotal, 
			                                                        (select isnull(sum(inv.Quantity), 0)
			                                                         from Inventory inv 
			                                                         where inv.ProductId = prl.ProductId 
				                                                        and inv.LocationId = prl.LocationId
				                                                        ) CurrentStock 
		                                                        from PurchaseOrderReceive_Line prl, PurchaseOrder_Line pol, Product p 
		                                                        where prl.PurchaseOrderId = pol.PurchaseOrderId 
			                                                        and prl.PurchaseOrderLineId = pol.PurchaseOrderLineId 
			                                                        and p.ProductId = prl.ProductId
			                                                        and Isnull(p.MarketListItem, 0) = 0  )v
                                                         where ReceiptId=@receiptId";
            SqlParameter parameter = new SqlParameter("@receiptId", receiptId);
            DataTable inventoryReceiveLinesData = dataAccessHandler.executeSelectQuery(selectInventoryReceiveLinesQuery, new SqlParameter[] { parameter });
            if (inventoryReceiveLinesData.Rows.Count > 0)
            {
                inventoryReceiveLinesList = new List<InventoryReceiveLinesDTO>();
                foreach (DataRow inventoryReceiveLinesDataRow in inventoryReceiveLinesData.Rows)
                {
                    InventoryReceiveLinesDTO inventoryReceiveLinesDTO = GetInventoryReceiveLinesDTO(inventoryReceiveLinesDataRow);
                    inventoryReceiveLinesList.Add(inventoryReceiveLinesDTO);
                }
            }
            log.LogMethodExit(inventoryReceiveLinesList);
            return inventoryReceiveLinesList;
        }

        /// <summary>
        /// Gets the InventoryReceiveLinesDTO List for inventoryReceipts Id List
        /// </summary>
        /// <param name="inventoryReceiptsIdList">integer list parameter</param>
        /// <returns>Returns List of InventoryReceiveLinesDTO</returns>
        public List<InventoryReceiveLinesDTO> GetInventoryReceiveLinesDTOList(List<int> inventoryReceiptsIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(inventoryReceiptsIdList);
            List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOs = new List<InventoryReceiveLinesDTO>();
            string selectInventoryReceiveLinesQuery = @"select *
                                                        from (
		                                                        select prl.*,
                                                                    p.Code ProductCode,
			                                                        pol.Quantity POQuantity, 
			                                                        pol.UnitPrice POUnitPrice, 
			                                                        pol.TaxAmount POTaxAmount, 
			                                                        pol.SubTotal POSubTotal, 
			                                                        (select isnull(sum(inv.Quantity), 0)
			                                                         from Inventory inv 
			                                                         where inv.ProductId = prl.ProductId 
				                                                        and inv.LocationId = prl.LocationId
				                                                        ) CurrentStock 
		                                                        from @ReceiptIdList List, PurchaseOrderReceive_Line prl, PurchaseOrder_Line pol, Product p 
		                                                        where prl.PurchaseOrderId = pol.PurchaseOrderId 
			                                                        and prl.PurchaseOrderLineId = pol.PurchaseOrderLineId 
			                                                        and p.ProductId = prl.ProductId and prl.ReceiptId = List.Id)v";

            DataTable table = dataAccessHandler.BatchSelect(selectInventoryReceiveLinesQuery, "@ReceiptIdList", inventoryReceiptsIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                inventoryReceiveLinesDTOs = table.Rows.Cast<DataRow>().Select(x => GetInventoryReceiveLinesDTO(x)).ToList();
            }
            log.LogMethodExit(inventoryReceiveLinesDTOs);
            return inventoryReceiveLinesDTOs;
        }
    }
}

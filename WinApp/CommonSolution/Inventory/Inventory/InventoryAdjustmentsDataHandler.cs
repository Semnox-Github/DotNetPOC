/********************************************************************************************
 * Project Name -Inventory Adjustments DataHandler
 * Description  -Data object of inventory Adjustments
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Aug-2016   Amaresh          Created 
 *1.10        16-May-2017   Lakshminarayana  Modified   Added new serach filters.
 *2.70.2      13-Jul-2019   Deeksha          Modifications as per three tier standard
 *2.70.2      09-Dec-2019   Jinto Thomas     Removed site id from update query 
 *2.100.0     27-Jul-2020   Deeksha          Modified : Added UOMId field.
 *2.110.0     29-Dec-2020   Abhishek         Modified : added GetInventoryAdjustmentsCount() for web API changes
 *2.110.4     01-Oct-2021   Guru S A         Physical count performance fixes
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using Microsoft.SqlServer.Server;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory Adjustments - Handles insert, update and select of inventory Adjustments objects
    /// </summary>
    public class InventoryAdjustmentsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        List<SqlParameter> parameters = new List<SqlParameter>();

        private const string SELECT_QUERY = @"SELECT * FROM InventoryAdjustments AS ia ";

        /// <summary>
        /// Dictionary for searching Parameters for the inventory Adjustments  object.
        /// </summary>
        private static readonly Dictionary<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string> DBSearchParameters = new Dictionary<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>
            {
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_ID, "ia.AdjustmentId"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_LOCATION_ID, "ia.FromLocationId"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.TO_LOCATION_ID, "ia.ToLocationId"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_TYPE,"ia.AdjustmentType"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.PRODUCT_ID, "ia.ProductId"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.LOT_ID, "ia.LotID"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.DOCUMENT_TYPE_ID, "ia.ocumentTypeID"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_TYPE_ID, "ia.AdjustmentTypeId"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.SITE_ID, "ia.site_id"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_TIMESTAMP, "ia.Timestamp"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.TO_TIMESTAMP, "ia.Timestamp"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.BULK_UPDATED, "ia.BulkUpdated"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.MASTER_ENTITY_ID, "ia.MasterEntityId"},
                {InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.UOM_ID, "ia.UOMId"}
            };

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS InventoryAdjustmentType;
                                            MERGE INTO InventoryAdjustments tbl
                                            USING @InventoryAdjustmentList AS src
                                            ON src.AdjustmentId = tbl.AdjustmentId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                             AdjustmentType=src.AdjustmentType,
                                             AdjustmentQuantity=src.AdjustmentQuantity,
                                             FromLocationId=src.FromLocationId,
                                             ToLocationId=src.ToLocationId,
                                             Remarks=src.Remarks,
                                             ProductId=src.ProductId,
                                             Timestamp=Getdate(),
                                             UserId=src.UserId,
                                             --site_id=@siteId,
                                             SourceSystemID = src.SourceSystemID,
                                             AdjustmentTypeId=src.AdjustmentTypeId,
                                             MasterEntityId=src.MasterEntityId,
                                             LotID = src.LotID,
                                             Price = src.Price,
                                             DocumentTypeID = src.DocumentTypeID,
                                             BulkUpdated = src.BulkUpdated,
                                             OriginalReferenceId = src.OriginalReferenceId,
                                             LastUpdatedBy = src.LastUpdatedBy,
                                             LastUpdateDate = getdate(),
                                             UOMId = src.UOMId
                                            WHEN NOT MATCHED THEN INSERT (
                                                AdjustmentType,
                                                AdjustmentQuantity,
                                                FromLocationId,
                                                ToLocationId,
                                                Remarks,
                                                ProductId,
                                                Timestamp,
                                                UserId,
                                                site_id,
                                                Guid,                                                 
                                                SourceSystemID,
                                                AdjustmentTypeId,
                                                MasterEntityId,
                                                LotID,
                                                Price,
                                                DocumentTypeID,
                                                BulkUpdated,
                                                OriginalReferenceId,
                                                CreatedBy,
                                                CreationDate,
                                                LastUpdatedBy,
                                                LastUpdateDate,
                                                UOMId
                                            )VALUES (
                                                src.AdjustmentType,
                                                src.AdjustmentQuantity,
                                                src.FromLocationId,
                                                src.ToLocationId,
                                                src.Remarks,
                                                src.ProductId,        
                                                Getdate(),
                                                src.UserId,
                                                src.site_id,
                                                src.Guid,                                                      
                                                src.SourceSystemID,
                                                src.AdjustmentTypeId,
                                                src.MasterEntityId,
                                                src.LotID,
                                                src.Price,
                                                src.DocumentTypeID,
                                                src.BulkUpdated,
                                                src.OriginalReferenceId,
                                                src.CreatedBy,
                                                GETDATE(),
                                                src.LastUpdatedBy,
                                                GETDATE(),
                                                src.UOMId
                                            )
                                            OUTPUT
                                            inserted.AdjustmentId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid,
                                            inserted.Timestamp
                                            INTO @Output(
                                            AdjustmentId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid,
                                            Timestamp);
                                            SELECT * FROM @Output;";
        #endregion

        /// <summary>
        /// Parameterized Constructor for InventoryAdjustmentsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public InventoryAdjustmentsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryAdjustmentsDataHandler Record.
        /// </summary>
        /// <param name="inventoryAdjustmentsDTO">InventoryAdjustmentsDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryAdjustmentsDTO inventoryAdjustmentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryAdjustmentsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@adjustmentId", inventoryAdjustmentsDTO.AdjustmentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adjustmentType", string.IsNullOrEmpty(inventoryAdjustmentsDTO.AdjustmentType) ? DBNull.Value : (object)inventoryAdjustmentsDTO.AdjustmentType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adjustmentQuantity", inventoryAdjustmentsDTO.AdjustmentQuantity == 0 ? 0 : (object)inventoryAdjustmentsDTO.AdjustmentQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromLocationId", inventoryAdjustmentsDTO.FromLocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toLocationId", inventoryAdjustmentsDTO.ToLocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(inventoryAdjustmentsDTO.Remarks) ? DBNull.Value : (object)inventoryAdjustmentsDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", inventoryAdjustmentsDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceSystemID", string.IsNullOrEmpty(inventoryAdjustmentsDTO.SourceSystemID) ? DBNull.Value : (object)inventoryAdjustmentsDTO.SourceSystemID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adjustmentTypeId", inventoryAdjustmentsDTO.AdjustmentTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryAdjustmentsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lotID", inventoryAdjustmentsDTO.LotID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@price", inventoryAdjustmentsDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@documentTypeID", inventoryAdjustmentsDTO.DocumentTypeID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalReferenceId", inventoryAdjustmentsDTO.OriginalReferenceId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BulkUpdated", inventoryAdjustmentsDTO.BulkUpdated));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", inventoryAdjustmentsDTO.UOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderReceiveLineId", inventoryAdjustmentsDTO.PurchaseOrderReceiveLineId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the inventory Adjustments record to the database
        /// </summary>
        /// <param name="inventoryAdjustmentsDTO">InventoryAdjustmentsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SqlTransaction</param>
        /// <returns>Returns inserted record id</returns>
        public InventoryAdjustmentsDTO InsertInventoryAdjustments(InventoryAdjustmentsDTO inventoryAdjustmentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryAdjustmentsDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[InventoryAdjustments]
                                                        (
                                                        AdjustmentType,
                                                        AdjustmentQuantity,
                                                        FromLocationId,
                                                        ToLocationId,
                                                        Remarks,
                                                        ProductId,
                                                        Timestamp,
                                                        UserId,
                                                        site_id,
                                                        Guid,                                                 
                                                        SourceSystemID,
                                                        AdjustmentTypeId,
                                                        MasterEntityId,
                                                        LotID,
                                                        Price,
                                                        DocumentTypeID,
                                                        BulkUpdated,
                                                        OriginalReferenceId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        UOMId,
                                                        PurchaseOrderReceiveLineId
                                                        ) 
                                                values 
                                                        ( 
                                                         @adjustmentType,
                                                         @adjustmentQuantity,
                                                         @fromLocationId,
                                                         @toLocationId,
                                                         @remarks,
                                                         @productId,        
                                                         Getdate(),
                                                         @userId,
                                                         @siteId,
                                                         NEWID(),                                                      
                                                         @sourceSystemID,
                                                         @adjustmentTypeId,
                                                         @masterEntityId,
                                                         @lotID,
                                                         @price,
                                                         @documentTypeID,
                                                         @BulkUpdated,
                                                         @OriginalReferenceId,
                                                         @CreatedBy,
                                                         GETDATE(),
                                                         @LastUpdatedBy,
                                                         GETDATE(),
                                                         @UOMId,
                                                         @PurchaseOrderReceiveLineId
                                                        )  SELECT * FROM InventoryAdjustments WHERE AdjustmentId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryAdjustmentsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryAdjustmentsDTO(inventoryAdjustmentsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryAdjustmentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryAdjustmentsDTO);
            return inventoryAdjustmentsDTO;
        }


        /// <summary>
        /// Updates the Inventory receipt record
        /// </summary>
        /// <param name="inventoryAdjustmentsDTO">InventoryAdjustmentsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SqlTransaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryAdjustmentsDTO UpdateInventoryAdjustments(InventoryAdjustmentsDTO inventoryAdjustmentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryAdjustmentsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[InventoryAdjustments]
                                         SET AdjustmentType=@adjustmentType,
                                             AdjustmentQuantity=@adjustmentQuantity,
                                             FromLocationId=@fromLocationId,
                                             ToLocationId=@toLocationId,
                                             Remarks=@remarks,
                                             ProductId=@productId,
                                             Timestamp=Getdate(),
                                             UserId=@userId,
                                             --site_id=@siteId,
                                             SourceSystemID = @sourceSystemID,
                                             AdjustmentTypeId=@adjustmentTypeId,
                                             MasterEntityId=@masterEntityId,
                                             LotID = @lotID,
                                             Price = @price,
                                             DocumentTypeID = @documentTypeID,
                                             BulkUpdated = @BulkUpdated,
                                             OriginalReferenceId = @OriginalReferenceId,
                                             LastUpdatedBy = @LastUpdatedBy,
                                             LastUpdateDate = getdate(),
                                             UOMId = @UOMId,
                                             PurchaseOrderReceiveLineId = @PurchaseOrderReceiveLineId
                                             WHERE AdjustmentId  = @adjustmentId
                                                 SELECT * FROM InventoryAdjustments WHERE AdjustmentId = @adjustmentId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryAdjustmentsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryAdjustmentsDTO(inventoryAdjustmentsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating inventoryAdjustmentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryAdjustmentsDTO);
            return inventoryAdjustmentsDTO;
        }

        /// <summary>
        ///  Deletes the InventoryAdjustments record
        /// </summary>
        /// <param name="inventoryAdjustmentsDTO">InventoryAdjustmentsDTO is passed as parameter</param>
        internal void Delete(InventoryAdjustmentsDTO inventoryAdjustmentsDTO)
        {
            log.LogMethodEntry(inventoryAdjustmentsDTO);
            string query = @"DELETE  
                             FROM InventoryAdjustments
                             WHERE InventoryAdjustments.AdjustmentId = @adjustmentId";
            SqlParameter parameter = new SqlParameter("@adjustmentId", inventoryAdjustmentsDTO.AdjustmentId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            inventoryAdjustmentsDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="IinventoryAdjustmentsDTO">InventoryAdjustmentsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshInventoryAdjustmentsDTO(InventoryAdjustmentsDTO inventoryAdjustmentsDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryAdjustmentsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryAdjustmentsDTO.AdjustmentId = Convert.ToInt32(dt.Rows[0]["AdjustmentId"]);
                inventoryAdjustmentsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                inventoryAdjustmentsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryAdjustmentsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryAdjustmentsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryAdjustmentsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryAdjustmentsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to InventoryAdjustmentsDTO class type
        /// </summary>
        /// <param name="inventoryAdjustmentsDataRow">InventoryAdjustments DataRow</param>
        /// <returns>Returns inventoryAdjustments</returns>
        private InventoryAdjustmentsDTO GetInventoryAdjustmentsDTO(DataRow inventoryAdjustmentsDataRow)
        {
            log.LogMethodEntry(inventoryAdjustmentsDataRow);
            InventoryAdjustmentsDTO inventoryAdjustmentsDataObject = new InventoryAdjustmentsDTO(
                                            inventoryAdjustmentsDataRow["AdjustmentId"] == DBNull.Value ? 0 : Convert.ToDouble(inventoryAdjustmentsDataRow["AdjustmentId"]),
                                            inventoryAdjustmentsDataRow["AdjustmentType"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsDataRow["AdjustmentType"]),
                                            inventoryAdjustmentsDataRow["AdjustmentQuantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryAdjustmentsDataRow["AdjustmentQuantity"]),
                                            inventoryAdjustmentsDataRow["FromLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["FromLocationId"]),
                                            inventoryAdjustmentsDataRow["ToLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["ToLocationId"]),
                                            inventoryAdjustmentsDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsDataRow["Remarks"]),
                                            inventoryAdjustmentsDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["ProductId"]),
                                            inventoryAdjustmentsDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryAdjustmentsDataRow["Timestamp"]),
                                            inventoryAdjustmentsDataRow["UserId"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsDataRow["UserId"]),
                                            inventoryAdjustmentsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["site_id"]),
                                            inventoryAdjustmentsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsDataRow["Guid"]),
                                            inventoryAdjustmentsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryAdjustmentsDataRow["SynchStatus"]),
                                            inventoryAdjustmentsDataRow["SourceSystemID"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsDataRow["SourceSystemID"]),
                                            inventoryAdjustmentsDataRow["AdjustmentTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["AdjustmentTypeId"]),
                                            inventoryAdjustmentsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["MasterEntityId"]),
                                            inventoryAdjustmentsDataRow["LotID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["LotID"]),
                                            inventoryAdjustmentsDataRow["Price"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryAdjustmentsDataRow["Price"].ToString()),
                                            inventoryAdjustmentsDataRow["DocumentTypeID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["DocumentTypeID"]),
                                            inventoryAdjustmentsDataRow["BulkUpdated"] == DBNull.Value ? false : Convert.ToBoolean(inventoryAdjustmentsDataRow["BulkUpdated"]),
                                            inventoryAdjustmentsDataRow["OriginalReferenceId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["OriginalReferenceId"]),
                                            inventoryAdjustmentsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsDataRow["CreatedBy"]),
                                            inventoryAdjustmentsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryAdjustmentsDataRow["CreationDate"]),
                                            inventoryAdjustmentsDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsDataRow["LastUpdatedBy"]),
                                            inventoryAdjustmentsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryAdjustmentsDataRow["LastUpdateDate"]),
                                            inventoryAdjustmentsDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["UOMId"]),
                                            inventoryAdjustmentsDataRow["PurchaseOrderReceiveLineId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsDataRow["PurchaseOrderReceiveLineId"])
                                            );
            log.LogMethodExit(inventoryAdjustmentsDataObject);
            return inventoryAdjustmentsDataObject;
        }

        /// <summary>
        /// Converts the Data row object to InventoryAdjustmentsSummaryDTO class type
        /// </summary>
        /// <param name="inventoryAdjustmentsSummaryDataRow">inventoryAdjustmentsSummaryDataRow</param>
        /// <returns></returns>
        private InventoryAdjustmentsSummaryDTO GetInventoryAdjustmentsSummaryDTO(DataRow inventoryAdjustmentsSummaryDataRow)
        {
            log.LogMethodEntry(inventoryAdjustmentsSummaryDataRow);
            InventoryAdjustmentsSummaryDTO inventoryAdjustmentsSummaryDataObject = new InventoryAdjustmentsSummaryDTO(
                                            inventoryAdjustmentsSummaryDataRow["Code"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsSummaryDataRow["Code"]),
                                            inventoryAdjustmentsSummaryDataRow["Barcode"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsSummaryDataRow["Barcode"]),
                                            inventoryAdjustmentsSummaryDataRow["LotId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsSummaryDataRow["LotId"]),
                                            inventoryAdjustmentsSummaryDataRow["LotNumber"] == DBNull.Value ? string.Empty : inventoryAdjustmentsSummaryDataRow["LotNumber"].ToString(),
                                            inventoryAdjustmentsSummaryDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsSummaryDataRow["Description"]),
                                            inventoryAdjustmentsSummaryDataRow["SKU"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsSummaryDataRow["SKU"]),
                                            inventoryAdjustmentsSummaryDataRow["Location_Name"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryAdjustmentsSummaryDataRow["Location_Name"]),
                                            inventoryAdjustmentsSummaryDataRow["Avl_Qty"] == DBNull.Value ? double.NaN : Convert.ToDouble(inventoryAdjustmentsSummaryDataRow["Avl_Qty"]),
                                            inventoryAdjustmentsSummaryDataRow["Allocated"] == DBNull.Value ? 0 : Convert.ToDouble(inventoryAdjustmentsSummaryDataRow["Allocated"]),
                                            inventoryAdjustmentsSummaryDataRow["Total_Cost"] == DBNull.Value ? 0 : Convert.ToDouble(inventoryAdjustmentsSummaryDataRow["Total_Cost"]),
                                            inventoryAdjustmentsSummaryDataRow["Reorder_Quantity"] == DBNull.Value ? 0 : Convert.ToDouble(inventoryAdjustmentsSummaryDataRow["Reorder_Quantity"]),
                                            inventoryAdjustmentsSummaryDataRow["Reorder_Point"] == DBNull.Value ? 0 : Convert.ToDouble(inventoryAdjustmentsSummaryDataRow["Reorder_Point"]),
                                            inventoryAdjustmentsSummaryDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsSummaryDataRow["ProductId"]),
                                            inventoryAdjustmentsSummaryDataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsSummaryDataRow["LocationId"]),
                                            inventoryAdjustmentsSummaryDataRow["LotControlled"] == DBNull.Value ? false : Convert.ToBoolean(inventoryAdjustmentsSummaryDataRow["LotControlled"]),
                                            inventoryAdjustmentsSummaryDataRow["InvUomId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryAdjustmentsSummaryDataRow["InvUomId"])
                                            );
            log.LogMethodExit(inventoryAdjustmentsSummaryDataObject);
            return inventoryAdjustmentsSummaryDataObject;
        }

        /// <summary>
        /// Converts the Data row object to BarCodeScanSummaryDTO class type
        /// </summary>
        /// <param name="barcodeScanSummaryDataRow">barcodeScanSummaryDataRow</param>
        /// <returns>barcodescanSummaryDataObject</returns>
        private BarcodeScanSummaryDTO GetBarcodeSummaryDTO(DataRow barcodeScanSummaryDataRow)
        {
            log.LogMethodEntry(barcodeScanSummaryDataRow);
            BarcodeScanSummaryDTO barcodescanSummaryDataObject = new BarcodeScanSummaryDTO(
                                            barcodeScanSummaryDataRow["Trx_Quantity"] == DBNull.Value ? 0 : Convert.ToDouble(barcodeScanSummaryDataRow["Trx_Quantity"]),
                                            barcodeScanSummaryDataRow["Code"] == DBNull.Value ? string.Empty : Convert.ToString(barcodeScanSummaryDataRow["Code"]),
                                            barcodeScanSummaryDataRow["LotId"] == DBNull.Value ? -1 : Convert.ToInt32(barcodeScanSummaryDataRow["LotId"]),
                                            barcodeScanSummaryDataRow["LotNumber"] == DBNull.Value ? string.Empty : barcodeScanSummaryDataRow["LotNumber"].ToString(),
                                            barcodeScanSummaryDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(barcodeScanSummaryDataRow["Description"]),
                                            barcodeScanSummaryDataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDouble(barcodeScanSummaryDataRow["Quantity"]),
                                            barcodeScanSummaryDataRow["Location_Name"] == DBNull.Value ? string.Empty : Convert.ToString(barcodeScanSummaryDataRow["Location_Name"]),
                                            barcodeScanSummaryDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(barcodeScanSummaryDataRow["ProductId"]),
                                            barcodeScanSummaryDataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(barcodeScanSummaryDataRow["LocationId"])
                                            );
            log.LogMethodExit(barcodescanSummaryDataObject);
            return barcodescanSummaryDataObject;
        }



        /// <summary>
        /// Gets the InventoryAdjustments data of passed id 
        /// </summary>
        /// <param name="id">id of InventoryAdjustments is passed as parameter</param>
        /// <returns>Returns InventoryAdjustmentsDTO</returns>
        public InventoryAdjustmentsDTO GetInventoryAdjustmentsDTO(double id)
        {
            log.LogMethodEntry(id);
            InventoryAdjustmentsDTO result = null;
            string query = SELECT_QUERY + @" WHERE ia.AdjustmentId = @adjustmentId";
            SqlParameter parameter = new SqlParameter("@adjustmentId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetInventoryAdjustmentsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Gets the Adjustment quantity
        /// </summary>
        /// <param name="locationId">int type parameter</param>
        /// <param name="productId">int type parameter</param>
        /// <param name="poreceiptlineId">int type parameter</param>
        /// <param name="documentypeId">int type parameter</param>
        /// <param name="adjustmentType">string type parameter</param>
        /// <returns>Returns total quantity returned</returns>
        public double GetAdjustmentQuantity(int locationId, int productId, int poreceiptlineId, int documentypeId, string adjustmentType)
        {
            log.LogMethodEntry(locationId, productId, poreceiptlineId, documentypeId, adjustmentType);
            double totalQnty = 0;

            string matchLotsQry = string.Empty;
            SqlParameter[] selectInventoryLotIdParameters = new SqlParameter[1];
            selectInventoryLotIdParameters[0] = new SqlParameter("@poreceiptlineId", poreceiptlineId);
            DataTable dtLots = dataAccessHandler.executeSelectQuery(@"select LotID from InventoryLot where PurchaseOrderReceiveLineId = @poreceiptlineId", selectInventoryLotIdParameters, sqlTransaction);

            if (dtLots != null && dtLots.Rows.Count > 0)
                matchLotsQry = "and LotID in ( select LotID from InventoryLot where PurchaseOrderReceiveLineId = @poreceiptlineId)";
            else
                matchLotsQry = "and LotID is null";

            string selectInventoryAdjustmentsQuery = @"select sum(IA.AdjustmentQuantity) Quantity 
	                                                        from InventoryAdjustments IA 
		                                                        where AdjustmentType=@adjustmentType 
		                                                        and FromLocationId=@locationId 
		                                                        and ProductId = @productid 
		                                                        and DocumentTypeID = @documentTypeId and PurchaseOrderReceiveLineId = @poreceiptlineId "
                                                                + matchLotsQry;

            SqlParameter[] selectInventoryAdjustmentsParameters = new SqlParameter[5];
            selectInventoryAdjustmentsParameters[0] = new SqlParameter("@locationId", locationId);
            selectInventoryAdjustmentsParameters[1] = new SqlParameter("@productid", productId);
            selectInventoryAdjustmentsParameters[2] = new SqlParameter("@documentTypeId", documentypeId);
            selectInventoryAdjustmentsParameters[3] = new SqlParameter("@poreceiptlineId", poreceiptlineId);
            selectInventoryAdjustmentsParameters[4] = new SqlParameter("@adjustmentType", adjustmentType);
            DataTable inventoryAdjustments = dataAccessHandler.executeSelectQuery(selectInventoryAdjustmentsQuery, selectInventoryAdjustmentsParameters, sqlTransaction);
            if (inventoryAdjustments.Rows.Count > 0)
            {
                DataRow InventoryAdjustmentsRow = inventoryAdjustments.Rows[0];
                totalQnty = InventoryAdjustmentsRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDouble(InventoryAdjustmentsRow["Quantity"]);
                log.LogMethodEntry(totalQnty);
                return totalQnty;
            }
            else
            {
                log.LogMethodExit();
                return 0;
            }
        }

        /// <summary>
        /// Gets the InventoryAdjustmentsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryAdjustmentsDTO matching the search criteria</returns>
        public List<InventoryAdjustmentsDTO> GetInventoryAdjustmentsList(List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryAdjustmentsDTO> inventoryAdjustmentsList = null;
            string selectInventoryAdjustmentsQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_ID
                            || searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_LOCATION_ID
                            || searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.TO_LOCATION_ID
                            || searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.PRODUCT_ID
                            || searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.LOT_ID
                            || searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.DOCUMENT_TYPE_ID
                            || searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_TYPE_ID
                            || searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.UOM_ID
                            || searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.TO_TIMESTAMP)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }

                        else if (searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_TIMESTAMP)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_TYPE)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                selectInventoryAdjustmentsQuery = selectInventoryAdjustmentsQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryAdjustmentsQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryAdjustmentsList = new List<InventoryAdjustmentsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryAdjustmentsDTO inventoryAdjustmentsDTO = GetInventoryAdjustmentsDTO(dataRow);
                    inventoryAdjustmentsList.Add(inventoryAdjustmentsDTO);
                }
            }
            log.LogMethodExit(inventoryAdjustmentsList);
            return inventoryAdjustmentsList;
        }

        /// <summary>
        /// Gets the InventoryAdjustmentsSummaryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="advancedSearch">advancedSearch</param>
        /// <param name="pivotColumns">pivotColumns</param>
        /// <returns>inventoryAdjustmentsSummaryList</returns>
        public List<InventoryAdjustmentsSummaryDTO> GetInventoryAdjustmentsSummaryDTO(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters, string advancedSearch, string pivotColumns)
        {
            log.LogMethodEntry(searchParameters, advancedSearch, pivotColumns);
            List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryList = null;
            string selectInventoryAdjustmentsSummaryQuery = GetFilterQuery(searchParameters, advancedSearch, pivotColumns);
            DataTable inventoryAdjustmentsSummaryData = dataAccessHandler.executeSelectQuery(selectInventoryAdjustmentsSummaryQuery, null, sqlTransaction);
            if (inventoryAdjustmentsSummaryData.Rows.Count > 0)
            {
                inventoryAdjustmentsSummaryList = new List<InventoryAdjustmentsSummaryDTO>();
                foreach (DataRow inventoryAdjustmentsDataRow in inventoryAdjustmentsSummaryData.Rows)
                {
                    InventoryAdjustmentsSummaryDTO inventoryAdjustmentsSummaryDTO = GetInventoryAdjustmentsSummaryDTO(inventoryAdjustmentsDataRow);
                    inventoryAdjustmentsSummaryList.Add(inventoryAdjustmentsSummaryDTO);
                }
            }
            log.LogMethodExit(inventoryAdjustmentsSummaryList);
            return inventoryAdjustmentsSummaryList;
        }

        private string GetFilterQuery(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters, string advancedSearch, string pivotColumns)
        {
            log.LogMethodEntry(searchParameters);
            string barcode = string.Empty;
            string productCode = string.Empty;
            int locationId = -1;
            string purchaseable = "Y";
            string description = string.Empty;
            int siteId = -1;
            string productId = string.Empty;
            foreach (KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string> searchParameter in searchParameters)
            {
                if (searchParameter.Key == InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.BARCODE)
                {
                    barcode = searchParameter.Value;
                }
                else if (searchParameter.Key == InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PRODUCT_CODE)
                {
                    productCode = searchParameter.Value;
                }
                else if (searchParameter.Key == InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.LOCATION_ID)
                {
                    if (searchParameter.Value != null && searchParameter.Value != "null")
                    { locationId = Convert.ToInt32(searchParameter.Value); }
                    else
                        locationId = -1;
                }
                else if (searchParameter.Key == InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PURCHASEABLE)
                {
                    purchaseable = searchParameter.Value;
                }
                else if (searchParameter.Key == InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.DESCRIPTION)
                {
                    description = searchParameter.Value;
                }
                else if (searchParameter.Key == InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.SITE_ID)
                {
                    siteId = Convert.ToInt32(searchParameter.Value);
                }
                else if (searchParameter.Key == InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PRODUCT_ID_LIST)
                {
                    productId = "and Product.ProductId IN(" + searchParameter.Value + @")";
                }
            }

            string selectInventoryAdjustmentsSummaryQuery = "";
            if (pivotColumns != null && !string.IsNullOrEmpty(pivotColumns))
            {
                selectInventoryAdjustmentsSummaryQuery = @"select Code, Barcode, LotId, LotNumber, Description, SKU, Location_Name, Avl_Qty, Allocated, Total_Cost, Reorder_Quantity, Reorder_Point, ProductId, LocationId, LotControlled,InvUomId
                                   from (
                                            select Code, Barcode, LotId, LotNumber, Description, SKU, Location_Name, Avl_Qty, Allocated, Total_Cost, Reorder_Quantity, Reorder_Point, ProductId, LotControlled,InvUomId, LocationId " +
                                                 pivotColumns +
                                             @" from (
		                                            select Code, 
			                                            isnull(b.Barcode, '') Barcode, 
			                                            Inventory.LotId, InventoryLot.LotNumber,
			                                            Description, STUFF((SELECT '.'+ valuechar 
					                                            FROM segmentdataview 
					                                            WHERE segmentcategoryid = Product.SegmentCategoryId and ValueChar is not null 
					                                            order by segmentdefinitionid 
					                                            FOR XML PATH('')),1,1,'') SKU, 
			                                            Location.Name Location_Name, Inventory.Quantity Avl_Qty, Inventory.AllocatedQuantity Allocated, 
                                                        CASE WHEN ISNULL(Inventory.LotId,-1) >- 1 THEN
                                                                  Inventory.Quantity * InventoryLot.ReceivePrice
                                                             Else
                                                                  Inventory.Quantity * Product.Cost 
                                                             End Total_Cost, 
                                                        ReorderQuantity Reorder_Quantity, Product.ReorderPoint Reorder_Point, 
			                                            LotControlled, Product.ProductId, Inventory.LocationId, segmentname, valuechar ,Inventory.UOMId InvUomId
		                                            from Product left outer join Inventory on Inventory.ProductId = Product.ProductId 
			                                            left outer join (select * 
							                                             from (
									                                            select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
									                                            from productbarcode 
									                                            where BarCode like N'%" + barcode + @"%' and isactive = 'Y')v 
							                                             where num = 1) b on product.productid = b.productid 
			                                            left outer join Location on Inventory.LocationId = Location.LocationId 
			                                            left outer join InventoryLot on Inventory.LotId = InventoryLot.LotId 
			                                            left outer join SegmentDataView sdv on sdv.SegmentCategoryId = Product.segmentcategoryid
		                                           where Product.IsActive = 'Y' 
			                                            and Code like N'%" + productCode + @"%' and Description like N'%" + description + @"%' 
			                                            and (Location.LocationId = " + locationId + @" or " + locationId + @" = -1) 
			                                            and (isPurchaseable = '" + purchaseable + @"' or '" + purchaseable + @"' = 'N') 
			                                            and (isnull(b.BarCode, '') like N'%" + barcode + @"%' or '%" + barcode + @"%' is null)"
                                                        + productId +
                                                        "and (Product.site_id = " + siteId + @" or " + siteId + @" = -1) )v
                                            PIVOT 
							                   ( max(valuechar) for segmentname in (" + pivotColumns.Substring(2) + ")" + ")  as v1 ) as v2" +
                                         advancedSearch +
                                         " order by 1";

            }
            else
            {
                selectInventoryAdjustmentsSummaryQuery = @"select Code, 
                                 isnull(b.Barcode, '') Barcode, 
                                 Inventory.LotId, InventoryLot.LotNumber,
                                 Description, '' SKU, 
                                 Location.Name Location_Name, Inventory.Quantity Avl_Qty, Inventory.AllocatedQuantity Allocated, 
                                 CASE WHEN ISNULL(Inventory.LotId,-1) >- 1 THEN
                                           Inventory.Quantity * InventoryLot.ReceivePrice
                                      Else
                                           Inventory.Quantity * Product.Cost 
                                      End Total_Cost, 
                                 ReorderQuantity Reorder_Quantity, Product.ReorderPoint Reorder_Point, 
                                 Product.ProductId, Inventory.LocationId, Product.LotControlled ,Inventory.UOMId InvUomId
                                from Product left outer join Inventory on Inventory.ProductId = Product.ProductId 
                                 left outer join (select * 
                                      from (
                                       select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
                                       from productbarcode 
                                       where BarCode like N'%" + barcode + @"%'  and isactive = 'Y')v 
                                      where num = 1) b on product.productid = b.productid 
                                 left outer join Location on Inventory.LocationId = Location.LocationId 
                                 left outer join InventoryLot on Inventory.LotId = InventoryLot.LotId 
                                where Product.IsActive = 'Y' 
                                 and Code like N'%" + productCode + @"%' and Description like N'%" + description + @"%' 
                                 and (Location.LocationId = " + locationId + @" or " + locationId + @" = -1) 
                                  and (isPurchaseable =  '" + purchaseable + @"' or '" + purchaseable + @"' = 'N') 
                                  and (isnull(b.BarCode, '') like N'%" + barcode + @"%'  or '%" + barcode + @"%' is null)"
                                 + productId +
                                    "and (Product.site_id =  " + siteId + @" or " + siteId + @" = -1) 
                                    order by 1";
            }
            return selectInventoryAdjustmentsSummaryQuery;
        }


        /// <summary>
        /// Gets the BarcodeScanSummaryDTO list matching the search key
        /// </summary>
        /// <param name="Barcode">Barcode</param>
        /// <param name="cmbScannedLocation">cmbScannedLocation</param>
        /// <param name="siteId">siteId</param>
        /// <returns>barcodeSummaryList</returns>
        public List<BarcodeScanSummaryDTO> GetBarcodeScanSummaryDTO(string barcode, string cmbScannedLocation, int siteId)
        {
            log.LogMethodEntry(barcode, cmbScannedLocation, siteId);
            string selectBarcodeScanQuery = @"select 1 Trx_qty, Code, i.LotId, il.LotNumber, Description, i.Quantity Current_Stock, l.Name trx_from_location, p.ProductId, p.DefaultLocationId LocationId
                                from Product p left outer join inventory i 
                                on i.productId = p.productId
                                and i.locationId = @locationId
                                left outer join (select *
                                                from (
		                                                select *, row_number() over(partition by productid order by productid) as num
							                                                 from productbarcode 
							                                                 where BarCode = @BarCode and isactive = 'Y')v
                                                where num = 1)b on p.productid = b.productid
                                left outer join location l
                                on l.locationId = p.DefaultLocationId
                                left outer join InventoryLot il
                                on i.LotId = il.LotId
                                where b.BarCode = @BarCode and (p.site_id = @site_id or @site_id = -1)";
            SqlParameter[] barcodeScanSummaryParameters = new SqlParameter[3];
            barcodeScanSummaryParameters[0] = new SqlParameter("@BarCode", barcode);
            barcodeScanSummaryParameters[1] = new SqlParameter("@locationId", cmbScannedLocation);
            barcodeScanSummaryParameters[2] = new SqlParameter("@site_id", siteId);

            DataTable GetBarcodeScanSummaryData = dataAccessHandler.executeSelectQuery(selectBarcodeScanQuery, barcodeScanSummaryParameters, sqlTransaction);

            if (GetBarcodeScanSummaryData.Rows.Count > 0)
            {
                List<BarcodeScanSummaryDTO> barcodeSummaryList = new List<BarcodeScanSummaryDTO>();
                foreach (DataRow barcodeScanSummaryDataRow in GetBarcodeScanSummaryData.Rows)
                {
                    BarcodeScanSummaryDTO barcodeSummaryDataObject = GetBarcodeSummaryDTO(barcodeScanSummaryDataRow);
                    barcodeSummaryList.Add(barcodeSummaryDataObject);
                }
                log.LogMethodExit(barcodeSummaryList);
                return barcodeSummaryList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the InventoryAdjustmentsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryAdjustmentsDTO matching the search criteria</returns>
        public List<InventoryAdjustmentsSummaryDTO> GetInventoryAdjustmentsSummaryList(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters, string advancedSearch, string pivotColumns, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters, advancedSearch, pivotColumns, currentPage, pageSize);
            List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOList = null;
            string selectQuery = GetFilterQuery(searchParameters, advancedSearch, pivotColumns);
            if (currentPage > 0 || pageSize > 0)
            {
                selectQuery += " OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryAdjustmentsSummaryDTOList = new List<InventoryAdjustmentsSummaryDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryAdjustmentsSummaryDTO inventoryAdjustmentsSummaryDTO = GetInventoryAdjustmentsSummaryDTO(dataRow);
                    inventoryAdjustmentsSummaryDTOList.Add(inventoryAdjustmentsSummaryDTO);
                }
            }
            log.LogMethodExit(inventoryAdjustmentsSummaryDTOList);
            return inventoryAdjustmentsSummaryDTOList;
        }

        /// <summary>
        /// Returns the no of Inventory Adjustments matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetInventoryAdjustmentsSummaryCount(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters, string advancedSearch, string pivotColumns)
        {
            log.LogMethodEntry(searchParameters);
            int inventoryWastagesDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = GetFilterQuery(searchParameters, advancedSearch, pivotColumns);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryWastagesDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(inventoryWastagesDTOCount);
            return inventoryWastagesDTOCount;
        }

        /// <summary>
        /// Inserts the inventoryHistory record to the database
        /// </summary>
        /// <param name="inventoryAdjustmentsDTOList">List of inventoryHistoryDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public List<InventoryAdjustmentsDTO> Save(List<InventoryAdjustmentsDTO> inventoryAdjustmentsDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryAdjustmentsDTOList, loginId, siteId);
            Dictionary<string, InventoryAdjustmentsDTO> inventoryAdjustmentDTOGuidMap = GetInventoryAdjustmentDTOGuidMap(inventoryAdjustmentsDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(inventoryAdjustmentsDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "InventoryAdjustmentType",
                                                                "@InventoryAdjustmentList");
            Update(inventoryAdjustmentDTOGuidMap, dataTable);
            List<InventoryAdjustmentsDTO> updatedDTOList = GetUpdatedDTOListFromDictonary(inventoryAdjustmentDTOGuidMap);
            log.LogMethodExit(updatedDTOList);
            return updatedDTOList;
        }
        
        private List<SqlDataRecord> GetSqlDataRecords(List<InventoryAdjustmentsDTO> inventoryAdjusmentDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(inventoryAdjusmentDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[25];
            columnStructures[0] = new SqlMetaData("AdjustmentId", SqlDbType.Decimal, 18, 0);
            columnStructures[1] = new SqlMetaData("AdjustmentType", SqlDbType.NVarChar, 50);
            columnStructures[2] = new SqlMetaData("AdjustmentQuantity", SqlDbType.Decimal, 18, 4);
            columnStructures[3] = new SqlMetaData("FromLocationId", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("ToLocationId", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("Remarks", SqlDbType.NVarChar, SqlMetaData.Max);
            columnStructures[6] = new SqlMetaData("ProductId", SqlDbType.Int);
            columnStructures[7] = new SqlMetaData("Timestamp", SqlDbType.DateTime);
            columnStructures[8] = new SqlMetaData("UserId", SqlDbType.NVarChar, 50);
            columnStructures[9] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[11] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[12] = new SqlMetaData("SourceSystemID", SqlDbType.NVarChar, 100);
            columnStructures[13] = new SqlMetaData("AdjustmentTypeId", SqlDbType.Int);
            columnStructures[14] = new SqlMetaData("LotId", SqlDbType.Int);
            columnStructures[15] = new SqlMetaData("Price", SqlDbType.Decimal, 18, 4);
            columnStructures[16] = new SqlMetaData("DocumentTypeID", SqlDbType.Int);
            columnStructures[17] = new SqlMetaData("BulkUpdated", SqlDbType.Bit);
            columnStructures[18] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[19] = new SqlMetaData("OriginalReferenceId", SqlDbType.Int);
            columnStructures[20] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[21] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[22] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[23] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            columnStructures[24] = new SqlMetaData("UOMId", SqlDbType.Int);


            for (int i = 0; i < inventoryAdjusmentDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue((decimal)inventoryAdjusmentDTOList[i].AdjustmentId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].AdjustmentType));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue((decimal)inventoryAdjusmentDTOList[i].AdjustmentQuantity));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].FromLocationId, true));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].ToLocationId, true));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].Remarks));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].ProductId, true));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].Timestamp));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].UserId));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(Guid.Parse(inventoryAdjusmentDTOList[i].Guid)));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].SynchStatus));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].SourceSystemID));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].AdjustmentTypeId, true));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].LotID, true));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue((decimal)inventoryAdjusmentDTOList[i].Price));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].DocumentTypeID, true));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].BulkUpdated));
                dataRecord.SetValue(18, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(19, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].OriginalReferenceId, true));
                dataRecord.SetValue(20, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(21, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].CreationDate));
                dataRecord.SetValue(22, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(23, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].LastUpdateDate));
                dataRecord.SetValue(24, dataAccessHandler.GetParameterValue(inventoryAdjusmentDTOList[i].UOMId, true));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }
        private Dictionary<string, InventoryAdjustmentsDTO> GetInventoryAdjustmentDTOGuidMap(List<InventoryAdjustmentsDTO> inventoryAdjustmentDTOList)
        {
            log.LogMethodEntry();
            Dictionary<string, InventoryAdjustmentsDTO> result = new Dictionary<string, InventoryAdjustmentsDTO>();
            for (int i = 0; i < inventoryAdjustmentDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(inventoryAdjustmentDTOList[i].Guid))
                {
                    inventoryAdjustmentDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(inventoryAdjustmentDTOList[i].Guid, inventoryAdjustmentDTOList[i]);
            }
            log.LogMethodExit();
            return result;
        }
        private void Update(Dictionary<string, InventoryAdjustmentsDTO> inventoryAdjustmentDTOGuidMap, DataTable table)
        {
            log.LogMethodEntry();
            foreach (DataRow row in table.Rows)
            {
                InventoryAdjustmentsDTO inventoryAdjustmentDTO = inventoryAdjustmentDTOGuidMap[Convert.ToString(row["Guid"])];
                inventoryAdjustmentDTO.AdjustmentId = row["AdjustmentId"] == DBNull.Value ? -1 : Convert.ToInt32(row["AdjustmentId"]);
                inventoryAdjustmentDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                inventoryAdjustmentDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                inventoryAdjustmentDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                inventoryAdjustmentDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                inventoryAdjustmentDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                inventoryAdjustmentDTO.Timestamp = row["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["Timestamp"]);
                inventoryAdjustmentDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        private List<InventoryAdjustmentsDTO> GetUpdatedDTOListFromDictonary(Dictionary<string, InventoryAdjustmentsDTO> inventoryAdjustmentDTOGuidMap)
        {
            log.LogMethodEntry();
            List<InventoryAdjustmentsDTO> inventoryAdjustmentDTOList = new List<InventoryAdjustmentsDTO>();
            Dictionary<string, InventoryAdjustmentsDTO>.ValueCollection valueCollectionList = inventoryAdjustmentDTOGuidMap.Values;
            if (valueCollectionList != null)
            {
                foreach (InventoryAdjustmentsDTO dtoValue in valueCollectionList)
                {
                    inventoryAdjustmentDTOList.Add(dtoValue);
                }
            }
            log.LogMethodExit();
            return inventoryAdjustmentDTOList;
        }
    }
}
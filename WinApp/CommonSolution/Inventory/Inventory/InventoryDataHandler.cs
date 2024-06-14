/********************************************************************************************
* Project Name -Inventory DataHandler
* Description  -Data object of inventory 
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        12-Aug-2016    Amaresh          Created 
*2.70        28-Jun-2019    Archana          Modified: Inventory stock and vendor search in PO
*                                            and receive screen change 
*2.70.2      12-Jul-2019    Deeksha          Modifications as per three tier standard
*2.70.2      27-Nov-2019    Girish Kundar    Modified: Issue fix - Inventory Adjustment 
*2.70.2      09-Dec-2019    Jinto Thomas     Removed site id from update query 
*2.70.2      29-Dec-2019    Girish Kundar    Modified : GetAllInventory() to get category name and UOM for the product
*2.70.2      10-Jan-2020    Deeksha          Modified : Inventory Next Rel Enhancement changes
*2.100.0     27-Jul-2020    Deeksha          Modified : Added UOMId field.
*2.120.1     25-Jun-2021    Deeksha          Modified : Issue Fix: Bounce Physical Count upload/download fix for Lot products
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Microsoft.SqlServer.Server;
using System.Globalization;

namespace Semnox.Parafait.Inventory
{
    public class InventoryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Inventory  ";
        /// <summary>
        /// Dictionary for searching Parameters for the Inventory object.
        /// </summary>
        private static readonly Dictionary<InventoryDTO.SearchByInventoryParameters, string> DBSearchParameters = new Dictionary<InventoryDTO.SearchByInventoryParameters, string>
               {
                    {InventoryDTO.SearchByInventoryParameters.INVENTORY_ID, "InventoryId"},
                    {InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, "ProductId"},
                    {InventoryDTO.SearchByInventoryParameters.PRODUCT_ID_LIST, "ProductId"},
                    {InventoryDTO.SearchByInventoryParameters.LOCATION_ID, "LocationId"},
                    {InventoryDTO.SearchByInventoryParameters.SITE_ID, "site_id"},
                    {InventoryDTO.SearchByInventoryParameters.QUANTITY, "Quantity"},
                    {InventoryDTO.SearchByInventoryParameters.LOT_ID, "LotId"},
                    {InventoryDTO.SearchByInventoryParameters.INVENTORY_ITEMS_ONLY, "IsPurchaseable"},
                    {InventoryDTO.SearchByInventoryParameters.REMARKS_MANDATORY, "RemarksMandatory"},
                    {InventoryDTO.SearchByInventoryParameters.MASS_UPDATE_ALLOWED, "MassUpdateAllowed"},
                    {InventoryDTO.SearchByInventoryParameters.CODE, "Code"},
                    {InventoryDTO.SearchByInventoryParameters.DESCRIPTION, "Description"},
                    {InventoryDTO.SearchByInventoryParameters.BARCODE, "Barcode"},
                    {InventoryDTO.SearchByInventoryParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                    {InventoryDTO.SearchByInventoryParameters.UOM_ID, "UOMId"},
                    {InventoryDTO.SearchByInventoryParameters.POS_MACHINE_ID, ""},
                    {InventoryDTO.SearchByInventoryParameters.IS_REDEEMABLE, "IsRedeemable"} ,
					{InventoryDTO.SearchByInventoryParameters.IS_SELLABLE, "IsSellable"},
					{InventoryDTO.SearchByInventoryParameters.UPDATED_AFTER_DATE, "Timestamp"},
                    {InventoryDTO.SearchByInventoryParameters.GREATER_THAN_ZERO_STOCK, "Quantity"}
               };



        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS InventoryType;
                                            MERGE INTO Inventory tbl
                                            USING @InventoryList AS src
                                            ON src.InventoryId = tbl.InventoryId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            ProductId = src.ProductId,
                                            LocationId = src.LocationId,
                                            Quantity = src.Quantity,
                                            Timestamp = GETDATE(),
                                            Lastupdated_userid = src.Lastupdated_userid,
                                            AllocatedQuantity = src.AllocatedQuantity,
                                            site_id = src.site_id,
                                            Guid = src.Guid,
                                            remarks = src.remarks,
                                            MasterEntityId = src.MasterEntityId,
                                            LotId = src.LotId,
                                            ReceivePrice = src.ReceivePrice,
                                            SourceSystemReference = src.SourceSystemReference,
                                            UOMId = src.UOMId,
                                            LastUpdateDate = GETDATE()
                                            WHEN NOT MATCHED THEN INSERT (
                                            ProductId,
                                            LocationId,
                                            Quantity,
                                            Timestamp,
                                            Lastupdated_userid,
                                            AllocatedQuantity,
                                            site_id,
                                            Guid,
                                            remarks,
                                            MasterEntityId,
                                            LotId,
                                            ReceivePrice,
                                            SourceSystemReference,
                                            UOMId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdateDate
                                            )VALUES (
                                            src.ProductId,
                                            src.LocationId,
                                            src.Quantity,
                                            GETDATE(),
                                            src.Lastupdated_userid,
                                            src.AllocatedQuantity,
                                            src.site_id,
                                            src.Guid,
                                            src.remarks,
                                            src.MasterEntityId,
                                            src.LotId,
                                            src.ReceivePrice,
                                            src.SourceSystemReference,
                                            src.UOMId,
                                            src.CreatedBy,
                                            GETDATE(),
                                            GETDATE()
                                            )
                                            OUTPUT
                                            inserted.InventoryId,
                                            inserted.Timestamp,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.Lastupdated_userid,
                                            inserted.LastUpdateDate,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output
                                            (InventoryId,
                                             Timestamp,
                                             CreatedBy, 
                                             CreationDate,
                                             Lastupdated_userid,
                                             LastUpdateDate,
                                             site_id,
                                             Guid);
                                            SELECT * FROM @Output;";
        #endregion



        /// <summary>
        /// Parameterized constructor of InventoryDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public InventoryDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryDataHandler Record.
        /// </summary>
        /// <param name="inventoryDTO">inventoryDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>parameters</returns>
        private List<SqlParameter> GetSQLParameters(InventoryDTO inventoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", inventoryDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@locationId", inventoryDTO.LocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@quantity", inventoryDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", inventoryDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastupdated_userid", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@allocatedQuantity", inventoryDTO.AllocatedQuantity == 0 ? DBNull.Value : (object)inventoryDTO.AllocatedQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(inventoryDTO.Remarks) ? DBNull.Value : (object)inventoryDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lotId", inventoryDTO.LotId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receivePrice", inventoryDTO.ReceivePrice == 0 ? DBNull.Value : (object)inventoryDTO.ReceivePrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@inventoryId", inventoryDTO.InventoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceSystemReference", string.IsNullOrEmpty(inventoryDTO.SourceSystemReference) ? DBNull.Value : (object)inventoryDTO.SourceSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", inventoryDTO.UOMId,true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to InventoryDTO class type
        /// </summary>
        /// <param name="inventoryDataRow">InventoryDTO DataRow</param>
        /// <returns>Returns InventoryDTO</returns>
        private InventoryDTO GetInventoryDTO(DataRow inventoryDataRow)
        {
            log.LogMethodEntry(inventoryDataRow);
            InventoryDTO inventoryDataObject = new InventoryDTO(
                                             inventoryDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryDataRow["ProductId"]),
                                             inventoryDataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryDataRow["LocationId"]),
                                             inventoryDataRow["Quantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryDataRow["Quantity"]),
                                             inventoryDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryDataRow["Timestamp"]),
                                             inventoryDataRow["Lastupdated_userid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDataRow["Lastupdated_userid"]),
                                             inventoryDataRow["AllocatedQuantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryDataRow["AllocatedQuantity"]),
                                             inventoryDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryDataRow["site_id"]),
                                             inventoryDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDataRow["Guid"]),
                                             inventoryDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryDataRow["SynchStatus"]),
                                             inventoryDataRow["remarks"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDataRow["remarks"]),
                                             inventoryDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryDataRow["MasterEntityId"]),
                                             inventoryDataRow["LotId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryDataRow["LotId"]),
                                             inventoryDataRow["ReceivePrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryDataRow["ReceivePrice"]),
                                             inventoryDataRow["InventoryId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryDataRow["InventoryId"]),
                                             inventoryDataRow["LotNumber"] == DBNull.Value ? string.Empty : inventoryDataRow["LotNumber"].ToString(),
                                             inventoryDataRow["Code"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDataRow["Code"]),
                                             inventoryDataRow["Description"] == DBNull.Value ? string.Empty : inventoryDataRow["Description"].ToString(),
                                             inventoryDataRow["IsPurchaseable"] == DBNull.Value ? "N" : inventoryDataRow["IsPurchaseable"].ToString(),
                                             inventoryDataRow["Lotcontrolled"] == DBNull.Value ? false : Convert.ToBoolean(inventoryDataRow["Lotcontrolled"]),
                                             inventoryDataRow["SKU"] == DBNull.Value ? string.Empty : inventoryDataRow["SKU"].ToString(),
                                             inventoryDataRow["Barcode"] == DBNull.Value ? string.Empty : inventoryDataRow["Barcode"].ToString(),
                                             inventoryDataRow["RemarksMandatory"] == DBNull.Value ? "N" : inventoryDataRow["RemarksMandatory"].ToString(),
                                             inventoryDataRow["MassUpdateAllowed"] == DBNull.Value ? "N" : inventoryDataRow["MassUpdateAllowed"].ToString(),
                                             inventoryDataRow["TotalCost"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryDataRow["TotalCost"]),
                                             inventoryDataRow["StagingQuantity"] == DBNull.Value ? -1 : Convert.ToDouble(inventoryDataRow["StagingQuantity"]),
                                             inventoryDataRow["StagingRemarks"] == DBNull.Value ? string.Empty : inventoryDataRow["StagingRemarks"].ToString(),
                                             inventoryDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDataRow["CreatedBy"]),
                                             inventoryDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryDataRow["CreationDate"]),
                                             inventoryDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryDataRow["LastUpdateDate"]),
                                             inventoryDataRow["LocationName"] == DBNull.Value ? string.Empty : inventoryDataRow["LocationName"].ToString(),
                                             inventoryDataRow["SourceSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDataRow["SourceSystemReference"]),
                                             inventoryDataRow["categoryName"] == DBNull.Value ? string.Empty : inventoryDataRow["categoryName"].ToString(),
                                             inventoryDataRow["uom"] == DBNull.Value ? string.Empty : inventoryDataRow["uom"].ToString(),
                                             inventoryDataRow["InvUOMId"] == DBNull.Value ?-1 : Convert.ToInt32(inventoryDataRow["InvUOMId"])
                                             );
            log.LogMethodExit(inventoryDataObject);
            return inventoryDataObject;
        }
        /// <summary>
        /// Inserts the inventory record to the database
        /// </summary>
        /// <param name="inventoryDTO">InventoryDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SqlTransaction</param>
        /// <returns>Returns inserted record id</returns>
        public InventoryDTO InsertInventory(InventoryDTO inventoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryDTO, loginId, siteId);
            string insertInventoryQuery = @"insert into Inventory 
                                                        (
                                                        ProductId,
                                                        LocationId,
                                                        Quantity,
                                                        Timestamp,
                                                        Lastupdated_userid,
                                                        AllocatedQuantity,
                                                        site_id,
                                                        Guid,
                                                        remarks,
                                                        MasterEntityId,
                                                        LotId,
                                                        ReceivePrice,
                                                        SourceSystemReference,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdateDate,
                                                        UOMId
                                                        ) 
                                                values 
                                                        ( 
                                                         @productId,
                                                         @locationId,
                                                         @quantity,
                                                         Getdate(),
                                                         @lastupdated_userid,
                                                         @allocatedQuantity,        
                                                         @siteId,
                                                         NEWID(),                                                       
                                                         @remarks,
                                                         @masterEntityId,
                                                         @lotId, 
                                                         @receivePrice,
                                                         @sourceSystemReference,
                                                         @createdBy,
                                                         Getdate(),
                                                         Getdate(),
                                                         @UOMId
                                                        ) SELECT * FROM Inventory WHERE InventoryId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertInventoryQuery, GetSQLParameters(inventoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryDTO(inventoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting inventoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryDTO);
            return inventoryDTO;
        }


        /// <summary>
        /// Updates the Inventory record
        /// </summary>
        /// <param name="inventoryDTO">InventoryDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">Sqltransaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryDTO UpdateInventory(InventoryDTO inventoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryDTO, loginId, siteId);
            string updateInventoryQuery = @"update Inventory 
                                             set Quantity=@quantity,
                                             Timestamp=Getdate(),
                                             Lastupdated_userid= @lastupdated_userid,
                                             AllocatedQuantity=@allocatedQuantity,
                                             --site_id=@siteId,
                                             remarks =@remarks,
                                             MasterEntityId=@masterEntityId,
                                             ReceivePrice =@receivePrice,
                                             LastUpdateDate = Getdate(),
                                             UOMId = UOMId
                                             where InventoryId = @inventoryId
                                        select * from Inventory WHERE InventoryId = @inventoryId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateInventoryQuery, GetSQLParameters(inventoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryDTO(inventoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating inventoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryDTO);
            return inventoryDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryDTO">inventoryDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshInventoryDTO(InventoryDTO inventoryDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryDTO.InventoryId = Convert.ToInt32(dt.Rows[0]["InventoryId"]);
                inventoryDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                inventoryDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryDTO.Lastupdated_userid = dataRow["Lastupdated_userid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Lastupdated_userid"]);
                inventoryDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Inventory data of passed Id
        /// </summary>
        /// <param name="productId">Int type parameter</param>
        /// <returns>Returns InventoryDTO</returns>
        public InventoryDTO GetInventory(int productId)
        {
            log.LogMethodEntry(productId);
            string selectInventoryQuery = @"select i.*, 
                                            LotNumber,
                                            c.Name as categoryName,
                                            u.UOM as uom,
	                                        p.code, 
	                                        p.description, 
                                            l.Name as LocationName,
                                            p.IsPurchaseable,
                                            p.Lotcontrolled,
	                                        STUFF((SELECT '.'+ valuechar 
			                                        FROM segmentdataview 
			                                        WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
			                                        order by segmentdefinitionid 
			                                        FOR XML PATH('')),1,1,'') SKU,
	                                        isnull(b.Barcode, '') Barcode,
	                                        l.remarksmandatory,
	                                        l.massupdateallowed,
                                            i.UOMId InvUOMId,
                                            quantity * (case when i.LotId is null then p.Cost else il.ReceivePrice end) TotalCost,
                                            null StagingQuantity,
                                            null StagingRemarks
                                        from inventory i 
                                             left outer join InventoryLot il on il.LotId = i.lotid, 
                                             Location l,
                                             Product p 
	                                         left outer join (select * 
					                                        from (
						                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
						                                        from productbarcode 
						                                        where isactive = 'Y')v 
					                                        where num = 1) b on p.productid = b.productid 
                                              left outer join category c on c.categoryId = p.categoryId
                                              left outer join UOM u on u.UOMId = p.InventoryUOMId
                                        where i.ProductId = p.ProductId
	                                        and i.LocationId = l.LocationId
                                            and p.IsActive = 'Y'
                                            and i.ProductId = @productId ";

            SqlParameter[] selectInventoryParameters = new SqlParameter[1];
            selectInventoryParameters[0] = new SqlParameter("@productId", productId);
            DataTable inventory = dataAccessHandler.executeSelectQuery(selectInventoryQuery, selectInventoryParameters, sqlTransaction);

            if (inventory.Rows.Count > 0)
            {
                DataRow inventoryRow = inventory.Rows[0];
                InventoryDTO inventoryDataObject = GetInventoryDTO(inventoryRow);
                log.LogMethodExit(inventoryDataObject);
                return inventoryDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        public InventoryDTO GetInventory(int productId, int locationId, int lotId = -1)
        {
            log.LogMethodEntry(productId, locationId, lotId);
            string selectInventoryQuery = @"select i.*, 
                                            LotNumber,
                                            c.Name as categoryName,
                                            u.UOM as uom,
	                                        p.code, 
                                            l.Name as LocationName,
	                                        p.description, 
                                            p.IsPurchaseable,
                                            p.Lotcontrolled,
                                            i.UOMId as InvUOMId,
	                                        STUFF((SELECT '.'+ valuechar 
			                                        FROM segmentdataview 
			                                        WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
			                                        order by segmentdefinitionid 
			                                        FOR XML PATH('')),1,1,'') SKU,
	                                        isnull(b.Barcode, '') Barcode,
	                                        l.remarksmandatory,
	                                        l.massupdateallowed,
                                            quantity * (case when i.LotId is null then p.Cost else il.ReceivePrice end) TotalCost,
                                            null StagingQuantity,
                                            null StagingRemarks
                                       from inventory i 
                                            left outer join InventoryLot il on il.LotId = i.lotid, 
                                            Location l,
                                            Product p 
	                                        left outer join (select * 
					                                        from (
						                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
						                                        from productbarcode 
						                                        where isactive = 'Y')v 
					                                        where num = 1) b on p.productid = b.productid 
                                            left outer join category c on c.categoryId = p.categoryId
                                            left outer join UOM u on u.UOMId = p.InventoryUOMId
                                      where i.ProductId = p.ProductId
	                                    and i.LocationId = l.LocationId
                                        and p.IsActive = 'Y'
	                                    and i.ProductId = @productId 
	                                    AND i.LocationId = @locationId
	                                    AND (i.LotId = @lotId or Isnull(i.LotId,-1) = @lotId)
                                       ";

            SqlParameter[] selectInventoryParameters = new SqlParameter[3];

            selectInventoryParameters[0] = new SqlParameter("@productId", productId);
            selectInventoryParameters[1] = new SqlParameter("@locationId", locationId);
            selectInventoryParameters[2] = new SqlParameter("@lotId", lotId);

            DataTable inventory = dataAccessHandler.executeSelectQuery(selectInventoryQuery, selectInventoryParameters, sqlTransaction);

            if (inventory.Rows.Count > 0)
            {
                DataRow inventoryRow = inventory.Rows[0];
                InventoryDTO inventoryDataObject = GetInventoryDTO(inventoryRow);
                log.LogMethodExit(inventoryDataObject);
                return inventoryDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the Inventory data of passed Id
        /// </summary>
        /// <param name="productId">Int type parameter</param>
        /// <returns>Returns InventoryDTO</returns>
        public List<InventoryDTO> GetUntouchedInventory(DateTime StartDate, int LocationID, int SiteID)
        {
            log.LogMethodEntry(StartDate, LocationID, SiteID);
            string selectInventoryQuery = @"select i.*, 
                                            LotNumber,
	                                        p.code, 
                                            c.Name as categoryName,
                                            u.UOM as uom,
                                            i.UOMId as InvUOMId,
	                                        p.description, 
                                            l.Name as LocationName,
                                            p.IsPurchaseable,
                                            p.Lotcontrolled,
	                                        STUFF((SELECT '.'+ valuechar 
			                                        FROM segmentdataview 
			                                        WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
			                                        order by segmentdefinitionid 
			                                        FOR XML PATH('')),1,1,'') SKU,
	                                        isnull(b.Barcode, '') Barcode,
	                                        l.remarksmandatory,
	                                        l.massupdateallowed,
                                            quantity * (case when i.LotId is null then p.Cost else il.ReceivePrice end) TotalCost,
                                            null StagingQuantity,
                                            null StagingRemarks
                                        from inventory i left outer join InventoryLot il on il.LotId = i.lotid, Location l, Product p 
	                                        left outer join (select * 
					                                        from (
						                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
						                                        from productbarcode 
						                                        where isactive = 'Y')v 
					                                        where num = 1) b on p.productid = b.productid 
                                            left outer join category c on c.categoryId = p.categoryId
                                            left outer join UOM u on u.UOMId = p.InventoryUOMId
                                        where i.ProductId = p.ProductId
	                                        and i.LocationId = l.LocationId
                                            and l.MassUpdateAllowed = 'Y'
                                            and p.IsActive = 'Y'
                                            and (i.LocationID = @LocationID or @LocationID = -1)
                                            and (i.site_id = @SiteID or @SiteID = -1)
                                            and not exists (select 1 
                                                            from InventoryAdjustments 
                                                            where FromLocationID = i.locationID
                                                                and ProductID = i.ProductID
                                                                and isnull(LotID, -1) = isnull(i.LotID, -1)
                                                                and BulkUpdated = 1) ";

            SqlParameter[] selectInventoryParameters = new SqlParameter[3];
            selectInventoryParameters[0] = new SqlParameter("@LocationID", LocationID);
            selectInventoryParameters[1] = new SqlParameter("@StartDate", StartDate);
            selectInventoryParameters[2] = new SqlParameter("@SiteID", SiteID);
            DataTable inventoryData = dataAccessHandler.executeSelectQuery(selectInventoryQuery, selectInventoryParameters, sqlTransaction);

            if (inventoryData.Rows.Count > 0)
            {
                List<InventoryDTO> inventoryList = new List<InventoryDTO>();
                foreach (DataRow inventoryDataRow in inventoryData.Rows)
                {
                    InventoryDTO inventoryDataObject = GetInventoryDTO(inventoryDataRow);
                    inventoryList.Add(inventoryDataObject);
                }
                log.LogMethodExit(inventoryList);
                return inventoryList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the InventoryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryDTO matching the search criteria</returns>
        public List<InventoryDTO> GetInventoryList(List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            inventoryDTOList = GetInventoryListDTO(searchParameters);
            log.LogMethodExit(inventoryDTOList);
            return inventoryDTOList;
        }

        /// <summary>
        /// Gets the InventoryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryDTO matching the search criteria</returns>
        public List<InventoryDTO> GetInventoryListDTO(List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParameters, bool multiFieldDescriptionSearch = false)
        {
            log.LogMethodEntry(searchParameters, multiFieldDescriptionSearch);
            List<InventoryDTO> inventoryList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectInventoryQuery = @"select *
                                            from (
                                                    select i.*,
                                                    c.Name as categoryName,
                                                    u.UOM as uom,
                                                    LotNumber,
                                                    p.code,
                                                    p.description,
                                                    l.Name as LocationName,
                                                    p.IsPurchaseable,
													p.IsRedeemable,
													p.IsSellable,
                                                    p.Lotcontrolled,
                                                    i.UOMId as InvUOMId,
                                                    STUFF((SELECT '.'+ valuechar
                                                            FROM segmentdataview
                                                            WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null
                                                            order by segmentdefinitionid
                                                            FOR XML PATH('')),1,1,'') SKU,
	                                                isnull(b.Barcode, '') Barcode,
	                                                l.remarksmandatory,
	                                                l.massupdateallowed,
                                                    quantity * (case when i.LotId is null then p.Cost else il.ReceivePrice end) TotalCost,
                                                    null StagingQuantity,
                                                    null StagingRemarks,
                                                    p.ManualProductId
                                                from inventory i 
                                                    left outer join InventoryLot il on il.LotId = i.lotid, Location l,Product p
                                                    left outer join (select *
                                                                    from (
                                                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num
                                                                        from productbarcode
                                                                        where isactive = 'Y')v
                                                                    where num = 1) b on p.productid = b.productid
                                            left outer join category c on c.categoryId = p.categoryId
                                            left outer join UOM u on u.UOMId = p.InventoryUOMId
                                                where i.ProductId = p.ProductId
                                                    and p.IsActive = 'Y'
                                                    and i.LocationId = l.LocationId
                                                     and l.LocationTypeID NOT IN (select LocationTypeId from LocationType where LocationType = 'Wastage')
                                                      )v";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<InventoryDTO.SearchByInventoryParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {

                        if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.PRODUCT_ID
                            || searchParameter.Key == InventoryDTO.SearchByInventoryParameters.INVENTORY_ID
                            || searchParameter.Key == InventoryDTO.SearchByInventoryParameters.LOT_ID
                            || searchParameter.Key == InventoryDTO.SearchByInventoryParameters.LOCATION_ID
                            || searchParameter.Key == InventoryDTO.SearchByInventoryParameters.UOM_ID
                            || searchParameter.Key == InventoryDTO.SearchByInventoryParameters.MASTER_ENTITY_ID)


                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.REMARKS_MANDATORY
                                || searchParameter.Key == InventoryDTO.SearchByInventoryParameters.MASS_UPDATE_ALLOWED
                                || searchParameter.Key == InventoryDTO.SearchByInventoryParameters.INVENTORY_ITEMS_ONLY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                        }
                        else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.QUANTITY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.IS_REDEEMABLE
                            || searchParameter.Key == InventoryDTO.SearchByInventoryParameters.IS_SELLABLE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.GREATER_THAN_ZERO_STOCK && searchParameter.Value=="Y")
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">= 0 " );
                        }
                        else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.PRODUCT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.CODE ||
                                 searchParameter.Key == InventoryDTO.SearchByInventoryParameters.DESCRIPTION ||
                                 searchParameter.Key == InventoryDTO.SearchByInventoryParameters.BARCODE)
                        {
                            if (multiFieldDescriptionSearch)
                            {
                                query.Append(joiner + "( Isnull(code,'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'" +
                                                     " OR  Isnull(description, '') like " + "N'%' + " + dataAccessHandler.GetParameterName(searchParameter.Key) + " + '%'" + 
                                                     " OR  Isnull(barcode, '') like " + "N'%' + " + dataAccessHandler.GetParameterName(searchParameter.Key) + " + '%' )" );
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                        }
                        else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.POS_MACHINE_ID)
                        {
                            query.Append(joiner + @" NOT EXISTS (SELECT 1 
                                                                from ProductsDisplayGroup pd , 
														             ProductDisplayGroupFormat pdgf,
														             POSProductExclusions ppe ,
                                                                     products ps
												  	           where ps.product_id = pd.ProductId 
                                                                 and ps.product_id = v.ManualProductId   
													             and pd.DisplayGroupId = pdgf.Id 
												  	             and ppe.ProductDisplayGroupFormatId = pdgf.Id
                                                                 and ppe.POSMachineId = " + searchParameter.Value + ")");

                         }
						else if (searchParameter.Key == InventoryDTO.SearchByInventoryParameters.UPDATED_AFTER_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));                                                    
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

                if (searchParameters.Count > 0)
                    selectInventoryQuery = selectInventoryQuery + query;
            }

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryList = new List<InventoryDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryDTO inventoryDTO = GetInventoryDTO(dataRow);
                    inventoryList.Add(inventoryDTO);
                }
            }
            log.LogMethodExit(inventoryList);
            return inventoryList;
        }


        /// <summary>
        /// Gets the Inventory data of passed Id and LocationId and Lottable or not
        /// </summary>
        /// <param name="productId">Int type parameter</param>
        /// <returns>Returns InventoryDTO</returns>
        public InventoryDTO GetInventoryOnLotControlled(int productId, int locationId, string lotCheckQuery)
        {
            log.LogMethodEntry(productId, locationId, lotCheckQuery);

            string selectInventoryQuery = @"select i.*, 
                                                LotNumber,
	                                            p.code, 
	                                            p.description,
                                                c.Name as categoryName,
                                                u.UOM as uom,
                                                i.UOMId as InvUOMId,
                                                l.Name as LocationName,
                                                p.IsPurchaseable,
                                                p.Lotcontrolled,
	                                            STUFF((SELECT '.'+ valuechar 
			                                            FROM segmentdataview 
			                                            WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
			                                            order by segmentdefinitionid 
			                                            FOR XML PATH('')),1,1,'') SKU,
	                                            isnull(b.Barcode, '') Barcode,
	                                            l.remarksmandatory,
	                                            l.massupdateallowed,
                                                quantity * (case when i.LotId is null then p.Cost else il.ReceivePrice end) TotalCost,
                                                null StagingQuantity,
                                                null StagingRemarks
                                            from inventory i left outer join InventoryLot il on il.LotId = i.lotid, Location l, Product p 
	                                            left outer join (select * 
					                                            from (
						                                            select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
						                                            from productbarcode 
						                                            where isactive = 'Y')v 
					                                            where num = 1) b on p.productid = b.productid 
                                            left outer join category c on c.categoryId = p.categoryId
                                            left outer join UOM u on u.UOMId = p.InventoryUOMId                                           
                                            where i.ProductId = p.ProductId
	                                            and i.LocationId = l.LocationId
                                                    and i.ProductId = @productId
                                                    and i.LocationId = @locationId and " + lotCheckQuery;

            SqlParameter[] selectInventoryParameters = new SqlParameter[2];
            selectInventoryParameters[0] = new SqlParameter("@productId", productId);
            selectInventoryParameters[1] = new SqlParameter("@locationId", locationId);
            DataTable inventory = dataAccessHandler.executeSelectQuery(selectInventoryQuery, selectInventoryParameters, sqlTransaction);

            if (inventory.Rows.Count > 0)
            {
                DataRow inventoryRow = inventory.Rows[0];
                InventoryDTO inventoryDataObject = GetInventoryDTO(inventoryRow);
                log.LogMethodExit(inventoryDataObject);
                return inventoryDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the InventoryDTO list matching the search key
        /// </summary>
        /// <param name="filterCondition">Filter condition and For product table columns use p.,UOM table columns u.</param>
        /// <returns>Returns the list of InventoryDTO matching the search criteria</returns>
        public List<InventoryDTO> GetInventoryList(string filterCondition)
        {
            log.LogMethodEntry(filterCondition);
            string filterQuery = filterCondition.ToUpper();
            if (filterQuery.Contains("DROP") || filterQuery.Contains("UPDATE ") || filterQuery.Contains("DELETE"))
            {
                log.LogMethodExit();
                return null;
            }

            string selectInventoryQuery;

            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(ExecutionContext.GetExecutionContext());
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "Y"));
            segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams);

            if (segmentDefinitionDTOList != null)
            {
                string pivotColumns = "";
                foreach (SegmentDefinitionDTO sd in segmentDefinitionDTOList)
                {
                    pivotColumns += ", [" + sd.SegmentName + "]";
                }
                selectInventoryQuery = "select * " +
                                            pivotColumns +
                                     @"from (
                                                select i.*, 
                                                LotNumber,
	                                            p.code, 
                                                c.Name as categoryName,
                                                u.UOM as uom,
	                                            p.description, 
                                                p.IsPurchaseable,
                                                l.Name as LocationName,
                                                p.Lotcontrolled,
	                                            STUFF((SELECT '.'+ valuechar 
			                                            FROM segmentdataview 
			                                            WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
			                                            order by segmentdefinitionid 
			                                            FOR XML PATH('')),1,1,'') SKU,
	                                            isnull(b.Barcode, '') Barcode,
	                                            l.remarksmandatory,
	                                            l.massupdateallowed,
                                                quantity * (case when i.LotId is null then p.Cost else il.ReceivePrice end) TotalCost,
                                                null StagingQuantity,
                                                null StagingRemarks,
                                                segmentname,
                                                    valuechar
                                            from inventory i left outer join InventoryLot il on il.LotId = i.lotid, Location l, Product p 
	                                            left outer join (select * 
					                                            from (
						                                            select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
						                                            from productbarcode 
						                                            where isactive = 'Y')v 
					                                            where num = 1) b on p.productid = b.productid 
												left outer join SegmentDataView sdv on sdv.SegmentCategoryId = p.segmentcategoryid
                                            left outer join category c on c.categoryId = p.categoryId
                                            left outer join UOM u on u.UOMId = p.InventoryUOMId    
                                            where i.ProductId = p.ProductId
                                                and p.IsActive = 'Y'
	                                            and i.LocationId = l.LocationId
			                            )p 
                                    PIVOT 
							        ( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as v2 ";
            }
            else
            {
                selectInventoryQuery = @"select *
                                         from (
                                                 select i.*, 
                                                        LotNumber,
	                                                    p.code, 
	                                                    p.description, 
                                                        c.Name as categoryName,
                                                        u.UOM as uom,
                                                        i.UOMId as InvUOMId,
                                                        p.IsPurchaseable,
                                                        l.Name as LocationName,
                                                        p.Lotcontrolled,
	                                                    STUFF((SELECT '.'+ valuechar 
			                                                    FROM segmentdataview 
			                                                    WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
			                                                    order by segmentdefinitionid 
			                                                    FOR XML PATH('')),1,1,'') SKU,
	                                                    isnull(b.Barcode, '') Barcode,
	                                                    l.remarksmandatory,
	                                                    l.massupdateallowed,
                                                        quantity * (case when i.LotId is null then p.Cost else il.ReceivePrice end) TotalCost,
                                                        null StagingQuantity,
                                                        null StagingRemarks
                                                    from inventory i left outer join InventoryLot il on il.LotId = i.lotid, Location l, Product p 
	                                                    left outer join (select * 
					                                                    from (
						                                                    select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
						                                                    from productbarcode 
						                                                    where isactive = 'Y')v 
					                                                    where num = 1) b on p.productid = b.productid 
                                                    left outer join category c on c.categoryId = p.categoryId
                                                    left outer join UOM u on u.UOMId = p.InventoryUOMId   											      
                                                    where i.ProductId = p.ProductId
                                                        and p.IsActive = 'Y'
	                                                    and i.LocationId = l.LocationId)v ";
            }

            selectInventoryQuery = selectInventoryQuery + ((string.IsNullOrEmpty(filterQuery)) ? " " : " Where " + filterCondition);

            DataTable inventoryData = dataAccessHandler.executeSelectQuery(selectInventoryQuery, null, sqlTransaction);
            if (inventoryData.Rows.Count > 0)
            {
                List<InventoryDTO> inventoryList = new List<InventoryDTO>();
                foreach (DataRow inventoryDataRow in inventoryData.Rows)
                {
                    InventoryDTO inventoryDataObject = GetInventoryDTO(inventoryDataRow);
                    inventoryList.Add(inventoryDataObject);
                }
                log.LogMethodExit(inventoryList);
                return inventoryList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        public DataTable GetInventoryDataForExcel(int locationID, int siteID, int physicalCountId = -1)
        {
            log.LogMethodEntry(locationID, siteID);
            string selectInventoryPhysicalCountQuery = @"select i.ProductId, Code, i.LotId, il.LotNumber, Description, l.LocationId, l.Name Location_Name, u.UOM ProductUOM, i.Quantity Avl_Qty, '' New_Quantity, '' UOM,'' Remarks , ih.ID , ih.PhysicalCountId 
                                                       from  Location l,Product p
                                                                        left outer join Inventory i on i.ProductId = p.ProductId
                                                    left outer join InventoryHist ih on i.ProductId = ih.ProductId and i.LocationId = ih.LocationId and( i.LotId = ih.LotId or ih.LotId is null)
                                                       left outer join InventoryLot il on il.Lotid = i.Lotid
                                                       left outer join uom u on u.UOMId = i.UOMId
                                                       where p.ProductId = i.ProductId
                                                                        and  isnull(ih.LotId, -1) = isnull(il.lotid, -1)
                                                       and i.LocationId = l.LocationId
                                                       and p.IsActive = 'Y'
                                                           and (i.locationid = @locationID or @locationID = -1)
                                                           and (i.site_id = @siteID or @siteID= -1)
                                                           and l.massupdateallowed = 'Y'
                                                        and (ih.PhysicalCountId = @physicalCountID or ih.PhysicalCountId is null)
                                                       order by 1";
            SqlParameter[] selectInventoryPhysicalCountParameters = new SqlParameter[3];
            selectInventoryPhysicalCountParameters[0] = new SqlParameter("@locationID", locationID);
            selectInventoryPhysicalCountParameters[1] = new SqlParameter("@siteID", siteID);
            selectInventoryPhysicalCountParameters[2] = new SqlParameter("@physicalCountId", physicalCountId);
            DataTable inventoryPhysicalCountDt = dataAccessHandler.executeSelectQuery(selectInventoryPhysicalCountQuery, selectInventoryPhysicalCountParameters, sqlTransaction);
            log.LogMethodExit(inventoryPhysicalCountDt);
            return inventoryPhysicalCountDt;
        }

        /// <summary>
        /// GetInventoryList which retorns list of inventory DTO
        /// </summary>
        /// <param name="lastTimeStamp">timestamp of last updated</param>
        /// <param name="maxRowsToFetch">max number of rows can be fetched</param>
        /// <param name="lastInventoryId">last updated inventory id</param>
        /// <returns>list of inventory DTOs</returns>
        public List<InventoryDTO> GetInventoryList(string lastTimeStamp, int maxRowsToFetch, int lastInventoryId)
        {
            log.LogMethodEntry(lastTimeStamp, maxRowsToFetch, lastInventoryId);
            string selectInventoryListQuery = @"select top (@maxRows)
                                                i.ProductId, 
                                                i.LocationId, 
                                                i.Quantity, 
                                                i.Timestamp,
                                                i.Lastupdated_userid,
			                                    i.AllocatedQuantity,
			                                    i.site_id,
			                                    i.Guid,
			                                    i.SynchStatus,
                                                i.remarks,
                                                i.LotId, 
                                                i.ReceivePrice, 
                                                i.MasterEntityId, 
                                                i.InventoryId ,
                                                il.LotNumber,
                                                p.Code,
                                                p.Description,
                                                p.IsPurchaseable,
                                                p.Lotcontrolled,
                                                STUFF((SELECT '.' + valuechar
                                                    FROM segmentdataview
                                                    WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null
                                                    order by segmentdefinitionid
                                                    FOR XML PATH('')),1,1,'') SKU,
	                                            isnull(b.Barcode, '') Barcode,
                                                c.Name as categoryName,
                                                u.UOM as uom, 
                                                i.UOMId as InvUOMId,
                                                l.Name as LocationName,
                                                l.remarksMandatory,
                                                l.MassUpdateAllowed,
                                                quantity * (case when i.LotId is null then p.Cost else il.ReceivePrice end) TotalCost,
                                                NULL StagingQuantity,
                                                NULL StagingRemarks,
                                                i.CreatedBy,
                                                i.CreationDate,
                                                i.LastUpdateDate,
                                                i.SourceSystemReference
                                                
                                            From Inventory i
                                            left outer join InventoryLot il on il.LotId = i.lotid, 
                                                 Location l,
                                                 Product p
                                                    left outer join(select*
                                                            from (
                                                                select*, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num
                                                                from productbarcode
                                                                where isactive = 'Y')v
                                                            where num = 1) b on p.productid = b.productid
                                            left outer join category c on c.categoryId = p.categoryId
                                            left outer join UOM u on u.UOMId = p.UOMId
                                              where i.ProductId = p.ProductId
                                                and i.LocationId = l.LocationId
                                                and p.IsActive = 'Y'
                                                and Timestamp > @timestamp
                                                and InventoryId > @inventoryId";

            List<SqlParameter> selectInventoryListParameters = new List<SqlParameter>();

            selectInventoryListParameters.Add(new SqlParameter("@maxRows", maxRowsToFetch));
            selectInventoryListParameters.Add(new SqlParameter("@timestamp", lastTimeStamp));
            selectInventoryListParameters.Add(new SqlParameter("@inventoryId", lastInventoryId));

            DataTable inventoryData = dataAccessHandler.executeSelectQuery(selectInventoryListQuery, selectInventoryListParameters.ToArray(), sqlTransaction);
            List<InventoryDTO> inventoryList = new List<InventoryDTO>();
            if (inventoryData.Rows.Count > 0)
            {

                foreach (DataRow inventoryDataRow in inventoryData.Rows)
                {
                    InventoryDTO inventoryDataObject = GetInventoryDTO(inventoryDataRow);
                    inventoryList.Add(inventoryDataObject);
                }
                log.LogMethodExit(inventoryList);
                return inventoryList;
            }
            else
            {
                inventoryList = new List<InventoryDTO>();
                log.LogMethodExit(inventoryList);
                return inventoryList;
            }
        }

        /// <summary>
        /// Inserts the Inventory record to the database
        /// </summary>
        /// <param name="inventoryDTOList">List of InventoryDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<InventoryDTO> inventoryDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(inventoryDTOList, userId, siteId);
            Dictionary<string, InventoryDTO> inventoryDTOGuidMap = GetInventoryDTOGuidMap(inventoryDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(inventoryDTOList, userId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "InventoryType",
                                                                "@InventoryList");
            UpdateInventoryDTOList(inventoryDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<InventoryDTO> inventoryDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(inventoryDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[19];
            int column = 0;
            columnStructures[column++] = new SqlMetaData("InventoryId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("ProductId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("LocationId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("Quantity", SqlDbType.Decimal, 18, 4);
            columnStructures[column++] = new SqlMetaData("Timestamp", SqlDbType.DateTime);
            columnStructures[column++] = new SqlMetaData("Lastupdated_userid", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("AllocatedQuantity", SqlDbType.Decimal, 18, 4);
            columnStructures[column++] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[column++] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[column++] = new SqlMetaData("remarks", SqlDbType.NVarChar, -1);
            columnStructures[column++] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("LotId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("ReceivePrice", SqlDbType.Decimal, 18, 4);
            columnStructures[column++] = new SqlMetaData("SourceSystemReference", SqlDbType.NVarChar, 200);
            columnStructures[column++] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[column++] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            columnStructures[column++] = new SqlMetaData("UOMId", SqlDbType.Int);
            for (int i = 0; i < inventoryDTOList.Count; i++)
            {
                column = 0;
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].InventoryId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].ProductId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].LocationId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue((decimal)inventoryDTOList[i].Quantity));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].Timestamp));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue((decimal)inventoryDTOList[i].AllocatedQuantity));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(Guid.Parse(inventoryDTOList[i].Guid)));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].SynchStatus));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].Remarks));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].LotId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue((decimal)inventoryDTOList[i].ReceivePrice));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(null));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].CreationDate));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].LastUpdateDate));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(inventoryDTOList[i].UOMId, true));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, InventoryDTO> GetInventoryDTOGuidMap(List<InventoryDTO> inventoryDTOList)
        {
            Dictionary<string, InventoryDTO> result = new Dictionary<string, InventoryDTO>();
            for (int i = 0; i < inventoryDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(inventoryDTOList[i].Guid))
                {
                    inventoryDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(inventoryDTOList[i].Guid, inventoryDTOList[i]);
            }
            return result;
        }

        private void UpdateInventoryDTOList(Dictionary<string, InventoryDTO> inventoryDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                InventoryDTO inventoryDTO = inventoryDTOGuidMap[Convert.ToString(row["Guid"])];
                inventoryDTO.InventoryId = row["InventoryId"] == DBNull.Value ? -1 : Convert.ToInt32(row["InventoryId"]);
                inventoryDTO.Timestamp = row["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["Timestamp"]);
                inventoryDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                inventoryDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                inventoryDTO.Lastupdated_userid = row["Lastupdated_userid"] == DBNull.Value ? string.Empty : Convert.ToString(row["Lastupdated_userid"]);
                inventoryDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                inventoryDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                inventoryDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Get the Inventory details locations
        /// </summary>
        /// <param name="invProductCode"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetInventoryLocations(string invProductCode)
        {
            Dictionary<string, string> inventoryLocations = new Dictionary<string, string>();
            log.LogMethodEntry(inventoryLocations);
            string selectQuantityQuery = @"select code, description, outL.name outboundLocation, l.name location, quantity
                                        from Product p left outer join Inventory i on p.productId = i.productId left outer join location l 
                                        on l.locationId = i.locationid, location outL 
                                        where code = @code 
                                      and p.outboundLocationId = outL.locationid 
                                      order by l.name  ";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@code", invProductCode);
            DataTable inventoryData = dataAccessHandler.executeSelectQuery(selectQuantityQuery, selectParameters);
            if (inventoryData.Rows.Count > 0)
            {
                foreach (DataRow inventoryDataRow in inventoryData.Rows)
                {
                    if (!inventoryLocations.ContainsKey(inventoryDataRow["location"].ToString()))
                    {
                        inventoryLocations.Add(inventoryDataRow["location"].ToString(), inventoryDataRow["quantity"].ToString());
                    }
                }
                log.LogMethodExit(inventoryLocations);
                return inventoryLocations;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}

/********************************************************************************************
 * Project Name - Product Data Handler
 * Description  - Data handler of the product data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        14-Apr-2016   Raghuveera        Created 
 ********************************************************************************************
 *1.00        11-Aug-2016   Soumya            Updated to add more columns 
 *******************************************************************************************
 *1.00        18-Aug-2016   Suneetha          Modified 
 ********************************************************************************************
 *1.00        22-Aug-2016   Suneetha          Modified 
 *********************************************************************************************
 *2.60        10-Apr-2019   Archana           Include/Exclude for redeemable products changes
 *2.60.2      29-May-2019   Jagan Mohan    Code merge from Development to WebManagementStudio
 *2.70.0      17-Jun-2019   Akshay G       modified isActive DataType
 *2.70.0      27-June-2019  Jagan Mohana   Created the method GetInventoryLocations() 
              27-July-2019  Jagan Mohana   GetInventoryLocations() method moved to Invenotry class.
 *2.90.0      03-Jun-2020   Deeksha        Modified : Bulk product publish & weighted avg costing changes
 *2.100.0     26-Jul-2020   Deeksha        Modified : Added new fields as part of Recipe Mgt enhancement
 *2.110.0     19-Oct-2020   Mushahid Faizan    Modified : Inventory Enhancement
 *2.110.0     03-Dec-2020   Mushahid Faizan    WMS Issue fixes.
 *2.110.0     15-Dec-2020   Deeksha        Modified : Web Inventory Design changes
 *2.120.0     06-May-2021   Mushahid Faizan Modified : LowStock/MostViewed/Wastage Product query changes for Web Inventory.
 *2.120.0     18-May-2021   Mushahid Faizan     Modified : Added search parameter as part of Web Inventory changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Product Data Handler - Handles insert, update and select of product data objects
    /// </summary>
    public class ProductDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ProductDTO.SearchByProductParameters, string> DBSearchParameters = new Dictionary<ProductDTO.SearchByProductParameters, string>
               {
                    {ProductDTO.SearchByProductParameters.PRODUCT_ID, "ProductId"},
                    {ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID, "SegmentCategoryId"},
                    {ProductDTO.SearchByProductParameters.IS_ACTIVE, "p.IsActive"},
                    //Added more search columns 11-Aug-2016
                    {ProductDTO.SearchByProductParameters.CODE,"code"},
                    {ProductDTO.SearchByProductParameters.SITE_ID,"p.site_id"},
                    {ProductDTO.SearchByProductParameters.DESCRIPTION,"Description"},
                    {ProductDTO.SearchByProductParameters.ISREDEEMABLE,"IsRedeemable"},
                    {ProductDTO.SearchByProductParameters.ISSELLABLE,"IsSellable"},
                    {ProductDTO.SearchByProductParameters.CATEGORY,"CategoryId"},
                    {ProductDTO.SearchByProductParameters.CATEGORY_ID_LIST,"CategoryId"},
                    {ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM,"MarketListItem"},
                    {ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID,"p.MasterEntityId"}, //Added search parameter 16-May-2017
                    {ProductDTO.SearchByProductParameters.PRODUCT_NAME,"ProductName"},
                    {ProductDTO.SearchByProductParameters.BARCODE,"Barcode" },
                    {ProductDTO.SearchByProductParameters.UOMID,"UomId" },
                    {ProductDTO.SearchByProductParameters.UOM_ID_LIST,"UomId" },
                    {ProductDTO.SearchByProductParameters.IS_PUBLISHED,"p.MasterEntityId" },
                    {ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH,"code" },
                    {ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH,"ProductName" },
                    {ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH,"Description" },
                    {ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH,"Barcode" },
                    {ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID, "ManualProductId" },
                    {ProductDTO.SearchByProductParameters.VENDOR_ID, "DefaultVendorId" },
                    {ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME, "pdgf.DisplayGroup" },
                    {ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE, "LotControlled" },
                    {ProductDTO.SearchByProductParameters.ITEM_TYPE, "ItemType" },
                    {ProductDTO.SearchByProductParameters.ITEM_TYPE_ID_LIST, "ItemType" },
                    {ProductDTO.SearchByProductParameters.INVENTORY_UOM_ID, "InventoryUOMId" },
                    {ProductDTO.SearchByProductParameters.INCLUDE_IN_PLAN, "IncludeInPlan" },
                    {ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID_LIST, "ManualProductId" },
                    {ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST, "ProductId" },
                    {ProductDTO.SearchByProductParameters.ISPURCHASEABLE, "IsPurchaseable" },
                    {ProductDTO.SearchByProductParameters.CODE_OR_DESCRIPTION, "" }
               };
        DataAccessHandler dataAccessHandler;
        Utilities utilities;
        ExecutionContext executionContext;
        private readonly SqlTransaction sqlTransaction;
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS ProductType;
                                            MERGE INTO Product tbl
                                            USING @ProductList AS src
                                            ON src.ProductId = tbl.ProductId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            Code = src.Code,
                                            Description = src.Description,
                                            Remarks = src.Remarks,
                                            BarCode = src.BarCode,
                                            CategoryId = src.CategoryId,
                                            DefaultLocationId = src.DefaultLocationId,
                                            ReorderPoint = src.ReorderPoint,
                                            ReorderQuantity = src.ReorderQuantity,
                                            UomId = src.UomId,
                                            MasterPackQty = src.MasterPackQty,
                                            InnerPackQty = src.InnerPackQty,
                                            DefaultVendorId = src.DefaultVendorId,
                                            Cost = src.Cost,
                                            LastPurchasePrice = src.LastPurchasePrice,
                                            IsRedeemable = src.IsRedeemable,
                                            IsSellable = src.IsSellable,
                                            IsPurchaseable = src.IsPurchaseable,
                                            LastModUserId = src.LastModUserId,
                                            LastModDttm = GETDATE(),
                                            IsActive = src.IsActive,
                                            PriceInTickets = src.PriceInTickets,
                                            OutboundLocationId = src.OutboundLocationId,
                                            SalePrice = src.SalePrice,
                                            PurchaseTaxId = src.PurchaseTaxId,
                                            TaxInclusiveCost = src.TaxInclusiveCost,
                                            ImageFileName = src.ImageFileName,
                                            LowerLimitCost = src.LowerLimitCost,
                                            UpperLimitCost = src.UpperLimitCost,
                                            CostVariancePercentage = src.CostVariancePercentage,
                                            TurnInPriceInTickets = src.TurnInPriceInTickets,
                                            MasterEntityId = src.MasterEntityId,
                                            CustomDataSetId = src.CustomDataSetId,
                                            SegmentCategoryId = src.SegmentCategoryId,
                                            LotControlled = src.LotControlled,
                                            MarketListItem = src.MarketListItem,
                                            ExpiryType = src.ExpiryType,
                                            IssuingApproach = src.IssuingApproach,
                                            ExpiryDays = src.ExpiryDays,
                                            ItemMarkupPercent = src.ItemMarkupPercent,
                                            AutoUpdateMarkup = src.AutoUpdateMarkup,
                                            ProductName = src.ProductName,
                                            ManualProductId = src.ManualProductId,
                                            CostIncludesTax = src.CostIncludesTax,
                                            ItemType = src.ItemType,
                                            YieldPercentage = src.YieldPercentage,
                                            IncludeInPlan = src.IncludeInPlan,
                                            RecipeDescription = src.RecipeDescription,
                                            InventoryUOMId = src.InventoryUOMId,
                                            PreparationTime = src.PreparationTime
                                            WHEN NOT MATCHED THEN INSERT (
                                            Code,
                                            Description,
                                            Remarks,
                                            BarCode,
                                            CategoryId,
                                            DefaultLocationId,
                                            ReorderPoint,
                                            ReorderQuantity,
                                            UomId,
                                            MasterPackQty,
                                            InnerPackQty,
                                            DefaultVendorId,
                                            Cost,
                                            LastPurchasePrice,
                                            IsRedeemable,
                                            IsSellable,
                                            IsPurchaseable,
                                            LastModUserId,
                                            LastModDttm,
                                            IsActive,
                                            PriceInTickets,
                                            OutboundLocationId,
                                            SalePrice,
                                            PurchaseTaxId,
                                            TaxInclusiveCost,
                                            ImageFileName,
                                            LowerLimitCost,
                                            UpperLimitCost,
                                            CostVariancePercentage,
                                            site_id,
                                            Guid,
                                            TurnInPriceInTickets,
                                            MasterEntityId,
                                            CustomDataSetId,
                                            SegmentCategoryId,
                                            LotControlled,
                                            MarketListItem,
                                            ExpiryType,
                                            IssuingApproach,
                                            ExpiryDays,
                                            ItemMarkupPercent,
                                            AutoUpdateMarkup,
                                            ProductName,
                                            CreatedBy,
                                            CreationDate,
                                            ManualProductId,
                                            CostIncludesTax,
                                            ItemType,
                                            YieldPercentage,
                                            IncludeInPlan,
                                            RecipeDescription,
                                            InventoryUOMId,
                                            PreparationTime
                                            )VALUES (
                                            src.Code,
                                            src.Description,
                                            src.Remarks,
                                            src.BarCode,
                                            src.CategoryId,
                                            src.DefaultLocationId,
                                            src.ReorderPoint,
                                            src.ReorderQuantity,
                                            src.UomId,
                                            src.MasterPackQty,
                                            src.InnerPackQty,
                                            src.DefaultVendorId,
                                            src.Cost,
                                            src.LastPurchasePrice,
                                            src.IsRedeemable,
                                            src.IsSellable,
                                            src.IsPurchaseable,
                                            src.LastModUserId,
                                            GETDATE(),
                                            src.IsActive,
                                            src.PriceInTickets,
                                            src.OutboundLocationId,
                                            src.SalePrice,
                                            src.PurchaseTaxId,
                                            src.TaxInclusiveCost,
                                            src.ImageFileName,
                                            src.LowerLimitCost,
                                            src.UpperLimitCost,
                                            src.CostVariancePercentage,
                                            src.site_id,
                                            src.Guid,
                                            src.TurnInPriceInTickets,
                                            src.MasterEntityId,
                                            src.CustomDataSetId,
                                            src.SegmentCategoryId,
                                            src.LotControlled,
                                            src.MarketListItem,
                                            src.ExpiryType,
                                            src.IssuingApproach,
                                            src.ExpiryDays,
                                            src.ItemMarkupPercent,
                                            src.AutoUpdateMarkup,
                                            src.ProductName,
                                            src.CreatedBy,
                                            GETDATE(),
                                            src.ManualProductId,
                                            src.CostIncludesTax,
                                            src.ItemType,
                                            src.YieldPercentage,
                                            src.IncludeInPlan,
                                            src.RecipeDescription,
                                            src.InventoryUOMId,
                                            src.PreparationTime
                                            )
                                            OUTPUT
                                            inserted.ProductId,
                                            inserted.LastModUserId,  
                                            inserted.LastModDttm,
                                            inserted.CreatedBy,  
                                            inserted.CreationDate,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(ProductId, LastModUserId, LastModDttm, CreatedBy, CreationDate, site_id, Guid);
                                            SELECT * FROM @Output;";
        #endregion
        /// <summary>
        /// Default constructor of ProductDataHandler class
        /// </summary>
        public ProductDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            utilities = new Utilities();
            log.LogMethodExit();
        }
        /// <summary>
        /// Default constructor of ProductDataHandler class
        /// </summary>
        public ProductDataHandler(ExecutionContext executionContext)
            : this()
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the Product record to the database
        /// </summary>
        /// <param name="productDTO">ProductDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(ProductDTO productDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productDTO, userId, siteId);
            Save(new List<ProductDTO>() { productDTO }, userId, siteId);
            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the product record to the database
        /// </summary>
        /// <param name="productDTO">ProductDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="categoryName">categoryName to which the record belongs</param>
        /// <param name="vendorName">vendorName to which the record belongs</param>
        /// <param name="uom">uom to which the record belongs</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertProduct(ProductDTO productDTO, string userId, int siteId, string categoryName, string vendorName, string uom, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(productDTO, userId, siteId, categoryName, vendorName, uom, SQLTrx);
            double verifyDouble = 0;
            string insertProductQuery = @"insert into Product 
                                                        (
                                                          Code,
                                                          Description,
                                                          Remarks,
                                                          CategoryId,
                                                          DefaultLocationId,
                                                          ReorderPoint,
                                                          ReorderQuantity,
                                                          UomId,
                                                          MasterPackQty,
                                                          InnerPackQty,
                                                          DefaultVendorId,
                                                          Cost,
                                                          LastPurchasePrice,
                                                          IsRedeemable,
                                                          IsSellable,
                                                          IsPurchaseable,
                                                          LastModUserId,
                                                          LastModDttm,
                                                          IsActive,
                                                          PriceInTickets,
                                                          OutboundLocationId,
                                                          SalePrice,
                                                          TaxInclusiveCost,
                                                          ImageFileName,
                                                          LowerLimitCost,
                                                          UpperLimitCost,
                                                          CostVariancePercentage,
                                                          site_id,
                                                          Guid,
                                                          TurnInPriceInTickets,
                                                          SegmentCategoryId,
                                                          MasterEntityId,
                                                          CustomDataSetId,
                                                          LotControlled,
                                                          MarketListItem,
                                                          ExpiryType,
                                                          IssuingApproach,
                                                          ExpiryDays,
                                                          ItemMarkupPercent,
                                                          AutoUpdateMarkup,
                                                          ProductName,
                                                          ManualProductId,
							                              PurchaseTaxId,
                                                          CostIncludesTax,
                                                          ItemType,
                                                          YieldPercentage,
                                                          IncludeInPlan,
                                                          RecipeDescription,
                                                          InventoryUOMId,
                                                          PreparationTime
                                                        ) 
                                                select top 1 @code,
                                                          @description,
                                                          @remarks,
                                                          c.CategoryId,
                                                          @defaultLocationId,
                                                          @reorderPoint,
                                                          @reorderQuantity,
                                                          u.uomId,
                                                          @masterPackQty,
                                                          1,
                                                          v.VendorId,
                                                          @cost,
                                                          @lastPurchasePrice,
                                                          @isRedeemable,
                                                          @isSellable,
                                                          @isPurchaseable,
                                                          @lastModUserId,
                                                          getdate(),
                                                          @isActive,
                                                          @priceInTickets,
                                                          @outboundLocationId,
                                                          @salePrice,
                                                          @taxInclusiveCost,
                                                          @imageFileName,
                                                          @lowerLimitCost,
                                                          @upperLimitCost,
                                                          @costVariancePercentage,
                                                          @siteId,
                                                          NewId(),
                                                          @turnInPriceInTickets,
                                                          @segmentCategoryId,
                                                          @MasterEntityId,
                                                          @CustomDataSetId,
                                                          @LotControlled,
                                                          @MarketListItem,
                                                          @ExpiryType,
                                                          @IssuingApproach,
                                                          @expiryDays,
                                                          @ItemMarkupPercent,
                                                          @AutoUpdateMarkup,
                                                          @ProductName,
                                                          @ManualProductId,
						                                  @PurchaseTaxId,
                                                          @CostIncludesTax,
                                                          @ItemType,
                                                          @YieldPercentage,
                                                          @IncludeInPlan,
                                                          @RecipeDescription,
                                                          @InventoryUOMId,
                                                          @PreparationTime
                                                        from category c, vendor v, uom u 
                                       where c.name = @categoryName 
                                       and v.name = @vendorName 
                                       and u.uom = @uom 
                                       and (c.site_id = @site_id or @site_id = -1)
                                       and (v.site_id = @site_id or @site_id = -1)
                                       and (u.site_id = @site_id or @site_id = -1)
                                       SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateProductParameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(categoryName))
            {
                updateProductParameters.Add(new SqlParameter("@categoryName", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@categoryName", categoryName));
            }
            if (string.IsNullOrEmpty(vendorName))
            {
                updateProductParameters.Add(new SqlParameter("@vendorName", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@vendorName", vendorName));
            }
            if (string.IsNullOrEmpty(uom))
            {
                updateProductParameters.Add(new SqlParameter("@uom", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@uom", uom));
            }
            if (string.IsNullOrEmpty(productDTO.Code))
            {
                updateProductParameters.Add(new SqlParameter("@code", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@code", productDTO.Code));
            }
            if (string.IsNullOrEmpty(productDTO.Description))
            {
                updateProductParameters.Add(new SqlParameter("@description", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@description", productDTO.Description));
            }
            if (string.IsNullOrEmpty(productDTO.Remarks))
            {
                updateProductParameters.Add(new SqlParameter("@remarks", ""));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@remarks", productDTO.Remarks));
            }
            if (productDTO.DefaultLocationId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@defaultLocationId", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@defaultLocationId", productDTO.DefaultLocationId));
            }
            if (productDTO.MasterEntityId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@MasterEntityId", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@MasterEntityId", productDTO.MasterEntityId));
            }
            if (productDTO.CustomDataSetId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@CustomDataSetId", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@CustomDataSetId", productDTO.CustomDataSetId));
            }
            updateProductParameters.Add(new SqlParameter("@reorderPoint", productDTO.ReorderPoint));
            updateProductParameters.Add(new SqlParameter("@reorderQuantity", productDTO.ReorderQuantity));
            updateProductParameters.Add(new SqlParameter("@masterPackQty", productDTO.MasterPackQty));
            updateProductParameters.Add(new SqlParameter("@cost", productDTO.Cost));
            if (productDTO.LastPurchasePrice == 0)
            {
                updateProductParameters.Add(new SqlParameter("@lastPurchasePrice", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@lastPurchasePrice", productDTO.LastPurchasePrice));
            }
            updateProductParameters.Add(new SqlParameter("@isRedeemable", string.IsNullOrEmpty(productDTO.IsRedeemable) ? "N" : productDTO.IsRedeemable));
            updateProductParameters.Add(new SqlParameter("@isSellable", string.IsNullOrEmpty(productDTO.IsSellable) ? "N" : productDTO.IsSellable));
            updateProductParameters.Add(new SqlParameter("@isPurchaseable", string.IsNullOrEmpty(productDTO.IsPurchaseable) ? "N" : productDTO.IsPurchaseable));
            updateProductParameters.Add(new SqlParameter("@lastModUserId", userId));
            updateProductParameters.Add(new SqlParameter("@isActive", productDTO.IsActive ? "Y" : "N"));
            updateProductParameters.Add(new SqlParameter("@priceInTickets", productDTO.PriceInTickets));
            if (productDTO.OutboundLocationId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@outboundLocationId", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@outboundLocationId", productDTO.OutboundLocationId));
            }
            updateProductParameters.Add(new SqlParameter("@salePrice", productDTO.SalePrice));
            //if (productDTO.TaxId == -1)
            //{
            //    updateProductParameters.Add(new SqlParameter("@taxId", DBNull.Value));
            //}
            //else
            //{
            //    updateProductParameters.Add(new SqlParameter("@taxId", productDTO.TaxId));
            //}
            updateProductParameters.Add(new SqlParameter("@taxInclusiveCost", string.IsNullOrEmpty(productDTO.TaxInclusiveCost) ? "N" : productDTO.TaxInclusiveCost));
            if (string.IsNullOrEmpty(productDTO.ImageFileName))
            {
                updateProductParameters.Add(new SqlParameter("@imageFileName", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@imageFileName", productDTO.ImageFileName));
            }
            updateProductParameters.Add(new SqlParameter("@lowerLimitCost", productDTO.LowerLimitCost));
            updateProductParameters.Add(new SqlParameter("@upperLimitCost", productDTO.UpperLimitCost));
            updateProductParameters.Add(new SqlParameter("@costVariancePercentage", productDTO.CostVariancePercentage));
            updateProductParameters.Add(new SqlParameter("@site_id", siteId));
            if (siteId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (productDTO.TurnInPriceInTickets == -1)
            {
                updateProductParameters.Add(new SqlParameter("@turnInPriceInTickets", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@turnInPriceInTickets", productDTO.TurnInPriceInTickets));
            }
            if (productDTO.SegmentCategoryId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@segmentCategoryId", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@segmentCategoryId", productDTO.SegmentCategoryId));
            }
            if (productDTO.ExpiryType == "E" || productDTO.ExpiryType == "D")
                updateProductParameters.Add(new SqlParameter("@LotControlled", true));
            else if (productDTO.LotControlled == false)
                updateProductParameters.Add(new SqlParameter("@LotControlled", false));
            else
            {
                updateProductParameters.Add(new SqlParameter("@LotControlled", productDTO.LotControlled));
            }
            if (productDTO.MarketListItem)
            {
                updateProductParameters.Add(new SqlParameter("@MarketListItem", productDTO.MarketListItem));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@MarketListItem", false));
            }
            updateProductParameters.Add(new SqlParameter("@ExpiryType", string.IsNullOrEmpty(productDTO.ExpiryType) ? "N" : productDTO.ExpiryType));
            updateProductParameters.Add(new SqlParameter("@IssuingApproach", string.IsNullOrEmpty(productDTO.IssuingApproach) ? "None" : productDTO.IssuingApproach));
            updateProductParameters.Add(new SqlParameter("@expiryDays", productDTO.ExpiryDays));
            if ((Double.TryParse(productDTO.ItemMarkupPercent.ToString(), out verifyDouble) == false) || Double.IsNaN(productDTO.ItemMarkupPercent) || productDTO.ItemMarkupPercent.ToString() == "")
            {
                updateProductParameters.Add(new SqlParameter("@ItemMarkupPercent", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@ItemMarkupPercent", productDTO.ItemMarkupPercent));
            }
            if (productDTO.AutoUpdateMarkup)
            {
                updateProductParameters.Add(new SqlParameter("@AutoUpdateMarkup", productDTO.AutoUpdateMarkup));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@AutoUpdateMarkup", false));
            }
            if (string.IsNullOrEmpty(productDTO.ProductName))
            {
                updateProductParameters.Add(new SqlParameter("@ProductName", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@ProductName", productDTO.ProductName));
            }
            if (productDTO.ManualProductId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@ManualProductId", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@ManualProductId", productDTO.ManualProductId));
            }
            if (productDTO.PurchaseTaxId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@PurchaseTaxId", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@PurchaseTaxId", productDTO.PurchaseTaxId));
            }
            if (productDTO.CostIncludesTax)
            {
                updateProductParameters.Add(new SqlParameter("@CostIncludesTax", productDTO.CostIncludesTax));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@CostIncludesTax", false));
            }
            if (productDTO.ItemType == -1)
            {
                updateProductParameters.Add(new SqlParameter("@ItemType", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@ItemType", productDTO.ItemType));
            }
            updateProductParameters.Add(new SqlParameter("@YieldPercentage", productDTO.YieldPercentage));
            updateProductParameters.Add(new SqlParameter("@IncludeInPlan", productDTO.IncludeInPlan));
            updateProductParameters.Add(new SqlParameter("@PreparationTime", productDTO.PreparationTime));
            updateProductParameters.Add(new SqlParameter("@RecipeDescription", productDTO.RecipeDescription));
            if (productDTO.InventoryUOMId == -1)
            {
                updateProductParameters.Add(new SqlParameter("@InventoryUOMId", DBNull.Value));
            }
            else
            {
                updateProductParameters.Add(new SqlParameter("@InventoryUOMId", productDTO.InventoryUOMId));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertProductQuery, updateProductParameters.ToArray(), SQLTrx);
            log.Debug("Ends-InsertProduct(productDTO, userId, siteId, categoryName, vendorName, uom, SQLTrx) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReceiptId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public int UpdateProductLastPurchasePrice(int ReceiptId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-UpdateProduct(productDTO, userId, siteId) Method.");
            string updateProductQuery = @"update Product 
                                          set LastPurchasePrice = (select top 1 case when Product.TaxInclusiveCost = 'Y'then (Price + Price * Tax_Percentage / 100.0)
                                                                                else Price end 
                                                                    from PurchaseOrderReceive_Line
                                                                    where productId = Product.ProductId
                                                                    and ReceiptId = @ReceiptId)
                                        where exists (select 1 
                                                        from PurchaseOrderReceive_Line
                                                        where productId = Product.ProductId
                                                        and ReceiptId = @ReceiptId
                                                        and isnull(Price, 0) > 0) ";
            List<SqlParameter> updateProductParameters = new List<SqlParameter>();
            updateProductParameters.Add(new SqlParameter("@ReceiptId", ReceiptId));
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateProductQuery, updateProductParameters.ToArray(), SQLTrx);
            log.Debug("Ends-UpdateProduct(productDTO, userId, siteId) Method.");
            return rowsUpdated;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public int UpdateProductCost(int ProductId, int receiptId, string lastUpdatedBy, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(ProductId, SQLTrx);
            string updateProductQuery = @"declare @latestOnHandQty int = 0;
                                          declare @newWACO float = 0 ;
                                          BEGIN
                                            select @latestOnHandQty =  sum(ISNULL(i.Quantity,0))
                                                from Inventory I,
                                                (select l.LocationId
                                                from LocationType lt, location l
                                                where lt.LocationType = 'Store'
                                                and lt.LocationTypeId = l.LocationTypeID) as loc
                                                where i.ProductId = @productId
                                                and loc.LocationId = i.LocationId
   

                                            select @newWACO = (select cast(
                                                                ((isnull(p.cost,0)* (@latestOnHandQty -sum(r.Quantity) )) + -- onhand before this reeipt
			                                                    (case when p.CostIncludesTax = 1 then sum(iSNULL(r.amount,0)) 
                                                                else sum(ISNULL(r.price,0)* ISNULL(r.Quantity,0)) end )) / NULLIF(@latestOnHandQty, 0) as decimal(15,3)))
                                                                from PurchaseOrderReceive_Line r,
                                                                product p
                                                                where p.ProductId =  @productId
                                                                and r.ProductId = p.ProductId
                                                                and r.ReceiptId = @receiptId
                                                                group by p.productId,p.cost,p.CostIncludesTax

                                            update product  set cost = @newWACO ,LastModDttm =getdate() ,LastModUserId = @lastUpdatedBy  where ProductId = @productId --and 
                                        END";

            List<SqlParameter> updateProductParameters = new List<SqlParameter>();
            updateProductParameters.Add(new SqlParameter("@ProductId", ProductId));
            updateProductParameters.Add(new SqlParameter("@receiptId", receiptId));
            updateProductParameters.Add(new SqlParameter("@lastUpdatedBy", lastUpdatedBy));
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateProductQuery, updateProductParameters.ToArray(), SQLTrx);
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to ProductDTO class type
        /// </summary>
        /// <param name="productDataRow">ProductDTO DataRow</param>
        /// <returns>Returns ProductDTO</returns>
        private ProductDTO GetProductDTO(DataRow productDataRow)
        {
            log.LogMethodEntry(productDataRow);
            ProductDTO productDataObject = new ProductDTO(Convert.ToInt32(productDataRow["ProductId"]),
                                                          productDataRow["Code"].ToString(),
                                                          productDataRow["Description"].ToString(),
                                                          productDataRow["Remarks"].ToString(),
                                                          productDataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["CategoryId"]),
                                                          productDataRow["DefaultLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["DefaultLocationId"]),
                                                          productDataRow["ReorderPoint"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["ReorderPoint"]),
                                                          productDataRow["ReorderQuantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["ReorderQuantity"]),
                                                          productDataRow["UomId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["UomId"]),
                                                          productDataRow["MasterPackQty"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["MasterPackQty"]),
                                                          productDataRow["InnerPackQty"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["InnerPackQty"]),
                                                          productDataRow["DefaultVendorId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["DefaultVendorId"]),
                                                          productDataRow["Cost"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["Cost"]),
                                                          productDataRow["LastPurchasePrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["LastPurchasePrice"]),
                                                          productDataRow["IsRedeemable"] == DBNull.Value ? "N" : productDataRow["IsRedeemable"].ToString(),
                                                          productDataRow["IsSellable"].ToString(),
                                                          productDataRow["IsPurchaseable"].ToString(),
                                                          productDataRow["LastModUserId"].ToString(),
                                                          productDataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productDataRow["LastModDttm"]),
                                                          string.IsNullOrEmpty(productDataRow["IsActive"].ToString()) ? true : productDataRow["IsActive"].ToString() == "Y",
                                                          productDataRow["PriceInTickets"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["PriceInTickets"]),
                                                          productDataRow["OutboundLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["OutboundLocationId"]),
                                                          productDataRow["SalePrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["SalePrice"]),
                                                          //productDataRow["TaxId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["TaxId"]),
                                                          productDataRow["TaxInclusiveCost"] == DBNull.Value ? "N" : productDataRow["TaxInclusiveCost"].ToString(),
                                                          productDataRow["ImageFileName"].ToString(),
                                                          productDataRow["LowerLimitCost"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["LowerLimitCost"]),
                                                          productDataRow["UpperLimitCost"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["UpperLimitCost"]),
                                                          productDataRow["CostVariancePercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(productDataRow["CostVariancePercentage"]),
                                                          productDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["site_id"]),
                                                          productDataRow["Guid"].ToString(),
                                                          productDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["SynchStatus"]),
                                                          productDataRow["TurnInPriceInTickets"] == DBNull.Value ? 0 : Convert.ToInt32(productDataRow["TurnInPriceInTickets"]),
                                                          productDataRow["SegmentCategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["SegmentCategoryId"]),
                                                          productDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["MasterEntityId"]),
                                                          productDataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["CustomDataSetId"]),
                                                          productDataRow["LotControlled"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["LotControlled"]),
                                                          productDataRow["MarketListItem"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["MarketListItem"]),
                                                          productDataRow["ExpiryType"].ToString(),
                                                          productDataRow["IssuingApproach"] == DBNull.Value ? "None" : productDataRow["IssuingApproach"].ToString(),
                                                          productDataRow["UOM"] == DBNull.Value ? string.Empty : Convert.ToString(productDataRow["UOM"]), //Modified for Requisition
                                                          productDataRow["ExpiryDays"] == DBNull.Value ? 0 : Convert.ToInt32(productDataRow["ExpiryDays"]),
                                                          productDataRow["ProdBarCode"].ToString(),
                                                          productDataRow["ItemMarkupPercent"] == DBNull.Value ? double.NaN : Convert.ToDouble(productDataRow["ItemMarkupPercent"]),
                                                          productDataRow["AutoUpdateMarkup"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["AutoUpdateMarkup"]),
                                                          productDataRow["ProductName"] == DBNull.Value ? "" : productDataRow["ProductName"].ToString(),
                                                          productDataRow["CreatedBy"] == DBNull.Value ? "" : productDataRow["CreatedBy"].ToString(),
                                                          productDataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(productDataRow["CreationDate"]),
                                                          productDataRow["ManualProductId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["ManualProductId"]),
                                                          productDataRow["PurchaseTaxId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["PurchaseTaxId"]),
                                                          productDataRow["CostIncludesTax"] == DBNull.Value ? true : Convert.ToBoolean(productDataRow["CostIncludesTax"]),
                                                          productDataRow["ItemType"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["ItemType"]),
                                                          productDataRow["YieldPercentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productDataRow["YieldPercentage"]),
                                                          productDataRow["IncludeInPlan"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(productDataRow["IncludeInPlan"]),
                                                          productDataRow["RecipeDescription"] == DBNull.Value ? null : Convert.ToString(productDataRow["RecipeDescription"]),
                                                          productDataRow["InventoryUOMId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["InventoryUOMId"]),
                                                          productDataRow["PreparationTime"] == DBNull.Value ? (int?)null : Convert.ToInt32(productDataRow["PreparationTime"])
                                                         );
            log.LogMethodExit(productDataObject);
            return productDataObject;
        }

        /// <summary>
        /// Gets the product data of passed patch asset application id
        /// </summary>
        /// <param name="productId">integer type parameter</param>
        /// <param name="SQLTrx">SQLTransaction type parameter</param>
        /// <returns>Returns ProductDTO</returns>
        public ProductDTO GetProduct(int productId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(productId);
            string selectProductQuery = @"select p.*,u.UOM,
                                            (select top 1 BarCode 
                                                from productBarcode pb
                                                where p.ProductId=pb.ProductId and pb.isActive='Y' 
                                                order by LastUpdatedDate desc ) as ProdBarCode 
                                        from Product p left outer join UOM u on p.UomId = u.UOMId 
                                        left join productBarcode pb on pb.ProductId=p.ProductId
                                        where p.ProductId = @productId";
            SqlParameter[] selectProductParameters = new SqlParameter[1];
            selectProductParameters[0] = new SqlParameter("@productId", productId);
            DataTable product = dataAccessHandler.executeSelectQuery(selectProductQuery, selectProductParameters, SQLTrx);
            if (product.Rows.Count > 0)
            {
                DataRow productRow = product.Rows[0];
                ProductDTO productDataObject = GetProductDTO(productRow);
                log.LogMethodExit(productDataObject);
                return productDataObject;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="SQLTrx">Sql trasnsaction</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetProductList(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, string advSearch = null, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(searchParameters, SQLTrx, currentPage, pageSize, advSearch);
            List<ProductDTO> list = new List<ProductDTO>();
            int count = 0;
            string selectProductQuery = string.Empty;

            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();

            if (!string.IsNullOrEmpty(advSearch))
            {
                SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(ExecutionContext.GetExecutionContext());
                List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "1"));
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, (executionContext == null) ? "-1" : executionContext.GetSiteId().ToString()));
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, "PRODUCT"));
                segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams);
                log.Info(segmentDefinitionDTOList);
            }
            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
            {
                string pivotColumns = string.Empty;
                foreach (SegmentDefinitionDTO sd in segmentDefinitionDTOList)
                {
                    pivotColumns += ", [" + sd.SegmentName + "]";
                }
                selectProductQuery = "select * " +
                                            pivotColumns +
                                     @"from (
                                                select pIn.*, 
                                                    u.UOM, pd.product_id ,
                                                    (select top 1 BarCode 
                                                        from productBarcode pb
                                                        where pIn.ProductId=pb.ProductId and pb.isActive='Y' 
                                                        order by LastUpdatedDate desc ) as ProdBarCode,
                                                    segmentname,
                                                    valuechar
                                                from Product pIn left outer join UOM u on pIn.UomId = u.UOMId
                                                left outer join products pd on  pd.product_id = pIn.ManualProductId
                                                    left outer join SegmentDataView sdv on sdv.SegmentCategoryId = pIn.segmentcategoryid
			                            )p1 
                                    PIVOT 
							        ( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as p ";
            }
            //22-Aug-2016 - Query modified
            else
            {
                selectProductQuery = @"select p.*, (select UOM from UOM where uomid = p.UomId) UOM,
                                            (select top 1 BarCode 
                                                from productBarcode pb
                                                where p.ProductId=pb.ProductId and pb.isActive='Y' 
                                                order by LastUpdatedDate desc ) as ProdBarCode 
                                        from Product p ";
            }
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductDTO.SearchByProductParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOMID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.INVENTORY_UOM_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.VENDOR_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            //Added condition to include 16-May-2017
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID))
                            {
                                query.Append("isnull(" + DBSearchParameters[searchParameter.Key] + ", '') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.IS_PUBLISHED))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " IS NOT Null");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.SITE_ID)
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.PRICE_IN_TICKETS)
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "<=" + searchParameter.Value + " ) ");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH
                                     || searchParameter.Key == ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH
                                     || searchParameter.Key == ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH
                                     || searchParameter.Key == ProductDTO.SearchByProductParameters.ISPURCHASEABLE
                                     || searchParameter.Key == ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = N'" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME))
                            {
                                query.Append("p.product_id in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdgf.DisplayGroup like '%" + searchParameter.Value + "%')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE))
                            {
                                query.Append("and (ISNULL((Select ISNULL(LotControlled,'0') from product where product.ManualProductId = p.product_id),'1') = '" + searchParameter.Value + "')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID_LIST))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") " + " ) ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST) ||
                                searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY_ID_LIST) ||
                                searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOM_ID_LIST) ||
                                searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE_ID_LIST))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") " + " ) ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CODE_OR_DESCRIPTION))
                            {
                                query.Append(" and (p.Code like '%" + searchParameter.Value + "%' or p.ProductName like '%" + searchParameter.Value + "%' or p.description like '%" + searchParameter.Value + "%')");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like N" + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.VENDOR_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.INVENTORY_UOM_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOMID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID))
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ", 0)" + " = " + searchParameter.Value);
                            }
                            //Added condition to include 16-May-2017
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(" and isnull(" + DBSearchParameters[searchParameter.Key] + ", '') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.IS_PUBLISHED))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " IS NOT Null");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.SITE_ID)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.PRICE_IN_TICKETS)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "<=" + searchParameter.Value + " ) ");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.INCLUDE_IN_PLAN)
                            {
                                query.Append( " and " + DBSearchParameters[searchParameter.Key] + " =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "True") ? "1" : "0")));
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH)
                                    || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH)
                                    || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH)
                                    || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ISPURCHASEABLE)
                                    || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + " = N'" + searchParameter.Value + "' ) ");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME))
                            {
                                query.Append("p.product_id in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdgf.DisplayGroup like '%" + searchParameter.Value + "%')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE))
                            {
                                query.Append("and (ISNULL((Select ISNULL(LotControlled,'0') from product where product.ManualProductId = p.product_id),'1') = '" + searchParameter.Value + "')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID_LIST))
                            {
                                query.Append("and (" + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") " + " ) ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOM_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE_ID_LIST))
                            {
                                query.Append("and (" + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") " + " ) ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CODE_OR_DESCRIPTION))
                            {
                                query.Append(" and (p.Code like '%" + searchParameter.Value + "%' or p.ProductName like '%" + searchParameter.Value + "%' or p.description like '%" + searchParameter.Value + "%')");
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like N" + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetProductList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception(utilities.MessageUtils.getMessage(1244) + searchParameter.Key);
                    }
                }
                //query.Append(" Order by DeploymentPlannedDate, ProductId ASC");
                if (searchParameters.Count > 0)
                {
                    selectProductQuery = selectProductQuery + query;
                    if (!string.IsNullOrEmpty(advSearch)) // build advance search query
                        selectProductQuery += advSearch;
                    log.Info("Build Advance Search Query" + selectProductQuery);
                    if (currentPage > 0 || pageSize > 0)
                    {
                        selectProductQuery += " ORDER BY p.ProductId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                        selectProductQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
                    }
                }
            }

            DataTable table = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Returns the no of Product matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of product matching the criteria</returns>
        public int GetProductCount(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int productDTOCount = 0;
            int count = 0;
            //22-Aug-2016 - Query modified
            string selectProductQuery = @"select p.*, (select UOM from UOM where uomid = p.UomId) UOM,
                                            (select top 1 BarCode 
                                                from productBarcode pb
                                                where p.ProductId=pb.ProductId and pb.isActive='Y' 
                                                order by LastUpdatedDate desc ) as ProdBarCode 
                                        from Product p ";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductDTO.SearchByProductParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOMID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.VENDOR_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            //Added condition to include 16-May-2017
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID))
                            {
                                query.Append("isnull(" + DBSearchParameters[searchParameter.Key] + ", '') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.IS_PUBLISHED))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " IS NOT Null");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.SITE_ID)
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.PRICE_IN_TICKETS)
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "<=" + searchParameter.Value + " ) ");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH
                                     || searchParameter.Key == ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH
                                     || searchParameter.Key == ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH
                                     || searchParameter.Key == ProductDTO.SearchByProductParameters.ISPURCHASEABLE
                                     || searchParameter.Key == ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = N'" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME))
                            {
                                query.Append("p.product_id in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdgf.DisplayGroup like '%" + searchParameter.Value + "%')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE))
                            {
                                query.Append("and (ISNULL((Select ISNULL(LotControlled,'0') from product where product.ManualProductId = p.product_id),'1') = '" + searchParameter.Value + "')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CODE_OR_DESCRIPTION))
                            {
                                query.Append(" and (p.Code like '%" + searchParameter.Value + "%' or p.ProductName like '%" + searchParameter.Value + "%' or p.description like '%" + searchParameter.Value + "%')");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like N" + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.VENDOR_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOMID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID))
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ", 0)" + " = " + searchParameter.Value);
                            }
                            //Added condition to include 16-May-2017
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(" and isnull(" + DBSearchParameters[searchParameter.Key] + ", '') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.IS_PUBLISHED))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " IS NOT Null");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.SITE_ID)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.PRICE_IN_TICKETS)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "<=" + searchParameter.Value + " ) ");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH)
                                    || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH)
                                    || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH)
                                    || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ISPURCHASEABLE)
                                    || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + " = N'" + searchParameter.Value + "' ) ");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME))
                            {
                                query.Append("p.product_id in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdgf.DisplayGroup like '%" + searchParameter.Value + "%')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE))
                            {
                                query.Append("and (ISNULL((Select ISNULL(LotControlled,'0') from product where product.ManualProductId = p.product_id),'1') = '" + searchParameter.Value + "')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CODE_OR_DESCRIPTION))
                            {
                                query.Append(" and (p.Code like '%" + searchParameter.Value + "%' or p.ProductName like '%" + searchParameter.Value + "%' or p.description like '%" + searchParameter.Value + "%')");
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like N" + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetProductList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception(utilities.MessageUtils.getMessage(1244) + searchParameter.Key);
                    }
                }
                //query.Append(" Order by DeploymentPlannedDate, ProductId ASC");
                if (searchParameters.Count > 0)
                    selectProductQuery = selectProductQuery + query;
            }
            DataTable table = dataAccessHandler.executeSelectQuery(selectProductQuery, null, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                productDTOCount = Convert.ToInt32(table.Rows.Count);
            }
            log.LogMethodExit(productDTOCount);
            return productDTOCount;
        }


        /// <summary>
        /// Inserts the Product record to the database
        /// </summary>
        /// <param name="productDTOList">List of ProductDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<ProductDTO> productDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productDTOList, userId, siteId);
            Dictionary<string, ProductDTO> productDTOGuidMap = GetProductDTOGuidMap(productDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(productDTOList, userId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                        sqlTransaction,
                                        MERGE_QUERY,
                                        "ProductType",
                                        "@ProductList");
            UpdateProductDTOList(productDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private void UpdateProductDTOList(Dictionary<string, ProductDTO> productDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                ProductDTO productDTO = productDTOGuidMap[Convert.ToString(row["Guid"])];
                productDTO.ProductId = row["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(row["ProductId"]);
                productDTO.LastModUserId = row["LastModUserId"] == DBNull.Value ? null : Convert.ToString(row["LastModUserId"]);
                productDTO.LastModDttm = row["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastModDttm"]);
                productDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                productDTO.CreationDate = row["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["CreationDate"]);
                productDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                productDTO.AcceptChanges();
            }
        }

        private Dictionary<string, ProductDTO> GetProductDTOGuidMap(List<ProductDTO> productDTOList)
        {
            Dictionary<string, ProductDTO> result = new Dictionary<string, ProductDTO>();
            for (int i = 0; i < productDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(productDTOList[i].Guid))
                {
                    productDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(productDTOList[i].Guid, productDTOList[i]);
            }
            return result;
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<ProductDTO> productDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[55];
            columnStructures[0] = new SqlMetaData("ProductId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("Code", SqlDbType.NVarChar, 50);
            columnStructures[2] = new SqlMetaData("Description", SqlDbType.NVarChar, 200);
            columnStructures[3] = new SqlMetaData("Remarks", SqlDbType.NVarChar, -1);
            columnStructures[4] = new SqlMetaData("BarCode", SqlDbType.NVarChar, 50);
            columnStructures[5] = new SqlMetaData("CategoryId", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("DefaultLocationId", SqlDbType.Int);
            columnStructures[7] = new SqlMetaData("ReorderPoint", SqlDbType.Decimal, 18, 4);
            columnStructures[8] = new SqlMetaData("ReorderQuantity", SqlDbType.Decimal, 18, 4);
            columnStructures[9] = new SqlMetaData("UomId", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("MasterPackQty", SqlDbType.Decimal, 18, 4);
            columnStructures[11] = new SqlMetaData("InnerPackQty", SqlDbType.Decimal, 18, 4);
            columnStructures[12] = new SqlMetaData("DefaultVendorId", SqlDbType.Int);
            columnStructures[13] = new SqlMetaData("Cost", SqlDbType.Decimal, 18, 4);
            columnStructures[14] = new SqlMetaData("LastPurchasePrice", SqlDbType.Decimal, 18, 4);
            columnStructures[15] = new SqlMetaData("IsRedeemable", SqlDbType.Char, 1);
            columnStructures[16] = new SqlMetaData("IsSellable", SqlDbType.Char, 1);
            columnStructures[17] = new SqlMetaData("IsPurchaseable", SqlDbType.Char, 1);
            columnStructures[18] = new SqlMetaData("LastModUserId", SqlDbType.NVarChar, 50);
            columnStructures[19] = new SqlMetaData("LastModDttm", SqlDbType.DateTime);
            columnStructures[20] = new SqlMetaData("IsActive", SqlDbType.Char, 1);
            columnStructures[21] = new SqlMetaData("PriceInTickets", SqlDbType.Decimal, 18, 5);
            columnStructures[22] = new SqlMetaData("OutboundLocationId", SqlDbType.Int);
            columnStructures[23] = new SqlMetaData("SalePrice", SqlDbType.Decimal, 18, 2);
            columnStructures[24] = new SqlMetaData("TaxInclusiveCost", SqlDbType.Char, 1);
            columnStructures[25] = new SqlMetaData("ImageFileName", SqlDbType.NVarChar, 100);
            columnStructures[26] = new SqlMetaData("LowerLimitCost", SqlDbType.Decimal, 18, 4);
            columnStructures[27] = new SqlMetaData("UpperLimitCost", SqlDbType.Decimal, 18, 4);
            columnStructures[28] = new SqlMetaData("CostVariancePercentage", SqlDbType.Decimal, 6, 2);
            columnStructures[29] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[30] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[31] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[32] = new SqlMetaData("TurnInPriceInTickets", SqlDbType.Int);
            columnStructures[33] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[34] = new SqlMetaData("CustomDataSetId", SqlDbType.Int);
            columnStructures[35] = new SqlMetaData("SegmentCategoryId", SqlDbType.Int);
            columnStructures[36] = new SqlMetaData("LotControlled", SqlDbType.Bit);
            columnStructures[37] = new SqlMetaData("MarketListItem", SqlDbType.Bit);
            columnStructures[38] = new SqlMetaData("ExpiryType", SqlDbType.Char, 1);
            columnStructures[39] = new SqlMetaData("IssuingApproach", SqlDbType.NVarChar, 10);
            columnStructures[40] = new SqlMetaData("ExpiryDays", SqlDbType.Int);
            columnStructures[41] = new SqlMetaData("ItemMarkupPercent", SqlDbType.Float);
            columnStructures[42] = new SqlMetaData("AutoUpdateMarkup", SqlDbType.Bit);
            columnStructures[43] = new SqlMetaData("ProductName", SqlDbType.NVarChar, 100);
            columnStructures[44] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[45] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[46] = new SqlMetaData("ManualProductId", SqlDbType.Int);
            columnStructures[47] = new SqlMetaData("PurchaseTaxId", SqlDbType.Int);
            columnStructures[48] = new SqlMetaData("CostIncludesTax", SqlDbType.Bit);
            columnStructures[49] = new SqlMetaData("ItemType", SqlDbType.Int);
            columnStructures[50] = new SqlMetaData("YieldPercentage", SqlDbType.Decimal, 18, 5);
            columnStructures[51] = new SqlMetaData("IncludeInPlan", SqlDbType.Bit);
            columnStructures[52] = new SqlMetaData("RecipeDescription", SqlDbType.NVarChar, 4000);
            columnStructures[53] = new SqlMetaData("InventoryUOMId", SqlDbType.Int);
            columnStructures[54] = new SqlMetaData("PreparationTime", SqlDbType.Int);
            for (int i = 0; i < productDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(productDTOList[i].ProductId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(productDTOList[i].Code));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(productDTOList[i].Description));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(productDTOList[i].Remarks));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(productDTOList[i].BarCode));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(productDTOList[i].CategoryId, true));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(productDTOList[i].DefaultLocationId, true));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].ReorderPoint));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].ReorderQuantity));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(productDTOList[i].UomId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].MasterPackQty));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].InnerPackQty));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(productDTOList[i].DefaultVendorId, true));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].Cost));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue((((decimal?)productDTOList[i].LastPurchasePrice) == 0 ? null : ((decimal?)productDTOList[i].LastPurchasePrice))));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(productDTOList[i].IsRedeemable));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(productDTOList[i].IsSellable));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(productDTOList[i].IsPurchaseable));
                dataRecord.SetValue(18, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(19, dataAccessHandler.GetParameterValue(productDTOList[i].LastModDttm));
                dataRecord.SetValue(20, dataAccessHandler.GetParameterValue((productDTOList[i].IsActive == true) ? 'Y' : 'N'));
                dataRecord.SetValue(21, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].PriceInTickets));
                dataRecord.SetValue(22, dataAccessHandler.GetParameterValue(productDTOList[i].OutboundLocationId, true));
                dataRecord.SetValue(23, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].SalePrice));
                dataRecord.SetValue(24, dataAccessHandler.GetParameterValue(string.IsNullOrEmpty(productDTOList[i].TaxInclusiveCost) ? "N" : productDTOList[i].TaxInclusiveCost));//productDTOList[i].TaxInclusiveCost));
                dataRecord.SetValue(25, dataAccessHandler.GetParameterValue(productDTOList[i].ImageFileName));
                dataRecord.SetValue(26, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].LowerLimitCost));
                dataRecord.SetValue(27, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].UpperLimitCost));
                dataRecord.SetValue(28, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].CostVariancePercentage));
                dataRecord.SetValue(29, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(30, dataAccessHandler.GetParameterValue(Guid.Parse(productDTOList[i].Guid)));
                dataRecord.SetValue(31, dataAccessHandler.GetParameterValue(productDTOList[i].SynchStatus));
                dataRecord.SetValue(32, dataAccessHandler.GetParameterValue(productDTOList[i].TurnInPriceInTickets));
                dataRecord.SetValue(33, dataAccessHandler.GetParameterValue(productDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(34, dataAccessHandler.GetParameterValue(productDTOList[i].CustomDataSetId, true));
                dataRecord.SetValue(35, dataAccessHandler.GetParameterValue(productDTOList[i].SegmentCategoryId, true));
                dataRecord.SetValue(36, dataAccessHandler.GetParameterValue((productDTOList[i].ExpiryType == "E" || productDTOList[i].ExpiryType == "D" ? true : productDTOList[i].LotControlled)));
                dataRecord.SetValue(37, dataAccessHandler.GetParameterValue(productDTOList[i].MarketListItem));
                dataRecord.SetValue(38, dataAccessHandler.GetParameterValue(productDTOList[i].ExpiryType));
                dataRecord.SetValue(39, dataAccessHandler.GetParameterValue(productDTOList[i].IssuingApproach));
                dataRecord.SetValue(40, dataAccessHandler.GetParameterValue(productDTOList[i].ExpiryDays));
                dataRecord.SetValue(41, dataAccessHandler.GetParameterValue(productDTOList[i].ItemMarkupPercent));
                dataRecord.SetValue(42, dataAccessHandler.GetParameterValue(productDTOList[i].AutoUpdateMarkup));
                dataRecord.SetValue(43, dataAccessHandler.GetParameterValue(productDTOList[i].ProductName));
                dataRecord.SetValue(44, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(45, dataAccessHandler.GetParameterValue(productDTOList[i].CreationDate));
                dataRecord.SetValue(46, dataAccessHandler.GetParameterValue(productDTOList[i].ManualProductId, true));
                dataRecord.SetValue(47, dataAccessHandler.GetParameterValue(productDTOList[i].PurchaseTaxId, true));
                dataRecord.SetValue(48, dataAccessHandler.GetParameterValue(productDTOList[i].CostIncludesTax));
                dataRecord.SetValue(49, dataAccessHandler.GetParameterValue(productDTOList[i].ItemType, true));
                dataRecord.SetValue(50, dataAccessHandler.GetParameterValue((decimal?)productDTOList[i].YieldPercentage));
                dataRecord.SetValue(51, dataAccessHandler.GetParameterValue(productDTOList[i].IncludeInPlan));
                dataRecord.SetValue(52, dataAccessHandler.GetParameterValue(productDTOList[i].RecipeDescription));
                dataRecord.SetValue(53, dataAccessHandler.GetParameterValue(productDTOList[i].InventoryUOMId, true));
                dataRecord.SetValue(54, dataAccessHandler.GetParameterValue((int?)productDTOList[i].PreparationTime));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="filterCondition">Filter condition and For product table columns use p.,UOM table columns u.</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetProductList(string filterCondition, List<SqlParameter> parameters)
        {
            log.Debug("Starts-GetProductList(searchParameters) Method.");
            string filterQuery = filterCondition.ToUpper();
            if (filterQuery.Contains("DROP") || filterQuery.Contains("UPDATE") || filterQuery.Contains("DELETE"))
            {
                log.Debug("Ends-GetCategory(sqlQuery) Method by invalid query.");
                return null;
            }

            string selectProductQuery;

            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(ExecutionContext.GetExecutionContext());
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "1"));
            segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams);

            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
            {
                string pivotColumns = "";
                foreach (SegmentDefinitionDTO sd in segmentDefinitionDTOList)
                {
                    pivotColumns += ", [" + sd.SegmentName + "]";
                }
                selectProductQuery = "select * " +
                                            pivotColumns +
                                     @"from (
                                                select pIn.*, 
                                                    u.UOM,
                                                    (select top 1 BarCode 
                                                        from productBarcode pb
                                                        where pIn.ProductId=pb.ProductId and pb.isActive='Y' 
                                                        order by LastUpdatedDate desc ) as ProdBarCode,
                                                    segmentname,
                                                    valuechar
                                                from Product pIn left outer join UOM u on pIn.UomId = u.UOMId
                                                    left outer join SegmentDataView sdv on sdv.SegmentCategoryId = pIn.segmentcategoryid
			                            )p1 
                                    PIVOT 
							        ( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as p ";
            }
            else
            {
                selectProductQuery = @"select *
                                       from (
                                            select pIn.*, u.UOM,
                                                (select top 1 BarCode 
                                                    from productBarcode pb
                                                    where pIn.ProductId=pb.ProductId and pb.isActive='Y' 
                                                    order by LastUpdatedDate desc ) as ProdBarCode 
                                            from Product pIn left outer join UOM u on pIn.UomId = u.UOMId) p";
            }

            selectProductQuery = selectProductQuery + ((string.IsNullOrEmpty(filterQuery)) ? " " : " Where " + filterCondition);


            DataTable productData = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(), sqlTransaction);
            if (productData.Rows.Count > 0)
            {
                List<ProductDTO> productList = new List<ProductDTO>();
                foreach (DataRow productDataRow in productData.Rows)
                {
                    ProductDTO productDataObject = GetProductDTO(productDataRow);
                    productList.Add(productDataObject);
                }
                log.Debug("Ends-GetProductList(searchParameters) Method by returning productList.");
                return productList;
            }
            else
            {
                log.Debug("Ends-GetProductList(searchParameters) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="filterCondition">Filter condition and For product table columns use p.,UOM table columns u.</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetProductListOnBarcode(string filterCondition, List<SqlParameter> parameters)
        {
            log.LogMethodEntry(filterCondition, parameters);
            string filterQuery = filterCondition.ToUpper();

            string selectProductQuery = @"Select * from (select p.*, u.UOM,
                                          (select top 1 BarCode 
                                              from productBarcode pb
                                              where p.ProductId=pb.ProductId and pb.isActive='Y' 
                                              order by LastUpdatedDate desc ) as ProdBarCode 
                                        from Product p 
                                        left outer join UOM u on p.UomId = u.UOMId )T ";

            selectProductQuery = selectProductQuery + ((string.IsNullOrEmpty(filterQuery)) ? " " : " Where " + filterCondition);


            DataTable productData = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(), sqlTransaction);
            if (productData.Rows.Count > 0)
            {
                List<ProductDTO> productList = new List<ProductDTO>();
                foreach (DataRow productDataRow in productData.Rows)
                {
                    ProductDTO productDataObject = GetProductDTO(productDataRow);
                    productList.Add(productDataObject);
                }
                log.LogMethodExit(productList);
                return productList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetAdvancedProductList(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAdvancedProductList(searchParameters) Method.");
            int count = 0;
            //22-Aug-2016 - Query modified
            string selectProductQuery = @"select p.*, (select UOM from UOM where uomid = p.UomId) UOM,
                                            (select top 1 BarCode 
                                                from productBarcode pb
                                                where p.ProductId=pb.ProductId and pb.isActive='Y' 
                                                order by LastUpdatedDate desc ) as ProdBarCode 
                                        from Product p ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductDTO.SearchByProductParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.INVENTORY_UOM_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOMID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM))
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ", 0) = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " in ( " + searchParameter.Value + " )");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.SITE_ID)
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOM_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE_ID_LIST))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") " + " ) ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = N'" + searchParameter.Value + "'");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like N" + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.INVENTORY_UOM_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOMID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM))
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ", 0) = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " in ( " + searchParameter.Value + " )");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.SITE_ID)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOM_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE_ID_LIST))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") " + " ) ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + " = N'" + searchParameter.Value + "') ");
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetAdvancedProductList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception(utilities.MessageUtils.getMessage(1244) + searchParameter.Key);
                    }
                }
                //query.Append(" Order by DeploymentPlannedDate, ProductId ASC");
                if (searchParameters.Count > 0)
                    selectProductQuery = selectProductQuery + query;
            }

            DataTable productData = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(), sqlTransaction);
            if (productData.Rows.Count > 0)
            {
                List<ProductDTO> productList = new List<ProductDTO>();
                foreach (DataRow productDataRow in productData.Rows)
                {
                    ProductDTO productDataObject = GetProductDTO(productDataRow);
                    productList.Add(productDataObject);
                }
                log.Debug("Ends-GetProductList(searchParameters) Method by returning productList.");
                return productList;
            }
            else
            {
                log.Debug("Ends-GetProductList(searchParameters) Method by returning null.");
                return null;
            }
        }

        ///<summary>
        ///Gets the cost of the Product 
        ///</summary>
        public decimal GetBOMProductCost(int productId)
        {
            log.Debug("Starts-GetBOMProductCost(productId) Method.");
            string Query = @"select dbo.GetBOMProductCost(@ProductId) ";
            SqlParameter[] Parameter = new SqlParameter[1];
            Parameter[0] = new SqlParameter("@ProductId", productId);
            DataTable dtcost = dataAccessHandler.executeSelectQuery(Query, Parameter, sqlTransaction);
            if (dtcost.Rows.Count > 0)
            {
                decimal cost = Convert.ToDecimal(dtcost.Rows[0][0]);
                log.Debug("Ends-GetBOMProductCost(productId) Method by returnting cost.");
                return cost;
            }
            else
            {
                log.Debug("Ends-GetBOMProductCost(productId) Method by returnting null.");
                return 0;
            }

        }

        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="AdvancedSearch">Search Criteria for advanced search </param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetSearchCriteriaAllProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, AdvancedSearch AdvancedSearch)
        {
            log.Debug("Starts-GetProductList(searchParameters) Method.");
            int count = 0;
            string selectProductQuery = "";
            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(ExecutionContext.GetExecutionContext());
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "1"));
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, (executionContext == null) ? "-1" : executionContext.GetSiteId().ToString()));
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, "PRODUCT"));
            segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams);

            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
            {
                string pivotColumns = "";
                foreach (SegmentDefinitionDTO sd in segmentDefinitionDTOList)
                {
                    pivotColumns += ", [" + sd.SegmentName + "]";
                }
                selectProductQuery = "select * " +
                                            pivotColumns +
                                     @"from (
                                                select pIn.*, 
                                                    u.UOM, pd.product_id ,
                                                    (select top 1 BarCode 
                                                        from productBarcode pb
                                                        where pIn.ProductId=pb.ProductId and pb.isActive='Y' 
                                                        order by LastUpdatedDate desc ) as ProdBarCode,
                                                    segmentname,
                                                    valuechar
                                                from Product pIn left outer join UOM u on pIn.UomId = u.UOMId
                                                left outer join products pd on  pd.product_id = pIn.ManualProductId
                                                    left outer join SegmentDataView sdv on sdv.SegmentCategoryId = pIn.segmentcategoryid
			                            )p1 
                                    PIVOT 
							        ( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as p ";
            }
            else
            {
                selectProductQuery = @"select *
                                       from (
                                            select pIn.*, u.UOM, pd.product_id ,
                                                (select top 1 BarCode 
                                                    from productBarcode pb
                                                    where pIn.ProductId=pb.ProductId and pb.isActive='Y' 
                                                    order by LastUpdatedDate desc ) as ProdBarCode 
                                            from Product pIn left outer join UOM u on pIn.UomId = u.UOMId 
                                                             left outer join products pd on  pd.product_id = pIn.ManualProductId)p";
            }
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductDTO.SearchByProductParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.VENDOR_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.INVENTORY_UOM_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOMID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID))
                            {
                                query.Append("isnull(" + DBSearchParameters[searchParameter.Key] + ", '') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.SITE_ID)
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.BARCODE)
                            {
                                query.Append(" p.ProductId in ( select productId from  ProductBarcode where barcode like " + "N'%" + searchParameter.Value + "%' and isactive ='Y') ");
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH
                                || searchParameter.Key == ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = N'" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOM_ID_LIST) ||
                                     searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE_ID_LIST))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") " + " ) ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME))
                            {
                                query.Append("and p.ProductId in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdgf.DisplayGroup like '%" + searchParameter.Value + "%')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE))
                            {
                                query.Append("and (ISNULL((Select ISNULL(LotControlled,'0') from product where product.ManualProductId = p.product_id),'1') = '" + searchParameter.Value + "')");
                            }
                            else if (AdvancedSearch != null)
                            {
                                query.Append(" (" + AdvancedSearch.searchCriteria + ") ");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY) || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.VENDOR_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.INVENTORY_UOM_ID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOMID)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID))
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ", 0)" + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(" and isnull(" + DBSearchParameters[searchParameter.Key] + ", '') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.SITE_ID)
                            {
                                query.Append(" and (" + searchParameter.Key + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST) ||
                                        searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CATEGORY_ID_LIST) ||
                                        searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.UOM_ID_LIST) ||
                                        searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.ITEM_TYPE_ID_LIST))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") " + " ) ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key == ProductDTO.SearchByProductParameters.BARCODE)
                            {
                                query.Append(" and p.ProductId in ( select productId from  ProductBarcode where barcode like " + "'%" + searchParameter.Value + "%' and isactive ='Y') ");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.PRODUCT_NAME_EXACT_MATCH)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DESCRIPTION_EXACT_MATCH)
                                 || searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.BARCODE_EXACT_MATCH))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + " = N'" + searchParameter.Value + "' ) ");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME))
                            {
                                query.Append("and  p.product_id in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdgf.DisplayGroup like '%" + searchParameter.Value + "%')");
                            }
                            else if (searchParameter.Key.Equals(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE))
                            {
                                query.Append("and (ISNULL((Select ISNULL(LotControlled,'0') from product where product.ManualProductId = p.product_id),'1') = '" + searchParameter.Value + "')");
                            }
                            else if (AdvancedSearch != null)
                            {
                                query.Append(" and (" + AdvancedSearch.searchCriteria + ") ");
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetProductList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception(utilities.MessageUtils.getMessage(1244) + searchParameter.Key);
                    }
                }
                //query.Append(" Order by DeploymentPlannedDate, ProductId ASC");
                if (searchParameters.Count > 0)
                    selectProductQuery = selectProductQuery + query;
                log.Debug(selectProductQuery);
            }

            DataTable productData = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(), sqlTransaction);
            if (productData.Rows.Count > 0)
            {
                List<ProductDTO> productList = new List<ProductDTO>();
                foreach (DataRow productDataRow in productData.Rows)
                {
                    ProductDTO productDataObject = GetProductDTO(productDataRow);
                    productList.Add(productDataObject);
                }
                log.Debug("Ends-GetProductList(searchParameters) Method by returning productList.");
                return productList;
            }
            else
            {
                log.Debug("Ends-GetProductList(searchParameters) Method by returning null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="AdvancedSearch">Search Criteria for advanced search </param>
        /// <param name="description">Search Criteria for description </param>
        /// <param name="range">Search Criteria for range </param>
        ///  <param name="checkMinimumQtyLookup">checkMinimumQtyLookup </param>
        /// <param name=" posMachineId"> posMachineId </param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetSearchCriteriaAllProductsWithInventory(AdvancedSearch AdvancedSearch, string description, int range, string checkMinimumQtyLookup, int posMachineId, string userId)
        {
            log.LogMethodEntry(AdvancedSearch, description, range, checkMinimumQtyLookup, posMachineId);
            string selectProductQuery = "";
            if (description == null)
            {
                description = "";
            }

            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(ExecutionContext.GetExecutionContext());
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "1"));
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, "PRODUCT"));
            segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams);
            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
            {
                string pivotColumns = "";
                foreach (SegmentDefinitionDTO sd in segmentDefinitionDTOList)
                {
                    pivotColumns += ", [" + sd.SegmentName + "]";
                }
                selectProductQuery = "select * " +
                                            pivotColumns +
                                     @"from (select  I.Quantity, pIn.*, u.UOM,
                                                    (select top 1 BarCode
                                                       from productBarcode pb
                                                      where pIn.ProductId=pb.ProductId and pb.isActive='Y'
                                                      order by LastUpdatedDate desc ) as ProdBarCode,
                                                     segmentname,
                                                     valuechar
                                                from Product pIn join Products ps on pIn.ManualProductId = ps.product_id
                                                     left outer join UOM u on pIn.UomId = u.UOMId
                                                     left outer join SegmentDataView sdv on sdv.SegmentCategoryId = pIn.segmentcategoryid
                                                    ,Inventory I
                                               where pIn.productId = I.ProductId
                                                and pIn.IsActive = 'Y'
                                                and  I.LocationId = ISNULL((SELECT pos.InventoryLocationId
                                                                             from POSMachines pos
                                                                            where POSMachineId = @posMachine ),pIn.outboundLocationId)
                                              and pIn.IsRedeemable = 'Y'
                                              and (@ignoreQtyCheck = 'Y' OR ( CASE @considerMinimumQtyCheck WHEN 'N' THEN I.Quantity ELSE (I.Quantity - pIn.ReorderPoint) END > 0  ))
                                              and pIn.ProductId not in (SELECT rc.ProductId FROM RedemptionCurrency rc where rc.ProductId = pIn.ProductId)
                                              and pIn.PriceInTickets between 0 and @range
                                              and (pIn.description like N'%" + description.ToString() + @"%' OR pIn.ProductName like N'%" + description.ToString() + @"%')
                                              and not exists (select 1 
					                                            from ProductsDisplayGroup pd , 
						                                            ProductDisplayGroupFormat pdgf,
						                                            POSProductExclusions ppe 
					                                            where ps.product_id = pd.ProductId 
					                                            and pd.DisplayGroupId = pdgf.Id 
					                                            and ppe.ProductDisplayGroupFormatId = pdgf.Id
					                                            and ppe.POSMachineId = @posMachine )
                                              and not exists (select 1 
                                                                from ProductsDisplayGroup pdf,
                                                                     UserRoleDisplayGroupExclusions urdge , 
                                                                     users u
                                                                where urdge.ProductDisplayGroupId = pdf.Id
                                                                and urdge.role_id = u.role_id
                                                                and u.loginId = @loginId )

                                    )p1 
                                    PIVOT 
							        ( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as p";
            }
            else
            {
                selectProductQuery = @"select  I.Quantity, pIn.*, u.UOM, (select top 1 BarCode
                                                                            from productBarcode pb
                                                                           where pIn.ProductId=pb.ProductId and pb.isActive='Y'
                                                                           order by LastUpdatedDate desc ) as ProdBarCode
                                       from Product pIn join Products ps on pIn.ManualProductId = ps.product_id
                                            left outer join UOM u on pIn.UomId = u.UOMId ,
                                            Inventory I
                                       where pIn.productId = I.ProductId
                                       and pIn.IsActive = 'Y'
                                       and I.LocationId = ISNULL((SELECT pos.InventoryLocationId
                                                                     from POSMachines pos
                                                                    where POSMachineId = @posMachine ),pIn.outboundLocationId)
								       and pIn.IsRedeemable = 'Y'
                                       and (@ignoreQtyCheck = 'Y' OR ( CASE @considerMinimumQtyCheck WHEN 'N' THEN I.Quantity ELSE (I.Quantity - pIn.ReorderPoint) END > 0  ))
                                       and pIn.ProductId not in (SELECT rc.ProductId FROM RedemptionCurrency rc where rc.ProductId = pIn.ProductId)
                                       and pIn.PriceInTickets between 0 and @range
                                       and (pIn.description like N'%" + description.ToString() + @"%' OR pIn.ProductName like N'%" + description.ToString() + @"%')
	                                   and not exists (select 1 
					                                    from ProductsDisplayGroup pd , 
						                                    ProductDisplayGroupFormat pdgf,
						                                    POSProductExclusions ppe 
					                                    where ps.product_id = pd.ProductId 
					                                    and pd.DisplayGroupId = pdgf.Id 
					                                    and ppe.ProductDisplayGroupFormatId = pdgf.Id
					                                    and ppe.POSMachineId = @posMachine )
                                       and not exists (select 1 
                                                        from ProductsDisplayGroup pdf,
                                                             UserRoleDisplayGroupExclusions urdge , 
                                                             users u
                                                        where urdge.ProductDisplayGroupId = pdf.Id
                                                        and urdge.role_id = u.role_id
                                                        and u.loginId = @loginId )";
            }
            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any() && AdvancedSearch != null && AdvancedSearch.searchCriteria != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                query.Append(" (" + AdvancedSearch.searchCriteria + ") ");
                selectProductQuery = selectProductQuery + query;
            }
            selectProductQuery = ("(" + selectProductQuery + ")" + "order by PriceInTickets desc");

            SqlParameter[] sqlParam = new SqlParameter[6];
            sqlParam[0] = new SqlParameter("@posMachine", posMachineId);
            sqlParam[1] = new SqlParameter("@range", (range));
            sqlParam[2] = new SqlParameter("@description", description == null ? "%" : description);
            sqlParam[3] = new SqlParameter("@ignoreQtyCheck", ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext.GetExecutionContext(), "ALLOW_TRANSACTION_ON_ZERO_STOCK"));
            sqlParam[4] = new SqlParameter("@considerMinimumQtyCheck", checkMinimumQtyLookup);
            sqlParam[5] = new SqlParameter("@loginId", userId);

            DataTable productData = dataAccessHandler.executeSelectQuery(selectProductQuery, sqlParam, sqlTransaction);
            if (productData.Rows.Count > 0)
            {
                List<ProductDTO> productList = new List<ProductDTO>();
                foreach (DataRow productDataRow in productData.Rows)
                {
                    ProductDTO productDataObject = GetProductDTO(productDataRow);
                    productList.Add(productDataObject);
                }
                log.Debug("Ends-GetProductList(searchParameters) Method by returning productList.");
                return productList;
            }
            else
            {
                log.Debug("Ends-GetProductList(searchParameters) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Returns the tree nodes of the ProductId
        /// </summary>
        /// <param name="LocalProductId"></param>
        /// <returns></returns>
        public TreeNode[] getChildren(int LocalProductId)
        {
            log.Debug("Starts-getChildren(LocalId)method");
            string Query = @"select ChildProductId, Code + 
                                ' (' + description + '-' + cast(convert(decimal(18, 3), quantity) as varchar) + ' ' + uom + 
                                ') Cost: ' + cast(convert(decimal(18, 3), isnull(dbo.GetBOMProductCost(ChildProductId) * quantity / case isnull(innerPackQty, 0) when 0 then 1 else innerPackQty end, 0)) as varchar) " +
                                "from Product p, ProductBOM bom, uom u " +
                                "where bom.ProductId = @productId " +
                                "and p.ProductId = bom.ChildProductId " +
                                "and u.uomId = isnull(bom.uomId,p.InventoryUOMId) and bom.IsActive=1 ";
            SqlParameter[] Parameter = new SqlParameter[1];
            Parameter[0] = new SqlParameter("@ProductId", LocalProductId);
            DataTable dt = dataAccessHandler.executeSelectQuery(Query, Parameter, sqlTransaction);
            if (dt.Rows.Count > 0)
            {
                TreeNode[] tnCollection = new TreeNode[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tnCollection[i] = new TreeNode(dt.Rows[i][1].ToString());
                    tnCollection[i].Tag = dt.Rows[i][0];
                }
                log.Debug("Ends-getChildren(LocalId)method");
                return (tnCollection);
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="parentProductId"></param>
        /// <returns></returns>
        public List<ProductDTO> GetEligibleChildProductList(int parentProductId)
        {
            log.Debug("Starts-GetEligibleChildProductList(parentProductId) Method.");
            string selectProductQuery = @"begin
                        with n (productid) as
                        (select productid
                        from ProductBOM
                        where ChildProductId = @p
                          and IsActive=1
                        union all
                        select np.ProductId
                        from ProductBOM np, n
                        where n.productid = np.ChildProductId
                        and np.IsActive=1)
                        select p.*, (select UOM from UOM where uomid = p.UomId) UOM, null ProdBarCode
                        from product p
                        where p.productid not in (select ProductId from n union select @p)
                        and p.isactive = 'Y'
                    end";
            SqlParameter[] selectProductParameters = new SqlParameter[1];
            selectProductParameters[0] = new SqlParameter("@p", parentProductId);
            DataTable productData = dataAccessHandler.executeSelectQuery(selectProductQuery, selectProductParameters, sqlTransaction);
            if (productData.Rows.Count > 0)
            {
                List<ProductDTO> productList = new List<ProductDTO>();
                foreach (DataRow productDataRow in productData.Rows)
                {
                    ProductDTO productDataObject = GetProductDTO(productDataRow);
                    productList.Add(productDataObject);
                }
                log.Debug("Ends-GetEligibleChildProductList(parentProductId) Method by returning productList.");
                return productList;
            }
            else
            {
                log.Debug("Ends-GetEligibleChildProductList(parentProductId) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// product Quantity in inventory
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int GetProductStockDetails(int productId, int posMachineId)
        {
            log.LogMethodEntry(productId);
            int Quantity = 0;
            string selectQuantityQuery = @"select SUM( Quantity )
                                            from inventory i ,product p
                                           where i.ProductId = @productId
                                            and i.productId=p.productId
                                            and LocationId = ISNULL((SELECT pos.InventoryLocationId
                                                                       from POSMachines pos
                                                                      where POSMachineId = @posMachineId),p.outboundLocationId) ";
            SqlParameter[] selectParameters = new SqlParameter[2];
            selectParameters[0] = new SqlParameter("@productId", productId);
            selectParameters[1] = new SqlParameter("@posMachineId", posMachineId);
            DataTable dt = dataAccessHandler.executeSelectQuery(selectQuantityQuery, selectParameters, sqlTransaction);
            if (dt.Rows.Count > 0)
            {
                Quantity = Convert.ToInt32(dt.Rows[0][0]);
            }
            //foreach (DataRow i in dt.Rows)
            //{
            //    Quantity += Convert.ToInt32(i[0]);
            //}
            log.LogMethodExit(Quantity);
            return (Quantity);
        }

        /// <summary>
        /// Gets the ProductBarcodeDTO List for ProductBarcodeSet Id List
        /// </summary>
        /// <param name="manualProductIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<ProductDTO> GetProductDTOListOfProducts(List<int> manualProductIdList)
        {
            log.LogMethodEntry(manualProductIdList);
            List<ProductDTO> list = new List<ProductDTO>();
            string query = @"select p.*,u.UOM,
                                            (select top 1 BarCode 
                                                from productBarcode pb
                                                where p.ProductId=pb.ProductId and pb.isActive='Y' 
                                                order by LastUpdatedDate desc ) as ProdBarCode 
                                        from Product p left outer join UOM u on p.UomId = u.UOMId 
                                        inner join @ManualProductIdList List on ManualProductId = List.Id ";

            DataTable table = dataAccessHandler.BatchSelect(query, "@ManualProductIdList", manualProductIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Returns the products without inventory entry
        /// </summary>
        /// <param name="manualProductIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<ProductDTO> GetProductDTOListWithoutInventory(int siteId,
                                                                  bool activeChildRecords = false)
        {
            log.LogMethodEntry(siteId, activeChildRecords);
            List<ProductDTO> list = null;
            //22-Aug-2016 - Query modified
            string selectProductQuery = @"select p.*, (select UOM from UOM where uomid = p.UomId) UOM,
                                            (select top 1 BarCode 
                                                from productBarcode pb
                                                where p.ProductId=pb.ProductId and pb.isActive='Y' 
                                                order by LastUpdatedDate desc) as ProdBarCode 
                                        from Product p 
                                        WHERE NOT EXISTS (SELECT 1 FROM Inventory Where productId = p.ProductId) ";
            if (activeChildRecords)
            {
                selectProductQuery += " AND p.IsActive = 'Y' ";
            }
            if (siteId != -1)
            {
                selectProductQuery += " AND p.site_id = " + siteId;
            }
            DataTable table = dataAccessHandler.executeSelectQuery(selectProductQuery, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
        public DateTime? GetProductModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastModDttm) LastModDttm  
                            FROM (
                            select max(LastModDttm) LastModDttm from Product WHERE (site_id = -1 or -1 = -1)) LastModDttm";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastModDttm"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastModDttm"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ProductDTO> GetMostViewedProductDTOList(int currentPage, int pageSize)
        {
            log.LogMethodEntry(currentPage, pageSize);
            string query = @"select productId,COUNT(*) from InventoryTransaction where  TrxDate >= GetDate()-7
                                group by productId having COUNT(*) > 0
                                order by COUNT(*) desc";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
            List<ProductDTO> productDTOList = GetProductDTOList(currentPage, pageSize, dataTable);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        internal List<ProductDTO> GetLowStockProductDTOList(int currentPage, int pageSize, int quantity)
        {
            log.LogMethodEntry(currentPage, pageSize);
            string query = @"select sum(Quantity) LowStockQuantity,ProductId
                                    from(
                                    SELECT Distinct(ProductId),Quantity
                                    FROM
                                    inventory where Quantity < @quantity) as v
                                    group by ProductId
                                    order by LowStockQuantity asc";
            SqlParameter parameter = new SqlParameter("@quantity", quantity);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            List<ProductDTO> productDTOList = GetProductDTOList(currentPage, pageSize, dataTable);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        internal List<ProductDTO> GetMostWastedProductDTOList(int currentPage, int pageSize)
        {
            log.LogMethodEntry(currentPage, pageSize);
            string query = @"select productId,COUNT(*) from InventoryAdjustments
								where Timestamp >= GetDate()-7 and
								ToLocationId is not null
                                and ToLocationId = (select locationId from Location 
                                where Name = 'Wastage') 
                                group by productId having COUNT(*) > 0
                                order by COUNT(*) desc";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
            List<ProductDTO> productDTOList = GetProductDTOList(currentPage, pageSize, dataTable);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        private List<ProductDTO> GetProductDTOList(int currentPage, int pageSize, DataTable dataTable)
        {
            log.LogMethodEntry(currentPage, pageSize, dataTable);
            List<int> productIdList = new List<int>();
            List<ProductDTO> productDTOList = new List<ProductDTO>();

            if (dataTable.Rows.Count <= 0)
            {
                return productDTOList;
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                productIdList.Add(Convert.ToInt32(dataTable.Rows[i]["productId"]));
            }
            string idList = string.Join(",", productIdList);
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID_LIST, idList));
            productDTOList = GetProductList(searchParams, currentPage, pageSize);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        /// <summary>
        /// product Quantity in inventory
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool GetProductInventory(int productId)
        {
            log.LogMethodEntry(productId);
            bool hasRecord = false;
            string selectQuantityQuery = @"select LotId  
                                            from inventory i ,product p
                                           where i.ProductId = @productId
                                            and i.productId=p.productId
                                            and LocationId = ISNULL((SELECT pos.InventoryLocationId
                                                                       from POSMachines pos
                                                                      where POSMachineId = @posMachineId),p.outboundLocationId) ";
            SqlParameter[] selectParameters = new SqlParameter[2];
            selectParameters[0] = new SqlParameter("@productId", productId);
            selectParameters[1] = new SqlParameter("@posMachineId", utilities.ParafaitEnv.POSMachineId);
            DataTable dt = dataAccessHandler.executeSelectQuery(selectQuantityQuery, selectParameters, sqlTransaction);
            foreach (DataRow i in dt.Rows)
            {
                if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(i[0].ToString()) && Convert.ToInt32(i[0]) > -1)
                {
                    hasRecord = true;
                    break;
                }
            }
            log.LogMethodExit(hasRecord);
            return (hasRecord);
        }
    }
}

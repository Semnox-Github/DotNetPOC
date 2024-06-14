/********************************************************************************************
 * Project Name - Product                                                                          
 * Description  - Download and upload all Inventory product. 
 * 
 **************
 **Version Log
 **************
  *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.70.0    28-Jun-2019      Mehraj                  Created                          
 *2.70.0    20-Aug-2019      Akshay Gulaganji        modified UploadInventoryList class
 *2.70.2    20-Dec-2019      Girish Kundar           Modified : InventoryDTO constructor
 *2.90.0    02-Jul-2020      Deeksha                 Inventory process : Weighted Avg Costing changes.
 *2.90.0    14-Aug-2020      Deeksha                 Modified : IssueFix Added ContInclusiveTax field to ProductDTO
 *2.100.0   13-Sep-2020      Deeksha                 Modified : Recipe Management Enhancement changes
 *2.100.0   01-Dec-2020      Mushahid Faizan         WMS Issue fixes.
 *2.110.0   04-Dec-2020      Mushahid Faizan         Modified : Web Inventory Enhancement changes
 *2.120.1   21-Jun-2021      Deeksha                 Modified : Turn in PIT default to 0 during upload download
 *2.130.0   18-Aug-2021      Abhishek                Modified : Update product details even if it has barcode
 *2.130.2   18-Aug-2021      Abhishek                WMS Fix : Upload product issue fix
 *2.120.5   29-Dec-2021      Abhishek                WMS Fix : Upload product issue fix
 *2.140.1   29-Dec-2021      Abhishek                WMS Fix : Upload product issue fix
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;


namespace Semnox.Parafait.Inventory
{
    public class UploadInventory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private const string PARAFAITINVENTORYPRODUCTS = "Parafait Inventory Products";
        private Sheet responseSheet;
        private UploadInventoryProductDTO uploadInventoryProductDTO;
        private SqlTransaction sqlTransaction;
        private SegmentCategorizationValueDTO segmentCategorizationValueDTO;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public UploadInventory(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            UOMContainer uOMContainer = new UOMContainer(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// Build the sheet object templete UploadInventoryProductDTODefination for webmanagement
        /// </summary>
        /// <returns></returns>
        public Sheet BuildTemplete()
        {
            try
            {
                log.LogMethodEntry();
                Sheet sheet = new Sheet();
                ///All column Headings are in a headerRow object
                Row headerRow = new Row();
                ///All defaultvalues for attributes are in defaultValueRow object
                Row defaultValueRow = new Row();
                ///Mapper class thats map sheet object
                UploadInventoryProductDTODefination uploadInventoryProductDTODefination = new UploadInventoryProductDTODefination(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"));
                ///Building headers from uploadInventoryProductDTODefination
                uploadInventoryProductDTODefination.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);

                CustomSegmentList customSegmentListBL = new CustomSegmentList(executionContext);
                List<CustomSegmentDTO> customSegmentList = customSegmentListBL.GetSegments(executionContext.GetSiteId());

                ///Creation of row with defaultvalues
                if (customSegmentList != null)
                {
                    foreach (Row row in sheet.RowList)
                    {
                        foreach (Cell cell in row.CellList)
                        {
                            string defaultValue = string.Empty;
                            foreach (CustomSegmentDTO customSegmentDTO in customSegmentList)
                            {
                                if (cell.Value == customSegmentDTO.Name.ToString())
                                {
                                    ///Adding default value based on  name
                                    //defaultValue = customSegmentDTO.Name;
                                }
                            }
                            ///Creating a cell with default value
                            defaultValueRow.AddCell(new Cell(defaultValue));
                        }
                    }
                }
                ///Adding Row of defaultvalues to sheet
                sheet.AddRow(defaultValueRow);
                log.LogMethodExit();
                return sheet;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Download Inventory Products
        /// </summary>
        /// <returns></returns>
        public List<UploadInventoryProductDTO> DownloadData()
        {
            try
            {
                log.LogMethodEntry();
                UploadInventoryDataHandler uploadInventoryDataHandler = new UploadInventoryDataHandler();
                log.LogMethodExit();
                return uploadInventoryDataHandler.GetProductData(executionContext.GetSiteId().ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Upload Inventory Product
        /// This methods picks all inventory product and dump in products 
        /// Producttype MANUAL for inventory product
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="category"></param>
        /// <param name="vendor"></param>
        /// <param name="uom"></param>
        /// <param name="tax"></param>
        /// <param name="inboundLocation"></param>
        /// <param name="outboundLocation"></param>
        /// <param name="isPurchasable"></param>
        /// <returns></returns>
        public Sheet UploadData(Sheet sheet, string category, string vendor, string uom, int tax, int inboundLocation, int outboundLocation, bool isPurchasable)
        {
            log.LogMethodEntry(sheet, category, vendor, uom, tax, inboundLocation, outboundLocation, isPurchasable);
            int productId = -1;
            ProductList productListBL = new ProductList(executionContext);
            double openingQuantity = 0;
            double receivePrice = 0;
            int lotId = -1;
            DateTime expiryDate = DateTime.MinValue;
            InventoryTransactionList inventoryTransactionListBL = new InventoryTransactionList(executionContext);
            int inventoryTransactionTypeId = inventoryTransactionListBL.GetInventoryTransactionTypeId("OpeningQuantity");

            //Mapper class initialization. This class does all the converstions for sheet
            //uploadInventoryProductsList contains the collection which will be populated to sheet 
            //uploadInventoryProductDTODefination is the mirror of ProductDTO and Segments which does data manupulation
            //uploadInventoryProductDTODefination inherits complexattribute defination class which has abstract methods like Serialize, DeSerialize and Configure
            UploadInventoryProductDTODefination uploadInventoryProductDTODefination = new UploadInventoryProductDTODefination(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"));

            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    this.sqlTransaction = parafaitDBTrx.SQLTrx;
                    List<ProductDisplayGroupFormatDTO> listProductDisplayGroupFormatDTO = new List<ProductDisplayGroupFormatDTO>();
                    ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
                    List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>
                    {
                        new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, (executionContext.GetSiteId().ToString()))
                    };
                    listProductDisplayGroupFormatDTO = productDisplayGroupList.GetAllProductDisplayGroup(searchParams, true, true, this.sqlTransaction);
                    List<ProductDTO> productListDTO = GetProductList();
                    List<CategoryDTO> categoryDTOList = GetCategoryList();
                    List<VendorDTO> vendorDTOList = GetVendorList();
                    List<UOMDTO> umoDTOList = GetUMOList();
                    List<ProductBarcodeDTO> productBarcodeDTOLists = GetProductBarcodeList();
                    CustomSegmentList customSegmentListBL = new CustomSegmentList(executionContext);
                    List<CustomSegmentDTO> customSegmentsList = customSegmentListBL.GetSegments(executionContext.GetSiteId());
                    for (int i = 1; i < sheet.Rows.Count; i++)
                    {
                        int index = 0;
                        uploadInventoryProductDTO = (UploadInventoryProductDTO)uploadInventoryProductDTODefination.Deserialize(sheet[0], sheet[i], ref index);
                        if (uploadInventoryProductDTO != null)
                        {
                            try
                            {
                                CategoryDTO categoryDTO = null;
                                UOMDTO uomDTO = null;
                                VendorDTO vendorDTO = null;
                                int categoryId = -1;
                                int uomId = -1;
                                int vendorId = -1;
                                if (categoryDTOList != null && categoryDTOList.Any())
                                {
                                    if (string.IsNullOrEmpty(uploadInventoryProductDTO.CategoryName) && Convert.ToInt32(category) > -1)
                                    {
                                        uploadInventoryProductDTO.CategoryName = categoryDTOList.Find(m => m.CategoryId.ToString() == category).Name;
                                    }
                                    categoryDTO = categoryDTOList.Find(m => m.Name == uploadInventoryProductDTO.CategoryName);
                                    if (categoryDTO != null)
                                    {
                                        categoryId = categoryDTO.CategoryId;
                                    }
                                }
                                if (umoDTOList != null && umoDTOList.Any())
                                {
                                    if (string.IsNullOrEmpty(uploadInventoryProductDTO.Uom) && Convert.ToInt32(uom) > -1)
                                    {
                                        uploadInventoryProductDTO.Uom = umoDTOList.Find(m => m.UOMId.ToString() == uom).UOM;
                                    }
                                    uomDTO = umoDTOList.Find(m => m.UOM == uploadInventoryProductDTO.Uom);
                                    if (uomDTO != null)
                                    {
                                        uomId = uomDTO.UOMId;
                                    }
                                }
                                if (vendorDTOList != null && vendorDTOList.Any())
                                {
                                    if (string.IsNullOrEmpty(uploadInventoryProductDTO.Vendor) && Convert.ToInt32(vendor) > -1)
                                    {
                                        uploadInventoryProductDTO.Vendor = vendorDTOList.Find(m => m.VendorId.ToString() == vendor).Name;
                                    }
                                    vendorDTO = vendorDTOList.Find(m => m.Name == uploadInventoryProductDTO.Vendor);
                                    if (vendorDTO != null)
                                    {
                                        vendorId = vendorDTO.VendorId;
                                    }
                                }

                                //sheet[0] contains headings
                                //sheet[i] contains values
                                //uploadInventoryProductDTODefination.Deserialize converts every row of sheet into a single object to UploadInventoryProductDTO

                                if (uploadInventoryProductDTO != null)
                                {
                                    if (!string.IsNullOrEmpty(uploadInventoryProductDTO.DisplayGroup))
                                    {
                                        if (listProductDisplayGroupFormatDTO != null && listProductDisplayGroupFormatDTO.Count > 0)
                                        {
                                            if (listProductDisplayGroupFormatDTO.Any(x => x.DisplayGroup == (uploadInventoryProductDTO.DisplayGroup.ToString())))
                                            {
                                                //continue;
                                            }
                                            else
                                            {
                                                throw new ValidationException(uploadInventoryProductDTO.DisplayGroup + " " + MessageContainerList.GetMessage(executionContext, 2078)); // Invalid Display group
                                            }
                                        }
                                    }
                                    else
                                    {
                                        uploadInventoryProductDTO.DisplayGroup = PARAFAITINVENTORYPRODUCTS; // default Display Group Name
                                    }
                                }

                                //parafaitDBTrx.BeginTransaction();
                                //this.sqlTransaction = parafaitDBTrx.SQLTrx;

                                if (!string.IsNullOrEmpty(uploadInventoryProductDTO.CategoryName) && categoryDTO == null)
                                {

                                    CategoryDTO categoryDTOObj = new CategoryDTO(-1, -1, uploadInventoryProductDTO.CategoryName, true, executionContext.GetUserId(), executionContext.GetSiteId(),
                                                                            string.Empty, false, -1, executionContext.GetUserId(), DateTime.MinValue, DateTime.MinValue);

                                    Semnox.Parafait.Category.Category categorytBL = new Semnox.Parafait.Category.Category(executionContext, categoryDTOObj);
                                    categorytBL.Save(this.sqlTransaction);
                                    categoryId = categorytBL.GetCategoryDTO.CategoryId;

                                    categoryDTOList.Add(categorytBL.GetCategoryDTO);

                                }
                                if (!string.IsNullOrEmpty(uploadInventoryProductDTO.Vendor) && vendorDTO == null)
                                {
                                    if (vendorDTOList != null && vendorDTOList.Any())
                                    {
                                        VendorDTO vendorDTOObj = new VendorDTO(-1, uploadInventoryProductDTO.Vendor, string.Empty, -1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, executionContext.GetUserId(), DateTime.MinValue, true, string.Empty,
                                                                            string.Empty, executionContext.GetSiteId(), string.Empty, false, string.Empty, -1, 0.0, -1, -1, -1, executionContext.GetUserId(), DateTime.MinValue, executionContext.GetUserId(), DateTime.MinValue, string.Empty, string.Empty);

                                        Semnox.Parafait.Vendor.Vendor vendorBL = new Semnox.Parafait.Vendor.Vendor(executionContext, vendorDTOObj);
                                        vendorBL.Save(this.sqlTransaction);

                                        vendorId = vendorBL.getVendorDTO.VendorId;
                                        vendorDTOList.Add(vendorBL.getVendorDTO);
                                    }
                                }
                                if (!string.IsNullOrEmpty(uploadInventoryProductDTO.Uom) && uomDTO == null)
                                {

                                    UOMDTO uomDTOObj = new UOMDTO(-1, uploadInventoryProductDTO.Uom, uploadInventoryProductDTO.Uom, executionContext.GetSiteId(), string.Empty,
                                                            false, -1, true, executionContext.GetUserId(), DateTime.MinValue, executionContext.GetUserId(), DateTime.MinValue);

                                    UOM uomBL = new UOM(executionContext, uomDTOObj);
                                    uomBL.Save(this.sqlTransaction);
                                    uomId = uomBL.getUOMDTO.UOMId;
                                    umoDTOList.Add(uomBL.getUOMDTO);
                                }

                                bool bBreak = false;
                                openingQuantity = 0;
                                receivePrice = 0;
                                expiryDate = DateTime.MinValue;

                                ProductDTO productDTO = null;
                                List<ProductDTO> productDTOList = new List<ProductDTO>();
                                if (productListDTO != null && productListDTO.Any())
                                {
                                    productDTO = productListDTO.Find(product => product.Code == uploadInventoryProductDTO.Code);
                                }

                                //Check if product already exists

                                //If product does not exist, add product
                                if (productDTO == null)
                                {
                                    try
                                    {
                                        int itemTypeId = GeItemType(-1);
                                        productDTO = new ProductDTO(
                                                                    -1, // productId
                                                                    uploadInventoryProductDTO.Code,
                                                                    uploadInventoryProductDTO.Description,
                                                                    uploadInventoryProductDTO.Remarks,
                                                                    categoryId,
                                                                    inboundLocation,
                                                                    uploadInventoryProductDTO.ReorderPoint,
                                                                    uploadInventoryProductDTO.ReorderQty,
                                                                    uomId,
                                                                    0.0, // masterPackQty
                                                                    0.0, // innerPackQty
                                                                    vendorId,
                                                                    uploadInventoryProductDTO.Cost,
                                                                    0.0, // lastPurchasePrice
                                                                    string.IsNullOrEmpty(uploadInventoryProductDTO.Redeemable) ? "Y" : uploadInventoryProductDTO.Redeemable.Substring(0, 1).ToUpper(),
                                                                    string.IsNullOrEmpty(uploadInventoryProductDTO.Sellable) ? "Y" : uploadInventoryProductDTO.Sellable.Substring(0, 1).ToUpper(),
                                                                    isPurchasable == true ? "Y" : "N",// isPurchaseable,
                                                                    executionContext.GetUserId(), // lastModUserId
                                                                    DateTime.MinValue, //lastModDttm,
                                                                    true, // isActive, 
                                                                    uploadInventoryProductDTO.PriceInTickets,
                                                                    outboundLocation,
                                                                    Convert.ToDouble(uploadInventoryProductDTO.SalePrice),
                                                                    string.Empty, // taxInclusiveCost, 
                                                                    string.Empty, // imageFileName, 
                                                                    0.0, // lowerLimitCost, 
                                                                    0.0, // upperLimitCost, 
                                                                    0.0,  // costVariancePrcentage, 
                                                                    executionContext.GetSiteId(),
                                                                    string.Empty, // guid, 
                                                                    false,
                                                                     0, // turnInPriceInTickets, 
                                                                    -1, //segmentCategoryId, 
                                                                    -1, //masterEntityId, 
                                                                    -1, // customDataSetId,
                                                                    uploadInventoryProductDTO.LotControlled,
                                                                    uploadInventoryProductDTO.MarketListItem,
                                                                    string.IsNullOrEmpty(uploadInventoryProductDTO.ExpiryType) ? "N" : uploadInventoryProductDTO.ExpiryType,//check for N, 
                                                                    string.IsNullOrEmpty(uploadInventoryProductDTO.IssuingApproach) ? "None" : uploadInventoryProductDTO.IssuingApproach,
                                                                    string.Empty, //uomPassed,
                                                                    uploadInventoryProductDTO.ExpiryDays,
                                                                    string.Empty, // barCode,
                                                                    uploadInventoryProductDTO.ItemMarkUp,
                                                                    uploadInventoryProductDTO.AutoUpdatePit,
                                                                    uploadInventoryProductDTO.ProductName,
                                                                    executionContext.GetUserId(),
                                                                    DateTime.MinValue, //creationDate, 
                                                                    -1, //manualProductId,
                                                                    tax < 0 ? -1 : tax // purchaseTaxId
                                                                    , uploadInventoryProductDTO.CostIncludesTax
                                                                    , itemTypeId
                                                                    , uploadInventoryProductDTO.YieldPercentage
                                                                    , uploadInventoryProductDTO.IncludeInPlan
                                                                    , uploadInventoryProductDTO.RecipeDescription
                                                                    , uomId
                                                                    , uploadInventoryProductDTO.PreparationTime
                                                                    );

                                        if (productDTO.AutoUpdateMarkup && productDTO.IsRedeemable == "Y")
                                        {
                                            try
                                            {  //recalculate PIT.
                                                double newPITValue = productListBL.calculatePITByMarkUp(productDTO.Cost, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);
                                                if (newPITValue > 0)
                                                    productDTO.PriceInTickets = newPITValue;
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error(ex);
                                                throw;
                                            }
                                        }
                                        if (productDTO.LotControlled && productDTO.IssuingApproach == "None")
                                        {
                                            if (productDTO.ExpiryType == "E" || productDTO.ExpiryType == "D")
                                                productDTO.IssuingApproach = "FEFO";
                                            else
                                                productDTO.IssuingApproach = "FIFO";
                                        }
                                        else
                                        {
                                            productDTO.IssuingApproach = uploadInventoryProductDTO.IssuingApproach == null ? "None" : uploadInventoryProductDTO.IssuingApproach;
                                        }
                                        try
                                        {
                                            openingQuantity = double.IsNaN(uploadInventoryProductDTO.OpeningQty) ? double.NaN : uploadInventoryProductDTO.OpeningQty;
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1365)); //Error validating Open Quantity
                                        }
                                        try
                                        {
                                            if (openingQuantity != 0 && openingQuantity != double.NaN)
                                            {
                                                if (uploadInventoryProductDTO.ReceivePrice.ToString().Length == 0)
                                                {
                                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1738)); //Receive Price is mandatory
                                                }
                                                else
                                                    receivePrice = uploadInventoryProductDTO.ReceivePrice;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1364)); //Error validating receive price
                                        }
                                        try
                                        {
                                            if (productDTO.LotControlled)
                                            {
                                                if (uploadInventoryProductDTO.ExpiryDate.ToString().Length == 0)
                                                {
                                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1363)); //Expiry Date is mandatory
                                                }
                                                else
                                                    expiryDate = DateTime.Parse(uploadInventoryProductDTO.ExpiryDate.ToString());
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 597)); //Invalid date format in Expiry Date
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                                        throw;
                                    }
                                    string[] delimiter = new string[] { "|" }; // changed delimiter to '|' instead of '||'
                                    string[] barCode = uploadInventoryProductDTO.BarCode.ToString().Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                                    try
                                    {
                                        productDTO = RefreshManualProducts(productDTO, uploadInventoryProductDTO);
                                        productListDTO.Add(productDTO);
                                        productId = productDTO.ProductId;
                                        if (productId != -1)
                                        {
                                            if (!string.IsNullOrEmpty(uploadInventoryProductDTO.BarCode))
                                            {

                                                for (int j = 0; j < barCode.Length; j++)
                                                {
                                                    if (string.IsNullOrEmpty(barCode[j]))
                                                        continue;
                                                    ProductBarcodeDTO productBarcodeDTO = SaveProductBarcode(productBarcodeDTOLists, productId, barCode[j]);
                                                    productBarcodeDTOLists.Add(productBarcodeDTO);

                                                }
                                            }
                                            if (openingQuantity != 0 && openingQuantity != Double.NaN)
                                            {
                                                try
                                                {
                                                    lotId = -1;
                                                    if (productDTO.LotControlled)
                                                    {
                                                        InventoryLotDTO inventoryLotRecDTO = new InventoryLotDTO(-1, "", openingQuantity, openingQuantity, receivePrice,
                                                                                                         -1, expiryDate, true, executionContext.GetSiteId(), "", false, -1, "", ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetUserId(), productDTO.InventoryUOMId);
                                                        InventoryLotBL inventoryLotBL = new InventoryLotBL(inventoryLotRecDTO, executionContext);
                                                        inventoryLotBL.Save(this.sqlTransaction);
                                                        lotId = inventoryLotRecDTO.LotId;
                                                    }
                                                    InventoryDTO inventoryDTO = new InventoryDTO(productId, productDTO.DefaultLocationId, openingQuantity, ServerDateTime.Now, executionContext.GetUserId(), 0,
                                                                                executionContext.GetSiteId(), "", false, "", -1, lotId, receivePrice, -1, "", "", "", "", null, "", "", "", "", 0, 0, "", "", ServerDateTime.Now, ServerDateTime.Now, "", "", "", "", productDTO.InventoryUOMId);
                                                    Inventory inventoryBl = new Inventory(inventoryDTO, executionContext);
                                                    inventoryBl.Save(this.sqlTransaction);
                                                    double taxPercentage = GetTaxPercentage(productDTO.PurchaseTaxId);

                                                    // The POSMachineId is setting in API Controller level to executionContext from parafaitEnv.POSMachineId
                                                    InventoryTransactionDTO inventoryTransactionDTO = new InventoryTransactionDTO(-1, -1, ServerDateTime.Now, executionContext.GetUserId(), Environment.MachineName,
                                                                                                            productId, productDTO.DefaultLocationId, openingQuantity, -1, taxPercentage, productDTO.TaxInclusiveCost, -1, executionContext.GetMachineId(),
                                                                                                            executionContext.GetSiteId(), string.Empty, false, -1, inventoryTransactionTypeId, lotId, "", "", executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now, productDTO.InventoryUOMId);
                                                    InventoryTransactionBL inventoryTransactionBL = new InventoryTransactionBL(inventoryTransactionDTO, executionContext);
                                                    inventoryTransactionBL.Save(this.sqlTransaction);

                                                }
                                                catch (Exception ex)
                                                {
                                                    log.Error(ex);
                                                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                                                    throw;
                                                }
                                            }

                                            if (bBreak)
                                                continue;

                                            string message = string.Empty;
                                            SaveProductSegment(productDTO, customSegmentsList);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                                        throw;
                                    }
                                }
                                else //Update product if it already exists
                                {
                                    try
                                    {
                                        productDTO.Description = uploadInventoryProductDTO.Description;
                                        productDTO.ProductName = productDTO.ProductName == string.Empty ? productDTO.Code : uploadInventoryProductDTO.ProductName;

                                        productDTO.PriceInTickets = uploadInventoryProductDTO.PriceInTickets;
                                        productDTO.InnerPackQty = 1; //Setting Default
                                        productDTO.Cost = uploadInventoryProductDTO.Cost;
                                        productDTO.ReorderPoint = uploadInventoryProductDTO.ReorderPoint;
                                        productDTO.ReorderQuantity = uploadInventoryProductDTO.ReorderQty;
                                        productDTO.SalePrice = Convert.ToDouble(uploadInventoryProductDTO.SalePrice);
                                        productDTO.Remarks = uploadInventoryProductDTO.Remarks;
                                        if (productDTO.LotControlled && productDTO.IssuingApproach == "None")
                                        {
                                            if (productDTO.ExpiryType == "E" || productDTO.ExpiryType == "D")
                                                productDTO.IssuingApproach = "FEFO";
                                            else
                                                productDTO.IssuingApproach = "FIFO";
                                        }
                                        else
                                            productDTO.IssuingApproach = uploadInventoryProductDTO.IssuingApproach == null ? "None" : uploadInventoryProductDTO.IssuingApproach;
                                        productDTO.IsRedeemable = uploadInventoryProductDTO.Redeemable == null || string.IsNullOrEmpty(uploadInventoryProductDTO.Redeemable) ? "Y" : uploadInventoryProductDTO.Redeemable.Substring(0, 1).ToUpper();
                                        productDTO.IsSellable = uploadInventoryProductDTO.Sellable == null || string.IsNullOrEmpty(uploadInventoryProductDTO.Sellable) ? "Y" : uploadInventoryProductDTO.Sellable.Substring(0, 1).ToUpper();
                                        //productDTO.ProductId = productId;
                                        productDTO.ItemMarkupPercent = double.IsNaN(uploadInventoryProductDTO.ItemMarkUp) ? double.NaN : uploadInventoryProductDTO.ItemMarkUp;
                                        productDTO.AutoUpdateMarkup = uploadInventoryProductDTO.AutoUpdatePit;
                                        if (productDTO.IsRedeemable == "N" && productDTO.AutoUpdateMarkup)
                                            productDTO.AutoUpdateMarkup = false;
                                        if (productDTO.AutoUpdateMarkup && productDTO.IsRedeemable == "Y")
                                        {
                                            try
                                            {  //recalculate PIT.
                                                double newPITValue = productListBL.calculatePITByMarkUp(productDTO.Cost, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);
                                                if (newPITValue > 0)
                                                    productDTO.PriceInTickets = newPITValue;
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error(ex);
                                                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                                                throw;
                                            }
                                        }
                                        int itemType = productDTO.ItemType;
                                        itemType = GeItemType(itemType);
                                        productDTO.CategoryId = categoryId;
                                        productDTO.DefaultVendorId = vendorId;
                                        productDTO.UomId = uomId;
                                        productDTO.CostIncludesTax = uploadInventoryProductDTO.CostIncludesTax;
                                        productDTO.ItemType = itemType;
                                        productDTO.IncludeInPlan = uploadInventoryProductDTO.IncludeInPlan;
                                        productDTO.RecipeDescription = uploadInventoryProductDTO.RecipeDescription;
                                        productDTO.InventoryUOMId = uomId;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                                        throw;
                                    }
                                    try
                                    {
                                        RefreshManualProducts(productDTO, uploadInventoryProductDTO);
                                        if (productDTO.LotControlled && productDTO.IssuingApproach == "FIFO")
                                        {
                                            InventoryLotBL inventoryLot = new InventoryLotBL(executionContext);
                                            inventoryLot.UpdateNonLotableToLotable(productDTO.ProductId, this.sqlTransaction);
                                        }
                                        if (!string.IsNullOrEmpty(uploadInventoryProductDTO.BarCode))
                                        {
                                            string[] delimiter = new string[] { "|" }; // changed delimiter to '|' instead of '||'
                                            string[] barCode = uploadInventoryProductDTO.BarCode.ToString().Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                                            // SaveProductBarcode(productBarcodeDTOLists, productId, null, true);

                                            if (string.IsNullOrEmpty(barCode.ToString()) == false)
                                            {
                                                for (int j = 0; j < barCode.Length; j++)
                                                {
                                                    if (string.IsNullOrEmpty(barCode[j]))
                                                        continue;

                                                    ProductBarcodeDataHandler productBarcodeDataHandler = new ProductBarcodeDataHandler(this.sqlTransaction);
                                                    if (productBarcodeDataHandler.CheckExistanceForBarCodeWithProduct(productDTO.ProductId, executionContext.GetSiteId(), barCode[j]))
                                                    {
                                                        bBreak = true;
                                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1885)); //Barcode exists
                                                    }

                                                    if (bBreak) // check if inner loop set break
                                                        break;
                                                    ProductBarcodeDTO productBarcodeDTO = SaveProductBarcode(productBarcodeDTOLists, productDTO.ProductId, barCode[j]);
                                                    productBarcodeDTOLists.Add(productBarcodeDTO);


                                                }
                                            }
                                        }
                                        if (bBreak) // check if inner loop set break
                                            continue;

                                        string message = string.Empty;
                                        SaveProductSegment(productDTO, customSegmentsList);

                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        throw;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string errorMessage = string.Empty;
                                string seperator = string.Empty;
                                //In case of exception we add a cell to status object with cell value saved
                                if (responseSheet == null)
                                {
                                    responseSheet = new Sheet();
                                    responseSheet.AddRow(sheet[0]);
                                    responseSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Status")));
                                }
                                responseSheet.AddRow(sheet[i]);

                                if (ex is ValidationException)
                                {
                                    foreach (var validationError in (ex as ValidationException).ValidationErrorList)
                                    {
                                        errorMessage += seperator;
                                        errorMessage += validationError.Message;
                                        seperator = ", ";
                                    }
                                }
                                else
                                {
                                    errorMessage = ex.Message;
                                }
                                responseSheet[responseSheet.Rows.Count - 1].AddCell(new Cell("Failed: " + errorMessage));
                                log.Error(errorMessage);
                                log.LogVariableState("Row", sheet[i]);
                                //parafaitDBTrx.EndTransaction();
                                continue;
                            }
                        }
                        //  sqlTransaction.Commit();
                        //parafaitDBTrx.EndTransaction();
                    }
                    parafaitDBTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }
            }
            log.LogMethodExit(responseSheet);
            return responseSheet;
        }

        private int GeItemType(int itemTypeId)
        {
            log.LogMethodEntry(itemTypeId);
            LookupsList lookupList = new LookupsList(executionContext);
            List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParames = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
            searchParames.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "PRODUCT_ITEM_TYPE"));
            searchParames.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupsDTO> lookuDTOList = lookupList.GetAllLookups(searchParames, true, true);
            if (lookuDTOList != null)
            {
                if (itemTypeId != -1 || !string.IsNullOrEmpty(uploadInventoryProductDTO.ItemType))
                {
                    LookupValuesDTO lookupValuesDTO = lookuDTOList[0].LookupValuesDTOList.Where(x => x.Description == uploadInventoryProductDTO.ItemType).FirstOrDefault();
                    if (lookupValuesDTO != null)
                    {
                        itemTypeId = lookupValuesDTO.LookupValueId;
                    }
                }
                else
                {
                    LookupValuesDTO lookupValuesDTO = lookuDTOList[0].LookupValuesDTOList.Where(x => x.LookupValue == "STANDARD_ITEM").FirstOrDefault();
                    if (lookupValuesDTO != null)
                    {
                        itemTypeId = lookupValuesDTO.LookupValueId;
                    }
                }
            }
            log.LogMethodExit(itemTypeId);
            return itemTypeId;
        }

        /// <summary>
        /// Refreshes Manual Product
        /// </summary>
        /// <param name="inventoryProductDTO"></param>
        /// <param name="uploadInventoryProductDTO"></param>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        private ProductDTO RefreshManualProducts(ProductDTO inventoryProductDTO, UploadInventoryProductDTO uploadInventoryProductDTO)
        {
            log.LogMethodEntry(inventoryProductDTO, uploadInventoryProductDTO);
            ProductsDTO manualProductDTO = new ProductsDTO();
            Products productsBL = new Products(inventoryProductDTO.ManualProductId);
            if (inventoryProductDTO.ProductId != -1)
            {
                if (productsBL.GetProductsDTO != null)
                {
                    manualProductDTO = productsBL.GetProductsDTO;
                }
            }
            manualProductDTO.InventoryItemDTO = inventoryProductDTO;
            manualProductDTO.DisplayGroup = uploadInventoryProductDTO.DisplayGroup;
            manualProductDTO.CategoryId = inventoryProductDTO.CategoryId;
            manualProductDTO.DisplayInPOS = uploadInventoryProductDTO.DisplayInPos;
            manualProductDTO.InventoryProductCode = inventoryProductDTO.Code;
            manualProductDTO.ProductName = inventoryProductDTO.ProductName;
            manualProductDTO.Description = inventoryProductDTO.Description;
            manualProductDTO.MapedDisplayGroup = uploadInventoryProductDTO.DisplayGroup;
            manualProductDTO.HsnSacCode = uploadInventoryProductDTO.HsnSacCode;
            manualProductDTO.ActiveFlag = true;
            manualProductDTO.Price = uploadInventoryProductDTO.SalePrice;
            try
            {
                manualProductDTO.ProductTypeId = SetManualProductTypeId();
                productsBL = new Products(executionContext, manualProductDTO);
                productsBL.Save(sqlTransaction);
                inventoryProductDTO = productsBL.GetProductsDTO.InventoryItemDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryProductDTO.ProductId);
            return inventoryProductDTO;
        }

        /// <summary>
        /// Fetches Manual Products
        /// </summary>
        /// <returns></returns>
        private int SetManualProductTypeId()
        {
            log.LogMethodEntry();
            int manualProductTypeId = -1;
            ProductTypeListBL productTypeListBL = new ProductTypeListBL(executionContext);
            List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, "MANUAL"),
                new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())
            };
            List<ProductTypeDTO> listProductTypeDTOs = productTypeListBL.GetProductTypeDTOList(searchParameters, sqlTransaction);
            if (listProductTypeDTOs != null && listProductTypeDTOs.Count > 0)
            {
                manualProductTypeId = listProductTypeDTOs[0].ProductTypeId;
            }
            log.LogMethodExit();
            return manualProductTypeId;
        }
        /// <summary>
        /// Gets TaxPercentage by taxId
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns></returns>
        private double GetTaxPercentage(int taxId)
        {
            log.LogMethodEntry(taxId);
            double taxPercentage = 0;
            TaxList purchaseTaxList = new TaxList(executionContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchByPTaxParameters;
            searchByPTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            searchByPTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_ID, taxId.ToString()));
            searchByPTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
            List<TaxDTO> purchaseTaxListDTO = purchaseTaxList.GetAllTaxes(searchByPTaxParameters, false, false, sqlTransaction);

            if (purchaseTaxListDTO != null)
            {
                if (purchaseTaxListDTO.Count > 1)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1303, taxId.ToString())); // More than one record exists in Inventory for Tax id &1. 
                }
                else
                {
                    taxPercentage = purchaseTaxListDTO[0].TaxPercentage;
                }
            }
            log.LogMethodExit(taxPercentage);
            return taxPercentage;
        }

        private List<SegmentCategorizationValueDTO> SaveSegmentData(CustomSegmentDTO items, KeyValuePair<string, string> item, int segmentCategoryId = -1)
        {
            log.LogMethodEntry(items, item, segmentCategoryId);
            List<SegmentCategorizationValueDTO> segmentCategorizationValues = new List<SegmentCategorizationValueDTO>();
            segmentCategorizationValueDTO = new SegmentCategorizationValueDTO();
            if (items.DataSourceType == "STATIC LIST")
            {
                SegmentDefinitionSourceMapList segmentDefinitionSourceMap = new SegmentDefinitionSourceMapList(executionContext);
                List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>> searchParameters = new List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>>();
                searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_ID, items.SegmentDefinationId));
                searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.DATA_SOURCE_TYPE, items.DataSourceType));
                searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTO = segmentDefinitionSourceMap.GetAllSegmentDefinitionSourceMaps(searchParameters, true);
                var a = segmentDefinitionSourceMapDTO.SelectMany(x => x.SegmentDefinitionSourceValueDTOList).SingleOrDefault(x => x.ListValue == item.Value);
                if (a != null)
                {
                    segmentCategorizationValueDTO.SegmentStaticValueId = a.SegmentDefinitionSourceValueId;
                }
                else
                {
                    throw new Exception("Invalid value for Segment ");
                }
            }
            else if (items.DataSourceType == "DYANAMIC LIST")
            {
                segmentCategorizationValueDTO.SegmentDynamicValueId = item.Value;
            }
            else if (items.DataSourceType == "TEXT")
            {
                segmentCategorizationValueDTO.SegmentValueText = item.Value;
            }
            else if (items.DataSourceType == "DATE")
            {
                segmentCategorizationValueDTO.SegmentValueDate = Convert.ToDateTime(item.Value);
            }
            segmentCategorizationValueDTO.SegmentCategoryId = segmentCategoryId;
            segmentCategorizationValueDTO.SegmentDefinitionId = Convert.ToInt32(items.SegmentDefinationId);
            segmentCategorizationValueDTO.Siteid = executionContext.GetSiteId();
            segmentCategorizationValueDTO.CreatedBy = executionContext.GetUserId();
            segmentCategorizationValueDTO.CreationDate = ServerDateTime.Now;
            segmentCategorizationValueDTO.LastUpdatedBy = executionContext.GetUserId();
            segmentCategorizationValueDTO.IsChanged = false;
            segmentCategorizationValues.Add(segmentCategorizationValueDTO);
            log.LogMethodExit(segmentCategorizationValues);
            return segmentCategorizationValues;
        }

        /// <summary>
        /// Get ProductDTO lists
        /// </summary>
        /// <returns></returns>
        private List<ProductDTO> GetProductList()
        {
            log.LogMethodEntry();
            ProductList productListBL = new ProductList(executionContext);
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>
            {
                new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString())
            };
            List<ProductDTO> productDTOs = productListBL.GetAllProducts(productSearchParams, false);
            log.LogMethodExit(productDTOs);
            return productDTOs;

        }
        /// <summary>
        /// Get CategoryDTO lists
        /// </summary>
        /// <returns></returns>
        private List<CategoryDTO> GetCategoryList()
        {
            log.LogMethodEntry();
            CategoryList categoryListBL = new CategoryList(executionContext);
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categorySearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>
            {
                new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString())
            };
            List<CategoryDTO> categoryDTOs = categoryListBL.GetAllCategory(categorySearchParams);
            log.LogMethodExit(categoryDTOs);
            return categoryDTOs;
        }
        /// <summary>
        /// Get VendorDTO lists
        /// </summary>
        /// <returns></returns>
        private List<VendorDTO> GetVendorList()
        {
            log.LogMethodEntry();
            VendorList vendorListBL = new VendorList(executionContext);
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>
            {
                new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, executionContext.GetSiteId().ToString())
            };
            List<VendorDTO> vendorDTOs = vendorListBL.GetAllVendors(vendorSearchParams);
            log.LogMethodExit(vendorDTOs);
            return vendorDTOs;
        }
        /// <summary>
        /// Get UMODTO lists
        /// </summary>
        /// <returns></returns>
        private List<UOMDTO> GetUMOList()
        {
            log.LogMethodEntry();
            UOMList uomListBL = new UOMList(executionContext);
            List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomSearchParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>
            {
                new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, executionContext.GetSiteId().ToString())
            };
            List<UOMDTO> uOMDTOs = uomListBL.GetAllUOMs(uomSearchParams);
            log.LogMethodExit(uOMDTOs);
            return uOMDTOs;
        }
        /// <summary>
        /// Get ProductBarcode lists
        /// </summary>
        /// <returns></returns>
        private List<ProductBarcodeDTO> GetProductBarcodeList()
        {
            log.LogMethodEntry();
            List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.IS_ACTIVE, "1"),
                new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())
            };
            ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(executionContext);
            List<ProductBarcodeDTO> productBarcodeDTOs = productBarcodeListBL.GetProductBarcodeDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(productBarcodeDTOs);
            return productBarcodeDTOs;
        }
        public ProductBarcodeDTO SaveProductBarcode(List<ProductBarcodeDTO> productBarcodeDTOLists, int productId, string barCode, bool isUpdate = false)
        {
            ProductBarcodeDTO isProductBarCodeDTO = null;
            if (productBarcodeDTOLists != null && productBarcodeDTOLists.Any())
            {
                isProductBarCodeDTO = productBarcodeDTOLists.Find(barcode => barcode.Product_Id != productId && barcode.BarCode == barCode);
                if (isProductBarCodeDTO != null)
                {
                    log.Error("BarCode Exists");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1885)); // Barcode exists
                }

                isProductBarCodeDTO = isUpdate ? productBarcodeDTOLists.Find(barcode => barcode.Product_Id == productId) : productBarcodeDTOLists.Find(barcode => barcode.Product_Id == productId && barcode.BarCode == barCode);
                if (isProductBarCodeDTO == null)
                {
                    isProductBarCodeDTO = new ProductBarcodeDTO(-1, barCode, productId, true, DateTime.MinValue, executionContext.GetUserId(), executionContext.GetUserId(),
                                                                                DateTime.MinValue, executionContext.GetSiteId(), string.Empty, false, -1);
                }
                if (isProductBarCodeDTO != null)
                {
                    if (!barCode.Contains(isProductBarCodeDTO.BarCode))
                    {
                        isProductBarCodeDTO.IsActive = false;
                    }
                }
                ProductBarcodeBL productBarcodeBL = new ProductBarcodeBL(executionContext, isProductBarCodeDTO);
                productBarcodeBL.Save(sqlTransaction);
                isProductBarCodeDTO = productBarcodeBL.GetProductBarcodeDTO;
            }
            return isProductBarCodeDTO;
        }
        private void SaveProductSegment(ProductDTO productDTO, List<CustomSegmentDTO> customSegmentsList)
        {
            if (customSegmentsList != null && customSegmentsList.Count > 0)
            {
                List<SegmentCategorizationValueDTO> segmentCategorizationValues = new List<SegmentCategorizationValueDTO>();
                if (uploadInventoryProductDTO.CustomSegmentDefinitionList != null && uploadInventoryProductDTO.CustomSegmentDefinitionList.Count > 0)
                {
                    foreach (var item in uploadInventoryProductDTO.CustomSegmentDefinitionList)
                    {
                        List<CustomSegmentDTO> segmentList = customSegmentsList.Where(x => x.Name == item.Key).ToList();
                        foreach (CustomSegmentDTO customSegmentDTO in segmentList)
                        {
                            if (!string.IsNullOrEmpty(item.Value))
                            {
                                segmentCategorizationValues = SaveSegmentData(customSegmentDTO, item, productDTO.SegmentCategoryId);
                            }
                        }
                        if (segmentCategorizationValues.Count != 0)
                        {
                            SegmentCategorizationValue segmentCategorization = new SegmentCategorizationValue(executionContext);
                            segmentCategorization.SaveSegmentCategorizationValues(segmentCategorizationValues, productDTO.ProductId, "PRODUCT", sqlTransaction);
                        }
                        else
                        {
                            productDTO.SegmentCategoryId = -1;
                            ProductBL product = new ProductBL(productDTO);
                            product.Save(sqlTransaction);
                        }
                    }
                }
            }
        }
    }
}

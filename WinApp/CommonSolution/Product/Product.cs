/********************************************************************************************
 * Project Name - Product BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.3.0       05-Jul-2017      Archana/Guru S A     IsAvailableInInventory and 
 *                                                  GetSearchCriteriaAllProductsWithnventory are added
 *2.60        10-Apr-2019      Archana              Include/Exclude for redeemable products changes
 *2.60.2      29-May-2019      Jagan Mohan          Code merge from Development to WebManagementStudio
 *2.70..0     27-June-2019     Jagan Mohana         Created the method GenerateUPCBarCode(), GetCheckBit() and GetInventoryLocations()
 *            27-July-2019     Jagan Mohana         GetInventoryLocations() method moved to Inventory class.
 *2.70.2      19-Dec-2019      Akshay G             Modified Save() - changed parameters GetCustomAttributesDTOMap()
 *2.90.0      20-May-2020      Deeksha              Modified :Bulk product publish for inventory products & weighted average costing changes.
 *2.100.0     25-Aug-2020      Deeksha              Modified : Build BOM DTO list during product save /fetch
 *2.110.0     19-Oct-2020      Mushahid Faizan      Modified : Inventory Enhancement
 *2.110.0     15-Dec-2020      Deeksha              Modified : Web Inventory Enhancements
 *2.110.0     06-May-2021      Mushahid Faizan      Modified : GetAllProducts() for LowStock products
 *2.120.0     18-May-2021      Mushahid Faizan      Modified : GenerateUPCBarCode() to validate product having id less than 0.
 *2.150.0     22-Sep-2022      Abhishek             Modified : Web Inventory Redesign - Added validation to check stock while product update to Lotcontrolled.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Vendor;
using System.IO;
using System.Drawing;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class ProductBL
    {
        private ProductDTO productDTO;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string avgCostEnabled = "";
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductBL()
        {
            log.LogMethodEntry();
            productDTO = null;
            executionContext = ExecutionContext.GetExecutionContext();
            SetAverageCostEnabledFlag();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="productId">Parameter of the type interger</param>
        public ProductBL(int productId) : this(ExecutionContext.GetExecutionContext(), productId, true)
        {
            log.LogMethodEntry(productId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductId parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="productId">productId</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductBL(ExecutionContext executionContext,
                         int productId,
                         bool loadChildRecords = true,
                         bool activeChildRecords = true,
                         SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, productId, loadChildRecords, activeChildRecords, sqlTransaction);
            this.executionContext = executionContext;
            ProductDataHandler productDataHandler = new ProductDataHandler();
            this.productDTO = productDataHandler.GetProduct(productId, sqlTransaction);
            if (loadChildRecords == false ||
                productDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, productDTO.CustomDataSetId, true, activeChildRecords, sqlTransaction);
            productDTO.CustomDataSetDTO = customDataSetBL.CustomDataSetDTO;
            ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(executionContext);
            productDTO.ProductBarcodeDTOList = productBarcodeListBL.GetProductBarcodeDTOListOfProducts(new List<int> { productId }, activeChildRecords, sqlTransaction);
            BOMList bomListBL = new BOMList(executionContext);
            productDTO.ProductBOMDTOList = bomListBL.GetProductBOMDTOListOfProducts(new List<int> { productId }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="productDTO">Parameter of the type ProductDTO</param>
        public ProductBL(ProductDTO productDTO) : this(ExecutionContext.GetExecutionContext(), productDTO)
        {
            log.LogMethodEntry();
            this.productDTO = productDTO;
            SetAverageCostEnabledFlag();
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates ProductBL object using the ProductDTO
        /// </summary>
        /// <param name="executionContext">Execution Context</param>
        /// <param name="productDTO">ProductDTO object</param>
        public ProductBL(ExecutionContext executionContext, ProductDTO productDTO)
        {
            log.LogMethodEntry(executionContext, productDTO);
            this.executionContext = executionContext;
            this.productDTO = productDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Validates the Product
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(Dictionary<int, CustomAttributesDTO> customAttributesDTOMap)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrors = new List<ValidationError>();
            if (productDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(validationErrors, "Product is not changed.");
                return validationErrors;
            }
            //ExecutionContext deploymentPlanUserContext = executionContext;
            //if (deploymentPlanUserContext == null)
            //{
            //    deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            //}
            ProductDataHandler productDataHandler = new ProductDataHandler();
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH, productDTO.Code));
            List<ProductDTO> productDTOList = productDataHandler.GetProductList(searchParameters);

            if (productDTOList != null && productDTOList.Any())
            {
                if (productDTOList.Exists(x => x.Code.ToLower() == productDTO.Code.ToLower()) && productDTO.ProductId == -1)
                {
                    log.Debug("Duplicate entries detail");
                    validationErrors.Add(new ValidationError("Product", "Code", MessageContainerList.GetMessage(executionContext, "Duplicate Product Code is not allowed", MessageContainerList.GetMessage(executionContext, "Code"))));
                }
                else if (productDTOList.Exists(x => x.Code.ToLower() == productDTO.Code.ToLower() && x.ProductId != productDTO.ProductId))
                {
                    log.Debug("Duplicate Update detail");
                    validationErrors.Add(new ValidationError("Product", "Code", MessageContainerList.GetMessage(executionContext, "Duplicate Product Code is not allowed", MessageContainerList.GetMessage(executionContext, "Code"))));
                }
            }
            if (string.IsNullOrWhiteSpace(productDTO.Code))
            {
                ValidationError validationError = new ValidationError("Product", "Code", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Code")));
                validationErrors.Add(validationError);
            }
            if (string.IsNullOrWhiteSpace(productDTO.Description))
            {
                ValidationError validationError = new ValidationError("Product", "Description", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Description")));
                validationErrors.Add(validationError);
            }
            if (productDTO.CategoryId < 0)
            {
                ValidationError validationError = new ValidationError("Product", "CategoryId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Category")));
                validationErrors.Add(validationError);
            }
            if (productDTO.DefaultLocationId < 0)
            {
                ValidationError validationError = new ValidationError("Product", "DefaultLocationId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Default Location")));
                validationErrors.Add(validationError);
            }
            if (productDTO.OutboundLocationId < 0)
            {
                ValidationError validationError = new ValidationError("Product", "OutboundLocationId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Outbound Location")));
                validationErrors.Add(validationError);
            }
            if (productDTO.CustomDataSetDTO != null)
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, productDTO.CustomDataSetDTO);
                validationErrors.AddRange(customDataSetBL.Validate(customAttributesDTOMap));
            }
            if (productDTO.ProductBarcodeDTOList != null &&
                productDTO.ProductBarcodeDTOList.Any())
            {
                for (int i = 0; i < productDTO.ProductBarcodeDTOList.Count; i++)
                {
                    if (productDTO.ProductBarcodeDTOList[i].IsChanged == false)
                    {
                        continue;
                    }
                    ProductBarcodeBL productBarcodeBL = new ProductBarcodeBL(executionContext, productDTO.ProductBarcodeDTOList[i]);
                    validationErrors.AddRange(productBarcodeBL.Validate());
                }
            }
            log.LogMethodExit(validationErrors);
            return validationErrors;
        }
        /// <summary>
        /// Saves the product
        /// product will be inserted if ProductId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Product not changed.");
                return;
            }
            ExecutionContext context = executionContext;
            if (context == null)
            {
                context = ExecutionContext.GetExecutionContext();
            }
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(context);
            Dictionary<int, CustomAttributesDTO> customAttributesDTOMap = customAttributesListBL.GetCustomAttributesDTOMap(Applicability.INVPRODUCT);
            List<ValidationError> validationErrors = Validate(customAttributesDTOMap);
            if (validationErrors.Count > 0)
            {
                log.LogMethodExit(validationErrors, "Validation failed");
                throw new ValidationException(MessageContainerList.GetMessage(context, "Validation failed"), validationErrors);
            }
            if (productDTO.CustomDataSetDTO != null &&
                productDTO.CustomDataSetDTO.IsChangedRecursive)
            { 
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(context, productDTO.CustomDataSetDTO);
                customDataSetBL.Save(sqlTransaction);
                if (productDTO.CustomDataSetId != productDTO.CustomDataSetDTO.CustomDataSetId)
                {
                    productDTO.CustomDataSetId = productDTO.CustomDataSetDTO.CustomDataSetId;
                }
            }
            if (productDTO.Image != null)
            {
                SaveProductImage();
            }

            if (productDTO.IsChanged)
            {
                ProductDataHandler productDataHandler = new ProductDataHandler(sqlTransaction);
                if (productDTO.ProductId > -1)
                {
                    ProductBL productBL = new ProductBL(context, productDTO.ProductId, false, false, sqlTransaction);
                    if (productDTO.LotControlled != productBL.getProductDTO.LotControlled)
                    {
                        bool hasRecord = productDataHandler.GetProductInventory(productDTO.ProductId);
                        if (hasRecord)
                        {
                            log.LogMethodExit("The selected product has stock at location. The lot controlled status change is prohibited.", "Validation failed");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5062));
                        }
                    }
                }
                productDataHandler.Save(productDTO, context.GetUserId(), context.GetSiteId());
            }
            if (productDTO.ProductBarcodeDTOList != null &&
               productDTO.ProductBarcodeDTOList.Any())
            {
                List<ProductBarcodeDTO> updatedProductBarcodeDTOList = new List<ProductBarcodeDTO>();
                for (int i = 0; i < productDTO.ProductBarcodeDTOList.Count; i++)
                {
                    if (productDTO.ProductBarcodeDTOList[i].IsChanged == false)
                    {
                        continue;
                    }
                    if (productDTO.ProductBarcodeDTOList[i].Product_Id != productDTO.ProductId)
                    {
                        productDTO.ProductBarcodeDTOList[i].Product_Id = productDTO.ProductId;
                    }
                    updatedProductBarcodeDTOList.Add(productDTO.ProductBarcodeDTOList[i]);
                }
                if (updatedProductBarcodeDTOList.Any())
                {
                    ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(context, updatedProductBarcodeDTOList);
                    productBarcodeListBL.Save(sqlTransaction);
                }
            }
            if (productDTO.ProductBOMDTOList != null &&
                productDTO.ProductBOMDTOList.Count != 0)
            {
                foreach (BOMDTO bomDTO in productDTO.ProductBOMDTOList)
                {
                    if (bomDTO.ProductId != productDTO.ProductId)
                    {
                        bomDTO.ProductId = productDTO.ProductId;
                    }
                    if (bomDTO.IsChanged)
                    { 
                        BOMBL bomBL = new BOMBL(executionContext, bomDTO);
                        bomBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save Product Image into the directory.
        /// </summary>
        public void SaveProductImage()
        {
            log.LogMethodEntry(productDTO.Image);
            if (productDTO.Image != null)
            {
                string imageFolder = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY");
                log.Debug(imageFolder);
                string fileName = productDTO.ImageFileName;
                string imagePath = imageFolder + fileName;
                log.Debug(imagePath);
                MemoryStream ms = new MemoryStream(productDTO.Image);
                Image img = Image.FromStream(ms);
                img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Adds a new barcode if not exists. Activates the existing barcode if exists
        /// </summary>
        /// <param name="barcode"></param>
        public void AddBarcode(string barcode)
        {
            log.LogMethodEntry(barcode);
            if (string.IsNullOrWhiteSpace(barcode))
            {
                log.LogMethodExit(null, "Throwing exception - Invalid barcode. Barcode should not be empty.");
                throw new Exception("Invalid barcode. Barcode should not be empty.");
            }
            if (productDTO.ProductBarcodeDTOList == null)
            {
                productDTO.ProductBarcodeDTOList = new List<ProductBarcodeDTO>();
            }
            if (productDTO.ProductBarcodeDTOList.Any(x => x.BarCode.Trim() == barcode.Trim() && x.IsActive == true))
            {
                log.LogMethodExit(null, "Active barcode already exists.");
                return;
            }
            ProductBarcodeDTO productBarcodeDTO = productDTO.ProductBarcodeDTOList.FirstOrDefault(x => x.BarCode.Trim() == barcode.Trim());
            if (productBarcodeDTO == null)
            {
                productBarcodeDTO = new ProductBarcodeDTO();
                productDTO.ProductBarcodeDTOList.Add(productBarcodeDTO);
            }
            productBarcodeDTO.BarCode = barcode.Trim();
            productBarcodeDTO.IsActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// inactivates barcode if exists.
        /// </summary>
        /// <param name="barcode"></param>
        public void RemoveBarcode(string barcode)
        {
            log.LogMethodEntry(barcode);
            if (string.IsNullOrWhiteSpace(barcode))
            {
                log.LogMethodExit(null, "Throwing exception - Invalid barcode. Barcode should not be empty.");
                throw new Exception("Invalid barcode. Barcode should not be empty.");
            }
            if (productDTO.ProductBarcodeDTOList == null)
            {
                log.LogMethodExit(null, "ProductDTO.ProductBarcodeDTOList is empty.");
                return;
            }
            ProductBarcodeDTO productBarcodeDTO = productDTO.ProductBarcodeDTOList.FirstOrDefault(x => x.BarCode.Trim() == barcode.Trim() && x.IsActive == true);
            if (productBarcodeDTO == null)
            {
                log.LogMethodExit(null, "Unable to find the barcode (" + barcode + ").");
                return;
            }
            productBarcodeDTO.IsActive = false;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the active barcode hashset
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetProductBarcodeHashSet()
        {
            log.LogMethodEntry();
            HashSet<string> result = new HashSet<string>();
            if (productDTO == null ||
               productDTO.ProductBarcodeDTOList == null ||
               productDTO.ProductBarcodeDTOList.Any() == false)
            {
                log.LogMethodExit();
                return result;
            }
            result.UnionWith(productDTO.ProductBarcodeDTOList
                                       .Where(x => x.IsActive == true)
                                       .Select(y => y.BarCode));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// This returns the price of the loaded product DTO of passed quantity. 
        /// </summary>
        /// <param name="quantity">Number of products</param>
        /// <returns>Product price inclusive of tax</returns>
        public double GetProductPrice(double quantity)
        {
            log.LogMethodEntry(quantity);
            double taxAmount = 0;
            double productPrice = 0;
            if (productDTO == null)
            {
                log.LogMethodExit();
                return 0;
            }
            if (productDTO.PurchaseTaxId > -1)
            {
                Tax purchaseTax = new Tax(executionContext, productDTO.PurchaseTaxId);
                taxAmount = purchaseTax.GetTaxAmount(((productDTO.LastPurchasePrice == 0) ? productDTO.Cost : productDTO.LastPurchasePrice), productDTO.TaxInclusiveCost.Equals("Y"));
                productPrice = quantity * (productDTO.Cost + taxAmount);
            }
            else
            {
                productPrice = quantity * ((productDTO.LastPurchasePrice == 0) ? productDTO.Cost : productDTO.LastPurchasePrice);
            }
            log.LogMethodExit(productPrice);
            return productPrice;
        }

        public void UpdateProductLastPurchasePrice(int ReceiptId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(ReceiptId, SQLTrx);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            productDataHandler.UpdateProductLastPurchasePrice(ReceiptId, SQLTrx);
            log.LogMethodExit();
        }

        public void UpdateProductCost(int productId, int receiptId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(productId, receiptId, SQLTrx);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            productDataHandler.UpdateProductCost(productId, receiptId, executionContext.GetUserId(), SQLTrx);
            log.LogMethodExit();
        }

        public void UpdatePIT(int productId, double priceInTickets, double markupPercent, string CostSource, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(productId, priceInTickets, CostSource, SQLTrx);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            this.productDTO = productDataHandler.GetProduct(productId, SQLTrx);
            ProductList productList = new ProductList();
            if (productDTO != null)
            {
                //if DPL file provieds markup then calculate PIT based on that and update redeemable product irrespective of auto update setup
                if (productDTO.IsRedeemable == "Y" && markupPercent > 0)
                {
                    double newPITValue;
                    if (CostSource == "Cost")
                        newPITValue = productList.calculatePITByMarkUp(productDTO.Cost, markupPercent, productDTO.DefaultVendorId);
                    else
                        newPITValue = productList.calculatePITByMarkUp(productDTO.LastPurchasePrice, markupPercent, productDTO.DefaultVendorId);

                    if (productDTO.PriceInTickets != newPITValue)
                    {
                        productDTO.PriceInTickets = newPITValue;
                        //Semnox.Core.Product.Product productBL = new Semnox.Core.Product.Product(productDTO);
                        //productBL.Save(SQLTrx);
                    }
                }
                else
                {
                    if (productDTO.AutoUpdateMarkup && productDTO.IsRedeemable == "Y")
                    {
                        try
                        {
                            double newPITValue;
                            if (CostSource == "Cost")
                                newPITValue = productList.calculatePITByMarkUp(productDTO.Cost, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);
                            else
                                newPITValue = productList.calculatePITByMarkUp(productDTO.LastPurchasePrice, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);

                            if (productDTO.PriceInTickets != newPITValue)
                            {
                                Semnox.Core.Utilities.ParafaitDefaultsListBL parafaitDefaults = new Semnox.Core.Utilities.ParafaitDefaultsListBL(executionContext);
                                List<KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>> defaultsSearchParams;
                                defaultsSearchParams = new List<KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>>();
                                defaultsSearchParams.Add(new KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>(Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "MAINTENANCE_START_HOUR"));
                                List<Semnox.Core.Utilities.ParafaitDefaultsDTO> parafaitDefaultsListDTO = parafaitDefaults.GetParafaitDefaultsDTOList(defaultsSearchParams);
                                string rangeStart = "";
                                string rangeEnd = "";
                                if (parafaitDefaultsListDTO != null)
                                    rangeStart = parafaitDefaultsListDTO[0].DefaultValue;
                                parafaitDefaultsListDTO = null;
                                defaultsSearchParams.Clear();
                                defaultsSearchParams.Add(new KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>(Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "MAINTENANCE_END_HOUR"));
                                parafaitDefaultsListDTO = parafaitDefaults.GetParafaitDefaultsDTOList(defaultsSearchParams);
                                if (parafaitDefaultsListDTO != null)
                                    rangeEnd = parafaitDefaultsListDTO[0].DefaultValue;

                                double rangeStartValue = (rangeStart != "" ? Convert.ToDouble(rangeStart) : 0.0);
                                double rangeEndValue = (rangeEnd != "" ? Convert.ToDouble(rangeEnd) : 0.0);
                                if (GenericUtils.WithInHoursRange(rangeStartValue, rangeEndValue))
                                {
                                    productDTO.PriceInTickets = newPITValue;
                                    //Semnox.Core.Product.Product productBL = new Semnox.Core.Product.Product(productDTO);
                                    //productBL.Save(SQLTrx);
                                }
                                else
                                {
                                    AddToBatchJobRequest(productDTO, newPITValue, SQLTrx);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error in UpdatePIT " + ex.Message);
                            throw new Exception("Error in UpdatePIT ");
                        }
                    }
                    else if (productDTO.IsRedeemable == "Y" && !productDTO.AutoUpdateMarkup)
                    {
                        if (productDTO.PriceInTickets != priceInTickets && priceInTickets > 0)
                        {
                            productDTO.PriceInTickets = priceInTickets;
                            //Semnox.Core.Product.Product productBL = new Semnox.Core.Product.Product(productDTO);
                            //productBL.Save(SQLTrx);
                        }
                    }
                }
                this.Save(SQLTrx);
            }
            log.LogMethodExit();
        }

        public void UpdatePIT(double priceInTickets, double markupPercent, string CostSource, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(priceInTickets, markupPercent, CostSource, SQLTrx);
            ProductList productList = new ProductList();
            if (productDTO != null)
            {
                //if DPL file provieds markup then calculate PIT based on that and update redeemable product irrespective of auto update setup
                if (productDTO.IsRedeemable == "Y" && markupPercent > 0)
                {
                    double newPITValue;
                    if (CostSource == "Cost")
                        newPITValue = productList.calculatePITByMarkUp(productDTO.Cost, markupPercent, productDTO.DefaultVendorId);
                    else
                        newPITValue = productList.calculatePITByMarkUp(productDTO.LastPurchasePrice, markupPercent, productDTO.DefaultVendorId);

                    if (productDTO.PriceInTickets != newPITValue)
                    {
                        productDTO.PriceInTickets = newPITValue;
                        //Semnox.Core.Product.Product productBL = new Semnox.Core.Product.Product(productDTO);
                        //productBL.Save(SQLTrx);
                    }
                }
                else
                {
                    if (productDTO.AutoUpdateMarkup && productDTO.IsRedeemable == "Y")
                    {
                        try
                        {
                            double newPITValue;
                            if (CostSource == "Cost")
                                newPITValue = productList.calculatePITByMarkUp(productDTO.Cost, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);
                            else
                                newPITValue = productList.calculatePITByMarkUp(productDTO.LastPurchasePrice, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);

                            if (productDTO.PriceInTickets != newPITValue)
                            {
                                Semnox.Core.Utilities.ParafaitDefaultsListBL parafaitDefaults = new Semnox.Core.Utilities.ParafaitDefaultsListBL(executionContext);
                                List<KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>> defaultsSearchParams;
                                defaultsSearchParams = new List<KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>>();
                                defaultsSearchParams.Add(new KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>(Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "MAINTENANCE_START_HOUR"));
                                List<Semnox.Core.Utilities.ParafaitDefaultsDTO> parafaitDefaultsListDTO = parafaitDefaults.GetParafaitDefaultsDTOList(defaultsSearchParams);
                                string rangeStart = "";
                                string rangeEnd = "";
                                if (parafaitDefaultsListDTO != null)
                                    rangeStart = parafaitDefaultsListDTO[0].DefaultValue;
                                parafaitDefaultsListDTO = null;
                                defaultsSearchParams.Clear();
                                defaultsSearchParams.Add(new KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>(Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "MAINTENANCE_END_HOUR"));
                                parafaitDefaultsListDTO = parafaitDefaults.GetParafaitDefaultsDTOList(defaultsSearchParams);
                                if (parafaitDefaultsListDTO != null)
                                    rangeEnd = parafaitDefaultsListDTO[0].DefaultValue;

                                double rangeStartValue = (rangeStart != "" ? Convert.ToDouble(rangeStart) : 0.0);
                                double rangeEndValue = (rangeEnd != "" ? Convert.ToDouble(rangeEnd) : 0.0);
                                if (GenericUtils.WithInHoursRange(rangeStartValue, rangeEndValue))
                                {
                                    productDTO.PriceInTickets = newPITValue;
                                    //Semnox.Core.Product.Product productBL = new Semnox.Core.Product.Product(productDTO);
                                    //productBL.Save(SQLTrx);
                                }
                                else
                                {
                                    AddToBatchJobRequest(productDTO, newPITValue, SQLTrx);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error in UpdatePIT " + ex.Message);
                            throw new Exception("Error in UpdatePIT ");
                        }
                    }
                    else if (productDTO.IsRedeemable == "Y" && !productDTO.AutoUpdateMarkup)
                    {
                        if (productDTO.PriceInTickets != priceInTickets && priceInTickets > 0)
                        {
                            productDTO.PriceInTickets = priceInTickets;
                            //Semnox.Core.Product.Product productBL = new Semnox.Core.Product.Product(productDTO);
                            //productBL.Save(SQLTrx);
                        }
                    }
                }
                this.Save(SQLTrx);
            }
            log.LogMethodExit();
        }


        private void AddToBatchJobRequest(ProductDTO productDTO, double newPITValue, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(productDTO, newPITValue, SQLTrx);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            int batchJobActionId = -1;
            int batchJobModuleId = -1;
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BATCH_JOB_ACTION"));
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "UPDATE"));
            List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
            if (lookupValuesListDTO != null)
            {
                batchJobActionId = lookupValuesListDTO[0].LookupValueId;
            }
            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BATCH_JOB_MODULE"));
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "INVENTORY"));
            lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
            if (lookupValuesListDTO != null)
            {
                batchJobModuleId = lookupValuesListDTO[0].LookupValueId;
                BatchJobActivityList batchJobActivityList = new BatchJobActivityList();
                List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>> searchBJAParameters;
                searchBJAParameters = new List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>>();
                searchBJAParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.MODULE_ID, batchJobModuleId.ToString()));
                searchBJAParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.ACTION_ID, batchJobActionId.ToString()));
                searchBJAParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.ENTITYNAME, "Product"));
                searchBJAParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.ENTITYCOLUMN, "PriceInTickets"));
                List<BatchJobActivityDTO> batchJobActivityListDTO = batchJobActivityList.GetAlltBatchJobActivityList(searchBJAParameters);
                if (batchJobActivityListDTO != null)
                {
                    BatchJobRequest batchJobRequest;
                    BatchJobRequestDTO batchJobRequestDTO = new BatchJobRequestDTO();
                    batchJobRequestDTO.BatchJobActivityID = batchJobActivityListDTO[0].BatchJobActivityId;
                    batchJobRequestDTO.EntityGuid = productDTO.Guid;
                    batchJobRequestDTO.EntityColumnValue = newPITValue.ToString();
                    batchJobRequestDTO.ProcesseFlag = false;
                    batchJobRequest = new BatchJobRequest(batchJobRequestDTO);
                    batchJobRequest.Save(SQLTrx);
                }
            }
            log.LogMethodExit();
        }

        private void SetAverageCostEnabledFlag()
        {
            log.LogMethodEntry();
            Semnox.Core.Utilities.ParafaitDefaultsListBL parafaitDefaults = new Semnox.Core.Utilities.ParafaitDefaultsListBL(executionContext);
            List<KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>> defaultsSearchParams;
            defaultsSearchParams = new List<KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>>();
            defaultsSearchParams.Add(new KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>(Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "ENABLE_WEIGHTED_AVERAGE_COST_METHOD"));
            List<Semnox.Core.Utilities.ParafaitDefaultsDTO> parafaitDefaultsListDTO = parafaitDefaults.GetParafaitDefaultsDTOList(defaultsSearchParams);
            if (parafaitDefaultsListDTO != null && parafaitDefaultsListDTO.Count > 0)
                avgCostEnabled = parafaitDefaultsListDTO[0].DefaultValue;

            log.LogMethodExit();
        }
        public void UpdatePITAndCost(int productId, double priceInTickets, double markupPercent, int receiptId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(productId, priceInTickets, markupPercent, receiptId, SQLTrx);
            if (avgCostEnabled == "Y")
            {
                UpdateProductCost(productId, receiptId, SQLTrx);
                UpdatePIT(productId, priceInTickets, markupPercent, "Cost", SQLTrx);
            }
            else
            {
                UpdatePIT(productId, priceInTickets, markupPercent, "LastPurchasePrice", SQLTrx);
            }
            log.LogMethodExit();
        }

        public void UpdatePITAndCost(double priceInTickets, double markupPercent, int receiptId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(priceInTickets, markupPercent, receiptId, SQLTrx);
            if (avgCostEnabled == "Y")
            {
                UpdateProductCost(productDTO.ProductId, receiptId, SQLTrx);
                ProductDataHandler productDataHandler = new ProductDataHandler();
                this.productDTO = productDataHandler.GetProduct(productDTO.ProductId, SQLTrx);
                UpdatePIT((priceInTickets > 0 ? priceInTickets : productDTO.PriceInTickets), markupPercent, "Cost", SQLTrx);
            }
            else
            {
                UpdatePIT((priceInTickets > 0 ? priceInTickets : productDTO.PriceInTickets), markupPercent, "LastPurchasePrice", SQLTrx);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Check whether product is available in inventory
        /// </summary>
        public bool IsAvailableInInventory(ExecutionContext executionContext, int quantity, bool checkreorderpoint=true,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(executionContext, quantity, checkreorderpoint, sqlTransaction);
            bool retVal = false;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_TRANSACTION_ON_ZERO_STOCK"))
            {
                retVal = true;
            }
            else
            {
                string checkMinimumQtyLookup = "N";
                if (checkreorderpoint)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                    {
                        new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "REDEMPTION_CHECK_MINIMUM_QTY"),
                        new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "CheckMinimumQty")
                    };

                    List<LookupValuesDTO> checkMinimumQtyLookupList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams,sqlTransaction);
                    if (checkMinimumQtyLookupList != null)
                    {
                        checkMinimumQtyLookup = (checkMinimumQtyLookupList[0].Description).ToString();
                    }
                }
                ProductDataHandler productDataHandler = new ProductDataHandler(sqlTransaction);
                int qty = productDataHandler.GetProductStockDetails(productDTO.ProductId,executionContext.GetMachineId());
                if (((checkMinimumQtyLookup == "N") ? quantity : (quantity + Convert.ToInt32(productDTO.ReorderPoint))) <= qty)
                {
                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            log.LogMethodExit(retVal);
            return retVal;
        }

        /// <summary>
        /// Generating the UPC barcode for the inventory details
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="preferredVendor"></param>
        /// <param name="productCode"></param>
        public void GenerateUPCBarCode(int productId, int preferredVendor, string productCode)
        {
            log.LogMethodEntry(productId, preferredVendor, productCode);
            try
            {
                if (productId < 0)
                {
                    //27-Jun-2019 Updated code to add Message No.
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1009, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                }
                else
                {
                    long preferredvendor = -1;
                    string vendorCode = "";
                    string typeChar = "";
                    int checkBit = 0;
                    string UPCCode = "";

                    try
                    {
                        //Utilities utilities = new Utilities();
                        typeChar = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "UPC_TYPE");
                        //Get the 1st digit the type digit 
                        //typeChar = utilities.getParafaitDefaults("UPC_TYPE");
                        if (string.IsNullOrEmpty(typeChar))
                        {
                            //27-Jun-2016 Updated code to add Message No.
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1011, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                        }
                        //Check if type character has all numeric characters and length is greater than 1
                        else if (typeChar.Length > 1 || !typeChar.All(char.IsDigit))
                        {
                            //27-Jun-2016 Updated code to add Message No.
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1012, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                        }

                        //Check if Product code has all numeric characters
                        if (!productCode.All(char.IsDigit) || productCode.Length > 5)
                        {
                            //27-Jun-2016 Updated code to add Message No.
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1013, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                        }

                        //Left pad product code with 0
                        productCode = productCode.PadLeft(5, '0');

                        if (preferredVendor != 0)
                        {
                            //Get vendorcode
                            VendorList vendorList = new VendorList(executionContext);
                            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> SearchVendorListParameter = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                            SearchVendorListParameter.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDOR_ID, preferredVendor.ToString()));
                            List<VendorDTO> vendorListOnDisplay = vendorList.GetAllVendors(SearchVendorListParameter);

                            if (vendorListOnDisplay != null && vendorListOnDisplay.Count > 0)
                            {
                                if (vendorListOnDisplay[0].VendorCode != null)
                                {
                                    //Check if vendor code is numeric and has length greater than 5
                                    if (!vendorListOnDisplay[0].VendorCode.ToString().All(char.IsDigit) || vendorListOnDisplay[0].VendorCode.ToString().Length > 5)
                                    {
                                        //27-Jun-2016 Updated code to add Message No.
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1014, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                                    }
                                    else
                                    {
                                        //Left pad vendor code with 0
                                        vendorCode = vendorListOnDisplay[0].VendorCode.ToString().PadLeft(5, '0');
                                    }
                                }
                                else
                                {
                                    //27-Jun-2016 Updated code to add Message No.
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1014, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                                }

                            }
                            else
                            {
                                //27-Jun-2016 Updated code to add Message No.
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1014, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                            }
                        }
                        else
                        {
                            //27-Jun-2016 Updated code to add Message No.
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1015, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                        }

                        //Get check bit (Last digit in barcode)
                        checkBit = GetCheckBit(typeChar, vendorCode, productCode);
                        UPCCode = typeChar + vendorCode + productCode + checkBit;

                        //Check for duplicate barcode
                        ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(executionContext);
                        List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> searchParameters;
                        searchParameters = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.BARCODE, UPCCode));
                        searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.IS_ACTIVE, "Y"));
                        searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ProductBarcodeDTO> productBarcodeListDTO = productBarcodeListBL.GetProductBarcodeDTOList(searchParameters);

                        if (productBarcodeListDTO != null)
                        {
                            //27-Jun-2016 Updated code to add Message No.
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 904, MessageContainerList.GetMessage(executionContext, "Generate UPC Barcode")));
                        }

                        //Insert barcode into database
                        ProductBarcodeDTO productBarcodeDTO = new ProductBarcodeDTO();
                        productBarcodeDTO.BarCode = UPCCode;
                        productBarcodeDTO.Product_Id = productDTO.ProductId;
                        productBarcodeDTO.IsActive = true;

                        ProductBarcodeBL productBarcodeBL = new ProductBarcodeBL(executionContext, productBarcodeDTO);
                        productBarcodeBL.Save();
                        /// Barcode added successfully
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw;
                    }
                }
                log.LogMethodExit();
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Getting the check bit (Last digit in barcode) 
        /// </summary>
        /// <param name="typeChar"></param>
        /// <param name="vendorCode"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        private int GetCheckBit(string typeChar, string vendorCode, string productCode)
        {
            log.LogMethodEntry(typeChar, vendorCode, productCode);
            int CheckBit = 0;
            int sumOddDigits = 0;
            int sumEvenDigits = 0;

            string UPCCode = typeChar + vendorCode + productCode;
            for (int i = 0; i < UPCCode.Length; i++)
            {
                if ((i + 1) % 2 == 0)
                {
                    sumEvenDigits += Convert.ToInt32(UPCCode[i].ToString());
                }
                else
                {
                    sumOddDigits += Convert.ToInt32(UPCCode[i].ToString());
                }
            }
            int digitSum = (sumOddDigits * 3) + sumEvenDigits;
            if (digitSum % 10 != 0)
                CheckBit = 10 - (digitSum % 10);
            else
                CheckBit = digitSum % 10;
            log.LogMethodExit(CheckBit);
            return CheckBit;
        }
        /// <summary>
        /// Get the ProductDTO
        /// </summary>
        public ProductDTO getProductDTO
        {
            get { return productDTO; }
        }

    }

    /// <summary>
    /// Manages the list of product
    /// </summary>
    public class ProductList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities = new Utilities();
        ExecutionContext executionContext = null;
        private readonly List<ProductDTO> productDTOList;
        public ProductList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public ProductList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productDTOList"></param>
        public ProductList(ExecutionContext executionContext, List<ProductDTO> productDTOList)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.productDTOList = productDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of Product matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetProductCount(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductDataHandler productDataHandler = new ProductDataHandler(sqlTransaction);
            int count = productDataHandler.GetProductCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Returns the product list
        /// </summary>
        public List<ProductDTO> GetAllProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters,
                                        SqlTransaction SQLTrx = null, int pageNumber = 0, int pageSize = 2000)
        {
            log.LogMethodEntry(searchParameters, SQLTrx);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            List<ProductDTO> productDTOList = productDataHandler.GetProductList(searchParameters, 0, 0, null, SQLTrx);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }
        /// <summary>
        /// Returns the product list
        /// </summary>
        public List<ProductDTO> GetAllProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters,
            bool loadChildRecords,
            bool activeChildRecords = true, int currentPage = 0, int pageSize = 0,
            SqlTransaction sqlTransaction = null, string type = null, bool buildImage = false, string advSearch = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction, type);
            ProductDataHandler productDataHandler = new ProductDataHandler(sqlTransaction);
            List<ProductDTO> productDTOList = new List<ProductDTO>();
            if (!string.IsNullOrEmpty(type))
            {
                if (type.ToLower() == "mostviewed")
                {
                    productDTOList = productDataHandler.GetMostViewedProductDTOList(currentPage, pageSize);
                }
                else if (type.ToLower() == "lowstock")
                {
                    string quantity = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRODUCT_LOW_STOCK_QUANTITY_LIMIT");
                    log.Debug("Low Stock Quantity: "+ quantity);
                    productDTOList = productDataHandler.GetLowStockProductDTOList(currentPage, pageSize, Convert.ToInt32(quantity));
                }
                else if (type.ToLower() == "mostwasted")
                {
                    productDTOList = productDataHandler.GetMostWastedProductDTOList(currentPage, pageSize);
                }
            }
            else
            {
                productDTOList = productDataHandler.GetProductList(searchParameters, currentPage, pageSize, advSearch, sqlTransaction);
                //if (loadChildRecords == false ||
                //    productDTOList == null ||
                //    productDTOList.Any() == false)
                //{
                //    log.LogMethodExit(productDTOList, "Child records are not loaded.");
                //    return productDTOList;
                //}
                if (loadChildRecords && productDTOList != null && productDTOList.Any())
                {
                    //log.LogMethodExit(productDTOList, "Child records are not loaded.");
                    //return productDTOList;

                    BuildProductDTOList(productDTOList, activeChildRecords, sqlTransaction);
                }
                // BuildProductDTOList(productDTOList, activeChildRecords, sqlTransaction);
            }
            if (productDTOList != null && productDTOList.Any() && buildImage)
            {
                foreach (ProductDTO productDTO in productDTOList)
                {
                    productDTO.Image = ReadProductImage(productDTO.ImageFileName);
                    productDTO.IsChanged = false;
                }
            }
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageFileName"></param>
        /// <returns></returns>
        internal byte[] ReadProductImage(string ImageFileName)
        {
            log.LogMethodEntry(ImageFileName);
            try
            {
                string imageFolder = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY");
                log.Debug(imageFolder);
                if (string.IsNullOrEmpty(ImageFileName))
                {
                    return null;
                }
                byte[] imageBytes = System.IO.File.ReadAllBytes(imageFolder + "\\" + ImageFileName.ToString());
                log.Debug(imageBytes);
                return imageBytes;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        private void BuildProductDTOList(List<ProductDTO> productDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, ProductDTO> productDTOCustomDataSetIdMap = new Dictionary<int, ProductDTO>();
            List<int> customDataSetIdList = new List<int>();
            for (int i = 0; i < productDTOList.Count; i++)
            {
                if (productDTOList[i].CustomDataSetId == -1 ||
                   productDTOCustomDataSetIdMap.ContainsKey(productDTOList[i].CustomDataSetId))
                {
                    continue;
                }
                productDTOCustomDataSetIdMap.Add(productDTOList[i].CustomDataSetId, productDTOList[i]);
                customDataSetIdList.Add(productDTOList[i].CustomDataSetId);
            }
            CustomDataSetListBL customDataSetListBL = new CustomDataSetListBL(executionContext);
            List<CustomDataSetDTO> customDataSetDTOList = customDataSetListBL.GetCustomDataSetDTOList(customDataSetIdList, true, activeChildRecords, sqlTransaction);
            if (customDataSetDTOList != null && customDataSetDTOList.Any())
            {
                for (int i = 0; i < customDataSetDTOList.Count; i++)
                {
                    if (productDTOCustomDataSetIdMap.ContainsKey(customDataSetDTOList[i].CustomDataSetId) == false)
                    {
                        continue;
                    }
                    productDTOCustomDataSetIdMap[customDataSetDTOList[i].CustomDataSetId].CustomDataSetDTO = customDataSetDTOList[i];
                }
            }
            Dictionary<int, ProductDTO> productDTOProductIdMap = new Dictionary<int, ProductDTO>();
            List<int> productIdList = new List<int>();
            for (int i = 0; i < productDTOList.Count; i++)
            {
                if (productDTOProductIdMap.ContainsKey(productDTOList[i].ProductId))
                {
                    continue;
                }
                productDTOProductIdMap.Add(productDTOList[i].ProductId, productDTOList[i]);
                productIdList.Add(productDTOList[i].ProductId);
            }
            ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(executionContext);
            List<ProductBarcodeDTO> productBarcodeDTOList = productBarcodeListBL.GetProductBarcodeDTOListOfProducts(productIdList, activeChildRecords, sqlTransaction);
            if (productBarcodeDTOList != null && productBarcodeDTOList.Any())
            {
                for (int i = 0; i < productBarcodeDTOList.Count; i++)
                {
                    if (productDTOProductIdMap.ContainsKey(productBarcodeDTOList[i].Product_Id) == false)
                    {
                        continue;
                    }
                    ProductDTO productDTO = productDTOProductIdMap[productBarcodeDTOList[i].Product_Id];
                    if (productDTO.ProductBarcodeDTOList == null)
                    {
                        productDTO.ProductBarcodeDTOList = new List<ProductBarcodeDTO>();
                    }
                    productDTO.ProductBarcodeDTOList.Add(productBarcodeDTOList[i]);
                }
            }

            BOMList productBOMListBL = new BOMList(executionContext);
            List<BOMDTO> productBOMDTOList = productBOMListBL.GetProductBOMDTOListOfProducts(productIdList, activeChildRecords, sqlTransaction);
            if (productBOMDTOList != null && productBOMDTOList.Any())
            {
                for (int i = 0; i < productBOMDTOList.Count; i++)
                {
                    if (productDTOProductIdMap.ContainsKey(productBOMDTOList[i].ProductId) == false)
                    {
                        continue;
                    }
                    ProductDTO productDTO = productDTOProductIdMap[productBOMDTOList[i].ProductId];
                    if (productDTO.ProductBOMDTOList == null)
                    {
                        productDTO.ProductBOMDTOList = new List<BOMDTO>();
                    }
                    productDTO.ProductBOMDTOList.Add(productBOMDTOList[i]);
                }
            }
        }


        /// <summary>
        /// Returns the product list for Advanced search
        /// </summary>
        public List<ProductDTO> GetAdancedAllProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters);
            ProductDataHandler productDataHandler = new ProductDataHandler(sqlTransaction);
            List<ProductDTO> productDTOList = productDataHandler.GetAdvancedProductList(searchParameters);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="filterCondition">Filter condition and For product table columns use p.,UOM table columns u.</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetBarcodeSearchProducts(string filterCondition, List<SqlParameter> parameterList)
        {
            log.LogMethodEntry(filterCondition);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            List<ProductDTO> productDTOList = productDataHandler.GetProductListOnBarcode(filterCondition, parameterList);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="filterCondition">Filter condition and For product table columns use p.,UOM table columns u.</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetProductList(string filterCondition, List<SqlParameter> parameter)
        {
            log.LogMethodEntry(filterCondition);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            List<ProductDTO> productDTOList = productDataHandler.GetProductList(filterCondition, parameter);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        /// <summary>
        /// Returns the product DTO
        /// </summary>
        public ProductDTO GetProduct(int productId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(productId);
            ProductDataHandler productDataHandler = new ProductDataHandler(SQLTrx);
            ProductDTO productDTO = productDataHandler.GetProduct(productId, SQLTrx);
            log.LogMethodExit(productDTO);
            return productDTO;
        }
        ///<summary>
        ///Returns the Cost of ProductBOM
        ///</summary>
        public decimal GetBOMProductCost(int productId)
        {
            log.LogMethodEntry(productId);
            try
            {
                ProductDataHandler productDataHandler = new ProductDataHandler();
                decimal productCost = productDataHandler.GetBOMProductCost(productId);
                log.LogMethodExit(productCost);
                return productCost;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1250, MessageContainerList.GetMessage(executionContext, "Cost Validation")));
            }
        }

        /// <summary>
        /// Returns the product list for Advanced search
        /// </summary>
        public List<ProductDTO> GetSearchCriteriaAllProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, AdvancedSearch AdvancedSearch)
        {
            log.LogMethodEntry(searchParameters);
            ProductDataHandler productDataHandler = new ProductDataHandler(executionContext);
            log.LogMethodExit();
            return productDataHandler.GetSearchCriteriaAllProducts(searchParameters, AdvancedSearch);
        }

        /// <summary>
        /// Returns the product list for Advanced search
        /// </summary>
        public List<ProductDTO> GetSearchCriteriaAllProductsWithInventory(AdvancedSearch advancedSearch, string description, int range, int posMachineId, string userId)
        {
            log.LogMethodEntry(advancedSearch, description, range, posMachineId, userId);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            string checkMinimumQtyLookup = "N";
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
            {
                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "REDEMPTION_CHECK_MINIMUM_QTY"),
                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "CheckMinimumQty")
            };

            List<LookupValuesDTO> checkMinimumQtyLookupList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (checkMinimumQtyLookupList != null)
            {
                checkMinimumQtyLookup = (checkMinimumQtyLookupList[0].Description).ToString();
            }
            List<ProductDTO> productDTOList = productDataHandler.GetSearchCriteriaAllProductsWithInventory(advancedSearch, description, range, checkMinimumQtyLookup, posMachineId, userId);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        /// <summary>
        /// Returns the tree nodes of the ProductId
        /// </summary>
        /// <param name="LocalProductId"></param>
        /// <returns></returns>
        public TreeNode[] getChildren(int LocalProductId)
        {
            log.LogMethodEntry(LocalProductId);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.LogMethodExit();
            return productDataHandler.getChildren(LocalProductId);
        }
        /// <summary>
        /// Returns the PIT calculated using Markup %
        /// </summary>
        public double calculatePITByMarkUp(double productCost, double itemMarkUpPercent, int vendorId)
        {
            log.LogMethodEntry(productCost, itemMarkUpPercent, vendorId);
            double verifyDouble = 0;
            double markUpPercent;
            double vendorLevelMarkup = double.NaN;
            double newPITValue = 0;
            if (!Double.TryParse(itemMarkUpPercent.ToString(), out verifyDouble) || Double.IsNaN(itemMarkUpPercent))
            {
                VendorList vendorList = new VendorList(executionContext);
                List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters;
                searchParameters = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDOR_ID, vendorId.ToString()));
                List<VendorDTO> vendorDTO = vendorList.GetAllVendors(searchParameters);
                if (vendorDTO != null)
                    vendorLevelMarkup = vendorDTO[0].VendorMarkupPercent;
                else
                    vendorLevelMarkup = Double.NaN;
            }

            markUpPercent = ((Double.TryParse(itemMarkUpPercent.ToString(), out verifyDouble) == false || Double.IsNaN(itemMarkUpPercent)) ? vendorLevelMarkup : itemMarkUpPercent);

            if (productCost == 0 || (Double.TryParse(markUpPercent.ToString(), out verifyDouble) == false) || Double.IsNaN(markUpPercent))
            {
                //Please set Cost and Markup % (at Item or Vendor level) to compute PIT 
                throw new Exception(utilities.MessageUtils.getMessage(1227));
            }
            else
            {
                try
                {
                    Utilities utilities = new Utilities();
                    double ticketCost;
                    try
                    {
                        ticketCost = Convert.ToDouble(utilities.getParafaitDefaults("TICKET_COST"));
                    }
                    catch
                    {
                        ticketCost = 0;
                    }
                    double newPIT;
                    string pitRoundOffString = "";
                    string pitRoundingType = "";
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
                    searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PIT_ROUNDOFF_TO"));
                    searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "ROUNDOFF_TO"));
                    List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                    if (lookupValuesListDTO != null)
                    {
                        pitRoundOffString = lookupValuesListDTO[0].Description;
                    }
                    searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PIT_ROUNDING_TYPE"));
                    searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "ROUNDING_TYPE"));
                    lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                    if (lookupValuesListDTO != null)
                    {
                        pitRoundingType = lookupValuesListDTO[0].Description;
                    }
                    double pitRoundOff = Convert.ToDouble((pitRoundOffString.Trim() != "" ? pitRoundOffString : "1"));
                    int pitRoundingPrecision = 0;
                    newPIT = (productCost + (productCost * (markUpPercent / 100))) / ticketCost;
                    newPITValue = GenericUtils.RoundOffFunction(newPIT, pitRoundOff, pitRoundingPrecision, pitRoundingType);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(utilities.MessageUtils.getMessage(1228));
                }
            }

            log.LogMethodExit(newPITValue);
            return newPITValue;
        }
        /// <summary>
        /// Returns the product list for Parent Product Id
        /// </summary>
        /// <param name="parentProductId"></param>
        /// <returns></returns>
        public List<ProductDTO> GetEligibleChildProductList(int parentProductId)
        {
            log.LogMethodEntry(parentProductId);
            ProductDataHandler productDataHandler = new ProductDataHandler();
            List<ProductDTO> productDTO = productDataHandler.GetEligibleChildProductList(parentProductId);
            log.LogMethodExit(productDTO);
            return productDTO;
        }

        /// <summary>
        /// Validates and saves the productDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productDTOList == null ||
                productDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            List<CustomDataSetDTO> updatedCustomDataSetDTOList = new List<CustomDataSetDTO>();
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
            Dictionary<int, CustomAttributesDTO> customAttributesDTOMap = customAttributesListBL.GetCustomAttributesDTOMap(Applicability.INVPRODUCT);
            for (int i = 0; i < productDTOList.Count; i++)
            {
                ProductDTO productDTO = productDTOList[i];
                if (productDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                ProductBL productBL = new ProductBL(executionContext, productDTO);
                List<ValidationError> validationErrors = productBL.Validate(customAttributesDTOMap);
                if (validationErrors.Any())
                {
                    log.LogMethodExit(null, "Validation failed. " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException("Validation failed for Product.", validationErrors, i);
                }

                if (productDTO.CustomDataSetDTO == null ||
                    productDTO.CustomDataSetDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                updatedCustomDataSetDTOList.Add(productDTO.CustomDataSetDTO);
            }
            if (updatedCustomDataSetDTOList.Any())
            {
                CustomDataSetListBL customDataSetListBL = new CustomDataSetListBL(executionContext, updatedCustomDataSetDTOList);
                customDataSetListBL.Save(sqlTransaction);
            }
            List<ProductDTO> updatedProductDTOList = new List<ProductDTO>(productDTOList.Count);
            for (int i = 0; i < productDTOList.Count; i++)
            {

                if (productDTOList[i].Image != null)
                {
                    log.Debug("Image in Save"+ productDTOList[i].Image);
                    ProductBL productBL = new ProductBL(executionContext, productDTOList[i]);
                    productBL.SaveProductImage();
                }
                if (productDTOList[i].CustomDataSetDTO != null &&
                   productDTOList[i].CustomDataSetId != productDTOList[i].CustomDataSetDTO.CustomDataSetId)
                {
                    productDTOList[i].CustomDataSetId = productDTOList[i].CustomDataSetDTO.CustomDataSetId;
                }
                if (productDTOList[i].IsChanged == false)
                {
                    continue;
                }
                updatedProductDTOList.Add(productDTOList[i]);
            }
            if (updatedProductDTOList.Any())
            {
                ProductDataHandler productDataHandler = new ProductDataHandler(sqlTransaction);
                productDataHandler.Save(updatedProductDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            }
            List<ProductBarcodeDTO> updatedProductBarcodeDTOList = new List<ProductBarcodeDTO>();
            for (int i = 0; i < productDTOList.Count; i++)
            {
                ProductDTO productDTO = productDTOList[i];
                if (productDTO.ProductBarcodeDTOList != null &&
                    productDTO.ProductBarcodeDTOList.Any())
                {
                    for (int j = 0; j < productDTO.ProductBarcodeDTOList.Count; j++)
                    {
                        if (productDTO.ProductBarcodeDTOList[j].IsChanged == false)
                        {
                            continue;
                        }
                        if (productDTO.ProductBarcodeDTOList[j].Product_Id != productDTO.ProductId)
                        {
                            productDTO.ProductBarcodeDTOList[j].Product_Id = productDTO.ProductId;
                        }
                        updatedProductBarcodeDTOList.Add(productDTO.ProductBarcodeDTOList[j]);
                    }
                }
            }
            if (updatedProductBarcodeDTOList.Any())
            {
                ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(executionContext, updatedProductBarcodeDTOList);
                productBarcodeListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductDTO List for ManualProductId List
        /// </summary>
        /// <param name="manualProductIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<ProductDTO> GetProductDTOListOfProducts(List<int> manualProductIdList, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(manualProductIdList, activeChildRecords, sqlTransaction);
            ProductDataHandler productDataHandler = new ProductDataHandler(sqlTransaction);
            List<ProductDTO> productDTOList = productDataHandler.GetProductDTOListOfProducts(manualProductIdList);
            if (loadChildRecords == false ||
                productDTOList == null ||
                productDTOList.Any() == false)
            {
                log.LogMethodExit(productDTOList, "Child records are not loaded.");
                return productDTOList;
            }
            BuildProductDTOList(productDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        public List<ProductDTO> GetProductDTOListWithoutInventory(int siteId,
                                                                  bool loadChildRecords = false,
                                                                  bool activeChildRecords = false,
                                                                  SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, loadChildRecords, activeChildRecords, sqlTransaction);
            ProductDataHandler productDataHandler = new ProductDataHandler(sqlTransaction);
            List<ProductDTO> productDTOList = productDataHandler.GetProductDTOListWithoutInventory(siteId, activeChildRecords);
            if (loadChildRecords == false ||
                productDTOList == null ||
                productDTOList.Any() == false)
            {
                log.LogMethodExit(productDTOList, "Child records are not loaded.");
                return productDTOList;
            }
            BuildProductDTOList(productDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        public DateTime? GetProductModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductDataHandler uomDataHandler = new ProductDataHandler();
            DateTime? result = uomDataHandler.GetProductModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the no of Currencies matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetProductsCount(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            int count = productsDataHandler.GetProductsCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }
    }
}

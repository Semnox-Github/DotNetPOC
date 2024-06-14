//using ParafaitUtils;
using Semnox.Core;
//using Semnox.Core.BatchJobs;
//using Semnox.Core.GenericCore;
//using Semnox.Core.Lookups;
//using Semnox.Core.Vendor;
//using Semnox.Parafait.ParafaitDefaults;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Transaction
{
    public class Product
    {
        private ProductDTO productDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string avgCostEnabled = "";
        /// <summary>
        /// Default constructor
        /// </summary>
        public Product()
        {
            log.Debug("Starts-Product() default constructor");
            productDTO = null;
            SetAverageCostEnabledFlag();
            log.Debug("Ends-Product() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="productId">Parameter of the type interger</param>
        public Product(int productId)
        {
            log.Debug("Starts-Product(ProductDTO) parameterized constructor.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            this.productDTO = productDataHandler.GetProduct(productId);
            log.Debug("Ends-Product(ProductDTO) parameterized constructor.");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="productDTO">Parameter of the type ProductDTO</param>
        public Product(ProductDTO productDTO)
        {
            log.Debug("Starts-Product(ProductDTO) parameterized constructor.");
            this.productDTO = productDTO;
            SetAverageCostEnabledFlag();
            log.Debug("Ends-Product(ProductDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the product
        /// product will be inserted if ProductId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-Save() method.");
              Semnox.Core.Utilities.ExecutionContext deploymentPlanUserContext =   Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            ProductDataHandler productDataHandler = new ProductDataHandler();
            if (productDTO.ProductId < 0)
            {
                int ProductId = productDataHandler.InsertProduct(productDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId(), SQLTrx);
                productDTO.ProductId = ProductId;
            }
            else
            {
                if (productDTO.IsChanged == true)
                {
                    productDataHandler.UpdateProduct(productDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId(), SQLTrx);
                    productDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }

        public void UpdateProductLastPurchasePrice(int ReceiptId, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-UpdateProductLastPurchasePrice(int ReceiptId, SqlTransaction SQLTrx = null) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            productDataHandler.UpdateProductLastPurchasePrice(ReceiptId, SQLTrx);
            log.Debug("Ends-UpdateProductLastPurchasePrice(int ReceiptId, SqlTransaction SQLTrx = null) method.");
        }

        public void UpdateProductCost(int productId, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-UpdateProductCost(int productId, SqlTransaction SQLTrx = null) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            productDataHandler.UpdateProductCost(productId, SQLTrx);
            log.Debug("Ends-UpdateProductCost(int productId, SqlTransaction SQLTrx = null) method.");
        }

        public void UpdatePIT(int productId, double priceInTickets, double markupPercent, string CostSource, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-UpdatePIT(int productId, double priceInTickets, CostSource, sqlTrx) method.");
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
                                Semnox.Core.Utilities.ParafaitDefaultsListBL parafaitDefaults = new Semnox.Core.Utilities.ParafaitDefaultsListBL();
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
            log.Debug("Ends-UpdatePIT(int productId,double priceInTickets, CostSource, sqlTrx) method.");
        }

        public void UpdatePIT(double priceInTickets, double markupPercent, string CostSource, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-UpdatePIT(int productId, double priceInTickets, CostSource, sqlTrx) method.");
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
                                Semnox.Core.Utilities.ParafaitDefaultsListBL parafaitDefaults = new Semnox.Core.Utilities.ParafaitDefaultsListBL();
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
            log.Debug("Ends-UpdatePIT(int productId,double priceInTickets, CostSource, sqlTrx) method.");
        }


        private void AddToBatchJobRequest(ProductDTO productDTO, double newPITValue, SqlTransaction SQLTrx = null)
        {
            log.Debug("Begins-AddToBatchJobRequest(ProductDTO productDTO, double newPITValue, SqlTransaction SQLTrx = null) method.");
            LookupValuesList lookupValuesList = new LookupValuesList();
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
            log.Debug("Ends-AddToBatchJobRequest(ProductDTO productDTO, double newPITValue, SqlTransaction SQLTrx = null) method.");
        }

        private void SetAverageCostEnabledFlag()
        {
            Semnox.Core.Utilities.ParafaitDefaultsListBL parafaitDefaults = new Semnox.Core.Utilities.ParafaitDefaultsListBL();
            List<KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>> defaultsSearchParams;
            defaultsSearchParams = new List<KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>>();
            defaultsSearchParams.Add(new KeyValuePair<Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters, string>(Semnox.Core.Utilities.ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "ENABLE_AVERAGE_COST_METHOD"));
            List<Semnox.Core.Utilities.ParafaitDefaultsDTO> parafaitDefaultsListDTO = parafaitDefaults.GetParafaitDefaultsDTOList(defaultsSearchParams);
            if (parafaitDefaultsListDTO != null && parafaitDefaultsListDTO.Count > 0)
                avgCostEnabled = parafaitDefaultsListDTO[0].DefaultValue;
        }
        public void UpdatePITAndCost(int productId, double priceInTickets, double markupPercent, SqlTransaction SQLTrx = null)
        {
            if (avgCostEnabled == "Y")
            {
                UpdateProductCost(productId, SQLTrx);
                UpdatePIT(productId, priceInTickets, markupPercent, "Cost", SQLTrx);
            }
            else
            {
                UpdatePIT(productId, priceInTickets, markupPercent, "LastPurchasePrice", SQLTrx);
            }
        }

        public void UpdatePITAndCost(double priceInTickets, double markupPercent, SqlTransaction SQLTrx = null)
        {
            if (avgCostEnabled == "Y")
            {
                UpdateProductCost(productDTO.ProductId, SQLTrx);
                ProductDataHandler productDataHandler = new ProductDataHandler();
                this.productDTO = productDataHandler.GetProduct(productDTO.ProductId, SQLTrx);
                UpdatePIT((priceInTickets > 0 ? priceInTickets : productDTO.PriceInTickets), markupPercent, "Cost", SQLTrx);
            }
            else
            {
                UpdatePIT((priceInTickets > 0 ? priceInTickets : productDTO.PriceInTickets), markupPercent, "LastPurchasePrice", SQLTrx);
            }
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
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities = new Utilities();
        /// <summary>
        /// Returns the product list
        /// </summary>
        public List<ProductDTO> GetAllProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllProducts(searchParameters) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-GetAllProducts(searchParameters) method by returning the result of ProductDataHandler.GetProductList() call.");
            return productDataHandler.GetProductList(searchParameters);
        }


        /// <summary>
        /// Returns the product list for Advanced search
        /// </summary>
        public List<ProductDTO> GetAdancedAllProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllProducts(searchParameters) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-GetAllProducts(searchParameters) method by returning the result of ProductDataHandler.GetProductList() call.");
            return productDataHandler.GetAdvancedProductList(searchParameters);
        }

        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="filterCondition">Filter condition and For product table columns use p.,UOM table columns u.</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetBarcodeSearchProducts(string filterCondition)
        {
            log.Debug("Starts-GetBarcodeSearchProducts(filterCondition) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-GetBarcodeSearchProducts(filterCondition) method by returning the result of ProductDataHandler.GetBarcodeSearchProducts(barcode, filterCondition) call.");
            return productDataHandler.GetProductListOnBarcode(filterCondition);
        }

        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="filterCondition">Filter condition and For product table columns use p.,UOM table columns u.</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<ProductDTO> GetProductList(string filterCondition)
        {
            log.Debug("Starts-GetAllProducts(filterCondition) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-GetAllProducts(filterCondition) method by returning the result of ProductDataHandler.GetProductList(filterCondition) call.");
            return productDataHandler.GetProductList(filterCondition);
        }

        /// <summary>
        /// Returns the product DTO
        /// </summary>
        public ProductDTO GetProduct(int productId, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-GetProduct(productId) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-GetProduct(productId) method by returning the result of ProductDataHandler.GetProduct() call.");
            return productDataHandler.GetProduct(productId, SQLTrx);
        }
        ///<summary>
        ///Returns the Cost of ProductBOM
        ///</summary>
        public decimal GetBOMProductCost(int productId)
        {
            log.Debug("Starts-GetBOMProductCost(productId) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-GetBOMProductCost(productId) method by returning the result of ProductDataHandler.GetProduct() call.");
            return productDataHandler.GetBOMProductCost(productId);
        }

        /// <summary>
        /// Returns the product list for Advanced search
        /// </summary>
        public List<ProductDTO> GetSearchCriteriaAllProducts(List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters, AdvancedSearch AdvancedSearch)
        {
            log.Debug("Starts-GetSearchCriteriaAllProducts(searchParameters) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-GetSearchCriteriaAllProducts(searchParameters) method by returning the result of ProductDataHandler.GetProductList() call.");
            return productDataHandler.GetSearchCriteriaAllProducts(searchParameters, AdvancedSearch);
        }

        /// <summary>
        /// Returns the tree nodes of the ProductId
        /// </summary>
        /// <param name="LocalProductId"></param>
        /// <returns></returns>
        public TreeNode[] getChildren(int LocalProductId)
        {
            log.Debug("Starts-getChildren(LocalId)method");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-getChildren(LocalId)method");
            return productDataHandler.getChildren(LocalProductId);
        }
        /// <summary>
        /// Returns the PIT calculated using Markup %
        /// </summary>
        public double calculatePITByMarkUp(double productCost, double itemMarkUpPercent, int vendorId)
        {
            log.Debug("Starts-calculatePITByMarkUp(double productCost, double itemMarkUpPercent, int vendorId) method.");
            double verifyDouble = 0;
            double markUpPercent;
            double vendorLevelMarkup = double.NaN;
            double newPITValue = 0;
            if (!Double.TryParse(itemMarkUpPercent.ToString(), out verifyDouble) || Double.IsNaN(itemMarkUpPercent))
            {
                VendorList vendorList = new VendorList();
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
                    double ticketCost = Convert.ToDouble(utilities.getParafaitDefaults("TICKET_COST"));
                    double newPIT;
                    string pitRoundOffString = "";
                    string pitRoundingType = "";
                    LookupValuesList lookupValuesList = new LookupValuesList();
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
                catch
                {
                    throw new Exception(utilities.MessageUtils.getMessage(1228));
                }
            }

            log.Debug("Ends-calculatePITByMarkUp(double productCost, double itemMarkUpPercent, int vendorId) method.");
            return newPITValue;
        }
        /// <summary>
        /// Returns the product list for Parent Product Id
        /// </summary>
        /// <param name="parentProductId"></param>
        /// <returns></returns>
        public List<ProductDTO> GetEligibleChildProductList(int parentProductId)
        {
            log.Debug("Starts-GetEligibleChildProductList(parentProductId) method.");
            ProductDataHandler productDataHandler = new ProductDataHandler();
            log.Debug("Ends-GetEligibleChildProductList(parentProductId) method by returning the result of ProductDataHandler.GetEligibleChildProductList() call.");
            return productDataHandler.GetEligibleChildProductList(parentProductId);
        }

    }
}

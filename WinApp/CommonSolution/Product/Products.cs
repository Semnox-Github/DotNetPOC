/********************************************************************************************
 * Project Name - Product Display List BL
 * Description  - Product Display List Methods
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   Jeevan              Created 
 ********************************************************************************************
 *1.00        14-Jul-2016   Raghuveera          Modified 
 ********************************************************************************************
 *2.00        20-Jan-2016   Jeevan              Modified 
 *                                              Added Methods related to get product details for reservation 
 *2.40        05-Dec-2018   Jagan Mohana        Added new method GetProductByTypeList
 *2.50        10-Jan-2019   Jagan Mohana        Added new class ProductsList,GetProductByTypeList(),constructors and methods.
 *2.60        08-Feb-2019   Akshay Gulaganji    Added Parameterized constructor with executionContext, productsDTO and method SaveProducts() in Products class.
                                                added method SaveProductsList() in ProductsList class
 *            15-Feb-2019   Akshay Gulaganji    Added a method GetProductDetailsList(searchParameters) in ProductsList Class
 *            04-Mar-2019   Akshay Gulaganji    Added GetProductsDescriptionList() method for fetching Product Description in ProductsList Class
 *            01-Apr-2019   Akshay Gulaganji    modified ActiveFlag DataType( from string to bool) 
 *2.60        10-Apr-2019   Archana             Include/Exclude for redeemable products changes
 *2.60        08-May-2019   Nitin Pai           Added method to fethc product by filter parameters
 *2.70        25-Jun-2019   Mathew Ninan        GetCheckInSlabPrice method added 
 *            14-Apr-2019   Guru S A            Booking Phase 2 changes
 *            29-May-2019   Jagan Mohana        Code merge from Development to WebManagementStudio
              28-Jun-2019   Jagan Mohana        Created SaveDuplicateProductDetails()
 *            29-Jun-2019   Indrajeet Kumar     Created DeleteProductsList() & DeleteProducts()- Implement for Hard Deletion
 *            01-Jul-2019   Indrajeet Kumar     Created GetProductsListFromDisplayGroup() method to get the list of Product from product display group for viator.
 *            30-Jul-2017   Jeevan              Modified BookingPackageProduct class to include ComboProductId as part of booking phase2 changes
 *            05-Sept-2019  Jagan Mohan         Added the new method GetProductsDTOCount()
 *            25-Sept-2019  Jagan Mohana        Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 *2.70.2      19-Nov-2019   Jinto Thomas        Linking the active flag column of product to inventory products active flag
 *            19-Dec-2019   Akshay G            Added CustomDataSetDTO and modified method - Validate(), Save(), GetProductsDTOList()
 *2.80.0      11-Feb-2020   Deeksha             Modified :Re-Enabling New button in inventory module.
 *2.80.0      09-Jun-2020   Jinto Thomas        Enable Active flag for Comboproduct data
 *2.110.0     15-Dec-2020   Deeksha             Modified as part of Web inventory UI design
 *2.110.0     08-Dec-2020   Guru S A            Subscription changes
 *2.120.0     18-Mar-2021   Guru S A            For Subscription phase 2 changes
 *2.140.0     14-Sep-2021   Prajwal S           Modified : GetProductsDTOList to build combo product, upsell and crossSellProducts.
 *2.140.0     24-Dec-2021   Prajwal S           Handling Time Conversion acording to site Time Zones for Start Date and Expiry Date fields
 *2.140.2     04-May-2022   Ashish Sreejith     Updated BookingProduct constructor by added AdvancePercentage field 
 *2.150.0     06-Mar-2022   Girish Kundar       Modified : Added a new column  MaximumQuantity & PauseType to Products
 *2.130.12    22-Dec-2022   Ashish Sreejith     Updated BookingProduct constructor by adding WebDescription field
 *2.150.4     02-Jun-2023   Ashish Sreejith     Updated BookingProduct constructor and added ProductsDisplayGroupFormatDTO field
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Manages the Product Display List Methods for POS, Online etc
    /// </summary>
    public class Products
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ProductsDTO productsDTO;
        ExecutionContext executionContext;
        private int duplicateProductId = -1;
        private int newProductId = -1;
        private SqlTransaction sqlTransaction;
        private List<ProductTypeDTO> productTypeDTOList = null;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Products()
        {
            log.LogMethodEntry();
            this.executionContext = ExecutionContext.GetExecutionContext();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor passing execution context
        /// </summary>
        /// <param name="executionContext">Execution Context</param>
        public Products(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>//Starts: Modification on 14-Jul-2016 for updating the segmentation to products.
        /// This method is used to update the categorization id to the products table
        /// </summary>
        /// <param name="productsId">Product id from the Products table.</param>
        /// <param name="segmentGetegoryId">Segment category Id of the Product</param>
        /// <returns>the count of rows updated.</returns>
        public int UpdateProductsSegmentCategoryId(int productsId, int segmentGetegoryId)
        {
            log.LogMethodEntry(productsId, segmentGetegoryId);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            int updatedId = productsDataHandler.UpdateProductsSegmentCategoryId(productsId, segmentGetegoryId);
            log.LogMethodExit(updatedId);
            return updatedId;
        }//Ends: Modification on 14-Jul-2016 for updating the segmentation to products.

        ///// <summary>
        ///// Get the product by type
        ///// </summary>
        ///// <param name="productType"> product type should be passed</param>
        ///// <param name="siteId"> site id of the product</param>
        ///// <returns></returns>
        //public ProductsDTO GetProductByType(string productType, int siteId)
        //{
        //    log.LogMethodEntry();
        //    ProductsDataHandler productsDataHandler = new ProductsDataHandler();
        //    log.LogMethodExit();
        //    return productsDataHandler.GetProductByType(productType, siteId);            
        //}

        /// <summary>
        /// Get the product by type
        /// </summary>
        /// <param name="productType"> product type should be passed</param>
        /// <param name="siteId"> site id of the product</param>
        /// <returns>Products by ProductType List</returns>
        /// Get the Lookups values for products in games entity by passing the product type
        /// Created by Jagan Mohana - 05-Dec-2018
        public List<ProductsDTO> GetProductByTypeList(string productType, int siteId)
        {
            log.LogMethodEntry(productType, siteId);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, productType));
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, siteId.ToString()));
            List<ProductsDTO> productsDTOLIst = productsDataHandler.GetAllProductList(searchParams);
            productsDTOLIst = SetFromSiteTimeOffset(productsDTOLIst);
            log.LogMethodExit(productsDTOLIst);
            return productsDTOLIst;
        }

        /// <summary>
        /// Fetches the ProductsDTO
        /// </summary>
        /// <param name="productId"></param>
        public Products(int productId) : this()
        {
            log.LogMethodEntry(productId);
            this.executionContext = ExecutionContext.GetExecutionContext();
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            productsDTO = productsDataHandler.GetProductDTO(productId);
            SetFromSiteTimeOffset();
            LoadProductSubscritionDTOChild(productId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Fetches the ProductsDTO. Same query as getProductDetails in Transaction
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="roleId">roleId</param>
        /// <param name="membershipId">MembershipId if available</param>
        public Products(int productId, int roleId, int? membershipId = null) : this()
        {
            log.LogMethodEntry(productId, roleId, membershipId);
            this.executionContext = ExecutionContext.GetExecutionContext();
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            productsDTO = productsDataHandler.GetProductDTO(productId, roleId, membershipId);
            SetFromSiteTimeOffset();
            LoadProductSubscritionDTOChild(productId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor passing executionContext and productsDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productsDTO"></param>
        public Products(ExecutionContext executionContext, ProductsDTO productsDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productsDTO);
            this.productsDTO = productsDTO;
            log.LogMethodExit();
        }
        /*/// <summary>
        /// This method is used to get products list used for purchase
        /// </summary>
        /// <param name="productsFilterParams">ProductsFilterParams type</param>
        /// <returns>returns ProductsWithComboDetails List with the product details.</returns>
        public List<Semnox.Parafait.Product.ProductsWithComboDetails> GetProductsList(Semnox.Parafait.Product.ProductsFilterParams productsFilterParams)
        {
            log.Debug("Starts-GetProductsList(productsFilterParams) method.");

            if (productsFilterParams.LanguageId == -1 && productsFilterParams.LanguageCode != "")
            {
                Languages.Languages languages = new Languages.Languages();
                List<KeyValuePair<Languages.LanguagesDTO.SearchByParameters, string>> languageSerachParam = new List<KeyValuePair<Languages.LanguagesDTO.SearchByParameters, string>>();
                languageSerachParam.Add(new KeyValuePair<Languages.LanguagesDTO.SearchByParameters, string>(Languages.LanguagesDTO.SearchByParameters.LANGUAGE_CODE, productsFilterParams.LanguageCode));
                List<Languages.LanguagesDTO> langaugesList = languages.GetAllLanguagesList(languageSerachParam);
                if (langaugesList.Count > 0)
                {
                    productsFilterParams.LanguageId = langaugesList[0].LanguageId;
                }
                else
                {
                    log.Debug("Error-GetProductsList(productsFilterParams) Language code Not exist ");
                    throw new Exception("Language code Not exist ");
                }
            }

            if ((productsFilterParams.ValidateCard) && (!String.IsNullOrEmpty(productsFilterParams.CardNumber)))
            {
                CardCoreBL cardCore = new CardCoreBL(productsFilterParams.CardNumber);
                CardCoreDTO cardCoreDTO = cardCore.GetCardCoreDTO;
                if (cardCoreDTO.CardId == -1)
                {
                    productsFilterParams.NewCard = true;
                    productsFilterParams.ProductTypeExclude = "RECHARGE";
                }
                else
                {
                    productsFilterParams.NewCard = false;
                    productsFilterParams.ProductTypeExclude = "NEW";
                }
            }

            ProductsDataHandler productsDataHandler=new ProductsDataHandler();
            ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
            List<Semnox.Parafait.Product.ProductsWithComboDetails> foodAndBeverageProductList = new List<Semnox.Parafait.Product.ProductsWithComboDetails>();
            foodAndBeverageProductList =  productsDataHandler.GetProductList(productsFilterParams);

            // update Promotional Price to Products 
            log.Debug("Ends-GetProductsList(productsFilterParams) method .");
            return reservationDatahandler.GetPromotionalPriceUpdate(foodAndBeverageProductList, productsFilterParams);


        }*/

        /// <summary>
        /// Toget all staff card products
        /// </summary>
        /// <param name="filterParams">search parameters</param>
        /// <returns>returns list of staff card produc</returns>
        public List<ProductsStruct> GetStaffCardProducts(ProductsFilterParams filterParams)
        {
            log.LogMethodEntry(filterParams);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            List<ProductsStruct> productsStructList = productsDataHandler.StaffCardProductList(filterParams);
            log.LogMethodExit(productsStructList);
            return productsStructList;
        }


        /// <summary>
        /// GetProductDTOList
        /// </summary>
        /// <param name="productsFilterParams"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetProductDTOList(ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(productsFilterParams);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            List<ProductsDTO> productsDTOList = productsDataHandler.GetProductDTOList(productsFilterParams);
            productsDTOList = SetFromSiteTimeOffset(productsDTOList);
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        /// <summary>
        /// Check if queue products are already part of card or in the transaction lines
        /// </summary>
        /// <param name="cardId">Card Id</param>
        /// <param name="productId">Product Id</param>
        /// <param name="productIdList">List of product Ids based on existing transaction lines</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns></returns>
        public bool CheckForQueueProducts(int cardId, int productId, string productIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(cardId, productId, productIdList, sqlTransaction);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            log.LogMethodExit();
            return productsDataHandler.CheckQueueProductsExistence(cardId, productId, productIdList);
        }

        /// <summary>
        /// Get Special Price value for given product id
        /// </summary>
        /// <param name="productID">Product Id</param>
        /// <param name="price">Price of product</param>
        /// <param name="specialPricingId">Special Pricing Id used</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns>special price</returns>
        public double GetSpecialPrice(int productID, double price, int specialPricingId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productID, price, specialPricingId, sqlTransaction);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            log.LogMethodExit();
            return productsDataHandler.GetSpecialPrice(productID, price, specialPricingId);
        }

        /// <summary>
        /// Get slab price value for given check-in/check-out product id
        /// </summary>
        /// <param name="overdue">Total overdue time in minutes</param>
        /// <returns>slab price</returns>
        public decimal? GetCheckInSlabPrice(int overdue, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(overdue, sqlTransaction);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            decimal? slabPrice = productsDataHandler.GetCheckInSlabPrice(productsDTO.ProductId, overdue);
            log.LogMethodExit(slabPrice);
            return slabPrice;
        }

        /// <summary>
        /// GetRewardProductDTOList
        /// </summary>
        /// <param name="productsFilterParams"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetRewardProductDTOList(ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(productsFilterParams);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            log.LogMethodExit();
            return SetFromSiteTimeOffset(productsDataHandler.GetRewardProductDTOList(productsFilterParams));
        }
        /// <summary>
        /// GetProductDTOByDisplayGroupId
        /// </summary>
        /// <param name="displayGroupId"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetProductDTOByDisplayGroupId(int displayGroupId)
        {
            log.LogMethodEntry(displayGroupId);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            log.LogMethodExit();
            return SetFromSiteTimeOffset(productsDataHandler.GetProductDTOByDisplayGroupId(displayGroupId));
        }



        public double UpdateParentModifierPrice(PurchasedProducts products, ModifierSetDTO modifiersDTO)
        {
            log.LogMethodEntry(products, modifiersDTO);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            double price = productsDataHandler.UpdateParentModifierPrice(products, modifiersDTO);
            log.LogMethodExit(price);
            return price;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public int InsertUpdateProducts()
        //{
        //    // UserId and siteId should come from the context.
        //    int result = 0;
        //    string userId = executionContext.GetUserId();
        //    int siteId = executionContext.GetSiteId();
        //    try
        //    {
        //        log.Debug("Starts-InsertUpdateGames" + "() Method.");
        //        ProductsDataHandler productHandler = new ProductsDataHandler();
        //        SqlTransaction SQLTrx = null;
        //        if (producDto.ProductId > 0 && producDto.IsChanged == true)
        //        {
        //            result = productHandler.UpdateProducts(producDto, userId, siteId, SQLTrx);
        //            producDto.AcceptChanges();
        //        }
        //        else if (producDto.ProductId < 0)
        //        {
        //            result = productHandler.InsertProducts(producDto, userId, siteId, SQLTrx);
        //        }
        //        log.Debug("ends-InsertUpdateGames() Method.");
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return result;
        //}

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductsDTO GetProductsDTO { get { return productsDTO; } }

        public PurchasedProducts GetPurchasedProducts(int TrxLineIndex = -1)
        {
            log.LogMethodEntry(TrxLineIndex);
            // ProductModifiersBL productModifiersBL = new ProductModifiersBL(productsDTO.ProductId);
            PurchasedProducts purchasedProducts = new PurchasedProducts();
            purchasedProducts.ProductId = productsDTO.ProductId;
            purchasedProducts.ProductName = productsDTO.ProductName;
            purchasedProducts.MinimumQuantity = productsDTO.MinimumQuantity;
            purchasedProducts.Price = Convert.ToDouble(productsDTO.Price);
            purchasedProducts.ProductDescription = productsDTO.Description;
            purchasedProducts.ProductType = productsDTO.ProductType;
            purchasedProducts.ProductDisplayGroup = productsDTO.DisplayGroup;
            purchasedProducts.IsSelected = true;
            purchasedProducts.TrxLineId = TrxLineIndex;

            ProductModifiersList productModifiersList = new ProductModifiersList(executionContext);
            List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.PRODUCT_ID, purchasedProducts.ProductId.ToString()));
            List<ProductModifiersDTO> productModifiersDtoList = productModifiersList.GetAllProductModifiersList(searchParameters);

            if (productModifiersDtoList != null)
            {
                for (int i = 0; i < productModifiersDtoList.Count; i++)
                {
                    ModifierSetBL modifierSetBL = new ModifierSetBL(Convert.ToInt32(productModifiersDtoList[i].ModifierSetId), executionContext);
                    ModifierSetDTO modifierSetDTO = modifierSetBL.GetModifierSetDTO;
                    modifierSetBL.PurchasedModifierSet(modifierSetDTO);
                    purchasedProducts.PurchasedModifierSetDTOList.Add(modifierSetBL.GetPurchasedModifierSet);
                }
            }
            log.LogMethodExit(purchasedProducts);
            return purchasedProducts;
        }

        /// <summary>
        /// Constructor with the productId parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="productId">productId</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Products(ExecutionContext executionContext,
                         int productId,
                         bool loadChildRecords = true,
                         bool activeChildRecords = true,
                         SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, productId, loadChildRecords, activeChildRecords, sqlTransaction);
            this.executionContext = executionContext;
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            this.productsDTO = productsDataHandler.GetProductsDTO(productId);
            if (loadChildRecords == false ||
                productsDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            ProductList productList = new ProductList(executionContext);
            List<ProductDTO> productDTOList = productList.GetProductDTOListOfProducts(new List<int>() { productId }, loadChildRecords, activeChildRecords, sqlTransaction);
            if (productDTOList != null &&
                productDTOList.Any())
            {
                productsDTO.InventoryItemDTO = productDTOList[0];
            }
            ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext);
            productsDTO.ProductsDisplayGroupDTOList = productsDisplayGroupList.GetProductsDisplayGroupDTOOfProducts(new List<int>() { productId }, activeChildRecords, sqlTransaction);
            LoadProductSubscritionDTOChild(productId);
            if (productsDTO.CustomDataSetId > -1 && productsDTO.CustomDataSetDTO == null)
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, productsDTO.CustomDataSetId, true, true, sqlTransaction);
                productsDTO.CustomDataSetDTO = customDataSetBL.CustomDataSetDTO;
            }
            SetFromSiteTimeOffset();
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the Products
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(Dictionary<int, CustomAttributesDTO> customAttributesDTOMap, Dictionary<int, CustomAttributesDTO> productCustomAttributesDTOMap)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrors = new List<ValidationError>();
            if (productsDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(validationErrors, "Products not changed.");
                return validationErrors;
            }
            //ExecutionContext deploymentPlanUserContext = executionContext;
            //if (deploymentPlanUserContext == null)
            //{
            //    deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            //}
            if (string.IsNullOrWhiteSpace(productsDTO.ProductName))
            {
                ValidationError validationError = new ValidationError("Products", "ProductName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Product Name")));
                validationErrors.Add(validationError);
            }
            if (productsDTO.ProductTypeId < 0)
            {
                ValidationError validationError = new ValidationError("Products", "ProductTypeId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Product Type")));
                validationErrors.Add(validationError);
            }
            if (productsDTO.InventoryItemDTO != null)
            {
                ProductBL productBL = new ProductBL(executionContext, productsDTO.InventoryItemDTO);
                validationErrors.AddRange(productBL.Validate(customAttributesDTOMap));
            }
            if (productsDTO.ProductsDisplayGroupDTOList != null &&
                productsDTO.ProductsDisplayGroupDTOList.Any())
            {
                for (int i = 0; i < productsDTO.ProductsDisplayGroupDTOList.Count; i++)
                {
                    ProductsDisplayGroup productDisplayGroup = new ProductsDisplayGroup(executionContext, productsDTO.ProductsDisplayGroupDTOList[i]);
                    validationErrors.AddRange(productDisplayGroup.Validate());
                }
            }
            if (productsDTO.CustomDataSetDTO != null)
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, productsDTO.CustomDataSetDTO);
                validationErrors.AddRange(customDataSetBL.Validate(productCustomAttributesDTOMap));
            }

            if (productsDTO.ProductSubscriptionDTO != null)
            {
                ValidationError canAddSubScriptionCheckError = ProductTypeValidationsforSubscription();
                if (canAddSubScriptionCheckError != null)
                {
                    validationErrors.Add(canAddSubScriptionCheckError);
                }
                else
                {
                    ProductSubscriptionBL productSubscriptionBL = new ProductSubscriptionBL(executionContext, productsDTO.ProductSubscriptionDTO);
                    List<ValidationError> vErrorList = productSubscriptionBL.ValidateProductSubscription();
                    if (vErrorList != null && vErrorList.Any())
                    {
                        validationErrors.AddRange(vErrorList);
                    }
                }
            }

            if (productsDTO.MaximumQuantity.HasValue && productsDTO.MinimumQuantity >= 0)
            {
                if (productsDTO.MaximumQuantity < productsDTO.MinimumQuantity)
                {
                    ValidationError quantityValidationError = new ValidationError("Products", "MaximumQuantity", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Maximum Quantity")));
                    validationErrors.Add(quantityValidationError);
                }
                if (productsDTO.MaximumQuantity < 0)
                {
                    ValidationError quantityValidationError = new ValidationError("Products", "MaximumQuantity", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Maximum Quantity")));
                    validationErrors.Add(quantityValidationError);
                }

            }
            if (productsDTO.MinimumQuantity < 0)
            {
                ValidationError quantityValidationError = new ValidationError("Products", "MinimumQuantity", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Minimum Quantity")));
                validationErrors.Add(quantityValidationError);
            }
            log.LogMethodExit(validationErrors);
            return validationErrors;
        }

        /// <summary>
        /// Saves the Products
        /// </summary>
        public virtual void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productsDTO.InventoryItemDTO != null)
            {
                if (productsDTO.InventoryProductCode != productsDTO.InventoryItemDTO.Code)
                {
                    productsDTO.InventoryProductCode = productsDTO.InventoryItemDTO.Code;
                }
                if (productsDTO.InventoryItemDTO.CategoryId != productsDTO.CategoryId && productsDTO.CategoryId != -1)
                {
                    productsDTO.InventoryItemDTO.CategoryId = productsDTO.CategoryId;
                }
                else
                {
                    productsDTO.CategoryId = productsDTO.InventoryItemDTO.CategoryId;
                }
                if (productsDTO.InventoryItemDTO.ProductName != productsDTO.ProductName && !string.IsNullOrEmpty(productsDTO.ProductName))
                {
                    productsDTO.InventoryItemDTO.ProductName = productsDTO.ProductName;
                }
                else
                {
                    productsDTO.ProductName = productsDTO.InventoryItemDTO.ProductName;
                }
                if (productsDTO.InventoryItemDTO.Description != productsDTO.Description && !string.IsNullOrEmpty(productsDTO.Description))
                {
                    productsDTO.InventoryItemDTO.Description = productsDTO.Description;
                }
                else
                {
                    productsDTO.Description = productsDTO.InventoryItemDTO.Description;
                }
                if (productsDTO.InventoryItemDTO.IsActive != productsDTO.ActiveFlag && productsDTO.InventoryItemDTO.ProductId < 0)
                {
                    productsDTO.InventoryItemDTO.IsActive = productsDTO.ActiveFlag;
                }
                if ((decimal)productsDTO.InventoryItemDTO.SalePrice != productsDTO.Price)
                {
                    if (productsDTO.InventoryItemDTO.SalePrice != -1 && (productsDTO.Price == -1 || productsDTO.Price == 0))
                    {
                        productsDTO.Price = Convert.ToDecimal(productsDTO.InventoryItemDTO.SalePrice);
                    }
                    if (productsDTO.Price == -1)
                    {
                        productsDTO.InventoryItemDTO.SalePrice = 0;
                    }
                    else
                    {
                        productsDTO.InventoryItemDTO.SalePrice = (double)productsDTO.Price;
                    }
                }
                if (productsDTO.ProductTypeId == -1)
                {
                    ProductTypeListBL productTypeListBL = new ProductTypeListBL(executionContext);
                    List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>
                    {
                            new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, "MANUAL"),
                            new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())};
                    List<ProductTypeDTO> listProductTypeDTOs = productTypeListBL.GetProductTypeDTOList(searchParameters);
                    if (listProductTypeDTOs != null && listProductTypeDTOs.Count > 0)
                    {
                        productsDTO.ProductTypeId = listProductTypeDTOs[0].ProductTypeId;
                    }
                }
                if(string.IsNullOrEmpty(productsDTO.DisplayInPOS))
                {
                    productsDTO.DisplayInPOS = "N";
                }
            }
            if (productsDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Products not changed.");
                return;
            }
            Dictionary<int, CustomAttributesDTO> customAttributesDTOMap = null;
            Dictionary<int, CustomAttributesDTO> productCustomAttributesDTOMap = null;
            if (productsDTO.CustomDataSetId > -1 || productsDTO.CustomDataSetDTO != null)
            {
                CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
                productCustomAttributesDTOMap = customAttributesListBL.GetCustomAttributesDTOMap(Applicability.PRODUCT);
            }
            if (productsDTO.InventoryItemDTO != null)
            {
                CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
                customAttributesDTOMap = customAttributesListBL.GetCustomAttributesDTOMap(Applicability.INVPRODUCT);
            }
            List<ValidationError> validationErrors = Validate(customAttributesDTOMap, productCustomAttributesDTOMap);
            if (validationErrors.Count > 0)
            {
                log.LogMethodExit(null, "Validation failed : " + string.Join(", ", validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation failed.", validationErrors);
            }
            if (productsDTO.CustomDataSetDTO != null &&
                productsDTO.CustomDataSetDTO.IsChangedRecursive)
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, productsDTO.CustomDataSetDTO);
                customDataSetBL.Save(sqlTransaction);
                if (productsDTO.CustomDataSetId != productsDTO.CustomDataSetDTO.CustomDataSetId)
                {
                    productsDTO.CustomDataSetId = productsDTO.CustomDataSetDTO.CustomDataSetId;
                }
            }
            if (productsDTO.IsChanged)
            {
                SetToSiteTimeOffset();
                ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
                productsDataHandler.Save(productsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (productsDTO.ProductsDisplayGroupDTOList != null &&
                    productsDTO.ProductsDisplayGroupDTOList.Any())
                {
                    for (int i = 0; i < productsDTO.ProductsDisplayGroupDTOList.Count; i++)
                    {
                        ProductsDisplayGroupDTO productsDisplayGroupDTO = productsDTO.ProductsDisplayGroupDTOList[i];
                        if (productsDisplayGroupDTO.ProductId != productsDTO.ProductId)
                        {
                            productsDisplayGroupDTO.ProductId = productsDTO.ProductId;
                        }
                    }
                }
                if (productsDTO.InventoryItemDTO != null &&
                    productsDTO.InventoryItemDTO.ManualProductId != productsDTO.ProductId)
                {
                    productsDTO.InventoryItemDTO.ManualProductId = productsDTO.ProductId;
                }
            }
            if (productsDTO.InventoryItemDTO != null &&
                productsDTO.InventoryItemDTO.IsChangedRecursive)
            {
                if (productsDTO.InventoryItemDTO.ManualProductId != productsDTO.ProductId)
                {
                    productsDTO.InventoryItemDTO.ManualProductId = productsDTO.ProductId;
                }
                ProductBL inventoryItemBL = new ProductBL(executionContext, productsDTO.InventoryItemDTO);
                inventoryItemBL.Save(sqlTransaction);
            }
            List<ProductsDisplayGroupDTO> updatedProductDisplayGroupDTOList = new List<ProductsDisplayGroupDTO>();
            if (productsDTO.ProductsDisplayGroupDTOList != null &&
                productsDTO.ProductsDisplayGroupDTOList.Any())
            {
                for (int j = 0; j < productsDTO.ProductsDisplayGroupDTOList.Count; j++)
                {
                    ProductsDisplayGroupDTO productsDisplayGroupDTO = productsDTO.ProductsDisplayGroupDTOList[j];
                    if (productsDisplayGroupDTO.ProductId != productsDTO.ProductId)
                    {
                        productsDisplayGroupDTO.ProductId = productsDTO.ProductId;
                    }
                    if (productsDisplayGroupDTO.IsChanged)
                    {
                        updatedProductDisplayGroupDTOList.Add(productsDisplayGroupDTO);
                    }
                }
            }
            if (updatedProductDisplayGroupDTOList.Any())
            {
                ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext, updatedProductDisplayGroupDTOList);
                productsDisplayGroupList.Save(sqlTransaction);
            }
            if (productsDTO.InventoryItemDTO != null)
            {
                AssignDisplayGroup(sqlTransaction);
            }
            if (productsDTO != null && productsDTO.ProductSubscriptionDTO != null
                && (productsDTO.ProductSubscriptionDTO.IsChanged || productsDTO.ProductSubscriptionDTO.ProductSubscriptionId == -1))
            {
                ProductSubscriptionBL productSubscriptionBL = new ProductSubscriptionBL(executionContext, productsDTO.ProductSubscriptionDTO);
                productSubscriptionBL.Save(sqlTransaction);
            }
            SetFromSiteTimeOffset();
            log.LogMethodExit();
        }

        /// <summary>
        /// Hard Deletions for Products
        /// </summary>
        /// <param name="productsId"></param>
        /// <param name="sqlTransaction"></param>
        public void DeleteProducts(int productsId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productsId);
            try
            {
                ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
                productsDataHandler.DeleteProducts(productsId);
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        private void AssignDisplayGroup(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            CheckAndAssignDisplayGroupForProduct(sqlTransaction);
            log.LogMethodExit();
        }

        private void CheckAndAssignDisplayGroupForProduct(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            int displayGroupId = -1;
            List<ProductDisplayGroupFormatDTO> listProductDisplayGroupFormatDTO = new List<ProductDisplayGroupFormatDTO>();
            if (string.IsNullOrEmpty(productsDTO.MapedDisplayGroup) == false)
            {
                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
                List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                searchParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP, productsDTO.MapedDisplayGroup.ToString()));
                searchParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                listProductDisplayGroupFormatDTO = productDisplayGroupList.GetAllProductDisplayGroup(searchParams, false, false, sqlTransaction);
                if (listProductDisplayGroupFormatDTO != null && listProductDisplayGroupFormatDTO.Count > 0)
                {
                    displayGroupId = listProductDisplayGroupFormatDTO[0].Id;
                }
                else
                {
                    throw new ValidationException("Display Group is not defined" + productsDTO.MapedDisplayGroup.ToString());
                }
            }
            ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext);
            List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> productDisplayGroupSearchParams = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>
                {
                    new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, productsDTO.ProductId.ToString()),
                    new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };

            if (displayGroupId > -1)
            {
                productDisplayGroupSearchParams.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, displayGroupId.ToString()));
            }
            List<ProductsDisplayGroupDTO> listProductsDisplayGroupDTOs = productsDisplayGroupList.GetAllProductsDisplayGroup(productDisplayGroupSearchParams, sqlTransaction);
            if (listProductsDisplayGroupDTOs == null || listProductsDisplayGroupDTOs.Count == 0)
            {
                SetupDisplayGroup(displayGroupId, sqlTransaction);
                log.LogMethodExit();
            }
        }

        private void SetupDisplayGroup(int displayGroupId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(displayGroupId, sqlTransaction);
            if (displayGroupId == -1)
            {
                List<ProductDisplayGroupFormatDTO> listProductDisplayGroupFormatDTO = new List<ProductDisplayGroupFormatDTO>();
                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
                List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                searchParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP, "Parafait Inventory Products"));
                searchParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                listProductDisplayGroupFormatDTO = productDisplayGroupList.GetAllProductDisplayGroup(searchParams, false, false, sqlTransaction);
                if (listProductDisplayGroupFormatDTO != null && listProductDisplayGroupFormatDTO.Count > 0)
                {
                    displayGroupId = listProductDisplayGroupFormatDTO[0].Id;
                }
            }
            ProductsDisplayGroupDTO productsDisplayGroupDTO = new ProductsDisplayGroupDTO();
            productsDisplayGroupDTO.DisplayGroupId = displayGroupId;
            productsDisplayGroupDTO.ProductId = productsDTO.ProductId;
            ProductsDisplayGroup productsDisplayGroup = new ProductsDisplayGroup(executionContext, productsDisplayGroupDTO);
            productsDisplayGroup.Save(sqlTransaction);
            log.LogMethodExit();
        }


        public List<ComboProductDTO> GetComboProductSetup(bool getActiveRecords)
        {
            log.LogMethodEntry(getActiveRecords);
            List<ComboProductDTO> comboProductDTOList = null;
            if (this.productsDTO != null)
            {
                ComboProductList comboProductList = new ComboProductList(executionContext);
                List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, this.productsDTO.ProductId.ToString()));
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (getActiveRecords)
                {
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                comboProductDTOList = comboProductList.GetComboProductDTOList(searchParameters);
                if (comboProductDTOList != null && comboProductDTOList.Count > 1)
                {
                    comboProductDTOList = comboProductDTOList.OrderBy(cp => (cp.SortOrder == null ? 0 : cp.SortOrder)).ToList();
                }
                this.productsDTO.ComboProductDTOList = comboProductDTOList;
            }
            log.LogMethodExit(comboProductDTOList);
            return comboProductDTOList;
        }

        public List<ComboProductDTO> GetComboPackageProductSetup(bool getActiveRecords)
        {
            log.LogMethodEntry(getActiveRecords);
            List<ComboProductDTO> comboProductDTOList = null;
            if (this.productsDTO != null)
            {
                if (this.productsDTO.ComboProductDTOList != null)
                {
                    comboProductDTOList = this.productsDTO.ComboProductDTOList.Where(cmb => cmb.AdditionalProduct == false).ToList();
                }
                if (comboProductDTOList == null || comboProductDTOList.Count == 0)
                {
                    ComboProductList comboProductList = new ComboProductList(executionContext);
                    List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, this.productsDTO.ProductId.ToString()));
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.ADDITIONAL_PRODUCT, "0"));
                    if (getActiveRecords)
                    {
                        searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    comboProductDTOList = comboProductList.GetComboProductDTOList(searchParameters);
                    if (comboProductDTOList != null && comboProductDTOList.Count > 1)
                    {
                        comboProductDTOList = comboProductDTOList.OrderBy(cp => (cp.SortOrder == null ? 0 : cp.SortOrder)).ToList();
                    }
                }
            }
            log.LogMethodExit(comboProductDTOList);
            return comboProductDTOList;
        }

        public List<ComboProductDTO> GetComboAdditionalProductSetup(bool getActiveRecords)
        {
            log.LogMethodEntry(getActiveRecords);
            List<ComboProductDTO> comboProductDTOList = null;
            if (this.productsDTO != null)
            {
                if (this.productsDTO.ComboProductDTOList != null)
                {
                    comboProductDTOList = this.productsDTO.ComboProductDTOList.Where(cmb => cmb.AdditionalProduct == true).ToList();
                    if (comboProductDTOList != null && comboProductDTOList.Any() && getActiveRecords)
                    {
                        comboProductDTOList = comboProductDTOList.Where(cmb => cmb.IsActive == true).ToList();
                    }
                }
                if (comboProductDTOList == null || comboProductDTOList.Count == 0)
                {
                    ComboProductList comboProductList = new ComboProductList(executionContext);
                    List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, this.productsDTO.ProductId.ToString()));
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.ADDITIONAL_PRODUCT, "1"));
                    if (getActiveRecords)
                    {
                        searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    comboProductDTOList = comboProductList.GetComboProductDTOList(searchParameters);
                }
                if (comboProductDTOList != null && comboProductDTOList.Count > 1)
                {
                    comboProductDTOList = comboProductDTOList.OrderBy(cp => (cp.SortOrder == null ? 0 : cp.SortOrder)).ToList();
                }
            }
            log.LogMethodExit(comboProductDTOList);
            return comboProductDTOList;
        }

        /// <summary>
        /// GetProductPackageContents method
        /// </summary>
        /// <param name="comboProductId">comboProductId</param>
        /// <returns>returns List of ProductContent</returns>
        public List<BookingProductContent> GetProductPackageContents(bool getActiveRecords)
        {
            log.LogMethodEntry(getActiveRecords);
            List<BookingProductContent> productContentList = null;
            if (this.productsDTO != null && this.productsDTO.ProductId > -1)
            {
                ProductsDataHandler productsDataHandler = new ProductsDataHandler();
                productContentList = productsDataHandler.GetPackageContents(productsDTO.ProductId, getActiveRecords);
            }
            log.LogMethodExit(productContentList);
            return productContentList;
        }

        /// <summary>
        /// GetBookingProductPackageContents method
        /// </summary> 
        /// <returns>returns booking package product content</returns>
        public List<BookingProductContent> GetBookingProductPackageContents(bool getActiveRecords)
        {
            log.LogMethodEntry(getActiveRecords);
            List<BookingProductContent> bookingProductContentList = null;
            if (this.productsDTO != null && this.productsDTO.ProductId > -1)
            {
                ProductsDataHandler productsDataHandler = new ProductsDataHandler();
                bookingProductContentList = productsDataHandler.GetBookingProductContents(productsDTO.ProductId, getActiveRecords);
            }
            log.LogMethodExit(bookingProductContentList);
            return bookingProductContentList;
        }

        /// <summary>
        /// GetBookingAdditionalProducts method
        /// </summary> 
        /// <returns>returns additional product content for booking product</returns>
        public List<AdditionalProduct> GetBookingAdditionalProducts(bool getActiveRecords)
        {
            log.LogMethodEntry(getActiveRecords);
            List<AdditionalProduct> bookingAdditionalProdContentList = null;
            if (this.productsDTO != null && this.productsDTO.ProductId > -1)
            {
                ProductsDataHandler productsDataHandler = new ProductsDataHandler();
                bookingAdditionalProdContentList = productsDataHandler.GetAdditionalProducts(productsDTO.ProductId, getActiveRecords);
            }
            log.LogMethodExit(bookingAdditionalProdContentList);
            return bookingAdditionalProdContentList;
        }

        /// <summary>
        /// GetBookingProductContent
        /// </summary>
        /// <returns></returns>
        public BookingProduct GetBookingProductContent(bool getActiveRecords)
        {
            log.LogMethodEntry(getActiveRecords);
            BookingProduct bookingProduct = null;
            if (this.productsDTO != null)
            {
                bookingProduct = new BookingProduct(
                                                   productsDTO.ProductId,
                                                   productsDTO.ProductName,
                                                    productsDTO.Description,
                                                    productsDTO.ImageFileName,
                                                    (double)productsDTO.Price,
                                                    (double)productsDTO.AdvanceAmount,
                                                    productsDTO.MinimumQuantity,
                                                    productsDTO.MinimumTime,
                                                    productsDTO.MaximumTime,
                                                    0,
                                                    productsDTO.AvailableUnits,
                                                    (int)productsDTO.SortOrder,
                                                    (double)productsDTO.AdvancePercentage,
                                                    productsDTO.WebDescription,
                                                    null
                                                   );

                ProductsDataHandler productsDataHandler = new ProductsDataHandler();
                List<BookingProductContent> bpcList = new List<BookingProductContent>();
                bpcList = productsDataHandler.GetBookingProductContents(productsDTO.ProductId, getActiveRecords);

                List<BookingPackageProduct> bookingPackageProductList = new List<BookingPackageProduct>();
                foreach (BookingProductContent bpc in bpcList)
                {
                    BookingPackageProduct bpp = new BookingPackageProduct(bpc, productsDataHandler.GetPackageContents(bpc.ProductId, getActiveRecords));
                    bpp.CategoryId = bpc.CategoryId;
                    bpp.Price = bpc.Price;
                    bpp.PriceInclusive = bpc.PriceInclusive;
                    bpp.ProductDescription = bpc.ProductDescription;
                    bpp.ProductDisplayGroup = bpc.ProductDisplayGroup;
                    bpp.ProductId = bpc.ProductId;
                    bpp.ProductImage = bpc.ProductImage;
                    bpp.ProductName = bpc.ProductName;
                    bpp.ProductType = bpc.ProductType;
                    bpp.Quantity = bpc.Quantity;

                    bookingPackageProductList.Add(bpp);
                }

                bookingProduct.BookingProductPackagelist = bookingPackageProductList;

            }
            return bookingProduct;
        }

        /// <summary>
        /// GetUpsellProductId method
        /// </summary> 
        /// <returns>UpsellProductId</returns>
        public int GetUpsellProductId(int productId)
        {
            log.LogMethodEntry();
            int upsellProductId = -1;
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            upsellProductId = productsDataHandler.GetUpsellProductId(productId);
            log.LogMethodExit(upsellProductId);
            return upsellProductId;
        }

        /// <summary>
        /// GetModifierSet method
        /// </summary> 
        /// <returns>productsModifierSetStruct</returns>
        public ProductsModifierSetStruct GetModifierSet(int modifierSetId)
        {
            log.LogMethodEntry();
            ProductsModifierSetStruct productsModifierSetStruct = null;
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            productsModifierSetStruct = productsDataHandler.GetModifierSet(modifierSetId);
            log.LogMethodExit(productsModifierSetStruct);
            return productsModifierSetStruct;
        }

        public bool AdvanceAmountRequired()
        {
            log.LogMethodEntry();
            bool advAmountRequired = false;
            log.LogVariableState("productsDTO", this.productsDTO);
            advAmountRequired = (productsDTO != null && (productsDTO.AdvanceAmount > 0 || productsDTO.AdvancePercentage > 0));
            log.LogMethodExit(advAmountRequired);
            return advAmountRequired;
        }
        /// <summary>
        /// Duplicates based on the product type and Id
        /// Duplicating Product Details
        /// </summary>        
        /// <returns>resultId</returns>
        public int SaveDuplicateProductDetails(int duplicateProductId)
        {
            log.LogMethodEntry(duplicateProductId);
            this.duplicateProductId = duplicateProductId;
            ProductsDTO productsDTO = new ProductsDTO();

            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, Convert.ToString(duplicateProductId)));
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            ProductsList productList = new ProductsList(executionContext);
            List<ProductsDTO> productsDTOList = productList.GetProductsDTOList(searchParameters, false);
            productsDTO = productsDTOList.FindAll(m => m.ProductId == duplicateProductId).SingleOrDefault();
            productsDTO.ProductId = -1;
            productsDTO.Guid = string.Empty;
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    this.sqlTransaction = parafaitDBTrx.SQLTrx;
                    ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
                    if (productsDTO.ProductId < 0 && duplicateProductId >= 0)
                    {
                        productsDataHandler.Save(productsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        productsDTO.AcceptChanges();
                        newProductId = productsDTO.ProductId;
                        UpdateDuplicateProductDetails();
                        SaveDuplicateProductsDisplayGroup();
                        SaveDuplicateProductDiscounts();
                        SaveDuplicateProductCalendar();
                        SaveDuplicateComboProduct();
                        SaveDuplicateProductSpecialPricing();
                        SaveDuplicateUpsellOffers();
                        SaveDuplicateProductModifiers();
                        SaveDuplicateAllowedFacilityMap();
                        SaveDuplicateProductCreditPlus();
                        SaveDuplicateProductGames();
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (ValidationException valEx)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(valEx.Message);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex.Message);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }
            }
            log.LogMethodExit(productsDTO.ProductId);
            return productsDTO.ProductId;
        }
        /// <summary>
        /// Saves ProductGames and ProductGamesExtended for duplicate productDetails
        /// </summary>        
        private void SaveDuplicateProductGames()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> searchParameters = new List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID, duplicateProductId.ToString()));

                ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext);
                List<ProductGamesDTO> productGamesDTOList = productGamesListBL.GetProductGamesDTOList(searchParameters, sqlTransaction);
                if (productGamesDTOList != null)
                {
                    foreach (ProductGamesDTO productGamesDTO in productGamesDTOList)
                    {
                        ProductGamesDTO duplicateProductGamesDTO = new ProductGamesDTO(-1, newProductId, productGamesDTO.Game_id, productGamesDTO.Quantity, productGamesDTO.ValidFor, productGamesDTO.ExpiryDate, productGamesDTO.ValidMinutesDays,
                                                                        productGamesDTO.Game_profile_id, productGamesDTO.Frequency, string.Empty, productGamesDTO.Site_id, false, productGamesDTO.CardTypeId, productGamesDTO.EntitlementType, productGamesDTO.OptionalAttribute,
                                                                        productGamesDTO.ExpiryTime, productGamesDTO.CustomDataSetId, productGamesDTO.TicketAllowed, productGamesDTO.EffectiveAfterDays, productGamesDTO.FromDate, -1, productGamesDTO.Monday,
                                                                        productGamesDTO.Tuesday, productGamesDTO.Wednesday, productGamesDTO.Thursday, productGamesDTO.Friday, productGamesDTO.Saturday, productGamesDTO.Sunday, productGamesDTO.CreatedBy,
                                                                        productGamesDTO.CreationDate, productGamesDTO.LastUpdatedBy, DateTime.MinValue, productGamesDTO.ISActive);
                        if (productGamesDTO.ProductGamesExtendedDTOList != null)
                        {
                            foreach (ProductGamesExtendedDTO productGamesExtendedDTO in productGamesDTO.ProductGamesExtendedDTOList)
                            {
                                ProductGamesExtendedDTO duplicateProductGamesExtendedDTO = new ProductGamesExtendedDTO(-1, productGamesExtendedDTO.ProductGameId, productGamesExtendedDTO.GameId, productGamesExtendedDTO.GameProfileId, productGamesExtendedDTO.Exclude, productGamesExtendedDTO.Site_id, string.Empty,
                                                                                                false, -1, productGamesExtendedDTO.CreatedBy, DateTime.MinValue, productGamesExtendedDTO.LastUpdatedBy, DateTime.MinValue, productGamesExtendedDTO.PlayLimitPerGame, productGamesExtendedDTO.ISActive);
                                duplicateProductGamesDTO.ProductGamesExtendedDTOList.Add(duplicateProductGamesExtendedDTO);
                            }
                        }
                        ProductGamesBL productGamesBL = new ProductGamesBL(executionContext, duplicateProductGamesDTO);
                        productGamesBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves ProductCreditPlus for duplicate productDetails
        /// </summary>
        private void SaveDuplicateProductCreditPlus()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>(ProductCreditPlusDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>(ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID, duplicateProductId.ToString()));

                ProductCreditPlusBLList productCreditPlusBLList = new ProductCreditPlusBLList(executionContext);
                List<ProductCreditPlusDTO> productCreditPlusDTOList = productCreditPlusBLList.GetAllProductCreditPlusListDTOList(searchParameters, true, sqlTransaction);
                if (productCreditPlusDTOList != null)
                {
                    foreach (ProductCreditPlusDTO productCreditPlusDTO in productCreditPlusDTOList)
                    {
                        ProductCreditPlusDTO duplicateProductCreditPlusDTO = new ProductCreditPlusDTO(-1, productCreditPlusDTO.CreditPlus, productCreditPlusDTO.Refundable, productCreditPlusDTO.Remarks,
                                                                                newProductId, productCreditPlusDTO.CreditPlusType, string.Empty, productCreditPlusDTO.Site_id, false, productCreditPlusDTO.PeriodFrom,
                                                                                productCreditPlusDTO.PeriodTo, productCreditPlusDTO.ValidForDays, productCreditPlusDTO.ExtendOnReload, productCreditPlusDTO.TimeFrom,
                                                                                productCreditPlusDTO.TimeTo, productCreditPlusDTO.Minutes, productCreditPlusDTO.Monday, productCreditPlusDTO.Tuesday, productCreditPlusDTO.Wednesday, productCreditPlusDTO.Thursday,
                                                                                productCreditPlusDTO.Friday, productCreditPlusDTO.Saturday, productCreditPlusDTO.Sunday, productCreditPlusDTO.TicketAllowed, -1, productCreditPlusDTO.Frequency, productCreditPlusDTO.PauseAllowed,
                                                                                productCreditPlusDTO.CreatedBy, DateTime.MinValue, productCreditPlusDTO.LastUpdatedBy, DateTime.MinValue, productCreditPlusDTO.IsActive);

                        if (productCreditPlusDTO.CreditPlusConsumptionRulesList != null)
                        {
                            foreach (CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO in productCreditPlusDTO.CreditPlusConsumptionRulesList)
                            {
                                CreditPlusConsumptionRulesDTO duplicateCreditPlusConsumptionRulesDTO = new CreditPlusConsumptionRulesDTO(-1, creditPlusConsumptionRulesDTO.ProductCreditPlusId, creditPlusConsumptionRulesDTO.POSTypeId, creditPlusConsumptionRulesDTO.ExpiryDate, string.Empty,
                                                                                                            creditPlusConsumptionRulesDTO.SiteId, false, creditPlusConsumptionRulesDTO.GameId, creditPlusConsumptionRulesDTO.GameProfileId, newProductId, creditPlusConsumptionRulesDTO.Quantity,
                                                                                                            creditPlusConsumptionRulesDTO.QuantityLimit, -1, creditPlusConsumptionRulesDTO.CategoryId, creditPlusConsumptionRulesDTO.DiscountAmount, creditPlusConsumptionRulesDTO.DiscountPercentage,
                                                                                                            creditPlusConsumptionRulesDTO.DiscountedPrice, creditPlusConsumptionRulesDTO.OrderTypeId, creditPlusConsumptionRulesDTO.CreatedBy, DateTime.MinValue, creditPlusConsumptionRulesDTO.LastUpdatedBy,
                                                                                                            DateTime.MinValue, creditPlusConsumptionRulesDTO.IsActive);
                                duplicateProductCreditPlusDTO.CreditPlusConsumptionRulesList.Add(duplicateCreditPlusConsumptionRulesDTO);
                            }
                        }

                        ProductCreditPlusBL productCreditPlusBL = new ProductCreditPlusBL(executionContext, duplicateProductCreditPlusDTO);
                        productCreditPlusBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves AllowedFacilityMap for duplicate productDetails
        /// </summary>       
        private void SaveDuplicateAllowedFacilityMap()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID, duplicateProductId.ToString()));
                searchParameters.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
                ProductsAllowedInFacilityMapListBL productsAllowedInFacilityMapListBL = new ProductsAllowedInFacilityMapListBL(executionContext);
                List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList = productsAllowedInFacilityMapListBL.GetProductsAllowedInFacilityMapDTOList(searchParameters);
                if (productsAllowedInFacilityMapDTOList != null && productsAllowedInFacilityMapDTOList.Count > 0)
                {
                    List<ProductsAllowedInFacilityMapDTO> duplicateProductsAllowedInFacilityMapDTOList = new List<ProductsAllowedInFacilityMapDTO>();
                    foreach (ProductsAllowedInFacilityMapDTO productsAllowedInFacilityMapDTO in productsAllowedInFacilityMapDTOList)
                    {
                        ProductsAllowedInFacilityMapDTO duplicateProductsAllowedInFacilityMapDTO = new ProductsAllowedInFacilityMapDTO(-1, productsAllowedInFacilityMapDTO.FacilityMapId, productsAllowedInFacilityMapDTO.FacilityMapName, newProductId, productsAllowedInFacilityMapDTO.ProductType, false,
                                                                                            productsAllowedInFacilityMapDTO.IsActive, string.Empty, productsAllowedInFacilityMapDTO.CreatedBy, DateTime.MinValue, productsAllowedInFacilityMapDTO.LastUpdatedBy, DateTime.MinValue, productsAllowedInFacilityMapDTO.SiteId, false, -1);
                        duplicateProductsAllowedInFacilityMapDTOList.Add(duplicateProductsAllowedInFacilityMapDTO);
                    }
                    ProductsAllowedInFacilityMapListBL duplicateProductsAllowedInFacilityMapListBL = new ProductsAllowedInFacilityMapListBL(executionContext, duplicateProductsAllowedInFacilityMapDTOList);
                    duplicateProductsAllowedInFacilityMapListBL.Save(sqlTransaction);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves ProductModifiers for duplicate productDetails
        /// </summary>        
        private void SaveDuplicateProductModifiers()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.PRODUCT_ID, duplicateProductId.ToString()));

                ProductModifiersList productModifiersList = new ProductModifiersList(executionContext);
                List<ProductModifiersDTO> productModifiersDTOList = productModifiersList.GetAllProductModifiersList(searchParameters, sqlTransaction);
                if (productModifiersDTOList != null)
                {
                    foreach (ProductModifiersDTO productModifiersDTO in productModifiersDTOList)
                    {
                        ProductModifiersDTO duplicateProductModifiersDTO = new ProductModifiersDTO(-1, productModifiersDTO.CategoryId, newProductId, productModifiersDTO.ModifierSetId, productModifiersDTO.AutoShowinPOS,
                                                                               productModifiersDTO.SortOrder, productModifiersDTO.IsActive, DateTime.MinValue, productModifiersDTO.CreatedBy, DateTime.MinValue, productModifiersDTO.LastUpdatedBy,
                                                                               productModifiersDTO.Site_Id, string.Empty, false, -1);
                        ProductModifiersBL productModifiersBL = new ProductModifiersBL(executionContext, duplicateProductModifiersDTO);
                        productModifiersBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves UpsellOffers for duplicate productDetails
        /// </summary>
        private void SaveDuplicateUpsellOffers()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>> searchParameters = new List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>>();
                searchParameters.Add(new KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>(UpsellOffersDTO.SearchByUpsellOffersParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>(UpsellOffersDTO.SearchByUpsellOffersParameters.PRODUCT_ID, duplicateProductId.ToString()));

                UpsellOffersList upsellOffersList = new UpsellOffersList(executionContext);
                List<UpsellOffersDTO> upsellOffersDTOList = upsellOffersList.GetAllUpsellOffers(searchParameters, sqlTransaction);
                if (upsellOffersDTOList != null)
                {
                    foreach (UpsellOffersDTO upsellOffersDTO in upsellOffersDTOList)
                    {
                        UpsellOffersDTO duplicateUpsellOffersDTO = new UpsellOffersDTO(-1, newProductId, upsellOffersDTO.OfferProductId, upsellOffersDTO.OfferMessage, upsellOffersDTO.EffectiveDate,
                                                                            upsellOffersDTO.CreatedBy, upsellOffersDTO.IsActive, upsellOffersDTO.SiteId, false, string.Empty, upsellOffersDTO.LastUpdatedBy, DateTime.MinValue, -1,
                                                                            upsellOffersDTO.SaleGroupId);
                        UpsellOffer upsellOfferBL = new UpsellOffer(executionContext, duplicateUpsellOffersDTO);
                        upsellOfferBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves ProductSpecialPricing for duplicate productDetails
        /// </summary>
        private void SaveDuplicateProductSpecialPricing()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>> searchParameters = new List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRODUCT_ID, duplicateProductId.ToString()));

                ProductsSpecialPricingListBL productsSpecialPricingListBL = new ProductsSpecialPricingListBL(executionContext);
                List<ProductsSpecialPricingDTO> productsSpecialPricingDTOList = productsSpecialPricingListBL.GetAllProductsSpecialPricing(searchParameters, sqlTransaction);
                if (productsSpecialPricingDTOList != null)
                {
                    foreach (ProductsSpecialPricingDTO productsSpecialPricingDTO in productsSpecialPricingDTOList)
                    {
                        ProductsSpecialPricingDTO duplicateProductsSpecialPricingDTO = new ProductsSpecialPricingDTO(-1, newProductId, productsSpecialPricingDTO.PricingId, productsSpecialPricingDTO.Price,
                                                                                        productsSpecialPricingDTO.ActiveFlag, string.Empty, productsSpecialPricingDTO.Site_id, false, -1, productsSpecialPricingDTO.CreatedBy,
                                                                                        DateTime.MinValue, productsSpecialPricingDTO.LastUpdatedBy, DateTime.MinValue);
                        ProductsSpecialPricingBL productsSpecialPricingBL = new ProductsSpecialPricingBL(executionContext, duplicateProductsSpecialPricingDTO);
                        productsSpecialPricingBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves ComboProduct for duplicate productDetails
        /// </summary>
        private void SaveDuplicateComboProduct()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, duplicateProductId.ToString()));


                ComboProductList comboProductListBL = new ComboProductList(executionContext);
                List<ComboProductDTO> comboProductDTOList = comboProductListBL.GetComboProductDTOList(searchParameters, sqlTransaction);
                if (comboProductDTOList != null)
                {
                    foreach (ComboProductDTO comboProductDTO in comboProductDTOList)
                    {
                        ComboProductDTO duplicateComboProductDTO = new ComboProductDTO(-1, newProductId, comboProductDTO.ChildProductId, comboProductDTO.Quantity, comboProductDTO.CategoryId,
                                                                        comboProductDTO.DisplayGroup, comboProductDTO.PriceInclusive, comboProductDTO.AdditionalProduct, comboProductDTO.DisplayGroupId, comboProductDTO.Price,
                                                                        comboProductDTO.SortOrder, comboProductDTO.SiteId, string.Empty, false, -1, comboProductDTO.CreatedBy, comboProductDTO.CreationDate, comboProductDTO.LastUpdatedBy, comboProductDTO.LastUpdateDate,
                                                                        string.Empty, string.Empty, string.Empty, comboProductDTO.IsActive, string.Empty, comboProductDTO.MaximumQuantity);
                        ComboProductBL comboProductBL = new ComboProductBL(executionContext, duplicateComboProductDTO);
                        comboProductBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves productCalender for duplicate productDetails
        /// </summary>
        private void SaveDuplicateProductCalendar()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>(ProductsCalenderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>(ProductsCalenderDTO.SearchByParameters.PRODUCT_ID, duplicateProductId.ToString()));

                ProductsCalenderList productsCalenderList = new ProductsCalenderList(executionContext);
                List<ProductsCalenderDTO> productsCalenderDTOList = productsCalenderList.GetAllProductCalenderList(searchParameters, sqlTransaction);
                if (productsCalenderDTOList != null)
                {
                    foreach (ProductsCalenderDTO productsCalenderDTO in productsCalenderDTOList)
                    {
                        ProductsCalenderDTO duplicateProductsCalenderDTO = new ProductsCalenderDTO(-1, newProductId, productsCalenderDTO.Day, productsCalenderDTO.Date,
                                                                                                productsCalenderDTO.FromTime, productsCalenderDTO.ToTime, productsCalenderDTO.ShowHide, productsCalenderDTO.Site_id, string.Empty, false, -1);
                        ProductsCalender productsCalenderBL = new ProductsCalender(executionContext, duplicateProductsCalenderDTO);
                        productsCalenderBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves productDiscounts for duplicate productDetails
        /// </summary>        
        private void SaveDuplicateProductDiscounts()
        {
            log.LogMethodEntry();
            try
            {
                ProductDiscountsListBL productDiscountsListBL = new ProductDiscountsListBL(executionContext);
                List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> searchProductDiscountsParams = new List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>>();
                searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.PRODUCT_ID, duplicateProductId.ToString()));
                searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.IS_ACTIVE, "Y"));
                List<ProductDiscountsDTO> productDiscountsDTOList = productDiscountsListBL.GetProductDiscountsDTOList(searchProductDiscountsParams, sqlTransaction);
                if (productDiscountsDTOList != null)
                {
                    foreach (var productDiscountsDTO in productDiscountsDTOList)
                    {
                        ProductDiscountsDTO duplicateProductDiscountsDTO = new ProductDiscountsDTO(-1, newProductId, productDiscountsDTO.DiscountId, productDiscountsDTO.ExpiryDate, productDiscountsDTO.CreatedBy, productDiscountsDTO.CreationDate, productDiscountsDTO.LastUpdatedUser, productDiscountsDTO.LastUpdatedDate, null,
                                                                                                    productDiscountsDTO.ValidFor, productDiscountsDTO.ValidForDaysMonths, productDiscountsDTO.IsActive, productDiscountsDTO.SiteId, -1, false, string.Empty);
                        ProductDiscountsBL productDiscountsBL = new ProductDiscountsBL(executionContext, duplicateProductDiscountsDTO);
                        productDiscountsBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves displayGroup for duplicate productDetails
        /// </summary>        
        private void SaveDuplicateProductsDisplayGroup()
        {
            log.LogMethodEntry();
            try
            {
                ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext);
                List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParameters = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, duplicateProductId.ToString()));

                List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList = productsDisplayGroupList.GetAllProductsDisplayGroup(searchParameters, sqlTransaction);
                bool isExistDisplayGroup = false;
                if (productsDisplayGroupDTOList != null)
                {
                    foreach (ProductsDisplayGroupDTO productsDisplayGroupDTO in productsDisplayGroupDTOList)
                    {
                        ProductsDisplayGroupDTO duplicateProductsDisplayGroupDTO = new ProductsDisplayGroupDTO(-1, newProductId, productsDisplayGroupDTO.DisplayGroupId, productsDisplayGroupDTO.CreatedBy, productsDisplayGroupDTO.CreatedDate, productsDisplayGroupDTO.LastUpdatedBy, productsDisplayGroupDTO.LastUpdatedDate, string.Empty, productsDisplayGroupDTO.SiteId, false, -1, productsDisplayGroupDTO.IsActive, string.Empty);
                        ProductsDisplayGroup productsDisplayGroupBL = new ProductsDisplayGroup(executionContext, duplicateProductsDisplayGroupDTO);
                        isExistDisplayGroup = productsDisplayGroupBL.IsExistDisplayGroup(duplicateProductsDisplayGroupDTO.DisplayGroupId, duplicateProductsDisplayGroupDTO.ProductId, sqlTransaction);

                        if (duplicateProductsDisplayGroupDTO.DisplayGroupId != -1 && isExistDisplayGroup == false)
                        {
                            productsDisplayGroupBL.Save(sqlTransaction);
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1809));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates duplicate values in products as per previous product_Id
        /// </summary>        
        private void UpdateDuplicateProductDetails()
        {
            log.LogMethodEntry();
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            productsDataHandler.UpdateDuplicateProductDetails(duplicateProductId, newProductId);
            log.LogMethodExit();
        }

        private void LoadProductSubscritionDTOChild(int productId)
        {
            log.LogMethodEntry();
            if (productsDTO != null && productsDTO.ProductId > -1)
            {
                ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext);
                List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID, productId.ToString()));
                searchParams.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<ProductSubscriptionDTO> productSubscriptionDTOList = productSubscriptionListBL.GetProductSubscriptionDTOList(searchParams);
                if (productSubscriptionDTOList != null && productSubscriptionDTOList.Any())
                {
                    productsDTO.ProductSubscriptionDTO = productSubscriptionDTOList[0];
                }
            }
            log.LogMethodExit();
        }

        private ValidationError ProductTypeValidationsforSubscription()
        {
            log.LogMethodEntry();
            ValidationError canAddSubScriptionCheckError = null;
            if (productsDTO.ProductTypeId > -1)
            {
                if (productTypeDTOList == null)
                {
                    ProductTypeListBL productTypeListBL = new ProductTypeListBL(executionContext);
                    List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
                    productTypeDTOList = productTypeListBL.GetProductTypeDTOList(searchParams);
                }
                if (productTypeDTOList != null && productTypeDTOList.Any())
                {
                    ProductTypeDTO currentProductTypeDTO = productTypeDTOList.Find(pt => pt.ProductTypeId == productsDTO.ProductTypeId);
                    if (currentProductTypeDTO != null &&
                             (currentProductTypeDTO.ProductType == ProductTypeValues.CARDSALE.ToString()
                             || currentProductTypeDTO.ProductType == ProductTypeValues.NEW.ToString()
                             || currentProductTypeDTO.ProductType == ProductTypeValues.RECHARGE.ToString()
                             || currentProductTypeDTO.ProductType == ProductTypeValues.GAMETIME.ToString()
                             || (currentProductTypeDTO.ProductType == ProductTypeValues.LOCKER.ToString() && currentProductTypeDTO.CardSale)) == false)
                    {
                        canAddSubScriptionCheckError = new ValidationError("Products", "ProductTypeId", MessageContainerList.GetMessage(executionContext, 2983, currentProductTypeDTO.ProductType));
                        // "Cannot add subscription details for type &1."
                    }
                    if (currentProductTypeDTO.ProductType == ProductTypeValues.LOCKER.ToString() && this.productsDTO.ProductSubscriptionDTO.AutoRenew)
                    {
                        canAddSubScriptionCheckError = new ValidationError("Products", "ProductTypeId", MessageContainerList.GetMessage(executionContext, 2994));
                        //"Locker product subscription cannot be auto renewed"
                    }
                    if (currentProductTypeDTO.ProductType == ProductTypeValues.NEW.ToString() && this.productsDTO.ProductSubscriptionDTO.AutoRenew)
                    {
                        canAddSubScriptionCheckError = new ValidationError("Products", "ProductTypeId", MessageContainerList.GetMessage(executionContext, 3000));
                        //"New product subscription cannot be set with auto renew option"
                    }
                }
            }
            log.LogMethodExit(canAddSubScriptionCheckError);
            return canAddSubScriptionCheckError;
        }

        private void SetFromSiteTimeOffset()
        {
            log.LogMethodEntry(productsDTO);
            if (SiteContainerList.IsCorporate())
            {
                if (productsDTO != null)
                {
                    if (productsDTO.StartDate != null && productsDTO.StartDate != DateTime.MinValue)
                    {
                        productsDTO.StartDate = SiteContainerList.FromSiteDateTime(productsDTO.SiteId, (DateTime)productsDTO.StartDate);
                    }
                    if (productsDTO.ExpiryDate != null && productsDTO.ExpiryDate != DateTime.MinValue)
                    {
                        productsDTO.ExpiryDate = SiteContainerList.FromSiteDateTime(productsDTO.SiteId, (DateTime)productsDTO.ExpiryDate);
                    }
                    productsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit(productsDTO);
        }

        private void SetToSiteTimeOffset()
        {
            log.LogMethodEntry(productsDTO);
            if (SiteContainerList.IsCorporate())
            {
                if (productsDTO != null && (productsDTO.ProductId == -1 || productsDTO.IsChanged))
                {
                    int siteId = executionContext.GetSiteId();
                    log.Info(siteId);
                    if (productsDTO.StartDate != null && productsDTO.StartDate != DateTime.MinValue)
                    {
                        productsDTO.StartDate = SiteContainerList.ToSiteDateTime(siteId, (DateTime)productsDTO.StartDate);
                    }
                    if (productsDTO.ExpiryDate != null && productsDTO.ExpiryDate != DateTime.MinValue)
                    {
                        productsDTO.ExpiryDate = SiteContainerList.ToSiteDateTime(siteId, (DateTime)productsDTO.ExpiryDate);
                    }
                }
            }
            log.LogMethodExit(productsDTO);
        }

        private List<ProductsDTO> SetFromSiteTimeOffset(List<ProductsDTO> productsDTOList)
        {
            log.LogMethodEntry(productsDTOList);
            if (SiteContainerList.IsCorporate())
            {
                if (productsDTOList != null && productsDTOList.Any())
                {
                    for (int i = 0; i < productsDTOList.Count; i++)
                    {
                        if (productsDTOList[i].StartDate != null && productsDTOList[i].StartDate != DateTime.MinValue)
                        {
                            productsDTOList[i].StartDate = SiteContainerList.FromSiteDateTime(productsDTOList[i].SiteId, (DateTime)productsDTOList[i].StartDate);
                        }
                        if (productsDTOList[i].ExpiryDate != null && productsDTOList[i].ExpiryDate != DateTime.MinValue)
                        {
                            productsDTOList[i].ExpiryDate = SiteContainerList.FromSiteDateTime(productsDTOList[i].SiteId, (DateTime)productsDTOList[i].ExpiryDate);
                        }
                        productsDTOList[i].AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }
    }
    public class ProductsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductsDTO productsListDTO;
        private readonly bool enableDBAudit = true;
        List<ProductsDTO> productsDTOList;
        private ExecutionContext executionContext;
        ProductsFilterParams productsFilterParams;
        string productsType;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ProductsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productsDTOList = new List<ProductsDTO>();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with one parameters.
        /// </summary>
        /// <param name="executionContexts"></param>
        public ProductsList(ExecutionContext executionContext, string productsType, ProductsFilterParams productsFilterParams)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productsType, productsFilterParams);
            this.productsType = productsType;
            this.productsFilterParams = productsFilterParams;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with two parameters.
        /// </summary>
        /// <param name="productLists"></param>
        /// <param name="executionContexts"></param>
        public ProductsList(ExecutionContext executionContext, List<ProductsDTO> productLists)
        {
            log.LogMethodEntry(productLists, executionContext);
            this.productsDTOList = productLists;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with two parameters.
        /// </summary>
        /// <param name="productLists"></param>
        /// <param name="executionContexts"></param>
        public ProductsList(ExecutionContext executionContext, List<ProductsDTO> productLists, bool enableDBAudit)
            : this(executionContext, productLists)
        {
            log.LogMethodEntry(productLists, executionContext, enableDBAudit);
            this.enableDBAudit = enableDBAudit;
            log.LogMethodExit();
        }

        /// <summary>
        ///  GetGetProductsDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetProductsDTOList(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            List<ProductsDTO> returnValue = productsDataHandler.GetAllProductList(searchParameters);
            returnValue = SetFromSiteTimeOffset(returnValue);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the products list matching with search key
        /// </summary>
        /// <returns></returns>
        public List<ProductsDTO> GetProductList()
        {
            try
            {
                log.LogMethodEntry();
                Products productsData = new Products();

                switch (productsType.ToUpper().ToString())
                {
                    case "CARDS":
                        productsType = "LOCKER,LOCKER_RETURN,NEW,RECHARGE,VARIABLECARD,CARDSALE,GAMETIME";
                        break;
                    case "NON-CARD":
                        productsType = "MANUAL";
                        break;
                    case "COMBO":
                        productsType = "COMBO";
                        break;
                    case "ATTRACTIONS":
                        productsType = "ATTRACTION";
                        break;
                    case "CHECKINOUT":
                        productsType = "CHECK-IN,CHECK-OUT";
                        break;
                    case "RENTAL":
                        productsType = "RENTAL,RENTAL_RETURN";
                        break;
                    case "VOUCHERS":
                        productsType = "VOUCHER";
                        break;
                    case "ALL":
                        productsType = "";
                        break;
                }


                string[] productTypes = productsType.Split(',');
                foreach (var prodType in productTypes)
                {
                    List<ProductsDTO> tempproductsList = new List<ProductsDTO>();
                    productsFilterParams.ProductType = prodType;
                    tempproductsList = productsData.GetProductDTOList(productsFilterParams);
                    if (tempproductsList != null && tempproductsList.Count != 0)
                    {
                        productsDTOList.AddRange(tempproductsList);
                    }
                    productsFilterParams.ProductTypeId = -1;
                }
                productsDTOList = SetFromSiteTimeOffset(productsDTOList);
                log.LogMethodExit(productsDTOList);
                return productsDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Returns the products list
        /// </summary>
        public List<ProductsDTO> GetProductsDTOList(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters,
            bool loadChildRecords = false,
            bool activeChildRecords = true,
            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            List<ProductsDTO> productsDTOList = productsDataHandler.GetAllProductList(searchParameters);
            if (loadChildRecords == false ||
                productsDTOList == null ||
                productsDTOList.Any() == false)
            {
                log.LogMethodExit(productsDTOList, "Child records are not loaded.");
                return productsDTOList;
            }
            Dictionary<int, ProductsDTO> productsDTOCustomDataSetIdMap = new Dictionary<int, ProductsDTO>();
            List<int> customDataSetIdList = new List<int>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                if (productsDTOList[i].CustomDataSetId > -1 &&
                   productsDTOCustomDataSetIdMap.ContainsKey(productsDTOList[i].CustomDataSetId) == false)
                {
                    productsDTOCustomDataSetIdMap.Add(productsDTOList[i].CustomDataSetId, productsDTOList[i]);
                    customDataSetIdList.Add(productsDTOList[i].CustomDataSetId);
                }
            }
            CustomDataSetListBL customDataSetListBL = new CustomDataSetListBL(executionContext);
            List<CustomDataSetDTO> customDataSetDTOList = customDataSetListBL.GetCustomDataSetDTOList(customDataSetIdList, true, activeChildRecords, sqlTransaction);
            if (customDataSetDTOList != null && customDataSetDTOList.Any())
            {
                for (int i = 0; i < customDataSetDTOList.Count; i++)
                {
                    if (productsDTOCustomDataSetIdMap.ContainsKey(customDataSetDTOList[i].CustomDataSetId))
                    {
                        productsDTOCustomDataSetIdMap[customDataSetDTOList[i].CustomDataSetId].CustomDataSetDTO = customDataSetDTOList[i]; ;
                    }
                }
            }
            Dictionary<int, ProductsDTO> productsDTOProductsIdMap = new Dictionary<int, ProductsDTO>();
            List<int> productsIdList = new List<int>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                if (productsDTOProductsIdMap.ContainsKey(productsDTOList[i].ProductId))
                {
                    continue;
                }
                productsDTOProductsIdMap.Add(productsDTOList[i].ProductId, productsDTOList[i]);
                productsIdList.Add(productsDTOList[i].ProductId);
            }
            ProductList productList = new ProductList(executionContext);
            List<ProductDTO> productDTOList = productList.GetProductDTOListOfProducts(productsIdList, loadChildRecords, activeChildRecords, sqlTransaction);
            if (productDTOList != null && productDTOList.Any())
            {
                for (int i = 0; i < productDTOList.Count; i++)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productDTOList[i].ManualProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productDTOList[i].ManualProductId];

                    productsDTO.InventoryItemDTO = productDTOList[i];
                }
            }
            ProductsDisplayGroupList productDisplayGroupList = new ProductsDisplayGroupList(executionContext);
            List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList = productDisplayGroupList.GetProductsDisplayGroupDTOOfProducts(productsIdList, activeChildRecords, sqlTransaction);
            if (productsDisplayGroupDTOList != null && productsDisplayGroupDTOList.Any())
            {
                for (int i = 0; i < productsDisplayGroupDTOList.Count; i++)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productsDisplayGroupDTOList[i].ProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productsDisplayGroupDTOList[i].ProductId];
                    if (productsDTO.ProductsDisplayGroupDTOList == null)
                    {
                        productsDTO.ProductsDisplayGroupDTOList = new List<ProductsDisplayGroupDTO>();
                    }
                    productsDTO.ProductsDisplayGroupDTOList.Add(productsDisplayGroupDTOList[i]);
                }
            }

            ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext);
            List<ProductSubscriptionDTO> productSubscriptionDTOList = productSubscriptionListBL.GetProductSubscriptionDTOList(productsIdList, sqlTransaction);
            if (productSubscriptionDTOList != null && productSubscriptionDTOList.Any())
            {
                for (int i = 0; i < productSubscriptionDTOList.Count; i++)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productSubscriptionDTOList[i].ProductsId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productSubscriptionDTOList[i].ProductsId];
                    productsDTO.ProductSubscriptionDTO = productSubscriptionDTOList[i];
                }
            }
            ProductModifiersList productModifiersListBL = new ProductModifiersList(executionContext);
            List<ProductModifiersDTO> productModifiersDTOList = productModifiersListBL.GetProductModifiersDTOListForProducts(productsIdList, activeChildRecords, sqlTransaction);
            if (productModifiersDTOList != null && productModifiersDTOList.Any())
            {
                foreach (ProductModifiersDTO productModiferDTO in productModifiersDTOList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productModiferDTO.ProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productModiferDTO.ProductId];
                    if (productsDTO.ProductModifierDTOList == null)
                    {
                        productsDTO.ProductModifierDTOList = new List<ProductModifiersDTO>();
                    }
                    productsDTO.ProductModifierDTOList.Add(productModiferDTO);
                }

            }

            // Build Combo Product
            //Dictionary<int, ProductsDTO> comboProductsIdMap = new Dictionary<int, ProductsDTO>();
            //String productsIdListString = "";
            //for (int i = 0; i < productsDTOList.Count; i++)
            //{
            //    if (comboProductsIdMap.ContainsKey(productsDTOList[i].ProductId))
            //    {
            //        continue;
            //    }
            //    comboProductsIdMap.Add(productsDTOList[i].ProductId, productsDTOList[i]);
            //    productsIdListString += productsDTOList[i].ProductId.ToString() + ",";
            //}

            //// remove the last ,
            //if (productsIdListString.Length > 0)
            //    productsIdListString = productsIdListString.Remove(productsIdListString.Length - 1);

            ComboProductList comboProductListBL = new ComboProductList(executionContext);
            List<ComboProductDTO> comboProductList = comboProductListBL.GetComboProductDTOListForProducts(productsIdList, activeChildRecords, sqlTransaction);
            if (comboProductList != null && comboProductList.Any())
            {
                foreach (ComboProductDTO comboProductDTO in comboProductList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(comboProductDTO.ProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[comboProductDTO.ProductId];
                    if (productsDTO.ComboProductDTOList == null)
                    {
                        productsDTO.ComboProductDTOList = new List<ComboProductDTO>();
                    }
                    productsDTO.ComboProductDTOList.Add(comboProductDTO);
                }

            }

            ProductsCalenderList ProductCalendarListBL = new ProductsCalenderList(executionContext);
            List<ProductsCalenderDTO> productCalendarList = ProductCalendarListBL.GetProductsCalenderDTOListForProducts(productsIdList, activeChildRecords, sqlTransaction);
            if (productCalendarList != null && productCalendarList.Any())
            {
                foreach (ProductsCalenderDTO productsCalenderDTO in productCalendarList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productsCalenderDTO.Product_Id) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productsCalenderDTO.Product_Id];
                    if (productsDTO.ProductsCalenderDTOList == null)
                    {
                        productsDTO.ProductsCalenderDTOList = new List<ProductsCalenderDTO>();
                    }
                    productsDTO.ProductsCalenderDTOList.Add(productsCalenderDTO);
                }

            }

            List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> searchSalesOfferGroupParameters = new List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>>();
            searchSalesOfferGroupParameters.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_ACTIVE, "1"));
            SalesOfferGroupList salesOfferGroupList = new SalesOfferGroupList(executionContext);
            List<SalesOfferGroupDTO> salesOfferGroupDTOList = salesOfferGroupList.GetAllSalesOfferGroups(searchSalesOfferGroupParameters);
            UpsellOffersList upsellOffersList = new UpsellOffersList(executionContext);
            List<UpsellOffersDTO> upsellOfferDTOList = upsellOffersList.GetUpsellOffersDTOListForProducts(productsIdList, activeChildRecords, sqlTransaction);

            if (upsellOfferDTOList != null && upsellOfferDTOList.Any())
            {
                foreach (UpsellOffersDTO upsellOffersDTO in upsellOfferDTOList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(upsellOffersDTO.ProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[upsellOffersDTO.ProductId];
                    if (productsDTO.UpsellOffersDTOList == null)
                    {
                        productsDTO.UpsellOffersDTOList = new List<UpsellOffersDTO>();
                    }
                    if (productsDTO.CrossSellOffersDTOList == null)
                    {
                        productsDTO.CrossSellOffersDTOList = new List<UpsellOffersDTO>();
                    }
                    if (salesOfferGroupDTOList != null && salesOfferGroupDTOList.Any() && salesOfferGroupDTOList.Exists(x => x.SaleGroupId == upsellOffersDTO.SaleGroupId && x.IsUpsell == false))
                    {
                        productsDTO.CrossSellOffersDTOList.Add(upsellOffersDTO);
                    }
                    else
                    {
                        productsDTO.UpsellOffersDTOList.Add(upsellOffersDTO);
                    }

                }
            }

            ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext);
            List<ProductGamesDTO> productGamesList = productGamesListBL.GetProductGamesDTOListForProducts(productsIdList, activeChildRecords, sqlTransaction);
            if (productGamesList != null && productGamesList.Any())
            {
                foreach (ProductGamesDTO productGamesDTO in productGamesList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productGamesDTO.Product_id) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productGamesDTO.Product_id];
                    if (productsDTO.ProductGamesDTOList == null)
                    {
                        productsDTO.ProductGamesDTOList = new List<ProductGamesDTO>();
                    }
                    productsDTO.ProductGamesDTOList.Add(productGamesDTO);
                }
            }

            //Check in Check out phase 2 changes
            CustomerProfilingGroupListBL customerProfilingGroupListBL = new CustomerProfilingGroupListBL(executionContext);
            List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>> profileSearchParameters = new List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>>();
            profileSearchParameters.Add(new KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>(CustomerProfilingGroupDTO.SearchByParameters.IS_ACTIVE, "1"));

            List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList = customerProfilingGroupListBL.GetCustomerProfilingGroups(profileSearchParameters, true, true, sqlTransaction);
            log.LogVariableState("customerProfilingGroupDTOList", customerProfilingGroupDTOList);
            if (customerProfilingGroupDTOList != null && customerProfilingGroupDTOList.Any())
            {
                foreach (int productId in productsIdList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productId];
                    if (productsDTO.CustomerProfilingGroupId > -1)
                    {
                        log.Debug("productsDTO.CustomerProfilingGroupId : " + productsDTO.CustomerProfilingGroupId);
                        CustomerProfilingGroupDTO customerProfilingGroupDTO = customerProfilingGroupDTOList.Where(x => x.CustomerProfilingGroupId == productsDTO.CustomerProfilingGroupId).FirstOrDefault();
                        productsDTO.CustomerProfilingGroupDTO = customerProfilingGroupDTO;
                    }
                }
            }
            

            ProductCreditPlusBLList pproductCreditPlusListBL = new ProductCreditPlusBLList(executionContext);
            List<ProductCreditPlusDTO> pproductCreditPlusList = pproductCreditPlusListBL.GetProductCreditPlusDTOListForProducts(productsIdList, activeChildRecords, sqlTransaction);
            if (pproductCreditPlusList != null && pproductCreditPlusList.Any())
            {
                foreach (ProductCreditPlusDTO pproductCreditPlusDTO in pproductCreditPlusList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(pproductCreditPlusDTO.Product_id) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[pproductCreditPlusDTO.Product_id];
                    if (productsDTO.ProductCreditPlusDTOList == null)
                    {
                        productsDTO.ProductCreditPlusDTOList = new List<ProductCreditPlusDTO>();
                    }
                    productsDTO.ProductCreditPlusDTOList.Add(pproductCreditPlusDTO);
                }
            }
            productsDTOList = SetFromSiteTimeOffset(productsDTOList);
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        /// <summary>
        /// Returns the products list
        /// </summary>
        public List<ProductsDTO> GetProductsList(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters,
            int currentPage = 0,
            int pageSize = 0,
            bool loadChildRecords = false,
            bool activeChildRecords = true,
            SqlTransaction sqlTransaction = null,
            bool buildForTransaction = true,
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> transactionSearchParameters = null)
        {
            log.LogMethodEntry(searchParameters, currentPage, pageSize, loadChildRecords, activeChildRecords, sqlTransaction);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            List<ProductsDTO> productsDTOList = productsDataHandler.GetProductsList(searchParameters, currentPage, pageSize, executionContext.GetLanguageId());

            // if this is for a transaction build the transaction
            if (buildForTransaction && productsDTOList != null && productsDTOList.Any())
            {
                Dictionary<int, ProductsDTO> transactionProductsDTOIdMap = new Dictionary<int, ProductsDTO>();
                String transactionProductsIdString = "";
                for (int i = 0; i < productsDTOList.Count; i++)
                {
                    if (productsDTOList[i].ProductId > -1 &&
                       transactionProductsDTOIdMap.ContainsKey(productsDTOList[i].ProductId) == false)
                    {
                        transactionProductsDTOIdMap.Add(productsDTOList[i].ProductId, productsDTOList[i]);
                        transactionProductsIdString += productsDTOList[i].ProductId + ",";
                    }
                }
                if (!string.IsNullOrEmpty(transactionProductsIdString))
                    transactionProductsIdString = transactionProductsIdString.Remove(transactionProductsIdString.Length - 1);

                transactionSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST, transactionProductsIdString));
                List<ProductsDTO> transactionProductsDTOList = GetTransactionProductsList(transactionSearchParameters, sqlTransaction);
                if (transactionProductsDTOList != null && transactionProductsDTOList.Any())
                {
                    List<ProductsDTO> tempTrxProductsDTOList = new List<ProductsDTO>();
                    for (int i = 0; i < transactionProductsDTOList.Count; i++)
                    {
                        if (transactionProductsDTOIdMap.ContainsKey(transactionProductsDTOList[i].ProductId))
                        {
                            ProductsDTO productsDTO = transactionProductsDTOIdMap[transactionProductsDTOList[i].ProductId];
                            ProductsDTO tempProductsDTO = transactionProductsDTOList[i];

                            productsDTO.Price = tempProductsDTO.Price;

                            // if no sort order is set, it comes as -1, reset this to 999
                            if (productsDTO.SortOrder == -1)
                                productsDTO.SortOrder = 999;

                            tempTrxProductsDTOList.Add(productsDTO);
                        }
                    }
                    productsDTOList = tempTrxProductsDTOList.GroupBy(x => x.ProductId).Select(y => y.First()).ToList();
                    productsDTOList = productsDTOList.OrderBy(x => x.SortOrder).ToList();
                }
                else
                {
                    productsDTOList.Clear();
                }
            }

            if (loadChildRecords == false ||
                productsDTOList == null ||
                productsDTOList.Any() == false)
            {
                log.LogMethodExit(productsDTOList, "Child records are not loaded.");
                return productsDTOList;
            }
            Build(productsDTOList, loadChildRecords, activeChildRecords, sqlTransaction);
            productsDTOList = SetFromSiteTimeOffset(productsDTOList);
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        private void Build(List<ProductsDTO> productsDTOList, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productsDTOList, loadChildRecords, activeChildRecords, sqlTransaction);
            Dictionary<int, ProductsDTO> productsDTOCustomDataSetIdMap = new Dictionary<int, ProductsDTO>();
            List<int> customDataSetIdList = new List<int>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                if (productsDTOList[i].CustomDataSetId > -1 &&
                   productsDTOCustomDataSetIdMap.ContainsKey(productsDTOList[i].CustomDataSetId) == false)
                {
                    productsDTOCustomDataSetIdMap.Add(productsDTOList[i].CustomDataSetId, productsDTOList[i]);
                    customDataSetIdList.Add(productsDTOList[i].CustomDataSetId);
                }
            }

            CustomDataSetListBL customDataSetListBL = new CustomDataSetListBL(executionContext);
            List<CustomDataSetDTO> customDataSetDTOList = customDataSetListBL.GetCustomDataSetDTOList(customDataSetIdList, true, activeChildRecords, sqlTransaction);
            if (customDataSetDTOList != null && customDataSetDTOList.Any())
            {
                for (int i = 0; i < customDataSetDTOList.Count; i++)
                {
                    if (productsDTOCustomDataSetIdMap.ContainsKey(customDataSetDTOList[i].CustomDataSetId) == false)
                    {
                        continue;
                    }
                    productsDTOCustomDataSetIdMap[customDataSetDTOList[i].CustomDataSetId].CustomDataSetDTO = customDataSetDTOList[i];
                }
            }

            Dictionary<int, ProductsDTO> productsDTOProductsIdMap = new Dictionary<int, ProductsDTO>();
            List<int> productsIdList = new List<int>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                if (productsDTOProductsIdMap.ContainsKey(productsDTOList[i].ProductId))
                {
                    continue;
                }
                productsDTOProductsIdMap.Add(productsDTOList[i].ProductId, productsDTOList[i]);
                productsIdList.Add(productsDTOList[i].ProductId);
            }
            String productsIdListString = String.Join(",", productsIdList.ToArray());

            ProductList productList = new ProductList(executionContext);
            List<ProductDTO> productDTOList = productList.GetProductDTOListOfProducts(productsIdList, loadChildRecords, activeChildRecords, sqlTransaction);
            if (productDTOList != null && productDTOList.Any())
            {
                for (int i = 0; i < productDTOList.Count; i++)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productDTOList[i].ManualProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productDTOList[i].ManualProductId];

                    productsDTO.InventoryItemDTO = productDTOList[i];
                }
            }

            ProductsDisplayGroupList productDisplayGroupList = new ProductsDisplayGroupList(executionContext);
            List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList = productDisplayGroupList.GetProductsDisplayGroupDTOOfProducts(productsIdList, activeChildRecords, sqlTransaction);
            if (productsDisplayGroupDTOList != null && productsDisplayGroupDTOList.Any())
            {
                for (int i = 0; i < productsDisplayGroupDTOList.Count; i++)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productsDisplayGroupDTOList[i].ProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productsDisplayGroupDTOList[i].ProductId];
                    if (productsDTO.ProductsDisplayGroupDTOList == null)
                    {
                        productsDTO.ProductsDisplayGroupDTOList = new List<ProductsDisplayGroupDTO>();
                    }
                    productsDTO.ProductsDisplayGroupDTOList.Add(productsDisplayGroupDTOList[i]);
                }
            }

            //// Build Modifiers Product
            //Dictionary<int, ProductsDTO> modifierProductsIdMap = new Dictionary<int, ProductsDTO>();
            //String modifiersIdListString = "";
            //for (int i = 0; i < productsDTOList.Count; i++)
            //{
            //    if (modifierProductsIdMap.ContainsKey(productsDTOList[i].ProductId))
            //    {
            //        continue;
            //    }
            //    modifierProductsIdMap.Add(productsDTOList[i].ProductId, productsDTOList[i]);
            //    modifiersIdListString += productsDTOList[i].ProductId.ToString() + ",";
            //}

            //// remove the last ,
            //if (modifiersIdListString.Length > 0)
            //    modifiersIdListString = modifiersIdListString.Remove(modifiersIdListString.Length - 1);

            List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> productModifiersListSearchParameters = new List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>>();
            productModifiersListSearchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.ISACTIVE, "1"));
            productModifiersListSearchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.PRODUCT_ID_LIST, productsIdListString));

            ProductModifiersList productModifiersListBL = new ProductModifiersList(executionContext);
            List<ProductModifiersDTO> productModifiersDTOList = productModifiersListBL.GetAllProductModifiersList(productModifiersListSearchParameters, null, loadChildRecords);
            if (productModifiersDTOList != null && productModifiersDTOList.Any())
            {
                foreach (ProductModifiersDTO productModiferDTO in productModifiersDTOList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productModiferDTO.ProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productModiferDTO.ProductId];
                    if (productsDTO.ProductModifierDTOList == null)
                    {
                        productsDTO.ProductModifierDTOList = new List<ProductModifiersDTO>();
                    }
                    productsDTO.ProductModifierDTOList.Add(productModiferDTO);
                }

            }

            // Build Combo Product
            //Dictionary<int, ProductsDTO> comboProductsIdMap = new Dictionary<int, ProductsDTO>();
            //String productsIdListString = "";
            //for (int i = 0; i < productsDTOList.Count; i++)
            //{
            //    if (comboProductsIdMap.ContainsKey(productsDTOList[i].ProductId))
            //    {
            //        continue;
            //    }
            //    comboProductsIdMap.Add(productsDTOList[i].ProductId, productsDTOList[i]);
            //    productsIdListString += productsDTOList[i].ProductId.ToString() + ",";
            //}

            //// remove the last ,
            //if (productsIdListString.Length > 0)
            //    productsIdListString = productsIdListString.Remove(productsIdListString.Length - 1);

            List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> comboProductsSearchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
            comboProductsSearchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.IS_ACTIVE, "1"));
            comboProductsSearchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID_LIST, productsIdListString));

            ComboProductList comboProductListBL = new ComboProductList(executionContext);
            List<ComboProductDTO> comboProductList = comboProductListBL.GetComboProductDTOList(comboProductsSearchParameters, null);
            if (comboProductList != null && comboProductList.Any())
            {
                foreach (ComboProductDTO comboProductDTO in comboProductList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(comboProductDTO.ProductId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[comboProductDTO.ProductId];
                    if (productsDTO.ComboProductDTOList == null)
                    {
                        productsDTO.ComboProductDTOList = new List<ComboProductDTO>();
                    }
                    productsDTO.ComboProductDTOList.Add(comboProductDTO);
                }

            }

            // Build Product Credit Plus 
            //Dictionary<int, ProductsDTO> creditPlusProductsIdMap = new Dictionary<int, ProductsDTO>();
            //String cpProductsIdListString = "";
            //for (int i = 0; i < productsDTOList.Count; i++)
            //{
            //    if (creditPlusProductsIdMap.ContainsKey(productsDTOList[i].ProductId))
            //    {
            //        continue;
            //    }
            //    creditPlusProductsIdMap.Add(productsDTOList[i].ProductId, productsDTOList[i]);
            //    cpProductsIdListString += productsDTOList[i].ProductId.ToString() + ",";
            //}

            //// remove the last ,
            //if (cpProductsIdListString.Length > 0)
            //    cpProductsIdListString = cpProductsIdListString.Remove(productsIdListString.Length - 1);

            List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>> cpSearchParameters = new List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>>();
            cpSearchParameters.Add(new KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>(ProductCreditPlusDTO.SearchByParameters.ISACTIVE, "1"));
            cpSearchParameters.Add(new KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>(ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID_LIST, productsIdListString));

            ProductCreditPlusBLList productCreditPlusListBL = new ProductCreditPlusBLList(executionContext);
            List<ProductCreditPlusDTO> productCreditPlusDTOList = productCreditPlusListBL.GetAllProductCreditPlusListDTOList(cpSearchParameters, true, null);
            if (productCreditPlusDTOList != null && productCreditPlusDTOList.Any())
            {
                foreach (ProductCreditPlusDTO productCPDTO in productCreditPlusDTOList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productCPDTO.Product_id) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productCPDTO.Product_id];
                    if (productsDTO.ProductCreditPlusDTOList == null)
                    {
                        productsDTO.ProductCreditPlusDTOList = new List<ProductCreditPlusDTO>();
                    }
                    productsDTO.ProductCreditPlusDTOList.Add(productCPDTO);
                }
            }


            // Build Product Games Plus 
            List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> productGamesSearchParameters = new List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>();
            productGamesSearchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.ISACTIVE, "1"));
            productGamesSearchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID_LIST, productsIdListString));

            ProductGamesListBL productsGamesListBL = new ProductGamesListBL(executionContext);
            List<ProductGamesDTO> productGamessDTOList = productsGamesListBL.GetProductGamesDTOList(productGamesSearchParameters, null);
            if (productGamessDTOList != null && productGamessDTOList.Any())
            {
                foreach (ProductGamesDTO gamesDTO in productGamessDTOList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(gamesDTO.Product_id) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[gamesDTO.Product_id];
                    if (productsDTO.ProductGamesDTOList == null)
                    {
                        productsDTO.ProductGamesDTOList = new List<ProductGamesDTO>();
                    }
                    productsDTO.ProductGamesDTOList.Add(gamesDTO);
                }
            }
            //Add prodSubscription
            List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> prodSubscriptionSearchParameters = new List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>>();
            //prodSubscriptionSearchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.IS_ACTIVE, "1"));
            prodSubscriptionSearchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID_LIST, productsIdListString));

            ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext);
            List<ProductSubscriptionDTO> productSubscriptionDTOList = productSubscriptionListBL.GetProductSubscriptionDTOList(prodSubscriptionSearchParameters, null);
            if (productSubscriptionDTOList != null && productSubscriptionDTOList.Any())
            {
                foreach (ProductSubscriptionDTO productSubscriptionDTOItem in productSubscriptionDTOList)
                {
                    if (productsDTOProductsIdMap.ContainsKey(productSubscriptionDTOItem.ProductsId) == false)
                    {
                        continue;
                    }
                    ProductsDTO productsDTO = productsDTOProductsIdMap[productSubscriptionDTOItem.ProductsId];
                    productsDTO.ProductSubscriptionDTO = productSubscriptionDTOItem;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the products list
        /// </summary>
        public List<ProductsDTO> GetTransactionProductsList(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> transactionSearchParameters,
            SqlTransaction sqlTransaction = null)
        {
            List<ProductsDTO> transactionProductsDTOList = null;
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            transactionProductsDTOList = productsDataHandler.GetTransactionProductsList(transactionSearchParameters);
            transactionProductsDTOList = SetFromSiteTimeOffset(transactionProductsDTOList);
            return transactionProductsDTOList;
        }

        /// <summary>
        /// Retruns the no of products details count
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public int GetProductsDTOCount(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            int productsCount = productsDataHandler.GetProductsDTOCount(searchParameters);
            return productsCount;
        }
        /// <summary>
        /// Validates and saves the productsDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productsDTOList == null ||
                productsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            List<CustomDataSetDTO> updatedCustomDataSetDTOList = new List<CustomDataSetDTO>();
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
            Dictionary<int, CustomAttributesDTO> customAttributesDTOMap = customAttributesListBL.GetCustomAttributesDTOMap(Applicability.INVPRODUCT);
            Dictionary<int, CustomAttributesDTO> productCustomAttributesDTOMap = customAttributesListBL.GetCustomAttributesDTOMap(Applicability.PRODUCT);
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                ProductsDTO productsDTO = productsDTOList[i];
                ProductsDTO existingProductsDTO = null;
                if(productsDTO.ProductId > -1)
                {
                    Products products = new Products(executionContext, productsDTO.ProductId, true, true);
                    existingProductsDTO = products.GetProductsDTO;
                }
                if (productsDTO.InventoryItemDTO != null)
                {
                    if (existingProductsDTO!= null && existingProductsDTO.Description != productsDTO.Description && productsDTO.IsChanged)
                    {
                        productsDTO.InventoryItemDTO.Description = productsDTO.Description;
                    }
                    if(existingProductsDTO != null && existingProductsDTO.InventoryItemDTO != null && existingProductsDTO.InventoryItemDTO.Description != productsDTO.InventoryItemDTO.Description && productsDTO.InventoryItemDTO.IsChanged)
                    {
                        productsDTO.Description = productsDTO.InventoryItemDTO.Description;
                    }
                    if(existingProductsDTO == null && productsDTO.Description != productsDTO.InventoryItemDTO.Description)
                    {
                        productsDTO.Description = productsDTO.InventoryItemDTO.Description;
                    }
                    if (existingProductsDTO == null && productsDTO.CategoryId != productsDTO.InventoryItemDTO.CategoryId)
                    {
                        productsDTO.CategoryId = productsDTO.InventoryItemDTO.CategoryId;
                    }
                    if (existingProductsDTO != null && existingProductsDTO.CategoryId != productsDTO.CategoryId && productsDTO.IsChanged)
                    {
                        productsDTO.InventoryItemDTO.CategoryId = productsDTO.CategoryId;
                    }
                    if (existingProductsDTO != null && existingProductsDTO.InventoryItemDTO != null && existingProductsDTO.InventoryItemDTO.CategoryId != productsDTO.InventoryItemDTO.CategoryId && productsDTO.InventoryItemDTO.IsChanged)
                    {
                        productsDTO.CategoryId = productsDTO.InventoryItemDTO.CategoryId;
                    }
                    if (productsDTO.InventoryProductCode != productsDTO.InventoryItemDTO.Code)
                    {
                        productsDTO.InventoryProductCode = productsDTO.InventoryItemDTO.Code;
                    }
                    if (productsDTO.InventoryItemDTO.TaxInclusiveCost != productsDTO.TaxInclusivePrice)
                    {
                        productsDTO.InventoryItemDTO.TaxInclusiveCost = productsDTO.TaxInclusivePrice;
                    }
                    if (productsDTO.InventoryItemDTO.ProductName != productsDTO.ProductName)
                    {
                        productsDTO.InventoryItemDTO.ProductName = productsDTO.ProductName;
                    }
                    if ((decimal)productsDTO.InventoryItemDTO.SalePrice != productsDTO.Price)
                    {
                        productsDTO.InventoryItemDTO.SalePrice = (double)productsDTO.Price;
                    }
                }
                if (productsDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                Products productsBL = new Products(executionContext, productsDTO);
                List<ValidationError> validationErrors = productsBL.Validate(customAttributesDTOMap, productCustomAttributesDTOMap);
                if (validationErrors.Any())
                {
                    log.LogMethodExit(null, "Validation failed. " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException("Validation failed for Products.", validationErrors, i);
                }
                if (productsDTO.CustomDataSetDTO != null &&
                   productsDTO.CustomDataSetDTO.IsChangedRecursive)
                {
                    updatedCustomDataSetDTOList.Add(productsDTO.CustomDataSetDTO);
                }
            }
            if (updatedCustomDataSetDTOList.Any())
            {
                CustomDataSetListBL customDataSetListBL = new CustomDataSetListBL(executionContext, updatedCustomDataSetDTOList);
                customDataSetListBL.Save(sqlTransaction);
            }
            List<ProductsDTO> updatedProductsDTOList = new List<ProductsDTO>(productsDTOList.Count);
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                if (productDTOList[i].CustomDataSetDTO != null &&
                   productDTOList[i].CustomDataSetId != productDTOList[i].CustomDataSetDTO.CustomDataSetId)
                {
                    productDTOList[i].CustomDataSetId = productDTOList[i].CustomDataSetDTO.CustomDataSetId;
                }
                if (productsDTOList[i].IsChanged)
                {
                    updatedProductsDTOList.Add(productsDTOList[i]);
                }
            }
            if (updatedProductsDTOList.Any())
            {
                ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
                productsDataHandler.Save(updatedProductsDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
                if (enableDBAudit)
                {
                    /// This for audit for Product post insertion/update
                    /// Below code is to save the Audit log details into DBAuditLog
                    for (int i = 0; i < updatedProductsDTOList.Count; i++)
                    {
                        ProductsDTO productsDTO = updatedProductsDTOList[i];
                        if (!string.IsNullOrEmpty(productsDTO.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("products", productsDTO.Guid);
                        }
                    }
                }
                for (int i = 0; i < updatedProductsDTOList.Count; i++)
                {
                    ProductsDTO productsDTO = updatedProductsDTOList[i];
                    if (productsDTO.ProductsDisplayGroupDTOList != null)
                    {
                        for (int j = 0; j < productsDTO.ProductsDisplayGroupDTOList.Count; j++)
                        {
                            ProductsDisplayGroupDTO productsDisplayGroupDTO = productsDTO.ProductsDisplayGroupDTOList[j];
                            if (productsDisplayGroupDTO.ProductId != productsDTO.ProductId)
                            {
                                productsDisplayGroupDTO.ProductId = productsDTO.ProductId;
                            }
                        }
                    }
                    if (productsDTO.InventoryItemDTO != null &&
                        productsDTO.InventoryItemDTO.ManualProductId != productsDTO.ProductId)
                    {
                        productsDTO.InventoryItemDTO.ManualProductId = productsDTO.ProductId;
                    }
                }
            }
            List<ProductDTO> updatedProductDTOList = new List<ProductDTO>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                ProductsDTO productsDTO = productsDTOList[i];
                if (productsDTO.InventoryItemDTO == null ||
                    productsDTO.InventoryItemDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                if (productsDTO.InventoryItemDTO.ManualProductId != productsDTO.ProductId)
                {
                    productsDTO.InventoryItemDTO.ManualProductId = productsDTO.ProductId;
                }
                updatedProductDTOList.Add(productsDTO.InventoryItemDTO);
            }
            if (updatedProductDTOList.Any())
            {
                ProductList productList = new ProductList(executionContext, updatedProductDTOList);
                productList.Save(sqlTransaction);

            }
            List<ProductsDisplayGroupDTO> updatedProductDisplayGroupDTOList = new List<ProductsDisplayGroupDTO>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                ProductsDTO productsDTO = productsDTOList[i];
                if (productsDTO.ProductsDisplayGroupDTOList == null ||
                    productsDTO.ProductsDisplayGroupDTOList.Any() == false)
                {
                    continue;
                }
                for (int j = 0; j < productsDTO.ProductsDisplayGroupDTOList.Count; j++)
                {
                    ProductsDisplayGroupDTO productsDisplayGroupDTO = productsDTO.ProductsDisplayGroupDTOList[j];
                    if (productsDisplayGroupDTO.ProductId != productsDTO.ProductId)
                    {
                        productsDisplayGroupDTO.ProductId = productsDTO.ProductId;
                    }
                    if (productsDisplayGroupDTO.IsChanged)
                    {
                        updatedProductDisplayGroupDTOList.Add(productsDisplayGroupDTO);
                    }
                }
            }
            if (updatedProductDisplayGroupDTOList.Any())
            {
                ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext, updatedProductDisplayGroupDTOList);
                productsDisplayGroupList.Save(sqlTransaction);
            }

            List<ProductSubscriptionDTO> updatedProductSubscriptionDTOList = new List<ProductSubscriptionDTO>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                ProductsDTO productsDTO = productsDTOList[i];
                if (productsDTO.ProductSubscriptionDTO == null)
                {
                    continue;
                }
                ProductSubscriptionDTO productSubscriptionDTO = productsDTO.ProductSubscriptionDTO;
                if (productSubscriptionDTO.ProductsId != productsDTO.ProductId)
                {
                    productSubscriptionDTO.ProductsId = productsDTO.ProductId;
                }
                if (productSubscriptionDTO.IsChanged)
                {
                    updatedProductSubscriptionDTOList.Add(productSubscriptionDTO);
                }
            }
            if (updatedProductSubscriptionDTOList.Any())
            {
                ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext, updatedProductSubscriptionDTOList);
                productSubscriptionListBL.Save(sqlTransaction);
            }
            List<ComboProductDTO> updatedComboProductDTOList = new List<ComboProductDTO>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                ProductsDTO productsDTO = productsDTOList[i];
                if (productsDTO.ComboProductDTOList == null ||
                    productsDTO.ComboProductDTOList.Any() == false)
                {
                    continue;
                }
                for (int j = 0; j < productsDTO.ComboProductDTOList.Count; j++)
                {
                    ComboProductDTO comboProductDTO = productsDTO.ComboProductDTOList[j];
                    if (comboProductDTO.ProductId != productsDTO.ProductId)
                    {
                        comboProductDTO.ProductId = productsDTO.ProductId;
                    }
                    if (comboProductDTO.IsChanged)
                    {
                        updatedComboProductDTOList.Add(comboProductDTO);
                    }
                }
            }
            if (updatedComboProductDTOList.Any())
            {
                ComboProductList comboProductList = new ComboProductList(executionContext, updatedComboProductDTOList);
                comboProductList.SaveUpdateComboProductList(sqlTransaction);
            }
            log.LogMethodExit();
        }




        /// <summary>
        /// Gets the SplitProductList
        /// </summary>
        /// <param name="price"></param>
        /// <param name="productType"></param>
        /// <returns></returns>
        public List<ProductsDTO> getSplitProductList(double price, string productType)
        {
            log.LogMethodEntry(price, productsType);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            List<ProductsDTO> returnValue = productsDataHandler.getSplitProductList(price, productType);
            returnValue = SetFromSiteTimeOffset(returnValue);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the products list
        /// </summary>
        public List<ProductsDTO> productDTOList { get { return productsDTOList; } }

        public List<ProductsDTO> GetProductListByFilterparams(ProductsFilterParams productsFilterParams)
        {
            List<ProductsDTO> productsList;
            try
            {
                log.LogMethodEntry();
                ProductsDataHandler dataHandler = new ProductsDataHandler();

                if (productsFilterParams.RequiresCardProduct)
                {
                    if (productsFilterParams.NewCard)
                    {
                        productsFilterParams.ProductTypeExclude = "RECHARGE,LOCKER,LOCKER_RETURN,VARIABLECARD,MANUAL,COMBO,BOOKINGS,ATTRACTION";
                    }
                    else
                    {
                        productsFilterParams.ProductTypeExclude = "NEW,LOCKER,LOCKER_RETURN,VARIABLECARD,MANUAL,COMBO,BOOKINGS,ATTRACTION";
                    }
                }
                else
                {
                    productsFilterParams.ProductTypeExclude = "LOCKER','LOCKER_RETURN','NEW,RECHARGE','VARIABLECARD','CARDSALE','GAMETIME";
                }

                productsList = dataHandler.GetProductListByFilterParameters(productsFilterParams);
                productsList = SetFromSiteTimeOffset(productsList);
                log.LogMethodExit(productsList);
                return productsList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex);
                throw;
            }
        }

        /// <summary>
        /// GetBookingProductList
        /// </summary>
        /// <param name="productsFilterParams"></param>
        /// <param name="getActiveRecords"></param>
        /// <returns>List of BookingProduct</returns>
        public List<BookingProduct> GetBookingProductList(ProductsFilterParams productsFilterParams, bool getActiveRecords)
        {
            log.LogMethodEntry(productsFilterParams, getActiveRecords);
            DataTable dtbookingProducts = new DataTable();
            List<BookingProduct> bookingProductList = new List<BookingProduct>();
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            dtbookingProducts = productsDataHandler.GetBookingProducts(productsFilterParams.SiteId, productsFilterParams.MachineName, productsFilterParams.LoginId);
            dtbookingProducts.DefaultView.Sort = "sort_order asc";

            foreach (DataRow productRow in dtbookingProducts.Rows)
            {
                bool blnSkipRec = false;
                if (productsFilterParams.ProductId > 0)
                {
                    if (productsFilterParams.ProductId != Convert.ToInt32(productRow["product_id"]))
                    {
                        blnSkipRec = true;
                    }
                }

                if (blnSkipRec == false)
                {
                    BookingProduct bookingProduct = new BookingProduct();
                    bookingProduct = new BookingProduct(
                                                        Convert.ToInt32(productRow["product_id"]),
                                                        productRow["product_name"].ToString(),
                                                        productRow["description"].ToString(),
                                                        productRow["ImageFileName"].ToString(),
                                                        (productRow["price"] == DBNull.Value ? 0 : Convert.ToDouble(productRow["price"])),
                                                        Convert.ToDouble(productRow["AdvanceAmount"]),
                                                        Convert.ToInt32(productRow["MinimumQuantity"]),
                                                        Convert.ToInt32(productRow["MinimumTime"]),
                                                        Convert.ToInt32(productRow["MaximumTime"]), 0,
                                                        Convert.ToInt32(productRow["AvailableUnits"]),
                                                        Convert.ToInt32(productRow["sort_order"]),
                                                        (productRow["AdvancePercentage"] == DBNull.Value ? 0 : Convert.ToDouble(productRow["AdvancePercentage"])), (productRow["WebDescription"] == DBNull.Value ? "" : productRow["WebDescription"].ToString()));

                    bookingProductList.Add(bookingProduct);
                }
            }


            if (productsFilterParams.ShowProductContents)
            {
                foreach (BookingProduct bp in bookingProductList)
                {
                    List<BookingProductContent> bpcList = new List<BookingProductContent>();
                    bpcList = productsDataHandler.GetBookingProductContents(bp.ProductId, getActiveRecords);

                    List<BookingPackageProduct> bookingPackageProductList = new List<BookingPackageProduct>();
                    foreach (BookingProductContent bpc in bpcList)
                    {
                        BookingPackageProduct bpp = new BookingPackageProduct(bpc, productsDataHandler.GetPackageContents(bpc.ProductId, getActiveRecords));
                        bpp.CategoryId = bpc.CategoryId;
                        bpp.Price = bpc.Price;
                        bpp.PriceInclusive = bpc.PriceInclusive;
                        bpp.ProductDescription = bpc.ProductDescription;
                        bpp.ProductDisplayGroup = bpc.ProductDisplayGroup;
                        bpp.ProductId = bpc.ProductId;
                        bpp.ProductImage = bpc.ProductImage;
                        bpp.ProductName = bpc.ProductName;
                        bpp.ProductType = bpc.ProductType;
                        bpp.Quantity = bpc.Quantity;
                        bpp.ComboProductId = bpc.ComboProductId;
                        bpp.FaceValue = bpc.FaceValue;
                        bpp.WebDescription = bpc.WebDescription;

                        bookingPackageProductList.Add(bpp);
                    }

                    bp.BookingProductPackagelist = bookingPackageProductList;
                }
            }

            return bookingProductList;
        }

        /// <summary>
        /// Gets the ProductsDescription List based on searchParameters and productId
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<ObjectTranslationsDTO> GetProductsDescriptionList(List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters, string productId)
        {
            log.LogMethodEntry(searchParameters, productId);
            try
            {
                Products products = new Products(Convert.ToInt32(productId));
                searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, products.GetProductsDTO.Guid));
                ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(executionContext);
                List<ObjectTranslationsDTO> objectTranslationsDTOList = objectTranslationsList.GetAllObjectTranslations(searchParameters);

                if (objectTranslationsDTOList == null)
                {
                    ObjectTranslationsDTO objectTranslationsDTO = new ObjectTranslationsDTO();
                    foreach (KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string> searchParameter in searchParameters)
                    {
                        if (searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT)
                        {
                            objectTranslationsDTO.Element = searchParameter.Value;
                        }
                        else if (searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.OBJECT)
                        {
                            objectTranslationsDTO.TableObject = searchParameter.Value;
                        }
                        else if (searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.SITE_ID)
                        {
                            objectTranslationsDTO.SiteId = Convert.ToInt32(searchParameter.Value);
                        }
                    }
                    objectTranslationsDTO.ElementGuid = products.GetProductsDTO.Guid;
                    objectTranslationsDTO.AcceptChanges();
                    List<ObjectTranslationsDTO> emptyObjectTranslationsDTOList = new List<ObjectTranslationsDTO>();
                    emptyObjectTranslationsDTOList.Add(objectTranslationsDTO);

                    log.LogMethodExit(objectTranslationsDTOList);
                    return emptyObjectTranslationsDTOList;
                }
                log.LogMethodExit(objectTranslationsDTOList);
                return objectTranslationsDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Delete the Products details based on ProductId
        /// </summary>
        public void DeleteProductsList()
        {
            log.LogMethodEntry();
            if (productsDTOList != null && productsDTOList.Count > 0)
            {
                foreach (ProductsDTO productsDTO in productsDTOList)
                {
                    if (productsDTO.IsChanged && productsDTO.ActiveFlag == false)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();

                                Products products = new Products(executionContext, productsDTO.ProductId, true);
                                if ((products.GetProductsDTO.CrossSellOffersDTOList != null && products.GetProductsDTO.CrossSellOffersDTOList.Any())
                                    || (products.GetProductsDTO.ProductCreditPlusDTOList != null && products.GetProductsDTO.ProductCreditPlusDTOList.Any())
                                    || (products.GetProductsDTO.ProductGamesDTOList != null && products.GetProductsDTO.ProductGamesDTOList.Any())
                                    || (products.GetProductsDTO.ProductModifierDTOList != null && products.GetProductsDTO.ProductModifierDTOList.Any())
                                    || (products.GetProductsDTO.ProductsCalenderDTOList != null && products.GetProductsDTO.ProductsCalenderDTOList.Any())
                                    || (products.GetProductsDTO.ProductsDisplayGroupDTOList != null && products.GetProductsDTO.ProductsDisplayGroupDTOList.Any())
                                    || (products.GetProductsDTO.ProductSubscriptionDTO != null)
                                    || (products.GetProductsDTO.UpsellOffersDTOList != null && products.GetProductsDTO.UpsellOffersDTOList.Any()))
                                {
                                    string message = MessageContainerList.GetMessage(executionContext, 1869);
                                    log.LogMethodExit(message);
                                    throw new ValidationException(message);
                                }
                                products.DeleteProducts(productsDTO.ProductId, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method is Used for Viator.
        /// Get the List of Product based on productDisplayGroupFrormatId, siteId
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetProductsListFromDisplayGroup(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            List<ProductsDTO> returnValue = productsDataHandler.GetProductsListFromDisplayGroup(searchParameters);
            returnValue = SetFromSiteTimeOffset(returnValue);
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        public DateTime? GetProductsLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
            DateTime? result = productsDataHandler.GetProductsLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public List<ProductsDTO> GetSystemProductsDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler(null);
            List<ProductsDTO> result = productsDataHandler.GetSystemProductsDTOList(siteId);
            if(result != null && result.Any())
            {
                Build(result, true, true);
            }
            result = SetFromSiteTimeOffset(result);
            log.LogMethodExit(result);
            return result;
        }


        public ProductsDTO GeneateDuplicateCopy(int productsId)
        {
            log.LogMethodEntry();
            Products products = new Products(executionContext, productsId, true, true);
            ProductsDTO productsDuplicateDTO = CloneProductsDTO(products.GetProductsDTO);
            if (productsDuplicateDTO != null && productsDuplicateDTO.InventoryItemDTO != null)
            {
                productsDuplicateDTO.InventoryItemDTO.Code = string.Empty;
            }
            log.LogMethodExit(productsDuplicateDTO);
            return productsDuplicateDTO;
        }

        private ProductsDTO CloneProductsDTO(ProductsDTO sourceDTO)
        {
            log.LogMethodEntry();
            if (sourceDTO == null || sourceDTO.ProductId == -1)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 989));
                //Please save the entry first.
            }
            sourceDTO.ProductId = -1;
            sourceDTO.SegmentCategoryId = -1;
            sourceDTO.Guid = string.Empty;
            sourceDTO.SiteId = executionContext.GetSiteId();
            if (sourceDTO.ComboProductDTOList != null && sourceDTO.ComboProductDTOList.Any())
            {
                for (int i = 0; i < sourceDTO.ComboProductDTOList.Count; i++)
                {
                    sourceDTO.ComboProductDTOList[i].ComboProductId = -1;
                    sourceDTO.ComboProductDTOList[i].ProductId = -1;
                    sourceDTO.ComboProductDTOList[i].Guid = string.Empty;
                    sourceDTO.ComboProductDTOList[i].SiteId = sourceDTO.SiteId;
                }
            }
            if (sourceDTO.ProductModifierDTOList != null && sourceDTO.ProductModifierDTOList.Any())
            {
                for (int i = 0; i < sourceDTO.ProductModifierDTOList.Count; i++)
                {
                    sourceDTO.ProductModifierDTOList[i].ProductModifierId = -1;
                    sourceDTO.ProductModifierDTOList[i].ProductId = -1;
                    sourceDTO.ProductModifierDTOList[i].GUID = string.Empty;
                    sourceDTO.ProductModifierDTOList[i].Site_Id = sourceDTO.SiteId;
                }
            }

            if (sourceDTO.ProductCreditPlusDTOList != null && sourceDTO.ProductCreditPlusDTOList.Any())
            {
                for (int i = 0; i < sourceDTO.ProductCreditPlusDTOList.Count; i++)
                {
                    sourceDTO.ProductCreditPlusDTOList[i].ProductCreditPlusId = -1;
                    sourceDTO.ProductCreditPlusDTOList[i].Product_id = -1;
                    sourceDTO.ProductCreditPlusDTOList[i].Guid = string.Empty;
                    sourceDTO.ProductCreditPlusDTOList[i].Site_id = sourceDTO.SiteId;
                    if (sourceDTO.ProductCreditPlusDTOList[i].CreditPlusConsumptionRulesList != null && sourceDTO.ProductCreditPlusDTOList[i].CreditPlusConsumptionRulesList.Any())
                    {
                        for (int j = 0; j < sourceDTO.ProductCreditPlusDTOList[i].CreditPlusConsumptionRulesList.Count; j++)
                        {
                            sourceDTO.ProductCreditPlusDTOList[i].CreditPlusConsumptionRulesList[j].PKId = -1;
                            sourceDTO.ProductCreditPlusDTOList[i].CreditPlusConsumptionRulesList[j].ProductCreditPlusId = -1;
                            sourceDTO.ProductCreditPlusDTOList[i].CreditPlusConsumptionRulesList[j].SiteId = sourceDTO.SiteId;
                            //Exlcusion?
                        }
                    }
                }
            }
            if (sourceDTO.ProductGamesDTOList != null && sourceDTO.ProductGamesDTOList.Any())
            {
                for (int i = 0; i < sourceDTO.ProductGamesDTOList.Count; i++)
                {
                    sourceDTO.ProductGamesDTOList[i].Product_game_id = -1;
                    sourceDTO.ProductGamesDTOList[i].Product_id = -1;
                    //sourceDTO.ProductGamesDTOList[i].Guid = string.Empty;
                    //sourceDTO.ProductGamesDTOList[i].Site_id = sourceDTO.SiteId;
                    if (sourceDTO.ProductGamesDTOList[i].ProductGamesExtendedDTOList != null && sourceDTO.ProductGamesDTOList[i].ProductGamesExtendedDTOList.Any())
                    {
                        for (int j = 0; j < sourceDTO.ProductGamesDTOList[i].ProductGamesExtendedDTOList.Count; j++)
                        {
                            sourceDTO.ProductGamesDTOList[i].ProductGamesExtendedDTOList[j].Id = -1;
                            sourceDTO.ProductGamesDTOList[i].ProductGamesExtendedDTOList[j].ProductGameId = -1;
                            //sourceDTO.ProductGamesDTOList[i].ProductGamesExtendedDTOList[j].Guid = string.Empty;
                            //sourceDTO.ProductGamesDTOList[i].ProductGamesExtendedDTOList[j].Site_id = sourceDTO.SiteId;
                            //Exlcusion?
                        }
                    }
                }
            }
            if (sourceDTO.ProductsDisplayGroupDTOList != null && sourceDTO.ProductsDisplayGroupDTOList.Any())
            {
                for (int i = 0; i < sourceDTO.ProductsDisplayGroupDTOList.Count; i++)
                {
                    sourceDTO.ProductsDisplayGroupDTOList[i].Id = -1;
                    sourceDTO.ProductsDisplayGroupDTOList[i].ProductId = -1;
                    sourceDTO.ProductsDisplayGroupDTOList[i].Guid = string.Empty;
                    sourceDTO.ProductsDisplayGroupDTOList[i].SiteId = sourceDTO.SiteId;
                }
            }
            if (sourceDTO.InventoryItemDTO != null && sourceDTO.InventoryItemDTO.ProductId > -1)
            {
                sourceDTO.InventoryItemDTO.ProductId = -1;
                sourceDTO.InventoryItemDTO.ManualProductId = -1;
                sourceDTO.InventoryItemDTO.SegmentCategoryId = -1;
                sourceDTO.InventoryItemDTO.Guid = string.Empty;
                sourceDTO.InventoryItemDTO.SiteId = sourceDTO.SiteId;
                if (sourceDTO.InventoryItemDTO.CustomDataSetId > -1 || sourceDTO.InventoryItemDTO.CustomDataSetDTO != null)
                {
                    sourceDTO.InventoryItemDTO.CustomDataSetId = -1;
                    if (sourceDTO.InventoryItemDTO.CustomDataSetDTO != null)
                    {
                        sourceDTO.InventoryItemDTO.CustomDataSetDTO.CustomDataSetId = -1;
                        sourceDTO.InventoryItemDTO.CustomDataSetDTO.Guid = string.Empty;
                        sourceDTO.InventoryItemDTO.CustomDataSetDTO.SiteId = sourceDTO.SiteId;
                        if (sourceDTO.InventoryItemDTO.CustomDataSetDTO.CustomDataDTOList != null && sourceDTO.InventoryItemDTO.CustomDataSetDTO.CustomDataDTOList.Any())
                        {
                            for (int i = 0; i < sourceDTO.InventoryItemDTO.CustomDataSetDTO.CustomDataDTOList.Count; i++)
                            {
                                sourceDTO.InventoryItemDTO.CustomDataSetDTO.CustomDataDTOList[i].CustomDataId = -1;
                                sourceDTO.InventoryItemDTO.CustomDataSetDTO.CustomDataDTOList[i].CustomDataSetId = -1;
                                sourceDTO.InventoryItemDTO.CustomDataSetDTO.CustomDataDTOList[i].Guid = string.Empty;
                                sourceDTO.InventoryItemDTO.CustomDataSetDTO.CustomDataDTOList[i].SiteId = sourceDTO.SiteId;
                            }
                        }
                    }

                }
                if (sourceDTO.InventoryItemDTO.ProductBarcodeDTOList != null && sourceDTO.InventoryItemDTO.ProductBarcodeDTOList.Any())
                {
                    sourceDTO.InventoryItemDTO.ProductBarcodeDTOList = new List<ProductBarcodeDTO>();
                    //dont copy barcodes
                }
                if (sourceDTO.InventoryItemDTO.ProductBOMDTOList != null && sourceDTO.InventoryItemDTO.ProductBOMDTOList.Any())
                {
                    for (int i = 0; i < sourceDTO.InventoryItemDTO.ProductBOMDTOList.Count; i++)
                    {
                        sourceDTO.InventoryItemDTO.ProductBOMDTOList[i].BOMId = -1;
                        sourceDTO.InventoryItemDTO.ProductBOMDTOList[i].ProductId = -1;
                        sourceDTO.InventoryItemDTO.ProductBOMDTOList[i].Guid = string.Empty;
                        sourceDTO.InventoryItemDTO.ProductBOMDTOList[i].SiteId = sourceDTO.SiteId;
                    }
                }
            }
            if (sourceDTO.CustomDataSetId > -1 || sourceDTO.CustomDataSetDTO != null)
            {

                sourceDTO.CustomDataSetId = -1;
                if (sourceDTO.CustomDataSetDTO != null)
                {
                    sourceDTO.CustomDataSetDTO.CustomDataSetId = -1;
                    sourceDTO.CustomDataSetDTO.Guid = string.Empty;
                    sourceDTO.CustomDataSetDTO.SiteId = sourceDTO.SiteId;
                    if (sourceDTO.CustomDataSetDTO.CustomDataDTOList != null && sourceDTO.CustomDataSetDTO.CustomDataDTOList.Any())
                    {
                        for (int i = 0; i < sourceDTO.CustomDataSetDTO.CustomDataDTOList.Count; i++)
                        {
                            sourceDTO.CustomDataSetDTO.CustomDataDTOList[i].CustomDataId = -1;
                            sourceDTO.CustomDataSetDTO.CustomDataDTOList[i].CustomDataSetId = -1;
                            sourceDTO.CustomDataSetDTO.CustomDataDTOList[i].Guid = string.Empty;
                            sourceDTO.CustomDataSetDTO.CustomDataDTOList[i].SiteId = sourceDTO.SiteId;
                        }
                    }
                }
            }
            if (sourceDTO.ProductSubscriptionDTO != null && sourceDTO.ProductSubscriptionDTO.ProductSubscriptionId > -1)
            {
                sourceDTO.ProductSubscriptionDTO.ProductSubscriptionId = -1;
                sourceDTO.ProductSubscriptionDTO.ProductsId = -1;
                sourceDTO.ProductSubscriptionDTO.Guid = string.Empty;
                sourceDTO.ProductSubscriptionDTO.SiteId = sourceDTO.SiteId;
            }
            log.LogMethodExit(sourceDTO);
            return sourceDTO;
        }

        private List<ProductsDTO> SetFromSiteTimeOffset(List<ProductsDTO> productsDTOList)
        {
            log.LogMethodEntry(productsDTOList);
            if (SiteContainerList.IsCorporate())
            {
                if (productsDTOList != null && productsDTOList.Any())
                {
                    for (int i = 0; i < productsDTOList.Count; i++)
                    {
                        if (productsDTOList[i].StartDate != null && productsDTOList[i].StartDate != DateTime.MinValue)
                        {
                            productsDTOList[i].StartDate = SiteContainerList.FromSiteDateTime(productsDTOList[i].SiteId, (DateTime)productsDTOList[i].StartDate);
                        }
                        if (productsDTOList[i].ExpiryDate != null && productsDTOList[i].ExpiryDate != DateTime.MinValue)
                        {
                            productsDTOList[i].ExpiryDate = SiteContainerList.FromSiteDateTime(productsDTOList[i].SiteId, (DateTime)productsDTOList[i].ExpiryDate);
                        }
                        productsDTOList[i].AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        public List<int> GetInactiveProductsToBePublihsed(int masterSiteId)
        {
            log.LogMethodEntry(masterSiteId);
            ProductsDataHandler productsDataHandler = new ProductsDataHandler();
            List<int> result = productsDataHandler.GetInactiveProductsToBePublished(masterSiteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

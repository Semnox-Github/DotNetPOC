/* Project Name -ProductsWithComboDetails
* Description  - ProductsWithComboDetails
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.70        14-Mar-2019    Guru S A               Booking phase 2 enhancement changes 
*2.70.2        15-Sep-2019   Nitin Pai         BIR Enhancement
*2.120.0     15-Apr-2021   Nitin Pai         price Override feature in tablet
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
   
    /// <summary>
    /// Summary description for FoodAndBeverageProduct
    /// </summary>
    public class ProductsWithComboDetails : ProductsStruct
    {
        /// <summary>
        /// CONSTATNT COMBOPRODUCT
        /// </summary>
        public const string COMBOPRODUCT = "ComboProduct";
        /// <summary>
        /// CONSTATNT COMBOCATEGORY
        /// </summary>
        public const string COMBOCATEGORY = "ComboCategory";
        List<PurchasedProductsStruct> comboProducts;
        List<ProductsModifierSetStruct > modifierSets;
        /// <summary>
        /// Default Constructor 
        /// </summary>
        public ProductsWithComboDetails() : base()
        {
            comboProducts = null;
            modifierSets = null;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsWithComboDetails(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage)
            : base(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage)
        {
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added translation fields for Product Name and Product Description
        public ProductsWithComboDetails(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt,double faceValue)
            : base(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity,quantityPrompt,faceValue)
        {
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added displayGroupId,isInvokeCustomerRegistration and autoGenerateCardnumber
        public ProductsWithComboDetails(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration)
            : base(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration)
        {
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added trxHeaderRemarksMandatory, trxRemarksMandatory
        public ProductsWithComboDetails(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration, string trxHeaderRemarksMandatory, string trxRemarksMandatory, string webDescription, string translatedWebDescription, string barcode)
            : base(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration, trxHeaderRemarksMandatory, trxRemarksMandatory, webDescription, translatedWebDescription, barcode)
        {
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added isTaxInclusiveFiled
        public ProductsWithComboDetails(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration, string trxHeaderRemarksMandatory, string trxRemarksMandatory, string webDescription, string translatedWebDescription, string barcode, bool isTaxInclusive, int waiverSetId, int attractionMasterScheduleId)
            : base(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration, trxHeaderRemarksMandatory, trxRemarksMandatory, webDescription, translatedWebDescription, barcode, isTaxInclusive, waiverSetId, attractionMasterScheduleId)
        {
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added isTaxInclusiveFiled
        public ProductsWithComboDetails(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration, string trxHeaderRemarksMandatory, string trxRemarksMandatory, string webDescription, string translatedWebDescription, string barcode, bool isTaxInclusive, int waiverSetId, int attractionMasterScheduleId, bool isGroupMeal, bool allowPriceOverride)
            : base(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration, trxHeaderRemarksMandatory, trxRemarksMandatory, webDescription, translatedWebDescription, barcode, isTaxInclusive, waiverSetId, attractionMasterScheduleId, isGroupMeal, allowPriceOverride)
        {
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added SiteId
        public ProductsWithComboDetails(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration, string trxHeaderRemarksMandatory, string trxRemarksMandatory, string webDescription, string translatedWebDescription, string barcode, bool isTaxInclusive, int waiverSetId, int attractionMasterScheduleId, bool isGroupMeal, bool allowPriceOverride, int siteId)
            : base(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration, trxHeaderRemarksMandatory, trxRemarksMandatory, webDescription, translatedWebDescription, barcode, isTaxInclusive, waiverSetId, attractionMasterScheduleId, isGroupMeal, allowPriceOverride, siteId)
        {
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public void AddComboProduct(int productId, string productName, int quantity, string productType)
        {
            if (comboProducts == null)
                comboProducts = new List<PurchasedProductsStruct>();
            comboProducts.Add(new PurchasedProductsStruct(productId, productName, "", productType, "", quantity, 0, 0, ""));
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public void AddModifierProduct(int modifierSetId, string modifierSetName, string modifierShowInPOS, int modifierChildProductId, int minQuantity, int maxQuantity, int freeQuantity)
        {
            if (modifierSets == null)
                modifierSets = new List<ProductsModifierSetStruct >();
            bool modifierSetFound = false;
            ProductsModifierSetStruct  currModifierSet = null;
            for (int i = 0; (i < modifierSets.Count) && (modifierSetFound == false); i++)
            {
                if (modifierSetId == modifierSets[i].ModifierSetId) 
                {
                    modifierSetFound = true;
                    currModifierSet = modifierSets[i];
                }
            }
            if (modifierSetFound == false)
            {
                currModifierSet = new ProductsModifierSetStruct(modifierSetId, modifierSetName, modifierShowInPOS, minQuantity, maxQuantity, freeQuantity);
                modifierSets.Add(currModifierSet);
                modifierSetFound = true;
            }
            currModifierSet.AddModifier(modifierChildProductId);
        }


        /// <summary>
        ///   Constructor With parameter
        ///   Added to handle parent modifier set details
        /// </summary>
        public void AddModifierProduct(int modifierSetId, string modifierSetName, string modifierShowInPOS, int modifierChildProductId, string modifierChildProductPrice, int minQuantity, int maxQuantity, int freeQuantity, ProductsModifierSetStruct parentModifierSet)
        {
            if (modifierSets == null)
                modifierSets = new List<ProductsModifierSetStruct>();
            bool modifierSetFound = false;
            ProductsModifierSetStruct currModifierSet = null;
            for (int i = 0; (i < modifierSets.Count) && (modifierSetFound == false); i++)
            {
                if (modifierSetId == modifierSets[i].ModifierSetId)
                {
                    modifierSetFound = true;
                    currModifierSet = modifierSets[i];
                }
            }
            if (modifierSetFound == false)
            {
                currModifierSet = new ProductsModifierSetStruct(modifierSetId, modifierSetName, modifierShowInPOS, minQuantity, maxQuantity, freeQuantity, parentModifierSet);
                modifierSets.Add(currModifierSet);
                modifierSetFound = true;
            }
            currModifierSet.AddModifier(modifierChildProductId, "", Double.Parse(modifierChildProductPrice));
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public List<PurchasedProductsStruct> ComboProducts { get { return comboProducts; } set {comboProducts=value;} }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public List<ProductsModifierSetStruct > ModifierSets { get { return modifierSets; } }
    }

   

    public class ProductsWithComboDetailsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductsWithComboDetailsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// method which returns the ProductsWithComboDetails object by using product id.
        /// </summary>
        /// <param name="productsFilterParams">product list fetchparams</param>
        public ProductsWithComboDetails GetProductStructById(ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(productsFilterParams);
            ProductsWithComboDetailsDatahandler productsWithComboDetailsDatahandler = new ProductsWithComboDetailsDatahandler(null);
            ProductsWithComboDetails productsWithComboDetails = productsWithComboDetailsDatahandler.GetProductList(productsFilterParams)[0];
            log.LogMethodExit(productsWithComboDetails);
            return productsWithComboDetails;
        }

        /// <summary>
        /// This method is used to get products list used for purchase
        /// </summary>
        /// <param name="productsFilterParams">ProductsFilterParams type</param>
        /// <returns>returns ProductsWithComboDetails List with the product details.</returns>
        public List<ProductsWithComboDetails> GetProductsList(ProductsFilterParams productsFilterParams)
        {
            log.Debug("Starts-GetProductsList(productsFilterParams) method.");

            if (new Site.SiteList(null).GetMasterSiteFromHQ() != null)
            {
                executionContext.SetSiteId(productsFilterParams.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }

            if (productsFilterParams.LanguageId == -1 && productsFilterParams.LanguageCode != "")
            {
                Languages.Languages languages = new Languages.Languages(executionContext);
                List<KeyValuePair<Languages.LanguagesDTO.SearchByParameters, string>> languageSerachParam = new List<KeyValuePair<Languages.LanguagesDTO.SearchByParameters, string>>();
                languageSerachParam.Add(new KeyValuePair<Languages.LanguagesDTO.SearchByParameters, string>(Languages.LanguagesDTO.SearchByParameters.LANGUAGE_CODE, productsFilterParams.LanguageCode));
                List<Languages.LanguagesDTO> langaugesList = languages.GetAllLanguagesList(languageSerachParam);
                if (langaugesList != null && langaugesList.Any())
                {
                    productsFilterParams.LanguageId = langaugesList[0].LanguageId;
                }
                else
                {
                    log.Debug("Error-GetProductsList(productsFilterParams) Language code Not exist ");
                    throw new Exception("Language code Not exist ");
                }
            }
            else
            {
                productsFilterParams.LanguageId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "DEFAULT_LANGUAGE");
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

            ProductsWithComboDetailsDatahandler productsWithComboDetailsDatahandler = new ProductsWithComboDetailsDatahandler(null);
            List<ProductsWithComboDetails> foodAndBeverageProductList = new List<ProductsWithComboDetails>();
            foodAndBeverageProductList = productsWithComboDetailsDatahandler.GetProductList(productsFilterParams);

            log.LogMethodExit(foodAndBeverageProductList);
            // update Promotional Price to Products 
            return productsWithComboDetailsDatahandler.GetPromotionalPriceUpdate(foodAndBeverageProductList, productsFilterParams);
        }
    }
}

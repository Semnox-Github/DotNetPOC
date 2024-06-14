//MOved to transaction project do not use this 
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel; 

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
            : base(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription,productMinQuantity,productMaxQuantity,quantityPrompt,faceValue)
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

    /// <summary>
    /// class ProductsModifierSetStruct 
    /// </summary>
    [Serializable]
    public class ProductsModifierSetStruct 
    {
        string modifierSetName;
        int modifierSetId;
        string modifierShowInPOS;
        int minQuantity;
        int maxQuantity;
        List<ProductsStruct> modifiers;
        int freeQuantity;

        /// <summary>
        /// Added for storing the parent modifier set details
        /// </summary>
        public ProductsModifierSetStruct parentModifierSet;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public ProductsModifierSetStruct ()
        {
            modifierSetName = "";
            modifiers = null;
            parentModifierSet = null;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsModifierSetStruct (int modifierSetId, string modifierSetName, string modifierShowInPOS)
        {
            this.modifierSetId = modifierSetId;
            this.modifierSetName = modifierSetName;
            this.modifierShowInPOS = modifierShowInPOS;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //new constructor to add min and max quantity - added on 03-Nov-2015
        public ProductsModifierSetStruct (int modifierSetId, string modifierSetName, string modifierShowInPOS, int minQuantity, int maxQuantity, int freeQuantity)
        {
            this.modifierSetId = modifierSetId;
            this.modifierSetName = modifierSetName;
            this.modifierShowInPOS = modifierShowInPOS;
            this.minQuantity = minQuantity;
            this.maxQuantity = maxQuantity;
            this.freeQuantity = freeQuantity;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //new constructor to add parent modifier set
        public ProductsModifierSetStruct(int modifierSetId, string modifierSetName, string modifierShowInPOS, int minQuantity, int maxQuantity, int freeQuantity, ProductsModifierSetStruct parentModifierSet)
        {
            this.modifierSetId = modifierSetId;
            this.modifierSetName = modifierSetName;
            this.modifierShowInPOS = modifierShowInPOS;
            this.minQuantity = minQuantity;
            this.maxQuantity = maxQuantity;
            this.freeQuantity = freeQuantity;
            this.parentModifierSet = parentModifierSet;
        }

        /// <summary>
        /// AddModifier method
        /// </summary>
        /// <param name="productId">productId</param>
        public void AddModifier(int productId)
        {
            if (modifiers == null)
                modifiers = new List<ProductsStruct>();
            modifiers.Add(new ProductsStruct(productId));
        }

        /// <summary>
        /// AddModifier method
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="productName">productName</param>
        public void AddModifier(int productId, string productName)
        {
            if (modifiers == null)
                modifiers = new List<ProductsStruct>();
            modifiers.Add(new ProductsStruct(productId, productName, 0.0));
        }

        /// <summary>
        /// AddModifier method
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="productName">productName</param>
        /// <param name="price">productPrice</param>
        public void AddModifier(int productId, string productName, Double price)
        {
            if (modifiers == null)
                modifiers = new List<ProductsStruct>();
            modifiers.Add(new ProductsStruct(productId, productName, price));
        }

        /// <summary>
        /// Get/Set method of the ModifierSetName field
        /// </summary>
        public string ModifierSetName { get { return modifierSetName; } set { modifierSetName = value; } }

        /// <summary>
        /// Get/Set method of the ModifierSetId field
        /// </summary>
        public int ModifierSetId { get { return modifierSetId; } set { modifierSetId = value; } }

        /// <summary>
        /// Get/Set method of the ModifierSetShowInPOS field
        /// </summary>
        public string ModifierSetShowInPOS { get { return modifierShowInPOS; } set { modifierShowInPOS = value; } }

        /// <summary>
        /// Get/Set method of the MinQuantity field
        /// </summary>
        public int MinQuantity { get { return minQuantity; } set { minQuantity = value; } }

        /// <summary>
        /// Get/Set method of the MaxQuantity field
        /// </summary>
        public int MaxQuantity { get { return maxQuantity; } set { maxQuantity = value; } }

        /// <summary>
        /// Get/Set method of the Modifiers field
        /// </summary>
        public List<ProductsStruct> Modifiers { get { return modifiers; } }

        /// <summary>
        /// Get/Set method of the FreeQuantity field
        /// </summary>
        [DisplayName("FreeQuantity")]
        public int FreeQuantity { get { return freeQuantity; } set { freeQuantity = value; } }
        
	/// <summary>
        /// Get/Set method of the ParentModifierSet
        /// </summary>
        [DisplayName("ParentModifierSet")]
        public ProductsModifierSetStruct ParentModifierSet { get { return parentModifierSet; } }
    }
}

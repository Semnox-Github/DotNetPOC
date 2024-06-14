/********************************************************************************************
 * Project Name - LinkedPurchaseProductsStruct DTO
 * Description  - Data object of LinkedPurchaseProductsStruct 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   Rakshith          Created 
 *2.70.0      22-Apr-2019   Guru S A          Moved the PurchasedProductsStruct from Product project to transaction project
 *2.70.0      01-Aug-2019   Nitin Pai         Added ComboProductType to product struct
 *2.70.2.0      15-Sep-2019   Nitin Pai         BIR Enhancement
 *2.120.0     15-Apr-2021   Nitin Pai         price Override feature in tablet
 *2.90.0.2    15-Dec-2022   Muaaz Musthafa    Added Product siteId
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the ProductDisplayGroup data object class. This acts as data holder for the product business object
    /// </summary>
    public class ProductDisplayGroup
    {
        private string displayGroupName;
        private string displayGroupDescription;
        private string displayGroupImageName;
        private string sortOrder;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public ProductDisplayGroup()
        {
            displayGroupName = "";
            displayGroupDescription = "";
            displayGroupImageName = "";
            sortOrder = "";
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductDisplayGroup(string displayGroupName, string displayGroupDescription, string displayGroupImageName, string sortOrder)
        {
            this.displayGroupName = displayGroupName;
            this.displayGroupDescription = displayGroupDescription;
            this.displayGroupImageName = displayGroupImageName;
            this.sortOrder = sortOrder;
        }

        /// <summary>
        /// Get/Set method of the DisplayGroupName field
        /// </summary>
        public string DisplayGroupName { get { return displayGroupName; } set { displayGroupName = value; } }

        /// <summary>
        /// Get/Set method of the DisplayGroupDescription field
        /// </summary>
        public string DisplayGroupDescription { get { return displayGroupDescription; } set { displayGroupDescription = value; } }

        /// <summary>
        /// Get/Set method of the DisplayGroupImageName field
        /// </summary>
        public string DisplayGroupImageName { get { return displayGroupImageName; } set { displayGroupImageName = value; } }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        public string SortOrder { get { return sortOrder; } set { sortOrder = value; } }
    }
    /// <summary>
    /// Summary description for ProductsStruct
    /// </summary>
    public class ProductsStruct
    {
        private int productId;
        private string productName;
        private string productDescription;
        private string productType;
        private string displayGroup;
        private double price;
        private double taxPercentage;
        private int categoryId;
        private string categoryName;
        private string buttonColor;
        private string textColor;
        private string displayInPos;
        private string productImage;
        //Added additional parameters to get the credits, time, bonus and courtesy of products
        private double credits;
        private double courtesy;
        private double time;
        private double bonus;
        private string cardSale;
        private string guid;
        //Added translation specific parameters
        private string translatedProductName;
        private string translatedProductDescription;
        private int minQuantity;
        private int maxQuantity;
        private string quantityPrompt;
        private double faceValue;
        //added Tablet required parameters
        private int displayGroupId;
        private bool isInvokeCustomerRegistration;
        private string autoGenerateCardNumber;
        private string trxHeaderRemarksMandatory;
        private string trxRemarksMandatory;
        private string webDescription;
        private string translatedWebDescription;

        private string barcode;
        private bool isTaxInclusive;

		private int waiverSetId;
		private int attractionMasterScheduleId;

        private bool isGroupMeal;
        private bool allowPriceOverride;
        private int siteId;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public ProductsStruct()
		{
			this.waiverSetId = -1;
			this.attractionMasterScheduleId = -1;
            this.isGroupMeal = false;
            this.allowPriceOverride = false;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsStruct(int productId)
            : this()
        {
            this.productId = productId;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsStruct(int productId, string description)
            :this()
        {
            this.productId = productId;
            productDescription = description;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsStruct(int productId, string productName, Double price)
            : this()
        {
            this.productId = productId;
            this.productName = productName;
            this.price = price;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup) 
            : this(productId, productDescription)
        {
            this.productName = productName;
            this.productType = productType;
            this.displayGroup = displayGroup;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int siteId)
            : this(productId, productName, productDescription, productType, displayGroup)
        {
            this.siteId = siteId;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsStruct(int productId, string productName, string productDescription, string productType, Double price, string displayGroup, string productImage)
            : this(productId, productName, productDescription, productType, displayGroup)
        {
            this.productImage = productImage;
            this.price = price;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string buttonColor, string textColor, string displayInPos, string cardSale)
            : this(productId, productName, productDescription, productType, displayGroup)
        {
            this.price = price;
            this.taxPercentage = taxPercentage;
            this.categoryId = categoryId;
            this.buttonColor = buttonColor;
            this.textColor = textColor;
            this.displayInPos = displayInPos;
            this.cardSale = cardSale;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage)
            : this(productId, productName, productDescription, productType, displayGroup)
        {
            this.price = price;
            this.taxPercentage = taxPercentage;
            this.categoryId = categoryId;
            this.buttonColor = buttonColor;
            this.textColor = textColor;
            this.displayInPos = displayInPos;
            this.cardSale = cardSale;
            this.productImage = productImage;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added CategoryName to the struct
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage)
            : this(productId, productName, productDescription, productType, displayGroup)
        {
            this.price = price;
            this.taxPercentage = taxPercentage;
            this.categoryId = categoryId;
            this.categoryName = categoryName;
            this.buttonColor = buttonColor;
            this.textColor = textColor;
            this.displayInPos = displayInPos;
            this.cardSale = cardSale;
            this.productImage = productImage;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added to get the all the available Products in the POS system 
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double credits, double courtesy, double bonus, double time, string cardSale)
            : this(productId, productName, productDescription, productType, displayGroup)
        {
            this.price = price;
            this.credits = credits;
            this.courtesy = courtesy;
            this.bonus = bonus;
            this.time = time;
            this.cardSale = cardSale;
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added Translation Name and Translation Description to the struct
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt,double faceValue)
            : this(productId, productName, productDescription, productType, displayGroup)
        {
            this.price = price;
            this.taxPercentage = taxPercentage;
            this.categoryId = categoryId;
            this.categoryName = categoryName;
            this.buttonColor = buttonColor;
            this.textColor = textColor;
            this.displayInPos = displayInPos;
            this.cardSale = cardSale;
            this.productImage = productImage;
            this.translatedProductName = translatedProductName;
            this.translatedProductDescription = translatedProductDescription;
            this.minQuantity = productMinQuantity;
            this.maxQuantity = productMaxQuantity;
            this.quantityPrompt = quantityPrompt;
            this.faceValue = faceValue;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added displayGroupId,invokeCustomerRegistration and autoGenerateCardnumber
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration)
            : this(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue)
        {
            this.displayGroupId = displayGroupId;
            this.autoGenerateCardNumber = autoGenerateCardNumber;
            this.isInvokeCustomerRegistration = isInvokeCustomerRegistration;

        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added trxHeaderRemarksMandatory, trxRemarksMandatory, webDescription, translatedWebDescription
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration, string trxHeaderRemarksMandatory, string trxRemarksMandatory, string webDescription, string translatedWebDescription, string barcode)
            : this(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration)
        {
            this.trxHeaderRemarksMandatory = trxHeaderRemarksMandatory;
            this.trxRemarksMandatory = trxRemarksMandatory;
            this.webDescription = webDescription;
            this.translatedWebDescription = translatedWebDescription;
            this.barcode = barcode;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added IsTaxInclusive
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration, string trxHeaderRemarksMandatory, string trxRemarksMandatory, string webDescription, string translatedWebDescription, string barcode, bool isTaxInclusive, int waiverSetId, int attractionMasterSchedule)
            : this(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration, trxHeaderRemarksMandatory, trxRemarksMandatory, webDescription, translatedWebDescription, barcode)
        {
            this.isTaxInclusive = isTaxInclusive;
            this.waiverSetId = waiverSetId;
			this.attractionMasterScheduleId = attractionMasterSchedule;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added IsTaxInclusive
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration, string trxHeaderRemarksMandatory, string trxRemarksMandatory, string webDescription, string translatedWebDescription, string barcode, bool isTaxInclusive, int waiverSetId, int attractionMasterSchedule, bool isGroupMeal, bool allowPriceOverride)
            : this(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration, trxHeaderRemarksMandatory, trxRemarksMandatory, webDescription, translatedWebDescription, barcode, isTaxInclusive, waiverSetId, attractionMasterSchedule)
        {
            this.isGroupMeal = isGroupMeal;
            this.allowPriceOverride = allowPriceOverride;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added SiteId
        public ProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, Double price, double taxPercentage, int categoryId, string categoryName, string buttonColor, string textColor, string displayInPos, string cardSale, string productImage, string translatedProductName, string translatedProductDescription, int productMinQuantity, int productMaxQuantity, string quantityPrompt, double faceValue, int displayGroupId, string autoGenerateCardNumber, bool isInvokeCustomerRegistration, string trxHeaderRemarksMandatory, string trxRemarksMandatory, string webDescription, string translatedWebDescription, string barcode, bool isTaxInclusive, int waiverSetId, int attractionMasterSchedule, bool isGroupMeal, bool allowPriceOverride, int siteId)
            : this(productId, productName, productDescription, productType, displayGroup, price, taxPercentage, categoryId, categoryName, buttonColor, textColor, displayInPos, cardSale, productImage, translatedProductName, translatedProductDescription, productMinQuantity, productMaxQuantity, quantityPrompt, faceValue, displayGroupId, autoGenerateCardNumber, isInvokeCustomerRegistration, trxHeaderRemarksMandatory, trxRemarksMandatory, webDescription, translatedWebDescription, barcode, isTaxInclusive, waiverSetId, attractionMasterSchedule, isGroupMeal, allowPriceOverride)
        {
            this.siteId = siteId;
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; } }

        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        public string ProductName { get { return productName; } set { productName = value; } }

        /// <summary>
        /// Get/Set method of the ProductDescription field
        /// </summary>
        public string ProductDescription { get { return productDescription; } set { productDescription = value; } }

        /// <summary>
        /// Get/Set method of the ProductType field
        /// </summary>
        public string ProductType { get { return productType; } set { productType = value; } }

        /// <summary>
        /// Get/Set method of the LinkLineId field
        /// </summary>
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value; } }

        /// <summary>
        /// Get/Set method of the LinkLineId field
        /// </summary>
        public Double Price { get { return price; } set { price = value; } }

        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; } }

        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }

        /// <summary>
        /// Get/Set method of the CategoryName field
        /// </summary>
        public string CategoryName { get { return categoryName; } set { categoryName = value; } }

        /// <summary>
        /// Get/Set method of the ButtonColor field
        /// </summary>
        public string ButtonColor { get { return buttonColor; } set { buttonColor = value; } }

        /// <summary>
        /// Get/Set method of the TextColor field
        /// </summary>
        public string TextColor { get { return textColor; } set { textColor = value; } }

        /// <summary>
        /// Get/Set method of the LinkLineId field
        /// </summary>
        public string DisplayInPos { get { return displayInPos; } set { displayInPos = value; } }

        /// <summary>
        /// Get/Set method of the LinkLineId field
        /// </summary>
        public string ProductImage { get { return productImage; } set { productImage = value; } }

        /// <summary>
        /// Get/Set method of the Credits field
        /// </summary>
        //Added additional parameters to get the credits, time, bonus and courtesy of products
        public double Credits { get { return credits; } set { credits = value; } }

        /// <summary>
        /// Get/Set method of the Courtesy field
        /// </summary>
        public double Courtesy { get { return courtesy; } set { courtesy = value; } }

        /// <summary>
        /// Get/Set method of the Bonus field
        /// </summary>
        public double Bonus { get { return bonus; } set { bonus = value; } }

        /// <summary>
        /// Get/Set method of the Time field
        /// </summary>
        public double Time { get { return time; } set { time = value; } }

        /// <summary>
        /// Get/Set method of the CardSale field
        /// </summary>
        public string CardSale { get { return cardSale; } set { cardSale = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the TranslatedProductName field
        /// </summary>
        public string TranslatedProductName { get { return translatedProductName; } set { translatedProductName = value; } }

        /// <summary>
        /// Get/Set method of the LinkLineId field
        /// </summary>
        public string TranslatedProductDescription
        {
            get { return translatedProductDescription; }
            set { translatedProductDescription = value; }
        }

        /// <summary>
        /// Get/Set method of the MinQuantity field
        /// </summary>
        public int MinQuantity { get { return minQuantity; } set { minQuantity = value; } }

        /// <summary>
        /// Get/Set method of the MaxQuantity field
        /// </summary>
        [DefaultValue(0)]
        public int MaxQuantity { get { return maxQuantity; } set { maxQuantity = value; } }

        /// <summary>
        /// Get/Set method of the QuantityPrompt field
        /// </summary>
        public string QuantityPrompt { get { return quantityPrompt; } set { quantityPrompt = value; } }

        /// <summary>
        /// Get/Set method of the FaceValue field
        /// </summary>
        [DefaultValue(0)]
        public double FaceValue { get { return faceValue; } set { faceValue = value; } }

        /// <summary>
        /// Get/Set method of the DisplayGroupId field
        /// </summary>
        public int DisplayGroupId
        {
            get { return displayGroupId; }
            set { displayGroupId = value; }
        }

        /// <summary>
        /// Get/Set method of the IsInvokeCustomerRegistration field
        /// </summary>
        public bool IsInvokeCustomerRegistration
        {
            get { return isInvokeCustomerRegistration; }
            set { isInvokeCustomerRegistration = value; }
        }

        /// <summary>
        /// Get/Set method of the AutoGenerateCardNumber field
        /// </summary>
        public string AutoGenerateCardNumber
        {
            get { return autoGenerateCardNumber; }
            set { autoGenerateCardNumber = value; }
        }

        /// <summary>
        /// Get/Set method of the TrxHeaderRemarksMandatory field
        /// </summary>
        public string TrxHeaderRemarksMandatory
        {
            get { return trxHeaderRemarksMandatory; }
            set { trxHeaderRemarksMandatory = value; }
        }

        /// <summary>
        /// Get/Set method of the TrxRemarksMandatory field
        /// </summary>
        public string TrxRemarksMandatory
        {
            get { return trxRemarksMandatory; }
            set { trxRemarksMandatory = value; }
        }

        /// <summary>
        /// Get/Set method of the WebDescription field
        /// </summary>
        public string WebDescription { get { return webDescription; } set { webDescription = value; } }


        /// <summary>
        /// Get/Set method of the TranslatedWebDescription field
        /// </summary>
        public string TranslatedWebDescription { get { return translatedWebDescription; } set { translatedWebDescription = value; } }

        /// <summary>
        /// Get/Set method of the Barcode field
        /// </summary>
        public string Barcode { get { return barcode; } set { barcode = value; } }

        /// <summary>
        /// Get/Set method of the IsTaxInclusive field
        /// </summary>
        public bool IsTaxInclusive { get { return isTaxInclusive; } set { isTaxInclusive = value; } }


        /// <summary>
        /// WaiverSetId
        /// </summary>
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; } }

		/// <summary>
		/// WaiverSetId
		/// </summary>
		public int AttractionMasterScheduleId { get { return attractionMasterScheduleId; } set { attractionMasterScheduleId = value; } }


        /// <summary>
        /// Get/Set method of the IsGroupMeal field
        /// </summary>
        public bool IsGroupMeal { get { return isGroupMeal; } set { isGroupMeal = value; } }

        /// <summary>
        /// Get/Set method of the IsGroupMeal field
        /// </summary>
        public bool AllowPriceOverride { get { return allowPriceOverride; } set { allowPriceOverride = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }


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
        public ProductsModifierSetStruct()
        {
            modifierSetName = "";
            modifiers = null;
            parentModifierSet = null;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public ProductsModifierSetStruct(int modifierSetId, string modifierSetName, string modifierShowInPOS)
        {
            this.modifierSetId = modifierSetId;
            this.modifierSetName = modifierSetName;
            this.modifierShowInPOS = modifierShowInPOS;
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //new constructor to add min and max quantity - added on 03-Nov-2015
        public ProductsModifierSetStruct(int modifierSetId, string modifierSetName, string modifierShowInPOS, int minQuantity, int maxQuantity, int freeQuantity)
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

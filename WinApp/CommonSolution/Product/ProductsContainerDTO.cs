/********************************************************************************************
 * Project Name - Products
 * Description  - ProductsConatinerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By      Remarks          
 *********************************************************************************************
 2.110.0      01-Dec-2020       Deeksha          Created : POS/Web Inventory UI Redesign with REST API
 2.140.0      23-June-2021      Prashanth V      Modified : Added StartTime and ExpiryTime fields
 2.140.0      14-Sep-2021       Prajwal S         Modified : Added More fields and child containers
 *2.150.0     28-Mar-2022       Girish Kundar     Modified : Added  new columns  MaximumQuantity & PauseType to Products
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int product_id;
        private string product_name;
        private string productType;
        private string description;
        private decimal price;
        private int product_type_id;
        private int categoryId;
        private decimal sort_order;
        private int site_id;
        private string autoGenerateCardNumber;
        private string quantityPrompt;
        private string onlyForVIP;
        private string allowPriceOverride;
        private string registeredCustomerOnly;
        private string managerApprovalRequired;
        private string verifiedCustomerOnly;
        private int minimumQuantity;
        private int? maxQtyPerDay;
        private string webDescription;
        private int availableUnits;
        private string translatedProductName;
        private string translatedProductDescription;
        private string guid;
        private bool trxHeaderRemarksMandatory;
        private string imageFileName;
        private decimal advancePercentage;
        private decimal advanceAmount;
        private string waiverRequired;
        private bool invokeCustomerRegistration;
        private int waiverSetId;
        private bool loadToSingleCard;
        private string isGroupMeal;
        private bool isSystemProduct;
        private bool isActive;
        private int posTypeId;
        private string searchDescription;
        private bool isRecommended;
        private string displayInPOS;
        private string ticketAllowed;
        private string externalSystemReference;
        private List<ComboProductContainerDTO> comboProductContainerDTOList;
        private List<ProductModifierContainerDTO> productModifierContainerDTOList;
        private List<ProductsDisplayGroupContainerDTO> productsDisplayGroupContainerDTOList;
        private InventoryItemContainerDTO inventoryItemContainerDTO;
        private ProductSubscriptionContainerDTO productSubscriptionContainerDTO;
        private bool? issueNotificationDevice;
        private int notificationTagProfileId;
        private List<UpsellOffersContainerDTO> upsellOffersContainerDTOList;
        private List<UpsellOffersContainerDTO> crossSellProductsContainerDTOList;
        private List<CustomDataContainerDTO> customDataContainerDTOList;
        private DateTime? startTime;
        private DateTime? expiryTime;
        private decimal taxPercentage;
        private int taxId;
        private int orderTypeId;
        private int cardCount;
        private string trxRemarksMandatory;
        private decimal tickets;
        private decimal faceValue;
        private string displayGroup;
        private string vipCard;
        private string taxInclusivePrice;
        private string inventoryProductCode;
        private DateTime expiryDate;
        private string autoCheckOut;
        private int checkInFacilityId;
        private decimal maxCheckOutAmount;
        private int customDataSetId;
        private int cardTypeId;
        private int overridePrintTemplateId;
        private DateTime startDate;
        private string buttonColor;
        private decimal minimumUserPrice;
        private string textColor;
        private string font;
        private string modifier;
        private int emailTemplateId;
        private int maximumTime;
        private int minimumTime;
        private int cardValidFor;
        private string additionalTaxInclusive;
        private decimal additionalPrice;
        private int additionalTaxId;
        private int segmentCategoryId;
        private DateTime cardExpiryDate;
        private int productDisplayGroupFormatId;
        private bool enableVariableLockerHours;
        private string categoryName;
        private string cardSale;
        private string zoneCode;
        private string lockerMode;
        private string taxName;
        private bool? usedInDiscounts;
        private int? creditPlusConsumptionId;
        private string mapedDisplayGroup;
        private bool linkChildCard;
        private string licenseType;
        private int zoneId;
        private double lockerExpiryInHours;
        private DateTime lockerExpiryDate;
        private string hsnSacCode;
        private int? membershipId;
        private bool isSellable;
        private decimal? serviceCharge;
        private decimal? packingCharge;
        private decimal bonus;
        private decimal credits;
        private decimal courtesy;
        private decimal time;
        private bool serviceChargeIsApplicable;
        private bool gratuityIsApplicable;
        private decimal? serviceChargePercentage;
        private decimal? gratuityPercentage;        
        private int? maximumQuantity;
        private PauseUnPauseType pauseType;
        private int customerProfilingGroupId;
        private CustomerProfilingGroupContainerDTO customerProfilingGroupContainerDTO;
        private const int AGE_UPPER_LIMIT = 999;
        private decimal ageUpperLimit;
        private decimal ageLowerLimit;
        private List<ProductCreditPlusContainerDTO> productCreditPlusContainerDTOList;
        private List<ProductGamesContainerDTO> productGamesContainerDTOList;


        /// <summary>
        /// Enum which holds the Pause Type. Used only for Pause and Unpause operations.
        /// </summary>
        public enum PauseUnPauseType
        {
            /// <summary>
            /// NONE
            /// </summary>
            NONE,
            /// <summary>
            /// PAUSE
            /// </summary>
            PAUSE,
            /// <summary>
            /// UNPAUSE
            /// </summary>
            UNPAUSE
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsContainerDTO()
        {
            log.LogMethodEntry();
            serviceChargeIsApplicable = false;
            gratuityIsApplicable = false;
            serviceChargePercentage = null;
            gratuityPercentage = null;
            customerProfilingGroupContainerDTO = new CustomerProfilingGroupContainerDTO();
            this.productCreditPlusContainerDTOList = new List<ProductCreditPlusContainerDTO>();
            productGamesContainerDTOList = new List<ProductGamesContainerDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ProductsContainerDTO(int product_id, string product_name, string productType, string description, int product_type_id,
                                    int categoryId, decimal price, decimal sort_order, int site_id, string autoGenerateCardNumber,
                                    string quantityPrompt, string onlyForVIP, string allowPriceOverride,
                                    string registeredCustomerOnly, string managerApprovalRequired,
                                    string verifiedCustomerOnly, int minimumQuantity, bool trxHeaderRemarksMandatory,
                                    string imageFileName, decimal advancePercentage, decimal advanceAmount,
                                    string waiverRequired, bool invokeCustomerRegistration, int waiverSetId,
                                    bool loadToSingleCard, string isGroupMeal, bool isSystemProduct, bool isActive, int posTypeId,
                                    bool? issueNotificationDevice, int notificationTagProfileId, string shortName, bool isRecommended,
                                    int? maxQtyPerDay, string webDescription, int availableUnits, string translatedProductName,
                                    string translatedProductDescription, string displayInPOS, string guid, string externalSystemReference,
                                    DateTime? startTime, DateTime? expiryTime, int taxId, decimal taxPercentage, int orderTypeId, int cardCount, string trxRemarksMandatory,
                                     decimal tickets, decimal faceValue, string displayGroup, string ticketAllowed, string vipCard, string taxInclusivePrice, string inventoryProductCode, DateTime expiryDate,
                                    string autoCheckOut, int checkInFacilityId, decimal maxCheckOutAmount, int customDataSetId, int cardTypeId, int overridePrintTemplateId, DateTime startDate, string buttonColor,
                                    decimal minimumUserPrice, string textColor, string font, string modifier, int emailTemplateId, int maximumTime, int minimumTime, int cardValidFor, string additionalTaxInclusive,
                                    decimal additionalPrice, int additionalTaxId, int segmentCategoryId, DateTime cardExpiryDate, int productDisplayGroupFormatId, bool enableVariableLockerHours, string categoryName,
                                    string cardSale, string zoneCode, string lockerMode, string taxName, bool? usedInDiscounts, int? creditPlusConsumptionId, string mapedDisplayGroup, bool linkChildCard,
                                    string licenseType, int zoneId, double lockerExpiryInHours, DateTime lockerExpiryDate, string hsnSacCode, int? membershipId, bool isSellable,
                                    decimal? serviceCharge, decimal? packingCharge, decimal bonus, decimal credits, decimal courtesy, decimal time, bool serviceChargeIsApplicable,
                                    decimal? serviceChargePercentage, bool gratuityIsApplicable, decimal? gratuityPercentage, 
                                    int? maximumQuantity, PauseUnPauseType pauseType, int customerProfilingGroupId, decimal ageLowerLimit, decimal ageUpperLimit) 
           
            : this()
        {
            log.LogMethodEntry(product_id, product_name, productType, description, product_type_id,
                                    categoryId, price, sort_order, site_id, autoGenerateCardNumber,
                                    quantityPrompt, onlyForVIP, allowPriceOverride,
                                    registeredCustomerOnly, managerApprovalRequired,
                                    verifiedCustomerOnly, minimumQuantity, trxHeaderRemarksMandatory,
                                    imageFileName, advancePercentage, advanceAmount,
                                    waiverRequired, invokeCustomerRegistration, waiverSetId,
                                    loadToSingleCard, isGroupMeal, isSystemProduct, isActive, posTypeId,
                                    issueNotificationDevice, notificationTagProfileId, shortName, isRecommended,
                                    maxQtyPerDay, webDescription, availableUnits, translatedProductName,
                                    translatedProductDescription, displayInPOS, guid, externalSystemReference,
                                    startTime, expiryTime, taxId, taxPercentage, orderTypeId, cardCount, trxRemarksMandatory,
                                     tickets, faceValue, displayGroup, ticketAllowed, vipCard, taxInclusivePrice, inventoryProductCode, expiryDate,
                                    autoCheckOut, checkInFacilityId, maxCheckOutAmount, customDataSetId, cardTypeId, overridePrintTemplateId, startDate, buttonColor,
                                    minimumUserPrice, textColor, font, modifier, emailTemplateId, maximumTime, minimumTime, cardValidFor, additionalTaxInclusive,
                                    additionalPrice, additionalTaxId, segmentCategoryId, cardExpiryDate, productDisplayGroupFormatId, enableVariableLockerHours, categoryName,
                                    cardSale, zoneCode, lockerMode, taxName, usedInDiscounts, creditPlusConsumptionId, mapedDisplayGroup, linkChildCard,
                                    licenseType, zoneId, lockerExpiryInHours, lockerExpiryDate, hsnSacCode, membershipId, isSellable,
                                    serviceCharge, packingCharge, bonus, credits, courtesy, time,
                                    serviceChargeIsApplicable, serviceChargePercentage, gratuityIsApplicable, gratuityPercentage, maximumQuantity, pauseType,
                                customerProfilingGroupId, ageLowerLimit, ageUpperLimit);
            this.product_id = product_id;
            this.product_name = product_name;
            this.productType = productType;
            this.description = description;
            this.product_type_id = product_type_id;
            this.categoryId = categoryId;
            this.price = price;
            this.sort_order = sort_order;
            this.site_id = site_id;
            this.autoGenerateCardNumber = autoGenerateCardNumber;
            this.quantityPrompt = quantityPrompt;
            this.onlyForVIP = onlyForVIP;
            this.allowPriceOverride = allowPriceOverride;
            this.registeredCustomerOnly = registeredCustomerOnly;
            this.managerApprovalRequired = managerApprovalRequired;
            this.verifiedCustomerOnly = verifiedCustomerOnly;
            this.minimumQuantity = minimumQuantity;
            this.trxHeaderRemarksMandatory = trxHeaderRemarksMandatory;
            this.imageFileName = imageFileName;
            this.advancePercentage = advancePercentage;
            this.advanceAmount = advanceAmount;
            this.waiverRequired = waiverRequired;
            this.invokeCustomerRegistration = invokeCustomerRegistration;
            this.waiverSetId = waiverSetId;
            this.loadToSingleCard = loadToSingleCard;
            this.isGroupMeal = isGroupMeal;
            this.isSystemProduct = isSystemProduct;
            this.isActive = isActive;
            this.posTypeId = posTypeId;
            this.notificationTagProfileId = notificationTagProfileId;
            this.issueNotificationDevice = issueNotificationDevice;
            this.searchDescription = shortName;
            this.isRecommended = isRecommended;
            this.maxQtyPerDay = maxQtyPerDay;
            this.webDescription = webDescription;
            this.availableUnits = availableUnits;
            this.translatedProductName = translatedProductName;
            this.translatedProductDescription = translatedProductDescription;
            this.guid = guid;
            this.displayInPOS = displayInPOS;
            this.startTime = startTime;
            this.expiryTime = expiryTime;
            this.externalSystemReference = externalSystemReference;
            this.taxId = taxId;
            this.taxPercentage = taxPercentage;
            this.orderTypeId = orderTypeId;
            this.cardCount = cardCount;
            this.trxRemarksMandatory = trxRemarksMandatory;
            this.tickets = tickets;
            this.faceValue = faceValue;
            this.displayGroup = displayGroup;
            this.ticketAllowed = ticketAllowed;
            this.vipCard = vipCard;
            this.taxInclusivePrice = taxInclusivePrice;
            this.inventoryProductCode = inventoryProductCode;
            this.expiryDate = expiryDate;
            this.autoCheckOut = autoCheckOut;
            this.checkInFacilityId = checkInFacilityId;
            this.maxCheckOutAmount = maxCheckOutAmount;
            this.customDataSetId = customDataSetId;
            this.cardTypeId = cardTypeId;
            this.overridePrintTemplateId = overridePrintTemplateId;
            this.startDate = startDate;
            this.buttonColor = buttonColor;
            this.minimumUserPrice = minimumUserPrice;
            this.textColor = textColor;
            this.font = font;
            this.modifier = modifier;
            this.emailTemplateId = emailTemplateId;
            this.maximumTime = maximumTime;
            this.minimumTime = minimumTime;
            this.cardValidFor = cardValidFor;
            this.additionalTaxInclusive = additionalTaxInclusive;
            this.additionalPrice = additionalPrice;
            this.additionalTaxId = additionalTaxId;
            this.segmentCategoryId = segmentCategoryId;
            this.cardExpiryDate = cardExpiryDate;
            this.productDisplayGroupFormatId = productDisplayGroupFormatId;
            this.enableVariableLockerHours = enableVariableLockerHours;
            this.categoryName = categoryName;
            this.cardSale = cardSale;
            this.zoneCode = zoneCode;
            this.lockerMode = lockerMode;
            this.taxName = taxName;
            this.usedInDiscounts = usedInDiscounts;
            this.creditPlusConsumptionId = creditPlusConsumptionId;
            this.mapedDisplayGroup = mapedDisplayGroup;
            this.linkChildCard = linkChildCard;
            this.licenseType = licenseType;
            this.zoneId = zoneId;
            this.lockerExpiryInHours = lockerExpiryInHours;
            this.lockerExpiryDate = lockerExpiryDate;
            this.hsnSacCode = hsnSacCode;
            this.membershipId = membershipId;
            this.isSellable = isSellable;
            this.serviceCharge = serviceCharge;
            this.packingCharge = packingCharge;
            this.bonus = bonus;
            this.credits = credits;
            this.courtesy = courtesy;
            this.time = time;
            this.serviceChargeIsApplicable = serviceChargeIsApplicable;
            this.serviceChargePercentage = serviceChargePercentage;
            this.gratuityIsApplicable = gratuityIsApplicable;
            this.gratuityPercentage = gratuityPercentage;
            this.maximumQuantity = maximumQuantity;
            this.pauseType = pauseType;
            this.customerProfilingGroupId = customerProfilingGroupId;
            this.ageLowerLimit = ageLowerLimit;
            this.ageUpperLimit = ageUpperLimit;
            log.LogMethodExit();
        }

        public ProductsContainerDTO(ProductsContainerDTO productsContainerDTO)
            : this(productsContainerDTO.product_id, productsContainerDTO.product_name, productsContainerDTO.productType, productsContainerDTO.description, productsContainerDTO.product_type_id,
                                    productsContainerDTO.categoryId, productsContainerDTO.price, productsContainerDTO.sort_order, productsContainerDTO.site_id, productsContainerDTO.autoGenerateCardNumber,
                                    productsContainerDTO.quantityPrompt, productsContainerDTO.onlyForVIP, productsContainerDTO.allowPriceOverride,
                                    productsContainerDTO.registeredCustomerOnly, productsContainerDTO.managerApprovalRequired,
                                    productsContainerDTO.verifiedCustomerOnly, productsContainerDTO.minimumQuantity, productsContainerDTO.trxHeaderRemarksMandatory,
                                    productsContainerDTO.imageFileName, productsContainerDTO.advancePercentage, productsContainerDTO.advanceAmount,
                                    productsContainerDTO.waiverRequired, productsContainerDTO.invokeCustomerRegistration, productsContainerDTO.waiverSetId,
                                    productsContainerDTO.loadToSingleCard, productsContainerDTO.isGroupMeal, productsContainerDTO.isSystemProduct, productsContainerDTO.isActive, productsContainerDTO.posTypeId,
                                    productsContainerDTO.issueNotificationDevice, productsContainerDTO.notificationTagProfileId, productsContainerDTO.searchDescription,
                                    productsContainerDTO.isRecommended, productsContainerDTO.maxQtyPerDay, productsContainerDTO.webDescription, productsContainerDTO.availableUnits, productsContainerDTO.translatedProductName,
                                    productsContainerDTO.translatedProductDescription, productsContainerDTO.displayInPOS, productsContainerDTO.guid, productsContainerDTO.externalSystemReference,
                                    productsContainerDTO.startTime, productsContainerDTO.ExpiryTime, productsContainerDTO.taxId, productsContainerDTO.taxPercentage,
                                    productsContainerDTO.orderTypeId, productsContainerDTO.cardCount, productsContainerDTO.trxRemarksMandatory,
                                     productsContainerDTO.tickets, productsContainerDTO.faceValue, productsContainerDTO.displayGroup, productsContainerDTO.ticketAllowed, productsContainerDTO.vipCard, productsContainerDTO.taxInclusivePrice, productsContainerDTO.inventoryProductCode, productsContainerDTO.expiryDate,
                                    productsContainerDTO.autoCheckOut, productsContainerDTO.checkInFacilityId, productsContainerDTO.maxCheckOutAmount, productsContainerDTO.customDataSetId, productsContainerDTO.cardTypeId, productsContainerDTO.overridePrintTemplateId, productsContainerDTO.startDate, productsContainerDTO.buttonColor,
                                    productsContainerDTO.minimumUserPrice, productsContainerDTO.textColor, productsContainerDTO.font, productsContainerDTO.modifier, productsContainerDTO.emailTemplateId, productsContainerDTO.maximumTime, productsContainerDTO.minimumTime, productsContainerDTO.cardValidFor, productsContainerDTO.additionalTaxInclusive,
                                    productsContainerDTO.additionalPrice, productsContainerDTO.additionalTaxId, productsContainerDTO.segmentCategoryId, productsContainerDTO.cardExpiryDate, productsContainerDTO.productDisplayGroupFormatId, productsContainerDTO.enableVariableLockerHours, productsContainerDTO.categoryName,
                                    productsContainerDTO.cardSale, productsContainerDTO.zoneCode, productsContainerDTO.lockerMode, productsContainerDTO.taxName, productsContainerDTO.usedInDiscounts, productsContainerDTO.creditPlusConsumptionId, productsContainerDTO.mapedDisplayGroup, productsContainerDTO.linkChildCard,
                                    productsContainerDTO.licenseType, productsContainerDTO.zoneId, productsContainerDTO.lockerExpiryInHours, productsContainerDTO.lockerExpiryDate, productsContainerDTO.hsnSacCode, productsContainerDTO.membershipId, productsContainerDTO.isSellable,
                                    productsContainerDTO.serviceCharge, productsContainerDTO.packingCharge, productsContainerDTO.bonus, productsContainerDTO.credits, productsContainerDTO.courtesy, productsContainerDTO.time, productsContainerDTO.serviceChargeIsApplicable, productsContainerDTO.serviceChargePercentage,
                                    productsContainerDTO.gratuityIsApplicable, productsContainerDTO.gratuityPercentage, productsContainerDTO.MaximumQuantity, productsContainerDTO.pauseType, productsContainerDTO.customerProfilingGroupId,
                                    productsContainerDTO.AgeLowerLimit, productsContainerDTO.ageUpperLimit
                                    )
        {
            log.LogMethodEntry(productsContainerDTO);
            if (productsContainerDTO.comboProductContainerDTOList != null)
            {
                comboProductContainerDTOList = new List<ComboProductContainerDTO>();
                foreach (var comboProductContainerDTO in productsContainerDTO.comboProductContainerDTOList)
                {
                    ComboProductContainerDTO comboProductContainerDTOCopy = new ComboProductContainerDTO(comboProductContainerDTO);
                    comboProductContainerDTOList.Add(comboProductContainerDTOCopy);
                }
            }
            if (productsContainerDTO.productModifierContainerDTOList != null)
            {
                productModifierContainerDTOList = new List<ProductModifierContainerDTO>();
                foreach (var productModifierContainerDTO in productsContainerDTO.productModifierContainerDTOList)
                {
                    ProductModifierContainerDTO productModifierContainerDTOCopy = new ProductModifierContainerDTO(productModifierContainerDTO);
                    productModifierContainerDTOList.Add(productModifierContainerDTOCopy);
                }
            }
            if (productsContainerDTO.productsDisplayGroupContainerDTOList != null)
            {
                productsDisplayGroupContainerDTOList = new List<ProductsDisplayGroupContainerDTO>();
                foreach (var productsDisplayGroupContainerDTO in productsContainerDTO.productsDisplayGroupContainerDTOList)
                {
                    ProductsDisplayGroupContainerDTO productsDisplayGroupContainerDTOCopy = new ProductsDisplayGroupContainerDTO(productsDisplayGroupContainerDTO);
                    productsDisplayGroupContainerDTOList.Add(productsDisplayGroupContainerDTOCopy);
                }
            }
            if(productsContainerDTO.inventoryItemContainerDTO != null)
            {
                inventoryItemContainerDTO = new InventoryItemContainerDTO(productsContainerDTO.inventoryItemContainerDTO);
            }
            if(productsContainerDTO.productSubscriptionContainerDTO != null)
            {
                productSubscriptionContainerDTO = new ProductSubscriptionContainerDTO(productsContainerDTO.productSubscriptionContainerDTO); 
            }
            if(productsContainerDTO.upsellOffersContainerDTOList != null)
            {
                upsellOffersContainerDTOList = new List<UpsellOffersContainerDTO>();
                foreach (var upsellOffersContainerDTO in productsContainerDTO.upsellOffersContainerDTOList)
                {
                    UpsellOffersContainerDTO upsellOffersContainerDTOCopy = new UpsellOffersContainerDTO(upsellOffersContainerDTO);
                    upsellOffersContainerDTOList.Add(upsellOffersContainerDTOCopy);
                }
            }
            if(productsContainerDTO.crossSellProductsContainerDTOList != null)
            {
                crossSellProductsContainerDTOList = new List<UpsellOffersContainerDTO>();
                foreach (var crossSellProductsContainerDTO in productsContainerDTO.crossSellProductsContainerDTOList)
                {
                    UpsellOffersContainerDTO crossSellProductsContainerDTOCopy = new UpsellOffersContainerDTO(crossSellProductsContainerDTO);
                    crossSellProductsContainerDTOList.Add(crossSellProductsContainerDTOCopy);
                }
            }
            if (productsContainerDTO.customDataContainerDTOList != null)
            {
                customDataContainerDTOList = new List<CustomDataContainerDTO>();
                foreach (var customDataContainerDTO in productsContainerDTO.customDataContainerDTOList)
                {
                    CustomDataContainerDTO customDataContainerDTOCopy = new CustomDataContainerDTO(customDataContainerDTO);
                    customDataContainerDTOList.Add(customDataContainerDTOCopy);
                }
            }
            if (productsContainerDTO.productGamesContainerDTOList != null)
            {
                productGamesContainerDTOList = new List<ProductGamesContainerDTO>();
                foreach (var productGamesContainerDTO in productsContainerDTO.productGamesContainerDTOList)
                {
                    ProductGamesContainerDTO copy = new ProductGamesContainerDTO(productGamesContainerDTO);
                    productGamesContainerDTOList.Add(copy);
                }
            }
            if (productsContainerDTO.productCreditPlusContainerDTOList != null)
            {
                productCreditPlusContainerDTOList = new List<ProductCreditPlusContainerDTO>();
                foreach (var productCreditPlusContainerDTO in productsContainerDTO.productCreditPlusContainerDTOList)
                {
                    ProductCreditPlusContainerDTO copy = new ProductCreditPlusContainerDTO(productCreditPlusContainerDTO);
                    productCreditPlusContainerDTOList.Add(copy);
                }
            }
            log.LogMethodExit(productsContainerDTO);
        }

        /// <summary>
        /// Get/Set method of the Tickets field
        /// </summary>
        public decimal Tickets { get { return tickets; } set { tickets = value; } }   
        
        /// <summary>
        /// Get/Set method of the FaceValue field
        /// </summary>
        public decimal FaceValue { get { return faceValue; } set { faceValue = value; } }    
        
        /// <summary>
        /// Get/Set method of the DisplayGroup field
        /// </summary>
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value; } }
     
        /// <summary>
        /// Get/Set method of the VipCard field
        /// </summary>
        public string VipCard { get { return vipCard; } set { vipCard = value; } }

        /// <summary>
        /// Get/Set method of the TicketAllowed field
        /// </summary>
        public string TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; } }   
        
        /// <summary>
        /// Get/Set method of the TaxInclusivePrice field
        /// </summary>
        public string TaxInclusivePrice { get { return taxInclusivePrice; } set { taxInclusivePrice = value; } }  
        
        /// <summary>
        /// Get/Set method of the InventoryProductCode field
        /// </summary>
        public string InventoryProductCode { get { return inventoryProductCode; } set { inventoryProductCode = value; } }  
        
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }  
        
        /// <summary>
        /// Get/Set method of the AutoCheckOut field
        /// </summary>
        public string AutoCheckOut { get { return autoCheckOut; } set { autoCheckOut = value; } } 
        
        /// <summary>
        /// Get/Set method of the CheckInFacilityId field
        /// </summary>
        public int CheckInFacilityId { get { return checkInFacilityId; } set { checkInFacilityId = value; } }

        /// <summary>
        /// Get/Set method of the maxCheckOutAmount field
        /// </summary>
        public decimal MaxCheckOutAmount { get { return maxCheckOutAmount; } set { maxCheckOutAmount = value; } }

        /// <summary>
        /// Get/Set method of the CustomDataSetId field
        /// </summary>
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; } } 
        
        /// <summary>
        /// Get/Set method of the CardTypeId field
        /// </summary>
        public int CardTypeId { get { return cardTypeId; } set { cardTypeId = value; } } 
        
        /// <summary>
        /// Get/Set method of the OverridePrintTemplateId field
        /// </summary>
        public int OverridePrintTemplateId { get { return overridePrintTemplateId; } set { overridePrintTemplateId = value; } } 
        
        /// <summary>
        /// Get/Set method of the StartDate field
        /// </summary>
        public DateTime StartDate { get { return startDate; } set { startDate = value; } }  
        
        /// <summary>
        /// Get/Set method of the ButtonColor field
        /// </summary>
        public string ButtonColor { get { return buttonColor; } set { buttonColor = value; } }  
        
        /// <summary>
        /// Get/Set method of the MinimumUserPrice field
        /// </summary>
        public decimal MinimumUserPrice { get { return minimumUserPrice; } set { minimumUserPrice = value; } } 
        
        /// <summary>
        /// Get/Set method of the TextColor field
        /// </summary>
        public string TextColor { get { return textColor; } set { textColor = value; } } 
        
        /// <summary>
        /// Get/Set method of the Font field
        /// </summary>
        public string Font { get { return font; } set { font = value; } }  
        
        /// <summary>
        /// Get/Set method of the Modifier field
        /// </summary>
        public string Modifier { get { return modifier; } set { modifier = value; } } 
        
        /// <summary>
        /// Get/Set method of the EmailTemplateId field
        /// </summary>
        public int EmailTemplateId { get { return emailTemplateId; } set { emailTemplateId = value; } } 
        
        /// <summary>
        /// Get/Set method of the MaximumTime field
        /// </summary>
        public int MaximumTime { get { return maximumTime; } set { maximumTime = value; } }  
      
        /// <summary>
        /// Get/Set method of the MinimumTime field
        /// </summary>
        public int MinimumTime { get { return minimumTime; } set { minimumTime = value; } }  
        
        /// <summary>
        /// Get/Set method of the CardValidFor field
        /// </summary>
        public int CardValidFor { get { return cardValidFor; } set { cardValidFor = value; } }
        
        /// <summary>
        /// Get/Set method of the AdditionalPrice field
        /// </summary>
        public decimal AdditionalPrice { get { return additionalPrice; } set { additionalPrice = value; } }

        /// <summary>
        /// Get/Set method of the AdditionalTaxInclusive field
        /// </summary>
        public string AdditionalTaxInclusive { get { return additionalTaxInclusive; } set { additionalTaxInclusive = value; } }
        
        /// <summary>
        /// Get/Set method of the AdditionalTaxId field
        /// </summary>
        public int AdditionalTaxId { get { return additionalTaxId; } set { additionalTaxId = value; } }
        
        /// <summary>
        /// Get/Set method of the SegmentCategoryId field
        /// </summary>
        public int SegmentCategoryId { get { return segmentCategoryId; } set { segmentCategoryId = value; } }
        
        /// <summary>
        /// Get/Set method of the CardExpiryDate field
        /// </summary>
        public DateTime CardExpiryDate { get { return cardExpiryDate; } set { cardExpiryDate = value; } } 
        
        /// <summary>
        /// Get/Set method of the ProductDisplayGroupFormatId field
        /// </summary>
        public int ProductDisplayGroupFormatId { get { return productDisplayGroupFormatId; } set { productDisplayGroupFormatId = value; } } 
    
        /// <summary>
        /// Get/Set method of the EnableVariableLockerHours field
        /// </summary>
        public bool EnableVariableLockerHours { get { return enableVariableLockerHours; } set { enableVariableLockerHours = value; } } 
     
        /// <summary>
        /// Get/Set method of the CategoryName field
        /// </summary>
        public string CategoryName { get { return categoryName; } set { categoryName = value; } } 
        
        /// <summary>
        /// Get/Set method of the LicenseType field
        /// </summary>
        public string LicenseType { get { return licenseType; } set { licenseType = value; } }

        /// <summary>
        /// Get/Set method of the CardSale field
        /// </summary>
        public string CardSale { get { return cardSale; } set { cardSale = value; } } 
        
        /// <summary>
        /// Get/Set method of the ZoneCode field
        /// </summary>
        public string ZoneCode { get { return zoneCode; } set { zoneCode = value; } }

        /// <summary>
        /// Get/Set method of the LockerMode field
        /// </summary>
        public string LockerMode { get { return lockerMode; } set { lockerMode = value; } }

        /// <summary>
        /// Get/Set method of the TaxName field
        /// </summary>
        public string TaxName { get { return taxName; } set { taxName = value; } }

        /// <summary>
        /// Get/Set method of the UsedInDiscounts field
        /// </summary>
        public bool? UsedInDiscounts { get { return usedInDiscounts; } set { usedInDiscounts = value; } }

        /// <summary>
        /// Get/Set method of the CreditPlusConsumptionId field
        /// </summary>
        public int? CreditPlusConsumptionId { get { return creditPlusConsumptionId; } set { creditPlusConsumptionId = value; } }

        /// <summary>
        /// Get/Set method of the MapedDisplayGroup field
        /// </summary>
        public string MapedDisplayGroup { get { return mapedDisplayGroup; } set { mapedDisplayGroup = value; } }

        /// <summary>
        /// Get/Set method of the LinkChildCard field
        /// </summary>
        public bool LinkChildCard { get { return linkChildCard; } set { linkChildCard = value; } }

        /// <summary>
        /// Get/Set method of the ZoneId field
        /// </summary>
        public int ZoneId { get { return zoneId; } set { zoneId = value; } } 
        
        /// <summary>
        /// Get/Set method of the LockerExpiryInHours field
        /// </summary>
        public double LockerExpiryInHours { get { return lockerExpiryInHours; } set { lockerExpiryInHours = value; } } 
        
        /// <summary>
        /// Get/Set method of the LockerExpiryDate field
        /// </summary>
        public DateTime LockerExpiryDate { get { return lockerExpiryDate; } set { lockerExpiryDate = value; } } 
        
        /// <summary>
        /// Get/Set method of the hsnSacCode field
        /// </summary>
        public string HsnSacCode { get { return hsnSacCode; } set { hsnSacCode = value; } }
        
        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        public int? MembershipId { get { return membershipId; } set { membershipId = value; } }
        
        /// <summary>
        /// Get/Set method of the IsSellable field
        /// </summary>
        public bool IsSellable { get { return isSellable; } set { isSellable = value; } }

        /// <summary>
        /// Get/Set method of the serviceCharge field
        /// </summary>
        public decimal? ServiceCharge { get { return serviceCharge; } set { serviceCharge = value; } }

        /// <summary>
        /// Get/Set method of the PackingCharge field
        /// </summary>
        public decimal? PackingCharge { get { return packingCharge; } set { packingCharge = value; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product Id")]
        [ReadOnly(true)]
        public int ProductId { get { return product_id; } set { product_id = value; } }

        /// <summary>
        /// Get/Set method of the Product Name field
        /// </summary>
        [DisplayName("Product Name")]
        public string ProductName { get { return product_name; } set { product_name = value; } }

        /// <summary>
        /// Get/Set method of the productType field
        /// </summary>
        [DisplayName("Product Type")]
        public string ProductType { get { return productType; } set { productType = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the Product Type field
        /// </summary>
        [DisplayName("Product Type")]
        public int ProductTypeId { get { return product_type_id; } set { product_type_id = value; } }

        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        [DisplayName("Category")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public decimal Price { get { return price; } set { price = value; } }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        [DisplayName("Sort Order")]
        public decimal SortOrder { get { return sort_order; } set { sort_order = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the LastUploadTime field
        /// </summary>
        [DisplayName("Last Upload Time")]
        public string AutoGenerateCardNumber { get { return autoGenerateCardNumber; } set { autoGenerateCardNumber = value; } }

        /// <summary>
        /// Get/Set method of the QuantityPrompt field
        /// </summary>
        [DisplayName("Quantity Prompt")]
        public string QuantityPrompt { get { return quantityPrompt; } set { quantityPrompt = value; } }

        /// <summary>
        /// Get/Set method of the trxRemarksMandatory field
        /// </summary>
        [DisplayName("TrxRemarksMandatory")]
        public string TrxRemarksMandatory { get { return trxRemarksMandatory; } set { trxRemarksMandatory = value; } }

        /// <summary>
        /// Get/Set method of the OnlyForVIP field
        /// </summary>
        [DisplayName("OnlyForVIP")]
        public string OnlyForVIP { get { return onlyForVIP; } set { onlyForVIP = value; } }
        /// <summary>
        /// Get/Set method of the AllowPriceOverride field
        /// </summary>
        [DisplayName("AllowPriceOverride")]
        public string AllowPriceOverride { get { return allowPriceOverride; } set { allowPriceOverride = value; } }

        /// <summary>
        /// Get/Set method of the Registered CustomerOnly field
        /// </summary>
        [DisplayName("Registered CustomerOnly")]
        public string RegisteredCustomerOnly { get { return registeredCustomerOnly; } set { registeredCustomerOnly = value; } }

        /// <summary>
        /// Get/Set method of the Manager Approval Required field
        /// </summary>
        [DisplayName("Manager Approval Required")]
        public string ManagerApprovalRequired { get { return managerApprovalRequired; } set { managerApprovalRequired = value; } }

        /// <summary>
        /// Get/Set method of the Verified CustomerOnly field
        /// </summary>
        [DisplayName("Verified CustomerOnly")]
        public string VerifiedCustomerOnly { get { return verifiedCustomerOnly; } set { verifiedCustomerOnly = value; } }

        /// <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        [DisplayName("ExternalSystemReference")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; } }

        /// <summary>
        /// Get/Set method of the Minimum Quantity field
        /// </summary>
        [DisplayName("Minimum Quantity")]
        public int MinimumQuantity { get { return minimumQuantity; } set { minimumQuantity = value; } }

        /// <summary>
        /// Get/Set method of the CardCount field
        /// </summary>
        [DisplayName("CardCount")]
        public int CardCount { get { return cardCount; } set { cardCount = value; } }

        /// <summary>
        /// Get/Set method of the TrxHeaderRemarksMandatory field
        /// </summary>
        [DisplayName("TrxHeaderRemarksMandatory")]
        public bool TrxHeaderRemarksMandatory { get { return trxHeaderRemarksMandatory; } set { trxHeaderRemarksMandatory = value; } }

        /// Get/Set method of the ImageFileName field
        /// </summary>
        [DisplayName("ImageFileName")]
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; } }

        /// <summary>
        /// Get/Set method of the AdvancePercentage field
        /// </summary>
        [DisplayName("AdvancePercentage")]
        public decimal AdvancePercentage { get { return advancePercentage; } set { advancePercentage = value; } }

        /// <summary>
		/// Get/Set method of the AdvanceAmount field
		/// </summary>
		[DisplayName("AdvanceAmount")]
        public decimal AdvanceAmount { get { return advanceAmount; } set { advanceAmount = value; } }

        /// <summary>
        /// Get/Set method of the WaiverRequired field
        /// </summary>
        [DisplayName("WaiverRequired")]
        public string WaiverRequired { get { return waiverRequired; } set { waiverRequired = value; } }


        /// <summary>
        /// Get/Set method of the InvokeCustomerRegistration field
        /// </summary>
        [DisplayName("InvokeCustomerRegistration")]
        public bool InvokeCustomerRegistration { get { return invokeCustomerRegistration; } set { invokeCustomerRegistration = value; } }

        /// <summary>
        /// Get/Set method of the DisplayInPOS field
        /// </summary>
        [DisplayName("DisplayInPOS")]
        public string DisplayInPOS { get { return displayInPOS; } set { displayInPOS = value; } }
        ///<summary>
        ///Get/Set method of the taxId field
        ///</summary>
        public int TaxId
        {
            get
            {
                return taxId;
            }
            set
            {
                taxId = value;
            }
        }

        ///<summary>
        ///Get/Set method of the taxPercentage field
        ///</summary>
        public decimal TaxPercentage
        {
            get
            {
                return taxPercentage;
            }
            set
            {
                taxPercentage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the WaiverSetId field
        /// </summary>
        [DisplayName("WaiverSetId")]
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; } }

        /// <summary>
        /// Get/Set method of the Load To Single Card field
        /// </summary>
        [DisplayName("LoadToSingleCard")]
        public bool LoadToSingleCard { get { return loadToSingleCard; } set { loadToSingleCard = value; } }

        /// <summary>
		/// Get/Set method of the IsGroupMeal field
		/// </summary>
		[DisplayName("IsGroupMeal")]
        public string IsGroupMeal { get { return isGroupMeal; } set { isGroupMeal = value; } }

        /// <summary>
		/// Get/Set method of the isSystemProduct field
		/// </summary>
		[DisplayName("IsSystemProduct")]
        public bool IsSystemProduct { get { return isSystemProduct; } set { isSystemProduct = value; } }

        /// <summary>
		/// Get/Set method of the isActive field
		/// </summary>
		[DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the POSTypeId field
        /// </summary>
        [DisplayName("POSTypeId")]
        public int POSTypeId { get { return posTypeId; } set { posTypeId = value; } }

        [DisplayName("NotificationTagProfileId")]
        public int NotificationTagProfileId { get { return notificationTagProfileId; } set { notificationTagProfileId = value; } }

        [DisplayName("IssueNotificationDevice")]
        public bool? IssueNotificationDevice { get { return issueNotificationDevice; } set { issueNotificationDevice = value; } }

        [DisplayName("IsRecommended")]
        public bool IsRecommended { get { return isRecommended; } set { isRecommended = value; } }

        [DisplayName("SearchDescription")]
        public string SearchDescription { get { return searchDescription; } set { searchDescription = value; } }

        [DisplayName("MaxQtyPerDay")]
        public int? MaxQtyPerDay { get { return maxQtyPerDay; } set { maxQtyPerDay = value; } }

        [DisplayName("AvailableUnits")]
        public int AvailableUnits { get { return availableUnits; } set { availableUnits = value; } }

        [DisplayName("WebDescription")]
        public string WebDescription { get { return webDescription; } set { webDescription = value; } }

        [DisplayName("TranslatedProductName")]
        public string TranslatedProductName { get { return translatedProductName; } set { translatedProductName = value; } }

        [DisplayName("TranslatedProductDescription")]
        public string TranslatedProductDescription { get { return translatedProductDescription; } set { translatedProductDescription = value; } }

        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the ComboProductContainerDTO field
        /// </summary>
        public List<ComboProductContainerDTO> ComboProductContainerDTOList { get { return comboProductContainerDTOList; } set { comboProductContainerDTOList = value; } }

        /// <summary>
        /// Get/Set method of the ProductModifierContainerDTO field
        /// </summary>
        public List<ProductModifierContainerDTO> ProductModifierContainerDTOList { get { return productModifierContainerDTOList; } set { productModifierContainerDTOList = value; } }

        /// <summary>
        /// Get/Set method of the ProductsDisplayGroupContainerDTO field
        /// </summary>
        public List<ProductsDisplayGroupContainerDTO> ProductsDisplayGroupContainerDTOList { get { return productsDisplayGroupContainerDTOList; } set { productsDisplayGroupContainerDTOList = value; } }
        /// <summary>
        /// Get/Set method of the customerProfilingGroupContainerDTO field
        /// </summary>
        public CustomerProfilingGroupContainerDTO CustomerProfilingGroupContainerDTO { get { return customerProfilingGroupContainerDTO; } set { customerProfilingGroupContainerDTO = value; } }
        /// <summary>
        /// Get/Set method of the InventoryItemContainerDTO field
        /// </summary>
        public InventoryItemContainerDTO InventoryItemContainerDTO { get { return inventoryItemContainerDTO; } set { inventoryItemContainerDTO = value; } }

        /// <summary>
        /// Get/Set method of the ProductSubscriptionContainerDTO field
        /// </summary>
        public ProductSubscriptionContainerDTO ProductSubscriptionContainerDTO { get { return productSubscriptionContainerDTO; } set { productSubscriptionContainerDTO = value; } }

        /// <summary>
        /// Get/Set method of the UpsellOffersContainerDTOList field
        /// </summary>
        public List<UpsellOffersContainerDTO> UpsellOffersContainerDTOList { get { return upsellOffersContainerDTOList; } set { upsellOffersContainerDTOList = value; } }

        /// <summary>
        /// Get/Set method of the CrossSellProductsContainerDTOList field
        /// </summary>
        public List<UpsellOffersContainerDTO> CrossSellProductsContainerDTOList { get { return crossSellProductsContainerDTOList; } set { crossSellProductsContainerDTOList = value; } }
        /// <summary>
        /// Get/Set method of the customDataContainerDTO field
        /// </summary>
        public List<CustomDataContainerDTO> CustomDataContainerDTOList { get { return customDataContainerDTOList; } set { customDataContainerDTOList = value; } }

        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        public DateTime? StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
        }

        ///<summary>
        ///Get/Set methid of the ExpiryTime field
        ///</summary>
        public DateTime? ExpiryTime
        {
            get
            {
                return expiryTime;
            }
            set
            {
                expiryTime = value;
            }
        }
 
        ///<summary>
        ///Get/Set method of the orderTypeId field
        ///</summary>
        public int OrderTypeId
        {
            get
            {
                return orderTypeId;
            }
            set
            {
                orderTypeId = value;
            }
        }
        ///<summary>
        ///Get/Set method of the credits field
        ///</summary>
        [DisplayName("Credits")]
        public decimal Credits { get { return credits; } set { credits = value; } }

        ///<summary>
        ///Get/Set method of the courtesy field
        ///</summary>
        [DisplayName("Courtesy")]
        public decimal Courtesy { get { return courtesy; } set { courtesy = value; } }

        ///<summary>
        ///Get/Set method of the bonus field
        ///</summary>
        [DisplayName("Bonus")]
        public decimal Bonus { get { return bonus; } set { bonus = value; } }

        ///<summary>
        ///Get/Set method of the time field
        ///</summary>
        [DisplayName("Time")]
        public decimal Time { get { return time; } set { time = value; } }
        /// <summary>
        /// serviceChargeIsApplicable
        /// </summary>
        [DisplayName("serviceChargeIsApplicable")]
        public bool ServiceChargeIsApplicable { get { return serviceChargeIsApplicable; } set { serviceChargeIsApplicable = value; } }
        /// <summary>
        /// serviceChargePercentage
        /// </summary>
        [DisplayName("serviceChargePercentage")]
        public decimal? ServiceChargePercentage { get { return serviceChargePercentage; } set { serviceChargePercentage = value; } }
        
        /// <summary>
        /// gratuityIsApplicable
        /// </summary>
        [DisplayName("gratuityIsApplicable")]
        public bool GratuityIsApplicable { get { return gratuityIsApplicable; } set { gratuityIsApplicable = value; } }
        /// <summary>
        /// GratuityPercentage
        /// </summary>
        [DisplayName("GratuityPercentage")]
        public decimal? GratuityPercentage { get { return gratuityPercentage; } set { gratuityPercentage = value; } }

        ///<summary>
        ///Get/Set method of the MaximumQuantity field
        ///</summary>
        [DisplayName("MaximumQuantity")]
        public int? MaximumQuantity { get { return maximumQuantity; } set { maximumQuantity = value; } }
        ///<summary>
        ///Get/Set method of the PauseType field
        ///</summary>
        [DisplayName("PauseType")]
        public PauseUnPauseType PauseType { get { return pauseType; } set { pauseType = value; } }
        ///<summary>
        ///Get/Set method of the CustomerProfilingGroupId field
        ///</summary>
        [DisplayName("CustomerProfilingGroupId")]
        public int CustomerProfilingGroupId { get { return customerProfilingGroupId; } set { customerProfilingGroupId = value; } }
        /// <summary>
        /// Get/Set method of the AgeUpperLimit field
        /// </summary>
        public decimal AgeUpperLimit
        {
            get { return ageUpperLimit; }
            set { ageUpperLimit = value; }
        }
        /// <summary>
        /// Get/Set method of the ageLowerLimit field
        /// </summary>
        public decimal AgeLowerLimit
        {
            get { return ageLowerLimit; }
            set { ageLowerLimit = value; }
        }
        /// <summary>
        /// Get/Set method of the ProductCreditPlusContainerDTOList field
        /// </summary>
        public List<ProductCreditPlusContainerDTO> ProductCreditPlusContainerDTOList { get { return productCreditPlusContainerDTOList; } set { productCreditPlusContainerDTOList = value; } }

        /// <summary>
        /// Get/Set method of the ProductGamesContainerDTOList field
        /// </summary>
        public List<ProductGamesContainerDTO> ProductGamesContainerDTOList { get { return productGamesContainerDTOList; } set { productGamesContainerDTOList = value; } }
    }
}

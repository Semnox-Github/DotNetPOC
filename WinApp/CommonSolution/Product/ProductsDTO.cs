/********************************************************************************************
 * Project Name - Products Programs
 * Description  - Data object of ProductDTO    
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Feb-2017   Rakshith       Created 
 *********************************************************************************************
 *2.60        15-Mar-2019    Nitin Pai      Added new search for PRODUCT_TYPE_NAME_LIST
 *2.60        11-Apr-2019    Archana        Include/Exclude for redeemable products changes
 *2.60       08-Feb-2019     Akshay G       Added log,notifyingObjectIsChangedSyncRoot, notifyingObjectIsChanged, added IsChanged in Set properties
 *                                          Made changes to nullability for some fields and respective properties of type (i.e, int?, decimal?, double?, DateTime?) 
 *2.60        12-Feb-2019    Akshay G       In Product class Modified minimumQuantity field and respective property to nullability
 *2.60        18-Feb-2019    Akshay G       Added Parameterized Constructor for ProductDetails
 *2.60        01-Apr-2019    Akshay G       modified ActiveFlag DataType( from string to bool) 
 *2.70        04-Apr-2019    Guru S A       Booking phase 2 changes
              29-May-2019    Jagan Mohan    Code merge from Development to WebManagementStudio
              01-Jul-2019    Indrajeet K    Added PRODUCT_DISPLAY_GROUP_FORMAT_ID in enum SearchByProductParameters.
              05-Jul-2019    Akshay G       merged from Development to Web branch
              30-Jul-2019    Jeevan         Modified BookingProductContent & AdditionalProductclass to include ComboProductId as part of booking phase2 changes
              06-Aug-2019    Nitin Pai      Added search by category id
 *2.70.2      12-Oct-2019    Akshay G       ClubSpeed enhancement Changes - Added ExternalSystemReference and modified SearchByProductParameters
 *2.70.2      12-Oct-2019    Akshay G       Added CustomDataSetDTO and SearchByProductParameters
 *2.70.2      04-Feb-2020    Nitin Pai      Fixed circular reference issue, this was causing IIS to crash
 *2.80        20-Apr-2020    Indrajeet K    Added EXTERNAL_SYSTEM_REFERENCE_LIST in enum SearchByProductParameters.
 *2.80.0      26-May-2020    Dakshakh       Added - LoadToSingleCard Attribute as a part of CardCount enhancement
 *3.00.0      26-Oct-2020    Girish Kundar  Added - LinkChildCard field and ProductName search for center edge  enhancement
 *2.110.0     08-Dec-2020    Indrajeet K    Added - LicenseType Property to Enhance License Functionality  
 *2.110.0     08-Dec-2020    Guru S A       Subscription changes
 *2.120.0     01-Mar-2020    Girish Kundar  Modified : Radian module changes - Added NotificationTagId, IssueNotificationdevice fields
 *2.120.0     01-Apr-2021    Dakshakh raj   modified : Enabling variable hours for Passtech Lockers and enabling function to extend the time -Added EnableVariableLockerHours field
 *2.140.0     14-Sep-2021    Prajwal S      modified : Added Short Name and IsRecommended fields for F&B requirements.
 *2.140.0     02-Feb-2022    Fiona          Added masterEntityId search param
 *2.130.04    17-Feb-2022    Nitin Pai      Added Product offset value to calculate against Product Calendar for website and app
 *2.130.3     01-Apr-2022    Ashish Sreejith Modified : Added advanceAmount property to BookingProducts class
 *2.160.0     02-May-2022    Guru S A       Auto Service Charges and Gratuity changes
 *2.150.0     06-May-2022    Girish Kundar  Modified : Added a new column  MaximumQuantity to Products
 *2.130.12    22-Dec-2022   Ashish Sreejith Added - webDescription to BookingProduct constructor
 *2.150.4     02-Jun-2023   Ashish Sreejith Added - ProductsDisplayGroupFormatDTO to BookingProduct constructor
********************************************************************************************/

using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductDTO Class
    /// </summary>
    public class ProductsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        /// <summary>
        /// SearchByProductParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProductParameters
        {
            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by PRODUCT_TYPE_ID field
            /// </summary>
            PRODUCT_TYPE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID,
            /// <summary>
            /// Search by Product id list
            /// </summary>
            PRODUCT_ID_LIST,
            /// <summary>
            /// Search by PRODUCT TYPE NAME
            /// </summary>
            PRODUCT_TYPE_NAME,
            /// <summary>
            /// Search by PRICE
            /// </summary>
            PRICE,
            /// <summary>
            /// Search by PRODUCT TYPE ID LIST
            /// </summary>
            PRODUCT_TYPE_NAME_LIST,
            /// <summary>
            /// Search by DISPLAY IN POS
            /// </summary>
            DISPLAY_IN_POS,
            ///// <summary>
            ///// Search by FACILITY_MAP_ID 
            ///// </summary>
            //FACILITY_MAP_ID,
            /// <summary>
            /// Search by CHECKIN_FACILITY_ID 
            /// </summary>
            CHECKIN_FACILITY_ID,
            /// <summary>
            /// Search by HSNSACCODE
            /// </summary>
            HSNSACCODE,
            /// <summary>
            /// Search by HAS_MODIFIER
            /// </summary>
            HAS_MODIFIER,
            /// <summary>
            /// Search by PRODUCT_DISPLAY_GROUP_FORMAT_ID
            /// </summary>
            PRODUCT_DISPLAY_GROUP_FORMAT_ID,
            /// <summary>
            /// Search by PRODUCT_DISPLAY_GROUP_FORMAT_ID_LIST
            /// </summary>
            PRODUCT_DISPLAY_GROUP_FORMAT_ID_LIST,
            /// <summary>
            /// Search by CATEGORY_ID
            /// </summary>
            CATEGORY_ID,
            /// <summary>
            /// Search by PRODUCT_NAME
            /// </summary>
            PRODUCT_NAME,
            /// <summary>
            /// Search by POS_TYPE_ID
            /// </summary>
            POS_TYPE_ID,
            /// <summary>
            /// Search by DISPLAY_GROUP_NAME
            /// </summary>
            DISPLAY_GROUP_NAME,
            /// <summary>
            /// Search by IS_SELLABLE
            /// </summary>
            IS_SELLABLE,
            /// <summary>
            /// Search by EXTERNAL_SYSTEM_REFERENCE
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by LAST_UPDATED_FROM_DATE
            /// </summary>
            LAST_UPDATED_FROM_DATE,
            /// <summary>
            /// Search by LAST_UPDATED_TO_DATE
            /// </summary>
            LAST_UPDATED_TO_DATE,
            /// <summary>
            /// Search by CUSTOM_DATA_SET_IS_SET
            /// </summary>
            CUSTOM_DATA_SET_IS_SET,
            /// <summary>
            /// Search by EXTERNAL_SYSTEM_REFERENCE_IS_SET
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE_IS_SET,
            /// <summary>
            /// Search by PRODUCTS_ENTITY_LAST_UPDATED_FROM_DATE
            /// </summary>
            PRODUCTS_ENTITY_LAST_UPDATED_FROM_DATE,
            /// <summary>
            /// Search by PRODUCT_SELL_DATE
            /// </summary>
            TRX_ONLY_PRODUCT_PURCHASE_DATE,
            /// <summary>
            /// Search by USER_LOGIN
            /// </summary>
            TRX_ONLY_USER_ROLE,
            /// <summary>
            /// Search by POS_MACHINE
            /// </summary>
            TRX_ONLY_POS_MACHINE,
            /// <summary>
            /// Search by POS_MACHINE
            /// </summary>
            TRX_ONLY_MEMBERSHIP,
            /// <summary>
            /// Search by EXTERNAL_SYSTEM_REFERENCE_LIST
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE_LIST,
            /// <summary>
            /// Search by PRODUCT_EXCACT_NAME
            /// </summary>
            PRODUCT_EXACT_NAME,
            /// <summary>
            /// Search by IS_SUBSCRIPTION_PRODUCT
            /// </summary>
            IS_SUBSCRIPTION_PRODUCT,
            /// <summary>
            /// Search by PRODUCT_GUID
            /// </summary>
            PRODUCT_GUID,
            ///<Summary>
            ///search by IS_A_SUBSCRIPTION_PRODUCT_OR_HAS_ACTIVE_SUBSCRIPTION_CHILD
            ///</Summary>
            IS_A_SUBSCRIPTION_PRODUCT_OR_HAS_ACTIVE_SUBSCRIPTION_CHILD,
            /// <summary>
            /// Search by SHORT_NAME
            /// </summary>
            SHORT_NAME,
            /// <summary>
            /// Search by ISRECOMMENDED
            /// </summary>
            ISRECOMMENDED,
            /// <summary>
            /// search by MINIMUM QUANTITY
            /// </summary>
            MINIMUM_QUANTITY,
            /// <summary>
            /// search by MINIMUM QUANTITY
            /// </summary>
            TAX_ID,
            /// <summary>
            /// search by MINIMUM QUANTITY
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// search by SERVICE_CHARGE_IS_APPLICABLE
            /// </summary>
            SERVICE_CHARGE_IS_APPLICABLE,
            /// <summary>
            /// search by GRATUITY_IS_APPLICABLE
            /// </summary>
            GRATUITY_IS_APPLICABLE,
            ///<Summary>
            ///search by OFFSET
            ///</Summary>
            OFFSET,
            ///<Summary>
            ///search by WAIVER_SET_ID
            ///</Summary>
            WAIVER_SET_ID,
            /// <summary>
            /// Search by CUSTOM_DATA_SET_ID 
            /// </summary>
            CUSTOM_DATA_SET_ID,
            /// <summary>
            /// Search by CUSTOM_DATA_SET_ID_LIST 
            /// </summary>
            CUSTOM_DATA_SET_ID_LIST,
        }

        private int product_id;
        private string product_name;
        private string description;
        private bool active_flag;
        private int product_type_id;
        private decimal price;
        private decimal credits;
        private decimal courtesy;
        private decimal bonus;
        private decimal time;
        private decimal sort_order;
        private int tax_id;
        private decimal tickets;
        private decimal face_value;
        private string display_group;
        private string ticket_allowed;
        private string vip_card;
        private DateTime last_updated_date;
        private string last_updated_user;
        private int internetKey;
        private string taxInclusivePrice;
        private string inventoryProductCode;
        private DateTime expiryDate;
        private int availableUnits;
        private string autoCheckOut;
        private int checkInFacilityId;
        private decimal maxCheckOutAmount;
        private int posTypeId;
        private int customDataSetId;
        private int cardTypeId;
        private string guid;
        private int site_id;
        private bool synchStatus;
        private string trxRemarksMandatory;
        private int categoryId;
        private int overridePrintTemplateId;
        private DateTime startDate;
        private string buttonColor;
        private string autoGenerateCardNumber;
        private string quantityPrompt;
        private string onlyForVIP;
        private string allowPriceOverride;
        private string registeredCustomerOnly;
        private string managerApprovalRequired;
        private decimal minimumUserPrice;
        private string textColor;
        private string font;
        private string verifiedCustomerOnly;
        private string modifier;
        private int minimumQuantity;
        private string displayInPOS;
        private bool trxHeaderRemarksMandatory;
        private int cardCount;
        private string imageFileName;
        private decimal advancePercentage;
        private decimal advanceAmount;
        private int emailTemplateId;
        private int maximumTime;
        private int minimumTime;
        private int cardValidFor;
        private string additionalTaxInclusive;
        private decimal additionalPrice;
        private int additionalTaxId;
        private int masterEntityId;
        private string waiverRequired;
        private int segmentCategoryId;
        private DateTime cardExpiryDate;
        private bool invokeCustomerRegistration;
        private int productDisplayGroupFormatId;
        private int waiverSetId;
        private bool loadToSingleCard;
        private bool enableVariableLockerHours;

        //Begin ExtraFields Other than product Table
        private string translatedProductName;
        private string translatedProductDescription;
        private string productType;
        private string categoryName;
        private double taxPercentage;
        string cardSale;
        string zoneCode;
        string lockerMode;
        string taxName;
        bool? usedInDiscounts;
        int? creditPlusConsumptionId;
        string mapedDisplayGroup;
        private bool linkChildCard;
        string licenseType;
        //End

        private int zoneId;
        private double lockerExpiryInHours;
        private DateTime lockerExpiryDate;

        //new fields
        int? maxQtyPerDay;
        string hsnSacCode;
        string webDescription;
        int? orderTypeId;
        string isGroupMeal;
        int? membershipId;
        string createdBy;
        DateTime creationDate;
        bool isSellable;
        string externalSystemReference;
        List<ComboProductDTO> comboProductDTOList = new List<ComboProductDTO>();
        List<ProductModifiersDTO> productModifierDTOList = new List<ProductModifiersDTO>();
        List<ProductCreditPlusDTO> productCreditPlusDTOList = new List<ProductCreditPlusDTO>();
        List<ProductGamesDTO> productGamesDTOList = new List<ProductGamesDTO>();
        List<ProductsCalenderDTO> productsCalenderDTOList = new List<ProductsCalenderDTO>();
        private ProductDTO inventoryItemDTO;
        private List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList;
        private List<UpsellOffersDTO> upsellOffersDTOList;
        private List<UpsellOffersDTO> crossSellOffersDTOList;
        private CustomDataSetDTO customDataSetDTO;
        private bool? issueNotificationDevice;
        private int notificationTagProfileId;
        private decimal orderedQuantity;
        private ProductSubscriptionDTO productSubscriptionDTO;
        private decimal? serviceCharge;
        private decimal? packingCharge;
        private string searchDescription;
        private bool isRecommended;
        private bool serviceChargeIsApplicable;
        private bool gratuityIsApplicable;
        private decimal? serviceChargePercentage;
        private decimal? gratuityPercentage;
        private int? maximumQuantity;
        private int customerProfilingGroupId;
        private ProductsContainerDTO.PauseUnPauseType pauseType;
        private CustomerProfilingGroupDTO customerProfilingGroupDTO;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductsDTO()
        {
            log.LogMethodEntry();
            this.product_id = -1;
            this.product_name = "";
            this.description = "";
            this.active_flag = true;
            this.product_type_id = -1;
            this.price = -1;
            this.credits = -1;
            this.courtesy = -1;
            this.bonus = -1;
            this.time = -1;
            this.sort_order = -1;
            this.tax_id = -1;
            this.tickets = -1;
            this.face_value = -1;
            this.display_group = "";
            this.ticket_allowed = "";
            this.vip_card = "";
            this.last_updated_date = DateTime.MinValue; ;
            this.last_updated_user = "";
            this.internetKey = -1;
            this.taxInclusivePrice = "";
            this.inventoryProductCode = "";
            this.expiryDate = DateTime.MinValue; ;
            this.availableUnits = -1;
            this.autoCheckOut = "";
            this.checkInFacilityId = -1;
            //this.facilityMapId = -1;
            this.maxCheckOutAmount = -1;
            this.posTypeId = -1;
            this.customDataSetId = -1;
            this.cardTypeId = -1;
            this.guid = "";
            this.site_id = -1;
            this.synchStatus = false;
            this.trxRemarksMandatory = "";
            this.categoryId = -1;
            this.overridePrintTemplateId = -1;
            this.startDate = DateTime.MinValue;
            this.buttonColor = "";
            this.autoGenerateCardNumber = "";
            this.quantityPrompt = "";
            this.onlyForVIP = "";
            this.allowPriceOverride = "";
            this.registeredCustomerOnly = "";
            this.managerApprovalRequired = "";
            this.minimumUserPrice = -1;
            this.textColor = "";
            this.font = "";
            this.verifiedCustomerOnly = "";
            this.modifier = "";
            this.minimumQuantity = 0;
            this.displayInPOS = "";
            this.trxHeaderRemarksMandatory = false;
            this.cardCount = -1;
            this.imageFileName = "";
            //this.attractionMasterScheduleId = -1;
            this.advancePercentage = -1;
            this.advanceAmount = -1;
            this.emailTemplateId = -1;
            this.maximumTime = -1;
            this.minimumTime = -1;
            this.cardValidFor = -1;
            this.additionalTaxInclusive = "";
            this.additionalPrice = -1;
            this.additionalTaxId = -1;
            this.masterEntityId = -1;
            this.waiverRequired = "";
            this.segmentCategoryId = -1;
            this.cardExpiryDate = DateTime.MinValue;
            this.invokeCustomerRegistration = false;
            this.productDisplayGroupFormatId = -1;
            this.zoneId = -1;
            this.lockerExpiryInHours = 0;
            this.waiverSetId = -1;
            this.hsnSacCode = "";
            this.webDescription = "";
            this.isGroupMeal = "";
            this.usedInDiscounts = false;
            this.mapedDisplayGroup = "";
            this.orderedQuantity = -1;
            this.loadToSingleCard = false;
            this.enableVariableLockerHours = false;
            this.linkChildCard = false;
            this.licenseType = "";
            this.notificationTagProfileId = -1;
            this.issueNotificationDevice = null;
            this.searchDescription = "";
            this.isRecommended = false;
            this.serviceChargeIsApplicable = false;
            this.ServiceChargePercentage = null;
            this.gratuityIsApplicable = false;
            this.gratuityPercentage = null;
            this.maximumQuantity = null;
            this.pauseType = ProductsContainerDTO.PauseUnPauseType.NONE;
            this.customerProfilingGroupId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor for Products Setup Entity
        /// </summary>
        public ProductsDTO(int product_id, string product_name, string description, bool active_flag, int product_type_id, decimal price, decimal credits,
                            decimal courtesy, decimal bonus, decimal time, decimal sort_order, int tax_id, decimal tickets, decimal facevalue, string display_group,
                            string ticket_allowed, string vip_card, DateTime last_updated_date, string last_updated_user, int internetKey,
                            string taxInclusivePrice, string inventoryProductCode, DateTime expiryDate, int availableUnits, string autoCheckOut,
                            int checkInFacilityId,
                            decimal maxCheckOutAmount, int posTypeId, int customDataSetId, int cardTypeId, string guid,
                            int site_id, bool synchStatus, string trxRemarksMandatory, int categoryId, int overridePrintTemplateId, DateTime startDate,
                            string buttonColor, string autoGenerateCardNumber, string quantityPrompt, string onlyForVIP, string allowPriceOverride,
                            string registeredCustomerOnly, string managerApprovalRequired, decimal minimumUserPrice, string textColor, string font,
                            string verifiedCustomerOnly, string modifier, int minimumQuantity, string displayInPOS, bool trxHeaderRemarksMandatory,
                            int cardCount, string imageFileName, decimal advancePercentage, decimal advanceAmount,
                            int emailTemplateId, int maximumTime, int minimumTime, int cardValidFor, string additionalTaxInclusive, decimal additionalPrice,
                            int additionalTaxId, int masterEntityId, string waiverRequired, int segmentCategoryId, DateTime cardExpiryDate,
                            bool invokeCustomerRegistration, int productDisplayGroupFormatId, int zoneId, double lockerExpiryInHours, DateTime lockerExpiryDate, int waiverSetId,
                            string productType, int? maxQtyPerDay, string hsnSacCode, string webDescription, int? orderTypeId,
                            string isGroupMeal, int? membershipId, string cardSale, string zoneCode, string lockerMode,
                            string taxName, bool? usedInDiscounts, string createdBy, DateTime creationDate, bool isSellable, string externalSystemReference,
                            string translatedProductName, string translatedProductDescription, bool loadToSingleCard, bool linkChildCard, string licenseType,
                            bool? issueNotificationDevice, bool enableVariableLockerHours, int notificationTagProfileId, decimal? serviceCharge,
                            decimal? packingCharge, string searchDescription, bool isRecommended,
                            bool serviceChargeIsApplicable, decimal? serviceChargePercentage,
                            bool gratuityIsApplicable, decimal? gratuityPercentage, int? maximumQuantity, ProductsContainerDTO.PauseUnPauseType pauseType, int customerProfilingGroupId)//, int facilityMapId)
        {
            log.LogMethodEntry(product_id, product_name, description, active_flag, product_type_id, price, credits,
                             courtesy, bonus, time, sort_order, tax_id, tickets, facevalue, display_group,
                             ticket_allowed, vip_card, last_updated_date, last_updated_user, internetKey,
                             taxInclusivePrice, inventoryProductCode, expiryDate, availableUnits, autoCheckOut,
                            checkInFacilityId,
                            maxCheckOutAmount, posTypeId, customDataSetId, cardTypeId, guid,
                            site_id, synchStatus, trxRemarksMandatory, categoryId, overridePrintTemplateId, startDate,
                            buttonColor, autoGenerateCardNumber, quantityPrompt, onlyForVIP, allowPriceOverride,
                            registeredCustomerOnly, managerApprovalRequired, minimumUserPrice, textColor, font,
                            verifiedCustomerOnly, modifier, minimumQuantity, displayInPOS, trxHeaderRemarksMandatory,
                            cardCount, imageFileName, advancePercentage, advanceAmount,
                            emailTemplateId, maximumTime, minimumTime, cardValidFor, additionalTaxInclusive, additionalPrice,
                            additionalTaxId, masterEntityId, waiverRequired, segmentCategoryId, cardExpiryDate,
                            invokeCustomerRegistration, productDisplayGroupFormatId, zoneId, lockerExpiryInHours, lockerExpiryDate, waiverSetId,
                            productType, maxQtyPerDay, hsnSacCode, webDescription, orderTypeId,
                            isGroupMeal, membershipId, cardSale, zoneCode, lockerMode,
                            taxName, usedInDiscounts, createdBy, creationDate, isSellable, externalSystemReference, translatedProductName, translatedProductDescription,
                            loadToSingleCard, licenseType, issueNotificationDevice, enableVariableLockerHours, notificationTagProfileId,
                            serviceCharge, packingCharge, searchDescription, isRecommended, serviceChargeIsApplicable, serviceChargePercentage, gratuityIsApplicable, gratuityPercentage);//, facilityMapId);

            this.product_id = product_id;
            this.product_name = product_name;
            this.description = description;
            this.active_flag = active_flag;
            this.product_type_id = product_type_id;
            this.price = price;
            this.credits = credits;
            this.courtesy = courtesy;
            this.bonus = bonus;
            this.time = time;
            this.sort_order = sort_order;
            this.tax_id = tax_id;
            this.tickets = tickets;
            this.face_value = facevalue;
            this.display_group = display_group;
            this.ticket_allowed = ticket_allowed;
            this.vip_card = vip_card;
            this.last_updated_date = last_updated_date;
            this.last_updated_user = last_updated_user;
            this.internetKey = internetKey;
            this.taxInclusivePrice = taxInclusivePrice;
            this.inventoryProductCode = inventoryProductCode;
            this.expiryDate = expiryDate;
            this.availableUnits = availableUnits;
            this.autoCheckOut = autoCheckOut;
            this.checkInFacilityId = checkInFacilityId;
            //this.facilityMapId = facilityMapId;
            this.maxCheckOutAmount = maxCheckOutAmount;
            this.posTypeId = posTypeId;
            this.customDataSetId = customDataSetId;
            this.cardTypeId = cardTypeId;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.trxRemarksMandatory = trxRemarksMandatory;
            this.categoryId = categoryId;
            this.overridePrintTemplateId = overridePrintTemplateId;
            this.startDate = startDate;
            this.buttonColor = buttonColor;
            this.autoGenerateCardNumber = autoGenerateCardNumber;
            this.quantityPrompt = quantityPrompt;
            this.onlyForVIP = onlyForVIP;
            this.allowPriceOverride = allowPriceOverride;
            this.registeredCustomerOnly = registeredCustomerOnly;
            this.managerApprovalRequired = managerApprovalRequired;
            this.minimumUserPrice = minimumUserPrice;
            this.textColor = textColor;
            this.font = font;
            this.verifiedCustomerOnly = verifiedCustomerOnly;
            this.modifier = modifier;
            this.minimumQuantity = minimumQuantity;
            this.displayInPOS = displayInPOS;
            this.trxHeaderRemarksMandatory = trxHeaderRemarksMandatory;
            this.cardCount = cardCount;
            this.imageFileName = imageFileName;
            //this.attractionMasterScheduleId = attractionMasterScheduleId;
            this.advancePercentage = advancePercentage;
            this.advanceAmount = advanceAmount;
            this.emailTemplateId = emailTemplateId;
            this.maximumTime = maximumTime;
            this.minimumTime = minimumTime;
            this.cardValidFor = cardValidFor;
            this.additionalTaxInclusive = additionalTaxInclusive;
            this.additionalPrice = additionalPrice;
            this.additionalTaxId = additionalTaxId;
            this.masterEntityId = masterEntityId;
            this.waiverRequired = waiverRequired;
            this.segmentCategoryId = segmentCategoryId;
            this.cardExpiryDate = cardExpiryDate;
            this.invokeCustomerRegistration = invokeCustomerRegistration;
            this.productDisplayGroupFormatId = productDisplayGroupFormatId;
            this.zoneId = zoneId;
            this.lockerExpiryInHours = lockerExpiryInHours;
            this.lockerExpiryDate = lockerExpiryDate;
            this.waiverSetId = waiverSetId;
            this.productType = productType;
            this.maxQtyPerDay = maxQtyPerDay;
            this.hsnSacCode = hsnSacCode;
            this.webDescription = webDescription;
            this.orderTypeId = orderTypeId;
            this.isGroupMeal = isGroupMeal;
            this.membershipId = membershipId;
            this.cardSale = cardSale;
            this.zoneCode = zoneCode;
            this.lockerMode = lockerMode;
            this.taxName = taxName;
            this.usedInDiscounts = usedInDiscounts;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.isSellable = isSellable;
            this.externalSystemReference = externalSystemReference;
            this.translatedProductName = translatedProductName;
            this.translatedProductDescription = translatedProductDescription;
            this.loadToSingleCard = loadToSingleCard;
            this.linkChildCard = linkChildCard;
            this.licenseType = licenseType;
            this.notificationTagProfileId = notificationTagProfileId;
            this.issueNotificationDevice = issueNotificationDevice;
            this.enableVariableLockerHours = enableVariableLockerHours;
            this.serviceCharge = serviceCharge;
            this.packingCharge = packingCharge;
            this.searchDescription = searchDescription;
            this.isRecommended = isRecommended;
            this.serviceChargeIsApplicable = serviceChargeIsApplicable;
            this.serviceChargePercentage = serviceChargePercentage;
            this.gratuityIsApplicable = gratuityIsApplicable;
            this.gratuityPercentage = gratuityPercentage;
            this.maximumQuantity = maximumQuantity;
            this.pauseType = pauseType;
            this.customerProfilingGroupId = customerProfilingGroupId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set For product id
        /// </summary>
        public int ProductId { get { return product_id; } set { product_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For product name
        /// </summary>
        public string ProductName { get { return product_name; } set { product_name = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For Description
        /// </summary>
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For ActiveFlag
        /// </summary>
        public bool ActiveFlag { get { return active_flag; } set { active_flag = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For productTypeId
        /// </summary>
        public int ProductTypeId { get { return product_type_id; } set { product_type_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For Price
        /// </summary>
        public decimal Price { get { return price; } set { price = value; this.IsChanged = true; } }
        /// <summary> 
        /// Get/Set For Credits
        /// </summary>
        public decimal Credits { get { return credits; } set { credits = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For Courtasy
        /// </summary>
        public decimal Courtesy { get { return courtesy; } set { courtesy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For Bonus
        /// </summary>
        public decimal Bonus { get { return bonus; } set { bonus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For Time
        /// </summary>
        public decimal Time { get { return time; } set { time = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For SortOrder
        /// </summary>
        public decimal SortOrder { get { return sort_order; } set { sort_order = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For Tax_id
        /// </summary>
        public int Tax_id { get { return tax_id; } set { tax_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For tickets
        /// </summary>
        public decimal Tickets { get { return tickets; } set { tickets = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For facevalue
        /// </summary>
        public decimal FaceValue { get { return face_value; } set { face_value = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For DisplayGroup
        /// </summary>
        public string DisplayGroup { get { return display_group; } set { display_group = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For TicketAllowed
        /// </summary>
        public string TicketAllowed { get { return ticket_allowed; } set { ticket_allowed = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For VIPCard
        /// </summary>
        public string VipCard { get { return vip_card; } set { vip_card = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For LastUpdatedDate
        /// </summary>
        public DateTime LastUpdatedDate { get { return last_updated_date; } set { last_updated_date = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For LastUpdated User
        /// </summary>
        public string LastUpdatedUser { get { return last_updated_user; } set { last_updated_user = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For InternetKey
        /// </summary>
        public int InternetKey { get { return internetKey; } set { internetKey = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For TaxInclusivePrice
        /// </summary>
        public string TaxInclusivePrice { get { return taxInclusivePrice; } set { taxInclusivePrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For InventoryProductCode
        /// </summary>
        public string InventoryProductCode { get { return inventoryProductCode; } set { inventoryProductCode = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For ExpiryDate
        /// </summary>
        public DateTime ExpiryDate { get { return expiryDate; } set { expiryDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For AvailableUnits
        /// </summary>
        public int AvailableUnits { get { return availableUnits; } set { availableUnits = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For AutoCheckOut
        /// </summary>
        public string AutoCheckOut { get { return autoCheckOut; } set { autoCheckOut = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For CheckFacilityId
        /// </summary>
        public int CheckInFacilityId { get { return checkInFacilityId; } set { checkInFacilityId = value; this.IsChanged = true; } }
        ///// <summary>
        ///// Get/Set For facilityMapId
        ///// </summary>
        //public int FacilityMapId { get { return facilityMapId; } set { facilityMapId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For MaxCheckOutAmount
        /// </summary>
        public decimal MaxCheckOutAmount { get { return maxCheckOutAmount; } set { maxCheckOutAmount = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For POSTypeId
        /// </summary>
        public int POSTypeId { get { return posTypeId; } set { posTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For CustomDatasetId
        /// </summary>
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For CardTypeId
        /// </summary>
        public int CardTypeId { get { return cardTypeId; } set { cardTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For GUID
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For SiteId
        /// </summary>
        public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For SyncStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For TrxRearksMandatory
        /// </summary>
        public string TrxRemarksMandatory { get { return trxRemarksMandatory; } set { trxRemarksMandatory = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For categoryId
        /// </summary>
        public int CategoryId { get { return categoryId; } set { categoryId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For OverridePrintTemplateId
        /// </summary>
        public int OverridePrintTemplateId { get { return overridePrintTemplateId; } set { overridePrintTemplateId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For StartDate
        /// </summary>
        public DateTime StartDate { get { return startDate; } set { startDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For ButtonColor
        /// </summary>
        public string ButtonColor { get { return buttonColor; } set { buttonColor = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For AutogenerateCardNumber
        /// </summary>
        public string AutoGenerateCardNumber { get { return autoGenerateCardNumber; } set { autoGenerateCardNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For QuantityPromt
        /// </summary>
        public string QuantityPrompt { get { return quantityPrompt; } set { quantityPrompt = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For OnlyForVip
        /// </summary>
        public string OnlyForVIP { get { return onlyForVIP; } set { onlyForVIP = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For AllowPriceOverride
        /// </summary>
        public string AllowPriceOverride { get { return allowPriceOverride; } set { allowPriceOverride = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For RegisteredCustomerOnly
        /// </summary>
        public string RegisteredCustomerOnly { get { return registeredCustomerOnly; } set { registeredCustomerOnly = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For ManagerApprovalRequired
        /// </summary>
        public string ManagerApprovalRequired { get { return managerApprovalRequired; } set { managerApprovalRequired = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For MinimumUserPrice
        /// </summary>
        public decimal MinimumUserPrice { get { return minimumUserPrice; } set { minimumUserPrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For TextColor
        /// </summary>
        public string TextColor { get { return textColor; } set { textColor = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For Font
        /// </summary>
        public string Font { get { return font; } set { font = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For VerifiedCustomerOnly
        /// </summary>
        public string VerifiedCustomerOnly { get { return verifiedCustomerOnly; } set { verifiedCustomerOnly = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For Modifier
        /// </summary>
        public string Modifier { get { return modifier; } set { modifier = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For MinimumQuantity
        /// </summary>
        public int MinimumQuantity { get { return minimumQuantity; } set { minimumQuantity = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For DisplayInPos
        /// </summary>
        public string DisplayInPOS { get { return displayInPOS; } set { displayInPOS = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For TrxHeaderRemarksMandatory
        /// </summary>
        public bool TrxHeaderRemarksMandatory { get { return trxHeaderRemarksMandatory; } set { trxHeaderRemarksMandatory = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For CardCount
        /// </summary>
        public int CardCount { get { return cardCount; } set { cardCount = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For ImageFileName
        /// </summary>
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; this.IsChanged = true; } }
        ///// <summary>
        ///// Get/Set For AttractionMasterScheduleId
        ///// </summary>
        // public int AttractionMasterScheduleId { get { return attractionMasterScheduleId; } set { attractionMasterScheduleId = value; } }
        /// <summary>
        /// Get/Set For AdvancePercentage
        /// </summary>
        public decimal AdvancePercentage { get { return advancePercentage; } set { advancePercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For AdvanceAmount
        /// </summary>
        public decimal AdvanceAmount { get { return advanceAmount; } set { advanceAmount = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For EmailTemplateId
        /// </summary>
        public int EmailTemplateId { get { return emailTemplateId; } set { emailTemplateId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For MaximumTime
        /// </summary>
        public int MaximumTime { get { return maximumTime; } set { maximumTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For MinimumTime
        /// </summary>
        public int MinimumTime { get { return minimumTime; } set { minimumTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For CardValidFor
        /// </summary>
        public int CardValidFor { get { return cardValidFor; } set { cardValidFor = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For AdditionalTaxInclusive
        /// </summary>
        public string AdditionalTaxInclusive { get { return additionalTaxInclusive; } set { additionalTaxInclusive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For AdditionalPrice
        /// </summary>
        public decimal AdditionalPrice { get { return additionalPrice; } set { additionalPrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For AdditionalTaxId
        /// </summary>
        public int AdditionalTaxId { get { return additionalTaxId; } set { additionalTaxId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For WaiverRequired
        /// </summary>
        public string WaiverRequired { get { return waiverRequired; } set { waiverRequired = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set ForSegmentCategory
        /// </summary>
        public int SegmentCategoryId { get { return segmentCategoryId; } set { segmentCategoryId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For CardExpirydate
        /// </summary>
        public DateTime CardExpiryDate { get { return cardExpiryDate; } set { cardExpiryDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For InvokeCustomerregistration
        /// </summary>
        public bool InvokeCustomerRegistration { get { return invokeCustomerRegistration; } set { invokeCustomerRegistration = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For ProductDisplayGroupFormatId
        /// </summary>
        public int ProductDisplayGroupFormatId { get { return productDisplayGroupFormatId; } set { productDisplayGroupFormatId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For ComboProductDTOList
        /// </summary>
        public List<ComboProductDTO> ComboProductDTOList { get { return comboProductDTOList; } set { comboProductDTOList = value; this.IsChanged = true; } }
        public List<ProductModifiersDTO> ProductModifierDTOList { get { return productModifierDTOList; } set { productModifierDTOList = value; } }
        //public List<AttractionMasterScheduleDTO> AttractionMasterSchedule { get { return AttractionMasterSchedule; } set { value = AttractionMasterSchedule; } }

        /// <summary>
        /// Get/Set For TranslatedProductName
        /// </summary>
        public string TranslatedProductName { get { return translatedProductName; } set { translatedProductName = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set For translatedProductDescription
        /// </summary>
        public string TranslatedProductDescription { get { return translatedProductDescription; } set { translatedProductDescription = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set For productType
        /// </summary>
        public string ProductType { get { return productType; } set { productType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set For categoryName
        /// </summary>
        public string CategoryName { get { return categoryName; } set { categoryName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set For taxPercentage
        /// </summary>
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Locker zone id 
        /// </summary>
        public int ZoneId { get { return zoneId; } set { zoneId = value; this.IsChanged = true; } }
        /// <summary>
        /// Locker Expiry in hours
        /// </summary>
        public double LockerExpiryInHours { get { return lockerExpiryInHours; } set { lockerExpiryInHours = value; this.IsChanged = true; } }
        /// <summary>
        /// Locker Expiry date 
        /// </summary>
        public DateTime LockerExpiryDate { get { return lockerExpiryDate; } set { lockerExpiryDate = value; this.IsChanged = true; } }

        /// <summary>
        /// WaiverSetId
        /// </summary>
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// MaxQtyPerDay
        /// </summary>
        public int? MaxQtyPerDay { get { return maxQtyPerDay; } set { maxQtyPerDay = value; this.IsChanged = true; } }

        /// <summary>
        /// HsnSacCode
        /// </summary>
        public string HsnSacCode { get { return hsnSacCode; } set { hsnSacCode = value; this.IsChanged = true; } }

        /// <summary>
        /// WebDescription
        /// </summary>
        public string WebDescription { get { return webDescription; } set { webDescription = value; this.IsChanged = true; } }

        /// <summary>
        /// OrderTypeId
        /// </summary>
        public int? OrderTypeId { get { return orderTypeId; } set { orderTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// IsGroupMeal
        /// </summary>
        public string IsGroupMeal { get { return isGroupMeal; } set { isGroupMeal = value; this.IsChanged = true; } }

        /// <summary>
        /// MembershipId
        /// </summary>
        public int? MembershipId { get { return membershipId; } set { membershipId = value; this.IsChanged = true; } }

        /// <summary>
        /// CardSale
        /// </summary>
        public string CardSale { get { return cardSale; } set { cardSale = value; this.IsChanged = true; } }

        /// <summary>
        /// ZoneCode
        /// </summary>
        public string ZoneCode { get { return zoneCode; } set { zoneCode = value; this.IsChanged = true; } }

        /// <summary>
        /// LockerMode
        /// </summary>
        public string LockerMode { get { return lockerMode; } set { lockerMode = value; this.IsChanged = true; } }

        /// <summary>
        /// taxName
        /// </summary>
        public string TaxName { get { return taxName; } set { taxName = value; this.IsChanged = true; } }

        /// <summary>
        /// UsedInDiscounts
        /// </summary>
        public bool? UsedInDiscounts { get { return usedInDiscounts; } set { usedInDiscounts = value; this.IsChanged = true; } }

        /// <summary>
        /// CreditPlusConsumptionId
        /// </summary>
        public int? CreditPlusConsumptionId { get { return creditPlusConsumptionId; } set { creditPlusConsumptionId = value; this.IsChanged = true; } }

        /// <summary>
        /// InventoryItemDTO
        /// </summary>
        public ProductDTO InventoryItemDTO { get { return inventoryItemDTO; } set { inventoryItemDTO = value; } }

        /// <summary>
        /// MapedDisplayGroup
        /// </summary>
        public string MapedDisplayGroup { get { return mapedDisplayGroup; } set { mapedDisplayGroup = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set Method of productsDisplayGroupDTOList field
        /// </summary>
        public List<ProductsDisplayGroupDTO> ProductsDisplayGroupDTOList { get { return productsDisplayGroupDTOList; } set { productsDisplayGroupDTOList = value; } }

        /// <summary>
        /// Get/Set Method of ProductsCalenderDTOList field
        /// </summary>
        public List<ProductsCalenderDTO> ProductsCalenderDTOList { get { return productsCalenderDTOList; } set { productsCalenderDTOList = value; } }

        /// <summary>
        /// Get/Set Method of Sellable field
        /// </summary>
        public bool IsSellable { get { return isSellable; } set { isSellable = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set Method of ExternalSystemReference field
        /// </summary>
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public bool LoadToSingleCard { get { return loadToSingleCard; } set { loadToSingleCard = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set For LinkChildCard
        /// </summary>
        public bool LinkChildCard { get { return linkChildCard; } set { linkChildCard = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set For LinkChildCard
        /// </summary>
        public string LicenseType { get { return licenseType; } set { licenseType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public bool EnableVariableLockerHours { get { return enableVariableLockerHours; } set { enableVariableLockerHours = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the SearchDescription field
        /// </summary>
        public string SearchDescription { get { return searchDescription; } set { searchDescription = value; this.IsChanged = true; } }
         
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public bool IsRecommended { get { return isRecommended; } set { isRecommended = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set For CreatedBy
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                this.IsChanged = true;
                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                this.IsChanged = true;
                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CustomDataSetDTO field
        /// </summary>
        public CustomDataSetDTO CustomDataSetDTO { get { return customDataSetDTO; } set { customDataSetDTO = value; } }

        public List<ProductCreditPlusDTO> ProductCreditPlusDTOList { get { return productCreditPlusDTOList; } set { productCreditPlusDTOList = value; } }

        public List<ProductGamesDTO> ProductGamesDTOList { get { return productGamesDTOList; } set { productGamesDTOList = value; } }

        public List<UpsellOffersDTO> UpsellOffersDTOList { get { return upsellOffersDTOList; } set { upsellOffersDTOList = value; } }

        public List<UpsellOffersDTO> CrossSellOffersDTOList { get { return crossSellOffersDTOList; } set { crossSellOffersDTOList = value; } }

        /// <summary>
        /// ProductSubscriptionDTO
        /// </summary>
        public ProductSubscriptionDTO ProductSubscriptionDTO { get { return productSubscriptionDTO; } set { productSubscriptionDTO = value; } }
        /// <summary>
        /// CustomerProfilingGroupDTO
        /// </summary>
        public CustomerProfilingGroupDTO CustomerProfilingGroupDTO { get { return customerProfilingGroupDTO; } set { customerProfilingGroupDTO = value; } }
        public int NotificationTagProfileId { get { return notificationTagProfileId; } set { notificationTagProfileId = value; this.IsChanged = true; } }
        public bool? IssueNotificationDevice { get { return issueNotificationDevice; } set { issueNotificationDevice = value; this.IsChanged = true; } }

        public decimal? ServiceCharge { get { return serviceCharge; } set { serviceCharge = value; this.IsChanged = true; } }
        public decimal? PackingCharge { get { return packingCharge; } set { packingCharge = value; this.IsChanged = true; } }
        /// <summary>
        /// serviceChargeIsApplicable
        /// </summary>
        public bool ServiceChargeIsApplicable { get { return serviceChargeIsApplicable; } set { serviceChargeIsApplicable = value; this.IsChanged = true; } }
        /// <summary>
        /// serviceChargePercentage
        /// </summary>
        public decimal? ServiceChargePercentage { get { return serviceChargePercentage; } set { serviceChargePercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// gratuityIsApplicable
        /// </summary>
        public bool GratuityIsApplicable { get { return gratuityIsApplicable; } set { gratuityIsApplicable = value; this.IsChanged = true; } }
        /// <summary>
        /// GratuityPercentage
        /// </summary>
        public decimal? GratuityPercentage { get { return gratuityPercentage; } set { gratuityPercentage = value; this.IsChanged = true; } }
        public int? MaximumQuantity { get { return maximumQuantity; } set { maximumQuantity = value; this.IsChanged = true; } }
        public int CustomerProfilingGroupId { get { return customerProfilingGroupId; } set { customerProfilingGroupId = value; this.IsChanged = true; } }
        public ProductsContainerDTO.PauseUnPauseType PauseType { get { return pauseType; } set { pauseType = value; this.IsChanged = true; } }
        ///// <summary>
        ///// Get/Set For orderedQuantity
        ///// </summary>
        //public decimal OrderedQuantity { get { return orderedQuantity; } set { orderedQuantity = value; } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged//Added on 08-Feb-2019 By Akshay Gulaganji
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || product_id < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }
        /// <summary>
        /// Returns whether Product or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (inventoryItemDTO != null &&
                    inventoryItemDTO.IsChangedRecursive)
                {
                    return true;
                }
                if (productsDisplayGroupDTOList != null &&
                   productsDisplayGroupDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (customDataSetDTO != null &&
                    customDataSetDTO.IsChangedRecursive)
                {
                    return true;
                }
                if(productsCalenderDTOList != null &&
                    productsCalenderDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (upsellOffersDTOList != null &&
                   upsellOffersDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (crossSellOffersDTOList != null &&
                   crossSellOffersDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit(null);
        }

    }
    public class Product
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId;
        private string productName;
        private string productDescription;
        private string productType;
        private string productDisplayGroup;
        private string productImage;
        private double price;
        private int minimumQuantity;
        private int maximumQuantity;
        private int availableUnits;
        private double faceValue;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public Product()
        {
            log.LogMethodEntry();
            availableUnits = 0;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>  
        public Product(int orderedProductId)
        {
            log.LogMethodEntry(orderedProductId);
            productId = orderedProductId;
            productName = "";
            productDescription = "";
            productType = "";
            productDisplayGroup = "";
            productImage = "";
            minimumQuantity = 0;
            maximumQuantity = 0;
            faceValue = 0;
            log.LogMethodExit();

        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public Product(int productId, string productDesc) : this(productId)
        {
            log.LogMethodEntry(productId, productDesc);
            productName = productDesc;
            productDescription = productDesc;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public Product(int productId, string productDesc, int minimumQuantity)
            : this(productId)
        {
            log.LogMethodEntry(productId, productDesc, minimumQuantity);
            productName = productDesc;
            productDescription = productDesc;
            this.minimumQuantity = minimumQuantity;
            log.LogMethodExit();
        }


        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public Product(int productId, string productDesc, int minimumQuantity, int availableUnits)
            : this(productId)
        {
            log.LogMethodEntry(productId, productDesc, minimumQuantity, availableUnits);
            productName = productDesc;
            productDescription = productDesc;
            this.minimumQuantity = minimumQuantity;
            this.availableUnits = availableUnits;
            log.LogMethodExit();
        }


        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public Product(int productId, string productDesc, double price)
            : this(productId, productDesc)
        {
            log.LogMethodEntry(productId, productDesc, price);
            this.price = price;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public Product(int productId, string productDesc, string productType, string displayGroup) : this(productId, productDesc)
        {
            log.LogMethodEntry(productId, productDesc, productType, displayGroup);
            this.productType = productType;
            this.productDisplayGroup = displayGroup;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public Product(int productId, string productDesc, string productType, string displayGroup, string productImage)
            : this(productId, productDesc, productType, displayGroup)
        {
            log.LogMethodEntry(productId, productDesc, productType, displayGroup, productImage);
            this.productImage = productImage;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public Product(int productId, string productDesc, string productType, string displayGroup, string productImage, double price)
            : this(productId, productDesc, productType, displayGroup)
        {
            log.LogMethodEntry(productId, productDesc, productType, displayGroup, productImage, price);
            this.productImage = productImage;
            this.price = price;
            log.LogMethodExit();
        }
        ///// <summary>
        /////   Constructor With parameter
        ///// </summary>
        //public Product(int productId, string productName, string productDesc, string productType, string displayGroup, string productImage, double price): this(productId)
        //{
        //    this.productName = productName;
        //    this.productImage = productImage;
        //    this.price = price;
        //    this.productDisplayGroup = displayGroup;
        //    this.productDescription = productDesc;
        //    this.productType = productType;
        //}
        /// <summary>
        ///   Constructor With parameter
        /// </summary>

        public Product(int productId, string productName, string productDesc, string productType, string displayGroup, string productImage, double price, int minimumQuantity, int maximumQuantity)
            : this(productId)
        {
            log.LogMethodEntry(productId, productName, productDesc, productType, displayGroup, productImage, price, minimumQuantity, maximumQuantity);
            this.productName = productName;
            this.productImage = productImage;
            this.price = price;
            this.productDisplayGroup = displayGroup;
            this.productDescription = productDesc;
            this.productType = productType;
            this.minimumQuantity = minimumQuantity;
            this.maximumQuantity = maximumQuantity;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; } }

        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        [DisplayName("ProductName")]
        public string ProductName { get { return productName; } set { productName = value; } }

        /// <summary>
        /// Get/Set method of the ProductDescription field
        /// </summary>
        [DisplayName("ProductDescription")]
        public string ProductDescription { get { return productDescription; } set { productDescription = value; } }

        /// <summary>
        /// Get/Set method of the ProductType field
        /// </summary>
        [DisplayName("ProductType")]
        public string ProductType { get { return productType; } set { productType = value; } }

        /// <summary>
        /// Get/Set method of the ProductDisplayGroup field
        /// </summary>
        [DisplayName("ProductDisplayGroup")]
        public string ProductDisplayGroup { get { return productDisplayGroup; } set { productDisplayGroup = value; } }

        /// <summary>
        /// Get/Set method of the ProductImage field
        /// </summary>
        [DisplayName("ProductImage")]
        public string ProductImage { get { return productImage; } set { productImage = value; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public double Price { get { return price; } set { price = value; } }

        /// <summary>
        /// Get/Set method of the MinimumQuantity field
        /// </summary>
        [DisplayName("MinimumQuantity")]
        public int MinimumQuantity { get { return minimumQuantity; } set { minimumQuantity = value; } }

        /// <summary>
        /// Get/Set method of the MaximumQuantity field
        /// </summary>
        [DisplayName("MaximumQuantity")]
        public int MaximumQuantity { get { return maximumQuantity; } set { maximumQuantity = value; } }


        /// <summary>
        /// Get/Set method of the AvailableUnits field
        /// </summary>
        [DisplayName("AvailableUnits")]
        public int AvailableUnits { get { return availableUnits; } set { availableUnits = value; } }


        /// <summary>
        /// Get/Set method of the FaceValue field
        /// </summary>
        [DisplayName("FaceValue")]
        public double FaceValue { get { return faceValue; } set { faceValue = value; } }


    }
    /// <summary>
    ///  PurchasedProducts class. 
    /// </summary>
    public class PurchasedProducts : Product
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int quantity;
        private int lineId;
        private int cardId;
        private string cardNumber;
        private double taxAmount;
        private string remarks;
        private bool isGroupedCombo;
        private string autoGenerateCardNumber;
        private int siteId;

        List<PurchasedModifierSet> purchasedModifierSetDTOList = new List<PurchasedModifierSet>();
        PurchasedProducts parentModifierProduct;
        private bool isSelected;
        /// <summary>
        /// Default Constructor 
        /// </summary>
        public PurchasedProducts()
        {
            log.LogMethodEntry();
            isSelected = false;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProducts(int productId)
            : base(productId)
        {
            log.LogMethodEntry(productId);
            quantity = 0;
            lineId = -1;
            cardId = -1;
            cardNumber = "";
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProducts(int productId, string productDesc, int purchasedLineId, int productQuantity, double purchaseAmount, double taxAmount)
            : base(productId, productDesc, purchaseAmount)
        {
            log.LogMethodEntry(productId, productDesc, purchasedLineId, productQuantity, purchaseAmount, taxAmount);
            lineId = purchasedLineId;
            quantity = productQuantity;
            this.taxAmount = taxAmount;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProducts(int productId, string productDesc, int purchasedLineId, int productQuantity, double purchaseAmount, double taxAmount, int purchaseCardId, string purchaseCardNumber, string remarks)
            : this(productId, productDesc, purchasedLineId, productQuantity, purchaseAmount, taxAmount)
        {
            log.LogMethodEntry(productId, productDesc, purchasedLineId, productQuantity, purchaseAmount, taxAmount, purchaseCardId, purchaseCardNumber, remarks);
            cardId = purchaseCardId;
            cardNumber = purchaseCardNumber;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProducts(int productId, string productDesc, int productQuantity, double purchaseAmount, double taxAmount, int purchaseCardId, string purchaseCardNumber, string remarks)
            : base(productId, productDesc, purchaseAmount)
        {
            log.LogMethodEntry(productId, productDesc, productQuantity, purchaseAmount, taxAmount, purchaseCardId, purchaseCardNumber, remarks);
            cardId = purchaseCardId;
            cardNumber = purchaseCardNumber;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int Quantity { get { return quantity; } set { quantity = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        [DisplayName("TrxLineId")]
        public int TrxLineId { get { return lineId; } set { lineId = value; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        public int CardId { get { return cardId; } set { cardId = value; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("CardNumber")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; } }

        /// <summary>
        /// Get/Set method of the Amount field
        /// </summary>
        [DisplayName("Amount")]
        public double Amount { get { return Price; } set { Price = value; } }

        /// <summary>
        /// Get/Set method of the TaxAmount field
        /// </summary>
        [DisplayName("TaxAmount")]
        public double TaxAmount { get { return taxAmount; } set { taxAmount = value; } }

        /// <summary>
        /// Get/Set method of the TotalAmount field
        /// </summary>
        [DisplayName("TotalAmount")]
        public double TotalAmount { get { return (quantity * Price); } set { /*TotalAmount = value;*/ } }

        /// <summary>
        /// Get/Set method of the GrandTotal field
        /// </summary>
        [DisplayName("GrandTotal")]
        public double GrandTotal { get { return (quantity * Price) + taxAmount; } set { /*GrandTotal = value;*/ } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        /// Get/Set method of the IsGroupedCombo field
        /// </summary>
        [DisplayName("IsGroupedCombo")]
        public bool IsGroupedCombo { get { return isGroupedCombo; } set { isGroupedCombo = value; } }

        /// <summary>
        /// Get/Set For AutogenerateCardNumber
        /// </summary>
        public string AutoGenerateCardNumber { get { return autoGenerateCardNumber; } set { autoGenerateCardNumber = value; } }

        /// <summary>
        /// Get/Set method of the IsSelected field
        /// </summary>
        [DisplayName("IsSelected")]
        public bool IsSelected { get { return isSelected; } set { isSelected = value; } }


        /// <summary>
        /// Get/Set Property for PurchasedModifierSetDTOList
        /// </summary>
        public List<PurchasedModifierSet> PurchasedModifierSetDTOList
        {
            get
            {
                return purchasedModifierSetDTOList;
            }
            set
            {
                purchasedModifierSetDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Property for ParentModifierProduct
        /// </summary>
        public PurchasedProducts ParentModifierProduct
        {
            get
            {
                return parentModifierProduct;
            }
            set
            {
                parentModifierProduct = value;
            }
        }

    }



    /// <summary>
    /// BookingProduct class. 
    /// </summary>
    public class BookingProduct : Product
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private double advanceAmount;
        private int minimumTime;
        private int maximumTime;
        private int sort_order;
        private List<BookingPackageProduct> bookingProductPackagelist;
        private double advancePercentage;
        private string webDescription;
        private List<ProductDisplayGroupFormatDTO> productsDisplayGroupFormatDTO = new List<ProductDisplayGroupFormatDTO>();

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public BookingProduct()
        {
            log.LogMethodEntry();
            bookingProductPackagelist = new List<BookingPackageProduct>();
            log.LogMethodExit();
        }


        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public BookingProduct(int productId, string productName, string productDesc, string imageFileName, double price, double advanceAmount, int minimumQuantity, int minimumTime, int maximumTime, int quantity, int availableUnits, int sortorder, double advancePercentage)
            : base(productId, productName, minimumQuantity, availableUnits)
        {
            log.LogMethodEntry(productId, productName, productDesc, imageFileName, price, advanceAmount, minimumQuantity, minimumTime, maximumTime, quantity, availableUnits, sortorder, advancePercentage);
            base.ProductDescription = productDesc;
            base.ProductImage = imageFileName;
            base.Price = price;
            this.advanceAmount = advanceAmount;
            this.minimumTime = minimumTime;
            this.maximumTime = maximumTime;
            this.sort_order = sortorder;
            this.advancePercentage = advancePercentage;
            log.LogMethodExit();
        }
        public BookingProduct(int productId, string productName, string productDesc, string imageFileName, double price, double advanceAmount, int minimumQuantity, int minimumTime, int maximumTime, int quantity, int availableUnits, int sortorder, double advancePercentage, string webDescription)
            : this(productId, productName, productDesc, imageFileName, price, advanceAmount, minimumQuantity, minimumTime, maximumTime, quantity, availableUnits, sortorder, advancePercentage)
        {
            log.LogMethodEntry(webDescription);
            this.webDescription = webDescription;
            log.LogMethodExit();
        }

        public BookingProduct(int productId, string productName, string productDesc, string imageFileName, double price, double advanceAmount, int minimumQuantity, int minimumTime, int maximumTime, int quantity, int availableUnits, int sortorder, double advancePercentage, string webDescription, List<ProductDisplayGroupFormatDTO> productsDisplayGroupFormatDTO)
            : this(productId, productName, productDesc, imageFileName, price, advanceAmount, minimumQuantity, minimumTime, maximumTime, quantity, availableUnits, sortorder, advancePercentage, webDescription)
        {
            log.LogMethodEntry(productsDisplayGroupFormatDTO);
            this.productsDisplayGroupFormatDTO = productsDisplayGroupFormatDTO;
            log.LogMethodExit();
        }

        // <summary>
        // Get/Set method of the ProductsDisplayGroupFormatDTO field
        // </summary>
        [DisplayName("ProductsDisplayGroupFormatDTO")]
        public List<ProductDisplayGroupFormatDTO> ProductsDisplayGroupFormatDTO { get { return productsDisplayGroupFormatDTO; } set { productsDisplayGroupFormatDTO = value; } }

        /// <summary>
        /// Get/Set method of the WebDescription field
        /// </summary>
        [DisplayName("WebDescription")]
        public string WebDescription { get { return webDescription; } set { webDescription = value; } }


        /// <summary>
        /// Get/Set method of the AdvanceAmount field
        /// </summary>
        [DisplayName("AdvanceAmount")]
        public Double AdvanceAmount { get { return advanceAmount; } set { advanceAmount = value; } }

        /// <summary>
        /// Get/Set method of the AdvancePercentage field
        /// </summary>
        [DisplayName("AdvancePercentage")]
        public Double AdvancePercentage { get { return advancePercentage; } set { advancePercentage = value; } }

        /// <summary>
        /// Get/Set method of the MinimumTime field
        /// </summary>
        [DisplayName("MinimumTime")]
        public int MinimumTime { get { return minimumTime; } set { minimumTime = value; } }


        /// <summary>
        /// Get/Set method of the MaximumTime field
        /// </summary>
        [DisplayName("MaximumTime")]
        public int MaximumTime { get { return maximumTime; } set { maximumTime = value; } }


        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        [DisplayName("SortOrder")]
        public int SortOrder { get { return sort_order; } set { sort_order = value; } }


        /// <summary>
        /// Get/Set method of the BookingProductPackagelist field
        /// </summary>
        [DisplayName("BookingProductPackagelist")]
        public List<BookingPackageProduct> BookingProductPackagelist { get { return bookingProductPackagelist; } set { bookingProductPackagelist = value; } }


    }


    /// <summary>
    /// BookingProduct class. 
    /// </summary>
    public class BookingProductContent : Product
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int categoryId;
        private int quantity;
        private string priceInclusive;
        private int comboProductId;
        private string webDescription;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public BookingProductContent()
        {
            log.LogMethodEntry();
            quantity = 0;
            log.LogMethodExit();
        }


        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public BookingProductContent(int productId, string productName, string imageFileName, string displayGroup, int childProductId, int categoryId, double price, string productType, int quantity, string priceInclusive, int comboProductId, double faceValue)
            : base(productId, productName, productType, displayGroup, imageFileName, price)
        {
            log.LogMethodEntry(productId, productName, imageFileName, displayGroup, childProductId, categoryId, price, productType, quantity, priceInclusive, comboProductId, faceValue);
            this.categoryId = categoryId;
            this.priceInclusive = priceInclusive;
            this.quantity = quantity;
            this.comboProductId = comboProductId;
            this.FaceValue = faceValue;
            log.LogMethodExit();

        }
        public BookingProductContent(int productId, string productName, string imageFileName, string displayGroup, int childProductId, int categoryId, double price, string productType, int quantity, string priceInclusive, int comboProductId, double faceValue, string webDescription)
            : this(productId, productName, imageFileName, displayGroup, childProductId, categoryId, price, productType, quantity, priceInclusive, comboProductId, faceValue)
        {
            log.LogMethodEntry(webDescription);
            this.webDescription = webDescription;
            log.LogMethodExit();

        }

        /// <summary>
        /// Get/Set method of the WebDescription field
        /// </summary>
        [DisplayName("WebDescription")]
        public string WebDescription { get { return webDescription; } set { webDescription = value; } }


        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        [DisplayName("CategoryId")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }


        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int Quantity { get { return quantity; } set { quantity = value; } }


        /// <summary>
        /// Get/Set method of the PriceInclusive field
        /// </summary>
        [DisplayName("PriceInclusive")]
        public string PriceInclusive { get { return priceInclusive; } set { priceInclusive = value; } }

        /// <summary>
        /// Get/Set method of the ComboProductId field
        /// </summary>
        [DisplayName("ComboProductId")]
        public int ComboProductId { get { return comboProductId; } set { comboProductId = value; } }

    }


    /// <summary>
    /// This is the  BookingPackageProduct data object class. This acts as data holder for the CMSGroupsDTO  Items business object
    /// </summary>
    public class BookingPackageProduct : BookingProductContent
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<BookingProductContent> bookingPackageProductContents;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BookingPackageProduct()
        {
            log.LogMethodEntry();
            bookingPackageProductContents = new List<BookingProductContent>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Parent class data and Child  data fields
        /// </summary> 
        public BookingPackageProduct(BookingProductContent bookingProductContent, List<BookingProductContent> packageContents)
        {
            log.LogMethodEntry(bookingProductContent, packageContents);
            this.bookingPackageProductContents = packageContents;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the BookingPackageProductContents
        /// </summary>
        [DisplayName("BookingPackageProductContents")]
        public List<BookingProductContent> BookingPackageProductContents { get { return bookingPackageProductContents; } set { bookingPackageProductContents = value; } }

    }





    /// <summary>
    /// AdditionalProduct class. 
    /// </summary>
    public class AdditionalProduct : Product
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int categoryId;
        private int childProductId;
        private int orderedQuantity;
        private int comboProductId;
        private string webDescription;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public AdditionalProduct()
        {
            log.LogMethodEntry();
            this.orderedQuantity = 0;
            this.comboProductId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public AdditionalProduct(int productId, string productName, string productdesc, string imageFileName, string displayGroup, int childProductId, int categoryId, double price, string productType, int minimumQuantity, int maximumQuantity, int comboProductId, double faceValue, string webDescription)
            : base(productId, productName, productdesc, productType, displayGroup, imageFileName, price, minimumQuantity, maximumQuantity)
        {
            log.LogMethodEntry(productId, productName, productdesc, imageFileName, displayGroup, childProductId, categoryId, price, productType, minimumQuantity, maximumQuantity, comboProductId);
            this.categoryId = categoryId;
            this.childProductId = childProductId;
            this.comboProductId = comboProductId;
            this.FaceValue = faceValue;
            this.webDescription = webDescription;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        [DisplayName("CategoryId")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }


        /// <summary>
        /// Get/Set method of the ChildProductId field
        /// </summary>
        [DisplayName("ChildProductId")]
        public int ChildProductId { get { return childProductId; } set { childProductId = value; } }


        /// <summary>
        /// Get/Set method of the OrderedQuantity fields
        /// </summary>
        [DisplayName("OrderedQuantity")]
        public int OrderedQuantity { get { return orderedQuantity; } set { orderedQuantity = value; } }


        /// Get/Set method of the ComboProductId field
        /// </summary>
        [DisplayName("ComboProductId")]
        public int ComboProductId { get { return comboProductId; } set { comboProductId = value; } }

        /// <summary>
        /// Get/Set method of the WebDescription field
        /// </summary>
        [DisplayName("WebDescription")]
        public string WebDescription { get { return webDescription; } set { webDescription = value; } }

    }
}

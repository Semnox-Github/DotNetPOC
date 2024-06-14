/********************************************************************************************
 * Project Name - DigitalSignage Module
 * Description  - Localization for all Literals and messages 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        17-Jan-2018   Jagan Mohana Rao          Created
 *2.60        10-Apr-2019   Akshay Gulaganji          merged "PRODUCTDETAILS" literals to "PRODUCTSSETUP" literals
 *2.70        22-Jul-2019   Akshay Gulaganji          merged from WebManagementStudio to Development branch
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Newtonsoft.Json;

namespace Semnox.Parafait.Product
{
    public class ProductsLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();
        /// <summary>
        ///   Default Constructor for Products Locallization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public ProductsLocalizationBL(ExecutionContext executionContext, string entityName)
        {
            log.LogMethodEntry(executionContext, entityName);
            this.executionContext = executionContext;
            this.entityName = entityName;
            log.LogMethodExit();
        }
        /// <summary>
        /// Getting lable messageno and headers
        /// </summary>
        /// <returns>json</returns>
        private void GetLiteralsAndMessages(string entity, List<string> literalsOrMessageList)
        {
            log.LogMethodEntry(entity, literalsOrMessageList);
            string localizedValue = "";
            foreach (string literalsOrMessages in literalsOrMessageList)
            {
                localizedValue = MessageContainerList.GetMessage(executionContext, literalsOrMessages);
                if (!listHeadersList.ContainsKey(literalsOrMessages))
                {
                    listHeadersList.Add(literalsOrMessages, localizedValue);
                }
            }
            log.LogMethodExit();
        }

        public string GetLocalizedLabelsAndHeaders()
        {
            log.LogMethodEntry();

            string literalsMessagesList = string.Empty;
            if (!string.IsNullOrEmpty(entityName))
            {
                List<string> literalsOrMessage = GetLiterals(entityName);
                GetLiteralsAndMessages(entityName, literalsOrMessage);
                if (listHeadersList.Count != 0)
                {
                    literalsMessagesList = JsonConvert.SerializeObject(listHeadersList, Formatting.Indented);
                }
            }
            log.LogMethodExit(literalsMessagesList);
            return literalsMessagesList;
        }

        private List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);
            List<string> literals = new List<string>();
            switch (entityName.ToUpper().ToString())
            {
                case "PRODUCTS":
                    literals.Add("Products");
                    break;
                case "PRODUCTDISPLAYGROUP":
                    literals.Add("Display Group");
                    literals.Add("Select Display Groups");
                    literals.Add("Product Display Groups");
                    break;
                case "PRODUCTDETAILSPRODUCTMODIFIERS":
                    literals.Add("Product Modifiers");
                    literals.Add("Modifier Set");
                    literals.Add("Auto Show In Pos");
                    break;
                case "PRODUCTDISCOUNTS":
                    literals.Add("Product Discounts");
                    literals.Add("Discount");
                    literals.Add("Valid For");
                    literals.Add("Valid For Days or Months");
                    literals.Add("Discounts to load through this product");
                    literals.Add("Show Active Only");
                    break;
                case "PRODUCTSENTITYEXCLUSION":
                    literals.Add("Include / Exclude Days From Product Games");
                    literals.Add("Override Date");
                    literals.Add("Day");
                    literals.Add("Remarks");
                    literals.Add("Entity Exclusion");
                    literals.Add("Include This Day?");
                    break;
                case "WAIVERSIGNINGOPTION":
                    literals.Add("Lookup Value");
                    literals.Add("Enable");
                    literals.Add("Waiver Signing Option");
                    break;
                case "PRODUCTGAMESENTITLEMENTS": //Product Setup Menu -> Product Details -> Games
                    literals.Add("Product Games / Entitlements");
                    literals.Add("Add or Remove Games / Entitlements from Product");
                    literals.Add("Game Profile");
                    literals.Add("Game");
                    literals.Add("Play Count / Entt.Value");
                    literals.Add("Frequency");
                    literals.Add("Valid For");
                    literals.Add("Valid for Days / Minutes");
                    literals.Add("Effective After Days");
                    literals.Add("From Date");
                    literals.Add("Expiry Date");
                    literals.Add("Expiry Time");
                    literals.Add("Entitlement Type");
                    literals.Add("Ticket Allowed");
                    literals.Add("Optional Attribute");
                    literals.Add("CustomDataSetId");
                    literals.Add("Monday");
                    literals.Add("Tuesday");
                    literals.Add("Wednesday");
                    literals.Add("Thursday");
                    literals.Add("Friday");
                    literals.Add("Saturday");
                    literals.Add("Sunday");
                    literals.Add("Extended Inclusion / Exclusions");
                    literals.Add("Exclude");
                    literals.Add("Audit");
                    literals.Add("Hint: Enter - 1 in Play Count field to exclude a specific Game or Game Profile");
                    literals.Add("Play Limit Per Game");
                    literals.Add("Incl. / Excl. Dates");
                    literals.Add("Custom");
                    literals.Add("Entity Exclusion");
                    literals.Add("Audit Log");
                    literals.Add("Product Games");
                    break;
                case "PRODUCTGAMESENTITLEMENTSAUDIT":
                    literals.Add("product_game_id");
                    literals.Add("product_id");
                    literals.Add("game_id");
                    literals.Add("quantity");
                    literals.Add("ValidFor");
                    literals.Add("ExpiryDate");
                    literals.Add("ValidMinutesDays");
                    literals.Add("game_profile_id");
                    literals.Add("Frequency");
                    literals.Add("CardTypeId");
                    literals.Add("EntitlementType");
                    literals.Add("OptionalAttribute");
                    literals.Add("ExpiryTime");
                    literals.Add("CustomDataSetId");
                    literals.Add("TicketAllowed");
                    literals.Add("EffectiveAfterDays");
                    literals.Add("FromDate");
                    literals.Add("DateOfLog");
                    literals.Add("UserName");
                    literals.Add("ISActive");
                    break;
                case "SETUPOFFERGROUPS":
                    literals.Add("Show Active Entries");
                    literals.Add("Offer Group");
                    literals.Add("SaleGroupId");
                    literals.Add("Name");
                    literals.Add("IsUpsell");
                    break;
                case "SETUPOFFERGROUPPRODUCTMAP":
                    literals.Add("Offer Group Product Map");
                    literals.Add("Show Active Entries");
                    literals.Add("Group");
                    literals.Add("Search");
                    literals.Add("Map Id");
                    literals.Add("Sale Group");
                    literals.Add("Product");
                    literals.Add("Display Order");
                    break;
                case "PRODUCTDETAILSUPSELLOFFERS":
                    literals.Add("Upsell Offers");
                    literals.Add("Offer Id");
                    literals.Add("Sale Group Id");
                    literals.Add("Offer Product");
                    literals.Add("Offer Message");
                    literals.Add("Effective Date");
                    break;
                case "SETUPSEGMENTDEFINITION":
                    literals.Add("Segment Definition");
                    literals.Add("Definition Id");
                    literals.Add("Name");
                    literals.Add("Applicable Entity");
                    literals.Add("Sequence Order");
                    literals.Add("Mandatory?");
                    break;
                case "POSMANAGEMENTCOUNTERS":
                    literals.Add("POS Management");
                    literals.Add("POS Counters");
                    literals.Add("Counter Id");
                    literals.Add("Counter Name");
                    break;
                case "POSMANAGEMENTMACHINES":
                    literals.Add("POS Name");
                    literals.Add("Computer Name");
                    literals.Add("IP Address");
                    literals.Add("Friendly Name");
                    literals.Add("Legal Entity");
                    literals.Add("Remarks");
                    literals.Add("Options");
                    literals.Add("Peripherals");
                    literals.Add("Counter");
                    literals.Add("Inventory Location");
                    literals.Add("POS Machines");
                    literals.Add("POS Management");
                    literals.Add("Printer");
                    literals.Add("POS Printers");
                    literals.Add("Printer Type");
                    literals.Add("Order Type Group");
                    literals.Add("Print Template");
                    literals.Add("Secondary Printer");
                    literals.Add("Parafait Options");
                    literals.Add("Values");
                    literals.Add("POS Counters");
                    literals.Add("Product Group Inclusion / Exclusions");
                    literals.Add("Include");
                    literals.Add("Exclude");
                    literals.Add("POS Peripherals");
                    break;
                case "POSMANAGEMENTPERIPHERALS":
                    literals.Add("POS Peripherals");
                    literals.Add("DeviceId");
                    literals.Add("Device Name");
                    literals.Add("Device Type");
                    literals.Add("Device SubType");
                    literals.Add("VID");
                    literals.Add("PID");
                    literals.Add("Optional String / Serial Number");
                    break;
                case "DISCOUNTSSETUP":
                    literals.Add("Discounts Setup");
                    literals.Add("Transaction Discounts");
                    literals.Add("Game Play Discounts (Loaded on Cards)");
                    literals.Add("Loyalty Discounts(Automatic on Game Play)");
                    literals.Add("Discount Id");
                    literals.Add("Discount Name");
                    literals.Add("Automatic Apply");
                    literals.Add("Discount Percentage");
                    literals.Add("Discount Amount");
                    literals.Add("Minimum Sale Amount");
                    literals.Add("Minimum Used Credits");
                    literals.Add("Display In POS");
                    literals.Add("Display Order");
                    literals.Add("Manager Approval Required");
                    literals.Add("Variable Discounts");
                    literals.Add("Coupon Mandatory?");
                    literals.Add("Remarks Mandatory");
                    literals.Add("Transaction Profile");
                    literals.Add("Coupons");
                    literals.Add("Schedule");
                    literals.Add("Discounted Products");
                    literals.Add("Populate Products");
                    literals.Add("Clear");
                    literals.Add("De-Select All");
                    literals.Add("Category");
                    literals.Add("Discounted");
                    literals.Add("Product Purchase Criteria");
                    literals.Add("Criteria Id");
                    literals.Add("Min. Quantity");
                    literals.Add("Game");
                    literals.Add("Populate Games");
                    literals.Add("Discounted Games");
                    literals.Add("Discount Coupons");
                    break;
                case "DISCOUNTSSETUPUSEDCOUPONS":
                    literals.Add("Used Coupons");
                    literals.Add("Coupon Number");
                    literals.Add("Transaction Id");
                    literals.Add("Line Id");
                    literals.Add("Import");
                    break;
                case "PRODUCTCATEGORY":
                    literals.Add("Product Category");
                    literals.Add("Category Id");
                    literals.Add("Name");
                    literals.Add("Parent Category");
                    literals.Add("Assign");
                    literals.Add("Accounting Code");
                    break;
                case "FACILITYPOSASSIGNMENT": //Facility -> POS Assignment -> Facility - POS Assignment For
                    literals.Add("POS Machine");
                    literals.Add("POS Assignment");
                    break;
                case "PRICELISTS":  //Product Setup Menu -> Price Lists
                    literals.Add("Price Lists");
                    literals.Add("Price List Id");
                    literals.Add("Price List Name");
                    literals.Add("Price List Products");
                    literals.Add("Price");
                    literals.Add("Effective Date");
                    break;
                case "SETUPSPECIALPRICING": //Product Setup Menu -> Set Up -> Special Pricing Options -> Special Pricing
                    literals.Add("Special Pricing");
                    literals.Add("Special Pricing Options");
                    literals.Add("Pricing Id");
                    literals.Add("Pricing Name");
                    literals.Add("Percentage of Regular Price");
                    literals.Add("Requires Manager Approval");
                    literals.Add("Product Special Prices");
                    literals.Add("Product Name");
                    literals.Add("Price");
                    literals.Add("Special Price");
                    literals.Add("Change Price");
                    literals.Add("Overridden?");
                    break;
                case "PRODUCTSETUPSPECIALPRICING": //Product Setup -> Special Pricing
                    literals.Add("Special Pricing");
                    literals.Add("Product Pricing");
                    literals.Add("Product Pricing Id");
                    literals.Add("Pricing Option");
                    literals.Add("Price");
                    literals.Add("ActiveFlag");
                    break;
                case "SEGMENTDEFINITIONSOURCEMAP": //Product Setup Menu -> Segment Defination Source Map // Updated on 13 Feb
                    literals.Add("Segment Definition Source Map");
                    literals.Add("Definition Source");
                    literals.Add("Definition Source Id");
                    literals.Add("Definition");
                    literals.Add("Data Source Type");
                    literals.Add("Data Source Entity");
                    literals.Add("Data Source Column");
                    literals.Add("Active ?");
                    literals.Add("Source Values");
                    literals.Add("Value Id");
                    literals.Add("List Value");
                    literals.Add("DBQuery");
                    literals.Add("Active ?");
                    literals.Add("Advanced Search");
                    literals.Add("AND");
                    literals.Add("OR");
                    literals.Add("Field");
                    literals.Add("Criteria");
                    literals.Add("Condition");
                    literals.Add("Value");
                    literals.Add("Add");
                    literals.Add("Date Lookup");
                    literals.Add("Please select the data source entity.");
                    break;
                case "DISPLAYGROUP": //Product Setup Menu -> Product Display Groups
                    literals.Add("Display Group");
                    literals.Add("Enter Display Group");
                    literals.Add("Display Group");
                    literals.Add("Sort Order");
                    break;
                case "WAIVERS": //Product Setup Menu -> Waivers
                    literals.Add("Waiver Set");
                    literals.Add("Waiver Name");
                    literals.Add("Active ?");
                    literals.Add("Signing Options");
                    literals.Add("Waiver Set Details");
                    literals.Add("Waiver Detail");
                    literals.Add("Waiver File Name");
                    literals.Add("Effective Date");
                    literals.Add("Waiver Set Details Language");
                    literals.Add("Language");
                    literals.Add("Waiver Language File Name");
                    literals.Add("Waiver Signing Option");
                    literals.Add("Description");
                    literals.Add("Waiver Set Name");
                    literals.Add("Valid For Days");
                    literals.Add("Effective From Date");
                    literals.Add("Waivers Language");
                    break;
                case "AUDITLOG-PRODUCTS": //Product Setup Menu -> Product Setup -> Product Details -> Audits
                    literals.Add("product_id");
                    literals.Add("product_name");
                    literals.Add("description");
                    literals.Add("product_type_id");
                    literals.Add("price");
                    literals.Add("credits");
                    literals.Add("courtesy");
                    literals.Add("bonus");
                    literals.Add("time");
                    literals.Add("sort_order");
                    literals.Add("tax_id");
                    literals.Add("tickets");
                    literals.Add("face_value");
                    literals.Add("display_group");
                    literals.Add("ticket_allowed");
                    literals.Add("vip_card");
                    literals.Add("InternetKey");
                    literals.Add("TaxInclusivePrice");
                    literals.Add("InventoryProductCode");
                    literals.Add("ExpiryDate");
                    literals.Add("AvailableUnits");
                    literals.Add("AutoCheckOut");
                    literals.Add("CheckInFacilityId");
                    literals.Add("MaxCheckOutAmout");
                    literals.Add("POSTypeId");
                    literals.Add("CustomDataSetId");
                    literals.Add("CardTypeId");
                    literals.Add("TrxRemarksMandatory");
                    literals.Add("CategoryId");
                    literals.Add("OverridePrintTemplateId");
                    literals.Add("StartDate");
                    literals.Add("ButtonColor");
                    literals.Add("AutoGenerateCardNumber");
                    literals.Add("QuantityPrompt");
                    literals.Add("OnlyForVIP");
                    literals.Add("AllowPriceOverride");
                    literals.Add("RegisteredCustomerOnly");
                    literals.Add("ManagerApprovalRequired");
                    literals.Add("MinimumUserPrice");
                    literals.Add("TextColor");
                    literals.Add("Font");
                    literals.Add("VerifiedCustomerOnly");
                    literals.Add("Modifier");
                    literals.Add("MinimumQuantity");
                    literals.Add("DisplayInPOS");
                    literals.Add("TrxHeaderRemarksMandatory");
                    literals.Add("CardCount");
                    literals.Add("ImageFileName");
                    literals.Add("AttractionMasterScheduledId");
                    literals.Add("AdvancePercentage");
                    literals.Add("AdvanceAmount");
                    literals.Add("EmailTemplateId");
                    literals.Add("MaximumTime");
                    literals.Add("MinimumTime");
                    literals.Add("CardValidFor");
                    literals.Add("AdditionalTaxInclusive");
                    literals.Add("AdditionalPrice");
                    literals.Add("AdditionalTaxId");
                    literals.Add("WaiverRequired");
                    literals.Add("SegmentCategoryId");
                    literals.Add("CardExpiryDate");
                    literals.Add("InvokeCustomerRegistration");
                    literals.Add("ProductDisplayGroupFormatId");
                    literals.Add("MaxQtyPerDay");
                    literals.Add("HsnSacCode");
                    literals.Add("WebDescription");
                    literals.Add("OrderTypeId");
                    literals.Add("ZoneId");
                    literals.Add("LockerExpiryInHours");
                    literals.Add("LockerExpiryDate");
                    literals.Add("WaiverSetId");
                    literals.Add("isGroupMeal");
                    literals.Add("MembershipID");
                    literals.Add("DateOfLog");
                    literals.Add("UserName");
                    literals.Add("active_flag");
                    literals.Add("MaxCheckOutAmount");
                    literals.Add("AttractionMasterScheduleId");
                    literals.Add("ExternalSystemReference");
                    break;
                case "AUDITLOG-PRODUCTCREDITPLUS": //Product Setup Menu -> Product Setup -> Product Details -> Credit Plus -> Product Credit Plus -> Audits
                    literals.Add("ProductCreditPlusId");
                    literals.Add("CreditPlus");
                    literals.Add("Refundable");
                    literals.Add("Remarks");
                    literals.Add("Product_id");
                    literals.Add("CreditPlusType");
                    literals.Add("PeriodFrom");
                    literals.Add("PeriodTo");
                    literals.Add("ValidForDays");
                    literals.Add("ExtendOnReload");
                    literals.Add("TimeFrom");
                    literals.Add("TimeTo");
                    literals.Add("Minutes");
                    literals.Add("Monday");
                    literals.Add("Tuesday");
                    literals.Add("Wednesday");
                    literals.Add("Thursday");
                    literals.Add("Friday");
                    literals.Add("Saturday");
                    literals.Add("Sunday");
                    literals.Add("TicketAllowed");
                    literals.Add("Frequency");
                    literals.Add("PauseAllowed");
                    literals.Add("LastUpdatedDate");
                    break;
                case "AUDITLOG-PRODUCTCREDITPLUSCONSUMPTION": //Product Setup Menu -> Product Setup -> Product Details -> Credit Plus -> Credit Plus Consumption Rules -> Audits
                    literals.Add("PKId");
                    literals.Add("ProductCreditPlusId");
                    literals.Add("POSTypeId");
                    literals.Add("ExpiryDate");
                    literals.Add("GameId");
                    literals.Add("GameProfileId");
                    literals.Add("Product_id");
                    literals.Add("QuantityLimit");
                    literals.Add("CategoryId");
                    literals.Add("DiscountAmount");
                    literals.Add("DiscountPercentage");
                    literals.Add("DiscountedPrice");
                    literals.Add("OrderTypeId");
                    literals.Add("LastUpdatedDate");
                    literals.Add("DateOfLog");
                    literals.Add("UserName");
                    break;
                case "AUDITLOG-PRODUCTGAMES": //Product Setup Menu -> Product Setup -> Product Details -> Games -> Audits
                    literals.Add("product_game_id");
                    literals.Add("product_id");
                    literals.Add("game_id");
                    literals.Add("quantity");
                    literals.Add("ValidFor");
                    literals.Add("ExpiryDate");
                    literals.Add("ValidMinutesDays");
                    literals.Add("game_profile_id");
                    literals.Add("Frequency");
                    literals.Add("CardTypeId");
                    literals.Add("EntitlementType");
                    literals.Add("OptionalAttribute");
                    literals.Add("ExpiryTime");
                    literals.Add("CustomDataSetId");
                    literals.Add("TicketAllowed");
                    literals.Add("EffectiveAfterDays");
                    literals.Add("FromDate");
                    literals.Add("Monday");
                    literals.Add("Tuesday");
                    literals.Add("Wednesday");
                    literals.Add("Thursday");
                    literals.Add("Friday");
                    literals.Add("Saturday");
                    literals.Add("Sunday");
                    literals.Add("LastUpdatedDate");
                    break;
                case "FACILITYSEATLAYOUT": //Product Setup Menu -> Facility Seat Layout
                    literals.Add("Add Facility");
                    literals.Add("Select Facility");
                    literals.Add("Total Seats");
                    literals.Add("Screen / Stage This Way");
                    literals.Add("(Double - Click to Change)");
                    literals.Add("Add New Row");
                    literals.Add("Add New Column");
                    literals.Add("Add Passage");
                    literals.Add("Add Aisle");
                    literals.Add("Disable");
                    literals.Add("Enable");
                    literals.Add("Remove Seat");
                    literals.Add("Remove Seat");
                    literals.Add("Accessibile");
                    literals.Add("Add Seat");
                    literals.Add(" Max &1 characters");
                    literals.Add("Facility Seat Layout");
                    literals.Add("Facility Name");
                    literals.Add("Facility");
                    literals.Add("Seat");
                    break;
                case "PRODUCTSETUPMENU": //Product Setup Menu
                    literals.Add("Choose by Product Type");
                    literals.Add("Cards");
                    literals.Add("Non - Card Sales(F & B etc)");
                    literals.Add("Combo");
                    literals.Add("Bookings");
                    literals.Add("Attractions");
                    literals.Add("Check - In / Check - Out");
                    literals.Add("Rental");
                    literals.Add("Vouchers");
                    literals.Add("All");
                    literals.Add("Set Up");
                    literals.Add("Product Category");
                    literals.Add("Product Modifiers");
                    literals.Add("Price List");
                    literals.Add("Facility");
                    literals.Add("Facility Seat Layout");
                    literals.Add("Attraction Plays");
                    literals.Add("Attraction Schedule");
                    literals.Add("Special Pricing Options");
                    literals.Add("Format Display Groups");
                    literals.Add("POS Exclusions");
                    literals.Add("Segment Definition");
                    literals.Add("Segment Definition Mapping");
                    literals.Add("Display Groups");
                    literals.Add("Offer Groups");
                    literals.Add("Offer Group Product Map");
                    literals.Add("Waivers");
                    literals.Add("Product Setup Menu");
                    literals.Add("Facility Map");
                    break;
                case "POSPERIPHERALS": //Product Setup Menu -> POS Exclusions -> POS Machines -> Peripherals
                    literals.Add("DeviceId");
                    literals.Add("Device Name");
                    literals.Add("Device Type");
                    literals.Add("Device SubType");
                    literals.Add("VID");
                    literals.Add("PID");
                    literals.Add("Optional String / Serial Number");
                    literals.Add("Last Updated User");
                    break;
                case "PRODUCTSETMODIFIERS": //Product Setup Menu -> Product Modifiers // Updated 12 Feb
                    literals.Add("Set Id");
                    literals.Add("Modifier Set Name");
                    literals.Add("Min Quantity");
                    literals.Add("Max Quantity");
                    literals.Add("Free Quantity");
                    literals.Add("Parent Modifier Set");
                    literals.Add("Last Update User");
                    literals.Add("Modifier Set Details");
                    literals.Add("Modifier Product");
                    literals.Add("Price");
                    literals.Add("Parent Modifier");
                    literals.Add("Parent Modifiers");
                    literals.Add("Modifier Id");
                    literals.Add("Modifier Name");
                    literals.Add("Product Modifiers");
                    literals.Add("Site Id");
                    break;
                case "PRODUCTDISPLAYGROUPS": //Product Setup Menu -> Product Setup -> ..(Between Display Group and Display Order)
                    literals.Add("Select Display Groups");
                    literals.Add("Display Group");
                    break;
                case "PRODUCTCATEGORYACCOUNTINGCODE": // Product Category ---> Assign ---> Accounting Code Literals
                    literals.Add("Enter Accounting Code Details");
                    literals.Add("Transaction");
                    literals.Add("Accounting Code");
                    break;
                case "DISCOUNTCOUPONS":  // Discount Coupons Literals
                    literals.Add("Coupons Header");
                    literals.Add("Effective Date");
                    literals.Add("Expiry Date");
                    literals.Add("Coupon Count");
                    literals.Add("Expires in Days");
                    literals.Add("Print Coupons");
                    literals.Add("Sequential");
                    literals.Add("Show Active Only");
                    literals.Add("Coupon Number");
                    literals.Add("Transaction Id");
                    literals.Add("Used Coupons");
                    literals.Add("Coupon Set Id ");
                    literals.Add("From Number ");
                    literals.Add("To Number ");
                    literals.Add("Count");
                    literals.Add("Coupon Expiry Date");
                    literals.Add("TransactionId");
                    literals.Add("Coupons");
                    literals.Add("Discount Coupons");
                    literals.Add("Import");
                    literals.Add("Used Count");
                    break;
                case "PRODUCTDESCRIPTION":   // Edit Literals in Product Details Page
                    literals.Add("Edit &1");
                    literals.Add("Translate");
                    literals.Add("Save Translation");
                    literals.Add("Choose Language");
                    literals.Add("Translate to language");
                    literals.Add("PRODUCT_NAME");
                    literals.Add("PRODUCT_IMAGE");
                    literals.Add("DESCRIPTION");
                    break;
                case "MEMBERSHIPEXCLUSIONRULE": // Membership Exclusion Rule For Product in Product Details Page // Updated 12-Feb
                    literals.Add("Membership Exclusion Rule for Product");
                    literals.Add("Membership");
                    literals.Add("DisAllowed?");
                    literals.Add("Populate Membership");
                    literals.Add("Save Membership");
                    literals.Add("Last Update Date");
                    break;
                case "FACILITY(ATTRACTION/CHECK-IN)": // Facility (Attraction/Check-In) Literals 
                    literals.Add("Facility (Attraction / Check-In)");
                    literals.Add("Facility Id");
                    literals.Add("Facility Name");
                    literals.Add("Capacity");
                    literals.Add("GraceTime");
                    literals.Add("Screen Position");
                    literals.Add("Seat Layout");
                    literals.Add("POS Assignment");
                    literals.Add("Faciity Assignment");
                    literals.Add("Faciity Waivers");
                    break;
                case "FORMATDISPLAYGROUPS": // Format Display Groups Literals
                    literals.Add("Format Display Groups");
                    literals.Add("Button Color");
                    literals.Add("Text Color");
                    literals.Add("Font");
                    literals.Add("Reset");
                    literals.Add("Move Next");
                    literals.Add("Move Prev");
                    literals.Add("Image File");
                    break;
                case "SCHEDULEXCLUSION":    // Schedule Exclusion Literals 
                    literals.Add("Schedule Exclusion");
                    literals.Add("Exclusion Id");
                    literals.Add("Exclusion Date");
                    literals.Add("Day");
                    literals.Add("Include Date?");
                    break;
                case "PRODUCTSETUPCALENDAR":     // Product Calender Literals 
                    literals.Add("Day");
                    literals.Add("Date");
                    literals.Add("From Time");
                    literals.Add("To Time");
                    literals.Add("Show / Hide");
                    literals.Add("Product Calendar");
                    break;
                case "ATTRACTIONSCHEDULE":  // Attraction Schedule Literals
                    literals.Add("Schedule");
                    literals.Add("Master Schedule");
                    literals.Add("Master Schedule Name");
                    literals.Add("ActiveFlag");
                    literals.Add("Schedules");
                    literals.Add("Schedule Name");
                    literals.Add("Schedule Hour");
                    literals.Add("Minute");
                    literals.Add("Schedule To Hour");
                    literals.Add("Attraction Play");
                    literals.Add("Fixed Schedule");
                    break;
                case "PRODUCTCREDITPLUS": // Extended Credits (Credit Plus) For Literals 
                    literals.Add("ProductCreditPlus");
                    literals.Add("Audit Log");
                    literals.Add("Credit Plus Type");
                    literals.Add("Credit Plus Amount");
                    literals.Add("Valid For Days");
                    literals.Add("Extended Credits (CreditPlus) for");
                    literals.Add("Pause Allowed");
                    literals.Add("Minutes");
                    literals.Add("Period From");
                    literals.Add("Period To");
                    literals.Add("Frequency");
                    literals.Add("Extend On Reload");
                    literals.Add("Refundable?");
                    literals.Add("Time From");
                    literals.Add("Time To");
                    literals.Add("Monday");
                    literals.Add("Tuesday");
                    literals.Add("Wednesday");
                    literals.Add("Thursday");
                    literals.Add("Friday");
                    literals.Add("Saturday");
                    literals.Add("Sunday");
                    literals.Add("Ticket Allowed");
                    literals.Add("Remarks");
                    literals.Add("Audit");
                    literals.Add("Credit Plus Consumption Rules");
                    literals.Add("POS Counter");
                    literals.Add("Order Type");
                    literals.Add("Game Profile");
                    literals.Add("Games");
                    literals.Add("Discount %");
                    literals.Add("Discount Amount");
                    literals.Add("Discounted Price");
                    literals.Add("Daily Limit");
                    literals.Add("Incl. / Excl.Dates");

                    literals.Add("Effective After Minutes"); // Added on 16-Apr-2020 by Mushahid Faizan

                    break;
                case "SEGMENTS":  // Segments Literals  
                    literals.Add("Segments for");
                    literals.Add("Segment Values");
                    literals.Add("Segment Id");
                    break;
                case "HTMLEDITOR": // Html Editor Literals 
                    literals.Add("Choose Language");
                    literals.Add("Edit HTML");
                    break;
                case "CUSTOMATTRIBUTES": // Custom Attributes Literals 
                    literals.Add("Segments For");
                    literals.Add("Segment Values");
                    literals.Add("Segment Id");
                    literals.Add("Location");
                    literals.Add("SKU Data For");
                    literals.Add("SKU Values");
                    literals.Add("SKU ID");
                    break;
                case "GENERATEBARCODE": // Generate Bar Code Literals 
                    literals.Add("Generate BarCode");
                    literals.Add("Text to encode");
                    literals.Add("Bar weight");
                    literals.Add("Make barcode");
                    literals.Add("Exit");
                    literals.Add("Print");
                    literals.Add("OK");
                    break;
                case "TAXSETUP": // Tax Setup Literals 
                    literals.Add("Purchase Tax");
                    literals.Add("Tax Setup");
                    literals.Add("Tax Set");
                    literals.Add("Tax Id");
                    literals.Add("Tax Name");
                    literals.Add("Tax Percentage");
                    literals.Add("Tax Structure");
                    literals.Add("Tax Structure Id");
                    literals.Add("Structure Name");
                    literals.Add("Percentage");
                    literals.Add("Parent Structure");
                    break;
                case "CHECKOUTPRICES": // Check-Out Prices Literals
                    literals.Add("Check-Out Prices");
                    literals.Add("Time Slab");
                    literals.Add("Price");
                    break;
                case "ATTRACTIONMASTERSCHEDULE": // Attraction Master Schedule Literals 
                    literals.Add("Schedule");
                    literals.Add("Master Schedule");
                    literals.Add("Master Schedule Name");
                    literals.Add("ActiveFlag");
                    literals.Add("Schedules");
                    literals.Add("Schedule Name");
                    literals.Add("Schedule Hour");
                    literals.Add("Minute");
                    literals.Add("Schedule To Hour");
                    literals.Add("Attraction Play");
                    literals.Add("Fixed Schedule");
                    literals.Add("Schedule Rules");
                    literals.Add("Day");
                    literals.Add("FromDate");
                    literals.Add("Todate");
                    literals.Add("Units");
                    break;
                case "COMBOPRODUCTDETAILS": // Product Details Combo Details in Combo Button Literals 
                    literals.Add("Combo Product Details for");
                    literals.Add("Child Product");
                    literals.Add("Price");
                    break;
                case "BOOKINGPRODUCTDETAILS": // Booking Product Details
                    literals.Add("Booking Product Details For");
                    literals.Add("Child Product");
                    literals.Add("Display Group");
                    literals.Add("Price Inclusive");
                    literals.Add("Additional Product");
                    literals.Add("Sort Order");
                    break;
                case "PRODUCTSSETUP": // Product Setup Literals 
                    literals.Add("Product Setup");
                    literals.Add("Edit");
                    literals.Add("Product Name");
                    literals.Add("POS Counter");
                    literals.Add("Display Group");
                    literals.Add("Product Display Group");
                    literals.Add("Button Color");
                    literals.Add("Text Color");
                    literals.Add("ImageFileName");
                    literals.Add("Font");
                    literals.Add("X");
                    literals.Add("Card Face Value");
                    literals.Add("Price");
                    literals.Add("Advance %");
                    literals.Add("Advance Amount");
                    literals.Add("Display In POS?");
                    literals.Add("Units");
                    literals.Add("Available Units");
                    literals.Add("From Date");
                    literals.Add("To Date");
                    literals.Add("Email Template");
                    literals.Add("Minimum Time");
                    literals.Add("Check In Facility");
                    literals.Add("Auto Check-Out ?");
                    literals.Add("Tax Inclusive");
                    literals.Add("Max Check-Out Amount");
                    literals.Add("Credits");
                    literals.Add("Courtesy");
                    literals.Add("Bonus");
                    literals.Add("Time");
                    literals.Add("Games");
                    literals.Add("CreditPlus");
                    literals.Add("Tickets");
                    literals.Add("Ticket Allowed");
                    literals.Add("Vip Card");
                    literals.Add("Only For VIP");
                    literals.Add("Start Date");
                    literals.Add("ExpiryDate");
                    literals.Add("TaxName");
                    literals.Add("Tax %");
                    literals.Add("Membership");
                    literals.Add("Custom");
                    literals.Add("Trx Remarks Mandatory");
                    literals.Add("Effective Price");
                    literals.Add("Final Price");
                    literals.Add("Invoke Customer Registration");
                    literals.Add("Registered Customer Only");
                    literals.Add("Verified Customer Only");
                    literals.Add("Manager Approval Required");
                    literals.Add("Auto Generate Card Number");
                    literals.Add("Quantity Prompt?");
                    literals.Add("Card Count");
                    literals.Add("Allow Price Override");
                    literals.Add("Min.User Price");
                    literals.Add("Min.Quantity");
                    literals.Add("Modifier");
                    literals.Add("Order Type");
                    literals.Add("Inv Product Code");
                    literals.Add("Last Updated User");
                    literals.Add("New");
                    literals.Add("Calendar");
                    literals.Add("Special Pricing");
                    literals.Add("Discounts");
                    literals.Add("Show Only Active Products");
                    literals.Add("POS Counter");
                    literals.Add("Import Inv.Products");
                    literals.Add("Card Valid For");
                    literals.Add("Attraction Schedule");
                    literals.Add("Check-Out Pricing");
                    literals.Add("Assign");
                    literals.Add("Attraction");
                    literals.Add("Attraction Details");
                    literals.Add("Audit");
                    literals.Add("Booking Details");
                    literals.Add("Bookings");
                    literals.Add("Card Expery Date");
                    literals.Add("Card Expiry Date");
                    literals.Add("Card Valid For (in Days)");
                    literals.Add("Card Valid For(in Days)");
                    literals.Add("Cards");
                    literals.Add("Clear");
                    literals.Add("Clear Date");
                    literals.Add("Combo");
                    literals.Add("Combo Details");
                    literals.Add("Coupons");
                    literals.Add("Duplicate");
                    literals.Add("Entitlements");
                    literals.Add("Exclusion");
                    literals.Add("Face Value");
                    literals.Add("Get Products");
                    literals.Add("HSN / SAC Code");
                    literals.Add("Inventory");
                    literals.Add("Last Update User");
                    literals.Add("Line Remarks Mandatory");
                    literals.Add("Manual");
                    literals.Add("Maximum Quantity(Per Day)");
                    literals.Add("Maximum Time(Mins)");
                    literals.Add("Minimum Time(Mins)");
                    literals.Add("Modifiers");
                    literals.Add("Next");
                    literals.Add("Previous");
                    literals.Add("Pricing");
                    literals.Add("Print Bar Code");
                    literals.Add("Product Id");
                    literals.Add("Product Image");
                    literals.Add("Product Saved Successfully");
                    literals.Add("Quantity Prompt");
                    literals.Add("Segments");
                    literals.Add("Tax Inclusive?");
                    literals.Add("Translate");
                    literals.Add("Upsell Offers");
                    literals.Add("VIP Card");
                    literals.Add("Waiver Set");
                    literals.Add("Web Description");
                    literals.Add("Product Games / Entitlements");
                    literals.Add("Miscellaneous");
                    literals.Add("Miscellaneous Details");
                    literals.Add("Booking Details");
                    literals.Add("Attraction Details");
                    literals.Add("Inventory Details");
                    literals.Add("Entitlements Details");
                    literals.Add("Price Details");
                    literals.Add("Product Details");
                    literals.Add("Product Calendar");
                    literals.Add("Membership Exclusion Rule for Product");
                    literals.Add("Product Modifiers");
                    literals.Add("Extended Credits (CreditPlus) for");
                    literals.Add("Product Display Groups");
                    literals.Add("Show Only Sellable Products");
                    literals.Add("Maintain Products");
                    literals.Add("Location");
                    literals.Add("Audit Log");
                    literals.Add("Products");
                    literals.Add("Custom Attributes Data For Product");
                    literals.Add("Html Editor");
                    literals.Add("Translate Description");
                    literals.Add("Translate Product Image");
                    literals.Add("Group Meal");
                    literals.Add("Product Discounts");
                    literals.Add("Child Product");
                    literals.Add("Facility Map");
                    literals.Add("CheckInOut");
                    literals.Add("Rental");
                    literals.Add("Zones");
                    literals.Add("Locker Validity In Hours");
                    literals.Add("Locker Expiry Date");
                    literals.Add("Facility Map for Product");
                    literals.Add("Upload Inv. Products");

                    literals.Add("External System Reference");  // Added on 16-Apr-2020 by Mushahid Faizan

                    break;
                case "PRODUCTTYPE":               // Setup --> Product Type Updated on 12 Feb
                    literals.Add("Product Type Id");
                    literals.Add("Report Group");
                    literals.Add("Card Sales?");
                    literals.Add("Order Type");
                    break;
                case "ATTRACTIONPLAYS":
                    literals.Add("Attraction Plays");
                    literals.Add("Play Name");
                    literals.Add("Price");
                    literals.Add("Expiry Date");
                    break;

                case "ADVANCEDSEARCH":
                    literals.Add("Advanced Search");
                    literals.Add("AND");
                    literals.Add("OR");
                    literals.Add("Field");
                    literals.Add("Conditions");
                    literals.Add("Value");
                    literals.Add("Add");
                    literals.Add("Date Lookup");
                    literals.Add("Criteria");
                    literals.Add("Close");
                    literals.Add("Is InActive?");
                    literals.Add("Is Active?");
                    literals.Add("Date Range");
                    literals.Add("Is Null");
                    literals.Add("Is Not Null");
                    break;
                case "TAXPRICING":
                    literals.Add("Tax Structure Pricing");
                    literals.Add("Product Name");
                    literals.Add("Tax Name");
                    literals.Add("Tax Price Details");
                    literals.Add("Structure Name");
                    literals.Add("Tax Percentage");
                    literals.Add("Price");
                    literals.Add("Active");
                    break;
                case "PARAFAITOPTIONS":
                    literals.Add("Parafait Options");
                    literals.Add("Options");
                    literals.Add("Values");
                    literals.Add("Option Name");
                    literals.Add("Option Value");
                    literals.Add("ActiveFlag");
                    break;
                case "INVENTORYINPRODUCT":
                    literals.Add("Maintain Products");
                    literals.Add("Item Details");
                    literals.Add("Location");
                    literals.Add("Purchase Tax");
                    literals.Add("Product Id");
                    literals.Add("Product Image");
                    literals.Add("Code");
                    literals.Add("Product Name");
                    literals.Add("Clear");
                    literals.Add("Add Category");
                    literals.Add("Add UOM");
                    literals.Add("Reorder Point");
                    literals.Add("Reorder Quantity");
                    literals.Add("Unit of Measure");
                    literals.Add("Add UOM");
                    literals.Add("Add Tax");
                    literals.Add("Tax Inclusive?");
                    literals.Add("Bar Code");
                    literals.Add("Edit Bar Codes");
                    literals.Add("Turn-in PIT");
                    literals.Add("Price in Tickets");
                    literals.Add("Generate UPC Bar Code");
                    literals.Add("Auto Update PIT?");
                    literals.Add("Inbound Location");
                    literals.Add("Add Location");
                    literals.Add("Outbound Location");
                    literals.Add("Preferred Vendor");
                    literals.Add("Add Vendor");
                    literals.Add("Custom");
                    literals.Add("Item Attribute");
                    literals.Add("Inventory");
                    literals.Add("Sellable?");
                    literals.Add("Redeemable?");
                    literals.Add("Lot Controlled?");
                    literals.Add("Market List Item?");
                    literals.Add("Expiry Type");
                    literals.Add("Issuing Approach");
                    literals.Add("SKU Segments");
                    literals.Add("Costing");
                    literals.Add("Inner Cost Unit");
                    literals.Add("Lower Cost Unit");
                    literals.Add("Variance %");
                    literals.Add("Sale Price");
                    literals.Add("Cost");
                    literals.Add("Upper Cost Limit");
                    literals.Add("Last Purch Price");
                    literals.Add("Markup %");
                    literals.Add("Build from BOM");
                    literals.Add("Remarks");
                    literals.Add("Bill of Material");
                    literals.Add("Add Child Product for");
                    literals.Add("Child Product Code");
                    literals.Add("Inventory Details");
                    literals.Add("Add Barcode");
                    literals.Add("SKU Data");
                    break;
                case "PRODUCTUOM":
                    literals.Add("Unit of Measure");
                    literals.Add("UOMId");
                    literals.Add("Remarks");
                    break;
                case "BARCODE":
                    literals.Add("Add Barcode");
                    literals.Add("Barcode for ");
                    literals.Add("BarCode");
                    literals.Add("Barcode Id");
                    literals.Add("Barcode");
                    literals.Add("Generate");
                    literals.Add("Generate Barcode");
                    break;
                case "PRODUCTLOCATION":
                    literals.Add("Location");
                    literals.Add("Location Id");
                    literals.Add("Remarks Mandatory");
                    literals.Add("Available To Sell");
                    literals.Add("Is Store");
                    literals.Add("Turn In Location");
                    literals.Add("Allow Mass Update");
                    literals.Add("Location Type Id");
                    literals.Add("Barcode");
                    literals.Add("Custom");
                    literals.Add("External System Reference");
                    literals.Add("Generate BarCode");
                    literals.Add("Import Machines");
                    break;
                case "PRODUCTLOCATIONIMPORTMACHINES":
                    literals.Add("Import Machines");
                    literals.Add("Select Location Type");
                    literals.Add("Import All Machines");
                    literals.Add("Select");
                    literals.Add("MachineId");
                    literals.Add("MachineName");
                    break;
                case "PRODUCTVENDOR":
                    literals.Add("VendorName");
                    literals.Add("View Vendors");
                    literals.Add("Vendor Id");
                    literals.Add("Website");
                    literals.Add("Vendor Address");
                    literals.Add("Address Line 1");
                    literals.Add("Address Line 2");
                    literals.Add("City");
                    literals.Add("State");
                    literals.Add("Code");
                    literals.Add("Contact Person");
                    literals.Add("Contact Name");
                    literals.Add("Phone");
                    literals.Add("Fax");
                    literals.Add("Email");
                    literals.Add("Zip Code");
                    literals.Add("Country");
                    literals.Add("Address Remarks");
                    literals.Add("VAT/Tax No");
                    literals.Add("Markup %");
                    literals.Add("Remarks");
                    literals.Add("Duplicate");
                    break;
                case "SKUDATA":
                    literals.Add("SKU Data");
                    literals.Add("Sku Data For");
                    literals.Add("SKU Values");
                    literals.Add("SKU ID");
                    break;
                case "PRODUCTBOM":
                    literals.Add("Bill of Meterial");
                    literals.Add("Add Child Product for");
                    literals.Add("Child Product Code");
                    break;
                case "ALLOWEDPRODUCTS":
                    literals.Add("Allowed Product");
                    literals.Add("Allowed Products");
                    literals.Add("Default Rental Product?");
                    break;
                case "FACILITYMAP":
                    literals.Add("Facility Map Name");
                    literals.Add("Cancellation Product");
                    literals.Add("Grace Time");
                    literals.Add("Mapped Facilities");
                    literals.Add("Facility");
                    literals.Add("Schedule");
                    literals.Add("Allowed Products");
                    literals.Add("Facility Map");
                    break;
                case "MAPPRODUCTSTOFACILITYMAP":
                    literals.Add("Facility Map For Product");
                    literals.Add("Facility Map");
                    literals.Add("Default Rental Product?");
                    break;
                case "UPLOADINVENTORYPRODUCTS":
                    literals.Add("Upload Products");
                    literals.Add("Defaults");
                    literals.Add("Inbound Location");
                    literals.Add("Outbound Location");
                    literals.Add("Inventory Item?");
                    literals.Add("File Format");
                    literals.Add("Download");
                    literals.Add("Upload");
                    break;
                case "FACILITYWAIVER":
                    literals.Add("Facility Waivers");
                    literals.Add("WaiverSetId");
                    literals.Add("EffectiveFrom");
                    literals.Add("EffectiveTo");
                    literals.Add("EffectiveFrom");
                    break;
                case "AUDITPRICEPRODUCTLIST":
                    literals.Add("DateOfLog");
                    literals.Add("Type");
                    literals.Add("UserName");
                    literals.Add("PriceListProductId");
                    literals.Add("PriceListId");
                    literals.Add("ProductId");
                    literals.Add("Price");
                    literals.Add("EffectiveDate");
                    literals.Add("LastUpdatedDate");
                    literals.Add("LastUpdatedBy");
                    literals.Add("Guid");
                    literals.Add("SynchStatus");
                    literals.Add("site_id");
                    literals.Add("MasterEntityId");
                    literals.Add("CreatedBy");
                    literals.Add("CreationDate");
                    literals.Add("IsActive");
                    break;
            }
            log.LogMethodExit(literals);
            return literals;
        }
    }
}

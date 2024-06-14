/********************************************************************************************
 * Project Name - Table factory
 * Description  - Table factory UI
 * 
 **************
 *Version      Date          Modified By       Remarks
 ********************************************************************************************
  *2.90.0      03-Jun-2020   Deeksha        Modified : Bulk product publish & weighted avg costing changes
  *2.130.0     12-Jul-2021   Lakshminarayana      Modified : Static menu enhancement
  *2.140.0     18-Nov-2021   Abhishek        Modified : Added tables to be published
  *2.130.10    22-Aug-2022   Abhishek       Modified : Added Users Publish Entities
  *2.150.3     29-Mar-2023   Abhishek       Modified : Added EntityOverrideDates as Publish Entity
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System.Collections.Concurrent;


namespace Semnox.Parafait.Publish
{
    public class TableFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<string, Table> tableNameTableMap = new ConcurrentDictionary<string, Table>();

        public static Table GetTable(ExecutionContext executionContext, string tableName)
        {
            Table table = GetSupportedTable(executionContext, tableName);
            if (table == null)
            {
                log.Warn("Batch publish is not fully supported for the table : " + tableName);
                table = new Table(executionContext, tableName);
            }
            return table;
        }

        private static Table GetSupportedTable(ExecutionContext executionContext, string tableName)
        {
            log.LogMethodEntry(tableName);
            Table table = null;
            if (tableNameTableMap.ContainsKey(tableName.ToLower()))
            {
                table = tableNameTableMap[tableName.ToLower()];
                log.LogMethodExit(table);
                return table;
            }
            switch (tableName.ToLower())
            {
                case "products":
                    {
                        table = new Table(executionContext, "Products");
                        table.AddChild(GetTable(executionContext, "ProductGames"));
                        table.AddChild(GetTable(executionContext, "ProductCreditPlus"));
                        table.AddChild(GetTable(executionContext, "ProductCalendar"));
                        table.AddChild(GetTable(executionContext, "ProductDiscounts"));
                        table.AddChild(GetTable(executionContext, "ProductSpecialPricing"));
                        table.AddChild(GetTable(executionContext, "UpsellOffers"), "ProductId");
                        table.AddChild(GetTable(executionContext, "ComboProduct"), "Product_Id");
                        table.AddChild(GetTable(executionContext, "CardTypeRule"));
                        table.AddChild(GetTable(executionContext, "ProductsDisplayGroup"));
                        table.AddChild(GetTable(executionContext, "ObjectTranslations"), "ElementGuid");
                        table.AddChild(GetTable(executionContext, "Product"));
                        table.AddChild(GetTable(executionContext, "ProductModifiers"));
                        table.AddChild(GetTable(executionContext, "ProductSubscription"));
                        table.AddChild(GetTable(executionContext, "CheckInPrices"));
                        break;
                    }
                case "productgames":
                    {
                        table = new Table(executionContext,"ProductGames");
                        table.AddChild(GetTable(executionContext,"ProductGameExtended"));
                        table.AddChild(GetTable(executionContext,"EntityOverrideDates"), "EntityGuid");
                        break;
                    }
                case "productgameextended":
                    {
                        table = new Table(executionContext,"ProductGameExtended");
                        break;
                    }
                case "productcreditplus":
                    {
                        table = new Table(executionContext,"ProductCreditPlus");
                        table.AddChild(GetTable(executionContext,"ProductCreditPlusConsumption"));
                        table.AddChild(GetTable(executionContext,"EntityOverrideDates"), "EntityGuid");
                        break;
                    }
                case "entityoverridedates":
                    {
                        table = new EntityOverrideDateTable(executionContext);
                        break;
                    }
                case "productcreditplusconsumption":
                    {
                        table = new Table(executionContext,"ProductCreditPlusConsumption");
                        break;
                    }
                case "productcalendar":
                    {
                        table = new Table(executionContext,"ProductCalendar");
                        break;
                    }
                case "productdiscounts":
                    {
                        table = new Table(executionContext,"ProductDiscounts");
                        break;
                    }
                case "productspecialpricing":
                    {
                        table = new Table(executionContext, "ProductSpecialPricing");
                        break;
                    }
                case "upselloffers":
                    {
                        table = new Table(executionContext, "UpsellOffers");
                        break;
                    }
                case "comboproduct":
                    {
                        table = new Table(executionContext, "ComboProduct");
                        break;
                    }
                case "cardtyperule":
                    {
                        table = new Table(executionContext, "CardTypeRule");
                        break;
                    }
                case "objecttranslations":
                    {
                        table = new ObjectTranslationTable(executionContext);
                        break;
                    }
                case "product":
                    {
                        table = new Table(executionContext, "Product");
                        table.AddChild(GetTable(executionContext, "ProductBarcode"));
                        break;
                    }
                case "productmodifiers":
                    {
                        table = new Table(executionContext, "ProductModifiers");
                        break;
                    }
                case "productsubscription":
                    {
                        table = new Table(executionContext, "ProductSubscription");
                        break;
                    }
                case "checkinprices":
                    {
                        table = new Table(executionContext, "CheckInPrices");
                        break;
                    }
                case "productbarcode":
                    {
                        table = new Table(executionContext, "ProductBarcode");
                        break;
                    }
                case "segment_categorization":
                    {
                        table = new Table(executionContext, "Segment_Categorization");
                        table.AddChild(GetTable(executionContext, "Segment_Categorization_Values"), "SegmentCategoryId");
                        break;
                    }
                case "segment_categorization_values":
                    {
                        table = new Table(executionContext, "Segment_Categorization_Values");
                        break;
                    }
                case "segment_definition":
                    {
                        table = new Table(executionContext, "Segment_Definition");
                        table.AddChild(GetTable(executionContext, "Segment_Definition_Source_Mapping"));
                        break;
                    }
                case "segment_definition_source_mapping":
                    {
                        table = new Table(executionContext, "Segment_Definition_Source_Mapping");
                        table.AddChild(GetTable(executionContext, "Segment_Definition_Source_Values"));
                        break;
                    }
                case "segment_definition_source_values":
                    {
                        table = new Table(executionContext, "Segment_Definition_Source_Values");
                        break;
                    }
                case "customdataset":
                    {
                        table = new Table(executionContext, "CustomDataSet");
                        table.AddChild(GetTable(executionContext, "CustomData"));
                        break;
                    }
                case "customdata":
                    {
                        table = new Table(executionContext, "customData");
                        break;
                    }
                case "customattributes":
                    {
                        table = new Table(executionContext, "CustomAttributes");
                        table.AddChild(GetTable(executionContext, "CustomAttributeValueList"));
                        break;
                    }
                case "customattributevaluelist":
                    {
                        table = new Table(executionContext, "CustomAttributeValueList");
                        break;
                    }
                case "ordertype":
                    {
                        table = new Table(executionContext, "OrderType");
                        table.AddChild(GetTable(executionContext, "OrderTypeGroupMap"));
                        break;
                    }
                case "ordertypegroupmap":
                    {
                        table = new Table(executionContext, "OrderTypeGroupMap");
                        break;
                    }
                case "ordertypegroup":
                    {
                        table = new Table(executionContext, "OrderTypeGroup");
                        break;
                    }
                case "product_type":
                    {
                        table = new Table(executionContext, "product_type");
                        break;
                    }
                case "tax":
                    {
                        table = new Table(executionContext, "tax");
                        table.AddChild(GetTable(executionContext, "TaxStructure"));
                        break;
                    }
                case "taxstructure":
                    {
                        table = new Table(executionContext, "TaxStructure");
                        break;
                    }
                case "checkinfacility":
                    {
                        table = new Table(executionContext, "CheckInFacility");
                        table.AddChild(GetTable(executionContext, "FacilityWaiver"));
                        table.AddChild(GetTable(executionContext, "FacilityTables"));
                        table.AddChild(GetTable(executionContext, "FacilitySeatLayout"));
                        table.AddChild(GetTable(executionContext, "FacilitySeats"));
                        break;
                    }
                case "facilitywaiver":
                    {
                        table = new Table(executionContext, "FacilityWaiver");
                        break;
                    }
                case "facilitytables":
                    {
                        table = new Table(executionContext, "FacilityTables");
                        break;
                    }
                case "facilityseatlayout":
                    {
                        table = new Table(executionContext, "FacilitySeatLayout");
                        break;
                    }
                case "facilityseats":
                    {
                        table = new Table(executionContext, "FacilitySeats");
                        break;
                    }
                case "systemoptions":
                    {
                        table = new SystemOptionsTable(executionContext);
                        break;
                    }
                case "managementforms":
                    {
                        table = new ManagementFormsTable(executionContext);
                        break;
                    }
                case "postypes":
                    {
                        table = new Table(executionContext,"POSTypes");
                        table.AddChild(new ManagementFormAccessChildTable(executionContext, "POSTypes", "Data Access", "POS Counter", "POSTypeName", "IsActive", "1"), "FunctionGUID");
                        break;
                    }
                case "cardtype":
                    {
                        table = new Table(executionContext, "CardType");
                        break;
                    }
                case "discounts":
                    {
                        table = new Table(executionContext, "Discounts");
                        table.AddChild(GetTable(executionContext, "DiscountCouponsHeader"));
                        table.AddChild(GetTable(executionContext, "DiscountedGames"));
                        table.AddChild(GetTable(executionContext, "DiscountedProducts"));
                        table.AddChild(GetTable(executionContext, "DiscountPurchaseCriteria"));
                        break;
                    }
                case "discountcouponsheader":
                    {
                        table = new Table(executionContext, "DiscountCouponsHeader");
                        //table.AddChild(GetTable(executionContext,"DiscountCoupons"));
                        break;
                    }
                case "discountcoupons":
                    {
                        table = new Table(executionContext, "DiscountCoupons");
                        break;
                    }
                case "country":
                    {
                        table = new Table(executionContext, "Country");
                        table.AddChild(GetTable(executionContext, "State"));
                        break;
                    }
                case "state":
                    {
                        table = new Table(executionContext, "State");
                        break;
                    }
                case "discountedgames":
                    {
                        table = new Table(executionContext, "DiscountedGames");
                        break;
                    }
                case "discountedproducts":
                    {
                        table = new Table(executionContext, "DiscountedProducts");
                        break;
                    }
                case "discountpurchasecriteria":
                    {
                        table = new Table(executionContext, "DiscountPurchaseCriteria");
                        break;
                    }
                case "schedule":
                    {
                        table = new Table(executionContext, "Schedule");
                        table.AddChild(GetTable(executionContext, "Schedule_ExclusionDays"));
                        break;
                    }
                case "schedule_exclusiondays":
                    {
                        table = new Table(executionContext, "Schedule_ExclusionDays");
                        break;
                    }
                case "trxprofiles":
                    {
                        table = new Table(executionContext, "TrxProfiles");
                        table.AddChild(GetTable(executionContext, "TrxProfileTaxRules"));
                        break;
                    }
                case "trxprofiletaxrules":
                    {
                        table = new Table(executionContext, "TrxProfileTaxRules");
                        break;
                    }
                case "pricelist":
                    {
                        table = new Table(executionContext, "PriceList");
                        table.AddChild(GetTable(executionContext, "PriceListProducts"));
                        break;
                    }
                case "pricelistproducts":
                    {
                        table = new Table(executionContext, "PriceListProducts");
                        break;
                    }
                case "category":
                    {
                        table = new Table(executionContext, "Category");
                        break;
                    }
                case "receiptprinttemplateheader":
                    {
                        table = new Table(executionContext, "ReceiptPrintTemplateHeader");
                        table.AddChild(GetTable(executionContext, "ReceiptPrintTemplate"));
                        table.AddChild(GetTable(executionContext, "TicketTemplateHeader"), "TemplateId");
                        break;
                    }
                case "receiptprinttemplate":
                    {
                        table = new Table(executionContext, "ReceiptPrintTemplate");
                        break;
                    }
                case "tickettemplateheader":
                    {
                        table = new Table(executionContext, "TicketTemplateHeader");
                        table.AddChild(GetTable(executionContext, "TicketTemplateElements"));
                        break;
                    }
                case "tickettemplateelements":
                    {
                        table = new Table(executionContext, "TicketTemplateElements");
                        break;
                    }
                case "attractionmasterschedule":
                    {
                        table = new Table(executionContext, "AttractionMasterSchedule");
                        table.AddChild(GetTable(executionContext, "AttractionSchedules"));
                        break;
                    }
                case "attractionschedules":
                    {
                        table = new Table(executionContext, "AttractionSchedules");
                        table.AddChild(GetTable(executionContext, "AttractionScheduleRules"));
                        break;
                    }
                case "attractionschedulerules":
                    {
                        table = new Table(executionContext, "AttractionScheduleRules");
                        break;
                    }
                case "emailtemplate":
                    {
                        table = new Table(executionContext, "EmailTemplate");
                        break;
                    }
                case "location":
                    {
                        table = new Table(executionContext, "Location");
                        break;
                    }
                case "locationtype":
                    {
                        table = new Table(executionContext, "LocationType");
                        break;
                    }
                case "uom":
                    {
                        table = new Table(executionContext, "UOM");
                        break;
                    }
                case "uomconversionfactor":
                    {
                        table = new Table(executionContext, "UOMConversionFactor");
                        break;
                    }
                case "vendor":
                    {
                        table = new Table(executionContext, "Vendor");
                        break;
                    }
                case "productsdisplaygroup":
                    {
                        table = new Table(executionContext, "ProductsDisplayGroup");
                        break;
                    }
                case "productdisplaygroupformat":
                    {
                        table = new Table(executionContext, "ProductDisplayGroupFormat");
                        break;
                    }
                case "languages":
                    {
                        table = new Table(executionContext, "Languages");
                        break;
                    }
                case "lookups":
                    {
                        table = new Table(executionContext, "Lookups");
                        table.AddChild(GetTable(executionContext, "LookupValues"));
                        break;
                    }
                case "lookupvalues":
                    {
                        table = new Table(executionContext, "LookupValues");
                        break;
                    }
                case "theme":
                    {
                        table = new Table(executionContext, "Theme");
                        table.AddChild(GetTable(executionContext, "ScreenTransitions"));
                        break;
                    }
                case "screentransitions":
                    {
                        table = new Table(executionContext, "ScreenTransitions");
                        break;
                    }
                case "screensetup":
                    {
                        table = new Table(executionContext, "ScreenSetup");
                        table.AddChild(GetTable(executionContext, "ScreenZoneDefSetup"));
                        break;
                    }
                case "screenzonedefsetup":
                    {
                        table = new Table(executionContext, "ScreenZoneDefSetup");
                        table.AddChild(GetTable(executionContext, "ScreenZoneContentMap"));
                        break;
                    }
                case "screenzonecontentmap":
                    {
                        table = new ScreenZoneContentMapTable(executionContext);
                        //table.AddChild(GetTable(executionContext,"Ticker"));
                        //table.AddChild(GetTable(executionContext,"DSLookup"));
                        //table.AddChild(GetTable(executionContext,"Media"));
                        //table.AddChild(GetTable(executionContext,"SignagePattern"));
                        break;
                    }
                case "ticker":
                    {
                        table = new Table(executionContext, "Ticker");
                        break;
                    }
                case "dslookup":
                    {
                        table = new Table(executionContext, "DSLookup");
                        table.AddChild(GetTable(executionContext, "DSignageLookupValues"));
                        break;
                    }
                case "dsignagelookupvalues":
                    {
                        table = new Table(executionContext, "DSignageLookupValues");
                        break;
                    }
                case "media":
                    {
                        table = new Table(executionContext, "Media");
                        break;
                    }
                case "signagepattern":
                    {
                        table = new Table(executionContext, "SignagePattern");
                        break;
                    }
                case "productmenu":
                    {
                        table = new Table(executionContext, "ProductMenu");
                        table.AddChild(GetTable(executionContext, "ProductMenuPanelMapping"));
                        table.AddChild(GetTable(executionContext, "ProductMenuPOSMachineMap"));
                        break;
                    }
                case "productmenupanelmapping":
                    {
                        table = new Table(executionContext, "ProductMenuPanelMapping");
                        break;
                    }
                case "productmenuposmachinemap":
                    {
                        table = new Table(executionContext, "ProductMenuPOSMachineMap");
                        break;
                    }
                case "productmenupanel":
                    {
                        table = new Table(executionContext, "ProductMenuPanel");
                        table.AddChild(GetTable(executionContext, "ProductMenuPanelContent"));
                        break;
                    }
                case "productmenupanelcontent":
                    {
                        table = new ProductMenuPanelContentTable(executionContext);
                        break;
                    }
                case "productmenupanelexclusion":
                    {
                        table = new Table(executionContext, "ProductMenuPanelExclusion");
                        break;
                    }
                case "userpayrate":
                    {
                        table = new Table(executionContext, "UserPayRate");
                        break;
                    }
                case "cashdrawers":
                    {
                        table = new Table(executionContext, "Cashdrawers");
                        break;
                    }
                case "modifierset":
                    {
                        table = new Table(executionContext, "ModifierSet");
                        table.AddChild(GetTable(executionContext, "ModifierSetDetails"));
                        break;
                    }
                case "modifiersetdetails":
                    {
                        table = new Table(executionContext, "ModifierSetDetails");
                        table.AddChild(GetTable(executionContext, "ParentModifiers"), "ModifierId");
                        break;
                    }
                case "parentmodifiers":
                    {
                        table = new Table(executionContext, "ParentModifiers");
                        break;
                    }
                case "machines":
                    {
                        table = new Table(executionContext, "machines");
                        break;
                    }
                case "messagingclient":
                    {
                        table = new Table(executionContext, "MessagingClient");
                        break;
                    }
                case "printers":
                    {
                        table = new Table(executionContext, "printers");
                        table.AddChild(GetTable(executionContext, "PrinterDisplayGroup"));
                        table.AddChild(GetTable(executionContext, "PrinterProducts"));
                        break;
                    }
                case "printerdisplaygroup":
                    {
                        table = new Table(executionContext, "PrinterDisplayGroup");
                        break;
                    }
                case "printerproducts":
                    {
                        table = new Table(executionContext, "PrinterProducts");
                        break;
                    }
                case "shiftconfigurations":
                    {
                        table = new Table(executionContext, "ShiftConfigurations");
                        break;
                    }
                case "payconfigurations":
                    {
                        table = new Table(executionContext, "PayConfigurations");
                        break;
                    }
                case "appscreenprofile":
                    {
                        table = new Table(executionContext, "AppScreenProfile");
                        break;
                    }
                case "attractionplays":
                    {
                        table = new Table(executionContext, "AttractionPlays");
                        break;
                    }
                case "posprinteroverrideoptions":
                    {
                        table = new Table(executionContext, "POSPrinterOverrideOptions");
                        break;
                    }
                case "displaypanel":
                    {
                        table = new Table(executionContext, "DisplayPanel");
                        break;
                    }
                case "currency":
                    {
                        table = new Table(executionContext, "Currency");
                        break;
                    }
                case "accountingcalendarmaster":
                    {
                        table = new Table(executionContext, "AccountingCalendarMaster");
                        break;
                    }
                case "redemptioncurrencyrule":
                    {
                        table = new Table(executionContext, "RedemptionCurrencyRule");
                        table.AddChild(GetTable(executionContext, "RedemptionCurrencyRuleDetail"));
                        break;
                    }
                case "redemptioncurrencyruledetail":
                    {
                        table = new Table(executionContext, "RedemptionCurrencyRuleDetail");
                        break;
                    }
                case "dataaccessrule":
                    {
                        table = new Table(executionContext, "DataAccessRule");
                        table.AddChild(GetTable(executionContext, "DataAccessDetail"));
                        break;
                    }
                case "dataaccessdetail":
                    {
                        table = new Table(executionContext, "DataAccessDetail");
                        table.AddChild(GetTable(executionContext, "EntityExclusionDetail"));
                        break;
                    }
                case "entityexclusiondetail":
                    {
                        table = new Table(executionContext, "EntityExclusionDetail");
                        break;
                    }
                case "holiday":
                    {
                        table = new Table(executionContext, "Holiday");
                        break;
                    }
                case "ticketstations":
                    {
                        table = new Table(executionContext, "TicketStations");
                        break;
                    }
                case "leavetemplate":
                    {
                        table = new Table(executionContext, "LeaveTemplate");
                        break;
                    }
                case "maint_asset_types":
                    {
                        table = new Table(executionContext, "Maint_Asset_Types");
                        break;
                    }
                case "redemptioncurrency":
                    {
                        table = new Table(executionContext,"RedemptionCurrency");
                        table.AddChild(new ManagementFormAccessChildTable(executionContext, "RedemptionCurrency", "Data Access", "Redemption Currency", "CurrencyName", "IsActive", "1"), "FunctionGUID");
                        break;
                    }
                case "specialpricing":
                    {
                        table = new Table(executionContext, "SpecialPricing");
                        break;
                    }
                case "waiverset":
                    {
                        table = new Table(executionContext, "WaiverSet");
                        table.AddChild(GetTable(executionContext, "WaiverSetDetails"));
                        table.AddChild(GetTable(executionContext, "WaiverSetSigningOptions"));
                        break;
                    }
                case "waiversetdetails":
                    {
                        table = new Table(executionContext, "WaiverSetDetails");
                        break;
                    }
                case "waiversetsigningoptions":
                    {
                        table = new Table(executionContext, "WaiverSetSigningOptions");
                        break;
                    }
                case "maint_assets":
                    {
                        table = new Table(executionContext, "Maint_Assets");
                        break;
                    }
                case "maint_assetgroups":
                    {
                        table = new Table(executionContext, "Maint_AssetGroups");
                        table.AddChild(GetTable(executionContext, "Maint_AssetGroup_Assets"));
                        break;
                    }
                case "maint_assetgroup_assets":
                    {
                        table = new Table(executionContext, "Maint_AssetGroup_Assets");
                        break;
                    }
                case "promotions":
                    {
                        table = new Table(executionContext, "Promotions");
                        table.AddChild(GetTable(executionContext, "PromotionRule"));
                        table.AddChild(GetTable(executionContext, "promotion_detail"));
                        table.AddChild(GetTable(executionContext, "PromotionExclusionDates"));
                        table.AddChild(GetTable(executionContext, "GameProfileAttributeValues"));
                        break;
                    }
                case "promotionrule":
                    {
                        table = new Table(executionContext, "PromotionRule");
                        break;
                    }
                case "promotion_detail":
                    {
                        table = new Table(executionContext, "promotion_detail");
                        table.AddChild(GetTable(executionContext, "GameProfileAttributeValues"));
                        break;
                    }
                case "promotionexclusiondates":
                    {
                        table = new Table(executionContext, "PromotionExclusionDates");
                        break;
                    }
                case "user_roles":
                    {
                        table = new Table(executionContext,"user_roles");
                        table.AddChild(GetTable(executionContext,"ManagementFormAccess"));
                        table.AddChild(GetTable(executionContext,"UserRoleDisplayGroups"));
                        table.AddChild(GetTable(executionContext,"UserRolePriceList"));
                        table.AddChild(GetTable(executionContext,"UserRoleDisplayGroupExclusions"));
                        table.AddChild(GetTable(executionContext,"AttendanceRoles"), "RoleId");
                        table.AddChild(GetTable(executionContext,"UserPayRate"), "UserRoleId");
                        table.AddChild(new ManagementFormAccessChildTable(executionContext, "user_roles", "Data Access", "User Roles", "Role", "IsActive", "1"), "FunctionGUID");
                        table.AddChild(new ManagementFormAccessUserRoleChildTable(executionContext), "FunctionGUID");
                        break;
                    }
                case "userrolepricelist":
                    {
                        table = new Table(executionContext, "UserRolePriceList");
                        break;
                    }
                case "userroledisplaygroups":
                    {
                        table = new Table(executionContext, "UserRoleDisplayGroups");
                        break;
                    }
                case "userroledisplaygroupexclusions":
                    {
                        table = new Table(executionContext, "UserRoleDisplayGroupExclusions");
                        break;
                    }
                case "attendanceroles":
                    {
                        table = new Table(executionContext, "AttendanceRoles");
                        break;
                    }
                case "managementformaccess":
                    {
                        table = new ManagementFormAccessTable(executionContext);
                        break;
                    }
                case "event":
                    {
                        table = new Table(executionContext, "Event");
                        break;
                    }
                case "achievementproject":
                    {
                        table = new Table(executionContext, "AchievementProject");
                        table.AddChild(GetTable(executionContext, "AchievementClass"));
                        break;
                    }
                case "achievementclass":
                    {
                        table = new Table(executionContext, "AchievementClass");
                        table.AddChild(GetTable(executionContext, "AchievementClassLevel"));
                        break;
                    }
                case "achievementclasslevel":
                    {
                        table = new Table(executionContext, "AchievementClassLevel");
                        break;
                    }
                case "messages":
                    {
                        table = new Table(executionContext, "Messages");
                        table.AddChild(GetTable(executionContext, "MessagesTranslated"));
                        break;
                    }
                case "messagestranslated":
                    {
                        table = new Table(executionContext, "MessagesTranslated");
                        break;
                    }
                case "maint_taskgroups":
                    {
                        table = new Table(executionContext, "Maint_TaskGroups");
                        table.AddChild(GetTable(executionContext, "Maint_Tasks"));
                        break;
                    }
                case "maint_tasks":
                    {
                        table = new Table(executionContext, "Maint_Tasks");
                        break;
                    }
                case "membershiprule":
                    {
                        table = new Table(executionContext, "MembershipRule");
                        break;
                    }
                case "membership":
                    {
                        table = new MembershipTable(executionContext);
                        break;
                    }
                case "membershiprewards":
                    {
                        table = new Table(executionContext, "MembershipRewards");
                        break;
                    }
                case "machinegroupmachines":
                    {
                        table = new Table(executionContext, "MachineGroupMachines");
                        break;
                    }
                case "maintenanceassettypes":
                    {
                        table = new Table(executionContext, "MaintenanceAssetTypes");
                        table.AddChild(GetTable(executionContext, "MaintenanceAssets"));
                        break;
                    }
                case "maintenanceassets":
                    {
                        table = new Table(executionContext, "MaintenanceAssets");
                        break;
                    }
                case "facilitymap":
                    {
                        table = new Table(executionContext, "FacilityMap");
                        table.AddChild(GetTable(executionContext, "FacilityMapDetails"));
                        table.AddChild(GetTable(executionContext, "ProductsAllowedInFacility"));
                        break;
                    }
                case "facilitymapdetails":
                    {
                        table = new Table(executionContext, "FacilityMapDetails");
                        break;
                    }
                case "productsallowedinfacility":
                    {
                        table = new Table(executionContext, "ProductsAllowedInFacility");
                        break;
                    }

                case "salesoffergroup":
                    {
                        table = new Table(executionContext, "SalesOfferGroup");
                        table.AddChild(GetTable(executionContext, "SaleGroupProductMap"));
                        break;
                    }
                case "salegroupproductmap":
                    {
                        table = new Table(executionContext, "SaleGroupProductMap");
                        break;
                    }
                case "lockerzones":
                    {
                        table = new Table(executionContext, "LockerZones");
                        table.AddChild(GetTable(executionContext, "LockerPanels"));
                        break;
                    }
                case "lockerpanels":
                    {
                        table = new Table(executionContext, "LockerPanels");
                        break;
                    }
                case "paymentmodes":
                    {
                        table = new Table(executionContext, "PaymentModes");
                        table.AddChild(GetTable(executionContext, "PaymentModeChannels"));
                        break;
                    }
                case "paymentmodechannels":
                    {
                        table = new Table(executionContext, "PaymentModeChannels");
                        break;
                    }
                case "game_profile":
                    {
                        table = new Table(executionContext,"Game_Profile");
                        table.AddChild(GetTable(executionContext,"GameProfileAttributeValues"));
                        table.AddChild(new ManagementFormAccessChildTable(executionContext, "Game_Profile", "Data Access", "Game Profile", "profile_name", "ISActive","1"), "FunctionGUID");
                        //table.AddChild(GetTable(executionContext,"Games"));
                        break;
                    }
                case "games":
                    {
                        table = new Table(executionContext, "Games");
                        table.AddChild(GetTable(executionContext, "GameProfileAttributeValues"));
                        table.AddChild(GetTable(executionContext, "AllowedMachineNames"));
                        break;
                    }
                case "allowedmachinenames":
                    {
                        table = new AllowedMachineNamesTable(executionContext);
                        break;
                    }
                case "masters":
                    {
                        table = new Table(executionContext, "Masters");
                        break;
                    }
                case "loyaltyrule":
                    {
                        table = new Table(executionContext, "LoyaltyRule");
                        table.AddChild(GetTable(executionContext, "LoyaltyRuleTriggers"));
                        break;
                    }
                case "loyaltyruletriggers":
                    {
                        table = new Table(executionContext, "LoyaltyRuleTriggers");
                        break;
                    }

                case "loyaltyredemptionrule":
                    {
                        table = new Table(executionContext, "LoyaltyRedemptionRule");
                        break;
                    }
                case "deliverychannels":
                    {
                        table = new Table(executionContext, "DeliveryChannels");
                        break;
                    }
                case "posmachines":
                    {
                        table = new Table(executionContext,"POSMachines");
                        table.AddChild(GetTable(executionContext,"POSPrinters"));
                        table.AddChild(GetTable(executionContext,"POSProductExclusions"));
                        table.AddChild(GetTable(executionContext,"POSCashdrawers"));
                        table.AddChild(GetTable(executionContext,"Peripherals"));
                        table.AddChild(GetTable(executionContext,"POSPaymentModeInclusions"));
                        table.AddChild(new ManagementFormAccessChildTable(executionContext, "POSMachines", "Data Access", "POS Machine", "POSName", "IsActive","1"), "FunctionGUID");
                        break;
                    }
                case "posprinters":
                    {
                        table = new Table(executionContext, "POSPrinters");
                        table.AddChild(GetTable(executionContext, "POSPrinterOverrideRules"));
                        table.AddChild(GetTable(executionContext, "POSPrintDisplayGroup"));
                        table.AddChild(GetTable(executionContext, "POSPrintProducts"));
                        break;
                    }
                case "posprinteroverriderules":
                    {
                        table = new Table(executionContext, "POSPrinterOverrideRules");
                        break;
                    }
                case "posprintdisplaygroup":
                    {
                        table = new Table(executionContext, "POSPrintDisplayGroup");
                        break;
                    }
                case "posprintproducts":
                    {
                        table = new Table(executionContext, "POSPrintProducts");
                        break;
                    }
                case "posproductexclusions":
                    {
                        table = new Table(executionContext, "POSProductExclusions");
                        break;
                    }
                case "poscashdrawers":
                    {
                        table = new Table(executionContext, "POSCashdrawers");
                        break;
                    }
                case "peripherals":
                    {
                        table = new Table(executionContext, "Peripherals");
                        break;
                    }
                case "pospaymentmodeinclusions":
                    {
                        table = new Table(executionContext, "POSPaymentModeInclusions");
                        break;
                    }
                case "organization":
                    {
                        table = new Table(executionContext, "Organization");
                        break;
                    }
                case "loyaltyattributes":
                    {
                        table = new Table(executionContext, "LoyaltyAttributes");
                        break;
                    }
                case "gameprofileattributevalues":
                    {
                        table = new Table(executionContext, "GameProfileAttributeValues");
                        break;
                    }
                case "gameprofileattributes":
                    {
                        table = new Table(executionContext, "GameProfileAttributes");
                        break;
                    }
                case "users":
                    {
                        table = new Table(executionContext, "Users");
                        table.AddChild(GetTable(executionContext, "UserPasswordHistory"));
                        table.AddChild(GetTable(executionContext, "UserIdentificationTags"));
                        table.AddChild(GetTable(executionContext, "UserToAttendanceRolesMap "));
                        table.AddChild(GetTable(executionContext, "UserPayRate"), "UserId");
                        break;
                    }
                case "userpasswordhistory":
                    {
                        table = new Table(executionContext, "UserPasswordHistory");
                        break;
                    }
                case "useridentificationtags":
                    {
                        table = new Table(executionContext, "UserIdentificationTags");
                        break;
                    }
                case "usertoattendancerolesmap":
                    {
                        table = new Table(executionContext, "UserToAttendanceRolesMap");
                        break;
                    }
                case "custfeedbacksurvey":
                    {
                        table = new Table(executionContext, "CustFeedbackSurvey");
                        table.AddChild(GetTable(executionContext, "CustFeedbackSurveyDetails"));
                        table.AddChild(GetTable(executionContext, "CustFeedbackSurveyPOSMapping"));
                        break;
                    }
                case "custfeedbacksurveydetails":
                    {
                        table = new Table(executionContext, "CustFeedbackSurveyDetails");
                        break;
                    }
                case "custfeedbackquestions":
                    {
                        table = new Table(executionContext, "CustFeedbackQuestions");
                        break;
                    }
                case "custfeedbackresponse":
                    {
                        table = new Table(executionContext, "CustFeedbackResponse");
                        table.AddChild(GetTable(executionContext, "CustFeedbackResponseValues"));
                        break;
                    }
                case "custfeedbackresponsevalues":
                    {
                        table = new Table(executionContext, "CustFeedbackResponseValues");
                        break;
                    }
                case "custfeedbacksurveyposmapping":
                    {
                        table = new Table(executionContext, "CustFeedbackSurveyPOSMapping");
                        break;
                    }
                case "parafait_defaults":
                    {
                        table = new Table(executionContext, "Parafait_Defaults");
                        break;
                    }
                case "approvalrule":
                    {
                        table = new Table(executionContext, "ApprovalRule");
                        break;
                    }
                case "inventorydocumenttype":
                    {
                        table = new Table(executionContext, "InventoryDocumentType", "Name");
                        break;
                    }
                case "maint_checklistdetails":
                    {
                        table = new Table(executionContext, "Maint_ChecklistDetails");
                        break;
                    }
                case "maint_schedule":
                    {
                        table = new Table(executionContext, "Maint_Schedule");
                        table.AddChild(GetTable(executionContext, "Maint_SchAssetTasks"));
                        break;
                    }
                case "maint_schassettasks":
                    {
                        table = new Table(executionContext, "Maint_SchAssetTasks");
                        break;
                    }
            }
            tableNameTableMap[tableName.ToLower()] = table;
            log.LogMethodExit(table);
            return table;
        }

        public static bool EntitySupportedByTableFactory(string tableName, ExecutionContext executionContext)
        {
            log.LogMethodEntry(tableName);
            Table supportedTable = GetSupportedTable(executionContext, tableName);
            bool result = supportedTable != null;
            log.LogMethodExit(result);
            return result;
        }
    }
}

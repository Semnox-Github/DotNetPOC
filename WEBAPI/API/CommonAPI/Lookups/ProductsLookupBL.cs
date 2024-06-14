/********************************************************************************************
 * Project Name - Products
 * Description  - Bussiness logic of lookups for product module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.60        17-Jan-2019   Jagan Mohana      Created 
 *            23-Jan-2019   Mushahid Faizan   Modified
 *            01-Apr-2019   Akshay Gulaganji    modified productsFilterParams.IsActive(string to bool convertion) 
              10-Apr-2019   Akshay Gulaganji    merged PRODUCTSSETUP and PRODUCTDETAILS lookups 
              11-Apr-2019   Akshay Gulaganji    added deserializeToDictionary() method and modified GetLookupFilteredList() method
 *2.70.0      17-Jun-2019   Nagesh Badiger      added Product maintains entity lookups.
 *2.80.0      02-Apr-2019   Girish Kundar       Modified: Added Recharge product to PriceList and auto generated check is removed for Price List entity
 *2.80.0      17-Jun-2020   Mushahid Faizan    Modified:  Checked all lookups null condition and WMS issues Fixes.
 *2.150.0     06-Jul-2022   Abhishek           Modified: Added CustomerProfilingGroup, PauseType Load on Product setup
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Vendor;
using System.Data;
using Newtonsoft.Json;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Category;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Game;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Product
{
    public class ProductsLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        private Dictionary<string, string> keyValuePairs;
        private string keyValuePair;
        string dependentDropdownName = string.Empty;
        string dependentDropdownSelectedId = string.Empty;
        string isActive = string.Empty;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private CommonLookupsDTO lookupDTO;
        /// <summary>
        /// Constructor for the method ProductLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public ProductsLookupBL(string entityName, ExecutionContext executioncontext, string dependentDropdownName, string dependentDropdownSelectedId, string isActive)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            this.dependentDropdownName = dependentDropdownName;
            this.dependentDropdownSelectedId = dependentDropdownSelectedId;
            this.isActive = isActive;
            this.lookupDTO = new CommonLookupsDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor for the method ProductLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        /// <param name="keyValuePairsString"></param>
        public ProductsLookupBL(string entityName, ExecutionContext executioncontext, string keyValuePair)
        {
            log.LogMethodEntry(entityName, executioncontext, this.keyValuePair);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            this.keyValuePair = keyValuePair;
            this.lookupDTO = new CommonLookupsDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the All look ups for all dropdowns based on the page in the Product module.
        /// </summary>       
        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> completeTablesDataList = new List<CommonLookupsDTO>();
                string dropdownNames = "";
                string[] dropdowns = null;

                switch (entityName.ToUpper().ToString())
                {
                    case "PRODUCTSSETUP":
                        dropdownNames = "MEMBERSHIP,LICENSETYPE,CATEGORY,ORDERTYPE,POSTYPE,TAX,MASTERSCHEDULE,COUNTER,CHECKINFACILITY,EMAILTEMPLATE,WAIVERSET,ZONES,DISPLAYGROUP,CUSTOMERPROFILEGROUP,PAUSETYPE";
                        break;
                    case "PRODUCTDISPLAYGROUP":
                        dropdownNames = "DISPLAYGROUP";
                        break;
                    case "COMBOPRODUCTDETAILS":
                        dropdownNames = "CHILDPRODUCT,CATEGORY";
                        break;
                    case "BOOKINGPRODUCTDETAILS":
                        dropdownNames = "CHILDPRODUCT,DISPLAYGROUP";
                        break;
                    case "PRODUCTTYPE":
                        dropdownNames = "ORDERTYPE";
                        break;
                    case "PRODUCTDETAILSPRODUCTMODIFIERS":
                        dropdownNames = "MODIFIERSET";
                        break;
                    case "PRODUCTGAMESENTITLEMENTS":
                        dropdownNames = "GAMEPROFILE,GAME,FREQUENCY,VALIDDAYSMINUTES,CARD_GAMES_ENTITLEMENT_TYPES,TIME_TYPE";
                        break;
                    case "SEGMENTDEFINITIONSOURCEMAP":
                        dropdownNames = "SEGMENT_DEFINATION,DATASOURCE_TYPE,DATASOURCE_ENTITY,DATASOURCE_COLUMN_VENDOR,DATASOURCE_COLUMN_CATEGORY";
                        break;
                    case "SETUPSEGMENTDEFINITION":
                        dropdownNames = "APPLICABLE_ENTITY";
                        break;
                    case "PRODUCTDISCOUNTS":
                        dropdownNames = "DISCOUNTS,VALIDDAYSMONTHS";
                        break;
                    case "PRODUCTSETUPCALENDAR":
                        dropdownNames = "TIME_TYPE,DAYLOOKUP";
                        break;
                    case "PRODUCTDETAILSUPSELLOFFERS":
                        dropdownNames = "SALEGROUPID"; // 
                        break;
                    case "SETUPOFFERGROUPPRODUCTMAP":
                        dropdownNames = "SALEGROUPID,PRODUCT";
                        break;
                    case "WAIVERSIGNINGOPTION":
                        dropdownNames = "WAIVER_SIGNING_OPTION";
                        break;
                    case "PRODUCTSENTITYEXCLUSION":
                        dropdownNames = "DAY";
                        break;
                    case "POSMANAGEMENTMACHINES":
                        dropdownNames = "COUNTER,INVENTORYLOCATION,PRINTER,PRINTTEMPLATE,PRINTER_TYPE,ORDERTYPEGROUP,FISCALIZATION_TYPE,OVERRIDE_OPTION_ITEM";
                        break;
                    case "POSMANAGEMENTPERIPHERALS":
                        dropdownNames = "DEVICETYPE,DEVICESUBTYPE";
                        break;
                    case "DISCOUNTSSETUP":
                        dropdownNames = "TRANSACTION_PROFILE,CATEGORY,PRODUCT,SCHEDULE,RECURFREQUENCY,RECURTYPE,TIME_TYPE,TIMELOOKUP,PRODUCT_GROUP,GAME";
                        break;
                    case "PRODUCTCATEGORY":
                        dropdownNames = "CATEGORY";
                        break;
                    case "PRODUCTCATEGORYACCOUNTINGCODE":
                        dropdownNames = "TYPE,TRANSACTION,TAXTYPE";
                        break;
                    case "TAXSETUP":
                        dropdownNames = "PARENTSTRUCTURE";
                        break;
                    case "PRICELISTS":
                        dropdownNames = "PRICELIST";
                        break;
                    case "PRODUCTSETMODIFIERS":
                        dropdownNames = "MODIFIERPRODUCT";
                        break;
                    case "PRODUCTCREDITPLUS":
                        dropdownNames = "CREDITPLUSTYPE,FREQUENCY,TIME_TYPE,COUNTER,CATEGORY,ORDERTYPE,GAMEPROFILE,GAME";
                        break;
                    case "FACILITYPOSASSIGNMENT":
                        dropdownNames = "POSMACHINES";
                        break;
                    case "PRODUCTSETUPSPECIALPRICING":
                        dropdownNames = "PRICINGOPTION";
                        break;
                    case "HTMLEDITOR":
                        dropdownNames = "LANGUAGE";
                        break;
                    case "PRODUCTDESCRIPTION":
                        dropdownNames = "LANGUAGE";
                        break;
                    case "MEMBERSHIPEXCLUSIONRULE":
                        dropdownNames = "MEMBERSHIPRULE";
                        break;
                    case "SCHEDULEEXCLUSION":
                        dropdownNames = "DAY";
                        break;
                    case "PARAFAITOPTIONS":
                        dropdownNames = "OPTIONNAME";
                        break;
                    case "FACILITYSEATLAYOUT":
                        dropdownNames = "CHECKINFACILITY";
                        break;
                    case "WAIVERS":
                        dropdownNames = "LANGUAGE";
                        break;
                    case "POSMANAGEMENTPRODUCTS":
                        dropdownNames = "DISPLAYGROUP";
                        break;
                    case "ATTRACTIONSCHEDULE":
                        dropdownNames = "CHECKINFACILITY,ATTRACTIONPLAY,DAYLOOKUP,HOURS,MINUTES";
                        break;
                    case "INVENTORYINPRODUCT":
                        dropdownNames = "CATEGORY,UOM,INBOUNDLOCATION,OUTBOUNDLOCATION,TAXTYPE,EXPIRYTYPE,ISSUEAPPROACH,VENDOR";
                        break;
                    case "PRODUCTLOCATION":
                        dropdownNames = "LOCATIONTYPEID";
                        break;
                    case "PRODUCTLOCATIONIMPORTMACHINES":
                        dropdownNames = "LOCATIONTYPE";
                        break;
                    case "PRODUCTVENDOR":
                        dropdownNames = "COUNTRY,TAXTYPE";
                        break;
                    //case "FACILITY":
                    //    dropdownNames = "MASTERSCHEDULE,CANCELLATIONPRODUCT";
                    //    break;
                    case "ALLOWEDPRODUCTS":
                        dropdownNames = "ALLOWEDPRODUCT";
                        break;
                    case "FACILITYMAP":
                        dropdownNames = "FACILITYMAPMASTERSCHEDULE,CANCELLATIONPRODUCT,FACILITY";
                        break;
                    case "MAPPRODUCTSTOFACILITYMAP":
                        dropdownNames = "FACILITYMAP";
                        break;
                    case "UPLOADINVENTORYPRODUCTS":
                        dropdownNames = "VENDOR,INBOUNDLOCATION,OUTBOUNDLOCATION,TAXTYPE,CATEGORYACITVE,UOMACTIVE";
                        break;
                    case "FACILITYWAIVER":
                        dropdownNames = "WAIVERSET";
                        break;
                    case "CUSTOMERPROFILE":
                        dropdownNames = "PROFILEOPERATOR,PROFILETYPE";
                        break;
                    case "PRODUCTLOOKUP":
                        dropdownNames = "PRODUCT";
                        break;
                }

                dropdowns = dropdownNames.Split(',');
                ProductsDataHandler productsDataHandler = new ProductsDataHandler();
                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO
                    {
                        Items = new List<CommonLookupDTO>(),
                        DropdownName = dropdownName
                    };

                    if (dropdownName.ToUpper().ToString() == "TIME_TYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        List<KeyValuePair<decimal, string>> timeList = new List<KeyValuePair<decimal, string>>();
                        TimeSpan ts;
                        for (int i = 0; i <= 95; i++)
                        {
                            ts = new TimeSpan(0, i * 15, 0);
                            timeList.Add(new KeyValuePair<decimal, string>(Convert.ToDecimal(ts.Hours + ts.Minutes * 0.01), string.Format("{0:0}:{1:00} {2}", (ts.Hours % 12) == 0 ? (ts.Hours == 12 ? 12 : 0) : ts.Hours % 12, ts.Minutes, ts.Hours >= 12 ? "PM" : "AM")));
                        }
                        if (timeList.Count != 0)
                        {
                            foreach (var timeValue in timeList)
                            {
                                CommonLookupDTO digatalSignageDataObject;
                                digatalSignageDataObject = new CommonLookupDTO(Convert.ToString(timeValue.Key), timeValue.Value);
                                lookupDTO.Items.Add(digatalSignageDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TIMELOOKUP")
                    {
                        CommonLookupDTO lookupDataObject;
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        string time;
                        int hour;
                        int mins;
                        string ampm;
                        for (int i = 0; i < 48; i++)
                        {
                            hour = i / 2;
                            mins = (i % 2) * 30;

                            if (hour >= 12)
                                ampm = "PM";
                            else
                                ampm = "AM";

                            if (hour == 0)
                                hour = 12;
                            if (hour > 12)
                                hour = hour - 12;

                            time = hour.ToString() + ":" + mins.ToString().PadLeft(2, '0') + " " + ampm;
                            lookupDataObject = new CommonLookupDTO(time, time);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "HOURS")
                    {
                        loadDefaultValue("<SELECT>");
                        for (int i = 0; i < 24; i++)
                        {
                            CommonLookupDTO digatalSignageDataObject;
                            digatalSignageDataObject = new CommonLookupDTO(Convert.ToString(i), i.ToString());
                            lookupDTO.Items.Add(digatalSignageDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MINUTES")
                    {
                        loadDefaultValue("<SELECT>");

                        string attractionScheduleMinuteDefaultValue = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TIME_SPAN_FOR_ATTRACTION_SCHEDULE");

                        if (!string.IsNullOrEmpty(attractionScheduleMinuteDefaultValue))
                        {
                            int defaultValue = Convert.ToInt32(attractionScheduleMinuteDefaultValue);
                            int minutes = 60;
                            int scheduleValue = minutes / defaultValue;
                            int reminderValue = minutes % defaultValue;
                            int value = 0;
                            for (int i = 0; i < scheduleValue; i++)
                            {
                                value = i * defaultValue;
                                CommonLookupDTO digatalSignageDataObject;
                                digatalSignageDataObject = new CommonLookupDTO(Convert.ToString(value).PadLeft(2, '0'), ":" + value.ToString().PadLeft(2, '0'));
                                lookupDTO.Items.Add(digatalSignageDataObject);
                            }
                            /// The below condtion is check for if any reminder value is zero then last time value added to lookupup.
                            value = scheduleValue * defaultValue;
                            if (reminderValue != 0 && value < minutes)
                            {
                                CommonLookupDTO digatalSignageDataObject;
                                digatalSignageDataObject = new CommonLookupDTO(Convert.ToString(value).PadLeft(2, '0'), ":" + value.ToString().PadLeft(2, '0'));
                                lookupDTO.Items.Add(digatalSignageDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ZONES")
                    {
                        loadDefaultValue("<SELECT>");
                        LockerZonesList lockerZonesList = new LockerZonesList(executionContext);
                        List<LockerZonesDTO> lockerZonesDTOList = lockerZonesList.GetZones(false, false);
                        if (lockerZonesDTOList != null && lockerZonesDTOList.Any())
                        {
                            foreach (LockerZonesDTO lockerZonesDTO in lockerZonesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lockerZonesDTO.ZoneId), lockerZonesDTO.ZoneName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FACILITYMAP")
                    {
                        loadDefaultValue("<SELECT>");
                        FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                        List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParam, false, false);
                        if (facilityMapDTOList != null && facilityMapDTOList.Any())
                        {
                            foreach (FacilityMapDTO facilityMapDTO in facilityMapDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(facilityMapDTO.FacilityMapId), facilityMapDTO.FacilityMapName + (facilityMapDTO.IsActive == false ? "(Inactive)" : string.Empty));
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RECURFREQUENCY")
                    {
                        List<KeyValuePair<string, string>> recurFrequecyList = new List<KeyValuePair<string, string>>();
                        recurFrequecyList.Add(new KeyValuePair<string, string>("D", "Daily"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("W", "Weekly"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("M", "Monthly"));
                        foreach (var recurFrequecy in recurFrequecyList)
                        {
                            CommonLookupDTO lookupDataObject;
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(recurFrequecy.Key), recurFrequecy.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RECURTYPE")
                    {
                        List<KeyValuePair<string, string>> recurFrequecyList = new List<KeyValuePair<string, string>>();
                        recurFrequecyList.Add(new KeyValuePair<string, string>("D", "Day"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("W", "Weekday"));
                        foreach (var recurFrequecy in recurFrequecyList)
                        {
                            CommonLookupDTO lookupDataObject;
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(recurFrequecy.Key), recurFrequecy.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CANCELLATIONPRODUCT")
                    {
                        loadDefaultValue("<SELECT>");
                        ProductsList productsList = new ProductsList(executionContext);
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, ProductTypeValues.MANUAL));
                        List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            productsDTOList = productsDTOList.Where(prod => prod.AllowPriceOverride == "Y" && prod.QuantityPrompt == "N").ToList();
                        }
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            foreach (ProductsDTO productsDTO in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FACILITYMAPMASTERSCHEDULE")
                    {
                        loadDefaultValue("<SELECT>");
                        MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext);
                        List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.ACTIVE_FLAG, "Y"));

                        List<MasterScheduleDTO> masterScheduleDTOList = masterScheduleList.GetMasterScheduleDTOsList(searchParameters, false, false);
                        if (masterScheduleDTOList != null && masterScheduleDTOList.Any())
                        {
                            foreach (MasterScheduleDTO masterScheduleDTO in masterScheduleDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(masterScheduleDTO.MasterScheduleId), masterScheduleDTO.MasterScheduleName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FACILITY")
                    {
                        loadDefaultValue("<SELECT>");
                        FacilityList facilityListBL = new FacilityList(executionContext);
                        List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                        List<FacilityDTO> facilityDTOList = facilityListBL.GetFacilityDTOList(searchParameters);
                        if (facilityDTOList != null && facilityDTOList.Any())
                        {
                            facilityDTOList = facilityDTOList.OrderBy(x => x.FacilityName).ToList();
                            foreach (FacilityDTO facilityDTO in facilityDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(facilityDTO.FacilityId), facilityDTO.FacilityName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ALLOWEDPRODUCT")
                    {
                        loadDefaultValue("<SELECT>");
                        ProductsList productsList = new ProductsList(executionContext);
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>
                        {
                            new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST, " 'BOOKINGS','RENTAL','ATTRACTION' "),
                            new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()),
                            new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "1")
                        };
                        List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            foreach (ProductsDTO productsDTO in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    { "ProductTypeId", Convert.ToString(productsDTO.ProductTypeId) },
                                    { "ProductType", Convert.ToString(productsDTO.ProductType) }
                                };
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName, values);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "EXPIRYTYPE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                       {
                           { "N","None"  },
                           { "D","In Days" },
                           { "E","Expiry Date" }
                       };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "COUNTRY")
                    {
                        loadDefaultValue("<SELECT>");
                        CountryDTOList countryDTOListClass = new CountryDTOList(executionContext);
                        List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchParameters;
                        searchParameters = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<CountryDTO> countryDTOList = countryDTOListClass.GetCountryDTOList(searchParameters);
                        if (countryDTOList != null && countryDTOList.Any())
                        {
                            foreach (CountryDTO countryDTO in countryDTOList)
                            {
                                StateDTOList stateDTOListObj = new StateDTOList(executionContext);
                                List<KeyValuePair<StateDTO.SearchByParameters, string>> searchStateParameters = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
                                searchStateParameters.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                searchStateParameters.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.COUNTRY_ID, countryDTO.CountryId.ToString()));
                                StateDTO stateDTOForSearch = new StateDTO();
                                List<StateDTO> stateDTOList = stateDTOListObj.GetStateDTOList(searchStateParameters);
                                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                                if (stateDTOList != null && stateDTOList.Any())
                                {
                                    foreach (StateDTO stateDTO in stateDTOList)
                                    {
                                        keyValuePairs.Add(Convert.ToString(stateDTO.StateId), stateDTO.State);
                                    }
                                }
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(countryDTO.CountryId), countryDTO.CountryName, keyValuePairs);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ISSUEAPPROACH")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "None", "None" },
                            { "FEFO","FEFO"},
                            { "FIFO","FIFO"},
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "UOM" || dropdownName.ToUpper().ToString() == "UOMACTIVE")
                    {
                        loadDefaultValue("<SELECT>");
                        UOMList uOMList = new UOMList(executionContext);
                        List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomDTOSearchParams;
                        uomDTOSearchParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
                        uomDTOSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, executionContext.GetSiteId().ToString()));
                        if (dropdownName.ToUpper().ToString() == "UOMACTIVE")
                        {
                            uomDTOSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.IS_ACTIVE, "1"));
                        }
                        List<UOMDTO> uomDTOList = uOMList.GetAllUOMs(uomDTOSearchParams);
                        if (uomDTOList != null && uomDTOList.Any())
                        {
                            foreach (UOMDTO uOMDTO in uomDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(uOMDTO.UOMId), uOMDTO.UOM);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DAYLOOKUP")
                    {
                        List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
                        selectKey.Add(new KeyValuePair<string, string>("", "<SELECT>"));
                        foreach (var select in selectKey)
                        {
                            CommonLookupDTO lookupDataObject;
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                        DayLookupList dayLookupList = new DayLookupList();
                        List<DayLookupDTO> dayLookups = dayLookupList.GetAllDayLookup();
                        if (dayLookups != null && dayLookups.Any())
                        {
                            foreach (var dayLookupDTO in dayLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(dayLookupDTO.Day), dayLookupDTO.Display);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CATEGORY" || dropdownName.ToUpper().ToString() == "CATEGORYACITVE")
                    {
                        if (entityName.ToUpper().ToString() == "DISCOUNTSSETUP")
                        {
                            loadDefaultValue("-ALL-");
                        }
                        else
                        {
                            loadDefaultValue("<SELECT>");
                        }
                        CategoryList categoryList = new CategoryList(executionContext);
                        List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        if (dropdownName.ToUpper().ToString() == "CATEGORYACITVE")
                        {
                            searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "1"));
                        }
                        List<CategoryDTO> categoryDTOList = categoryList.GetAllCategory(searchParameters);

                        if (categoryDTOList != null && categoryDTOList.Any())
                        {
                            categoryDTOList = categoryDTOList.OrderBy(x => x.Name).ToList();
                            foreach (CategoryDTO category in categoryDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(category.CategoryId), category.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LOCATIONTYPE" || dropdownName.ToUpper().ToString() == "LOCATIONTYPEID")
                    {
                        loadDefaultValue("<SELECT>");

                        LocationTypeList locationTypeList = new LocationTypeList(executionContext);
                        List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> inventoryLocationTypeSearchParams = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
                        inventoryLocationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                        if (dropdownName.ToUpper().ToString() == "LOCATIONTYPE")
                        {
                            inventoryLocationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE, "Department,Store"));
                        }
                        List<LocationTypeDTO> inventoryLocationsListOnDisplay = locationTypeList.GetAllLocationType(inventoryLocationTypeSearchParams);

                        if (inventoryLocationsListOnDisplay != null && inventoryLocationsListOnDisplay.Any())
                        {
                            foreach (LocationTypeDTO locationTypeDTO in inventoryLocationsListOnDisplay)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(locationTypeDTO.LocationTypeId), locationTypeDTO.LocationType);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "INBOUNDLOCATION" || dropdownName.ToUpper().ToString() == "OUTBOUNDLOCATION")
                    {
                        loadDefaultValue("<SELECT>");
                        LocationList locationList = new LocationList(executionContext);
                        List<LocationDTO> locationDTOList = new List<LocationDTO>();
                        if (dropdownName.ToUpper().ToString() == "INBOUNDLOCATION")
                        {
                            locationDTOList = locationList.GetAllLocations("Store");
                        }
                        else
                        {
                            locationDTOList = locationList.GetAllLocations("Store,Department");
                        }

                        if (locationDTOList != null && locationDTOList.Any())
                        {
                            List<LocationDTO> filteredLocationDTOList = new List<LocationDTO>(locationDTOList.OrderBy(location => location.Name));

                            foreach (LocationDTO locationDTO in filteredLocationDTOList)
                            {
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                   { "IsStore", Convert.ToString(locationDTO.IsStore) }
                                };
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(locationDTO.LocationId), locationDTO.Name.ToString(), values);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ORDERTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        OrderTypeListBL orderTypeListBL = new OrderTypeListBL(executionContext);
                        List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<OrderTypeDTO> orderTypes = orderTypeListBL.GetOrderTypeDTOList(searchParameters, false, false);
                        if (orderTypes != null && orderTypes.Any())
                        {
                            orderTypes = orderTypes.OrderBy(x => x.Name).ToList();
                            foreach (var order in orderTypes)
                            {
                                string orderTypeName;
                                orderTypeName = order.Name;
                                if (order.IsActive == false)
                                {
                                    orderTypeName = order.Name + "(Inactive)";
                                }
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(order.Id), orderTypeName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TAXTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        TaxList taxList = new TaxList(executionContext);
                        List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        if (entityName.ToUpper().ToString() == "UPLOADINVENTORYPRODUCTS")
                        {
                            searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
                        }
                        var taxes = taxList.GetAllTaxes(searchParameters, false, false);
                        if (taxes != null && taxes.Any())
                        {
                            taxes = taxes.OrderBy(x => x.TaxName).ToList();
                            foreach (var tax in taxes)
                            {
                                CommonLookupDTO lookupDataObject;
                                if (entityName.ToUpper().ToString() == "PRODUCTCATEGORYACCOUNTINGCODE")
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(tax.TaxId), (tax.ActiveFlag) ? tax.TaxName : tax.TaxName + " (Inactive)");
                                }
                                else
                                {
                                    if (tax.ActiveFlag)
                                    {
                                        lookupDataObject = new CommonLookupDTO(Convert.ToString(tax.TaxId), tax.TaxName);
                                    }
                                    else
                                        continue;
                                  
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DISPLAYGROUP")
                    {
                        ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
                        List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        if (entityName.ToUpper().ToString() == "PRODUCTDISPLAYGROUP" || entityName.ToUpper().ToString() == "PRODUCTSSETUP")
                        {
                            loadDefaultValue("None");
                        }
                        else
                        {
                            loadDefaultValue("<SELECT>");
                            searchParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE, isActive));
                        }
                        List<ProductDisplayGroupFormatDTO> displayGroupDTOs = productDisplayGroupList.GetAllProductDisplayGroup(searchParameters);
                        if (displayGroupDTOs != null && displayGroupDTOs.Any())
                        {
                            displayGroupDTOs = displayGroupDTOs.OrderBy(x => x.DisplayGroup).ToList();
                            foreach (ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO in displayGroupDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productDisplayGroupFormatDTO.Id), productDisplayGroupFormatDTO.DisplayGroup);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MODIFIERSET")
                    {
                        loadDefaultValue("<SELECT>");
                        ModifierSetDTOList modifierSetBL = new ModifierSetDTOList(executionContext);
                        List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                        List<ModifierSetDTO> modifierSetDTOs = modifierSetBL.GetAllModifierSetDTOList(searchParameters, false, false);
                        if (modifierSetDTOs != null && modifierSetDTOs.Any())
                        {
                            modifierSetDTOs = modifierSetDTOs.OrderBy(x => x.SetName).ToList();
                            foreach (var modifier in modifierSetDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(modifier.ModifierSetId), modifier.SetName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "GAME")
                    {
                        loadDefaultValue("-All-");
                        List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        GameList gamesList = new GameList(executionContext);
                        List<GameDTO> games = gamesList.GetGameList(searchParameters, false);
                        if (games != null && games.Any())
                        {
                            games = games.OrderBy(x => x.GameName).ToList();
                            foreach (var gameDTO in games)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(gameDTO.GameId), gameDTO.IsActive == true ? gameDTO.GameName.ToString() : gameDTO.GameName + "(Inactive)");
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CARD_GAMES_ENTITLEMENT_TYPES" || dropdownName.ToUpper().ToString() == "WAIVER_SIGNING_OPTION")
                    {
                        if (dropdownName.ToUpper().ToString() == "CARD_GAMES_ENTITLEMENT_TYPES")
                        {
                            loadDefaultValue("Default");
                        }

                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, dropdownName));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (var lookupValueDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                if (dropdownName.ToUpper().ToString() == "WAIVER_SIGNING_OPTION")
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValueDTO.LookupValueId), lookupValueDTO.LookupValue);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValueDTO.LookupValue), lookupValueDTO.Description);
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "SEGMENT_DEFINATION")
                    {
                        loadDefaultValue("<SELECT>");
                        //Hardcoded value need to be remove   
                        string applicability = "POS PRODUCTS";
                        SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);
                        List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionDTOSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                        segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, applicability));
                        segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "1"));
                        segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        var segmentDefinitions = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionDTOSearchParams);
                        if (segmentDefinitions != null && segmentDefinitions.Any())
                        {
                            segmentDefinitions = segmentDefinitions.OrderBy(x => x.SegmentName).ToList();
                            foreach (var segment in segmentDefinitions)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(segment.SegmentDefinitionId), segment.SegmentName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATASOURCE_TYPE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "DATE", "DATE" },
                            { "DYNAMIC LIST","DYNAMIC LIST"},
                            { "TEXT","TEXT"},
                            { "STATIC LIST","STATIC LIST"}
                        };
                        loadDefaultValue("<SELECT>");
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATASOURCE_ENTITY")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "VENDOR", "VENDOR" },
                            { "CATEGORY","CATEGORY"}
                        };
                        loadDefaultValue("<SELECT>");
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var dataentity in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(dataentity.Key, dataentity.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "APPLICABLE_ENTITY")
                    {
                        Dictionary<string, string> applicableEntityLookups = new Dictionary<string, string>
                        {
                            { "PRODUCT","PRODUCT" },
                            { "POS PRODUCTS","POS PRODUCTS" }
                        };
                        loadDefaultValue("<SELECT>");
                        if (applicableEntityLookups.Count != 0)
                        {
                            foreach (var item in applicableEntityLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(item.Key, item.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DISCOUNTS")
                    {
                        loadDefaultValue("<SELECT>");
                        List<DiscountsDTO> discountsList = null;
                        using(UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                            searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                            DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, unitOfWork);
                            discountsList = discountsListBL.GetDiscountsDTOList(searchParameters, false, false);
                        }
                        
                        if (discountsList != null && discountsList.Any())
                        {
                            discountsList = discountsList.OrderBy(x => x.DiscountName).ToList();
                            discountsList = discountsList.OrderByDescending(x => x.IsActive).ToList();
                            foreach (var discountDTO in discountsList)
                            {
                                string discountName;
                                discountName = discountDTO.DiscountName;
                                if (discountDTO.IsActive == false)
                                {
                                    discountName = discountDTO.DiscountName + "(Inactive)";
                                }
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(discountDTO.DiscountId), discountName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "INVENTORYLOCATION")
                    {
                        loadDefaultValue("<SELECT>");
                        List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParam = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                        searchParam.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        LocationList locationList = new LocationList(executionContext);
                        List<LocationDTO> locationDTOList = locationList.GetAllLocations(searchParam);
                        if (locationDTOList != null && locationDTOList.Any())
                        {
                            locationDTOList = locationDTOList.OrderBy(x => x.Name).ToList();
                            foreach (var locationDTO in locationDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(locationDTO.LocationId), locationDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "COUNTER")
                    {
                        loadDefaultValue("<SELECT>");
                        POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
                        List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<POSTypeDTO> posTypeList = pOSTypeListBL.GetPOSTypeDTOList(searchParam);
                        if (posTypeList != null && posTypeList.Any())
                        {
                            posTypeList = posTypeList.OrderBy(x => x.POSTypeName).ToList();
                            foreach (var posType in posTypeList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(posType.POSTypeId), posType.POSTypeName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRINTER")
                    {
                        loadDefaultValue("<SELECT>");
                        PrinterListBL printerListBL = new PrinterListBL(executionContext);
                        List<KeyValuePair<PrinterDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<PrinterDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<PrinterDTO.SearchByParameters, string>(PrinterDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<PrinterDTO> printerDTOs = printerListBL.GetPrinterDTOList(searchParam);
                        if (printerDTOs != null && printerDTOs.Any())
                        {
                            foreach (var printer in printerDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(printer.PrinterId), printer.PrinterName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ORDERTYPEGROUP")
                    {
                        loadDefaultValue("<SELECT>");
                        OrderTypeGroupListBL orderTypeGroupList = new OrderTypeGroupListBL(executionContext);
                        List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<OrderTypeGroupDTO> orderTypeDTOs = orderTypeGroupList.GetOrderTypeGroupDTOList(searchParameters);
                        if (orderTypeDTOs != null && orderTypeDTOs.Any())
                        {
                            foreach (var orderTypeGroup in orderTypeDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(orderTypeGroup.Id), orderTypeGroup.IsActive == true ? orderTypeGroup.Name.ToString() : orderTypeGroup.Name + "(Inactive)");
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FISCALIZATION_TYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        Dictionary<string, string> fiscalizationTypeLookups = new Dictionary<string, string>
                            {
                                { Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.CHILE), Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.CHILE) },
                                { Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.ECUADOR), Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.ECUADOR) },
                                { Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.PERU), Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.PERU)},
                                { Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.VIETNAM), Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.VIETNAM)}
                            };
                        if (fiscalizationTypeLookups.Count != 0)
                        {
                            foreach (var fiscalizationType in fiscalizationTypeLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(fiscalizationType.Key, fiscalizationType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "OVERRIDE_OPTION_ITEM")
                    {
                        loadDefaultValue("<SELECT>");
                        Dictionary<string, string> overrideOptionItemLookups = new Dictionary<string, string>
                            {
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.NONE), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.NONE) },
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.RECEIPT), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.RECEIPT) },
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.SEQUENCE), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.SEQUENCE)},
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE)},
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.SHOWERROR), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.SHOWERROR)}
                            };
                        if (overrideOptionItemLookups.Count != 0)
                        {
                            foreach (var overrideOptionItem in overrideOptionItemLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(overrideOptionItem.Key, overrideOptionItem.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRINTTEMPLATE")
                    {
                        loadDefaultValue("<SELECT>");
                        ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderListBL = new ReceiptPrintTemplateHeaderListBL(executionContext);
                        List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>(ReceiptPrintTemplateHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOs = receiptPrintTemplateHeaderListBL.GetReceiptPrintTemplateHeaderDTOList(searchParam, false);
                        if (receiptPrintTemplateHeaderDTOs != null && receiptPrintTemplateHeaderDTOs.Any())
                        {
                            foreach (var printerTemplate in receiptPrintTemplateHeaderDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(printerTemplate.TemplateId), printerTemplate.IsActive == true ? printerTemplate.TemplateName : printerTemplate.TemplateName + "(Inactive)");
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRINTER_TYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<LookupValuesDTO> lookupValuesDTOs = lookupValuesList.GetInventoryLookupValuesByValueName(dropdownName, executionContext.GetSiteId());
                        if (lookupValuesDTOs != null && lookupValuesDTOs.Any())
                        {
                            foreach (var lookup in lookupValuesDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookup.LookupId), lookup.LookupValue.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DEVICETYPE")
                    {
                        Dictionary<string, string> deviceTypeLookups = new Dictionary<string, string>
                        {
                            { "-1","<SELECT>"},
                            { "CardReader", "CardReader" },
                            { "BarcodeReader", "BarcodeReader" },
                            { "Waiver", "Waiver" }
                        };
                        if (deviceTypeLookups.Count != 0)
                        {
                            foreach (var deviceType in deviceTypeLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(deviceType.Key, deviceType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DEVICESUBTYPE")
                    {
                        Dictionary<string, string> deviceSubTypeLookups = new Dictionary<string, string>
                        {
                            { "-1","<SELECT>"},
                            { "KeyboardWedge", "KeyboardWedge" },
                            { "ACR1252U", "ACR1252U" },
                            { "ACR122U", "ACR122U" },
                            { "ACR1222L", "ACR1222L" },
                            { "Wacom", "Wacom" }
                        };
                        if (deviceSubTypeLookups.Count != 0)
                        {
                            foreach (var deviceSubType in deviceSubTypeLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(deviceSubType.Key, deviceSubType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TRANSACTION_PROFILE")
                    {
                        loadDefaultValue("<SELECT>");
                        TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL(executionContext);
                        List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<TransactionProfileDTO> transactionProfileDTOList = transactionProfileListBL.GetTransactionProfileDTOList(searchParameters, false, false);

                        if (transactionProfileDTOList != null && transactionProfileDTOList.Any())
                        {
                            transactionProfileDTOList = transactionProfileDTOList.OrderBy(x => x.ProfileName).ToList();
                            foreach (var item in transactionProfileDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(item.TransactionProfileId), item.ProfileName.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TYPE")
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>
                        {
                            { "-1" , "<SELECT>" },
                            { "Revenue", "Revenue" },
                            { "Cost", "Cost" },
                            { "Inventory Receipt", "Inventory Receipt" },
                            { "Inventory Adjustments", "Inventory Adjustments" },
                            { "Sales Invoice", "Sales Invoice" }
                        };
                        if (values.Count != 0)
                        {
                            foreach (var type in values)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(type.Key, type.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TRANSACTION")
                    {
                        Dictionary<string, string> transactionValues = new Dictionary<string, string>
                        {
                            { "-1" , "<SELECT>"},
                            { "Debit", "Debit" },
                            { "Debit /Cash", "Debit /Cash" },
                            { "Credit /Revenue", "Credit /Revenue" },
                            { "Credit /VAT", "Credit /VAT" }
                        };
                        if (transactionValues.Count != 0)
                        {
                            foreach (var transaction in transactionValues)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(transaction.Key, transaction.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "SALEGROUPID")
                    {
                        loadDefaultValue("<SELECT>");
                        SalesOfferGroupList salesOfferGroupList = new SalesOfferGroupList(executionContext);
                        List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> salesOfferGroupSearchParams = new List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>>();
                        salesOfferGroupSearchParams.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        salesOfferGroupSearchParams.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_ACTIVE, "1"));
                        List<SalesOfferGroupDTO> salesOfferGroupListOnDisplay = salesOfferGroupList.GetAllSalesOfferGroups(salesOfferGroupSearchParams);

                        if (salesOfferGroupListOnDisplay != null && salesOfferGroupListOnDisplay.Any())
                        {
                            SortableBindingList<SalesOfferGroupDTO> salesOfferGroupDTOSortList = new SortableBindingList<SalesOfferGroupDTO>(salesOfferGroupListOnDisplay);
                            salesOfferGroupListOnDisplay = salesOfferGroupListOnDisplay.OrderBy(x => x.Name).ToList();
                            foreach (var item in salesOfferGroupListOnDisplay)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(item.SaleGroupId), item.Name.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRODUCT")
                    {
                        Products products = new Products();
                        ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                        productsFilterParams.SiteId = executionContext.GetSiteId();
                        if (entityName.ToUpper().ToString() == "SETUPOFFERGROUPPRODUCTMAP")
                        {
                            loadDefaultValue("<SELECT>");
                        }
                        else if (entityName.ToUpper().ToString() == "DISCOUNTSSETUP")
                        {
                            productsFilterParams.IsActive = true;
                            loadDefaultValue("-ALL-");
                        }
                        else
                        {
                            productsFilterParams.IsActive = true;
                            loadDefaultValue("-All-");
                        }
                        List<ProductsDTO> productsDTOs = products.GetProductDTOList(productsFilterParams);
                        if (productsDTOs != null && productsDTOs.Any())
                        {
                            productsDTOs = productsDTOs.OrderBy(x => x.ProductName).ToList();
                            foreach (var product in productsDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(product.ProductId),product.ProductName.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRICELIST")
                    {
                        loadDefaultValue("<SELECT>");
                        //Temperory lookupDtoObject
                        CommonLookupsDTO templookupsDTO = new CommonLookupsDTO();
                        templookupsDTO.Items = new List<CommonLookupDTO>();
                        lookupDTO.DropdownName = "PRODUCT";//Explicitly Assigning DropdownName as "PRODUCT"
                        //This snippet code will fetch products, where product_type is MANUAL,ATTRACTION and COMBO   
                        ProductsList productsBLList = new ProductsList(executionContext);
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchByProductParameter = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        searchByProductParameter.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));

                        searchByProductParameter.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST, "'" + ProductTypeValues.MANUAL + "','" + ProductTypeValues.ATTRACTION + "','" + ProductTypeValues.COMBO + "'"));
                        List<ProductsDTO> productsDTOList = productsBLList.GetProductsDTOList(searchByProductParameter);// MANUAL,ATTRACTION,COMBO
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            foreach (var product in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(product.ProductId), product.ProductName.ToString());
                                templookupsDTO.Items.Add(lookupDataObject);
                            }
                        }
                        //This snippet code will fetch products, where product.AutoGenerateCardNumber = "Y" and product_type is NEW and CARDSALE
                        productsBLList = new ProductsList(executionContext);
                        searchByProductParameter = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        searchByProductParameter.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        searchByProductParameter.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST, "'" + ProductTypeValues.NEW + "','" + ProductTypeValues.RECHARGE + "','" + ProductTypeValues.CARDSALE + "'"));
                        productsDTOList = productsBLList.GetProductsDTOList(searchByProductParameter);
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            foreach (var product in productsDTOList)
                            {
                                //    if (product.AutoGenerateCardNumber == "Y")
                                //    {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(product.ProductId), product.ProductName.ToString());
                                templookupsDTO.Items.Add(lookupDataObject);
                                //    }
                            }
                        }
                        lookupDTO.Items.AddRange(templookupsDTO.Items.OrderBy(m => m.Name));// Sorting LookupsDTO.Items List by ProductName
                    }
                    else if (dropdownName.ToUpper().ToString() == "MODIFIERPRODUCT")
                    {
                        // The below snippet will fetch the Modifier Product lookup values.
                        loadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject;
                        ProductsList productsList = new ProductsList(executionContext);
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchByProductParameter = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        searchByProductParameter.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        searchByProductParameter.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.HAS_MODIFIER, string.Empty));
                        List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchByProductParameter);
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            productsDTOList = productsDTOList.OrderBy(x => x.ProductName).ToList();
                            foreach (var productsDTO in productsDTOList)
                            {
                                if (productsDTO.ActiveFlag)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName + "(InActive)");
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                        productsList = new ProductsList(executionContext);
                        searchByProductParameter = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        searchByProductParameter.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, ProductTypeValues.MANUAL));
                        searchByProductParameter.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        productsDTOList = productsList.GetProductsDTOList(searchByProductParameter, false);
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            productsDTOList = productsDTOList.OrderBy(x => x.ProductName).ToList();
                            foreach (var productDTO in productsDTOList)
                            {
                                if (productDTO.ActiveFlag)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(productDTO.ProductId), productDTO.ProductName);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(productDTO.ProductId), productDTO.ProductName + "(InActive)");
                                }
                                if (lookupDTO.Items != null && lookupDTO.Items.Any() && lookupDTO.Items.Exists(x => x.Id == lookupDataObject.Id) == false)
                                {
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "POSTYPE")
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>
                        {
                            { "1", "Default" },
                            { "2", "Ideal" },
                            { "3", "Combo" }
                        };
                        if (values.Count != 0)
                        {
                            foreach (var posType in values)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(posType.Key, posType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FREQUENCY")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "N", "<SELECT>" },
                            { "D", "Daily" },
                            { "W", "Weekly" },
                            { "M", "Monthly" },
                            { "Y", "Yearly" },
                            { "B", "Birthday" },
                            { "A", "Anniversary" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var frequency in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(frequency.Key, frequency.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "VALIDDAYSMINUTES")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "M", "Minutes" },
                            { "D", "Days" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var daymins in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(daymins.Key, daymins.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "VALIDDAYSMONTHS")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "D", "Days" },
                            { "M", "Months" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var daymins in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(daymins.Key, daymins.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DAY")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "-1","<SELECT>"},
                            { "1", "Sunday" },
                            { "2", "Monday" },
                            { "3", "Tuesday" },
                            { "4", "Wednesday" },
                            { "5", "Thursday" },
                            { "6", "Friday" },
                            { "7", "Saturday" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var daymins in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(daymins.Key, daymins.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CHECKINFACILITY")
                    {
                        loadDefaultValue("<SELECT>");
                        FacilityList facilityList = new FacilityList(executionContext);
                        List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParam.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                        List<FacilityDTO> facilityDtoList = facilityList.GetFacilityDTOList(searchParam);
                        if (facilityDtoList != null && facilityDtoList.Any())
                        {
                            facilityDtoList = facilityDtoList.OrderBy(x => x.FacilityName).ToList();
                            foreach (var facility in facilityDtoList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(facility.FacilityId), facility.FacilityName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PARENTSTRUCTURE")
                    {
                        loadDefaultValue("<SELECT>");
                        Dictionary<string, Dictionary<string, string>> taxStructureDict = new Dictionary<string, Dictionary<string, string>>();
                        TaxStructureList taxStructureList = new TaxStructureList(executionContext);
                        List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>> searchParameters = new List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.IS_ACTIVE, "1"));
                        List<TaxStructureDTO> taxStructureDTOList = taxStructureList.GetTaxStructureList(searchParameters);
                        if (taxStructureDTOList != null && taxStructureDTOList.Any())
                        {
                            foreach (var taxStructure in taxStructureDTOList)
                            {
                                if (taxStructureDict.ContainsKey(taxStructure.TaxId.ToString()) == false)
                                {
                                    taxStructureDict.Add(taxStructure.TaxId.ToString(), new Dictionary<string, string>());
                                }
                                if (taxStructureDict[taxStructure.TaxId.ToString()].ContainsKey(taxStructure.TaxStructureId.ToString()) == false)
                                {
                                    taxStructureDict[taxStructure.TaxId.ToString()].Add(taxStructure.TaxStructureId.ToString(), taxStructure.StructureName);
                                }
                            }
                        }
                        foreach (var key in taxStructureDict.Keys)
                        {
                            CommonLookupDTO lookupDataObject;
                            lookupDataObject = new CommonLookupDTO(key, "PARENTSTRUCTURE", taxStructureDict[key]);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "TAX")
                    {
                        CommonLookupDTO lookupDataObject;
                        Dictionary<string, string> values1 = new Dictionary<string, string> { { "tax_percentage", "" } };
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>", values1);//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        TaxList taxList = new TaxList(executionContext);
                        List<TaxDTO> taxDTOList = taxList.GetAllTaxes(searchParameters, false, false);
                        if (taxDTOList != null && taxDTOList.Any())
                        {
                            taxDTOList = taxDTOList.OrderBy(x => x.TaxName).ToList();
                            foreach (var taxes in taxDTOList)
                            {
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    { "tax_percentage", Convert.ToString(taxes.TaxPercentage) }
                                };
                                if (taxes.ActiveFlag)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(taxes.TaxId), taxes.TaxName, values);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(taxes.TaxId), taxes.TaxName + "(Inactive)", values);
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "WAIVERSET")
                    {
                        loadDefaultValue("<SELECT>");
                        WaiverSetListBL waiverListBL = new WaiverSetListBL(executionContext);
                        List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchParam = new List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>>();
                        searchParam.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParam.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE, "1"));
                        List<WaiverSetDTO> waiverSetDTOs = waiverListBL.GetWaiverSetDTOList(searchParam, false, false);
                        if (waiverSetDTOs != null && waiverSetDTOs.Any())
                        {
                            waiverSetDTOs = waiverSetDTOs.OrderBy(x => x.Name).ToList();
                            foreach (WaiverSetDTO waiverSetDTO in waiverSetDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(waiverSetDTO.WaiverSetId), waiverSetDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "GAMEPROFILE")
                    {
                        loadDefaultValue("-All-");

                        GameProfileList gameProfileList = new GameProfileList(executionContext);
                        List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParam = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
                        searchParam.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<GameProfileDTO> gameProfileDTOs = gameProfileList.GetGameProfileDTOList(searchParam, false);
                        if (gameProfileDTOs != null && gameProfileDTOs.Any())
                        {
                            gameProfileDTOs = gameProfileDTOs.OrderBy(x => x.ProfileName).ToList();
                            foreach (var gameProfile in gameProfileDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(gameProfile.GameProfileId), gameProfile.ProfileName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "POSMACHINES")
                    {
                        loadDefaultValue("<SELECT>");
                        POSMachineList pOSMachineList = new POSMachineList(executionContext);
                        List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParam = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                        searchParam.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<POSMachineDTO> pOSMachineDTOs = pOSMachineList.GetAllPOSMachines(searchParam, false, false);
                        if (pOSMachineDTOs != null && pOSMachineDTOs.Any())
                        {
                            pOSMachineDTOs = pOSMachineDTOs.OrderBy(x => x.POSName).ToList();
                            foreach (var posMachines in pOSMachineDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(posMachines.POSMachineId), posMachines.POSName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRICINGOPTION")
                    {
                        loadDefaultValue("<SELECT>");
                        SpecialPricingOptionsBLList specialPricingOptionsBLList = new SpecialPricingOptionsBLList(executionContext);
                        List<KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>> searchParameters = new List<KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>>();
                        searchParameters.Add(new KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>(SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<SpecialPricingOptionsDTO> specialPricingOptionsDtoObj = specialPricingOptionsBLList.GetSpecialPricingOptionsList(searchParameters, false);
                        if (specialPricingOptionsDtoObj != null && specialPricingOptionsDtoObj.Any())
                        {
                            specialPricingOptionsDtoObj = specialPricingOptionsDtoObj.OrderBy(x => x.PricingName).ToList();
                            foreach (SpecialPricingOptionsDTO specialPricingOptionsDTO in specialPricingOptionsDtoObj)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(specialPricingOptionsDTO.PricingId), specialPricingOptionsDTO.PricingName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LANGUAGE")
                    {
                        loadDefaultValue("Default");
                        Semnox.Parafait.Languages.Languages languagesList = new Semnox.Parafait.Languages.Languages(executionContext);
                        List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LanguagesDTO> languagesDTOs = languagesList.GetAllLanguagesList(searchParam);
                        if (languagesDTOs != null && languagesDTOs.Any())
                        {
                            foreach (var language in languagesDTOs)
                            {

                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(language.LanguageId), language.LanguageName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MEMBERSHIP" || dropdownName.ToUpper().ToString() == "MEMBERSHIPRULE")
                    {
                        loadDefaultValue("<SELECT>");
                        MembershipsList membershipsListBL = new MembershipsList(executionContext);
                        List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                        if (dropdownName.ToUpper().ToString() == "MEMBERSHIP")
                        {
                            searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        }
                        List<MembershipDTO> membershipList = membershipsListBL.GetAllMembership(searchParam, executionContext.GetSiteId(),false);
                        if (membershipList != null && membershipList.Any())
                        {
                            membershipList = membershipList.OrderBy(x => x.MembershipName).ToList();
                            membershipList = membershipList.OrderByDescending(x => x.IsActive).ToList();
                            foreach (var membership in membershipList)
                            {
                                string memberShipName;
                                memberShipName = membership.MembershipName;
                                if (membership.IsActive == false)
                                {
                                    memberShipName = membership.MembershipName + "(Inactive)";
                                }
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(membership.MembershipID), memberShipName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MASTERSCHEDULE")
                    {
                        loadDefaultValue("<SELECT>");
                        MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext);
                        List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                        List<MasterScheduleDTO> masterSchedules = masterScheduleList.GetMasterScheduleDTOList(searchParam);
                        if (masterSchedules != null && masterSchedules.Any())
                        {
                            masterSchedules = masterSchedules.OrderBy(x => x.MasterScheduleName).ToList();
                            foreach (var master in masterSchedules)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(master.MasterScheduleId), master.MasterScheduleName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "EMAILTEMPLATE")
                    {
                        loadDefaultValue("<SELECT>");
                        EmailTemplateListBL emailTemplateList = new EmailTemplateListBL(executionContext);
                        List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<EmailTemplateDTO> emailTemplateObj = emailTemplateList.GetEmailTemplateDTOList(searchParam);
                        if (emailTemplateObj != null && emailTemplateObj.Any())
                        {
                            foreach (var template in emailTemplateObj)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(template.EmailTemplateId), template.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATASOURCE_COLUMN_VENDOR")
                    {
                        VendorList vendorList = new VendorList(executionContext);
                        DataTable dTable = vendorList.GetVendorColumnsName();
                        var data = dTable.AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
                                  row => row.Field<string>(0));

                        foreach (var item in data)
                        {
                            CommonLookupDTO lookupDataObject;
                            if (string.IsNullOrEmpty(item.Value))
                            {
                                lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                            }
                            else
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(item.Key), item.Value);
                            }
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATASOURCE_COLUMN_CATEGORY")
                    {
                        List<KeyValuePair<string, string>> searchParameters = new List<KeyValuePair<string, string>>();
                        CategoryList categoryList = new CategoryList(executionContext);
                        DataTable dTable = categoryList.GetCategoryColumnsName();
                        if (dTable.Rows.Count != 0)
                        {
                            var categoryData = dTable.AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
                                      row => row.Field<string>(0));
                            if (categoryData != null && categoryData.Count != 0)
                            {
                                foreach (var category in categoryData)
                                {
                                    CommonLookupDTO lookupDataObject;

                                    if (string.IsNullOrEmpty(category.Value))
                                    {
                                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                                    }
                                    else
                                    {
                                        lookupDataObject = new CommonLookupDTO(Convert.ToString(category.Key), category.Value);
                                    }
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "OPTIONNAME")
                    {
                        CommonLookupDTO defaultLookupObject;
                        Dictionary<string, string> values1 = new Dictionary<string, string> { { "default_value", "" } };
                        defaultLookupObject = new CommonLookupDTO("-1", "<SELECT>", values1);//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(defaultLookupObject);
                        CommonLookupsDTO templookupsDTO = new CommonLookupsDTO();
                        templookupsDTO.Items = new List<CommonLookupDTO>();
                        ParafaitDefaultsListBL optionValueList = new ParafaitDefaultsListBL(executionContext);
                        List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ParafaitDefaultsDTO> parafaitDefaultObj = optionValueList.GetParafaitDefaultsDTOList(searchParameters);
                        if (parafaitDefaultObj != null && parafaitDefaultObj.Any())
                        {
                            foreach (var option in parafaitDefaultObj)
                            {
                                if (option.POSLevel == "Y")
                                {
                                    Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    { "default_value", Convert.ToString(option.DefaultValue) }

                                };
                                    CommonLookupDTO lookupDataObject;

                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(option.DefaultValueId), option.DefaultValueName, values);
                                    templookupsDTO.Items.Add(lookupDataObject);
                                }
                            }
                            lookupDTO.Items.AddRange(templookupsDTO.Items.OrderBy(m => m.Name));
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CHILDPRODUCT")
                    {
                        loadDefaultValue("<SELECT>");
                        string productTypes;
                        CommonLookupsDTO tempLookupDTO = new CommonLookupsDTO();
                        tempLookupDTO.Items = new List<CommonLookupDTO>();
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        if (entityName.ToUpper().ToString() == "COMBOPRODUCTDETAILS")
                        {
                            productTypes = "'" + ProductTypeValues.MANUAL + "', '" + ProductTypeValues.CHECKIN + "','" + ProductTypeValues.CHECKOUT + "','" + ProductTypeValues.ATTRACTION + "','" + ProductTypeValues.NEW + "','" + ProductTypeValues.CARDSALE + "','" + ProductTypeValues.RECHARGE + "','" + ProductTypeValues.GAMETIME + "','" + ProductTypeValues.LOCKER + "','" + ProductTypeValues.RENTAL + "'";
                        }
                        else
                        {
                            productTypes = "'" + ProductTypeValues.MANUAL + "','" + ProductTypeValues.ATTRACTION + "','" + ProductTypeValues.NEW + "','" + ProductTypeValues.CARDSALE + "','" + ProductTypeValues.RECHARGE + "','" + ProductTypeValues.GAMETIME + "','" + ProductTypeValues.COMBO + "'";
                        }
                        if (!string.IsNullOrEmpty(productTypes))
                        {
                            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST, productTypes));
                        }
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, isActive));
                        ProductsList productList = new ProductsList(executionContext);
                        List<ProductsDTO> productsDTOList = productList.GetProductsDTOList(searchParameters, false);
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            productsDTOList = productsDTOList.OrderBy(x => x.ProductName).ToList();
                            foreach (var item in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject = new CommonLookupDTO(Convert.ToString(item.ProductId), item.ProductName);
                                tempLookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                        lookupDTO.Items.AddRange(tempLookupDTO.Items.OrderBy(m => m.Name));
                    }
                    else if (dropdownName.ToUpper().ToString() == "ATTRACTIONPLAY")
                    {
                        loadDefaultValue("<SELECT>");
                        AttractionPlaysBLList attractionPlaysBLList = new AttractionPlaysBLList(executionContext);
                        List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>> searchParam = new List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>>();
                        searchParam.Add(new KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>(AttractionPlaysDTO.SearchByAttractionPlaysParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AttractionPlaysDTO> attractionPlayObj = attractionPlaysBLList.GetAttractionPlaysDTOList(searchParam);
                        if (attractionPlayObj != null && attractionPlayObj.Any())
                        {
                            attractionPlayObj = attractionPlayObj.OrderBy(x => x.PlayName).ToList();
                            foreach (var attractionPlay in attractionPlayObj)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(attractionPlay.AttractionPlayId), attractionPlay.PlayName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LICENSETYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        List<KeyValuePair<string, string>> creditPlusTypeList = CreditPlusTypeConverter.GetuserDefinedCreditPlusTypes();
                        if (creditPlusTypeList != null && creditPlusTypeList.Any())
                        {
                            foreach (var creditPlus in creditPlusTypeList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(creditPlus.Key), creditPlus.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CREDITPLUSTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        List<KeyValuePair<string, string>> creditPlusTypeList = new List<KeyValuePair<string, string>>();
                        creditPlusTypeList.Add(new KeyValuePair<string, string>(CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE), "Card Balance"));
                        creditPlusTypeList.Add(new KeyValuePair<string, string>(CreditPlusTypeConverter.ToString(CreditPlusType.COUNTER_ITEM), "Counter Items Only"));
                        creditPlusTypeList.Add(new KeyValuePair<string, string>(CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS), "Game Play Bonus"));
                        creditPlusTypeList.Add(new KeyValuePair<string, string>(CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_CREDIT), "Game Play Credits"));
                        creditPlusTypeList.Add(new KeyValuePair<string, string>(CreditPlusTypeConverter.ToString(CreditPlusType.LOYALTY_POINT), "Loyalty Points"));
                        creditPlusTypeList.Add(new KeyValuePair<string, string>(CreditPlusTypeConverter.ToString(CreditPlusType.TICKET), "Tickets"));
                        creditPlusTypeList.Add(new KeyValuePair<string, string>(CreditPlusTypeConverter.ToString(CreditPlusType.TIME), "Time"));

                        List<KeyValuePair<string, string>> udCreditPlusTypeList = CreditPlusTypeConverter.GetuserDefinedCreditPlusTypes();
                        if (udCreditPlusTypeList != null && udCreditPlusTypeList.Any())
                        {
                            creditPlusTypeList.AddRange(udCreditPlusTypeList);
                        }

                        if (creditPlusTypeList != null && creditPlusTypeList.Any())
                        {
                            foreach (var creditPlus in creditPlusTypeList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(creditPlus.Key), creditPlus.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "VENDOR")
                    {
                        loadDefaultValue("<SELECT>");
                        List<VendorDTO> vendorDTOList;
                        VendorList vendorList = new VendorList(executionContext);
                        List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorDTOSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                        vendorDTOSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, executionContext.GetSiteId().ToString()));
                        if (entityName.ToUpper().ToString() == "UPLOADINVENTORYPRODUCTS")
                        {
                            vendorDTOSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.IS_ACTIVE, "1"));
                            vendorDTOList = vendorList.GetAllVendors(vendorDTOSearchParams);
                        }
                        else
                        {
                            vendorDTOList = vendorList.GetAllVendors(vendorDTOSearchParams);

                        }
                        if (vendorDTOList != null && vendorDTOList.Any())
                        {
                            vendorDTOList = new List<VendorDTO>(vendorDTOList.OrderBy(v => v.Name));
                            foreach (VendorDTO vendorDTO in vendorDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(vendorDTO.VendorId), vendorDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "SCHEDULE")
                    {
                        List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchScheduleParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                        searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        ScheduleCalendarListBL scheduleCalendarListBL = new ScheduleCalendarListBL(executionContext);
                        List<ScheduleCalendarDTO> scheduleDTOList = scheduleCalendarListBL.GetAllSchedule(searchScheduleParameters, false, false);
                        if (scheduleDTOList != null && scheduleDTOList.Any())
                        {
                            foreach (var option in scheduleDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(option.ScheduleId), option.ScheduleName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PROFILEOPERATOR")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "","<SELECT>"},
                            { ">=", "Greater Than or Equal To" },
                            { "<=", "Less Than or Equal To" },
                            { ">", "Greater Than" },
                            { "<", "Less Than" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var operators in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(operators.Key, operators.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PROFILETYPE")
                    {
                        List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
                        selectKey.Add(new KeyValuePair<string, string>("", "<SELECT>"));
                        foreach (var select in selectKey)
                        {
                            CommonLookupDTO lookupDataObject;
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("CUSTOMER_PROFILE_TYPES", executionContext.GetSiteId());
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CUSTOMERPROFILEGROUP")
                    {
                        List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
                        selectKey.Add(new KeyValuePair<string, string>("", "<SELECT>"));
                        foreach (var select in selectKey)
                        {
                            CommonLookupDTO lookupDataObject;
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                        List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>(CustomerProfilingGroupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                        CustomerProfilingGroupListBL customerProfilingGroupListBL = new CustomerProfilingGroupListBL(executionContext);
                        List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList = customerProfilingGroupListBL.GetCustomerProfilingGroups(searchParameters, false, false);
                        if (customerProfilingGroupDTOList != null && customerProfilingGroupDTOList.Any())
                        {
                            customerProfilingGroupDTOList = customerProfilingGroupDTOList.OrderBy(x => x.CustomerProfilingGroupId).ToList();
                            foreach (var customerProfilingGroupDTO in customerProfilingGroupDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(customerProfilingGroupDTO.CustomerProfilingGroupId), customerProfilingGroupDTO.GroupName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PAUSETYPE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "0","<SELECT>"},
                            { "1", "Pause" },
                            { "2", "UnPause" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var pauseType in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(pauseType.Key, pauseType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    //added ProductGroup Lookup
                    else if (dropdownName.ToUpper().ToString() == "PRODUCT_GROUP")
                    {
                        loadDefaultValue("<SELECT>");
                        ProductGroupListBL productGroupListBL = new ProductGroupListBL(executionContext, new ExternallyManagedUnitOfWork());
                        SearchParameterList<ProductGroupDTO.SearchByParameters> searchParameters = new SearchParameterList<ProductGroupDTO.SearchByParameters>();
                        searchParameters.Add(ProductGroupDTO.SearchByParameters.IS_ACTIVE, "1");
                        searchParameters.Add(ProductGroupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString());

                        List<ProductGroupDTO> productGroupDTOList = productGroupListBL.GetProductGroupDTOList(searchParameters);
                        if (productGroupDTOList != null && productGroupDTOList.Any())
                        {
                            foreach (ProductGroupDTO productGroupDTO in productGroupDTOList)
                            {
                                CommonLookupDTO lookupDataObject = new CommonLookupDTO(productGroupDTO.Id.ToString(), productGroupDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    completeTablesDataList.Add(lookupDTO);
                }

                log.LogMethodExit(completeTablesDataList);
                return completeTablesDataList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Loads default value to the lookupDto
        /// </summary>
        /// <param name="defaultValue">default value of Type string</param>
        /// Added on 16-Apr-2019
        private void loadDefaultValue(string defaultValue)
        {
            List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
            selectKey.Add(new KeyValuePair<string, string>("-1", defaultValue));
            foreach (var select in selectKey)
            {
                CommonLookupDTO lookupDataObject;
                lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                lookupDTO.Items.Add(lookupDataObject);
            }
        }
        /// <summary>
        /// Deserializing Json string to keyValuepairs
        /// </summary>
        /// <param name="keyValuePairJsonString">Json formatted string</param>
        /// <returns>keyvaluePairs</returns>
        private Dictionary<string, string> deserializeToDictionary(string keyValuePairJsonString)
        {
            try
            {
                log.LogMethodEntry(keyValuePairJsonString);
                Dictionary<string, string> keyvaluePairs = new Dictionary<string, string>();
                Dictionary<string, object> desirializedJsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(keyValuePairJsonString);

                foreach (var obj in desirializedJsonObject)
                {
                    var value = JsonConvert.DeserializeObject<string>(obj.Value.ToString());
                    keyvaluePairs.Add(obj.Key, value);
                }
                log.LogMethodExit(keyvaluePairs);
                return keyvaluePairs;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// It will filter the details lookups based on the dynamic properties for the parent to child entity lookups
        /// </summary>
        /// <returns>completeTablesDataList</returns>
        public List<CommonLookupsDTO> GetLookupFilteredList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> completeTablesDataList = new List<CommonLookupsDTO>();
                string dropdownNames = "";
                string[] dropdowns = null;
                switch (entityName.ToUpper().ToString())
                {
                    case "PRODUCTDETAILSUPSELLOFFERS":
                        dropdownNames = "OFFER_PRODUCT";
                        break;
                    case "PRODUCTSSETUP":
                        dropdownNames = "PRODUCTTYPE";
                        break;
                    case "PRODUCTBOM":
                        dropdownNames = "CHILDPRODUCTCODE";
                        break;
                    case "INVENTORYLOCATION":
                        dropdownNames = "INVENTORYLOCATION";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                ProductsDataHandler productsDataHandler = new ProductsDataHandler();
                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO();
                    lookupDTO.Items = new List<CommonLookupDTO>();
                    lookupDTO.DropdownName = dropdownName;

                    if (dropdownName.ToUpper().ToString() == "OFFER_PRODUCT")
                    {
                        // It will filter the details based on the product type and except the current product(productId)
                        // Entity name "Upsell Offer" and dropdown name Offer Product from the Product Details page
                        // Added By Jagan 19-Mar-2019
                        keyValuePairs = deserializeToDictionary(keyValuePair);
                        Products productsList = new Products();
                        ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                        productsFilterParams.IsActive = (isActive == "1") ? true : false;
                        productsFilterParams.SiteId = executionContext.GetSiteId();
                        int productId = 0;
                        foreach (var key in keyValuePairs)
                        {
                            if (key.Key == "ProductId")
                            {
                                productId = Convert.ToInt32(key.Value);
                            }
                            if (key.Key == "ProductTypeId")
                            {
                                productsFilterParams.ProductTypeId = Convert.ToInt32(key.Value);
                            }
                        }
                        loadDefaultValue("<SELECT>");
                        List<ProductsDTO> productsDTOList = productsList.GetProductDTOList(productsFilterParams);
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            productsDTOList = productsDTOList.OrderBy(x => x.ProductName).ToList();
                            foreach (ProductsDTO productsDTO in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                if (productsDTO.ProductId != productId)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName.ToString());
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRODUCTTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        string productTypeValue = "";
                        string filter;
                        string[] productTypes;
                        CommonLookupsDTO tempLookupDTO = new CommonLookupsDTO();
                        tempLookupDTO.Items = new List<CommonLookupDTO>();
                        Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(keyValuePair);
                        foreach (var key in keyValuePairs)
                        {
                            if (key.Key == "ProductType")
                            {
                                productTypeValue = key.Value.ToString();
                            }
                        }
                        switch (productTypeValue.ToUpper().ToString())
                        {
                            case "CARDS":
                                filter = "NEW,RECHARGE,VARIABLECARD,GAMETIME,CARDSALE,LOCKER,LOCKER_RETURN";
                                break;
                            case "ATTRACTIONS":
                                //Utilities utilities = new Utilities();
                                //KeyManagement km = new KeyManagement(utilities.DBUtilities, utilities.ParafaitEnv);
                                //if (km.FeatureValid("Attraction Ticketing"))
                                filter = "ATTRACTION";//or nothing
                                //else
                                //    filter = "nothing";
                                break;
                            case "CHECKINOUT":
                                //Utilities utilities1 = new Utilities();
                                //KeyManagement km1 = new KeyManagement(utilities1.DBUtilities, utilities1.ParafaitEnv);
                                //if (km1.FeatureValid("CheckIn - CheckOut"))
                                filter = "CHECK-IN,CHECK-OUT";// or nothing
                                //else
                                //    filter = "nothing";
                                break;
                            case "RENTAL":
                                filter = "RENTAL,RENTAL_RETURN";
                                break;
                            case "NON-CARD":
                                filter = "MANUAL";
                                break;
                            case "VOUCHERS":
                                filter = "VOUCHER";
                                break;
                            case "ALL":
                                filter = "";
                                break;
                            default:
                                filter = productTypeValue.ToUpper().ToString();
                                break;
                        }
                        productTypes = filter.Split(',');
                        ProductTypeListBL productTypeList = new ProductTypeListBL(executionContext);
                        List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ProductTypeDTO> productTypeObj = productTypeList.GetProductTypeDTOList(searchParameters);
                        if (productTypeObj != null && productTypeObj.Any())
                        {
                            foreach (string item in productTypes)
                            {
                                foreach (var type in productTypeObj)
                                {
                                    if (item == "" || item == type.ProductType)
                                    {
                                        CommonLookupDTO lookupDataObject;
                                        lookupDataObject = new CommonLookupDTO(Convert.ToString(type.ProductTypeId), type.ProductType);
                                        tempLookupDTO.Items.Add(lookupDataObject);
                                    }
                                }
                            }
                        }
                        lookupDTO.Items.AddRange(tempLookupDTO.Items.OrderBy(m => m.Name));
                    }
                    else if (dropdownName.ToUpper().ToString() == "CHILDPRODUCTCODE")
                    {
                        loadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(keyValuePair);
                        int parentProductId = -1;
                        foreach (var key in keyValuePairs)
                        {
                            if (key.Key == "ProductId")
                            {
                                parentProductId = Convert.ToInt32(key.Value);
                            }
                        }
                        ProductList productList = new ProductList(executionContext);
                        List<ProductDTO> productIdDTOList = productList.GetEligibleChildProductList(parentProductId);
                        if (productIdDTOList != null && productIdDTOList.Any())
                        {
                            foreach (ProductDTO productDTO in productIdDTOList)
                            {
                                Dictionary<string, string> productKeyValuePairs = new Dictionary<string, string>();
                                productKeyValuePairs.Add("Description", productDTO.Description);
                                productKeyValuePairs.Add("Cost", productDTO.Cost.ToString());
                                productKeyValuePairs.Add("UOM", productDTO.UOMValue);
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productDTO.ProductId), productDTO.Code, productKeyValuePairs);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "INVENTORYLOCATION")
                    {
                        Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(keyValuePair);
                        string invProductCode = string.Empty;
                        foreach (var key in keyValuePairs)
                        {
                            if (key.Key == "InvProductCode")
                            {
                                invProductCode = key.Value;
                            }
                        }
                        InventoryList inventoryList = new InventoryList(executionContext);
                        Dictionary<string, string> invProductCodeList = inventoryList.GetInventoryLocations(invProductCode);
                        if (invProductCodeList != null && invProductCodeList.Any())
                        {
                            foreach (KeyValuePair<string, string> location in invProductCodeList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(location.Key), location.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    completeTablesDataList.Add(lookupDTO);
                }
                log.LogMethodExit(completeTablesDataList);
                return completeTablesDataList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}
/********************************************************************************************
 * Project Name - Game Lookup BL
 * Description  - Business class of the GameLookupBL class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        01-Apr-2019   Akshay Gulaganji    modified product.ActiveFlag check(product.ActiveFlag == "Y" to product.ActiveFlag == true)
 *2.60        09-Apr-2019   Akshay Gulaganji    modified for PRODUCTS Lookup
  *2.130.4     01-Mar-2022   Abhishek            Modified : Added GAMEMACHINELEVEL lookup for EntitlementType
 **********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Product;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Game;
using System.Linq;

namespace Semnox.CommonAPI.Lookups
{
    public class GameLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
      
        private CommonLookupDTO lookupDataObject;
        public List<LookupValuesDTO> lookupValuesDTOList;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executionContext"></param>
        public GameLookupBL(string entityName, ExecutionContext executionContext)
        {
            log.LogMethodEntry(entityName, executionContext);
            this.entityName = entityName;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the All look ups for all dropdowns based on the page in the Game module.
        /// </summary>       
        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();

                List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                string dropdownNames = "";
                string[] dropdowns = null;

                switch (entityName.ToUpper().ToString())
                {
                    case "GAMES":
                        dropdownNames = "GAME_PROFILE,PRODUCTS";
                        break;
                    case "GAME_PROFILE":
                        dropdownNames = "GAME_PROFILE,READERTHEMES,DAYLOOKUP,TIMELOOKUP,REDEEMTOKENTOTYPES";
                        break;
                    case "MACHINES":
                        dropdownNames = "GAMES,MASTERS,MACHINES,READERTHEMES,LOCATIONS,PREVIOUSMACHINES,NEXTMACHINES,TICKETMODES,READERTYPES,INTRANSIT,GAMEMACHINELEVEL";
                        break;
                    case "GENERICCALENDER":
                        dropdownNames = "DAYLOOKUP,TIMELOOKUP,READERTHEMES";
                        break;
                    case "SCREENTRANSITION":
                        dropdownNames = "THEME_TYPE,SETUPSCREEN,EVENT_LIST";
                        break;
                    case "INPUTDEVICES":
                        dropdownNames = "DEVICETYPES,DEVICEMODELS,TEMPLATEFORMATS";
                        break;
                    case "DISPLAYSCHEDULE":
                        dropdownNames = "GAMES,GAME_PROFILE,MACHINES,THEME";
                        break;
                    case "EBYTECONFIGURATION":
                        dropdownNames = "UARTPARITY,BAUDRATE,DATARATE,TRANSMISSIONMODE,IODRIVEMODE,WAKEUPTIME,FECSWITCH,OUTPUTPOWER";
                        break;
                    case "CONFIGUREHUB":
                        dropdownNames = "REGISTER";
                        break;

                }

                dropdowns = dropdownNames.Split(',');
                string siteId = Convert.ToString(executionContext.GetSiteId());
                foreach (string dropdownName in dropdowns)
                {
                    CommonLookupsDTO lookupDTO = new CommonLookupsDTO();
                    lookupDTO.Items = new List<CommonLookupDTO>();
                    lookupDTO.DropdownName = dropdownName;

                    if (dropdownName.ToUpper().ToString() == "GAME_PROFILE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, siteId));
                        GameProfileList gameProfileList = new GameProfileList(executionContext);
                        List<GameProfileDTO> gameProfileDTOList = gameProfileList.GetGameProfileDTOList(searchParameters, false);
                        if (gameProfileDTOList != null && gameProfileDTOList.Any())
                        {
                            gameProfileDTOList = gameProfileDTOList.OrderBy(x => x.ProfileName).ToList();
                            foreach (GameProfileDTO gameProfileDTO in gameProfileDTOList)
                            {
                                // Hard coded property name should remove and get property name dynamically.                                
                                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                                {
                                    { "PlayCredits", Convert.ToString(gameProfileDTO.PlayCredits) },
                                    { "VipPlayCredits", Convert.ToString(gameProfileDTO.VipPlayCredits) }
                                };
                                if (gameProfileDTO.IsActive)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(gameProfileDTO.GameProfileId), gameProfileDTO.ProfileName, keyValuePairs);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(gameProfileDTO.GameProfileId), gameProfileDTO.ProfileName + "(Inactive)", keyValuePairs);
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "GAMES")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, siteId));
                        GameList gamesList = new GameList(executionContext);
                        List<GameDTO> gamesDTOList = gamesList.GetGameList(searchParameters, false);
                        if (gamesDTOList != null && gamesDTOList.Any())
                        {
                            gamesDTOList = gamesDTOList.OrderBy(x => x.GameName).ToList();
                            // Machine Entity and Game Entity
                            // It will load all game profiles list and if the game contains normal price and vip price is null then it will fetch the respective game profile normal price and vip price values.
                            List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchGameProfileParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
                            searchGameProfileParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, siteId));
                            GameProfileList gameProfileList = new GameProfileList(executionContext);
                            List<GameProfileDTO> gameProfileDTOList = gameProfileList.GetGameProfileDTOList(searchGameProfileParameters, false);
                            foreach (GameDTO gameDTO in gamesDTOList)
                            {
                                double? playCredits;
                                double? vipPlayCredits;

                                if (gameDTO.PlayCredits != null)
                                {
                                    playCredits = gameDTO.PlayCredits;
                                }
                                else
                                {
                                    playCredits = gameProfileDTOList.Find(id => id.GameProfileId == gameDTO.GameProfileId).PlayCredits;
                                }
                                if (gameDTO.VipPlayCredits != null)
                                {
                                    vipPlayCredits = gameDTO.VipPlayCredits;
                                }
                                else
                                {
                                    vipPlayCredits = gameProfileDTOList.Find(id => id.GameProfileId == gameDTO.GameProfileId).VipPlayCredits;
                                }
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    { "PlayCredits", Convert.ToString(playCredits) },
                                    { "VipPlayCredits",Convert.ToString(vipPlayCredits) }
                                };

                                if (gameDTO.IsActive)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(gameDTO.GameId), gameDTO.GameName, values);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(gameDTO.GameId), gameDTO.GameName + "(Inactive)", values);
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRODUCTS")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<string> productsType = new List<string>();
                        productsType.Add("GAMEPLAYTRXPRODUCT");
                        productsType.Add("CHECK-IN");
                        productsType.Add("CHECK-OUT");

                        List<ProductsDTO> productsDTOList = new List<ProductsDTO>();
                        foreach (var productType in productsType)
                        {
                            List<ProductsDTO> tempProductsDTOList = new List<ProductsDTO>();
                            Semnox.Parafait.Product.Products products = new Semnox.Parafait.Product.Products();
                            ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                            productsFilterParams.ProductType = productType;
                            productsFilterParams.SiteId = executionContext.GetSiteId();
                            productsFilterParams.IsActive = true;
                            tempProductsDTOList = products.GetProductDTOList(productsFilterParams);
                            if (tempProductsDTOList != null && tempProductsDTOList.Any())
                            {
                                tempProductsDTOList = tempProductsDTOList.OrderBy(x => x.ProductName).ToList();
                                productsDTOList.AddRange(tempProductsDTOList);
                            }
                            productsFilterParams.ProductTypeId = -1;
                        }
                        if (productsDTOList != null && productsDTOList.Count != 0)
                        {
                            foreach (ProductsDTO productsDTO in productsDTOList)
                            {
                                if (productsDTO.ActiveFlag)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName + "(Inactive)");
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "READERTHEMES")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        string themeTypeList = "Audio,Display,Visualization";
                        ThemeListBL themeListBL = new ThemeListBL(executionContext);
                        List<KeyValuePair<ThemeDTO.SearchByParameters, string>> themeSearchParams = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                        themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_LIST, themeTypeList));
                        List<ThemeDTO> themeDTOList = themeListBL.GetThemeDTOList(themeSearchParams,true,true);
                        if (themeDTOList != null && themeDTOList.Any())
                        {
                            themeDTOList = themeDTOList.OrderBy(x => x.Name).ToList();
                            foreach (ThemeDTO themeDTO in themeDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(themeDTO.Id), themeDTO.Name + " [" + themeDTO.ThemeNumber + "]");
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DAYLOOKUP")
                    {
                        DayLookupList dayLookupList = new DayLookupList();
                        List<DayLookupDTO> dayLookupDTOList = dayLookupList.GetAllDayLookup();
                        if (dayLookupDTOList != null && dayLookupDTOList.Any())
                        {
                            foreach (DayLookupDTO dayLookupDTO in dayLookupDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(dayLookupDTO.Day), dayLookupDTO.Display);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TIMELOOKUP")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<decimal, string>> timeList = new List<KeyValuePair<decimal, string>>();
                        TimeSpan ts;
                        for (int i = 0; i <= 95; i++)
                        {
                            ts = new TimeSpan(0, i * 15, 0);
                            timeList.Add(new KeyValuePair<decimal, string>(Convert.ToDecimal(ts.Hours + ts.Minutes * 0.01), string.Format("{0:0}:{1:00} {2}", (ts.Hours % 12) == 0 ? (ts.Hours == 12 ? 12 : 0) : ts.Hours % 12, ts.Minutes, ts.Hours >= 12 ? "PM" : "AM")));
                        }
                        foreach (var timeValue in timeList)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(timeValue.Key.ToString("N2")), timeValue.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MASTERS")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
                        searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, siteId));

                        HubList hubList = new HubList(executionContext);
                        List<HubDTO> hubDTOList = hubList.GetHubSearchList(searchParameters);
                        if (hubDTOList != null && hubDTOList.Any())
                        {
                            hubDTOList = hubDTOList.OrderBy(x => x.MasterName).ToList();
                            foreach (HubDTO hubDTO in hubDTOList)
                            {
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    { "Address", Convert.ToString(hubDTO.Address) },
                                    { "DirectMode", Convert.ToString(hubDTO.DirectMode) }
                                };

                                if (hubDTO.IsActive)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(hubDTO.MasterId), hubDTO.MasterName, values);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(hubDTO.MasterId), hubDTO.MasterName + "(Inactive)", values);
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MACHINES" || dropdownName.ToUpper().ToString() == "PREVIOUSMACHINES" 
                            || dropdownName.ToUpper().ToString() == "NEXTMACHINES")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                        searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, siteId));

                        MachineList machineList = new MachineList(executionContext);
                        List<MachineDTO> machineDTOList = machineList.GetMachineList(searchParameters, false);
                        if (machineDTOList != null && machineDTOList.Any())
                        {
                            foreach (MachineDTO machineDTO in machineDTOList)
                            {
                                if (machineDTO.IsActive == "Y" || machineDTO.IsActive == "T")
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(machineDTO.MachineId), machineDTO.MachineName);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(machineDTO.MachineId), machineDTO.MachineName + "(Inactive)");
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "THEMETYPES")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        string lookupName = "READER_THEME_TYPE";
                        LoadLookupValues(lookupName);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LOCATIONS")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);

                        List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                        searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        LocationList locationList = new LocationList(executionContext);
                        List<LocationDTO> locationDTOList = locationList.GetAllLocations(searchParameters);                        
                        if (locationDTOList != null && locationDTOList.Count != 0)
                        {
                            foreach (LocationDTO location in locationDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(location.LocationId), location.Name.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DEVICETYPES")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        string lookupName = "MACHINE_INPUT_DEVICE_TYPE";
                        LoadLookupValues(lookupName);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DEVICEMODELS")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        string lookupName = "MACHINE_INPUT_DEVICE_MODEL";
                        LoadLookupValues(lookupName);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TEMPLATEFORMATS")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        string lookupName = "FP_TEMPLATE_FORMAT";
                        LoadLookupValues(lookupName);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "REDEEMTOKENTOTYPES")
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>
                        {
                            { "B", "Bonus" },
                            { "C", "Credit" },
                            { "RP", "Refundable CreditPlus" },
                            { "NP", "Non-Refundable CreditPlus" },
                            { "L", "Loyalty Points" }
                        };
                        foreach (KeyValuePair<string, string> redeem in values)
                        {
                            lookupDataObject = new CommonLookupDTO(redeem.Key, redeem.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "READERTYPES")
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>
                        {
                            { "-1", "Default" },
                            { "0", "2 Line" },
                            { "1", "Color ISM" },
                            { "2", "Wifi" }
                        };
                        foreach (KeyValuePair<string, string> redeem in values)
                        {
                            lookupDataObject = new CommonLookupDTO(redeem.Key, redeem.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TICKETMODES")
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>
                        {
                            { "D", "Default" },
                            { "T", "Physical" },
                            { "E", "E-Ticket" }
                        };
                        foreach (KeyValuePair<string, string> redeem in values)
                        {
                            lookupDataObject = new CommonLookupDTO(redeem.Key, redeem.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "THEME")
                    {

                        ThemeListBL themeListBL = new ThemeListBL(executionContext);
                        List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ThemeDTO> themeDTOList = themeListBL.GetThemeDTOList(searchParameters, true, true);
                        if (themeDTOList != null && themeDTOList.Any())
                        {
                            foreach (var themeValueDTO in themeDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(themeValueDTO.Id), themeValueDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);                               
                            }
                        }                        
                    }                    
                    else if (dropdownName.ToUpper().ToString() == "INTRANSIT")
                    {
                        string lookupName = "GAME_MACHINE_STATUS";
                        LoadLookupValues(lookupName);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValue), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "THEME_TYPE")
                    {
                        string themeTypeList = "Audio,Display,Visualization";
                        LoadLookupValues(dropdownName);
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (var themeTypeValueDTO in lookupValuesDTOList)
                            {
                                if (themeTypeList.Contains(themeTypeValueDTO.LookupValue) && themeTypeValueDTO.LookupName == dropdownName)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(themeTypeValueDTO.LookupValueId), themeTypeValueDTO.LookupValue);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "SETUPSCREEN")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);

                        ScreenSetupList screenSetupList = new ScreenSetupList(executionContext);
                        List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>>();
                        searchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.IS_ACTIVE, "1"));
                        List<ScreenSetupDTO> screenSetupDTOList = screenSetupList.GetAllScreenSetup(searchParams);
                        if (screenSetupDTOList != null && screenSetupDTOList.Any())
                        {
                            SortableBindingList<ScreenSetupDTO> screenSetupDTOSortableBindingList = new SortableBindingList<ScreenSetupDTO>(screenSetupDTOList);
                            foreach (var screenSetupValueDTO in screenSetupDTOSortableBindingList)
                            {                               
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(screenSetupValueDTO.ScreenId), screenSetupValueDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "EVENT_LIST")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);

                        EventListBL eventListBL = new EventListBL(executionContext);
                        List<KeyValuePair<EventDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EventDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.IS_ACTIVE, "1"));
                        List<EventDTO> eventDTOList = eventListBL.GetEventDTOList(searchParameters);
                        if (eventDTOList != null && eventDTOList.Any())
                        {
                            foreach (var eventValueDTO in eventDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(eventValueDTO.Id), eventValueDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "REGISTER")
                    {
                        foreach (var redeem in AdvancedRegister.RegisterMap)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(redeem.Value), redeem.Key.ToString());
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "UARTPARITY")
                    {                        
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                        {
                            { "0", "8N1"},{"1","8O1"},{"2","8E1"},{"3","8N1"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> counter in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(counter.Key), counter.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TRANSMISSIONMODE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                        {
                            { "0", "Transparent"},{ "1","Fixed"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> counter in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(counter.Key), counter.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "IODRIVEMODE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                        {
                            { "0", "Push-pull"},{ "1","Open Collector"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> counter in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(counter.Key), counter.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "WAKEUPTIME")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                        {
                            { "0", "250 ms"},{ "1","500 ms"},{"2","750 ms"},{"3","1000 ms"},{"4","1250 ms"},{"5","1500 ms"},{"6","1750 ms"},{"7","2000 ms"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> counter in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(counter.Key), counter.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FECSWITCH")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                        {
                            { "0", "Off"},{ "1","On"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> counter in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(counter.Key), counter.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "OUTPUTPOWER")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                        {
                            { "0", "Max dBm"},{ "1","Max - 3 dBm"},{"2","Max - 6 dBm"},{"3","Max - 9 dBm"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> counter in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(counter.Key), counter.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATARATE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                        {
                            { "0", "0.3 kbps"},{ "1","1.2 kbps"},{"2","2.4 kbps"},{"3","4.8 kbps"},{"4","9.6 kbps"},{"5","19.2 kbps"},{"6","19.2 kbps"},{"7","19.2 kbps"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> counter in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(counter.Key), counter.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "BAUDRATE")
                    {                        
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                        {
                            { "0", "1200"},{ "1","2400"},{"2","4800"},{"3","9600"},{"4","19200"},{"5","38400"},{"6","57600"},{"7","115200"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> counter in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(counter.Key), counter.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "GAMEMACHINELEVEL")
                    {
                        CommonLookupDTO lookupDataObject;
                        lookupDataObject = new CommonLookupDTO("", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "Tickets", "Tickets"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var keyValue in keyValuePairs)
                            {
                                lookupDataObject = new CommonLookupDTO(keyValue.Key, keyValue.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    lookups.Add(lookupDTO);
                }
                log.LogMethodExit(lookups);
                return lookups;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Loads the lookupValues
        /// </summary>
        /// <param name="lookupName"></param>
        private void LoadLookupValues(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - ToolsLookupBL
 * Description  - Created to fetch lookup values in Tools Module.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.90      09-Jun-2020   Mushahid Faizan   Created.
 *2.90      18-Aug-2020   Mushahid Faizan   WMS - Issue fixes for EXECUTABLENAME lookup.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Game;
using Semnox.Parafait.KioskUIFamework;
using Semnox.Parafait.Languages;
using Semnox.Parafait.logger;
using Semnox.Parafait.POS;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.DigitalSignage;
using System.IO;
using System.Web;

namespace Semnox.CommonAPI.CommonServices
{
    public class ToolsLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        private Dictionary<string, string> keyValuePairs;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        public delegate Dictionary<string, string> DelegateToolsLookup(ExecutionContext executionContext, string dropdownName);
        private CommonLookupsDTO lookupDTO = new CommonLookupsDTO();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        /// <param name="dependentDropdownName"></param>
        /// <param name="dependentDropdownSelectedId"></param>
        /// <param name="isActive"></param>
        public ToolsLookupBL(string entityName, ExecutionContext executioncontext)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor for the method ToolsLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public ToolsLookupBL(string entityName, ExecutionContext executioncontext, Dictionary<string, string> keyValuePairs)
        {
            log.LogMethodEntry(entityName, executioncontext, keyValuePairs);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            this.keyValuePairs = keyValuePairs;
            log.LogMethodExit();
        }
        public enum ToolsEntityNameLookup
        {
            LOCKERMANAGEMENT,
            LOCKERACCESSPOINT,
            MONITOR,
            PARTNERS,
            AGENTS,
            AGENTGROUPS,
            ADMANAGEMENT,
            APPLICATIONCONTENTMANAGEMENT,
            CONCURRENTMANAGER,
            ACCESSCONTROLS,
            CARDTYPEMIGRATION,
            MACHINEGROUPS,
            APPUISETUP,
            PROGRAMSTATUS,
            GENERICCALENDER
        }
        /// <summary>
        /// Gets the All lookups for all dropdowns based on the page in the Tools module.
        /// </summary>
        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> completeTablesDataList = new List<CommonLookupsDTO>();
                string[] dropdowns = null;
                string dropdownNames = string.Empty;

                ToolsEntityNameLookup toolsEntityNameLookup = (ToolsEntityNameLookup)Enum.Parse(typeof(ToolsEntityNameLookup), entityName.ToUpper().ToString());
                switch (toolsEntityNameLookup)
                {
                    case ToolsEntityNameLookup.LOCKERMANAGEMENT:
                        dropdownNames = "LOCKERZONES,LOCKER_ZONE_CODE,LOCKERMODE,LOCKERMAKE,LOCKERPANELS,PARENTZONE";
                        break;
                    case ToolsEntityNameLookup.LOCKERACCESSPOINT:
                        dropdownNames = "LOCKER_ZONE_CODE";
                        break;
                    case ToolsEntityNameLookup.GENERICCALENDER:
                        dropdownNames = "DAYLOOKUP,TIMELOOKUP,READERTHEMES";
                        break;
                    case ToolsEntityNameLookup.MONITOR:
                        dropdownNames = "MONITORASSETNAME,MONITORTYPE,MONITORAPPLICATION,MONITORAPPMODULE,MONITORPRIORITY,MONITORASSETTYPE,MONITORSTATUS";
                        break;
                    case ToolsEntityNameLookup.PARTNERS:
                        dropdownNames = "MACHINEGROUP,AGENTGROUP,POSCOUNTER";
                        break;
                    case ToolsEntityNameLookup.AGENTS:
                        dropdownNames = "PARTNERS,LOGIN_ID,USERNAME,EMAIL";
                        break;
                    case ToolsEntityNameLookup.AGENTGROUPS:
                        dropdownNames = "PARTNERS,AGENTS";
                        break;
                    case ToolsEntityNameLookup.ADMANAGEMENT:
                        dropdownNames = "ADTYPE,MACHINES";
                        break;
                    case ToolsEntityNameLookup.APPLICATIONCONTENTMANAGEMENT:
                        dropdownNames = "APPLICATION_LIST,MODULE_LIST,RICHCONTENT,LANGUAGE";
                        break;
                    case ToolsEntityNameLookup.CONCURRENTMANAGER:
                        dropdownNames = "EXECUTIONMETHOD,ARGUMENTTYPE,RUNAT,DAYLOOKUP,EXECUTABLENAME";
                        break;
                    case ToolsEntityNameLookup.ACCESSCONTROLS:
                        dropdownNames = "GAME_PROFILE,TURNSTILE_TYPE,TURNSTILE_MAKE,TURNSTILE_MODEL";
                        break;
                    case ToolsEntityNameLookup.CARDTYPEMIGRATION:
                        dropdownNames = "EXISTINGTRIGGER,REDEEMLOYALITYPOINTS,NEWMEMBERSHIP";
                        break;
                    case ToolsEntityNameLookup.APPUISETUP:
                        dropdownNames = "APP_SCREEN_PROFILE,ACTIONSCREEN,LANGUAGE,UIPANEL,ELEMENT,PARENT";
                        break;
                    case ToolsEntityNameLookup.MACHINEGROUPS:
                        dropdownNames = "MACHINES";
                        break;
                    case ToolsEntityNameLookup.PROGRAMSTATUS:
                        dropdownNames = "PHASE,STATUS";
                        break;
                    default:
                        break;
                }

                dropdowns = dropdownNames.Split(',');

                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO
                    {
                        Items = new List<CommonLookupDTO>(),
                        DropdownName = dropdownName
                    };

                    if (dropdownName.ToUpper().ToString() == "PARENTZONE")
                    {
                        loadDefaultValue("<SELECT>");
                        LockerZonesList lockerZonesList = new LockerZonesList(executionContext);
                        //List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
                        //searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<LockerZonesDTO> lockerZonesDTOList = lockerZonesList.GetParentZones(false, false);

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
                    if (dropdownName.ToUpper().ToString() == "LOCKERZONES")
                    {
                        loadDefaultValue("<SELECT>");
                        LockerZonesList lockerZonesList = new LockerZonesList(executionContext);
                        List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
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
                    else if (dropdownName.ToUpper().ToString() == "READERTHEMES")
                    {
                        loadDefaultValue("<SELECT>");
                        string themeTypeList = "Audio,Display,Visualization";
                        ThemeListBL themeListBL = new ThemeListBL(executionContext);
                        List<KeyValuePair<ThemeDTO.SearchByParameters, string>> themeSearchParams = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                        themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_LIST, themeTypeList));
                        List<ThemeDTO> themeDTOList = themeListBL.GetThemeDTOList(themeSearchParams);
                        if (themeDTOList != null && themeDTOList.Any())
                        {
                            themeDTOList = themeDTOList.OrderBy(x => x.Name).ToList();
                            foreach (ThemeDTO themeDTO in themeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(themeDTO.Id), themeDTO.Name + " [" + themeDTO.ThemeNumber + "]");
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DAYLOOKUP")
                    {
                        //loadDefaultValue("<SELECT>");
                        DayLookupList dayLookupList = new DayLookupList();
                        List<DayLookupDTO> dayLookupDTOList = dayLookupList.GetAllDayLookup();
                        if (dayLookupDTOList != null && dayLookupDTOList.Any())
                        {
                            foreach (DayLookupDTO dayLookupDTO in dayLookupDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(dayLookupDTO.Day), dayLookupDTO.Display);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TIMELOOKUP")
                    {
                        loadDefaultValue("<SELECT>");
                        List<KeyValuePair<decimal, string>> timeList = new List<KeyValuePair<decimal, string>>();
                        TimeSpan ts;
                        for (int i = 0; i <= 95; i++)
                        {
                            ts = new TimeSpan(0, i * 15, 0);
                            timeList.Add(new KeyValuePair<decimal, string>(Convert.ToDecimal(ts.Hours + ts.Minutes * 0.01), string.Format("{0:0}:{1:00} {2}", (ts.Hours % 12) == 0 ? (ts.Hours == 12 ? 12 : 0) : ts.Hours % 12, ts.Minutes, ts.Hours >= 12 ? "PM" : "AM")));
                        }
                        foreach (var timeValue in timeList)
                        {
                            CommonLookupDTO lookupDataObject;
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(timeValue.Key.ToString("N2")), timeValue.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LOCKERMODE")
                    {
                        //loadDefaultValue("<SELECT>");
                        DefaultDataTypeBL defaultDataTypeBL = new DefaultDataTypeBL(executionContext);
                        Dictionary<string, string> keyValuePairs = defaultDataTypeBL.GetDefaultCustomDataType("Custom10");
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var template in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(template.Key, template.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LOCKERMAKE")
                    {
                       // loadDefaultValue("<SELECT>");
                        DefaultDataTypeBL defaultDataTypeBL = new DefaultDataTypeBL(executionContext);
                        Dictionary<string, string> keyValuePairs = defaultDataTypeBL.GetDefaultCustomDataType("Custom11");
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var template in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(template.Key, template.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LOCKERPANELS")
                    {
                        loadDefaultValue("<SELECT>");
                        LockerPanelsList lockerPanelsList = new LockerPanelsList(executionContext);
                        List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>> searchParameters = new List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>>();
                        searchParameters.Add(new KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>(LockerPanelDTO.SearchByLockerPanelsParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<LockerPanelDTO> lockerPanelDTOList = lockerPanelsList.GetAllLockerPanels(searchParameters, false);

                        if (lockerPanelDTOList != null && lockerPanelDTOList.Any())
                        {
                            foreach (LockerPanelDTO lockerPanelDTO in lockerPanelDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lockerPanelDTO.PanelId), lockerPanelDTO.PanelName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MONITORASSETTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        MonitorAssetTypeList monitorAssetTypeList = new MonitorAssetTypeList(executionContext);
                        List<KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>> searchParameters = new List<KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>>();
                        searchParameters.Add(new KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>(MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<MonitorAssetTypeDTO> monitorAssetTypeDTOList = monitorAssetTypeList.GetAllMonitorAssetTypes(searchParameters);

                        if (monitorAssetTypeDTOList != null && monitorAssetTypeDTOList.Any())
                        {
                            foreach (MonitorAssetTypeDTO monitorAssetTypeDTO in monitorAssetTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(monitorAssetTypeDTO.AssetTypeId), monitorAssetTypeDTO.AssetType);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MONITORASSETNAME")
                    {
                        loadDefaultValue("<SELECT>");
                        MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext);
                        List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> searchParameters = new List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>>();
                        //searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<MonitorAssetDTO> monitorAssetDTOList = monitorAssetList.GetAllMonitorAssets(searchParameters);
                        if (monitorAssetDTOList != null && monitorAssetDTOList.Any())
                        {
                            foreach (MonitorAssetDTO monitorAssetDTO in monitorAssetDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(monitorAssetDTO.AssetId), monitorAssetDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MONITORTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        MonitorTypeList monitorTypeList = new MonitorTypeList(executionContext);
                        List<KeyValuePair<MonitorTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MonitorTypeDTO.SearchByParameters, string>>();
                        //searchParameters.Add(new KeyValuePair<MonitorTypeDTO.SearchByParameters, string>(MonitorTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<MonitorTypeDTO> monitorTypeDTOList = monitorTypeList.GetAllMonitorTypeDTO(searchParameters);
                        if (monitorTypeDTOList != null && monitorTypeDTOList.Any())
                        {
                            foreach (MonitorTypeDTO monitorTypeDTO in monitorTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(monitorTypeDTO.MonitorTypeId), monitorTypeDTO.MonitorType);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MONITORAPPLICATION")
                    {
                        loadDefaultValue("<SELECT>");
                        MonitorApplicationList monitorApplicationList = new MonitorApplicationList(executionContext);
                        List<KeyValuePair<MonitorApplicationDTO.SearchByMonitorApplicationParameters, string>> searchParameters = new List<KeyValuePair<MonitorApplicationDTO.SearchByMonitorApplicationParameters, string>>();
                        //searchParameters.Add(new KeyValuePair<MonitorApplicationDTO.SearchByMonitorApplicationParameters, string>(MonitorApplicationDTO.SearchByMonitorApplicationParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<MonitorApplicationDTO> monitorApplicationDTOList = monitorApplicationList.GetAllMonitorApplicationDTO(searchParameters);
                        if (monitorApplicationDTOList != null && monitorApplicationDTOList.Any())
                        {
                            foreach (MonitorApplicationDTO monitorApplicationDTO in monitorApplicationDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(monitorApplicationDTO.ApplicationId), monitorApplicationDTO.ApplicationName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MONITORSTATUS")
                    {
                        loadDefaultValue("<SELECT>");
                        MonitorLogStatusList monitorLogStatusList = new MonitorLogStatusList(executionContext);
                        List<KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string>>();
                        List<MonitorLogStatusDTO> monitorLogStatusDTOList = monitorLogStatusList.GetAllMonitorLogStatusDTO(searchParameters);
                        if (monitorLogStatusDTOList != null && monitorLogStatusDTOList.Any())
                        {
                            foreach (MonitorLogStatusDTO monitorLogStatusTO in monitorLogStatusDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(monitorLogStatusTO.StatusId), monitorLogStatusTO.Status);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MONITORAPPMODULE")
                    {
                        loadDefaultValue("<SELECT>");
                        MonitorAppModuleList monitorAppModuleList = new MonitorAppModuleList(executionContext);
                        List<KeyValuePair<MonitorAppModuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MonitorAppModuleDTO.SearchByParameters, string>>();
                        //searchParameters.Add(new KeyValuePair<MonitorAppModuleDTO.SearchByParameters, string>(MonitorAppModuleDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<MonitorAppModuleDTO> monitorAppModuleDTOList = monitorAppModuleList.GetAllMonitorAppModuleDTO(searchParameters);
                        if (monitorAppModuleDTOList != null && monitorAppModuleDTOList.Any())
                        {
                            foreach (MonitorAppModuleDTO monitorAppModuleDTO in monitorAppModuleDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(monitorAppModuleDTO.ModuleId), monitorAppModuleDTO.ModuleName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MONITORPRIORITY")
                    {
                        loadDefaultValue("<SELECT>");
                        MonitorPriorityList monitorPriorityList = new MonitorPriorityList(executionContext);
                        List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>>();
                       // searchParameters.Add(new KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>(MonitorPriorityDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<MonitorPriorityDTO> monitorPriorityDTOList = monitorPriorityList.GetAllMonitorPriorityList(searchParameters);
                        if (monitorPriorityDTOList != null && monitorPriorityDTOList.Any())
                        {
                            foreach (MonitorPriorityDTO monitorPriorityDTO in monitorPriorityDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(monitorPriorityDTO.PriorityId), monitorPriorityDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MACHINEGROUP")
                    {
                        loadDefaultValue("-All-");
                        MachineGroupsList machineGroupsList = new MachineGroupsList(executionContext);
                        List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<MachineGroupsDTO> machineGroupsDTOList = machineGroupsList.GetAllMachineGroupsDTOList(searchParameters);
                        if (machineGroupsDTOList != null && machineGroupsDTOList.Any())
                        {
                            foreach (MachineGroupsDTO machineGroupsDTO in machineGroupsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(machineGroupsDTO.MachineGroupId), machineGroupsDTO.GroupName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "AGENTGROUP")
                    {
                        loadDefaultValue("<SELECT>");
                        AgentGroupsList agentGroupsList = new AgentGroupsList(executionContext);
                        List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<AgentGroupsDTO> agentGroupsDTOList = agentGroupsList.GetAllAgentGroupsList(searchParameters);
                        if (agentGroupsDTOList != null && agentGroupsDTOList.Any())
                        {
                            foreach (AgentGroupsDTO agentGroupsDTO in agentGroupsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(agentGroupsDTO.AgentGroupId), agentGroupsDTO.GroupName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "POSCOUNTER")
                    {
                        loadDefaultValue("-None-");
                        POSTypeListBL posTypeListBL = new POSTypeListBL(executionContext);
                        List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<POSTypeDTO> posTypeDTOList = posTypeListBL.GetPOSTypeDTOList(searchParameters, null);
                        if (posTypeDTOList != null && posTypeDTOList.Any())
                        {
                            posTypeDTOList = posTypeDTOList.OrderBy(x => x.POSTypeName).ToList();
                            foreach (POSTypeDTO posTypeDTO in posTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(posTypeDTO.POSTypeId), posTypeDTO.POSTypeName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PARTNERS")
                    {
                        loadDefaultValue("<SELECT>");
                        PartnersList partnersList = new PartnersList(executionContext);
                        List<KeyValuePair<PartnersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PartnersDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PartnersDTO.SearchByParameters, string>(PartnersDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<PartnersDTO> partnersDTOList = partnersList.GetAllPartnersList(searchParameters);
                        if (partnersDTOList != null && partnersDTOList.Any())
                        {
                            foreach (PartnersDTO partnersDTO in partnersDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(partnersDTO.PartnerId), partnersDTO.PartnerName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ADTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "A", "Ad" },
                            { "T", "Theme" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(keyValues.Key, keyValues.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RICHCONTENT")
                    {
                        loadDefaultValue("<SELECT>");
                        RichContentListBL richContentListBL = new RichContentListBL(executionContext);
                        List<KeyValuePair<RichContentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RichContentDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        List<RichContentDTO> richContentDTOList = richContentListBL.GetRichContentDTOList(searchParameters);

                        if (richContentDTOList != null && richContentDTOList.Any())
                        {
                            foreach (RichContentDTO richContentDTO in richContentDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(richContentDTO.Id), richContentDTO.ContentName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LANGUAGE")
                    {
                        loadDefaultValue("<SELECT>");
                        Languages languagesList = new Languages(executionContext);
                        List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LanguagesDTO> languagesDTOList = languagesList.GetAllLanguagesList(searchParam);
                        if (languagesDTOList != null && languagesDTOList.Any())
                        {
                            languagesDTOList = languagesDTOList.OrderBy(x => x.LanguageName).ToList();
                            foreach (LanguagesDTO languagesDTO in languagesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(languagesDTO.LanguageId), languagesDTO.LanguageName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "EXECUTIONMETHOD")
                    {
                        loadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "E", "EXE" },
                            { "L", "LIBRARY" },
                            { "P", "SQL Procedure" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(keyValues.Key, keyValues.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ARGUMENTTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "S", "String" },
                            { "I", "Int" },
                            { "B", "Boolean" },
                            { "D", "DateTime" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(keyValues.Key, keyValues.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RUNAT")
                    {
                        loadDefaultValue("<SELECT>");
                        for (int i = 00; i < 24; i++)
                        {
                            for (int k = 00; k < 60; k = k + 5)
                            {
                                CommonLookupDTO digatalSignageDataObject;
                                if (k == 00)
                                {
                                    digatalSignageDataObject = new CommonLookupDTO(Convert.ToString(i.ToString() + ":00"), Convert.ToString(i.ToString() + ":00"));
                                }
                                else if (k == 5)
                                {
                                    digatalSignageDataObject = new CommonLookupDTO(Convert.ToString(i.ToString() + ":" + k), Convert.ToString(i.ToString() + ":0" + k));
                                }
                                else
                                {
                                    digatalSignageDataObject = new CommonLookupDTO(Convert.ToString(i.ToString() + ":" + k), Convert.ToString(i.ToString() + ":" + k));
                                }
                                lookupDTO.Items.Add(digatalSignageDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DAYLOOKUP")
                    {
                        DayLookupList dayLookupList = new DayLookupList();
                        List<DayLookupDTO> dayLookups = dayLookupList.GetAllDayLookup();
                        if (dayLookups.Count != 0)
                        {
                            foreach (var dayLookupDTO in dayLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(dayLookupDTO.Day), dayLookupDTO.Display);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "GAME_PROFILE")
                    {
                        loadDefaultValue("<SELECT>");
                        List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        GameProfileList gameProfileList = new GameProfileList(executionContext);
                        List<GameProfileDTO> gameProfileDTOList = gameProfileList.GetGameProfileDTOList(searchParameters, false);
                        if (gameProfileDTOList != null && gameProfileDTOList.Any())
                        {
                            foreach (GameProfileDTO gameProfileDTO in gameProfileDTOList)
                            {

                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(gameProfileDTO.GameProfileId), gameProfileDTO.ProfileName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "EXISTINGTRIGGER")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "-1", "<SELECT>" },
                            { "1", "Purchase" },
                            { "2", "Recharge" },
                            { "3", "Loyalty Points" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(keyValues.Key, keyValues.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "REDEEMLOYALITYPOINTS")
                    {
                        loadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "0", "No" },
                            { "1", "Yes" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(keyValues.Key, keyValues.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "NEWMEMBERSHIP")
                    {
                        loadDefaultValue("<SELECT>");
                        MembershipsList membershipList = new MembershipsList(executionContext);
                        List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));
                        searchByParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<MembershipDTO> membershipDTOList = membershipList.GetAllMembership(searchByParameters, executionContext.GetSiteId(), false);
                        if (membershipDTOList != null && membershipDTOList.Any())
                        {
                            foreach (MembershipDTO membershipDTO in membershipDTOList)
                            {

                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(membershipDTO.MembershipID), membershipDTO.MembershipName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "APP_SCREEN_PROFILE")
                    {
                        loadDefaultValue("<SELECT>");
                        AppScreenProfileListBL appScreenProfileListBL = new AppScreenProfileListBL(executionContext);
                        List<KeyValuePair<AppScreenProfileDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AppScreenProfileDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<AppScreenProfileDTO.SearchByParameters, string>(AppScreenProfileDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AppScreenProfileDTO> appScreenProfileDTOList = appScreenProfileListBL.GetAllAppScreenProfileDTOList(searchByParameters);
                        if (appScreenProfileDTOList != null && appScreenProfileDTOList.Any())
                        {
                            foreach (AppScreenProfileDTO appScreenProfileDTO in appScreenProfileDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(appScreenProfileDTO.AppScreenProfileId), appScreenProfileDTO.AppScreenProfileName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ACTIONSCREEN")
                    {
                        loadDefaultValue("<SELECT>");
                        AppScreenListBL appScreenListBL = new AppScreenListBL(executionContext);
                        List<KeyValuePair<AppScreenDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AppScreenDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<AppScreenDTO.SearchByParameters, string>(AppScreenDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AppScreenDTO> appScreenDTOList = appScreenListBL.GetAppScreenDTOList(searchByParameters, false, true);
                        if (appScreenDTOList != null && appScreenDTOList.Any())
                        {
                            foreach (AppScreenDTO appScreenDTO in appScreenDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(appScreenDTO.ScreenId), appScreenDTO.ScreenName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "UIPANEL")
                    {
                        loadDefaultValue("<SELECT>");
                        AppUIPanelListBL appUIPanelListBL = new AppUIPanelListBL(executionContext);
                        List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<AppUIPanelDTO.SearchByParameters, string>(AppUIPanelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AppUIPanelDTO> appUIPanelDTOList = appUIPanelListBL.GetAppUIPanelDTOList(searchByParameters, false, true);
                        if (appUIPanelDTOList != null && appUIPanelDTOList.Any())
                        {
                            foreach (AppUIPanelDTO appUIPanelDTO in appUIPanelDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(appUIPanelDTO.UIPanelId), appUIPanelDTO.UIPanelName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "ELEMENT")
                    {
                        loadDefaultValue("<SELECT>");
                        AppUIPanelElementListBL appUIPanelElementListBL = new AppUIPanelElementListBL(executionContext);
                        List<KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>(AppUIPanelElementDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AppUIPanelElementDTO> appUIPanelElementDTOList = appUIPanelElementListBL.GetAppUIPanelElementDTOList(searchByParameters, false, true);
                        if (appUIPanelElementDTOList != null && appUIPanelElementDTOList.Any())
                        {
                            foreach (AppUIPanelElementDTO appUIPanelElementDTO in appUIPanelElementDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(appUIPanelElementDTO.UIPanelElementId), appUIPanelElementDTO.ElementName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "PARENT")
                    {
                        loadDefaultValue("<SELECT>");
                        AppUIElementParameterListBL appUIElementParameterListBL = new AppUIElementParameterListBL(executionContext);
                        List<KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>(AppUIElementParameterDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AppUIElementParameterDTO> appUIElementParameterDTOList = appUIElementParameterListBL.GetAppUIElementParameterDTOList(searchByParameters, false, true);
                        if (appUIElementParameterDTOList != null && appUIElementParameterDTOList.Any())
                        {
                            foreach (AppUIElementParameterDTO appUIElementParameterDTO in appUIElementParameterDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(appUIElementParameterDTO.ParameterId), appUIElementParameterDTO.ParameterName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "LOGIN_ID")
                    {
                        loadDefaultValue("<SELECT>");
                        UsersList users = new UsersList(executionContext);
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UsersDTO> usersDTOList = users.GetAllUsers(searchParameters);
                        if (usersDTOList != null && usersDTOList.Any())
                        {
                            foreach (UsersDTO usersDTO in usersDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.LoginId);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "USERNAME")
                    {
                        loadDefaultValue("<SELECT>");
                        UsersList users = new UsersList(executionContext);
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UsersDTO> usersDTOList = users.GetAllUsers(searchParameters);
                        if (usersDTOList != null && usersDTOList.Any())
                        {
                            foreach (UsersDTO usersDTO in usersDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.UserName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "EMAIL")
                    {
                        loadDefaultValue("<SELECT>");
                        UsersList users = new UsersList(executionContext);
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UsersDTO> usersDTOList = users.GetAllUsers(searchParameters);
                        if (usersDTOList != null && usersDTOList.Any())
                        {
                            foreach (UsersDTO usersDTO in usersDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.Email);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "MACHINES")
                    {
                        //loadDefaultValue("<SELECT>");
                        MachineList machineList = new MachineList(executionContext);
                        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                        searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<MachineDTO> machineDTOList = machineList.GetMachineList(searchParameters);
                        if (machineDTOList != null && machineDTOList.Any())
                        {
                            foreach (MachineDTO machineDTO in machineDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(machineDTO.MachineId), machineDTO.MachineName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "AGENTS")
                    {
                        loadDefaultValue("<SELECT>");
                        AgentsList agentsList = new AgentsList(executionContext);
                        List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AgentsDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AgentsDTO> agentsDTOList = agentsList.GetAllAgentsList(searchParameters);
                        if (agentsDTOList != null && agentsDTOList.Any())
                        {
                            agentsDTOList = agentsDTOList.OrderBy(x => x.User_Id).ToList();
                            foreach (AgentsDTO agentsDTO in agentsDTOList)
                            {
                                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                                searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, agentsDTO.User_Id.ToString()));

                                UsersList usersList = new UsersList(executionContext);
                                List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
                                //usersDTOs = usersDTOs.OrderBy(x => x.UserName).ToList();
                                foreach (UsersDTO usersDTO in usersDTOs)
                                {
                                    CommonLookupDTO lookupDataObject;
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(agentsDTO.AgentId), usersDTO.UserName);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }

                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "EXECUTABLENAME")
                    {
                        //string root = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("\\bin\\debug", "\\");
                        //root = root.Replace("file:\\", "");

                        //log.Debug("Path is  : " + root);
                        //if (root.Contains("\\Program Files\\Semnox Solutions".ToLower()) == true)
                        //{
                        //    root = root.Replace("\\application", "\\server") + "\\";
                        //}
                        //log.Debug("New Path is  : " + root);

                        //string[] exeFiles = Directory.GetFiles(root, "*.exe");

                        //for (int i = 0; i < exeFiles.Length; i++)
                        //    exeFiles[i] = Path.GetFileName(exeFiles[i]);
                        Parafait.JobUtils.ConcurrentProgramList concurrentProgramList = new Parafait.JobUtils.ConcurrentProgramList(executionContext);
                        List<KeyValuePair<Parafait.JobUtils.ConcurrentProgramsDTO.SearchByProgramsParameters, string>> programSearch = new List<KeyValuePair<Parafait.JobUtils.ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
                        programSearch.Add(new KeyValuePair<Parafait.JobUtils.ConcurrentProgramsDTO.SearchByProgramsParameters, string>(Parafait.JobUtils.ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "1"));
                        programSearch.Add(new KeyValuePair<Parafait.JobUtils.ConcurrentProgramsDTO.SearchByProgramsParameters, string>(Parafait.JobUtils.ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<Parafait.JobUtils.ConcurrentProgramsDTO> concurrentProgramsListDTO = concurrentProgramList.GetAllConcurrentPrograms(programSearch);
                        concurrentProgramsListDTO = concurrentProgramsListDTO.Where(x=>x.ExecutionMethod == "E").ToList();
                        if (concurrentProgramsListDTO != null && concurrentProgramsListDTO.Any())
                        {
                            foreach (Parafait.JobUtils.ConcurrentProgramsDTO concurrentProgramsDTO in concurrentProgramsListDTO)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(concurrentProgramsDTO.ProgramId.ToString(), concurrentProgramsDTO.ExecutableName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PHASE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "-All-", "-All-" },
                            { "Complete", "Complete" },
                            { "Pending", "Pending" },
                            { "Running", "Running" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(keyValues.Key, keyValues.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "STATUS")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "-All-", "-All-" },
                            { "Normal", "Normal" },
                            { "Error", "Error" },
                            { "Aborted", "Aborted" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(keyValues.Key, keyValues.Value);
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
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Loads default value to the lookupDto
        /// </summary>
        /// <param name="defaultValue">default value of Type string</param>        
        private void loadDefaultValue(string defaultValue)
        {
            List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
            selectKey.Add(new KeyValuePair<string, string>("-1", defaultValue));
            foreach (KeyValuePair<string, string> select in selectKey)
            {
                CommonLookupDTO lookupDataObject;
                lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                lookupDTO.Items.Add(lookupDataObject);
            }
        }
        ///// <summary>
        ///// Lookups
        ///// </summary>
        ///// <param name="lookupName"></param>
        ///// <returns></returns>
        //private List<LookupValuesDTO> GetLookups(string lookupName)
        //{
        //    log.LogMethodEntry(lookupName);
        //    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
        //    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
        //    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
        //    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
        //    List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
        //    log.LogMethodExit(lookupValuesDTOList);
        //    return lookupValuesDTOList;
        //}
    }
}

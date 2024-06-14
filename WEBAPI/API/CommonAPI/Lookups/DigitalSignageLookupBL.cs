/********************************************************************************************
 * Project Name - Digital Signage
 * Description  - Business class of the DigitalSignageLookupBL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.60        07-Apr-2019      Akshay Gulaganji     added "CONTENT_TYPE", "PANEL_TYPE_ACTIVE" and "PATTERNCONTENT" lookups
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using Semnox.Parafait.DigitalSignage;

namespace Semnox.CommonAPI.Lookups
{
    public class DigitalSignageLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        string dependentDropdownName = string.Empty;
        string dependentDropdownSelectedId = string.Empty;
        string isActive = string.Empty;
        public List<MediaDTO> mediaDTOList;
        private List<LookupValuesDTO> lookupValuesDTOList;
        private CommonLookupDTO lookupDataObject;

        /// <summary>
        /// Constructor for the DigitalSignageLookupMasterBL
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        /// <param name="dependentDropdownName"></param>
        /// <param name="dependentDropdownSelectedId"></param>
        /// <param name="isActive"></param>
        public DigitalSignageLookupBL(string entityName, ExecutionContext executioncontext, string dependentDropdownName, string dependentDropdownSelectedId, string isActive)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            this.dependentDropdownName = dependentDropdownName;
            this.dependentDropdownSelectedId = dependentDropdownSelectedId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the All look ups for all dropdowns based on the page in the Digital Signage module.
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
                    case "CONTENTMEDIA":
                        dropdownNames = "MEDIA_TYPE";
                        break;
                    case "CONTENTLIST":
                        dropdownNames = "INDENTATION,DATA_TYPE";
                        break;
                    case "CONTENTTICKER":
                        dropdownNames = "INDENTATION";
                        break;
                    case "EVENT":
                        dropdownNames = "EVENT_TYPE";
                        break;
                    case "SETUPPANEL":
                        dropdownNames = "SCREEN RESOLUTION HORIZONTAL,SCREEN RESOLUTION VERTICAL,TIME_TYPE";
                        break;
                    case "SETUPSCREENDESIGN":
                        dropdownNames = "SCREEN_ALIGNMENT";
                        break;
                    case "SETUPSCREENDESIGNCONTENTMAP":
                        dropdownNames = "CONTENT_TYPE,LOOKUPCONTENT,PATTERNCONTENT,TICKERCONTENT,IMAGECONTENT,AUDIOCONTENT,VIDEOCONTENT,MEDIA_LIST,INDENTATION"; // IMAGE_SIZE,MEDIA_TYPE // Indentation for Ticker Scroll Direction
                        break;
                    case "SCREENTRANSITION":
                        dropdownNames = "THEME_TYPE,SETUPSCREEN,EVENT_LIST";
                        break;
                    case "SIGNAGESCHEDULE":
                        dropdownNames = "RECURFREQUENCY,RECURTYPE,PANEL_TYPE,PANEL_TYPE_ACTIVE,THEMES,TIMELOOKUP";
                        break;
                    case "SIGNAGESCHEDULEEXCLUSION":
                        dropdownNames = "DAYLOOKUP";
                        break;
                    case "SIGNAGESCHEDULEFILTER":
                        dropdownNames = "PANEL_TYPE";
                        break;

                }

                dropdowns = dropdownNames.Split(',');

                foreach (string dropdownName in dropdowns)
                {
                    CommonLookupsDTO lookupDTO = new CommonLookupsDTO();
                    lookupDTO.Items = new List<CommonLookupDTO>();
                    lookupDTO.DropdownName = dropdownName;
                    if (dropdownName.ToUpper().ToString() == "TIME_TYPE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>"); //to load Default Value (i.e., <SELECT>)
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
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(timeValue.Key), timeValue.Value);
                            lookupDTO.Items.Add(lookupDataObject);
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
                    else if (dropdownName.ToUpper().ToString() == "LOOKUPCONTENT")
                    {
                        DSLookupListBL dSLookupListBL = new DSLookupListBL(executionContext);
                        List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<DSLookupDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchByParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.IS_ACTIVE, isActive));
                        List<DSLookupDTO> dSLookupDTOList = dSLookupListBL.GetDSLookupDTOList(searchByParameters);
                        if (dSLookupDTOList != null && dSLookupDTOList.Any())
                        {
                            foreach (var dsLookupValueDTO in dSLookupDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(dsLookupValueDTO.DSLookupID), dsLookupValueDTO.DSLookupName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATA_TYPE")
                    {
                        List<LookupValuesDTO> lookupValuesDTOList = LoadLookupValues("DATA_TYPE");
                        foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                        {
                            CommonLookupDTO lookupDataObject;
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CONTENT_TYPE") //"Digital Signage --> Setup --> Screen Design --> Content Map --> Screen Content Map"
                    {
                        if (dependentDropdownName.ToUpper().ToString() == "CONTENT_TYPE")
                        {
                            lookupDTO.DropdownName = dependentDropdownName;
                        }
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<LookupValuesDTO> lookupValuesDTOList = LoadLookupValues(dropdownName);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            //the below snippet will fetch the list of DSLookupDTO
                            DSLookupListBL dSLookupListBL = new DSLookupListBL(executionContext);
                            List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchByLookUpParameters = new List<KeyValuePair<DSLookupDTO.SearchByParameters, string>>();
                            searchByLookUpParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            searchByLookUpParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.IS_ACTIVE, isActive));
                            List<DSLookupDTO> dSLookupDTOList = dSLookupListBL.GetDSLookupDTOList(searchByLookUpParameters);
                            if (dSLookupDTOList != null && dSLookupDTOList.Any())
                            {
                                dSLookupDTOList = dSLookupDTOList.OrderBy(x => x.DSLookupName).ToList();
                                foreach (var lookupValueDTO in lookupValuesDTOList)
                                {
                                    if (lookupValueDTO.LookupValue == "LOOKUP")
                                    {
                                        lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValueDTO.LookupValueId), lookupValueDTO.Description);
                                        lookupDTO.Items.Add(lookupDataObject);
                                    }
                                }
                            }
                            //the below snippet will fetch the list of Ticker
                            TickerListBL tickerListBL = new TickerListBL(executionContext);
                            List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchByTickerParameters = new List<KeyValuePair<TickerDTO.SearchByParameters, string>>();
                            searchByTickerParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            searchByTickerParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.IS_ACTIVE, isActive));
                            List<TickerDTO> tickerDTOList = tickerListBL.GetTickerDTOList(searchByTickerParameters);
                            if (tickerDTOList != null)
                            {
                                foreach (var lookupValueDTO in lookupValuesDTOList)
                                {
                                    if (lookupValueDTO.LookupValue == "TICKER")
                                    {
                                        lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValueDTO.LookupValueId), lookupValueDTO.Description);
                                        lookupDTO.Items.Add(lookupDataObject);
                                    }
                                }
                            }
                            //the below snippet will fetch the list of Signage Pattern
                            SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext);
                            List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.IS_ACTIVE, "1"));
                            searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            List<SignagePatternDTO> signagePatternDTOList = signagePatternListBL.GetSignagePatternDTOList(searchParameters);
                            if (signagePatternDTOList != null && signagePatternDTOList.Any())
                            {
                                foreach (var lookupValueDTO in lookupValuesDTOList)
                                {
                                    if (lookupValueDTO.LookupValue == "PATTERN")
                                    {
                                        lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValueDTO.LookupValueId), lookupValueDTO.Description);
                                        lookupDTO.Items.Add(lookupDataObject);
                                    }
                                }
                            }
                            //the below snippet will fetch the list of Media
                            LoadMediaList();
                            List<LookupValuesDTO> mediaTypeLookupsList = LoadLookupValues("MEDIA_TYPE");
                            bool image = false;//to load distinct image value
                            bool audio = false;//to load distinct audio value
                            bool video = false;//to load distinct video value
                            if (lookupValuesDTOList != null)
                            {
                                if (mediaDTOList != null)
                                {
                                    foreach (LookupValuesDTO mediaLookupValueDTO in mediaTypeLookupsList)
                                    {
                                        foreach (MediaDTO mediaDto in mediaDTOList)
                                        {
                                            if (mediaLookupValueDTO.LookupValue == "IMAGE" && mediaDto.TypeId == mediaLookupValueDTO.LookupValueId)
                                            {
                                                if (!image)
                                                {
                                                    CommonLookupDTO imageDataObject;
                                                    LookupValuesDTO lookupImageValuesDTO = lookupValuesDTOList.Where(m => m.LookupValue == "IMAGE").FirstOrDefault();
                                                    imageDataObject = new CommonLookupDTO(Convert.ToString(lookupImageValuesDTO.LookupValueId), lookupImageValuesDTO.LookupValue);
                                                    lookupDTO.Items.Add(imageDataObject);
                                                    image = true;
                                                }
                                            }
                                            if (mediaLookupValueDTO.LookupValue == "AUDIO" && mediaDto.TypeId == mediaLookupValueDTO.LookupValueId)
                                            {
                                                if (!audio)
                                                {
                                                    CommonLookupDTO audioDataObject;
                                                    LookupValuesDTO lookupAudioValuesDTO = lookupValuesDTOList.Where(m => m.LookupValue == "AUDIO").FirstOrDefault();
                                                    audioDataObject = new CommonLookupDTO(Convert.ToString(lookupAudioValuesDTO.LookupValueId), lookupAudioValuesDTO.LookupValue);
                                                    lookupDTO.Items.Add(audioDataObject);
                                                    audio = true;
                                                }
                                            }
                                            if (mediaLookupValueDTO.LookupValue == "VIDEO" && mediaDto.TypeId == mediaLookupValueDTO.LookupValueId)
                                            {
                                                if (!video)
                                                {
                                                    CommonLookupDTO videoDataObject;
                                                    LookupValuesDTO lookupVideoValuesDTO = lookupValuesDTOList.Where(m => m.LookupValue == "VIDEO").FirstOrDefault();
                                                    videoDataObject = new CommonLookupDTO(Convert.ToString(lookupVideoValuesDTO.LookupValueId), lookupVideoValuesDTO.LookupValue);
                                                    lookupDTO.Items.Add(videoDataObject);
                                                    video = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TICKERCONTENT")
                    {
                        if (dependentDropdownName.ToUpper().ToString() == "TICKERCONTENT")
                        {
                            lookupDTO.DropdownName = dependentDropdownName;
                        }
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        TickerListBL tickerListBL = new TickerListBL(executionContext);
                        List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<TickerDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchByParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.IS_ACTIVE, isActive));
                        List<TickerDTO> tickerDTOList = tickerListBL.GetTickerDTOList(searchByParameters);
                        if (tickerDTOList != null && tickerDTOList.Any())
                        {
                            foreach (var tickerValueDTO in tickerDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(tickerValueDTO.TickerId), tickerValueDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PATTERNCONTENT" || (entityName.ToUpper().ToString() == "SETUPSCREENDESIGNCONTENTMAP" && dependentDropdownName.ToUpper().ToString() == "PATTERNCONTENT"))
                    {
                        if (dependentDropdownName.ToUpper().ToString() == "PATTERNCONTENT")
                        {
                            lookupDTO.DropdownName = dependentDropdownName;
                        }
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext);
                        List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.IS_ACTIVE, "1"));
                        searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<SignagePatternDTO> signagePatternDTOList = signagePatternListBL.GetSignagePatternDTOList(searchParameters);
                        if (signagePatternDTOList != null && signagePatternDTOList.Any())
                        {
                            foreach (var dsLookupValueDTO in signagePatternDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(dsLookupValueDTO.SignagePatternId), dsLookupValueDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MEDIA_LIST")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        LoadMediaList();

                        if (mediaDTOList != null && mediaDTOList.Any())
                        {
                            lookupDTO.DropdownName = "MEDIA_LIST";
                            foreach (var mediaDTO in mediaDTOList)
                            {
                                if (string.Equals(GetMediaType(mediaDTO.TypeId), "IMAGE"))
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(mediaDTO.MediaId), mediaDTO.Name);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                    }
                    else if ((dropdownName.ToUpper().ToString() == "IMAGECONTENT" || dropdownName.ToUpper().ToString() == "VIDEOCONTENT" || dropdownName.ToUpper().ToString() == "AUDIOCONTENT"))
                    {
                        LoadMediaList();
                        LoadLookupValues("MEDIA_TYPE");
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        if (mediaDTOList != null && (dependentDropdownName.ToUpper().ToString() == "IMAGECONTENT" || dropdownName.ToUpper().ToString() == "IMAGECONTENT"))
                        {
                            foreach (var mediaDTO in mediaDTOList)
                            {
                                lookupDTO.DropdownName = "IMAGECONTENT";
                                if (string.Equals(GetMediaType(mediaDTO.TypeId), "IMAGE"))
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(mediaDTO.MediaId), mediaDTO.Name);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                        else if (mediaDTOList != null && (dependentDropdownName.ToUpper().ToString() == "VIDEOCONTENT" || dropdownName.ToUpper().ToString() == "VIDEOCONTENT"))
                        {
                            foreach (var mediaDTO in mediaDTOList)
                            {
                                lookupDTO.DropdownName = "VIDEOCONTENT";
                                if (string.Equals(GetMediaType(mediaDTO.TypeId), "VIDEO"))
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(mediaDTO.MediaId), mediaDTO.Name);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                        else if (mediaDTOList != null && (dependentDropdownName.ToUpper().ToString() == "AUDIOCONTENT" || dropdownName.ToUpper().ToString() == "AUDIOCONTENT"))
                        {
                            foreach (var mediaDTO in mediaDTOList)
                            {
                                lookupDTO.DropdownName = "AUDIOCONTENT";
                                if (string.Equals(GetMediaType(mediaDTO.TypeId), "AUDIO"))
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(mediaDTO.MediaId), mediaDTO.Name);
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
                        searchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.IS_ACTIVE, isActive));
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
                        searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.IS_ACTIVE, isActive));
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
                    else if (dropdownName.ToUpper().ToString() == "PANEL_TYPE_ACTIVE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        DisplayPanelListBL displayPanelListBL = new DisplayPanelListBL(executionContext);
                        List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.IS_ACTIVE, isActive));
                        List<DisplayPanelDTO> displayPanelDTOList = displayPanelListBL.GetDisplayPanelDTOList(searchParameters);
                        if (displayPanelDTOList != null && displayPanelDTOList.Any())
                        {
                            foreach (var panelValueDTO in displayPanelDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(panelValueDTO.PanelId), panelValueDTO.PanelName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PANEL_TYPE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        DisplayPanelListBL displayPanelListBL = new DisplayPanelListBL(executionContext);
                        List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        if (entityName.ToUpper().ToString() != "SIGNAGESCHEDULE")
                        {
                            searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.IS_ACTIVE, isActive));
                        }
                        List<DisplayPanelDTO> displayPanelDTOList = displayPanelListBL.GetDisplayPanelDTOList(searchParameters);
                        if (displayPanelDTOList != null && displayPanelDTOList.Any())
                        {
                            displayPanelDTOList = displayPanelDTOList.OrderBy(x => x.PanelName).ToList();
                            foreach (var panelValueDTO in displayPanelDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(panelValueDTO.PanelId), panelValueDTO.PanelName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "THEMES")
                    {
                        List<LookupValuesDTO> lookupValuesDTOList = LoadLookupValues("THEME_TYPE");
                        List<ThemeDTO> themeDTOList = new List<ThemeDTO>();
                        foreach (var lookup in lookupValuesDTOList)
                        {
                            if (lookup.LookupValue == "Panel")
                            {
                                ThemeListBL themeListBL = new ThemeListBL(executionContext);
                                List<KeyValuePair<ThemeDTO.SearchByParameters, string>> themeSearchParams = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                                themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_ID, lookup.LookupValueId.ToString()));
                                themeSearchParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.IS_ACTIVE, "1"));
                                themeDTOList = themeListBL.GetThemeDTOList(themeSearchParams,true,true);
                            }
                        }
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        if (themeDTOList != null && themeDTOList.Any())
                        {
                            themeDTOList = themeDTOList.OrderBy(x => x.Name).ToList();
                            foreach (var themeValueDTO in themeDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(themeValueDTO.Id), themeValueDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "THEME_TYPE")
                    {
                        string themeTypeList = "Panel";
                        LoadLookupValues(dropdownName);
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (var themeTypeValueDTO in lookupValuesDTOList)
                            {
                                if (themeTypeList.Contains(themeTypeValueDTO.LookupValue))
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(themeTypeValueDTO.LookupValueId), themeTypeValueDTO.LookupValue);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
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
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(recurFrequecy.Key), recurFrequecy.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DAYLOOKUP")
                    {
                        List<KeyValuePair<string, string>> weekDays = new List<KeyValuePair<string, string>>();
                        weekDays.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
                        weekDays.Add(new KeyValuePair<string, string>("1", "Sunday"));
                        weekDays.Add(new KeyValuePair<string, string>("2", "Monday"));
                        weekDays.Add(new KeyValuePair<string, string>("3", "Tuesday"));
                        weekDays.Add(new KeyValuePair<string, string>("4", "Wednesday"));
                        weekDays.Add(new KeyValuePair<string, string>("5", "Thursday"));
                        weekDays.Add(new KeyValuePair<string, string>("6", "Friday"));
                        weekDays.Add(new KeyValuePair<string, string>("7", "Saturday"));
                        foreach (var week in weekDays)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(week.Key), week.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "SCREEN RESOLUTION HORIZONTAL" || dropdownName.ToUpper().ToString() == "SCREEN RESOLUTION VERTICAL")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        LoadLookupValues(dropdownName);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            List<LookupValuesDTO> filterLookups = lookupValuesDTOList.OrderBy(x => x.LookupValue).ToList();
                            foreach (var lookupValueDTO in filterLookups)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValueDTO.LookupValueId), lookupValueDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "INDENTATION")
                    {
                        // INDENTATION
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        LoadLookupValues(dropdownName);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (var lookupValueDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValueDTO.LookupValueId), lookupValueDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);
                        LoadLookupValues(dropdownName);
                        if (lookupValuesDTOList != null)
                        {
                            foreach (var lookupValueDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValueDTO.LookupValueId), lookupValueDTO.LookupValue);
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
        /// Gets Media Type
        /// </summary>
        /// <param name="lookupValueId"></param>
        /// <returns></returns>
        private string GetMediaType(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            string returnValue = string.Empty;
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                foreach (var lookupValueDTO in lookupValuesDTOList)
                {
                    if (lookupValueDTO.LookupValueId == lookupValueId)
                    {
                        returnValue = lookupValueDTO.LookupValue;
                        break;
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Loads the lookupValues
        /// </summary>
        /// <param name="lookupName"></param>
        private List<LookupValuesDTO> LoadLookupValues(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList;
        }
        /// <summary>
        /// Loads the Media List
        /// </summary>
        private void LoadMediaList()
        {
            log.LogMethodEntry();
            MediaList mediaListBL = new MediaList(executionContext);
            List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> searchByParameters = new List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>>();
            searchByParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchByParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.IS_ACTIVE, "1"));
            mediaDTOList = mediaListBL.GetAllMedias(searchByParameters);
            log.LogMethodExit();
        }
    }
}

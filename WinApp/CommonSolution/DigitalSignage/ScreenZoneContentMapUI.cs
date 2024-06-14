/********************************************************************************************
 * Project Name - Screen Zone Content Map UI
 * Description  - User interface for screen zone content map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera          Created 
 *2.70.2      14-Aug-2019   Dakshakh            Added logger methods
 *2.80.0      17-Feb-2019   Deeksha             Modified to Make DigitalSignage module as
 *                                              read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// ScreenZoneContentMap UI
    /// </summary>
    public partial class ScreenZoneContentMapUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private int screenId = -1;
        private List<LookupValuesDTO> mediaTypeLookupValuesDTOList;
        private List<LookupValuesDTO> contentTypeLookupValuesDTOList;
        private List<KeyValuePair<int, string>> contentTypeList;
        private List<KeyValuePair<string, string>> tickerList;
        private List<KeyValuePair<string, string>> dSLookupList;
        private List<KeyValuePair<string, string>> imageList;
        private List<KeyValuePair<int, string>> backImageList;
        private List<KeyValuePair<string, string>> audioList;
        private List<KeyValuePair<string, string>> videoList;
        private List<KeyValuePair<string, string>> webUrlList;
        private List<KeyValuePair<string, string>> signagePatternList;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities"> Will hold environment</param>
        /// <param name="screenId"> Will hold screenId</param>
        public ScreenZoneContentMapUI(Utilities _Utilities, int screenId)
        {
            log.LogMethodEntry(_Utilities, screenId);
            InitializeComponent();
            utilities = _Utilities;
            this.screenId = screenId;
            utilities.setupDataGridProperties(ref dgvContentSetupGrid);
            utilities.setupDataGridProperties(ref dgvZones);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void ScreenZoneContentMapUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadTickerScrollDirection();
            LoadImageSize();
            LoadMediaType();
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            try
            {
                LoadTickerDTOList();
                LoadDSLookupDTOList();
                LoadMediaDTOList();
                LoadSignagePatternDTOList();
                LoadContentType();
                List<KeyValuePair<string, string>> defaultContentList = new List<KeyValuePair<string, string>>();
                defaultContentList.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
                defaultContentList.AddRange(tickerList);
                defaultContentList.AddRange(dSLookupList);
                defaultContentList.AddRange(imageList);
                defaultContentList.AddRange(audioList);
                defaultContentList.AddRange(videoList);
                defaultContentList.AddRange(webUrlList);
                defaultContentList.AddRange(signagePatternList);
                BindingSource bs = new BindingSource();
                bs.DataSource = defaultContentList;
                ContentId.DataSource = bs;
                ContentId.ValueMember = "Key";
                ContentId.DisplayMember = "Value";
                LoadScreenZoneDefSetupDTOList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            log.LogMethodExit();
        }

        private void LoadTickerScrollDirection()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INDENTATION"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> tikerScrollDirectionList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (tikerScrollDirectionList == null)
                {
                    tikerScrollDirectionList = new List<LookupValuesDTO>();
                }
                tikerScrollDirectionList.Insert(0, new LookupValuesDTO());
                tikerScrollDirectionList[0].LookupValueId = -1;
                tikerScrollDirectionList[0].Description = "<SELECT>";
                tickerScrollDirectionDataGridViewTextBoxColumn.DataSource = tikerScrollDirectionList;
                tickerScrollDirectionDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                tickerScrollDirectionDataGridViewTextBoxColumn.DisplayMember = "Description";
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadIndentation() Method with an Exception:", e);
            }
        }

        private void LoadImageSize()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "IMAGE_SIZE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> imageSizeList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (imageSizeList == null)
                {
                    imageSizeList = new List<LookupValuesDTO>();
                }
                imageSizeList.Insert(0, new LookupValuesDTO());
                imageSizeList[0].LookupValueId = -1;
                imageSizeList[0].LookupValue = "<SELECT>";
                imgSizeDataGridViewTextBoxColumn.DataSource = imageSizeList;
                imgSizeDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                imgSizeDataGridViewTextBoxColumn.DisplayMember = "LookupValue";

                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        private void LoadContentType()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CONTENT_TYPE"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            contentTypeLookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            contentTypeList = new List<KeyValuePair<int, string>>();
            contentTypeList.Add(new KeyValuePair<int, string>(-1, "<SELECT>"));
            if (contentTypeLookupValuesDTOList != null)
            {
                foreach (var lookupValueDTO in contentTypeLookupValuesDTOList)
                {
                    if ((String.Equals(lookupValueDTO.LookupValue, "IMAGE") && imageList.Count > 1) ||
                        (String.Equals(lookupValueDTO.LookupValue, "VIDEO") && videoList.Count > 1) ||
                        (String.Equals(lookupValueDTO.LookupValue, "AUDIO") && audioList.Count > 1) ||
                        (String.Equals(lookupValueDTO.LookupValue, "LOOKUP") && dSLookupList.Count > 1) ||
                        (String.Equals(lookupValueDTO.LookupValue, "PATTERN") && signagePatternList.Count > 1) ||
                        (String.Equals(lookupValueDTO.LookupValue, "TICKER") && tickerList.Count > 1) ||
                        (String.Equals(lookupValueDTO.LookupValue, "WEBSITE") && webUrlList.Count > 1))
                    {
                        contentTypeList.Add(new KeyValuePair<int, string>(lookupValueDTO.LookupValueId, lookupValueDTO.Description));
                    }
                }
            }
            BindingSource bs = new BindingSource();
            bs.DataSource = contentTypeList;
            ContentTypeId.DataSource = bs;
            ContentTypeId.ValueMember = "Key";
            ContentTypeId.DisplayMember = "Value";

            log.LogMethodExit();
        }

        private void LoadTickerDTOList()
        {
            log.LogMethodEntry();
            TickerListBL tickerListBL = new TickerListBL(machineUserContext);
            List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<TickerDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<TickerDTO> tickerDTOList = tickerListBL.GetTickerDTOList(searchByParameters);
            tickerList = new List<KeyValuePair<string, string>>();
            tickerList.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
            if (tickerDTOList != null)
            {
                foreach (var tickerDTO in tickerDTOList)
                {
                    tickerList.Add(new KeyValuePair<string, string>(tickerDTO.Guid, tickerDTO.Name));
                }
            }
            log.LogMethodExit();
        }

        private void LoadSignagePatternDTOList()
        {
            log.LogMethodEntry();
            SignagePatternListBL signagePatternListBL = new SignagePatternListBL(utilities.ExecutionContext);
            List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<SignagePatternDTO> signagePatternDTOList = signagePatternListBL.GetSignagePatternDTOList(searchByParameters);
            signagePatternList = new List<KeyValuePair<String, string>>();
            signagePatternList.Add(new KeyValuePair<String, string>("-1", "<SELECT>"));
            if (signagePatternDTOList != null)
            {
                foreach (var signagePatternDTO in signagePatternDTOList)
                {
                    signagePatternList.Add(new KeyValuePair<String, string>(signagePatternDTO.Guid, signagePatternDTO.Name));
                }
            }
            log.LogMethodExit();
        }

        private void LoadDSLookupDTOList()
        {
            log.LogMethodEntry();
            DSLookupListBL dSLookupListBL = new DSLookupListBL(machineUserContext);
            List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<DSLookupDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<DSLookupDTO> dSLookupDTOList = dSLookupListBL.GetDSLookupDTOList(searchByParameters);
            dSLookupList = new List<KeyValuePair<string, string>>();
            dSLookupList.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
            if (dSLookupDTOList != null)
            {
                foreach (var dSLookupDTO in dSLookupDTOList)
                {
                    dSLookupList.Add(new KeyValuePair<string, string>(dSLookupDTO.Guid, dSLookupDTO.DSLookupName));
                }
            }
            log.LogMethodExit();
        }

        private void LoadMediaType()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MEDIA_TYPE"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            mediaTypeLookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            log.LogMethodExit();
        }

        private string GetMediaType(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            string returnValue = string.Empty;
            if (mediaTypeLookupValuesDTOList != null)
            {
                foreach (var lookupValueDTO in mediaTypeLookupValuesDTOList)
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

        private string GetContentType(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            string returnValue = string.Empty;
            if (contentTypeLookupValuesDTOList != null)
            {
                foreach (var lookupValueDTO in contentTypeLookupValuesDTOList)
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

        private void LoadMediaDTOList()
        {
            log.LogMethodEntry();
            MediaList mediaListBL = new MediaList(machineUserContext);
            List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> searchByParameters = new List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>>();
            searchByParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.IS_ACTIVE, "1"));
            List<MediaDTO> mediaDTOList = mediaListBL.GetAllMedias(searchByParameters);
            imageList = new List<KeyValuePair<string, string>>();
            imageList.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
            backImageList = new List<KeyValuePair<int, string>>();
            backImageList.Add(new KeyValuePair<int, string>(-1, "<SELECT>"));
            videoList = new List<KeyValuePair<string, string>>();
            videoList.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
            audioList = new List<KeyValuePair<string, string>>();
            audioList.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
            webUrlList = new List<KeyValuePair<string, string>>();
            webUrlList.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
            if (mediaDTOList != null)
            {
                foreach (var mediaDTO in mediaDTOList)
                {
                    if (string.Equals(GetMediaType(mediaDTO.TypeId), "IMAGE"))
                    {
                        imageList.Add(new KeyValuePair<string, string>(mediaDTO.Guid, mediaDTO.Name));
                        backImageList.Add(new KeyValuePair<int, string>(mediaDTO.MediaId, mediaDTO.Name));
                    }
                    else if (string.Equals(GetMediaType(mediaDTO.TypeId), "AUDIO"))
                    {
                        audioList.Add(new KeyValuePair<string, string>(mediaDTO.Guid, mediaDTO.Name));
                    }
                    else if (string.Equals(GetMediaType(mediaDTO.TypeId), "VIDEO"))
                    {
                        videoList.Add(new KeyValuePair<string, string>(mediaDTO.Guid, mediaDTO.Name));
                    }
                    else if (string.Equals(GetMediaType(mediaDTO.TypeId), "WEBSITE", StringComparison.InvariantCultureIgnoreCase))
                    {
                        webUrlList.Add(new KeyValuePair<string, string>(mediaDTO.Guid, mediaDTO.Name));
                    }
                }
            }
            BindingSource bs = new BindingSource();
            bs.DataSource = backImageList;
            backImageDataGridViewTextBoxColumn.DataSource = bs;
            backImageDataGridViewTextBoxColumn.DisplayMember = "Value";
            backImageDataGridViewTextBoxColumn.ValueMember = "Key";
            log.LogMethodExit();
        }

        private void LoadScreenZoneDefSetupDTOList()
        {
            log.LogMethodEntry();
            ScreenZoneDefSetupList screenZoneDefSetupList = new ScreenZoneDefSetupList(machineUserContext);
            List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.SCREEN_ID, screenId.ToString()));
            List<ScreenZoneDefSetupDTO> screenZoneDefSetupDTOList = screenZoneDefSetupList.GetAllScreenZoneDefSetup(searchParameters);
            SortableBindingList<ScreenZoneDefSetupDTO> screenZoneDefSetupDTOSortableBindingList;
            if (screenZoneDefSetupDTOList != null)
            {
                dgvContentSetupGrid.AllowUserToAddRows = true;
                screenZoneDefSetupDTOSortableBindingList = new SortableBindingList<ScreenZoneDefSetupDTO>(screenZoneDefSetupDTOList);
            }
            else
            {
                dgvContentSetupGrid.AllowUserToAddRows = false;
                screenZoneDefSetupDTOSortableBindingList = new SortableBindingList<ScreenZoneDefSetupDTO>();
            }

            screenZoneDefSetupDTOBindingSource.DataSource = screenZoneDefSetupDTOSortableBindingList;
            log.LogMethodExit();
        }

        private void screenZoneDefSetupDTOBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            RefreshDgvContentSetupGrid();
            log.LogMethodExit();
        }

        private void RefreshDgvContentSetupGrid()
        {
            log.LogMethodEntry();
            SortableBindingList<ScreenZoneContentMapDTO> screenZoneContentMapDTOSortableBindingList = null;
            if (screenZoneDefSetupDTOBindingSource.Current != null && screenZoneDefSetupDTOBindingSource.Current is ScreenZoneDefSetupDTO)
            {
                ScreenZoneDefSetupDTO screenZoneDefSetupDTO = screenZoneDefSetupDTOBindingSource.Current as ScreenZoneDefSetupDTO;
                if (screenZoneDefSetupDTO.ZoneId != -1)
                {
                    ScreenZoneContentMapList screenZoneContentMapList = new ScreenZoneContentMapList(machineUserContext);
                    List<KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>(ScreenZoneContentMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>(ScreenZoneContentMapDTO.SearchByParameters.ZONE_ID, screenZoneDefSetupDTO.ZoneId.ToString()));
                    List<ScreenZoneContentMapDTO> screenZoneContentMapDTOList = screenZoneContentMapList.GetAllScreenZoneContentMap(searchParameters);
                    if (screenZoneContentMapDTOList != null)
                    {
                        screenZoneContentMapDTOSortableBindingList = new SortableBindingList<ScreenZoneContentMapDTO>(screenZoneContentMapDTOList);
                    }
                }
            }
            if (screenZoneContentMapDTOSortableBindingList == null)
            {
                screenZoneContentMapDTOSortableBindingList = new SortableBindingList<ScreenZoneContentMapDTO>();
            }
            screenZoneContentMapDTOBindingSource.DataSource = screenZoneContentMapDTOSortableBindingList;
            log.LogMethodExit();
        }

        private string ValidateScreenZoneContentMapDTO(ScreenZoneContentMapDTO screenZoneContentMapDTO)
        {
            log.LogMethodEntry(screenZoneContentMapDTO);
            string message = string.Empty;
            if (screenZoneContentMapDTO.ContentTypeId == -1)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", ContentTypeId.HeaderText);
            }
            if (screenZoneContentMapDTO.ContentGuid.Equals("-1"))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", ContentId.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void UpdateScreenZoneContentMapDTO(ScreenZoneContentMapDTO screenZoneContentMapDTO)
        {
            log.LogMethodEntry(screenZoneContentMapDTO);
            if (screenZoneContentMapDTO.ZoneId == -1)
            {
                ScreenZoneDefSetupDTO screenZoneDefSetupDTO = screenZoneDefSetupDTOBindingSource.Current as ScreenZoneDefSetupDTO;
                if (screenZoneDefSetupDTO != null)
                {
                    screenZoneContentMapDTO.ZoneId = screenZoneDefSetupDTO.ZoneId;
                }
            }
            screenZoneContentMapDTO.ContentType = GetContentType(screenZoneContentMapDTO.ContentTypeId);
            if (!(string.Equals(screenZoneContentMapDTO.ContentType, "VIDEO") ||
                 string.Equals(screenZoneContentMapDTO.ContentType, "AUDIO")))
            {
                screenZoneContentMapDTO.VideoRepeat = "N";
            }
            if (!string.Equals(screenZoneContentMapDTO.ContentType, "TICKER"))
            {
                screenZoneContentMapDTO.TickerScrollDirection = -1;
                screenZoneContentMapDTO.TickerRefreshSecs = -1;
                screenZoneContentMapDTO.TickerSpeed = -1;
            }
            if (!string.Equals(screenZoneContentMapDTO.ContentType, "IMAGE"))
            {
                screenZoneContentMapDTO.ImgSize = -1;
                screenZoneContentMapDTO.ImgAlignment = string.Empty;
            }
            if (!((string.Equals(screenZoneContentMapDTO.ContentType, "IMAGE") ||
                (string.Equals(screenZoneContentMapDTO.ContentType, "WEBSITE"))) ||
                string.Equals(screenZoneContentMapDTO.ContentType, "PATTERN")))
            {
                screenZoneContentMapDTO.ImgRefreshSecs = -1;
            }
            if (!string.Equals(screenZoneContentMapDTO.ContentType, "LOOKUP"))
            {
                screenZoneContentMapDTO.LookupRefreshSecs = -1;
                screenZoneContentMapDTO.LookupHeaderDisplay = "N";
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvContentSetupGrid.EndEdit();
                SortableBindingList<ScreenZoneContentMapDTO> screenZoneContentMapDTOSortableList = (SortableBindingList<ScreenZoneContentMapDTO>)screenZoneContentMapDTOBindingSource.DataSource;
                string message;
                ScreenZoneContentMap screenZoneContentMapBL;
                bool error = false;
                if (screenZoneContentMapDTOSortableList != null)
                {
                    for (int i = 0; i < screenZoneContentMapDTOSortableList.Count; i++)
                    {
                        if (screenZoneContentMapDTOSortableList[i].IsChanged)
                        {
                            message = ValidateScreenZoneContentMapDTO(screenZoneContentMapDTOSortableList[i]);
                            if (string.IsNullOrEmpty(message))
                            {
                                UpdateScreenZoneContentMapDTO(screenZoneContentMapDTOSortableList[i]);
                                try
                                {
                                    screenZoneContentMapBL = new ScreenZoneContentMap(machineUserContext, screenZoneContentMapDTOSortableList[i]);
                                    screenZoneContentMapBL.Save(null);
                                }
                                catch (Exception)
                                {
                                    error = true;
                                    log.Error("Error while saving screenZoneContentMap.");
                                    dgvContentSetupGrid.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                    break;
                                }
                            }
                            else
                            {
                                error = true;
                                dgvContentSetupGrid.Rows[i].Selected = true;
                                MessageBox.Show(message);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                }
                if (!error)
                {
                    RefreshDgvContentSetupGrid();
                }
                else
                {
                    dgvContentSetupGrid.Update();
                    dgvContentSetupGrid.Refresh();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private bool ValidateColor(string stringColor)
        {
            log.LogMethodEntry(stringColor);
            bool valid = false;
            try
            {
                Color color = System.Drawing.ColorTranslator.FromHtml(stringColor);
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                valid = string.Equals(converter.ConvertToString(color), stringColor);
            }
            catch (Exception)
            {
                log.LogMethodExit(false);
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private void btnRefersh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.dgvContentSetupGrid.SelectedRows.Count <= 0 && this.dgvContentSetupGrid.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit("Ends-screenZoneContentMapDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                bool refreshFromDB = false;
                if (this.dgvContentSetupGrid.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvContentSetupGrid.SelectedCells)
                    {
                        dgvContentSetupGrid.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow screenZoneContentMapRow in this.dgvContentSetupGrid.SelectedRows)
                {
                    if (Convert.ToInt32(screenZoneContentMapRow.Cells[0].Value.ToString()) < 0)
                    {
                        dgvContentSetupGrid.Rows.RemoveAt(screenZoneContentMapRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            refreshFromDB = true;
                            BindingSource screenZoneContentMapDTOListDTOBS = (BindingSource)dgvContentSetupGrid.DataSource;
                            var screenZoneContentMapDTOList = (SortableBindingList<ScreenZoneContentMapDTO>)screenZoneContentMapDTOListDTOBS.DataSource;
                            ScreenZoneContentMapDTO screenZoneContentMapDTO = screenZoneContentMapDTOList[screenZoneContentMapRow.Index];
                            screenZoneContentMapDTO.IsActive = false;
                            ScreenZoneContentMap screenZoneContentMap = new ScreenZoneContentMap(machineUserContext, screenZoneContentMapDTO);
                            screenZoneContentMap.Save(null);
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                if (refreshFromDB)
                {
                    RefreshDgvContentSetupGrid();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void dgvZones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex > -1 && e.ColumnIndex == dgvZones.Columns["GetZone"].Index)
            {
                ViewZone viewZone = new ViewZone(Convert.ToInt32(dgvZones.Rows[e.RowIndex].Cells["screenIdDataGridViewTextBoxColumn"].Value), dgvZones.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString(), Convert.ToInt32(dgvZones.Rows[e.RowIndex].Cells["TopLeft"].Value), Convert.ToInt32(dgvZones.Rows[e.RowIndex].Cells["BottomRight"].Value));
                viewZone.ShowDialog();
            }
            log.LogMethodExit();
        }

        private void UpdateDgvContentSetupGrid()
        {
            log.LogMethodEntry();
            SortableBindingList<ScreenZoneContentMapDTO> screenZoneContentMapDTOSortableList = null;
            if (screenZoneContentMapDTOBindingSource.DataSource is SortableBindingList<ScreenZoneContentMapDTO>)
            {
                screenZoneContentMapDTOSortableList = (SortableBindingList<ScreenZoneContentMapDTO>)screenZoneContentMapDTOBindingSource.DataSource;
            }
            if (screenZoneContentMapDTOSortableList != null)
            {
                for (int i = 0; i < screenZoneContentMapDTOSortableList.Count; i++)
                {
                    UpdateDgvContentSetupGridRow(dgvContentSetupGrid.Rows[i], screenZoneContentMapDTOSortableList[i]);
                }
            }
            log.LogMethodExit();
        }

        private void UpdateDgvContentSetupGridRow(DataGridViewRow row, ScreenZoneContentMapDTO screenZoneContentMapDTO)
        {
            log.LogMethodEntry(row, screenZoneContentMapDTO);
            if (row != null && screenZoneContentMapDTO != null)
            {
                DataGridViewComboBoxCell cell = row.Cells[ContentId.Index] as DataGridViewComboBoxCell;
                if (cell != null)
                {
                    cell.DataSource = GetContentDataSource(screenZoneContentMapDTO.ContentTypeId);
                    cell.ValueMember = "Key";
                    cell.DisplayMember = "Value";
                }
            }
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> GetContentDataSource(int contentTypeId)
        {
            log.LogMethodEntry(contentTypeId);
            List<KeyValuePair<string, string>> dataSource = null;
            switch (GetContentType(contentTypeId))
            {
                case "IMAGE":
                    {
                        dataSource = imageList;
                        break;
                    }
                case "VIDEO":
                    {
                        dataSource = videoList;
                        break;
                    }
                case "AUDIO":
                    {
                        dataSource = audioList;
                        break;
                    }
                case "LOOKUP":
                    {
                        dataSource = dSLookupList;
                        break;
                    }
                case "TICKER":
                    {
                        dataSource = tickerList;
                        break;
                    }
                case "PATTERN":
                    {
                        dataSource = signagePatternList;
                        break;
                    }
                case "WEBSITE":
                    {
                        dataSource = webUrlList;
                        break;
                    }
            }
            if (dataSource == null)
            {
                dataSource = new List<KeyValuePair<string, string>>();
                dataSource.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
            }
            BindingSource bs = new BindingSource();
            bs.DataSource = dataSource;
            log.LogMethodExit(dataSource);
            return dataSource;
        }

        private void dgvContentSetupGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            bool handled = false;
            if (e.ColumnIndex == ContentId.Index)
            {
                SortableBindingList<ScreenZoneContentMapDTO> screenZoneContentMapDTOSortableList = null;
                if (screenZoneContentMapDTOBindingSource.DataSource is SortableBindingList<ScreenZoneContentMapDTO>)
                {
                    screenZoneContentMapDTOSortableList = (SortableBindingList<ScreenZoneContentMapDTO>)screenZoneContentMapDTOBindingSource.DataSource;
                }
                if (screenZoneContentMapDTOSortableList != null && screenZoneContentMapDTOSortableList.Count > 0)
                {
                    DataGridViewComboBoxCell cell = dgvContentSetupGrid[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
                    if (cell != null)
                    {
                        cell.Value = -1;
                        handled = true;
                    }
                }
            }
            if (!handled)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvContentSetupGrid.Columns[e.ColumnIndex].HeaderText + ": " + (e.Exception == null ? "" : e.Exception.Message));
            }
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvContentSetupGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgvContentSetupGrid.CurrentCell.ColumnIndex == ContentTypeId.Index)
            {
                dgvContentSetupGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            log.LogMethodExit();
        }

        private void dgvContentSetupGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == ContentTypeId.Index)
            {
                SortableBindingList<ScreenZoneContentMapDTO> screenZoneContentMapDTOSortableList = null;
                if (screenZoneContentMapDTOBindingSource.DataSource is SortableBindingList<ScreenZoneContentMapDTO>)
                {
                    screenZoneContentMapDTOSortableList = (SortableBindingList<ScreenZoneContentMapDTO>)screenZoneContentMapDTOBindingSource.DataSource;
                }
                if (screenZoneContentMapDTOSortableList != null)
                {
                    ScreenZoneContentMapDTO screenZoneContentMapDTO = screenZoneContentMapDTOSortableList[e.RowIndex];
                    DataGridViewComboBoxCell cell = dgvContentSetupGrid.CurrentRow.Cells[ContentId.Index] as DataGridViewComboBoxCell;
                    if (cell != null)
                    {
                        cell.Value = -1;
                        cell.DataSource = GetContentDataSource(screenZoneContentMapDTO.ContentTypeId);
                        cell.ValueMember = "Key";
                        cell.DisplayMember = "Value";
                    }
                    UpdateContentTypeIdChanges(dgvContentSetupGrid.CurrentRow, screenZoneContentMapDTO.ContentTypeId);
                }

            }
            log.LogMethodExit();
        }

        private void UpdateContentTypeIdChanges(DataGridViewRow row, int contentTypeId)
        {
            log.LogMethodEntry(row, contentTypeId);
            row.Cells[imgSizeDataGridViewTextBoxColumn.Index].ReadOnly = true;
            row.Cells[imgAlignmentDataGridViewTextBoxColumn.Index].ReadOnly = true;
            row.Cells[imgRefreshSecsDataGridViewTextBoxColumn.Index].ReadOnly = true;
            row.Cells[videoRepeatDataGridViewTextBoxColumn.Index].ReadOnly = true;
            row.Cells[lookupRefreshSecsDataGridViewTextBoxColumn.Index].ReadOnly = true;
            row.Cells[lookupHeaderDisplayDataGridViewTextBoxColumn.Index].ReadOnly = true;
            row.Cells[tickerScrollDirectionDataGridViewTextBoxColumn.Index].ReadOnly = true;
            row.Cells[tickerSpeedDataGridViewTextBoxColumn.Index].ReadOnly = true;
            row.Cells[tickerRefreshSecsDataGridViewTextBoxColumn.Index].ReadOnly = true;
            switch (GetContentType(contentTypeId))
            {
                case "IMAGE":
                    {
                        row.Cells[imgSizeDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        row.Cells[imgAlignmentDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        row.Cells[imgRefreshSecsDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        break;
                    }
                case "PATTERN":
                    {
                        row.Cells[imgRefreshSecsDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        break;
                    }
                case "VIDEO":
                case "AUDIO":
                    {
                        row.Cells[videoRepeatDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        break;
                    }
                case "LOOKUP":
                    {
                        row.Cells[lookupRefreshSecsDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        row.Cells[lookupHeaderDisplayDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        break;
                    }
                case "TICKER":
                    {
                        row.Cells[tickerScrollDirectionDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        row.Cells[tickerSpeedDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        row.Cells[tickerRefreshSecsDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        break;
                    }
                case "WEBSITE":
                    {
                        row.Cells[imgRefreshSecsDataGridViewTextBoxColumn.Index].ReadOnly = false;
                        break;
                    }
            }
            log.LogMethodExit();
        }

        private void dgvContentSetupGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            string backColor = string.Empty;
            string textColor = string.Empty;
            string fontname = string.Empty;
            if (e.ColumnIndex == backColorDataGridViewTextBoxColumn.Index || e.ColumnIndex == borderColorDataGridViewTextBoxColumn.Index)
            {
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                    textColor = converter.ConvertToString(cd.Color);
                    dgvContentSetupGrid.CurrentCell.Value = textColor;
                    dgvContentSetupGrid.CurrentCell = dgvContentSetupGrid[e.ColumnIndex + 5, e.RowIndex];
                }
            }
            log.LogMethodExit();
        }

        private void dgvContentSetupGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry();
            SortableBindingList<ScreenZoneContentMapDTO> screenZoneContentMapDTOSortableList = null;
            if (screenZoneContentMapDTOBindingSource.DataSource is SortableBindingList<ScreenZoneContentMapDTO>)
            {
                screenZoneContentMapDTOSortableList = (SortableBindingList<ScreenZoneContentMapDTO>)screenZoneContentMapDTOBindingSource.DataSource;
            }
            if (screenZoneContentMapDTOSortableList != null)
            {
                for (int i = 0; i < screenZoneContentMapDTOSortableList.Count; i++)
                {
                    UpdateContentTypeIdChanges(dgvContentSetupGrid.Rows[i], screenZoneContentMapDTOSortableList[i].ContentTypeId);
                    UpdateDgvContentSetupGridRow(dgvContentSetupGrid.Rows[i], screenZoneContentMapDTOSortableList[i]);
                }
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvZones.AllowUserToAddRows = true;
                dgvZones.ReadOnly = false;
                dgvContentSetupGrid.AllowUserToAddRows = true;
                dgvContentSetupGrid.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvZones.AllowUserToAddRows = false;
                dgvZones.ReadOnly = true;
                dgvContentSetupGrid.AllowUserToAddRows = false;
                dgvContentSetupGrid.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }

    }
}

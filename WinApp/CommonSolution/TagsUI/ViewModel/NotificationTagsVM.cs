/********************************************************************************************
 * Project Name - TagsUI
 * Description  - NotificationTagsVM 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120       04-Mar-2021   Girish Kundar          Created - Is Radian change
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Tags;

namespace Semnox.Parafait.TagsUI
{
    /// <summary>
    /// NotificationTagStatusDTO
    /// </summary>
    public class NotificationTagStatusDTO
    {
        private string key;
        private string value;
        public string Key { get { return key; } set { key = value; } }
        public string Value { get { return value; } set { value = value; } }

        /// <summary>
        /// NotificationTagStatusDTO
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public NotificationTagStatusDTO(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
    /// <summary>
    /// NotificationTagFieldsDTO
    /// </summary>
    public class NotificationTagFieldsDTO
    {
        private string key;
        private string value;
        public string Key { get { return key; } set { key = value; } }
        public string Value { get { return value; } set { value = value; } }

        /// <summary>
        /// NotificationTagStatusDTO
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public NotificationTagFieldsDTO(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    /// <summary>
    /// SearchDTO
    /// </summary>
    public class SearchDTO
    {
        private string key;
        private string value;
        public string Key { get { return key; } set { key = value; } }
        public string Value { get { return value; } set { value = value; } }

        /// <summary>
        /// SearchDTO
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public SearchDTO(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
    /// <summary>
    /// StatusCode
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// Failure
        /// </summary>
        Failure
    }
    /// <summary>
    /// NotificationTagsVM
    /// </summary>
    public class NotificationTagsVM : BaseWindowViewModel
    {
        #region Members
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<NotificationTagStatusDTO> tagStatus;
        private ObservableCollection<NotificationTagFieldsDTO> tagFields;
        private ObservableCollection<SearchDTO> tagSearchFields;
        private ObservableCollection<string> tagListFields;
        private TagCommandDTO selectedCommand;
        private NotificationTagStatusDTO selectedTagStatus;
        private NotificationTagFieldsDTO selectedFieldName;
        private SearchDTO selectedSearchName;
        private string selectedFieldListName;
        private string selectedTextValue;
        private StatusCode statusCode;
        private string message;
        private bool dropDownVisible;
        private bool textBoxDownVisible;
        private NotificationTagsView notificationTagsView;
        private ObservableCollection<NotificationTagViewDTO> notificationTagViewDTOList;
        private NotificationTagViewDTO selectedNotificationTagViewDTO;
        private DisplayTagsVM displayTagsVM;
        private CustomDataGridVM customDataGridVM;
        private ObservableCollection<NotificationTagViewDTO> gridSource;
        private List<NotificationTagViewDTO> gridSourceBackUp;
        private Dictionary<string, CustomDataGridColumnElement> dataEntryElements;
        List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters;
        private ICommand searchCommand;
        private ICommand clearCommand;
        private ICommand closeCommand;
        private ICommand saveCommand;
        private ICommand refreshCommand;
        private ICommand performCommand;
        private ICommand rowSectionCommand;
        private ICommand navigationClickCommand;
        private ICommand selectionChangedCmd;
        private ICommand selectionChangedCBCmd;
        private ICommand loadedCommand;
        // Lables 
        private string labelStatus;
        private string labelFields;
        private string moduleName;
        #endregion Members
        #region Properties
        /// <summary>
        /// CustomDataGridVM
        /// </summary>
        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                return customDataGridVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref customDataGridVM, value);
                log.LogMethodExit(customDataGridVM);
            }

        }

        /// <summary>
        /// DisplayTagsVM
        /// </summary>
        public DisplayTagsVM DisplayTagsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTagsVM);
                return displayTagsVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTagsVM, value);
                log.LogMethodExit(displayTagsVM);
            }
        }
        public System.Windows.Visibility AddUserButtonVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dropDownVisible);
                if (dropDownVisible)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }

        public System.Windows.Visibility AddUserTextBoxVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(textBoxDownVisible);
                if (textBoxDownVisible)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }
        /// <summary>
        /// GridSource
        /// </summary>
        public ObservableCollection<NotificationTagViewDTO> GridSource
        {
            get
            {
                return gridSource;
            }
            set
            {
                gridSource = value;
            }
        }

        /// <summary>
        /// DataEntryElements
        /// </summary>
        public Dictionary<string, CustomDataGridColumnElement> DataEntryElements
        {
            get
            {
                return dataEntryElements;
            }
        }
        /// <summary>
        /// labelStatus
        /// </summary>
        public string LabelStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(labelStatus);
                return labelStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref labelStatus, value);
            }
        }

        /// <summary>
        /// labelStatus
        /// </summary>
        public ObservableCollection<NotificationTagViewDTO> NotificationTagViewDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(notificationTagViewDTOList);
                return notificationTagViewDTOList;
            }
            set
            {
                log.LogMethodEntry(value);
                notificationTagViewDTOList = value;
            }
        }

        /// <summary>
        /// labelStatus
        /// </summary>
        public string LabelFields
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(labelFields);
                return labelFields;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref labelFields, value);
            }
        }

        /// <summary>
        /// ModuleName
        /// </summary>
        public string ModuleName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(moduleName);
                return moduleName;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref moduleName, value);
                }
            }
        }

        /// <summary>
        /// SelectedNotificationTagViewDTO
        /// </summary>
        public NotificationTagViewDTO SelectedNotificationTagViewDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedNotificationTagViewDTO);
                return selectedNotificationTagViewDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref selectedNotificationTagViewDTO, value);
                }
            }
        }
        /// <summary>
        /// navigationClickCommand
        /// </summary>
        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(navigationClickCommand);
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                navigationClickCommand = value;
            }
        }

        /// <summary>
        /// SearchCommand
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchCommand);
                return searchCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                searchCommand = value;
            }
        }

        /// <summary>
        /// SearchCommand   
        /// </summary>
        public ICommand RowSectionCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rowSectionCommand);
                return rowSectionCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                rowSectionCommand = value;
            }
        }

        /// <summary>
        /// ClearCommand
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(clearCommand);
                return clearCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                clearCommand = value;
            }
        }

        /// <summary>
        /// CloseCommand
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(closeCommand);
                return closeCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                closeCommand = value;
            }
        }

        /// <summary>
        /// SaveCommand
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(saveCommand);
                return saveCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                saveCommand = value;
            }
        }

        /// <summary>
        /// SaveCommand
        /// </summary>
        public ICommand PerformCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(performCommand);
                return performCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                performCommand = value;
            }
        }

        /// <summary>
        /// RefreshCommand
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(refreshCommand);
                return refreshCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                refreshCommand = value;
            }
        }

        /// <summary>
        /// TagStatus
        /// </summary>
        public ObservableCollection<NotificationTagStatusDTO> TagStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tagStatus);
                return tagStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref tagStatus, value);
            }
        }
        /// <summary>
        /// TagStatus
        /// </summary>
        public string SelectedTextValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedTextValue);
                return selectedTextValue;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedTextValue, value);
            }
        }

        /// <summary>
        /// StatusCode
        /// </summary>
        public StatusCode StatusCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(statusCode);
                return statusCode;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref statusCode, value);
                log.LogMethodEntry(statusCode);
            }
        }
        /// <summary>
        /// selectedTagStatus
        /// </summary>
        public NotificationTagStatusDTO SelectedTagStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedTagStatus);
                return selectedTagStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedTagStatus, value);
            }
        }
        /// <summary>
        /// selectedTagStatus
        /// </summary>
        public SearchDTO SelectedSearchName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedSearchName);
                return selectedSearchName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedSearchName, value);
            }
        }
        /// <summary>
        /// selectedTagStatus
        /// </summary>
        public string SelectedFieldListName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedFieldListName);
                return selectedFieldListName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedFieldListName, value);
            }
        }

        /// <summary>
        /// selectedFieldName
        /// </summary>
        public NotificationTagFieldsDTO SelectedFieldName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedFieldName);
                return selectedFieldName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedFieldName, value);


            }
        }

        /// <summary>
        /// SuccessMessage
        /// </summary>
        public string SuccessMessage
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(message);
                return message;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref message, value);
            }
        }
        /// <summary>
        /// tagFields
        /// </summary>
        public ObservableCollection<NotificationTagFieldsDTO> TagFields
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tagFields);
                return tagFields;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref tagFields, value);
            }
        }
        /// <summary>
        /// tagFields
        /// </summary>
        public ObservableCollection<SearchDTO> TagSearchFields
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tagSearchFields);
                return tagSearchFields;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref tagSearchFields, value);
            }
        }

        /// <summary>
        /// tagFields
        /// </summary>
        public ObservableCollection<string> TagListFields
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tagListFields);
                return tagListFields;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref tagListFields, value);
            }
        }

        /// <summary>
        /// navigationClickCommand
        /// </summary>
        public ICommand SelectionChangedCmd
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(selectionChangedCmd);
                return selectionChangedCmd;
            }
            set
            {
                log.LogMethodEntry(value);
                selectionChangedCmd = value;
            }
        }

        /// <summary>
        /// navigationClickCommand
        /// </summary>
        public ICommand SelectionChangedCBCmd
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(selectionChangedCBCmd);
                return selectionChangedCBCmd;
            }
            set
            {
                log.LogMethodEntry(value);
                selectionChangedCBCmd = value;
            }
        }
        #endregion Properties
        #region Constructor
        /// <summary>
        /// NotificationTagsVM
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="SearchParameters"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public NotificationTagsVM(ExecutionContext executionContext, List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>
                                  SearchParameters, NotificationTagViewDTO.SearchByParameters key, string value)
        {
            log.LogMethodEntry(SearchParameters);
            this.ExecutionContext = executionContext;
            this.SearchParameters = SearchParameters;
            LoadNotificationStatus();
            LoadFields();
            LoadLables();
            SetDisplayTagsVM();
            dataEntryElements = new Dictionary<string, CustomDataGridColumnElement>();
            dataEntryElements.Add("IsInStorage", new CustomDataGridColumnElement() { Heading = "  Storage  ", Type = DataEntryType.CheckBox, IsReadOnly = false  });
            dataEntryElements.Add("TagNumber", new CustomDataGridColumnElement() { Heading = "  Tag Number  ", Type = DataEntryType.TextBlock, IsReadOnly = true  });
            dataEntryElements.Add("LastCommunicatedOn", new CustomDataGridColumnElement() { Heading = "  Last Communicated On ", Type = DataEntryType.TextBlock, IsReadOnly = true   });
            dataEntryElements.Add("DeviceStatus", new CustomDataGridColumnElement() { Heading = "  Device Status ", Type = DataEntryType.TextBlock, IsReadOnly = true   });
            dataEntryElements.Add("PingStatus", new CustomDataGridColumnElement() { Heading = "   Ping Status  ", Type = DataEntryType.CheckBox, IsReadOnly = true });
            dataEntryElements.Add("BatteryStatus", new CustomDataGridColumnElement() { Heading = "  Battery Status  ", Type = DataEntryType.TextBlock, IsReadOnly = true  });
            dataEntryElements.Add("SignalStrength", new CustomDataGridColumnElement() { Heading = "  Signal Strength  ", Type = DataEntryType.TextBlock, IsReadOnly = true  });
            dataEntryElements.Add("ExpiryDate", new CustomDataGridColumnElement() { Heading = "  Expiry Date  ", Type = DataEntryType.TextBlock, IsReadOnly = true  });
            ExecuteAction(async () =>
            {
                searchCommand = new DelegateCommand(Search);
                clearCommand = new DelegateCommand(ClearSearch);
                closeCommand = new DelegateCommand(Close);
                saveCommand = new DelegateCommand(Save);
                refreshCommand = new DelegateCommand(Refresh);
                performCommand = new DelegateCommand(Perform);
                navigationClickCommand = new DelegateCommand(NavigationClick);
                selectionChangedCmd = new DelegateCommand(SelectionChanged);
            });
            ExecuteAction(() =>
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<NotificationTagViewDTO>> t = GetNotificationTagView(SearchParameters);
                    t.Wait();
                    List<NotificationTagViewDTO> result = t.Result;
                    if (result != null)
                    {
                        NotificationTagViewDTOList = new ObservableCollection<NotificationTagViewDTO>(result);
                    }
                }
            });
            gridSource = NotificationTagViewDTOList;
            gridSourceBackUp = NotificationTagViewDTOList.Select(item => item.Clone() as NotificationTagViewDTO).ToList();
            customDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                CollectionToBeRendered = new ObservableCollection<object>(gridSource),
                HeaderCollection = dataEntryElements,
                SelectOption = SelectOption.CheckBox,
                ShowSearchTextBox = false,
                IsComboAndSearchVisible = false
            };
            SetSearchCriteria(key, value);
            SelectedTextValue = selectedTextValue;
            dropDownVisible = false;
            textBoxDownVisible = true;
            FooterVM = new FooterVM(this.ExecutionContext)
            {
                Message = "",
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed
            };
            log.LogMethodExit();
        }

        #endregion Constructor

        #region Methods
        private void SelectionChanged(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            selectedTextValue = string.Empty;
            notificationTagsView = param as NotificationTagsView;
            if (notificationTagsView != null && selectedFieldName != null)
            {
                if (selectedFieldName.Key == "IS_IN_STORAGE" || 
                    selectedFieldName.Key == "IS_RETURNED" ||
                    selectedFieldName.Key == "PING_STATUS")
                {
                    List<SearchDTO> list = new List<SearchDTO>();
                    SearchDTO searchDTOYes = new SearchDTO("Yes", "Yes");
                    SearchDTO searchDTONO = new SearchDTO("No", "No");
                    list.Add(searchDTOYes);
                    list.Add(searchDTONO);
                    TagSearchFields = new ObservableCollection<SearchDTO>(list);
                    notificationTagsView.cbListFields.Visibility = System.Windows.Visibility.Visible;
                    notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Hidden;
                    dropDownVisible = true;
                    if (SelectedSearchName != null)
                    {
                        selectedTextValue = SelectedSearchName.Value;
                    }
                }
                else if (selectedFieldName.Key == "DEVICE_STATUS")
                {
                    List<SearchDTO> list = new List<SearchDTO>();
                    string[] enumValues = Enum.GetNames(typeof(RadianDeviceStatus)).ToArray();
                    foreach (string value in enumValues)
                    {
                        string result = value.Replace("_"," ");
                        log.Debug(result);
                        SearchDTO searchDTO = new SearchDTO(value, result);
                        list.Add(searchDTO);
                    }
                    TagSearchFields = new ObservableCollection<SearchDTO>(list);
                    notificationTagsView.cbListFields.Visibility = System.Windows.Visibility.Visible;
                    notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Hidden;
                    dropDownVisible = true;
                    if (SelectedSearchName != null)
                    {
                        selectedTextValue = SelectedSearchName.Value;
                    }
                    
                }
                else if (selectedFieldName.Key == "SIGNAL_STRENGTH")
                {
                    List<SearchDTO> list = new List<SearchDTO>();
                    TagSearchFields = new ObservableCollection<SearchDTO>();
                    string[] enumValues = Enum.GetNames(typeof(RadianDeviceStatusRSSI)).ToArray();
                    foreach (string value in enumValues)
                    {
                        string result = value.Substring(0, value.IndexOf("_") < 0 ? value.Length : value.IndexOf("_"));
                        log.Debug(result);
                        SearchDTO searchDTO = new SearchDTO(value, result);
                        list.Add(searchDTO);
                    }
                    TagSearchFields = new ObservableCollection<SearchDTO>(list);
                    if (SelectedSearchName != null)
                    {
                        selectedTextValue = SelectedSearchName.Value;
                    }
                    notificationTagsView.cbListFields.Visibility = System.Windows.Visibility.Visible;
                    notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Hidden;
                    dropDownVisible = true;
                }
                else if (selectedFieldName.Key == "EXPIRED" || selectedFieldName.Key == "EXPIRING_TODAY")
                {
                    notificationTagsView.cbListFields.Visibility = System.Windows.Visibility.Hidden;
                    notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Hidden;
                    dropDownVisible = false;
                    SelectedTextValue = "Expired";

                }
                else
                {
                    notificationTagsView.txtSearchValue.Text = string.Empty;
                    notificationTagsView.cbListFields.Visibility = System.Windows.Visibility.Hidden;
                    notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Visible;
                    dropDownVisible =false;
                }
            }
            log.LogMethodExit();
        }
        private void SetSearchCriteria(NotificationTagViewDTO.SearchByParameters key, string value)
        {
            try
            {
                log.LogMethodEntry();
                if (key == NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS)
                {
                    TagNotificationStatus TagNotificationStatus = TagNotificationStatusConverter.FromString(value);
                    selectedTagStatus = tagStatus.Where(x => x.Key == TagNotificationStatus.ToString()).FirstOrDefault();
                    selectedTextValue = "";
                    dropDownVisible = false;
                }
                else if (key == NotificationTagViewDTO.SearchByParameters.DEVICE_STATUS)
                {
                    selectedFieldName = TagFields.Where(x => x.Key == key.ToString()).FirstOrDefault();
                    if(TagListFields == null)
                    {
                        List<SearchDTO> list = new List<SearchDTO>();
                        string[] enumValues = Enum.GetNames(typeof(RadianDeviceStatus)).ToArray();
                        foreach (string deviceStatus in enumValues)
                        {
                            SearchDTO searchDTO = new SearchDTO(deviceStatus, deviceStatus);
                            list.Add(searchDTO);
                        }
                        TagSearchFields = new ObservableCollection<SearchDTO>(list);
                        SelectedSearchName = TagSearchFields.Where(x => x.Value == value).FirstOrDefault();
                        if (SelectedSearchName != null)
                        {
                            selectedTextValue = SelectedSearchName.Value;
                        }
                    }
                    selectedTextValue = value;
                    dropDownVisible = true;
                }
                else if (key == NotificationTagViewDTO.SearchByParameters.ALL)
                {
                    dropDownVisible = false;
                }
                else if (key == NotificationTagViewDTO.SearchByParameters.SIGNAL_STRENGTH)
                {
                    List<SearchDTO> list = new List<SearchDTO>();
                    TagSearchFields = new ObservableCollection<SearchDTO>();
                    string[] enumValues = Enum.GetNames(typeof(RadianDeviceStatusRSSI)).ToArray();
                    foreach (string signalStrength in enumValues)
                    {
                        SearchDTO searchDTO = new SearchDTO(signalStrength, signalStrength);
                        list.Add(searchDTO);
                    }
                    TagSearchFields = new ObservableCollection<SearchDTO>(list);
                    SelectedSearchName = TagSearchFields.Where(x => x.Value == value).FirstOrDefault();
                    if (SelectedSearchName != null)
                    {
                        selectedTextValue = SelectedSearchName.Value;
                    }
                    notificationTagsView.cbListFields.Visibility = System.Windows.Visibility.Visible;
                    notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Hidden;
                    dropDownVisible = true;
                }
                else
                {
                    selectedFieldName = TagFields.Where(x => x.Key == key.ToString()).FirstOrDefault();
                    selectedTextValue =  value == "1" ? "Yes" : value;
                    if (selectedFieldName.Key == "IS_IN_STORAGE" || selectedFieldName.Key == "PING_STATUS" || selectedFieldName.Key == "IS_RETURNED")
                    {
                        List<SearchDTO> list = new List<SearchDTO>();
                        SearchDTO searchDTOYes = new SearchDTO("Yes", "Yes");
                        SearchDTO searchDTONO = new SearchDTO("No", "No");
                        list.Add(searchDTOYes);
                        list.Add(searchDTONO);
                        TagSearchFields = new ObservableCollection<SearchDTO>(list);
                        SelectedSearchName = TagSearchFields.FirstOrDefault();
                        if (SelectedSearchName != null)
                        {
                            selectedTextValue = SelectedSearchName.Value;
                        }
                        dropDownVisible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void SetDisplayTagsVM()
        {
            try
            {
                if (DisplayTagsVM == null)
                {
                    DisplayTagsVM = new DisplayTagsVM();
                }
                DisplayTagsVM.DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                                    {
                                      new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                              Text = MessageContainerList.GetMessage(ExecutionContext,  "Notification Devices"),
                                               TextSize = TextSize.Medium,
                                               FontWeight = System.Windows.FontWeights.Bold
                                          }
                                      }
                                    };
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        private async Task<List<NotificationTagViewDTO>> GetNotificationTagView(List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>
                                  SearchParameters)
        {
            log.LogMethodEntry();
            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                foreach (NotificationTagViewDTO notificationTagViewDTO in result)
                {
                    notificationTagViewDTO.AcceptChanges();
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private void LoadLables()
        {
            log.LogMethodEntry();
            labelStatus = MessageContainerList.GetMessage(ExecutionContext, "Status: ");
            labelFields = MessageContainerList.GetMessage(ExecutionContext, "Search By: ");
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "Notification Tags");
            log.LogMethodExit();
        }


        private void Search(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagsView = param as NotificationTagsView;
            try
            {
                List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                if (SelectedSearchName != null && dropDownVisible)
                {
                    SelectedTextValue = SelectedSearchName.Value ;
                }
                log.Debug("selectedTextValue :" + selectedTextValue);
                if (string.IsNullOrWhiteSpace(SelectedTextValue) ==false)
                {

                    if (SelectedFieldName.Key.Equals("DEVICE_STATUS", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.DEVICE_STATUS, selectedTextValue));
                    }
                    if (SelectedFieldName.Key.Equals("IS_IN_STORAGE", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.IS_IN_STORAGE, (selectedTextValue == "Y" || selectedTextValue == "Yes" || selectedTextValue == "1") ? "1" : "0"));
                    }
                    if (SelectedFieldName.Key.Equals("IS_RETURNED", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.IS_RETURNED, (selectedTextValue == "Y" || selectedTextValue == "Yes" || selectedTextValue == "1") ? "1" : "0"));
                    }
                    if (SelectedFieldName.Key.Equals("PING_STATUS", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.PING_STATUS, (selectedTextValue == "Y" || selectedTextValue == "Yes" || selectedTextValue == "1") ? "1" : "0"));
                    }
                    if (SelectedFieldName.Key.Equals("TAG_NUMBER", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.TAG_NUMBER, selectedTextValue));
                    }
                    if (SelectedFieldName.Key.Equals("SIGNAL_STRENGTH", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SIGNAL_STRENGTH, selectedTextValue));
                    }
                    if (SelectedFieldName.Key.Equals("BATTERY_PERCENTAGE", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.BATTERY_PERCENTAGE, selectedTextValue));
                    }
                    if (SelectedFieldName.Key.Equals("EXPIRING_IN_X_MINUTES", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRING_IN_X_MINUTES, selectedTextValue));
                    }
                    if (SelectedFieldName.Key.Equals("EXPIRED", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRED, selectedTextValue));
                    }
                    if (SelectedFieldName.Key.Equals("EXPIRING_TODAY", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRING_TODAY, selectedTextValue));
                    }
                }

                if (SelectedTagStatus != null && SelectedTagStatus.Value != "Select")
                {
                    TagNotificationStatus tagNotificationStatus;
                    string value = string.Empty;
                    try
                    {
                        tagNotificationStatus = (TagNotificationStatus)Enum.Parse(typeof(TagNotificationStatus), SelectedTagStatus.Key, true);
                        value = TagNotificationStatusConverter.ToString(tagNotificationStatus);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while parsing the TagNotificationStatus type", ex);
                    }
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, value));
                }
                if(SelectedTagStatus.Value == "Select" && string.IsNullOrWhiteSpace(SelectedTextValue))
                {
                    return;
                }

                ExecuteAction(async () =>
                {

                    INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
                    List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
                    if (result != null && result.Any())
                    {
                        foreach (NotificationTagViewDTO notificationTagViewDTO in result)
                        {
                            notificationTagViewDTO.AcceptChanges();
                        }
                    }
                    if (result != null)
                    {
                        GridSource = new ObservableCollection<NotificationTagViewDTO>(result);
                    }
                    CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(GridSource);

                });
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            }
        }


        private void Perform(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagsView = param as NotificationTagsView;
            try
            {
                int count = this.CustomDataGridVM.SelectedItems.Count;
                if (count == 0)
                {
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 2460);
                    ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, "Required"), ErrorMessage);
                    CustomDataGridVM.SelectedItems.Clear();
                    //Refresh(param);
                    return;
                }
                if (count > 1)
                {
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 2460);
                    ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, "Required"), ErrorMessage);
                    CustomDataGridVM.SelectedItems.Clear();
                    //Refresh(param);
                    return;
                }
                SelectedNotificationTagViewDTO = (NotificationTagViewDTO)this.CustomDataGridVM.SelectedItems[0];
                if (SelectedNotificationTagViewDTO != null)
                {
                    PerformManualEventView performManualEventView = new PerformManualEventView();
                    PerformManualEventVM performManualEventVM = new PerformManualEventVM(ExecutionContext, SelectedNotificationTagViewDTO.TagNumber, SelectedNotificationTagViewDTO.NotificationTagId);
                    performManualEventView.DataContext = performManualEventVM;
                    performManualEventView.ShowDialog();
                    CustomDataGridVM.SelectedItems.Clear();
                    Refresh(param);
                }
                else
                {
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 2460);
                    ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, "Required"), ErrorMessage);
                }

            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;

            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            }
        }

        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagsView = param as NotificationTagsView;
            try
            {
                if (notificationTagsView != null)
                {
                    notificationTagsView.Close();
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            }
        }
        private void ClearSearch(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagsView = param as NotificationTagsView;
            try
            {
                notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Visible;
                textBoxDownVisible = true;
                dropDownVisible = false;
                SelectedTextValue = string.Empty;
                SelectedFieldName = TagFields.FirstOrDefault();
                SelectedTagStatus = TagStatus.FirstOrDefault();
                SelectedSearchName = new SearchDTO("", "");
                GridSource = new ObservableCollection<NotificationTagViewDTO>();
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(GridSource);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            }
        }

        private void Close(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagsView = param as NotificationTagsView;
            try
            {
                if (notificationTagsView != null)
                {
                    notificationTagsView.Close();
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;

            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
        }
        private void Save(object param)
        {
            try
            {
                log.LogMethodEntry(param);
                log.Info("Notification tag Save");
                string message = string.Empty;
                String ErrorMessage = String.Empty;
                notificationTagsView = param as NotificationTagsView;
                List<NotificationTagViewDTO> modifiedItems = new List<NotificationTagViewDTO>();
                foreach (object obj in customDataGridVM.CollectionToBeRendered)
                {
                    NotificationTagViewDTO modifiedNotificationTagViewDTO = (NotificationTagViewDTO)obj;
                    if (modifiedNotificationTagViewDTO.IsChanged)
                    {
                        modifiedItems.Add(modifiedNotificationTagViewDTO);
                    }

                }
                ExecuteAction(async () =>
               {

                   INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
                   foreach (NotificationTagViewDTO dto in modifiedItems)
                   {
                       message = await notificationTagUseCases.StorageInOutStatusChange(dto.NotificationTagId, dto.IsInStorage);
                       log.Debug(message);
                       log.LogVariableState("notificationTagUseCases result", message);
                       IsLoadingVisible = false;

                   }
                   if (message == "Success")
                   {
                       ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 1764);
                       ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Success"), "", ErrorMessage);
                   }
                   else
                   {
                       ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Update failed");
                       ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Failure", null), MessageContainerList.GetMessage(ExecutionContext, "Try Again!", null), ErrorMessage);
                   }
               });
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
        }
        private void Refresh(object param)
        {
            log.LogMethodEntry(param);
            try
            {
                ClearSearch(param);
                List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                using (NoSynchronizationContextScope.Enter())
                {
                    if (SearchParameters != null)
                    {
                        searchParameters = SearchParameters;
                    }
                    Task<List<NotificationTagViewDTO>> t = GetNotificationTagView(searchParameters);
                    t.Wait();
                    List<NotificationTagViewDTO> result = t.Result;
                    if (result != null)
                    {
                        GridSource = new ObservableCollection<NotificationTagViewDTO>(result);
                    }
                    CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(GridSource);
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            }
        }

        /// <summary>
        /// GetStatusBinding
        /// </summary>
        /// <returns></returns>
        public void LoadNotificationStatus()
        {
            tagStatus = new ObservableCollection<NotificationTagStatusDTO>();
            string[] enumValues = Enum.GetNames(typeof(TagNotificationStatus)).ToArray();
            NotificationTagStatusDTO defaultItem = new NotificationTagStatusDTO("NONE", MessageContainerList.GetMessage(ExecutionContext, "Select"));
            tagStatus.Add(defaultItem);
            foreach (string status in enumValues)
            {
                try
                {
                    if (status == TagNotificationStatus.DORMANT.ToString()
                        || status == (TagNotificationStatus.LOST.ToString())
                        || status == (TagNotificationStatus.IN_USE.ToString())
                        || status == (TagNotificationStatus.NOT_IN_USE.ToString()))
                    {
                        NotificationTagStatusDTO item = new NotificationTagStatusDTO(status, MessageContainerList.GetMessage(ExecutionContext, status.Replace("_", " ")));
                        tagStatus.Add(item);
                    }

                }
                catch (Exception ex)
                {
                    log.Error("Error occured while parsing the tagNotificationStatus type", ex);
                    throw ex;
                }
            }
            SelectedTagStatus = tagStatus.FirstOrDefault();
            log.LogMethodExit();
        }

        private void LoadFields()
        {
            log.LogMethodEntry();
            tagFields = new ObservableCollection<NotificationTagFieldsDTO>();
            tagFields.Add(new NotificationTagFieldsDTO("NONE", "Select"));
            tagFields.Add(new NotificationTagFieldsDTO("PING_STATUS", "Ping Status"));
            tagFields.Add(new NotificationTagFieldsDTO("DEVICE_STATUS", "Device Status"));
            tagFields.Add(new NotificationTagFieldsDTO("SIGNAL_STRENGTH", "Signal Strength"));
            tagFields.Add(new NotificationTagFieldsDTO("TAG_NUMBER", "Tag Number"));
            tagFields.Add(new NotificationTagFieldsDTO("BATTERY_PERCENTAGE", "Battery Percentage"));
            tagFields.Add(new NotificationTagFieldsDTO("IS_IN_STORAGE", "Is In Storage"));
            tagFields.Add(new NotificationTagFieldsDTO("IS_RETURNED", "Is Returned"));
            tagFields.Add(new NotificationTagFieldsDTO("EXPIRED", "Expired"));
            tagFields.Add(new NotificationTagFieldsDTO("EXPIRING_TODAY", "Expiring Today"));
            tagFields.Add(new NotificationTagFieldsDTO("EXPIRING_IN_X_MINUTES", "Expiring in X Minutes"));
            SelectedFieldName = tagFields.FirstOrDefault();
            log.LogMethodExit();
        }

        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);

            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = notificationTagsView;

            GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                Heading = heading,
                SubHeading = subHeading,
                Content = content,
                CancelButtonText = MessageContainerList.GetMessage(ExecutionContext, "OK"),
                TimerMilliSeconds = 5000,
                PopupType = PopupType.Timer,
            };

            messagePopupView.DataContext = messagePopupVM;
            messagePopupView.ShowDialog();

            if (messagePopupVM != null && messagePopupVM.Heading != null &&
                messagePopupVM.Heading.ToLower() == MessageContainerList.GetMessage(ExecutionContext, "Success").ToLower())
            {
                StatusCode = StatusCode.Success;
                //CloseAddWindow(messagePopupVM.Content);
            }
            log.LogMethodExit();
        }

        private void CloseAddWindow(string message)
        {
            SuccessMessage = message;
            if (notificationTagsView != null)
            {
                notificationTagsView.Close();
            }
        }
        #endregion Methods
    }
}

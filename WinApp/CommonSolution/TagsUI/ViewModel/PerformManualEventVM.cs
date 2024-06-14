/********************************************************************************************
 * Project Name - TagsUI
 * Description  - PerformManualEventVM 
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
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Tags;
using Semnox.Parafait.Transaction;
namespace Semnox.Parafait.TagsUI
{
    public class TagCommandDTO
    {
        private string key;
        private string command;
        public string Key { get { return key; } set { key = value; } }
        public string Command { get { return command; } set { command = value; } }

        public TagCommandDTO(string key, string command)
        {
            this.key = key;
            this.command = command;
        }
    }

    /// <summary>
    /// NotificationTagsVM
    /// </summary>
    public class PerformManualEventVM : BaseWindowViewModel
    {
        #region Members
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<NotificationTagProfileDTO> tagProfiles;
        // private ObservableCollection<string> tagCommands;
        private ObservableCollection<TagCommandDTO> tagCommands;
        private TagCommandDTO selectedCommand;
        private StatusCode statusCode;
        private string message;
        private int notificationTagId;
        private string notificationTagNumber;
        private PerformManualEventView performManualEventVM;
        private NotificationTagProfileDTO selectedNotificationTagProfileDTO;
        private DisplayTagsVM displayTagsVM;

        private ICommand closeCommand;
        private ICommand saveCommand;
        private ICommand navigationClickCommand;

        // Lables 
        private string labelStatus;
        private string labelFields;
        private string moduleName;
        #endregion Members

        #region Properties
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

        /// <summary>
        /// SelectedNotificationTagProfileDTO
        /// </summary>
        public NotificationTagProfileDTO SelectedNotificationTagProfileDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedNotificationTagProfileDTO);
                return selectedNotificationTagProfileDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedNotificationTagProfileDTO, value);
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
        /// NotificationTagId
        /// </summary>
        public int NotificationTagId
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(notificationTagId);
                return notificationTagId;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref notificationTagId, value);
                }
            }
        }
        /// <summary>
        /// NotificationTagNumber
        /// </summary>
        public string NotificationTagNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(notificationTagNumber);
                return notificationTagNumber;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref notificationTagNumber, value);
                }
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
        /// TagProfiles
        /// </summary>
        public ObservableCollection<NotificationTagProfileDTO> TagProfiles
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tagProfiles);
                return tagProfiles;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref tagProfiles, value);
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
        /// SelectedCommand
        /// </summary>
        public TagCommandDTO SelectedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedCommand);
                return selectedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedCommand, value);
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
        /// tagCommands
        /// </summary>
        public ObservableCollection<TagCommandDTO> TagCommands
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tagCommands);
                return tagCommands;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref tagCommands, value);
            }
        }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// NotificationTagsVM
        /// </summary>
        /// <param name="executionContext"></param>
        public PerformManualEventVM(ExecutionContext executionContext, string notificationTagNumber, int notificationTagId)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            this.notificationTagNumber = notificationTagNumber;
            this.notificationTagId = notificationTagId;
            SetDisplayTagsVM();
            LoadCommands();
            LoadLables();
            ExecuteAction(async () =>
            {
                closeCommand = new DelegateCommand(Close);
                saveCommand = new DelegateCommand(Save);
                navigationClickCommand = new DelegateCommand(NavigationClick);
                List<NotificationTagProfileDTO> result = await LoadNotificationProfiles();
                if (result != null && result.Any())
                {
                    TagProfiles = new ObservableCollection<NotificationTagProfileDTO>(result);
                    SelectedNotificationTagProfileDTO = TagProfiles[0];
                }
            });
            log.LogMethodExit();
        }
        #endregion Constructor

        #region Methods

        private void SetDisplayTagsVM()
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
                                               Text = MessageContainerList.GetMessage(ExecutionContext,  "Manual event for the notification Tag :" + NotificationTagNumber),
                                               TextSize = TextSize.Medium,
                                               FontWeight = System.Windows.FontWeights.Bold
                                          }
                                      }
                                    };

        }


        private void LoadLables()
        {
            log.LogMethodEntry();
            labelStatus = MessageContainerList.GetMessage(ExecutionContext, "Notification Tag Profile: ");
            labelFields = MessageContainerList.GetMessage(ExecutionContext, "Cammand: ");
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "Manual Events");
            log.LogMethodExit();
        }



        private void Close(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            performManualEventVM = param as PerformManualEventView;
            try
            {
                if (performManualEventVM != null)
                {
                    performManualEventVM.Close();
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
        }
        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            performManualEventVM = param as PerformManualEventView;
            try
            {
                if (performManualEventVM != null)
                {
                    performManualEventVM.Close();
                }
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
            log.LogMethodEntry(param);
            log.Info("Notification tag Save");
            String ErrorMessage = String.Empty;
            performManualEventVM = param as PerformManualEventView;
            try
            {
                //if (SelectedNotificationTagProfileDTO == null || SelectedNotificationTagProfileDTO.NotificationTagProfileId < 0)
                //{
                //    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Please select the notification tag profile", null);
                //}
                if (SelectedCommand == null || SelectedCommand.Key == "NONE")
                {
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Please select the command", null);
                }
                if (ErrorMessage != String.Empty)
                {
                    ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Error"), MessageContainerList.GetMessage(ExecutionContext, "Required fields"), ErrorMessage);
                    IsLoadingVisible = false;
                    return;
                }
                ExecuteAction(async () =>
                {

                    NotificationTagManualEventsDTO notificationTagManualEventsDTO = new NotificationTagManualEventsDTO(-1, NotificationTagId, SelectedCommand.Key.ToString()
                                                                            , -1, null, null, null, ServerDateTime.Now
                                                                             , "P", null, true, "");
                    List<NotificationTagManualEventsDTO> notificationTagManualEventsDTOList = new List<NotificationTagManualEventsDTO>();
                    notificationTagManualEventsDTOList.Add(notificationTagManualEventsDTO);

                    INotificationTagManualEventUseCases notificationTagManualEventUseCases = TransactionUseCaseFactory.GetNotificationTagManualEventsUseCases(ExecutionContext);
                    message = await notificationTagManualEventUseCases.SaveNotificationTagManualEvents(notificationTagManualEventsDTOList);
                    log.LogVariableState("notificationTagManualEventsDTOList result", message);
                    IsLoadingVisible = false;
                    if (message == "Success")
                    {
                        ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 2998, notificationTagNumber);
                        ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Success"), "Tag Manual Events", ErrorMessage);
                    }
                    else
                    {
                        ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 2999, notificationTagNumber);
                        ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Failure", null), MessageContainerList.GetMessage(ExecutionContext, "Try Again!", null), ErrorMessage);
                    }

                });
            }

            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
        }

        /// <summary>
        /// GetStatusBinding
        /// </summary>
        /// <returns></returns>
        public async Task<List<NotificationTagProfileDTO>> LoadNotificationProfiles()
        {
            log.LogMethodEntry();
            List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>(NotificationTagProfileDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            INotificationTagProfileUseCases notificationTagProfileUseCases = TransactionUseCaseFactory.GetNotificationTagProfileUseCases(ExecutionContext);
            List<NotificationTagProfileDTO> result = await notificationTagProfileUseCases.GetNotificationTagProfiles(SearchParameters);
            if (result != null && result.Any())
            {
                NotificationTagProfileDTO notificationTagProfileDTO = new NotificationTagProfileDTO(-1, "Select", 0, -1, 0, 0, 0, 0, 0, 0, true);
                result.Insert(0, notificationTagProfileDTO);
            }
            log.LogMethodExit();
            return result;
        }

        private void LoadCommands()
        {
            log.LogMethodEntry();
            string[] enumValues = Enum.GetNames(typeof(RadianDeviceCommand)).ToArray();
            if (tagCommands == null)
            {
                tagCommands = new ObservableCollection<TagCommandDTO>();
            }
            TagCommandDTO defaultItem = new TagCommandDTO("NONE", MessageContainerList.GetMessage(ExecutionContext, "Select Command"));
            TagCommands.Add(defaultItem);
            foreach (string status in enumValues)
            {
                try
                {

                    if (status == RadianDeviceCommand.PING_WB.ToString()
                        || status == (RadianDeviceCommand.TEST_WB.ToString())
                        || status == (RadianDeviceCommand.SOFT_WB_REMOVE.ToString())
                        || status == (RadianDeviceCommand.STORAGE_WB.ToString()))
                    {
                        TagCommandDTO item = new TagCommandDTO(status, MessageContainerList.GetMessage(ExecutionContext, status.Replace("_", " ")));
                        TagCommands.Add(item);
                    }

                }
                catch (Exception ex)
                {
                    log.Error("Error occured while parsing the tagNotificationStatus type", ex);
                    throw ex;
                }
            }
            SelectedCommand = TagCommands.FirstOrDefault();
            log.LogMethodExit();
        }

        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);

            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = performManualEventVM;

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
                CloseAddWindow(messagePopupVM.Content);
            }
            log.LogMethodExit();
        }

        private void CloseAddWindow(string message)
        {
            SuccessMessage = message;
            if (performManualEventVM != null)
            {
                performManualEventVM.Close();
            }
        }

        #endregion Methods
    }
}

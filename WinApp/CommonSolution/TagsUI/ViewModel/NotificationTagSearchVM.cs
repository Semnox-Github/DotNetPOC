/********************************************************************************************
 * Project Name - TagsUI
 * Description  - NotificationTagSearchVM 
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
using System.Globalization;
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
    /// NotificationTagSearchVM
    /// </summary>
    public class NotificationTagSearchVM : BaseWindowViewModel
    {
        #region Members
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string message;
        private string moduleName;
        private string selectedButton;

        private string lblLowBattery;
        private string lblWeakSignalStrength;
        private string lblLostDevice;
        private string lblInStorage;
        private string lblInUse;
        private string lblDormant;
        private string lblExpired;
        private string lblExpiringInXMinutes;
        private string lblExpiringToday;
        private string lblViewAll;
        private string lblIdleDevices;
        private string lblReturnedDevices;

        private DisplayTagsVM displayTagsVM;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private ObservableCollection<DisplayTag> displayTags1;
        private ObservableCollection<DisplayTag> displayTags2;
        private ObservableCollection<DisplayTag> displayTags3;
        private ObservableCollection<DisplayTag> displayTags4;
        private ObservableCollection<DisplayTag> displayTags5;
        private ObservableCollection<DisplayTag> displayTags6;
        private ObservableCollection<DisplayTag> displayTags7;
        private ObservableCollection<DisplayTag> displayTags8;
        private ObservableCollection<DisplayTag> displayTags9;
        private ObservableCollection<DisplayTag> displayTags10;
        private ObservableCollection<DisplayTag> displayTags11;
        private NotificationTagSearchView notificationTagSearchView;

        private ICommand lowBatteryCommand;
        private ICommand weakSignalStrengthCommand;
        private ICommand lostDeviceCommand;
        private ICommand inStorageCommand;
        private ICommand inUseCommand;
        private ICommand dormantCommand;
        private ICommand expiredCommand;
        private ICommand expiringInXminutesCommand;
        private ICommand expiringTodayCommand;
        private ICommand viewAllCommand;
        private ICommand idleDevicesCommand;
        private ICommand returnedDevicesCommand;
        private ICommand navigationClickCommand;
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
        public ObservableCollection<DisplayTag> DisplayTags1
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags1);
                return displayTags1;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags1, value);
                log.LogMethodExit(displayTags1);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags2
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags2);
                return displayTags2;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags2, value);
                log.LogMethodExit(displayTags2);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags3
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags3);
                return displayTags3;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags3, value);
                log.LogMethodExit(displayTags3);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags4
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags4);
                return displayTags4;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags4, value);
                log.LogMethodExit(displayTags4);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags5
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags5);
                return displayTags5;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags5, value);
                log.LogMethodExit(displayTags5);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags6
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags6);
                return displayTags6;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags6, value);
                log.LogMethodExit(displayTags6);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags7
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags7);
                return displayTags7;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags7, value);
                log.LogMethodExit(displayTags7);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags8
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags8);
                return displayTags8;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags8, value);
                log.LogMethodExit(displayTags8);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags9
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags9);
                return displayTags9;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags9, value);
                log.LogMethodExit(displayTags9);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags10
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags10);
                return displayTags10;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags10, value);
                log.LogMethodExit(displayTags10);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags11
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags11);
                return displayTags11;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags11, value);
                log.LogMethodExit(displayTags11);
            }
        }
        public GenericToggleButtonsVM GenericToggleButtonsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericToggleButtonsVM);
                return genericToggleButtonsVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref genericToggleButtonsVM, value);
                log.LogMethodExit(genericToggleButtonsVM);
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
        /// NotificationTagNumber
        /// </summary>
        public string NotificationTagNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedButton);
                return selectedButton;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedButton, value);
            }
        }

        /// <summary>
        /// ReturnedDevicesCommand
        /// </summary>
        public ICommand ReturnedDevicesCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(returnedDevicesCommand);
                return returnedDevicesCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref returnedDevicesCommand, value);
                log.LogMethodExit(returnedDevicesCommand);
            }
        }
        /// <summary>
        /// CloseCommand
        /// </summary>
        public ICommand LowBatteryCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(lowBatteryCommand);
                return lowBatteryCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                lowBatteryCommand = value;
            }
        }
        /// <summary>
        /// WeakSignalStrengthCommand
        /// </summary>
        public ICommand WeakSignalStrengthCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(weakSignalStrengthCommand);
                return weakSignalStrengthCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                weakSignalStrengthCommand = value;
            }
        }
        /// <summary>
        /// lostDeviceCommand
        /// </summary>
        public ICommand LostDeviceCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(lostDeviceCommand);
                return lostDeviceCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                lostDeviceCommand = value;
            }
        }

        /// <summary>
        /// InStorageCommand
        /// </summary>
        public ICommand InStorageCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(inStorageCommand);
                return inStorageCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                inStorageCommand = value;
            }
        }

        /// <summary>
        /// InUseCommand
        /// </summary>
        public ICommand InUseCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(inUseCommand);
                return inUseCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                inUseCommand = value;
            }
        }
        /// <summary>
        /// dormantCommand
        /// </summary>
        public ICommand DormantCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(dormantCommand);
                return dormantCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                dormantCommand = value;
            }
        }
        /// <summary>
        /// expiredCommand
        /// </summary>
        public ICommand ExpiredCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(expiredCommand);
                return expiredCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                expiredCommand = value;
            }
        }

        /// <summary>
        /// ExpiringInXminutesCommand
        /// </summary>
        public ICommand ExpiringInXminutesCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(expiringInXminutesCommand);
                return expiringInXminutesCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                expiringInXminutesCommand = value;
            }
        }

        /// <summary>
        /// expiringTodayCommand
        /// </summary>
        public ICommand ExpiringTodayCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(expiringTodayCommand);
                return expiringTodayCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                expiringTodayCommand = value;
            }
        }

        /// <summary>
        /// viewAllCommand
        /// </summary>
        public ICommand ViewAllCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(viewAllCommand);
                return viewAllCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                viewAllCommand = value;
            }
        }

        /// <summary>
        /// idleDevicesCommand
        /// </summary>
        public ICommand IdleDevicesCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(idleDevicesCommand);
                return idleDevicesCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                idleDevicesCommand = value;
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
        #endregion Properties

        #region Constructor
        /// <summary>
        /// NotificationTagsVM
        /// </summary>
        /// <param name="executionContext"></param>
        public NotificationTagSearchVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            SetDisplayTagsVM();
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "Summary View");
            ExecuteAction(async () =>
            {
                dormantCommand = new DelegateCommand(DormantButtonClicked);
                idleDevicesCommand = new DelegateCommand(IdleButtonClicked);
                viewAllCommand = new DelegateCommand(ViewAllButtonClicked);
                expiringTodayCommand = new DelegateCommand(ExpiringTodayButtonClicked);
                expiringInXminutesCommand = new DelegateCommand(ExpiringInXButtonClicked);
                expiredCommand = new DelegateCommand(ExpiredButtonClicked);
                inUseCommand = new DelegateCommand(InUseButtonClicked);
                inStorageCommand = new DelegateCommand(InStorageButtonClicked);
                lostDeviceCommand = new DelegateCommand(LostDevicesButtonClicked);
                weakSignalStrengthCommand = new DelegateCommand(WeakSignalButtonClicked);
                lowBatteryCommand = new DelegateCommand(LowBatteryButtonClicked);
                returnedDevicesCommand = new DelegateCommand(ReturnedDevicesButtonClicked);
                navigationClickCommand = new DelegateCommand(NavigationClick);
                await LoadButtonTextAsync();
            });
            log.LogMethodExit();
        }
        #endregion Constructor

        #region Methods
        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    notificationTagSearchView.Close();
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
        }

        private void IdleButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.DEVICE_STATUS, "IDLE"));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.DEVICE_STATUS, "IDLE");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void ViewAllButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.ALL, "");// NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "U");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void ExpiringTodayButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    double startTime = 6;
                    string businessStartTime = ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME");
                    log.Debug("businessStartTime : " + businessStartTime);
                    try
                    {
                        startTime = Convert.ToDouble(businessStartTime);
                        log.Debug("startTime : " + startTime);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        startTime = 6;
                    }
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRING_TODAY, startTime.ToString()));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.EXPIRING_TODAY, "");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void ExpiringInXButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    string expiringInXMiniutes = "10";
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRING_IN_X_MINUTES, expiringInXMiniutes));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.EXPIRING_IN_X_MINUTES, expiringInXMiniutes);
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void ExpiredButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRED, ""));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.EXPIRED, "");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void InUseButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "U"));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "U");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void InStorageButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.IS_IN_STORAGE, "1"));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.IS_IN_STORAGE, "1");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            }
            log.LogMethodExit();
        }

        private void LostDevicesButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "L"));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "L");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void WeakSignalButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SIGNAL_STRENGTH, "Weak"));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.SIGNAL_STRENGTH, "Weak");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void LowBatteryButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    string lowbatteryThreshold = ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "BATTERY_STATUS_THRESHOLD_FOR_SALE");
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.BATTERY_PERCENTAGE, lowbatteryThreshold));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.BATTERY_PERCENTAGE, lowbatteryThreshold);
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

        private void ReturnedDevicesButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.IS_RETURNED, "1"));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.IS_RETURNED, "1");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }
        private void DormantButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            notificationTagSearchView = param as NotificationTagSearchView;
            try
            {
                if (notificationTagSearchView != null)
                {
                    List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "D"));
                    LaunchNotificationTagSearch(SearchParameters, NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "D");
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            };
            log.LogMethodExit();
        }

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
                                               Text = MessageContainerList.GetMessage(ExecutionContext,  "Notification Tag Device Summary"),
                                               TextSize = TextSize.Medium,
                                               FontWeight = System.Windows.FontWeights.Bold
                                          }
                                      }
                                    };

        }

        private async Task LoadButtonTextAsync()
        {
            log.LogMethodEntry();
            int lowBatteryCount = await GetLowBatteryDeviceCount();
            DisplayTags1 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "LOW BATTERY DEVICES"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =lowBatteryCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };

            int weakSignalCount = await GetWeakSignalDeviceCount();
            DisplayTags2 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "WEAK SIGNAL DEVICES"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =weakSignalCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };

            int lostDeviceCount = await GetLostDeviceCount();
            DisplayTags3 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "LOST DEVICES"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =lostDeviceCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };
            int isInStorgeCount = await GetInStorageDeviceCount();
            DisplayTags4 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "DEVICES IN STORAGE"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =isInStorgeCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };

            int isUseDeviceCount = await GetInUseDeviceCount();
            DisplayTags5 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "DEVICES IN USE"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =isUseDeviceCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };

            int dormantDeviceCount = await GetDormantDeviceCount();
            DisplayTags6 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "DORMANT DEVICES"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =dormantDeviceCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };


            int expiredDeviceCount = await GetExipredDeviceCount();
            DisplayTags7 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "EXPIRED DEVICES"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =expiredDeviceCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };

            int expiredInXMinutesDeviceCount = await GetExipringInXMinDeviceCount();
            DisplayTags8 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "EXPIRING IN 10 MIN"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =expiredInXMinutesDeviceCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };

            int expiringToday = await GetExipringTodayDeviceCount();
            DisplayTags9 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "DEVICES EXPIRING TODAY"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =expiringToday.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };

            int allDeviceCount = await GetAllDeviceCount();
            DisplayTags10 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "ALL DEVICES"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text =allDeviceCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };


            int returnedDeviceCount = await GetReturnedDeviceCount();
            DisplayTags11 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext, "RETURNED DEVICES"), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text = returnedDeviceCount.ToString(), FontWeight = FontWeights.Bold, TextSize = TextSize.Large},
                                                        };

            moduleName = MessageContainerList.GetMessage(ExecutionContext, "Summary View");
            log.LogMethodExit();
        }
        private void LaunchNotificationTagSearch(List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> searchParameters,
                                                 NotificationTagViewDTO.SearchByParameters key, string value)
        {
            log.LogMethodEntry();
            NotificationTagsVM notificationTagsVM = new NotificationTagsVM(ExecutionContext, searchParameters, key, value);
            NotificationTagsView tagView = new NotificationTagsView();
            tagView.DataContext = notificationTagsVM;
            tagView.ShowDialog();
            ExecuteAction(async () =>
            {
                await LoadButtonTextAsync();
            });
            log.LogMethodExit();
        }

        /// <summary>
        /// GetLostDeviceCount
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetLostDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "L"));
            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }
            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetDormantDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "D"));
            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }
            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetInUseDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "U"));
            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }
            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetInStorageDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.IS_IN_STORAGE, "1"));
            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }
            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetLowBatteryDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.BATTERY_PERCENTAGE, "30"));

            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }

            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetAllDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }
            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetExipredDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRED, ""));

            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }

            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetExipringTodayDeviceCount()
        {
            log.LogMethodEntry();
            double startTime = 6;
            string businessStartTime = ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME");
            log.Debug("businessStartTime : " + businessStartTime);
            try
            {
                startTime = Convert.ToDouble(businessStartTime);
                log.Debug("startTime : " + startTime);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                startTime = 6;
            }
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRING_TODAY, startTime.ToString()));

            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }

            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetExipringInXMinDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.EXPIRING_IN_X_MINUTES, "10"));

            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }

            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetWeakSignalDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SIGNAL_STRENGTH, "Weak"));

            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }

            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetIdleDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.DEVICE_STATUS, "IDLE"));

            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }

            log.LogMethodExit(count);
            return count;
        }
        private async Task<int> GetReturnedDeviceCount()
        {
            log.LogMethodEntry();
            int count = 0;
            List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
            SearchParameters.Add(new KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>(NotificationTagViewDTO.SearchByParameters.IS_RETURNED, "1"));
            INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(ExecutionContext);
            List<NotificationTagViewDTO> result = await notificationTagUseCases.GetNotificationTagViewDTOList(SearchParameters);
            if (result != null && result.Any())
            {
                count = result.Count;
            }
            log.LogMethodExit(count);
            return count;
        }
        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);

            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = notificationTagSearchView;

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
                CloseAddWindow(messagePopupVM.Content);
            }
            log.LogMethodExit();
        }
        private void CloseAddWindow(string message)
        {
            SuccessMessage = message;
            if (notificationTagSearchView != null)
            {
                notificationTagSearchView.Close();
            }
        }

        #endregion Methods
    }
}

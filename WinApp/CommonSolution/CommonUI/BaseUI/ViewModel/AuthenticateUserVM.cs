/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Authorize User View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     17-Nov-2020   Amitha Joy              Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public enum loginStyle
    {
        FullScreen = 0,
        PopUp = 1
    }
    class AuthenticateUserVM : ViewModelBase
    {
        #region Members
        private bool ismultiScreenRowTwo;
        private bool multiScreenMode;

        private string userName;
        private string password;
        private string heading1 = "SEMNOX";
        private string heading2;
        private loginStyle loginStyle;
        private bool isLogoVisible;
        private bool isFingerPrintEnabled;
        internal ICommand loginCommand;
        private ICommand cardTapCommand;
        private ICommand fingerprintCommand;
        private ICommand closeCommand;
        private bool isValid;
        private GenericScanPopupView genericScanPopupView;
        private AuthenticateUserView authenticateUserView;
        private AuthenticateManagerVM authenticateManagerVM;
        internal AuthenticateManagerView authenticateManagerView;
        private GenericScanPopupVM genericScanPopupVM;
        private DeviceClass cardReader;
        private ExecutionContext systemuserExecutioncontext;
        private FooterVM footerVM = null;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public ICommand CloseCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(closeCommand);
                return closeCommand;
            }
            set
            {
                log.LogMethodEntry(closeCommand, value);
                SetProperty(ref closeCommand, value);
                log.LogMethodExit(closeCommand);
            }
        }
        public DeviceClass CardReader
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardReader);
                return cardReader;
            }
            set
            {
                log.LogMethodEntry(cardReader, value);
                SetProperty(ref cardReader, value);
                log.LogMethodExit(cardReader);
            }
        }
        public bool IsMultiScreenRowTwo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ismultiScreenRowTwo);
                return ismultiScreenRowTwo;
            }
            set
            {
                log.LogMethodEntry(ismultiScreenRowTwo, value);
                SetProperty(ref ismultiScreenRowTwo, value);
                log.LogMethodExit(ismultiScreenRowTwo);
            }
        }

        public bool MultiScreenMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenMode);
                return multiScreenMode;
            }
            set
            {
                log.LogMethodEntry(multiScreenMode, value);
                SetProperty(ref multiScreenMode, value);
                log.LogMethodExit(multiScreenMode);
            }
        }

        public string UserName
        {
            get
            {
                log.LogMethodEntry();
                return userName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref userName, value);
            }
        }

        public string Password
        {
            get
            {
                log.LogMethodEntry();
                return password;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref password, value);
            }
        }

        public string Heading1
        {
            get
            {
                log.LogMethodEntry();
                return heading1;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref heading1, value);
            }
        }

        public string Heading2
        {
            get
            {
                log.LogMethodEntry();
                return heading2;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref heading2, value);
            }
        }

        public loginStyle LoginStyle
        {
            get
            {
                log.LogMethodEntry();
                return loginStyle;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref loginStyle, value);
            }
        }

        public bool IsLogoVisible
        {
            get
            {
                log.LogMethodEntry();
                return isLogoVisible;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isLogoVisible, value);
            }
        }

        public bool IsValid
        {
            get
            {
                log.LogMethodEntry();
                return isValid;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isValid, value);
            }
        }

        public Visibility IsFingerPrintEnabled
        {
            get
            {
                log.LogMethodEntry();
                if (isFingerPrintEnabled)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                };
            }
        }

        public FooterVM FooterVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(footerVM);
                return footerVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref footerVM, value);
                log.LogMethodExit(footerVM);
            }
        }

        public GenericScanPopupVM GenericScanPopupVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericScanPopupVM);
                return genericScanPopupVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref genericScanPopupVM, value);
                log.LogMethodExit(genericScanPopupVM);
            }
        }

        public AuthenticateUserView AuthenticateUserView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(authenticateUserView);
                return authenticateUserView;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref authenticateUserView, value);
                log.LogMethodExit(authenticateUserView);
            }
        }

        public AuthenticateManagerVM AuthenticateManagerVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(authenticateManagerVM);
                return authenticateManagerVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref authenticateManagerVM, value);
                log.LogMethodExit(authenticateManagerVM);
            }
        }
        public AuthenticateManagerView AuthenticateManagerView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(authenticateManagerView);
                return authenticateManagerView;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref authenticateManagerView, value);
                log.LogMethodExit(authenticateManagerView);
            }
        }
        public GenericScanPopupView GenericScanPopupView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericScanPopupView);
                return genericScanPopupView;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref genericScanPopupView, value);
                log.LogMethodExit(genericScanPopupView);
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loginCommand);
                return loginCommand;
            }
            internal set
            {
                log.LogMethodEntry(value);
                SetProperty(ref loginCommand, value);
                log.LogMethodExit(loginCommand);
            }
        }

        public ICommand CardTapCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardTapCommand);
                return cardTapCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                SetProperty(ref cardTapCommand, value);
                log.LogMethodExit(cardTapCommand);
            }
        }

        public ICommand FingerPrintCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fingerprintCommand);
                return fingerprintCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                SetProperty(ref fingerprintCommand, value);
                log.LogMethodExit(fingerprintCommand);
            }
        }
        #endregion

        #region Constructors

        public AuthenticateUserVM()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public AuthenticateUserVM(ExecutionContext executioncontext, string heading1, string heading2, loginStyle loginStyle, bool isLogoVisible, DeviceClass cardReader)
        {
            log.LogMethodEntry(executioncontext, heading1, heading2, loginStyle, IsLogoVisible);
            this.heading1 = heading1;
            if (string.IsNullOrEmpty(this.heading1))
            {
                this.heading1 = "SEMNOX";
            }
            this.heading2 = heading2;
            this.loginStyle = loginStyle;
            this.isLogoVisible = isLogoVisible;
            ismultiScreenRowTwo = false;
            multiScreenMode = false;
            Initialize(executioncontext,cardReader);
            log.LogMethodExit();
        }
        #endregion


        #region Methods

        private void OnClosed(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                AuthenticateUserView userView = parameter as AuthenticateUserView;
                if (userView != null)
                {
                    userView.RaiseCloseButtonClickedEvent();
                }
            }
            log.LogMethodExit();
        }

        internal void Initialize(ExecutionContext executioncontext, DeviceClass cardReader)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executioncontext;
            this.cardReader = cardReader;
            if (LoginStyle != loginStyle.PopUp)
            {
                FooterVM = new FooterVM(this.ExecutionContext)
                {
                    Message = "",
                    MessageType = MessageType.None,
                    HideSideBarVisibility = Visibility.Hidden,

                };
            }
            if (LoginStyle == loginStyle.PopUp)
            {
                if (FooterVM != null)
                {
                    FooterVM.MultiScreenMode = true;
                }
            }
            //this.isFingerPrintEnabled = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "POS_FINGER_PRINT_AUTHENTICATION") == "Y" ? true : false;
            loginCommand = new DelegateCommand(LoginCredentials);
            cardTapCommand = new DelegateCommand(OpenCardTap);
            fingerprintCommand = new DelegateCommand(LoginFingerPrint);
            closeCommand = new DelegateCommand(OnClosed);
            if (cardReader != null)
            {
                log.Debug("Card Reader: " + cardReader);
                cardReader.Register(new EventHandler(CardScanCompleteEventHandle));
            }
            try
            {
                ExecuteAction(() =>
                {
                    systemuserExecutioncontext = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                });
            }
            catch (Exception ex)
            {
                if (FooterVM != null)
                {
                    this.FooterVM.Message = ex.Message;
                    this.FooterVM.MessageType = MessageType.Error;
                }
                IsValid = false;
                return;
            }

            log.LogMethodExit();
        }

        internal async Task LoginUser(object parameter,LoginRequest loginRequest)
        {
            loginRequest.MachineName = Environment.MachineName;
            loginRequest.SiteId = ConfigurationManager.AppSettings["SITE_ID"];
            IAuthenticationUseCases authenticateUseCases = AuthenticationUseCaseFactory.GetAuthenticationUseCases(systemuserExecutioncontext);
            ExecutionContext userExecutionContext;

            try
            {
                userExecutionContext = await authenticateUseCases.LoginUser(loginRequest);
                this.ExecutionContext = userExecutionContext;
                log.LogMethodExit(true);
                IsValid = true;
                if (authenticateUserView != null)
                {
                    if (cardReader != null)
                    {
                        cardReader.UnRegister();
                    }
                    authenticateUserView.Close();
                }
            }
            catch (UserAuthenticationException ue)
            {
                if (ue.UserAuthenticationErrorType == UserAuthenticationErrorType.CHANGE_PASSWORD)
                {
                    ChangePasswordView changePasswordView = new ChangePasswordView();
                    ChangePasswordVM changePasswordVM = new ChangePasswordVM(this.ExecutionContext, UserName);
                    changePasswordView.DataContext = changePasswordVM;
                    changePasswordView.Loaded += OnWindowLoaded;
                    if (parameter is AuthenticateUserView)
                    {
                        changePasswordView.Owner = parameter as AuthenticateUserView;
                        GenericScanPopupVM.IsMultiScreenRowTwo = this.IsMultiScreenRowTwo;
                        GenericScanPopupVM.MultiScreenMode = this.MultiScreenMode;
                    }
                    else if (parameter is AuthenticateManagerView)
                    {
                        authenticateManagerView = parameter as AuthenticateManagerView;
                        changePasswordView.Owner = authenticateManagerView;
                        if (authenticateManagerView.DataContext != null && authenticateManagerView.DataContext is AuthenticateManagerVM)
                        {
                            authenticateManagerVM = authenticateManagerView.DataContext as AuthenticateManagerVM;
                            GenericScanPopupVM.IsMultiScreenRowTwo = authenticateManagerVM.IsMultiScreenRowTwo;
                            GenericScanPopupVM.MultiScreenMode = authenticateManagerVM.MultiScreenMode;
                        }
                    }
                    changePasswordView.Show();
                }
                else
                {
                    OpenGenericMessagePopup(ue.Message, parameter);
                    IsValid = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                OpenGenericMessagePopup(ex.Message, parameter);
                IsValid = false;
                return;
            }
        }
        private void OpenGenericMessagePopup(string message,object parameter)
        {
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                MessageButtonsType = MessageButtonsType.OkCancel,
                SubHeading = null,
                Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "LOGIN FAILED", null),
                Content = MessageViewContainerList.GetMessage(this.ExecutionContext, message)
            };
            messagePopupView.DataContext = genericMessagePopupVM;
            if (parameter is AuthenticateUserView)
            {
                messagePopupView.Owner = parameter as AuthenticateUserView;
            }
            else if (parameter is AuthenticateManagerView)
            {
                authenticateManagerView = parameter as AuthenticateManagerView;
                messagePopupView.Owner = authenticateManagerView;
                if (authenticateManagerView.DataContext != null && authenticateManagerView.DataContext is AuthenticateManagerVM)
                {
                    authenticateManagerVM = authenticateManagerView.DataContext as AuthenticateManagerVM;
                }
            }
            messagePopupView.Loaded += OnWindowLoaded;
            messagePopupView.Show();
        }
        internal async void LoginCredentials(object parameter)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                {
                    OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                    CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                    MessageButtonsType = MessageButtonsType.OkCancel,
                    SubHeading = null,
                    Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "LOGIN FAILED", null),
                    Content = MessageViewContainerList.GetMessage(this.ExecutionContext, 807)
                };
                messagePopupView.DataContext = genericMessagePopupVM;
                if (parameter is AuthenticateUserView)
                {
                    messagePopupView.Owner = parameter as AuthenticateUserView;
                }
                else if (parameter is AuthenticateManagerView)
                {
                    authenticateManagerView = parameter as AuthenticateManagerView;
                    messagePopupView.Owner = authenticateManagerView;
                    if (authenticateManagerView.DataContext != null && authenticateManagerView.DataContext is AuthenticateManagerVM)
                    {
                        authenticateManagerVM = authenticateManagerView.DataContext as AuthenticateManagerVM;
                    }
                }
                messagePopupView.Loaded += OnWindowLoaded;
                messagePopupView.Show();
                IsValid = false;
                return;
            }
            LoginRequest loginRequest = new LoginRequest();
            loginRequest.LoginId = UserName;
            loginRequest.Password = Password;
            await LoginUser(parameter, loginRequest);
        }

        void LoginFingerPrint(object parameter)
        {
            //log.LogMethodEntry();
            //FingerPrintLoginVM fingerPrintLoginVM = new FingerPrintLoginVM(this.ExecutionContext);
            //FingerPrintLoginView fingerPrintLoginView = new FingerPrintLoginView();
            //fingerPrintLoginView.DataContext = fingerPrintLoginVM;
            //if (parameter is AuthenticateUserView)
            //{
            //    fingerPrintLoginView.Owner = parameter as AuthenticateUserView;
            //    fingerPrintLoginVM.IsMultiScreenRowTwo = this.ismultiScreenRowTwo;
            //    fingerPrintLoginVM.MultiScreenMode = this.MultiScreenMode;
            //}
            //else if (parameter is AuthenticateManagerView)
            //{
            //    authenticateManagerView = parameter as AuthenticateManagerView;
            //    fingerPrintLoginView.Owner = authenticateManagerView;
            //    if (authenticateManagerView.DataContext != null && authenticateManagerView.DataContext is AuthenticateManagerVM)
            //    {
            //        authenticateManagerVM = authenticateManagerView.DataContext as AuthenticateManagerVM;
            //        fingerPrintLoginVM.IsMultiScreenRowTwo = authenticateManagerVM.IsMultiScreenRowTwo;
            //        fingerPrintLoginVM.MultiScreenMode = authenticateManagerVM.MultiScreenMode;
            //    }
            //}
            //fingerPrintLoginView.Loaded += OnWindowLoaded;
            //fingerPrintLoginView.Show();
            //log.LogMethodExit(true);
            //IsValid = true;
        }

        private async Task LoginCardTap(string cardNumber)
        {
            log.LogMethodEntry();
            LoginRequest loginRequest = new LoginRequest();
            loginRequest.TagNumber = cardNumber;
            if (this.authenticateUserView != null)
            {
                await LoginUser(authenticateUserView, loginRequest);
            }
            if (this.authenticateManagerView != null)
            {
                await LoginUser(authenticateManagerView, loginRequest);
            }
            if (authenticateManagerVM != null)
            {
                if (IsValid)
                {
                    authenticateManagerVM.LoginManagerCardTap(ExecutionContext.UserId);
                }
                else
                {
                    authenticateManagerVM.ManagerId = -1;
                }
            }

        }

        void OpenCardTap(object parameter)
        {
            log.LogMethodEntry();
            GenericScanPopupView = new GenericScanPopupView();
            GenericScanPopupVM = new GenericScanPopupVM(ExecutionContext, cardReader)
            {
                Title = MessageViewContainerList.GetMessage(ExecutionContext, "TAP CARD NOW", null),
                ScanPopupType = GenericScanPopupVM.PopupType.TAPCARD,
                TimerMilliSeconds = 5000
            };
            GenericScanPopupView.DataContext = GenericScanPopupVM;
            if (parameter is AuthenticateUserView)
            {
                GenericScanPopupView.Owner = parameter as AuthenticateUserView;
                GenericScanPopupVM.IsMultiScreenRowTwo = this.ismultiScreenRowTwo;
                GenericScanPopupVM.MultiScreenMode = this.multiScreenMode;
            }
            else if (parameter is AuthenticateManagerView)
            {
                authenticateManagerView = parameter as AuthenticateManagerView;
                GenericScanPopupView.Owner = authenticateManagerView;
                if (authenticateManagerView.DataContext != null && authenticateManagerView.DataContext is AuthenticateManagerVM)
                {
                    authenticateManagerVM = authenticateManagerView.DataContext as AuthenticateManagerVM;
                    GenericScanPopupVM.IsMultiScreenRowTwo = authenticateManagerVM.IsMultiScreenRowTwo;
                    GenericScanPopupVM.MultiScreenMode = authenticateManagerVM.MultiScreenMode;
                }
            }
            GenericScanPopupView.Loaded += OnWindowLoaded;
            GenericScanPopupView.Show();
            log.LogMethodExit(true);
            IsValid = true;
        }

        internal void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Window window = sender as Window;
            if (AuthenticateUserView != null)
            {
                window.Width = AuthenticateUserView.ActualWidth;
                window.Height = AuthenticateUserView.ActualHeight;
                window.Top = AuthenticateUserView.Top;
                window.Left = AuthenticateUserView.Left;
            }
            else if (authenticateManagerView != null)
            {
                window.Width = authenticateManagerView.ActualWidth;
                window.Height = authenticateManagerView.ActualHeight;
                window.Top = authenticateManagerView.Top;
                window.Left = authenticateManagerView.Left;
            }
        }

        private async void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            object parameter = null;
            if (GenericScanPopupView != null)
            {
                GenericScanPopupVM GenericScanPopupVM = GenericScanPopupView.DataContext as GenericScanPopupVM;
                if (GenericScanPopupVM.ScanPopupType == GenericScanPopupVM.PopupType.TAPCARD)
                {
                    GenericScanPopupView.Close();
                }
            }
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                log.Debug("Scanned Number: " + checkScannedEvent.Message);
                TagNumberViewParser tagNumberViewParser = new TagNumberViewParser(ExecutionContext);
                TagNumberView tagNumberView;
                if (authenticateUserView != null)
                {
                    parameter = authenticateUserView;
                }
                else if (authenticateManagerView != null)
                {
                    parameter = authenticateManagerView;
                }
                if (tagNumberViewParser.TryParse(checkScannedEvent.Message, out tagNumberView) == false)
                {
                    string message = tagNumberViewParser.Validate(checkScannedEvent.Message);
                    OpenGenericMessagePopup(message, parameter);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }
                try
                {
                    await LoginCardTap(tagNumberView.Value);
                }
                catch (Exception ex)
                {
                    OpenGenericMessagePopup(ex.Message, parameter);
                }
            }
            log.LogMethodExit();
        }

        string GetHash(string input)
        {
            log.LogMethodEntry(input);
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            Core.Utilities.ByteArray byteArray = new Core.Utilities.ByteArray(hash);
            string result = byteArray.ToString();
            log.LogMethodExit(result);
            return result;
        }
        #endregion

    }
}

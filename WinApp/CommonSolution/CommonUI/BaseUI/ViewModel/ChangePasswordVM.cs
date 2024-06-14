/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Change password View Model
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
using System.Windows;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.CommonUI
{
    class ChangePasswordVM : ViewModelBase
    {
        #region Members
        private bool ismultiScreenRowTwo;
        private bool multiScreenMode;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string password;
        private string newPassword;
        private string reneterPassword;
        private string username;
        private FooterVM footerVM;
        private ICommand changeCommand;
        private ICommand cancelCommand;
        private ICommand closeCommand;
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
                return username;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref username, value);
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

        public string NewPassword
        {
            get
            {
                log.LogMethodEntry();
                return newPassword;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref newPassword, value);
            }
        }

        public string ReEnterPassword
        {
            get
            {
                log.LogMethodEntry();
                return reneterPassword;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref reneterPassword, value);
            }
        }

        public FooterVM FooterVM
        {
            get
            {
                log.LogMethodEntry();
                return footerVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref footerVM, value);
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cancelCommand);
                return cancelCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                SetProperty(ref cancelCommand, value);
                log.LogMethodExit(cancelCommand);
            }
        }

        public ICommand ChangeCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(changeCommand);
                return changeCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                SetProperty(ref changeCommand, value);
                log.LogMethodExit(changeCommand);
            }
        }

        #endregion

        #region Constructors
        public ChangePasswordVM(ExecutionContext executioncontext, string username)
        {
            this.ExecutionContext = executioncontext;
            this.UserName = username;

            FooterVM = new FooterVM(this.ExecutionContext)
            {
                Message = "",
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Hidden,
                MultiScreenMode = true

            };
            closeCommand = new DelegateCommand(OnCloseClicked);
            cancelCommand = new DelegateCommand(Cancel);
            changeCommand = new DelegateCommand(ChangePassword);
        }

        #endregion

        #region Methods
        private void OnCloseClicked(object parameter)
        {
            log.LogMethodEntry();
            ChangePasswordView changePasswordView = parameter as ChangePasswordView;
            if (changePasswordView != null)
            {
                changePasswordView.Close();
            }
            log.LogMethodExit();
        }

        public ExecutionContext GetExecutionContext()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ExecutionContext);
            return ExecutionContext;

        }
        private void Cancel(object parameter)
        {
            log.LogMethodEntry();
            ChangePasswordView changePasswordView = parameter as ChangePasswordView;
            log.LogMethodExit();
            if (changePasswordView != null)
            {
                changePasswordView.Close();
            }
        }

        internal async void ChangePassword(object parameter)
        {
            log.LogMethodEntry();
            bool error = false;
            string message = string.Empty;
            ChangePasswordView changePasswordView = parameter as ChangePasswordView;
            if (string.IsNullOrEmpty(username))
            {
                message = MessageViewContainerList.GetMessage(this.ExecutionContext, 2926, null);
                error = true;
            }
            else if (string.IsNullOrEmpty(newPassword))
            {
                message = MessageViewContainerList.GetMessage(this.ExecutionContext, 273, null);
                error = true;
            }
            else if (reneterPassword == null || reneterPassword.Equals(newPassword) == false)
            {
                message = MessageViewContainerList.GetMessage(this.ExecutionContext, 274, null);
                error = true;
            }
            if (error)
            {
                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                {
                    OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                    CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                    MessageButtonsType = MessageButtonsType.OkCancel,
                    SubHeading = null,
                    Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "CHANGE PASSWORD FAILED", null),
                    Content = message
                };
                messagePopupView.DataContext = genericMessagePopupVM;
                messagePopupView.Owner = changePasswordView;
                //messagePopupView.Loaded += OnWindowLoaded;
                messagePopupView.Show();
            }
            else
            {
                try
                {
                    ExecutionContext systemuserExecutioncontext = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                    LoginRequest loginRequest = new LoginRequest();
                    loginRequest.LoginId = username;
                    loginRequest.Password = password;
                    loginRequest.NewPassword = newPassword;
                    loginRequest.MachineName = Environment.MachineName;
                    IAuthenticationUseCases authenticateUseCases = AuthenticationUseCaseFactory.GetAuthenticationUseCases(systemuserExecutioncontext);
                    ExecutionContext userExecutioncontext = await authenticateUseCases.LoginUser(loginRequest);
                    if (changePasswordView != null)
                    {
                        changePasswordView.Close();
                    }
                }
                catch (Exception ex)
                {
                    GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                    GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                    {
                        OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                        CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                        MessageButtonsType = MessageButtonsType.OkCancel,
                        SubHeading = null,
                        Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "CHANGE PASSWORD FAILED", null),
                        Content = ex.Message
                    };
                    messagePopupView.DataContext = genericMessagePopupVM;
                    messagePopupView.Owner = changePasswordView;
                    messagePopupView.Show();
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}

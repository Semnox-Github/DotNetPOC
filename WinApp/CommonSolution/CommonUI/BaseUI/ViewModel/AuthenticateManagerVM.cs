using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    class AuthenticateManagerVM : AuthenticateUserVM
    {
        #region Members
        private string userId;
        private int userroleId;
        private int managerId;
        private ICommand cancelCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Propertiess

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

        public string UserId
        {
            get
            {
                log.LogMethodEntry();
                return userId;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref userId, value);
            }
        }

        public int ManagerId
        {
            get
            {
                log.LogMethodEntry();
                return managerId;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref managerId, value);
            }
        }
        #endregion

        #region Methods
        public bool CheckManager(string UserLoginId)
        {
            if (UserRoleViewContainerList.CanApproveFor(ExecutionContext.SiteId, userroleId, UserViewContainerList.GetUserContainerDTO(ExecutionContext.SiteId, UserLoginId).RoleId))
            {
                managerId = UserViewContainerList.GetUserContainerDTO(ExecutionContext.SiteId, UserLoginId).UserId;
                return true;
            }
            else
            {
                managerId = -1;
                return false;
            }

        }
        internal async void LoginManagerCredentials(object parameter)
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
                if (parameter is AuthenticateManagerView)
                {
                    authenticateManagerView = parameter as AuthenticateManagerView;
                    messagePopupView.Owner = authenticateManagerView;
                    if (authenticateManagerView.DataContext != null && authenticateManagerView.DataContext is AuthenticateManagerVM)
                    {
                        AuthenticateManagerVM = authenticateManagerView.DataContext as AuthenticateManagerVM;
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
            if (IsValid)
            {
                IsValid = false;
                bool isManager = CheckManager(UserName);
                if (isManager)
                {
                    IsValid = true;
                }
                else
                {
                    managerId = -1;
                    IsValid = false;
                }
                if (IsValid)
                {
                    AuthenticateManagerView authenticateManagerView = parameter as AuthenticateManagerView;
                    log.LogMethodExit();
                    if (authenticateManagerView != null)
                    {
                        if (CardReader != null)
                        {
                            CardReader.UnRegister();
                        }
                        authenticateManagerView.Close();
                    }
                }
                else
                {
                    GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                    GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                    {
                        OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                        CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                        MessageButtonsType = MessageButtonsType.OkCancel,
                        SubHeading = null,
                        Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "APPROVAL FAILED", null),
                        Content = MessageViewContainerList.GetMessage(this.ExecutionContext, 2925) // change to message
                    };
                    messagePopupView.DataContext = genericMessagePopupVM;
                    messagePopupView.Owner = parameter as AuthenticateManagerView;
                    messagePopupView.Loaded += OnWindowLoaded;
                    messagePopupView.Show();
                }
            }
        }

        private void Cancel(object parameter)
        {
            log.LogMethodEntry();
            AuthenticateManagerView authenticateManagerView = parameter as AuthenticateManagerView;
            log.LogMethodExit();
            if (authenticateManagerView != null)
            {
                IsValid = false;
                if (CardReader != null)
                {
                    CardReader.UnRegister();
                }
                authenticateManagerView.Close();
            }
        }

        internal void LoginManagerCardTap(string userLoginId)
        {
            if (IsValid)
            {
                IsValid = false;
                bool isManager = CheckManager(userLoginId);
                if (isManager)
                {
                    IsValid = true;
                }
                else
                {
                    managerId = -1;
                    IsValid = false;
                }
                if (IsValid)
                {
                    log.LogMethodExit();
                    if (authenticateManagerView != null)
                    {
                        if (CardReader != null)
                        {
                            CardReader.UnRegister();
                        }
                        authenticateManagerView.Close();
                    }
                }
                else
                {
                    GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                    GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                    {
                        OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                        CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                        MessageButtonsType = MessageButtonsType.OkCancel,
                        SubHeading = null,
                        Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "APPROVAL FAILED", null),
                        Content = MessageViewContainerList.GetMessage(this.ExecutionContext, 2925) // change to message
                    };
                    messagePopupView.DataContext = genericMessagePopupVM;
                    if (authenticateManagerView != null)
                    {
                        messagePopupView.Owner = authenticateManagerView;
                    }
                    messagePopupView.Loaded += OnWindowLoaded;
                    messagePopupView.Show();
                }
            }
        }

        internal void LoginManagerFingerPrint(object parameter)
        {

        }
        #endregion
        #region Constructor
        public AuthenticateManagerVM(ExecutionContext executioncontext, DeviceClass cardReader)
        {
            log.LogMethodEntry(executioncontext);
            userId = executioncontext.UserId;
            userroleId = UserViewContainerList.GetUserContainerDTO(executioncontext.SiteId, userId).RoleId;
            ExecutionContext managerExecutionContext = new ExecutionContext(string.Empty, executioncontext.GetSiteId(), executioncontext.MachineId, -1,executioncontext.IsCorporate,executioncontext.LanguageId);
            base.Initialize(managerExecutionContext, cardReader);
            LoginCommand = new DelegateCommand(LoginManagerCredentials);
            CancelCommand = new DelegateCommand(Cancel);
            FooterVM.MultiScreenMode = true;
            AuthenticateManagerVM = this;
            log.LogMethodExit();
        }
        #endregion 
    }
}

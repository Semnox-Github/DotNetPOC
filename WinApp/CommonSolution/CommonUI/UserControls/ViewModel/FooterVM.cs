/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Footer View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda            modified for multi user
 ********************************************************************************************/
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{

    public enum MessageType
    {
        Error = 0,
        Warning = 1,
        Info = 2,
        None = 3
    }

    public class FooterVM : ViewModelBase
    {
        #region Members
        private GenericMessagePopupView messagePopupView;
        private DateTime clickStarted;
        private bool spanHideSideBar;
        private Visibility visibility;
        private bool multiScreenMode;

        private string sideBarContent;
        private ICommand hideSideBarCommand;
        private ICommand messageClickedCommand;
        private Visibility hideSideBarVisibility;
        private ConnectionStatus connectionStatus;
        private string message;
        private MessageType messageType;
        private string time;
        private string dateFormat;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties       
        public bool SpanHideSideBar
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(spanHideSideBar);
                return spanHideSideBar;
            }
            set
            {
                log.LogMethodEntry(spanHideSideBar, value);
                SetProperty(ref spanHideSideBar, value);
                log.LogMethodExit(spanHideSideBar);
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

        public Visibility Visibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(visibility);
                return visibility;
            }
            set
            {
                log.LogMethodEntry(visibility, value);
                SetProperty(ref visibility, value);
                log.LogMethodExit(visibility);
            }
        }
        public GenericMessagePopupView MessagePopupView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(messagePopupView);
                return messagePopupView;
            }
        }
        public ICommand MessageClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(messageClickedCommand);
                return messageClickedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                messageClickedCommand = value;
            }
        }


        public ICommand HideSideBarCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hideSideBarCommand);
                return hideSideBarCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                hideSideBarCommand = value;
            }
        }

        public Visibility HideSideBarVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hideSideBarVisibility);
                return hideSideBarVisibility;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref hideSideBarVisibility, value);
            }
        }

        public ConnectionStatus ConnectionStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(connectionStatus);
                return connectionStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref connectionStatus, value);
            }
        }

        public string Time
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(time);
                return time;
            }
            private set
            {
                log.LogMethodEntry(value);
                SetProperty(ref time, value);
            }
        }

        public string Message
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

        public bool IsLowerResoultion
        {
            get
            {
                log.LogMethodEntry();
                if (SystemParameters.PrimaryScreenWidth > 1024)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    log.LogMethodExit(true);
                    return true;
                }

            }
        }

        public MessageType MessageType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(messageType);
                return messageType;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref messageType, value);
            }
        }

        public string SideBarContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(sideBarContent);
                return sideBarContent;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref sideBarContent, value);
            }
        }
        #endregion

        #region Constructor
        public FooterVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;

            hideSideBarCommand = new DelegateCommand(OnHideSideBarClicked);
            messageClickedCommand = new DelegateCommand(OnMessageClicked);

            message = string.Empty;

            messageType = MessageType.Warning;

            connectionStatus = ConnectionStatus.Online;

            hideSideBarVisibility = Visibility.Visible;

            sideBarContent = MessageViewContainerList.GetMessage(this.ExecutionContext, "Hide Sidebar");

            dateFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");

            Time = DateTime.Now.ToString("dddd, " + dateFormat + " h:mm:ss tt");

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += timer_Tick;

            timer.Start();

            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnMessageClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (messagePopupView == null && parameter != null && !string.IsNullOrEmpty(message))
            {
                messagePopupView = new GenericMessagePopupView();
                GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "Message", null),
                    Content = this.message,
                    OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "COPY", null),
                    CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                    MessageButtonsType = MessageButtonsType.OkCancel
                };
                messagePopupView.DataContext = messagePopupVM;
                messagePopupView.Closed += OnCopyMessagePopupClosed;
                messagePopupView.Show();
                FooterUserControl footerUserControl = parameter as FooterUserControl;
                if (footerUserControl != null)
                {
                    footerUserControl.RaiseMessageClickedEvent();
                }
            }
            log.LogMethodExit();
        }

        private void OnCopyMessagePopupClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (messagePopupView != null && messagePopupView.DataContext != null)
            {
                GenericMessagePopupVM messagePopupVM = messagePopupView.DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    Clipboard.SetText(message);
                }
                messagePopupView = null;
            }
            log.LogMethodExit();
        }

        public void OnHideSideBarClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (this.SideBarContent == MessageViewContainerList.GetMessage(ExecutionContext, "Hide Sidebar"))
            {
                this.SideBarContent = MessageViewContainerList.GetMessage(ExecutionContext, "Show Sidebar");
            }
            else
            {
                this.SideBarContent = MessageViewContainerList.GetMessage(ExecutionContext, "Hide Sidebar");
            }
            if (parameter != null)
            {
                FooterUserControl footerUserControl = parameter as FooterUserControl;
                if (footerUserControl != null)
                {
                    footerUserControl.RaiseSidebarClickedEvent();
                }
            }
            log.LogMethodExit(SideBarContent);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            Time = DateTime.Now.ToString("dddd, " + dateFormat + " h:mm:ss tt");

            ConnectionStatus = RemoteConnectionCheckContainer.GetInstance.GetStatus();
            log.LogMethodExit();
        }

        #endregion
    }
}

/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - view model for generic message pop up
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Windows;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Threading;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public enum PopupType
    {
        Normal = 0,
        Timer = 1
    }

    public enum MessageButtonsType
    {
        OK,
        OkCancel
    }
    public class GenericMessagePopupVM : ViewModelBase
    {

        #region Members
        private int timerMilliSeconds;

        private string heading;
        private string content;
        private string subHeading;
        private string okButtonText;
        private string cancelButtonText;

        private PopupType popupType;
        private Visibility okButtonVisibility;
        private MessageButtonsType messageType;
        private ButtonClickType buttonClickType;

        private DispatcherTimer timer;
        private GenericMessagePopupView messagePopupView;

        private ICommand actionsCommand;

        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string OkButtonText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(okButtonText);
                return okButtonText;
            }
            set
            {
                log.LogMethodEntry(okButtonText, value);
                SetProperty(ref okButtonText, value);
                log.LogMethodExit(okButtonText);
            }
        }
        public Visibility OkButtonVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(okButtonVisibility);
                return okButtonVisibility;
            }
            private set
            {
                log.LogMethodEntry(okButtonVisibility, value);
                SetProperty(ref okButtonVisibility, value);
                log.LogMethodExit(okButtonVisibility);
            }
        }
        public MessageButtonsType MessageButtonsType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(messageType);
                return messageType;
            }
            set
            {
                log.LogMethodEntry(messageType, value);
                SetProperty(ref messageType, value);
                log.LogMethodExit(messageType);
            }
        }
        public ButtonClickType ButtonClickType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonClickType);
                return buttonClickType;
            }
            set
            {
                log.LogMethodEntry(buttonClickType, value);
                SetProperty(ref buttonClickType, value);
                log.LogMethodExit(buttonClickType);
            }
        }
        public int TimerMilliSeconds
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(timerMilliSeconds);
                return timerMilliSeconds;
            }
            set
            {
                log.LogMethodEntry(timerMilliSeconds, value);
                SetProperty(ref timerMilliSeconds, value);
                log.LogMethodExit(timerMilliSeconds);
            }
        }
        public PopupType PopupType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(popupType);
                return popupType;
            }
            set
            {
                log.LogMethodEntry(popupType, value);
                SetProperty(ref popupType, value);
                log.LogMethodExit(popupType);
            }
        }
        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(heading);
                return heading.ToUpper();
            }
            set
            {
                log.LogMethodEntry(heading, value);
                SetProperty(ref heading, value);
                log.LogMethodExit(heading);
            }
        }
        public string SubHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(subHeading);
                return subHeading;
            }
            set
            {
                log.LogMethodEntry(subHeading, value);
                SetProperty(ref subHeading, value);
                log.LogMethodExit(subHeading);
            }
        }
        public string Content
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(content);
                return content;
            }
            set
            {
                log.LogMethodEntry(content, value);
                SetProperty(ref content, value);
                log.LogMethodExit(content);
            }
        }
        public string CancelButtonText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cancelButtonText);
                return cancelButtonText.ToUpper();
            }
            set
            {
                log.LogMethodEntry(cancelButtonText, value);
                SetProperty(ref cancelButtonText, value);
                log.LogMethodExit(cancelButtonText);
            }
        }
        public ICommand ActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(actionsCommand);
                return actionsCommand;
            }
        }
        #endregion

        #region Constructors
        public GenericMessagePopupVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            ExecutionContext = executionContext;
            InitilaizeCommands();

            heading = string.Empty;
            subHeading = string.Empty;
            content = string.Empty;
            cancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "CANCEL");
            okButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK");

            popupType = PopupType.Normal;
            MessageButtonsType = MessageButtonsType.OK;

            SetOkButtonVisibiltiy();

            log.LogMethodExit();
        }
        public GenericMessagePopupVM(ExecutionContext executionContext,string heading = "", string subHeading = "", string content = "", string cancelbuttonText = "CANCEL", string okButtonText = "OK",
            PopupType popupType = PopupType.Normal, MessageButtonsType messageType = MessageButtonsType.OK)
        {
            log.LogMethodEntry(executionContext, heading, subHeading, content, cancelButtonText, okButtonText, popupType, messageType);
            ExecutionContext = executionContext;
            InitilaizeCommands();
            this.heading = heading;
            this.subHeading = subHeading;
            this.content = content;
            if (!string.IsNullOrEmpty(cancelButtonText) && cancelButtonText.ToLower() == "CANCEL".ToLower())
            {
                this.cancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "CANCEL");
            }
            if (cancelButtonText.ToLower() == "OK".ToLower())
            {
                this.okButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK");
            }
            this.popupType = popupType;
            MessageButtonsType = messageType;

            SetOkButtonVisibiltiy();

            log.LogMethodExit();
        }
        #endregion

        #region Methods      
        private void InitilaizeCommands()
        {
            log.LogMethodEntry();
            PropertyChanged += OnPropertyChanged;
            actionsCommand = new DelegateCommand(OnActionsClicked);
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrWhiteSpace(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case "MessageButtonsType":
                        {
                            SetOkButtonVisibiltiy();
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void SetOkButtonVisibiltiy()
        {
            log.LogMethodEntry();
            OkButtonVisibility = messageType == MessageButtonsType.OK ? Visibility.Collapsed : Visibility.Visible;
            log.LogMethodExit();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PerformClose();
            log.LogMethodExit();
        }
        private void PerformClose()
        {
            log.LogMethodEntry();
            if (timer != null && timer.IsEnabled)
            {
                timer.Stop();
                timer.Tick -= TimerTick;
            }
            if (messagePopupView != null)
            {
                messagePopupView.Close();
            }
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                string actionText = parameter as string;
                GenericMessagePopupView popupView = parameter as GenericMessagePopupView;
                if (!string.IsNullOrWhiteSpace(actionText))
                {
                    switch (actionText.ToLower())
                    {
                        case "ok":
                            {
                                buttonClickType = ButtonClickType.Ok;
                            }
                            break;
                        case "cancel":
                            {
                                buttonClickType = MessageButtonsType == MessageButtonsType.OK ? ButtonClickType.Ok : ButtonClickType.Cancel;
                            }
                            break;
                    }
                    PerformClose();
                }
                else if (popupView != null)
                {
                    messagePopupView = popupView;
                    StartTimer();
                }
            }
            log.LogMethodExit();
        }
        private void StartTimer()
        {
            log.LogMethodEntry();
            if (this.PopupType == PopupType.Timer)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(this.TimerMilliSeconds);
                timer.Tick += TimerTick;
                timer.Start();
            }
            log.LogMethodExit();
        }
        #endregion
    }
}

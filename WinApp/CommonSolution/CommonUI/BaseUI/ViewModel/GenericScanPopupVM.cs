/********************************************************************************************
* Project Name - POS Redesign
* Description  - Common - view model for item info popup
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign
*2.140.0     23-Jul-2021   Prashanth V             Modified : Added IsTimerRequired Property
********************************************************************************************/
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.CommonUI
{
    class GenericScanPopupVM : ViewModelBase
    {
        public enum PopupType
        {
            SCANTICKET = 0,
            SCANGIFT = 1,
            TAPCARD = 2
        };

        #region Members
        private bool multiScreenMode;
        private bool ismultiScreenRowTwo;

        private string title;
        private PopupType popuptype;
        private DispatcherTimer timer;
        private int timerMilliSeconds = 5000;
        private bool isTimerRequired;
        private GenericScanPopupView scanPopupView;
        private ICommand loadedCommand;
        private ICommand closeCommand;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #endregion

        #region Properties
        public bool IsTimerRequired
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isTimerRequired);
                return isTimerRequired;
            }
            set
            {
                log.LogMethodEntry(isTimerRequired, value);
                SetProperty(ref isTimerRequired, value);
                log.LogMethodExit(isTimerRequired);
            }
        }
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

        public string Title
        {
            get
            {
                log.LogMethodEntry();
                return title;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref title, value);
            }
        }

        public PopupType ScanPopupType
        {
            get
            {
                log.LogMethodEntry();
                return popuptype;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref popuptype, value);
            }
        }

        public int TimerMilliSeconds
        {
            get
            {
                log.LogMethodEntry();
                return timerMilliSeconds;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref timerMilliSeconds, value);
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                return loadedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                loadedCommand = value;
            }
        }
        #endregion

        #region Constructors
        public GenericScanPopupVM(ExecutionContext executioncontext, DeviceClass CardReader = null, DeviceClass BarcodeReader = null)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executioncontext;
            loadedCommand = new DelegateCommand(OnLoaded);
            closeCommand = new DelegateCommand(OnClosed);
            ismultiScreenRowTwo = false;
            multiScreenMode = false;
            IsTimerRequired = true;
            log.LogMethodExit();
        }

        #endregion

        #region Methods
        private void OnClosed(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                Window window = parameter as Window;
                if (window != null)
                {
                    window.Close();
                }
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                scanPopupView = parameter as GenericScanPopupView;
                if (scanPopupView != null)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(this.TimerMilliSeconds);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                }
            }
            log.LogMethodExit();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
        }

        private void Close()
        {
            log.LogMethodEntry();
            if (timer != null && timer.IsEnabled)
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
            }
            if (scanPopupView != null && IsTimerRequired)
            {
                scanPopupView.Close();
            }
            log.LogMethodExit();
        }
        #endregion

    }
}

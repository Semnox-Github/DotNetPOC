/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for scan pop up
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionScanVM : ViewModelBase
    {
        #region Members
        private RedemptionMainUserControlVM redemptionMainUserControlVM;
        private int columnCount;
        private bool scanGiftButtonVisible;
        private bool scanTicketButtonVisible;
        private bool enterTicketButtonVisible;

        private RedemptionScanView redemptionScanView;
        private GenericScanPopupView scanpopupView;

        private bool multiScreenMode;

        private string heading;
        private string giftheading;
        private string enterTicketHeading;
        private string ticketheading;

        private ICommand giftCommand;
        private ICommand ticketCommand;
        private ICommand enterTicketCommand;

        private ICommand closeCommand;
        private ICommand loadedCommand;
        private DispatcherTimer timer;
        private int timerMilliSeconds;
        private PopupType popupType;
        private char scanTicketGiftMode;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        internal RedemptionMainUserControlVM RedemptionMainUserControlVM
        {
            get
            {
                return redemptionMainUserControlVM;
            }
        }
        public char ScanTicketGiftMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scanTicketGiftMode);
                return scanTicketGiftMode;
            }
            set
            {
                log.LogMethodEntry(scanTicketGiftMode, value);
                SetProperty(ref scanTicketGiftMode, value);
                log.LogMethodExit(scanTicketGiftMode);
            }
        }
        public GenericScanPopupView ScanPopupView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scanpopupView);
                return scanpopupView;
            }
            set
            {
                log.LogMethodEntry(scanpopupView, value);
                SetProperty(ref scanpopupView, value);
                log.LogMethodExit(scanpopupView);
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
                return heading;
            }
            set
            {
                log.LogMethodEntry(heading, value);
                SetProperty(ref heading, value);
                log.LogMethodExit(heading);
            }
        }

        public string EnterTicketHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enterTicketHeading);
                return enterTicketHeading;
            }
            set
            {
                log.LogMethodEntry(enterTicketHeading, value);
                SetProperty(ref enterTicketHeading, value);
                log.LogMethodExit(enterTicketHeading);
            }
        }

        public string GiftHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(giftheading);
                return giftheading;
            }
            set
            {
                log.LogMethodEntry(giftheading, value);
                SetProperty(ref giftheading, value);
                log.LogMethodExit(giftheading);
            }
        }

        public string TicketHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticketheading);
                return ticketheading;
            }
            set
            {
                log.LogMethodEntry(ticketheading, value);
                SetProperty(ref ticketheading, value);
                log.LogMethodExit(ticketheading);
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

        public ICommand GiftCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(giftCommand);
                return giftCommand;
            }
            set
            {
                log.LogMethodEntry(giftCommand, value);
                SetProperty(ref giftCommand, value);
                log.LogMethodExit(giftCommand);
            }
        }

        public ICommand TicketCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticketCommand);
                return ticketCommand;
            }
            set
            {
                log.LogMethodEntry(ticketCommand, value);
                SetProperty(ref ticketCommand, value);
                log.LogMethodExit(ticketCommand);
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadedCommand);
                return loadedCommand;
            }
            set
            {
                log.LogMethodEntry(loadedCommand, value);
                SetProperty(ref loadedCommand, value);
                log.LogMethodExit(loadedCommand);
            }
        }

        public ICommand EnterTicketCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enterTicketCommand);
                return enterTicketCommand;
            }
            set
            {
                log.LogMethodEntry(enterTicketCommand, value);
                SetProperty(ref enterTicketCommand, value);
                log.LogMethodExit(enterTicketCommand);
            }
        }

        public int ColumnCount
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(columnCount);
                if (scanGiftButtonVisible && scanTicketButtonVisible && enterTicketButtonVisible)
                {
                    columnCount = 3;
                }
                else if ((scanGiftButtonVisible && scanTicketButtonVisible) || (scanTicketButtonVisible && enterTicketButtonVisible)
                    || (scanGiftButtonVisible && enterTicketButtonVisible))
                {
                    columnCount = 2;
                }
                else if (scanGiftButtonVisible || scanGiftButtonVisible || enterTicketButtonVisible)
                {
                    columnCount = 1;
                }
                return columnCount;
            }
        }

        public bool ScanGiftButtonVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scanGiftButtonVisible);
                return scanGiftButtonVisible;
            }
            set
            {
                log.LogMethodEntry(scanGiftButtonVisible, value);
                SetProperty(ref scanGiftButtonVisible, value);
                log.LogMethodExit(scanGiftButtonVisible);
            }
        }

        public bool ScanTicketButtonVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scanTicketButtonVisible);
                return scanTicketButtonVisible;
            }
            set
            {
                log.LogMethodEntry(scanTicketButtonVisible, value);
                SetProperty(ref scanTicketButtonVisible, value);
                log.LogMethodExit(scanTicketButtonVisible);
            }
        }

        public bool EnterTicketButtonVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enterTicketButtonVisible);
                return enterTicketButtonVisible;
            }
            set
            {
                log.LogMethodEntry(enterTicketButtonVisible, value);
                SetProperty(ref enterTicketButtonVisible, value);
                log.LogMethodExit(enterTicketButtonVisible);
            }
        }
        #endregion

        #region Methods
        private void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Window window = sender as Window;
            if (redemptionScanView != null)
            {
                window.Owner = this.redemptionScanView;
                window.Width = this.redemptionScanView.ActualWidth;
                window.Height = this.redemptionScanView.ActualHeight;
                window.Top = this.redemptionScanView.Top;
                window.Left = this.redemptionScanView.Left;
            }
            log.LogMethodExit();
        }
        private void redemptionScanView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (redemptionScanView != null)
            {
                if (scanpopupView != null)
                {
                    this.OnWindowLoaded(scanpopupView, null);
                }
            }
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                redemptionScanView = parameter as RedemptionScanView;
                if (this.PopupType == PopupType.Timer)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(this.TimerMilliSeconds);
                    timer.Tick += TimerTick;
                }
                if (redemptionScanView != null)
                {
                    redemptionScanView.SizeChanged += redemptionScanView_SizeChanged;
                }
            }
            log.LogMethodExit();
        }

        private void TimerTick(object sender, EventArgs e)
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
                timer.Tick -= TimerTick;
            }
            if (redemptionScanView != null)
            {
                redemptionScanView.Close();
            }
            log.LogMethodExit();
        }

        private void OnGiftScanClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            scanTicketGiftMode = 'G';
            ShowScanPopupView(scanTicketGiftMode);
            log.LogMethodExit();
        }


        private void OnTicketScanClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            scanTicketGiftMode = 'T';
            ShowScanPopupView(scanTicketGiftMode);
            log.LogMethodExit();
        }

        private void ShowScanPopupView(char scanTicketGiftMode)
        {
            log.LogMethodEntry(scanTicketGiftMode);
            if (timer != null && timer.IsEnabled)
            {
                timer.Interval = TimeSpan.FromMilliseconds(this.TimerMilliSeconds);
            }
            if (redemptionScanView != null)
            {
                scanpopupView = new GenericScanPopupView();
                scanpopupView.Owner = redemptionScanView;
                GenericScanPopupVM scanPopupVM = new GenericScanPopupVM(this.ExecutionContext)
                {
                    TimerMilliSeconds = this.TimerMilliSeconds,
                    MultiScreenMode = this.MultiScreenMode
                };
                scanpopupView.DataContext = scanPopupVM;
                if (scanTicketGiftMode == 'T')
                {
                    scanPopupVM.Title = MessageViewContainerList.GetMessage(this.ExecutionContext, "SCAN TICKET NOW");
                    scanPopupVM.ScanPopupType = GenericScanPopupVM.PopupType.SCANTICKET;
                }
                else
                {
                    scanTicketGiftMode = 'G';
                    scanPopupVM.Title = MessageViewContainerList.GetMessage(this.ExecutionContext, "SCAN GIFT NOW");
                    scanPopupVM.ScanPopupType = GenericScanPopupVM.PopupType.SCANGIFT;
                }
                scanpopupView.Loaded += OnRedemptionScanPopupViewLoaded;
                scanpopupView.Closed += OnScanpopupViewClosed;
                redemptionScanView.ShowInTaskbar = false;
                scanpopupView.Show();
            }
            log.LogMethodExit();
        }
        private void OnScanpopupViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.RedemptionMainUserControl != null)
            {
                redemptionMainUserControlVM.RedemptionMainUserControl.Focus();
            }
            log.LogMethodExit();
        }
        private void OnRedemptionScanPopupViewLoaded(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            GenericScanPopupView scanPopupView = sender as GenericScanPopupView;
            if (scanPopupView != null)
            {
                scanPopupView.Width = redemptionScanView.ActualWidth;
                scanPopupView.Height = redemptionScanView.ActualHeight;
                scanPopupView.Top = redemptionScanView.Top;
                scanPopupView.Left = redemptionScanView.Left;
            }
            log.LogMethodExit();
        }

        private void OnEnterTicketClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                if (timer != null && timer.IsEnabled)
                {
                    timer.Interval = TimeSpan.FromMilliseconds(this.TimerMilliSeconds);
                }
                redemptionScanView = parameter as RedemptionScanView;
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.ShowFlagOrEnterTicketView();
                }
            }
            log.LogMethodExit();
        }

        private void OnClosed(object parameter)
        {
            log.LogMethodEntry(parameter);
            Close();
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public RedemptionScanVM(ExecutionContext executionContext, RedemptionMainUserControlVM redemptionMainUserControlVM)
        {
            log.LogMethodEntry();
            ExecuteAction(() =>
            {
                this.ExecutionContext = executionContext;

                scanGiftButtonVisible = true;
                scanTicketButtonVisible = true;
                enterTicketButtonVisible = true;
                this.redemptionMainUserControlVM = redemptionMainUserControlVM;
                popupType = PopupType.Timer;
                timerMilliSeconds = 5000;

                heading = MessageViewContainerList.GetMessage(executionContext, "SCAN OR ENTER");
                giftheading = MessageViewContainerList.GetMessage(executionContext, "Scan Gift");
                ticketheading = MessageViewContainerList.GetMessage(executionContext, "Scan Ticket");
                enterTicketHeading = MessageViewContainerList.GetMessage(executionContext, "Enter Ticket No.");

                loadedCommand = new DelegateCommand(OnLoaded);
                giftCommand = new DelegateCommand(OnGiftScanClicked);
                ticketCommand = new DelegateCommand(OnTicketScanClicked);
                enterTicketCommand = new DelegateCommand(OnEnterTicketClicked);
                closeCommand = new DelegateCommand(OnClosed);

                multiScreenMode = false;
            });
            log.LogMethodExit();
        }

        #endregion
    }
}

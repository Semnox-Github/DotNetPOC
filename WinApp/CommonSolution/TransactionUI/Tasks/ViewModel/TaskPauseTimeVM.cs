/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Pause Time View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     18-May-2021   Prashanth               Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    public class TaskPauseTimeVM : TaskBaseViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string timeRemaining;
        private string eticket;
        private bool eTicketVisible;
        private bool showPauseTimeScreen;
        private ICommand loaded;
        private ICommand backButtonCommand;
        private ICommand cardAddedCommand;
        private CardDetailsVM cardDetailsVM;
        #endregion

        #region Properties

        public string TimeRemaining
        {
            set
            {
                log.LogMethodEntry(timeRemaining, value);
                SetProperty(ref timeRemaining, value);
                log.LogMethodExit(timeRemaining);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(timeRemaining);
                return timeRemaining;
            }
        }

        public string ETicket
        {
            set
            {
                log.LogMethodEntry(eticket, value);
                SetProperty(ref eticket, value);
                log.LogMethodExit(eticket);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(eticket);
                return eticket;
            }
        }



        public bool EticketVisible
        {
            set
            {
                log.LogMethodEntry(eTicketVisible, value);
                SetProperty(ref eTicketVisible, value);
                log.LogMethodExit(eTicketVisible);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(eTicketVisible);
                return eTicketVisible;
            }
        }

        public bool ShowPauseTimeScreen
        {
            set
            {
                log.LogMethodEntry(showPauseTimeScreen, value);
                SetProperty(ref showPauseTimeScreen, value);
                log.LogMethodExit(showPauseTimeScreen);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showPauseTimeScreen);
                return showPauseTimeScreen;
            }
        }

        public ICommand BackButtonCommand
        {
            set
            {
                log.LogMethodEntry(backButtonCommand, value);
                SetProperty(ref backButtonCommand, value);
                log.LogMethodExit(backButtonCommand);
            }

            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(backButtonCommand);
                return backButtonCommand;
            }
        }

        public ICommand CardAddedCommand
        {
            set
            {
                log.LogMethodEntry(cardAddedCommand, value);
                SetProperty(ref cardAddedCommand, value);
                log.LogMethodExit(cardAddedCommand);
            }

            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardAddedCommand);
                return cardAddedCommand;
            }
        }

        public ICommand Loaded
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loaded);
                return loaded;
            }
            set
            {
                log.LogMethodEntry(loaded, value);
                SetProperty(ref loaded, value);
                log.LogMethodExit(loaded);
            }
        }




        public CardDetailsVM CardDetailsVM
        {
            set
            {
                log.LogMethodEntry(cardDetailsVM, value);
                SetProperty(ref cardDetailsVM, value);
                log.LogMethodExit(cardDetailsVM);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardDetailsVM);
                return cardDetailsVM;
            }
        }
        #endregion

        #region Methods

        private void OnLoadedCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            TaskPauseTimeView taskPauseTimeView = parameter as TaskPauseTimeView;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            if (!ManagerApprovalCheck(TaskType.PAUSETIMEENTITLEMENT,parameter))
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                PerformClose(parameter);
            }
            log.LogMethodExit();
        }

        private void OnBackButtonCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            PerformClose(parameter);
            log.LogMethodExit();
        }

        private void OnClearCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty,MessageType.None);
            TappedAccountDTO = null;
            CardDetailsVM.AccountDTO = null;
            ShowPauseTimeScreen = false;
            Remarks = string.Empty;
            log.LogMethodExit();
        }

        private async void OnOkCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            TaskPauseTimeView taskPauseTimeView = parameter as TaskPauseTimeView;
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId > -1)
            {
                ITaskUseCases taskUseCase = TaskUseCaseFactory.GetTaskUseCases(ExecutionContext);
                AccountTimeStatusDTO accountTimeStatusDTO = new AccountTimeStatusDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId, AccountDTO.AccountTimeStatusEnum.PAUSED, Remarks);
                try
                {
                    IsLoadingVisible = true;
                    AccountTimeStatusDTO accountsTimeStatusDTO = await taskUseCase.UpdateTimeStatus(accountTimeStatusDTO);
                    if (accountsTimeStatusDTO != null)
                    {
                        SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 1388);
                        SetFooterContent(SuccessMessage, MessageType.Info);
                        IsLoadingVisible = false;
                        PerformClose(taskPauseTimeView);
                    }
                }
                catch (ValidationException vex)
                {
                    log.Error(vex);
                    SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
                catch (UnauthorizedException uaex)
                {
                    log.Error(uaex);
                    IsLoadingVisible = false;
                    throw;
                }
                catch (ParafaitApplicationException pax)
                {
                    log.Error(pax);
                    SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
            }
            log.LogMethodExit();
        }

        private void OnCardAdded(object parameter)
        {
            log.LogMethodEntry(parameter);
            HandleCardRead();
            log.LogMethodExit();
        }

        internal void HandleCardRead()
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            if (TappedAccountDTO != null)
            {
                CardDetailsVM.AccountDTO = TappedAccountDTO;
            }
            if (CardDetailsVM.AccountDTO == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257, null), MessageType.Warning);
                return;
            }
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId >= 0)
            {
                int time = 0;
                int creditPlusTime = 0;
                if (CardDetailsVM.AccountDTO.Time != null && CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTime != null)
                {
                    time = Convert.ToInt32(CardDetailsVM.AccountDTO.Time);
                    creditPlusTime = Convert.ToInt32(CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTime);
                }
                else if (CardDetailsVM.AccountDTO.Time != null)
                {
                    time = Convert.ToInt32(CardDetailsVM.AccountDTO.Time);
                    creditPlusTime = 0;
                }
                else if (CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTime != null)
                {
                    time = 0;
                    creditPlusTime = Convert.ToInt32(CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTime);
                }
                else
                {
                    time = 0;
                    creditPlusTime = 0;
                }

                TimeRemaining = (time + creditPlusTime).ToString() + " " + MessageViewContainerList.GetMessage(ExecutionContext, "Minutes");
                int creditPlusTickets = 0;
                int ticketCount = 0;
                if (CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTickets != null && CardDetailsVM.AccountDTO.TicketCount != null)
                {
                    creditPlusTickets = Convert.ToInt32(CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTickets);
                    ticketCount = Convert.ToInt32(CardDetailsVM.AccountDTO.TicketCount);
                }
                else if (CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTickets != null)
                {
                    creditPlusTickets = Convert.ToInt32(CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTickets);
                    ticketCount = 0;
                }
                else if (CardDetailsVM.AccountDTO.TicketCount != null)
                {
                    creditPlusTickets = 0;
                    ticketCount = Convert.ToInt32(CardDetailsVM.AccountDTO.TicketCount);
                }
                else
                {
                    creditPlusTickets = 0;
                    ticketCount = 0;
                }
                if ((creditPlusTickets + ticketCount) > 0)
                {
                    ETicket = (creditPlusTickets + ticketCount).ToString() + " " + MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"));
                    EticketVisible = true;
                }
                else
                {
                    EticketVisible = false;
                }

                ShowPauseTimeScreen = true;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Balance Time will be paused", null), MessageType.Info);
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 459, null), MessageType.Warning);
            }
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (BackButtonCommand as DelegateCommand).RaiseCanExecuteChanged();
            (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ClearCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            switch (e.PropertyName)
            {
                case "IsLoadingVisible":
                    if (sender is CardDetailsVM)
                    {
                        IsLoadingVisible = cardDetailsVM.IsLoadingVisible;
                    }
                    else
                    {
                        RaiseCanExecuteChanged();
                    }
                    break;
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructors

        public TaskPauseTimeVM(ExecutionContext executionContext, DeviceClass cardReader) : base(executionContext, cardReader)
        {
            log.LogMethodEntry(executionContext, cardReader);
            ExecutionContext = executionContext;
            ShowPauseTimeScreen = false;
            CardTappedEvent += HandleCardRead;
            Loaded = new DelegateCommand(OnLoadedCommand);
            CardAddedCommand = new DelegateCommand(OnCardAdded);
            BackButtonCommand = new DelegateCommand(OnBackButtonCommand, ButtonEnable);
            ClearCommand = new DelegateCommand(OnClearCommand, ButtonEnable);
            OkCommand = new DelegateCommand(OnOkCommand,ButtonEnable);
            PropertyChanged += OnPropertyChanged;
            CardDetailsVM = new CardDetailsVM(ExecutionContext);
            CardDetailsVM.PropertyChanged += OnPropertyChanged;
            CardDetailsVM.EnableManualEntry = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "ALLOW_MANUAL_CARD_IN_POS") == "Y" ? true : false;
            log.LogMethodExit();
        }
        #endregion
    }
}

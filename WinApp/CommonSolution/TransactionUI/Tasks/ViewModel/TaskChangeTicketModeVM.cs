/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Change Ticket Mode UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     28-Mar-2021   Prashanth V            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.TransactionUI
{
    public class TaskChangeTicketModeVM : TaskBaseViewModel
    {
        #region Members
        private string enteredCardNumber;
        private CardDetailsVM cardDetailsVM;
        private TaskChangeTicketModeView taskChangeTicketModeView;
        private bool showTicketModeScreen;
        private bool isSelectedRealTicket;
        private bool realTicketMode;
        private bool eTicketMode;
        private ICommand cardAddedCommand;
        private ICommand eTicketClickedCommand;
        private ICommand realTicketClickedCommand;
        private ICommand loaded;
        private ICommand backButton;
        private string moduleName;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties

        public bool RealTicketMode
        {
            set
            {
                log.LogMethodEntry(realTicketMode, value);
                SetProperty(ref realTicketMode, value);
                log.LogMethodExit(realTicketMode);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(realTicketMode);
                return realTicketMode;
            }
        }

        public bool ETicketMode
        {
            set
            {
                log.LogMethodEntry(eTicketMode, value);
                SetProperty(ref eTicketMode, value);
                log.LogMethodExit(eTicketMode);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(eTicketMode);
                return eTicketMode;
            }
        }
        public TaskChangeTicketModeView TaskChangeTicketModeView
        {
            set
            {
                log.LogMethodEntry(taskChangeTicketModeView, value);
                SetProperty(ref taskChangeTicketModeView, value);
                log.LogMethodExit(taskChangeTicketModeView);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(taskChangeTicketModeView);
                return taskChangeTicketModeView;
            }
        }
        public ICommand Loaded
        {
            set
            {
                log.LogMethodEntry(loaded, value);
                SetProperty(ref loaded, value);
                log.LogMethodExit(loaded);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loaded);
                return loaded;
            }
        }
        public ICommand ETicketClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(eTicketClickedCommand);
                return eTicketClickedCommand;
            }
            set
            {
                log.LogMethodEntry(eTicketClickedCommand, value);
                SetProperty(ref eTicketClickedCommand, value);
                log.LogMethodExit(eTicketClickedCommand);
            }
        }

        public ICommand RealTicketClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(realTicketClickedCommand);
                return realTicketClickedCommand;
            }
            set
            {
                log.LogMethodEntry(realTicketClickedCommand, value);
                SetProperty(ref realTicketClickedCommand, value);
                log.LogMethodExit(realTicketClickedCommand);
            }
        }
        public ICommand CardAddedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardAddedCommand);
                return cardAddedCommand;
            }
            set
            {
                log.LogMethodEntry(cardAddedCommand, value);
                SetProperty(ref cardAddedCommand, value);
                log.LogMethodExit(cardAddedCommand);
            }
        }
        public string EnteredCardNumber
        {
            set
            {
                log.LogMethodEntry(enteredCardNumber, value);
                SetProperty(ref enteredCardNumber, value);
                log.LogMethodExit(enteredCardNumber);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enteredCardNumber);
                return enteredCardNumber;
            }
        }
        public bool ShowTicketModeScreen
        {
            set
            {
                log.LogMethodEntry(showTicketModeScreen, value);
                SetProperty(ref showTicketModeScreen, value);
                log.LogMethodExit(showTicketModeScreen);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showTicketModeScreen);
                return showTicketModeScreen;
            }
        }
        public bool IsSelectedRealTicket
        {
            set
            {
                log.LogMethodEntry(isSelectedRealTicket);
                SetProperty(ref isSelectedRealTicket, value);
                log.LogMethodExit(isSelectedRealTicket);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isSelectedRealTicket);
                return isSelectedRealTicket;
            }
        }
        public ICommand BackButton
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(backButton);
                return backButton;
            }
            set
            {
                log.LogMethodEntry(backButton, value);
                SetProperty(ref backButton, value);
                log.LogMethodExit(backButton);
            }
        }
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
                log.LogMethodEntry(moduleName, value);
                SetProperty(ref moduleName, value);
                log.LogMethodExit(moduleName);
            }
        }
        public CardDetailsVM CardDetailsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardDetailsVM);
                return cardDetailsVM;
            }
            set
            {
                log.LogMethodEntry(cardDetailsVM, value);
                SetProperty(ref cardDetailsVM, value);
                log.LogMethodExit(cardDetailsVM);
            }
        }
        #endregion
        #region Methods


        private void OnETicketClickedCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            IsSelectedRealTicket = false;
            RealTicketMode = false;
            log.LogMethodExit();
        }

        private void OnRealTicketClickedCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            IsSelectedRealTicket = true;
            ETicketMode = false;
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            taskChangeTicketModeView = parameter as TaskChangeTicketModeView;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            if (!ManagerApprovalCheck(TaskType.REALETICKET,parameter))
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                PerformClose(parameter);
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
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.TechnicianCard.Equals("Y"))
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 197, CardDetailsVM.AccountDTO.TagNumber), MessageType.Warning);
                CardDetailsVM.AccountDTO = null;
                return;
            }
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId >= 0)
            {
                ShowTicketModeScreen = true;
                IsSelectedRealTicket = CardDetailsVM.AccountDTO.RealTicketMode;
                if (IsSelectedRealTicket)
                {
                    RealTicketMode = true;
                    ETicketMode = false;
                }
                else
                {
                    RealTicketMode = false;
                    ETicketMode = true;
                }
                SetFooterContent(string.Empty, MessageType.None);
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 260, null), MessageType.Error);
            }
            log.LogMethodExit();
        }
        private async void OnOkButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId > -1)
            {
                ITaskUseCases taskUseCase = TaskUseCaseFactory.GetTaskUseCases(ExecutionContext);
                TicketModeType ticketMode;
                if (IsSelectedRealTicket)
                {
                    ticketMode = TicketModeType.REAL;
                }
                else
                {
                    ticketMode = TicketModeType.ETICKET;
                }
                TicketModeDTO ticketModeDTO = new TicketModeDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId, ticketMode, Remarks);
                try
                {
                    IsLoadingVisible = true;
                    TicketModeDTO ticketModesDTO = await taskUseCase.TicketModes(ticketModeDTO);
                    if (ticketModeDTO != null)
                    {
                        CardDetailsVM.AccountDTO = null;
                        ShowTicketModeScreen = false;
                        SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2996);
                        SetFooterContent(SuccessMessage, MessageType.Info);
                        IsLoadingVisible = false;
                        PerformClose(taskChangeTicketModeView);
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
        private void OnBackButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            PerformClose(parameter);
            log.LogMethodExit();
        }
        private void OnClearCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            Remarks = string.Empty;
            CardDetailsVM.AccountDTO = null;
            ShowTicketModeScreen = false;
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            SetFooterContent(string.Empty, MessageType.None);
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (BackButton as DelegateCommand).RaiseCanExecuteChanged();
            (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ClearCommand as DelegateCommand).RaiseCanExecuteChanged();
            (cardAddedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ETicketClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (RealTicketClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
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
        #region Constructor
        public TaskChangeTicketModeVM(ExecutionContext executionContext, DeviceClass cardReader) : base(executionContext, cardReader)
        {
            log.LogMethodEntry(executionContext, cardReader);
            Loaded = new DelegateCommand(OnLoaded);
            BackButton = new DelegateCommand(OnBackButtonClicked, ButtonEnable);
            OkCommand = new DelegateCommand(OnOkButtonClicked, ButtonEnable);
            ClearCommand = new DelegateCommand(OnClearCommand, ButtonEnable);
            cardAddedCommand = new DelegateCommand(OnCardAdded);
            ETicketClickedCommand = new DelegateCommand(OnETicketClickedCommand, ButtonEnable);
            RealTicketClickedCommand = new DelegateCommand(OnRealTicketClickedCommand, ButtonEnable);
            CardDetailsVM = new CardDetailsVM(ExecutionContext);
            CardDetailsVM.PropertyChanged += OnPropertyChanged;
            PropertyChanged += OnPropertyChanged;
            ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Real / e-Ticket");
            ShowTicketModeScreen = false;
            IsSelectedRealTicket = true;
            base.CardTappedEvent += HandleCardRead;
            log.LogMethodExit();
        }
        #endregion
    }
}

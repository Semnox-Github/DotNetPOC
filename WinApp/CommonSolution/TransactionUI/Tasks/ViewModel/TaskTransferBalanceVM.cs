/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Transfer Balance
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     30-Mar-2021    Raja Uthanda           Created for POS UI Redesign 
 *2.130.2     13-Dec-2021    Deeksha                Modified as part of Transfer Balance to support playcredits,CounterItems,Time and Courtesy
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    public class TaskTransferBalanceVM : TaskBaseViewModel
    {
        #region Members
        private decimal credits;
        private decimal bonus;
        private decimal tickets;
        private decimal playCredits;
        private decimal courtesy;
        private decimal time;
        private decimal counterItems;
        private bool enableManualEntry;
        private bool parentAddedManually = false;
        private TaskTransferBalanceView taskTransferBalanceView;
        private CardDetailsVM inputCardDetailsVM;
        private ObservableCollection<TaskTransferChildModel> childCardsCollection;

        private ICommand backButtonCommand;
        private ICommand cardDetailsClearCommand;
        private ICommand enterCardNoClickedCommand;
        private ICommand loadedCommand;
        private ICommand cardAddedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties       
        public bool EnableManualEntry
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableManualEntry);
                return enableManualEntry;
            }
            set
            {
                log.LogMethodEntry(enableManualEntry, value);
                SetProperty(ref enableManualEntry, value);
                log.LogMethodExit(enableManualEntry);
            }
        }
        public TaskTransferBalanceView TaskTransferBalanceView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(taskTransferBalanceView);
                return taskTransferBalanceView;
            }
            set
            {
                log.LogMethodEntry(taskTransferBalanceView, value);
                SetProperty(ref taskTransferBalanceView, value);
                log.LogMethodExit(taskTransferBalanceView);
            }
        }
        public CardDetailsVM InputCardDetailsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(inputCardDetailsVM);
                return inputCardDetailsVM;
            }
            set
            {
                log.LogMethodEntry(inputCardDetailsVM, value);
                SetProperty(ref inputCardDetailsVM, value);
                log.LogMethodExit(inputCardDetailsVM);
            }
        }
        public ObservableCollection<TaskTransferChildModel> ChildCardsCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(childCardsCollection);
                return childCardsCollection;
            }
            set
            {
                log.LogMethodEntry(childCardsCollection, value);
                SetProperty(ref childCardsCollection, value);
                log.LogMethodExit(childCardsCollection);
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
        public ICommand EnterCardNoClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enterCardNoClickedCommand);
                return enterCardNoClickedCommand;
            }
            set
            {
                log.LogMethodEntry(enterCardNoClickedCommand, value);
                SetProperty(ref enterCardNoClickedCommand, value);
                log.LogMethodExit(enterCardNoClickedCommand);
            }
        }
        public ICommand CardDetailsClearCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardDetailsClearCommand);
                return cardDetailsClearCommand;
            }
            set
            {
                log.LogMethodEntry(cardDetailsClearCommand, value);
                SetProperty(ref cardDetailsClearCommand, value);
                log.LogMethodExit(cardDetailsClearCommand);
            }
        }
        public ICommand BackButtonCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(backButtonCommand);
                return backButtonCommand;
            }
            set
            {
                log.LogMethodEntry(backButtonCommand, value);
                SetProperty(ref backButtonCommand, value);
                log.LogMethodExit(backButtonCommand);
            }
        }
        #endregion

        #region Methods
        internal void HandleCardRead()
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            if (TappedAccountDTO != null)
            {
                if (TappedAccountDTO.AccountId < 0)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 459), MessageType.Warning);
                    log.Info("Ends - BalanceTransfer() as Tapped card was a New Card");
                    return;
                }
                if (TappedAccountDTO.TechnicianCard.Equals("Y"))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 197, TappedAccountDTO.TagNumber), MessageType.Warning);
                    log.Info("Ends - BalanceTransfer() as Tapped a Technician Card(" + TappedAccountDTO.TagNumber + ")");
                    return;
                }
                if (TappedAccountDTO.SiteId != -1 && TappedAccountDTO.SiteId != ExecutionContext.GetSiteId() && !ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ALLOW_ROAMING_CARDS", false))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 133), MessageType.Warning);
                    log.Info("Ends-BalanceTransfer() as Tapped a Roaming cards");
                    return;
                }
                if (!parentAddedManually)
                {
                    if ((childCardsCollection != null && childCardsCollection.Any() && childCardsCollection.Any(x => x.CardDetailsVM.AccountDTO.AccountId == TappedAccountDTO.AccountId))
                    || (inputCardDetailsVM != null && inputCardDetailsVM.AccountDTO != null && inputCardDetailsVM.AccountDTO.AccountId == TappedAccountDTO.AccountId))
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 59), MessageType.Warning);
                        log.Info("Ends-BalanceTransfer() as Tapped cards is already addeds");
                        return;
                    }
                    if (inputCardDetailsVM.AccountDTO == null)
                    {
                        inputCardDetailsVM.AccountDTO = TappedAccountDTO;
                        SetReadOnlyDisplayValues();
                    }
                    else
                    {
                        TaskTransferChildModel childModel = new TaskTransferChildModel()
                        {
                            CardDetailsVM = new CardDetailsVM(this.ExecutionContext)
                            {
                                AccountDTO = TappedAccountDTO
                            }
                        };
                        childCardsCollection.Add(childModel);
                        childModel.PropertyChanged += OnChildModelPropertyChanged;
                        ScrollToBottom();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SetReadOnlyDisplayValues()
        {
            log.LogMethodEntry();
            if (taskTransferBalanceView != null)
            {
                string amountFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT");
                if (inputCardDetailsVM.AccountDTO != null)
                {
                    if (inputCardDetailsVM.AccountDTO.Credits != null)
                    {
                        credits += (decimal)inputCardDetailsVM.AccountDTO.Credits;
                    }
                    if (inputCardDetailsVM.AccountDTO.Bonus != null)
                    {
                        bonus += (decimal)inputCardDetailsVM.AccountDTO.Bonus;
                    }
                    if (inputCardDetailsVM.AccountDTO.TicketCount != null)
                    {
                        tickets += (decimal)inputCardDetailsVM.AccountDTO.TicketCount;
                    }
                    //if(inputCardDetailsVM.AccountDTO.CreditsPlayed != null)
                    //{
                    //    playCredits += (decimal)inputCardDetailsVM.AccountDTO.CreditsPlayed;
                    //}
                    if (inputCardDetailsVM.AccountDTO.Courtesy != null)
                    {
                        courtesy += (decimal)inputCardDetailsVM.AccountDTO.Courtesy;
                    }
                    //if (inputCardDetailsVM.AccountDTO.Time != null)
                    //{
                    //    time += (decimal)inputCardDetailsVM.AccountDTO.Time;
                    //}                  
                    if (inputCardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                    {
                        if (inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
                        {
                            credits += (decimal)inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance;
                        }
                        if (inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusBonus != null)
                        {
                            bonus += (decimal)inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusBonus;
                        }
                        if (inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTickets != null)
                        {
                            tickets += (decimal)inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTickets;
                        }
                        if(inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits != null)
                        {
                            playCredits += (decimal)inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits;
                        }
                        if(inputCardDetailsVM.AccountDTO.AccountSummaryDTO.TotalTimeBalance != null)
                        {
                            time += (decimal)inputCardDetailsVM.AccountDTO.AccountSummaryDTO.TotalTimeBalance;
                        }
                        if(inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase != null)
                        {
                            counterItems = (decimal)inputCardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase;
                        }
                    }
                }
                taskTransferBalanceView.txtInputCardTickets.Heading = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT");
                taskTransferBalanceView.txtInputCardCredits.Text = credits.ToString(amountFormat);
                taskTransferBalanceView.txtInputCardTickets.Text = tickets.ToString(amountFormat);
                taskTransferBalanceView.txtInputCardBonus.Text = bonus.ToString(amountFormat);
                taskTransferBalanceView.txtInputCardPlayCredits.Text = playCredits.ToString(amountFormat);
                taskTransferBalanceView.txtInputCardCourtesy.Text = courtesy.ToString(amountFormat);
                taskTransferBalanceView.txtInputCardTime.Text = time.ToString(amountFormat);
                taskTransferBalanceView.txtInputCardCounterItems.Text = counterItems.ToString(amountFormat);
            }
            log.LogMethodExit();
        }

        private void ScrollToBottom()
        {
            log.LogMethodEntry();
            if (taskTransferBalanceView != null)
            {
                if (taskTransferBalanceView.scvChildControls != null)
                {
                    taskTransferBalanceView.scvChildControls.ScrollToBottom();
                }
                taskTransferBalanceView.UpdateLayout();
                taskTransferBalanceView.OnContentRendered(null, null);
            }
            log.LogMethodExit();
        }

        private void OnBackClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            PerformClose(parameter);
            log.LogMethodExit();
        }

        private void OnClearClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            ResetValues();
            log.LogMethodExit();
        }

        private async void OnOkClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (inputCardDetailsVM.AccountDTO == null || inputCardDetailsVM.AccountDTO.AccountId<0)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 749), MessageType.Error);
                log.Info("Ends - BalanceTransfer() no card to transfer entitlements from");
                return;
            }
            if (childCardsCollection == null || !childCardsCollection.Any())
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 750), MessageType.Error);
                log.Info("Ends - BalanceTransfer() no card to transfer entitlements to");
                return;
            }
            if (ValidateTotal())
            {
                log.LogMethodExit();
                return;
            }
            if (parameter != null)
            {
                try
                {
                    List<BalanceTransferDTO.TransferDetailsDTO> transferDetails = new List<BalanceTransferDTO.TransferDetailsDTO>();
                    if (inputCardDetailsVM != null && inputCardDetailsVM.AccountDTO != null &&
                        childCardsCollection != null && childCardsCollection.Count > 0)
                    {
                        foreach (TaskTransferChildModel childModel in childCardsCollection)
                        {
                            if (childModel != null && childModel.CardDetailsVM != null
                                && childModel.CardDetailsVM.AccountDTO != null)
                            {
                                Dictionary<RedeemEntitlementDTO.FromTypeEnum, decimal> entitles = new Dictionary<RedeemEntitlementDTO.FromTypeEnum, decimal>();
                                Dictionary<string, decimal> additionalEntitles = new Dictionary<string, decimal>();
                                decimal bonus = 0;
                                decimal credit = 0;
                                decimal ticket = 0;
                                decimal courtesy = 0;
                                decimal playCredits = 0;
                                decimal counterItems = 0;
                                decimal time = 0;
                                if (childModel.BonusToBeTransfer != null)
                                {
                                    decimal.TryParse(childModel.BonusToBeTransfer, out bonus);
                                }
                                if (childModel.CreditToBeTransfer != null)
                                {
                                    decimal.TryParse(childModel.CreditToBeTransfer, out credit);
                                }
                                if (childModel.TicketsToBeTransfer != null)
                                {
                                    decimal.TryParse(childModel.TicketsToBeTransfer, out ticket);
                                }
                                if (childModel.PlayCreditsToBeTransfer != null)
                                {
                                    decimal.TryParse(childModel.PlayCreditsToBeTransfer, out playCredits);
                                }
                                if (childModel.CounterItemstoBeTransfer != null)
                                {
                                    decimal.TryParse(childModel.CounterItemstoBeTransfer, out counterItems);
                                }
                                if (childModel.TimeToBeTransfer != null)
                                {
                                    decimal.TryParse(childModel.TimeToBeTransfer, out time);
                                }
                                if (childModel.CourtesyToBeTransfer != null)
                                {
                                    decimal.TryParse(childModel.CourtesyToBeTransfer, out courtesy);
                                }
                                entitles.Add(RedeemEntitlementDTO.FromTypeEnum.BONUS, Convert.ToDecimal(bonus));
                                entitles.Add(RedeemEntitlementDTO.FromTypeEnum.CREDITS, Convert.ToDecimal(credit));
                                entitles.Add(RedeemEntitlementDTO.FromTypeEnum.TICKETS, Convert.ToDecimal(ticket));
                                entitles.Add(RedeemEntitlementDTO.FromTypeEnum.COURTESY, Convert.ToDecimal(courtesy));
                                entitles.Add(RedeemEntitlementDTO.FromTypeEnum.TIME, Convert.ToDecimal(time));
                                entitles.Add(RedeemEntitlementDTO.FromTypeEnum.PLAYCREDITS, Convert.ToDecimal(playCredits));
                                entitles.Add(RedeemEntitlementDTO.FromTypeEnum.COUNTERITEMS, Convert.ToDecimal(counterItems));
                                transferDetails.Add(new BalanceTransferDTO.TransferDetailsDTO(childModel.CardDetailsVM.AccountDTO.AccountId, entitles));
                            }
                        }
                        IsLoadingVisible = true;
                        LocalTaskUseCases taskUseCases = new LocalTaskUseCases(this.ExecutionContext);
                        BalanceTransferDTO balanceTransferDTO = await taskUseCases.BalanceTransfer(new BalanceTransferDTO(this.inputCardDetailsVM.AccountDTO.AccountId,ManagerId, transferDetails,Remarks));
                        IsLoadingVisible = false;
                        SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 798);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    IsLoadingVisible = false;
                }
                finally
                {
                    IsLoadingVisible = false;
                    PerformClose(parameter);
                }
            }
            log.LogMethodExit();
        }

        private void OnCardDetailsClearClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            if (parameter != null)
            {
                TaskTransferChildModel taskBaseViewModel = parameter as TaskTransferChildModel;
                if (taskBaseViewModel != null && ChildCardsCollection.Contains(taskBaseViewModel))
                {
                    ChildCardsCollection.Remove(taskBaseViewModel);
                    ValidateTotal();
                }
            }
            log.LogMethodExit();
        }

        private void ResetValues()
        {
            log.LogMethodEntry();
            Remarks = string.Empty;
            if (InputCardDetailsVM != null)
            {
                InputCardDetailsVM.AccountDTO = null;
                credits = 0;
                tickets = 0;
                bonus = 0;
                playCredits = 0;
                courtesy = 0;
                time = 0;
                counterItems = 0;
                SetReadOnlyDisplayValues();
            }
            if (ChildCardsCollection != null)
            {
                ChildCardsCollection.Clear();
            }
            if (TappedAccountDTO != null)
            {
                TappedAccountDTO = null;
            }
            log.LogMethodExit();
        }

        private void OnEnterCardNumberClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            GenericDataEntryView dataEntryView = new GenericDataEntryView();
            GenericDataEntryVM dataEntryVM = new GenericDataEntryVM(ExecutionContext)
            {
                Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "Enter Card No"),
                DataEntryCollections = new ObservableCollection<DataEntryElement>()
                    {
                        new DataEntryElement()
                        {
                            Heading=MessageViewContainerList.GetMessage(this.ExecutionContext,"Card No"),
                            Type = DataEntryType.TextBox,
                            DefaultValue = MessageViewContainerList.GetMessage(this.ExecutionContext,"Enter Card No"),
                            IsMandatory = true
                        }
                    }
            };
            dataEntryView.Width = SystemParameters.PrimaryScreenWidth;
            dataEntryView.Height = SystemParameters.PrimaryScreenHeight;
            dataEntryView.DataContext = dataEntryVM;
            if (parameter != null)
            {
                dataEntryView.Owner = parameter as Window;
            }
            dataEntryView.ShowDialog();
            if (dataEntryVM.ButtonClickType == ButtonClickType.Ok)
            {
                SetAccountsDTO(dataEntryVM.DataEntryCollections[0].Text);
            }
            log.LogMethodExit();
        }

        private async void SetAccountsDTO(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            try
            {
                IsLoadingVisible = true;
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(ExecutionContext);
                AccountDTOCollection accountDTOCollection = await accountUseCases.GetAccounts(accountNumber: cardNumber, tagSiteId: ExecutionContext.GetSiteId(), buildChildRecords: true, activeRecordsOnly: true);
                if (accountDTOCollection != null && accountDTOCollection.data != null)
                {
                    TappedAccountDTO = accountDTOCollection.data[0];
                    HandleCardRead();
                }
                else
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 172), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 172) + " " + ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoadingVisible = false;
            }
            log.LogMethodExit();
        }
        private bool ValidateTotal()
        {
            log.LogMethodEntry();
            bool isExceed = false;
            string message = string.Empty;
            MessageType messageType = MessageType.None;
            List<string> failedConditions = new List<string>();
            decimal result = 0;
            if (credits < childCardsCollection.Where(
                            c => !string.IsNullOrEmpty(c.CreditToBeTransfer) && decimal.TryParse(c.CreditToBeTransfer, out result)).
                            Sum(s => Convert.ToDecimal(s.CreditToBeTransfer)))
            {
                failedConditions.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "credit"));
            }
            if (bonus < childCardsCollection.Where(
                            c => !string.IsNullOrEmpty(c.BonusToBeTransfer) && decimal.TryParse(c.BonusToBeTransfer, out result)).
                            Sum(s => Convert.ToDecimal(s.BonusToBeTransfer)))
            {
                failedConditions.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "bonus"));
            }
            if (tickets < childCardsCollection.Where(
                            c => !string.IsNullOrEmpty(c.TicketsToBeTransfer) && decimal.TryParse(c.TicketsToBeTransfer, out result)).
                            Sum(s => Convert.ToDecimal(s.TicketsToBeTransfer)))
            {
                failedConditions.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "ticket"));
            }
            if(playCredits < childCardsCollection.Where(
                              c=> !string.IsNullOrEmpty(c.PlayCreditsToBeTransfer) && decimal.TryParse(c.PlayCreditsToBeTransfer, out result)).
                              Sum(s => Convert.ToDecimal(s.PlayCreditsToBeTransfer)))
            {
                failedConditions.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "play credit"));
            }
            if (time < childCardsCollection.Where(
                               c => !string.IsNullOrEmpty(c.TimeToBeTransfer) && decimal.TryParse(c.TimeToBeTransfer, out result)).
                               Sum(s => Convert.ToDecimal(s.TimeToBeTransfer)))
            {
                failedConditions.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "time"));
            }
            if (courtesy < childCardsCollection.Where(
                               c => !string.IsNullOrEmpty(c.CourtesyToBeTransfer) && decimal.TryParse(c.CourtesyToBeTransfer, out result)).
                               Sum(s => Convert.ToDecimal(s.CourtesyToBeTransfer)))
            {
                failedConditions.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "courtesy"));
            }
            if (counterItems < childCardsCollection.Where(
                               c => !string.IsNullOrEmpty(c.CounterItemstoBeTransfer) && decimal.TryParse(c.CounterItemstoBeTransfer, out result)).
                               Sum(s => Convert.ToDecimal(s.CounterItemstoBeTransfer)))
            {
                failedConditions.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "CounterItems"));
            }
            if (failedConditions.Count > 0)
            {
                message = MessageViewContainerList.GetMessage(this.ExecutionContext, 746);
                messageType = MessageType.Error;
                isExceed = true;
            }
            SetFooterContent(message, messageType);
            log.LogMethodExit();
            return isExceed;
        }

        private void OnChildModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                ValidateTotal();
            }
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                taskTransferBalanceView = parameter as TaskTransferBalanceView;
                if (taskTransferBalanceView != null)
                {
                    if (ManagerApprovalCheck(TaskType.BALANCETRANSFER,parameter))
                    {
                        IsRemarkMandatory = false;
                        if (IsRemarkMandatory)
                        {
                            taskTransferBalanceView.txtRemarks.DefaultValue = MessageViewContainerList.GetMessage(this.ExecutionContext, "Enter Remarks")
                                + " (" + MessageViewContainerList.GetMessage(this.ExecutionContext, "Mandatory") + ")";
                        }
                        else
                        {
                            taskTransferBalanceView.txtRemarks.DefaultValue = MessageViewContainerList.GetMessage(this.ExecutionContext, "Enter Remarks")
                                + " (" + MessageViewContainerList.GetMessage(this.ExecutionContext, "Optional") + ")";
                        }
                        SetReadOnlyDisplayValues();
                    }
                    else
                    {
                        ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                        PerformClose(parameter);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void OnCardAdded(object parameter)
        {
            log.LogMethodEntry(parameter);
            parentAddedManually = true;
            TappedAccountDTO = inputCardDetailsVM.AccountDTO;
            HandleCardRead();
            parentAddedManually = false;
            if (parameter != null)
            {
                CardDetailsUserControl detailsUserControl = parameter as CardDetailsUserControl;
                if (detailsUserControl != null && !string.IsNullOrEmpty(detailsUserControl.Name)
                    && detailsUserControl.Name == "ParentCardDetailsUserControl")
                {
                    SetReadOnlyDisplayValues();
                }
            }
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (enterCardNoClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (backButtonCommand as DelegateCommand).RaiseCanExecuteChanged();
            (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ClearCommand as DelegateCommand).RaiseCanExecuteChanged();
            (cardDetailsClearCommand as DelegateCommand).RaiseCanExecuteChanged();
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
                        IsLoadingVisible = inputCardDetailsVM.IsLoadingVisible;
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
        public TaskTransferBalanceVM(ExecutionContext executioncontext, DeviceClass cardReader) : base(executioncontext, cardReader)
        {
            log.LogMethodEntry(executioncontext, cardReader);
            credits = 0;
            bonus = 0;
            tickets = 0;
            playCredits = 0;
            courtesy = 0;
            time = 0;
            counterItems = 0;
            ShowRemark = false;
            enableManualEntry = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ALLOW_MANUAL_CARD_IN_POS", false);
            PropertyChanged += OnPropertyChanged;
            inputCardDetailsVM = new CardDetailsVM(this.ExecutionContext);
            inputCardDetailsVM.PropertyChanged += OnPropertyChanged;
            childCardsCollection = new ObservableCollection<TaskTransferChildModel>();

            enterCardNoClickedCommand = new DelegateCommand(OnEnterCardNumberClicked, ButtonEnable);
            backButtonCommand = new DelegateCommand(OnBackClicked, ButtonEnable);
            cardDetailsClearCommand = new DelegateCommand(OnCardDetailsClearClicked, ButtonEnable);
            loadedCommand = new DelegateCommand(OnLoaded);
            ClearCommand = new DelegateCommand(OnClearClicked, ButtonEnable);
            OkCommand = new DelegateCommand(OnOkClicked,ButtonEnable);
            cardAddedCommand = new DelegateCommand(OnCardAdded);
            base.CardTappedEvent += HandleCardRead;
            log.LogMethodExit();
        }
        #endregion
    }
}

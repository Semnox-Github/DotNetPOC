/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Card Details view model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     18-Mar-2021   Amitha Joy            Created for POS UI Redesign 
 *2.140.0     30-Apr-2021   Deeksha               Modified as part of Transfer balance UI changes
 ********************************************************************************************/
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.AccountsUI
{
    public class CardDetailsVM : ViewModelBase
    {
        #region Members
        private AccountDTO accountDTO;
        private string accounGamebalance;
        private CardDetailsUserControl cardDetailsUserControl;
        //private bool addCreditPlusInfo;
        private bool enableManualEntry;
        private ICommand enterCardNoClickedCommand;
        private ICommand loadedCommand;
        private ICommand cardDetailClickedCommand;
        private ICommand customerDetailsClickedCommand;
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


        public string AccountGameBalance
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(accounGamebalance);
                return accounGamebalance;
            }
            set
            {
                log.LogMethodEntry(accounGamebalance, value);
                SetProperty(ref accounGamebalance, value);
                log.LogMethodExit(accounGamebalance);
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

        public ICommand CardDetailClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardDetailClickedCommand);
                return cardDetailClickedCommand;
            }
            set
            {
                log.LogMethodEntry(cardDetailClickedCommand, value);
                SetProperty(ref cardDetailClickedCommand, value);
                log.LogMethodExit(cardDetailClickedCommand);
            }
        }

        public ICommand CustomerDetailsClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customerDetailsClickedCommand);
                return customerDetailsClickedCommand;
            }
            set
            {
                log.LogMethodEntry(customerDetailsClickedCommand, value);
                SetProperty(ref customerDetailsClickedCommand, value);
                log.LogMethodExit(customerDetailsClickedCommand);
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

        public AccountDTO AccountDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(accountDTO);
                return accountDTO;
            }
            set
            {
                log.LogMethodEntry(accountDTO, value);
                SetProperty(ref accountDTO, value);
                SetAccountDTOValues();
                log.LogMethodExit(accountDTO);
            }
        }
        #endregion

        #region Methods

        private void InitializeCommands()
        {
            log.LogMethodEntry();
            loadedCommand = new DelegateCommand(OnLoaded);
            customerDetailsClickedCommand = new DelegateCommand(OnCustomerDetailsClicked);
            cardDetailClickedCommand = new DelegateCommand(OnCardDetailClicked);
            enterCardNoClickedCommand = new DelegateCommand(OnEnterCardNumberClicked, ButtonEnable);
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                cardDetailsUserControl = parameter as CardDetailsUserControl;
            }
            SetAccountDTOValues();
            log.LogMethodExit();
        }

        private void SetAccountDTOValues()
        {
            log.LogMethodEntry();
            if (cardDetailsUserControl != null && accountDTO != null)
            {
                if (accountDTO.ExpiryDate == null || accountDTO.ExpiryDate.Equals(DateTime.MinValue))
                {
                    if (accountDTO.IssueDate != null)
                    {
                        DateTime issueDate = (DateTime)accountDTO.IssueDate;
                        cardDetailsUserControl.tbDate.Text = issueDate.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT").Replace("yyyy", "yy").Replace(":ss", ""));
                    }
                    else
                    {
                        cardDetailsUserControl.tbDate.Text = null;
                    }
                }
                else
                {
                    if (accountDTO.ExpiryDate != null)
                    {
                        DateTime expiryDate = (DateTime)accountDTO.ExpiryDate;
                        cardDetailsUserControl.tbDate.Text = expiryDate.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT").Replace("yyyy", "yy").Replace(":ss", ""));
                    }
                }
                if (accountDTO.AccountId < 0)
                {
                    cardDetailsUserControl.tbStatus.Text = MessageViewContainerList.GetMessage(ExecutionContext, "New Card");
                }
                else if (accountDTO.ExpiryDate == null || accountDTO.ExpiryDate.Equals(DateTime.MinValue) || accountDTO.ExpiryDate > DateTime.Now)
                {
                    cardDetailsUserControl.tbStatus.Text = MessageViewContainerList.GetMessage(ExecutionContext, "Issued Card");
                }
                else
                {
                    cardDetailsUserControl.tbStatus.Text = MessageViewContainerList.GetMessage(ExecutionContext, "Expired Card");
                }
                if (accountDTO.VipCustomer)
                {
                    cardDetailsUserControl.tbMemberShip.Text = MessageViewContainerList.GetMessage(ExecutionContext, "VIP") + (!string.IsNullOrEmpty(accountDTO.MembershipName) ? "-" : "") + accountDTO.MembershipName;
                }
                else
                {
                    cardDetailsUserControl.tbMemberShip.Text = !string.IsNullOrEmpty(accountDTO.MembershipName) ? accountDTO.MembershipName : MessageViewContainerList.GetMessage(ExecutionContext, "Normal");
                }
                if (!string.IsNullOrEmpty(accountDTO.CustomerName) && !string.IsNullOrWhiteSpace(accountDTO.CustomerName))
                {
                    cardDetailsUserControl.tbCustomerName.Text = accountDTO.CustomerName;
                }
                else
                {
                    cardDetailsUserControl.tbCustomerName.Text = accountDTO.TagNumber;
                }
                string amountFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT");
                //if (addCreditPlusInfo)
                //{
                double totalCredits = 0;
                double totalTime = 0;
                double totalBonus = 0;
                double courtesy = 0;
                if (accountDTO.Credits != null)
                {
                    totalCredits += (double)accountDTO.Credits;
                }
                //if (accountDTO.Time != null)
                //{
                //    totalTime += (double)accountDTO.Time;
                //}
                if (accountDTO.Bonus != null)
                {
                    totalBonus += (double)accountDTO.Bonus;
                }
                if (accountDTO.Courtesy != null)
                {
                    courtesy += (double)accountDTO.Courtesy;
                }
                if (accountDTO.AccountSummaryDTO != null)
                {
                    if (accountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
                    {
                        totalCredits += (double)accountDTO.AccountSummaryDTO.CreditPlusCardBalance;
                    }
                    //if (accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits != null)
                    //{
                    //    totalCredits += (double)accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits;
                    //}
                    //if (accountDTO.AccountSummaryDTO.CreditPlusItemPurchase != null)
                    //{
                    //    totalCredits += (double)accountDTO.AccountSummaryDTO.CreditPlusItemPurchase;
                    //}
                    if (accountDTO.AccountSummaryDTO.TotalTimeBalance != null)
                    {
                        totalTime += (double)accountDTO.AccountSummaryDTO.TotalTimeBalance;
                    }
                    if (accountDTO.AccountSummaryDTO.CreditPlusBonus != null)
                    {
                        totalBonus += (double)accountDTO.AccountSummaryDTO.CreditPlusBonus;
                    }
                    AccountGameBalance = accountDTO.AccountSummaryDTO.AccountGameBalance.ToString(amountFormat);
                }
                cardDetailsUserControl.tbCreditsValue.Text = totalCredits.ToString(amountFormat);
                cardDetailsUserControl.tbTimeValue.Text = totalTime.ToString(amountFormat);
                cardDetailsUserControl.tbBonusValue.Text = totalBonus.ToString(amountFormat);
                cardDetailsUserControl.tbCourtesyValue.Text = courtesy.ToString(amountFormat);
                //}
                //else
                //{
                //    cardDetailsUserControl.tbCreditsValue.Text = ((decimal)((accountDTO.Credits==null?0: accountDTO.Credits))).ToString(amountFormat);
                //    cardDetailsUserControl.tbBonusValue.Text = ((decimal)((accountDTO.Bonus==null?0: accountDTO.Bonus))).ToString(amountFormat);
                //    cardDetailsUserControl.tbTimeValue.Text = ((decimal)((accountDTO.Time==null?0: accountDTO.Time))).ToString(amountFormat);
                //}
                //string numberFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT");
                cardDetailsUserControl.tbTickets.Text = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT");
                decimal ticketsValue = 0;
                decimal playCreditsValue = 0;
                decimal pointsValue = 0;
                decimal counterItemsValue = 0;
                decimal cardDepositeValue = 0;
                if (accountDTO.TicketCount != null)
                {
                    ticketsValue += (decimal)accountDTO.TicketCount;
                }
                if (accountDTO.LoyaltyPoints != null)
                {
                    pointsValue += (decimal)accountDTO.LoyaltyPoints;
                }
                if (accountDTO.FaceValue != null)
                {
                    cardDepositeValue = ((decimal)accountDTO.FaceValue);
                }
                else
                {
                    cardDepositeValue = 0;
                }
                cardDetailsUserControl.tbCardDepositValue.Text = cardDepositeValue.ToString(amountFormat);
                if (accountDTO.AccountSummaryDTO != null)
                {
                    if (accountDTO.AccountSummaryDTO.CreditPlusTickets != null)
                    {
                        ticketsValue += (decimal)accountDTO.AccountSummaryDTO.CreditPlusTickets;
                    }
                    //if (accountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
                    //{
                    //    creditsPlusValue += (decimal)accountDTO.AccountSummaryDTO.CreditPlusCardBalance;
                    //}
                    if (accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits != null)
                    {
                        playCreditsValue = (decimal)accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits;
                    }
                    if (accountDTO.AccountSummaryDTO.CreditPlusItemPurchase != null)
                    {
                        counterItemsValue += (decimal)accountDTO.AccountSummaryDTO.CreditPlusItemPurchase;
                    }
                    if (accountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints != null)
                    {
                        pointsValue += (decimal)accountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints;
                    }
                }
                cardDetailsUserControl.tbCounterItemsValue.Text = counterItemsValue.ToString(amountFormat);
                cardDetailsUserControl.tbTicketsValue.Text = ticketsValue.ToString(amountFormat);
                cardDetailsUserControl.tbPlayCreditsValue.Text = playCreditsValue.ToString(amountFormat);
                cardDetailsUserControl.tbPointsValue.Text = pointsValue.ToString(amountFormat);
            }
            log.LogMethodExit();
        }

        private void OnCustomerDetailsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit();
        }

        private void OnCardDetailClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                string name = (parameter as UniformGrid).Name;
                if (!string.IsNullOrEmpty(name))
                {
                    switch (name)
                    {
                        case "CreditUniformGrid":
                            {

                            }
                            break;
                        case "TicketsUniformGrid":
                            {

                            }
                            break;
                        case "TimeUniformGrid":
                            {

                            }
                            break;
                        case "GameUniformGrid":
                            {

                            }
                            break;
                        case "PointsUniformGrid":
                            {

                            }
                            break;
                        case "PlayCreditsUniformGrid":
                            {

                            }
                            break;
                        case "BonusUniformGrid":
                            {

                            }
                            break;
                        case "CourtesyUniformGrid":
                            {

                            }
                            break;
                        case "CounterItemsUniformGrid":
                            {

                            }
                            break;
                        case "RecSpentUniformGrid":
                            {

                            }
                            break;
                        case "OpenTransUniformGrid":
                            {

                            }
                            break;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void OnEnterCardNumberClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDataEntryView dataEntryView = new GenericDataEntryView();
                GenericDataEntryVM dataEntryVM = new GenericDataEntryVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Enter Card No"),
                    DataEntryCollections = new ObservableCollection<DataEntryElement>()
                {
                    new DataEntryElement()
                    {
                        Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Card No"),
                        Type = DataEntryType.TextBox,
                        DefaultValue = MessageViewContainerList.GetMessage(ExecutionContext,"Enter Card No"),
                        IsMandatory = true
                    }
                }
                };
                dataEntryView.Width = SystemParameters.PrimaryScreenWidth;
                dataEntryView.Height = SystemParameters.PrimaryScreenHeight;
                dataEntryView.DataContext = dataEntryVM;
                if (parameter != null)
                {
                    dataEntryView.Owner = (Window)PresentationSource.FromVisual(parameter as CardDetailsUserControl).RootVisual as Window;
                }
                dataEntryView.ShowDialog();
                if (dataEntryVM.ButtonClickType == ButtonClickType.Ok)
                {
                    SetAccountsDTO(dataEntryVM.DataEntryCollections[0].Text, parameter);
                }
            }
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (enterCardNoClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            switch (e.PropertyName)
            {
                case "IsLoadingVisible":
                    RaiseCanExecuteChanged();
                    break;
            }
            log.LogMethodExit();
        }
        private async void SetAccountsDTO(string cardNumber, object parameter)
        {
            log.LogMethodEntry(cardNumber, parameter);
            try
            {
                IsLoadingVisible = true;
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(ExecutionContext);
                AccountDTOCollection accountDTOCollection = await accountUseCases.GetAccounts(accountNumber: cardNumber, tagSiteId: ExecutionContext.GetSiteId(), buildChildRecords: true, activeRecordsOnly: true);
                if (accountDTOCollection != null && accountDTOCollection.data != null)
                {
                    AccountDTO = accountDTOCollection.data[0];
                    if (cardDetailsUserControl != null)
                    {
                        cardDetailsUserControl.RaiseCardAddedEvent();
                    }
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                AccountDTO = null;
                GenericMessagePopupView genericMessagePopupView = new GenericMessagePopupView();
                GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                {
                    OkButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                    MessageButtonsType = MessageButtonsType.OK,
                    SubHeading = MessageViewContainerList.GetMessage(ExecutionContext, 172),
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, 172),
                    Content = MessageViewContainerList.GetMessage(ExecutionContext, 172) + " " + ex.Message
                };
                genericMessagePopupView.DataContext = genericMessagePopupVM;
                if (parameter != null)
                {
                    genericMessagePopupView.Owner = (Window)PresentationSource.FromVisual(parameter as CardDetailsUserControl).RootVisual as Window;
                }
                genericMessagePopupView.ShowDialog();
            }
            finally
            {
                IsLoadingVisible = false;
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public CardDetailsVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            PropertyChanged += OnPropertyChanged;
            accountDTO = null;
            //this.addCreditPlusInfo = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ADD_CREDITPLUS_IN_CARD_INFO", false);
            this.enableManualEntry = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ALLOW_MANUAL_CARD_IN_POS", false);
            InitializeCommands();
            log.LogMethodExit();
        }
        public CardDetailsVM(ExecutionContext executionContext, AccountDTO accountDTO, bool addCreditPlusInfo,
            bool enableManualEntry)
        {
            log.LogMethodEntry(accountDTO, accountDTO, addCreditPlusInfo, enableManualEntry);
            this.ExecutionContext = executionContext;
            PropertyChanged += OnPropertyChanged;
            this.accountDTO = accountDTO;
            //this.addCreditPlusInfo = addCreditPlusInfo;
            this.enableManualEntry = enableManualEntry;
            InitializeCommands();
            log.LogMethodExit();
        }
        #endregion
    }
}
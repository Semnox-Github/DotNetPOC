using System;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.AccountsUI
{
    public class TransactionCardDetailsVM : ViewModelBase
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool addCreditPlusInfo;
        private bool expand;
        private bool isDeleteButtonVisible; // added by prashanth

        private string dateTitle;

        private AccountDTO accountDTO;
        private TransactionCardDetailsUserControl transactionCardDetailsUserControl;

        private ICommand loadedCommand;
        private ICommand deleteCommand;
        private ICommand headerClickedCommand;
        private ICommand cardDetailClickedCommand;
        #endregion

        #region Properties
        public string DateTitle
        {
            get { return dateTitle; }
            set
            {
                SetProperty(ref dateTitle, value);
            }
        }
        // added IsDeleteButtonVisible property - by prashanth
        public bool IsDeleteButtonVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isDeleteButtonVisible);
                return isDeleteButtonVisible;
            }
            set
            {
                log.LogMethodEntry(isDeleteButtonVisible, value);
                SetProperty(ref isDeleteButtonVisible, value);
                log.LogMethodExit(isDeleteButtonVisible);
            }
        }
        public bool Expand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(expand);
                return expand;
            }
            set
            {
                log.LogMethodEntry(expand, value);
                SetProperty(ref expand, value);
                log.LogMethodExit(expand);
            }
        }
        public ICommand HeaderClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(headerClickedCommand);
                return headerClickedCommand;
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
        }
        public ICommand CardDetailClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardDetailClickedCommand);
                return cardDetailClickedCommand;
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deleteCommand);
                return deleteCommand;
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
            deleteCommand = new DelegateCommand(OnDeleteClicked);
            cardDetailClickedCommand = new DelegateCommand(OnCardDetailClicked);
            headerClickedCommand = new DelegateCommand(OnHeaderClicked);
            log.LogMethodExit();
        }
        private void OnHeaderClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            Expand = !expand;
            if (transactionCardDetailsUserControl != null)
            {
                transactionCardDetailsUserControl.RaiseHeaderClickedEvent();
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                transactionCardDetailsUserControl = parameter as TransactionCardDetailsUserControl;
            }
            SetAccountDTOValues();
            log.LogMethodExit();
        }
        private void SetAccountDTOValues()
        {
            log.LogMethodEntry();
            if (transactionCardDetailsUserControl != null && accountDTO != null)
            {
                string dataTimeFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT").Replace("yyyy", "yy").Replace(":ss", "");
                if ((accountDTO.ExpiryDate == null || accountDTO.ExpiryDate.Equals(DateTime.MinValue)) && accountDTO.IssueDate != null)
                {
                    DateTitle = MessageViewContainerList.GetMessage(ExecutionContext, "Issued On - ");
                    DateTime issueDate = (DateTime)accountDTO.IssueDate;
                    transactionCardDetailsUserControl.tbDate.Text = issueDate.ToString(dataTimeFormat);
                }
                else if (accountDTO.ExpiryDate != null)
                {
                    DateTitle = MessageViewContainerList.GetMessage(ExecutionContext, "Valid Till - ");
                    DateTime expiryDate = (DateTime)accountDTO.ExpiryDate;
                    transactionCardDetailsUserControl.tbDate.Text = expiryDate.ToString(dataTimeFormat);
                }
                if (accountDTO.AccountId < 0)
                {
                    transactionCardDetailsUserControl.tbStatus.Text = MessageViewContainerList.GetMessage(ExecutionContext, "New Card");
                }
                else if (accountDTO.ExpiryDate == null || accountDTO.ExpiryDate.Equals(DateTime.MinValue) || accountDTO.ExpiryDate > DateTime.Now)
                {
                    transactionCardDetailsUserControl.tbStatus.Text = MessageViewContainerList.GetMessage(ExecutionContext, "Issued Card");
                }
                else
                {
                    transactionCardDetailsUserControl.tbStatus.Text = MessageViewContainerList.GetMessage(ExecutionContext, "Expired Card");
                }
                string amountFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT");
                //if (addCreditPlusInfo)
                //{
                double totalCredits = accountDTO.Credits != null ? (double)accountDTO.Credits : 0;
                // double totalTime = accountDTO.Time != null ? (double)accountDTO.Time : 0;
                double totalTime = 0;
                 double totalBonus = accountDTO.Bonus != null ? (double)accountDTO.Bonus : 0;
                double courtesy = accountDTO.Courtesy != null ? (double)accountDTO.Courtesy : 0;
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
                }
                transactionCardDetailsUserControl.tbCreditsValue.Text = totalCredits.ToString(amountFormat);
                transactionCardDetailsUserControl.tbTimeValue.Text = totalTime.ToString(amountFormat);
                transactionCardDetailsUserControl.tbBonusValue.Text = totalBonus.ToString(amountFormat);
                transactionCardDetailsUserControl.tbCourtesyValue.Text = courtesy.ToString(amountFormat);
                //}
                //else
                //{
                //    transactionCardDetailsUserControl.tbCreditsValue.Text = accountDTO.Credits != null ? ((decimal)accountDTO.Credits).ToString(amountFormat) : string.Empty;
                //    transactionCardDetailsUserControl.tbBonusValue.Text = accountDTO.Bonus != null ? ((decimal)accountDTO.Bonus).ToString(amountFormat) : string.Empty;
                //    transactionCardDetailsUserControl.tbTimeValue.Text = accountDTO.Time != null ? ((decimal)accountDTO.Time).ToString(amountFormat) : string.Empty;
                //}
                //string numberFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT");
                transactionCardDetailsUserControl.tbTickets.Text = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT");
                decimal playCreditsValue = 0;
                decimal counterItemsValue = 0;
                decimal ticketsValue = accountDTO.TicketCount != null ? (decimal)accountDTO.TicketCount : 0;
                decimal pointsValue = accountDTO.LoyaltyPoints != null ? (decimal)accountDTO.LoyaltyPoints : 0;
                if (accountDTO.FaceValue != null)
                {
                    transactionCardDetailsUserControl.tbCardDepositValue.Text = ((decimal)accountDTO.FaceValue).ToString(amountFormat);
                }
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
                    //if (accountDTO.AccountSummaryDTO.CreditPlusBonus != null && !addCreditPlusInfo)
                    //{
                    //    creditsPlusValue += (decimal)accountDTO.AccountSummaryDTO.CreditPlusBonus;
                    //}
                    if (accountDTO.AccountSummaryDTO.CreditPlusItemPurchase != null)
                    {
                        counterItemsValue = (decimal)accountDTO.AccountSummaryDTO.CreditPlusItemPurchase;
                    }
                    if (accountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints != null)
                    {
                        pointsValue += (decimal)accountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints;
                    }
                }
                transactionCardDetailsUserControl.tbTicketsValue.Text = ticketsValue.ToString(amountFormat);
                transactionCardDetailsUserControl.tbPlayCreditsValue.Text = playCreditsValue.ToString(amountFormat);
                transactionCardDetailsUserControl.tbCounterItemsValue.Text = counterItemsValue.ToString(amountFormat);
                transactionCardDetailsUserControl.tbPointsValue.Text = pointsValue.ToString(amountFormat);
            }
            log.LogMethodExit();
        }

        private void OnDeleteClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (transactionCardDetailsUserControl != null)
            {
                transactionCardDetailsUserControl.RaiseDeleteClickedEvent();
            }
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
                        case "CreditPlusUniformGrid":
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
        #endregion

        #region Constructor
        public TransactionCardDetailsVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ExecutionContext = executionContext;
            accountDTO = null;
            expand = true;
            IsDeleteButtonVisible = true; // added by prashanth
            addCreditPlusInfo = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ADD_CREDITPLUS_IN_CARD_INFO", false);
            InitializeCommands();
            log.LogMethodExit();
        }
        public TransactionCardDetailsVM(ExecutionContext executionContext, AccountDTO accountDTO, bool addCreditPlusInfo, bool expand, bool isDeleteButtonVisible = true)
        {
            log.LogMethodEntry(accountDTO, accountDTO, addCreditPlusInfo, expand);
            this.expand = expand;
            this.accountDTO = accountDTO;
            this.ExecutionContext = executionContext;
            this.addCreditPlusInfo = addCreditPlusInfo;
            this.isDeleteButtonVisible = isDeleteButtonVisible;
            InitializeCommands();
            log.LogMethodExit();
        }
        #endregion
    }
}
